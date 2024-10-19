using ems.hrm.Models;
using ems.utilities.Functions;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.WebSockets;

namespace ems.hrm.DataAccess
{
    public class DaExitManagement
    {
        string msSQL, msSQL1, msSQL2, msSQL3, msSQL4;
        DataTable dataTable, dt_leavedtl, dt_initaproval, dt_salaryDtl, dt_component, dt_asset;
        dbconn objconn = new dbconn();
        cmnfunctions cmnfunctions = new cmnfunctions();
        string leavetype_gid, leavetype_name, leavetype_code;
        OdbcDataReader odbcreader;
        string manageremployee_gid, department_name, lsdepartment_name, lsdepartment_gid, department_gid,
            manager_name, lsexitemployee_gid, lsemployee_gid, lsuser_gid, msGetGid_dtl, lsmanageremployee_gid,
            template_content;
        int mnResult;
        public void DaGetExitmanagementSummary(MdlExitmanagement values)
        {
            try
            {
                msSQL = "select a.exitemployee_gid,date_format(a.created_date,'%d-%m-%Y')as created_date,concat(f.user_firstname,' ',f.user_lastname) as employee_name,f.user_code,b.branch_name,c.department_name, " +
               "date_format(e.employee_joiningdate,'%d-%m-%Y') as joining_date,a.overall_status,d.designation_name,a.remarks from hrm_trn_texitemployee a " +
              "inner join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
              "inner join hrm_mst_tdepartment c on a.department_gid=c.department_gid " +
              "inner join adm_mst_tdesignation d on a.designation_gid=d.designation_gid " +
              "inner join hrm_mst_temployee e on a.employee_gid=e.employee_gid " +
              "inner join adm_mst_tuser f on e.user_gid=f.user_gid ";
                dataTable = objconn.GetDataTable(msSQL);
                var GetExitmanagament = new List<GetExitmanagament_list>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dataTable.Rows)
                    {
                        GetExitmanagament.Add(new GetExitmanagament_list
                        {
                            exitemployee_gid = dt["exitemployee_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            joining_date = dt["joining_date"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            remarks = dt["remarks"].ToString(),
                        });
                        values.GetExitmanagament_list = GetExitmanagament;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting whatsapp summary!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }

        public void DaGetEmployeeDetails(string exitemployee_gid, MdlExitmanagement values)
        {
            try
            {
                msSQL = "  select a.exitemployee_gid, concat(d.user_code,'/',d.user_firstname,' ',d.user_lastname,'/',e.department_name) as employee_name , " +
                    "  concat('applied',' ','Exit Requisition',' on ',date_format(a.created_date,'%d-%m-%Y'),'  ' ,'For Relieving at', '  ',   date_format(a.exit_date,'%d-%m-%Y') ,' ','  for',  ' ',' ','the Reason ', a.remarks ) as details_employee" +
                    "  from hrm_trn_texitemployee a  left join hrm_mst_temployee c on c.employee_gid=a.employee_gid " +
                    " left join adm_mst_tuser d on d.user_gid=c.user_gid" +
                    " left join hrm_mst_tdepartment e on e.department_gid=a.department_gid" +
                    " where a.exitemployee_gid='" + exitemployee_gid + "' group by a.exitemployee_gid ";
                dataTable = objconn.GetDataTable(msSQL);
                var GetEmployee = new List<GetEmployee_list>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dataTable.Rows)
                    {
                        GetEmployee.Add(new GetEmployee_list
                        {
                            employee_name = dt["employee_name"].ToString(),
                            details_employee = dt["details_employee"].ToString(),
                        });
                        values.GetEmployee_list = GetEmployee;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting whatsapp summary!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }
        public void DaGetLeaveDetails(string employee_gid, MdlExitmanagement values)
        {
            try
            {

                msSQL = " select cast(concat(date_format(e.attendance_date,'%b'),' - ',year(e.attendance_date)) as char) as Duration ";

                msSQL1 = "select leavetype_gid,leavetype_name,leavetype_code from hrm_mst_tleavetype where 1=1 " +
                    "group by leavetype_gid order by leavetype_gid asc ";
                dt_leavedtl = objconn.GetDataTable(msSQL1);
                if (dt_leavedtl.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_leavedtl.Rows)
                    {
                        msSQL2 = "";
                        leavetype_gid = dt["leavetype_gid"].ToString();
                        leavetype_name = dt["leavetype_name"].ToString();
                        leavetype_code = dt["leavetype_code"].ToString();

                        msSQL2 = " ,(select ifnull(SUM(if(a.day_session='NA','1','0.5')),0) as count   " +
                           " from hrm_trn_tattendance a where a.employee_gid='" + employee_gid + "' and a.employee_attendance='Leave'" +
                           " and a.attendance_type='" + leavetype_code + "'" +
                           " and a.employee_gid='" + employee_gid + "' and month(a.attendance_date)=month(e.attendance_date)" +
                           " and year(a.attendance_date)=year(e.attendance_date)) as '" + leavetype_name + "' ";

                        msSQL += msSQL2;
                    }
                }
                msSQL += ",(select count(employee_attendance) from hrm_trn_tattendance  x " +
                        " where x.employee_gid='" + employee_gid + "' and employee_attendance='Absent' and " +
                        " month(x.attendance_date)=month(e.attendance_date) and year(x.attendance_date)=year(e.attendance_date)) as LOP, " +
                        " (select ifnull(SUM(if(x.attendance_type='OD','1','0.5')),0) from hrm_trn_tattendance  x " +
                        " where x.employee_gid='" + employee_gid + "' and employee_attendance='Onduty' and " +
                        " month(x.attendance_date)=month(e.attendance_date) and year(x.attendance_date)=year(e.attendance_date)) as OD," +
                        " (select ifnull(sum(permission_totalhours),0) as total_hours from hrm_trn_tpermission h where h.permission_status='Approved'" +
                        " and employee_gid='" + employee_gid + "' and month(h.permission_date)=month(e.attendance_date) and year(h.permission_date)=year(e.attendance_date)) as Permission," +
                        " (select ifnull(sum(compoff_noofdays),0) as compoff from hrm_trn_tcompensatoryoff i where i.compensatoryoff_status='Approved'" +
                        " and employee_gid='" + employee_gid + "' and month(i.compensatoryoff_applydate)=month(e.attendance_date) and year(i.compensatoryoff_applydate)=year(e.attendance_date)) as CompOff ";

                msSQL += " From hrm_trn_tattendance e " +
                 " where attendance_date <= date(now()) and attendance_date >=date('" + DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") + "')";

                msSQL += " group by monthname(e.attendance_date) order by year(e.attendance_date) desc, month(e.attendance_date) desc ";
                dataTable = objconn.GetDataTable(msSQL);
                var GetLeaveDetails = new List<GetLeaveDetails_list>();
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dataTable.Rows)
                    {
                        GetLeaveDetails.Add(new GetLeaveDetails_list
                        {
                            Duration = dt["Duration"].ToString(),
                            LOP = dt["LOP"].ToString(),
                            OD = dt["OD"].ToString(),
                            Permission = dt["Permission"].ToString(),
                            CompOff = dt["CompOff"].ToString(),
                        });
                        values.GetLeaveDetails_list = GetLeaveDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting whatsapp summary!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }
        public void DaGetInitiateApproval(string exitemployee_gid, MdlExitmanagement values)
        {
            msSQL = "select a.employee_gid from hrm_mst_temployee a " +
                " left join hrm_trn_texitemployee b on a.employee_gid=b.employee_gid " +
                " where  b.exitemployee_gid='" + exitemployee_gid + "'";
            odbcreader = objconn.GetDataReader(msSQL);
            if (odbcreader.HasRows == true)
            {
                odbcreader.Read();
                manageremployee_gid = odbcreader["employee_gid"].ToString();
                odbcreader.Close();
            }
            msSQL = " select b.department_name,b.department_gid from hrm_mst_temployee a " +
                    " left join hrm_mst_tdepartment b on a.department_gid=b.department_gid " +
                    " where employee_gid='" + manageremployee_gid + "' ";
            odbcreader = objconn.GetDataReader(msSQL);
            if (odbcreader.HasRows == true)
            {
                odbcreader.Read();
                department_name = odbcreader["department_name"].ToString();
                department_gid = odbcreader["department_gid"].ToString();
                odbcreader.Close();
            }
            msSQL = "select concat(c.user_firstname,'',c.user_lastname)as manager_name from hrm_mst_tdepartment a " +
                " left join hrm_mst_temployee b on b.employee_gid=a.department_manager " +
                " left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                " where a.department_gid='" + department_gid + "' ";
            odbcreader = objconn.GetDataReader(msSQL);
            if (odbcreader.HasRows == true)
            {
                odbcreader.Read();
                manager_name = odbcreader["manager_name"].ToString();
                odbcreader.Close();
            }

            List<Initiateapproval_list> initiateapproval = new List<Initiateapproval_list>();

            initiateapproval.Add(new Initiateapproval_list
            {
                manageremployee_gid = manageremployee_gid,
                department_name = department_name,
                department_gid = department_gid,
                manager_name = manager_name,
            });
            values.Initiateapproval_list = initiateapproval;
        }
        public void PostInitiateApproval(string exitemployee_gid, string manager_name,string employee_gid, result lsresult)
        {
            msSQL = "select b.department_name,b.department_gid from hrm_mst_temployee a " +
               " left join hrm_mst_tdepartment b on a.department_gid=b.department_gid " +
               " left join hrm_trn_texitemployee c on c.employee_gid=a.employee_gid " +
               " where exitemployee_gid='" + exitemployee_gid + "'";
            odbcreader = objconn.GetDataReader(msSQL);
            if (odbcreader.HasRows == true)
            {
                odbcreader.Read();
                lsdepartment_name = odbcreader["department_name"].ToString();
                lsdepartment_gid = odbcreader["department_gid"].ToString();
                odbcreader.Close();
            }
            msSQL = "select b.employee_gid from hrm_mst_tdepartment a " +
                " left join hrm_mst_temployee b on b.employee_gid=a.department_manager " +
                " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                " where  a.department_gid='" + lsdepartment_gid + "'";
            odbcreader = objconn.GetDataReader(msSQL);
            if (odbcreader.HasRows == true)
            {
                odbcreader.Read();
                lsmanageremployee_gid = odbcreader["employee_gid"].ToString();
                odbcreader.Close();
            }
            msSQL = " select exitemployee_gid,employee_gid from hrm_trn_texitemployeedtl " +
                "where exitemployee_gid='" + exitemployee_gid + "'";
            odbcreader = objconn.GetDataReader(msSQL);
            if (odbcreader.HasRows == true)
            {
                odbcreader.Read();
                lsexitemployee_gid = odbcreader["exitemployee_gid"].ToString();
                odbcreader.Close();
                lsresult.status = false;
                lsresult.message = " Exit Approval Is Already Initiated ";
                return;
            }
            else
            {
                msSQL = " select employee_gid from hrm_trn_texitemployee " +
                    "where exitemployee_gid='" + exitemployee_gid + "'";
                odbcreader = objconn.GetDataReader(msSQL);
                if (odbcreader.HasRows == true)
                {
                    odbcreader.Read();
                    lsemployee_gid = odbcreader["employee_gid"].ToString();
                    odbcreader.Close();
                }
            }

            msGetGid_dtl = cmnfunctions.GetMasterGID("EXTD");
            msSQL = " insert into hrm_trn_texitemployeedtl ( " +
                           " exitemployeedtl_gid, " +
                           " exitemployee_gid, " +
                           " employee_gid, " +
                           " department_gid, " +
                           " exit_status, " +
                            " manager_gid, " +
                           " department_manager, " +
                           " created_by, " +
                           " created_date " +
                           " ) values ( " +
                           " '" + msGetGid_dtl + "', " +
                           " '" + exitemployee_gid + "', " +
                           " '" + lsemployee_gid + "', " +
                           " '" + lsdepartment_gid + "', " +
                           " 'Pending', " +
                            " '" + lsmanageremployee_gid + "', " +
                           " '" + manager_name + "', " +
                           " '" + employee_gid + "', " +
                           " '" + DateTime.Now.ToString("yyyy-MM-dd") + "') ";
            mnResult = objconn.ExecuteNonQuerySQL(msSQL);

            msSQL = " update hrm_trn_texitemployee set " +
                    " overall_status='Approval Pending' " +
                    " where exitemployee_gid='" + exitemployee_gid + "' and employee_gid='" + lsemployee_gid + "'";
            mnResult = objconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                lsresult.status = true;
                lsresult.message = "Approved !!";
            }
            else
            {
                lsresult.status = false;
                lsresult.message = " Error while Approve !!";
            }
        }
        public void DaGetInitiateApprovalSummary(string exitemployee_gid, MdlExitmanagement values)
        {
            msSQL = " select a.exitemployeedtl_gid, a.department_gid,a.department_manager,b.department_name,a.exit_status " +
                " from hrm_trn_texitemployeedtl a " +
                " left join hrm_mst_tdepartment b on a.department_gid=b.department_gid " +
                " where a.exitemployee_gid='" + exitemployee_gid + "' ";
            dt_initaproval = objconn.GetDataTable(msSQL);
            var GetInitiateApproval = new List<GetInitiateApproval_list>();
            if (dt_initaproval.Rows.Count > 0)
            {
                foreach (DataRow inapr in dt_initaproval.Rows)
                {
                    GetInitiateApproval.Add(new GetInitiateApproval_list
                    {
                        exitemployeedtl_gid = inapr["exitemployeedtl_gid"].ToString(),
                        department_gid = inapr["department_gid"].ToString(),
                        department_manager = inapr["department_manager"].ToString(),
                        department_name = inapr["department_name"].ToString(),
                        exit_status = inapr["exit_status"].ToString(),
                    });
                    values.GetInitiateApproval_list = GetInitiateApproval;
                }
            }
        }
        public MdlExitmanagement DaGetSalaryDetailsSummary(string exitemployee_gid, MdlExitmanagement values)
        {
            var GetSalaryDetailsAll_list = new MdlExitmanagement();

            msSQL = " select a.employee_gid,c.user_gid,a.template_content from hrm_trn_texitemployee a " +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " left join adm_mst_tuser c on c.user_gid=b.user_gid " +
                    "where a.exitemployee_gid='" + exitemployee_gid + "'";
            odbcreader = objconn.GetDataReader(msSQL);
            if (odbcreader.HasRows == true)
            {
                odbcreader.Read();
                lsemployee_gid = odbcreader["employee_gid"].ToString();
                template_content = odbcreader["template_content"].ToString();
                lsuser_gid = odbcreader["user_gid"].ToString();
                odbcreader.Close();
            }
            GetSalaryDetailsAll_list.GetEmployeename_list.Add(new GetEmployeename_list
            {
                employee_gid = lsemployee_gid,
                template_content = template_content,
            });
            msSQL = " select a.payment_gid,b.salary_gid,b.month,b.year,format(b.basic_salary, 2) as basic_salary," +
                    " format(b.earned_basic_salary, 2) as earnedbasic_salary ," +
                    " format(b.earned_gross_salary,2)as gross_salary,format(b.earned_net_salary,2)As net_salary from pay_trn_tpayment a" +
                    " left join pay_trn_tsalary b on a.salary_gid=b.salary_gid" +
                    " left join hrm_mst_temployee c on a.employee_gid=c.employee_gid" +
                    " left join adm_mst_tuser d on d.user_gid=c.user_gid  where d.user_gid='" + lsuser_gid + "'" +
                    " and payment_date <= date(now()) and payment_date >=date('" + DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd") + "')" +
                    "  Order by b.salary_gid desc ";
            dt_salaryDtl = objconn.GetDataTable(msSQL);
            var GetSalaryDetails = new List<GetSalaryDetails_list>();
            if (dt_salaryDtl.Rows.Count > 0)
            {
                foreach (DataRow sd in dt_salaryDtl.Rows)
                {
                    GetSalaryDetailsAll_list.GetSalaryDetails_list.Add
                    (new GetSalaryDetails_list
                    {
                        payment_gid = sd["payment_gid"].ToString(),
                        salary_gid = sd["salary_gid"].ToString(),
                        month = sd["month"].ToString(),
                        year = sd["year"].ToString(),
                        basic_salary = sd["basic_salary"].ToString(),
                        earnedbasic_salary = sd["earnedbasic_salary"].ToString(),
                        gross_salary = sd["gross_salary"].ToString(),
                        net_salary = sd["net_salary"].ToString(),
                    });
                }
            }
            for (int i=0; i< GetSalaryDetailsAll_list.GetSalaryDetails_list.Count; i++ )
            {
                msSQL = " select a.salary_gid,(c.componentgroup_name) as salarycomponent_name,b.salarycomponent_percentage," +
                                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                                            "  from pay_trn_tsalary a" +
                                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                                            " left join pay_trn_tpayment d on  d.salary_gid=a.salary_gid" +
                                            " where a.salary_gid ='" + GetSalaryDetailsAll_list.GetSalaryDetails_list[i].salary_gid + "' and b.salarygradetype='Addition' and c.primecomponent_flag='N'";
                dt_component = objconn.GetDataTable(msSQL);
                var GetAddition = new List<GetAddition_list>();
                if (dt_component.Rows.Count > 0)
                {
                    foreach (DataRow adtncmt in dt_component.Rows)
                    {
                        GetSalaryDetailsAll_list.GetAddition_list.Add
                        (new GetAddition_list
                        {
                            salary_gid = adtncmt["salary_gid"].ToString(),
                            salarycomponent_name = adtncmt["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = adtncmt["salarycomponent_percentage"].ToString(),
                            earned_salarycomponent_amount = adtncmt["earned_salarycomponent_amount"].ToString(),
                        });
                    }
                }
                msSQL = " select a.salary_gid,(c.componentgroup_name) as salarycomponent_name,b.salarycomponent_percentage," +
                                " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                                "  from pay_trn_tsalary a" +
                                " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                                " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                                 " left join pay_trn_tpayment d on  d.salary_gid=a.salary_gid" +
                                " where a.salary_gid ='" + GetSalaryDetailsAll_list.GetSalaryDetails_list[i].salary_gid + "' and b.salarygradetype='Deduction' and c.primecomponent_flag='N'";
                dt_component = objconn.GetDataTable(msSQL);
                var GetDeduction = new List<GetDeduction_list>();
                if (dt_component.Rows.Count > 0)
                {
                    foreach (DataRow dectcmt in dt_component.Rows)
                    {
                        GetSalaryDetailsAll_list.GetDeduction_list.Add
                        (new GetDeduction_list
                        {
                            salary_gid = dectcmt["salary_gid"].ToString(),
                            salarycomponent_name = dectcmt["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = dectcmt["salarycomponent_percentage"].ToString(),
                            earned_salarycomponent_amount = dectcmt["earned_salarycomponent_amount"].ToString(),
                        });
                    }
                }
                msSQL = " select a.salary_gid,(c.componentgroup_name) as salarycomponent_name,b.salarycomponent_percentage,b.othercomponent_type," +
                               " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                               "  from pay_trn_tsalary a" +
                               " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                               " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                               " left join pay_trn_tpayment d on  d.salary_gid=a.salary_gid" +
                               " where a.salary_gid ='" + GetSalaryDetailsAll_list.GetSalaryDetails_list[i].salary_gid + "' and b.salarygradetype='Other' and c.primecomponent_flag='N'";
                dt_component = objconn.GetDataTable(msSQL);
                var GetOther = new List<GetOther_list>();
                if (dt_component.Rows.Count > 0)
                {
                    foreach (DataRow otr in dt_component.Rows)
                    {
                        GetSalaryDetailsAll_list.GetOther_list.Add
                        (new GetOther_list
                        {
                            salary_gid = otr["salary_gid"].ToString(),
                            salarycomponent_name = otr["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = otr["salarycomponent_percentage"].ToString(),
                            earned_salarycomponent_amount = otr["earned_salarycomponent_amount"].ToString(),
                        });
                        values.GetOther_list = GetOther;
                    }
                }
            }            
            return GetSalaryDetailsAll_list;
        }
        public void DaGetAssetCustodianSummary(string employee_gid, MdlExitmanagement values)
        {
            msSQL = " select asset_gid, assetref_no,asset_name," +
                "cast((case when custodian_enddate<>'' then 'Returned' else  'Not Yet Returned' end ) as char) as asset_status " +
                " from hrm_trn_tassetcustodian where employee_gid='" + employee_gid + "' ";
            dt_asset = objconn.GetDataTable (msSQL);
            var GetAssetCustodian = new List<GetAddCustodian_list>();
            if (dt_asset.Rows.Count > 0)
            {
                foreach (DataRow ast in dt_asset.Rows)
                {
                    GetAssetCustodian.Add(new GetAddCustodian_list
                    {
                        asset_gid = ast["asset_gid"].ToString(),
                        assetref_no = ast["assetref_no"].ToString(),
                        asset_name = ast["asset_name"].ToString(),
                        asset_status = ast["asset_status"].ToString(),
                    });
                    values.GetAddCustodian_list = GetAssetCustodian;
                }
            }
        }
        public void Post360Submit(string exitemployee_gid, string editor_content, string employee_gid, result lsresult)
        {
            msSQL = " update hrm_trn_texitemployee set template_content='" + editor_content + "'" +
                " where exitemployee_gid='" + exitemployee_gid + "'";
            mnResult = objconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                lsresult.status = true;
                lsresult.message = "Exit Requisition Approved Successfully !!";
            }
            else
            {
                lsresult.status = false;
                lsresult.message = "Exit Error While Approve !!";
            }
        }
    }
}