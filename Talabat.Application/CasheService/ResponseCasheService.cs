using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.CasheService
{
	public class ResponseCasheService : IResponseCasheService
	{
		private readonly IDatabase _database;

		public ResponseCasheService(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase();
		}
		public async Task CasheResponseAsync(string key, object response, TimeSpan timeToLive)
		{
			if (response is null)
				return;

			var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

			var responseJson = JsonSerializer.Serialize(response, jsonSerializerOptions);

			await _database.StringSetAsync(key, responseJson, timeToLive);

		}

		public async Task<string?> GetCashedResponseAsync(string key)
		{
			var response = await _database.StringGetAsync(key);

			if (response.IsNullOrEmpty)
				return null;

			return response;

		}
	}
}
