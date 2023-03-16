using FoodHub.Data;
using FoodHub.Models;
using FoodHub.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FoodHub.Controllers
{
	public class AccountController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly ILogger<AccountController> _logger;
		private readonly ApplicationDbContext _context;

		public AccountController(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			ApplicationDbContext context,
			ILogger<AccountController> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
			_logger = logger;
		}

		// POST api/auth/register/customer
		[HttpPost("register/customer")]
		public async Task<IActionResult> RegisterCustomer( RegisterDto model)
		{
			var user = new CustomerUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			await _userManager.AddToRoleAsync(user, "customer");
			await _signInManager.SignInAsync(user, isPersistent: false);

			return Ok(new { token = GenerateJwtToken(user) });
		}

		// POST api/auth/register/business
		[HttpPost("register/business")]
		public async Task<IActionResult> RegisterBusiness([FromBody] RegisterDto model)
		{
			var user = new BusinessUser { UserName = model.Email, Email = model.Email, BusinessName = model.FirstName };
			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			await _userManager.AddToRoleAsync(user, "business");
			await _signInManager.SignInAsync(user, isPersistent: false);

			return Ok(new { token = GenerateJwtToken(user) });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);
			if (result.Succeeded)
			{
				var user = _userManager.Users.SingleOrDefault(r => r.UserName == loginDto.Email);
				return Ok(new { Token = GenerateJwtToken(user) });

				//_logger.LogInformation("User logged in.");
				//return Ok();
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

		private string GenerateJwtToken(IdentityUser user)
		{
			var claims = new List<Claim> {
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.NameId, user.Id)
			};

			var now = DateTime.UtcNow;

			var signingCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
			var expires = now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME));

			var token = new JwtSecurityToken(
			    issuer: AuthOptions.ISSUER,
			    audience: AuthOptions.AUDIENCE,
			    notBefore: now,
			    claims: claims,
			    expires: expires,
			    signingCredentials: signingCredentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}

