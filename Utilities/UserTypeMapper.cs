using ProgramPlatform.Enums;

namespace ProgramPlatform.Utilities;

public static class UserTypeMapper
{
    /// <summary>
    /// Parses a string representation of an account type into its corresponding AccountTypeEnum value.
    /// </summary>
    /// <param name="accountType">The string representation of the account type.</param>
    /// <returns>The corresponding AccountTypeEnum value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the account type string is not recognized.</exception>
    public static AccountTypeEnum ParseAccountType(string accountType)
    {
        return accountType switch
        {
            "Commtel" => AccountTypeEnum.Commtel,
            "Installer" => AccountTypeEnum.Installer,
            "Managing Agent" => AccountTypeEnum.ManagingAgent,
            _ => throw new ArgumentOutOfRangeException(nameof(accountType),
                $"Unexpected account type value: {accountType}")
        };
    }
    
    /// <summary>
    /// Maps an AccountTypeEnum value to a UserTypeEnum value.
    /// </summary>
    /// <param name="accountType">The AccountTypeEnum value to map.</param>
    /// <returns>The corresponding UserTypeEnum value.</returns>
    public static UserTypeEnum MapAccountTypeToUserType(AccountTypeEnum accountType)
    {
        return accountType switch
        {
            AccountTypeEnum.Commtel => UserTypeEnum.Commtel,
            AccountTypeEnum.Installer => UserTypeEnum.Installer,
            AccountTypeEnum.ManagingAgent => UserTypeEnum.ManagingAgent,
            _ => throw new ArgumentOutOfRangeException(nameof(accountType),
                $"Unexpected account type value: {accountType}")
        };
    }
}