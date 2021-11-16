using Schaeffler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Persistence.Interface
{
    public interface IFeedbackDao
    {

        Int64 Savefeedback(feedback AI);

        feedback GetFeedback(Int64 Id);
        IList<BrandService> GetBrandServices();
        IList<VehicleType> GetVehicleTypes();
        IList<InformationType> GetInformationTypes();

        ReportModelView GetFeedbackReport();

        ReportModelView GetFeedbackReportNew();

    }
}
