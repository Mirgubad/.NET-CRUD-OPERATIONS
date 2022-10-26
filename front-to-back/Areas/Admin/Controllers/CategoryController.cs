using front_to_back.Areas.Admin.ViewModels;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using front_to_back.Migrations;
using front_to_back.Models;
using front_to_back.ViewModels.Home;
using Xunit.Sdk;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Immutable;

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
                Categories = await _appDbContext.Categories.ToListAsync(),
                CategoryComponents = await _appDbContext.CategoryComponents.ToListAsync()
            };

            return View(model);
        }

        #region Create
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
                ModelState.AddModelError("Title", "This name already in use");
                return View(category);
            }
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion


        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (!ModelState.IsValid) return View(category);

            if (id != category.Id) return BadRequest();

            bool exists = await _appDbContext.Categories
                .AnyAsync(ct => ct.Title == category.Title && ct.Id != category.Id);

            if (exists)
            {
                ModelState.AddModelError("Title", "This name already taken");
                return View(category);
            }

            var dbcategory = await _appDbContext.Categories.FindAsync(id);
            if (dbcategory == null) return NotFound();

            dbcategory.Title = category.Title;

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }
        #endregion


        #region Details
        [HttpGet]
        public async Task<IActionResult>Details(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);

            if (category==null) return NotFound();

            return View(category);
        }

        #endregion


        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _appDbContext.Categories.Remove(category);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion
    }
}
