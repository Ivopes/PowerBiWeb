using System.ComponentModel.DataAnnotations;

namespace PowerBiWeb.Shared.Users
{
    public class UserRegisterInformation : UserLoginInformation
    {
        [Required]
        public string Firstname { get; set; } = string.Empty;
        [Required]
        public string Lastname { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
