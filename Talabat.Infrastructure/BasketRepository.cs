using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Infrastructure
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;

		public BasketRepository(IConnectionMultiplexer redis)
        {
			_database = redis.GetDatabase();
		}
        public async Task<bool> DeleteBasketAsync(string BasketId)
			=> await _database.KeyDeleteAsync(BasketId);

		public async Task<CustomerBasket?> GetBasketAsync(string basketId)
		{
			var basket = await _database.StringGetAsync(basketId);
			if (basket.IsNullOrEmpty)
				return null;
			var basketSerilize = JsonSerializer.Deserialize<CustomerBasket>(basket!);
			return basketSerilize;
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
		{
			var basketJson = JsonSerializer.Serialize(basket);
			var createdOrUpdated = await _database.StringSetAsync(basket.Id, basketJson, TimeSpan.FromDays(30));
			if (!createdOrUpdated) return null;
			return await GetBasketAsync(basket.Id);
		}
	}
}
