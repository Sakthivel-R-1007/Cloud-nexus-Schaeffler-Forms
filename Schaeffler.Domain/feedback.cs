using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Domain
{
    public class feedback
    {

        public Int64 Id { get; set; }
        public string Country { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string CompanyName { get; set; }

        public string PhoneNumber { get; set; }
        public string Message { get; set; }

        public string SystemIp { get; set; }

       

        public List<BrandService> BrandService { get; set; }

        public List<VehicleType> VehicleType { get; set; }

        public List<InformationType> InformationType { get; set; }

        public string CreatedOn { get; set; }
    }
}
