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
  
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        // Injection des deux repositories !
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) throw new NotFoundException("Order", id);

            return MapToDto(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToDto);
        }

        public async Task<Guid> CreateCartAsync()
        {
            var order = new Order();
            await _orderRepository.AddAsync(order);
            return order.Id;
        }

        public async Task AddItemToCartAsync(Guid orderId, AddItemToOrderDto dto)
        {
            // 1. Récupérer la commande
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException("Order", orderId);

            // 2. Récupérer le produit pour avoir son vrai prix et vérifier s'il est actif
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null) throw new NotFoundException("Product", dto.ProductId);

            if (!product.IsActive)
                throw new ApplicationServiceExceptions($"Le produit {product.Name} n'est plus disponible à la vente.");

            if (product.StockQuantity < dto.Quantity)
                throw new ApplicationServiceExceptions($"Stock insuffisant pour {product.Name}. Stock actuel : {product.StockQuantity}");

            // 3. Ajouter l'article via le Domaine
            order.AddItem(product.Id, product.Name, product.Price.Amount, dto.Quantity);

            // 4. Sauvegarder
            await _orderRepository.UpdateAsync(order);
        }

        public async Task ValidateOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) throw new NotFoundException("Order", id);

            order.Validate();
            await _orderRepository.UpdateAsync(order);

            // NOTE : Dans un vrai système, c'est ici qu'on diminuerait le stock 
            // dans le ProductRepository de chaque produit vendu. (Je le garde simple pour l'instant).
        }

        public async Task CancelOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) throw new NotFoundException("Order", id);

            order.Cancel();
            await _orderRepository.UpdateAsync(order);
        }

        // Méthode utilitaire pour transformer l'Entité en DTO proprement
        private static OrderDto MapToDto(Order order)
        {
            var itemDtos = order.Items.Select(i =>
                new OrderItemDto(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.UnitPrice * i.Quantity)
            );

            return new OrderDto(order.Id, order.Status.ToString(), order.CalculateTotal(), itemDtos);
        }
    }
}
