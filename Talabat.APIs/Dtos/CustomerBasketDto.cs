using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
	public class CustomerBasketDto
	{
		[Required]
		public string Id { get; set; } = null!;
        public int? DeliveryMethodId { get; set; }
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
    }
}
