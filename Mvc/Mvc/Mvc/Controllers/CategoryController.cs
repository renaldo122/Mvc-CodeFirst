// <copyright file="CategoryController.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/24/2018</date>
// <summary>Category Controller representing crud operation</summary>

namespace Mvc.Controllers
{
    using log4net;
    using Mvc.Domain.Abstract;
    using Mvc.Domain.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    public class CategoryController : Controller
    {
        private IEntitiesRepository _repository;
        ILog logger = log4net.LogManager.GetLogger("ErrorLog");

        public CategoryController()
        {

        }

        public CategoryController(IEntitiesRepository repository)
        {
            _repository = repository;
        }

   

        // GET: Category
        /// <summary>
        /// Action method to get all the category
        /// </summary>
        /// <returns>The list with category</returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                var model = _repository.GetCategoriesAsync();

                return View(model);
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting category list: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // GET: Category/Details/5
        /// <summary>
        /// Action method to get the category details
        /// </summary>
        /// <param name="id">Id of the category</param>
        /// <returns>The category details</returns>
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var category = await _repository.GetCategoryByIdAsync(id);
                return View(category);
            }
            catch (InvalidCastException ex)
            {
                logger.Info("Error on getting category details, Invalid category id: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting category details: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // GET: Category/Create
        /// <summary>
        /// Get action method to create category 
        /// </summary>
        /// <returns>View with category model</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        /// <summary>
        /// Post action method to create a new category
        /// </summary>
        /// <param name="newCategoryVm">Category view model</param>
        /// <returns>The new category</returns>
        [HttpPost]
        public async Task<ActionResult> Create(FunitorViewModel newCategoryVm)
        {
            try
            {
                newCategoryVm.DateCreated = DateTime.Now;
                newCategoryVm.DateModified = DateTime.Now;
                _repository.AddOrUpdateCategory(newCategoryVm);
                await _repository.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                logger.Info("Error on create new category: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // GET: Category/Edit/5
        /// <summary>
        /// Get category details to modify
        /// </summary>
        /// <param name="id">Id of the category</param>
        /// <returns>The category details to modify</returns>
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var category = await _repository.GetCategoryByIdAsync(id);
                return View(category);
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting category details to modify: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // POST: Category/Edit/5
        /// <summary>
        /// Post action method to modify the category
        /// </summary>
        /// <param name="editCategryVm">Category view model</param>
        /// <returns>The modified category</returns>
        [HttpPost]
        public async Task<ActionResult> Edit(FunitorViewModel editCategryVm)
        {
            try
            {
                editCategryVm.DateModified = DateTime.Now;
                _repository.AddOrUpdateCategory(editCategryVm);
                await _repository.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                logger.Info("Error on modify category: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // GET: Category/Delete/5
        /// <summary>
        /// Get category details to delete
        /// </summary>
        /// <param name="id">Id of the category</param>
        /// <returns>The category details to delete</returns>
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var category = await _repository.GetCategoryByIdAsync(id);
                return View(category);
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting category details to delete: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // POST: Category/Delete/5
        /// <summary>
        /// Post action method to delete category
        /// </summary>
        /// <param name="id">Id of the category to delete</param>
        /// <returns>True if the category is deleted</returns>
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                
                await _repository.DeleteCategoryByIdAsync(id);
                await _repository.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                logger.Info("Error on delete category: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }
    }
}
