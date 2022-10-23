using Microsoft.Build.Framework;
using Xunit.Sdk;

namespace front_to_back.Models
{
    public class CategoryComponent
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]

        public int CategoryId { get; set; }

    }
}
