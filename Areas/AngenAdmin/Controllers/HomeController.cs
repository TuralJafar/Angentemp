using Microsoft.AspNetCore.Mvc;

namespace WebApplication8.Areas.AngenAdmin.Controllers
{
    [Area("AngenAdmin")]
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
