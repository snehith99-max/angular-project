using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using System.Web.Security;
using System.Xml.Linq;
using System.Text;

namespace ems.hrm.DataAccess
{
    public class DaRoleDesignation
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
       
        OdbcDataReader objOdbcDataReader;
        DataTable dt_dataTable, dt_datatable1, dt_datatable2, objtbl;
        int mnResult;
        OdbcDataReader objMySqlDataReader;
        string msGetGid, lsdesignation_code;
        public void DagetRoleDesignationSummary(MdlRoleDesignation values)
        {
            try
            {
                msSQL = " select count(c.designation_gid) as count, a.designation_gid, a.role_gid," +                       
                        " role_name,designation_code, designation_name from  adm_mst_tdesignation a" +
                        " left join hrm_mst_trole b on a.role_gid=b.role_gid" +
                        " left join hrm_mst_temployee c on c.designation_gid=a.designation_gid" + " where 1=1  " +
                        " group by designation_gid order by designation_gid desc";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<RoleDesignationLists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new RoleDesignationLists
                        {
                            Role_Name = dt["role_name"].ToString(),
                            Designation_Code = dt["designation_code"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            Designation_Name = dt["designation_name"].ToString(),
                            TotalNoofEmployee = dt["count"].ToString(),
                        });
                        values.RoleDesignationLists = getModuleList;
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

        public void DaPostRoleDesignation(RoleDesignationLists values, string user_gid)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("SDGM");

                if (values.Code_Generation == "N")
                {

                    msSQL = "select designation_code from adm_mst_tdesignation  where designation_code = '" + values.Designation_code_manual + "'";
                    DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);


                    if (dt_datatable1.Rows.Count > 0)
                    {
                        values.status = false;
                        values.message = "Designation Code already Exist";
                        return;
                    }
                    lsdesignation_code = values.Designation_code_manual;
                }
                else
                {

                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='SDGM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    lsdesignation_code = "DC" + "000" + lsCode;

                }
                if (msGetGid == "E")
                {
                    values.status = false;
                    values.message = "Designation Gid Not genarated";
                    return;

                }
                else
                {
                    msSQL = " insert into adm_mst_tdesignation(" +
                        " designation_gid," +
                        " designation_code," +
                        " designation_name,created_by,created_date,role_gid)" +
                        " values(" +
                        "'" + msGetGid + "'," +
                        "'" + lsdesignation_code + "'," +
                        "'" + values.Designation_Name + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'," +
                        "'" + values.Role_Name + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Role Designation details added successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Role Designation details added unsuccessfully";
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
        public void DaGetRoleDesignationdropdown(MdlRoleDesignation values)
        {
            try
            {
                msSQL = "select role_gid, role_name from hrm_mst_trole where 1=1";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<rolelists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new rolelists
                        {
                            role_name = dt["role_name"].ToString(),
                            role_gid = dt["role_gid"].ToString(),

                        });
                        values.rolelists = getModuleList;
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
        public void DagetUpdatedRoleDesignation(string user_gid, RoleDesignationLists values)
        {
            msSQL = " Select designation_gid from adm_mst_tdesignation " +
                    " where designation_name = '" + values.Designation_Name + "' ";
          
            dt_dataTable = objdbconn.GetDataTable(msSQL);
            msSQL = " Select role_gid from hrm_mst_trole " +
                    " where role_name = '" + values.Role_Name + "' ";

            string lsrolegid = objdbconn.GetExecuteScalar(msSQL);
            msSQL = " update adm_mst_tdesignation set " +
                    " designation_code = '" + values.Designation_Code + "'," +
                    " designation_name = '" + values.Designation_Name + "'," +
                    " role_gid = '" + lsrolegid + "'," +
                    " updated_by = '" + user_gid + "'," +
                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' " +
                    " where designation_gid='" + values.designation_gid + "' ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)

            {
                values.status = true;
                values.message = "Role Designation Updated Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Role Designation";
            }
        }
        public void DaDeleteRoleDesignation(string designation_gid, RoleDesignationLists values)

        {
            msSQL = "  delete from adm_mst_tdesignation where designation_gid='" + designation_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Role Designation Deleted Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Deleting Role Designation";
                }
            }

        }
    }
}