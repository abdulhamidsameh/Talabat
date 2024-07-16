using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	public class PaymentController : BaseApiController
	{
		private readonly IPaymentService _paymentService;
		private readonly ILogger<PaymentController> _logger;
		private const string webHookSecrte = "whsec_f971b0ec6742427eb9ee2ce5278deb16ea0c51969b4a3d99640865e5a3c5746c";

		public PaymentController(IPaymentService paymentService,
			ILogger<PaymentController> logger)
		{
			_paymentService = paymentService;
			_logger = logger;
		}

		[HttpGet("{basketId}")]
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);

			if (basket is null)
				return BadRequest(new ApiResponse(400, "Basket Not Found"));

			return Ok(basket);
		}

		[HttpPost("{state}")]
		public async Task<IActionResult> WebHook(string state)
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			var stripeEvent = EventUtility.ConstructEvent(json,
				Request.Headers["Stripe-Signature"], webHookSecrte);

			var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
			// Handle the event

			Order? order;

			//switch (stripeEvent.Type)
			//{
			//	case Events.PaymentIntentSucceeded:

			//		order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);

			//		_logger.LogInformation("Order is Succeded {0}", order?.PaymentIntentId);
			//		_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);

			//		break;
			//	case Events.PaymentIntentPaymentFailed:

			//		order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
			//		_logger.LogInformation("Order is Faild {0}", order?.PaymentIntentId);
			//		_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);

			//		break;
			//	default:
			//		break;
			//}

			switch (state)
			{
				case "success":

					order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);

					_logger.LogInformation("Order is Succeded {0}", order?.PaymentIntentId);
					_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);

					break;
				case "failed":

					order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
					_logger.LogInformation("Order is Faild {0}", order?.PaymentIntentId);
					_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);

					break;
				default:
					break;
			}

			return Ok();
		}

	}
}
