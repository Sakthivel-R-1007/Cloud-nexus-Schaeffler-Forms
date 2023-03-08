using Schaeffler.Domain;
using Schaeffler.Persistence.Interface;
using Schaeffler.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Schaeffler.Web.Controllers
{
    public class AustraliaController : Controller
    {
        #region Constructor And Private Members

        private IFeedbackDao _feedbackDao;
        private IUtilityService _utilityService;

        public AustraliaController(IFeedbackDao feedbackDao, IUtilityService utilityService)
        {
            _feedbackDao = feedbackDao;
            _utilityService = utilityService;
        }

        #endregion
        public ActionResult Index()
        {
            IList<BrandService> brandServices = _feedbackDao.GetBrandServices();
            IList<VehicleType> vehicleTypes = _feedbackDao.GetVehicleTypes();
            IList<InformationType> informationTypes = _feedbackDao.GetInformationTypes();
            if (brandServices != null && brandServices.Count > 0)
            {
                ViewBag.BrandServices = brandServices;
            }
            if (vehicleTypes != null && vehicleTypes.Count > 0)
            {
                ViewBag.VehicleTypes = vehicleTypes;
            }
            if (informationTypes != null && informationTypes.Count > 0)
            {
                ViewBag.InformationTypes = informationTypes;
            }



            return View();
        }

        [HttpPost]
        public ActionResult Index(feedback fb)
        {

            fb.Country = "Australia";

            fb.SystemIp = GetIp();
            Int64 Id = _feedbackDao.Savefeedback(fb);
            if (Id > 0)
            {
                feedback feedback = _feedbackDao.GetFeedback(Id);

                StringBuilder sb = new StringBuilder();
                sb.Append(RenderRazorViewToString("_feedbackform_au", feedback));
                var result = _utilityService.SendEmail("Schaeffler - (AU) Contact Us  Enquiry <" + DateTime.Now.ToString() + " > ", sb.ToString(),null, true, null,null, "Australia");
                return RedirectToRoute("index_au-thankyou");
            }
            IList<BrandService> brandServices = _feedbackDao.GetBrandServices();
            IList<VehicleType> vehicleTypes = _feedbackDao.GetVehicleTypes();
            IList<InformationType> informationTypes = _feedbackDao.GetInformationTypes();
            if (brandServices != null && brandServices.Count > 0)
            {
                ViewBag.BrandServices = brandServices;
            }
            if (vehicleTypes != null && vehicleTypes.Count > 0)
            {
                ViewBag.VehicleTypes = vehicleTypes;
            }
            if (informationTypes != null && informationTypes.Count > 0)
            {
                ViewBag.InformationTypes = informationTypes;
            }
            return View();
        }

        public ActionResult Index_AU_ThankYou()
        {
            return View();
        }
        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                        viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
    }
}