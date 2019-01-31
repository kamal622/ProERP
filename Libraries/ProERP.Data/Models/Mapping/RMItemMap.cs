using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class RMItemMap : EntityTypeConfiguration<RMItem>
    {
        public RMItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ItemCode)
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("RMItems");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.ItemCode).HasColumnName("ItemCode");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateBy).HasColumnName("UpdateBy");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedBy).HasColumnName("DeletedBy");

            // Relationships
            this.HasRequired(t => t.ItemCategory)
                .WithMany(t => t.RMItems)
                .HasForeignKey(d => d.CategoryId);

        }
    }
}
