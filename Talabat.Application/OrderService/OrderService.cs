using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.UnitOfWork.Contract;

namespace Talabat.Application.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymentService _paymentService;

		public OrderService(IBasketRepository basketRepo,
			IUnitOfWork unitOfWork,
			IPaymentService paymentService)
		{
			_basketRepository = basketRepo;
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
		}
		public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			// 1.Get Basket From Baskets Repo

			var basket = await _basketRepository.GetBasketAsync(basketId);

			var orderItems = new List<OrderItem>();
			// 2. Get Selected Items at Basket From Products Repo

			if (basket?.Items?.Count > 0)
			{
				var productRepository = _unitOfWork.Repository<Product>();
				foreach (var item in basket.Items)
				{
					var product = await productRepository.GetAsync(item.Id);
					if (product is not null)
					{
						var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl); ;

						var order = new OrderItem(productItemOrdered, product.Price, item.Quantity);
						orderItems.Add(order);
					}

				}
			}

			// 3. Calculate SubTotal

			var subTotal = orderItems.Sum(O => O.Quantity * O.Price);

			var orderRepository = _unitOfWork.Repository<Order>();

			// 4. Get Delivery Method From DeliveryMethods Repo

			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

			var existingOrder = await orderRepository.GetWithSpecAsync(new BaseSpecifications<Order>(O => O.PaymentIntentId == basket!.PaymentIntentId));

			if(existingOrder is not null)
			{
				orderRepository.Delete(existingOrder);
				await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);
			}

			// 5. Create Order

			var oredr = new Order(
				
				buyerEmail: buyerEmail,
				shippingAddress: shippingAddress,
				subTotal: subTotal,
				deliveryMethod:deliveryMethod,
				items: orderItems,
				paymentIntentId: basket!.PaymentIntentId!
				
				);

			// 6. Save To Database [TODO]

			orderRepository.Add(oredr);

			var result = await _unitOfWork.CompleteAsync();
			if (result <= 0)
				return null!;

			return oredr;

		}

		public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
			=> await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(new BaseSpecifications<Order>(O => O.BuyerEmail == buyerEmail));

		public async Task<Order?> GetOrderbyIdForUserAsync(string buyerEmail, int orderId)
			=> await _unitOfWork.Repository<Order>().GetWithSpecAsync(new BaseSpecifications<Order>(O => O.BuyerEmail == buyerEmail && O.Id == orderId));

		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
			=> await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

		

		
	}
}
