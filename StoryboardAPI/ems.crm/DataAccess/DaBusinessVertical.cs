using ems.crm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.crm.DataAccess
{
    public class DaBusinessVertical
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
#pragma warning disable CS0169 // The field 'DaBusinessVertical.msGetGid1' is never used
        string msGetGid, msGetGid1, lsbusinessvertical_gid, lsbusiness_vertical, lsbusinessvertical_desc;
#pragma warning restore CS0169 // The field 'DaBusinessVertical.msGetGid1' is never used
        public void DaGetBusinessSummary(MdlBusinessVertical values)
        {
            try
            {

                msSQL = "select businessvertical_gid,business_vertical,businessvertical_desc,status_flag from crm_mst_tbusinessvertical";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<businessvertical_summary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new businessvertical_summary
                        {
                            businessvertical_gid = dt["businessvertical_gid"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),
                            businessvertical_desc = dt["businessvertical_desc"].ToString(),
                            status_flag = dt["status_flag"].ToString(),

                        });
                        values.businessvertical_summary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Business Vertical Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostbusinessvertical(string user_gid, businessvertical_summary values)
        {
            try
            {
                msSQL = " select business_vertical from crm_mst_tbusinessvertical where business_vertical = '" + values.business_vertical.Trim().Replace("'", "\\\'")+ "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Business Vertical Already Exist !!";
                }
                else
                {
                    if(values.businessvertical_desc != null)
                    {
                        lsbusinessvertical_desc= values.businessvertical_desc.Trim().Replace("'", "\\\'");
                    }
                    else
                    {
                        lsbusinessvertical_desc = values.businessvertical_desc;
                    }
                    msGetGid = objcmnfunctions.GetMasterGID("BV");

                    msSQL = " insert into crm_mst_tbusinessvertical(" +
                                " businessvertical_gid," +
                                " business_vertical," +
                                " businessvertical_desc," +
                                " created_by," +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" +values.business_vertical.Trim().Replace("'", "\\\'") + "'," +
                                "'" + lsbusinessvertical_desc + "',";
                    msSQL += "'" + user_gid + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Business Vertical Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Business Vertical!!";
                    }

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Posting Business Vertical !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetDeletebusinessvertical(string businessvertical_gid, businessvertical_summary values)
        {
            try
            {

                    msSQL = "  delete from crm_mst_tbusinessvertical where businessvertical_gid = '" + businessvertical_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Business Vertical Deleted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Business Vertical !!";
                    }
                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Business Vertical!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetUpdatebusinessvertical(string user_gid, businessvertical_summary values)
        {
            try
            {
                msSQL = " select businessvertical_gid,business_vertical from crm_mst_tbusinessvertical where business_vertical = '" + values.businessvertiacal_edit.Trim().Replace("'", "\\\'") + "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows == true)
                {
                    lsbusinessvertical_gid = objOdbcDataReader["businessvertical_gid"].ToString();
                    lsbusiness_vertical = objOdbcDataReader["business_vertical"].ToString();
                }
                if (lsbusinessvertical_gid == values.businessvertical_gidedit.Trim())
                {
                    msSQL = " update  crm_mst_tbusinessvertical set " +
                            " business_vertical = '" + values.businessvertiacal_edit.Trim().Replace("'", "\\\'") + "'," +
                            " businessvertical_desc = '" + values.businessvertical_descedit.Replace("'", "\\\'") + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where businessvertical_gid  ='" + values.businessvertical_gidedit + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Business Vertical Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Business Vertical !!";
                    }
                }
                else
                {
                    if (string.Equals(lsbusiness_vertical, values.businessvertiacal_edit, StringComparison.OrdinalIgnoreCase))
                    {
                        values.status = false;
                        values.message = "Business Vertical with the same name already exists !!";
                    }
                    else
                    {
                        msSQL = " update  crm_mst_tbusinessvertical set " +
                                " business_vertical = '" + values.businessvertiacal_edit.Trim().Replace("'", "\\\'") + "'," +
                                " businessvertical_desc = '" + values.businessvertical_descedit.Replace("'", "\\\'") + "'," +
                                " updated_by = '" + user_gid + "'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where businessvertical_gid  ='" + values.businessvertical_gidedit + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Business Vertical Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Business Vertical !!";
                        }
                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Business Vertical !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaActivatebusinessvertical(string businessvertical_gid, businessvertical_summary values)
        {
            try
            {
                msSQL = " update crm_mst_tbusinessvertical set " +
                        "status_flag = 'Y'" +
                        " where businessvertical_gid = '" + businessvertical_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Business Vertical Activated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Activating The Business Vertical !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Activate The Business Vertical!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaInactivatebusinessvertical(string businessvertical_gid, businessvertical_summary values)
        {
            try
            {
                msSQL = " update crm_mst_tbusinessvertical set " +
                        "status_flag = 'N'" +
                        " where businessvertical_gid = '" + businessvertical_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Business Vertical Deactivated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deactivating The Business Vertical !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deactivate The Business Vertical!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}
    