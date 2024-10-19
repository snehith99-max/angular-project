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
    public class DaCallResponse
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        int mnResult;
#pragma warning disable CS0169 // The field 'DaCallResponse.lscallresponse_code' is never used
#pragma warning disable CS0169 // The field 'DaCallResponse.msGetGid1' is never used
        string msGetGid, msGetGid1, lscall_response, lscallresponse_code, lscallresponse_gid, lsleadstage_gid;
#pragma warning restore CS0169 // The field 'DaCallResponse.msGetGid1' is never used
#pragma warning restore CS0169 // The field 'DaCallResponse.lscallresponse_code' is never used

        // Module Master Summary

        public void DaGetCallResponseSummary(MdlCallResponse values)
        {
            try
            {

                msSQL = "select  a.callresponse_gid,a.callresponse_code,a.active_flag,a.call_response," +
                    "b.leadstage_name, a.moving_stage from crm_mst_callresponse a left join crm_mst_tleadstage b " +
                    "on b.leadstage_gid = a.moving_stage left join adm_mst_tuser c on c.user_gid = a.created_by " +
                    "order by a.callresponse_gid desc;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<call_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new call_lists
                        {
                            callresponse_gid = dt["callresponse_gid"].ToString(),
                            call_response = dt["call_response"].ToString(),
                            callresponse_code = dt["callresponse_code"].ToString(),
                            moving_stagename = dt["leadstage_name"].ToString(),
                            moving_stage = dt["moving_stage"].ToString(),
                            active_flag = dt["active_flag"].ToString(),
                           

                        });
                        values.call_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Call Response Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }        
        }
        
        public void DaPostCallResponse(string user_gid, call_lists values)

        {
            try
            {
                 
                msSQL = " select call_response from crm_mst_callresponse where call_response = '" + values.call_response.Trim().Replace("'", "\\\'") + "'";
                objOdbcDataReader  = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader .HasRows == true)
                {
                    values.status = false;
                    values.message = "Call Response Already Exist !!";
                }

                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("MKCR");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='MKCR' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lscallresponse_code = "MCR" + "000" + lsCode;

                    msSQL = " insert into crm_mst_callresponse(" +
                                " callresponse_gid," +
                                " callresponse_code," +
                                " call_response," +
                                " moving_stage," +
                                " created_by," +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" + lscallresponse_code + "'," +
                                "'" + values.call_response.Trim().Replace("'", "\\\'") + "'," +
                                " '" + values.moving_stage.Replace("'", "\\\'") + "',";



                    msSQL += "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Call Response Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Call Response!!";
                    }
                }
                objOdbcDataReader .Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Call Response Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }


        public void GetupdateCallResponsedetails(string user_gid, call_lists values)

        {

            try
            {
                 
                msSQL = "select callresponse_gid,call_response from crm_mst_callresponse where call_response='" + values.callresponseedit_name.Trim().Replace("'", "\\\'") + "'";
                objOdbcDataReader  = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader .HasRows)
                {
                    lscallresponse_gid = objOdbcDataReader ["callresponse_gid"].ToString();
                    lscall_response = objOdbcDataReader ["call_response"].ToString();
                }

                if (lscallresponse_gid == values.callresponse_gid)
                {

                    if (values.movingstage_edit !=null)
                    {
                        // Your code here



                        msSQL = " update  crm_mst_callresponse set " +
                    " call_response = '" + values.callresponseedit_name.Trim().Replace("'", "\\\'") + "'," +
                    " moving_stage = '" + values.movingstage_edit.Replace("'", "\\\'") + "'," +
                    " created_by = '" + user_gid + "'," +
                    " created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where callresponse_gid='" + values.callresponse_gid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = "select leadstage_gid  from crm_mst_tleadstage where leadstage_name ='" + values.movingstage_edit.Replace("'", "\\\'")+ "'";
                        objOdbcDataReader  = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader .HasRows)
                        {
                            lsleadstage_gid = objOdbcDataReader ["leadstage_gid"].ToString();

                        }
                        msSQL = " update  crm_mst_callresponse set " +
                                " call_response = '" + values.callresponseedit_name.Trim().Replace("'", "\\\'") + "'," +
                                " moving_stage = '" + lsleadstage_gid + "'," +
                                " created_by = '" + user_gid + "'," +
                                " created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where callresponse_gid='" + values.callresponse_gid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Call Response Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Call Response !!";
                    }
                }
                else
                {
                    if (string.Equals(lscall_response, values.callresponseedit_name.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        values.status = false;
                        values.message = "Call Response with the same name already exists !!";
                    }
                    else
                    {

                        if (values.movingstage_edit!=null)
                        {
                            msSQL = "select leadstage_gid  from crm_mst_tleadstage where leadstage_name ='" + values.movingstage_edit.Replace("'", "\\\'") + "'";
                            objOdbcDataReader  = objdbconn.GetDataReader(msSQL);

                            if (objOdbcDataReader .HasRows)
                            {
                                lsleadstage_gid = objOdbcDataReader ["leadstage_gid"].ToString();


                            msSQL = " update  crm_mst_callresponse set " +
                                    " call_response = '" + values.callresponseedit_name.Trim().Replace("'", "\\\'") + "'," +
                                    " moving_stage = '" + lsleadstage_gid + "'," +
                                    " created_by = '" + user_gid + "'," +
                                    " created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where callresponse_gid='" + values.callresponse_gid + "'  ";

                            }
                            else
                            {
                            msSQL = " update  crm_mst_callresponse set " +
                                    " call_response = '" + values.callresponseedit_name.Trim().Replace("'", "\\\'") + "'," +
                                    " moving_stage = '" + values.movingstage_edit.Replace("'", "\\\'") + "'," +
                                    " created_by = '" + user_gid + "'," +
                                    " created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where callresponse_gid='" + values.callresponse_gid + "'  ";

                            }

                        }

                      
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Call Response Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Call Response !!";
                        }
                    }

                }
                objOdbcDataReader .Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Call Response Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }
        public void DaGetdeleteCallResponsedetails(string callresponse_gid, call_lists values)
        {
            try
            {
                 
                msSQL = " select call_response from crm_mst_callresponse where callresponse_gid='" + callresponse_gid + "'";
                string lscallresponse = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select calllog_gid from crm_trn_tcalllog where call_response = '" + callresponse_gid + "'";
                objOdbcDataReader  = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader .HasRows)
                {
                    values.status = false;
                    values.message = "This Call Response is in use and cannot be deleted !!!";

                }
                else
                {
                    msSQL = "  delete from crm_mst_callresponse where callresponse_gid='" + callresponse_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Call Response Deleted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Call Response !!";
                    }
                }
                objOdbcDataReader .Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Call Response Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetResponseINActive(string callresponse_gid, MdlCallResponse values)
        {
            try
            {

                msSQL = " update crm_mst_callresponse set" +
                        " active_flag = 'N'" +
                        " where callresponse_gid = '" + callresponse_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Response Deactivated Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Response Deactivating";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while   Updating Response Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetResponseActive(string callresponse_gid, MdlCallResponse values)
        {
            try
            {

                msSQL = " update crm_mst_callresponse set" +
                        " active_flag = 'Y'" +
                        " where callresponse_gid = '" + callresponse_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Response Activated Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Response Activating";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Response Activated!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetleadstagedropdown(MdlCallResponse values)
        {
            try
            {

                msSQL = "select leadstage_gid,leadstage_name from crm_mst_tleadstage where leadstage_gid<3 or leadstage_gid=5 or leadstage_gid=7 ;";
                  dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadstagedropdown_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadstagedropdown_list
                        {
                            leadstage_gid = dt["leadstage_gid"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                           

                        });
                        values.leadstagedropdown_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting leadstage dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

    }
}
