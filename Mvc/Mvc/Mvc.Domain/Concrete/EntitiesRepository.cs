// <copyright file="EntitiesRepository.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/22/2018</date>
// <summary>EntitiesRepository representing implementation of the repository methods</summary>

namespace Mvc.Domain.Concrete
{
    using Mvc.Domain.Abstract;
    using Mvc.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Mvc.Domain.ViewModels;
    using Mvc.Domain.Extension;
    using System.Data.Entity.Migrations;
    using System.Data.Entity;

    public class EntitiesRepository : IEntitiesRepository
    {
        public ApplicationDbContext context = new ApplicationDbContext();

        /// <summary>
        /// Add an category on database or update an egzistin one
        /// </summary>
        /// <param name="categoryVm">Category to be added or updated</param>
        public void AddOrUpdateCategory(FunitorViewModel categoryVm)
        {
            Helper helper = new Helper();
            Funitor addOrUpdateCategory = helper.ToCategory(categoryVm);

            context.Funitor.AddOrUpdate(addOrUpdateCategory);
        }


        /// <summary>
        /// Add an category on database or update an egzistin one
        /// </summary>
        /// <param name="compareVm">Category to be added or updated</param>
        public void AddOrUpdateCompare(CompareViewModel compareVm)
        {
            Helper helper = new Helper();
            Compare addOrUpdateCompare = helper.ToCompare(compareVm);

            context.Compare.AddOrUpdate(addOrUpdateCompare);
        }


        /// <summary>
        /// Add an product on database or update an egzistin one
        /// </summary>
        /// <param name="productVm">Product to be added or updated</param>
        public void AddOrUpdateProduct(ProductViewModel productVm)
        {
            Helper helper = new Helper();
            Product addOrUpdateProduct = helper.ToProduct(productVm);

            context.Product.AddOrUpdate(addOrUpdateProduct);
        }

        /// <summary>
        /// Remove an category from the database async
        /// </summary>
        /// <param name="id">Id of the category to be deleted</param>
        /// <returns>True if the category is deleted, false if there is any problem</returns>
        public async Task<bool> DeleteCategoryByIdAsync(int id)
        {
            try
            {
                Funitor category = await context.Funitor.FindAsync(id);
                context.Funitor.Remove(category);

                return true;

            }
            catch (Exception ex)
            {
                //TODO: log exception here
                return false;
            }

        }

        /// <summary>
        /// Remove an product from the database async
        /// </summary>
        /// <param name="id">Id of the product to be deleted</param>
        /// <returns>True if the product is deleted, false if there is any problem</returns>
        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            try
            {
                Product product = await context.Product.FindAsync(id);
                context.Product.Remove(product);

                return true;

            }
            catch (Exception ex)
            {
                //TODO: log exception here
                return false;
            }
        }


        /// <summary>
        /// Remove an product from the database async
        /// </summary>
        /// <param name="id">Id of the product to be deleted</param>
        /// <returns>True if the product is deleted, false if there is any problem</returns>
        public async Task<bool> DeleteProductsImportedByFurnitorIdAsync(int id, string type)
        {
            try
            {
                const string query = "DELETE FROM [dbo].[Products] WHERE [CategoryId]={0}";
                var rows = context.Database.ExecuteSqlCommand(query, id);

                const string query1 = "DELETE FROM [dbo].[Compares]  WHERE [Type]='Product'";
                var rows1 = context.Database.ExecuteSqlCommand(query1, type);

                return true;
            }
            catch (Exception ex)
            {
                //TODO: log exception here
                return false;
            }
        }

        public async Task<bool> DeleteProductsByCategoriIDAsync(int id)
        {
            try
            {
                const string query = "DELETE FROM [dbo].[Products] WHERE [CategoryId]={0}";
                var rows = context.Database.ExecuteSqlCommand(query, id);
                return true;
            }
            catch (Exception ex)
            {
                //TODO: log exception here
                return false;
            }
        }

        

        public async Task<bool> DeleteProductsImportedByFurnitorAsync( string type)
        {
            try
            {

                const string query1 = "DELETE FROM [dbo].[Compares]  WHERE [Type]='Furnitor'";
                var rows1 = context.Database.ExecuteSqlCommand(query1);

                return true;
            }
            catch (Exception ex)
            {
                //TODO: log exception here
                return false;
            }
        }

        


        /// <summary>
        /// Get all categories from the database async
        /// </summary>
        /// <returns>List with categories</returns>
        public IEnumerable<FunitorViewModel> GetCategoriesAsync()
        {
            IQueryable<Funitor> query = context.Funitor;
            return query.Select(x => new FunitorViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified
            }).ToList();
        }


        public IEnumerable<ProductViewModel> GetMarkaAsync()
        {
            IQueryable<Product> query = context.Product;
            return query.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Marka = x.Marka,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified
            }).ToList();
        }

        /// <summary>
        /// Get a category from the database based on id
        /// </summary>
        /// <param name="id">Id of the category</param>
        /// <returns>The category we search for</returns>
        public async Task<FunitorViewModel> GetCategoryByIdAsync(int id)
        {
            IQueryable<Funitor> query = context.Funitor;

            return await query.Where(x => x.Id == id).Select(x => new FunitorViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                Description = x.Description
            }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get a list of products based on category
        /// </summary>
        /// <param name="categoryId">Id of the category</param>
        /// <returns>List with product for specified category</returns>
        public async Task<IEnumerable<ProductViewModel>> GetProductByCategoryIdAsync(int categoryId)
        {
            IQueryable<Product> query = context.Product;

            return await query.Where(x => x.CategoryId == categoryId).Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                CategoryId = x.CategoryId,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified
            }).ToListAsync();
        }

        /// <summary>
        /// Get a product from the database based on id
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>The product we search for</returns>
        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            IQueryable<Product> query = context.Product;

            return await query.Where(x => x.Id == id).Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                CategoryId = x.CategoryId,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                FunitorName = context.Funitor.Where(fu => fu.Id == x.CategoryId).FirstOrDefault().Name
            }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        public async Task<IEnumerable<ProductViewModel>> GetProductsAsync(string currentFilter)
        {
            IQueryable<Product> query;
            if (!string.IsNullOrEmpty(currentFilter))
            {
                query = context.Product.Where(pr => pr.Name.Contains(currentFilter));
            }
            else
            {
                query = context.Product;
            }

            return await query.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                Marka=x.Marka,
                Klasifikim=x.Klasifikim,
                Price = x.Price,
                Quantity = x.Quantity,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                CategoryId = x.CategoryId,
                FunitorName = context.Funitor.Where(fu => fu.Id == x.CategoryId).FirstOrDefault().Name,
            }).ToListAsync();
        }


        public async Task<IEnumerable<CompareViewModel>> GetComparedProductsAsync(string type)
        {
            IQueryable<Compare> query = context.Compare.Where(cp=>cp.Type== type);

            return await query.Select(x => new CompareViewModel()
            {
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                FurnitorPrice = x.FurnitorPrice,
                ProductPrice = x.ProductPrice,
                SmallPrice = x.SmallPrice,
                Quantity = x.Quantity,
                ProductFound = x.ProductFound,
                Marka=x.Marka,
                Klasifikim=x.Klasifikim,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                CategoryId = x.CategoryId,
                FunitorName = context.Funitor.Where(fu => fu.Id == x.CategoryId).FirstOrDefault().Name,
            }).ToListAsync();
        }

        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        public async Task<IEnumerable<CompareViewModel>> ExportComparedProduct()
        {
            IQueryable<Compare> query = context.Compare;

            return await query.Select(x => new CompareViewModel()
            {
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                FurnitorPrice = x.FurnitorPrice,
                ProductPrice = x.ProductPrice,
                SmallPrice = x.SmallPrice,
                Quantity = x.Quantity,
                ProductFound = x.ProductFound,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                FunitorName = context.Funitor.Where(fu => fu.Id == x.CategoryId).FirstOrDefault().Name,
            }).ToListAsync();
        }


        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        public async Task<IEnumerable<ProductViewModel>> ListOfProductsImportedToCompareByCategorNoMarkayAsync(
            int CategoryId1, int CategoryId2, int CategoryId3, int CategoryId4)
        {
            IQueryable<Product> query = context.Product.Where(
                pr => pr.CategoryId == CategoryId1 || pr.CategoryId == CategoryId2 || pr.CategoryId == CategoryId3 || pr.CategoryId == CategoryId4);

            return await query.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                Klasifikim = x.Klasifikim,
                Marka = x.Marka,
                CategoryId = x.CategoryId,
                FunitorName = context.Funitor.Where(fu => fu.Id == x.CategoryId).FirstOrDefault().Name,
            }).ToListAsync();
        }




        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        public async Task<IEnumerable<ProductViewModel>> ListOfProductsImportedToCompareByCategoryAsync(
            int CategoryId1, int CategoryId2, int CategoryId3, int CategoryId4,
             string MarkaName1, string MarkaName2, string MarkaName3, string MarkaName4, string MarkaName5)
        {
            IQueryable<Product> query = context.Product.Where(
                pr => (pr.CategoryId == CategoryId1 || pr.CategoryId == CategoryId2 || pr.CategoryId == CategoryId3 || pr.CategoryId == CategoryId4)
               &&( pr.Marka == MarkaName1 || pr.Marka == MarkaName2 || pr.Marka == MarkaName3 || pr.Marka == MarkaName4 || pr.Marka == MarkaName5));
            
            return await query.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                Klasifikim=x.Klasifikim,
                Marka=x.Marka,
                CategoryId = x.CategoryId,
                FunitorName = context.Funitor.Where(fu => fu.Id == x.CategoryId).FirstOrDefault().Name,
            }).ToListAsync();
        }


        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        public async Task<IEnumerable<ProductViewModel>> ListOfProductsImportedToCompareByCategoryIDAsync(int CategoryId1)
        {
            IQueryable<Product> query = context.Product.Where(pr => pr.CategoryId == CategoryId1 );

            return await query.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                Klasifikim = x.Klasifikim,
                CategoryId = x.CategoryId,
                FunitorName = context.Funitor.Where(fu => fu.Id == x.CategoryId).FirstOrDefault().Name,
            }).ToListAsync();
        }


        
        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        public async Task<IEnumerable<ProductViewModel>> ListOfFurnitorImportedToCompareByCategoryAsync(int categoriId)
        {
            IQueryable<Product> query = context.Product.Where(fu=>fu.CategoryId!=categoriId);

            return await query.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                CategoryId = x.CategoryId,
                FunitorName = context.Funitor.Where(fu => fu.Id == x.CategoryId).FirstOrDefault().Name,
            }).ToListAsync();
        }


        /// <summary>
        /// Get all products from the database async
        /// </summary>
        /// <returns>List with products</returns>
        public async Task<IEnumerable<CompareViewModel>> GetProductsToCompareAsync(string type)
        {

            IQueryable<Compare> query = context.Compare.Where(cp=>cp.Type== type);

            return await query.Select(x => new CompareViewModel()
            {
                Id = x.Id,
                Barcode = x.Barcode,
                Kod = x.Kod,
                Name = x.Name,
                FurnitorPrice = x.FurnitorPrice,
                ProductPrice = x.ProductPrice,
                SmallPrice=x.SmallPrice,
                Quantity = x.Quantity,
                ProductFound = x.ProductFound,
                Klasifikim=x.Klasifikim,
                Marka=x.Marka,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                CategoryId = x.CategoryId,
                FunitorName = context.Funitor.Where(fu => fu.Id == x.CategoryId).FirstOrDefault().Name,
            }).ToListAsync();
        }


        

        /// <summary>
        /// Method to save the changes
        /// </summary>
        /// <returns>True if changes are succesfully, false if not</returns>
        public async Task<bool> SaveChangesAsync()
        {
            await context.SaveChangesAsync();
            return true;
        }
    }
}
