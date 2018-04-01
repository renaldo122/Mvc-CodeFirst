// <copyright file="UnitTest.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/24/2018</date>
// <summary>UnitTest class to test the method used</summary>

namespace Mvc.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mvc.Controllers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Mvc.Domain.ViewModels;
    using Mvc.Domain.Abstract;

    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async void IndexTest()
        {
            CategoryController categoryController = new CategoryController();

            ViewResult result = await categoryController.Details(1) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public async void DetailsTest()
        {
            CategoryController categoryController = new CategoryController();

            ViewResult result = await categoryController.Details(1) as ViewResult;

            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod()]
        public async void CreateTest()
        {
            CategoryController categoryController = new CategoryController();

            FunitorViewModel categoryVm = new FunitorViewModel()
            {
                Id = 22,
                Name = "Test Category",
                Description = "Description for the test category",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            await categoryController.Create(categoryVm);

            Assert.Fail();
        }

    }
}
