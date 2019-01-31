using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Order");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PoNo).HasColumnName("PoNo");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.OrderQty).HasColumnName("OrderQty");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.TotalPrice).HasColumnName("TotalPrice");
            this.Property(t => t.DeliveryDate).HasColumnName("DeliveryDate");
        }
    }
}
