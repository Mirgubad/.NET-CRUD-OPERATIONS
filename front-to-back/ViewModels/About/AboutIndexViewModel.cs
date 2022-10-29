using front_to_back.Models;
using front_to_back.ViewComponents;

namespace front_to_back.ViewModels.About
{
    public class AboutIndexViewModel
    {
        public List<OurPartner> ourPartners  { get; set; }
        public List<Aims> Aims { get; set; }
        public List<ObjectiveComponentViewComponent> objectiveComponentViewComponents { get; set; }
        public List<OurWork> ourWorks { get; set; }
        public List<TeamMember> teamMembers { get; set; }
    }
}
