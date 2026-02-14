using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.DTOs
{
    public record ProductDto(Guid Id, string Name, decimal Price, bool IsActive, int StockQuantity);
    public record CreateProductDto(string Name, decimal Price, int StockQuantity);
    public record UpdateProductDto(string Name, decimal Price, int StockQuantity);
    public record PatchProductDto(string? Name, decimal? Price, int? StockQuantity);
}
