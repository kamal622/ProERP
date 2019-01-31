using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class PreventiveHoldHistoryMap : EntityTypeConfiguration<PreventiveHoldHistory>
    {
        public PreventiveHoldHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PreventiveHoldHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PreventiveId).HasColumnName("PreventiveId");
            this.Property(t => t.HoldBy).HasColumnName("HoldBy");
            this.Property(t => t.HoldOn).HasColumnName("HoldOn");
            this.Property(t => t.Reason).HasColumnName("Reason");
            this.Property(t => t.HoldDays).HasColumnName("HoldDays");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.PreventiveHoldHistories)
                .HasForeignKey(d => d.HoldBy);

        }
    }
}
