using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Domain.ViewModels
{
    public class CompareFurnitorsViewModel
    {

        [Display(Name = "Delete Old Furnitor")]
        public bool DeleteExisting { get; set; }

        [Required(ErrorMessage = "Furnitor No1 is required")]
        [Display(Name = "Furnitor No1")]
        public int CategoryId1 { get; set; }

        [Required(ErrorMessage = "Furnitor No2 is required")]
        [Display(Name = "Furnitor No2")]
        public int CategoryId2 { get; set; }

        [Display(Name = "Furnitor No3")]
        public int? CategoryId3 { get; set; }

        [Display(Name = "Furnitor No4")]
        public int? CategoryId4 { get; set; }


        [Display(Name = "Compare By Marka")]
        public bool ComparebyMarka { get; set; }
        
        [Display(Name = "Marka 1")]
        public string MarkaName1 { get; set; }

        [Display(Name = "Marka 2")]
        public string MarkaName2 { get; set; }

        [Display(Name = "Marka 3")]
        public string MarkaName3 { get; set; }

        [Display(Name = "Marka 4")]
        public string MarkaName4 { get; set; }

        [Display(Name = "Marka 5")]
        public string MarkaName5 { get; set; }


    }
}
