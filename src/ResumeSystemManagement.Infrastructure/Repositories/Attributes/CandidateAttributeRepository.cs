using Microsoft.EntityFrameworkCore;
using Npgsql;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Exceptions;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Infrastructure.Context;

namespace ResumeSystemManagement.Infrastructure.Repositories.Attributes;

public class CandidateAttributeRepository(ApplicationDbContext context, IAttributeLibraryRepository attributeLibraryRepository) : ICandidateAttributeRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAttributeLibraryRepository _attributeLibraryRepository = attributeLibraryRepository;

    public Task<List<CandidateAttributeValue>> GetAttributeValues(string userId)
    {
        return _context.CandidateAttributeValues
            .Include(a => a.Attribute)
            .ThenInclude(at => at.AttributeType)
            .Include(a => a.Attribute)
            .ThenInclude(al => al.AttributeValueForLists)
            .Where(cav => cav.UserId == userId && !cav.Attribute.IsBuiltIn)
            .ToListAsync();
    }

    public Task<List<CandidateAttributeValue>> GetBuildInAttribute(string userId)
    {
        return _context.CandidateAttributeValues
            .Include(a => a.Attribute)
            .ThenInclude(at => at.AttributeType)
            .Where(cav => cav.UserId == userId && cav.Attribute.IsBuiltIn)
            .ToListAsync();
    }

    private Task<string?> GetTitleAttribute(int attributeId)
    {
        return _context.AttributeLibraries.Where(a => a.Id == attributeId)
            .Select(a => a.Title)
            .FirstOrDefaultAsync();
    }
    
    public async Task InitialBuildInAttributes(string userId, string username)
    {
        var (initFullname, candidateAttribute) = await GetPopulateInitAttributes(userId, username);
        _context.CandidateAttributeValues.AddRange(initFullname);
        _context.CandidateAttributeValues.AddRange(candidateAttribute);
        await _context.SaveChangesAsync();
    }

    private async Task<(IEnumerable<CandidateAttributeValue> initFullname, IEnumerable<CandidateAttributeValue> candidateAttribute)> 
        GetPopulateInitAttributes(string userId, string username)
    {
        var buildInAttributes = await _attributeLibraryRepository.GetBuildInAttribute();
        var initFullname = buildInAttributes
            .Where(a => a.Title == "First Name" || a.Title == "Last Name")
            .Select(a => new CandidateAttributeValue(userId, a.Id, username.Split(' ')[a.Title == "First Name" ? 0 : 1]));
        var candidateAttribute = buildInAttributes
            .Where(a => a.Title != "First Name" && a.Title != "Last Name")
            .Select(a => new CandidateAttributeValue(userId, a.Id, string.Empty));
        return (initFullname, candidateAttribute);
    }

    public async Task<bool> CreateAttributeValue(CandidateAttributeValue candidateAttributeValue)
    {
        await _context.CandidateAttributeValues.AddAsync(candidateAttributeValue);
        try
        {
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException e)when 
            (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            var title = await GetTitleAttribute(candidateAttributeValue.AttributeId);
            throw new DuplicateRecordException(title ?? string.Empty);
        }
    }

    public Task UpdateAttributeValue(List<CandidateAttributeValue> candidateAttributes)
    {
        try
        {
            _context.CandidateAttributeValues.UpdateRange(candidateAttributes);
            return _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new OperationCanceledException("The candidate attribute values were modified by another user. Please reload the data and try again.");
        }
    }

    public async Task UpdateInfoAttributeValues(List<CandidateAttributeValue> candidateAttributes, string userId)
    {
        var ids = candidateAttributes.Select(value => value.Id).Distinct().ToList();
        if (ids.Count != candidateAttributes.Count)
            throw new InvalidOperationException("Duplicate profile attribute values were submitted.");

        var existingValues = await _context.CandidateAttributeValues
            .Include(value => value.Attribute)
            .Where(value =>
                ids.Contains(value.Id) &&
                value.UserId == userId &&
                !value.Attribute.IsBuiltIn)
            .ToListAsync();

        if (existingValues.Count != candidateAttributes.Count)
            throw new InvalidOperationException("One or more profile attributes could not be updated.");

        var valuesById = candidateAttributes.ToDictionary(value => value.Id, value => value.Value);
        foreach (var existingValue in existingValues)
            existingValue.UpdateValue(valuesById[existingValue.Id]);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAttributeValues(List<int> ids, string userId)
    {
        var uniqueIds = ids.Distinct().ToList();
        if (uniqueIds.Count != ids.Count)
            return false;

        var attributeValues = await _context.CandidateAttributeValues
            .Include(value => value.Attribute)
            .Where(value =>
                uniqueIds.Contains(value.Id) &&
                value.UserId == userId &&
                !value.Attribute.IsBuiltIn)
            .ToListAsync();

        if (attributeValues.Count != uniqueIds.Count)
            return false;

        _context.CandidateAttributeValues.RemoveRange(attributeValues);
        await _context.SaveChangesAsync();
        return true;
    }
}