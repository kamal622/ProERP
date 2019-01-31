using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Email)
                .HasMaxLength(256);

            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.EmailConfirmed).HasColumnName("EmailConfirmed");
            this.Property(t => t.PasswordHash).HasColumnName("PasswordHash");
            this.Property(t => t.SecurityStamp).HasColumnName("SecurityStamp");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
            this.Property(t => t.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
            this.Property(t => t.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
            this.Property(t => t.LockoutEnabled).HasColumnName("LockoutEnabled");
            this.Property(t => t.AccessFailedCount).HasColumnName("AccessFailedCount");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.UserProfile_Id).HasColumnName("UserProfile_Id");

            // Relationships
            this.HasOptional(t => t.UserProfile)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.UserProfile_Id);

        }
    }
}
