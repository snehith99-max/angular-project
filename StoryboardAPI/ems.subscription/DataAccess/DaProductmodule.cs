using ems.utilities.Functions;
using ems.utilities.Models;
using System.Collections.Generic;
using System.Data.Odbc;
using ems.subscription.Models;
using System.Data;
using System.Web;
using System;
using System.Web.Http;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using Stripe.Forwarding;
using System.Data.Common;
using System.Web.Http;
using System.Web.Http.Results;

namespace ems.subscription.DataAccess
{
    public class DaProductmodule
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objODBCdatareader, objOdbcDataReader, objODBCDataReader, odbcDataReader;
        DataTable dt_datatable;
        int mnresult;
        string msGetGid;

        public void DaPostProductmodule(string employee_gid, productmodulelists values)
        {
            try
            {
                msSQL = " select productmodule_name from vcxcontroller.sub_mst_tproductmodule where productmodule_name='" + values.productmodule_name.ToUpper() + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Already Product Details Exist !..";
                    return;
                }
                objOdbcDataReader.Close();

                    msGetGid = objcmnfunctions.GetMasterGID("PRMO");
                msSQL = "insert into vcxcontroller.sub_mst_tproductmodule(" +
                            "productmodule_name," +
                            "productmodule_gid," +
                            "created_by," +
                            "created_date)" +
                            "values(" +
                            "'" + values.productmodule_name.ToUpper() + "'," +                           
                            "'" + msGetGid + "'," +                           
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnresult != 0)
                {
                    values.status = true;
                    values.message = "Product Details Added Successfully";
                }
                else
                {
                    values.message = "Error occured while adding!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetproductmoduleSummary(MdlProductmodule values)
        {
            try
            {
                msSQL = "select a.productmodule_name,a.productmodule_gid,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by from sub_mst_tproductmodule a " +
                    " left join hrm_mst_temployee b on a.created_by = b.employee_gid " +
                    "  left join adm_mst_tuser c on c.user_gid = b.user_gid order by productmodule_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<productlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productlists
                        {
                            productmodule_gid = dt["productmodule_gid"].ToString(),
                            productmodule_name = dt["productmodule_name"].ToString(),                           
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.productlists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteProductmodule(string productmodule_gid, MdlProductmodule values)

        {
            msSQL = "  delete from sub_mst_tproductmodule where productmodule_gid='" + productmodule_gid + "'  ";
            mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnresult != 0)
            {
                values.status = true;
                values.message = "Product Deleted Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Deleting Product";
                }
            }

        }
    }
}