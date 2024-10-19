using ems.outlet.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.outlet.Dataaccess
{
    public class Darevenuecategory
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsentity_name, lsregion_code, lsregion_name, lsregion_gid, lscity_name_edit, lscity;


        public void Dagetrevenuesummary(Mdlrevenuecategory values)
        {
            try
            {
                msSQL = "select revenue_name, revenue_code,revenue_desc,revenue_gid from sys_mst_trevenuecategory" +
                    " order by revenue_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<revenue_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new revenue_list1
                        {
                            revenue_gid = dt["revenue_gid"].ToString(),
                            revenue_code = dt["revenue_code"].ToString(),
                            revenue_desc = dt["revenue_desc"].ToString(),
                            revenue_name = dt["revenue_name"].ToString(),


                        });
                        values.revenue_list1 = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Revenue Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void Dapostrevenue(string user_gid, revenue_list1 values)

        {
            try
            {

                msSQL = "select * from sys_mst_trevenuecategory where revenue_name = '"+values.revenue_name+"'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Revenue Name already exist";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("RCC");
                     msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='RCC' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lsrevenue_code = "RCC" + "000" + lsCode;

                    msGetGid1 = objcmnfunctions.GetMasterGID("RCRN");

                    msSQL = " insert into sys_mst_trevenuecategory(" +
                            " revenue_gid," +
                            " revenue_code," +
                            " revenue_desc," +
                            "revenue_name," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid1 + "'," +
                            " '" + lsrevenue_code + "'," +
                            "'" + values.revenue_desc + "'," +
                            "'" + values.revenue_name + "',";
                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Revenue Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Revenue!!";
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Revenue Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleterevenue(string revenue_gid, revenue_list1 values)
        {
            try
            {

                msSQL = "select revenue_amount from sys_mst_tdaytrackerdtl where revenue_gid ='" + revenue_gid + "'";
                 string lsrevenueamount = objdbconn.GetExecuteScalar(msSQL);

                if (lsrevenueamount == "" || lsrevenueamount == null)
                {

                    msSQL = "  delete from sys_mst_trevenuecategory where revenue_gid='" + revenue_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Revenue Deleted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Region !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Cannot delete Revenue detail due to daytracker raised";
                    return;

                }
               
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Region Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

    }
}