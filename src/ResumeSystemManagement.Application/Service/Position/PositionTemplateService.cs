using FluentResults;
using ResumeSystemManagement.Application.DTOs.PositionTemplate;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Application.Interfaces.Position;
using ResumeSystemManagement.Core.Enums;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;

namespace ResumeSystemManagement.Application.Service.Position;

public class PositionTemplateService(IPositionTemplateRepository repository, IPositionService positionService, IAttributeLibraryService attributeLibraryService): IPositionTemplateService
{
    private readonly IPositionTemplateRepository _repository = repository;
    private readonly IPositionService _positionService = positionService;
    private readonly IAttributeLibraryService _attributeLibraryService = attributeLibraryService;
    public async Task<Result<PositionTemplateDTo>> GetById(int id)
    {
        var positionTemplate = await _repository.GetByIdAsync(id);
        if (positionTemplate is null) return Result.Fail<PositionTemplateDTo>("Position not found");
        return positionTemplate.MapToPositionTemplate();
    }

    public Task<Result<HashSet<int>>> GetAttributeInPosition(int positionId)
    {
        return Result.Try(() => _repository.GetAttributeInPosition(positionId));
    }

    public async Task<Result> AssignAttributeAsync(int positionId, int attributeId, TemplateSection section)
    {
        var positionExisting = await _positionService.GetPosition(positionId);
        if (positionExisting.IsFailed) return Result.Fail(positionExisting.Errors);
        var attributeExisting = await _attributeLibraryService.GetAttributeLibrary(attributeId);
        if (attributeExisting.IsFailed) return Result.Fail(attributeExisting.Errors);
        return await Result.Try(() => _repository.AttachAttributeAsync(positionId, attributeId, section));
    }

    public Task<Result> UpdateSectionAsync(int positionId, int attributeId, TemplateSection section)
    {
        return Result.Try(() => _repository.Update(positionId, attributeId, section));
    }

    public Task<Result> DeleteAttributeAsync(int positionId, int attributeId)
    {
        return Result.Try(() => _repository.Delete(positionId, attributeId));
    }
}