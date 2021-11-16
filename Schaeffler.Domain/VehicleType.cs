using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Domain
{
    public class VehicleType
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Id_Name { get; set; }
        public string TH_Name { get; set; }
        public string JP_Name { get; set; }

        public string PassengerCar { get; set; }
        public string LightCommercialVehicle { get; set; }
        public string HeavyCommercialVehicle { get; set; }
        public string Tractor { get; set; }
    }
}
