using Microsoft.EntityFrameworkCore;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;
using ResumeSystemManagement.Infrastructure.Context;

namespace ResumeSystemManagement.Infrastructure.Repositories.Positions;

public class PositionRepository(ApplicationDbContext context) : IPositionRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<Position?> GetByIdAsync(int id)
    {
        return _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<Position>> GetAllByNameAsync(string name)
    {
        return _context.Positions.Where(p => p.Title == name).ToListAsync();
    }

    public Task<List<Position>> GetAllAsync(int pageNumber, int pageSize)
    {
        return _context.Positions.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task AddAsync(Position position)
    {
        _context.Positions.AddAsync(position);
        return _context.SaveChangesAsync();
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