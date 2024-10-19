using ems.payroll.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Globalization;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ems.payroll.DataAccess
{
    public class DaPayTrnSalaryManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msSQL1, msSQL2, lspayrun_flag = string.Empty;

        OdbcDataReader objOdbcDataReader, objOdbcDataReader1, objOdbcDataReader2, objOdbcDataReader3, objODBCDataReader;
        DataTable dt_datatable, dt_datatable2;
        int mnResult, noofdays, daysinmonth1, salarystart_date, salary_date, sal_year, exceedmonth, i, salmonth1, month_name, monthnum;
        string lsbranch_name, lsbranch_gid, lsdepartment_name, lsdepartment_gid, lsdesignation_name, lsdesignation_gid;
        string msGetGid, msGetGid1, msGetDlGID, msgetsalary_gid, lsempoyeegid, start_month, start_year, end_month, end_year, exit_date, days1;
        string lsflag, ls_gid, lssalarycomponent_gid, lscomponentgroup_gid, lsgeneratedby, lsmonthworkingdays, lsearnednetsalary, lsnetsalary;
        string lsstatutory_flag, payrunyear, payrunmonth;
        double earn_salarycomponent_amount, earned_salarycomponent_amount_employer;
        double lsdeduction, lsaddition, statutory_amount, lsadvance;
        finance_cmnfunction objfinance_cmnfunction = new finance_cmnfunction();
        string lsstart_date, lsend_date;

        // Module Summary
        public void DaEmployeeSalaryManagement(MdlPayTrnSalaryManagement values)
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
                                if (i == DateTime.Now.Year && salmonth1 > DateTime.Now.Month)
                                {
                                    break;
                                }

                                if (sal_year == i && salmonth1 >= salarystart_date)
                                {
                                    string month_name = DateTimeFormatInfo.CurrentInfo.GetMonthName(salmonth1);
                                    msSQL += $"('{month_name}', '{i}'),";
                                }
                                else if (i == DateTime.Now.Year && salmonth1 <= DateTime.Now.Month)
                                {
                                    if (i == sal_year && salmonth1 >= salarystart_date && salmonth1 <= DateTime.Now.Month)
                                    {
                                        msSQL += $"('{DateTimeFormatInfo.CurrentInfo.GetMonthName(salmonth1)}', '{i}'),";
                                    }
                                    else if (i != sal_year)
                                    {
                                        msSQL += $"('{DateTimeFormatInfo.CurrentInfo.GetMonthName(salmonth1)}', '{i}'),";
                                    }
                                }
                                else if (i != sal_year && i != DateTime.Now.Year)
                                {
                                    msSQL += $"('{DateTimeFormatInfo.CurrentInfo.GetMonthName(salmonth1)}', '{i}'),";
                                }
                            }
                        }

                        msSQL = msSQL.TrimEnd(',');

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                }
                objOdbcDataReader.Close();

                msSQL = " select a.salary_gid, c.month, c.sal_year, b.user_gid,concat(b.user_firstname,'-',b.user_lastname) as user_name, " +
                        " a.leave_taken, a.lop_days as lop, a.month_workingdays,format(sum(a.earned_net_salary),2) as earned_net_salary ,format(sum(a.net_salary),2) as net_salary, a.generated_by, a.generated_on ,count(a.employee_gid) as totalemployee" +
                         " from pay_trn_tsalmonth c " +
                          " left join pay_trn_tsalary a on a.month=c.month and a.year=c.sal_year " +
                         " left join hrm_mst_temployee d on a.employee_gid=d.employee_gid " +
                         " left join adm_mst_tuser b on a.generated_by=b.user_gid group by c.month,c.sal_year  " +
                         " order by c.sal_year desc,MONTH(STR_TO_DATE(substring(c.month,1,3),'%b')) desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<employeesalary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        if (dt["user_name"].ToString() == "")
                        {
                            lsgeneratedby = "Not Generated";
                        }
                        else
                        {
                            lsgeneratedby = dt["user_name"].ToString();
                        }

                        if (dt["month_workingdays"].ToString() == "")
                        {
                            lsmonthworkingdays = "Not Generated";
                        }
                        else
                        {
                            lsmonthworkingdays = dt["month_workingdays"].ToString();
                        }

                        if (dt["net_salary"].ToString() == "")
                        {
                            lsnetsalary = "Not Generated";
                        }
                        else
                        {
                            lsnetsalary = dt["net_salary"].ToString();
                        }

                        if (dt["earned_net_salary"].ToString() == "")
                        {
                            lsearnednetsalary = "Not Generated";
                        }
                        else
                        {
                            lsearnednetsalary = dt["earned_net_salary"].ToString();
                        }

                        getModuleList.Add(new employeesalary_list
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            month = dt["month"].ToString(),
                            year = dt["sal_year"].ToString(),
                            Workingdays = lsmonthworkingdays,
                            generated_by = lsgeneratedby,
                            totalemployee = dt["totalemployee"].ToString(),
                            net_salary = lsnetsalary,
                            earned_net_salary = lsearnednetsalary,


                        });
                        values.employeesalarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Employee Salary list!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaAdditionalsubsummary(string salary_gid, MdlPayTrnSalaryManagement values)
        {
            try
            {

                msSQL = " select a.salary_gid,(b.salarycomponent_name) as salarycomponent_name,b.salarycomponent_percentage," +
                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                            "  from pay_trn_tsalary a" +
                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                            " where a.salary_gid ='" + salary_gid + "' and b.salarygradetype='Addition' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addsummary1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addsummary1
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                        });
                        values.addsummary1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Sub summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaDeductsubsummary(string salary_gid, MdlPayTrnSalaryManagement values)
        {
            try
            {

                msSQL = " select a.salary_gid,(b.salarycomponent_name) as salarycomponent_name,b.salarycomponent_percentage," +
                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                            "  from pay_trn_tsalary a" +
                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                            " where a.salary_gid ='" + salary_gid + "' and b.salarygradetype='Deduction' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<deductsummary1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new deductsummary1
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                        });
                        values.deductsummary1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Add Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void Daothersummary(string salary_gid, MdlPayTrnSalaryManagement values)
        {
            try
            {

                msSQL = " select a.salary_gid,(b.salarycomponent_name) as salarycomponent_name,b.salarycomponent_percentage," +
                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                            "  from pay_trn_tsalary a" +
                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                            " where a.salary_gid ='" + salary_gid + "' and b.salarygradetype='Others' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<otherssummary1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new otherssummary1
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                        });
                        values.otherssummary1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Add Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetEmployeeSelect(string month, string year, MdlPayTrnSalaryManagement values)
        {
            try
            {

                int monthNumber = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month;

                // SQL query to select data from the database
                string msSQL = "SELECT start_day, end_day, month_interval FROM pay_mst_tpayrollconfig";

                // Execute the SQL query and retrieve data
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    DataRow objrow = dt_datatable1.Rows[0];
                    string startDay = objrow["start_day"].ToString();
                    string endDay = objrow["end_day"].ToString();
                    string monthInterval = objrow["month_interval"].ToString();

                    string lsinterval = monthInterval;
                    string lsstartday = startDay;
                    string lsendday = endDay;

                    if (lsinterval == "current")
                    {
                        start_month = monthNumber.ToString();
                        if (start_month.Length == 1)
                        {
                            start_month = "0" + start_month;
                        }
                        end_month = start_month;
                        start_year = year;
                        end_year = year;

                        int totaldays1 = DateTime.DaysInMonth(int.Parse(start_year), int.Parse(start_month));

                        if (int.Parse(lsendday) > totaldays1)
                        {
                            lsendday = totaldays1.ToString();
                        }
                    }
                    else
                    {
                        end_month = monthNumber.ToString();
                        if (end_month.Length == 1)
                        {
                            end_month = "0" + end_month;
                        }
                        string end_year = year;

                        // -----------------------to get start date-------------------'
                        string lsstring = lsendday + "-" + end_month + "-" + end_year;
                        DateTime tempdate = DateTime.ParseExact(lsstring.Replace("-", ""), "ddMMyyyy", null);
                        tempdate = tempdate.AddMonths(-1);
                        start_year = tempdate.Year.ToString();
                        start_month = tempdate.Month.ToString();

                        if (start_month.Length == 1)
                        {
                            start_month = "0" + start_month;
                        }
                    }

                    string lsstartdate = lsstartday + "-" + start_month + "-" + start_year;
                    string lsenddate = lsendday + "-" + end_month + "-" + end_year;

                    DateTime startdate = DateTime.ParseExact(lsstartdate.Replace("-", ""), "ddMMyyyy", null);
                    DateTime enddate = DateTime.ParseExact(lsenddate.Replace("-", ""), "ddMMyyyy", null);

                    int totaldays = (int)(enddate - startdate).TotalDays;
                    totaldays = totaldays + 1;
                    noofdays = totaldays;



                    msSQL = "Select /*+ MAX_EXECUTION_TIME(300000) */ distinct b.employee_gid,a.user_code,concat(ifnull(a.user_firstname,''),' ',ifnull(a.user_lastname,'')) as employee_name, " +
                            "d.designation_name ,c.employee_gid,e.branch_name,MONTH(STR_TO_DATE(substring(monthname(c.employee_joiningdate),1,3),'%b')) as joiningmonth_number," +
                            "c.department_gid, c.branch_gid, g.department_name " +
                            "FROM pay_trn_temployee2salarygradetemplate b " +
                            "left join  pay_trn_temployee2salarygradetemplatedtl x on b.employee2salarygradetemplate_gid=x.employee2salarygradetemplate_gid " +
                            "inner join hrm_mst_temployee c on b.employee_gid = c.employee_gid " +
                            "inner join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                            "inner join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                            "inner join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                            "left join hrm_trn_temployeetypedtl h on c.employee_gid=h.employee_gid " +
                            "inner join adm_mst_tuser a on c.user_gid=a.user_gid  " +
                            "left join hrm_trn_texitemployee y on c.employee_gid=y.employee_gid " +
                            "left join  pay_trn_temployeepayrun z on c.employee_gid=z.employee_gid " +
                            "where 1=1  and b.employee_gid not in( select employee_gid from pay_mst_tassignemployee2wages) " +
                            " and b.employee_gid not in(select employee_gid from pay_mst_ttailormaster) and " +
                            " b.employee_gid not in( select employee_gid from pay_mst_tassignemployee2nonmanagement)  and ((c.exit_date>='" + startdate.ToString("yyyy-MM-dd") + "' and c.exit_date<='" + enddate.ToString("yyyy-MM-dd") + "') or c.exit_date is null) " +
                            "  and c.employee_joiningdate<='" + enddate.ToString("yyyy-MM-dd") + "'   and c.employee_gid not in (select e.employee_gid from " +
                            " pay_trn_temployeepayrun e where e.month='" + month + "' " +
                            " and e.year='" + year + "' and e.employee_select='Y')  " +
                            " or c.employee_gid in ( select v.employee_gid from hrm_mst_temployee v " +
                            " where v.exit_date>'" + enddate.ToString("yyyy-MM-dd") + "')  group by  b.employee_gid order by e.branch_name,a.user_code,b.basic_salary asc ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetEmployeeSelect>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetEmployeeSelect
                            {
                                employee_gid = dt["employee_gid"].ToString(),
                                user_code = dt["user_code"].ToString(),
                                employee_name = dt["employee_name"].ToString(),
                                designation_name = dt["designation_name"].ToString(),
                                branch_name = dt["branch_name"].ToString(),
                                joiningmonth_number = dt["joiningmonth_number"].ToString(),
                                department_gid = dt["department_gid"].ToString(),
                                branch_gid = dt["branch_gid"].ToString(),
                                department_name = dt["department_name"].ToString(),
                            });
                            values.GetEmployeeSelect = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();


                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Employee Select!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetpayrunsummary(string month, string year, MdlPayTrnSalaryManagement values)
        {
            try
            {

                msSQL = " select a.salary_gid, a.employee_gid,c.user_code,concat(c.user_firstname,' ',c.user_lastname)as employee_name,b.branch_gid,a.month,a.year," +
                    " d.branch_name,e.department_name as department,a.leave_taken,a.lop_days as lop,a.leave_salary as leave_wage,a.ot_hours,a.ot_rate,f.designation_name, " +
                    " format(a.basic_salary,2)as basic_salary , format(a.earned_basic_salary,2)as earned_basic_salary ,b.user_gid," +
                    " format(a.gross_salary,2)as gross_salary , format(a.earned_gross_salary,2)as earned_gross_salary ,a.public_holidays,round(a.permission_wage) as permission_wage, " +
                    " format(a.net_salary,2)As net_salary , format(a.earned_net_salary,2)As earned_net_salary , a.actual_month_workingdays,a.month_workingdays," +
                    " ifnull(loanadvance_amount,0) as loanadvance_amount,attendance_allowance from pay_trn_tsalary a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " left join hrm_mst_tbranch d on b.branch_gid=d.branch_gid" +
                    " left join hrm_mst_tdepartment e on b.department_gid=e.department_gid" +
                    " left join adm_mst_tdesignation f on b.designation_gid=f.designation_gid " +
                    " where  a.month='" + month + "' and a.year='" + year + "' and a.payrun_flag='Y'  " +
                    "  group by a.salary_gid order by b.employee_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<payrunviewlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new payrunviewlist
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            month = dt["month"].ToString(),
                            year = dt["year"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department = dt["department"].ToString(),
                            leave_taken = dt["leave_taken"].ToString(),
                            lop = dt["lop"].ToString(),
                            leave_wage = dt["leave_wage"].ToString(),
                            ot_hours = dt["ot_hours"].ToString(),
                            ot_rate = dt["ot_rate"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            basic_salary = dt["basic_salary"].ToString(),
                            earned_basic_salary = dt["earned_basic_salary"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                            earned_gross_salary = dt["earned_gross_salary"].ToString(),
                            public_holidays = dt["public_holidays"].ToString(),
                            permission_wage = dt["permission_wage"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            earned_net_salary = dt["earned_net_salary"].ToString(),
                            actual_month_workingdays = dt["actual_month_workingdays"].ToString(),
                            month_workingdays = dt["month_workingdays"].ToString(),
                            loanadvance_amount = dt["loanadvance_amount"].ToString(),
                            attendance_allowance = dt["attendance_allowance"].ToString(),
                        });
                        values.payrunviewlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payrun View!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostforpayrun(GetEmployeelist values)
        {
            try
            {

                foreach (var data in values.detailsdtl_list)
                {
                    msSQL = " select exit_date from hrm_trn_texitemployee where monthname(exit_date)= '" + values.month + "'  " +
                        " and year(exit_date)= '" + values.year + "' and employee_gid= '" + data.employee_gid + "'  ";

                    DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable1.Rows.Count > 0)
                    {
                        DataRow objrow = dt_datatable1.Rows[0];
                        exit_date = objrow["exit_date"].ToString();

                        msSQL = "select * from hrm_trn_tattendance where attendance_date >'" + exit_date + "' " +
                                " and employee_gid='" + data.employee_gid + "' ";
                        dt_datatable2 = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable2.Rows.Count > 0)
                        {
                            msSQL = " delete from hrm_trn_tattendance where employee_gid='" + data.employee_gid + "' and attendance_date>'" + exit_date + "' ";

                        }
                    }

                    msSQL = "select * from pay_trn_temployeepayrun where employee_gid='" + data.employee_gid + "' and month='" + values.month + "' and year='" + values.year + "' ";
                    DataTable dt_datatable3 = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable3.Rows.Count > 0)
                    {
                        msSQL = " update pay_trn_temployeepayrun set month_workingdays='" + noofdays + "',employee_select='Y' where employee_gid='" + data.employee_gid + "' " +
                                " and month = '" + values.month + "' and year='" + values.year + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("EMPP");

                        msSQL = " insert into pay_trn_temployeepayrun ( " +
                                    " employeepayrun_gid, " +
                                    " employee_gid, " +
                                    " month, " +
                                    " year, " +
                                    " month_workingdays, " +
                                    " employee_select, " +
                                    " employee_source " +
                                    " ) Values ( " +
                              " '" + msGetGid + "'," +
                              " '" + data.employee_gid + "'," +
                              " '" + values.month + "'," +
                              " '" + values.year + "'," +
                              " '" + noofdays + "'," +
                              " '" + "Y" + "'," +
                              " '" + "Employee_select" + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Employee Generated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While selecting employee";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Selecting Employee!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaUpdatemonthlypayrun(string user_gid, string branch_gid, Getmonthlypayrun values)
        {
            try
            {

                msSQL = " select salary_gid from pay_trn_tsalary where month='" + values.month + "' and year='" + values.year + "' group by generated_on ";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                double refno = objOdbcDataReader.RecordsAffected;

                string journal_refno = "00" + refno + 1;

                objOdbcDataReader.Close();



                int monthNumber = DateTime.ParseExact(values.month, "MMMM", CultureInfo.CurrentCulture).Month;

                // SQL query to select data from the database
                msSQL = "SELECT start_day, end_day, month_interval FROM pay_mst_tpayrollconfig";

                // Execute the SQL query and retrieve data
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    DataRow objrow = dt_datatable1.Rows[0];
                    string startDay = objrow["start_day"].ToString();
                    string endDay = objrow["end_day"].ToString();
                    string monthInterval = objrow["month_interval"].ToString();

                    string lsinterval = monthInterval;
                    string lsstartday = startDay;
                    string lsendday = endDay;

                    if (lsinterval == "current")
                    {
                        start_month = monthNumber.ToString();
                        if (start_month.Length == 1)
                        {
                            start_month = "0" + start_month;
                        }
                        end_month = start_month;
                        start_year = values.year;
                        end_year = values.year;

                        int totaldays1 = DateTime.DaysInMonth(int.Parse(start_year), int.Parse(start_month));

                        if (int.Parse(lsendday) > totaldays1)
                        {
                            lsendday = totaldays1.ToString();
                        }
                    }
                    else
                    {
                        end_month = monthNumber.ToString();
                        if (end_month.Length == 1)
                        {
                            end_month = "0" + end_month;
                        }
                        string end_year = values.year;

                        // -----------------------to get start date-------------------'
                        string lsstring = lsendday + "-" + end_month + "-" + end_year;
                        DateTime tempdate = DateTime.ParseExact(lsstring.Replace("-", ""), "ddMMyyyy", null);
                        tempdate = tempdate.AddMonths(-1);
                        start_year = tempdate.Year.ToString();
                        start_month = tempdate.Month.ToString();

                        if (start_month.Length == 1)
                        {
                            start_month = "0" + start_month;
                        }
                    }


                    string lsstartdate = start_year + "-" + start_month + "-" + lsstartday;
                    string lsenddate = end_year + "-" + end_month + "-" + lsendday;

                    DateTime startdate = DateTime.ParseExact(lsstartdate.Replace("-", ""), "yyyyMMdd", null);
                    DateTime enddate = DateTime.ParseExact(lsenddate.Replace("-", ""), "yyyyMMdd", null);
                }

                DataTable DT_salarycomponentdtl;
                List<mdlsalarycomponentlist> mdlsalarycomponent_dtl = new List<mdlsalarycomponentlist>();

                msSQL1 = " select a.employee_gid,b.salarycomponent_gid,a.employee2salarygradetemplate_gid,b.employee2salarygradetemplatedtl_gid,a.basic_salary,a.gross_salary," +
                            " a.net_salary,b.salarycomponent_name,b.componentgroup_gid,b.componentgroup_name,a.ctc, " +
                            " b.salarycomponent_amount, b.salarygradetype, ifnull(b.othercomponent_type,' ') as othercomponent_type,b.affect_in,b.salarycomponent_amount_employer," +
                            " b.primecomponent_flag,c.lop_flag,c.statutory_flag,a.salary_mode from pay_trn_temployee2salarygradetemplate a " +
                            " left join pay_trn_temployee2salarygradetemplatedtl b on a.employee2salarygradetemplate_gid=b.employee2salarygradetemplate_gid " +
                            " left join pay_mst_tsalarycomponent c on c.salarycomponent_gid=b.salarycomponent_gid group by employee_gid";

                DT_salarycomponentdtl = objdbconn.GetDataTable(msSQL1);
                mdlsalarycomponent_dtl = cmnfunctions.ConvertDataTable<mdlsalarycomponentlist>(DT_salarycomponentdtl);


                DataTable DT_salarycomponentamountdtl;
                List<mdlsalarycomponentlist> mdlsalarycomponentamount_dtl = new List<mdlsalarycomponentlist>();

                msSQL1 = " select a.employee_gid,b.salarycomponent_gid,a.employee2salarygradetemplate_gid,b.employee2salarygradetemplatedtl_gid,a.basic_salary,a.gross_salary," +
                            " a.net_salary,b.salarycomponent_name,b.componentgroup_gid,b.componentgroup_name,a.ctc,b.salarycomponent_percentage, " +
                            " b.salarycomponent_amount, b.salarygradetype, ifnull(b.othercomponent_type,' ') as othercomponent_type,b.affect_in,b.salarycomponent_amount_employer," +
                            " b.primecomponent_flag,c.lop_flag,c.statutory_flag,a.salary_mode from pay_trn_temployee2salarygradetemplate a " +
                            " left join pay_trn_temployee2salarygradetemplatedtl b on a.employee2salarygradetemplate_gid=b.employee2salarygradetemplate_gid " +
                            " left join pay_mst_tsalarycomponent c on c.salarycomponent_gid=b.salarycomponent_gid";

                DT_salarycomponentamountdtl = objdbconn.GetDataTable(msSQL1);
                mdlsalarycomponentamount_dtl = cmnfunctions.ConvertDataTable<mdlsalarycomponentlist>(DT_salarycomponentamountdtl);



                DataTable DT_empdtl;
                List<mdlemployeedetailslist> mdlemployee_dtl = new List<mdlemployeedetailslist>();

                msSQL = " select a.designation_gid,a.employee_gid,b.designation_name,a.branch_gid,c.branch_name,a.department_gid,d.department_name from hrm_mst_temployee a " +
                               " left join adm_mst_tdesignation b on a.designation_gid=b.designation_gid " +
                               " left join hrm_mst_tbranch c on c.branch_gid=a.branch_gid" +
                               " left join hrm_mst_tdepartment d on d.department_gid=a.department_gid";

                DT_empdtl = objdbconn.GetDataTable(msSQL);
                mdlemployee_dtl = cmnfunctions.ConvertDataTable<mdlemployeedetailslist>(DT_empdtl);

                DataTable DT_loandtl;
                List<mdlloanlist> mdlloan_dtl = new List<mdlloanlist>();

                msSQL = " select a.employee_gid,repayment_gid,repayment_amount,monthname(repayment_duration) as month, " +
                        " year(repayment_duration) as year from pay_trn_tloan a " +
                        " left join pay_trn_tloanrepayment b on a.loan_gid = b.loan_gid ";

                DT_loandtl = objdbconn.GetDataTable(msSQL);
                mdlloan_dtl = cmnfunctions.ConvertDataTable<mdlloanlist>(DT_loandtl);


                DataTable DT_salary_id;
                List<mdlsalaryid> mdlsalary_id = new List<mdlsalaryid>();


                msSQL = " select employee_gid,salary_gid from pay_trn_tsalary where month='" + values.month + "' and year='" + values.year + "'";

                DT_salary_id = objdbconn.GetDataTable(msSQL);
                mdlsalary_id = cmnfunctions.ConvertDataTable<mdlsalaryid>(DT_salary_id);




                foreach (var data in values.employeeleave_list)
                {
                    List<mdlsalarycomponentlist> DT_employeetemplatedtl = mdlsalarycomponent_dtl.Where(a => a.employee_gid == data.employee_gid).ToList();

                    foreach (var dr in DT_employeetemplatedtl)
                    {
                        string lssalary_gid = dr.employee2salarygradetemplate_gid;
                        string basic_salary = dr.basic_salary;
                        string grosssalary = dr.gross_salary;
                        string net_salary = dr.net_salary;
                        string ctc = dr.ctc;

                        double basicsalary = double.Parse(basic_salary);
                        double month_workingdays = double.Parse(data.totaldays);

                        double actual_leave_rate = (basicsalary * 1.5) / month_workingdays;

                        double leavecount = double.Parse(data.leavecount);
                        double leave_rate = Math.Round((basicsalary * leavecount) / month_workingdays, 2);

                        double monthworkingdays = double.Parse(data.salary_days);
                        double earned_basic_salary = Math.Round((basicsalary / month_workingdays) * monthworkingdays, 2);
                        double lslopdays = 0;
                        if (data.adjusted_lop == "0" || data.adjusted_lop == "")
                        {
                            lslopdays = double.Parse(data.absent);
                        }
                        else
                        {
                            lslopdays = double.Parse(data.adjusted_lop);
                        }
                        double lop = double.Parse(data.absent);

                        double lop_amount = Math.Round((basicsalary / month_workingdays) * lslopdays, 2);

                        double lsactual_month_workingdays = month_workingdays - lslopdays;

                        double earned_gross_salary = earned_basic_salary;

                        List<mdlemployeedetailslist> DT_employeedtl = mdlemployee_dtl.Where(a => a.employee_gid == data.employee_gid).ToList();
                        foreach (var drs in DT_employeedtl)
                        {
                            lsdesignation_gid = drs.designation_gid;
                            lsdesignation_name = drs.designation_name;
                            lsdepartment_gid = drs.department_gid;
                            lsdepartment_name = drs.department_name;
                            lsbranch_gid = drs.branch_gid;
                            lsbranch_name = drs.branch_name;
                        }
                        
                        msgetsalary_gid = objcmnfunctions.GetMasterGID_SP("PSLT");
                        msSQL = " insert into pay_trn_tsalary(" +
                                " salary_gid, " +
                                " month, " +
                                " year, " +
                                " employee_gid, " +
                                " basic_salary, " +
                                " gross_salary, " +
                                " net_salary, " +
                                " earned_basic_salary, " +
                                " actual_month_workingdays, " +
                                " actual_leave_salary, " +
                                " leave_taken, " +
                                " leave_salary, " +
                                " lop_days, " +
                                " worked_days, " +
                                " weekoff_days, " +
                                " public_holidays," +
                                " leave_generated_flag, " +
                                " payrun_flag, " +
                                " generated_by, " +
                                " generated_on, " +
                                " ctc, " +
                                " payrun_date, " +
                                " designation_gid, " +
                                " designation_name, " +
                                " branch_gid, " +
                                " branch_name, " +
                                " department_gid, " +
                                " department_name, " +
                                " month_workingdays) " +
                                " values( " +
                                "'" + msgetsalary_gid + "', " +
                                "'" + values.month + "', " +
                                "'" + values.year + "', " +
                                "'" + data.employee_gid + "', " +
                                "'" + basic_salary + "', " +
                                "'" + grosssalary + "', " +
                                "'" + net_salary + "', " +
                                "'" + earned_basic_salary + "', " +
                                "'" + lsactual_month_workingdays + "', " +
                                "'" + actual_leave_rate + "', " +
                                "'" + data.leavecount + "', " +
                                "'" + leave_rate + "', " +
                                 "'" + lslopdays + "', " +
                                "'" + data.month_workingdays + "', " +
                                "'" + data.weekoff_days + "', " +
                                "'" + data.holidaycount + "', " +
                                " 'Y', " +
                                " 'Y', " +
                                " '" + user_gid + "', " +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                " '" + ctc + "', " +
                                 " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                " '" + lsdesignation_gid + "', " +
                                " '" + lsdesignation_name + "', " +
                                " '" + lsbranch_gid + "', " +
                                " '" + lsbranch_name + "', " +
                                " '" + lsdepartment_gid + "', " +
                                " '" + lsdepartment_name + "', " +
                             "'" + data.totaldays + "') ";
                        ls_gid = msgetsalary_gid;
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        List<mdlsalarycomponentlist> DT_salarydtl = mdlsalarycomponentamount_dtl.Where(a => a.employee_gid == data.employee_gid).ToList();
                        foreach (var dr_s in DT_salarydtl)
                        {
                            if (dr_s.employee2salarygradetemplatedtl_gid != "")
                            {

                                lsflag = dr_s.lop_flag;
                                lssalarycomponent_gid = dr_s.salarycomponent_gid;
                                lscomponentgroup_gid = dr_s.componentgroup_gid;
                                lsstatutory_flag = dr_s.statutory_flag;
                            }


                            if (lsflag == "N")
                            {

                                string earn_salarycomponent1_amount = dr_s.salarycomponent_amount;
                                string earned_salarycomponent1_amount_employer = dr_s.salarycomponent_amount_employer;

                                earn_salarycomponent_amount = double.Parse(earn_salarycomponent1_amount);
                                earned_salarycomponent_amount_employer = double.Parse(earned_salarycomponent1_amount_employer);
                            }



                            else
                            {
                                string salarycomponent_amount = dr_s.salarycomponent_amount;
                                double salaryAmount = double.Parse(salarycomponent_amount);
                                string salarycomponent_amount_employer = dr_s.salarycomponent_amount_employer;
                                double salarycomponent_emp_amt = double.Parse(salarycomponent_amount_employer);

                                earn_salarycomponent_amount = Math.Round((salaryAmount / month_workingdays) * monthworkingdays, 2);
                                earned_salarycomponent_amount_employer = Math.Round(((salarycomponent_emp_amt / month_workingdays) * monthworkingdays), 2);
                            }
                            if (lsstatutory_flag == "Y")
                            {
                                statutory_amount = statutory_amount + (earn_salarycomponent_amount + earned_salarycomponent_amount_employer);

                            }

                            string lsgradetype = dr_s.salarygradetype;
                            string othercomponent_type = dr.othercomponent_type;

                            if (lsgradetype == "Addition")
                            {
                                lsaddition = lsaddition + earn_salarycomponent_amount;
                            }
                            else
                            {
                                lsdeduction = lsdeduction + earn_salarycomponent_amount;
                            }

                            if (monthworkingdays == 0)

                            {

                                earn_salarycomponent_amount = 0.0;

                                lsaddition = 0.0;

                                lsdeduction = 0.0;

                            }

                            string msgetsalary_gid1 = objcmnfunctions.GetMasterGID_SP("EMPF");
                            if (dr_s.componentgroup_name == "PF" || dr_s.componentgroup_name == "EPF")

                            {
                                msSQL = "delete from  pay_trn_employeepf where employee_gid='" + data.employee_gid + "'" +
                                        " and month='" + values.month + "' and year='" + values.year + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = " insert into pay_trn_employeepf(" +
                                         " employeepf_gid," +
                                         " month," +
                                         " year," +
                                         " employee_gid," +
                                         " earnedbasic_salary," +
                                         " employeepf_amount," +
                                         " actual_month_workingdays," +
                                         " lop_days," +
                                         " created_by," +
                                         " created_date)" +
                                         " values(" +
                                         "'" + msgetsalary_gid1 + "'," +
                                         "'" + values.month + "'," +
                                         "'" + values.year + "'," +
                                         "'" + data.employee_gid + "'," +
                                         "'" + earned_basic_salary + "'," +
                                         "'" + earn_salarycomponent_amount + "'," +
                                         "'" + monthworkingdays + "'," +
                                         "'" + (month_workingdays - monthworkingdays) + "'," +
                                         "'" + user_gid + "'," +
                                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }


                            string msgetsalarydtl_gid = objcmnfunctions.GetMasterGID_SP("PSLD");
                            msSQL = " insert into pay_trn_tsalarydtl( " +
                                        " salarydtl_gid, " +
                                        " salary_gid, " +
                                        " salarygradetype, " +
                                        " salarycomponent_name, " +
                                        " salarycomponent_gid, " +
                                        " componentgroup_gid," +
                                        " componentgroup_name, " +
                                        " created_by, " +
                                        " created_date, " +
                                        " salarycomponent_amount, " +
                                        " salarycomponent_percentage," +
                                        " earned_salarycomponent_amount, " +
                                        " earnedemployer_salarycomponentamount, " +
                                        " employersalarycomponent_amount, " +
                                        " othercomponent_type, " +
                                        " statutory_flag, " +
                                        " affect_in) " +
                                        " values( " +
                                        "'" + msgetsalarydtl_gid + "', " +
                                        "'" + msgetsalary_gid + "', " +
                                        "'" + lsgradetype + "', " +
                                        "'" + dr_s.salarycomponent_name + "', " +
                                        "'" + lssalarycomponent_gid + "'," +
                                        "'" + lscomponentgroup_gid + "'," +
                                        "'" + dr_s.componentgroup_name + "'," +
                                        "'" + user_gid + "', " +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                        "'" + dr_s.salarycomponent_amount + "', " +
                                        "'" + dr_s.salarycomponent_percentage + "'," +
                                        "'" + earn_salarycomponent_amount + "', " +
                                        "'" + earned_salarycomponent_amount_employer + "', " +
                                        "'" + dr_s.salarycomponent_amount_employer + "'," +
                                        "'" + dr_s.othercomponent_type + "', " +
                                        "'" + dr_s.statutory_flag + "', " +
                                        "'" + dr_s.affect_in + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }

                        if (dr.salary_mode == "Basic")
                        {
                            earned_gross_salary = Math.Round(earned_gross_salary + lsaddition);
                        }
                        else if (dr.salary_mode == "Gross")
                        {
                            earned_gross_salary = Math.Round(lsaddition);
                        }
                        double lsloan_amount = 0;
                        string lsrepayment_gid = "";
                        List<mdlloanlist> loan = mdlloan_dtl.Where(a => a.employee_gid == data.employee_gid && a.month == values.month
                       && a.year == values.year).ToList();
                        foreach (var ln in loan)
                        {
                            lsloan_amount = double.Parse(ln.repayment_amount);
                            lsrepayment_gid = ln.repayment_gid;

                            msSQL = "update pay_trn_tloanrepayment set " +
                                    " repaid_amount ='" + lsloan_amount + "'," +
                                    " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    " updated_by='" + user_gid + "'," +
                                    " actual_date ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " repayment_remarks = '" + values.month + "-" + values.year + " Salary process '" +
                                    " where repayment_gid='" + lsrepayment_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }

                        msSQL = " update pay_trn_employeepf set " +
                                " earned_gross_salary='" + earned_gross_salary + "' ," +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                " updated_by='" + user_gid + "'" +
                                " where employee_gid='" + data.employee_gid + "'" +
                                " and month='" + values.month + "' and year='" + values.month + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                        string lsattendance_allowanceflag = "", lsallowance_amount = "0";
                        double lsallowed_leave = 0.0;
                        double attendance_allowance_amount = 0.0;
                        msSQL = "select attendance_allowanceflag,allowed_leave,allowance_amount from hrm_mst_tattendanceconfig where 1=1 ";
                        objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader1.HasRows == true)
                        {
                            lsattendance_allowanceflag = objOdbcDataReader1["attendance_allowanceflag"].ToString();
                            lsallowed_leave = double.Parse(objOdbcDataReader1["allowed_leave"].ToString());
                            lsallowance_amount = objOdbcDataReader1["allowance_amount"].ToString();
                        }

                        if (lslopdays == 0)
                        {
                            if (lsattendance_allowanceflag == "Y")
                            {
                                if (double.Parse(data.leavecount) <= lsallowed_leave)
                                {
                                    attendance_allowance_amount = double.Parse(lsallowance_amount);
                                }
                                else
                                {
                                    attendance_allowance_amount = 0.0;
                                }
                            }
                        }
                        objOdbcDataReader1.Close();

                        double earned_net_salary = Math.Round(earned_gross_salary - (lsdeduction + lsloan_amount) + attendance_allowance_amount);
                        double lstakehomeamount = earned_net_salary + attendance_allowance_amount;

                        msSQL = " update pay_trn_tsalary set " +
                                " earned_gross_salary='" + earned_gross_salary + "', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                " updated_by='" + user_gid + "'," +
                                " earned_net_salary='" + earned_net_salary + "', " +
                                " takehome_salary='" + lstakehomeamount + "', " +
                                " repayment_gid ='" + lsrepayment_gid + "'," +
                                " loanadvance_amount = '" + lsloan_amount + "'," +
                                " attendance_allowance = '" + attendance_allowance_amount + "'" +
                                " where salary_gid='" + ls_gid + "' ";
                        int mnResult5 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update pay_trn_temployeepayrun set leave_generate_flag='Y' where employee_gid='" + data.employee_gid + "' and month='" + values.month + "' and year='" + values.year + "' ";
                        int mnResult6 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "select finance_flag from adm_mst_tcompany";
                        string lsfinance_flag = objdbconn.GetExecuteScalar(msSQL);

                        if (lsfinance_flag == "Y")
                        {
                            objfinance_cmnfunction.employee_payrun(ls_gid,
                                                              DateTime.Now.ToString("yyyy-MM-dd"),
                                                              data.employee_gid,
                                                              data.user_code,
                                                              data.username,
                                                              double.Parse(net_salary),
                                                              lsdeduction,
                                                              statutory_amount,
                                                              branch_gid,
                                                              "Salary Payment for the employee " + data.employee_gid + " on this " + values.month + "-" + values.year,
                                                              lsloan_amount,
                                                              lsadvance,
                                                              lsaddition,
                                                              lsdeduction,
                                                              journal_refno,
                                                              values.month,
                                                              values.year);
                        }
                    }

                    earn_salarycomponent_amount = 0.0;
                    lsaddition = 0.0;
                    lsdeduction = 0.0;


                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Monthly Payrun successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Monthly Payrun";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Monthly Payrun!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaGetManageLeave(string month, string year, MdlPayTrnSalaryManagement values)
        {
            try
            {
                msSQL1 = "select payrun_dateflag from adm_mst_tcompany ";
                lspayrun_flag = objdbconn.GetExecuteScalar(msSQL1);

                int monthNumber = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month;

                // SQL query to select data from the database
                string msSQL = "SELECT start_day, end_day, month_interval FROM pay_mst_tpayrollconfig";

                // Execute the SQL query and retrieve data
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    DataRow objrow = dt_datatable1.Rows[0];
                    string startDay = objrow["start_day"].ToString();
                    string endDay = objrow["end_day"].ToString();
                    string monthInterval = objrow["month_interval"].ToString();

                    string lsinterval = monthInterval;
                    string lsstartday = startDay;
                    string lsendday = endDay;

                    if (lsinterval == "current")
                    {
                        start_month = monthNumber.ToString();
                        if (start_month.Length == 1)
                        {
                            start_month = "0" + start_month;
                        }
                        end_month = start_month;
                        start_year = year;
                        end_year = year;

                        int totaldays1 = DateTime.DaysInMonth(int.Parse(start_year), int.Parse(start_month));

                        if (int.Parse(lsendday) > totaldays1)
                        {
                            lsendday = totaldays1.ToString();
                        }
                    }
                    else
                    {
                        end_month = monthNumber.ToString();
                        if (end_month.Length == 1)
                        {
                            end_month = "0" + end_month;
                        }
                        string end_year = year;

                        // -----------------------to get start date-------------------'
                        string lsstring = lsendday + "-" + end_month + "-" + end_year;
                        DateTime tempdate = DateTime.ParseExact(lsstring.Replace("-", ""), "ddMMyyyy", null);
                        tempdate = tempdate.AddMonths(-1);
                        start_year = tempdate.Year.ToString();
                        start_month = tempdate.Month.ToString();

                        if (start_month.Length == 1)
                        {
                            start_month = "0" + start_month;
                        }
                    }


                    string lsstartdate = start_year + "-" + start_month + "-" + lsstartday;
                    string lsenddate = end_year + "-" + end_month + "-" + lsendday;

                    DateTime startdate = DateTime.ParseExact(lsstartdate.Replace("-", ""), "yyyyMMdd", null);
                    DateTime enddate = DateTime.ParseExact(lsenddate.Replace("-", ""), "yyyyMMdd", null);

                    int totaldays = (int)(enddate - startdate).TotalDays;
                    totaldays = totaldays + 1;


                    int noofdays = totaldays;
                    days1 = noofdays.ToString();

                    msSQL = " select distinct a.actuallop_days as actual_lop,a.adjusted_lopdays as lop," +
                        " b.user_gid,b.user_code,concat(ifnull(b.user_firstname, ''), ' ', ifnull(b.user_lastname, '')) as username,a.employee_gid, a.month_workingdays" +
                        " from pay_trn_temployeepayrun a" +
                        " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid" +
                        " left join adm_mst_tuser b on c.user_gid = b.user_gid" +
                        " left join hrm_trn_temployeetypedtl h on c.employee_gid = h.employee_gid" +
                        " where a.month = '" + month + "' and a.year ='" + year + "' and a.employee_select = 'Y' and leave_generate_flag = 'N'" +
                        " group by a.employee_gid order by b.user_code asc ";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<employeeleave_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msSQL = "select count(attendance_gid) as absent_count" +
                                   "  from hrm_trn_tattendance" +
                                   " where  attendance_type='A' and attendance_date>='" + lsstartdate + "'" +
                                   " and attendance_date<='" + lsenddate + "'" +
                                   " and employee_gid='" + dt["employee_gid"].ToString() + "' ";
                            string absent_count = objdbconn.GetExecuteScalar(msSQL);
                            if (absent_count == "")
                            {
                                absent_count = "0.0";
                            }
                            msSQL = "select count(attendance_gid) as leave_count" +
                                  "  from hrm_trn_tattendance" +
                                  " where  employee_attendance='Leave' and attendance_date>='" + lsstartdate + "'" +
                                  " and attendance_date<='" + lsenddate + "'" +
                                  " and employee_gid='" + dt["employee_gid"].ToString() + "' ";
                            string leave_count = objdbconn.GetExecuteScalar(msSQL);
                            if (leave_count == "")
                            {
                                leave_count = "0.0";
                            }

                            msSQL = " select count(attendance_gid) as week_off from hrm_trn_tattendance " +
                                " where attendance_type='WH' and employee_gid='" + dt["employee_gid"].ToString() + "' " +
                                " and attendance_date>='" + lsstartdate + "' " +
                                " and attendance_date<='" + lsenddate + "' ";
                            string lsweekoff = objdbconn.GetExecuteScalar(msSQL);
                            if (lsweekoff == "")
                            {
                                lsweekoff = "0.0";
                            }

                            msSQL = " select count(attendance_gid) as holiday from hrm_trn_tattendance " +
                                " where employee_attendance='Holiday' and employee_gid='" + dt["employee_gid"].ToString() + "' " +
                                " and attendance_date>='" + lsstartdate + "' " +
                                " and attendance_date<='" + lsenddate + "' ";
                            string lsholiday = objdbconn.GetExecuteScalar(msSQL);
                            if (lsholiday == "")
                            {
                                lsholiday = "0.0";
                            }
                            msSQL = " select count(attendance_gid) as day_count from hrm_trn_tattendance where employee_gid='" + dt["employee_gid"].ToString() + "' " +
                                 " and attendance_type='P'" +
                                 " and attendance_date>='" + lsstartdate +
                                 "' and attendance_date<='" + lsenddate + "' ";
                            string presentcount = objdbconn.GetExecuteScalar(msSQL);

                            int presentcount1 = int.Parse(presentcount);
                            int lsholiday1 = int.Parse(lsholiday);
                            int lsweekoff1 = int.Parse(lsweekoff);
                            int leave_count1 = int.Parse(leave_count);

                            int salarydays = presentcount1 + lsholiday1 + lsweekoff1 + leave_count1;


                            if (presentcount == "")
                            {
                                presentcount = "0.0";
                            }


                            getModuleList.Add(new employeeleave_list
                            {
                                user_gid = dt["user_gid"].ToString(),
                                employee_gid = dt["employee_gid"].ToString(),
                                actual_lop = dt["actual_lop"].ToString(),
                                adjusted_lop = absent_count,
                                user_code = dt["user_code"].ToString(),
                                username = dt["username"].ToString(),
                                month_workingdays = presentcount,
                                absent = absent_count,
                                salary_days = salarydays.ToString(),
                                leavecount = leave_count,
                                holidaycount = lsholiday,
                                weekoffcount = lsweekoff,
                                weekoff_days = lsweekoff,
                                actualworkingdays = presentcount,
                                totaldays = days1,
                                lspayrun_flag = lspayrun_flag,




                            });
                            values.employeeleavelist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Manage Leave!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetManageLeaveDate(string fromDate, string toDate, MdlPayTrnSalaryManagement values)
        {
            try
            {
                msSQL1 = "select payrun_dateflag from adm_mst_tcompany ";
                lspayrun_flag = objdbconn.GetExecuteScalar(msSQL1);


                // SQL query to select data from the database

                string lsstartdate = fromDate;
                string lsenddate = toDate;
                DateTime startdate = DateTime.Parse(lsstartdate);
                DateTime enddate = DateTime.Parse(lsenddate);
                int monthNumber = startdate.Month;



                int totaldays = (int)(enddate - startdate).TotalDays;
                totaldays = totaldays + 1;

                int noofdays = totaldays;
                days1 = noofdays.ToString();
                // Example month number
                string monthName = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(monthNumber);

                msSQL = " select distinct a.actuallop_days as actual_lop,a.adjusted_lopdays as lop," +
                    " b.user_gid,b.user_code,concat(ifnull(b.user_firstname, ''), ' ', ifnull(b.user_lastname, '')) as username,a.employee_gid, a.month_workingdays" +
                    " from pay_trn_temployeepayrun a" +
                    " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid" +
                    " left join adm_mst_tuser b on c.user_gid = b.user_gid" +
                    " left join hrm_trn_temployeetypedtl h on c.employee_gid = h.employee_gid" +
                    " where a.month = '" + monthName + "' and a.year ='" + startdate.ToString("yyyy") + "' and a.employee_select = 'Y' and leave_generate_flag = 'N'" +
                    " group by a.employee_gid order by b.user_code asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeeleave_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = "select count(attendance_gid) as absent_count" +
                               "  from hrm_trn_tattendance" +
                               " where  attendance_type='A' and attendance_date>='" + lsstartdate + "'" +
                               " and attendance_date<='" + lsenddate + "'" +
                               " and employee_gid='" + dt["employee_gid"].ToString() + "' ";
                        string absent_count = objdbconn.GetExecuteScalar(msSQL);
                        if (absent_count == "")
                        {
                            absent_count = "0.0";
                        }
                        msSQL = "select count(attendance_gid) as leave_count" +
                              "  from hrm_trn_tattendance" +
                              " where  employee_attendance='Leave' and attendance_date>='" + lsstartdate + "'" +
                              " and attendance_date<='" + lsenddate + "'" +
                              " and employee_gid='" + dt["employee_gid"].ToString() + "' ";
                        string leave_count = objdbconn.GetExecuteScalar(msSQL);
                        if (leave_count == "")
                        {
                            leave_count = "0.0";
                        }

                        msSQL = " select count(attendance_gid) as week_off from hrm_trn_tattendance " +
                            " where attendance_type='WH' and employee_gid='" + dt["employee_gid"].ToString() + "' " +
                            " and attendance_date>='" + lsstartdate + "' " +
                            " and attendance_date<='" + lsenddate + "' ";
                        string lsweekoff = objdbconn.GetExecuteScalar(msSQL);
                        if (lsweekoff == "")
                        {
                            lsweekoff = "0.0";
                        }

                        msSQL = " select count(attendance_gid) as holiday from hrm_trn_tattendance " +
                            " where employee_attendance='Holiday' and employee_gid='" + dt["employee_gid"].ToString() + "' " +
                            " and attendance_date>='" + lsstartdate + "' " +
                            " and attendance_date<='" + lsenddate + "' ";
                        string lsholiday = objdbconn.GetExecuteScalar(msSQL);
                        if (lsholiday == "")
                        {
                            lsholiday = "0.0";
                        }
                        msSQL = " select count(attendance_gid) as day_count from hrm_trn_tattendance where employee_gid='" + dt["employee_gid"].ToString() + "' " +
                             " and attendance_type='P'" +
                             " and attendance_date>='" + lsstartdate +
                             "' and attendance_date<='" + lsenddate + "' ";
                        string presentcount = objdbconn.GetExecuteScalar(msSQL);

                        int presentcount1 = int.Parse(presentcount);
                        int lsholiday1 = int.Parse(lsholiday);
                        int lsweekoff1 = int.Parse(lsweekoff);
                        int leave_count1 = int.Parse(leave_count);

                        int salarydays = presentcount1 + lsholiday1 + lsweekoff1 + leave_count1;


                        if (presentcount == "")
                        {
                            presentcount = "0.0";
                        }


                        getModuleList.Add(new employeeleave_list
                        {
                            user_gid = dt["user_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            actual_lop = dt["actual_lop"].ToString(),
                            adjusted_lop = absent_count,
                            user_code = dt["user_code"].ToString(),
                            username = dt["username"].ToString(),
                            month_workingdays = presentcount,
                            absent = absent_count,
                            salary_days = salarydays.ToString(),
                            leavecount = leave_count,
                            holidaycount = lsholiday,
                            weekoffcount = lsweekoff,
                            weekoff_days = lsweekoff,
                            actualworkingdays = presentcount,
                            totaldays = days1,
                            lspayrun_flag = lspayrun_flag,




                        });
                        values.employeeleavelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }

            catch (Exception ex)
            {
                values.message = "Exception occured while loading Manage Leave!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }




        //public void DagetUpdatedLeave(string user_gid, string month, string year, MdlPayTrnSalaryManagement values)

        //{

        //    msSQL = " update pay_trn_temployeepayrun set leave_generate_flag='Y' " +
        //            " updated_by='" + user_gid + "'," +
        //            " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
        //            " where employee_gid='" + lsemployeegid + "' and month= '" + month + "' and year= '" + year + "'";
        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //    if (mnResult != 0)
        //    {
        //        values.status = true;
        //        values.message = "Leave Updated Successfully";
        //    }
        //    else
        //    {
        //        values.status = false;
        //        values.message = "Error While Updating Leave";
        //    }

        //}

        public static bool IsLeapYear(int year)
        {
            if (year > 1 || year < 9999)
            {
                if (year % 4 == 0)
                {
                    if (year % 100 == 0)
                    {
                        return year % 400 == 0;
                    }

                    return true;
                }

            }

            return false;
        }
        private static readonly int[] DaysToMonth365 = new[]
   {
        0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365
    };

        private static readonly int[] DaysToMonth366 = new[]
        {
        0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366
    };

        public payrunedit_list1 DaGetpayrunedit(string salary_gid)
        {
            try
            {
                payrunedit_list1 objpayrunedit_list1 = new payrunedit_list1();
                {
                    msSQL = " select concat(c.user_firstname,' ',ifnull(c.user_lastname,' ')) as employee_name,a.basic_salary,c.user_code,a.employee_gid,a.payrun_date  " +
                            " from pay_trn_tsalary a " +
                            " inner join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                            " inner join adm_mst_tuser c on b.user_gid=c.user_gid " +
                            " where a.salary_gid='" + salary_gid + "' ";

                }

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objpayrunedit_list1.employee_name = objOdbcDataReader["employee_name"].ToString();
                    objpayrunedit_list1.payrun_date = objOdbcDataReader["payrun_date"].ToString();
                    objpayrunedit_list1.basic_salary = objOdbcDataReader["basic_salary"].ToString();


                    objOdbcDataReader.Close();
                }
                return objpayrunedit_list1;
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }

        public payrunedit_list2 DaGetpayruneditaddition(string salary_gid)
        {
            try
            {
                payrunedit_list2 objpayrunedit_list2 = new payrunedit_list2();
                {
                    msSQL = " select salarydtl_gid, salary_gid, salarygradetype,earnedemployer_salarycomponentamount,componentgroup_gid,componentgroup_name, " +
                            " salarycomponent_name,earned_salarycomponent_amount as salarycomponent_amount  " +
                            " from pay_trn_tsalarydtl " +
                            " where salary_gid='" + salary_gid + "' and salarygradetype='Addition' and primecomponent_flag<>'Y'";

                }

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objpayrunedit_list2.salarycomponent_name = objOdbcDataReader["salarycomponent_name"].ToString();
                    objpayrunedit_list2.componentgroup_name = objOdbcDataReader["componentgroup_name"].ToString();
                    objpayrunedit_list2.salarycomponent_amount = objOdbcDataReader["salarycomponent_amount"].ToString();
                    objpayrunedit_list2.earnedemployer_salarycomponentamount = objOdbcDataReader["earnedemployer_salarycomponentamount"].ToString();


                    objOdbcDataReader.Close();
                }
                return objpayrunedit_list2;
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }

        public payrunedit_list3 DaGetpayruneditdeduction(string salary_gid)
        {
            try
            {
                payrunedit_list3 objpayrunedit_list3 = new payrunedit_list3();
                {
                    msSQL = " select salarydtl_gid, salary_gid, salarygradetype,componentgroup_gid,componentgroup_name, " +
                            " salarycomponent_name,earned_salarycomponent_amount as salarycomponent_amount,earnedemployer_salarycomponentamount  " +
                            " from pay_trn_tsalarydtl " + " where salary_gid='" + salary_gid + "' and salarygradetype='Deduction'";
                }

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objpayrunedit_list3.salarycomponent_name = objOdbcDataReader["salarycomponent_name"].ToString();
                    objpayrunedit_list3.componentgroup_name = objOdbcDataReader["componentgroup_name"].ToString();
                    objpayrunedit_list3.salarycomponent_amount = objOdbcDataReader["salarycomponent_amount"].ToString();
                    objpayrunedit_list3.earnedemployer_salarycomponentamount = objOdbcDataReader["earnedemployer_salarycomponentamount"].ToString();


                    objOdbcDataReader.Close();
                }
                return objpayrunedit_list3;
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }

        public Payrun_list DaGetpayrundetails(string salary_gid)
        {
            try
            {
                Payrun_list objPayrun_list = new Payrun_list();
                {
                    msSQL = " SELECT concat(c.user_firstname,' ',ifnull(c.user_lastname,' ')) as employee_name,a.salary_gid,a.employee_gid,date_format(a.payrun_date,'%d-%m-%Y') as payrun_date,a.basic_salary,d.salary_mode " +
                            " from pay_trn_tsalary a " +
                            " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                            " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                            " left join pay_trn_temployee2salarygradetemplate d on a.employee_gid = d.employee_gid " +
                            " where a.salary_gid ='" + salary_gid + "' ";
                }

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objPayrun_list.employee_name = objOdbcDataReader["employee_name"].ToString();
                    objPayrun_list.basic_salary = objOdbcDataReader["basic_salary"].ToString();
                    objPayrun_list.payrun_date = objOdbcDataReader["payrun_date"].ToString();
                    objPayrun_list.salary_mode = objOdbcDataReader["salary_mode"].ToString();



                    objOdbcDataReader.Close();
                }
                return objPayrun_list;
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }

        public void DaGetpayruneditsummary(string salary_gid, MdlPayTrnSalaryManagement values)
        {
            try
            {
                msSQL = " select salarydtl_gid, salary_gid, salarygradetype,earnedemployer_salarycomponentamount,componentgroup_gid,componentgroup_name, " +
                        " salarycomponent_name,earned_salarycomponent_amount as salarycomponent_amount  " +
                        " from pay_trn_tsalarydtl " +
                        " where salary_gid='" + salary_gid + "' and salarygradetype='Addition' and primecomponent_flag<>'Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editaddition_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editaddition_list
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarydtl_gid = dt["salarydtl_gid"].ToString(),
                            addsalarycomponent_name = dt["salarycomponent_name"].ToString(),
                            addsalarycomponent_amount = dt["earnedemployer_salarycomponentamount"].ToString(),
                            addemployer_salarycomponentamount = dt["salarycomponent_amount"].ToString(),
                            addcomponentgroup_name = dt["componentgroup_name"].ToString(),

                        });
                        values.Editaddition_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

                msSQL = " select salarydtl_gid, salary_gid, salarygradetype,componentgroup_gid,componentgroup_name, " +
               " salarycomponent_name,earned_salarycomponent_amount as salarycomponent_amount,earnedemployer_salarycomponentamount  " +
               " from pay_trn_tsalarydtl " +
               " where salary_gid='" + salary_gid + "' and salarygradetype='Deduction'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList1 = new List<Editdeduction_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new Editdeduction_list
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarydtl_gid = dt["salarydtl_gid"].ToString(),
                            dedsalarycomponent_name = dt["salarycomponent_name"].ToString(),
                            dedsalarycomponent_amount = dt["earnedemployer_salarycomponentamount"].ToString(),
                            dedemployer_salarycomponentamount = dt["salarycomponent_amount"].ToString(),
                            dedcomponentgroup_name = dt["componentgroup_name"].ToString(),

                        });
                        values.Editdeduction_list = getModuleList1;
                    }
                }
                dt_datatable.Dispose();

                msSQL = " select salarydtl_gid, salary_gid, salarygradetype, componentgroup_gid,componentgroup_name," +
                        " salarycomponent_name,earned_salarycomponent_amount as salarycomponent_amount , othercomponent_type,earnedemployer_salarycomponentamount " +
                        " from pay_trn_tsalarydtl " +
                        " where salary_gid='" + salary_gid + "' and salarygradetype='others'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList2 = new List<Editothers_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList2.Add(new Editothers_list
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarydtl_gid = dt["salarydtl_gid"].ToString(),
                            otherssalarycomponent_name = dt["salarycomponent_name"].ToString(),
                            othersemployer_salarycomponentamount = dt["earnedemployer_salarycomponentamount"].ToString(),
                            otherssalarycomponent_amount = dt["salarycomponent_amount"].ToString(),
                            otherscomponentgroup_name = dt["componentgroup_name"].ToString(),

                        });
                        values.Editothers_list = getModuleList2;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetadditionEdit(string salary_gid, MdlPayTrnSalaryManagement values)
        {
            double lsgrosssalary = 0;
            double lsgrosssalaryemployer = 0;

            try
            {
                msSQL = " select salarydtl_gid, salary_gid, salarygradetype,format(earnedemployer_salarycomponentamount,2) as earnedemployer_salarycomponentamount,componentgroup_gid,componentgroup_name, " +
                        " salarycomponent_name,earned_salarycomponent_amount as salarycomponent_amount  " +
                        " from pay_trn_tsalarydtl " +
                        " where salary_gid='" + salary_gid + "' and salarygradetype='Addition' and primecomponent_flag<>'Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editaddition_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new Editaddition_list
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarydtl_gid = dt["salarydtl_gid"].ToString(),
                            addsalarycomponent_name = dt["salarycomponent_name"].ToString(),
                            addsalarycomponent_amount = dt["salarycomponent_amount"].ToString(),
                            addemployer_salarycomponentamount = dt["earnedemployer_salarycomponentamount"].ToString(),
                            addcomponentgroup_name = dt["componentgroup_name"].ToString(),

                        });
                        values.Editaddition_list = getModuleList;

                    }
                }
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetdeductionEdit(string salary_gid, MdlPayTrnSalaryManagement values)
        {
            try
            {
                msSQL = " select salarydtl_gid, salary_gid, salarygradetype,componentgroup_gid,componentgroup_name, " +
                        " salarycomponent_name, salarycomponent_amount,earnedemployer_salarycomponentamount " +
                        " from pay_trn_tsalarydtl " +
                        " where salary_gid='" + salary_gid + "' and salarygradetype='Deduction'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editdeduction_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editdeduction_list
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarydtl_gid = dt["salarydtl_gid"].ToString(),
                            dedsalarycomponent_name = dt["salarycomponent_name"].ToString(),
                            dedsalarycomponent_amount = dt["salarycomponent_amount"].ToString(),
                            dedemployer_salarycomponentamount = dt["earnedemployer_salarycomponentamount"].ToString(),
                            dedcomponentgroup_name = dt["componentgroup_name"].ToString(),

                        });
                        values.Editdeduction_list = getModuleList;
                    }
                }
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetothersEdit(string salary_gid, MdlPayTrnSalaryManagement values)
        {
            try
            {
                msSQL = " select salarydtl_gid, salary_gid, salarygradetype, componentgroup_gid,componentgroup_name," +
               " salarycomponent_name,format(earned_salarycomponent_amount,2) as salarycomponent_amount , othercomponent_type,format(earnedemployer_salarycomponentamount,2) as earnedemployer_salarycomponentamount " +
               " from pay_trn_tsalarydtl " +
               " where salary_gid='" + salary_gid + "' and salarygradetype='others'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editothers_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editothers_list
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarydtl_gid = dt["salarydtl_gid"].ToString(),
                            otherssalarycomponent_name = dt["salarycomponent_name"].ToString(),
                            otherssalarycomponent_amount = dt["salarycomponent_amount"].ToString(),
                            othersemployer_salarycomponentamount = dt["earnedemployer_salarycomponentamount"].ToString(),
                            otherscomponentgroup_name = dt["componentgroup_name"].ToString(),


                        });
                        values.Editothers_list = getModuleList;
                    }
                }
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetSalaryEdit(string salary_gid, MdlPayTrnSalaryManagement values)
        {
            try
            {
                msSQL = " select salary_gid, format(earned_basic_salary,2) as basic_salary, " +
                        " earned_gross_salary as gross_salary, earned_net_salary as net_salary,ctc from pay_trn_tsalary" +
                        " where salary_gid ='" + salary_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editsalary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editsalary_list
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            basic_salary = dt["basic_salary"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            ctc = dt["ctc"].ToString(),


                        });
                        values.Editsalary_list = getModuleList;
                    }
                }
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaUpdatepayrunedit(string user_gid, UpdatePayrun values)
        {
            try
            {

                string uiDateStr = values.payrun_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string payrun_date = uiDate.ToString("yyyy-MM-dd");


                if (values.Editaddition_list != null)
                {
                    foreach (var data in values.Editaddition_list)
                    {
                        msSQL = " update pay_trn_tsalarydtl set " +
                                " earnedemployer_salarycomponentamount='" + data.addemployer_salarycomponentamount + "', " +
                                " earned_salarycomponent_amount='" + data.addsalarycomponent_amount + "' " +
                                " where salarydtl_gid='" + data.salarydtl_gid + "' and salary_gid ='" + values.salary_gid + "' ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }


                if (values.Editdeduction_list != null)
                {
                    foreach (var data in values.Editdeduction_list)
                    {

                        msSQL = " update pay_trn_tsalarydtl set " +
                                " earnedemployer_salarycomponentamount='" + data.dedemployer_salarycomponentamount + "', " +
                                " earned_salarycomponent_amount='" + data.dedsalarycomponent_amount + "' " +
                                " where salarydtl_gid='" + data.salarydtl_gid + "' and salary_gid ='" + values.salary_gid + "' ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }

                if (values.Editothers_list != null)
                {

                    foreach (var data in values.Editothers_list)
                    {

                        msSQL = " update pay_trn_tsalarydtl set " +
                                " earnedemployer_salarycomponentamount='" + data.othersemployer_salarycomponentamount + "', " +
                                " earned_salarycomponent_amount='" + data.otherssalarycomponent_amount + "' " +
                                " where salarydtl_gid='" + data.salarydtl_gid + "' and salary_gid ='" + values.salary_gid + "' ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }

                msSQL = " update pay_trn_tsalary set" +
                        " earned_gross_salary='" + values.gross_salary + "', " +
                        " earned_net_salary='" + values.net_salary + "', " +
                        " ctc='" + values.ctc + "', " +
                        " basic_salary='" + values.basic_salary + "', " +
                        " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                        " updated_by='" + user_gid + "', " +
                        " payrun_date='" + payrun_date + "' " +
                        " where salary_gid ='" + values.salary_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Payrun Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Payrun";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Payrun!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetLeaveReport(string month, string year, MdlPayTrnSalaryManagement values)
        {
            try
            {
                int monthNumber = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month;

                // SQL query to select data from the database
                string msSQL = "SELECT start_day, end_day, month_interval FROM pay_mst_tpayrollconfig";

                // Execute the SQL query and retrieve data
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    DataRow objrow = dt_datatable1.Rows[0];
                    string startDay = objrow["start_day"].ToString();
                    string endDay = objrow["end_day"].ToString();
                    string monthInterval = objrow["month_interval"].ToString();

                    string lsinterval = monthInterval;
                    string lsstartday = startDay;
                    string lsendday = endDay;

                    if (lsinterval == "current")
                    {
                        start_month = monthNumber.ToString();
                        if (start_month.Length == 1)
                        {
                            start_month = "0" + start_month;
                        }
                        end_month = start_month;
                        start_year = year;
                        end_year = year;

                        int totaldays1 = DateTime.DaysInMonth(int.Parse(start_year), int.Parse(start_month));

                        if (int.Parse(lsendday) > totaldays1)
                        {
                            lsendday = totaldays1.ToString();
                        }
                    }
                    else
                    {
                        end_month = monthNumber.ToString();
                        if (end_month.Length == 1)
                        {
                            end_month = "0" + end_month;
                        }
                        string end_year = year;

                        // -----------------------to get start date-------------------'
                        string lsstring = lsendday + "-" + end_month + "-" + end_year;
                        DateTime tempdate = DateTime.ParseExact(lsstring.Replace("-", ""), "ddMMyyyy", null);
                        tempdate = tempdate.AddMonths(-1);
                        start_year = tempdate.Year.ToString();
                        start_month = tempdate.Month.ToString();

                        if (start_month.Length == 1)
                        {
                            start_month = "0" + start_month;
                        }
                    }


                    string lsstartdate = lsstartday + "-" + start_month + "-" + start_year;
                    string lsenddate = lsendday + "-" + end_month + "-" + end_year;

                    DateTime startdate = DateTime.ParseExact(lsstartdate.Replace("-", ""), "ddMMyyyy", null);
                    DateTime enddate = DateTime.ParseExact(lsenddate.Replace("-", ""), "ddMMyyyy", null);

                    int totaldays = (int)(enddate - startdate).TotalDays;
                    totaldays = totaldays + 1;
                    noofdays = totaldays;

                    msSQL = " select a.salary_gid, a.employee_gid,c.user_code,concat(c.user_firstname,' ',c.user_lastname)as username,a.lop_days as lop," +
                        " a.month_workingdays,d.actuallop_days as actual_lop,c.user_gid " +
                        " from pay_trn_tsalary a" +
                        " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                        " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                        " left join pay_trn_temployeepayrun d on d.employee_gid = a.employee_gid " +
                        " where  a.month='" + month + "' and a.year='" + year + "' and a.payrun_flag='Y'  " +
                        "  group by a.salary_gid order by b.employee_gid";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<leavereport_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msSQL = " select count(attendance_gid) as day_count from hrm_trn_tattendance where employee_gid='" + dt["employee_gid"].ToString() + "' " +
                                     " and attendance_type='P'" +
                                     " and attendance_date>='" + lsstartdate +
                                     "' and attendance_date<='" + lsenddate + "' ";
                            string presentcount = objdbconn.GetExecuteScalar(msSQL);

                            getModuleList.Add(new leavereport_list
                            {
                                employee_gid = dt["employee_gid"].ToString(),
                                user_code = dt["user_code"].ToString(),
                                username = dt["username"].ToString(),
                                lop = dt["lop"].ToString(),
                                user_gid = dt["user_gid"].ToString(),
                                month_workingdays = dt["month_workingdays"].ToString(),
                                actual_lop = dt["actual_lop"].ToString(),
                                salary_days = presentcount,

                            });
                            values.leavereport_list = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payrun View!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaDeleteleavereport(deleteleavereportlist values)
        {
            try
            {

                foreach (var data in values.employeeleavelist)
                {
                    monthnum = GetMonthNumber(values.month);

                    msSQL = "select * from hrm_mst_tleavecreditsdtl where employee_gid='" + data.employee_gid + "' and year='" + data.year + "' and month='" + monthnum + "'";
                    DataTable objTbl = objdbconn.GetDataTable(msSQL);
                    foreach (DataRow dt in objTbl.Rows)
                    {
                        msSQL = "update hrm_trn_tleavecreditsdtl set " +
                               " available_leavecount='" + dt["available_leavecount"].ToString() + "', " +
                              " leave_carrycount='" + dt["leavecarry_count"].ToString() + "' " +
                               " where employee_gid='" + data.employee_gid + "' and leavecreditsdtl_gid='" + dt["leavecreditsdtl_gid"].ToString() + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    msSQL = " delete from pay_trn_tsalarydtl where salary_gid='" + data.salary_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = " delete from pay_trn_tsalary where salary_gid='" + data.salary_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    msSQL = "update pay_trn_temployeepayrun set payrun_flag='N',leave_generate_flag='N',employee_select='N' where employee_gid='" + data.employee_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Leave Report Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While deleting the Leave Report";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Selecting Employee!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public static int GetMonthNumber(string monthName)
        {
            try
            {
                // Attempt to parse the month name
                DateTime date = DateTime.ParseExact(monthName, "MMMM", System.Globalization.CultureInfo.CurrentCulture);

                // Return the month number
                return date.Month;
            }
            catch (FormatException)
            {
                // If parsing fails, return -1
                return -1;
            }
        }



        public void employee_payrun(string salary_gid, string payrun_date, string employee_gid, string employee_code, string employee_name, double gross_salary, double employeecomponent_amount, double statutory_amount,
                  string branch, string journal_refno, double loan_amount, double advance_amount, double other_addition, double other_deduction, string refno, string payrunmonth, string payrunyear)
        {
            List<string> journal_monthandyear = new List<string>();

            string msGetGID, msGetdlGID;
            DateTime journal_date;
            int journal_year, journal_month, journal_day;
            string account_gid;
            double grandtotal = 0.0;
            string month = payrunmonth;
            string year = payrunyear;
            grandtotal = gross_salary + statutory_amount - employeecomponent_amount;
            DateTime payrundate = Convert.ToDateTime(payrun_date);
            DateTime date = DateTime.Now; // Example date
            journal_month = date.Month;
            journal_day = date.Day;
            journal_year = date.Year;

            JournalFunction(payrundate);


            //journal_year = journal_monthandyear[0];
            //journal_month = journal_monthandyear[1];
            //journal_day = journal_monthandyear[2];


            msGetGID = objcmnfunctions.GetMasterGID("FPCC");

            msSQL = " Insert into acc_trn_journalentry " +
                 " (journal_gid, " +
                 " journal_refno, " +
                 " transaction_code, " +
                 " transaction_date, " +
                 " transaction_type," +
                 " reference_type," +
                 " reference_gid," +
                 " transaction_gid, " +
                 " journal_from," +
                 " journal_year, " +
                 " journal_month, " +
                 " journal_day, " +
                 " remarks, " +
                 " branch_gid)" +
                 " values (" +
                 "'" + msGetGID + "', " +
                 "'" + month + year + "-" + refno + "'," +
                 "'" + employee_code + "'," +
                 "'" + payrun_date + "', " +
                 "'Journal', " +
                 "'" + employee_name + "', " +
                 "'" + employee_gid + "', " +
                 "'" + salary_gid + "', " +
                 "'Payroll'," + "'" +
                 journal_year + "', " +
                 "'" + journal_month + "', " +
                 "'" + journal_day + "', " +
                 " 'Salary Payrun for the employee" + " " + employee_name + " " + "on this" + " " + month + "-" + year + "', " +
                 "'" + branch + "') ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");

            msSQL = "select account_gid from hrm_mst_temployee where employee_gid='" + employee_gid + "' ";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);

            if (objODBCDataReader.HasRows)
            {
                objODBCDataReader.Read();
                account_gid = objODBCDataReader["account_gid"].ToString();
                objODBCDataReader.Close();

                double total = (grandtotal - statutory_amount) + other_addition - other_deduction;

                msSQL = " Insert into acc_trn_journalentrydtl " +
                                    " (journaldtl_gid, " +
                                    " journal_gid, " +
                                    " account_gid," +
                                    " journal_type," +
                                    " transaction_amount)" +
                                    " values (" +
                                    "'" + msGetDlGID + "', " +
                                    "'" + msGetGID + "'," +
                                    "'" + account_gid + "'," +
                                    "'cr'," +
                                    "'" + total + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            }


            else
            {
                objODBCDataReader.Close();
            }

            double debittotal = (grandtotal - statutory_amount) + other_addition - other_deduction;
            msGetdlGID = objcmnfunctions.GetMasterGID("FPCD");


            msSQL = " Insert into acc_trn_journalentrydtl " +
                         " (journaldtl_gid, " +
                         " journal_gid, " +
                         " account_gid," +
                         " journal_type," +
                         " transaction_amount)" +
                         " values (" +
                         "'" + msGetdlGID + "', " +
                         "'" + msGetGID + "'," +
                         "'FCOA000148'," +
                         "'dr'," +
                         "'" + debittotal + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        }


        public List<string> JournalFunction(DateTime invoice_date)
        {
            List<string> journal_monthandyear = new List<string>();
            DateTime finyear_start;
            string journal_month = null;
            string journal_Day = null;
            string journal_year = null;

            objdbconn.OpenConn();
            msSQL = "SELECT DATE_FORMAT(fyear_start, '%Y-%m-%d') AS fyear_start, finyear_gid FROM adm_mst_tyearendactivities " +
                    "ORDER BY finyear_gid DESC LIMIT 1";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                finyear_start = DateTime.Parse(objOdbcDataReader["fyear_start"].ToString());

                objOdbcDataReader.Close();

                msSQL = "SELECT TIMESTAMPDIFF(Month, '" + finyear_start.ToString("yyyy-MM-dd") + "', '" + invoice_date.ToString("yyyy-MM-dd") + "') AS month, " +
                        "TIMESTAMPDIFF(Day, '" + finyear_start.ToString("yyyy-MM-dd") + "', '" + invoice_date.ToString("yyyy-MM-dd") + "') AS day";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    journal_month = objOdbcDataReader["month"].ToString();
                    journal_Day = objOdbcDataReader["day"].ToString();
                    journal_year = finyear_start.Year.ToString();
                }
                objOdbcDataReader.Close();
            }
            objdbconn.CloseConn();

            if (journal_month.Length == 1)
            {
                journal_month = "0" + (int.Parse(journal_month) + 1);
            }
            else
            {
                journal_month = (int.Parse(journal_month) + 1).ToString();
            }

            if (journal_Day.Length == 1)
            {
                journal_Day = "0" + (int.Parse(journal_Day) + 1);
            }
            else
            {
                journal_Day = (int.Parse(journal_Day) + 1).ToString();
            }

            journal_monthandyear.Add(journal_year);
            journal_monthandyear.Add(journal_month);
            journal_monthandyear.Add(journal_Day);

            return journal_monthandyear;
        }


        public void DaGetBasicSalary(string employee_gid, MdlPayTrnSalaryManagement values)
        {
            try
            {
                msSQL = " select salary_mode from pay_trn_temployee2salarygradetemplate ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<basicsalarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new basicsalarylist
                        {
                            salary_mode = dt["salary_mode"].ToString(),

                        });
                        values.basicsalarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}



