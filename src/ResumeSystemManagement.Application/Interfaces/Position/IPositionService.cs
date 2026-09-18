using FluentResults;
using ResumeSystemManagement.Application.DTOs.Position;

namespace ResumeSystemManagement.Application.Interfaces.Position;

public interface IPositionService
{
    Task<Result<PositionDetail>>  GetPosition(int id);
    Task<Result<EditPositionDto>> GetEditPosition(int id);
    Task<Result<PositionDetails>> GetAllPositions(int pageIndex, int pageSize);
    Task<Result<List<PositionDetail>>>  GetAllPositionsByName(string name);
    Task<Result> CreatePosition(CreatePositionDto dto);
    Task<Result> DuplicatePosition(int id);
    Task<Result> EditPosition(EditPositionDto dto);
    Task<Result> DeletePosition(List<int> ids);
}