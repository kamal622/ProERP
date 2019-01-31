using ProERP.Data.Models;
using ProERP.Services.Line;
using ProERP.Services.Machine;
using ProERP.Services.Part;
using ProERP.Services.Plant;
using ProERP.Services.Site;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

namespace ProERP.Web.Controllers
{
    [Authorize(Roles = "Admin,Lavel1,Lavel2")]
    public class PartController : BaseController
    {
        // GET: Part
       
        private readonly PartService _partService;
        private readonly PlantService _plantService;
        private readonly MachineService _machineService;
        private readonly LineService _lineService;
        public PartController ( PartService partService, PlantService plantService, MachineService machineService, LineService lineService)
        {
            //  this._lineService = lineService;
           
            this._partService = partService;
            this._plantService = plantService;
            this._machineService = machineService;
            this._lineService = lineService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }


        public JsonResult GetPartList(string Name)
        {
            var allData = this._partService.GetAll(Name);
            var part = allData.Select(s => new
            {
                s.Id,
                s.Name,
                //s.MachineId,
                //s.PlantId,
                //s.LineId,
                //LineName = s.Line.Name,
                //MachineName = s.Machine.Name,
                //PlantName = s.Plant.Name,
                
            });
            return Json(part, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add()
        {
            PartViewModel model = new PartViewModel();
            model.Id = 0;
            return View(model);
        }
        public ActionResult Update(int? Id)
        {
            PartViewModel model = new PartViewModel();

            model.Id = Id ?? 0;

            return View(model);

        }

        public JsonResult SavePart(ProERP.Data.Models.Part part)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
               Data.Models.Part partobj = new Data.Models.Part();

                if (part.Id == 0)
                {
                    this._partService.Add(part);
                }
                else
                {
                    this._partService.Update(part);
                }
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in adding Parts.Please contact Admin." };
            }
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPartById(int Id)
        {
            PartViewModel model = new PartViewModel();

            Data.Models.Part partobj = this._partService.GetForId(Id);

            if (partobj != null)
            {
                //model.SiteId = 1;
                //model.MachineId = partobj.MachineId;
                //model.PlantId = partobj.PlantId;
                //model.LineId = partobj.LineId;
                model.Name = partobj.Name;
                model.Description = partobj.Description;


            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePart(int[] Ids)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                int[] ids = this._partService.Delete(Ids);
                if (ids.Length > 0)
                    result = new { Success = "false", Message = "Some dependency found in lines." };

            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in deleting Part(s).Please contact Admin." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}