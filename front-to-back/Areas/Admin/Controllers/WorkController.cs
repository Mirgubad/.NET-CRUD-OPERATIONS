using front_to_back.Areas.Admin.ViewModels;
using front_to_back.DAL;
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

        public WorkController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var model = new WorkIndexViewModel
            {
                RecentWorkComponents = await _appDbContext.RecentWorkComponents.ToListAsync()
            };
            return View(model);
        }

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


            if (!recentWorkComponent.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Incorrect type");
                return View(recentWorkComponent);
            }

            if (recentWorkComponent.Photo.Length / 1024 > 1000)
            {
                ModelState.AddModelError("Photo", "Photo size must be 1000kb ");
                return View(recentWorkComponent);
            }

            string fileName = $"{Guid.NewGuid()}_{recentWorkComponent.Photo.FileName}";

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await recentWorkComponent.Photo.CopyToAsync(stream);

            }

            recentWorkComponent.FilePath = fileName;
            if (!ModelState.IsValid) return View(recentWorkComponent);
            await _appDbContext.RecentWorkComponents.AddAsync(recentWorkComponent);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var recentWorkComponents = await _appDbContext.RecentWorkComponents.FindAsync(id);
            if (recentWorkComponents == null) return NotFound();

            return View(recentWorkComponents);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, RecentWorkComponent recentWorkComponent)
        {

            if (id != recentWorkComponent.Id) return BadRequest();

            bool isExists = await _appDbContext.RecentWorkComponents.AnyAsync(rcw =>
            rcw.Title.ToLower().Trim() == recentWorkComponent.Title.ToLower().Trim() && rcw.Id != recentWorkComponent.Id);

            if (isExists)
            {
                ModelState.AddModelError("Title", "This name already taken");
                return View(recentWorkComponent);
            }
            var dbrecentworkComponent = await _appDbContext.RecentWorkComponents.FindAsync(id);
            if (dbrecentworkComponent == null) return NotFound();

            if (!recentWorkComponent.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Incorrect type");
                return View(recentWorkComponent);
            }

            if (recentWorkComponent.Photo.Length / 1024 > 1000)
            {
                ModelState.AddModelError("Photo", "Photo size must be 1000kb ");
                return View(recentWorkComponent);
            }

            string fileName = $"{Guid.NewGuid()}_{recentWorkComponent.Photo.FileName}";

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", fileName);

            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbrecentworkComponent.FilePath);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await recentWorkComponent.Photo.CopyToAsync(stream);
            }

            if (!ModelState.IsValid) return View(recentWorkComponent);

            dbrecentworkComponent.Title = recentWorkComponent.Title;
            dbrecentworkComponent.Text = recentWorkComponent.Text;
            dbrecentworkComponent.Photo = recentWorkComponent.Photo;
            dbrecentworkComponent.FilePath = fileName;

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

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

        [HttpGet]
        public async Task<IActionResult> Read(int id)
        {
            var recentworkComponent = await _appDbContext.RecentWorkComponents.FindAsync(id);
            if (recentworkComponent == null) return NotFound();
            return View(recentworkComponent);
        }
    }
}
