using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Services.AuthServices.IndetityServices
{
    public interface IIdentityService
    {
        Task<User> LoginByUserNameAndEmailQuery(User? userGetByUserName, User? userGetByEmail);

    }
}
