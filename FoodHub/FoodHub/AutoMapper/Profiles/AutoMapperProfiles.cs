using AutoMapper;
using FoodHub.Models;
using FoodHub.Models.DTO;

namespace FoodHub.AutoMapper.Profiles
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<Product, ProductDto>();
			CreateMap<ProductDto, Product>();

			//CreateMap<CartItem, CartItemDTO>();
			//CreateMap<CartItemDTO, CartItem>();

			//CreateMap<OrderItem, OrderItemDTO>();
			//CreateMap<OrderItemDTO, OrderItem>();

			//CreateMap<Order, OrderDTO>();
			//CreateMap<OrderDTO, Order>();
		}
	}
}
