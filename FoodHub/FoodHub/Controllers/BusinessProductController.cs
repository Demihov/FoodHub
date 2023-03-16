using AutoMapper;
using FoodHub.Data;
using FoodHub.Models;
using FoodHub.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodHub.Controllers
{
	//[Authorize(Roles = "Business")]
	[ApiController]
	[Route("api/business")]
	public class BusinessProductController: ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;
		public readonly IMapper _mapper;

		public BusinessProductController(
			UserManager<User> userManager, 
			ApplicationDbContext context,
			IMapper mapper)
		{
			_userManager = userManager;
			_context = context;
			_mapper = mapper;
		}

		[HttpPost("products")]
		public async Task<ActionResult<ProductDto>> AddProduct(ProductDto productDto)
		{
			var businessUser = await _userManager.GetUserAsync(User);
			if (businessUser == null)
			{
				return NotFound("Business not found");
			}

			var product = new Product
			{
				BusinessId = businessUser.Id,
				Name = productDto.Name,
				Description = productDto.Description,
				Price = productDto.Price,
				ImageUrl = productDto.ImageUrl
			};

			_context.Products.Add(product);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);
		}

		// GET api/business/products
		[HttpGet("products")]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
		{
			var businessUser = await _userManager.GetUserAsync(User);
			if (businessUser == null)
			{
				return NotFound("Business not found");
			}
			var products = await _context.Products
				.Where(p => p.BusinessId == businessUser.Id)
				.Select(p => _mapper.Map<ProductDto>(p))
				.ToListAsync();

			return products;
		}

		// GET api/business/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDto>> GetProduct(int id)
		{
			var user = await _userManager.GetUserAsync(User);
			var product = await _context.Products.FindAsync(id);

			if (product == null || product.BusinessId != user.Id)
			{
				return NotFound();
			}

			return _mapper.Map<ProductDto>( product);
		}


		// PUT api/business/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
		{
			if (id != productDto.Id)
			{
				return BadRequest();
			}

			var user = await _userManager.GetUserAsync(User);
			var product = await _context.Products.FindAsync(productDto.Id);

			if (product.BusinessId != user.Id)
			{
				return Forbid();
			}

			_context.Entry(productDto).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(id))
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

		// DELETE api/business/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var user = await _userManager.GetUserAsync(User);

			var product = await _context.Products.FindAsync(id);

			if (product == null || product.BusinessId != user.Id)
			{
				return NotFound();
			}

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.Id == id);
		}

	}
}
