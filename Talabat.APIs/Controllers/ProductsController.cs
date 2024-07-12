using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpec;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IGenericRepository<ProductBrand> _brandsRepo;
		private readonly IGenericRepository<ProductCategory> _categoriesRepo;
		private readonly IMapper _mapper;

		public ProductsController(IGenericRepository<Product> productRepo,
			IGenericRepository<ProductBrand> brandsRepo,
			IGenericRepository<ProductCategory> categoriesRepo,
			IMapper mapper)
		{
			_productRepo = productRepo;
			_brandsRepo = brandsRepo;
			_categoriesRepo = categoriesRepo;
			_mapper = mapper;
		}

		// baseUrl/api/Products
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery] ProductSpecParams specParams)
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

			var products = await _productRepo.GetAllWithSpecAsync(spec);

			if (products is null)
				return NotFound(new ApiResponse(404, "Products Was Not Found"));
			var productsDto = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			var pagination = new Pagination<ProductToReturnDto>()
			{
				Data = productsDto,
				PageIndex = specParams.PageIndex,
				PageSize = specParams.PageSize,
				Count = spec.Count,
			};
			
			return Ok(pagination);
		}

		// baseUrl/api/Products/id
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>>? GetProduct(int id)
		{
			var spec = new BaseSpecifications<Product>(P => P.Id == id);
			spec.Includes.Add(P => P.Brand);
			spec.Includes.Add(P => P.Category);
			var product = await _productRepo.GetWithSpecAsync(spec);
			if (product is null)
				return NotFound(new ApiResponse(404, "Product Was Not Found"));

			var productDto = _mapper.Map<Product, ProductToReturnDto>(product);

			return Ok(productDto);

		}

		// baseUrl/api/products/brands
		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _brandsRepo.GetAllAsync();
			if (brands is null)
				return NotFound(new ApiResponse(404, "Brands Not Found"));
			return Ok(brands);
		}

		// baseUrl/api/products/categories
		[HttpGet("categories")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await _categoriesRepo.GetAllAsync();
			if (categories is null)
				return NotFound(new ApiResponse(404, "Categories Not Found"));
			return Ok(categories);
		}
	}
}
