using System.Net.Http.Headers;
using ProgramPlatform.Data;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Services;

public class ZohoServices(HttpClient httpClient, ApplicationDbContext database, TokenService tokenService,
    ILogger<ZohoServices> logger): IZohoInterface
{
    /// <summary>
    /// Check if an account with the given reference number exists in the database.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the account.</param>
    /// <returns>True if an account with the given reference number exists, otherwise false.</returns>
    public async Task<bool> AccountExistsAsync(string referenceNumber)
    {
        //return await database.AccountModels.AnyAsync(a => a.ReferenceNumber == referenceNumber);
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="referenceNumber"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ZohoAccountModel> GetAccountByReferenceNumberAsync(string referenceNumber)
    {
        /*var accessToken = await tokenService.GetAccessToken();
        
        string apiUrl = $"https://www.zohoapis.com/crm/v2/CPM/{referenceNumber}";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);
        logger.LogInformation(apiUrl);
        
        var response = await httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<ZohoResponse>(jsonResponse);
        var accountData = data.Data.FirstOrDefault();

        if (accountData == null)
        {
            throw new Exception("Account not found");
        }

        return new ZohoAccountModel
        {
            Id = accountData.Id,
            Sub_Exp = accountData.Sub_Exp,
            Account_Type = accountData.Account_Type,
            Email = accountData.Email,
            Name = accountData.Name,
            Phone = accountData.Phone
        };*/
        throw new NotImplementedException();
    }
}

/// <summary>
/// Represents a response from Zoho APIs.
/// </summary>
public class ZohoResponse
{
    public List<ZohoAccountData> Data { get; set; }
}

/// <summary>
/// Represents data for a Zoho account.
/// </summary>
public class ZohoAccountData
{
    public string Id { get; set; }
    public string Sub_Exp { get; set; }
    public string Account_Type { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
}