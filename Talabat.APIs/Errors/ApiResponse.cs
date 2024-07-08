
namespace Talabat.APIs.Errors
{
	public class ApiResponse
	{
		public int StatusCode { get; set; }
		public string? Message { get; set; }
		public ApiResponse(int statusCode, string? message = null)
		{
			StatusCode = statusCode;
			Message = message ?? GetDefaultMessageForStatusCode(statusCode);
		}

		private string? GetDefaultMessageForStatusCode(int statusCode)
			=> statusCode switch
			{
				400 => "A Bad Request, You Have Made",
				401 => "Unauthorized, You Are Not",
				404 => "Resource Was Not Found",
				500 => "Server Error",
				_ => null,
			};
	}
}
