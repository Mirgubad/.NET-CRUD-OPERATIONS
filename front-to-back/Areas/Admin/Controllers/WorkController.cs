using front_to_back.Areas.Admin.ViewModels;
using front_to_back.DAL;
using front_to_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Areas.Admin.Controllers

{
    [Area("Admin")]
    public class WorkController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public WorkController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
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
            if (!ModelState.IsValid) return View(recentWorkComponent);

            bool isExist = await _appDbContext.RecentWorkComponents
                .AnyAsync(rw => rw.Title.ToLower().Trim() == recentWorkComponent.Title.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Title", "This name already exists");
                return View(recentWorkComponent);
            }

            await _appDbContext.RecentWorkComponents.AddAsync(recentWorkComponent);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");



        }
    }
}
