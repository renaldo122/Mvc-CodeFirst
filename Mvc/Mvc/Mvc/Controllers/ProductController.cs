// <copyright file="ProductController.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/24/2018</date>
// <summary>Product Controller representing crud operation</summary>

namespace Mvc.Controllers
{
    using ExcelDataReader;
    using log4net;
    using PagedList;
    using Mvc.Domain.Abstract;
    using Mvc.Domain.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ProductController : Controller
    {
        private IEntitiesRepository _repository;
        ILog logger = log4net.LogManager.GetLogger("ErrorLog");

        public ProductController()
        {

        }

        public ProductController(IEntitiesRepository repository)
        {
            _repository = repository;
        }

        #region Constants
        public const string typeProducts = "Product";
        public const string typeFurnitor = "Furnitor";
        #endregion

        #region GetFurnitorList
        private List<SelectListItem> GetFurnitorList()
        {
            var categories = _repository.GetCategoriesAsync();

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in categories)
            {
                list.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            return list;
        }
        private List<SelectListItem> GetMarkaList()
        {
            var marka = _repository.GetMarkaAsync();

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in marka.GroupBy(m => m.Marka))
            {
                if (!string.IsNullOrEmpty(item.Key))
                {
                    list.Add(new SelectListItem { Text = item.Key, Value = item.Key });
                }
            }

            return list;
        }

        
        #endregion

        #region ImportProducts
        public async Task<ActionResult> Import()
        {
            ViewBag.CategoryId = GetFurnitorList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Import(ImportViewModel model, HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                
                if (uploadfile != null && uploadfile.ContentLength > 0)
                {
                    //ExcelDataReader works on binary excel file
                    Stream stream = uploadfile.InputStream;
                    //We need to written the Interface.
                    IExcelDataReader reader = null;
                    if (uploadfile.FileName.EndsWith(".xls"))
                    {
                        //reads the excel file with .xls extension
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (uploadfile.FileName.EndsWith(".xlsx"))
                    {
                        //reads excel file with .xlsx extension
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else if (uploadfile.FileName.EndsWith(".csv"))
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var filePath = path + Path.GetFileName(uploadfile.FileName);
                        string extension = Path.GetExtension(uploadfile.FileName);
                        uploadfile.SaveAs(filePath);
                        string csvData = System.IO.File.ReadAllText(filePath);

                        //foreach (string row in csvData.Split('\n'))
                        //{
                        //    if (!string.IsNullOrEmpty(row))
                        //    {
                        //        customers.Add(new CustomerModel
                        //        {
                        //            CustomerId = Convert.ToInt32(row.Split(',')[0]),
                        //            Name = row.Split(',')[1],
                        //            Country = row.Split(',')[2]
                        //        });
                        //    }
                        //}

                    }
                    else
                    {
                        //Shows error if uploaded file is not Excel file
                        ModelState.AddModelError("File", "This file format is not supported");

                       
                        ViewBag.CategoryId = GetFurnitorList();
                        return View();
                    }
                    //treats the first row of excel file as Coluymn Names
                    // reader.IsFirstRowAsColumnNames = true;
                    //Adding reader data to DataSet()

                    if (model.DeleteExisting)
                    {
                       await _repository.DeleteProductsByCategoriIDAsync(model.CategoryId);
                    }

                    DataSet result = reader.AsDataSet();
                    foreach (DataTable dt in result.Tables)
                    {
                        
                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            ProductViewModel newProdVm = new ProductViewModel();
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[i][column].ToString()))
                                {
                                    if (dt.Rows[0][column].ToString() == "Barcode")
                                    {
                                        newProdVm.Barcode = dt.Rows[i][column].ToString().TrimStart('0');
                                    }
                                    else if (dt.Rows[0][column].ToString() == "Kod")
                                    {
                                        newProdVm.Kod = dt.Rows[i][column].ToString();
                                    }
                                    else if (dt.Rows[0][column].ToString() == "Name")
                                    {
                                        newProdVm.Name = dt.Rows[i][column].ToString();
                                    }
                                    else if (dt.Rows[0][column].ToString() == "Quantity")
                                    {
                                        newProdVm.Quantity = dt.Rows[i][column].ToString();
                                    }
                                    else if (dt.Rows[0][column].ToString() == "Price")
                                    {
                                        newProdVm.Price = Convert.ToDecimal(dt.Rows[i][column]);
                                    }
                                    else if (dt.Rows[0][column].ToString() == "Marka")
                                    {
                                        newProdVm.Marka = dt.Rows[i][column].ToString();
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(newProdVm.Barcode) && !string.IsNullOrEmpty(newProdVm.Name))
                            {
                                newProdVm.CategoryId = model.CategoryId;
                                newProdVm.DateCreated = DateTime.Now;
                                newProdVm.DateModified = DateTime.Now;
                                _repository.AddOrUpdateProduct(newProdVm);
                                }

                        }
                    }

                        await _repository.SaveChangesAsync();
                        //reader.Close();
                        //Sending result data to View
                        return RedirectToAction("Index");
                }
                }
                catch (Exception etc)
                {
                    ModelState.AddModelError("File", "Check your file");
                    ViewBag.CategoryId = GetFurnitorList();
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("File", "Please upload your file");

            }

            ViewBag.CategoryId = GetFurnitorList();
            return View();
        }

        #endregion

        #region ExportExcel Products
        public async Task<ActionResult> ExportToExcel(string currentFilter)
        {
            var gv = new GridView();
            gv.DataSource = await _repository.GetProductsAsync(currentFilter);
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            var filename = DateTime.Now + "-" + "Products.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + "");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");
        }
        #endregion




        #region ExportComparedProducts
        public async Task<ActionResult> ExportComparedProduct()
        {
            var gv = new GridView();
            gv.DataSource = await _repository.GetComparedProductsAsync(typeProducts);
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            var filename = DateTime.Now + "-" + "ProductsCompared.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + "");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");
        }
        #endregion


        #region ExportComparedProducts
        public async Task<ActionResult> ExportComparedFurnitors()
        {
            var gv = new GridView();
            gv.DataSource = await _repository.GetComparedProductsAsync(typeFurnitor);
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            var filename = DateTime.Now + "-" + "FurnitorCompared.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + "");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");
        }
        #endregion

        public async Task<ActionResult> CmpFurnitor(int? page)
        {
            try
            {
                var model = await _repository.GetProductsToCompareAsync(typeFurnitor);
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting product list: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        public async Task<ActionResult> ImportFurnitors()
        {
            var model = new CompareFurnitorsViewModel();
            ViewBag.CategoryId1 = GetFurnitorList();
            ViewBag.CategoryId2 = GetFurnitorList();
            ViewBag.CategoryId3 = GetFurnitorList();
            ViewBag.CategoryId4 = GetFurnitorList();
            ViewBag.MarkaName1 = GetMarkaList();
            ViewBag.MarkaName2 = GetMarkaList();
            ViewBag.MarkaName3 = GetMarkaList();
            ViewBag.MarkaName4 = GetMarkaList();
            ViewBag.MarkaName5 = GetMarkaList();
            model.DeleteExisting = true;
            return View(model);
        }

        public async Task<ActionResult> CompareFurnitors(CompareFurnitorsViewModel model)
        {
            try
            {
                ViewBag.CategoryId1 = GetFurnitorList();
                ViewBag.CategoryId2 = GetFurnitorList();
                ViewBag.CategoryId3 = GetFurnitorList();
                ViewBag.CategoryId4 = GetFurnitorList();
                ViewBag.MarkaName1 = GetMarkaList();
                ViewBag.MarkaName2 = GetMarkaList();
                ViewBag.MarkaName3 = GetMarkaList();
                ViewBag.MarkaName4 = GetMarkaList();
                ViewBag.MarkaName5 = GetMarkaList();

                model.DeleteExisting = true;
                if (ModelState.IsValid)
                {
                   if(!model.ComparebyMarka)
                    {
                        try
                        {


                            if (string.IsNullOrEmpty(model.CategoryId3.ToString()))
                            {
                                model.CategoryId3 = 0;
                            }
                            if (string.IsNullOrEmpty(model.CategoryId4.ToString()))
                            {
                                model.CategoryId4 = 0;
                            }
                            #region deleteProductBy furnitor
                            await _repository.DeleteProductsImportedByFurnitorAsync(typeFurnitor);
                            await _repository.SaveChangesAsync();

                            #endregion

                            var allCategories = await _repository.ListOfProductsImportedToCompareByCategorNoMarkayAsync(Convert.ToInt32(model.CategoryId1), Convert.ToInt32(model.CategoryId2), Convert.ToInt32(model.CategoryId3), Convert.ToInt32(model.CategoryId4));


                            var uniqBarcode = await _repository.ListOfProductsImportedToCompareByCategorNoMarkayAsync(Convert.ToInt32(model.CategoryId1), Convert.ToInt32(model.CategoryId2), Convert.ToInt32(model.CategoryId3), Convert.ToInt32(model.CategoryId4));


                            foreach (var itemfunitor in uniqBarcode.GroupBy(uq => uq.Barcode))
                            {
                                var itemwithsmallprice = allCategories.Where(fu => fu.Barcode == itemfunitor.Key).OrderBy(fu => fu.Price);
                                if (itemwithsmallprice.Count() >= 2)
                                {
                                    var itemProductsS = itemwithsmallprice.FirstOrDefault();
                                    var itemProductsB = itemwithsmallprice.Skip(1).Take(1).FirstOrDefault();
                                    var compareVm = new CompareViewModel
                                    {
                                        Barcode = itemProductsS.Barcode,
                                        Kod = itemProductsS.Kod,
                                        Name = itemProductsS.Name,
                                        CategoryId = itemProductsS.CategoryId,
                                        Quantity = itemProductsS.Quantity,
                                        ProductPrice = itemProductsS.Price,
                                        FurnitorPrice = itemProductsB.Price,
                                        SmallPrice = itemProductsS.Price - itemProductsB.Price,
                                        ProductFound = "true",
                                        Type = typeFurnitor,
                                        Marka = itemProductsS.Marka,
                                        Klasifikim = itemProductsS.Klasifikim,
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                    };
                                    _repository.AddOrUpdateCompare(compareVm);

                                }
                                else
                                {
                                    var itemProducts = itemwithsmallprice.FirstOrDefault();
                                    var compareVm = new CompareViewModel
                                    {
                                        Barcode = itemProducts.Barcode,
                                        Kod = itemProducts.Kod,
                                        Name = itemProducts.Name,
                                        CategoryId = itemProducts.CategoryId,
                                        Quantity = itemProducts.Quantity,
                                        ProductPrice = itemProducts.Price,
                                        FurnitorPrice = 0,
                                        SmallPrice = itemProducts.Price,
                                        ProductFound = "false",
                                        Type = typeFurnitor,
                                        Marka = itemProducts.Marka,
                                        Klasifikim = itemProducts.Klasifikim,
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                    };
                                    _repository.AddOrUpdateCompare(compareVm);
                                }


                            }

                            await _repository.SaveChangesAsync();
                            return RedirectToAction("CmpFurnitor");
                        }
                        catch (Exception etc)
                        {

                            ViewBag.CategoryId1 = GetFurnitorList();
                            ViewBag.CategoryId2 = GetFurnitorList();
                            ViewBag.CategoryId3 = GetFurnitorList();
                            ViewBag.CategoryId4 = GetFurnitorList();
                            ViewBag.MarkaName1 = GetMarkaList();
                            ViewBag.MarkaName2 = GetMarkaList();
                            ViewBag.MarkaName3 = GetMarkaList();
                            ViewBag.MarkaName4 = GetMarkaList();
                            ViewBag.MarkaName5 = GetMarkaList();
                            ModelState.AddModelError("Data", "Check Data Compare");
                            return View("ImportFurnitors", model);
                        }
                    }
                   else
                    {
                        try
                        {


                            if (string.IsNullOrEmpty(model.CategoryId3.ToString()))
                            {
                                model.CategoryId3 = 0;
                            }
                            if (string.IsNullOrEmpty(model.CategoryId4.ToString()))
                            {
                                model.CategoryId4 = 0;
                            }

                            if (string.IsNullOrEmpty(model.MarkaName1))
                            {
                                model.MarkaName1 = "notfound";
                            }
                            if (string.IsNullOrEmpty(model.MarkaName2))
                            {
                                model.MarkaName2 = "notfound";
                            }
                            if (string.IsNullOrEmpty(model.MarkaName3))
                            {
                                model.MarkaName3 = "notfound";
                            }
                            if (string.IsNullOrEmpty(model.MarkaName4))
                            {
                                model.MarkaName4 = "notfound";
                            }
                            if (string.IsNullOrEmpty(model.MarkaName5))
                            {
                                model.MarkaName5 = "notfound";
                            }

                            #region deleteProductBy furnitor
                            await _repository.DeleteProductsImportedByFurnitorAsync(typeFurnitor);
                            await _repository.SaveChangesAsync();

                            #endregion

                            var allCategories = await _repository.ListOfProductsImportedToCompareByCategoryAsync(
                                Convert.ToInt32(model.CategoryId1), Convert.ToInt32(model.CategoryId2), Convert.ToInt32(model.CategoryId3), Convert.ToInt32(model.CategoryId4),
                                model.MarkaName1,model.MarkaName2,model.MarkaName3,model.MarkaName4,model.MarkaName5 );


                            var uniqBarcode = await _repository.ListOfProductsImportedToCompareByCategoryAsync(
                                Convert.ToInt32(model.CategoryId1), Convert.ToInt32(model.CategoryId2), Convert.ToInt32(model.CategoryId3), Convert.ToInt32(model.CategoryId4),
                                 model.MarkaName1, model.MarkaName2, model.MarkaName3, model.MarkaName4, model.MarkaName5);


                            foreach (var itemfunitor in uniqBarcode.GroupBy(uq => uq.Barcode))
                            {
                                var itemwithsmallprice = allCategories.Where(fu => fu.Barcode == itemfunitor.Key).OrderBy(fu => fu.Price);
                                if (itemwithsmallprice.Count() >= 2)
                                {
                                    var itemProductsS = itemwithsmallprice.FirstOrDefault();
                                    var itemProductsB = itemwithsmallprice.Skip(1).Take(1).FirstOrDefault();
                                    var compareVm = new CompareViewModel
                                    {
                                        Barcode = itemProductsS.Barcode,
                                        Kod = itemProductsS.Kod,
                                        Name = itemProductsS.Name,
                                        CategoryId = itemProductsS.CategoryId,
                                        Quantity = itemProductsS.Quantity,
                                        ProductPrice = itemProductsS.Price,
                                        FurnitorPrice = itemProductsB.Price,
                                        SmallPrice = itemProductsS.Price - itemProductsB.Price,
                                        ProductFound = "true",
                                        Type = typeFurnitor,
                                        Marka = itemProductsS.Marka,
                                        Klasifikim = itemProductsS.Klasifikim,
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                    };
                                    _repository.AddOrUpdateCompare(compareVm);

                                }
                                else
                                {
                                    var itemProducts = itemwithsmallprice.FirstOrDefault();
                                    var compareVm = new CompareViewModel
                                    {
                                        Barcode = itemProducts.Barcode,
                                        Kod = itemProducts.Kod,
                                        Name = itemProducts.Name,
                                        CategoryId = itemProducts.CategoryId,
                                        Quantity = itemProducts.Quantity,
                                        ProductPrice = itemProducts.Price,
                                        FurnitorPrice = 0,
                                        SmallPrice = itemProducts.Price,
                                        ProductFound = "false",
                                        Type = typeFurnitor,
                                        Marka = itemProducts.Marka,
                                        Klasifikim = itemProducts.Klasifikim,
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                    };
                                    _repository.AddOrUpdateCompare(compareVm);
                                }


                            }

                            await _repository.SaveChangesAsync();
                            return RedirectToAction("CmpFurnitor");
                        }
                        catch (Exception etc)
                        {

                            ViewBag.CategoryId1 = GetFurnitorList();
                            ViewBag.CategoryId2 = GetFurnitorList();
                            ViewBag.CategoryId3 = GetFurnitorList();
                            ViewBag.CategoryId4 = GetFurnitorList();
                            ViewBag.MarkaName1 = GetMarkaList();
                            ViewBag.MarkaName2 = GetMarkaList();
                            ViewBag.MarkaName3 = GetMarkaList();
                            ViewBag.MarkaName4 = GetMarkaList();
                            ViewBag.MarkaName5 = GetMarkaList();
                            ModelState.AddModelError("Data", "Check Data Compare");
                            return View("ImportFurnitors", model);
                        }
                    }
                   
                   
                }
                return View("ImportFurnitors", model);
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting product list: Message =>" + ex.Message + " " + ex.StackTrace);
                return View("ImportFurnitors", model);
            }
        }
       

        #region CompareProducts

        public async Task<ActionResult> Compare(int? page, string name)
        {
            try
            {
                var model = await _repository.GetProductsToCompareAsync(typeProducts);
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting product list: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        public async Task<ActionResult> ImportProducts()
        {
            var model = new ImportProductsViewModel();
         
            ViewBag.CategoryId = GetFurnitorList();
            model.DeleteExisting = true;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Compare(ImportProductsViewModel model, HttpPostedFileBase uploadfile)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadfile != null && uploadfile.ContentLength > 0)
                    {
                        #region deleteProductBy furnitor
                        await _repository.DeleteProductsImportedByFurnitorIdAsync(model.CategoryId, typeProducts);
                        await _repository.SaveChangesAsync();
                        #endregion

                        //ExcelDataReader works on binary excel file
                        Stream stream = uploadfile.InputStream;
                        //We need to written the Interface.
                        IExcelDataReader reader = null;
                        if (uploadfile.FileName.EndsWith(".xls"))
                        {
                            //reads the excel file with .xls extension
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (uploadfile.FileName.EndsWith(".xlsx"))
                        {
                            //reads excel file with .xlsx extension
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else if (uploadfile.FileName.EndsWith(".csv"))
                        {
                            string path = Server.MapPath("~/Uploads/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            var filePath = path + Path.GetFileName(uploadfile.FileName);
                            string extension = Path.GetExtension(uploadfile.FileName);
                            uploadfile.SaveAs(filePath);
                            string csvData = System.IO.File.ReadAllText(filePath);

                            //foreach (string row in csvData.Split('\n'))
                            //{
                            //    if (!string.IsNullOrEmpty(row))
                            //    {
                            //        customers.Add(new CustomerModel
                            //        {
                            //            CustomerId = Convert.ToInt32(row.Split(',')[0]),
                            //            Name = row.Split(',')[1],
                            //            Country = row.Split(',')[2]
                            //        });
                            //    }
                            //}

                        }
                        else
                        {
                            //Shows error if uploaded file is not Excel file
                            ModelState.AddModelError("File", "This file format is not supported");


                            ViewBag.CategoryId = GetFurnitorList();
                            return View();
                        }
                        //treats the first row of excel file as Coluymn Names
                        // reader.IsFirstRowAsColumnNames = true;
                        //Adding reader data to DataSet()
                        DataSet result = reader.AsDataSet();
                        foreach (DataTable dt in result.Tables)
                        {
                            for (int i = 1; i < dt.Rows.Count; i++)
                            {
                                ProductViewModel newProdVm = new ProductViewModel();
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (!string.IsNullOrEmpty(dt.Rows[i][column].ToString()))
                                    {
                                        if (dt.Rows[0][column].ToString() == "Barcode")
                                        {
                                            newProdVm.Barcode = dt.Rows[i][column].ToString().TrimStart('0');
                                        }
                                        else if (dt.Rows[0][column].ToString() == "Kod")
                                        {
                                            newProdVm.Kod = dt.Rows[i][column].ToString();
                                        }
                                        else if (dt.Rows[0][column].ToString() == "Name")
                                        {
                                            newProdVm.Name = dt.Rows[i][column].ToString();
                                        }
                                        else if (dt.Rows[0][column].ToString() == "Quantity")
                                        {
                                            newProdVm.Quantity = dt.Rows[i][column].ToString();
                                        }
                                        else if (dt.Rows[0][column].ToString() == "Price")
                                        {
                                            newProdVm.Price = Convert.ToDecimal(dt.Rows[i][column]);
                                        }
                                        else if (dt.Rows[0][column].ToString() == "Klasifikim")
                                        {
                                            newProdVm.Klasifikim = dt.Rows[i][column].ToString();
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(newProdVm.Barcode) && !string.IsNullOrEmpty(newProdVm.Name))
                                {
                                    newProdVm.CategoryId = model.CategoryId;
                                    newProdVm.DateCreated = DateTime.Now;
                                    newProdVm.DateModified = DateTime.Now;
                                    _repository.AddOrUpdateProduct(newProdVm);
                                }
                            }
                        }
                         await _repository.SaveChangesAsync();
                        //reader.Close();
                        //Sending result data to View

                        #region CompareProducts

                        await CompareProductsToFindBestPrices(model.CategoryId);

                        #endregion

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please upload your file");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("File", "Error Compare Data");
                    ViewBag.CategoryId = GetFurnitorList();
                    return View("ImportProducts", model);
                }

               
            }
            ViewBag.CategoryId = GetFurnitorList();
            return View("ImportProducts",model);
        }

        private async Task<bool> CompareProductsToFindBestPrices(int id)
        {
            var ListOfProductsImportedToCompare = await _repository.ListOfProductsImportedToCompareByCategoryIDAsync(id);
            var ListOfFurnitorImportedToCompare = await _repository.ListOfFurnitorImportedToCompareByCategoryAsync(id);

            foreach (var itemProducts in ListOfProductsImportedToCompare)
            {
                var itemFurnitor = ListOfFurnitorImportedToCompare.Where(fu => fu.Barcode == itemProducts.Barcode).OrderBy(fu => fu.Price);
                if (!itemFurnitor.Any())
                {
                    var compareVm = new CompareViewModel
                    {
                        Barcode = itemProducts.Barcode,
                        Kod = itemProducts.Kod,
                        Name = itemProducts.Name,
                        CategoryId = itemProducts.CategoryId,
                        Quantity = itemProducts.Quantity,
                        ProductPrice = itemProducts.Price,
                        FurnitorPrice = 0,
                        SmallPrice = itemProducts.Price - 0,
                        ProductFound = "false",
                        Marka= itemProducts.Marka,
                        Type = typeProducts,
                        Klasifikim=itemProducts.Klasifikim,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                    };
                    _repository.AddOrUpdateCompare(compareVm);

                }
                else
                {
                    var furnitor = itemFurnitor.FirstOrDefault();
                    var compareVm = new CompareViewModel
                    {
                        Barcode = furnitor.Barcode,
                        Kod = furnitor.Kod,
                        Name = furnitor.Name,
                        CategoryId = furnitor.CategoryId,
                        Quantity = itemProducts.Quantity,
                        ProductPrice = itemProducts.Price,
                        FurnitorPrice = furnitor.Price,
                        SmallPrice = furnitor.Price - itemProducts.Price,
                        ProductFound = "true",
                        Type = typeProducts,
                        Marka = furnitor.Marka,
                        Klasifikim = itemProducts.Klasifikim,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                    };
                    _repository.AddOrUpdateCompare(compareVm);

                }
            }

            await _repository.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Index/CRUD Operation Products

        // GET: Product
        /// <summary>
        /// Action method to get all the products
        /// </summary>
        /// <param name="id">Id of the category to get the product</param>
        /// <returns>The list with products or the product category</returns>
        public async Task<ActionResult> Index(int? page, string currentFilter)
        {
            try
            {
                if (!string.IsNullOrEmpty(currentFilter))
                {
                    page = 1;
                }
                else
                {
                    ViewBag.CurrentFilter = currentFilter;
                }
                // var model = id == 0 ? await _repository.GetProductsAsync(currentFilter) : await _repository.GetProductByCategoryIdAsync(id);
                var model = await _repository.GetProductsAsync(currentFilter);
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting product list: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // GET: Product/Details/5
        /// <summary>
        /// Action method to get the product details
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>The category product</returns>
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var product = await _repository.GetProductByIdAsync(id);
                return View(product);
            }
            catch (InvalidCastException ex)
            {
                logger.Info("Error on getting product details, Invalid product id: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting product details: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // GET: Product/Create
        /// <summary>
        /// Get action method to create product 
        /// </summary>
        /// <returns>View with product model</returns>
        public async Task<ActionResult> Create()
        {
            try
            {
               
                ViewBag.CategoryId = GetFurnitorList(); 
                return View();
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting product to create: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // POST: Product/Create
        /// <summary>
        /// Post action method to create a new product
        /// </summary>
        /// <param name="newProdVm">Product view model</param>
        /// <returns>The new product</returns>
        [HttpPost]
        public async Task<ActionResult> Create(ProductViewModel newProdVm)
        {
            try
            {
                newProdVm.DateCreated = DateTime.Now;
                newProdVm.DateModified = DateTime.Now;

                _repository.AddOrUpdateProduct(newProdVm);
                await _repository.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Info("Error on creating new product: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // GET: Product/Edit/5
        /// <summary>
        /// Get product details to modify
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>The product details to modify</returns>
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var product = await _repository.GetProductByIdAsync(id);

                ViewBag.CategoryId = GetFurnitorList(); 

                return View(product);
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting product details to modify: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // POST: Product/Edit/5
        /// <summary>
        /// Post action method to modify the product
        /// </summary>
        /// <param name="editProdVm">Product view model</param>
        /// <returns>The modified product</returns>
        [HttpPost]
        public async Task<ActionResult> Edit(ProductViewModel editProdVm)
        {
            try
            {
                editProdVm.DateModified = DateTime.Now;

                _repository.AddOrUpdateProduct(editProdVm);
                await _repository.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Info("Error on modifing product: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // GET: Product/Delete/5
        /// <summary>
        /// Get product details to delete
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>The product details to delete</returns>
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var product = await _repository.GetProductByIdAsync(id);
                return View(product);
            }
            catch (Exception ex)
            {
                logger.Info("Error on getting product details to delete: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        // POST: Product/Delete/5
        /// <summary>
        /// Post action method to delete product
        /// </summary>
        /// <param name="id">Id of the product to delete</param>
        /// <returns>True if the product is deleted</returns>
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repository.DeleteProductByIdAsync(id);
                await _repository.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Info("Error on delete product: Message =>" + ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

#endregion

    }
}
