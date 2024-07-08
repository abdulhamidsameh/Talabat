using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILoggerFactory _logger;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next,ILoggerFactory logger, IWebHostEnvironment env )
        {
			_next = next;
			_logger = logger;
			_env = env;
		}
        public async Task InvokeAsync(HttpContext context)
		{


			try
			{
				await _next.Invoke(context);
			}
			catch (Exception ex)
			{
				// 1. LogException

				var loger = _logger.CreateLogger<ExceptionMiddleware>();
				loger.LogError(ex.Message);

				context.Response.StatusCode = 500;
				context.Response.ContentType = "application/json";

				var response = _env.IsDevelopment()? new ApiExceptionResponse(500,ex.Message,ex.StackTrace?.ToString()) : 
					new ApiExceptionResponse(500);

				var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

				var json = JsonSerializer.Serialize(response, options);

				await context.Response.WriteAsync(json);

				
			}

			

		}
	}
}
