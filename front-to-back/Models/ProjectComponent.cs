using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace front_to_back.Models
{
    public class ProjectComponent
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }

        public string? FilePath { get; set; }

        public string Type { get; set; }

        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
