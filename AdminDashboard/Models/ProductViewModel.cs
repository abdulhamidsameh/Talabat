using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Product;

namespace AdminDashboard.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }=null!;

        public IFormFile Image { get; set; } = null!;

        public string PictureUrl { get; set; } = null!;

        [Required(ErrorMessage = "Price is Required")]
        [Range(1, 100000)]
        public decimal Price { get; set; }


		public int? BrandId { get; set; }
		public virtual ProductBrand? Brand { get; set; } = null!;

	}
}
