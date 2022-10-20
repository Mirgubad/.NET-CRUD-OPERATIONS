using front_to_back.DAL;
using front_to_back.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Controllers
{
    public class PricingController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public PricingController(AppDbContext appDbContext)
        {
           _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new PricingIndexViewModel
            {
                Pricings = await _appDbContext.Pricings
                .OrderBy(pr=>pr.Id)
                .Take(1)
                .ToListAsync(),
            };
            return View(model);
        }

        public async Task<IActionResult> LoadMore(int skipRow)
        {
            var pricing = await _appDbContext.Pricings
               .OrderBy(pr => pr.Id)
               .Skip(1 * skipRow)
               .Take(1)
               .ToListAsync();

            return PartialView("_PricingPartial",pricing);
        }

    }
}
