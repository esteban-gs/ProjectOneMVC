using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectOneMVC.Core.Entities
{
    public class UserClass
    {
        [ForeignKey("SchoolUser")]
        public string SchoolUserId { get; set; }
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        public IdentityUser SchoolUser { get; set; }
        public Class Class { get; set; }
    }
}
