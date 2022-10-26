using front_to_back.DAL;
using front_to_back.Migrations;
using front_to_back.Models;
using front_to_back.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectComponentController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectComponentController(AppDbContext appDbContext,IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel
            {
                ProjectComponents = await _appDbContext.ProjectComponents.ToListAsync()
            };
            return View(model);
        }
        #region Create

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectComponent projectComponent)
        {

            if (!projectComponent.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Incorrect");
                return View(projectComponent);
            }

            if(projectComponent.Photo.Length/1024 > 1000)
            {
                ModelState.AddModelError("Photo", "Photo size must be 1000kb");
                return View(projectComponent);
            }

            string fileName = $"{Guid.NewGuid()}_{projectComponent.Photo.FileName}";
            string path =Path.Combine(_webHostEnvironment.WebRootPath,"assets/img",fileName) ;


            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await projectComponent.Photo.CopyToAsync(stream);   
            }

            projectComponent.FilePath=fileName;
            if(!ModelState.IsValid) return View(projectComponent);  
            await _appDbContext.AddAsync(projectComponent);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        #endregion
    }
}
