using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{
    public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository,
			IMapper mapper)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;
		}

		[HttpGet]
		// baseUrl/Basket?id=basket01
		public async Task<ActionResult<CustomerBasketDto>> GetBasket(string id)
		{
			var basket = await _basketRepository.GetBasketAsync(id);
			if (basket is null)
				return Ok(new CustomerBasket(id));
		
			return Ok(basket);
		}

		[HttpPost]
		public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basketDto)
		{
			var basket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basketDto);
			var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(basket);
			if (createdOrUpdatedBasket is null)
				return BadRequest(new ApiResponse(400));
			return Ok(createdOrUpdatedBasket);
		}

		[HttpDelete]
		public async Task DeleteBasket(string id)
		{
			await _basketRepository.DeleteBasketAsync(id);
		}
	}
}
