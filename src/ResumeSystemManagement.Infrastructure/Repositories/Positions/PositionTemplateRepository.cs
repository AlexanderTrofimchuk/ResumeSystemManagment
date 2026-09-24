using Microsoft.EntityFrameworkCore;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Enums;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;
using ResumeSystemManagement.Infrastructure.Context;

namespace ResumeSystemManagement.Infrastructure.Repositories.Positions;

public class PositionTemplateRepository(ApplicationDbContext context): IPositionTemplateRepository
{
    private readonly ApplicationDbContext _context = context;
    
    public Task<Position?> GetByIdAsync(int id)
    {
        return _context.Positions.AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.PositionAttributeLibraries)
            .ThenInclude(a => a.AttributeLibrary)
            .ThenInclude(t => t.AttributeType)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<HashSet<int>> GetAttributeInPosition(int positionId)
    {
        return _context.PositionAttributeLibraries.Where(pa => pa.PositionId == positionId)
            .Select(pa => pa.AttributeLibraryId).ToHashSetAsync();
    }

    public Task AttachAttributeAsync(int positionId, int attributeId, TemplateSection section)
    {
        var positionAttribute = CreatePositionAttribute(positionId, attributeId, section);
        _context.PositionAttributeLibraries.AddAsync(positionAttribute);
        return _context.SaveChangesAsync();
    }

    public Task Update(int positionId, int attributeId, TemplateSection section)
    {
        var existing = _context.AttributeLibraries.FirstOrDefault(pa => pa.Id == attributeId);
        if  (existing is null) throw new  ArgumentException("attribute not exists");
        if (existing.IsBuiltIn) throw new  ArgumentException("This attribute dont move. Because it is built-in");
        var  positionAttribute = CreatePositionAttribute(positionId, attributeId, section);
        _context.PositionAttributeLibraries.Update(positionAttribute);
        return _context.SaveChangesAsync();
    }

    public Task Delete(int positionId, int attributeId)
    {
        return _context.PositionAttributeLibraries
            .Where(pa => pa.AttributeLibraryId == attributeId && pa.PositionId == positionId)
            .ExecuteDeleteAsync();
    }

    private static PositionAttributeLibrary  CreatePositionAttribute(int positionId, int attributeId, TemplateSection section) =>
        new()
        {
            PositionId = positionId,
            AttributeLibraryId = attributeId,
            Section = section
        };
}