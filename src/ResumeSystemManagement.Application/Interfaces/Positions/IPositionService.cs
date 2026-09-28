using FluentResults;
using ResumeSystemManagement.Application.DTOs.Positions;

namespace ResumeSystemManagement.Application.Interfaces.Positions;

public interface IPositionService
{
    Task<Result<PositionDetail>>  GetPositionDetail(int id);
    Task<Result<PositionDetailForUser>>   GetPositionDetailForUser(int id);
    Task<Result<EditPositionDto>> GetEditPosition(int id);
    Task<Result<PositionDetails>> GetAllPositions(int page);
    Task<Result<PositionDetails>> GetAllPositionsByName(string name, int page);
    Task<Result> PublishPosition(int positionId);
    Task<Result> DraftPosition(int positionId);
    Task<Result<int>> CreatePosition(CreatePositionDto dto);
    Task<Result> DuplicatePosition(int id);
    Task<Result> EditPosition(EditPositionDto dto);
    Task<Result> DeletePosition(List<int> ids);
}