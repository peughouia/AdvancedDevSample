using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Exceptions
{
    public class ApplicationServiceExceptions : Exception
    {
        public ApplicationServiceExceptions(string message) : base(message) { }
    }

    public class NotFoundException : ApplicationServiceExceptions
    {
        public NotFoundException(string entityName, Guid id)
            : base($"L'entité {entityName} avec l'ID {id} est introuvable.") { }
    }
}
