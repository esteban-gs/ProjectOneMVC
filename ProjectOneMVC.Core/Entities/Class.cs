using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectOneMVC.Core.Entities
{
    public class Class : BaseEntity
    {
        [Display(Name = "Class Name")]
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Display(Name = "Class Description")]
        [MaxLength(750)]
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
