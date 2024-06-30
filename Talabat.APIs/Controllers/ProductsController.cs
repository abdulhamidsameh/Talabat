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


    }
}
