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

    public class CompareViewModel
    {
        [Key]
        [Required]
        public int Id { get; set; }


        [Required]
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Display(Name = "Kod")]
        public string Kod { get; set; }

      
        [Display(Name = "Furnitor")]
        public string FunitorName { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Furnitor")]
        public int CategoryId { get; set; }

        [Display(Name = "Marka")]
        public string Marka { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public string Quantity { get; set; }

        [Display(Name = "Furnitor1 Price")]
        public decimal? ProductPrice { get; set; }

        [Display(Name = "Furnitor2 Price")]
        public decimal? FurnitorPrice { get; set; }

        [Display(Name = "Difference between 2 furnitors")]
        public decimal? SmallPrice { get; set; }

        [Display(Name = "Product Found")]
        public string ProductFound { get; set; }


        [Display(Name = "Furnitor Type")]
        public string Type { get; set; }

        [Display(Name = "Klasifikim")]
        public string Klasifikim { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }
    }
}
