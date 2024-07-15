using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpec;
using Talabat.Core.UnitOfWork.Contract;

namespace Talabat.Application.ProductService
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}

		public async Task<Product?> GetProductAsync(int productId)
		{
			var spec = new BaseSpecifications<Product>(P => P.Id == productId);
			spec.Includes.Add(P => P.Brand);
			spec.Includes.Add(P => P.Category);
			return await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
		}

		public async Task<IReadOnlyList<ProductBrand?>> GetProductBrands()
		{
			return await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
		}

		public async Task<IReadOnlyList<ProductCategory?>> GetProductCategories()
		{
			return await _unitOfWork.Repository<ProductCategory>().GetAllAsync();

		}

		public async Task<ProductServiceReturn> GetProductsAsync(ProductSpecParams specParams)
		{
			var spec = new BaseSpecifications<Product>();
			spec.Criteria = P =>
			(string.IsNullOrEmpty(specParams.Search) || P.Name.Contains(specParams.Search)) &&
			(!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
			(!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value);

			spec.Take = specParams.PageSize;
			spec.Skip = specParams.PageSize * (specParams.PageIndex - 1);

			spec.Includes.Add(P => P.Brand);
			spec.Includes.Add(P => P.Category);

			switch (specParams.Sort)
			{
				case "priceAsc":
					spec.OrderBy = P => P.Price;
					break;
				case "priceDesc":
					spec.OrderByDesc = P => P.Price;
					break;
				case "nameDesc":
					spec.OrderByDesc = P => P.Name;
					break;
				default:
					spec.OrderBy = P => P.Name;
					break;
			}
			var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
			return new ProductServiceReturn() { Products = products ,Count=spec.Count};
		}
	}
}
