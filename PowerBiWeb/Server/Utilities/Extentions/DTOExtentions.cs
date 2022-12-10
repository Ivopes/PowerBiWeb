using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Utilities.Extentions
{
    public static class DTOExtentions
    {
        public static ProjectDTO ToDTO(this Project p)
        {
            return new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name
            };
        }
        public static Project ToBO(this ProjectDTO p)
        {
            return new Project
            {
                Id = p.Id,
                Name = p.Name
            };
        }
    }
}
