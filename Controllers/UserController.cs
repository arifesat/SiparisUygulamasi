using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using MongoDB.Bson;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Services;
using SiparisUygulamasi.Dtos;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly MongoDBContext _context;
    private readonly UserService _userService;

    public UserController(MongoDBContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<User>> Get()
    {
        return await _context.Users.Find(_ => true).ToListAsync();
    }

    [HttpGet("{id}")]
    [Authorize]
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
    [Authorize]
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
    [Authorize]
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var token = await _userService.AuthenticateAsync(loginDTO.Email, loginDTO.Password);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }
}

