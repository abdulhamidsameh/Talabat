using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.UnitOfWork.Contract;

namespace Talabat.Application.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;

		public OrderService(IBasketRepository basketRepo,
			IUnitOfWork unitOfWork)
		{
			_basketRepository = basketRepo;
			_unitOfWork = unitOfWork;
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


			// 4. Get Delivery Method From DeliveryMethods Repo

			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

			// 5. Create Order

			var oredr = new Order(
				
				buyerEmail: buyerEmail,
				shippingAddress: shippingAddress,
				subTotal: subTotal,
				deliveryMethod:deliveryMethod,
				items: orderItems
				
				);

			// 6. Save To Database [TODO]

			_unitOfWork.Repository<Order>().Add(oredr);

			var result = await _unitOfWork.CompleteAsync();
			if (result <= 0)
				return null!;

			return oredr;

		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderbyIdForUserAsync(string buyerEmail, int orderId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
