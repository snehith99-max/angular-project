using System;
using System.Collections.Generic;
using System.Web;
using ems.payroll.Models;
using ems.utilities.Functions;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using File = System.IO.File;
using System.Data;
using System.Globalization;
using System.Data.Odbc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using MySqlX.XDevAPI;
using System.Diagnostics.Eventing.Reader;
using MySql.Data.MySqlClient;
using System.Net.Http;
using System.Net.Http.Headers;


namespace ems.payroll.DataAccess
{
    public class DaPayMstAssessment
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string msGetGid, pdf_name, lspath2, lspath1, mail_path, final_path, lsgrosssalary;
        double lsgross_salary, lsgross_salary_new;

        public void Daassessmentsummary(MdlPayMstAssessment values)
        {
            try
            {
                msSQL = " select assessment_gid, cast(concat(assessmentyear_startdate,' ','to',' ', assessmentyear_enddate) as char) as duration, " +
                        " cast(concat(financialyear_startdate,' ','to',' ', financialyear_enddate) as char) as fin_duration " +
                        " from pay_mst_tassessmentyear " +
                        " order by assessment_gid asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<assessmentsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new assessmentsummary_list
                        {
                            assessment_gid = dt["assessment_gid"].ToString(),
                            duration = dt["duration"].ToString(),
                            fin_duration = dt["fin_duration"].ToString(),
                        });
                        values.assessmentsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading assessment details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetassessmentyear(MdlPayMstAssessment values, string assessment_gid)
        {
            try
            {
                msSQL = " select assessment_gid, cast(concat(assessmentyear_startdate,' ','to',' ', assessmentyear_enddate) as char) as duration, " +
                        " cast(concat(financialyear_startdate,' ','to',' ', financialyear_enddate) as char) as fin_duration " +
                        " from pay_mst_tassessmentyear " +
                        " where assessment_gid = '" + assessment_gid + "' " +
                        " order by assessment_gid asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<assessmentsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new assessmentsummary_list
                        {
                            assessment_gid = dt["assessment_gid"].ToString(),
                            duration = dt["duration"].ToString(),
                            fin_duration = dt["fin_duration"].ToString(),
                        });
                        values.assessmentsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading assessment details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void Daassignempsummary(string assessment_gid, MdlPayMstAssessment values)
        {
            try
            {
                msSQL = " Select distinct c.employee_gid, a.user_code, concat(a.user_firstname,' ',a.user_lastname) as employee_name, " +
                        " d.designation_name, e.branch_name, g.department_name " +
                        " from hrm_mst_temployee c " +
                        " inner join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                        " inner join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                        " inner join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                        " inner join adm_mst_tuser a on c.user_gid=a.user_gid " +
                        " where a.user_status = 'Y' " +
                        " and c.employee_gid not in " +
                        " (select x.employee_gid from pay_trn_tassessment2employee x where x.assessment_gid='" + assessment_gid + "') " +
                        " order by e.branch_name,a.user_code asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<assignempsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new assignempsummary_list
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                        });
                        values.assignempsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading assigning employee details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostassignemployee(postassignemployeelist values, string employee_gid)
        {
            try
            {
                foreach (var data in values.assignempsummary_list)
                {
                    string msGetGid = objcmnfunctions.GetMasterGID("PAAE");

                    msSQL = " select DATE_FORMAT(assessmentyear_startdate, '%Y-%m-%d') as assessmentyear_startdate, DATE_FORMAT(assessmentyear_enddate, '%Y-%m-%d') as assessmentyear_enddate from pay_mst_tassessmentyear " +
                            " where assessment_gid='" + values.assessment_gid + "'";

                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                    if (objMySqlDataReader.HasRows == true)
                    {
                        objMySqlDataReader.Read();
                        string assessmentyear_startdate = objMySqlDataReader["assessmentyear_startdate"].ToString();
                        string assessmentyear_enddate = objMySqlDataReader["assessmentyear_enddate"].ToString();
                        objMySqlDataReader.Close();

                        msSQL = " select b.employee_gid, sum(a.earned_basic_salary) as total_basic, " +
                                " sum(a.earned_gross_salary) as total_gross, " +
                                " (select sum(x.earned_salarycomponent_amount) from pay_trn_tsalarydtl x " +
                                " inner join pay_trn_tsalary y on x.salary_gid = y.salary_gid " +
                                " inner join pay_trn_tpayment z on y.salary_gid = z.salary_gid " +
                                " where y.employee_gid='" + data.employee_gid + "' and x.salarygradetype='Addition' " +
                                " and (cast(concat(z.payment_year,'-',MONTH(STR_TO_DATE(substring(z.payment_month,1,3),'%b')),'-01') as date)  between '" + assessmentyear_startdate + "'  and '" + assessmentyear_enddate + "')) as total_addition, " +
                                " (select sum(x.earned_salarycomponent_amount) from pay_trn_tsalarydtl x " +
                                " inner join pay_trn_tsalary y on x.salary_gid=y.salary_gid " +
                                " inner join pay_trn_tpayment z on y.salary_gid=z.salary_gid " +
                                " where y.employee_gid='" + data.employee_gid + "' and  x.salarygradetype='Deduction' " +
                                " and (cast(concat(z.payment_year,'-',MONTH(STR_TO_DATE(substring(z.payment_month,1,3),'%b')),'-01') as date)  between '" + assessmentyear_startdate + "'  and '" + assessmentyear_enddate + "')) as total_deduction, " +
                                " sum(a.earned_net_salary) as total_net " + " from pay_trn_tsalary a " +
                                " inner join pay_trn_tpayment b on a.salary_gid=b.salary_gid " +
                                " where (cast(concat(payment_year,'-',MONTH(STR_TO_DATE(substring(b.payment_month,1,3),'%b')),'-01') as date)  between '" + assessmentyear_startdate + "'  and '" + assessmentyear_enddate + "') " + " and b.employee_gid='" + data.employee_gid + "' " +
                                " group by b.employee_gid ";

                        dt_datatable = objdbconn.GetDataTable(msSQL);
                    }

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " insert into pay_trn_tassessment2employee(" +
                            " assessment2employee_gid, " +
                            " assessment_gid, " +
                            " employee_gid, " +
                            " basic_salary, " +
                            " total_addition, " +
                            " gross_salary, " +
                            " total_deduction," +
                            " net_salary) " +
                            " values( " +
                            "'" + msGetGid + "', " +
                            "'" + values.assessment_gid + "', " +
                            "'" + data.employee_gid + "', " +
                            "'" + dt["total_basic"].ToString() + "'," +
                            "'" + dt["total_addition"].ToString() + "'," +
                            "'" + dt["total_gross"].ToString() + "'," +
                            "'" + dt["total_deduction"].ToString() + "'," +
                            "'" + dt["total_net"].ToString() + "') ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Employee selected Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While selecting employee";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while selecting employee details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void Dagenerateformsummary(string assessment_gid, MdlPayMstAssessment values)
        {
            try
            {
                msSQL = " Select distinct c.employee_gid, z.assessment_gid, a.user_code, concat(a.user_firstname,' ',a.user_lastname) as employee_name,z.assessment_gid, " +
                        " d.designation_name, e.branch_name, g.department_name " +
                        " from pay_trn_tassessment2employee z " +
                        " inner join hrm_mst_temployee c on z.employee_gid = c.employee_gid " +
                        " inner join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                        " inner join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                        " inner join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                        " inner join adm_mst_tuser a on c.user_gid=a.user_gid " +
                        " where z.assessment_gid='" + assessment_gid + "' " +
                        " order by e.branch_name,a.user_code asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<generateformsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new generateformsummary_list
                        {
                            branch_name = dt["branch_name"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            assessment_gid = dt["assessment_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                        });
                        values.generateformsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading assigning employee details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public MdlPersonalData DaGetPersonaldata(string employee_gid)
        {
            try
            {
                MdlPersonalData objpersonaldatalist = new MdlPersonalData();

                msSQL = " select permanentaddress_gid from hrm_trn_temployeedtl " +
                        " where employee_gid='" + employee_gid + "' ";

                string lspermanentaddress_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select b.user_firstname, b.user_lastname, Date_Format(a.employee_dob,'%Y-%m-%d') as employee_dob, a.employee_emailid, a.employee_gender, " +
                        " a.pan_no, a.uan_no, a.bloodgroup, a.employee_mobileno, x.address1, x.address2, x.city, x.state, x.postal_code, z.country_name " +
                        " from hrm_mst_temployee a inner join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " left join adm_mst_taddress x on a.employee_gid = x.parent_gid " +
                        " left join hrm_trn_temployeedtl y on x.address_gid = y.permanentaddress_gid and y.permanentaddress_gid='" + lspermanentaddress_gid + "' " +
                        " left join adm_mst_tcountry z on x.country_gid = z.country_gid " +
                        " where a.employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();

                    objpersonaldatalist.user_firstname = objMySqlDataReader["user_firstname"].ToString();
                    objpersonaldatalist.user_lastname = objMySqlDataReader["user_lastname"].ToString();
                    objpersonaldatalist.employee_dob = objMySqlDataReader["employee_dob"].ToString();
                    objpersonaldatalist.employee_emailid = objMySqlDataReader["employee_emailid"].ToString();
                    objpersonaldatalist.employee_gender = objMySqlDataReader["employee_gender"].ToString();
                    objpersonaldatalist.pan_no = objMySqlDataReader["pan_no"].ToString();
                    objpersonaldatalist.uan_no = objMySqlDataReader["uan_no"].ToString();
                    objpersonaldatalist.bloodgroup = objMySqlDataReader["bloodgroup"].ToString();
                    objpersonaldatalist.employee_mobileno = objMySqlDataReader["employee_mobileno"].ToString();
                    objpersonaldatalist.address1 = objMySqlDataReader["address1"].ToString();
                    objpersonaldatalist.address2 = objMySqlDataReader["address2"].ToString();
                    objpersonaldatalist.city = objMySqlDataReader["city"].ToString();
                    objpersonaldatalist.state = objMySqlDataReader["state"].ToString();
                    objpersonaldatalist.postal_code = objMySqlDataReader["postal_code"].ToString();
                    objpersonaldatalist.country_name = objMySqlDataReader["country_name"].ToString();

                    objMySqlDataReader.Close();
                }
                return objpersonaldatalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                ex.ToString();
                return null;
            }
        }
        public void DaUpdatePersonalInfo(string employee_gid, updatepersonalinfolist values)
        {
            try
            {
                string uiDateStr2 = values.dob;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string dob = uiDate2.ToString("yyyy-MM-dd");

                msSQL = " select user_gid from hrm_mst_temployee where employee_gid='" + employee_gid + "'";
                string lsUserGid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select permanentaddress_gid from hrm_trn_temployeedtl " +
                        " where a.employee_gid='" + employee_gid + "' ";

                string lspermanentaddress_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update adm_mst_tuser set " +
                        " user_firstname = '" + values.first_name + "'," +
                        " user_lastname = '" + values.last_name + "'," +
                        " updated_by = '" + employee_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                        " where user_gid = '" + lsUserGid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " update hrm_mst_temployee set " +
                        " employee_dob='" + dob + "'," +
                        " employee_emailid = '" + values.email_id + "'," +
                        " employee_gender = '" + values.active_flag + "'," +
                        " pan_no = '" + values.pan_number + "'," +
                        " uan_no = '" + values.uan_number + "'," +
                        " bloodgroup = '" + values.blood_group + "'," +
                        " employee_mobileno = '" + values.phone + "'," +
                        " updated_by = '" + employee_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                        " where employee_gid = '" + employee_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " update adm_mst_taddress set " +
                        " country_gid='" + values.country + "'," +
                        " address1 = '" + values.address_line1 + "'," +
                        " address2 = '" + values.address_line2 + "'," +
                        " city = '" + values.city + "'," +
                        " postal_code = '" + values.postal_code + "'," +
                        " state = '" + values.state + "'," +
                        " updated_by = '" + employee_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                        " where address_gid = '" + lspermanentaddress_gid + "' and " +
                        " parent_gid = '" + employee_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Personal Information Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while updating personal information";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetfinyeardropdown(MdlPayMstAssessment values)
        {
            try
            {
                msSQL = " SELECT finyear_gid, CONCAT(YEAR(fyear_start), ' - ', YEAR(IFNULL(fyear_end, NOW()))) AS finyear_range FROM adm_mst_tyearendactivities ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getfinyeardropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getfinyeardropdown
                        {
                            finyear_gid = dt["finyear_gid"].ToString(),
                            finyear_range = dt["finyear_range"].ToString(),

                        });
                        values.Getfinyeardropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Finyear!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostIncometax(HttpRequest httpRequest, string user_gid, result objResult)
        {

            // attachment get function

            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            HttpPostedFile httpPostedFile;

            string lspath;
            string msGetGid;

            msSQL = "Select company_code  from adm_mst_tcompany ";
            string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            //split function

            string finyear_range = httpRequest.Form[1];
            string document_type = httpRequest.Form[2];
            string document_title = httpRequest.Form[3];
            string remarks = httpRequest.Form[4];
            string fName = httpRequest.Form[5];



            MemoryStream ms = new MemoryStream();
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        //string lsfile_gid = msdocument_gid + FileExtension;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        bool status1;

                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Payroll/Assessment/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                        ms.Close();
                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "Payroll/Assessment/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";



                        try
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("TDMP");
                            {
                                msSQL = " insert into pay_trn_ttaxdocument " +
                                        " (taxdocument_gid, " +
                                        " fin_year ," +
                                        " documenttype_gid, " +
                                        " document_title," +
                                        " remarks, " +
                                        " document_upload," +
                                        " created_by, " +
                                        " created_date) " +
                                        " values( " +
                                        "'" + msGetGid + "'," +
                                        "'" + finyear_range + "'," +
                                        "'" + document_type + "'," +
                                        "'" + document_title + "'," +
                                        "'" + remarks + "'," +
                                        "'" + final_path + msdocument_gid + FileExtension + "'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            }

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                objResult.status = true;
                                objResult.message = "Income Tax document details submitted successfully";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error occured while submitting income tax document details !!";
                            }
                        }
                        catch (Exception ex)
                        {
                            objResult.message = "Exception occured while loading Finyear!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }
        public void DaGetincometaxsummary(MdlPayMstAssessment values)
        {
            try
            {
                msSQL = " select a.taxdocument_gid,a.document_upload, fin_year, date_format(a.created_date,'%Y-%m-%d') as created_date, a.documenttype_gid, a.document_title " +
                        " from pay_trn_ttaxdocument a left join adm_mst_tyearendactivities b on a.finyear_gid = b.finyear_gid " +
                        " ORDER BY taxdocument_gid DESC ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<incometaxsummary_lists>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new incometaxsummary_lists
                        {
                            taxdocument_gid = dt["taxdocument_gid"].ToString(),
                            finyear_range = dt["fin_year"].ToString(),
                            created_date = Convert.ToDateTime(dt["created_date"].ToString()),
                            documenttype_gid = dt["documenttype_gid"].ToString(),
                            document_title = dt["document_title"].ToString(),
                            document_upload = dt["document_upload"].ToString(),

                        });
                        values.incometaxsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Finyear!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void Dadeleteincometaxsummary(string taxdocument_gid, MdlPayMstAssessment values)
        {
            try
            {

                msSQL = "  delete from pay_trn_ttaxdocument where taxdocument_gid='" + taxdocument_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "IncomeTax Document Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting IncomeTax Document";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting IncomeTax Document!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostQuartersInfo(postquartersinfolist values)
        {
            try
            {
                msSQL = " delete from pay_trn_ttdssummary where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                string msGetGid = objcmnfunctions.GetMasterGID("PAQA");

                msSQL = " insert into pay_trn_ttdssummary  ( " +
                        " tdssummary_gid, " +
                        " assessment_gid, " +
                        " employee_gid, " +
                        " tdsquarter1_receiptno, " +
                        " tdsquarter1_paidcredited, " +
                        " tdsquarter1_amount_deducted, " +
                        " tdsquarter1_amount_deposited, " +
                        " tdsquarter2_receiptno, " +
                        " tdsquarter2_paidcredited, " +
                        " tdsquarter2_amount_deducted, " +
                        " tdsquarter2_amount_deposited, " +
                        " tdsquarter3_receiptno, " +
                        " tdsquarter3_paidcredited, " +
                        " tdsquarter3_amount_deducted, " +
                        " tdsquarter3_amount_deposited, " +
                        " tdsquarter4_receiptno, " +
                        " tdsquarter4_paidcredited, " +
                        " tdsquarter4_amount_deducted, " +
                        " tdsquarter4_amount_deposited, " +
                        " totalamount_paidcredited, " +
                        " tdsquarter_totalamount_deducted, " +
                        " tdsquarter_totalamount_deposited, " +
                        " created_by, " +
                        " created_date " +
                        " ) values ( " +
                        "'" + msGetGid + "'," +
                        "'" + values.assessment_gid + "'," +
                        "'" + values.employee_gid + "'," +
                        "'" + values.q1_rpt_original_statement + "'," +
                        "'" + values.q1_amt_paid_credited + "'," +
                        "'" + values.q1_amt_tax_deducted + "'," +
                        "'" + values.q1_amt_tax_deposited + "'," +
                        "'" + values.q2_rpt_original_statement + "'," +
                        "'" + values.q2_amt_paid_credited + "'," +
                        "'" + values.q2_amt_tax_deducted + "'," +
                        "'" + values.q2_amt_tax_deposited + "'," +
                        "'" + values.q3_rpt_original_statement + "'," +
                        "'" + values.q3_amt_paid_credited + "'," +
                        "'" + values.q3_amt_tax_deducted + "'," +
                        "'" + values.q3_amt_tax_deposited + "'," +
                        "'" + values.q4_rpt_original_statement + "'," +
                        "'" + values.q4_amt_paid_credited + "'," +
                        "'" + values.q4_amt_tax_deducted + "'," +
                        "'" + values.q4_amt_tax_deposited + "'," +
                        "'" + values.total_amt_paid_credited + "'," +
                        "'" + values.total_amt_tax_deducted + "'," +
                        "'" + values.total_amt_tax_deposited + "', " +
                        "'" + values.employee_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Quarter Details are Inserted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while inserting quarter details";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public MdlQuartersData DaGetQuatersdata(string employee_gid)
        {
            try
            {
                MdlQuartersData objquartersdatalist = new MdlQuartersData();

                msSQL = " select tdsquarter1_receiptno, format(tdsquarter1_paidcredited, 2) as tdsquarter1_paidcredited, format(tdsquarter1_amount_deposited, 2) as tdsquarter1_amount_deposited, format(tdsquarter1_amount_deducted, 2) as tdsquarter1_amount_deducted, " +
                        " tdsquarter2_receiptno, format(tdsquarter2_paidcredited, 2) as tdsquarter2_paidcredited, format(tdsquarter2_amount_deposited, 2) as tdsquarter2_amount_deposited, format(tdsquarter2_amount_deducted, 2) as tdsquarter2_amount_deducted, " +
                        " tdsquarter3_receiptno, format(tdsquarter3_paidcredited, 2) as tdsquarter3_paidcredited, format(tdsquarter3_amount_deposited, 2) as tdsquarter3_amount_deposited, format(tdsquarter3_amount_deducted, 2) as tdsquarter3_amount_deducted, " +
                        " tdsquarter4_receiptno, format(tdsquarter4_paidcredited, 2) as tdsquarter4_paidcredited, format(tdsquarter4_amount_deposited, 2) as tdsquarter4_amount_deposited, format(tdsquarter4_amount_deducted, 2) as tdsquarter4_amount_deducted, " +
                        " format(totalamount_paidcredited, 2) as totalamount_paidcredited, format(tdsquarter_totalamount_deposited, 2) as tdsquarter_totalamount_deposited, format(tdsquarter_totalamount_deducted, 2) as tdsquarter_totalamount_deducted " +
                        " from pay_trn_ttdssummary " +
                        " where employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();
                    objquartersdatalist.tdsquarter1_receiptno = objMySqlDataReader["tdsquarter1_receiptno"].ToString();
                    objquartersdatalist.tdsquarter1_paidcredited = objMySqlDataReader["tdsquarter1_paidcredited"].ToString();
                    objquartersdatalist.tdsquarter1_amount_deducted = objMySqlDataReader["tdsquarter1_amount_deducted"].ToString();
                    objquartersdatalist.tdsquarter1_amount_deposited = objMySqlDataReader["tdsquarter1_amount_deposited"].ToString();
                    objquartersdatalist.tdsquarter2_receiptno = objMySqlDataReader["tdsquarter2_receiptno"].ToString();
                    objquartersdatalist.tdsquarter2_paidcredited = objMySqlDataReader["tdsquarter2_paidcredited"].ToString();
                    objquartersdatalist.tdsquarter2_amount_deducted = objMySqlDataReader["tdsquarter2_amount_deducted"].ToString();
                    objquartersdatalist.tdsquarter2_amount_deposited = objMySqlDataReader["tdsquarter2_amount_deposited"].ToString();
                    objquartersdatalist.tdsquarter3_receiptno = objMySqlDataReader["tdsquarter3_receiptno"].ToString();
                    objquartersdatalist.tdsquarter3_paidcredited = objMySqlDataReader["tdsquarter3_paidcredited"].ToString();
                    objquartersdatalist.tdsquarter3_amount_deducted = objMySqlDataReader["tdsquarter3_amount_deducted"].ToString();
                    objquartersdatalist.tdsquarter3_amount_deposited = objMySqlDataReader["tdsquarter3_amount_deposited"].ToString();
                    objquartersdatalist.tdsquarter4_receiptno = objMySqlDataReader["tdsquarter4_receiptno"].ToString();
                    objquartersdatalist.tdsquarter4_paidcredited = objMySqlDataReader["tdsquarter4_paidcredited"].ToString();
                    objquartersdatalist.tdsquarter4_amount_deducted = objMySqlDataReader["tdsquarter4_amount_deducted"].ToString();
                    objquartersdatalist.tdsquarter4_amount_deposited = objMySqlDataReader["tdsquarter4_amount_deposited"].ToString();
                    objquartersdatalist.totalamount_paidcredited = objMySqlDataReader["totalamount_paidcredited"].ToString();
                    objquartersdatalist.tdsquarter_totalamount_deducted = objMySqlDataReader["tdsquarter_totalamount_deducted"].ToString();
                    objquartersdatalist.tdsquarter_totalamount_deposited = objMySqlDataReader["tdsquarter_totalamount_deposited"].ToString();

                    objMySqlDataReader.Close();
                }
                return objquartersdatalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                ex.ToString();
                return null;
            }
        }
        public MdlPayIncomedata DaGetIncomedata(string employee_gid, string assessment_gid)
        {
            try
            {
                MdlPayIncomedata objincomedatalist = new MdlPayIncomedata();

                msSQL = " select format(a.grosssalary_amount, 2) as grosssalary_amount, format(a.perquisites_amount, 2) as perquisites_amount, " +
                        " format(a.profitinlieu_amount, 2) as profitinlieu_amount, format(a.grosstotal_qualifiying_amount, 2) as grosstotal_qualifiying_amount, " +
                        " b.lessallowence_name1, b.lessallowence_name2, b.lessallowence_name3, format(b.lessallowence_amount1, 2) as lessallowence_amount1, " +
                        " format(b.lessallowence_amount2, 2) as lessallowence_amount2, format(b.lessallowence_amount3, 2) as lessallowence_amount3, " +
                        " format(b.transport_qualifiying_amount, 2) as transport_qualifiying_amount, format(b.balance_qualifiying_amount, 2) as balance_qualifiying_amount, " +
                        " format(b.entertainment_amount, 2) as entertainment_amount, format(b.taxonemployment_amount, 2) as taxonemployment_amount, " +
                        " format(b.aggreegate_qualifiying_amount, 2) as aggreegate_qualifiying_amount, format(b.incomechargableunder_headsal_deductible_amount, 2) as incomechargableunder_headsal_deductible_amount, " +
                        " c.otherincome1_name, c.otherincome2_name, c.otherincome3_name, format(c.otherincomeemployee_totamount1, 2) as otherincomeemployee_totamount1, " +
                        " format(c.otherincomeemployee_totamount2, 2) as otherincomeemployee_totamount2, format(c.otherincomeemployee_totamount3, 2) as otherincomeemployee_totamount3, " +
                        " format(c.otherincomeemployee_qualifiying_amount3, 2) as otherincomeemployee_qualifiying_amount3, format(c.overallgross_deductible_amount, 2) as overallgross_deductible_amount " +
                        " from pay_trn_ttdsgrosssalary a " +
                        " inner join pay_trn_ttdsallowencetotheextent b on a.employee_gid = b.employee_gid " +
                        " inner join pay_trn_ttdsotherincomeemployee c on a.employee_gid = c.employee_gid " +
                        " where a.assessment_gid='" + assessment_gid + "' and a.employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                msSQL = " select gross_salary from pay_trn_temployee2salarygradetemplate where employee_gid = '" + employee_gid + "' ";
                lsgrosssalary = objdbconn.GetExecuteScalar(msSQL);
                
                if(lsgrosssalary == "")
                {
                    lsgrosssalary = "0";
                }

                lsgross_salary = double.Parse(lsgrosssalary);
                lsgross_salary_new = Math.Round((lsgross_salary) * 12, 2);
                objincomedatalist.grosssalary_amount = lsgross_salary_new;

                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();

                    objincomedatalist.grosssalary_amount = lsgross_salary_new;
                    objincomedatalist.perquisites_amount = objMySqlDataReader["perquisites_amount"].ToString();
                    objincomedatalist.profitof_salary = objMySqlDataReader["profitinlieu_amount"].ToString();
                    objincomedatalist.totalamount = objMySqlDataReader["grosstotal_qualifiying_amount"].ToString();
                    objincomedatalist.component1 = objMySqlDataReader["lessallowence_name1"].ToString();
                    objincomedatalist.component2 = objMySqlDataReader["lessallowence_name2"].ToString();
                    objincomedatalist.component3 = objMySqlDataReader["lessallowence_name3"].ToString();
                    objincomedatalist.pfamount1 = objMySqlDataReader["lessallowence_amount1"].ToString();
                    objincomedatalist.pfamount2 = objMySqlDataReader["lessallowence_amount2"].ToString();
                    objincomedatalist.pfamount3 = objMySqlDataReader["lessallowence_amount3"].ToString();
                    objincomedatalist.lessallowancetotal = objMySqlDataReader["transport_qualifiying_amount"].ToString();
                    objincomedatalist.balanceamount = objMySqlDataReader["balance_qualifiying_amount"].ToString();
                    objincomedatalist.entertainment_allowance = objMySqlDataReader["entertainment_amount"].ToString();
                    objincomedatalist.taxon_emp = objMySqlDataReader["taxonemployment_amount"].ToString();
                    objincomedatalist.aggreagateofab = objMySqlDataReader["aggreegate_qualifiying_amount"].ToString();
                    objincomedatalist.incomecharge_headsalaries = objMySqlDataReader["incomechargableunder_headsal_deductible_amount"].ToString();
                    objincomedatalist.employee_income1 = objMySqlDataReader["otherincome1_name"].ToString();
                    objincomedatalist.employee_income2 = objMySqlDataReader["otherincome2_name"].ToString();
                    objincomedatalist.employee_income3 = objMySqlDataReader["otherincome3_name"].ToString();
                    objincomedatalist.employeeincome_rs1 = objMySqlDataReader["otherincomeemployee_totamount1"].ToString();
                    objincomedatalist.employeeincome_rs2 = objMySqlDataReader["otherincomeemployee_totamount2"].ToString();
                    objincomedatalist.employeeincome_rs3 = objMySqlDataReader["otherincomeemployee_totamount3"].ToString();
                    objincomedatalist.employeeincome_total = objMySqlDataReader["otherincomeemployee_qualifiying_amount3"].ToString();
                    objincomedatalist.grosstotal_income = objMySqlDataReader["overallgross_deductible_amount"].ToString();

                    objMySqlDataReader.Close();
                }
                return objincomedatalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }
        public void DaPostIncome(MdlPayIncomedata values, string user_gid)
        {
            try
            {
                msSQL = " delete from pay_trn_ttdsgrosssalary where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttdsallowencetotheextent where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttdsotherincomeemployee where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select gross_salary from pay_trn_temployee2salarygradetemplate where employee_gid = '" + values.employee_gid + "' ";
                lsgrosssalary = objdbconn.GetExecuteScalar(msSQL);

                lsgross_salary = double.Parse(lsgrosssalary);
                lsgross_salary_new = (lsgross_salary) * 12;

                string msGetGid = objcmnfunctions.GetMasterGID("PAGS");

                msSQL = " insert into pay_trn_ttdsgrosssalary ( " +
                        " tdsgrosssalary_gid, " +
                        " assessment_gid, " +
                        " employee_gid, " +
                        " grosssalary_amount, " +
                        " perquisites_amount, " +
                        " profitinlieu_amount, " +
                        " grosstotal_qualifiying_amount, " +
                        " created_by," +
                        " created_date " +
                        " ) values ( " +
                        " '" + msGetGid + "', " +
                        " '" + values.assessment_gid + "', " +
                        " '" + values.employee_gid + "', " +
                        " '" + lsgross_salary_new + "', " +
                        " '" + values.perquisites_amount + "', " +
                        " '" + values.profitof_salary + "', " +
                        " '" + values.totalamount + "', " +
                        " '" + values.employee_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PAAE");

                    msSQL = " insert into pay_trn_ttdsallowencetotheextent ( " +
                            " tdsallowencetotheextent_gid, " +
                            " assessment_gid, " +
                            " employee_gid, " +
                            " lessallowence_name1, " +
                            " lessallowence_name2, " +
                            " lessallowence_name3, " +
                            " lessallowence_amount1, " +
                            " lessallowence_amount2, " +
                            " lessallowence_amount3, " +
                            " transport_qualifiying_amount, " +
                            " balance_qualifiying_amount, " +
                            " entertainment_amount, " +
                            " taxonemployment_amount, " +
                            " aggreegate_qualifiying_amount, " +
                            " incomechargableunder_headsal_deductible_amount, " +
                            " created_by," +
                            " created_date " +
                            " ) values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.assessment_gid + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + values.component1 + "', " +
                            " '" + values.component2 + "', " +
                            " '" + values.component3 + "', " +
                            " '" + values.pfamount1 + "', " +
                            " '" + values.pfamount2 + "', " +
                            " '" + values.pfamount3 + "', " +
                            " '" + values.lessallowancetotal + "', " +
                            " '" + values.balanceamount + "', " +
                            " '" + values.entertainment_allowance + "', " +
                            " '" + values.taxon_emp + "', " +
                            " '" + values.aggreagateofab + "', " +
                            " '" + values.incomecharge_headsalaries + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PAOI");

                    msSQL = " insert into pay_trn_ttdsotherincomeemployee ( " +
                            " tdsotherincomeemployee_gid, " +
                            " assessment_gid, " +
                            " employee_gid, " +
                            " otherincome1_name, " +
                            " otherincome2_name, " +
                            " otherincome3_name, " +
                            " otherincomeemployee_totamount1, " +
                            " otherincomeemployee_totamount2, " +
                            " otherincomeemployee_totamount3, " +
                            " otherincomeemployee_qualifiying_amount3, " +
                            " overallgross_deductible_amount, " +
                            " created_by," +
                            " created_date " +
                            " ) values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.assessment_gid + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + values.employee_income1 + "', " +
                            " '" + values.employee_income2 + "', " +
                            " '" + values.employee_income3 + "', " +
                            " '" + values.employeeincome_rs1 + "', " +
                            " '" + values.employeeincome_rs2 + "', " +
                            " '" + values.employeeincome_rs3 + "', " +
                            " '" + values.employeeincome_total + "', " +
                            " '" + values.grosstotal_income + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Income Details are Inserted Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while inserting income details !!";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostTDS(MdlPayTDSdata values, string user_gid)
        {
            try
            {
                msSQL = " delete from pay_trn_ttdsdeductions where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttdsothersections where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttdsaggregateofdeductable where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttds where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                string msGetGid = objcmnfunctions.GetMasterGID("PADE");

                msSQL = " insert into pay_trn_ttdsdeductions ( " +
                        " tdsdeductions_gid, " +
                        " assessment_gid, " +
                        " employee_gid, " +
                        " section80c_name1, " +
                        " section80c_grossamount1, " +
                        " section80c_name2, " +
                        " section80c_grossamount2, " +
                        " section80c_name3, " +
                        " section80c_grossamount3, " +
                        " section80c_name4, " +
                        " section80c_grossamount4, " +
                        " section80c_name5, " +
                        " section80c_grossamount5, " +
                        " section80c_name6, " +
                        " section80c_grossamount6, " +
                        " section80c_name7, " +
                        " section80c_grossamount7, " +
                        " section80c_amount7, " +
                        " section80c_deductible_amount7, " +
                        " section80ccc_grossamount, " +
                        " section80ccc_deductible_amount, " +
                        " section80ccd_grossamount, " +
                        " section80ccd_deductible_amount, " +
                        " aggregate_gross, " +
                        " aggregate_deduct, " +
                        " section80ccd1b_grossamount, " +
                        " section80ccd1b_deductamount, " +
                        " created_by, " +
                        " created_date " +
                        " ) values ( " +
                        " '" + msGetGid + "', " +
                        " '" + values.assessment_gid + "', " +
                        " '" + values.employee_gid + "', " +
                        " '" + values.section80C_i_name + "', " +
                        " '" + values.section80C_i_value + "', " +
                        " '" + values.section80C_ii_name + "', " +
                        " '" + values.section80C_ii_value + "', " +
                        " '" + values.section80C_iii_name + "', " +
                        " '" + values.section80C_iii_value + "', " +
                        " '" + values.section80C_iv_name + "', " +
                        " '" + values.section80C_iv_value + "', " +
                        " '" + values.section80C_v_name + "', " +
                        " '" + values.section80C_v_value + "', " +
                        " '" + values.section80C_vi_name + "', " +
                        " '" + values.section80C_vi_value + "', " +
                        " '" + values.section80C_vii_name + "', " +
                        " '" + values.section80C_vii_value + "', " +
                        " '" + values.section80C_vii_gross_total + "', " +
                        " '" + values.section80C_vii_deductable_total + "', " +
                        " '" + values.section80CCC_gross_total + "', " +
                        " '" + values.section80CCC_deductable_total + "', " +
                        " '" + values.section80CCD_gross_total + "', " +
                        " '" + values.section80CCD_deductable_total + "', " +
                        " '" + values.aggregate3sec_gross_total + "', " +
                        " '" + values.aggregate3sec_deductable_total + "', " +
                        " '" + values.section80CCD1B_gross_total + "', " +
                        " '" + values.section80CCD1B_deductable_total + "', " +
                        " '" + values.employee_gid + "', " +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PAOS");

                    msSQL = " insert into pay_trn_ttdsothersections ( " +
                            " tdsothersections_gid, " +
                            " assessment_gid, " +
                            " employee_gid, " +
                            " section1_name, " +
                            " section1_grossamount, " +
                            " section1_qualifiying_amount, " +
                            " section1_deductible_amount, " +
                            " section2_name, " +
                            " section2_grossamount, " +
                            " section2_qualifiying_amount, " +
                            " section2_deductible_amount, " +
                            " section3_name, " +
                            " section3_grossamount, " +
                            " section3_qualifiying_amount, " +
                            " section3_deductible_amount, " +
                            " section4_name, " +
                            " section4_grossamount, " +
                            " section4_qualifiying_amount, " +
                            " section4_deductible_amount, " +
                            " section5_name, " +
                            " section5_grossamount, " +
                            " section5_qualifiying_amount, " +
                            " section5_deductible_amount, " +
                            " created_by, " +
                            " created_date " +
                            " ) values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.assessment_gid + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + values.other_section1_value + "', " +
                            " '" + values.other_section1_gross_amount + "', " +
                            " '" + values.other_section1_qualifying_amount + "', " +
                            " '" + values.other_section1_deductable + "', " +
                            " '" + values.other_section2_value + "', " +
                            " '" + values.other_section2_gross_amount + "', " +
                            " '" + values.other_section2_qualifying_amount + "', " +
                            " '" + values.other_section2_deductable + "', " +
                            " '" + values.other_section3_value + "', " +
                            " '" + values.other_section3_gross_amount + "', " +
                            " '" + values.other_section3_qualifying_amount + "', " +
                            " '" + values.other_section3_deductable + "', " +
                            " '" + values.other_section4_value + "', " +
                            " '" + values.other_section4_gross_amount + "', " +
                            " '" + values.other_section4_qualifying_amount + "', " +
                            " '" + values.other_section4_deductable + "', " +
                            " '" + values.other_section5_value + "', " +
                            " '" + values.other_section5_gross_amount + "', " +
                            " '" + values.other_section5_qualifying_amount + "', " +
                            " '" + values.other_section5_deductable + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PAAD");

                    msSQL = " insert into pay_trn_ttdsaggregateofdeductable ( " +
                            " ttdsaggregateofdeductable_gid, " +
                            " aggregatedeductable_totalamount, " +
                            " employee_gid, " +
                            " assessment_gid, " +
                            " created_by, " +
                            " created_date " +
                            " ) Values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.aggregate4Asec_deductible_total + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + values.assessment_gid + "'," +
                            " '" + values.employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PTDS");

                    msSQL = " insert into pay_trn_ttds ( " +
                            " tds_gid, " +
                            " assessment_gid, " +
                            " employee_gid, " +
                            " total_income, " +
                            " taxpercentold, " +
                            " taxpercentnew, " +
                            " tax_total_income, " +
                            " educationcess_amount, " +
                            " tax_payable12plus13, " +
                            " less_relief89, " +
                            " tax_payable14minus15, " +
                            " less_tds, " +
                            " balance_tax, " +
                            " created_by, " +
                            " created_date " +
                            " ) values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.assessment_gid + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + values.total_taxable_income + "', " +
                            " '" + values.taxpercentold + "', " +
                            " '" + values.taxpercentnew + "', " +
                            " '" + values.tax_on_total_income + "', " +
                            " '" + values.educationcess + "', " +
                            " '" + values.tax_payable + "', " +
                            " '" + values.less_relief + "', " +
                            " '" + values.net_tax_payable + "', " +
                            " '" + values.less_tax_deducted_at_source + "', " +
                            " '" + values.balance_tax_pay_refund + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "TDS Details are Inserted Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while inserting TDS details !!";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public MdlPayTDSdata DaGetTDSdata(string employee_gid, string assessment_gid)
        {
            try
            {
                MdlPayTDSdata objtdsdatalist = new MdlPayTDSdata();

                msSQL = " select a.tdsdeductions_gid, a.assessment_gid, a.employee_gid, a.section80c_name1, format(a.section80c_grossamount1, 2) as section80c_grossamount1, " +
                        " a.section80c_name2, format(a.section80c_grossamount2, 2) as section80c_grossamount2, a.section80c_name3, format(a.section80c_grossamount3, 2) as section80c_grossamount3, " +
                        " a.section80c_name4, format(a.section80c_grossamount4, 2) as section80c_grossamount4, a.section80c_name5, format(a.section80c_grossamount5, 2) as section80c_grossamount5, " +
                        " a.section80c_name6, format(a.section80c_grossamount6, 2) as section80c_grossamount6, a.section80c_name7, format(a.section80c_grossamount7, 2) as section80c_grossamount7, " +
                        " format(a.section80c_amount7, 2) as section80c_amount7, format(a.section80c_deductible_amount7, 2) as section80c_deductible_amount7, format(a.section80ccc_grossamount, 2) as section80ccc_grossamount, " +
                        " format(a.section80ccc_deductible_amount, 2) as section80ccc_deductible_amount, format(a.section80ccd_grossamount, 2) as section80ccd_grossamount, format(a.section80ccd_deductible_amount, 2) as section80ccd_deductible_amount, " +
                        " format(a.aggregate_gross, 2) as aggregate_gross, format(a.aggregate_deduct, 2) as aggregate_deduct, format(a.section80ccd1b_grossamount, 2) as section80ccd1b_grossamount, format(a.section80ccd1b_deductamount, 2) as section80ccd1b_deductamount," +
                        " b.section1_name, format(b.section1_grossamount, 2) as section1_grossamount, format(b.section1_qualifiying_amount, 2) as section1_qualifiying_amount, format(b.section1_deductible_amount, 2) as section1_deductible_amount, " +
                        " b.section2_name, format(b.section2_grossamount, 2) as section2_grossamount, format(b.section2_qualifiying_amount, 2) as section2_qualifiying_amount, format(b.section2_deductible_amount, 2) as section2_deductible_amount, " +
                        " b.section3_name, format(b.section3_grossamount, 2) as section3_grossamount, format(b.section3_qualifiying_amount, 2) as section3_qualifiying_amount, format(b.section3_deductible_amount, 2) as section3_deductible_amount, " +
                        " b.section4_name, format(b.section4_grossamount, 2) as section4_grossamount, format(b.section4_qualifiying_amount, 2) as section4_qualifiying_amount, format(b.section4_deductible_amount, 2) as section4_deductible_amount, " +
                        " b.section5_name, format(b.section5_grossamount, 2) as section5_grossamount, format(b.section5_qualifiying_amount, 2) as section5_qualifiying_amount, format(b.section5_deductible_amount, 2) as section5_deductible_amount, " +
                        " format(c.aggregatedeductable_totalamount, 2) as aggregatedeductable_totalamount, format(d.total_income, 2) as total_income, d.taxpercentold, d.taxpercentnew, format(d.tax_total_income, 2) as tax_total_income, " +
                        " format(d.educationcess_amount, 2) as educationcess_amount, format(d.tax_payable12plus13, 2) as tax_payable12plus13, format(d.less_relief89, 2) as less_relief89, " +
                        " format(d.tax_payable14minus15, 2) as tax_payable14minus15, format(d.less_tds, 2) as less_tds, format(d.balance_tax, 2) as balance_tax " +
                        " from pay_trn_ttdsdeductions a " +
                        " left join pay_trn_ttdsothersections b on a.employee_gid = b.employee_gid " +
                        " left join pay_trn_ttdsaggregateofdeductable c on b.employee_gid = c.employee_gid " +
                        " left join pay_trn_ttds d on c.employee_gid = d.employee_gid " +
                        " where a.assessment_gid='" + assessment_gid + "' and a.employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();
                    objtdsdatalist.section80C_i_name = objMySqlDataReader["section80c_name1"].ToString();
                    objtdsdatalist.section80C_i_value = objMySqlDataReader["section80c_grossamount1"].ToString();
                    objtdsdatalist.section80C_ii_name = objMySqlDataReader["section80c_name2"].ToString();
                    objtdsdatalist.section80C_ii_value = objMySqlDataReader["section80c_grossamount2"].ToString();
                    objtdsdatalist.section80C_iii_name = objMySqlDataReader["section80c_name3"].ToString();
                    objtdsdatalist.section80C_iii_value = objMySqlDataReader["section80c_grossamount3"].ToString();
                    objtdsdatalist.section80C_iv_name = objMySqlDataReader["section80c_name4"].ToString();
                    objtdsdatalist.section80C_iv_value = objMySqlDataReader["section80c_grossamount4"].ToString();
                    objtdsdatalist.section80C_v_name = objMySqlDataReader["section80c_name5"].ToString();
                    objtdsdatalist.section80C_v_value = objMySqlDataReader["section80c_grossamount5"].ToString();
                    objtdsdatalist.section80C_vi_name = objMySqlDataReader["section80c_name6"].ToString();
                    objtdsdatalist.section80C_vi_value = objMySqlDataReader["section80c_grossamount6"].ToString();
                    objtdsdatalist.section80C_vii_name = objMySqlDataReader["section80c_name7"].ToString();
                    objtdsdatalist.section80C_vii_value = objMySqlDataReader["section80c_grossamount7"].ToString();
                    objtdsdatalist.section80C_vii_gross_total = objMySqlDataReader["section80c_amount7"].ToString();
                    objtdsdatalist.section80C_vii_deductable_total = objMySqlDataReader["section80c_deductible_amount7"].ToString();
                    objtdsdatalist.section80CCC_gross_total = objMySqlDataReader["section80ccc_grossamount"].ToString();
                    objtdsdatalist.section80CCC_deductable_total = objMySqlDataReader["section80ccc_deductible_amount"].ToString();
                    objtdsdatalist.section80CCD_gross_total = objMySqlDataReader["section80ccd_grossamount"].ToString();
                    objtdsdatalist.section80CCD_deductable_total = objMySqlDataReader["section80ccd_deductible_amount"].ToString();
                    objtdsdatalist.aggregate3sec_gross_total = objMySqlDataReader["aggregate_gross"].ToString();
                    objtdsdatalist.aggregate3sec_deductable_total = objMySqlDataReader["aggregate_deduct"].ToString();
                    objtdsdatalist.section80CCD1B_gross_total = objMySqlDataReader["section80ccd1b_grossamount"].ToString();
                    objtdsdatalist.section80CCD1B_deductable_total = objMySqlDataReader["section80ccd1b_deductamount"].ToString();
                    objtdsdatalist.other_section1_value = objMySqlDataReader["section1_name"].ToString();
                    objtdsdatalist.other_section1_gross_amount = objMySqlDataReader["section1_grossamount"].ToString();
                    objtdsdatalist.other_section1_qualifying_amount = objMySqlDataReader["section1_qualifiying_amount"].ToString();
                    objtdsdatalist.other_section1_deductable = objMySqlDataReader["section1_deductible_amount"].ToString();
                    objtdsdatalist.other_section2_value = objMySqlDataReader["section2_name"].ToString();
                    objtdsdatalist.other_section2_gross_amount = objMySqlDataReader["section2_grossamount"].ToString();
                    objtdsdatalist.other_section2_qualifying_amount = objMySqlDataReader["section2_qualifiying_amount"].ToString();
                    objtdsdatalist.other_section2_deductable = objMySqlDataReader["section2_deductible_amount"].ToString();
                    objtdsdatalist.other_section3_value = objMySqlDataReader["section3_name"].ToString();
                    objtdsdatalist.other_section3_gross_amount = objMySqlDataReader["section3_grossamount"].ToString();
                    objtdsdatalist.other_section3_qualifying_amount = objMySqlDataReader["section3_qualifiying_amount"].ToString();
                    objtdsdatalist.other_section3_deductable = objMySqlDataReader["section3_deductible_amount"].ToString();
                    objtdsdatalist.other_section4_value = objMySqlDataReader["section4_name"].ToString();
                    objtdsdatalist.other_section4_gross_amount = objMySqlDataReader["section4_grossamount"].ToString();
                    objtdsdatalist.other_section4_qualifying_amount = objMySqlDataReader["section4_qualifiying_amount"].ToString();
                    objtdsdatalist.other_section4_deductable = objMySqlDataReader["section4_deductible_amount"].ToString();
                    objtdsdatalist.other_section5_value = objMySqlDataReader["section5_name"].ToString();
                    objtdsdatalist.other_section5_gross_amount = objMySqlDataReader["section5_grossamount"].ToString();
                    objtdsdatalist.other_section5_qualifying_amount = objMySqlDataReader["section5_qualifiying_amount"].ToString();
                    objtdsdatalist.other_section5_deductable = objMySqlDataReader["section5_deductible_amount"].ToString();
                    objtdsdatalist.aggregate4Asec_deductible_total = objMySqlDataReader["aggregatedeductable_totalamount"].ToString();
                    objtdsdatalist.total_taxable_income = objMySqlDataReader["total_income"].ToString();
                    objtdsdatalist.taxpercentold = objMySqlDataReader["taxpercentold"].ToString();
                    objtdsdatalist.taxpercentnew = objMySqlDataReader["taxpercentnew"].ToString();
                    objtdsdatalist.tax_on_total_income = objMySqlDataReader["tax_total_income"].ToString();
                    objtdsdatalist.educationcess = objMySqlDataReader["educationcess_amount"].ToString();
                    objtdsdatalist.tax_payable = objMySqlDataReader["tax_payable12plus13"].ToString();
                    objtdsdatalist.less_relief = objMySqlDataReader["less_relief89"].ToString();
                    objtdsdatalist.net_tax_payable = objMySqlDataReader["tax_payable14minus15"].ToString();
                    objtdsdatalist.less_tax_deducted_at_source = objMySqlDataReader["less_tds"].ToString();
                    objtdsdatalist.balance_tax_pay_refund = objMySqlDataReader["balance_tax"].ToString();

                    objMySqlDataReader.Close();
                }
                return objtdsdatalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                ex.ToString();
                return null;
            }
        }

        public Mdlanx1data DaGetanx1data(string employee_gid, string assessment_gid)
        {
            try
            {
                Mdlanx1data objanx1datalist = new Mdlanx1data();

                msSQL = "select * from pay_trn_tannexure1 where assessment_gid='" + assessment_gid + "' " + " and employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();
                    objanx1datalist.tax_deposit1 = objMySqlDataReader["tax_deposit1"].ToString();
                    objanx1datalist.receipt_no1 = objMySqlDataReader["receipt_no1"].ToString();
                    objanx1datalist.ddo_no1 = objMySqlDataReader["ddo_no1"].ToString();
                    objanx1datalist.date_transfer1 = objMySqlDataReader["date_transfer1"].ToString();
                    objanx1datalist.status1 = objMySqlDataReader["status1"].ToString();
                    objanx1datalist.tax_deposit2 = objMySqlDataReader["tax_deposit2"].ToString();
                    objanx1datalist.receipt_no2 = objMySqlDataReader["receipt_no2"].ToString();
                    objanx1datalist.ddo_no2 = objMySqlDataReader["ddo_no2"].ToString();
                    objanx1datalist.date_transfer2 = objMySqlDataReader["date_transfer2"].ToString();
                    objanx1datalist.status2 = objMySqlDataReader["status2"].ToString();
                    objanx1datalist.tax_deposit3 = objMySqlDataReader["tax_deposit3"].ToString();
                    objanx1datalist.receipt_no3 = objMySqlDataReader["receipt_no3"].ToString();
                    objanx1datalist.ddo_no3 = objMySqlDataReader["ddo_no3"].ToString();
                    objanx1datalist.date_transfer3 = objMySqlDataReader["date_transfer3"].ToString();
                    objanx1datalist.status3 = objMySqlDataReader["status3"].ToString();
                    objanx1datalist.tax_deposit4 = objMySqlDataReader["tax_deposit4"].ToString();
                    objanx1datalist.receipt_no4 = objMySqlDataReader["receipt_no4"].ToString();
                    objanx1datalist.ddo_no4 = objMySqlDataReader["ddo_no4"].ToString();
                    objanx1datalist.date_transfer4 = objMySqlDataReader["date_transfer4"].ToString();
                    objanx1datalist.status4 = objMySqlDataReader["status4"].ToString();
                    objanx1datalist.tax_deposit5 = objMySqlDataReader["tax_deposit5"].ToString();
                    objanx1datalist.receipt_no5 = objMySqlDataReader["receipt_no5"].ToString();
                    objanx1datalist.ddo_no5 = objMySqlDataReader["ddo_no5"].ToString();
                    objanx1datalist.date_transfer5 = objMySqlDataReader["date_transfer5"].ToString();
                    objanx1datalist.status5 = objMySqlDataReader["status5"].ToString();
                    objanx1datalist.tax_deposit6 = objMySqlDataReader["tax_deposit6"].ToString();
                    objanx1datalist.receipt_no6 = objMySqlDataReader["receipt_no6"].ToString();
                    objanx1datalist.ddo_no6 = objMySqlDataReader["ddo_no6"].ToString();
                    objanx1datalist.date_transfer6 = objMySqlDataReader["date_transfer6"].ToString();
                    objanx1datalist.status6 = objMySqlDataReader["status6"].ToString();
                    objanx1datalist.tax_deposit7 = objMySqlDataReader["tax_deposit7"].ToString();
                    objanx1datalist.receipt_no7 = objMySqlDataReader["receipt_no7"].ToString();
                    objanx1datalist.ddo_no7 = objMySqlDataReader["ddo_no7"].ToString();
                    objanx1datalist.date_transfer7 = objMySqlDataReader["date_transfer7"].ToString();
                    objanx1datalist.status7 = objMySqlDataReader["status7"].ToString();
                    objanx1datalist.tax_deposit8 = objMySqlDataReader["tax_deposit8"].ToString();
                    objanx1datalist.receipt_no8 = objMySqlDataReader["receipt_no8"].ToString();
                    objanx1datalist.ddo_no8 = objMySqlDataReader["ddo_no8"].ToString();
                    objanx1datalist.date_transfer8 = objMySqlDataReader["date_transfer8"].ToString();
                    objanx1datalist.status8 = objMySqlDataReader["status8"].ToString();
                    objanx1datalist.tax_deposit9 = objMySqlDataReader["tax_deposit9"].ToString();
                    objanx1datalist.receipt_no9 = objMySqlDataReader["receipt_no9"].ToString();
                    objanx1datalist.ddo_no9 = objMySqlDataReader["ddo_no9"].ToString();
                    objanx1datalist.date_transfer9 = objMySqlDataReader["date_transfer9"].ToString();
                    objanx1datalist.status9 = objMySqlDataReader["status9"].ToString();
                    objanx1datalist.tax_deposit10 = objMySqlDataReader["tax_deposit10"].ToString();
                    objanx1datalist.receipt_no10 = objMySqlDataReader["receipt_no10"].ToString();
                    objanx1datalist.ddo_no10 = objMySqlDataReader["ddo_no10"].ToString();
                    objanx1datalist.date_transfer10 = objMySqlDataReader["date_transfer10"].ToString();
                    objanx1datalist.status10 = objMySqlDataReader["status10"].ToString();
                    objanx1datalist.tax_deposit11 = objMySqlDataReader["tax_deposit11"].ToString();
                    objanx1datalist.receipt_no11 = objMySqlDataReader["receipt_no11"].ToString();
                    objanx1datalist.ddo_no11 = objMySqlDataReader["ddo_no11"].ToString();
                    objanx1datalist.date_transfer11 = objMySqlDataReader["date_transfer11"].ToString();
                    objanx1datalist.status11 = objMySqlDataReader["status11"].ToString();
                    objanx1datalist.tax_deposit12 = objMySqlDataReader["tax_deposit12"].ToString();
                    objanx1datalist.receipt_no12 = objMySqlDataReader["receipt_no12"].ToString();
                    objanx1datalist.ddo_no12 = objMySqlDataReader["ddo_no12"].ToString();
                    objanx1datalist.date_transfer12 = objMySqlDataReader["date_transfer12"].ToString();
                    objanx1datalist.status12 = objMySqlDataReader["status12"].ToString();
                    objanx1datalist.total_taxdeposit = objMySqlDataReader["total_taxdeposit"].ToString();

                    objMySqlDataReader.Close();
                }
                return objanx1datalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                ex.ToString();
                return null;
            }
        }

        public void DaPostanx1(MdlAnx1data values, string user_gid)
        {
            try
            {
                //string uiDateStr = values.invoicedate;
                //DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //string mysqlinvoiceDate = uiDate.ToString("yyyy-MM-dd");

                string uiDate1Str = values.date1_anx1;
                DateTime uiDate1 = DateTime.ParseExact(uiDate1Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date1_anx1 = uiDate1.ToString("yyyy-MM-dd");

                string uiDate2Str = values.date2_anx1;
                DateTime uiDate2 = DateTime.ParseExact(uiDate2Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date2_anx1 = uiDate2.ToString("yyyy-MM-dd");

                string uiDate3Str = values.date3_anx1;
                DateTime uiDate3 = DateTime.ParseExact(uiDate3Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date3_anx1 = uiDate3.ToString("yyyy-MM-dd");

                string uiDate4Str = values.date4_anx1;
                DateTime uiDate4 = DateTime.ParseExact(uiDate4Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date4_anx1 = uiDate4.ToString("yyyy-MM-dd");

                string uiDate5Str = values.date5_anx1;
                DateTime uiDate5 = DateTime.ParseExact(uiDate5Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date5_anx1 = uiDate5.ToString("yyyy-MM-dd");

                string uiDate6Str = values.date6_anx1;
                DateTime uiDate6 = DateTime.ParseExact(uiDate6Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date6_anx1 = uiDate6.ToString("yyyy-MM-dd");

                string uiDate7Str = values.date7_anx1;
                DateTime uiDate7 = DateTime.ParseExact(uiDate7Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date7_anx1 = uiDate7.ToString("yyyy-MM-dd");

                string uiDate8Str = values.date8_anx1;
                DateTime uiDate8 = DateTime.ParseExact(uiDate8Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date8_anx1 = uiDate8.ToString("yyyy-MM-dd");

                string uiDate9Str = values.date9_anx1;
                DateTime uiDate9 = DateTime.ParseExact(uiDate9Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date9_anx1 = uiDate9.ToString("yyyy-MM-dd");

                string uiDate10Str = values.date10_anx1;
                DateTime uiDate10 = DateTime.ParseExact(uiDate10Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date10_anx1 = uiDate10.ToString("yyyy-MM-dd");

                string uiDate11Str = values.date11_anx1;
                DateTime uiDate11 = DateTime.ParseExact(uiDate11Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date11_anx1 = uiDate11.ToString("yyyy-MM-dd");

                string uiDate12Str = values.date12_anx1;
                DateTime uiDate12 = DateTime.ParseExact(uiDate6Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date12_anx1 = uiDate12.ToString("yyyy-MM-dd");

                msSQL = " delete from pay_trn_tannexure1 where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                string msGetGid = objcmnfunctions.GetMasterGID("ANX1");

                msSQL = " insert into pay_trn_tannexure1 ( " +
                        " annexure1_gid, " +
                        " assessment_gid, " +
                        " employee_gid, " +
                        " tax_deposit1, " +
                        " tax_deposit2, " +
                        " tax_deposit3, " +
                        " tax_deposit4, " +
                        " tax_deposit5, " +
                        " tax_deposit6, " +
                        " tax_deposit7, " +
                        " tax_deposit8, " +
                        " tax_deposit9, " +
                        " tax_deposit10, " +
                        " tax_deposit11, " +
                        " tax_deposit12, " +
                        " receipt_no1, " +
                        " receipt_no2, " +
                        " receipt_no3, " +
                        " receipt_no4, " +
                        " receipt_no5, " +
                        " receipt_no6, " +
                        " receipt_no7, " +
                        " receipt_no8, " +
                        " receipt_no9, " +
                        " receipt_no10, " +
                        " receipt_no11, " +
                        " receipt_no12, " +
                        " ddo_no1, " +
                        " ddo_no2, " +
                        " ddo_no3, " +
                        " ddo_no4, " +
                        " ddo_no5, " +
                        " ddo_no6, " +
                        " ddo_no7, " +
                        " ddo_no8, " +
                        " ddo_no9, " +
                        " ddo_no10, " +
                        " ddo_no11, " +
                        " ddo_no12, " +
                        " date_transfer1, " +
                        " date_transfer2, " +
                        " date_transfer3, " +
                        " date_transfer4, " +
                        " date_transfer5, " +
                        " date_transfer6, " +
                        " date_transfer7, " +
                        " date_transfer8, " +
                        " date_transter9, " +
                        " date_transfer10, " +
                        " date_transfer11, " +
                        " date_transfer12, " +
                        " status1, " +
                        " status2, " +
                        " status3, " +
                        " status4, " +
                        " status5, " +
                        " status6, " +
                        " status7, " +
                        " status8, " +
                        " status9, " +
                        " status10, " +
                        " status11, " +
                        " status12, " +
                        " total_taxdeposit, " +
                        " created_by," +
                        " created_date " +
                        " ) values ( " +
                        " '" + msGetGid + "', " +
                        " '" + values.assessment_gid + "', " +
                        " '" + values.employee_gid + "', " +
                        " '" + values.totaltax_dep1_anx1 + "', " +
                        " '" + values.totaltax_dep2_anx1 + "', " +
                        " '" + values.totaltax_dep3_anx1 + "', " +
                        " '" + values.totaltax_dep4_anx1 + "', " +
                        " '" + values.totaltax_dep5_anx1 + "', " +
                        " '" + values.totaltax_dep6_anx1 + "', " +
                        " '" + values.totaltax_dep7_anx1 + "', " +
                        " '" + values.totaltax_dep8_anx1 + "', " +
                        " '" + values.totaltax_dep9_anx1 + "', " +
                        " '" + values.totaltax_dep10_anx1 + "', " +
                        " '" + values.totaltax_dep11_anx1 + "', " +
                        " '" + values.totaltax_dep12_anx1 + "', " +
                        " '" + values.receiptnum1_anx1 + "', " +
                        " '" + values.receiptnum2_anx1 + "', " +
                        " '" + values.receiptnum3_anx1 + "', " +
                        " '" + values.receiptnum4_anx1 + "', " +
                        " '" + values.receiptnum5_anx1 + "', " +
                        " '" + values.receiptnum6_anx1 + "', " +
                        " '" + values.receiptnum7_anx1 + "', " +
                        " '" + values.receiptnum8_anx1 + "', " +
                        " '" + values.receiptnum9_anx1 + "', " +
                        " '" + values.receiptnum10_anx1 + "', " +
                        " '" + values.receiptnum11_anx1 + "', " +
                        " '" + values.receiptnum12_anx1 + "', " +
                        " '" + values.ddonum1_anx1 + "', " +
                        " '" + values.ddonum2_anx1 + "', " +
                        " '" + values.ddonum3_anx1 + "', " +
                        " '" + values.ddonum4_anx1 + "', " +
                        " '" + values.ddonum5_anx1 + "', " +
                        " '" + values.ddonum6_anx1 + "', " +
                        " '" + values.ddonum7_anx1 + "', " +
                        " '" + values.ddonum8_anx1 + "', " +
                        " '" + values.ddonum9_anx1 + "', " +
                        " '" + values.ddonum10_anx1 + "', " +
                        " '" + values.ddonum11_anx1 + "', " +
                        " '" + values.ddonum12_anx1 + "',";
                        if (date1_anx1 != "")
                          {
                               msSQL += "'" + date1_anx1 + "',";
                          }
                        else
                           {
                               msSQL += "null,";
                           }

                        if (date2_anx1 != "")
                          {
                                msSQL += "'" + date2_anx1 + "',";
                          }
                        else
                          {
                            msSQL += "null,";
                          }

                        if (date3_anx1 != "")
                            {
                             msSQL += "'" + date3_anx1 + "',";
                            }
                        else
                            {
                             msSQL += "null,";
                            }

                        if (date4_anx1 != "")
                            {
                              msSQL += "'" + date4_anx1 + "',";
                            }
                        else
                            {
                              msSQL += "null,";
                            }

                        if (date5_anx1 != "")
                            {
                              msSQL += "'" + date5_anx1 + "',";
                            }
                        else
                            {
                              msSQL += "null,";
                            }

                        if (date6_anx1 != "")
                            {
                              msSQL += "'" + date6_anx1 + "',";
                            }
                        else
                            {
                              msSQL += "null,";
                            }

                        if (date7_anx1 != "")
                            {
                              msSQL += "'" + date7_anx1 + "',";
                            }
                        else
                            {
                              msSQL += "null,";
                            }

                        if (date8_anx1 != "")
                            {
                             msSQL += "'" + date8_anx1 + "',";
                            }
                        else
                            {
                              msSQL += "null,";
                            }

                        if (date9_anx1 != "")
                            {
                              msSQL += "'" + date9_anx1 + "',";
                            }
                        else
                            {
                              msSQL += "null,";
                            }

                        if (date10_anx1 != "")
                            {
                             msSQL += "'" + date9_anx1 + "',";
                            }
                        else
                            {
                              msSQL += "null,";
                            }

                        if (date11_anx1 != "")
                            {
                              msSQL += "'" + date10_anx1 + "',";
                            }
                        else
                            {
                              msSQL += "null,";
                            }

                        if (date12_anx1 != "")
                            {
                             msSQL += "'" + date11_anx1 + "',";
                            }
                        else
                            {
                             msSQL += "null,";
                            }

    

                  msSQL +=  " '" + values.status1_anx1 + "', " +
                            " '" + values.status2_anx1 + "', " +
                            " '" + values.status3_anx1 + "', " +
                            " '" + values.status4_anx1 + "', " +
                            " '" + values.status5_anx1 + "', " +
                            " '" + values.status6_anx1 + "', " +
                            " '" + values.status7_anx1 + "', " +
                            " '" + values.status8_anx1 + "', " +
                            " '" + values.status9_anx1 + "', " +
                            " '" + values.status10_anx1 + "', " +
                            " '" + values.status11_anx1 + "', " +
                            " '" + values.status12_anx1 + "', " +
                            " '" + values.total_tax_deposited_anx1 + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Annexure1 Details are Inserted Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while inserting Annexure1 details !!";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public Mdlanx2data DaGetanx2data(string employee_gid, string assessment_gid)
        {
            try
            {
                Mdlanx2data objanx2datalist = new Mdlanx2data();

                msSQL = "select * from pay_trn_tannexure where assessment_gid='" + assessment_gid + "' " + " and employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();
                    objanx2datalist.bsrcode1 = objMySqlDataReader["bsrcode1"].ToString();
                    objanx2datalist.date_tax1 = objMySqlDataReader["date_tax1"].ToString();
                    objanx2datalist.challanno_tax1 = objMySqlDataReader["challanno_tax1"].ToString();
                    objanx2datalist.status1 = objMySqlDataReader["status1"].ToString();
                    objanx2datalist.totaltax_deposited2 = objMySqlDataReader["totaltax_deposited2"].ToString();
                    objanx2datalist.bsrcode2 = objMySqlDataReader["bsrcode2"].ToString();
                    objanx2datalist.date_tax2 = objMySqlDataReader["date_tax2"].ToString();
                    objanx2datalist.challanno_tax2 = objMySqlDataReader["challanno_tax2"].ToString();
                    objanx2datalist.status2 = objMySqlDataReader["status2"].ToString();
                    objanx2datalist.totaltax_deposited3 = objMySqlDataReader["totaltax_deposited3"].ToString();
                    objanx2datalist.bsrcode3 = objMySqlDataReader["bsrcode3"].ToString();
                    objanx2datalist.date_tax3 = objMySqlDataReader["date_tax5"].ToString();
                    objanx2datalist.challanno_tax3 = objMySqlDataReader["challanno_tax3"].ToString();
                    objanx2datalist.status3 = objMySqlDataReader["status3"].ToString();
                    objanx2datalist.totaltax_deposited4 = objMySqlDataReader["totaltax_deposited4"].ToString();
                    objanx2datalist.bsrcode4 = objMySqlDataReader["bsrcode4"].ToString();
                    objanx2datalist.date_tax4 = objMySqlDataReader["date_tax4"].ToString();
                    objanx2datalist.challanno_tax4 = objMySqlDataReader["challanno_tax4"].ToString();
                    objanx2datalist.status4 = objMySqlDataReader["status4"].ToString();
                    objanx2datalist.totaltax_deposited5 = objMySqlDataReader["totaltax_deposited5"].ToString();
                    objanx2datalist.bsrcode5 = objMySqlDataReader["bsrcode5"].ToString();
                    objanx2datalist.date_tax5 = objMySqlDataReader["date_tax5"].ToString();
                    objanx2datalist.challanno_tax5 = objMySqlDataReader["challanno_tax5"].ToString();
                    objanx2datalist.status5 = objMySqlDataReader["status5"].ToString();
                    objanx2datalist.totaltax_deposited6 = objMySqlDataReader["totaltax_deposited6"].ToString();
                    objanx2datalist.bsrcode6 = objMySqlDataReader["bsrcode6"].ToString();
                    objanx2datalist.date_tax6 = objMySqlDataReader["date_tax6"].ToString();
                    objanx2datalist.challanno_tax6 = objMySqlDataReader["challanno_tax6"].ToString();
                    objanx2datalist.status6 = objMySqlDataReader["status6"].ToString();
                    objanx2datalist.totaltax_deposited7 = objMySqlDataReader["totaltax_deposited7"].ToString();
                    objanx2datalist.bsrcode7 = objMySqlDataReader["bsrcode7"].ToString();
                    objanx2datalist.date_tax7 = objMySqlDataReader["date_tax7"].ToString();
                    objanx2datalist.challanno_tax7 = objMySqlDataReader["challanno_tax7"].ToString();
                    objanx2datalist.status7 = objMySqlDataReader["status7"].ToString();
                    objanx2datalist.totaltax_deposited8 = objMySqlDataReader["totaltax_deposited8"].ToString();
                    objanx2datalist.bsrcode8 = objMySqlDataReader["bsrcode8"].ToString();
                    objanx2datalist.date_tax8 = objMySqlDataReader["date_tax8"].ToString();
                    objanx2datalist.challanno_tax8 = objMySqlDataReader["challanno_tax8"].ToString();
                    objanx2datalist.status8 = objMySqlDataReader["status8"].ToString();
                    objanx2datalist.totaltax_deposited9 = objMySqlDataReader["totaltax_deposited9"].ToString();
                    objanx2datalist.bsrcode9 = objMySqlDataReader["bsrcode9"].ToString();
                    objanx2datalist.date_tax9 = objMySqlDataReader["date_tax9"].ToString();
                    objanx2datalist.challanno_tax9 = objMySqlDataReader["challanno_tax9"].ToString();
                    objanx2datalist.status9 = objMySqlDataReader["status9"].ToString();
                    objanx2datalist.totaltax_deposited10 = objMySqlDataReader["totaltax_deposited10"].ToString();
                    objanx2datalist.bsrcode10 = objMySqlDataReader["bsrcode10"].ToString();
                    objanx2datalist.date_tax10 = objMySqlDataReader["date_tax10"].ToString();
                    objanx2datalist.challanno_tax10 = objMySqlDataReader["challanno_tax10"].ToString();
                    objanx2datalist.status10 = objMySqlDataReader["status10"].ToString();
                    objanx2datalist.totaltax_deposited11 = objMySqlDataReader["totaltax_deposited11"].ToString();
                    objanx2datalist.bsrcode11 = objMySqlDataReader["bsrcode11"].ToString();
                    objanx2datalist.date_tax11 = objMySqlDataReader["date_tax11"].ToString();
                    objanx2datalist.challanno_tax11 = objMySqlDataReader["challanno_tax11"].ToString();
                    objanx2datalist.status11 = objMySqlDataReader["status11"].ToString();
                    objanx2datalist.totaltax_deposited12 = objMySqlDataReader["totaltax_deposited12"].ToString();
                    objanx2datalist.bsrcode12 = objMySqlDataReader["bsrcode12"].ToString();
                    objanx2datalist.date_tax12 = objMySqlDataReader["date_tax12"].ToString();
                    objanx2datalist.challanno_tax12 = objMySqlDataReader["challanno_tax12"].ToString();
                    objanx2datalist.status12 = objMySqlDataReader["status12"].ToString();
                    objanx2datalist.total_taxdeposited = objMySqlDataReader["total_taxdeposited"].ToString();

                    objMySqlDataReader.Close();
                }
                return objanx2datalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                ex.ToString();
                return null;
            }
        }

        public void DaPostanx2(MdlAnx2data values, string user_gid)
        {
            try
            {
                string uiDate1Str = values.date1_anx2;
                DateTime uiDate1 = DateTime.ParseExact(uiDate1Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date1_anx2 = uiDate1.ToString("yyyy-MM-dd");

                string uiDate2Str = values.date2_anx2;
                DateTime uiDate2 = DateTime.ParseExact(uiDate2Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date2_anx2 = uiDate2.ToString("yyyy-MM-dd");

                string uiDate3Str = values.date3_anx2;
                DateTime uiDate3 = DateTime.ParseExact(uiDate3Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date3_anx2 = uiDate3.ToString("yyyy-MM-dd");

                string uiDate4Str = values.date4_anx2;
                DateTime uiDate4 = DateTime.ParseExact(uiDate4Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date4_anx2 = uiDate4.ToString("yyyy-MM-dd");

                string uiDate5Str = values.date5_anx2;
                DateTime uiDate5 = DateTime.ParseExact(uiDate5Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date5_anx2 = uiDate5.ToString("yyyy-MM-dd");

                string uiDate6Str = values.date6_anx2;
                DateTime uiDate6 = DateTime.ParseExact(uiDate6Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date6_anx2 = uiDate6.ToString("yyyy-MM-dd");

                string uiDate7Str = values.date7_anx2;
                DateTime uiDate7 = DateTime.ParseExact(uiDate7Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date7_anx2 = uiDate7.ToString("yyyy-MM-dd");

                string uiDate8Str = values.date8_anx2;
                DateTime uiDate8 = DateTime.ParseExact(uiDate8Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date8_anx2 = uiDate8.ToString("yyyy-MM-dd");

                string uiDate9Str = values.date9_anx2;
                DateTime uiDate9 = DateTime.ParseExact(uiDate9Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date9_anx2 = uiDate9.ToString("yyyy-MM-dd");

                string uiDate10Str = values.date10_anx2;
                DateTime uiDate10 = DateTime.ParseExact(uiDate10Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date10_anx2 = uiDate10.ToString("yyyy-MM-dd");

                string uiDate11Str = values.date11_anx2;
                DateTime uiDate11 = DateTime.ParseExact(uiDate11Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date11_anx2 = uiDate11.ToString("yyyy-MM-dd");

                string uiDate12Str = values.date12_anx2;
                DateTime uiDate12 = DateTime.ParseExact(uiDate6Str, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string date12_anx2 = uiDate12.ToString("yyyy-MM-dd");

                msSQL = " delete from pay_trn_tannexure where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                string msGetGid = objcmnfunctions.GetMasterGID("ANRE");

                msSQL = " insert into pay_trn_tannexure ( " +
                        " annexure_gid, " +
                        " assessment_gid, " +
                        " employee_gid, " +
                        " totaltax_deposited1, " +
                        " totaltax_deposited2, " +
                        " totaltax_deposited3, " +
                        " totaltax_deposited4, " +
                        " totaltax_deposited5, " +
                        " totaltax_deposited6, " +
                        " totaltax_deposited7, " +
                        " totaltax_deposited8, " +
                        " totaltax_deposited9, " +
                        " totaltax_deposited10, " +
                        " totaltax_deposited11, " +
                        " totaltax_deposited12, " +
                        " bsrcode1, " +
                        " bsrcode2, " +
                        " bsrcode3, " +
                        " bsrcode4, " +
                        " bsrcode5, " +
                        " bsrcode6, " +
                        " bsrcode7, " +
                        " bsrcode8, " +
                        " bsrcode9, " +
                        " bsrcode10, " +
                        " bsrcode11, " +
                        " bsrcode12, " +
                        " challanno_tax1, " +
                        " challanno_tax2, " +
                        " challanno_tax3, " +
                        " challanno_tax4, " +
                        " challanno_tax5, " +
                        " challanno_tax6, " +
                        " challanno_tax7, " +
                        " challanno_tax8, " +
                        " challanno_tax9, " +
                        " challanno_tax10, " +
                        " challanno_tax11, " +
                        " challanno_tax12, " +
                        " date_tax1, " +
                        " date_tax2, " +
                        " date_tax3, " +
                        " date_tax4, " +
                        " date_tax5, " +
                        " date_tax6, " +
                        " date_tax7, " +
                        " date_tax8, " +
                        " date_tax9, " +
                        " date_tax10, " +
                        " date_tax11, " +
                        " date_tax12, " +
                        " status1, " +
                        " status2, " +
                        " status3, " +
                        " status4, " +
                        " status5, " +
                        " status6, " +
                        " status7, " +
                        " status8, " +
                        " status9, " +
                        " status10, " +
                        " status11, " +
                        " status12, " +
                        " total_taxdeposited, " +
                        " created_by," +
                        " created_date " +
                        " ) values ( " +
                        " '" + msGetGid + "', " +
                        " '" + values.assessment_gid + "', " +
                        " '" + values.employee_gid + "', " +
                        " '" + values.totaltax_dep1_anx2 + "', " +
                        " '" + values.totaltax_dep2_anx2 + "', " +
                        " '" + values.totaltax_dep3_anx2 + "', " +
                        " '" + values.totaltax_dep4_anx2 + "', " +
                        " '" + values.totaltax_dep5_anx2 + "', " +
                        " '" + values.totaltax_dep6_anx2 + "', " +
                        " '" + values.totaltax_dep7_anx2 + "', " +
                        " '" + values.totaltax_dep8_anx2 + "', " +
                        " '" + values.totaltax_dep9_anx2 + "', " +
                        " '" + values.totaltax_dep10_anx2 + "', " +
                        " '" + values.totaltax_dep11_anx2 + "', " +
                        " '" + values.totaltax_dep12_anx2 + "', " +
                        " '" + values.bsrcode1_anx2 + "', " +
                        " '" + values.bsrcode2_anx2 + "', " +
                        " '" + values.bsrcode3_anx2 + "', " +
                        " '" + values.bsrcode4_anx2 + "', " +
                        " '" + values.bsrcode5_anx2 + "', " +
                        " '" + values.bsrcode6_anx2 + "', " +
                        " '" + values.bsrcode7_anx2 + "', " +
                        " '" + values.bsrcode8_anx2 + "', " +
                        " '" + values.bsrcode9_anx2 + "', " +
                        " '" + values.bsrcode10_anx2 + "', " +
                        " '" + values.bsrcode11_anx2 + "', " +
                        " '" + values.bsrcode12_anx2 + "', " +
                        " '" + values.challan1_anx2 + "', " +
                        " '" + values.challan2_anx2 + "', " +
                        " '" + values.challan3_anx2 + "', " +
                        " '" + values.challan4_anx2 + "', " +
                        " '" + values.challan5_anx2 + "', " +
                        " '" + values.challan6_anx2 + "', " +
                        " '" + values.challan7_anx2 + "', " +
                        " '" + values.challan8_anx2 + "', " +
                        " '" + values.challan9_anx2 + "', " +
                        " '" + values.challan10_anx2 + "', " +
                        " '" + values.challan11_anx2 + "', " +
                        " '" + values.challan12_anx2 + "',";
                if (date1_anx2 != "")
                {
                    msSQL += "'" + date1_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date2_anx2 != "")
                {
                    msSQL += "'" + date2_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date3_anx2 != "")
                {
                    msSQL += "'" + date3_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date4_anx2 != "")
                {
                    msSQL += "'" + date4_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date5_anx2 != "")
                {
                    msSQL += "'" + date5_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date6_anx2 != "")
                {
                    msSQL += "'" + date6_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date7_anx2 != "")
                {
                    msSQL += "'" + date7_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date8_anx2 != "")
                {
                    msSQL += "'" + date8_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date9_anx2 != "")
                {
                    msSQL += "'" + date9_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date10_anx2 != "")
                {
                    msSQL += "'" + date10_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date11_anx2 != "")
                {
                    msSQL += "'" + date11_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                if (date12_anx2 != "")
                {
                    msSQL += "'" + date12_anx2 + "',";
                }
                else
                {
                    msSQL += "null,";
                }

                //if (date1_anx2 != "")
                //{
                //    msSQL += "'" + date1_anx2 + "',";
                //}
                //else
                //{
                //    msSQL += "null, ";
                //}

                msSQL += " '" + values.status1_anx2 + "', " +
                          " '" + values.status2_anx2 + "', " +
                          " '" + values.status3_anx2 + "', " +
                          " '" + values.status4_anx2 + "', " +
                          " '" + values.status5_anx2 + "', " +
                          " '" + values.status6_anx2 + "', " +
                          " '" + values.status7_anx2 + "', " +
                          " '" + values.status8_anx2 + "', " +
                          " '" + values.status9_anx2 + "', " +
                          " '" + values.status10_anx2 + "', " +
                          " '" + values.status11_anx2 + "', " +
                          " '" + values.status12_anx2 + "', " +
                          " '" + values.total_tax_deposited_anx2 + "', " +
                          " '" + values.employee_gid + "', " +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Annexure2 Details are Inserted Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while inserting Annexure2 details !!";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }



        public Dictionary<string, object> DaGetTdsPDF(string assessment_gid, string employee_gid, MdlPayMstAssessment values)
        {
            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();

            msSQL = "select contact_person,company_address,panno_deductor,tanno_deductor,cit_address,cit_city,cit_pincode,date_format(Now(),'%d-%m-%Y')as print_date from adm_mst_tcompany ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "adm_mst_tcompany");


            msSQL = " select tdsquarter1_receiptno,tdsquarter1_amount_deducted,tdsquarter1_amount_deposited,tdsquarter2_receiptno, " +
                    " tdsquarter2_amount_deducted,tdsquarter2_amount_deposited,tdsquarter3_receiptno,tdsquarter3_amount_deducted, " +
                    " tdsquarter3_amount_deposited,tdsquarter4_receiptno,tdsquarter4_amount_deducted,tdsquarter4_amount_deposited, " +
                    " tdsquarter_totalamount_deducted,tdsquarter_totalamount_deposited,tdsquarter1_paidcredited,tdsquarter2_paidcredited, " +
                    " tdsquarter3_paidcredited,tdsquarter4_paidcredited,totalamount_paidcredited,assessment_gid,employee_gid from pay_Trn_ttdssummary " +
                    " where assessment_gid='" + assessment_gid + "' and employee_gid='" + employee_gid + "'";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_Trn_ttdssummary");

            msSQL = " select totaltax_deposited1,totaltax_deposited2,totaltax_deposited3,totaltax_deposited4,totaltax_deposited5,totaltax_deposited6,totaltax_deposited7, " +
                    " totaltax_deposited8,totaltax_deposited9,totaltax_deposited10,totaltax_deposited11,totaltax_deposited12, " +
                    " bsrcode1,bsrcode2,bsrcode3,bsrcode4,bsrcode5,bsrcode6, " +
                    " bsrcode7,bsrcode8,bsrcode9,bsrcode10,bsrcode11,bsrcode12,date_format(date_tax1,'%d-%m-%Y') as date_tax1,date_format(date_tax2,'%d-%m-%Y') as date_tax2, " +
                    " date_format(date_tax3,'%d-%m-%Y') as date_tax3,date_format(date_tax4,'%d-%m-%Y') as date_tax4,date_format(date_tax5,'%d-%m-%Y') as date_tax5, " +
                    " date_format(date_tax6,'%d-%m-%Y') as date_tax6,date_format(date_tax7,'%d-%m-%Y') as date_tax7,date_format(date_tax8,'%d-%m-%Y') as date_tax8, " +
                    " date_format(date_tax9,'%d-%m-%Y') as date_tax9,date_format(date_tax10,'%d-%m-%Y') as date_tax10,date_format(date_tax11,'%d-%m-%Y') as date_tax11, " +
                    " date_format(date_tax12,'%d-%m-%Y') as date_tax12,challanno_tax1,challanno_tax2,challanno_tax3,challanno_tax4,challanno_tax5,challanno_tax6,challanno_tax7, " +
                    " challanno_tax8,challanno_tax9,challanno_tax10,challanno_tax11,challanno_tax12,total_taxdeposited,assessment_gid,employee_gid, " +
                    " status1,status2,status3,status4,status5,status6,status7,status8,status9,status10,status11," +
                    " status12 from pay_trn_tannexure where employee_gid='" + employee_gid + "' " + " and assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_trn_tannexure");

            msSQL = " select grosssalary_amount,perquisites_amount,profitinlieu_amount as profitinlieu,grosstotal_qualifiying_amount,assessment_gid,employee_gid from pay_trn_ttdsgrosssalary " + " where employee_gid='" + employee_gid + "' and assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_Trn_ttdsgrosssalary");

            msSQL = " select transport_totamount,transport_qualifiying_amount,balance_qualifiying_amount,entertainment_amount,taxonemployment_amount, " +
                    " aggreegate_qualifiying_amount,incomechargableunder_headsal_deductible_amount,assessment_gid,employee_gid,lessallowence_name1,lessallowence_name2, " +
                    " lessallowence_name3,lessallowence_amount2,lessallowence_amount3 from pay_trn_ttdsallowencetotheextent where " +
                    " employee_gid='" + employee_gid + "' and assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_Trn_ttdsallowencetotheextent");

            msSQL = " select otherincomeemployee_totamount1,otherincomeemployee_totamount2,otherincomeemployee_totamount3, " +
                    " otherincomeemployee_qualifiying_amount3,overallgross_deductible_amount,otherincome1_name,otherincome2_name, " +
                    " otherincome3_name,assessment_gid,employee_gid from pay_trn_ttdsotherincomeemployee where employee_gid='" + employee_gid + "' " + " and assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_Trn_ttdsotherincomeemployee");

            msSQL = " select section80c_grossamount1,section80c_deductible_amount1,section80c_grossamount2,section80c_deductible_amount2,section80c_grossamount3, " +
                    " section80c_deductible_amount3,section80c_grossamount4,section80c_deductible_amount4,section80c_grossamount5,section80c_deductible_amount5, " +
                    " section80c_grossamount6,section80c_deductible_amount6,section80c_grossamount7,section80c_deductible_amount7,section80ccc_grossamount, " +
                    " section80ccc_deductible_amount,section80ccd_grossamount,section80ccd_deductible_amount,section80c_name1,section80c_name2,section80c_name3, " +
                    " section80c_name4,section80c_name5,section80c_name6,section80c_name7,assessment_gid,employee_gid,aggregate_gross,aggregate_deduct,section80ccd1b_grossamount, " +
                    " section80ccd1b_deductamount,section80c_amount7 from pay_trn_ttdsdeductions where employee_gid='" + employee_gid + "' " + " and assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_Trn_ttdsdeductions");

            msSQL = " select section1_grossamount,section1_qualifiying_amount,section1_deductible_amount,section2_grossamount,section2_qualifiying_amount," +
                    " section2_deductible_amount,section3_grossamount,section3_qualifiying_amount,section3_deductible_amount,section4_grossamount,section4_qualifiying_amount, " +
                    " section4_deductible_amount,section5_grossamount,section5_qualifiying_amount,section5_deductible_amount,assessment_gid,employee_gid,section1_name, " +
                    " section2_name,section3_name,section4_name,section5_name from pay_trn_ttdsothersections " +
                    " where employee_gid='" + employee_gid + "' and assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_trn_ttdsothersections");

            msSQL = " select aggregatedeductable_totalamount,assessment_gid,employee_gid from pay_trn_ttdsaggregateofdeductable " +
                    " where employee_gid='" + employee_gid + "' and assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_trn_ttdsaggregateofdeductable");

            msSQL = " select a.pan_no, concat(b.user_firstname, ' ',b.user_lastname) employee_name,c.designation_name,x.city, " +
                    " concat(x.address1,' ',x.address2,' ',x.city,' ',x.state,' ',x.postal_code) as employee_address,a.employee_gid  " +
                    " from hrm_mst_temployee a " +
                    " inner join adm_mst_tuser b on a.user_gid=b.user_gid " +
                    " inner join adm_mst_tdesignation c on a.designation_gid=c.designation_gid " +
                    " left join adm_mst_taddress x on a.employee_gid=x.parent_gid " +
                    " left join hrm_trn_temployeedtl y on x.address_gid=y.permanentaddress_gid " +
                    " where a.employee_gid='" + employee_gid + "' group by employee_name ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = " select tax_deposit1,tax_deposit2,tax_deposit3,tax_deposit4,tax_deposit5,tax_deposit6,tax_deposit7,tax_deposit8,tax_deposit9,tax_deposit10," +
                    " tax_deposit11,tax_deposit12,receipt_no1,receipt_no2,receipt_no3,receipt_no4,receipt_no5,receipt_no6,receipt_no7,receipt_no8,receipt_no9, " +
                    " receipt_no10,receipt_no11,receipt_no12,ddo_no1,ddo_no2,ddo_no3,ddo_no4,ddo_no5,ddo_no6,ddo_no7,ddo_no8,ddo_no9,ddo_no10,ddo_no11,ddo_no12, " +
                    " date_format(date_transfer1,'%d-%m-%Y') as date_transfer1,date_format(date_transfer2,'%d-%m-%Y') as date_transfer2,date_format(date_transfer3,'%d-%m-%Y') as date_transfer3, " +
                    " date_format(date_transfer4,'%d-%m-%Y') as date_transfer4,date_format(date_transfer5,'%d-%m-%Y') as date_transfer5,date_format(date_transfer6,'%d-%m-%Y') as date_transfer6, " +
                    " date_format(date_transfer7,'%d-%m-%Y') as date_transfer7,date_format(date_transfer8,'%d-%m-%Y') as date_transfer8,date_format(date_transter9,'%d-%m-%Y') as date_transter9, " +
                    " date_format(date_transfer10,'%d-%m-%Y') as date_transfer10,date_format(date_transfer11,'%d-%m-%Y') as date_transfer11,date_format(date_transfer12,'%d-%m-%Y') as date_transfer12, " +
                    " status1,status2,status3,status4,status5,status6,status7,status8,status9 as stauts9,status10,status11," +
                    " status12,total_taxdeposit,assessment_gid,employee_gid from pay_trn_tannexure1 " +
                    " where employee_gid='" + employee_gid + "' and assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_trn_tannexure1");

            msSQL = " select employee_gid,total_income,tax_total_income,educationcess_amount,tax_payable12plus13,less_relief89,tax_payable14minus15, " +
                    " less_tds,balance_tax,assessment_gid from pay_trn_ttds  where employee_gid='" + employee_gid + "' " + " and assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_trn_ttds");

            msSQL = " select cast(concat(assessmentyear_startdate,' ','to',' ',assessmentyear_enddate) as char) as assessment_year,date_format(financialyear_startdate,'%d-%m-%Y') as financialyear_startdate," +
                    " date_format(financialyear_enddate,'%d-%m-%Y') as financialyear_enddate,assessment_gid from pay_mst_tassessmentyear where assessment_gid='" + assessment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "pay_mst_tassessmentyear");


            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_payroll"].ToString(), "pay_rpt_form16.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "TDS_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();

            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);

            return ls_response;

        }
        public MdlIncometaxData DaGetIncometaxdata(string employee_gid)
        {
            try
            {
                MdlIncometaxData objincometaxdatalist = new MdlIncometaxData();

                msSQL = " select fin_year, documenttype_gid, document_title, remarks from pay_trn_ttaxdocument " +
                        " where a.employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();

                    objincometaxdatalist.fin_year = objMySqlDataReader["fin_year"].ToString();
                    objincometaxdatalist.documenttype_gid = objMySqlDataReader["documenttype_gid"].ToString();
                    objincometaxdatalist.document_title = objMySqlDataReader["document_title"].ToString();
                    objincometaxdatalist.remarks = objMySqlDataReader["remarks"].ToString();

                    objMySqlDataReader.Close();
                }
                return objincometaxdatalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                ex.ToString();
                return null;
            }
        }
    }
}
