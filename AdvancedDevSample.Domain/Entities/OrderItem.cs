using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Entities
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; } // Le prix figé au moment de l'ajout
        public int Quantity { get; private set; }

        public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("La quantité doit être supérieure à zéro.");

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        // Permet d'augmenter la quantité si on ajoute le même produit plusieurs fois
        public void AddQuantity(int amount)
        {
            if (amount <= 0) throw new DomainException("La quantité à ajouter doit être positive.");
            Quantity += amount;
        }
    }
}
