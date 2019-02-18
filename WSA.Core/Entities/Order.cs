using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSAManager.Core.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            DateBegin = DateTime.UtcNow;
        }

        public string Comment { get; set; }

        [Required]
        public DateTime DateBegin { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [DefaultValue(1)]
        public int StatusId { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
