using System.Net;
using System.Text;
using System.Text.Json;
using FluentResults;
using ResumeSystemManagement.Application.DTOs.SalesForce;
using ResumeSystemManagement.Core.Interfaces.Service.SalesForce;

namespace ResumeSystemManagement.Infrastructure.Repositories.SalesForce;

public class SalesForceRepository(HttpClient client) : ISalesForceRepository
{
    private readonly HttpClient _client = client;
    private const string ApiUrl = $"https://orgfarm-5f5bebf671-dev-ed.develop.my.salesforce.com";
    private string _accessToken = string.Empty;
    private DateTime _tokenExpirationTime = DateTime.MinValue;
    private const string ApiVersion = "v67.0";
    
    private static string SObject(string model) => $"{ApiUrl}/services/data/{ApiVersion}/sobjects/{model}";
    private static string SObjectWithId(string model, string id) => $"{ApiUrl}/services/data/{ApiVersion}/sobjects/{model}/{id}";
    private string BearerToken => $"Bearer {_accessToken}";

    public async Task<Result<string>> CreateAccount(string jsonAccount)
    {
        return await CreateRecord("Account", jsonAccount, "Failed to create Salesforce account.");
    }
    
    public async Task<Result<string>> CreateContact(string jsonContact)
    {
        return await CreateRecord("Contact", jsonContact, "Failed to create Salesforce contact.");
    }

    public async Task<Result> DeleteAccount(string accountId)
    {
        return await DeleteRecord("Account", accountId, "Failed to delete Salesforce account.");
    }

    private async Task<Result<string>> CreateRecord(string model, string json, string failedMessage, bool retried = false)
    {
        await InitializeAccessToken();

        var response = await SendPost(json, SObject(model));

        if (response.StatusCode == HttpStatusCode.Unauthorized && !retried)
        {
            _accessToken = string.Empty;
            return await CreateRecord(model, json, failedMessage, retried: true);
        }

        if (!response.IsSuccessStatusCode)
            return Result.Fail<string>(failedMessage);

        var created = await SalesforceCreateResponse(response);
        return Result.Ok(created!.Id!);
    }
    
    private async Task<Result> DeleteRecord(string model, string id, string failedMessage, bool retried = false)
    {
        await InitializeAccessToken();

        var response = await SendDelete(SObjectWithId(model, id));

        if (response.StatusCode == HttpStatusCode.Unauthorized && !retried)
        {
            _accessToken = string.Empty;
            return await DeleteRecord(model, id, failedMessage, retried: true);
        }

        return !response.IsSuccessStatusCode ? Result.Fail(failedMessage) : Result.Ok();
    }
    
    private static async Task<SalesforceCreateResponse?> SalesforceCreateResponse(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseDict = JsonSerializer.Deserialize<SalesforceCreateResponse>(responseContent);
        return responseDict;
    }

    private async Task<HttpResponseMessage> SendPost(string jsonContact, string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("Authorization", BearerToken);
        request.Content = new StringContent(jsonContact, Encoding.UTF8, "application/json");
        return await _client.SendAsync(request);
    }
    private async Task<HttpResponseMessage> SendDelete(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Add("Authorization", BearerToken);
        return await _client.SendAsync(request);
    }

    private async Task<SalesForceTokenResponse> GetAccessToken()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{ApiUrl}/services/oauth2/token");
        request.Content = RequestContent();
        var responseMessage = await _client.SendAsync(request);
        if (!responseMessage.IsSuccessStatusCode) throw new HttpRequestException(
            $"Failed to get access token from Salesforce. Status code: {responseMessage.StatusCode}");
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<SalesForceTokenResponse>(responseContent);
        return tokenResponse!;
    }

    private async Task InitializeAccessToken()
    {
        if(_accessToken == string.Empty  || _tokenExpirationTime <= DateTime.UtcNow)
        {
            var tokenResponse = await GetAccessToken();
            _accessToken = tokenResponse.AccessToken!;
            var time = Convert.ToDouble(tokenResponse.IssuedAt);
            _tokenExpirationTime = DateTime.UtcNow.AddMicroseconds(time);
        }
    }
    
    private static FormUrlEncodedContent RequestContent()
    {
        return new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", Environment.GetEnvironmentVariable("SALESFORCE_CONSUME_KEY")!},
            { "client_secret", Environment.GetEnvironmentVariable("SALESFORCE_CONSUME_SECRET")! }
        });
    }
}