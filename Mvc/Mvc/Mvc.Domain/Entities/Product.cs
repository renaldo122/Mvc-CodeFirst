// <copyright file="Product.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/22/2018</date>
// <summary>Product representing product entity</summary>

namespace Mvc.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Product
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

        [Display(Name = "Klasifikim")]
        public string Klasifikim { get; set; }


        [Display(Name = "Quantity")]
        public string Quantity { get; set; }

        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }
    }
}
