using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}