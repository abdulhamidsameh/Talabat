using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Infrastructure.Data;

namespace Talabat.APIs.Controllers
{
	public class BugyController : BaseApiController
	{

		// baseUrl/bugy/NotFound
		[HttpGet("notfound")]
		public ActionResult NotFoundRequest()
		{
			return NotFound(new ApiResponse(404));
		}

		// baseUrl/bugy/serverError
		[HttpGet("servererror")]
		public ActionResult ServerErrorRequest()
		{
			int X = 5;
			int Y = 0;
			int A = X /Y;
			
			return Ok(A);
		}

		// baseUrl/bugy/badrequest
		[HttpGet("badrequest")]
		public ActionResult GetBadRequest()
		{
			return BadRequest(new ApiResponse(400));
		}

		// baseUrl/bugy/badrequest/five
		[HttpGet("badrequest/{id}")]
		public ActionResult GetBadRequest(int id)
		{
			return Ok(new ApiResponse(401));
		}

		// baseUrl/bugy/unauthorized
		[HttpGet("unauthorized")]
		public ActionResult GetUnauthorized()
		{
			return Unauthorized(new ApiResponse(401));
		}
	}
}
