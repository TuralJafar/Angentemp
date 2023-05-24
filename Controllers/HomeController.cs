using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication8.DAL;
using WebApplication8.Models;
using WebApplication8.ViewModels;

namespace WebApplication8.Controllers
{   
    public class HomeController : Controller
    { private readonly AppDbContext _context;
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