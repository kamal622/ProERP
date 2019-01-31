using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("UserProfile");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.IsBlocked).HasColumnName("IsBlocked");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.IsVersionUpdated).HasColumnName("IsVersionUpdated");
        }
    }
}
