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
            var aims = new List<Aims>
            {new Aims{Classname="bxs-bulb",Text="Incididunt ut labore et dolore magna aliqua.Quis ipsum suspendisse commodo viverra."},
            new Aims{Classname="bx-revision",Text=" Eiusmod tempor incididunt ut labore et dolore magna aliqua.Ut enim ad minim veniam quis."},
            new Aims{Classname="bxs-select-multiple",Text=" Lorem ipsum dolor sit amet, consectetur adipisicing elit,sed do eiusmod tempor."}
            };
           

            var model = new AboutIndexViewModel
            {
              
                ourPartners = ourpartners,
                Aims=aims,
            };
            return View(model);
        }
    }
}
