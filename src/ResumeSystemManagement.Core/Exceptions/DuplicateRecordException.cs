namespace ResumeSystemManagement.Core.Exceptions;

public class DuplicateRecordException(string nameColumn) : Exception($"Record '{nameColumn}' already exists.")
{
    public string NameColumn { get; } = nameColumn;
}