using Dapper;
using Schaeffler.Domain;
using Schaeffler.Persistence.DBConnectionFactory;
using Schaeffler.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Persistence.Implementation
{
    public class FeedbackDao : IFeedbackDao
    {
        #region Private variables and constructors

        protected readonly IDbConnectionFactory factory;

        public FeedbackDao()
        {
            this.factory = new DbConnectionFactory("DefaultDB");
        }
        #endregion

        public Int64 Savefeedback(feedback AI)
        {
            Int64 result = 0;
            DataTable BrandServiceTbl = new DataTable("BrandServiceTbl");
            BrandServiceTbl.Columns.Add("Id", typeof(Int64));
            BrandServiceTbl.Columns.Add("Name", typeof(string));

            if (AI != null && AI.BrandService != null && AI.BrandService.Count() > 0)
            {
                AI.BrandService.ForEach(d =>
                {
                    {
                        if (d.Id > 0)
                        {
                            BrandServiceTbl.Rows.Add(d.Id, d.Name);
                        }
                    }
                });
            }


            DataTable VehicleTypeTbl = new DataTable("VehicleTypeTbl");
            VehicleTypeTbl.Columns.Add("Id", typeof(Int64));


            if (AI != null && AI.VehicleType != null && AI.VehicleType.Count() > 0)
            {
                AI.VehicleType.ForEach(d =>
                {
                    {
                        if (d.Id > 0)
                        {
                            VehicleTypeTbl.Rows.Add(d.Id);
                        }
                    }
                });
            }


            DataTable InformationTypeTbl = new DataTable("InformationTypeTbl");
            InformationTypeTbl.Columns.Add("Id", typeof(Int64));
            InformationTypeTbl.Columns.Add("Name", typeof(string));

            if (AI != null && AI.InformationType != null && AI.InformationType.Count() > 0)
            {
                AI.InformationType.ForEach(d =>
                {
                    {
                        if (d.Id > 0)
                        {
                            InformationTypeTbl.Rows.Add(d.Id, d.Name);
                        }
                    }
                });
            }

            DynamicParameters param = new DynamicParameters();

            param.Add("@FullName", AI.FullName, dbType: DbType.String);
            param.Add("@Email", AI.Email, dbType: DbType.String);
            param.Add("@CompanyName", AI.CompanyName, dbType: DbType.String);
            param.Add("@PhoneNumber", AI.PhoneNumber, dbType: DbType.String);
            param.Add("@Message", AI.Message, dbType: DbType.String);
            param.Add("@SystemIP", AI.SystemIp, dbType: DbType.String);
            param.Add("@Country", AI.Country, dbType: DbType.String);

            param.Add("@BrandServiceTbl", BrandServiceTbl.AsTableValuedParameter());
            param.Add("@VehicleTypeTbl", VehicleTypeTbl.AsTableValuedParameter());
            param.Add("@InformationTypeTbl", InformationTypeTbl.AsTableValuedParameter());
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_Savefeedback]";
                result = conn.Query<Int64>(SQL, param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                conn.Close();
            }

            return result;
        }


        public IList<BrandService> GetBrandServices()
        {
            IList<BrandService> C = null;
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_GetBrandService]";
                C = conn.Query<BrandService>(SQL, commandType: CommandType.StoredProcedure).ToList<BrandService>();
            }
            return C;
        }

        public IList<VehicleType> GetVehicleTypes()
        {
            IList<VehicleType> C = null;
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_GetVehicleType]";
                C = conn.Query<VehicleType>(SQL, commandType: CommandType.StoredProcedure).ToList<VehicleType>();
            }
            return C;
        }

        public IList<InformationType> GetInformationTypes()
        {
            IList<InformationType> C = null;
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_GetInformationType]";
                C = conn.Query<InformationType>(SQL, commandType: CommandType.StoredProcedure).ToList<InformationType>();
            }
            return C;
        }


        public ReportModelView GetFeedbackReport()
        {
            ReportModelView report = new ReportModelView();
            List<feedback> feedback = null;
            DynamicParameters param = new DynamicParameters();

            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[GetFeedBackReport]";
                var contents = conn.QueryMultiple(SQL, param, commandType: CommandType.StoredProcedure);
                if (contents != null)
                {
                    feedback = contents.Read<feedback>().ToList();
                    if (feedback != null)
                    {
                        report.Feedback = feedback;

                        report.BrandService = contents.Read<BrandService>().ToList();
                        report.VehicleType = contents.Read<VehicleType>().ToList();
                        report.InformationType = contents.Read<InformationType>().ToList();
                    }
                }
            }
            return report;

        }

        public feedback GetFeedback(Int64 Id)
        {

            feedback feedback = new feedback();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", Id);
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_GetFeedbackById]";
                var contents = conn.QueryMultiple(SQL, param, commandType: CommandType.StoredProcedure);
                if (contents != null)
                {
                    feedback = contents.Read<feedback>().FirstOrDefault();
                    if (feedback != null)
                    {


                        feedback.BrandService = contents.Read<BrandService>().ToList();
                        feedback.VehicleType = contents.Read<VehicleType>().ToList();
                        feedback.InformationType = contents.Read<InformationType>().ToList();
                    }
                }
            }
            return feedback;

        }

        public ReportModelView GetFeedbackReportNew(string Country)
        {
            ReportModelView report = new ReportModelView();
            List<feedback> feedback = null;
            DynamicParameters param = new DynamicParameters();
            param.Add("@Country", Country);
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[GetFeedBackReportNew]";
                var contents = conn.QueryMultiple(sql: SQL, param: param, commandType: CommandType.StoredProcedure);
                if (contents != null)
                {
                    feedback = contents.Read<feedback>().ToList();
                    if (feedback != null)
                    {
                        report.Feedback = feedback;

                        report.BrandService = contents.Read<BrandService>().ToList();
                        report.VehicleType = contents.Read<VehicleType>().ToList();
                        report.InformationType = contents.Read<InformationType>().ToList();
                    }
                }
            }
            return report;

        }


    }
}
