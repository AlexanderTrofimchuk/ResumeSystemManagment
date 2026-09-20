using FluentResults;
using ResumeSystemManagement.Application.DTOs.Position;
using ResumeSystemManagement.Application.DTOs.PositionTemplate;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Application.Interfaces.Position;
using ResumeSystemManagement.Core.Enums;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;

namespace ResumeSystemManagement.Application.Service.Position;

public class PositionService(IPositionRepository repository, IAttributeLibraryService libraryService,IUserContext userContext): IPositionService
{
    
    public async Task<Result<EditPositionDto>> GetEditPosition(int id)
    {
        var position = await repository.GetByIdAsync(id);
        if (position == null) return Result.Fail("Position not found");
        return Result.Ok(position.MapToEditPosition());
    }
    
    public async Task<Result<PositionDetail>> GetPosition(int id)
    {
        var position = await repository.GetByIdAsync(id);
        if (position == null) return Result.Fail("Position not found");
        return Result.Ok(position.ToPositionDetail());
    }

    public async Task<Result<PositionDetails>> GetAllPositions(int pageIndex, int pageSize)
    {
        var positions = await repository.GetAllAsync(pageIndex, pageSize);
        return Result.Ok(
            new PositionDetails { Positions = positions.Select(p => p.ToPositionDetail()).ToList() });
    }

    public async Task<Result<PositionDetails>> GetAllPositionsByName(string name)
    {
        var position = await repository.GetAllByNameAsync(name);
        return Result.Ok(new PositionDetails {
            Positions = position.Select(p => p.ToPositionDetail()).ToList()
        });
    }

    public Task<Result<int>> CreatePosition(CreatePositionDto dto)
    {
        var position = dto.MapToPosition(userContext.UserId.ToString());
        return Result.Try(() => repository.AddAsync(position));
    }

    

    public async Task<Result> DuplicatePosition(int id)
    {
        var position = await GetPosition(id);
        if (position.IsFailed) return Result.Fail(position.Errors);
        var duplicate = CreateDuplicatePosition(position.Value.ToPosition());
        var duplicateResult = await Result.Try(() => repository.AddAsync(duplicate));
        return duplicateResult.IsFailed ? Result.Fail(duplicateResult.Errors) : Result.Ok();
    }

    public Task<Result> EditPosition(EditPositionDto dto)
    {
        var position = dto.MapToPosition();
        return Result.Try(() => repository.UpdateAsync(position));
    }

    public Task<Result> DeletePosition(List<int> ids)
    {
        return Result.Try(() => repository.BulkDeleteAsync(ids));
    }

    private Core.Entities.Position  CreateDuplicatePosition(Core.Entities.Position position)
    {
        return new()
        {
            Id = 0,
            Title = position.Title + "[Duplicate]",
            Description = position.Description,
            CreatedBy = userContext.UserId.ToString(),
        };
    }
}