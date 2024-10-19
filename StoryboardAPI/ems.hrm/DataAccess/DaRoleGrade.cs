using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace ems.hrm.DataAccess
{
    public class DaRoleGrade
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_dataTable, objtbl, dt_datatable1;
        int mnResult;
        string msGetGid, lsgradelevel_code;

        public void DagetRoleGradeSummary(mdlrolegrade values)
       {
            try
            {
                msSQL = " select gradelevel_gid, gradelevel_code, gradelevel_name from hrm_mst_tgradelevel where 1=1 order by gradelevel_gid desc";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<RoleGradeLists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new RoleGradeLists
                        {
                            gradelevel_gid = dt["gradelevel_gid"].ToString(),
                            gradelevel_code = dt["gradelevel_code"].ToString(),
                            gradelevel_name = dt["gradelevel_name"].ToString(),
                        });
                        values.RoleGradeLists = getModuleList;
                    }
                }
                dt_dataTable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostRoleGrade(RoleGradeList values,string user_gid)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("HRGL");
                if (values.Code_Generation == "N")
                {

                    msSQL = "select gradelevel_code from hrm_mst_tgradelevel  where gradelevel_code = '" + values.role_code_manual + "'";
                    DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable1.Rows.Count > 0)
                    {
                        values.status = false;
                        values.message = "Role Grade Code already Exist";
                        return;
                    }
                    lsgradelevel_code = values.role_code_manual;
                }
                else
                {
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='HRGL' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    lsgradelevel_code = "RG" + "000" + lsCode;

                }
                if (msGetGid == "E")
                {
                    values.status = false;
                    values.message = "Rolegrade Gid Not genarated";
                    return;

                }
                else
                {

                    msSQL = " insert into hrm_mst_tgradelevel(" +
                            " gradelevel_gid," +
                            " gradelevel_code," +
                            " gradelevel_name," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + lsgradelevel_code + "', " +
                            "'" + values.RoleGradename + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Role Grade details added successfully";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Role Grade details added unsuccessfully";
                    }

                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DagetUpdatedRoleGrade(string user_gid, RoleGradeList values)
        {
            msSQL = " select gradelevel_name from  hrm_mst_tgradelevel where gradelevel_name = '" + values.RoleGradename + "' ";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

            msSQL = " update  hrm_mst_tgradelevel set " +
             " gradelevel_code = '" + values.rolegradecode + "'," +
             " gradelevel_name = '" + values.RoleGradename + "'," +
             " updated_by = '" + user_gid + "'," +
             " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where gradelevel_gid='" + values.gradelevel_gid + "'  ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)

            {
                values.status = true;
                values.message = "Role Grade Updated Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Role Grade";
            }
        }





        public void DaDeleteRoleGrade(string gradelevel_gid, RoleGradeList values)
        
        {
            msSQL = "  delete from hrm_mst_tgradelevel where gradelevel_gid='" + gradelevel_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Role Grade Deleted Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Deleting Role Grade";
                }
            }

        }
    }
}