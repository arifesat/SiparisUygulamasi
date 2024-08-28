using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Models.Response.UserResponse;

namespace SiparisUygulamasi.Services.AuthServices.LoginServices
{
    public interface ILoginService
    {
        Task<User> GetByNameAsync(string name);
        Task<User> GetByEmailAsync(string email);

        Task LogoutUserAsync();
        Task<ActionResult<LoginResponse>> LoginUserAsync(Models.Request.UserRequest.LoginRequest request);
    }
}