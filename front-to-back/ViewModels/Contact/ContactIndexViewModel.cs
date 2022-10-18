using front_to_back.Models;

namespace front_to_back.ViewModels
{
    public class ContactIndexViewModel
    {
        public ContractIntroComponent ContractIntroComponent { get; set; }

        public CreateWithUs CreateWithUs { get; set; }
        public List<ContactFormComponent> ContactFormComponent { get; set; }
    }
}
