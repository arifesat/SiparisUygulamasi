//using Microsoft.AspNetCore.Mvc;
//using SiparisUygulamasi.Models.Request.UserRequest;
//using SiparisUygulamasi.Models.Response.UserResponse;
//using SiparisUygulamasi.Services.AuthServices.LoginServices;
//using System.Threading.Tasks;

//namespace SiparisUygulamasi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly ILoginService _loginService;

//        public AuthController(ILoginService loginService)
//        {
//            _loginService = loginService;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] SiparisUygulamasi.Models.Request.UserRequest.LoginRequest request)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var response = await _loginService.LoginUserAsync(request);

//            if (!response.AuthenticateResult)
//            {
//                return Unauthorized("Invalid credentials");
//            }

//            return Ok(response);
//        }

//        [HttpPost("logout")]
//        public async Task<IActionResult> Logout()
//        {
//            await _loginService.LogoutUserAsync();
//            return Ok("Logged out successfully");
//        }
//    }
//}
