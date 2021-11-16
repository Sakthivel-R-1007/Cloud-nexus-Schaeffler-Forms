using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Domain
{
    public class InformationType
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Id_Name { get; set; }
        public string TH_Name { get; set; }
        public string JP_Name { get; set; }

        public string InformationName { get; set; }

        public string ProductInformation { get; set; }
        public string Msg1 { get; set; }
        public string TechnicalInformation { get; set; }
        public string Msg2 { get; set; }
        public string NewProduct { get; set; }
        public string Msg3 { get; set; }
        public string Others { get; set; }
        public string Msg4 { get; set; }


    }
}
