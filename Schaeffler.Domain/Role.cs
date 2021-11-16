using Schaeffler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Domain
{
    public class Role : Entity<Int64>
    {
        public string Name { get; set; }
    }
}
