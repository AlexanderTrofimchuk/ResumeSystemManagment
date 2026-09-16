using Microsoft.EntityFrameworkCore;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Infrastructure.Context;
using ResumeSystemManagement.Infrastructure.Mappers;

namespace ResumeSystemManagement.Infrastructure.Repositories;

public class AttributeLibraryRepository(ApplicationDbContext context) : IAttributeLibraryRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<AttributeLibrary?> GetAttributeByIdAsync(int id)
    {
        return _context.AttributeLibraries.FirstOrDefaultAsync(a => a.Id == id);
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
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(a => a.ToAttributeDetail())
            .ToListAsync();
    }

    public async Task<bool> CreateAttributeAsync(AttributeLibrary attribute)
    {
        try
        {
            await _context.AttributeLibraries.AddAsync(attribute);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> CreateAttributeListValue(List<AttributeValueForList> variations)
    {
        foreach (var variation in variations) {
            await _context.AttributeValueForLists.AddAsync(variation); }
        
        try {
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> UpdateAttributeAsync(AttributeLibrary attribute)
    {
        try
        {
            _context.AttributeLibraries.Update(attribute);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteAttributeAsync(AttributeLibrary attribute)
    {
        _context.AttributeLibraries.Remove(attribute);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> BulkDeleteAttributeAsync(List<int> ids)
    {
        try
        {
           var length = await _context.AttributeLibraries.Where(a => ids.Contains(a.Id))
                .ExecuteDeleteAsync();
           return length > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}