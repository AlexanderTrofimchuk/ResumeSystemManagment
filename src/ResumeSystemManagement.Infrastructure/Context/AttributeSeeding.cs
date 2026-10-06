using Microsoft.EntityFrameworkCore;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Infrastructure.Context;

public static class AttributeSeeding
{
    public static void SeedAttributes(this DbContext context)
    {   
        context.SeedAttributeTypes();
        context.SeedAttributeCategories();
        context.SeedBuildInAttribute();
    }
    
    public static async Task SeedAttributesAsync(this DbContext context, CancellationToken cancellationToken = default)
    {   
        await context.SeedAttributeTypesAsync(cancellationToken);
        await context.SeedAttributeCategoriesAsync(cancellationToken);
        await context.SeedBuildInAttributeAsync(cancellationToken);
    }

    private static void SeedAttributeTypes(this DbContext context)
    {
        string[] types = [nameof(String),"Text","Image","Numeric","Date","Period","Boolean","One of many"];
        var existingTypes = context.Set<AttributeType>()
            .Select(attribute => attribute.Title)
            .ToList();
        foreach (var type in types.Where(type => !existingTypes.Contains(type)))
            context.Set<AttributeType>().Add(new AttributeType(type));
        context.SaveChanges();
    }
    
    private static async Task SeedAttributeTypesAsync(this DbContext context,CancellationToken  cancellationToken = default)
    {
        string[] types = ["String","Text","Image","Numeric","Date","Period","Boolean","One of many"];
        var existingTypes = await context.Set<AttributeType>()
            .Select(attribute => attribute.Title)
            .ToListAsync(cancellationToken);
        foreach (var type in types.Where(type => !existingTypes.Contains(type)))
                await context.Set<AttributeType>().AddAsync(new AttributeType(type),cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static void SeedAttributeCategories(this DbContext context)
    {
        string[] categories = ["Certification","Domain Knowledge",$"Personal Information","Soft Skills"];
        var existingCategories = context.Set<AttributeCategory>()
            .Select(attribute => attribute.Title)
            .ToList();
        foreach (var category in categories.Where(category => !existingCategories.Contains(category)))
            context.Set<AttributeCategory>().Add(new AttributeCategory(category));
        context.SaveChanges();
    }
    
    private static async Task SeedAttributeCategoriesAsync(this DbContext context,CancellationToken cancellationToken = default)
    {
        string[] categories = ["Certification","Domain Knowledge","Personal Information","Soft Skills"];
        foreach (var category in categories)
        {
            if (!await context.Set<AttributeCategory>().AnyAsync(at => at.Title == category,cancellationToken))
                await context.Set<AttributeCategory>().AddAsync(new AttributeCategory(category),cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private static void SeedBuildInAttribute(this DbContext context)
    {
        var stringId = context.Set<AttributeType>().FirstOrDefault(at => at.Title == "String")!.Id;
        var imageId = context.Set<AttributeType>().FirstOrDefault(at => at.Title == "Image")!.Id;
        var personalInformation = context.Set<AttributeCategory>().FirstOrDefault(ac => ac.Title == "Personal Information")!.Id;

        AttributeLibrary[] buildInAttribute = [
            Create("First Name", stringId, personalInformation),
            Create("Last Name", stringId, personalInformation),
            Create("Location", stringId, personalInformation),
            Create("Personal Photo", imageId, personalInformation),
            Create("Email", stringId, personalInformation),
            Create("Phone", stringId, personalInformation)
        ];

        var existingTitles = context.Set<AttributeLibrary>()
            .Select(attribute => attribute.Title)
            .ToList();
        foreach (var attribute in buildInAttribute.Where(attribute => !existingTitles.Contains(attribute.Title)))
            context.Set<AttributeLibrary>().Add(attribute);
        context.SaveChanges();
    }
    
    private static async Task SeedBuildInAttributeAsync(this DbContext context, CancellationToken cancellationToken = default)
    {
        var stringId = await context.Set<AttributeType>().FirstOrDefaultAsync(at => at.Title == "String",cancellationToken);
        var imageId = await context.Set<AttributeType>().FirstOrDefaultAsync(at => at.Title == "Image",cancellationToken);
        var personalInformation = await context.Set<AttributeCategory>()
            .FirstOrDefaultAsync(ac => ac.Title == "Personal Information",cancellationToken);

        AttributeLibrary[] buildInAttribute =
        [
            Create("First Name", stringId!.Id, personalInformation!.Id),
            Create("Last Name", stringId.Id, personalInformation.Id),
            Create("Location", stringId.Id, personalInformation.Id),
            Create("Personal Photo", imageId!.Id, personalInformation.Id),
            Create("Email", stringId.Id, personalInformation.Id),
            Create("Phone", stringId.Id, personalInformation.Id)
        ];
        
        var existingTitles = await context.Set<AttributeLibrary>()
            .Select(attribute => attribute.Title)
            .ToListAsync(cancellationToken);
        
        foreach (var attribute in buildInAttribute.Where(attribute => !existingTitles.Contains(attribute.Title)))
            await context.Set<AttributeLibrary>().AddAsync(attribute, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static AttributeLibrary Create(string title, int typeId, int categoryId)
    {
        return new(
            title,
            "Basic candidate information",
            categoryId,
            typeId,
            true
        );
    }
}