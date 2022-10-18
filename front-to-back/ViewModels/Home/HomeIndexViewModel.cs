using front_to_back.Models;

namespace front_to_back.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public List<ProjectComponent> ProjectComponents { get; set; }
        public List<RecentWorkComponent> RecentWorkComponents { get; set; }
    }
}
