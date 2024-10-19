using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.payroll.Models;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ems.payroll.DataAccess
{
    public class DaEmployeewisepayment
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty, msGetGID;
        OdbcDataReader objOdbcDataReader;
        string lsemployee_gid;
        DataTable dt_dataTable, objtbl;
        int mnResult;

        public void DaGetbranchdropdownlist(MdlEmployeewisepayment values)
        {
            try
            {

                msSQL = " Select branch_name,branch_gid  " +
                    " from hrm_mst_tbranch ";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getbranchdropdown>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getbranchdropdown
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.Getbranchdropdown = getModuleList;
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

        public void DagetemployeewiseSummary(MdlEmployeewisepayment values)
        {
            try
            {
                msSQL = " select distinct e.employee_gid,u.user_code,concat(u.user_firstname,u.user_lastname) as user_firstname,d.department_name,d.department_gid,b.branch_gid,b.branch_name, " +
                " p.payment_month, p.payment_year,cast(ifnull(p.net_salary,0.0) as decimal) as net_salary,m.payment_type,s.designation_name,s.designation_gid " +
                " from adm_mst_tuser u inner join hrm_mst_temployee e on e.user_gid=u.user_gid " +
                " inner join hrm_mst_tdepartment d on d.department_gid=e.department_gid " +
                " inner join hrm_mst_tbranch b on b.branch_gid=e.branch_gid " +
                " left join pay_trn_tpayment p on p.employee_gid=e.employee_gid " +
                " inner join adm_mst_tdesignation s on s.designation_gid=e.designation_gid " +
                " inner join pay_mst_tmodeofpayment m on m.modeofpayment_gid=p.payment_type where 1=1";


                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Employeewisepaymentlists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Employeewisepaymentlists
                        {
                            Department_Name  = dt["department_name"].ToString(),
                            User_Code = dt["user_code"].ToString(),
                            Employee_Name = dt["user_firstname"].ToString(),
                            Designation_Name = dt["user_firstname"].ToString(),
                            Month = dt["payment_month"].ToString(),
                            Year = dt["payment_year"].ToString(),
                            Payment_Type = dt["payment_type"].ToString(),
                            Total_Salary = dt["net_salary"].ToString(),
                        });
                        values.Employeewisepaymentlists = getModuleList;
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
        public void DagetemployeewiseSummarySearch(string branch_gid, string month, string year, MdlEmployeewisepayment values)
        {
            try
            {

                msSQL = " select distinct e.employee_gid,u.user_code,concat(u.user_firstname,u.user_lastname) as user_firstname,d.department_name,d.department_gid,b.branch_gid,b.branch_name, " +
                " p.payment_month, p.payment_year,cast(ifnull(p.net_salary,0.0) as decimal) as net_salary,m.payment_type,s.designation_name,s.designation_gid " +
                " from adm_mst_tuser u inner join hrm_mst_temployee e on e.user_gid=u.user_gid " +
                " inner join hrm_mst_tdepartment d on d.department_gid=e.department_gid " +
                " inner join hrm_mst_tbranch b on b.branch_gid=e.branch_gid " +
                " left join pay_trn_tpayment p on p.employee_gid=e.employee_gid " +
                " inner join adm_mst_tdesignation s on s.designation_gid=e.designation_gid " +
                " inner join pay_mst_tmodeofpayment m on m.modeofpayment_gid=p.payment_type where 1=1";
                if (year != "null" || month != "null" || branch_gid != "null")
                {
                  
                    if (year != null)
                    {
                        msSQL += "  and p.payment_year= '" + year + "' ";
                        
                    }
                    if (month != "null")
                    {
                        msSQL += " and p.payment_month= '" + month + "' ";
                    }
                    if (branch_gid != "null")
                    {
                        msSQL += "  and b.branch_gid = '" + branch_gid + "' ";
                    }
                   
                }


                    dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Employeewisepaymentlists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Employeewisepaymentlists
                        {
                            Department_Name = dt["department_name"].ToString(),
                            User_Code = dt["user_code"].ToString(),
                            Employee_Name = dt["user_firstname"].ToString(),
                            Designation_Name = dt["user_firstname"].ToString(),
                            Month = dt["payment_month"].ToString(),
                            Year = dt["payment_year"].ToString(),
                            Payment_Type = dt["payment_type"].ToString(),
                            Total_Salary = dt["net_salary"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.Employeewisepaymentlists = getModuleList;
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
    }
}