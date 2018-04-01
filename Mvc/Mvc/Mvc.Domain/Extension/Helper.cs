// <copyright file="Helper.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/23/2018</date>
// <summary>Extension to convert view model to entiry model</summary>

namespace Mvc.Domain.Extension
{
    using Mvc.Domain.Entities;
    using Mvc.Domain.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Helper
    {
        /// <summary>
        /// Method to convert category view model to category model
        /// </summary>
        /// <param name="categoryVm">Category view model to convert</param>
        /// <returns>The category model</returns>
        public Funitor ToCategory(FunitorViewModel categoryVm)
        {
            return new Funitor()
            {
                Id = categoryVm.Id,
                Name = categoryVm.Name,
                DateCreated = categoryVm.DateCreated,
                DateModified = categoryVm.DateModified,
                Description = categoryVm.Description
            };
        }

        /// <summary>
        /// Method to convert product view model to product model
        /// </summary>
        /// <param name="productVm">Product view model to convert</param>
        /// <returns>The product model</returns>
        public Product ToProduct(ProductViewModel productVm)
        {
            return new Product()
            {
                Id = productVm.Id,
                Name = productVm.Name,
                Barcode=productVm.Barcode,
                Kod=productVm.Kod,
                CategoryId = productVm.CategoryId,
                Marka=productVm.Marka,
                Klasifikim=productVm.Klasifikim,
                DateCreated = productVm.DateCreated,
                DateModified = productVm.DateModified,
                Price = productVm.Price,
                Quantity = productVm.Quantity
            };
        }



        

        /// <summary>
        /// Method to convert product view model to product model
        /// </summary>
        /// <param name="productVm">Product view model to convert</param>
        /// <returns>The product model</returns>
        public Compare ToCompare(CompareViewModel compareVM)
        {
            return new Compare()
            {

                Barcode = compareVM.Barcode,
                Kod = compareVM.Kod,
                Name = compareVM.Name,
                CategoryId = compareVM.CategoryId,
                Marka=compareVM.Marka,
                Quantity = compareVM.Quantity,
                ProductPrice = compareVM.ProductPrice,
                FurnitorPrice = compareVM.FurnitorPrice,
                SmallPrice = compareVM.SmallPrice,
                ProductFound = compareVM.ProductFound,
                Type=compareVM.Type,
                Klasifikim=compareVM.Klasifikim,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
            };
        }

    }
}
