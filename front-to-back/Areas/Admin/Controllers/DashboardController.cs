using front_to_back.Areas.Admin.ViewModels;
using front_to_back.DAL;
using front_to_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]

    
    public class DashboardController : Controller
    {
        private readonly AppDbContext _appDbContext;
     
        public async Task< IActionResult> Index()
        {
            return View();
        }

    }

}
