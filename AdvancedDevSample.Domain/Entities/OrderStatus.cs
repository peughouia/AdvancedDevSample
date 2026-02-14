using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Entities
{
    public enum OrderStatus
    {
        Cart = 0,       // Panier en cours
        Validated = 1,  // Commande validée
        Cancelled = 2   // Commande annulée
    }
}
