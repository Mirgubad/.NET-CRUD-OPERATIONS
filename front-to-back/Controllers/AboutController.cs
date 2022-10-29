using front_to_back.DAL;
using front_to_back.Models;
using front_to_back.ViewModels.About;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AboutController(AppDbContext appDbContext)
        {
           _appDbContext = appDbContext;
        }
        public async Task< IActionResult> Index()
        {
            var ourpartners = new List<OurPartner>
            {
                new OurPartner{ClassName= "bxs-buildings"},
                new OurPartner{ClassName= "bxs-check-shield"},
                new OurPartner{ClassName= "bxs-bolt-circle"},
                new OurPartner{ClassName= "bxs-spa"},
            };

            var model = new AboutIndexViewModel
            {

                ourPartners = ourpartners,

                teamMembers = await _appDbContext.TeamMembers.ToListAsync()
            };
            return View(model);
        }
    }
}
