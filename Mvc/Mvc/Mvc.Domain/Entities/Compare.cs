// <copyright file="Product.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/22/2018</date>
// <summary>Product representing product entity</summary>

namespace Mvc.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Compare
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Display(Name = "Barcode")]
        public string Barcode { get; set; }


        [Display(Name = "Name")]
        public string Name { get; set; }


        [Display(Name = "Kod")]
        public string Kod { get; set; }

        [Required]
        [Display(Name = "Furnitor")]
        public int CategoryId { get; set; }

        [Display(Name = "Marka")]
        public string Marka { get; set; }

        [Display(Name = "Quantity")]
        public string Quantity { get; set; }

        [Display(Name = "Product Price")]
        public decimal? ProductPrice { get; set; }

        [Display(Name = "Furnitor Price")]
        public decimal? FurnitorPrice { get; set; }

        [Display(Name = "Small Price")]
        public decimal? SmallPrice { get; set; }


        [Display(Name = "Product Found")]
        public string ProductFound { get; set; }


        [Display(Name = "Furnitor Type")]
        public string Type { get; set; }


        [Display(Name = "Klasifikim")]
        public string Klasifikim { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }
    }
}
