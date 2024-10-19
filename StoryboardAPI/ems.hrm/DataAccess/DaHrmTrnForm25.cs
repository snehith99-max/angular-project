using System;
using System.Collections.Generic;
using System.Linq;
using ems.hrm.Models;
using ems.utilities.Functions;
using System.Data;
using System.Configuration;
using System.Data.Odbc;

using System.IO;
using System.Web;
using OfficeOpenXml;
using System.Data.OleDb;

using System.Text.RegularExpressions;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ems.hrm.DataAccess
{
    public class DaHrmTrnForm25
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        string lsmonth, lsyear;
        OdbcDataReader objOdbcDataReader;
        int mnResult, noofdays, daysinmonth1, salarystart_date, lsmonth_totaldays, salary_date, sal_year, exceedmonth, i, salmonth1, month_name,ls_year, ls_month;

        DataTable dt_datatable, objtbl;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        public void DaGetMusterSummary(MdlHrmTrnForm25 values)
        {
            try
            {
                msSQL = " select monthname(salary_startdate) as month_name,month(salary_startdate) as sal_month,year(salary_startdate) as sal_year  from adm_mst_tcompany ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {

                    if (objOdbcDataReader["sal_month"].ToString() is null)
                    {
                        salarystart_date = DateTime.Today.Month;
                        salary_date = DateTime.Today.Month;

                    }
                    else
                    {
                        string salarystartdate = objOdbcDataReader["sal_month"].ToString();
                        salarystart_date = int.Parse(salarystartdate);
                        salary_date = int.Parse(salarystartdate);

                    }
                    if (objOdbcDataReader["sal_year"].ToString() is null)
                    {
                        sal_year = DateTime.Today.Year;

                    }
                    else
                    {
                        string salaryyear = objOdbcDataReader["sal_year"].ToString();
                        sal_year = int.Parse(salaryyear);

                    }
                    int tomonth = DateTime.Now.Month;
                    if (salarystart_date > tomonth)
                    {
                        exceedmonth = salarystart_date;
                    }
                    else
                    {
                        exceedmonth = tomonth + 2;
                    }
                    {
                        msSQL = " truncate table pay_trn_tsalmonth ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1)
                    {
                        string msSQL = "INSERT INTO pay_trn_tsalmonth (month, sal_year) VALUES ";
                        for (int i = sal_year; i <= DateTime.Now.Year; i++)
                        {
                            for (int salmonth1 = 1; salmonth1 <= 12; salmonth1++)
                            {
                                if (i == DateTime.Now.Year &+salmonth1 > DateTime.Now.Month)
                                {
                                    break;
                                }

                                if (sal_year == i &+salmonth1 >= salarystart_date)
                                {
                                    string month_name = DateTimeFormatInfo.CurrentInfo.GetMonthName(salmonth1);
                                    msSQL += $"('{month_name}', '{i}'),";
                                }
                                else if (i == DateTime.Now.Year &+salmonth1 <= DateTime.Now.Month)
                                {
                                    if (i == sal_year &+salmonth1 >= salarystart_date &+salmonth1 <= DateTime.Now.Month)
                                    {
                                        msSQL += $"('{DateTimeFormatInfo.CurrentInfo.GetMonthName(salmonth1)}', '{i}'),";
                                    }
                                    else if (i != sal_year)
                                    {
                                        msSQL += $"('{DateTimeFormatInfo.CurrentInfo.GetMonthName(salmonth1)}', '{i}'),";
                                    }
                                }
                                else if (i != sal_year &+i != DateTime.Now.Year)
                                {
                                    msSQL += $"('{DateTimeFormatInfo.CurrentInfo.GetMonthName(salmonth1)}', '{i}'),";
                                }
                            }
                        }

                        msSQL = msSQL.TrimEnd(',');

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                }
                msSQL = "select monthname(str_to_date(substr(month,1,3),'%b'))as sal_month,sal_year from pay_trn_tsalmonth "+
                        " order by sal_year desc,MONTH(STR_TO_DATE(substring(sal_month,1,3),'%b')) desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<month_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new month_list
                        {

                           
                            month = dt["sal_month"].ToString(),
                            year = dt["sal_year"].ToString(),

                        });
                        values.month_list = getModuleList;
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
        public void DaGetDetailSummary(MdlHrmTrnForm25 values)
        {
            try
            {
                msSQL = " Select distinct '' as unit_gid,a.branch_gid,a.branch_name,c.department_gid,c.department_name from hrm_mst_tbranch a" +
                    " left join hrm_mst_temployee b on a.branch_gid=b.branch_gid" +
                    " left join hrm_mst_tdepartment c on b.department_gid=c.department_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<form253mployee_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new form253mployee_list
                        {


                            branch_gid = dt["branch_gid"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),

                        });
                        values.form253mployee_list = getModuleList;
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
        public Dictionary<string, object> DaGetForm25Rpt(string month, string year, string branch_gid,string department_gid,MdlHrmTrnForm25 values)
        {

             
            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();
            string lsday = "01";
        

            msSQL = " select month(str_to_date(month, '%M') ) as sal_month ,sal_year from pay_trn_tsalmonth where month ='" + month + "' and sal_year='" + year + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows == true)
            {
                lsmonth = objOdbcDataReader["sal_month"].ToString();
                lsyear = objOdbcDataReader["sal_year"].ToString();
                 ls_year = int.Parse(lsyear);
                 ls_month = int.Parse(lsmonth);
            }
            
             lsmonth_totaldays = System.DateTime.DaysInMonth(ls_year, ls_month);
            string lsmon = lsmonth.ToString();
            if (lsmon.Length == 1)
            {
                lsmon = "0" + lsmon;
            }
            else
            {
                // Do nothing, lsmon remains the same
            }
            string lsdaystr = lsday + "/" + lsmon + "/" + lsyear;
            DateTime lsdate2 = DateTime.ParseExact(lsdaystr.Replace("/", ""), "ddMMyyyy", null);
            DateTime lsdate = lsdate2;
            msSQL = " select /*+ MAX_EXECUTION_TIME(300000) */ distinct a.employee_gid,c.user_code,concat(c.user_firstname,' ' ,c.user_lastname) as user_name,br.branch_gid," +
                        " a.father_name,m.designation_name,date_format(a.employee_joiningdate,'%d-%m-%Y') as DOJ," +
                        " (select sum(if(halfdayabsent_flag='N',1,0.5)) as ss from hrm_trn_tattendance x where attendance_type='P' and month(attendance_date)='" + lsmon + "'" +
                        " and year(attendance_date)='" + lsyear + "' and x.employee_gid=a.employee_gid group by a.employee_gid) as attendance_count," +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then '0' end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid" +
                        " and x.attendance_date='" + lsdate.ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date1, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid  " +
                        " and x.attendance_date='" + lsdate.AddDays(1).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date2, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(2).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date3, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(3).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date4, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(4).ToString("yyyy-MM-dd") + "' group by a.employee_gid )   as date5, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(5).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date6, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH'))then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(6).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date7, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(7).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date8, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(8).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date9, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(9).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date10, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(10).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date11, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(11).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date12, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(12).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date13, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH'))then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid  " +
                        " and x.attendance_date='" + lsdate.AddDays(13).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date14, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(14).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date15, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(15).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date16, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(16).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date17, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(17).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date18, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(18).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date19, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(19).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date20, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(20).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date21, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(21).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date22, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(22).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date23, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where(a.employee_gid = x.employee_gid) " +
                        " and x.attendance_date='" + lsdate.AddDays(23).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date24, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(24).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date25, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(25).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date26, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(26).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date27, ";
            if (lsmonth_totaldays == 28)
            {
                msSQL += " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                            " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                            " where a.employee_gid = x.employee_gid " +
                            " and x.attendance_date='" + lsdate.AddDays(27).ToString("yyyy-MM-dd") + "' group by a.employee_gid )as date28 , ";
            }
            else if(lsmonth_totaldays == 29)
            {
                msSQL += " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                           " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                           " where a.employee_gid = x.employee_gid " +
                           " and x.attendance_date='" + lsdate.AddDays(27).ToString("yyyy-MM-dd") + "' group by a.employee_gid )as date28, " +
                           " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                           " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                           " where a.employee_gid = x.employee_gid " +
                           " and x.attendance_date='" + lsdate.AddDays(28).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date29 , ";
            }
            else if(lsmonth_totaldays == 30) {
                msSQL += " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                              " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                              " where a.employee_gid = x.employee_gid " +
                              " and x.attendance_date='" + lsdate.AddDays(27).ToString("yyyy-MM-dd") + "' group by a.employee_gid )as date28, " +
                              " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                              " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                              " where a.employee_gid = x.employee_gid " +
                              " and x.attendance_date='" + lsdate.AddDays(28).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date29, " +
                              " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                              " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                              " where a.employee_gid = x.employee_gid " +
                              " and x.attendance_date='" + lsdate.AddDays(29).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date30 ,";

            }
            else  if(lsmonth_totaldays==31){
                msSQL += " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(27).ToString("yyyy-MM-dd") + "' group by a.employee_gid )as date28, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(28).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date29, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(29).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date30, " +
                        " (select case when (x.attendance_type='P' and x.halfdayabsent_flag='N') then '1' when (x.attendance_type='P' and x.halfdayabsent_flag='Y') then '0.5' " +
                        " when ((x.attendance_type='A') or (x.attendance_type='WH') or (x.attendance_type='NH')) then 0 end as attendance_type from hrm_trn_tattendance x " +
                        " where a.employee_gid = x.employee_gid " +
                        " and x.attendance_date='" + lsdate.AddDays(30).ToString("yyyy-MM-dd") + "' group by a.employee_gid )  as date31 , ";

            }
            msSQL += " br.branch_name,n.department_name,' ' as unit_name,designation_name as section_name, " +
                                " substring(k.shifttype_name,1,1) as shifttype_name from hrm_mst_temployee a " +
                                " inner join adm_mst_tuser c on a.user_gid=c.user_gid " +
                                " inner join hrm_mst_tbranch br on a.branch_gid=br.branch_gid " +
                                " left join hrm_trn_temployee2shifttype j on a.employee_gid=j.employee_gid" +
                                " left join hrm_mst_tshifttype k on k.shifttype_gid=j.shifttype_gid" +
                                " left join adm_mst_tdesignation m on a.designation_gid=m.designation_gid " +
                                "left join hrm_mst_tdepartment n on a.department_gid=n.department_gid" +
                                 " where c.user_status='Y' and br.branch_gid='" + branch_gid + "' and n.department_gid='" + department_gid + "'" +
                                 " group by a.employee_gid order by a.employee_gid ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = " select month , sal_year as year from pay_trn_tsalmonth where month ='" + month + "' and sal_year='" +year + "'";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");

            msSQL = " select company_name,company_address,ind_labour_company from adm_mst_tcompany";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable3");





            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_hrm"].ToString(), "hrm_crp_form25musterregister.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Form25" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();

            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);

            return ls_response;


        }

    }
}
