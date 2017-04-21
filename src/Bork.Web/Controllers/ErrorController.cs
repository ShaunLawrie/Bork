using Microsoft.AspNetCore.Mvc;

namespace Bork.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View("Error");
        }
    }
}
