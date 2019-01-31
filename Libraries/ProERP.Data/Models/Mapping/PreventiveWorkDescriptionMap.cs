using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class PreventiveWorkDescriptionMap : EntityTypeConfiguration<PreventiveWorkDescription>
    {
        public PreventiveWorkDescriptionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1028);

            // Table & Column Mappings
            this.ToTable("PreventiveWorkDescription");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
