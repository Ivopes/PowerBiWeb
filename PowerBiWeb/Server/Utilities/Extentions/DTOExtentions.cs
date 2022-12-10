﻿using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Utilities.Extentions
{
    public static class DTOExtentions
    {
        public static ProjectDTO ToDTO(this Project p)
        {
            var dto = new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name
            };

            dto.Users = new List<UserProject>();

            foreach (var user in p.AppUserProject)
            {
                dto.Users.Add(new()
                {
                    Email = user.AppUser.Email,
                    Username = user.AppUser.Username,
                    Role = (ProjectRoleDTO)user.Role,
                });
            }

            return dto;
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