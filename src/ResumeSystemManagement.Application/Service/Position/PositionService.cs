using FluentResults;
using ResumeSystemManagement.Application.DTOs.Position;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Application.Interfaces.Position;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Enums;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;

namespace ResumeSystemManagement.Application.Service.Position;

public class PositionService(IPositionRepository repository,IUserContext userContext, IAttributeLibraryRepository libraryRepository): IPositionService
{
    private readonly IUserContext _userContext = userContext;
    private readonly IPositionRepository _repository = repository;
    private readonly IAttributeLibraryRepository  _libraryRepository = libraryRepository;

    public async Task<Result<PositionDetailForUser>> GetPositionDetailForUser(int id)
    {
        var position = await GetPosition(id);
        if (position.IsFailed) return Result.Fail<PositionDetailForUser>(position.Errors);
        return position.Value.MapToPositionForUser();
    }

    public async Task<Result<EditPositionDto>> GetEditPosition(int id)
    {
        var position = await GetPosition(id);
        if  (position.IsFailed) return Result.Fail<EditPositionDto>(position.Errors);
        return Result.Ok(position.Value.MapToEditPosition());
    }
    
    public async Task<Result<PositionDetail>> GetPositionDetail(int id)
    {
        var position = await GetPosition(id);
        if (position.IsFailed) return Result.Fail<PositionDetail>(position.Errors);
        return Result.Ok(position.Value.ToPositionDetail());
    }

    private async Task<Result<Core.Entities.Position>> GetPosition(int id)
    {
        var position = await _repository.GetByIdAsync(id);
        if (position == null) return Result.Fail("Position not found");
        return position;
    }

    public async Task<Result<PositionDetails>> GetAllPositions(int pageIndex, int pageSize)
    {
        var positions = await _repository.GetAllAsync(pageIndex, pageSize);
        return Result.Ok(
            new PositionDetails { Positions = positions.Select(p => p.ToPositionDetail()).ToList() });
    }

    public async Task<Result<PositionDetails>> GetAllPositionsByName(string name)
    {
        var position = await _repository.GetAllByNameAsync(name);
        return Result.Ok(new PositionDetails {
            Positions = position.Select(p => p.ToPositionDetail()).ToList()
        });
    }

    public async Task<Result> PublishPosition(int positionId)
    {
        return await ChangePublishStatus(positionId,PublishStatus.Publish);
    }

    public async Task<Result> DraftPosition(int positionId)
    {
        return await ChangePublishStatus(positionId,PublishStatus.Draft);
    }

    private async Task<Result> ChangePublishStatus(int positionId,PublishStatus status)
    {
        var position = await GetPosition(positionId);
        if (position.IsFailed) return Result.Fail(position.Errors);
        position.Value.SetPublishStatus(PublishStatus.Publish); 
        return await Result.Try(() => _repository.UpdateAsync(position.Value));
    }

    public async Task<Result<int>> CreatePosition(CreatePositionDto dto)
    {
        var position = dto.MapToPosition(_userContext.UserId.ToString());
        var resultAdd = await Result.Try(() => _repository.AddAsync(position));
        if (resultAdd.IsFailed) return resultAdd;
        var buildAttribute = await _libraryRepository.GetBuildInAttribute();
        var assignResult = await Result.Try(() => _repository.AssignBuildInAttributeAsync(position.Id, buildAttribute));
        return assignResult.IsFailed ? Result.Fail(assignResult.Errors) : Result.Ok(resultAdd.Value);
    }

    
    public async Task<Result> DuplicatePosition(int id)
    {
        var position = await _repository.GetByIdAsync(id);
        if (position is null) return Result.Fail("Position not found");
        var duplicateResult = await CreatDuplicatePosition(position);
        if (duplicateResult.IsFailed) return duplicateResult.ToResult();
        return await Result.Try(() => _repository.DuplicateAttributeAsync(position.Id,duplicateResult.Value));
    }

    private async Task<Result<int>> CreatDuplicatePosition(Core.Entities.Position position)
    {
        var duplicate = CreateDuplicatePosition(position);
        var duplicateResult = await Result.Try(() => _repository.AddAsync(duplicate));
        if (duplicateResult.IsFailed) return Result.Fail("Duplicate Position don't duplicate");
        return duplicateResult.Value;
    }

    public Task<Result> EditPosition(EditPositionDto dto)
    {
        var position = dto.MapToPosition();
        return Result.Try(() => _repository.UpdateAsync(position));
    }

    public Task<Result> DeletePosition(List<int> ids)
    {
        return Result.Try(() => _repository.BulkDeleteAsync(ids));
    }

    private Core.Entities.Position  CreateDuplicatePosition(Core.Entities.Position position)
    {
        var duplicatePosition = new Core.Entities.Position ( 
            position.Title + "[Duplicate]",
            position.ShortDescription,
            _userContext.UserId.ToString(),
            permissions:position.Permissions);
        
        duplicatePosition.SetMaxProjects(position.MaxProjects);
        duplicatePosition.SetLevel(position.Level);
        return duplicatePosition;
    }
}