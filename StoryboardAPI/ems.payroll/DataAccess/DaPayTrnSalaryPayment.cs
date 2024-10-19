using ems.payroll.Models;

using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using Bytescout.Spreadsheet;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Collections.Specialized.BitVector32;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Globalization;
using System.Net.Http;
using System.Data.Common;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using OfficeOpenXml.Style;



namespace ems.payroll.DataAccess
{
    public class DaPayTrnSalaryPayment
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msGetloangid, formattedpayment_date, LSTransactionCode;
        string  msgetpayment_gid,account_gid, LSTransactionType, LSReferenceGID, lslimit_flag;
        OdbcDataReader objMySqlDataReader, objMySqlDataReader2;
        double lspayable_amount;
        DataTable dt_datatable;
        int mnResult;
        double lspaidamount1 =0;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        finance_cmnfunction objfinance_cmnfunction=new finance_cmnfunction();


        public void DaGetSalaryPaymentSummary(MdlPayTrnSalaryPayment values)
        {
            try
            {
                
                msSQL = " select x.month as payment_month,x.sal_year as payment_year,y.payment_gid, " +
                   " case when (select a.month_workingdays from pay_trn_tsalary a where x.month=a.month and x.sal_year=a.year group by a.month,a.year) is null then '0' else " +
                   " (select a.month_workingdays from pay_trn_tsalary a where x.month=a.month and x.sal_year=a.year group by a.month,a.year) end as total_working_days, " +
                   " cast(case when (select count(b.employee_gid) as no_of_employee from pay_trn_tsalary b where x.month=b.month and x.sal_year=b.year and b.payrun_flag='Y' group by b.month,b.year) is null then '0' else " +
                   " (select count(b.employee_gid) as no_of_employee from pay_trn_tsalary b where x.month=b.month and x.sal_year=b.year and b.payrun_flag='Y' group by b.month,b.year) end as char)as no_of_employee, " +
                   " case when (select format(sum(d.earned_net_salary),2) from pay_trn_tsalary d " +
                   " where x.month=d.month and x.sal_year=d.year and d.payrun_flag='Y' group by d.month,d.year) is null then 'Not Generated' " +
                   " else (select format(sum(d.earned_net_salary),2) " +
                   " from pay_trn_tsalary d where x.month=d.month and x.sal_year=d.year and d.payrun_flag='Y' group by d.month,d.year) end as total_salary, " +
                   " cast(case when count(distinct y.employee_gid) is null then 'Not Generated' else count(distinct y.employee_gid) end as char)as paid_employee_count, " +
                   " case when format(sum(y.net_salary),2) is null then'Not Generated' else  format(sum(net_salary),2) end as salary_disposed, " +
                   " case when format(((select sum(f.earned_net_salary) from pay_trn_tsalary f where f.month=x.month and f.year=x.sal_year  and f.payrun_flag='Y' group by f.month,f.year)- " +
                   " (select sum(d.net_salary) from pay_trn_tpayment d where d.payment_month=x.month and d.payment_year=x.sal_year " +
                   " group by d.payment_month,d.payment_year)),2) is null then 'Not Generated'  else " + " format(((select sum(f.earned_net_salary) from pay_trn_tsalary f where f.month=x.month and f.year=x.sal_year and f.payrun_flag='Y' group by f.month,f.year)- " +
                   " (select sum(d.net_salary) from pay_trn_tpayment d where d.payment_month=x.month and d.payment_year=x.sal_year " +
                   " group by d.payment_month,d.payment_year)),2) end as outstanding_amount " +
                   " from pay_trn_tsalmonth x " +
                   " left join pay_trn_tpayment y on x.month=y.payment_month and x.sal_year=y.payment_year " +
                   " group by x.month,x.sal_year  order by x.sal_year desc,MONTH(STR_TO_DATE(substring(x.month,1,3),'%b')) desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPaymentlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPaymentlist
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            payment_month = dt["payment_month"].ToString(),
                            payment_year = dt["payment_year"].ToString(),
                            total_working_days = dt["total_working_days"].ToString(),
                            no_of_employee = dt["no_of_employee"].ToString(),
                            total_salary = dt["total_salary"].ToString(),
                            paid_employee_count = dt["paid_employee_count"].ToString(),
                            salary_disposed = dt["salary_disposed"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString()


                        });
                        values.paymentlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
        }


        public void Dagetonchangebankname(string month, string year, string bankname, MdlPayTrnSalaryPayment values)
        {

            try
            {

                string lslimit_amount, lsactual_month_workingdays, lsemployee_gid, lspaid_amount, lspayable_amount, lsmonthworking_days;
                double lslimit_amount1, lsoutstanding_amount1 = 0.0;
                string lsoutstanding_amount = "", lslimit_flag = "";


                msSQL = " select f.user_code,a.salary_gid,actual_month_workingdays,concat(ifnull(f.user_firstname,''),' ',ifnull(f.user_lastname,'')) " +
                        " as employee_name,c.branch_name,b.branch_gid,b.employee_gid, " +
                        " d.department_name,a.earned_net_salary from pay_trn_tsalary a " +
                        " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                        " left join hrm_mst_tbranch c on b.branch_gid=c.branch_gid " +
                        " left join hrm_mst_tdepartment d on b.department_gid=d.department_gid " +
                        " left join adm_mst_tuser f on b.user_gid=f.user_gid " +
                        " where a.month='" + month + "' and a.year='" + year + "' and b.bank = '" + bankname + "'" +
                        " and a.payrun_flag='Y'  and a.payment_flag!='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMakePaymentlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        lsemployee_gid = dt["employee_gid"].ToString();


                        msSQL = "select * from pay_trn_tpaymentlimit where employee_gid='" + lsemployee_gid +
                          "' and month='" + month + "'" +
                          " and year='" + year + "' ";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsoutstanding_amount = objMySqlDataReader["outstanding_amount"].ToString();
                            lslimit_flag = objMySqlDataReader["limit_flag"].ToString();
                            lsoutstanding_amount1 = double.Parse(lsoutstanding_amount);
                        }

                        objMySqlDataReader.Close();

                        getModuleList.Add(new GetMakePaymentlist
                        {
                            
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            earned_net_salary = dt["earned_net_salary"].ToString(),
                            outstanding_amount = double.Parse((lsoutstanding_amount1).ToString("N")),
                        });
                        values.makepaymentlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetEmployeeBankDtl(MdlPayTrnSalaryPayment values)
        {
            try
            {
               
                msSQL = "select concat(bank_prefix_code,' ','/',' ',bank_name) as bank_name From acc_mst_tallbank";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEmployeeBankdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEmployeeBankdropdown
                        {
                            bank_name = dt["bank_name"].ToString(),

                        });
                        values.getemployeebankdtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objdbconn.OpenConn();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Employee Bank details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetEmployeePayFromBank(MdlPayTrnSalaryPayment values)
        {
            try
            {

                msSQL = "select bank_gid,bank_name from acc_mst_tbank";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeebankdtl>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeebankdtl
                        {
                            bank_gid = dt["bank_gid"].ToString(),
                            bank_name = dt["bank_name"].ToString(),

                        });
                        values.employeebankdtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objdbconn.OpenConn();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Employee Bank details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetSalaryPaymentExpand(string month, string year, MdlPayTrnSalaryPayment values)
        {
            try
            {
                
                msSQL = " select date_format(a.payment_date,'%d-%m-%Y') as payment_date,a.payment_month,b.payment_type,a.payment_year, " +
                            "  case when b.payment_type='Cash' then b.payment_type else concat(a.cheque_bank,'/',b.payment_type) end as modeof_payment, " +
                            " count( distinct a.employee_gid) as no_of_employees,format(sum(a.net_salary),2) as paid_amount,concat(ifnull(c.user_firstname,''),' ',ifnull(c.user_lastname,'')) as paid_by,  " +
                            " a.payment_type as modeofpayment_gid,a.cheque_bank,a.payment_gid from pay_trn_tpayment a " +
                            " left join pay_mst_tmodeofpayment b on a.payment_type=b.modeofpayment_gid " +
                            " left join adm_mst_tuser c on a.issued_by=c.user_gid " +
                            " where a.payment_month='" + month + "' and a.payment_year='" + year + "'  group by a.cheque_bank,a.payment_date,a.payment_type ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPayment>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPayment
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            paid_amount = dt["paid_amount"].ToString(),
                            no_of_employees = dt["no_of_employees"].ToString(),
                            modeof_payment = dt["modeof_payment"].ToString(),
                            payment_type = dt["payment_type"].ToString(),
                            paidbybank = dt["cheque_bank"].ToString(),
                            paid_by = dt["paid_by"].ToString(),

                        });
                        values.getpayment = getModuleList;
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


        public void DaGetSalaryPaymentExpand2(string month, string year, string payment_date, string modeof_payment, MdlPayTrnSalaryPayment values)
        {
            try
            {
                
                msSQL = " select f.user_code,concat(ifnull(f.user_firstname,''),' ',ifnull(f.user_lastname,'')) as employee_name,c.branch_name,date_format(a.payment_date, '%d-%m-%Y') as payment_date ,format(a.net_salary,2) as payment_amount, " +
                    " format(a.net_salary,2) as paid_amount,d.department_name,e.designation_name,g.payment_type,a.payment_gid,a.payment_month,a.payment_year from pay_trn_tpayment a " +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " left join hrm_mst_tbranch c on b.branch_gid=c.branch_gid " +
                    " left join hrm_mst_tdepartment d on b.department_gid=d.department_gid " +
                    " left join adm_mst_tdesignation e on b.designation_gid=e.designation_gid " +
                    " left join adm_mst_tuser f on b.user_gid=f.user_gid " +
                    " left join pay_mst_tmodeofpayment g on a.payment_type=g.modeofpayment_gid " +
                    " where a.payment_month='" + month + "' and a.payment_year='" + year + "' and a.payment_date='" + (payment_date) + "' " +
                    " and g.payment_type='" + modeof_payment + "' order by f.user_firstname asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPayment1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPayment1
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            paid_amount = dt["paid_amount"].ToString(),
                            payment_type = dt["payment_type"].ToString(),


                        });
                        values.getpayment1 = getModuleList;
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

        public void Dagetsalarypaymentedit(string payment_type , string payment_date , string payment_month, string payment_year, MdlPayTrnSalaryPayment values )
        {
            try
            {
                msSQL = "select modeofpayment_gid from pay_mst_tmodeofpayment where payment_type='" + payment_type + "'";
                string lspayment_type = objdbconn.GetExecuteScalar(msSQL);



                msSQL = " select f.user_code,concat(ifnull(f.user_firstname,''),' ',ifnull(f.user_lastname,'')) as employee_name,c.branch_name,date_format(a.payment_date, '%d-%m-%Y') as payment_date ,format(a.net_salary,2) as payment_amount, a.employee_gid, " +
                    " format(a.net_salary,2) as paid_amount,d.department_name,e.designation_name,g.payment_type,a.payment_gid,a.payment_month,a.payment_year from pay_trn_tpayment a " +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " left join hrm_mst_tbranch c on b.branch_gid=c.branch_gid " +
                    " left join hrm_mst_tdepartment d on b.department_gid=d.department_gid " +
                    " left join adm_mst_tdesignation e on b.designation_gid=e.designation_gid " +
                    " left join adm_mst_tuser f on b.user_gid=f.user_gid " +
                    " left join pay_mst_tmodeofpayment g on a.payment_type=g.modeofpayment_gid " +
                    " where a.payment_month='" + payment_month + "' and a.payment_year='" + payment_year + "' and a.payment_date='" + (payment_date) + "' " +
                    " and g.modeofpayment_gid='" + lspayment_type + "' order by f.user_firstname asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salaryedit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salaryedit_list
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            net_salary = dt["paid_amount"].ToString(),
                            payment_type = dt["payment_type"].ToString(),
                            payment_month=payment_month,
                            payment_year=payment_year
                        });
                        values.salaryedit_list = getModuleList;
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


        public void Dagetdeletepayment(string payment_type, string payment_date, string payment_month, string payment_year, string paid_bank, MdlPayTrnSalaryPayment values)
        {
            try
            {
                string lslimit_amount = "";
                string lspaid_amount = "";
                msSQL = "select modeofpayment_gid from pay_mst_tmodeofpayment where payment_type='" + payment_type+ "'";
                string lspayment_type = objdbconn.GetExecuteScalar(msSQL);

                if (payment_type.ToUpper() ==  ("Cash").ToUpper())
                {
                    msSQL = " select distinct b.paymentcount_gid,a.employee_gid,a.payment_gid from pay_trn_tpayment a " +
                      " left join pay_trn_tpaymentcount b on a.paymentcount_gid=b.paymentcount_gid " +
                       " where a.payment_date='" + payment_date + "' and a.payment_type='" + lspayment_type + "' and a.payment_flag='Y' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                    if (objMySqlDataReader.HasRows == true)
                    {
                        msSQL = "delete from pay_trn_tpaymentcount where paymentcount_gid='" + objMySqlDataReader["paymentcount_gid"].ToString() + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "delete from pay_trn_tpayment where payment_gid='" + objMySqlDataReader["payment_gid"].ToString() + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "select limit_amount from pay_trn_tpaymentlimit where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "' ";
                            lslimit_amount = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = "select ifnull(sum(earned_net_salary),0.00) from pay_trn_tsalary where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "' ";
                            lspaid_amount = objdbconn.GetExecuteScalar(msSQL);

                            double lslimitamount = double.Parse(lslimit_amount);
                            double lspaidamount = double.Parse(lspaid_amount);

                            if (lspaidamount < lslimitamount)
                            {

                                msSQL = "update pay_trn_tpaymentlimit set limit_flag='N' where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                msSQL = "update pay_trn_tsalary set limit_flag='N',payment_flag='N' where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            }
                            else if (lspaidamount == lslimitamount)
                            {
                                msSQL = "update pay_trn_tsalary set limit_flag='N',payment_flag='N' where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            values.status = true;

                        }

                    }

                    objMySqlDataReader.Close();
                }
                else
                {
                    msSQL = " select distinct b.paymentcount_gid,a.employee_gid,a.payment_gid from pay_trn_tpayment a " +
                 " left join pay_trn_tpaymentcount b on a.paymentcount_gid=b.paymentcount_gid " +
                 " where a.payment_date='" + payment_date + "' and a.payment_type='" + lspayment_type + "'" +
                 " and a.cheque_bank='" + paid_bank + "' and a.payment_flag='Y' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                    //if (objMySqlDataReader.HasRows == true)
                    //{
                    //    msSQL = "delete from pay_trn_tpaymentcount where paymentcount_gid='" + objMySqlDataReader["paymentcount_gid"].ToString() + "' ";
                    //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //    if(mnResult == 1)
                    //    {
                    //        msSQL = "delete from pay_trn_tpayment where payment_gid='" + objMySqlDataReader["payment_gid"].ToString() + "' ";
                    //        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    //        msSQL = "select limit_amount from pay_trn_tpaymentlimit  where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "' ";
                    //        lslimit_amount = objdbconn.GetExecuteScalar(msSQL);

                    //        msSQL = "select ifnull(sum(earned_net_salary),0.00) from pay_trn_tsalary where month='" +payment_month + "' and year='" +payment_year+ "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "' ";
                    //        lspaid_amount = objdbconn.GetExecuteScalar(msSQL);

                    //        double lslimitamount = double.Parse(lslimit_amount);
                    //        double lspaidamount = double.Parse(lspaid_amount);


                    //        if (lspaidamount < lslimitamount)
                    //        {
                    //            msSQL = "update pay_trn_tpaymentlimit set limit_flag='N' where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "'";
                    //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    //            msSQL = "update pay_trn_tsalary set limit_flag='N',payment_flag='N' where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "'";
                    //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //        }
                    //        else if (lspaidamount == lslimitamount)
                    //        {
                    //            msSQL = "update pay_trn_tsalary set limit_flag='N',payment_flag='N' where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "'";
                    //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //        }
                    //        values.status = true;
                    //    }

                    //}
                    while (objMySqlDataReader.Read()) // Loop through all records
                    {
                        // Delete from pay_trn_tpaymentcount
                        msSQL = "delete from pay_trn_tpaymentcount where paymentcount_gid='" + objMySqlDataReader["paymentcount_gid"].ToString() + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            // Delete from pay_trn_tpayment
                            msSQL = "delete from pay_trn_tpayment where payment_gid='" + objMySqlDataReader["payment_gid"].ToString() + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            // Fetch limit and paid amounts for the employee
                            msSQL = "select limit_amount from pay_trn_tpaymentlimit where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "' ";
                            lslimit_amount = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = "select ifnull(sum(earned_net_salary), 0.00) from pay_trn_tsalary where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "' ";
                            lspaid_amount = objdbconn.GetExecuteScalar(msSQL);

                            double lslimitamount = double.Parse(lslimit_amount);
                            double lspaidamount = double.Parse(lspaid_amount);

                            // Update flags based on comparison
                            if (lspaidamount < lslimitamount || lspaidamount == lslimitamount)
                            {
                                msSQL = "update pay_trn_tpaymentlimit set limit_flag='N' where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "update pay_trn_tsalary set limit_flag='N', payment_flag='N' where month='" + payment_month + "' and year='" + payment_year + "' and employee_gid='" + objMySqlDataReader["employee_gid"] + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }

                    // Set the status after the loop
                    values.status = true;

                }
                objMySqlDataReader.Close();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Salary Payment expand!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public Dictionary<string, object> DaExportexcelbankdetails(string user_gid , string payment_date, string paidbybank, string payment_type, MdlPayTrnSalaryPayment values)
        {
            msSQL = "select CONCAT(user_firstname,'',user_lastname) as username from adm_mst_tuser where user_gid ='" + user_gid + "'";
            string lsusername = objdbconn.GetExecuteScalar(msSQL);
            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
             string lscompany_code = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT a.company_name FROM adm_mst_tcompany a ";
            string lscompany_name = objdbconn.GetExecuteScalar(msSQL);

            string inputDate = payment_date;
                DateTime uiDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string payment_date1 = uiDate.ToString("yyyy-MM-dd");

                msSQL = "select modeofpayment_gid from pay_mst_tmodeofpayment where payment_type = '" + payment_type + "'";
                string lspayment_type = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select account_no,bank_branch,cheque_number,cheque_bank, cast(monthname(payment_date)as char) as payment_month,year(payment_date) as payment_year from pay_trn_tpayment  where payment_date='" + payment_date1 + "' and payment_flag='Y' " +
                      " and payment_type='" + lspayment_type + "' and cheque_bank='" + paidbybank + "' limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                string payment_month = "";
                string payment_year = "";
                string cheque_bank = "";
                string cheque_number = "";
            if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                       string account_no = dt["account_no"].ToString();
                       cheque_bank = dt["cheque_bank"].ToString();
                       payment_month = dt["payment_month"].ToString();
                       payment_year = dt["payment_year"].ToString();
                       cheque_number = dt["cheque_number"].ToString();

                    }
                }
                dt_datatable.Dispose();
                //spreadsheet creation
                Spreadsheet document = new Spreadsheet();
                Worksheet sheet = document.Workbook.Worksheets.Add("ExcelSheet1");
            //Spreadsheet Styling
            sheet.Range("A1:C1").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell("A2").Font = new Font("Arial",10,FontStyle.Bold);
                sheet.Cell("A4").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell("A5").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell("A12").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell("B12").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell("C12").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell("D12").Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Range("A12:D12").BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                sheet.Range("A12:D12").TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                sheet.Range("A12:D12").RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                sheet.Range("A12:D12").LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                sheet.Columns[0].Width = 130;
                sheet.Columns[1].Width = 170;
                sheet.Columns[2].Width = 230;
                sheet.Columns[3].Width = 70;
                sheet.ViewOptions.ShowGridLines = false;
                sheet.Range("A1:C1").Merge();
                sheet.Cell(0,0).AlignmentHorizontal = Bytescout.Spreadsheet.Constants.AlignmentHorizontal.Centered;
                sheet.Range("A2:C2").Merge();
                sheet.Cell(1,0).AlignmentHorizontal = Bytescout.Spreadsheet.Constants.AlignmentHorizontal.Centered;


            //data inserting in respective cells
            sheet.Cell(0,0).Value = "Monthly Salary - " + payment_month + " - " + payment_year;
                sheet.Cell(1,0).Value = "Bank -" + cheque_bank;
                sheet.Cell("A4").Value = "Date :"+ inputDate;
                sheet.Cell("A5").Value = "From :"+ lscompany_name;
                sheet.Cell("A7").Value = "Dear Sir/Madam,";
                sheet.Cell("A8").Value = "We have current account with " + cheque_bank + " with the following name and account number.";
                sheet.Cell("A9").Value = "Kindly credit the salary for the following employees from the cheque No :" + cheque_number;
                sheet.Cell("A12").Value = "Employee Code";
                sheet.Cell("B12").Value = "Employee Name";
                sheet.Cell("C12").Value = "Employee's Account Number";
                sheet.Cell("D12").Value = "Salary";


            msSQL = "select sum(net_salary) as totalpaidamount from pay_trn_tpayment where payment_date = '" + payment_date1 + "' and payment_type='" + lspayment_type + "'";
            string totalamount = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select f.user_code,concat(ifnull(f.user_firstname,''),' ',ifnull(f.user_lastname,'')) as employee_name,c.branch_name,date_format(a.payment_date, '%d-%m-%Y') as payment_date ,format(a.net_salary,2) as payment_amount, " +
                  " format(a.net_salary,2) as paid_amount, d.department_name,e.designation_name,g.payment_type,a.employee_accountno,a.payment_gid,a.payment_month,a.payment_year from pay_trn_tpayment a " +
                  " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                  " left join hrm_mst_tbranch c on b.branch_gid=c.branch_gid " +
                  " left join hrm_mst_tdepartment d on b.department_gid=d.department_gid " +
                  " left join adm_mst_tdesignation e on b.designation_gid=e.designation_gid " +
                  " left join adm_mst_tuser f on b.user_gid=f.user_gid " +
                  " left join pay_mst_tmodeofpayment g on a.payment_type=g.payment_type " +
                  " where a.payment_date='" + (payment_date1) + "' " +
                  " and a.payment_type='" + lspayment_type + "' order by f.user_firstname asc";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            //table data inserting based on rows from db
            int cellrowindex;
            if (dt_datatable.Rows.Count != 0)
            {
                cellrowindex = 12;

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    int cellcloumnindex = 0;        

                    sheet.Cell(cellrowindex , 0 + cellcloumnindex).Value = dt["user_code"].ToString();
                    sheet.Cell(cellrowindex, 0 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 0 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 0 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 0 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 1 + cellcloumnindex).Value = dt["employee_name"].ToString();
                    sheet.Cell(cellrowindex, 1 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 1 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 1 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 1 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 2 + cellcloumnindex).Value = dt["employee_accountno"].ToString();
                    sheet.Cell(cellrowindex, 2 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 2 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 2 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 2 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 3 + cellcloumnindex).Value = dt["paid_amount"].ToString();
                    sheet.Cell(cellrowindex, 3 + cellcloumnindex).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 3 + cellcloumnindex).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 3 + cellcloumnindex).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    sheet.Cell(cellrowindex, 3 + cellcloumnindex).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;


                    cellrowindex++;
                }

                int endRowIndex = cellrowindex;
                sheet.Cell(endRowIndex + 1, 0).Value = "Total Amount:Rs."+ totalamount +.00;
                sheet.Cell(endRowIndex + 1, 0).Font = new Font("Arial", 10, FontStyle.Bold);
                sheet.Cell(endRowIndex + 2,0).Value = "Regards";
                sheet.Cell(endRowIndex + 3, 0).Value = lsusername;

            }

            string lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "ExportExcel/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
            if (!System.IO.Directory.Exists(lspath))
            {
                System.IO.Directory.CreateDirectory(lspath);
            }
            string file_name = "Monthly_Salary" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string excelFilePath = Path.Combine(lspath, file_name);

            document.SaveAsXLSX(excelFilePath + ".xlsx");
            document.Close();

            var ls_response = reportexcelfileStreamDownload(excelFilePath + ".xlsx");
            return ls_response;
        }
        public Dictionary<string, object> reportexcelfileStreamDownload(string path)
        {
            var ls_response = new Dictionary<string, object>();
            string file_name = Path.GetFileName(path);
            string file_format = Path.GetExtension(file_name);
            string file_name_extension = Path.GetFileNameWithoutExtension(file_name);

            // Load the Excel file using EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorkbook workbook = package.Workbook;

                // Ensure the workbook is not null and contains at least one worksheet
                if (workbook != null && workbook.Worksheets.Count > 0)
                {
                    // Get the first worksheet
                    ExcelWorksheet firstSheet = workbook.Worksheets.First();


                    // Create a new Excel package for modified content
                    using (ExcelPackage modifiedPackage = new ExcelPackage())
                    {
                        // Add a new worksheet to the modified package
                        ExcelWorksheet modifiedSheet = modifiedPackage.Workbook.Worksheets.Add("Sheet1");

                        // Copy cell values and styles
                        for (int row = 1; row <= firstSheet.Dimension.Rows; row++)
                        {
                            for (int col = 1; col <= firstSheet.Dimension.Columns; col++)
                            {
                                // Copy cell value
                                modifiedSheet.Cells[row, col].Value = firstSheet.Cells[row, col].Value;

                                // Clone cell style
                                modifiedSheet.Cells[row, col].Style.Font.Bold = firstSheet.Cells[row, col].Style.Font.Bold;
                                modifiedSheet.Cells[row, col].Style.Font.Italic = firstSheet.Cells[row, col].Style.Font.Italic;
                                modifiedSheet.Cells[row, col].Style.Font.UnderLine = firstSheet.Cells[row, col].Style.Font.UnderLine;             
                                modifiedSheet.Cells[row, col].Style.Fill.PatternType = firstSheet.Cells[row, col].Style.Fill.PatternType;
                                modifiedSheet.Cells[row, col].Style.Border.Top.Style = firstSheet.Cells[row, col].Style.Border.Top.Style;
                                modifiedSheet.Cells[row, col].Style.Border.Bottom.Style = firstSheet.Cells[row, col].Style.Border.Bottom.Style;
                                modifiedSheet.Cells[row, col].Style.Border.Left.Style = firstSheet.Cells[row, col].Style.Border.Left.Style;
                                modifiedSheet.Cells[row, col].Style.Border.Right.Style = firstSheet.Cells[row, col].Style.Border.Right.Style;
                                modifiedSheet.Cells[row, col].Style.Fill.PatternType = firstSheet.Cells[row, col].Style.Fill.PatternType;
                                if (firstSheet.Cells[row, col].Style.Font.Color.Rgb != null)
                                {
                                    OfficeOpenXml.Style.ExcelColor excelFontColor = firstSheet.Cells[row, col].Style.Font.Color;
                                    System.Drawing.Color fontColor = System.Drawing.ColorTranslator.FromHtml(excelFontColor.Rgb);
                                    modifiedSheet.Cells[row, col].Style.Font.Color.SetColor(fontColor);
                                }
                                modifiedSheet.Column(col).Width = firstSheet.Column(col).Width;
                                modifiedSheet.Cells[row, col].Style.WrapText = firstSheet.Cells[row, col].Style.WrapText;
                                modifiedSheet.Cells[row, col].Style.HorizontalAlignment = firstSheet.Cells[row, col].Style.HorizontalAlignment;
                                modifiedSheet.Cells[row, col].Style.VerticalAlignment = firstSheet.Cells[row, col].Style.VerticalAlignment;
                            }
                        }
                        modifiedSheet.Cells[1,1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        modifiedSheet.View.ShowGridLines = false;


                        // Save the modified package to a memory stream
                        MemoryStream ms = new MemoryStream();
                        modifiedPackage.SaveAs(ms);

                        // Convert the memory stream to a byte array
                        byte[] bytes = ms.ToArray();


                        ls_response.Add("FileName", file_name);
                        ls_response.Add("FileFormat", file_format);
                        ls_response.Add("FileBytes", bytes);

                        ls_response = objFnazurestorage.ConvertDocumentToByteArray(ms, file_name_extension, file_format);
                    }
                }
            }

            return ls_response;
        }


        public void DaGetMakePaymentSummary(string month, string year, string user_gid, MdlPayTrnSalaryPayment values)
        {
            try
            {

                string lslimit_amount, lsactual_month_workingdays, lsemployee_gid, lsmonthworking_days;
                double lspaid_amount, lspayable_amount, lslimit_amount1, lsoutstanding_amount1 = 0.0;
                string lsoutstanding_amount = "", lslimit_flag="";

                msSQL = " select f.user_code,a.salary_gid,actual_month_workingdays,concat(ifnull(f.user_firstname,''),' ',ifnull(f.user_lastname,'')) " +
                        " as employee_name,c.branch_name,b.branch_gid,b.employee_gid, " +
                    " d.department_name,a.earned_net_salary,a.net_salary from pay_trn_tsalary a " +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " left join hrm_mst_tbranch c on b.branch_gid=c.branch_gid " +
                    " left join hrm_mst_tdepartment d on b.department_gid=d.department_gid " +
                    " left join adm_mst_tuser f on b.user_gid=f.user_gid " +
                    " where a.month='" + month + "' and a.year='" + year + "'" +
                    " and a.payrun_flag='Y'  and a.payment_flag!='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMakePaymentlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        lsemployee_gid = dt["employee_gid"].ToString();
                        lsactual_month_workingdays = dt["actual_month_workingdays"].ToString();
                        lspayable_amount = double.Parse(dt["earned_net_salary"].ToString());
                        lspaid_amount = double.Parse(dt["net_salary"].ToString());
                        msSQL = " select sum(a.earned_net_salary) from pay_trn_tsalary a " +
                        " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                        " where b.employee_gid='" + lsemployee_gid + "'  " +
                        " and a.year<='" + year + "'  and a.payrun_flag='Y' group by b.employee_gid";
                        lslimit_amount = objdbconn.GetExecuteScalar(msSQL);

                        string limitamount = "";
                        msSQL = " select sum(a.net_salary) as paid_amount from  pay_trn_tpayment a " +
                                " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                                " where b.employee_gid='" + lsemployee_gid + "'   " +
                                " and a.payment_year<='" + year + "' group by b.employee_gid";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == false)
                        {
                            limitamount = "0";
                        }
                        else
                        {
                            limitamount = objMySqlDataReader["paid_amount"].ToString();
                        }

                        objMySqlDataReader.Close();

                        lslimit_amount1 = double.Parse(lslimit_amount) - double.Parse(limitamount);

                        msSQL = "select * from pay_trn_tpaymentlimit where employee_gid='" + lsemployee_gid + "' and month='" + month + "'" +
                                " and year='" + year + "' ";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            msSQL = " update pay_trn_tpaymentlimit set limit_amount='" + lslimit_amount + "',paid_amount='" + lslimit_amount1 + "'," +
                           " payable_amount='" + lspayable_amount + "',no_of_workeddays='" + lsactual_month_workingdays + "'," +
                           " outstanding_amount='" + lslimit_amount1 + "'  where employee_gid='" + lsemployee_gid + "' and" +
                           " month='" + month + "'and year='" + year + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }

                        objMySqlDataReader.Close();

                        msSQL = "select * from pay_trn_tpaymentlimit where employee_gid='" + lsemployee_gid +
                            "' and month='" + month + "'" +
                            " and year='" + year + "' ";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsoutstanding_amount = objMySqlDataReader["outstanding_amount"].ToString();
                            lslimit_flag = objMySqlDataReader["limit_flag"].ToString();
                            lsoutstanding_amount1 = double.Parse(lsoutstanding_amount);
                        }


                        else
                        {
                            var msgetpamentlimit_gid = objcmnfunctions.GetMasterGID("PAYL");
                            msSQL = " insert into pay_trn_tpaymentlimit ( " +
                                  " paymentlimit_gid, " +
                                  " salary_gid, " +
                                  " employee_gid, " +
                                  " month, " +
                                  " year, " +
                                  " no_of_workeddays, " +
                                  " empbranch_gid, " +
                                  " limit_amount, " +
                                  " paid_amount, " +
                                  " created_by, " +
                                  " created_date, " +
                                  " payable_amount " +
                                  " ) values ( " +
                                 "'" + msgetpamentlimit_gid + "', " +
                                 "'" + dt["salary_gid"].ToString() + "', " +
                                 "'" + lsemployee_gid + "', " +
                                 "'" + month + "', " +
                                 "'" + year + "', " +
                                 "'" + lsactual_month_workingdays + "', " +
                                 "'" + dt["branch_gid"].ToString() + "', " +
                                 "'" + lslimit_amount + "', " +
                                 "'" + lslimit_amount1 + "', " +
                                 "'" + user_gid + "', " +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                 "'" + lspayable_amount + "' ) ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        objMySqlDataReader.Close();

                        //if (lsoutstanding_amount1 != 0++ lslimit_flag != "Y")
                        //{
                        //    //lsoutstanding_amount1 = lsoutstanding_amount1;
                        //}
                        //else
                        //{
                        //    lsoutstanding_amount1 = lslimit_amount1;
                        //}
                        getModuleList.Add(new GetMakePaymentlist
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            earned_net_salary = dt["earned_net_salary"].ToString(),
                            outstanding_amount = double.Parse((lsoutstanding_amount1).ToString("N")),
                        });
                        values.makepaymentlist = getModuleList;
                    }


                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Make Payment!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void Dasalarypaymentupdate(string user_gid, salaryedit_listdtl values)
        {
            try
            {

                foreach (var dt in values.salaryedit_list)
                {
                    msSQL = "select employee_gid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                    string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                    string lsnet_salary = "";
                    string lssalary_gid = "";
                    string lspaid_amount = "";
                    string lslimit_amount = "";
                    double lsoutstandingamount;

                    msSQL = "select net_salary,salary_gid from pay_trn_tpayment where payment_gid='" + dt.payment_gid + "'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        lsnet_salary = objMySqlDataReader["net_salary"].ToString();
                        lssalary_gid = objMySqlDataReader["salary_gid"].ToString();

                    }
                    objMySqlDataReader.Close();
                    msSQL = " select cast(ifnull(sum(net_salary),'0.0') as decimal) as paid_amount from pay_trn_tpayment where payment_month='" + dt.payment_month + "' and payment_year='" + dt.payment_year + "' and employee_gid='" + dt.employee_gid + "' ";
                    lspaid_amount = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select cast(ifnull(limit_amount,'0.0') as decimal) as limit_amount from pay_trn_tpaymentlimit where month='" + dt.payment_month + "' and year='" + dt.payment_year + "' and employee_gid='" + dt.employee_gid + "' ";
                    lslimit_amount = objdbconn.GetExecuteScalar(msSQL);

                    double lslimitamount = double.Parse(lslimit_amount);
                    double lspaidamount = double.Parse(lspaid_amount);


                    lspaidamount1 = lspaidamount - double.Parse(lsnet_salary);

                    lsoutstandingamount = lslimitamount - lspaidamount1 - double.Parse(values.net_salary);

                    if (lspaidamount > lslimitamount)
                    {
                        lsemployee_gid = "'" + dt.employee_gid + "'," + lsemployee_gid;
                        continue;
                    }

                    string uiDateStr2 = dt.payment_date;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    formattedpayment_date = uiDate2.ToString("yyyy-MM-dd");

                    string netSalary = dt.net_salary;
                    string formatedNetSalary;

                    if (netSalary.Contains(","))
                    {
                        formatedNetSalary = netSalary.Replace(",", "");
                    }
                    else
                    {
                        formatedNetSalary = netSalary;
                    }


                    msSQL = " update pay_trn_tpayment set net_salary='" + formatedNetSalary + "', " +
                            " payment_method='Partial', " +
                            " payment_date='" + formattedpayment_date + "', " +
                            " updated_by='" + user_gid + "', " +
                            " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                            " where  payment_gid='" + dt.payment_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update pay_trn_tpaymentlimit set outstanding_amount='" + lsoutstandingamount + "', paid_amount='" + lspaidamount + "',  " +
                           " updated_by='" + user_gid + "', " +
                            " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "',  " +
                            " limit_flag='N' " +
                            " where month='" + dt.payment_month + "' and year='" + dt.payment_year + "' and employee_gid='" + dt.employee_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update pay_trn_tsalary set limit_flag='N' where salary_gid='" + lssalary_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (lsoutstandingamount == 0.0)
                    {

                        msSQL = " update pay_trn_tpaymentlimit set  limit_flag='Y',payment_method='Full', " +
                                " updated_by='" + user_gid + "', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "',  " +
                                " where month='" + dt.payment_month + "' and year='" + dt.payment_year + "' and employee_gid='" + dt.employee_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update pay_trn_tsalary set limit_flag='Y' where salary_gid='" + lssalary_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else { }
                }
                values.status = true;

            }
           
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Make Payment!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaPostMakePayment(string user_gid, string branch_gid, payment_listdtl values)
        {
            try
            {
                string uiDateStr2 = values.payment_date;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                formattedpayment_date = uiDate2.ToString("yyyy-MM-dd");

                msSQL = "select modeofpayment_gid from pay_mst_tmodeofpayment where payment_type='"+values.payment_type+"'";
                string lspayment_type=objdbconn.GetExecuteScalar(msSQL);
                
                double lscount = 0;
                string lsaccount_no = null;
                string lspayment_gid = null;
                string lsemployee_gid = null;
                double lsupdated_limitamount;
                string lsnet_salary = "";
                string lsemployeraccount_gid = null;
                string lsuser_gid = null;

                string lsuser_code = null;
                string lsemployee_accountgid = null;
                string LSTransactionType = null;
                string LSReferenceGID = null;
                string lsemployerbank_code = null;
                string lsbank_name = null;
                string lslimit_amount = "";
                string lspaid_amount = "";


                msSQL = " select payment_gid from pay_trn_tpayment a " +
                        " left join pay_mst_tmodeofpayment b on b.modeofpayment_gid=a.payment_type " +
                        " where payment_month='" + values.month + "' and payment_year='" + values.year + "' group by a.payment_date,a.payment_type ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                double refno = objMySqlDataReader.RecordsAffected;

                objMySqlDataReader.Close();

                string journal_refno = "00" + refno + 1;

                foreach (var dt in values.payment_list)
                {


                    msSQL = " select account_gid,bank_code from acc_mst_tbank where bank_gid='" + values.bank_name + "'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                    if (objMySqlDataReader.HasRows == true)
                    {
                        lsemployeraccount_gid = objMySqlDataReader["account_gid"].ToString();
                        lsemployerbank_code = objMySqlDataReader["bank_code"].ToString();

                    }

                    objMySqlDataReader.Close();

                    msSQL = "select ifnull(sum(net_salary),0.00) as net_salary from pay_trn_tsalary where employee_gid='" + values.employee_gid + "' and month='" + values.month + "' " +
                            " and year='" + values.year + "'";
                    lsnet_salary = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select ac_no,user_gid,bank from hrm_mst_temployee where employee_gid='" + dt.employee_gid + "'";
                    objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                    if (objMySqlDataReader2.HasRows == true)
                    {
                        lsaccount_no = objMySqlDataReader2["ac_no"].ToString();
                        lsuser_gid = objMySqlDataReader2["user_gid"].ToString();
                        lsbank_name = objMySqlDataReader2["bank"].ToString();

                    }

                    objMySqlDataReader2.Close();

                    msSQL = " select paymentlimit_gid,salary_gid,employee_gid,month,year,limit_amount,empbranch_gid,paid_amount,payable_amount,no_of_workeddays,outstanding_amount from pay_trn_tpaymentlimit where employee_gid='" + dt.employee_gid + "' and month='" + values.month + "' " +
                            " and year='" + values.year + "' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        objMySqlDataReader.Read();
                        lslimit_amount = objMySqlDataReader["limit_amount"].ToString();
                        lspaid_amount = objMySqlDataReader["paid_amount"].ToString();
                        string lspaymentlimit_gid = objMySqlDataReader["paymentlimit_gid"].ToString();
                        string lssalary_gid = objMySqlDataReader["salary_gid"].ToString();
                        string lsempbranch_gid = objMySqlDataReader["empbranch_gid"].ToString();
                        string lsno_of_workeddays = objMySqlDataReader["no_of_workeddays"].ToString();
                        lspayable_amount = double.Parse(objMySqlDataReader["payable_amount"].ToString());




                        double lslimitamount = double.Parse(lslimit_amount);
                        double lspaidamount = double.Parse(lspaid_amount);


                        msSQL = "select ifnull(sum(earned_net_salary),0.00) as net_salary from pay_trn_tsalary " +
                            " where employee_gid='" + dt.employee_gid +
                            "' and month='" + values.month + "' " +
                            " and year='" + values.year + "'";
                        lsnet_salary = objdbconn.GetExecuteScalar(msSQL);

                        lsupdated_limitamount = lslimitamount - double.Parse(lsnet_salary) - double.Parse(dt.earned_net_salary);

                        if (lslimitamount < lspaidamount)
                        {
                            lsemployee_gid = "'" + dt.employee_gid + "'," + lsemployee_gid;

                            continue;
                        }
                        string msgetpayment_gid = objcmnfunctions.GetMasterGID("SLPY");
                        string msgetpaycount = objcmnfunctions.GetMasterGID("PYPC");

                        msSQL = " insert into pay_trn_tpayment ( " +
                                                      " payment_gid, " +
                                                      " salary_gid, " +
                                                      " paymentlimit_gid, " +
                                                      " employee_gid, " +
                                                      " payment_month, " +
                                                      " payment_year, " +
                                                      " no_of_workingdays, " +
                                                      " empbranch_gid, " +
                                                      " payable_amount, " +
                                                      " net_salary, " +
                                                      " payment_date, " +
                                                      " payment_type, " +
                                                      " cheque_bank, " +
                                                      " bank_branch, " +
                                                      " paymentcount_gid, " +
                                                      " payment_flag, " +
                                                      " account_no, " +
                                                      " employee_bankname, " +
                                                      " cheque_number, " +
                                                      " company_check_number, " +
                                                      " payment_method, " +
                                                      " issued_by, " +
                                                      " issued_date " +
                                                      " ) Values ( " +
                                                      " '" + msgetpayment_gid + "', " +
                                                      " '" + lssalary_gid + "', " +
                                                      " '" + lspaymentlimit_gid + "', " +
                                                      " '" + dt.employee_gid + "', " +
                                                      " '" + values.month + "', " +
                                                      " '" + values.year + "', " +
                                                      " '" + lsno_of_workeddays + "', " +
                                                      " '" + lsempbranch_gid + "', " +
                                                      " '" + lspayable_amount + "', " +
                                                      " '" + lsnet_salary + "', " +
                                                      "'" + formattedpayment_date + "', " +
                                                      "'" + lspayment_type + "', " +
                                                      " '" + values.bankname_pay + "', " +
                                                      " '" + values.bank_branch + "', " +
                                                      " '" + msgetpaycount + "', " +
                                                      " 'Y', " +
                                                      " '" + values.account_no + "', " +
                                                      " '" + values.bank_name + "', " +
                                                      " '" + values.cheque_number + "', " +
                                                      " '" + dt.company_check_number + "', " +
                                                      " 'Partial', " +
                                                       "'" + user_gid + "', " +
                                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (lsupdated_limitamount == 0.0)
                        {
                            msSQL = " update pay_trn_tpayment set " +
                                        " payment_method='Full' " +
                                        " where payment_gid='" + msgetpayment_gid + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " update pay_trn_tpaymentlimit set limit_flag='Y' " +
                                    " where  month='" + values.month + "' " +
                                    " and year='" + values.year + "' and employee_gid='" + dt.employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " update pay_trn_tsalary a " +
                                    " set a.limit_flag='Y'" +
                                    " where a.month='" + values.month + "' " +
                                    " and a.year='" + values.year + "' and a.employee_gid='" + dt.employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }


                        if (mnResult == 1)
                        {
                            lscount = lscount + 1;
                            msSQL = " update pay_trn_tsalary a " +
                                    " set a.payment_flag='Y'" +
                                    " where a.month='" + values.month + "' " +
                                    " and a.year='" + values.year + "' and a.employee_gid='" + dt.employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " update pay_trn_tpaymentlimit set outstanding_amount='" + lsupdated_limitamount + "', " +
                                    " paid_amount='" + lspaidamount + "' " +
                                    " where  month='" + values.month + "' " +
                                    " and year='" + values.year + "' and employee_gid='" + dt.employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            

                            msSQL = " insert into pay_trn_tpaymentcount( " +
                                    " paymentcount_gid, "+
                                    " employee_count, " +
                                    " payment_month, " +
                                    " payment_year, " +
                                    " created_by, " +
                                   " created_date ) " +
                                   " values(  " +
                                   " '" + msgetpaycount +"'," +
                                    " '" + lscount + "'," +
                                   " '" + values.month + "'," +
                                  " '" + values.year + "', " +
                                  " '" + user_gid + "'," +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                    }
                    objMySqlDataReader.Close();

                    msSQL = "select account_gid from acc_mst_tchartofaccount where account_name = 'Salary A/C'";
                    account_gid = objdbconn.GetExecuteScalar(msSQL);

                    objfinance_cmnfunction.finance_payment("Payroll",
                                        values.payment_type,
                                        values.bank_name,
                                        DateTime.Now.ToString("yyyy-MM-dd"),
                                        lspayable_amount,
                                        branch_gid,
                                        "Salary Payment",
                                        "PAY",
                                        dt.employee_gid,
                                        "Salary Payment for the employee " + dt.employee_gid + " on this " + values.month + "-" + values.year,
                                         values.month + values.year + "-" + journal_refno,
                                        0.0,
                                        0.0,
                                        lspayable_amount,
                                        msgetpayment_gid);


                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Payment Done Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Payment";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Payment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          


        }


        //public void finance_payment(string payment_type, string bank_branch, string branch_gid, string payment_date, string payment_amount, string screenname, string module_name,
        //                           string account_gid, string remarks, string refno, double tds_amount, double adjustment_amount, string paid_amount, string transaction_gid)
        //{
        //    string SEjournal_type;
        //    int journal_year, journal_month, journal_day;

        //    DateTime paymentdate = Convert.ToDateTime(payment_date);
        //    DateTime date = DateTime.Now; // Example date
        //    journal_month = date.Month;
        //    journal_day = date.Day;
        //    journal_year = date.Year;

        //    if (payment_type == "Cash")
        //    {
        //        LSTransactionCode = "CC001";
        //        LSTransactionType = "Cash Book";
        //        LSReferenceGID = branch_gid;
        //    }
        //    else if (payment_type == "Cheque")
        //    {
        //        LSTransactionType = "Credit Card Book";
        //        LSReferenceGID = transaction_gid;
        //    }
      
        //    else
        //    {
        //        LSTransactionType = "NEFT";
        //        LSReferenceGID = transaction_gid;
        //    }
        //    string msGetGID = objcmnfunctions.GetMasterGID("FPCC");

        //    string msGetSEDlGID = objcmnfunctions.GetMasterGID("FPCD");

        //    msSQL = " Insert into acc_trn_journalentrydtl " +
        //            " (journaldtl_gid, " +
        //            " journal_gid, " +
        //            " account_gid," +
        //            " transaction_gid," +
        //            " journal_type," +
        //            " remarks, " +
        //            " transaction_amount) " +
        //            " values (" +
        //            "'" + msGetSEDlGID + "', " +
        //            "'" + msGetGID + "'," +
        //            "'" + account_gid + "'," +
        //            "'" + account_gid + "'," +
        //            "'dr'," +
        //            "'" + remarks.Trim() + "', " +
        //            "'" + payment_amount + "')";
        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //    string msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");

        //    msSQL = " Insert into acc_trn_journalentrydtl " +
        //            " (journaldtl_gid, " +
        //            " journal_gid, " +
        //            " account_gid," +
        //            " transaction_gid," +
        //            " journal_type," +
        //            " remarks, " +
        //            " transaction_amount) " +
        //            " values (" +
        //            "'" + msGetDlGID + "', " +
        //            "'" + msGetGID + "'," +
        //            "'" + account_gid + "'," +
        //            "'" + account_gid + "'," +
        //            "'cr'," +
        //            "'" + remarks.Trim() + "', " +
        //            "'" + paid_amount + "')";
        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //    msSQL = " Insert into acc_trn_journalentry " +
        //      " (journal_gid, " +
        //      " transaction_date, " +
        //      " remarks, " +
        //      " transaction_type," +
        //      " branch_gid," +
        //      " reference_type, " +
        //      " reference_gid, " +
        //      " journal_from," +
        //      " transaction_code, " +
        //      " transaction_gid, " +
        //      " journal_year, " +
        //      " journal_month, " +
        //      " journal_day, " +
        //      " journal_refno)" +
        //      " values (" +
        //      "'" + msGetGID + "', " +
        //      "'" + payment_date + "', " +
        //      "'" + remarks.Trim() + "'," +
        //      "'" + LSTransactionType + "'," +
        //      "'" + branch_gid + "', " +
        //      "'Salary A/C', " +
        //      "'" + LSReferenceGID + "', " +
        //      "'" + payment_type + "'," +
        //      "'" + LSTransactionCode + "'," +
        //      "'" + transaction_gid + "', " +
        //      "'" + journal_year + "', " +
        //      "'" + journal_month + "', " +
        //      "'" + journal_day + "', " +
        //      "'" + refno + "')";

        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



        //}

    }
}





  
