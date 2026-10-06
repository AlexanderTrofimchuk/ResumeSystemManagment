namespace ResumeSystemManagement.Application.DTOs.SalesForce;

using System.Text.Json.Serialization;

public class SalesForceTokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("signature")]
    public string? Signature { get; set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    [JsonPropertyName("instance_url")]
    public string? InstanceUrl { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("issued_at")]
    public string? IssuedAt { get; set; }
}