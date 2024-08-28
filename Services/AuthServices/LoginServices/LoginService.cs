using System.Text;
using SiparisUygulamasi.Models.Request.UserRequest;
using SiparisUygulamasi.Models.Response.UserResponse;
using SiparisUygulamasi.Services.AuthServices.TokenServices;
using SiparisUygulamasi.Middleware;
using SiparisUygulamasi.Services.AuthServices.IndetityServices;
using SiparisUygulamasi.Repositories;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;

namespace SiparisUygulamasi.Services.AuthServices.LoginServices
{
    public class LoginService : ILoginService

    {
        private readonly IUserRepository<User> _repository;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IIdentityService _ıdentityservice;
        private readonly IMemoryCache _memoryCache;
        private string hashPassword;


        public LoginService(IUserRepository<User> repository, ITokenService tokenService, IHttpContextAccessor contextAccessor, IMemoryCache memoryCache, IIdentityService ıdentityService)
        {
            _repository = repository;
            _tokenService = tokenService;
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
            _ıdentityservice = ıdentityService;
        }

        public async Task<User> GetByNameAsync(string name)
        {
            User user = await _repository.GetByNameAsync(name);
            if (user == null)
            {
                throw new NotFoundException($"{name} not found ");
            }
            return user;
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            User user = await _repository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException($"{email} not found ");
            }
            return user;
        }


        public async Task<LoginResponse> LoginUserAsync(Models.Request.UserRequest.LoginRequest request)

        {
            LoginResponse response = new LoginResponse();
            User user = await _ıdentityservice.LoginByUserNameAndEmailQuery(await _repository.GetByNameAsync(request.Username), await _repository.GetByEmailAsync(request.Email));

            SHA1 sha = new SHA1CryptoServiceProvider();
            hashPassword = Convert.ToBase64String(sha.ComputeHash(Encoding.ASCII.GetBytes(request.Password)));
            GenerateTokenResponse generatedTokenInformation = new GenerateTokenResponse();


            if ((string.IsNullOrEmpty(request.Username) && (string.IsNullOrEmpty(request.Email)) || string.IsNullOrEmpty(request.Password)))
            {
                throw new BadRequestException($"Alanlar boş bırakılmaz");
            }


            if (user == null)
            {
                response.AuthenticateResult = false;
                return response;
            }



            if (user.Password == hashPassword)
            {
                generatedTokenInformation = await _tokenService.GenerateToken(new GenerateTokenRequest { id = user.Id.ToString() });
                response.AuthenticateResult = true;
                response.AuthToken = generatedTokenInformation.Token;
                response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = generatedTokenInformation.TokenExpireDate,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };
                _contextAccessor.HttpContext.Response.Cookies.Append("AuthToken", generatedTokenInformation.Token, cookieOptions);
                _memoryCache.Set("Bearer", generatedTokenInformation.Token);
                response.Admin = user.IsAdmin ? "Admin" : "Kullanici";

            }
            else
            {
                response.AuthenticateResult = false;
            }
            return response;
        }



        public async Task LogoutUserAsync()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            _contextAccessor.HttpContext.Response.Cookies.Append("AuthToken", "", cookieOptions);
            await Task.CompletedTask;
        }
    }
}
