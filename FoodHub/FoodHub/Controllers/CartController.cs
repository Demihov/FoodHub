using FoodHub.Data;
using FoodHub.Models;
using FoodHub.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
	private readonly ApplicationDbContext _context;
	private readonly UserManager<CustomerUser> _userManager;

	public CartController(ApplicationDbContext context, UserManager<CustomerUser> userManager)
	{
		_context = context;
		_userManager = userManager;
	}

	// GET: api/cart
	[HttpGet]
	public async Task<ActionResult<Cart>> GetCart()
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return NotFound();
		}

		var cart = await _context.Carts
			.Include(c => c.CartItems)
			.ThenInclude(ci => ci.Product)
			.FirstOrDefaultAsync(c => c.UserId == user.Id);

		if (cart == null)
		{
			cart = new Cart
			{
				UserId = user.Id,
				CartItems = new List<CartItem>()
			};
			_context.Carts.Add(cart);
			await _context.SaveChangesAsync();
		}

		return cart;
	}

	// POST: api/cart/items
	[HttpPost("items")]
	public async Task<ActionResult<CartItem>> AddCartItem(CartItem cartItem)
	{
		if (cartItem == null)
		{
			return BadRequest();
		}

		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return Unauthorized();
		}

		var cart = await _context.Carts
			.Include(c => c.CartItems)
			.ThenInclude(ci => ci.Product)
			.FirstOrDefaultAsync(c => c.UserId == user.Id);

		if (cart == null)
		{
			cart = new Cart
			{
				UserId = user.Id,
				CartItems = new List<CartItem>()
			};
			_context.Carts.Add(cart);
		}

		var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItem.ProductId);

		if (existingCartItem == null)
		{
			cart.CartItems.Add(cartItem);
			//_context.CartItems.Add(cartItem);
		}
		else
		{
			existingCartItem.Quantity += cartItem.Quantity;
		}

		await _context.SaveChangesAsync();

		return Ok(cart);
		//return CreatedAtAction("GetCartItems", new { id = item.Id }, item);
	}

	// DELETE: api/cart/items/5
	[HttpDelete("items/{id}")]
	public async Task<ActionResult<CartItem>> DeleteCartItem(int id)
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return Unauthorized();
		}

		var cart = await _context.Carts
			.Include(c => c.CartItems)
			.FirstOrDefaultAsync(c => c.UserId == user.Id);

		if (cart == null)
		{
			return NotFound();
		}

		var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == id);

		if (cartItem == null)
		{
			return NotFound();
		}


		_context.CartItems.Remove(cartItem);
		await _context.SaveChangesAsync();

		return cartItem;
	}

	[HttpPost("checkout")]
	public async Task<ActionResult> Checkout()
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return Unauthorized();
		}

		var cart = await _context.Carts.SingleOrDefaultAsync(c => c.UserId == user.Id);

		var cartItems = await _context.CartItems
			.Where(ci => ci.CartId == cart.Id)
			.Include(ci => ci.Product)
			.ToListAsync();

		if (cartItems.Count == 0)
		{
			return BadRequest("Cart is empty");
		}

		var order = new Order
		{
			OrderDate = DateTime.UtcNow,
			UserId = user.Id,
			OrderItems = cartItems.Select(cartItem => new OrderItem
			{
				Quantity = cartItem.Quantity,
				ProductId = cartItem.ProductId,
			}).ToList(),
		};

		_context.Orders.Add(order);
		_context.CartItems.RemoveRange(cartItems);
		await _context.SaveChangesAsync();

		return Ok();

		//new OrderDto
		//{
		//	Id = order.Id,
		//	OrderDate = order.OrderDate,
		//	UserId = order.UserId,
		//	OrderItems = order.OrderItems.Select(orderItem => new OrderItemDto
		//	{
		//		Id = orderItem.Id,
		//		Quantity = orderItem.Quantity,
		//		Product = new ProductDto
		//		{
		//			Id = orderItem.Product.Id,
		//			Name = orderItem.Product.Name,
		//			Description = orderItem.Product.Description,
		//			Price = orderItem.Product.Price,
		//			ImageUrl = orderItem.Product.ImageUrl,
		//		},
		//	}).ToList(),
		//};
	}


}
