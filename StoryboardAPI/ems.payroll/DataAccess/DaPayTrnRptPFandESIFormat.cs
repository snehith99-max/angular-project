using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System.Drawing;
using ems.payroll.Models;
using System.Net.Mail;
using System.IO;
using System.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Web.UI.WebControls;


namespace ems.payroll.DataAccess
{
    public class DaPayTrnRptPFandESIFormat
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string lspop_server, lspop_mail, lspop_password, lscompany, lscompany_code;
        string msINGetGID, msGet_att_Gid, msenquiryloggid;
        string lspath, lspath1, lspath2, mail_path, mail_filepath, pdf_name = "";
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        OdbcDataReader ds_tsalesorderadd;
        int lsamendcount;
        DataTable mail_datatable, dt_datatable;
        string company_logo_path, lsPF1, lsPF2;
        DataTable dt1 = new DataTable();
        DataTable DataTable3 = new DataTable();

        public void DaGetPFandESISummary(MdlPayTrnRptPFandESIFormat values)
        {
            try
            {

                msSQL = " select a.month,a.sal_year,ifnull(convert(format(sum( earnedbasic_salary),2),char) ,'Not Generated') as earnedbasic_salary," +
                " cast(count(b.employee_gid) as char) as no_of_employee," +
                " ifnull(format(sum(employeepf_amount),2),'Not Generated') as pf_amount " +
                " from  pay_trn_tsalmonth a" +
                " left join pay_trn_employeepf b on a.month=b.month and a.sal_year=b.year" +
                " group by month,year" +
                " order by a.sal_year desc,MONTH(STR_TO_DATE(substring(a.month,1,3),'%b')) desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<PfListFormat>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new PfListFormat
                        {
                            month = dt["month"].ToString(),
                            sal_year = dt["sal_year"].ToString(),
                            no_of_employee = dt["no_of_employee"].ToString(),
                            earnedbasic_salary = dt["earnedbasic_salary"].ToString(),
                            pf_amount = dt["pf_amount"].ToString(),
                        });
                        values.PFList_format = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting PF and ESI Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payoll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaPfassign(string month, string sal_year, MdlPayTrnRptPFandESIFormat values)
        {
            try
            {

                msSQL = " select month,sal_year from pay_trn_tsalmonth " +
                        " where month ='" + month + "' and sal_year ='" + sal_year + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Pfassign_type>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Pfassign_type
                        {

                            month = dt["month"].ToString(),
                            sal_year = dt["sal_year"].ToString(),
                        });
                        values.Pfassign_type = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetPFSummary(string month, string year, MdlPayTrnRptPFandESIFormat values)
        {
            string component_percentage;

            msSQL = " select  ifnull(salarycomponent_percentage,0) as salarycomponent_percentage from pay_trn_tsalarydtl a" + " left join pay_trn_tsalary b on b.salary_gid=a.salary_gid " +
                    " where componentgroup_name in('EPF','PF') and year='" + year + "' and month='" + month + "'  and salarycomponent_percentage<> 0 group by salarycomponent_gid ";
            component_percentage = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select  round(((" + component_percentage + ")*0.694),2)";
            lsPF1 = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select  round(((" + component_percentage + ")*0.306),2)";
            lsPF2 = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select a.employee_gid,ifnull(a.uan_no,' ') as uan_no,concat(c.user_firstname,' ',c.user_lastname) as employee_name, format(earned_gross_salary,2) as  Gross, " +
                    " format(round(cast(case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end as decimal(0,0))),2) as EPF, " +
                    " format(round(cast(case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end as decimal(0,0))),2) as EPS, " +
                    " format(round(cast(case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end as decimal(0,0))),2) as EDLI, " +
                    " format(round(cast((case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end *(12))/100 as decimal(0,0))),2) as EE, " +
                    " format(round(cast((case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end *(8.33))/100 as decimal(0,0))),2) as EPS1, " +
                    " format(round(cast((case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end *(3.67))/100 as decimal(0,0))),2) as ER, " +
                    " ifnull(b.lop_days,0) as lop_days,0 as Refunds,0 as Pension_Share,0 as ER_PF_Share,0 as EE_Share,0 as Posting_location_of_the_member " +
                    " from hrm_mst_temployee a " +
                    " left join pay_trn_employeepf b on a.employee_gid=b.employee_gid " +
                    " left join hrm_mst_tbranch f on a.branch_gid=f.branch_gid " +
                    " left join hrm_mst_tdepartment g on a.department_gid=g.department_gid " +
                    " left join adm_mst_tuser c on a.user_gid=c.user_gid " +
                    " where b.month='" + month + "' and b.year='" + year + "' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<PfList>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new PfList
                    {
                        employee_gid = dt["employee_gid"].ToString(),
                        uan_no = dt["uan_no"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                        Gross = dt["Gross"].ToString(),
                        EPF = dt["EPF"].ToString(),
                        EPS = dt["EPS"].ToString(),
                        EDLI = dt["EDLI"].ToString(),
                        EE = dt["EE"].ToString(),
                        EPS1 = dt["EPS1"].ToString(),
                        ER = dt["ER"].ToString(),
                        lop_days = dt["lop_days"].ToString(),
                        Refunds = dt["Refunds"].ToString(),
                        Pension_Share = dt["Pension_Share"].ToString(),
                        ER_PF_Share = dt["ER_PF_Share"].ToString(),
                        EE_Share = dt["EE_Share"].ToString(),
                        Posting_location_of_the_member = dt["Posting_location_of_the_member"].ToString(),
                    });
                    values.pf_listdata = getModuleList;
                }
            }
            dt_datatable.Dispose();

        }

        public void DaGetESISummary(string month, string year, MdlPayTrnRptPFandESIFormat values)
        {

            msSQL = " select a.esi_no,b.actual_month_workingdays,format(b.earned_gross_salary,2) as earned_gross_salary," +
                    " concat(c.user_firstname,' ',c.user_lastname) as employee_name, " +
                    " round(cast((case when b.earned_gross_salary >'21000' then '0.00' else b.earned_gross_salary end *(0.75))/100 as decimal(0,0))) as employee_esi," +
                    " round(cast((case when b.earned_gross_salary >'21000' then '0.00' else b.earned_gross_salary end *(3.25))/100 as decimal(0,0))) as employer_esi," +
                    " null as temp1 from hrm_mst_temployee a " +
                    " left join pay_trn_employeepf b on a.employee_gid=b.employee_gid " +
                    " left join hrm_mst_tbranch f on a.branch_gid=f.branch_gid " +
                    " left join hrm_mst_tdepartment g on a.department_gid=g.department_gid " +
                    " left join adm_mst_tuser c on a.user_gid=c.user_gid " +
                    " where b.month='" + month + "' and b.year='" + year + "' " +
                    " and earned_gross_salary >0.00 " +
                    " order by c.user_firstname ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<esi_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new esi_list
                    {
                        esi_no = dt["esi_no"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                        actual_month_workingdays = dt["actual_month_workingdays"].ToString(),
                        employee_esi = dt["employee_esi"].ToString(),
                        employer_esi = dt["employer_esi"].ToString(),
                        earned_gross_salary = dt["earned_gross_salary"].ToString(),
                 
                    });
                    values.esi_list = getModuleList;
                }
            }
            dt_datatable.Dispose();

        }


        //protected void btndownlodtxt_Click(string month, string year, MdlPayTrnRptPFandESIFormat values)
        //{
        //    objdbconn.OpenConn();
        //    string return_path = null;
        //    string upload_gid = null;
        //    string lssalarycomponent_gid = null;
        //    string component_percentage;
        //    double lsPF1;
        //    double lsPF2;
        //    upload_gid = month + "-" + year;
        //    string lspath = Server.MapPath(@"..\..\PF Report\");
        //    string path = lspath;
        //    if (!System.IO.Directory.Exists(path))
        //    {
        //        System.IO.Directory.CreateDirectory(path);
        //    }
        //    FileUpload uploadid = new FileUpload();

        //    string msSQL = " select  ifnull(salarycomponent_percentage,0) as salarycomponent_percentage from pay_trn_tsalarydtl a" +
        //                   " left join pay_trn_tsalary b on b.salary_gid=a.salary_gid " +
        //                   " where componentgroup_name in('EPF','PF') and year='" + year + "' and month='" + month + "'" +
        //                   " and salarycomponent_percentage<> 0 group by salarycomponent_gid ";
        //    component_percentage = objdbconn.GetExecuteScalar(msSQL);

        //    msSQL = " select  round(((" + component_percentage + ")*0.694),2)";
        //    lsPF1 = objdbconn.GetExecuteScalar(msSQL);

        //    msSQL = "select  round(((" + component_percentage + ")*0.306),2)";
        //    lsPF2 = objdbconn.GetExecuteScalar(msSQL);

        //    msSQL = " select ifnull(a.uan_no,' ') as  uan_no,concat(c.user_firstname,' ',c.user_lastname) as employee_name, " +
        //             " round(cast(case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end as decimal(0,0))) as earned_basic_salary1, " +
        //             " round(cast(case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end as decimal(0,0))) as earned_basic_salary2, " +
        //             " round(cast((case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end *(" + component_percentage + "))/100 as decimal(0,0))) as earned_basic_salary3, " +
        //             " round(cast((case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end *(" + component_percentage + "))/100 as decimal(0,0))) as earned_basic_salary4, " +
        //             " round(cast((case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end *(" + lsPF1 + "))/100 as decimal(0,0))) as earned_basic_salary5, " +
        //             " round(cast((case when b.earnedbasic_salary >='15000' then '15000' else b.earnedbasic_salary end *(" + lsPF2 + "))/100 as decimal(0,0))) as earned_basic_salary6, " +
        //             " ifnull(b.lop_days,0) as lop_days,0 as first from hrm_mst_temployee a " +
        //             " left join pay_trn_employeepf b on a.employee_gid=b.employee_gid " +
        //             " left join hrm_mst_tbranch f on a.branch_gid=f.branch_gid " +
        //             " left join hrm_mst_tdepartment g on a.department_gid=g.department_gid " +
        //             " left join adm_mst_tuser c on a.user_gid=c.user_gid " +
        //             " where b.month='" + month + "' and b.year='" + year + "' ";
        //    DataTable objtbl = objdbconn.GetDatatable(msSQL);

        //    using (StreamWriter f = new StreamWriter(path + upload_gid + ".txt"))
        //    {
        //        string col = "";
        //        string row = "";
        //        int i = 0;
        //        foreach (DataRow row1 in objtbl.Rows)
        //        {
        //            for (i = 0; i < objtbl.Columns.Count; i++)
        //            {
        //                if (!row1.IsNull(i))
        //                {
        //                    f.Write(row1[i].ToString());
        //                }
        //                if ((i + 1) != objtbl.Columns.Count)
        //                {
        //                    f.Write("#~#");
        //                }
        //            }
        //            f.WriteLine();
        //        }
        //        f.WriteLine(row);
        //    }

        //    OdbcConnection myConnection = new OdbcConnection();
        //    myConnection.ConnectionString = Session["ConnectionString"].ToString();
        //    OdbcCommand MyCommand = new OdbcCommand();
        //    MyCommand.Connection = myConnection;
        //    MyCommand.CommandText = msSQL;
        //    MyCommand.CommandType = CommandType.Text;
        //    OdbcDataAdapter MyDA = new OdbcDataAdapter();
        //    MyDA.SelectCommand = MyCommand;
        //    DataSet Myds = new DataSet();
        //    Myds.EnforceConstraints = false;
        //    MyDA.Fill(Myds, "file_txt");

        //    lblErrMsg.Text = "PF Report has been Generated for the Month of  " + Request.QueryString["month"] + " - " + Request.QueryString["year"];
        //}

    }
}