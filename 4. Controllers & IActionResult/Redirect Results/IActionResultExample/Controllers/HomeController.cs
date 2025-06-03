using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("bookstore")]

        public IActionResult Index()
        {
            if (!Request.Query.ContainsKey("bookid"))
            {
                return BadRequest("Book id is not supplied");
            }

            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                return BadRequest("Book id can't be null or empty");
            }

            int bookId = Convert.ToInt16(ControllerContext.HttpContext.Request.Query["bookid"]);
            if (bookId <= 0)
            {
                return BadRequest("Book id can't be less than or equal to zero");
            }
            if (bookId > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }

            if (Convert.ToBoolean(Request.Query["isloggedin"]) == false)
            {
                return StatusCode(401);
            }

            return new LocalRedirectResult($"store/books/{bookId}", true);
        }
    }
}
