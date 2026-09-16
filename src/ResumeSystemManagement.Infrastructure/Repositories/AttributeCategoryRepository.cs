using Microsoft.EntityFrameworkCore;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Infrastructure.Context;

namespace ResumeSystemManagement.Infrastructure.Repositories;

public class AttributeCategoryRepository(ApplicationDbContext context) : IAttributeCategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<List<AttributeCategory>> GetAttributeCategories()
    {
        return _context.AttributeCategories.ToListAsync();
    }
}