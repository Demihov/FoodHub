using FoodHub.Data;
using FoodHub.Models;
using FoodHub.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodHub.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<CustomerUser> _userManager;
		private readonly SignInManager<CustomerUser> _signInManager;
		private readonly ILogger<AccountController> _logger;
		private readonly ApplicationDbContext _context;

		public AccountController(
			UserManager<CustomerUser> userManager, 
			SignInManager<CustomerUser> signInManager,
			ApplicationDbContext context,
			ILogger<AccountController> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
			_logger = logger;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterDto registerDto)
		{
			var user = new CustomerUser { UserName = registerDto.Email, Email = registerDto.Email };
			var result = await _userManager.CreateAsync(user, registerDto.Password);
			

			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, isPersistent: false);
				_logger.LogInformation("User created a new account with password.");

				return Ok();
			}

			return BadRequest(result.Errors);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);
			if (result.Succeeded)
			{
				_logger.LogInformation("User logged in.");
				return Ok();
			}

			return BadRequest("Invalid login attempt.");
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");
			return Ok();
		}

		[HttpGet("user/{id}")]
		public async Task<IActionResult> GetUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return Ok(new
			{
				user.Id,
				user.UserName,
				user.Email
			});
		}

		[HttpGet("users")]
		public async Task<IActionResult> GetUsers()
		{
			var users = _userManager.Users.Select(u => new
			{
				u.Id,
				u.UserName,
				u.Email
			});

			return Ok(users);
		}
	}
}

