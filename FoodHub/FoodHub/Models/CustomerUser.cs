using Microsoft.AspNetCore.Identity;

namespace FoodHub.Models
{
	public class CustomerUser: User
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

	}
}
