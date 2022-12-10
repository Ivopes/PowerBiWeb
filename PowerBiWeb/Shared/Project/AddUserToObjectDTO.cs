using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiWeb.Shared.Project
{
    public class AddUserToObjectDTO
    {
        public string UserEmail { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public ProjectRoleDTO Role { get; set; }
    }
}
