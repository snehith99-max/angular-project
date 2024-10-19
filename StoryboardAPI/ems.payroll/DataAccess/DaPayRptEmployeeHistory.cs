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
using ems.system.Models;

namespace ems.payroll.DataAccess
{
    public class DaPayRptEmployeeHistory
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1;

        string msGetloangid, lsotherscomponenttype;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, mail_datatable;
        int mnResult, monthnum;
        string lscompany_code, file_name = "", lspath1, lspath2, mail_path, msGet_mailattachement_Gid, msenquiryloggid, lspermanentaddressGID, lswage_gid;
        List<addsummary> getModuleList;
        List<dedsummary> getModuleList1;
        List<othersummary> getModuleList2;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        public void DaGetBranchDetail(MdlPayRptEmployeeHistory values)
        {
            try
            {

                msSQL = " Select branch_gid,branch_name  " +
                    " from  hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchdata>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchdata
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.GetBranchDetail = getModuleList;
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

        public void DaGetDepartmentDetail(string branch_gid, MdlPayRptEmployeeHistory values)
        {
            try
            {
                if (branch_gid != "all")
                {
                    msSQL = "select distinct a.department_gid,a.department_name from hrm_mst_tdepartment a " +
                            "inner join hrm_mst_temployee b on a.department_gid = b.department_gid " +
                            "inner join hrm_mst_tbranch c on b.branch_gid = c.branch_gid " +
                            "where c.branch_gid ='" + branch_gid + "' ";
                }
                else
                {
                    msSQL = "select distinct a.department_gid,a.department_name from hrm_mst_tdepartment a " +
                            "inner join hrm_mst_temployee b on a.department_gid = b.department_gid " +
                            "inner join hrm_mst_tbranch c on b.branch_gid = c.branch_gid ";
                }

                msSQL = " select department_gid,department_name" +
                    " from hrm_mst_tdepartment ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDepartmentdata>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDepartmentdata
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

        public void DaGetEmployeeHistory(string branch_name, string department_name, MdlPayRptEmployeeHistory values)
        {
            try
            {
                msSQL = " select a.user_code,concat(a.user_firstname,' ',a.user_lastname) as employee_name, " +
                    " b.employee_gid,date_format(b.employee_joiningdate, '%d-%m-%Y') as employee_joiningdate,c.branch_name,d.department_name,e.designation_name, " +
                    " cast(case when (case when h.wagestype_gid='wg001' then (select sum(x.net_salary) from pay_trn_tpayment x where x.employee_gid=b.employee_gid) " +
                    " else (select sum(y.total_salary) from pay_trn_tweeklyemployee2payrundtl y where y.employee_gid=b.employee_gid) end) is null then '0.00' " +
                    " else (case when h.wagestype_gid='wg001' then (select sum(x.net_salary) from pay_trn_tpayment x where x.employee_gid=b.employee_gid) " +
                    " else (select sum(y.total_salary) from pay_trn_tweeklyemployee2payrundtl y where y.employee_gid=b.employee_gid) end) end as decimal) as total_salary " +
                    " from adm_mst_tuser a " +
                    " inner join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                    " inner join hrm_mst_tbranch c on b.branch_gid=c.branch_gid " +
                    " inner join hrm_mst_tdepartment d on b.department_gid=d.department_gid " +
                    " inner join adm_mst_tdesignation e on b.designation_gid=e.designation_gid " +
                    " left join hrm_trn_temployeetypedtl h on b.employee_gid=h.employee_gid ";

                


                if (department_name != "all" && branch_name != "all")
                {
                    msSQL += "WHERE c.branch_gid ='" + branch_name + "' and  d.department_gid='" + department_name + "' ";
                }
                else
                {
                    if (department_name != "all")
                    {

                        msSQL += " WHERE d.department_gid ='" + department_name + "' ";
                    }
                    if (branch_name != "all")
                    {

                        msSQL += " WHERE c.branch_gid ='" + branch_name + "' ";
                    }

                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeehistory_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeehistory_list
                        {
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            total_salary = dt["total_salary"].ToString(),




                        });
                        values.employeehistory_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Employee History!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DagetViewEmployeePaymentSummary(string employee_gid, MdlPayRptEmployeeHistory values)
        {
            msSQL = " select a.employee_gid,a.employee_gender, a.identity_no,date_format(a.employee_dob,'%d-%m-%Y') as employee_dob,a.employee_sign,a.bloodgroup, " +
               " a.employee_image, date_format(a.employee_joiningdate,'%d-%m-%Y') as employee_joingdate ,x.employeepreviouscompany_name,format(y.daysalary_rate,3) as daysalary_rate, " +
               " h.employeetype_name,i.wagetype_name,j.workertype_name,k.roll_name,a.nationality,a.employee_diffabled, " +
               " a.employee_emailid,a.employee_mobileno,a.employee_qualification,a.employee_documents, " +
               " a.employee_experience,a.employee_experiencedtl, a.employeereporting_to , a.employment_type , " +
               " b.user_code,b.user_firstname,b.user_lastname, b.user_status,b.usergroup_gid,c.usergroup_code, " +
               " d.branch_name,  e.department_name,f.designation_name, g.jobtype_name,s.section_name, " +
               " (select i.user_firstname from adm_mst_tuser i ,  hrm_mst_temployee j where i.user_gid = j.user_gid " +
               " and a.employeereporting_to = j.employee_gid)  as approveby_name,(date_format(a.employee_joiningdate,'%d/%m/%Y')) as employee_joiningdate, " +
               " ( Select k.user_firstname from adm_mst_tuser k ,hrm_mst_temployee l " +
               "  where k.user_gid = l.user_gid and l.employee_gid = ' " + employee_gid + "')  as approver_name " +
               " FROM hrm_mst_temployee a  LEFT JOIN adm_mst_tuser b ON a.user_gid = b.user_gid " +
               " LEFT JOIN adm_mst_tusergroup c ON b.usergroup_gid = c.usergroup_gid " +
               " LEFT JOIN hrm_mst_tbranch d ON a.branch_gid = d.branch_gid " +
               " LEFT JOIN hrm_mst_tdepartment e ON a.department_gid = e.department_gid  " +
               " LEFT JOIN adm_mst_tdesignation f ON a.designation_gid = f.designation_gid " +
               " LEFT JOIN hrm_mst_tjobtype g ON a.jobtype_gid = g.jobtype_gid " +
               " LEFT JOIN hrm_trn_temployeetypedtl h ON a.employee_gid = h.employee_gid" +
               " LEFT JOIN hrm_mst_twagestype i ON h.wagestype_gid = i.wagestype_gid" +
               " LEFT JOIN hrm_mst_tworkertype j ON h.workertype_gid = j.workertype_gid" +
               " LEFT JOIN hrm_mst_temployeerolltype k ON h.systemtype_gid = k.systemtype_name" +
               " left join hrm_mst_temployeepreviouscompany x on a.employeepreviouscompany_gid=x.employeepreviouscompany_gid " +
               " left join pay_trn_temployee2wage y on a.employee_gid=y.employee_gid " +
               " LEFT JOIN hrm_mst_tsection s ON a.section_gid= s.section_gid " +
               " WHERE a.employee_gid = '" + employee_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<getViewEmployeeReportSummary>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new getViewEmployeeReportSummary
                    {
                        branch_name = dt["branch_name"].ToString(),
                        bloodgroup = dt["bloodgroup"].ToString(),
                        employee_joiningdate = dt["employee_joiningdate"].ToString(),
                        identity_no = dt["identity_no"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        user_code = dt["user_code"].ToString(),
                        user_firstname = dt["user_firstname"].ToString(),
                        user_lastname = dt["user_lastname"].ToString(),
                        designation_name = dt["designation_name"].ToString(),
                        user_status = dt["user_status"].ToString(),
                        employee_gender = dt["employee_gender"].ToString(),
                        employee_dob = dt["employee_dob"].ToString(),
                        employee_emailid = dt["employee_emailid"].ToString(),
                        employee_mobileno = dt["employee_mobileno"].ToString(),
                        employee_qualification = dt["employee_qualification"].ToString(),
                        employee_experience = dt["employee_experience"].ToString(),
                        jobtype_name = dt["jobtype_name"].ToString(),
                        nationality = dt["nationality"].ToString(),
                        daysalary_rate = dt["daysalary_rate"].ToString(),
                        employeepreviouscompany_name = dt["employeepreviouscompany_name"].ToString(),
                        employee_diffabled = dt["employee_diffabled"].ToString(),
                        employeetype_name = dt["employeetype_name"].ToString(),
                        wagetype_name = dt["wagetype_name"].ToString(),
                        workertype_name = dt["workertype_name"].ToString(),
                        roll_name = dt["roll_name"].ToString(),


                    });
                    values.getViewEmployeeReportSummary = getModuleList;
                }
            }
            dt_datatable.Dispose();



        }

        public void DagetViewPromotionHistory(string employee_gid, MdlPayRptEmployeeHistory values)
        {
            msSQL = " select a.employee_gid,a.designation_gid,a.branch_gid,b.branch_name,d.designation_name,concat(u.user_firstname,' ',u.user_lastname) as username, " +
            " a.salarygrade_gid,z.department_name,y.salarygradetemplate_name from hrm_trn_tpromotionhistory a " +
            " inner join hrm_mst_temployee e on e.employee_gid=a.employee_gid " +
            " inner join  hrm_mst_tbranch b on b.branch_gid=a.branch_gid " +
            " inner join adm_mst_tdesignation d on d.designation_gid=a.designation_gid " +
            " inner join hrm_mst_tdepartment z on z.department_gid=e.department_gid " +
            " inner join pay_trn_temployee2salarygradetemplate s on s.employee_gid=e.employee_gid " +
            " inner join pay_mst_tsalarygradetemplate y on s.salarygradetemplate_gid=y.salarygradetemplate_gid " +
            " inner join adm_mst_tuser u on u.user_gid=e.user_gid " +
            " where e.employee_gid='" + employee_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<getViewPromotionHistory>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new getViewPromotionHistory
                    {
                        employee_gid = dt["employee_gid"].ToString(),
                        designation_gid = dt["designation_gid"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        designation_name = dt["designation_name"].ToString(),
                        username = dt["username"].ToString(),
                        salarygrade_gid = dt["salarygrade_gid"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        salarygradetemplate_name = dt["salarygradetemplate_name"].ToString(),

                    });
                    values.getViewPromotionHistory = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DagetViewPaymentDetails(string employee_gid, MdlPayRptEmployeeHistory values)
        {

            msSQL = " select wagestype_gid from hrm_trn_temployeetypedtl where employee_gid='" + employee_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    if (dt["wagestype_gid"] is DBNull)
                    {
                    }
                    else
                    {
                        lswage_gid = dt["wagestype_gid"].ToString();
                    }
                }



                if (lswage_gid == "wg001")
                {

                    msSQL = " select b.employee_gid,concat(c.user_firstname,' ',c.user_lastname) as employee_name,c.user_code, " +
                    " a.payment_month as payment_month, a.payment_year,format(sum(a.net_salary),2) as net_salary from pay_trn_tpayment a " +
                    " inner join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " inner join adm_mst_tuser c on b.user_gid=c.user_gid " +
                    " inner join hrm_trn_temployeetypedtl d on b.employee_gid=d.employee_gid " +
                    " where a.employee_gid='" + employee_gid + "' " +
                    " group by a.payment_month,a.payment_year ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<getViewPaymentDetails>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new getViewPaymentDetails
                            {
                                employee_gid = dt["employee_gid"].ToString(),
                                employee_name = dt["employee_name"].ToString(),
                                user_code = dt["user_code"].ToString(),
                                payment_month = dt["payment_month"].ToString(),
                                payment_year = dt["payment_year"].ToString(),
                                net_salary = dt["net_salary"].ToString(),

                            });
                            values.getViewPaymentDetails = getModuleList;

                        }
                    }

                    dt_datatable.Dispose();
                }

                else
                {
                    msSQL = " select b.employee_gid,concat(c.user_firstname,' ',c.user_lastname) as employee_name,c.user_code, " +
                        " cast(monthname(a.payrun_date) as char) as payment_month,year(a.payrun_date) as payment_year, " +
                        " format(sum(a.total_salary),2) as net_salary from pay_trn_tweeklyemployee2payrundtl a " +
                        " inner join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                        " inner join adm_mst_tuser c on b.user_gid=c.user_gid " +
                        " inner join hrm_trn_temployeetypedtl d on b.employee_gid=d.employee_gid " +
                        " where a.employee_gid='" + employee_gid + "' " +
                        " group by monthname(a.payrun_date),year(a.payrun_date) ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<getViewPaymentDetails>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new getViewPaymentDetails
                            {
                                employee_gid = dt["employee_gid"].ToString(),
                                employee_name = dt["employee_name"].ToString(),
                                user_code = dt["user_code"].ToString(),
                                payment_month = dt["payment_month"].ToString(),
                                payment_year = dt["payment_year"].ToString(),
                                net_salary = dt["net_salary"].ToString(),

                            });
                            values.getViewPaymentDetails = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }



            }
        }
    }
}
    


