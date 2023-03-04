using FoodHub.Data;
using FoodHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodHub.Controllers
{
	[Authorize]
	public class BusinessesController: Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public BusinessesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Businesses
		public async Task<IActionResult> Index()
		{
			var businesses = await _context.BusinessUsers.ToListAsync();
			return View(businesses);
		}

		// GET: Businesses/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Businesses/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Description")] BusinessUser business)
		{
			if (ModelState.IsValid)
			{
				_context.Add(business);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(business);
		}

		// GET: Businesses/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var business = await _context.BusinessUsers.FindAsync(id);
			if (business == null)
			{
				return NotFound();
			}
			return View(business);
		}
	}
}
