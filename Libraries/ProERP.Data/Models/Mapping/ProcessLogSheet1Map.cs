using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class ProcessLogSheet1Map : EntityTypeConfiguration<ProcessLogSheet1>
    {
        public ProcessLogSheet1Map()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProcessLogSheet1");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.BatchId).HasColumnName("BatchId");
            this.Property(t => t.GradeId).HasColumnName("GradeId");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Time).HasColumnName("Time");
            this.Property(t => t.TZ1).HasColumnName("TZ1");
            this.Property(t => t.TZ2).HasColumnName("TZ2");
            this.Property(t => t.TZ3).HasColumnName("TZ3");
            this.Property(t => t.TZ4).HasColumnName("TZ4");
            this.Property(t => t.TZ5).HasColumnName("TZ5");
            this.Property(t => t.TZ6).HasColumnName("TZ6");
            this.Property(t => t.TZ7).HasColumnName("TZ7");
            this.Property(t => t.TZ8).HasColumnName("TZ8");
            this.Property(t => t.TZ9).HasColumnName("TZ9");
            this.Property(t => t.TZ10).HasColumnName("TZ10");
            this.Property(t => t.TZ11).HasColumnName("TZ11");
            this.Property(t => t.TZ12Die).HasColumnName("TZ12Die");
            this.Property(t => t.TM1).HasColumnName("TM1");
            this.Property(t => t.PM1).HasColumnName("PM1");
            this.Property(t => t.PM11).HasColumnName("PM11");
            this.Property(t => t.Vaccumembar).HasColumnName("Vaccumembar");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateBy).HasColumnName("UpdateBy");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.ProcessLogSheet1)
                .HasForeignKey(d => d.BatchId);
            this.HasRequired(t => t.Line)
                .WithMany(t => t.ProcessLogSheet1)
                .HasForeignKey(d => d.LineId);
            this.HasRequired(t => t.ProductMaster)
                .WithMany(t => t.ProcessLogSheet1)
                .HasForeignKey(d => d.GradeId);

        }
    }
}
