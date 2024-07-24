using ProgramPlatform.Models;

namespace ProgramPlatform.Interfaces;

public interface IZohoInterface
{
    Task<bool> AccountExistsAsync(string referenceNumber);
    Task<ZohoAccountModel> GetAccountByReferenceNumberAsync(string referenceNumber);
}