using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.payroll.Models;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Configuration;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;

namespace ems.payroll.DataAccess
{
    public class DaPayTrnReportPayment
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msGetloangid;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string lsbranchname;
        public void DaPaymentSummary(MdlPayTrnReportPayment values)
        {
            try
            {
                
                msSQL = " select sum(net_salary) as payment_amount,count(employee_gid) as employee_count, payment_month, payment_year " +
              " from pay_trn_tpayment group by payment_year, payment_month order by payment_year desc,MONTH(STR_TO_DATE(substring(payment_month,1,3),'%b')) desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPaylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPaylist
                        {
                            Amount = dt["payment_amount"].ToString(),
                            Employee_count = dt["employee_count"].ToString(),
                            month = dt["payment_month"].ToString(),
                            year = dt["payment_year"].ToString(),

                        });
                        values.GetPaylist = getModuleList;
                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment report!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaPaymentSummarybasedondate(MdlPayTrnReportPayment values , string fromdate, string todate , string branch_name)
        {
            try
            {
                msSQL = "select branch_name from hrm_mst_tbranch where branch_gid='"+branch_name+"'";
                lsbranchname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select sum(net_salary) as payment_amount,count(employee_gid) as employee_count,payment_date, payment_month, payment_year " +
              " from pay_trn_tpayment"+
              " where payment_date between DATE_FORMAT(STR_TO_DATE('" + fromdate + "', '%d-%m-%Y'), '%Y-%m-%d') and DATE_FORMAT(STR_TO_DATE('" + todate + "', '%d-%m-%Y'), '%Y-%m-%d') and empbranch_gid = '"+branch_name+ "'" +
              " group by payment_year, payment_month order by payment_year desc,MONTH(STR_TO_DATE(substring(payment_month,1,3),'%b')) desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPaylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPaylist
                        {
                            Amount = dt["payment_amount"].ToString(),
                            Employee_count = dt["employee_count"].ToString(),
                            month = dt["payment_month"].ToString(),
                            year = dt["payment_year"].ToString(),

                        });
                        values.GetPaylist = getModuleList;
                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment report!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DareportPaymentExpand(string month, string year, MdlPayTrnReportPayment values)
        {
            try
            {
                
                msSQL = " select a.employee_gid,payment_year,payment_month,c.user_code,concat(c.user_firstname,' ',c.user_lastname) as employee_name, " +
                    " d.designation_name,e.department_name, " +
                    " a.net_salary from pay_trn_tpayment a " +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                    " left join adm_mst_tdesignation d on b.designation_gid=d.designation_gid " +
                    " left join hrm_mst_tdepartment e on b.department_gid=e.department_gid " +
                    " where a.payment_month ='"+month+"' and a.payment_year = '"+year+"' group by employee_gid order by c.user_firstname asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetreportExpand>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetreportExpand
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            net_salary = dt["net_salary"].ToString(),

                        });
                        values.GetreportExpand = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment expand!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetPaymentmodeExpand(MdlPayTrnReportPayment values)
        {
            try
            {

                msSQL = " select date_format(a.payment_date,'%d-%m-%Y') as payment_date,a.payment_month,b.payment_type,a.payment_year, " +
                            "  case when b.payment_type='Cash' then b.payment_type else concat(a.cheque_bank,'/',b.payment_type) end as modeof_payment, " +
                            " count( distinct a.employee_gid) as no_of_employees,format(sum(a.net_salary),2) as paid_amount,concat(ifnull(c.user_firstname,''),' ',ifnull(c.user_lastname,'')) as paid_by,  " +
                            " a.payment_type as modeofpayment_gid,a.cheque_bank,group_concat('\'',a.payment_gid,'\'') as payment_gid from pay_trn_tpayment a " +
                            " left join pay_mst_tmodeofpayment b on a.payment_type=b.modeofpayment_gid " +
                            " left join adm_mst_tuser c on a.issued_by=c.user_gid " +
                            " where 1=1  group by a.cheque_bank,a.payment_date,a.payment_type ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPaymentmode>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPaymentmode
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            paid_amount = dt["paid_amount"].ToString(),
                            no_of_employees = dt["no_of_employees"].ToString(),
                            modeof_payment = dt["modeof_payment"].ToString(),
                            payment_type = dt["payment_type"].ToString(),
                            paid_by = dt["paid_by"].ToString(),

                        });
                        values.getpaymentmode = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Salary Payment expand!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetReportExportExcel(MdlPayTrnReportPayment values)
        {
            try
            {
                msSQL = " select sum(net_salary) as payment_amount,count(employee_gid) as employee_count, payment_month, payment_year " +
              " from pay_trn_tpayment group by payment_year, payment_month order by payment_year desc,MONTH(STR_TO_DATE(substring(payment_month,1,3),'%b')) desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                string lscompany_code = string.Empty;
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Payment Report");
                try
                {
                    msSQL = " select company_code from adm_mst_tcompany";

                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    string lspath = ConfigurationManager.AppSettings["exportexcelfile"] + "/Payment/export" + "/" + lscompany_code + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                    //values.lspath = ConfigurationManager.AppSettings["file_path"] + "/erp_documents" + "/" + lscompany_code + "/" + "SDC/TestReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                    {
                        if ((!System.IO.Directory.Exists(lspath)))
                            System.IO.Directory.CreateDirectory(lspath);
                    }

                    string lsname = "payment_Report" + DateTime.Now.ToString("(dd-MM-yyyy HH-mm-ss)") + ".xlsx";
                    string lspath1 = ConfigurationManager.AppSettings["exportexcelfile"] + "/Payment/export" + "/" + lscompany_code + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsname;

                    workSheet.Cells[1, 1].LoadFromDataTable(dt_datatable, true);
                    FileInfo file = new FileInfo(lspath1);
                    using (var range = workSheet.Cells[1, 1, 1, 8])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                    }
                    excel.SaveAs(file);

                    var getModuleList = new List<GetreportExportExcel>();
                    if (dt_datatable.Rows.Count != 0)
                    {

                        getModuleList.Add(new GetreportExportExcel
                        {

                            lsname = lsname,
                            lspath1 = lspath1,



                        });
                        values.GetreportExportExcel = getModuleList;

                    }
                    dt_datatable.Dispose();
                    values.status = true;
                    values.message = "Success";
                }
                catch (Exception ex)
                {
                    values.status = false;
                    values.message = "Failure";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment expand!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetLastSixMonths_List(MdlPayTrnReportPayment values)
        {
            try
            {
                msSQL = " select a.payment_gid,count(a.employee_gid)as employeecount,date_format(payment_date,'%b-%Y') as payment_date,substring(date_format(a.payment_date,'%M'),1,3)as month, " +
                        " year(a.payment_date) as year,format(round(sum(a.payable_amount),2),2)as amount,count(a.payment_gid)as paymentcount ,date_format(payment_date,'%M/%Y') as month_wise " +
                        " from pay_trn_tpayment a " +
                        " where a.payment_date > date_add(now(), interval-6 month) and a.payment_date<=date(now()) " +
                        " group by date_format(a.payment_date,'%M') order by a.payment_date desc  ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetLastSixMonths_List = new List<GetLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetLastSixMonths_List.Add(new GetLastSixMonths_List
                        {
                            payment_date = (dt["payment_date"].ToString()),
                            payment_gid = (dt["payment_gid"].ToString()),
                            paymentcount = (dt["paymentcount"].ToString()),
                            month_wise = (dt["month_wise"].ToString()),
                            amount = (dt["amount"].ToString()),
                            employeecount = (dt["employeecount"].ToString()),
                            month = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                        });
                        values.GetLastSixMonths_List = GetLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Report For Last Six Months !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetPaymentReportForLastSixMonthsSearch(MdlPayTrnReportPayment values, string from, string to)
        {
            try
            {
                if (from == null && to == null)
                {
                    msSQL = " select  a.payment_gid,count(a.employee_gid)as employeecount,date_format(payment_date,'%b-%Y') as payment_date,substring(date_format(a.payment_date,'%M'),1,3)as month, " +
                            " format(round(sum(a.payable_amount),2),2)as amount,count(a.payment_gid)as paymentcount ,date_format(payment_date,'%M/%Y') as month_wise,year(a.payment_date) as year " +
                            " from pay_trn_tpayment a " +
                            " where a.payment_date > date_add(now(), interval-6 month) and a.payment_date<=date(now()) " +
                            " group by date_format(a.payment_date,'%M') order by a.payment_date desc  ";
                }
                else
                {
                    msSQL = " select  a.payment_gid,count(a.employee_gid)as employeecount,date_format(payment_date,'%b-%Y') as payment_date,substring(date_format(a.payment_date,'%M'),1,3)as month, " +
                             " format(round(sum(a.payable_amount),2),2)as amount,count(a.payment_gid)as paymentcount ,date_format(payment_date,'%M/%Y') as month_wise,year(a.payment_date) as year " +
                             " from pay_trn_tpayment a   " +
                             " where a.payment_date between '" + from + "' and '" + to + "' " +
                             " group by date_format(a.payment_date,'%M') order by a.payment_date desc ";

                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetLastSixMonths_List = new List<GetLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetLastSixMonths_List.Add(new GetLastSixMonths_List
                        {
                            payment_date = (dt["payment_date"].ToString()),
                            payment_gid = (dt["payment_gid"].ToString()),
                            paymentcount = (dt["paymentcount"].ToString()),
                            month_wise = (dt["month_wise"].ToString()),
                            amount = (dt["amount"].ToString()),
                            employeecount = (dt["employeecount"].ToString()),
                            month = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                        });
                        values.GetLastSixMonths_List = GetLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetEmployeeDetailsSummary(string employee_gid, string month_wise, MdlPayTrnReportPayment values)
        {
            try
            {
                msSQL = " select a.employee_gid,payment_year,payment_month,c.user_code,concat(c.user_firstname,' ',c.user_lastname) as employee_name, " +
                    " d.designation_name,e.department_name, " +
                    " a.net_salary from pay_trn_tpayment a " +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                    " left join adm_mst_tdesignation d on b.designation_gid=d.designation_gid " +
                    " left join hrm_mst_tdepartment e on b.department_gid=e.department_gid " +
                    " where date_format(a.payment_date,'%M/%Y') ='" + month_wise + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetEmployeeDetailsSummary = new List<GetEmployeeDetailsSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetEmployeeDetailsSummary.Add(new GetEmployeeDetailsSummary
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                        });
                        values.GetEmployeeDetailsSummary = GetEmployeeDetailsSummary;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Detail Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


    }
}