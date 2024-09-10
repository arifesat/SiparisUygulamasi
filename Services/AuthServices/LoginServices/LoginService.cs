using System.Text;
using SiparisUygulamasi.Models.Request.UserRequest;
using SiparisUygulamasi.Models.Response.UserResponse;
using SiparisUygulamasi.Services.AuthServices.TokenServices;
using SiparisUygulamasi.Middleware;
using SiparisUygulamasi.Services.AuthServices.IndetityServices;
using SiparisUygulamasi.Repositories;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Amazon.Runtime.Internal.Util;

namespace SiparisUygulamasi.Services.AuthServices.LoginServices
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository<User> _repository;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IIdentityService _identityservice;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<LoginService> _logger;
        private string hashPassword;

        public LoginService(IUserRepository<User> repository, ITokenService tokenService, IHttpContextAccessor contextAccessor, IMemoryCache memoryCache, IIdentityService identityService, ILogger<LoginService> logger)
        {
            _repository = repository;
            _tokenService = tokenService;
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
            _identityservice = identityService;
            _logger = logger;
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

        public async Task<ActionResult<LoginResponse>> LoginUserAsync(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();

            // Log the incoming request
            Console.WriteLine($"Login request received. Username: {request.Username}, Email: {request.Email}");

            // Fetch user by username and email
            User user = await _identityservice.LoginByUserNameAndEmailQuery(
                await _repository.GetByNameAsync(request.Username),
                await _repository.GetByEmailAsync(request.Email)
            );

            // Log the fetched user
            if (user != null)
            {
                Console.WriteLine($"User found. Username: {user.Username}, Email: {user.Email}");
            }
            else
            {
                Console.WriteLine("User not found.");
                response.AuthenticateResult = false;
                return response;
            }

            // Hash the password using SHA-256
            string hashPassword;
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                hashPassword = builder.ToString();
            }

            // Log the hashed password
            Console.WriteLine($"Hashed password: {hashPassword}");

            // Check if any fields are empty
            if (string.IsNullOrEmpty(request.Username) && string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                throw new BadRequestException("Fields cannot be empty");
            }

            if (user.PasswordHash == hashPassword)
            {
                GenerateTokenResponse generatedTokenInformation = await _tokenService.GenerateToken(new GenerateTokenRequest { id = user.Id.ToString() });

                // Log the generated token
                Console.WriteLine($"Generated token: {generatedTokenInformation.Token}");

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
                response.Admin = user.IsAdmin ? "Admin" : "User";
            }
            else
            {
                response.AuthenticateResult = false;
            }

            Console.WriteLine($"Login response: AuthenticateResult={response.AuthenticateResult}, AuthToken={response.AuthToken}, AccessTokenExpireDate={response.AccessTokenExpireDate}, Admin={response.Admin}");

            return response;
        }

        private async Task<LoginResponse> LoginUserAsyncInternal(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            User user = await _identityservice.LoginByUserNameAndEmailQuery(await _repository.GetByNameAsync(request.Username), await _repository.GetByEmailAsync(request.Email));

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
