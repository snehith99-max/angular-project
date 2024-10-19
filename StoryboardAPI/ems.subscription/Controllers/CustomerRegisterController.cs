using ems.subscription.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using Razorpay.Api;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Stripe;
using Newtonsoft.Json;
using RestSharp;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.Http.Results;

namespace ems.subscription.Controllers
{
    [RoutePrefix("api/CustomerRegister")]
    [AllowAnonymous]
    public class CustomerRegisterController : ApiController
    {
        dbconn objdbconn = new dbconn();

        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objODBCdatareader, objOdbcDataReader, objODBCDataReader, odbcDataReader;
        DataTable dt_datatable;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        int mnResult, ls_port, mnresult;
        string[] createtables;
        string ls_server, customer_code, ls_password, ls_username, company_code, lsSQLpath, Scripts, GetFilesinPath, lshostingconnectionstring, lsviewhostingconnectionstring;
        string msGetGid, plan_flag, plan_price, currency, msGetGid2, inserpath, msGetGid3, lshosting_details,lsserver_gid, lsexpiry_time;
        string lscompany_code, lssource_db, c_code, lsregister_gid, lssignupcompany_code, lsBccmail_id, lsto_mail;
        private readonly HttpClient _httpClient;
        string domain = string.Empty;
        public string[] lsBCCReceipients;
        public string[] lsToReceipients;

        public CustomerRegisterController()
        {
            _httpClient = new HttpClient();
        }

        //-----Register API common for Razorpay and Stripe------//
        [ActionName("CompanyRegister")]
        [HttpPost]
        public HttpResponseMessage CompanyRegister(register_list values)
        {
            result objresult = new result();
            objresult = checkIfFieldsExists(values);
            if (objresult.status)
            {
                msSQL = "insert into storyboarderp.crm_mst_tcompanyregister(" +
                        "company_name," +
                        "company_code," +
                        "contact_person," +
                        "authmobile_number," +
                        "auth_email, " +
                        "gst_number," +
                        "company_address," +
                        "branch_name," +
                        "phone_shopicart," +
                        "created_date)" +
                        "values(" +
                        "'" + values.company + "'," +
                        "'" + values.company_id + "'," +
                        "'" + values.fname + "'," +
                        "'" + values.phone + "'," +
                        "'" + values.email + "'," +
                        "'" + values.gst_number + "'," +
                        "'" + values.address + "'," +
                        "'" + values.branch + "'," +
                        "'" + values.phone_shopicart + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    objresult.status = true;
                    objresult.message = "Register Added Successfully";
                }
                else
                {
                    objresult.message = "Error occured while onboarding!";
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        public result checkIfFieldsExists(register_list values)
        {
            result objresult = new result();
            objresult.status = true;
            msSQL = "select company_name from " + c_code + ".crm_mst_tcompanyregister where company_name='" + values.company + "'";
            objODBCdatareader = objdbconn.GetDataReader(msSQL);
            if (objODBCdatareader.HasRows == true)
            {
                objODBCdatareader.Close();
                objresult.status = false;
                objresult.message = "Company Name exists already!";
            }
            msSQL = "select authmobile_number from " + c_code + ".crm_mst_tcompanyregister where authmobile_number='" + values.phone + "'";
            objODBCdatareader = objdbconn.GetDataReader(msSQL);
            if (objODBCdatareader.HasRows == true)
            {
                objODBCdatareader.Close();
                objresult.status = false;
                objresult.message = "Company with entered WhatsApp Number exists already!";
            }
            return objresult;
        }


        // -------------------------------------------Razor Pay-------------------------------------------------------------//
        // ---------------------------- Razorpay payment Update And order create  -------------------------//
        [ActionName("updatePayment")]
        [HttpPost]
        public HttpResponseMessage updatePayment(mdlUpdatePayment values)
        {
            msSQL = "select company_name from storyboarderp.crm_mst_tcompanyregister where company_name='" + values.company + "'";
            objODBCdatareader = objdbconn.GetDataReader(msSQL);
            if (objODBCdatareader.HasRows == true)
            {
                objODBCdatareader.Read();
                values.company_name = objODBCdatareader["company_name"].ToString();
                objODBCdatareader.Close();
                msSQL = " select plan_flag from storyboarderp.crm_mst_tworpricing " +
                        " where pricing_plan='SetUp Cost' and plan_package='" + values.signup_title + "'";
                plan_flag = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "update storyboarderp.crm_mst_tcompanyregister set payment_flag='" + plan_flag + "'" +
                        "where company_name='" + values.company + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    if (plan_flag != "F")
                    {
                        createOrder(values);
                    }
                    else
                    {
                        values.message = "Payment status updated successfully";
                        values.status = true;
                    }
                }
                else
                {
                    values.message = "Error occured while updating Payment Status!";
                }
            }
            else
            {
                objODBCdatareader.Close();
                values.message = "Kindy register and try again later!";
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        public mdlUpdatePayment createOrder(mdlUpdatePayment values)
        {
            try
            {
                msSQL = "select register_gid from storyboarderp.crm_mst_tcompanyregister where company_name='" + values.company + "'";
                string lsregister = objdbconn.GetExecuteScalar(msSQL);
                string receipt = "ORD" + "000" + lsregister;
                msSQL = " select plan_price,currency from storyboarderp.crm_mst_tworpricing " +
                        " where pricing_plan='SetUp Cost' and plan_package='" + values.signup_title + "'";
                objODBCdatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCdatareader.HasRows == true)
                {
                    objODBCdatareader.Read();
                    plan_price = objODBCdatareader["plan_price"].ToString();
                    currency = objODBCdatareader["currency"].ToString();
                }
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                RazorpayClient client = new RazorpayClient(ConfigurationManager.AppSettings["razorpay_id"], ConfigurationManager.AppSettings["razorpay_secret"]);
                Dictionary<string, object> notes = new Dictionary<string, object>
                {
                    {"notes_key_1", values.signup_title },
                    {"notes_key_2", values.company }
                };
                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", plan_price },
                    { "receipt", receipt },
                    { "currency",currency },
                    { "notes", notes }
                };

                Order order = client.Order.Create(options);

                values.order_id = order["id"];
                values.status = true;
                values.message = "Order created successfully!";
            }
            catch (Exception e)
            {
                //LogForAudit(e.Message.ToString());
                values.message = "Error occured while creating order!";
            }
            return values;
        }
        public void LogForAudit(string strVal)
        {
            try
            {
                string lspath = ConfigurationManager.AppSettings["file_path"].ToString() + "/erp_documents/whastappwebsiteerrorlog/" + DateTime.Now.Year + @"\" + DateTime.Now.Month;
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

        // ------------------- payment verification in Razorpay----------------------------//
        [ActionName("verifyPayment")]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyPayment([FromBody] mdlUpdatePayment values)
        {
            if (values == null || string.IsNullOrEmpty(values.payment_id))
            {
                return BadRequest("Payment ID is required.");
            }
            var paymentId = values.payment_id;
            var requestUrl = $"https://api.razorpay.com/v1/payments/{paymentId}";
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, requestUrl);
                var razorpayId = ConfigurationManager.AppSettings["razorpay_id"];
                var razorpaySecret = ConfigurationManager.AppSettings["razorpay_secret"];
                var authToken = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{razorpayId}:{razorpaySecret}"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Content(response.StatusCode, errorMessage);
                }
                var paymentDetails = await response.Content.ReadAsStringAsync();
                dynamic paymentInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(paymentDetails);
                var paymentStatus = paymentInfo.status;
                if (paymentStatus == "captured")
                {
                    return Ok("Payment was successful.");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, $"Payment status: {paymentStatus}");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //--------------------------------------------Stripe Payment ------------------------------------------------//
        // --------------------------------------Stripe Payment Intent and update Payment ---------------------------------------------------------------//

        // -------------------- customer Sign up-----------------------------//

        [ActionName("customer_signup")]
        [HttpPost]
        public HttpResponseMessage customer_signup(register_list values)
        {
            result objresult = new result();
            //c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
            c_code = "vcxcontroller";
            LogForAudit("------ c_code " + c_code + " -------");
            objresult = checkcustomerExists(values);
            try
            {
                if (objresult.status)
                {
                    msSQL = "insert into vcxcontroller.crm_mst_tcompanysignup(" +
                        "company_name," +
                        "company_code," +
                        "contact_person," +
                        "authmobile_number," +
                        "auth_email, " +
                        "created_date)" +
                        "values(" +
                        "'" + values.company + "'," +
                       "'" + (values.code.Length >= 6 ? values.code.Substring(0, 6).ToLower() : values.code.ToLower()) + "'," +
                        "'" + values.fname + "'," +
                        "'" + values.phone + "'," +
                        "'" + values.email + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    LogForAudit("------ msSQL " + msSQL + " -------");

                    if (mnResult != 0)
                    {

                        Random rnd = new Random();
                        string otp_value = (rnd.Next(100000, 999999)).ToString();

                        msSQL = "update vcxcontroller.crm_mst_tcompanysignup set otp_value='" + otp_value + "',otpcreate_flag = 'Y',otpexpiry_time= '" + DateTime.Now.AddMinutes(2).ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                " where  company_code= '" + (values.code.Length >= 6 ? values.code.Substring(0, 6).ToLower() : values.code.ToLower()) + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " INSERT INTO vcxcontroller.adm_mst_twhatsappotplogin ( " +
                                " otp_value, " +
                                " company_name, " +
                                " company_code," +
                                " customermail_id," +
                                " created_time," +
                                " expiry_time" +
                                " )VALUES( " +
                                " '" + otp_value + "'," +
                                " '" + values.company + "'," +
                                " '" + values.code + "'," +
                                " '" + values.email + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                " '" + DateTime.Now.AddMinutes(2).ToString("yyyy-MM-dd HH:mm:ss") + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        string Body = "<p>Dear " + values.fname + ",</p>" +
                                      "<p>Please use the verification code below to sign in.</p>" +
                                      "<table style=\"font-family: Arial, sans-serif; border-collapse: collapse; width: 50%;\">" +
                                      "<tr><td style=\"padding: 10px; border: 1px solid #ccc;\"><b>" + otp_value + "</b></td></tr>" +
                                      "</table>" +
                                      "<p>If you didn’t request this, you can ignore this email.</p>" +
                                      "<p>Thanks,</p>" +
                                      "<p>Best regards,<br> Vcidex Support Team</p>";


                        objcmnfunctions.otpmail(values.code, values.email, "Verification code", Body);
                        objresult.status = true;
                        objresult.message = "Sign Up Successfully";

                    }
                    else
                    {

                        objresult.message = "Error occured while onboarding!";
                        objresult.status = false;

                    }
                }
            }
            catch(Exception ex)
            {
                LogForAudit(ex.Message.ToString());

            }
            if (objresult.status == true)
            {
                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    objcmnfunctions.SignupMailtrigger(values.company, values.code, values.fname, values.phone, values.email);
                }));
                t.Start();
                values.status = true;
            }
            return Request.CreateResponse(HttpStatusCode.OK, objresult);

           
        }
        public result checkcustomerExists(register_list values)
        {
            result objresult = new result();
            objresult.status = true;
            msSQL = "select company_name from " + c_code + ".crm_mst_tcompanysignup where company_name='" + values.company + "'";
            objODBCdatareader = objdbconn.GetDataReader(msSQL);

            LogForAudit("------ objODBCdatareader " + msSQL + " -------");

            if (objODBCdatareader.HasRows == true)
            {
                objODBCdatareader.Close();
                objresult.status = false;
                objresult.message = "Company Name exists already!";
            }
            msSQL = "select authmobile_number from " + c_code + ".crm_mst_tcompanysignup where authmobile_number='" + values.phone + "'";
            objODBCdatareader = objdbconn.GetDataReader(msSQL);
            if (objODBCdatareader.HasRows == true)
            {
                objODBCdatareader.Close();
                objresult.status = false;
                objresult.message = "Company with entered WhatsApp Number exists already!";
            }
            msSQL = "select company_code from " + c_code + ".crm_mst_tcompanysignup where company_code='" + 
                (values.code.Length >= 6 ? values.code.Substring(0, 6).ToLower() : values.code.ToLower()) + "'";
            objODBCdatareader = objdbconn.GetDataReader(msSQL);
            if (objODBCdatareader.HasRows == true)
            {
                objODBCdatareader.Close();
                objresult.status = false;
                objresult.message = "Company Code exists already!";
            }
            objODBCdatareader.Close();

            return objresult;
        }

        [ActionName("otpverifysignup")]
        [HttpPost]
        public HttpResponseMessage GetUserReturnValue(customerotpverify values)
        {
            customerotpverifyresponse GetLoginResponse = new customerotpverifyresponse();
            try
            {
                var username = string.Empty;

                msSQL = "SELECT otpexpiry_time FROM vcxcontroller.crm_mst_tcompanysignup where otp_value ='" + values.OTP_verify + "'";
                objODBCdatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCdatareader.HasRows == true)
                {
                    lsexpiry_time = objODBCdatareader["otpexpiry_time"].ToString();
                }
                objODBCdatareader.Close();

                DateTime expiry_time = DateTime.Parse(lsexpiry_time);

                DateTime now = DateTime.Now;

                if (expiry_time > now)
                {
                    msSQL = "update vcxcontroller.crm_mst_tcompanysignup set otpverify_flag = 'Y'" +
                           " where  otp_value= '" + values.OTP_verify + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    msSQL = "SELECT company_code FROM vcxcontroller.crm_mst_tcompanysignup where otp_value ='" + values.OTP_verify + "'";
                    lssignupcompany_code = objdbconn.GetExecuteScalar(msSQL);

                    if (!string.IsNullOrEmpty(lssignupcompany_code))
                    {
                        HttpContext ctx = HttpContext.Current;
                        System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                        {
                            HttpContext.Current = ctx;
                            try
                            {
                                DaSignupDynamicDBcreationInSQLFiles(lssignupcompany_code);
                                values.status = true;
                                 // Set to true if the method completes successfully
                            }
                            catch (Exception ex)
                            {
                                LogForAudit(ex.Message.ToString());

                            }
                        }));
                        t.Start();

                    }
                    if(mnResult !=0)
                    {
                        values.status = true;

                    }

                }
                else
                {
                    values.status = false;
                    values.message = "Login time has been expired. kindly click the OTP ";
                }

            }
            catch (Exception ex)
            {
                LogForAudit(ex.Message.ToString());

                values.status = false;
                values.message = "Incorrect OTP. Please verify and try again";
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("Stripe_Register")]
        [HttpPost]
        public HttpResponseMessage Stripe_Register(register_list values)
        {
            result objresult = new result();
            c_code = Request.Headers.GetValues("c_code").FirstOrDefault();
            msSQL = " select plan_flag from " + c_code + ".crm_mst_tworpricing " +
                    " where pricing_plan='SetUp Cost' and plan_package='" + values.signup_title + "'";
            plan_flag = objdbconn.GetExecuteScalar(msSQL);
            objresult = checkIfFieldsExists(values);
            if (objresult.status)
            {

                msSQL = "insert into " + c_code + ".crm_mst_tcompanyregister(" +
                        "company_name," +
                        "company_code," +
                        "contact_person," +
                        "authmobile_number," +
                        "auth_email, " +
                        "gst_number," +
                        "company_address," +
                        "branch_name," +
                        "payment_flag," +
                        "phone_shopicart," +
                        "created_date)" +
                        "values(" +
                        "'" + values.company + "'," +
                        "'" + values.code + "'," +
                        "'" + values.fname + "'," +
                        "'" + values.phone + "'," +
                        "'" + values.email + "'," +
                        "'" + values.gst_number + "'," +
                        "'" + values.address + "'," +
                        "'" + values.branch + "'," +
                        "'" + plan_flag + "'," +
                        "'" + values.phone_shopicart + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    try
                    {
                        string stripeSecretKey = ConfigurationManager.AppSettings["stripeSecretKey"].ToString();
                        string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(stripeSecretKey + ":"));
                        string token = "Basic " + encodedCredentials;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        var client = new RestSharp.RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                        var request = new RestSharp.RestRequest("/v1/payment_links", RestSharp.Method.POST);
                        request.AddHeader("Authorization", token);
                        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                        // Assuming dt_datatable is available and populated
                        msSQL = "select stripe_price_gid, currency from " + c_code + ".crm_mst_tworpricing " +
                                "where pricing_plan='SetUp Cost' and plan_package='" + values.signup_title + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        int i = 0;
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            request.AddParameter($"line_items[{i}][price]", dt["stripe_price_gid"].ToString());
                            request.AddParameter($"line_items[{i}][quantity]", "1");
                            i++;
                        }
                        msSQL = "select register_gid from " + c_code + ".crm_mst_tcompanyregister where company_name='" + values.company + "'";
                        string lsregister = objdbconn.GetExecuteScalar(msSQL);
                        request.AddParameter("after_completion[type]", "redirect");
                        //request.AddParameter("after_completion[redirect][url]", ConfigurationManager.AppSettings["payment_URL"].ToString() + "/#/auth/payments?v1=" + objFnazurestorage.EncryptData(lsregister) + "&v2=" + objFnazurestorage.EncryptData(c_code));
                        string paymentURL = ConfigurationManager.AppSettings["payment_URL"].ToString() + "/#/auth/payments?v1=" + objFnazurestorage.EncryptData(lsregister) + "&v2=" + objFnazurestorage.EncryptData(c_code);
                        Console.WriteLine("Payment URL: " + paymentURL);

                        // Add to request
                        request.AddParameter("after_completion[redirect][url]", paymentURL);

                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            MdlStripePaymentLink objMdlStripePaymentLink = JsonConvert.DeserializeObject<MdlStripePaymentLink>(response.Content);
                            values.payment_link = objMdlStripePaymentLink.url.Replace("https://buy.stripe.com/", "");
                            msSQL = "update " + c_code + ".crm_mst_tcompanyregister set payment_link = '" + objMdlStripePaymentLink.id + "' where register_gid = '" + lsregister + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("Update failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");
                            }
                            objresult.status = true;
                            objresult.message = "Register and Payment Link Created Successfully";
                            objresult.payment_link = objMdlStripePaymentLink.url;

                            msSQL = "update " + c_code + ".adm_mst_tconsumer set subscription_details = 'Paid' where company_code = '" + values.code + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("Error occurred while generating payment link: " + response.Content, "/errorLog/whatsapporder/Log.txt");
                            objresult.message = "Error occurred while generating payment link!";
                        }
                    }
                    catch (Exception ex)
                    {
                        objresult.message = "Exception occurred: " + ex.ToString();
                        objcmnfunctions.LogForAudit("Exception: " + ex.ToString(), "/errorLog/whatsapporder/Log.txt");
                    }
                }
                else
                {
                    objresult.message = "Error occured while onboarding!";
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [HttpPost]
        [ActionName("updateOrderPayment")]
        public HttpResponseMessage updateOrderPayment(register_list values)
        {
            try
            {
                lsregister_gid = objFnazurestorage.DecryptData(values.register_gid);
                values.c_code = objFnazurestorage.DecryptData(values.c_code);
                msSQL = "update " + values.c_code + ".crm_mst_tcompanyregister set payment_status = 'PAID' where register_gid = '" + lsregister_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    //HttpContext ctx = HttpContext.Current;
                    //System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    //{
                    //    HttpContext.Current = ctx;
                    //    DaDynamicDBcreationInSQLFiles(values);
                    //}));
                    //t.Start();
                    values.status = true;
                }
                else
                {
                    objcmnfunctions.LogForAudit("Update Failed: " + msSQL, "/errorLog/whatsapporder/Log.txt");
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured: " + ex.ToString();
                objcmnfunctions.LogForAudit("Exception occured while updating payment status: " + ex.ToString(), "/errorLog/whatsapporder/Log.txt");
            }
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // --------------------------------------------------------- Dynamic DB creation ------------------------------------------------------------------ //
        public void DaDynamicDBcreationInSQLFiles(register_list values)
        {
            result objValues = new result();

            //lsregister_gid = values.register_gid;

            msSQL = " select register_gid,company_name,contact_person,company_code,authmobile_number,auth_email,gst_number,company_address" +
                    " from " + values.c_code + ".crm_mst_tcompanyregister where register_gid='" + lsregister_gid + "'";
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

                domain = Request.RequestUri.Host.ToLower();

                LogForAudit("DomainName: " +  domain);

                string[] parts = domain.Split('.');

                // Get the last part (TLD)
                string countryCode = parts.Last();

                LogForAudit("countryCode: " + countryCode);

                // Check if it's "uk"
                if (countryCode == "uk")
                {

                    msSQL = "select concat(hosting_details,'Database=sys;') as hosting_details,server_gid from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name='United Kingdom'";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {

                        lsserver_gid = objODBCDataReader["server_gid"].ToString();
                        lshosting_details = objODBCDataReader["hosting_details"].ToString();
                    }

                    string databaseconnectionString = lshosting_details;
                    objODBCDataReader.Close();

                    msSQL1 = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name ='United Kingdom'";
                    //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                    objODBCDataReader = objdbconn.GetDataReader(msSQL1);
                    if (objODBCDataReader.HasRows == true)
                    {

                        lsviewhostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                    }
                    objODBCDataReader.Close();

                    using (OdbcConnection connection = new OdbcConnection(databaseconnectionString))
                    {
                        try
                        {
                            connection.Open();

                            string msSQL = "CREATE DATABASE `" + company_code + "`;";

                            OdbcCommand command = new OdbcCommand(msSQL, connection);

                            // Execute the SQL command
                            int mnDBResult = command.ExecuteNonQuery();
                            mnDBResult = 1;

                            if (mnDBResult == 1)

                            {
                                lsSQLpath = ConfigurationManager.AppSettings["SQlpath"].ToString();
                                var SQLfiles = Directory.GetFiles(lsSQLpath, "*.sql");
                                foreach (var createstatement in SQLfiles)
                                {
                                    GetFilesinPath = System.IO.File.ReadAllText(createstatement);
                                    if (createstatement.EndsWith("kotSP.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` PROCEDURE", "CREATE PROCEDURE`" + company_code + "`." + "").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);

                                        //var combinedScripts = string.Join(";\r\n", createtables.Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p))) + ";";

                                        //if (!string.IsNullOrEmpty(combinedScripts))
                                        //{
                                        //    OdbcCommand commandsp = new OdbcCommand(combinedScripts, connection);
                                        //    mnresult = commandsp.ExecuteNonQuery();
                                        //}
                                    }
                                    else if (createstatement.EndsWith("Functions.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` FUNCTION", "CREATE FUNCTION").Replace("CREATE FUNCTION", "CREATE FUNCTION `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                                    }
                                    else if (createstatement.EndsWith("View.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW", "CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW").Replace("VIEW `", "VIEW `");

                                        //Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);



                                        using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                        {
                                            viewconnection.Open();
                                            foreach (var tables in createtables)
                                            {
                                                if (tables.Trim() != "")
                                                {
                                                    msSQL = tables.Trim();
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                            }
                                            viewconnection.Close();

                                        }


                                    }
                                    else if (createstatement.EndsWith("kotTables.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                    }

                                    else if (createstatement.EndsWith("TableAlter.sql"))
                                    {
                                        string Scripts = GetFilesinPath.Replace("ALTER TABLE", "ALTER TABLE `" + company_code + "`." + "");

                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                        {
                                            viewconnection.Open();
                                            foreach (var tables in createtables)
                                            {
                                                if (tables.Trim() != "")
                                                {
                                                    msSQL = tables.Trim();
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                            }
                                            viewconnection.Close();

                                        }


                                    }
                                    if (!createstatement.EndsWith("View.sql", StringComparison.OrdinalIgnoreCase) &&
                                         !createstatement.EndsWith("TableAlter.sql", StringComparison.OrdinalIgnoreCase))
                                    {
                                        foreach (var tables in createtables)
                                        {
                                            if (tables.Trim() != "")
                                            {
                                                msSQL = tables.Trim();
                                                OdbcCommand commands = new OdbcCommand(msSQL, connection);
                                                // Execute the SQL command
                                                mnresult = commands.ExecuteNonQuery();
                                                mnresult = 1;
                                            }
                                        }
                                    }

                                }


                                if (mnresult == 1)
                                {
                                    msSQL = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name ='United Kingdom'";
                                    //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objODBCDataReader.HasRows == true)
                                    {

                                        lshostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                                    }
                                    objODBCDataReader.Close();
                                    //string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                                    //    "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                                    //    "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";

                                    DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                                    builder.ConnectionString = lshostingconnectionstring;

                                    // Extract the components
                                    string uid = builder.ContainsKey("UID") ? builder["UID"].ToString() : string.Empty;
                                    string pwd = builder.ContainsKey("PWD") ? builder["PWD"].ToString() : string.Empty;
                                    string server = builder.ContainsKey("Server") ? builder["Server"].ToString() : string.Empty;
                                    string port = builder.ContainsKey("port") ? builder["port"].ToString() : string.Empty;


                                    msSQL = "select company_code " +
                                        "from storyboard.adm_mst_tconsumerdb where company_code='" + company_code + "'";

                                    OdbcCommand commandreader = new OdbcCommand(msSQL, connection);

                                    // Execute the command and obtain a data reader
                                    OdbcDataReader objOdbcDataReader = commandreader.ExecuteReader();

                                    if (!objOdbcDataReader.HasRows)
                                    {
                                        msSQL = "insert into storyboard.adm_mst_tconsumerdb(" +
                                                "company_code," +
                                                "server_name," +
                                                "db_name," +
                                                "user_name," +
                                                "password," +
                                                "connection_string," +
                                                "created_date, created_by) values (" +
                                                "'" + company_code + "'," +
                                                "'" + server + "'," +
                                                "'" + company_code + "'," +
                                                "'" + uid + "'," +
                                                "'" + pwd + "'," +
                                                "'" + lshostingconnectionstring + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                "'U1')";

                                        OdbcCommand consumerdbcommand = new OdbcCommand(msSQL, connection);
                                        mnResult = consumerdbcommand.ExecuteNonQuery();
                                        mnResult = 1;

                                       
                                        if (mnResult == 1)
                                        {
                                            msSQL = " insert into " + company_code + ".adm_mst_tcompany (" +
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
                                            OdbcCommand companycommand = new OdbcCommand(msSQL, connection);
                                            mnResult = companycommand.ExecuteNonQuery();
                                            mnResult = 1;
                                            if (mnResult == 1)
                                            {
                                                msSQL = "insert into " + company_code + ".adm_mst_tuser(" +
                                                       "user_gid," +
                                                       "user_code," +
                                                       "user_firstname," +
                                                       "user_lastname," +
                                                       "user_password," +
                                                       "user_status" +
                                                       " ) values (" +
                                                       "'U1'," +
                                                       "'superadmin'," +
                                                       "'superadmin'," +
                                                       "'administrator'," +
                                                       "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                       "'Y')";

                                                OdbcCommand usercommand = new OdbcCommand(msSQL, connection);
                                                mnResult = usercommand.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into " + company_code + ".adm_mst_tuser(" +
                                                       "user_gid," +
                                                       "user_code," +
                                                       "user_firstname," +
                                                        "user_lastname," +
                                                       "user_password," +
                                                       "user_status" +
                                                       " ) values (" +
                                                       "'U2'," +
                                                       "'admin'," +
                                                       "'admin'," +
                                                       "'administrator'," +
                                                       "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                       "'Y')";

                                                OdbcCommand usercommand1 = new OdbcCommand(msSQL, connection);
                                                mnResult = usercommand1.ExecuteNonQuery();
                                                mnResult = 1;

                                                if (mnResult == 1)
                                                {
                                                    msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                                                        "employee_gid," +
                                                        "biometric_id," +
                                                        "user_gid" +
                                                        " )values(" +
                                                        "'E1'," +
                                                         "'1'," +
                                                        "'U1')";
                                                    OdbcCommand employeecommand = new OdbcCommand(msSQL, connection);
                                                    mnResult = employeecommand.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                                                       "employee_gid," +
                                                       "biometric_id," +
                                                       "user_gid" +
                                                       " )values(" +
                                                       "'E2'," +
                                                       "'1'," +
                                                       "'U2')";
                                                    OdbcCommand employeecommand1 = new OdbcCommand(msSQL, connection);
                                                    mnResult = employeecommand1.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                        "module2employee_gid," +
                                                        "module_gid," +
                                                        "employee_gid," +
                                                        "branch_gid," +
                                                        "employeereporting_to," +
                                                        "hierarchy_level," +
                                                        "visible," +
                                                        "approval_hierarchy_removed," +
                                                        "created_by," +
                                                        "created_date" +
                                                        " ) values (" +
                                                        "'SMEM1611170112'," +
                                                        "'SYS'," +
                                                        "'E1'," +
                                                        "''," +
                                                        "'null'," +
                                                        "'1'," +
                                                        "'T'," +
                                                        "'N'," +
                                                        "'E1'," +
                                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                    OdbcCommand module2employee = new OdbcCommand(msSQL, connection);
                                                    mnResult = module2employee.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                            "module2employee_gid," +
                                                            "module_gid," +
                                                            "employee_gid," +
                                                            "branch_gid," +
                                                            "employeereporting_to," +
                                                            "hierarchy_level," +
                                                            "visible," +
                                                            "approval_hierarchy_removed," +
                                                            "created_by," +
                                                            "created_date" +
                                                            " ) values (" +
                                                            "'SMEM1611170113'," +
                                                            "'SYS'," +
                                                            "'E2'," +
                                                            "''," +
                                                            "'null'," +
                                                            "'1'," +
                                                            "'T'," +
                                                            "'N'," +
                                                            "'E2'," +
                                                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                    OdbcCommand module2employee1 = new OdbcCommand(msSQL, connection);
                                                    mnResult = module2employee1.ExecuteNonQuery();
                                                    mnResult = 1;

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

                                                                    try
                                                                    {
                                                                        // Attempt to execute the insert query
                                                                        OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                                        mnResult = metacommand.ExecuteNonQuery();
                                                                        mnResult = 1; // Assuming the insert was successful
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        if (ex.Message.Contains("Duplicate entry"))
                                                                        {
                                                                            // If a duplicate is found, move it to the 'duplicate_records' table
                                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.duplicate_records (original_data, error_message) VALUES (@original_data, @error_message)";
                                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                                            duplicateCommand.Parameters.AddWithValue("@original_data", msSQL);
                                                                            duplicateCommand.Parameters.AddWithValue("@error_message", ex.Message);
                                                                            duplicateCommand.ExecuteNonQuery();
                                                                        }
                                                                        else
                                                                        {
                                                                            // Re-throw the exception if it's not a duplicate-related error
                                                                            throw;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (mnResult == 1)
                                                        {
                                                            msSQL = "insert into vcxcontroller.adm_mst_tconsumer(" +
                                                                 "server_gid," +
                                                                 "company_code," +
                                                                 "consumer_url," +
                                                                  "start_date," +
                                                                  "end_date," +
                                                                 "subscription_details," +
                                                                 "created_by," +
                                                                 "created_date," +
                                                                 "status) values (" +
                                                                 "'" + lsserver_gid + "'," +
                                                                 "'" + company_code + "'," +
                                                                 "'" + domain + "'," +
                                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                 "'" + DateTime.Now.AddDays(30).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                 "'Free'," +
                                                                 "'E1'," +
                                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                 "'Y')";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

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
                                    objOdbcDataReader.Close();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            connection.Close();

                            LogForAudit(ex.Message.ToString());
                        }
                        connection.Close();
                    }
                }
                else
                {
                   
                    msSQL = "select concat(hosting_details,'Database=sys;') as hosting_details,server_gid from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name !='United Kingdom'";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {

                        lsserver_gid = objODBCDataReader["server_gid"].ToString();
                        lshosting_details = objODBCDataReader["hosting_details"].ToString();
                    }


                   string databaseconnectionString = lshosting_details;
                    objODBCDataReader.Close();
                    msSQL1 = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name !='United Kingdom'";
                    //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                    objODBCDataReader = objdbconn.GetDataReader(msSQL1);
                    if (objODBCDataReader.HasRows == true)
                    {

                        lsviewhostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                    }
                    objODBCDataReader.Close();

                    using (OdbcConnection connection = new OdbcConnection(databaseconnectionString))
                    {
                        try
                        {
                            connection.Open();

                            string msSQL = "CREATE DATABASE `" + company_code + "`;";

                             OdbcCommand command = new OdbcCommand(msSQL, connection);
                           
                                // Execute the SQL command
                                int mnDBResult = command.ExecuteNonQuery();
                                 mnDBResult = 1;
                           
                            if (mnDBResult == 1)

                                {
                                    lsSQLpath = ConfigurationManager.AppSettings["SQlpath"].ToString();
                                    var SQLfiles = Directory.GetFiles(lsSQLpath, "*.sql");
                                    foreach (var createstatement in SQLfiles)
                                    {
                                    GetFilesinPath = System.IO.File.ReadAllText(createstatement);
                                    if (createstatement.EndsWith("kotSP.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` PROCEDURE", "CREATE PROCEDURE`" + company_code + "`." + "").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);

                                        //var combinedScripts = string.Join(";\r\n", createtables.Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p))) + ";";

                                        //if (!string.IsNullOrEmpty(combinedScripts))
                                        //{
                                        //    OdbcCommand commandsp = new OdbcCommand(combinedScripts, connection);
                                        //    mnresult = commandsp.ExecuteNonQuery();
                                        //}
                                    }
                                    else if (createstatement.EndsWith("Functions.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` FUNCTION", "CREATE FUNCTION").Replace("CREATE FUNCTION", "CREATE FUNCTION `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                                    }
                                    else if (createstatement.EndsWith("View.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW", "CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW").Replace("VIEW `", "VIEW `");

                                        //Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);



                                        using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                        {
                                            viewconnection.Open();
                                            foreach (var tables in createtables)
                                            {
                                                if (tables.Trim() != "")
                                                {
                                                    msSQL = tables.Trim();
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                            }
                                            viewconnection.Close();

                                        }


                                    }
                                    else if (createstatement.EndsWith("kotTables.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                    }

                                    else if (createstatement.EndsWith("TableAlter.sql"))
                                    {
                                        string Scripts = GetFilesinPath.Replace("ALTER TABLE", "ALTER TABLE `" + company_code + "`." + "");

                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                        {
                                            viewconnection.Open();
                                            foreach (var tables in createtables)
                                            {
                                                if (tables.Trim() != "")
                                                {
                                                    msSQL = tables.Trim();
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                            }
                                            viewconnection.Close();

                                        }


                                    }
                                    if (!createstatement.EndsWith("View.sql", StringComparison.OrdinalIgnoreCase) &&
                                         !createstatement.EndsWith("TableAlter.sql", StringComparison.OrdinalIgnoreCase))
                                    {
                                        foreach (var tables in createtables)
                                        {
                                            if (tables.Trim() != "")
                                            {
                                                msSQL = tables.Trim();
                                                OdbcCommand commands = new OdbcCommand(msSQL, connection);
                                                // Execute the SQL command
                                                mnresult = commands.ExecuteNonQuery();
                                                mnresult = 1;
                                            }
                                        }
                                    }

                                }


                                if (mnresult == 1)
                                {
                                    msSQL = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name !='United Kingdom'";
                                    //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objODBCDataReader.HasRows == true)
                                    {

                                        lshostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                                    }
                                    objODBCDataReader.Close();

                                    //string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                                    //    "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                                    //    "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";

                                    DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                                    builder.ConnectionString = lshostingconnectionstring;

                                    // Extract the components
                                    string uid = builder.ContainsKey("UID") ? builder["UID"].ToString() : string.Empty;
                                    string pwd = builder.ContainsKey("PWD") ? builder["PWD"].ToString() : string.Empty;
                                    string server = builder.ContainsKey("Server") ? builder["Server"].ToString() : string.Empty;
                                    string port = builder.ContainsKey("port") ? builder["port"].ToString() : string.Empty;


                                    msSQL = "select company_code " +
                                        "from storyboard.adm_mst_tconsumerdb where company_code='" + company_code + "'";

                                    OdbcCommand commandreader = new OdbcCommand(msSQL, connection);

                                    // Execute the command and obtain a data reader
                                    OdbcDataReader objOdbcDataReader = commandreader.ExecuteReader();

                                    if (!objOdbcDataReader.HasRows)
                                    {
                                        msSQL = "insert into storyboard.adm_mst_tconsumerdb(" +
                                                "company_code," +
                                                "server_name," +
                                                "db_name," +
                                                "user_name," +
                                                "password," +
                                                "connection_string," +
                                                "created_date, created_by) values (" +
                                                "'" + company_code + "'," +
                                                "'" + server + "'," +
                                                "'" + company_code + "'," +
                                                "'" + uid + "'," +
                                                "'" + pwd + "'," +
                                                "'" + lshostingconnectionstring + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                "'U1')";

                                        OdbcCommand consumerdbcommand = new OdbcCommand(msSQL, connection);
                                        mnResult = consumerdbcommand.ExecuteNonQuery();
                                        mnResult = 1;

                                        if (mnResult == 1)
                                        {
                                            msSQL = " insert into " + company_code + ".adm_mst_tcompany (" +
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
                                            OdbcCommand companycommand = new OdbcCommand(msSQL, connection);
                                            mnResult = companycommand.ExecuteNonQuery();
                                            mnResult = 1;
                                            if (mnResult == 1)
                                            {
                                                msSQL = "insert into " + company_code + ".adm_mst_tuser(" +
                                                       "user_gid," +
                                                       "user_code," +
                                                       "user_firstname," +
                                                       "user_lastname," +
                                                       "user_password," +
                                                       "user_status" +
                                                       " ) values (" +
                                                       "'U1'," +
                                                       "'superadmin'," +
                                                       "'superadmin'," +
                                                        "'administrator'," +
                                                       "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                       "'Y')";

                                                OdbcCommand usercommand = new OdbcCommand(msSQL, connection);
                                                mnResult = usercommand.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into " + company_code + ".adm_mst_tuser(" +
                                                       "user_gid," +
                                                       "user_code," +
                                                       "user_firstname," +
                                                        "user_lastname," +
                                                       "user_password," +
                                                       "user_status" +
                                                       " ) values (" +
                                                       "'U2'," +
                                                       "'admin'," +
                                                       "'admin'," +
                                                       "'administrator'," +
                                                       "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                       "'Y')";

                                                OdbcCommand usercommand1 = new OdbcCommand(msSQL, connection);
                                                mnResult = usercommand1.ExecuteNonQuery();
                                                mnResult = 1;

                                                if (mnResult == 1)
                                                {
                                                    msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                                                        "employee_gid," +
                                                        "biometric_id," +
                                                        "user_gid" +
                                                        " )values(" +
                                                        "'E1'," +
                                                         "'1'," +
                                                        "'U1')";
                                                    OdbcCommand employeecommand = new OdbcCommand(msSQL, connection);
                                                    mnResult = employeecommand.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                                                       "employee_gid," +
                                                       "biometric_id," +
                                                       "user_gid" +
                                                       " )values(" +
                                                       "'E2'," +
                                                       "'1'," +
                                                       "'U2')";
                                                    OdbcCommand employeecommand1 = new OdbcCommand(msSQL, connection);
                                                    mnResult = employeecommand1.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                        "module2employee_gid," +
                                                        "module_gid," +
                                                        "employee_gid," +
                                                        "branch_gid," +
                                                        "employeereporting_to," +
                                                        "hierarchy_level," +
                                                        "visible," +
                                                        "approval_hierarchy_removed," +
                                                        "created_by," +
                                                        "created_date" +
                                                        " ) values (" +
                                                        "'SMEM1611170112'," +
                                                        "'SYS'," +
                                                        "'E1'," +
                                                        "''," +
                                                        "'null'," +
                                                        "'1'," +
                                                        "'T'," +
                                                        "'N'," +
                                                        "'E1'," +
                                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                    OdbcCommand module2employee = new OdbcCommand(msSQL, connection);
                                                    mnResult = module2employee.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                            "module2employee_gid," +
                                                            "module_gid," +
                                                            "employee_gid," +
                                                            "branch_gid," +
                                                            "employeereporting_to," +
                                                            "hierarchy_level," +
                                                            "visible," +
                                                            "approval_hierarchy_removed," +
                                                            "created_by," +
                                                            "created_date" +
                                                            " ) values (" +
                                                            "'SMEM1611170113'," +
                                                            "'SYS'," +
                                                            "'E2'," +
                                                            "''," +
                                                            "'null'," +
                                                            "'1'," +
                                                            "'T'," +
                                                            "'N'," +
                                                            "'E2'," +
                                                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                    OdbcCommand module2employee1 = new OdbcCommand(msSQL, connection);
                                                    mnResult = module2employee1.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    if (mnResult == 1)
                                                    {
                                                        //lsSQLpath = ConfigurationManager.AppSettings["SQlMetaData"].ToString();
                                                        //var InsertFiles = Directory.GetFiles(lsSQLpath, "*.sql");
                                                        //foreach (var insertquery in InsertFiles)
                                                        //{
                                                        //    inserpath = System.IO.File.ReadAllText(insertquery);

                                                        //    string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`." + "");
                                                        //    string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);
                                                        //    foreach (var insert in metadatasforinsert)
                                                        //    {
                                                        //        if (insert.Trim() != "")
                                                        //        {
                                                        //            msSQL = insert.Trim();
                                                        //            OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                        //            mnResult = metacommand.ExecuteNonQuery();
                                                        //            mnResult = 1;
                                                        //        }
                                                        //    }
                                                        //}

                                                        lsSQLpath = ConfigurationManager.AppSettings["SQlMetaData"].ToString();
                                                        var InsertFiles = Directory.GetFiles(lsSQLpath, "*.sql");
                                                        foreach (var insertquery in InsertFiles)
                                                        {
                                                            inserpath = System.IO.File.ReadAllText(insertquery);

                                                            string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`.");
                                                            string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);

                                                            foreach (var insert in metadatasforinsert)
                                                            {
                                                                if (insert.Trim() != "")
                                                                {
                                                                    msSQL = insert.Trim();

                                                                    try
                                                                    {
                                                                        // Attempt to execute the insert query
                                                                        OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                                        mnResult = metacommand.ExecuteNonQuery();
                                                                        mnResult = 1; // Assuming the insert was successful
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        if (ex.Message.Contains("Duplicate entry"))
                                                                        {
                                                                            // If a duplicate is found, move it to the 'duplicate_records' table
                                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.duplicate_records (original_data, error_message) VALUES (@original_data, @error_message)";
                                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                                            duplicateCommand.Parameters.AddWithValue("@original_data", msSQL);
                                                                            duplicateCommand.Parameters.AddWithValue("@error_message", ex.Message);
                                                                            duplicateCommand.ExecuteNonQuery();
                                                                        }
                                                                        else
                                                                        {
                                                                            // Re-throw the exception if it's not a duplicate-related error
                                                                            throw;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (mnResult == 1)
                                                        {
                                                            msSQL = "insert into vcxcontroller.adm_mst_tconsumer(" +
                                                                 "server_gid," +
                                                                 "company_code," +
                                                                 "consumer_url," +
                                                                  "start_date," +
                                                                  "end_date," +
                                                                 "subscription_details," +
                                                                 "created_by," +
                                                                 "created_date," +
                                                                 "status) values (" +
                                                                 "'" + lsserver_gid + "'," +
                                                                 "'" + company_code + "'," +
                                                                 "'" + domain + "'," +
                                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                 "'" + DateTime.Now.AddDays(30).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                 "'Free'," +
                                                                 "'E1'," +
                                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                 "'Y')";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                                    objOdbcDataReader.Close();

                                }
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                            connection.Close();

                            LogForAudit(ex.Message.ToString());
                        }
                        connection.Close();

                    }
                }

                // ----------------------------------DB CREATE-------------------------------------------------------
                //msSQL = "CREATE DATABASE " + company_code + "";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //if (mnResult == 1)

                //{
                //    lsSQLpath = ConfigurationManager.AppSettings["SQlpath"].ToString();
                //    var SQLfiles = Directory.GetFiles(lsSQLpath, "*.sql");
                //    foreach (var createstatement in SQLfiles)
                //    {
                //        GetFilesinPath = System.IO.File.ReadAllText(createstatement);
                //        if (createstatement.EndsWith("kotSP.sql"))
                //        {
                //            Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + company_code + "`." + "");
                //            createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                //        }
                //        else if (createstatement.EndsWith("Functions.sql"))
                //        {
                //            Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` FUNCTION", "CREATE FUNCTION").Replace("CREATE FUNCTION", "CREATE FUNCTION `" + company_code + "`." + "");
                //            createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                //        }
                //        else if (createstatement.EndsWith("View.sql"))
                //        {
                //            Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                //            createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                //        }
                //        else if (createstatement.EndsWith("kotTables.sql"))
                //        {
                //            Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + company_code + "`." + "");
                //            createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                //        }
                //        foreach (var tables in createtables)
                //        {
                //            if (tables.Trim() != "")
                //            {
                //                msSQL = tables.Trim();
                //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //            }
                //        }
                //    }

                //    if (mnResult == 1)
                //    {
                //        string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                //        "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                //        "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";
                //        msSQL = "select company_code " +
                //        "from storyboard.adm_mst_tconsumerdb where company_code='" + company_code + "'";
                //        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                //        if (!objOdbcDataReader.HasRows)
                //        {
                //            msSQL = "insert into storyboard.adm_mst_tconsumerdb(" +
                //                    "consumer_gid," +
                //                    "company_code," +
                //                    "server_name," +
                //                    "db_name," +
                //                    "user_name," +
                //                    "password," +
                //                    "connection_string," +
                //                    "created_date, created_by) values (" +
                //                    "'" + consumer_gid + "'," +
                //                    "'" + company_code + "'," +
                //                    "'" + ConfigurationManager.AppSettings["DBserver"] + "'," +
                //                    "'" + company_code + "'," +
                //                    "'" + ConfigurationManager.AppSettings["DBUID"] + "'," +
                //                    "'" + ConfigurationManager.AppSettings["DBpwd"] + "'," +
                //                    "'" + connectionString + "'," +
                //                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                //                    "'U1')";
                //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //            if (mnResult == 1)
                //            {
                //                msSQL = " insert into " + company_code + " .adm_mst_tcompany (" +
                //                    "company_code," +
                //                    "company_name," +
                //                    "company_phone," +
                //                    "company_address," +
                //                    "contact_person," +
                //                    "company_mail," +
                //                    "auth_code," +
                //                    "pop_password" +
                //                    " ) values ( " +
                //                    "'" + company_code + "'," +
                //                    "'" + company_name + "'," +
                //                    "'" + mobile_number + "'," +
                //                    "'" + company_address + "'," +
                //                    "'" + contact_person + "'," +
                //                    "'" + toaddress + "'," +
                //                    "''," +
                //                    "'')";
                //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //                if (mnResult == 1)
                //                {
                //                    msSQL = "insert into " + company_code + ".adm_mst_tuser(" +
                //                           "user_gid," +
                //                           "user_code," +
                //                           "user_firstname," +
                //                           "user_password," +
                //                           "user_status" +
                //                           " ) values (" +
                //                           "'U1'," +
                //                           "'admin'," +
                //                           "'admin'," +
                //                           "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                //                           "'Y')";
                //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //                    if (mnResult == 1)
                //                    {
                //                        msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                //                            "employee_gid," +
                //                            "user_gid" +
                //                            " )values(" +
                //                            "'E1'," +
                //                            "'U1')";
                //                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //                        if (mnResult == 1)
                //                        {
                //                            lsSQLpath = ConfigurationManager.AppSettings["SQlMetaData"].ToString();
                //                            var InsertFiles = Directory.GetFiles(lsSQLpath, "*.sql");
                //                            foreach (var insertquery in InsertFiles)
                //                            {
                //                                inserpath = System.IO.File.ReadAllText(insertquery);

                //                                string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`." + "");
                //                                string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);
                //                                foreach (var insert in metadatasforinsert)
                //                                {
                //                                    if (insert.Trim() != "")
                //                                    {
                //                                        msSQL = insert.Trim();
                //                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //                                    }
                //                                }
                //                            }

                //                            if (mnResult == 1)
                //                            {
                //                                objcmnfunctions.activationMailtrigger(company_code, "admin", "Welcome@123", toaddress, contact_person);
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            objValues.status = false;
                //            objValues.message = " Already Registered, Kindly Check Your mail...";
                //        }
                //    }
                //}
                objValues.status = true;
                return;
            }
        }


        public void DaSignupDynamicDBcreationInSQLFiles(string c_code)
        {
            result objValues = new result();

            msSQL = " select company_name,company_code,authmobile_number,auth_email,contact_person" +
                    " from vcxcontroller.crm_mst_tcompanysignup where company_code='" + c_code + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                objOdbcDataReader.Read();
                string company_name = objOdbcDataReader["company_name"].ToString();
                string mobile_number = objOdbcDataReader["authmobile_number"].ToString();
                string contact_person = objOdbcDataReader["contact_person"].ToString();
                string toaddress = objOdbcDataReader["auth_email"].ToString();
                company_code = objOdbcDataReader["company_code"].ToString();
                objOdbcDataReader.Close();

                domain = Request.RequestUri.Host.ToLower();

                LogForAudit("DomainName: " + domain);

                string[] parts = domain.Split('.');

                // Get the last part (TLD)
                string countryCode = parts.Last();

                LogForAudit("countryCode: " + countryCode);

                // Check if it's "uk"
                if (countryCode == "uk")
                {

                    msSQL = "select concat(hosting_details,'Database=sys;') as hosting_details,server_gid from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name='United Kingdom'";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {

                        lsserver_gid = objODBCDataReader["server_gid"].ToString();
                        lshosting_details = objODBCDataReader["hosting_details"].ToString();
                    }

                    string databaseconnectionString = lshosting_details;
                    objODBCDataReader.Close();

                    msSQL1 = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name ='United Kingdom'";
                    //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                    objODBCDataReader = objdbconn.GetDataReader(msSQL1);
                    if (objODBCDataReader.HasRows == true)
                    {

                        lsviewhostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                    }
                    objODBCDataReader.Close();

                    using (OdbcConnection connection = new OdbcConnection(databaseconnectionString))
                    {
                        try
                        {
                            connection.Open();

                            string msSQL = "CREATE DATABASE `" + company_code + "`;";

                            OdbcCommand command = new OdbcCommand(msSQL, connection);

                            // Execute the SQL command
                            int mnDBResult = command.ExecuteNonQuery();
                            mnDBResult = 1;

                            if (mnDBResult == 1)

                            {
                                lsSQLpath = ConfigurationManager.AppSettings["SQlpath"].ToString();
                                var SQLfiles = Directory.GetFiles(lsSQLpath, "*.sql");
                                foreach (var createstatement in SQLfiles)
                                {
                                    GetFilesinPath = System.IO.File.ReadAllText(createstatement);
                                    if (createstatement.EndsWith("kotSP.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` PROCEDURE", "CREATE PROCEDURE`" + company_code + "`." + "").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);

                                        //var combinedScripts = string.Join(";\r\n", createtables.Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p))) + ";";

                                        //if (!string.IsNullOrEmpty(combinedScripts))
                                        //{
                                        //    OdbcCommand commandsp = new OdbcCommand(combinedScripts, connection);
                                        //    mnresult = commandsp.ExecuteNonQuery();
                                        //}
                                    }
                                    else if (createstatement.EndsWith("Functions.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` FUNCTION", "CREATE FUNCTION").Replace("CREATE FUNCTION", "CREATE FUNCTION `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                                    }
                                    else if (createstatement.EndsWith("View.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW", "CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW").Replace("VIEW `", "VIEW `");

                                        //Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                       

                                        using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                        {
                                            viewconnection.Open();
                                            foreach (var tables in createtables)
                                            {
                                                if (tables.Trim() != "")
                                                {
                                                    msSQL = tables.Trim();
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                            }
                                            viewconnection.Close();

                                        }


                                    }
                                    else if (createstatement.EndsWith("kotTables.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                      
                                    }

                                    else if (createstatement.EndsWith("TableAlter.sql"))
                                    {
                                        string Scripts = GetFilesinPath.Replace("ALTER TABLE", "ALTER TABLE `" + company_code + "`." + "");

                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                        {
                                            viewconnection.Open();
                                            foreach (var tables in createtables)
                                            {
                                                if (tables.Trim() != "")
                                                {
                                                    msSQL = tables.Trim();
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                            }
                                            viewconnection.Close();

                                        }


                                    }
                                    if (!createstatement.EndsWith("View.sql", StringComparison.OrdinalIgnoreCase) &&
                                         !createstatement.EndsWith("TableAlter.sql", StringComparison.OrdinalIgnoreCase))
                                    {
                                        foreach (var tables in createtables)
                                        {
                                            if (tables.Trim() != "")
                                            {
                                                msSQL = tables.Trim();
                                                OdbcCommand commands = new OdbcCommand(msSQL, connection);
                                                // Execute the SQL command
                                                mnresult = commands.ExecuteNonQuery();
                                                mnresult = 1;
                                            }
                                        }
                                    }

                                }


                                if (mnresult == 1)
                                {
                                    msSQL = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name ='United Kingdom'";
                                    //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objODBCDataReader.HasRows == true)
                                    {

                                        lshostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                                    }
                                    objODBCDataReader.Close();
                                    //string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                                    //    "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                                    //    "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";

                                    DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                                    builder.ConnectionString = lshostingconnectionstring;

                                    // Extract the components
                                    string uid = builder.ContainsKey("UID") ? builder["UID"].ToString() : string.Empty;
                                    string pwd = builder.ContainsKey("PWD") ? builder["PWD"].ToString() : string.Empty;
                                    string server = builder.ContainsKey("Server") ? builder["Server"].ToString() : string.Empty;
                                    string port = builder.ContainsKey("port") ? builder["port"].ToString() : string.Empty;


                                    msSQL = "select company_code " +
                                        "from storyboard.adm_mst_tconsumerdb where company_code='" + company_code + "'";

                                    OdbcCommand commandreader = new OdbcCommand(msSQL, connection);

                                    // Execute the command and obtain a data reader
                                    OdbcDataReader objOdbcDataReader = commandreader.ExecuteReader();

                                    if (!objOdbcDataReader.HasRows)
                                    {
                                        msSQL = "insert into storyboard.adm_mst_tconsumerdb(" +
                                                "company_code," +
                                                "server_name," +
                                                "db_name," +
                                                "user_name," +
                                                "password," +
                                                "connection_string," +
                                                "created_date, created_by) values (" +
                                                "'" + company_code + "'," +
                                                "'" + server + "'," +
                                                "'" + company_code + "'," +
                                                "'" + uid + "'," +
                                                "'" + pwd + "'," +
                                                "'" + lshostingconnectionstring + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                "'U1')";

                                        OdbcCommand consumerdbcommand = new OdbcCommand(msSQL, connection);
                                        mnResult = consumerdbcommand.ExecuteNonQuery();
                                        mnResult = 1;

                                        if (mnResult == 1)
                                        {
                                            msSQL = " insert into `" + company_code + "`.adm_mst_tcompany (" +
                                                "company_code," +
                                                "company_name," +
                                                "company_phone," +
                                                //"company_address," +
                                                "contact_person," +
                                                "company_mail," +
                                                "auth_code," +
                                                "pop_password" +
                                                " ) values ( " +
                                                "'" + company_code + "'," +
                                                "'" + company_name + "'," +
                                                "'" + mobile_number + "'," +
                                                //"'" + company_address + "'," +
                                                "'" + contact_person + "'," +
                                                "'" + toaddress + "'," +
                                                "''," +
                                                "'')";
                                            OdbcCommand companycommand = new OdbcCommand(msSQL, connection);
                                            mnResult = companycommand.ExecuteNonQuery();
                                            mnResult = 1;
                                            if (mnResult == 1)
                                            {
                                                msSQL = "insert into `" + company_code + "`.adm_mst_tuser(" +
                                                   "user_gid," +
                                                   "user_code," +
                                                   "user_firstname," +
                                                    "user_lastname," +
                                                   "user_password," +
                                                   "user_status" +
                                                   " ) values (" +
                                                   "'U1'," +
                                                   "'superadmin'," +
                                                   "'superadmin'," +
                                                   "'administrator'," +
                                                   "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                   "'Y')";

                                                OdbcCommand usercommand = new OdbcCommand(msSQL, connection);
                                                mnResult = usercommand.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into `" + company_code + "`.adm_mst_tuser(" +
                                                      "user_gid," +
                                                      "user_code," +
                                                      "user_firstname," +
                                                       "user_lastname," +
                                                      "user_password," +
                                                      "user_status" +
                                                      " ) values (" +
                                                      "'U2'," +
                                                      "'admin'," +
                                                      "'admin'," +
                                                      "'administrator'," +
                                                      "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                      "'Y')";

                                                OdbcCommand usercommand1 = new OdbcCommand(msSQL, connection);
                                                mnResult = usercommand1.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                        "module2employee_gid," +
                                                        "module_gid," +
                                                        "employee_gid," +
                                                        "branch_gid," +
                                                        "employeereporting_to," +
                                                        "hierarchy_level," +
                                                        "visible," +
                                                        "approval_hierarchy_removed," +
                                                        "created_by," +
                                                        "created_date" +
                                                        " ) values (" +
                                                        "'SMEM1611170112'," +
                                                        "'SYS'," +
                                                        "'E1'," +
                                                        "''," +
                                                        "'null'," +
                                                        "'1'," +
                                                        "'T'," +
                                                        "'N'," +
                                                        "'E1'," +
                                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                OdbcCommand module2employee = new OdbcCommand(msSQL, connection);
                                                mnResult = module2employee.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                        "module2employee_gid," +
                                                        "module_gid," +
                                                        "employee_gid," +
                                                        "branch_gid," +
                                                        "employeereporting_to," +
                                                        "hierarchy_level," +
                                                        "visible," +
                                                        "approval_hierarchy_removed," +
                                                        "created_by," +
                                                        "created_date" +
                                                        " ) values (" +
                                                        "'SMEM1611170113'," +
                                                        "'SYS'," +
                                                        "'E2'," +
                                                        "''," +
                                                        "'null'," +
                                                        "'1'," +
                                                        "'T'," +
                                                        "'N'," +
                                                        "'E2'," +
                                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                OdbcCommand module2employee1 = new OdbcCommand(msSQL, connection);
                                                mnResult = module2employee1.ExecuteNonQuery();
                                                mnResult = 1;

                                                if (mnResult == 1)
                                                {
                                                    msSQL = " insert into `" + company_code + "`.hrm_mst_temployee(" +
                                                   "employee_gid," +
                                                   "biometric_id," +
                                                   "user_gid" +
                                                   " )values(" +
                                                   "'E1'," +
                                                   "'1'," +
                                                   "'U1')";
                                                    OdbcCommand employeecommand = new OdbcCommand(msSQL, connection);
                                                    mnResult = employeecommand.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    msSQL = " insert into `" + company_code + "`.hrm_mst_temployee(" +
                                                      "employee_gid," +
                                                      "biometric_id," +
                                                      "user_gid" +
                                                      " )values(" +
                                                      "'E2'," +
                                                       "'1'," +
                                                      "'U2')";
                                                    OdbcCommand employeecommand1 = new OdbcCommand(msSQL, connection);
                                                    mnResult = employeecommand1.ExecuteNonQuery();
                                                    mnResult = 1;


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

                                                                    try
                                                                    {
                                                                        // Attempt to execute the insert query
                                                                        OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                                        mnResult = metacommand.ExecuteNonQuery();
                                                                        mnResult = 1; // Assuming the insert was successful
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        if (ex.Message.Contains("Duplicate entry"))
                                                                        {
                                                                            // If a duplicate is found, move it to the 'duplicate_records' table
                                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.duplicate_records (original_data, error_message) VALUES (@original_data, @error_message)";
                                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                                            duplicateCommand.Parameters.AddWithValue("@original_data", msSQL);
                                                                            duplicateCommand.Parameters.AddWithValue("@error_message", ex.Message);
                                                                            duplicateCommand.ExecuteNonQuery();
                                                                        }
                                                                        else
                                                                        {
                                                                            // Re-throw the exception if it's not a duplicate-related error
                                                                            throw;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (mnResult == 1)
                                                        {
                                                            msSQL = "insert into vcxcontroller.adm_mst_tconsumer(" +
                                                                 "server_gid," +
                                                                 "company_code," +
                                                                 "consumer_url," +
                                                                  "start_date," +
                                                                  "end_date," +
                                                                 "subscription_details," +
                                                                 "created_by," +
                                                                 "created_date," +
                                                                 "status) values (" +
                                                                 "'" + lsserver_gid + "'," +
                                                                 "'" + company_code + "'," +
                                                                 "'" + domain + "'," +
                                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                 "'" + DateTime.Now.AddDays(30).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                 "'Free'," +
                                                                 "'E1'," +
                                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                 "'Y')";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                            objcmnfunctions.activationMailtrigger(company_code, "admin", "Welcome@123", toaddress, contact_person);
                                                            objValues.status = true;

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
                                    objOdbcDataReader.Close();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            connection.Close();

                            LogForAudit(ex.Message.ToString());
                        }
                        connection.Close();
                    }
                }
                else
                {

                    msSQL = "select concat(hosting_details,'Database=sys;') as hosting_details,server_gid from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name !='United Kingdom'";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {

                        lsserver_gid = objODBCDataReader["server_gid"].ToString();
                        lshosting_details = objODBCDataReader["hosting_details"].ToString();
                    }


                    string databaseconnectionString = lshosting_details;
                    objODBCDataReader.Close();

                    msSQL1 = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name !='United Kingdom'";
                    //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                    objODBCDataReader = objdbconn.GetDataReader(msSQL1);
                    if (objODBCDataReader.HasRows == true)
                    {

                        lsviewhostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                    }
                    objODBCDataReader.Close();
                    using (OdbcConnection connection = new OdbcConnection(databaseconnectionString))
                    {
                        try
                        {
                            connection.Open();

                            string msSQL = "CREATE DATABASE `" + company_code + "`;";

                            OdbcCommand command = new OdbcCommand(msSQL, connection);

                            // Execute the SQL command
                            int mnDBResult = command.ExecuteNonQuery();
                            mnDBResult = 1;

                            if (mnDBResult == 1)

                            {
                                lsSQLpath = ConfigurationManager.AppSettings["SQlpath"].ToString();
                                var SQLfiles = Directory.GetFiles(lsSQLpath, "*.sql");
                                foreach (var createstatement in SQLfiles)
                                {
                                    GetFilesinPath = System.IO.File.ReadAllText(createstatement);
                                    if (createstatement.EndsWith("kotSP.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` PROCEDURE", "CREATE PROCEDURE`" + company_code + "`." + "").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);

                                        //var combinedScripts = string.Join(";\r\n", createtables.Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p))) + ";";

                                        //if (!string.IsNullOrEmpty(combinedScripts))
                                        //{
                                        //    OdbcCommand commandsp = new OdbcCommand(combinedScripts, connection);
                                        //    mnresult = commandsp.ExecuteNonQuery();
                                        //}
                                    }
                                    else if (createstatement.EndsWith("Functions.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` FUNCTION", "CREATE FUNCTION").Replace("CREATE FUNCTION", "CREATE FUNCTION `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                                    }
                                    //else if (createstatement.EndsWith("View.sql"))
                                    //{
                                    //    Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                                    //    createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                    //}
                                    else if (createstatement.EndsWith("View.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW", "CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW").Replace("VIEW `", "VIEW `");

                                        //Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                        using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                        {
                                            viewconnection.Open();
                                            foreach (var tables in createtables)
                                            {
                                                if (tables.Trim() != "")
                                                {
                                                    msSQL = tables.Trim();
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                            }
                                            viewconnection.Close();

                                        }
                                    }
                                    else if (createstatement.EndsWith("kotTables.sql"))
                                    {
                                        Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + company_code + "`." + "");
                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                        //var combinedScript = string.Join(";", createtables.Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)));

                                        //if (!string.IsNullOrEmpty(combinedScript))
                                        //{
                                        //    OdbcCommand commands = new OdbcCommand(combinedScript, connection);
                                        //    mnresult = commands.ExecuteNonQuery();
                                        //    mnresult = 1;
                                        //}
                                    }
                                    else if (createstatement.EndsWith("TableAlter.sql"))
                                    {
                                        string Scripts = GetFilesinPath.Replace("ALTER TABLE", "ALTER TABLE `" + company_code + "`." + "");

                                        createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        using (OdbcConnection viewconnection = new OdbcConnection(lsviewhostingconnectionstring))
                                        {
                                            viewconnection.Open();
                                            foreach (var tables in createtables)
                                            {
                                                if (tables.Trim() != "")
                                                {
                                                    msSQL = tables.Trim();
                                                    OdbcCommand commands = new OdbcCommand(msSQL, viewconnection);
                                                    // Execute the SQL command
                                                    mnresult = commands.ExecuteNonQuery();
                                                    mnresult = 1;
                                                }
                                            }
                                            viewconnection.Close();

                                        }


                                    }
                                    if (!createstatement.EndsWith("View.sql", StringComparison.OrdinalIgnoreCase) &&
                                         !createstatement.EndsWith("TableAlter.sql", StringComparison.OrdinalIgnoreCase))
                                    {
                                        foreach (var tables in createtables)
                                        {
                                            if (tables.Trim() != "")
                                            {
                                                msSQL = tables.Trim();
                                                OdbcCommand commands = new OdbcCommand(msSQL, connection);
                                                // Execute the SQL command
                                                mnresult = commands.ExecuteNonQuery();
                                                mnresult = 1;
                                            }
                                        }
                                    }
                                  
                                }


                                if (mnresult == 1)
                                {
                                    msSQL = "select concat(hosting_details,'Database=" + company_code + ";') as hosting_details from vcxcontroller.adm_mst_tserver  where server_status='Open' and country_name !='United Kingdom'";
                                    //string lshostingconnectionstring = objdbconn.GetExecuteScalar(msSQL);
                                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objODBCDataReader.HasRows == true)
                                    {

                                        lshostingconnectionstring = objODBCDataReader["hosting_details"].ToString();
                                    }
                                    objODBCDataReader.Close();

                                    //string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                                    //    "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                                    //    "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";

                                    DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                                    builder.ConnectionString = lshostingconnectionstring;

                                    // Extract the components
                                    string uid = builder.ContainsKey("UID") ? builder["UID"].ToString() : string.Empty;
                                    string pwd = builder.ContainsKey("PWD") ? builder["PWD"].ToString() : string.Empty;
                                    string server = builder.ContainsKey("Server") ? builder["Server"].ToString() : string.Empty;
                                    string port = builder.ContainsKey("port") ? builder["port"].ToString() : string.Empty;


                                    msSQL = "select company_code " +
                                        "from storyboard.adm_mst_tconsumerdb where company_code='" + company_code + "'";

                                    OdbcCommand commandreader = new OdbcCommand(msSQL, connection);

                                    // Execute the command and obtain a data reader
                                    OdbcDataReader objOdbcDataReader = commandreader.ExecuteReader();

                                    if (!objOdbcDataReader.HasRows)
                                    {
                                        msSQL = "insert into storyboard.adm_mst_tconsumerdb(" +
                                                "company_code," +
                                                "server_name," +
                                                "db_name," +
                                                "user_name," +
                                                "password," +
                                                "connection_string," +
                                                "created_date, created_by) values (" +
                                                "'" + company_code + "'," +
                                                "'" + server + "'," +
                                                "'" + company_code + "'," +
                                                "'" + uid + "'," +
                                                "'" + pwd + "'," +
                                                "'" + lshostingconnectionstring + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                "'U1')";

                                        OdbcCommand consumerdbcommand = new OdbcCommand(msSQL, connection);
                                        mnResult = consumerdbcommand.ExecuteNonQuery();
                                        mnResult = 1;
                                     
                                        if (mnResult == 1)
                                        {
                                            msSQL = " insert into `" + company_code + "`.adm_mst_tcompany (" +
                                                "company_code," +
                                                "company_name," +
                                                "company_phone," +
                                                //"company_address," +
                                                "contact_person," +
                                                "company_mail," +
                                                "auth_code," +
                                                "pop_password" +
                                                " ) values ( " +
                                                "'" + company_code + "'," +
                                                "'" + company_name + "'," +
                                                "'" + mobile_number + "'," +
                                                //"'" + company_address + "'," +
                                                "'" + contact_person + "'," +
                                                "'" + toaddress + "'," +
                                                "''," +
                                                "'')";
                                            OdbcCommand companycommand = new OdbcCommand(msSQL, connection);
                                            mnResult = companycommand.ExecuteNonQuery();
                                            mnResult = 1;
                                            if (mnResult == 1)
                                            {
                                                msSQL = "insert into `" + company_code + "`.adm_mst_tuser(" +
                                                       "user_gid," +
                                                       "user_code," +
                                                       "user_firstname," +
                                                       "user_lastname," +
                                                       "user_password," +
                                                       "user_status" +
                                                       " ) values (" +
                                                       "'U1'," +
                                                       "'superadmin'," +
                                                       "'superadmin'," +
                                                       "'administrator'," +
                                                       "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                       "'Y')";

                                                OdbcCommand usercommand = new OdbcCommand(msSQL, connection);
                                                mnResult = usercommand.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into " + company_code + ".adm_mst_tuser(" +
                                                     "user_gid," +
                                                     "user_code," +
                                                     "user_firstname," +
                                                      "user_lastname," +
                                                     "user_password," +
                                                     "user_status" +
                                                     " ) values (" +
                                                     "'U2'," +
                                                     "'admin'," +
                                                     "'admin'," +
                                                     "'administrator'," +
                                                     "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                                                     "'Y')";

                                                OdbcCommand usercommand1 = new OdbcCommand(msSQL, connection);
                                                mnResult = usercommand1.ExecuteNonQuery();
                                                mnResult = 1;


                                                msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                       "module2employee_gid," +
                                                       "module_gid," +
                                                       "employee_gid," +
                                                       "branch_gid," +
                                                       "employeereporting_to," +
                                                       "hierarchy_level," +
                                                       "visible," +
                                                       "approval_hierarchy_removed," +
                                                       "created_by," +
                                                       "created_date" +
                                                       " ) values (" +
                                                       "'SMEM1611170112'," +
                                                       "'SYS'," +
                                                       "'E1'," +
                                                       "''," +
                                                       "'null'," +
                                                       "'1'," +
                                                       "'T'," +
                                                       "'N'," +
                                                       "'E1'," +
                                                       " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                OdbcCommand module2employee = new OdbcCommand(msSQL, connection);
                                                mnResult = module2employee.ExecuteNonQuery();
                                                mnResult = 1;

                                                msSQL = "insert into `" + company_code + "`.adm_mst_tmodule2employee(" +
                                                        "module2employee_gid," +
                                                        "module_gid," +
                                                        "employee_gid," +
                                                        "branch_gid," +
                                                        "employeereporting_to," +
                                                        "hierarchy_level," +
                                                        "visible," +
                                                        "approval_hierarchy_removed," +
                                                        "created_by," +
                                                        "created_date" +
                                                        " ) values (" +
                                                        "'SMEM1611170113'," +
                                                        "'SYS'," +
                                                        "'E2'," +
                                                        "''," +
                                                        "'null'," +
                                                        "'1'," +
                                                        "'T'," +
                                                        "'N'," +
                                                        "'E2'," +
                                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                OdbcCommand module2employee1 = new OdbcCommand(msSQL, connection);
                                                mnResult = module2employee1.ExecuteNonQuery();
                                                mnResult = 1;

                                                if (mnResult == 1)
                                                {
                                                    msSQL = " insert into `" + company_code + "`.hrm_mst_temployee(" +
                                                        "employee_gid," +
                                                        "biometric_id," +
                                                        "user_gid" +
                                                        " )values(" +
                                                        "'E1'," +
                                                         "'1'," +
                                                        "'U1')";
                                                    OdbcCommand employeecommand = new OdbcCommand(msSQL, connection);
                                                    mnResult = employeecommand.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    msSQL = " insert into `" + company_code + "`.hrm_mst_temployee(" +
                                                       "employee_gid," +
                                                        "biometric_id," +
                                                       "user_gid" +
                                                       " )values(" +
                                                       "'E2'," +
                                                        "'1'," +
                                                       "'U2')";
                                                    OdbcCommand employeecommand2 = new OdbcCommand(msSQL, connection);
                                                    mnResult = employeecommand2.ExecuteNonQuery();
                                                    mnResult = 1;

                                                    if (mnResult == 1)
                                                    {
                                                        //lsSQLpath = ConfigurationManager.AppSettings["SQlMetaData"].ToString();
                                                        //var InsertFiles = Directory.GetFiles(lsSQLpath, "*.sql");
                                                        //foreach (var insertquery in InsertFiles)
                                                        //{
                                                        //    inserpath = System.IO.File.ReadAllText(insertquery);

                                                        //    string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`." + "");
                                                        //    string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);
                                                        //    foreach (var insert in metadatasforinsert)
                                                        //    {
                                                        //        if (insert.Trim() != "")
                                                        //        {
                                                        //            msSQL = insert.Trim();
                                                        //            OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                        //            mnResult = metacommand.ExecuteNonQuery();
                                                        //            mnResult = 1;
                                                        //        }
                                                        //    }
                                                        //}

                                                        lsSQLpath = ConfigurationManager.AppSettings["SQlMetaData"].ToString();
                                                        var InsertFiles = Directory.GetFiles(lsSQLpath, "*.sql");
                                                        foreach (var insertquery in InsertFiles)
                                                        {
                                                            inserpath = System.IO.File.ReadAllText(insertquery);

                                                            string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`.");
                                                            string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);

                                                            foreach (var insert in metadatasforinsert)
                                                            {
                                                                if (insert.Trim() != "")
                                                                {
                                                                    msSQL = insert.Trim();

                                                                    try
                                                                    {
                                                                        // Attempt to execute the insert query
                                                                        OdbcCommand metacommand = new OdbcCommand(msSQL, connection);
                                                                        mnResult = metacommand.ExecuteNonQuery();
                                                                        mnResult = 1; // Assuming the insert was successful
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        if (ex.Message.Contains("Duplicate entry"))
                                                                        {
                                                                            // If a duplicate is found, move it to the 'duplicate_records' table
                                                                            string moveToDuplicateTable = "INSERT INTO vcxcontroller.duplicate_records (original_data, error_message) VALUES (@original_data, @error_message)";
                                                                            OdbcCommand duplicateCommand = new OdbcCommand(moveToDuplicateTable, connection);
                                                                            duplicateCommand.Parameters.AddWithValue("@original_data", msSQL);
                                                                            duplicateCommand.Parameters.AddWithValue("@error_message", ex.Message);
                                                                            duplicateCommand.ExecuteNonQuery();
                                                                        }
                                                                        else
                                                                        {
                                                                            // Re-throw the exception if it's not a duplicate-related error
                                                                            throw;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (mnResult == 1)
                                                        {
                                                            msSQL = "insert into vcxcontroller.adm_mst_tconsumer(" +
                                                                "server_gid," +
                                                                "company_code," +
                                                                "consumer_url," +
                                                                 "start_date," +
                                                                 "end_date," +
                                                                "subscription_details," +
                                                                "created_by," +
                                                                "created_date," +
                                                                "status) values (" +
                                                                "'" + lsserver_gid + "'," +
                                                                "'" + company_code + "'," +
                                                                "'" + domain + "'," +
                                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                "'" + DateTime.Now.AddDays(30).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                "'Free'," +
                                                                "'E1'," +
                                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                "'Y')";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                            objcmnfunctions.activationMailtrigger(company_code, "admin", "Welcome@123", toaddress, contact_person);
                                                            objValues.status = true;

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
                                    objOdbcDataReader.Close();

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                            connection.Close();

                            LogForAudit(ex.Message.ToString());
                        }
                        connection.Close();

                    }
                }

                // ----------------------------------DB CREATE-------------------------------------------------------
                //msSQL = "CREATE DATABASE " + company_code + "";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //if (mnResult == 1)

                //{
                //    lsSQLpath = ConfigurationManager.AppSettings["SQlpath"].ToString();
                //    var SQLfiles = Directory.GetFiles(lsSQLpath, "*.sql");
                //    foreach (var createstatement in SQLfiles)
                //    {
                //        GetFilesinPath = System.IO.File.ReadAllText(createstatement);
                //        if (createstatement.EndsWith("kotSP.sql"))
                //        {
                //            Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE  PROCEDURE", "CREATE PROCEDURE `" + company_code + "`." + "");
                //            createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                //        }
                //        else if (createstatement.EndsWith("Functions.sql"))
                //        {
                //            Scripts = GetFilesinPath.Replace("DELIMITER $$", "").Replace("CREATE DEFINER=`root`@`localhost` FUNCTION", "CREATE FUNCTION").Replace("CREATE FUNCTION", "CREATE FUNCTION `" + company_code + "`." + "");
                //            createtables = Scripts.Split(new[] { "\r", "$$\nDELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);
                //        }
                //        else if (createstatement.EndsWith("View.sql"))
                //        {
                //            Scripts = GetFilesinPath.Replace("VIEW", "VIEW `" + company_code + "`." + "");
                //            createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                //        }
                //        else if (createstatement.EndsWith("kotTables.sql"))
                //        {
                //            Scripts = GetFilesinPath.Replace("CREATE TABLE", "CREATE TABLE `" + company_code + "`." + "");
                //            createtables = Scripts.Split(new[] { '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                //        }
                //        foreach (var tables in createtables)
                //        {
                //            if (tables.Trim() != "")
                //            {
                //                msSQL = tables.Trim();
                //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //            }
                //        }
                //    }

                //    if (mnResult == 1)
                //    {
                //        string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Database=" + company_code + ";" +
                //        "UID=" + ConfigurationManager.AppSettings["DBUID"] + ";port=" + ConfigurationManager.AppSettings["DBport"] + ";" +
                //        "PWD=" + ConfigurationManager.AppSettings["DBpwd"] + ";Server=" + ConfigurationManager.AppSettings["DBserver"] + ";";
                //        msSQL = "select company_code " +
                //        "from storyboard.adm_mst_tconsumerdb where company_code='" + company_code + "'";
                //        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                //        if (!objOdbcDataReader.HasRows)
                //        {
                //            msSQL = "insert into storyboard.adm_mst_tconsumerdb(" +
                //                    "consumer_gid," +
                //                    "company_code," +
                //                    "server_name," +
                //                    "db_name," +
                //                    "user_name," +
                //                    "password," +
                //                    "connection_string," +
                //                    "created_date, created_by) values (" +
                //                    "'" + consumer_gid + "'," +
                //                    "'" + company_code + "'," +
                //                    "'" + ConfigurationManager.AppSettings["DBserver"] + "'," +
                //                    "'" + company_code + "'," +
                //                    "'" + ConfigurationManager.AppSettings["DBUID"] + "'," +
                //                    "'" + ConfigurationManager.AppSettings["DBpwd"] + "'," +
                //                    "'" + connectionString + "'," +
                //                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                //                    "'U1')";
                //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //            if (mnResult == 1)
                //            {
                //                msSQL = " insert into " + company_code + " .adm_mst_tcompany (" +
                //                    "company_code," +
                //                    "company_name," +
                //                    "company_phone," +
                //                    "company_address," +
                //                    "contact_person," +
                //                    "company_mail," +
                //                    "auth_code," +
                //                    "pop_password" +
                //                    " ) values ( " +
                //                    "'" + company_code + "'," +
                //                    "'" + company_name + "'," +
                //                    "'" + mobile_number + "'," +
                //                    "'" + company_address + "'," +
                //                    "'" + contact_person + "'," +
                //                    "'" + toaddress + "'," +
                //                    "''," +
                //                    "'')";
                //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //                if (mnResult == 1)
                //                {
                //                    msSQL = "insert into " + company_code + ".adm_mst_tuser(" +
                //                           "user_gid," +
                //                           "user_code," +
                //                           "user_firstname," +
                //                           "user_password," +
                //                           "user_status" +
                //                           " ) values (" +
                //                           "'U1'," +
                //                           "'admin'," +
                //                           "'admin'," +
                //                           "'" + objcmnfunctions.ConvertToAscii("Welcome@123") + "'," +
                //                           "'Y')";
                //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //                    if (mnResult == 1)
                //                    {
                //                        msSQL = " insert into " + company_code + ".hrm_mst_temployee(" +
                //                            "employee_gid," +
                //                            "user_gid" +
                //                            " )values(" +
                //                            "'E1'," +
                //                            "'U1')";
                //                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //                        if (mnResult == 1)
                //                        {
                //                            lsSQLpath = ConfigurationManager.AppSettings["SQlMetaData"].ToString();
                //                            var InsertFiles = Directory.GetFiles(lsSQLpath, "*.sql");
                //                            foreach (var insertquery in InsertFiles)
                //                            {
                //                                inserpath = System.IO.File.ReadAllText(insertquery);

                //                                string insertsqeriery = inserpath.Replace("INSERT INTO", "INSERT INTO `" + company_code + "`." + "");
                //                                string[] metadatasforinsert = insertsqeriery.Split(new[] { "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);
                //                                foreach (var insert in metadatasforinsert)
                //                                {
                //                                    if (insert.Trim() != "")
                //                                    {
                //                                        msSQL = insert.Trim();
                //                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //                                    }
                //                                }
                //                            }

                //                            if (mnResult == 1)
                //                            {
                //                                objcmnfunctions.activationMailtrigger(company_code, "admin", "Welcome@123", toaddress, contact_person);
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            objValues.status = false;
                //            objValues.message = " Already Registered, Kindly Check Your mail...";
                //        }
                //    }
                //}
            }
            return;

        }
    }
}