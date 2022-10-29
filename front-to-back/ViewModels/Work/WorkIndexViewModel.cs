using front_to_back.Models;

namespace front_to_back.ViewModels.Work
{
    public class WorkIndexViewModel
    {
        public List<OurWork> OurWorks { get; set; }

        public List<Category> Categories { get; set; }
        public List <CategoryComponent> CategoryComponents { get; set; }

        public List <RecentWorkComponent>RecentWorkComponents { get; set; }
        public FeaturedWorkComponent FeaturedWorkComponent { get; set; }
        

    }
}
