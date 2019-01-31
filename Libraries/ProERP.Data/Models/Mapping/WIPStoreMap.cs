using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class WIPStoreMap : EntityTypeConfiguration<WIPStore>
    {
        public WIPStoreMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Location)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("WIPStore");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.CreateOn).HasColumnName("CreateOn");
            this.Property(t => t.UpdateBy).HasColumnName("UpdateBy");
            this.Property(t => t.UpdateOn).HasColumnName("UpdateOn");
        }
    }
}
