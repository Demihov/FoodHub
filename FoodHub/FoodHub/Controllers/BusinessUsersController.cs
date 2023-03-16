using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodHub.Models;
using FoodHub.Data;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BusinessUsersController : ControllerBase
{
	private readonly UserManager<User> _userManager;
	private readonly ApplicationDbContext _context;

	public BusinessUsersController(UserManager<User> userManager, ApplicationDbContext context)
	{
		_userManager = userManager;
		_context = context;
	}

	// GET: api/BusinessUsers
	[HttpGet]
	public async Task<ActionResult<IEnumerable<BusinessUser>>> GetBusinessUsers()
	{
		var businessUsers = await _context.Users.OfType<BusinessUser>().ToListAsync();
		return businessUsers;
	}

	// GET: api/BusinessUsers/5
	[HttpGet("{id}")]
	public async Task<ActionResult<BusinessUser>> GetBusinessUser(string id)
	{
		var businessUser = await _context.Users.OfType<BusinessUser>()
		    .FirstOrDefaultAsync(u => u.Id == id);

		if (businessUser == null)
		{
			return NotFound();
		}

		return businessUser;
	}

	// PUT: api/BusinessUsers/5
	[HttpPut("{id}")]
	public async Task<IActionResult> PutBusinessUser(string id, BusinessUser businessUser)
	{
		if (id != businessUser.Id)
		{
			return BadRequest();
		}

		_context.Entry(businessUser).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!BusinessUserExists(id))
			{
				return NotFound();
			}
			else
			{
				throw;
			}
		}

		return NoContent();
	}

	// POST: api/BusinessUsers
	[HttpPost]
	public async Task<ActionResult<BusinessUser>> PostBusinessUser(BusinessUser businessUser)
	{
		_context.Users.Add(businessUser);
		await _context.SaveChangesAsync();

		return CreatedAtAction("GetBusinessUser", new { id = businessUser.Id }, businessUser);
	}

	// DELETE: api/BusinessUsers/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteBusinessUser(string id)
	{
		var businessUser = await _context.Users.OfType<BusinessUser>()
		    .FirstOrDefaultAsync(u => u.Id == id);

		if (businessUser == null)
		{
			return NotFound();
		}

		_context.Users.Remove(businessUser);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool BusinessUserExists(string id)
	{
		return _context.Users.OfType<BusinessUser>().Any(u => u.Id == id);
	}
}
