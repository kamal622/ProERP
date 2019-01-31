using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class FormulationRequestMap : EntityTypeConfiguration<FormulationRequest>
    {
        public FormulationRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.LotNo)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.GradeName)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.LOTSize)
                .HasMaxLength(500);

            this.Property(t => t.ColorSTD)
                .HasMaxLength(500);

            this.Property(t => t.WorkOrderNo)
                .HasMaxLength(100);

            this.Property(t => t.ProgressNotes)
                .HasMaxLength(200);

            this.Property(t => t.ReadyForTestNotes)
                .HasMaxLength(200);

            this.Property(t => t.TestNotes)
                .HasMaxLength(200);

            this.Property(t => t.CloseNotes)
                .HasMaxLength(200);

            this.Property(t => t.VerifyNotes)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("FormulationRequests");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LotNo).HasColumnName("LotNo");
            this.Property(t => t.GradeName).HasColumnName("GradeName");
            this.Property(t => t.QtyToProduce).HasColumnName("QtyToProduce");
            this.Property(t => t.UOM).HasColumnName("UOM");
            this.Property(t => t.LOTSize).HasColumnName("LOTSize");
            this.Property(t => t.ColorSTD).HasColumnName("ColorSTD");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.QAStatusId).HasColumnName("QAStatusId");
            this.Property(t => t.WorkOrderNo).HasColumnName("WorkOrderNo");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.ProgressOn).HasColumnName("ProgressOn");
            this.Property(t => t.ProgressBy).HasColumnName("ProgressBy");
            this.Property(t => t.ProgressNotes).HasColumnName("ProgressNotes");
            this.Property(t => t.ReadyForTestOn).HasColumnName("ReadyForTestOn");
            this.Property(t => t.ReadyForTestBy).HasColumnName("ReadyForTestBy");
            this.Property(t => t.ReadyForTestNotes).HasColumnName("ReadyForTestNotes");
            this.Property(t => t.TestOn).HasColumnName("TestOn");
            this.Property(t => t.TestBy).HasColumnName("TestBy");
            this.Property(t => t.TestNotes).HasColumnName("TestNotes");
            this.Property(t => t.CloseOn).HasColumnName("CloseOn");
            this.Property(t => t.CloseBy).HasColumnName("CloseBy");
            this.Property(t => t.CloseNotes).HasColumnName("CloseNotes");
            this.Property(t => t.VerifyOn).HasColumnName("VerifyOn");
            this.Property(t => t.VerifyBy).HasColumnName("VerifyBy");
            this.Property(t => t.VerifyNotes).HasColumnName("VerifyNotes");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateBy).HasColumnName("UpdateBy");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedBy).HasColumnName("DeletedBy");
            this.Property(t => t.VerNo).HasColumnName("VerNo");

            // Relationships
            this.HasRequired(t => t.FormulationRequestsStatu)
                .WithMany(t => t.FormulationRequests)
                .HasForeignKey(d => d.StatusId);
            this.HasRequired(t => t.Line)
                .WithMany(t => t.FormulationRequests)
                .HasForeignKey(d => d.LineId);
            this.HasRequired(t => t.ProductMaster)
                .WithMany(t => t.FormulationRequests)
                .HasForeignKey(d => d.ProductId);
            this.HasOptional(t => t.QAStatu)
                .WithMany(t => t.FormulationRequests)
                .HasForeignKey(d => d.QAStatusId);

        }
    }
}
