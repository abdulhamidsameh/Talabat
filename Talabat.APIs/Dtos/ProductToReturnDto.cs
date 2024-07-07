using Talabat.Core.Entities;

namespace Talabat.APIs.Dtos
{
	public record ProductToReturnDto
	(
		 int Id,
		 string Name,
		 string Description,
		 string PictureUrl,
		 decimal Price,
		 int? BrandId,
		 string Brand,
		 int? CategoryId,
		 string Category
	);
}
