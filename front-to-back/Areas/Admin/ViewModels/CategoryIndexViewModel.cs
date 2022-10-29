using front_to_back.Models;

namespace front_to_back.Areas.Admin.ViewModels
{
    public class CategoryIndexViewModel
    {

        public List<Models.Category> Categories { get; set; }
        public List<CategoryComponent> CategoryComponents { get; set; }
    }
}

