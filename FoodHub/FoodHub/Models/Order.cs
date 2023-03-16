namespace FoodHub.Models
{
	public class Order
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }

		public string UserId { get; set; }
		public virtual CustomerUser User { get; set; }

		public string BusinessUserId { get; set; }
		public virtual BusinessUser BusinessUser { get; set; }

		public virtual List<OrderItem> OrderItems { get; set; }
	}
}
