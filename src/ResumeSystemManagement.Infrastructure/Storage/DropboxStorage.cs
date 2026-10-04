using System.Text;
using Dropbox.Api;
using Dropbox.Api.Files;
using ResumeSystemManagement.Application.Interfaces.FileStorage;

namespace ResumeSystemManagement.Infrastructure.Storage;

public class DropboxStorage(DropboxClient dropboxClient): IFileStorage
{
    private readonly DropboxClient _dropboxClient = dropboxClient;
    private const string Folder = @"/SupportTickets_ResumeSystem";

    public async Task SaveFileAsync(string file, string content)
    {
        using var mem = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var updated = await _dropboxClient.Files.UploadAsync(
            $"{Folder}/{file}",
            WriteMode.Overwrite.Instance,
            body: mem);
        Console.WriteLine("Saved {0}/{1} rev {2}", Folder, file, updated.Rev);
    }
}