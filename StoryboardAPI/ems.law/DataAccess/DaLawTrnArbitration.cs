using ems.utilities.Functions;
using ems.law.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ems.law.DataAccess
{
    public class DaLawTrnArbitration
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2;
        string msGetGid, msGetGid1, lsempoyeegid;
        public void DaGetArbitrationsummary(MdlLawTrnArbitration values)
        {
            try
            {
                msSQL = "  select arbitration_gid,arbit_gid,arbit_type,arbitration_no,arbitration_date," +
                        "  arbitrator,title,status as arbit_status,institute_gid,created_date from law_trn_tarbitration " +
                        "  order by created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<arbitration_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new arbitration_list
                        {
                            arbitration_gid = dt["arbitration_gid"].ToString(),
                            arbit_gid = dt["arbit_gid"].ToString(),
                            arbit_type = dt["arbit_type"].ToString(),
                            arbitration_no = dt["arbitration_no"].ToString(),
                            arbitration_date = dt["arbitration_date"].ToString(),
                            arbitrator = dt["arbitrator"].ToString(),
                            title = dt["title"].ToString(),
                            arbit_status = dt["arbit_status"].ToString(),
                            institute_gid = dt["institute_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.arbitration_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading arbitration Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}