﻿namespace FoodHub.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string ImageUrl { get; set; }

		public string BusinessId { get; set; }
		public BusinessUser Business { get; set; }
	}
}
