using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.utilities.Functions;
using StoryboardAPI.Models;
using System.Data;
using System.Data.Odbc;
using Newtonsoft.Json;
using RestSharp;
using System.Web.UI.WebControls;
using System.IO;
using MimeKit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Cryptography;
using System.Net.Mail;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Diagnostics;


namespace StoryboardAPI.Controllers
{
    [RoutePrefix("api/Login")]
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        dbconn objdbconn = new dbconn();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        OdbcDataReader objMySqlDataReader, odbcDataReader, objOdbcDataReader, objODBCdatareader;
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string dashboard_flag = string.Empty;
        string msSQL = string.Empty;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, mnResult6;
        string user_status;
        string vendoruser_status;
        string tokenvalue = string.Empty;
        string user_gid = string.Empty;
        string employee_gid = string.Empty;
        string department_gid = string.Empty;
        string password = string.Empty;
        string username = string.Empty;
        string departmentname = string.Empty;
        string lscompany_code, lssource_db;
        string lscompany_dbname;
        string domain = string.Empty;
        string lsexpiry_time, lsfreetrailexpiry_time;
        DataTable dt_datatable, dt_datatable1, objDataTable;
        string lsuser_password, lsuser_code, lsemployee_emailid, lsuser_gid, lscompanyid, lscontact_id, lsusercode, msGetGid, msGetGid1, lsdefault_screen, msGetGid2, msGetGid3, msGetGid4;
        string productgroup_gid, product_name, customercontact_name, customer_gid, customercontact_gid, address1, body, email, mobile, currency_code, currencyexchange_gid, customer_name, productuom_gid, productuom_name, product_code, productgroup_name;
        string ls_server, customer_code, ls_password, ls_username, company_code, lsSQLpath, Scripts, GetFilesinPath, inserpath, employeename;
        string[] createtables;
        int ls_port;

        [HttpPost]
        [ActionName("UserLogin")]
        public HttpResponseMessage PostUserLogin(PostUserLogin values)
        {
            loginresponse objloginresponse = new loginresponse();
            try
            {
                domain = Request.RequestUri.Host.ToLower();
                var host = HttpContext.Current.Request.Url.Host;
                if (!String.IsNullOrEmpty(values.company_code))
                {
                    var ObjToken = Token(values.user_code, objcmnfunctions.ConvertToAscii(values.user_password), values.company_code.ToLower());
                    dynamic newobj = JsonConvert.DeserializeObject(ObjToken);
                    if (newobj.access_token != null)
                    {
                       
                            tokenvalue = "Bearer " + newobj.access_token;
                        msSQL = "call adm_mst_spstoretoken('" + tokenvalue + "','" + values.user_code + "','" + objcmnfunctions.ConvertToAscii(values.user_password) + "','" + values.company_code + "')";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows)
                        {
                            objloginresponse.token = tokenvalue;
                            objloginresponse.user_gid = objMySqlDataReader["user_gid"].ToString();
                            objloginresponse.dashboard_flag = objMySqlDataReader["dashboard_flag"].ToString();

                            msSQL = "select end_date from vcxcontroller.adm_mst_tconsumer where company_code='" + values.company_code + "' and subscription_details = 'Free'";
                            objODBCdatareader = objdbconn.GetDataReader(msSQL);
                            if (objODBCdatareader.HasRows == true)
                            {
                                lsfreetrailexpiry_time = objODBCdatareader["end_date"].ToString();

                                if (!String.IsNullOrEmpty(lsfreetrailexpiry_time))
                                {
                                    DateTime freetrailexpiry_time = DateTime.Parse(lsfreetrailexpiry_time);
                                    DateTime now = DateTime.Now;

                                    // Calculate the difference
                                    TimeSpan difference = now - freetrailexpiry_time;

                                    // Get the total number of days
                                    int daysDifference = difference.Days;

                                    if (daysDifference > 0)
                                    {
                                        objloginresponse.freetrail_flag = "Y";
                                        objloginresponse.status = false;
                                        return Request.CreateResponse(HttpStatusCode.OK, objloginresponse);
                                    }
                                }
                            }
                        
                            objloginresponse.freetrail_flag = "N";
                            objloginresponse.c_code = values.company_code;
                            objloginresponse.message = "Login Successful!";
                            objloginresponse.status = true;
                            msSQL = "select a.default_screen from " + objloginresponse.c_code + ".hrm_mst_temployee a left join " + objloginresponse.c_code + ".adm_mst_tuser b " +
                               "  on a.user_gid= b.user_gid where b.user_gid ='" + objloginresponse.user_gid + "'";
                            lsdefault_screen = objdbconn.GetExecuteScalar(msSQL);
                            if (lsdefault_screen != "")
                            {
                                msSQL = " select sref,k_sref from " + objloginresponse.c_code + ".adm_mst_tmoduleangular where module_gid " +
                                    "in (select default_screen from " + objloginresponse.c_code + ".hrm_mst_temployee a left join " + objloginresponse.c_code + ".adm_mst_tuser b" +
                                    " on b.user_gid = a.user_gid where b.user_gid ='" + objloginresponse.user_gid + "')";
                                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                if (objMySqlDataReader.HasRows)
                                {
                                    objloginresponse.sref = objMySqlDataReader["sref"].ToString();
                                    objloginresponse.k_sref = objMySqlDataReader["k_sref"].ToString();
                                }
                            }

                          
                        }
                        else
                        {
                            objloginresponse.message = "Invalid Credentials!";
                        }
                    }
                    else
                    {
                        objloginresponse.message = "Invalid Credentials!";
                    }
                }
                else
                {
                    objloginresponse.message = "Company Code cannot be empty!";
                }
            }
            catch (Exception ex)
            {
                objloginresponse.message = "Exception occured while loggin in!";
            }
            finally
            {
                if (objMySqlDataReader != null)
                    objMySqlDataReader.Close();
            }
            return Request.CreateResponse(HttpStatusCode.OK, objloginresponse);
        }
        [HttpPost]
        [ActionName("VcxControllerUserLogin")]
        public HttpResponseMessage PostVcxControllerUserLogin(PostUserLogin values)
        {
            loginresponse objloginresponse = new loginresponse();
            try
            {
                domain = Request.RequestUri.Host.ToLower();
                var host = HttpContext.Current.Request.Url.Host;
                if (!String.IsNullOrEmpty(values.company_code))
                {
                    msSQL = " select user_gid from vcxcontroller.adm_mst_tuser " +
                        " where user_code='" + values.user_code + "'";
                    user_gid = objdbconn.GetExecuteScalar(msSQL);
                    
                                Random rnd = new Random();
                                values.otp_value = (rnd.Next(100000, 999999)).ToString();

                                msSQL = "update vcxcontroller.hrm_mst_temployee set otp_value='" + values.otp_value + "',otpexpiry_date= '" + DateTime.Now.AddSeconds(60).ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                        " where  user_gid= '" + user_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "select employee_gid,employee_emailid from vcxcontroller.hrm_mst_temployee where user_gid = '" + user_gid + "'";
                                objODBCdatareader = objdbconn.GetDataReader(msSQL);
                                if (objODBCdatareader.HasRows == true)
                                {
                                    values.employee_gid = objODBCdatareader["employee_gid"].ToString();
                                    values.employee_emailid = objODBCdatareader["employee_emailid"].ToString();
                                }

                                msSQL = " INSERT INTO vcxcontroller.adm_mst_totplogin ( " +
                                        " otp_value, " +
                                        " employee_gid, " +
                                        " user_gid," +
                                        " employee_emailid," +
                                        " created_time," +
                                        " expiry_time" +
                                        " )VALUES( " +
                                        " '" + values.otp_value + "'," +
                                        " '" + values.employee_gid + "'," +
                                        " '" + user_gid + "'," +
                                        " '" + values.employee_emailid + "'," +
                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " '" + DateTime.Now.AddSeconds(60).ToString("yyyy-MM-dd HH:mm:ss") + "'" + ")";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                objODBCdatareader.Close();

                                msSQL = "select concat(user_firstname,'',user_lastname) from vcxcontroller.adm_mst_tuser a " +
                                                "left join vcxcontroller.hrm_mst_temployee b on a.user_gid = b.user_gid " +
                                                "where b.employee_gid ='" + values.employee_gid + "'";
                                employeename = objdbconn.GetExecuteScalar(msSQL);


                                string Body = "<p>Dear " + employeename + ",</p>" +
                                              "<p>Please use the verification code below to sign in.</p>" +
                                              "<table style=\"font-family: Arial, sans-serif; border-collapse: collapse; width: 50%;\">" +
                                              "<tr><td style=\"padding: 10px; border: 1px solid #ccc;\"><b>" + values.otp_value + "</b></td></tr>" +
                                              "</table>" +
                                              "<p>If you didn’t request this, you can ignore this email.</p>" +
                                              "<p>Thanks,</p>" +
                                              "<p>Best regards,<br> Vcidex Support Team</p>";


                    // Send email
                    bool status = mail(values.company_code, values.employee_emailid, "Verification code", Body);
                                if (status)
                                {
                                    values.status = true;
                                    values.message = "The OTP has been sent to your email!";
                                    values.status = true;
                                    values.user_gid = user_gid;

                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Failed to send the email. Please try again!";

                                }                   
                }
                else
                {
                    values.message = "Company Code cannot be empty!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loggin in!";
            }
            finally
            {
                if (objMySqlDataReader != null)
                    objMySqlDataReader.Close();
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // OTPLogin verification
        [AllowAnonymous]
        [ActionName("otpverifyvcxcontroller")]
        [HttpPost]
        public HttpResponseMessage GetUserReturnValue(otpverify values)
        {
            otpverifyresponse GetLoginResponse = new otpverifyresponse();
            try
            {
                var username = string.Empty;

                msSQL = "SELECT otpexpiry_date FROM vcxcontroller.hrm_mst_temployee where otp_value ='" + values.otp_value + "'";
                lsexpiry_time = objdbconn.GetExecuteScalar(msSQL);

                DateTime expiry_time = DateTime.Parse(lsexpiry_time);

                DateTime now = DateTime.Now;

                if (expiry_time > now)
                {
                    msSQL = "SELECT user_gid FROM vcxcontroller.adm_mst_totplogin where otp_value ='" + values.otp_value + "'";
                    //msSQL = " SELECT b.user_gid, a.employee_gid, a.otpvalue, b.employee_mobileno FROM adm_mst_totplogin a " +
                    //        " INNER JOIN hrm_mst_temployee b on b.employee_mobileno = a.employee_mobileno " +
                    //        " WHERE otpvalues = '" + values.otpvalue + "'";
                    //msSQL = " SELECT b.user_gid,a.department_gid, a.employee_gid, user_password, user_code, concat(user_firstname, ' ', user_lastname) as username FROM hrm_mst_temployee a " +
                    //        " INNER JOIN adm_mst_tuser b on b.user_gid = a.user_gid " +
                    //        " WHERE otpvalue = '" + values.otpvalue + "'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        objMySqlDataReader.Read();
                        var user_gid = objMySqlDataReader["user_gid"].ToString();
                        msSQL = "select user_code, user_password from vcxcontroller.adm_mst_tuser where user_gid = '" + user_gid + "'";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            values.user_code = objMySqlDataReader["user_code"].ToString();
                            values.user_password = objMySqlDataReader["user_password"].ToString();
                            values.company_code = "vcxcontroller";

                            if (!String.IsNullOrEmpty(values.company_code))
                            {
                                var ObjToken = Token(values.user_code, values.user_password, values.company_code.ToLower());
                                dynamic newobj = JsonConvert.DeserializeObject(ObjToken);
                                if (newobj.access_token != null)
                                {
                                    tokenvalue = "Bearer " + newobj.access_token;
                                    msSQL = "call adm_mst_spstoretoken('" + tokenvalue + "','" + values.user_code + "','" + values.user_password + "','" + values.company_code + "')";
                                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                                    if (objMySqlDataReader.HasRows)
                                    {
                                        GetLoginResponse.token = tokenvalue;
                                        GetLoginResponse.user_gid = objMySqlDataReader["user_gid"].ToString();
                                        GetLoginResponse.dashboard_flag = objMySqlDataReader["dashboard_flag"].ToString();
                                        GetLoginResponse.c_code = values.company_code;
                                        GetLoginResponse.message = "Login Successful!";
                                        GetLoginResponse.status = true;
                                        msSQL = "select a.default_screen from " + GetLoginResponse.c_code + ".hrm_mst_temployee a left join " + GetLoginResponse.c_code + ".adm_mst_tuser b " +
                                           "  on a.user_gid= b.user_gid where b.user_gid ='" + GetLoginResponse.user_gid + "'";
                                        lsdefault_screen = objdbconn.GetExecuteScalar(msSQL);
                                        if (lsdefault_screen != "")
                                        {
                                            msSQL = " select sref,k_sref from " + GetLoginResponse.c_code + ".adm_mst_tmodule where module_gid " +
                                                "in (select default_screen from " + GetLoginResponse.c_code + ".hrm_mst_temployee a left join " + GetLoginResponse.c_code + ".adm_mst_tuser b" +
                                                " on b.user_gid = a.user_gid where b.user_gid ='" + GetLoginResponse.user_gid + "')";
                                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                            if (objMySqlDataReader.HasRows)
                                            {
                                                GetLoginResponse.sref = objMySqlDataReader["sref"].ToString();
                                                GetLoginResponse.k_sref = objMySqlDataReader["k_sref"].ToString();
                                            }
                                        }
                                        bool status = objcmnfunctions.PortalManagementMailtrigger(values.user_code);

                                    }
                                    else
                                    {
                                        GetLoginResponse.message = "Invalid Credentials!";
                                    }
                                }
                                else
                                {
                                    GetLoginResponse.message = "Invalid Credentials!";
                                }
                            }
                            else
                            {
                                GetLoginResponse.message = "Company Code cannot be empty!";
                            }
                        }



                    }
                    objMySqlDataReader.Close();
                }

                else
                {
                    GetLoginResponse.status = false;
                    GetLoginResponse.message = "Login time has been expired. kindly click the blade resend OTP ";

                }

            }
            catch (Exception ex)
            {
                GetLoginResponse.status = false;
                GetLoginResponse.message = "Invalid mail ID. Kindly contact your administrator";
            }
            finally
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK, GetLoginResponse);
        }


        [HttpPost]
        [ActionName("UserForgot")]
        public HttpResponseMessage PostUserForgot(PostUserForgot values)
        {
            PostUserForgot GetForgotResponse = new PostUserForgot();
            try
            {
                // Check if companyid and usercode are provided
                if (string.IsNullOrEmpty(values.companyid) || string.IsNullOrEmpty(values.usercode))
                {
                    GetForgotResponse.message = "Company ID and User Code are mandatory.";
                }
                else
                {
                    string lsuser_code = null;
                    string lsuser_password = null;
                    string lsemployee_emailid = null;
                    string lscompany_code = null;
                    string lscompanyid = null;
                    string lsusercode = null;
                    string lsuser_gid = null;
                    string luser_name = null;


                    // Fetch user details
                    string msSQL = "SELECT user_code, LENGTH(user_password) AS length,concat(user_firstname,' ',user_lastname)as name,user_password, user_gid FROM " + values.companyid + ".adm_mst_tuser WHERE user_code = '" + values.usercode + "'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows)
                    {
                        lsuser_code = objMySqlDataReader["user_code"].ToString();
                        lsuser_password = objMySqlDataReader["user_password"].ToString();
                        lsuser_gid = objMySqlDataReader["user_gid"].ToString();
                        luser_name = objMySqlDataReader["name"].ToString();

                        string length = objMySqlDataReader["length"].ToString();
                        int originallength = Convert.ToInt32(length);
                        string password = ReverseAscii(lsuser_password, originallength);


                        // Fetch employee email
                        msSQL = "SELECT employee_emailid FROM " + values.companyid + ".hrm_mst_temployee WHERE user_gid = '" + lsuser_gid + "'";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows)
                        {
                            lsemployee_emailid = objMySqlDataReader["employee_emailid"].ToString();
                        }

                        // Fetch company code
                        msSQL = "SELECT company_code FROM " + values.companyid + ".adm_mst_tcompany";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows)
                        {
                            lscompany_code = objMySqlDataReader["company_code"].ToString();
                        }

                        // Validate and format strings
                        lsuser_code = string.IsNullOrEmpty(lsuser_code) ? null : lsuser_code.ToUpper();
                        lscompany_code = string.IsNullOrEmpty(lscompany_code) ? null : lscompany_code.ToUpper();
                        lscompanyid = string.IsNullOrEmpty(values.companyid) ? null : values.companyid.ToUpper();
                        lsusercode = string.IsNullOrEmpty(values.usercode) ? null : values.usercode.ToUpper();

                        // Check conditions
                        if (lscompany_code == lscompanyid)
                        {
                            if (lsuser_code == lsusercode)
                            {

                                if (!string.IsNullOrEmpty(lsemployee_emailid))
                                {
                                    string Body = "<p>Dear " + luser_name + ",</p>" +
                                                  "<p>We have received a request to recover your username and password. Please find your details below:</p>" +
                                                  "<table style=\"font-family: Arial, sans-serif; border-collapse: collapse; width: 50%;\">" +
                                                  "<tr><td><b>User Name:</b></td><td>" + lsuser_code + "</td></tr>" +
                                                  "<tr><td><b>Password:</b></td><td>" + password + "</td></tr>" +
                                                  "</table>" +
                                                  "<p><b>Please note : Kindly keep these credentials confidential and do not share them with others.</b></p>" +
                                                  "<p>If you did not initiate this recovery request, please contact our support team immediately at  <a href=\"mailto:support@vcidex.com\">support@vcidex.com.</a></p>" +
                                                  "<p>Thank you for your attention to this matter.</p>" +
                                                  "<p>Best regards,<br> Vcidex Support Team</p>";

                                    // Send email
                                    bool status = mail(values.companyid, lsemployee_emailid, "Your User Code and Password Recovery", Body);
                                    if (status)
                                    {
                                        GetForgotResponse.status = true;
                                        GetForgotResponse.message = "Email containing your user code and password has been successfully dispatched.";
                                    }
                                    else
                                    {
                                        GetForgotResponse.status = false;
                                        GetForgotResponse.message = "Failed to send the email. Please try again.";
                                    }
                                }
                                else
                                {
                                    GetForgotResponse.message = "Employee email ID not found.";
                                }
                            }
                        }
                        else
                        {
                            GetForgotResponse.message = "User code does not match.";
                        }
                    }
                    else
                    {
                        GetForgotResponse.message = "Company code does not match.";
                    }
                }
            }
            catch (Exception ex)
            {
                GetForgotResponse.message = "Error: " + ex.Message;
                // Log the exception for debugging purposes
            }

            return Request.CreateResponse(HttpStatusCode.OK, GetForgotResponse);
        }
        [HttpPost]
        [ActionName("submitforgot")]
        public HttpResponseMessage submitforgot(PostUserForgot values)
        {
            PostUserForgot GetRestResponse = new PostUserForgot();
            domain = Request.RequestUri.Host.ToLower();
            msSQL = " SELECT  user_code,user_password,user_gid from " + values.companyid + ".adm_mst_tuser    where user_code = '" + values.usercode + "' ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows)
            {
                lsuser_code = objMySqlDataReader["user_code"].ToString();
                lsuser_password = objMySqlDataReader["user_password"].ToString();
                lsuser_gid = objMySqlDataReader["user_gid"].ToString();
            }

            if (lsuser_code != null && lsuser_code != "")
            {
                lsuser_code = lsuser_code.ToUpper();
            }
            else
            {
                lsuser_code = null;

            }
            msSQL = " select   employee_emailid FROM " + values.companyid + ".hrm_mst_temployee     where user_gid = '" + lsuser_gid + "' ";

            objMySqlDataReader = objdbconn.GetDataReader(msSQL);

            if (objMySqlDataReader.HasRows)

            {

                lsemployee_emailid = objMySqlDataReader["employee_emailid"].ToString();

            }

            msSQL = " SELECT  company_code FROM " + values.companyid + ".adm_mst_tcompany ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows)
            {
                lscompany_code = objMySqlDataReader["company_code"].ToString();

            }
            if (lscompany_code != null && lscompany_code != "")
            {
                lscompany_code = lscompany_code.ToUpper();
            }
            else
            {
                lscompany_code = null;

            }
            if (values.companyid != null && values.companyid != "")
            {
                lscompanyid = values.companyid.ToUpper();
            }
            else
            {
                lscompanyid = null;

            }
            if (values.usercode != null && values.usercode != "")
            {
                lsusercode = values.usercode.ToUpper();
            }
            else
            {
                lsusercode = null;

            }

            if (values.forgot_pwd == null)
            {
                values.forgot_pwd = "";
            }

            if (lscompany_code == lscompanyid)
            {
                if (lsuser_code == lsusercode)
                {

                    if (lsemployee_emailid == values.forgotportal_emailid)
                    {
                        msSQL = " update " + values.companyid + ".adm_mst_tuser set " +

                                 " user_password = '" + objcmnfunctions.ConvertToAscii(values.forgot_pwd) + "'," +

                                 " updated_by = '" + lsuser_gid + "'," +

                                 " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' where user_gid='" + lsuser_gid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQLForgot(msSQL);
                        if (mnResult == 1)
                        {
                            GetRestResponse.status = true;
                            GetRestResponse.message = "Forgot Password Updated Successfully !";
                            return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);
                        }
                        else
                        {
                            GetRestResponse.status = false;
                            GetRestResponse.message = "Error Occur While Forgot Password !";
                            return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);
                        }

                    }
                    else
                    {

                        GetRestResponse.status = false;
                        GetRestResponse.message = "Email-ID is Invaild !";
                        return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);
                    }
                }
                else
                {
                    GetRestResponse.status = false;
                    GetRestResponse.message = "User code is Invaild !";
                    return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);

                }
            }
            else
            {

                GetRestResponse.status = false;
                GetRestResponse.message = "Company code is Invaild !";
                return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);
            }

        }
        [HttpPost]
        [ActionName("forgormailtrigger")]
        public HttpResponseMessage forgormailtrigger(PostUserForgot values)
        {
            PostUserForgot GetForgotResponse = new PostUserForgot();
            try
            {

                string lsuser_code = null;
                string lsuser_password = null;
                string lsemployee_emailid = null;
                string lscompany_code = null;
                string lscompanyid = null;
                string lsusercode = null;
                string lsuser_gid = null;
                string luser_name = null;

                string msSQL = "SELECT user_code, LENGTH(user_password) AS length,concat(user_firstname,' ',user_lastname)as name,user_password, user_gid FROM " + values.companyid + ".adm_mst_tuser WHERE user_code = '" + values.usercode + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lsuser_code = objMySqlDataReader["user_code"].ToString();
                    lsuser_gid = objMySqlDataReader["user_gid"].ToString();
                    luser_name = objMySqlDataReader["name"].ToString();
                }


                msSQL = "SELECT employee_emailid FROM " + values.companyid + ".hrm_mst_temployee WHERE user_gid = '" + lsuser_gid + "'";

                lsemployee_emailid = objdbconn.GetExecuteScalar(msSQL);

                Random random = new Random();
                int randomNumber = random.Next(100000, 1000000);

                msSQL = "update " + values.companyid + ".adm_mst_tuser set forgot_code=" + randomNumber + " where user_gid='" + lsuser_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                string Body = "<p>Dear " + luser_name + ",</p>" +
              "<p>Please use the verification code below to sign in.</p>" +
              "<table style=\"font-family: Arial, sans-serif; border-collapse: collapse; width: 50%;\">" +
              "<tr><td style=\"padding: 10px; border: 1px solid #ccc;\"><b>" + randomNumber + "</b></td></tr>" +
              "</table>" +
              "<p>If you didn’t request this, you can ignore this email.</p>" +
              "<p>Thanks,</p>" +
              "<p>Best regards,<br> Vcidex Support Team</p>";


                // Send email
                bool status = mail(values.companyid, lsemployee_emailid, "Verification code", Body);



                if (status)
                {
                    GetForgotResponse.status = true;
                    GetForgotResponse.message = "The OTP has been sent to your email!";

                }
                else
                {
                    GetForgotResponse.status = false;
                    GetForgotResponse.message = "Failed to send the email. Please try again!";

                }


            }
            catch (Exception ex)
            {
                GetForgotResponse.message = "Error: " + ex.Message;
                // Log the exception for debugging purposes
            }

            return Request.CreateResponse(HttpStatusCode.OK, GetForgotResponse);
        }
        public string ReverseAscii(string encodedStr, int originalLength)
        {
            string reversedWords = string.Empty;
            int lstemp = encodedStr.Length / 3;
            int j = 0;
            for (int i = 0; i < lstemp; i++) // Process each group of three characters.
            {
                string numberStr = encodedStr.Substring(j, 3);
                int number = int.Parse(numberStr);
                char character = (char)(number + lstemp);
                reversedWords += character;
                j = j + 3;
            }

            return reversedWords;
        }
        public bool mail(string companycode, string to, string sub, string body)
        {
            try
            {
                msSQL = "SELECT company_mail,pop_server,pop_port,pop_username,pop_password FROM " + companycode + ".adm_mst_tcompany ";
                odbcDataReader = objdbconn.GetDataReader(msSQL);
                if (odbcDataReader.HasRows == true)
                {
                    ls_server = odbcDataReader["pop_server"].ToString();
                    ls_port = Convert.ToInt32(odbcDataReader["pop_port"]);
                    ls_username = odbcDataReader["pop_username"].ToString();
                    ls_password = odbcDataReader["pop_password"].ToString();
                }
                odbcDataReader.Close();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(ls_username);
                message.To.Add(new MailAddress(to));
                message.Subject = sub;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = body;
                smtp.Port = ls_port;
                smtp.Host = ls_server; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(ls_username, ls_password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpPost]
        [ActionName("UserReset")]
        public HttpResponseMessage PostUserReset(PostUserReset values)
        {
            PostUserReset GetRestResponse = new PostUserReset();
            domain = Request.RequestUri.Host.ToLower();
            //string jsonFilePath = @" " + ConfigurationManager.AppSettings["CmnConfigfile_path"].ToString();
            //string jsonString = File.ReadAllText(jsonFilePath);
            //var jsonDataArray = JsonConvert.DeserializeObject<MdlCmnConn[]>(jsonString);
            //string lscompany_dbname = (from a in jsonDataArray
            //                           where a.company_code == values.companyid_reset
            //                           select a.company_dbname).FirstOrDefault();
            //string lscompany_code = (from a in jsonDataArray
            //                         where a.company_code == values.companyid_reset
            //                         select a.company_code).FirstOrDefault();
            //if (lscompany_code != null && lscompany_code != " ")
            //{
            msSQL = " SELECT  user_code,user_password,user_gid from " + values.companyid_reset + ".adm_mst_tuser    where user_code = '" + values.usercode_reset + "' ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows)
            {
                lsuser_code = objMySqlDataReader["user_code"].ToString();
                lsuser_password = objMySqlDataReader["user_password"].ToString();
                lsuser_gid = objMySqlDataReader["user_gid"].ToString();
            }

            if (lsuser_code != null && lsuser_code != "")
            {
                lsuser_code = lsuser_code.ToUpper();
            }
            else
            {
                lsuser_code = null;

            }

            msSQL = " SELECT  company_code FROM " + values.companyid_reset + ".adm_mst_tcompany ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows)
            {
                lscompany_code = objMySqlDataReader["company_code"].ToString();

            }
            if (lscompany_code != null && lscompany_code != "")
            {
                lscompany_code = lscompany_code.ToUpper();
            }
            else
            {
                lscompany_code = null;

            }
            if (values.companyid_reset != null && values.companyid_reset != "")
            {
                lscompanyid = values.companyid_reset.ToUpper();
            }
            else
            {
                lscompanyid = null;

            }
            if (values.usercode_reset != null && values.usercode_reset != "")
            {
                lsusercode = values.usercode_reset.ToUpper();
            }
            else
            {
                lsusercode = null;

            }

            if (lscompany_code == lscompanyid)
            {
                if (lsuser_code == lsusercode)
                {

                    if (lsuser_password == objcmnfunctions.ConvertToAscii(values.old_password))
                    {
                        msSQL = " update " + values.companyid_reset + ".adm_mst_tuser set " +
                            " user_password = '" + objcmnfunctions.ConvertToAscii(values.password) + "'," +
                            " updated_by = '" + lsuser_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' where user_gid='" + lsuser_gid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQLForgot(msSQL);
                        if (mnResult == 1)
                        {
                            GetRestResponse.status = true;
                            GetRestResponse.message = "Password Reset Successfully !";
                            return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);
                        }
                        else
                        {
                            GetRestResponse.status = false;
                            GetRestResponse.message = "Error Occur While Password Reset !";
                            return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);
                        }

                    }
                    else
                    {

                        GetRestResponse.status = false;
                        GetRestResponse.message = "Old Paaword is Invaild !";
                        return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);
                    }
                }
                else
                {
                    GetRestResponse.status = false;
                    GetRestResponse.message = "User code is Invaild !";
                    return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);

                }
            }
            else
            {

                GetRestResponse.status = false;
                GetRestResponse.message = "Company code is Invaild !";
                return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);
            }

            //}
            //else
            //{

            //    GetRestResponse.status = false;
            //    GetRestResponse.message = "Company code is Invaild !";
            //    return Request.CreateResponse(HttpStatusCode.OK, GetRestResponse);
            //}


        }
        public class MdlCmnConn
        {
            public string connection_string { get; set; }
            public string company_code { get; set; }
            public string company_dbname { get; set; }
        }

        // ------------- For SSO Login & OTP Validation ------------------
        [AllowAnonymous]
        [ActionName("LoginReturn")]
        [HttpPost]
        public HttpResponseMessage GetLoginReturn(logininput values)
        {
            var url = ConfigurationManager.AppSettings["host"];
            if (url == ConfigurationManager.AppSettings["livedomain_url"].ToString())
            {
                var getSpireDocLicense = ConfigurationManager.AppSettings["SpireDocLicenseKey"];
                Spire.License.LicenseProvider.SetLicenseKey(getSpireDocLicense);
            }

            loginresponse GetLoginResponse = new loginresponse();
            string code = values.code;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestSharp.RestClient("https://login.microsoftonline.com/655a0e0e-4a74-4a0c-86d8-370a992e90a6/oauth2/v2.0/token");
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("client_id", ConfigurationManager.AppSettings["client_id"]);
            request.AddParameter("code", code);
            request.AddParameter("scope", "https://graph.microsoft.com/User.Read");
            request.AddParameter("client_secret", ConfigurationManager.AppSettings["client_secret"]);
            request.AddParameter("redirect_uri", ConfigurationManager.AppSettings["redirect_url"]);
            request.AddParameter("grant_type", "authorization_code");
            IRestResponse response = client.Execute(request);
            token json = JsonConvert.DeserializeObject<token>(response.Content);

            var client1 = new RestSharp.RestClient("https://graph.microsoft.com/v1.0/me");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("Authorization", "Bearer " + json.access_token);
            IRestResponse response1 = client1.Execute(request1);
            Rootobject json1 = JsonConvert.DeserializeObject<Rootobject>(response1.Content);
            object lsDBmobilePhone;

            if (json1.userPrincipalName != null && json1.userPrincipalName != "")
            {
                msSQL = " SELECT b.user_gid,a.department_gid, a.employee_gid, user_password, user_code, a.employee_mobileno, concat(user_firstname, ' ', user_lastname) as username FROM hrm_mst_temployee a " +
                        " INNER JOIN adm_mst_tuser b on b.user_gid = a.user_gid " +
                        " WHERE employee_emailid = '" + json1.userPrincipalName + "' and b.user_status = 'Y'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {

                    objMySqlDataReader.Read();
                    var tokenresponse = Token(objMySqlDataReader["user_code"].ToString(), objMySqlDataReader["user_password"].ToString());
                    dynamic newobj = Newtonsoft.Json.JsonConvert.DeserializeObject(tokenresponse);
                    tokenvalue = newobj.access_token;
                    employee_gid = objMySqlDataReader["employee_gid"].ToString();
                    user_gid = objMySqlDataReader["user_gid"].ToString();
                    department_gid = objMySqlDataReader["department_gid"].ToString();
                    GetLoginResponse.username = objMySqlDataReader["username"].ToString();
                    lsDBmobilePhone = objMySqlDataReader["employee_mobileno"].ToString();
                    objMySqlDataReader.Close();
                }
                else
                    objMySqlDataReader.Close();

                msSQL = " INSERT INTO adm_mst_ttoken ( " +
                         " token, " +
                         " employee_gid, " +
                         " user_gid, " +
                         " department_gid, " +
                         " company_code " +
                         " )VALUES( " +
                         " 'Bearer " + tokenvalue + "'," +
                         " '" + employee_gid + "'," +
                         " '" + user_gid + "'," +
                         " '" + department_gid + "'," +
                         " '" + ConfigurationManager.AppSettings["company_code"] + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                GetLoginResponse.status = true;
                GetLoginResponse.message = "";
                GetLoginResponse.token = "Bearer " + tokenvalue;
                GetLoginResponse.user_gid = user_gid;

            }
            else
            {
                GetLoginResponse.user_gid = null;
            }
            return Request.CreateResponse(HttpStatusCode.OK, GetLoginResponse);
        }


        //OTP LOGIN
        [AllowAnonymous]
        [ActionName("OTPlogin")]
        [HttpPost]
        public HttpResponseMessage GetUserotpReturn(otplogin values)

        {

            try
            {

                msSQL = " SELECT * FROM hrm_mst_temployee ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                List<string> employeeemailid_List = new List<string>();

                employeeemailid_List = dt_datatable.AsEnumerable().Select(p => p.Field<string>("employee_emailid")).ToList();

                if (employeeemailid_List.Contains(values.employee_emailid))
                {
                    var username = string.Empty;
                    //string *randomNumber*/;
                    Random rnd = new Random();
                    values.otpvalue = (rnd.Next(100000, 999999)).ToString();

                    //msSQL = " SELECT * FROM hrm_mst_temployee a " +
                    //        " INNER JOIN adm_mst_tuser b on b.user_gid = a.user_gid " +
                    //        " WHERE employee_emailid = '" + values.emailid + "'";
                    //msSQL= "SELECT employee_gid,user_gid,employee_emailid,employee_mobileno, FROM hrm_mst_temployee where employee_emailid ='" + values.employee_emailid + "'";
                    msSQL = "SELECT * FROM hrm_mst_temployee where employee_emailid ='" + values.employee_emailid + "'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        objMySqlDataReader.Read();
                        employee_gid = objMySqlDataReader["employee_gid"].ToString();
                        user_gid = objMySqlDataReader["user_gid"].ToString();
                        //values.employee_mobileno = objMySqlDataReader["employee_mobileno"].ToString();
                        //values.created_time= objMySqlDataReader["created_time"].ToString();
                        //values.expiry_time = objMySqlDataReader["expiry_time"].ToString();
                        values.employee_mobileno = objMySqlDataReader["employee_mobileno"].ToString();
                        values.employee_emailid = objMySqlDataReader["employee_emailid"].ToString();
                        string requestUri = ConfigurationManager.AppSettings["smspushapi_url"].ToString();
                        var client = new RestClient(requestUri);
                        var request = new RestRequest(Method.GET);
                        request.AddParameter("appid", ConfigurationManager.AppSettings["smspushapi_appid"].ToString());
                        request.AddParameter("userId", ConfigurationManager.AppSettings["smspushapi_userid"].ToString());
                        request.AddParameter("pass", ConfigurationManager.AppSettings["smspushapi_password"].ToString());
                        request.AddParameter("contenttype", "3");
                        request.AddParameter("from", ConfigurationManager.AppSettings["smspushapi_from"].ToString());
                        request.AddParameter("selfid", "true");
                        request.AddParameter("alert", "1");
                        request.AddParameter("dlrreq", "true");
                        request.AddParameter("intflag", "false");

                        request.AddParameter("to", values.employee_mobileno);
                        request.AddParameter("text", "Use Verification code " + values.otpvalue + " for One.Samunnati portal authentication.\nTEAM SAMUNNATI");

                        IRestResponse response = client.Execute(request);


                        objMySqlDataReader.Close();
                    }
                    msSQL = " INSERT INTO adm_mst_totplogin ( " +
                             " otpvalue, " +
                             " employee_gid, " +
                             " user_gid," +
                             " employee_mobileno," +
                             " created_time," +
                             " expiry_time" +
                             " )VALUES( " +
                             " '" + values.otpvalue + "'," +
                             " '" + employee_gid + "'," +
                             " '" + user_gid + "'," +
                             " '" + values.employee_mobileno + "'," +
                           " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                          " '" + DateTime.Now.AddSeconds(60).ToString("yyyy-MM-dd HH:mm:ss") + "'" + ")";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "OTP sent successfully to your registered mobile number ending with" + " " + values.employee_mobileno.Substring(values.employee_mobileno.Length - 4) + "... ";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occurred while sending the OTP to your registered mobile number";

                    }

                }

                else
                {
                    values.status = false;
                    values.message = "Invalid email id";

                }




            }
            catch (Exception ex)
            {
                values.status = false;

                values.message = ex.ToString();
            }
            finally
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // OTPLogin verification
        [AllowAnonymous]
        [ActionName("otpverify")]
        [HttpPost]
        public HttpResponseMessage GetUserReturn(otpverify values)
        {
            otpverifyresponse GetLoginResponse = new otpverifyresponse();
            try
            {
                var username = string.Empty;

                msSQL = "SELECT expiry_time FROM adm_mst_totplogin where otpvalue ='" + values.otpvalue + "'";
                lsexpiry_time = objdbconn.GetExecuteScalar(msSQL);



                DateTime expiry_time = DateTime.Parse(lsexpiry_time);

                DateTime now = DateTime.Now;




                if (expiry_time > now)
                {
                    msSQL = "SELECT user_gid FROM adm_mst_totplogin where otpvalue ='" + values.otpvalue + "'";
                    //msSQL = " SELECT b.user_gid, a.employee_gid, a.otpvalue, b.employee_mobileno FROM adm_mst_totplogin a " +
                    //        " INNER JOIN hrm_mst_temployee b on b.employee_mobileno = a.employee_mobileno " +
                    //        " WHERE otpvalues = '" + values.otpvalue + "'";
                    //msSQL = " SELECT b.user_gid,a.department_gid, a.employee_gid, user_password, user_code, concat(user_firstname, ' ', user_lastname) as username FROM hrm_mst_temployee a " +
                    //        " INNER JOIN adm_mst_tuser b on b.user_gid = a.user_gid " +
                    //        " WHERE otpvalue = '" + values.otpvalue + "'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        objMySqlDataReader.Read();
                        var user_gid = objMySqlDataReader["user_gid"].ToString();
                        msSQL = "select user_code, user_password from adm_mst_tuser where user_gid = '" + user_gid + "'";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            values.user_code = objMySqlDataReader["user_code"].ToString();
                            values.user_password = objMySqlDataReader["user_password"].ToString();
                            var ObjToken = Token(values.user_code, values.user_password);
                            dynamic newobj = JsonConvert.DeserializeObject(ObjToken);
                            tokenvalue = "Bearer " + newobj.access_token;

                            if (tokenvalue != null)
                            {
                                msSQL = "CALL storyboard.adm_mst_spstoretoken('" + tokenvalue + "', '" + values.user_code + "',  '" + values.user_password + "', '" + ConfigurationManager.AppSettings[domain].ToString() + "')";
                                user_gid = objdbconn.GetExecuteScalar("CALL storyboard.adm_mst_spstoretoken('" + tokenvalue + "','" + values.user_code + "','" + values.user_password + "','" + ConfigurationManager.AppSettings[domain].ToString() + "','Web','')");
                                GetLoginResponse.status = true;
                                GetLoginResponse.message = "";
                                GetLoginResponse.token = tokenvalue;
                                GetLoginResponse.user_gid = user_gid;
                            }
                        }



                    }
                    objMySqlDataReader.Close();
                }

                else
                {
                    GetLoginResponse.status = false;
                    GetLoginResponse.message = "Login time has been expired. kindly click the blade resend OTP ";

                }

            }
            catch (Exception ex)
            {
                GetLoginResponse.status = false;
                GetLoginResponse.message = "Invalid mail ID. Kindly contact your administrator";
            }
            finally
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK, GetLoginResponse);
        }

        [AllowAnonymous]
        [ActionName("GetOTPFlag")]
        [HttpGet]
        public HttpResponseMessage GetOTPFlag()
        {
            otpresponse GetOtpResponse = new otpresponse();
            try
            {
                GetOtpResponse.otp_flag = ConfigurationManager.AppSettings["otpFlag"].ToString();

            }
            catch (Exception ex)
            {
                GetOtpResponse.otp_flag = "N";

            }

            return Request.CreateResponse(HttpStatusCode.OK, GetOtpResponse);
        }

        public string Token(string userName, string password, string company_code = null)
        {

            var pairs = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "grant_type", "password" ),
                            new KeyValuePair<string, string>( "username", userName ),
                            new KeyValuePair<string, string> ( "Password", password ),
                            new KeyValuePair<string, string>("Scope",company_code),
                            new KeyValuePair<string, string>("RouterPrefix","api/login")
                        };
            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                domain = Request.RequestUri.Authority.ToLower();
                var host = HttpContext.Current.Request.Url.Host;
                if (host == "localhost")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var response = client.PostAsync(ConfigurationManager.AppSettings["protocol"].ToString() + domain +
                               "/StoryboardAPI/token", new FormUrlEncodedContent(pairs)).Result;
                    return response.Content.ReadAsStringAsync().Result;


                }
                else
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var response = client.PostAsync(ConfigurationManager.AppSettings["protocol"].ToString() + domain +
                               "/StoryboardAPI/token", new FormUrlEncodedContent(pairs)).Result;
                    return response.Content.ReadAsStringAsync().Result;

                }

            }
        }

        public void LoginErrorLog(string strVal)
        {
            try
            {
                string lspath = ConfigurationManager.AppSettings["file_path"].ToString() + "/erpdocument/LOGIN_ERRLOG/" + DateTime.Now.Year + @"\" + DateTime.Now.Month;
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);

                lspath = lspath + @"\" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";
                System.IO.StreamWriter sw = new System.IO.StreamWriter(lspath, true);
                sw.WriteLine(strVal);
                sw.Close();
            }
            catch (Exception ex)
            {
            }
        }
        [ActionName("incomingMessage")]
        [HttpPost]
        public HttpResponseMessage incomingMessage(mdlIncomingMessage values)
        {

            IEnumerable<string> headerAPIkeyValues = null;

            string APIKeyConfigured = ConfigurationManager.AppSettings["API_Key"].ToString();

            string type = "";

            if (Request.Headers.TryGetValues("API_Key", out headerAPIkeyValues))

            {

                var secretKey = headerAPIkeyValues.First();

                if (!string.IsNullOrEmpty(secretKey) && APIKeyConfigured.Equals(secretKey))

                {

                    if (ModelState.IsValid)

                    {

                        string mediaURL = "";

                        string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();

                        result objresult = new result();
                        if (values.message.body.type == "location")
                        {
                            msSQL = "insert into " + c_code + ".crm_trn_twhatsappmessages(" +
                                    "message_id," +
                                    "contact_id," +
                                    "direction," +
                                    "type," +
                                    "message_text," +
                                    "lat," +
                                    "lon," +
                                    "created_date)" +
                                    "values(" +
                                    "'" + values.message.messageId + "'," +
                                    "'" + values.message.sender.contact.contactId + "'," +
                                    "'incoming'," +
                                    "'" + values.message.body.type + "'," +
                                    "'Location'," +
                                    "'" + values.message.body.location.coordinations.latitude + "'," +
                                    "'" + values.message.body.location.coordinations.longitude + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        }
                        else
                        {
                            msSQL = "insert into " + c_code + ".crm_trn_twhatsappmessages(" +

                         "message_id," +

                         "contact_id," +

                         "direction," +

                         "type," +

                         "message_text," +

                         "content_type," +

                         "status," +

                         "created_date)" +

                         "values(" +

                         "'" + values.message.messageId + "'," +

                         "'" + values.message.sender.contact.contactId + "'," +

                         "'incoming'," +

                         "'" + values.message.body.type + "',";

                            if (values.message.body.type == "text")

                            {

                                type = "text";

                                msSQL += "'" + values.message.body.text.text + "'," +

                                         "null,";

                            }

                            else if (values.message.body.type == "image")

                            {

                                type = "image";

                                mediaURL = values.message.body.image.images[0].mediaUrl;

                                msSQL += "'Image'," +

                                         "null,";

                            }

                            else if (values.message.body.type == "list")

                            {

                                type = "list";

                                msSQL += "'List'," +

                                         "null,";

                            }

                            else

                            {

                                type = "file";

                                msSQL += "'File'," +

                                         "'" + values.message.body.file.files[0].contentType + "',";

                            }

                            msSQL += "'" + values.message.status + "'," +

                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        }

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)

                        {

                            if (type == "image" || type == "file")

                                fnFile(values, type, c_code);

                            //if (values.message.meta.order != null)
                            //{
                            //    HttpContext ctx = HttpContext.Current;
                            //    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                            //    {
                            //        HttpContext.Current = ctx;
                            //        fnWhatsappOrder(values, c_code);
                            //    }));
                            //    t.Start();
                            //}


                            objresult.status = true;

                            objresult.message = "success";


                        }

                        else

                        {

                            objresult.message = "fail";

                        }

                        msSQL = "select id from " + c_code + ". crm_smm_whatsapp where id ='" + values.message.sender.contact.contactId + "'";

                        string contact_id = objdbconn.GetExecuteScalar(msSQL);

                        if (contact_id != values.message.sender.contact.contactId)

                        {

                            msSQL = "insert into " + c_code + ".crm_smm_whatsapp(" +

                                    "id," +

                                    "displayName," +

                                    "wkey," +

                                    "wvalue," +

                                    "created_date )" +

                                    "values(" +

                                    "'" + values.message.sender.contact.contactId + "'," +

                                    "'Unknown Number '," +

                                    "'" + values.message.sender.contact.identifierKey + "'," +

                                    "'" + values.message.sender.contact.identifierValue + "'," +

                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)

                            {


                            }



                        }

                        else

                        {

                        }



                        return Request.CreateResponse(HttpStatusCode.OK, objresult);

                    }

                    else

                    {

                        return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                    }

                }

            }

            return Request.CreateResponse(System.Net.HttpStatusCode.Forbidden, "API key is invalid.");

        }

        [ActionName("incomingMail")]
        [HttpPost]
        public HttpResponseMessage incomingMail()
        {
            string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
            msSQL = " SELECT " + c_code + ".company_code FROM adm_mst_tcompany";
            string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            result objresult = new result();
            try
            {

                string jsonString = Request.Content.ReadAsStringAsync().Result;
                List<incomingMail> relayMessage1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<incomingMail>>(jsonString);
                incomingMail relayMessage = relayMessage1[0];
                msSQL = "select " + c_code + ".fn_getgid('MILC', '');";
                string mailmanagement_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "INSERT INTO  " + c_code + ".crm_smm_mailmanagement(" +
                        "mailmanagement_gid," +
                        "to_mail," +
                        "reply_to, " +
                        "sub," +
                        "body," +
                        "direction," +
                        "created_date)" +
                        "VALUES(" +
                        " '" + mailmanagement_gid + "'," +
                        " '" + relayMessage.msys.relay_message.friendly_from + "'," +
                        " '" + relayMessage.msys.relay_message.rcpt_to + "'," +
                        " '" + relayMessage.msys.relay_message.content.subject.Replace("'", "\\'") + "'," +
                        " '" + relayMessage.msys.relay_message.content.html.Replace("'", "\\'") + "'," +
                        "'incoming'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    objresult.message = "Error occured while inserting";
                }
                else
                {
                    bool status = mailAttachments(relayMessage.msys.relay_message.content.email_rfc822, mailmanagement_gid, c_code);
                    objresult.message = "success";
                    objresult.status = true;
                }
                msSQL = "select leadbank_gid from " + c_code + ".crm_smm_mailmanagement where to_mail='" + relayMessage.msys.relay_message.friendly_from + "';";
                string leadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                if (leadbank_gid != null)
                {
                    msSQL = "update " + c_code + ".crm_smm_mailmanagement set leadbank_gid ='" + leadbank_gid + "' where mailmanagement_gid='" + mailmanagement_gid + " '; ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Exception occured:" + ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        public void fnFile(mdlIncomingMessage values, string type, string c_code)
        {
            try
            {
                if (type == "image")
                {
                    foreach (var item in values.message.body.image.images)
                    {
                        string ext, filename, lspath, filepath = "", lspath1 = "/erpdocument/CRM/Whatsapp/" + values.message.sender.contact.contactId + "/";
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["messageBirdMediaURL"].ToString());
                        var request = new RestRequest(item.mediaUrl.Replace("https://media.nest.messagebird.com", ""), Method.GET);
                        request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            filename = response.Headers.FirstOrDefault(h => h.Name.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase))?.Value.ToString().Replace("inline; filename=\"", "").Replace("\"", "");
                            ext = System.IO.Path.GetExtension(filename).ToLower();
                            msSQL = "select " + c_code + ".fn_getgid('UPLF','')";
                            string file_gid = objdbconn.GetExecuteScalar(msSQL);
                            filepath = lspath1 + file_gid + ext;
                            lspath = ConfigurationManager.AppSettings["file_path"].ToString() + lspath1 + file_gid + ext;
                            // Save the content of the response to the specified local file
                            if ((!System.IO.Directory.Exists(ConfigurationManager.AppSettings["file_path"].ToString() + lspath1)))
                                System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["file_path"].ToString() + lspath1);
                            File.WriteAllBytes(lspath, response.RawBytes);

                            msSQL = "insert into " + c_code + ".crm_trn_tfiles(" +
                                    "file_gid," +
                                    "message_gid," +
                                    "contact_gid," +
                                    "document_name," +
                                    "document_path)values(" +
                                    "'" + file_gid + "'," +
                                    "'" + values.message.messageId + "'," +
                                    "'" + values.message.sender.contact.contactId + "'," +
                                    "'" + filename.Replace("'", "\\'") + "'," +
                                    "'" + filepath.Replace("'", "\\'") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }
                    }
                }
                else if (type == "file")
                {
                    foreach (var item in values.message.body.file.files)
                    {
                        string ext, filename, lspath, filepath = "", lspath1 = "/erpdocument/CRM/Whatsapp/" + values.message.sender.contact.contactId + "/";
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["messageBirdMediaURL"].ToString());
                        var request = new RestRequest(item.mediaUrl.Replace("https://media.nest.messagebird.com", ""), Method.GET);
                        request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            filename = response.Headers.FirstOrDefault(h => h.Name.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase))?.Value.ToString().Replace("inline; filename=\"", "").Replace("\"", "");
                            ext = System.IO.Path.GetExtension(filename).ToLower();
                            msSQL = "select " + c_code + ".fn_getgid('UPLF','')";
                            string file_gid = objdbconn.GetExecuteScalar(msSQL);
                            filepath = lspath1 + file_gid + ext;
                            lspath = ConfigurationManager.AppSettings["file_path"].ToString() + lspath1 + file_gid + ext;
                            // Save the content of the response to the specified local file
                            if ((!System.IO.Directory.Exists(ConfigurationManager.AppSettings["file_path"].ToString() + lspath1)))
                                System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["file_path"].ToString() + lspath1);
                            File.WriteAllBytes(lspath, response.RawBytes);

                            msSQL = "insert into " + c_code + ".crm_trn_tfiles(" +
                                    "file_gid," +
                                    "message_gid," +
                                    "contact_gid," +
                                    "document_name," +
                                    "document_path)values(" +
                                    "'" + file_gid + "'," +
                                    "'" + values.message.messageId + "'," +
                                    "'" + values.message.sender.contact.contactId + "'," +
                                    "'" + filename.Replace("'", "\\'") + "'," +
                                    "'" + filepath.Replace("'", "\\'") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + values.message.messageId.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        private bool mailAttachments(string rfc822_content, string mail_gid, string c_code)
        {

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            MemoryStream ms = new MemoryStream();
            var message = MimeMessage.Load(new MemoryStream(Encoding.UTF8.GetBytes(rfc822_content)));

            foreach (var part in message.Attachments.Where(a => a is MimeKit.MimePart).Cast<MimeKit.MimePart>())
            {
                msSQL = "select " + c_code + ".fn_getgid('UPLF', '');";
                string file_gid = objdbconn.GetExecuteScalar(msSQL);
                string fileName = part.FileName ?? "NoFileName";
                string fileExtension = System.IO.Path.GetExtension(fileName);
                bool status1;

                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Mail/IncomingMail/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + file_gid + fileExtension, fileExtension, ms);

                string final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/IncomingMail/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                string httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + file_gid + fileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                        '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                        '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];

                using (var stream = File.Create(httpsUrl))
                {
                    part.Content.DecodeTo(stream);
                }
                ms.Close();
                msSQL = "INSERT INTO " + c_code + ".crm_trn_tfiles (" +
                           "file_gid, " +
                          "mailmanagement_gid, " +
                          "document_name, " +
                        "document_path, " +
                          "created_date) " +
                          "VALUES (" +
                           "'" + file_gid + "', " +
                          "'" + mail_gid + "', " +
                          "'" + fileName + "', " +
                           "'" + httpsUrl + "', " +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }
            return true;
        }

        [HttpPost]
        [ActionName("calendlyWebhook")]
        public HttpResponseMessage calendlyWebhook()
        {
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            string company_code = HttpContext.Current.Request.Headers["Calendly-Webhook-Signature"];

            MdlCalendy objcalendly = JsonConvert.DeserializeObject<MdlCalendy>(bodyText);
            List<string> meeting_attendees = new List<string>();
            meeting_attendees.Add(objcalendly.payload.email);

            DateTime starttimeFromOtherSystem = objcalendly.payload.scheduled_event.start_time;
            DateTime start_time = ConvertToMyTimeZone(starttimeFromOtherSystem);
            DateTime endtimeFromOtherSystem = objcalendly.payload.scheduled_event.end_time;
            DateTime end_time = ConvertToMyTimeZone(endtimeFromOtherSystem);

            string meeting_participants = objcalendly.payload.email;

            foreach (var items in objcalendly.payload.scheduled_event.event_guests)
            {
                meeting_participants += "," + items.email;
                meeting_attendees.Add(items.email);

            }

            string company_db = getCompanyDB(company_code, bodyText);

            msSQL = "select meeting_gid from " + company_db + ".crm_mst_tcalendlymeetings where meeting_start_time = '" + start_time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (!objOdbcDataReader.HasRows)
            {
                msSQL = "insert into " + company_db + ".crm_mst_tcalendlymeetings(" +
                    "meeting_url," +
                    "meeting_organiser_name," +
                    "meeting_organiser_mail," +
                    "meeting_participants," +
                    "meeting_description," +
                    "meeting_description_html," +
                    "meeting_title," +
                    "meeting_start_time," +
                    "meeting_end_time,created_date,meeting_type,location)values(" +
                    "'" + objcalendly.payload.scheduled_event.location.join_url + "'," +
                    "'" + objcalendly.payload.scheduled_event.event_memberships[0].user_name + "'," +
                    "'" + objcalendly.payload.scheduled_event.event_memberships[0].user_email + "'," +
                    "'" + meeting_participants + "'," +
                    "'" + objcalendly.payload.scheduled_event.meeting_notes_plain + "'," +
                    "'" + objcalendly.payload.scheduled_event.meeting_notes_html + "'," +
                    "'" + objcalendly.payload.scheduled_event.name + "'," +
                    "'" + start_time.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + end_time.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                if (objcalendly.payload.scheduled_event.location.type == "google_conference" || objcalendly.payload.scheduled_event.location.type == "microsoft_teams_conference")
                    msSQL += "'Online',";
                else
                    msSQL += "'Offline',";
                msSQL += "'" + objcalendly.payload.scheduled_event.location.location + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = "select meeting_gid from " + company_db + ".crm_mst_tcalendlymeetings order by created_date desc limit 1";
                    string meeting_gid = objdbconn.GetExecuteScalar(msSQL);

                    foreach (var data in meeting_attendees)
                    {
                        msSQL = "insert into " + company_db + ".crm_trn_tcalendlymeetingattendees(meeting_gid,attendee_email,user_gid)values(" +
                                "'" + meeting_gid + "'," +
                                "'" + data + "'," +
                                "(select user_gid from " + company_db + ".hrm_mst_temployee where employee_emailid  = '" + data + "' limit 1))";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                            objcmnfunctions.LogForAudit("Insert failed: " + msSQL);
                    }
                }
                else
                {
                    objcmnfunctions.LogForAudit("Insert failed: " + msSQL);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        public string getCompanyDB(string company_code, string bodyText)
        {
            string[] signatureParts = company_code.Split(',');
            string t = "";
            string signature = "";

            foreach (string part in signatureParts)
            {
                string[] keyValue = part.Trim().Split('=');
                if (keyValue.Length == 2)
                {
                    if (keyValue[0] == "t")
                    {
                        t = keyValue[1];
                    }
                    else if (keyValue[0] == "v1")
                    {
                        signature = keyValue[1];
                    }
                }
            }

            string data = t + "." + bodyText;

            string[] company_db = { "vcidex", "boba_tea" };


            foreach (string db in company_db)
            {
                using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(db)))
                {
                    byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                    string computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    if (computedSignature == signature)
                    {
                        company_code = db;
                    }
                }
            }

            return company_code;
        }

        static DateTime ConvertToMyTimeZone(DateTime utcTime)
        {
            // Get the timezone info for your timezone
            TimeZoneInfo myTimeZone = TimeZoneInfo.Local;
            // Convert the UTC time to your local timezone
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, myTimeZone);
            return localTime;
        }

        public void fnWhatsappOrder(mdlIncomingMessage values, string company_code)
        {
            try
            {
                double total = 0.00;
                string lsrefno = objcmnfunctions.GetMasterGID("SOR", company_code, "", "");
                string mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP", company_code, "", "");

                msSQL = "select  branch_gid from " + company_code + ".hrm_mst_tbranch order by created_date desc  limit 1; ";
                string branch_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT currency_code,currencyexchange_gid FROM " + company_code + ".crm_trn_tcurrencyexchange WHERE default_currency = 'Y';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    currency_code = objOdbcDataReader["currency_code"].ToString();
                    currencyexchange_gid = objOdbcDataReader["currencyexchange_gid"].ToString();
                }

                foreach (var item in values.message.meta.order.products)
                {
                    item.price.amount = item.price.amount * Math.Pow(10, item.price.exponent);
                    total += item.price.amount * item.quantity;
                }

                msSQL = "select a.customer_gid,a.customercontact_gid,a.customercontact_name,a.email,a.mobile, " +
                        "b.customer_address as address1,b.customer_name,b.customer_address2 as address2,b.customer_city,b.customer_country " +
                        "from " + company_code + ".crm_mst_tcustomercontact a " +
                        "left join " + company_code + ".crm_mst_tcustomer b on b.customer_gid=a.customer_gid " +
                        "where a.mobile like '%" + Regex.Replace(values.message.sender.contact.identifierValue, @"^\+?(44|0|91|1)", "") + "%'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    customercontact_name = objOdbcDataReader["customercontact_name"].ToString();
                    customer_name = objOdbcDataReader["customer_name"].ToString();
                    customer_gid = objOdbcDataReader["customer_gid"].ToString();
                    customercontact_gid = objOdbcDataReader["customercontact_gid"].ToString();
                    address1 = objOdbcDataReader["address1"].ToString();
                    email = objOdbcDataReader["email"].ToString();
                    mobile = objOdbcDataReader["mobile"].ToString();

                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("CC", company_code, "", "");
                    msSQL = " Select sequence_curval from " + company_code + ".adm_mst_tsequence where sequence_code ='CC'";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);
                    string lscustomer_code = "CC-00" + lsCode;
                    msGetGid1 = objcmnfunctions.GetMasterGID("BCRM", company_code, "", "");
                    msGetGid2 = objcmnfunctions.GetMasterGID("BCCM", company_code, "", "");
                    msGetGid3 = objcmnfunctions.GetMasterGID("BLBP", company_code, "", "");
                    msGetGid4 = objcmnfunctions.GetMasterGID("BLCC", company_code, "", "");

                    msSQL = " insert into " + company_code + ". crm_mst_tcustomer (" +
                            " customer_gid," +
                            " customer_id, " +
                            " customer_name, " +
                            " status ," +
                            " created_by," +
                            "created_date" +
                             ") values (" +
                            "'" + msGetGid1 + "', " +
                            "'" + lscustomer_code + "'," +
                            "'" + values.message.sender.contact.identifierValue + "'," +
                            "'Active'," +
                            "'U1'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult1 != 0)
                    {

                        msSQL = " insert into " + company_code + ". crm_mst_tcustomercontact  (" +
                                " customercontact_gid," +
                                " customer_gid," +
                                " customerbranch_name," +
                                " customercontact_name, " +
                                " mobile, " +
                                " main_contact, " +
                                " created_by," +
                                "created_date" +
                                ") values (" +
                                "'" + msGetGid2 + "', " +
                                "'" + msGetGid1 + "', " +
                                 "'H.Q', " +
                                "'" + values.message.sender.contact.identifierValue + "'," +
                                "'" + values.message.sender.contact.identifierValue + "'," +
                                "'Y'," +
                                "'U1'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult2 != 0)
                        {
                            msSQL = " INSERT into " + company_code + ".  crm_trn_tleadbank " +
                                    " (leadbank_gid, " +
                                    " customer_gid, " +
                                    " leadbank_name," +
                                    " leadbank_code, " +
                                    " approval_flag, " +
                                    " leadbank_id, " +
                                    " status, " +
                                    " main_branch," +
                                    " main_contact," +
                                    " created_by, " +
                                    " created_date)" +
                                    " values ( " +
                                    "'" + msGetGid3 + "'," +
                                    "'" + msGetGid1 + "'," +
                                    "'" + values.message.sender.contact.identifierValue + "'," +
                                    "'H.Q'," +
                                    "'Approved'," +
                                    "'" + lscustomer_code + "'," +
                                    "'Y'," +
                                    "'Y'," +
                                    "'Y'," +
                                    "'U1'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");
                            }
                            else
                            {
                                msSQL = " INSERT into " + company_code + ". crm_trn_tleadbankcontact (" +
                                " leadbankcontact_gid, " +
                                " leadbank_gid," +
                                " leadbankbranch_name, " +
                                " leadbankcontact_name," +
                                " mobile," +
                                " main_contact," +
                                " created_by," +
                                " created_date)" +
                                " values (" +
                                " '" + msGetGid4 + "'," +
                                " '" + msGetGid3 + "'," +
                                "'H.Q'," +
                                "'" + values.message.sender.contact.identifierValue + "'," +
                                "'" + values.message.sender.contact.identifierValue + "'," +
                                "'Y'," +
                                "'U1'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 0)
                                {
                                    objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");
                                }
                            }
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");
                        }

                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");

                    }

                }
                if (customer_gid == null)
                {
                    customer_gid = msGetGid1;
                }
                msSQL = " insert  into " + company_code + ".smr_trn_tsalesorder (" +
                         " salesorder_gid ," +
                         " branch_gid ," +
                         " salesorder_date," +
                         " customer_gid," +
                         " customer_name," +
                         " customer_contact_gid," +
                         " customer_contact_person," +
                         " customerbranch_gid," +
                         " customer_address," +
                         " customer_email, " +
                         " customer_mobile, " +
                         " created_by," +
                         " so_referenceno1 ," +
                         " Grandtotal, " +
                         " salesorder_status, " +
                         " grandtotal_l, " +
                         " currency_code, " +
                         " currency_gid, " +
                         " total_price," +
                         " total_amount," +
                          "source_flag," +
                         " message_id," +
                         "created_date" +
                         " )values(" +
                         " '" + mssalesorderGID + "'," +
                         " '" + branch_gid + "'," +
                         " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                         " '" + customer_gid + "'," +
                         " '" + customer_name + "'," +
                         " '" + customercontact_gid + "'," +
                         " '" + customercontact_name + "'," +
                         " 'H.Q'," +
                         " '" + address1 + "',";
                if (!String.IsNullOrEmpty(msSQL))
                {
                    msSQL += " '" + email + "',";
                }
                else
                {
                    msSQL += "null,";

                }
                msSQL += " '" + mobile + "'," +
                         "'U1' ," +
                         " '" + lsrefno + "'," +
                         " '" + total + "'," +
                         "'Approved' ," +
                         " '" + total + "'," +
                         " '" + currency_code + "'," +
                         " '" + currencyexchange_gid + "'," +
                         " '" + total + "'," +
                         " '" + total + "'," +
                          "'W' ," +
                            "'" + values.message.messageId + "',";
                msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult1 != 0)
                {
                    msSQL = " insert  into " + company_code + ".acp_trn_torder (" +
                      " salesorder_gid ," +
                      " branch_gid ," +
                      " salesorder_date," +
                      " customer_gid," +
                      " customer_name," +
                      " customer_contact_gid," +
                      " customer_contact_person," +
                      " customerbranch_gid," +
                      " customer_address," +
                      " customer_email, " +
                      " customer_mobile, " +
                      " created_by," +
                      " so_referencenumber ," +
                      " Grandtotal, " +
                      " salesorder_status, " +
                      " grandtotal_l, " +
                      " currency_code, " +
                      " currency_gid, " +
                      " campaign_gid," +
                      "created_date" +
                      " )values(" +
                      " '" + mssalesorderGID + "'," +
                      " '" + branch_gid + "'," +
                      " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                      " '" + customer_gid + "'," +
                      " '" + customer_name + "'," +
                      " '" + customercontact_gid + "'," +
                      " '" + customercontact_name + "'," +
                      " 'H.Q'," +
                      " '" + address1 + "'," +
                      " '" + email + "'," +
                      " '" + mobile + "'," +
                      "'U1' ," +
                      " '" + lsrefno + "'," +
                      " '" + total + "'," +
                      "'Approved' ," +
                      " '" + total + "'," +
                      " '" + currency_code + "'," +
                      " '" + currencyexchange_gid + "'," +
                         "'NO CAMPAIGN',";
                    msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult2 != 0)
                    {
                        foreach (var item in values.message.meta.order.products)

                        {
                            string salesorderdtl_gid = objcmnfunctions.GetMasterGID("VSDC", company_code, "", "");
                            double totalValue = item.quantity * item.price.amount;

                            msSQL = "select a.productgroup_gid,a.product_name,a.product_code,a.productuom_gid,b.productuom_name,c.productgroup_name " +
                         " from " + company_code + ".pmr_mst_tproduct a left join " + company_code + ".pmr_mst_tproductgroup c on c.productgroup_gid=a.productgroup_gid " +
                         " left join " + company_code + ".pmr_mst_tproductuom b on b.productuom_gid=a.productuom_gid where a.product_gid='" + item.externalProductId + "' ";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows)
                            {
                                productgroup_gid = objOdbcDataReader["productgroup_gid"].ToString();
                                productgroup_name = objOdbcDataReader["productgroup_name"].ToString();
                                product_name = objOdbcDataReader["product_name"].ToString();
                                product_code = objOdbcDataReader["product_code"].ToString();
                                productuom_gid = objOdbcDataReader["productuom_gid"].ToString();
                                productuom_name = objOdbcDataReader["productuom_name"].ToString();

                            }
                            msSQL = " insert  into " + company_code + ".smr_trn_tsalesorderdtl (" +
                                       " salesorderdtl_gid ," +
                                       " salesorder_gid ," +
                                       " product_gid," +
                                       " product_name," +
                                       " product_code," +
                                       " productgroup_gid," +
                                       " productgroup_name," +
                                       " product_price," +
                                       " qty_quoted," +
                                       " uom_gid," +
                                       " uom_name, " +
                                       " price, " +
                                       " salesorder_refno ," +
                                      " created_by," +
                                       " currency_gid, " +
                                       "created_date" +
                                       " )values(" +
                                       " '" + salesorderdtl_gid + "'," +
                                       " '" + mssalesorderGID + "'," +
                                       " '" + item.externalProductId + "'," +
                                       " '" + product_name + "'," +
                                       " '" + product_code + "'," +
                                       " '" + productgroup_gid + "'," +
                                       " '" + productgroup_name + "'," +
                                       " '" + item.price.amount + "'," +
                                       " '" + item.quantity + "'," +
                                       " '" + productuom_gid + "'," +
                                       " '" + productuom_name + "'," +
                                       " '" + totalValue + "'," +
                                       " '" + lsrefno + "'," +
                                         "'U1' ," +
                                       " '" + currencyexchange_gid + "',";
                            msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult3 != 0)
                            {
                                msSQL = " insert into " + company_code + ". acp_trn_torderdtl (" +
                          " salesorderdtl_gid ," +
                          " salesorder_gid," +
                          " product_gid ," +
                          " product_name," +
                           " productgroup_gid," +
                          " productgroup_name," +
                          " product_price," +
                          " qty_quoted," +
                          " uom_gid," +
                          " uom_name," +
                          " price," +
                          " type, " +
                          " salesorder_refno ," +
                          " created_by," +
                          "created_date" +
                          ")values(" +
                          " '" + salesorderdtl_gid + "'," +
                          " '" + mssalesorderGID + "'," +
                          " '" + item.externalProductId + "'," +
                          " '" + product_name + "'," +
                          " '" + productgroup_gid + "'," +
                          " '" + productgroup_name + "'," +
                          " '" + item.price.amount + "'," +
                          " '" + item.quantity + "'," +
                          " '" + productuom_gid + "'," +
                          " '" + productuom_name + "'," +
                          " '" + totalValue + "'," +
                          " 'Sales'," +
                          " '" + lsrefno + "'," +
                           "'U1', ";
                                msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                mnResult4 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult4 == 0)

                                {
                                    objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");

                                }
                            }

                            else
                            {
                                objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");

                            }

                        }
                        if (mnResult4 != 0)
                        {
                            string msGetGID1 = objcmnfunctions.GetMasterGID("PODC", company_code, "", "");
                            msSQL = " insert into " + company_code + ".smr_trn_tapproval ( " +
                             " approval_gid, " +
                             " approved_by, " +
                             " approved_date, " +
                             " submodule_gid, " +
                             " soapproval_gid " +
                             " ) values ( " +
                             "'" + msGetGID1 + "'," +
                             " 'E1'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'SMRSROSOA'," +
                             "'" + mssalesorderGID + "') ";
                            mnResult5 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult5 == 0)
                            {
                                objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");

                            }
                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");

                    }
                }
                else
                {
                    objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");
                }

            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        [ActionName("incomingOrderfromWhatsApp")]
        public HttpResponseMessage incomingOrderfromWhatsApp(mdlIncomingMessage values)
        {
            MdlIncomingOrder objMdlIncomingOrder = new MdlIncomingOrder();
            try
            {
                string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
                objMdlIncomingOrder.order_gid = objcmnfunctions.GetMasterGID("VSOP", c_code, "", "");

                msSQL = "select address from " + c_code + ".otl_trn_tkot where contact_id = '" + values.message.sender.participant.participantId + "' and address is not null order by created_date desc limit 1";
                objMdlIncomingOrder.address = objdbconn.GetExecuteScalar(msSQL);
                objMdlIncomingOrder.address = string.IsNullOrEmpty(objMdlIncomingOrder.address) ? null : objMdlIncomingOrder.address;


                msSQL = "insert into " + c_code + ".otl_trn_tkot(" +
                        "kot_gid," +
                        //"branch_gid," +
                        "order_id," +
                        "contact_id," +
                        "source," +
                        "message_id," +
                        "created_date," +
                        "created_by)" +
                        "values(" +
                        "'" + objMdlIncomingOrder.order_gid + "'," +
                        //"'" + branch_gid + "'," +
                        "''," +
                        "'" + values.message.sender.participant.participantId + "'," +
                        "'W'," +
                        "'" + values.message.conversationMessageId + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'U1')";

                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult1 != 0)
                {
                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    {
                       HttpContext.Current = ctx;
                        fnOrderLineItems(values, c_code, objMdlIncomingOrder.order_gid);
                    }));
                    t.Start();
                    objMdlIncomingOrder.status = true;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("Excception occured in incomingOrderfromWhatsApp : " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Insert/incomingOrderfromWhatsApp.txt");
            }
            return Request.CreateResponse(HttpStatusCode.OK, objMdlIncomingOrder);
        }

        public void fnOrderLineItems(mdlIncomingMessage values, string c_code, string order_gid)
        {
            try
            {
                string branch_gid = getBranchGID(values.message.channelId, c_code);
                
                double order_total = 0.00;
                msSQL = " insert into " + c_code + ".otl_trn_tkotdtl(kot_gid,kot_product_gid,kot_product_price,product_quantity,product_amount,currency,created_date,created_by) values";

                foreach (var item in values.message.meta.order.products)
                {
                    item.price.amount = item.price.amount * Math.Pow(10, item.price.exponent);
                    double totalValue = item.quantity * item.price.amount;
                    order_total += totalValue;

                    msSQL += "('" + order_gid + "','" + item.externalProductId + "','" + item.price.amount + "', '" + item.quantity + "','" + totalValue + "', '" + item.price.currencyCode + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','U1'),";
                }
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL.TrimEnd(','));
                if (mnResult == 0)
                {
                    objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Insert/fnOrderLineItems.txt");
                }
                msSQL = "update " + c_code + ".otl_trn_tkot set line_items_total='" + Math.Round(order_total, 2) + "'," +
                        "kot_tot_price = '" + Math.Round(order_total,2) + "', branch_gid = '" + branch_gid + "' " + 
                        "where kot_gid = '" + order_gid + "'";
                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult2 == 0)
                {
                    objcmnfunctions.LogForAudit("Update Failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/fnOrderLineItems.txt");
                }

                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    fnWhatsAppMessage(values);
                }));
                t.Start();
                
                whatsappconfiguration1 getwhatsappcredentials = whatsappcredentials(c_code, branch_gid);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/contacts/" + values.message.sender.participant.participantId, Method.GET);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                IRestResponse response = client.Execute(request);
                MdlContactResponse objMdlContactResponse = JsonConvert.DeserializeObject<MdlContactResponse>(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    msSQL = "update " + c_code + ".otl_trn_tkot set customer_phone = '" + objMdlContactResponse.featuredIdentifiers[0].value + "' where kot_gid = '" + order_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        objcmnfunctions.LogForAudit("Insert Failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/fnOrderLineItems.txt");
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("Exception occured " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Update/fnOrderLineItems.txt");
            }
        }

        public whatsappconfiguration1 whatsappcredentials(string c_code, string branch_gid)
        {
            whatsappconfiguration1 getwhatsappcredentials = new whatsappconfiguration1();
            try
            {
                msSQL = "select waworkspace_id,wachannel_id,bird_token from " + c_code + ".hrm_mst_tbranch where branch_gid = '" + branch_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    getwhatsappcredentials.workspace_id = objOdbcDataReader["waworkspace_id"].ToString();
                    getwhatsappcredentials.channel_id = objOdbcDataReader["wachannel_id"].ToString();
                    getwhatsappcredentials.access_token = objOdbcDataReader["bird_token"].ToString();
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Configuartion" + "whatsappcredentials " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return getwhatsappcredentials;
        }

        [HttpPost]
        [ActionName("updateOrderAddress")]
        public HttpResponseMessage updateOrderAddress(MdlOrderDispatchMode values)
        {
            string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
            values.address = string.IsNullOrEmpty(values.address) ? null : values.address.Replace("'", "\\'");
            msSQL = "update " + c_code + ".otl_trn_tkot set address = '" + values.address + "' where kot_gid = '" + values.order_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    fnWhatsAppMessage1(values.customer_message);
                }));
                t.Start();
            }
            else
            {
                objcmnfunctions.LogForAudit("Update Failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/updateOrderAddress.txt");
            }

            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [HttpPost]
        [ActionName("Getemail")]
        public HttpResponseMessage Getemail(PostUserLogin values)
        {
            msSQL = "select employee_companyemailid from  `" + values.company_code + "`.hrm_mst_temployee a left join  `" + values.company_code + "`.adm_mst_tuser b on b.user_gid=a.user_gid where user_code='" + values.user_code + "'";
            values.email = objdbconn.GetExecuteScalar(msSQL);
            values.status = true;
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [HttpPost]
        [ActionName("verifypassword")]
        public HttpResponseMessage verifypassword(PostUserLogin values)
        {
            msSQL = "select forgot_code from  `" + values.company_code + "`.adm_mst_tuser  where user_code='" + values.user_code + "'";
            values.email = objdbconn.GetExecuteScalar(msSQL);
            if (values.email == values.code)
            {
                values.status = true;
                values.message = "Verified OTP Successfully!";
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            else
            {
                values.status = false;
                values.message = "Invalid OTP!";
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }

            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [HttpPost]
        [ActionName("updateOrderDispatchMode")]
        public HttpResponseMessage updateOrderDispatchMode(MdlOrderDispatchMode values)
        {
            string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
            msSQL = "update " + c_code + ".otl_trn_tkot set order_type = '" + values.dispatch_mode + "' where kot_gid = '" + values.order_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    fnWhatsAppMessage1(values.customer_message);
                }));
                t.Start();
            }
            else
            {
                objcmnfunctions.LogForAudit("Update Failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/updateOrderDispatchMode.txt");
            }

            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        public string getBranchGID(string channel_id, string c_code)
        {
            string branchGID = null;

            msSQL = "select  branch_gid from " + c_code + ".hrm_mst_tbranch where wachannel_id='" + channel_id + "' ";
            branchGID = objdbconn.GetExecuteScalar(msSQL);

            if (string.IsNullOrEmpty(branchGID))
            {
                msSQL = "select campaign_gid from " + c_code + ".otl_trn_touletcampaign where whatsappchannel_id='" + channel_id + "' ";
                branchGID = objdbconn.GetExecuteScalar(msSQL);
            }

            return branchGID;
        }

        [HttpPost]
        [ActionName("stripeProductPush")]
        public HttpResponseMessage stripeProductPush(MdlStripeProducts values)
        {
            try
            {
                paymentgatewayconfiguration1 getpaymentgatewaycredentials1 = paymentgatewaycredentials1(values.c_code);
                string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(getpaymentgatewaycredentials1.stripe_key + ":"));
                string token = "Basic " + encodedCredentials;
                msSQL = "select currency_code from " + values.c_code + ".crm_trn_tcurrencyexchange where default_currency = 'Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);

                if (values.table == "pmr_mst_tproduct")
                {
                    msSQL = "select product_name,product_desc,product_gid,mrp_price from " + values.c_code + ".pmr_mst_tproduct where stripe_price_gid is null";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable != null)
                    {
                        if (dt_datatable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt_datatable.Rows)
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var productclient = new RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                                var productrequest = new RestRequest("/v1/products", Method.POST);
                                productrequest.AddHeader("authorization", token);
                                productrequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                productrequest.AddParameter("name", dr["product_name"].ToString());
                                productrequest.AddParameter("description", dr["product_desc"].ToString());
                                productrequest.AddParameter("id", dr["product_gid"].ToString());
                                IRestResponse productresponse = productclient.Execute(productrequest);
                                if (productresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    double amount = Convert.ToDouble(dr["mrp_price"]) * 100;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    var priceclient = new RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                                    var pricerequest = new RestRequest("/v1/prices", Method.POST);
                                    pricerequest.AddHeader("authorization", token);
                                    pricerequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                    pricerequest.AddParameter("currency", currency);
                                    pricerequest.AddParameter("unit_amount", amount);
                                    pricerequest.AddParameter("product", dr["product_gid"].ToString());
                                    IRestResponse priceresponse = priceclient.Execute(pricerequest);
                                    if (priceresponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        stripePriceResponse objstripePriceResponse = JsonConvert.DeserializeObject<stripePriceResponse>(priceresponse.Content);

                                        msSQL = "update " + values.c_code + ".pmr_mst_tproduct set stripe_price_gid = '" + objstripePriceResponse.id + "' where product_gid = '" + dr["product_gid"].ToString() + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 0)
                                        {
                                            objcmnfunctions.LogForAudit("Update failed: " + msSQL, values.c_code + "/errorLog/stripeProducts/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        }
                                    }
                                    else
                                    {
                                        objcmnfunctions.LogForAudit("Error occured while adding price: " + productresponse.Content, values.c_code + "/errorLog/stripeProducts/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                    }
                                }
                                else
                                {
                                    objcmnfunctions.LogForAudit("Error occured while adding product: " + productresponse.Content, values.c_code + "/errorLog/stripeProducts/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                }
                            }
                        }
                    }
                }
                else
                {
                    msSQL = "select b.product_name,b.product_desc,a.whatsapp_price,a.branch2product_gid from " + values.c_code + ".otl_trn_branch2product a " +
                            "left join " + values.c_code + ".pmr_mst_tproduct b on a.product_gid = b.product_gid where a.stripe_price_gid is null";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable != null)
                    {
                        if (dt_datatable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt_datatable.Rows)
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var productclient = new RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                                var productrequest = new RestRequest("/v1/products", Method.POST);
                                productrequest.AddHeader("authorization", token);
                                productrequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                productrequest.AddParameter("name", dr["product_name"].ToString());
                                productrequest.AddParameter("description", dr["product_desc"].ToString());
                                productrequest.AddParameter("id", dr["branch2product_gid"].ToString());
                                IRestResponse productresponse = productclient.Execute(productrequest);
                                if (productresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    double amount = Convert.ToDouble(dr["whatsapp_price"]) * 100;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    var priceclient = new RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                                    var pricerequest = new RestRequest("/v1/prices", Method.POST);
                                    pricerequest.AddHeader("authorization", token);
                                    pricerequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                    pricerequest.AddParameter("currency", currency);
                                    pricerequest.AddParameter("unit_amount", amount);
                                    pricerequest.AddParameter("product", dr["branch2product_gid"].ToString());
                                    IRestResponse priceresponse = priceclient.Execute(pricerequest);
                                    if (priceresponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        stripePriceResponse objstripePriceResponse = JsonConvert.DeserializeObject<stripePriceResponse>(priceresponse.Content);

                                        msSQL = "update " + values.c_code + ".otl_trn_branch2product set stripe_price_gid = '" + objstripePriceResponse.id + "' where branch2product_gid = '" + dr["branch2product_gid"].ToString() + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 0)
                                        {
                                            objcmnfunctions.LogForAudit("Update failed: " + msSQL, values.c_code + "/errorLog/stripeProducts/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        }
                                    }
                                    else
                                    {
                                        objcmnfunctions.LogForAudit("Error occured while adding price: " + productresponse.Content, values.c_code + "/errorLog/stripeProducts/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                    }
                                }
                                else
                                {
                                    objcmnfunctions.LogForAudit("Error occured while adding product: " + productresponse.Content, values.c_code + "/errorLog/stripeProducts/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("Exception occured while adding product: " + ex.ToString(), values.c_code + "/errorLog/stripeProducts/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        [HttpPost]
        [ActionName("updateOrderInstructions")]
        public HttpResponseMessage updateOrderInstructions(MdlOrderDispatchMode values)
        {
            try
            {
                string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
                msSQL = "update " + c_code + ".otl_trn_tkot set order_instructions = '" + values.order_instructions.Replace("'", "\\'") + "' where kot_gid = '" + values.order_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    {
                        HttpContext.Current = ctx;
                        fnWhatsAppMessage1(values.customer_message);
                    }));
                    t.Start();
                }
                else
                {
                    objcmnfunctions.LogForAudit("Update Failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/updateOrderInstructions.txt");
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured: " + ex.ToString();
                objcmnfunctions.LogForAudit("Exception occured while updating payment status: " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Update/updateOrderInstructions.txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                {
                    objOdbcDataReader.Close();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [HttpPost]
        [ActionName("updateOrderPayment")]
        public HttpResponseMessage updateOrderPayment(MdlOrderDispatchMode values)
        {
            try
            {
                values.order_gid = objFnazurestorage.DecryptData(values.order_gid);
                values.c_code = objFnazurestorage.DecryptData(values.c_code);

                msSQL = "update " + values.c_code + ".otl_trn_tkot set payment_status = 'PAID',order_status = 'CONFIRMED' where kot_gid = '" + values.order_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    {
                        HttpContext.Current = ctx;
                        sendConfirmationMessage(values.order_gid, values.c_code);
                    }));
                    t.Start();
                    values.status = true;
                }
                else
                {
                    objcmnfunctions.LogForAudit("Update Failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/updateOrderPayment.txt");
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured: " + ex.ToString();
                objcmnfunctions.LogForAudit("Exception occured while updating payment status: " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Update/updateOrderPayment.txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                {
                    objOdbcDataReader.Close();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [HttpPost]
        [ActionName("createPaymentLink")]
        public HttpResponseMessage createPaymentLink(MdlOrderDispatchMode values)
        {
            try
            {
                string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
                paymentgatewayconfiguration1 getpaymentgatewaycredentials1 = paymentgatewaycredentials1(c_code);
                string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(getpaymentgatewaycredentials1.stripe_key + ":"));
                string token = "Basic " + encodedCredentials;
                int i = 0;

                msSQL = "select a.product_quantity,b.product_gid, " +
                        "case when b.product_gid is null then c.stripe_price_gid else b.stripe_price_gid end as stripe_price_gid " +
                        "from " + c_code + ".otl_trn_tkotdtl a " +
                        "left join " + c_code + ".pmr_mst_tproduct b on b.product_gid = a.kot_product_gid " +
                        "left join " + c_code + ".otl_trn_branch2product c on c.branch2product_gid = a.kot_product_gid " +
                        "where a.kot_gid = '" + values.order_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable != null)
                {
                    if (dt_datatable.Rows.Count > 0)
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                        var request = new RestRequest("/v1/payment_links", Method.POST);
                        request.AddHeader("Authorization", token);
                        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            request.AddParameter("line_items[" + i + "][price]", dt["stripe_price_gid"].ToString());
                            request.AddParameter("line_items[" + i + "][quantity]", dt["product_quantity"].ToString());
                            i++;
                        }
                        request.AddParameter("allow_promotion_codes", "true");
                        request.AddParameter("after_completion[type]", "redirect");
                        request.AddParameter("after_completion[redirect][url]", ConfigurationManager.AppSettings["paymentredirect_URL"].ToString() + objFnazurestorage.EncryptData(values.order_gid) + "&v2=" + objFnazurestorage.EncryptData(c_code));
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            MdlStripePaymentLink objMdlStripePaymentLink = JsonConvert.DeserializeObject<MdlStripePaymentLink>(response.Content);
                            values.payment_link = objMdlStripePaymentLink.url.Replace("https://buy.stripe.com/", "");
                            
                            HttpContext ctx = HttpContext.Current;
                            System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                            {
                                HttpContext.Current = ctx;
                                updatePaymentLink(c_code, objMdlStripePaymentLink.id, values.order_gid);
                            }));
                            t.Start();
                            values.status = true;
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("error occured while generating payment link: " + response.Content, "/WhatsAppOrders/ErrorLog/Update/createPaymentLink.txt");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured: " + ex.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        public void sendConfirmationMessage(string order_gid, string c_code)
        {
            string contactjson, branch_gid = "", manager_number = "", products = "", order_id = "", customer_no = "", manager_message = "", payment_link = "";
            try
            {
                order_id = objcmnfunctions.GetMasterGID("SOR", c_code, "", "");
                msSQL = "select customer_phone,payment_message,branch_gid,manager_message,payment_link from " + c_code + ".otl_trn_tkot where kot_gid = '" + order_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    branch_gid = objOdbcDataReader["branch_gid"].ToString();
                    //order_id = objOdbcDataReader["order_id"].ToString();
                    customer_no = objOdbcDataReader["customer_phone"].ToString();
                    manager_message = objOdbcDataReader["manager_message"].ToString();
                    payment_link = objOdbcDataReader["payment_link"].ToString();
                    if (objOdbcDataReader["payment_message"].ToString() == "N")
                    {
                        contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + objOdbcDataReader["customer_phone"].ToString() + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"*Order* *Confirmed*\\n\\nDear Customer, Your Order No is *" + order_id + "*.\\n\\nWe will process your request and keep you posted on the progress.\\n\\nThank You\"}}}";
                        //contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + objOdbcDataReader["customer_phone"].ToString() + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + ConfigurationManager.AppSettings["orderConfirmationProjID"].ToString() + "\",\"version\":\"" + ConfigurationManager.AppSettings["orderConfirmationVersionID"].ToString() + "\",\"parameters\":[{\"type\": \"string\",\"key\": \"order_id\",\"value\": \"" + objOdbcDataReader["order_id"].ToString() + "\"}],\"locale\":\"en\"}}";
                        whatsappconfiguration1 getwhatsappcredentials = whatsappcredentials(c_code, branch_gid);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                        var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                        request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                        request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        Results objsendmessage = JsonConvert.DeserializeObject<Results>(response.Content);
                        if (response.StatusCode == HttpStatusCode.Accepted)
                        {
                            msSQL = "update " + c_code + ".otl_trn_tkot set payment_message = 'Y', order_id = '" + order_id +"' where kot_gid = '" + order_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            string message = "Order Confirmed\n\nDear Customer,\n\nYour Order No is " + order_id + ". We will process your request and keep you posted on the progress.\n\nThank You";
                            msSQL = "insert into " + c_code + ".crm_trn_twhatsappmessages(" +
                                    "message_id," +
                                    "contact_id," +
                                    "direction," +
                                    "type," +
                                    "message_text," +
                                    "status," +
                                    "created_date)" +
                                    "values(" +
                                    "'" + objsendmessage.id + "'," +
                                    "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                    "'" + objsendmessage.direction + "'," +
                                    "'text'," +
                                    "'" + message.Replace("'", "\'") + "'," +
                                    "'" + objsendmessage.status + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + contactjson + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Update/" + "sendConfirmationMessage " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                }
                objOdbcDataReader.Close();

                
                msSQL = "select msgsend_manger from " + c_code + ".hrm_mst_tbranch where branch_gid = '" + branch_gid + "'";
                string manager_flag = objdbconn.GetExecuteScalar(msSQL);

                if (manager_message == "N" && manager_flag == "Y")
                {
                    msSQL = "select manager_number from " + c_code + ".hrm_mst_tbranch where branch_gid = '" + branch_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        manager_number = objOdbcDataReader["manager_number"].ToString();
                    }
                    objOdbcDataReader.Close();

                    msSQL = "select c.product_gid,a.product_quantity, " +
                            "case when c.product_gid is not null then c.product_name else (select product_name from " + c_code + ".pmr_mst_tproduct a " +
                            "left join " + c_code + ".otl_trn_branch2product b on a.product_gid = b.product_gid " +
                            "where b.branch2product_gid = a.kot_product_gid) end as product_name " +
                            "from " + c_code + ".otl_trn_tkotdtl a " +
                            "left join " + c_code + ".otl_trn_branch2product b on a.kot_product_gid = b.branch2product_gid " +
                            "left join " + c_code + ".pmr_mst_tproduct c on c.product_gid = a.kot_product_gid " +
                            "where a.kot_gid = '" + order_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable != null)
                    {
                        if (dt_datatable.Rows.Count > 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                products += dt["product_quantity"].ToString() + " " + dt["product_name"].ToString() + "\\n";
                            }
                        }
                    }

                    string text = "*New* *Order* *Received*\\n\\nPlease find below the order details\\n\\nRef No:*" + order_id + "*\\n\\nCustomer No:" + customer_no + "\\n\\n" + products + "\"";
                    if(text.Length > 4096)
                    {
                       text =  text.Substring(0, 4093) + "...";
                    }
                    string body = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + manager_number + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"" + text + "\"}}}";
                    whatsappconfiguration1 getwhatsappcredentials1 = whatsappcredentials(c_code, branch_gid);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials1.workspace_id + "/channels/" + getwhatsappcredentials1.channel_id + "/messages", Method.POST);
                    request1.AddHeader("authorization", "" + getwhatsappcredentials1.access_token + "");
                    request1.AddParameter("application/json", body.Replace("\"\"","\""), ParameterType.RequestBody);
                    IRestResponse response1 = client1.Execute(request1);
                    if (response1.StatusCode == HttpStatusCode.Accepted)
                    {
                        msSQL = "update " + c_code + ".otl_trn_tkot set manager_message = 'Y' where kot_gid = '" + order_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            objcmnfunctions.LogForAudit("Update failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/" + "sendConfirmationMessage " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("Error Occured while sending message to manager: " + body + " Error: " + response1.Content, "/WhatsAppOrders/ErrorLog/Update/" + "sendConfirmationMessage " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }

                msSQL = "select payment_gateway from " + c_code + ".adm_mst_tcompany";
                string payment_gateway = objdbconn.GetExecuteScalar(msSQL);

                if(payment_gateway == "STRIPE")
                {
                    paymentgatewayconfiguration1 getpaymentgatewaycredentials1 = paymentgatewaycredentials1(c_code);

                    string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(getpaymentgatewaycredentials1.stripe_key + ":"));
                    string token = "Basic " + encodedCredentials;

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client2 = new RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                    var request2 = new RestRequest("/v1/payment_links/" + payment_link, Method.POST);
                    request2.AddHeader("Authorization", token);
                    request2.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                    request2.AddParameter("active", "false");
                    IRestResponse response2 = client2.Execute(request2);
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        objcmnfunctions.LogForAudit("Payment link deactivated successfully: " + payment_link, "/WhatsAppOrders/sucessLog/sendConfirmationMessage.txt");
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("Payment link deactivation unsuccessful: " + payment_link, "/WhatsAppOrders/ErrorLog/sendConfirmationMessage.txt");
                    }
                }                
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("Exception Occured while sending confirmationmessage: " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Summary" + "sendConfirmationMessage " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                {
                    if (!objOdbcDataReader.IsClosed)
                        objOdbcDataReader.Close();
                }
            }
        }
        [ActionName("dynamicDBCreation")]
        [HttpPost]
        public HttpResponseMessage dynamicDBCreation(mdlDynamicDBCreation values)
        {
            result objValues = new result();
            string company_code = string.Empty;
            try
            {
                msSQL = "select register_gid,company_code,company_name,auth_email,authmobile_number,gst_number,company_address,contact_person " +
                        "from adm_mst_tcreatedb where otp='" + values.otp + "' and otp_expiry > '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    string consumer_gid = objOdbcDataReader["register_gid"].ToString();
                    string company_name = objOdbcDataReader["company_name"].ToString();
                    string mobile_number = objOdbcDataReader["authmobile_number"].ToString();
                    string gst = objOdbcDataReader["gst_number"].ToString();
                    string company_address = objOdbcDataReader["company_address"].ToString();
                    string contact_person = objOdbcDataReader["contact_person"].ToString();
                    string toaddress = objOdbcDataReader["auth_email"].ToString();
                    company_code = objOdbcDataReader["company_code"].ToString();
                    objOdbcDataReader.Close();
                    lssource_db = ConfigurationManager.AppSettings["source_db"];

                    // ----------------------------------DB CREATE-------------------------------------------------------
                    msSQL = "CREATE DATABASE " + company_code + "";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msSQL = "USE " + lssource_db + "";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "SELECT distinct table_name, table_schema, table_rows " +
                                "FROM information_schema.tables where table_schema='" + lssource_db + "'  and table_name not in ('crm_mst_teinvoiceregister','adm_mst_tconsumerdb','adm_trn_tconsumertoken') ORDER BY table_name";
                        objDataTable = objdbconn.GetDataTable(msSQL);
                        if (objDataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in objDataTable.Rows)
                            {
                                string tablename = row["table_name"].ToString();
                                string table_count = row["table_rows"].ToString();
                                msSQL = "create table " + company_code + "." + tablename + " like " + lssource_db + "." + tablename + "";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    msSQL = "insert into " + company_code + "." + tablename + " (select * from " + lssource_db + "." + tablename + ")";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }
                        }
                        msSQL = " USE " + lssource_db + "";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "select db,name,param_list,body from mysql.proc where db='" + lssource_db + "'";
                        objDataTable = objdbconn.GetDataTable(msSQL);
                        if (objDataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in objDataTable.Rows)
                            {
                                ////string db = row["db"].ToString();
                                ////string Name = row["name"].ToString();

                                ////byte[] paramBytes = (byte[])row["param_list"];
                                ////string parameter = Encoding.UTF8.GetString(paramBytes); 

                                ////byte[] param = (byte[])row["body"];
                                ////string body = Encoding.UTF8.GetString(param);

                                //msSQL = " USE " + company_code + "";
                                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                ////msSQL = "  create procedure " + Name + " ( " + parameter + " ) \n" + body + "$$" + "\n" + "DELIMITER ;";
                                ////mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                //string procedureName = row["name"].ToString(); ;
                                //byte[] paramBytes = (byte[])row["param_list"];
                                //string paramString = System.Text.Encoding.UTF8.GetString(paramBytes);

                                //// Add parentheses around the parameter string
                                //string parameters = "(" + paramString + ")";
                                //byte[] param = (byte[])row["body"];

                                //string body = "BEGIN"+ System.Text.Encoding.UTF8.GetString(paramBytes) + "END$$"; // make sure the body is properly defined

                                // msSQL = "CREATE PROCEDURE " + procedureName + " " + parameters + " " + body + "";
                                // mnResult =objdbconn.ExecuteNonQuerySQL(msSQL);

                                string db = row["db"].ToString();
                                string Name = row["name"].ToString();

                                byte[] paramBytes = (byte[])row["param_list"];
                                string parameter = Encoding.UTF8.GetString(paramBytes);

                                byte[] param = (byte[])row["body"];
                                string body = Encoding.UTF8.GetString(param);

                                msSQL = " USE " + company_code + "";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "CREATE PROCEDURE " + company_code + "." + Name + " (" + parameter + ") " + "\n" + body + "";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {

                                }

                            }
                        }
                        msSQL = " USE " + lssource_db + "";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " select view_definition,table_schema,table_name" +
                                " from INFORMATION_SCHEMA.VIEWS where table_schema='" + lssource_db + "'";
                        objDataTable = objdbconn.GetDataTable(msSQL);
                        if (objDataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in objDataTable.Rows)
                            {
                                string view_tablename = row["table_name"].ToString();
                                string view_definition = row["view_definition"].ToString();
                                msSQL = " USE " + company_code + "";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL = " create view " + company_code + "." + view_tablename + " as " + view_definition + " ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {

                                }
                            }
                        }
                        msSQL = " USE " + lssource_db + "";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " SELECT distinct table_name, table_schema" +
                                " FROM information_schema.tables where table_type='BASE TABLE' and table_schema='sample_data'" +
                                " ORDER BY table_name";
                        objDataTable = objdbconn.GetDataTable(msSQL);
                        if (objDataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in objDataTable.Rows)
                            {
                                string tablename = row["table_name"].ToString();
                                if (tablename == "acc_mst_tcash" | tablename == "adm_mst_tcompany" | tablename == "adm_mst_tmodule")
                                {
                                    objValues.message = "If Table Already Exists.";
                                }
                                else
                                {
                                    string qry = "DROP TABLE IF EXISTS `" + company_code + "`.`" + tablename + "`";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(qry);

                                }

                                if (mnResult == 1)
                                {
                                    msSQL = "insert into " + company_code + "." + tablename + " (select * from sample_data." + tablename + ")";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }
                        }
                        if (mnResult == 1)
                        {
                            objValues.status = true;
                            objValues.message = "Setup Complete. Kindly login using the credentials sent to the registered email ID";


                            // ----------------------------------Capture Connection String-------------------------------------------------------
                            string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";

                            msSQL = "select company_code " +
                                    "from adm_mst_tconsumerdb where company_code='" + company_code + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (!objOdbcDataReader.HasRows)
                            {
                                msSQL = "insert into adm_mst_tconsumerdb(" +
                                        "consumer_gid," +
                                        "company_code," +
                                        "server_name," +
                                        "db_name," +
                                        "user_name," +
                                        "password," +
                                        "connection_string," +
                                        "created_date) values (" +
                                        "'" + consumer_gid + "'," +
                                        "'" + company_code + "'," +
                                        "'" + ConfigurationManager.AppSettings["DBserver"] + "'," +
                                        "'" + company_code + "'," +
                                        "'" + ConfigurationManager.AppSettings["DBUID"] + "'," +
                                        "'" + ConfigurationManager.AppSettings["DBpwd"] + "'," +
                                        "'" + connectionString + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    msSQL = "update " + company_code + ".adm_mst_tcompany set company_code ='" + company_code + "'," +
                                            "company_name='" + company_name + "'," +
                                            "company_phone='" + mobile_number + "'," +
                                            "gst_no='" + gst + "'," +
                                            "company_address='" + company_address + "'," +
                                            "contact_person='" + contact_person + "'," +
                                            "company_mail='" + toaddress + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        msSQL = "update " + company_code + ".adm_mst_tuser set user_code ='admin', user_password='" + objcmnfunctions.ConvertToAscii("Welcome@123") + "';";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            //ativationMailtrigger(company_code, "admin", "Welcome@123", toaddress, contact_person);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objValues.status = false;
                                objValues.message = " Already Registered, Kindly Check Your mail...";
                            }
                            // ----------------------------------Capture Connection String-------------------------------------------------------
                        }
                    }
                    else
                    {
                        objValues.status = false;
                        objValues.message = "Error occured while setting up environment. Please try again!";
                    }

                    // ----------------------------------DB CREATE-------------------------------------------------------
                }
                else
                {
                    objValues.status = false;
                    objValues.message = "Invalid OTP/OTP has been expired.";
                }
            }
            catch (Exception ex)
            {
                objValues.status = false;
                objValues.message = "Exception occured while setting up environment! Please try again.";
                msSQL = "drop database " + company_code + ";";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                objcmnfunctions.LogForAudit(ex.ToString());
            }
            return Request.CreateResponse(HttpStatusCode.OK, objValues);
        }

        [HttpPost]
        [ActionName("resetpasswordcheck")]
        public HttpResponseMessage resetpasswordcheck(PostUserReset values)
        {

            msSQL = " SELECT  user_password from " + values.companyid_reset + ".adm_mst_tuser  where user_code = '" + values.usercode_reset + "' ";
            string old_password = objdbconn.GetExecuteScalar(msSQL);

            string oldpassword = objcmnfunctions.ConvertToAscii(values.old_password);
            if (oldpassword == old_password)
            {
                values.status = true;
                values.message = "Password Verified Successfully";
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            else
            {
                values.status = false;
                values.message = "Invalid Password!";
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
        }
        [ActionName("DynamicDBcreationInSQLFiles")]
        [HttpPost]
        public HttpResponseMessage DynamicDBcreationInSQLFiles(mdlDynamicDBCreation values)
        {
            result objValues = new result();
            HttpContext ctx = HttpContext.Current;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                HttpContext.Current = ctx;
                DaDynamicDBcreationInSQLFiles(values);
            }));
            t.Start();
            objValues.status = true;
            objValues.message = "";
            return Request.CreateResponse(HttpStatusCode.OK, objValues);
        }
        public void DaDynamicDBcreationInSQLFiles(mdlDynamicDBCreation values)
        {
            result objValues = new result();
            msSQL = "select register_gid,company_code,company_name,auth_email,authmobile_number,gst_number,company_address,contact_person " +
                       "from adm_mst_tcreatedb where otp='" + values.otp + "' and otp_expiry > '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                objOdbcDataReader.Read();
                string consumer_gid = objOdbcDataReader["register_gid"].ToString();
                string company_name = objOdbcDataReader["company_name"].ToString();
                string mobile_number = objOdbcDataReader["authmobile_number"].ToString();
                string gst = objOdbcDataReader["gst_number"].ToString();
                string company_address = objOdbcDataReader["company_address"].ToString();
                string contact_person = objOdbcDataReader["contact_person"].ToString();
                string toaddress = objOdbcDataReader["auth_email"].ToString();
                company_code = objOdbcDataReader["company_code"].ToString();
                objOdbcDataReader.Close();
                lssource_db = ConfigurationManager.AppSettings["source_db"];

                // ----------------------------------DB CREATE-------------------------------------------------------
                msSQL = "CREATE DATABASE " + company_code + "";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    lsSQLpath = ConfigurationManager.AppSettings["SQlpath"].ToString();
                    var SQLfiles = Directory.GetFiles(lsSQLpath, "*.sql");
                    foreach (var createstatement in SQLfiles)
                    {
                        GetFilesinPath = System.IO.File.ReadAllText(createstatement);
                        if (createstatement.EndsWith("SP.sql"))
                        {
                            Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` PROCEDURE", "CREATE PROCEDURE").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + company_code + "`." + "");
                            createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        else if (createstatement.EndsWith("Functions.sql"))
                        {
                            Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` FUNCTION", "CREATE FUNCTION").Replace("CREATE FUNCTION", "CREATE FUNCTION `" + company_code + "`." + "");
                            createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        else if (createstatement.EndsWith("View.sql"))
                        {
                            Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                            createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        else if (createstatement.EndsWith("kotTables.sql"))
                        {
                            Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + company_code + "`." + "");
                            createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        foreach (var tables in createtables)
                        {
                            if (tables.Trim() != "")
                            {
                                msSQL = tables.Trim();
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }

                    if (mnResult == 1)
                    {
                        string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                        "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                        "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";
                        msSQL = "select company_code " +
                        "from storyboard.adm_mst_tconsumerdb where company_code='" + company_code + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (!objOdbcDataReader.HasRows)
                        {
                            msSQL = "insert into storyboard.adm_mst_tconsumerdb(" +
                                    "consumer_gid," +
                                    "company_code," +
                                    "server_name," +
                                    "db_name," +
                                    "user_name," +
                                    "password," +
                                    "connection_string," +
                                    "created_date, created_by) values (" +
                                    "'" + consumer_gid + "'," +
                                    "'" + company_code + "'," +
                                    "'" + ConfigurationManager.AppSettings["DBserver"] + "'," +
                                    "'" + company_code + "'," +
                                    "'" + ConfigurationManager.AppSettings["DBUID"] + "'," +
                                    "'" + ConfigurationManager.AppSettings["DBpwd"] + "'," +
                                    "'" + connectionString + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    "'U1')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = " insert into " + company_code + " .adm_mst_tcompany (" +
                                    "company_code," +
                                    "company_name," +
                                    "company_phone," +
                                    "company_address," +
                                    "contact_person," +
                                    "company_mail," +
                                    "auth_code," +
                                    "pop_password" +
                                    " ) values ( " +
                                    "'" + company_code + "'," +
                                    "'" + company_name + "'," +
                                    "'" + mobile_number + "'," +
                                    "'" + company_address + "'," +
                                    "'" + contact_person + "'," +
                                    "'" + toaddress + "'," +
                                    "''," +
                                    "'')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    msSQL = "insert into " + company_code + ".adm_mst_tuser(" +
                                           "user_gid," +
                                           "user_code," +
                                           "user_firstname," +
                                           "user_password," +
                                           "user_status" +
                                           " ) values (" +
                                           "'U1'," +
                                           "'admin'," +
                                           "'admin'," +
                                           "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                           "'Y')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                                            "employee_gid," +
                                            "user_gid" +
                                            " )values(" +
                                            "'E1'," +
                                            "'U1')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            lsSQLpath = ConfigurationManager.AppSettings["SQlMetaData"].ToString();
                                            var InsertFiles = Directory.GetFiles(lsSQLpath, "*.sql");
                                            foreach (var insertquery in InsertFiles)
                                            {
                                                inserpath = System.IO.File.ReadAllText(insertquery);

                                                string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`." + "");
                                                string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var insert in metadatasforinsert)
                                                {
                                                    if (insert.Trim() != "")
                                                    {
                                                        msSQL = insert.Trim();
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    }
                                                }
                                            }

                                            if (mnResult == 1)
                                            {
                                                objcmnfunctions.activationMailtrigger(company_code, "admin", "Welcome@123", toaddress, contact_person);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objValues.status = false;
                            objValues.message = " Already Registered, Kindly Check Your mail...";
                        }
                    }
                }
            }
        }
        [ActionName("DynamicDBcreationSP")]
        [HttpPost]
        public HttpResponseMessage DynamicDBcreationSP(mdlDynamicDBCreation values)
        {
            result objValues = new result();
            msSQL = "select register_gid,company_code,company_name,auth_email,authmobile_number,gst_number,company_address,contact_person " +
                       "from adm_mst_tcreatedb where otp='" + values.otp + "' and otp_expiry > '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                objOdbcDataReader.Read();
                string consumer_gid = objOdbcDataReader["register_gid"].ToString();
                string company_name = objOdbcDataReader["company_name"].ToString();
                string mobile_number = objOdbcDataReader["authmobile_number"].ToString();
                string gst = objOdbcDataReader["gst_number"].ToString();
                string company_address = objOdbcDataReader["company_address"].ToString();
                string contact_person = objOdbcDataReader["contact_person"].ToString();
                string toaddress = objOdbcDataReader["auth_email"].ToString();
                company_code = objOdbcDataReader["company_code"].ToString();
                objOdbcDataReader.Close();
                lssource_db = ConfigurationManager.AppSettings["source_db"];

                // ----------------------------------DB CREATE-------------------------------------------------------
                msSQL = "CREATE DATABASE " + company_code + "";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    msSQL = "call acc_mst_spcreatestatement('" + company_code + "')";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        msSQL = "call ";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                            "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                            "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";
                            msSQL = "select company_code " +
                            "from adm_mst_tconsumerdb where company_code='" + company_code + "'";
                            if (!objOdbcDataReader.HasRows)
                            {
                                msSQL = "insert into adm_mst_tconsumerdb(" +
                                        "consumer_gid," +
                                        "company_code," +
                                        "server_name," +
                                        "db_name," +
                                        "user_name," +
                                        "password," +
                                        "connection_string," +
                                        "created_date) values (" +
                                        "'" + consumer_gid + "'," +
                                        "'" + company_code + "'," +
                                        "'" + ConfigurationManager.AppSettings["DBserver"] + "'," +
                                        "'" + company_code + "'," +
                                        "'" + ConfigurationManager.AppSettings["DBUID"] + "'," +
                                        "'" + ConfigurationManager.AppSettings["DBpwd"] + "'," +
                                        "'" + connectionString + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    msSQL = "update " + company_code + ".adm_mst_tcompany set company_code ='" + company_code + "'," +
                                            "company_name='" + company_name + "'," +
                                            "company_phone='" + mobile_number + "'," +
                                            "gst_no='" + gst + "'," +
                                            "company_address='" + company_address + "'," +
                                            "contact_person='" + contact_person + "'," +
                                            "company_mail='" + toaddress + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        msSQL = "update " + company_code + ".adm_mst_tuser set user_code ='admin', user_password='" + objcmnfunctions.ConvertToAscii("Welcome@123") + "';";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            //ativationMailtrigger(company_code, "admin", "Welcome@123", toaddress, contact_person);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objValues.status = false;
                                objValues.message = " Already Registered, Kindly Check Your mail...";
                            }
                        }
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, objValues);
        }
        public paymentgatewayconfiguration1 paymentgatewaycredentials1(string c_code)
        {
            paymentgatewayconfiguration1 getpaymentgatewaycredentials1 = new paymentgatewayconfiguration1();
            try
            {


                msSQL = " SELECT CASE WHEN payment_gateway = 'RAZORPAY' THEN key1 ELSE NULL END AS key1,key2,CASE WHEN payment_gateway = 'STRIPE' THEN key1 ELSE NULL END AS key3,payment_gateway FROM " + c_code + ".adm_Mst_tcompany LIMIT 1; ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {

                    getpaymentgatewaycredentials1.razorpay_accname = objOdbcDataReader["key1"].ToString();
                    getpaymentgatewaycredentials1.razorpay_accpwd = objOdbcDataReader["key2"].ToString();
                    getpaymentgatewaycredentials1.stripe_key = objOdbcDataReader["key3"].ToString();
                    getpaymentgatewaycredentials1.payment_gateway = objOdbcDataReader["payment_gateway"].ToString();

                }

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Configuration/" + "DaWhatsappproductpricemanagement.cs " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

            return getpaymentgatewaycredentials1;
        }

        [HttpPost]
        [ActionName("cancelOrder")]
        public HttpResponseMessage cancelOrder(MdlOrderDispatchMode values)
        {
            try
            {
                values.c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
                msSQL = "update " + values.c_code + ".otl_trn_tkot set order_status = 'CANCELLED' where kot_gid = '" + values.order_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                }
                else
                {
                    objcmnfunctions.LogForAudit("Update Failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/CancelOrder.txt");
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured: " + ex.ToString();
                objcmnfunctions.LogForAudit("Exception occured while updating payment status: " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Update/CancelOrder.txt");
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [HttpPost]
        [ActionName("updatePaymentMode")]
        public HttpResponseMessage updatePaymentMode(MdlOrderDispatchMode values)
        {
            try
            {
                values.c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
                msSQL = "update " + values.c_code + ".otl_trn_tkot set payment_method = '" + values.payment_mode + "',payment_link = '-',order_status = 'CONFIRMED' where kot_gid = '" + values.order_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    {
                        HttpContext.Current = ctx;
                        sendConfirmationMessage(values.order_gid, values.c_code);
                    }));
                    t.Start();
                    values.status = true;

                }
                else
                {
                    objcmnfunctions.LogForAudit("Update Failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/CancelOrder.txt");
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured: " + ex.ToString();
                objcmnfunctions.LogForAudit("Exception occured while updating payment status: " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Update/CancelOrder.txt");
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [HttpPost]
        [ActionName("createPaymentLinkRP")]
        public HttpResponseMessage createPaymentLinkRP(MdlOrderDispatchMode values)
        {
            try
            {
                string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();

                msSQL = "select kot_tot_price from " + c_code + ".otl_trn_tkot where kot_gid = '" + values.order_gid + "'";
                double amount = double.Parse(objdbconn.GetExecuteScalar(msSQL) != "" ? objdbconn.GetExecuteScalar(msSQL) : "0.00");

                if (amount == 0)
                {
                    values.message = "Error occured while generating payment link: Order values is 0 for the order: " + values.order_gid;
                    return Request.CreateResponse(HttpStatusCode.OK, values);
                }

                string request_ID = objcmnfunctions.GetMasterGID("RQST", c_code, "", "");
                paymentgatewayconfiguration1 getpaymentgatewaycredentials1 = paymentgatewaycredentials1(c_code);
                string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(getpaymentgatewaycredentials1.razorpay_accname + ":" + getpaymentgatewaycredentials1.razorpay_accpwd));
                string token = "Basic " + encodedCredentials;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["RAZORPAY_BASE_URL"]);
                var request = new RestRequest("/v1/payment_links", Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", token);
                string body = "{\"upi_link\": " + ConfigurationManager.AppSettings["UPI_ENABLE"] + ", \"accept_partial\": false,\"amount\": " + amount * 100 + ",\"currency\": \"INR\",\"reference_id\": \"" + request_ID + "\",\"reminder_enable\": false,\"callback_url\": \"" + ConfigurationManager.AppSettings["paymentredirect_URL"].ToString() + objFnazurestorage.EncryptData(values.order_gid) + "&v2=" + objFnazurestorage.EncryptData(c_code) + "\",\"callback_method\": \"get\"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    MdlRazorpayResponse objMdlRazorpayResponse = JsonConvert.DeserializeObject<MdlRazorpayResponse>(response.Content);
                    values.payment_link = objMdlRazorpayResponse.short_url.Replace("https://rzp.io/", "");
                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    {
                        HttpContext.Current = ctx;
                        updatePaymentLink(c_code, objMdlRazorpayResponse.id, values.order_gid);
                    }));
                    t.Start();
                    values.status = true;
                }
                else 
                { 
                    values.message = "Error occured while genreating link: " + response.Content.ToString();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured: " + ex.ToString();
                objcmnfunctions.LogForAudit("Exception occured while updating payment status: " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Update/CancelOrder.txt");
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        public bool updatePaymentLink(string c_code,string payment_link,string order_gid)
        {
            try
            {
                msSQL = "update " + c_code + ".otl_trn_tkot " +
                                    "set payment_link = '" + payment_link + "' " +
                                    "where kot_gid = '" + order_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    objcmnfunctions.LogForAudit("Update failed: " + msSQL, "/WhatsAppOrders/ErrorLog/Update/createPaymentLinkRP.txt");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                objcmnfunctions.LogForAudit("Exception occured while updating payment link: " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Update/createPaymentLinkRP.txt");
                return false;
            }
        }

        [HttpPost]
        [ActionName("pincodeValidation")]
        public HttpResponseMessage checkPincode(MdlOrderDispatchMode values)
        {
            try
            {
                double total = 0.00;
                string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
                if(values.pin_code == null)
                    return Request.CreateResponse(HttpStatusCode.OK, values);

                msSQL = "select kot_tot_price from " + c_code + ".otl_trn_tkot where kot_gid = '" + values.order_gid + "'";
                objODBCdatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCdatareader.HasRows)
                {
                    total = double.Parse(objODBCdatareader["kot_tot_price"].ToString());
                }

                values.pin_code = values.pin_code != null ? values.pin_code.Replace(" ", "") : values.pin_code;

                if(c_code == "apna_tiffin")
                {
                    if(values.pin_code.Length == 6)
                        msSQL = "select pincode_code from " + c_code + ".adm_mst_tpincode where pincode_code like '" + values.pin_code.Substring(0,3) + "%'";
                    else if(values.pin_code.Length == 7)
                        msSQL = "select pincode_code from " + c_code + ".adm_mst_tpincode where pincode_code like '" + values.pin_code.Substring(0, 4) + "%'";
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, values);

                    objODBCdatareader = objdbconn.GetDataReader(msSQL);
                    if (objODBCdatareader.HasRows)
                    {
                        values.status = true;
                        HttpContext ctx = HttpContext.Current;
                        System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                        {
                            HttpContext.Current = ctx;
                            fnWhatsAppMessage1(values.customer_message);
                        }));
                        t.Start();
                    }
                   
                }
                else if(c_code == ConfigurationManager.AppSettings["pincodevalidationKey"].ToString())
                {
                    msSQL = "select pincode_code,format(deliverycost,2) as deliverycost from " + c_code + ".adm_mst_tpincode limit 1";
                    objODBCdatareader = objdbconn.GetDataReader(msSQL);
                    if (objODBCdatareader.HasRows)
                    {
                        values.delivery_cost = double.Parse(objODBCdatareader["deliverycost"].ToString()).ToString();
                        total = total + double.Parse(values.delivery_cost);
                        msSQL = "update " + c_code + ".otl_trn_tkot set kot_tot_price = '" + total + "', kot_delivery_charges = '" + double.Parse(values.delivery_cost) + "' where kot_gid = '" + values.order_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            HttpContext ctx = HttpContext.Current;
                            System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                            {
                                HttpContext.Current = ctx;
                                fnWhatsAppMessage1(values.customer_message);
                            }));
                            t.Start();
                        }
                    }
                    else
                    {
                        values.message = "No such pin code exists.";
                    }
                }
                else
                {
                    msSQL = "select pincode_code,format(deliverycost,2) as deliverycost from " + c_code + ".adm_mst_tpincode where pincode_code = '" + values.pin_code + "'";
                    objODBCdatareader = objdbconn.GetDataReader(msSQL);
                    if (objODBCdatareader.HasRows)
                    {
                        values.delivery_cost = double.Parse(objODBCdatareader["deliverycost"].ToString()).ToString();
                        total = total + double.Parse(values.delivery_cost);
                        msSQL = "update " + c_code + ".otl_trn_tkot set kot_tot_price = '" + total + "', kot_delivery_charges = '" + double.Parse(values.delivery_cost) + "' where kot_gid = '" + values.order_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            HttpContext ctx = HttpContext.Current;
                            System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                            {
                                HttpContext.Current = ctx;
                                fnWhatsAppMessage1(values.customer_message);
                            }));
                            t.Start();
                        }
                    }
                    else
                    {
                        values.message = "No such pin code exists.";
                    }
                }                             
            }
            catch (Exception ex)
            {
                values.message = "Exception occured: " + ex.ToString();
                objcmnfunctions.LogForAudit("Exception occured while updating payment status: " + ex.ToString(), "/WhatsAppOrders/ErrorLog/Update/CancelOrder.txt");
            }
            finally
            {
                if(objODBCdatareader != null) 
                    objODBCdatareader.Close();
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [HttpPost]
        [ActionName("WhatsAppMessage")]
        public HttpResponseMessage WhatsAppMessage(mdlIncomingMessage values)
        {
            result objresult = new result();
            try
            {
                objresult = fnWhatsAppMessage(values);
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("Exception occured: " + ex.ToString(), "");
            }
            finally
            {
                if (objODBCdatareader != null)
                    objODBCdatareader.Close();
            }
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        public result fnWhatsAppMessage(mdlIncomingMessage values)
        {
            result objresult = new result();
            try
            {
                string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
                string type = string.Empty, mediaURL = string.Empty;
                if (values.message.body.type == "location")
                {
                    msSQL = "insert into " + c_code + ".crm_trn_twhatsappmessages(" +
                            "message_id," +
                            "contact_id," +
                            "direction," +
                            "type," +
                            "message_text," +
                            "lat," +
                            "lon," +
                            "created_date)" +
                            "values(" +
                            "'" + values.message.messageId + "'," +
                            "'" + values.message.sender.contact.contactId + "'," +
                            "'incoming'," +
                            "'" + values.message.body.type + "'," +
                            "'Location'," +
                            "'" + values.message.body.location.coordinations.latitude + "'," +
                            "'" + values.message.body.location.coordinations.longitude + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                }
                else
                {
                    msSQL = "insert into " + c_code + ".crm_trn_twhatsappmessages(" +
                        "message_id," +
                        "contact_id," +
                        "direction," +
                        "type," +
                        "message_text," +
                        "content_type," +
                        "status," +
                        "created_date)" +
                        "values(" +
                        "'" + values.message.conversationMessageId + "'," +
                        "'" + values.message.sender.participant.participantId + "'," +
                        "'incoming'," +
                        "'" + values.message.body.type + "',";
                    if (values.message.body.type == "text")
                    {
                        type = "text";
                        string text = values.message.body.text.text == null ? "" : values.message.body.text.text.Replace("'", "\\'");
                        msSQL += "'" + values.message.body.text.text + "'," +
                                 "null,";
                    }
                    else if (values.message.body.type == "image")
                    {
                        type = "image";
                        mediaURL = values.message.body.image.images[0].mediaUrl;
                        msSQL += "'Image'," +
                                 "null,";
                    }
                    else if (values.message.body.type == "list")
                    {
                        type = "list";
                        msSQL += "'List'," +
                                 "null,";
                    }
                    else
                    {
                        type = "file";
                        msSQL += "'File'," +
                                 "'" + values.message.body.file.files[0].contentType + "',";
                    }
                    msSQL += "'" + values.message.status + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                }
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    objresult.status = true;
                    objresult.message = "Success";
                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    {
                        HttpContext.Current = ctx;
                        createWAContact(values, c_code);
                        downloadWAFiles(values, type, c_code);
                    }));
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Exception occured: " + ex.ToString();
            }
            return objresult;
        }

        public void downloadWAFiles(mdlIncomingMessage values, string type, string c_code)
        {
            try
            {
                string branch_gid = getBranchGID(values.message.channelId,c_code);
                whatsappconfiguration1 getwhatsappcredentials = whatsappcredentials(c_code, branch_gid);
                if (type == "image")
                {
                    foreach (var item in values.message.body.image.images)
                    {
                        string ext, filename, lspath, filepath = "", lspath1 = "/erp_documents/" + c_code + "/WOS/IMAGES/" + values.message.sender.participant.participantId + "/";
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["messageBirdMediaURL"].ToString());
                        var request = new RestRequest(item.mediaUrl.Replace(ConfigurationManager.AppSettings["messageBirdMediaURL"].ToString(), ""), Method.GET);
                        request.AddHeader("authorization", getwhatsappcredentials.access_token);
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            filename = response.Headers.FirstOrDefault(h => h.Name.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase))?.Value.ToString().Replace("inline; filename=\"", "").Replace("\"", "");
                            ext = System.IO.Path.GetExtension(filename).ToLower();
                            string file_gid = objcmnfunctions.GetMasterGID("UPLF", c_code, "", "");
                            filepath = lspath1 + file_gid + ext;
                            lspath = ConfigurationManager.AppSettings["file_path"].ToString() + lspath1 + file_gid + ext;
                            // Save the content of the response to the specified local file
                            if ((!System.IO.Directory.Exists(ConfigurationManager.AppSettings["file_path"].ToString() + lspath1)))
                                System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["file_path"].ToString() + lspath1);
                            File.WriteAllBytes(lspath, response.RawBytes);

                            msSQL = "insert into " + c_code + ".crm_trn_tfiles(" +
                                    "file_gid," +
                                    "message_gid," +
                                    "contact_gid," +
                                    "document_name," +
                                    "document_path)values(" +
                                    "'" + file_gid + "'," +
                                    "'" + values.message.conversationMessageId + "'," +
                                    "'" + values.message.sender.participant.participantId + "'," +
                                    "'" + filename.Replace("'", "\\'") + "'," +
                                    "'" + filepath.Replace("'", "\\'") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }
                    }
                }
                else if (type == "file")
                {
                    foreach (var item in values.message.body.file.files)
                    {
                        string ext, filename, lspath, filepath = "", lspath1 = "/erp_documents/" + c_code + "/WOS/FILES/" + values.message.sender.participant.participantId + "/";
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["messageBirdMediaURL"].ToString());
                        var request = new RestRequest(item.mediaUrl.Replace(ConfigurationManager.AppSettings["messageBirdMediaURL"].ToString(), ""), Method.GET);
                        request.AddHeader("authorization", getwhatsappcredentials.access_token);
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            filename = response.Headers.FirstOrDefault(h => h.Name.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase))?.Value.ToString().Replace("inline; filename=\"", "").Replace("\"", "");
                            ext = System.IO.Path.GetExtension(filename).ToLower();
                            string file_gid = objcmnfunctions.GetMasterGID("UPLF", c_code, "", "");
                            filepath = lspath1 + file_gid + ext;
                            lspath = ConfigurationManager.AppSettings["file_path"].ToString() + lspath1 + file_gid + ext;
                            // Save the content of the response to the specified local file
                            if ((!System.IO.Directory.Exists(ConfigurationManager.AppSettings["file_path"].ToString() + lspath1)))
                                System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["file_path"].ToString() + lspath1);
                            File.WriteAllBytes(lspath, response.RawBytes);

                            msSQL = "insert into " + c_code + ".crm_trn_tfiles(" +
                                    "file_gid," +
                                    "message_gid," +
                                    "contact_gid," +
                                    "document_name," +
                                    "document_path)values(" +
                                    "'" + file_gid + "'," +
                                    "'" + values.message.conversationMessageId + "'," +
                                    "'" + values.message.sender.participant.participantId + "'," +
                                    "'" + filename.Replace("'", "\\'") + "'," +
                                    "'" + filepath.Replace("'", "\\'") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString() , "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void createWAContact(mdlIncomingMessage values, string c_code)
        {
            try
            {
                string branch_gid = getBranchGID(values.message.channelId, c_code);
                whatsappconfiguration1 getwhatsappcredentials = whatsappcredentials(c_code, branch_gid);
                msSQL = "select id from " + c_code + ". crm_smm_whatsapp where id ='" + values.message.sender.participant.participantId + "'";
                string contact_id = objdbconn.GetExecuteScalar(msSQL);
                if (contact_id != values.message.sender.participant.participantId)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/contacts/" + values.message.sender.participant.participantId, Method.GET);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                    IRestResponse response = client.Execute(request);
                    MdlContactResponse objMdlContactResponse = JsonConvert.DeserializeObject<MdlContactResponse>(response.Content);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        msSQL = "insert into " + c_code + ".crm_smm_whatsapp(" +
                                "id," +
                                "displayName," +
                                "wkey," +
                                "wvalue," +
                                "created_date )" +
                                "values(" +
                                "'" + values.message.sender.participant.participantId + "'," +
                                "'New Customer'," +
                                "'" + objMdlContactResponse.featuredIdentifiers[0].key + "'," +
                                "'" + objMdlContactResponse.featuredIdentifiers[0].value + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
            }
            catch(Exception ex)
            {
                objcmnfunctions.LogForAudit("Exception occured while posting contact to BIRD: " + ex.ToString(), "");
            }
            
        }

        public result fnWhatsAppMessage1(Message values)
        {
            result objresult = new result();
            try
            {
                string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
                string type = string.Empty, mediaURL = string.Empty;
                if (values.body.type == "location")
                {
                    msSQL = "insert into " + c_code + ".crm_trn_twhatsappmessages(" +
                            "message_id," +
                            "contact_id," +
                            "direction," +
                            "type," +
                            "message_text," +
                            "lat," +
                            "lon," +
                            "created_date)" +
                            "values(" +
                            "'" + values.messageId + "'," +
                            "'" + values.sender.contact.contactId + "'," +
                            "'incoming'," +
                            "'" + values.body.type + "'," +
                            "'Location'," +
                            "'" + values.body.location.coordinations.latitude + "'," +
                            "'" + values.body.location.coordinations.longitude + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                }
                else
                {
                    msSQL = "insert into " + c_code + ".crm_trn_twhatsappmessages(" +
                        "message_id," +
                        "contact_id," +
                        "direction," +
                        "type," +
                        "message_text," +
                        "content_type," +
                        "status," +
                        "created_date)" +
                        "values(" +
                        "'" + values.conversationMessageId + "'," +
                        "'" + values.sender.participant.participantId + "'," +
                        "'incoming'," +
                        "'" + values.body.type + "',";
                    if (values.body.type == "text")
                    {
                        type = "text";
                        string text = values.body.text.text == null ? "" : values.body.text.text.Replace("'", "\\'");
                        msSQL += "'" + values.body.text.text + "'," +
                                 "null,";
                    }
                    else if (values.body.type == "image")
                    {
                        type = "image";
                        mediaURL = values.body.image.images[0].mediaUrl;
                        msSQL += "'Image'," +
                                 "null,";
                    }
                    else if (values.body.type == "list")
                    {
                        type = "list";
                        msSQL += "'List'," +
                                 "null,";
                    }
                    else
                    {
                        type = "file";
                        msSQL += "'File'," +
                                 "'" + values.body.file.files[0].contentType + "',";
                    }
                    msSQL += "'" + values.status + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                }
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    objresult.status = true;
                    objresult.message = "Success";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Exception occured: " + ex.ToString();
            }
            return objresult;
        }

        [HttpPost]
        [ActionName("oruspoon_paymentlink")]
        public HttpResponseMessage oruspoon_paymentlink(MdlOrderDispatchMode values)
        {
            try
            {
                string c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
                string lsexistingdeliverycharges = "", lstotalcharges = "";
                int FreeChargesLimit = int.Parse(ConfigurationManager.AppSettings["oruspoon_quantity"]);
                msSQL = " select sum(product_quantity) as QuantityCheck from " + c_code + ".otl_trn_tkotdtl " +
                        " where kot_gid ='" + values.order_gid + "'";
                string lsQuantityCheck = objdbconn.GetExecuteScalar(msSQL);
                int lsquantity = int.Parse(lsQuantityCheck);
                if (!string.IsNullOrEmpty(lsQuantityCheck))
                {                    
                    if (lsquantity < FreeChargesLimit)
                    {
                        string additionalCharges = ConfigurationManager.AppSettings["oruspoon_delivery_cost"];
                        msSQL = " select kot_delivery_charges,kot_tot_price from " + c_code + ".otl_trn_tkot " +
                                " where kot_gid ='" + values.order_gid + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsexistingdeliverycharges = objOdbcDataReader["kot_delivery_charges"].ToString();
                            lstotalcharges = objOdbcDataReader["kot_tot_price"].ToString();
                        }
                        // Calculate the new delivery charges and total price
                        string newDeliveryCharges = (Convert.ToDecimal(lsexistingdeliverycharges) + Convert.ToDecimal(additionalCharges)).ToString();
                        string newTotalPrice = (Convert.ToDecimal(lstotalcharges) + Convert.ToDecimal(additionalCharges)).ToString();
                        msSQL = " UPDATE " + c_code + ".otl_trn_tkot SET kot_delivery_charges = '" + newDeliveryCharges + "'," +
                                " kot_tot_price = '" + newTotalPrice + "'" +
                                " WHERE kot_gid = '" + values.order_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    msSQL = "select kot_tot_price from " + c_code + ".otl_trn_tkot where kot_gid = '" + values.order_gid + "'";
                    double amount = double.Parse(objdbconn.GetExecuteScalar(msSQL) != "" ? objdbconn.GetExecuteScalar(msSQL) : "0.00");
                    if (amount == 0)
                    {
                        values.message = "Error occured while generating payment link: Order values is 0 for the order: " + values.order_gid;
                        return Request.CreateResponse(HttpStatusCode.OK, values);
                    }
                    string request_ID = objcmnfunctions.GetMasterGID("RQST", c_code, "", "");
                    paymentgatewayconfiguration1 getpaymentgatewaycredentials1 = paymentgatewaycredentials1(c_code);
                    string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(getpaymentgatewaycredentials1.razorpay_accname + ":" + getpaymentgatewaycredentials1.razorpay_accpwd));
                    string token = "Basic " + encodedCredentials;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["RAZORPAY_BASE_URL"]);
                    var request = new RestRequest("/v1/payment_links", Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", token);
                    string body = "{\"upi_link\": " + ConfigurationManager.AppSettings["UPI_ENABLE"] + ", \"accept_partial\": false,\"amount\": " + amount * 100 + ",\"currency\": \"INR\",\"reference_id\": \"" + request_ID + "\",\"reminder_enable\": false,\"callback_url\": \"" + ConfigurationManager.AppSettings["paymentredirect_URL"].ToString() + objFnazurestorage.EncryptData(values.order_gid) + "&v2=" + objFnazurestorage.EncryptData(c_code) + "\",\"callback_method\": \"get\"}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        MdlRazorpayResponse objMdlRazorpayResponse = JsonConvert.DeserializeObject<MdlRazorpayResponse>(response.Content);
                        values.payment_link = objMdlRazorpayResponse.short_url.Replace("https://rzp.io/", "");
                        HttpContext ctx = HttpContext.Current;
                        System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                        {
                            HttpContext.Current = ctx;
                            updatePaymentLink(c_code, objMdlRazorpayResponse.id, values.order_gid);
                        }));
                        t.Start();
                        values.status = true;
                        values.message = "Delivery Charges and Total Price updated Successfully!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Provided Kot details doesn't available!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = ex.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
