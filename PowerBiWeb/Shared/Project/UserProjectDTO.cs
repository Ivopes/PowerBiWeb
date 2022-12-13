using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiWeb.Shared.Project
{
    public class UserProjectDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ProjectRoleDTO Role { get; set; }
    }
}
