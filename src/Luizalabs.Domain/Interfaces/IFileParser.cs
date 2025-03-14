using Luizalabs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luizalabs.Domain.Interfaces
{
    public interface IFileParser
    {
        IReadOnlyCollection<User> Parse(Stream fileStream);
    }
}
