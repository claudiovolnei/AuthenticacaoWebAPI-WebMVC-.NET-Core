using Microsoft.AspNetCore.Mvc;
namespace LuizaLabs.Application.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            ViewData["Message"] = "Your welcome message";

            return View();
        }
    }
}