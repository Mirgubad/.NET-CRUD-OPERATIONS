using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit.Sdk;

namespace front_to_back.Models
{
    public class RecentWorkComponent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please  fill the Name"), MinLength(3, ErrorMessage = "Name cannot be less 3")]
        public string Title { get; set; }
        public string Text { get; set; }

        public string? FilePath { get; set; }


        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
