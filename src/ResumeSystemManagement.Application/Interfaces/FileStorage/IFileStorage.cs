namespace ResumeSystemManagement.Application.Interfaces.FileStorage;

public interface IFileStorage
{
    Task SaveFileAsync(string file, string content);
}