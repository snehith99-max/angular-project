using ems.crm.Models;
using ems.utilities.Functions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ems.crm.DataAccess
{
    public class DaConstitution
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        OdbcDataReader objOdbcDataReader;
        string msSQL = string.Empty;
        DataTable dt_datatable;
#pragma warning disable CS0169 // The field 'DaConstitution.msGetGid1' is never used
        string msGetGid, msGetGid1, msGetEntityCode;
#pragma warning restore CS0169 // The field 'DaConstitution.msGetGid1' is never used
        int mnResult;
        string lsmaster_value;
        public void DaConstitutionSummary(MdlConstitution values)
        {
            try
            {
                msSQL = "select a.constitution_gid,a.constitution_code,a.constitution_name,a.status as Status,date_format(a.created_date,'%d-%m-%Y') as created_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by " +
                        "from crm_mst_tconstitution a " +
                       " left join hrm_mst_temployee b on a.created_by = b.user_gid" +
                       " left join adm_mst_tuser c on c.user_gid = b.user_gid" +
                        " order by a.constitution_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<constitution_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)

                    {
                        getModuleList.Add(new constitution_list

                        {
                            constitution_code = (dt["constitution_code"].ToString()),
                            constitution_name = (dt["constitution_name"].ToString()),
                            Status = (dt["status"].ToString()),
                            constitution_gid = (dt["constitution_gid"].ToString()),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.constitutionlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "DTS");
                ex.StackTrace.ToString();
                values.status = false;

            }
        }
        public void DaConstitutionAdd(constitution_list values, string user_gid)
        {
            try
            {

                msSQL = " select constitution_name from crm_mst_tconstitution " +
                     " where LOWER (constitution_name) = '" + values.constitution_name.Replace("'", "\\'").ToLower() + "'" +
                     " and constitution_gid !='" + values.constitution_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {

                    values.status = false;
                    values.message = "Constitution Name Already Exist";
                    return;
                }

                msGetGid = objcmnfunctions.GetMasterGID("AENG");
                msGetEntityCode = objcmnfunctions.GetMasterGID("AENC");
                msSQL = " insert into crm_mst_tconstitution(" +
                        " constitution_gid," +
                        " constitution_code," +
                        " constitution_name," +
                        " created_by," +
                        " status," +
                        " created_date)" +
                        " values(" +
                        "'" + msGetGid + "'," +
                         "'" + msGetEntityCode + "'," +
                        "'" + values.constitution_name.Replace("'", "''").Trim() + "', " +
                         " '" + user_gid + "', 'Y', " +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Constitution Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Constitution";
                }
                objOdbcDataReader.Close();
            }
            
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "DTS");
                ex.StackTrace.ToString();
                values.status = false;

            }
        }
        public bool DaConstitutionDelete(string constitution_gid, string employee_gid, constitution_list values)
        {
            try
            {
                msSQL = " select constitution_name from crm_mst_tconstitution where constitution_gid='" + constitution_gid + "'";
                lsmaster_value = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "delete from crm_mst_tconstitution where constitution_gid='" + constitution_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Constitution Deleted Successfully";
                    values.status = true;
                    return true;
                }
                else
                {
                    values.message = "Error while Deleting Constitution";
                    values.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "DTS");
                ex.StackTrace.ToString();
                values.status = false;
                return false;
            }
        }
        public void DaGetConstitutionEdit(string constitution_gid, constitution_list values)
        {
            try
            {
                msSQL = " select constitution_gid,constitution_code,constitution_name,status as Status from crm_mst_tconstitution" +
                        " where constitution_gid='" + constitution_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.constitution_gid = objOdbcDataReader["constitution_gid"].ToString();
                    values.constitution_code = objOdbcDataReader["constitution_code"].ToString();
                    values.constitution_name = objOdbcDataReader["constitution_name"].ToString();
                    values.status_log = objOdbcDataReader["Status"].ToString();
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "DTS");
                ex.StackTrace.ToString();
                values.status = false;

            }
        }
        public void DaUpdateConstitution(string user_gid, constitution_list values)
        {
            try
            {
                msSQL = " select constitution_name from crm_mst_tconstitution " +
                     " where LOWER (constitution_name) = '" + values.constitution_name.Replace("'", "\\'").ToLower() + "'" +
                     " and constitution_gid !='" + values.constitution_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {

                    values.status = false;
                    values.message = "Constitution Name Already Exist";
                    return;
                }
                msSQL = " select constitution_name from  crm_mst_tconstitution where constitution_gid ='" + values.constitution_gid + "' ";
                string lsconstitution = objdbconn.GetExecuteScalar(msSQL);

                if (values.constitution_name == lsconstitution)
                {
                    values.status = false;
                    values.message = "No changes in Constitution Name";
                    return;
                }
                msSQL = " update crm_mst_tconstitution set " +
                        " constitution_gid ='" + values.constitution_gid + "', " +
                        " constitution_code ='" + values.constitution_code + "', " +
                        " constitution_name ='" + values.constitution_name.Replace("'", "''").Trim()+ "', " +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where constitution_gid='" + values.constitution_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Constitution Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Constitution";
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "DTS");
                ex.StackTrace.ToString();
                values.status = false;

            }
        }
        public void DaInactiveConstitution(Constitutionstatus values, string employee_gid)
        {
            try
            {
                msSQL = " update crm_mst_tconstitution set status='" + values.rbo_status + "'," +
                        " remarks='" + values.remarks.Replace("'", "''") + "'" +
                        " where constitution_gid='" + values.constitution_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Constitution Inactivated Successfully";
                }
                else
                {
                    values.status = true;
                    values.message = "Constitution Activated Successfully";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "DTS");
                ex.StackTrace.ToString();
                values.status = false;

            }
        }
        public void DaConstitutionInactiveHistory(ConstitutionInactiveHistory objapplicationhistory, string constitution_gid)
        {
            try
            {
                msSQL = " select a.remarks, CONVERT(NVARCHAR(10),a.updated_date, 105) as updated_date, " +
                " concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as updated_by," +
                " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                " from crm_mst_tconstitutioninactivelog a " +
                " left join hrm_mst_temployee b on a.updated_by = b.employee_gid" +
                " left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                " where a.constitution_gid='" + constitution_gid + "' order by a.constitutioninactivelog_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getinactiveHistory_list = new List<ConstitutionInactiveHistory_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getinactiveHistory_list.Add(new ConstitutionInactiveHistory_list
                        {
                            status_log = (dr_datarow["status"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                            updated_by = (dr_datarow["updated_by"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString())
                        });
                    }
                    objapplicationhistory.ConstitutionInactiveHistorylist = getinactiveHistory_list;
                }

                objapplicationhistory.status = true;
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "DTS");
                ex.StackTrace.ToString();
                objapplicationhistory.status = false;

            }
        }
        public void Daconstitutionstatusupdate(mdConstitutionstatus values)
        {

            try
            {
                msSQL = "update crm_mst_tconstitution set status ='" + values.status_flag + "' WHERE constitution_gid='" + values.constitution_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    if(values.status_flag == "Y")
                    {
                        values.status = true;
                        values.message = "Constitution Activated Successfully !!";
                    }
                    else
                    {
                        values.status = true;
                        values.message = "Constitution Inactivated Successfully !!";

                    }
                    
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Status !!";
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "DTS");
                ex.StackTrace.ToString();


            }
        }
    }
}
