using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productRepo;

		public ProductsController(IGenericRepository<Product> productRepo)
        {
			_productRepo = productRepo;
		}

		// baseUrl/api/Products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var spec = new BaseSpecifications<Product>();
			spec.Includes.Add(P => P.Brand);
			spec.Includes.Add(P => P.Category);
			var products = await _productRepo.GetAllWithSpecAsync(spec);

			if(products is null)
				return NotFound();

			return Ok(products);
		}

		// baseUrl/api/Products/id
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>>? GetProduct(int id)
		{ 
			var spec = new BaseSpecifications<Product>(P => P.Id == id);
			spec.Includes.Add(P => P.Brand);
			spec.Includes.Add(P => P.Category);
			var product = await _productRepo.GetWithSpecAsync(spec);
			if(product is null)
				return NotFound();

			return Ok(product);

		}

    }
}
