using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;
using System.Data.Common;

namespace ems.inventory.DataAccess
{
    public class DaImsMstBin
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        DataTable dt_datatable, dt_datatable1;
        string company_logo_path, authorized_sign_path;
        OdbcDataReader objOdbcDataReader, objODBCDataReader;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        int mnResult, mnResult2, mnResult1, mnResult3;
        DataTable dt1 = new DataTable();
        DataTable DataTable3 = new DataTable();
        string lsissuenow_qty, lsqty_issued, lsqty_requested, msGetGid;

        public void DaImsbinsummary(MdlImsMstBin values)
        {
            try
            {
                msSQL = " select a.branch_gid,a.location_gid,a.location_name,a.location_code,b.branch_code,  " +
                        " a.created_date,b.branch_name,b.branch_prefix,concat(c.user_firstname, ' ', c.user_lastname) as created_by " +
                        " from ims_mst_tlocation a " +
                        " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid " +
                        " left join adm_mst_tuser c on a.created_by = c.user_gid " +
                        " where 1 = 1";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<imsbin_summarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new imsbin_summarylist
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_code = dt["branch_code"].ToString(),
                            location_gid = dt["location_gid"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            location_code = dt["location_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.imsbin_summarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bin summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostBin(string user_gid, imsbin_addlist values)
        {
            try
            {
                msSQL = "Select * from ims_mst_tbin where location_gid = '" + values.location_gid + "' And bin_number='"+values.bin_number+"'";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable1.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Bin Number Already Exist";
                    return;
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("BTTP");
                    msSQL = " INSERT INTO ims_mst_tbin (" +
                            " bin_gid, " +
                            " location_gid, " +
                            " bin_number, " +
                            " created_date," +
                            " created_by)" +
                            " VALUES (" +
                            " '" + msGetGid + "'," +
                            " '" + values.location_gid + "'," +
                            " '" + values.bin_number + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                            " '" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                     values.status = true;
                     values.message = "Bin Added Successfully";
                    }
                    else
                    {
                     values.status = false;
                     values.message = "Error While Adding Bin";
                     }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Bin!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaImsbinaddsummary(string location_gid,MdlImsMstBin values)
        {
            try
            {
                msSQL = " select a.branch_gid,a.location_gid,a.location_name,a.location_code,b.branch_code,  " +
                        " a.created_date,b.branch_name,concat(c.user_firstname, ' ', c.user_lastname) as created_by " +
                        " from ims_mst_tlocation a " +
                        " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid " +
                        " left join adm_mst_tuser c on a.created_by = c.user_gid " +
                        " where 1 = 1 and location_gid='"+ location_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<imsbinadd_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new imsbinadd_list
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_code = dt["branch_code"].ToString(),
                            location_gid = dt["location_gid"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            location_code = dt["location_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.imsbinadd_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bin summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaImsbinadd(string location_gid, MdlImsMstBin values)
        {
            try
            {
                msSQL = " select bin_gid,location_gid,bin_number from ims_mst_tbin where location_gid='" + location_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<assignedbin_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new assignedbin_list
                        {
                            bin_gid = dt["bin_gid"].ToString(),
                            location_gid = dt["location_gid"].ToString(),
                            bin_number = dt["bin_number"].ToString(),
                        });
                        values.assignedbin_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bin summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaDeletebin(string bin_gid, MdlImsMstBin values)
        {
            try
            {
                msSQL = "select bin_gid from ims_trn_tstock where bin_gid='" + bin_gid + "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "you can not delete this bin because products taged in this bin!!";
                    objOdbcDataReader.Close();
                }
                else
                {
                    msSQL = " delete from ims_mst_tbin where bin_gid='" + bin_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Bin Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Bin";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Bin Delete!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    }
}