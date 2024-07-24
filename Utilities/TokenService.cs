using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProgramPlatform.Data;
using ProgramPlatform.Models;
using ProgramPlatform.Services;

namespace ProgramPlatform.Utilities;

public class TokenService(ApplicationDbContext database, ILogger<TokenService> logger, HttpClient httpClient,
    EncryptionServices encryptionServices)
{
    /// <summary>
    /// Retrieves the access token from the OAuthTokens table and refreshes it if necessary.
    /// </summary>
    /// <returns>The decrypted access token.</returns>
    public async Task<string> GetAccessToken()
    {
        var token = await database.OAuthTokens.OrderByDescending(t => t.Id).FirstOrDefaultAsync();

        if (token == null || token.ExpiresAt <= DateTime.UtcNow)
        {
            // Refresh token
            token = await RefreshTokenAsync(encryptionServices.Decrypt(token.RefreshToken));
        }

        return encryptionServices.Decrypt(token.AccessToken);
    }

    /// <summary>
    /// Refreshes the access token using the given refresh token and returns the updated token.
    /// </summary>
    /// <param name="refreshToken">The refresh token used to refresh the access token.</param>
    /// <returns>The updated access token.</returns>
    private async Task<ZohoTokenModel> RefreshTokenAsync(string refreshToken)
    {
        var clientId = Environment.GetEnvironmentVariable("CirrusZohoClientId");
        var clientSecret = Environment.GetEnvironmentVariable("CirrusZohoClientSecret");
        var refreshUrl = "https://accounts.zoho.com/oauth/v2/token?";

        var request = new HttpRequestMessage(HttpMethod.Post, refreshUrl);
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "refresh_token", refreshToken },
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "grant_type", "refresh_token" }
        });

        request.Content = content;
        logger.LogInformation(request.ToString());
        logger.LogInformation(request.Content.ToString());

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<ZohoTokenResponse>(jsonResponse);

        var newToken = new ZohoTokenModel
        {
            AccessToken = encryptionServices.Encrypt(tokenResponse.AccessToken),
            RefreshToken = encryptionServices.Encrypt(tokenResponse.RefreshToken ?? refreshToken),
            ExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
        };

        database.OAuthTokens.Add(newToken);
        await database.SaveChangesAsync();

        return newToken;
    }
}

/// <summary>
/// Represents a Zoho token response that contains the access token, refresh token, and expiration time.
/// </summary>
public class ZohoTokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
    
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
}