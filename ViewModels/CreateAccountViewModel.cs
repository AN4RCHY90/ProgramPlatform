namespace ProgramPlatform.ViewModels;

public class CreateAccountViewModel
{
    public string ReferenceNumber { get; set; }
    public string AccountName { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime SubscriptionExpiryDate { get; set; }
    public string AccountType { get; set; }
    public string AdminFirstName { get; set; }
    public string AdminLastName { get; set; }
}