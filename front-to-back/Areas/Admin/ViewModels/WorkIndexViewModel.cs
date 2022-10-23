using front_to_back.Models;

namespace front_to_back.Areas.Admin.ViewModels
{
    public class WorkIndexViewModel
    {

        public List< RecentWorkComponent> RecentWorkComponents { get; set; }

        public List<OurWork> OurWorks { get; set; }
    }
}
