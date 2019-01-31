using ProERP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    class SystemViewModel
    {
    }

    public class BackupViewModel
    {
        public int PlantId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<Data.Models.Plant> PlantData { get; set; }
        public List<int> YearData { get; set; }
        public List<DropDownData> MonthData { get; set; }
        
    }
    public class DropDownData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
