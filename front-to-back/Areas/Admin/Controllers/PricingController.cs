using front_to_back.Areas.Admin.ViewModels;
using front_to_back.DAL;
using front_to_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                Pricings = await _appDbContext.Pricings.ToListAsync()
            };

            return View(model);
        }


        [HttpGet]

        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Pricing pricing)
        {
            if (!ModelState.IsValid) return View(pricing);

            bool exists = await _appDbContext.Pricings.AnyAsync(pr=>pr.Title.ToLower().Trim()==pricing.Title.ToLower().Trim());
            if (exists)
            {
                ModelState.AddModelError("Title", "This name already in use");
                return View(pricing);
            }

            await _appDbContext.Pricings.AddAsync(pricing);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");



          
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var pricing = await _appDbContext.Pricings.FindAsync(id);
            if (pricing == null) return NotFound();
            return View(pricing);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,Pricing pricing)
        {

            if (id != pricing.Id) return BadRequest();

            var dbpricing = await _appDbContext.Pricings.FindAsync(id);
            if (dbpricing == null) return NotFound();

            if(!ModelState.IsValid) return View(pricing);

            bool exists=await _appDbContext.Pricings.AnyAsync(pr=>pr.Title.ToLower().Trim()==pricing.Title && pr.Id!=id);


            if(exists)
            {
                ModelState.AddModelError("Title", "This name already in use");
            }

            dbpricing.Title=pricing.Title;
            dbpricing.Description = pricing.Description;
            dbpricing.Cost=pricing.Cost;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dbpricing = await _appDbContext.Pricings.FindAsync(id);
            if (dbpricing == null) return NotFound();

           return View(dbpricing);

        }

        [HttpPost]
        public async Task<IActionResult> DeletePricing(int id )
        {
            var dbpricing = await _appDbContext.Pricings.FindAsync(id);
            if (dbpricing == null) return NotFound();

             _appDbContext.Pricings.Remove(dbpricing);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dbpricing = await _appDbContext.Pricings.FindAsync(id);

            if (dbpricing == null) return NotFound();

            return View(dbpricing);

        }
    }
}
