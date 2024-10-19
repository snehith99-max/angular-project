using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using static ems.hrm.Models.MdlHrmMstConfig;

namespace ems.hrm.DataAccess
{
    public class DaHrmMstConfig
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objMySqlDataReader;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaPostHrmconfig(string employee_gid, hrmconfiglist values)
        {
            try
            {
                msSQL = " Update hrm_mst_tattendanceconfig set  " +
                        " totalshift_hours= '" + values.totalshifthours + "'," +
                        " halfmin_hours='" + values.halfmineligiblehours + "'," +
                        " halfmax_hours='" + values.halfmaxeligiblehours + "'," +
                        " weekoff_salary='" + values.weekoff_salary + "'," +
                        " holiday_salary='" + values.holiday_salary + "'," +
                        " totalpermission_hours='" + values.totalpermissionallowed + "'," +
                        " otminhours='" + values.otminhours + "'," +
                        " otmaxhours='" + values.otmaxhours + "'," +
                        " otavailed_flag='" + values.otavailed + "'," +
                        " attendance_allowanceflag='"+ values.attendance_allowance_flag +"'," +
                        " allowed_leave='"+ values.allowed_leave +"'," +
                        " allowance_amount='"+ values.allowance_amount +"'," +
                        " updated_by='" + employee_gid + "'," +
                        " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                   
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = " Attendance Configuration Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while Adding Attendance Configuration";
                }
            }

            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/hrm/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public hrmconfiglist DaGetAttendanceConfiguration()
        {
            try
            {
                hrmconfiglist objhrmconfiglist = new hrmconfiglist();
                {
                    msSQL = "select totalshift_hours, halfmin_hours,halfmax_hours, format(weekoff_salary,2) as weekoff_salary," +
                            " format(holiday_salary, 2) as holiday_salary, totalpermission_hours, otminhours, otmaxhours, " +
                            " otavailed_flag,attendance_allowanceflag,allowed_leave,allowance_amount " +
                            " from hrm_mst_tattendanceconfig ";
                }

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();
                    objhrmconfiglist.totalshifthours = objMySqlDataReader["totalshift_hours"].ToString();
                    objhrmconfiglist.halfmineligiblehours = objMySqlDataReader["halfmin_hours"].ToString();
                    objhrmconfiglist.halfmaxeligiblehours = objMySqlDataReader["halfmax_hours"].ToString();
                    objhrmconfiglist.weekoff_salary = objMySqlDataReader["weekoff_salary"].ToString();
                    objhrmconfiglist.holiday_salary = objMySqlDataReader["holiday_salary"].ToString();
                    objhrmconfiglist.totalpermissionallowed = objMySqlDataReader["totalpermission_hours"].ToString();
                    objhrmconfiglist.otminhours = objMySqlDataReader["otminhours"].ToString();
                    objhrmconfiglist.otmaxhours = objMySqlDataReader["otmaxhours"].ToString();
                    objhrmconfiglist.otavailed = objMySqlDataReader["otavailed_flag"].ToString();
                    objhrmconfiglist.attendance_allowance_flag = objMySqlDataReader["attendance_allowanceflag"].ToString();
                    objhrmconfiglist.allowed_leave = objMySqlDataReader["allowed_leave"].ToString();
                    objhrmconfiglist.allowance_amount = objMySqlDataReader["allowance_amount"].ToString();

                    objMySqlDataReader.Close();
                }
                return objhrmconfiglist;
            }
            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/hrm/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }
    }
}