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
using System.Web.UI.WebControls;
using System.Web.Http.Results;
using System.Data.SqlTypes;
using static OfficeOpenXml.ExcelErrorValue;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace ems.crm.DataAccess
{
    public class DaFacebook
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid, msGetGid1, final_path, lsaccess_token, errorMessage;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, mnResult6, mnResult9;

        public void DaGetPagedetails(MdlFacebook values)
        {
            try
            {
                msSQL = " select page_id,access_token from crm_smm_tfacebookservice";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<list_accesstoken>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new list_accesstoken
                        {
                            page_id = dt["page_id"].ToString(),
                            access_token = dt["access_token"].ToString()
                        });
                        values.list_accesstoken = getModuleList;
                    }
                }
                if (values.list_accesstoken != null)
                {
                    for (int i = 0; i < values.list_accesstoken.ToArray().Length; i++)
                    {
                        result result = new result();
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        string requestAddressURL = "https://graph.facebook.com/v18.0/me?fields=id%2Cname%2Ccategory%2Cfollowers_count%2Clink%2Cpicture%2Cvideos%7Bpermalink_url%2Csource%2Cid%2Cviews%2Cdescription%2Ccreated_time%2Ccomments%7Bid%2Ccreated_time%2Cmessage%7D%7D%2Cposts%7Bfull_picture%2Cpermalink_url%2Cmessage%2Ccreated_time%2Ccomments%7Bid%2Cmessage%2Ccreated_time%7D%7D&access_token=" + values.list_accesstoken[i].access_token + "";
                        var clientAddress = new RestClient(requestAddressURL);
                        var requestAddress = new RestRequest(Method.GET);
                        IRestResponse responseAddress = clientAddress.Execute(requestAddress);
                        string address_erpid = responseAddress.Content;
                        string errornetsuiteJSON = responseAddress.Content;
                        facebooklist objMdlFacebookMessageResponse = new facebooklist();
                        objMdlFacebookMessageResponse = JsonConvert.DeserializeObject<facebooklist>(errornetsuiteJSON);
                        if (responseAddress.StatusCode == HttpStatusCode.OK)
                        {
                            msSQL = "delete from crm_smm_facebookdetails where  id = '" + objMdlFacebookMessageResponse.id + "'";
                            mnResult9 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult9 == 1)
                            {
                                msSQL = "insert into crm_smm_facebookdetails(" +
                                                     "id," +
                                                     "user_name," +
                                                     "profile_picture," +
                                                    "friends_count," +
                                                    "page_category," +
                                                   "page_link," +
                                                    "facebook_type," +
                                                     "created_date)" +
                                                     "values(" +
                                                     "'" + objMdlFacebookMessageResponse.id + "'," +
                                                     "'" + objMdlFacebookMessageResponse.name + "'," +
                                                    "'" + objMdlFacebookMessageResponse.picture.data.url + "'," +
                                                    "'" + objMdlFacebookMessageResponse.followers_count + "'," +
                                                    "'" + objMdlFacebookMessageResponse.category + "'," +
                                                     "'" + objMdlFacebookMessageResponse.link + "'," +
                                                    " 'Page', " +
                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult1 == 0)
                                {

                                    result.message = "Failed!";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Inserting account details-(DaGetPagedetails)!! " + objMdlFacebookMessageResponse.id + " " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                }
                            }
                        }
                        else
                        {

                            result.message = "Failed!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with accesstoken- (DaGetPagedetails) " + values.list_accesstoken[i].access_token + " *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting page details(DaGetPagedetails)";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured getting account details-(DaGetPagedetails)!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaGetPagedetailssummary(MdlFacebook values)
        {

            try
            {
                msSQL = " select a.facebook_gid,a.id,a.page_category,a.user_name,a.profile_picture,a.friends_count,a.page_link " +
                    " from crm_smm_facebookdetails a left join crm_smm_tfacebookservice b on b.page_id=a.id  where  b.page_id=a.id;";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList1 = new List<facebookpage_summarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new facebookpage_summarylist
                        {
                            facebook_gid = dt["facebook_gid"].ToString(),
                            facebook_page_id = dt["id"].ToString(),
                            page_category = dt["page_category"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            profile_picture = dt["profile_picture"].ToString(),
                            friends_count = dt["friends_count"].ToString(),
                            page_link = dt["page_link"].ToString(),

                        });
                        values.facebookpage_summarylist = getModuleList1;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Loading Page Record !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with Loading Record- (DaGetPagedetailssummary)  *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetpagepost(MdlFacebook values, string page_id)
        {
            try
            {
                msSQL = " select access_token from crm_smm_tfacebookservice where page_id='" + page_id + "'  ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.access_token = objOdbcDataReader["access_token"].ToString();
                }
                if (values.access_token != null)
                {
                    result result = new result();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    string requestAddressURL = "https://graph.facebook.com/v18.0/me?fields=id%2Cname%2Ccategory%2Cfollowers_count%2Clink%2Cpicture%2Cvideos%7Bpermalink_url%2Csource%2Cid%2Cviews%2Cdescription%2Ccreated_time%2Ccomments%7Bid%2Ccreated_time%2Cmessage%7D%7D%2Cposts%7Bfull_picture%2Cpermalink_url%2Cmessage%2Ccreated_time%2Ccomments%7Bid%2Cmessage%2Ccreated_time%7D%7D&access_token=" + values.access_token + "";
                    var clientAddress = new RestClient(requestAddressURL);
                    var requestAddress = new RestRequest(Method.GET);
                    IRestResponse responseAddress = clientAddress.Execute(requestAddress);
                    string address_erpid = responseAddress.Content;
                    string errornetsuiteJSON = responseAddress.Content;
                    facebooklist objMdlFacebookMessageResponse = new facebooklist();
                    objMdlFacebookMessageResponse = JsonConvert.DeserializeObject<facebooklist>(errornetsuiteJSON);
                    if (responseAddress.StatusCode == HttpStatusCode.OK)
                    {
                        msSQL = "delete from crm_smm_facebookpage where  page_id = '" + page_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "delete from crm_smm_facebookpagedtl where  page_id = '" + page_id + "'";
                            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult1 == 1)
                            {
                                if (objMdlFacebookMessageResponse.posts != null)
                                {
                                    var imagelist = objMdlFacebookMessageResponse.posts.data;
                                    foreach (var item in imagelist)
                                    {
                                        string message = System.Net.WebUtility.HtmlEncode(item.message);

                                        Console.WriteLine("Original Emoji: " + item.message);
                                        Console.WriteLine("HTML Entity Code: " + message);

                                        msGetGid = objcmnfunctions.GetMasterGID("FB");

                                        msSQL = "insert into crm_smm_facebookpage(" +
                                              "facebookmain_gid," +
                                             "post_id," +
                                             "page_id," +
                                             "post_type," +
                                             "post_url," +
                                             "permalink_url," +
                                             "caption," +
                                             "postcreated_time )" +
                                            "values(" +
                                            " '" + msGetGid + "'," +
                                            "'" + item.id + "'," +
                                           "'" + objMdlFacebookMessageResponse.id + "'," +
                                            " 'Picture', " +
                                            "'" + item.full_picture + "'," +
                                            "'" + item.permalink_url + "'," +
                                            "'" + message + "'," +
                                            "'" + item.created_time.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            if (item.comments != null)
                                            {

                                                foreach (var items in item.comments.data)
                                                {
                                                    string commentmessage = System.Net.WebUtility.HtmlEncode(items.message);

                                                    Console.WriteLine("Original Emoji: " + items.message);
                                                    Console.WriteLine("HTML Entity Code: " + commentmessage);

                                                    msSQL = "insert into crm_smm_facebookpagedtl(" +
                                                          "facebookmain_gid," +
                                                         "post_id," +
                                                         "page_id," +
                                                         "post_type," +
                                                         "commentmessage_id," +
                                                         "comment_message," +
                                                         "comment_time )" +
                                                        "values(" +
                                                         " '" + msGetGid + "'," +
                                                        "'" + items.id + "'," +
                                                         "'" + objMdlFacebookMessageResponse.id + "'," +
                                                        " 'Picture', " +
                                                        "'" + items.id + "'," +
                                                        "'" + commentmessage + "'," +
                                                        "'" + items.created_time.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    if (mnResult2 == 0)

                                                    {
                                                        result.message = "Failed!";
                                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Insering Image Comments!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result.message = "Failed!";
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Insering Image Comments!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        }
                                    }
                                }

                                if (objMdlFacebookMessageResponse.videos != null)
                                {
                                    var videoslist = objMdlFacebookMessageResponse.videos.data;

                                    foreach (var item in videoslist)
                                    {
                                        string description = System.Net.WebUtility.HtmlEncode(item.description);

                                        Console.WriteLine("Original Emoji: " + item.description);
                                        Console.WriteLine("HTML Entity Code: " + description);



                                        msGetGid = objcmnfunctions.GetMasterGID("FB");
                                        msSQL = "insert into crm_smm_facebookpage(" +
                                             "facebookmain_gid," +
                                             "post_id," +
                                              "page_id," +
                                             "post_type," +
                                             "post_url," +
                                             "permalink_url," +
                                             "views_count," +
                                             "caption," +
                                             "postcreated_time )" +
                                            "values(" +
                                             " '" + msGetGid + "'," +
                                            "'" + item.id + "'," +
                                            "'" + objMdlFacebookMessageResponse.id + "'," +
                                            " 'Videos', " +
                                            "'" + item.source + "'," +
                                            "'" + item.permalink_url + "'," +
                                            "'" + item.views + "'," +
                                            "'" + description + "'," +
                                            "'" + item.created_time.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            if (item.comments != null)
                                            {

                                                foreach (var items in item.comments.data)
                                                {
                                                    string comment_message = System.Net.WebUtility.HtmlEncode(items.message);

                                                    Console.WriteLine("Original Emoji: " + items.message);
                                                    Console.WriteLine("HTML Entity Code: " + comment_message);

                                                    msSQL = "insert into crm_smm_facebookpagedtl(" +
                                                        "facebookmain_gid," +
                                                        "post_id," +
                                                         "post_type," +
                                                         "page_id," +
                                                         "commentmessage_id," +
                                                         "comment_message," +
                                                         "comment_time )" +
                                                        "values(" +
                                                         " '" + msGetGid + "'," +
                                                        "'" + items.id + "'," +
                                                        " 'Videos', " +
                                                         "'" + objMdlFacebookMessageResponse.id + "'," +
                                                        "'" + items.id + "'," +
                                                        "'" + comment_message + "'," +
                                                        "'" + items.created_time.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                    mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    if (mnResult3 == 0)
                                                    {
                                                        result.message = "Failed!";
                                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Insering Video Comments!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result.message = "Failed!";
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Insering Video Comments!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        result.message = "Failed!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with accesstoken- (DaGetpagepost) " + values.access_token + " *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }

                }
                else
                {

                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with accesstoken- (DaGetpagepost) " + values.access_token + " *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting page details(DaGetPagedetails)";
                objcmnfunctions.LogForAudit(ex.ToString(), "SocialMedia/ErrorLog/Facebook/" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public void DaPostsummarydetails(string facebook_page_id, MdlFacebook values)
        {

            try
            {
                msSQL = "select a.facebookmain_gid,a.post_id, a.post_url, DATE_FORMAT( a.postcreated_time, '%d-%m-%Y') AS postcreated_time,a.post_type,a.caption,a.views_count , ifnull(count(b.comment_message),0) as comment_message,c.user_name " +
                        "  from  crm_smm_facebookpage a left join crm_smm_facebookpagedtl b on b.facebookmain_gid=a.facebookmain_gid  left join crm_smm_facebookdetails c on c.id=a.page_id  where a.page_id='" + facebook_page_id + "' group by a.post_id order by a.postcreated_time desc;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<postsummary_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new postsummary_List
                        {
                            facebookmain_gid = dt["facebookmain_gid"].ToString(),
                            post_url = dt["post_url"].ToString(),
                            postcreated_time = dt["postcreated_time"].ToString(),
                            post_type = dt["post_type"].ToString(),
                            caption = dt["caption"].ToString(),
                            views_count = dt["views_count"].ToString(),
                            comment_message = dt["comment_message"].ToString(),
                            post_id = dt["post_id"].ToString(),
                            user_name = dt["user_name"].ToString()
                        });
                        values.postsummary_List = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Loading Record !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with Loading Record- (DaPostsummarydetails)  *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetViewpostcomment(string facebookmain_gid, mdlFbPostView values)
        {
            try
            {

                int count = 0;
                msSQL = " select post_url,post_type,post_id,facebookmain_gid,caption,postcreated_time from crm_smm_facebookpage" +
                   "  where facebookmain_gid='" + facebookmain_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.post_url = objOdbcDataReader["post_url"].ToString();
                    values.post_type = objOdbcDataReader["post_type"].ToString();

                    values.post_id = objOdbcDataReader["post_id"].ToString();
                    values.facebookmain_gid = objOdbcDataReader["facebookmain_gid"].ToString();
                    values.caption = objOdbcDataReader["caption"].ToString();
                    values.postcreated_time = objOdbcDataReader["postcreated_time"].ToString();

                }
                msSQL = " SELECT b.commentmessage_id,b.comment_message,b.comment_time FROM crm_smm_facebookpagedtl b" +
                        " where b.facebookmain_gid='" + facebookmain_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<postcomment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new postcomment_list
                        {
                            comment_time = dt["comment_time"].ToString(),
                            commentmessage_id = dt["commentmessage_id"].ToString(),
                            comment_message = dt["comment_message"].ToString(),
                        });
                        values.postcomment_list = getModuleList;
                        count++;
                    }
                }
                dt_datatable.Dispose();
                values.comments_count = count;
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting view Comment";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUploadImage(HttpRequest httpRequest, string user_gid, result objResult)
        {

            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            HttpPostedFile httpPostedFile;
            string image_caption = httpRequest.Form[0];
            string page_id = httpRequest.Form[1];

            MemoryStream ms = new MemoryStream();
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

                        msSQL = " select page_id,access_token from crm_smm_tfacebookservice where page_id= '" + page_id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsaccess_token = objOdbcDataReader["access_token"].ToString();

                        }
                        if (lsaccess_token != null)
                        {
                            if (FileExtension == ".jpg" | FileExtension == ".png" | FileExtension == ".jpeg" | FileExtension == ".gif" | FileExtension == ".JPG" | FileExtension == ".JPEG" | FileExtension == ".JIFF" | FileExtension == ".TIFF" | FileExtension == ".PNG" | FileExtension == ".GIF" | FileExtension == ".JFIF" | FileExtension == ".svg" | FileExtension == ".jfif")
                            {


                                bool status1;
                                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                    "Facebook/" + msdocument_gid + FileExtension,
                                    FileExtension, ms);
                                ms.Close();
                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Facebook/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + page_id + "/photos", Method.POST);
                                request.AlwaysMultipartFormData = true;
                                request.AddParameter("access_token", lsaccess_token);
                                string filePath = ConfigurationManager.AppSettings["blob_imagepath1"]+
                                                                final_path +msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
                                request.AddParameter("url", filePath);
                                // Add other parameters if needed
                                request.AddParameter("message", image_caption);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    objResult.status = true;
                                    objResult.message = "Posted in Facebook Successfully !!";
                                }
                                else if (response.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    objResult.status = false;
                                    objResult.message = "Error While Posting in Facebook !!";

                                }
                            }
                            else if (FileExtension == ".mp4" | FileExtension == ".MP4" | FileExtension == ".avi" | FileExtension == ".mkv" | FileExtension == ".wmv" | FileExtension == ".mov" | FileExtension == ".WebM" | FileExtension == ".flv" | FileExtension == ".hevc" | FileExtension == ".vpg")

                            {
                                bool status1;
                                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                     "Facebook/" + msdocument_gid + FileExtension,
                                     FileExtension, ms);
                                ms.Close();

                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Facebook/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + page_id + "/videos", Method.POST);
                                request.AlwaysMultipartFormData = true;
                                request.AddParameter("access_token", lsaccess_token);
                                string filePath =ConfigurationManager.AppSettings["blob_imagepath1"]+
                                                                final_path+ msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
                                string finalpath = filePath;
                                request.AddParameter("file_url", filePath);
                                request.AddParameter("message", image_caption);
                                IRestResponse response = client.Execute(request);

                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    objResult.status = true;
                                    objResult.message = "Posted in Facebook Successfully !!";
                                }
                                else
                                {
                                    objResult.status = false;
                                    objResult.message = "Error While Posting in Facebook !!";
                                }

                            }

                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Check the Access Token !!";
                        }
                    }

                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Posting in Facebook !!";
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Uploading";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured While uploading- (DaUploadImage)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaScheduleUploadImage(HttpRequest httpRequest, string user_gid, result objResult)
        {
            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            HttpPostedFile httpPostedFile;
            string image_schedulercaption = httpRequest.Form[0];
            string schedule_at = httpRequest.Form[1];
            string page_id = httpRequest.Form[2];

            // Split the input string by space to separate date and time
            string[] parts = schedule_at.Split(' ');

            // Extract date and time parts
            string datePart = parts[0];
            string timePart = parts[1];

            // Concatenate date and time parts into a format that can be parsed
            string dateTimeString = $"{datePart} {timePart}";

            // Parse the concatenated string to a DateTime object
            DateTime dateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", null);

            // Convert the DateTime object to Unix timestamp
            long unixTimestamp = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();

            MemoryStream ms = new MemoryStream();
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
                        msSQL = " select page_id,access_token from crm_smm_tfacebookservice where page_id= '" + page_id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsaccess_token = objOdbcDataReader["access_token"].ToString();

                        }
                        if (lsaccess_token != null)
                        {

                            if (FileExtension == ".jpg" | FileExtension == ".png" | FileExtension == ".jpeg" | FileExtension == ".gif" | FileExtension == ".JPG" | FileExtension == ".JPEG" | FileExtension == ".JIFF" | FileExtension == ".TIFF" | FileExtension == ".PNG" | FileExtension == ".GIF" | FileExtension == ".JFIF" | FileExtension == ".svg" | FileExtension == ".jfif")
                            {


                                bool status1;
                                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                    "Facebook/" + msdocument_gid + FileExtension,
                                    FileExtension, ms);
                                ms.Close();
                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Facebook/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/v19.0/" + page_id + "/photos?access_token=" + lsaccess_token, Method.POST);
                                request.AddHeader("Content-Type", "application/json");
                                string filePath = ConfigurationManager.AppSettings["blob_imagepath1"] +
                                                                final_path+ msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
                                //string bodycontent = "{\"url\":\"" + filePath + "\"," + "\"message\":\"" + image_schedulercaption + "\"," + "\"published\":\"false\"," + "\"scheduled_publish_time\":\"" + unixTimestamp + "\"}";

                                MdlSchedulePost obj = new MdlSchedulePost();
                                obj.url = filePath;
                                obj.message = image_schedulercaption;
                                obj.published = "false";
                                obj.scheduled_publish_time = unixTimestamp.ToString();
                                string bodycontent = JsonConvert.SerializeObject(obj);


                                request.AddParameter("application/json", bodycontent, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                string errornetsuiteJSON = response.Content;
                                schedulelist objMdlFacebookMessageResponse = new schedulelist();
                                try
                                {
                                    objMdlFacebookMessageResponse = JsonConvert.DeserializeObject<schedulelist>(errornetsuiteJSON);
                                }
                                catch (JsonException)
                                {

                                }

                                string errorMessage = null;
                                if (!string.IsNullOrEmpty(errornetsuiteJSON))
                                {
                                    JObject errorObject = JObject.Parse(errornetsuiteJSON);
                                    if (errorObject["error"] != null && errorObject["error"]["message"] != null)
                                    {
                                        string fullErrorMessage = (string)errorObject["error"]["message"];
                                        errorMessage = fullErrorMessage.Substring(6);
                                    }
                                }

                                if (response.StatusCode == HttpStatusCode.OK)
                                {

                                    msSQL = " insert into crm_smm_tfacebookschedule(" +
                                            " post_id," +
                                            " post_type," +
                                            " page_id," +
                                            " published," +
                                            " post_url," +
                                            " caption," +
                                            " file_name," +
                                            " schedule_at," +
                                            " postcreated_time)" +
                                              " values(" +
                                               " '" + objMdlFacebookMessageResponse.id + "'," +
                                               " 'Picture'," +
                                               " '" + page_id + "'," +
                                               " 'false'," +
                                               " '" + filePath + "'," +
                                               " '" + image_schedulercaption + "'," +
                                               " '" + httpPostedFile.FileName + "'," +
                                               "'" + dateTimeString + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        objResult.status = true;
                                        objResult.message = "Posted in Facebook Successfully !!";
                                    }
                                    else
                                    {
                                        objResult.status = false;
                                        objResult.message = "Error While Posting in Facebook !!";
                                    }

                                }
                                else
                                {
                                    objResult.status = false;
                                    objResult.message = errorMessage;

                                }
                            }
                            else if (FileExtension == ".mp4" | FileExtension == ".MP4" | FileExtension == ".avi" | FileExtension == ".mkv" | FileExtension == ".wmv" | FileExtension == ".mov" | FileExtension == ".WebM" | FileExtension == ".flv" | FileExtension == ".hevc" | FileExtension == ".vpg")

                            {
                                bool status1;
                                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                     "Facebook/" + msdocument_gid + FileExtension,
                                     FileExtension, ms);
                                ms.Close();

                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Facebook/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/v19.0/" + page_id + "/videos?access_token=" + lsaccess_token, Method.POST);
                                request.AddHeader("Content-Type", "application/json");
                                string filePath = ConfigurationManager.AppSettings["blob_imagepath1"]+
                                                                final_path+ msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
                                //string bodycontent = "{\"file_url\":\"" + filePath + "\"," + "\"message\":\"" + image_schedulercaption + "\"," + "\"published\":\"false\"," + "\"scheduled_publish_time\":\"" + unixTimestamp + "\"}";
                                MdlSchedulePost obj = new MdlSchedulePost();
                                obj.url = filePath;
                                obj.message = image_schedulercaption;
                                obj.published = "false";
                                obj.scheduled_publish_time = unixTimestamp.ToString();
                                string bodycontent = JsonConvert.SerializeObject(obj);
                                request.AddParameter("application/json", bodycontent, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                string errornetsuiteJSON = response.Content;
                                schedulelist objMdlFacebookMessageResponse = new schedulelist();
                                try
                                {
                                    objMdlFacebookMessageResponse = JsonConvert.DeserializeObject<schedulelist>(errornetsuiteJSON);
                                }
                                catch (JsonException)
                                {

                                }

                                string errorMessage = null;
                                if (!string.IsNullOrEmpty(errornetsuiteJSON))
                                {
                                    JObject errorObject = JObject.Parse(errornetsuiteJSON);
                                    if (errorObject["error"] != null && errorObject["error"]["message"] != null)
                                    {
                                        string fullErrorMessage = (string)errorObject["error"]["message"];
                                        errorMessage = fullErrorMessage.Substring(6);
                                    }
                                }

                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    msSQL = " insert into crm_smm_tfacebookschedule(" +
                                            " post_id," +
                                            " post_type," +
                                            " page_id," +
                                            " published," +
                                            " post_url," +
                                            " caption," +
                                           " file_name," +
                                            " schedule_at," +
                                            " postcreated_time)" +
                                              " values(" +
                                               " '" + objMdlFacebookMessageResponse.id + "'," +
                                               " 'Videos'," +
                                              " '" + page_id + "'," +
                                               " 'false'," +
                                              " '" + filePath + "'," +
                                              " '" + image_schedulercaption + "'," +
                                              " '" + httpPostedFile.FileName + "'," +
                                              "'" + dateTimeString + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        objResult.status = true;
                                        objResult.message = "Posted in Facebook Successfully !!";
                                    }
                                    else
                                    {
                                        objResult.status = false;
                                        objResult.message = "Error While Posting in Facebook !!";
                                    }
                                }
                                else
                                {
                                    objResult.status = false;
                                    objResult.message = errorMessage;

                                }

                            }

                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Check the Access Token !!";
                        }
                    }

                }

                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Posting in Facebook !!";
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Scheduling !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured While uploading- (DaScheduleUploadImage)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaGetschedulelogdetails(MdlFacebook values, string page_id)
        {
            try
            {
                msSQL = " select page_id,access_token from crm_smm_tfacebookservice where page_id= '" + page_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsaccess_token = objOdbcDataReader["access_token"].ToString();

                }
                if (lsaccess_token != null)
                {
                    result result = new result();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    string requestAddressURL = "https://graph.facebook.com/v18.0/me?fields=id%2Cname%2Ccategory%2Cfollowers_count%2Clink%2Cpicture%2Cvideos%7Bpermalink_url%2Csource%2Cid%2Cviews%2Cdescription%2Ccreated_time%2Ccomments%7Bid%2Ccreated_time%2Cmessage%7D%7D%2Cposts%7Bfull_picture%2Cpermalink_url%2Cmessage%2Ccreated_time%2Ccomments%7Bid%2Cmessage%2Ccreated_time%7D%7D&access_token=" + lsaccess_token + "";
                    var clientAddress = new RestClient(requestAddressURL);
                    var requestAddress = new RestRequest(Method.GET);
                    IRestResponse responseAddress = clientAddress.Execute(requestAddress);
                    string address_erpid = responseAddress.Content;
                    string errornetsuiteJSON = responseAddress.Content;
                    facebooklist objMdlFacebookMessageResponse = new facebooklist();
                    objMdlFacebookMessageResponse = JsonConvert.DeserializeObject<facebooklist>(errornetsuiteJSON);


                    if (responseAddress.StatusCode == HttpStatusCode.OK)
                    {
                        if (objMdlFacebookMessageResponse.posts != null)
                        {
                            var imagelist = objMdlFacebookMessageResponse.posts.data;
                            foreach (var item in imagelist)
                            {
                                string[] parts = item.id.Split('_');
                                string postIdSuffix = parts[1];
                                msSQL = "select post_id from  crm_smm_tfacebookschedule where posted_flag = 'N' and  post_id ='" + postIdSuffix + "' ";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true || objOdbcDataReader.HasRows)
                                {
                                    msSQL = "UPDATE crm_smm_tfacebookschedule SET posted_flag = 'Y' WHERE post_id ='" + postIdSuffix + "' ";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        result.status = true;
                                        result.message = "Updated!";

                                    }
                                    else
                                    {
                                        result.message = "Failed!";
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while editing details!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        objcmnfunctions.LogForAudit("");
                                    }
                                }
                            }
                        }

                        if (objMdlFacebookMessageResponse.videos != null)
                        {
                            var videoslist = objMdlFacebookMessageResponse.videos.data;

                            foreach (var item in videoslist)
                            {
                                msSQL = "select post_id from  crm_smm_tfacebookschedule where posted_flag = 'N' and  post_id ='" + item.id + "' ";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true || objOdbcDataReader.HasRows)
                                {
                                    msSQL = "UPDATE crm_smm_tfacebookschedule SET posted_flag = 'Y' WHERE  post_id ='" + item.id + "' ";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        result.status = true;
                                        result.message = "Updated!";
                                    }
                                    else
                                    {
                                        result.message = "Failed!";
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while editing details!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        objcmnfunctions.LogForAudit("");
                                    }
                                }
                            }

                        }
                    }
                }
                msSQL = " select post_id,post_type,post_url,caption,schedule_at,postcreated_time,file_name from crm_smm_tfacebookschedule where posted_flag !='Y' and page_id='"+ page_id + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<schedulelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new schedulelist
                        {
                            post_id = dt["post_id"].ToString(),
                            post_type = dt["post_type"].ToString(),
                            post_url = dt["post_url"].ToString(),
                            caption = dt["caption"].ToString(),
                            schedule_at = dt["schedule_at"].ToString(),
                            postcreated_time = dt["postcreated_time"].ToString(),
                            file_name = dt["file_name"].ToString()

                        });
                        values.schedulelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting facebook user detail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaGetpagelist(MdlFacebook values)
        {
            try
            {
                msSQL = " select a.id,a.user_name from crm_smm_facebookdetails a left join crm_smm_tfacebookservice b on b.page_id=a.id  where  b.page_id=a.id; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList1 = new List<facebookpage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new facebookpage_list
                        {
                            page_id = dt["id"].ToString(),
                            page_name = dt["user_name"].ToString(),
                            platform = "Facebook",
                        }); ;
                        values.facebookpage_list = getModuleList1;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting facebook user detail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaMultiplepagepost(HttpRequest httpRequest, string user_gid, result objResult)
        {   

            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            HttpPostedFile httpPostedFile;
            string image_caption = httpRequest.Form[0];
            string page_id = httpRequest.Form[1];
            MemoryStream ms = new MemoryStream();
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

        string[] pageIds = page_id.Split(',');
                        bool status1;
                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                            "Facebook/" + msdocument_gid + FileExtension,
                            FileExtension, ms);
                        ms.Close();
                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Facebook/";
                        string filePath = Path.Combine(ConfigurationManager.AppSettings["blob_imagepath1"],
                                                   final_path, msdocument_gid + FileExtension +
                                                   ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                   ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                   ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                   ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                   ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                   ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                   ConfigurationManager.AppSettings["blob_imagepath8"]);
                        foreach (string id in pageIds)
        {
            msSQL = "SELECT page_id, access_token FROM crm_smm_tfacebookservice WHERE page_id = '" + id + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                lsaccess_token = objOdbcDataReader["access_token"].ToString();
            }

            if (lsaccess_token != null)
            {
                if (FileExtension == ".jpg" || FileExtension == ".png" || FileExtension == ".jpeg" || FileExtension == ".gif" || FileExtension == ".JPG" || FileExtension == ".JPEG" || FileExtension == ".JIFF" || FileExtension == ".TIFF" || FileExtension == ".PNG" || FileExtension == ".GIF" || FileExtension == ".JFIF" || FileExtension == ".svg" || FileExtension == ".jfif")
                {
                   
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient("https://graph.facebook.com");
                    var request = new RestRequest("/" + id + "/photos", Method.POST);
                    request.AlwaysMultipartFormData = true;
                    request.AddParameter("access_token", lsaccess_token);
                   
                    request.AddParameter("url", filePath);
                    // Add other parameters if needed
                    request.AddParameter("message", image_caption);
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objResult.status = true;
                        objResult.message = "Posted in Facebook Successfully !!";
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        objResult.status = false;
                        objResult.message = "Error While Posting in Facebook !!";
                    }
                }
                else if (FileExtension == ".mp4" || FileExtension == ".MP4" || FileExtension == ".avi" || FileExtension == ".mkv" || FileExtension == ".wmv" || FileExtension == ".mov" || FileExtension == ".WebM" || FileExtension == ".flv" || FileExtension == ".hevc" || FileExtension == ".vpg")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient("https://graph.facebook.com");
                    var request = new RestRequest("/" + id + "/videos", Method.POST);
                    request.AlwaysMultipartFormData = true;
                    request.AddParameter("access_token", lsaccess_token);
                    request.AddParameter("file_url", filePath);
                    request.AddParameter("message", image_caption);
                    IRestResponse response = client.Execute(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objResult.status = true;
                        objResult.message = "Posted in Facebook Successfully !!";
                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Error While Posting in Facebook !!";
                    }
                }
            }
            else
            {
                objResult.status = false;
                objResult.message = "Check the Access Token !!";
            }
        }
    }
}
else
{
    objResult.status = false;
    objResult.message = "Error While Posting in Facebook !!";
}

            }
                catch (Exception ex)
                {
                    objResult.message = "Exception occured while Uploading";

                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured While uploading- (DaUploadImage)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaGetinstagramaccountlist(MdlFacebook values)
        {
            try
            {
                msSQL = " select a.account_id,a.username from crm_smm_tinstagramdetails a left join crm_smm_tinstagramservice b on b.account_id=a.account_id  where  b.account_id=a.account_id; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList1 = new List<instagramaccount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new instagramaccount_list
                        {
                            account_id = dt["account_id"].ToString(),
                            username = dt["username"].ToString(),
                            platform = "Instagram",

                        });
                        values.instagramaccount_list = getModuleList1;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting facebook user detail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostinstaplatformpost(HttpRequest httpRequest, result objResult)
        {
            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            HttpPostedFile httpPostedFile;
            string image_caption = httpRequest.Form[0];
            string instaaccount_id = httpRequest.Form[2];
            string mention = httpRequest.Form[3];
            MemoryStream ms = new MemoryStream();
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
                        bool status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                   "Instagram/" + msdocument_gid + FileExtension,
                                   FileExtension, ms);
                        ms.Close();
                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Instagram/";
                        string filePath = ConfigurationManager.AppSettings["blob_imagepath1"] +
                                                             final_path + msdocument_gid + FileExtension +
                                                             ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                             ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                             ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                             ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                             ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                             ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                             ConfigurationManager.AppSettings["blob_imagepath8"];

                        msSQL = " select access_token from crm_smm_tinstagramservice where account_id= '" + instaaccount_id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsaccess_token = objOdbcDataReader["access_token"].ToString();

                        }
                        if (lsaccess_token != null)
                        {
                            if (FileExtension == ".jpg" | FileExtension == ".png" | FileExtension == ".jpeg" | FileExtension == ".gif" | FileExtension == ".JPG" | FileExtension == ".JPEG" | FileExtension == ".JIFF" | FileExtension == ".TIFF" | FileExtension == ".PNG" | FileExtension == ".GIF" | FileExtension == ".JFIF" | FileExtension == ".svg" | FileExtension == ".jfif")
                            {
                               
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + instaaccount_id + "/media?media_type=IMAGE", Method.POST);
                                request.AddQueryParameter("access_token", lsaccess_token);
                                request.AddQueryParameter("image_url", filePath);
                                request.AddQueryParameter("caption", image_caption);
                                string[] mentions = mention.Split(',');
                                taggedUsers objtaggedUsers = new taggedUsers();
                                objtaggedUsers.user_tags = new UserTag[mentions.Length];

                                double step = 0.9 / Math.Max(mentions.Length - 1, 1); // Ensuring denominator is at least 1 to avoid division by zero
                                Console.WriteLine($"Step: {step}");

                                for (int j = 0; j < mentions.Length; j++)
                                {
                                    objtaggedUsers.user_tags[j] = new UserTag();
                                    objtaggedUsers.user_tags[j].username = mentions[j];

                                    if (mentions.Length > 1)
                                    {
                                        objtaggedUsers.user_tags[j].x = 0.1 + j * step;
                                        objtaggedUsers.user_tags[j].y = 0.1 + j * step;
                                    }
                                    else
                                    {
                                        // Set default values if there's only one mention
                                        objtaggedUsers.user_tags[j].x = 0.5;
                                        objtaggedUsers.user_tags[j].y = 0.5;
                                    }

                                    // Debugging information
                                    Console.WriteLine($"User: {objtaggedUsers.user_tags[j].username}, x: {objtaggedUsers.user_tags[j].x}, y: {objtaggedUsers.user_tags[j].y}");
                                }

                                string body = JsonConvert.SerializeObject(objtaggedUsers);
                                request.AddParameter("application/json", body, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var responseData = JsonConvert.DeserializeObject<Testas>(response.Content);
                                    string post_id = responseData.id;
                                    string message = instapostpublishfn(instaaccount_id, lsaccess_token, post_id, objResult);
                                    Console.WriteLine(message);

                                }
                                else if (response.StatusCode == HttpStatusCode.BadRequest)
                                {

                                    string content = response.Content;
                                    JObject errorJson;
                                    try
                                    {
                                        errorJson = JObject.Parse(content);
                                        if (errorJson["error"]?["message"]?.ToString() == "The aspect ratio is not supported.")
                                        {
                                            objResult.status = false;
                                            objResult.message = "Submit an image with a valid aspect ratio!!";
                                        }
                                        else
                                        {
                                            objResult.status = false;
                                            objResult.message = "Posted Sucessfully at Facebbok and Error Occured while Posting at Instagram  ";

                                        }

                                    }
                                    catch (JsonException)
                                    {
                                        objResult.status = false;
                                        objResult.message = "Error Occured while uploading!!!";
                                    }

                                }
                            }

                            else if (FileExtension == ".mp4" | FileExtension == ".MP4" | FileExtension == ".avi" | FileExtension == ".mkv" | FileExtension == ".wmv" | FileExtension == ".mov" | FileExtension == ".WebM" | FileExtension == ".flv" | FileExtension == ".hevc" | FileExtension == ".vpg")

                            {
                              
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + instaaccount_id + "/media?media_type=REELS", Method.POST);
                                request.AddQueryParameter("access_token", lsaccess_token);
                                request.AddQueryParameter("video_url", filePath);
                                request.AddQueryParameter("caption", image_caption);
                                string[] mentions = mention.Split(',');
                                taggedUsers1 objtaggedUsers = new taggedUsers1();
                                objtaggedUsers.user_tags = new UserTags[mentions.Length];

                                double step = 0.9 / Math.Max(mentions.Length - 1, 1); // Ensuring denominator is at least 1 to avoid division by zero
                                Console.WriteLine($"Step: {step}");

                                for (int j = 0; j < mentions.Length; j++)
                                {
                                    objtaggedUsers.user_tags[j] = new UserTags();
                                    objtaggedUsers.user_tags[j].username = mentions[j];



                                    // Debugging information
                                    Console.WriteLine($"User: {objtaggedUsers.user_tags[j].username}");
                                }

                                string body = JsonConvert.SerializeObject(objtaggedUsers);
                                request.AddParameter("application/json", body, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var responseData = JsonConvert.DeserializeObject<Testas>(response.Content);
                                    string post_id = responseData.id;
                                    string message = instapostpublishfn(instaaccount_id, lsaccess_token, post_id, objResult);
                                    Console.WriteLine(message);

                                }
                                else if (response.StatusCode == HttpStatusCode.BadRequest)
                                {
                                   
                                    objResult.status = false;
                                    objResult.message = "Posted Sucessfully at Facebbok and Error Occured while Posting at Instagram  ";
                                }

                            }

                            else
                            {
                                objResult.status = false;
                                objResult.message = "Check The File Format !!";
                            }

                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Check the Access Token !!";
                        }
                    }

                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Posting in Instagram !!";
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Uploading";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured While uploading- (DaPostinstaplatformpost)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public string instapostpublishfn(string instaaccount_id, string lsaccess_token, string post_id, result objResult)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://graph.facebook.com");
            var request = new RestRequest("/" + instaaccount_id + "/media_publish?", Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("access_token", lsaccess_token);
            request.AddParameter("creation_id", post_id);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                objResult.status = true;
                objResult.message = "Posted Successfully!!";
                return objResult.message;
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                var errorMessage = errorResponse?.error?.error_user_msg?.ToString();

                if (errorMessage.Contains("The media is not ready to be published. Please wait a moment."))
                {
                    return instapostpublishfn(instaaccount_id, lsaccess_token, post_id, objResult);
                }
                objResult.status = false;
                objResult.message = errorMessage;
                return objResult.message;


            }
        }


    }
}