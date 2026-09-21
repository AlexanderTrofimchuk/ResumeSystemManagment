using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attribute;
using ResumeSystemManagement.Application.DTOs.Attribute.Mapper;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

namespace ResumeSystemManagement.Application.Service.Attributes;

public class AttributeLibraryService (IAttributeLibraryRepository repository) : IAttributeLibraryService
{
    public async Task<Result<AttributeDetails>> GetAttributesAsync(int pageSize, int page)
    {
        var resultTask = Result.Try(() => repository.GetAttributesAsync(pageSize, page));
        var attributes = await resultTask;
        return Result.Ok(new AttributeDetails { Attributes = attributes.Value});
    }

    public async Task<Result<AttributeLibrary>> GetAttributeLibrary(int attributeId)
    {
        var attribute = await repository.GetAttributeByIdAsync(attributeId);
        if (attribute is null) return Result.Fail("Attribute not found");
        return attribute;
    }

    public async Task<Result<EditAttributeDTo>> GetEditAttributeAsync(int id)
    {
        var attribute = await GetAttributeLibrary(id);
        if (attribute.IsFailed) return Result.Fail(attribute.Errors);
        return Result.Ok(attribute.Value.ToEditAttributeDTo());
    }

    public async Task<Result<AttributeDetails>> GetAttributesByNameAsync(string name)
    {
        var resultTask = Result.Try(() => repository.GetByNameAsync(name));
        var attributes = await resultTask;
        return Result.Ok(new AttributeDetails { Attributes = attributes.Value });
    }

    public async Task<Result<int>> CreateAttributeAsync(CreateAttributeDto dto)
    {
        var createTask = Result.Try(() => repository.CreateAttributeAsync(dto.ToAttributeLibrary()));
        return await createTask;
    }

    public async Task<Result<bool>> AddDropDownOptions(int id, CreateAttributeDto dto)
    {
        if (dto.DropDownOptions!.Count == 0) return Result.Fail<bool>("Not found options");
        var addTask = Result.Try(() => repository.CreateAttributeListValue(dto.ToValueForList(id)));
        return await addTask;
    }

    public async Task<Result> EditAttributeAsync(EditAttributeDTo dTo)
    {
        var attribute = dTo.ToAttributeLibrary();
        var updateTask = Result.Try(() => repository.UpdateAttributeAsync(attribute));
        var result = await updateTask;
        if (result.IsFailed) return Result.Fail(result.Errors.Select(e => e.Message));
        return  Result.Ok();
    }

    public async Task<Result> DeleteAttributeAsync(int id)
    {
        var attribute = await repository.GetAttributeByIdAsync(id);
        if (attribute is null) return Result.Fail("Attribute not found");
        return await repository.DeleteAttributeAsync(attribute) ? Result.Ok() : Result.Fail("Something wrong. Please try again");
    }

    public async Task<Result> BulkDeleteAttributesAsync(List<int>? ids)
    {
        if (ids is null || ids.Count == 0) return Result.Fail("You must provide at least one id");
        var deleteTask = Result.Try(() => repository.BulkDeleteAttributeAsync(ids));
        var result = await deleteTask;
        if (result.IsFailed) return Result.Fail(result.Errors.Select(e => e.Message));
        return !result.Value ? Result.Fail("Something wrong. Please try again") : Result.Ok();
    }
}