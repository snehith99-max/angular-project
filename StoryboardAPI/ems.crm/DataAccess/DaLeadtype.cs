using ems.crm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ems.crm.DataAccess
{
    public class DaLeadtype
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        DataTable dt_datatable;
        OdbcDataReader objOdbcDataReader;
        int mnResult;
        string msGetGid, lsleadtype_gid, lsleadtype_name;

        public void DaLeadtypeSummary(MdlLeadtype values)
        {
            try
            {
                    msSQL = " select  a.leadtype_gid, a.leadtype_name,a.leadtype_code,a.status_flag, CONCAT(b.user_firstname,' ',b.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date " +
                        " from crm_mst_tleadtype a " +
                        " left join adm_mst_tuser b on b.user_gid=a.created_by order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadtype_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadtype_lists
                        {
                           leadtype_gid = dt["leadtype_gid"].ToString(),
                            leadtype_name = dt["leadtype_name"].ToString(),
                            leadtype_code = dt["leadtype_code"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            status_flag= dt["status_flag"].ToString()
                        });
                        values.leadtype_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Type Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostLeadtype(string user_gid, leadtype_lists values)
        {
            try
            {
                msSQL = " select leadtype_name from crm_mst_tleadtype where leadtype_name ='"+values.leadtype_name.Trim().Replace("'", "\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Lead Type Name Already Exist !!";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("BLTM");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='BLTM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lsleadtype_code = "LTM" + "000" + lsCode;
                    msSQL = " insert into crm_mst_tleadtype (" +
                                   " leadtype_gid," +
                                   " leadtype_code," +
                                   " leadtype_name," +
                                   " created_by, " +
                                   " created_date)" +
                                   " values(" +
                                   " '" + msGetGid + "'," +
                                   " '" + lsleadtype_code + "'," +
                                   "'" + values.leadtype_name.Trim().Replace("'", "\\'") + "',"+
                                   "'" + user_gid + "'," +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Lead Type Added Successfully";
                    }
                    else
                    {

                        values.status = false;
                        values.message = "Error While Adding Leadtype";
                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Lead Type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaUpdatedLeadtype(string user_gid, leadtype_lists values)

        {

            try
            {

                msSQL = " select leadtype_gid,leadtype_name from crm_mst_tleadtype where leadtype_name = '" + values.leadtype_nameedit.Trim().Replace("'", "\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows)
                {
                    lsleadtype_gid = objOdbcDataReader["leadtype_gid"].ToString();
                    lsleadtype_name = objOdbcDataReader["leadtype_name"].ToString();
                }

                if (lsleadtype_gid == values.leadtype_gid)
                {

                    msSQL = " update  crm_mst_tleadtype  set " +
                         " leadtype_code = '" + values.leadtype_codeedit + "'," +
                  " leadtype_name = '" + values.leadtype_nameedit.Trim().Replace("'", "\\'") + "'," +
                  " updated_by = '" + user_gid + "'," +
                  " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where leadtype_gid='" + values.leadtype_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Lead Type Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Leadtype !!";
                    }
                }
                else
                {
                    if (string.Equals(lsleadtype_name, values.leadtype_nameedit.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        values.status = false;
                        values.message = "Lead Type with the same name already exists !!";
                    }
                    else
                    {
                        msSQL = " update  crm_mst_tleadtype  set " +
                       " leadtype_code = '" + values.leadtype_codeedit + "'," +
                " leadtype_name = '" + values.leadtype_nameedit.Trim().Replace("'", "\\'") + "'," +
                " updated_by = '" + user_gid + "'," +
                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where leadtype_gid='" + values.leadtype_gid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Lead Type Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Leadtype!!";
                        }
                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Lead Type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DadeleteLeadtypeSummary(string leadtype_gid, leadtype_lists values)
        {

            try
            {

                msSQL = "select lead_type from crm_trn_tleadbank where lead_type='" + leadtype_gid + "';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Lead Type already used hence can't be deleted!!";
                }
                else
                {
                    msSQL = "  delete from crm_mst_tleadtype where leadtype_gid='" + leadtype_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;

                        values.message = "Lead Type Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Lead Type";
                    }

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Lead Type Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaOnStatusUpdateLeadtype(mdlstatus_update values  )
        {
            try
            {
                msSQL = "update crm_mst_tleadtype set status_flag ='"+values.status_flag+ "' WHERE leadtype_gid='"+values.leadtype_gid+"' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    
                    
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Status Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Status !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating the Lead Type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}


