using front_to_back.Areas.Admin.ViewModels.FeaturedWorkComponent;
using front_to_back.DAL;
using front_to_back.Helpers;
using front_to_back.Migrations;
using front_to_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeaturedWorkComponentController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public FeaturedWorkComponentController(AppDbContext appDbContext,
           IWebHostEnvironment webHostEnvironment,
           IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new FeaturedWorkComponentIndexViewModel
            {
                FeaturedWorkComponent = await _appDbContext.FeaturedWorkComponents.FirstOrDefaultAsync()
            };
            return View(model);
        }

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var featuredWorkComponent = await _appDbContext.FeaturedWorkComponents
                .Include(rwc => rwc.FeaturedWorkComponentPhotos)
                .FirstOrDefaultAsync();
            if (featuredWorkComponent != null) return NotFound();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeaturedWorkComponentCreateViewModel model)
        {
            var checkfeaturedwork = await _appDbContext.FeaturedWorkComponents.FirstOrDefaultAsync();

            if (checkfeaturedwork == null)
            {
                if (!ModelState.IsValid) return View(model);
                var featuredWorkComponent = new FeaturedWorkComponent
                {
                    Title = model.Title,
                    Description = model.Description,
                };
                await _appDbContext.FeaturedWorkComponents.AddAsync(featuredWorkComponent);
                await _appDbContext.SaveChangesAsync();
                var maxSize = 1000;
                int order = 1;
                bool hasError = false;
                foreach (var photo in model.Photos)
                {
                    if (!_fileService.CheckFile(photo))
                    {
                        ModelState.AddModelError("Photos", $"File {photo.FileName} incorrect type");
                        hasError = true;
                    }
                    else if (!_fileService.MaxSize(photo, maxSize))
                    {
                        ModelState.AddModelError("Photos", $"Photo size must be less{maxSize}kb");
                        hasError = true;
                    }
                }
                if (hasError)
                {
                    return View(model);
                }
                foreach (var photo in model.Photos)
                {
                    var featuredworkcomponentphoto = new FeaturedWorkComponentPhoto
                    {
                        Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                        Order = order,
                        FeaturedWorkComponentId = featuredWorkComponent.Id
                    };
                    await _appDbContext.FeaturedWorkComponentPhotos.AddAsync(featuredworkcomponentphoto);
                    await _appDbContext.SaveChangesAsync();
                    order++;

                }
            }
            return RedirectToAction("index");
        }

        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var featuredWorkComponent = await _appDbContext.FeaturedWorkComponents
                .Include(rwc => rwc.FeaturedWorkComponentPhotos)
                .FirstOrDefaultAsync();
            if (featuredWorkComponent == null) return NotFound();
            var model = new FeaturedWorkComponentUpdateViewModel
            {
                Id = featuredWorkComponent.Id,
                Title = featuredWorkComponent.Title,
                Description = featuredWorkComponent.Description,
                FeaturedWorkComponentPhotos = featuredWorkComponent.FeaturedWorkComponentPhotos.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(FeaturedWorkComponentUpdateViewModel model)
        {
            var dbfeaturedWorkComponent = await _appDbContext.FeaturedWorkComponents
               .Include(rwc => rwc.FeaturedWorkComponentPhotos)
               .FirstOrDefaultAsync();
            model.FeaturedWorkComponentPhotos = dbfeaturedWorkComponent.FeaturedWorkComponentPhotos.ToList();
            if (dbfeaturedWorkComponent == null) return NotFound();
            if (!ModelState.IsValid) return View(model);
            dbfeaturedWorkComponent.Title = model.Title;
            dbfeaturedWorkComponent.Description = model.Description;
            var featuredWorkComponent = new FeaturedWorkComponent
            {
                Title = model.Title,
                Description = model.Description,
            };

            await _appDbContext.SaveChangesAsync();
            var maxSize = 1000;
            int order = dbfeaturedWorkComponent.FeaturedWorkComponentPhotos.Count;
            if (order == 0)
            {
                order = 1;
            }
            bool hasError = false;
            if (model.Photos != null)
            {
                foreach (var photo in model.Photos)
                {
                    if (!_fileService.CheckFile(photo))
                    {
                        ModelState.AddModelError("Photos", $"File {photo.FileName} incorrect type");
                        hasError = true;
                    }
                    else if (!_fileService.MaxSize(photo, maxSize))
                    {
                        ModelState.AddModelError("Photos", $"Photo size must be less{maxSize}kb");
                        hasError = true;
                    }
                }
                if (hasError)
                {
                    return View(model);
                }
                foreach (var photo in model.Photos)
                {
                    var featuredworkcomponentphoto = new FeaturedWorkComponentPhoto
                    {
                        Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                        Order = order,
                        FeaturedWorkComponentId = dbfeaturedWorkComponent.Id
                    };
                    await _appDbContext.FeaturedWorkComponentPhotos.AddAsync(featuredworkcomponentphoto);
                    await _appDbContext.SaveChangesAsync();
                    order++;
                }
            }
            return RedirectToAction("index");
        }

        #endregion


        #region Details
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var featuredworkComponent = await _appDbContext.FeaturedWorkComponents
                .Include(rcwp => rcwp.FeaturedWorkComponentPhotos)
                .FirstOrDefaultAsync();

            if (featuredworkComponent == null) return NotFound();

            var model = new FeaturedWorkComponentDetailsViewModel
            {
                Id = featuredworkComponent.Id,
                Title = featuredworkComponent.Title,
                Description = featuredworkComponent.Description,
                FeaturedWorkComponentPhotos = _appDbContext.FeaturedWorkComponentPhotos.ToList(),

            };

            return View(model);

        }
        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var featuredworkComponent = await _appDbContext.FeaturedWorkComponents
                .Include(rcwp => rcwp.FeaturedWorkComponentPhotos)
                .FirstOrDefaultAsync();

            if (featuredworkComponent == null) return NotFound();

            foreach (var photo in featuredworkComponent.FeaturedWorkComponentPhotos)
            {
                _fileService.Delete(_webHostEnvironment.WebRootPath, photo.Name);
            }
            _appDbContext.Remove(featuredworkComponent);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion

        #region UpdatePhoto

        [HttpGet]
        public async Task<IActionResult> UpdatePhoto(int id)
        {
            var photo = await _appDbContext.FeaturedWorkComponentPhotos.FindAsync(id);
            if (photo == null) return NotFound();
            var model = new FeaturedWorkComponentUpdatePhotoViewModel
            {
                Order = photo.Order,
                FeaturedWorkComponentId = photo.FeaturedWorkComponentId

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(int id, FeaturedWorkComponentUpdatePhotoViewModel model)
        {
            var photo = await _appDbContext.FeaturedWorkComponentPhotos.FindAsync(id);
            if (photo == null) return NotFound();
            photo.Order = model.Order;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("update", "featuredworkcomponent", new { Id = photo.FeaturedWorkComponentId });
        }
        #endregion

        #region DeletePhoto

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _appDbContext.FeaturedWorkComponentPhotos.FindAsync(id);
            if (photo == null) return NotFound();
            _appDbContext.FeaturedWorkComponentPhotos.Remove(photo);
            _fileService.Delete(_webHostEnvironment.WebRootPath, photo.Name);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("update", "featuredworkcomponent", new { Id = photo.FeaturedWorkComponentId });
        }
        #endregion
    }
}
