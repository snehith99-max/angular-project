using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ems.hrm.Models;
using ems.utilities.Functions;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace ems.hrm.DataAccess
{
    public class DaAddRelievingLetter
    {
        dbconn objdbconn = new dbconn();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objtbl;
        int mnresult, mnresult1;
        string msGetGID, msGetGid_dtl, lsbranch_gid, lsdesignation_gid, lsdepartment_gid, lsexistdate, lsdepartmentmanager_gid, lsuser_name, lsemployeegender, lsemployee_qualification,
            lspermanentaddress_gid, lstemporaryaddress_gid, lsUser_Email, lsUser_Phone, lsfirstname,lslastname;
        DataTable dt1 = new DataTable();
        DataTable DataTable2 = new DataTable();
        string company_logo_path;
        System.Drawing.Image company_logo;
        public void DaGetEmployeedropdown(MdlAddRelievingLetter values)
        {
            try
            {
                msSQL = " select a.employee_gid, concat(b.user_code,' || ',b.user_firstname,' ',b.user_lastname) as empname, a.employee_joiningdate, a.exit_date " +
                        " from hrm_mst_temployee a left join adm_mst_tuser b on b.user_gid = a.user_gid " +
                        " where user_status='N' and a.employee_joiningdate <> '' and a.exit_date <> '' and employee_gid not in (SELECT employee_gid FROM hrm_trn_texitemployee) order by b.user_firstname ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Employeelists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Employeelists
                        {
                            Employeegid = dt["employee_gid"].ToString(),
                            EmployeeCode = dt["empname"].ToString(),                            
                        });
                        values.Employeelists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetOnChangeEmployee(MdlAddRelievingLetter values, string Employeegid)
        {
            try
            {
                string lsjoiningdate, lsexitdate;

                msSQL = "SELECT DISTINCT c.branch_name, e.department_name, " +
                        "CONCAT(d.user_firstname, ' ', d.user_lastname) AS empname, " +
                        "DATE_FORMAT(b.exit_date, '%d-%m-%Y') AS exit_date, " +
                        "f.designation_name, d.user_code AS employee_code, d.user_gid, " +
                        "b.employee_gid, DATE_FORMAT(b.employee_joiningdate, '%d-%m-%Y') AS employee_joiningdate, " +
                        "d.user_status " +
                        "FROM hrm_mst_temployee b " +
                        "LEFT JOIN hrm_mst_tbranch c ON c.branch_gid = b.branch_gid " +
                        "LEFT JOIN adm_mst_tuser d ON d.user_gid = b.user_gid " +
                        "LEFT JOIN hrm_mst_tdepartment e ON e.department_gid = b.department_gid " +
                        "LEFT JOIN adm_mst_tdesignation f ON b.designation_gid = f.designation_gid " +
                        "WHERE b.employee_gid = '" + Employeegid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEmployeeonchangedetails>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        lsjoiningdate = dt["employee_joiningdate"].ToString();
                        lsexitdate = dt["exit_date"].ToString();

                        int lsday = 0, lsyear = 0, lsmonth = 0;
                        string lsduration = "";

                        if (DateTime.TryParseExact(lsjoiningdate, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime joiningDate) &&
                            DateTime.TryParseExact(lsexitdate, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime exitDate))
                        {
                            TimeSpan duration = exitDate - joiningDate;
                            lsyear = (int)Math.Floor(duration.TotalDays / 365);
                            lsmonth = (int)Math.Floor((duration.TotalDays % 365) / 30);
                            lsday = (int)(duration.TotalDays % 30);

                            lsduration = $"{lsyear} Years {lsmonth} Months {lsday} Days";
                        }

                        getModuleList.Add(new GetEmployeeonchangedetails
                        {
                            Name = dt["empname"].ToString(),
                            IDNo = dt["employee_code"].ToString(),
                            joiningdate = lsjoiningdate,
                            exit_date = lsexitdate,
                            Department = dt["department_name"].ToString(),
                            Designation = dt["designation_name"].ToString(),
                            TotalServices = lsduration,
                        });
                    }
                    values.GetEmployeeonchangedetails = getModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Change Customer!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaRelivingLetterTemplate(MdlAddRelievingLetter values)
        {
            try
            {
                msSQL = " select a.template_gid,a.template_name, a.template_content from adm_mst_ttemplate a " +
                       " left join adm_trn_ttemplate2module b on a.template_gid=b.template_gid where b.module_gid='HRM'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRelievingLetterdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRelievingLetterdropdown
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_content = dt["template_content"].ToString()
                        });
                        values.GetRelievingLetterdropdown = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaPostRelievingLetter(PostEmployeeLists values, string user_gid)
        {
            try
            {
                string uiDateStr2 = values.Date_of_Joining;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string Date_of_Joining = uiDate2.ToString("yyyy-MM-dd");

                string uiDateStr3 = values.Date_of_Relieving;
                DateTime uiDate3 = DateTime.ParseExact(uiDateStr3, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string Date_of_Relieving = uiDate3.ToString("yyyy-MM-dd");

                msSQL = " select a.branch_gid, a.designation_gid,department_gid,a.exit_date" +
                        " from hrm_mst_temployee a " +
                        " where a.employee_gid='" + values.employee_gid+"'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lsbranch_gid = objOdbcDataReader["branch_gid"].ToString();
                    lsdesignation_gid = objOdbcDataReader["designation_gid"].ToString();
                    lsdepartment_gid = objOdbcDataReader["department_gid"].ToString();
                    lsexistdate = objOdbcDataReader["exit_date"].ToString();

                    objOdbcDataReader.Close();
                }

                msGetGID = objcmnfunctions.GetMasterGID("EXTE");

                msSQL = " insert into hrm_trn_texitemployee ( " +
                        " exitemployee_gid, " +
                        " branch_gid, " +
                        " designation_gid, " +
                        " department_gid, " +
                        " employee_gid, " +
                        " overall_status, " +
                        " remarks," +
                        " exit_date, " +
                        " created_by, " +
                        " created_date " +
                        " ) values ( " +
                        " '" + msGetGID + "', " +
                        " '" + lsbranch_gid + "', " +
                        " '" + lsdesignation_gid + "', " +
                        " '" + lsdepartment_gid + "', " +
                        " '" + values.employee_gid + "', " +
                        " 'Approved', " +
                        " '" + values.Reason_for_Settlement + "', " +
                        " '" + Convert.ToDateTime(lsexistdate).ToString("yyyy-MM-dd") + "'," +
                        " '" + user_gid + "', " +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "') ";
                mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select department_manager from hrm_mst_tdepartment where department_gid='" + lsdepartment_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lsdepartmentmanager_gid = objOdbcDataReader["department_manager"].ToString();

                    msSQL = " select concat(b.user_firstname,'',b.user_lastname) as empname from hrm_mst_temployee a" +
                            " left join adm_mst_tuser b on b.user_gid=a.user_gid" +
                            " where a.employee_gid= '" + lsdepartmentmanager_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows == true)
                    {
                        lsuser_name = objOdbcDataReader["empname"].ToString();

                        objOdbcDataReader.Close();
                    }
                }

                        msGetGid_dtl = objcmnfunctions.GetMasterGID("EXTD");

                        msSQL = " insert into hrm_trn_texitemployeedtl ( " +
                                " exitemployeedtl_gid, " +
                                " exitemployee_gid, " +
                                " employee_gid, " +
                                " department_gid, " +
                                " exit_status, " +
                                " department_manager, " +
                                " manager_gid, " +
                                " created_by, " +
                                " created_date " +
                                " ) values ( " +
                                " '" + msGetGid_dtl + "', " +
                                " '" + values.employee_gid + "', " +
                                " '" + values.employee_gid + "', " +
                                " '" + lsdepartment_gid + "', " +
                                " 'Approved', " +
                                " '" + lsuser_name + "'," +
                                " '" + lsdepartmentmanager_gid + "'," +
                                " '" + user_gid + "', " +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "') ";
                        mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update hrm_trn_texitemployee set overall_status='Approved',approvalhrdepartment_flag='N' where employee_gid='" + values.employee_gid+ "'";
                        mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msGetGID = objcmnfunctions.GetMasterGID("HRST");
                        msSQL = " Insert into hrm_trn_tsettlement " +
                                " (settlement_gid," +
                                " employee_gid," +
                                " settlement_flag, " +
                                " relieving_date, " +
                                " created_date, " +
                                " created_by" +
                                "  ) " +
                                " values ( " +
                               " '" + msGetGID + "', " +
                               " '" +values.employee_gid + "', " +
                               " 'Y', " +
                               " '" + Convert.ToDateTime(lsexistdate).ToString("yyyy-MM-dd") + "'," +
                               " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                               " '" + values.employee_gid + "') ";
                        mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnresult== 1) 
                {
                    msSQL =   " update hrm_trn_tsettlement set " +
                              " settlement_wages='" +values.Salary + "', " +
                              " settlement_pf='" + values.EPF + "', " +
                              " settlement_lwf='" +values.LWF + "', " +
                              " settlement_bonus='" + values.Bonus + "', " +
                              " settlement_loan='" +values.Loan + "', " +
                              " settlement_leavesalary='" + values.Leave_Salary + "', " +
                              " settlement_esic='" + values.ESIC + "' , " +
                              " settlement_gratuity='" + values.Gratuity + "', " +
                              " settlement_totalamount='" + values.Total + "' , " +
                              " joining_date= '" + Date_of_Joining + "'," +
                              " employee_salary='" + values.Min_Wages + "'," +
                              " total_service='" +values.Total_Services + "', " +
                              " reason='" + values.Reason_for_Settlement + "'," +
                              " settlement_flag='Y'" +
                              " where settlement_gid='" + msGetGID + "'";
                    mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                msSQL = " update hrm_mst_temployee set " +
                        " exit_date='" + Date_of_Relieving + "' " +
                        " where employee_gid='" +values.employee_gid + "' ";
                mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select  a.employee_joiningdate,c.user_firstname,c.user_lastname, a.employee_qualification,b.temporaryaddress_gid,b.permanentaddress_gid," +
                        " a.designation_gid, a.employee_phoneno, a.employee_mobileno, a.employee_dob, a.employee_emailid," +
                        " a.employee_experience, a.employee_experiencedtl, a.employee_gender" +
                        " from hrm_mst_temployee a" +
                        " left join hrm_trn_temployeedtl b on a.employee_gid = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid=a.user_gid" +
                        " where a.employee_gid = '" + values.employee_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lsemployeegender = objOdbcDataReader["employee_gender"].ToString();
                    lsfirstname = objOdbcDataReader["user_firstname"].ToString();
                    lslastname = objOdbcDataReader["user_lastname"].ToString();
                    lsUser_Phone = objOdbcDataReader["employee_mobileno"].ToString();
                    lsUser_Email = objOdbcDataReader["employee_emailid"].ToString();
                    lstemporaryaddress_gid = objOdbcDataReader["temporaryaddress_gid"].ToString();
                    lspermanentaddress_gid = objOdbcDataReader["permanentaddress_gid"].ToString();
                    lsemployee_qualification = objOdbcDataReader["employee_qualification"].ToString();
                    objOdbcDataReader.Close();
                }
                msGetGid_dtl = objcmnfunctions.GetMasterGID("HORL");

                msSQL = " insert into hrm_trn_treleivingletter " +
                        " ( releiving_gid ," +
                        " employee_gid , " +
                        " first_name," +
                        " last_name," +
                        " gender, " +
                        " mobile_number, " +
                        " offertemplate_content," +
                        " email_address," +
                        " qualification," +
                        " perm_address_gid," +
                        " temp_address_gid," +
                        " template_gid," +
                        " created_by," +
                        " created_date, " +
                        " employee_salary, " +
                        " joiningdate " +
                        " )values ( " +
                        "'" + msGetGid_dtl + "', " +
                        "'" + values.employee_gid + "', " +
                        "'" +lsfirstname + "'," +
                        "'" + lslastname + "'," +
                        "'" + lsemployeegender + "'," +
                        "'" + lsUser_Phone + "'," +
                        "'" + values.Relievinglettertemplate_content + "'," +
                        "'" + lsUser_Email + "'," +
                        "'" + lsemployee_qualification + "'," +
                        "'" + lspermanentaddress_gid + "'," +
                        "'" + lstemporaryaddress_gid + "'," +
                        "'" + values.templatename + "'," +
                        "'" + user_gid+ "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'," +
                        "'" +values.employee_salary + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
            mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnresult != 0)
                {
                    values.status = true;
                    values.message = "Relieving Letter added successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while adding Relieving letter";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public Dictionary<string, object> DaGetrelievingletterpdf(string releiving_gid, MdlAddRelievingLetter values,string branch_gid)
        {
            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();

            msSQL = "select offertemplate_content,first_name from hrm_trn_treleivingletter " +
                    " where releiving_gid  ='" + releiving_gid + "'";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = "SELECT a.branch_logo_path as company_logo " +
                    " FROM hrm_mst_tbranch a  where a.branch_gid='" + branch_gid + "'";
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

            ReportDocument oRpt = new ReportDocument();
            string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ems.hrm", "hrm_crp_relievingletter.rpt");
            oRpt.Load(reportPath);
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_file_path_hrm"].ToString(), "Relievingletter" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();
            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;
        }
    }
}

  
