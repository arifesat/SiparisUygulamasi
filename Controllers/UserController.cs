using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Data;
using MongoDB.Driver;
using MongoDB.Bson;
using SiparisUygulamasi.Services;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        return await _userService.GetAllUsersAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var objectId = ObjectId.Parse(id);
        var User = await _userService.GetUserByIdAsync(objectId);

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
        await _userService.AddUserAsync(User);
        return CreatedAtRoute(new { id = User.Id }, User);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, User UserIn)
    {
        var objectId = ObjectId.Parse(id);
        var User = await _userService.GetUserByIdAsync(objectId);

        if (User == null)
        {
            return NotFound();
        }

        await _userService.UpdateUserAsync(objectId, UserIn);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var objectId = ObjectId.Parse(id);

        var User = await _userService.GetUserByIdAsync(objectId); ;

        if (User == null)
        {
            return NotFound();
        }

        await _userService.DeleteUserAsync(objectId);

        return NoContent();
    }
}

