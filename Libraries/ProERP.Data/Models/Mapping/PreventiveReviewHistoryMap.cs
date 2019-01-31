using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class PreventiveReviewHistoryMap : EntityTypeConfiguration<PreventiveReviewHistory>
    {
        public PreventiveReviewHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Notes)
                .HasMaxLength(2056);

            // Table & Column Mappings
            this.ToTable("PreventiveReviewHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PreventiveId).HasColumnName("PreventiveId");
            this.Property(t => t.ReviewDate).HasColumnName("ReviewDate");
            this.Property(t => t.ReviewBy).HasColumnName("ReviewBy");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.ScheduledReviewDate).HasColumnName("ScheduledReviewDate");
            this.Property(t => t.HoldId).HasColumnName("HoldId");
            this.Property(t => t.ShutdownId).HasColumnName("ShutdownId");
            this.Property(t => t.IsLaps).HasColumnName("IsLaps");
            this.Property(t => t.IsOverdue).HasColumnName("IsOverdue");
            this.Property(t => t.IsLineActive).HasColumnName("IsLineActive");
            this.Property(t => t.IsMachineActive).HasColumnName("IsMachineActive");
            this.Property(t => t.VerifyDate).HasColumnName("VerifyDate");
            this.Property(t => t.VerifyBy).HasColumnName("VerifyBy");

            // Relationships
            this.HasOptional(t => t.PreventiveHoldHistory)
                .WithMany(t => t.PreventiveReviewHistories)
                .HasForeignKey(d => d.HoldId);
            this.HasRequired(t => t.PreventiveMaintenance)
                .WithMany(t => t.PreventiveReviewHistories)
                .HasForeignKey(d => d.PreventiveId);
            this.HasOptional(t => t.ShutdownHistory)
                .WithMany(t => t.PreventiveReviewHistories)
                .HasForeignKey(d => d.ShutdownId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.PreventiveReviewHistories)
                .HasForeignKey(d => d.ReviewBy);

        }
    }
}
