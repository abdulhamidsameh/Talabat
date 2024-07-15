using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;
using Talabat.Core.Specifications.ProductSpec;

namespace Talabat.Core.Services.Contract
{
	public interface IProductService
	{
		Task<ProductServiceReturn> GetProductsAsync(ProductSpecParams specParams);
		Task<Product?> GetProductAsync(int  productId);

		Task<IReadOnlyList<ProductBrand?>> GetProductBrands(); 
		Task<IReadOnlyList<ProductCategory?>> GetProductCategories();
	}
}
