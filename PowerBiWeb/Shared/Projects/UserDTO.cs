namespace PowerBiWeb.Shared.Projects
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ProjectRoleDTO Role { get; set; }
    }
}
