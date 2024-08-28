using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using SiparisUygulamasi.Models.Response.UserResponse;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Services.AuthServices.LoginServices;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Models.Request.UserRequest;
using MongoDB.Driver;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILoginService _service;
    //private readonly IUpdateService _updateService;
    private readonly IMemoryCache _memoryCache;
    private readonly MongoDBContext _context;

    public UserController(MongoDBContext context, ILoginService service, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    //[HttpGet]
    //public async Task<IEnumerable<User>> Get()
    //{
    //    return await _context.Users.Find(_ => true).ToListAsync();
    //}

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var objectId = ObjectId.Parse(id);
        var User = await _context.Users.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        if (User == null)
        {
            return NotFound();
        }
        return User;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User User)
    {
        User.SetPassword(User.Password);
        await _context.Users.InsertOneAsync(User);
        return CreatedAtRoute(new { id = User.Id }, User);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, User UserIn)
    {
        var objectId = ObjectId.Parse(id);
        var User = await _context.Users.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        if (User == null)
        {
            return NotFound();
        }

        await _context.Users.ReplaceOneAsync(p => p.Id == objectId, UserIn);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var objectId = ObjectId.Parse(id);

        var User = await _context.Users.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        if (User == null)
        {
            return NotFound();
        }

        await _context.Users.DeleteOneAsync(p => p.Id == objectId);

        return NoContent();
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
    [HttpGet("redis get")]
    public string Get()
    {
        return _memoryCache.Get<string>("Bearer");

    }
}

