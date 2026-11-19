using Microsoft.AspNetCore.Mvc;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly Data data;

        public UsersController(Data data)
        {
            this.data = data;
        }

        [AcceptVerbs("GET", "POST")]
        [Route("index")]
        public IActionResult Index([FromForm] string id)
        {
            if (id != "df2323eoT")
            {
                return Unauthorized();
            }
            return View(data.Users);
        }
    }
}
