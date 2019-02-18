using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSAManager.Dto.Utilities;

namespace WSAManager.Dto.Dtos
{
    public class OrderDto : BaseDto
    {
        public OrderDto()
        { }

        public string Comment { get; set; }

        [Required]
        public string DateBegin { get; set; }

        [Required]
        public int ClientId { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [DefaultValue(1)]
        public int StatusId { get; set; }
    }

    public class GetOrderDto : OrderDto
    {
        public GetOrderDto()
        {
            OrderItems = new HashSet<OrderItemDto>();
        }

        public string Status { get { return EnumUtility.GetEnumValue<OrderStatuses>(this.StatusId).ToString(); } }

        public virtual ICollection<OrderItemDto> OrderItems { get; set; }
    }

    public enum OrderStatuses
    {
        New = 1,
        Approved = 2,
        Rejected = 3
    }
}
