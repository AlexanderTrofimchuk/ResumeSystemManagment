namespace ResumeSystemManagement.Core.StaticDatas;

public static class RoleNames
{
    public static readonly string Candidate = nameof(Candidate);
    public static readonly string Recruiter = nameof(Recruiter);
    public static readonly string Administrator = nameof(Administrator);
    
    public static readonly List<string> Roles = [Candidate, Recruiter, Administrator];
}