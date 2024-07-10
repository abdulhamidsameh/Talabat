using Talabat.Core.Entities;

namespace Talabat.APIs.Dtos
{
	public class ProductToReturnDto
	{
        public int Id { get; set; }
        public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string PictureUrl { get; set; } = null!;
		public decimal Price { get; set; }
		public int? BrandId { get; set; }
		public virtual string Brand { get; set; } = null!;
		public int? CategoryId { get; set; }
		public virtual string Category { get; set; } = null!;
	}
}
