using ProERP.Services.Product;
using ProERP.Web.Framework;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProERP.Web.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {

        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            this._productService = productService;
        }

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,QA,QAManager")]
        public ActionResult List()
        {
            return View();
        }

        public JsonNetResult SaveProduct(Data.Models.ProductMaster productData)
        {
            var result = new JsonResponse();
            try
            {
                if (productData.Id > 0)
                {
                    this._productService.UpdateProduct(productData);
                    result.Status = JsonResponseStatus.Success;
                    result.Message = "Product updated";
                }
                else
                {
                    this._productService.SaveProduct(productData);
                    result.Status = JsonResponseStatus.Success;
                    result.Message = "Product saved";
                }
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Message = ProcessException(ex);
            }
            return JsonNet(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList(string productName)
        {
            var allData = this._productService.GetProdutListForGrid(productName);
            var productData = allData.Select(s => new
            {
                s.Id,
                s.CategoryId,
                CategoryName = s.ProductCategory.Name,
                s.Code,
                s.Name,
                s.ShortName,
                s.Description
            });
            return Json(productData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductById(int Id)
        {
            ProductViewModel model = new ProductViewModel();
            Data.Models.ProductMaster objProduct = this._productService.GetProductById(Id);
            if (objProduct != null)
            {
                model.Id = objProduct.Id;
                model.CategoryId = objProduct.CategoryId;
                model.Code = objProduct.Code;
                model.Name = objProduct.Name;
                model.ShortName = objProduct.ShortName;
                model.Description = objProduct.Description;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProduct(int Id)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
                int id = this._productService.DeleteProduct(Id);
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = ProcessException(ex) };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetProductCategoryList()
        {
            var allData = this._productService.GetProductCategoryList();
            return JsonNet(allData.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetProductListById(int categoryId)
        {
            var allData = this._productService.GetProductListById(categoryId);
            return JsonNet(allData.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetGradeNameByID(int Id)
        {
            JsonResponse resonse = new JsonResponse();
            var productName = this._productService.GetGradeNameById(Id);
            resonse.Data = new { GradeId = productName.Id , GradeName = productName.Name };
            return JsonNet(resonse, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetProductByLineId(int LineId)
        {
            var productData =this._productService.GetProductNameByLine(LineId);
            return JsonNet(productData.Select(s => new { s.Id , s.Name }), JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetProductByBatchId(int BatchId)
        {
            var productData = this._productService.GetProductByBatchId(BatchId);
            ProductDropDownModel model = new ProductDropDownModel();
            model.ProductId = productData.Id;
            model.GradeName = productData.Name;
            return JsonNet(model, JsonRequestBehavior.AllowGet);
        }

    }
}