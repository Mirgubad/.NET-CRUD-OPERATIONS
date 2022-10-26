using front_to_back.Areas.Admin.ViewModels;
using front_to_back.DAL;
using front_to_back.Helpers;
using front_to_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace front_to_back.Areas.Admin.Controllers

{
    [Area("Admin")]
    public class WorkController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public WorkController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment,IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new WorkIndexViewModel
            {
                RecentWorkComponents = await _appDbContext.RecentWorkComponents.ToListAsync()
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
        public async Task<IActionResult> Create(RecentWorkComponent recentWorkComponent)
        {

            bool isExist = await _appDbContext.RecentWorkComponents
                .AnyAsync(rw => rw.Title.ToLower().Trim() == recentWorkComponent.Title.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Title", "This name already exists");
                return View(recentWorkComponent);
            }

            if (!_fileService.CheckFile(recentWorkComponent.Photo))
            {
                ModelState.AddModelError("Photo", "Incorrect type");
                return View(recentWorkComponent);
            }

            int maxSize = 1000;

            if (!_fileService.MaxSize(recentWorkComponent.Photo,maxSize))
            {
                ModelState.AddModelError("Photo", $"Photo size must be {maxSize}kb ");
                return View(recentWorkComponent);
            }
        
            recentWorkComponent.FilePath = await _fileService.UploadAsync(recentWorkComponent.Photo,_webHostEnvironment.WebRootPath);
            if (!ModelState.IsValid) return View(recentWorkComponent);
            await _appDbContext.RecentWorkComponents.AddAsync(recentWorkComponent);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id,WorkUpdateViewModel model)
        {
            var recentWorkComponents = await _appDbContext.RecentWorkComponents.FindAsync(id);
            if (recentWorkComponents == null) return NotFound();

            model = new WorkUpdateViewModel
            {
                Id=recentWorkComponents.Id,
                FilePath=recentWorkComponents.FilePath,
                Text=recentWorkComponents.Text,
                Title=recentWorkComponents.Title,
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> UpdateWork(int id, WorkUpdateViewModel model)
        {

            if (id != model.Id) return BadRequest();

            bool isExists = await _appDbContext.RecentWorkComponents.AnyAsync(rcw =>
            rcw.Title.ToLower().Trim() == model.Title.ToLower().Trim() && rcw.Id != model.Id);

            if (isExists)
            {
                ModelState.AddModelError("Title", "This name already taken");
                return View(model);
            }
            var dbrecentworkComponent = await _appDbContext.RecentWorkComponents.FindAsync(id);
            if (dbrecentworkComponent == null) return NotFound();

            if (model.Photo != null)
            {
                if (!_fileService.CheckFile(model.Photo))
                {
                    ModelState.AddModelError("Photo", "Incorrect type");
                    return View(model);
                }

                int maxSize = 1000;
                if (!_fileService.MaxSize(model.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"Photo size must be {maxSize}kb ");
                    return View(model);
                }

                _fileService.Delete(_webHostEnvironment.WebRootPath, dbrecentworkComponent.FilePath);
                dbrecentworkComponent.FilePath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }
          

            if (!ModelState.IsValid) return View(model);

            dbrecentworkComponent.Title = model.Title;
            dbrecentworkComponent.Text = model.Text;
          
       

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var recentworkComponent = await _appDbContext.RecentWorkComponents.FindAsync(id);
            if (recentworkComponent == null) return NotFound();
            return View(recentworkComponent);
        }
        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dbrecentWorkComponent = await _appDbContext.RecentWorkComponents.FindAsync(id);
            if (dbrecentWorkComponent == null) return NotFound();
            return View(dbrecentWorkComponent);
        }


        [HttpPost]

        public async Task<IActionResult> DeleteComponent(int id)
        {

            var dbrecentWorkComponents = await _appDbContext.RecentWorkComponents.FindAsync(id);
            if (dbrecentWorkComponents == null) return NotFound();
            _appDbContext.RecentWorkComponents.Remove(dbrecentWorkComponents);
            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbrecentWorkComponents.FilePath);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        #endregion
    }
}
