using front_to_back.DAL;
using front_to_back.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using front_to_back.Models;
using front_to_back.Migrations;
using System.IO;

namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContactController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()

        {
            var contractIntroComponent = await _appDbContext.ContractIntroComponent.FirstOrDefaultAsync();

            if (contractIntroComponent == null)
            {
                return RedirectToAction("create");
            }

            var model = new ContactIndexViewModel
            {
                ContractIntroComponent = await _appDbContext.ContractIntroComponent.FirstOrDefaultAsync()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContractIntroComponent contractIntro)
        {

            if (!contractIntro.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Incorrect type");
                return View(contractIntro);
            }

            if (contractIntro.Photo.Length / 1024 > 1000)
            {
                ModelState.AddModelError("Photo", "Photo size must be 1000kb");
                return View(contractIntro);
            }

            string fileName = $"{Guid.NewGuid()}_{contractIntro.Photo.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", fileName);

            using (FileStream stream = new(path, FileMode.Create))
            {

                await contractIntro.Photo.CopyToAsync(stream);
            }

            contractIntro.FilePath = fileName;

            if (!ModelState.IsValid) return View(contractIntro);

            await _appDbContext.AddAsync(contractIntro);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var contactintrocomponent = await _appDbContext.ContractIntroComponent.FindAsync(id);
            if (contactintrocomponent == null) return NotFound();
            return View(contactintrocomponent);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ContractIntroComponent contractIntroComponent)
        {
            if (id != contractIntroComponent.Id) return BadRequest();
            var dbcontractIntroComponent = await _appDbContext.ContractIntroComponent.FindAsync(id);

            if (dbcontractIntroComponent==null) return NotFound();

            if (!contractIntroComponent.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "File was incorrect");
                return View(contractIntroComponent);
            }
      

            if (contractIntroComponent.Photo.Length / 1024 > 1000)
            {
                ModelState.AddModelError("Photo", "Photo size must be less 1000kb");
                return View(contractIntroComponent);
            }
          

            string fileName = $"{Guid.NewGuid()}_{contractIntroComponent.Photo.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath,"assets/img", fileName);

            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbcontractIntroComponent.FilePath);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
         
 
            using (FileStream stream = new FileStream(path, FileMode.Create,FileAccess.ReadWrite))
            {
                await contractIntroComponent.Photo.CopyToAsync(stream);
            }

            if (!ModelState.IsValid) return View(contractIntroComponent);
           

            dbcontractIntroComponent.Title = contractIntroComponent.Title;
            dbcontractIntroComponent.Description = contractIntroComponent.Description;
            dbcontractIntroComponent.Photo = contractIntroComponent.Photo;
            dbcontractIntroComponent.FilePath = fileName;

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");            
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var dbContactIntroComponent = await _appDbContext.ContractIntroComponent.FindAsync(id);
            if (dbContactIntroComponent == null) return NotFound();
            return View(dbContactIntroComponent);
        }

        [HttpPost]

        public async Task<IActionResult> DeleteComponent(int id)
        {
            var dbContactIntroComponent = await _appDbContext.ContractIntroComponent.FindAsync(id);
            if(dbContactIntroComponent == null) return NotFound();

            _appDbContext.ContractIntroComponent.Remove(dbContactIntroComponent);
            await _appDbContext.SaveChangesAsync();
            string oldFilePath=Path.Combine(_webHostEnvironment.WebRootPath,"assets/img",dbContactIntroComponent.FilePath);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dbcontactIntroComponent = await _appDbContext.ContractIntroComponent.FindAsync(id);
            if(dbcontactIntroComponent == null) return NotFound();

            return View(dbcontactIntroComponent);

        }
    }

}
