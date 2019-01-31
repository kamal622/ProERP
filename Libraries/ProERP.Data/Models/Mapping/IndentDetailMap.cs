using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class IndentDetailMap : EntityTypeConfiguration<IndentDetail>
    {
        public IndentDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UnitOfMeasure)
                .HasMaxLength(70);

            this.Property(t => t.PoNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("IndentDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PreferredVendorId).HasColumnName("PreferredVendorId");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.QtyNeeded).HasColumnName("QtyNeeded");
            this.Property(t => t.JobDescription).HasColumnName("JobDescription");
            this.Property(t => t.UnitOfMeasure).HasColumnName("UnitOfMeasure");
            this.Property(t => t.Make).HasColumnName("Make");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.IsApprove).HasColumnName("IsApprove");
            this.Property(t => t.ApprovedBy).HasColumnName("ApprovedBy");
            this.Property(t => t.ApprovedOn).HasColumnName("ApprovedOn");
            this.Property(t => t.RejectedBy).HasColumnName("RejectedBy");
            this.Property(t => t.Rejectedon).HasColumnName("Rejectedon");
            this.Property(t => t.IssuedBy).HasColumnName("IssuedBy");
            this.Property(t => t.IssuedOn).HasColumnName("IssuedOn");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.IndentId).HasColumnName("IndentId");
            this.Property(t => t.EstimatePrice).HasColumnName("EstimatePrice");
            this.Property(t => t.FinalPrice).HasColumnName("FinalPrice");
            this.Property(t => t.RequiredByDate).HasColumnName("RequiredByDate");
            this.Property(t => t.PoDate).HasColumnName("PoDate");
            this.Property(t => t.DeliveryDate).HasColumnName("DeliveryDate");
            this.Property(t => t.PoNo).HasColumnName("PoNo");
            this.Property(t => t.IssuedQty).HasColumnName("IssuedQty");
            this.Property(t => t.PoAmount).HasColumnName("PoAmount");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");

            // Relationships
            this.HasOptional(t => t.CurrencyMaster)
                .WithMany(t => t.IndentDetails)
                .HasForeignKey(d => d.Currency);
            this.HasRequired(t => t.Indent)
                .WithMany(t => t.IndentDetails)
                .HasForeignKey(d => d.IndentId);
            this.HasRequired(t => t.IndentStatu)
                .WithMany(t => t.IndentDetails)
                .HasForeignKey(d => d.StatusId);
            this.HasOptional(t => t.Item)
                .WithMany(t => t.IndentDetails)
                .HasForeignKey(d => d.ItemId);
            this.HasOptional(t => t.Line)
                .WithMany(t => t.IndentDetails)
                .HasForeignKey(d => d.LineId);
            this.HasOptional(t => t.Machine)
                .WithMany(t => t.IndentDetails)
                .HasForeignKey(d => d.MachineId);
            this.HasOptional(t => t.Plant)
                .WithMany(t => t.IndentDetails)
                .HasForeignKey(d => d.PlantId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.IndentDetails)
                .HasForeignKey(d => d.ApprovedBy);
            this.HasOptional(t => t.User1)
                .WithMany(t => t.IndentDetails1)
                .HasForeignKey(d => d.IssuedBy);
            this.HasOptional(t => t.User2)
                .WithMany(t => t.IndentDetails2)
                .HasForeignKey(d => d.RejectedBy);
            this.HasRequired(t => t.Vendor)
                .WithMany(t => t.IndentDetails)
                .HasForeignKey(d => d.PreferredVendorId);

        }
    }
}
