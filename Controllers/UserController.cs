using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly MongoDBContext _context;

    public UserController(MongoDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        return await _context.Users.Find(_ => true).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var User = await _context.Users.Find(p => p.Id == id).FirstOrDefaultAsync();

        if (User == null)
        {
            return NotFound();
        }

        return User;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User User)
    {
        await _context.Users.InsertOneAsync(User);
        return CreatedAtRoute(new { id = User.Id }, User);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, User UserIn)
    {
        var User = await _context.Users.Find(p => p.Id == id).FirstOrDefaultAsync();

        if (User == null)
        {
            return NotFound();
        }

        await _context.Users.ReplaceOneAsync(p => p.Id == id, UserIn);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var User = await _context.Users.Find(p => p.Id == id).FirstOrDefaultAsync();

        if (User == null)
        {
            return NotFound();
        }

        await _context.Users.DeleteOneAsync(p => p.Id == id);

        return NoContent();
    }

}

