using FoodHub.Models.DTO;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodHub.Models
{
	public class BusinessUser: User
	{
		public string BusinessName { get; set; }

		public List<Product> Products { get; set; }
	}
}
