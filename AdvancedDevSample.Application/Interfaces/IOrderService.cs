using AdvancedDevSample.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<Guid> CreateCartAsync();
        Task AddItemToCartAsync(Guid orderId, AddItemToOrderDto dto);
        Task ValidateOrderAsync(Guid id);
        Task CancelOrderAsync(Guid id);
    }
}
