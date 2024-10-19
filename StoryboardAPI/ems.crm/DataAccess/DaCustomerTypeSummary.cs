using ems.crm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ems.crm.DataAccess
{
    public class DaCustomerTypeSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsdisplay_name, lscustomertype_gid;
        // Customer Type Summary
        public void DaGetCustomerTypeSummary(MdlCustomerTypeSummary values)
        {
            try
            {

                msSQL = "select customertype_gid,display_name,customertype_desc,customer_type,status_flag from crm_mst_tcustomertype";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<customertypesummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new customertypesummary_lists
                        {
                            customertype_gid = dt["customertype_gid"].ToString(),
                            display_name = dt["display_name"].ToString(),
                            customertype_desc = dt["customertype_desc"].ToString(),
                            status_flag = dt["status_flag"].ToString(),

                        });
                        values.customertypesummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Type Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostCustomerType(string user_gid, customertypesummary_lists values)
        {
            try
            {
                msSQL = " select display_name from crm_mst_tcustomertype where display_name = '" + values.customer_type + "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Customer type Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("BCRT");
                    msSQL = " insert into crm_mst_tcustomertype(" +
                                " customertype_gid," +
                                " display_name," +
                                "customer_type," +
                                " customertype_desc," +
                                " created_by," +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" + values.customer_type + "'," +
                                " '" + values.customer_type + "'," +
                                "'" + values.customertype_description + "',";
                    msSQL += "'" + user_gid + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Customer Type Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Customer Type!!";
                    }

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Post Customer Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetDeleteCustomerType(string customertype_gid, customertypesummary_lists values)
        {
            try
            {

                msSQL = "select customertype_gid from crm_trn_tleadbank where customertype_gid = '" + customertype_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "This Customer Type is in use and cannot be deleted !!!";

                }
                else
                {
                    msSQL = "  delete from crm_mst_tcustomertype where customertype_gid = '" + customertype_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Customer Type Deleted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Customer Type !!";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Customer Type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetUpdateCustomerType(string user_gid, customertypesummary_lists values)
        {
            try
            {
                msSQL = " select customertype_gid,display_name from crm_mst_tcustomertype where display_name = '" + values.customer_typeedit + "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows == true)
                {
                    lscustomertype_gid = objOdbcDataReader["customertype_gid"].ToString();
                    lsdisplay_name = objOdbcDataReader["display_name"].ToString();
                }
                if (lscustomertype_gid== values.customertype_gidedit)
                {
                    msSQL = " update  crm_mst_tcustomertype set " +
                            " display_name = '" + values.customer_typeedit + "'," +
                            " customertype_desc = '" + values.customertype_descriptionedit + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where customertype_gid  ='" + values.customertype_gidedit + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Customer Type Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Customer Type !!";
                    }
                }
                else 
                {
                    if (string.Equals(lsdisplay_name, values.customer_typeedit, StringComparison.OrdinalIgnoreCase))
                    {
                        values.status = false;
                        values.message = "Customer Type with the same name already exists !!";
                    }
                    else
                    {
                        msSQL = " update  crm_mst_tcustomertype set " +
                                " display_name = '" + values.customer_typeedit + "'," +
                                " customertype_desc = '" + values.customertype_descriptionedit + "'," +
                                " updated_by = '" + user_gid + "'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where customertype_gid  ='" + values.customertype_gidedit + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Customer Type Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Customer Type !!";
                        }
                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Customer Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaActivateCustomerType(string customertype_gid, customertypesummary_lists values)
        {
            try
            {
                    msSQL = " update crm_mst_tcustomertype set "+
                            "status_flag = 'Y'" +
                            " where customertype_gid = '" + customertype_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Customer Type Activated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Activating The Customer Type !!";
                    }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Activate The Customer Type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaInactivateCustomerType(string customertype_gid, customertypesummary_lists values)
        {
            try
            {
                msSQL = " update crm_mst_tcustomertype set " +
                        "status_flag = 'N'" +
                        " where customertype_gid = '" + customertype_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Customer Type Deactivated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deactivating The Customer Type !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deactivate The Customer Type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}