using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using SiparisUygulamasi.Models.Response.UserResponse;
using SiparisUygulamasi.Services.AuthServices.LoginServices;
using SiparisUygulamasi.Models.Request.UserRequest;
using MongoDB.Driver;

namespace SiparisUygulamasi.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;
        private readonly IMemoryCache _memoryCache;

        public LoginController(ILoginService service, IMemoryCache memoryCache)
        {
            _service = service;
            _memoryCache = memoryCache;
        }

        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> LoginUserAsync([FromBody] LoginRequest request)
        {
            var result = await _service.LoginUserAsync(request);
            return result;
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutUserAsync();
            _memoryCache.Remove("Bearer");
            return Ok();
        }

        [HttpGet("redis/{name}")]
        public void Set(string name)
        {
            _memoryCache.Set("name", name);

        }
    }
}
