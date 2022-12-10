using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiWeb.Shared
{
    public class AddUserToObjectDTO
    {
        public string UserEmail { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public int Role { get; set; }
    }
}
