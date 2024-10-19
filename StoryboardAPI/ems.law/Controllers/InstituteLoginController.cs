using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using static ems.law.Models.MdlInstituteLogin;
using System.Data.Odbc;
using Newtonsoft.Json.Linq;
using System.Data;
using ems.law.Models;

namespace ems.law.Controllers
{
    [RoutePrefix("api/InstituteLogin")]
    [AllowAnonymous]
    public class InstituteLoginController : ApiController
    {
        string msSQL = string.Empty;
        cmnfunctions cmnfunctions =  new cmnfunctions();
        string domain = string.Empty;
        string tokenvalue = string.Empty;
        OdbcDataReader objMySqlDataReader;        
        dbconn objdbconn = new dbconn();
        string lsdefaultScreen, lsemployee_gid;
        DataTable dt_datatable;
        int i;

        [ActionName("IntUserLogin")]
        [HttpPost]

        public HttpResponseMessage IntUserLogin(PostInstituteLogin values)
        {
            InstituteLoginResponse response = new InstituteLoginResponse();
            try
            {
                if (!String.IsNullOrEmpty(values.company_code))
                {
                    msSQL = "select active_flag from " + values.company_code + ".law_mst_tinstitute where institute_code ='" + values.institute_code + "'";
                    string lsstatus = objdbconn.GetExecuteScalar(msSQL);
                    if (lsstatus == "N")
                    {
                        var ObjToken = Token(values.institute_code, cmnfunctions.ConvertToAscii(values.user_password), values.company_code.ToLower());
                        dynamic newobj = JsonConvert.DeserializeObject(ObjToken);
                        if (newobj.access_token != null)
                        {
                            tokenvalue = "Bearer " + newobj.access_token;
                            msSQL = "call adm_mst_spinststoretoken('" + tokenvalue + "','" + values.institute_code + "','" + cmnfunctions.ConvertToAscii(values.user_password) + "','" + values.company_code + "','Institute')";
                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader.HasRows == true)
                            {
                                objMySqlDataReader.Read();
                                response.token = tokenvalue;
                                response.institute_gid = objMySqlDataReader["institute_gid"].ToString();
                                response.c_code = values.company_code;
                                response.message = "Login Successful!";
                                response.status = true;
                                msSQL = " select default_screen from " + response.c_code + ".law_mst_tinstitute where institute_code='" + values.institute_code + "'";
                                lsdefaultScreen = objdbconn.GetExecuteScalar(msSQL);
                                if (lsdefaultScreen != "")
                                {
                                    msSQL = " select sref from " + response.c_code + ".adm_mst_tmodule where module_gid " +
                                        " in (select default_screen from " + response.c_code + ". law_mst_tinstitute" +
                                        " where institute_code='" + values.institute_code + "')"
                                        ;
                                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objMySqlDataReader.HasRows)
                                    {
                                        response.sref = objMySqlDataReader["sref"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                response.message = "Invalid Credentials!";
                            }
                        }
                        else
                        {
                            response.message = "Invalid Credentials!";
                        }
                    }
                    else
                    {
                        response.message = "Institute InActive State!";
                    }
                }
                else
                {
                    response.message = "Company Code cannot be empty!";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message.ToString();
            }
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
        }
        public string Token(string institute_code, string password, string company_code = null)
        {

            var pairs = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "grant_type", "password" ),
                            new KeyValuePair<string, string>( "username", institute_code ),
                            new KeyValuePair<string, string> ( "Password", password ),
                            new KeyValuePair<string, string>("Scope",company_code),
                            new KeyValuePair<string, string>("RouterPrefix","api/InstituteLogin")
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

        public HttpResponseMessage GetInstituteCode(string user_gid)
        {
            Institutename_list Institutename_list = new Institutename_list();
            
            try
            {
                msSQL = " select concat(contact_person,' / ',institute_code) As Name from law_mst_tinstitute  "
                    + " where institute_gid='" + user_gid + "'";
              objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    objMySqlDataReader.Read();
                    Institutename_list.name = objMySqlDataReader["Name"].ToString();
                    objMySqlDataReader.Close();
                }
            }
            catch
            {
              
            }
           return Request.CreateResponse(HttpStatusCode.OK, Institutename_list);
        }

        public void LoginErrorLog(string strVal)
        {
            try
            {
                string lspath = ConfigurationManager.AppSettings["file_path"].ToString() + "/erpdocument/Institue_LOGIN_ERRLOG/" + DateTime.Now.Year + @"\" + DateTime.Now.Month;
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

    }
}