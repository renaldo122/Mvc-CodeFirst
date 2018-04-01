// <copyright file="ProductViewModel.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/24/2018</date>
// <summary>Product View Model</summary>

namespace Mvc.Domain.ViewModels
{
    using Mvc.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProductViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Barcode is required")]
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [ForeignKey("FunitorId")]
        [Required]
        [Display(Name = "Furnitor")]
        public int CategoryId { get; set; }

        [Display(Name = "Marka")]
        public string Marka { get; set; }

        [Display(Name = "Kod")]
        public string Kod { get; set; }

        [Display(Name = "Furnitor")]
        public string FunitorName { get; set; }

        [Display(Name = "Klasifikim")]
        public string Klasifikim { get; set; }


        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity")]
        public string Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Date Created is required")]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }
    }
}
