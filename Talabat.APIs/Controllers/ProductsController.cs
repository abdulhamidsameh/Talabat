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
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpec;
using Talabat.Core.UnitOfWork.Contract;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
	{
		private readonly IMapper _mapper;
		private readonly IProductService _productService;

		public ProductsController(IUnitOfWork unitOfWork,
			IMapper mapper,
			IProductService productService)
		{
			_mapper = mapper;
			_productService = productService;
		}

		// baseUrl/api/Products
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery] ProductSpecParams specParams)
		{
			var productsService = await _productService.GetProductsAsync(specParams);

			var products = productsService.Products;

			if (products is null)
				return NotFound(new ApiResponse(404, "Products Was Not Found"));
			var productsDto = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			var pagination = new Pagination<ProductToReturnDto>()
			{
				Data = productsDto,
				PageIndex = specParams.PageIndex,
				PageSize = specParams.PageSize,
				Count = productsService.Count,
			};
			
			return Ok(pagination);
		}

		// baseUrl/api/Products/id
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>>? GetProduct(int id)
		{
			var product = await _productService.GetProductAsync(id);
			if (product is null)
				return NotFound(new ApiResponse(404, "Product Was Not Found"));

			var productDto = _mapper.Map<Product, ProductToReturnDto>(product);

			return Ok(productDto);

		}

		// baseUrl/api/products/brands
		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _productService.GetProductBrands();
			if (brands is null)
				return NotFound(new ApiResponse(404, "Brands Not Found"));
			return Ok(brands);
		}

		// baseUrl/api/products/categories
		[HttpGet("categories")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await _productService.GetProductCategories();
			if (categories is null)
				return NotFound(new ApiResponse(404, "Categories Not Found"));
			return Ok(categories);
		}
	}
}
