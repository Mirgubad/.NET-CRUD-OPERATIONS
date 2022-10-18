
using front_to_back.DAL;
using front_to_back.Migrations;
using front_to_back.Models;
using front_to_back.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var recentWorkComponents = await _appDbContext.RecentWorkComponents.ToListAsync();
            var projectComponents = await _appDbContext.ProjectComponents.ToListAsync();
           
            var model = new HomeIndexViewModel
            {
                
                RecentWorkComponents = recentWorkComponents,
                ProjectComponents = projectComponents,

            };

            return View(model);
        }


    }
}

