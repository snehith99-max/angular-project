﻿using ems.crm.Models;
using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Collections.Specialized.BitVector32;


namespace ems.crm.DataAccess
{
    public class DaMailManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader  objOdbcDataReader ;

        DataTable dt_datatable;
        int mnResult, ls_port;
        string msUserGid, msEmployeeGID, msBiometricGID, msGetemployeetype, msTemporaryAddressGetGID, msPermanentAddressGetGID, usercode, lsuser_gid, lsemployee_gid, lsuser_code, lscountry_gid2, lscountry_gid, ls_server, ls_username, final_path, ls_password;

        public void DaGetMailSummary(MdlMailmanagement values)
        {
            msSQL = " SELECT a.mailmanagement_gid,a.from_mail,a.to_mail,a.sub,a.body, CONCAT(b.user_firstname,' ',b.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date  from crm_smm_mailmanagement a " +
                    " left join adm_mst_tuser b on b.user_gid=a.created_by order by a.created_date desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<mail_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new mail_list
                    {
                        mailmanagement_gid = dt["mailmanagement_gid"].ToString(),
                        from = dt["from_mail"].ToString(),
                        to = dt["to_mail"].ToString(),
                        sub = dt["sub"].ToString(),
                        body = dt["body"].ToString(),
                        created_by = dt["created_by"].ToString(),
                        created_date = dt["created_date"].ToString(),
                       

                    });
                    values.mail_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }


        public void DaGetfrommaildropdown(MdlMailmanagement values)
        {
            msSQL = " SELECT company_gid,pop_server, pop_port, pop_username, pop_password  FROM adm_mst_tcompany";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<from_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new from_list
                    {
                        mail = dt["pop_username"].ToString(),
                        company_gid = dt["company_gid"].ToString(),




                    });
                    values.from_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }


        public void Damailmanagementupload(HttpRequest httpRequest, result objResult, string user_gid)
        {

            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;

            string lspath;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);


            MemoryStream ms = new MemoryStream();
            lspath = ConfigurationManager.AppSettings["mailuploadfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "MailManagement/Upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
            string mail_from = httpRequest.Form[0];
            string sub = httpRequest.Form[1];
            string to = httpRequest.Form[2];
            string body = httpRequest.Form[3];

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
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
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
                        lspath = ConfigurationManager.AppSettings["mailuploadfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "MailManagement/Upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        //string local_path = "E:/Angular15/AngularUI/src";
                        ms.Close();
                        lspath = "/assets/media/images/erpdocument" + "/" + lscompany_code + "/" + "MailManagement/Upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension; ;
                        string final_path = lspath + msdocument_gid + FileExtension;

                    }


                    {
                        msSQL = " SELECT pop_server, pop_port, pop_username, pop_password  FROM adm_mst_tcompany";

                        objOdbcDataReader  = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader .HasRows == true)
                        {

                            ls_server = objOdbcDataReader ["pop_server"].ToString();

                            ls_port = Convert.ToInt32(objOdbcDataReader ["pop_port"]);

                            ls_username = objOdbcDataReader ["pop_username"].ToString();

                            ls_password = objOdbcDataReader ["pop_password"].ToString();

                        }
                         



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

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        smtp.Credentials = new NetworkCredential(ls_username, ls_password);

                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        try
                        {

                            smtp.Send(message);

                            //mail_send_result = true;

                            //result = "Mail Send Successfully";
                            msSQL = "INSERT INTO crm_smm_mailmanagement (" +
                          "from_mail, " +
                          "to_mail, " +
                          "sub, " +
                          "body, " +
                          "image_path, "+
                           " created_by, " +
                          "created_date) " +                        
                          "VALUES (" +
                          "'" + ls_username + "', " +
                          "'" + to + "', " +
                          "'"+ sub + "', " +
                          "'" + body + "', " +
                          "'" + lspath + "'," +
                            "'" + user_gid + "'," +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult > 0)
                            {
                                // Email sent and details inserted successfully
                                //return true;
                            }
                            else
                            {
                                // Failed to insert email details
                                //return false;
                            }


                        }
                        catch (Exception ex)

                        {

                            //mail_send_result = false;

                            //result = ex.ToString();

                        }

                    }

                }
            }

            catch (Exception ex)
            {
                objResult.message = ex.ToString();
            }
            //return true;

        }

        public void Damailmanagementsend(HttpRequest httpRequest, from_list values, string user_gid)

        {

          
            try
            {
                // Retrieve SMTP server and credentials from the database
                string msSQL = "SELECT pop_server, pop_port, pop_username, pop_password FROM adm_mst_tcompany";
                objOdbcDataReader  = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader .HasRows)
                {
                      // You need to read the data row before accessing columns.
                    string ls_server = objOdbcDataReader ["pop_server"].ToString();
                    int ls_port = Convert.ToInt32(objOdbcDataReader ["pop_port"]);
                    string ls_username = objOdbcDataReader ["pop_username"].ToString();
                    string ls_password = objOdbcDataReader ["pop_password"].ToString();
                     

                    // Create a new MailMessage
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();

                    // Set sender and recipient email addresses
                    message.From = new MailAddress(ls_username);
                    message.To.Add(new MailAddress(values.to));

                    // Set email subject, body, and HTML format
                    message.Subject = values.sub;
                    message.IsBodyHtml = true;
                    message.Body = values.body;

                    // Configure SMTP client
                    smtp.Port = ls_port;
                    smtp.Host = ls_server;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    smtp.Credentials = new NetworkCredential(ls_username, ls_password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    try
                    {
                        // Send the email
                        smtp.Send(message);

                        // Insert email details into the database
                        msSQL = "INSERT INTO crm_smm_mailmanagement (" +
                            "from_mail, " +
                            "to_mail, " +
                            "sub, " +
                            "body, " +
                             " created_by, " +
                            "created_date) " +                           
                            "VALUES (" +
                            "'" + ls_username + "', " +
                            "'" + values.to + "', " +
                            "'" + values.sub + "', " +
                            "'" + values.body + "', " +
                              "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult > 0)
                        {
                            // Email sent and details inserted successfully
                            //return true;
                        }
                        else
                        {
                            // Failed to insert email details
                            //return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Error occurred while sending the email
                        // Handle the exception or log it
                        //return false;
                    }
                }
                else
                {
                    // No SMTP server configuration found in the database
                    //return false;
                }
            }
            catch (Exception ex)
            {
                // Error occurred while retrieving SMTP server details from the database
                // Handle the exception or log it
                //return false;
            }
        }
      
    }
    }
