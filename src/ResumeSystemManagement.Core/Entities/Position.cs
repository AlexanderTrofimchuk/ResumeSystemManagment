using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Core.Entities;

public class Position
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public PositionPermissions Permissions { get; set; } = PositionPermissions.Public;
    public uint Version { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; } = null;

    public List<Resume> Resumes { get; } = new();
    public List<AttributeFilter> AttributeFilters { get; } = new();
    public List<AttributeLibrary> AttributeLibraries { get; } = new();
}