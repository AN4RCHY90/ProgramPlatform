using System.ComponentModel.DataAnnotations.Schema;
using ProgramPlatform.Enums;

namespace ProgramPlatform.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public PreferredModeEnum PreferredMode { get; set; }

        [ForeignKey("RoleModel")]
        public Guid RoleId { get; set; }
        public RoleModel Role { get; set; }

        public bool MultiFactor { get; set; }
        public bool IsAdmin { get; set; }

        // Store UserTypes as a comma-separated string
        public string UserTypes { get; set; }

        [ForeignKey("AccountModel")]
        public Guid AccountId { get; set; }
        public AccountModel Account { get; set; }

        public bool IsArchived { get; set; }

        [NotMapped]
        public List<UserTypeEnum> UserTypesList
        {
            get => UserTypes?.Split(',')?.Select(Enum.Parse<UserTypeEnum>)?.ToList() ?? new List<UserTypeEnum>();
            set => UserTypes = string.Join(',', value);
        }
    }
}