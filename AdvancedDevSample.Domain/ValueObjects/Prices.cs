using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.ValueObjects
{
    public record Prices
    {
        public decimal Amount { get; }

        public Prices(decimal amount)
        {
            if (amount <= 0)
                throw new DomainException("Le prix doit être strictement positif.");
            Amount = amount;
        }

        public static Prices operator *(Prices prices, decimal multiplier) => new Prices(prices.Amount * multiplier);
        public static Prices operator -(Prices prices, Prices discount) => new Prices(prices.Amount - discount.Amount);
    }
}
