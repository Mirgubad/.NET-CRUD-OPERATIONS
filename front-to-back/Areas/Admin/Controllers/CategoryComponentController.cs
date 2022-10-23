using front_to_back.Areas.Admin.ViewModels;
using front_to_back.DAL;
using front_to_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryComponentController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryComponentController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult>  Index()
        {

            var model = new CategoryIndexViewModel
            {
                Categories = await _appDbContext.Categories.ToListAsync(),
                CategoryComponents = await _appDbContext.CategoryComponents.ToListAsync()
            
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
           
            return View();
        }

        [HttpPost]  
        public async  Task<IActionResult> Create(CategoryComponent categoryComponent)
        {
            if(ModelState.IsValid) return View(categoryComponent);


            await _appDbContext.CategoryComponents.AddAsync(categoryComponent);

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");

        }
    }
}
