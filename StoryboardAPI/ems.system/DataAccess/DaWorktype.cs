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
    public class DaWorktype
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty, msGetGID;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_dataTable, objtbl;
        int mnResult;

        public void DagetWorktypeSummary(MdlWorktype values)
        {
            try
            {

                msSQL = "select workertype_gid,workertype_name from hrm_mst_tworkertype" + " where 1=1 order by workertype_name desc";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<WorktypeLists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new WorktypeLists
                        {
                            Worktype_gid = dt["workertype_gid"].ToString(),                           
                            WorkType_Name = dt["workertype_name"].ToString(),

                        });
                        values.WorktypeLists = getModuleList;
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
        public void DaPostWorktype(WorktypeLists values, string user_gid)
        {
            try
            {
                string msGetGID = objcmnfunctions.GetMasterGID("SDGM");

                msSQL = " insert into hrm_mst_tworkertype(" +
                        " workertype_gid," +                       
                        " workertype_name,created_by,created_date)" +
                        " values(" +
                        "'" + msGetGID + "'," +                        
                        "'" + values.WorkType_Name + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Worktype details added successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Worktype details added unsuccessfully";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DagetUpdatedWorktype(string user_gid, WorktypeLists values)
        {
            try
            {
                msSQL = " update hrm_mst_tworkertype set " +
                        " workertype_name = '" + values.WorkType_Name + "'," +

                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' " +
                        " where workertype_gid='" + values.Worktype_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Worktype Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Worktype";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteWorktype(string Worktype_gid, WorktypeLists values)
        {
            try
            {
                msSQL = "  delete from hrm_mst_tworkertype where workertype_gid='" + Worktype_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Worktype Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Worktype";
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