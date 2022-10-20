using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace front_to_back.Models
{
    public class Pricing
    {
        public int Id { get; set; }
         
        [Required(ErrorMessage = "Please the fill Name"), MinLength(3, ErrorMessage = "Name cannot be less 3")]
        public string  Title { get; set; }

        [Required(ErrorMessage = "Please the fill Description"), MinLength(3, ErrorMessage = "Description cannot be less 3")]
        public string  Description { get; set; }
        public string  Cost { get; set; }

    }
}
