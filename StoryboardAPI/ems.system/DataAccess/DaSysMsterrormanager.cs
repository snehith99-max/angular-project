using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.system.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
using OfficeOpenXml;
using System.Configuration;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using System.Globalization;

namespace ems.system.DataAccess
{
    

    public class DaSysMsterrormanager
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        HttpPostedFile httpPostedFile;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, dt_datatable1;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lserror_gid,lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsdepartment_code, lsdepartment_name, lsdepartment_prefix, lsdepartment_name_edit;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGeterrorSummary(MdlSysMsterrormanager values)
        {
            try
            {
                msSQL = " select error_gid,error_code,error_message,error_type from adm_mst_terror order by created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<errorcount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new errorcount
                        {
                            error_gid = dt["error_gid"].ToString(),
                            error_code = dt["error_code"].ToString(),
                            error_message = dt["error_message"].ToString(),
                            error_type = dt["error_type"].ToString()                          

                        });
                        values.errorcount = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }



        public void DaAdderrorsubmit(string employee_gid, errorsubmit values)
        {
            try
            {
                string error_message = values.error_message.Replace("'", "\\'");
                string error_type = values.error_type.Replace("'", "\\'");

                msSQL = "SELECT error_message FROM adm_mst_terror " +
                        "WHERE LOWER(error_message) = LOWER('" + values.error_message + "') " +
                        "OR UPPER(error_message) = UPPER('" + values.error_message + "')";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Error message already exist";
                    return;
                }

                msSQL = "SELECT error_code FROM adm_mst_terror " +
                        "WHERE LOWER(error_code) = LOWER('" + values.error_code + "') " +
                        "OR UPPER(error_code) = UPPER('" + values.error_code + "')";

                DataTable dt_datatable5 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable5.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Error code already exist";
                    return;
                }


                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("EROR");

                    msSQL = " insert into adm_mst_terror (" +
                               " error_gid," +
                               " error_code," +
                               " error_message," +
                               " error_type," +
                               " created_by, " +
                               " created_date)" +
                               " values(" +
                               " '" + msGetGid + "'," +
                               " '" + values.error_code + "'," +
                               " '" + error_message.Replace("'", "\\'") + "'," +
                               " '" + error_type.Replace("'", "\\'") + "'," +
                               " '" + employee_gid + "'," +
                               " '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Error Message Added Sucessfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding error Message";
                    }
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DagetUpdatederrormanage(string employee_gid, errorsubmit values)
        {
            string error_message = values.error_message.Replace("'", "\\'");

            msSQL = " SELECT error_message  FROM " +
                       " adm_mst_terror WHERE error_message = '" + error_message + "' and   error_gid !='" + values.error_gid + "' ";

            DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
            if (dt_datatable1.Rows.Count > 0)
            {
                values.status = false;
                values.message = "Error Message already exist";
                return;
            }

            msSQL = " select error_gid,error_code,error_message,error_type from adm_mst_terror" + 
                    " where error_gid='" +values.ref_no + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);    
            
            //msSQL = " update adm_mst_terror set " +
            //    " error_message= '" +values.error_message + "' " +
            //    " WHERE error_gid='" + values.error_gid + "' ";

            msSQL = " update adm_mst_terror set " +
                   " error_message = '" + error_message + "'," +
                   " error_type = '" + values.error_type.Replace("'", "\\'") + "'," +
                   " error_code = '" + values.error_code + "'," +
                   " error_gid = '" + values.ref_no + "'," +
                   " updated_by = '" + employee_gid + "'," +
                   " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' " +
                   " where error_gid='" + values.ref_no + "' ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)

            {
                values.status = true;
                values.message = "Error Message Updated Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Error Message";
            }
        }

    }
}