using front_to_back.DAL;
using front_to_back.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using front_to_back.Models;
using front_to_back.Migrations;
using System.IO;
using front_to_back.Areas.Admin.ViewModels;
using front_to_back.Helpers;

namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public ContactController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()

        {
            var contractIntroComponent = await _appDbContext.ContractIntroComponent.FirstOrDefaultAsync();

            if (contractIntroComponent == null)
            {
                return RedirectToAction("create");
            }

            var model = new ContactIntroViewModel
            {
                contractIntroComponents = await _appDbContext.ContractIntroComponent.ToListAsync()
            };
            return View(model);
        }

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContractIntroComponent contractIntro)
        {

            if (!_fileService.CheckFile(contractIntro.Photo))
            {
                ModelState.AddModelError("Photo", "Incorrect type");
                return View(contractIntro);
            }

            int maxSize = 1000;
            if (!_fileService.MaxSize(contractIntro.Photo,maxSize))
            {
                ModelState.AddModelError("Photo", $"Photo size must be {maxSize}kb");
                return View(contractIntro);
            }


            contractIntro.FilePath = await _fileService.UploadAsync(contractIntro.Photo, _webHostEnvironment.WebRootPath);

            if (!ModelState.IsValid) return View(contractIntro);

            await _appDbContext.AddAsync(contractIntro);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var contactintrocomponent = await _appDbContext.ContractIntroComponent.FindAsync(id);
            if (contactintrocomponent == null) return NotFound();

            var model = new ContactIntroUpdateViewModel
            {
                Id = contactintrocomponent.Id,
                Title = contactintrocomponent.Title,
                Description = contactintrocomponent.Description,
                FilePath = contactintrocomponent.FilePath
             
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ContactIntroUpdateViewModel model)
        {
            var dbcontractIntroComponent = await _appDbContext.ContractIntroComponent.FindAsync(id);

            if (id != model.Id) return BadRequest();
            if (dbcontractIntroComponent == null) return NotFound();

            if (model.Photo != null)
            {
    
                if (!_fileService.CheckFile(model.Photo))
                {
                    ModelState.AddModelError("Photo","File format was incorrect ");
                    return View(model);
                }
   
                var maxSize = 1000;

                if (!_fileService.MaxSize(model.Photo,maxSize))
                {
                    ModelState.AddModelError("Photo",$"Photo size must be less {maxSize}kb");
                    return View(model);
                }

                _fileService.Delete(_webHostEnvironment.WebRootPath, dbcontractIntroComponent.FilePath);
                dbcontractIntroComponent.FilePath = await
                _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }

            if (!ModelState.IsValid) return View(model);

            dbcontractIntroComponent.Title = model.Title;
            dbcontractIntroComponent.Description = model.Description;

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dbcontactIntroComponent = await _appDbContext.ContractIntroComponent.FindAsync(id);
            if (dbcontactIntroComponent == null) return NotFound();

            return View(dbcontactIntroComponent);

        }

        #endregion

        #region Delete
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
            if (dbContactIntroComponent == null) return NotFound();

            _fileService.Delete(_webHostEnvironment.WebRootPath, dbContactIntroComponent.FilePath);
            _appDbContext.ContractIntroComponent.Remove(dbContactIntroComponent);

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        #endregion
    }

}
