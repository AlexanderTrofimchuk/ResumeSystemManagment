using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;

public interface IPositionRepository
{
    Task<Position?> GetByIdAsync(int id);
    Task<List<Position>> GetAllByNameAsync(string name, int pageNumber, bool includeUnpublished);
    Task<int> GetCountByNameAsync(string name, bool includeUnpublished);
    Task<int> GetCountAsync();
    Task<int> GetPublishCountAsync();
    Task<List<Position>> GetPublishPositionAsync(int pageNumber);
    Task<List<Position>> GetAllAsync(int pageNumber);
    int GetCountResume(int positionId);
    Task<List<Position>> GetLatestPositionAsync();
    Task<List<Position>> GetPopularPositionAsync();
    Task AssignBuildInAttributeAsync(int positionId,List<AttributeLibrary>  attributes);
    Task DuplicateAttributeAsync(int originId, int duplicateId);
    Task<int> AddAsync(Position position);
    Task UpdateAsync(Position position);
    Task BulkDeleteAsync(List<int> ids);
}