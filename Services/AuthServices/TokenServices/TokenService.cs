using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SiparisUygulamasi.Models.Request.UserRequest;
using SiparisUygulamasi.Models.Response.UserResponse;

namespace SiparisUygulamasi.Services.AuthServices.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrEmpty(_configuration["AppSettings:Secret"]))
            {
                throw new ArgumentNullException("AppSettings:Secret", "Secret key cannot be null or empty.");
            }
        }

        public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.id))
            {
                throw new ArgumentNullException(nameof(request.id), "User ID cannot be null or empty.");
            }

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]));
            var dateTimeNow = DateTime.UtcNow;

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _configuration["AppSettings:ValidIssuer"],
                audience: _configuration["AppSettings:ValidAudience"],
                claims: new List<Claim> {
                    new Claim("id", request.id)
                },
                notBefore: dateTimeNow,
                expires: dateTimeNow.Add(TimeSpan.FromMinutes(500)),
                signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            );

            return Task.FromResult(new GenerateTokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                TokenExpireDate = dateTimeNow.Add(TimeSpan.FromMinutes(500))
            });
        }
    }
}
