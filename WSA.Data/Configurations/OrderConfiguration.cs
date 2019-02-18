using WSAManager.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace WSAManager.Data.Configurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            HasMany(q => q.OrderItems).WithRequired(q => q.Order);
        }

    }
}
