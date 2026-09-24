using FluentResults;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Exceptions;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Infrastructure.Context;
using ResumeSystemManagement.Infrastructure.Mappers;

namespace ResumeSystemManagement.Infrastructure.Repositories.Attributes;

public class AttributeLibraryRepository(ApplicationDbContext context) : IAttributeLibraryRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<AttributeLibrary?> GetAttributeByIdAsync(int id)
    {
        return _context.AttributeLibraries
            .Include(av => av.AttributeValueForLists)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public Task<List<AttributeLibrary>> GetByCategoryAsync(int categoryId)
    {
        return _context.AttributeLibraries.Where(a => a.CategoryId == categoryId).ToListAsync();
    }

    public Task<List<AttributeDetail>> GetByNameAsync(string name)
    {
        return _context.AttributeLibraries.AsNoTracking()
            .Include(t => t.AttributeType)
            .Include(c => c.AttributeCategory)
            .Where(a => a.Title.ToLower() == name.ToLower())
            .Select(a => a.ToAttributeDetail())
            .ToListAsync();
    }

    public Task<List<AttributeDetail>> GetAttributesAsync(int pageSize, int page)
    {
        return _context.AttributeLibraries
            .AsNoTracking()
            .Include(t => t.AttributeType)
            .Include(c => c.AttributeCategory)
            .Where(a => !a.IsBuiltIn)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(a => a.ToAttributeDetail())
            .ToListAsync();
    }

    public Task<List<AttributeLibrary>> GetBuildInAttribute()
    {
        return _context.AttributeLibraries.Where(a => a.IsBuiltIn)
            .ToListAsync();
    }

    public async Task<int> CreateAttributeAsync(AttributeLibrary attribute)
    {
        var existing = await _context.AttributeLibraries.FirstOrDefaultAsync(al => al.Title.ToLower() == attribute.Title.ToLower());
        if (existing is null) throw new  ArgumentException("attribute title already exists");
        try
        {
            await _context.AttributeLibraries.AddAsync(attribute); 
            await _context.SaveChangesAsync();
            return attribute.Id;
        }
        catch (DbUpdateException e)when 
            (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new DuplicateRecordException(attribute.Title);
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine("Unexpected database error while creating attribute values. \n Error {0}", e);
            throw new DatabaseException("Failed to save attribute values. Please try again.");
        }
    }

    public async Task<bool> CreateAttributeListValue(List<AttributeValueForList> variations)
    {
        try {
            await _context.AttributeValueForLists.AddRangeAsync(variations);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException e)when 
            (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            var existingTitles = await GetExistingTitles(variations);
            throw new DuplicateRecordException(string.Join(",",existingTitles));
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine("Unexpected database error while creating attribute values. \n Error {0}", e);
            throw new DatabaseException("Failed to save attribute values. Please try again.");
        }
    }

    private async Task<List<string>> GetExistingTitles(List<AttributeValueForList> variations)
    {
        var existingTitles = await _context.AttributeValueForLists
            .Where(x => variations
                .Select(v => v.AttributeId)
                .Contains(x.AttributeId))
            .Select(x => x.Value)
            .ToListAsync();
        return existingTitles;
    }

    public async Task<bool> UpdateAttributeAsync(AttributeLibrary attribute)
    {
        var existing = await GetAttributeByIdAsync(attribute.Id);
        if (existing is null) return false;
        SyncOptions(existing, attribute);
        UpdateFields(existing, attribute);
        try
        {
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InvalidOperationException("Position was modified by another user. Please reload and try again.");
        }
    }
    
    private void SyncOptions(AttributeLibrary existing, AttributeLibrary attribute)
    {
        if (existing.TypeId != attribute.TypeId)
            _context.AttributeValueForLists
                .RemoveRange(existing.AttributeValueForLists);

        RemoveDeletedOptions(existing, attribute);
        AddNewOptions(existing, attribute);
    }
    
    private static void UpdateFields(AttributeLibrary existing, AttributeLibrary attribute)
    {
        existing.SetType(attribute.TypeId);
        existing.SetCategoryId(attribute.CategoryId);
        existing.SetTitle(attribute.Title);
        existing.SetDescription(attribute.Description);
    }

    private void RemoveDeletedOptions(AttributeLibrary existing, AttributeLibrary attribute)
    {
        var incoming = attribute.AttributeValueForLists
            .Where(a => a.Id != 0).Select(i => i.Id).ToHashSet();

        var toDelete = existing.AttributeValueForLists
            .Where(ex => !incoming.Contains(ex.Id));

        _context.AttributeValueForLists.RemoveRange(toDelete);
    }

    private static void AddNewOptions(AttributeLibrary existing, AttributeLibrary attribute)
    {
        var toAdd = attribute.AttributeValueForLists.Where(a => a.Id == 0);
        existing.AttributeValueForLists.AddRange(toAdd);
    }
    

    public async Task<bool> DeleteAttributeAsync(AttributeLibrary attribute)
    {
        _context.AttributeLibraries.Remove(attribute);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Result<bool>> BulkDeleteAttributesAsync(List<int> ids)
    {
        try
        {
            var usingAttributes = await AttributeUsingPosition(ids);
            if (usingAttributes.Count > 0) 
                return Result.Fail($"This {string.Join(",",usingAttributes)} attributes were used in positions.");
            var length = await _context.AttributeLibraries
                .Where(a => ids.Contains(a.Id))
                .ExecuteDeleteAsync();
            return length > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private Task<List<string>> AttributeUsingPosition(List<int> ids)
    {
        return _context.PositionAttributeLibraries.AsNoTracking()
            .Where(pal => ids.Contains(pal.AttributeLibraryId))
            .Select(a => a.AttributeLibrary.Title)
            .ToListAsync();
    }
}