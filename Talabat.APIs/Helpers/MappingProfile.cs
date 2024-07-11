﻿using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<CustomerBasket,CustomerBasketDto>().ReverseMap();
			CreateMap<BasketItem,BasketItemDto>().ReverseMap();

			CreateMap<Product, ProductToReturnDto>()
				.ForMember(D => D.Brand, options => options.MapFrom(S => S.Brand.Name))
				.ForMember(D => D.Category, options => options.MapFrom(S => S.Category.Name))
				.ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
		}
	}
}
