using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;

namespace Talabat.APIs.Helpers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
			CreateMap<BasketItem, BasketItemDto>().ReverseMap();
			CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

			CreateMap<Core.Entities.Order_Aggregate.Address, AddressDto>().ReverseMap();

			CreateMap<Product, ProductToReturnDto>()
				.ForMember(D => D.Brand, options => options.MapFrom(S => S.Brand.Name))
				.ForMember(D => D.Category, options => options.MapFrom(S => S.Category.Name))
				.ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

			CreateMap<ApplicationUser, UserDto>().ReverseMap();

			CreateMap<OrderToReturnDto, Order>().ReverseMap()
				.ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod!.ShortName))
				.ForMember(d => d.DeliveryMethodCoast, O => O.MapFrom(s => s.DeliveryMethod!.Cost));
		
			CreateMap<OrderItemDto, OrderItem>().ReverseMap()
				.ForMember(d => d.ProductName,O=> O.MapFrom(s => s.Product.ProductName))
				.ForMember(d => d.ProductId,O=> O.MapFrom(s => s.Product.ProductId))
				.ForMember(d => d.PictureUrl,O=> O.MapFrom(s => s.Product.PictureUrl))
				.ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>()); 
		
		}
	}
}
