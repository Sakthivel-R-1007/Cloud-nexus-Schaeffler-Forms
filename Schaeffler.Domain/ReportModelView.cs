using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Domain
{
    public class ReportModelView
    {
        public List<feedback> Feedback { get; set; }

        public List<BrandService> BrandService { get; set; }

        public List<VehicleType> VehicleType { get; set; }

        public List<InformationType> InformationType { get; set; }
    }
}
