using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class ReadingDataMap : EntityTypeConfiguration<ReadingData>
    {
        public ReadingDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ReadingData");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.C1).HasColumnName("C1");
            this.Property(t => t.C2).HasColumnName("C2");
            this.Property(t => t.C3).HasColumnName("C3");
            this.Property(t => t.C4).HasColumnName("C4");
            this.Property(t => t.C5).HasColumnName("C5");
            this.Property(t => t.C6).HasColumnName("C6");
            this.Property(t => t.C7).HasColumnName("C7");
            this.Property(t => t.C8).HasColumnName("C8");
            this.Property(t => t.C9).HasColumnName("C9");
            this.Property(t => t.C10).HasColumnName("C10");
            this.Property(t => t.C11).HasColumnName("C11");
            this.Property(t => t.C12).HasColumnName("C12");
            this.Property(t => t.C13).HasColumnName("C13");
            this.Property(t => t.C14).HasColumnName("C14");
            this.Property(t => t.C15).HasColumnName("C15");
            this.Property(t => t.C16).HasColumnName("C16");
            this.Property(t => t.C17).HasColumnName("C17");
            this.Property(t => t.C18).HasColumnName("C18");
            this.Property(t => t.C19).HasColumnName("C19");
            this.Property(t => t.C20).HasColumnName("C20");
            this.Property(t => t.C21).HasColumnName("C21");
            this.Property(t => t.C22).HasColumnName("C22");
            this.Property(t => t.C23).HasColumnName("C23");
            this.Property(t => t.C24).HasColumnName("C24");
            this.Property(t => t.C25).HasColumnName("C25");
            this.Property(t => t.C26).HasColumnName("C26");
            this.Property(t => t.C27).HasColumnName("C27");
            this.Property(t => t.C28).HasColumnName("C28");
            this.Property(t => t.C29).HasColumnName("C29");
            this.Property(t => t.C30).HasColumnName("C30");
            this.Property(t => t.C31).HasColumnName("C31");
            this.Property(t => t.C32).HasColumnName("C32");
            this.Property(t => t.C33).HasColumnName("C33");
            this.Property(t => t.C34).HasColumnName("C34");
            this.Property(t => t.C35).HasColumnName("C35");
            this.Property(t => t.C36).HasColumnName("C36");
            this.Property(t => t.C37).HasColumnName("C37");
            this.Property(t => t.C38).HasColumnName("C38");
            this.Property(t => t.C39).HasColumnName("C39");
            this.Property(t => t.C40).HasColumnName("C40");
            this.Property(t => t.C41).HasColumnName("C41");
            this.Property(t => t.C42).HasColumnName("C42");
            this.Property(t => t.C43).HasColumnName("C43");
            this.Property(t => t.C44).HasColumnName("C44");
            this.Property(t => t.C45).HasColumnName("C45");
            this.Property(t => t.C46).HasColumnName("C46");
            this.Property(t => t.C47).HasColumnName("C47");
            this.Property(t => t.C48).HasColumnName("C48");
            this.Property(t => t.C49).HasColumnName("C49");
            this.Property(t => t.C50).HasColumnName("C50");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedOn).HasColumnName("UpdatedOn");

            // Relationships
            this.HasRequired(t => t.Template)
                .WithMany(t => t.ReadingDatas)
                .HasForeignKey(d => d.TemplateId);

        }
    }
}
