using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Infrastructure.Data;

namespace Talabat.APIs.Controllers
{
	public class BugyController : BaseApiController
	{

		// baseUrl/bugy/NotFound
		[HttpGet("notfound")]
		public ActionResult NotFoundRequest()
		{
			return NotFound();
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
			return BadRequest();
		}

		// baseUrl/bugy/badrequest/five
		[HttpGet("badrequest/{id}")]
		public ActionResult GetBadRequest(int id)
		{
			return Ok();
		}
    }
}
