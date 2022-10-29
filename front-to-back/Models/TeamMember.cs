using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace front_to_back.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Surname { get; set; }
        [Required]
        public string Position { get; set; }
        public string? PhotoPath { get; set; }
        [Required]
        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
