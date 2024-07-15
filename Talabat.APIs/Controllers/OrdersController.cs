using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(IOrderService orderService,
			IMapper mapper)
        {
			_orderService = orderService;
			_mapper = mapper;
		}

		[HttpPost] // baseUrl/api/orders
		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var address = _mapper.Map<AddressDto,Address>(orderDto.ShippingAddress);
			var order = await _orderService.CreateOrderAsync(email!, orderDto.BasketId, orderDto.DeliveryMethodId, address);

			if (order is null)
				return BadRequest(new ApiResponse(400));

			return Ok(order);
		}

		[HttpGet] // baseUrl/api/Orders
		public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var orders = await _orderService.GetOrdersForUserAsync(email!);

			var selected = orders.Where(O => O.BuyerEmail == email);

			return Ok(selected);
		}


    }
}
