using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace front_to_back.Areas.Admin.ViewModels.Category
{
    public class CategoryComponentDeleteViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string? FilePath { get; set; }

        public int CategoryId { get; set; }

        public List<SelectListItem>? Categories { get; set; }
    }
}
