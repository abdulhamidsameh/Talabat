using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;

namespace Talabat.Infrastructure.Data
{
    public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreContext _dbContext)
		{
			if (_dbContext.ProductBrands.Count() == 0)
			{
				var brandsData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
				if (brands?.Count > 0)
					foreach (var brand in brands)
						_dbContext.Set<ProductBrand>().Add(brand);
			}
			if (_dbContext.ProductCategories.Count() == 0)
			{
				var catgoryData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(catgoryData);
				if (categories?.Count > 0)
					foreach (var category in categories)
						_dbContext.Set<ProductCategory>().Add(category);
			}
			if (_dbContext.Products.Count() == 0)
			{
				var productData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productData);
				if (products?.Count > 0)
					foreach (var product in products)
						_dbContext.Set<Product>().Add(product);
			}

			if (_dbContext.DeliveryMethods.Count() == 0)
			{
				var deliveryData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/delivery.json");
				var deliveres = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
				if (deliveres?.Count > 0)
					foreach (var delivery in deliveres)
						_dbContext.Set<DeliveryMethod>().Add(delivery);
			}

			await _dbContext.SaveChangesAsync();

		}
	}
}
