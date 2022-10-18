using System.ComponentModel.DataAnnotations;

namespace front_to_back.Models
{
    public class Category
    {

        public Category()
        {
            CategoryComponents=new List<CategoryComponent>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage ="Please the fill Name"),MinLength(3,ErrorMessage ="Name cannot be less 3")]
        public string Title { get; set; }

        public ICollection<CategoryComponent> CategoryComponents { get; set; }
    }
}
