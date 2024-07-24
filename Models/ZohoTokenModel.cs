using System.ComponentModel.DataAnnotations;

namespace ProgramPlatform.Models
{
    public class ZohoTokenModel
    {
        [Key]
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}