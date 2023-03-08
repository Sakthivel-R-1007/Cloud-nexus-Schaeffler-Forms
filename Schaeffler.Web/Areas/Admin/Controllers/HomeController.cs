using ClosedXML.Excel;
using Schaeffler.Domain;
using Schaeffler.Filters;
using Schaeffler.Helpers;
using Schaeffler.Persistence.Interface;
using Schaeffler.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Schaeffler.Web.Areas.Admin.Controllers
{
    [AuthenticationFilter]
    [HandleError]
    public class HomeController : Controller
    {
        #region Constructor And Private Members

        private IUserAccountDao _userAccountDao;
        private IFeedbackDao _feedbackDao;
        private IUtilityService _utilityService;

        public HomeController(IFeedbackDao feedbackDao, IUtilityService utilityService, IUserAccountDao userAccountDao)
        {
            _userAccountDao = userAccountDao;
            _feedbackDao = feedbackDao;
            _utilityService = utilityService;
        }

        #endregion
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public void Export()
        {
            User U = new User();
            if (Session["UserAccount"] != null)
            {
                U = ((User)Session["UserAccount"]);
               
            }
            DataTable list = new DataTable();

            string ReportName = "Schaeffler";
            var workbook = new XLWorkbook();
            string range = "A1:B2";


            ReportModelView report = _feedbackDao.GetFeedbackReportNew(U.Country);
            if (report != null)
            {

                if (report != null && report.Feedback != null && report.Feedback.Count > 0 && report.BrandService != null && report.BrandService.Count > 0 && report.VehicleType != null && report.VehicleType.Count > 0 && report.InformationType != null && report.InformationType.Count > 0)
                {
                    List<feedback> fb = report.Feedback;
                    List<BrandService> BS = report.BrandService;
                    List<VehicleType> VT = report.VehicleType;
                    List<InformationType> TI = report.InformationType;

                    if (fb != null && fb.Count > 0)
                    {
                        DataTable list1 = new DataTable("User Information");
                        list1 = FeedbackDataTable(fb);
                        var worksheet = workbook.AddWorksheet(list1, "User Information");
                        worksheet.Row(1).InsertRowsAbove(1);
                        worksheet.Range(range).Row(1);
                        worksheet.Range(range).Row(1).Merge();
                        worksheet.Columns().AdjustToContents();
                        worksheet.Tables.FirstOrDefault().SetShowAutoFilter(false);
                        //worksheet.Range(range).Style.Fill.BackgroundColor = XLColor.BrightGreen;
                    }

                    if (BS != null && BS.Count > 0)
                    {
                        DataTable list1 = new DataTable("Brand Service");
                        list1 = BrandServiceDataTable(BS, U.Country);
                        DataRow dr1 = list.NewRow();
                        var worksheet = workbook.AddWorksheet(list1, "Brand Service");
                        worksheet.Row(1).InsertRowsAbove(1);
                        worksheet.Range(range).Row(1);
                        worksheet.Range(range).Row(1).Merge();
                        worksheet.Columns().AdjustToContents();
                        worksheet.Tables.FirstOrDefault().SetShowAutoFilter(false);
                        // worksheet.Range(range).Style.Fill.BackgroundColor = XLColor.Green;
                    }

                    if (VT != null && VT.Count > 0)
                    {
                        DataTable list1 = new DataTable("Vehicle Type");
                        list1 = VehicleTypeDataTable(VT);
                        DataRow dr1 = list.NewRow();
                        var worksheet = workbook.AddWorksheet(list1, "Vehicle Type");
                        worksheet.Row(1).InsertRowsAbove(1);
                        worksheet.Range(range).Row(1);
                        worksheet.Range(range).Row(1).Merge();
                        worksheet.Columns().AdjustToContents();
                        worksheet.Tables.FirstOrDefault().SetShowAutoFilter(false);
                        //worksheet.Range(range).Style.Fill.BackgroundColor = XLColor.LightGreen;
                    }

                    if (TI != null && TI.Count > 0)
                    {
                        DataTable list1 = new DataTable("Type of Information");
                        list1 = InformationTypeDataTable(TI);
                        DataRow dr1 = list.NewRow();
                        var worksheet = workbook.AddWorksheet(list1, "Type of Information");
                        worksheet.Row(1).InsertRowsAbove(1);
                        worksheet.Range(range).Row(1);
                        worksheet.Range(range).Row(1).Merge();
                        worksheet.Columns().AdjustToContents();
                        worksheet.Tables.FirstOrDefault().SetShowAutoFilter(false);
                        // worksheet.Range(range).Style.Fill.BackgroundColor = XLColor.DarkGreen;
                    }



                }

            }
            else
            {
                DataTable list1 = new DataTable("report Empty");
                var worksheet = workbook.AddWorksheet(list1, "No Results Found");
                worksheet.Row(1).InsertRowsAbove(1);
                worksheet.Range(range).Row(1);
                worksheet.Range(range).Row(1).Merge();
            }


            Response.AddHeader("content-disposition", "attachment; filename=" + ReportName + "_" + DateTime.Now.ToString() + "Reports.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            using (var ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
                ms.Close();
            }
            Response.Flush();
            Response.End();


        }
        private DataTable FeedbackDataTable<T>(List<T> items)
        {
            DataTable dataTable = ToDataTable<T>(items);
            dataTable.Columns.Remove("SystemIp");
            dataTable.Columns.Remove("BrandService");
            dataTable.Columns.Remove("VehicleType");
            dataTable.Columns.Remove("InformationType");


            dataTable.Columns["Id"].ColumnName = "No.";
            dataTable.Columns["Country"].ColumnName = "Country";
            dataTable.Columns["FullName"].ColumnName = "Full Name";
            dataTable.Columns["Email"].ColumnName = "Email Address";
            dataTable.Columns["CompanyName"].ColumnName = "Company Name";
            dataTable.Columns["PhoneNumber"].ColumnName = "Phone Number";
            dataTable.Columns["Message"].ColumnName = "Any other message";
            dataTable.Columns["CreatedOn"].ColumnName = "Date and Time";
            dataTable.AcceptChanges();
            return dataTable;
        }

        private DataTable BrandServiceDataTable<T>(List<T> items, string Country)
        {
            DataTable dataTable = ToDataTable<T>(items);
           // dataTable.Columns.Remove("Name");
            dataTable.Columns.Remove("Id_Name");
            dataTable.Columns.Remove("TH_Name");
            dataTable.Columns.Remove("KR_Name");
            dataTable.Columns.Remove("JP_Name");
            dataTable.Columns.Remove("AU_Name");
            dataTable.Columns.Remove("BrandName");

            if (Country == "Korea")
            {
                dataTable.Columns["Id"].ColumnName = "No.";
                dataTable.Columns["LuK"].ColumnName = "LuK";
                dataTable.Columns["Msg1"].ColumnName = "Please specify";
                dataTable.Columns["INA"].ColumnName = "INA";
                dataTable.Columns["Msg2"].ColumnName = "Please specify ";
                dataTable.Columns["FAG"].ColumnName = "FAG";
                dataTable.Columns["Msg3"].ColumnName = " Please specify";
                dataTable.Columns["REPXPERT"].ColumnName = "REPXPERT";
                dataTable.Columns["Msg4"].ColumnName = " Please specify ";
                dataTable.Columns["TruPower"].ColumnName = "셰플러 TruPower";
                dataTable.Columns["Msg5"].ColumnName = " Please specify  ";
            }
            else
            {
                dataTable.Columns.Remove("TruPower");
                dataTable.Columns.Remove("Msg5");

                dataTable.Columns["Id"].ColumnName = "No.";
                dataTable.Columns["LuK"].ColumnName = "LuK";
                dataTable.Columns["Msg1"].ColumnName = "Please specify";
                dataTable.Columns["INA"].ColumnName = "INA";
                dataTable.Columns["Msg2"].ColumnName = "Please specify ";
                dataTable.Columns["FAG"].ColumnName = "FAG";
                dataTable.Columns["Msg3"].ColumnName = " Please specify";
                dataTable.Columns["REPXPERT"].ColumnName = "REPXPERT";
                dataTable.Columns["Msg4"].ColumnName = " Please specify ";
                //dataTable.Columns["TruPower"].ColumnName = "셰플러 TruPower";
                //dataTable.Columns["Msg5"].ColumnName = " Please specify  ";
            }
            dataTable.AcceptChanges();
            return dataTable;
        }

        private DataTable VehicleTypeDataTable<T>(List<T> items)
        {
            DataTable dataTable = ToDataTable<T>(items);
            //dataTable.Columns.Remove("Name");
            dataTable.Columns.Remove("Id_Name");
            dataTable.Columns.Remove("TH_Name");
            dataTable.Columns.Remove("KR_Name");
            dataTable.Columns.Remove("JP_Name");

            dataTable.Columns["Id"].ColumnName = "No.";
            dataTable.Columns["PassengerCar"].ColumnName = "Passenger Car";
            dataTable.Columns["LightCommercialVehicle"].ColumnName = "Light Commercial Vehicle";
            dataTable.Columns["HeavyCommercialVehicle"].ColumnName = "Heavy Commercial Vehicle";
            dataTable.Columns["Tractor"].ColumnName = "Tractor";

            dataTable.AcceptChanges();
            return dataTable;
        }

        private DataTable InformationTypeDataTable<T>(List<T> items)
        {
            DataTable dataTable = ToDataTable<T>(items);
           // dataTable.Columns.Remove("Name");
            dataTable.Columns.Remove("Id_Name");
            dataTable.Columns.Remove("TH_Name");
            dataTable.Columns.Remove("KR_Name");
            dataTable.Columns.Remove("JP_Name");
            dataTable.Columns.Remove("InformationName");

            dataTable.Columns["Id"].ColumnName = "No.";
            dataTable.Columns["ProductInformation"].ColumnName = "Product Information";
            dataTable.Columns["Msg1"].ColumnName = "Please specify";
            dataTable.Columns["TechnicalInformation"].ColumnName = "Technical Information";
            dataTable.Columns["Msg2"].ColumnName = "Please specify ";
            dataTable.Columns["NewProduct"].ColumnName = "New Product";
            dataTable.Columns["Msg3"].ColumnName = " Please specify";
            dataTable.Columns["Others"].ColumnName = "Others";
            dataTable.Columns["Msg4"].ColumnName = " Please specify ";
            dataTable.AcceptChanges();
            return dataTable;
        }
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
    }
}