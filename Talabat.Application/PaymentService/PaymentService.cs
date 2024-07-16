using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.UnitOfWork.Contract;

namespace Talabat.Application.PaymentService
{
	public class PaymentService : IPaymentService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IConfiguration _configuration;
		private readonly IUnitOfWork _unitOfWork;

		public PaymentService(IBasketRepository basketRepository,
			IConfiguration configuration,
			IUnitOfWork unitOfWork)
		{
			_basketRepository = basketRepository;
			_configuration = configuration;
			_unitOfWork = unitOfWork;
		}
		public async Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string basketId)
		{
			StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

			var basket = await _basketRepository.GetBasketAsync(basketId);

			if (basket is null)
				return null!;

			var productRepository = _unitOfWork.Repository<Core.Entities.Product.Product>();

			if (basket.Items.Count > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = await productRepository.GetAsync(item.Id);
					if (product is not null)
						item.Price = product.Price;
				}
			}



			var shippingPrice = 0M;

			if (basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
				shippingPrice = deliveryMethod!.Cost;
				basket.ShippingPrice = shippingPrice;
			}

			PaymentIntent paymentIntent;
			PaymentIntentService paymentIntentService = new PaymentIntentService();

			if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create
			{
				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long)(basket.Items.Sum(i => i.Price * i.Quantity) + shippingPrice) * 100,
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card" }
				};

				paymentIntent = await paymentIntentService.CreateAsync(options); // Integration with Stripe

				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;
			}
			else // Update
			{
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)(basket.Items.Sum(i => i.Price * i.Quantity) + shippingPrice) * 100,
				};

				await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
			}

			await _basketRepository.UpdateBasketAsync(basket);

			return basket;
		}

		public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool IsPaid)
		{
			var orderRepository = _unitOfWork.Repository<Order>();
			var order = await orderRepository.GetWithSpecAsync(new BaseSpecifications<Order>(O => O.PaymentIntentId == paymentIntentId));

			if (order is null)
				return null;

			if (IsPaid)
				order.Status = OrderStatus.PaymentReceived;
			else
				order.Status = OrderStatus.PaymentFailed;

			orderRepository.Update(order);

			await _unitOfWork.CompleteAsync();

			return order;

		}
	}
}
