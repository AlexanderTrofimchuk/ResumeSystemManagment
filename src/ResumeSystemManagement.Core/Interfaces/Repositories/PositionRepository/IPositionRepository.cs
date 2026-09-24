using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;

public interface IPositionRepository
{
    Task<Position?> GetByIdAsync(int id);
    Task<List<Position>> GetAllByNameAsync(string name);
    Task<int> GetCountAsync();
    int GetCountResume(int positionId);
    Task<List<Position>> GetLatestPositionAsync();
    Task<List<Position>> GetPopularPositionAsync();
    Task AssignBuildInAttributeAsync(int positionId,List<AttributeLibrary>  attributes);
    Task DuplicateAttributeAsync(int originId, int duplicateId);
    Task<List<Position>> GetAllAsync(int pageNumber, int pageSize);
    Task<int> AddAsync(Position position);
    Task UpdateAsync(Position position);
    Task BulkDeleteAsync(List<int> ids);
}