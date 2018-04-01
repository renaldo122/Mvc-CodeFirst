// <copyright file="IEntitiesRepository.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/22/2018</date>
// <summary>IEntitiesRepository representing interface of the repository</summary>

namespace Mvc.Domain.Abstract
{
    using Mvc.Domain.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface to the database
    /// </summary>
    public interface IEntitiesRepository
    {
        #region Category
        /// <summary>
        /// Get all categories from the database async
        /// </summary>
        /// <returns>List with categories</returns>
        IEnumerable<FunitorViewModel> GetCategoriesAsync();


        IEnumerable<ProductViewModel> GetMarkaAsync();

        /// <summary>
        /// Get a category from the database based on id
        /// </summary>
        /// <param name="id">Id of the category</param>
        /// <returns>The category we search for</returns>
        Task<FunitorViewModel> GetCategoryByIdAsync(int id);

        /// <summary>
        /// Add an category on database or update an egzistin one
        /// </summary>
        /// <param name="categoryVm">Category to be added or updated</param>
        void AddOrUpdateCategory(FunitorViewModel categoryVm);


        /// <summary>
        /// Add an compareVm on database or update an egzistin one
        /// </summary>
        /// <param name="categoryVm">Category to be added or updated</param>
        void AddOrUpdateCompare(CompareViewModel compareVm);

        
        /// <summary>
        /// Remove an category from the database async
        /// </summary>
        /// <param name="id">Id of the category to be deleted</param>
        /// <returns>True if the category is deleted, false if there is any problem</returns>
        Task<bool> DeleteCategoryByIdAsync(int id);
        #endregion

        #region Product
        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        Task<IEnumerable<ProductViewModel>> GetProductsAsync(string currentFilter);

 
        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        Task<IEnumerable<CompareViewModel>> GetComparedProductsAsync(string type);

        /// <summary>
        /// Get all products from the CategoriID async
        /// </summary>
        /// <returns>List with products</returns>
        Task<IEnumerable<ProductViewModel>> ListOfProductsImportedToCompareByCategorNoMarkayAsync(
            int CategoryId1, int CategoryId2, int CategoryId3, int CategoryId4);


        /// <summary>
        /// Get all products from the CategoriID async
        /// </summary>
        /// <returns>List with products</returns>
        Task<IEnumerable<ProductViewModel>> ListOfProductsImportedToCompareByCategoryAsync(
            int CategoryId1,int CategoryId2, int CategoryId3, int CategoryId4,
             string MarkaName1, string MarkaName2, string MarkaName3, string MarkaName4, string MarkaName5);

        /// <summary>
        /// Get all products from the CategoriID async
        /// </summary>
        /// <returns>List with products</returns>
        Task<IEnumerable<ProductViewModel>> ListOfProductsImportedToCompareByCategoryIDAsync(int CategoryId1);


        /// <summary>
        /// Get all Furnitor from the CategoriID async
        /// </summary>
        /// <returns>List with products</returns>
        Task<IEnumerable<ProductViewModel>> ListOfFurnitorImportedToCompareByCategoryAsync(int categoriId);



        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        Task<IEnumerable<CompareViewModel>> GetProductsToCompareAsync(string type);

        /// <summary>
        /// Get a product from the database based on id
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>The product we search for</returns>
        Task<ProductViewModel> GetProductByIdAsync(int id);

        /// <summary>
        /// Get a list of products based on category
        /// </summary>
        /// <param name="categoryId">Id of the category</param>
        /// <returns>List with product for specified category</returns>
        Task<IEnumerable<ProductViewModel>> GetProductByCategoryIdAsync(int categoryId);

        /// <summary>
        /// Add an product on database or update an egzistin one
        /// </summary>
        /// <param name="productVm">Product to be added or updated</param>
        void AddOrUpdateProduct(ProductViewModel productVm);

        /// <summary>
        /// Remove an product from the database async
        /// </summary>
        /// <param name="id">Id of the product to be deleted</param>
        /// <returns>True if the product is deleted, false if there is any problem</returns>
        Task<bool> DeleteProductByIdAsync(int id);
        #endregion


        /// <summary>
        /// Delete All Furnitor by Furnitor ID
        /// </summary>
        /// <returns>List with categories</returns>
        Task<bool> DeleteProductsImportedByFurnitorIdAsync(int id,string type);


        Task<bool> DeleteProductsByCategoriIDAsync(int id);


        Task<bool> DeleteProductsImportedByFurnitorAsync(string type);

        /// <summary>
        /// Method to save the data to db
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();
    }
}
