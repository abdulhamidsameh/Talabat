using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
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
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
			var order = await _orderService.CreateOrderAsync(email!, orderDto.BasketId, orderDto.DeliveryMethodId, address);

			if (order is null)
				return BadRequest(new ApiResponse(400));

			var orderToReturnDto = _mapper.Map<OrderToReturnDto>(order);

			return Ok(orderToReturnDto);
		}

		[HttpGet] // baseUrl/api/Orders
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var orders = await _orderService.GetOrdersForUserAsync(email!);

			var selected = orders.Where(O => O.BuyerEmail == email);

			var orderDto = _mapper.Map<IEnumerable<OrderToReturnDto>>(selected);

			return Ok(orderDto);
		}

		[HttpGet("{id}")] // baseUrl/api/Orders/1

		public async Task<ActionResult<OrderToReturnDto>> GetOrder(int id)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var order = await _orderService.GetOrderbyIdForUserAsync(email!, id);
			if (order is null) return BadRequest(new ApiResponse(400));
			if (order.BuyerEmail != email) return BadRequest(new ApiResponse(400));

			var orderDto = _mapper.Map<OrderToReturnDto>(order);

			return Ok(orderDto);
		}

		[HttpGet("deliveryMethods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
			return Ok(deliveryMethods);
		}

	}
}
