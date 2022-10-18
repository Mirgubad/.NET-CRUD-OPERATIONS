using front_to_back.DAL;
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
            var contractIntroComponent = await _appDbContext.ContractIntroComponent.FirstOrDefaultAsync();
            var createwithUs=await _appDbContext.CreateWithUs.FirstOrDefaultAsync();
            var contactFormComponent = await _appDbContext.ContactFormComponents.ToListAsync();

            var model = new ContactIndexViewModel
            {
                ContractIntroComponent = contractIntroComponent,
                CreateWithUs = createwithUs,
                ContactFormComponent=contactFormComponent
            };

            return View(model);
        }
    }
}