﻿using System.ComponentModel.DataAnnotations;

namespace front_to_back.Areas.Admin.ViewModels.TeamMember
{
    public class TeamMemberUpdateViewModel
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Surname { get; set; }
        [Required]
        public string Position { get; set; }
        public string? PhotoPath { get; set; }

        public IFormFile? Photo { get; set; }
    }
}
