using front_to_back.DAL;
using front_to_back.ViewModels.About;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.ViewComponents
{
    public class ObjectiveComponentViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;

        public ObjectiveComponentViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AboutIndexViewModel
            {
                ourWorks = await _appDbContext.OurWorks.ToListAsync()

            };
            return View(model);
        }
    }
}
