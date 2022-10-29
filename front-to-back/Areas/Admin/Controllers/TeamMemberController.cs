using front_to_back.Areas.Admin.ViewModels.TeamMember;
using front_to_back.DAL;
using front_to_back.Helpers;
using front_to_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamMemberController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public TeamMemberController(AppDbContext appDbContext,
           IWebHostEnvironment webHostEnvironment,
           IFileService fileService)
        {
            _appDbContext = appDbContext;
          _webHostEnvironment = webHostEnvironment;
           _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new TeamMemberIndexViewModel
            {
                TeamMembers = await _appDbContext.TeamMembers.ToListAsync()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMemberCreateViewModel model)
        {

            if (!ModelState.IsValid) return View(model);
            if (!_fileService.CheckFile(model.Photo))
            {
                ModelState.AddModelError("Photo","Incorrect file");
                return View(model);
            }
            int maxSize = 1000;
            if (!_fileService.MaxSize(model.Photo, maxSize))
            {
                ModelState.AddModelError("Photo", $"Photo size must be less {maxSize}kb");
                return View(model);
            }
            var teamMember = new TeamMember
            {
                Id=model.Id,
                Name=model.Name,
                Surname=model.Surname,
                Position=model.Position,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
        };
            await _appDbContext.TeamMembers.AddAsync(teamMember);
           await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dbteamMember = await _appDbContext.TeamMembers.FindAsync(id);
            if (dbteamMember == null) return NotFound();

            var model = new TeamMemberUpdateViewModel
            {
                Id = dbteamMember.Id,
                Surname = dbteamMember.Surname,
                Name = dbteamMember.Name,
                Position = dbteamMember.Position,
                PhotoPath=dbteamMember.PhotoPath
            };

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,TeamMemberUpdateViewModel model)
        {
            var dbteamMember = await _appDbContext.TeamMembers.FindAsync(id);
            if (dbteamMember == null) return NotFound();

            if (model.Photo != null)
            {
                if (!_fileService.CheckFile(model.Photo))
                {
                    ModelState.AddModelError("Photo", "File was't image");
                    return View(model);
                }
                int maxSize = 1000;
                if (!_fileService.MaxSize(model.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"Photo size must be less {maxSize}kb");
                    return View(model);
                }

                _fileService.Delete(_webHostEnvironment.WebRootPath, dbteamMember.PhotoPath);
                dbteamMember.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }
            dbteamMember.Name = model.Name;
            dbteamMember.Position = model.Position;
            dbteamMember.Surname = model.Surname;
            dbteamMember.Id = model.Id;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
         
            var dbteamMember = await _appDbContext.TeamMembers.FindAsync(id);
            if (dbteamMember == null) return NotFound();
            if(id!=dbteamMember.Id) return BadRequest();
            var model = new TeamMemberDetailsViewModel
            {
                Id = dbteamMember.Id,
                Name = dbteamMember.Name,
                Surname = dbteamMember.Surname,
                Position = dbteamMember.Position,
                PhotoPath = dbteamMember.PhotoPath
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult>Delete(int id)
        {
            var dbteamMember = await _appDbContext.TeamMembers.FindAsync(id);
            if (dbteamMember == null) return NotFound();
            _fileService.Delete(_webHostEnvironment.WebRootPath, dbteamMember.PhotoPath);
            _appDbContext.TeamMembers.Remove(dbteamMember);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}
