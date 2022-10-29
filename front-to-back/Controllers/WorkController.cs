using front_to_back.DAL;
using front_to_back.Models;
using front_to_back.ViewModels.Work;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Controllers
{
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
                Categories = await _appDbContext.Categories.ToListAsync(),
                CategoryComponents = await _appDbContext.CategoryComponents.ToListAsync(),
                RecentWorkComponents = await _appDbContext.RecentWorkComponents.ToListAsync(),
                FeaturedWorkComponent=await _appDbContext.FeaturedWorkComponents
                .Include(rcwp=>rcwp.FeaturedWorkComponentPhotos.OrderBy(order => order.Order)).FirstOrDefaultAsync()

            };
            return View(model);
        }
    }
}
