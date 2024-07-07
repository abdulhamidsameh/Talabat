using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IMapper _mapper;

		public ProductsController(IGenericRepository<Product> productRepo,
			IMapper mapper)
		{
			_productRepo = productRepo;
			_mapper = mapper;
		}

		// baseUrl/api/Products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var spec = new BaseSpecifications<Product>();
			spec.Includes.Add(P => P.Brand);
			spec.Includes.Add(P => P.Category);
			var products = await _productRepo.GetAllWithSpecAsync(spec);

			if (products is null)
				return NotFound(new ApiResponse(404, "Products Was Not Found"));
			var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);
			return Ok(productsDto);
		}

		// baseUrl/api/Products/id
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>>? GetProduct(int id)
		{
			//var spec = new BaseSpecifications<Product>(P => P.Id == id);
			//spec.Includes.Add(P => P.Brand);
			//spec.Includes.Add(P => P.Category);
			var product = await _productRepo.GetAsync(id);
			if (product is null)
				return NotFound(new ApiResponse(404, "Product Was Not Found"));

			var productDto = _mapper.Map<Product, ProductToReturnDto>(product);

			return Ok(productDto);

		}

	}
}
