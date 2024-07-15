using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
	public class OrderToReturnDto
	{
		public int Id { get; set; }
		public string BuyerEmail { get; set; } = null!;
		public DateTimeOffset OrderDate { get; set; }

		public string Status { get; set; } = null!;

		public Address ShippingAddress { get; set; } = null!;

		public decimal SubTotal { get; set; }

		public string PaymentIntentId { get; set; } = string.Empty;

		public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();

		public string DeliveryMethod { get; set; } = null!;
		public decimal DeliveryMethodCoast { get; set; }

		public decimal Total { get; set; }
	}
}
