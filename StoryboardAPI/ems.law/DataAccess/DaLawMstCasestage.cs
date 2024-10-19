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
    public class DaLawMstCasestage : ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2;
        string msGetGid, msGetGid1, lsempoyeegid;
        public void DaGetcasestagesummary(MdlLawMstCasestage values)
        {
            try
            {
                msSQL = "  select a.casestage_gid,a.casestage_code,a.casestage_name,  " +
                        "  concat(c.user_firstname, ' ', c.user_lastname) as created_by  , " +
                        "  date_format(a.created_date, '%d-%b-%Y') as created_date  " +
                        "  from lgl_mst_tcasestage a    " +
                        "  left join adm_mst_tuser c on a.created_by = c.user_gid    " +
                        "  order by a.casestage_code asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<casestage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new casestage_list
                        {
                            casestage_gid = dt["casestage_gid"].ToString(),
                            casestage_code = dt["casestage_code"].ToString(),
                            casestage_name = dt["casestage_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                        });
                        values.casestage_list = getModuleList;
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

        public void DaPostcasestageAdd(string user_gid, casestage_list values)
        {
            try
            {
                msSQL = " select * from lgl_mst_tcasestage where casestage_name = '" + values.casestage_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Case stage Already Exist !!";
                }
                msSQL = " select * from lgl_mst_tcasestage where casestage_code = '" + values.casestage_code + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Case stage Code Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("CSTE");
                    msSQL = " insert into lgl_mst_tcasestage(" +
                              " casestage_gid," +
                              " casestage_code," +
                              " casestage_name," +
                              " created_by, " +
                              " created_date)" +
                              " values(" +
                              " '" + msGetGid + "'," +
                              " '" + values.casestage_code + "'," +
                              " '" + values.casestage_name + "'," +
                              "'" + user_gid + "'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Case stage Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Case stage";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Case stage Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPostUpdatecasestage(string user_gid, casestage_list values)
        {
            try
            {
                msSQL = " select * from lgl_mst_tcasestage where casestage_name = '" + values.casestage_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Case stage Already Exist !!";
                }
                else
                {
                    msSQL = " update lgl_mst_tcasestage set" +
                            " casestage_name = '" + values.casestage_name + "'," +
                            " update_by = '" + user_gid + "'," +
                            " update_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                            " where casestage_gid = '" + values.casestage_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Case stage Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While updating Case stage";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Case stage !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetDeletecasestage(string casestage_gid, casestage_list values)
        {
            try
            {
                msSQL = " delete from lgl_mst_tcasestage " +
                        " where casestage_gid = '" + casestage_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Case stage Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Delete Case stage";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while delete Case stage !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}