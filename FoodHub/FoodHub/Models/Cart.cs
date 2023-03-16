namespace FoodHub.Models
{
	public class Cart
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }

		public string UserId { get; set; }
		public virtual User User { get; set; }

		public virtual List<CartItem> CartItems { get; set; }
	}
}
