using ems.law.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ems.law.DataAccess
{
    public class DaLawMstCasetype : ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2;
        string msGetGid, msGetGid1, lsempoyeegid;
        public void DaGetcasetypesummary(MdlLawMstCasetype values)
        {
            try
            {
                msSQL = "  select a.casetype_gid,a.casetype_code,a.casetype_name,  " +
                        "  concat(c.user_firstname, ' ', c.user_lastname) as created_by  , " +
                        "  date_format(a.created_date, '%d-%b-%Y') as created_date  " +
                        "  from lgl_mst_tcasetype a    " +
                        "  left join adm_mst_tuser c on a.created_by = c.user_gid    " +
                        "   order by a.casetype_code asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<case_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new case_list
                        {
                            casetype_gid = dt["casetype_gid"].ToString(),
                            casetype_code = dt["casetype_code"].ToString(),
                            casetype_name = dt["casetype_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                        });
                        values.case_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Case Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPostcasetypeAdd(string user_gid, case_list values)
        {
            try
            {
                msSQL = " select * from lgl_mst_tcasetype where casetype_name = '" + values.casetype_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Case Type Already Exist !!";
                }
                msSQL = " select * from lgl_mst_tcasetype where casetype_code = '" + values.casetype_code + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Case Code Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("CSTE");
                    msSQL = " insert into lgl_mst_tcasetype(" +
                              " casetype_gid," +
                              " casetype_code," +
                              " casetype_name," +
                              " created_by, " +
                              " created_date)" +
                              " values(" +
                              " '" + msGetGid + "'," +
                              " '" + values.casetype_code + "'," +
                              " '" + values.casetype_name + "'," +
                              "'" + user_gid + "'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Case Type Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Case Type";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Case Type Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPostUpdatecasetype(string user_gid, case_list values)
        {
            try
            {
                msSQL = " select * from lgl_mst_tcasetype where casetype_name = '" + values.casetype_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Case Type Already Exist !!";
                }
                else
                {
                    msSQL = " update lgl_mst_tcasetype set" +
                            " casetype_name = '" + values.casetype_name + "'," +
                            " update_by = '" + user_gid + "'," +
                            " update_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                            " where casetype_gid = '" + values.casetype_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Case Type Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While updating Case Type";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Case Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetDeletecasetype(string casetype_gid, case_list values)
        {
            try
            {
                msSQL = " delete from lgl_mst_tcasetype " +
                        " where casetype_gid = '" + casetype_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Case Type Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Delete Case Type";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while delete Case Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}