using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
	public class CustomerBasketDto
	{
		[Required]
		public string Id { get; set; } = null!;
		public string? PaymentIntentId { get; set; } = null!;
		public string? ClientSecret { get; set; } = null!;

		public int? DeliveryMethodId { get; set; }

		public decimal ShippingPrice { get; set; }
		public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
    }
}
