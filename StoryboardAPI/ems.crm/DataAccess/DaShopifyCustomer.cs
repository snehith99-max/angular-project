using ems.crm.Models;
using ems.system.Models;
using ems.utilities.Functions;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Collections.Concurrent;



using static OfficeOpenXml.ExcelErrorValue;

namespace ems.crm.DataAccess
{
    public class DaShopifyCustomer
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        finance_cmnfunction objfinance = new finance_cmnfunction();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string lssource_gid;
        string lssource_name, param1, lsleadbank_name, lscategoryindustry_name, lscountry_name, msGetGidInvDt, lscustomergid, lscustomer, lsproductgid, lsproductuom_gid, lsproductuomgid, lscreated_date, lstotal_amount, lstotal_invoice, msINGetGID, lsqty_invoice, lsaddon_charge, lssalesorder_date, lssalesorderdtl_gid, lssalesorder_gid, lsproduct_gid, lsshopify_lineitemid, lsshopify_orderid, lsproduct_name, lsproduct_price, lsqty_quoted, lsproduct_price_l, lsselling_price, lsprice_l, lscustomer_email, lscurrency_code, lscustomer_mobile, lscustomer_gid, lsproductgroup_gid, lsproductgroup_name, lsemployee_gid, lsleadbank_gid, lsaccess_token, lsshopify_productid, lsshopify_store_name, lsstore_month_year, mssalesorderGID, mssalesorderGID1, lscountry_gid, mscusconGetGID, lscountrygid, mscustomerGetGID, msGETcustomercode,
            lsregion_name, lsbankcontact, msGetGid, msGetGid1, msGetGid2, msGetGid3, msGetGid4,
            msGetGid5, msGetGid6, msGetGid7, msGetGid8, msGetGid9, msGetGid10, msGetGid11, lscurrencyexchange_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5,
            mnResult6, mnResult7, mnResult8, mnResult9, mnResult10, mnResult11,
            mnResult12, mnResult13, mnResult14, mnResult15, mnResult16, mnResult17, mnResult18, mnResult19;
        char lsstatus, lsaddtocustomer;
        ///code  by snehith <summary>
        /// code  by snehith
        /// </summary>
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private static Task<get> _runningTask;

        public async Task<get> DaGetShopifyCustomer(string user_gid)
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

                    _runningTask = ExecuteGetShopifyCustomer(user_gid); // Start a new task
                    await _runningTask;
                }
                finally
                {
                    _semaphore.Release(); // Release the semaphore
                }
            }
        }


        public async Task<get> ExecuteGetShopifyCustomer(string user_gid)
        {
            get objresult = new get(); // Replace YourResultType with the actual return type

            try
            {
                string msSQL = " SELECT access_token,shopify_store_name,store_month_year FROM crm_smm_shopify_service limit 1 ";
                var objOdbcDataReader = objdbconn.GetDataReader(msSQL); 

                if (objOdbcDataReader.HasRows)
                {
                    string lsaccess_token = objOdbcDataReader["access_token"].ToString();
                    string lsshopify_store_name = objOdbcDataReader["shopify_store_name"].ToString();
                    string lsstore_month_year = objOdbcDataReader["store_month_year"].ToString();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    DateTime currentDateTime = DateTime.UtcNow;
                    string daysToSubtractString = ConfigurationManager.AppSettings["daysToSubtract"];
                    int daysToSubtract;
                    int.TryParse(daysToSubtractString, out daysToSubtract);
                    DateTime sevenDaysAgoDateTime = currentDateTime.AddDays(-daysToSubtract);

                    string currentDateTimeFormatted = currentDateTime.ToString("yyyy-MM-ddTHH:mm:ss-00:00");
                    string sevenDaysAgoDateTimeFormatted = sevenDaysAgoDateTime.ToString("yyyy-MM-ddTHH:mm:ss-00:00");

                    var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                    var request = new RestRequest("/admin/api/" + lsstore_month_year + "/customers.json?limit=250&created_at_min='" + sevenDaysAgoDateTimeFormatted + "'&created_at_max='" + currentDateTimeFormatted + "'", Method.GET);
                    request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                    request.AddHeader("Cookie", "_master_udr=eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaEpJaWxqWXpsak9UQXhPUzAyWkRZMkxUUXlOR1F0T0RKbVl5MDNaVEZsTnpFM09EY3dOV0lHT2daRlJnPT0iLCJleHAiOiIyMDI1LTEwLTIwVDA4OjI3OjU2LjU4MloiLCJwdXIiOiJjb29raWUuX21hc3Rlcl91ZHIifX0%3D--6f6310c22570c2812426da811c5f9f64d2d35161; _secure_admin_session_id=bbc22793fbba552b04eeebfaeb0de080; _secure_admin_session_id_csrf=bbc22793fbba552b04eeebfaeb0de080; identity-state=BAhbB0kiJWVhODM3YTZhN2M3Njg1MzhlNWQ3MTNhYzg2NmM5MWUwBjoGRUZJIiUwY2M0MWQ1ZjE4ZTQwZTcwYWQ1ZTVkMWUzMDBkMzZlYgY7AEY%3D--69633ab3c25bb20e105bbe14b912f36422abe9b1; identity-state-0cc41d5f18e40e70ad5e5d1e300d36eb=BAh7DEkiDnJldHVybi10bwY6BkVUSSI0aHR0cHM6Ly9lNDQ1NWYtMi5teXNob3BpZnkuY29tL2FkbWluL2F1dGgvbG9naW4GOwBUSSIRcmVkaXJlY3QtdXJpBjsAVEkiQGh0dHBzOi8vZTQ0NTVmLTIubXlzaG9waWZ5LmNvbS9hZG1pbi9hdXRoL2lkZW50aXR5L2NhbGxiYWNrBjsAVEkiEHNlc3Npb24ta2V5BjsAVDoMYWNjb3VudEkiD2NyZWF0ZWQtYXQGOwBUZhcxNjk3NzkxOTE0LjQyODUwMDJJIgpub25jZQY7AFRJIiU1YTQyNzRiZTI5ZmVjODE0MDU4ZTlmNGU3ZGZiNzU4MwY7AEZJIgpzY29wZQY7AFRbEEkiCmVtYWlsBjsAVEkiN2h0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvZGVzdGluYXRpb25zLnJlYWRvbmx5BjsAVEkiC29wZW5pZAY7AFRJIgxwcm9maWxlBjsAVEkiTmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvcGFydG5lcnMuY29sbGFib3JhdG9yLXJlbGF0aW9uc2hpcHMucmVhZG9ubHkGOwBUSSIwaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9iYW5raW5nLm1hbmFnZQY7AFRJIkJodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL21lcmNoYW50LXNldHVwLWRhc2hib2FyZC5ncmFwaHFsBjsAVEkiPGh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvc2hvcGlmeS1jaGF0LmFkbWluLmdyYXBocWwGOwBUSSI3aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9mbG93LndvcmtmbG93cy5tYW5hZ2UGOwBUSSI%2BaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9vcmdhbml6YXRpb24taWRlbnRpdHkubWFuYWdlBjsAVEkiPmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvbWVyY2hhbnQtYmFuay1hY2NvdW50Lm1hbmFnZQY7AFRJIg9jb25maWcta2V5BjsAVEkiDGRlZmF1bHQGOwBU--eb8709d3d8002d429911a5b1c28afca37dd02431; identity-state-ea837a6a7c768538e5d713ac866c91e0=BAh7DEkiDnJldHVybi10bwY6BkVUSSI0aHR0cHM6Ly9lNDQ1NWYtMi5teXNob3BpZnkuY29tL2FkbWluL2F1dGgvbG9naW4GOwBUSSIRcmVkaXJlY3QtdXJpBjsAVEkiQGh0dHBzOi8vZTQ0NTVmLTIubXlzaG9waWZ5LmNvbS9hZG1pbi9hdXRoL2lkZW50aXR5L2NhbGxiYWNrBjsAVEkiEHNlc3Npb24ta2V5BjsAVDoMYWNjb3VudEkiD2NyZWF0ZWQtYXQGOwBUZhYxNjk3NzkwNDc2LjU5MTkxOEkiCm5vbmNlBjsAVEkiJWM5NjYwYTQ2NmZhMTlhZTJlNDQyYmM3NjU0NDMxZWMzBjsARkkiCnNjb3BlBjsAVFsQSSIKZW1haWwGOwBUSSI3aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9kZXN0aW5hdGlvbnMucmVhZG9ubHkGOwBUSSILb3BlbmlkBjsAVEkiDHByb2ZpbGUGOwBUSSJOaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9wYXJ0bmVycy5jb2xsYWJvcmF0b3ItcmVsYXRpb25zaGlwcy5yZWFkb25seQY7AFRJIjBodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL2JhbmtpbmcubWFuYWdlBjsAVEkiQmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvbWVyY2hhbnQtc2V0dXAtZGFzaGJvYXJkLmdyYXBocWwGOwBUSSI8aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9zaG9waWZ5LWNoYXQuYWRtaW4uZ3JhcGhxbAY7AFRJIjdodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL2Zsb3cud29ya2Zsb3dzLm1hbmFnZQY7AFRJIj5odHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL29yZ2FuaXphdGlvbi1pZGVudGl0eS5tYW5hZ2UGOwBUSSI%2BaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9tZXJjaGFudC1iYW5rLWFjY291bnQubWFuYWdlBjsAVEkiD2NvbmZpZy1rZXkGOwBUSSIMZGVmYXVsdAY7AFQ%3D--7f37bdb0df101ca716441e71427765ef41612063");
                    IRestResponse response = client.Execute(request);
                    string response_content = response.Content;
                    shopifycustomerlist objMdlShopifyMessageResponse = new shopifycustomerlist();
                    objMdlShopifyMessageResponse = JsonConvert.DeserializeObject<shopifycustomerlist>(response_content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string url = response.Headers[16].Value.ToString();
                        string[] array = url.Split('<', '>');
                        string param1 = null;

                        if (array.Length > 1)
                        {
                            Uri myUri = new Uri(array[1]);
                            param1 = HttpUtility.ParseQueryString(myUri.Query).Get("page_info");
                        }

                        await fnLoadCustomers(objMdlShopifyMessageResponse, param1);

                        objresult.status = true;
                    }

                }

                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

            return objresult;
        }


        public async Task<bool> fnLoadCustomers(shopifycustomerlist objMdlShopifyMessageResponse, string pageToken)
        {
            try
            {
                foreach (var item in objMdlShopifyMessageResponse.customers)
                {

                    msSQL = " select shopify_id  from crm_trn_tshopifycustomer where shopify_id = '" + item.id + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows != true)
                    {
                        msSQL = " insert into crm_trn_tshopifycustomer(" +

                                " shopify_id," +

                                " email," +

                                " orders_count," +

                                " total_spent," +

                                " first_name," +

                                " last_name," +

                                " email_state," +

                                " default_company," +

                                " default_address1," +

                                " default_address2," +

                                " default_city," +

                                " default_country," +

                                " default_countrycode," +

                                " default_zip," +

                                " default_phone," +

                                " last_order_id)" +

                                " values(" +

                                 "'" + item.id + "'," +
                               "'" + item.email + "'," +
                               "'" + item.orders_count + "'," +
                               "'" + item.total_spent + "',";
                        if (item.first_name == null || item.first_name == "")
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.first_name.ToString().Replace("'", " ").Replace("'", " ") + "',";
                        }
                        if (item.last_name == null || item.last_name == "")
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.last_name.ToString().Replace("'", " ").Replace("'", " ") + "',";
                        }
                        if (item.email_marketing_consent == null)
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.email_marketing_consent.state + "',";
                        }
                        if (item.default_address == null || item.default_address.company == null)
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.default_address.company.ToString().Replace("'", " ") + "',";
                        }
                        if (item.default_address == null || item.default_address.address1 == null)
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                        }
                        if (item.default_address == null || item.default_address.address2 == null)
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                        }
                        if (item.default_address == null)
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.default_address.city + "',";
                        }
                        if (item.default_address == null)
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.default_address.country + "',";
                        }
                        if (item.default_address == null)
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.default_address.country_code + "',";
                        }
                        if (item.default_address == null)
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.default_address.zip + "',";
                        }
                        if (item.default_address == null)
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + item.default_address.phone + "',";
                        }
                        msSQL += "'" + item.last_order_id + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            objcmnfunctions.LogForAudit(msSQL);
                        }
                    }
                    else
                    {
                        string msSQL = "UPDATE crm_trn_tshopifycustomer SET " +
                                "email = '" + item.email + "'," +
                                "orders_count = '" + item.orders_count + "'," +
                                "total_spent = '" + item.total_spent + "',";

                        if (item.first_name == null || item.first_name == "")
                        {
                            msSQL += "first_name = NULL,";
                        }
                        else
                        {
                            msSQL += "first_name = '" + item.first_name.ToString().Replace("'", "").Replace("'", "") + "',";
                        }

                        if (item.last_name == null || item.last_name == "")
                        {
                            msSQL += "last_name = NULL,";
                        }
                        else
                        {
                            msSQL += "last_name = '" + item.last_name.ToString().Replace("'", " ").Replace("'", "") + "',";
                        }

                        if (item.email_marketing_consent == null)
                        {
                            msSQL += "email_state = NULL,";
                        }
                        else
                        {
                            msSQL += "email_state = '" + item.email_marketing_consent.state + "',";
                        }

                        if (item.default_address == null || item.default_address.company == null)
                        {
                            msSQL += "default_company = NULL,";
                        }
                        else
                        {
                            msSQL += "default_company = '" + item.default_address.company.ToString().Replace("'", " ") + "',";
                        }

                        if (item.default_address == null || item.default_address.address1 == null)
                        {
                            msSQL += "default_address1 = NULL,";
                        }
                        else
                        {
                            msSQL += "default_address1 = '" + item.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                        }

                        if (item.default_address == null || item.default_address.address2 == null)
                        {
                            msSQL += "default_address2 = NULL,";
                        }
                        else
                        {
                            msSQL += "default_address2 = '" + item.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                        }

                        if (item.default_address == null)
                        {
                            msSQL += "default_city = NULL,";
                        }
                        else
                        {
                            msSQL += "default_city = '" + item.default_address.city + "',";
                        }

                        if (item.default_address == null)
                        {
                            msSQL += "default_country = NULL,";
                        }
                        else
                        {
                            msSQL += "default_country = '" + item.default_address.country + "',";
                        }

                        if (item.default_address == null)
                        {
                            msSQL += "default_countrycode = NULL,";
                        }
                        else
                        {
                            msSQL += "default_countrycode = '" + item.default_address.country_code + "',";
                        }

                        if (item.default_address == null)
                        {
                            msSQL += "default_zip = NULL,";
                        }
                        else
                        {
                            msSQL += "default_zip = '" + item.default_address.zip + "',";
                        }

                        if (item.default_address == null)
                        {
                            msSQL += "default_phone = NULL,";
                        }
                        else
                        {
                            msSQL += "default_phone = '" + item.default_address.phone + "',";
                        }

                        msSQL += "last_order_id = '" + item.last_order_id + "' ";

                        // Assuming shopify_id is the primary key or unique identifier for the record
                        msSQL += "WHERE shopify_id = '" + item.id + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }

                    objOdbcDataReader.Close();
                }
                try
                {
                    msSQL = " SELECT access_token,shopify_store_name,store_month_year FROM crm_smm_shopify_service limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {

                        lsaccess_token = objOdbcDataReader["access_token"].ToString();
                        lsshopify_store_name = objOdbcDataReader["shopify_store_name"].ToString();
                        lsstore_month_year = objOdbcDataReader["store_month_year"].ToString();

                    }
                    objOdbcDataReader.Close();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                    var request = new RestRequest("/admin/api/" + lsstore_month_year + "/customers.json?limit=250&page_info=" + pageToken, Method.GET);
                    request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                    request.AddHeader("Cookie", "_master_udr=eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaEpJaWxqWXpsak9UQXhPUzAyWkRZMkxUUXlOR1F0T0RKbVl5MDNaVEZsTnpFM09EY3dOV0lHT2daRlJnPT0iLCJleHAiOiIyMDI1LTEwLTIwVDA4OjI3OjU2LjU4MloiLCJwdXIiOiJjb29raWUuX21hc3Rlcl91ZHIifX0%3D--6f6310c22570c2812426da811c5f9f64d2d35161; _secure_admin_session_id=bbc22793fbba552b04eeebfaeb0de080; _secure_admin_session_id_csrf=bbc22793fbba552b04eeebfaeb0de080; identity-state=BAhbB0kiJWVhODM3YTZhN2M3Njg1MzhlNWQ3MTNhYzg2NmM5MWUwBjoGRUZJIiUwY2M0MWQ1ZjE4ZTQwZTcwYWQ1ZTVkMWUzMDBkMzZlYgY7AEY%3D--69633ab3c25bb20e105bbe14b912f36422abe9b1; identity-state-0cc41d5f18e40e70ad5e5d1e300d36eb=BAh7DEkiDnJldHVybi10bwY6BkVUSSI0aHR0cHM6Ly9lNDQ1NWYtMi5teXNob3BpZnkuY29tL2FkbWluL2F1dGgvbG9naW4GOwBUSSIRcmVkaXJlY3QtdXJpBjsAVEkiQGh0dHBzOi8vZTQ0NTVmLTIubXlzaG9waWZ5LmNvbS9hZG1pbi9hdXRoL2lkZW50aXR5L2NhbGxiYWNrBjsAVEkiEHNlc3Npb24ta2V5BjsAVDoMYWNjb3VudEkiD2NyZWF0ZWQtYXQGOwBUZhcxNjk3NzkxOTE0LjQyODUwMDJJIgpub25jZQY7AFRJIiU1YTQyNzRiZTI5ZmVjODE0MDU4ZTlmNGU3ZGZiNzU4MwY7AEZJIgpzY29wZQY7AFRbEEkiCmVtYWlsBjsAVEkiN2h0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvZGVzdGluYXRpb25zLnJlYWRvbmx5BjsAVEkiC29wZW5pZAY7AFRJIgxwcm9maWxlBjsAVEkiTmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvcGFydG5lcnMuY29sbGFib3JhdG9yLXJlbGF0aW9uc2hpcHMucmVhZG9ubHkGOwBUSSIwaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9iYW5raW5nLm1hbmFnZQY7AFRJIkJodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL21lcmNoYW50LXNldHVwLWRhc2hib2FyZC5ncmFwaHFsBjsAVEkiPGh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvc2hvcGlmeS1jaGF0LmFkbWluLmdyYXBocWwGOwBUSSI3aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9mbG93LndvcmtmbG93cy5tYW5hZ2UGOwBUSSI%2BaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9vcmdhbml6YXRpb24taWRlbnRpdHkubWFuYWdlBjsAVEkiPmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvbWVyY2hhbnQtYmFuay1hY2NvdW50Lm1hbmFnZQY7AFRJIg9jb25maWcta2V5BjsAVEkiDGRlZmF1bHQGOwBU--eb8709d3d8002d429911a5b1c28afca37dd02431; identity-state-ea837a6a7c768538e5d713ac866c91e0=BAh7DEkiDnJldHVybi10bwY6BkVUSSI0aHR0cHM6Ly9lNDQ1NWYtMi5teXNob3BpZnkuY29tL2FkbWluL2F1dGgvbG9naW4GOwBUSSIRcmVkaXJlY3QtdXJpBjsAVEkiQGh0dHBzOi8vZTQ0NTVmLTIubXlzaG9waWZ5LmNvbS9hZG1pbi9hdXRoL2lkZW50aXR5L2NhbGxiYWNrBjsAVEkiEHNlc3Npb24ta2V5BjsAVDoMYWNjb3VudEkiD2NyZWF0ZWQtYXQGOwBUZhYxNjk3NzkwNDc2LjU5MTkxOEkiCm5vbmNlBjsAVEkiJWM5NjYwYTQ2NmZhMTlhZTJlNDQyYmM3NjU0NDMxZWMzBjsARkkiCnNjb3BlBjsAVFsQSSIKZW1haWwGOwBUSSI3aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9kZXN0aW5hdGlvbnMucmVhZG9ubHkGOwBUSSILb3BlbmlkBjsAVEkiDHByb2ZpbGUGOwBUSSJOaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9wYXJ0bmVycy5jb2xsYWJvcmF0b3ItcmVsYXRpb25zaGlwcy5yZWFkb25seQY7AFRJIjBodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL2JhbmtpbmcubWFuYWdlBjsAVEkiQmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvbWVyY2hhbnQtc2V0dXAtZGFzaGJvYXJkLmdyYXBocWwGOwBUSSI8aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9zaG9waWZ5LWNoYXQuYWRtaW4uZ3JhcGhxbAY7AFRJIjdodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL2Zsb3cud29ya2Zsb3dzLm1hbmFnZQY7AFRJIj5odHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL29yZ2FuaXphdGlvbi1pZGVudGl0eS5tYW5hZ2UGOwBUSSI%2BaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9tZXJjaGFudC1iYW5rLWFjY291bnQubWFuYWdlBjsAVEkiD2NvbmZpZy1rZXkGOwBUSSIMZGVmYXVsdAY7AFQ%3D--7f37bdb0df101ca716441e71427765ef41612063");
                    IRestResponse response = client.Execute(request);
                    string response_content = response.Content;
                    shopifycustomerlist objMdlShopifyMessageResponse1 = new shopifycustomerlist();
                    objMdlShopifyMessageResponse1 = JsonConvert.DeserializeObject<shopifycustomerlist>(response_content);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string param1;
                        string url = response.Headers[16].Value.ToString();
                        string[] array = url.Split('<', '>');
                        if (url.Contains("rel=\"next\""))
                        {
                            Uri myUri = new Uri(array[3]);
                            param1 = HttpUtility.ParseQueryString(myUri.Query).Get("page_info");
                            fnLoadCustomers(objMdlShopifyMessageResponse1, param1);
                        }
                        else
                        {

                            foreach (var item in objMdlShopifyMessageResponse1.customers)
                            {

                                msSQL = " select shopify_id  from crm_trn_tshopifycustomer where shopify_id = '" + item.id + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    msSQL = " insert into crm_trn_tshopifycustomer(" +
                               " shopify_id," +
                               " email," +
                               " orders_count," +
                               " total_spent," +
                               " first_name," +
                               " last_name," +
                               " email_state," +
                               " default_company," +
                               " default_address1," +
                               " default_address2," +
                               " default_city," +
                               " default_country," +
                               " default_countrycode," +
                               " default_zip," +
                               " default_phone," +
                               " last_order_id)" +
                               " values(" +

                                 "'" + item.id + "'," +
                               "'" + item.email + "'," +
                               "'" + item.orders_count + "'," +
                               "'" + item.total_spent + "',";
                                    if (item.first_name == null || item.first_name == "")
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.first_name.ToString().Replace("'", "").Replace("'", "") + "',";
                                    }
                                    if (item.last_name == null || item.last_name == "")
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.last_name.ToString().Replace("'", " ").Replace("'", "") + "',";
                                    }
                                    if (item.email_marketing_consent == null)
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.email_marketing_consent.state + "',";
                                    }
                                    if (item.default_address == null || item.default_address.company == null)
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.default_address.company.ToString().Replace("'", " ") + "',";
                                    }
                                    if (item.default_address == null || item.default_address.address1 == null)
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                    }
                                    if (item.default_address == null || item.default_address.address2 == null)
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                    }
                                    if (item.default_address == null)
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.default_address.city + "',";
                                    }
                                    if (item.default_address == null)
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.default_address.country + "',";
                                    }
                                    if (item.default_address == null)
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.default_address.country_code + "',";
                                    }
                                    if (item.default_address == null)
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.default_address.zip + "',";
                                    }
                                    if (item.default_address == null)
                                    {
                                        msSQL += "'" + null + "',"; ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + item.default_address.phone + "',";
                                    }
                                    msSQL += "'" + item.last_order_id + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                                else
                                {
                                    string msSQL = "UPDATE crm_trn_tshopifycustomer SET " +
                                            "email = '" + item.email + "'," +
                                            "orders_count = '" + item.orders_count + "'," +
                                            "total_spent = '" + item.total_spent + "',";

                                    if (item.first_name == null || item.first_name == "")
                                    {
                                        msSQL += "first_name = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "first_name = '" + item.first_name.ToString().Replace("'", "").Replace("'", "") + "',";
                                    }

                                    if (item.last_name == null || item.last_name == "")
                                    {
                                        msSQL += "last_name = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "last_name = '" + item.last_name.ToString().Replace("'", " ").Replace("'", "") + "',";
                                    }

                                    if (item.email_marketing_consent == null)
                                    {
                                        msSQL += "email_state = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "email_state = '" + item.email_marketing_consent.state + "',";
                                    }

                                    if (item.default_address == null || item.default_address.company == null)
                                    {
                                        msSQL += "default_company = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "default_company = '" + item.default_address.company.ToString().Replace("'", " ") + "',";
                                    }

                                    if (item.default_address == null || item.default_address.address1 == null)
                                    {
                                        msSQL += "default_address1 = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "default_address1 = '" + item.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                    }

                                    if (item.default_address == null || item.default_address.address2 == null)
                                    {
                                        msSQL += "default_address2 = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "default_address2 = '" + item.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                    }

                                    if (item.default_address == null)
                                    {
                                        msSQL += "default_city = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "default_city = '" + item.default_address.city + "',";
                                    }

                                    if (item.default_address == null)
                                    {
                                        msSQL += "default_country = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "default_country = '" + item.default_address.country + "',";
                                    }

                                    if (item.default_address == null)
                                    {
                                        msSQL += "default_countrycode = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "default_countrycode = '" + item.default_address.country_code + "',";
                                    }

                                    if (item.default_address == null)
                                    {
                                        msSQL += "default_zip = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "default_zip = '" + item.default_address.zip + "',";
                                    }

                                    if (item.default_address == null)
                                    {
                                        msSQL += "default_phone = NULL,";
                                    }
                                    else
                                    {
                                        msSQL += "default_phone = '" + item.default_address.phone + "',";
                                    }

                                    msSQL += "last_order_id = '" + item.last_order_id + "' ";

                                    // Assuming shopify_id is the primary key or unique identifier for the record
                                    msSQL += "WHERE shopify_id = '" + item.id + "'";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                }
                                objOdbcDataReader.Close();
                            }
                        }
                    }
                    objOdbcDataReader.Close();
                }
                catch (Exception ex)
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    return false;
                }
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return true;
        }

        public void DaGetShopifyCustomersList(MdlShopifyCustomer values)
        {
            try
            {


                msSQL = " select a.shopify_id,a.first_name,id,a.last_name, email, orders_count, last_order_id, total_spent, email_state, default_company, default_address1, default_address2, " +
                        " default_city, default_country, default_countrycode, default_zip, default_phone ,(case when a.shopify_id=b.shopify_id then 'Assigned' when b.shopify_id is null then 'Not Assign' end) as status_flag," +
                        " (case when a.shopify_id = c.customer_gid then 'Order Raised' when b.shopify_id = null then 'Not Raised' when b.customer_gid is null then 'Not Raised' end) as order_status " +
                        " from crm_trn_tshopifycustomer a " +
                        " left join crm_trn_tleadbank b on a.shopify_id=b.shopify_id " +
                        " left join cmr_smm_tshopifysalesorder c on a.shopify_id = c.customer_gid " +
                        " group by a.shopify_id";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<shopifycustomers_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new shopifycustomers_list
                        {
                            shopify_id = dt["shopify_id"].ToString(),
                            first_name = dt["first_name"].ToString(),
                            id = dt["id"].ToString(),
                            last_name = dt["last_name"].ToString(),
                            email = dt["email"].ToString(),
                            orders_count = dt["orders_count"].ToString(),
                            last_order_id = dt["last_order_id"].ToString(),
                            total_spent = dt["total_spent"].ToString(),
                            email_state = dt["email_state"].ToString(),
                            order_status = dt["order_status"].ToString(),
                            default_company = dt["default_company"].ToString(),
                            default_address1 = dt["default_address1"].ToString(),
                            default_address2 = dt["default_address2"].ToString(),
                            default_city = dt["default_city"].ToString(),
                            default_country = dt["default_country"].ToString(),
                            default_countrycode = dt["default_countrycode"].ToString(),
                            default_zip = dt["default_zip"].ToString(),
                            default_phone = dt["default_phone"].ToString(),
                            status_flag = dt["status_flag"].ToString(),

                        });
                        values.shopifycustomers_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Shopify CustomersList";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetShopifyCustomersAssignedList(MdlShopifyCustomer values)
        {
            try
            {


                msSQL = " select a.shopify_id,first_name,id, last_name, email, orders_count, last_order_id, total_spent, email_state, default_company, default_address1, default_address2, " +
                        " default_city, default_country, default_countrycode, default_zip, default_phone ,(case when a.shopify_id=b.shopify_id then 'Assigned' when b.shopify_id is null then 'Not Assign' end) as status_flag, " +
                        " (case when a.shopify_id = c.customer_gid then 'Order Raised' when b.shopify_id = null then 'Not Raised' when b.customer_gid is null then 'Not Raised' end) as order_status " +
                        " from crm_trn_tshopifycustomer a " +
                         " left join cmr_smm_tshopifysalesorder c on a.shopify_id=c.customer_gid " +
                        " left join crm_trn_tleadbank b on a.shopify_id=b.shopify_id where b.shopify_id is not null  group by a.shopify_id";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<shopifycustomersassigned_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new shopifycustomersassigned_list
                        {
                            shopify_id = dt["shopify_id"].ToString(),
                            first_name = dt["first_name"].ToString(),
                            id = dt["id"].ToString(),
                            last_name = dt["last_name"].ToString(),
                            email = dt["email"].ToString(),
                            orders_count = dt["orders_count"].ToString(),
                            last_order_id = dt["last_order_id"].ToString(),
                            total_spent = dt["total_spent"].ToString(),
                            order_status = dt["order_status"].ToString(),
                            email_state = dt["email_state"].ToString(),
                            default_company = dt["default_company"].ToString(),
                            default_address1 = dt["default_address1"].ToString(),
                            default_address2 = dt["default_address2"].ToString(),
                            default_city = dt["default_city"].ToString(),
                            default_country = dt["default_country"].ToString(),
                            default_countrycode = dt["default_countrycode"].ToString(),
                            default_zip = dt["default_zip"].ToString(),
                            default_phone = dt["default_phone"].ToString(),
                            status_flag = dt["status_flag"].ToString(),

                        });
                        values.shopifycustomersassigned_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching CustomersAssignedList";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaGetShopifyCustomersUnassignedList(MdlShopifyCustomer values)
        {
            try
            {


                msSQL = " select a.shopify_id,a.first_name,id,a.last_name, email, orders_count, last_order_id, total_spent, email_state, default_company, default_address1, default_address2, " +
                        " default_city, default_country, default_countrycode, default_zip, default_phone ,(case when a.shopify_id=b.shopify_id then 'Assigned' when b.shopify_id is null then 'Not Assign' end) as status_flag , " +
                        " (case when a.shopify_id = c.customer_gid then 'Order Raised' when b.shopify_id = null then 'Not Raised' when b.customer_gid is null then 'Not Raised' end) as order_status " +
                        " from crm_trn_tshopifycustomer a " +
                        " left join crm_trn_tleadbank b on a.shopify_id=b.shopify_id " +
                        " left join cmr_smm_tshopifysalesorder c on a.shopify_id=c.customer_gid " +
                        "where b.shopify_id is null  group by a.shopify_id";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<shopifycustomersunassigned_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new shopifycustomersunassigned_list
                        {
                            shopify_id = dt["shopify_id"].ToString(),
                            first_name = dt["first_name"].ToString(),
                            id = dt["id"].ToString(),
                            last_name = dt["last_name"].ToString(),
                            email = dt["email"].ToString(),
                            orders_count = dt["orders_count"].ToString(),
                            last_order_id = dt["last_order_id"].ToString(),
                            total_spent = dt["total_spent"].ToString(),
                            email_state = dt["email_state"].ToString(),
                            order_status = dt["order_status"].ToString(),
                            default_company = dt["default_company"].ToString(),
                            default_address1 = dt["default_address1"].ToString(),
                            default_address2 = dt["default_address2"].ToString(),
                            default_city = dt["default_city"].ToString(),
                            default_country = dt["default_country"].ToString(),
                            default_countrycode = dt["default_countrycode"].ToString(),
                            default_zip = dt["default_zip"].ToString(),
                            default_phone = dt["default_phone"].ToString(),
                            status_flag = dt["status_flag"].ToString(),

                        });
                        values.shopifycustomersunassigned_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While fetching CustomersUnassignedList";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetCustomerTotalCount(MdlShopifyCustomer values)
        {
            try
            {


                msSQL = " select count(shopify_id) as customer_totalcount from crm_trn_tshopifycustomer";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<customertotalcount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new customertotalcount_list
                        {
                            customer_totalcount = dt["customer_totalcount"].ToString(),


                        });
                        values.customertotalcount_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While fetching CustomerTotalCount";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetCustomerAssignedCount(MdlShopifyCustomer values)
        {
            try
            {

                msSQL = "  select count(a.shopify_id) as customer_assigncount from crm_trn_tleadbank a left join crm_trn_tshopifycustomer b on a.shopify_id=b.shopify_id where b.shopify_id is not null ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<customerassignedcount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new customerassignedcount_list
                        {
                            customer_assigncount = dt["customer_assigncount"].ToString(),


                        });
                        values.customerassignedcount_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While fetching CustomerAssignedCount";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetCustomerUnassignedCount(MdlShopifyCustomer values)
        {
            try
            {


                msSQL = "  select count(a.shopify_id) as unassign_count from crm_trn_tshopifycustomer   a left  join crm_trn_tleadbank b on a.shopify_id = b.shopify_id where b.shopify_id is  null ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<customerunassignedcount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new customerunassignedcount_list
                        {
                            unassign_count = dt["unassign_count"].ToString(),


                        });
                        values.customerunassignedcount_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While fetching CustomersUnassignedList";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public Status DaGetLeadmoved(string user_gid, shopifycustomermovingtolead values)

        {

            Status result = new Status();

            try

            {

                msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customer_type + "'";

                string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

                string ls_customer_typegid = values.customer_type;

                HttpContext ctx = HttpContext.Current;

                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {

                    HttpContext.Current = ctx;

                    DaGetcustomermoved(user_gid, values.shopifycustomers_lists, lscustomer_type, ls_customer_typegid);

                }));

                t.Start();

                result.status = true;

                result.message = "Leads Moving to CRM!";

            }

            catch (Exception ex)

            {

                result.message = "Error while Sending Message";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;

        }
        public void DaGetcustomermoved(string user_gid, shopifycustomers_lists[] shopifycustomers_lists, string lscustomer_type, string ls_customer_typegid)

        {

            try

            {


                for (int i = 0; i < shopifycustomers_lists.ToArray().Length; i++)

                {

                    msSQL = "select leadbank_gid from crm_trn_tleadbank where shopify_id='" + shopifycustomers_lists[i].shopify_id + "' ";

                    dt_datatable = objdbconn.GetDataTable(msSQL);


                    if (dt_datatable.Rows.Count == 0)

                    {
                        msSQL = " Select source_gid  from crm_mst_tsource where source_name = 'Shopify' ";

                        string lssource_name = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select leadbank_gid from crm_trn_tleadbankcontact where email='" + shopifycustomers_lists[i].email + "'  and mobile='" + shopifycustomers_lists[i].default_phone + "' and leadbank_name='" + shopifycustomers_lists[i].default_company + "' and main_contact ='Y'  group by email,mobile";

                        string lead = objdbconn.GetExecuteScalar(msSQL);

                        if (lead != null && lead != "")
                        {
                            msSQL = "update crm_trn_tleadbank set shopify_id='" + shopifycustomers_lists[i].shopify_id + "',source_gid='" + lssource_name + "' where leadbank_gid='" + lead + "'";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        else
                        {


                            msSQL = " Select country_gid  from adm_mst_tcountry where country_name = '" + shopifycustomers_lists[i].default_country + "' ";

                            string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = " Select  employee_gid  from hrm_mst_temployee where  user_gid = '" + user_gid + "'";

                            string employee_gid = objdbconn.GetExecuteScalar(msSQL);


                            msGetGid = objcmnfunctions.GetMasterGID("BMCC");

                            msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                            msSQL = " INSERT INTO crm_trn_tleadbank(" +

                                    " leadbank_gid," +

                                    " shopify_id," +

                                    " source_gid," +

                                    " leadbank_id," +

                                    " status," +

                                    " approval_flag, " +

                                    " lead_status," +

                                    " leadbank_code," +

                                    " leadbank_address1," +

                                    " leadbank_address2," +

                                    " leadbank_country," +

                                    " leadbank_city," +

                                    " leadbank_pin," +

                                    " customer_type," +

                                    " customertype_gid," +

                                    " main_branch," +

                                    " leadbank_name," +

                                    " created_by," +

                                    " created_date)" +

                                    " values(" +

                                    " '" + msGetGid1 + "'," +

                                    " '" + shopifycustomers_lists[i].shopify_id + "'," +

                                    " '" + lssource_name + "'," +

                                    " '" + msGetGid + "'," +

                                    " 'Y'," +

                                    " 'Approved'," +

                                    " 'Not Assigned'," +

                                    " 'H.Q'," +

                                    " '" + shopifycustomers_lists[i].default_address1 + "'," +

                                    " '" + shopifycustomers_lists[i].default_address2 + "'," +

                                    " '" + shopifycustomers_lists[i].default_country + "'," +

                                    " '" + shopifycustomers_lists[i].default_city + "'," +

                                    " '" + shopifycustomers_lists[i].default_zip + "'," +

                                    " '" + lscustomer_type + "'," +

                                    " '" + ls_customer_typegid + "'," +

                                    "'Y',";

                            if (shopifycustomers_lists[i].default_company == null || shopifycustomers_lists[i].default_company == "")

                            {

                                msSQL += "'" + shopifycustomers_lists[i].first_name + "  " + shopifycustomers_lists[i].last_name + "',";

                            }

                            else

                            {

                                msSQL += "'" + shopifycustomers_lists[i].default_company + "',";

                            }

                            msSQL += "'" + employee_gid + "'," +

                                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");

                            msSQL = " INSERT INTO crm_trn_tleadbankcontact" +

                                " (leadbankcontact_gid," +

                                " leadbank_gid," +

                                " leadbankcontact_name," +

                                " email, mobile," +

                                " country_code1," +

                                " created_date," +

                                " created_by," +

                                " leadbankbranch_name, " +

                                " address1, " +

                                " address2, " +

                                " city, " +

                                " pincode, " +

                                " country_gid, " +

                                " main_contact)" +

                                " values( " +

                                " '" + msGetGid2 + "'," +

                                " '" + msGetGid1 + "'," +

                                " '" + shopifycustomers_lists[i].first_name + "  " + shopifycustomers_lists[i].last_name + "'," +

                                " '" + shopifycustomers_lists[i].email + "'," +

                                " '" + shopifycustomers_lists[i].default_phone + "'," +

                                " '" + shopifycustomers_lists[i].default_countrycode + "'," +

                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +

                                " '" + employee_gid + "'," +

                                " 'H.Q'," +

                                " '" + shopifycustomers_lists[i].default_address1 + "'," +

                                " '" + shopifycustomers_lists[i].default_address2 + "'," +

                          " '" + shopifycustomers_lists[i].default_city + "'," +

                                " '" + shopifycustomers_lists[i].default_zip + "'," +

                                " '" + lscountry_gid + "'," +

                                " 'Y'" + ")";

                            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult1 != 0)

                            {
                                msSQL = " select currencyexchange_gid from crm_trn_tcurrencyexchange where country = '" + shopifycustomers_lists[i].default_country + "'  limit 1 ";

                                string lscurrencyexchange_gid = objdbconn.GetExecuteScalar(msSQL);

                            }

                            msSQL = "select shopify_id from crm_mst_tcustomer where shopify_id='" + shopifycustomers_lists[i].shopify_id + "' ";
                            string shopify_idcustomer = objdbconn.GetExecuteScalar(msSQL);
                            if (shopify_idcustomer == null || shopify_idcustomer == "")
                            {
                                msGetGid = objcmnfunctions.GetMasterGID("CC");
                                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC'";
                                string lsCode = objdbconn.GetExecuteScalar(msSQL);

                                string lscustomer_code = "CC-" + "00" + lsCode;
                                mscustomerGetGID = objcmnfunctions.GetMasterGID("BCRM");
                                msGETcustomercode = objcmnfunctions.GetMasterGID("CO");
                                msSQL = " INSERT INTO crm_mst_tcustomer" +

                                   " (customer_gid," +

                                   " shopify_id," +

                                   " customer_type," +

                                   " customer_id," +

                                   " customer_name," +

                                   " customer_code," +

                                   " customer_address," +

                                   " customer_address2," +

                                   " customer_country," +

                                   " currency_gid," +

                                   " customer_city," +

                                   " customer_pin," +

                                   " main_branch," +

                                   " status, " +

                                   " created_by," +

                                   " created_date)" +

                                   " values( " +

                                   "'" + mscustomerGetGID + "'," +

                                   "'" + shopifycustomers_lists[i].shopify_id + "'," +

                                   "'" + lscustomer_type + "'," +

                                   "'" + msGETcustomercode + "',";

                                if (shopifycustomers_lists[i].default_company == null || shopifycustomers_lists[i].default_company == "")

                                {

                                    msSQL += "'" + shopifycustomers_lists[i].first_name + "  " + shopifycustomers_lists[i].last_name + "',";

                                }

                                else

                                {

                                    msSQL += "'" + shopifycustomers_lists[i].default_company + "',";

                                }

                                msSQL += "'" + lscustomer_code + "'," +

                                         " '" + shopifycustomers_lists[i].default_address1 + "'," +

                                        " '" + shopifycustomers_lists[i].default_address2 + "'," +

                                        " '" + lscountry_gid + "'," +

                                         "'" + lscurrencyexchange_gid + "'," +

                                         " '" + shopifycustomers_lists[i].default_city + "'," +

                                         " '" + shopifycustomers_lists[i].default_zip + "'," +

                                         " 'Y'," +

                                         " 'Active'," +

                                    "'" + user_gid + "'," +

                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "update crm_trn_tleadbank set " +

                                      "customer_gid = '" + mscustomerGetGID + "'" +

                                      "where leadbank_gid='" + msGetGid1 + "'";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = " select country_gid from crm_trn_tcurrencyexchange where country = '" + shopifycustomers_lists[i].default_country + "'  limit 1 ";

                                string lscountrygid = objdbconn.GetExecuteScalar(msSQL);

                                if (mnResult == 1)
                                {
                                    string customer_name = shopifycustomers_lists[i].first_name + "  " + shopifycustomers_lists[i].last_name;
                                    objfinance.finance_vendor_debitor("Sales", lscustomer_code, customer_name, mscustomerGetGID, user_gid);
                                    string trace_comment = " Added a customer on " + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    objcmnfunctions.Tracelog(mscustomerGetGID, user_gid, trace_comment, "added_customer");

                                    mscusconGetGID = objcmnfunctions.GetMasterGID("BCCM");

                                    msSQL = " INSERT INTO crm_mst_tcustomercontact" +

                                   " (customercontact_gid," +

                                   " customer_gid," +

                                   " customerbranch_name, " +

                                   " customercontact_name," +

                                   " email," +

                                   " created_date," +

                                   " created_by," +

                                   " address1, " +

                                   " address2, " +

                                   " country_gid, " +

                                   " city, " +

                                   " zip_code, " +

                                   " country_code," +

                                   " mobile)" +

                                   " values( " +

                                   "'" + mscusconGetGID + "'," +

                                   "'" + mscustomerGetGID + "'," +

                                   "'H.Q', " +

                                   "'" + shopifycustomers_lists[i].first_name + "  " + shopifycustomers_lists[i].last_name + "'," +

                                   "'" + shopifycustomers_lists[i].email + "'," +

                                   "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +

                                   "'" + employee_gid + "', " +

                                   " '" + shopifycustomers_lists[i].default_address1 + "'," +

                                   " '" + shopifycustomers_lists[i].default_address2 + "'," +

                                   "'" + lscountrygid + "'," +

                                  " '" + shopifycustomers_lists[i].default_city + "'," +

                                  " '" + shopifycustomers_lists[i].default_zip + "'," +

                                   "'" + shopifycustomers_lists[i].default_countrycode + "'," +

                                   "'" + shopifycustomers_lists[i].default_phone + "')";

                                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                               

                            }



                        }

                    }



                }
            }

            catch (Exception ex)

            {


                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }


        }


        private static SemaphoreSlim _semaphore1 = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue1 = new ConcurrentQueue<string>();
        private static Task<getorders> _runningTask1;
        public async Task<getorders> DaGetShopifyOrder(string user_gid)
        {
            _queue1.Enqueue(user_gid); // Add the request to the queue
            await ProcessQueue1(); // Process the queue
            return await _runningTask1;
        }

        private async Task ProcessQueue1()
        {
            while (_queue1.TryDequeue(out string user_gid))
            {
                await _semaphore1.WaitAsync(); // Ensure that only one method runs at a time
                try
                {
                    if (_runningTask1 != null && !_runningTask1.IsCompleted)
                    {
                        await _runningTask1; // Wait for the previous task to complete
                    }

                    _runningTask1 = ExecuteGetShopifyOrder(user_gid); // Start a new task
                    await _runningTask1;
                }
                finally
                {
                    _semaphore1.Release(); // Release the semaphore
                }
            }
        }

        public async Task<getorders> ExecuteGetShopifyOrder(string user_gid)
        {
            getorders objresult = new getorders();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                string lsaccess_token, lsshopify_store_name, lsstore_month_year, msSQL, mssalesorderGID, mssalesorderGID1;
                int mnResult;

                msSQL = "SELECT access_token,shopify_store_name,store_month_year FROM crm_smm_shopify_service LIMIT 1";
                var objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsaccess_token = objOdbcDataReader["access_token"].ToString();
                    lsshopify_store_name = objOdbcDataReader["shopify_store_name"].ToString();
                    lsstore_month_year = objOdbcDataReader["store_month_year"].ToString();
                }
                else
                {
                    // Handle missing rows
                    return null;
                }
                objOdbcDataReader.Close();
                DateTime currentDateTime = DateTime.UtcNow;
                string daysToSubtractString = ConfigurationManager.AppSettings["daysToSubtract"];
                int daysToSubtract;
                int.TryParse(daysToSubtractString, out daysToSubtract);
                DateTime sevenDaysAgoDateTime = currentDateTime.AddDays(-daysToSubtract);
                string currentDateTimeFormatted = currentDateTime.ToString("yyyy-MM-ddTHH:mm:ss-00:00");
                string sevenDaysAgoDateTimeFormatted = sevenDaysAgoDateTime.ToString("yyyy-MM-ddTHH:mm:ss-00:00");


                string baseUrl = $"https://{lsshopify_store_name}.myshopify.com";
                string resourceUrl = $"/admin/api/{lsstore_month_year}/orders.json?status=any&limit=250&created_at_min={sevenDaysAgoDateTimeFormatted}&created_at_max={currentDateTimeFormatted}";
                string url = $"{baseUrl}{resourceUrl}";

                while (!string.IsNullOrEmpty(url))
                {
                    var client = new RestClient(baseUrl);
                    var request = new RestRequest(resourceUrl, Method.GET);
                    if (url.Contains("page_info"))
                    {
                        var uri = new Uri(url);
                        var pageInfo = HttpUtility.ParseQueryString(uri.Query).Get("page_info");
                        request.Resource = $"/admin/api/{lsstore_month_year}/orders.json";
                        request.Parameters.Clear();
                        request.AddParameter("page_info", pageInfo);
                        request.AddParameter("limit", "250");
                        request.AddHeader("X-Shopify-Access-Token", lsaccess_token);
                    }
                    else
                    {
                        request.AddHeader("X-Shopify-Access-Token", lsaccess_token);
                        request.Resource = resourceUrl;
                    }
                    IRestResponse response = client.Execute(request);


                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception("Failed to retrieve orders from Shopify API.");
                    }

                    string response_content = response.Content;
                    shopifyorder_lists objMdlShopifyMessageResponse = JsonConvert.DeserializeObject<shopifyorder_lists>(response_content);

                    //await Task.WhenAll(objMdlShopifyMessageResponse.orders.Select(async item =>
                    //{
                    foreach (var item in objMdlShopifyMessageResponse.orders)
                    {
                        try
                        {
                            string customer_gid, lscountry_gid;
                            double price;
                            msSQL = "SELECT shopify_orderid FROM smr_trn_tsalesorder WHERE shopify_orderid = '" + item.id + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows != true)
                            {
                                objOdbcDataReader.Close();
                                msSQL = "SELECT employee_gid FROM hrm_mst_temployee WHERE user_gid = '" + user_gid + "'";
                                string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = "SELECT customer_gid FROM crm_mst_tcustomer WHERE shopify_id = '" + item.customer.id + "'";
                                 customer_gid = objdbconn.GetExecuteScalar(msSQL);
                                if (customer_gid == null || customer_gid == "")
                                {
                                    msGetGid = objcmnfunctions.GetMasterGID("CC");
                                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC'";
                                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                                    string lscustomer_code = "CC-" + "00" + lsCode;
                                     customer_gid = objcmnfunctions.GetMasterGID("BCRM");
                                    if(item.customer.default_address.country != null)
                                    {

                                        msSQL = " select currencyexchange_gid from crm_trn_tcurrencyexchange where country = '" + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'  limit 1 ";
                                        string lscurrencyexchange_gid = objdbconn.GetExecuteScalar(msSQL);
                                    }
                                    else
                                    {
                                        lscurrencyexchange_gid = null;
                                    }
                                    if (item.customer.default_address.country != null)
                                    {
                                        msSQL = " Select country_gid  from adm_mst_tcountry where country_name = '" + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "' ";
                                        lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
                                    }
                                    else
                                    {
                                        lscountry_gid = null;
                                    }
                                    msGETcustomercode = objcmnfunctions.GetMasterGID("CO");
                                    msSQL = " INSERT INTO crm_mst_tcustomer" +
                                   " (customer_gid," +
                                   " shopify_id," +
                                   " customer_id," +
                                   " currency_gid," +
                                   " customer_name," +
                                   " customer_address," +
                                   " customer_address2," +
                                   " customer_country," +
                                   " customer_city," +
                                   " customer_pin," +
                                   " main_branch," +
                                   " status, " +
                                   " created_by," +
                                   " created_date)" +
                                   " values( " +
                                   "'" + customer_gid + "'," +
                                   "'" + item.customer.id + "'," +
                                   "'" + lscustomer_code + "'," +
                                   "'" + lscurrencyexchange_gid + "',";
                                    if (item.customer.first_name != null)
                                    {
                                        if (item.customer.last_name != null)
                                        {
                                           msSQL+= "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "',";

                                        }
                                        else
                                        {
                                            msSQL += "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + "',";

                                        }
                                    }
                                    else
                                    {
                                        msSQL += "'" + null + "',";
                                    }
                                    if (item.customer.default_address.address1 != null)
                                    {
                                        msSQL += " '" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + null + "',";
                                    }
                                    if (item.customer.default_address.address2 != null)
                                    {
                                        msSQL += " '" + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + null + "',";
                                    }
                                    if (item.customer.default_address.country != null)
                                    {
                                        msSQL += " '" + lscountry_gid + "',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + null + "',";
                                    }
                                    if (item.customer.default_address.city != null)
                                    {
                                        msSQL += " '" + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                    }   
                                    else
                                    {
                                        msSQL += "'" + null + "',";
                                    }
                                    if (item.customer.default_address.zip != null)
                                    {
                                        msSQL += " '" + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                        ;
                                    }
                                    else
                                    {
                                        msSQL += "'" + null + "',";
                                    }          
                                    msSQL+=" 'Y'," +
                                            " 'Active'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult == 1)
                                    {
                                        string customer_name = item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "");
                                        objfinance.finance_vendor_debitor("Sales", lscustomer_code, customer_name, customer_gid, user_gid);
                                        string trace_comment = " Added a customer on " + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        objcmnfunctions.Tracelog(customer_gid, user_gid, trace_comment, "added_customer");

                                        mscusconGetGID = objcmnfunctions.GetMasterGID("BCCM");

                                        msSQL = " INSERT INTO crm_mst_tcustomercontact" +

                                       " (customercontact_gid," +

                                       " customer_gid," +

                                       " customerbranch_name, " +

                                       " customercontact_name," +

                                       " email," +

                                       " created_date," +

                                       " created_by," +

                                       " address1, " +

                                       " address2, " +

                                       " country_gid, " +

                                       " city, " +

                                       " zip_code, " +

                                       " mobile)" +

                                       " values( " +

                                       "'" + mscusconGetGID + "'," +

                                       "'" + customer_gid + "'," +

                                       "'H.Q', ";

                                           if (item.customer.first_name != null)
                                        {
                                            if (item.customer.last_name != null)
                                            {
                                                msSQL += "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "',";

                                            }
                                            else
                                            {
                                                msSQL += "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + "',";

                                            }
                                        }
                                        else
                                        {
                                            msSQL += "'" + null + "',";
                                        }
                                       msSQL+= "'" + item.email + "'," +

                                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +

                                       "'" + user_gid + "', ";
                                        if (item.customer.default_address.address1 != null)
                                        {
                                            msSQL += " '" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + null + "',";
                                        }
                                        if (item.customer.default_address.address2 != null)
                                        {
                                            msSQL += " '" + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + null + "',";
                                        }
                                        if (item.customer.default_address.country != null)
                                        {
                                            msSQL += " '" + lscountry_gid + "',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + null + "',";
                                        }
                                        if (item.customer.default_address.city != null)
                                        {
                                            msSQL += " '" + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                        }
                                        else
                                        {
                                            msSQL +=    "'" + null + "',";
                                        }
                                        if (item.customer.default_address.zip != null)
                                        {
                                            msSQL += " '" + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";
                                            ;
                                        }
                                        else
                                        {
                                            msSQL += "'" + null + "',";
                                        }
                                        if (item.customer.phone == null || item.customer == null)
                                        {
                                            msSQL += "'" + null + "')";
                                        }
                                        else
                                        {
                                            msSQL += "'" + item.customer.phone + "')";
                                        }

                                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult2 == 1)
                                        {
                                            mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP");
                                            string formattedDate = "";

                                            string dateString = item.created_at.ToString();
                                            DateTimeOffset originalDate;

                                            if (DateTimeOffset.TryParseExact(dateString, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalDate))
                                            {
                                                formattedDate = originalDate.ToString("yyyy-MM-dd");
                                            }
                                            else if (DateTimeOffset.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:sszzz", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalDate))
                                            {
                                                formattedDate = originalDate.ToString("yyyy-MM-dd");
                                            }
                                            else if (DateTimeOffset.TryParseExact(dateString, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalDate))
                                            {
                                                formattedDate = originalDate.ToString("yyyy-MM-dd");
                                            }

                                            msSQL = "select branch_gid from hrm_mst_tbranch limit 1;";
                                            string branch_gid = objdbconn.GetExecuteScalar(msSQL);
                                            msSQL = "select exchange_rate from crm_trn_tcurrencyexchange where currency_code='"+item.currency+"';";
                                            string exchange_rate = objdbconn.GetExecuteScalar(msSQL);

                                            msSQL = "INSERT INTO cmr_smm_tshopifysalesorder (" +
                                                    "salesorder_gid," +
                                                    "branch_gid," +
                                                    "shopify_orderid," +
                                                    "salesorder_date," +
                                                    "shopifyorder_number," +
                                                    "shopify_customerid," +
                                                    "customer_gid," +
                                                    "customer_name," +
                                                    "customer_contact_person," +
                                                    "created_by," +
                                                    "created_date," +
                                                    "additional_discount," +
                                                    "Grandtotal," +
                                                    "salesorder_status," +
                                                    "addon_charge," +
                                                    "grandtotal_l," +
                                                    "exchange_rate," +
                                                    "currency_code," +
                                                    "customer_mobile," +
                                                    "customer_address," +
                                                    "shipping_to," +
                                                    "billing_to," +
                                                    "total_price," +
                                                    "fulfillment_status," +
                                                    "customer_email" +
                                                    ") VALUES (" +
                                                    "'" + mssalesorderGID + "'," +
                                                    "'" + branch_gid + "'," +
                                                    "'" + item.id + "'," +
                                                    "'" + formattedDate + "'," +
                                                    "'" + item.name + "'," +
                                                    "'" + item.customer.id + "'," +
                                                    "'" + customer_gid + "'," +
                                                    "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                                    "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                                    "'" + lsemployee_gid + "'," +
                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";

                                            msSQL += "'" + item.total_discounts_set.shop_money.amount + "'," +
                                                "'" + item.total_price + "'," +
                                                "'" + item.financial_status + "'," +
                                                "'" + item.total_shipping_price_set.shop_money.amount + "'," +
                                                "'" + item.total_price + "'," +
                                                "'" + exchange_rate + "'," +
                                                "'" + item.currency + "',";
                                            if (item.customer.phone == null || item.customer == null)
                                            {
                                                msSQL += "'" + null + "',";
                                            }
                                            else
                                            {
                                                msSQL += "'" + item.customer.phone + "',";
                                            }

                                            if (item.customer == null || item.customer.default_address == null)
                                            {
                                                msSQL += "'" + null + "',";
                                            }
                                            else
                                            {
                                                if (item.customer.default_address.address2 == null)
                                                {
                                                    msSQL += "'" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                                else
                                                {
                                                    msSQL += "'" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                            }

                                            if (item.shipping_address == null || item.shipping_address.address1 == null)
                                            {
                                                msSQL += "'" + null + "',";
                                            }
                                            else
                                            {
                                                if (item.shipping_address.address2 == null)
                                                {
                                                    msSQL += "'" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                                else
                                                {
                                                    msSQL += "'" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.shipping_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                            }

                                            if (item.billing_address == null || item.billing_address.address1 == null)
                                            {
                                                msSQL += "'" + null + "',";
                                            }
                                            else
                                            {
                                                if (item.billing_address.address2 == null)
                                                {
                                                    msSQL += "'" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                                else
                                                {
                                                    msSQL += "'" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.billing_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                            }


                                            msSQL += "'" + item.total_line_items_price + "',";
                                            msSQL += "'" + item.fulfillment_status + "',";
                                            msSQL += "'" + item.email + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult == 1)
                                            {
                                                msSQL = "INSERT INTO smr_trn_tsalesorder (" +
                                                        "salesorder_gid," +
                                                        "branch_gid," +
                                                        "shopify_orderid," +
                                                        "so_referenceno1," +
                                                        "salesorder_date," +
                                                        "shopifyorder_number," +
                                                        "shopify_customerid," +
                                                        "customer_gid," +
                                                        "customer_name," +
                                                        "customer_contact_person," +
                                                        "created_by," +
                                                        "created_date," +
                                                        "additional_discount," +
                                                        "Grandtotal," +
                                                        "salesorder_status," +
                                                        "addon_charge," +
                                                        "grandtotal_l," +
                                                        "exchange_rate," +
                                                        "currency_code," +
                                                        "customer_mobile," +
                                                        "customer_address," +
                                                        "shipping_to," +
                                                        "billing_to," +
                                                        "total_price," +
                                                        "customer_email," +
                                                        "fulfillment_status," +
                                                        "source_flag" +
                                                        ") VALUES (" +
                                                        "'" + mssalesorderGID + "'," +
                                                        "'" + branch_gid + "'," +
                                                        "'" + item.id + "'," +
                                                        "'" + item.name + "'," +
                                                        "'" + formattedDate + "'," +
                                                        "'" + item.name + "'," +
                                                        "'" + item.customer.id + "'," +
                                                        "'" + customer_gid + "'," +
                                                        "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                                        "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                                        "'" + lsemployee_gid + "'," +
                                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                                                msSQL += "'" + item.total_discounts_set.shop_money.amount + "'," +
                                                    "'" + item.total_price + "'," +
                                                    "'" + item.financial_status + "'," +
                                                    "'" + item.total_shipping_price_set.shop_money.amount + "'," +
                                                    "'" + item.total_price + "'," +
                                                    "'" + exchange_rate + "'," +
                                                    "'" + item.currency + "',";

                                                if (item.customer.phone == null || item.customer == null)
                                                {
                                                    msSQL += "'" + null + "',";
                                                }
                                                else
                                                {
                                                    msSQL += "'" + item.customer.phone + "',";
                                                }

                                                if (item.customer == null || item.customer.default_address == null)
                                                {
                                                    msSQL += "'" + null + "',";
                                                }
                                                else
                                                {
                                                    if (item.customer.default_address.address2 == null)
                                                    {
                                                        msSQL += "'" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                    else
                                                    {
                                                        msSQL += "'" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                }

                                                if (item.shipping_address == null || item.shipping_address.address1 == null)
                                                {
                                                    msSQL += "'" + null + "',";
                                                }
                                                else
                                                {
                                                    if (item.shipping_address.address2 == null)
                                                    {
                                                        msSQL += "'" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                    else
                                                    {
                                                        msSQL += "'" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.shipping_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                }

                                                if (item.billing_address == null || item.billing_address.address1 == null)
                                                {
                                                    msSQL += "'" + null + "',";
                                                }
                                                else
                                                {
                                                    if (item.billing_address.address2 == null)
                                                    {
                                                        msSQL += "'" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                    else
                                                    {
                                                        msSQL += "'" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.billing_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                }

                                                msSQL += "'" + item.total_line_items_price + "',";
                                                msSQL += "'" + item.email + "',";
                                                msSQL += "'" + item.fulfillment_status + "',";
                                                msSQL += "'S')";
                                                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                if (mnResult2 != 0)
                                                {
                                                    //await Task.WhenAll(item.line_items.Select(async item1 =>
                                                    //{
                                                    foreach (var item1 in item.line_items)
                                                    {
                                                        try
                                                        {


                                                            msSQL = "SELECT productgroup_gid FROM pmr_mst_tproduct WHERE customerproduct_code = '" + item1.sku + "'";
                                                            string productgroup_gid = objdbconn.GetExecuteScalar(msSQL);
                                                            msSQL = "SELECT product_gid FROM pmr_mst_tproduct WHERE customerproduct_code = '" + item1.sku + "'";
                                                            string product_gid = objdbconn.GetExecuteScalar(msSQL);

                                                            mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                                                            msSQL = "INSERT INTO cmr_smm_tshopifysalesorderdtl (" +
                                                                   "salesorderdtl_gid," +
                                                                   "salesorder_gid," +
                                                                   "product_gid," +
                                                                   "productgroup_gid," +
                                                                   "product_code," +
                                                                   "customerproduct_code," +
                                                                   "shopify_lineitemid," +
                                                                   "shopify_orderid," +
                                                                   "product_name," +
                                                                   "display_field," +
                                                                   "product_price," +
                                                                   "discount_amount," +
                                                                   "qty_quoted," +
                                                                   "taxable," +
                                                                   "tax1_gid," +
                                                                   "tax_name," +
                                                                   "tax_amount," +
                                                                   "selling_price," +
                                                                   "price," +
                                                                   "product_price_l," +
                                                                   "created_by," +
                                                                   "created_date," +
                                                                   "price_l" +
                                                                   ") VALUES (" +
                                                                   "'" + mssalesorderGID1 + "'," +
                                                                   "'" + mssalesorderGID + "'," +
                                                                   "'" + product_gid + "'," +
                                                                   "'" + productgroup_gid + "'," +
                                                                   "'" + item1.sku + "'," +
                                                                   "'" + item1.sku + "'," +
                                                                   "'" + item1.id + "'," +
                                                                   "'" + item.id + "'," +
                                                                   "'" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                   "'" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                   "'" + item1.price + "',";
                                                            if (item1.discount_allocations == null || item1.discount_allocations.Length == 0)
                                                            {
                                                                price = (item1.quantity) * double.Parse(item1.price);
                                                                msSQL += "'" + 0.00 + "',";
                                                                msSQL += "'" + item1.quantity + "',";
                                                            }
                                                            else
                                                            {
                                                                price = (item1.quantity) * double.Parse(item1.price) - double.Parse(item1.discount_allocations[0].amount);
                                                                msSQL += "'" + item1.discount_allocations[0].amount + "',";
                                                                msSQL += "'" + item1.quantity + "',";
                                                            }

                                                            msSQL += "'" + item1.taxable + "',";
                                                            if (item1.taxable == true)
                                                            {
                                                                if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                {
                                                                    msSQL += "'" + null + "',";
                                                                    msSQL += "'" + 0.00 + "',";
                                                                }
                                                                else
                                                                {
                                                                    var tax_rates = (item1.tax_lines[0].rate * 100);
                                                                    if (tax_rates == 20)
                                                                    {
                                                                        msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE '%20%'";
                                                                        string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_gid + "',";
                                                                        msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE '%20%'";
                                                                        string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_namelines + "',";
                                                                        msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                    }
                                                                    else
                                                                    {

                                                                        msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE '%5%'";
                                                                        string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_gid + "',";
                                                                        msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE '%5%'";
                                                                        string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_namelines + "',";
                                                                        msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                {
                                                                    msSQL += "'" + null + "',";
                                                                    msSQL += "'" + 0.00 + "',";
                                                                }
                                                                else
                                                                {
                                                                    msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE'%VAT 0%'";
                                                                    string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                    msSQL += "'" + tax_gid + "',";
                                                                    msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE'%VAT 0%'";
                                                                    string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                    msSQL += "'" + tax_namelines + "',";
                                                                    msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                }
                                                            }

                                                            msSQL += "'" + item1.price + "'," +
                                                                 "'" + price + "'," +
                                                            "'" + item1.price + "'," +
                                                            "'" + lsemployee_gid + "'," +
                                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                            "'" + price + "')";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                            if (mnResult == 1)
                                                            {
                                                                msSQL = "INSERT INTO smr_trn_tsalesorderdtl (" +
                                                                        "salesorderdtl_gid," +
                                                                        "salesorder_gid," +
                                                                        "product_gid," +
                                                                        "productgroup_gid," +
                                                                        "product_code," +
                                                                        "customerproduct_code," +
                                                                        "shopify_lineitemid," +
                                                                        "shopify_orderid," +
                                                                        "product_name," +
                                                                        "display_field," +
                                                                        "product_price," +
                                                                        "discount_amount," +
                                                                        "qty_quoted," +
                                                                        "taxable," +
                                                                   "tax1_gid," +
                                                                        "tax_name," +
                                                                        "tax_amount," +
                                                                        "selling_price," +
                                                                        "price," +
                                                                        "product_price_l," +
                                                                        "created_by," +
                                                                        "created_date," +
                                                                        "price_l" +
                                                                        ") VALUES (" +
                                                                        "'" + mssalesorderGID1 + "'," +
                                                                        "'" + mssalesorderGID + "'," +
                                                                        "'" + product_gid + "'," +
                                                                        "'" + productgroup_gid + "'," +
                                                                        "'" + item1.sku + "'," +
                                                                        "'" + item1.sku + "'," +
                                                                        "'" + item1.id + "'," +
                                                                        "'" + item.id + "'," +
                                                                        "'" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                        "'" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                        "'" + item1.price + "',";
                                                                if (item1.discount_allocations == null || item1.discount_allocations.Length == 0)
                                                                {

                                                                    price = (item1.quantity) * double.Parse(item1.price);
                                                                    msSQL += "'" + 0.00 + "',";
                                                                    msSQL += "'" + item1.quantity + "',";
                                                                }
                                                                else
                                                                {
                                                                    price = (item1.quantity) * double.Parse(item1.price) - double.Parse(item1.discount_allocations[0].amount);
                                                                    msSQL += "'" + item1.discount_allocations[0].amount + "',";
                                                                    msSQL += "'" + item1.quantity + "',";
                                                                }
                                                                msSQL += "'" + item1.taxable + "',";
                                                                if (item1.taxable == true)
                                                                {
                                                                    if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                    {
                                                                        msSQL += "'" + null + "',";
                                                                        msSQL += "'" + 0.00 + "',";
                                                                    }
                                                                    else
                                                                    {
                                                                        var tax_rates = (item1.tax_lines[0].rate * 100);
                                                                        if (tax_rates == 20)
                                                                        {
                                                                            msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE '%20%'";
                                                                            string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "'" + tax_gid + "',";
                                                                            msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE '%20%'";
                                                                            string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "'" + tax_namelines + "',";
                                                                            msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                        }
                                                                        else
                                                                        {

                                                                            msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE '%5%'";
                                                                            string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "'" + tax_gid + "',";
                                                                            msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE '%5%'";
                                                                            string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "'" + tax_namelines + "',";
                                                                            msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                    {
                                                                        msSQL += "'" + null + "',";
                                                                        msSQL += "'" + 0.00 + "',";
                                                                    }
                                                                    else
                                                                    {
                                                                        msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE'%VAT 0%'";
                                                                        string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_gid + "',";
                                                                        msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE'%VAT 0%'";
                                                                        string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_namelines + "',";
                                                                        msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                    }
                                                                }
                                                                msSQL = "'" + item1.price + "'," +
                                                                "'" + price + "'," +
                                                                "'" + item1.price + "'," +
                                                                "'" + lsemployee_gid + "'," +
                                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                "'" + price + "')";
                                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                            }
                                                            else
                                                            {
                                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while inserting new salesorderdtl from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                                        }

                                                    };
                                                }
                                                else
                                                {
                                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while inserting new salesorderdtl from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                                }
                                            }
                                        }
                                        else
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while inserting new customercontact from shopify order***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                        }
                                    }

                                    else
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********Erorr occured while inserting shopify order customerss ***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                    }
                                }
                                else if (customer_gid != null)
                                {
                                    if (item.customer.default_address.country != null)
                                    {
                                        msSQL = "select currencyexchange_gid from crm_trn_tcurrencyexchange where country = '" + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "' limit 1";
                                        string lscurrencyexchange_gid = objdbconn.GetExecuteScalar(msSQL);
                                    }
                                    else
                                    {
                                        lscurrencyexchange_gid = null;
                                    }
                                    if (item.customer.default_address.country != null)
                                    {
                                        msSQL = "Select country_gid from adm_mst_tcountry where country_name = '" + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'";
                                        lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
                                    }
                                    else
                                    {
                                        lscountry_gid = null;
                                    }
                                    msGETcustomercode = objcmnfunctions.GetMasterGID("CO");

                                    msSQL = "UPDATE crm_mst_tcustomer SET " +
                                        "currency_gid = '" + lscurrencyexchange_gid + "', ";

                                    if (item.customer.first_name != null)
                                    {
                                        if (item.customer.last_name != null)
                                        {
                                            msSQL += "customer_name = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "customer_name = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + "', ";
                                        }
                                    }
                                    else
                                    {
                                        msSQL += "customer_name = NULL, ";
                                    }
                                    if (item.customer.default_address.address1 != null)
                                    {
                                        msSQL += "customer_address = '" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_address = NULL, ";
                                    }
                                    if (item.customer.default_address.address2 != null)
                                    {
                                        msSQL += "customer_address2 = '" + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_address2 = NULL, ";
                                    }
                                    if (item.customer.default_address.country != null)
                                    {
                                        msSQL += "customer_country = '" + lscountry_gid + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_country = NULL, ";
                                    }
                                    if (item.customer.default_address.city != null)
                                    {
                                        msSQL += "customer_city = '" + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_city = NULL, ";
                                    }
                                    if (item.customer.default_address.zip != null)
                                    {
                                        msSQL += "customer_pin = '" + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_pin = NULL, ";
                                    }
                                    msSQL += "main_branch = 'Y', " +
                                        "status = 'Active', " +
                                        "updated_by = '" + user_gid + "', " +
                                        "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                                        "WHERE customer_gid = '" + customer_gid + "'";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        msSQL = "UPDATE crm_mst_tcustomercontact SET " +
                                             "customerbranch_name = 'H.Q', ";

                                        if (item.customer.first_name != null)
                                        {
                                            if (item.customer.last_name != null)
                                            {
                                                msSQL += "customercontact_name = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "', ";
                                            }
                                            else
                                            {
                                                msSQL += "customercontact_name = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + "', ";
                                            }
                                        }
                                        else
                                        {
                                            msSQL += "customercontact_name = NULL, ";
                                        }

                                        msSQL += "email = '" + item.email + "', " +
                                            "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                            "updated_by = '" + user_gid + "', ";

                                        if (item.customer.default_address.address1 != null)
                                        {
                                            msSQL += "address1 = '" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "address1 = NULL, ";
                                        }

                                        if (item.customer.default_address.address2 != null)
                                        {
                                            msSQL += "address2 = '" + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "address2 = NULL, ";
                                        }

                                        if (item.customer.default_address.country != null)
                                        {
                                            msSQL += "country_gid = '" + lscountry_gid + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "country_gid = NULL, ";
                                        }

                                        if (item.customer.default_address.city != null)
                                        {
                                            msSQL += "city = '" + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "city = NULL, ";
                                        }

                                        if (item.customer.default_address.zip != null)
                                        {
                                            msSQL += "zip_code = '" + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "zip_code = NULL, ";
                                        }

                                        if (item.customer.phone != null)
                                        {
                                            msSQL += "mobile = '" + item.customer.phone + "' ";
                                        }
                                        else
                                        {
                                            msSQL += "mobile = NULL ";
                                        }

                                        msSQL += "WHERE customer_gid = '" + customer_gid + "'";

                                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult2 == 1)
                                        {
                                            string formattedDate = "";
                                            mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP");

                                            string dateString = item.created_at.ToString();
                                            DateTimeOffset originalDate;

                                            if (DateTimeOffset.TryParseExact(dateString, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalDate))
                                            {
                                                formattedDate = originalDate.ToString("yyyy-MM-dd");
                                            }
                                            else if (DateTimeOffset.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:sszzz", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalDate))
                                            {
                                                formattedDate = originalDate.ToString("yyyy-MM-dd");
                                            }
                                            else if (DateTimeOffset.TryParseExact(dateString, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalDate))
                                            {
                                                formattedDate = originalDate.ToString("yyyy-MM-dd");
                                            }

                                            msSQL = "select branch_gid from hrm_mst_tbranch limit 1;";
                                            string branch_gid = objdbconn.GetExecuteScalar(msSQL);

                                            msSQL = "select exchange_rate from crm_trn_tcurrencyexchange where currency_code='" + item.currency + "';";
                                            string exchange_rate = objdbconn.GetExecuteScalar(msSQL);
                                            msSQL = "INSERT INTO cmr_smm_tshopifysalesorder (" +
                                                    "salesorder_gid," +
                                                    "branch_gid," +
                                                    "shopify_orderid," +
                                                    "salesorder_date," +
                                                    "shopifyorder_number," +
                                                    "shopify_customerid," +
                                                    "customer_gid," +
                                                    "customer_name," +
                                                    "customer_contact_person," +
                                                    "created_by," +
                                                    "created_date," +
                                                    "additional_discount," +
                                                    "Grandtotal," +
                                                    "salesorder_status," +
                                                    "addon_charge," +
                                                    "grandtotal_l," +
                                                    "exchange_rate," +
                                                    "currency_code," +
                                                    "customer_mobile," +
                                                    "customer_address," +
                                                    "shipping_to," +
                                                    "billing_to," +
                                                    "total_price," +
                                                    "fulfillment_status," +
                                                    "customer_email" +
                                                    ") VALUES (" +
                                                    "'" + mssalesorderGID + "'," +
                                                    "'" + branch_gid + "'," +
                                                    "'" + item.id + "'," +
                                                    "'" + formattedDate + "'," +
                                                    "'" + item.name + "'," +
                                                    "'" + item.customer.id + "'," +
                                                    "'" + customer_gid + "'," +
                                                    "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                                    "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                                    "'" + lsemployee_gid + "'," +
                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";

                                            msSQL += "'" + item.total_discounts_set.shop_money.amount + "'," +
                                                "'" + item.total_price + "'," +
                                                "'" + item.financial_status + "'," +
                                                "'" + item.total_shipping_price_set.shop_money.amount + "'," +
                                                "'" + item.total_price + "'," +
                                                "'" + exchange_rate+ "'," +
                                                "'" + item.currency + "',";
                                            if (item.customer.phone == null || item.customer == null)
                                            {
                                                msSQL += "'" + null + "',";
                                            }
                                            else
                                            {
                                                msSQL += "'" + item.customer.phone + "',";
                                            }

                                            if (item.customer == null || item.customer.default_address == null)
                                            {
                                                msSQL += "'" + null + "',";
                                            }
                                            else
                                            {
                                                if (item.customer.default_address.address2 == null)
                                                {
                                                    msSQL += "'" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                                else
                                                {
                                                    msSQL += "'" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                            }

                                            if (item.shipping_address == null || item.shipping_address.address1 == null)
                                            {
                                                msSQL += "'" + null + "',";
                                            }
                                            else
                                            {
                                                if (item.shipping_address.address2 == null)
                                                {
                                                    msSQL += "'" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                                else
                                                {
                                                    msSQL += "'" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.shipping_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                            }

                                            if (item.billing_address == null || item.billing_address.address1 == null)
                                            {
                                                msSQL += "'" + null + "',";
                                            }
                                            else
                                            {
                                                if (item.billing_address.address2 == null)
                                                {
                                                    msSQL += "'" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                                else
                                                {
                                                    msSQL += "'" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.billing_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                }
                                            }


                                            msSQL += "'" + item.total_line_items_price + "'," +
                                                     "'" + item.fulfillment_status + "'," +
                                                     "'" + item.email + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult == 1)
                                            {
                                                msSQL = "INSERT INTO smr_trn_tsalesorder (" +
                                                        "salesorder_gid," +
                                                        "branch_gid," +
                                                        "shopify_orderid," +
                                                        "so_referenceno1," +
                                                        "salesorder_date," +
                                                        "shopifyorder_number," +
                                                        "shopify_customerid," +
                                                        "customer_gid," +
                                                        "customer_name," +
                                                        "customer_contact_person," +
                                                        "created_by," +
                                                        "created_date," +
                                                        "additional_discount," +
                                                        "Grandtotal," +
                                                        "salesorder_status," +
                                                        "addon_charge," +
                                                        "grandtotal_l," +
                                                        "exchange_rate," +
                                                        "currency_code," +
                                                        "customer_mobile," +
                                                        "customer_address," +
                                                        "shipping_to," +
                                                        "billing_to," +
                                                        "total_price," +
                                                        "customer_email," +
                                                        "fulfillment_status," +
                                                        "source_flag" +
                                                        ") VALUES (" +
                                                        "'" + mssalesorderGID + "'," +
                                                        "'" + branch_gid + "'," +
                                                        "'" + item.id + "'," +
                                                        "'" + item.name + "'," +
                                                        "'" + formattedDate + "'," +
                                                        "'" + item.name + "'," +
                                                        "'" + item.customer.id + "'," +
                                                        "'" + customer_gid + "'," +
                                                        "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                                        "'" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                                        "'" + lsemployee_gid + "'," +
                                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                                                msSQL += "'" + item.total_discounts_set.shop_money.amount + "'," +
                                                    "'" + item.total_price + "'," +
                                                    "'" + item.financial_status + "'," +
                                                    "'" + item.total_shipping_price_set.shop_money.amount + "'," +
                                                    "'" + item.total_price + "'," +
                                                    "'" + exchange_rate + "'," +
                                                    "'" + item.currency + "',";

                                                if (item.customer.phone == null || item.customer == null)
                                                {
                                                    msSQL += "'" + null + "',";
                                                }
                                                else
                                                {
                                                    msSQL += "'" + item.customer.phone + "',";
                                                }

                                                if (item.customer == null || item.customer.default_address == null)
                                                {
                                                    msSQL += "'" + null + "',";
                                                }
                                                else
                                                {
                                                    if (item.customer.default_address.address2 == null)
                                                    {
                                                        msSQL += "'" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                    else
                                                    {
                                                        msSQL += "'" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                }

                                                if (item.shipping_address == null || item.shipping_address.address1 == null)
                                                {
                                                    msSQL += "'" + null + "',";
                                                }
                                                else
                                                {
                                                    if (item.shipping_address.address2 == null)
                                                    {
                                                        msSQL += "'" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                    else
                                                    {
                                                        msSQL += "'" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.shipping_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                }

                                                if (item.billing_address == null || item.billing_address.address1 == null)
                                                {
                                                    msSQL += "'" + null + "',";
                                                }
                                                else
                                                {
                                                    if (item.billing_address.address2 == null)
                                                    {
                                                        msSQL += "'" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                    else
                                                    {
                                                        msSQL += "'" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + " , " + item.billing_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "',";

                                                    }
                                                }

                                                msSQL += "'" + item.total_line_items_price + "',";
                                                msSQL += "'" + item.email + "',";
                                                msSQL += "'" + item.fulfillment_status + "',";
                                                msSQL += "'S')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                if (mnResult != 0)
                                                {
                                                    //await Task.WhenAll(item.line_items.Select(async item1 =>
                                                    //{
                                                    foreach (var item1 in item.line_items)
                                                    {
                                                        try
                                                        {


                                                            msSQL = "SELECT productgroup_gid FROM pmr_mst_tproduct WHERE customerproduct_code = '" + item1.sku + "'";
                                                            string productgroup_gid = objdbconn.GetExecuteScalar(msSQL);
                                                            msSQL = "SELECT product_gid FROM pmr_mst_tproduct WHERE customerproduct_code = '" + item1.sku + "'";
                                                            string product_gid = objdbconn.GetExecuteScalar(msSQL);

                                                            mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                                                            msSQL = "INSERT INTO cmr_smm_tshopifysalesorderdtl (" +
                                                                   "salesorderdtl_gid," +
                                                                   "salesorder_gid," +
                                                                   "product_gid," +
                                                                   "productgroup_gid," +
                                                                   "product_code," +
                                                                   "customerproduct_code," +
                                                                   "shopify_lineitemid," +
                                                                   "shopify_orderid," +
                                                                   "product_name," +
                                                                   "display_field," +
                                                                   "product_price," +
                                                                   "discount_amount," +
                                                                   "qty_quoted," +
                                                                   "taxable," +
                                                                   "tax1_gid," +
                                                                   "tax_name," +
                                                                   "tax_amount," +
                                                                   "selling_price," +
                                                                   "price," +
                                                                   "product_price_l," +
                                                                   "created_by," +
                                                                   "created_date," +
                                                                   "price_l" +
                                                                   ") VALUES (" +
                                                                   "'" + mssalesorderGID1 + "'," +
                                                                   "'" + mssalesorderGID + "'," +
                                                                   "'" + product_gid + "'," +
                                                                   "'" + productgroup_gid + "'," +
                                                                   "'" + item1.sku + "'," +
                                                                   "'" + item1.sku + "'," +
                                                                   "'" + item1.id + "'," +
                                                                   "'" + item.id + "'," +
                                                                   "'" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                   "'" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                   "'" + item1.price + "',";
                                                            if (item1.discount_allocations == null || item1.discount_allocations.Length == 0)
                                                            {
                                                                price = (item1.quantity) * double.Parse(item1.price);
                                                                msSQL += "'" + 0.00 + "',";
                                                                msSQL += "'" + item1.quantity + "',";
                                                            }
                                                            else
                                                            {
                                                                price = (item1.quantity) * double.Parse(item1.price) - double.Parse(item1.discount_allocations[0].amount);
                                                                msSQL += "'" + item1.discount_allocations[0].amount + "',";
                                                                msSQL += "'" + item1.quantity + "',";
                                                            }

                                                            msSQL += "'" + item1.taxable + "',";
                                                            if (item1.taxable == true)
                                                            {
                                                                if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                {
                                                                    msSQL += "'" + null + "',";
                                                                    msSQL += "'" + 0.00 + "',";
                                                                }
                                                                else
                                                                {
                                                                    var tax_rates = (item1.tax_lines[0].rate * 100);
                                                                    if (tax_rates == 20)
                                                                    {
                                                                        msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE '%20%'";
                                                                        string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_gid + "',";
                                                                        msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE '%20%'";
                                                                        string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_namelines + "',";
                                                                        msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                    }
                                                                    else
                                                                    {

                                                                        msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE '%5%'";
                                                                        string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_gid + "',";
                                                                        msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE '%5%'";
                                                                        string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_namelines + "',";
                                                                        msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                {
                                                                    msSQL += "'" + null + "',";
                                                                    msSQL += "'" + 0.00 + "',";
                                                                }
                                                                else
                                                                {
                                                                    msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE'%VAT 0%'";
                                                                    string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                    msSQL += "'" + tax_gid + "',";
                                                                    msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE'%VAT 0%'";
                                                                    string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                    msSQL += "'" + tax_namelines + "',";
                                                                    msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                }
                                                            }
                                                            msSQL += "'" + item1.price + "'," +
                                                                 "'" + price + "'," +
                                                            "'" + item1.price + "'," +
                                                            "'" + lsemployee_gid + "'," +
                                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                            "'" + price + "')";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                            if (mnResult == 1)
                                                            {
                                                                msSQL = "INSERT INTO smr_trn_tsalesorderdtl (" +
                                                                        "salesorderdtl_gid," +
                                                                        "salesorder_gid," +
                                                                        "product_gid," +
                                                                        "productgroup_gid," +
                                                                        "product_code," +
                                                                        "customerproduct_code," +
                                                                        "shopify_lineitemid," +
                                                                        "shopify_orderid," +
                                                                        "product_name," +
                                                                        "display_field," +
                                                                        "product_price," +
                                                                        "discount_amount," +
                                                                        "qty_quoted," +
                                                                        "taxable," +
                                                                   "tax1_gid," +
                                                                        "tax_name," +
                                                                        "tax_amount," +
                                                                        "selling_price," +
                                                                        "price," +
                                                                        "product_price_l," +
                                                                        "created_by," +
                                                                        "created_date," +
                                                                        "price_l" +
                                                                        ") VALUES (" +
                                                                        "'" + mssalesorderGID1 + "'," +
                                                                        "'" + mssalesorderGID + "'," +
                                                                        "'" + product_gid + "'," +
                                                                        "'" + productgroup_gid + "'," +
                                                                        "'" + item1.sku + "'," +
                                                                        "'" + item1.sku + "'," +
                                                                        "'" + item1.id + "'," +
                                                                        "'" + item.id + "'," +
                                                                        "'" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                        "'" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                        "'" + item1.price + "',";
                                                                if (item1.discount_allocations == null || item1.discount_allocations.Length == 0)
                                                                {

                                                                    price = (item1.quantity) * double.Parse(item1.price);
                                                                    msSQL += "'" + 0.00 + "',";
                                                                    msSQL += "'" + item1.quantity + "',";
                                                                }
                                                                else
                                                                {
                                                                    price = (item1.quantity) * double.Parse(item1.price) - double.Parse(item1.discount_allocations[0].amount);
                                                                    msSQL += "'" + item1.discount_allocations[0].amount + "',";
                                                                    msSQL += "'" + item1.quantity + "',";
                                                                }

                                                                msSQL += "'" + item1.taxable + "',";
                                                                if (item1.taxable == true)
                                                                {
                                                                    if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                    {
                                                                        msSQL += "'" + null + "',";
                                                                        msSQL += "'" + 0.00 + "',";
                                                                    }
                                                                    else
                                                                    {
                                                                        var tax_rates = (item1.tax_lines[0].rate * 100);
                                                                        if (tax_rates == 20)
                                                                        {
                                                                            msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE '%20%'";
                                                                            string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "'" + tax_gid + "',";
                                                                            msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE '%20%'";
                                                                            string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "'" + tax_namelines + "',";
                                                                            msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                        }
                                                                        else
                                                                        {

                                                                            msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE '%5%'";
                                                                            string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "'" + tax_gid + "',";
                                                                            msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE '%5%'";
                                                                            string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "'" + tax_namelines + "',";
                                                                            msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                    {
                                                                        msSQL += "'" + null + "',";
                                                                        msSQL += "'" + 0.00 + "',";
                                                                    }
                                                                    else
                                                                    {
                                                                        msSQL1 = "select tax_gid from acp_mst_ttax where tax_prefix LIKE'%VAT 0%'";
                                                                        string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_gid + "',";
                                                                        msSQL1 = "select tax_name from acp_mst_ttax where tax_prefix LIKE'%VAT 0%'";
                                                                        string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "'" + tax_namelines + "',";
                                                                        msSQL += "'" + item1.tax_lines[0].price + "',";
                                                                    }
                                                                }
                                                                msSQL += "'" + item1.price + "'," +
                                                                "'" + price + "'," +
                                                                "'" + item1.price + "'," +
                                                                "'" + lsemployee_gid + "'," +
                                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                "'" + price + "')";
                                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                            }
                                                            else
                                                            {
                                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while inserting existing customer salesorderdtl from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                                        }

                                                    };
                                                }

                                            }
                                            else
                                            {
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while inserting existing customer salesorder from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                            }
                                        }
                                        else
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while updating existing salesorder customer from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                        }
                                    }

                                }
                                else
                                {
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while inserting new salesorder from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                }

                            }
                            else
                            {
                                if(item.fulfillment_status != "fulfilled")
                                {

                                msSQL = "SELECT employee_gid FROM hrm_mst_temployee WHERE user_gid = '" + user_gid + "'";
                                string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = "SELECT customer_gid FROM crm_mst_tcustomer WHERE shopify_id = '" + item.customer.id + "'";
                                customer_gid = objdbconn.GetExecuteScalar(msSQL);

                                  
                                    if (item.customer.default_address.country != null)
                                    {
                                        msSQL = "select currencyexchange_gid from crm_trn_tcurrencyexchange where country = '" + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "' limit 1";
                                        string lscurrencyexchange_gid = objdbconn.GetExecuteScalar(msSQL);
                                    }
                                    else
                                    {
                                        lscurrencyexchange_gid = null;
                                    }
                                    if (item.customer.default_address.country != null)
                                    {
                                        msSQL = "Select country_gid from adm_mst_tcountry where country_name = '" + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'";
                                        lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
                                    }
                                    else
                                    {
                                        lscountry_gid = null;
                                    }
                                    msGETcustomercode = objcmnfunctions.GetMasterGID("CO");

                                    msSQL = "UPDATE crm_mst_tcustomer SET " +
                                        "currency_gid = '" + lscurrencyexchange_gid + "', ";

                                    if (item.customer.first_name != null)
                                    {
                                        if (item.customer.last_name != null)
                                        {
                                            msSQL += "customer_name = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "customer_name = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + "', ";
                                        }
                                    }
                                    else
                                    {
                                        msSQL += "customer_name = NULL, ";
                                    }
                                    if (item.customer.default_address.address1 != null)
                                    {
                                        msSQL += "customer_address = '" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_address = NULL, ";
                                    }
                                    if (item.customer.default_address.address2 != null)
                                    {
                                        msSQL += "customer_address2 = '" + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_address2 = NULL, ";
                                    }
                                    if (item.customer.default_address.country != null)
                                    {
                                        msSQL += "customer_country = '" + lscountry_gid + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_country = NULL, ";
                                    }
                                    if (item.customer.default_address.city != null)
                                    {
                                        msSQL += "customer_city = '" + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_city = NULL, ";
                                    }
                                    if (item.customer.default_address.zip != null)
                                    {
                                        msSQL += "customer_pin = '" + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "customer_pin = NULL, ";
                                    }
                                    msSQL += "main_branch = 'Y', " +
                                        "status = 'Active', " +
                                        "updated_by = '" + user_gid + "', " +
                                        "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                                        "WHERE customer_gid = '" + customer_gid + "'";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        msSQL = "UPDATE crm_mst_tcustomercontact SET " +
                                             "customerbranch_name = 'H.Q', ";

                                        if (item.customer.first_name != null)
                                        {
                                            if (item.customer.last_name != null)
                                            {
                                                msSQL += "customercontact_name = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "', ";
                                            }
                                            else
                                            {
                                                msSQL += "customercontact_name = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + "', ";
                                            }
                                        }
                                        else
                                        {
                                            msSQL += "customercontact_name = NULL, ";
                                        }

                                        msSQL += "email = '" + item.email + "', " +
                                            "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                            "updated_by = '" + user_gid + "', ";

                                        if (item.customer.default_address.address1 != null)
                                        {
                                            msSQL += "address1 = '" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "address1 = NULL, ";
                                        }

                                        if (item.customer.default_address.address2 != null)
                                        {
                                            msSQL += "address2 = '" + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "address2 = NULL, ";
                                        }

                                        if (item.customer.default_address.country != null)
                                        {
                                            msSQL += "country_gid = '" + lscountry_gid + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "country_gid = NULL, ";
                                        }

                                        if (item.customer.default_address.city != null)
                                        {
                                            msSQL += "city = '" + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "city = NULL, ";
                                        }

                                        if (item.customer.default_address.zip != null)
                                        {
                                            msSQL += "zip_code = '" + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "zip_code = NULL, ";
                                        }

                                        if (item.customer.phone != null)
                                        {
                                            msSQL += "mobile = '" + item.customer.phone + "' ";
                                        }
                                        else
                                        {
                                            msSQL += "mobile = NULL ";
                                        }

                                        msSQL += "WHERE customer_gid = '" + customer_gid + "'";

                                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            // mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP");
                                            string formattedDate = "";
                                            if (item.created_at != null && !string.IsNullOrEmpty(item.created_at.ToString()))
                                            {
                                                string dateString = item.created_at.ToString();
                                                DateTimeOffset originalDate;

                                                if (DateTimeOffset.TryParseExact(dateString, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalDate))
                                                {
                                                    formattedDate = originalDate.ToString("yyyy-MM-dd");
                                                }
                                                else if (DateTimeOffset.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:sszzz", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalDate))
                                                {
                                                    formattedDate = originalDate.ToString("yyyy-MM-dd");
                                                }
                                                else if (DateTimeOffset.TryParseExact(dateString, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalDate))
                                                {
                                                    formattedDate = originalDate.ToString("yyyy-MM-dd");
                                                }

                                            }


                                            msSQL = "select branch_gid from hrm_mst_tbranch limit 1;";
                                            string branch_gid = objdbconn.GetExecuteScalar(msSQL);

                                            msSQL = "select exchange_rate from crm_trn_tcurrencyexchange where currency_code='" + item.currency + "';";
                                            string exchange_rate = objdbconn.GetExecuteScalar(msSQL);
                                        msSQL = "UPDATE cmr_smm_tshopifysalesorder SET " +
                                            //"branch_gid = '" + branch_gid + "'," +
                                          //  "shopify_orderid = '" + item.id + "'," +
                                            "salesorder_date = '" + formattedDate + "'," +
                                            "shopifyorder_number = '" + item.name + "'," +
                                            "shopify_customerid = '" + item.customer.id + "'," +
                                           // "customer_gid = '" + customer_gid + "'," +
                                           // "customer_name = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                            "customer_contact_person = '" + item.customer.first_name.ToString().Replace("'", "").Replace("'", "") + " " + item.customer.last_name.ToString().Replace("'", "").Replace("'", "") + "'," +
                                            "updated_by = '" + lsemployee_gid + "'," +
                                            "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "additional_discount = '" + item.total_discounts_set.shop_money.amount + "'," +
                                            "Grandtotal = '" + item.total_price + "'," +
                                            "salesorder_status = '" + item.financial_status + "'," +
                                            "addon_charge = '" + item.total_shipping_price_set.shop_money.amount + "'," +
                                            "grandtotal_l = '" + item.total_price + "'," +
                                            "exchange_rate = '" + exchange_rate + "'," +
                                            "currency_code = '" + item.currency + "',";

                                        if (item.customer.phone == null || item.customer == null)
                                        {
                                            msSQL += "customer_mobile = NULL,";
                                        }
                                        else
                                        {
                                            msSQL += "customer_mobile = '" + item.customer.phone + "',";
                                        }

                                        if (item.customer == null || item.customer.default_address == null)
                                        {
                                            msSQL += "customer_address = NULL,";
                                        }
                                        else
                                        {
                                            if (item.customer.default_address.address2 == null)
                                            {
                                                msSQL += "customer_address = '" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",") + "',";
                                            }
                                            else
                                            {
                                                msSQL += "customer_address = '" + item.customer.default_address.address1.ToString().Replace("'", " ").Replace("，", ",") + " , " + item.customer.default_address.address2.ToString().Replace("'", " ").Replace("，", ",") + " , " + item.customer.default_address.city.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.customer.default_address.country.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.customer.default_address.zip.ToString().Replace("'", " ").Replace("，", ",") + "',";
                                            }
                                        }

                                        if (item.shipping_address == null || item.shipping_address.address1 == null)
                                        {
                                            msSQL += "shipping_to = NULL,";
                                        }
                                        else
                                        {
                                            if (item.shipping_address.address2 == null)
                                            {
                                                msSQL += "shipping_to = '" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",") + " , " + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",") + "',";
                                            }
                                            else
                                            {
                                                msSQL += "shipping_to = '" + item.shipping_address.address1.ToString().Replace("'", " ").Replace("，", ",") + " , " + item.shipping_address.address2.ToString().Replace("'", " ").Replace("，", ",") + item.shipping_address.city.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.shipping_address.country.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.shipping_address.zip.ToString().Replace("'", " ").Replace("，", ",") + "',";
                                            }
                                        }

                                        if (item.billing_address == null || item.billing_address.address1 == null)
                                        {
                                            msSQL += "billing_to = NULL,";
                                        }
                                        else
                                        {
                                            if (item.billing_address.address2 == null)
                                            {
                                                msSQL += "billing_to = '" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",") + "',";
                                            }
                                            else
                                            {
                                                msSQL += "billing_to = '" + item.billing_address.address1.ToString().Replace("'", " ").Replace("，", ",") + " , " + item.billing_address.address2.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.billing_address.city.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.billing_address.country.ToString().Replace("'", " ").Replace("，", ",") + ", " + item.billing_address.zip.ToString().Replace("'", " ").Replace("，", ",") + "',";
                                            }
                                        }

                                        msSQL += "total_price = '" + item.total_line_items_price + "'," +
                                            "fulfillment_status = '" + item.fulfillment_status + "'," +
                                            "customer_email = '" + item.email + "' " +
                                            "WHERE shopify_orderid = '" + item.id + "'"; 

                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult == 1)
                                            {
                                                msSQL = "UPDATE smr_trn_tsalesorder SET " +
                                                //"branch_gid = '" + branch_gid + "'," +
                                                //"shopify_orderid = '" + item.id + "'," +
                                                "so_referenceno1 = '" + item.name + "'," +
                                                "salesorder_date = '" + formattedDate + "'," +
                                                "shopifyorder_number = '" + item.name + "'," +
                                                "shopify_customerid = '" + item.customer.id + "'," +
                                               // "customer_gid = '" + customer_gid + "'," +
                                               // "customer_name = '" + item.customer.first_name.Replace("'", "") + " " + item.customer.last_name.Replace("'", "") + "'," +
                                                "customer_contact_person = '" + item.customer.first_name.Replace("'", "") + " " + item.customer.last_name.Replace("'", "") + "'," +
                                                "updated_by = '" + lsemployee_gid + "'," +
                                                "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                "additional_discount = '" + item.total_discounts_set.shop_money.amount + "'," +
                                                "Grandtotal = '" + item.total_price + "'," +
                                                "salesorder_status = '" + item.financial_status + "'," +
                                                "addon_charge = '" + item.total_shipping_price_set.shop_money.amount + "'," +
                                                "grandtotal_l = '" + item.total_price + "'," +
                                                "exchange_rate = '" + exchange_rate + "'," +
                                                "currency_code = '" + item.currency + "'," +
                                                "customer_mobile = " + (item.customer.phone == null || item.customer == null ? "NULL" : "'" + item.customer.phone + "'") + "," +
                                                "customer_address = " + (item.customer == null || item.customer.default_address == null
                                                    ? "NULL"
                                                    : "'" + (item.customer.default_address.address1 + " " + (item.customer.default_address.address2 ?? "") + ", " + item.customer.default_address.city + ", " + item.customer.default_address.country + ", " + item.customer.default_address.zip).Replace("'", " ") + "'") + "," +
                                                "shipping_to = " + (item.shipping_address == null || item.shipping_address.address1 == null
                                                    ? "NULL"
                                                    : "'" + (item.shipping_address.address1 + " " + (item.shipping_address.address2 ?? "") + ", " + item.shipping_address.city + ", " + item.shipping_address.country + ", " + item.shipping_address.zip).Replace("'", " ") + "'") + "," +
                                                "billing_to = " + (item.billing_address == null || item.billing_address.address1 == null
                                                    ? "NULL"
                                                    : "'" + (item.billing_address.address1 + " " + (item.billing_address.address2 ?? "") + ", " + item.billing_address.city + ", " + item.billing_address.country + ", " + item.billing_address.zip).Replace("'", " ") + "'") + "," +
                                                "total_price = '" + item.total_line_items_price + "'," +
                                                "customer_email = '" + item.email + "'," +
                                                "fulfillment_status = '" + item.fulfillment_status + "'," +
                                                "source_flag = 'S' " +
                                                "WHERE shopify_orderid = '" + item.id + "'";

                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                if (mnResult != 0)
                                                {
                                                    foreach (var item1 in item.line_items)
                                                    {
                                                        try
                                                        {
                                                            msSQL = "SELECT productgroup_gid FROM pmr_mst_tproduct WHERE customerproduct_code = '" + item1.sku + "'";
                                                            string productgroup_gid = objdbconn.GetExecuteScalar(msSQL);
                                                            msSQL = "SELECT product_gid FROM pmr_mst_tproduct WHERE customerproduct_code = '" + item1.sku + "'";
                                                            string product_gid = objdbconn.GetExecuteScalar(msSQL);

                                                            //mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                                                            msSQL = "UPDATE cmr_smm_tshopifysalesorderdtl SET " +
                                                              "product_gid = '" + product_gid + "'," +
                                                              "productgroup_gid = '" + productgroup_gid + "'," +
                                                              "product_code = '" + item1.sku + "'," +
                                                              "customerproduct_code = '" + item1.sku + "'," +
                                                              "shopify_lineitemid = '" + item1.id + "'," +
                                                              "shopify_orderid = '" + item.id + "'," +
                                                              "product_name = '" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                              "display_field = '" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                              "product_price = '" + item1.price + "',";

                                                            if (item1.discount_allocations == null || item1.discount_allocations.Length == 0)
                                                            {
                                                                price = (item1.quantity) * double.Parse(item1.price);
                                                                msSQL += "discount_amount = '0.00'," +
                                                                         "qty_quoted = '" + item1.quantity + "',";
                                                            }
                                                            else
                                                            {
                                                                price = (item1.quantity) * double.Parse(item1.price) - double.Parse(item1.discount_allocations[0].amount);
                                                                msSQL += "discount_amount = '" + item1.discount_allocations[0].amount + "'," +
                                                                         "qty_quoted = '" + item1.quantity + "',";
                                                            }

                                                            msSQL += "taxable = '" + item1.taxable + "',";

                                                            if (item1.taxable)
                                                            {
                                                                if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                {
                                                                    msSQL += "tax1_gid = NULL," +
                                                                             "tax_name = NULL," +
                                                                             "tax_amount = '0.00',";
                                                                }
                                                                else
                                                                {
                                                                    var tax_rates = (item1.tax_lines[0].rate * 100);
                                                                    if (tax_rates == 20)
                                                                    {
                                                                        msSQL1 = "SELECT tax_gid FROM acp_mst_ttax WHERE tax_prefix LIKE '%20%'";
                                                                        string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "tax1_gid = '" + tax_gid + "',";
                                                                        msSQL1 = "SELECT tax_name FROM acp_mst_ttax WHERE tax_prefix LIKE '%20%'";
                                                                        string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "tax_name = '" + tax_namelines + "'," +
                                                                                 "tax_amount = '" + item1.tax_lines[0].price + "',";
                                                                    }
                                                                    else
                                                                    {
                                                                        msSQL1 = "SELECT tax_gid FROM acp_mst_ttax WHERE tax_prefix LIKE '%5%'";
                                                                        string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "tax1_gid = '" + tax_gid + "',";
                                                                        msSQL1 = "SELECT tax_name FROM acp_mst_ttax WHERE tax_prefix LIKE '%5%'";
                                                                        string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "tax_name = '" + tax_namelines + "'," +
                                                                                 "tax_amount = '" + item1.tax_lines[0].price + "',";
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                {
                                                                    msSQL += "tax1_gid = NULL," +
                                                                             "tax_name = NULL," +
                                                                             "tax_amount = '0.00',";
                                                                }
                                                                else
                                                                {
                                                                    msSQL1 = "SELECT tax_gid FROM acp_mst_ttax WHERE tax_prefix LIKE '%VAT 0%'";
                                                                    string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                    msSQL += "tax1_gid = '" + tax_gid + "',";
                                                                    msSQL1 = "SELECT tax_name FROM acp_mst_ttax WHERE tax_prefix LIKE '%VAT 0%'";
                                                                    string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                    msSQL += "tax_name = '" + tax_namelines + "'," +
                                                                             "tax_amount = '" + item1.tax_lines[0].price + "',";
                                                                }
                                                            }

                                                            msSQL += "selling_price = '" + item1.price + "'," +
                                                                     "price = '" + price + "'," +
                                                                     "product_price_l = '" + item1.price + "'," +
                                                                     "updated_by = '" + lsemployee_gid + "'," +
                                                                     "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                     "price_l = '" + price + "' " +
                                                                     "WHERE shopify_orderid = '" + item.id + "'";

                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                            if (mnResult == 1)
                                                            {
                                                                msSQL = "UPDATE smr_trn_tsalesorderdtl SET " +
                                                                         //"salesorder_gid = '" + mssalesorderGID + "'," +
                                                                         "product_gid = '" + product_gid + "'," +
                                                                         "productgroup_gid = '" + productgroup_gid + "'," +
                                                                         "product_code = '" + item1.sku + "'," +
                                                                         "customerproduct_code = '" + item1.sku + "'," +
                                                                         "shopify_lineitemid = '" + item1.id + "'," +
                                                                         "shopify_orderid = '" + item.id + "'," +
                                                                         "product_name = '" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                         "display_field = '" + item1.name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "'," +
                                                                         "product_price = '" + item1.price + "',";

                                                                if (item1.discount_allocations == null || item1.discount_allocations.Length == 0)
                                                                {
                                                                    price = (item1.quantity) * double.Parse(item1.price);
                                                                    msSQL += "discount_amount = '0.00'," +
                                                                             "qty_quoted = '" + item1.quantity + "',";
                                                                }
                                                                else
                                                                {
                                                                    price = (item1.quantity) * double.Parse(item1.price) - double.Parse(item1.discount_allocations[0].amount);
                                                                    msSQL += "discount_amount = '" + item1.discount_allocations[0].amount + "'," +
                                                                             "qty_quoted = '" + item1.quantity + "',";
                                                                }

                                                                msSQL += "taxable = '" + item1.taxable + "',";

                                                                if (item1.taxable)
                                                                {
                                                                    if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                    {
                                                                        msSQL += "tax1_gid = NULL," +
                                                                                 "tax_name = NULL," +
                                                                                 "tax_amount = '0.00',";
                                                                    }
                                                                    else
                                                                    {
                                                                        var tax_rates = (item1.tax_lines[0].rate * 100);
                                                                        if (tax_rates == 20)
                                                                        {
                                                                            msSQL1 = "SELECT tax_gid FROM acp_mst_ttax WHERE tax_prefix LIKE '%20%'";
                                                                            string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "tax1_gid = '" + tax_gid + "',";
                                                                            msSQL1 = "SELECT tax_name FROM acp_mst_ttax WHERE tax_prefix LIKE '%20%'";
                                                                            string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "tax_name = '" + tax_namelines + "'," +
                                                                                     "tax_amount = '" + item1.tax_lines[0].price + "',";
                                                                        }
                                                                        else
                                                                        {
                                                                            msSQL1 = "SELECT tax_gid FROM acp_mst_ttax WHERE tax_prefix LIKE '%5%'";
                                                                            string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "tax1_gid = '" + tax_gid + "',";
                                                                            msSQL1 = "SELECT tax_name FROM acp_mst_ttax WHERE tax_prefix LIKE '%5%'";
                                                                            string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                            msSQL += "tax_name = '" + tax_namelines + "'," +
                                                                                     "tax_amount = '" + item1.tax_lines[0].price + "',";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (item1.tax_lines == null || item1.tax_lines.Length == 0)
                                                                    {
                                                                        msSQL += "tax1_gid = NULL," +
                                                                                 "tax_name = NULL," +
                                                                                 "tax_amount = '0.00',";
                                                                    }
                                                                    else
                                                                    {
                                                                        msSQL1 = "SELECT tax_gid FROM acp_mst_ttax WHERE tax_prefix LIKE '%VAT 0%'";
                                                                        string tax_gid = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "tax1_gid = '" + tax_gid + "',";
                                                                        msSQL1 = "SELECT tax_name FROM acp_mst_ttax WHERE tax_prefix LIKE '%VAT 0%'";
                                                                        string tax_namelines = objdbconn.GetExecuteScalar(msSQL1);
                                                                        msSQL += "tax_name = '" + tax_namelines + "'," +
                                                                                 "tax_amount = '" + item1.tax_lines[0].price + "',";
                                                                    }
                                                                }

                                                                msSQL += "selling_price = '" + item1.price + "'," +
                                                                         "price = '" + price + "'," +
                                                                         "product_price_l = '" + item1.price + "'," +
                                                                         "updated_by = '" + lsemployee_gid + "'," +
                                                                         "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                         "price_l = '" + price + "' " +
                                                                         "WHERE shopify_orderid = '" + item.id + "'";

                                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                            }
                                                            else
                                                            {
                                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while updating existing salesorderdtl  from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                                        }

                                                    };
                                                }
                                            }
                                            else
                                            {
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while updating existing salesorder  from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                            }
                                        }
                                        else
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while updating existing salesorder customer from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                        }

                                    }
                                    else
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "*********** Error occur while updating existing salesorder from shopify***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                    }


                                }

                            }
                            objOdbcDataReader.Close();

                        }

                        catch (Exception ex)
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    };

                    // Get the next page URL from the Link header
                    url = null;
                    if (response.Headers != null)
                    {
                        foreach (var header in response.Headers)
                        {
                            if (header.Name == "link")
                            {
                                var links = header.Value.ToString().Split(',');
                                foreach (var link in links)
                                {
                                    var linkParts = link.Split(';');
                                    if (linkParts.Length == 2 && linkParts[1].Contains("rel=\"next\""))
                                    {
                                        url = linkParts[0].Trim().Trim('<', '>');
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

            return objresult;
        }
        public void DaGetShopifyOrderSummary(MdlShopifyCustomer values)
        {
            try
            {


                msSQL = "select a.customer_address,a.salesorder_gid,a.shopifyorder_number, " +
                    "date_format(a.salesorder_date,'%d-%m-%Y') as salesorder_date,a.shopify_orderid,a.customer_contact_person," +
                    " a.Grandtotal,a.salesorder_status,(case when a.shopify_orderid=b.shopify_orderid then sum(b.qty_quoted) else sum(b.qty_quoted) end  ) as item_count" +
                    " from cmr_smm_tshopifysalesorder a left join cmr_smm_tshopifysalesorderdtl b on a.salesorder_gid=b.salesorder_gid GROUP BY  a.customer_address," +
                    "a.salesorder_gid,a.shopifyorder_number, a.salesorder_date,a.shopify_orderid,a.customer_contact_person," +
                    "a.Grandtotal,a.salesorder_status order by a.shopifyorder_number desc;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<shopifyordersummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new shopifyordersummary_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            shopifyorder_number = dt["shopifyorder_number"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            shopify_orderid = dt["shopify_orderid"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            item_count = dt["item_count"].ToString(),


                        });
                        values.shopifyordersummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Shopify OrderSummary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaViewShopifyOrderSummary(MdlShopifyCustomer values, string salesorder_gid)
        {
            try
            {


                msSQL = " select a.customer_address,a.salesorder_gid,a.shopifyorder_number," +
                    "date_format(a.salesorder_date,'%d-%m-%Y') as salesorder_date,a.shopify_orderid," +
                    "a.customer_contact_person,a.Grandtotal,a.salesorder_status," +
                    "(case when b.shopify_orderid=a.shopify_orderid then count(b.qty_quoted) else count(b.qty_quoted) end) as item_count," +
                    "(case when c.shopify_orderid=a.shopify_orderid then 'Assigned' when c.shopify_orderid is null then 'Not Assign' end) " +
                    "as status_flag from cmr_smm_tshopifysalesorder a  left join cmr_smm_tshopifysalesorderdtl  b" +
                    " on b.shopify_orderid=a.shopify_orderid left join smr_trn_tsalesorder  c" +
                    " on c.shopify_orderid=a.shopify_orderid  where a.salesorder_gid='" + salesorder_gid + "' group by b.shopify_orderid order by shopifyorder_number desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<shopifyordersummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new shopifyordersummary_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            shopifyorder_number = dt["shopifyorder_number"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            shopify_orderid = dt["shopify_orderid"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            item_count = dt["item_count"].ToString(),
                            status_flag = dt["status_flag"].ToString(),


                        });
                        values.shopifyordersummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Shopify OrderSummary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetViewsalesproductSummary(MdlShopifyCustomer values, string salesorder_gid)
        {
            try
            {


                msSQL = "select b.product_name,b.qty_quoted,b.product_price,c.customerproduct_code,b.discount_amount,b.taxable,tax_amount,b.tax_name from  cmr_smm_tshopifysalesorder a " +
                    " left join cmr_smm_tshopifysalesorderdtl b on b.shopify_orderid=a.shopify_orderid left join pmr_mst_tproduct c" +
                    " on  b.product_gid=c.product_gid where a.salesorder_gid='" + salesorder_gid + "';";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<shopifyproductordersummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new shopifyproductordersummary_list
                        {
                            product_name = dt["product_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            taxable = dt["taxable"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),

                        });
                        values.shopifyproductordersummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Shopify OrderSummary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetShopifyPaymentSummary(MdlShopifyCustomer values)
        {
            try
            {


                msSQL = " select    a.customer_address,a.salesorder_gid,a.shopifyorder_number," +
                    "date_format(a.salesorder_date,'%d-%m-%Y') as salesorder_date,a.shopify_orderid,a.customer_contact_person," +
                    "a.Grandtotal,a.salesorder_status,(case when b.shopify_orderid=a.shopify_orderid then sum(b.qty_quoted) else sum(b.qty_quoted) end) as item_count," +
                    "(case when c.shopify_orderid=a.shopify_orderid then 'Assigned' when c.shopify_orderid is null then 'Not Assign' end) as status_flag" +
                    " from smr_trn_tsalesorder a  left join smr_trn_tsalesorderdtl  b  on b.shopify_orderid=a.shopify_orderid " +
                    "left join rbl_trn_tinvoice  c  on c.shopify_orderid=a.shopify_orderid  where a.salesorder_status='paid'  group by" +
                    " b.shopify_orderid  order by shopifyorder_number desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<shopifypaymentsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new shopifypaymentsummary_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            shopifyorder_number = dt["shopifyorder_number"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            shopify_orderid = dt["shopify_orderid"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            item_count = dt["item_count"].ToString(),
                            status_flag = dt["status_flag"].ToString(),


                        });
                        values.shopifypaymentsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Shopify PaymentSummary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetShopifyOrderCountSummary(MdlShopifyCustomer values)
        {
            try
            {


                msSQL = " select ( select  count(shopify_orderid) as order_paidcount from cmr_smm_tshopifysalesorder  where salesorder_status='paid') as order_paidcount, " +
                                    " (select  count(shopify_orderid) as order_penidngcount from cmr_smm_tshopifysalesorder  where salesorder_status='pending') as order_penidngcount," +
                                " (select  count(shopify_orderid) as order_refundedcount from cmr_smm_tshopifysalesorder  where salesorder_status='refunded') as order_refundedcount, " +
                                  " (select  count(shopify_productid) as product_count from crm_smm_tshopifyproduct  ) as product_count, " +
                                "  (select  count(shopify_orderid) as total_order from cmr_smm_tshopifysalesorder  ) as total_order, " +
                                " (select  count(shopify_orderid) as order_partiallycount from cmr_smm_tshopifysalesorder  where salesorder_status='partially_refunded') as order_partiallycount";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<shopifyordercountsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new shopifyordercountsummary_list
                        {
                            order_paidcount = dt["order_paidcount"].ToString(),
                            order_penidngcount = dt["order_penidngcount"].ToString(),
                            order_refundedcount = dt["order_refundedcount"].ToString(),
                            order_partiallycount = dt["order_partiallycount"].ToString(),
                            product_count = dt["product_count"].ToString(),
                            total_order = dt["total_order"].ToString(),


                        });
                        values.shopifyordercountsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error While Fetching Shopify OrderCountSummary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        public void DaSendorder(string user_gid, shopifyordermovingtoorder values)
        {
            try
            {

                for (int i = 0; i < values.shopifyorderlists.ToArray().Length; i++)
                {
                    msSQL = "select shopify_orderid from smr_trn_tsalesorder where shopify_orderid='" + values.shopifyorderlists[i].shopify_orderid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count == 0)
                    {

                        msSQL = " select employee_gid  from hrm_mst_temployee where user_gid = '" + user_gid + "'";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsemployee_gid = objOdbcDataReader["employee_gid"].ToString();

                        }
                        objOdbcDataReader.Close();
                        msSQL = " select customer_gid,addon_charge,currency_code,customer_mobile,customer_email,date_format(salesorder_date,'%Y-%m-%d') as salesorder_date  from cmr_smm_tshopifysalesorder where shopify_orderid='" + values.shopifyorderlists[i].shopify_orderid + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lssalesorder_date = objOdbcDataReader["salesorder_date"].ToString();
                            lscustomer_gid = objOdbcDataReader["customer_gid"].ToString();
                            lsaddon_charge = objOdbcDataReader["addon_charge"].ToString();
                            lscurrency_code = objOdbcDataReader["currency_code"].ToString();
                            lscustomer_mobile = objOdbcDataReader["customer_mobile"].ToString();
                            lscustomer_email = objOdbcDataReader["customer_email"].ToString();
                        }
                        objOdbcDataReader.Close();
                        msSQL = " select customer_gid from crm_mst_tcustomer where shopify_id='" + lscustomer_gid + "' ";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lscustomergid = objOdbcDataReader["customer_gid"].ToString();


                        }
                        if (lscustomergid != null && lscustomergid != "")
                        {
                            lscustomer = lscustomergid;
                        }
                        else
                        {
                            lscustomer = null;
                        }

                        objOdbcDataReader.Close();

                        msSQL = " insert  into smr_trn_tsalesorder (" +
                                      " salesorder_gid ," +
                                      " shopify_orderid ," +
                                      " salesorder_date," +
                                      " customer_gid," +
                                      " source_flag," +
                                      " shopifyorder_number," +
                                      " shopifycustomer_id," +
                                      " customer_name," +
                                      " customer_contact_person," +
                                      " created_by," +
                                      " Grandtotal, " +
                                      " salesorder_status, " +
                                      " addon_charge, " +
                                      " grandtotal_l, " +
                                      " currency_code, " +
                                      " customer_mobile, " +
                                      " customer_address," +
                                      " shipping_to," +
                                      " customer_email " +
                                       "  )values(" +
                                      " '" + values.shopifyorderlists[i].salesorder_gid + "'," +
                                      " '" + values.shopifyorderlists[i].shopify_orderid + "'," +
                                      " '" + lssalesorder_date + "'," +
                                       " '" + lscustomer + "'," +
                                       " 'S'," +
                                      " '" + values.shopifyorderlists[i].shopifyorder_number + "'," +
                                      " '" + lscustomer_gid + "'," +
                                      " '" + values.shopifyorderlists[i].customer_contact_person + "'," +
                                      " '" + values.shopifyorderlists[i].customer_contact_person + "'," +
                                      " '" + lsemployee_gid + "'," +
                                      " '" + values.shopifyorderlists[i].Grandtotal + lsaddon_charge + "'," +
                                      " '" + values.shopifyorderlists[i].salesorder_status + "'," +
                                      " '" + lsaddon_charge + "'," +
                                      " '" + values.shopifyorderlists[i].Grandtotal + lsaddon_charge + "'," +
                                      " '" + lscurrency_code + "',";
                        if (lscustomer_mobile == null || lscustomer_mobile == "")
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + lscustomer_mobile + "',";
                        }
                        if (values.shopifyorderlists[i].customer_address == null || (values.shopifyorderlists[i].customer_address == null))
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + (values.shopifyorderlists[i].customer_address) + "',";
                        }
                        if (values.shopifyorderlists[i].customer_address == null || (values.shopifyorderlists[i].customer_address == null))
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + (values.shopifyorderlists[i].customer_address) + "',";
                        }
                        msSQL += "'" + lscustomer_email + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " select salesorderdtl_gid,salesorder_gid,product_gid,shopify_lineitemid,shopify_orderid,product_name,product_price,qty_quoted,product_price_l,selling_price,price_l,discount_amount  from cmr_smm_tshopifysalesorderdtl where shopify_orderid='" + values.shopifyorderlists[i].shopify_orderid + "'";
                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    msSQL = " select product_gid ,productuom_gid  from pmr_mst_tproduct where shopify_productid='" + dt.ItemArray[2] + "' ";

                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lsproduct_gid = objOdbcDataReader["product_gid"].ToString();
                                        lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();

                                    }
                                    if (lsproduct_gid != null && lsproduct_gid != "")
                                    {
                                        lsproductgid = lsproduct_gid;
                                    }
                                    else
                                    {
                                        lsproductgid = null;
                                    }
                                    if (lsproductuom_gid != null && lsproductuom_gid != "")
                                    {
                                        lsproductuomgid = lsproductuom_gid;
                                    }
                                    else
                                    {
                                        lsproductuomgid = null;
                                    }
                                    objOdbcDataReader.Close();

                                    msSQL = " insert into smr_trn_tsalesorderdtl (" +
                                 " salesorderdtl_gid ," +
                                 " salesorder_gid," +
                                 " shopify_productid ," +
                                  " product_gid ," +
                                   " uom_gid ," +
                                 " shopify_lineitemid ," +
                                 " shopify_orderid ," +
                                 " product_name," +
                                 " display_field," +
                                 " product_price," +
                                   " qty_quoted," +
                                 " selling_price," +
                                 " product_price_l, " +
                                 " price_l," +
                                 " discount_amount" +
                                 ")values(" +
                                 " '" + dt.ItemArray[0] + "'," +
                                 " '" + dt.ItemArray[1] + "'," +
                                 " '" + dt.ItemArray[2] + "'," +
                                 " '" + lsproductgid + "'," +
                                 " '" + lsproductuomgid + "'," +
                                 " '" + dt.ItemArray[3] + "'," +
                                 " '" + dt.ItemArray[4] + "'," +
                                 " '" + dt.ItemArray[5] + "'," +
                                 " '" + dt.ItemArray[5] + "'," +
                                 " '" + dt.ItemArray[6] + "'," +
                                 " '" + dt.ItemArray[7] + "'," +
                                 " '" + dt.ItemArray[8] + "'," +
                                 " '" + dt.ItemArray[9] + "'," +
                                 " '" + dt.ItemArray[9] + "'," +
                                 " '" + dt.ItemArray[11] + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    msSQL = " select (product_price*qty_quoted) as total_amount  from smr_trn_tsalesorderdtl where salesorderdtl_gid='" + dt.ItemArray[0] + "' ";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lstotal_amount = objOdbcDataReader["total_amount"].ToString();


                                    }

                                    objOdbcDataReader.Close();
                                    if (mnResult != 0)
                                    {
                                        msSQL = " update  smr_trn_tsalesorderdtl set " +
                                 " price = '" + lstotal_amount + "'" +
                                 " where salesorderdtl_gid='" + dt.ItemArray[0] + "' ";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            values.status = true;
                                            values.message = "Order Sent Successfully";
                                        }
                                        else
                                        {
                                            values.status = false;
                                            values.message = "Error Occurred While Sending Order ";
                                        }

                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error Occurred While Sending Order ";
                                    }
                                }
                            }



                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Order Sending";
                        }

                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {

                values.message = "Error Occurred While Order Sending";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }



        public void DaPostshopifyacceptorder(string user_gid, shopifyorderlists1 values)
        {
            try
            {

                msSQL = "select shopify_orderid from smr_trn_tsalesorder where shopify_orderid='" + values.salesorder_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count == 0)
                {

                    msSQL = " select employee_gid  from hrm_mst_temployee where user_gid = '" + user_gid + "'";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsemployee_gid = objOdbcDataReader["employee_gid"].ToString();

                    }
                    objOdbcDataReader.Close();
                    msSQL = " select customer_gid,addon_charge,currency_code,customer_mobile,customer_email,date_format(salesorder_date,'%Y-%m-%d') as salesorder_date  from cmr_smm_tshopifysalesorder where shopify_orderid='" + values.shopify_orderid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lssalesorder_date = objOdbcDataReader["salesorder_date"].ToString();
                        lscustomer_gid = objOdbcDataReader["customer_gid"].ToString();
                        lsaddon_charge = objOdbcDataReader["addon_charge"].ToString();
                        lscurrency_code = objOdbcDataReader["currency_code"].ToString();
                        lscustomer_mobile = objOdbcDataReader["customer_mobile"].ToString();
                        lscustomer_email = objOdbcDataReader["customer_email"].ToString();
                    }
                    objOdbcDataReader.Close();
                    msSQL = " select customer_gid from crm_mst_tcustomer where shopify_id='" + lscustomer_gid + "' ";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lscustomergid = objOdbcDataReader["customer_gid"].ToString();


                    }
                    if (lscustomergid != null && lscustomergid != "")
                    {
                        lscustomer = lscustomergid;
                    }
                    else
                    {
                        lscustomer = null;
                    }

                    objOdbcDataReader.Close();

                    msSQL = " insert  into smr_trn_tsalesorder (" +
                                  " salesorder_gid ," +
                                  " shopify_orderid ," +
                                  " salesorder_date," +
                                  " customer_gid," +
                                  " source_flag," +
                                  " shopifyorder_number," +
                                  " shopifycustomer_id," +
                                  " customer_name," +
                                  " customer_contact_person," +
                                  " created_by," +
                                  " Grandtotal, " +
                                  " salesorder_status, " +
                                  " addon_charge, " +
                                  " grandtotal_l, " +
                                  " currency_code, " +
                                  " customer_mobile, " +
                                  " customer_address," +
                                  " shipping_to," +
                                  " customer_email " +
                                   "  )values(" +
                                  " '" + values.salesorder_gid + "'," +
                                  " '" + values.shopify_orderid + "'," +
                                  " '" + lssalesorder_date + "'," +
                                   " '" + lscustomer + "'," +
                                   " 'S'," +
                                  " '" + values.shopifyorder_number + "'," +
                                  " '" + lscustomer_gid + "'," +
                                  " '" + values.customer_contact_person + "'," +
                                  " '" + values.customer_contact_person + "'," +
                                  " '" + lsemployee_gid + "'," +
                                  " '" + values.Grandtotal + lsaddon_charge + "'," +
                                  " '" + values.salesorder_status + "'," +
                                  " '" + lsaddon_charge + "'," +
                                  " '" + values.Grandtotal + lsaddon_charge + "'," +
                                  " '" + lscurrency_code + "',";
                    if (lscustomer_mobile == null || lscustomer_mobile == "")
                    {
                        msSQL += "'" + null + "',"; ;
                    }
                    else
                    {
                        msSQL += "'" + lscustomer_mobile + "',";
                    }
                    if (values.customer_address == null || (values.customer_address == null))
                    {
                        msSQL += "'" + null + "',"; ;
                    }
                    else
                    {
                        msSQL += "'" + (values.customer_address) + "',";
                    }
                    if (values.customer_address == null || (values.customer_address == null))
                    {
                        msSQL += "'" + null + "',"; ;
                    }
                    else
                    {
                        msSQL += "'" + (values.customer_address) + "',";
                    }
                    msSQL += "'" + lscustomer_email + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " select salesorderdtl_gid,salesorder_gid,product_gid,shopify_lineitemid,shopify_orderid,product_name,product_price,qty_quoted,product_price_l,selling_price,price_l,discount_amount  from cmr_smm_tshopifysalesorderdtl where shopify_orderid='" + values.shopify_orderid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                msSQL = " select product_gid ,productuom_gid  from pmr_mst_tproduct where shopify_productid='" + dt.ItemArray[2] + "' ";

                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    lsproduct_gid = objOdbcDataReader["product_gid"].ToString();
                                    lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();

                                }
                                if (lsproduct_gid != null && lsproduct_gid != "")
                                {
                                    lsproductgid = lsproduct_gid;
                                }
                                else
                                {
                                    lsproductgid = null;
                                }
                                if (lsproductuom_gid != null && lsproductuom_gid != "")
                                {
                                    lsproductuomgid = lsproductuom_gid;
                                }
                                else
                                {
                                    lsproductuomgid = null;
                                }
                                objOdbcDataReader.Close();

                                msSQL = " insert into smr_trn_tsalesorderdtl (" +
                             " salesorderdtl_gid ," +
                             " salesorder_gid," +
                             " shopify_productid ," +
                              " product_gid ," +
                               " uom_gid ," +
                             " shopify_lineitemid ," +
                             " shopify_orderid ," +
                             " product_name," +
                             " display_field," +
                             " product_price," +
                               " qty_quoted," +
                             " selling_price," +
                             " product_price_l, " +
                             " price_l," +
                             " discount_amount" +
                             ")values(" +
                             " '" + dt.ItemArray[0] + "'," +
                             " '" + dt.ItemArray[1] + "'," +
                             " '" + dt.ItemArray[2] + "'," +
                             " '" + lsproductgid + "'," +
                             " '" + lsproductuomgid + "'," +
                             " '" + dt.ItemArray[3] + "'," +
                             " '" + dt.ItemArray[4] + "'," +
                             " '" + dt.ItemArray[5] + "'," +
                             " '" + dt.ItemArray[5] + "'," +
                             " '" + dt.ItemArray[6] + "'," +
                             " '" + dt.ItemArray[7] + "'," +
                             " '" + dt.ItemArray[8] + "'," +
                             " '" + dt.ItemArray[9] + "'," +
                             " '" + dt.ItemArray[9] + "'," +
                             " '" + dt.ItemArray[11] + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = " select (product_price*qty_quoted) as total_amount  from smr_trn_tsalesorderdtl where salesorderdtl_gid='" + dt.ItemArray[0] + "' ";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows)
                                {
                                    lstotal_amount = objOdbcDataReader["total_amount"].ToString();


                                }

                                objOdbcDataReader.Close();
                                if (mnResult != 0)
                                {
                                    msSQL = " update  smr_trn_tsalesorderdtl set " +
                             " price = '" + lstotal_amount + "'" +
                             " where salesorderdtl_gid='" + dt.ItemArray[0] + "' ";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Order Accepted Successfully";
                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error Occurred While Accepting Order ";
                                    }

                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error Occurred While Accepting Order ";
                                }
                            }
                        }



                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Order Accepting";
                    }

                }

                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {

                values.message = "Error Occurred While Order Accepting";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        public void DaSendpayment(string user_gid, shopifyordermovingtopayment values)
        {
            try
            {


                for (int i = 0; i < values.shopifypaymentlists.ToArray().Length; i++)
                {
                    msSQL = "select shopify_orderid from smr_trn_tsalesorder where shopify_orderid='" + values.shopifypaymentlists[i].shopify_orderid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count == 0)
                    {

                        msSQL = " select employee_gid  from hrm_mst_temployee where user_gid = '" + user_gid + "'";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsemployee_gid = objOdbcDataReader["employee_gid"].ToString();

                        }
                        objOdbcDataReader.Close();
                        msSQL = " select customer_gid,addon_charge,currency_code,customer_mobile,customer_email,date_format(salesorder_date,'%Y-%m-%d') as salesorder_date  from cmr_smm_tshopifysalesorder where shopify_orderid='" + values.shopifypaymentlists[i].shopify_orderid + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lssalesorder_date = objOdbcDataReader["salesorder_date"].ToString();
                            lscustomer_gid = objOdbcDataReader["customer_gid"].ToString();
                            lsaddon_charge = objOdbcDataReader["addon_charge"].ToString();
                            lscurrency_code = objOdbcDataReader["currency_code"].ToString();
                            lscustomer_mobile = objOdbcDataReader["customer_mobile"].ToString();
                            lscustomer_email = objOdbcDataReader["customer_email"].ToString();
                        }
                        objOdbcDataReader.Close();
                        msSQL = " select customer_gid from crm_mst_tcustomer where shopify_id='" + lscustomer_gid + "' ";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lscustomer_gid = objOdbcDataReader["customer_gid"].ToString();


                        }
                        if (lscustomer_gid != null && lscustomer_gid != "")
                        {
                            lscustomergid = lscustomer_gid;
                        }
                        else
                        {
                            lscustomergid = null;
                        }


                        objOdbcDataReader.Close();

                        msSQL = " insert  into smr_trn_tsalesorder (" +
                                      " salesorder_gid ," +
                                      " shopify_orderid ," +
                                      " salesorder_date," +
                                      " customer_gid," +
                                      " shopifyorder_number," +
                                      " shopifycustomer_id," +
                                      " customer_name," +
                                      " customer_contact_person," +
                                      " created_by," +
                                      " Grandtotal, " +
                                      " salesorder_status, " +
                                      " addon_charge, " +
                                      " grandtotal_l, " +
                                      " currency_code, " +
                                      " customer_mobile, " +
                                      " customer_address," +
                                      " shipping_to," +
                                      " customer_email " +
                                       "  )values(" +
                                      " '" + values.shopifypaymentlists[i].salesorder_gid + "'," +
                                      " '" + values.shopifypaymentlists[i].shopify_orderid + "'," +
                                      " '" + lssalesorder_date + "'," +
                                       " '" + lscustomergid + "'," +
                                      " '" + values.shopifypaymentlists[i].shopifyorder_number + "'," +
                                      " '" + lscustomer_gid + "'," +
                                      " '" + values.shopifypaymentlists[i].customer_contact_person + "'," +
                                      " '" + values.shopifypaymentlists[i].customer_contact_person + "'," +
                                      " '" + lsemployee_gid + "'," +
                                      " '" + values.shopifypaymentlists[i].Grandtotal + lsaddon_charge + "'," +
                                      " '" + values.shopifypaymentlists[i].salesorder_status + "'," +
                                      " '" + lsaddon_charge + "'," +
                                      " '" + values.shopifypaymentlists[i].Grandtotal + lsaddon_charge + "'," +
                                      " '" + lscurrency_code + "',";
                        if (lscustomer_mobile == null || lscustomer_mobile == "")
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + lscustomer_mobile + "',";
                        }
                        if (values.shopifypaymentlists[i].customer_address == null || (values.shopifypaymentlists[i].customer_address == null))
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + (values.shopifypaymentlists[i].customer_address) + "',";
                        }
                        if (values.shopifypaymentlists[i].customer_address == null || (values.shopifypaymentlists[i].customer_address == null))
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + (values.shopifypaymentlists[i].customer_address) + "',";
                        }
                        msSQL += "'" + lscustomer_email + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " select salesorderdtl_gid,salesorder_gid,product_gid,shopify_lineitemid,shopify_orderid,product_name,product_price,qty_quoted,product_price_l,selling_price,price_l  from cmr_smm_tshopifysalesorderdtl where shopify_orderid='" + values.shopifypaymentlists[i].shopify_orderid + "'";
                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    msSQL = " select product_gid ,productuom_gid  from pmr_mst_tproduct where shopify_productid='" + dt.ItemArray[2] + "' ";

                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lsproduct_gid = objOdbcDataReader["product_gid"].ToString();
                                        lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();

                                    }
                                    if (lsproduct_gid != null && lsproduct_gid != "")
                                    {
                                        lsproductgid = lsproduct_gid;
                                    }
                                    else
                                    {
                                        lsproductgid = null;
                                    }
                                    if (lsproductuom_gid != null && lsproductuom_gid != "")
                                    {
                                        lsproductuomgid = lsproductuom_gid;
                                    }
                                    else
                                    {
                                        lsproductuomgid = null;
                                    }
                                    objOdbcDataReader.Close();


                                    msSQL = " insert into smr_trn_tsalesorderdtl (" +
                                 " salesorderdtl_gid ," +
                                 " salesorder_gid," +
                                 " shopify_productid ," +
                                  " product_gid ," +
                                   " uom_gid ," +
                                 " shopify_lineitemid ," +
                                 " shopify_orderid ," +
                                 " product_name," +
                                 " display_field," +
                                 " product_price," +
                                 " qty_quoted" +
                                 ")values(" +
                                 " '" + dt.ItemArray[0] + "'," +
                                 " '" + dt.ItemArray[1] + "'," +
                                 " '" + dt.ItemArray[2] + "'," +
                                 " '" + lsproductgid + "'," +
                                 " '" + lsproductuomgid + "'," +
                                 " '" + dt.ItemArray[3] + "'," +
                                 " '" + dt.ItemArray[4] + "'," +
                                 " '" + dt.ItemArray[5] + "'," +
                                 " '" + dt.ItemArray[5] + "'," +
                                 " '" + dt.ItemArray[6] + "'," +
                                 " '" + dt.ItemArray[7] + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    msSQL = " select (product_price*qty_quoted) as total_amount  from smr_trn_tsalesorderdtl where salesorderdtl_gid='" + dt.ItemArray[0] + "' ";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lstotal_amount = objOdbcDataReader["total_amount"].ToString();


                                    }
                                    objOdbcDataReader.Close();

                                    if (mnResult != 0)
                                    {

                                        msSQL = " update  smr_trn_tsalesorderdtl set " +
                                 " price = '" + lstotal_amount + "'" +
                                 " where salesorderdtl_gid='" + dt.ItemArray[0] + "' ";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        if (mnResult != 0)
                                        {
                                            values.status = true;
                                            values.message = "Payment Approved Successfully";
                                        }
                                        else
                                        {
                                            values.status = false;
                                            values.message = "Error Occurred While Payment Approve";
                                        }
                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error Occurred While Payment Approve";
                                    }

                                }
                            }



                        }


                    }
                    msSQL = "select shopify_orderid from rbl_trn_tinvoice where shopify_orderid='" + values.shopifypaymentlists[i].shopify_orderid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count == 0)
                    {

                        msSQL = " select employee_gid  from hrm_mst_temployee where user_gid = '" + user_gid + "'";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsemployee_gid = objOdbcDataReader["employee_gid"].ToString();

                        }
                        objOdbcDataReader.Close();
                        msSQL = " select customer_gid,addon_charge,currency_code,customer_mobile,customer_email,date_format(salesorder_date,'%Y-%m-%d') as salesorder_date  from cmr_smm_tshopifysalesorder where shopify_orderid='" + values.shopifypaymentlists[i].shopify_orderid + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lssalesorder_date = objOdbcDataReader["salesorder_date"].ToString();
                            lscustomer_gid = objOdbcDataReader["customer_gid"].ToString();
                            lsaddon_charge = objOdbcDataReader["addon_charge"].ToString();
                            lscurrency_code = objOdbcDataReader["currency_code"].ToString();
                            lscustomer_mobile = objOdbcDataReader["customer_mobile"].ToString();
                            lscustomer_email = objOdbcDataReader["customer_email"].ToString();
                        }
                        objOdbcDataReader.Close();
                        msINGetGID = objcmnfunctions.GetMasterGID("SIVT");
                        msSQL = " select customer_gid from crm_mst_tcustomer where shopify_id='" + lscustomer_gid + "' ";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lscustomer_gid = objOdbcDataReader["customer_gid"].ToString();


                        }
                        if (lscustomer_gid != null && lscustomer_gid != "")
                        {
                            lscustomergid = lscustomer_gid;
                        }
                        else
                        {
                            lscustomergid = null;
                        }

                        objOdbcDataReader.Close();

                        msSQL = " insert  into rbl_trn_tinvoice (" +
                                      " invoice_gid ," +
                                      " shopify_orderid ," +
                                      " invoice_date," +
                                      " customer_gid," +
                                      " shopifyorder_number," +
                                      " shopifycustomer_id," +
                                      " customer_name," +
                                      " customer_contactperson," +
                                      " approved_by," +
                                      " invoice_amount, " +
                                      " payment_flag, " +
                                      " additionalcharges_amount_L, " +
                                      " payment_amount, " +
                                      " total_amount, " +
                                      " currency_code, " +
                                      " customer_address," +
                                      " customer_email " +
                                       "  )values(" +
                                      " '" + msINGetGID + "'," +
                                      " '" + values.shopifypaymentlists[i].shopify_orderid + "'," +
                                      " '" + lssalesorder_date + "'," +
                                       " '" + lscustomergid + "'," +
                                      " '" + values.shopifypaymentlists[i].shopifyorder_number + "'," +
                                      " '" + lscustomer_gid + "'," +
                                      " '" + values.shopifypaymentlists[i].customer_contact_person + "'," +
                                      " '" + values.shopifypaymentlists[i].customer_contact_person + "'," +
                                      " '" + lsemployee_gid + "'," +
                                      " '" + values.shopifypaymentlists[i].Grandtotal + lsaddon_charge + "'," +
                                      " '" + values.shopifypaymentlists[i].salesorder_status + "'," +
                                      " '" + lsaddon_charge + "'," +
                                      " '" + values.shopifypaymentlists[i].Grandtotal + lsaddon_charge + "'," +
                                      " '" + values.shopifypaymentlists[i].Grandtotal + lsaddon_charge + "'," +
                                      " '" + lscurrency_code + "',";
                        if (values.shopifypaymentlists[i].customer_address == null || (values.shopifypaymentlists[i].customer_address == null))
                        {
                            msSQL += "'" + null + "',"; ;
                        }
                        else
                        {
                            msSQL += "'" + (values.shopifypaymentlists[i].customer_address) + "',";
                        }
                        msSQL += "'" + lscustomer_email + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " select salesorderdtl_gid,salesorder_gid,product_gid,shopify_lineitemid,shopify_orderid,product_name,product_price,qty_quoted,product_price_l,selling_price,price_l  from cmr_smm_tshopifysalesorderdtl where shopify_orderid='" + values.shopifypaymentlists[i].shopify_orderid + "'";
                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    msSQL = " select product_gid ,productuom_gid  from pmr_mst_tproduct where shopify_productid='" + dt.ItemArray[2] + "' ";

                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lsproduct_gid = objOdbcDataReader["product_gid"].ToString();
                                        lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();

                                    }
                                    if (lsproduct_gid != null && lsproduct_gid != "")
                                    {
                                        lsproductgid = lsproduct_gid;
                                    }
                                    else
                                    {
                                        lsproductgid = null;
                                    }
                                    if (lsproductuom_gid != null && lsproductuom_gid != "")
                                    {
                                        lsproductuomgid = lsproductuom_gid;
                                    }
                                    else
                                    {
                                        lsproductuomgid = null;
                                    }

                                    objOdbcDataReader.Close();

                                    msGetGidInvDt = objcmnfunctions.GetMasterGID("SIVC");
                                    msSQL = " insert into rbl_trn_tinvoicedtl (" +
                                    " invoicedtl_gid ," +
                                    " invoice_gid," +
                                     " product_gid," +
                                      " uom_gid," +
                                    " shopify_productid ," +
                                    " shopify_lineitemid ," +
                                    " shopify_orderid ," +
                                    " product_name," +
                                    " product_price," +
                                    " qty_invoice," +
                                    " vendor_price" +
                                    ")values(" +
                                    " '" + msGetGidInvDt + "'," +
                                    " '" + msINGetGID + "'," +
                                    " '" + lsproductgid + "'," +
                                    " '" + lsproductuomgid + "'," +
                                    " '" + dt.ItemArray[2] + "'," +
                                    " '" + dt.ItemArray[3] + "'," +
                                    " '" + dt.ItemArray[4] + "'," +
                                    " '" + dt.ItemArray[5] + "'," +
                                    " '" + dt.ItemArray[6] + "'," +
                                    " '" + dt.ItemArray[7] + "'," +
                                    " '" + dt.ItemArray[6] + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        msSQL = " select (product_price*qty_invoice) as total_invoice  from rbl_trn_tinvoicedtl where invoicedtl_gid='" + msGetGidInvDt + "' ";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objOdbcDataReader.HasRows)
                                        {
                                            lstotal_invoice = objOdbcDataReader["total_invoice"].ToString();


                                        }
                                        objOdbcDataReader.Close();
                                        if (mnResult != 0)
                                        {

                                            msSQL = " update  rbl_trn_tinvoicedtl set " +
                                     " product_total = '" + lstotal_invoice + "'" +
                                     "  where invoicedtl_gid='" + msGetGidInvDt + "' ";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult != 0)
                                            {
                                                values.status = true;
                                                values.message = "Payment Approved Successfully";
                                            }
                                            else
                                            {
                                                values.status = false;
                                                values.message = "Error Occurred While Payment Approve";
                                            }

                                        }

                                        else
                                        {
                                            values.status = false;
                                            values.message = "Error Occurred While Payment Approve";
                                        }
                                    }
                                }



                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occurred While Payment Approve";
                            }
                        }
                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Payment Approve";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaSendinventorystock(string user_gid, shopifyinventorystocksend values)
        {
            try
            {


                for (int i = 0; i < values.shopifyinventorystocksendlist.ToArray().Length; i++)
                {
                    msSQL = "select shopify_productid from ims_trn_tstock where shopify_productid='" + values.shopifyinventorystocksendlist[i].shopify_productid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count == 0)
                    {

                        msSQL = " select date_format(created_date,'%Y-%m-%d') as created_date   from crm_smm_tshopifyproduct where shopify_productid='" + values.shopifyinventorystocksendlist[i].shopify_productid + "' ";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lscreated_date = objOdbcDataReader["created_date"].ToString();

                        }
                        objOdbcDataReader.Close();
                        msSQL = " select product_gid ,productuom_gid  from pmr_mst_tproduct where shopify_productid='" + values.shopifyinventorystocksendlist[i].shopify_productid + "' ";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsproduct_gid = objOdbcDataReader["product_gid"].ToString();
                            lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();

                        }
                        if (lsproduct_gid != null && lsproduct_gid != "")
                        {
                            lsproductgid = lsproduct_gid;
                        }
                        else
                        {
                            lsproductgid = null;
                        }
                        if (lsproductuom_gid != null && lsproductuom_gid != "")
                        {
                            lsproductuomgid = lsproductuom_gid;
                        }
                        else
                        {
                            lsproductuomgid = null;
                        }
                        objOdbcDataReader.Close();

                        msGetGid = objcmnfunctions.GetMasterGID("ISKP");
                        msSQL = " insert  into ims_trn_tstock (" +
                                      " stock_gid ," +
                                      " shopify_productid ," +
                                      " product_gid," +
                                        " uom_gid," +
                                      " created_date," +
                                      " display_field," +
                                      " stock_qty," +
                                      " grn_qty," +
                                      " unit_price," +
                                      " created_by," +
                                      " stock_flag " +
                                       "  )values(" +
                                      " '" + msGetGid + "'," +
                                      " '" + values.shopifyinventorystocksendlist[i].shopify_productid + "'," +
                                      " '" + lsproductgid + "'," +
                                      " '" + lsproductuomgid + "'," +
                                      " '" + lscreated_date + "'," +
                                      " '" + values.shopifyinventorystocksendlist[i].product_name + "'," +
                                      " '" + values.shopifyinventorystocksendlist[i].inventory_quantity + "'," +
                                      " '" + values.shopifyinventorystocksendlist[i].old_inventory_quantity + "'," +
                                      " '" + values.shopifyinventorystocksendlist[i].product_price + "'," +
                                      " '" + user_gid + "'," +
                                    "'Y')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Moved To Stock Successfully";




                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Moving Stock";
                        }

                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Error Occurred While Moving Stock";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


    }
}