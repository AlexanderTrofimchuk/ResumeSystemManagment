using Microsoft.EntityFrameworkCore;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Enums;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Infrastructure.Context;

namespace ResumeSystemManagement.Infrastructure.Repositories.Positions;

public class PositionRepository(ApplicationDbContext context) : IPositionRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<Position?> GetByIdAsync(int id)
    {
        return _context.Positions
            .Include(pl => pl.PositionAttributeLibraries)
            .ThenInclude(a => a.AttributeLibrary)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<Position>> GetAllByNameAsync(string name)
    {
        return _context.Positions
            .Where(p => EF.Functions.ILike(p.Title,$"{name}%"))
            .ToListAsync();
    }

    public Task<int> GetCountAsync()
    {
        return _context.Positions.CountAsync(
            p => p.PublishStatus == PublishStatus.Publish && p.Permissions == PositionPermissions.Public);
    }

    public int GetCountResume(int positionId)
    {
        return _context.Resumes.Count(r => r.PositionId == positionId);
    }

    public Task<List<Position>> GetLatestPositionAsync()
    {
        return _context.Positions
            .Where(p => p.PublishStatus == PublishStatus.Publish && p.Permissions == PositionPermissions.Public)
            .OrderByDescending(p => p.CreatedBy)
            .Take(PaginationConstants.MaxPositionOnMain)
            .ToListAsync();
    }

    public Task<List<Position>> GetPopularPositionAsync()
    {
        return _context.Positions
            .Where(p => p.PublishStatus == PublishStatus.Publish && p.Permissions == PositionPermissions.Public)
            .OrderByDescending(p => p.Resumes.Count(r => r.Status == PublishStatus.Publish))
            .Take(PaginationConstants.MaxPopularPosition)
            .ToListAsync();
    }

    public async Task AssignBuildInAttributeAsync(int positionId, List<AttributeLibrary> attributes)
    {
        var position = await GetByIdAsync(positionId);
        if (position == null) throw new InvalidOperationException("Position not found.");
        var listBuildIn = attributes.Select(a => new PositionAttributeLibrary
        {
            Position = position,
            AttributeLibrary = a,
            Section = TemplateSection.Personal
        });
        position.PositionAttributeLibraries.AddRange(listBuildIn);
        await _context.SaveChangesAsync();
    }

    public async Task DuplicateAttributeAsync(int originId, int duplicateId)
    {
        var originAttribute = await _context.PositionAttributeLibraries
            .Where(pa => pa.PositionId == originId).ToListAsync();

        var dublicateAttribute = originAttribute
            .Select(oa => new PositionAttributeLibrary
                {PositionId = duplicateId,AttributeLibraryId = oa.AttributeLibraryId});
        
        _context.PositionAttributeLibraries.AddRange(dublicateAttribute);
        await _context.SaveChangesAsync();
    }

    public Task<List<Position>> GetAllAsync(int pageNumber, int pageSize)
    {
        return _context.Positions.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> AddAsync(Position position)
    {
        await _context.Positions.AddAsync(position);
        await _context.SaveChangesAsync();
        return position.Id;
    }

    public Task UpdateAsync(Position position)
    {
        try
        {
            _context.Positions.Update(position);
            return _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InvalidOperationException("Position was modified by another user. Please reload and try again.");
        }
    }

    public async Task BulkDeleteAsync(List<int> ids)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var isUsed = await _context.Resumes.AnyAsync(p => ids.Contains(p.PositionId));
        if (isUsed) throw new InvalidOperationException("Cannot delete positions that have already been used.");

        await _context.Positions.Where(p => ids.Contains(p.Id)).ExecuteDeleteAsync(); 
        
        await transaction.CommitAsync();
    }
}