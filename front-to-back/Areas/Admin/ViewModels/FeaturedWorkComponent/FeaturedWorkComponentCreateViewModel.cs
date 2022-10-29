using front_to_back.Models;
using System.Reflection.Metadata.Ecma335;

namespace front_to_back.Areas.Admin.ViewModels.FeaturedWorkComponent
{
    public class FeaturedWorkComponentCreateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<IFormFile> Photos { get; set; }

    }
}
