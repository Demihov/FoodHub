using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodHub.Models
{
	public class BusinessUser: User
	{
		public string BusinessName { get; set; }
	}
}
