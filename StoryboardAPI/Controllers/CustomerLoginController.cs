using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using StoryboardAPI.Models;
using System.Web.Http;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Mail;

namespace StoryboardAPI.Controllers
{
    [RoutePrefix("api/CustomerLogin")]
    [AllowAnonymous]
    public class CustomerLoginController : ApiController
    {
        dbconn dbconn = new dbconn();
        cmnfunctions cnmf = new cmnfunctions();
        string tokenvalue, msSQL, domain = string.Empty;
        OdbcDataReader odbcDataReader;        
        string body, email, customer_code, password, ls_server, ls_password, ls_username;
        int ls_port;

        [ActionName("customerlogin")]
        [HttpPost]
        public HttpResponseMessage customerlogin(Postcustomer values)
        {
            customerloginResponse objresponse = new customerloginResponse();
            try
            {
                if (!String.IsNullOrEmpty(values.company_code))
                {
                    msSQL = " select eportal_status from " + values.company_code + ".crm_mst_tcustomer where eportal_emailid='" + values.eportal_emailid + "'";
                    string portal_status = dbconn.GetExecuteScalar(msSQL);
                    if (portal_status == "Y")
                    {
                        var objtoken = Token(values.eportal_emailid, cnmf.ConvertToAscii(values.eportal_password), values.company_code.ToLower());
                        dynamic newobj = JsonConvert.DeserializeObject(objtoken);
                        if (newobj.access_token != null)
                        {
                            tokenvalue = "Bearer " + newobj.access_token;
                            msSQL = " call adm_mst_spcvstoretoken('" + tokenvalue + "','" + values.eportal_emailid + "','" + cnmf.ConvertToAscii(values.eportal_password) + "','" + values.company_code + "')";
                            odbcDataReader = dbconn.GetDataReader(msSQL);
                            if (odbcDataReader.HasRows == true)
                            {
                                odbcDataReader.Read();
                                objresponse.token = tokenvalue;
                                objresponse.customer_gid = odbcDataReader["customer_gid"].ToString();
                                objresponse.c_code = values.company_code;
                                objresponse.message = "Login Successfull!";
                                objresponse.dashboard_flag = "SMR";
                                objresponse.status = true;
                            }
                        }
                        else
                        {
                            objresponse.message = "Invalid Credentials !!";
                        }
                    }
                    else
                    {
                        objresponse.message = "Activate the ePortal status for the customer.";
                    }
                }
                else
                {
                    objresponse.message = "Company code can't be empty !!";
                }
            }
            catch (Exception ex)
            {
                objresponse.message = ex.Message.ToString();
            }
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, objresponse);
        }

        public string Token(string customer_code, string password, string company_code = null)
        {
            var pairs = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "grant_type", "password" ),
                            new KeyValuePair<string, string>( "username", customer_code ),
                            new KeyValuePair<string, string> ( "Password", password ),
                            new KeyValuePair<string, string>("Scope",company_code),
                            new KeyValuePair<string, string>("RouterPrefix","api/customerlogin")
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

        [ActionName("ForgotCustomerCode")]
        [HttpPost]
        public HttpResponseMessage ForgotCustomerCode(PostCustomerForgotuser values)
        {
            PostCustomerForgotuser forgotcode = new PostCustomerForgotuser();
            try
            {
                if (!String.IsNullOrEmpty(values.forgoteportal_emailid))
                {
                    msSQL = " select customer_id, eportal_password, eportal_emailid, length from " + values.companyid + ".crm_mst_tcustomer" +
                    " where eportal_emailid='" + values.forgoteportal_emailid.ToLower().Trim() + "'";
                    odbcDataReader = dbconn.GetDataReader(msSQL);
                    if (odbcDataReader.HasRows == true) 
                    {
                        email = odbcDataReader["eportal_emailid"].ToString();
                        customer_code = odbcDataReader["customer_id"].ToString();
                        password = odbcDataReader["eportal_password"].ToString();
                        string length = odbcDataReader["length"].ToString();
                        int originallength = Convert.ToInt32(length);
                        string password1 = ReverseAscii(password, originallength);


                        body = "Dear Customer, <br/>";
                        body += "<br />";
                        body = body + "Greetings,  <br />";
                        body = body + "<br />";
                        body = body + "We received a request to recover your customer code and password. Please find your details below: <br/>";
                        body = body + "<br />";
                        body = body + " <b> Customer Code:  " + customer_code + "<br/>";
                        body = body + "<br />";
                        body = body + " <b> Password:   " + password1 + "<br/>";
                        body = body + "<br />";
                        body = body + "<b> URL: </b>  <a href=\"https://myorders.storyboardsystems.com\">myorders.storyboardsystems.com</a>";
                        body = body + "<br />";
                        body = body + " <br> If you did not request this recovery, please contact our support team immediately at <a href=\"mailto:support@vcidex.com\">support@vcidex.com</a>.<br/>";
                        body = body + "<br />";
                        body = body + "<br />";
                        body = body + " Thank you for your prompt attention to this matter.<br/>";
                        body = body + "<br />";
                        body = body + "<br/>";
                        body = body + "Best Regards,";
                        body = body + "<br/>";
                        body = body + "Support Team";

                        bool status = mail(values.companyid,email, "Your Customer Code and Password Recovery", body);

                        if (status == true)
                        {
                            forgotcode.status = true;
                            forgotcode.message = "Email containing your customer code and password has been successfully dispatched.";
                        }
                        else
                        {
                            forgotcode.status = false;
                            forgotcode.message = "Failed to send the email. Please try again.";
                        }
                    }
                    else
                    {
                        forgotcode.message = "You are not part of the customer. !!";
                    }
                }
                else
                {
                    forgotcode.message = "Mail Address mandorty. !!";
                }
            }
            catch (Exception ex)
            {
                forgotcode.message = ex.Message.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, forgotcode);
        }

        public string ReverseAscii(string encodedStr, int originalLength)
        {
            string reversedWords = string.Empty;

            for (int i = 0; i < encodedStr.Length; i += 3) // Process each group of three characters.
            {
                string numberStr = encodedStr.Substring(i, 3);
                int number = int.Parse(numberStr);
                char character = (char)(number + originalLength);
                reversedWords += character;
            }

            return reversedWords;
        }

        public bool mail(string companycode, string to, string sub, string body)
        {
            try
            {
                msSQL = "SELECT company_mail,pop_server,pop_port,pop_username,pop_password FROM " + companycode + ".adm_mst_tcompany ";
                odbcDataReader = dbconn.GetDataReader(msSQL);
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
        [ActionName("GetCustomerName")]
        [HttpGet]
        public HttpResponseMessage GetCustomerName(string customer_gid)
        {
            GetCustomerName getCustomerName = new GetCustomerName();
            msSQL = "select concat(b.customer_id, ' | ', a.customercontact_name) as customer_name from crm_mst_tcustomercontact a " +
                    " left join crm_mst_tcustomer b on b.customer_gid = a.customer_gid " +
                    " where a.customer_gid='" + customer_gid + "'";
            odbcDataReader = dbconn.GetDataReader(msSQL);
            if (odbcDataReader.HasRows == true)
            {
                odbcDataReader.Read();
                getCustomerName.customer_name = odbcDataReader["customer_name"].ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, getCustomerName);
        }
    }
}

