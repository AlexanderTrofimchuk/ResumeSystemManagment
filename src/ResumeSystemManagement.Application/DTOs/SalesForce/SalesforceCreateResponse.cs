using System.Text.Json.Serialization;

namespace ResumeSystemManagement.Application.DTOs.SalesForce;

public class SalesforceCreateResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("errors")]
    public List<SalesforceError> Errors { get; set; } = new();
}

public class SalesforceError
{
    [JsonPropertyName("statusCode")]
    public string? StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("fields")]
    public List<string> Fields { get; set; } = new();
}