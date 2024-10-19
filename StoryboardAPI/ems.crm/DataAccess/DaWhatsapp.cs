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
using System.IO;
using static OfficeOpenXml.ExcelErrorValue;
using System.Web.Http.Results;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Threading;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Runtime.InteropServices;
using OfficeOpenXml;
using OfficeOpenXml.Style;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Blob;
using System.Data.OleDb;
using System.Globalization;
using static ems.crm.Models.MdlWhatsapp;




namespace ems.crm.DataAccess
{
    public class DaWhatsapp
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        HttpPostedFile httpPostedFile;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string lscustomer_gid, msGetGid, msGetGid1, msGETinvGID, msGetGid2, lssource_gid, final_path, lscompany_code;
        int mnResult, mnResult3, mnResult1, mnResult12, importcount;
        //------------------------------start for excel------------------------------//
        string leadbank_name, upload_gid, mobile, customer_type, file_name, msWTAPLGGid;
        string msdocument_gid;
        //------------------------------end for excel------------------------------//
        public result dacreatecontact(mdlCreateContactInput values, string user_gid)
        {
            result objresult = new result();
            try
            {
                int i = 0;
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                Rootobject objRootobject = new Rootobject();
                string contactjson = "{\"displayName\":\"" + values.CompanyName + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + values.phone.e164Number + "\"}],\"firstName\":\"" + values.firstName + "\",\"gender\":\"" + values.gender + "\",\"lastName\":\"" + values.lastName + "\"}";
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
                    msSQL = "insert into crm_smm_whatsapp(id,wkey,wvalue,displayName,firstName,lastName,gender,created_date,created_by)values(" +
                            "'" + objRootobject.id + "'," +
                            "'" + values.key + "'," +
                            "'" + values.phone.e164Number + "'," +
                            "'" + values.CompanyName + "'," +
                            "'" + values.firstName + "'," +
                            "'" + values.lastName + "'," +
                            "'" + values.gender + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {

                        msSQL = "select source_gid from crm_mst_tsource where source_name = 'Whatsapp'";
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
                                    "'Whatsapp'," +
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
                                " '" + values.CompanyName + "'," +
                                " 'y'," +
                                " 'Approved'," +
                                " 'Not Assigned'," +
                                " 'H.Q'," +
                                " '" + values.customer_type + "'," +
                                " '" + values.customer_type + "'," +
                                " '" + lsemployee_gid + "'," +
                                " 'Y'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                        if (msGetGid2 == "E")
                        {
                            objresult.status = false;
                            objresult.message = "Create sequence code BLCC for Lead Bank";
                        }
                        msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                            " (leadbankcontact_gid," +
                            " leadbank_gid," +
                            " leadbankcontact_name," +
                            " mobile," +
                            " created_date," +
                            " created_by," +
                            " leadbankbranch_name, " +
                            " main_contact)" +
                            " values( " +
                            " '" + msGetGid2 + "'," +
                            " '" + msGetGid1 + "'," +
                            " '" + values.ContactpersonName + "'," +
                            " '" + values.phone.e164Number + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            " '" + lsemployee_gid + "'," +
                            " 'H.Q'," +
                            " 'y'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msSQL = "update crm_smm_whatsapp set leadbank_gid='" + msGetGid1 + "' where id='" + objRootobject.id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update crm_trn_tleadbank set wh_id='" + objRootobject.id + "' where leadbank_gid='" + msGetGid1 + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }

                    if (mnResult == 1)
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
                objresult.message = "Error occured while posting contact!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

            return objresult;
        }


        public void Daupdatewhatsappcontact(mdlUpdateContactInput values, string user_gid)
        {
            try
            {
                msSQL = "update  crm_smm_whatsapp set " +
                        " displayName = '" + values.displayName_edit + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                        "' where wvalue ='" + values.phone_edit.e164Number + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Contact Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Contact !!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Contact!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }



        public result daUpdateContact(mdlUpdateContactInput values, string user_gid)
        {
            result objresult = new result();
            try
            {
                msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customertype_edit + "'";
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
                        " 'BSEM231215135'," +
                        " '" + msGetGid + "'," +
                        " '" + values.displayName_edit + "'," +
                        " 'y'," +
                        " 'Approved'," +
                        " 'Not Assigned'," +
                        " 'H.Q'," +
                        " '" + lscustomer_type + "'," +
                        " '" + values.customertype_edit + "'," +
                        " '" + lsemployee_gid + "'," +
                        " 'Y'," +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                if (msGetGid2 == "E")
                {
                    objresult.status = false;
                    objresult.message = "Create sequence code BLCC for Lead Bank";
                }
                msSQL = " select id from crm_smm_whatsapp Where wvalue='" + values.phone_edit.e164Number + "'";
                string lscontact_id = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                    " (leadbankcontact_gid," +
                    " leadbank_gid," +
                    " leadbankcontact_name," +
                    " mobile," +
                    " created_date," +
                    " created_by," +
                    " leadbankbranch_name, " +
                    " main_contact)" +
                    " values( " +
                    " '" + msGetGid2 + "'," +
                    " '" + msGetGid1 + "'," +
                    " '" + values.displayName_edit + "'," +
                    " '" + values.phone_edit.e164Number + "'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    " '" + lsemployee_gid + "'," +
                    " 'H.Q'," +
                    " 'y'" + ")";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "update crm_smm_whatsapp set leadbank_gid='" + msGetGid1 + "' where id = '" + lscontact_id + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "update crm_trn_tleadbank set wh_id='" + lscontact_id + "' where leadbank_gid='" + msGetGid1 + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    objresult.status = true;
                    objresult.message = "Contact added to lead successfully!";
                }
                else
                {
                    objresult.message = "Error occured while adding contact!";
                }


            }
            catch (Exception ex)
            {
                objresult.message = "Error occured while posting contact!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

            return objresult;
        }

        public result daCreateProject(mdlCreateTemplateInput values, string user_gid)
        {
            result objresult = new result();
            try
            {

                int i = 0;
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                createProject objcreateProject = new createProject();
                string contactjson = "{\"type\":\"channelTemplate\",\"name\":\"" + values.name + "\",\"description\":\"" + values.description + "\"}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects", Method.POST);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseoutput = response.Content;
                objcreateProject = JsonConvert.DeserializeObject<createProject>(responseoutput);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    msSQL = "insert into crm_smm_whatsapptemplate(project_id,p_type,p_name,description,created_date)values(" +
                            "'" + objcreateProject.id + "'," +
                            "'" + objcreateProject.type + "'," +
                            "'" + objcreateProject.name + "'," +
                            "'" + values.description + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        objresult.status = true;
                        objresult.message = "Project created successfully!";
                    }
                    else
                    {
                        objresult.message = "Error occured while adding Project!";
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error occured while posting Project!!";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Error occured while posting Project!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return objresult;
        }
        public result daPostTemplateCreation(HttpRequest httpRequest)
        {
            result objresult = new result();

            try
            {

                HttpFileCollection httpFileCollection;
                HttpPostedFile httpPostedFile;
                string file_type = httpRequest.Form[0];
                string body = httpRequest.Form[1];
                string template_name = httpRequest.Form[2];
                string description = httpRequest.Form[3];
                string p_name = httpRequest.Form[4];
                string footer = httpRequest.Form[5];
                string contactjson5 = "";

                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();

                createProject objcreateProject = new createProject();
                string contactjson = "{\"type\":\"channelTemplate\",\"name\":\"" + p_name + "\",\"description\":\"" + description + "\"}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects", Method.POST);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseoutput = response.Content;
                objcreateProject = JsonConvert.DeserializeObject<createProject>(responseoutput);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    msSQL = "insert into crm_smm_whatsapptemplate(project_id,p_type,p_name,description,created_date)values(" +
                            "'" + objcreateProject.id + "'," +
                            "'" + objcreateProject.type + "'," +
                            "'" + objcreateProject.name + "'," +
                            "'" + description + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = " select company_code from adm_mst_tcompany";
                        lscompany_code = objdbconn.GetExecuteScalar(msSQL);


                        if (httpRequest.Files.Count > 0)
                        {
                            string lsfirstdocument_filepath = string.Empty;
                            httpFileCollection = httpRequest.Files;
                            for (int i = 0; i < httpFileCollection.Count; i++)
                            {
                                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                                httpPostedFile = httpFileCollection[i];
                                string file_name = httpPostedFile.FileName;
                                string lsfile_gid = msdocument_gid;
                                string lscompany_document_flag = string.Empty;
                                string FileExtension = Path.GetExtension(file_name).ToLower();
                                lsfile_gid += FileExtension;
                                Stream ls_readStream = httpPostedFile.InputStream;
                                MemoryStream ms = new MemoryStream();
                                ls_readStream.CopyTo(ms);
                                string mime_type = MimeMapping.GetMimeMapping(httpPostedFile.FileName);


                                bool status1;
                                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Whatsapp/WhatsappTemplate" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                                ms.Close();

                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Whatsapp/WhatsappTemplate" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";


                                msSQL = "insert into crm_trn_tfiles(" +
                                        "file_gid," +
                                        "document_name," +
                                        "document_path)values(" +
                                        "'" + msdocument_gid + "'," +
                                        "'" + file_name.Replace("'", "\\'") + "'," +
                                        "'" + ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                                   '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                                   '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"] + "')";
                                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult1 == 1)
                                {
                                    string mediaurl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                                                 '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                                                 '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];



                                    TemplateCreation objcreatetemplate = new TemplateCreation();
                                    if (file_type == "image")

                                        contactjson5 = "{\"defaultLocale\":\"en\",\"genericContent\":[],\"platformContent\":[{\"platform\":\"whatsapp\",\"locale\":\"en\",\"blocks\":[{\"type\":\"image\",\"role\":\"header\",\"image\":{\"mediaUrl\":\"" + mediaurl + "\"}," +
                                                             "\"id\":\"7vY1pgyRZ7ETflSK46d5qL\"},{\"type\":\"text\",\"role\":\"body\",\"text\":{\"text\":\"" + body + "\"},\"id\":\"cbA0XdmkFDAbJiGfS41kK9\"},{\"type\":\"text\",\"role\":\"footer\"," +
                                                             "\"text\":{\"text\":\"" + footer + "\"},\"id\":\"lA0CtTqaNO2zFz9p_Uoxo7\"}],\"type\":\"text\",\"channelGroupIds\":[\"" + getwhatsappcredentials.channelgroup_id + "\"]}],\"supportedPlatforms\":[\"whatsapp\"]," +
                                                             "\"deployments\":[{\"key\":\"whatsappTemplateName\",\"platform\":\"whatsapp\",\"value\":\"" + template_name + "\"},{\"key\":\"whatsappCategory\",\"platform\":\"whatsapp\",\"value\":\"MARKETING\"}," +
                                                             "{\"key\":\"whatsappAllowCategoryChange\",\"platform\":\"whatsapp\",\"value\":\"false\"}]}";
                                    else
                                        contactjson5 = "{\"defaultLocale\":\"en\",\"genericContent\":[],\"platformContent\":[{\"platform\":\"whatsapp\",\"locale\":\"en\",\"blocks\":[{\"type\":\"file\",\"role\":\"header\",\"file\":{\"mediaUrl\":\"" + mediaurl + "\",\"contentType\":\"" + mime_type + "\"}," +
                "\"id\":\"9B02RxTQq3UnLmY8EJgDKo\"},{\"type\":\"text\",\"role\":\"body\",\"text\":{\"text\":\"" + body + "\"},\"id\":\"cbA0XdmkFDAbJiGfS41kK9\"},{\"type\":\"text\",\"role\":\"footer\"," +
                "\"text\":{\"text\":\"" + footer + "\"},\"id\":\"lA0CtTqaNO2zFz9p_Uoxo7\"}],\"type\":\"text\",\"channelGroupIds\":[\"" + getwhatsappcredentials.channelgroup_id + "\"]}],\"supportedPlatforms\":[\"whatsapp\"]," +
                "\"deployments\":[{\"key\":\"whatsappTemplateName\",\"platform\":\"whatsapp\",\"value\":\"" + template_name + "\"},{\"key\":\"whatsappCategory\",\"platform\":\"whatsapp\",\"value\":\"MARKETING\"}," +
                "{\"key\":\"whatsappAllowCategoryChange\",\"platform\":\"whatsapp\",\"value\":\"false\"}]}";


                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                    var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects/" + objcreateProject.id + "/channel-templates", Method.POST);
                                    request1.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                                    request1.AddParameter("application/json", contactjson5, ParameterType.RequestBody);
                                    IRestResponse response1 = client1.Execute(request1);
                                    var responseoutput1 = response1.Content;
                                    objcreatetemplate = JsonConvert.DeserializeObject<TemplateCreation>(responseoutput1);
                                    if (response1.StatusCode == HttpStatusCode.Created)
                                    {
                                        string commentmessage = System.Net.WebUtility.HtmlEncode(body);

                                        Console.WriteLine("Original Emoji: " + body);
                                        Console.WriteLine("HTML Entity Code: " + commentmessage);

                                        msSQL = " update  crm_smm_whatsapptemplate  set " +
                                                " template_id = '" + objcreatetemplate.id + "'," +
                                                " template_body = '" + commentmessage + "'," +
                                                " footer = '" + footer + "'," +
                                                " file_type = '" + file_type + "'," +
                                                " media_url = '" + mediaurl + "' " +
                                                " where project_id='" + objcreateProject.id + "'  ";


                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                            var client3 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                            var request3 = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects/" + objcreateProject.id + "/channel-templates/" + objcreatetemplate.id + "/activate", Method.PUT);
                                            request3.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                                            IRestResponse response3 = client.Execute(request3);
                                            if (response3.StatusCode == HttpStatusCode.OK)
                                            {
                                                objresult.status = true;
                                                objresult.message = "Campaign Created successfully!";
                                            }
                                            else
                                            {
                                                objresult.message = "Error occured while Creating Template!";
                                            }
                                        }
                                        else
                                        {
                                            objresult.message = "Error occured while Creating Campaign!";
                                        }
                                    }
                                    else
                                    {
                                        objresult.status = false;
                                        objresult.message = "Error occured while Creating Campaign!!";
                                    }

                                }
                                else
                                {
                                    objresult.message = "Error occured while uploading document!";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                }
                            }
                        }
                    }
                    else
                    {
                        objresult.message = "Error occured while Creating Campaign!";
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error occured while Creating Campaign!!";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Exception occured while Creating Campaign!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return objresult;
        }
        public result daPostTextTemplateCreation(template_creation values)
        {
            result objresult = new result();

            try
            {

                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();

                createProject objcreateProject = new createProject();
                string contactjson = "{\"type\":\"channelTemplate\",\"name\":\"" + values.p_name + "\",\"description\":\"" + values.description + "\"}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects", Method.POST);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseoutput = response.Content;
                objcreateProject = JsonConvert.DeserializeObject<createProject>(responseoutput);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    msSQL = "insert into crm_smm_whatsapptemplate(project_id,p_type,p_name,description,created_date)values(" +
                            "'" + objcreateProject.id + "'," +
                            "'" + objcreateProject.type + "'," +
                            "'" + objcreateProject.name + "'," +
                            "'" + values.description + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {

                        TemplateCreation objcreatetemplate = new TemplateCreation();

                        string contactjson1 = "{\"defaultLocale\":\"en\",\"genericContent\":[],\"platformContent\":[{\"platform\":\"whatsapp\"," +
                                       "\"locale\":\"en\",\"blocks\":[{\"type\":\"text\",\"role\":\"body\",\"text\":{\"text\":\"" + values.body + "\"}" +
                                       ",\"id\":\"cbA0XdmkFDAbJiGfS41kK9\"},{\"type\":\"text\",\"role\":\"footer\"," +
                                       " \"text\":{\"text\":\"" + values.footer + "\"},\"id\":\"lA0CtTqaNO2zFz9p_Uoxo7\"}],\"type\":\"text\",\"channelGroupIds\"" +
                                       ":[\"" + getwhatsappcredentials.channelgroup_id + "\"]}],\"supportedPlatforms\":[\"whatsapp\"],\"deployments\":[{\"key\":\"whatsappTemplateName\",\"platform\":\"whatsapp\",\"value\":\"" + values.template_name + "\"},{\"key\":\"whatsappCategory" +
                                       "\",\"platform\":\"whatsapp\",\"value\":\"MARKETING\"},{\"key\":\"whatsappAllowCategoryChange\",\"platform\":\"whatsapp\",\"value\":\"false\"}]}";

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                        var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects/" + objcreateProject.id + "/channel-templates", Method.POST);
                        request1.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                        request1.AddParameter("application/json", contactjson1, ParameterType.RequestBody);
                        IRestResponse response1 = client1.Execute(request1);
                        var responseoutput1 = response1.Content;
                        objcreatetemplate = JsonConvert.DeserializeObject<TemplateCreation>(responseoutput1);
                        if (response1.StatusCode == HttpStatusCode.Created)
                        {
                            string commentmessage = System.Net.WebUtility.HtmlEncode(values.body);

                            Console.WriteLine("Original Emoji: " + values.body);
                            Console.WriteLine("HTML Entity Code: " + commentmessage);

                            msSQL = " update  crm_smm_whatsapptemplate  set " +
                                    " template_id = '" + objcreatetemplate.id + "'," +
                                    " template_body = '" + commentmessage + "'," +
                                    " footer = '" + values.footer + "'" +
                                    " where project_id='" + objcreateProject.id + "'  ";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client3 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                var request3 = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects/" + objcreateProject.id + "/channel-templates/" + objcreatetemplate.id + "/activate", Method.PUT);
                                request3.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                                IRestResponse response3 = client.Execute(request3);
                                if (response3.StatusCode == HttpStatusCode.OK)
                                {
                                    objresult.status = true;
                                    objresult.message = "Campaign Created successfully!";
                                }
                                else
                                {
                                    objresult.message = "Error occured while Creating Template!";
                                }
                            }
                            else
                            {
                                objresult.message = "Error occured while Creating Campaign!";
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Error occured while Creating Campaign!!";
                        }


                    }
                    else
                    {
                        objresult.message = "Error occured while Creating Campaign!";
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error occured while Creating Campaign!!";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Exception occured while Creating Campaign!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return objresult;
        }
        public result DaWhatsappSend(sendmessage values, string user_gid)
        {
            result objresult = new result();
            try
            {

                int i = 0;
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();

                Result objsendmessage = new Result();
                if (values.project_id != null)
                {
                    string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + values.project_id + "\",\"version\":\"" + values.version + "\",\"locale\":\"en\"}}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string waresponse = response.Content;
                    objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        msSQL = " select media_url from crm_smm_whatsapptemplate where project_id ='" + values.project_id + "'";
                        string lsmedia_url = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select template_body from crm_smm_whatsapptemplate where project_id ='" + values.project_id + "'";
                        string lstemplate_body = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select footer from crm_smm_whatsapptemplate where project_id ='" + values.project_id + "'";
                        string lsfooter = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "insert into crm_trn_twhatsappmessages(" +
                                 "message_id," +
                                 "contact_id," +
                                 "direction," +
                                 "type," +
                                 "template_image," +
                                 "template_body," +
                                 "template_footer," +
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
                                 "'Template'," +
                                "'" + lsmedia_url + "'," +
                                "'" + lstemplate_body + "'," +
                                 "'" + lsfooter + "',";
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
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            objresult.status = true;
                            objresult.message = "Sent Successfully!!!";
                        }
                        else
                        {
                            objresult.message = "Failed to Send!!!";
                        }
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Failed to Send!!!";
                    }
                }
                else
                {
                    Servicewindow objsendmessage1 = new Servicewindow();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/contacts/" + values.contact_id, Method.GET);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                    IRestResponse response1 = client.Execute(request);
                    string waresponse1 = response1.Content;
                    objsendmessage1 = JsonConvert.DeserializeObject<Servicewindow>(waresponse1);
                    if (objsendmessage1.serviceWindowExpireAt != null)
                    {
                        string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"" + values.sendtext + "\"}}}";

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                        var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                        request1.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                        request1.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                        IRestResponse response2 = client1.Execute(request1);
                        string waresponse = response2.Content;
                        objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                        if (response2.StatusCode == HttpStatusCode.Accepted)
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
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult3 == 1)
                            {
                                objresult.status = true;
                                objresult.message = "Sent Successfully!";
                            }
                            else
                            {
                                objresult.message = "Failed to Send!";
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Service Window closed";
                        }
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Service Window closed";
                    }
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Service Window closed";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }


            return objresult;
        }


        public result DaGetTemplate()
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            result objresult = new result();
            try
            {

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://nest.messagebird.com");
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects", Method.GET);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                IRestResponse response = client.Execute(request);
                IRestResponse responseAddress = client.Execute(request);
                string errornetsuiteJSON = responseAddress.Content;
                Templatelist objMdlWhatsappTemplate = new Templatelist();
                objMdlWhatsappTemplate = JsonConvert.DeserializeObject<Templatelist>(errornetsuiteJSON);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var item in objMdlWhatsappTemplate.results)
                    {

                        if (item.activeResourceId != null)
                        {
                            msSQL = " update  crm_smm_whatsapptemplate  set " +
                                    " version_id = '" + item.activeResourceId + "' where project_id='" + item.id + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "template receive failed";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "template receive failed";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return objresult;
        }

        public void DaGetContact(MdlWhatsapp values)
        {
            try
            {



                msSQL = "SELECT c.source_name,d.region_name,b.customer_type,b.lead_status,SUBSTRING(displayName, 1, 1) AS first_letter,displayName,Wvalue,id,customer_from," +
                               " (SELECT COUNT(read_flag) FROM crm_trn_twhatsappmessages WHERE contact_id = id  AND direction = 'incoming' AND read_flag = 'N') AS count," +
                               " (SELECT CASE WHEN DATE(created_date) = CURDATE() THEN TIME_FORMAT(created_date, '%h:%i %p') " +
                               " WHEN DATE(created_date) = CURDATE() - INTERVAL 1 DAY THEN 'Yesterday'  ELSE DATE_FORMAT(created_date, '%d/%m/%y') END AS formatted_date" +
                               " FROM crm_trn_twhatsappmessages WHERE contact_id = id ORDER BY created_date DESC LIMIT 1) AS last_seen " +
                               " FROM crm_smm_whatsapp " +
                               " LEFT JOIN crm_trn_tleadbank b ON b.wh_id =id " +
                              " LEFT JOIN crm_mst_tsource c ON c.source_gid = b.source_gid" +
                              " LEFT JOIN crm_mst_tregion d ON d.region_gid = b.leadbank_region where Wvalue is not null " +
                               " ORDER BY CASE WHEN last_seen REGEXP '^[0-9]{2}:[0-9]{2} [APMapm]{2}$' THEN 1 WHEN last_seen = 'Yesterday' THEN 2 ELSE 3 END, last_seen DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatscontactlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatscontactlist
                        {
                            whatsapp_gid = dt["id"].ToString(),
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),
                            first_letter = dt["first_letter"].ToString(),
                            read_flag = dt["count"].ToString(),
                            last_seen = dt["last_seen"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            customer_from = dt["customer_from"].ToString(),
                            region_name = dt["region_name"].ToString(),





                        });
                        values.whatscontactlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contacts";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetMessage(MdlWhatsapp values, string whatsapp_gid)
        {
            try
            {


                msSQL = "update crm_trn_twhatsappmessages set read_flag = 'Y' where contact_id ='" + whatsapp_gid + "' and direction='incoming'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select customertype_gid from crm_trn_tleadbank where wh_id ='" + whatsapp_gid + "'";
                string lscustomertype_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT a.template_image,a.template_body,a.template_footer,a.message_id,a.project_id,d.template_body,d.footer,d.media_url,b.leadbank_gid,a.created_date,a.direction,a.contact_id,b.lastName,b.firstName, a.message_text,a.type,a.status," +
                        " CONCAT(DATE_FORMAT(a.created_date, '%e %b %y, '), DATE_FORMAT(a.created_date, '%h:%i %p')) AS time," +
                        " b.wvalue AS identifierValue,SUBSTRING(b.displayName, 1, 1) AS first_letter, b.displayName,c.document_name,c.document_path" +
                        " FROM crm_trn_twhatsappmessages a LEFT JOIN crm_smm_whatsapp b ON b.id = a.contact_id" +
                        " left join crm_trn_tfiles c on a.message_id = c.message_gid " +
                        " left join crm_smm_whatsapptemplate d on d.project_id = a.project_id" +
                        " WHERE  contact_id = '" + whatsapp_gid + "'ORDER BY a.created_date DESC, time DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsmessagelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsmessagelist
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
                            customertype_gid = lscustomertype_gid,

                        });
                        values.whatsmessagelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Messages";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetMessageTemplatesummary(MdlWhatsapp values)
        {
            try
            {
                msSQL = " select project_id,template_id,template_body,p_type,p_name,footer," +
                        " created_date,version_id from crm_smm_whatsapptemplate" +
                        " where campaign_flag='Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsappMessagetemplatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsappMessagetemplatelist
                        {
                            id = dt["project_id"].ToString(),
                            template_id = dt["template_id"].ToString(),
                            template_body = dt["template_body"].ToString(),
                            footer = dt["footer"].ToString(),
                            p_type = dt["p_type"].ToString(),
                            p_name = dt["p_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            version = dt["version_id"].ToString(),
                        });
                        values.whatsappMessagetemplatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Template Summary";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetMessageTemplateview(MdlWhatsapp values, string project_id)
        {
            try
            {


                msSQL = " select project_id,template_id,template_body,p_type,p_name,footer,media_url,created_date,version_id from crm_smm_whatsapptemplate" +
                      " WHERE  project_id = '" + project_id + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsappMessagetemplatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsappMessagetemplatelist
                        {
                            id = dt["project_id"].ToString(),
                            template_id = dt["template_id"].ToString(),
                            template_body = dt["template_body"].ToString(),
                            footer = dt["footer"].ToString(),
                            p_type = dt["p_type"].ToString(),
                            media_url = dt["media_url"].ToString(),
                            p_name = dt["p_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            version = dt["version_id"].ToString(),
                        });
                        values.whatsappMessagetemplatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Template View";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetContactCount(MdlWhatsapp values)
        {
            try
            {


                msSQL = "select count(*) as count  from crm_smm_whatsapp";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<contactcount_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new contactcount_list1
                        {
                            contact_count1 = dt["count"].ToString(),

                        });
                        values.contactcount_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contact Count";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetcampaign(MdlWhatsapp values)
        {
            try
            {
                msSQL = "select  COUNT(*) from crm_smm_whatsapptemplate";
                string lsproject_id;
                lsproject_id = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT COUNT(status) AS delivered_campaign FROM crm_trn_twhatsappmessages where status = 'accepted'";
                string lsdeliveredmessages;
                lsdeliveredmessages = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select count(project_id) from crm_trn_twhatsappmessages where month(created_date) = month(current_date())";
                string lsmtd;
                lsmtd = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select count(project_id) from crm_trn_twhatsappmessages where year(created_date) = year(current_date())";
                string lsytd;
                lsytd = objdbconn.GetExecuteScalar(msSQL);


                //msSQL = "SELECT distinct(a.project_id),a.campaign_flag,a.description, a.version_id, a.p_type, a.p_name, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date," +
                //    " (SELECT COUNT(b.status) FROM crm_trn_twhatsappmessages b WHERE a.project_id = b.project_id) AS send_campaign FROM crm_smm_whatsapptemplate a " +
                //    "LEFT JOIN crm_trn_twhatsappmessages b ON a.project_id = b.project_id order by a.created_date desc";

                msSQL = "call crm_trn_spcampaignsummary";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsappCampaign>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsappCampaign
                        {
                            project_id = dt["project_id"].ToString(),
                            version_id = dt["version_id"].ToString(),
                            p_type = dt["p_type"].ToString(),
                            p_name = dt["p_name"].ToString(),
                            description = dt["description"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            send_campaign = dt["send_campaign"].ToString(),
                            campaign_flag = dt["campaign_flag"].ToString(),
                            project_count = lsproject_id,
                            delivered_messages = lsdeliveredmessages,
                            lsmtd = lsmtd,
                            lsytd = lsytd,



                        });
                        values.whatsappCampaign = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Campaign";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        public void DaDeleteCampaign(string project_id, whatsappCampaign values)
        {
            try
            {
                msSQL = "delete from crm_smm_whatsapptemplate where project_id='" + project_id + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Deleted Successfully!!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Error while Deleting Campaign";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetlog(MdlWhatsapp values, string project_id)
        {
            try
            {


                string msSQL = " select b.p_name,a.identifiervalue,concat(i.user_firstname,' ',i.user_lastname) as assign_to,e.source_name,d.lead_status,d.leadbank_name, " +
                               " c.displayName,a.status,a.reason,a.direction, DATE_FORMAT(a.created_date, '%d-%m-%Y %h:%m:%s') AS created_date from crm_trn_twhatsappmessages a " +
                               " LEFT JOIN crm_smm_whatsapptemplate b ON b.project_id = a.project_id " +
                               " left join crm_smm_whatsapp c on a.contact_id = c.id " +
                               " LEFT JOIN crm_trn_tleadbank d ON d.wh_id = c.id " +
                               " LEFT JOIN crm_mst_tsource e ON e.source_gid = d.source_gid " +
                               " LEFT JOIN adm_mst_tuser f ON f.user_gid = d.assign_to " + 
                               " left join crm_trn_tappointment g on d.leadbank_gid = g.leadbank_gid " +
                               " left join hrm_mst_temployee h on  g.assign_to = h.employee_gid " +
                               " left join adm_mst_tuser i on h.user_gid = i.user_gid " +
                               " where a.project_id='" + project_id + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<log>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new log
                        {
                            displayName = dt["displayName"].ToString(),
                            identifiervalue = dt["identifiervalue"].ToString(),
                            status = dt["status"].ToString(),
                            direction = dt["direction"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            p_name = dt["p_name"].ToString(),
                            reason = dt["reason"].ToString(),

                        });
                        values.log = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Log";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        public void DaGetTemplatepreview(MdlWhatsapp values, string project_id)
        {
            try
            {


                string msSQL = "select file_type,p_name,template_body,footer,media_url from crm_smm_whatsapptemplate a " +
                    "where project_id='" + project_id + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettemplatepreviewview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Gettemplatepreviewview_list
                        {
                            p_name = dt["p_name"].ToString(),
                            template_body = dt["template_body"].ToString(),
                            footer = dt["footer"].ToString(),
                            file_type = dt["file_type"].ToString(),
                            media_url = dt["media_url"].ToString(),
                        });
                        values.Gettemplateview_list = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Template Preview";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public result dabulkcustomizeMessageSend(mdlBulkMessageList values)
        {
            result result = new result();
            try
            {
                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    dacustomizeMessageSend(values.sendtext, values.contacts_list);
                }));
                t.Start();

                result.status = true;
                result.message = "Messages sent successfully!";

            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Message";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }
        public void dacustomizeMessageSend(string sendtext, List<mdlBulkMessageContacts> contacts_list)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            foreach (var item in contacts_list)
            {
                Result objsendmessage = new Result();
                try
                {
                    string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + item.value + "\",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"" + sendtext + "\"}}}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string waresponse = response.Content;
                    objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                    if (response.StatusCode == HttpStatusCode.Accepted)
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

                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Failed to Send!" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error Occured while sending message to " + item.value + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error: " + response.Content + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
                catch (Exception ex)
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured while sending message to " + item.value + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }
        }
        public result dabulkMessageSend(mdlBulkMessageList values)
        {
            result result = new result();
            try
            {
                msSQL = "select version_id,p_name from crm_smm_whatsapptemplate where project_id = '" + values.project_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    string version_id = objOdbcDataReader["version_id"].ToString();
                    string project_name = objOdbcDataReader["p_name"].ToString();
                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    {
                        HttpContext.Current = ctx;
                        daSendBulkMessage(values.project_id, version_id, values.contacts_list);
                    }));
                    t.Start();
                }
                result.status = true;
                result.message = "Messages sent successfully!";
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Message";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }
        public void daSendBulkMessage(string project_id, string version_id, List<mdlBulkMessageContacts> contacts_list)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            foreach (var item in contacts_list)
            {
                Result objsendmessage = new Result();
                try
                {
                    string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + item.value + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + project_id + "\",\"version\":\"" + version_id + "\",\"locale\":\"en\"}}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string waresponse = response.Content;
                    objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        msSQL = " select media_url from crm_smm_whatsapptemplate where project_id ='" + project_id + "'";
                        string lsmedia_url = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select template_body from crm_smm_whatsapptemplate where project_id ='" + project_id + "'";
                        string lstemplate_body = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select footer from crm_smm_whatsapptemplate where project_id ='" + project_id + "'";
                        string lsfooter = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "insert into crm_trn_twhatsappmessages(" +
                                "message_id," +
                                "contact_id," +
                                 "identifiervalue," +
                                "direction," +
                                "type," +
                                 "template_image," +
                                 "template_body," +
                                 "template_footer," +
                                "message_text," +
                                "content_type," +
                                "project_id," +
                                "version_id," +
                                "status," +
                                "created_date)" +
                                "values(" +
                                "'" + objsendmessage.id + "'," +
                                "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                "'" + objsendmessage.receiver.contacts[0].identifierValue + "'," +
                                "'" + objsendmessage.direction + "'," +
                                 "'Template'," +
                                  "'" + lsmedia_url + "'," +
                                "'" + lstemplate_body + "'," +
                                 "'" + lsfooter + "',";
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
                        else if (objsendmessage.body.type == "file")
                        {
                            msSQL += "'" + objsendmessage.body.file.files[0].mediaUrl.Replace("'", "\\'") + "'," +
                                     "null,";
                        }

                        msSQL += "'" + objsendmessage.template.projectId + "'," +
                                "'" + objsendmessage.template.version + "'," +
                                "'" + objsendmessage.status + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "update crm_trn_twhatsappmessages set sendcampaign_flag ='Y'  where message_id  ='" + objsendmessage.id + "' and project_id = '" + project_id + "'";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error Occured while sending message to " + item.value + " project ID : " + project_id + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                catch (Exception ex)
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured while sending message to " + item.value + " project ID : " + project_id + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }
        }
        public void DaGetWhatsappMessageCount(MdlWhatsapp values)
        {
            try
            {


                msSQL = "SELECT COUNT(*) AS Total_messages,COUNT(CASE WHEN direction = 'incoming' THEN 1 END) AS Received_messages,COUNT(CASE WHEN direction = 'outgoing' THEN 1 END) AS Sent_messages FROM crm_trn_twhatsappmessages";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsappmessagescount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsappmessagescount
                        {

                            Total_messages = dt["Total_messages"].ToString(),
                            Received_messages = dt["Received_messages"].ToString(),
                            Sent_messages = dt["Sent_messages"].ToString(),
                        });
                        values.whatsappmessagescount = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Message Count";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public notifications daNotifications(string employee_gid)
        {
            int count = 0;
            notifications objNotifications = new notifications();
            try
            {
                msSQL = " select employee_emailid from hrm_mst_temployee where employee_gid= '" + employee_gid + "' ";
                string employee_emailid = objdbconn.GetExecuteScalar(msSQL);

                string msSQL1 = "select integrated_gmail from hrm_mst_temployee where employee_gid ='" + employee_gid + "'";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL1);

                //msSQL = " SELECT b.displayName,a.contact_id,CONCAT(d.user_firstname, ' ', d.user_lastname) AS assigned_to,COUNT(a.read_flag) AS count,b.leadbank_gid, " +
                //        " (SELECT MAX(created_date) FROM crm_trn_twhatsappmessages WHERE contact_id = a.contact_id) AS created_date,'wa' AS ca_type FROM crm_trn_twhatsappmessages a " +
                //        " LEFT JOIN crm_smm_whatsapp b ON b.id = a.contact_id " +
                //        " LEFT JOIN crm_trn_tleadbank c ON c.leadbank_gid = b.leadbank_gid " +
                //        " LEFT JOIN adm_mst_tuser d ON d.user_gid = c.assign_to " +
                //        " WHERE a.direction = 'incoming' AND a.read_flag = 'N' " +
                //        " GROUP BY a.contact_id, b.displayName, d.user_firstname, d.user_lastname, b.leadbank_gid " +
                //        " UNION ALL SELECT to_mail AS displayName,mailmanagement_gid AS contact_id,NULL AS assigned_to,COUNT(read_flag) AS count,leadbank_gid,MAX(created_date) AS created_date, " +
                //        " 'em' AS ca_type FROM crm_smm_mailmanagement WHERE direction = 'incoming' AND read_flag = 'N' GROUP BY mailmanagement_gid, to_mail, leadbank_gid " +
                //        " UNION ALL SELECT from_id AS displayName,inbox_id AS contact_id,NULL AS assigned_to, " +
                //        " COUNT(read_flag) AS count,NULL AS leadbank_gid,sent_date AS  created_date, 'om' AS ca_type FROM crm_trn_toutlookinbox " +
                //        " where read_flag = 'False' and inbox_status is Null AND ( FIND_IN_SET('" + employee_emailid + "', to_id) > 0 OR FIND_IN_SET('" + employee_emailid + "',cc) > 0 " +
                //        " OR FIND_IN_SET('" + employee_emailid + "',bcc) > 0  OR integrated_gmail='" + employee_emailid + "'  )  GROUP BY from_id   " +
                //        " UNION ALL SELECT from_id AS displayName,inbox_id AS contact_id,NULL AS assigned_to,COUNT(read_flag) AS count,NULL AS leadbank_gid,sent_date AS created_date, " +
                //        " 'gm' AS ca_type FROM crm_trn_tgmailinbox WHERE read_flag = 'True' and integrated_gmail = '"+ integrated_gmail + "' GROUP BY from_id ORDER BY created_date DESC ";
                             msSQL = $@" SELECT 
                                        b.displayName,
                                        a.contact_id,
                                        CONCAT(d.user_firstname, ' ', d.user_lastname) AS assigned_to,
                                        COUNT(a.read_flag) AS count,
                                        b.leadbank_gid,
                                        (SELECT MAX(created_date) 
                                         FROM crm_trn_twhatsappmessages 
                                         WHERE contact_id = a.contact_id) AS created_date,
                                        'wa' AS ca_type
                                    FROM 
                                        crm_trn_twhatsappmessages a
                                    LEFT JOIN 
                                        crm_smm_whatsapp b ON b.id = a.contact_id
                                    LEFT JOIN 
                                        crm_trn_tleadbank c ON c.leadbank_gid = b.leadbank_gid
                                    LEFT JOIN 
                                        adm_mst_tuser d ON d.user_gid = c.assign_to
                                    WHERE 
                                        a.direction = 'incoming' 
                                        AND a.read_flag = 'N'
                                    GROUP BY 
                                        a.contact_id, 
                                        b.displayName, 
                                        d.user_firstname, 
                                        d.user_lastname, 
                                        b.leadbank_gid

                                    UNION ALL

                                    SELECT 
                                        to_mail AS displayName,
                                        mailmanagement_gid AS contact_id,
                                        NULL AS assigned_to,
                                        NULL AS count,
                                        leadbank_gid,
                                        MAX(created_date) AS created_date,
                                        'em' AS ca_type
                                    FROM 
                                        crm_smm_mailmanagement
                                    WHERE 
                                        direction = 'incoming' 
                                        AND read_flag = 'N'
                                    GROUP BY 
                                        to_mail, 
                                        mailmanagement_gid, 
                                        leadbank_gid

                                    UNION ALL

                                    SELECT 
                                        from_id AS displayName,
                                        inbox_id AS contact_id,
                                        NULL AS assigned_to,
                                        NULL AS count,
                                        NULL AS leadbank_gid,
                                        sent_date AS created_date,
                                        'om' AS ca_type
                                    FROM 
                                        crm_trn_toutlookinbox
                                    WHERE 
                                        read_flag = 'False' 
                                        AND inbox_status IS NULL
                                        AND (
                                            FIND_IN_SET('{employee_emailid}', to_id) > 0 
                                            OR FIND_IN_SET('{employee_emailid}', cc) > 0
                                            OR FIND_IN_SET('{employee_emailid}', bcc) > 0
                                            OR integrated_gmail = '{employee_emailid}'
                                        )

                                    UNION ALL

                                    SELECT 
                                        from_id AS displayName,
                                        inbox_id AS contact_id,
                                        NULL AS assigned_to,
                                        NULL AS count,
                                        NULL AS leadbank_gid,
                                        sent_date AS created_date,
                                        'gm' AS ca_type
                                    FROM 
                                        crm_trn_tgmailinbox
                                    WHERE 
                                        read_flag = 'True' 
                                        AND integrated_gmail = '{integrated_gmail}'

                                    ORDER BY 
                                        created_date DESC;
                                    ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    var getNotificationList = new List<notification_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getNotificationList.Add(new notification_list
                            {
                                displayName = dt["displayName"].ToString(),
                                contact_id = dt["contact_id"].ToString(),
                                count = dt["count"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                ca_type = dt["ca_type"].ToString(),
                                assigned_to = dt["assigned_to"].ToString()

                            });
                            objNotifications.notification_Lists = getNotificationList;
                            count++;
                        }
                    }
                    dt_datatable.Dispose();
                    objNotifications.notification_count = count;
                }
            }
            catch (Exception ex)
            {

                //objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return objNotifications;
        }
        public void DaGetbreadcrumbmail(string sref, MdlWhatsapp values)
        {
            try
            {
                msSQL = " SELECT l1.module_name AS l1_menu,l1.sref AS l1_sref,l2.module_name AS l2_menu,l2.sref AS l2_sref, " +
                        " l3.module_name AS l3_menu,l3.sref AS l3_sref FROM adm_mst_tmoduleangular l3 " +
                        " LEFT JOIN adm_mst_tmoduleangular l2 ON l3.module_gid_parent = l2.module_gid " +
                        " LEFT JOIN adm_mst_tmoduleangular l1 ON l2.module_gid_parent = l1.module_gid " +
                        " WHERE l3.sref = '"+ sref + "' LIMIT 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<breadcrummaillist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new breadcrummaillist
                        {
                            l1_menu = dt["l1_menu"].ToString(),
                            l2_menu = dt["l2_menu"].ToString(),
                            l3_menu = dt["l3_menu"].ToString(),
                            l1_sref = dt["l1_sref"].ToString(),
                            l2_sref = dt["l2_sref"].ToString(),
                            l3_sref = dt["l3_sref"].ToString(),

                        });
                        values.breadcrummaillist = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public result daSendDocuments(HttpRequest httpRequest)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            result objresult = new result();
            try
            {


                Servicewindow objsendmessage1 = new Servicewindow();
                string contact_id = httpRequest.Form[1];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/contacts/" + contact_id, Method.GET);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                IRestResponse response1 = client.Execute(request);
                string waresponse1 = response1.Content;
                objsendmessage1 = JsonConvert.DeserializeObject<Servicewindow>(waresponse1);
                if (objsendmessage1.serviceWindowExpireAt != null)
                {
                    try
                    {
                        HttpFileCollection httpFileCollection;
                        HttpPostedFile httpPostedFile;
                        string file_type = httpRequest.Form[0];
                        string contactjson = "";
                        msSQL = " select company_code from adm_mst_tcompany";
                        lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                        if (httpRequest.Files.Count > 0)
                        {
                            string lsfirstdocument_filepath = string.Empty;
                            httpFileCollection = httpRequest.Files;
                            for (int i = 0; i < httpFileCollection.Count; i++)
                            {
                                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                                httpPostedFile = httpFileCollection[i];
                                string file_name = httpPostedFile.FileName;
                                string lsfile_gid = msdocument_gid;
                                string lscompany_document_flag = string.Empty;
                                string FileExtension = Path.GetExtension(file_name).ToLower();
                                lsfile_gid += FileExtension;
                                Stream ls_readStream = httpPostedFile.InputStream;
                                MemoryStream ms = new MemoryStream();
                                ls_readStream.CopyTo(ms);
                                string mime_type = MimeMapping.GetMimeMapping(httpPostedFile.FileName);

                                msSQL = "select wvalue from crm_smm_whatsapp where id = '" + contact_id + "'";
                                string phonenumber = objdbconn.GetExecuteScalar(msSQL);

                                bool status1;
                                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Whatsapp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                                ms.Close();

                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Whatsapp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";


                                msSQL = "insert into crm_trn_tfiles(" +
                                        "file_gid," +
                                        "document_name," +
                                        "document_path)values(" +
                                        "'" + msdocument_gid + "'," +
                                        "'" + file_name.Replace("'", "\\'") + "'," +
                                        "'" + ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                           '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                           '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"] + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {

                                    string mediaurl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                     '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                     '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];
                                    if (file_type == "image")
                                        contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + phonenumber + "\",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"image\",\"image\":{\"images\":[{\"mediaUrl\":\"" + mediaurl + "\"}]}}}";
                                    else
                                        contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\" " + phonenumber + " \",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"file\",\"file\":{\"files\":[{\"contentType\":\"" + mime_type + "\",\"mediaUrl\":\"" + mediaurl + "\"}]}}}";
                                    Result objsendmessage = new Result();
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                    var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                                    request1.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                                    request1.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                    IRestResponse response = client1.Execute(request1);
                                    string waresponse = response.Content;
                                    objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                                    if (response.StatusCode == HttpStatusCode.Accepted)
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
                                            msSQL += "'Image'," +
                                                     "null,";
                                        }
                                        else
                                        {
                                            msSQL += "'File'," +
                                                     "'" + objsendmessage.body.file.files[0].contentType.Replace("'", "\\'") + "',";
                                        }

                                        msSQL += "'" + objsendmessage.status + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            msSQL = "update crm_trn_tfiles set " +
                                                    "message_gid = '" + objsendmessage.id + "'," +
                                                    "contact_gid = '" + objsendmessage.receiver.contacts[0].id + "' where file_gid = '" + msdocument_gid + "'";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult == 0)
                                            {
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Update failed: " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                            }
                                            objresult.status = true;
                                            objresult.message = "Delivered!";
                                        }
                                        else
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Insert failed: " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                        }
                                    }
                                    else
                                    {
                                    }
                                }
                                else
                                {
                                    objresult.message = "Error occured while uploading document!";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Insert failed: " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        objresult.message = "Exception occured while sending message!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


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

                objresult.message = "Exception occured while sending message!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return objresult;
        }

        public MdlWaFiles daGetDocuments(string contact_id)
        {
            MdlWaFiles obj = new MdlWaFiles();
            var getimagesList = new List<wa_images_list>();
            var getfilesList = new List<wa_files_list>();
            try
            {
                msSQL = "select a.file_gid ,a.document_name,a.document_path,b.content_type, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date from crm_trn_tfiles a " +
                        "left join crm_trn_twhatsappmessages b on b.message_id = a.message_gid " +
                        "where contact_gid = '" + contact_id + "' and b.type = 'image'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getimagesList.Add(new wa_images_list
                        {
                            created_date = dt["created_date"].ToString(),
                            file_gid = dt["file_gid"].ToString(),
                            file_name = dt["document_name"].ToString(),
                            file_path = dt["document_path"].ToString(),
                        });
                        obj.wa_images_list = getimagesList;
                        obj.status = true;
                    }
                }
                msSQL = "select a.file_gid ,a.document_name,a.document_path,b.content_type, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date from crm_trn_tfiles a " +
                        "left join crm_trn_twhatsappmessages b on b.message_id = a.message_gid " +
                        "where contact_gid = '" + contact_id + "' and b.type = 'file'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getfilesList.Add(new wa_files_list
                        {
                            created_date = dt["created_date"].ToString(),
                            file_gid = dt["file_gid"].ToString(),
                            file_name = dt["document_name"].ToString(),
                            file_path = dt["document_path"].ToString(),
                        });
                        obj.wa_files_list = getfilesList;
                        obj.status = true;
                    }
                }
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return obj;
        }

        public void DaGetCampaignContactsent(string project_id, MdlWhatsapp values)
        {
            try
            {


                msSQL = " select d.Wvalue ,d.id,a.sendcampaign_flag,d.displayName, c.source_name ,b.customer_type ,b.lead_status from crm_trn_twhatsappmessages a" +
                        " LEFT JOIN crm_trn_tleadbank b ON b.wh_id = a.contact_id " +
                        " LEFT JOIN crm_mst_tsource c ON c.source_gid = b.source_gid " +
                        " LEFT JOIN crm_smm_whatsapp  d ON d.id = a.contact_id " +
                        " where a.project_id = '" + project_id + "' and a.sendcampaign_flag = 'Y' group by d.id";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatscontactlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatscontactlist
                        {
                            whatsapp_gid = dt["id"].ToString(),
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            sendcampaign_flag = dt["sendcampaign_flag"].ToString(),


                        });
                        values.whatscontactlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contacts";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetCampaignContactunsent(MdlWhatsapp values)
        {
            try
            {
                msSQL = " select d.Wvalue ,d.id,a.sendcampaign_flag,d.displayName, c.source_name ,b.customer_type ,b.lead_status from crm_trn_twhatsappmessages a" +
                        " LEFT JOIN crm_trn_tleadbank b ON b.wh_id = a.contact_id " +
                        " LEFT JOIN crm_mst_tsource c ON c.source_gid = b.source_gid " +
                        " LEFT JOIN crm_smm_whatsapp  d ON d.id = a.contact_id ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatscontactlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatscontactlist
                        {
                            whatsapp_gid = dt["id"].ToString(),
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            sendcampaign_flag = dt["sendcampaign_flag"].ToString(),


                        });
                        values.whatscontactlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contacts";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

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
        public void DaUpdateWhatsappCampaignStatus(Campaignstatus values)
        {


            try
            {


                msSQL = "select campaign_flag from crm_smm_whatsapptemplate where project_id='" + values.project_id + "'";
                string campaign_flag = objdbconn.GetExecuteScalar(msSQL);
                if (campaign_flag == "Y")
                {
                    msSQL = "update crm_smm_whatsapptemplate set campaign_flag='N' where project_id='" + values.project_id + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = "update crm_smm_whatsapptemplate set campaign_flag='Y' where project_id='" + values.project_id + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Campaign Status Updated Sucessfully!!";
                }
            }
            catch (Exception ex)
            {

                values.message = "Error While Updating Campaign Status";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");


            }
        }

        //Customer Type Dropdown

        public void DaGetCustomerTypeSummary(MdlWhatsapp values)

        {

            msSQL = "select customertype_gid,customer_type from crm_mst_tcustomertype ORDER BY customertype_gid ASC";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getmodulelist = new List<customertype_list2>();

            if (dt_datatable.Rows.Count != 0)

            {

                foreach (DataRow dt in dt_datatable.Rows)

                {

                    getmodulelist.Add(new customertype_list2

                    {

                        customertype_gid2 = dt["customertype_gid"].ToString(),

                        customer_type2 = dt["customer_type"].ToString(),

                    });

                    values.customertype_list2 = getmodulelist;

                }

            }

            dt_datatable.Dispose();

        }
        public void DaGetContacts(MdlWhatsapp values)
        {
            try
            {

                msSQL = " select id ,wvalue ,displayName,b.customer_type,b.lead_status,c.source_name from crm_smm_whatsapp " +
                        " LEFT JOIN crm_trn_tleadbank b ON b.wh_id = id " +
                        " LEFT JOIN crm_mst_tsource c ON c.source_gid = b.source_gid " +
                        " where Servicewindow_flag = 'Y' group by id ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatscontact_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatscontact_list
                        {
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),
                            id = dt["id"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            source_name = dt["source_name"].ToString(),


                        });
                        values.whatscontact_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contacts";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public result DaGetservicewindowcontact(MdlWhatsapp values)
        {
            result result = new result();
            try
            {
                msSQL = " select id,wvalue,displayName from crm_smm_whatsapp ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatscontact_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatscontact_list
                        {
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),
                            id = dt["id"].ToString(),


                        });
                        values.whatscontact_list = getModuleList;
                    }

                    dt_datatable.Dispose();

                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    {
                        HttpContext.Current = ctx;
                        daservicewindow(values.whatscontact_list);
                    }));
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Message";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }
        public void daservicewindow(List<whatscontact_list> whatscontact_list)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            foreach (var item in whatscontact_list)
            {
                try
                {
                    Servicewindow objsendmessage1 = new Servicewindow();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/contacts/" + item.id, Method.GET);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                    IRestResponse response1 = client.Execute(request);
                    string waresponse1 = response1.Content;
                    objsendmessage1 = JsonConvert.DeserializeObject<Servicewindow>(waresponse1);
                    if (objsendmessage1.serviceWindowExpireAt != null)
                    {

                        msSQL = "update crm_smm_whatsapp set Servicewindow_flag='Y' where id='" + item.id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = "update crm_smm_whatsapp set Servicewindow_flag='N' where id='" + item.id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }
                catch (Exception ex)
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured while sending message to " + item.value + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }
        }
        public void DaGetContactist(string region_name, string source_name, string customer_type, MdlWhatsapp values)
        {

            msSQL = "SELECT d.region_name,c.source_name,b.customer_type,b.lead_status,SUBSTRING(displayName, 1, 1) AS first_letter,displayName,Wvalue,id,customer_from," +
                               " (SELECT COUNT(read_flag) FROM crm_trn_twhatsappmessages WHERE contact_id = id  AND direction = 'incoming' AND read_flag = 'N') AS count," +
                               " (SELECT CASE WHEN DATE(created_date) = CURDATE() THEN TIME_FORMAT(created_date, '%h:%i %p') " +
                               " WHEN DATE(created_date) = CURDATE() - INTERVAL 1 DAY THEN 'Yesterday'  ELSE DATE_FORMAT(created_date, '%d/%m/%y') END AS formatted_date" +
                               " FROM crm_trn_twhatsappmessages WHERE contact_id = id ORDER BY created_date DESC LIMIT 1) AS last_seen " +
                               " FROM crm_smm_whatsapp " +
                               " LEFT JOIN crm_trn_tleadbank b ON b.wh_id =id " +
                              " LEFT JOIN crm_mst_tsource c ON c.source_gid = b.source_gid " +
                               " LEFT JOIN crm_mst_tregion d ON d.region_gid = b.leadbank_region ";
            if (region_name != "null" || source_name != "null" || customer_type != "null")
            {
                msSQL += " where 1=1 ";
                if (region_name != "null")
                {
                    msSQL += "and b.leadbank_region = '" + region_name + "' ";
                }
                if (source_name != "null")
                {
                    msSQL += "and b.source_gid = '" + source_name + "' ";
                }
                if (customer_type != "null")
                {
                    msSQL += "and b.customertype_gid = '" + customer_type + "' ";
                }
            }
            msSQL += " ORDER BY CASE WHEN last_seen REGEXP '^[0-9]{2}:[0-9]{2} [APMapm]{2}$' THEN 1 WHEN last_seen = 'Yesterday' THEN 2 ELSE 3 END, last_seen DESC";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<whatscontactlist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new whatscontactlist
                    {
                        whatsapp_gid = dt["id"].ToString(),
                        displayName = dt["displayName"].ToString(),
                        value = dt["Wvalue"].ToString(),
                        first_letter = dt["first_letter"].ToString(),
                        read_flag = dt["count"].ToString(),
                        last_seen = dt["last_seen"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        source_name = dt["source_name"].ToString(),
                        lead_status = dt["lead_status"].ToString(),
                        customer_from = dt["customer_from"].ToString(),
                        region_name = dt["region_name"].ToString(),



                    });
                    values.whatscontactlist = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public result daPostImportExcel(mdlimportlead values, string user_gid)
        {
            result result = new result();
            try
            {

                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    daimportlead(values.contacts_list, user_gid);
                }));
                t.Start();

                result.status = true;
                result.message = "Lead Imported Successfully";

            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Message";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }
        public void daimportlead(List<mdlimportleads> contacts_list, string user_gid)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            foreach (var item in contacts_list)
            {
                Result objsendmessage = new Result();
                try
                {
                    Rootobject objRootobject = new Rootobject();
                    string contactjson = "{\"displayName\":\"" + item.leadbank_name + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + item.mobile + "\"}],\"firstName\":\"" + item.leadbank_name + "\",\"lastName\":\"" + item.leadbank_name + "\"}";
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
                            "'" + item.mobile + "'," +
                            "'" + item.leadbank_name + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "update crm_smm_whatsapp set leadbank_gid='" + item.leadbank_gid + "' where id='" + objRootobject.id + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "update crm_trn_tleadbank set wh_id='" + objRootobject.id + "' where leadbank_gid='" + item.leadbank_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error Occured while sending message to " + item.value + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                catch (Exception ex)
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured while sending message to " + item.value + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }
        }
        public void DaGetLeadContact(MdlWhatsapp values)
        {
            try
            {
                //msSQL = " SELECT a.leadbank_gid, a.leadbank_name, b.source_name, a.customer_type, a.customertype_gid," +
                //        " CASE WHEN c.mobile LIKE '+91%' THEN c.mobile ELSE CONCAT('+91', c.mobile) END AS mobile,d.region_name" +
                //        " FROM crm_trn_tleadbank a" +
                //        " LEFT JOIN crm_mst_tsource b ON b.source_gid = a.source_gid" +
                //        " LEFT JOIN crm_trn_tleadbankcontact c ON c.leadbank_gid = a.leadbank_gid" +
                //        " LEFT JOIN crm_mst_tregion d ON d.region_gid = a.leadbank_region " +
                //        " WHERE a.wh_id IS NULL AND c.mobile IS NOT NULL AND c.mobile != ''";

                msSQL = "call crm_trn_spleadimport";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsleadlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsleadlist
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            region_name = dt["region_name"].ToString(),

                        });
                        values.whatsleadlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contacts";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public result DawhatsappcontactsImport(HttpRequest httpRequest, string user_gid, leadbank_list values)
        {
            result result = new result();
            try
            {

                HttpFileCollection httpFileCollection;
                HttpPostedFile httpPostedFile;
                string project_id = httpRequest.Form[0];

                msSQL = "select version_id,p_name from crm_smm_whatsapptemplate where project_id = '" + project_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    string version_id = objOdbcDataReader["version_id"].ToString();

                    HttpContext ctx = HttpContext.Current;
                    System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                    {
                        HttpContext.Current = ctx;
                        DawhatsappImport(httpRequest, user_gid, values, project_id, version_id);
                    }));
                    t.Start();
                }
                result.status = true;
                result.message = "Campaign Sent Successfully";

            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Message";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }
        public void DawhatsappImport(HttpRequest httpRequest, string user_gid, leadbank_list values, string project_id, string version_id)
        {
            try
            {

                string lscompany_code;
                string excelRange, endRange;
                int rowCount, columnCount;


                try
                {
                    int insertCount = 0;
                    HttpFileCollection httpFileCollection;
                    DataTable dt = null;
                    string lspath, lsfilePath;

                    msSQL = " select company_code from adm_mst_tcompany";
                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                    // Create Directory
                    lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"];

                    if (!Directory.Exists(lsfilePath))
                        Directory.CreateDirectory(lsfilePath);

                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        httpPostedFile = httpFileCollection[i];
                    }
                    string FileExtension = httpPostedFile.FileName;
                    string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                    string lsfile_gid = msdocument_gid;
                    FileExtension = Path.GetExtension(FileExtension).ToLower();
                    lsfile_gid = lsfile_gid + FileExtension;
                    FileInfo fileinfo = new FileInfo(lsfilePath);
                    Stream ls_readStream;
                    ls_readStream = httpPostedFile.InputStream;
                    MemoryStream ms = new MemoryStream();
                    ls_readStream.CopyTo(ms);

                    //path creation        
                    lspath = lsfilePath + "/";
                    FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                    ms.WriteTo(file);
                    bool status1;


                    status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Lead Import/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                    ms.Close();

                    //// Connect to the storage account
                    //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"].ToString());
                    //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    //CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["blob_containername"].ToString());

                    //// Get a reference to the blob
                    //CloudBlockBlob blockBlob = container.GetBlockBlobReference(lscompany_code + "/" + "Lead Import/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension);
                    //string path_url = lscompany_code + "/" + "Lead Import/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension;


                    // Download the blob's contents and read Excel file
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // await blockBlob.DownloadToStreamAsync(memoryStream);

                        // blockBlob.DownloadToStream(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.Position = 0;
                        // Load Excel package from the memory stream
                        using (ExcelPackage package = new ExcelPackage(memoryStream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"]; // worksheet name
                                                                                              // Remove the first row
                            worksheet.DeleteRow(1);

                            // Convert Excel data to array list format
                            List<List<string>> excelData = new List<List<string>>();

                            for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                            {
                                List<string> rowData = new List<string>();
                                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                                {
                                    var cellValue = worksheet.Cells[row, col].Value?.ToString();
                                    rowData.Add(cellValue);
                                }

                                string leadbank_name = rowData[0];
                                string customer_type = rowData[2];
                                string mobile = rowData[1];

                                msSQL = "select leadbank_gid from crm_trn_tleadbank where leadbank_name='" + leadbank_name + "'";
                                string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                                if (lsleadbank_gid == "")
                                {

                                    msSQL = "select employee_gid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                                    string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);
                                    {

                                        //insertion in table
                                        msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                                        msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                                        msSQL = " INSERT INTO crm_trn_tleadbank" +
                                                " (leadbank_gid, " +
                                                " leadbank_name," +
                                                " customer_type," +
                                                " created_by," +
                                                " status," +
                                                " leadbank_id," +
                                                " approval_flag," +
                                                " leadbank_code," +
                                                " main_branch," +
                                                " main_contact," +
                                                " lead_status," +
                                                " created_date)" +
                                                " values( " +
                                                " '" + msGetGid1 + "'," +
                                                " '" + leadbank_name + "'," +
                                                " '" + customer_type + "'," +
                                                " '" + lsemployee_gid + "'," +
                                                " 'Y'," +
                                                " '" + msGetGid + "'," +
                                                " 'Approved'," +
                                                " 'H.Q'," +
                                                " 'Y'," +
                                                " 'Y'," +
                                                " 'Not Assigned'," +
                                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                        mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        if (mnResult12 == 1)
                                        {
                                            msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");

                                            msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                                                    " (leadbankcontact_gid," +
                                                    " leadbankcontact_name," +
                                                    " leadbank_gid," +
                                                    " mobile," +
                                                    " created_by," +
                                                    " created_date)" +
                                                    " values( " +
                                                    " '" + msGetGid2 + "'," +
                                                    " '" + leadbank_name + "'," +
                                                    " '" + msGetGid1 + "'," +
                                                    " '" + mobile + "'," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                            mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        }
                                    }
                                }

                                Result objsendmessage = new Result();

                                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                                Rootobject objRootobject = new Rootobject();
                                string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + mobile + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + project_id + "\",\"version\":\"" + version_id + "\",\"locale\":\"en\"}}";

                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                string waresponse = response.Content;
                                objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

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
                                             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding lead template";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }



        }

        public result DawhatsappcontactsleadsImports(HttpRequest httpRequest, string user_gid, leadbank_list values)
        {
            result result = new result();
            try
            {
                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    whatsappcontactsleadImports(httpRequest, user_gid, values);
                }));
                t.Start();

                result.status = true;
                result.message = "Campaign Sent Successfully";

            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Message";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

            return result;
        }

        public void whatsappcontactsleadImports(HttpRequest httpRequest, string user_gid, leadbank_list values)
        {
            string lscompany_code;
            try
            {
                HttpFileCollection httpFileCollection;
                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"] + lscompany_code + "/" + " Import_Excel/CRM_Module/" + "Whatsappimportlead/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                if (!Directory.Exists(lsfilePath)) {
                    Directory.CreateDirectory(lsfilePath);
                }
                    
                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                string lsfile_gid = msdocument_gid;
                FileExtension = Path.GetExtension(FileExtension).ToLower();
                file_name = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);
                lsfile_gid = lsfile_gid + FileExtension;
                FileInfo fileinfo = new FileInfo(lsfilePath);
                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

                //path creation        
                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                try
                {
                    string status;
                    status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                    file.Close();
                    ms.Close();

                    msSQL = " insert into cmr_log_twhatsappexcelllog (" +
                        " upload_gid ," +
                        " file_name , " +
                        " file_extension , " +
                        " upload_date , " +
                        " upload_by  " +
                        " ) values ( " +
                        " '" + msdocument_gid + "'," +
                        " '" + file_name + "'," +
                        " '"+ FileExtension  + "'," +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " '" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                catch (Exception ex)
                {
                    values.message = ex.ToString();
                    return;
                }

                //Excel To DataTable
                try
                {
                    DataTable dataTable = new DataTable();
                    int totalSheet = 1;
                    string connectionString = string.Empty;
                    string fileExtension = Path.GetExtension(lspath);

                    lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                    string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");

                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                    }
                    catch (Exception ex)
                    {
                        values.message = ex.ToString();
                        return;
                    }

                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        if (schemaTable != null)
                        {
                            var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
                                                 where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                                 select dataRow).CopyToDataTable();

                            schemaTable = tempDataTable;
                            totalSheet = schemaTable.Rows.Count;
                            using (OleDbCommand command = new OleDbCommand())
                            {
                                command.Connection = connection;
                                command.CommandText = "select * from [Sheet1$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                importcount = 0;
                                foreach (DataRow dt_product in dataTable.Rows)
                                {
                                    if (dataTable.Columns.Contains("upload_gid"))
                                    {
                                        upload_gid = dt_product["upload_gid"].ToString();
                                        leadbank_name = dt_product["Name"]?.ToString();
                                        mobile = dt_product["Mobile Number"]?.ToString();
                                        customer_type = dt_product["Customer Type"]?.ToString();
                                    }
                                    else
                                    {
                                        leadbank_name = dt_product["Name"]?.ToString();
                                        mobile = dt_product["Mobile Number"]?.ToString();
                                        customer_type = dt_product["Customer Type"]?.ToString();
                                    }
                                    
                                    if(upload_gid != null)
                                    {
                                        msSQL = " delete from crm_log_twhatsapptrnlog where upload_gid='" + upload_gid + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        msSQL = " delete from cmr_log_twhatsappexcelllog where upload_gid='" + upload_gid + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }

                                    msSQL = "select leadbank_gid from crm_trn_tleadbank where leadbank_name='" + leadbank_name + "'";
                                    string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                                    if (lsleadbank_gid == "")
                                    {

                                        msSQL = "select employee_gid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                                        string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);
                                        {

                                            if (string.IsNullOrEmpty(leadbank_name) || string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(customer_type))
                                            {
                                                msWTAPLGGid = objcmnfunctions.GetMasterGID("WTAPLOG");

                                                msSQL = " insert into crm_log_twhatsapptrnlog ( " +
                                                    " whatsapplog_gid, " +
                                                    " upload_gid, " +
                                                    " leadbank_name ," +
                                                    " mobile ," +
                                                    " customer_type ," +
                                                    " created_date ," +
                                                    " created_by " +
                                                    " ) values ( " +
                                                    "'" + msWTAPLGGid + "'," +
                                                    "'" + msdocument_gid + "'," +
                                                    "'" + leadbank_name + "'," +
                                                    "'" + mobile + "'," +
                                                    "'" + customer_type + "'," +
                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + user_gid + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }
                                            else
                                            {
                                                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                                                Rootobject objRootobject = new Rootobject();

                                                var contactjson = JsonConvert.SerializeObject(new
                                                {
                                                    displayName = leadbank_name,
                                                    identifiers = new[] {
                                                    new 
                                                    { 
                                                         key = "phonenumber", value = mobile }
                                                    },
                                                    firstName = leadbank_name,
                                                    gender = leadbank_name,
                                                    lastName = leadbank_name
                                                });

                                                //string contactjson = "{\"displayName\":\"" + leadbank_name + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + mobile + "\"}],\"firstName\":\"" + leadbank_name + "\",\"gender\":\"" + leadbank_name + "\",\"lastName\":\"" + leadbank_name + "\"}";
                                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/contacts", Method.POST);
                                                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                                                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                                request.AddParameter("Phone_number", mobile);
                                                IRestResponse response = client.Execute(request);
                                                var responseoutput = response.Content;
                                                objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);
                                                if (response.StatusCode == HttpStatusCode.Created)
                                                {
                                                    msSQL = "insert into crm_smm_whatsapp(id,wvalue,displayName,created_date,created_by)values(" +
                                                            "'" + objRootobject.id + "'," +
                                                            "'" + mobile + "'," +
                                                            "'" + leadbank_name + "'," +
                                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                            "'" + user_gid + "')";

                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    if (mnResult == 1)
                                                    {

                                                        msSQL = "select source_gid from crm_mst_tsource where source_name = 'Whatsapp'";
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
                                                               "'Whatsapp'," +
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

                                                        msSQL = " select customertype_gid from crm_mst_tcustomertype Where customer_type='" + customer_type + "'";
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
                                                            " '" + leadbank_name + "'," +
                                                            " 'y'," +
                                                            " 'Approved'," +
                                                            " 'Not Assigned'," +
                                                            " 'H.Q'," +
                                                            " '" + customer_type + "'," +
                                                            " '" + lscustomer_type + "'," +
                                                            " '" + lsemployee_gid + "'," +
                                                            " 'Y'," +
                                                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                                                                                         
                                                        msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                                                        " (leadbankcontact_gid," +
                                                        " leadbank_gid," +
                                                        " leadbankcontact_name," +
                                                        " mobile," +
                                                        " created_date," +
                                                        " created_by," +
                                                        " leadbankbranch_name, " +
                                                        " main_contact)" +
                                                        " values( " +
                                                        " '" + msGetGid2 + "'," +
                                                        " '" + msGetGid1 + "'," +
                                                        " '" + leadbank_name + "'," +
                                                        " '" + mobile + "'," +
                                                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                        " '" + lsemployee_gid + "'," +
                                                        " 'H.Q'," +
                                                        " 'y'" + ")";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                                        msSQL = "update crm_smm_whatsapp set leadbank_gid='" + msGetGid1 + "' where id='" + objRootobject.id + "'";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                        msSQL = "update crm_trn_tleadbank set wh_id='" + objRootobject.id + "' where leadbank_gid='" + msGetGid1 + "'";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    }                                                   
                                                }
                                                else
                                                {
                                                    if (mobile.StartsWith("+")) 
                                                    {
                                                        msGETinvGID = objcmnfunctions.GetMasterGID("INVDW");

                                                        msSQL = " insert into crm_inv_twhatsappinvNo ( " +
                                                            " invalidate_gid," +
                                                            " upload_gid," +
                                                            " leadbank_name," +
                                                            " mobile ," +
                                                            " customer_type ," +
                                                            " remarks," +
                                                            " created_date ," +
                                                            " created_by" +
                                                            ") values (" +
                                                            "'" + msGETinvGID + "'," +
                                                            "'" + msdocument_gid + "'," +
                                                            "'" + leadbank_name + "'," +
                                                            "'" + mobile + "'," +
                                                            "'" + customer_type + "'," +
                                                            "' A resource with the same identifier already exists: contact identifier already exists on another contact. '," +
                                                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                            "'" + user_gid + "')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    }
                                                    else
                                                    {
                                                        msGETinvGID = objcmnfunctions.GetMasterGID("INVDW");

                                                        msSQL = " insert into crm_inv_twhatsappinvNo ( " +
                                                            " invalidate_gid," +
                                                            " upload_gid," +
                                                            " leadbank_name," +
                                                            " mobile ," +
                                                            " customer_type ," +
                                                            " remarks," +
                                                            " created_date ," +
                                                            " created_by" +
                                                            ") values (" +
                                                            "'" + msGETinvGID + "'," +
                                                            "'" + msdocument_gid + "'," +
                                                            "'" + leadbank_name + "'," +
                                                            "'" + mobile + "'," +
                                                            "'" + customer_type + "'," +
                                                            "' Input your mobile number starting with the country code. '," +
                                                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                            "'" + user_gid + "')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    }                                                    
                                                }
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
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaGetimportcontact(string region_name, string source_name, string customer_type, MdlWhatsapp values)
        {

            msSQL = "SELECT a.leadbank_gid, a.leadbank_name, b.source_name, a.customer_type, a.customertype_gid,c.mobile,d.region_name FROM crm_trn_tleadbank a" +
                       " LEFT JOIN crm_mst_tsource b ON b.source_gid = a.source_gid" +
                       " LEFT JOIN crm_trn_tleadbankcontact c ON c.leadbank_gid = a.leadbank_gid" +
                       " LEFT JOIN crm_mst_tregion d ON d.region_gid = a.leadbank_region ";
            if (region_name != "null" || source_name != "null" || customer_type != "null")
            {
                msSQL += " where 1=1 and c.main_contact ='Y'";
                if (region_name != "null")
                {
                    msSQL += "and a.leadbank_region = '" + region_name + "' ";
                }
                if (source_name != "null")
                {
                    msSQL += "and a.source_gid = '" + source_name + "' ";
                }
                if (customer_type != "null")
                {
                    msSQL += "and a.customertype_gid = '" + customer_type + "' ";
                }
                msSQL += " AND a.wh_id IS NULL AND c.mobile IS NOT NULL AND c.mobile != ''";
            }
            else
            {
                msSQL += " where a.wh_id IS NULL AND c.mobile IS NOT NULL AND c.mobile != '' and c.main_contact ='Y'";
            }

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<whatsleadlist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new whatsleadlist
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        source_name = dt["source_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        region_name = dt["region_name"].ToString(),


                    });
                    values.whatsleadlist = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        // -----------------------------------------start excel log -----------------------------------------------------//
        public void DaGetlogWhatsapplist(MdlWhatsapp values)
        {
            msSQL = " select count( b.whatsapplog_gid) as importmissed_count, a.upload_date, a.upload_by, " +                
                " concat(c.user_firstname ,' ', c.user_lastname) as user_name, a.upload_gid " +
                " from cmr_log_twhatsappexcelllog a " +
                " left join crm_log_twhatsapptrnlog b on b.upload_gid=a.upload_gid" +
                " left join adm_mst_tuser c on c.user_gid=a.upload_by where b.whatsapplog_gid is not null"+
                " GROUP BY a.upload_gid, c.user_firstname, c.user_lastname, a.upload_date, a.upload_by"
                ;
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var whatsapplog = new List<whatslog_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    whatsapplog.Add(new whatslog_list
                    {
                        importmissed_count = dt["importmissed_count"].ToString(),
                        upload_date = dt["upload_date"].ToString(),
                        upload_by = dt["upload_by"].ToString(),
                        upload_gid = dt["upload_gid"].ToString(),
                        user_name = dt["user_name"].ToString(),
                    });
                    values.whatslog_list = whatsapplog;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetlogWhatsappdtllist(string document_gid, MdlWhatsapp values)
        {
            msSQL = " select whatsapplog_gid, leadbank_name, mobile , customer_type, upload_gid from crm_log_twhatsapptrnlog " +
                "  where upload_gid = '" + document_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var whatsappdtllog = new List<whatsdtllog_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    whatsappdtllog.Add(new whatsdtllog_list
                    {
                        whatsapplog_gid = dt["whatsapplog_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        upload_gid = dt["upload_gid"].ToString(),
                    });
                    values.whatsdtllog_list = whatsappdtllog;
                }
            }
            dt_datatable.Dispose();
        }

// -------------------------------------invalid log =======================================//
        public void DaGetlogInvalidnumber( MdlWhatsapp values)
        {
            msSQL = " select count( b.invalidate_gid) as importinvalid_count, a.upload_date, a.upload_by, " +
                " concat(c.user_firstname ,' ', c.user_lastname) as user_name, a.upload_gid " +
                " from cmr_log_twhatsappexcelllog a " +
                " left join crm_inv_twhatsappinvNo b on b.upload_gid=a.upload_gid" +
                " left join adm_mst_tuser c on c.user_gid=a.upload_by where b.invalidate_gid is not null" +
                " GROUP BY a.upload_gid, c.user_firstname, c.user_lastname, a.upload_date, a.upload_by"
                ;
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var invalidlog = new List<invalidlog_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    invalidlog.Add(new invalidlog_list
                    {
                        importinvalid_count = dt["importinvalid_count"].ToString(),
                        upload_date = dt["upload_date"].ToString(),
                        upload_by = dt["upload_by"].ToString(),
                        upload_gid = dt["upload_gid"].ToString(),
                        user_name = dt["user_name"].ToString(),
                    });
                    values.invalidlog_list   = invalidlog;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetloginvalidDTLlist(string document_gid, MdlWhatsapp values)
        {
            msSQL = " select invalidate_gid, leadbank_name, mobile , customer_type, upload_gid, remarks from crm_inv_twhatsappinvNo" +
                "  where upload_gid = '" + document_gid + "'"; 
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var invaliddtllog = new List<invalidDTLlog_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    invaliddtllog.Add(new invalidDTLlog_list
                    {
                        invalidate_gid = dt["invalidate_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        upload_gid = dt["upload_gid"].ToString(),
                        remarks = dt["remarks"].ToString(),
                    });
                    values.invalidDTLlog_list = invaliddtllog;
                }
            }
            dt_datatable.Dispose();
        }

        // -----------------------------------------end excel log -----------------------------------------------------//
        public void DaGetsentcampaignsentchart(MdlWhatsapp values)
        {
            try
            {
                msSQL = " SELECT date_format(a.created_date,'%b-%y') as months,substring(date_format(a.created_date,'%M'),1,3)as month,year(a.created_date) as year,count(a.version_id) AS whatsappsent_count " +
                        " FROM crm_trn_twhatsappmessages a " +
                        " left join crm_smm_whatsapp b on a.contact_id = b.id " +
                        " WHERE a.created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) GROUP BY months order by STR_TO_DATE(months, '%b-%y') ASC ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<campaignsentchart_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new campaignsentchart_lists
                        {
                            whatsappsent_count = dt["whatsappsent_count"].ToString(),
                            months = dt["months"].ToString(),
                            year = dt["year"].ToString(),
                            month = dt["month"].ToString(),

                        });
                        values.campaignsentchart_lists = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetsentDetailSummary(string employee_gid, string month, string year, MdlWhatsapp values)
        {
            try
            {
                    msSQL = " select a.version_id,a.identifiervalue,c.leadbank_gid,c.leadbank_name,d.p_name,c.customer_type,e.source_name,f.region_name from crm_trn_twhatsappmessages a " +
                            " left join crm_smm_whatsapp b on a.contact_id = b.id " +
                            " left join crm_trn_tleadbank c on c.leadbank_gid = b.leadbank_gid " +
                            " LEFT JOIN crm_smm_whatsapptemplate d ON d.version_id = a.version_id " +
                            " LEFT JOIN crm_mst_tsource e ON e.source_gid = c.source_gid " +
                            " LEFT JOIN crm_mst_tregion f ON f.region_gid = c.leadbank_region " +
                            " where a.version_id != '' and substring(date_format(a.created_date,'%M'),1,3)= '" + month + "' and " +
                            " year(a.created_date) = '" + year + "'  order by a.created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetOrderDetailSummary = new List<leadsummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetOrderDetailSummary.Add(new leadsummary
                        {
                            campaign_title = (dt["p_name"].ToString()),
                            leadbank_name = (dt["leadbank_name"].ToString()),
                            region = (dt["region_name"].ToString()),
                            source = (dt["source_name"].ToString()),
                            customer_type = (dt["customer_type"].ToString()),
                            identifiervalue = (dt["identifiervalue"].ToString()),
                          
                        });
                        values.leadsummary = GetOrderDetailSummary;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Detail Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetcampaignSearch(MdlWhatsapp values, string from_date, string to_date)
        {
            try
            {
                if (from_date == null && to_date == null)
                {
                   msSQL = " SELECT date_format(a.created_date,'%b-%y') as months,substring(date_format(a.created_date,'%M'),1,3)as month,year(a.created_date) as year,count(a.version_id) AS whatsappsent_count " +
                        " FROM crm_trn_twhatsappmessages a " +
                        " left join crm_smm_whatsapp b on a.contact_id = b.id " +
                        " WHERE a.created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) GROUP BY months order by STR_TO_DATE(months, '%b-%y') ASC ";

                }
                else
                {
                   msSQL = " SELECT date_format(a.created_date,'%b-%y') as months,substring(date_format(a.created_date,'%M'),1,3)as month,year(a.created_date) as year,count(a.version_id) AS whatsappsent_count  FROM crm_trn_twhatsappmessages a " +
                   " left join crm_smm_whatsapp b on a.contact_id = b.id " +
                   " WHERE a.created_date between '" + from_date + "' and '" + to_date + "'" +
                   " group by date_format(a.created_date,'%M') order by STR_TO_DATE(months, '%b-%y') ASC ";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetOrderForLastSixMonths_List = new List<campaignsentchart_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetOrderForLastSixMonths_List.Add(new campaignsentchart_lists
                        {
                            whatsappsent_count = dt["whatsappsent_count"].ToString(),
                            months = dt["months"].ToString(),
                            year = dt["year"].ToString(),
                            month = dt["month"].ToString(),

                        });
                        values.campaignsentchart_lists = GetOrderForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetcurrentchart(string status ,MdlWhatsapp values)
        {
            try
            {
                if (status == "current") {

                msSQL = " SELECT date_format(a.created_date,'%b-%d') as months,substring(date_format(a.created_date,'%M'),1,3)as month,year(a.created_date) as " +
                        " year,DATE(a.created_date) AS day,count(a.version_id) AS whatsappsent_count FROM crm_trn_twhatsappmessages a " +
                        " left join crm_smm_whatsapp b on a.contact_id = b.id " +
                        " WHERE a.created_date >= DATE_SUB(CURDATE(),  INTERVAL 1 DAY) GROUP BY months, day order by STR_TO_DATE(months, '%b-%y') ASC ";
                }
                else if(status == "7day")
                {
                    msSQL = " SELECT date_format(a.created_date,'%b-%d') as months,substring(date_format(a.created_date,'%M'),1,3)as month,year(a.created_date) as " +
                            " year,DATE(a.created_date) AS day,count(a.version_id) AS whatsappsent_count FROM crm_trn_twhatsappmessages a " +
                            " left join crm_smm_whatsapp b on a.contact_id = b.id " +
                            " WHERE a.created_date >= DATE_SUB(CURDATE(),  INTERVAL 7 DAY) GROUP BY months, day order by STR_TO_DATE(months, '%b-%y') ASC ";
                }
                else if (status == "month")
                {
                    msSQL = " SELECT DATE_FORMAT(a.created_date,'%b-%d') AS months,SUBSTRING(DATE_FORMAT(a.created_date,'%M'),1,3) AS month, " +
                            " YEAR(a.created_date) AS year,DATE(a.created_date) AS day,COUNT(a.version_id) AS whatsappsent_count FROM crm_trn_twhatsappmessages a " +
                            " LEFT JOIN crm_smm_whatsapp b ON a.contact_id = b.id " +
                            " WHERE YEAR(a.created_date) = YEAR(CURRENT_DATE()) AND MONTH(a.created_date) = MONTH(CURRENT_DATE()) " +
                            " GROUP BY months, day ORDER BY STR_TO_DATE(months, '%b-%y') ASC ";
                }
                else
                {
                    msSQL = " SELECT date_format(a.created_date,'%b-%y') as months,substring(date_format(a.created_date,'%M'),1,3)as month,year(a.created_date) as year,count(a.version_id) AS whatsappsent_count " +
                            " FROM crm_trn_twhatsappmessages a " +
                            " left join crm_smm_whatsapp b on a.contact_id = b.id " +
                            " WHERE a.created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) GROUP BY months order by STR_TO_DATE(months, '%b-%y') ASC ";
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<campaignsentchart_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new campaignsentchart_lists
                        {
                            whatsappsent_count = dt["whatsappsent_count"].ToString(),
                            months = dt["months"].ToString(),
                            year = dt["year"].ToString(),
                            month = dt["month"].ToString(),
                            days = dt["day"].ToString(),

                        });
                        values.campaignsentchart_lists = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
            ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public result DaGetmessagestatus()
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            result objresult = new result();
            if(getwhatsappcredentials.access_token == null || getwhatsappcredentials.access_token == "")
            {
                objresult.status = false;
                objresult.message="Whatsapp Credentials Missing Contact Admin ";
            }
            else
            {

                int i = 0;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://nest.messagebird.com");
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.GET);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                request.AddQueryParameter("limit", "99");
                IRestResponse responseAddress = client.Execute(request);
                string address_erpid = responseAddress.Content;
                string errornetsuiteJSON = responseAddress.Content;
                messagestatus objMdlWhatsappMessagestatus = new messagestatus();
                objMdlWhatsappMessagestatus = JsonConvert.DeserializeObject<messagestatus>(errornetsuiteJSON);
                if(objMdlWhatsappMessagestatus.results==null )
                {
                    objresult.status = false;
                    objresult.message = "Whatsapp Credentials Missing Some Properties Contact Admin";
                }
                else
                {
                    fnLoadWhatsappConversations(objMdlWhatsappMessagestatus, null);
                    objresult.status = true;
                }
            }
            return objresult;
        }

        public void fnLoadWhatsappConversations(messagestatus objMdlWhatsappMessagestatus, string pageToken)
        {
            int i = 0;
            foreach (var item in objMdlWhatsappMessagestatus.results)
            {
                msSQL = "update  crm_trn_twhatsappmessages set " +
                   " status = '" + objMdlWhatsappMessagestatus.results[i].status + "'," +
                   " reason = '" + objMdlWhatsappMessagestatus.results[i].reason + "'" +
                   " where message_id ='" + objMdlWhatsappMessagestatus.results[i].id + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            }
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://nest.messagebird.com");
            var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.GET);
            request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
            request.AddQueryParameter("limit", "99");
            request.AddQueryParameter("pageToken", pageToken);
            IRestResponse responseAddress = client.Execute(request);
            string repsonse = responseAddress.Content;
            messagestatus objMdlWhatsappMessagestatus1 = new messagestatus();
            objMdlWhatsappMessagestatus1 = JsonConvert.DeserializeObject<messagestatus>(repsonse);
            if (!String.IsNullOrEmpty(objMdlWhatsappMessagestatus1.nextPageToken))
                fnLoadWhatsappConversations(objMdlWhatsappMessagestatus1, objMdlWhatsappMessagestatus1.nextPageToken);
        }
        public void DaGetcustomerreport(MdlWhatsapp values)
        {
            try
            {


                 msSQL = " select a.leadbank_name,a.leadbank_gid,c.contact_id, b.wvalue,d.p_name,COUNT(c.project_id) AS project_count, e.source_name,f.region_name from crm_trn_tleadbank a " +
                         " inner join crm_smm_whatsapp b on b.leadbank_gid = a.leadbank_gid " +
                         " inner join crm_trn_twhatsappmessages c on c.contact_id = b.id " +
                         " inner join crm_smm_whatsapptemplate d on d.project_id = c.project_id " +
                         " LEFT JOIN crm_mst_tsource e ON e.source_gid = a.source_gid " +
                         " LEFT JOIN crm_mst_tregion f ON f.region_gid = a.leadbank_region " +
                         " where a.wh_id != null or a.wh_id != '' group by a.leadbank_gid " ;

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<customerreport>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new customerreport
                        {
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_id = dt["contact_id"].ToString(),
                            wvalue = dt["wvalue"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            project_count = dt["project_count"].ToString(),

                        });
                        values.customerreport = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Log";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetcampaignlist(MdlWhatsapp values, string contact_id)
        {
            try
            {
                msSQL = " SELECT b.p_name,COUNT(a.project_id) AS project_count FROM crm_trn_twhatsappmessages a " +
                        " inner JOIN crm_smm_whatsapptemplate b ON b.project_id = a.project_id " +
                        " WHERE a.contact_id = '" + contact_id + "' GROUP BY b.p_name ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<campaign_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new campaign_lists
                        {
                            p_name = dt["p_name"].ToString(),
                            project_count = dt["project_count"].ToString(),
                        });
                        values.campaignlists = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Mailmanagement summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}