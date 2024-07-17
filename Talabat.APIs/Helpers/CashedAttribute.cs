using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Application.CasheService;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Helpers
{
	public class CashedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timeToLiveInSeconds;

		public CashedAttribute(int timeToLiveInSeconds)
        {
			_timeToLiveInSeconds = timeToLiveInSeconds;
		}
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var responseCasheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCasheService>();

			var key = GenerateCasheKeyFromRequest(context.HttpContext.Request);

			var response = await responseCasheService.GetCashedResponseAsync(key);

			if (!string.IsNullOrEmpty(response))
			{
				var result = new ContentResult()
				{
					Content = response,
					ContentType = "application/json",
					StatusCode = 200
				};

				context.Result = result;
				return;
			}

			var executedActionContext = await next.Invoke();

			if(executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
			{
				await responseCasheService.CasheResponseAsync(key, okObjectResult.Value,TimeSpan.FromSeconds(_timeToLiveInSeconds));
			}

		}

		private string GenerateCasheKeyFromRequest(HttpRequest request)
		{
			var keyBuilder = new StringBuilder();

			keyBuilder.Append(request.Path);
			foreach (var queryParam in request.Query.OrderBy(q => q.Key))
				keyBuilder.Append($"?{queryParam.Key}={queryParam.Value}");

			return keyBuilder.ToString();
		}
	}
}
