using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public OrderStatus Status { get; private set; }

        // On cache la vraie liste pour que personne ne puisse faire un .Add() depuis l'extérieur
        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public Order()
        {
            Id = Guid.NewGuid();
            Status = OrderStatus.Cart; // Par défaut, c'est un panier
        }

        // Règle métier : Calcul dynamique du total
        public decimal CalculateTotal()
        {
            return _items.Sum(item => item.UnitPrice * item.Quantity);
        }

        public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            if (Status != OrderStatus.Cart)
                throw new DomainException("Impossible d'ajouter des articles à une commande déjà validée ou annulée.");

            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.AddQuantity(quantity);
            }
            else
            {
                _items.Add(new OrderItem(productId, productName, unitPrice, quantity));
            }
        }

        public void Validate()
        {
            if (Status != OrderStatus.Cart)
                throw new DomainException("Seul un panier en cours peut être validé.");

            if (!_items.Any())
                throw new DomainException("Impossible de valider un panier vide.");

            Status = OrderStatus.Validated;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Validated)
                throw new DomainException("Impossible d'annuler une commande déjà validée (nécessite un processus de remboursement).");

            Status = OrderStatus.Cancelled;
        }
    }
}
