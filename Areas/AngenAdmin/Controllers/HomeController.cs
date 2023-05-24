using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.DAL;
using WebApplication8.Models;

namespace WebApplication8.Areas.AngenAdmin.Controllers
{
    [Area("AngenAdmin")]
    public class HomeController : Controller
	{
		private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
        {
			_context = context;
		}

        public IActionResult Index()
		{
			List<Product> products = _context.Products.Include(p => p.Category).ToList();
			return View(products);
		}
	}
}
