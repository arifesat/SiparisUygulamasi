using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Services.AuthServices.IndetityServices
{
    public class IdentityService : IIdentityService
    {
        public async Task<User> LoginByUserNameAndEmailQuery(User? userGetByUserName, User? userGetByEmail)
        {
            User user = new User
            {
                Username = string.Empty,
                Email = string.Empty,
                Password = string.Empty,
                Balance = 0.0m,
                IsAdmin = false
            }; ;
            if (userGetByUserName != null)
            {
                user = userGetByUserName;
            }
            if (userGetByEmail != null)
            {
                user = userGetByEmail;
            }
            return user;
        }
    }
}
