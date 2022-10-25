using Xunit.Sdk;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace front_to_back.Areas.Admin.ViewModels
{
    public class WorkUpdateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please  fill the Name"), MinLength(3, ErrorMessage = "Name cannot be less 3")]
        public string Title { get; set; }
        public string Text { get; set; }
        public string? FilePath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
