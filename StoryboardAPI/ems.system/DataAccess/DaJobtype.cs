using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.system.Models;

namespace ems.system.DataAccess
{
    public class DaJobtype
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty, msGetGID;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_dataTable, objtbl;
        int mnResult;
        public void DagetJobtypeSummary(MdlJobtype values)
        {
            try
            {

                msSQL = " select jobtype_gid,jobtype_code,jobtype_name from hrm_mst_tjobtype" +
                        " where 1=1 order by jobtype_gid desc";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<JobtypeLists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new JobtypeLists
                        {
                            Jobtype_gid = dt["jobtype_gid"].ToString(),
                            JobType_Code = dt["jobtype_code"].ToString(),
                            JobType_Name = dt["jobtype_name"].ToString(),

                        });
                        values.JobtypeLists = getModuleList;
                    }
                }
                dt_dataTable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPostJobtype(JobtypeLists values, string user_gid)
        {
            try
            {
                msSQL = "SELECT jobtype_code FROM hrm_mst_tjobtype " +
                        "WHERE LOWER(jobtype_code) = LOWER('" + values.JobType_Code + "') " +
                        "OR UPPER(jobtype_code) = UPPER('" + values.JobType_Code + "')";

                DataTable dt_datatable5 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable5.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "jobtype code already exist";
                    return;
                }

                msSQL = "SELECT jobtype_name FROM hrm_mst_tjobtype " +
                        "WHERE LOWER(jobtype_name) = LOWER('" + values.JobType_Name.Replace("'", "\\'") + "') " +
                        "OR UPPER(jobtype_name) = UPPER('" + values.JobType_Name.Replace("'", "\\'") + "')";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Jobtype already exist";
                    return;
                }
                else
                {

                    string msGetGID = objcmnfunctions.GetMasterGID("SDGM");

                    msSQL = " insert into hrm_mst_tjobtype(" +
                            " jobtype_gid," +
                            " jobtype_code," +
                            " jobtype_name,created_by,created_date)" +
                            " values(" +
                            "'" + msGetGID + "'," +
                            "'" + values.JobType_Code + "'," +
                            "'" + values.JobType_Name.Replace("'", "\\'") + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Jobtype details added successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured while adding job details";
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DagetUpdatedJobtype(string user_gid, JobtypeLists values)
        {
            try
            {
                msSQL = "SELECT jobtype_code FROM hrm_mst_tjobtype " +
                        "WHERE LOWER(jobtype_code) = LOWER('" + values.JobType_Code + "') " +
                        "OR UPPER(jobtype_code) = UPPER('" + values.JobType_Code + "')";

                DataTable dt_datatable5 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable5.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "jobtype code already exist";
                    return;
                }
                msSQL = "SELECT jobtype_name FROM hrm_mst_tjobtype " +
                        "WHERE LOWER(jobtype_name) = LOWER('" + values.JobType_Name.Replace("'", "\\'") + "') " +
                        "OR UPPER(jobtype_name) = UPPER('" + values.JobType_Name.Replace("'", "\\'") + "')";

                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Jobtype already exist";
                    return;
                }

                else
                {
                    msSQL = " update hrm_mst_tjobtype set " +
                            " jobtype_code = '" + values.JobType_Code + "'," +
                            " jobtype_name = '" + values.JobType_Name.Replace("'", "\\'") + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' " +
                            " where jobtype_gid='" + values.Jobtype_gid + "' ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)

                    {
                        values.status = true;
                        values.message = "Jobtype Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Jobtype";
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteJobtype(string jobtype_gid, JobtypeLists values)
        {
            try
            {
                msSQL = "  delete from hrm_mst_tjobtype where jobtype_gid='" + jobtype_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Jobtype Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Jobtype";
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
    }
}