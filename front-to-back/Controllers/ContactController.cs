using front_to_back.DAL;
using front_to_back.Models;
using front_to_back.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ContactController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new ContactIndexViewModel
            {
                ContractIntroComponent = await _appDbContext.ContractIntroComponent.FirstOrDefaultAsync(),
                CreateWithUs = await _appDbContext.CreateWithUs.FirstOrDefaultAsync(),
                ContactFormComponent = await _appDbContext.ContactFormComponents.ToListAsync()
            };
            return View(model);
        }
    }
}