using ems.utilities.Functions;
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
using System.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.IO;
using System.Web.Http.Results;
using System.Xml.Linq;
using System.Web.Http.Filters;
using System.Reflection;

using static OfficeOpenXml.ExcelErrorValue;
using System.Text.RegularExpressions;
using static ems.crm.Models.MdlLeadbank360;
using System.Runtime.Remoting.Contexts;
using System.Text;


namespace ems.crm.DataAccess
{
    public class DaLeadbank360
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objfinance = new finance_cmnfunction();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, msGetGid2, msGetGid3, lsdesignation_code, lscustomer_gid, lswhatsapp_contactid, lssource_gid, lswhatsapp_gid, lsCode, lslead_email, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, final_path;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        int mnResult6;

        // Code by SNEHITH

        //get indivdual contact details for whatsapp
        public void DaGetWhatsappLeadContact(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {

                msSQL = " select wh_id from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lswhatsapp_contactid = objOdbcDataReader["wh_id"].ToString();

                }

                msSQL = "select SUBSTRING(displayName, 1, 1) AS first_letter, displayName,Wvalue,id from crm_smm_whatsapp where id = '" + lswhatsapp_contactid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadwhatscontactlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadwhatscontactlist
                        {
                            whatsapp_gid = dt["id"].ToString(),
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),
                            first_letter = dt["first_letter"].ToString(),
                        });
                        values.leadwhatscontactlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Whatsapp Leadcount!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        //get indivdual contact message for whatsapp

        public result dacreatecontact(mdlCreateContactInput values, string user_gid)
        {
            int i = 0;
            result objresult = new result();
            Rootobject objRootobject = new Rootobject();
            string contactjson = "{\"displayName\":\"" + values.displayName + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + values.value + "\"}],\"firstName\":\"" + values.firstName + "\",\"gender\":\"" + values.gender + "\",\"lastName\":\"" + values.lastName + "\"}";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
            var request = new RestRequest(ConfigurationManager.AppSettings["messagebirdcontact"].ToString(), Method.POST);
            request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
            request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var responseoutput = response.Content;
            objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);


            try
            {

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    msSQL = "insert into crm_smm_whatsapp(id,wkey,wvalue,displayName,firstName,lastName,gender,created_date,created_by)values(" +
                            "'" + objRootobject.id + "'," +
                            "'" + values.key + "'," +
                            "'" + values.value + "'," +
                            "'" + values.displayName + "'," +
                            "'" + values.firstName + "'," +
                            "'" + values.lastName + "'," +
                            "'" + values.gender + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = "update crm_trn_tleadbank set" +
                               "whatsappcontact_id = '" + objRootobject.id + "'" +
                               "where leadbank_gid = '" + values.leadbank_gid + "'";
                        mnResult4 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult4 == 1)
                    {
                        objresult.status = true;
                        objresult.message = "Contact created successfully!";
                    }
                    else
                    {
                        objresult.message = "Error occured while adding contact!";
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error occured while posting contact!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            return objresult;

        }


        public void DaGetWhatsappLeadMessage(MdlLeadbank360 values, string leadbank_gid, string user_gid)
        {

            try
            {

                msSQL = " select wvalue from crm_smm_whatsapp where leadbank_gid  = '" + leadbank_gid + "'";
                string lswhatsapp_mobile = objdbconn.GetExecuteScalar(msSQL);

                if (lswhatsapp_mobile != null && lswhatsapp_mobile != "")
                {


                    msSQL = " select wh_id from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                    string lswhatsapp_contactid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "update crm_trn_twhatsappmessages set read_flag = 'Y' where contact_id ='" + lswhatsapp_contactid + "' and direction='incoming'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "SELECT a.template_image,a.template_body,a.template_footer,a.message_id,a.project_id,d.template_body,d.footer,d.media_url,b.leadbank_gid,a.created_date,a.direction,a.contact_id,b.lastName,b.firstName, a.message_text,a.type,a.status," +
                            " CONCAT(DATE_FORMAT(a.created_date, '%e %b %y, '), DATE_FORMAT(a.created_date, '%h:%i %p')) AS time," +
                            " b.wvalue AS identifierValue,SUBSTRING(b.displayName, 1, 1) AS first_letter, b.displayName,c.document_name,c.document_path" +
                            " FROM crm_trn_twhatsappmessages a LEFT JOIN crm_smm_whatsapp b ON b.id = a.contact_id" +
                            " left join crm_trn_tfiles c on a.message_id = c.message_gid " +
                            " left join crm_smm_whatsapptemplate d on d.project_id = a.project_id" +
                            " WHERE  contact_id = '" + lswhatsapp_contactid + "'ORDER BY a.created_date DESC, time DESC";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<leadwhatsmessagelist>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new leadwhatsmessagelist
                            {
                                displayName = dt["displayName"].ToString(),
                                first_letter = dt["first_letter"].ToString(),
                                firstName = dt["firstName"].ToString(),
                                lastName = dt["lastName"].ToString(),
                                message_text = dt["message_text"].ToString(),
                                type = dt["type"].ToString(),
                                status = dt["status"].ToString(),
                                time = dt["time"].ToString(),
                                contact_id = dt["contact_id"].ToString(),
                                direction = dt["direction"].ToString(),
                                created_date = dt["created_date"].ToString(),
                                message_id = dt["message_id"].ToString(),
                                identifierValue = dt["identifierValue"].ToString(),
                                document_name = dt["document_name"].ToString(),
                                document_path = dt["document_path"].ToString(),
                                project_id = dt["project_id"].ToString(),
                                template_body = dt["template_body"].ToString(),
                                footer = dt["template_footer"].ToString(),
                                template_image = dt["template_image"].ToString(),
                                media_url = dt["media_url"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                            });
                            values.leadwhatsmessagelist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {
                    msSQL = " select mobile from crm_trn_tleadbankcontact where leadbank_gid  = '" + leadbank_gid + "' and main_contact ='Y'";
                    string mobile = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select contact_id from crm_trn_twhatsappmessages where identifiervalue  = '" + mobile + "'";
                    string contact_id = objdbconn.GetExecuteScalar(msSQL);
                    if (contact_id != null && contact_id != "")
                    {
                        msSQL = " select leadbank_name from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                        string displayName = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "insert into crm_smm_whatsapp(id,wvalue,displayName,created_date,created_by)values(" +
                                  "'" + contact_id + "'," +
                                  "'" + mobile + "'," +
                                  "'" + displayName + "'," +
                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                  "'" + user_gid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update crm_smm_whatsapp set leadbank_gid='" + leadbank_gid + "' where id='" + contact_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update crm_trn_tleadbank set wh_id='" + contact_id + "' where leadbank_gid='" + leadbank_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                    else
                    {
                        msSQL = " select leadbank_name from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                        string displayName = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select mobile from crm_trn_tleadbankcontact where leadbank_gid  = '" + leadbank_gid + "' and main_contact ='Y'";
                        string mobiles = objdbconn.GetExecuteScalar(msSQL);

                        whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                        Rootobject objRootobject = new Rootobject();
                        string contactjson = "{\"displayName\":\"" + displayName + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + mobiles + "\"}],\"firstName\":\"" + displayName + "\",\"lastName\":\"" + displayName + "\"}";
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                        var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/contacts", Method.POST);
                        request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                        request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        var responseoutput = response.Content;
                        objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            msSQL = "insert into crm_smm_whatsapp(id,wvalue,displayName,created_date,created_by)values(" +
                                    "'" + objRootobject.id + "'," +
                                    "'" + mobiles + "'," +
                                    "'" + displayName + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    "'" + user_gid + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "update crm_smm_whatsapp set leadbank_gid='" + leadbank_gid + "' where id = '" + objRootobject.id + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "update crm_trn_tleadbank set wh_id='" + objRootobject.id + "' where leadbank_gid='" + leadbank_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        else
                        {

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Whatsapp Lead Message!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        //post indivdual contact message for whatsapp
        public result DaPostLeadWhatsappMessage(leadwhatsappsendmessage values, string user_gid)
        {
            int i = 0;
            Result objsendmessage = new Result();

            result objresult = new result();
            if (values.project_id != null)
            {
                string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + values.project_id + "\",\"version\":\"" + values.version + "\",\"locale\":\"en\"}}";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/8f60b013-65ac-4db2-ad01-e9d0ee7c0d5d/channels/c21b849f-5e1a-49d2-a7dc-414a96b19391/messages", Method.POST);
                request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string waresponse = response.Content;
                objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                try
                {

                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        msSQL = "insert into crm_trn_twhatsappmessages(" +
                                 "message_id," +
                                 "contact_id," +
                                 "direction," +
                                 "type," +
                                 "message_text," +
                                 "content_type," +
                                  "project_id," +
                                 "version_id," +
                                 "status," +
                                 "created_date)" +
                                 "values(" +
                                 "'" + objsendmessage.id + "'," +
                                 "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                 "'" + objsendmessage.direction + "'," +
                                "'" + objsendmessage.body.type + "',";
                        if (objsendmessage.body.type == "text")
                        {
                            msSQL += "'" + objsendmessage.body.text.text.Replace("'", "\\'") + "'," +
                                     "null,";
                        }
                        else if (objsendmessage.body.type == "list")
                        {
                            msSQL += "'" + objsendmessage.body.list.text.Replace("'", "\\'") + "'," +
                                     "null,";
                        }
                        else if (objsendmessage.body.type == "image")
                        {
                            msSQL += "'" + objsendmessage.body.image.images[0].mediaUrl.Replace("'", "\\'") + "'," +
                                     "null,";
                        }
                        else
                        {
                            msSQL += "'" + objsendmessage.body.file.files[0].mediaUrl.Replace("'", "\\'") + "'," +
                                     "'" + objsendmessage.body.file.files[0].contentType.Replace("'", "\\'") + "',";
                        }
                        msSQL += "'" + objsendmessage.template.projectId + "'," +
                                 "'" + objsendmessage.template.version + "'," +
                                 "'" + objsendmessage.status + "'," +
                                 "'" + objsendmessage.createdAt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            objresult.status = true;
                            objresult.message = "Delivered!";
                        }
                        else
                        {
                            objresult.message = "Failed!";
                        }
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Sending failed";
                    }
                }
                catch (Exception ex)
                {
                    values.message = "Exception occured while Sending Whatsapp Template!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }

            }
            else
            {
                Servicewindow objsendmessage1 = new Servicewindow();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/8f60b013-65ac-4db2-ad01-e9d0ee7c0d5d/channels/c21b849f-5e1a-49d2-a7dc-414a96b19391/contacts/" + values.contact_id, Method.GET);
                request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                IRestResponse response1 = client.Execute(request);
                string waresponse1 = response1.Content;
                objsendmessage1 = JsonConvert.DeserializeObject<Servicewindow>(waresponse1);
                if (objsendmessage1.serviceWindowExpireAt != null)
                {
                    string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"" + values.sendtext + "\"}}}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request1 = new RestRequest("/workspaces/8f60b013-65ac-4db2-ad01-e9d0ee7c0d5d/channels/c21b849f-5e1a-49d2-a7dc-414a96b19391/messages", Method.POST);
                    request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response2 = client.Execute(request);
                    string waresponse = response2.Content;
                    objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                    try
                    {

                        if (response1.StatusCode == HttpStatusCode.Accepted)
                        {
                            msSQL = "insert into crm_trn_twhatsappmessages(" +
                                     "message_id," +
                                     "contact_id," +
                                     "direction," +
                                     "type," +
                                     "message_text," +
                                     "content_type," +
                                     "status," +
                                     "created_date)" +
                                     "values(" +
                                     "'" + objsendmessage.id + "'," +
                                     "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                     "'" + objsendmessage.direction + "'," +
                                    "'" + objsendmessage.body.type + "',";
                            if (objsendmessage.body.type == "text")
                            {
                                msSQL += "'" + objsendmessage.body.text.text.Replace("'", "\\'") + "'," +
                                         "null,";
                            }
                            else if (objsendmessage.body.type == "list")
                            {
                                msSQL += "'" + objsendmessage.body.list.text.Replace("'", "\\'") + "'," +
                                         "null,";
                            }
                            else if (objsendmessage.body.type == "image")
                            {
                                msSQL += "'" + objsendmessage.body.image.images[0].mediaUrl.Replace("'", "\\'") + "'," +
                                         "null,";
                            }
                            else
                            {
                                msSQL += "'" + objsendmessage.body.file.files[0].mediaUrl.Replace("'", "\\'") + "'," +
                                         "'" + objsendmessage.body.file.files[0].contentType.Replace("'", "\\'") + "',";
                            }
                            msSQL += "'" + objsendmessage.status + "'," +
                                     "'" + objsendmessage.createdAt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                objresult.status = true;
                                objresult.message = "Delivered!";
                            }
                            else
                            {
                                objresult.message = "Failed!";
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Service Window closed";
                        }
                    }
                    catch (Exception ex)
                    {
                        values.message = "Exception occured while Sending Whatsapp!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }

                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Service Window closed";
                }
            }

            return objresult;
        }
        //get indivdual email details based on lead email
        public void DaGetEmailSendDetails(MdlLeadbank360 values, string leadbank_gid)
        {
            try
            {

                msSQL = " select  email from crm_trn_tleadbank a left join crm_trn_tleadbankcontact b on b.leadbank_gid=a.leadbank_gid where b.leadbank_gid='" + leadbank_gid + "' and b.main_contact ='Y'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lslead_email = objOdbcDataReader["email"].ToString();

                }

                msSQL = " select mailmanagement_gid, sub, body, created_by, created_date, image_path, from_mail, to_mail, transmission_id, bcc, cc, reply_to, status_delivery, status_open, status_click, scheduled_time, temp_mail_gid, schedule_id from crm_smm_mailmanagement where to_mail = '" + lslead_email + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leademailsendlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leademailsendlist
                        {
                            mailmanagement_gid = dt["mailmanagement_gid"].ToString(),
                            sub = dt["sub"].ToString(),
                            body = dt["body"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            image_path = dt["image_path"].ToString(),
                            from_mail = dt["from_mail"].ToString(),
                            to_mail = dt["to_mail"].ToString(),
                            transmission_id = dt["transmission_id"].ToString(),
                            bcc = dt["bcc"].ToString(),
                            cc = dt["cc"].ToString(),
                            reply_to = dt["reply_to"].ToString(),
                            status_delivery = dt["status_delivery"].ToString(),
                            status_open = dt["status_open"].ToString(),
                            status_click = dt["status_click"].ToString(),
                            scheduled_time = dt["scheduled_time"].ToString(),
                            temp_mail_gid = dt["temp_mail_gid"].ToString(),
                            schedule_id = dt["schedule_id"].ToString(),


                        });
                        values.leademailsendlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Email Send Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Post indivdual text email based on lead email
        public void DaLeadMailSend(leadmailsummary_list values, string user_gid, result objResult)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://api.sparkpost.com");
                var request = new RestRequest("/api/v1/transmissions", Method.POST);
                request.AddHeader("Authorization", "14e9f31c9e5002fb9dcf7e28b89f7d0d92759a4c");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"options\":{\"open_tracking\":true,\"click_tracking\":false},\"recipients\":[{\"address\":{\"email\":" + "\"" + values.to + "\"" + "}},{\"address\":{\"email\":" + "\"" + values.cc + "\"" + ",\"header_to\":" + "\"" + values.to + "\"" + "}},{\"address\":{\"email\":" + "\"" + values.bcc + "\"" + ",\"header_to\":" + "\"" + values.to + "\"" + "}}],\"content\":{\"from\":" + "\"" + values.mail_from + "\"" + ",\"headers\":{\"CC\":" + "\"" + values.cc + "\"" + "},\"subject\":" + "\"" + values.sub + "\"" + ",\"reply_to\":" + "\"" + values.reply_to + "\"" + ",\"html\":" + "\"" + values.body.Replace("\"", "\\\"") + "\"" + "}}";
                var body_content = JsonConvert.DeserializeObject(body);
                request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string errornetsuiteJSON = response.Content;
                sendmail_list objMdlMailCampaignResponse = new sendmail_list(); ;
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
                        "body, " +
                         " created_by, " +
                        "created_date) " +
                        "VALUES (" +
                        "'" + msGetGid + "', " +
                        "'" + values.mail_from + "', " +
                        "'" + values.to + "', " +
                        "'" + values.sub + "', " +
                        "'" + objMdlMailCampaignResponse.results.id + "', " +
                        "'" + values.bcc + "', " +
                        "'" + values.cc + "', " +
                        "'" + values.reply_to + "', " +
                        "'" + values.body + "', " +
                          "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        objResult.status = true;
                        objResult.message = "Mail Send Successfully !!";
                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Error While Sending Mail !!";
                    }
                }

            }
            catch (Exception ex)
            {
                objResult.status = false;
                //objResult.message = ex.ToString();
                values.message = "Exception occured while Sending Lead Mail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        //Post indivdual text with attachment email based on lead email
        public void DaLeadMailUpload(HttpRequest httpRequest, string user_gid, result objResult)
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
            try
            {
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
                        var bodies = "{\"options\":{\"open_tracking\":true,\"click_tracking\":false},\"recipients\":[{\"address\":{\"email\":" + "\"" + to + "\"" + "}},{\"address\":{\"email\":" + "\"" + cc + "\"" + ",\"header_to\":" + "\"" + to + "\"" + "}},{\"address\":{\"email\":" + "\"" + bcc + "\"" + ",\"header_to\":" + "\"" + to + "\"" + "}}],\"content\":{\"from\":" + "\"" + mail_from + "\"" + ",\"headers\":{\"CC\":" + "\"" + cc + "\"" + "},\"subject\":" + "\"" + sub + "\"" + ",\"reply_to\":" + "\"" + reply_to + "\"" + ",\"html\":" + "\"" + body.Replace("\"", "\\\"") + "\"" + ",\"attachments\":[{\"name\":" + "\"" + httpPostedFile.FileName + "\"" + ",\"type\":" + "\"" + type + "\"" + ",\"data\":" + "\"" + basecode + "\"" + "}]}}";
                        var body_content = JsonConvert.DeserializeObject(bodies);
                        request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        string errornetsuiteJSON = response.Content;
                        sendmail_list objMdlMailCampaignResponse = new sendmail_list(); ;
                        objMdlMailCampaignResponse = JsonConvert.DeserializeObject<sendmail_list>(errornetsuiteJSON);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Insert email details into the database
                            msGetGid = objcmnfunctions.GetMasterGID("MILC");
                            msSQL = "INSERT INTO crm_smm_mailmanagement (" +
                                 "mailmanagement_gid, " +
                                "from_mail, " +
                                "image_path, " +
                                "to_mail, " +
                                "sub, " +
                                "transmission_id, " +
                                "bcc, " +
                                "cc, " +
                                "reply_to, " +
                                "body, " +
                                 " created_by, " +
                                "created_date) " +
                                "VALUES (" +
                                 "'" + msGetGid + "', " +
                                "'" + mail_from + "', " +
                                "'" + lspath1 + "', " +
                                "'" + to + "', " +
                                "'" + sub + "', " +
                                "'" + objMdlMailCampaignResponse.results.id + "', " +
                                "'" + bcc + "', " +
                                "'" + cc + "', " +
                                "'" + reply_to + "', " +
                                "'" + body + "', " +
                                  "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                objResult.status = true;
                                objResult.message = "Mail Send Successfully !!";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Send Mail !!";
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                objResult.status = false;
                //objResult.message = ex.ToString();
                objResult.message = "Exception occured while Upload Lead Mail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Get sales order details
        public void DaGetLeadOrderDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {

                msSQL = " select customer_gid from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lscustomer_gid = objOdbcDataReader["customer_gid"].ToString();

                }
                msSQL = " select distinct  k.leadbank_gid,j.leadbankcontact_gid,i.lead2campaign_gid,a.salesorder_gid, a.so_referenceno1, date_format(a.salesorder_date,'%d-%m-%Y') as salesorder_date,c.user_firstname,a.so_type,a.currency_code, " +
                    "  a.customer_contact_person, a.salesorder_status,a.currency_code, " +
                    " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                    " a.customer_name, " +
                    " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                    " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact,invoice_flag " +
                    "  from smr_trn_tsalesorder a " +
                    " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
                    " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid " +
                    " left join crm_trn_tleadbank k on a.customer_gid=k.customer_gid " +
                    " left join crm_trn_tlead2campaign i on i.leadbank_gid = k.leadbank_gid" +
                    " left join crm_trn_tleadbankcontact j on j.leadbank_gid = k.leadbank_gid" +
                    " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                    " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                    " where 1=1  and k.leadbank_gid ='" + leadbank_gid + "' and j.main_contact ='Y' order by  a.salesorder_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadorderdetailslist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadorderdetailslist
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            //so_typecurrency_code = dt["so_typecurrency_code"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),

                        });
                        values.leadorderdetailslist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Order Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Get sales Quotation details
        public void DaGetLeadQuotationDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {

                msSQL = "select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "'";
                lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select distinct i.lead2campaign_gid,j.leadbankcontact_gid,d.leadbank_gid,a.quotation_gid, a.quotation_referenceno1, date_format(a.quotation_date,'%d-%m-%Y') as quotation_date,c.user_firstname,a.quotation_type,a.currency_code, " +
                     " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                     " a.customer_name," +
                     " a.customer_contact_person, a.quotation_status,a.enquiry_gid, " +
                     " case when a.contact_mail is null then concat(e.leadbankcontact_name,'/',e.mobile,'/',e.email) " +
                     " when a.contact_mail is not null then concat(a.customer_contact_person,' / ',a.contact_no,' / ',a.contact_mail) end as contact " +
                     " from smr_trn_treceivequotation a " +
                     " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                     " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                     " left join crm_trn_tleadbank d on d.customer_gid=a.customer_gid " +
                     " left join crm_trn_tlead2campaign i on i.leadbank_gid = d.leadbank_gid " +
                     " left join crm_trn_tleadbankcontact j on j.leadbank_gid = d.leadbank_gid " +
                     " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                     " left join crm_trn_tleadbankcontact e on e.leadbank_gid=d.leadbank_gid " +
                     " where d.customer_gid='" + lscustomer_gid + "' and j.main_contact ='Y'order by a.quotation_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadquotationdetailslist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadquotationdetailslist
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            quotation_gid = dt["quotation_gid"].ToString(),
                            quotation_referenceno1 = dt["quotation_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            quotation_type = dt["quotation_type"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            quotation_status = dt["quotation_status"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            contact = dt["contact"].ToString(),



                        });
                        values.leadquotationdetailslist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Quotation Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        //Get sales Invoice details
        public void DaGetLeadInvoiceDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {

                msSQL = " select customer_gid from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lscustomer_gid = objOdbcDataReader["customer_gid"].ToString();

                }
                msSQL = " select distinct a.invoice_gid, case when a.invoice_reference like '%AREF%' then j.agreement_referencenumber else  " +
                        " cast(concat(s.so_referenceno1,if(s.so_referencenumber<>'',concat(' ',' | ',' ',s.so_referencenumber),'') ) as char)  end as so_referencenumber, " +
                        " a.invoice_refno, " +
                        " a.mail_Status,a.customer_gid,date_format(a.invoice_date, '%d-%m-%Y') as 'invoice_date'," +
                        " a.invoice_reference,a.additionalcharges_amount,a.discount_amount,  " +
                        " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', " +
                        " a.payment_flag,  format(a.initialinvoice_amount,2) as initialinvoice_amount,a.invoice_status,a.invoice_flag,  " +
                        " format(a.invoice_amount,2) as invoice_amount, " +
                        " c.customer_name,a.currency_code,  " +
                        " a.customer_contactnumber  as mobile,a.invoice_from,i.directorder_gid,a.progressive_invoice " +
                        " from rbl_trn_tinvoice a  " +
                        " left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid  " +
                        " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid  " +
                        " left join crm_trn_tleadbank p on p.customer_gid = c.customer_gid  " +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code  " +
                        " left join smr_trn_tsalesorder s on a.invoice_reference = s. salesorder_gid  " +
                        " left join crm_trn_tagreement j on j.agreement_gid = a.invoice_reference  " +
                        " left join smr_trn_tdeliveryorder i on s.salesorder_gid=i.salesorder_gid " +
                        " where a. customer_gid='" + lscustomer_gid + "' and a.customer_gid!='' order by a.invoice_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadinvoicedetailslist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadinvoicedetailslist
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            so_referencenumber = dt["so_referencenumber"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            mail_status = dt["mail_Status"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_reference = dt["invoice_reference"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            payment_flag = dt["payment_flag"].ToString(),
                            initialinvoice_amount = dt["initialinvoice_amount"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            directorder_gid = dt["directorder_gid"].ToString(),
                            progressive_invoice = dt["progressive_invoice"].ToString(),

                        });
                        values.leadinvoicedetailslist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Invoice Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Get Lead count details
        public void DaGetLeadCountDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {

                msSQL = " select customer_gid from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = "select count(enquiry_gid)as count ,FORMAT(SUM(potorder_value),2)AS amount,date_format(created_date,'%b-%d')as dates," +
                //     "'ENQUIRY'as source from smr_trn_tsalesenquiry where customer_gid='" + lscustomer_gid + "'  group by date_format(created_date,'%b-%d') union all " +
                //     "select count(quotation_gid)as count ,FORMAT(SUM(Grandtotal),2)AS amount,date_format(created_date,'%b-%d')as dates," +
                //     "'QUOTATION'as source from smr_trn_treceivequotation where customer_gid='" + lscustomer_gid + "' group by date_format(created_date,' %b-%d')" +
                //     " union all select count(salesorder_gid) as count,FORMAT(SUM(Grandtotal),2)AS amount,date_format(created_date,'%b-%d')as dates," +
                //     "'ORDER'as source from smr_trn_tsalesorder  where customer_gid='" + lscustomer_gid + "'group by date_format(created_date,' %b-%d') " +
                //     "union all select count(invoice_gid)as count,FORMAT(SUM(invoice_amount),2)AS amount,date_format(created_date,'%b-%d')as dates," +
                //     "'INVOICE'as source from rbl_trn_tinvoice where customer_gid='" + lscustomer_gid + "' group by date_format(created_date,' %b-%d');";
                // dt_datatable = objdbconn.GetDataTable(msSQL);
                // var getModuleList = new List<leadsaleschart>();
                // if (dt_datatable.Rows.Count != 0)
                // {
                //     foreach (DataRow dt in dt_datatable.Rows)
                //     {
                //         getModuleList.Add(new leadsaleschart
                //         {
                //             count = dt["count"].ToString(),
                //             amount = Convert.ToSingle(dt["amount"]),
                //             dates = dt["dates"].ToString(),
                //             source = dt["source"].ToString(),
                //         });
                //         values.leadsaleschart = getModuleList;
                //     }
                // }
                // dt_datatable.Dispose();

                msSQL = "SELECT Months, SUM(quotation_count) AS quotation_count, SUM(enquiry_count) AS enquiry_count, SUM(order_count) AS order_count," +
                    "sum(invoice_count) as invoice_count FROM ( SELECT DATE_FORMAT(a.created_date,'%b-%y') AS Months, COUNT(a.quotation_gid) AS " +
                    "quotation_count, 0 AS enquiry_count, 0 AS order_count,0 as invoice_count,created_date AS ordermonth FROM smr_trn_treceivequotation a" +
                    " WHERE   customer_gid='" + lscustomer_gid + "' and customer_gid!=''  GROUP BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b-%y') AS Months," +
                    " 0 AS quotation_count, COUNT(a.enquiry_gid) AS enquiry_count, 0 AS order_count,0 as invoice_count,created_date AS ordermonth " +
                    "FROM smr_trn_tsalesenquiry a WHERE  customer_gid='" + lscustomer_gid + "' and customer_gid!='' GROUP  BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b-%y')" +
                    " AS Months,  0 AS quotation_count, 0 AS enquiry_count, COUNT(a.salesorder_gid) AS order_count,0 as invoice_count,created_date AS ordermonth " +
                    "FROM smr_trn_tsalesorder a WHERE  customer_gid='" + lscustomer_gid + "'and customer_gid!='' GROUP BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b-%y') AS Months," +
                    " 0 AS quotation_count, 0 AS enquiry_count,0 AS order_count,COUNT(a.invoice_gid) AS invoice_count,created_date AS ordermonth FROM rbl_trn_tinvoice a" +
                    " WHERE customer_gid='" + lscustomer_gid + "' and customer_gid!='' GROUP BY Months) AS combined_data GROUP BY Months order by ordermonth;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var leadsaleschart = new List<leadsaleschart>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        leadsaleschart.Add(new leadsaleschart
                        {
                            quotation_count = (dt["quotation_count"].ToString()),
                            enquiry_count = (dt["enquiry_count"].ToString()),
                            order_count = (dt["order_count"].ToString()),
                            invoice_count = (dt["invoice_count"].ToString()),
                            Months = (dt["Months"].ToString()),


                        });
                        values.leadsaleschart = leadsaleschart;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Count Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Get Lead Document details
        public void DaGetLeadDocumentDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {

                msSQL = "Select document_gid,document_title,document_upload,leadbank_gid ,document_type, remarks,date_format(created_date,'%d-%m-%Y') as created_date" +
              " from crm_trn_tdocument where leadbank_gid='" + leadbank_gid + "' and document_type='mylead'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leaddocumentdetails>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leaddocumentdetails
                        {
                            document_gid = dt["document_gid"].ToString(),
                            document_title = dt["document_title"].ToString(),
                            document_upload = dt["document_upload"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            document_type = dt["document_type"].ToString(),
                            created_date = dt["created_date"].ToString(),


                        });
                        values.leaddocumentdetails = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Document Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Document Upload 
        public void DaLeadDocumentUpload(HttpRequest httpRequest, string user_gid, results objresult)
        {
            //result objresult = new result();
            try
            {
                // Access form data using updated structure
                string leadbank_gid = httpRequest.Form["leadbank_gid"];
                string document_title = httpRequest.Form["document_title"];
                string remarks = httpRequest.Form["remarks"];
                string customer_gid = httpRequest.Form["customer_gid"];
                string lscompany_code = string.Empty;
                string lspath, lspath1;
                if (customer_gid != "" && leadbank_gid=="")
                {
                    msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + customer_gid + "'";
                    leadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = " select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "'";
                    customer_gid = objdbconn.GetExecuteScalar(msSQL);
                }

                msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                HttpFileCollection httpFileCollection;
                HttpPostedFile httpPostedFile;
                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["upload_file"] + "/" + lscompany_code + "/" + "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

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
                        string msdocument_gid = objcmnfunctions.GetMasterGID("BDNP");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        //string lsfile_gid = msdocument_gid + FileExtension;   
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        lspath = ConfigurationManager.AppSettings["upload_file"] + "/" + lscompany_code + "/" + "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);

                        //bool status1;
                        //status1 = objcmnfunctions.UploadStream(msdocument_gid + FileExtension, FileExtension, ms);
                        //string local_path = "E:/Angular15/AngularUI/src";
                        ms.Close();
                        //lspath = "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        //lspath1 = "/erpdocument" + "/" + lscompany_code + "/" + "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension;

                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension;

                        msSQL = " insert into crm_trn_tdocument( " +
                               " document_gid," +
                               " leadbank_gid," +
                               " customer_gid," +
                               " document_title, " +
                               " document_type, " +
                               " remarks," +
                               " document_upload," +
                               " created_by," +
                               " created_date" +
                               " )values( " +
                               "'" + msdocument_gid + "'," +
                               "'" + leadbank_gid + "'," +
                               "'" + customer_gid + "'," +
                               "'" + document_title + "'," +
                               "'mylead'," +
                               "'" + remarks + "'," +
                               "'" + lspath + "', " +
                               "'" + user_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            objresult.status = true;
                            objresult.message = "Document Uploaded Successfully";
                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Error while uploading document!!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +  objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        }
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Kindly Choose The File!!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                }
            }
            catch (Exception ex)
            {
                objresult.status = false;
                //objresult.message = "Exception occured while uploading document!";
                objresult.message = "Exception occured while uploading document!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Document download
        public void DaLeadDocumentdownload(string document_gid, MdlLeadbank360 values)
        {

            try
            {

                msSQL = "SELECT document_upload, document_title FROM crm_trn_tdocument WHERE document_gid = '" + document_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<document_download>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new document_download
                        {
                            document_upload = dt["document_upload"].ToString(),
                            document_title = dt["document_title"].ToString()
                        });
                    }
                    values.document_download = getModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Document Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Notes summary
        public void DaGetNotesSummary(MdlLeadbank360 values, string leadbank_gid)
        {
            try
            {

                msSQL = "SELECT leadbank_gid, internal_notes, 'LEAD' AS source, '0' AS s_no, created_date " +
                    "FROM (SELECT leadbank_gid, internal_notes, created_date FROM crm_trn_tappointment " +
                    "WHERE leadbank_gid = '" + leadbank_gid + "' and internal_notes!='' ORDER BY created_date DESC) AS ordered_lead UNION ALL" +
                    " SELECT leadbank_gid, internal_notes, 'NOTES' AS source, s_no, created_date FROM" +
                    " (SELECT leadbank_gid, internal_notes, s_no, created_date FROM crm_trn_tleadnotes " +
                    "WHERE leadbank_gid = '" + leadbank_gid + "' and internal_notes!='' ORDER BY created_date DESC) AS ordered_notes ORDER BY created_date DESC;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<notes>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new notes
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            source = dt["source"].ToString(),
                            s_no = dt["s_no"].ToString(),
                        });
                        values.notes = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Notes Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Upload Notes details
        public void DaLeadNotesUpload(notes values, string user_gid, result objResult)
        {
            try
            {
                msSQL = "update crm_trn_tlead2campaign set internal_notes = '" + values.internalnotestext_area + "' " +
                   "where leadbank_gid = '" + values.leadgig + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Notes Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while updating Notes!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Uploading Lead Notes!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        // Lead Basic Details
        public void DaGetLeadBasicDetails(MdlLeadbank360 values, string appointment_gid, string user_gid)
        {
            string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
            string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
            gmailconfiguration getgmailcredentials = gmailcrendentials(integrated_gmail);
            try
            {
                msSQL = "select sref from adm_mst_tmoduleangular  where module_name ='Compose Mail'";
                string lsmodule_ref = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select mail_service from crm_smm_tmailconfig where switch_flag ='Y';";
                string mail_service = objdbconn.GetExecuteScalar(msSQL);


                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_name ='Click To Call'";
                string lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select b.leadbank_gid,b.customer_gid,format(a.potential_value,2)as potential_value,b.leadbank_name, date_format(b.created_date, '%e %b %Y') as created_date," +
                        " b.customer_type,c.leadbankcontact_name,c.email,c.mobile,concat_ws(',',c.address1,c.address2,c.city,c.state,c.pincode) as address from crm_trn_tappointment a " +
                        " left join crm_trn_tleadbank b on b.leadbank_gid = a.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact c on c.leadbank_gid = b.leadbank_gid " +
                        " where a.appointment_gid = '" + appointment_gid + "' and a.appointment_gid !='' and c.main_contact ='Y' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var leadbasicdetails = new List<leadbasicdetails_list>();

                values.gmail_address = getgmailcredentials.gmail_address;
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        leadbasicdetails.Add(new leadbasicdetails_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            email = dt["email"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            gmail_address = values.gmail_address,
                            lsmodule_ref = lsmodule_ref,
                            mail_service = mail_service,
                            lsshopify_flag = lsshopify_flag,
                        });
                        values.leadbasicdetails_list = leadbasicdetails;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Basic Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        // Lead Basic teleDetails
        public void DaGetLeadBasicTeleDetails(MdlLeadbank360 values, string leadbank_gid, string user_gid)
        {
            string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
            string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
            gmailconfiguration getgmailcredentials = gmailcrendentials(integrated_gmail);
            try
            {
                msSQL = "select sref from adm_mst_tmoduleangular  where module_name ='Compose Mail'";
                string lsmodule_ref = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select mail_service from crm_smm_tmailconfig where switch_flag ='Y';";
                string mail_service = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_name ='Click To Call'";
                 string lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select a.leadbank_gid,a.customer_gid,format(a.potential_value,2)as potential_value,a.leadbank_name, date_format(a.created_date, '%e %b %Y') as created_date," +
                   " a.customer_type,b.leadbankcontact_name,b.email,b.mobile from crm_trn_tleadbank a " +
                " left join crm_trn_tleadbankcontact b on b.leadbank_gid = a.leadbank_gid where a.leadbank_gid = '" + leadbank_gid + "' and b.main_contact ='Y'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var leadbasicdetails = new List<leadbasicdetails_list>();

                values.gmail_address = getgmailcredentials.gmail_address;
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        leadbasicdetails.Add(new leadbasicdetails_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            email = dt["email"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            gmail_address = values.gmail_address,
                            lsmodule_ref = lsmodule_ref,
                            mail_service = mail_service,
                            lsshopify_flag = lsshopify_flag,
                        });
                        values.leadbasicdetails_list = leadbasicdetails;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Basic Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //edit contact details//
        public void DaGetupdatecontactdetails(contactedit_list values)
        {

            try
            {

                msSQL = "select customer_gid from crm_trn_tleadbank where leadbank_gid='" + values.leadbank_gid + "'";
                lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update  crm_trn_tleadbankcontact set " +
                        " leadbankcontact_name = '" + values.displayName + "'," +
                        " mobile = '" + values.mobile.e164Number + "'," +
                        " email = '" + values.email + "'," +
                        " address1 = '" + values.leadbank_address1 + "'," +
                        " address2 = '" + values.leadbank_address2 + "'," +
                        " city = '" + values.leadbank_city + "'," +
                        " state = '" + values.leadbank_state + "'," +
                        " country_gid = '" + values.country_name + "'," +
                        " pincode = '" + values.leadbank_pin + "'" +
                        " where leadbank_gid = '" + values.leadbank_gid + "' and main_contact ='Y'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (lscustomer_gid != null && lscustomer_gid.Trim() != "")
                {
                    msSQL = "update crm_mst_tcustomercontact set" +
                      " customercontact_name = '" + values.displayName + "'," +
                      " mobile = '" + values.mobile.e164Number + "'," +
                      " email =  '" + values.email + "'," +
                      " address1 = '" + values.leadbank_address1 + "'" +
                       " address2 = '" + values.leadbank_address2 + "'," +
                        " city = '" + values.leadbank_city + "'," +
                        " country_gid = '" + values.country_name + "'," +
                        " zip_code = '" + values.leadbank_pin + "'" +
                      " where customer_gid = '" + lscustomer_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Update Customer Contact Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

            if (mnResult == 1)
            {

                Rootobject objRootobject = new Rootobject();
                string contactjson = "{\"displayName\":\"" + values.displayName + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + values.mobile.e164Number + "\"}],\"firstName\":\"" + values.displayName + "\",\"lastName\":\"" + values.displayName + "\"}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest(ConfigurationManager.AppSettings["messagebirdcontact"].ToString(), Method.POST);
                request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseoutput = response.Content;
                objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);

                try
                {

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        msSQL = "insert into crm_smm_whatsapp(leadbank_gid,id,wkey,wvalue,displayName,created_date)values(" +
                                " '" + values.leadbank_gid + "'," +
                                "'" + objRootobject.id + "'," +
                                "'phonenumber'," +
                                "'" + values.mobile.e164Number + "'," +
                                "'" + values.displayName + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "update crm_trn_tleadbank set wh_flag = 'Y', wh_id = '" + objRootobject.id + "' where leadbank_gid = '" + values.leadbank_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }

                    }
                }
                catch (Exception ex)
                {
                    values.message = "Exception occured while Updating Contact Details in Whatsapp!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }


                values.status = true;
                values.message = "Contact Updated Successfully !!";
            }

            else
            {
                values.status = false;
                values.message = "Error while updating contact !!";
            }


        }
        //Get edit contact summary
        public void DaGetEditContactdetails(string leadbank_gid, MdlLeadbank360 values, string user_gid)
        {

            try
            {

                msSQL = " select leadbank_gid,leadbankcontact_name,email,mobile,address1,address2,city,state,pincode,b.country_name,a.country_gid " +
                    " from crm_trn_tleadbankcontact a" +
                    " left join adm_mst_tcountry b on b.country_gid = a.country_gid" +
                    " where a.leadbank_gid='" + leadbank_gid + "' and a.main_contact ='Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<contactedit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new contactedit_list
                        {
                            displayName = dt["leadbankcontact_name"].ToString(),
                            email = dt["email"].ToString(),
                            mobile1 = dt["mobile"].ToString(),
                            address1 = dt["address1"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            address2 = dt["address2"].ToString(),
                            city = dt["city"].ToString(),
                            pincode = dt["pincode"].ToString(),
                            state = dt["state"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            country_gid = dt["country_gid"].ToString(),

                        });
                        values.contactedit_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Edit Contact Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Raise enquiry approval//
        //public void DaGetenquiryapproved(MdlLeadbank360 values, string leadbank_gid)

        //{

        //    msSQL = " Select a.leadbank_gid, a.leadbank_name ," +
        //        " from crm_trn_tleadbank a  " +
        //        " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
        //        " where b.customer_gid is not null and a.leadbank_gid='" + leadbank_gid + "' and b.status='Active' " +
        //        " order by a.leadbank_name asc ";

        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //    if (mnResult == 0)
        //    {
        //        values.status = false;
        //        values.message = "Change lead to customer to raise enquiry !!";
        //    }
        //}

        //Get sales Enquiry details
        public void DaGetEnquiryDetails(MdlLeadbank360 values, string leadbank_gid)
        {
            try
            {

                msSQL = "select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "'";
                lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select distinct concat(a.enquiry_gid,' / ',a.enquiry_type) as enquiry_refno, a.enquiry_gid,a.leadbank_gid,n.leadbankcontact_gid,f.lead2campaign_gid," +
                        " date_format(a.enquiry_date, '%d-%m-%Y') as enquiry_date,b.user_firstname,a.customer_name,a.branch_gid," +
                        " a.customer_gid,a.lead_status," +
                        " concat(o.region_name, ' / ', m.leadbank_city, ' / ', m.leadbank_state) as region_name," +
                        " a.enquiry_referencenumber,a.enquiry_status,a.enquiry_type," +
                        " concat(b.user_firstname, ' ', b.user_lastname) as campaign,a.enquiry_remarks," +
                        " a.contact_person,a.contact_email,a.contact_address," +
                        " case when a.contact_person is null then concat(n.leadbankcontact_name,' / ',n.mobile,' / ',n.email)" +
                        " when a.contact_person is not null then concat(a.contact_person,' / ',a.contact_number,' / ',a.contact_email) end as contact_details," +
                        " r.leadstage_name,a.enquiry_type from smr_trn_tsalesenquiry a" +
                        " left join crm_trn_tleadbank m on m.leadbank_gid = a.leadbank_gid" +
                        " left join crm_trn_tleadbankcontact n on n.leadbank_gid = a.leadbank_gid" +
                        " left join crm_mst_tregion o on m.leadbank_region = o.region_gid" +
                        " left join crm_trn_tenquiry2campaign p on p.enquiry_gid = a.enquiry_gid" +
                        " left join crm_trn_tlead2campaign f on f.leadbank_gid = a.leadbank_gid" +
                        " left join crm_mst_tleadstage r on r.leadstage_gid = p.leadstage_gid" +
                        " left join smr_trn_tcampaign q on q.campaign_gid = p.campaign_gid" +
                        " left join hrm_mst_temployee d on d.employee_gid = p.assign_to" +
                        " left join adm_mst_tuser b on b.user_gid = d.user_gid" +
                        " where m.leadbank_gid = '" + leadbank_gid + "' and n.main_contact ='Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getEnquiryList = new List<Enquiry_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getEnquiryList.Add(new Enquiry_list
                        {
                            enquiry_refno = dt["enquiry_refno"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            enquiry_date = dt["enquiry_date"].ToString(),
                            enquiry_status = dt["enquiry_status"].ToString(),
                            enquiry_type = dt["enquiry_type"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            user_firstname = dt["user_firstname"].ToString()
                        });
                        values.Enquiry_list = getEnquiryList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Enquiry Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Get Gid details
        public void DaGetGidDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {

                msSQL = "select a.leadbank_gid,a.campaign_gid,b.leadbankcontact_gid,c.leadstage_name from crm_trn_tappointment a" +
                   " left join crm_trn_tleadbankcontact b on b.leadbank_gid = a.leadbank_gid" +
                   " left join crm_mst_tleadstage c on a.leadstage_gid = c.leadstage_gid" +
                   " where a.appointment_gid = '" + leadbank_gid + "' and b.main_contact ='Y' and b.main_contact ='Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getGidList = new List<gid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getGidList.Add(new gid_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),


                        });
                        values.gid_list = getGidList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Gid Detail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTeleGidDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {

                msSQL = "select a.leadbank_gid,a.lead2campaign_gid,a.campaign_gid,b.leadbankcontact_gid,c.leadstage_name from crm_trn_ttelelead2campaign a" +
                   " left join crm_trn_tleadbankcontact b on b.leadbank_gid = a.leadbank_gid" +
                   " left join crm_mst_tleadstage c on a.leadstage_gid = c.leadstage_gid" +
                   " where a.leadbank_gid = '" + leadbank_gid + "' and b.main_contact ='Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getGidList = new List<gid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getGidList.Add(new gid_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),


                        });
                        values.gid_list = getGidList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Gid Detail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        // Add to customer
        public void DaAddtocustomer(MdlLeadbank360 values, string leadbank_gid, string displayName, string mobile, string email, string address1, string taxsegment_name, string address2,string customer_city, string currency, string postal_code, string countryname, string user_gid)
        {

            try
            {
                string lsemail, lsleadbankcontact_name;

                msSQL = "select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "'";
                lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);
                if (lscustomer_gid == "" || lscustomer_gid == null)
                {
                    msSQL = "select customer_gid from crm_mst_tcustomer where email='" + email + "'";
                     lsemail = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select customercontact_name from crm_mst_tcustomer where customercontact_name='" + displayName + "'";
                     lsleadbankcontact_name = objdbconn.GetExecuteScalar(msSQL);
                    if (lsemail=="" || lsleadbankcontact_name == "")
                    {
                       msSQL = "select a.leadbank_gid,b.leadbankcontact_gid,a.leadbank_name,a.company_website,a.leadbank_address1," +
                    " a.leadbank_address2,a.leadbank_city,a.leadbank_country,a.leadbank_region,a.leadbank_state,a.leadbank_pin," +
                    " a.customer_type,b.leadbankcontact_name,b.email,b.mobile,b.leadbankbranch_name,b.main_contact,b.designation," +
                    " b.address1,b.address2,b.state,b.city,b.country_gid,b.region_name,b.fax,b.fax_area_code,b.fax_country_code" +
                    " from crm_trn_tleadbank a" +
                    " left join crm_trn_tleadbankcontact b on b.leadbank_gid = a.leadbank_gid" +
                    " where a.leadbank_gid ='" + leadbank_gid + "' and b.main_contact ='Y'";

                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getGidList = new List<addtocustomer>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getGidList.Add(new addtocustomer
                                {
                                    leadbank_gid = dt["leadbank_gid"].ToString(),
                                    leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                                    customer_name = dt["leadbank_name"].ToString(),
                                    company_website = dt["company_website"].ToString(),
                                    customer_address = dt["leadbank_address1"].ToString(),
                                    customer_address2 = dt["leadbank_address2"].ToString(),
                                    customer_city = dt["leadbank_city"].ToString(),
                                    countryname_gid = dt["leadbank_country"].ToString(),
                                    region = dt["leadbank_region"].ToString(),
                                    customer_state = dt["leadbank_state"].ToString(),
                                    customer_pin = dt["leadbank_pin"].ToString(),
                                    customer_type = dt["customer_type"].ToString(),
                                    customercontact_name = dt["leadbankcontact_name"].ToString(),
                                    email = dt["email"].ToString(),
                                    mobile = dt["mobile"].ToString(),
                                    customerbranch_name = dt["leadbankbranch_name"].ToString(),
                                    main_contact = dt["main_contact"].ToString(),
                                    designation = dt["designation"].ToString(),
                                    address1 = dt["address1"].ToString(),
                                    address2 = dt["address2"].ToString(),
                                    state = dt["state"].ToString(),
                                    city = dt["city"].ToString(),
                                    country_gid = dt["country_gid"].ToString(),
                                    region_name = dt["region_name"].ToString(),
                                    fax = dt["fax"].ToString(),
                                    fax_area_code = dt["fax_area_code"].ToString(),
                                    fax_country_code = dt["fax_country_code"].ToString(),

                                });
                                values.addtocustomer = getGidList;
                            }
                        }
                        result objresult = new result();

                        //customer table
                        msGetGid = objcmnfunctions.GetMasterGID("CC");
                        msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC' order by finyear asc limit 0,1 ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);

                        string lscustomer_code = "CC-" + "00" + lsCode;
                        //string lscustomercode = "H.Q";
                        //string lscustomer_branch = "H.Q";


                        foreach (var item in getGidList)
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("BCRM");
                            msGetGid1 = objcmnfunctions.GetMasterGID("BCCM");

                            msSQL = " insert into crm_mst_tcustomer (" +
                               " customer_gid," +
                               " customer_id, " +
                               " customer_name, " +
                               " company_website, " +
                               " customer_address, " +
                               " customer_address2," +
                               " customer_city," +
                               " currency_gid," +
                               " customer_country," +
                               " customer_region," +
                               " customer_state," +
                               " gst_number ," +
                               " customer_pin ," +
                               " customer_type ," +
                              " status ," +
                              " taxsegment_gid ," +
                              " created_by," +
                               "created_date" +
                                ") values (" +
                               "'" + msGetGid + "', " +
                               "'" + lscustomer_code + "'," +
                               "'" + item.customer_name + "'," +
                               "'" + item.company_website + "'," +
                               "'" + address1 + "'," +
                               "'" + address2 + "'," +
                               "'" + customer_city + "'," +
                               "'" + currency + "'," +
                               "'" + countryname + "'," +
                               "'" + item.region_name + "'," +
                               "'" + item.customer_state + "'," +
                               "'" + item.gst_number + "'," +
                               "'" + postal_code + "'," +
                                "'" + item.customer_type + "'," +
                                "'Active'," +
                                "'" + taxsegment_name + "'," +
                                "'" + user_gid + "'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                objfinance.finance_vendor_debitor("Sales", lscustomer_code, item.customer_name, msGetGid, user_gid);
                                string trace_comment = " Added a customer on " + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_customer");

                                msSQL = " insert into crm_mst_tcustomercontact  (" +
                                " customercontact_gid," +
                                " customer_gid," +
                                " customercontact_name, " +
                                " customerbranch_name, " +
                                " email, " +
                                " mobile, " +
                                " main_contact, " +
                                " designation," +
                                " address1," +
                                " address2," +
                                " state," +
                                " city," +
                                " country_gid," +
                                " region," +
                                " fax, " +
                                " zip_code, " +
                                " fax_area_code, " +
                                " fax_country_code," +
                                " gst_number, " +
                                " created_by," +
                                " created_date" +
                                ") values (" +
                                "'" + msGetGid1 + "', " +
                                "'" + msGetGid + "', " +
                                "'" + displayName + "'," +
                                "'" + item.customerbranch_name + "'," +
                                "'" + email + "'," +
                                "'" + mobile + "'," +
                                "'Y'," +
                                "'" + item.designation + "'," +
                                "'" + address1 + "'," +
                                "'" + address2 + "'," +
                                "'" + item.customer_state + "'," +
                                "'" + customer_city + "'," +
                                "'" + countryname + "'," +
                                "'" + item.region_name + "'," +
                                "'" + item.fax + "'," +
                                "'" + postal_code + "'," +
                                "'" + item.fax_area_code + "'," +
                                "'" + item.fax_country_code + "'," +
                                "'" + item.gst_number + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                msSQL = "  update crm_trn_tleadbank set customer_gid ='" + msGetGid + "' where leadbank_gid = '" + leadbank_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL = "  update crm_trn_tleadbankcontact set leadbankcontact_name ='" + displayName + "',mobile ='" + mobile + "'," +
                                    "email ='" + email + "',address1 ='" + address1 + "',address2 ='" + address2 + "',city='"+ customer_city + "'," +
                                    "pincode='" + postal_code + "',country_gid='" + countryname + "',region_name='" + item.region_name + "' where leadbank_gid = '" + leadbank_gid + "' and main_contact ='Y'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "Lead added to customer";
                                    values.addtocustomer[0].customer_gid = msGetGid;
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error while adding lead to customer!!";
                                }
                            }
                        }
                    }
                    else
                    {
                        msSQL = "select a.leadbank_gid,b.leadbankcontact_gid,a.leadbank_name,a.company_website,a.leadbank_address1," +
                  " a.leadbank_address2,a.leadbank_city,a.leadbank_country,a.leadbank_region,a.leadbank_state,a.leadbank_pin," +
                  " a.customer_type,b.leadbankcontact_name,b.email,b.mobile,b.leadbankbranch_name,b.main_contact,b.designation," +
                  " b.address1,b.address2,b.state,b.city,b.country_gid,b.region_name,b.fax,b.fax_area_code,b.fax_country_code" +
                  " from crm_trn_tleadbank a" +
                  " left join crm_trn_tleadbankcontact b on b.leadbank_gid = a.leadbank_gid" +
                  " where a.leadbank_gid ='" + leadbank_gid + "' and b.main_contact ='Y'";

                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getGidList = new List<addtocustomer>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getGidList.Add(new addtocustomer
                                {
                                    leadbank_gid = dt["leadbank_gid"].ToString(),
                                    leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                                    customer_name = dt["leadbank_name"].ToString(),
                                    company_website = dt["company_website"].ToString(),
                                    customer_address = dt["leadbank_address1"].ToString(),
                                    customer_address2 = dt["leadbank_address2"].ToString(),
                                    customer_city = dt["leadbank_city"].ToString(),
                                    countryname_gid = dt["leadbank_country"].ToString(),
                                    region = dt["leadbank_region"].ToString(),
                                    customer_state = dt["leadbank_state"].ToString(),
                                    customer_pin = dt["leadbank_pin"].ToString(),
                                    customer_type = dt["customer_type"].ToString(),
                                    customercontact_name = dt["leadbankcontact_name"].ToString(),
                                    email = dt["email"].ToString(),
                                    mobile = dt["mobile"].ToString(),
                                    customerbranch_name = dt["leadbankbranch_name"].ToString(),
                                    main_contact = dt["main_contact"].ToString(),
                                    designation = dt["designation"].ToString(),
                                    address1 = dt["address1"].ToString(),
                                    address2 = dt["address2"].ToString(),
                                    state = dt["state"].ToString(),
                                    city = dt["city"].ToString(),
                                    country_gid = dt["country_gid"].ToString(),
                                    region_name = dt["region_name"].ToString(),
                                    fax = dt["fax"].ToString(),
                                    fax_area_code = dt["fax_area_code"].ToString(),
                                    fax_country_code = dt["fax_country_code"].ToString(),

                                });
                                values.addtocustomer = getGidList;
                            }
                        }
                        result objresult = new result();
                        foreach (var item in getGidList)
                        {

                            msSQL = "select customer_gid from crm_trn_tcustomer where email='" + email + "' and customercontact_name='" + displayName + "'";
                            lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = "UPDATE crm_mst_tcustomer SET " +
                          "customer_name = '" + item.customer_name + "', " +
                          "company_website = '" + item.company_website + "', " +
                          "customer_address = '" + address1 + "', " +
                          "customer_address2 = '" + address2 + "', " +
                          "customer_city = '" + customer_city + "', " +
                          "currency_gid = '" + currency + "', " +
                          "customer_country = '" + countryname + "', " +
                          "customer_region = '" + item.region_name + "', " +
                          "customer_state = '" + item.customer_state + "', " +
                          "gst_number = '" + item.gst_number + "', " +
                          "customer_pin = '" + postal_code + "', " +
                          "customer_type = '" + item.customer_type + "', " +
                          "status = 'Active', " +
                          "taxsegment_gid = '" + taxsegment_name + "', " +
                          "created_by = '" + user_gid + "', " +
                          "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                          "WHERE customer_gid = '" + lscustomer_gid + "';";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = "UPDATE crm_mst_tcustomercontact SET " +
                                 "customercontact_name = '" + displayName + "', " +
                                 "customerbranch_name = '" + item.customerbranch_name + "', " +
                                 "email = '" + email + "', " +
                                 "mobile = '" + mobile + "', " +
                                 "main_contact = 'Y', " +
                                 "designation = '" + item.designation + "', " +
                                 "address1 = '" + address1 + "', " +
                                 "address2 = '" + address2 + "', " +
                                 "state = '" + item.customer_state + "', " +
                                 "city = '" + customer_city + "', " +
                                 "country_gid = '" + countryname + "', " +
                                 "region = '" + item.region_name + "', " +
                                 "fax = '" + item.fax + "', " +
                                 "zip_code = '" + postal_code + "', " +
                                 "fax_area_code = '" + item.fax_area_code + "', " +
                                 "fax_country_code = '" + item.fax_country_code + "', " +
                                 "gst_number = '" + item.gst_number + "', " +
                                 "created_by = '" + user_gid + "', " +
                                 "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                                 "WHERE customer_gid = '" + lscustomer_gid + "';";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "  update crm_trn_tleadbankcontact set leadbankcontact_name ='" + displayName + "',mobile ='" + mobile + "'," +
                                   "email ='" + email + "',address1 ='" + address1 + "',address2 ='" + address2 + "',city='" + customer_city + "'," +
                                   "pincode='" + postal_code + "',country_gid='" + countryname + "',region_name='" + item.region_name + "' where leadbank_gid = '" + leadbank_gid + "'and main_contact ='Y'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "Lead added to customer";
                                    values.addtocustomer[0].customer_gid = msGetGid;
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error while adding lead to customer!!";
                                }

                            }
                        }
                    }
                }
                 

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while coverting lead to customer in 360!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGmailupload(HttpRequest httpRequest, string user_gid, results objResult)
        {
            string leadbank_gid = string.Empty;
            string finalEmailBody = string.Empty;
            string msdocument_gid = string.Empty;
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
                    refreshtokenlist objMdlGmailCampaignResponse = new refreshtokenlist();
                    objMdlGmailCampaignResponse = JsonConvert.DeserializeObject<refreshtokenlist>(errornetsuiteJSON);


                    HttpFileCollection httpFileCollection;
                    HttpPostedFile httpPostedFile;
                    string basecode = httpRequest.Form[0];
                    List<sendmail_list> hrDocuments = JsonConvert.DeserializeObject<List<sendmail_list>>(basecode);

                    string gmail_address = httpRequest.Form[1];
                    string sub = httpRequest.Form[2];
                    string to = httpRequest.Form[3];
                    string[] to_mails = to.Split(',');
                    string cc = httpRequest.Form[6];
                    string[] cc_mails = cc.Split(',');
                    string bcc = httpRequest.Form[7];
                    string[] bcc_mails = bcc.Split(',');
                    //string[] toAddresses = httpRequest.Form[3].Split(',');
                    //string tomailaddress_list = "[" + string.Join(",", toAddresses.Select(email => "\"" + email + "\"")) + "]";

                    string bodies = httpRequest.Form[4];
                    if (string.IsNullOrEmpty(httpRequest.Form[5]))
                    {
                        msSQL = "select leadbank_gid from crm_trn_tleadbankcontact where email='" + to + "' and main_contact ='Y' limit 1 ";
                        leadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                    }
                    else
                    {
                        leadbank_gid = httpRequest.Form[5];
                    }

                    List<DbAttachmentPath> dbattachmentpath = new List<DbAttachmentPath>();
                    List<MailAttachmentbase64> mailAttachmentbase64 = new List<MailAttachmentbase64>();
                    string lsfilepath = string.Empty;
                    string document_gid = string.Empty;
                    string lspath, lspath1;
                    string lscompany_code = string.Empty;
                    string FileExtension = string.Empty;
                    string file_name = string.Empty;
                    string httpsUrl = string.Empty;
                    msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                    if (httpRequest.Files.Count > 0)
                    {
                        string lsfirstdocument_filepath = string.Empty;
                        httpFileCollection = httpRequest.Files;
                        for (int i = 0; i < httpFileCollection.Count; i++)
                        {
                            MemoryStream ms = new MemoryStream();
                            var getdocumentdtl = hrDocuments.Where(a => a.AutoID_Key == httpFileCollection.AllKeys[i]).FirstOrDefault();
                            msdocument_gid = objcmnfunctions.GetMasterGID("GILC");
                            httpPostedFile = httpFileCollection[i];
                            file_name = httpPostedFile.FileName;
                            string type = httpPostedFile.ContentType;
                            string lsfile_gid = msdocument_gid;
                            string lscompany_document_flag = string.Empty;
                            FileExtension = Path.GetExtension(file_name).ToLower();
                            lsfile_gid = lsfile_gid + FileExtension;
                            Stream ls_readStream;
                            ls_readStream = httpPostedFile.InputStream;
                            ls_readStream.CopyTo(ms);
                            string base64String = string.Empty;
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
                            mailAttachmentbase64.Add(new MailAttachmentbase64
                            {
                                name = httpPostedFile.FileName,
                                type = type,
                                data = base64String

                            });


                        }


                        StringBuilder emailBody = new StringBuilder();
                        string boundary = "--boundary_example";
                        foreach (var attachment in mailAttachmentbase64)
                        {
                            emailBody.AppendLine("Content-Type: " + attachment.type + "; charset=UTF-8");
                            emailBody.AppendLine("Content-Transfer-Encoding: base64");
                            emailBody.AppendLine("Content-Disposition: attachment; filename=\"" + attachment.name + "\"");
                            emailBody.AppendLine();
                            emailBody.AppendLine(attachment.data);
                            emailBody.AppendLine(boundary);
                        }
                        finalEmailBody = emailBody.ToString();
                        string to_emailString = String.Join(", ", to_mails);
                        string cc_emailString = String.Join(", ", cc_mails);
                        string bcc_emailString = String.Join(", ", bcc_mails);
                        var options1 = new RestClient("https://www.googleapis.com");
                        var request1 = new RestRequest("/upload/gmail/v1/users/me/messages/send?uploadType=media", Method.POST);
                        request1.AddHeader("Authorization", "Bearer  " + objMdlGmailCampaignResponse.access_token + "");
                        request1.AddHeader("Content-Type", "message/rfc822");
                        request1.AddHeader("Cookie", "COMPASS=gmail-api-uploads-blobstore=CgAQ18HPrwYagAEACWuJVwxF5peESk5gbz5AE37T8tg_Yoh-YDKTZilInpK22DHDg7FuuU3LXoN11GAOnyqdLslKu6I5ePsGXCEsd3xSS2yUEWvqsZJtNX4R-ajkWB37okK3XRlg3MQM0P22BdEB5efrYEFEwlQWnrxQUPmucfMFffcwQAMnVi0yfzAB; COMPASS=gmail-api-uploads-blobstore=CgAQnrbWrwYagAEACWuJVwxF5peESk5gbz5AE37T8tg_Yoh-YDKTZilInpK22DHDg7FuuU3LXoN11GAOnyqdLslKu6I5ePsGXCEsd3xSS2yUEWvqsZJtNX4R-ajkWB37okK3XRlg3MQM0P22BdEB5efrYEFEwlQWnrxQUPmucfMFffcwQAMnVi0yfzAB; COMPASS=gmail-api-uploads-blobstore=CgAQ3M3WrwYagAEACWuJV70L2ArobrIhJ3QHHVMUMuhVzIt7hY_BOzuIcJ9f8aTM0lWNTsBGq8iRVZqbbVDXK1zOu9pSPMnm5hcrkX1dIku9gne04K4azeD3LO9TlrMLOaKbRMBzaLZZEsjzHG9ogDw5OoF3IB-_eL6aX22cxCxfAiuXIJU9MPFqsDAB");
                        var body1 = @"From:" + getgmailcredentials.gmail_address + "" + "\n" +
                        @"To:" + to_emailString + "" + "\n" +
                        @"Cc:" + cc_emailString + "" + "\n" +
                        @"Bcc:" + bcc_emailString + "" + "\n" +
                        @"Subject: " + sub + "" + "\n" +
                        @"MIME-Version: 1.0" + "\n" +
                        @"Content-Type: multipart/mixed; boundary=""boundary_example""" + "\n" +
                        @"" + "\n" +
                        @"--boundary_example" + "\n" +
                        @"Content-Type: text/html; charset=""UTF-8""" + "\n" +
                        @"MIME-Version: 1.0" + "\n" +
                        @"" + "\n" +
                        @"<html>" + "\n" +
                        @"  <body>" + "\n" +
                        @"    <p>" + bodies + "</p>" + "\n" +
                       // @"    <p>" + getgmailcredentials.default_template + "</p>" + "\n" +
                        @"  </body>" + "\n" +
                        @"</html>" + "\n" +
                        @"" + "\n" +
                        @"--boundary_example" + "\n" +
                        finalEmailBody +
                        "\n";
                        request1.AddParameter("message/rfc822", body1, ParameterType.RequestBody);
                        IRestResponse response1 = options1.Execute(request1);
                        string errornetsuiteJSON1 = response1.Content;
                        responselist objMdlGmailCampaignResponse1 = new responselist();
                        objMdlGmailCampaignResponse1 = JsonConvert.DeserializeObject<responselist>(errornetsuiteJSON);
                        if (response1.StatusCode == HttpStatusCode.OK)
                        {
                            msSQL = "INSERT INTO crm_trn_gmail (" +
                                  "gmail_gid, " +
                                "from_mailaddress, " +
                                "to_mailaddress, " +
                                "mail_subject, " +
                                "mail_body, " +
                                "transmission_id, " +
                                "leadbank_gid, " +
                                 " created_by, " +
                                "created_date) " +
                                "VALUES (" +
                                 "'" + msdocument_gid + "', " +
                                "'" + getgmailcredentials.gmail_address + "', " +
                                "'" + to + "', " +
                                "'" + sub.Replace("'", "\\\'") + "', " +
                                "'" + bodies.Replace("'", "\\\'") + "', " +
                                "'" + objMdlGmailCampaignResponse1.id + "', " +
                                "'" + leadbank_gid + "', " +
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
                                objResult.message = "Mail Sent Successfully !!";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Sending Mail !!";
                            }
                        }

                    }
                }
                else
                {
                    objResult.message = "User Don't Map Any Gmail API Account !";
                    objResult.status = false;
                }
            }
            catch (Exception ex)
            {
                objResult.status = false;
                objResult.message = "Error While Sending Mail !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }



        }

        public gmailconfiguration gmailcrendentials(string integrated_gmail)
        {
            gmailconfiguration getgmailcredentials = new gmailconfiguration();
            try
            {
                msSQL = "select *from  crm_smm_gmail_service where gmail_address ='" + integrated_gmail + "' limit 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    getgmailcredentials.default_template = objOdbcDataReader["default_template"].ToString();
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            return getgmailcredentials;
        }

        public void DaGmailtext(responselist values, string user_gid)
        {

            string msSQL1 = "select integrated_gmail from hrm_mst_temployee where user_gid ='" + user_gid + "'";
            string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);
            gmailconfiguration getgmailcredentials = gmailcrendentials(integrated_gmail);
            try
            {
                if (integrated_gmail != null && integrated_gmail != "")
                {
                    if (values.leadbank_gid == null || values.leadbank_gid == "")
                    {
                        msSQL = "select leadbank_gid from crm_trn_tleadbankcontact where email='" + values.gmail_to_mail + "' and main_contact ='Y' limit 1 ";
                        values.leadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                    }

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
                    refreshtokenlist objMdlGmailCampaignResponse = new refreshtokenlist();
                    objMdlGmailCampaignResponse = JsonConvert.DeserializeObject<refreshtokenlist>(errornetsuiteJSON);
                    if(response.StatusCode == HttpStatusCode.OK)
                    {

                        var options1 = new RestClient("https://gmail.googleapis.com");
                        var request1 = new RestRequest("/gmail/v1/users/me/messages/send", Method.POST);
                        request1.AddHeader("Authorization", "Bearer  " + objMdlGmailCampaignResponse.access_token + "");
                        request1.AddHeader("Content-Type", "application/json");
                        var bodies = @"{" + "\n" +
                        @"    ""raw"":""" + values.base64EncodedText + "\"" + "\n" +
                        @"    " + "\n" +
                        @"}";
                        request1.AddParameter("application/json", bodies, ParameterType.RequestBody);
                        IRestResponse response1 = options1.Execute(request1);
                        string errornetsuiteJSON1 = response1.Content;
                        responselist objMdlGmailCampaignResponse1 = new responselist();
                        objMdlGmailCampaignResponse1 = JsonConvert.DeserializeObject<responselist>(errornetsuiteJSON);

                        if (response1.StatusCode == HttpStatusCode.OK)
                        {

                            msGetGid = objcmnfunctions.GetMasterGID("GILC");
                            msSQL = "INSERT INTO crm_trn_gmail (" +
                                        "gmail_gid, " +
                                        "from_mailaddress, " +
                                        "to_mailaddress, " +
                                        "mail_subject, " +
                                        "mail_body, " +
                                        "transmission_id, " +
                                        "leadbank_gid, " +
                                         " created_by, " +
                                        "created_date) " +
                                        "VALUES (" +
                                        "'" + msGetGid + "', " +
                                        "'" + getgmailcredentials.gmail_address + "', " +
                                        "'" + values.tomailaddress_list + "', " +
                                        "'" + values.gmail_sub.Replace("'", "\\\'") + "', " +
                                        "'" + values.gmail_body.Replace("'", "\\\'") + "', " +
                                        "'" + objMdlGmailCampaignResponse1.id + "', " +
                                        "'" + values.leadbank_gid + "', " +
                                          "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Mail Sent Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Sending Mail !!";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error while sending mail!!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        values.message = "Error while sending mail contact admin !";
                        values.status = false;
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }
                }
                else
                {
                    values.message = "User Don't Map Any Gmail API Account !";
                    values.status = false;
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" +"***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Sending Mails !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }


            //}



        }
        public void DaPotentialValue(responselist values, string user_gid)
        {

            try
            {
                msSQL = "update crm_trn_tappointment set potential_value = '" + values.potential_value.ToString().Replace(",", "") + "'" +
                        "where appointment_gid = '" + values.appointment_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Potential Value Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while updating Potential Value!!";
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Sending Mails !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }

        public void DaGetLeadStage(MdlLeadbank360 values, string leadstage_name, string leadbank_gid)
        {

            try
            {
                msSQL = "select leadstage_gid,leadstage_name from crm_mst_tleadstage where leadstage_gid != '2' and leadstage_gid != '7'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadstage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadstage_list
                        {
                            leadstage_name = dt["leadstage_name"].ToString(),
                            leadstage_gid = dt["leadstage_gid"].ToString(),

                        });
                        values.leadstage_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Callresponse Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTeleLeadStage(MdlLeadbank360 values, string leadstage_name, string leadbank_gid)
        {

            try
            {
                msSQL = "select leadstage_gid,leadstage_name from crm_mst_tleadstage where leadstage_gid != '4' and leadstage_gid != '8' and leadstage_gid != '6'and leadstage_gid != '3'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadstage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadstage_list
                        {
                            leadstage_name = dt["leadstage_name"].ToString(),
                            leadstage_gid = dt["leadstage_gid"].ToString(),

                        });
                        values.leadstage_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Callresponse Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostLeadStage(string user_gid, new_list values)
        {
            try
            {

                msSQL = "select leadstage_gid from crm_mst_tleadstage where leadstage_name = '" + values.call_response + "'";
                string lsstage = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update crm_trn_tappointment set leadstage_gid='" + lsstage + "' where appointment_gid ='" + values.appointment_gid + "' and  leadstage_gid is not null ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = " Lead Stage Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = " Error Occurs ";

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Lead Stage!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostteleLeadStage(string user_gid, new_list values)
        {
            try
            {

                msSQL = "select leadstage_gid from crm_mst_tleadstage where leadstage_name = '" + values.call_response + "'";
                string lsstage = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select leadstage_name from crm_mst_tleadstage where leadstage_name = '" + values.call_response + "'";
                string lsstagename = objdbconn.GetExecuteScalar(msSQL);
                if (lsstagename == values.call_response)
                {
                    msSQL = " update crm_trn_tlead2campaign set leadstage_gid='" + lsstage + "' where leadbank_gid='" + values.leadbank_gid + "' and  leadstage_gid is not null ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update crm_trn_ttelelead2campaign set leadstage_gid='" + lsstage + "' where leadbank_gid='" + values.leadbank_gid + "' and  leadstage_gid is not null ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = " update crm_trn_ttelelead2campaign set leadstage_gid='" + lsstage + "' where leadbank_gid='" + values.leadbank_gid + "' and  leadstage_gid is not null ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }


                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = " Lead Stage Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = " Error Occurs ";

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Lead Stage!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetcalllogreport(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                msSQL = "select mobile from crm_trn_tleadbankcontact where leadbank_gid  = '" + leadbank_gid + "' and main_contact ='Y'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null && objOdbcDataReader.HasRows)
                {
                    string lsmobile = objOdbcDataReader["mobile"].ToString();
                    if (!string.IsNullOrEmpty(lsmobile))
                    {
                        lsmobile = Regex.Replace(lsmobile, "[^0-9]", "");
                        if (lsmobile.Length > 10)
                        {
                            lsmobile = lsmobile.Substring(lsmobile.Length - 10);
                        }
                    }

                    msSQL = "Select a.station,a.status,a.direction,a.duration,a.start_time,a.phone_number,a.uniqueid,a.call_status,b.agent_name as agent from crm_smm_tclicktocall a" +
                       " left join crm_smm_tclicktocallagents b on b.agent_mailid=a.agent  where phone_number= '" + lsmobile + "'  order by a.start_time desc ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getmodulelist = new List<call_logreport>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            DateTime startTime = (DateTime)dt["start_time"];
                            string startDateString = startTime.ToString("yyyy-MM-dd");
                            string callStatus = dt["call_status"].ToString();
                            string status = callStatus.Equals("CALLER", StringComparison.OrdinalIgnoreCase) ? "ANSWERED" :
                            callStatus.Equals("AGENT", StringComparison.OrdinalIgnoreCase) ? "ANSWERED" : callStatus;

                            getmodulelist.Add(new call_logreport
                            {
                                station = dt["station"].ToString(),
                                uniqueid = dt["uniqueid"].ToString(),
                                phone_number = dt["phone_number"].ToString(),
                                status = dt["status"].ToString(),
                                direction = dt["direction"].ToString(),
                                duration = dt["duration"].ToString(),
                                start_time = startDateString,
                                call_status = status,
                                agent = dt["agent"].ToString(),
                            });
                        }
                        values.call_logreport = getmodulelist;
                    }

                    if (dt_datatable != null)
                    {
                        dt_datatable.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Document Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public whatsappconfiguration whatsappcredentials()
        {
            whatsappconfiguration getwhatsappcredentials = new whatsappconfiguration();
            try
            {


                msSQL = " select workspace_id,channel_id,access_token,channelgroup_id from crm_smm_whatsapp_service";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {

                    getwhatsappcredentials.workspace_id = objOdbcDataReader["workspace_id"].ToString();
                    getwhatsappcredentials.channel_id = objOdbcDataReader["channel_id"].ToString();
                    getwhatsappcredentials.access_token = objOdbcDataReader["access_token"].ToString();
                    getwhatsappcredentials.channelgroup_id = objOdbcDataReader["channelgroup_id"].ToString();


                }
                else
                {

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return getwhatsappcredentials;
        }
        public void DaGetdeletedocuments(string document_gid, MdlLeadbank360 values)
        {
            try
            {

                msSQL = "  delete from crm_trn_tdocument where document_gid ='" + document_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "File Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting File";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting product details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Upload Notes details
        public void DaNotesadd(notes values, string user_gid, result objResult)
        {
            try
            {
                msSQL = "insert into crm_trn_tleadnotes " +
                    "(internal_notes," +
                    "leadbank_gid," +
                    "created_date," +
                    "created_by) " +
                    "values(" +
                       "'" + values.internalnotestext_area.Replace("'", "\\\'") + "', " +
                       "'" + values.leadgig + "', " +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                       "'" + user_gid + "') ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Notes Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Notes!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Uploading Telecaller Lead Notes!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaNoteupdate(notes values, string user_gid, result objResult)
        {
            try
            {
                if (values.source == "TELELEAD")
                {
                    msSQL = "Update crm_trn_ttelelead2campaign set internal_notes='" + values.internal_notes.Replace("'", "\\\'") + "', updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', updated_by='" + user_gid + "' where leadbank_gid='" + values.leadbank_gid + "'; ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else if (values.source == "LEAD")
                {
                    msSQL = "Update crm_trn_tappointment set internal_notes='" + values.internal_notes.Replace("'", "\\\'") + "', updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', updated_by='" + user_gid + "' where leadbank_gid='" + values.leadbank_gid + "'; ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = "Update crm_trn_tleadnotes set internal_notes='" + values.internal_notes.Replace("'", "\\\'") + "', updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', updated_by='" + user_gid + "' where s_no='" + values.s_no + "'; ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Note Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Note!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Uploading Telecaller Lead Notes!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaNotedelete(notes values, string user_gid, result objResult)
        {
            try
            {
                if (values.source == "TELELEAD")
                {
                    msSQL = "Update crm_trn_ttelelead2campaign set internal_notes=null, updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', updated_by='" + user_gid + "' where leadbank_gid='" + values.leadbank_gid + "'; ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else if (values.source == "LEAD")
                {
                    msSQL = "Update crm_trn_tappointment set internal_notes=null, updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', updated_by='" + user_gid + "' where leadbank_gid='" + values.leadbank_gid + "'; ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = "Delete from crm_trn_tleadnotes  where s_no='" + values.s_no + "'; ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Note Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Note!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Telecaller Lead Notes!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Notes summary
        public void DaGetTeleLeadNotesSummary(MdlLeadbank360 values, string leadbank_gid)
        {
            try
            {


                msSQL = "SELECT leadbank_gid, internal_notes, 'TELELEAD' AS source, '0' AS s_no, created_date " +
                    "FROM (SELECT leadbank_gid, internal_notes, created_date FROM crm_trn_ttelelead2campaign " +
                    "WHERE leadbank_gid = '" + leadbank_gid + "' and internal_notes!='' ORDER BY created_date DESC) AS ordered_lead UNION ALL" +
                    " SELECT leadbank_gid, internal_notes, 'NOTES' AS source, s_no, created_date FROM" +
                    " (SELECT leadbank_gid, internal_notes, s_no, created_date FROM crm_trn_tleadnotes " +
                    "WHERE leadbank_gid = '" + leadbank_gid + "' and internal_notes!='' ORDER BY created_date DESC) AS ordered_notes ORDER BY created_date DESC;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<notes>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new notes
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            source = dt["source"].ToString(),
                            s_no = dt["s_no"].ToString(),
                        });
                        values.notes = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Notes Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLeadAppointmentLog(MdlLeadbank360 values, string leadbank_gid)
        {
            try
            {


                msSQL = " select a.lead_title,a.appointment_date,d.business_vertical, concat(c.user_firstname,' ',c.user_lastname,'/',c.user_code)as assign_to " +
                        " from crm_trn_tappointment a " +
                        " left join hrm_mst_temployee b on a.assign_to=b.employee_gid " +
                        " left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                        " left join crm_mst_tbusinessvertical d on a.business_vertical=d.businessvertical_gid " +
                        " where a.Leadstage_gid!='0' and leadbank_gid='" + leadbank_gid + "' order by appointment_date ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<myappointmentlog_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new myappointmentlog_list
                        {
                            lead_title = dt["lead_title"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                        });
                        values.myappointmentlog_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Notes Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdatepricesegement(pricesegement_list values, string user_gid)
        {
            try
            {
                string lscustomer_name, msGetGID;

                msSQL = "select customer_gid from smr_trn_tpricesegment2customer where customer_gid ='" + values.customer_gid + "'";
                lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);
                if(lscustomer_gid == null || lscustomer_gid == "")
                {
                    msSQL = "select pricesegment_name from smr_trn_tpricesegment where pricesegment_gid ='" + values.pricesegment_gid + "'";
                    string pricesegement_name = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select customer_name from crm_mst_tcustomer where customer_gid ='" + values.customer_gid + "'";
                    lscustomer_name = objdbconn.GetExecuteScalar(msSQL);

                    msGetGID = objcmnfunctions.GetMasterGID("VPDC");
                    msSQL = " insert into smr_trn_tpricesegment2customer(" +
                                   " pricesegment2customer_gid, " +
                                   " pricesegment_gid, " +
                                   " pricesegment_name," +
                                   " customer_gid, " +
                                   " customer_name, " +
                                   " created_by, " +
                                   " created_date" +
                                   " )values " +
                                   "('" + msGetGID + "', " +
                        "'" + values.pricesegment_gid + "'," +
                        "'" + pricesegement_name + "'," +
                        "'" + values.customer_gid + "', " +
                        "'" + lscustomer_name + "', " +
                        "'" + user_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msSQL = " update crm_mst_tcustomer set pricesegment_gid='" + values.pricesegment_gid + "'  where  customer_gid='" + values.customer_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Price Segment Updated Sucessfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Price Segment to Customer ";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Price Segment ";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }

                }
                else
                {
                    msSQL = " update crm_mst_tcustomer set pricesegment_gid='" + values.pricesegment_gid + "'  where  customer_gid='" + values.customer_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update smr_trn_tpricesegment2customer set pricesegment_gid='" + values.pricesegment_gid + "'  where  customer_gid='" + values.customer_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Price Segment Updated Sucessfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Price Segment to Customer ";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }

                }


            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Exception occured while Updating Price Segment!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetpricesegement(pricesegement_list values, string customer_gid)
        {
            try
            {


                msSQL = "select pricesegment_gid from smr_trn_tpricesegment2customer where  customer_gid='" + customer_gid + "'";
                string pricesegment_gid = objdbconn.GetExecuteScalar(msSQL);

                values.pricesegment_gid = pricesegment_gid;

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Notes Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


    }
}