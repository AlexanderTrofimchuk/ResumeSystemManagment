namespace ResumeSystemManagement.Core.ReadModels;

public static class RoleNames
{
    public const string Candidate = nameof(Candidate);
    public const string Recruiter = nameof(Recruiter);
    public const string Administrator = nameof(Administrator);
    
    public static readonly List<string> Roles = [Candidate, Recruiter, Administrator];
}