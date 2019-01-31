using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class BreakDown : BaseEntity
    {
        public BreakDown()
        {
            this.BreakDownAttachments = new List<BreakDownAttachment>();
            this.BreakDownMenPowers = new List<BreakDownMenPower>();
            this.BreakDownServices = new List<BreakDownService>();
            this.BreakDownSpares = new List<BreakDownSpare>();
        }

        public int PlantId { get; set; }
        public int LineId { get; set; }
        public int MachineId { get; set; }
        public Nullable<int> SubAssemblyId { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public int TotalTime { get; set; }
        public string FailureDescription { get; set; }
        public bool ElectricalTime { get; set; }
        public bool MechTime { get; set; }
        public bool InstrTime { get; set; }
        public bool UtilityTime { get; set; }
        public bool PowerTime { get; set; }
        public bool ProcessTime { get; set; }
        public bool PrvTime { get; set; }
        public bool IdleTime { get; set; }
        public Nullable<System.TimeSpan> ResolveTimeTaken { get; set; }
        public Nullable<int> SpareTypeId { get; set; }
        public string SpareDescription { get; set; }
        public string DoneBy { get; set; }
        public string RootCause { get; set; }
        public string Correction { get; set; }
        public string CorrectiveAction { get; set; }
        public string PreventingAction { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsHistory { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> UploadId { get; set; }
        public bool IsRepeated { get; set; }
        public bool IsMajor { get; set; }
        public virtual BreakDownUploadHistory BreakDownUploadHistory { get; set; }
        public virtual Line Line { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual Machine Machine1 { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual ICollection<BreakDownAttachment> BreakDownAttachments { get; set; }
        public virtual ICollection<BreakDownMenPower> BreakDownMenPowers { get; set; }
        public virtual ICollection<BreakDownService> BreakDownServices { get; set; }
        public virtual ICollection<BreakDownSpare> BreakDownSpares { get; set; }
    }
}
