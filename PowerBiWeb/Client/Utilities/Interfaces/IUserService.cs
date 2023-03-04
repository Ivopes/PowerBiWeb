﻿using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.User;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IUserService
    {
        Task<HttpResponse<UserDetail>> ChangeUsername(string username);
        Task<HttpResponse<UserDetail>> ChangeUsername(string username, CancellationToken ct);
    }
}