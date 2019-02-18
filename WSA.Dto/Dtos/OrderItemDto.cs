using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSAManager.Dto.Dtos
{
    public class OrderItemDto : BaseDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [DefaultValue(1)]
        public int Quantity { get; set; }
    }
}
