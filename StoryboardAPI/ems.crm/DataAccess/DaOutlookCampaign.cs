using ems.crm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Http.Results;
using System.IO;
using System.Web.Http.Filters;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Security.Policy;
using System.Text;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Web.Services.Description;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using FileIO = System.IO.File;
using static ems.crm.DataAccess.DaGmailCampaign;

namespace ems.crm.DataAccess
{

    public class DaOutlookCampaign
    {


        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, msGetGid2, lspath2, final_path, lssent_date, lsbody, lssubject, lsfrom_id, messageId, lssource_gid;
        public void DaPostOutlookTemplate(outlooktemplate_list values, string user_gid)
        {
            try
            {
                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                string httpsUrl = string.Empty;
                string lscompany_code = string.Empty;
                msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                string dataUrl = values.template_body;
                string matchString = Regex.Match(dataUrl, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
                Match match = Regex.Match(matchString, @"^data:image\/(?<fileExtension>[a-zA-Z]+);base64,(?<base64Content>.+)$");
                string base64Content = match.Groups["base64Content"].Value;
                string fileExtension = match.Groups["fileExtension"].Value;
                byte[] bytes = Convert.FromBase64String(base64Content);
                if (bytes != null && bytes.Length > 0)
                {
                    using (MemoryStream imageStream = new MemoryStream(bytes))
                    {


                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(imageStream))
                        {
                            int newWidth = 300;
                            int newHeight = (int)(((float)newWidth / image.Width) * image.Height);

                            using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                            {
                                using (Graphics g = Graphics.FromImage(resizedImage))
                                {
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    g.DrawImage(image, 0, 0, newWidth, newHeight);
                                }
                                using (MemoryStream resizedImageStream = new MemoryStream())
                                {
                                    switch (fileExtension.ToLower())
                                    {
                                        case "jpg":
                                        case "jpeg":
                                            resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            break;
                                        case "png":
                                            resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Png);
                                            break;
                                        case "gif":
                                            resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Gif);
                                            break;
                                        case "bmp":
                                            resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Bmp);
                                            break;
                                        case "tif":
                                        case "tiff":
                                            resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Tiff);
                                            break;
                                        default:
                                            resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Png);
                                            break;
                                    }
                                    byte[] resizedImageBytes = resizedImageStream.ToArray();
                                    //System.IO.File.WriteAllBytes(lspath + msdocument_gid + "." + fileExtension, resizedImageBytes);

                                    bool status1;


                                    status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Mail/Template/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + '.' + fileExtension, fileExtension, imageStream);

                                }

                            }
                        }
                    }

                    final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/Template/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                    httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + '.' + fileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                     '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                     '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];


                }
                else
                {
                    values.message = "Error While Adding Campaign!!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * *********************" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }
                if (httpsUrl != null)
                {
                    string originalHtml = values.template_body.Replace("\"", "\\\"").Replace("'", "\\\'");
                    string temp_body = Regex.Replace(originalHtml, @"data:[^;]+;base64,[^""]+", httpsUrl + "\\");
                    msSQL = "INSERT INTO crm_trn_toutlooktemplate (" +
                               "template_name, " +
                               "template_subject, " +
                               "template_body, " +
                                " created_by, " +
                               "created_date) " +
                               "VALUES (" +
                                "'" + values.template_name.Replace("\"", "\\\"").Replace("'", "\\\'") + "', " +
                               "'" + values.template_subject.Replace("\"", "\\\"").Replace("'", "\\\'") + "', " +
                               "'" + temp_body + "', " +
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
                    }
                }


            }
            catch (Exception ex)
            {
                values.message = "Error While Adding Mail Tempalte";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/outlook/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }
        public void DaOutlookTemplateSummary(MdlOutlookCampaign values)

        {
            try
            {


                msSQL = "select a.template_gid,a.template_name,a.template_subject,a.template_body,  " +
                        " date_format(a.created_date, '%d-%m-%Y') as date,a.created_by,a.template_flag, count(b.template_gid) as template_count  " +
                        " from crm_trn_toutlooktemplate a " +
                        " left join crm_trn_toutlook b on b.template_gid=a.template_gid group by template_gid order by created_date";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<outlooktemplatesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new outlooktemplatesummary_list
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
                        values.outlooktemplatesummary_list = getmodulelist;
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
        public void DaPostCampaignStatus(outlooktemplatesummary_list values)

        {
            try
            {
                msSQL = "select template_flag from crm_trn_toutlooktemplate where template_gid='" + values.template_gid + "'";
                string template_flag = objdbconn.GetExecuteScalar(msSQL);
                if (template_flag == "Y")
                {
                    msSQL = "update crm_trn_toutlooktemplate set template_flag='N' where template_gid='" + values.template_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Template Deactivated!!";
                    }
                }
                else
                {
                    msSQL = "update crm_trn_toutlooktemplate set template_flag='Y' where template_gid='" + values.template_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Template Activated!!";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Update  Template Status";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaComposeOutlookMail(string employee_gid, MdlOutlookCampaign values)

        {
            try
            {


                msSQL = " select employee_emailid from hrm_mst_temployee where employee_gid= '" + employee_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<ComposeOutlookMail_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new ComposeOutlookMail_list
                        {

                            employee_emailid = dt["employee_emailid"].ToString(),
                        });
                        values.ComposeOutlookMail_list = getmodulelist;
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
        ///Compose Mail///
        public result DaSendOutlookMail(string user_gid, MdlSendMail values)
        {
            result objresult = new result();
            graphtoken objtoken = new graphtoken();
            try
            {
                objtoken = generateGraphAccesstoken();
                if (objtoken.status)
                {
                    MdlGraphMailContent objMdlGraphMailContent = new MdlGraphMailContent();
                    objMdlGraphMailContent.message = new Message1();
                    objMdlGraphMailContent.saveToSentItems = true;
                    objMdlGraphMailContent.message.body = new Body2();
                    objMdlGraphMailContent.message.body.contentType = "HTML";

                    string urlPattern = @"(http|https)://\S+";
                    MatchCollection urlMatches = Regex.Matches(values.mail_body, urlPattern);
                    string htmlContent = values.mail_body;

                    // Split the htmlContent into separate lines
                    string[] lines = htmlContent.Split(new[] { "<br>" }, StringSplitOptions.None);

                    // Create a new htmlContent with separate <a> tags for each URL
                    htmlContent = "<html><body>";
                    foreach (string line in lines)
                    {
                        if (Regex.IsMatch(line, urlPattern))
                        {
                            Match urlMatch = Regex.Match(line, urlPattern);
                            string url = urlMatch.Value;
                            string anchorTag = $"<a href=\"{url}\" target=\"_blank\">{url}</a><br>";
                            htmlContent += anchorTag;
                        }
                        else
                        {
                            htmlContent += line + "<br>";
                        }
                    }


                    // Remove unnecessary HTML tags
                    //htmlContent = htmlContent.Replace("<font face=\"Arial\">", "<br>");
                    //htmlContent = htmlContent.Replace("</font>", "<br>");
                    //htmlContent = htmlContent.Replace("<p><br></p>", "");
                    //htmlContent = htmlContent.Replace("<p>", "");
                    //htmlContent = htmlContent.Replace("</p>", "<br>");
                    htmlContent += "</body></html>";

                    objMdlGraphMailContent.message.body.content = htmlContent;
                    objMdlGraphMailContent.message.subject = values.mail_sub;
                    string[] to_mail = values.to_mail.Split(',');
                    string[] cc_mails = null;
                    string[] bcc_mails = null;
                    if (values.cc_mail != null && values.cc_mail != "")
                    {
                        cc_mails = values.cc_mail.Split(',');
                    }
                    if (values.bcc_mail != null && values.bcc_mail != "")
                    {
                        bcc_mails = values.bcc_mail.Split(',');
                    }
                    if (to_mail != null)
                    {
                        objMdlGraphMailContent.message.toRecipients = new Torecipient[to_mail.Length];
                        for (int i = 0; i < objMdlGraphMailContent.message.toRecipients.Length; i++)
                        {
                            objMdlGraphMailContent.message.toRecipients[i] = new Torecipient();
                            objMdlGraphMailContent.message.toRecipients[i].emailAddress = new Emailaddress();
                            objMdlGraphMailContent.message.toRecipients[i].emailAddress.address = to_mail[i];
                        }
                    }

                    if (cc_mails != null && cc_mails.Length != 0)
                    {
                        objMdlGraphMailContent.message.ccRecipients = new Torecipient[cc_mails.Length];
                        for (int i = 0; i < objMdlGraphMailContent.message.ccRecipients.Length; i++)
                        {
                            objMdlGraphMailContent.message.ccRecipients[i] = new Torecipient();
                            objMdlGraphMailContent.message.ccRecipients[i].emailAddress = new Emailaddress();
                            objMdlGraphMailContent.message.ccRecipients[i].emailAddress.address = cc_mails[i];
                        }
                    }

                    if (bcc_mails != null && bcc_mails.Length != 0)
                    {
                        objMdlGraphMailContent.message.bccRecipients = new Torecipient[bcc_mails.Length];
                        for (int i = 0; i < objMdlGraphMailContent.message.bccRecipients.Length; i++)
                        {
                            objMdlGraphMailContent.message.bccRecipients[i] = new Torecipient();
                            objMdlGraphMailContent.message.bccRecipients[i].emailAddress = new Emailaddress();
                            objMdlGraphMailContent.message.bccRecipients[i].emailAddress.address = bcc_mails[i];
                        }
                    }

                    if (values.file != null && values.file.Length != 0)
                    {
                        objMdlGraphMailContent.message.attachments = new attachments[values.file.Length];
                        for (int i = 0; i < objMdlGraphMailContent.message.attachments.Length; i++)
                        {
                            objMdlGraphMailContent.message.attachments[i] = new attachments();
                            objMdlGraphMailContent.message.attachments[i].name = values.file[i].file_name;
                            objMdlGraphMailContent.message.attachments[i].contentBytes = values.file[i].content_bytes;
                            objMdlGraphMailContent.message.attachments[i].OdataType = "#microsoft.graph.fileAttachment";
                        }
                    }

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["GraphSendURL"].ToString());
                    var request = new RestRequest("/v1.0/users/" + values.from_mail + "/sendMail", Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", objtoken.access_token);
                    string request_body = JsonConvert.SerializeObject(objMdlGraphMailContent);
                    request.AddParameter("application/json", request_body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        string encodedBody = EncodeToBase64(htmlContent);
                        string msdocument_gid = objcmnfunctions.GetMasterGID("OILC");
                        msSQL = "INSERT INTO crm_trn_toutlook (" +
                                "mail_gid, " +
                                "from_mailaddress, " +
                                "to_mailaddress, " +
                                "mail_subject, " +
                                "mail_body, " +
                                "leadbank_gid, " +
                                 " sent_by, " +
                                "sent_date) " +
                        "VALUES (" +
                                "'" + msdocument_gid + "', " +
                                "'" + values.from_mail + "', " +
                                "'" + values.to_mail + "', " +
                                "'" + values.mail_sub.Replace("\"", "\\\"") + "', " +
                                "'" + encodedBody + "', " +
                                "'" + values.leadbank_gid + "', " +
                                  "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        objresult.status = true;
                        objresult.message = "Mail sent Successfully. Our Team will get in touch shortly!";
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + response.Content.ToString() + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }
                }
                else
                {
                    objresult.message = "Error occured while sending mail :" + objtoken.message;
                }
            }
            catch (Exception e)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + e.ToString() + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            return objresult;
        }
        public graphtoken generateGraphAccesstoken()
        {
            graphtoken objtoken = new graphtoken();
            mdlgraph_list objMdlGraph = new mdlgraph_list();

            try
            {
                msSQL = "select client_id,client_secret,tenant_id from crm_smm_outlook_service";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null && objOdbcDataReader.HasRows)
                {
                    objMdlGraph.tenantID = objOdbcDataReader["tenant_id"].ToString();
                    objMdlGraph.clientID = objOdbcDataReader["client_id"].ToString();
                    objMdlGraph.client_secret = objOdbcDataReader["client_secret"].ToString();
                }
                objOdbcDataReader.Close();
                if (!string.IsNullOrEmpty(objMdlGraph.tenantID) && !string.IsNullOrEmpty(objMdlGraph.clientID) && !string.IsNullOrEmpty(objMdlGraph.client_secret))
                {
                    msSQL = "select token,expiry_time from crm_smm_tgraphtoken";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader != null && objOdbcDataReader.HasRows)
                    {
                        DateTime expiry = DateTime.Parse(objOdbcDataReader["expiry_time"].ToString());
                        if (DateTime.Compare(expiry, DateTime.Now) < 0)
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["GraphLoginURL"].ToString());
                            var request = new RestRequest("/" + objMdlGraph.tenantID + "/oauth2/v2.0/token", Method.POST);
                            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                            request.AddParameter("client_id", objMdlGraph.clientID);
                            request.AddParameter("scope", ConfigurationManager.AppSettings["GraphLoginScope"].ToString());
                            request.AddParameter("client_secret", objMdlGraph.client_secret);
                            request.AddParameter("grant_type", "client_credentials");
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                graphLoginSuccessResponse objgraphLoginSuccessResponse = new graphLoginSuccessResponse();
                                objgraphLoginSuccessResponse = JsonConvert.DeserializeObject<graphLoginSuccessResponse>(response.Content);
                                objtoken.access_token = objgraphLoginSuccessResponse.access_token;
                                objtoken.status = true;
                                msSQL = "update crm_smm_tgraphtoken set token = '" + objtoken.access_token +
                                        "',expiry_time = '" + DateTime.Now.AddSeconds(3595).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 0)
                                {
                                    objcmnfunctions.LogForAudit("Error occurred while insert: " + msSQL);
                                }
                            }
                        }
                        else
                        {
                            objtoken.access_token = objOdbcDataReader["token"].ToString();
                            objtoken.status = true;
                        }
                    }
                    else
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["GraphLoginURL"].ToString());
                        var request = new RestRequest("/" + objMdlGraph.tenantID + "/oauth2/v2.0/token", Method.POST);
                        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        request.AddParameter("client_id", objMdlGraph.clientID);
                        request.AddParameter("scope", ConfigurationManager.AppSettings["GraphLoginScope"].ToString());
                        request.AddParameter("client_secret", objMdlGraph.client_secret);
                        request.AddParameter("grant_type", "client_credentials");
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            graphLoginSuccessResponse objgraphLoginSuccessResponse = new graphLoginSuccessResponse();
                            objgraphLoginSuccessResponse = JsonConvert.DeserializeObject<graphLoginSuccessResponse>(response.Content);
                            objtoken.access_token = objgraphLoginSuccessResponse.access_token;
                            objtoken.status = true;
                            msSQL = "insert into crm_smm_tgraphtoken(token,expiry_time)values(" +
                                    "'" + objtoken.access_token + "'," +
                                    "'" + DateTime.Now.AddSeconds(3595).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("Error occurred while insert: " + msSQL);
                            }
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("Error while generating access token: " + response.Content.ToString());
                        }
                    }

                    objOdbcDataReader.Close();
                }
                else
                {
                    objtoken.message = "Kindly add the app details for sending mails!";
                }

            }
            catch (Exception ex)
            {
                objtoken.message = ex.Message;
                objcmnfunctions.LogForAudit("Exception while generating access token: " + ex.ToString());
            }
            return objtoken;
        }
        public result DaSendOutlookMailwithfiles(HttpRequest httpRequest, string user_gid)
        {
            result objresult = new result();
            graphtoken objtoken = new graphtoken();

            try
            {
                HttpFileCollection httpFileCollection;
                HttpPostedFile httpPostedFile;

                string basecode = httpRequest.Form["mailfiles"];
                List<sendmail_list> hrDocuments = JsonConvert.DeserializeObject<List<sendmail_list>>(basecode);
                string from_mail = httpRequest.Form["from_mail"];
                string mail_sub = httpRequest.Form["mail_sub"];
                string to_mail = httpRequest.Form["to_mail"];
                string cc_mail = httpRequest.Form["cc_mail"];
                string bcc_mail = httpRequest.Form["bcc_mail"];
                string mail_body = httpRequest.Form["mail_body"];

                httpFileCollection = httpRequest.Files;
                string lsfilepath = string.Empty;
                string document_gid = string.Empty;
                string lspath, lspath1;
                string lscompany_code = string.Empty;
                string httpsUrl = string.Empty;
                msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select leadbank_gid from crm_trn_tleadbankcontact where email='" + to_mail + "' and main_contact ='Y'";
                string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                List<DbAttachmentPath> dbattachmentpath = new List<DbAttachmentPath>();
                List<MailAttachmentbase64> mailAttachmentbase64 = new List<MailAttachmentbase64>();
                if (httpRequest.Files.Count > 0)
                {
                    string msdocument_gid = objcmnfunctions.GetMasterGID("OILC");
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        MemoryStream ms = new MemoryStream();
                        var getdocumentdtl = hrDocuments.Where(a => a.AutoID_Key == httpFileCollection.AllKeys[i]).FirstOrDefault();
                        httpPostedFile = httpFileCollection[i];
                        string file_name = httpPostedFile.FileName;
                        string type = httpPostedFile.ContentType;
                        //string apibasecode = "data:" + type + ";base64,";
                        //basecode = basecode.Substring(apibasecode.Length);
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        string FileExtension = Path.GetExtension(file_name).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        bool status1;
                        string final_path;
                        string base64String = string.Empty;

                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);

                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];

                        byte[] fileBytes = ms.ToArray();
                        base64String = Convert.ToBase64String(fileBytes);

                        dbattachmentpath.Add(new DbAttachmentPath
                        { path = httpsUrl }
                       );
                        // Add attachment to the list
                        mailAttachmentbase64.Add(new MailAttachmentbase64
                        {
                            name = file_name,
                            type = type,
                            data = base64String
                        });
                        ms.Close();
                        objtoken = generateGraphAccesstoken();

                    }
                    if (objtoken.status)
                    {
                        MdlGraphMailContent objMdlGraphMailContent = new MdlGraphMailContent();
                        objMdlGraphMailContent.message = new Message1();
                        objMdlGraphMailContent.saveToSentItems = true;
                        objMdlGraphMailContent.message.body = new Body2();
                        objMdlGraphMailContent.message.body.contentType = "HTML";

                        string urlPattern = @"(http|https)://\S+";
                        MatchCollection urlMatches = Regex.Matches(mail_body, urlPattern);
                        string htmlContent = mail_body;

                        // Split the htmlContent into separate lines
                        string[] lines = htmlContent.Split(new[] { "<br>" }, StringSplitOptions.None);

                        // Create a new htmlContent with separate <a> tags for each URL
                        htmlContent = "<html><body>";
                        foreach (string line in lines)
                        {
                            if (Regex.IsMatch(line, urlPattern))
                            {
                                Match urlMatch = Regex.Match(line, urlPattern);
                                string url = urlMatch.Value;
                                string anchorTag = $"<a href=\"{url}\" target=\"_blank\">{url}</a><br>";
                                htmlContent += anchorTag;
                            }
                            else
                            {
                                htmlContent += line + "<br>";
                            }
                        }


                        // Remove unnecessary HTML tags
                        //htmlContent = htmlContent.Replace("<font face=\"Arial\">", "<br>");
                        //htmlContent = htmlContent.Replace("</font>", "<br>");
                        //htmlContent = htmlContent.Replace("<p><br></p>", "");
                        //htmlContent = htmlContent.Replace("<p>", "");
                        //htmlContent = htmlContent.Replace("</p>", "<br>");
                        htmlContent += "</body></html>";

                        objMdlGraphMailContent.message.body.content = htmlContent;
                        //objMdlGraphMailContent.message.body.content = mail_body;
                        objMdlGraphMailContent.message.subject = mail_sub;
                        string[] to_mails = to_mail.Split(',');
                        string[] cc_mails = null;
                        string[] bcc_mails = null;
                        if (cc_mail != null && cc_mail != "")
                        {
                            cc_mails = cc_mail.Split(',');
                        }
                        if (bcc_mail != null && bcc_mail != "")
                        {
                            bcc_mails = bcc_mail.Split(',');
                        }
                        //string[] files = lspath2.Split(',');

                        if (to_mails != null)
                        {
                            objMdlGraphMailContent.message.toRecipients = new Torecipient[to_mails.Length];
                            for (int i = 0; i < objMdlGraphMailContent.message.toRecipients.Length; i++)
                            {
                                objMdlGraphMailContent.message.toRecipients[i] = new Torecipient();
                                objMdlGraphMailContent.message.toRecipients[i].emailAddress = new Emailaddress();
                                objMdlGraphMailContent.message.toRecipients[i].emailAddress.address = to_mails[i];
                            }
                        }

                        if (cc_mails != null && cc_mails.Length != 0)
                        {
                            objMdlGraphMailContent.message.ccRecipients = new Torecipient[cc_mails.Length];
                            for (int i = 0; i < objMdlGraphMailContent.message.ccRecipients.Length; i++)
                            {
                                objMdlGraphMailContent.message.ccRecipients[i] = new Torecipient();
                                objMdlGraphMailContent.message.ccRecipients[i].emailAddress = new Emailaddress();
                                objMdlGraphMailContent.message.ccRecipients[i].emailAddress.address = cc_mails[i];
                            }
                        }

                        if (bcc_mails != null && bcc_mails.Length != 0)
                        {
                            objMdlGraphMailContent.message.bccRecipients = new Torecipient[bcc_mails.Length];
                            for (int i = 0; i < objMdlGraphMailContent.message.bccRecipients.Length; i++)
                            {
                                objMdlGraphMailContent.message.bccRecipients[i] = new Torecipient();
                                objMdlGraphMailContent.message.bccRecipients[i].emailAddress = new Emailaddress();
                                objMdlGraphMailContent.message.bccRecipients[i].emailAddress.address = bcc_mails[i];
                            }
                        }

                        if (httpFileCollection.Count != null && httpFileCollection.Count != 0)
                        {
                            objMdlGraphMailContent.message.attachments = new attachments[mailAttachmentbase64.Count];
                            for (int i = 0; i < mailAttachmentbase64.Count; i++)
                            {
                                objMdlGraphMailContent.message.attachments[i] = new attachments();
                                objMdlGraphMailContent.message.attachments[i].name = mailAttachmentbase64[i].name;
                                objMdlGraphMailContent.message.attachments[i].contentBytes = mailAttachmentbase64[i].data;
                                objMdlGraphMailContent.message.attachments[i].OdataType = "#microsoft.graph.fileAttachment";
                            }
                        }

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["GraphSendURL"].ToString());
                        var request = new RestRequest("/v1.0/users/" + from_mail + "/sendMail", Method.POST);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", objtoken.access_token);
                        string request_body = JsonConvert.SerializeObject(objMdlGraphMailContent);
                        request.AddParameter("application/json", request_body, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.Accepted)
                        {
                            string encodedBody = EncodeToBase64(htmlContent);
                            msSQL = "INSERT INTO crm_trn_toutlook (" +
                                    "mail_gid, " +
                                    "from_mailaddress, " +
                                    "to_mailaddress, " +
                                    "mail_subject, " +
                                    "mail_body, " +
                                    "leadbank_gid, " +
                                     " sent_by, " +
                                    "sent_date) " +
                            "VALUES (" +
                                    "'" + msdocument_gid + "', " +
                                    "'" + from_mail + "', " +
                                    "'" + to_mail + "', " +
                                    "'" + mail_sub.Replace("'", "\\\"") + "', " +
                                    "'" + encodedBody + "', " +
                                    "'" + lsleadbank_gid + "', " +
                                      "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            for (int i = 0; i < dbattachmentpath.Count; i++)
                            {
                                msGetGid1 = objcmnfunctions.GetMasterGID("UPLF");
                                msSQL = "INSERT INTO crm_trn_tfiles (" +
                                 "file_gid, " +
                                "mailmanagement_gid, " +
                                "document_name, " +
                              "document_path, " +
                                "created_date) " +
                                "VALUES (" +
                                 "'" + msGetGid1 + "', " +
                                "'" + msdocument_gid + "', " +
                                "'" + mailAttachmentbase64[i].name + "', " +
                                 "'" + dbattachmentpath[i].path + "', " +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            if(mnResult == 1)
                            { 
                            objresult.status = true;
                            objresult.message = "Mail sent Successfully. Our Team will get in touch shortly!";
                            }
                            else
                            {
                                objresult.status = false;
                                objresult.message = "Error occured while sending mail!";
                            }
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + response.Content.ToString() + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        }
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Error occured while sending mail :" + objtoken.message;
                    }
                }

            }
            catch (Exception e)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + e.ToString() + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            return objresult;
        }
        public void DaOutlookMailSentSummary(MdlOutlookCampaign values)

        {
            try
            {


                msSQL = " select a.mail_gid,a.to_mailaddress,a.mail_body,a.mail_subject,a.from_mailaddress,a.sent_flag,concat(b.user_firstname,' ',b.user_lastname)as sent_by,a.sent_date,a.leadbank_gid from crm_trn_toutlook a left join adm_mst_tuser b on b.user_gid = a.sent_by order by a.sent_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<outlooksentMail_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new outlooksentMail_list
                        {

                            to_mailaddress = dt["to_mailaddress"].ToString(),
                            sent_flag = dt["sent_flag"].ToString(),
                            sent_by = dt["sent_by"].ToString(),
                            sent_date = dt["sent_date"].ToString(),
                            mail_gid = dt["mail_gid"].ToString(),
                            mail_body = DecodeFromBase64(dt["mail_body"].ToString()),
                            mail_subject = dt["mail_subject"].ToString(),
                            from_mailaddress = dt["from_mailaddress"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString()



                        });
                        values.outlooksentMail_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Sent Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }

        public result DaSendOutlookTemplate(outlookmailtemplatesendsummary_list values, string user_gid)
        {
            result result = new result();
            try
            {
                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() => {
                    HttpContext.Current = ctx;
                    DaTemplatesend(values.template_gid, values.outlookmailsendchecklist, user_gid);
                }));
                t.Start();

                result.status = true;
                result.message = "Mail sending successfully!";

            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Mail";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }

        public void DaTemplatesend(string template_gid, List<outlookmailsendchecklist> outlookmailsendchecklist, string user_gid)
        {
            try
            {
                msSQL = " select employee_gid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select employee_emailid from hrm_mst_temployee where employee_gid= '" + lsemployee_gid + "' ";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "SELECT template_subject FROM crm_trn_toutlooktemplate WHERE template_gid = '" + template_gid + "';";
                string template_subject = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT template_body FROM crm_trn_toutlooktemplate WHERE template_gid = '" + template_gid + "';";
                string template_body = objdbconn.GetExecuteScalar(msSQL);

                result objresult = new result();
                graphtoken objtoken = new graphtoken();
                objtoken = generateGraphAccesstoken();

                if (objtoken.status)
                {
                    foreach (var item in outlookmailsendchecklist)
                    {
                        MdlGraphMailContent objMdlGraphMailContent = new MdlGraphMailContent();
                        objMdlGraphMailContent.message = new Message1();
                        objMdlGraphMailContent.saveToSentItems = true;
                        objMdlGraphMailContent.message.body = new Body2();
                        objMdlGraphMailContent.message.body.contentType = "HTML";
                        objMdlGraphMailContent.message.body.content = template_body;
                        objMdlGraphMailContent.message.subject = template_subject;
                        string[] to_mail = item.email.Split(',');
                        if (to_mail != null)
                        {
                            objMdlGraphMailContent.message.toRecipients = new Torecipient[to_mail.Length];
                            for (int i = 0; i < objMdlGraphMailContent.message.toRecipients.Length; i++)
                            {
                                objMdlGraphMailContent.message.toRecipients[i] = new Torecipient();
                                objMdlGraphMailContent.message.toRecipients[i].emailAddress = new Emailaddress();
                                objMdlGraphMailContent.message.toRecipients[i].emailAddress.address = to_mail[i];
                            }
                        }
                        if (item.file != null && item.file.Length != 0)
                        {
                            objMdlGraphMailContent.message.attachments = new attachments[item.file.Length];
                            for (int i = 0; i < objMdlGraphMailContent.message.attachments.Length; i++)
                            {
                                objMdlGraphMailContent.message.attachments[i] = new attachments();
                                objMdlGraphMailContent.message.attachments[i].name = item.file;
                                objMdlGraphMailContent.message.attachments[i].contentBytes = item.file;
                                objMdlGraphMailContent.message.attachments[i].OdataType = "#microsoft.graph.fileAttachment";
                            }
                        }
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["GraphSendURL"].ToString());
                        var request = new RestRequest("/v1.0/users/" + lsemployee_mailid + "/sendMail", Method.POST);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", objtoken.access_token);
                        string request_body = JsonConvert.SerializeObject(objMdlGraphMailContent);
                        request.AddParameter("application/json", request_body, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.Accepted)
                        {
                            string msdocument_gid = objcmnfunctions.GetMasterGID("OILC");
                            msSQL = "INSERT INTO crm_trn_toutlook (" +
                                   "mail_gid, " +
                                   "from_mailaddress, " +
                                   "to_mailaddress, " +
                                   "mail_subject, " +
                                   "template_gid, " +
                                   "leadbank_gid, " +
                                    " sent_by, " +
                                   "sent_date) " +
                           "VALUES (" +
                                   "'" + msdocument_gid + "', " +
                                   "'" + lsemployee_mailid + "', " +
                                   "'" + item.email + "', " +
                                   "'" + template_subject + "', " +
                                   "'" + template_gid + "', " +
                                   "'" + item.leadbank_gid + "', " +
                                     "'" + user_gid + "'," +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }
        public void DaSendTemplatePreview(string template_gid, MdlOutlookCampaign values)

        {
            try
            {


                msSQL = " select template_gid,template_name,template_subject,template_body, " +
                        " date_format(created_date, '%d-%m-%Y') as date,created_by,template_flag " +
                        " from crm_trn_toutlooktemplate where template_gid = '" + template_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<outlooktemplatesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new outlooktemplatesummary_list
                        {

                            created_date = dt["date"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_gid = dt["template_gid"].ToString(),
                            template_subject = dt["template_subject"].ToString(),
                            template_body = dt["template_body"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            template_flag = dt["template_flag"].ToString(),



                        });
                        values.outlooktemplatesummary_list = getmodulelist;
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
        public void DaOutlookCampaignSentSummary(string template_gid, MdlOutlookCampaign values)

        {
            try
            {


                msSQL = "select a.leadbank_gid,date_format(a.sent_date, '%d-%m-%Y')as sent_date,b.leadbank_name,(e.region_name) as leadbank_region,c.source_name,d.email from crm_trn_toutlook a " +
                        " left join crm_trn_tleadbank b on b.leadbank_gid = a.leadbank_gid " +
                        " left join crm_mst_tsource c on c.source_gid = b.source_gid " +
                        " left join crm_mst_tregion e on e.region_gid = b.leadbank_region " +
                        " left join crm_trn_tleadbankcontact d on d.leadbank_gid = a.leadbank_gid where template_gid = '" + template_gid + "' and d.main_contact ='Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<outlooksentcampaign_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new outlooksentcampaign_list
                        {

                            leadbank_region = dt["leadbank_region"].ToString(),
                            email = dt["email"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            sent_date = dt["sent_date"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),



                        });
                        values.outlooksentcampaign_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Error While Campaign Sent Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaRecipientMailList(MdlOutlookCampaign values)

        {
            try
            {


                msSQL = "SELECT a.leadbank_gid AS id, a.leadbank_name AS name, b.leadbankcontact_name AS contact_name, b.email FROM crm_trn_tleadbank a " +
                        " LEFT JOIN crm_trn_tleadbankcontact b ON a.leadbank_gid = b.leadbank_gid UNION ALL " +
                        " SELECT NULL AS id, concat(b.user_firstname, ' ', b.user_lastname) AS name, NULL AS contact_name, a.employee_emailid AS email " +
                        " FROM hrm_mst_temployee a LEFT JOIN adm_mst_tuser b ON b.user_gid = a.user_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<RecipientMailList_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new RecipientMailList_list
                        {

                            name = dt["name"].ToString(),
                            contact_name = dt["contact_name"].ToString(),
                            email = dt["email"].ToString(),

                        });
                        values.RecipientMailList_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Sent Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
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

        ///code by snehith for outlook inbox <summary>
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private static Task<get> _runningTask;
        public async Task<get> DaReadEmailsOutlookmail(string user_gid)
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

                    _runningTask = ExecuteDaReadEmailsOutlookmail(user_gid); // Start a new task
                    await _runningTask;
                }
                finally
                {
                    _semaphore.Release(); // Release the semaphore
                }
            }
        }

        public async Task<get> ExecuteDaReadEmailsOutlookmail(string user_gid)
        {
                get objresult = new get();
                try
            {

                DateTime currentDateTimeUtc = DateTime.UtcNow;
                string formattedCurrentDateTimeUtc = currentDateTimeUtc.ToString("yyyy-MM-ddTHH:mm:ssZ");
                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if(lsemployee_mailid !=null && lsemployee_mailid != "") 
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    string messagesUrl = $"https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + $"/mailFolders/Inbox/messages?$orderby=receivedDateTime desc,hasAttachments&$top=100&$expand=attachments";

                    //string messagesUrl = $"https://graph.microsoft.com/v1.0/users/snehith@vcidex.com/mailFolders/Inbox/messages?$orderby=receivedDateTime desc,hasAttachments&$top=20&$expand=attachments";

                    //string messagesUrl = $"https://graph.microsoft.com/v1.0/users/snehith@vcidex.com/mailFolders/Inbox/messages?$orderby=receivedDateTime desc,hasAttachments&$filter=receivedDateTime le 2024-07-22T18:00:00Z and hasAttachments eq true&$top=10&$expand=attachments";
                    graphtoken objtoken = new graphtoken();
                objtoken = generateGraphAccesstoken();

                if (objtoken.status)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                        HttpResponseMessage response = await client.GetAsync(messagesUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            JObject jsonResponse = JObject.Parse(responseBody);
                            dbconn objdbconn = new dbconn();
                            cmnfunctions objcmnfunctions = new cmnfunctions();
                            DataTable dt_datatable;
                            string msSQL, lsbusinessunit_gid, lsbusinessunit_name, lsassigned_manager, lsassigned_manager_gid, lsactivity_name, lsactivity_gid;
                            int mnResult;
                            string msmailattachment_gid, lsattachment_flag;
                            string msGetSRQGID, msGetGIDs;
                            foreach (var item in jsonResponse["value"])
                            {
                                string sentDateString = item["sentDateTime"].ToString();
                               DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(sentDateString, null, System.Globalization.DateTimeStyles.RoundtripKind);

                                    // Format the DateTimeOffset to desired string format
                                    string formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");

                                string fromId = item["from"]["emailAddress"]["address"].ToString();
                                string profilePictureUrl = jsonResponse["photo"]?["@odata.mediaEditLink"]?.ToString();
                                string messageId = item["id"].ToString();
                                string subject = item["subject"].ToString();
                                string body = item["body"]["content"].ToString();
                                //string fromName = item["from"]["emailAddress"]["name"].ToString();
                                string bodyPreview = item["bodyPreview"]?.ToString()?.Replace("'", "")?.Trim() ?? "";
                                string messageNo = item["@odata.etag"].ToString();
                                //string formattedDate = sentDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                string fromName = GetRecipientEmailAddresses(item["toRecipients"]);
                                    string cc = GetRecipientEmailAddresses(item["ccRecipients"]);
                                string bcc = GetRecipientEmailAddresses(item["bccRecipients"]);
                                string parentFolderId = item["parentFolderId"].ToString();
                                string conversationId = item["conversationId"].ToString();
                                string conversationIndex = item["conversationIndex"].ToString();
                                string isRead = item["isRead"].ToString();
                                string lscompany_code;
                                string hasAttachments = item["hasAttachments"].ToString();
                                msSQL = " select inbox_id  from crm_trn_toutlookinbox where inbox_id = '" + messageId + "'";
                                dt_datatable = objdbconn.GetDataTable(msSQL);
                                if (dt_datatable.Rows.Count == 0)
                                {

                                    if (hasAttachments == "True")
                                    {
                                        lsattachment_flag = "Y";
                                    }
                                    else
                                    {
                                        lsattachment_flag = "N";
                                    }
                                    msSQL = " select company_code from  adm_mst_tcompany limit 1";
                                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                                    msSQL = " INSERT INTO crm_trn_toutlookinbox(" +
                                            " inbox_id," +
                                            " from_id," +
                                            " to_id," +
                                            " cc," +
                                            " bcc," +
                                            " subject," +
                                            " body," +
                                            " sent_date," +
                                            " read_flag," +
                                            " attachement_flag," +
                                            " parentfolder_id," +
                                            " conversation_id ," +
                                            " conversation_index ," +
                                            " company_code ," +
                                            " integrated_gmail )" +
                                            " VALUES (" +
                                            "'" + messageId + "'," +
                                            "'" + fromId + "'," +
                                            "'" + fromName + "'," +
                                            "'" + cc + "'," +
                                            "'" + bcc + "'," +
                                            "'" + subject.Replace("'", "''") + "'," +
                                            "'" + body.Replace("'", "''") + "'," +
                                            "'" + formattedDateString + "'," +
                                            "'" + isRead + "'," +
                                            "'" + lsattachment_flag + "'," +
                                            "'" + parentFolderId + "'," +
                                            "'" + conversationId + "'," +
                                            "'" + conversationIndex + "'," +
                                            "'" + lscompany_code + "'," +
                                            "'" + lsemployee_mailid + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {

                                        if (hasAttachments == "True")
                                        {
                                            msSQL = "SELECT a.company_code FROM adm_mst_tcompany a";
                                            lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                                            

                                                foreach (var attachment in item["attachments"])
                                            {
                                                string attachmentName = attachment["name"].ToString();
                                                //string fileTablePath = temp_path + "/" + attachmentName.Replace("'", @"\'");
                                                byte[] contentBytes = Convert.FromBase64String(attachment["contentBytes"].ToString());
                                                    string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                                                    string lsfile_gid = msdocument_gid;
                                                    string FileExtension = Path.GetExtension(attachmentName).ToLowerInvariant();
                                                    lsfile_gid = lsfile_gid + FileExtension;

                                                    string logFilePath = ConfigurationManager.AppSettings["filepathoutlook"];

                                                    // Check if the directory exists and create it if it does not
                                                    if (!Directory.Exists(logFilePath))
                                                    {
                                                        Directory.CreateDirectory(logFilePath);
                                                    }
                                                    await SaveAttachmentToFile(contentBytes, lsfile_gid, logFilePath);

                                                    msSQL = " INSERT INTO crm_trn_toutlookinboxattachement (" +
                                                        " inbox_id," +
                                                        " attachment_id," +
                                                        " original_filename," +
                                                        " modified_filename," +
                                                        " file_path," +
                                                        " created_by," +
                                                        " created_date) " +
                                                        "VALUES (" +
                                                        " '" + messageId + "'," +
                                                        " '" + attachment["id"].ToString() + "'," +
                                                        " '" + attachmentName.Replace("'", "") + "'," +
                                                        " '" + lsfile_gid + "'," +
                                                         " '" + ConfigurationManager.AppSettings["dbpath_outlook"].ToString() + lsfile_gid + "'," +
                                                        " '" + user_gid + "'," +
                                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                if (mnResult == 0)
                                                {

                                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }


                                }


                            }
                        }
                        else
                        {
                            // Retrieve error content from the response
                            string errorContent = await response.Content.ReadAsStringAsync();

                            // Log the error
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                                        "***********" + errorContent +
                                                        "**************Query****" + msSQL +
                                                        "*******Apiref********",
                                                        "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        }

                    }
                }
                objresult.status = true;
                }
                else
                {
                    objresult.status = false;
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*********** ExecuteDaReadEmailsOutlookmail token issue" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");


                }
            }
            catch (Exception e)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + e.ToString() + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                objresult.status = false;
                objresult.message = e.Message;
            }
            return objresult;
        }
        public async Task SaveAttachmentToFile(byte[] contentBytes, string fileName, string directoryPath)
        {
            try
            {
                string filePath = Path.Combine(directoryPath, fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileStream.WriteAsync(contentBytes, 0, contentBytes.Length);
                    }
                }
          
            }
            catch (Exception e)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + e.ToString() , "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        static string GetRecipientEmailAddresses(JToken recipients)
        {
            if (recipients is JArray array && array.Count > 0)
            {
                var emailAddresses = array
                    .Where(recipient => recipient is JObject obj &&
                                        obj.ContainsKey("emailAddress") &&
                                        obj["emailAddress"] is JObject emailObj &&
                                        emailObj.ContainsKey("address"))
                    .Select(recipient => recipient["emailAddress"]["address"].ToString())
                    .ToArray();

                return string.Join(",", emailAddresses);
            }
            return "";
        }

        public void DaGmailAPIOutlookinboxSummary(MdlGmailCampaign values, string user_gid)
        {


            try
            {

                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL);
                //string formattedEmails = string.Join(",", integrated_gmail.Split(',').Select(email => $"'{email.Trim()}'"));

                //msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time," +
                //    " cc, subject, body, attachement_flag,read_flag,integrated_gmail FROM crm_trn_tgmailinbox where inbox_status is Null and  integrated_gmail in (" + formattedEmails + ") ORDER BY s_no DESC";
                //dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time," +
                    " cc,bcc, subject, body, attachement_flag,read_flag, COALESCE(NULLIF(to_id, ''), integrated_gmail) AS integrated_gmail  FROM crm_trn_toutlookinbox where inbox_status is Null " +
                    " AND ( FIND_IN_SET('" + integrated_gmail + "', to_id) > 0 OR FIND_IN_SET('" + integrated_gmail + "',cc) > 0  OR FIND_IN_SET('" + integrated_gmail + "',bcc) > 0 " +
                    " OR integrated_gmail='" + integrated_gmail + "'  ) ORDER BY s_no DESC";
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
                       bcc = row["bcc"].ToString(),
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
        public void DaGetOutlookInboxAttchement(string inbox_id, MdlGmailCampaign values)
        {

            try
            {
                msSQL = "select s_no, inbox_id, original_filename, modified_filename, file_path from crm_trn_toutlookinboxattachement where inbox_id='" + inbox_id + "' ";
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

        public void DaOutlookGetReplyMail(string inbox_id, MdlGmailCampaign values)
        {
            try
            {
                string msSQL = "SELECT r.s_no, r.reply_id, r.inbox_id, r.from_id, r.cc, DATE_FORMAT(r.sent_date, '%b %e, %Y %h:%i %p') AS sent_date, DATE_FORMAT(r.sent_date, '%h:%i %p') AS sent_time, r.bcc, r.subject, r.body, r.attachement_flag, r.both_body, a.original_filename, a.modified_filename, a.file_path " +
                               "FROM crm_trn_toutlookinboxreply r " +
                               "LEFT JOIN crm_trn_toutlookreplyattachment a ON r.reply_id = a.reply_id " +
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
        public void DaGetOutlookInboxCustomerDetails(string email_id, MdlGmailCampaign values, string user_gid)
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
        public void DaGetOutlookForwardMail(string inbox_id, MdlGmailCampaign values)
        {
            try
            {
                // Step 1: Get forward mails with attachment flag 'N'
                string msSQL1 = $@"SELECT s_no, forward_id, reply_id, inbox_id, to_id, cc, sent_date, bcc, subject, body, attachement_flag, both_body 
                            FROM crm_trn_toutlookinboxreplyforward  
                            WHERE inbox_id = '{inbox_id}' AND attachement_flag = 'N'  order by s_no desc";
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
                            FROM crm_trn_toutlookinboxreplyforward 
                            WHERE inbox_id = '{inbox_id}' AND attachement_flag = 'Y'  order by s_no desc";
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
                                    FROM crm_trn_toutlookinboxattachement 
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
                                    FROM crm_trn_toutlookreplyattachment 
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
        public async Task DaOutlookInboxStatusUpdate(replymail_list values,string user_gid)
        {
            try
            {
            
                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    if (objtoken.status)
                    {
                     
                            var client = new RestClient("https://graph.microsoft.com/v1.0");
                        string messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/" + values.inbox_id ;


                        // Create a RestRequest for the PATCH method with the required endpoint
                         var request = new RestRequest(messagesUrl, Method.PATCH);

                            // Add the Authorization header
                            request.AddHeader("Authorization", objtoken.access_token);

                            // Add the Content-Type header
                            request.AddHeader("Content-Type", "application/json");

                            // Add the JSON body
                            var body = new
                            {
                                isRead = true
                            };
                            request.AddJsonBody(body);

                            // Execute the request and get the response
                            IRestResponse response = client.Execute(request);

                            // Check if the response is successful
                            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                            {


                            msSQL = "UPDATE crm_trn_toutlookinbox SET read_flag = 'True' WHERE inbox_id = '" + values.inbox_id + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 1)
                            {
                                objcmnfunctions.LogForAudit($"Error: Failed to update read status of email {values.inbox_id}.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                            }

                            }
                            else
                            {
                          

                                // Log the error
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                                            "***********" +
                                                            "**************Query****" + msSQL +
                                                            "*******Apiref********",
                                                            "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                            }

                        }
                        values.status = true;
                    }
                 
                
                else
                {
                    values.status = false;
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*********** ExecuteDaReadEmailsOutlookmail token issue" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");


                }
               
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error occurred while processing emails.";
                objcmnfunctions.LogForAudit($"Error in DaOutlookInboxStatusUpdate: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task DaOutlookReplyInboxMail(replymail_list values, string user_gid)
        {
            try
            {

                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {


                    msSQL = "select body,subject, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,from_id from  crm_trn_toutlookinbox where inbox_id='" + values.inbox_id + "' limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsbody = objOdbcDataReader["body"].ToString();
                        lssent_date = objOdbcDataReader["sent_date"].ToString();
                        lssubject = objOdbcDataReader["subject"].ToString();
                        lsfrom_id = objOdbcDataReader["from_id"].ToString();
                    }
                    objOdbcDataReader.Close();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    // Initialize the RestClient with the base URL
                    var client = new RestClient("https://graph.microsoft.com/v1.0");
                    string messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/{values.inbox_id}/reply" ;

                    // Create a RestRequest for the POST method with the required endpoint
                    var request = new RestRequest(messagesUrl, Method.POST);

                    // Add the Authorization header
                    request.AddHeader("Authorization", $"Bearer {objtoken.access_token}");

                    // Add the Content-Type header
                    request.AddHeader("Content-Type", "application/json");
                    string htmlContent = $@"
                                    {values.emailBody.Replace("'", "''")}
                                    <br><br>
                                    On {lssent_date}, {lsfrom_id} wrote:
                                    <br><br>
                                    Subject: {lssubject.Replace("'", "''")}
                                    <br><br>
                                    {lsbody.Replace("'", "''")}
                                   
                                ";

                    // Split the email addresses by comma and trim whitespace
                    var toAddresses = values.replytoid?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
      .Select(email => new { emailAddress = new { address = email.Trim() } })
      .ToArray() ?? new object[0];

                    var ccAddresses = values.replyccid?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(email => new { emailAddress = new { address = email.Trim() } })
                        .ToArray();

                    var bccAddresses = values.replybccid?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(email => new { emailAddress = new { address = email.Trim() } })
                        .ToArray();

                    var message = new
                    {
                        subject = $"Re: {lssubject.Replace("'", "''")}",
                        body = new
                        {
                            contentType = "HTML",
                            content = htmlContent
                        },
                        toRecipients = toAddresses
                    };

                    // Conditionally add properties to the dictionary
                    var messageObject = new Dictionary<string, object>
                    {
                        ["subject"] = $"Re: {lssubject.Replace("'", "''")}",
                        ["body"] = new
                        {
                            contentType = "HTML",
                            content = htmlContent
                        },
                        ["toRecipients"] = toAddresses
                    };

                    if (ccAddresses != null && ccAddresses.Any())
                    {
                        messageObject["ccRecipients"] = ccAddresses;
                    }

                    if (bccAddresses != null && bccAddresses.Any())
                    {
                        messageObject["bccRecipients"] = bccAddresses;
                    }

                    // Wrap in the final object
                    var body = new
                    {
                        message = messageObject
                    };

                    request.AddJsonBody(body);

                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);
                    // Check if the response is successful
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        await Task.Delay(10000);
                        using (HttpClient client2 = new HttpClient())
                        {
                            string messagesUrl1 = $"https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + $"/messages?$top=1";


                            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                            HttpResponseMessage response2 = await client2.GetAsync(messagesUrl1);

                            if (response2.StatusCode == HttpStatusCode.OK)
                            {
                                string responseBody = await response2.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                foreach (var item in jsonResponse["value"])
                                {
                                    messageId = item["id"].ToString();
                                }
                                string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                // Insert into crm_trn_tgmailinboxreply
                                msSQL = "INSERT INTO crm_trn_toutlookinboxreply (" +
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
                                 "'" + messageId + "', " +
                                "'" + values.replytoid + "', " +
                                "'" + $"Re: {lssubject.Replace("'", "''")}" + "', " +
                                "'" + htmlContent.Replace("'", "''") + "', " +
                                "'" + values.replyccid + "', " +
                                "'" + values.replybccid + "', " +
                                "'" + values.emailBody.Replace("'", "''") + "', " +
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
                                values.message = "Error: Reply id not Get.";
                                values.status = false;
                                objcmnfunctions.LogForAudit("Error: Failed to send reply mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
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
                objcmnfunctions.LogForAudit($"Error in DaOutlookReplyInboxMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }

        public async Task DaOutlookReplyAllWithAttachment(HttpRequest httpRequest, results values, string user_gid)
        {
            try
            {

  
                string inboxId = httpRequest.Form["inbox_id"];
                string email_Body = httpRequest.Form["emailBody"];
                string replytoid = httpRequest.Form["replytoid"];
                string replyccid = httpRequest.Form["replyccid"];
                string replybccid = httpRequest.Form["replybccid"];
              
          
                var attachments = httpRequest.Files;
                List<string> savedFileNames = new List<string>();
                List<string> savedFileNamesdb = new List<string>(); // Track renamed file paths
                List<string> savedFileNamesdbenc = new List<string>();
                List<string> fileNames = new List<string>();
                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {
                    foreach (string fileName in attachments.AllKeys)
                {
                    var file = attachments[fileName];
                    if (file != null && file.ContentLength > 0)
                    {
                        // Generate a unique file name
                        string uniqueFileName = objcmnfunctions.GetMasterGID("UPLF");
                        string fileExtension = Path.GetExtension(file.FileName);
                        string logFilePath = ConfigurationManager.AppSettings["filepathoutlook"];

                        // Check if the directory exists and create it if it does not
                        if (!Directory.Exists(logFilePath))
                        {
                            Directory.CreateDirectory(logFilePath);
                        }
                        // Save the file to specified directory with the unique name
                        string saveFilePath = ConfigurationManager.AppSettings["filepathoutlook"];
                        string savePath = Path.Combine(saveFilePath, $"{uniqueFileName}{fileExtension}");
                        file.SaveAs(savePath);

                        savedFileNames.Add(savePath); // Add path with original file name
                        savedFileNamesdb.Add(ConfigurationManager.AppSettings["dbpath_outlook"] + uniqueFileName + fileExtension); // Add path with renamed file name
                    }
                }
                foreach (string filePath in savedFileNames)
                {
                    byte[] fileBytes = FileIO.ReadAllBytes(filePath);
                    string base64String = Convert.ToBase64String(fileBytes);

                    var fileName = Path.GetFileName(filePath);

                    savedFileNamesdbenc.Add(base64String);
                    fileNames.Add(fileName);
                }

                for (int i = 0; i < savedFileNamesdbenc.Count; i++)
                {
                    string base64String = savedFileNamesdbenc[i];
                    string fileName = fileNames[i];

                    // Do something with the Base64 string and file name
                }
            


                    msSQL = "select body,subject, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,from_id from  crm_trn_toutlookinbox where inbox_id='" + inboxId + "' limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsbody = objOdbcDataReader["body"].ToString();
                        lssent_date = objOdbcDataReader["sent_date"].ToString();
                        lssubject = objOdbcDataReader["subject"].ToString();
                        lsfrom_id = objOdbcDataReader["from_id"].ToString();
                    }
                    objOdbcDataReader.Close();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    var client = new RestClient("https://graph.microsoft.com/v1.0");
                    string messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/{inboxId}/reply";

                    var request = new RestRequest(messagesUrl, Method.POST);
                    request.AddHeader("Authorization", $"Bearer {objtoken.access_token}");
                    request.AddHeader("Content-Type", "application/json");

                    string htmlContent = $@"
                                            {email_Body.Replace("'", "''")}
                                            <br><br>
                                            On {lssent_date}, {lsfrom_id} wrote:
                                            <br><br>
                                            Subject: {lssubject.Replace("'", "''")}
                                            <br><br>
                                            {lsbody.Replace("'", "''")}
                                        ";

                    var toAddresses = replytoid?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(email => new { emailAddress = new { address = email.Trim() } })
                        .ToArray() ?? new object[0];

                    var ccAddresses = replyccid?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(email => new { emailAddress = new { address = email.Trim() } })
                        .ToArray();

                    var bccAddresses = replybccid?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(email => new { emailAddress = new { address = email.Trim() } })
                        .ToArray();

                    var messageObject = new Dictionary<string, object>
                    {
                        ["subject"] = $"Re: {lssubject.Replace("'", "''")}",
                        ["body"] = new
                        {
                            contentType = "HTML", // Changed to "Text" from "HTML"
                            content = $" {htmlContent}" // Changed to match the JSON body format
                        },
                        ["toRecipients"] = toAddresses
                    };

                    if (ccAddresses != null && ccAddresses.Any())
                    {
                        messageObject["ccRecipients"] = ccAddresses;
                    }

                    if (bccAddresses != null && bccAddresses.Any())
                    {
                        messageObject["bccRecipients"] = bccAddresses;
                    }

                    if (savedFileNamesdbenc != null && savedFileNamesdbenc.Count > 0)
                    {
                        var fileAttachments = new List<Dictionary<string, object>>();
                        for (int i = 0; i < savedFileNamesdbenc.Count; i++)
                        {
                            fileAttachments.Add(new Dictionary<string, object>
                            {
                                ["@odata.type"] = "#microsoft.graph.fileAttachment",
                                ["name"] = fileNames[i],
                                ["contentBytes"] = savedFileNamesdbenc[i]
                            });
                        }
                        messageObject["attachments"] = fileAttachments;
                    }

                    var body = new
                    {
                        message = messageObject
                    };

                    request.AddJsonBody(body);

                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        await Task.Delay(10000);
                        using (HttpClient client2 = new HttpClient())
                        {
                            string messagesUrl1 = $"https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + $"/messages?$top=1";


                            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                            HttpResponseMessage response2 = await client2.GetAsync(messagesUrl1);

                            if (response2.StatusCode == HttpStatusCode.OK)
                            {
                                string responseBody = await response2.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                foreach (var item in jsonResponse["value"])
                                {
                                    messageId = item["id"].ToString();
                                }
                                string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                // Insert into crm_trn_tgmailinboxreply
                                msSQL = "INSERT INTO crm_trn_toutlookinboxreply (" +
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
                                 "'" + inboxId + "', " +
                                 "'" + messageId + "', " +
                                "'" + replytoid + "', " +
                                "'" + $"Re: {lssubject.Replace("'", "''")}" + "', " +
                                "'" + htmlContent.Replace("'", "''") + "', " +
                                "'" + replyccid + "', " +
                                "'" + replybccid + "', " +
                                "'" + email_Body.Replace("'", "''") + "', " +
                                  "'Y', " +
                                "'" + formattedDateString + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    // Insert into crm_trn_tgmailreplyattachment
                                    foreach (var filePath in savedFileNamesdb)
                                    {
                                        string originalFileName = Path.GetFileName(filePath);
                                        string modifiedFileName = originalFileName; // Adjust if needed
                                        string insertQuery = "INSERT INTO crm_trn_toutlookreplyattachment (reply_id, inbox_id, original_filename, modified_filename, file_path) " +
                                                             $"VALUES ('{messageId}', '{inboxId}', '{originalFileName}', '{modifiedFileName}', '{filePath}')";
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
                            // If sending failed
                            values.message = "Error: Reply id not Get.";
                            values.status = false;
                            objcmnfunctions.LogForAudit("Error: Failed to send reply mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
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
                values.message = "Error: Failed to send reply mail.";
                values.status = false;
                objcmnfunctions.LogForAudit(
                                "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public async Task DaOutlookReplyOrForwardInboxMail(forwardmail_list values, string user_gid)
        {
            try
            {
                string messagesUrl;

                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {


                    msSQL = "select body,subject, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,from_id from  crm_trn_toutlookinbox where inbox_id='" + values.inbox_id + "' limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsbody = objOdbcDataReader["body"].ToString();
                        lssent_date = objOdbcDataReader["sent_date"].ToString();
                        lssubject = objOdbcDataReader["subject"].ToString();
                        lsfrom_id = objOdbcDataReader["from_id"].ToString();
                    }
                    objOdbcDataReader.Close();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    // Initialize the RestClient with the base URL
                    var client = new RestClient("https://graph.microsoft.com/v1.0");
                    if (values.reply_id == "No")
                    {
                         messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/{values.inbox_id}/forward";
                    }
                    else
                    {
                         messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/{values.reply_id}/forward";
                    }

                    // Create a RestRequest for the POST method with the required endpoint
                    var request = new RestRequest(messagesUrl, Method.POST);

                    // Add the Authorization header
                    request.AddHeader("Authorization", $"Bearer {objtoken.access_token}");

                    // Add the Content-Type header
                    request.AddHeader("Content-Type", "application/json");
                    string htmlContent = $@"
                                    {values.emailBody.Replace("'", "''")}
                                    <br><br>
                                    On {lssent_date}, {lsfrom_id} wrote:
                                    <br><br>
                                    Subject: {lssubject.Replace("'", "''")}
                                    <br><br>
                                    {lsbody.Replace("'", "''")}
                                   
                                ";

                    // Split the email addresses by comma and trim whitespace
                    var toAddresses = values.forwardto?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(email => new { emailAddress = new { address = email.Trim() } })
                      .ToArray() ?? new object[0];


                    var message = new
                    {
                        subject = $"Fwd: {lssubject.Replace("'", "''")}",
                        body = new
                        {
                            contentType = "HTML",
                            content = htmlContent
                        },
                        toRecipients = toAddresses
                    };

                    // Conditionally add properties to the dictionary
                    var messageObject = new Dictionary<string, object>
                    {
                        ["subject"] = $"Fwd: {lssubject.Replace("'", "''")}",
                        ["body"] = new
                        {
                            contentType = "HTML",
                            content = htmlContent
                        },
                        ["toRecipients"] = toAddresses
                    };

               

                    // Wrap in the final object
                    var body = new
                    {
                        message = messageObject
                    };

                    request.AddJsonBody(body);

                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);
                    // Check if the response is successful
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        await Task.Delay(10000);
                        using (HttpClient client2 = new HttpClient())
                        {
                            string messagesUrl1 = $"https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + $"/messages?$top=1";


                            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                            HttpResponseMessage response2 = await client2.GetAsync(messagesUrl1);

                            if (response2.StatusCode == HttpStatusCode.OK)
                            {
                                string responseBody = await response2.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                foreach (var item in jsonResponse["value"])
                                {
                                    messageId = item["id"].ToString();
                                }
                                string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                string msSQL = "INSERT INTO crm_trn_toutlookinboxreplyforward (" +
                                                                       "inbox_id, " +
                                                                       "reply_id, " +
                                                                       "forward_id, " +
                                                                       "to_id, " +
                                                                       "subject, " +
                                                                       "both_body, " +
                                                                       "body, " +
                                                                       "attachement_flag, " +
                                                                       "sent_date) " +
                                                                       "VALUES (" +
                                                                       "'" + values.inbox_id + "', " +
                                                                      "'" + values.reply_id + "', " +
                                                                       "'" + messageId + "', " +
                                                                       "'" + values.forwardto + "', " +
                                                                       "'" + $"Fwd: {lssubject.Replace("'", "''")}" + "', " +
                                                                       "'" + htmlContent.Replace("'", "''") + "', " +
                                                                       "'" + values.emailBody.Replace("'", "''") + "', " +
                                                                       "'N', " +
                                                                       "'" + formattedDateString + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                                values.message = "Error: Forward id not Get.";
                                values.status = false;
                                objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
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
                values.message = "Error While Forward Mail !!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaOutlookReplyOrForwardInboxMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }
        public async Task DaOutlookReplyOrForwardInboxMailWithAttach(forwardmail_list values, string user_gid)
        {
            try
            {
                string messagesUrl;
                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {


                string msSQL = values.reply_id == "No"
                    ? $"SELECT body,subject, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,from_id FROM crm_trn_toutlookinbox WHERE inbox_id='{values.inbox_id}' LIMIT 1"
                    : $"SELECT body,subject, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,from_id FROM crm_trn_toutlookinboxreply WHERE reply_id='{values.reply_id}' LIMIT 1";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsbody = objOdbcDataReader["body"].ToString();
                    lssent_date = objOdbcDataReader["sent_date"].ToString();
                     lssubject = objOdbcDataReader["subject"].ToString();
                        lsfrom_id = objOdbcDataReader["from_id"].ToString();
                    }
                objOdbcDataReader.Close();

                string emailBody = values.emailBody; // Assuming values.emailBody is already in HTML format
                string original_body = lsbody; // Assuming original_body is already in HTML format
                string sent_date = lssent_date;


                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    var client = new RestClient("https://graph.microsoft.com/v1.0");
                    if (values.reply_id == "No")
                    {
                        messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/{values.inbox_id}/forward";
                    }
                    else
                    {
                        messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/{values.reply_id}/forward";
                    }

                    var request = new RestRequest(messagesUrl, Method.POST);
                    request.AddHeader("Authorization", $"Bearer {objtoken.access_token}");
                    request.AddHeader("Content-Type", "application/json");

                    string htmlContent = $@"
                                            {values.emailBody}
                                            <br><br>
                                            On {lssent_date}, {lsfrom_id} wrote:
                                            <br><br>
                                            Subject: {lssubject}
                                            <br><br>
                                            {lsbody}
                                        ";

                    var toAddresses = values.forwardto?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(email => new { emailAddress = new { address = email.Trim() } })
                        .ToArray() ?? new object[0];

                    var messageObject = new Dictionary<string, object>
                    {
                        ["subject"] = $"Fwd: {lssubject}",
                        ["body"] = new
                        {
                            contentType = "HTML", // Changed to "Text" from "HTML"
                            content = $" {htmlContent}" // Changed to match the JSON body format
                        },
                        ["toRecipients"] = toAddresses
                    };
                    if (values.reply_id == "No")
                    {
                        msSQL = "SELECT s_no, inbox_id, original_filename, modified_filename, file_path FROM crm_trn_toutlookinboxattachement where inbox_id='" + values.inbox_id + "'  ORDER BY  s_no asc";
                    }
                    else
                    {
                        msSQL = "SELECT s_no, inbox_id, original_filename, modified_filename, file_path FROM crm_trn_toutlookreplyattachment where reply_id='" + values.reply_id + "'  ORDER BY  s_no asc";

                    }
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count > 0)
                    {
                        var fileAttachments = new List<Dictionary<string, object>>();
                        foreach (DataRow dt in dt_datatable.Rows)
                        {


                            string filepath = ConfigurationManager.AppSettings["fwdfilepath"] + dt["file_path"].ToString();
                            byte[] fileBytes = FileIO.ReadAllBytes(filepath);
                            string base64String = Convert.ToBase64String(fileBytes);

                           
                            fileAttachments.Add(new Dictionary<string, object>
                            {
                                ["@odata.type"] = "#microsoft.graph.fileAttachment",
                                ["name"] = dt["original_filename"].ToString(),
                                ["contentBytes"] = base64String
                            });

                        }
                        messageObject["attachments"] = fileAttachments;
                    }

                 

                    var body = new
                    {
                        message = messageObject
                    };

                    request.AddJsonBody(body);

                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        await Task.Delay(10000);
                        using (HttpClient client2 = new HttpClient())
                        {
                            string messagesUrl1 = $"https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + $"/messages?$top=1";


                            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                            HttpResponseMessage response2 = await client2.GetAsync(messagesUrl1);

                            if (response2.StatusCode == HttpStatusCode.OK)
                            {
                                string responseBody = await response2.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                foreach (var item in jsonResponse["value"])
                                {
                                    messageId = item["id"].ToString();
                                }
                                string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                // Insert into crm_trn_tgmailinboxreply
                                msSQL = "INSERT INTO crm_trn_toutlookinboxreplyforward (" +
                                "inbox_id, " +
                                "reply_id, " +
                                "forward_id, " +
                                "to_id, " +
                                "subject, " +
                                "both_body, " +
                                "body, " +
                                "attachement_flag, " +
                                "sent_date) " +
                                "VALUES (" +
                                $"'{values.inbox_id}', " +
                                $"'{values.reply_id}', " +
                                $"'{messageId}', " +
                                $"'{values.forwardto}', " +
                                $"'{$"Fwd: {lssubject}"}', " +
                                $"'{htmlContent}', " +
                                $"'{emailBody}', " +
                                $"'{values.attachement_flag}', " +
                                $"'{formattedDateString}')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                                values.message = "Error: Forward id not Get.";
                                values.status = false;
                                objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
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
                objcmnfunctions.LogForAudit($"Error in DaOutlookReplyOrForwardInboxMailWithAttach: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }

        public async Task DaOutlookForwardOfFwdMail(forwardoffwdmail_list values, string user_gid)
        {
            try
            {
                string messagesUrl;

                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {


                    msSQL = "select body,subject,DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,to_id from  crm_trn_toutlookinboxreplyforward where forward_id='" + values.forward_id + "' limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsbody = objOdbcDataReader["body"].ToString();
                        lssent_date = objOdbcDataReader["sent_date"].ToString();
                        lssubject = objOdbcDataReader["subject"].ToString();
                        lsfrom_id = objOdbcDataReader["to_id"].ToString();
                    }
                    objOdbcDataReader.Close();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    // Initialize the RestClient with the base URL
                    var client = new RestClient("https://graph.microsoft.com/v1.0");

                        messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/{values.forward_id}/forward";

                    // Create a RestRequest for the POST method with the required endpoint
                    var request = new RestRequest(messagesUrl, Method.POST);

                    // Add the Authorization header
                    request.AddHeader("Authorization", $"Bearer {objtoken.access_token}");

                    // Add the Content-Type header
                    request.AddHeader("Content-Type", "application/json");
                    string htmlContent = $@"
                                    {values.emailBody}
                                    <br><br>
                                    On {lssent_date}, {lsfrom_id} wrote:
                                    <br><br>
                                    Subject: {lssubject}
                                    <br><br>
                                    {lsbody}
                                   
                                ";

                    // Split the email addresses by comma and trim whitespace
                    var toAddresses = values.forwardto?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(email => new { emailAddress = new { address = email.Trim() } })
                      .ToArray() ?? new object[0];


                    var message = new
                    {
                        subject = $"Fwd: {lssubject}",
                        body = new
                        {
                            contentType = "HTML",
                            content = htmlContent
                        },
                        toRecipients = toAddresses
                    };

                    // Conditionally add properties to the dictionary
                    var messageObject = new Dictionary<string, object>
                    {
                        ["subject"] = $"Fwd: {lssubject}",
                        ["body"] = new
                        {
                            contentType = "HTML",
                            content = htmlContent
                        },
                        ["toRecipients"] = toAddresses
                    };



                    // Wrap in the final object
                    var body = new
                    {
                        message = messageObject
                    };

                    request.AddJsonBody(body);

                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);
                    // Check if the response is successful
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        await Task.Delay(10000);
                        using (HttpClient client2 = new HttpClient())
                        {
                            string messagesUrl1 = $"https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + $"/messages?$top=1";


                            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                            HttpResponseMessage response2 = await client2.GetAsync(messagesUrl1);

                            if (response2.StatusCode == HttpStatusCode.OK)
                            {
                                string responseBody = await response2.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                foreach (var item in jsonResponse["value"])
                                {
                                    messageId = item["id"].ToString();
                                }
                                string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                string msSQL = "INSERT INTO crm_trn_toutlookinboxreplyforward (" +
                                                                       "inbox_id, " +
                                                                       "reply_id, " +
                                                                       "forward_id, " +
                                                                       "fwd_id, " +
                                                                       "to_id, " +
                                                                       "subject, " +
                                                                       "both_body, " +
                                                                       "body, " +
                                                                       "attachement_flag, " +
                                                                       "sent_date) " +
                                                                       "VALUES (" +
                                                                       "'" + values.inbox_id + "', " +
                                                                      "'" + values.reply_id + "', " +
                                                                      "'" + values.forward_id + "', " +
                                                                       "'" + messageId + "', " +
                                                                       "'" + values.forwardto + "', " +
                                                                       "'" + $"Fwd: {lssubject}" + "', " +
                                                                       "'" + htmlContent + "', " +
                                                                       "'" + values.emailBody + "', " +
                                                                       "'N', " +
                                                                       "'" + formattedDateString + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                                values.message = "Error: Forward id not Get.";
                                values.status = false;
                                objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
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
                values.message = "Error While Forward Mail !!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaOutlookReplyOrForwardInboxMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }
        public async Task DaOutlookForwardOfFwdMailWithAttach(forwardoffwdmail_list values, string user_gid)
        {
            try
            {
                string messagesUrl;
                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {


                    msSQL = "select body,subject,DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,to_id from  crm_trn_toutlookinboxreplyforward where forward_id='" + values.forward_id + "' limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsbody = objOdbcDataReader["body"].ToString();
                        lssent_date = objOdbcDataReader["sent_date"].ToString();
                        lssubject = objOdbcDataReader["subject"].ToString();
                        lsfrom_id = objOdbcDataReader["to_id"].ToString();
                    }
                    objOdbcDataReader.Close();

                    string emailBody = values.emailBody; // Assuming values.emailBody is already in HTML format
                    string original_body = lsbody; // Assuming original_body is already in HTML format
                    string sent_date = lssent_date;


                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    var client = new RestClient("https://graph.microsoft.com/v1.0");

                    messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/{values.forward_id}/forward";

                    var request = new RestRequest(messagesUrl, Method.POST);
                    request.AddHeader("Authorization", $"Bearer {objtoken.access_token}");
                    request.AddHeader("Content-Type", "application/json");

                    string htmlContent = $@"
                                            {values.emailBody.Replace("'", "''")}
                                            <br><br>
                                            On {lssent_date}, {lsfrom_id} wrote:
                                            <br><br>
                                            Subject: {lssubject.Replace("'", "''")}
                                            <br><br>
                                            {lsbody.Replace("'", "''")}
                                        ";

                    var toAddresses = values.forwardto?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(email => new { emailAddress = new { address = email.Trim() } })
                        .ToArray() ?? new object[0];

                    var messageObject = new Dictionary<string, object>
                    {
                        ["subject"] = $"Fwd: {lssubject.Replace("'", "''")}",
                        ["body"] = new
                        {
                            contentType = "HTML", // Changed to "Text" from "HTML"
                            content = $" {htmlContent}" // Changed to match the JSON body format
                        },
                        ["toRecipients"] = toAddresses
                    };
                    if (values.reply_id == "No")
                    {
                        msSQL = "SELECT s_no, inbox_id, original_filename, modified_filename, file_path FROM crm_trn_toutlookinboxattachement where inbox_id='" + values.inbox_id + "'  ORDER BY  s_no asc";
                    }
                    else
                    {
                        msSQL = "SELECT s_no, inbox_id, original_filename, modified_filename, file_path FROM crm_trn_toutlookreplyattachment where reply_id='" + values.reply_id + "'  ORDER BY  s_no asc";

                    }
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count > 0)
                    {
                        var fileAttachments = new List<Dictionary<string, object>>();
                        foreach (DataRow dt in dt_datatable.Rows)
                        {


                            string filepath = ConfigurationManager.AppSettings["fwdfilepath"] + dt["file_path"].ToString();
                            byte[] fileBytes = FileIO.ReadAllBytes(filepath);
                            string base64String = Convert.ToBase64String(fileBytes);


                            fileAttachments.Add(new Dictionary<string, object>
                            {
                                ["@odata.type"] = "#microsoft.graph.fileAttachment",
                                ["name"] = dt["original_filename"].ToString(),
                                ["contentBytes"] = base64String
                            });

                        }
                        messageObject["attachments"] = fileAttachments;
                    }



                    var body = new
                    {
                        message = messageObject
                    };

                    request.AddJsonBody(body);

                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        await Task.Delay(10000);
                        using (HttpClient client2 = new HttpClient())
                        {
                            string messagesUrl1 = $"https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + $"/messages?$top=1";


                            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                            HttpResponseMessage response2 = await client2.GetAsync(messagesUrl1);

                            if (response2.StatusCode == HttpStatusCode.OK)
                            {
                                string responseBody = await response2.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                foreach (var item in jsonResponse["value"])
                                {
                                    messageId = item["id"].ToString();
                                }
                                string formattedDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                string msSQL = "INSERT INTO crm_trn_toutlookinboxreplyforward (" +
                                                                 "inbox_id, " +
                                                                 "reply_id, " +
                                                                 "forward_id, " +
                                                                 "fwd_id, " +
                                                                 "to_id, " +
                                                                 "subject, " +
                                                                 "both_body, " +
                                                                 "body, " +
                                                                 "attachement_flag, " +
                                                                 "sent_date) " +
                                                                 "VALUES (" +
                                                                 "'" + values.inbox_id + "', " +
                                                                "'" + values.reply_id + "', " +
                                                                "'" + values.forward_id + "', " +
                                                                 "'" + messageId + "', " +
                                                                 "'" + values.forwardto + "', " +
                                                                 "'" + $"Fwd: {lssubject.Replace("'", "''")}" + "', " +
                                                                 "'" + htmlContent.Replace("'", "''") + "', " +
                                                                 "'" + values.emailBody.Replace("'", "''") + "', " +
                                                                 "'" + values.attachement_flag + "', " +
                                                                 "'" + formattedDateString + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);                               
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
                                values.message = "Error: Forward id not Get.";
                                values.status = false;
                                objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
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
                objcmnfunctions.LogForAudit($"Error in DaOutlookReplyOrForwardInboxMailWithAttach: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt");
                throw; // Re-throw the exception to be caught by the caller
            }
        }

        //Outlook folder
        public async Task DaGetOutlookFolderDetails(MdlOutlookCampaign values, string user_gid)
        {
            try
            {
                string messagesUrl;
                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {
                 

                    msSQL = " select company_code from  adm_mst_tcompany limit 1";
                    string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                        var response = await client.GetAsync("https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + "/mailFolders?$expand=childFolders");

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var jsonResponse = JObject.Parse(content);
                            var folders = jsonResponse["value"] as JArray;

                            if (folders != null)
                            {
                                foreach (var folder in folders)
                                {
                                    await InsertFolder(folder, lscompany_code, lsemployee_mailid, user_gid);

                                    var childFolders = folder["childFolders"] as JArray;
                                    if (childFolders != null)
                                    {
                                        foreach (var childFolder in childFolders)
                                        {
                                            await InsertFolder(childFolder, lscompany_code, lsemployee_mailid, user_gid, folder["id"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception($"Error fetching mailbox folders: {response.StatusCode}");
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

        private async Task InsertFolder(JToken folder, string companyCode, string lsemployee_mailid, string userGid, string parentFolderId = null, string parentDisplayName = null)
        {
            string folderDisplayName = folder["displayName"].ToString();
            if (parentDisplayName != null)
            {
                folderDisplayName = $"{parentDisplayName}/{folderDisplayName}";
            }

            string msSQL = " select label_id from crm_trn_toutlookfolder where label_id='" + folder["id"].ToString() + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count == 0)
            {
                string msSQLInsert = "INSERT INTO crm_trn_toutlookfolder (" +
                                    "label_id, " +
                                    "label_name, " +
                                    "parent_folder_id, " +
                                    "child_folder_count, " +
                                    "unread_item_count, " +
                                    "total_item_count, " +
                                    "size_in_bytes, " +
                                    "is_hidden, " +
                                    "company_code, " +
                                    "integrated_gmail, " +
                                    "created_by, " +
                                    "created_date) " +
                                    "VALUES (" +
                                    "'" + folder["id"].ToString() + "', " +
                                    "'" + folderDisplayName.Replace("'", "''") + "', " +
                                    "'" + (parentFolderId ?? folder["parentFolderId"]?.ToString()) + "', " +
                                    "'" + folder["childFolderCount"].ToString() + "', " +
                                    "'" + folder["unreadItemCount"].ToString() + "', " +
                                    "'" + folder["totalItemCount"].ToString() + "', " +
                                    "'" + folder["sizeInBytes"].ToString() + "', " +
                                    "'" + folder["isHidden"].ToString() + "', " +
                                    "'" + companyCode + "', " +
                                    "'" + lsemployee_mailid + "', " +
                                    "'" + userGid + "', " +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQLInsert);
            }

            // Handle child folders
            var childFolders = folder["childFolders"] as JArray;
            if (childFolders != null)
            {
                foreach (var childFolder in childFolders)
                {
                    await InsertFolder(childFolder, companyCode, lsemployee_mailid, userGid, folder["id"].ToString(), folderDisplayName);
                }
            }
        }
        public void DaGetOutlookMailFolder(MdlOutlookCampaign values, string user_gid)
        {
            try
            {

                msSQL = " select company_code from  adm_mst_tcompany limit 1 ";
                string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
               string  msSQL1 = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL1);
                msSQL = "select s_no, label_id, label_name, created_by, created_date, company_code,integrated_gmail from  crm_trn_toutlookfolder  where" +
                    " label_name not in ('Archive','Conversation History','Deleted Items','Drafts','Inbox','Outbox','Sent Items','Junk Email','Notes','Tasks') and  " +
                    " company_code='" + lscompany_code + "' and integrated_gmail ='" + lsemployee_mailid + "' order by label_name asc ";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetOutlookFolder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOutlookFolder_list
                        {
                            s_no = dt["s_no"].ToString(),
                            label_id = dt["label_id"].ToString(),
                            label_name = dt["label_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            company_code = dt["company_code"].ToString(),
                            integrated_gmail = dt["integrated_gmail"].ToString(),
                        });
                        values.GetOutlookFolder_list = getModuleList;
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
        public async Task DaOutlookMovetoFolder(gmailfolderlist values,string user_gid)
        {
            try
            {
            

                foreach (var emailId in values.gmailmovelist)
                {
                  
                    try
                    {

                        string messagesUrl;
                        msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                        string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                        if (lsemployee_mailid != null && lsemployee_mailid != "")
                        {

                            graphtoken objtoken = new graphtoken();
                            objtoken = generateGraphAccesstoken();

                            var client = new HttpClient();
                            var request = new HttpRequestMessage(HttpMethod.Post, $"https://graph.microsoft.com/v1.0/users/{lsemployee_mailid}/messages/{emailId.inbox_id}/move");

                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);

                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var requestBody = new
                            {
                                destinationId = values.label_id
                            };

                            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                            var response = await client.SendAsync(request);
                            if (response.StatusCode == HttpStatusCode.Created)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                string id = jsonResponse["id"]?.ToString();
                                string parentFolderId = jsonResponse["parentFolderId"]?.ToString();

                                msSQL = "UPDATE crm_trn_toutlookinbox SET " +
                                       "inbox_status = '" + values.label_id + "', " +
                                       "inbox_id = '" + id + "', " +
                                       "parentfolder_id = '" + parentFolderId + "' " +
                                       "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                             

                                if (mnResult != 0)
                                {
                                    msSQL = "UPDATE crm_trn_toutlookinboxreply SET " +
                                   "inbox_id = '" + id + "' " +
                                   "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        msSQL = "UPDATE crm_trn_toutlookinboxattachement SET " +
                                                                          "inbox_id = '" + id + "' " +
                                                                          "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            msSQL = "UPDATE crm_trn_toutlookinboxreplyforward SET " +
                                                                        "inbox_id = '" + id + "' " +
                                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult != 0)
                                            {
                                                msSQL = "UPDATE crm_trn_toutlookreplyattachment SET " +
                                                   "inbox_id = '" + id + "' " +
                                                 "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                if (mnResult != 0)
                                                {
                                                    msSQL = "UPDATE crm_smm_tgmailcomment SET " +
                                                       "inbox_id = '" + id + "' " +
                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    if (mnResult != 0)
                                                    {
                                                        msSQL = "UPDATE crm_trn_tgmail2customer SET " +
                                                       "inbox_id = '" + id + "' " +
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
                            else
                            {
                                values.message = $"Error: Failed to move email {emailId.inbox_id} to folder.";
                                values.status = false;
                                objcmnfunctions.LogForAudit($"Error: Failed to move email {emailId.inbox_id} to folder.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }
                        else
                        {
                            // If sending failed
                            values.message = "User can't available any Email Id";
                            values.status = false;
                            //objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
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
        public void DaOutlookAPIinboxFolderSummary(MdlGmailCampaign values, string label_id, string user_gid)
        {


            try
            {
                string msSQL1 = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL1);

                msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time, cc,bcc, " +
                    "subject, body, attachement_flag,read_flag,COALESCE(NULLIF(to_id, ''), integrated_gmail) AS integrated_gmail  FROM crm_trn_toutlookinbox where inbox_status='" + label_id + "'   " +
               " AND ( FIND_IN_SET('" + lsemployee_mailid + "', to_id) > 0 OR FIND_IN_SET('" + lsemployee_mailid + "',cc) > 0  OR FIND_IN_SET('" + lsemployee_mailid + "',bcc) > 0 " +
                    " OR integrated_gmail='" + lsemployee_mailid + "'  ) ORDER BY s_no DESC";
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
                       bcc = row["bcc"].ToString(),
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
        public void DaOutlookAPIinboxTrashSummary(MdlGmailCampaign values, string user_gid)
        {


            try
            {
                string msSQL1 = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL1);
                string msSQL2 = "select label_id from crm_trn_toutlookfolder where  label_name  ='Deleted Items'";
                string label_id = objdbconn.GetExecuteScalar(msSQL2);
                msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time, cc,bcc," +
                    " subject, body, attachement_flag,read_flag,COALESCE(NULLIF(to_id, ''), integrated_gmail) AS integrated_gmail   FROM crm_trn_toutlookinbox where inbox_status ='" + label_id + "'  " +
                     " AND ( FIND_IN_SET('" + lsemployee_mailid + "', to_id) > 0 OR FIND_IN_SET('" + lsemployee_mailid + "',cc) > 0  OR FIND_IN_SET('" + lsemployee_mailid + "',bcc) > 0 " +
                    " OR integrated_gmail='" + lsemployee_mailid + "'  ) ORDER BY s_no DESC";
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
                       bcc = row["bcc"].ToString(),
                       subject = row["subject"].ToString(),
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

        public async Task DaOutlookMovetoTrash(gmailmovedlist values,string user_gid)
        {
            try
            {
             
                foreach (var emailId in values.gmailmovelist)
                {
                    try
                    {
                        msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                        string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                        if (lsemployee_mailid != null && lsemployee_mailid != "")
                        {
                            string msSQL1 = "select label_id from crm_trn_toutlookfolder where  label_name  ='Deleted Items'";
                        string label_id = objdbconn.GetExecuteScalar(msSQL1);
                            graphtoken objtoken = new graphtoken();
                            objtoken = generateGraphAccesstoken();

                            var client = new HttpClient();
                            var request = new HttpRequestMessage(HttpMethod.Post, $"https://graph.microsoft.com/v1.0/users/{lsemployee_mailid}/messages/{emailId.inbox_id}/move");

                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);

                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var requestBody = new
                            {
                                destinationId = label_id
                            };

                            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                            var response = await client.SendAsync(request);
                            if (response.StatusCode == HttpStatusCode.Created)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                string id = jsonResponse["id"]?.ToString();
                                string parentFolderId = jsonResponse["parentFolderId"]?.ToString();

                                msSQL = "UPDATE crm_trn_toutlookinbox SET " +
                                       "inbox_status = '" + label_id + "', " +
                                       "inbox_id = '" + id + "', " +
                                       "parentfolder_id = '" + parentFolderId + "' " +
                                       "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                if (mnResult != 0)
                                {
                                    msSQL = "UPDATE crm_trn_toutlookinboxreply SET " +
                                   "inbox_id = '" + id + "' " +
                                   "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        msSQL = "UPDATE crm_trn_toutlookinboxattachement SET " +
                                                                          "inbox_id = '" + id + "' " +
                                                                          "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            msSQL = "UPDATE crm_trn_toutlookinboxreplyforward SET " +
                                                                        "inbox_id = '" + id + "' " +
                                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult != 0)
                                            {
                                                msSQL = "UPDATE crm_trn_toutlookreplyattachment SET " +
                                                   "inbox_id = '" + id + "' " +
                                                 "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                if (mnResult != 0)
                                                {
                                                    msSQL = "UPDATE crm_smm_tgmailcomment SET " +
                                                       "inbox_id = '" + id + "' " +
                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    if (mnResult != 0)
                                                    {
                                                        msSQL = "UPDATE crm_trn_tgmail2customer SET " +
                                                       "inbox_id = '" + id + "' " +
                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        if (mnResult != 0)
                                                        {
                                                            values.message = $"Email moved to Trash Successfully.";
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
                        else
                        {
                            // If sending failed
                            values.message = "User can't available any Email Id";
                            values.status = false;
                            //objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
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
                objcmnfunctions.LogForAudit($"Error in DaOutlookMovetoTrash: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task DaOutlooMoveToInbox(gmailmovedlist values, string user_gid)
        {
            try
            {

                foreach (var emailId in values.gmailmovelist)
                {
                    try
                    {
                        msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                        string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                        if (lsemployee_mailid != null && lsemployee_mailid != "")
                        {
                            string msSQL1 = "select label_id from crm_trn_toutlookfolder where label_name ='Inbox'";
                            string label_id = objdbconn.GetExecuteScalar(msSQL1);
                            graphtoken objtoken = new graphtoken();
                            objtoken = generateGraphAccesstoken();

                            var client = new HttpClient();
                            var request = new HttpRequestMessage(HttpMethod.Post, $"https://graph.microsoft.com/v1.0/users/{lsemployee_mailid}/messages/{emailId.inbox_id}/move");

                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);

                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var requestBody = new
                            {
                                destinationId = label_id
                            };

                            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                            var response = await client.SendAsync(request);
                            if (response.StatusCode == HttpStatusCode.Created)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                string id = jsonResponse["id"]?.ToString();
                                string parentFolderId = jsonResponse["parentFolderId"]?.ToString();

                                msSQL = "UPDATE crm_trn_toutlookinbox SET " +
                                       "inbox_status = NULL, " +
                                       "inbox_id = '" + id + "', " +
                                       "parentfolder_id = '" + parentFolderId + "' " +
                                       "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                if (mnResult != 0)
                                {
                                    msSQL = "UPDATE crm_trn_toutlookinboxreply SET " +
                                   "inbox_id = '" + id + "' " +
                                   "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        msSQL = "UPDATE crm_trn_toutlookinboxattachement SET " +
                                                                          "inbox_id = '" + id + "' " +
                                                                          "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            msSQL = "UPDATE crm_trn_toutlookinboxreplyforward SET " +
                                                                        "inbox_id = '" + id + "' " +
                                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult != 0)
                                            {
                                                msSQL = "UPDATE crm_trn_toutlookreplyattachment SET " +
                                                   "inbox_id = '" + id + "' " +
                                                 "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                if (mnResult != 0)
                                                {
                                                    msSQL = "UPDATE crm_smm_tgmailcomment SET " +
                                                       "inbox_id = '" + id + "' " +
                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    if (mnResult != 0)
                                                    {
                                                        msSQL = "UPDATE crm_trn_tgmail2customer SET " +
                                                       "inbox_id = '" + id + "' " +
                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        if (mnResult != 0)
                                                        {
                                                            values.message = $"Email moved to Inbox Successfully.";
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
                        else
                        {
                            // If sending failed
                            values.message = "User can't available any Email Id";
                            values.status = false;
                            //objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
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
                values.message = "Error While Moving Mail to Inbox!";
                // Log the exception and handle accordingly
                objcmnfunctions.LogForAudit($"Error in DaOutlookMoveToInbox: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }

        public async Task DaOutlookMoveToFolderFromTrash(gmailfolderlist values, string user_gid)
        {
            try
            {


                foreach (var emailId in values.gmailmovelist)
                {

                    try
                    {

                        string messagesUrl;
                        msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                        string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                        if (lsemployee_mailid != null && lsemployee_mailid != "")
                        {

                            graphtoken objtoken = new graphtoken();
                            objtoken = generateGraphAccesstoken();

                            var client = new HttpClient();
                            var request = new HttpRequestMessage(HttpMethod.Post, $"https://graph.microsoft.com/v1.0/users/{lsemployee_mailid}/messages/{emailId.inbox_id}/move");

                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);

                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var requestBody = new
                            {
                                destinationId = values.label_id
                            };

                            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                            var response = await client.SendAsync(request);
                            if (response.StatusCode == HttpStatusCode.Created)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                string id = jsonResponse["id"]?.ToString();
                                string parentFolderId = jsonResponse["parentFolderId"]?.ToString();

                                msSQL = "UPDATE crm_trn_toutlookinbox SET " +
                                       "inbox_status = '" + values.label_id + "', " +
                                       "inbox_id = '" + id + "', " +
                                       "parentfolder_id = '" + parentFolderId + "' " +
                                       "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                if (mnResult != 0)
                                {
                                    msSQL = "UPDATE crm_trn_toutlookinboxreply SET " +
                                   "inbox_id = '" + id + "' " +
                                   "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        msSQL = "UPDATE crm_trn_toutlookinboxattachement SET " +
                                                                          "inbox_id = '" + id + "' " +
                                                                          "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            msSQL = "UPDATE crm_trn_toutlookinboxreplyforward SET " +
                                                                        "inbox_id = '" + id + "' " +
                                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult != 0)
                                            {
                                                msSQL = "UPDATE crm_trn_toutlookreplyattachment SET " +
                                                   "inbox_id = '" + id + "' " +
                                                 "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                if (mnResult != 0)
                                                {
                                                    msSQL = "UPDATE crm_smm_tgmailcomment SET " +
                                                       "inbox_id = '" + id + "' " +
                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    if (mnResult != 0)
                                                    {
                                                        msSQL = "UPDATE crm_trn_tgmail2customer SET " +
                                                       "inbox_id = '" + id + "' " +
                                                        "WHERE inbox_id = '" + emailId.inbox_id + "'";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        if (mnResult != 0)
                                                        {
                                                            values.message = $"Email moved to Folder Successfully.";
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
                        else
                        {
                            // If sending failed
                            values.message = "User can't available any Email Id";
                            values.status = false;
                            //objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
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
                objcmnfunctions.LogForAudit($"Error in DaOutlookMoveToFolderFromTrash: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task DaOutlookTrashDeleteMail(gmailmovedlist values,string user_gid)
        {
            try
            {
     
                foreach (var emailId in values.gmailmovelist)
                {
                    try
                    {
                        msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                        string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                        if (lsemployee_mailid != null && lsemployee_mailid != "")
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

                        
                        string msSQL3 = "select * from  crm_trn_toutlookinbox  where inbox_id= '" + emailId.inbox_id + "' limit 1";
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
                        string insertSQL = "INSERT INTO crm_trn_toutlookinboxdeletelog (" +
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
                            msSQL = "DELETE FROM crm_trn_toutlookinbox WHERE inbox_id = '" + emailId.inbox_id + "'";
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
                        else
                        {
                            // If sending failed
                            values.message = "User can't available any Email Id";
                            values.status = false;
                           // objcmnfunctions.LogForAudit("Error: Failed to send forward mail.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
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
                objcmnfunctions.LogForAudit($"Error in DaOutlookTrashDeleteMail: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task  DaOutlookCreateEmailLabel(createlabel_list values, string user_gid)
        {
            try
            {
                string msSQL = "select label_id from crm_trn_toutlookfolder where label_name='" + values.labelName.Replace("'", "''") + "'";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count == 0)
                {
                 
                    msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                    string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                    if (lsemployee_mailid != null && lsemployee_mailid != "")
                    {
                      

                        msSQL = "select company_code from adm_mst_tcompany limit 1";
                        string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                        graphtoken objtoken = new graphtoken();
                        objtoken = generateGraphAccesstoken();

                        var client = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Post, $"https://graph.microsoft.com/v1.0/users/{lsemployee_mailid}/mailFolders");

                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);

                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var requestBody = new
                        {
                            displayName = values.labelName.Replace("'", "''")
                        };

                        request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                        var response = await client.SendAsync(request);
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            JObject jsonResponse = JObject.Parse(responseBody);
                            string id = jsonResponse["id"]?.ToString();
                            string parentFolderId = jsonResponse["parentFolderId"]?.ToString();
                            string childFolderCount = jsonResponse["childFolderCount"]?.ToString();
                            string unreadItemCount = jsonResponse["unreadItemCount"]?.ToString();
                            string totalItemCount = jsonResponse["totalItemCount"]?.ToString();
                            string sizeInBytes = jsonResponse["sizeInBytes"]?.ToString();
                            string isHidden = jsonResponse["isHidden"]?.ToString();


                            string msSQLInsert = "INSERT INTO crm_trn_toutlookfolder (" +
                                                    "label_id, " +
                                                    "label_name, " +
                                                    "parent_folder_id, " +
                                                    "child_folder_count, " +
                                                    "unread_item_count, " +
                                                    "total_item_count, " +
                                                    "size_in_bytes, " +
                                                    "is_hidden, " +
                                                    "company_code, " +
                                                    "integrated_gmail, " +
                                                    "created_by, " +
                                                    "created_date) " +
                                                    "VALUES (" +
                                                    "'" + id.ToString() + "', " +
                                                    "'" + values.labelName.Replace("'", "''") + "', " +
                                                    "'" + parentFolderId.ToString() + "', " +
                                                    "'" + childFolderCount.ToString() + "', " +
                                                    "'" + unreadItemCount.ToString() + "', " +
                                                    "'" + totalItemCount.ToString() + "', " +
                                                    "'" + sizeInBytes.ToString() + "', " +
                                                    "'" + isHidden.ToString() + "', " +
                                                    "'" + lscompany_code + "', " +
                                                    "'" + lsemployee_mailid + "', " +
                                                    "'" + user_gid + "', " +
                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQLInsert);
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
                            values.message = "Error While Creating Folder on Outlook.";
                            values.status = false;
                            objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }

                      
                    }
                    else
                    {
                        values.message = "User Don't Map Any Outlook API Account !";
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
                objcmnfunctions.LogForAudit($"Error in DaOutlookCreateEmailLabel: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw;
            }
        }
        public void DaOutlookUpdateEmailLabel(updatelabel_list values, string user_gid)
        {
            try
            {
                string msSQL = "select label_id from crm_trn_toutlookfolder where label_name='" + values.labelNameEdit.Replace("'", "''") + "'";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count == 0)
                {

                    msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                    string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                    if (lsemployee_mailid != null && lsemployee_mailid != "")
                    {


                        msSQL = "select company_code from adm_mst_tcompany limit 1";
                        string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                        graphtoken objtoken = new graphtoken();
                        objtoken = generateGraphAccesstoken();

                        var client = new RestClient("https://graph.microsoft.com/v1.0");
                        string messagesUrl = $"/users/" + lsemployee_mailid + $"/mailFolders/" + values.label_id;


                        // Create a RestRequest for the PATCH method with the required endpoint
                        var request = new RestRequest(messagesUrl, Method.PATCH);

                        // Add the Authorization header
                        request.AddHeader("Authorization", objtoken.access_token);

                        // Add the Content-Type header
                        request.AddHeader("Content-Type", "application/json");

                        var requestBody = new
                        {
                            displayName = values.labelNameEdit.Replace("'", "''")
                        };
                        request.AddJsonBody(requestBody);

                        // Execute the request and get the response
                        IRestResponse response = client.Execute(request);

                        // Check if the response is successful
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                        {                         
                            string responseBody =  response.Content;
                            JObject jsonResponse = JObject.Parse(responseBody);
                            string id = jsonResponse["id"]?.ToString();
                            string parentFolderId = jsonResponse["parentFolderId"]?.ToString();
                            string childFolderCount = jsonResponse["childFolderCount"]?.ToString();
                            string unreadItemCount = jsonResponse["unreadItemCount"]?.ToString();
                            string totalItemCount = jsonResponse["totalItemCount"]?.ToString();
                            string sizeInBytes = jsonResponse["sizeInBytes"]?.ToString();
                            string isHidden = jsonResponse["isHidden"]?.ToString();


                            string msSQLUpdate = "UPDATE crm_trn_toutlookfolder SET " +
                                               "label_name = '" + values.labelNameEdit.Replace("'", "''") + "', " +
                                               "parent_folder_id = '" + parentFolderId.ToString() + "', " +
                                               "child_folder_count = '" + childFolderCount.ToString() + "', " +
                                               "unread_item_count = '" + unreadItemCount.ToString() + "', " +
                                               "total_item_count = '" + totalItemCount.ToString() + "', " +
                                               "size_in_bytes = '" + sizeInBytes.ToString() + "', " +
                                               "is_hidden = '" + isHidden.ToString() + "', " +
                                               "company_code = '" + lscompany_code + "', " +
                                               "integrated_gmail = '" + lsemployee_mailid + "', " +
                                               "created_by = '" + user_gid + "', " +
                                               "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                               "WHERE label_id = '" + values.label_id.ToString() + "'";

                            int mnResult = objdbconn.ExecuteNonQuerySQL(msSQLUpdate);

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
                            values.message = "Error While Updating Folder on Outlook.";
                            values.status = false;
                            objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} SQL Query: {msSQL} API Reference", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }


                    }
                    else
                    {
                        values.message = "User Don't Map Any Outlook API Account !";
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
                values.message = "Error While Updating Folder.";
                values.status = false;
                objcmnfunctions.LogForAudit($"Error in DaOutlookUpdateEmailLabel: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw;
            }
        }
        public async Task DaOutlookDeleteLabelOrFolder(updatelabel_list values, string label_id,string user_gid)
        {
            try
            {
                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    string messagesUrl = $"https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + $"/mailFolders/{label_id}/messages";

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    if (objtoken.status)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                            HttpResponseMessage response = await client.GetAsync(messagesUrl);

                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                dbconn objdbconn = new dbconn();
                                cmnfunctions objcmnfunctions = new cmnfunctions();
                                DataTable dt_datatable;
                                string msSQL, lsbusinessunit_gid, lsbusinessunit_name, lsassigned_manager, lsassigned_manager_gid, lsactivity_name, lsactivity_gid;
                                int mnResult;
                                string msmailattachment_gid, lsattachment_flag;
                                string msGetSRQGID, msGetGIDs;
                                if (jsonResponse["value"] != null && jsonResponse["value"].HasValues)
                                {
                                    values.message = "Folder is not empty. Cannot delete.";
                                    values.status = false;
                                    return;

                                }
                                else
                                {
                                    string messagesUrl1 = $"https://graph.microsoft.com/v1.0/users/{lsemployee_mailid}/mailFolders/{label_id}";
                                     using (HttpClient client1 = new HttpClient())
                                        {
                                            client1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                                            HttpResponseMessage response1 = await client1.DeleteAsync(messagesUrl1);

                                        if (response1.StatusCode == HttpStatusCode.NoContent)
                                         {
                                            msSQL = "delete from crm_trn_toutlookfolder where label_id='" + label_id + "'";
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
                                            else
                                            {
                                                values.message = "Error fetching data from API.";
                                                values.status = false;
                                                string errorResponse = await response.Content.ReadAsStringAsync();
                                                objcmnfunctions.LogForAudit($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name} API Error Response: {errorResponse}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                            }
                                        }

                                   
                                }
                            }
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occured while sending mail :" + objtoken.message;
                    }
                }
                else
                {
                    // If sending failed
                    values.message = "User can't available any Email Id";
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
        public void DaOutlooklSenditemSummary(MdlOutlookCampaign values, string employee_gid)

        {
            try
            {
                msSQL = " select employee_emailid from hrm_mst_temployee where employee_gid = '" + employee_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select a.mail_gid,a.to_mailaddress,a.mail_body,a.mail_subject,a.from_mailaddress,a.sent_flag,concat(b.user_firstname,' ',b.user_lastname)as sent_by," +
                        " DATE_FORMAT(a.sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(a.sent_date, '%h:%i %p') AS sent_time,a.leadbank_gid from crm_trn_toutlook a" +
                        " left join adm_mst_tuser b on b.user_gid = a.sent_by" +
                        " where a.from_mailaddress = '"+ lsemployee_mailid + "' " +
                        " order by a.sent_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<outlooksentMail_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new outlooksentMail_list
                        {

                            to_mailaddress = dt["to_mailaddress"].ToString(),
                            sent_flag = dt["sent_flag"].ToString(),
                            sent_by = dt["sent_by"].ToString(),
                            sent_date = dt["sent_date"].ToString(),
                            sent_time = dt["sent_time"].ToString(),
                            mail_gid = dt["mail_gid"].ToString(),
                            mail_body = DecodeFromBase64(dt["mail_body"].ToString()),
                            mail_subject = dt["mail_subject"].ToString(),
                            from_mailaddress = dt["from_mailaddress"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString()



                        });
                        values.outlooksentMail_list = getmodulelist;
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
        public void DaOutlooklindividualSenditemSummary(MdlOutlookCampaign values,string leadbank_gid, string employee_gid)

        {
            try
            {
                msSQL = " select employee_emailid from hrm_mst_temployee where employee_gid = '" + employee_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select a.mail_gid,a.to_mailaddress,a.mail_body,a.mail_subject,a.from_mailaddress,a.sent_flag,concat(b.user_firstname,' ',b.user_lastname)as sent_by," +
                        " DATE_FORMAT(a.sent_date, '%b %e, %Y %h:%i %p') AS sent_date,   DATE_FORMAT(a.sent_date, '%h:%i %p') AS sent_time,a.leadbank_gid from crm_trn_toutlook a" +
                        " left join adm_mst_tuser b on b.user_gid = a.sent_by" +
                        " where a.leadbank_gid = '" + leadbank_gid + "' and a.from_mailaddress = '" + lsemployee_mailid + "' " +
                        " order by a.sent_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<outlooksentMail_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new outlooksentMail_list
                        {

                            to_mailaddress = dt["to_mailaddress"].ToString(),
                            sent_flag = dt["sent_flag"].ToString(),
                            sent_by = dt["sent_by"].ToString(),
                            sent_date = dt["sent_date"].ToString(),
                            sent_time = dt["sent_time"].ToString(),
                            mail_gid = dt["mail_gid"].ToString(),
                            mail_body = DecodeFromBase64(dt["mail_body"].ToString()),
                            mail_subject = dt["mail_subject"].ToString(),
                            from_mailaddress = dt["from_mailaddress"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString()



                        });
                        values.outlooksentMail_list = getmodulelist;
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
        public void DaPostcreateAppointment(string user_gid, appointmentcreation values)
        {
            try
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
                        msSQL = " update  crm_trn_toutlookinbox set " +
                       " leadbank_gid = '" + leadbank_gid + "' where inbox_id='" + values.inbox_id + "' ";
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
                    msSQL = " update  crm_trn_toutlookinbox set " +
                      " leadbank_gid = '" + msGetGid1 + "' where inbox_id='" + values.inbox_id + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                        " 'Y'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

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
                            values.message = "Error While Occured Creating Appointment ";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Occured Creating Appointment ";
                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Appointment Created Successfully";
                }
                else
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + "***********" +
              values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    values.status = false;
                    values.message = "Error While Occured Creating Appointment  ";
                }
            }
            catch (Exception ex)
            {


                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                values.status = false;
                values.message = "Error While Occured Creating Appointment  ";

            }
        }

        public class Email
        {
            public string MessageId { get; set; }
            public string FromId { get; set; }
            public string To { get; set; }
            public string CC { get; set; }
            public string BCC { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public string SentDate { get; set; }
            public string IsRead { get; set; }
            public string HasAttachments { get; set; }
            public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
        }

        public class EmailAttachment
        {
            public string AttachmentId { get; set; }
            public string Name { get; set; }
            public byte[] Content { get; set; }
        }

        public async Task DaOutlookAPIDirectly(MdlGmailCampaign values, string user_gid)
        {
            List<gmailapiinboxsummary_lists> emailSummaryList = new List<gmailapiinboxsummary_lists>();

            try
            {
                string msSQL = "SELECT employee_emailid FROM hrm_mst_temployee WHERE user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);

                if (!string.IsNullOrEmpty(lsemployee_mailid))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    string messagesUrl = $"https://graph.microsoft.com/v1.0/users/{lsemployee_mailid}/mailFolders/Inbox/messages?$orderby=receivedDateTime desc,hasAttachments&$top=50&$expand=attachments";

                    graphtoken objtoken = generateGraphAccesstoken();

                    if (objtoken.status)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                            HttpResponseMessage response = await client.GetAsync(messagesUrl);

                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);

                                foreach (var item in jsonResponse["value"])
                                {
                                    // Extract basic email details
                                    var emailSummary = new gmailapiinboxsummary_lists
                                    {
                                        inbox_id = item["id"].ToString(),
                                        integrated_gmail = lsemployee_mailid,
                                        from_id = item["from"]["emailAddress"]["address"].ToString(),
                                        //to_id = GetRecipientEmailAddresses(item["toRecipients"]),
                                        cc = GetRecipientEmailAddresses(item["ccRecipients"]),
                                        bcc = GetRecipientEmailAddresses(item["bccRecipients"]),
                                        sent_date = DateTimeOffset.Parse(item["sentDateTime"].ToString()).ToString("MMM dd, yyyy hh:mm tt"), // Example: "Sep 23, 2024 12:20 PM"
                                        sent_time = DateTimeOffset.Parse(item["sentDateTime"].ToString()).ToString("hh:mm tt"), // Example: "12:20 PM"
                                        subject = item["subject"].ToString(),
                                        body = item["body"]["content"].ToString().Replace("'", "''"),
                                        read_flag = item["isRead"].ToObject<bool>()

                                    };

                                    // Process attachments if they exist
                                    if (item["hasAttachments"].ToString() == "True")
                                    {
                                        emailSummary.Attachments = item["attachments"].Select(attachment =>
                                        {
                                            // Directly get the base64 string from the attachment
                                            string base64Content = attachment["contentBytes"].ToString();
                                            string fileName = attachment["name"].ToString();
                                            // Construct the Data URI
                                            string dataUri = $"data:{attachment["contentType"].ToString()};base64,{base64Content}";

                                            return new gmailapiinboxatatchement_lists
                                            {
                                                inbox_id = emailSummary.inbox_id,
                                                original_filename = fileName,
                                                file_path = dataUri  // The constructed Data URI
                                            };
                                        }).ToList();


                                    }

                                    emailSummaryList.Add(emailSummary);
                                }
                            }
                            else
                            {
                                string errorContent = await response.Content.ReadAsStringAsync();
                                objcmnfunctions.LogForAudit($"Error: {errorContent}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("Token issue", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
            }
            catch (Exception e)
            {
                objcmnfunctions.LogForAudit($"Exception: {e}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            values.gmailapiinboxsummary_lists = emailSummaryList;
        }

        private string GetMimeTypeFromFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg": return "image/jpeg";
                case ".png": return "image/png";
                case ".pdf": return "application/pdf";
                case ".txt": return "text/plain";
                // Add more MIME types as needed
                default: return "application/octet-stream"; // Default fallback
            }
        }
        public async Task DaOutlookInboxStatusUpdateBack(replymail_list values, string user_gid)
        {
            try
            {

                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    if (objtoken.status)
                    {

                        var client = new RestClient("https://graph.microsoft.com/v1.0");
                        string messagesUrl = $"/users/" + lsemployee_mailid + $"/messages/" + values.inbox_id;


                        // Create a RestRequest for the PATCH method with the required endpoint
                        var request = new RestRequest(messagesUrl, Method.PATCH);

                        // Add the Authorization header
                        request.AddHeader("Authorization", objtoken.access_token);

                        // Add the Content-Type header
                        request.AddHeader("Content-Type", "application/json");

                        // Add the JSON body
                        var body = new
                        {
                            isRead = true
                        };
                        request.AddJsonBody(body);

                        // Execute the request and get the response
                        IRestResponse response = client.Execute(request);

                        // Check if the response is successful
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                        {


                            //msSQL = "UPDATE crm_trn_toutlookinbox SET read_flag = 'True' WHERE inbox_id = '" + values.inbox_id + "'";
                            //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            //if (mnResult != 1)
                            //{
                            //    objcmnfunctions.LogForAudit($"Error: Failed to update read status of email {values.inbox_id}.", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                           // }

                        }
                        else
                        {


                            // Log the error
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                                        "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                                        "***********" +
                                                        "**************Query****" + msSQL +
                                                        "*******Apiref********",
                                                        "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        }

                    }
                    values.status = true;
                }


                else
                {
                    values.status = false;
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*********** DaOutlookInboxStatusUpdateBack token issue" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");


                }

            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error occurred while processing emails.";
                objcmnfunctions.LogForAudit($"Error in DaOutlookInboxStatusUpdateBack: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                throw; // Rethrow the exception or handle it as per your application's needs
            }
        }
        public async Task DaPostOutlookcreateAppointments(string user_gid, appointmentcreations values)
        {
            try
            {
                msSQL = " select employee_emailid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_mailid = objdbconn.GetExecuteScalar(msSQL);
                if (lsemployee_mailid != null && lsemployee_mailid != "")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    string messagesUrl = $"https://graph.microsoft.com/v1.0/users/" + lsemployee_mailid + $"/mailFolders/Inbox/messages/" + values.inbox_id + $"?$orderby=receivedDateTime desc,hasAttachments&$top=100&$expand=attachments";

                    //string messagesUrl = $"https://graph.microsoft.com/v1.0/users/snehith@vcidex.com/mailFolders/Inbox/messages?$orderby=receivedDateTime desc,hasAttachments&$top=20&$expand=attachments";

                    //string messagesUrl = $"https://graph.microsoft.com/v1.0/users/snehith@vcidex.com/mailFolders/Inbox/messages?$orderby=receivedDateTime desc,hasAttachments&$filter=receivedDateTime le 2024-07-22T18:00:00Z and hasAttachments eq true&$top=10&$expand=attachments";
                    graphtoken objtoken = new graphtoken();
                    objtoken = generateGraphAccesstoken();

                    if (objtoken.status)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objtoken.access_token);
                            HttpResponseMessage response = await client.GetAsync(messagesUrl);

                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                JObject jsonResponse = JObject.Parse(responseBody);
                                dbconn objdbconn = new dbconn();
                                cmnfunctions objcmnfunctions = new cmnfunctions();
                                DataTable dt_datatable;
                                string msSQL, lsbusinessunit_gid, lsbusinessunit_name, lsassigned_manager, lsassigned_manager_gid, lsactivity_name, lsactivity_gid;
                                int mnResult;
                                string msmailattachment_gid, lsattachment_flag;
                                string msGetSRQGID, msGetGIDs;
                                string lscompanycode;
                                var item = jsonResponse;
                                string sentDateString = item["sentDateTime"].ToString();
                                DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(sentDateString, null, System.Globalization.DateTimeStyles.RoundtripKind);

                                // Format the DateTimeOffset to desired string format
                                string formattedDateString = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");

                                string fromId = item["from"]["emailAddress"]["address"].ToString();
                                string profilePictureUrl = jsonResponse["photo"]?["@odata.mediaEditLink"]?.ToString();
                                string messageId = item["id"].ToString();
                                string subject = item["subject"].ToString();
                                string body = item["body"]["content"].ToString();
                                //string fromName = item["from"]["emailAddress"]["name"].ToString();
                                string bodyPreview = item["bodyPreview"]?.ToString()?.Replace("'", "")?.Trim() ?? "";
                                string messageNo = item["@odata.etag"].ToString();
                                //string formattedDate = sentDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                string fromName = GetRecipientEmailAddresses(item["toRecipients"]);
                                string cc = GetRecipientEmailAddresses(item["ccRecipients"]);
                                string bcc = GetRecipientEmailAddresses(item["bccRecipients"]);
                                string parentFolderId = item["parentFolderId"].ToString();
                                string conversationId = item["conversationId"].ToString();
                                string conversationIndex = item["conversationIndex"].ToString();
                                string isRead = item["isRead"].ToString();
                                string hasAttachments = item["hasAttachments"].ToString();
                                msSQL = " select inbox_id  from crm_trn_toutlookinbox where inbox_id = '" + messageId + "'";
                                dt_datatable = objdbconn.GetDataTable(msSQL);
                                if (dt_datatable.Rows.Count == 0)
                                {

                                    if (hasAttachments == "True")
                                    {
                                        lsattachment_flag = "Y";
                                    }
                                    else
                                    {
                                        lsattachment_flag = "N";
                                    }
                                    msSQL = " select company_code from  adm_mst_tcompany limit 1";
                                    lscompanycode = objdbconn.GetExecuteScalar(msSQL);
                                    msSQL = " INSERT INTO crm_trn_toutlookinbox(" +
                                            " inbox_id," +
                                            " from_id," +
                                            " to_id," +
                                            " cc," +
                                            " bcc," +
                                            " subject," +
                                            " body," +
                                            " sent_date," +
                                            " read_flag," +
                                            " attachement_flag," +
                                            " parentfolder_id," +
                                            " conversation_id ," +
                                            " conversation_index ," +
                                            " company_code ," +
                                            " integrated_gmail )" +
                                            " VALUES (" +
                                            "'" + messageId + "'," +
                                            "'" + fromId + "'," +
                                            "'" + fromName + "'," +
                                            "'" + cc + "'," +
                                            "'" + bcc + "'," +
                                            "'" + subject.Replace("'", "''") + "'," +
                                            "'" + body.Replace("'", "''") + "'," +
                                            "'" + formattedDateString + "'," +
                                            "'" + isRead + "'," +
                                            "'" + lsattachment_flag + "'," +
                                            "'" + parentFolderId + "'," +
                                            "'" + conversationId + "'," +
                                            "'" + conversationIndex + "'," +
                                            "'" + lscompanycode + "'," +
                                            "'" + lsemployee_mailid + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {

                                        if (hasAttachments == "True")
                                        {


                                            foreach (var attachment in item["attachments"])
                                            {
                                                string attachmentName = attachment["name"].ToString();
                                                //string fileTablePath = temp_path + "/" + attachmentName.Replace("'", @"\'");
                                                byte[] contentBytes = Convert.FromBase64String(attachment["contentBytes"].ToString());
                                                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                                                string lsfile_gid = msdocument_gid;
                                                string FileExtension = Path.GetExtension(attachmentName).ToLowerInvariant();
                                                lsfile_gid = lsfile_gid + FileExtension;

                                                string logFilePath = ConfigurationManager.AppSettings["filepathoutlook"];

                                                // Check if the directory exists and create it if it does not
                                                if (!Directory.Exists(logFilePath))
                                                {
                                                    Directory.CreateDirectory(logFilePath);
                                                }
                                                await SaveAttachmentToFile(contentBytes, lsfile_gid, logFilePath);

                                                msSQL = " INSERT INTO crm_trn_toutlookinboxattachement (" +
                                                    " inbox_id," +
                                                    " attachment_id," +
                                                    " original_filename," +
                                                    " modified_filename," +
                                                    " file_path," +
                                                    " created_by," +
                                                    " created_date) " +
                                                    "VALUES (" +
                                                    " '" + messageId + "'," +
                                                    " '" + attachment["id"].ToString() + "'," +
                                                    " '" + attachmentName.Replace("'", "") + "'," +
                                                    " '" + lsfile_gid + "'," +
                                                     " '" + ConfigurationManager.AppSettings["dbpath_outlook"].ToString() + lsfile_gid + "'," +
                                                    " '" + user_gid + "'," +
                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                if (mnResult == 0)
                                                {

                                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                                }
                                            }
                                        }

                                        if (mnResult == 1)
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
                                                        msSQL = " update  crm_trn_toutlookinbox set " +
                                                       " leadbank_gid = '" + msGetGid + "' where inbox_id='" + values.inbox_id + "' ";

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
                                                    msSQL = " update  crm_trn_toutlookinbox set " +
                                                     " leadbank_gid = '" + msGetGid1 + "' where inbox_id='" + values.inbox_id + "' ";

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
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

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
                                                    msSQL = " update  crm_trn_toutlookinbox set " +
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
                                                msSQL = " update  crm_trn_toutlookinbox set " +
                                                 " leadbank_gid = '" + msGetGid1 + "' where inbox_id='" + values.inbox_id + "' ";

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
                                   
                                    dt_datatable.Dispose();




                                }
                            }
                            else
                            {
                                // Retrieve error content from the response
                                string errorContent = await response.Content.ReadAsStringAsync();

                                // Log the error
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                                            "***********" + errorContent +
                                                            "**************Query****" + msSQL +
                                                            "*******Apiref********",
                                                            "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                            }
                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********token not getting" + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        values.status = false;
                        values.message = "Error While Occured Submitting Appointment ";
                    }
                }
                else
                {

                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + "***********" +
              values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    values.status = false;
                    values.message = "Error While Occured Submitting Appointment ";
                }
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
    }

}