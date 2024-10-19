using ems.payroll.Models;

using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using ems.payroll.Models;

using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ems.payroll.DataAccess
{
    public class DaRptPayrunSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1;
     
        DataTable dt1 = new DataTable();

        DataTable DataTable2 = new DataTable();

        string company_logo_path;

        Image company_logo;

        string msGetloangid,lsotherscomponenttype;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, mail_datatable;
        int mnResult,monthnum;
        string lscompany_code,file_name="",lspath1,lspath2, mail_path, msGet_mailattachement_Gid, msenquiryloggid;
        List<addsummary> getModuleList;
        List<dedsummary> getModuleList1;
        List<othersummary> getModuleList2;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();


        public void DaGetpayruninitialsummary(string month,string year, MdlRptPayrunSummary values)
          {
            try
            {
                msSQL = "select a.salary_gid,a.month,a.year, a.employee_gid,concat_ws(' ', c.user_firstname, user_lastname) as employee_name,b.branch_gid,c.user_code," +
                    " d.branch_name,e.department_name as department,a.leave_taken,a.lop_days as lop,b.employee_emailid, " +
                     " format(a.basic_salary, 2) as basic_salary , format(a.earned_basic_salary, 2) as earned_basic_salary ,b.user_gid, " +
                     " format(a.gross_salary, 2) as gross_salary , format(a.earned_gross_salary, 2) as earned_gross_salary ,a.public_holidays, " +
                     " format(a.net_salary, 2)As net_salary, format(a.earned_net_salary, 2)As earned_net_salary, a.actual_month_workingdays,a.month_workingdays " +
                     " from pay_trn_tsalary a " +
                     " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                     " left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                     " left join hrm_mst_tbranch d on b.branch_gid = d.branch_gid " +
                     " left join hrm_mst_tdepartment e on b.department_gid = e.department_gid " +
                     "where a.month='"+ month +"' and a.year='"+ year +"' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPayrunlist>();
                if(dt_datatable.Rows.Count > 0)
                {
                    foreach(DataRow dt in dt_datatable.Rows)
                    {
                        string salary_gid = dt["salary_gid"].ToString();

                        getModuleList.Add(new GetPayrunlist()
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            month = dt["month"].ToString(),
                            year = dt["year"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department = dt["department"].ToString(),
                            leave_taken = dt["leave_taken"].ToString(),
                            lop = dt["lop"].ToString(),
                            to_emailid1 = dt["employee_emailid"].ToString(),
                            basic_salary = dt["basic_salary"].ToString(),
                            earned_basic_salary = dt["earned_basic_salary"].ToString(),
                            public_holidays = dt["public_holidays"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                            earned_gross_salary = dt["earned_gross_salary"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            earned_net_salary = dt["earned_net_salary"].ToString(),
                            actual_month_workingdays = dt["actual_month_workingdays"].ToString(),
                            month_workingdays = dt["month_workingdays"].ToString(),
                        });
                        values.payrunlist = getModuleList;
                        Daadditionalsummary( salary_gid,values);
                        Dadeductsummary(salary_gid, values);
                        Daothersummary(salary_gid, values);
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payrun Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            } 
        }

        public void DaGetPayrunSummary(string branch_gid, string department_gid, string month, string year, MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = "select a.salary_gid,a.month,a.year, a.employee_gid,concat_ws(' ', c.user_firstname, user_lastname) as employee_name,b.branch_gid,c.user_code," +
                    " d.branch_name,e.department_name as department,a.leave_taken,a.lop_days as lop,b.employee_emailid, " +
                     " format(a.basic_salary, 2) as basic_salary , format(a.earned_basic_salary, 2) as earned_basic_salary ,b.user_gid, " +
                     " format(a.gross_salary, 2) as gross_salary , format(a.earned_gross_salary, 2) as earned_gross_salary ,a.public_holidays, " +
                     " format(a.net_salary, 2)As net_salary, format(a.earned_net_salary, 2)As earned_net_salary, a.actual_month_workingdays,a.month_workingdays " +
                     " from pay_trn_tsalary a " +
                     " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                     " left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                     " left join hrm_mst_tbranch d on b.branch_gid = d.branch_gid " +
                     " left join hrm_mst_tdepartment e on b.department_gid = e.department_gid ";
                if (year != "null" || month != "null" || branch_gid != "null" || department_gid != "null")
                {
                    msSQL += " where ";
                    if (year != null)
                    {
                        msSQL += " a.year = '" + year + "' ";
                        addfunyear();
                    }
                    if (month != "null")
                    {
                        msSQL += " a.month = '" + month + "' ";
                        addfunmonth();
                    }
                    if (branch_gid != "null")
                    {
                        msSQL += " d.branch_gid = '" + branch_gid + "' ";
                        addfunbranch();
                    }
                    if (department_gid != "null")
                    {
                        msSQL += " e.department_gid = '" + department_gid + "' ";
                    }
                    void addfunyear()
                    {
                        if (month != "null" || branch_gid != "null" || department_gid != "null") { msSQL += " and "; }
                    }
                    void addfunmonth()
                    {
                        if (branch_gid != "null" || department_gid != "null") { msSQL += " and "; }
                    }
                    void addfunbranch()
                    {
                        if (department_gid != "null") { msSQL += " and "; }
                    }

                }


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPayrunlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string salary_gid = dt["salary_gid"].ToString();

                        getModuleList.Add(new GetPayrunlist
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            month = dt["month"].ToString(),
                            year = dt["year"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department = dt["department"].ToString(),
                            leave_taken = dt["leave_taken"].ToString(),
                            to_emailid1 = dt["employee_emailid"].ToString(),
                            lop = dt["lop"].ToString(),
                            basic_salary = dt["basic_salary"].ToString(),
                            earned_basic_salary = dt["earned_basic_salary"].ToString(),
                            public_holidays = dt["public_holidays"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                            earned_gross_salary = dt["earned_gross_salary"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            earned_net_salary = dt["earned_net_salary"].ToString(),
                            actual_month_workingdays = dt["actual_month_workingdays"].ToString(),
                            month_workingdays = dt["month_workingdays"].ToString(),

                        });
                        values.payrunlist = getModuleList;
                        Daadditionalsummary(salary_gid, values);
                        Dadeductsummary(salary_gid, values);
                        Daothersummary(salary_gid, values);
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payrun Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetBranchDtl(MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = " Select branch_gid,branch_name  " +
                    " from  hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchdropdown
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.GetBranchDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Branch detail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetDepartmentDtl(MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = " select department_gid,department_name" +
                    " from hrm_mst_tdepartment ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDepartmentdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDepartmentdropdown
                        {
                            department_gid = dt["department_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                        });
                        values.GetDepartmentDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Department!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void Daadditionalsummary(string salary_gid, MdlRptPayrunSummary values)
        {
            
            try
            {

                msSQL = " select a.salary_gid,(c.componentgroup_name) as salarycomponent_name,b.salarycomponent_percentage," +
                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                            "  from pay_trn_tsalary a" +
                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                            " where a.salary_gid ='" + salary_gid + "' and b.salarygradetype='Addition' and c.primecomponent_flag='N'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (getModuleList == null)
                {
                    getModuleList = new List<addsummary>();

                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new addsummary
                            {
                                salary_gid = dt["salary_gid"].ToString(),
                                salarycomponent_name = dt["salarycomponent_name"].ToString(),
                                earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                            });
                            values.addsummary = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {

                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new addsummary
                            {
                                salary_gid = dt["salary_gid"].ToString(),
                                salarycomponent_name = dt["salarycomponent_name"].ToString(),
                                earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                            });
                            values.addsummary = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void Dadeductsummary(string salary_gid, MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = " select a.salary_gid,(c.componentgroup_name) as salarycomponent_name,b.salarycomponent_percentage," +
                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                            "  from pay_trn_tsalary a" +
                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                            " where a.salary_gid ='" + salary_gid + "' and b.salarygradetype='Deduction' and c.primecomponent_flag='N'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (getModuleList1 == null)
                {
                    getModuleList1 = new List<dedsummary>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList1.Add(new dedsummary
                            {
                                salary_gid = dt["salary_gid"].ToString(),
                                salarycomponent_name = dt["salarycomponent_name"].ToString(),
                                earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                            });
                            values.dedsummary = getModuleList1;
                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList1.Add(new dedsummary
                            {
                                salary_gid = dt["salary_gid"].ToString(),
                                salarycomponent_name = dt["salarycomponent_name"].ToString(),
                                earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                            });
                            values.dedsummary = getModuleList1;
                        }
                    }
                    dt_datatable.Dispose();
                }
               
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Deduction Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void Daothersummary(string salary_gid, MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = " select a.salary_gid,(c.componentgroup_name) as salarycomponent_name,b.salarycomponent_percentage," +
                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                            "  from pay_trn_tsalary a" +
                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                            " where a.salary_gid ='" + salary_gid + "' and b.salarygradetype='Others'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (getModuleList2 == null)
                {
                    getModuleList2 = new List<othersummary>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList2.Add(new othersummary
                            {
                                salary_gid = dt["salary_gid"].ToString(),
                                salarycomponent_name = dt["salarycomponent_name"].ToString(),
                                earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                            });
                            values.othersummary = getModuleList2;
                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList2.Add(new othersummary
                            {
                                salary_gid = dt["salary_gid"].ToString(),
                                salarycomponent_name = dt["salarycomponent_name"].ToString(),
                                earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                            });
                            values.othersummary = getModuleList2;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Other Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
       }

        public Dictionary<string, object> DaGetPayslipRpt(string month, string year, string salary_gid, MdlRptPayrunSummary values)
        {

            var response = new Dictionary<string, object>();
          string   full_path = null;
           string lscompanycode, report_path, base_pathOF_currentFILE;

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();
            msSQL1 = "SELECT b.othercomponent_type FROM pay_trn_tsalarydtl a  " +
                    "INNER JOIN pay_mst_tsalarycomponent b ON a.salarycomponent_gid = b.salarycomponent_gid " +
                    " WHERe a.salarygradetype IN ('Others') AND a.salary_gid = '" + salary_gid + "'" +
                     "and b.othercomponent_type='Addition'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL1);
            if (objOdbcDataReader.HasRows ==true)
            {
                msSQL = " select (b.display_name) as salarycomponent_name,a.salarycomponent_amount,a.earned_salarycomponent_amount from pay_trn_tsalarydtl a  " +
                  " inner join pay_mst_tsalarycomponent b on a.salarycomponent_gid=b.salarycomponent_gid where " +
                  " a.salarygradetype in ('Addition','Others') and a.salary_gid='" + salary_gid + "' and b.othercomponent_type<>'Deduction'";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable1");

            }
            else
            {

            msSQL = " select (b.display_name) as salarycomponent_name,a.salarycomponent_amount,a.earned_salarycomponent_amount from pay_trn_tsalarydtl a  " +
                    " inner join pay_mst_tsalarycomponent b on a.salarycomponent_gid=b.salarycomponent_gid where " +
                    " a.salarygradetype='Addition' and a.salary_gid='" + salary_gid + "'";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");
            }


        msSQL = "SELECT a.company_name, (a.company_address) as company_website,a.company_logo_path as company_logo, a.company_phone, a.company_mail, a.contact_person " +

         "FROM adm_mst_tcompany a ";

            dt1 = objdbconn.GetDataTable(msSQL);

            DataTable2.Columns.Add("company_logo", typeof(byte[]));

            if (dt1.Rows.Count != 0)

            {

                foreach (DataRow dr_datarow in dt1.Rows)

                {

                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo"].ToString().Replace("../../", ""));

                    if (System.IO.File.Exists(company_logo_path) == true)

                    {

                        //Convert  Image Path to Byte

                        company_logo = System.Drawing.Image.FromFile(company_logo_path);

                        byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));

                        DataTable2.Rows.Add(bytes);

                    }

                }

            }

            dt1.Dispose();
            DataTable2.TableName = "DataTable2";
            myDS.Tables.Add(DataTable2);


            msSQL1 = "SELECT b.othercomponent_type FROM pay_trn_tsalarydtl a  " +
               "INNER JOIN pay_mst_tsalarycomponent b ON a.salarycomponent_gid = b.salarycomponent_gid " +
               " WHERe a.salarygradetype IN ('Others') AND a.salary_gid = '" + salary_gid + "'" +
                "and b.othercomponent_type='Deduction'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL1);
            if(objOdbcDataReader.HasRows==false) { 

            msSQL = " select (b.display_name) as salarycomponent_name,a.salarycomponent_amount,a.earned_salarycomponent_amount from pay_trn_tsalarydtl a  " +
                    " inner join pay_mst_tsalarycomponent b on a.salarycomponent_gid=b.salarycomponent_gid where " +
                    " a.salarygradetype='Deduction'  and a.salary_gid='" + salary_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable3");
            }
            else
            {
                msSQL = " select (b.display_name) as salarycomponent_name,a.salarycomponent_amount,a.earned_salarycomponent_amount from pay_trn_tsalarydtl a  " +
                  " inner join pay_mst_tsalarycomponent b on a.salarycomponent_gid=b.salarycomponent_gid where " +
                  " a.salarygradetype in('Deduction','Others')  and a.salary_gid='" + salary_gid + "'and b.othercomponent_type<>'Addition'";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");

            }
            msSQL = " select  a.salary_gid, a.month, a.year,a.ctc as passport_no,a.employee_gid,d.user_code as employee_code, concat(d.user_firstname,' ',d.user_lastname)as employee_name, " +
                    " a.basic_salary, a.earned_basic_salary, a.gross_salary, a.earned_gross_salary, a.net_salary,b.uan_no,b.pan_no,concat(f.department_name,'-',e.branch_code) as department_withbranch, " +
                    " a.earned_net_salary, a.month_workingdays, a.actual_month_workingdays,b.pf_no,b.esi_no,a.department_name as department,a.designation_name,a.branch_name,a.designation_gid,a.department_gid,a.branch_gid," +
                    " b.bank as bank,b.ac_no as ac_no, " +
                    " a.leave_taken, a.lop_days as lop,a.month_workingdays, a.public_holidays,date_format(b.employee_joiningdate,'%d-%m-%Y') as employee_joiningdate, " +
                    " (select sum(earned_salarycomponent_amount) from pay_trn_tsalarydtl x " +
                    " left join pay_trn_tsalary y on y.salary_gid=x.salary_gid where salarygradetype in('Deduction','Others')   and y.month='" + month + "'" +
                    " and y.year='" + year + "' and " + " y.salary_gid='" + salary_gid + "') as totaldeduction, " +
                    " (select sum(earned_salarycomponent_amount) from pay_trn_tsalarydtl x1 " + " left join pay_trn_tsalary y1 on y1.salary_gid=x1.salary_gid where salarygradetype='Addition'  and y1.month='" + month + "'" +
                    " and y1.year='" + year + "' and " + " y1.salary_gid='" + salary_gid + "') as totaladdition, " +
                    " cast(ifnull((select  format(y.repayment_amount,2) as advance_amount from pay_trn_tloanrepayment y  inner join  pay_trn_tloan z on y.loan_gid=z.loan_gid " +
                    " where z.employee_gid=a.employee_gid and date_format(y.repayment_duration,'%M')='" + month + "' and  date_format(y.repayment_duration,'%Y')='" + year + "'  " +
                    " and y.type='advance' group by a.employee_gid),'0.00')as char) as advance, " +
                    " cast(ifnull((select  format(y.repayment_amount,2) as loan_amount from pay_trn_tloanrepayment y  inner join  pay_trn_tloan z on y.loan_gid=z.loan_gid " +
                    " where z.employee_gid=a.employee_gid and date_format(y.repayment_duration,'%M')='" + month +
                    "' and  date_format(y.repayment_duration,'%Y')='" + year + "' " +
                    " and y.type='loan' group by a.employee_gid),'0.00')as char) as loan " +
                    " from  pay_trn_tsalary a " +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " left join adm_mst_tuser d on b.user_gid=d.user_gid " +
                    " left join hrm_mst_tbranch e on b.branch_gid=e.branch_gid " +
                    " left join hrm_mst_tdepartment f on b.department_gid=f.department_gid  " +
                    " left join pay_trn_tpayment  h on a.salary_gid=h.salary_gid " +
                    " where a.month='" + month + "' " +
                    " and a.year='" + year + "' and a.salary_gid='" + salary_gid + "' " +
                    " group by salary_gid order by e.branch_name,d.user_code,a.basic_salary asc ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable4");
            msSQL = "SELECT a.company_name, a.company_address as company_website,a.company_logo_path as company_logo, a.company_phone, a.company_mail, a.contact_person " +

             "FROM adm_mst_tcompany a ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable5");


            ReportDocument oRpt = new ReportDocument();

            msSQL = "select payslip_format from pay_mst_tpayrollconfig";

            string lspdf = objdbconn.GetExecuteScalar(msSQL);

            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_payroll"].ToString(), lspdf));


            oRpt.SetDataSource(myDS);

            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "PaySlip_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");

            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);

            myConnection.Close();

            var ls_response = objFnazurestorage.reportStreamDownload(path);

            File.Delete(path);

            return ls_response;

            //var ls_response = objFnazurestorage.reportStreamDownload(path);
            //File.Delete(path);
            //return ls_response;

            //try
            //{

            //    ReportDocument oRpt = new ReportDocument();

            //    msSQL = "select company_code from adm_mst_tcompany";
            //    lscompanycode = objdbconn.GetExecuteScalar(msSQL);
            //    if (lscompanycode == "NOQU")
            //    {
            //         report_path = Path.Combine(base_pathOF_currentFILE, "ems.payroll", "pay_rpt_noqupayslip.rpt");
            //       // oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_payroll"].ToString(), "pay_rpt_noqupayslip.rpt"));

            //    }
            //    else
            //    {
            //        report_path = Path.Combine(base_pathOF_currentFILE, "ems.payroll", "pay_rpt_payaauraaslip.rpt");
            //        //oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_payroll"].ToString(), "pay_rpt_payaauraaslip.rpt"));

            //    }
            //   // string report_path = Path.Combine(base_pathOF_currentFILE, "ems.einvoice", "Reports", "rbl_crp_paymentreceipt.rpt");

            //    if (!File.Exists(report_path))
            //    {
            //        values.status = false;
            //        values.message = "Your Rpt path not found !!";
            //        response = new Dictionary<string, object>
            //        {
            //            {"status",false },
            //            {"message",values.message}
            //        };

            //    }
            //    oRpt.Load(report_path);
            //    oRpt.SetDataSource(myDS);


            //    path = Path.Combine(ConfigurationManager.AppSettings["report_path"]?.ToString());

            //    if (!Directory.Exists(path))
            //    {
            //        Directory.CreateDirectory(path);
            //    }

            //    string PDFfile_name = "Payslip" + ".pdf";
            //    full_path = Path.Combine(path, PDFfile_name);

            //    oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, full_path);
            //    myConnection.Close();
            //    response = objFnazurestorage.reportStreamDownload(full_path);
            //    values.status = true;

            //}
            //catch (Exception Ex)
            //{
            //    values.status = false;
            //    values.message = Ex.Message;
            //    response = new Dictionary<string, object>
            //    {
            //         { "status", false },
            //         { "message", Ex.Message }
            //    };
            //}
            //finally
            //{
            //    if (full_path != null)
            //    {
            //        try
            //        {
            //            File.Delete(full_path);
            //        }
            //        catch (Exception Ex)
            //        {
            //            values.status = false;
            //            values.message = Ex.Message;
            //            response = new Dictionary<string, object>
            //            {
            //                 { "status", false },
            //                 { "message", Ex.Message }
            //            };
            //        }
            //    }
            //}

            //return response;


        }
        public void Dapayrunmail(HttpRequest httpRequest,string user_gid,result objResult)
        {
            try
            {
                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                MemoryStream ms_stream = new MemoryStream();

                string employee_emailid = httpRequest.Form["employee_emailid"];
                string to_emailid = httpRequest.Form["to_emailid"];
                string subject = httpRequest.Form["subject"];
                string body = httpRequest.Form["body"];
                string bcc = httpRequest.Form["bcc"];
                string cc = httpRequest.Form["cc"];

                HttpPostedFile httpPostedFile;
                string lspath = string.Empty;
                lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Mails/Post/Payroll/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        string file_path = string.Empty;
                        httpFileCollection = httpRequest.Files;
                        for (int i = 0; i < httpFileCollection.Count; i++)
                        {
                            string document_gid = objcmnfunctions.GetMasterGID("UPLF");
                            httpPostedFile = httpFileCollection[i];
                            string fileExtension = httpPostedFile.FileName;
                            file_name = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);
                            string lsfilepath_gid = document_gid;
                            fileExtension = Path.GetExtension(fileExtension).ToLower();
                            string lsfilepaths_gid = lsfilepath_gid + fileExtension;
                            Stream ls_stream;
                            ls_stream = httpPostedFile.InputStream;
                            ls_stream.CopyTo(ms_stream);

                            string return_path, attachement_path;

                            string last_4_digits = file_name + lsfilepath_gid;
                            string get_last_4_digit = objcmnfunctions.ExtractLast4Digits(last_4_digits);

                            lspath1 = "erp_documents" + "/" + lscompany_code + "/" + "Mails/Post/Payroll/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsfilepath_gid + fileExtension;
                            lspath2 = "erp_documents" + "/" + lscompany_code + "/" + "Mails/Post/Payroll/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + file_name + "-" + get_last_4_digit + fileExtension;

                            mail_path = lspath1;
                            attachement_path = lspath2;

                            return_path = objcmnfunctions.uploadFile(lspath + file_name + "-" + get_last_4_digit, fileExtension);
                            ms_stream.Close();
                            msGet_mailattachement_Gid = objcmnfunctions.GetMasterGID("BEAC");
                            msenquiryloggid = objcmnfunctions.GetMasterGID("BELP");
                            msSQL = " insert into acc_trn_temailattachments (" +
                                         " emailattachment_gid, " +
                                         " email_gid, " +
                                         " attachment_systemname, " +
                                         " attachment_path, " +
                                         " inbuild_attachment, " +
                                         " attachment_type " +
                                         " ) values ( " +
                                         "'" + msGet_mailattachement_Gid + "'," +
                                         "'" + msenquiryloggid + "'," +
                                         "'" + file_name + "'," +
                                         "'" + attachement_path + "', " +
                                         "'" + mail_path + "', " +
                                         "'" + fileExtension + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
                catch (Exception ex)
                {
                    objResult.message = "Exception occured while Uploading Mail!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                    "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                }
                msSQL = " select attachment_path as document_path from acc_trn_temailattachments where email_gid='" + msenquiryloggid + "'";
                mail_datatable = objdbconn.GetDataTable(msSQL);
                string result_values = objcmnfunctions.send_mailSMTP(employee_emailid, to_emailid, subject, body, cc, bcc, mail_datatable);
                if (result_values == "Send")
                {
                    objResult.status = true;
                    objResult.message = "Mail Send Successfully !!";
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Mail Not Sent !!";
                }
            }
            catch(Exception ex)
            {
                objResult.message = "Exception occured while Uploading Mail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaDeleteforpayrun(deletepayrunlist values)
        {
            try
            {

                foreach (var data in values.payrunlist)
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

                    OdbcDataReader objOdbcDataReader5;
                    double lsloan_amount = 0;
                    msSQL = " select repayment_gid,repayment_amount from pay_trn_tloan a " +
                           " left join pay_trn_tloanrepayment b on a.loan_gid = b.loan_gid " +
                           " where monthname(repayment_duration)= '" + values.month + "' and " +
                           " year(repayment_duration)= '" + values.year + "' and b.employee_gid = '" + data.employee_gid + "'";
                    objOdbcDataReader5 = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader5.HasRows == true)
                    {
                        msSQL = "update pay_trn_tloanrepayment set " +
                                        " repaid_amount =Null," +
                                        " actual_date =Null," +
                                        " repayment_remarks = Null" +
                                        " where repayment_gid='" + objOdbcDataReader5["repayment_gid"].ToString() + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Payrun Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured While deleted the Payrun";
                    }
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

        public void DaMaillId(string employee_gid, MdlRptPayrunSummary values)
        {
            try
            {

                msSQL = "select employee_emailid from hrm_mst_temployee where employee_gid = '" + employee_gid + "' union select pop_username from adm_mst_tcompany";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMailId_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetMailId_list
                        {
                            employee_emailid = dt["employee_emailid"].ToString(),

                        });
                        values.GetMailId_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Mail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        //public void DaGetPayrunReportForLastSixMonthsSearch(MdlRptPayrunSummary values, string from, string to)
        //{
        //    try
        //    {
        //        if (from == null && to == null)
        //        {
        //            msSQL = " select a.salary_gid,count(a.employee_gid) as employeecount,date_format(payrun_date,'%b-%Y') as payrun_date,substring(date_format(a.payrun_date,'%M'),1,3) as month, " +
        //                    " format(round(sum(a.earned_net_salary),2),2)as amount,count(a.salary_gid)as payruncount ,date_format(payrun_date,'%M/%Y') as month_wise,year(a.payrun_date) as year " +
        //                    " from pay_trn_tsalary a " +
        //                    " where a.payrun_date > date_add(now(), interval-6 month) and a.payrun_date<=date(now()) " +
        //                    "group by date_format(a.payrun_date,'%M') order by a.payrun_date desc  ";
        //        }
        //        else
        //        {
        //            msSQL = " select a.salary_gid,count(a.employee_gid) as employeecount,date_format(payrun_date,'%b-%Y') as payrun_date,substring(date_format(a.payrun_date,'%M'),1,3) as month, " +
        //                    " format(round(sum(a.earned_net_salary),2),2)as amount,count(a.salary_gid)as payruncount ,date_format(payrun_date,'%M/%Y') as month_wise,year(a.payrun_date) as year " +
        //                    " from pay_trn_tsalary a " +
        //                    " where a.payrun_date between '" + from + "' and '" + to + "' " +
        //                    " group by date_format(a.payrun_date,'%M') order by a.payrun_date desc ";

        //        }
        //        dt_datatable = objdbconn.GetDataTable(msSQL);

        //        var GetLastSixMonthsList = new List<GetLastSixMonthsList>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                GetLastSixMonthsList.Add(new GetLastSixMonthsList
        //                {
        //                    payrun_date = (dt["payrun_date"].ToString()),
        //                    salary_gid = (dt["salary_gid"].ToString()),
        //                    payruncount = (dt["payruncount"].ToString()),
        //                    month_wise = (dt["month_wise"].ToString()),
        //                    amount = (dt["amount"].ToString()),
        //                    employeecount = (dt["employeecount"].ToString()),
        //                    month = (dt["month"].ToString()),
        //                    year = (dt["year"].ToString()),
        //                });
        //                values.GetLastSixMonthsList = GetLastSixMonthsList;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while loading Specific Date Data !";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
        //      $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
        //      msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
        //    }


        //}



    }
}
