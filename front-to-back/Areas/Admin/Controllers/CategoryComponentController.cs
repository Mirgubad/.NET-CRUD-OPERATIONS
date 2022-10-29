using front_to_back.Areas.Admin.ViewModels;
using front_to_back.Areas.Admin.ViewModels.Category;
using front_to_back.DAL;
using front_to_back.Helpers;
using front_to_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryComponentController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public CategoryComponentController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {

            var model = new CategoryIndexViewModel
            {
                CategoryComponents = await _appDbContext.CategoryComponents.Include(ct => ct.Category).ToListAsync()
            };

            return View(model);
        }

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CategoryComponentCreateViewModel
            {
                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                })
                .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryComponentCreateViewModel model)
        {
            model.Categories = await _appDbContext.Categories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            })
                .ToListAsync();

            if (!_fileService.CheckFile(model.Photo))
            {
                ModelState.AddModelError("Photo", "Incorrect File");
                return View(model);
            }

            int maxSize = 1000;
            if (!_fileService.MaxSize(model.Photo, maxSize))
            {
                ModelState.AddModelError("Photo", $"Photo size must be less {maxSize}kb ");
                return View(model);
            }

            if (!ModelState.IsValid) return View(model);
            var categoryComponents = new CategoryComponent
            {
                Title = model.Title,
                Description = model.Description,
                FilePath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath),
                CategoryId = model.CategoryId
            };
            await _appDbContext.CategoryComponents.AddAsync(categoryComponents);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int id)
        {
            var categoryComponent = await _appDbContext.CategoryComponents.FindAsync(id);
            if (categoryComponent == null) return NotFound();

            var model = new CategoryComponentUpdateViewModel
            {
                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                })
               .ToListAsync(),
                Id = categoryComponent.Id,
                CategoryId = categoryComponent.CategoryId,
                Description = categoryComponent.Description,
                FilePath = categoryComponent.FilePath,
                Title = categoryComponent.Title,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryComponentUpdateViewModel model)
        {
            model.Categories = await _appDbContext.Categories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()

            }).ToListAsync();

            var dbcategoryComponent = await _appDbContext.CategoryComponents.FindAsync(id);
            if (dbcategoryComponent == null) return NotFound();

            bool isExists = await _appDbContext.CategoryComponents
                .AnyAsync(cc => cc.Title.ToLower().Trim() == model.Title.ToLower().Trim()
                && id != dbcategoryComponent.Id);

            if (isExists)
            {
                ModelState.AddModelError("Title", "This category already created");

                return View(model);
            }

            if (id != model.Id) BadRequest();
            if (!ModelState.IsValid) return View(model);

            if (model.Photo != null)
            {
                if (!_fileService.CheckFile(model.Photo))
                {
                    ModelState.AddModelError("Photo", "Incorrect file");
                    return View(model);
                }
                int maxSize = 1000;
                if (!_fileService.MaxSize(model.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"Photo size must be less {maxSize}kb");
                    return View(model);
                }
                _fileService.Delete(_webHostEnvironment.WebRootPath, dbcategoryComponent.FilePath);
                dbcategoryComponent.FilePath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }

            var category = await _appDbContext.Categories.FindAsync(model.CategoryId);
            if (category == null) return NotFound();

            dbcategoryComponent.CategoryId = model.CategoryId;

            dbcategoryComponent.Title = model.Title;
            dbcategoryComponent.Description = model.Description;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dbcataegoryComponent = await _appDbContext.CategoryComponents.FindAsync(id);
            if (dbcataegoryComponent == null) return NotFound();

            var model = new CategoryComponentDetailsViewModel
            {
                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                })
               .ToListAsync(),
                Id = dbcataegoryComponent.Id,
                Title = dbcataegoryComponent.Title,
                Description = dbcataegoryComponent.Description,
                FilePath = dbcataegoryComponent.FilePath,
                CategoryId = dbcataegoryComponent.CategoryId,

            };
            return View(model);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dbcategoryComponent = await _appDbContext.CategoryComponents.FindAsync(id);
            if (dbcategoryComponent == null) return NotFound();
            if (id != dbcategoryComponent.Id) return BadRequest();

            var model = new CategoryComponentDeleteViewModel
            {
                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                })
               .ToListAsync(),
                Id = dbcategoryComponent.Id,
                Title = dbcategoryComponent.Title,
                Description = dbcategoryComponent.Description,
                CategoryId = dbcategoryComponent.CategoryId,
                FilePath = dbcategoryComponent.FilePath,


            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> DeleteComponent(int id)
        {
            var dbcategoryComponent = await _appDbContext.CategoryComponents.FindAsync(id);

            if (dbcategoryComponent == null) return NotFound();

            if (id != dbcategoryComponent.Id) return BadRequest();

            _appDbContext.CategoryComponents.Remove(dbcategoryComponent);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }




        #endregion

    }
}
