using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductDto(p.Id, p.Name, p.Price.Amount, p.IsActive, p.StockQuantity));
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product", id);

            return new ProductDto(product.Id, product.Name, product.Price.Amount, product.IsActive, product.StockQuantity);
        }

        public async Task<Guid> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product(dto.Name, dto.Price, dto.StockQuantity);
            await _repository.AddAsync(product);
            return product.Id;
        }

        public async Task UpdateProductAsync(Guid id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product", id);

            product.UpdateDetails(dto.Name, dto.Price, dto.StockQuantity);
            await _repository.UpdateAsync(product);
        }

        public async Task PatchProductAsync(Guid id, PatchProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product", id);

            product.UpdatePartial(dto.Name, dto.Price, dto.StockQuantity);
            await _repository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product", id);

            await _repository.DeleteAsync(product);
        }

        public async Task ApplyPromotionAsync(Guid id, decimal percentage)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product", id);

            product.ApplyPromotion(percentage);
            await _repository.UpdateAsync(product);
        }

        public async Task ChangeStatusAsync(Guid id, bool activate)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product", id);

            if (activate) product.Activate();
            else product.Deactivate();

            await _repository.UpdateAsync(product);
        }
    }
}
