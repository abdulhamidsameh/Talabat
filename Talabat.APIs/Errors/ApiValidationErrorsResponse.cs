namespace Talabat.APIs.Errors
{
	public class ApiValidationErrorsResponse : ApiResponse
	{
		public IEnumerable<string> Errors { get; set; }
		public ApiValidationErrorsResponse() : base(400)
		{
			Errors = new List<string>();
		}
	}
}
