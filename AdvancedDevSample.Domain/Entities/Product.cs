using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public Prices Price { get; private set; }
        public bool IsActive { get; private set; }
        public int StockQuantity { get; private set; }


        // Constructeur protégé pour les ORM (comme Entity Framework)
        protected Product() { }

        public Product(string name, decimal priceAmount, int stockQuantity)
        {
            Id = Guid.NewGuid();
            IsActive = true;
            UpdateDetails(name, priceAmount, stockQuantity);
        }

        // Méthode métier pour modifier (Encapsulation)
        public void UpdateDetails(string name, decimal priceAmount, int stockQuantity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Le nom du produit ne peut pas être vide.");

            if (stockQuantity < 0)
                throw new DomainException("Le stock ne peut pas être négatif.");

            Name = name;
            Price = new Prices(priceAmount); // Le Value Object valide que le prix est > 0
            StockQuantity = stockQuantity;
        }

        // Mise à jour partielle (Patch)
        public void UpdatePartial(string? name, decimal? priceAmount, int? stockQuantity)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (priceAmount.HasValue)
                Price = new Prices(priceAmount.Value);

            if (stockQuantity.HasValue)
            {
                if (stockQuantity.Value < 0) throw new DomainException("Le stock ne peut pas être négatif.");
                StockQuantity = stockQuantity.Value;
            }
        }

        // Activer/Désactiver
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        // Logique de promotion
        public void ApplyPromotion(decimal percentage)
        {
            if (percentage <= 0 || percentage > 50)
                throw new DomainException("La promotion doit être entre 1% et 50%.");

            if (!IsActive)
                throw new DomainException("Impossible d'appliquer une promotion sur un produit inactif.");

            Price = Price - (Price * (percentage / 100));
        }
    }
}
