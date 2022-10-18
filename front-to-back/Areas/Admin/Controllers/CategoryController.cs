using front_to_back.Areas.Admin.ViewModels;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using front_to_back.Migrations;
using front_to_back.Models;
using front_to_back.ViewModels.Home;


namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new CategoryIndexViewModel
            {
                Categories = await _appDbContext.Categories.ToListAsync()
            };

            return View(model); 
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            bool isExist = await _appDbContext.Categories
                .AnyAsync(c => c.Title.ToLower().Trim() == category.Title.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Title", "This name already exists");
                return View(category);
            }
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");

        }
    }
}
