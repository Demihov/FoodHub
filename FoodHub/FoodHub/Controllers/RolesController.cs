using FoodHub.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FoodHub.Controllers
{
	//[Authorize(Roles = "Admin")]
	[ApiController]
	[Route("api/[controller]")]
	public class RolesController : ControllerBase
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public RolesController(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<IdentityRole>>> GetAllRoles()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return Ok(roles);
		}

		[HttpPost]
		public async Task<ActionResult<IdentityRole>> CreateRole(RoleDto roleModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var role = new IdentityRole { Name = roleModel.Name };

			var result = await _roleManager.CreateAsync(role);

			if (result.Succeeded)
			{
				return Ok(role);
			}

			return BadRequest(result.Errors);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateRole(string id, RoleDto role)
		{
			var existingRole = await _roleManager.FindByIdAsync(id);

			if (existingRole == null)
			{
				return NotFound();
			}

			existingRole.Name = role.Name;

			var result = await _roleManager.UpdateAsync(existingRole);

			if (result.Succeeded)
			{
				return NoContent();
			}

			return BadRequest(result.Errors);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRole(string id)
		{
			var existingRole = await _roleManager.FindByIdAsync(id);

			if (existingRole == null)
			{
				return NotFound();
			}

			var result = await _roleManager.DeleteAsync(existingRole);

			if (result.Succeeded)
			{
				return NoContent();
			}

			return BadRequest(result.Errors);
		}
	}
}
