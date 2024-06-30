using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

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
			var products = await _productRepo.GetAllAsync();

			if(products is null)
				return NotFound();

			return Ok(products);
		}

		// baseUrl/api/Products/id
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>>? GetProduct(int id)
		{ 
			var product = await _productRepo.GetAsync(id);
			if(product is null)
				return NotFound();

			return Ok(product);

		}

    }
}
