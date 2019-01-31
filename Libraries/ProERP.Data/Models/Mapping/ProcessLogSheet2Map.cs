using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class ProcessLogSheet2Map : EntityTypeConfiguration<ProcessLogSheet2>
    {
        public ProcessLogSheet2Map()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Remarks)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("ProcessLogSheet2");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.BatchId).HasColumnName("BatchId");
            this.Property(t => t.GradeId).HasColumnName("GradeId");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Time).HasColumnName("Time");
            this.Property(t => t.RPM).HasColumnName("RPM");
            this.Property(t => t.TORQ).HasColumnName("TORQ");
            this.Property(t => t.AMPS).HasColumnName("AMPS");
            this.Property(t => t.RPM1).HasColumnName("RPM1");
            this.Property(t => t.RPM2).HasColumnName("RPM2");
            this.Property(t => t.RPM3).HasColumnName("RPM3");
            this.Property(t => t.F1KGHR).HasColumnName("F1KGHR");
            this.Property(t => t.F1Perc).HasColumnName("F1Perc");
            this.Property(t => t.F2KGHR).HasColumnName("F2KGHR");
            this.Property(t => t.F2Perc).HasColumnName("F2Perc");
            this.Property(t => t.F3KGHR).HasColumnName("F3KGHR");
            this.Property(t => t.F3Perc).HasColumnName("F3Perc");
            this.Property(t => t.F4KGHR).HasColumnName("F4KGHR");
            this.Property(t => t.F4Perc).HasColumnName("F4Perc");
            this.Property(t => t.F5KGHR).HasColumnName("F5KGHR");
            this.Property(t => t.F5Perc).HasColumnName("F5Perc");
            this.Property(t => t.F6KGHR).HasColumnName("F6KGHR");
            this.Property(t => t.F6Perc).HasColumnName("F6Perc");
            this.Property(t => t.Output).HasColumnName("Output");
            this.Property(t => t.NoofDiesHoles).HasColumnName("NoofDiesHoles");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateBy).HasColumnName("UpdateBy");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.ProcessLogSheet2)
                .HasForeignKey(d => d.BatchId);
            this.HasRequired(t => t.Line)
                .WithMany(t => t.ProcessLogSheet2)
                .HasForeignKey(d => d.LineId);
            this.HasRequired(t => t.ProductMaster)
                .WithMany(t => t.ProcessLogSheet2)
                .HasForeignKey(d => d.GradeId);

        }
    }
}
