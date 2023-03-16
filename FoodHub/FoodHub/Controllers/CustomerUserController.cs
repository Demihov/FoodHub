using FoodHub.Data;
using FoodHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FoodHub.Controllers
{
	//[Authorize(Roles = "Customer")]
	[ApiController]
	[Route("[controller]")]
	public class CustomerUserController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public CustomerUserController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CustomerUser>>> GetCustomerUsers()
		{
			return await _context.CustomerUsers.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerUser>> GetCustomerUser(int id)
		{
			var customerUser = await _context.CustomerUsers.FindAsync(id);

			if (customerUser == null)
			{
				return NotFound();
			}

			return customerUser;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCustomerUser(string id, CustomerUser customerUser)
		{
			if (id != customerUser.Id)
			{
				return BadRequest();
			}

			_context.Entry(customerUser).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CustomerUserExists(id))
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

		[HttpPost]
		public async Task<ActionResult<CustomerUser>> CreateCustomerUser(CustomerUser customerUser)
		{
			_context.CustomerUsers.Add(customerUser);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetCustomerUser), new { id = customerUser.Id }, customerUser);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCustomerUser(int id)
		{
			var customerUser = await _context.CustomerUsers.FindAsync(id);
			if (customerUser == null)
			{
				return NotFound();
			}

			_context.CustomerUsers.Remove(customerUser);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CustomerUserExists(string id)
		{
			return _context.CustomerUsers.Any(e => e.Id == id);
		}
	}
}
