using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ems.hrm.DataAccess
{
    public class DaHrmTrnEmployee360
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty, msGetGID;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_dataTable, objtbl;
        int mnResult;

        public void DaGetemployeedatabinding(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT CONCAT(a.user_firstname,' ',a.user_lastname) AS user_name,a.user_code,b.employee_photo,c.branch_name,d.department_name,e.designation_name FROM adm_mst_tuser a " +
                    " LEFT JOIN hrm_mst_temployee b on a.user_gid=b.user_gid " +
                    " LEFT JOIN hrm_mst_tbranch c on c.branch_gid=b.branch_gid " +
                    " LEFT JOIN hrm_mst_tdepartment d on d.department_gid=b.department_gid " +
                    " LEFT JOIN adm_mst_tdesignation e on e.designation_gid=b.designation_gid WHERE b.employee_gid = '"+ employee_gid + "'";

                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getemployeebinding>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getemployeebinding
                        {
                            user_name = dt["user_name"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_photo = dt["employee_photo"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                          
                        });
                        values.Getemployeebinding = getModuleList;
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


        public void DaGetemployeeinformation(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT b.branch_name,d.department_name,c.designation_name,a.employee_mobileno,a.employee_qualification,a.employee_experience from hrm_mst_temployee a "+
                 " LEFT JOIN hrm_mst_tbranch b on b.branch_gid=a.branch_gid " +
                 " LEFT JOIN adm_mst_tdesignation c on c.designation_gid=a.designation_gid "+
                 " LEFT JOIN hrm_mst_tdepartment d on d.department_gid=a.department_gid " +
                 " WHERE a.employee_gid = '" + employee_gid + "'";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getemployeeinformation>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getemployeeinformation
                        {
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            employee_qualification = dt["employee_qualification"].ToString(),
                            employee_experience = dt["employee_experience"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),

                        });
                        values.Getemployeeinformation = getModuleList;
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

        public void DaGetemployeegeneraldetails(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT father_name,mother_name,marital_status,religion,nationality FROM hrm_trn_tformgeneraldetails " +
                " WHERE employee_gid = '" + employee_gid + "'";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getemployeegeneral>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getemployeegeneral
                        {
                            father_name = dt["father_name"].ToString(),
                            mother_name = dt["mother_name"].ToString(),
                            marital_status = dt["marital_status"].ToString(),
                            religion = dt["religion"].ToString(),
                            nationality = dt["nationality"].ToString()

                        });
                        values.Getemployeegeneral = getModuleList;
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
        public void Dagetemployeeaccount(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT quarter_no,lic_no,pf_no,stateinsure_no,date_format(pf_doj, '%d-%m-%Y')as pf_doj FROM hrm_trn_tformaccountdetails " +
                " WHERE employee_gid = '" + employee_gid + "'";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<getaccount>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new getaccount
                        {
                            quarter_no = dt["quarter_no"].ToString(),
                            lic_no = dt["lic_no"].ToString(),
                            pf_no = dt["pf_no"].ToString(),
                            stateinsure_no = dt["stateinsure_no"].ToString(),
                            pf_doj = dt["pf_doj"].ToString()

                        });
                        values.getaccount = getModuleList;
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
        public void getemployeeaddress(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT employee_village,employee_taluk,employee_subdivision,employee_po,employee_district,employee_state FROM hrm_trn_tformgeneraldetails "+
                " WHERE employee_gid =  '" + employee_gid + "'";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<getaddrees>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new getaddrees
                        {
                            employee_village = dt["employee_village"].ToString(),
                            employee_taluk = dt["employee_taluk"].ToString(),
                            employee_subdivision = dt["employee_subdivision"].ToString(),
                            employee_po = dt["employee_po"].ToString(),
                            employee_district = dt["employee_district"].ToString(),
                            employee_state = dt["employee_state"].ToString()

                        });
                        values.getaddrees = getModuleList;
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

        public void DagetGetpaymentdetails(MdlHrmTrnEmployee360 values,string employee_gid)
        {
            try
            {
                msSQL = " SELECT a.employee_gid,b.salary_gid,b.earned_gross_salary,a.payment_date,a.payment_month,a.payment_year,format(a.net_salary,2) as net_salary ,a.account_no,a.bank_branch FROM pay_trn_tpayment a " +
                " left join pay_trn_tsalary b on a.salary_gid = b.salary_gid " +
                " WHERE a.employee_gid = '" + employee_gid + "' and a.payment_year>='" + DateTime.Now.ToString("yyyy") + "' ";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPaymentdetails>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new GetPaymentdetails
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            salary_gid = dt["salary_gid"].ToString(),
                            earned_gross_salary = dt["earned_gross_salary"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            payment_month = dt["payment_month"].ToString(),
                            payment_year = dt["payment_year"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            account_no = dt["account_no"].ToString(),
                            bank_branch = dt["bank_branch"].ToString(),
                        });
                        values.GetPaymentdetails = getModuleList;
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

        public void DagetGetloandetails(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT a.loan_gid,a.employee_gid,b.salary_gid,b.earned_gross_salary,a.created_date,a.loan_refno,format(a.loan_amount,2) as loan_amount, a.loan_duration,date_format(a.loan_repayment_startfrom, '%d-%m-%Y') as loan_repayment_startfrom ,a.loan_remarks,a.type,a.grade_name,format(a.net_salary,2) as net_salary  FROM pay_trn_tloan a " +
                " left join pay_trn_tsalary b on a.employee_gid = b.employee_gid " +
                " WHERE a.employee_gid = '" + employee_gid + "' and year(a.loan_repayment_startfrom)>='" + DateTime.Now.ToString("yyyy") + "' ";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getloandetails>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getloandetails
                        {
                            loan_gid = dt["loan_gid"].ToString(),
                            loan_refno = dt["loan_refno"].ToString(),
                            salary_gid = dt["salary_gid"].ToString(),
                            earned_gross_salary = dt["earned_gross_salary"].ToString(),
                            loan_remarks = dt["loan_remarks"].ToString(),
                            loan_repayment_startfrom = dt["loan_repayment_startfrom"].ToString(),
                            loan_duration = dt["loan_duration"].ToString(),
                            loan_amount = dt["loan_amount"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            type = dt["type"].ToString(),
                            grade_name = dt["grade_name"].ToString(),

                        });
                        values.Getloandetails = getModuleList;
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
        public void DagetGetattendancetails(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT  " +
                        " CAST(monthname(a.attendance_date) as char) as Month," +
                        " YEAR(a.attendance_date) as Year,b.employee_gid," +
                        " CAST(SUM(CASE WHEN a.attendance_type='P'  THEN 1  ELSE 0 END ) AS DECIMAL) AS present, " +
                        " CAST(SUM(CASE WHEN a.attendance_type='A'  THEN 1  ELSE 0 END ) AS DECIMAL) AS absent, " +
                        " CAST(SUM(CASE WHEN a.attendance_type='WH' THEN 1  ELSE 0 END ) AS DECIMAL) AS weekoff," +
                        " CAST(SUM(CASE WHEN a.attendance_type='NH' THEN 1  ELSE 0 END ) AS DECIMAL) AS Holiday, " +
                        " CAST(SUM(CASE WHEN a.day_session='AL' THEN 0.5  " +
                        "               WHEN a.day_session='FL' THEN 0.5 " +
                        "               WHEN a.employee_attendance='Leave' AND a.day_session='NA' THEN 1 ELSE 0 END ) AS DECIMAL) AS leave_count, " +
                        " CAST(IFNULL((select count(distinct(attendance_gid)) FROM hrm_trn_tattendance x " +
                        " LEFT JOIN hrm_mst_temployee m on x.employee_gid=m.employee_gid " +
                        " LEFT JOIN hrm_mst_tmastersettings y on m.branch_gid=y.branch_gid " +
                        " LEFT JOIN hrm_trn_temployee2shifttypedtl z on x.shifttype_gid=z.employee2shifttypedtl_gid " +
                        " WHERE b.employee_gid=m.employee_gid and month(a.attendance_date)=month(x.attendance_date) and year(a.attendance_date)=year(x.attendance_date) and " +
                        " (CAST(concat(x.attendance_date,' ',sec_to_time(time_to_sec(ifnull(x.login_time,'00:00:00'))-time_to_sec(cast(concat(x.attendance_date,' ','00:', " +
                        " CASE when length(ifnull(y.grace_time,'0'))=1 " +
                        " then concat('0',(ifnull(y.grace_time,'0'))) else y.grace_time end,':00')as datetime)))) as datetime)> " +
                        " CAST(concat(x.attendance_date,' ',z.shift_fromhours,':',z.shift_fromminutes,':00')as datetime)) " +
                        " GROUP BY month(a.attendance_date), year(a.attendance_date) " +
                        " ORDER BY year(a.attendance_date) asc , month(a.attendance_date) asc),0) AS DECIMAL) as late_count, " +
                        " CAST(IFNULL((select count(permission_gid) from hrm_trn_tpermission j " +
                        " LEFT JOIN hrm_mst_temployee m on m.employee_gid=j.employee_gid " +
                        " WHERE(b.employee_gid = m.employee_gid And Month(a.attendance_date) = Month(j.permission_date) And Year(a.attendance_date) = Year(j.permission_date)) " +
                        " GROUP BY month(a.attendance_date), year(a.attendance_date)" +
                        " ORDER BY year(a.attendance_date) asc , month(a.attendance_date) asc),0) AS DECIMAL) as permission_count," +
                        " FORMAT(IFNULL((select count(distinct(attendance_gid)) from hrm_trn_tattendance x " +
                        " LEFT JOIN hrm_mst_temployee m on x.employee_gid=m.employee_gid " +
                        " LEFT JOIN hrm_mst_tmastersettings y on m.branch_gid=y.branch_gid " +
                        " LEFT JOIN hrm_trn_temployee2shifttypedtl z on x.shifttype_gid=z.employee2shifttypedtl_gid " +
                        " WHERE b.employee_gid=m.employee_gid and month(a.attendance_date)=month(x.attendance_date) and year(a.attendance_date)=year(x.attendance_date) and " +
                        " (CAST(concat(x.attendance_date,' ',sec_to_time(time_to_sec(ifnull(x.logout_time,'00:00:00'))+time_to_sec(cast(concat(x.attendance_date,' ','00:', " +
                        " CASE when length(ifnull(y.grace_time,'0'))=1 " +
                        " then concat('0',(ifnull(y.grace_time,'0'))) else y.grace_time end,':00')as datetime)))) as datetime)< " +
                        " CAST(concat(x.attendance_date,' ',z.shift_fromhours,':',z.shift_fromminutes,':00')as datetime)) " +
                        " GROUP BY month(a.attendance_date), year(a.attendance_date) " +
                        " ORDER BY year(a.attendance_date) asc , month(a.attendance_date) asc),0),2) as earlyout_count " +
                        " FROM hrm_trn_tattendance a " +
                        " LEFT JOIN hrm_mst_temployee b ON a.employee_gid=b.employee_gid " +
                        " WHERE b.employee_gid  = '" + employee_gid + "' " +
                        " AND  year(a.attendance_date)>='" + DateTime.Now.ToString("yyyy") + "' " +
                        " GROUP BY month(a.attendance_date), year(a.attendance_date) " +
                        " ORDER BY year(a.attendance_date) asc , month(a.attendance_date) asc ";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getattendancedetails>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getattendancedetails
                        {
                            Month = dt["Month"].ToString(),
                            Year = dt["Year"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            present = dt["present"].ToString(),
                            absent = dt["absent"].ToString(),
                            weekoff = dt["weekoff"].ToString(),
                            leave_count = dt["leave_count"].ToString(),
                            Holiday = dt["Holiday"].ToString(),
                            late_count = dt["late_count"].ToString(),
                            permission_count = dt["permission_count"].ToString(),
                            earlyout_count = dt["earlyout_count"].ToString(),
                           

                        });
                        values.Getattendancedetails = getModuleList;
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
        public void DagetGetststutorydetails(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = " select format(earned_salarycomponent_amount,2) as eligible_component,b.salary_gid, " +
                " format(earnedemployer_salarycomponentamount,2) as eligibleemployer_component, "+
                " a.salarycomponent_name,b.month,b.year from pay_trn_tsalarydtl a "+
                " inner join pay_trn_tsalary b on a.salary_gid=b.salary_gid " +
                " inner join pay_mst_tsalarycomponent c on a.salarycomponent_gid=c.salarycomponent_gid " +
                " where b.employee_gid='" + employee_gid + "' and statutory_flag='Y' group by b.month,b.year,a.salarycomponent_name " +
                " order by b.year desc,MONTH(STR_TO_DATE(substring(b.month,1,3),'%b')) desc  ";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getstatutorydetails>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getstatutorydetails
                        {
                            eligible_component = dt["eligible_component"].ToString(),
                            salary_gid = dt["salary_gid"].ToString(),
                            eligibleemployer_component = dt["eligibleemployer_component"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            month = dt["month"].ToString(),
                            year = dt["year"].ToString(),
                           

                        });
                        values.Getstatutorydetails = getModuleList;
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
        public void DagetGetworkexperiencedetails(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = "select experience_gid, company_name, job_title, date_format(from_date,'%Y-%m-%d') as from_date,date_format(to_date,'%Y-%m-%d') as to_date, description, employee_gid " +
                " from hrm_trn_tworkexperience where employee_gid='" + employee_gid + "'";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getworkexperienedetails>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getworkexperienedetails
                        {
                            experience_gid = dt["experience_gid"].ToString(),
                            company_name = dt["company_name"].ToString(),
                            job_title = dt["job_title"].ToString(),
                            from_date = dt["from_date"].ToString(),
                            to_date = dt["to_date"].ToString(),
                            description = dt["description"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                          

                        });
                        values.Getworkexperienedetails = getModuleList;
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
        public void DagetGetdocumentdetails(MdlHrmTrnEmployee360 values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT a.document_gid ,date_format(a.created_date,'%d-%m-%y') as created_date,CONCAT(c.user_firstname,' ',c.user_lastname) " +
                " AS created_by,a.document_location,a.document_name FROM hrm_mst_temployeedocument a" +
                " left join hrm_mst_temployee b on b.user_gid=a.created_by" +
                " LEFT JOIN adm_mst_tuser c on b.user_gid=c.user_gid" +
                " where a.employee_gid='" + employee_gid + "'";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getdocumentdetails>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getdocumentdetails
                        {
                            document_gid = dt["document_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            document_location = dt["document_location"].ToString(),
                            document_name = dt["document_name"].ToString(),                            

                        });
                        values.Getdocumentdetails = getModuleList;
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