using Schaeffler.Domain;
using Schaeffler.Persistence.Interface;
using Schaeffler.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Schaeffler.Web.Controllers
{
    public class JapanController : Controller
    {
        #region Constructor And Private Members

        private IFeedbackDao _feedbackDao;
        private IUtilityService _utilityService;

        public JapanController(IFeedbackDao feedbackDao, IUtilityService utilityService)
        {
            _feedbackDao = feedbackDao;
            _utilityService = utilityService;
        }

        #endregion
        // GET: Japan
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
            var url = Request.RawUrl;
            if (url == @"/")
            {
                Response.RedirectToRoute("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(feedback fb)
        {


            fb.Country = "Japan";
            fb.SystemIp = GetIp();
            Int64 Id = _feedbackDao.Savefeedback(fb);
            if (Id > 0)
            {
                feedback feedback = _feedbackDao.GetFeedback(Id);

                StringBuilder sb = new StringBuilder();
                sb.Append(RenderRazorViewToString("_feedbackform_jp", feedback));
                var result = _utilityService.SendEmail("Schaeffler - (ID) Contact Us  Enquiry  <" + DateTime.Now.ToString() + ">", sb.ToString(), "marketingschaamsea@gmail.com", true, null);

                return RedirectToRoute("index_jp-thankyou");
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

        public ActionResult Index_JP_ThankYou()
        {
            return View();
        }
        #region Private Method

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
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

        #endregion
    }
}
