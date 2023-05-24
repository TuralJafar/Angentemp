using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.DAL;
using WebApplication8.Models;
using WebApplication8.Utilities.Extensions;
using WebApplication8.ViewModels;


namespace WebApplication8.Areas.AngenAdmin.Controllers
{
    [Area("AngenAdmin")]
    public class ProductController : Controller
    { private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;

		public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context= context;
			_env = env;
		}
		public async Task<IActionResult> Index()
		{
			List<Product> products = await _context.Products.Include(p => p.Category).ToListAsync();
			return View(products);
		}
		public async Task<IActionResult> Create()
		{
			ViewBag.Categories = await _context.Categories.ToListAsync();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateProductVM createProductVM)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Categories = _context.Categories;
				return View();
			}
			if (!createProductVM.Photo.CheckFileType(createProductVM.Photo.ContentType))
			{

				ViewBag.Categories = _context.Categories;
				ModelState.AddModelError("Photo", "Fayl formata uygun deyil");
				return View();
			}
			if (!createProductVM.Photo.CheckFileSize(200))
			{
				ModelState.AddModelError("Photo", "Fayl'in hecmi uygun deyil");
				return View();
			}
			bool result = await _context.Categories.AnyAsync(p => p.Id == createProductVM.CategoryId);
			if (!result)
			{
				ViewBag.Categories = _context.Categories;
				ModelState.AddModelError("CategoryId", "Bele bir id'li category yoxdur");
				return View();
			}
			Product product = new Product()
			{
				Name = createProductVM.Name,
				CategoryId = createProductVM.CategoryId,
				Image = await createProductVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/portfolio")
			};
			product.Image = await createProductVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/portfolio");
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Update(int id)
		{
			if (id == null || id < 1) return BadRequest();
			Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (product == null) return NotFound();
			UpdateProductVM updateProductVM = new UpdateProductVM()
			{
				Name = product.Name,
				CategoryId = product.CategoryId,
				Image = product.Image,

			};
			ViewBag.Categories = _context.Categories;
			return View(updateProductVM);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, UpdateProductVM updateProductVM)
		{
			if (id == null || id < 1) return BadRequest();
			Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (product == null) return NotFound();
			bool result = await _context.Categories.AnyAsync(p => p.Id == updateProductVM.CategoryId);
			if (!result)
			{
				ViewBag.Categories = _context.Categories;
				ModelState.AddModelError("CategoryId", "Bele bir id'li category yoxdur");
				updateProductVM.Image = product.Image;
				return View(updateProductVM);
			}
			if (updateProductVM.Photo != null)
			{
				if (updateProductVM.Photo.CheckFileType(updateProductVM.Photo.ContentType))
				{
					ViewBag.Categories = _context.Categories;
					ModelState.AddModelError("Photo", "Fayl tipi uygun deyil");
					updateProductVM.Image = product.Image;
					return View(updateProductVM);
				}

				if (!updateProductVM.Photo.CheckFileSize(200))
				{
					ViewBag.Categories = _context.Categories;
					ModelState.AddModelError("Photo", "Fayl'in hecmi uygun deyil");
					updateProductVM.Image = product.Image;
					return View(updateProductVM);
				}
				product.Image.DeleteFile(_env.WebRootPath, "assets/img/portfolio");
				product.Image = await updateProductVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/portfolio");
			}
			product.Name = updateProductVM.Name;
			product.CategoryId = updateProductVM.CategoryId;
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || id < 1) return BadRequest();
			Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (product == null) return NotFound();
			product.Image.DeleteFile(_env.WebRootPath, "assets/img/portfolio");
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}
