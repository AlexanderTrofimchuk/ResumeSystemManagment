using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;

public interface IPositionRepository
{
    Task<Position?> GetByIdAsync(int id);
    Task<List<Position>> GetAllByNameAsync(string name);  
    Task<List<Position>> GetAllAsync(int pageNumber, int pageSize);
    Task AddAsync(Position position);
    Task UpdateAsync(Position position);
    Task BulkDeleteAsync(List<int> ids);
}