using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneMVC.Web.ViewModels
{
    public class ClassViewModel
    {
        [Display(Name = "Database ID")]
        public int Id { get; set; }
        [Display(Name = "Class Name")]
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Display(Name = "Class Description")]
        [MaxLength(750)]
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
