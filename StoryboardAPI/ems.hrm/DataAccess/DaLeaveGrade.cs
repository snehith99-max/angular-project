using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace ems.hrm.DataAccess
{
    public class DaLeaveGrade
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string lsattendance_startdate, lsleavegrade_code, lsattendance_enddate, lsEmployee_gid, lsemployee_name, lsleavegrade_gid, msgetassign2employee_gid;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid, msgetshift , lsleavegrade_name, lsleavetype_gid, lsleavetype_name, lstotal_leavecount, lsavailable_leavecount, lsleave_limit;
        public void DaLeaveGradeSummary(MdlLeaveGrade values)
        {
            try
            {

                msSQL = " select  a.leavegrade_gid ,a.leavegrade_code, a.leavegrade_name ,c.leavetype_name , " +
               " format(sum(b.total_leavecount),2)as total_leavecount , format(sum(b.available_leavecount),2)as available_leavecount, " +
               " format(sum(b.leave_limit),2)as leave_limit from hrm_mst_tleavegrade a " +
               " left join hrm_mst_tleavegradedtl b on a.leavegrade_gid=b.leavegrade_gid  " +
               " left join hrm_mst_tleavetype c on c.leavetype_gid=b.leavetype_gid group by leavegrade_gid order by leavegrade_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leavegrade1_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leavegrade1_list
                        {
                            leavegrade_gid = dt["leavegrade_gid"].ToString(),
                            leavegrade_code = dt["leavegrade_code"].ToString(),
                            leavegrade_name = dt["leavegrade_name"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),
                            total_leavecount = dt["total_leavecount"].ToString(),
                            available_leavecount = dt["available_leavecount"].ToString(),
                            leave_limit = dt["leave_limit"].ToString(),

                        });
                        values.leavegrade_list = getModuleList;
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

        public void DaAssignEmployeeSummary(MdlLeaveGrade values)
        {
            try
            {

                msSQL = " Select distinct a.user_gid," +
                    " concat(a.user_code,' / ',a.user_firstname,' ',a.user_lastname) as user_name," +
                    " d.designation_name, c.designation_gid, c.employee_gid, e.branch_name,c.employee_gender,f.department_name," +
                    " c.department_gid, c.branch_gid " +
                    " FROM adm_mst_tuser a " +
                    " left join hrm_mst_temployee c on a.user_gid = c.user_gid" +
                    " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid" +
                    " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                    " left join hrm_mst_tdepartment f on c.department_gid = f.department_gid " +
                    " where c.employee_gid not in(select employee_gid from hrm_trn_tleavegrade2employee)" +
                    " and user_status='Y' group by c.employee_gid order by c.employee_gid asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<assignemployee_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new assignemployee_list
                        {
                            user_gid = dt["user_gid"].ToString(),
                            //user_code = dt["user_code"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            department_name = dt["department_name"].ToString(),


                        });
                        values.assign_employeelist = getModuleList;
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

        public void DaLeavegradeassign(MdlLeaveGrade values, string leavegrade_gid)
        {
            try
            {

                msSQL = " select leavegrade_code,leavegrade_name from hrm_mst_tleavegrade " +
                     " where leavegrade_gid ='" + leavegrade_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Leaveassign_type>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Leaveassign_type
                        {

                            leavegrade_code = dt["leavegrade_code"].ToString(),
                            leavegrade_name = dt["leavegrade_name"].ToString(),
                        });
                        values.Leaveassign_type = getModuleList;
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

        public void DaLeavegradeunassign(MdlLeaveGrade values, string leavegrade_gid)
        {
            try
            {

                msSQL = " select leavegrade_code,leavegrade_name from hrm_mst_tleavegrade " +
                    " where leavegrade_gid ='" + leavegrade_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Leaveunassign_type>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Leaveunassign_type
                        {

                            leavegrade_code = dt["leavegrade_code"].ToString(),
                            leavegrade_name = dt["leavegrade_name"].ToString(),
                        });
                        values.Leaveunassign_type = getModuleList;
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


      public void DaPostforunassign(assignsubmit_list values,string user_gid)
      {
            try
            {
                foreach (var data in values.assign_employeelist)
                {

                    msSQL = " select  c.leavetype_name,b.leavetype_gid,a.leavegrade_gid ,a.leavegrade_code, a.leavegrade_name ,c.leavetype_name ,a.attendance_startdate,a.attendance_enddate, " +
                        " format(sum(b.total_leavecount),2)as total_leavecount , format(sum(b.available_leavecount),2)as available_leavecount, " +
                        " format(sum(b.leave_limit),2)as leave_limit from hrm_mst_tleavegrade a " +
                        " left join hrm_mst_tleavegradedtl b on a.leavegrade_gid=b.leavegrade_gid  " +
                        " left join hrm_mst_tleavetype c on b.leavetype_gid=c.leavetype_gid where a.leavegrade_gid='" + values.leavegrade_gid + "' group by a.leavegrade_gid ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        lsleavegrade_code = objMySqlDataReader["leavegrade_code"].ToString();
                        lsleavegrade_name = objMySqlDataReader["leavegrade_name"].ToString();
                        lsattendance_startdate = objMySqlDataReader["attendance_startdate"].ToString();
                        lsattendance_enddate = objMySqlDataReader["attendance_enddate"].ToString();
                        lsleavetype_gid = objMySqlDataReader["leavetype_gid"].ToString();
                        lsleavetype_name = objMySqlDataReader["leavetype_name"].ToString();
                        lstotal_leavecount = objMySqlDataReader["total_leavecount"].ToString();
                        lsavailable_leavecount = objMySqlDataReader["available_leavecount"].ToString();
                        lsleave_limit = objMySqlDataReader["leave_limit"].ToString();

                        msgetassign2employee_gid = objcmnfunctions.GetMasterGID("LE2G");

                        msSQL = " insert into hrm_trn_tleavegrade2employee ( " +
                                    " leavegrade2employee_gid," +
                                    " employee_gid," +
                                    " employee_name," +
                                    " leavegrade_gid," +
                                    " leavegrade_code," +
                                    " leavegrade_name, " +
                                    " attendance_startdate, " +
                                    " attendance_enddate, " +
                                    " total_leavecount, " +
                                    " available_leavecount, " +
                                    " leave_limit " +
                                    " ) Values ( " +
                                    " '" + msgetassign2employee_gid + "', " +
                                    " '" + data.employee_gid + "', " +
                                    " '" + lsemployee_name + "', " +
                                    " '" + values.leavegrade_gid + "', " +
                                    " '" + lsleavegrade_code + "'," +
                                    " '" + lsleavegrade_name + "'," +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " '" + lstotal_leavecount + "'," +
                                    " '" + lsavailable_leavecount + "'," +
                                    " '" + lsleave_limit + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Employee Assigned Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Assigned Employee";
                        }
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

        public void DaUnassignEmployeeSummary( string leavegrade_gid, MdlLeaveGrade values)
        {
            try
            {

                msSQL = " Select distinct a.user_gid," +
                    " concat(a.user_code,' / ',a.user_firstname,' ',a.user_lastname) as user_name," +
                    " d.designation_name, c.designation_gid, c.employee_gid, e.branch_name,c.employee_gender,f.department_name," +
                    " c.department_gid, c.branch_gid " +
                    " FROM adm_mst_tuser a " +
                    " left join hrm_mst_temployee c on a.user_gid = c.user_gid" +
                    " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid" +
                    " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                    " left join hrm_mst_tdepartment f on c.department_gid = f.department_gid " +
                    " where c.employee_gid in(select employee_gid from hrm_trn_tleavegrade2employee " +
                    " where leavegrade_gid = '" + leavegrade_gid + "')" +
                    " and user_status='Y' group by c.employee_gid order by c.employee_gid asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<unassignemployee_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new unassignemployee_list
                        {
                            user_gid = dt["user_gid"].ToString(),
                            //user_code = dt["user_code"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            department_name = dt["department_name"].ToString(),


                        });
                        values.unassign_employeelist = getModuleList;
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

        public void DaDeleteUnassignemployee(unassignsubmit_list values, string user_gid)
        {
            try
            {
                foreach (var data in values.unassign_employeelist)
                {

                    msSQL = " delete from hrm_trn_tleavegrade2employee where employee_gid='" + data.employee_gid + "' " +
                        " and leavegrade_gid='" + values.leavegrade_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Employee Unassigned Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Employee Unassigned";
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

        public void DaGetleavegradecodesummary(leavegradesubmit_list values)
        {
            try
            {
                 
                msSQL = "select leavetype_gid,leavetype_code,leavetype_name from hrm_mst_tleavetype";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leavegradecode_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leavegradecode_list
                        {
                            leavetype_gid = dt["leavetype_gid"].ToString(),
                            leavetype_code = dt["leavetype_code"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),

                        });
                        values.leavegradecode_list = getModuleList;
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

        //public void DaLeaveGradeSubmit( leavegradesubmit_list values)
        //{
        //    try
        //    {
        //        msGetGid = objcmnfunctions.GetMasterGID("LEGD");

        //        if (values.Code_Generation == "N")
        //        {
        //                msSQL = "select leavegrade_code from hrm_mst_tleavegrade  where leavegrade_code = '" + values.leavegrade_code_manual + "'";
        //            DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

        //            if (dt_datatable1.Rows.Count > 0)
        //            {
        //                values.status = false;
        //                values.message = "Leave Grade already Exist";
        //                return;
        //            }
        //            lsleavegrade_code = values.leavegrade_code_manual;
        //        }

        //        msSQL = " SELECT leavegrade_name  FROM " +
        //                " hrm_mst_tleavegrade WHERE leavegrade_name = '" + values.leavegrade_name + "'";

        //        DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);
        //        if (dt_datatable2.Rows.Count > 0)
        //        {
        //            values.status = false;
        //            values.message = "Leave Grade Name already Exist";
        //            return;
        //        }
        //        else
        //        {
        //                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='LEGD' order by finyear desc limit 0,1 ";
        //                string lsCode = objdbconn.GetExecuteScalar(msSQL);

        //                lsleavegrade_code = "LG" + "000" + lsCode;

        //        }
        //            msSQL = "select attendance_startdate,attendance_enddate from adm_mst_Tcompany ";
        //        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
        //        if (objMySqlDataReader.HasRows == true)
        //        {
        //            lsattendance_startdate = objMySqlDataReader["attendance_startdate"].ToString();
        //            lsattendance_enddate = objMySqlDataReader["attendance_enddate"].ToString();
        //        }
        //        msSQL = " insert into hrm_mst_tleavegrade ( " +
        //            " leavegrade_gid, " +
        //            " leavegrade_code, " +
        //            " leavegrade_name," +
        //            " attendance_startdate," +
        //            " attendance_enddate) " +
        //           " values (" +
        //            "'" + msGetGid + "', "+
        //             "'" + lsleavegrade_code + "', "+
        //             "'" + values.leavegrade_name + "'," +
        //            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
        //            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
        //        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //        foreach (var data in values.leavegradecode_list)
        //        {
        //            //msGetGid1 = objcmnfunctions.GetMasterGID("LE2G");
        //            msSQL = " insert into hrm_mst_tleavegradedtl ( " +
        //           " leavegradedtl_gid, " +
        //           " leavegrade_gid, " +
        //           " leavetype_gid, " +
        //           " total_leavecount," +
        //           " available_leavecount," +
        //           " leave_limit) " +
        //           " values (" +
        //            "'" + msGetGid1 + "', " +
        //            "'" + msGetGid + "', " +
        //            "'" + data.leavetype_gid + "', " +
        //            "'" + data.total_leavecount + "'," +
        //            "'" + data.available_leavecount + "'," +
        //            "'" + data.leave_limit + "')";
        //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //        }
        //        if (mnResult == 1)
        //        {
        //            values.status = true;
        //            values.message = "Leave Grade Added Sucessfully";
        //        }
        //        else
        //        {
        //            values.status = false;
        //            values.message = "Error While Adding Leave Grade";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.status = false;

        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
        //         "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }
        //}
        public void DaLeaveGradeSubmit(leavegradesubmit_list values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("LEGD");
                msSQL = " SELECT leavegrade_name  FROM " +
                       " hrm_mst_tleavegrade WHERE leavegrade_name = '" + values.leavegrade_name + "'";
                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Leave Grade Name already Exist";
                    return;
                }
                if (values.Code_Generation == "N")
                {
                    msSQL = "select leavegrade_code from hrm_mst_tleavegrade  where leavegrade_code = '" + values.leavegrade_code_manual + "'";
                    DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable1.Rows.Count > 0)
                    {
                        values.status = false;
                        values.message = "Leave Grade already Exist";
                        return;
                    }
                    lsleavegrade_code = values.leavegrade_code_manual;
                }                              
                else
                {
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='LEGD' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    lsleavegrade_code = "LG" + "000" + lsCode;

                }
                msSQL = "select attendance_startdate,attendance_enddate from adm_mst_Tcompany ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lsattendance_startdate = objMySqlDataReader["attendance_startdate"].ToString();
                    lsattendance_enddate = objMySqlDataReader["attendance_enddate"].ToString();
                }
                if (msGetGid == "E")
                {
                    values.status = false;
                    values.message = " leavegrade_gid Not genarated";
                    return;
                }              
                msSQL = " insert into hrm_mst_tleavegrade ( " +
                        " leavegrade_gid, " +
                        " leavegrade_code, " +
                        " leavegrade_name," +
                        " attendance_startdate," +
                        " attendance_enddate) " +
                        " values (" +
                        "'" + msGetGid + "', " +
                        "'" + lsleavegrade_code + "', " +
                        "'" + values.leavegrade_name + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                foreach (var data in values.leavegradecode_list)
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("LE2G");
                    msSQL = " insert into hrm_mst_tleavegradedtl ( " +
                            " leavegradedtl_gid, " +
                            " leavegrade_gid, " +
                            " leavetype_gid, " +
                            " total_leavecount," +
                            " available_leavecount," +
                            " leave_limit) " +
                            " values (" +
                            "'" + msGetGid1 + "', " +
                            "'" + msGetGid + "', " +
                            "'" + data.leavetype_gid + "', " +
                            "'" + data.total_leavecount + "'," +
                            "'" + data.available_leavecount + "'," +
                            "'" + data.leave_limit + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Leave Grade Added Sucessfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Leave Grade";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGetleavetypepopup(string leavegrade_gid, MdlLeaveGrade values)
        {

            try
            {

                msSQL = " select a.leavetype_gid,c.leavetype_name,a.total_leavecount,a.available_leavecount,a.leave_limit from hrm_mst_tleavegradedtl a " +
                " left join hrm_mst_tleavegrade b on a.leavegrade_gid=b.leavegrade_gid " +
                " left join hrm_mst_tleavetype c on a.leavetype_gid=c.leavetype_gid where a.leavegrade_gid='" + leavegrade_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Leave_typepopup>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Leave_typepopup
                        {

                            leavetype_gid = dt["leavetype_gid"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),
                            total_leavecount = dt["total_leavecount"].ToString(),
                            available_leavecount = dt["available_leavecount"].ToString(),
                            leave_limit = dt["leave_limit"].ToString()
                           
                        });
                        values.Leave_typepopup = getModuleList;
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

    }
}