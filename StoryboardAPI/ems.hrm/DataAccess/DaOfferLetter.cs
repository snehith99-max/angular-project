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
using Mysqlx.Crud;
namespace ems.hrm.DataAccess
{
    public class DaOfferLetter
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader, objMySqlDataReader2;

        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid, msTemporaryAddressGetGID, lsemployee2salarygradetemplate_gid, msGetemployeetype, msPermanentAddressGetGID, msgetshift, lssalarycomponent_name, lssalarycomponent_gid, msUserGid, msEmployeeGID;
        double lsbasicsalary, lsnetsalary;
        Double lsother_addition = 0.00;
        Double lsotheraddition_employer = 0.00;
        Double lsother_deduction = 0.00;
        Double lsotherdeduction_employer = 0.00;
        Double lsdeductamount = 0.00;
        Double lsbasic_salary = 0.00;
        Double lsbasic_salary_employeer = 0.00;
        DataTable dt1 = new DataTable();
        DataTable DataTable2 = new DataTable();
        string company_logo_path;
        Image company_logo;

        public void DaOfferLetterSummary(MdlOfferLetter values)
        {
            try
            {
                msSQL = " SELECT a.offer_gid, concat(a.first_name,' ',a.last_name) as emp_name, a.gender, a.dob, a.mobile_number, a.email_address, a.qualification,a.offerletter_type, " +
                        " a.experience_detail, a.perm_address_gid, a.temp_address_gid, a.template_gid, a.created_by, a.created_date,a.employee_gid, " +
                        " a.branch_name,a.designation_name,date_format(a.offer_date,'%d-%m-%Y') as offer_date,a.department_name, date_format(a.joiningdate,'%d-%m-%Y') as joining_date " +
                        " FROM hrm_trn_tofferletter a " +
                        " left join hrm_trn_temployeetypedtl j on a.employee_gid=j.employee_gid " +
                        " order by a.offer_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Offersummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Offersummary_list
                        {
                            offer_gid = dt["offer_gid"].ToString(),
                            emp_name = dt["emp_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            offer_date = dt["offer_date"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            joining_date = dt["joining_date"].ToString(),
                        });
                        values.Offersummary_list = getModuleList;
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
        public void DaAddofferletter(string employee_gid, AddOfferletter_list values)
        {
            try
            {
                msSQL = "select branch_gid from hrm_mst_tbranch where branch_name='" + values.branch_gid + "'";
                string lsbranchgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select designation_gid from adm_mst_tdesignation where designation_name='" +  values.designation_gid + "'";
                string lsdesignation = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select department_gid from hrm_mst_Tdepartment where department_name='" + values.department_gid + "'";
                string lsdeparmentname = objdbconn.GetExecuteScalar(msSQL);

                msGetGid = objcmnfunctions.GetMasterGID("HOFP");
                if (DateTime.TryParseExact(values.offer_date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    values.offer_date = parsedDate.ToString("yyyy-MM-dd");
                }
                //if (DateTime.TryParseExact(values.dob, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate1))
                //{
                //    values.dob = parsedDate1.ToString("yyyy-MM-dd");
                //}
                string dateofbirth;
                if(values.dob == null || values.dob == "" || values.dob == "undefined")
                {
                    dateofbirth = "0000-00-00";
                }
                else
                {
                    //string uiDateStr = values.dob;
                    //DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    dateofbirth = values.dob;
                }
                if (DateTime.TryParseExact(values.joiningdate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate2))
                {
                    values.joiningdate = parsedDate2.ToString("yyyy-MM-dd");
                }
          

                msSQL =  " insert into hrm_trn_tofferletter " +
                         " ( offer_gid ," +
                         " offer_refno," +
                         " offer_date," +
                         " first_name," +
                         " last_name, " +
                         " gender, " +
                         " dob, " +
                         " mobile_number, " +
                         " email_address," +
                         " qualification," +
                         " experience_detail," +
                         " document_path," +
                         " created_by," +
                         " created_date, " +
                         " employee_salary, " +
                         " perm_address1, " +
                         " perm_address2, " +
                         " perm_country, " +
                         " perm_state, " +
                         " perm_city, " +
                         " perm_pincode, " +
                         " temp_address1, " +
                         " temp_address2, " +
                         " temp_country, " +
                         " temp_state, " +
                         " temp_city, " +
                         " temp_pincode, " +
                         " joiningdate, " +
                         " template_gid, " +
                         " letter_flag, " +
                         " employee_gid," +
                         " designation_gid," +
                         " designation_name, " +
                         " department_gid," +
                         " department_name, " +
                         " branch_gid," +
                         " offerletter_type," +
                         " offertemplate_content," +
                         " branch_name " +
                         " )values ( " +
                         "'" + msGetGid + "', " +
                         "'" + values.offer_no + "'," +
                         "'" + values.offer_date + "'," +
                         "'" + values.first_name + "'," +
                         "'" + values.last_name + "'," +
                         "'" + values.gender + "'," +
                         "'" + dateofbirth + "'," +
                         "'" + values.mobile_number + "'," +
                         "'" + values.email_address + "'," +
                         "'" + values.qualification + "'," +
                         "'" + values.experience_detail + "'," +
                         "'" + values.document_path + "', " +
                         "'" + employee_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                         "'" + values.employee_salary + "'," +
                         "'" + values.perm_address1 + "'," +
                         "'" + values.perm_address2 + "'," +
                         "'" + values.permanent_country + "'," +
                         "'" + values.perm_state + "'," +
                         "'" + values.perm_city + "'," +
                         "'" + values.perm_pincode + "'," +
                         "'" + values.temp_address1 + "'," +
                         "'" + values.temp_address2 + "'," +
                         "'" + values.temp_country + "'," +
                         "'" + values.temp_state + "'," +
                         "'" + values.temp_city + "'," +
                         "'" + values.temp_pincode + "'," +
                         "'" + values.joiningdate + "'," +
                         "'" + values.template_gid + "', " +
                         "'Pending'," +
                         "'" + employee_gid + "'," +
                         "'" + lsdesignation + "'," +
                         "'" + values.designation_gid + "'," +
                         "'" + values.department_gid + "'," +
                         "'" + lsdeparmentname + "'," +
                         "'" + lsbranchgid + "'," +
                         "'" + values.offerletter_type + "'," +
                         "'" + values.offertemplate_content + "'," +
                         "'" + values.branch_gid + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Offer Letter Added Sucessfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Offer Letter";
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaConfirmEmployee(string user_gid, EmployeedataConfirmation values)
        {
            try
            {
                msSQL = "select * from hrm_trn_tappointmentorder where offer_gid = '" + values.offer_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);                
                if (objMySqlDataReader.HasRows == true)
                {
                    msSQL = "delete from hrm_trn_tappointmentorder where offer_gid = '" + values.offer_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                msSQL = "select user_code from adm_mst_Tuser where user_code = '" + values.user_code + "'";
                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader2.HasRows == true)
                {
                    msSQL = "select b.employee_gid from adm_mst_Tuser a" +
                            " left join hrm_mst_temployee b on a.user_gid=b.user_gid where a.user_code='" + values.user_code + "'";

                    string lsemployeegid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "delete from adm_mst_tuser where user_code='" + values.user_code + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "delete from hrm_mst_temployee where employee_gid='" + lsemployeegid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "delete from hrm_trn_temployeetypedtl where employee_gid='" + lsemployeegid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "delete from adm_mst_taddress where parent_gid='" + lsemployeegid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "select employee2salarygradetemplate_gid from pay_trn_temployee2salarygradetemplate where employee_gid = '" + lsemployeegid + "'";
                    lsemployee2salarygradetemplate_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "delete from adm_mst_tuser where user_code='" + values.user_code + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "delete from pay_trn_temployee2salarygradetemplate where employee2salarygradetemplate_gid='" + lsemployee2salarygradetemplate_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "delete from pay_trn_temployee2salarygradetemplatedtl where employee2salarygradetemplate_gid='" + lsemployee2salarygradetemplate_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                msSQL = "select * from hrm_trn_tofferletter where offer_gid = '" + values.offer_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    objMySqlDataReader.Read();

                    string msgetgidof = objcmnfunctions.GetMasterGID("OAPM");

                    msSQL = " insert into hrm_trn_tappointmentorder" +
                        " ( appointmentorder_gid ," +
                        " offer_gid," +
                        " first_name," +
                        " last_name, " +
                        " gender, " +
                        " mobile_number, " +
                        " email_address," +
                        " qualification," +
                        " experience_detail," +
                        " perm_address_gid," +
                        " temp_address_gid," +
                        " perm_address1, " +
                        " perm_address2, " +
                        " perm_country, " +
                        " perm_state, " +
                        " perm_city, " +
                        " perm_pincode, " +
                        " temp_address1, " +
                        " temp_address2, " +
                        " temp_country, " +
                        " temp_state, " +
                        " temp_city, " +
                        " temp_pincode, " +
                        " designation_gid," +
                        " designation_name, " +
                        " branch_gid," +
                        " branch_name, " +
                        " department_gid," +
                        " department_name, " +
                        " appointment_date, " +
                        " employee_salary," +
                        " created_by," +
                        " created_date) " +
                        " values ( " +
                        "'" + msgetgidof + "', " +
                        "'" + values.offer_gid + "', " +
                        "'" + objMySqlDataReader["first_name"].ToString() + "'," +
                        "'" + objMySqlDataReader["last_name"].ToString() + "'," +
                        "'" + objMySqlDataReader["gender"].ToString() + "'," +
                        //"'" + DateTime.Parse(objMySqlDataReader["dob"]).ToString("yyyy-MM-dd") + "'," +
                        "'" + objMySqlDataReader["mobile_number"].ToString() + "'," +
                        "'" + objMySqlDataReader["email_address"].ToString() + "'," +
                        "'" + objMySqlDataReader["qualification"].ToString() + "'," +
                        "'" + objMySqlDataReader["experience_detail"].ToString() + "'," +
                        "'" + objMySqlDataReader["perm_address_gid"].ToString() + "'," +
                        "'" + objMySqlDataReader["temp_address_gid"].ToString() + "'," +
                        "'" + objMySqlDataReader["perm_address1"].ToString() + "'," +
                        "'" + objMySqlDataReader["perm_address2"].ToString() + "'," +
                        "'" + objMySqlDataReader["perm_country"].ToString() + "'," +
                        "'" + objMySqlDataReader["perm_state"].ToString() + "'," +
                        "'" + objMySqlDataReader["perm_city"].ToString() + "'," +
                        "'" + objMySqlDataReader["perm_pincode"].ToString() + "'," +
                        "'" + objMySqlDataReader["temp_address1"].ToString() + "'," +
                        "'" + objMySqlDataReader["temp_address2"].ToString() + "'," +
                        "'" + objMySqlDataReader["temp_country"].ToString() + "'," +
                        "'" + objMySqlDataReader["temp_state"].ToString() + "'," +
                        "'" + objMySqlDataReader["temp_city"].ToString() + "'," +
                        "'" + objMySqlDataReader["temp_pincode"].ToString() + "'," +
                        "'" + objMySqlDataReader["designation_gid"].ToString() + "'," +
                        "'" + objMySqlDataReader["designation_name"].ToString() + "'," +
                        "'" + objMySqlDataReader["branch_gid"].ToString() + "'," +
                        "'" + objMySqlDataReader["branch_name"].ToString() + "'," +
                        "'" + objMySqlDataReader["department_gid"].ToString() + "'," +
                        "'" + objMySqlDataReader["department_name"].ToString() + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'" + objMySqlDataReader["employee_salary"].ToString() + "'," +
                        "'" + objMySqlDataReader["created_by"].ToString() + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msSQL = "Update hrm_trn_tofferletter set appointmentorder_flag='Y' where offer_gid='" + values.offer_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msUserGid = objcmnfunctions.GetMasterGID("SUSM");

                        msSQL = " insert into adm_mst_tuser(" +
                        " user_gid," +
                        " user_code," +
                        " user_firstname," +
                        " user_password, " +
                        " user_status, " +
                        " created_by, " +
                        " created_date)" +
                        " values(" +
                        " '" + msUserGid + "'," +
                        " '" + values.user_code + "'," +
                        " '" + values.first_name + "'," +
                        " '" + objcmnfunctions.ConvertToAscii(values.user_password) + "'," +
                        " '" + values.active_flag + "',";
                        msSQL += "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            msEmployeeGID = objcmnfunctions.GetMasterGID("SERM");
                            //string msBiometricGID = objcmnfunctions.GetBiometricGID();

                            msSQL = " Insert into hrm_mst_temployee " +
                                " (employee_gid , " +
                                " user_gid," +
                                " designation_gid," +
                                " employee_mobileno , " +
                                " employee_personalno , " +
                                " employee_emailid , " +
                                " employee_gender , " +
                                " employee_joiningdate , " +
                                " department_gid," +
                                " employee_photo," +
                                " useraccess," +
                                " engagement_type," +
                                " attendance_flag, " +
                                " branch_gid, " +
                                " created_by, " +
                                " created_date " +
                                " )values( " +
                                "'" + msEmployeeGID + "', " +
                                "'" + msUserGid + "', " +
                                "'" + objMySqlDataReader["designation_gid"].ToString() + "'," +
                                "'" + objMySqlDataReader["mobile_number"].ToString() + "'," +
                                "'" + objMySqlDataReader["mobile_number"].ToString() + "'," +
                                "'" + objMySqlDataReader["email_address"].ToString() + "'," +
                                "'" + objMySqlDataReader["gender"].ToString() + "'," +
                                "'" + DateTime.Parse(objMySqlDataReader["joiningdate"].ToString()).ToString("yyyy-MM-dd") + "'," +
                                "'" + objMySqlDataReader["department_gid"].ToString() + "'," +
                                "'" + null + "'," +
                                "'" + values.active_flag + "'," +
                                "'Direct'," +
                                "'Y'," +
                                "'" + objMySqlDataReader["branch_gid"].ToString() + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            msGetemployeetype = objcmnfunctions.GetMasterGID("SETD");

                            msSQL = " insert into hrm_trn_temployeetypedtl(" +
                          " employeetypedtl_gid," +
                          " employee_gid," +
                          " workertype_gid," +
                          " systemtype_gid, " +
                          " branch_gid, " +
                          " wagestype_gid, " +
                          " department_gid, " +
                          " employeetype_name, " +
                          " designation_gid, " +
                          " created_by, " +
                          " created_date)" +
                          " values(" +
                          " '" + msGetemployeetype + "'," +
                          " '" + msEmployeeGID + "'," +
                          " 'null'," +
                          " 'Audit'," +
                            "'" + objMySqlDataReader["branch_gid"].ToString() + "'," +
                          " 'wg001'," +
                            "'" + objMySqlDataReader["department_gid"].ToString() + "'," +
                          "'Roll'," +
                            "'" + objMySqlDataReader["designation_gid"].ToString() + "'," +
                           "'" + user_gid + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                            msPermanentAddressGetGID = objcmnfunctions.GetMasterGID("SADM");
                            msTemporaryAddressGetGID = objcmnfunctions.GetMasterGID("SADM");

                            msSQL = " insert into adm_mst_taddress(" +
                                    " address_gid," +
                                    " parent_gid," +
                                    " country_gid," +
                                    " address1, " +
                                    " address2, " +
                                    " city, " +
                                    " state, " +
                                    " address_type, " +
                                    " postal_code, " +
                                    " created_by, " +
                                    " created_date)" +
                                    " values(" +
                                    " '" + msPermanentAddressGetGID + "'," +
                                    " '" + msEmployeeGID + "'," +
                                    "'" + objMySqlDataReader["perm_country"].ToString() + "'," +
                                    "'" + objMySqlDataReader["perm_address1"].ToString() + "'," +
                                    "'" + objMySqlDataReader["perm_address2"].ToString() + "'," +
                                    "'" + objMySqlDataReader["perm_state"].ToString() + "'," +
                                    "'" + objMySqlDataReader["perm_city"].ToString() + "'," +
                                    " 'Permanent'," +
                                    "'" + objMySqlDataReader["perm_pincode"].ToString() + "',";
                            msSQL += "'" + user_gid + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = " insert into adm_mst_taddress(" +
                                        " address_gid," +
                                        " parent_gid," +
                                        " country_gid," +
                                        " address1, " +
                                        " address2, " +
                                        " city, " +
                                        " state, " +
                                        " address_type, " +
                                        " postal_code, " +
                                        " created_by, " +
                                        " created_date)" +
                                        " values(" +
                                        " '" + msTemporaryAddressGetGID + "'," +
                                        " '" + msEmployeeGID + "'," +
                                        "'" + objMySqlDataReader["temp_country"].ToString() + "'," +
                                        "'" + objMySqlDataReader["temp_address1"].ToString() + "'," +
                                        "'" + objMySqlDataReader["temp_address2"].ToString() + "'," +
                                        "'" + objMySqlDataReader["temp_city"].ToString() + "'," +
                                        "'" + objMySqlDataReader["temp_state"].ToString() + "'," +
                                        " 'Temporary'," +
                                        "'" + objMySqlDataReader["temp_pincode"].ToString() + "',";
                                msSQL += "'" + user_gid + "'," +
                                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            string msgetsalarygradeGID = objcmnfunctions.GetMasterGID("PSAM");

                            msSQL = " insert into pay_trn_temployee2salarygradetemplate(" +
                                    " employee2salarygradetemplate_gid ," +
                                    " salarygradetemplate_gid," +
                                    " employee_gid ," +
                                    " user_gid ," +
                                    " created_by," +
                                    " created_date," +
                                    " basic_salary, " +
                                    " gross_salary," +
                                    " ctc, " +
                                    " salary_mode," +
                                    " net_salary" +
                                    " )values(" +
                                    " '" + msgetsalarygradeGID + "'," +
                                    " '" + values.template_name + "'," +
                                    " '" + msEmployeeGID + "'," +
                                    " '" + msUserGid + "'," +
                                    " '" + user_gid + "'," +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    " '" + values.BasicSalary + "'," +
                                    " '" + values.gross_salary + "'," +
                                    " '" + values.ctc + "'," +
                                    " '" + "Gross" + "'," +
                                    " '" + values.net_salary + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (values.Summarybind_list != null)
                            {
                                foreach (var data1 in values.Summarybind_list)
                                {
                                    string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSDM");
                                    if (msGetsalrygradedtlGID == "E")
                                    {
                                        values.status = false;
                                        values.message = "Error While generating Gid";
                                        return;

                                    }
                                    msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where  component_name = '" + data1.salarycomponent_name + "'";
                                    string lssalarycomponent_gid = objdbconn.GetExecuteScalar(msSQL);

                                    msSQL = " insert into pay_trn_temployee2salarygradetemplatedtl (" +
                                                    " employee2salarygradetemplatedtl_gid," +
                                                    " employee2salarygradetemplate_gid," +
                                                    " salarygradetemplate_giddtl," +
                                                    " salarygradetype," +
                                                    " componentgroup_gid," +
                                                    " componentgroup_name," +
                                                    " salarycomponent_name," +
                                                    " created_by," +
                                                    " created_date," +
                                                    " salarycomponent_percentage, " +
                                                    " salarycomponent_amount," +
                                                    " salarycomponent_percentage_employer, " +
                                                    " salarycomponent_amount_employer," +
                                                    " salarycomponent_gid," +
                                                    " affect_in" +
                                                    ")values(" +
                                                    " '" + msGetsalrygradedtlGID + "'," +
                                                    " '" + msgetsalarygradeGID + "'," +
                                                    " '" + values.salarygradetemplate_gid + "'," +
                                                    " 'Addition'," +
                                                    " '" + data1.componentgroup_gid + "'," +
                                                    " '" + data1.componentgroup_name + "'," +
                                                    " '" + data1.salarycomponent_name + "'," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                    " '" + data1.salarycomponent_percentage + "'," +
                                                    " '" + data1.salarycomponent_amount + "'," +
                                                    " '" + data1.salarycomponent_percentage_employer + "'," +
                                                    " '" + data1.salarycomponent_amount_employer + "'," +
                                                    " '" + lssalarycomponent_gid + "'," +
                                                    " 'Basic'" + ")";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                }
                            }
                            if (values.deductionSummarybind_list != null)
                            {
                                foreach (var data2 in values.deductionSummarybind_list)
                                {
                                    if (data2.salarycomponent_name == "" || data2.salarycomponent_name == null)
                                    {
                                        msSQL = "select component_name from pay_mst_tsalarycomponent where  salarycomponent_gid = '" + data2.salarycomponent_gid + "'";
                                        lssalarycomponent_name = objdbconn.GetExecuteScalar(msSQL);
                                    }
                                    else
                                    { lssalarycomponent_name = data2.salarycomponent_name; }
                                    if (data2.salarycomponent_gid == "" || data2.salarycomponent_gid == null)
                                    {
                                        msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where  component_name = '" + data2.salarycomponent_name + "'";
                                        lssalarycomponent_gid = objdbconn.GetExecuteScalar(msSQL);
                                    }
                                    else
                                    { lssalarycomponent_gid = data2.salarycomponent_gid; }



                                    string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSDM");
                                    msSQL = " insert into pay_trn_temployee2salarygradetemplatedtl (" +
                                                " employee2salarygradetemplatedtl_gid," +
                                                " employee2salarygradetemplate_gid," +
                                                " salarygradetemplate_giddtl," +
                                                " salarygradetype," +
                                                " componentgroup_gid," +
                                                " componentgroup_name," +
                                                " salarycomponent_name," +
                                                " created_by," +
                                                " created_date," +
                                                " salarycomponent_percentage, " +
                                                " salarycomponent_amount," +
                                                " salarycomponent_percentage_employer, " +
                                                " salarycomponent_amount_employer," +
                                                " salarycomponent_gid," +
                                                " affect_in" +
                                                ")values(" +
                                                " '" + msGetsalrygradedtlGID + "'," +
                                                " '" + msgetsalarygradeGID + "'," +
                                                " '" + values.salarygradetemplate_gid + "'," +
                                                " 'Addition'," +
                                                " '" + data2.componentgroup_gid + "'," +
                                                " '" + data2.componentgroup_name + "'," +
                                                " '" + lssalarycomponent_name + "'," +
                                                " '" + user_gid + "'," +
                                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                " '" + data2.salarycomponent_percentage + "'," +
                                                " '" + data2.salarycomponent_amount + "'," +
                                                " '" + data2.salarycomponent_percentage_employer + "'," +
                                                " '" + data2.salarycomponent_amount_employer + "'," +
                                                " '" + lssalarycomponent_gid + "'," +
                                                " 'Gross' " + ")";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }

                            if (values.othersSummarybind_list != null)
                            {
                                foreach (var data3 in values.othersSummarybind_list)
                                {
                                    string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSDM");
                                    if (msGetsalrygradedtlGID == "E")
                                    {
                                        values.status = false;
                                        values.message = "Error While generating Gid";
                                        return;

                                    }
                                    msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where  component_name = '" + data3.salarycomponent_name + "'";
                                    string lssalarycomponent_gid = objdbconn.GetExecuteScalar(msSQL);

                                    msSQL = " insert into pay_trn_temployee2salarygradetemplatedtl (" +
                                                    " employee2salarygradetemplatedtl_gid," +
                                                    " employee2salarygradetemplate_gid," +
                                                    " salarygradetemplate_giddtl," +
                                                    " salarygradetype," +
                                                    " componentgroup_gid," +
                                                    " componentgroup_name," +
                                                    " salarycomponent_name," +
                                                    " created_by," +
                                                    " created_date," +
                                                    " salarycomponent_percentage, " +
                                                    " salarycomponent_amount," +
                                                    " salarycomponent_percentage_employer, " +
                                                    " salarycomponent_amount_employer," +
                                                    " salarycomponent_gid," +
                                                    " affect_in" +
                                                    ")values(" +
                                                    " '" + msGetsalrygradedtlGID + "'," +
                                                    " '" + msgetsalarygradeGID + "'," +
                                                    " '" + values.salarygradetemplate_gid + "'," +
                                                    " 'Others'," +
                                                    " '" + data3.componentgroup_gid + "'," +
                                                    " '" + data3.componentgroup_name + "'," +
                                                    " '" + data3.salarycomponent_name + "'," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                    " '" + data3.salarycomponent_percentage + "'," +
                                                    " '" + data3.salarycomponent_amount + "'," +
                                                    " '" + data3.salarycomponent_percentage_employer + "'," +
                                                    " '" + data3.salarycomponent_amount_employer + "'," +
                                                    " '" + lssalarycomponent_gid + "'," +
                                                    " 'Basic'" + ")";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }
                        }
                        objMySqlDataReader.Close();
                    }
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Employee confirmed successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occured while confirming employee";
                    }
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public AddOfferletter_list DaGetconfirmationdetails(string offer_gid)
        {
            try
            {
                AddOfferletter_list objAddOfferletter_list = new AddOfferletter_list();
                {
                    msSQL = " SELECT a.offer_gid, concat(a.first_name,' ',a.last_name) as first_name, a.gender, a.dob, a.mobile_number, a.email_address, a.qualification,a.offerletter_type, " +
                            " a.experience_detail, a.perm_address_gid, a.temp_address_gid, a.template_gid, a.created_by, a.created_date,a.employee_gid, " +
                            " a.branch_name,a.designation_name,a.offer_date,a.department_name " +
                            " FROM hrm_trn_tofferletter a " +
                            " left join hrm_trn_temployeetypedtl j on a.employee_gid=j.employee_gid " +
                            " where a.offer_gid ='" + offer_gid + "' ";
                }

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    objAddOfferletter_list.first_name = objMySqlDataReader["first_name"].ToString();
                    objAddOfferletter_list.department_name = objMySqlDataReader["department_name"].ToString();
                    objAddOfferletter_list.designation_name = objMySqlDataReader["designation_name"].ToString();
                    objAddOfferletter_list.branch_name = objMySqlDataReader["branch_name"].ToString();


                    objMySqlDataReader.Close();
                }
                return objAddOfferletter_list;
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }

        public void DaGetgradetemplatedropdown(MdlOfferLetter values)
        {
            try
            {

                msSQL = " select salarygradetemplate_gid,salarygradetemplate_name  " +
                    " from pay_mst_tsalarygradetemplate ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getgradetemplatedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getgradetemplatedropdown
                        {
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            salarygradetemplate_name = dt["salarygradetemplate_name"].ToString(),
                        });
                        values.Getgradetemplatedropdown = getModuleList;
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

        public void DaAdditionsummary(string salarygradetemplate_gid, string salarygradetype, MdlOfferLetter values)
        {
            try
            {

                msSQL = " select salarygradetemplatedtl_gid, salarygradetemplate_gid, salarygradetype,salarycomponent_gid, " +
                " salarycomponent_name,componentgroup_gid, componentgroup_name,salarycomponent_amount, affect_in,salarycomponent_percentage,salarycomponent_percentage_employer, " +
                " case when salarycomponent_amount_employer is null then 0.00 else salarycomponent_amount_employer end as salarycomponent_amount_employer " +
                " from pay_mst_tsalarygradetemplatedtl " +
                " where salarygradetemplate_gid='" + salarygradetemplate_gid + "' and salarygradetype ='" + salarygradetype + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<SummaryAddition_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new SummaryAddition_list
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            employee_contribution = dt["salarycomponent_amount"].ToString(),
                            affect_in = dt["affect_in"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            employer_contribution = dt["salarycomponent_amount_employer"].ToString(),

                        });
                        values.SummaryAddition_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Addiction summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaDeductionsummary(string salarygradetemplate_gid, string salarygradetype, MdlOfferLetter values)
        {
            try
            {

                msSQL = " select salarygradetemplatedtl_gid, salarygradetemplate_gid, salarygradetype,salarycomponent_gid, " +
                " salarycomponent_name,componentgroup_gid, componentgroup_name,salarycomponent_amount, affect_in,salarycomponent_percentage,salarycomponent_percentage_employer, " +
                " case when salarycomponent_amount_employer is null then 0.00 else salarycomponent_amount_employer end as salarycomponent_amount_employer " +
                " from pay_mst_tsalarygradetemplatedtl " +
                " where salarygradetemplate_gid='" + salarygradetemplate_gid + "' and salarygradetype = '" + salarygradetype + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Deduction_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Deduction_list
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            demployee_contribution = dt["salarycomponent_amount"].ToString(),
                            affect_in = dt["affect_in"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            demployer_contribution = dt["salarycomponent_amount_employer"].ToString(),

                        });
                        values.Deduction_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding deduction summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaOtherssummary(string salarygradetemplate_gid, string gross_salary, MdlOfferLetter values)
        {
            double additioncomponentamount = 0.00;
            double additioncmpamt = 0.00;
            double basicsalary = 0.00;
            double employeerbasicsalary = 0.00;
            double othercomponentadd = 0.00;
            double othercomponentemployeradd = 0.00;
            double othercomponentded = 0.00;
            double othercomponentemployerded = 0.00;
            string lscomponentname1 = "";
            double lscomponent_percentage = 0.00;
            double lscomponent_amount = 0.00;
            double lsemployer_percentage = 0.00;
            double lsemployer_amount = 0.00;
            string lscomponent_flag = "";
            string lsemployercomponent_flag1 = "";
            string lsaffect_in = "";
            string lscomponentgroup_name = "";
            string lssalarycomponent_name = "";
            string lsemployercomponent_flag = "";
            Double lsgross_salary = Convert.ToDouble(gross_salary);
            Double lslocal = 0.00;
            Double lslocal_employer = 0.00;



            try
            {
                msSQL = " select a.affecting_in,a.component_flag,a.componentgroup_gid,a.componentgroup_name,a.component_percentage,a.component_amount,a.component_flag_employer,a.component_name," +
                " a.component_percentage_employer,a.component_amount_employer,b.group_belongsto from pay_mst_tsalarycomponent a" +
                " left join pay_mst_tcomponentgroupmaster b on a.componentgroup_gid= b.componentgroup_gid " +
                " left join pay_mst_tsalarygradetemplatedtl c on c.salarycomponent_gid= a.salarycomponent_gid " +
                " where a.component_name like '%basic%' and b.group_belongsto='Gross' and c.salarygradetemplate_gid = '" + salarygradetemplate_gid + "'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    lscomponentname1 = objMySqlDataReader["component_name"].ToString();
                    lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                    lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                    lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                    lsemployercomponent_flag1 = objMySqlDataReader["component_flag_employer"].ToString();
                    lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                    lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                }


                if (lscomponent_flag == "Y")
                {
                    lsbasicsalary = (lsgross_salary) * (lscomponent_percentage / 100);
                }
                else
                {
                    lsbasicsalary = lscomponent_amount;
                }

                if (lsemployercomponent_flag1 == "Y")
                {
                    lsbasic_salary_employeer = (lsgross_salary) * (lsemployer_percentage / 100);
                }
                else
                {
                    lsbasic_salary_employeer = lsemployer_amount;
                }



                msSQL = " select salarycomponent_gid, componentgroup_gid ,componentgroup_name,salarycomponent_gid,othercomponent_type from pay_mst_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplate_gid + "' and salarygradetype='Others'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<othersSummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                        " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsaffect_in = objMySqlDataReader["affecting_in"].ToString();
                            lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                            lscomponentgroup_name = objMySqlDataReader["componentgroup_name"].ToString();
                            lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                            lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                            lssalarycomponent_name = objMySqlDataReader["component_name"].ToString();
                            lsemployercomponent_flag = objMySqlDataReader["component_flag_employer"].ToString();
                            lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                            lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                        }


                        if (lssalarycomponent_name == "Basic salary")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal = lsemployer_amount;
                            }

                        }

                        if (lsaffect_in == "Basic")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }

                        if (dt["othercomponent_type"].ToString() == "Addition")
                        {
                            othercomponentadd += lslocal;
                            othercomponentemployeradd += lslocal_employer;

                        }


                        else if (dt["othercomponent_type"].ToString() == "Deduction")
                        {

                            othercomponentded += lslocal;
                            othercomponentemployerded += lslocal_employer;
                        }

                        values.netsalary += (othercomponentadd + othercomponentemployeradd) - (othercomponentded + othercomponentemployerded);
                        values.ctc += (othercomponentadd + othercomponentemployeradd) + (othercomponentded + othercomponentemployerded);





                        getModuleList.Add(new othersSummarybind_list
                        {
                            //salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = salarygradetemplate_gid,
                            componentgroup_name = lscomponentgroup_name,
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = lssalarycomponent_name,
                            salarycomponent_percentage = lscomponent_percentage,
                            salarycomponent_amount = Math.Round(lslocal, 2),
                            salarycomponent_percentage_employer = lsemployer_percentage,
                            salarycomponent_amount_employer = Math.Round(lslocal_employer, 2),
                            basicsalay = lsbasicsalary



                        });
                        values.othersSummarybind_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetAdditionalcomponentname(string componentgroup_name, MdlOfferLetter values)
        {
            try
            {

                msSQL = "select component_name,salarycomponent_gid from pay_mst_tsalarycomponent where componentgroup_name='" + componentgroup_name + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAdditionalcomponentname>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAdditionalcomponentname
                        {
                            salarycomponent_name = dt["component_name"].ToString(),
                            component_name = dt["component_name"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                        });
                        values.GetAdditionalcomponentname = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetdeductioncomponentname(string componentgroup_name, MdlOfferLetter values)
        {
            try
            {

                msSQL = "select component_name,salarycomponent_gid from pay_mst_tsalarycomponent where componentgroup_name='" + componentgroup_name + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getdeductioncomponentname>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdeductioncomponentname
                        {
                            salarycomponent_name = dt["component_name"].ToString(),
                            component_name = dt["component_name"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                        });
                        values.Getdeductioncomponentname = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaAdditionsummarybind(string salarygradetemplategid, string gross_salary, MdlOfferLetter values)
        {
            double additioncomponentamount = 0.00;
            double additioncmpamt = 0.00;
            double basicsalary = 0.00;
            double employeerbasicsalary = 0.00;
            string lscomponentname1 = "";
            double lscomponent_percentage = 0.00;
            double lscomponent_amount = 0.00;
            double lsemployer_percentage = 0.00;
            double lsemployer_amount = 0.00;
            string lscomponent_flag = "";
            string lsemployercomponent_flag1 = "";
            string lsaffect_in = "";
            string lscomponentgroup_name = "";
            string lssalarycomponent_name = "";
            string lsemployercomponent_flag = "";
            Double lsgross_salary = Convert.ToDouble(gross_salary);
            Double lslocal = 0.00;
            Double lslocal_employer = 0.00;

            string lscomponentname1_cust = "";
            double lscomponent_percentage_cust = 0;
            double lscomponent_amount_cust = 0;
            double lsemployer_percentage_coust = 0;
            string lscomponent_flag_cust = "";
            string lsemployercomponent_flag1_cust = "";
            double lsemployer_amount_cust = 0;

            string lscomponentname1_splf = "";
            double lscomponent_percentage_splf = 0;
            double lscomponent_amount_splf = 0;
            double lsemployer_percentage_splf = 0;
            string lscomponent_flag_splf = "";
            string lsemployercomponent_flag1_splf = "";
            double lsemployer_amount_custsplf = 0;

            double othercomponentadd = 0.00;
            double othercomponentemployeradd = 0.00;
            double othercomponentded = 0.00;
            double othercomponentemployerded = 0.00;


            try
            {
                msSQL = " select a.affecting_in,a.component_flag,a.componentgroup_gid,a.componentgroup_name,a.component_percentage,a.component_amount,a.component_flag_employer,a.component_name," +
                " a.component_percentage_employer,a.component_amount_employer,b.group_belongsto from pay_mst_tsalarycomponent a" +
                " left join pay_mst_tcomponentgroupmaster b on a.componentgroup_gid= b.componentgroup_gid " +
                " left join pay_mst_tsalarygradetemplatedtl c on c.salarycomponent_gid= a.salarycomponent_gid " +
                " where a.component_name like '%basic%' and b.group_belongsto='Gross' and c.salarygradetemplate_gid = '" + salarygradetemplategid + "'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    lscomponentname1 = objMySqlDataReader["component_name"].ToString();
                    lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                    lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                    lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                    lsemployercomponent_flag1 = objMySqlDataReader["component_flag_employer"].ToString();
                    lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                    lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                }

                if (lscomponent_flag == "Y")
                {
                    lsbasicsalary = (lsgross_salary) * (lscomponent_percentage / 100);
                }
                else
                {
                    lsbasicsalary = lscomponent_amount;
                }

                if (lsemployercomponent_flag1 == "Y")
                {
                    lsbasic_salary_employeer = (lsgross_salary) * (lsemployer_percentage / 100);
                }
                else
                {
                    lsbasic_salary_employeer = lsemployer_amount;
                }



                msSQL = " select salarycomponent_gid, componentgroup_gid ,componentgroup_name,salarycomponent_gid from pay_mst_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Addition'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Summarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                        " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsaffect_in = objMySqlDataReader["affecting_in"].ToString();
                            lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                            lscomponentgroup_name = objMySqlDataReader["componentgroup_name"].ToString();
                            lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                            lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                            lssalarycomponent_name = objMySqlDataReader["component_name"].ToString();
                            lsemployercomponent_flag = objMySqlDataReader["component_flag_employer"].ToString();
                            lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                            lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                        }


                        if (lsaffect_in == "Basic")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Gross")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Customize")
                        {
                            string lscustomize_component = "";
                            double lscustomize_employee = 0;
                            double lscustomize_employer = 0;
                            string[] lscustomizelist = null;
                            msSQL = "select customize_component from pay_mst_tsalarycomponent " +
                                   " where salarycomponent_gid = '" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lscustomize_component = objMySqlDataReader2["customize_component"].ToString();
                                lscustomizelist = lscustomize_component.Split(',');
                            }
                            for (int j = 0; j < lscustomizelist.Length; j++)
                            {
                                string lsaffect_in_cust = "", lssalarycomponent_name_cust = "", lsemployercomponent_flag_cust = "";
                                double lsemployer_percentage_cust = 0;
                                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                         " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lscustomizelist[j].ToString() + "'";

                                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader2.HasRows == true)
                                {
                                    lsaffect_in_cust = objMySqlDataReader2["affecting_in"].ToString();
                                    lscomponent_flag_cust = objMySqlDataReader2["component_flag"].ToString();
                                    //lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                    lscomponent_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                    lscomponent_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                    lssalarycomponent_name_cust = objMySqlDataReader2["component_name"].ToString();
                                    lsemployercomponent_flag_cust = objMySqlDataReader2["component_flag_employer"].ToString();
                                    lsemployer_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                    lsemployer_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                }


                                if (lsaffect_in_cust == "Basic")
                                {
                                    if (lscomponent_flag_cust == "Y")
                                    {
                                        lslocal = (lsbasicsalary) * (lscomponent_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_cust;
                                    }

                                    if (lsemployercomponent_flag_cust == "Y")
                                    {
                                        lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_cust;
                                    }

                                }
                                if (lsaffect_in_cust == "Gross")
                                {
                                    if (lscomponent_flag_cust == "Y")
                                    {
                                        lslocal = (lsgross_salary) * (lscomponent_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_cust;
                                    }

                                    if (lsemployercomponent_flag_cust == "Y")
                                    {
                                        lslocal_employer = (lsgross_salary) * (lsemployer_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_cust;
                                    }

                                }
                                lscustomize_employee += lslocal;
                                lscustomize_employer += lsemployer_amount;

                            }

                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lscustomize_employee) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lscustomize_employer) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Formula")
                        {
                            string lssource_variale = "", lsformula_operator = "", lsformula_variable = "";
                            string[] lsvariableslist = null;
                            double lsformula_employee = 0;
                            double lsformula_employer = 0;
                            msSQL = "select source_variale,formula_operator,formula_variable from pay_mst_tsalarycomponent" +
                                    " where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lssource_variale = objMySqlDataReader2["source_variale"].ToString();
                                lsformula_operator = objMySqlDataReader2["formula_operator"].ToString();
                                lsformula_variable = objMySqlDataReader2["formula_variable"].ToString();
                            }
                            lsvariableslist = lsformula_variable.Split(',');
                            for (int l = 0; l < lsvariableslist.Length; l++)
                            {
                                string lsaffect_in_splf = "", lssalarycomponent_name_splf = "", lsemployercomponent_flag_splf = "";
                                double lsemployer_amount_splf = 0;
                                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,othercomponent_type,component_amount,component_flag_employer,component_name," +
                                         " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lsvariableslist[l].ToString() + "'";

                                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader2.HasRows == true)
                                {
                                    lsaffect_in_splf = objMySqlDataReader2["affecting_in"].ToString();
                                    lscomponent_flag_splf = objMySqlDataReader2["component_flag"].ToString();
                                    lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                    lscomponent_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                    lscomponent_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                    lssalarycomponent_name_splf = objMySqlDataReader2["component_name"].ToString();
                                    lsemployercomponent_flag_splf = objMySqlDataReader2["component_flag_employer"].ToString();
                                    lsemployer_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                    lsemployer_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                }


                                if (lsaffect_in_splf == "Basic")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsbasicsalary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                if (lsaffect_in_splf == "Gross")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsgross_salary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsgross_salary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                lsformula_employee += lslocal;
                                lsformula_employer += lslocal_employer;

                            }

                            if (lssource_variale == "Gross")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsgross_salary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsgross_salary - lsformula_employee;
                                }
                            }
                            else if (lssource_variale == "Basic")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsbasicsalary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsbasicsalary - lsformula_employee;
                                }
                            }
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsformula_employee) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lsformula_employee;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsformula_employer) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsformula_employer;
                            }
                        }




                        getModuleList.Add(new Summarybind_list
                        {
                            //salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = salarygradetemplategid,
                            componentgroup_name = lscomponentgroup_name,
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = lssalarycomponent_name,
                            salarycomponent_percentage = lscomponent_percentage,
                            salarycomponent_amount = Math.Round(lslocal, 2),
                            salarycomponent_percentage_employer = lsemployer_percentage,
                            salarycomponent_amount_employer = Math.Round(lslocal_employer, 2),
                            basicsalay = lsbasicsalary



                        });
                        values.Summarybind_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void Dadeductionsummarybind(string salarygradetemplategid, string gross_salary, MdlOfferLetter values)
        {
            string lsothercomponttype = "";

            double employerdeductioncmpamt = 0.00;
            double deductioncmpamt = 0.00;
            double othercomponentadd = 0.00;
            double othercomponentemployeradd = 0.00;
            double othercomponentded = 0.00;
            double othercomponentemployerded = 0.00;
            double additioncomponentamount = 0.00;
            double additioncmpamt = 0.00;
            double basicsalary = 0.00;
            double netsalary = 0.00;
            double ctc = 0.00;
            double employeerbasicsalary = 0.00;
            string lscomponentname1 = "";
            double lscomponent_percentage = 0.00;
            double lscomponent_amount = 0.00;
            double lsemployer_percentage = 0.00;
            double lsemployer_amount = 0.00;
            string lscomponent_flag = "";
            string lsemployercomponent_flag1 = "";
            string lsaffect_in = "";
            string lscomponentgroup_name = "";
            string lssalarycomponent_name = "";
            string lsemployercomponent_flag = "";
            Double lsgross_salary = Convert.ToDouble(gross_salary);
            Double lslocal = 0.00;
            Double lslocal_employer = 0.00;

            string lscomponentname1_cust = "";
            double lscomponent_percentage_cust = 0;
            double lscomponent_amount_cust = 0;
            double lsemployer_percentage_coust = 0;
            string lscomponent_flag_cust = "";
            string lsemployercomponent_flag1_cust = "";
            double lsemployer_amount_cust = 0;

            try
            {
                msSQL = " select a.affecting_in,a.component_flag,a.componentgroup_gid,a.componentgroup_name,a.component_percentage,a.component_amount,a.component_flag_employer,a.component_name," +
                " a.component_percentage_employer,a.component_amount_employer,b.group_belongsto from pay_mst_tsalarycomponent a" +
                " left join pay_mst_tcomponentgroupmaster b on a.componentgroup_gid= b.componentgroup_gid " +
                " left join pay_mst_tsalarygradetemplatedtl c on c.salarycomponent_gid= a.salarycomponent_gid " +
                " where a.component_name like '%basic%' and b.group_belongsto='Gross' and c.salarygradetemplate_gid = '" + salarygradetemplategid + "'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    lscomponentname1 = objMySqlDataReader["component_name"].ToString();
                    lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                    lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                    lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                    lsemployercomponent_flag1 = objMySqlDataReader["component_flag_employer"].ToString();
                    lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                    lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                }

                if (lscomponent_flag == "Y")
                {
                    lsbasicsalary = (lsgross_salary) * (lscomponent_percentage / 100);
                }
                else
                {
                    lsbasicsalary = lscomponent_amount;
                }

                if (lsemployercomponent_flag1 == "Y")
                {
                    lsbasic_salary_employeer = (lsgross_salary) * (lsemployer_percentage / 100);
                }
                else
                {
                    lsbasic_salary_employeer = lsemployer_amount;
                }



                msSQL = " select salarycomponent_gid,componentgroup_gid,salarycomponent_name,salarycomponent_gid,componentgroup_name from pay_mst_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Deduction'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<deductionSummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsaffect_in = objMySqlDataReader["affecting_in"].ToString();
                            lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                            lscomponentgroup_name = objMySqlDataReader["componentgroup_name"].ToString();
                            lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                            lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                            lssalarycomponent_name = objMySqlDataReader["component_name"].ToString();
                            lsemployercomponent_flag = objMySqlDataReader["component_flag_employer"].ToString();
                            lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                            lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());

                        }




                        if (lsaffect_in == "Basic")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag == "Y")
                            {
                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }

                        if (lsaffect_in == "Gross")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag == "Y")
                            {
                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }

                        if (lssalarycomponent_name.Contains("ESI"))
                        {


                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag == "Y")
                            {
                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                            if (lsgross_salary >= 21000)
                            {
                                lslocal = 0;
                                lslocal_employer = 0;

                            }
                        }

                        if (lssalarycomponent_name.Contains("PF") || lssalarycomponent_name.Contains("EPF"))
                        {

                            if (lscomponent_flag == "Y")

                            {
                                if (lsbasicsalary >= 15000)
                                {
                                    lslocal = 1800;
                                }
                                else
                                {
                                    lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                                }
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }


                            if (lsemployercomponent_flag == "Y")
                            {
                                if (lsbasicsalary >= 15000)
                                {
                                    lslocal_employer = 1800;
                                }
                                else
                                {
                                    lslocal_employer = (lsbasicsalary) * (lscomponent_percentage / 100);
                                }
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }

                        if (lsaffect_in == "Customize")
                        {
                            string lscustomize_component = "";
                            double lscustomize_employee = 0;
                            double lscustomize_employer = 0;
                            string[] lscustomizelist = null;
                            msSQL = "select customize_component from pay_mst_tsalarycomponent " +
                                   " where salarycomponent_gid = '" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lscustomize_component = objMySqlDataReader2["customize_component"].ToString();
                                lscustomizelist = lscustomize_component.Split(',');
                            }
                            for (int j = 0; j < lscustomizelist.Length; j++)
                            {
                                string lsaffect_in_cust = "", lssalarycomponent_name_cust = "", lsemployercomponent_flag_cust = "";
                                double lsemployer_percentage_cust = 0;
                                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                         " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lscustomizelist[j].ToString() + "'";

                                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader2.HasRows == true)
                                {
                                    lsaffect_in_cust = objMySqlDataReader2["affecting_in"].ToString();
                                    lscomponent_flag_cust = objMySqlDataReader2["component_flag"].ToString();
                                    //lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                    lscomponent_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                    lscomponent_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                    lssalarycomponent_name_cust = objMySqlDataReader2["component_name"].ToString();
                                    lsemployercomponent_flag_cust = objMySqlDataReader2["component_flag_employer"].ToString();
                                    lsemployer_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                    lsemployer_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                }


                                if (lsaffect_in_cust == "Basic")
                                {
                                    if (lscomponent_flag_cust == "Y")
                                    {
                                        lslocal = (lsbasicsalary) * (lscomponent_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_cust;
                                    }

                                    if (lsemployercomponent_flag_cust == "Y")
                                    {
                                        lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_cust;
                                    }

                                }
                                if (lsaffect_in_cust == "Gross")
                                {
                                    if (lscomponent_flag_cust == "Y")
                                    {
                                        lslocal = (lsgross_salary) * (lscomponent_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_cust;
                                    }

                                    if (lsemployercomponent_flag_cust == "Y")
                                    {
                                        lslocal_employer = (lsgross_salary) * (lsemployer_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_cust;
                                    }

                                }
                                lscustomize_employee += lslocal;
                                lscustomize_employer += lsemployer_amount;

                            }

                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lscustomize_employee) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lscustomize_employer) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }
                            if (lssalarycomponent_name.Contains("PF") || lssalarycomponent_name.Contains("EPF"))
                            {

                                if (lscomponent_flag == "Y")

                                {
                                    if (lscustomize_employee >= 15000)
                                    {
                                        lslocal = 1800;
                                    }
                                    else
                                    {
                                        lslocal = (lscustomize_employee) * (lscomponent_percentage / 100);
                                    }
                                }
                                else
                                {
                                    lslocal = lscomponent_amount;
                                }


                                if (lsemployercomponent_flag == "Y")
                                {
                                    if (lscustomize_employer >= 15000)
                                    {
                                        lslocal_employer = 1800;
                                    }
                                    else
                                    {
                                        lslocal_employer = (lscustomize_employer) * (lscomponent_percentage / 100);
                                    }
                                }
                                else
                                {
                                    lslocal_employer = lsemployer_amount;
                                }

                            }

                        }
                        if (lsaffect_in == "Formula")
                        {
                            string lssource_variale = "", lsformula_operator = "", lsformula_variable = "";
                            string[] lsvariableslist = null;
                            double lsformula_employee = 0;
                            double lsformula_employer = 0;
                            msSQL = "select source_variale,formula_operator,formula_variable from pay_mst_tsalarycomponent" +
                                    " where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lssource_variale = objMySqlDataReader2["source_variale"].ToString();
                                lsformula_operator = objMySqlDataReader2["formula_operator"].ToString();
                                lsformula_variable = objMySqlDataReader2["formula_variable"].ToString();
                            }
                            lsvariableslist = lsformula_variable.Split(',');
                            for (int l = 0; l < lsvariableslist.Length; l++)
                            {
                                string lsaffect_in_splf = "", lssalarycomponent_name_splf = "", lsemployercomponent_flag_splf = "";
                                double lsemployer_amount_splf = 0, lsemployer_percentage_splf = 0, lscomponent_percentage_splf = 0, lscomponent_amount_splf = 0;
                                string lscomponent_flag_splf = "";
                                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                         " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lsvariableslist[l].ToString() + "'";

                                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader2.HasRows == true)
                                {
                                    lsaffect_in_splf = objMySqlDataReader2["affecting_in"].ToString();
                                    lscomponent_flag_splf = objMySqlDataReader2["component_flag"].ToString();
                                    //lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                    lscomponent_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                    lscomponent_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                    lssalarycomponent_name_splf = objMySqlDataReader2["component_name"].ToString();
                                    lsemployercomponent_flag_splf = objMySqlDataReader2["component_flag_employer"].ToString();
                                    lsemployer_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                    lsemployer_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                }


                                if (lsaffect_in_splf == "Basic")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsbasicsalary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                if (lsaffect_in_splf == "Gross")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsgross_salary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsgross_salary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                lsformula_employee += lslocal;
                                lsformula_employer += lslocal_employer;

                            }

                            if (lssource_variale == "Gross")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsgross_salary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsgross_salary - lsformula_employee;
                                }
                            }
                            else if (lssource_variale == "Basic")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsbasicsalary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsbasicsalary - lsformula_employee;
                                }
                            }
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsformula_employee) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lsformula_employee;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsformula_employer) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsformula_employer;
                            }
                        }


                        deductioncmpamt += lslocal;
                        employerdeductioncmpamt += lslocal_employer;

                        values.netsalary = Math.Round(lsgross_salary - deductioncmpamt);
                        values.ctc = Math.Round(lsgross_salary + deductioncmpamt + employerdeductioncmpamt);

                        getModuleList.Add(new deductionSummarybind_list
                        {
                            //salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = salarygradetemplategid,
                            componentgroup_name = lscomponentgroup_name,
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_name = lssalarycomponent_name,
                            salarycomponent_percentage = lscomponent_percentage,
                            salarycomponent_amount = Math.Round(lslocal, 2),
                            salarycomponent_percentage_employer = lsemployer_percentage,
                            salarycomponent_amount_employer = Math.Round(lslocal_employer, 2),
                            net_salary = Math.Round(netsalary),
                            ctc = Math.Round(ctc),


                        });
                        values.deductionSummarybind_list = getModuleList;

                    }

                }
                msSQL = " select a.affecting_in,a.component_flag,a.componentgroup_gid,a.componentgroup_name,a.component_percentage,a.component_amount,a.component_flag_employer,a.component_name," +
                " a.component_percentage_employer,a.component_amount_employer,b.group_belongsto from pay_mst_tsalarycomponent a" +
                " left join pay_mst_tcomponentgroupmaster b on a.componentgroup_gid= b.componentgroup_gid " +
                " left join pay_mst_tsalarygradetemplatedtl c on c.salarycomponent_gid= a.salarycomponent_gid " +
                " where a.component_name like '%basic%' and b.group_belongsto='Gross' and c.salarygradetemplate_gid = '" + salarygradetemplategid + "'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    lscomponentname1 = objMySqlDataReader["component_name"].ToString();
                    lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                    lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                    lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                    lsemployercomponent_flag1 = objMySqlDataReader["component_flag_employer"].ToString();
                    lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                    lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                }


                if (lscomponent_flag == "Y")
                {
                    lsbasicsalary = (lsgross_salary) * (lscomponent_percentage / 100);
                }
                else
                {
                    lsbasicsalary = lscomponent_amount;
                }

                if (lsemployercomponent_flag1 == "Y")
                {
                    lsbasic_salary_employeer = (lsgross_salary) * (lsemployer_percentage / 100);
                }
                else
                {
                    lsbasic_salary_employeer = lsemployer_amount;
                }
                msSQL = " select salarycomponent_gid, componentgroup_gid ,componentgroup_name,salarycomponent_gid,othercomponent_type from pay_mst_tsalarygradetemplatedtl " +
                       " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Others'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList1 = new List<othersSummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name,othercomponent_type," +
                        " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsaffect_in = objMySqlDataReader["affecting_in"].ToString();
                            lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                            lscomponentgroup_name = objMySqlDataReader["componentgroup_name"].ToString();
                            lsothercomponttype = objMySqlDataReader["othercomponent_type"].ToString();
                            lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                            lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                            lssalarycomponent_name = objMySqlDataReader["component_name"].ToString();
                            lsemployercomponent_flag = objMySqlDataReader["component_flag_employer"].ToString();
                            lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                            lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                        }


                        if (lssalarycomponent_name == "Basic salary")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal = lsemployer_amount;
                            }

                        }

                        if (lsaffect_in == "Basic")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Gross")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = double.Parse(gross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = double.Parse(gross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }

                        if (dt["othercomponent_type"].ToString() == "Addition")
                        {
                            othercomponentadd += lslocal;
                            othercomponentemployeradd += lslocal_employer;

                        }


                        else if (dt["othercomponent_type"].ToString() == "Deduction")
                        {

                            othercomponentded += lslocal;
                            othercomponentemployerded += lslocal_employer;
                        }
                        if (lsothercomponttype == "Addition")
                        {
                            values.overallnetsalary = values.netsalary + lslocal + lslocal_employer;

                        }
                        else if (lsothercomponttype == "Deduction")
                        {
                            values.overallnetsalary = values.netsalary - (lslocal + lslocal_employer);
                        }
                        else
                        {
                            values.overallnetsalary = values.netsalary;
                        }





                        getModuleList1.Add(new othersSummarybind_list
                        {
                            //salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = salarygradetemplategid,
                            componentgroup_name = lscomponentgroup_name,
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = lssalarycomponent_name,
                            salarycomponent_percentage = lscomponent_percentage,
                            salarycomponent_amount = Math.Round(lslocal, 2),
                            salarycomponent_percentage_employer = lsemployer_percentage,
                            salarycomponent_amount_employer = Math.Round(lslocal_employer, 2),
                            basicsalay = lsbasicsalary



                        });
                        values.othersSummarybind_list = getModuleList1;
                    }
                }
                if (values.othersSummarybind_list == null)
                {
                    values.overallnetsalary = values.netsalary;

                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DagetEmployeelist(MdlOfferLetter values)
        {
            try
            {
                msSQL = " select a.employee_gid,concat(b.user_code,' / ',b.user_firstname,' ',b.user_lastname) as emp_name from hrm_mst_temployee a" +
                " left join adm_mst_tuser b on a.user_gid=b.user_gid" +
                " where employee_gid  in (select employee_gid from hrm_trn_tofferletter) group by a.employee_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getemployeebind>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getemployeebind
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            emp_name = dt["emp_name"].ToString(),
                            

                        });
                        values.Getemployeebind = getModuleList;
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
        public void DaGetEditEmployeebind(string employee_gid, MdlOfferLetter values)
        {
            try
            {

                msSQL = " select  a.employee_gid,b.user_firstname,b.user_lastname,a.employee_gender,date_format(a.employee_joiningdate,'%Y-%m-%d') as employee_joiningdate" +
                  ",a.employee_mobileno,date_format(a.employee_dob,'%Y-%m-%d') as employee_dob,a.department_gid,a.designation_gid,a.branch_gid," +
                  " a.employee_emailid,a.employee_qualification,a.employee_experience,c.address1,c.address2,c.city,c.state,c.postal_code," +
                  " c.country_gid,a.designation_gid,d.designation_name,a.department_gid,e.department_name,f.branch_name from hrm_mst_temployee a"+
                  " left join adm_mst_tuser b on a.user_gid=b.user_gid" +
                  " left join adm_mst_taddress c on c.parent_gid=a.employee_gid" +
                  " left join adm_mst_tdesignation d on a.designation_gid=d.designation_gid" +
                  " left join hrm_mst_tdepartment e on a.department_gid=e.department_gid" +
                  " left join hrm_mst_tbranch f on a.branch_gid=f.branch_gid" +
                  " where employee_gid in (select employee_gid from hrm_trn_tofferletter)" +
                  " and a.employee_gid='" + employee_gid + "' group by a.employee_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<getexistemployee>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new getexistemployee
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            user_lastname = dt["user_lastname"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            employee_dob = dt["employee_dob"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            employee_emailid = dt["employee_emailid"].ToString(),
                            employee_qualification = dt["employee_qualification"].ToString(),
                            employee_experience = dt["employee_experience"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            postal_code = dt["postal_code"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(), 
                        });
                        values.getexistemployee = getModuleList;
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


        public Dictionary<string, object> GetOfferletterpdf(string offer_gid, MdlOfferLetter values,string branch_gid)
        {


            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();


            msSQL = " select a.offertemplate_content,date_format(a.offer_date,'%d-%m-%Y') as offer_date," +
                                    " a.designation_name,date_format(a.created_date,'%d-%m-%Y') as created_date" +
                                    " from  hrm_trn_tofferletter a" +
                                   " where a.offer_gid='" + offer_gid + "'";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");



            msSQL = "SELECT a.branch_logo_path as company_logo " +
            " FROM hrm_mst_tbranch a  where a.branch_gid='"+branch_gid+"'";
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
            //string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ems.hrm", "hrm_rpt_offerletter.rpt");
            //oRpt.Load(reportPath);
            //oRpt.SetDataSource(myDS);
            //string path = Path.Combine(ConfigurationManager.AppSettings["report_file_path_hrm"].ToString(), "Offerletter" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            //oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            //myConnection.Close();
            //var ls_response = objFnazurestorage.reportStreamDownload(path);
            //File.Delete(path);
            //return ls_response;
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_hrm"].ToString(), "hrm_rpt_offerletter.rpt"));


            oRpt.SetDataSource(myDS);

            string reportPath = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "OfferLetter" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");

            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, reportPath);

            myConnection.Close();

            var ls_response = objFnazurestorage.reportStreamDownload(reportPath);

            File.Delete(reportPath);

            return ls_response;
        }


    }
}