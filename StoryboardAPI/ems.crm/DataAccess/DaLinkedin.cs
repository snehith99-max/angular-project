using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.crm.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
using OfficeOpenXml;
using System.Configuration;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using RestSharp;
using Newtonsoft.Json;
using System.Web.Http.Results;

namespace ems.crm.DataAccess
{
    public class DaLinkedin
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult1;
        string lsaccess_token, final_path, linkedinText;
        string addedimage_url, addedvideo_url, media_type;  // Initialize addedimage_url


        public MdlLinkedin DaGetlinkedinaccountdetails()
        {
            MdlLinkedin values = new MdlLinkedin();

            try
            {
                msSQL = "select account_id,access_token from crm_smm_tlinkedinservice";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<list_accesstokens>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new list_accesstokens()
                        {
                            account_id = dt["account_id"].ToString(),
                            access_token = dt["access_token"].ToString(),
                        }
                        );
                    }
                    values.list_accesstokens = getModuleList;
                }
                if (values.list_accesstokens != null)
                {
                    for (int i = 0; i < values.list_accesstokens.ToArray().Length; i++)
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient("https://api.linkedin.com");
                        var request = new RestRequest("/rest/organizations/" + values.list_accesstokens[i].account_id + "");
                        request.AddHeader("LinkedIn-Version", "202405");
                        request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                        request.AddHeader("Authorization", "Bearer " + values.list_accesstokens[i].access_token + "");
                        IRestResponse response = client.Execute(request);
                        Mdlcompanydetails objMdlcompanydetails = new Mdlcompanydetails();
                        objMdlcompanydetails = JsonConvert.DeserializeObject<Mdlcompanydetails>(response.Content);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            msSQL = "delete from crm_smm_tlinkedincompany where account_id=" + objMdlcompanydetails.id + "  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                var specialties = string.Join(", ", objMdlcompanydetails.localizedSpecialties);
                                msSQL = "insert into crm_smm_tlinkedincompany(" +
                                   "account_id," +
                                   "company_name," +
                                   "company_website," +
                                   "founded_on," +
                                   "accountcreated_by," +
                                   "description," +
                                   "versionTag," +
                                   "organizationStatus," +
                                   "organizationType," +
                                   "coverPhotocropped," +
                                   "coverPhotoorginal," +
                                   "localizedSpecialties)values(" +
                                   "'" + objMdlcompanydetails.id + "', " +
                                   "'" + objMdlcompanydetails.localizedName + "', " +
                                   "'" + objMdlcompanydetails.website.localized.en_US + "', " +
                                   "'" + objMdlcompanydetails.foundedOn.year + "', " +
                                   "'" + objMdlcompanydetails.created.actor + "', " +
                                   "'" + objMdlcompanydetails.description.localized.en_US + "', " +
                                   "'" + objMdlcompanydetails.versionTag + "', " +
                                   "'" + objMdlcompanydetails.organizationStatus + "', " +
                                   "'" + objMdlcompanydetails.organizationType + "', " +
                                   "'" + objMdlcompanydetails.coverPhotoV2.cropped + "', " +
                                   "'" + objMdlcompanydetails.coverPhotoV2.original + "', " +
                                   "'" + specialties + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult != 0)
                                {

                                    values = locationfn(objMdlcompanydetails.id, values.list_accesstokens[i].access_token);

                                }
                                else
                                {

                                    values.status = false;
                                    values.message = "Error While Inserting  Records";
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Inserting Company details- (DaGetlinkedinaccountdetails)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            return values;
        }
        public MdlLinkedin locationfn(string account_id, string lsaccess_token)
        {
            MdlLinkedin values = new MdlLinkedin();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://api.linkedin.com");
                var request = new RestRequest("/rest/organizations/" + account_id + "");
                request.AddHeader("LinkedIn-Version", "202405");
                request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                request.AddHeader("Authorization", "Bearer " + lsaccess_token);
                IRestResponse response = client.Execute(request);
                Mdlcompanydetails objMdlcompanydetails = new Mdlcompanydetails();
                objMdlcompanydetails = JsonConvert.DeserializeObject<Mdlcompanydetails>(response.Content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    msSQL = "delete from crm_smm_tlinkedincompanydtl where account_id=" + objMdlcompanydetails.id + "  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        foreach (var item in objMdlcompanydetails.locations)
                        {
                            msSQL = "insert into crm_smm_tlinkedincompanydtl(" +
                                                           "account_id," +
                                                           "locationcompany_name," +
                                                           "location_type," +
                                                           "geographicArea," +
                                                           "country," +
                                                           "city," +
                                                           "line2," +
                                                           "line1," +
                                                           "postalCode," +
                                                           "geoLocation)values(" +
                                                           "'" + objMdlcompanydetails.id + "', " +
                                                           "'" + item.description.localized.en_US + "', " +
                                                           "'" + item.locationType + "', " +
                                                           "'" + item.address.geographicArea + "', " +
                                                           "'" + item.address.country + "', " +
                                                           "'" + item.address.city + "', " +
                                                           "'" + item.address.line2 + "', " +
                                                           "'" + item.address.line1 + "', " +
                                                           "'" + item.address.postalCode + "', " +
                                                           "'" + item.geoLocation + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                values = followesfn(account_id, lsaccess_token);
                            }
                            else
                            {

                                values.status = false;
                                values.message = "Error While Loading Records";
                            }
                        }

                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Inserting Company details- (locationfn)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            return values;
        }
        public MdlLinkedin followesfn(string account_id, string lsaccess_token)
        {
            MdlLinkedin values = new MdlLinkedin();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://api.linkedin.com");
                var request = new RestRequest("/rest/networkSizes/urn%3Ali%3Aorganization%3A" + account_id + "?edgeType=COMPANY_FOLLOWED_BY_MEMBER");
                request.AddHeader("LinkedIn-Version", "202405");
                request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                request.AddHeader("Authorization", "Bearer " + lsaccess_token);
                IRestResponse response = client.Execute(request);
                Mdlfollowers objMdlcompanydetails = new Mdlfollowers();
                objMdlcompanydetails = JsonConvert.DeserializeObject<Mdlfollowers>(response.Content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    msSQL = "update crm_smm_tlinkedincompany SET followers_count= " + objMdlcompanydetails.firstDegreeSize + " where account_id ='" + account_id + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Record Loadeds Successfully";
                    }
                    else
                    {

                        values.status = false;
                        values.message = "Error Occured while Loading Records";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured while Loading Records";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Inserting Company details- (followesfn)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            return values;
        }
        public void DaGetlinkedinsummary(MdlLinkedin values)
        {

            try
            {
                msSQL = "select account_id,company_name,company_website,founded_on,followers_count from crm_smm_tlinkedincompany ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList1 = new List<accountsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new accountsummary_list
                        {
                            account_id = dt["account_id"].ToString(),
                            company_name = dt["company_name"].ToString(),
                            company_website = dt["company_website"].ToString(),
                            founded_on = dt["founded_on"].ToString(),
                            followers_count = dt["followers_count"].ToString(),
                        });
                        values.accountsummary_list = getModuleList1;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Loading Page Record !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with Loading Record- (DaGetlinkedinsummary)  *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetlinkedinaccountview(Mdlaccountview values, string account_id)
        {
            try
            {
                msSQL = "select account_id,company_name,company_website,founded_on,followers_count,description,organizationStatus,organizationType,localizedSpecialties from crm_smm_tlinkedincompany where account_id='" + account_id + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.account_id = objOdbcDataReader["account_id"].ToString();
                    values.company_name = objOdbcDataReader["company_name"].ToString();
                    values.company_website = objOdbcDataReader["company_website"].ToString();
                    values.founded_on = objOdbcDataReader["founded_on"].ToString();
                    values.followers_count = objOdbcDataReader["followers_count"].ToString();
                    values.description = objOdbcDataReader["description"].ToString();
                    values.organizationStatus = objOdbcDataReader["organizationStatus"].ToString();
                    values.organizationType = objOdbcDataReader["organizationType"].ToString();
                    values.localizedSpecialties = objOdbcDataReader["localizedSpecialties"].ToString();
                }

                msSQL = " select locationcompany_name,location_type,geographicArea,country,city,line2,line1,postalCode from crm_smm_tlinkedincompanydtl where account_id='" + account_id + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<accountview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new accountview_list
                        {
                            locationcompany_name = dt["locationcompany_name"].ToString(),
                            location_type = dt["location_type"].ToString(),
                            geographicArea = dt["geographicArea"].ToString(),
                            country = dt["country"].ToString(),
                            city = dt["city"].ToString(),
                            line2 = dt["line2"].ToString(),
                            line1 = dt["line1"].ToString(),
                            postalCode = dt["postalCode"].ToString(),
                        });
                        values.accountview_list = getModuleList;
                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while View Account  Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While view Company details- (DaGetlinkedinaccountview)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public result DaLinkedintextpost(postvalues values, string user_gid)
        {
            result objResult = new result();
            try
            {
                msSQL = "select access_token from crm_smm_tlinkedinservice where account_id='" + values.account_id + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsaccess_token = objOdbcDataReader["access_token"].ToString();
                }
                if (lsaccess_token != null)
                {
                    string linkedinText = values.linkedin_text.Replace("\n", "").Replace("  ", " ").Trim();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient("https://api.linkedin.com");
                    var request = new RestRequest("/rest/posts", Method.POST);
                    request.AddHeader("LinkedIn-Version", "202405");
                    request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                    request.AddHeader("Authorization", "Bearer " + lsaccess_token);
                    var body = "{\"author\":\"urn:li:organization:" + values.account_id + "\",\"commentary\":\"" + linkedinText + "\",\"visibility\":\"PUBLIC\",\"distribution\":{\"feedDistribution\":\"MAIN_FEED\",\"targetEntities\":[],\"thirdPartyDistributionChannels\":[]},\"lifecycleState\":\"PUBLISHED\",\"isReshareDisabledByAuthor\":false}";

                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        objResult.status = true;
                        objResult.message = "Posted Sucessfully";

                    }
                    else
                    {
                        string content = response.Content;
                        if (content.Contains("code\":\"DUPLICATE_POST\""))
                        {
                            objResult.status = false;
                            objResult.message = "Duplicate post is detected";
                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Error Occured While Posting !!";
                            ;
                        }

                    }

                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Check the Access Token!!";
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured While Post Text- (DaLinkedintextpost)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return objResult;


        }
        public void DaLinkedinmediapost(HttpRequest httpRequest, result objResult)
        {
            try
            {
                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                string document_gid = string.Empty;
                string linkedin_text = httpRequest.Form[0];
                string account_id = httpRequest.Form[1];

                if (httpRequest.Files.Count > 0)
                {
                    httpFileCollection = httpRequest.Files;
                    HttpPostedFile httpPostedFile = httpFileCollection[0]; // Get the first file
                    string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                    string FileExtension = Path.GetExtension(httpPostedFile.FileName).ToLower();
                    string lsfile_gid = msdocument_gid + FileExtension;

                    byte[] fileBytes;
                    using (var binaryReader = new BinaryReader(httpPostedFile.InputStream))
                    {
                        fileBytes = binaryReader.ReadBytes(httpPostedFile.ContentLength);
                    }
                    msSQL = " select access_token from crm_smm_tlinkedinservice where account_id= '" + account_id + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsaccess_token = objOdbcDataReader["access_token"].ToString();
                    }
                    if (lsaccess_token != null)
                    {
                        if (FileExtension == ".jpg" | FileExtension == ".png" | FileExtension == ".jpeg" | FileExtension == ".gif" | FileExtension == ".JPG" | FileExtension == ".JPEG" | FileExtension == ".JIFF" | FileExtension == ".TIFF" | FileExtension == ".PNG" | FileExtension == ".GIF" | FileExtension == ".JFIF" | FileExtension == ".svg" | FileExtension == ".jfif")
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                            var client = new RestClient("https://api.linkedin.com");
                            var request = new RestRequest("/v2/assets?action=registerUpload", Method.POST);
                            request.AddHeader("LinkedIn-Version", "202405");
                            request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                            request.AddHeader("Authorization", "Bearer " + lsaccess_token);

                            var body = "{\"registerUploadRequest\":{\"recipes\":[\"urn:li:digitalmediaRecipe:feedshare-image\"],\"owner\":\"urn:li:organization:" + account_id + "\",\"serviceRelationships\":[{\"relationshipType\":\"OWNER\",\"identifier\":\"urn:li:userGeneratedContent\"}]}}";
                            request.AddParameter("application/json", body, ParameterType.RequestBody);

                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var responseData = JsonConvert.DeserializeObject<Responses>(response.Content);
                                string upload_url = responseData.value.uploadMechanism.MediaUploadHttpRequest.uploadUrl;
                                string assest = responseData.value.asset;
                                string file_type = httpPostedFile.ContentType;
                                string media_type = "IMAGE";
                                string filePath = ConfigurationManager.AppSettings["blob_containername"] + "/Linkedin/";
                                bool messae = uploadfilefn(upload_url, assest, lsaccess_token, fileBytes, linkedin_text, account_id, file_type, media_type, objResult);
                                Console.WriteLine(messae);

                            }
                            else if (response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                objResult.status = false;
                                objResult.message = "Error occurred while Uploading!!";
                            }
                        }
                        else if (FileExtension == ".mp4" | FileExtension == ".MP4" | FileExtension == ".avi" | FileExtension == ".mkv" | FileExtension == ".wmv" | FileExtension == ".mov" | FileExtension == ".WebM" | FileExtension == ".flv" | FileExtension == ".hevc" | FileExtension == ".vpg")

                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                            var client = new RestClient("https://api.linkedin.com");
                            var request = new RestRequest("/v2/assets?action=registerUpload", Method.POST);
                            request.AddHeader("LinkedIn-Version", "202405");
                            request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                            request.AddHeader("Authorization", "Bearer " + lsaccess_token);

                            var body = "{\"registerUploadRequest\":{\"recipes\":[\"urn:li:digitalmediaRecipe:feedshare-video\"],\"owner\":\"urn:li:organization:" + account_id + "\",\"serviceRelationships\":[{\"relationshipType\":\"OWNER\",\"identifier\":\"urn:li:userGeneratedContent\"}]}}";
                            request.AddParameter("application/json", body, ParameterType.RequestBody);

                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var responseData = JsonConvert.DeserializeObject<Responses>(response.Content);
                                string upload_url = responseData.value.uploadMechanism.MediaUploadHttpRequest.uploadUrl;
                                string assest = responseData.value.asset;
                                string file_type = httpPostedFile.ContentType;
                                string media_type = "VIDEO";
                                string filePath = ConfigurationManager.AppSettings["blob_containername"] + "/Linkedin/";
                                bool messae = uploadfilefn(upload_url, assest, lsaccess_token, fileBytes, linkedin_text, account_id, file_type, media_type, objResult);
                                Console.WriteLine(messae);

                            }
                            else if (response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                objResult.status = false;
                                objResult.message = "Error occurred while Uploading!";
                            }
                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Invalid File Format!";
                        }
                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Check the Access Token!";
                    }
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "No files uploaded!";
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occurred while Uploading";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Register- (DaLinkedinmediapost)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public bool uploadfilefn(string upload_url, string asset, string lsaccess_token, byte[] fileBytes, string linkedin_text, string account_id, string file_type, string media_type, result objResult)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(upload_url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("LinkedIn-Version", "202405");
                request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                request.AddHeader("Authorization", "Bearer " + lsaccess_token);
                request.AddHeader("Content-Type", file_type);
                request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return CreatePost(lsaccess_token, asset, linkedin_text, account_id, file_type, media_type, objResult);
                }
                else
                {
                    objResult.message = "Error Occured at upload file";
                    objResult.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occurred while uploading file";
                objResult.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While uploading- (uploadfilefn)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return false;
            }

        }
        private bool CreatePost(string lsaccess_token, string asset, string linkedin_text, string account_id, string file_type, string media_type, result objResult)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://api.linkedin.com");
                var request = new RestRequest("/v2/ugcPosts", Method.POST);
                request.AddHeader("LinkedIn-Version", "202405");
                request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                request.AddHeader("Authorization", "Bearer " + lsaccess_token);
                var body = "{\"author\":\"urn:li:organization:" + account_id + "\",\"lifecycleState\":\"PUBLISHED\",\"specificContent\":{\"com.linkedin.ugc.ShareContent\":{\"shareCommentary\":{\"text\":\"" + linkedin_text + "\"},\"shareMediaCategory\":\"" + media_type + "\",\"media\":[{\"status\":\"READY\",\"description\":{\"text\":\"Center stage!\"},\"media\":\"" + asset + "\"}]}},\"visibility\":{\"com.linkedin.ugc.MemberNetworkVisibility\":\"PUBLIC\"}}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    objResult.status = true;
                    objResult.message = "Post Created successfully";
                    return true;
                }
                else
                {

                    objResult.message = "Error Occured at Post";
                    objResult.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occurred while Creating Post";
                objResult.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While creating post- (CreatePost)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return false;
            }
        }
        public result DaLinkedinpollpost(pollpost values, string user_gid)
        {
            result objResult = new result();
            try
            {

                msSQL = "select access_token from crm_smm_tlinkedinservice where account_id='" + values.account_id + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    lsaccess_token = objOdbcDataReader["access_token"].ToString();
                }
                if (lsaccess_token != null)
                {
                    if (values.linkedin_text != null)
                    {
                        linkedinText = values.linkedin_text.Replace("\n", "").Replace("  ", " ").Trim();

                    }
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient("https://api.linkedin.com");
                    var request = new RestRequest("/rest/posts", Method.POST);
                    request.AddHeader("LinkedIn-Version", "202405");
                    request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                    request.AddHeader("Authorization", "Bearer " + lsaccess_token);
                    var body = "{\"author\":\"urn:li:organization:" + values.account_id + "\",\"commentary\":\"" + linkedinText + "\",\"visibility\":\"PUBLIC\",\"distribution\":{\"feedDistribution\":\"MAIN_FEED\",\"targetEntities\":[],\"thirdPartyDistributionChannels\":[]},\"lifecycleState\":\"PUBLISHED\",\"isReshareDisabledByAuthor\":false,\"content\":{\"poll\":{\"question\":\"" + values.poll_question + "\",\"options\":[{\"text\":\"" + values.option1 + "\"},{\"text\":\"" + values.option2 + "\"}";

                    // Check for option 3 an 4 exist?///
                    if (!string.IsNullOrEmpty(values.option3))
                    {
                        body += ",{\"text\":\"" + values.option3 + "\"}";
                    }
                    if (!string.IsNullOrEmpty(values.option4))
                    {
                        body += ",{\"text\":\"" + values.option4 + "\"}";
                    }
                    body += "],\"settings\":{\"duration\":\"" + values.poll_duration + "\"}}}}";

                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        objResult.status = true;
                        objResult.message = "Posted Sucessfully";

                    }
                    else
                    {
                        string content = response.Content;
                        if (content.Contains("code\":\"DUPLICATE_POST\""))
                        {
                            objResult.status = false;
                            objResult.message = "Duplicate post is detected";
                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Error Occured While Posting !!";
                            ;
                        }

                    }

                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Check the Access Token!!";
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured While Post Text- (DaLinkedintextpost)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Linkedin/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return objResult;


        }
        public void DaGetLinkedinpost(MdlLinkedin values, string account_id)
        {
            try
            {
                msSQL = "select access_token from  crm_smm_tlinkedinservice where account_id ='" + account_id + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsaccess_token = objOdbcDataReader["access_token"].ToString();
                    result result = new result();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var Client = new RestClient("https://api.linkedin.com");
                    var request = new RestRequest("/rest/posts?author=urn%3Ali%3Aorganization%3A" + account_id + "&q=author&count=100&sortBy=LAST_MODIFIED", Method.GET);
                    request.AddHeader("LinkedIn-Version", "202405");
                    request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                    request.AddHeader("Authorization", "Bearer " + lsaccess_token);
                    IRestResponse response = Client.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    mdlgetpost objMdllinkedinMessageResponse = new mdlgetpost();
                    objMdllinkedinMessageResponse = JsonConvert.DeserializeObject<mdlgetpost>(errornetsuiteJSON);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        msSQL = "delete from  crm_smm_tlinkedinpost where author ='" + account_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = "delete from  crm_smm_tlinkedinpostdtl where author ='" + account_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = "update set total_post=" + objMdllinkedinMessageResponse.paging.total + " where account_id='"+account_id+"' ";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        foreach (var item in objMdllinkedinMessageResponse.elements)
                        {
                            DateTime createdAtDateTime = DateTimeOffset.FromUnixTimeMilliseconds(item.createdAt).UtcDateTime;
                            string created_at = createdAtDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            item.commentary = item.commentary.Replace("'", "''").Replace("\\r\\n", "");
                            if (item.content.media != null)
                            {
                                string media_type = "Media";
                                msSQL = " insert into crm_smm_tlinkedinpost (post_id,Resharedisabledby_Author,createdAt,visibility,author,post_type,image_url,caption,isEditedByAuthor)" +
                                        " values ('" + item.id + "','" + item.isReshareDisabledByAuthor + "','" + created_at + "','" + item.visibility + "','" + item.author.Replace("urn:li:organization:", "") + "'," +
                                        "'" + media_type + "','" + item.content.media.id + "','" + item.commentary + "','" + item.lifecycleStateInfo.isEditedByAuthor + "')";
                                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            else if (item.content.poll != null)
                            {
                                string media_type = "Poll";
                                if (item.content.poll.options.Length == 2)
                                {
                                    msSQL = " insert into  crm_smm_tlinkedinpost (post_id,Resharedisabledby_Author,createdAt,visibility,author,post_type,caption,isEditedByAuthor,isVotedByViewer1,voteCount1,text1,isVotedByViewer2,voteCount2,text2,duration,voteSelectionType,isVoterVisibleToAuthor,question,uniqueVotersCount)" +
                                            " values ('" + item.id + "','" + item.isReshareDisabledByAuthor + "','" + created_at + "','" + item.visibility + "','" + item.author.Replace("urn:li:organization:", "") + "'," +
                                            "'" + media_type + "','" + item.commentary + "','" + item.lifecycleStateInfo.isEditedByAuthor + "','" + item.content.poll.options[0].isVotedByViewer + "','" + item.content.poll.options[0].voteCount + "'," +
                                            "'" + item.content.poll.options[0].text + "','" + item.content.poll.options[1].isVotedByViewer + "','" + item.content.poll.options[1].voteCount + "','" + item.content.poll.options[1].text + "', '" + item.content.poll.settings.duration + "', '" + item.content.poll.settings.voteSelectionType + "', " +
                                            "'" + item.content.poll.settings.isVoterVisibleToAuthor + "','" + item.content.poll.question + "','" + item.content.poll.uniqueVotersCount + "' )  ";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                                else if (item.content.poll.options.Length == 3)
                                {
                                    msSQL = " insert into crm_smm_tlinkedinpost (post_id,Resharedisabledby_Author,createdAt,visibility,author,post_type,caption,isEditedByAuthor,isVotedByViewer1,voteCount1,text1,isVotedByViewer2,voteCount2,text2,isVotedByViewer3,voteCount3,text3,duration,voteSelectionType,isVoterVisibleToAuthor,question,uniqueVotersCount)" +
                                      " values ('" + item.id + "','" + item.isReshareDisabledByAuthor + "','" + created_at + "','" + item.visibility + "','" + item.author.Replace("urn:li:organization:", "") + "'," +
                                      "'" + media_type + "','" + item.commentary + "','" + item.lifecycleStateInfo.isEditedByAuthor + "','" + item.content.poll.options[0].isVotedByViewer + "','" + item.content.poll.options[0].voteCount + "'," +
                                      "'" + item.content.poll.options[0].text + "','" + item.content.poll.options[1].isVotedByViewer + "','" + item.content.poll.options[1].voteCount + "','" + item.content.poll.options[1].text + "','" + item.content.poll.options[2].isVotedByViewer + "','" + item.content.poll.options[2].voteCount + "'," +
                                      "'" + item.content.poll.options[2].text + "', '" + item.content.poll.settings.duration + "', '" + item.content.poll.settings.voteSelectionType + "','" + item.content.poll.settings.isVoterVisibleToAuthor + "','" + item.content.poll.question + "','" + item.content.poll.uniqueVotersCount + "' )  ";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                }
                                else
                                {
                                    msSQL = " insert into  crm_smm_tlinkedinpost (post_id,Resharedisabledby_Author,createdAt,visibility,author,post_type,caption,isEditedByAuthor,isVotedByViewer1,voteCount1,text1,isVotedByViewer2,voteCount2,text2,isVotedByViewer3,voteCount3,text3,isVotedByViewer4,voteCount4,text4,duration,voteSelectionType,isVoterVisibleToAuthor,question,uniqueVotersCount)" +
                                     " values ('" + item.id + "','" + item.isReshareDisabledByAuthor + "','" + created_at + "','" + item.visibility + "','" + item.author.Replace("urn:li:organization:", "") + "'," +
                                     "'" + media_type + "','" + item.commentary + "','" + item.lifecycleStateInfo.isEditedByAuthor + "','" + item.content.poll.options[0].isVotedByViewer + "','" + item.content.poll.options[0].voteCount + "'," +
                                     "'" + item.content.poll.options[0].text + "','" + item.content.poll.options[1].isVotedByViewer + "','" + item.content.poll.options[1].voteCount + "','" + item.content.poll.options[1].text + "','" + item.content.poll.options[2].isVotedByViewer + "','" + item.content.poll.options[2].voteCount + "'," +
                                     "'" + item.content.poll.options[2].text + "',   '" + item.content.poll.options[3].isVotedByViewer + "','" + item.content.poll.options[3].voteCount + "','" + item.content.poll.options[3].text + "', '" + item.content.poll.settings.duration + "', '" + item.content.poll.settings.voteSelectionType + "','" + item.content.poll.settings.isVoterVisibleToAuthor + "','" + item.content.poll.question + "','" + item.content.poll.uniqueVotersCount + "'  )  ";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                }


                                if (mnResult == 0)
                                {
                                    values.message = "Error!";

                                }
                            }

                            else if (item.content.multiImage != null)
                            {
                                string media_type = "Multi Images";

                                foreach (var image in item.content.multiImage.images)
                                {

                                    msSQL = " insert into crm_smm_tlinkedinpostdtl (post_id,Resharedisabledby_Author,createdAt,visibility,author,post_type,image_url,caption,isEditedByAuthor)" +
                                     " values ('" + item.id + "','" + item.isReshareDisabledByAuthor + "','" + created_at + "','" + item.visibility + "','" + item.author.Replace("urn:li:organization:", "") + "'," +
                                     "'" + media_type + "','" + image.id + "','" + item.commentary + "','" + item.lifecycleStateInfo.isEditedByAuthor + "')";
                                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }

                            }
                            else if (item.content.article != null)
                            {

                                string media_type = "Article";
                                msSQL = " insert into crm_smm_tlinkedinpost (post_id,Resharedisabledby_Author,createdAt,visibility,author,post_type,image_url,caption,isEditedByAuthor,description,source,title)" +
                                        " values ('" + item.id + "','" + item.isReshareDisabledByAuthor + "','" + created_at + "','" + item.visibility + "','" + item.author.Replace("urn:li:organization:", "") + "'," +
                                        "'" + media_type + "','" + item.content.article.thumbnail + "','" + item.commentary + "','" + item.lifecycleStateInfo.isEditedByAuthor + "','" + item.content.article.description + "','" + item.content.article.source + "','" + item.content.article.title + "' )";
                                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            else
                            {
                                values.message = "Invalid Format";
                            }

                        }

                        fngetimage(account_id, lsaccess_token);

                    }
                    else

                    {
                        string content = response.Content;
                        if (content.Contains("code\":\"TOO_MANY_REQUESTS\""))
                        {
                            values.status = false;
                            values.message = "Day Limit for Api Reached!!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Errpr While Loading Records!!";
                        }
                    }

                }
                else
                {
                    values.message = "Check the access token";
                }
            }
            catch (Exception ex)
            {

            }
        }



        public downloadevent fngetimage(string account_id, string lsaccess_token)
        {
            downloadevent values = new downloadevent();
            try
            {
                string msSQL = "select image_url, post_type from crm_smm_tlinkedinpost where author='" + account_id + "' and image_url IS NOT NULL" +
                               " UNION ALL  select image_url as dtl_image_url, post_type as dtl_post_type from crm_smm_tlinkedinpostdtl where author='" + account_id + "' and image_url IS NOT NULL ";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<postsummarylist>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        if (dt["image_url"] != DBNull.Value)
                        {
                            getModuleList.Add(new postsummarylist
                            {
                                image_url = dt["image_url"].ToString(),
                                post_type = dt["post_type"].ToString(),
                                tableType = "post"
                            });
                        }
                        else if (dt["dtl_image_url"] != DBNull.Value)
                        {
                            getModuleList.Add(new postsummarylist
                            {
                                image_url = dt["dtl_image_url"].ToString(),
                                post_type = dt["dtl_post_type"].ToString(),
                                tableType = "postdtl"
                            });
                        }
                    }
                }

                values.postsummarylist = getModuleList;

                List<string> imageIds = new List<string>();
                List<string> videoIds = new List<string>();
                foreach (var summaryItem in values.postsummarylist)
                {
                    if (summaryItem.image_url.StartsWith("urn:li:image:") && (summaryItem.post_type == "Media" || summaryItem.post_type == "Multi Images"))
                    {
                        string encoded_url = summaryItem.image_url.Replace(":", "%3A");
                        imageIds.Add(encoded_url);
                    }
                    else if (summaryItem.image_url.StartsWith("urn:li:video:") && summaryItem.post_type == "Media")
                    {
                        string encoded_url = summaryItem.image_url.Replace(":", "%3A");
                        videoIds.Add(encoded_url);
                    }
                }

                if (imageIds.Count > 0)
                {
                    string addedimage_url = string.Join(",", imageIds);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var Client = new RestClient("https://api.linkedin.com");
                    var request = new RestRequest("/rest/images?ids=List(" + addedimage_url + ")", Method.GET);
                    request.AddHeader("LinkedIn-Version", "202405");
                    request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                    request.AddHeader("Authorization", "Bearer " + lsaccess_token);

                    IRestResponse response = Client.Execute(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                        foreach (var apiResult in apiResponse.Results.Values)
                        {
                            string transformedId = apiResult.Id.Replace(":", "%3A").Replace("/", "%2F");
                            if (imageIds.Contains(transformedId))
                            {
                                string media_type = "Image";
                                DateTime urlExpires = DateTimeOffset.FromUnixTimeMilliseconds(apiResult.DownloadUrlExpiresAt).UtcDateTime;
                                string expires = urlExpires.ToString("yyyy-MM-dd HH:mm:ss");

                                if (values.postsummarylist.Any(p => p.image_url == apiResult.Id && p.tableType == "post"))
                                {
                                    msSQL = "update crm_smm_tlinkedinpost set media_type='" + media_type + "',imagedownload_url ='" + apiResult.DownloadUrl + "'," +
                                                  "downloadUrlExpiresAt='" + expires + "',status='" + apiResult.Status + "' where image_url='" + apiResult.Id + "'";
                                }
                                else if (values.postsummarylist.Any(p => p.image_url == apiResult.Id && p.tableType == "postdtl"))
                                {
                                    msSQL = "update crm_smm_tlinkedinpostdtl set media_type='" + media_type + "',imagedownload_url ='" + apiResult.DownloadUrl + "'," +
                                                  "downloadUrlExpiresAt='" + expires + "',status='" + apiResult.Status + "' where image_url='" + apiResult.Id + "'";
                                }

                                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }
                }

                if (videoIds.Count > 0)
                {
                    string addedvideo_url = string.Join(",", videoIds);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var Client = new RestClient("https://api.linkedin.com");
                    var request = new RestRequest("/rest/videos?ids=List(" + addedvideo_url + ")", Method.GET);
                    request.AddHeader("LinkedIn-Version", "202405");
                    request.AddHeader("X-Restli-Protocol-Version", "2.0.0");
                    request.AddHeader("Authorization", "Bearer " + lsaccess_token);

                    IRestResponse response = Client.Execute(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                        foreach (var apiResult in apiResponse.Results.Values)
                        {
                            string transformedId = apiResult.Id.Replace(":", "%3A").Replace("/", "%2F");
                            if (videoIds.Contains(transformedId))
                            {
                                string media_type = "Video";
                                DateTime urlExpires = DateTimeOffset.FromUnixTimeMilliseconds(apiResult.DownloadUrlExpiresAt).UtcDateTime;
                                string expires = urlExpires.ToString("yyyy-MM-dd HH:mm:ss");

                                string updateQuery = "update crm_smm_tlinkedinpost set media_type='" + media_type + "',imagedownload_url ='" + apiResult.DownloadUrl + "'," +
                                                     "downloadUrlExpiresAt='" + expires + "',status='" + apiResult.Status + "',videoduration='" + apiResult.Duration + "' where image_url='" + apiResult.Id + "'";

                                mnResult1 = objdbconn.ExecuteNonQuerySQL(updateQuery);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., log the error
            }
            return values;
        }

        public void DaGetLinkedinpostsummary(string account_id, Mdlpostsummary values)
        {

            try
            {
                msSQL = "Select company_name,total_post from crm_smm_tlinkedincompany where account_id='" + account_id + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                  values.lscompany_name = objOdbcDataReader["company_name"].ToString();
                  values.lstotal_post = objOdbcDataReader["total_post"].ToString();
                }

                msSQL = "select post_id,createdAt,visibility,media_type,image_url,imagedownload_url,caption from crm_smm_tlinkedinpost where author= '" + account_id + "' and image_url IS NOT NULL" +
                   " UNION ALL  select post_id as dtlpost_id,createdAt as dtlcreatedAt,visibility as dtlvisibility,media_type as dtlmedia_type,image_url as dtlimage_url,imagedownload_url as dtlimagedownload_url,caption as dtlcaption from crm_smm_tlinkedinpostdtl where author='" + account_id + "' and image_url IS NOT NULL ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<postsummarylistview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        if (dt["image_url"] != DBNull.Value)
                        {
                            getModuleList.Add(new postsummarylistview
                            {
                                post_id = dt["post_id"].ToString(),
                                createdAt = dt["createdAt"].ToString(),
                                visibility = dt["visibility"].ToString(),
                                media_type = dt["media_type"].ToString(),
                                image_url = dt["image_url"].ToString(),
                                imagedownload_url = dt["imagedownload_url"].ToString(),
                                caption = dt["caption"].ToString(),

                            });
                        }
                        else if (dt["dtl_image_url"] != DBNull.Value)
                        {
                            getModuleList.Add(new postsummarylistview
                            {
                                post_id = dt["dtlpost_id"].ToString(),
                                createdAt = dt["dtlcreatedAt"].ToString(),
                                visibility = dt["dtlvisibility"].ToString(),
                                media_type = dt["dtlmedia_type"].ToString(),
                                image_url = dt["dtlimage_url"].ToString(),
                                imagedownload_url = dt["dtlimagedownload_url"].ToString(),
                                caption = dt["dtlcaption"].ToString(),
                            });
                        }
                        values.postsummarylistview = getModuleList;

                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}