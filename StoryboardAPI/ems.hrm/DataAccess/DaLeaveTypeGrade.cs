using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using MySql.Data.MySqlClient;


namespace ems.hrm.DataAccess
{
    public class DaLeaveTypeGrade
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid, msgetshift, lsleave_code;
        public void DaLeavetypeSummary(MdlLeaveTypeGrade values)
        {
            try
            {
                
                msSQL = " select a.leavetype_gid,concat(b.user_firstname,' ',b.user_lastname) as user_name, a.leavetype_code, a.leavetype_name, a.leavetype_count, a.consider_as, a.accrud," +
                " a.carryforward, a.leave_limit, a.created_by, a.created_date, a.updated_by,concat(c.user_firstname,' ',c.user_lastname) as updateduser_name, a.updated_date," +
                " case when leavetype_status ='Y' then 'ACTIVE' else 'INACTIVE' end as leavetypestatus from hrm_mst_tleavetype a" +
                " left join adm_mst_tuser b on a.created_by=b.user_gid" +
                " left join adm_mst_tuser c on  a.updated_by=c.user_gid order by leavetype_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Leavetype_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Leavetype_list
                        {
                            leavetype_gid = dt["leavetype_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            leavetype_code = dt["leavetype_code"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),
                            leavetype_count = dt["leavetype_count"].ToString(),
                            consider_as = dt["consider_as"].ToString(),
                            leavetypestatus = dt["leavetypestatus"].ToString(),

                        });
                        values.Leavetype_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void PostAddleave( Addleave_list values, string user_gid)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("LMAS");

                
                    if (values.Code_Generation == "N")
                {
                    {

                        msSQL = "select leavetype_code, leavetype_name from hrm_mst_tleavetype where leavetype_code='" + values.leave_code_manual + "'";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    }
                    if (objMySqlDataReader.HasRows == true)
                    {
                        values.status = false;
                        values.message = " The Leave Code Is Already Exist";
                    }
                    lsleave_code = values.leave_code_manual;
                }
                else
                {               
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='LMAS' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);
                    lsleave_code = "LC" + "000" + lsCode;
                }
                msSQL = "select leavetype_code, leavetype_name from hrm_mst_tleavetype where leavetype_name='" + values.leave_name + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = " The Leave Type Is Already Exist";
                }
                else
                {                    
                        msSQL = " insert into hrm_mst_tleavetype" +
                            "(" + " leavetype_gid," +
                            " leavetype_code," +
                            " leavetype_name, " +
                            "leavetype_status, " +
                            " consider_as," +
                            "weekoff_applicable," +
                            "holiday_applicable, " +
                            "carryforward," +
                            "accrud," +
                            "beyond_eligible, " +
                            "leave_days," +
                            "created_by," +
                            " created_date) values" +
                            " (" + " '" + msGetGid + "'," +
                            " '" + lsleave_code + "'," +                       
                            " '" + values.leave_name + "'," +
                            " '" + values.Status_flag + "'," +
                            " '" + values.Consider_as + "'," +
                            " '" + values.weekoff_consider + "'," +
                            " '" + values.holiday_consider + "'," +
                             " '" + values.carry_forward + "'," +
                             " '" + values.Accured_type + "'," +
                             " '" + values.negative_leave + "'," +
                             " '" + values.Leave_Days + "'," +
                             " '" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd  hh:mm:ss") + "')";
                    }

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = " Added leave Successfully";
                    }
                    else
                    {
                        values.message = "Error Occured While Adding";
                        values.status = false;
                    }
                }
            
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaDeleteLeaveType(string leavetype_gid, Addleave_list values)
        {
            try
            {
                msSQL = "  delete from hrm_mst_tleavetype where leavetype_gid='" + leavetype_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Leave Type Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Leave Type";
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

    }
}


    
