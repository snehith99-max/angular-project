using ems.subscription.Models;
using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Stripe.Forwarding;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using MailMessage = System.Net.Mail.MailMessage;


namespace ems.subscription.DataAccess
{
    public class DaHelpandSupport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader, objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, importcount, ls_port;
        public string[] lsCcReceipients;
        string lsCode, msGetGid,ls_server,ls_username,ls_password, lscc_mail;

        public void DaPostHelpandSupport(SupportLists values, string user_gid)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("TICK");

                msSQL = " insert into adm_mst_thelpandsupport(" +
                        " ticket_id," +
                        " module_name," +
                        " screen_name," +
                        " company_name," +
                        " contact_number," +
                        " mail_id," +
                        " description,created_by,created_date)" +
                        " values(" +
                        "'" + msGetGid + "'," +
                        "'" + values.module_name + "'," +
                        "'" + values.Module_name + "'," +
                        "'" + values.company_name + "'," +
                        "'" + values.contact_number + "'," +
                        "'" + values.mail_id + "'," +
                        "'" + values.Description + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {                 
                    string lscompany_mail = null;
                    string lscompany_code = null;
                    string lscompanyid = null;
                    string lsusercode = null;
                    string lsuser_gid = null;
                    string luser_name = null;
                   
                    // string Body = "<p>Dear Team,</p>" +
                    //"<p>Thank you for reaching out to Vcidex Support. We have received your request regarding the issue you encountered on our ERP System.</p>" +
                    //"<p>Your ticket ID is <b>" +msGetGid + "</b>. Our support team will review your issue and reach out to you as soon as possible. Please provide any additional details that might help us resolve the matter more quickly,if any</p>" +
                    //"<p>If you have any urgent questions, feel free to reply to this email.</p>" +
                    //"<p>We appreciate your patience and understanding.</p>" +
                    // "'" + values.Description + "'," +                   
                    //"<p>Best regards,<br> Vcidex Support Team</p>";


                    string body = "Dear Team, <br/>";
                    body += "<br />";
                    body += "Greetings, <br/>";
                    body += "<br />";
                    body += "Thank you for reaching out to Vcidex Support. We have received your request regarding the issue you encountered on our ERP System.<br/>";
                    body += "<br />";
                    body += "<b>Ticket ID:</b> " + msGetGid + "<br/>";
                    body += "<br />";
                    body += "<b>Description: </b>" + values.Description + "<br/>";                   
                    body += "<br />";                  
                    body += "Our support team will review your issue and reach out to you as soon as possible. Please provide any additional details that might help us resolve the matter more quickly,if any<br/>";
                    body += "<br />";
                    body += "If you have any urgent questions, feel free to reply to this email.<br/>";
                    body += "<br />";
                    body += "We appreciate your patience and understanding.<br/>";
                    body += "<br />";
                    body += "Best Regards,<br/>";
                    body += "Support Team";
                    // Send email
                    bool status = mail(values.mail_id, "Support Ticket", body);                   
                    values.status = true;
                    values.message = "Ticket raised to your mail";
                }
                else
                {
                    values.status = false;
                    values.message = "Failed to send the email. Please try again!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }      
        public bool mail( string to, string sub, string body)
        {
            try
            {
                msSQL = "SELECT company_mail,pop_server,pop_port,pop_username,pop_password FROM adm_mst_tcompany ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    ls_server = objOdbcDataReader["pop_server"].ToString();
                    ls_port = Convert.ToInt32(objOdbcDataReader["pop_port"]);
                    ls_username = objOdbcDataReader["pop_username"].ToString();
                    ls_password = objOdbcDataReader["pop_password"].ToString();
                }
                objOdbcDataReader.Close();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(ls_username);
                message.To.Add(new MailAddress(to));
                lscc_mail = ConfigurationManager.AppSettings["supportcc"].ToString();
                
                if (lscc_mail != null & lscc_mail != string.Empty & lscc_mail != "")
                {
                    lsCcReceipients = lscc_mail.Split(',');
                    if (lscc_mail.Length == 0)
                    {
                        message.CC.Add(new MailAddress(lscc_mail));
                    }
                    else
                    {
                        foreach (string CCEmail in lsCcReceipients)
                        {
                            message.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                        }
                    }
                }
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

        public void DaGetHelpandSupportSummary(MdlHelpandSupport values)
        {
            try
            {
                msSQL = "select module_name,screen_name,company_name,contact_number,description,mail_id from adm_mst_thelpandsupport";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<SupportLists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new SupportLists
                        {
                            module_name = dt["module_name"].ToString(),
                            screen_name = dt["screen_name"].ToString(),
                            company_name = dt["company_name"].ToString(),
                            contact_number = dt["contact_number"].ToString(),
                            description = dt["description"].ToString(),
                            mail_id = dt["mail_id"].ToString(),

                        });
                        values.SupportLists = getModuleList;
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
        //public void DaGetEditSupportSummary(MdlHelpandSupport values)
        //{
        //    try
        //    {

        //        msSQL = "select module_name,screen_name,company_name,contact_number,description,mail_id from adm_mst_thelpandsupport ";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getModuleList = new List<SupportLists>();

        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new SupportLists
        //                {
        //                    module_name = dt["module_name"].ToString(),
        //                    screen_name = dt["screen_name"].ToString(),
        //                    company_name = dt["company_name"].ToString(),
        //                    contact_number = dt["contact_number"].ToString(),
        //                    description = dt["description"].ToString(),
        //                    mail_id = dt["mail_id"].ToString(),
        //                });
        //                values.SupportLists = getModuleList;
        //            }
        //        }
        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        values.status = false;

        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
        //         "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }
        //}
    }
}