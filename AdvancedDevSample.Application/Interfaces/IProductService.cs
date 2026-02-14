using AdvancedDevSample.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<Guid> CreateProductAsync(CreateProductDto dto);
        Task UpdateProductAsync(Guid id, UpdateProductDto dto);
        Task PatchProductAsync(Guid id, PatchProductDto dto);
        Task DeleteProductAsync(Guid id);
        Task ApplyPromotionAsync(Guid id, decimal percentage);
        Task ChangeStatusAsync(Guid id, bool activate);
    }
}
