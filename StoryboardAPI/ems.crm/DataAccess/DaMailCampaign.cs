﻿using ems.crm.Models;
using ems.utilities.Functions;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using System.Web.Mail;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using static OfficeOpenXml.ExcelErrorValue;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;


namespace ems.crm.DataAccess
{
    public class DaMailCampaign
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, body, lsshopify_id, lsemail, lsname, lssource_gid, final_path;

        public void DaGetMailSummary(MdlMailCampaign values)
        {
            try 
            { 
                mailconfiguration getmailcredentials = mailcrendentials();
                msSQL = " select a.mailmanagement_gid,a.from_mail,a.to_mail,a.direction," +
                    "case when a.temp_mail_gid is not null then b.sub else a.sub end as sub," +
                    "a.status_open,a.image_path,a.temp_mail_gid ,date_format(a.created_date,'%d-%m-%y')  as created_date,a.created_date AS or_date,a.leadbank_gid," +
                    "time_format(a.created_date,'%h:%i:%s')as created_time from crm_smm_mailmanagement a  " +
                    "left join temp_crm_smm_mailmanagement b on a.temp_mail_gid=b.temp_mail_gid order by a.created_date desc,a.mailmanagement_gid; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
            
                var getModuleList = new List<mailsummary_list>();
                values.sending_domain = getmailcredentials.sending_domain;
                values.receiving_domain = getmailcredentials.receiving_domain;
                values.email_username = getmailcredentials.email_username;
                if (dt_datatable.Rows.Count != 0)

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new mailsummary_list
                        {
                            mailmanagement_gid = dt["mailmanagement_gid"].ToString(),
                            mail_from = dt["from_mail"].ToString(),
                            to = dt["to_mail"].ToString(),
                            sub = dt["sub"].ToString(),
                            image_path = dt["image_path"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            status_open = dt["status_open"].ToString(),
                            direction = dt["direction"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            created_time = dt["created_time"].ToString(),
                            sending_domain = values.sending_domain,
                            receiving_domain = values.receiving_domain,
                            email_username = values.email_username,


                        }) ;
                        values.mailsummary_list = getModuleList;
                    }

                dt_datatable.Dispose();
            }
            catch(Exception ex)
            {
                values.message = "Error While Fecthing Mail Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{ System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" +ex.ToString()+ "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
          
            
        }


        ///send mail with bcc & cc//
        public void DaMailSend(mailsummary_list values, string user_gid)

        {
            mailconfiguration getmailconfiguration = mailcrendentials();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://api.sparkpost.com");
                var request = new RestRequest("/api/v1/transmissions", Method.POST);
                request.AddHeader("Authorization", ""+getmailconfiguration.access_token+"");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"options\":{\"open_tracking\":true,\"click_tracking\":true},\"recipients\":[{\"address\":{\"email\":" + "\"" + values.to_mail.Replace(" ","") + "\"" + "}},{\"address\":{\"email\":" + "\"" + values.cc + "\"" + ",\"header_to\":" + "\"" + values.to_mail.Replace(" ", "") + "\"" + "}},{\"address\":{\"email\":" + "\"" + values.bcc + "\"" + ",\"header_to\":" + "\"" + values.to_mail.Replace(" ", "") + "\"" + "}}],\"content\":{\"from\":" + "\"" + getmailconfiguration.email_username +"<"+ getmailconfiguration.sending_domain + ">\"" + ",\"headers\":{\"CC\":" + "\"" + values.cc + "\"" + "},\"subject\":" + "\"" + values.sub.Replace("\"", "\\\"").Replace("'", "\\\'") + "\"" + ",\"reply_to\":" + "\"" + getmailconfiguration.receiving_domain + "\"" + ",\"html\":" + "\"" + values.body.Replace("\"", "\\\"").Replace("'", "\\\'").Replace("<img ", "<img style=\\\"width: 500px\\\"") + "\"" + "}}";
                var body_content = JsonConvert.DeserializeObject(body);
                request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string errornetsuiteJSON = response.Content;
                sendmail_list objMdlMailCampaignResponse = new sendmail_list(); ;
                objMdlMailCampaignResponse = JsonConvert.DeserializeObject<sendmail_list>(errornetsuiteJSON);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    msSQL= "select leadbank_gid from crm_smm_mailmanagement where to_mail='" + values.to_mail + "'";
                    string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                    // Insert email details into the database
                    msGetGid = objcmnfunctions.GetMasterGID("MILC");
                    msSQL = "INSERT INTO crm_smm_mailmanagement (" +
                           "mailmanagement_gid, " +
                        "from_mail, " +
                        "to_mail, " +
                        "sub, " +
                        "transmission_id, " +
                        "bcc, " +
                        "cc, " +
                        "reply_to, " +
                        "leadbank_gid, " +
                        "body, " +
                         " created_by, " +
                        "created_date) " +
                        "VALUES (" +
                        "'" + msGetGid + "', " +
                        "'" + getmailconfiguration.sending_domain + "', " +
                        "'" + values.to_mail.Replace(" ", "") + "', " +
                        "'" + values.sub.Replace("\"", "\\\"").Replace("'", "\\\'") + "', " +
                        "'" + objMdlMailCampaignResponse.results.id + "', " +
                        "'" + values.bcc + "', " +
                        "'" + values.cc + "', " +
                        "'" + getmailconfiguration.receiving_domain + "', " +
                        "'" + lsleadbank_gid + "', " +
                        "'" + values.body.Replace("\"", "\\\"").Replace("'", "\\\'").Replace("<img ", "<img style=\\\"width: 500px\\\"") + "', " +
                          "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Mail Send Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Sending Mail !!";
                    }
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }


        }

    
        public void DaScheduledMailSend(HttpRequest httpRequest, string user_gid, results objResult)
        {
            try 
            { 
            HttpFileCollection httpFileCollection;
            HttpPostedFile httpPostedFile;
            string basecode = httpRequest.Form[0];
            string mail_from = httpRequest.Form[1];
            string sub = httpRequest.Form[2];
            string to = httpRequest.Form[3];
            string body = httpRequest.Form[4];
            string bcc = httpRequest.Form[5];
            string cc = httpRequest.Form[6];
            string reply_to = httpRequest.Form[7];
            string schedule_time = httpRequest.Form[8];
            httpFileCollection = httpRequest.Files;
            string lsfilepath = string.Empty;
            string document_gid = string.Empty;
            string lspath, lspath1;
            string lscompany_code = string.Empty;
            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
            {
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);
            }
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        MemoryStream ms = new MemoryStream();
                        string msdocument_gid = objcmnfunctions.GetMasterGID("MILC");
                        httpPostedFile = httpFileCollection[i];
                        string name = httpPostedFile.FileName;
                        string type = httpPostedFile.ContentType;
                        string apibasecode = "data:" + type + ";base64,";
                        basecode = basecode.Substring(apibasecode.Length);
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        name = Path.GetExtension(name).ToLower();
                        lsfile_gid = lsfile_gid + name;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, name);

                        ms.Close();
                        lspath = "erpdocument" + "/" + lscompany_code + "/" + "Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        lspath1 = "/assets/media/images/erpdocument" + "/" + lscompany_code + "/" + "Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + name;

                        string final_path = ConfigurationManager.AppSettings["imgfile_path"] + lspath + msdocument_gid + name;

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient("https://api.sparkpost.com");
                        var request = new RestRequest("/api/v1/transmissions", Method.POST);
                        request.AddHeader("Authorization", "14e9f31c9e5002fb9dcf7e28b89f7d0d92759a4c");
                        request.AddHeader("Content-Type", "application/json");
                        var bodies = "{\"options\":{\"open_tracking\":true,\"click_tracking\":true,\"start_time\":" + "\"" + schedule_time + "+05:30\"},\"recipients\":[{\"address\":{\"email\":" + "\"" + to + "\"" + "}},{\"address\":{\"email\":" + "\"" + cc + "\"" + ",\"header_to\":" + "\"" + to + "\"" + "}},{\"address\":{\"email\":" + "\"" + bcc + "\"" + ",\"header_to\":" + "\"" + to + "\"" + "}}],\"content\":{\"from\":" + "\"" + mail_from + "\"" + ",\"headers\":{\"CC\":" + "\"" + cc + "\"" + "},\"subject\":" + "\"" + sub.Replace("\"", "\\\"") + "\"" + ",\"reply_to\":" + "\"" + reply_to + "\"" + ",\"html\":" + "\"" + body.Replace("\"", "\\\"") + "\"" + ",\"attachments\":[{\"name\":" + "\"" + httpPostedFile.FileName + "\"" + ",\"type\":" + "\"" + type + "\"" + ",\"data\":" + "\"" + basecode + "\"" + "}]}}";
                        var body_content = JsonConvert.DeserializeObject(bodies);
                        request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        string errornetsuiteJSON = response.Content;
                        sendmail_list objMdlMailCampaignResponse = new sendmail_list();
                        objMdlMailCampaignResponse = JsonConvert.DeserializeObject<sendmail_list>(errornetsuiteJSON);



                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Insert email details into the database
                            msGetGid = objcmnfunctions.GetMasterGID("MILC");
                            msGetGid1 = objcmnfunctions.GetMasterGID("SMIL");
                            msSQL = "INSERT INTO crm_smm_mailmanagement (" +
                                  "mailmanagement_gid, " +
                                "schedule_id, " +
                                  "image_path, " +
                                "from_mail, " +
                                "to_mail, " +
                                "sub, " +
                                "transmission_id, " +
                                "bcc, " +
                                "cc, " +
                                "reply_to, " +
                                "scheduled_time, " +
                                "body, " +
                                 " created_by, " +
                                "created_date) " +
                                "VALUES (" +
                                "'" + msGetGid + "', " +
                                  "'" + msGetGid1 + "', " +
                                 "'" + lspath1 + "', " +
                                "'" + mail_from + "', " +
                                "'" + to + "', " +
                                "'" + sub.Replace("\"", "\\\"") + "', " +
                                "'" + objMdlMailCampaignResponse.results.id + "', " +
                                "'" + bcc + "', " +
                                "'" + cc + "', " +
                                "'" + reply_to + "', " +
                                 "'" + schedule_time + "', " +
                                "'" + body.Replace("\"", "\\\"") + "', " +
                                  "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";



                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                objResult.status = true;
                                objResult.message = "Mail Scheduled Successfully !!";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Scheduled Mail !!";
                            }
                        }
                    }
                }
            
                }
            catch (Exception ex)
            {
                objResult.status = false;
                objResult.message = " Error While Scheduled Mail !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + objResult.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }

       
        public void DaMailUpload(HttpRequest httpRequest, string user_gid, results objResult)
        {
            try
            {

            
            mailconfiguration getmailconfiguration = mailcrendentials();
            HttpFileCollection httpFileCollection;
            HttpPostedFile httpPostedFile;
            string basecode = httpRequest.Form[0];
            List<sendmail_list> hrDocuments = JsonConvert.DeserializeObject<List<sendmail_list>>(basecode);

            string mail_from = httpRequest.Form[1];
            string sub = httpRequest.Form[2];
            string to = httpRequest.Form[3];
            string body = httpRequest.Form[4];
            string bcc = httpRequest.Form[5];
            string cc = httpRequest.Form[6];
            //string reply_to = httpRequest.Form[7];
            //string leadbank_gid = httpRequest.Form[7];
            httpFileCollection = httpRequest.Files;
            string lsfilepath = string.Empty;
            string document_gid = string.Empty;
            string lspath, lspath1,lspath2;
            string lscompany_code = string.Empty;
            string base64String = string.Empty;
            string httpsUrl = string.Empty;
            msSQL = "select leadbank_gid from crm_smm_mailmanagement where to_mail='" + to + "'";
            string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
          
            List<DbAttachmentPath> dbattachmentpath = new List<DbAttachmentPath>();
           
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    List<MailAttachmentbase64> mailAttachmentbase64 = new List<MailAttachmentbase64>();

                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        MemoryStream ms = new MemoryStream();
                        var getdocumentdtl = hrDocuments.Where(a => a.AutoID_Key == httpFileCollection.AllKeys[i]).FirstOrDefault(); 
                        string msdocument_gid = objcmnfunctions.GetMasterGID("MILC");
                        httpPostedFile = httpFileCollection[i];
                        string type = httpPostedFile.ContentType;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        string FileExtension = httpPostedFile.FileName;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        bool status1;

                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                      
                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];

                        byte[] fileBytes = ms.ToArray();
                         base64String = Convert.ToBase64String(fileBytes);

                        ms.Close();

                        dbattachmentpath.Add(new DbAttachmentPath
                        { path = httpsUrl }
                        );
                        // Add attachment to the list
                        mailAttachmentbase64.Add(new MailAttachmentbase64
                        {
                            name = httpPostedFile.FileName,
                            type = type,
                            data = base64String
                        }); 
                        ms.Close();
                        }

                    string contentWithAttachments = $"\"attachments\":{JsonConvert.SerializeObject(mailAttachmentbase64)}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient("https://api.sparkpost.com");
                    var request = new RestRequest("/api/v1/transmissions", Method.POST);
                    request.AddHeader("Authorization", ""+getmailconfiguration.access_token+"");
                    request.AddHeader("Content-Type", "application/json");
                    var bodies = "{\"options\":{\"open_tracking\":true,\"click_tracking\":true},\"recipients\":[{\"address\":{\"email\":" + "\"" + to + "\"" + "}},{\"address\":{\"email\":" + "\"" + cc + "\"" + ",\"header_to\":" + "\"" + to + "\"" + "}},{\"address\":{\"email\":" + "\"" + bcc + "\"" + ",\"header_to\":" + "\"" + to + "\"" + "}}],\"content\":{\"from\":" + "\"" + getmailconfiguration.email_username + "<" + getmailconfiguration.sending_domain + ">\"" + ",\"headers\":{\"CC\":" + "\"" + cc + "\"" + "},\"subject\":" + "\"" + sub.Replace("\"", "\\\"").Replace("'", "\\\'") + "\"" + ",\"reply_to\":" + "\"" + getmailconfiguration.receiving_domain + "\"" + ",\"html\":" + "\"" + body.Replace("\"", "\\\"").Replace("'", "\\\'").Replace("<img ", "<img style=\\\"width: 500px\\\"") + "\"" + "," + contentWithAttachments +"}}";
                    var body_content = JsonConvert.DeserializeObject(bodies);
                    request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    sendmail_list objMdlMailCampaignResponse = new sendmail_list();
                    objMdlMailCampaignResponse = JsonConvert.DeserializeObject<sendmail_list>(errornetsuiteJSON);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Insert email details into the database
                        msGetGid = objcmnfunctions.GetMasterGID("MILC");
                        msSQL = "INSERT INTO crm_smm_mailmanagement (" +
                           "mailmanagement_gid, " +
                          "from_mail, " +
                          "to_mail, " +
                          "sub, " +
                          "transmission_id, " +
                          "bcc, " +
                          "cc, " +
                          "reply_to, " +
                          "leadbank_gid, " +
                          "body, " +
                           " created_by, " +
                          "created_date) " +
                          "VALUES (" +
                           "'" + msGetGid + "', " +
                          "'" + getmailconfiguration.sending_domain + "', " +
                          "'" + to + "', " +
                          "'" + sub.Replace("\"", "\\\"").Replace("'", "\\\'") + "', " +
                          "'" + objMdlMailCampaignResponse.results.id + "', " +
                          "'" + bcc + "', " +
                          "'" + cc + "', " +
                          "'" + getmailconfiguration.receiving_domain + "', " +
                          "'" + lsleadbank_gid + "', " +
                          "'" + body.Replace("\"", "\\\"").Replace("'", "\\\'").Replace("<img ", "<img style=\\\"width: 500px\\\"") + "', " +
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
                            "'" + msGetGid + "', " +
                            "'" + mailAttachmentbase64[i].name + "', " +
                             "'" + dbattachmentpath[i].path + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        if (mnResult == 1)
                        {
                            objResult.status = true;
                            objResult.message = "Mail Send Successfully !!";
                           
                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Error While Send Mail !!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }


                }
            }
            catch (Exception ex)
            {
                objResult.status = false;
                objResult.message = "Error While Send Mail !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }
        /////////////////mail send event by template/////////////////////

        public void DaGetMailEventDelivery(MdlMailCampaign values)

        {
            try 
            { 
            mailconfiguration getmailconfiguration = mailcrendentials();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://api.sparkpost.com");
            var request = new RestRequest("/api/v1/events/message?from=2023-10-10&events=delivery", Method.GET);
            request.AddHeader("Authorization", ""+getmailconfiguration.access_token+"");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            string errornetsuiteJSON = response.Content;
            mailevent_list objMdlMailCampaignResponse = new mailevent_list(); ;
            objMdlMailCampaignResponse = JsonConvert.DeserializeObject<mailevent_list>(errornetsuiteJSON);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Insert email details into the database
                for (int i = 0; i < objMdlMailCampaignResponse.results.ToArray().Length; i++)
                {

                    msSQL = "UPDATE crm_smm_mailmanagement " +
                    "SET status_delivery='" + objMdlMailCampaignResponse.results[i].type + "'" +
                    "WHERE transmission_id ='" + objMdlMailCampaignResponse.results[i].transmission_id + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
            }
            }
            catch(Exception ex)
            {
                values.message = "Error While Updating MailEvent Delivery";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }

        public void DaGetMailEventOpen(MdlMailCampaign values)

        {
            try 
            { 
                mailconfiguration getmailconfiguration = mailcrendentials();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://api.sparkpost.com");
                var request = new RestRequest("/api/v1/events/message?from=2023-10-10&events=open", Method.GET);
                request.AddHeader("Authorization", getmailconfiguration.access_token);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request);
                string errornetsuiteJSON = response.Content;
                mailevent_list objMdlMailCampaignResponse = new mailevent_list(); ;
                objMdlMailCampaignResponse = JsonConvert.DeserializeObject<mailevent_list>(errornetsuiteJSON);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Insert email details into the database
                    for (int i = 0; i < objMdlMailCampaignResponse.results.ToArray().Length; i++)
                    {
                        msSQL = "UPDATE crm_smm_mailmanagement " +
                        "SET status_open='" + objMdlMailCampaignResponse.results[i].type + "'" +
                        "WHERE transmission_id ='" + objMdlMailCampaignResponse.results[i].transmission_id + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
            }
             catch(Exception ex)
            {
                values.message = "Error While Updating MailEvent Open";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }
        public void DaGetMailEventClick(MdlMailCampaign values)

        {
            try
            {

            
            mailconfiguration getmailconfiguration = mailcrendentials();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://api.sparkpost.com");
            var request = new RestRequest("/api/v1/events/message?from=2023-10-10&events=click", Method.GET);
            request.AddHeader("Authorization",  getmailconfiguration.access_token);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            string errornetsuiteJSON = response.Content;
            mailevent_list objMdlMailCampaignResponse = new mailevent_list(); ;
            objMdlMailCampaignResponse = JsonConvert.DeserializeObject<mailevent_list>(errornetsuiteJSON);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Insert email details into the database
                for (int i = 0; i < objMdlMailCampaignResponse.results.ToArray().Length; i++)
                {

                    msSQL = "UPDATE crm_smm_mailmanagement " +
                    "SET status_click='" + objMdlMailCampaignResponse.results[i].type + "'" +
                    "WHERE transmission_id ='" + objMdlMailCampaignResponse.results[i].transmission_id + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Error While Updating MailEvent Click";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }




        }

        public void DaGetMailEventCount(MdlMailCampaign values)

        {
            try
            {

            msSQL = "Select(select count(a.status_delivery)) as deliverytotal_count,(select count(a.status_open))as opentotal_count," +
                "(select count(a.status_click)) as clicktotal_count,(select count(a.temp_mail_gid)) as mailsendtotal_count," +
                "(select count(distinct(b.temp_mail_gid))) as template_count from crm_smm_mailmanagement a " +
                "left join temp_crm_smm_mailmanagement b on a.temp_mail_gid = b.temp_mail_gid WHERE a.temp_mail_gid IS NOT NULL";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<mailcount_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new mailcount_list
                    {
                        deliverytotal_count = dt["deliverytotal_count"].ToString(),
                        opentotal_count = dt["opentotal_count"].ToString(),
                        clicktotal_count = dt["clicktotal_count"].ToString(),
                        mailsendtotal_count = dt["mailsendtotal_count"].ToString(),
                        template_count = dt["template_count"].ToString(),

                    });
                    values.mailcount_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching EventCount";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }

        public void DaSaveTemplate(mailsummary_list values, string user_gid)
        {
            try
            {

            mailconfiguration getmailconfiguration = mailcrendentials();
            string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
            string httpsUrl = string.Empty;
            //string lspath = ConfigurationManager.AppSettings["file_path"] + "/erpdocument/CRM/Mail/Template_Assets/";
            //if ((!System.IO.Directory.Exists(lspath)))
            //    System.IO.Directory.CreateDirectory(lspath);
             string lscompany_code = string.Empty;
            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);

            string dataUrl = values.body;
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


                                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Mail/Template/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid +'.'+ fileExtension, fileExtension, imageStream);
                      


                            }

                        }
                    }
                }
                //httpsUrl = ConfigurationManager.AppSettings["Mailprotocol"].ToString() + ConfigurationManager.AppSettings["MailhostURL"].ToString() + msdocument_gid + "." + fileExtension;

                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/Template/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + '.'+fileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                 '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                 '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];


            }
            else
            {
                    values.message = "Error While Adding Campaign!!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * *********************" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }



                msGetGid1 = objcmnfunctions.GetMasterGID("TMIL");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://api.sparkpost.com");
            var request = new RestRequest("/api/v1/templates", Method.POST);
            request.AddHeader("Authorization",  getmailconfiguration.access_token);
            request.AddHeader("Content-Type", "application/json");
            string originalHtml = values.body.Replace("\"", "\\\"").Replace("'", "\\\'");
            string temp_body = Regex.Replace(originalHtml, @"data:[^;]+;base64,[^""]+", httpsUrl + "\\");
            var body = "{\"id\":" + "\"" + msGetGid1 + "\"" + ",\"name\":" + "\"" + values.template_name + "\"" + ",\"published\":true,\"options\":{\"open_tracking\":true,\"click_tracking\":true},\"content\":{\"from\":{\"email\":" + "\"" +getmailconfiguration.sending_domain+ "\"" + ",\"name\":" + "\"" + getmailconfiguration.email_username + "\"" + "},\"subject\":" + "\"" + values.sub.Replace("\"", "\\\"").Replace("'", "\\\'") + "\"" + ",\"reply_to\":" + "\"" +getmailconfiguration.receiving_domain + "\"" + ",\"html\":" + "\"" + temp_body + "\"" + "}}";
            var body_content = JsonConvert.DeserializeObject(body);
            request.AddParameter("application/json", body_content, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string errornetsuiteJSON = response.Content;
            mailtemplate_list objMdlMailCampaignResponse = new mailtemplate_list();
            objMdlMailCampaignResponse = JsonConvert.DeserializeObject<mailtemplate_list>(errornetsuiteJSON);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                msSQL = "INSERT INTO temp_crm_smm_mailmanagement (" +
                            "temp_mail_gid, " +
                           "from_mail, " +
                            "template_name, " +
                           "sub, " +
                           "bcc, " +
                           "cc, " +
                           "reply_to, " +
                           "body, " +
                            " created_by, " +
                           "created_date) " +
                           "VALUES (" +
                             "'" + msGetGid1 + "', " +
                           "'" + getmailconfiguration.sending_domain + "', " +
                            "'" + values.template_name.Replace("\"", "\\\"").Replace("'", "\\\'") + "', " +
                           "'" + values.sub.Replace("\"", "\\\"").Replace("'", "\\\'") + "', " +
                           "'" + values.bcc + "', " +
                           "'" + values.cc + "', " +
                           "'" +getmailconfiguration.receiving_domain + "', " +
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
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * *********************" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");


                    }

                }

            }
            catch (Exception ex)
            {
                values.message = "Error While Adding Mail Tempalte";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }

        public void DaTemplateSummary(MdlMailCampaign values)

        {
            try
            {


            msSQL = "SELECT DATE_FORMAT(a.created_date, '%d-%m-%Y') AS date, a.template_name ,a.temp_mail_gid,a.template_flag," +
                "(select count(b.mailmanagement_gid)) as sent_mail," +
                "(select count(b.status_delivery)) as send_mail,(select count(b.status_open)) as read_mail,(select count(b.status_click)) as click_mail  " +
                "FROM  temp_crm_smm_mailmanagement a left join crm_smm_mailmanagement b on a.temp_mail_gid= b.temp_mail_gid " +
                "GROUP BY a.created_date, a.template_name, a.temp_mail_gid order by a.created_date desc;";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<mailtemplate_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new mailtemplate_list
                    {
                        
                        created_date = dt["date"].ToString(),
                        template_name = dt["template_name"].ToString(),
                        temp_mail_gid = dt["temp_mail_gid"].ToString(),
                        sent_mail = dt["sent_mail"].ToString(),
                        send_mail = dt["send_mail"].ToString(),
                        read_mail = dt["read_mail"].ToString(),
                        click_mail = dt["click_mail"].ToString(),
                        template_flag = dt["template_flag"].ToString(),



                    });
                    values.mailtemplate_list = getmodulelist;
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

         public result DaSendTemplate(mailtemplatesendsummary_list values,string user_gid)
        {
            result result = new result();
            try
            {
               
                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() => {
                        HttpContext.Current = ctx;
                        DaTemplatesend(values.temp_mail_gid, values.mailsendchecklist , user_gid);
                    }));
                    t.Start();
                
                result.status =true;
                result.message = "Mail sent successfully!";
               
            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Mail";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }

        public void DaTemplatesend(string temp_mail_gid ,List<mailsendchecklist> mailsendchecklist, string user_gid)
        {
            try
            {


                mailconfiguration getmailcredentials = mailcrendentials();
                foreach (var item in mailsendchecklist)
                {

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient("https://api.sparkpost.com");
                    var request = new RestRequest("/api/v1/transmissions", Method.POST);
                    request.AddHeader("Authorization", getmailcredentials.access_token);
                    request.AddHeader("Content-Type", "application/json");
                    var body = "{\"options\":{\"open_tracking\":true,\"click_tracking\":true},\"content\":{\"template_id\":" + "\"" + temp_mail_gid + "\"" + "},\"recipients\":[{\"address\":{\"email\":" + "\"" + item.email + "\"" + "}}]}";
                    //var body = "{\"content\":{\"template_id\":" + "\"" + values.temp_mail_gid + "\"" + "},\"recipients\":[{\"address\":{\"email\":" + "\"" + lsemail + "\"" + ",\"name\":" + "\"" + lsname + "\"" + "}}]}";
                    var body_content = JsonConvert.DeserializeObject(body);
                    request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    sendmail_list objMdlMailCampaignResponse = new sendmail_list();
                    objMdlMailCampaignResponse = JsonConvert.DeserializeObject<sendmail_list>(errornetsuiteJSON);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("MILC");
                        msSQL = "INSERT INTO crm_smm_mailmanagement (" +
                               "mailmanagement_gid, " +
                            "leadbank_gid, " +
                               "temp_mail_gid, " +
                            "to_mail, " +
                            "transmission_id, " +
                            "sent_flag, " +
                             " created_by, " +
                            "created_date) " +
                            "VALUES (" +
                            "'" + msGetGid + "', " +
                              "'" + item.leadbank_gid + "', " +
                              "'" + temp_mail_gid + "', " +
                              "'" + item.email + "', " +
                              "'" + objMdlMailCampaignResponse.results.id + "', " +
                              "'Y', " +
                              "'" + user_gid + "'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                          
                        }
                        else
                        {
                           
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }

        public void DaMailTemplateSendSummary(MdlMailCampaign values,string temp_mail_gid)

        {
            try
            {
                msSQL = "SELECT a.leadbank_gid,a.leadbank_name,a.source_gid,a.leadbank_region,a.customer_type,CONCAT(a.leadbank_address1, ' ', a.leadbank_address2, ' ', a.leadbank_city, ' ', a.leadbank_state, ' ', a.leadbank_country) as address," +
                    "b.mailmanagement_gid,b.sent_flag,b.temp_mail_gid,(e.user_firstname) as lead_assignedto,f.source_name,g.region_name,h.template_name,i.email FROM crm_trn_tleadbank a LEFT JOIN (SELECT leadbank_gid,mailmanagement_gid,sent_flag,temp_mail_gid FROM " +
                    "crm_smm_mailmanagement WHERE temp_mail_gid = '"+temp_mail_gid+"') b ON b.leadbank_gid = a.leadbank_gid  left join crm_trn_tlead2campaign c on c.leadbank_gid = a.leadbank_gid  left join  hrm_mst_temployee d on d.employee_gid = c.assign_to" +
                    " left join adm_mst_tuser e on e.user_gid = d.user_gid left join crm_mst_tsource f on f.source_gid = a.source_gid left join crm_mst_tregion g on g.region_gid = a.leadbank_region left join temp_crm_smm_mailmanagement h " +
                    "on h.temp_mail_gid='"+temp_mail_gid+ "' left join crm_trn_tleadbankcontact i on i.leadbank_gid=a.leadbank_gid where i.main_contact ='Y'  GROUP BY a.leadbank_gid;";



                //msSQL = "select a.leadbank_gid,a.leadbank_name,a.source_gid,a.leadbank_region,a.customer_type,concat(a.leadbank_address1, ' ', a.leadbank_address2, ' ', a.leadbank_city, ' ', a.leadbank_state, ' ', a.leadbank_country) as address," +
                //    "b.mailmanagement_gid,b.temp_mail_gid,b.sent_flag,(e.user_firstname) as lead_assignedto,f.source_name,g.region_name,h.template_name,i.email from crm_trn_tleadbank a left join crm_smm_mailmanagement b  on b.leadbank_gid = a.leadbank_gid  left join crm_trn_tlead2campaign c " +
                //    "on c.leadbank_gid = a.leadbank_gid  left join  hrm_mst_temployee d on d.employee_gid = c.assign_to left join adm_mst_tuser e on e.user_gid = d.user_gid left join crm_mst_tsource f on f.source_gid = a.source_gid " +
                //    "left join crm_mst_tregion g on g.region_gid = a.leadbank_region left join temp_crm_smm_mailmanagement h on h.temp_mail_gid='"+ temp_mail_gid + "'left join crm_trn_tleadbankcontact i on i.leadbank_gid=a.leadbank_gid   group by a.leadbank_gid;";
                  dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<mailtemplatesendsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new mailtemplatesendsummary_list
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
                            template_name = dt["template_name"].ToString(),
                            region = dt["region_name"].ToString(),
                            mailmanagement_gid = dt["mailmanagement_gid"].ToString(),
                            temp_mail_gid = dt["temp_mail_gid"].ToString(),
                            sent_flag = dt["sent_flag"].ToString(),
                            lead_assignedto = dt["lead_assignedto"].ToString(),



                        });
                        values.mailtemplatesendsummary_list = getmodulelist;
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

        public void DaMailTemplateView(string temp_mail_gid, MdlMailCampaign values)

        {
            try
            {


                msSQL = "Select body,template_name,sub,date_format(created_date,'%d-%m-%Y %H:%i:%s')as created_date from temp_crm_smm_mailmanagement where temp_mail_gid= '" + temp_mail_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<mailtemplate_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new mailtemplate_list
                        {
                            body = dt["body"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            sub = dt["sub"].ToString(),
                            created_date = dt["created_date"].ToString(),

                        });
                        values.mailtemplate_list = getmodulelist;
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

        public void DaMailSendStatusSummary(string temp_mail_gid,MdlMailCampaign values)

        {
            try
            {


                msSQL1 = "select template_name from temp_crm_smm_mailmanagement where  temp_mail_gid='" + temp_mail_gid + "' ";
                string lstemplate_name;
                lstemplate_name = objdbconn.GetExecuteScalar(msSQL1);

                msSQL = "select status_open,status_delivery,to_mail,status_click,direction,date_format(created_date,'%d-%m-%Y')as created_dates,sub,body,to_mail from crm_smm_mailmanagement where temp_mail_gid='" + temp_mail_gid + "' order by created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<mailtemplatesendsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new mailtemplatesendsummary_list
                        {
                            status_open = dt["status_open"].ToString(),
                            status_delivery = dt["status_delivery"].ToString(),
                            status_click = dt["status_click"].ToString(),
                            to_mail = dt["to_mail"].ToString(),
                            date = dt["created_dates"].ToString(),
                            body = dt["body"].ToString(),
                            sub = dt["sub"].ToString(),
                            to = dt["to_mail"].ToString(),
                            direction = dt["direction"].ToString(),
                            template_name = lstemplate_name


                        });
                        values.mailtemplatesendsummary_list = getmodulelist;
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

        public void DaIndividualMailSummary(MdlMailCampaign values, string leadbank_gid)
        {
            try
            {

                mailconfiguration getmailcredentials = mailcrendentials();
                msSQL = "select a.mailmanagement_gid,a.direction,a.temp_mail_gid,a.from_mail,a.to_mail,a.status_open,a.leadbank_gid,date_format(a.created_date,'%d-%m-%Y')  as created_date," +
                    " time_format(a.created_date,'%h:%i:%s')as created_time, case when a.temp_mail_gid is null then a.body else b.body end as body,case when a.temp_mail_gid is null then a.sub else b.sub end as sub " +
                    "from crm_smm_mailmanagement a left join temp_crm_smm_mailmanagement b on b.temp_mail_gid = a.temp_mail_gid " +
                    "where leadbank_gid='" + leadbank_gid + "' order by a.created_date desc;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<mailsummary_list>();
                values.sending_domain = getmailcredentials.sending_domain;
                values.receiving_domain = getmailcredentials.receiving_domain;
                if (dt_datatable.Rows.Count != 0)

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new mailsummary_list
                        {
                            mailmanagement_gid = dt["mailmanagement_gid"].ToString(),
                            mail_from = dt["from_mail"].ToString(),
                            to = dt["to_mail"].ToString(),
                            sub = dt["sub"].ToString(),
                            temp_mail_gid = dt["temp_mail_gid"].ToString(),
                            body = dt["body"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            status_open = dt["status_open"].ToString(),
                            direction = dt["direction"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            created_time = dt["created_time"].ToString(),
                            sending_domain = values.sending_domain,
                            receiving_domain = values.receiving_domain,

                        });
                        values.mailsummary_list = getModuleList;
                    }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Error While Fecthing Individual Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }

        public void DaGetMailView(string mailmanagement_gid, MdlMailCampaign values)
        {
            try
            {


                msSQL = "UPDATE `boba_tea`.`crm_smm_mailmanagement` SET `read_flag` = 'Y' where mailmanagement_gid= '" + mailmanagement_gid + "';";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "select a.mailmanagement_gid,a.from_mail,a.to_mail,a.direction,case when a.temp_mail_gid is not null then b.sub else a.sub end as sub,a.leadbank_gid," +
                    "case when a.temp_mail_gid is not null then b.body else a.body end as body,a.status_open,a.image_path,a.temp_mail_gid ,date_format(a.created_date,'%d-%m-%y')  as created_date,c.document_path,  " +
                    "time_format(a.created_date,'%h:%i:%s')as created_time,c.document_name  from crm_smm_mailmanagement a  left join temp_crm_smm_mailmanagement b on a.temp_mail_gid=b.temp_mail_gid" +
                    " left join crm_trn_tfiles c on a.mailmanagement_gid=c.mailmanagement_gid  where a.mailmanagement_gid= '" + mailmanagement_gid + "'order by a.created_date desc;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<mailsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new mailsummary_list
                        {
                            mailmanagement_gid = dt["mailmanagement_gid"].ToString(),
                            mail_from = dt["from_mail"].ToString(),
                            to = dt["to_mail"].ToString(),
                            sub = dt["sub"].ToString(),
                            body = dt["body"].ToString(),
                            image_path = dt["image_path"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            status_open = dt["status_open"].ToString(),
                            direction = dt["direction"].ToString(),
                            created_time = dt["created_time"].ToString(),
                            document_path = dt["document_path"].ToString(),
                            document_name = dt["document_name"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),


                        });
                        values.mailsummary_list = getModuleList;
                    }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Vieing Mail";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }

        public mailconfiguration mailcrendentials()
        {
            mailconfiguration getmailcredentials = new mailconfiguration();
            try { 
                msSQL = "select *from crm_smm_mail_service;";
                objOdbcDataReader  = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader .HasRows == true)
                {
                     
                    getmailcredentials.base_url = objOdbcDataReader ["base_url"].ToString();
                    getmailcredentials.access_token = objOdbcDataReader ["access_token"].ToString();
                    getmailcredentials.receiving_domain = objOdbcDataReader ["receiving_domain"].ToString();
                    getmailcredentials.sending_domain = objOdbcDataReader ["sending_domain"].ToString();
                    getmailcredentials.email_username = objOdbcDataReader ["email_username"].ToString();
                     
                }
                objOdbcDataReader .Close();
            }
            catch(Exception ex) {
               
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            return getmailcredentials;
        }

        public result DaAddaslead(mdlAddaslead values, string user_gid)
        {


            result objresult = new result();
            try
            {


                msSQL = "SELECT leadbank_gid FROM crm_smm_mailmanagement WHERE to_mail = '" + values.email_address + "' AND(leadbank_gid IS not NULL and leadbank_gid != '')";
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

        public void DaGetMailEventCountSummary(MdlMailCampaign values)

        {
            try
            {


                msSQL = " select(select count(a.status_delivery)) as deliverytotal_count,(select count(a.status_open))as opentotal_count," +
                   "(select count(a.status_click)) as clicktotal_count from crm_smm_mailmanagement a";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<mailcount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new mailcount_list
                        {
                            deliverytotal_count = dt["deliverytotal_count"].ToString(),
                            opentotal_count = dt["opentotal_count"].ToString(),
                            clicktotal_count = dt["clicktotal_count"].ToString(),

                        });
                        values.mailcount_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching MailBox Summary Tiles Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + values.message + "*******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaUpdateMailTemplateStatus(mailtemplatesendsummary_list values, string user_gid)
        {
            try
            {


                msSQL = "select template_flag from temp_crm_smm_mailmanagement where temp_mail_gid='" + values.temp_mail_gid + "'";
                string template_flag = objdbconn.GetExecuteScalar(msSQL);
                if (template_flag == "Y")
                {
                    msSQL = "update temp_crm_smm_mailmanagement set template_flag='N' where temp_mail_gid='" + values.temp_mail_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = "update temp_crm_smm_mailmanagement set template_flag='Y' where temp_mail_gid='" + values.temp_mail_gid + "'";
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

        //Customer Type Dropdown
        public void DaGetCustomerTypeSummary(MdlMailCampaign values)
        {
            msSQL = "select customertype_gid,customer_type from crm_mst_tcustomertype ORDER BY customertype_gid ASC";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<customertype_list3>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new customertype_list3
                    {
                        customertype_gid3 = dt["customertype_gid"].ToString(),
                        customer_type3 = dt["customer_type"].ToString(),
                    });
                    values.customertype_list3 = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

    }
}
