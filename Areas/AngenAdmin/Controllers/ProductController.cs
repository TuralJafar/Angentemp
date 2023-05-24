using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.DAL;
using WebApplication8.Models;

namespace WebApplication8.Areas.AngenAdmin.Controllers
{
    [Area("AngenAdmin")]
    public class ProductController : Controller
    { private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context= context;
        }
        public async Task<IActionResult> Index()
        {   
          List<Product>Products=await _context.Products.Include(p=>p.Category).ToListAsync();
            return View(Products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Create(Product product)
        {
            if (!ModelState.IsValid) return View();
            bool result = _context.Products.Any(p => p.Name.Trim().ToLower() == product.Category.Name.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda categoriya artiq movcuddur");
                return View();
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
            
        }
        public IActionResult Update()
        {
            return View();
        }
        [HttpPost]
        //public async Task<IActionResult> Update(int? id)
        //{
        //    if (!ModelState.IsValid) return View();

        //}
        public IActionResult Delete()
        {
            return View();
        }
    }
}
