using Microsoft.EntityFrameworkCore;
using ResumeSystemManagement.Core.Entities;

using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Infrastructure.Context;

namespace ResumeSystemManagement.Infrastructure.Repositories;

public class AttributeTypeRepository(ApplicationDbContext context) : IAttributeTypeRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<List<AttributeType>> GetAttributeTypes()
    {
        return _context.AttributeTypes.ToListAsync();
    }
}