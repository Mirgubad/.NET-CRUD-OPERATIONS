﻿using System.ComponentModel.DataAnnotations.Schema;

namespace front_to_back.Areas.Admin.ViewModels.TeamMember
{
    public class TeamMemberIndexViewModel
    {
        public List<Models.TeamMember> TeamMembers { get; set; }

    }
}
