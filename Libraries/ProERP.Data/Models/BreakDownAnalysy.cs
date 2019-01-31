using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class BreakDownAnalysy : BaseEntity
    {
        public Nullable<int> PlantId { get; set; }
        public Nullable<int> LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public Nullable<int> SubAssemblyId { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> BreakDownTypeId { get; set; }
        public string FailureDescription { get; set; }
        public string DocumentLocation { get; set; }
        public string DocumentNo { get; set; }
        public Nullable<int> SpareTypeId { get; set; }
        public string SpareDescription { get; set; }
        public string DoneBy { get; set; }
        public string RootCause { get; set; }
        public string Correction { get; set; }
        public string CorrectiveAction { get; set; }
        public string PreventingAction { get; set; }
        public virtual BreakDownType BreakDownType { get; set; }
        public virtual Line Line { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual SubAssembly SubAssembly { get; set; }
    }
}
