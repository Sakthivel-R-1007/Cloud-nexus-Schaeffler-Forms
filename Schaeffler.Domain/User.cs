using Schaeffler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Domain
{
    public class User : Entity<Int64>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }

        public string Password { get; set; }
        public Role role { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public string SecurityCode { get; set; }

        public string Captcha { get; set; }

        public bool LastLoginStatus { get; set; }
        public Guid SessionId { get; set; }

        public string Country { get; set; }
    }
}
