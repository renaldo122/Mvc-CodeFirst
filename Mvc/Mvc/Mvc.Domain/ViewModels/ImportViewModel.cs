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

    public class ImportViewModel
    {
        [ForeignKey("FunitorId")]
        [Required]
        [Display(Name = "Furnitor")]
        public int CategoryId { get; set; }

        [Display(Name = "Delete Old Funitor Products")]
        public bool DeleteExisting { get; set; }

    }
}
