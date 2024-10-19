using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using ems.utilities.Functions;
using static ems.crm.Models.MdlGmailCampaign;
using System.Threading;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using static OfficeOpenXml.ExcelErrorValue;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Services.Description;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;
using MessagePart = Google.Apis.Gmail.v1.Data.MessagePart;
using System.Text;
using File = System.IO.File;
using System.Web.Http.Results;
using System.Data.SqlClient;
using System.Data.Common;
using System.Runtime.CompilerServices;
using OfficeOpenXml.Style;
using static ems.crm.Models.MdlContactManagement;
using System.Collections.Concurrent;
using System.Diagnostics.Eventing.Reader;

namespace ems.crm.DataAccess
{
    public class DaGmailCampaign
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, msGetGid2, body, encodebody, lssent_date, lsbody, lscc, lsbcc, current_label_id, lsshopify_id, lsemail, lsname, lssource_gid, final_path;

        public void DaGmailSaveTemplate(gmailsummary_list values, string user_gid)
        {
            try
            {



                msSQL = "INSERT INTO crm_trn_tgmailtemplate (" +
                           "template_name, " +
                           "template_subject, " +
                           "template_body, " +
                            " created_by, " +
                           "created_date) " +
                           "VALUES (" +
                            "'" + values.template_name.Replace("\"", "\\\"").Replace("'", "\\\'") + "', " +
                           "'" + values.template_subject.Replace("\"", "\\\"").Replace("'", "\\\'") + "', " +
                           "'" + values.template_body.Replace("\"", "\\\"").Replace("'", "\\\'") + "', " +
                             "'" + user_gid + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Campaign Added Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Campaign!!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * *********************" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Gmail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");


                }



            }
            catch (Exception ex)
            {
                values.message = "Error While Adding Mail Tempalte";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Gmail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }

        public void DaGmailTemplateSummary(MdlGmailCampaign values)

        {
            try
            {


                msSQL = "select a.template_gid,a.template_name,a.template_subject,a.template_body,count(b.template_gid) as template_count," +
                   "date_format(a.created_date, '%d-%m-%Y') as date,a.created_by,a.template_flag from crm_trn_tgmailtemplate a left join " +
                    "crm_trn_gmail b on a.template_gid = b.template_gid group by template_gid order by a.created_date; ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailtemplate_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailtemplate_list
                        {

                            created_date = dt["date"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_gid = dt["template_gid"].ToString(),
                            template_subject = dt["template_subject"].ToString(),
                            template_body = dt["template_body"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            template_flag = dt["template_flag"].ToString(),
                            template_count = dt["template_count"].ToString(),



                        });
                        values.gmailtemplate_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Template Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }


        public void DaGmailTemplateSendSummary(MdlGmailCampaign values)

        {
            try
            {
                msSQL = " SELECT a.leadbank_gid,a.leadbank_name,a.source_gid,a.leadbank_region,a.customer_type,CONCAT(a.leadbank_address1," +
                    " ' ', a.leadbank_address2, ' ', a.leadbank_city, ' ', a.leadbank_state, ' ', a.leadbank_country) as address," +
                    " (e.user_firstname) as lead_assignedto,f.source_name,g.region_name,i.email FROM crm_trn_tleadbank a  " +
                    " left join crm_trn_tlead2campaign c on c.leadbank_gid = a.leadbank_gid  left join  hrm_mst_temployee d " +
                    "on d.employee_gid = c.assign_to left join adm_mst_tuser e on e.user_gid = d.user_gid left join crm_mst_tsource f" +
                    " on f.source_gid = a.source_gid left join crm_mst_tregion g on g.region_gid = a.leadbank_region left join " +
                    "crm_trn_tleadbankcontact i on i.leadbank_gid=a.leadbank_gid where i.main_contact ='Y' GROUP BY a.leadbank_gid;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailtemplatesendsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailtemplatesendsummary_list
                        {
                            names = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            //default_phone = dt["mobile"].ToString(),
                            ////created_date = dt["created_date"].ToString(),
                            email = dt["email"].ToString(),
                            address1 = dt["address"].ToString(),
                            // address2 = dt["address2"].ToString(),
                            //city = dt["city"].ToString(),
                            //state = dt["state"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            source_gid = dt["source_gid"].ToString(),
                            //lead_status = dt["lead_status"].ToString(),
                            region = dt["region_name"].ToString(),
                            lead_assignedto = dt["lead_assignedto"].ToString(),



                        });
                        values.gmailtemplatesendsummary_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Template Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }

        public void DaGmailSenditemSummary(MdlGmailCampaign values, string user_gid)

        {
            try
            {
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);

                msSQL = "select a.gmail_gid,a.from_mailaddress,a.to_mailaddress,a.mail_subject,a.mail_body,date_format(a.created_date,'%d %b %Y %h:%i %p') as sent_date,DATE_FORMAT(a.created_date, '%h:%i %p') AS sent_time," +
                      " a.leadbank_gid,b.template_gid from crm_trn_gmail a left join crm_trn_tgmailtemplate b  " +
                      "on  a.template_gid=b.template_gid " +
                      "where a.direction='outgoing' and from_mailaddress = '" + integrated_gmail + "' order by a.created_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailsenditemsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailsenditemsummary_list
                        {
                            from_mailaddress = dt["from_mailaddress"].ToString(),
                            to_mailaddress = dt["to_mailaddress"].ToString(),
                            mail_subject = dt["mail_subject"].ToString(),
                            mail_body = dt["mail_body"].ToString(),
                            sent_date = dt["sent_date"].ToString(),
                            sent_time = dt["sent_time"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            template_gid = dt["template_gid"].ToString(),
                            gmail_gid = dt["gmail_gid"].ToString(),

                        });
                        values.gmailsenditemsummary_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Template Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaGmailindividualSenditemSummary(MdlGmailCampaign values, string leadbank_gid, string user_gid)

        {
            try
            {
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);

                msSQL = "select a.gmail_gid,a.from_mailaddress,a.to_mailaddress,a.mail_subject,a.mail_body,date_format(a.created_date,'%d %b %Y %h:%i %p') as sent_date,DATE_FORMAT(a.created_date, '%h:%i %p') AS sent_time," +
                      " a.leadbank_gid,b.template_gid from crm_trn_gmail a left join crm_trn_tgmailtemplate b  " +
                      "on  a.template_gid=b.template_gid " +
                      "where a.leadbank_gid='" + leadbank_gid + "' and a.direction='outgoing' and from_mailaddress = '" + integrated_gmail + "' order by a.created_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailsenditemsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailsenditemsummary_list
                        {
                            from_mailaddress = dt["from_mailaddress"].ToString(),
                            to_mailaddress = dt["to_mailaddress"].ToString(),
                            mail_subject = dt["mail_subject"].ToString(),
                            mail_body = dt["mail_body"].ToString(),
                            sent_date = dt["sent_date"].ToString(),
                            sent_time = dt["sent_time"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            template_gid = dt["template_gid"].ToString(),
                            gmail_gid = dt["gmail_gid"].ToString(),

                        });
                        values.gmailsenditemsummary_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Template Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public result DaSendGmailTemplate(gmailtemplatesendsummary_list values, string user_gid)
        {
            result result = new result();
            try
            {

                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    DaRefreshAccessTockenGenerate(values.template_gid, values.gmailsendchecklist, user_gid);
                }));
                t.Start();

                result.status = true;
                result.message = "Mail sent successfully!";

            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Mail";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }

        public void DaRefreshAccessTockenGenerate(string template_gid, List<gmailsendchecklist> gmailsendchecklist, string user_gid)
        {


            msSQL = "SELECT template_subject FROM crm_trn_tgmailtemplate WHERE template_gid = '" + template_gid + "';";
            string template_subject = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "SELECT template_body FROM crm_trn_tgmailtemplate WHERE template_gid = '" + template_gid + "';";
            string template_body = objdbconn.GetExecuteScalar(msSQL);

            string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
            string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
            gmailconfiguration getgmailcredentials = gmailcrendentials(integrated_gmail);
            try
            {

                if (integrated_gmail != null && integrated_gmail != "")
                {
                    var options = new RestClient("https://accounts.google.com");
                    var request = new RestRequest("/o/oauth2/token", Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Cookie", "__Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; __Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; NID=511=Kbcaybba2NN2meR6QaA1TfflTMD5X6JNa-vmRpwiHUMRZn4ru7MVloJIso4PHGVH40ORQYUKH1LJvUttyG79Vi5eCXIX4UgloYTQFHN0qYHR67sn7fC32LHA7Cfdbgz4G6g5FhOnXus0SVeSyNx4QPRGFQPakVRKwNlBIxK8FGc; __Host-GAPS=1:7IaKbeoNAD6DkBpPrg7Pl6ppMRE7yQ:a2G_qIglM2A-euWd");
                    var body = @"{" + "\n" +
                    @"    ""client_id"": " + "\"" + getgmailcredentials.client_id + "\"" + "," + "\n" +

                    @"    ""client_secret"":  " + "\"" + getgmailcredentials.client_secret + "\"" + "," + "\n" +
                    @"    ""grant_type"": ""refresh_token"",
" + "\n" +
                    @"    ""refresh_token"": " + "\"" + getgmailcredentials.refresh_token + "" + "\"" + "\n" + @"}";

                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = options.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    MdlGmailCampaign.Rootobject objMdlGmailCampaignResponse = new MdlGmailCampaign.Rootobject();
                    objMdlGmailCampaignResponse = JsonConvert.DeserializeObject<MdlGmailCampaign.Rootobject>(errornetsuiteJSON);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var item in gmailsendchecklist)
                        {
                            var options1 = new RestClient("https://www.googleapis.com");
                            var request1 = new RestRequest("/upload/gmail/v1/users/me/messages/send", Method.POST);
                            request1.AddHeader("Authorization", "Bearer " + objMdlGmailCampaignResponse.access_token + "");
                            request1.AddHeader("Content-Type", "message/rfc822");
                            request1.AddHeader("Cookie", "COMPASS=gmail-api-uploads-blobstore=CgAQt8zQrwYagAEACWuJVwxF5peESk5gbz5AE37T8tg_Yoh-YDKTZilInpK22DHDg7FuuU3LXoN11GAOnyqdLslKu6I5ePsGXCEsd3xSS2yUEWvqsZJtNX4R-ajkWB37okK3XRlg3MQM0P22BdEB5efrYEFEwlQWnrxQUPmucfMFffcwQAMnVi0yfzAB");


                            string dataUrl = template_body;
                            string matchString = Regex.Match(dataUrl, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            Match match = Regex.Match(matchString, @"^data:image\/(?<fileExtension>[a-zA-Z]+);base64,(?<base64Content>.+)$");
                            string base64Content = match.Groups["base64Content"].Value;
                            string fileExtension = match.Groups["fileExtension"].Value;
                            string imageUrl = "<img src=\"cid:image_cid\" alt=\"image\">";
                            string temp_body = Regex.Replace(template_body, "<img.+?src=[\"'](.+?)[\"'].+?>", imageUrl);

                            var body1 = @"From:  " + getgmailcredentials.gmail_address + "" + "\n" +
                            @"To: " + item.email + "" + "\n" +
                            @"Subject:" + template_subject + "" + "\n" +
                            @"MIME-Version: 1.0" + "\n" +
                            @"Content-Type: multipart/related; boundary=""boundary_example""" + "\n" +
                            @"" + "\n" +
                            @"--boundary_example" + "\n" +
                            @"Content-Type: text/html; charset=""UTF-8""" + "\n" +
                            @"MIME-Version: 1.0" + "\n" +
                            @"" + "\n" +
                            @"<html>" + "\n" +
                            @"  <body>" + "\n" +
                            @"    <p>" + temp_body + "</p>" + "\n" +
                            @"  </body>" + "\n" +
                            @"</html>" + "\n" +
                            @"" + "\n" +
                            @"--boundary_example" + "\n" +
                            @"Content-Type: image/jpeg" + "\n" +
                            @"MIME-Version: 1.0" + "\n" +
                            @"Content-Disposition: inline; filename=""image.jpg""" + "\n" +
                            @"Content-ID: <image_cid>" + "\n" +
                            @"Content-Transfer-Encoding: base64" + "\n" +
                            @"" + base64Content + "" +
                            @"--boundary_example" +
                            @"";
                            request1.AddParameter("message/rfc822", body1, ParameterType.RequestBody);
                            IRestResponse response1 = options1.Execute(request1);
                            MdlGmailCampaign.Rootobject1 objMdlGmailCampaignResponse1 = new MdlGmailCampaign.Rootobject1();
                            objMdlGmailCampaignResponse1 = JsonConvert.DeserializeObject<MdlGmailCampaign.Rootobject1>(errornetsuiteJSON);

                            if(response1.StatusCode == HttpStatusCode.OK)
                            {
                                msGetGid = objcmnfunctions.GetMasterGID("GILC");
                                msSQL = "INSERT INTO crm_trn_gmail (" +
                                                "gmail_gid, " +
                                           "template_gid, " +
                                           "to_mailaddress, " +
                                           "from_mailaddress, " +
                                           "transmission_id, " +
                                           "leadbank_gid, " +
                                            " created_by, " +
                                           "created_date) " +
                                           "VALUES (" +
                                             "'" + msGetGid + "'," +
                                             "'" + template_gid + "'," +
                                            "'" + item.email + "', " +
                                           "'" + getgmailcredentials.gmail_address + "', " +
                                           "'" + objMdlGmailCampaignResponse1.id + "', " +
                                           "'" + item.leadbank_gid + "', " +
                                             "'" + user_gid + "'," +
                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 0)
                                {
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                            " * **********Issue while inserting gmail campaign transaction ***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                }

                            }
                            else
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                               "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                               " * **********Issue while sending gmail campaign transaction ***********" + "*****API Response****" + response1 + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                            }
                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                               "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                               " * **********Issue with gmail credentials ***********" + "*****API Response****" + response + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                    }

                }
                else
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + 
                        " * **********No Gmail Integarted ***********" + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }


            //}




        }
        public gmailconfiguration gmailcrendentials(string integrated_gmail)
        {
            gmailconfiguration getgmailcredentials = new gmailconfiguration();
            try
            {
                msSQL = "select *from  crm_smm_gmail_service   where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            return getgmailcredentials;
        }
        public void DaGmailTemplateView(string template_gid, MdlGmailCampaign values)

        {
            try
            {

                msSQL = "select template_name,template_subject,template_body,created_date from crm_trn_tgmailtemplate where template_gid='" + template_gid + "';";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailtemplate_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailtemplate_list
                        {
                            template_body = dt["template_body"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_subject = dt["template_subject"].ToString(),
                            created_date = dt["created_date"].ToString(),

                        });
                        values.gmailtemplate_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fecthing Tempalte View";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaGetattachement(string mail_gid, MdlGmailCampaign values)

        {
            try
            {

                msSQL = "select document_name, document_path from crm_trn_tfiles where mailmanagement_gid = '" + mail_gid + "'; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<allattchement_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new allattchement_list
                        {
                            document_name = dt["document_name"].ToString(),
                            document_path = dt["document_path"].ToString(),

                        });
                        values.allattchement_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fecthing Tempalte View";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }



        public void DaUpdateGmailTemplateStatus(gmailtemplatesendsummary_list values)
        {
            try
            {


                msSQL = "select template_flag from crm_trn_tgmailtemplate where template_gid='" + values.template_gid + "'";
                string template_flag = objdbconn.GetExecuteScalar(msSQL);
                if (template_flag == "Y")
                {
                    msSQL = "update crm_trn_tgmailtemplate set template_flag='N' where template_gid='" + values.template_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = "update crm_trn_tgmailtemplate set template_flag='Y' where template_gid='" + values.template_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Template Status Updated Sucessfully!!";
                }
            }
            catch (Exception ex)
            {

                values.message = "Error While Updating Template Status";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");


            }

        }
        public static async Task<GmailService> GetGmailService(string clientId, string clientSecret, string refreshToken)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = new ClientSecrets
                        {
                            ClientId = clientId,
                            ClientSecret = clientSecret
                        },
                        Scopes = new[] { GmailService.Scope.MailGoogleCom },
                        // DataStore = new FileDataStore("GmailAPI")
                    });

                    var token = new TokenResponse
                    {
                        RefreshToken = refreshToken
                    };

                    var credential = new UserCredential(flow, Environment.UserName, token);

                    return new GmailService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Gmail API Example"
                    });
                });
            }
            catch (Exception ex)
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logFileName = $"Log{DateTime.Now:yyyy-MM-dd_HH}.txt";

                string logMessage = $"*******Date***** {timestamp} *********** " +
                                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name} *********** " +
                                    $"{ex} ***********";

                string logFilePath = $"SocialMedia/ErrorLog/Mail/{logFileName}";

                cmnfunctions objcmnfunctions = new cmnfunctions(); // Create an instance of the logging class
                objcmnfunctions.LogForAudit(logMessage, logFilePath);
                throw; // Re-throw the exception after logging it
            }
        }



        public void DaGmailSendStatusSummary(string template_gid, MdlGmailCampaign values)

        {
            try
            {


                msSQL1 = "select template_name from crm_trn_tgmailtemplate where  template_gid='" + template_gid + "' ";
                string lstemplate_name;
                lstemplate_name = objdbconn.GetExecuteScalar(msSQL1);

                //msSQL = "select a.gmail_gid,a.to_mailaddress,a.created_by,date_format(a.created_date,'%d-%m-%Y')as dates," +
                //    "direction,b.template_name,c.customer_type from crm_trn_gmail a left join crm_trn_tgmailtemplate b on" +
                //    " a.template_gid =b.template_gid left join  crm_trn_tleadbank c on c.leadbank_gid=a.leadbank_gid where b.template_gid='" + template_gid + "' order by a.created_date desc;";
                msSQL = "select a.gmail_gid,a.to_mailaddress,a.created_by,a.created_date,date_format(a.created_date,'%d-%m-%Y')as dates," +
                    "time_format(a.created_date,'%h:%i:%s')as created_time, direction,b.template_name,a.leadbank_gid,c.customer_type," +
                    "d.source_name,e.region_name from crm_trn_gmail a left join crm_trn_tgmailtemplate b on a.template_gid =b.template_gid " +
                    "left join crm_trn_tleadbank c on c.leadbank_gid=a.leadbank_gid  left join crm_mst_tsource d on d.source_gid=c.source_gid " +
                    " left join crm_mst_tregion e  on e.region_gid=c.leadbank_region where b.template_gid='" + template_gid + "' order by a.created_date desc;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailtemplatesendsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailtemplatesendsummary_list
                        {
                            gmail_gid = dt["gmail_gid"].ToString(),
                            to_mailaddress = dt["to_mailaddress"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            dates = dt["dates"].ToString(),
                            direction = dt["direction"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            region_name = dt["region_name"].ToString(),



                        });
                        values.gmailtemplatesendsummary_list = getmodulelist;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Mail Send Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }


        public void DaGmailinboxSummary(MdlGmailCampaign values, string user_gid)
        {
            string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
            string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
            gmailconfiguration getgmailcredentials = gmailcrendentials(integrated_gmail);
            try
            {
                msSQL = "select a.gmail_gid,a.to_mailaddress,a.created_by,a.created_date,date_format(a.created_date,'%d-%m-%Y')as dates,time_format(a.created_date,'%h:%i:%s')as created_time," +
                   "direction,b.template_name,a.leadbank_gid,c.customer_type from crm_trn_gmail a left join crm_trn_tgmailtemplate b on" +
                   " a.template_gid =b.template_gid left join crm_trn_tleadbank c on c.leadbank_gid=a.leadbank_gid  order by a.created_date desc;";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getmodulelist = new List<gmailtemplatesendsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailtemplatesendsummary_list
                        {
                            gmail_gid = dt["gmail_gid"].ToString(),
                            to_mailaddress = dt["to_mailaddress"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            dates = dt["dates"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_time = dt["created_time"].ToString(),
                            direction = dt["direction"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            customer_type = dt["customer_type"].ToString(),


                        });
                        values.gmailtemplatesendsummary_list = getmodulelist;
                    }
                }

                values.gmail_address = getgmailcredentials.gmail_address;
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaGmailAPIinboxSummary(MdlGmailCampaign values, string user_gid)
        {


            try
            {


                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                //string formattedEmails = string.Join(",", integrated_gmail.Split(',').Select(email => $"'{email.Trim()}'"));

                //msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time," +
                //    " cc, subject, body, attachement_flag,read_flag,integrated_gmail FROM crm_trn_tgmailinbox where inbox_status is Null and  integrated_gmail in (" + formattedEmails + ") ORDER BY s_no DESC";
                //dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time," +
                    " cc, subject, body, attachement_flag,read_flag,integrated_gmail FROM crm_trn_tgmailinbox where inbox_status is Null and  integrated_gmail='" + integrated_gmail + "' ORDER BY s_no DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                values.gmailapiinboxsummary_list = dt_datatable.AsEnumerable().AsParallel()
                   .Select(row => new gmailapiinboxsummary_list
                   {
                       s_no = row["s_no"].ToString(),
                       inbox_id = row["inbox_id"].ToString(),
                       from_id = row["from_id"].ToString(),
                       sent_date = row["sent_date"].ToString(),
                       sent_time = row["sent_time"].ToString(),
                       cc = row["cc"].ToString(),
                       subject = DecodeFromBase64(row["subject"].ToString()),
                       body = row["body"].ToString(),
                       attachement_flag = row["attachement_flag"].ToString(),
                       read_flag = row["read_flag"].ToString(),
                       integrated_gmail = row["integrated_gmail"].ToString(),
                   })
                   .ToList();

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }

        public void DaGmailAPIinboxTrashSummary(MdlGmailCampaign values, string user_gid)
        {


            try
            {
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time, cc," +
                    " subject, body, attachement_flag,read_flag,integrated_gmail  FROM crm_trn_tgmailinbox where inbox_status ='Trash'  and  integrated_gmail='" + integrated_gmail + "' ORDER BY s_no DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                values.gmailapiinboxsummary_list = dt_datatable.AsEnumerable().AsParallel()
                   .Select(row => new gmailapiinboxsummary_list
                   {
                       s_no = row["s_no"].ToString(),
                       inbox_id = row["inbox_id"].ToString(),
                       from_id = row["from_id"].ToString(),
                       sent_date = row["sent_date"].ToString(),
                       sent_time = row["sent_time"].ToString(),
                       cc = row["cc"].ToString(),
                       subject = DecodeFromBase64(row["subject"].ToString()),
                       body = row["body"].ToString(),
                       attachement_flag = row["attachement_flag"].ToString(),
                       read_flag = row["read_flag"].ToString(),
                       integrated_gmail = row["integrated_gmail"].ToString(),
                   })
                   .ToList();

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public async Task DaReplyInboxMail(replymail_list values)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                //string clientId = ConfigurationManager.AppSettings["clientId"];
                //string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
                //string refreshToken = ConfigurationManager.AppSettings["refreshToken"];
                msSQL = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + values.inbox_id + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                }
                objOdbcDataReader.Close();
                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;

                // Get the Gmail service instance
                var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                // Fetch the original message (you need to determine how to fetch messageId)
                var originalMessage = await gmailService.Users.Messages.Get("me", values.inbox_id).ExecuteAsync();

                // Extract relevant information from the original message
                string originalSubject = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                string originalFrom = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                string originalTo = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value;
                string originalCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;
                string originalBCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "BCC")?.Value;
                string originalDateStr = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;

                msSQL = "select body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,cc,bcc from  crm_trn_tgmailinbox where inbox_id='" + values.inbox_id + "' limit 1 ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsbody = DecodeBase64(objOdbcDataReader["body"].ToString());
                    lssent_date = objOdbcDataReader["sent_date"].ToString();
                    lscc = objOdbcDataReader["cc"].ToString();
                    lsbcc = objOdbcDataReader["bcc"].ToString();

                }
                objOdbcDataReader.Close();
                // Ensure emailBody and original_body retain their HTML formatting
                string emailBody = values.emailBody; // Assuming values.emailBody is already in HTML format
                string original_body = lsbody; // Assuming values.orginal_body is already in HTML format
                string sent_date = lssent_date;
                encodebody = EncodeToBase64(emailBody);
                // string reply_body = StripHtmlTags(emailBody.ToString());
                string replyBody = $"{emailBody}<br><br> On {sent_date}, {values.replytoid} wrote:<br><br>{original_body}";
                string subject = $"Re: {originalSubject}";

                // Create MIME message manually
                string boundary = "----=_Part_0_" + Guid.NewGuid().ToString("N");

                var mimeMessage = new StringBuilder();
                mimeMessage.AppendLine($"To: {values.replytoid}");
                mimeMessage.AppendLine($"Subject: {subject}");
                if (!string.IsNullOrEmpty(values.replyccid)) mimeMessage.AppendLine($"Cc: {values.replyccid}");
                if (!string.IsNullOrEmpty(values.replybccid)) mimeMessage.AppendLine($"Bcc: {values.replybccid}");
                mimeMessage.AppendLine("Content-Type: multipart/alternative; boundary=\"" + boundary + "\"");
                mimeMessage.AppendLine();
                mimeMessage.AppendLine("--" + boundary);
                mimeMessage.AppendLine("Content-Type: text/html; charset=UTF-8");
                mimeMessage.AppendLine("Content-Transfer-Encoding: 7bit");
                mimeMessage.AppendLine();
                mimeMessage.AppendLine(replyBody);
                mimeMessage.AppendLine();
                mimeMessage.AppendLine("--" + boundary + "--");

                // Encode the message to base64url
                var encodedRawMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage.ToString()))
                                                .Replace('+', '-')
                                                .Replace('/', '_')
                                                .Replace("=", "");

                // Create the reply message
                var replyMessage = new Google.Apis.Gmail.v1.Data.Message
                {
                    Raw = encodedRawMessageContent
                };

                // Send the reply message using Gmail API
                var sendMessageRequest = gmailService.Users.Messages.Send(replyMessage, "me");
                var sendMessageResponse = await sendMessageRequest.ExecuteAsync();
                string sentMessageId = sendMessageResponse.Id;
                // Check if message was sent successfully
                if (sendMessageResponse != null && sendMessageResponse.LabelIds.Contains("SENT"))
                {
                    string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Insert into crm_trn_tgmailinboxreply
                    msSQL = "INSERT INTO crm_trn_tgmailinboxreply (" +
                            "inbox_id, " +
                            "reply_id, " +
                            "from_id, " +
                            "subject, " +
                            "both_body, " +
                            "cc, " +
                            "bcc, " +
                            "body, " +
                            "attachement_flag, " +
                    "sent_date) " +
                             "VALUES (" +
                         "'" + values.inbox_id + "', " +
                          "'" + sentMessageId + "', " +
                        "'" + values.replytoid + "', " +
                        "'" + EncodeToBase64(subject) + "', " +
                        "'" + encodedRawMessageContent + "', " +
                        "'" + values.replyccid + "', " +
                        "'" + values.replybccid + "', " +
                        "'" + EncodeToBase64(values.emailBody) + "', " +
                          "'N', " +
                        "'" + formattedDateString + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Reply Mail Sent Successfully!";
                    }
                    else
                    {
                        values.message = "Error: Failed to insert reply mail into database.";
                        values.status = false;
                        // Log the error
                        objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                else
                {
                    // If sending failed
                    values.message = "Error: Failed to send reply mail.";
                    values.status = false;
                    objcmnfunctions.LogForAudit("Error: Failed to send reply mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error While Reply Mail !!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaReplyInboxMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }
        public async Task DaReplyOrForwardInboxMail(forwardmail_list values)
        {
            try
            {

                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                //string clientId = ConfigurationManager.AppSettings["clientId"];
                //string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
                //string refreshToken = ConfigurationManager.AppSettings["refreshToken"];
                string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + values.inbox_id + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                }
                objOdbcDataReader.Close();
                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;
                if (values.reply_id == "No")
                {
                    // Get the Gmail service instance
                    var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                    // Fetch the original message (you need to determine how to fetch messageId)
                    var originalMessage = await gmailService.Users.Messages.Get("me", values.inbox_id).ExecuteAsync();

                    // Extract relevant information from the original message
                    string originalSubject = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                    string originalFrom = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                    string originalTo = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value;
                    string originalCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;
                    string originalBCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "BCC")?.Value;
                    string originalDateStr = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;
                    msSQL = "select body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,cc,bcc from  crm_trn_tgmailinbox where inbox_id='" + values.inbox_id + "' limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsbody = DecodeBase64(objOdbcDataReader["body"].ToString());
                        lssent_date = objOdbcDataReader["sent_date"].ToString();
                        //lscc = objOdbcDataReader["cc"].ToString();
                        //lsbcc = objOdbcDataReader["bcc"].ToString();

                    }
                    objOdbcDataReader.Close();

                    // Ensure emailBody and original_body retain their HTML formatting
                    string emailBody = values.emailBody; // Assuming values.emailBody is already in HTML format
                    string original_body = lsbody; // Assuming values.orginal_body is already in HTML format
                    string sent_date = lssent_date;

                    // Compose the forwarded message body
                    string forwardBody = $"<br>{emailBody}<br><br> From: {originalFrom}<br>Date: {sent_date}<br>Subject: {originalSubject}<br><br>{original_body}";

                    // Create MIME message manually
                    string boundary = "----=_Part_0_" + Guid.NewGuid().ToString("N");

                    var mimeMessage = new StringBuilder();
                    string[] recipients = values.forwardto.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(r => r.Trim())
                                      .ToArray();

                    // Append each recipient to the mimeMessage
                    foreach (string recipient in recipients)
                    {
                        mimeMessage.AppendLine($"To: {recipient}");
                    }
                    //mimeMessage.AppendLine($"To: {values.forwardto}"); // Forward to the original recipient
                    mimeMessage.AppendLine($"Subject: Fwd: {originalSubject}");
                    //if (!string.IsNullOrEmpty(originalCC)) mimeMessage.AppendLine($"Cc: {lscc}");
                    //if (!string.IsNullOrEmpty(originalBCC)) mimeMessage.AppendLine($"Bcc: {lsbcc}");
                    mimeMessage.AppendLine("Content-Type: multipart/alternative; boundary=\"" + boundary + "\"");
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine("--" + boundary);
                    mimeMessage.AppendLine("Content-Type: text/html; charset=UTF-8");
                    mimeMessage.AppendLine("Content-Transfer-Encoding: 7bit");
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine(forwardBody);
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine("--" + boundary + "--");

                    // Encode the message to base64url
                    var encodedRawMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage.ToString()))
                                                    .Replace('+', '-')
                                                    .Replace('/', '_')
                                                    .Replace("=", "");

                    // Create the forwarded message
                    var forwardedMessage = new Google.Apis.Gmail.v1.Data.Message
                    {
                        Raw = encodedRawMessageContent
                    };

                    // Send the forwarded message using Gmail API
                    var sendMessageRequest = gmailService.Users.Messages.Send(forwardedMessage, "me");
                    var sendMessageResponse = await sendMessageRequest.ExecuteAsync();
                    string sentMessageId = sendMessageResponse.Id;

                    // Check if message was sent successfully
                    if (sendMessageResponse != null && sendMessageResponse.LabelIds.Contains("SENT"))
                    {
                        string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // Insert into crm_trn_tgmailinboxreply
                        string msSQL = "INSERT INTO crm_trn_tgmailinboxreplyforward (" +
                                        "inbox_id, " +
                                        "reply_id, " +
                                        "forward_id, " +
                                        "to_id, " +
                                        "subject, " +
                                        "both_body, " +
                                        "cc, " +
                                        "bcc, " +
                                        "body, " +
                                        "attachement_flag, " +
                                        "sent_date) " +
                                        "VALUES (" +
                                        "'" + values.inbox_id + "', " +
                                       "'" + values.reply_id + "', " +
                                        "'" + sentMessageId + "', " +
                                        "'" + values.forwardto + "', " +
                                        "'" + EncodeToBase64($"Fwd: {originalSubject}") + "', " +
                                        "'" + encodedRawMessageContent + "', " +
                                        "'" + originalCC + "', " +
                                        "'" + originalBCC + "', " +
                                        "'" + EncodeToBase64(emailBody) + "', " +
                                        "'" + values.attachement_flag + "', " +
                                        "'" + formattedDateString + "')";

                        int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Forward Mail Sent Successfully!";
                        }
                        else
                        {
                            values.message = "Error: Failed to insert forward mail into database.";
                            values.status = false;
                            // Log the error
                            objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    else
                    {
                        // If sending failed
                        values.message = "Error: Failed to send forward mail.";
                        values.status = false;
                        objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                else
                {
                    // Get the Gmail service instance
                    var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                    // Fetch the original message (you need to determine how to fetch messageId)
                    var originalMessage = await gmailService.Users.Messages.Get("me", values.reply_id).ExecuteAsync();

                    // Extract relevant information from the original message
                    string originalSubject = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                    string originalFrom = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                    string originalTo = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value;
                    string originalCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;
                    string originalBCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "BCC")?.Value;
                    string originalDateStr = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;
                    msSQL = "select body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date from  crm_trn_tgmailinboxreply where reply_id='" + values.reply_id + "' limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsbody = DecodeBase64(objOdbcDataReader["body"].ToString());
                        lssent_date = objOdbcDataReader["sent_date"].ToString();

                    }
                    objOdbcDataReader.Close();

                    // Ensure emailBody and original_body retain their HTML formatting
                    string emailBody = values.emailBody; // Assuming values.emailBody is already in HTML format
                    string original_body = lsbody; // Assuming values.orginal_body is already in HTML format
                    string sent_date = lssent_date;

                    // Compose the forwarded message body
                    string forwardBody = $"<br>{emailBody}<br><br> From: {originalFrom}<br>Date: {sent_date}<br>Subject: {originalSubject}<br><br>{original_body}";

                    // Create MIME message manually
                    string boundary = "----=_Part_0_" + Guid.NewGuid().ToString("N");

                    var mimeMessage = new StringBuilder();
                    string[] recipients = values.forwardto.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(r => r.Trim())
                                      .ToArray();

                    // Append each recipient to the mimeMessage
                    foreach (string recipient in recipients)
                    {
                        mimeMessage.AppendLine($"To: {recipient}");
                    }
                    // mimeMessage.AppendLine($"To: {values.forwardto}"); // Forward to the original recipient
                    mimeMessage.AppendLine($"Subject: Fwd: {originalSubject}");
                    //if (!string.IsNullOrEmpty(originalCC)) mimeMessage.AppendLine($"Cc: {originalCC}");
                    //if (!string.IsNullOrEmpty(originalBCC)) mimeMessage.AppendLine($"Bcc: {originalBCC}");
                    mimeMessage.AppendLine("Content-Type: multipart/alternative; boundary=\"" + boundary + "\"");
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine("--" + boundary);
                    mimeMessage.AppendLine("Content-Type: text/html; charset=UTF-8");
                    mimeMessage.AppendLine("Content-Transfer-Encoding: 7bit");
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine(forwardBody);
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine("--" + boundary + "--");

                    // Encode the message to base64url
                    var encodedRawMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage.ToString()))
                                                    .Replace('+', '-')
                                                    .Replace('/', '_')
                                                    .Replace("=", "");

                    // Create the forwarded message
                    var forwardedMessage = new Google.Apis.Gmail.v1.Data.Message
                    {
                        Raw = encodedRawMessageContent
                    };

                    // Send the forwarded message using Gmail API
                    var sendMessageRequest = gmailService.Users.Messages.Send(forwardedMessage, "me");
                    var sendMessageResponse = await sendMessageRequest.ExecuteAsync();
                    string sentMessageId = sendMessageResponse.Id;

                    // Check if message was sent successfully
                    if (sendMessageResponse != null && sendMessageResponse.LabelIds.Contains("SENT"))
                    {
                        string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // Insert into crm_trn_tgmailinboxreply
                        string msSQL = "INSERT INTO crm_trn_tgmailinboxreplyforward (" +
                                        "inbox_id, " +
                                        "reply_id, " +
                                        "forward_id, " +
                                        "to_id, " +
                                        "subject, " +
                                        "both_body, " +
                                        "cc, " +
                                        "bcc, " +
                                        "body, " +
                                        "attachement_flag, " +
                                        "sent_date) " +
                                        "VALUES (" +
                                        "'" + values.inbox_id + "', " +
                                       "'" + values.reply_id + "', " +
                                        "'" + sentMessageId + "', " +
                                        "'" + values.forwardto + "', " +
                                        "'" + EncodeToBase64($"Fwd: {originalSubject}") + "', " +
                                        "'" + encodedRawMessageContent + "', " +
                                        "'" + originalCC + "', " +
                                        "'" + originalBCC + "', " +
                                        "'" + EncodeToBase64(emailBody) + "', " +
                                        "'N', " +
                                        "'" + formattedDateString + "')";

                        int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Forward Mail Sent Successfully!";
                        }
                        else
                        {
                            values.message = "Error: Failed to insert forward mail into database.";
                            values.status = false;
                            // Log the error
                            objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    else
                    {
                        // If sending failed
                        values.message = "Error: Failed to send forward mail.";
                        values.status = false;
                        objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error While Forward Mail !!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaReplyOrForwardInboxMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }
        public async Task DaReplyOrForwardInboxMailWithAttach(forwardmail_list values)
        {
            try
            {

                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                //string clientId = ConfigurationManager.AppSettings["clientId"];
                //string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
                //string refreshToken = ConfigurationManager.AppSettings["refreshToken"];
                string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + values.inbox_id + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                }
                objOdbcDataReader.Close();
                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;
                var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                // Fetch the original message
                var originalMessage = await gmailService.Users.Messages.Get("me", values.reply_id == "No" ? values.inbox_id : values.reply_id).ExecuteAsync();

                // Extract relevant information from the original message
                string originalSubject = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                string originalFrom = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                string originalTo = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value;
                string originalCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;
                string originalBCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "BCC")?.Value;

                string msSQL = values.reply_id == "No"
                    ? $"SELECT body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date FROM crm_trn_tgmailinbox WHERE inbox_id='{values.inbox_id}' LIMIT 1"
                    : $"SELECT body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date FROM crm_trn_tgmailinboxreply WHERE reply_id='{values.reply_id}' LIMIT 1";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsbody = DecodeBase64(objOdbcDataReader["body"].ToString());
                    lssent_date = objOdbcDataReader["sent_date"].ToString();
                }
                objOdbcDataReader.Close();

                string emailBody = values.emailBody; // Assuming values.emailBody is already in HTML format
                string original_body = lsbody; // Assuming original_body is already in HTML format
                string sent_date = lssent_date;

                // Compose the forwarded message body
                string forwardBody = $"<br>{emailBody}<br><br> From: {originalFrom}<br>Date: {sent_date}<br>Subject: {originalSubject}<br><br>{original_body}";

                // Create MIME message manually with attachments
                string boundary = "----=_Part_0_" + Guid.NewGuid().ToString("N");

                var mimeMessage = new StringBuilder();
                string[] recipients = values.forwardto.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(r => r.Trim())
                                      .ToArray();

                // Append each recipient to the mimeMessage
                foreach (string recipient in recipients)
                {
                    mimeMessage.AppendLine($"To: {recipient}");
                }
                // mimeMessage.AppendLine($"To: {values.forwardto}");
                mimeMessage.AppendLine($"Subject: Fwd: {originalSubject}");
                //if (!string.IsNullOrEmpty(originalCC)) mimeMessage.AppendLine($"Cc: {originalCC}");
                //if (!string.IsNullOrEmpty(originalBCC)) mimeMessage.AppendLine($"Bcc: {originalBCC}");
                mimeMessage.AppendLine($"Content-Type: multipart/mixed; boundary=\"{boundary}\"");
                mimeMessage.AppendLine();
                mimeMessage.AppendLine($"--{boundary}");
                mimeMessage.AppendLine("Content-Type: text/html; charset=UTF-8");
                mimeMessage.AppendLine("Content-Transfer-Encoding: 7bit");
                mimeMessage.AppendLine();
                mimeMessage.AppendLine(forwardBody);
                mimeMessage.AppendLine();
                if (values.reply_id == "No")
                {
                    msSQL = "SELECT s_no, inbox_id, original_filename, modified_filename, file_path FROM crm_trn_tgmailinboxattachment where inbox_id='" + values.inbox_id + "'  ORDER BY  s_no asc";
                }
                else
                {
                    msSQL = "SELECT s_no, inbox_id, original_filename, modified_filename, file_path FROM crm_trn_tgmailreplyattachment where reply_id='" + values.reply_id + "'  ORDER BY  s_no asc";

                }
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {


                        string filepath = ConfigurationManager.AppSettings["fwdfilepath"] + dt["file_path"].ToString();
                        var contentType = MimeMapping.GetMimeMapping(filepath);
                        string attachmentData = Convert.ToBase64String(File.ReadAllBytes(filepath));
                        mimeMessage.AppendLine($"--{boundary}");
                        mimeMessage.AppendLine($"Content-Type: {contentType}; name=\"{dt["original_filename"].ToString()}\"");
                        mimeMessage.AppendLine("Content-Transfer-Encoding: base64");
                        mimeMessage.AppendLine("Content-Disposition: attachment");
                        mimeMessage.AppendLine();
                        mimeMessage.AppendLine(attachmentData);
                        mimeMessage.AppendLine();

                    }
                }


                mimeMessage.AppendLine($"--{boundary}--");

                // Encode the message to base64url
                var encodedRawMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage.ToString()))
                                                .Replace('+', '-')
                                                .Replace('/', '_')
                                                .Replace("=", "");

                // Create the forwarded message
                var forwardedMessage = new Google.Apis.Gmail.v1.Data.Message
                {
                    Raw = encodedRawMessageContent
                };

                // Send the forwarded message using Gmail API
                var sendMessageRequest = gmailService.Users.Messages.Send(forwardedMessage, "me");
                var sendMessageResponse = await sendMessageRequest.ExecuteAsync();
                string sentMessageId = sendMessageResponse.Id;

                // Check if message was sent successfully
                if (sendMessageResponse != null && sendMessageResponse.LabelIds.Contains("SENT"))
                {
                    string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Insert into crm_trn_tgmailinboxreplyforward
                    msSQL = "INSERT INTO crm_trn_tgmailinboxreplyforward (" +
                            "inbox_id, " +
                            "reply_id, " +
                            "forward_id, " +
                            "to_id, " +
                            "subject, " +
                            "both_body, " +
                            "cc, " +
                            "bcc, " +
                            "body, " +
                            "attachement_flag, " +
                            "sent_date) " +
                            "VALUES (" +
                            $"'{values.inbox_id}', " +
                            $"'{values.reply_id}', " +
                            $"'{sentMessageId}', " +
                            $"'{values.forwardto}', " +
                            $"'{EncodeToBase64($"Fwd: {originalSubject}")}', " +
                            $"'{encodedRawMessageContent}', " +
                            $"'{originalCC}', " +
                            $"'{originalBCC}', " +
                            $"'{EncodeToBase64(emailBody)}', " +
                            $"'{values.attachement_flag}', " +
                            $"'{formattedDateString}')";

                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {

                        values.status = true;
                        values.message = "Forward Mail Sent Successfully!";
                    }
                    else
                    {
                        values.message = "Error: Failed to insert forward mail into database.";
                        values.status = false;
                        // Log the error
                        objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                else
                {
                    // If sending failed
                    values.message = "Error: Failed to send forward mail.";
                    values.status = false;
                    objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error While Forwarding Mail !!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaReplyOrForwardInboxMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }
        public async Task DaGetEmailLabelDetails(MdlGmailCampaign values, string user_gid)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid='" + user_gid + "'";
                string gmail_address = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL = "select * from crm_smm_gmail_service  WHERE gmail_address = '" + gmail_address + "' LIMIT 1";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<gmailmultiple_credentials>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gmailmultiple_credentials
                        {
                            client_id = dt["client_id"].ToString(),
                            client_secret = dt["client_secret"].ToString(),
                            refresh_token = dt["refresh_token"].ToString(),
                            gmail_address = dt["gmail_address"].ToString(),
                            s_no = dt["s_no"].ToString()
                        });
                    }
                }

                for (int i = 0; i < getModuleList.Count; i++)
                {
                    string clientId = getModuleList[i].client_id;
                    string clientSecret = getModuleList[i].client_secret;
                    string refreshToken = getModuleList[i].refresh_token;

                    var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                    var request = gmailService.Users.Labels.List("me");
                    var response = await request.ExecuteAsync();

                    msSQL = " select company_code from  adm_mst_tcompany limit 1";
                    string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    foreach (var label in response.Labels)
                    {
                        if (label.Type == "user")
                        {
                            // Insert the main label
                            msSQL = " select label_id  from crm_trn_tgmailllabel where label_id='" + label.Id + "'";
                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count == 0)
                            {
                                string msSQLMain = "INSERT INTO crm_trn_tgmailllabel (" +
                                               "label_id, " +
                                               "label_name, " +
                                               "label_type, " +
                                               "company_code, " +
                                               "integrated_gmail, " +
                                               "created_by, " +
                                               "created_date) " +
                                               "VALUES (" +
                                               "'" + label.Id + "', " +
                                               "'" + label.Name.Replace("'", "''") + "', " +
                                               "'" + label.Type + "', " +
                                                "'" + lscompany_code + "', " +
                                                "'" + getModuleList[i].gmail_address + "', " +
                                               "'" + user_gid + "', " +
                                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                int mnResultMain = objdbconn.ExecuteNonQuerySQL(msSQLMain);
                            }

                            // If label name contains '/', insert sub-labels
                            if (label.Name.Contains("/"))
                            {
                                string parentName = label.Name.Split('/')[0];
                                foreach (var subLabel in response.Labels)
                                {
                                    if (subLabel.Name.StartsWith(parentName + "/"))
                                    {
                                        msSQL = " select label_id  from crm_trn_tgmailllabel where label_id='" + label.Id + "'";
                                        dt_datatable = objdbconn.GetDataTable(msSQL);
                                        if (dt_datatable.Rows.Count == 0)
                                        {
                                            string msSQLSub = "INSERT INTO crm_trn_tgmailllabel (" +
                                                           "label_id, " +
                                                           "label_name, " +
                                                           "label_type, " +
                                                           "company_code, " +
                                                           "integrated_gmail, " +
                                                           "created_by, " +
                                                           "created_date) " +
                                                           "VALUES (" +
                                                           "'" + subLabel.Id + "', " +
                                                           "'" + subLabel.Name.Replace("'", "''") + "', " +
                                                           "'" + subLabel.Type + "', " +
                                                           "'" + lscompany_code + "', " +
                                                           "'" + getModuleList[i].gmail_address + "', " +
                                                           "'" + user_gid + "', " +
                                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                            int mnResultSub = objdbconn.ExecuteNonQuerySQL(msSQLSub);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit($"Error in DaGetEmailLabelDetails: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw;
            }
        }
        public void DaGetMailFolderDetails(MdlGmailCampaign values, string user_gid)
        {
            try
            {

                msSQL = " select company_code from  adm_mst_tcompany limit 1 ";
                string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                msSQL = "select s_no, label_id, label_name, label_type, created_by, created_date, company_code,integrated_gmail from  crm_trn_tgmailllabel  where company_code='" + lscompany_code + "' and integrated_gmail ='" + integrated_gmail + "' order by label_name asc ";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetMailFolderDetails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetMailFolderDetails_list
                        {
                            s_no = dt["s_no"].ToString(),
                            label_id = dt["label_id"].ToString(),
                            label_name = dt["label_name"].ToString(),
                            label_type = dt["label_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            company_code = dt["company_code"].ToString(),
                            integrated_gmail = dt["integrated_gmail"].ToString(),
                        });
                        values.GetMailFolderDetails_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                objcmnfunctions.LogForAudit($"*******Date***** {DateTime.Now:yyyy-MM-dd HH:mm:ss} *********** DataAccess:{methodName} ********** {ex.ToString()} *********** Error While Fetching Mail configuration Summary ***** Query**** {msSQL} *******Apiref********", $"SocialMedia/ErrorLog/Mail/Log{DateTime.Now:yyyy-MM-dd HH}.txt");
                throw; // Optionally handle or log the exception
            }
        }
        public void DaGmailAPIinboxFolderSummary(MdlGmailCampaign values, string label_id, string user_gid)
        {


            try
            {
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time, cc, " +
                    "subject, body, attachement_flag,read_flag,integrated_gmail FROM crm_trn_tgmailinbox where inbox_status='" + label_id + "'   and  integrated_gmail='" + integrated_gmail + "' ORDER BY s_no DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                values.gmailapiinboxsummary_list = dt_datatable.AsEnumerable().AsParallel()
                   .Select(row => new gmailapiinboxsummary_list
                   {
                       s_no = row["s_no"].ToString(),
                       inbox_id = row["inbox_id"].ToString(),
                       from_id = row["from_id"].ToString(),
                       sent_date = row["sent_date"].ToString(),
                       sent_time = row["sent_time"].ToString(),
                       cc = row["cc"].ToString(),
                       subject = DecodeFromBase64(row["subject"].ToString()),
                       body = row["body"].ToString(),
                       attachement_flag = row["attachement_flag"].ToString(),
                       read_flag = row["read_flag"].ToString(),
                       integrated_gmail = row["integrated_gmail"].ToString(),
                   })
                   .ToList();

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaCreateEmailLabel(createlabel_list values, string user_gid)
        {
            try
            {
                string msSQL = "select label_id from crm_trn_tgmailllabel where label_name='" + values.labelName + "'";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count == 0)
                {
                    gmailconfiguration getgmailcredentials = new gmailconfiguration();


                    string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
                    string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                    if (integrated_gmail != null && integrated_gmail != "")
                    {
                        string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                        if (objOdbcDataReader.HasRows == true)
                        {

                            getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                            getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                            getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                            getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                        }
                        objOdbcDataReader.Close();

                        msSQL = "select company_code from adm_mst_tcompany limit 1";
                        string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                        string clientId = getgmailcredentials.client_id;
                        string clientSecret = getgmailcredentials.client_secret;
                        string refreshToken = getgmailcredentials.refresh_token;
                        var options = new RestClient("https://accounts.google.com");
                        var request = new RestRequest("/o/oauth2/token", Method.POST);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Cookie", "__Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; __Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; NID=511=Kbcaybba2NN2meR6QaA1TfflTMD5X6JNa-vmRpwiHUMRZn4ru7MVloJIso4PHGVH40ORQYUKH1LJvUttyG79Vi5eCXIX4UgloYTQFHN0qYHR67sn7fC32LHA7Cfdbgz4G6g5FhOnXus0SVeSyNx4QPRGFQPakVRKwNlBIxK8FGc; __Host-GAPS=1:7IaKbeoNAD6DkBpPrg7Pl6ppMRE7yQ:a2G_qIglM2A-euWd");
                        var body = @"{" + "\n" +
                        @"    ""client_id"": " + "\"" + getgmailcredentials.client_id + "\"" + "," + "\n" +

                        @"    ""client_secret"":  " + "\"" + getgmailcredentials.client_secret + "\"" + "," + "\n" +
                        @"    ""grant_type"": ""refresh_token"",
" + "\n" +
                        @"    ""refresh_token"": " + "\"" + getgmailcredentials.refresh_token + "" + "\"" + "\n" + @"}";

                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        IRestResponse response = options.Execute(request);
                        string errornetsuiteJSON = response.Content;
                        MdlGmailCampaign.Rootobject objMdlGmailCampaignResponse = new MdlGmailCampaign.Rootobject();
                        objMdlGmailCampaignResponse = JsonConvert.DeserializeObject<MdlGmailCampaign.Rootobject>(errornetsuiteJSON);


                        var client = new RestClient("https://gmail.googleapis.com");
                        var request1 = new RestRequest("/gmail/v1/users/me/labels", Method.POST);
                        request1.AddHeader("Authorization", "Bearer " + objMdlGmailCampaignResponse.access_token);
                        request1.AddHeader("Content-Type", "application/json");

                        var body1 = new
                        {
                            name = values.labelName,
                            labelListVisibility = "labelShow",
                            messageListVisibility = "show"
                        };
                        request1.AddJsonBody(body1);

                        IRestResponse response1 = client.Execute(request1);

                        if (response1.StatusCode == HttpStatusCode.OK || response1.StatusCode == HttpStatusCode.Created)
                        {
                            var labelResponse = JsonConvert.DeserializeObject<Label>(response1.Content);

                            if (labelResponse != null && !string.IsNullOrEmpty(labelResponse.Id))
                            {
                                msSQL = "INSERT INTO crm_trn_tgmailllabel (" +
                                        "label_id, " +
                                        "label_name, " +
                                        "label_type, " +
                                        "company_code, " +
                                        "integrated_gmail, " +
                                        "created_by, " +
                                        "created_date) " +
                                        "VALUES (" +
                                        "'" + labelResponse.Id + "', " +
                                        "'" + labelResponse.Name.Replace("'", "''") + "', " +
                                        "'user', " +
                                        "'" + lscompany_code + "', " +
                                        "'" + integrated_gmail + "', " +
                                        "'" + user_gid + "', " +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "Folder Created Successfully!";
                                }
                                else
                                {
                                    values.message = "Error While Creating Folder.";
                                    values.status = false;
                                    objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                }
                            }
                            else
                            {
                                values.message = "Error While Creating Folder on Gmail.";
                                values.status = false;
                                objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }
                        else
                        {
                            values.message = "Error While Creating Folder on Gmail.";
                            values.status = false;
                            objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }

                        //var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                        // Create a new label
                        //var label = new Google.Apis.Gmail.v1.Data.Label
                        //{
                        //    Name = values.labelName,
                        //    LabelListVisibility = "labelShow",
                        //    MessageListVisibility = "show",
                        //    Type = "user"
                        //};

                        //var request = gmailService.Users.Labels.Create(label, "me");
                        //var response = await request.ExecuteAsync();

                        // Check if the response is not null
                        //if (response != null && !string.IsNullOrEmpty(response.Id))
                        //{
                        //    // Insert the new label into the database
                        //    msSQL = "INSERT INTO crm_trn_tgmailllabel (" +
                        //            "label_id, " +
                        //            "label_name, " +
                        //            "label_type, " +
                        //            "company_code, " +
                        //            "created_by, " +
                        //            "created_date) " +
                        //            "VALUES (" +
                        //            "'" + response.Id + "', " +
                        //            "'" + response.Name.Replace("'", "''") + "', " +
                        //            "'" + response.Type + "', " +
                        //            "'" + lscompany_code + "', " +
                        //            "'" + user_gid + "', " +
                        //            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        //    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        //    if (mnResult == 1)
                        //    {
                        //        values.status = true;
                        //        values.message = "Folder Created Successfully!";
                        //    }
                        //    else
                        //    {
                        //        values.message = "Error While Creating Folder.";
                        //        values.status = false;
                        //        // Log the error
                        //        objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        //    }
                        //}
                        //else
                        //{
                        //    values.message = "Error While Creating Folder on Gmail.";
                        //    values.status = false;
                        //    objcmnfunctions.LogForAudit($"Error: Gmail API response is null or invalid", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        //}
                    }
                    else
                    {
                        values.message = "User Don't Map Any Gmail API Account !";
                        values.status = false;
                    }
                }
                else
                {
                    values.message = "Same Folder Name Already exists.";
                    values.status = false;
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Creating Folder.";
                values.status = false;
                objcmnfunctions.LogForAudit($"Error in DaCreateEmailLabel: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw;
            }
        }
        public void DaUpdateEmailLabel(updatelabel_list values, string user_gid)
        {
            try
            {
                // Check if a label with the same name already exists
                string msSQL = "select label_id from crm_trn_tgmailllabel where label_name='" + values.labelNameEdit + "'";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    // Check if the existing label_id is different from the current label_id
                    if (dt_datatable.Rows[0]["label_id"].ToString() != values.label_id)
                    {
                        // If a label with the same name already exists and it's not the same label_id, show an error message
                        values.message = "Same Folder Name Already exists.";
                        values.status = false;
                        return;
                    }
                }

                // Check if the label_id matches the existing label
                msSQL = "select label_id from crm_trn_tgmailllabel where label_id='" + values.label_id + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    gmailconfiguration getgmailcredentials = new gmailconfiguration();


                    //string msSQL1 = "select integrated_gmail from crm_trn_tgmailllabel where label_id ='" + values.label_id + "'";
                    //string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                    string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
                    string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);

                    string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                    if (objOdbcDataReader.HasRows == true)
                    {

                        getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                        getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                        getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                        getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    }
                    objOdbcDataReader.Close();




                    msSQL = "select company_code from adm_mst_tcompany limit 1";
                    string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    string clientId = getgmailcredentials.client_id;
                    string clientSecret = getgmailcredentials.client_secret;
                    string refreshToken = getgmailcredentials.refresh_token;
                    var options = new RestClient("https://accounts.google.com");
                    var request = new RestRequest("/o/oauth2/token", Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Cookie", "__Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; __Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; NID=511=Kbcaybba2NN2meR6QaA1TfflTMD5X6JNa-vmRpwiHUMRZn4ru7MVloJIso4PHGVH40ORQYUKH1LJvUttyG79Vi5eCXIX4UgloYTQFHN0qYHR67sn7fC32LHA7Cfdbgz4G6g5FhOnXus0SVeSyNx4QPRGFQPakVRKwNlBIxK8FGc; __Host-GAPS=1:7IaKbeoNAD6DkBpPrg7Pl6ppMRE7yQ:a2G_qIglM2A-euWd");
                    var body = @"{" + "\n" +
                    @"    ""client_id"": " + "\"" + getgmailcredentials.client_id + "\"" + "," + "\n" +

                    @"    ""client_secret"":  " + "\"" + getgmailcredentials.client_secret + "\"" + "," + "\n" +
                    @"    ""grant_type"": ""refresh_token"",
" + "\n" +
                    @"    ""refresh_token"": " + "\"" + getgmailcredentials.refresh_token + "" + "\"" + "\n" + @"}";

                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = options.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    MdlGmailCampaign.Rootobject objMdlGmailCampaignResponse = new MdlGmailCampaign.Rootobject();
                    objMdlGmailCampaignResponse = JsonConvert.DeserializeObject<MdlGmailCampaign.Rootobject>(errornetsuiteJSON);

                    var client = new RestClient("https://gmail.googleapis.com");
                    var request1 = new RestRequest("/gmail/v1/users/me/labels/" + values.label_id, Method.PATCH);
                    request1.AddHeader("Authorization", "Bearer " + objMdlGmailCampaignResponse.access_token);
                    request1.AddHeader("Content-Type", "application/json");

                    var body1 = new
                    {
                        name = values.labelNameEdit,
                        labelListVisibility = "labelShow",
                        messageListVisibility = "show"
                    };
                    request1.AddJsonBody(body1);

                    IRestResponse response1 = client.Execute(request1);

                    if (response1.StatusCode == HttpStatusCode.OK || response1.StatusCode == HttpStatusCode.Created)
                    {
                        // Update the label name in the table
                        msSQL = "UPDATE crm_trn_tgmailllabel SET label_name='" + values.labelNameEdit.Replace("'", "''") + "' WHERE label_id='" + values.label_id + "'";
                        int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Folder Updated Successfully!";
                        }
                        else
                        {
                            values.message = "Error While Updating Folder.";
                            values.status = false;
                            objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    else
                    {
                        values.message = "Error While Updating Folder on Gmail.";
                        values.status = false;
                        objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                else
                {
                    values.message = "Folder Not Found.";
                    values.status = false;
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Updating Folder.";
                values.status = false;
                objcmnfunctions.LogForAudit($"Error in DaUpdateEmailLabel: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw;
            }
        }
        public async Task DaDeleteLabelOrFolder(updatelabel_list values, string label_id)
        {
            try
            {
                if (objdbconn == null)
                {
                    values.status = false;
                    values.message = "Database connection is null.";
                    return;
                }

                gmailconfiguration getgmailcredentials = new gmailconfiguration();

                string msSQL1 = "select integrated_gmail from crm_trn_tgmailllabel where label_id ='" + label_id + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                }
                objOdbcDataReader.Close();

                msSQL = "select company_code from adm_mst_tcompany limit 1";
                string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;

                // Get the Gmail service instance
                var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);
                if (gmailService == null)
                {
                    values.status = false;
                    values.message = "Gmail service is null.";
                    return;
                }

                // Check if the folder has any mails before deleting
                var listRequest = gmailService.Users.Messages.List("me");
                listRequest.LabelIds = new[] { label_id };
                var listResponse = await listRequest.ExecuteAsync();


                if (listResponse.Messages != null)
                {
                    values.message = "Folder is not empty. Cannot delete.";
                    values.status = false;
                    return;
                }

                // Delete the label or folder
                var deleteRequest = gmailService.Users.Labels.Delete("me", label_id);
                if (deleteRequest == null)
                {
                    values.status = false;
                    values.message = "Delete request is null.";
                    return;
                }

                await deleteRequest.ExecuteAsync();

                msSQL = "delete from crm_trn_tgmailllabel where label_id='" + label_id + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.message = "Folder Deleted Successfully !!";
                    values.status = true;
                }
                else
                {
                    values.message = "Error while deleting folder from the database.";
                    values.status = false;
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error while deleting label or folder.";
                objcmnfunctions.LogForAudit($"Error in DaDeleteLabelOrFolder: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw;
            }
        }
        public async Task DaMovetoFolder(gmailfolderlist values)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();

                foreach (var emailId in values.gmailmovelist)
                {
                    string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + emailId.inbox_id + "'";
                    string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                    string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                    if (objOdbcDataReader.HasRows == true)
                    {

                        getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                        getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                        getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                        getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    }
                    objOdbcDataReader.Close();

                    string clientId = getgmailcredentials.client_id;
                    string clientSecret = getgmailcredentials.client_secret;
                    string refreshToken = getgmailcredentials.refresh_token;

                    // Get the Gmail service instance
                    var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);
                    try
                    {
                        // Modify the message's labels
                        var modifyRequest = new Google.Apis.Gmail.v1.Data.ModifyMessageRequest
                        {
                            AddLabelIds = new List<string> { values.label_id }, // Add the target label ID
                            RemoveLabelIds = new List<string> { "INBOX" } // Optionally remove the old label, e.g., "INBOX"
                        };

                        var modifyResponse = await gmailService.Users.Messages.Modify(modifyRequest, "me", emailId.inbox_id).ExecuteAsync();

                        // Check if the message was successfully moved to the specified label
                        if (modifyResponse.LabelIds.Contains(values.label_id))
                        {
                            msSQL = "UPDATE crm_trn_tgmailinbox SET " +
                                    "inbox_status = '" + values.label_id + "' " +
                                    "WHERE inbox_id = '" + emailId.inbox_id + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                values.message = $"Email moved to folder successfully.";
                                values.status = true;
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error occurred while updating database.";
                            }
                        }
                        else
                        {
                            values.message = $"Error: Failed to move email {emailId.inbox_id} to folder.";
                            values.status = false;
                            objcmnfunctions.LogForAudit($"Error: Failed to move email {emailId.inbox_id} to folder.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    catch (Exception ex)
                    {
                        values.message = $"Error: Exception occurred while moving email {emailId.inbox_id} to folder.";
                        values.status = false;
                        objcmnfunctions.LogForAudit($"Error moving email {emailId.inbox_id} to folder: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error while moving email to folder.";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaMovetoFolder: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task DaMoveToFolderFromTrash(gmailfolderlist values)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();


                foreach (var emailId in values.gmailmovelist)
                {
                    try
                    {
                        string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + emailId.inbox_id + "'";
                        string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                        string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                        if (objOdbcDataReader.HasRows == true)
                        {

                            getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                            getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                            getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                            getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                        }
                        objOdbcDataReader.Close();
                        string clientId = getgmailcredentials.client_id;
                        string clientSecret = getgmailcredentials.client_secret;
                        string refreshToken = getgmailcredentials.refresh_token;
                        var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);
                        // Modify the message's labels to remove "TRASH" and add the target label
                        var modifyRequest = new Google.Apis.Gmail.v1.Data.ModifyMessageRequest
                        {
                            AddLabelIds = new List<string> { values.label_id }, // Add the target label ID
                            RemoveLabelIds = new List<string> { "TRASH" } // Remove the "TRASH" label
                        };

                        var modifyResponse = await gmailService.Users.Messages.Modify(modifyRequest, "me", emailId.inbox_id).ExecuteAsync();

                        // Check if the message was successfully moved to the specified label
                        if (modifyResponse.LabelIds.Contains(values.label_id) && !modifyResponse.LabelIds.Contains("TRASH"))
                        {
                            msSQL = "UPDATE crm_trn_tgmailinbox SET " +
                                    "inbox_status = '" + values.label_id + "' " +
                                    "WHERE inbox_id = '" + emailId.inbox_id + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                values.message = $"Email moved to folder successfully.";
                                values.status = true;
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error occurred while updating database.";
                            }
                        }
                        else
                        {
                            values.message = $"Error: Failed to move email {emailId.inbox_id} to folder.";
                            values.status = false;
                            objcmnfunctions.LogForAudit($"Error: Failed to move email {emailId.inbox_id} to folder.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    catch (Exception ex)
                    {
                        values.message = $"Error: Exception occurred while moving email {emailId.inbox_id} to folder.";
                        values.status = false;
                        objcmnfunctions.LogForAudit($"Error moving email {emailId.inbox_id} to folder: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error while moving email to folder.";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaMoveToFolderFromTrash: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }

        public async Task DaMoveoneFoldertoother(gmailoneFoldertoother_list values)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();

                // Retrieve Gmail credentials from your database


                foreach (var emailId in values.gmailmovelist)
                {
                    string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + emailId.inbox_id + "'";
                    string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                    string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                    if (objOdbcDataReader.HasRows == true)
                    {

                        getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                        getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                        getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                        getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    }
                    objOdbcDataReader.Close();

                    string clientId = getgmailcredentials.client_id;
                    string clientSecret = getgmailcredentials.client_secret;
                    string refreshToken = getgmailcredentials.refresh_token;

                    // Get the Gmail service instance
                    var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);
                    try
                    {
                        msSQL = "select inbox_status from  crm_trn_tgmailinbox where inbox_id='" + emailId.inbox_id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader.HasRows == true)
                        {

                            current_label_id = objOdbcDataReader["inbox_status"].ToString();

                        }
                        objOdbcDataReader.Close();
                        // Modify the message's labels
                        var modifyRequest = new Google.Apis.Gmail.v1.Data.ModifyMessageRequest
                        {
                            AddLabelIds = new List<string> { values.label_id }, // Add the target label ID
                            RemoveLabelIds = new List<string> { current_label_id } // Remove the current label ID
                        };

                        var modifyResponse = await gmailService.Users.Messages.Modify(modifyRequest, "me", emailId.inbox_id).ExecuteAsync();

                        // Check if the message was successfully moved to the specified label
                        if (modifyResponse.LabelIds.Contains(values.label_id))
                        {
                            // Update your database to reflect the new label
                            msSQL = "UPDATE crm_trn_tgmailinbox SET " +
                                    "inbox_status = '" + values.label_id + "' " +
                                    "WHERE inbox_id = '" + emailId.inbox_id + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                values.message = $"Email moved to folder successfully.";
                                values.status = true;
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error occurred while updating database.";
                            }
                        }
                        else
                        {
                            values.message = $"Error: Failed to move email {emailId.inbox_id} to folder.";
                            values.status = false;
                            objcmnfunctions.LogForAudit($"Error: Failed to move email {emailId.inbox_id} to folder.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    catch (Exception ex)
                    {
                        values.message = $"Error: Exception occurred while moving email {emailId.inbox_id} to folder.";
                        values.status = false;
                        objcmnfunctions.LogForAudit($"Error moving email {emailId.inbox_id} to folder: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error while moving email to folder.";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaMovetoFolder: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task DaTrashDeleteMail(gmailmovedlist values)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();

                // Retrieve Gmail credentials


                foreach (var emailId in values.gmailmovelist)
                {
                    try
                    {
                        string s_no = string.Empty;
                        string inbox_id = string.Empty;
                        string from_id = string.Empty;
                        string cc = string.Empty;
                        string sent_date = string.Empty;
                        string bcc = string.Empty;
                        string subject = string.Empty;
                        string body = string.Empty;
                        string attachement_flag = string.Empty;
                        string inbox_status = string.Empty;
                        string shopify_enquiry = string.Empty;
                        string leadbank_gid = string.Empty;
                        string read_flag = string.Empty;
                        string integratedgmail = string.Empty;
                        string company_code = string.Empty;
                        string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + emailId.inbox_id + "'";
                        string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                        string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                        if (objOdbcDataReader.HasRows == true)
                        {

                            getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                            getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                            getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                            getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                        }
                        objOdbcDataReader.Close();

                        string clientId = getgmailcredentials.client_id;
                        string clientSecret = getgmailcredentials.client_secret;
                        string refreshToken = getgmailcredentials.refresh_token;
                        string msSQL3 = "select * from  crm_trn_tgmailinbox  where inbox_id= '" + emailId.inbox_id + "' limit 1";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL3);

                        if (objOdbcDataReader.HasRows == true)
                        {
                            s_no = objOdbcDataReader["s_no"].ToString();
                            inbox_id = objOdbcDataReader["inbox_id"].ToString();
                            from_id = objOdbcDataReader["from_id"].ToString();
                            cc = objOdbcDataReader["cc"].ToString();
                            sent_date = objOdbcDataReader["sent_date"].ToString();
                            bcc = objOdbcDataReader["bcc"].ToString();
                            subject = objOdbcDataReader["subject"].ToString();
                            body = objOdbcDataReader["body"].ToString();
                            attachement_flag = objOdbcDataReader["attachement_flag"].ToString();
                            inbox_status = objOdbcDataReader["inbox_status"].ToString();
                            shopify_enquiry = objOdbcDataReader["shopify_enquiry"].ToString();
                            leadbank_gid = objOdbcDataReader["leadbank_gid"].ToString();
                            read_flag = objOdbcDataReader["read_flag"].ToString();
                            integratedgmail = objOdbcDataReader["integrated_gmail"].ToString();
                            company_code = objOdbcDataReader["company_code"].ToString();

                        }
                        objOdbcDataReader.Close();
                        string insertSQL = "INSERT INTO crm_trn_tgmailinboxdeletelog (" +
                             "inbox_id, from_id, cc, sent_date, bcc, subject, body, attachement_flag, inbox_status, shopify_enquiry, leadbank_gid, read_flag, integrated_gmail, company_code) " +
                             "VALUES (" +
                             "'" + inbox_id + "', " +
                             "'" + from_id + "', " +
                             "'" + cc + "', " +
                             "'" + sent_date + "', " +
                             "'" + bcc + "', " +
                             "'" + subject + "', " +
                             "'" + body + "', " +
                             "'" + attachement_flag + "', " +
                             "'" + inbox_status + "', " +
                             "'" + shopify_enquiry + "', " +
                             "'" + leadbank_gid + "', " +
                             "'" + read_flag + "', " +
                             "'" + integratedgmail + "', " +
                             "'" + company_code + "')";

                        int mnResult = objdbconn.ExecuteNonQuerySQL(insertSQL);

                        // Get the Gmail service instance
                        // var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);
                        // Delete the email
                        // await gmailService.Users.Messages.Delete("me", emailId.inbox_id).ExecuteAsync();

                        // If deletion is successful, delete from the database
                        if (mnResult == 1)
                        {
                            msSQL = "DELETE FROM crm_trn_tgmailinbox WHERE inbox_id = '" + emailId.inbox_id + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                values.message = $"Email deleted successfully.";
                                values.status = true;
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error occurred while updating database.";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error occurred while updating database.";
                        }
                    }
                    catch (Exception ex)
                    {
                        values.message = $"Error: Exception occurred while deleting email {emailId.inbox_id}.";
                        values.status = false;
                        objcmnfunctions.LogForAudit($"Error deleting email {emailId.inbox_id}: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error while deleting email.";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaDeleteMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }


        public async Task DaForwardOfFwdMail(forwardoffwdmail_list values)
        {
            try
            {

                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                //string clientId = ConfigurationManager.AppSettings["clientId"];
                //string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
                //string refreshToken = ConfigurationManager.AppSettings["refreshToken"];
                string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + values.inbox_id + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                }
                objOdbcDataReader.Close();
                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;
                if (values.reply_id == "No")
                {
                    // Get the Gmail service instance
                    var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                    // Fetch the original message (you need to determine how to fetch messageId)
                    var originalMessage = await gmailService.Users.Messages.Get("me", values.forward_id).ExecuteAsync();

                    // Extract relevant information from the original message
                    string originalSubject = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                    string originalFrom = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                    string originalTo = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value;
                    string originalCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;
                    string originalBCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "BCC")?.Value;
                    string originalDateStr = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;
                    msSQL = "select body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date from  crm_trn_tgmailinboxreplyforward where forward_id='" + values.forward_id + "' limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsbody = DecodeBase64(objOdbcDataReader["body"].ToString());
                        lssent_date = objOdbcDataReader["sent_date"].ToString();

                    }
                    objOdbcDataReader.Close();

                    // Ensure emailBody and original_body retain their HTML formatting
                    string emailBody = values.emailBody; // Assuming values.emailBody is already in HTML format
                    string original_body = lsbody; // Assuming values.orginal_body is already in HTML format
                    string sent_date = lssent_date;

                    // Compose the forwarded message body
                    string forwardBody = $"<br>{emailBody}<br><br> From: {originalFrom}<br>Date: {sent_date}<br>Subject: {originalSubject}<br><br>{original_body}";

                    // Create MIME message manually
                    string boundary = "----=_Part_0_" + Guid.NewGuid().ToString("N");

                    var mimeMessage = new StringBuilder();
                    string[] recipients = values.forwardto.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(r => r.Trim())
                                      .ToArray();

                    // Append each recipient to the mimeMessage
                    foreach (string recipient in recipients)
                    {
                        mimeMessage.AppendLine($"To: {recipient}");
                    }
                    //mimeMessage.AppendLine($"To: {values.forwardto}"); // Forward to the original recipient
                    mimeMessage.AppendLine($"Subject: Fwd: {originalSubject}");
                    // if (!string.IsNullOrEmpty(originalCC)) mimeMessage.AppendLine($"Cc: {originalCC}");
                    // if (!string.IsNullOrEmpty(originalBCC)) mimeMessage.AppendLine($"Bcc: {originalBCC}");
                    mimeMessage.AppendLine("Content-Type: multipart/alternative; boundary=\"" + boundary + "\"");
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine("--" + boundary);
                    mimeMessage.AppendLine("Content-Type: text/html; charset=UTF-8");
                    mimeMessage.AppendLine("Content-Transfer-Encoding: 7bit");
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine(forwardBody);
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine("--" + boundary + "--");

                    // Encode the message to base64url
                    var encodedRawMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage.ToString()))
                                                    .Replace('+', '-')
                                                    .Replace('/', '_')
                                                    .Replace("=", "");

                    // Create the forwarded message
                    var forwardedMessage = new Google.Apis.Gmail.v1.Data.Message
                    {
                        Raw = encodedRawMessageContent
                    };

                    // Send the forwarded message using Gmail API
                    var sendMessageRequest = gmailService.Users.Messages.Send(forwardedMessage, "me");
                    var sendMessageResponse = await sendMessageRequest.ExecuteAsync();
                    string sentMessageId = sendMessageResponse.Id;

                    // Check if message was sent successfully
                    if (sendMessageResponse != null && sendMessageResponse.LabelIds.Contains("SENT"))
                    {
                        string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // Insert into crm_trn_tgmailinboxreply
                        string msSQL = "INSERT INTO crm_trn_tgmailinboxreplyforward (" +
                                        "inbox_id, " +
                                        "reply_id, " +
                                        "forward_id, " +
                                        "fwd_id, " +
                                        "to_id, " +
                                        "subject, " +
                                        "both_body, " +
                                        //"cc, " +
                                        //"bcc, " +
                                        "body, " +
                                        "attachement_flag, " +
                                        "sent_date) " +
                                        "VALUES (" +
                                        "'" + values.inbox_id + "', " +
                                       "'" + values.reply_id + "', " +
                                        "'" + sentMessageId + "', " +
                                         "'" + values.forward_id + "', " +
                                        "'" + values.forwardto + "', " +
                                        "'" + EncodeToBase64($"Fwd: {originalSubject}") + "', " +
                                        "'" + encodedRawMessageContent + "', " +
                                        //"'" + originalCC + "', " +
                                        //"'" + originalBCC + "', " +
                                        "'" + EncodeToBase64(emailBody) + "', " +
                                        "'" + values.attachement_flag + "', " +
                                        "'" + formattedDateString + "')";

                        int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Forward Mail Sent Successfully!";
                        }
                        else
                        {
                            values.message = "Error: Failed to insert forward mail into database.";
                            values.status = false;
                            // Log the error
                            objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    else
                    {
                        // If sending failed
                        values.message = "Error: Failed to send forward mail.";
                        values.status = false;
                        objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                else
                {
                    // Get the Gmail service instance
                    var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                    // Fetch the original message (you need to determine how to fetch messageId)
                    var originalMessage = await gmailService.Users.Messages.Get("me", values.forward_id).ExecuteAsync();

                    // Extract relevant information from the original message
                    string originalSubject = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                    string originalFrom = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                    string originalTo = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value;
                    string originalCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;
                    string originalBCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "BCC")?.Value;
                    string originalDateStr = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;
                    msSQL = "select body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date from  crm_trn_tgmailinboxreplyforward where forward_id='" + values.forward_id + "' limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsbody = DecodeBase64(objOdbcDataReader["body"].ToString());
                        lssent_date = objOdbcDataReader["sent_date"].ToString();

                    }
                    objOdbcDataReader.Close();

                    // Ensure emailBody and original_body retain their HTML formatting
                    string emailBody = values.emailBody; // Assuming values.emailBody is already in HTML format
                    string original_body = lsbody; // Assuming values.orginal_body is already in HTML format
                    string sent_date = lssent_date;

                    // Compose the forwarded message body
                    string forwardBody = $"<br>{emailBody}<br><br> From: {originalFrom}<br>Date: {sent_date}<br>Subject: {originalSubject}<br><br>{original_body}";

                    // Create MIME message manually
                    string boundary = "----=_Part_0_" + Guid.NewGuid().ToString("N");

                    var mimeMessage = new StringBuilder();
                    string[] recipients = values.forwardto.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(r => r.Trim())
                                      .ToArray();

                    // Append each recipient to the mimeMessage
                    foreach (string recipient in recipients)
                    {
                        mimeMessage.AppendLine($"To: {recipient}");
                    }
                    // mimeMessage.AppendLine($"To: {values.forwardto}"); // Forward to the original recipient
                    mimeMessage.AppendLine($"Subject: Fwd: {originalSubject}");
                    // if (!string.IsNullOrEmpty(originalCC)) mimeMessage.AppendLine($"Cc: {originalCC}");
                    // if (!string.IsNullOrEmpty(originalBCC)) mimeMessage.AppendLine($"Bcc: {originalBCC}");
                    mimeMessage.AppendLine("Content-Type: multipart/alternative; boundary=\"" + boundary + "\"");
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine("--" + boundary);
                    mimeMessage.AppendLine("Content-Type: text/html; charset=UTF-8");
                    mimeMessage.AppendLine("Content-Transfer-Encoding: 7bit");
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine(forwardBody);
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine("--" + boundary + "--");

                    // Encode the message to base64url
                    var encodedRawMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage.ToString()))
                                                    .Replace('+', '-')
                                                    .Replace('/', '_')
                                                    .Replace("=", "");

                    // Create the forwarded message
                    var forwardedMessage = new Google.Apis.Gmail.v1.Data.Message
                    {
                        Raw = encodedRawMessageContent
                    };

                    // Send the forwarded message using Gmail API
                    var sendMessageRequest = gmailService.Users.Messages.Send(forwardedMessage, "me");
                    var sendMessageResponse = await sendMessageRequest.ExecuteAsync();
                    string sentMessageId = sendMessageResponse.Id;

                    // Check if message was sent successfully
                    if (sendMessageResponse != null && sendMessageResponse.LabelIds.Contains("SENT"))
                    {
                        string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // Insert into crm_trn_tgmailinboxreply
                        string msSQL = "INSERT INTO crm_trn_tgmailinboxreplyforward (" +
                                        "inbox_id, " +
                                        "reply_id, " +
                                        "forward_id, " +
                                        "to_id, " +
                                        "subject, " +
                                        "both_body, " +
                                        //"cc, " +
                                        //"bcc, " +
                                        "body, " +
                                        "attachement_flag, " +
                                        "sent_date) " +
                                        "VALUES (" +
                                        "'" + values.inbox_id + "', " +
                                       "'" + values.reply_id + "', " +
                                        "'" + sentMessageId + "', " +
                                        "'" + values.forwardto + "', " +
                                        "'" + EncodeToBase64($"Fwd: {originalSubject}") + "', " +
                                        "'" + encodedRawMessageContent + "', " +
                                        //"'" + originalCC + "', " +
                                        //"'" + originalBCC + "', " +
                                        "'" + EncodeToBase64(emailBody) + "', " +
                                        "'N', " +
                                        "'" + formattedDateString + "')";

                        int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Forward Mail Sent Successfully!";
                        }
                        else
                        {
                            values.message = "Error: Failed to insert forward mail into database.";
                            values.status = false;
                            // Log the error
                            objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    else
                    {
                        // If sending failed
                        values.message = "Error: Failed to send forward mail.";
                        values.status = false;
                        objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error While Forward Mail !!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaReplyOrForwardInboxMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }
        public async Task DaForwardOfFwdMailWithAttach(forwardoffwdmail_list values)
        {
            try
            {

                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                //string clientId = ConfigurationManager.AppSettings["clientId"];
                //string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
                //string refreshToken = ConfigurationManager.AppSettings["refreshToken"];
                string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + values.inbox_id + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                }
                objOdbcDataReader.Close();

                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;
                var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                // Fetch the original message
                var originalMessage = await gmailService.Users.Messages.Get("me", values.forward_id).ExecuteAsync();

                // Extract relevant information from the original message
                string originalSubject = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                string originalFrom = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                string originalTo = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value;
                string originalCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;
                string originalBCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "BCC")?.Value;

                string msSQL = values.reply_id == "No"
                    ? $"SELECT body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date FROM crm_trn_tgmailinboxreplyforward WHERE forward_id='{values.forward_id}' LIMIT 1"
                    : $"SELECT body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date FROM crm_trn_tgmailinboxreplyforward WHERE forward_id='{values.forward_id}' LIMIT 1";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsbody = DecodeBase64(objOdbcDataReader["body"].ToString());
                    lssent_date = objOdbcDataReader["sent_date"].ToString();
                }
                objOdbcDataReader.Close();

                string emailBody = values.emailBody; // Assuming values.emailBody is already in HTML format
                string original_body = lsbody; // Assuming original_body is already in HTML format
                string sent_date = lssent_date;

                // Compose the forwarded message body
                string forwardBody = $"<br>{emailBody}<br><br> From: {originalFrom}<br>Date: {sent_date}<br>Subject: {originalSubject}<br><br>{original_body}";

                // Create MIME message manually with attachments
                string boundary = "----=_Part_0_" + Guid.NewGuid().ToString("N");

                var mimeMessage = new StringBuilder();
                string[] recipients = values.forwardto.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(r => r.Trim())
                                      .ToArray();

                // Append each recipient to the mimeMessage
                foreach (string recipient in recipients)
                {
                    mimeMessage.AppendLine($"To: {recipient}");
                }
                // mimeMessage.AppendLine($"To: {values.forwardto}");
                mimeMessage.AppendLine($"Subject: Fwd: {originalSubject}");
                // if (!string.IsNullOrEmpty(originalCC)) mimeMessage.AppendLine($"Cc: {originalCC}");
                //if (!string.IsNullOrEmpty(originalBCC)) mimeMessage.AppendLine($"Bcc: {originalBCC}");
                mimeMessage.AppendLine($"Content-Type: multipart/mixed; boundary=\"{boundary}\"");
                mimeMessage.AppendLine();
                mimeMessage.AppendLine($"--{boundary}");
                mimeMessage.AppendLine("Content-Type: text/html; charset=UTF-8");
                mimeMessage.AppendLine("Content-Transfer-Encoding: 7bit");
                mimeMessage.AppendLine();
                mimeMessage.AppendLine(forwardBody);
                mimeMessage.AppendLine();
                if (values.reply_id == "No")
                {
                    msSQL = "SELECT s_no, inbox_id, original_filename, modified_filename, file_path FROM crm_trn_tgmailinboxattachment where inbox_id='" + values.inbox_id + "'  ORDER BY  s_no asc";
                }
                else
                {
                    msSQL = "SELECT s_no, inbox_id, original_filename, modified_filename, file_path FROM crm_trn_tgmailreplyattachment where reply_id='" + values.reply_id + "'  ORDER BY  s_no asc";

                }
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {


                        string filepath = ConfigurationManager.AppSettings["fwdfilepath"] + dt["file_path"].ToString();
                        var contentType = MimeMapping.GetMimeMapping(filepath);
                        string attachmentData = Convert.ToBase64String(File.ReadAllBytes(filepath));
                        mimeMessage.AppendLine($"--{boundary}");
                        mimeMessage.AppendLine($"Content-Type: {contentType}; name=\"{dt["original_filename"].ToString()}\"");
                        mimeMessage.AppendLine("Content-Transfer-Encoding: base64");
                        mimeMessage.AppendLine("Content-Disposition: attachment");
                        mimeMessage.AppendLine();
                        mimeMessage.AppendLine(attachmentData);
                        mimeMessage.AppendLine();

                    }
                }


                mimeMessage.AppendLine($"--{boundary}--");

                // Encode the message to base64url
                var encodedRawMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage.ToString()))
                                                .Replace('+', '-')
                                                .Replace('/', '_')
                                                .Replace("=", "");

                // Create the forwarded message
                var forwardedMessage = new Google.Apis.Gmail.v1.Data.Message
                {
                    Raw = encodedRawMessageContent
                };

                // Send the forwarded message using Gmail API
                var sendMessageRequest = gmailService.Users.Messages.Send(forwardedMessage, "me");
                var sendMessageResponse = await sendMessageRequest.ExecuteAsync();
                string sentMessageId = sendMessageResponse.Id;

                // Check if message was sent successfully
                if (sendMessageResponse != null && sendMessageResponse.LabelIds.Contains("SENT"))
                {
                    string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Insert into crm_trn_tgmailinboxreplyforward
                    msSQL = "INSERT INTO crm_trn_tgmailinboxreplyforward (" +
                            "inbox_id, " +
                            "reply_id, " +
                            "forward_id, " +
                            "fwd_id, " +
                            "to_id, " +
                            "subject, " +
                            "both_body, " +
                            //"cc, " +
                            //"bcc, " +
                            "body, " +
                            "attachement_flag, " +
                            "sent_date) " +
                            "VALUES (" +
                            $"'{values.inbox_id}', " +
                            $"'{values.reply_id}', " +
                            $"'{sentMessageId}', " +
                            $"'{values.forward_id}', " +
                            $"'{values.forwardto}', " +
                            $"'{EncodeToBase64($"Fwd: {originalSubject}")}', " +
                            $"'{encodedRawMessageContent}', " +
                            //$"'{originalCC}', " +
                            //$"'{originalBCC}', " +
                            $"'{EncodeToBase64(emailBody)}', " +
                            $"'{values.attachement_flag}', " +
                            $"'{formattedDateString}')";

                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {

                        values.status = true;
                        values.message = "Forward Mail Sent Successfully!";
                    }
                    else
                    {
                        values.message = "Error: Failed to insert forward mail into database.";
                        values.status = false;
                        // Log the error
                        objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                else
                {
                    // If sending failed
                    values.message = "Error: Failed to send forward mail.";
                    values.status = false;
                    objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error While Forwarding Mail !!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaReplyOrForwardInboxMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }
        public static string StripHtmlTags(string html)
        {
            // Regular expression to strip HTML tags
            string text = Regex.Replace(html, "<.*?>", string.Empty);
            return text;
        }
        // Initialize a counter for generating unique IDs
        int uniqueIdCounter = 1;

        public async Task DaReplyAllWithAttachment(HttpRequest httpRequest, results values, string user_gid)
        {
            try
            {

                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                //string clientId = ConfigurationManager.AppSettings["clientId"];
                //string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
                //string refreshToken = ConfigurationManager.AppSettings["refreshToken"];
                string inboxId = httpRequest.Form["inbox_id"];
                string email_Body = httpRequest.Form["emailBody"];
                string replytoid = httpRequest.Form["replytoid"];
                string replyccid = httpRequest.Form["replyccid"];
                string replybccid = httpRequest.Form["replybccid"];
                msSQL = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + inboxId + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                }
                objOdbcDataReader.Close();
                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;

                // Initialize Gmail API service
                var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

                // Extract form data from the request

                // string originalBody = httpRequest.Form["original_body"];

                // Handle attachments
                var attachments = httpRequest.Files;
                List<string> savedFileNames = new List<string>();
                List<string> savedFileNamesdb = new List<string>(); // Track renamed file paths

                foreach (string fileName in attachments.AllKeys)
                {
                    var file = attachments[fileName];
                    if (file != null && file.ContentLength > 0)
                    {
                        // Generate a unique file name
                        string uniqueFileName = objcmnfunctions.GetMasterGID("UPLF");
                        string fileExtension = Path.GetExtension(file.FileName);
                        string logFilePath = ConfigurationManager.AppSettings["filepath"];

                        // Check if the directory exists and create it if it does not
                        if (!Directory.Exists(logFilePath))
                        {
                            Directory.CreateDirectory(logFilePath);
                        }
                        // Save the file to specified directory with the unique name
                        string saveFilePath = ConfigurationManager.AppSettings["filepath"];
                        string savePath = Path.Combine(saveFilePath, $"{uniqueFileName}{fileExtension}");
                        file.SaveAs(savePath);

                        savedFileNames.Add(savePath); // Add path with original file name
                        savedFileNamesdb.Add(ConfigurationManager.AppSettings["db_path"] + uniqueFileName + fileExtension); // Add path with renamed file name
                    }
                }

                // Fetch the original message
                var originalMessage = await gmailService.Users.Messages.Get("me", inboxId).ExecuteAsync();

                // Extract relevant information from the original message
                string originalSubject = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                string originalFrom = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                string originalTo = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value;
                string originalCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;
                string originalBCC = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "BCC")?.Value;
                string originalDateStr = originalMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;

                msSQL = "select body, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,cc,bcc from  crm_trn_tgmailinbox where inbox_id='" + inboxId + "' limit 1 ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsbody = DecodeBase64(objOdbcDataReader["body"].ToString());
                    lssent_date = objOdbcDataReader["sent_date"].ToString();
                    lscc = objOdbcDataReader["cc"].ToString();
                    lsbcc = objOdbcDataReader["bcc"].ToString();

                }
                objOdbcDataReader.Close();
                string originalBody = lsbody;
                string sent_date = lssent_date;
                // Construct reply body
                string replyBody = $"{email_Body}<br><br> On {sent_date}, {replytoid} wrote:<br><br>{originalBody}";
                string subject = $"Re: {originalSubject}";

                // Create MIME message manually
                string boundary = "----=_Part_0_" + Guid.NewGuid().ToString("N");

                var mimeMessage = new StringBuilder();
                mimeMessage.AppendLine($"To: {replytoid}");
                mimeMessage.AppendLine($"Subject: {subject}");
                if (!string.IsNullOrEmpty(replyccid)) mimeMessage.AppendLine($"Cc: {replyccid}");
                if (!string.IsNullOrEmpty(replybccid)) mimeMessage.AppendLine($"Bcc: {replybccid}");
                mimeMessage.AppendLine("Content-Type: multipart/mixed; boundary=\"" + boundary + "\"");
                mimeMessage.AppendLine();
                mimeMessage.AppendLine("--" + boundary);
                mimeMessage.AppendLine("Content-Type: text/html; charset=UTF-8");
                mimeMessage.AppendLine("Content-Transfer-Encoding: 7bit");
                mimeMessage.AppendLine();
                mimeMessage.AppendLine(replyBody);
                mimeMessage.AppendLine();

                foreach (var filePath in savedFileNames)
                {
                    var fileBytes = File.ReadAllBytes(filePath);
                    var fileName = Path.GetFileName(filePath);
                    var contentType = MimeMapping.GetMimeMapping(filePath);

                    mimeMessage.AppendLine("--" + boundary);
                    mimeMessage.AppendLine($"Content-Type: {contentType}; name=\"{fileName}\"");
                    mimeMessage.AppendLine("Content-Transfer-Encoding: base64");
                    mimeMessage.AppendLine($"Content-Disposition: attachment; filename=\"{fileName}\"");
                    mimeMessage.AppendLine();
                    mimeMessage.AppendLine(Convert.ToBase64String(fileBytes, Base64FormattingOptions.InsertLineBreaks));
                    mimeMessage.AppendLine();
                }

                mimeMessage.AppendLine("--" + boundary + "--");

                // Encode the message to base64url
                var encodedRawMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage.ToString()))
                                                 .Replace('+', '-')
                                                 .Replace('/', '_')
                                                 .Replace("=", "");

                // Create the reply message
                var replyMessage = new Google.Apis.Gmail.v1.Data.Message
                {
                    Raw = encodedRawMessageContent
                };

                // Send the reply message using Gmail API
                var sendMessageRequest = gmailService.Users.Messages.Send(replyMessage, "me");
                var sendMessageResponse = await sendMessageRequest.ExecuteAsync();

                // Check if message was sent successfully
                if (sendMessageResponse != null && sendMessageResponse.LabelIds.Contains("SENT"))
                {

                    string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Insert into crm_trn_tgmailinboxreply
                    msSQL = "INSERT INTO crm_trn_tgmailinboxreply (" +
                            "inbox_id, " +
                            "reply_id, " +
                            "from_id, " +
                            "subject, " +
                            "both_body, " +
                            "cc, " +
                            "bcc, " +
                            "body, " +
                            "attachement_flag, " +
                            "sent_date) " +
                           $"VALUES ('{inboxId}', '{sendMessageResponse.Id}', '{replytoid}', " +
                           $"'{EncodeToBase64(subject)}', '{encodedRawMessageContent}', '{replyccid}', '{replybccid}', " +
                           $"'{EncodeToBase64(email_Body)}', 'Y', '{formattedDateString}')";

                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        // Insert into crm_trn_tgmailreplyattachment
                        foreach (var filePath in savedFileNamesdb)
                        {
                            string originalFileName = Path.GetFileName(filePath);
                            string modifiedFileName = originalFileName; // Adjust if needed
                            string insertQuery = "INSERT INTO crm_trn_tgmailreplyattachment (reply_id, inbox_id, original_filename, modified_filename, file_path) " +
                                                 $"VALUES ('{sendMessageResponse.Id}', '{inboxId}', '{originalFileName}', '{modifiedFileName}', '{filePath}')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(insertQuery);

                            if (mnResult != 1)
                            {
                                values.message = "Error: Failed to insert attachment into database.";
                                values.status = false;
                                // Log the error
                                objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {insertQuery} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }

                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Reply Mail Sent Successfully!";
                        }
                        else
                        {
                            values.message = "Error: Failed to insert attachment into database.";
                            values.status = false;
                        }
                    }
                    else
                    {
                        values.message = "Error: Failed to insert reply mail into database.";
                        values.status = false;
                        // Log the error
                        objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }


                }
                else
                {
                    values.message = "Error: Failed to send reply mail.";
                    values.status = false;
                }
            }
            catch (Exception ex)
            {
                values.message = "Error: Failed to send reply mail.";
                values.status = false;
                objcmnfunctions.LogForAudit(
                                "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        // Example method to execute a non-query SQL command
        private void ExecuteNonQuery(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private string ExtractTextFromHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }

            // Remove HTML tags using Regex
            string htmlContent = Regex.Replace(html, "<.*?>", string.Empty);

            // Decode any encoded HTML entities
            htmlContent = WebUtility.HtmlDecode(htmlContent);

            // Optionally trim any leading or trailing whitespace
            htmlContent = htmlContent.Trim();

            return htmlContent;
        }

        // Helper function to extract the message body format (you may need to adjust it according to your needs)
        private string GetMessageBodyFormat(MessagePart payload)
        {
            // Extract the body content (text or HTML) from the message part
            var body = payload.Body.Data;
            var decodedBody = string.Empty;

            if (!string.IsNullOrEmpty(body))
            {
                var base64EncodedBytes = Convert.FromBase64String(body.Replace("-", "+").Replace("_", "/"));
                decodedBody = Encoding.UTF8.GetString(base64EncodedBytes);
            }

            return decodedBody;
        }
        public void DaGetReplyMail(string inbox_id, MdlGmailCampaign values)
        {
            try
            {
                string msSQL = "SELECT r.s_no, r.reply_id, r.inbox_id, r.from_id, r.cc, DATE_FORMAT(r.sent_date, '%b %e, %Y %h:%i %p') AS sent_date, DATE_FORMAT(r.sent_date, '%h:%i %p') AS sent_time, r.bcc, r.subject, r.body, r.attachement_flag, r.both_body, a.original_filename, a.modified_filename, a.file_path " +
                               "FROM crm_trn_tgmailinboxreply r " +
                               "LEFT JOIN crm_trn_tgmailreplyattachment a ON r.reply_id = a.reply_id " +
                               "WHERE r.inbox_id = '" + inbox_id + "'  order by r.sent_date asc ";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                List<gmailapireply_list> getmodulelist = new List<gmailapireply_list>();

                foreach (DataRow row in dt_datatable.Rows)
                {
                    string reply_id = row["reply_id"].ToString();

                    // Check if the reply already exists in getmodulelist
                    gmailapireply_list existingReply = getmodulelist.FirstOrDefault(r => r.reply_id == reply_id);

                    if (existingReply == null)
                    {
                        // If reply doesn't exist, create a new instance
                        existingReply = new gmailapireply_list
                        {
                            s_no = row["s_no"].ToString(),
                            inbox_id = row["inbox_id"].ToString(),
                            reply_id = reply_id,
                            from_id = row["from_id"].ToString(),
                            cc = row["cc"].ToString(),
                            bcc = row["bcc"].ToString(),
                            sent_date = row["sent_date"].ToString(),
                            sent_time = row["sent_time"].ToString(),
                            subject = row["subject"].ToString(),
                            body = row["body"].ToString(),
                            attachement_flag = row["attachement_flag"].ToString(),
                            both_body = row["both_body"].ToString(),
                            attachments = new List<gmailapireplyinboxatatchement_list>() // Initialize attachments list
                        };

                        getmodulelist.Add(existingReply); // Add the reply to the list
                    }

                    // Add attachment if exists
                    if (!string.IsNullOrEmpty(row["original_filename"].ToString()))
                    {
                        existingReply.attachments.Add(new gmailapireplyinboxatatchement_list
                        {
                            s_no = row["s_no"].ToString(),
                            inbox_id = row["inbox_id"].ToString(),
                            original_filename = row["original_filename"].ToString(),
                            modified_filename = row["modified_filename"].ToString(),
                            file_path = row["file_path"].ToString()
                        });
                    }
                }

                // Assign getmodulelist to values.gmailapireply_list
                values.gmailapireply_list = getmodulelist;

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                objcmnfunctions.LogForAudit($"*******Date***** {DateTime.Now:yyyy-MM-dd HH:mm:ss} *********** DataAccess:{methodName} ********** {ex.ToString()} *********** Error While Fetching Mail configuration Summary ***** Query**** {msSQL} *******Apiref********", $"SocialMedia/ErrorLog/Mail/Log{DateTime.Now:yyyy-MM-dd HH}.txt");
                throw; // Optionally handle or log the exception
            }
        }
        public void DaGetForwardMail(string inbox_id, MdlGmailCampaign values)
        {
            try
            {
                // Step 1: Get forward mails with attachment flag 'N'
                string msSQL1 = $@"SELECT s_no, forward_id, reply_id, inbox_id, to_id, cc, sent_date, bcc, subject, body, attachement_flag, both_body 
                            FROM crm_trn_tgmailinboxreplyforward 
                            WHERE inbox_id = '{inbox_id}' AND attachement_flag = 'N'";
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL1);

                List<gmailapiforward_list> getmodulelist1 = new List<gmailapiforward_list>();

                foreach (DataRow row in dt_datatable1.Rows)
                {
                    gmailapiforward_list forwardMail = new gmailapiforward_list
                    {
                        s_no = row["s_no"].ToString(),
                        forward_id = row["forward_id"].ToString(),
                        reply_id = row["reply_id"].ToString(),
                        inbox_id = row["inbox_id"].ToString(),
                        to_id = row["to_id"].ToString(),
                        cc = row["cc"].ToString(),
                        sent_date = row["sent_date"].ToString(),
                        bcc = row["bcc"].ToString(),
                        subject = row["subject"].ToString(),
                        body = row["body"].ToString(),
                        attachement_flag = row["attachement_flag"].ToString(),
                        both_body = row["both_body"].ToString(),
                        attachments = new List<gmailapireplyinboxatatchement_list>() // Initialize attachments list as empty
                    };

                    getmodulelist1.Add(forwardMail);
                }

                // Step 2: Get forward mails with attachment flag 'Y'
                string msSQL2 = $@"SELECT s_no, forward_id, reply_id, inbox_id, to_id, cc, sent_date, bcc, subject, body, attachement_flag, both_body 
                            FROM crm_trn_tgmailinboxreplyforward 
                            WHERE inbox_id = '{inbox_id}' AND attachement_flag = 'Y'";
                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL2);

                List<gmailapiforward_list> getmodulelist2 = new List<gmailapiforward_list>();

                foreach (DataRow row in dt_datatable2.Rows)
                {
                    gmailapiforward_list forwardMail = new gmailapiforward_list
                    {
                        s_no = row["s_no"].ToString(),
                        forward_id = row["forward_id"].ToString(),
                        reply_id = row["reply_id"].ToString(),
                        inbox_id = row["inbox_id"].ToString(),
                        to_id = row["to_id"].ToString(),
                        cc = row["cc"].ToString(),
                        sent_date = row["sent_date"].ToString(),
                        bcc = row["bcc"].ToString(),
                        subject = row["subject"].ToString(),
                        body = row["body"].ToString(),
                        attachement_flag = row["attachement_flag"].ToString(),
                        both_body = row["both_body"].ToString(),
                        attachments = new List<gmailapireplyinboxatatchement_list>() // Initialize attachments list
                    };

                    if (string.IsNullOrEmpty(row["reply_id"].ToString()) || row["reply_id"] == null || row["reply_id"].ToString() == "No")
                    {
                        // Get attachments from crm_trn_tgmailinboxattachment
                        string msSQL3 = $@"SELECT s_no, inbox_id, original_filename, modified_filename, file_path 
                                    FROM crm_trn_tgmailinboxattachment 
                                    WHERE inbox_id = '{row["inbox_id"].ToString()}'";
                        DataTable dt_datatable3 = objdbconn.GetDataTable(msSQL3);

                        foreach (DataRow attachmentRow in dt_datatable3.Rows)
                        {
                            forwardMail.attachments.Add(new gmailapireplyinboxatatchement_list
                            {
                                s_no = attachmentRow["s_no"].ToString(),
                                inbox_id = attachmentRow["inbox_id"].ToString(),
                                original_filename = attachmentRow["original_filename"].ToString(),
                                modified_filename = attachmentRow["modified_filename"].ToString(),
                                file_path = attachmentRow["file_path"].ToString()
                            });
                        }
                    }
                    else
                    {
                        // Get attachments from crm_trn_tgmailreplyattachment
                        string msSQL4 = $@"SELECT s_no, reply_id, original_filename, modified_filename, file_path 
                                    FROM crm_trn_tgmailreplyattachment 
                                    WHERE reply_id = '{row["reply_id"].ToString()}'";
                        DataTable dt_datatable4 = objdbconn.GetDataTable(msSQL4);

                        foreach (DataRow attachmentRow in dt_datatable4.Rows)
                        {
                            forwardMail.attachments.Add(new gmailapireplyinboxatatchement_list
                            {
                                s_no = attachmentRow["s_no"].ToString(),
                                reply_id = attachmentRow["reply_id"].ToString(),
                                original_filename = attachmentRow["original_filename"].ToString(),
                                modified_filename = attachmentRow["modified_filename"].ToString(),
                                file_path = attachmentRow["file_path"].ToString()
                            });
                        }
                    }

                    getmodulelist2.Add(forwardMail);
                }

                // Combine the two lists
                getmodulelist1.AddRange(getmodulelist2);

                // Assign getmodulelist to values.gmailapireply_list
                values.gmailapiforward_list = getmodulelist1;

                dt_datatable1.Dispose();
                dt_datatable2.Dispose();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                objcmnfunctions.LogForAudit($"*******Date***** {DateTime.Now:yyyy-MM-dd HH:mm:ss} *********** DataAccess:{methodName} ********** {ex.ToString()} *********** Error While Fetching Mail configuration Summary ***** Query**** {msSQL} *******Apiref********", $"SocialMedia/ErrorLog/Mail/Log{DateTime.Now:yyyy-MM-dd HH}.txt");
                throw; // Optionally handle or log the exception
            }
        }
        //public void DaGetForwardMail(string inbox_id, MdlGmailCampaign values)
        //{
        //    try
        //    {
        //        string msSQL = $@"SELECT r.s_no, r.reply_id, r.inbox_id, r.to_id, r.cc, 
        //            DATE_FORMAT(r.sent_date, '%b %e, %Y %h:%i %p') AS sent_date, 
        //            DATE_FORMAT(r.sent_date, '%h:%i %p') AS sent_time, 
        //            r.bcc, r.subject, r.body, r.attachement_flag, r.both_body, 
        //            CASE WHEN r.reply_id IS NULL THEN a_inbox.original_filename 
        //                 ELSE a_reply.original_filename 
        //            END AS original_filename, 
        //            CASE WHEN r.reply_id IS NULL THEN a_inbox.modified_filename 
        //                 ELSE a_reply.modified_filename 
        //            END AS modified_filename, 
        //            CASE WHEN r.reply_id IS NULL THEN a_inbox.file_path 
        //                 ELSE a_reply.file_path 
        //            END AS file_path 
        //        FROM crm_trn_tgmailinboxreplyforward r 
        //        LEFT JOIN crm_trn_tgmailinboxattachment a_inbox ON r.inbox_id = a_inbox.inbox_id AND r.reply_id IS NULL 
        //        LEFT JOIN crm_trn_tgmailreplyattachment a_reply ON r.reply_id = a_reply.reply_id AND r.reply_id IS NOT NULL 
        //        WHERE r.inbox_id = '{inbox_id}' 
        //        ORDER BY r.sent_date ASC";
        //        DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

        //        List<gmailapiforward_list> getmodulelist = new List<gmailapiforward_list>();

        //        foreach (DataRow row in dt_datatable.Rows)
        //        {
        //            string reply_id = row["reply_id"].ToString();

        //            // Check if the reply already exists in getmodulelist
        //            gmailapiforward_list existingReply = getmodulelist.FirstOrDefault(r => r.reply_id == reply_id);

        //            if (existingReply == null)
        //            {
        //                // If reply doesn't exist, create a new instance
        //                existingReply = new gmailapiforward_list
        //                {
        //                    s_no = row["s_no"].ToString(),
        //                    inbox_id = row["inbox_id"].ToString(),
        //                    reply_id = reply_id,
        //                    to_id = row["to_id"].ToString(),
        //                    cc = row["cc"].ToString(),
        //                    bcc = row["bcc"].ToString(),
        //                    sent_date = row["sent_date"].ToString(),
        //                    sent_time = row["sent_time"].ToString(),
        //                    subject = row["subject"].ToString(),
        //                    body = row["body"].ToString(),
        //                    attachement_flag = row["attachement_flag"].ToString(),
        //                    both_body = row["both_body"].ToString(),
        //                    attachments = new List<gmailapireplyinboxatatchement_list>() // Initialize attachments list
        //                };

        //                getmodulelist.Add(existingReply); // Add the reply to the list
        //            }

        //            // Add attachment if exists and attachement_flag is 'Y'
        //            if (!string.IsNullOrEmpty(row["original_filename"].ToString()) && row["attachement_flag"].ToString() == "Y")
        //            {
        //                existingReply.attachments.Add(new gmailapireplyinboxatatchement_list
        //                {
        //                    s_no = row["s_no"].ToString(),
        //                    inbox_id = row["inbox_id"].ToString(),
        //                    original_filename = row["original_filename"].ToString(),
        //                    modified_filename = row["modified_filename"].ToString(),
        //                    file_path = row["file_path"].ToString()
        //                });
        //            }
        //            else if (row["attachement_flag"].ToString() == "N")
        //            {
        //                existingReply.attachments = new List<gmailapireplyinboxatatchement_list>(); // Initialize attachments list as empty
        //            }
        //        }

        //        // Assign getmodulelist to values.gmailapireply_list
        //        values.gmailapiforward_list = getmodulelist;

        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //        objcmnfunctions.LogForAudit($"*******Date***** {DateTime.Now:yyyy-MM-dd HH:mm:ss} *********** DataAccess:{methodName} ********** {ex.ToString()} *********** Error While Fetching Mail configuration Summary ***** Query**** {msSQL} *******Apiref********", $"SocialMedia/ErrorLog/Mail/Log{DateTime.Now:yyyy-MM-dd HH}.txt");
        //        throw; // Optionally handle or log the exception
        //    }
        //}

        public async Task DaMovetoTrash(gmailmovedlist values)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();

                foreach (var emailId in values.gmailmovelist)
                {
                    try
                    {
                        string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + emailId.inbox_id + "'";
                        string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                        string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                        if (objOdbcDataReader.HasRows == true)
                        {

                            getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                            getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                            getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                            getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                        }
                        objOdbcDataReader.Close();
                        string clientId = getgmailcredentials.client_id;
                        string clientSecret = getgmailcredentials.client_secret;
                        string refreshToken = getgmailcredentials.refresh_token;

                        // Get the Gmail service instance
                        var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);
                        // Move the email to trash
                        var trashRequest = gmailService.Users.Messages.Trash("me", emailId.inbox_id);
                        var trashResponse = await trashRequest.ExecuteAsync();

                        // Check if the message was successfully moved to trash
                        if (trashResponse.LabelIds.Contains("TRASH"))
                        {
                            msSQL = " update  crm_trn_tgmailinbox set " +
                            " inbox_status = 'Trash'" +
                            "  where inbox_id='" + emailId.inbox_id + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        else
                        {
                            values.message = $"Error: Failed to move email {emailId} to trash.";
                            values.status = false;
                            objcmnfunctions.LogForAudit($"Error: Failed to move email {emailId} to trash.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                        if (mnResult != 0)
                        {
                            values.message = $"Email Moved to Trash successfully.";
                            values.status = true;
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Email Moving to Trash";
                        }
                    }
                    catch (Exception ex)
                    {
                        values.message = $"Error: Exception occurred while moving email {emailId} to trash.";
                        values.status = false;
                        objcmnfunctions.LogForAudit($"Error moving email {emailId} to trash: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error While Reply Mail !!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaMovetoTrash: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task DaMoveToInbox(gmailmovedlist values)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();



                foreach (var emailId in values.gmailmovelist)
                {
                    string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + emailId.inbox_id + "'";
                    string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                    string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                    if (objOdbcDataReader.HasRows == true)
                    {

                        getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                        getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                        getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                        getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    }
                    objOdbcDataReader.Close();
                    string clientId = getgmailcredentials.client_id;
                    string clientSecret = getgmailcredentials.client_secret;
                    string refreshToken = getgmailcredentials.refresh_token;

                    // Get the Gmail service instance
                    var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);
                    try
                    {
                        // Modify the labels to move the email from Trash to Inbox
                        var modifyRequest = new Google.Apis.Gmail.v1.Data.ModifyMessageRequest
                        {
                            AddLabelIds = new List<string> { "INBOX" },
                            RemoveLabelIds = new List<string> { "TRASH" }
                        };
                        var modifyResponse = await gmailService.Users.Messages.Modify(modifyRequest, "me", emailId.inbox_id).ExecuteAsync();

                        // Check if the message was successfully moved to Inbox
                        if (modifyResponse.LabelIds.Contains("INBOX") && !modifyResponse.LabelIds.Contains("TRASH"))
                        {
                            msSQL = "update crm_trn_tgmailinbox set " +
                                      "inbox_status = NULL " +
                                      "where inbox_id = '" + emailId.inbox_id + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                values.message = "Email Moved to Inbox successfully.";
                                values.status = true;
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occurred While Moving Email to Inbox";
                            }
                        }
                        else
                        {
                            values.message = $"Error: Failed to move email {emailId} to Inbox.";
                            values.status = false;
                            objcmnfunctions.LogForAudit($"Error: Failed to move email {emailId} to Inbox.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    catch (Exception ex)
                    {
                        values.message = $"Error: Exception occurred while moving email {emailId} to Inbox.";
                        values.status = false;
                        objcmnfunctions.LogForAudit($"Error moving email {emailId} to Inbox: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                values.status = false;
                values.message = "Error While Moving Mail to Inbox!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaMoveToInbox: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task DaGmailInboxStatusUpdate(replymail_list values)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();

                // Retrieve Gmail API credentials from database
                string msSQL1 = "select integrated_gmail from crm_trn_tgmailinbox where inbox_id ='" + values.inbox_id + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL2 = "select * from  crm_smm_gmail_service  where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL2);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                }
                objOdbcDataReader.Close();

                // Initialize Gmail service
                var gmailService = await GetGmailService(getgmailcredentials.client_id, getgmailcredentials.client_secret, getgmailcredentials.refresh_token);


                try
                {
                    // Fetch the message details to update read status
                    var messageDetails = await gmailService.Users.Messages.Get("me", values.inbox_id).ExecuteAsync();
                    var message = messageDetails.Payload;

                    // Set read_flag to true by default
                    bool isUnread = true;
                    // Update read status by modifying labels
                    var modifyRequest = new Google.Apis.Gmail.v1.Data.ModifyMessageRequest
                    {
                        RemoveLabelIds = isUnread ? new List<string> { "UNREAD" } : new List<string>(),
                        AddLabelIds = !isUnread ? new List<string> { "UNREAD" } : new List<string>()
                    };

                    // Execute the modify request
                    var modifyResponse = await gmailService.Users.Messages.Modify(modifyRequest, "me", values.inbox_id).ExecuteAsync();

                    // Check if the read status update was successful
                    if ((isUnread && !modifyResponse.LabelIds.Contains("UNREAD")) || (!isUnread && modifyResponse.LabelIds.Contains("UNREAD")))
                    {
                        // Update your database with the read status if necessary
                        msSQL = "UPDATE crm_trn_tgmailinbox SET read_flag = 'False' WHERE inbox_id = '" + values.inbox_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 1)
                        {
                            objcmnfunctions.LogForAudit($"Error: Failed to update read status of email {values.inbox_id}.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }

                    }
                    else
                    {

                        objcmnfunctions.LogForAudit($"Error: Failed to update read status of email {values.inbox_id}.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                catch (Exception ex)
                {

                    objcmnfunctions.LogForAudit($"Error updating read status of email {values.inbox_id}: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error occurred while processing emails.";
                objcmnfunctions.LogForAudit($"Error in DaGmailInboxStatusUpdate: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }

        static string DecodeBase64Format(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(bytes);
        }
        public void DaGetInboxAttchement(string inbox_id, MdlGmailCampaign values)
        {

            try
            {
                msSQL = "select s_no, inbox_id, original_filename, modified_filename, file_path from crm_trn_tgmailinboxattachment where inbox_id='" + inbox_id + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getmodulelist = new List<gmailapiinboxatatchement_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailapiinboxatatchement_list
                        {
                            s_no = row["s_no"].ToString(), // Modified line
                            inbox_id = row["inbox_id"].ToString(),
                            original_filename = row["original_filename"].ToString(),
                            modified_filename = row["modified_filename"].ToString(),
                            file_path = row["file_path"].ToString(),


                        });
                        values.gmailapiinboxatatchement_list = getmodulelist;
                    }
                }


                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        private static string DecodeFromBase64(string base64Value)
        {
            if (string.IsNullOrEmpty(base64Value))
            {
                return string.Empty;
            }

            // Check if the string is a valid Base64 encoded string
            if (IsBase64String(base64Value))
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(base64Value);
                    return Encoding.UTF8.GetString(bytes);
                }
                catch (FormatException)
                {
                    // Handle the case where the string is not a valid Base64 encoded string
                    return base64Value; // Or handle as needed, e.g., return an error message
                }
            }

            return base64Value;
        }

        private static bool IsBase64String(string value)
        {
            // Regular expression to check if a string is Base64 encoded
            string base64Pattern = @"^[a-zA-Z0-9\+/]*={0,2}$";
            return Regex.IsMatch(value, base64Pattern) && (value.Length % 4 == 0);
        }
        private static string DecodesFromBase64(string base64Value)
        {
            if (string.IsNullOrEmpty(base64Value))
            {
                return string.Empty;
            }

            // Check if the string is a valid Base64 encoded string
            if (IsBase64Strings(base64Value))
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(base64Value);
                    return Encoding.UTF8.GetString(bytes);
                }
                catch (FormatException)
                {
                    // Handle the case where the string is not a valid Base64 encoded string
                    return base64Value; // Or handle as needed, e.g., return an error message
                }
            }

            return base64Value;
        }

        // Helper method to check if a string is a valid base64 encoded string
        private static bool IsBase64Strings(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            value = value.Trim();
            return (value.Length % 4 == 0) && Regex.IsMatch(value, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        //public async Task<get> DaGmailAPIInboxMailLoad(string user_gid)
        //{
        //    get objresult = new get();
        //    gmailconfiguration getgmailcredentials = new gmailconfiguration();

        //    try
        //    {
        //        // Read Gmail credentials from database
        //        msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid='" + user_gid + "'";
        //        string gmail_address = objdbconn.GetExecuteScalar(msSQL1);
        //        msSQL = "select * from crm_smm_gmail_service WHERE gmail_address = '" + gmail_address + "'  LIMIT 1";
        //        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

        //        if (objOdbcDataReader.HasRows)
        //        {
        //            getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
        //            getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
        //            getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
        //            getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
        //            getgmailcredentials.s_no = objOdbcDataReader["s_no"].ToString();
        //        }
        //        objOdbcDataReader.Close();

        //        string clientId = getgmailcredentials.client_id;
        //        string clientSecret = getgmailcredentials.client_secret;
        //        string refreshToken = getgmailcredentials.refresh_token;
        //        string s_no = getgmailcredentials.s_no;

        //        // Initialize Gmail service asynchronously
        //        var service = await GetGmailService(clientId, clientSecret, refreshToken);

        //        // Run operations asynchronously using Task.WhenAll
        //        Task listMessagesWithoutAttachmentsTask = ListAllMessagesWithoutAttachments(service, "me", "inbox", s_no);
        //        Task listMessagesWithAttachmentsTask = ListAllMessagesWithAttachments(service, "me", "inbox", s_no);

        //        await Task.WhenAll(listMessagesWithoutAttachmentsTask, listMessagesWithAttachmentsTask);

        //        objresult.status = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception details
        //        objcmnfunctions.LogForAudit(
        //            "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
        //            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
        //            "***********" + ex.ToString() +
        //            "***********" + "*****Query****" + msSQL +
        //            "*******Apiref********",
        //            "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".txt"
        //        );

        //        // Optionally, you can set the status to false or add additional error information to the result
        //        objresult.status = false;
        //        // Assuming `get` class has an `errorMessage` property
        //    }

        //    return objresult;
        //}
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private static Task<get> _runningTask;

        public async Task<get> DaGmailAPIInboxMailLoad(string user_gid)
        {
            _queue.Enqueue(user_gid); // Add the request to the queue
            await ProcessQueue(); // Process the queue
            return await _runningTask;
        }

        private async Task ProcessQueue()
        {
            while (_queue.TryDequeue(out string user_gid))
            {
                await _semaphore.WaitAsync(); // Ensure that only one method runs at a time
                try
                {
                    if (_runningTask != null && !_runningTask.IsCompleted)
                    {
                        await _runningTask; // Wait for the previous task to complete
                    }

                    _runningTask = ExecuteDaGmailAPIInboxMailLoad(user_gid); // Start a new task
                    await _runningTask;
                }
                finally
                {
                    _semaphore.Release(); // Release the semaphore
                }
            }
        }


        private async Task<get> ExecuteDaGmailAPIInboxMailLoad(string user_gid)
        {
            get objresult = new get();
            gmailconfiguration getgmailcredentials = new gmailconfiguration();

            try
            {
                // Read Gmail credentials from database
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid='" + user_gid + "'";
                string gmail_address = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL = "select * from crm_smm_gmail_service WHERE gmail_address = '" + gmail_address + "' LIMIT 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    getgmailcredentials.s_no = objOdbcDataReader["s_no"].ToString();
                }
                objOdbcDataReader.Close();

                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;
                string s_no = getgmailcredentials.s_no;

                // Initialize Gmail service asynchronously
                var service = await GetGmailService(clientId, clientSecret, refreshToken);

                // Run operations asynchronously using Task.WhenAll
                Task listMessagesWithoutAttachmentsTask = ListAllMessagesWithoutAttachments(service, "me", "inbox", s_no);
                // Task listMessagesWithAttachmentsTask = ListAllMessagesWithAttachments(service, "me", "inbox", s_no);

                // await Task.WhenAll(listMessagesWithoutAttachmentsTask, listMessagesWithAttachmentsTask);
                await Task.WhenAll(listMessagesWithoutAttachmentsTask);

                objresult.status = true;
            }
            catch (Exception ex)
            {
                // Log the exception details
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    "***********" + ex.ToString() +
                    "***********" + "*****Query****" + msSQL +
                    "*******Apiref********",
                    "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".txt"
                );

                // Optionally, you can set the status to false or add additional error information to the result
                objresult.status = false;
                // Assuming `get` class has an `errorMessage` property
                objresult.message = ex.Message;
            }

            return objresult;
        }


        static async Task ListAllMessagesWithoutAttachments(GmailService service, string userId, string label, string s_no)
        {
            string lsdocumenttype_gid = string.Empty;
            string msSQL, msGETGID, msGETGID1, lsfile_name, file_date, current_date, destFile, lsupload_path, sourceFile, sourcePath, fileName, temp_path, lstemp_file;
            int mnResult;
            OdbcDataReader objOdbcDataReader;
            dbconn objdbconn = new dbconn();
            cmnfunctions objcmnfunctions = new cmnfunctions();
            DataTable dt_datatable;
            string ContentType = string.Empty;

            // Initialize the list request
            var listRequest = service.Users.Messages.List(userId);

            DateTime currentDate = DateTime.Now;
            DateTime previousDate = currentDate.AddDays(-1);

            string currentDateTimeString = currentDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string previousDateTimeString = previousDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

            listRequest.Q = "in:inbox -has:attachment"; // Query to fetch emails from the inbox
            listRequest.MaxResults = 50; // Limit to top 50 emails
            string lsgmail_address;
            // Keep track of total messages fetched
            int totalMessagesFetched = 0;

            do
            {

                // Execute the list request asynchronously
                ListMessagesResponse response = await listRequest.ExecuteAsync();

                // Check if messages were returned
                if (response.Messages != null && response.Messages.Any())
                {
                    foreach (var message in response.Messages)
                    {
                        var messageDetails = await service.Users.Messages.Get(userId, message.Id).ExecuteAsync();
                        var payload = messageDetails.Payload;

                        string messageId = message.Id;
                        string subject = payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                        string date = payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;
                        string body = string.Empty;
                        string encodedSubject = EncodeToBase64(subject);
                        string decodedBody = string.Empty;
                        string formattedDateString = string.Empty;
                        string from = null;
                        string cc = null;
                        string bcc = null;
                        bool isUnread = messageDetails.LabelIds.Contains("UNREAD");
                        //var fromHeader = payload.Headers.FirstOrDefault(h => h.Name == "From");
                        //if (fromHeader != null)
                        //{
                        //    // Extract the email address using a regular expression
                        //    var match = Regex.Match(fromHeader.Value, @"\<(.*?)\>");
                        //    if (match.Success)
                        //    {
                        //        from = match.Groups[1].Value;
                        //    }
                        //    else
                        //    {
                        //        // If the regular expression fails, fallback to using the entire From header value
                        //        from = fromHeader.Value;
                        //    }
                        //}
                        // string cc = payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;

                        string ccccc = payload.Headers.FirstOrDefault(h => h.Name.Equals("CC", StringComparison.OrdinalIgnoreCase))?.Value;
                        string fromcv = payload.Headers.FirstOrDefault(h => h.Name.Equals("From", StringComparison.OrdinalIgnoreCase))?.Value;
                        string Bcc = payload.Headers.FirstOrDefault(h => h.Name.Equals("Bcc", StringComparison.OrdinalIgnoreCase))?.Value;
                        if (!string.IsNullOrEmpty(ccccc))
                        {
                            // Use a regular expression to extract all email addresses
                            var emailMatches = Regex.Matches(ccccc, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                            var emailAddresses = emailMatches.Cast<Match>()
                                                             .Select(match => match.Value)
                                                             .ToList();

                            // Join the extracted email addresses with a comma
                            cc = string.Join(",", emailAddresses);
                        }
                        if (!string.IsNullOrEmpty(fromcv))
                        {
                            // Use a regular expression to extract all email addresses
                            var emailMatches = Regex.Matches(fromcv, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                            var emailAddresses = emailMatches.Cast<Match>()
                                                             .Select(match => match.Value)
                                                             .ToList();

                            // Join the extracted email addresses with a comma
                            from = string.Join(",", emailAddresses);

                        }
                        if (!string.IsNullOrEmpty(Bcc))
                        {
                            // Use a regular expression to extract all email addresses
                            var emailMatches = Regex.Matches(Bcc, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                            var emailAddresses = emailMatches.Cast<Match>()
                                                             .Select(match => match.Value)
                                                             .ToList();

                            // Join the extracted email addresses with a comma
                            bcc = string.Join(",", emailAddresses);
                        }
                        //string bcc = payload.Headers.FirstOrDefault(h => h.Name == "Bcc")?.Value;
                        string[] formats = {
                                // Existing formats
                                "ddd, dd MMM yyyy HH:mm:ss zzz",
                                "ddd, dd MMM yyyy HH:mm:ss zzzz",
                                "ddd, dd MMM yyyy HH:mm:ss zzz (zzz)",
                                "ddd, dd MMM yyyy HH:mm:ss zzz",
                                "ddd, dd MMM yyyy HH:mm:sszzz",
                                "ddd, dd MMM yyyy HH:mm:ss",
                                "dd MMM yyyy HH:mm:ss zzz",
                                "dd MMM yyyy HH:mm:sszzz",
                                "dd MMM yyyy HH:mm:ss",
                                "yyyy-MM-dd'T'HH:mm:ss'Z'",
                                "yyyy-MM-dd'T'HH:mm:sszzz",
                                "yyyy-MM-dd'T'HH:mm:ss.fffzzz",
                                "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                                "yyyy-MM-ddTHH:mm:sszzz",
                                "yyyy-MM-ddTHH:mm:ss.fffzzz",
                                "yyyy-MM-ddTHH:mm:ss.fff'Z'",
                                "yyyy-MM-dd HH:mm:sszzz",
                                "yyyy-MM-dd HH:mm:ss.fffzzz",
                                "yyyy-MM-dd HH:mm:ss.fff'Z'",
                                "ddd, dd MMM yyyy HH:mm:ss zzz '(UTC)'",
                                // India formats
                                "dd-MM-yyyy HH:mm:ss",
                                "dd/MM/yyyy HH:mm:ss",
                                "dd-MM-yyyy hh:mm:ss tt",
                                "dd/MM/yyyy hh:mm:ss tt",
                                "dd-MM-yyyy HH:mm:ss zzz",
                                "dd/MM/yyyy HH:mm:ss zzz",
                                // UK formats
                                "dd/MM/yyyy HH:mm:ss",
                                "dd/MM/yyyy hh:mm:ss tt",
                                "dd/MM/yyyy HH:mm:ss zzz",
                                "dd MMM yyyy HH:mm:ss",
                                "dd MMM yyyy hh:mm:ss tt",
                                "dd MMM yyyy HH:mm:ss zzz",
                                "ddd, dd MMM yyyy HH:mm:ss zzz",
                                // New format
                                "ddd, d MMM yyyy HH:mm:ss zzz"
                            };

                        //if (DateTimeOffset.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffset))
                        if (DateTimeOffset.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffset))

                        {
                            formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            try
                            {
                                // Manually process the date string to handle variations
                                string[] parts = date.Split(' ');

                                // Remove day of the week and optional parts like (UTC)
                                if (parts.Length > 6)
                                {
                                    parts = parts.Take(6).ToArray();
                                }

                                // Extract the date and time part
                                string dateTimePart = parts.Length >= 5 ? string.Join(" ", parts.Skip(1).Take(4)) : string.Empty;

                                // Extract the time zone offset part
                                string timeZoneOffsetPart = parts.Length == 6 ? parts[5] : string.Empty;

                                // Parse and format date and time
                                if (DateTimeOffset.TryParseExact(
                                    dateTimePart + " " + timeZoneOffsetPart,
                                    "dd MMM yyyy HH:mm:ss zzz",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out dateTimeOffset))
                                {
                                    formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else if (DateTimeOffset.TryParseExact(
                                    dateTimePart + " " + timeZoneOffsetPart,
                                    "dd MMM yyyy HH:mm:sszzz",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out dateTimeOffset))
                                {
                                    formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                            }
                            catch (FormatException ex)
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                             " * **********" + ex.Message.ToString() +
                                             "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                            }
                        }

                        // If the message has a text/plain part, use it as the message body
                        if (payload.MimeType == "text/html" && !string.IsNullOrEmpty(payload.Body?.Data))
                        {
                            body = payload.Body.Data;
                            decodedBody = DecodeBase64(body);
                        }
                        // If the message has multipart parts, recursively find the text/plain part
                        else if (payload.Parts != null)
                        {
                            foreach (var part in payload.Parts)
                            {
                                body = GetMessageBody(part);
                                decodedBody = DecodeBase64(body);
                                if (!string.IsNullOrEmpty(body))
                                    break;
                            }
                        }

                        msSQL = " select inbox_id  from crm_trn_tgmailinbox where inbox_id = '" + messageId + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count == 0)
                        {
                            if (DecodeFromBase64(encodedSubject.ToString()).IndexOf("New customer message on", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                msSQL = "select gmail_address from crm_smm_gmail_service where s_no ='" + s_no + "'";
                                string gmail_address = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = " select company_code from  adm_mst_tcompany limit 1";
                                string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = "INSERT INTO crm_trn_tgmailinbox (" +
                                    "inbox_id, " +
                                    "from_id, " +
                                    "integrated_gmail, " +
                                    "cc, " +
                                    "bcc, " +
                                    "subject, " +
                                    "body, " +
                                    "attachement_flag, " +
                                    "read_flag, " +
                                    "shopify_enquiry, " +
                                    "company_code, " +
                                    "sent_date) " +
                                    "VALUES (" +
                                    "'" + messageId + "', " +
                                    "'" + from + "', " +
                                    "'" + gmail_address + "', " +
                                    "'" + cc + "', " +
                                    "'" + bcc + "', " +
                                    "'" + encodedSubject + "', " +
                                    "'" + body + "', " +
                                    "'N', " +
                                    "'" + isUnread + "', " +
                                    "'Y', " +
                                    "'" + lscompany_code + "', " +
                                    "'" + formattedDateString + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 1)
                                {
                                    objcmnfunctions.LogForAudit(
                                        "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                        "***********" + msSQL +
                                        "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                        DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                }
                             
                            }
                        }
                        dt_datatable.Dispose();
                        //objOdbcDataReader.Close();
                    }

                    // Update total messages fetched
                    totalMessagesFetched += response.Messages.Count();
                    if (totalMessagesFetched >= 50)
                    {

                        //     msSQL = "SELECT * FROM crm_smm_gmail_service WHERE s_no != '" + s_no + "'";
                        //     dt_datatable = objdbconn.GetDataTable(msSQL);

                        //     var getModuleList = new List<gmailmultiple_credentials>();
                        //     if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                        //     {
                        //         foreach (DataRow dt in dt_datatable.Rows)
                        //         {
                        //             getModuleList.Add(new gmailmultiple_credentials
                        //             {
                        //                 client_id = dt["client_id"].ToString(),
                        //                 client_secret = dt["client_secret"].ToString(),
                        //                 refresh_token = dt["refresh_token"].ToString(),
                        //                 gmail_address = dt["gmail_address"].ToString(),
                        //                 s_no = dt["s_no"].ToString()
                        //             });
                        //         }
                        //     }

                        //     for (int i = 0; i < getModuleList.Count; i++)
                        //     {
                        //         var service1 = await GetGmailService(getModuleList[i].client_id, getModuleList[i].client_secret, getModuleList[i].refresh_token);
                        //         listRequest = service1.Users.Messages.List(userId);

                        //         listRequest.Q = "in:inbox -has:attachment"; // Query to fetch emails from the inbox
                        //         listRequest.MaxResults = 50; // Limit to top 50 emails


                        //         do
                        //         {
                        //             // Execute the list request asynchronously
                        //             ListMessagesResponse response1 = await listRequest.ExecuteAsync();

                        //             // Check if messages were returned
                        //             if (response1.Messages != null && response1.Messages.Any())
                        //             {
                        //                 foreach (var message in response1.Messages)
                        //                 {
                        //                     var messageDetails = await service1.Users.Messages.Get(userId, message.Id).ExecuteAsync();
                        //                     var payload = messageDetails.Payload;

                        //                     string messageId = message.Id;
                        //                     string subject = payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                        //                     string date = payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;
                        //                     string body = string.Empty;
                        //                     string encodedSubject = EncodeToBase64(subject);
                        //                     string decodedBody = string.Empty;
                        //                     string formattedDateString = string.Empty;
                        //                     string from = null;
                        //                     string cc = null;
                        //                     string bcc = null;
                        //                     bool isUnread = messageDetails.LabelIds.Contains("UNREAD");

                        //                     string ccccc = payload.Headers.FirstOrDefault(h => h.Name.Equals("CC", StringComparison.OrdinalIgnoreCase))?.Value;
                        //                     string fromcv = payload.Headers.FirstOrDefault(h => h.Name.Equals("From", StringComparison.OrdinalIgnoreCase))?.Value;
                        //                     string Bcc = payload.Headers.FirstOrDefault(h => h.Name.Equals("Bcc", StringComparison.OrdinalIgnoreCase))?.Value;
                        //                     if (!string.IsNullOrEmpty(ccccc))
                        //                     {
                        //                         // Use a regular expression to extract all email addresses
                        //                         var emailMatches = Regex.Matches(ccccc, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                        //                         var emailAddresses = emailMatches.Cast<Match>()
                        //                                                          .Select(match => match.Value)
                        //                                                          .ToList();

                        //                         // Join the extracted email addresses with a comma
                        //                         cc = string.Join(",", emailAddresses);
                        //                     }
                        //                     if (!string.IsNullOrEmpty(fromcv))
                        //                     {
                        //                         // Use a regular expression to extract all email addresses
                        //                         var emailMatches = Regex.Matches(fromcv, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                        //                         var emailAddresses = emailMatches.Cast<Match>()
                        //                                                          .Select(match => match.Value)
                        //                                                          .ToList();

                        //                         // Join the extracted email addresses with a comma
                        //                         from = string.Join(",", emailAddresses);

                        //                     }
                        //                     if (!string.IsNullOrEmpty(Bcc))
                        //                     {
                        //                         // Use a regular expression to extract all email addresses
                        //                         var emailMatches = Regex.Matches(Bcc, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                        //                         var emailAddresses = emailMatches.Cast<Match>()
                        //                                                          .Select(match => match.Value)
                        //                                                          .ToList();

                        //                         // Join the extracted email addresses with a comma
                        //                         bcc = string.Join(",", emailAddresses);
                        //                     }
                        //                     //string bcc = payload.Headers.FirstOrDefault(h => h.Name == "Bcc")?.Value;
                        //                     string[] formats = {
                        //    // Existing formats
                        //    "ddd, dd MMM yyyy HH:mm:ss zzz",
                        //    "ddd, dd MMM yyyy HH:mm:ss zzzz",
                        //    "ddd, dd MMM yyyy HH:mm:ss zzz (zzz)",
                        //    "ddd, dd MMM yyyy HH:mm:ss zzz",
                        //    "ddd, dd MMM yyyy HH:mm:sszzz",
                        //    "ddd, dd MMM yyyy HH:mm:ss",
                        //    "dd MMM yyyy HH:mm:ss zzz",
                        //    "dd MMM yyyy HH:mm:sszzz",
                        //    "dd MMM yyyy HH:mm:ss",
                        //    "yyyy-MM-dd'T'HH:mm:ss'Z'",
                        //    "yyyy-MM-dd'T'HH:mm:sszzz",
                        //    "yyyy-MM-dd'T'HH:mm:ss.fffzzz",
                        //    "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                        //    "yyyy-MM-ddTHH:mm:sszzz",
                        //    "yyyy-MM-ddTHH:mm:ss.fffzzz",
                        //    "yyyy-MM-ddTHH:mm:ss.fff'Z'",
                        //    "yyyy-MM-dd HH:mm:sszzz",
                        //    "yyyy-MM-dd HH:mm:ss.fffzzz",
                        //    "yyyy-MM-dd HH:mm:ss.fff'Z'",
                        //    "ddd, dd MMM yyyy HH:mm:ss zzz '(UTC)'",
                        //    // India formats
                        //    "dd-MM-yyyy HH:mm:ss",
                        //    "dd/MM/yyyy HH:mm:ss",
                        //    "dd-MM-yyyy hh:mm:ss tt",
                        //    "dd/MM/yyyy hh:mm:ss tt",
                        //    "dd-MM-yyyy HH:mm:ss zzz",
                        //    "dd/MM/yyyy HH:mm:ss zzz",
                        //    // UK formats
                        //    "dd/MM/yyyy HH:mm:ss",
                        //    "dd/MM/yyyy hh:mm:ss tt",
                        //    "dd/MM/yyyy HH:mm:ss zzz",
                        //    "dd MMM yyyy HH:mm:ss",
                        //    "dd MMM yyyy hh:mm:ss tt",
                        //    "dd MMM yyyy HH:mm:ss zzz",
                        //    "ddd, dd MMM yyyy HH:mm:ss zzz",
                        //    // New format
                        //    "ddd, d MMM yyyy HH:mm:ss zzz"
                        //};

                        //                     //if (DateTimeOffset.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffset))
                        //                     if (DateTimeOffset.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffset))

                        //                     {
                        //                         formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        //                     }
                        //                     else
                        //                     {
                        //                         try
                        //                         {
                        //                             // Manually process the date string to handle variations
                        //                             string[] parts = date.Split(' ');

                        //                             // Remove day of the week and optional parts like (UTC)
                        //                             if (parts.Length > 6)
                        //                             {
                        //                                 parts = parts.Take(6).ToArray();
                        //                             }

                        //                             // Extract the date and time part
                        //                             string dateTimePart = parts.Length >= 5 ? string.Join(" ", parts.Skip(1).Take(4)) : string.Empty;

                        //                             // Extract the time zone offset part
                        //                             string timeZoneOffsetPart = parts.Length == 6 ? parts[5] : string.Empty;

                        //                             // Parse and format date and time
                        //                             if (DateTimeOffset.TryParseExact(
                        //                                 dateTimePart + " " + timeZoneOffsetPart,
                        //                                 "dd MMM yyyy HH:mm:ss zzz",
                        //                                 CultureInfo.InvariantCulture,
                        //                                 DateTimeStyles.None,
                        //                                 out dateTimeOffset))
                        //                             {
                        //                                 formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        //                             }
                        //                             else if (DateTimeOffset.TryParseExact(
                        //                                 dateTimePart + " " + timeZoneOffsetPart,
                        //                                 "dd MMM yyyy HH:mm:sszzz",
                        //                                 CultureInfo.InvariantCulture,
                        //                                 DateTimeStyles.None,
                        //                                 out dateTimeOffset))
                        //                             {
                        //                                 formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        //                             }
                        //                         }
                        //                         catch (FormatException ex)
                        //                         {
                        //                             objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                        //                                          "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                        //                                          " * **********" + ex.Message.ToString() +
                        //                                          "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                        //                                          DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        //                         }
                        //                     }

                        //                     // If the message has a text/plain part, use it as the message body
                        //                     if (payload.MimeType == "text/html" && !string.IsNullOrEmpty(payload.Body?.Data))
                        //                     {
                        //                         body = payload.Body.Data;
                        //                         decodedBody = DecodeBase64(body);
                        //                     }
                        //                     // If the message has multipart parts, recursively find the text/plain part
                        //                     else if (payload.Parts != null)
                        //                     {
                        //                         foreach (var part in payload.Parts)
                        //                         {
                        //                             body = GetMessageBody(part);
                        //                             decodedBody = DecodeBase64(body);
                        //                             if (!string.IsNullOrEmpty(body))
                        //                                 break;
                        //                         }
                        //                     }

                        //                     msSQL = " select inbox_id  from crm_trn_tgmailinbox where inbox_id = '" + messageId + "'";
                        //                     objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        //                     if (objOdbcDataReader.HasRows != true)
                        //                     {
                        //                         msSQL = "select gmail_address from crm_smm_gmail_service where s_no ='" + getModuleList[i].s_no + "'";
                        //                         string gmail_address = objdbconn.GetExecuteScalar(msSQL);
                        //                         msSQL = " select company_code from  adm_mst_tcompany limit 1";
                        //                         string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                        //                         msSQL = "INSERT INTO crm_trn_tgmailinbox (" +
                        //                             "inbox_id, " +
                        //                             "from_id, " +
                        //                             "integrated_gmail, " +
                        //                             "cc, " +
                        //                             "bcc, " +
                        //                             "subject, " +
                        //                             "body, " +
                        //                             "attachement_flag, " +
                        //                             "read_flag, " +
                        //                             "company_code, " +
                        //                             "sent_date) " +
                        //                             "VALUES (" +
                        //                             "'" + messageId + "', " +
                        //                             "'" + from + "', " +
                        //                             "'" + gmail_address + "', " +
                        //                             "'" + cc + "', " +
                        //                             "'" + bcc + "', " +
                        //                             "'" + encodedSubject + "', " +
                        //                             "'" + body + "', " +
                        //                             "'N', " +
                        //                             "'" + isUnread + "', " +
                        //                             "'" + lscompany_code + "', " +
                        //                             "'" + formattedDateString + "')";

                        //                         mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        //                         if (mnResult != 1)
                        //                         {
                        //                             objcmnfunctions.LogForAudit(
                        //                                 "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                        //                                 "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                        //                                 "***********" + msSQL +
                        //                                 "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                        //                                 DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        //                         }
                        //                     }

                        //                     objOdbcDataReader.Close();
                        //                 }

                        //                 // Update total messages fetched
                        //                 totalMessagesFetched += response.Messages.Count();
                        //                 if (totalMessagesFetched >= 50)
                        //                 {

                        //                     break;
                        //                 }
                        //                 // Set the page token for the next request
                        //                 listRequest.PageToken = response.NextPageToken;
                        //             }
                        //             else
                        //             {
                        //                 break; // Break the loop if no more messages found
                        //             }
                        //             // } while (!string.IsNullOrEmpty(listRequest.PageToken));
                        //         } while (!string.IsNullOrEmpty(listRequest.PageToken) && totalMessagesFetched < 50);
                        //         // You can do further processing with service1 if needed
                        //     }
                        break;
                    }
                    // Set the page token for the next request
                    listRequest.PageToken = response.NextPageToken;
                }
                else
                {
                    break; // Break the loop if no more messages found
                }
                // } while (!string.IsNullOrEmpty(listRequest.PageToken));
            } while (!string.IsNullOrEmpty(listRequest.PageToken) && totalMessagesFetched < 50);

        }

        static async Task ListAllMessagesWithAttachments(GmailService service, string userId, string label, string s_no)
        {
            string lsdocumenttype_gid = string.Empty;
            string msSQL, msGETGID, msGETGID1, lsfile_name, file_date, current_date, destFile, lsupload_path, sourceFile, sourcePath, fileName, temp_path, lstemp_file;
            //var httpRequest = HttpContext.Current;
            int mnResult, mnResult1;
            string lstotaldn_count, lseligibledn_count, lsdngenerated_count, lsdnsent_count, lsdnskip_count, lsreverted_count, lsdn_hold, lsdnpending;
            OdbcDataReader objOdbcDataReader;
            dbconn objdbconn = new dbconn();
            cmnfunctions objcmnfunctions = new cmnfunctions();
            DataTable dt_datatable;
            string ContentType = string.Empty;
            // Initialize the list request
            var listRequest = service.Users.Messages.List(userId);
            //listRequest.LabelIds = label;
            listRequest.Q = "in:inbox has:attachment";
            //listRequest.Q = $"in:inbox has:attachment after:{DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd")} before:{DateTime.Now.ToString("yyyy-MM-dd")}";
            listRequest.MaxResults = 50;

            // Keep track of total messages fetched
            int totalMessagesFetched = 0;


            do
            {

                // Execute the list request asynchronously
                ListMessagesResponse response = await listRequest.ExecuteAsync();

                // Check if messages were returned
                if (response.Messages != null && response.Messages.Any())
                {
                    foreach (var message in response.Messages)
                    {
                        var messageDetails = await service.Users.Messages.Get(userId, message.Id).ExecuteAsync();
                        var payload = messageDetails.Payload;
                        string messageId = message.Id;
                        string subject = payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                        //  string from = payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                        string date = payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;
                        string body = string.Empty;
                        string decodedBody = string.Empty;
                        string encodedSubject = EncodeToBase64(subject);
                        string formattedDateString = string.Empty;
                        string from = null;
                        string cc = null;
                        string bcc = null;
                        bool isUnread = messageDetails.LabelIds.Contains("UNREAD");
                        //var fromHeader = payload.Headers.FirstOrDefault(h => h.Name == "From");
                        //if (fromHeader != null)
                        //{
                        //    // Extract the email address using a regular expression
                        //    var match = Regex.Match(fromHeader.Value, @"\<(.*?)\>");
                        //    if (match.Success)
                        //    {
                        //        from = match.Groups[1].Value;
                        //    }
                        //    else
                        //    {
                        //        // If the regular expression fails, fallback to using the entire From header value
                        //        from = fromHeader.Value;
                        //    }
                        //}
                        // string cc = payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;

                        string ccccc = payload.Headers.FirstOrDefault(h => h.Name.Equals("CC", StringComparison.OrdinalIgnoreCase))?.Value;
                        string fromcv = payload.Headers.FirstOrDefault(h => h.Name.Equals("From", StringComparison.OrdinalIgnoreCase))?.Value;
                        string Bcc = payload.Headers.FirstOrDefault(h => h.Name.Equals("Bcc", StringComparison.OrdinalIgnoreCase))?.Value;
                        if (!string.IsNullOrEmpty(ccccc))
                        {
                            // Use a regular expression to extract all email addresses
                            var emailMatches = Regex.Matches(ccccc, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                            var emailAddresses = emailMatches.Cast<Match>()
                                                             .Select(match => match.Value)
                                                             .ToList();

                            // Join the extracted email addresses with a comma
                            cc = string.Join(",", emailAddresses);
                        }
                        if (!string.IsNullOrEmpty(fromcv))
                        {
                            // Use a regular expression to extract all email addresses
                            var emailMatches = Regex.Matches(fromcv, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                            var emailAddresses = emailMatches.Cast<Match>()
                                                             .Select(match => match.Value)
                                                             .ToList();

                            // Join the extracted email addresses with a comma
                            from = string.Join(",", emailAddresses);

                        }
                        if (!string.IsNullOrEmpty(Bcc))
                        {
                            // Use a regular expression to extract all email addresses
                            var emailMatches = Regex.Matches(Bcc, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                            var emailAddresses = emailMatches.Cast<Match>()
                                                             .Select(match => match.Value)
                                                             .ToList();

                            // Join the extracted email addresses with a comma
                            bcc = string.Join(",", emailAddresses);
                        }
                        //string bcc = payload.Headers.FirstOrDefault(h => h.Name == "Bcc")?.Value;
                        string[] formats = {
                                    // Existing formats
                                    "ddd, dd MMM yyyy HH:mm:ss zzz",
                                    "ddd, dd MMM yyyy HH:mm:ss zzzz",
                                    "ddd, dd MMM yyyy HH:mm:ss zzz (zzz)",
                                    "ddd, dd MMM yyyy HH:mm:ss zzz",
                                    "ddd, dd MMM yyyy HH:mm:sszzz",
                                    "ddd, dd MMM yyyy HH:mm:ss",
                                    "dd MMM yyyy HH:mm:ss zzz",
                                    "dd MMM yyyy HH:mm:sszzz",
                                    "dd MMM yyyy HH:mm:ss",
                                    "yyyy-MM-dd'T'HH:mm:ss'Z'",
                                    "yyyy-MM-dd'T'HH:mm:sszzz",
                                    "yyyy-MM-dd'T'HH:mm:ss.fffzzz",
                                    "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                                    "yyyy-MM-ddTHH:mm:sszzz",
                                    "yyyy-MM-ddTHH:mm:ss.fffzzz",
                                    "yyyy-MM-ddTHH:mm:ss.fff'Z'",
                                    "yyyy-MM-dd HH:mm:sszzz",
                                    "yyyy-MM-dd HH:mm:ss.fffzzz",
                                    "yyyy-MM-dd HH:mm:ss.fff'Z'",
                                    "ddd, dd MMM yyyy HH:mm:ss zzz '(UTC)'",
                                    // India formats
                                    "dd-MM-yyyy HH:mm:ss",
                                    "dd/MM/yyyy HH:mm:ss",
                                    "dd-MM-yyyy hh:mm:ss tt",
                                    "dd/MM/yyyy hh:mm:ss tt",
                                    "dd-MM-yyyy HH:mm:ss zzz",
                                    "dd/MM/yyyy HH:mm:ss zzz",
                                    // UK formats
                                    "dd/MM/yyyy HH:mm:ss",
                                    "dd/MM/yyyy hh:mm:ss tt",
                                    "dd/MM/yyyy HH:mm:ss zzz",
                                    "dd MMM yyyy HH:mm:ss",
                                    "dd MMM yyyy hh:mm:ss tt",
                                    "dd MMM yyyy HH:mm:ss zzz",
                                    "ddd, dd MMM yyyy HH:mm:ss zzz",
                                    // New format
                                    "ddd, d MMM yyyy HH:mm:ss zzz"
                                };



                        //if (DateTimeOffset.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffset))
                        if (DateTimeOffset.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffset))

                        {
                            formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            try
                            {
                                // Manually process the date string to handle variations
                                string[] parts = date.Split(' ');

                                // Remove day of the week and optional parts like (UTC)
                                if (parts.Length > 6)
                                {
                                    parts = parts.Take(6).ToArray();
                                }

                                // Extract the date and time part
                                string dateTimePart = parts.Length >= 5 ? string.Join(" ", parts.Skip(1).Take(4)) : string.Empty;

                                // Extract the time zone offset part
                                string timeZoneOffsetPart = parts.Length == 6 ? parts[5] : string.Empty;

                                // Parse and format date and time
                                if (DateTimeOffset.TryParseExact(
                                    dateTimePart + " " + timeZoneOffsetPart,
                                    "dd MMM yyyy HH:mm:ss zzz",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out dateTimeOffset))
                                {
                                    formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else if (DateTimeOffset.TryParseExact(
                                    dateTimePart + " " + timeZoneOffsetPart,
                                    "dd MMM yyyy HH:mm:sszzz",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out dateTimeOffset))
                                {
                                    formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                            }
                            catch (FormatException ex)
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                             " * **********" + ex.Message.ToString() +
                                             "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                            }
                        }



                        // If the message has a text/plain part, use it as the message body
                        if (payload.MimeType == "text/plain" && !string.IsNullOrEmpty(payload.Body?.Data))
                        {
                            body = payload.Body.Data;
                            decodedBody = DecodeBase64(body);
                        }
                        // If the message has multipart parts, recursively find the text/plain part
                        else if (payload.Parts != null)
                        {
                            foreach (var part in payload.Parts)
                            {
                                body = GetMessageBody(part);
                                decodedBody = DecodeBase64(body);
                                if (!string.IsNullOrEmpty(body))
                                    break;
                            }
                        }

                        msSQL = " select inbox_id  from crm_trn_tgmailinbox where inbox_id = '" + messageId + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count == 0)
                        {
                            if (DecodeFromBase64(encodedSubject.ToString()).IndexOf("New customer message on", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                msSQL = "select gmail_address from crm_smm_gmail_service where s_no ='" + s_no + "'";
                            string gmail_address = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = " select company_code from  adm_mst_tcompany limit 1";
                            string lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = "INSERT INTO crm_trn_tgmailinbox (" +
                                         "inbox_id, " +
                                        "from_id, " +
                                         "integrated_gmail, " +
                                        "cc, " +
                                        "bcc, " +
                                        "subject, " +
                                        "body, " +
                                        "attachement_flag, " +
                                        "read_flag, " +
                                        "shopify_enquiry, " +
                                        "company_code, " +
                                        "sent_date) " +
                                        "VALUES (" +
                                         "'" + messageId + "', " +
                                        "'" + from + "', " +
                                         "'" + gmail_address + "', " +
                                        "'" + cc + "', " +
                                        "'" + bcc + "', " +
                                        "'" + encodedSubject + "', " +
                                         "'" + body + "', " +
                                         "'Y', " +
                                         "'" + isUnread + "', " +
                                          "'Y', " +
                                         "'" + lscompany_code + "', " +
                                        "'" + formattedDateString + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 1)
                                {
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                            "***********" + msSQL +
                                            "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                            DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                }
                            }

                            // Process attachments
                            if (payload.Parts != null)
                            {
                                foreach (var part in payload.Parts)
                                {
                                    if (part.Filename != null && part.Body.AttachmentId != null)
                                    {
                                        var attachment = await service.Users.Messages.Attachments.Get(userId, messageId, part.Body.AttachmentId).ExecuteAsync();
                                        var data = Convert.FromBase64String(attachment.Data.Replace('-', '+').Replace('_', '/'));
                                        // Initialize a counter for generating unique IDs
                                        int uniqueIdCounter = 1;

                                        // Increment the counter each time you need a new unique ID
                                        int uniqueId = uniqueIdCounter++;
                                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                                        string lsfile_gid = msdocument_gid;
                                        string FileExtension = Path.GetExtension(part.Filename).ToLowerInvariant();
                                        lsfile_gid = lsfile_gid + FileExtension;
                                        string logFilePath = ConfigurationManager.AppSettings["filepath"];

                                        // Check if the directory exists and create it if it does not
                                        if (!Directory.Exists(logFilePath))
                                        {
                                            Directory.CreateDirectory(logFilePath);
                                        }
                                        string attachmentFilePath = SaveAttachmentToFile(service, "me", message.Id, part.Body.AttachmentId, lsfile_gid, ConfigurationManager.AppSettings["filepath"]);
                                        // Save attachment to file
                                        // File.WriteAllBytes(part.Filename, data);

                                        msSQL = "INSERT INTO crm_trn_tgmailinboxattachment (" +
                                                 "inbox_id, " +
                                                "original_filename, " +
                                              "modified_filename, " +
                                                "file_path) " +
                                                "VALUES (" +
                                                 "'" + messageId + "', " +
                                                "'" + part.Filename + "', " +
                                                "'" + lsfile_gid + "', " +
                                                "'" + ConfigurationManager.AppSettings["db_path"].ToString() + lsfile_gid + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        if (mnResult != 1)
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                            " * **********" + msSQL +
                                            "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                            DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                        }

                                    }
                                }
                            }
                        }
                        //objOdbcDataReader.Close();
                        dt_datatable.Dispose();
                    }

                    // Update total messages fetched
                    totalMessagesFetched += response.Messages.Count();
                    if (totalMessagesFetched >= 50)
                    {

                        //msSQL = "SELECT * FROM crm_smm_gmail_service WHERE s_no != '" + s_no + "'";
                        //dt_datatable = objdbconn.GetDataTable(msSQL);

                        //var getModuleList = new List<gmailmultiple_credentials>();
                        //if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                        //{
                        //    foreach (DataRow dt in dt_datatable.Rows)
                        //    {
                        //        getModuleList.Add(new gmailmultiple_credentials
                        //        {
                        //            client_id = dt["client_id"].ToString(),
                        //            client_secret = dt["client_secret"].ToString(),
                        //            refresh_token = dt["refresh_token"].ToString(),
                        //            gmail_address = dt["gmail_address"].ToString(),
                        //            s_no = dt["s_no"].ToString()
                        //        });
                        //    }
                        //}

                        //for (int i = 0; i < getModuleList.Count; i++)
                        //{
                        //    var service1 = await GetGmailService(getModuleList[i].client_id, getModuleList[i].client_secret, getModuleList[i].refresh_token);
                        //    listRequest = service1.Users.Messages.List(userId);
                        //    listRequest.Q = "in:inbox has:attachment";
                        //   // listRequest.Q = "in:inbox -has:attachment"; // Query to fetch emails from the inbox
                        //    listRequest.MaxResults = 50; // Limit to top 50 emails


                        //    do
                        //    {

                        //        // Execute the list request asynchronously
                        //        ListMessagesResponse response1 = await listRequest.ExecuteAsync();

                        //        // Check if messages were returned
                        //        if (response1.Messages != null && response1.Messages.Any())
                        //        {
                        //            foreach (var message in response1.Messages)
                        //            {
                        //                var messageDetails = await service1.Users.Messages.Get(userId, message.Id).ExecuteAsync();
                        //                var payload = messageDetails.Payload;
                        //                string messageId = message.Id;
                        //                string subject = payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
                        //                //  string from = payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                        //                string date = payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;
                        //                string body = string.Empty;
                        //                string decodedBody = string.Empty;
                        //                string encodedSubject = EncodeToBase64(subject);
                        //                string formattedDateString = string.Empty;
                        //                string from = null;
                        //                string cc = null;
                        //                string bcc = null;
                        //                bool isUnread = messageDetails.LabelIds.Contains("UNREAD");
                        //                //var fromHeader = payload.Headers.FirstOrDefault(h => h.Name == "From");
                        //                //if (fromHeader != null)
                        //                //{
                        //                //    // Extract the email address using a regular expression
                        //                //    var match = Regex.Match(fromHeader.Value, @"\<(.*?)\>");
                        //                //    if (match.Success)
                        //                //    {
                        //                //        from = match.Groups[1].Value;
                        //                //    }
                        //                //    else
                        //                //    {
                        //                //        // If the regular expression fails, fallback to using the entire From header value
                        //                //        from = fromHeader.Value;
                        //                //    }
                        //                //}
                        //                // string cc = payload.Headers.FirstOrDefault(h => h.Name == "CC")?.Value;

                        //                string ccccc = payload.Headers.FirstOrDefault(h => h.Name.Equals("CC", StringComparison.OrdinalIgnoreCase))?.Value;
                        //                string fromcv = payload.Headers.FirstOrDefault(h => h.Name.Equals("From", StringComparison.OrdinalIgnoreCase))?.Value;
                        //                string Bcc = payload.Headers.FirstOrDefault(h => h.Name.Equals("Bcc", StringComparison.OrdinalIgnoreCase))?.Value;
                        //                if (!string.IsNullOrEmpty(ccccc))
                        //                {
                        //                    // Use a regular expression to extract all email addresses
                        //                    var emailMatches = Regex.Matches(ccccc, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                        //                    var emailAddresses = emailMatches.Cast<Match>()
                        //                                                     .Select(match => match.Value)
                        //                                                     .ToList();

                        //                    // Join the extracted email addresses with a comma
                        //                    cc = string.Join(",", emailAddresses);
                        //                }
                        //                if (!string.IsNullOrEmpty(fromcv))
                        //                {
                        //                    // Use a regular expression to extract all email addresses
                        //                    var emailMatches = Regex.Matches(fromcv, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                        //                    var emailAddresses = emailMatches.Cast<Match>()
                        //                                                     .Select(match => match.Value)
                        //                                                     .ToList();

                        //                    // Join the extracted email addresses with a comma
                        //                    from = string.Join(",", emailAddresses);

                        //                }
                        //                if (!string.IsNullOrEmpty(Bcc))
                        //                {
                        //                    // Use a regular expression to extract all email addresses
                        //                    var emailMatches = Regex.Matches(Bcc, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                        //                    var emailAddresses = emailMatches.Cast<Match>()
                        //                                                     .Select(match => match.Value)
                        //                                                     .ToList();

                        //                    // Join the extracted email addresses with a comma
                        //                    bcc = string.Join(",", emailAddresses);
                        //                }
                        //                //string bcc = payload.Headers.FirstOrDefault(h => h.Name == "Bcc")?.Value;
                        //                string[] formats = {
                        //            // Existing formats
                        //            "ddd, dd MMM yyyy HH:mm:ss zzz",
                        //            "ddd, dd MMM yyyy HH:mm:ss zzzz",
                        //            "ddd, dd MMM yyyy HH:mm:ss zzz (zzz)",
                        //            "ddd, dd MMM yyyy HH:mm:ss zzz",
                        //            "ddd, dd MMM yyyy HH:mm:sszzz",
                        //            "ddd, dd MMM yyyy HH:mm:ss",
                        //            "dd MMM yyyy HH:mm:ss zzz",
                        //            "dd MMM yyyy HH:mm:sszzz",
                        //            "dd MMM yyyy HH:mm:ss",
                        //            "yyyy-MM-dd'T'HH:mm:ss'Z'",
                        //            "yyyy-MM-dd'T'HH:mm:sszzz",
                        //            "yyyy-MM-dd'T'HH:mm:ss.fffzzz",
                        //            "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                        //            "yyyy-MM-ddTHH:mm:sszzz",
                        //            "yyyy-MM-ddTHH:mm:ss.fffzzz",
                        //            "yyyy-MM-ddTHH:mm:ss.fff'Z'",
                        //            "yyyy-MM-dd HH:mm:sszzz",
                        //            "yyyy-MM-dd HH:mm:ss.fffzzz",
                        //            "yyyy-MM-dd HH:mm:ss.fff'Z'",
                        //            "ddd, dd MMM yyyy HH:mm:ss zzz '(UTC)'",
                        //            // India formats
                        //            "dd-MM-yyyy HH:mm:ss",
                        //            "dd/MM/yyyy HH:mm:ss",
                        //            "dd-MM-yyyy hh:mm:ss tt",
                        //            "dd/MM/yyyy hh:mm:ss tt",
                        //            "dd-MM-yyyy HH:mm:ss zzz",
                        //            "dd/MM/yyyy HH:mm:ss zzz",
                        //            // UK formats
                        //            "dd/MM/yyyy HH:mm:ss",
                        //            "dd/MM/yyyy hh:mm:ss tt",
                        //            "dd/MM/yyyy HH:mm:ss zzz",
                        //            "dd MMM yyyy HH:mm:ss",
                        //            "dd MMM yyyy hh:mm:ss tt",
                        //            "dd MMM yyyy HH:mm:ss zzz",
                        //            "ddd, dd MMM yyyy HH:mm:ss zzz",
                        //            // New format
                        //            "ddd, d MMM yyyy HH:mm:ss zzz"
                        //        };



                        //                //if (DateTimeOffset.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffset))
                        //                if (DateTimeOffset.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffset))

                        //                {
                        //                    formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        //                }
                        //                else
                        //                {
                        //                    try
                        //                    {
                        //                        // Manually process the date string to handle variations
                        //                        string[] parts = date.Split(' ');

                        //                        // Remove day of the week and optional parts like (UTC)
                        //                        if (parts.Length > 6)
                        //                        {
                        //                            parts = parts.Take(6).ToArray();
                        //                        }

                        //                        // Extract the date and time part
                        //                        string dateTimePart = parts.Length >= 5 ? string.Join(" ", parts.Skip(1).Take(4)) : string.Empty;

                        //                        // Extract the time zone offset part
                        //                        string timeZoneOffsetPart = parts.Length == 6 ? parts[5] : string.Empty;

                        //                        // Parse and format date and time
                        //                        if (DateTimeOffset.TryParseExact(
                        //                            dateTimePart + " " + timeZoneOffsetPart,
                        //                            "dd MMM yyyy HH:mm:ss zzz",
                        //                            CultureInfo.InvariantCulture,
                        //                            DateTimeStyles.None,
                        //                            out dateTimeOffset))
                        //                        {
                        //                            formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        //                        }
                        //                        else if (DateTimeOffset.TryParseExact(
                        //                            dateTimePart + " " + timeZoneOffsetPart,
                        //                            "dd MMM yyyy HH:mm:sszzz",
                        //                            CultureInfo.InvariantCulture,
                        //                            DateTimeStyles.None,
                        //                            out dateTimeOffset))
                        //                        {
                        //                            formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        //                        }
                        //                    }
                        //                    catch (FormatException ex)
                        //                    {
                        //                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                        //                                     "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                        //                                     " * **********" + ex.Message.ToString() +
                        //                                     "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                        //                                     DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        //                    }
                        //                }



                        //                // If the message has a text/plain part, use it as the message body
                        //                if (payload.MimeType == "text/plain" && !string.IsNullOrEmpty(payload.Body?.Data))
                        //                {
                        //                    body = payload.Body.Data;
                        //                    decodedBody = DecodeBase64(body);
                        //                }
                        //                // If the message has multipart parts, recursively find the text/plain part
                        //                else if (payload.Parts != null)
                        //                {
                        //                    foreach (var part in payload.Parts)
                        //                    {
                        //                        body = GetMessageBody(part);
                        //                        decodedBody = DecodeBase64(body);
                        //                        if (!string.IsNullOrEmpty(body))
                        //                            break;
                        //                    }
                        //                }

                        //                msSQL = " select inbox_id  from crm_trn_tgmailinbox where inbox_id = '" + messageId + "'";
                        //                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        //                if (objOdbcDataReader.HasRows != true)
                        //                {
                        //                    msSQL = "select gmail_address from crm_smm_gmail_service WHERE s_no = '" + getModuleList[i].s_no + "'";
                        //                    string gmail_address = objdbconn.GetExecuteScalar(msSQL);
                        //                    msSQL = " select company_code from  adm_mst_tcompany limit 1";
                        //                    string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                        //                    msSQL = "INSERT INTO crm_trn_tgmailinbox (" +
                        //                 "inbox_id, " +
                        //                "from_id, " +
                        //                 "integrated_gmail, " +
                        //                "cc, " +
                        //                "bcc, " +
                        //                "subject, " +
                        //                "body, " +
                        //                "attachement_flag, " +
                        //                "read_flag, " +
                        //                "company_code, " +
                        //                "sent_date) " +
                        //                "VALUES (" +
                        //                 "'" + messageId + "', " +
                        //                "'" + from + "', " +
                        //                 "'" + gmail_address + "', " +
                        //                "'" + cc + "', " +
                        //                "'" + bcc + "', " +
                        //                "'" + encodedSubject + "', " +
                        //                 "'" + body + "', " +
                        //                 "'Y', " +
                        //                 "'" + isUnread + "', " +
                        //                 "'" + lscompany_code + "', " +
                        //                "'" + formattedDateString + "')";
                        //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        //                    if (mnResult != 1)
                        //                    {
                        //                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                        //                                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                        //                                "***********" + msSQL +
                        //                                "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                        //                                DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        //                    }


                        //                    // Process attachments
                        //                    if (payload.Parts != null)
                        //                    {
                        //                        foreach (var part in payload.Parts)
                        //                        {
                        //                            if (part.Filename != null && part.Body.AttachmentId != null)
                        //                            {
                        //                                var attachment = await service1.Users.Messages.Attachments.Get(userId, messageId, part.Body.AttachmentId).ExecuteAsync();
                        //                                var data = Convert.FromBase64String(attachment.Data.Replace('-', '+').Replace('_', '/'));
                        //                                // Initialize a counter for generating unique IDs
                        //                                int uniqueIdCounter = 1;

                        //                                // Increment the counter each time you need a new unique ID
                        //                                int uniqueId = uniqueIdCounter++;
                        //                                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        //                                string lsfile_gid = msdocument_gid;
                        //                                string FileExtension = Path.GetExtension(part.Filename).ToLowerInvariant();
                        //                                lsfile_gid = lsfile_gid + FileExtension;
                        //                                string logFilePath = ConfigurationManager.AppSettings["filepath"];

                        //                                // Check if the directory exists and create it if it does not
                        //                                if (!Directory.Exists(logFilePath))
                        //                                {
                        //                                    Directory.CreateDirectory(logFilePath);
                        //                                }
                        //                                string attachmentFilePath = SaveAttachmentToFile(service1, "me", message.Id, part.Body.AttachmentId, lsfile_gid, ConfigurationManager.AppSettings["filepath"]);
                        //                                // Save attachment to file
                        //                                // File.WriteAllBytes(part.Filename, data);

                        //                                msSQL = "INSERT INTO crm_trn_tgmailinboxattachment (" +
                        //                                         "inbox_id, " +
                        //                                        "original_filename, " +
                        //                                      "modified_filename, " +
                        //                                        "file_path) " +
                        //                                        "VALUES (" +
                        //                                         "'" + messageId + "', " +
                        //                                        "'" + part.Filename + "', " +
                        //                                        "'" + lsfile_gid + "', " +
                        //                                        "'" + ConfigurationManager.AppSettings["db_path"].ToString() + lsfile_gid + "')";
                        //                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        //                                if (mnResult != 1)
                        //                                {
                        //                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                        //                                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                        //                                    " * **********" + msSQL +
                        //                                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                        //                                    DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        //                                }

                        //                            }
                        //                        }
                        //                    }
                        //                }
                        //                objOdbcDataReader.Close();
                        //            }

                        //            // Update total messages fetched
                        //            totalMessagesFetched += response.Messages.Count();
                        //            if (totalMessagesFetched >= 50)
                        //            {
                        //                break;
                        //            }
                        //            // Set the page token for the next request
                        //            listRequest.PageToken = response.NextPageToken;
                        //        }
                        //        else
                        //        {

                        //            break; // Break the loop if no more messages found
                        //        }
                        //    }
                        //    // while (!string.IsNullOrEmpty(listRequest.PageToken));
                        //    while (!string.IsNullOrEmpty(listRequest.PageToken) && totalMessagesFetched < 50);

                        //}
                        break;
                    }
                    // Set the page token for the next request
                    listRequest.PageToken = response.NextPageToken;
                }
                else
                {

                    break; // Break the loop if no more messages found
                }
            }
            // while (!string.IsNullOrEmpty(listRequest.PageToken));
            while (!string.IsNullOrEmpty(listRequest.PageToken) && totalMessagesFetched < 50);


        }
        static string SaveAttachmentToFile(GmailService service, string userId, string messageId, string attachmentId, string filename, string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
                // Console.WriteLine($"Directory created: {savePath}");
            }
            var attachment = service.Users.Messages.Attachments.Get(userId, messageId, attachmentId).Execute();
            var data = Convert.FromBase64String(attachment.Data.Replace('-', '+').Replace('_', '/'));

            // Combine the provided savePath with the attachment filename
            string filePath = Path.Combine(savePath, filename);

            // Save attachment to file
            System.IO.File.WriteAllBytes(filePath, data);

            // Console.WriteLine($"Attachment saved to: {filePath}");

            // Return the path where the attachment is saved
            return filePath;
        }
        public void DaGetGmailComments(string inbox_id, MdlGmailCampaign values, string user_gid)
        {
            try
            {

                msSQL = " select  s_no, comments, CONCAT(b.user_firstname,' ',b.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date " +
                    " from crm_smm_tgmailcomment a " +
                    " left join adm_mst_tuser b on b.user_gid=a.created_by where a.inbox_id ='" + inbox_id + "' order by a.s_no desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gmailcomments_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gmailcomments_list
                        {
                            s_no = dt["s_no"].ToString(),
                            comments = dt["comments"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.gmailcomments_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Group Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostGmailComments(string user_gid, gmailcomments_list values)
        {
            try
            {
                msSQL = "insert into crm_smm_tgmailcomment ( " +
                    "comments," +
                    "inbox_id," +
                  " created_by, " +
                  " created_date)" +
                    " values(" +
                     "'" + values.comments + "'," +
                     "'" + values.inbox_id + "'," +
                     "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Comment Added Successfully";
                }
                else
                {

                    values.status = false;
                    values.message = "Error While Adding Comment";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Comment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdatedGmailComments(string user_gid, gmailcomments_list values)
        {
            try
            {
                msSQL = " update  crm_smm_tgmailcomment  set " +
               " comments = '" + values.comments + "'," +
              " updated_by = '" + user_gid + "'," +
              " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.s_no + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Comment Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Comment !!";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product Group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DadeleteGmailComments(string s_no, gmailcomments_list values)
        {

            try
            {
                msSQL = "  delete from crm_smm_tgmailcomment where s_no='" + s_no + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Comment Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Comment!!";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Comment!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Comment " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public result DaGmailAddaslead(mdlAddaslead values, string user_gid)
        {


            result objresult = new result();
            try
            {


                msSQL = "SELECT leadbank_gid FROM crm_trn_gmail WHERE to_mailaddress = '" + values.email_address + "' AND(leadbank_gid IS not NULL and leadbank_gid != '')";
                string leadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                if (leadbank_gid == null)
                {

                    msSQL = "select source_gid from crm_mst_tsource where source_name = 'Mail'";
                    string source_gid = objdbconn.GetExecuteScalar(msSQL);

                    if (string.IsNullOrEmpty(source_gid))
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("BSEM");
                        msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='BSEM' order by finyear desc limit 0,1 ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);

                        string lssource_code = "SCM" + "000" + lsCode;

                        msSQL = " insert into crm_mst_tsource(" +
                                " source_gid," +
                                " source_code," +
                                " source_name," +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" + lssource_code + "'," +
                                "'Mail'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            lssource_gid = msGetGid;
                        }
                    }
                    else
                    {
                        lssource_gid = source_gid;
                    }


                    msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                    string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customer_type + "'";
                    string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

                    msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                    msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                    msSQL = " INSERT INTO crm_trn_tleadbank(" +
                            " leadbank_gid," +
                            " source_gid," +
                            " leadbank_id," +
                            " leadbank_name," +
                            " status," +
                            " approval_flag, " +
                            " lead_status," +
                            " leadbank_code," +
                            " customer_type," +
                            " customertype_gid," +
                            " created_by," +
                            " main_branch," +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid1 + "'," +
                            " '" + lssource_gid + "'," +
                            " '" + msGetGid + "'," +
                            " '" + values.displayName.Replace("\"", "\\\"").Replace("'", "\\\'") + "'," +
                            " 'y'," +
                            " 'Approved'," +
                            " 'Not Assigned'," +
                            " 'H.Q'," +
                            " '" + lscustomer_type + "'," +
                            " '" + values.customer_type + "'," +
                            " '" + lsemployee_gid + "'," +
                            " 'Y'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    string msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                    if (msGetGid2 == "E")
                    {
                        objresult.status = false;
                        objresult.message = "Create sequence code BLCC for Lead Bank";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }

                    msSQL = "update crm_smm_mailmanagement set leadbank_gid='" + msGetGid1 + "' where to_mail='" + values.email_address + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                        " (leadbankcontact_gid," +
                        " leadbank_gid," +
                        " leadbankcontact_name," +
                        " email," +
                        " created_date," +
                        " created_by," +
                        " leadbankbranch_name, " +
                        " main_contact)" +
                        " values( " +
                        " '" + msGetGid2 + "'," +
                        " '" + msGetGid1 + "'," +
                        " '" + values.displayName.Replace("\"", "\\\"").Replace("'", "\\\'") + "'," +
                        " '" + values.email_address + "'," +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " '" + lsemployee_gid + "'," +
                        " 'H.Q'," +
                        " 'y'" + ")";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    if (mnResult == 1)
                    {
                        objresult.status = true;
                        objresult.message = "Contact created successfully!";
                    }
                    else
                    {
                        objresult.message = "Error occured while adding contact!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }

                }
                else
                {
                    msSQL = "update crm_smm_mailmanagement set leadbank_gid='" + leadbank_gid + "'  where to_mail='" + values.email_address + "';";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        objresult.status = true;
                        objresult.message = "Contact Already Created!";
                    }
                }

            }
            catch (Exception ex)
            {
                objresult.message = "Error While Updating MailEvent Delivery";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            return objresult;

        }
        static string GetMessageBody(MessagePart payload)
        {
            string body = string.Empty;

            // If the message has a text/plain part, use it as the message body
            if (payload.MimeType == "text/html" && !string.IsNullOrEmpty(payload.Body?.Data))
            {
                body = payload.Body.Data;
            }
            // If the message has multipart parts, recursively find the text/plain part
            else if (payload.Parts != null)
            {
                foreach (var part in payload.Parts)
                {
                    body = GetMessageBody(part);
                    if (!string.IsNullOrEmpty(body))
                        break;
                }
            }

            return body;
        }
        // Method to decode base64 string
        private static string EncodeToBase64(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        static string DecodeBase64(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String.Replace('-', '+').Replace('_', '/'));
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public void DaGmailView(string gmail_gid, MdlGmailCampaign values)

        {
            try
            {

                msSQL = "select a.gmail_gid,a.to_mailaddress,a.created_by,date_format(a.created_date,'%d-%m-%Y')as dates," +
                    " case when a.template_gid is not null then b.template_subject else a.mail_subject end as subject," +
                    "a.file_path,case when a.template_gid is not null then b.template_body else a.mail_body end as body," +
                    "direction,b.template_name,c.customer_type from crm_trn_gmail a left join crm_trn_tgmailtemplate b on" +
                    " a.template_gid =b.template_gid  left join  crm_trn_tleadbank c on c.leadbank_gid=a.leadbank_gid" +
                    " where a.gmail_gid='" + gmail_gid + "'  order by a.created_date desc;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailtemplatesendsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailtemplatesendsummary_list
                        {
                            gmail_gid = dt["gmail_gid"].ToString(),
                            to_mailaddress = dt["to_mailaddress"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            dates = dt["dates"].ToString(),
                            direction = dt["direction"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            subject = dt["subject"].ToString(),
                            body = dt["body"].ToString(),
                            file_path = dt["file_path"].ToString(),



                        });
                        values.gmailtemplatesendsummary_list = getmodulelist;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Mail Send Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }


        public void DaGet360Gmailsummary(string leadbank_gid, MdlGmailCampaign values)

        {
            try
            {

                msSQL = "select a.gmail_gid,a.to_mailaddress,a.leadbank_gid,a.created_by,date_format(a.created_date,'%d-%m-%Y')as dates," +
                    " case when a.template_gid is not null then b.template_subject else a.mail_subject end as subject," +
                    "case when a.template_gid is not null then b.template_body else a.mail_body end as body," +
                    "b.template_name from crm_trn_gmail a left join crm_trn_tgmailtemplate b on" +
                    " a.template_gid =b.template_gid  left join  crm_trn_tleadbank c on c.leadbank_gid=a.leadbank_gid" +
                    " where a.leadbank_gid='" + leadbank_gid + "'  order by a.created_date desc;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailtemplatesendsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailtemplatesendsummary_list
                        {
                            gmail_gid = dt["gmail_gid"].ToString(),
                            to_mailaddress = dt["to_mailaddress"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            dates = dt["dates"].ToString(),
                            //direction = dt["direction"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            //customer_type = dt["customer_type"].ToString(),
                            subject = dt["subject"].ToString(),
                            body = dt["body"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            //file_path = dt["file_path"].ToString(),



                        });
                        values.gmailtemplatesendsummary_list = getmodulelist;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Mail Send Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaGetLeaddropdown(string user_gid, MdlGmailCampaign values)
        {
            msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid='" + user_gid + "'";
            string gmail_address = objdbconn.GetExecuteScalar(msSQL1);
            msSQL1 = "select default_template from crm_smm_gmail_service where gmail_address = '" + gmail_address + "'";
            string default_template = objdbconn.GetExecuteScalar(msSQL1);

            msSQL = "select a.leadbank_gid,a.leadbank_name,b.leadbankbranch_name,b.leadbankcontact_name,b.address1,b.address2,b.city,b.state,b.pincode,b.mobile,b.email,c.region_name,d.source_name " +
                " from crm_trn_tleadbank a left join  crm_trn_tleadbankcontact  b on a.leadbank_gid=b.leadbank_gid " +
                "left join crm_mst_tregion c on a.leadbank_region=c.region_gid " +
                "left join crm_mst_tsource d on a.source_gid=d.source_gid where b.main_contact ='Y';";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetLeaddropdown_lists>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetLeaddropdown_lists
                    {
                        name = dt["leadbank_name"].ToString(),
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        email = dt["email"].ToString(),
                        gmail_address = gmail_address,
                        default_template = default_template,
                    });
                    values.GetLeaddropdown_lists = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        //code by snehith
        public void DaGetInboxCustomerDetails(string email_id, MdlGmailCampaign values, string user_gid)
        {
            try
            {

                msSQL = $@" SELECT 
                        (SELECT COUNT(salesorder_gid) 
                         FROM smr_trn_tsalesorder a
                         LEFT JOIN crm_mst_tcustomercontact b ON a.customer_gid = b.customer_gid 
                         WHERE b.email = '{email_id}') AS sales_order_count,

                        (SELECT COUNT(invoice_gid) 
                         FROM rbl_trn_tinvoice a
                         LEFT JOIN crm_mst_tcustomercontact b ON a.customer_gid = b.customer_gid 
                         WHERE b.email = '{email_id}') AS invoice_count,

                        (SELECT customercontact_name 
                         FROM crm_mst_tcustomercontact 
                         WHERE email = '{email_id}') AS customercontact_name,

                        (SELECT email 
                         FROM crm_mst_tcustomercontact 
                         WHERE email = '{email_id}') AS email,

                        (SELECT address1 
                         FROM crm_mst_tcustomercontact 
                         WHERE email = '{email_id}') AS address1,
                         (SELECT customer_name 
                    FROM crm_mst_tcustomer 
                    WHERE customer_gid = (
                        SELECT customer_gid 
                        FROM crm_mst_tcustomercontact 
                        WHERE email = '{email_id}'
                    )) as customer_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<inboxcustomer_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new inboxcustomer_list
                        {

                            sales_order_count = dt["sales_order_count"].ToString(),
                            invoice_count = dt["invoice_count"].ToString(),
                            customercontact_name = dt["customercontact_name"].ToString(),
                            email = dt["email"].ToString(),
                            address1 = dt["address1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                        });
                        values.inboxcustomer_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Group Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCustomerAssignedlist(string inbox_id, MdlGmailCampaign values)
        {
            try
            {
                msSQL = $@"SELECT a.customer_gid, a.customer_name, b.customercontact_name, b.email,b.address1,b.city,c.country_name
                            FROM crm_mst_tcustomer a
                            LEFT JOIN crm_mst_tcustomercontact b ON b.customer_gid = a.customer_gid
                            LEFT JOIN adm_mst_tcountry c ON c.country_gid = b.country_gid
                            WHERE a.customer_gid  Not IN (
                                SELECT customer_gid 
                                FROM crm_trn_tgmail2customer 
                                WHERE inbox_id = '{inbox_id}'
                            )   group by a.customer_gid order by a.customer_name  asc;
                            ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getgmailcustomerassignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getgmailcustomerassignedlist
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customercontact_name = dt["customercontact_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            email = dt["email"].ToString(),
                            address1 = dt["address1"].ToString(),
                            city = dt["city"].ToString(),
                            country_name = dt["country_name"].ToString(),

                        });
                        values.Getgmailcustomerassignedlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Assigned List Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCustomerUnAssignedlist(string inbox_id, MdlGmailCampaign values)
        {
            try
            {
                msSQL = $@"SELECT a.customer_gid, a.customer_name, b.customercontact_name, b.email,b.address1,b.city,c.country_name
                            FROM crm_mst_tcustomer a
                            LEFT JOIN crm_mst_tcustomercontact b ON b.customer_gid = a.customer_gid
                            LEFT JOIN adm_mst_tcountry c ON c.country_gid = b.country_gid
                            WHERE a.customer_gid  IN (
                                SELECT customer_gid 
                                FROM crm_trn_tgmail2customer 
                                WHERE inbox_id = '{inbox_id}'
                            )   group by a.customer_gid order by a.customer_name  asc;
                            ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getgmailcustomerunassignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getgmailcustomerunassignedlist
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customercontact_name = dt["customercontact_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            email = dt["email"].ToString(),
                            address1 = dt["address1"].ToString(),
                            city = dt["city"].ToString(),
                            country_name = dt["country_name"].ToString(),

                        });
                        values.Getgmailcustomerunassignedlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Assigned List Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaTagCustomertoGmail(string user_gid, tagcustomertogmail values)
        {
            try
            {

                for (int i = 0; i < values.Getgmailcustomerassignedlist.ToArray().Length; i++)
                {
                    msSQL = " select inbox_id  from crm_trn_tgmail2customer where inbox_id = '" + values.inbox_id + "' and customer_gid = '" + values.Getgmailcustomerassignedlist[i].customer_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows != true)
                    {
                        String msSQL = "INSERT INTO crm_trn_tgmail2customer (" +
                                       "inbox_id, " +
                                       "customer_gid, " +
                                       "created_date, " +
                                       "created_by) " +
                                       "VALUES (" +
                                       "'" + values.inbox_id + "', " +
                                       "'" + values.Getgmailcustomerassignedlist[i].customer_gid + "', " +
                                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                       "'" + user_gid + "')";

                        int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 1)
                        {
                            objcmnfunctions.LogForAudit(
                                "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                "***********" + msSQL +
                                "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Customer Tag to Gmail.";
                        }
                    }
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Inbox Mail Tag to Customer Successfully !!";


                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred While Customer Tag to Gmail.";
                    objcmnfunctions.LogForAudit(
                       "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                       "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                       "***********" + msSQL +
                       "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                          DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error Occurred While Customer Tag to Gmail.";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaUnTagCustomertoGmail(string user_gid, untagcustomertogmail values)
        {
            try
            {

                for (int i = 0; i < values.Getgmailcustomerunassignedlist.ToArray().Length; i++)
                {

                    string msSQL = "DELETE FROM crm_trn_tgmail2customer " +
                    "WHERE inbox_id = '" + values.inbox_id + "' " +
                    "AND customer_gid = '" + values.Getgmailcustomerunassignedlist[i].customer_gid + "'";

                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 1)
                    {
                        objcmnfunctions.LogForAudit(
                            "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                            "***********" + msSQL +
                            "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                            DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Customer Untag to Gmail.";
                    }
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Inbox Mail Untag from Customer Successfully !!";


                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred While Customer Untag to Gmail.";
                    objcmnfunctions.LogForAudit(
                       "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                       "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                       "***********" + msSQL +
                       "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                          DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error Occurred While Customer Untag to Gmail.";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaGetEmailId(MdlGmailCampaign values)
        {


            msSQL = "select gmail_address from crm_smm_gmail_service  limit 1";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetEmailId_lists>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEmailId_lists
                    {
                        gmail_address = dt["gmail_address"].ToString(),
                    });
                    values.GetEmailId_lists = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }


        public void Dashopifyenquiry(MdlGmailCampaign values)
        {


            try
            {
                msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date, DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time, cc, subject, body, attachement_flag FROM crm_trn_tgmailinbox where inbox_status is Null ORDER BY s_no DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                values.gmailapiinboxsummary_list = dt_datatable.AsEnumerable().AsParallel()
                    .Select(row =>
                    {
                        var decodedSubject = DecodeFromBase64(row["subject"].ToString());
                        var attachementFlag = row["attachement_flag"].ToString();
                        //if (decodedSubject.IndexOf("New customer message on", StringComparison.OrdinalIgnoreCase) >= 0)
                        //{
                        //    msSQL = "update crm_trn_tgmailinbox set shopify_enquiry = 'Y' where s_no='" + row["s_no"].ToString() + "'";
                        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        //}

                        return new gmailapiinboxsummary_list
                        {
                            s_no = row["s_no"].ToString(),
                            subject = decodedSubject,
                        };
                    })
                    .ToList();

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void Dashopifyenquirysummary(MdlGmailCampaign values, string user_gid)
        {
            string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
            string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
            gmailconfiguration getgmailcredentials = gmailcrendentials(integrated_gmail);
            try
            {
                msSQL = " SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date," +
                        " DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time, cc, subject," +
                        " body, attachement_flag FROM crm_trn_tgmailinbox where shopify_enquiry = 'Y' and inbox_status is Null ORDER BY s_no DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getmodulelist = new List<gmailapiinboxsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailapiinboxsummary_list
                        {
                            s_no = dt["s_no"].ToString(),
                            inbox_id = dt["inbox_id"].ToString(),
                            from_id = dt["from_id"].ToString(),
                            sent_date = dt["sent_date"].ToString(),
                            sent_time = dt["sent_time"].ToString(),
                            cc = dt["cc"].ToString(),
                            subject = DecodeFromBase64(dt["subject"].ToString()),
                            body = dt["body"].ToString(),
                            attachement_flag = dt["attachement_flag"].ToString(),


                        });
                        values.gmailapiinboxsummary_list = getmodulelist;
                    }
                }

                values.gmail_address = getgmailcredentials.gmail_address;
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaGetshopifyenquiryviewSummary(string s_no, gmailapiinboxsummary_list values)
        {
            try
            {
                msSQL = " SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date," +
                        " DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time, cc, subject," +
                        " body, attachement_flag FROM crm_trn_tgmailinbox where s_no = '" + s_no + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.s_no = objOdbcDataReader["s_no"].ToString();
                    values.inbox_id = objOdbcDataReader["inbox_id"].ToString();
                    values.from_id = objOdbcDataReader["from_id"].ToString();
                    values.sent_date = objOdbcDataReader["sent_date"].ToString();
                    values.sent_time = objOdbcDataReader["sent_time"].ToString();
                    values.subject = DecodeFromBase64(objOdbcDataReader["subject"].ToString());
                    values.body = DecodeFromBase64(objOdbcDataReader["body"].ToString());
                    values.attachement_flag = objOdbcDataReader["attachement_flag"].ToString();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank view summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public async Task DaGmailAPIDirectly(MdlGmailCampaign values, string user_gid)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid='" + user_gid + "'";
                string gmail_address = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL = "select * from crm_smm_gmail_service WHERE gmail_address = '" + gmail_address + "' LIMIT 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    getgmailcredentials.s_no = objOdbcDataReader["s_no"].ToString();
                }
                objOdbcDataReader.Close();

                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;
                string s_no = getgmailcredentials.s_no;
                var mailDetailsList = new List<MailDetails>();
                var service = await GetGmailService(clientId, clientSecret, refreshToken);
                var listRequest = service.Users.Messages.List("me");
                listRequest.Q = "in:inbox";  // Modify query as per your requirement
                listRequest.MaxResults = 50;
                int totalMessagesFetched = 0;
                const int maxRetryAttempts = 3;
               // int retryCount = 0;
                // Initialize Gmail service asynchronously
                do
                {
                    try
                    {

                        // Attempt to execute the list request
                        ListMessagesResponse response = await listRequest.ExecuteAsync();

                        // If response has messages, proceed
                        if (response.Messages != null && response.Messages.Any())
                        {
                            foreach (var message in response.Messages)
                            {
                                // Get detailed message
                                var messageDetails = await service.Users.Messages.Get("me", message.Id).ExecuteAsync();
                                var payload = messageDetails.Payload;

                                // Extract headers
                                var mailDetails = new MailDetails
                                {
                                    MessageId = message.Id,
                                    Subject = payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value,
                                    From = ExtractEmailAddresses(payload, "From"),
                                    Cc = ExtractEmailAddresses(payload, "Cc"),   // Extract Cc
                                    Bcc = ExtractEmailAddresses(payload, "Bcc"), // Extract Bcc
                                    Date = payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value,
                                    Body = ExtractBody(payload),
                                    isUnread = messageDetails.LabelIds.Contains("UNREAD"),
                                    Attachments = new List<AttachmentDetails>()
                                };

                                //if (payload.MimeType == "text/html" && !string.IsNullOrEmpty(payload.Body?.Data))
                                //{
                                //   // var decodedBytes = Convert.FromBase64String(payload.Body.Data.Replace('-', '+').Replace('_', '/'));
                                //    mailDetails.Body = payload.Body?.Data;
                                //}
                                // Extract attachment details
                                if (payload.Parts != null)
                                {
                                    foreach (var part in payload.Parts)
                                    {
                                        if (!string.IsNullOrEmpty(part.Filename) && !string.IsNullOrEmpty(part.Body.AttachmentId))
                                        {
                                            var attachment = await service.Users.Messages.Attachments.Get("me", message.Id, part.Body.AttachmentId).ExecuteAsync();
                                            var attachmentDetails = new AttachmentDetails
                                            {
                                                FileName = part.Filename,
                                                Base64Url = attachment.Data.Replace('-', '+').Replace('_', '/')
                                            };
                                            mailDetails.Attachments.Add(attachmentDetails);
                                        }
                                    }
                                }

                                mailDetailsList.Add(mailDetails);
                            }

                            totalMessagesFetched += response.Messages.Count;
                            listRequest.PageToken = response.NextPageToken;
                            values.gmailapiinboxsummary_lists = mailDetailsList.Select(mailDetails => {
                                DateTimeOffset dateTimeOffset;
                                string sentDate = "";
                                string sentTime = "";

                                // Attempt to parse the date using multiple formats
                                string[] formats = {
                                        // RFC 1123
                                        "ddd, dd MMM yyyy HH:mm:ss zzz",
                                        "ddd, dd MMM yyyy HH:mm:ss zzzz",
                                        "ddd, dd MMM yyyy HH:mm:ss zzz (zzz)",
                                        "ddd, dd MMM yyyy HH:mm:sszzz",
                                        "ddd, dd MMM yyyy HH:mm:ss",
    
                                        // Common date formats
                                        "dd MMM yyyy HH:mm:ss zzz",
                                        "dd MMM yyyy HH:mm:sszzz",
                                        "dd MMM yyyy HH:mm:ss",
    
                                        // ISO 8601 formats
                                        "yyyy-MM-dd'T'HH:mm:ss'Z'",
                                        "yyyy-MM-dd'T'HH:mm:sszzz",
                                        "yyyy-MM-dd'T'HH:mm:ss.fffzzz",
                                        "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                                        "yyyy-MM-ddTHH:mm:sszzz",
                                        "yyyy-MM-ddTHH:mm:ss.fffzzz",
                                        "yyyy-MM-ddTHH:mm:ss.fff'Z'",
    
                                        // General date and time formats
                                        "yyyy-MM-dd HH:mm:sszzz",
                                        "yyyy-MM-dd HH:mm:ss.fffzzz",
                                        "yyyy-MM-dd HH:mm:ss.fff'Z'",
    
                                        // UTC formats
                                        "ddd, dd MMM yyyy HH:mm:ss zzz '(UTC)'",
                                        "ddd, dd MMM yyyy HH:mm:ss '(UTC)'",
    
                                        // Hyphen and slash separated formats
                                        "dd-MM-yyyy HH:mm:ss",
                                        "dd/MM/yyyy HH:mm:ss",
                                        "dd-MM-yyyy hh:mm:ss tt",
                                        "dd/MM/yyyy hh:mm:ss tt",
    
                                        // Timezone variants
                                        "dd-MM-yyyy HH:mm:ss zzz",
                                        "dd/MM/yyyy HH:mm:ss zzz",
                                        "dd/MM/yyyy HH:mm:ss",
                                        "dd/MM/yyyy hh:mm:ss tt",
    
                                        // 12-hour formats with AM/PM
                                        "dd MMM yyyy HH:mm:ss",
                                        "dd MMM yyyy hh:mm:ss tt",
                                        "ddd, d MMM yyyy HH:mm:ss zzz",
    
                                        // Including time zones in parentheses
                                        "ddd, dd MMM yyyy HH:mm:ss zzz '(IST)'", // for Indian Standard Time
                                        "ddd, dd MMM yyyy HH:mm:ss zzz '(UTC)'", // for UTC
                                        "ddd, dd MMM yyyy HH:mm:ss zzz '(GMT)'", // for Greenwich Mean Time
                                        "ddd, dd MMM yyyy HH:mm:ss zzz '(PST)'", // for Pacific Standard Time
                                        "ddd, dd MMM yyyy HH:mm:ss zzz '(CST)'", // for Central Standard Time
                                        "ddd, dd MMM yyyy HH:mm:ss zzz '(EST)'",  // for Eastern Standard Time
                                        "ddd, dd MMM yyyy HH:mm:ss zzz '(CDT)'"
                                    };


                                // Try parsing the date from mailDetails.Date
                                if (DateTimeOffset.TryParseExact(mailDetails.Date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeOffset))
                                {
                                    // Format date as "Sep 23, 2024 12:20 PM"
                                    sentDate = dateTimeOffset.ToString("MMM dd, yyyy hh:mm tt", CultureInfo.InvariantCulture);

                                    // Format time as "12:20 PM"
                                    sentTime = dateTimeOffset.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    // Handle the case where the date could not be parsed
                                    // Log or manage this scenario appropriately
                                    sentDate = mailDetails.Date;
                                    sentTime = mailDetails.Date;
                                }

                                return new gmailapiinboxsummary_lists
                                {
                                    inbox_id = mailDetails.MessageId,
                                    integrated_gmail = gmail_address,
                                    from_id = mailDetails.From,
                                    cc = mailDetails.Cc,
                                    bcc = mailDetails.Bcc,
                                    sent_date = sentDate,    // Formatted date like "Sep 23, 2024 12:20 PM"
                                    sent_time = sentTime,    // Formatted time like "12:20 PM"
                                    subject = mailDetails.Subject,
                                    body = mailDetails.Body,
                                    read_flag = mailDetails.isUnread,
                                    Attachments = mailDetails.Attachments.Select(attachment => new gmailapiinboxatatchement_lists
                                    {
                                        inbox_id = mailDetails.MessageId,
                                        original_filename = attachment.FileName,
                                        file_path = attachment.Base64Url
                                    }).ToList() // Add attachments to each mail
                                };
                            }).ToList();

                        }
                        else
                        {
                            values.gmailapiinboxsummary_lists = new List<gmailapiinboxsummary_lists>();
                        } 

                       // retryCount = 0; // Reset retry count on success
                    }
                    catch (Exception ex)
                    {
                       // retryCount++;

                        // Log exception details
                        objcmnfunctions.LogForAudit(
                            "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                            "***********" + ex.ToString(),
                            "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".txt");
                    }
                }
                while (!string.IsNullOrEmpty(listRequest.PageToken) && totalMessagesFetched < 50);

                // Retrieve Gmail API details
                //var mailDetailsList = ListAllMessages(service, "me", "inbox").Result;

                // Map Gmail API details to MdlGmailCampaign

            }
            catch (Exception ex)
            {
                // Log the exception details
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    "***********" + ex.ToString() +
                    "***********" + "*****Query****" + msSQL +
                    "*******Apiref********",
                    "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".txt"
                );
            }
        }

        string ExtractBody(MessagePart payload)
        {
            // If the body is directly available in the payload
            if (payload.MimeType == "text/html")
            {
                return payload.Body?.Data;
            }

            // If the body is nested within parts
            if (payload.Parts != null && payload.Parts.Any())
            {
                foreach (var part in payload.Parts)
                {
                    // Check if it's the HTML part
                    if (part.MimeType == "text/html")
                    {
                        return part.Body?.Data;
                    }

                    // Handle nested parts recursively
                    if (part.Parts != null && part.Parts.Any())
                    {
                        var nestedBody = ExtractBody(part);
                        if (!string.IsNullOrEmpty(nestedBody))
                        {
                            return nestedBody;
                        }
                    }
                }
            }

            // Default case if no HTML body found
            return string.Empty;
        }


        // Method to decode Base64 string
        string DecodeBase64String(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            var decodedBytes = Convert.FromBase64String(base64String.Replace('-', '+').Replace('_', '/'));
            return Encoding.UTF8.GetString(decodedBytes);
        }
        public async Task<List<MailDetails>> ListAllMessages(GmailService service, string userId, string label)
        {
            var mailDetailsList = new List<MailDetails>();
            var listRequest = service.Users.Messages.List(userId);
            listRequest.Q = "in:inbox -has:attachment";  // Modify query as per your requirement
            listRequest.MaxResults = 50;
            int totalMessagesFetched = 0;
            const int maxRetryAttempts = 3;
            int retryCount = 0;

            do
            {
                try
                {
                    // Attempt to execute the list request
                    ListMessagesResponse response = await listRequest.ExecuteAsync();

                    // If response has messages, proceed
                    if (response.Messages != null && response.Messages.Any())
                    {
                        foreach (var message in response.Messages)
                        {
                            // Get detailed message
                            var messageDetails = await service.Users.Messages.Get(userId, message.Id).ExecuteAsync();
                            var payload = messageDetails.Payload;

                            // Extract headers
                            var mailDetails = new MailDetails
                            {
                                MessageId = message.Id,
                                Subject = payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value,
                                From = ExtractEmailAddresses(payload, "From"),
                                Date = payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value,
                                Body = string.Empty,
                                Attachments = new List<AttachmentDetails>()
                            };

                            // Extract attachment details
                            if (payload.Parts != null)
                            {
                                foreach (var part in payload.Parts)
                                {
                                    if (!string.IsNullOrEmpty(part.Filename) && !string.IsNullOrEmpty(part.Body.AttachmentId))
                                    {
                                        var attachment = await service.Users.Messages.Attachments.Get(userId, message.Id, part.Body.AttachmentId).ExecuteAsync();
                                        var attachmentDetails = new AttachmentDetails
                                        {
                                            FileName = part.Filename,
                                            Base64Url = attachment.Data.Replace('-', '+').Replace('_', '/')
                                        };
                                        mailDetails.Attachments.Add(attachmentDetails);
                                    }
                                }
                            }

                            mailDetailsList.Add(mailDetails);
                        }

                        totalMessagesFetched += response.Messages.Count;
                        listRequest.PageToken = response.NextPageToken;
                    }
                    else
                    {
                        break; // No more messages found
                    }

                    retryCount = 0; // Reset retry count on success
                }
                catch (Exception ex)
                {
                    retryCount++;

                    // Log exception details
                    objcmnfunctions.LogForAudit(
                        "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        "***********" + ex.ToString(),
                        "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".txt");

                    // If retry attempts are exhausted, break out of the loop
                    if (retryCount >= maxRetryAttempts)
                    {
                        break;
                    }

                    // Optional: Add delay before retrying to avoid hitting rate limits
                    await Task.Delay(2000);
                }
            }
            while (!string.IsNullOrEmpty(listRequest.PageToken) && totalMessagesFetched < 50);

            return mailDetailsList;
        }

        private static string ExtractEmailAddresses(MessagePart payload, string headerName)
        {
            var headerValue = payload.Headers.FirstOrDefault(h => h.Name.Equals(headerName, StringComparison.OrdinalIgnoreCase))?.Value;

            if (!string.IsNullOrEmpty(headerValue))
            {
                var emailMatches = Regex.Matches(headerValue, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

                var emailAddresses = emailMatches.Cast<Match>()
                                                 .Select(match => match.Value)
                                                 .ToList();

                return string.Join(",", emailAddresses);
            }

            return string.Empty;
        }

        public async Task DaGmailInboxStatusUpdateBack(replymail_list values,string user_gid)
        {
            try
            {

                // Retrieve Gmail API credentials from database
                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid='" + user_gid + "'";
                string gmail_address = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL = "select * from crm_smm_gmail_service WHERE gmail_address = '" + gmail_address + "' LIMIT 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    getgmailcredentials.s_no = objOdbcDataReader["s_no"].ToString();
                }
                objOdbcDataReader.Close();

                // Initialize Gmail service
                var gmailService = await GetGmailService(getgmailcredentials.client_id, getgmailcredentials.client_secret, getgmailcredentials.refresh_token);


                try
                {
                    // Fetch the message details to update read status
                    var messageDetails = await gmailService.Users.Messages.Get("me", values.inbox_id).ExecuteAsync();
                    var message = messageDetails.Payload;

                    // Set read_flag to true by default
                    bool isUnread = true;
                    // Update read status by modifying labels
                    var modifyRequest = new Google.Apis.Gmail.v1.Data.ModifyMessageRequest
                    {
                        RemoveLabelIds = isUnread ? new List<string> { "UNREAD" } : new List<string>(),
                        AddLabelIds = !isUnread ? new List<string> { "UNREAD" } : new List<string>()
                    };

                    // Execute the modify request
                    var modifyResponse = await gmailService.Users.Messages.Modify(modifyRequest, "me", values.inbox_id).ExecuteAsync();

                    // Check if the read status update was successful
                    if ((isUnread && !modifyResponse.LabelIds.Contains("UNREAD")) || (!isUnread && modifyResponse.LabelIds.Contains("UNREAD")))
                    {
                        // Update your database with the read status if necessary
                        //msSQL = "UPDATE crm_trn_tgmailinbox SET read_flag = 'False' WHERE inbox_id = '" + values.inbox_id + "'";
                        //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        //if (mnResult != 1)
                        //{
                        //    objcmnfunctions.LogForAudit($"Error: Failed to update read status of email {values.inbox_id}.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        //}

                    }
                    else
                    {

                        objcmnfunctions.LogForAudit($"Error: Failed to update read status of email {values.inbox_id}.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                catch (Exception ex)
                {

                    objcmnfunctions.LogForAudit($"Error updating read status of email {values.inbox_id}: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error occurred while processing emails.";
                objcmnfunctions.LogForAudit($"Error in DaGmailInboxStatusUpdate: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task DaPostcreateAppointments(string user_gid, appointmentcreations values)
        {
            try
            {
                gmailconfiguration getgmailcredentials = new gmailconfiguration();
                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid='" + user_gid + "'";
                string gmail_address = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL = "select * from crm_smm_gmail_service WHERE gmail_address = '" + gmail_address + "' LIMIT 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    getgmailcredentials.s_no = objOdbcDataReader["s_no"].ToString();
                }
                objOdbcDataReader.Close();

                string clientId = getgmailcredentials.client_id;
                string clientSecret = getgmailcredentials.client_secret;
                string refreshToken = getgmailcredentials.refresh_token;
                string s_no = getgmailcredentials.s_no;
                var mailDetailsList = new List<MailDetails>();
                var service = await GetGmailService(clientId, clientSecret, refreshToken);
                var messageDetails = await service.Users.Messages.Get("me", values.inbox_id).ExecuteAsync();
                var payload = messageDetails.Payload;

                // Extract headers
                var mailDetails = new MailDetails
                {
                    MessageId = values.inbox_id,
                    Subject = payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value,
                    From = ExtractEmailAddresses(payload, "From"),
                    Cc = ExtractEmailAddresses(payload, "Cc"),   // Extract Cc
                    Bcc = ExtractEmailAddresses(payload, "Bcc"), // Extract Bcc
                    Date = payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value,
                    Body = ExtractBody(payload),
                    isUnread = messageDetails.LabelIds.Contains("UNREAD"),
                    Attachments = new List<AttachmentDetails>()
                };
                if (payload.Parts != null)
                {
                    foreach (var part in payload.Parts)
                    {
                        if (!string.IsNullOrEmpty(part.Filename) && !string.IsNullOrEmpty(part.Body.AttachmentId))
                        {
                            var attachment = await service.Users.Messages.Attachments.Get("me", values.inbox_id, part.Body.AttachmentId).ExecuteAsync();
                            var attachmentDetails = new AttachmentDetails
                            {
                                FileName = part.Filename,
                                Base64Url = attachment.Data.Replace('-', '+').Replace('_', '/')
                            };
                            mailDetails.Attachments.Add(attachmentDetails);
                        }
                    }
                }

                mailDetailsList.Add(mailDetails);
                string formattedDateString = string.Empty;
                string[] formats = {
                    // RFC 1123
                    "ddd, dd MMM yyyy HH:mm:ss zzz",
                    "ddd, dd MMM yyyy HH:mm:ss zzzz",
                    "ddd, dd MMM yyyy HH:mm:ss zzz (zzz)",
                    "ddd, dd MMM yyyy HH:mm:sszzz",
                    "ddd, dd MMM yyyy HH:mm:ss",
    
                    // Common date formats
                    "dd MMM yyyy HH:mm:ss zzz",
                    "dd MMM yyyy HH:mm:sszzz",
                    "dd MMM yyyy HH:mm:ss",
    
                    // ISO 8601 formats
                    "yyyy-MM-dd'T'HH:mm:ss'Z'",
                    "yyyy-MM-dd'T'HH:mm:sszzz",
                    "yyyy-MM-dd'T'HH:mm:ss.fffzzz",
                    "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                    "yyyy-MM-ddTHH:mm:sszzz",
                    "yyyy-MM-ddTHH:mm:ss.fffzzz",
                    "yyyy-MM-ddTHH:mm:ss.fff'Z'",
    
                    // General date and time formats
                    "yyyy-MM-dd HH:mm:sszzz",
                    "yyyy-MM-dd HH:mm:ss.fffzzz",
                    "yyyy-MM-dd HH:mm:ss.fff'Z'",
    
                    // UTC formats
                    "ddd, dd MMM yyyy HH:mm:ss zzz '(UTC)'",
                    "ddd, dd MMM yyyy HH:mm:ss '(UTC)'",
    
                    // Hyphen and slash separated formats
                    "dd-MM-yyyy HH:mm:ss",
                    "dd/MM/yyyy HH:mm:ss",
                    "dd-MM-yyyy hh:mm:ss tt",
                    "dd/MM/yyyy hh:mm:ss tt",
    
                    // Timezone variants
                    "dd-MM-yyyy HH:mm:ss zzz",
                    "dd/MM/yyyy HH:mm:ss zzz",
                    "dd/MM/yyyy HH:mm:ss",
                    "dd/MM/yyyy hh:mm:ss tt",
    
                    // 12-hour formats with AM/PM
                    "dd MMM yyyy HH:mm:ss",
                    "dd MMM yyyy hh:mm:ss tt",
                    "ddd, d MMM yyyy HH:mm:ss zzz",
    
                    // Including time zones in parentheses
                    "ddd, dd MMM yyyy HH:mm:ss zzz '(IST)'", // for Indian Standard Time
                    "ddd, dd MMM yyyy HH:mm:ss zzz '(UTC)'", // for UTC
                    "ddd, dd MMM yyyy HH:mm:ss zzz '(GMT)'", // for Greenwich Mean Time
                    "ddd, dd MMM yyyy HH:mm:ss zzz '(PST)'", // for Pacific Standard Time
                    "ddd, dd MMM yyyy HH:mm:ss zzz '(CST)'", // for Central Standard Time
                    "ddd, dd MMM yyyy HH:mm:ss zzz '(EST)'",  // for Eastern Standard Time
                    "ddd, dd MMM yyyy HH:mm:ss zzz '(CDT)'"
                };
                if (DateTimeOffset.TryParseExact(mailDetails.Date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeOffset))

                {
                    formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    try
                    {
                        // Manually process the date string to handle variations
                        string[] parts = mailDetails.Date.Split(' ');

                        // Remove day of the week and optional parts like (UTC)
                        if (parts.Length > 6)
                        {
                            parts = parts.Take(6).ToArray();
                        }

                        // Extract the date and time part
                        string dateTimePart = parts.Length >= 5 ? string.Join(" ", parts.Skip(1).Take(4)) : string.Empty;

                        // Extract the time zone offset part
                        string timeZoneOffsetPart = parts.Length == 6 ? parts[5] : string.Empty;

                        // Parse and format date and time
                        if (DateTimeOffset.TryParseExact(
                            dateTimePart + " " + timeZoneOffsetPart,
                            "dd MMM yyyy HH:mm:ss zzz",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out dateTimeOffset))
                        {
                            formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else if (DateTimeOffset.TryParseExact(
                            dateTimePart + " " + timeZoneOffsetPart,
                            "dd MMM yyyy HH:mm:sszzz",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out dateTimeOffset))
                        {
                            formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    catch (FormatException ex)
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                     "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                     " * **********" + ex.Message.ToString() +
                                     "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                     DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }
                }
                string from = mailDetails.From;
                string cc = mailDetails.Cc;
                string bcc = mailDetails.Bcc;
                string subject = EncodeToBase64(mailDetails.Subject);
                string body = mailDetails.Body;
                bool isUnread = mailDetails.isUnread;
                int attachmentcount = mailDetails.Attachments.Count;
                string attachmentStatus = attachmentcount == 0 ? "N" : "Y";
                msSQL = " select company_code from  adm_mst_tcompany limit 1";
                string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select inbox_id  from crm_trn_tgmailinbox where inbox_id = '" + values.inbox_id + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count == 0)
                {
                    msSQL = "INSERT INTO crm_trn_tgmailinbox (" +
                    "inbox_id, " +
                    "from_id, " +
                    "integrated_gmail, " +
                    "cc, " +
                    "bcc, " +
                    "subject, " +
                    "body, " +
                    "attachement_flag, " +
                    "read_flag, " +
                    "company_code, " +
                    "sent_date) " +
                    "VALUES (" +
                    "'" + values.inbox_id + "', " +
                    "'" + from + "', " +
                    "'" + gmail_address + "', " +
                    "'" + cc + "', " +
                    "'" + bcc + "', " +
                    "'" + subject + "', " +
                    "'" + body + "', " +
                    "'" + attachmentStatus + "', " +
                    "'" + isUnread + "', " +
                    "'" + lscompany_code + "', " +
                    "'" + formattedDateString + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {

                            if (DecodeFromBase64(body.ToString()).IndexOf("New customer message on", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                msSQL = "update crm_trn_tgmailinbox set shopify_enquiry = 'Y' where inbox_id='" + values.inbox_id + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                        if (payload.Parts != null)
                        {
                            foreach (var part in payload.Parts)
                            {
                                if (part.Filename != null && part.Body.AttachmentId != null)
                                {
                                    var attachment = await service.Users.Messages.Attachments.Get("me", values.inbox_id, part.Body.AttachmentId).ExecuteAsync();
                                    var data = Convert.FromBase64String(attachment.Data.Replace('-', '+').Replace('_', '/'));
                                    // Initialize a counter for generating unique IDs
                                    int uniqueIdCounter = 1;

                                    // Increment the counter each time you need a new unique ID
                                    int uniqueId = uniqueIdCounter++;
                                    string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                                    string lsfile_gid = msdocument_gid;
                                    string FileExtension = Path.GetExtension(part.Filename).ToLowerInvariant();
                                    lsfile_gid = lsfile_gid + FileExtension;
                                    string logFilePath = ConfigurationManager.AppSettings["filepath"];

                                    // Check if the directory exists and create it if it does not
                                    if (!Directory.Exists(logFilePath))
                                    {
                                        Directory.CreateDirectory(logFilePath);
                                    }
                                    string attachmentFilePath = SaveAttachmentToFile(service, "me", values.inbox_id, part.Body.AttachmentId, lsfile_gid, ConfigurationManager.AppSettings["filepath"]);
                                    // Save attachment to file
                                    // File.WriteAllBytes(part.Filename, data);

                                    msSQL = "INSERT INTO crm_trn_tgmailinboxattachment (" +
                                             "inbox_id, " +
                                            "original_filename, " +
                                          "modified_filename, " +
                                            "file_path) " +
                                            "VALUES (" +
                                             "'" + values.inbox_id + "', " +
                                            "'" + part.Filename + "', " +
                                            "'" + lsfile_gid + "', " +
                                            "'" + ConfigurationManager.AppSettings["db_path"].ToString() + lsfile_gid + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 1)
                                    {
                                        values.status = false;
                                        values.message = "Error While Occured Submitting Appointment ";
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                        " * **********" + msSQL +
                                        "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                        DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                    }
                                    else
                                    {
                                        values.status = true;
                                        values.message = "Appointment Created Successfully";
                                    }

                                }
                            }
                        }
                        msSQL = "select leadbank_gid from crm_trn_tleadbankcontact where email ='" + values.email_id + "' and main_contact ='Y'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            string leadbank_gid = objOdbcDataReader["leadbank_gid"].ToString();

                            msGetGid = objcmnfunctions.GetMasterGID("APMT");
                            msSQL = " insert into crm_trn_tappointment (" +
                                             " appointment_gid," +
                                             " lead_title, " +
                                             " leadbank_gid, " +
                                             " business_vertical, " +
                                             " appointment_date, " +
                                             " Leadstage_gid," +
                                             " created_by," +
                                             "created_date" +
                                              ") values (" +
                                             "'" + msGetGid + "', " +
                                             "'" + values.lead_title + "'," +
                                             "'" + leadbank_gid + "'," +
                                             "'" + values.bussiness_verticle + "'," +
                                             "'" + values.appointment_timing + "'," +
                                             "'" + "1" + "'," +
                                              "'" + user_gid + "'," +
                                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = " insert into crm_trn_tOpportunitylog ( " +
                                        " appointment_gid, " +
                                        " log_type, " +
                                        " log_date, " +
                                        " log_remarks, " +
                                        " created_by, " +
                                        " created_date ) " +
                                        " values (  " +
                                        "'" + msGetGid + "'," +
                                        "'Opportunity'," +
                                        "'" + values.appointment_timing + "'," +
                                        "'" + values.lead_title + "'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    msSQL = " update  crm_trn_tgmailinbox set " +
                                   " leadbank_gid = '" + leadbank_gid + "' where inbox_id='" + values.inbox_id + "' ";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult == 1)
                                    {
                                        values.status = true;
                                        values.message = "Appointment Created Successfully";
                                    }
                                    else
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                       "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                       " * **********" + msSQL +
                                       "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                       DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                        values.status = false;
                                        values.message = "Error While Occured Submitting Appointment ";
                                    }
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Occured Submitting Appointment ";
                            }

                        }
                        else
                        {
                            msSQL = "select source_gid from crm_mst_tsource where source_name = 'Email'";
                            string source_gid = objdbconn.GetExecuteScalar(msSQL);

                            if (string.IsNullOrEmpty(source_gid))
                            {
                                msGetGid = objcmnfunctions.GetMasterGID("BSEM");
                                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='BSEM' order by finyear desc limit 0,1 ";
                                string lsCode = objdbconn.GetExecuteScalar(msSQL);
                                string lssource_code = "SCM" + "000" + lsCode;

                                msSQL = " insert into crm_mst_tsource(" +
                                        " source_gid," +
                                        " source_code," +
                                        " source_name," +
                                        " created_by, " +
                                        " created_date)" +
                                        " values(" +
                                        " '" + msGetGid + "'," +
                                        " '" + lssource_code + "'," +
                                        "'Email'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    lssource_gid = msGetGid;
                                }
                            }
                            else
                            {
                                lssource_gid = source_gid;
                            }

                            msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                            string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                            msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                            msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                            msSQL = " INSERT INTO crm_trn_tleadbank(" +
                                    " leadbank_gid," +
                                    " source_gid," +
                                    " leadbank_id," +
                                    " leadbank_name," +
                                    " status," +
                                    " approval_flag, " +
                                    " lead_status," +
                                    " leadbank_code," +
                                    " customer_type," +
                                    " customertype_gid," +
                                    " remarks," +
                                    " created_by," +
                                    " main_branch," +
                                    " created_date)" +
                                    " values(" +
                                    " '" + msGetGid1 + "'," +
                                    " '" + lssource_gid + "'," +
                                    " '" + msGetGid + "'," +
                                    " '" + values.email_id + "'," +
                                    " 'y'," +
                                    " 'Approved'," +
                                    " 'Not Assigned'," +
                                    " 'H.Q'," +
                                    " 'Corporate'," +
                                    " 'Corporate'," +
                                    "'" + values.lead_title + "'," +
                                    " '" + lsemployee_gid + "'," +
                                    " 'Y'," +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                            if (msGetGid2 == "E")
                            {
                                values.status = false;
                                values.message = "Create sequence code BLCC for Lead Bank";
                            }

                            if (mnResult == 1)
                            {
                                msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                                " (leadbankcontact_gid," +
                                " leadbank_gid," +
                                " leadbankcontact_name," +
                                " email," +
                                " created_date," +
                                " created_by," +
                                " leadbankbranch_name, " +
                                " main_contact)" +
                                " values( " +
                                " '" + msGetGid2 + "'," +
                                " '" + msGetGid1 + "'," +
                                " '" + values.email_id + "'," +
                                " '" + values.email_id + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                " '" + lsemployee_gid + "'," +
                                " 'H.Q'," +
                                " 'y'" + ")";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    msSQL = " update  crm_trn_tgmailinbox set " +
                                " leadbank_gid = '" + msGetGid1 + "' where inbox_id='" + values.inbox_id + "' ";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult == 1)
                                    {
                                        msGetGid = objcmnfunctions.GetMasterGID("APMT");
                                        msSQL = " insert into crm_trn_tappointment (" +
                                                         " appointment_gid," +
                                                         " lead_title, " +
                                                         " leadbank_gid, " +
                                                         " business_vertical, " +
                                                         " appointment_date, " +
                                                         " Leadstage_gid," +
                                                         " created_by," +
                                                         "created_date" +
                                                          ") values (" +
                                                         "'" + msGetGid + "', " +
                                                         "'" + values.lead_title + "'," +
                                                         "'" + msGetGid1 + "'," +
                                                         "'" + values.bussiness_verticle + "'," +
                                                         "'" + values.appointment_timing + "'," +
                                                         "'" + "1" + "'," +
                                                          "'" + user_gid + "'," +
                                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            msSQL = " insert into crm_trn_tOpportunitylog ( " +
                                                    " appointment_gid, " +
                                                    " log_type, " +
                                                    " log_date, " +
                                                    " log_remarks, " +
                                                    " created_by, " +
                                                    " created_date ) " +
                                                    " values (  " +
                                                    "'" + msGetGid + "'," +
                                                    "'Opportunity'," +
                                                    "'" + values.appointment_timing + "'," +
                                                    "'" + values.lead_title + "'," +
                                                    "'" + user_gid + "'," +
                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }
                                        else
                                        {
                                            values.status = false;
                                            values.message = "Error While Occured Creating Appointment  ";
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                        " * **********" + msSQL +
                                        "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                        DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                        }
                                    }

                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error While Occured Creating Appointment  ";
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                    " * **********" + msSQL +
                                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                    DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                    }
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Occured Creating Appointment  ";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                " * **********" + msSQL +
                                "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Occured Creating Appointment  ";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                            " * **********" + msSQL +
                            "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                            DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                            }

                        }
                        objOdbcDataReader.Close();
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Appointment Created Successfully";
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                            " * **********" + msSQL +
                                            "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                            DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                            values.status = false;
                            values.message = "Error While Occured Submitting Appointment ";
                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                        " * **********" + msSQL +
                                        "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                        DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        values.status = false;
                        values.message = "Error While Occured Submitting Appointment ";
                    }
                }
                else 
                {
                    msSQL = "select leadbank_gid from crm_trn_tleadbankcontact where email ='" + values.email_id + "' and main_contact ='Y'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        string leadbank_gid = objOdbcDataReader["leadbank_gid"].ToString();

                        msGetGid = objcmnfunctions.GetMasterGID("APMT");
                        msSQL = " insert into crm_trn_tappointment (" +
                                         " appointment_gid," +
                                         " lead_title, " +
                                         " leadbank_gid, " +
                                         " business_vertical, " +
                                         " appointment_date, " +
                                         " Leadstage_gid," +
                                         " created_by," +
                                         "created_date" +
                                          ") values (" +
                                         "'" + msGetGid + "', " +
                                         "'" + values.lead_title + "'," +
                                         "'" + leadbank_gid + "'," +
                                         "'" + values.bussiness_verticle + "'," +
                                         "'" + values.appointment_timing + "'," +
                                         "'" + "1" + "'," +
                                          "'" + user_gid + "'," +
                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = " insert into crm_trn_tOpportunitylog ( " +
                                    " appointment_gid, " +
                                    " log_type, " +
                                    " log_date, " +
                                    " log_remarks, " +
                                    " created_by, " +
                                    " created_date ) " +
                                    " values (  " +
                                    "'" + msGetGid + "'," +
                                    "'Opportunity'," +
                                    "'" + values.appointment_timing + "'," +
                                    "'" + values.lead_title + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = " update  crm_trn_tgmailinbox set " +
                               " leadbank_gid = '" + leadbank_gid + "' where inbox_id='" + values.inbox_id + "' ";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    if (DecodeFromBase64(body.ToString()).IndexOf("New customer message on", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        msSQL = "update crm_trn_tgmailinbox set shopify_enquiry = 'Y' where inbox_id='" + values.inbox_id + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            values.status = true;
                                            values.message = "Appointment Created Successfully";
                                        }
                                        else{
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                                                             " * **********" + msSQL +
                                                                             "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                            values.status = false;
                                            values.message = "Error While Occured Submitting Appointment ";
                                        }
                                        }
                                    else { 
                                        values.status = true;
                                        values.message = "Appointment Created Successfully";
                                    }
                                    
                                }
                                else
                                {
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                   "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                   " * **********" + msSQL +
                                   "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                   DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                    values.status = false;
                                    values.message = "Error While Occured Submitting Appointment ";
                                }
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Occured Submitting Appointment ";
                        }

                    }
                    else
                    {
                        msSQL = "select source_gid from crm_mst_tsource where source_name = 'Email'";
                        string source_gid = objdbconn.GetExecuteScalar(msSQL);

                        if (string.IsNullOrEmpty(source_gid))
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("BSEM");
                            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='BSEM' order by finyear desc limit 0,1 ";
                            string lsCode = objdbconn.GetExecuteScalar(msSQL);
                            string lssource_code = "SCM" + "000" + lsCode;

                            msSQL = " insert into crm_mst_tsource(" +
                                    " source_gid," +
                                    " source_code," +
                                    " source_name," +
                                    " created_by, " +
                                    " created_date)" +
                                    " values(" +
                                    " '" + msGetGid + "'," +
                                    " '" + lssource_code + "'," +
                                    "'Email'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                lssource_gid = msGetGid;
                            }
                        }
                        else
                        {
                            lssource_gid = source_gid;
                        }

                        msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                        string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                        msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                        msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                        msSQL = " INSERT INTO crm_trn_tleadbank(" +
                                " leadbank_gid," +
                                " source_gid," +
                                " leadbank_id," +
                                " leadbank_name," +
                                " status," +
                                " approval_flag, " +
                                " lead_status," +
                                " leadbank_code," +
                                " customer_type," +
                                " customertype_gid," +
                                " remarks," +
                                " created_by," +
                                " main_branch," +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid1 + "'," +
                                " '" + lssource_gid + "'," +
                                " '" + msGetGid + "'," +
                                " '" + values.email_id + "'," +
                                " 'y'," +
                                " 'Approved'," +
                                " 'Not Assigned'," +
                                " 'H.Q'," +
                                " 'Corporate'," +
                                " 'Corporate'," +
                                "'" + values.lead_title + "'," +
                                " '" + lsemployee_gid + "'," +
                                " 'Y'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                        if (msGetGid2 == "E")
                        {
                            values.status = false;
                            values.message = "Create sequence code BLCC for Lead Bank";
                        }

                        if (mnResult == 1)
                        {
                            msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                            " (leadbankcontact_gid," +
                            " leadbank_gid," +
                            " leadbankcontact_name," +
                            " email," +
                            " created_date," +
                            " created_by," +
                            " leadbankbranch_name, " +
                            " main_contact)" +
                            " values( " +
                            " '" + msGetGid2 + "'," +
                            " '" + msGetGid1 + "'," +
                            " '" + values.email_id + "'," +
                            " '" + values.email_id + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            " '" + lsemployee_gid + "'," +
                            " 'H.Q'," +
                            " 'y'" + ")";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = " update  crm_trn_tgmailinbox set " +
                            " leadbank_gid = '" + msGetGid1 + "' where inbox_id='" + values.inbox_id + "' ";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    msGetGid = objcmnfunctions.GetMasterGID("APMT");
                                    msSQL = " insert into crm_trn_tappointment (" +
                                                     " appointment_gid," +
                                                     " lead_title, " +
                                                     " leadbank_gid, " +
                                                     " business_vertical, " +
                                                     " appointment_date, " +
                                                     " Leadstage_gid," +
                                                     " created_by," +
                                                     "created_date" +
                                                      ") values (" +
                                                     "'" + msGetGid + "', " +
                                                     "'" + values.lead_title + "'," +
                                                     "'" + msGetGid1 + "'," +
                                                     "'" + values.bussiness_verticle + "'," +
                                                     "'" + values.appointment_timing + "'," +
                                                     "'" + "1" + "'," +
                                                      "'" + user_gid + "'," +
                                                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        msSQL = " insert into crm_trn_tOpportunitylog ( " +
                                                " appointment_gid, " +
                                                " log_type, " +
                                                " log_date, " +
                                                " log_remarks, " +
                                                " created_by, " +
                                                " created_date ) " +
                                                " values (  " +
                                                "'" + msGetGid + "'," +
                                                "'Opportunity'," +
                                                "'" + values.appointment_timing + "'," +
                                                "'" + values.lead_title + "'," +
                                                "'" + user_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error While Occured Creating Appointment  ";
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                    " * **********" + msSQL +
                                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                    DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                    }
                                }

                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Occured Creating Appointment  ";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                " * **********" + msSQL +
                                "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Occured Creating Appointment  ";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                            " * **********" + msSQL +
                            "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                            DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Occured Creating Appointment  ";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                        " * **********" + msSQL +
                        "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                        DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        }

                    }
                    objOdbcDataReader.Close();
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Appointment Created Successfully";
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                        " * **********" + msSQL +
                                        "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                        DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        values.status = false;
                        values.message = "Error While Occured Submitting Appointment ";
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public class MailDetails
        {
            public string MessageId { get; set; }
            public string Subject { get; set; }
            public string From { get; set; }
            public string Cc { get; set; }
            public string Bcc { get; set; }
            public string Date { get; set; }
            public string Body { get; set; }
            public bool isUnread { get; set; }
            public List<AttachmentDetails> Attachments { get; set; }
        }

        public class AttachmentDetails
        {
            public string FileName { get; set; }
            public string Base64Url { get; set; }
        }
    }
}