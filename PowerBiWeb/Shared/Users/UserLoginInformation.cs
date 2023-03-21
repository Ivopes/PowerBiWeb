using System.ComponentModel.DataAnnotations;

namespace PowerBiWeb.Shared.Users
{
    public class UserLoginInformation
    {

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
