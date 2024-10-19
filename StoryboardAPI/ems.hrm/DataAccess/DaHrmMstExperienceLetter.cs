using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using MySqlX.XDevAPI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using static System.Drawing.ImageConverter;
using System.Web.UI.WebControls;
using Mysqlx;
using System.Web.Http.Controllers;

namespace ems.hrm.DataAccess
{
    public class DaHrmMstExperienceLetter
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader, objMySqlDataReader2;


        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, experience_gid, msGetGid1, lsContent, xDate, xDate1, lsduration, lsdate, lsjoining_date, lsexit_date, lsempoyeegid, msTemporaryAddressGetGID, lsemployee2salarygradetemplate_gid, msGetemployeetype, msPermanentAddressGetGID, msgetshift, lssalarycomponent_name, lssalarycomponent_gid, msUserGid, msEmployeeGID;
        double lsday, lsyear, lsmonth;
        DataTable dt1 = new DataTable();
        DataTable DataTable2 = new DataTable();
        string company_logo_path;
        System.Drawing.Image company_logo;

        public void DaExperienceLetterSummary(MdlHrmMstExperienceLetter values)
        {
            msSQL = " select a.experience_gid, concat(c.user_code, ' || ',c.user_firstname,' ',c.user_lastname) as first_name, d.branch_prefix, e.department_name, " +
                    " f.designation_name from hrm_trn_texperienceletter a left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                    " left join adm_mst_tuser c on b.user_gid = c.user_gid left join hrm_mst_tbranch d on b.branch_gid = d.branch_gid " +
                    " left join hrm_mst_tdepartment e on b.department_gid = e.department_gid left join adm_mst_tdesignation f on b.designation_gid = f.designation_gid order by experience_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            
            var getModuleList = new List<Experiencesummary_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Experiencesummary_list
                    {
                        experience_gid = dt["experience_gid"].ToString(),
                        //user_code = dt["user_code"].ToString(),
                        first_name = dt["first_name"].ToString(),
                        branch_name = dt["branch_prefix"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        designation_name = dt["designation_name"].ToString(),
                    });
                    values.Experiencesummary_list = getModuleList;
                }
            }
        }

        public void DaGetUserDetail(MdlHrmMstExperienceLetter values)
        {
            msSQL = " select a.employee_gid, concat(b.user_code,' || ',b.user_firstname,' ',b.user_lastname) as employee_name, a.employee_joiningdate, a.exit_date from hrm_mst_temployee a " +
                    " inner join adm_mst_tuser b on b.user_gid=a.user_gid " +
                    " inner join hrm_mst_tbranch c on c.branch_gid=a.branch_gid " +
                    " where a.employee_joiningdate <> '' and a.exit_date <> '' and employee_gid not in (SELECT employee_gid FROM hrm_trn_texperienceletter) order by b.user_firstname ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetUserdropdown>();

            if (dt_datatable.Rows.Count != 0)
            {

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetUserdropdown
                    {
                        employee_gid = dt["employee_gid"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                    });
                    values.GetUserDtl = getModuleList;

                }
            }
            dt_datatable.Dispose();
        }
        public void DaAddexperienceletter(string employee_gid, AddExperienceletter_list values)
        {

            DateTime lsjoindate, lsexitdate;

            msSQL = " select b.user_firstname, b.user_lastname, a.employee_gender, a.employee_dob, a.employee_mobileno, a.employee_emailid, a.employee_qualification, " +
                    " a.employee_joiningdate, a.exit_date from hrm_mst_temployee a " +
                    " inner join adm_mst_tuser b on a.user_gid = b.user_gid " +
                    " where a.employee_gid = '" + values.employee_gid + "' group by a.employee_gid ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);

            if (objMySqlDataReader.HasRows == true)
            {
                objMySqlDataReader.Read();

                lsjoining_date = objMySqlDataReader["employee_joiningdate"].ToString();
                lsexit_date = objMySqlDataReader["exit_date"].ToString();

                msSQL = " select employee_gid from hrm_trn_texperienceletter where employee_gid='" + values.employee_gid + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    values.message = "Experience letter already generated";
                }
                else
                {
                    DateTime xDate = Convert.ToDateTime(lsexit_date);
                    DateTime xDate1 = Convert.ToDateTime(lsjoining_date);
                    
                    int lsday = (int)(xDate - xDate1).TotalDays;           // Calculate the difference in days between xDate1 and xDate
                    
                    int lsyear = lsday / 365;                              // Calculate years
                    
                    int remainingDays = lsday % 365;                       // Calculate remaining days after subtracting years
                    
                    int lsmonth = remainingDays / 30;                      // Calculate months from remaining days
                    
                    int remainingDaysAfterMonths = remainingDays % 30;     // Calculate remaining days after subtracting months
                    
                    int lsdayRemaining = remainingDaysAfterMonths;         // The remaining days will be the days
                    
                    lsduration = $"{lsyear} Years {lsmonth} Months {lsdayRemaining} Days";      // Format the duration as a string
                }

                msGetGid = objcmnfunctions.GetMasterGID("HOEL");

                msSQL = " select b.user_firstname, b.user_lastname, a.employee_gender, a.employee_dob, a.employee_mobileno, a.employee_emailid, a.employee_qualification, " +
                        " a.employee_joiningdate, a.exit_date from hrm_mst_temployee a " +
                        " inner join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " where a.employee_gid ='" + values.employee_gid + "' group by a.employee_gid";
                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader2.HasRows == true)
                {
                    objMySqlDataReader2.Read();

                    if (objMySqlDataReader2["employee_joiningdate"].ToString() != "")
                    {
                        lsjoindate = Convert.ToDateTime(objMySqlDataReader2["employee_joiningdate"].ToString());

                        msSQL = " insert into hrm_trn_texperienceletter " +
                                " ( experience_gid ," +
                                " employee_gid, " +
                                " first_name," +
                                " last_name, " +
                                " gender, " +
                                " mobile_number, " +
                                " email_address," +
                                " qualification," +
                                " experience_detail," +
                                " offertemplate_content," +
                                " created_by," +
                                " created_date, " +
                                " joiningdate " +
                                " )values ( " +
                                "'" + msGetGid + "', " +
                                "'" + values.employee_gid + "', " +
                                "'" + objMySqlDataReader2["user_firstname"].ToString() + "'," +
                                "'" + objMySqlDataReader2["user_lastname"].ToString() + "'," +
                                "'" + objMySqlDataReader2["employee_gender"].ToString() + "'," +
                                "'" + objMySqlDataReader2["employee_mobileno"].ToString() + "'," +
                                "'" + objMySqlDataReader2["employee_emailid"].ToString() + "'," +
                                "'" + objMySqlDataReader2["employee_qualification"].ToString() + "'," +
                                "'" + lsduration + "'," +
                                "'" + values.Experiencelettertemplate_content + "'," +
                                "'" + employee_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'" + lsjoindate.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        objMySqlDataReader2.Close();
                    }
                }
            }
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Experience Letter Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Adding Experience Letter";
            }

            msSQL = " update hrm_trn_texperienceletter set " +
                    " template_gid= '" + values.template_name + "', " +
                    " offertemplate_content='" + values.Experiencelettertemplate_content + "'" +
                    " where experience_gid = '" + msGetGid + "' ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        }
        public void DaDeleteExperience(string experience_gid, AddExperienceletter_list values)
        {
            msSQL = "  delete from hrm_trn_texperienceletter where experience_gid='" + experience_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Experience Deleted Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Deleting Experience";
                }
            }
        }
        public void DaGetOnChangeEmployee(string employee_gid, MdlAppoinmentOrder values)
        {
            msSQL = "select b.user_firstname,b.user_lastname,b.user_code,c.designation_name,a.employee_gender,a.employee_dob,a.employee_mobileno,a.employee_emailid,a.employee_qualification, " +
                " date_format(a.employee_joiningdate,'%d %M %Y')as employee_joiningdate,date_format(a.exit_date,'%d %M %Y')as exit_date from hrm_mst_temployee a " +
                " inner join adm_mst_tuser b on a.user_gid=b.user_gid " +
                 "inner join adm_mst_tdesignation c on a.designation_gid=c.designation_gid" +
                " where a.employee_gid='" + employee_gid + "' group by a.employee_gid";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetEmployeeList>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEmployeeList
                    {
                        user_code = dt["user_code"].ToString(),
                        employee_name = dt["user_firstname"].ToString(),
                        user_lastname = dt["user_lastname"].ToString(),
                        employee_joiningdate = dt["employee_joiningdate"].ToString(),
                        exit_date = dt["exit_date"].ToString(),
                        designation_name = dt["designation_name"].ToString(),                     

                    });
                    values.GetEmployeeList = getModuleList;
                }
            }
        }
        public Dictionary<string, object> DaGetexperienceletterpdf(string experience_gid, MdlHrmMstExperienceLetter values, string branch_gid)
         {


            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();


            msSQL = "select offertemplate_content,first_name from hrm_trn_texperienceletter " +
                                   " where experience_gid ='" + experience_gid + "'";
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
            string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ems.hrm", "hrm_crp_experienceletter.rpt");
            oRpt.Load(reportPath);
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_file_path_hrm"].ToString(), "Experienceletter" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();
            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;
        }

    }
}


