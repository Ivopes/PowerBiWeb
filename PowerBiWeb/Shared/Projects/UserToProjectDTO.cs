namespace PowerBiWeb.Shared.Projects
{
    public class UserToProjectDTO
    {
        public string UserEmail { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public ProjectRoleDTO Role { get; set; }
    }
}
