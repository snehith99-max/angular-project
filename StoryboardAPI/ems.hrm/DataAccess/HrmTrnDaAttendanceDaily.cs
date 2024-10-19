using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;

namespace ems.hrm.DataAccess
{

    public class HrmTrnDaAttendanceDaily
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid, msgetshift;
        // Module Master Summary
        public void DaGetDailySummary(MdlHrmTrnAttendanceDaily values, string date)
        {

            try
            {

                msSQL = " (select cast(case when (case when concat(x.shift_fromhours,':',x.shift_fromminutes,':00') is null then '00:00:00' " +
                    " else time(y.login_time) > addtime(case when length(e.grace_time)=1 then concat('00:','0',e.grace_time,':00') " +
                    " else concat('00:',e.grace_time,':00') end,concat(x.shift_fromhours,':',x.shift_fromminutes,':00')) end ) then " +
                    " (timediff(time(y.login_time),addtime(case when length(e.grace_time)=1 then concat('00:','0',e.grace_time,':00') " +
                    " else concat('00:',e.grace_time,':00') end,concat(x.shift_fromhours,':',x.shift_fromminutes,':00')))) " +
                    " else '--' end as char) as late_by " +
                    " from hrm_trn_temployee2shifttypedtl x " +
                    " inner join hrm_trn_tattendance y on x.employee_gid=y.employee_gid and x.employee2shifttypedtl_gid=y.employee2shifttypedtl_gid " +
                    " inner join hrm_mst_temployee z on y.employee_gid=z.employee_gid " +
                    " inner join hrm_mst_tbranch d on z.branch_gid=d.branch_gid " +
                    " inner join hrm_mst_tshifttype e on x.shifttype_gid=e.shifttype_gid " +
                    " where y.employee_gid = f.employee_gid " +
                    " and z.employee_gid = a.employee_gid and x.employee2shifttypedtl_gid=y.employee2shifttypedtl_gid " +
                    " and y.attendance_date='" + date + "'" +
                    " limit 1) as late_hours, " +
                    " (select concat(case when length(permission_totalhours)=1 then concat(0,permission_totalhours) " +
                                    " else permission_totalhours end,':', " +
                                    " case when length(total_mins)=1 then concat(0,total_mins) " +
                                    " else total_mins end,':00') from hrm_trn_tpermission p where a.employee_gid=p.employee_gid and " +
                   " p.permission_date='" + date + "') as permission_totalhours, " +
                    " (select " +
                    " cast(case when (case when time(b.logout_time) is null then '00:00:00' else  time(b.logout_time) end > " +
                    " concat(a.shift_tohours,':',a.shift_tominutes,':00')) then " +
                    " (timediff(time(b.logout_time), " +
                    " concat(a.shift_tohours,':',a.shift_tominutes,':00'))) else '--' end as char) as extra_time " +
                    " from hrm_trn_temployee2shifttypedtl a " +
                    " inner join hrm_trn_tattendance b on a.employee_gid=b.employee_gid " +
                    " where b.employee_gid = f.employee_gid and a.employee2shifttypedtl_gid=b.employee2shifttypedtl_gid " +
                    " and b.attendance_date='" + date + "' limit 1) as extra_hours, ";


                msSQL += " (select x.ip_name as login_ip from hrm_mst_tipmanage x " +
                         " left join hrm_trn_tattendance  f on x.ip_address=f.login_ip where  a.employee_gid=f.employee_gid and f.attendance_date='" + date + "' " +
                         " limit 1) as login_ip, " +
                         " (select g.ip_name  from hrm_mst_tipmanage g " +
                         " left join hrm_trn_tattendance  y on g.ip_address=y.logout_ip where a.employee_gid=y.employee_gid and y.attendance_date='" + date + "' " +
                         " limit 1) as logout_ip, " +
                         " concat(f.attendance_type,' ',case when f.day_session='NA' then '' else f.day_session end) as  attendance_status " +
                         " from hrm_mst_temployee a left join adm_mst_tuser b on a.user_gid=b.user_gid " +
                         " left join adm_mst_tdesignation c on a.designation_gid=c.designation_gid " +
                         " left join hrm_trn_temployee2shifttypedtl d on a.employee_gid=d.employee_gid " +
                         " left join hrm_mst_tshifttype e on d.shifttype_gid=e.shifttype_gid " +
                         " left join hrm_trn_tattendance f on a.employee_gid=f.employee_gid " +
                         " left join hrm_trn_tpermission g on a.employee_gid=g.employee_gid " +
                         " left join hrm_trn_temployeetypedtl j on a.employee_gid=j.employee_gid " +
                         " left join hrm_mst_tbranch k on k.branch_gid=a.branch_gid " +
                         " left join hrm_mst_tdepartment p on p.department_gid = a.department_gid " +
                         " left join hrm_mst_tsectionassign2employee q on q.employee_gid=a.employee_gid" +
                         " left join hrm_mst_tsection i on i.section_gid=q.section_gid" +
                         " left join hrm_mst_tblock z on z.block_gid=q.block_gid" +
                         " left join hrm_mst_tunit n on n.unit_gid=q.unit_gid" +
                         " where ((attendance_date='" + date + "' and exit_date is null) or (attendance_date='" + date + "' and exit_date>'" + date + "')) and d.shift_status='Y' and f.attendance_date='" + date + "' " +
                         " and a.attendance_flag='Y' and employee2shifttype_name= dayname(curdate()) " +
                         "and j.employeetype_name='Roll' " +
                         "order by length(b.user_code),b.user_code";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<daily_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new daily_list
                        {
                            late_hours = dt["late_hours"].ToString(),
                            permission_totalhours = dt["permission_totalhours"].ToString(),
                            extra_hours = dt["extra_hours"].ToString(),
                            attendance_status = dt["attendance_status"].ToString()
                        });
                        values.daily_list = getModuleList;
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
