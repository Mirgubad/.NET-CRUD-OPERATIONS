using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Xunit.Sdk;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace front_to_back.Models
{
    public class CategoryComponent
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string? FilePath { get; set; }
        [Required]
        public Category? Category { get; set; }

        [Required]
        [NotMapped]
        public IFormFile Photo { get; set; }

      
        public int CategoryId { get; set; }

    }
}
