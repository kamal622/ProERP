using ProERP.Services.Indents;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProERP.Web.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IndentServices _indentService;

        public PurchaseOrderController(IndentServices indentService)
        {
            this._indentService = indentService;
        }

        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            IndentsViewModel model = new IndentsViewModel();

            return View(model);
        }
        public JsonResult GetPOById(int Id)
        {
            IndentsViewModel model = new IndentsViewModel();

            Data.Models.Indent objPO = this._indentService.GetForId(Id);

            if (objPO != null)
            {
                model.ItemName = objPO.ItemName;
                model.Specification = objPO.Specification;
                model.QtyInStock = objPO.QtyInStock;
                model.Priority = objPO.Priority;
                model.PlantId = objPO.PlantId;
                model.UnitPrice = objPO.UnitPrice;
                model.UnitPrice = objPO.TotalAmount;
                model.QtyNeeded = objPO.QtyNeeded;
                model.IndentNo = objPO.IndentNo;

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}