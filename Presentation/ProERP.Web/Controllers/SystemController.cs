using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProERP.Web.Models;
using ProERP.Services.Models;
using ProERP.Services.Plant;

namespace ProERP.Web.Controllers
{
    public class SystemController : Controller
    {
        private readonly PlantService _plantService;

        public SystemController(PlantService plantService)
        {
            this._plantService = plantService;
        }

        // GET: System
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Backup()
        {
            BackupViewModel model = new BackupViewModel();
            model.YearData = new List<int>();
            model.MonthData = new List<Services.Models.DropDownData>();
            model.PlantData = this._plantService.GetAllPlants();
            model.PlantData.Insert(0, new Data.Models.Plant { Id = 0, Name = "--ALL--" });
            for (int i = 0; i < 15; i++)
                model.YearData.Add(DateTime.Now.Year - i);

            model.MonthData.Add(new Services.Models.DropDownData { Id = 0, Name = "-- ALL --" });
            for (int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                model.MonthData.Add(new Services.Models.DropDownData { Id = i, Name = dt.ToString("MMM") });
            }
            model.PlantId = 0;
            model.Year = DateTime.Now.Year;
            model.Month = DateTime.Now.Month;
            return View(model);
        }
    }
}