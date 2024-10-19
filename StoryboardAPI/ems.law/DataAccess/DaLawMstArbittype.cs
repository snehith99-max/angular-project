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

    public class DaLawMstArbittype : ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2;
        string msGetGid, msGetGid1, lsempoyeegid;
        public void DaGetarbittypesummary(MdlLawMstArbittype values)
        {
            try
            {
                msSQL = "  select a.arbit_gid,a.arbit_code,a.arbit_type,  "+
                        "  concat(c.user_firstname, ' ', c.user_lastname) as created_by  , "+
                        "  date_format(a.created_date, '%d-%b-%Y') as created_date  "+
                        "  from law_mst_tarbittype a    "+
                        "  left join adm_mst_tuser c on a.created_by = c.user_gid    "+
                        "  order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<arbit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new arbit_list
                        {
                            arbit_gid = dt["arbit_gid"].ToString(),
                            arbit_code = dt["arbit_code"].ToString(),
                            arbit_type = dt["arbit_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                        });
                        values.arbit_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading arbit_type Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPostArbittypeAdd(string user_gid, arbit_list values)
        {
            try
            {
                msSQL = " select * from law_mst_tarbittype where arbit_type = '" + values.arbit_type + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Arbitration Type Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("ARTE");
                    msSQL = " insert into law_mst_tarbittype(" +
                              " arbit_gid," +
                              " arbit_code," +
                              " arbit_type," +
                              " created_by, " +
                              " created_date)" +
                              " values(" +
                              " '" + msGetGid + "'," +
                              " '" + values.arbit_code + "'," +
                              " '" + values.arbit_type + "'," +
                              "'" + user_gid + "'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Arbitration Type Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Arbitration Type";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Arbitration Type Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPostUpdateArbittype(string user_gid, arbit_list values)
        {
            try
            {
                msSQL = " update law_mst_tarbittype set" +
                        " arbit_code = '" + values.arbit_code + "'," +
                        " arbit_type = '" + values.arbit_type + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                        " where arbit_gid = '" + values.arbit_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Arbitration Type Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While updating Arbitration Type";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Arbitration Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetDeletearbittype(string arbit_gid, arbit_list values)
        {
            try
            {
                msSQL = " delete from law_mst_tarbittype "+
                        " where arbit_gid = '" + arbit_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Arbitration Type Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Delete Arbitration Type";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while delete Arbitration Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}