using Microsoft.EntityFrameworkCore;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Infrastructure.Context;

namespace ResumeSystemManagement.Infrastructure.Repositories.Attributes;

public class CandidateAttributeRepository(ApplicationDbContext context, IAttributeLibraryRepository attributeLibraryRepository) : ICandidateAttributeRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAttributeLibraryRepository _attributeLibraryRepository = attributeLibraryRepository;

    public Task<List<CandidateAttributeValue>> GetCandidateAttributeValues(string userId)
    {
        return _context.CandidateAttributeValues
            .Include(a => a.Attribute)
            .ThenInclude(at => at.AttributeType)
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

    public Task CreateCandidateAttributeValue(CandidateAttributeValue candidateAttributeValue)
    {
        _context.CandidateAttributeValues.AddAsync(candidateAttributeValue);
        return _context.SaveChangesAsync();
    }

    public Task UpdateCandidateAttributeValue(List<CandidateAttributeValue> candidateAttributes)
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
}