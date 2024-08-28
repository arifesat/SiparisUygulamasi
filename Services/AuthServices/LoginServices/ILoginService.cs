using Microsoft.AspNetCore.Identity.Data;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Services.AuthServices.LoginServices
{
    public interface ILoginService
    {
        Task<User> GetByNameAsync(string name);
        Task<User> GetByEmailAsync(string email);
        //Task<LoginResponse> LoginUserAsync(LoginRequest request);
        Task LogoutUserAsync();
    }
}