using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	public class PaymentController : BaseApiController
	{
		private readonly IPaymentService _paymentService;

		public PaymentController(IPaymentService paymentService)
        {
			_paymentService = paymentService;
		}

		[HttpGet("{basketId}")]
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);

			if (basket is null)
				return BadRequest(new ApiResponse(400,"Basket Not Found"));

			return Ok(basket);
		}

    }
}
