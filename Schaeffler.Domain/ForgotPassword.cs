using Schaeffler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Domain 
{
    public class ForgotPassword : Entity<Int64>
    {
        public Guid UniqueId { get; set; }
        public User user { get; set; }
        public bool IsChanged { get; set; }
        public string Key { get; set; }
    }
}
