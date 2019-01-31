using System;
using System.ComponentModel.DataAnnotations;
using ProERP.Data.Models;

namespace ProERP.Services.Models
{
    public class MachineViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime? InstallationDate { get; set; }
        public string MachineInCharge { get; set; }

        public string Description { get; set; }
        public Data.Models.Site[] Sites { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public Data.Models.Plant[] Plants { get; set; }
        public Nullable<int> PlantId { get; set; }
        public string PlantName { get; set; }
        public Data.Models.Line[] Lines { get; set; }
        public Nullable<int> LineId { get; set; }
        public string LineName { get; set; }
        
        public MachineType[] MachineTypes { get; set; }
        public Nullable<int> MachineTypeId { get; set; }
        public string[] MachineName { get; set; }
        public bool IsActive { get; set; }

        public string ErrorMessage { get; set; }
    }
}
