using Microsoft.Build.Framework;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace front_to_back.Models
{
    public class ContractIntroComponent 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string?  FilePath { get; set; }
        [Required]
        [NotMapped]
        public IFormFile Photo { get; set; }   
    }
}