using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class FormulationCreateModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public string QtyToProduce { get; set; }
        public string LOTSize { get; set; }
        public string LotNo { get; set; }
        public List<FormulationMasterViewModel> formulation { get; set; }
    }
    public class FormulationMasterViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int? MachineId { get; set; }
        public string MachineName { get; set; }
        public decimal PreviousBaseValue { get; set; }
        public decimal BaseValue { get; set; }
        public decimal InGrams { get; set; }
        public int StatusId { get; set; }
    }
}
