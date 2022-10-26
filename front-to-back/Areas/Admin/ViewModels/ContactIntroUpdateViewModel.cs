using System.ComponentModel.DataAnnotations.Schema;

namespace front_to_back.Areas.Admin.ViewModels
{
    public class ContactIntroUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }


        public string? FilePath { get; set; }
    
        public IFormFile? Photo { get; set; }


    }
}
