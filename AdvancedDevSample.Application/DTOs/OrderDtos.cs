using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.DTOs
{
    public record OrderItemDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal TotalPrice);

    public record OrderDto(Guid Id, string Status, decimal TotalOrderPrice, IEnumerable<OrderItemDto> Items);

    public record AddItemToOrderDto(Guid ProductId, int Quantity);
}
