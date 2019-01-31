using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.Models
{
    public class BreakdownViewModel
    {
        public int SiteId { get; set; }
        public int PlantId { get; set; }

        public List<Data.Models.Site> SiteList { get; set; }
        public Data.Models.Plant[] PlantList { get; set; }
        public List<LineModel> LineList { get; set; }
    }

    public class SiteModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class PlantModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class LineModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }    
}