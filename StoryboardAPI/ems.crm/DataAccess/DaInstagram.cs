using System;
using System.Collections.Generic;
using System.Net;
using ems.crm.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using RestSharp;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Http.Results;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;


/// Important Note :-- Insta Post video should take min 30 seconds  to publish function so based on respective message
/// (The media is not ready to be published. Please wait a moment.) the function call again -  pathmanaban


namespace ems.crm.DataAccess
{
    public class DaInstagram
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        string lsaccess_token, final_path, lspath;


        public void DaGetaccountdetails(MdlInstagram values)
        {
            try
            {
                msSQL = " select account_id,access_token from crm_smm_tinstagramservice";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<list_instaccesstoken>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new list_instaccesstoken
                        {
                            account_id = dt["account_id"].ToString(),
                            access_token = dt["access_token"].ToString()
                        });
                        values.list_instaccesstoken = getModuleList;
                    }
                }
                if (values.list_instaccesstoken != null)
                {
                    for (int i = 0; i < values.list_instaccesstoken.ToArray().Length; i++)
                    {
                        result result = new result();
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        string requestAddressURL = "https://graph.facebook.com/" + values.list_instaccesstoken[i].account_id + "?fields=ig_id%2Cbiography%2Cfollowers_count%2Cfollows_count%2Cmedia_count%2Cusername%2Cid&access_token=" + values.list_instaccesstoken[i].access_token + "";
                        var clientAddress = new RestClient(requestAddressURL);
                        var requestAddress = new RestRequest(Method.GET);
                        IRestResponse responseAddress = clientAddress.Execute(requestAddress);
                        string address_erpid = responseAddress.Content;
                        string errornetsuiteJSON = responseAddress.Content;
                        instagramlist objMdlInstagramMessageResponse = new instagramlist();
                        objMdlInstagramMessageResponse = JsonConvert.DeserializeObject<instagramlist>(errornetsuiteJSON);
                        if (responseAddress.StatusCode == HttpStatusCode.OK)
                        {
                            msSQL = "delete from crm_smm_tinstagramdetails where  account_id = '" + objMdlInstagramMessageResponse.id + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = "insert into crm_smm_tinstagramdetails(" +
                                                     "account_id," +
                                                     "ig_id," +
                                                     "username," +
                                                    "media_count," +
                                                    "follows_count," +
                                                   "followers_count," +
                                                     "biography)" +
                                                     "values(" +
                                                     "'" + objMdlInstagramMessageResponse.id + "'," +
                                                     "'" + objMdlInstagramMessageResponse.ig_id + "'," +
                                                    "'" + objMdlInstagramMessageResponse.username + "'," +
                                                    "'" + objMdlInstagramMessageResponse.media_count + "'," +
                                                    "'" + objMdlInstagramMessageResponse.follows_count + "'," +
                                                     "'" + objMdlInstagramMessageResponse.followers_count + "'," +
                                                     "'" + objMdlInstagramMessageResponse.biography + "' )";
                                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult1 == 0)
                                {

                                    result.message = "Failed!";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Inserting account details-(DaGetaccountdetails)!! " + objMdlInstagramMessageResponse.id + " " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                }
                            }
                        }
                        else
                        {

                            result.message = "Failed!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with accesstoken- (DaGetaccountdetails) " + values.list_instaccesstoken[i].access_token + " *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                }
                else
                {
                    values.message = "Check the Access Token !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Account details(DaGetaccountdetails)";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured getting account details-(DaGetaccountdetails)!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaGetaccountdetailsummary(MdlInstagram values)
        {

            try
            {
                msSQL = " select a.account_id,a.username,a.media_count,a.follows_count,a.followers_count,a.biography " +
                    " from crm_smm_tinstagramdetails a left join crm_smm_tinstagramservice b on b.account_id=a.account_id  where  b.account_id=a.account_id;";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList1 = new List<instagramaccount_summarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new instagramaccount_summarylist
                        {
                            account_id = dt["account_id"].ToString(),
                            username = dt["username"].ToString(),
                            media_count = dt["media_count"].ToString(),
                            follows_count = dt["follows_count"].ToString(),
                            followers_count = dt["followers_count"].ToString(),
                            biography = dt["biography"].ToString(),

                        });
                        values.instagramaccount_summarylist = getModuleList1;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Loading Acoount Deatils !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with Loading Record- (DaGetaccountdetailsummary)  *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetinstapost(MdlInstagram values, string account_id)
        {
            try
            {
                msSQL = " select access_token from crm_smm_tinstagramservice where account_id='" + account_id + "'  ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.access_token = objOdbcDataReader["access_token"].ToString();
                }
                if (values.access_token != null)
                {
                    result result = new result();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    string requestAddressURL = "https://graph.facebook.com/" + account_id + "/media?fields=caption%2Ccomments_count%2Clike_count%2Cmedia_type%2Cmedia_url%2Ccreated_time&access_token=" + values.access_token + "";
                    var clientAddress = new RestClient(requestAddressURL);
                    var requestAddress = new RestRequest(Method.GET);
                    IRestResponse responseAddress = clientAddress.Execute(requestAddress);
                    string address_erpid = responseAddress.Content;
                    string errornetsuiteJSON = responseAddress.Content;
                    instagramlist objMdlInstagramMessageResponse = new instagramlist();
                    objMdlInstagramMessageResponse = JsonConvert.DeserializeObject<instagramlist>(errornetsuiteJSON);
                    if (responseAddress.StatusCode == HttpStatusCode.OK)
                    {
                        msSQL = "delete from crm_smm_instagramaccount where  account_id = '" + account_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            if (objMdlInstagramMessageResponse.data != null)
                            {
                                foreach (var item in objMdlInstagramMessageResponse.data)
                                {
                                    string caption = System.Net.WebUtility.HtmlEncode(item.caption);
                                    Console.WriteLine("Original Emoji: " + item.caption);
                                    Console.WriteLine("HTML Entity Code: " + caption);
                                    msSQL = "insert into crm_smm_instagramaccount(" +
                                          "account_id," +
                                         "post_id," +
                                         "caption," +
                                         "post_type," +
                                         "post_url," +
                                         "like_count," +
                                         "comments_count )" +
                                        "values(" +
                                        "'" + account_id + "'," +
                                       "'" + item.id + "'," +
                                       "'" + caption + "'," +
                                        "'" + item.media_type + "'," +
                                        "'" + item.media_url + "'," +
                                        "'" + item.like_count + "'," +
                                        "'" + item.comments_count + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {
                                        result.message = "Failed!";
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Insering Image Comments!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        result.message = "Failed!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with accesstoken- (DaGetinstapost) " + values.access_token + " *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }

                }
                else
                {
                    values.message = "Check the Access Token !!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Post details(DaGetinstapost)";
                objcmnfunctions.LogForAudit(ex.ToString(), "SocialMedia/ErrorLog/Instagram/" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public void DaGetinstapostsummary(string account_id, MdlInstagram values)
        {

            try
            {
                msSQL = "select b.username,a.post_id,a.caption,a.post_type,a.post_url,a.like_count,a.comments_count from crm_smm_instagramaccount a" +
                    " left join crm_smm_tinstagramdetails b on b.account_id=a.account_id where a.account_id=" + account_id + " ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<instapostsummary_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new instapostsummary_List
                        {
                            post_id = dt["post_id"].ToString(),
                            caption = dt["caption"].ToString(),
                            post_type = dt["post_type"].ToString(),
                            post_url = dt["post_url"].ToString(),
                            like_count = dt["like_count"].ToString(),
                            comments_count = dt["comments_count"].ToString(),
                            username = dt["username"].ToString()
                        });
                        values.instapostsummary_List = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Loading Record !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with Loading Record- (DaGetinstapostsummary)  *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetcomments(MdlInstagram values, string account_id)
        {
            try
            {
                msSQL = " select access_token from crm_smm_tinstagramservice where account_id='" + account_id + "'  ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.access_token = objOdbcDataReader["access_token"].ToString();
                }
                if (values.access_token != null)
                {
                    msSQL = "select post_id from crm_smm_instagramaccount where account_id='" + account_id + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<instagramcommentlist>();
                    if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new instagramcommentlist
                            {
                                post_id = dt["post_id"].ToString()
                            });
                            values.instagramcommentlist = getModuleList;
                        }
                    }
                    if (values.instagramcommentlist != null)
                    {
                        for (int i = 0; i < values.instagramcommentlist.ToArray().Length; i++)
                        {
                            result result = new result();
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            string requestAddressURL = "https://graph.facebook.com/" + values.instagramcommentlist[i].post_id + "/comments?fields=from%2Chidden%2Cid%2Clike_count%2Cmedia%2Cparent_id%2Creplies%2Ctext%2Ctimestamp%2Cuser%2Cusername&access_token=" + values.access_token + "";
                            var clientAddress = new RestClient(requestAddressURL);
                            var requestAddress = new RestRequest(Method.GET);
                            IRestResponse responseAddress = clientAddress.Execute(requestAddress);
                            string address_erpid = responseAddress.Content;
                            string errornetsuiteJSON = responseAddress.Content;
                            instagramcommentlist objMdlInstagramMessageResponse = new instagramcommentlist();
                            objMdlInstagramMessageResponse = JsonConvert.DeserializeObject<instagramcommentlist>(errornetsuiteJSON);
                            if (responseAddress.StatusCode == HttpStatusCode.OK)
                            {
                                msSQL = "delete from crm_smm_instagramaccountdtl where  post_id = '" + values.instagramcommentlist[i].post_id + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    if (objMdlInstagramMessageResponse.data != null)
                                    {
                                        foreach (var item in objMdlInstagramMessageResponse.data)
                                        {
                                            string text = System.Net.WebUtility.HtmlEncode(item.text);
                                            Console.WriteLine("Original Emoji: " + item.text);
                                            Console.WriteLine("HTML Entity Code: " + text);
                                            msSQL = "insert into crm_smm_instagramaccountdtl(" +
                                                  "account_id," +
                                                 "comment_id," +
                                                 "post_id," +
                                                 "user_id," +
                                                 "user_name," +
                                                 "hidden," +
                                                 "commentlike_count," +
                                                 "comment_message," +
                                                 "comment_time )" +
                                                "values(" +
                                                "'" + account_id + "'," +
                                               "'" + item.id + "'," +
                                               "'" + values.instagramcommentlist[i].post_id + "'," +
                                                "'" + item.from.id + "'," +
                                                "'" + item.from.username + "'," +
                                                "'" + item.hidden + "'," +
                                                "'" + item.like_count + "'," +
                                                "'" + text + "'," +
                                                "'" + item.timestamp.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult == 0)
                                            {
                                                result.message = "Failed!";
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Insering  Comments(DaGetcomments)!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                            }

                                        }
                                    }
                                }
                            }
                            else
                            {
                                result.message = "Failed!";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with accesstoken- (DaGetcomments) " + values.access_token + " *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }

                        }
                    }
                }
                else
                {
                    values.message = "Check the Access Token !!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Comments details(DaGetcomments)";
                objcmnfunctions.LogForAudit(ex.ToString(), "SocialMedia/ErrorLog/Instagram/" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public void DaGetinstacomments(string post_id, mdlcomment values)
        {

            try
            {
                msSQL = " select a.caption,a.post_type,a.post_url,a.like_count,a.comments_count,b.username from crm_smm_instagramaccount a left join crm_smm_tinstagramdetails b on b.account_id =a.account_id where post_id='" + post_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.caption = objOdbcDataReader["caption"].ToString();
                    values.post_type = objOdbcDataReader["post_type"].ToString();
                    values.post_url = objOdbcDataReader["post_url"].ToString();
                    values.like_count = objOdbcDataReader["like_count"].ToString();
                    values.comments_count = objOdbcDataReader["comments_count"].ToString();
                    values.username = objOdbcDataReader["username"].ToString();
                }

                msSQL = "select a.user_name,a.commentlike_count,a.comment_message,a.comment_time from crm_smm_instagramaccountdtl a where post_id='" + post_id + "' order by comment_time desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<instacomment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        DateTime commentTime = Convert.ToDateTime(dt["comment_time"]);
                        int daysDifference = (int)(DateTime.Now - commentTime).TotalDays;
                        string commentTimeString = daysDifference == 0 ? commentTime.ToString("h:mm tt") : daysDifference == 1 ? "1 d" : daysDifference + " d";

                        getModuleList.Add(new instacomment_list
                        {
                            user_name = dt["user_name"].ToString(),
                            commentlike_count = dt["commentlike_count"].ToString(),
                            comment_message = dt["comment_message"].ToString(),
                            comment_days = commentTimeString,
                            comment_time = dt["comment_time"].ToString()
                        }); ;
                    }

                    values.instacomment_list = getModuleList;
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Loading Record !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with Loading Record- (DaGetinstapostsummary)  *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaGetviewinsights(MdlInstagram values, string account_id)
        {
            try
            {
                msSQL = " select access_token from crm_smm_tinstagramservice where account_id='" + account_id + "'  ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.access_token = objOdbcDataReader["access_token"].ToString();
                }
                if (values.access_token != null)
                {
                    msSQL = "select post_id,post_type from crm_smm_instagramaccount where account_id='" + account_id + "' and post_type IN ('IMAGE', 'VIDEO') ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<instagramcommentlist>();
                    if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new instagramcommentlist
                            {
                                post_id = dt["post_id"].ToString(),
                                post_type = dt["post_type"].ToString(),

                            });
                            values.instagramcommentlist = getModuleList;
                        }
                    }
                    if (values.instagramcommentlist != null)
                    {
                        for (int i = 0; i < values.instagramcommentlist.ToArray().Length; i++)
                        {
                            if (values.instagramcommentlist[i].post_type == "IMAGE")
                            {
                                result result = new result();
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                string requestAddressURL = "https://graph.facebook.com/" + values.instagramcommentlist[i].post_id + "/insights?metric=comments,likes,shares,saved,engagement,reach,impressions&period=lifetime&access_token=" + values.access_token + "";
                                var clientAddress = new RestClient(requestAddressURL);
                                var requestAddress = new RestRequest(Method.GET);
                                IRestResponse responseAddress = clientAddress.Execute(requestAddress);
                                string address_erpid = responseAddress.Content;
                                string errornetsuiteJSON = responseAddress.Content;
                                instaview_insights objMdlInstagramMessageResponse = new instaview_insights();
                                objMdlInstagramMessageResponse = JsonConvert.DeserializeObject<instaview_insights>(errornetsuiteJSON);
                                if (responseAddress.StatusCode == HttpStatusCode.OK)
                                {
                                    msSQL = "delete from crm_smm_instagraminsightsdtl where  post_id = '" + values.instagramcommentlist[i].post_id + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        if (objMdlInstagramMessageResponse.data != null)
                                        {
                                            int engagement = 0, comments = 0, likes = 0, shares = 0, saved = 0, reach = 0, impressions = 0;

                                            foreach (var insight in objMdlInstagramMessageResponse.data)
                                            {
                                                switch (insight.name)

                                                {
                                                    case "engagement":
                                                        engagement = insight.values[0].value;
                                                        break;
                                                    case "comments":
                                                        comments = insight.values[0].value;
                                                        break;
                                                    case "likes":
                                                        likes = insight.values[0].value;
                                                        break;
                                                    case "shares":
                                                        shares = insight.values[0].value;
                                                        break;
                                                    case "saved":
                                                        saved = insight.values[0].value;
                                                        break;
                                                    case "reach":
                                                        reach = insight.values[0].value;
                                                        break;
                                                    case "impressions":
                                                        impressions = insight.values[0].value;
                                                        break;
                                                    default:
                                                        // Handle unknown insight name
                                                        break;
                                                }

                                            }
                                            msSQL = "INSERT INTO crm_smm_instagraminsightsdtl (" +
                                                     "account_id, post_id,post_type,engagement, comments, likes, shares, saved, reach, impressions" +
                                                     ") VALUES (" +
                                                     "'" + account_id + "'," +
                                                     "'" + values.instagramcommentlist[i].post_id + "'," +
                                                     "'" + values.instagramcommentlist[i].post_type + "'," +
                                                     engagement + "," +
                                                     comments + "," +
                                                     likes + "," +
                                                     shares + "," +
                                                     saved + "," +
                                                     reach + "," +
                                                     impressions +
                                                     ")";

                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult == 0)
                                            {
                                                result.message = "Failed!";
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Insering  Comments(DaGetcomments)!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    result.message = "Failed!";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with accesstoken- (DaGetcomments) " + values.access_token + " *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                }
                            }

                            else if (values.instagramcommentlist[i].post_type == "VIDEO")
                            {

                                result result = new result();
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                string requestAddressURL = "https://graph.facebook.com/" + values.instagramcommentlist[i].post_id + "/insights?metric=comments,likes,shares,saved,reach&period=lifetime&access_token=" + values.access_token + "";
                                var clientAddress = new RestClient(requestAddressURL);
                                var requestAddress = new RestRequest(Method.GET);
                                IRestResponse responseAddress = clientAddress.Execute(requestAddress);
                                string address_erpid = responseAddress.Content;
                                string errornetsuiteJSON = responseAddress.Content;
                                instaview_insights objMdlInstagramMessageResponse = new instaview_insights();
                                objMdlInstagramMessageResponse = JsonConvert.DeserializeObject<instaview_insights>(errornetsuiteJSON);
                                if (responseAddress.StatusCode == HttpStatusCode.OK)
                                {
                                    msSQL = "delete from crm_smm_instagraminsightsdtl where  post_id = '" + values.instagramcommentlist[i].post_id + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        if (objMdlInstagramMessageResponse.data != null)
                                        {
                                            int comments = 0, likes = 0, shares = 0, saved = 0, reach = 0;

                                            foreach (var insight in objMdlInstagramMessageResponse.data)
                                            {
                                                switch (insight.name)

                                                {
                                                    case "comments":
                                                        comments = insight.values[0].value;
                                                        break;
                                                    case "likes":
                                                        likes = insight.values[0].value;
                                                        break;
                                                    case "shares":
                                                        shares = insight.values[0].value;
                                                        break;
                                                    case "saved":
                                                        saved = insight.values[0].value;
                                                        break;
                                                    case "reach":
                                                        reach = insight.values[0].value;
                                                        break;

                                                }

                                            }
                                            msSQL = "INSERT INTO crm_smm_instagraminsightsdtl (" +
                                                     "account_id, post_id,post_type, comments, likes, shares, saved, reach" +
                                                     ") VALUES (" +
                                                     "'" + account_id + "'," +
                                                     "'" + values.instagramcommentlist[i].post_id + "'," +
                                                     "'" + values.instagramcommentlist[i].post_type + "'," +
                                                     comments + "," +
                                                     likes + "," +
                                                     shares + "," +
                                                     saved + "," +
                                                     reach + ")";


                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult == 0)
                                            {
                                                result.message = "Failed!";
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Insering  Comments(DaGetcomments)!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    result.message = "Failed!";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with accesstoken- (DaGetcomments) " + values.access_token + " *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                }
                            }

                        }

                    }
                }
                else
                {
                    values.message = "Check the Access Token !!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Comments details(DaGetcomments)";
                objcmnfunctions.LogForAudit(ex.ToString(), "SocialMedia/ErrorLog/Instagram/" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaGetinsights(mdlcomment values, string post_id)
        {

            try
            {
                msSQL = "select a.post_type,a.post_url,b.media_count,b.follows_count,b.followers_count from crm_smm_instagramaccount a left join crm_smm_tinstagramdetails b on b.account_id =a.account_id where post_id='" + post_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.post_type = objOdbcDataReader["post_type"].ToString();
                    values.post_url = objOdbcDataReader["post_url"].ToString();
                    values.media_count = objOdbcDataReader["media_count"].ToString();
                    values.follows_count = objOdbcDataReader["follows_count"].ToString();
                    values.followers_count = objOdbcDataReader["followers_count"].ToString();
                }

                msSQL = "select a.post_type,a.engagement,a.comments,likes,shares,saved,reach,impressions, CASE WHEN a.post_type = 'IMAGE' THEN a.engagement ELSE a.likes + a.comments + a.shares + a.saved END AS engagement  from crm_smm_instagraminsightsdtl a where post_id='" + post_id + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<instainsights_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new instainsights_list
                        {
                            post_type = dt["post_type"].ToString(),
                            engagement = dt["engagement"].ToString(),
                            comments = dt["comments"].ToString(),
                            likes = dt["likes"].ToString(),
                            shares = dt["shares"].ToString(),
                            saved = dt["saved"].ToString(),
                            reach = dt["reach"].ToString(),
                            impressions = dt["impressions"].ToString(),
                        }); ;
                    }
                    values.instainsights_list = getModuleList;
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Loading Record !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured with Loading Record- (DaGetinstapostsummary)  *******" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaPostInstaimage(HttpRequest httpRequest, result objResult)
        {
            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            HttpPostedFile httpPostedFile;
            string image_caption = httpRequest.Form[0];
            string account_id = httpRequest.Form[1];
            string mention = httpRequest.Form[2];
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

                        msSQL = " select access_token from crm_smm_tinstagramservice where account_id= '" + account_id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsaccess_token = objOdbcDataReader["access_token"].ToString();

                        }
                        if (lsaccess_token != null)
                        {
                            if (FileExtension == ".jpg" | FileExtension == ".png" | FileExtension == ".jpeg" | FileExtension == ".gif" | FileExtension == ".JPG" | FileExtension == ".JPEG" | FileExtension == ".JIFF" | FileExtension == ".TIFF" | FileExtension == ".PNG" | FileExtension == ".GIF" | FileExtension == ".JFIF" | FileExtension == ".svg" | FileExtension == ".jfif")
                            {
                                bool status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                    "Instagram/" + msdocument_gid + FileExtension,
                                    FileExtension, ms);
                                ms.Close();
                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Instagram/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + account_id + "/media?media_type=IMAGE", Method.POST);
                                request.AddQueryParameter("access_token", lsaccess_token);
                                string filePath = ConfigurationManager.AppSettings["blob_imagepath1"] +
                                                                final_path + msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
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
                                    string message = individualpostpublishfn(account_id, lsaccess_token, post_id, objResult);
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
                                            string errorMessage = errorJson["error"]["message"].ToString();
                                            if (errorJson["error"]["error_user_msg"] != null)
                                            {
                                                string inaccessibleAccounts = errorJson["error"]["error_user_msg"].ToString();
                                                objResult.message = $"{inaccessibleAccounts}";
                                            }
                                            else
                                            {
                                                objResult.message = errorMessage;
                                            }
                                        }

                                    }
                                    catch (JsonException)
                                    {
                                        objResult.status = false;
                                        objResult.message = "Error Occured while uploading!!!";
                                    }

                                }
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Invalid Image Format !!";
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

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured While uploading- (DaPostInstaimage)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public void DaPostInstareel(HttpRequest httpRequest, result objResult)
        {
            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            HttpPostedFile httpPostedFile;
            string image_caption1 = httpRequest.Form[0];
            string account_id = httpRequest.Form[1];
            string mention = httpRequest.Form[2];

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

                        msSQL = " select access_token from crm_smm_tinstagramservice where account_id= '" + account_id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsaccess_token = objOdbcDataReader["access_token"].ToString();

                        }
                        if (lsaccess_token != null)
                        {
                            if (FileExtension == ".mp4" | FileExtension == ".MP4" | FileExtension == ".avi" | FileExtension == ".mkv" | FileExtension == ".wmv" | FileExtension == ".mov" | FileExtension == ".WebM" | FileExtension == ".flv" | FileExtension == ".hevc" | FileExtension == ".vpg")

                            {
                                bool status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                    "Instagram/" + msdocument_gid + FileExtension,
                                    FileExtension, ms);
                                ms.Close();
                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Instagram/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + account_id + "/media?media_type=REELS", Method.POST);
                                request.AddQueryParameter("access_token", lsaccess_token);
                                string filePath = ConfigurationManager.AppSettings["blob_imagepath1"] +
                                                                final_path + msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
                                request.AddQueryParameter("video_url", filePath);
                                request.AddQueryParameter("caption", image_caption1);
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
                                    string message = individualpostpublishfn(account_id, lsaccess_token, post_id, objResult);
                                    Console.WriteLine(message);

                                }
                                else if (response.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    string content = response.Content;
                                    JObject errorJson;
                                    errorJson = JObject.Parse(content);
                                    objResult.status = false;
                                    objResult.message = errorJson["error"]["message"].ToString();
                                }

                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Invalid Video Format !!";
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

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured While uploading- (DaPostInstareel)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public string individualpostpublishfn(string account_id, string lsaccess_token, string post_id, result objResult)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://graph.facebook.com");
            var request = new RestRequest("/" + account_id + "/media_publish?", Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("access_token", lsaccess_token);
            request.AddParameter("creation_id", post_id);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                objResult.status = true;
                objResult.message = "Posted in Instagram Successfully!!";
                return objResult.message;
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                var errorMessage = errorResponse?.error?.error_user_msg?.ToString();

                if (errorMessage.Contains("The media is not ready to be published. Please wait a moment."))
                {
                    return individualpostpublishfn(account_id, lsaccess_token, post_id, objResult);
                }
                objResult.status = false;
                objResult.message = errorMessage;
                return objResult.message;


            }
        }
        public result Dacarouselpost(HttpRequest httpRequest, result objresult)
        {
            try
            {
                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                string document_gid = string.Empty;
                HttpPostedFile httpPostedFile;
                string image_caption = httpRequest.Form[1];
                string account_id = httpRequest.Form[2];
                string mention = httpRequest.Form[3];
                MemoryStream ms_stream = new MemoryStream();
                string documentData_list = httpRequest.Form["documentData_list"];
                List<HrDocument> hrDocuments = JsonConvert.DeserializeObject<List<HrDocument>>(documentData_list);
                if (httpRequest.Files.Count > 0)
                {
                    httpFileCollection = httpRequest.Files;
                    string post_id = ""; // Initialize post_id variable outside the loop

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
                        MemoryStream ms = new MemoryStream();
                        ls_readStream.CopyTo(ms);
                        byte[] bytes = ms.ToArray();


                        msSQL = " select access_token from crm_smm_tinstagramservice where account_id= '" + account_id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsaccess_token = objOdbcDataReader["access_token"].ToString();

                        }
                        if (lsaccess_token != null)
                        {
                            if (FileExtension == ".jpg" | FileExtension == ".png" | FileExtension == ".jpeg" | FileExtension == ".gif" | FileExtension == ".JPG" | FileExtension == ".JPEG" | FileExtension == ".JIFF" | FileExtension == ".TIFF" | FileExtension == ".PNG" | FileExtension == ".GIF" | FileExtension == ".JFIF" | FileExtension == ".svg" | FileExtension == ".jfif")
                            {
                                bool status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                    "Instagram/" + msdocument_gid + FileExtension,
                                    FileExtension, ms);
                                ms.Close();

                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Instagram/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + account_id + "/media?is_carousel_item=true&media_type=IMAGE", Method.POST);
                                request.AddQueryParameter("access_token", lsaccess_token);
                                string filePath = ConfigurationManager.AppSettings["blob_imagepath1"] +
                                                                final_path + msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
                                request.AddQueryParameter("image_url", filePath);
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
                                IRestResponse response = client.Execute(request); if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var responseData = JsonConvert.DeserializeObject<Testas>(response.Content);
                                    string id = responseData.id; // Get the response id

                                    if (post_id == "")
                                    {
                                        post_id += id;
                                    }
                                    else
                                    {
                                        post_id += "," + id;
                                    }

                                }

                                else if (response.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    objresult.status = false;
                                    objresult.message = "Error While Posting in Instagram !!";

                                }
                            }
                            else if (FileExtension == ".mp4" | FileExtension == ".MP4" | FileExtension == ".avi" | FileExtension == ".mkv" | FileExtension == ".wmv" | FileExtension == ".mov" | FileExtension == ".WebM" | FileExtension == ".flv" | FileExtension == ".hevc" | FileExtension == ".vpg" | FileExtension == ".MOV")
                            {
                                bool status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                    "Instagram/" + msdocument_gid + FileExtension,
                                    FileExtension, ms);
                                ms.Close();

                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Instagram/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + account_id + "/media?is_carousel_item=true&media_type=REELS", Method.POST);
                                request.AddQueryParameter("access_token", lsaccess_token);
                                string filePath = ConfigurationManager.AppSettings["blob_imagepath1"] +
                                                                final_path + msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
                                request.AddQueryParameter("video_url", filePath);
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
                                    string id = responseData.id; // Get the response id

                                    if (post_id == "")
                                    {
                                        post_id += id;
                                    }
                                    else
                                    {
                                        post_id += "," + id;
                                    }

                                }

                                else if (response.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    objresult.status = false;
                                    objresult.message = "Error While Posting in Instagram !!";

                                }
                            }

                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Check the Access Token !!";
                        }

                    }
                    if (!string.IsNullOrEmpty(post_id))
                    {
                        carouselcontainerfn(account_id, lsaccess_token, post_id, image_caption, objresult);
                        Console.WriteLine(objresult.message, objresult.status);
                    }
                }

            }
            catch (Exception ex)
            {
                objresult.status = false;
                objresult.message = "Error occurred while posting Project: " + ex.Message;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured getting account details-(Dacarouselpost)!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return objresult;
        }
        public result carouselcontainerfn(string account_id, string lsaccess_token, string post_id, string image_caption, result objresult)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://graph.facebook.com");
            var request = new RestRequest("/" + account_id + "/media?media_type=CAROUSEL", Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("access_token", lsaccess_token);
            request.AddParameter("children", post_id);
            request.AddParameter("caption", image_caption);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {

                var responseData = JsonConvert.DeserializeObject<Testas>(response.Content);
                string container_id = responseData.id;
                carouselpublishfn(account_id, lsaccess_token, container_id, objresult);

            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                var errorMessage = errorResponse?.error?.message?.ToString();

                if (errorMessage.Contains("An unexpected error has occurred. Please retry your request later."))
                {
                    return carouselcontainerfn(account_id, lsaccess_token, post_id, image_caption, objresult);

                }
                objresult.status = false;
                objresult.message = errorMessage;
            }
            return objresult;
        }
        public result carouselpublishfn(string account_id, string lsaccess_token, string container_id, result objresult)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://graph.facebook.com");
            var request = new RestRequest("/" + account_id + "/media_publish?", Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("access_token", lsaccess_token);
            request.AddParameter("creation_id", container_id);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                objresult.status = true;
                objresult.message = "Posted in Instagram Successfully!!";
                return objresult;
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                var errorMessage = errorResponse?.error?.error_user_msg?.ToString();

                if (errorMessage.Contains("The media is not ready to be published. Please wait a moment."))
                {
                    return carouselpublishfn(account_id, lsaccess_token, container_id, objresult);

                }

                objresult.status = false;
                objresult.message = errorMessage;
                return objresult;
            }
        }
        public void DaPostInstastory(HttpRequest httpRequest, result objResult)
        {
            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            HttpPostedFile httpPostedFile;
            string account_id = httpRequest.Form[0];
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

                        msSQL = " select access_token from crm_smm_tinstagramservice where account_id= '" + account_id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsaccess_token = objOdbcDataReader["access_token"].ToString();

                        }
                        if (lsaccess_token != null)
                        {
                            if (FileExtension == ".jpg" | FileExtension == ".png" | FileExtension == ".jpeg" | FileExtension == ".gif" | FileExtension == ".JPG" | FileExtension == ".JPEG" | FileExtension == ".JIFF" | FileExtension == ".TIFF" | FileExtension == ".PNG" | FileExtension == ".GIF" | FileExtension == ".JFIF" | FileExtension == ".svg" | FileExtension == ".jfif")
                            {
                                bool status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                    "Instagram/" + msdocument_gid + FileExtension,
                                    FileExtension, ms);
                                ms.Close();
                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Instagram/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + account_id + "/media?media_type=STORIES", Method.POST);
                                request.AlwaysMultipartFormData = true;
                                request.AddParameter("access_token", lsaccess_token);
                                string filePath = ConfigurationManager.AppSettings["blob_imagepath1"] +
                                                                final_path + msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
                                request.AddParameter("image_url", filePath);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var responseData = JsonConvert.DeserializeObject<Testas>(response.Content);
                                    string post_id = responseData.id;
                                    string message = storypublishfn(account_id, lsaccess_token, post_id, objResult);
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
                                            objResult.message = errorJson["error"]["message"].ToString();
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
                                bool status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                    "Instagram/" + msdocument_gid + FileExtension,
                                    FileExtension, ms);
                                ms.Close();
                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Instagram/";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient("https://graph.facebook.com");
                                var request = new RestRequest("/" + account_id + "/media?media_type=STORIES", Method.POST);
                                request.AlwaysMultipartFormData = true;
                                request.AddParameter("access_token", lsaccess_token);
                                string filePath = ConfigurationManager.AppSettings["blob_imagepath1"] +
                                                                final_path + msdocument_gid + FileExtension +
                                                                ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                                ConfigurationManager.AppSettings["blob_imagepath8"];
                                request.AddParameter("video_url", filePath);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {

                                    var responseData = JsonConvert.DeserializeObject<Testas>(response.Content);
                                    string post_id = responseData.id;
                                    string message = storypublishfn(account_id, lsaccess_token, post_id, objResult);
                                    Console.WriteLine(message);

                                }
                                else if (response.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    string content = response.Content;
                                    JObject errorJson;
                                    errorJson = JObject.Parse(content);
                                    objResult.status = false;
                                    objResult.message = errorJson["error"]["message"].ToString();
                                }

                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Invalid Image Format !!";
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
                    objResult.message = "Error occures at story post !!";
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Uploading";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured While uploading- (DaPostInstastory)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Instagram/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public string storypublishfn(string account_id, string lsaccess_token, string post_id, result objResult)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://graph.facebook.com");
            var request = new RestRequest("/" + account_id + "/media_publish?", Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("access_token", lsaccess_token);
            request.AddParameter("creation_id", post_id);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                objResult.status = true;
                objResult.message = "Story Added Successfully!!";
                return objResult.message;
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                var errorMessage = errorResponse?.error?.error_user_msg?.ToString();

                if (errorMessage.Contains("The media is not ready to be published. Please wait a moment."))
                {
                    return storypublishfn(account_id, lsaccess_token, post_id, objResult);
                }

                objResult.status = false;
                objResult.message = errorMessage;
                return objResult.message;
            }
        }
    }

}
