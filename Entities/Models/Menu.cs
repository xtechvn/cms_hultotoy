﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class Menu
    {
        public Menu()
        {
            Action = new HashSet<Action>();
            RolePermission = new HashSet<RolePermission>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string MenuCode { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public int? OrderNo { get; set; }
        public string FullParent { get; set; }

        public virtual ICollection<Action> Action { get; set; }
        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
