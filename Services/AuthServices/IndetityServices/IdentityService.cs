using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Services.AuthServices.IndetityServices
{
    public class IdentityService : IIdentityService
    {
        public async Task<User> LoginByUserNameAndEmailQuery(User? userGetByUserName, User? userGetByEmail)
        {
            if (userGetByUserName != null)
            {
                return userGetByUserName;
            }
            if (userGetByEmail != null)
            {
                return userGetByEmail;
            }
            return null;
        }
    }
}
