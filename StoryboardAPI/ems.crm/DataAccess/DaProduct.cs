using ems.crm.Models;
using ems.crm.DataAccess;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using static OfficeOpenXml.ExcelErrorValue;
using System.Diagnostics;
using System.Web.Http.Results;
using System.Threading;
using System.Data.Common;
using System.Threading.Tasks;
using System.Text;
using System.Security.Policy;
using System.Collections.Concurrent;
using System.Data.OleDb;
using System.Text.RegularExpressions;


namespace ems.crm.DataAccess
{
    public class DaProduct
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, dt_datatable1, dt_datatable2, dt_datatable3, dt_datatable4, dt_datatable5, dt_datatable6;
        string base_url, lsclient_id, api_key = string.Empty;

        string msEmployeeGID, lsemployee_gid, lsuser_gid, lsentity_code, producttype_gid, msGetGid3, msGetGid2, productuomclass_gid, productuomclassgid, lsproducttype_name, productgroup_gid, lsproduct_name, lslocation_id, lsproduct, lsshopify, lsshopify_flag, lsshopify_productid, lsaccess_token, lsshopify_store_name, lsstore_month_year, lsdesignation_code, lsCode, msGetGid9, lsproductgroup_gid, lsproductgroupgid, msGetGid5, msGetGid4, lsproducttypegid, lsproducttype_gid, final_path, lsproductuomgid, productuom_gid, lsproductuom_gid, lsproductuomclass_gid, lsproductuomclassgid, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsproduct_gid, lsproductgid, lsproductname, stock_status, lscurrency_code, lscompany_website, lsproduct_desc, lscost_price, lsproduct_image, lscompany_name;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;


        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private static Task<getproduct> _runningTask;

        public async Task<getproduct> DaGetShopifyProductdetails(string user_gid)
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

                    _runningTask = ExecuteGetShopifyProductdetails(user_gid); // Start a new task
                    await _runningTask;
                }
                finally
                {
                    _semaphore.Release(); // Release the semaphore
                }
            }
        }

        public async Task<getproduct> ExecuteGetShopifyProductdetails(string user_gid)
        {
            getproduct objresult = new getproduct();
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
                var request = new RestRequest("/admin/api/" + lsstore_month_year + "/products.json?limit=250", Method.GET);
                request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                request.AddHeader("Cookie", "_master_udr=eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaEpJaWxqWXpsak9UQXhPUzAyWkRZMkxUUXlOR1F0T0RKbVl5MDNaVEZsTnpFM09EY3dOV0lHT2daRlJnPT0iLCJleHAiOiIyMDI1LTEwLTIwVDA4OjI3OjU2LjU4MloiLCJwdXIiOiJjb29raWUuX21hc3Rlcl91ZHIifX0%3D--6f6310c22570c2812426da811c5f9f64d2d35161; _secure_admin_session_id=bbc22793fbba552b04eeebfaeb0de080; _secure_admin_session_id_csrf=bbc22793fbba552b04eeebfaeb0de080; identity-state=BAhbB0kiJWVhODM3YTZhN2M3Njg1MzhlNWQ3MTNhYzg2NmM5MWUwBjoGRUZJIiUwY2M0MWQ1ZjE4ZTQwZTcwYWQ1ZTVkMWUzMDBkMzZlYgY7AEY%3D--69633ab3c25bb20e105bbe14b912f36422abe9b1; identity-state-0cc41d5f18e40e70ad5e5d1e300d36eb=BAh7DEkiDnJldHVybi10bwY6BkVUSSI0aHR0cHM6Ly9lNDQ1NWYtMi5teXNob3BpZnkuY29tL2FkbWluL2F1dGgvbG9naW4GOwBUSSIRcmVkaXJlY3QtdXJpBjsAVEkiQGh0dHBzOi8vZTQ0NTVmLTIubXlzaG9waWZ5LmNvbS9hZG1pbi9hdXRoL2lkZW50aXR5L2NhbGxiYWNrBjsAVEkiEHNlc3Npb24ta2V5BjsAVDoMYWNjb3VudEkiD2NyZWF0ZWQtYXQGOwBUZhcxNjk3NzkxOTE0LjQyODUwMDJJIgpub25jZQY7AFRJIiU1YTQyNzRiZTI5ZmVjODE0MDU4ZTlmNGU3ZGZiNzU4MwY7AEZJIgpzY29wZQY7AFRbEEkiCmVtYWlsBjsAVEkiN2h0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvZGVzdGluYXRpb25zLnJlYWRvbmx5BjsAVEkiC29wZW5pZAY7AFRJIgxwcm9maWxlBjsAVEkiTmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvcGFydG5lcnMuY29sbGFib3JhdG9yLXJlbGF0aW9uc2hpcHMucmVhZG9ubHkGOwBUSSIwaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9iYW5raW5nLm1hbmFnZQY7AFRJIkJodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL21lcmNoYW50LXNldHVwLWRhc2hib2FyZC5ncmFwaHFsBjsAVEkiPGh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvc2hvcGlmeS1jaGF0LmFkbWluLmdyYXBocWwGOwBUSSI3aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9mbG93LndvcmtmbG93cy5tYW5hZ2UGOwBUSSI%2BaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9vcmdhbml6YXRpb24taWRlbnRpdHkubWFuYWdlBjsAVEkiPmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvbWVyY2hhbnQtYmFuay1hY2NvdW50Lm1hbmFnZQY7AFRJIg9jb25maWcta2V5BjsAVEkiDGRlZmF1bHQGOwBU--eb8709d3d8002d429911a5b1c28afca37dd02431; identity-state-ea837a6a7c768538e5d713ac866c91e0=BAh7DEkiDnJldHVybi10bwY6BkVUSSI0aHR0cHM6Ly9lNDQ1NWYtMi5teXNob3BpZnkuY29tL2FkbWluL2F1dGgvbG9naW4GOwBUSSIRcmVkaXJlY3QtdXJpBjsAVEkiQGh0dHBzOi8vZTQ0NTVmLTIubXlzaG9waWZ5LmNvbS9hZG1pbi9hdXRoL2lkZW50aXR5L2NhbGxiYWNrBjsAVEkiEHNlc3Npb24ta2V5BjsAVDoMYWNjb3VudEkiD2NyZWF0ZWQtYXQGOwBUZhYxNjk3NzkwNDc2LjU5MTkxOEkiCm5vbmNlBjsAVEkiJWM5NjYwYTQ2NmZhMTlhZTJlNDQyYmM3NjU0NDMxZWMzBjsARkkiCnNjb3BlBjsAVFsQSSIKZW1haWwGOwBUSSI3aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9kZXN0aW5hdGlvbnMucmVhZG9ubHkGOwBUSSILb3BlbmlkBjsAVEkiDHByb2ZpbGUGOwBUSSJOaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9wYXJ0bmVycy5jb2xsYWJvcmF0b3ItcmVsYXRpb25zaGlwcy5yZWFkb25seQY7AFRJIjBodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL2JhbmtpbmcubWFuYWdlBjsAVEkiQmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvbWVyY2hhbnQtc2V0dXAtZGFzaGJvYXJkLmdyYXBocWwGOwBUSSI8aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9zaG9waWZ5LWNoYXQuYWRtaW4uZ3JhcGhxbAY7AFRJIjdodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL2Zsb3cud29ya2Zsb3dzLm1hbmFnZQY7AFRJIj5odHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL29yZ2FuaXphdGlvbi1pZGVudGl0eS5tYW5hZ2UGOwBUSSI%2BaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9tZXJjaGFudC1iYW5rLWFjY291bnQubWFuYWdlBjsAVEkiD2NvbmZpZy1rZXkGOwBUSSIMZGVmYXVsdAY7AFQ%3D--7f37bdb0df101ca716441e71427765ef41612063");
                IRestResponse response = client.Execute(request);
                string response_content = response.Content;
                shopifyproductlist objMdlShopifyMessageResponse = new shopifyproductlist();
                objMdlShopifyMessageResponse = JsonConvert.DeserializeObject<shopifyproductlist>(response_content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await Task.WhenAll(objMdlShopifyMessageResponse.products.Select(async item =>
                    {

                        msSQL = " select shopify_productid  from crm_smm_tshopifyproduct where shopify_productid = '" + item.id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows != true)
                        {
                            objOdbcDataReader.Close();
                            // Parse the original date string to a DateTime object
                            DateTime originalDate = DateTime.ParseExact(item.created_at.ToString(), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                            // Convert the DateTime object to the desired format
                            string formattedDate = originalDate.ToString("yyyy-MM-dd");
                            msSQL = " insert into crm_smm_tshopifyproduct (" +
                                     " shopify_productid," +
                                       " product_name," +
                                    " variant_id, " +
                                    " option1, " +
                                     " product_type, " +
                                     " inventory_item_id, " +
                                    " inventory_quantity, " +
                                    " old_inventory_quantity, " +
                                    " status, " +
                                    " product_image, " +
                                    " grams, " +
                                    "  weight, " +
                                    "  weight_unit, " +
                                     "  sku, " +
                                    " price, " +
                                    " compare_at_price, " +
                                    " vendor_name, " +
                                       " created_by, " +
                                       " created_date)" +
                                       " values(" +
                                       " '" + item.id + "',";
                            if (item.title == null || item.title == "")
                            {
                                msSQL += "'',";
                            }
                            else
                            {

                                msSQL += "'" + item.title.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "',";
                            }
                            msSQL += "'" + item.variants[0].id + "'," +
                                "'" + item.variants[0].option1 + "'," +
                                "'" + item.product_type + "'," +
                               "'" + item.variants[0].inventory_item_id + "'," +
                            "'" + item.variants[0].inventory_quantity + "'," +
                            "'" + item.variants[0].old_inventory_quantity + "',";
                            if (item.status == "active")
                            {
                                msSQL += "'1',";
                            }
                            else if (item.status == "draft")
                            {
                                msSQL += "'3',";
                            }
                            else if (item.status == "archived")
                            {
                                msSQL += "'4',";
                            }
                            if (item.image == null)
                            {
                                msSQL += "'0',";
                            }
                            else
                            {
                                msSQL += "'" + item.images[0].src + "',";
                            }
                            msSQL += "'" + item.variants[0].grams + "'," +
                                 "'" + item.variants[0].weight + "'," +
                                 "'" + item.variants[0].weight_unit + "'," +
                                 "'" + item.variants[0].sku + "'," +
                           "'" + item.variants[0].price + "'," +
                           "'" + item.variants[0].compare_at_price + "'," +
                           "'" + item.vendor + "'," +
                            "'" + user_gid + "'," +
                            "'" + formattedDate + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            objOdbcDataReader.Close();
                        }


                        objOdbcDataReader.Close();

                    }));
                }

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "****************Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

            return objresult;
        }

        public getproduct DaGetShopifyProduct(string user_gid)
        {
            getproduct objresult = new getproduct();
            msSQL = " SELECT access_token,shopify_store_name,store_month_year FROM crm_smm_shopify_service limit 1 ";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {

                lsaccess_token = objOdbcDataReader["access_token"].ToString();
                lsshopify_store_name = objOdbcDataReader["shopify_store_name"].ToString();
                lsstore_month_year = objOdbcDataReader["store_month_year"].ToString();

            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
            var request = new RestRequest("/admin/api/" + lsstore_month_year + "/products.json?limit=250", Method.GET);
            request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
            request.AddHeader("Cookie", "_master_udr=eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaEpJaWxqWXpsak9UQXhPUzAyWkRZMkxUUXlOR1F0T0RKbVl5MDNaVEZsTnpFM09EY3dOV0lHT2daRlJnPT0iLCJleHAiOiIyMDI1LTEwLTIwVDA4OjI3OjU2LjU4MloiLCJwdXIiOiJjb29raWUuX21hc3Rlcl91ZHIifX0%3D--6f6310c22570c2812426da811c5f9f64d2d35161; _secure_admin_session_id=bbc22793fbba552b04eeebfaeb0de080; _secure_admin_session_id_csrf=bbc22793fbba552b04eeebfaeb0de080; identity-state=BAhbB0kiJWVhODM3YTZhN2M3Njg1MzhlNWQ3MTNhYzg2NmM5MWUwBjoGRUZJIiUwY2M0MWQ1ZjE4ZTQwZTcwYWQ1ZTVkMWUzMDBkMzZlYgY7AEY%3D--69633ab3c25bb20e105bbe14b912f36422abe9b1; identity-state-0cc41d5f18e40e70ad5e5d1e300d36eb=BAh7DEkiDnJldHVybi10bwY6BkVUSSI0aHR0cHM6Ly9lNDQ1NWYtMi5teXNob3BpZnkuY29tL2FkbWluL2F1dGgvbG9naW4GOwBUSSIRcmVkaXJlY3QtdXJpBjsAVEkiQGh0dHBzOi8vZTQ0NTVmLTIubXlzaG9waWZ5LmNvbS9hZG1pbi9hdXRoL2lkZW50aXR5L2NhbGxiYWNrBjsAVEkiEHNlc3Npb24ta2V5BjsAVDoMYWNjb3VudEkiD2NyZWF0ZWQtYXQGOwBUZhcxNjk3NzkxOTE0LjQyODUwMDJJIgpub25jZQY7AFRJIiU1YTQyNzRiZTI5ZmVjODE0MDU4ZTlmNGU3ZGZiNzU4MwY7AEZJIgpzY29wZQY7AFRbEEkiCmVtYWlsBjsAVEkiN2h0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvZGVzdGluYXRpb25zLnJlYWRvbmx5BjsAVEkiC29wZW5pZAY7AFRJIgxwcm9maWxlBjsAVEkiTmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvcGFydG5lcnMuY29sbGFib3JhdG9yLXJlbGF0aW9uc2hpcHMucmVhZG9ubHkGOwBUSSIwaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9iYW5raW5nLm1hbmFnZQY7AFRJIkJodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL21lcmNoYW50LXNldHVwLWRhc2hib2FyZC5ncmFwaHFsBjsAVEkiPGh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvc2hvcGlmeS1jaGF0LmFkbWluLmdyYXBocWwGOwBUSSI3aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9mbG93LndvcmtmbG93cy5tYW5hZ2UGOwBUSSI%2BaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9vcmdhbml6YXRpb24taWRlbnRpdHkubWFuYWdlBjsAVEkiPmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvbWVyY2hhbnQtYmFuay1hY2NvdW50Lm1hbmFnZQY7AFRJIg9jb25maWcta2V5BjsAVEkiDGRlZmF1bHQGOwBU--eb8709d3d8002d429911a5b1c28afca37dd02431; identity-state-ea837a6a7c768538e5d713ac866c91e0=BAh7DEkiDnJldHVybi10bwY6BkVUSSI0aHR0cHM6Ly9lNDQ1NWYtMi5teXNob3BpZnkuY29tL2FkbWluL2F1dGgvbG9naW4GOwBUSSIRcmVkaXJlY3QtdXJpBjsAVEkiQGh0dHBzOi8vZTQ0NTVmLTIubXlzaG9waWZ5LmNvbS9hZG1pbi9hdXRoL2lkZW50aXR5L2NhbGxiYWNrBjsAVEkiEHNlc3Npb24ta2V5BjsAVDoMYWNjb3VudEkiD2NyZWF0ZWQtYXQGOwBUZhYxNjk3NzkwNDc2LjU5MTkxOEkiCm5vbmNlBjsAVEkiJWM5NjYwYTQ2NmZhMTlhZTJlNDQyYmM3NjU0NDMxZWMzBjsARkkiCnNjb3BlBjsAVFsQSSIKZW1haWwGOwBUSSI3aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9kZXN0aW5hdGlvbnMucmVhZG9ubHkGOwBUSSILb3BlbmlkBjsAVEkiDHByb2ZpbGUGOwBUSSJOaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9wYXJ0bmVycy5jb2xsYWJvcmF0b3ItcmVsYXRpb25zaGlwcy5yZWFkb25seQY7AFRJIjBodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL2JhbmtpbmcubWFuYWdlBjsAVEkiQmh0dHBzOi8vYXBpLnNob3BpZnkuY29tL2F1dGgvbWVyY2hhbnQtc2V0dXAtZGFzaGJvYXJkLmdyYXBocWwGOwBUSSI8aHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9zaG9waWZ5LWNoYXQuYWRtaW4uZ3JhcGhxbAY7AFRJIjdodHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL2Zsb3cud29ya2Zsb3dzLm1hbmFnZQY7AFRJIj5odHRwczovL2FwaS5zaG9waWZ5LmNvbS9hdXRoL29yZ2FuaXphdGlvbi1pZGVudGl0eS5tYW5hZ2UGOwBUSSI%2BaHR0cHM6Ly9hcGkuc2hvcGlmeS5jb20vYXV0aC9tZXJjaGFudC1iYW5rLWFjY291bnQubWFuYWdlBjsAVEkiD2NvbmZpZy1rZXkGOwBUSSIMZGVmYXVsdAY7AFQ%3D--7f37bdb0df101ca716441e71427765ef41612063");
            IRestResponse response = client.Execute(request);
            string response_content = response.Content;
            shopifyproductlist objMdlShopifyMessageResponse = new shopifyproductlist();
            objMdlShopifyMessageResponse = JsonConvert.DeserializeObject<shopifyproductlist>(response_content);
            if (response.StatusCode == HttpStatusCode.OK)
            {

                foreach (var item in objMdlShopifyMessageResponse.products)
                {

                    msSQL = " select shopify_productid  from pmr_mst_tproduct where shopify_productid = '" + item.id + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows != true)
                    {
                        msSQL = " select productuomclass_gid  from pmr_mst_tproductuomclass where productuomclass_name = '" + item.variants[0].weight_unit + "' limit 1 ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsproductuomclass_gid = objOdbcDataReader["productuomclass_gid"].ToString();

                        }

                        if (lsproductuomclass_gid != null)
                        {
                            lsproductuomclassgid = lsproductuomclass_gid;
                        }
                        else
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("PUCM");
                            msSQL = " insert into pmr_mst_tproductuomclass (" +
                            " productuomclass_gid," +
                            " productuomclass_name ," +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "',";
                            if (item.variants[0].weight_unit == null || item.variants[0].weight_unit == "")
                            {
                                msSQL += "'',";
                            }
                            else
                            {
                                msSQL += "'" + item.variants[0].weight_unit.Replace("'", "\\'") + "',";
                            }
                            msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsproductuomclassgid = msGetGid;
                        }

                        msSQL = " select productuom_gid  from pmr_mst_tproductuom where productuomclass_gid = '" + lsproductuomclass_gid + "' and productuom_gid = '" + item.variants[0].weight + "' limit 1 ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();

                        }

                        if (lsproductuom_gid != null)
                        {
                            lsproductuomgid = lsproductuom_gid;
                        }
                        else
                        {
                            msGetGid1 = objcmnfunctions.GetMasterGID("PPMM");
                            msSQL = " insert into pmr_mst_tproductuom (" +
                            " productuom_gid," +
                         " productuomclass_gid," +
                         " productuom_name ," +
                         " created_by, " +
                         " created_date)" +
                         " values(" +
                         "'" + msGetGid1 + "'," +
                         " '" + lsproductuomclass_gid + "'," +
                             "'" + item.variants[0].weight + "'," +
                            "'" + user_gid + "',";
                            msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            lsproductuomgid = msGetGid1;

                        }




                        msSQL = " select productgroup_gid from pmr_mst_tproductgroup where productgroup_name = '" + item.product_type + "'  limit 1 ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsproductgroup_gid = objOdbcDataReader["productgroup_gid"].ToString();

                        }

                        if (lsproductgroup_gid != null)
                        {
                            lsproductgroupgid = lsproductgroup_gid;
                        }
                        else
                        {
                            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPGM' order by finyear desc limit 0,1 ";
                            string lsCode1 = objdbconn.GetExecuteScalar(msSQL);

                            string lsproductgroup_code = "PGC" + "00" + lsCode1;
                            msGetGid5 = objcmnfunctions.GetMasterGID("PPGM");

                            msSQL = " insert into pmr_mst_tproductgroup (" +
                                        " productgroup_gid," +
                                        " productgroup_code," +
                                        " productgroup_name," +
                                        " created_by, " +
                                        " created_date)" +
                                        " values(" +
                                        " '" + msGetGid5 + "'," +
                                        " '" + lsproductgroup_code + "'," +
                                        "'" + item.product_type + "'," +
                                        "'" + user_gid + "'," +
                                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsproductgroupgid = msGetGid5;

                        }





                        msSQL = " select producttype_gid from pmr_mst_tproducttype where producttype_name  = '" + item.product_type + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsproducttype_gid = objOdbcDataReader["producttype_gid"].ToString();

                        }

                        if (lsproducttype_gid != null)
                        {
                            lsproducttypegid = lsproducttype_gid;
                        }
                        else
                        {
                            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                            string lsCode6 = objdbconn.GetExecuteScalar(msSQL);

                            string lsproduct_typegid = "PG" + "00" + lsCode6;

                            msSQL = " insert into pmr_mst_tproducttype (" +
                            " producttype_gid," +
                         " producttype_name)" +
                         " values(" +
                         "'" + lsproduct_typegid + "',";
                            msSQL += "'" + item.product_type + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsproducttypegid = lsproduct_typegid;

                        }

                        msGetGid = objcmnfunctions.GetMasterGID("PPTM");
                        msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                        string lsCode7 = objdbconn.GetExecuteScalar(msSQL);

                        string lsproduct_code = "PC" + "00" + lsCode7;


                        msGetGid9 = objcmnfunctions.GetMasterGID("PPTM");
                        msSQL = " insert into pmr_mst_tproduct (" +
                               " product_gid," +
                               " product_code," +
                               " product_name," +
                            " productgroup_gid, " +
                            " productuomclass_gid, " +
                            " productuom_gid, " +
                            " stockable, " +
                            " producttype_gid, " +
                            " variant_id, " +
                            " inventory_quantity, " +
                            " sku, " +
                            " old_inventory_quantity, " +
                            " status, " +
                            " product_image, " +
                            " grams, " +
                            " price, " +
                            " compare_at_price, " +
                            " vendor_name, " +
                            " shopify_productid," +
                               " created_by, " +
                               " created_date)" +
                               " values(" +
                               " '" + msGetGid9 + "'," +
                               "'" + lsproduct_code + "',";
                        if (item.title == null || item.title == "")
                        {
                            msSQL += "'',";
                        }
                        else
                        {

                            msSQL += "'" + item.title.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "',";
                        }
                        msSQL += "'" + lsproductgroupgid + "'," +
                                 "'" + lsproductuomclassgid + "'," +
                                 "'" + lsproductuomgid + "'," +
                        "'" + "Y" + "'," +
                        "'" + lsproducttypegid + "'," +
                        "'" + item.variants[0].id + "'," +
                        "'" + item.variants[0].inventory_quantity + "'," +
                        "'" + item.variants[0].sku + "'," +
                        "'" + item.variants[0].old_inventory_quantity + "',";
                        if (item.status == "active")
                        {
                            msSQL += "'1',";
                        }
                        else if (item.status == "draft")
                        {
                            msSQL += "'3',";
                        }
                        else if (item.status == "archived")
                        {
                            msSQL += "'4',";
                        }
                        if (item.image == null)
                        {
                            msSQL += "'0',";
                        }
                        else
                        {
                            msSQL += "'" + item.images[0].src + "',";
                        }
                        msSQL += "'" + item.variants[0].grams + "'," +
                       "'" + item.variants[0].price + "'," +
                       "'" + item.variants[0].compare_at_price + "'," +
                       "'" + item.vendor + "'," +
                        "'" + item.id + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }




                }
                objOdbcDataReader.Close();
            }


            return objresult;
        }
        public void DaGetProductSummary(MdlProduct values)
        {
            try
            {

                msSQL = " SELECT d.producttype_name,a.whatsapp_id,a.whatsappstock_status,b.productgroup_name,a.product_desc,b.productgroup_code,a.product_gid, a.mrp_price, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,  CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
                   " c.productuomclass_code,e.productuom_code,c.productuomclass_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable,e.productuom_name,d.producttype_name as product_type,CASE  WHEN a.status = '1' THEN 'Active' WHEN a.status IS NULL OR a.status = '' OR a.status = '2' THEN 'Inactive' END AS Status," +
                   " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time,(case when a.shopify_productid is not null then 'No'  else 'Yes' end)as shopify_productid,a.product_image,(SELECT COUNT(product_gid) FROM pmr_mst_tproduct) AS product_count  from pmr_mst_tproduct a " +
                   " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                   " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                   " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                   " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                   " left join adm_mst_tuser f on f.user_gid=a.created_by order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list
                        {
                            shopify_productid = dt["shopify_productid"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            product_count = dt["product_count"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            stockable = dt["stockable"].ToString(),

                            productuom_name = dt["productuom_name"].ToString(),
                            product_type = dt["product_type"].ToString(),
                            Status = dt["Status"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            lead_time = dt["lead_time"].ToString(),
                            whatsapp_id = dt["whatsapp_id"].ToString(),
                            whatsappstock_status = dt["whatsappstock_status"].ToString(),




                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaGetShopifyProductSummary(MdlProduct values)
        {
            try
            {

                msSQL = " SELECT a.option1,a.inventory_quantity,a.old_inventory_quantity,a.variant_id,a.inventory_item_id,a.weight_unit,a.weight,a.price,a.grams,d.producttype_name,b.productgroup_name,b.productgroup_code,a.product_gid,a.sku, a.product_price, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,  CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
        " c.productuomclass_code,e.productuom_code,c.productuomclass_name,a.product_type,a.vendor_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable,e.productuom_name,d.producttype_name as product_type,(case when a.status ='1' then 'active'  when a.status ='2' then 'draft'  when a.status ='draft' then 'draft' when a.status ='active' then 'active' when a.status ='inactive' then 'inactive' when a.status ='archived' then 'archived'   when a.status ='3' then 'archived' else 'inactive' end)  as Status," +
        " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,(case when a.shopify_productid=s.shopify_productid then 'Assigned' when s.shopify_productid is null then 'Not Assign' end) as status_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time,a.shopify_productid,(case when a.product_image ='0' then 'no' else a.product_image end)  as product_image  from crm_smm_tshopifyproduct a " +
        " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
        " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
        " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
        " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
          " left join pmr_mst_tproduct s on a.shopify_productid = s.shopify_productid " +
        " left join adm_mst_tuser f on f.user_gid=a.created_by order by a.id desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list
                        {
                            option1 = dt["option1"].ToString(),
                            inventory_quantity = dt["inventory_quantity"].ToString(),
                            old_inventory_quantity = dt["old_inventory_quantity"].ToString(),
                            variant_id = dt["variant_id"].ToString(),
                            inventory_item_id = dt["inventory_item_id"].ToString(),
                            weight_unit = dt["weight_unit"].ToString(),
                            weight = dt["weight"].ToString(),
                            price = dt["price"].ToString(),
                            status_flag = dt["status_flag"].ToString(),
                            shopify_productid = dt["shopify_productid"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_type = dt["product_type"].ToString(),
                            vendor_name = dt["vendor_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            sku = dt["sku"].ToString(),
                            grams = dt["grams"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            stockable = dt["stockable"].ToString(),

                            productuom_name = dt["productuom_name"].ToString(),

                            Status = dt["Status"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            lead_time = dt["lead_time"].ToString(),


                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Getting Shopify Product Summary!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetShopifyProductInventorySummary(MdlProduct values)
        {
            try
            {

                msSQL = " SELECT inventory_item_id,a.variant_id,a.inventory_quantity,a.old_inventory_quantity,d.producttype_name,b.productgroup_name,b.productgroup_code,a.product_gid, a.product_price, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,  CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
                        " c.productuomclass_code,e.productuom_code,c.productuomclass_name,a.product_type,a.vendor_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable,e.productuom_name,d.producttype_name as product_type,(case when a.status ='1' then 'active'  when a.status ='2' then 'draft'  when a.status ='draft' then 'draft' when a.status ='active' then 'active' when a.status ='inactive' then 'inactive' when a.status ='archived' then 'archived'   when a.status ='3' then 'archived' else 'inactive' end)  as Status," +
                        " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,(case when a.shopify_productid=s.shopify_productid then 'Assigned' when s.shopify_productid is null then 'Not Assign' end) as status_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time,a.shopify_productid,(case when a.product_image ='0' then 'no' else a.product_image end)  as product_image  from crm_smm_tshopifyproduct a " +
                        " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                        " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                        " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                        " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                          " left join ims_trn_tstock s on a.shopify_productid = s.shopify_productid " +
                        " left join adm_mst_tuser f on f.user_gid=a.created_by order by a.id desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productinventory_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productinventory_list
                        {

                            inventory_item_id = dt["inventory_item_id"].ToString(),
                            variant_id = dt["variant_id"].ToString(),
                            inventory_quantity = dt["inventory_quantity"].ToString(),
                            old_inventory_quantity = dt["old_inventory_quantity"].ToString(),
                            status_flag = dt["status_flag"].ToString(),
                            shopify_productid = dt["shopify_productid"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_type = dt["product_type"].ToString(),
                            vendor_name = dt["vendor_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),

                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            stockable = dt["stockable"].ToString(),

                            productuom_name = dt["productuom_name"].ToString(),

                            Status = dt["Status"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            lead_time = dt["lead_time"].ToString(),


                        });
                        values.productinventory_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Getting Shopify Product Inventory Summary!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetproducttypedropdown(MdlProduct values)
        {
            try
            {

                msSQL = " Select producttype_name,producttype_gid  " +
                 " from pmr_mst_tproducttype ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getproducttypedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproducttypedropdown
                        {
                            producttype_name = dt["producttype_name"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                        });
                        values.Getproducttypedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaGetproductgroupdropdown(MdlProduct values)
        {
            try
            {

                msSQL = " Select productgroup_gid, productgroup_name from pmr_mst_tproductgroup  " +
                   " order by productgroup_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getproductgroupdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproductgroupdropdown
                        {
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.Getproductgroupdropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

        public void DaGetproductunitclassdropdown(MdlProduct values)
        {
            try
            {

                msSQL = " Select productuomclass_gid, productuomclass_code, productuomclass_name  " +
                  " from pmr_mst_tproductuomclass order by productuomclass_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getproductunitclassdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproductunitclassdropdown
                        {
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                        });
                        values.Getproductunitclassdropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured getting product unit class!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaGetproductunitdropdown(MdlProduct values)
        {
            try
            {

                msSQL = " select productuom_name,productuom_gid from pmr_mst_tproductuom a left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid  " +
                  " order by a.sequence_level ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getproductunitdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproductunitdropdown
                        {
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),

                        });
                        values.Getproductunitdropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaGetcurrencydropdown(MdlProduct values)
        {
            try
            {

                msSQL = " select currency_code,currencyexchange_gid  " +
                   " from crm_trn_tcurrencyexchange ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getcurrencydropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcurrencydropdown
                        {
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),

                        });
                        values.Getcurrencydropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaPostShopifyProduct(string user_gid, shopifyproduct_list values)
        {
            try
            {

                msSQL = " select product_name from crm_smm_tshopifyproduct where product_name = '" + values.product_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "product Name Already Exist !!";
                }
                else
                {


                    msSQL = " SELECT access_token,shopify_store_name,store_month_year FROM crm_smm_shopify_service limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {

                        lsaccess_token = objOdbcDataReader["access_token"].ToString();
                        lsshopify_store_name = objOdbcDataReader["shopify_store_name"].ToString();
                        lsstore_month_year = objOdbcDataReader["store_month_year"].ToString();

                    }



                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                    var request = new RestRequest("/admin/api/" + lsstore_month_year + "/products.json", Method.POST);
                    request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Cookie", "request_method=PUT");
                    var body = "{\"product\":{\"title\":" + "\"" + values.product_name + "\"" + ",\"body_html\":" + "\"" + values.product_desc.Replace("'", "\\\'") + "\"" + ",\"product_type\":" + "\"" + values.producttype_name + "\"" + ",\"vendor\":" + "\"" + values.vendor + "\"" + ",\"status\":" + "\"" + values.product_status + "\"" + "}}";
                    var body_content = JsonConvert.DeserializeObject(body);
                    request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    productpost_list objMdlMailCampaignResponse = new productpost_list();
                    objMdlMailCampaignResponse = JsonConvert.DeserializeObject<productpost_list>(errornetsuiteJSON);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        // Parse the original date string to a DateTime object
                        DateTime originalDate = DateTime.ParseExact(objMdlMailCampaignResponse.product.created_at.ToString(), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                        // Convert the DateTime object to the desired format
                        string formattedDate = originalDate.ToString("yyyy-MM-dd");
                        msSQL = " insert into crm_smm_tshopifyproduct (" +
                             " shopify_productid," +
                               " product_name," +
                            " variant_id, " +
                             " product_type, " +
                             " inventory_item_id, " +
                             " option1, " +
                             " product_desc, " +
                            " inventory_quantity, " +
                            " old_inventory_quantity, " +
                            " status, " +
                            " product_image, " +
                            " grams, " +
                            " price, " +
                            " compare_at_price, " +
                            " vendor_name, " +
                               " created_by, " +
                               " created_date)" +
                               " values(" +
                               " '" + objMdlMailCampaignResponse.product.id + "',";
                        if (objMdlMailCampaignResponse.product.title == null || objMdlMailCampaignResponse.product.title == "")
                        {
                            msSQL += "'',";
                        }
                        else
                        {

                            msSQL += "'" + objMdlMailCampaignResponse.product.title.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "',";
                        }
                        msSQL += "'" + objMdlMailCampaignResponse.product.variants[0].id + "'," +
                            "'" + objMdlMailCampaignResponse.product.product_type + "'," +
                            "'" + objMdlMailCampaignResponse.product.variants[0].inventory_item_id + "'," +
                            "'" + objMdlMailCampaignResponse.product.variants[0].option1 + "'," +
                            "'" + objMdlMailCampaignResponse.product.body_html + "'," +
                        "'" + objMdlMailCampaignResponse.product.variants[0].inventory_quantity + "'," +
                        "'" + objMdlMailCampaignResponse.product.variants[0].old_inventory_quantity + "',";
                        if (objMdlMailCampaignResponse.product.status == "active")
                        {
                            msSQL += "'1',";
                        }
                        else if (objMdlMailCampaignResponse.product.status == "draft")
                        {
                            msSQL += "'3',";
                        }
                        else if (objMdlMailCampaignResponse.product.status == "archived")
                        {
                            msSQL += "'4',";
                        }
                        if (objMdlMailCampaignResponse.product.image == null)
                        {
                            msSQL += "'0',";
                        }
                        else
                        {
                            msSQL += "'" + objMdlMailCampaignResponse.product.images[0].src + "',";
                        }
                        msSQL += "'" + objMdlMailCampaignResponse.product.variants[0].grams + "'," +
                       "'" + objMdlMailCampaignResponse.product.variants[0].price + "'," +
                       "'" + objMdlMailCampaignResponse.product.variants[0].compare_at_price + "'," +
                       "'" + objMdlMailCampaignResponse.product.vendor + "'," +
                        "'" + user_gid + "'," +
                        "'" + formattedDate + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Product";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Product";
                    }
                }
                objOdbcDataReader.Close();
            }

            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Adding Shopify Product!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }




        }

        public void DaShopifyProductUpdate(string user_gid, shopifyproduct_list values)
        {
            try
            {

                msSQL = " select product_name,shopify_productid from crm_smm_tshopifyproduct where product_name = '" + values.product_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    lsproduct_name = objOdbcDataReader["product_name"].ToString();

                    lsshopify_productid = objOdbcDataReader["shopify_productid"].ToString();
                }
                if (lsproduct_name == null || lsproduct_name == "")
                {
                    lsproduct = null;
                }
                else
                {
                    lsproduct = lsproduct_name.ToUpper();

                }
                if (lsshopify_productid == null || lsshopify_productid == "")
                {
                    lsshopify = null;
                }
                else
                {
                    lsshopify = lsshopify_productid;
                }
                string productname = values.product_name.ToUpper();

                if (lsshopify == values.shopify_productid)
                {
                    msSQL = " SELECT access_token,shopify_store_name,store_month_year FROM crm_smm_shopify_service limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {

                        lsaccess_token = objOdbcDataReader["access_token"].ToString();
                        lsshopify_store_name = objOdbcDataReader["shopify_store_name"].ToString();
                        lsstore_month_year = objOdbcDataReader["store_month_year"].ToString();

                    }



                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                    var request = new RestRequest("/admin/api/" + lsstore_month_year + "/products/" + values.shopify_productid + ".json", Method.PUT);
                    request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Cookie", "request_method=PUT");
                    var body = "{\"product\":{\"title\":" + "\"" + values.product_name + "\"" + ",\"body_html\":" + "\"" + values.product_desc.Replace("'", "\\\'") + "\"" + ",\"vendor\":" + "\"" + values.vendor + "\"" + ",\"status\":" + "\"" + values.product_status + "\"" + ",\"product_type\":" + "\"" + values.producttype_name + "\"" + "}}";
                    var body_content = JsonConvert.DeserializeObject(body);
                    request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    productpost_list objMdlMailCampaignResponse = new productpost_list();
                    objMdlMailCampaignResponse = JsonConvert.DeserializeObject<productpost_list>(errornetsuiteJSON);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Parse the original date string to a DateTime object
                        DateTime originalDate = DateTime.ParseExact(objMdlMailCampaignResponse.product.created_at.ToString(), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                        // Convert the DateTime object to the desired format
                        string formattedDate = originalDate.ToString("yyyy-MM-dd");
                        msSQL = " update  crm_smm_tshopifyproduct  set " +
                " product_name = '" + objMdlMailCampaignResponse.product.title + "'," +
                " product_desc = '" + objMdlMailCampaignResponse.product.body_html + "'," +
                " inventory_item_id = '" + objMdlMailCampaignResponse.product.variants[0].inventory_item_id + "'," +
                " product_type = '" + objMdlMailCampaignResponse.product.product_type + "'," +
                " status = '" + objMdlMailCampaignResponse.product.status + "'," +
                " vendor_name = '" + objMdlMailCampaignResponse.product.vendor + "'," +
                " updated_by = '" + user_gid + "'," +
                " updated_date = '" + formattedDate + "' where shopify_productid='" + values.shopify_productid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Updated Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Product";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product";
                    }
                }
                else if (lsproduct != productname)
                {
                    msSQL = " SELECT access_token,shopify_store_name,store_month_year FROM crm_smm_shopify_service limit 1 ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {

                        lsaccess_token = objOdbcDataReader["access_token"].ToString();
                        lsshopify_store_name = objOdbcDataReader["shopify_store_name"].ToString();
                        lsstore_month_year = objOdbcDataReader["store_month_year"].ToString();

                    }



                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                    var request = new RestRequest("/admin/api/" + lsstore_month_year + "/products/" + values.shopify_productid + ".json", Method.PUT);
                    request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Cookie", "request_method=PUT");
                    var body = "{\"product\":{\"title\":" + "\"" + values.product_name + "\"" + ",\"body_html\":" + "\"" + values.product_desc + "\"" + ",\"vendor\":" + "\"" + values.vendor + "\"" + ",\"status\":" + "\"" + values.product_status + "\"" + ",\"product_type\":" + "\"" + values.producttype_name + "\"" + "}}";
                    var body_content = JsonConvert.DeserializeObject(body);
                    request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    productpost_list objMdlMailCampaignResponse = new productpost_list();
                    objMdlMailCampaignResponse = JsonConvert.DeserializeObject<productpost_list>(errornetsuiteJSON);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Parse the original date string to a DateTime object
                        DateTime originalDate = DateTime.ParseExact(objMdlMailCampaignResponse.product.created_at.ToString(), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                        // Convert the DateTime object to the desired format
                        string formattedDate = originalDate.ToString("yyyy-MM-dd");
                        msSQL = " update  crm_smm_tshopifyproduct  set " +
                " product_name = '" + objMdlMailCampaignResponse.product.title + "'," +
                " product_desc = '" + objMdlMailCampaignResponse.product.body_html + "'," +
                " product_type = '" + objMdlMailCampaignResponse.product.product_type + "'," +
                " status = '" + objMdlMailCampaignResponse.product.status + "'," +
                " vendor_name = '" + objMdlMailCampaignResponse.product.vendor + "'," +
                " updated_by = '" + user_gid + "'," +
                " updated_date = '" + formattedDate + "' where shopify_productid='" + values.shopify_productid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Updated Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Product";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Product Name Already Exist !";
                }

                objOdbcDataReader.Close();

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Updating Shopify Product!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostProduct(string user_gid, product_list values)
        {
            try
            {
                msSQL = " select * from smr_trn_tminsoftconfig;";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    base_url = objOdbcDataReader["base_url"].ToString();
                    api_key = objOdbcDataReader["api_key"].ToString();
                    lsclient_id = objOdbcDataReader["client_id"].ToString();
                }
                objOdbcDataReader.Close();


                msSQL = " select product_name from pmr_mst_tproduct where product_name = '" + values.product_name.Trim().Replace("'", "\\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "product Name Already Exist !!";
                }

                else
                {


                    msGetGid = objcmnfunctions.GetMasterGID("PPTM");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lsproduct_code = "PC" + "00" + lsCode;

                    msGetGid = objcmnfunctions.GetMasterGID("PPTM");
                    msSQL = " insert into pmr_mst_tproduct (" +
                       " product_gid," +
                       " product_code," +
                       " product_name," +
                   " product_desc, " +
                   " productgroup_gid, " +
                   " productuom_gid, " +
                   " mrp_price, " +
                   " cost_price, " +
                   " avg_lead_time, " +
                   " stockable, " +
                   " status, " +
                       " created_by, " +
                       " created_date)" +
                       " values(" +
                       " '" + msGetGid + "'," +
                       "'" + lsproduct_code + "',";
                    if (values.product_name.Trim().Replace("'", "\\\'") == null || values.product_name.Trim().Replace("'", "\\\'") == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.product_name.Trim().Replace("'", "\\'") + "',";
                    }
                    msSQL += "'" + values.product_desc.Replace("'", "\\\'") + "'," +
                             "'" + values.productgroup_name.Replace("'", "\\\'") + "'," +
                             "'" + values.productuom_name.Replace("'", "\\\'") + "',";
                    if (values.mrp_price == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.mrp_price + "',";
                    }
                    if (values.cost_price == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.cost_price + "',";
                    }
                    msSQL += "'" + values.avg_lead_time + "'," +
                    "'" + "Y" + "'," +
                    "'" + "1" + "'," +
                    "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    //if (mnResult != 0)
                    //{

                    //    values.status = true;
                    //    values.message = "Product Added Successfully !";

                    //}

                    if (mnResult != 0)
                    {
                        msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                        string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                        // code by snehith to push  product in mintsoft
                        if (mintsoft_flag == "Y")
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(base_url);

                            var request = new RestRequest("/api/Product/",Method.PUT);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("ms-apikey",api_key);
                            var jsonPayload = new
                            {
                                SKU = values.sku,
                                Name = values.product_name.Replace("'", "\\'"),
                                Description = values.product_desc.Replace("'", "\\\'"),
                                Price = values.cost_price,
                                ClientId= lsclient_id
                                //CostPrice = values.cost_price
                            };
                            request.AddJsonBody(jsonPayload);
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var responseData = JsonConvert.DeserializeObject<MintsoftProductpostResponse>(response.Content);
                                msSQL = "update pmr_mst_tproduct set " +
                                " mintsoftproduct_id  = '" + responseData.ProductId + "'" +
                                " where product_gid = '" + msGetGid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "Product Added Successfully";
                                }
                                else
                                {
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                   "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                    values.status = false;
                                    values.message = "Error While Creating Product";
                                }
                            }
                            else
                            {
                                string errorMessage = $"Failed to fetch products. Status code: {response.StatusCode}, Reason: {response.ErrorMessage}";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                values.status = false;
                                values.message = "Error While Creating Product";
                            }
                        }
                        else
                        {
                            values.status = true;
                            values.message = "Product Added Successfully";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Product";
                    }

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding product!";
            }



        }
        public class MintsoftProductpostResponse
        {
            public string ProductId { get; set; }
            public string SKU { get; set; }
            public string Name { get; set; }
            public string Success { get; set; }
            public string Message { get; set; }
        }
        public void DaProductUpdate(string user_gid, product_list values)
        {
            try
            {
                msSQL = " select * from smr_trn_tminsoftconfig;";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    base_url = objOdbcDataReader["base_url"].ToString();
                    api_key = objOdbcDataReader["api_key"].ToString();
                }
                objOdbcDataReader.Close();

                msSQL = " select product_gid,product_name from pmr_mst_tproduct where product_name='" + values.product_name.Trim().Replace("'", "\\\'") + "' ; ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)

                {
                    lsproductgid = objOdbcDataReader["product_gid"].ToString();
                    lsproductname = objOdbcDataReader["product_name"].ToString();
                }

                if (lsproductgid == values.product_gid)

                {
                    msSQL = " SELECT productgroup_gid FROM pmr_mst_tproductgroup WHERE productgroup_name='" + values.productgroupname + "' ";
                    string lsproductgroup_gid = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " SELECT productuom_gid FROM pmr_mst_tproductuom WHERE productuom_name='" + values.productuomname + "' ";
                    string lsproductuom_gid = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " update  pmr_mst_tproduct  set " +
                    " product_name = '" + values.product_name.Trim().Replace("'", "\\\'") + "'," +
                    " product_code = '" + values.product_code + "'," +
                    " product_desc = '" + values.product_desc.Replace("'", "\\\'") + "'," +
                    " currency_code = '" + values.currency_code + "'," +
                    " productgroup_gid = '" + lsproductgroup_gid + "'," +
                    " productuom_gid = '" + lsproductuom_gid + "'," +
                    " mrp_price = '" + values.mrp_price + "'," +
                    " cost_price = '" + values.cost_price + "'," +
                    " avg_lead_time = '" + values.avg_lead_time + "'," +
                    " updated_by = '" + user_gid + "'," +
                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where product_gid='" + values.product_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Product Updated Successfully";
                    }

                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product";
                    }
                }
                else
                {
                    if (string.Equals(lsproductname, values.product_name, StringComparison.OrdinalIgnoreCase))
                    {
                        values.status = false;
                        values.message = "product Name Already Exist !!";
                    }
                    else
                    {
                        msSQL = " SELECT productgroup_gid FROM pmr_mst_tproductgroup WHERE productgroup_name='" + values.productgroupname + "' ";
                        string lsproductgroup_gid = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " SELECT productuom_gid FROM pmr_mst_tproductuom WHERE productuom_name='" + values.productuomname + "' ";
                        string lsproductuom_gid = objdbconn.GetExecuteScalar(msSQL);


                        msSQL = " update  pmr_mst_tproduct  set " +
                        " product_name = '" + values.product_name.Trim().Replace("'", "\\\'") + "'," +
                        " product_code = '" + values.product_code + "'," +
                        " product_desc = '" + values.product_desc.Replace("'", "\\\'") + "'," +
                        " currency_code = '" + values.currency_code + "'," +
                        " productgroup_gid = '" + lsproductgroup_gid + "'," +
                        " productuom_gid = '" + lsproductuom_gid + "'," +
                        " mrp_price = '" + values.mrp_price + "'," +
                        " cost_price = '" + values.cost_price + "'," +
                        " avg_lead_time = '" + values.avg_lead_time + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where product_gid='" + values.product_gid + "'  ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                            string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                            // code by snehith to push  product in mintsoft
                            if (mintsoft_flag == "Y")
                            {
                                msSQL = " select mintsoftproduct_id from pmr_mst_tproduct  where product_gid='" + values.product_gid + "'  ";
                                string mintsoftproduct_id = objdbconn.GetExecuteScalar(msSQL);
                                var client = new RestClient(base_url);
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("Content-Type", "application/json");
                                request.AddHeader("ms-apikey", api_key);
                                var jsonPayload = new
                                {
                                    ID = mintsoftproduct_id,
                                    SKU = values.sku,
                                    Name = values.product_name.Replace("'", "\\'"),
                                    Description = values.product_desc.Replace("'", "\\\'"),
                                    Price = values.cost_price
                                    // CostPrice = values.cost_price
                                };
                                request.AddJsonBody(jsonPayload);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {

                                    values.status = true;
                                    values.message = "Product Updated Successfully";
                                }
                                else
                                {
                                    string errorMessage = $"Failed to fetch products. Status code: {response.StatusCode}, Reason: {response.ErrorMessage}";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                    values.status = false;
                                    values.message = "Error While Creating Product";
                                }
                            }
                            else
                            {
                                values.status = true;
                                values.message = "Product Updated Successfully";
                            }

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Product";
                        }
                    }

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }



        }
        public void DaGetEditProductSummary(string product_gid, MdlProduct values)
        {
            try
            {

                msSQL = "  select  a.batch_flag,a.serial_flag, a.purchasewarrenty_flag,a.expirytracking_flag,a.product_desc,a.avg_lead_time," +
  "  a.mrp_price,a.cost_price,a.product_gid,a.product_name,a.product_code,b.productgroup_gid,b.productgroup_name,c.productuomclass_gid,c.productuomclass_name," +
  "  d.producttype_gid,d.producttype_name,e.productuom_gid,e.productuom_name from pmr_mst_tproduct a " +
  " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
  " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid" +
  " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
  " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid" +
  " where a.product_gid='" + product_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEditProductSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditProductSummary
                        {


                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            batch_flag = dt["batch_flag"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            expirytracking_flag = dt["expirytracking_flag"].ToString(),
                            purchasewarrenty_flag = dt["purchasewarrenty_flag"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            avg_lead_time = dt["avg_lead_time"].ToString(),



                        });
                        values.GetEditProductSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting edit product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }


        }

        public void DaGetEditShopifyProductSummary(string shopifyproductid, MdlProduct values)
        {
            try
            {

                msSQL = "  select  a.batch_flag,a.serial_flag,(case when a.status ='1' then 'active'  when a.status ='2' then 'draft'  when a.status ='draft' then 'draft' when a.status ='active' then 'active' when a.status ='inactive' then 'inactive' when a.status ='archived' then 'archived'   when a.status ='3' then 'archived' else 'inactive' end) as product_status, a.purchasewarrenty_flag,a.expirytracking_flag,a.product_desc,a.avg_lead_time," +
             "  a.mrp_price,a.cost_price,a.product_gid,a.vendor_name,a.shopify_productid,a.id,a.product_name,a.product_type,a.product_code,b.productgroup_gid,b.productgroup_name,c.productuomclass_gid,c.productuomclass_name," +
             "  d.producttype_gid,d.producttype_name,e.productuom_gid,e.productuom_name from crm_smm_tshopifyproduct a " +
             " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
             " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid" +
             " left join pmr_mst_tproducttype d on a.product_type = d.producttype_name " +
             " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid" +
             " where a.shopify_productid='" + shopifyproductid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEditProductSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditProductSummary
                        {


                            id = dt["id"].ToString(),
                            shopify_productid = dt["shopify_productid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_status = dt["product_status"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            batch_flag = dt["batch_flag"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            expirytracking_flag = dt["expirytracking_flag"].ToString(),
                            purchasewarrenty_flag = dt["purchasewarrenty_flag"].ToString(),
                            product_type = dt["product_type"].ToString(),
                            vendor_name = dt["vendor_name"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            avg_lead_time = dt["avg_lead_time"].ToString(),



                        });
                        values.GetEditProductSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Getting Edit Shopify Product Summary!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetViewProductSummary(string product_gid, MdlProduct values)
        {
            try
            {

                msSQL = "  select CASE WHEN a.batch_flag = 'N' THEN 'NO' ELSE 'YES' END AS batch_flag,CASE WHEN a.serial_flag = 'N' THEN 'NO' ELSE 'YES' END AS serial_flag," +
        " CASE WHEN a.purchasewarrenty_flag = 'N' THEN 'NO' ELSE 'YES' END AS purchasewarrenty_flag,CASE WHEN a.expirytracking_flag = 'N' THEN 'NO' ELSE 'YES' END AS expirytracking_flag," +
        "  a.product_desc,a.product_image,a.avg_lead_time,a.mrp_price,e.producttype_name,a.cost_price,b.currency_code,a.product_gid,c.productgroup_name,a.product_name,f.productuomclass_name,a.product_code,d.productuom_name " +
        "  from pmr_mst_tproduct a " +
        "  left join crm_trn_tcurrencyexchange b on b.currency_code=a.currency_code" +
        "  left  join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid" +
        "  left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid" +
        "  left  join pmr_mst_tproducttype e on a.producttype_gid=e.producttype_gid" +
        "  left  join pmr_mst_tproductuomclass f on a.productuomclass_gid=f.productuomclass_gid" +
        "  where a.product_gid='" + product_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetViewProductSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetViewProductSummary
                        {


                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            batch_flag = dt["batch_flag"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            expirytracking_flag = dt["expirytracking_flag"].ToString(),
                            purchasewarrenty_flag = dt["purchasewarrenty_flag"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            avg_lead_time = dt["avg_lead_time"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            product_image = dt["product_image"].ToString(),

                        });
                        values.GetViewProductSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product view!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaGetOnChangeProductUnitClass(string productuomclass_gid, MdlProduct values)
        {
            try
            {

                msSQL = " select productuom_name,productuom_gid from pmr_mst_tproductuom a left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid  " +
                   " where b.productuomclass_gid ='" + productuomclass_gid + "' order by a.sequence_level  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductUnit>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductUnit
                        {
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),

                        });
                        values.GetProductUnit = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

        public void DaGetProductImage(HttpRequest httpRequest, result objResult, string user_gid)
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
            string product_gid = httpRequest.Form[0];

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

                        bool status1;


                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                        ms.Close();

                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                          msSQL = "update pmr_mst_tproduct set " +
                                                       " product_image='" + ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                       '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                       '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"] + "'" +
                                                        " where product_gid='" + product_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }


                if (mnResult != 0)
                {
                    objResult.status = true;
                    objResult.message = "Product Image Added Successfully !!";
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Adding Product Image !!";
                }

            }

            catch (Exception ex)
            {
                objResult.message = ex.ToString();
            }
            //return true;

        }
        public void DaGetdeleteproductdetails(string product_gid, product_list values)
        {
            try
            {
                msSQL = " select product_gid from pmr_trn_tpurchaserequisitiondtl " +
                        " where product_gid = '" + product_gid + "'";
                //lsproduct_gid = objdbconn.GetExecuteScalar(msSQL);
                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = " select product_gid from ims_trn_tmaterialrequisitiondtl " +
                        " where product_gid = '" + product_gid + "'";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);

                msSQL = " select product_gid from smr_trn_tsalesorderdtl " +
                        " where product_gid = '" + product_gid + "'";
                dt_datatable2 = objdbconn.GetDataTable(msSQL);

                msSQL = " select product_gid from rbl_trn_tinvoicedtl " +
                       " where product_gid = '" + product_gid + "'";
                dt_datatable3 = objdbconn.GetDataTable(msSQL);

                msSQL = " select product_gid from smr_trn_tsalesenquirydtl " +
                       " where product_gid = '" + product_gid + "'";
                dt_datatable4 = objdbconn.GetDataTable(msSQL);

                msSQL = " select product_gid from smr_trn_treceivequotationdtl " +
                       " where product_gid = '" + product_gid + "'";
                dt_datatable5 = objdbconn.GetDataTable(msSQL);

                msSQL = " select product_gid from pbl_trn_tserviceorderdtl " +
                       " where product_gid = '" + product_gid + "'";
                dt_datatable6 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count == 0 && dt_datatable1.Rows.Count == 0 && dt_datatable2.Rows.Count == 0 &&
                    dt_datatable3.Rows.Count == 0 && dt_datatable4.Rows.Count == 0 && dt_datatable5.Rows.Count == 0 && dt_datatable6.Rows.Count == 0)
                {
                    msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                    string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                    // code by snehith to push  product in mintsoft
                    if (mintsoft_flag == "Y")
                    {
                        msSQL = " select mintsoftproduct_id from pmr_mst_tproduct  where product_gid='" + product_gid + "'";
                        string mintsoftproduct_id = objdbconn.GetExecuteScalar(msSQL);
                        if (string.IsNullOrEmpty(mintsoftproduct_id))
                        {
                            msSQL = "  delete from pmr_mst_tproduct where product_gid='" + product_gid + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = "Product Deleted Successfully";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Deleting Product";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Product Available in Mintsoft Can't Delete";
                        }
                    }
                    else
                    {

                        msSQL = "  delete from pmr_mst_tproduct where product_gid='" + product_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Deleted Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Deleting Product";
                        }
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Cannot delete product  since it is involved in transactions!";

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting product details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaProductUploadExcels(HttpRequest httpRequest, string user_gid, result objResult, product_list values)
        {
            try
            {
                msSQL = "select employee_gid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                HttpFileCollection httpFileCollection;
                string lspath, lsfilePath,lscompany_code; ;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"] + lscompany_code + "/" + " Import_Excel/CRM/" + "Product_Master/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                if (FileExtension.IndexOf("Product Template", StringComparison.OrdinalIgnoreCase) >= 0)
                {

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

                    try
                    {
                        string status;
                        status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                        file.Close();
                        ms.Close();
                    }
                    catch (Exception ex)
                    {
                        objResult.status = false;
                        objResult.message = ex.ToString();
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        return;
                    }

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
                            objResult.status = false;
                            objResult.message = ex.Message;
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

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
                                    foreach (DataRow row in dataTable.Rows)
                                    {
                                        string product_group = row["PRODUCT GROUP"].ToString();
                                        string product = row["PRODUCT"].ToString();
                                        string product_description = row["PRODUCT DESCRIPTION"].ToString();
                                        string units = row["UNITS"].ToString();
                                        string cost_price = row["COST PRICE"].ToString();
                                        string sales_price = row["Sales Price"].ToString();

                                        msSQL = " select product_name from pmr_mst_tproduct where product_name = '" + product.Replace("'","\\'") + "'";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                        if (objOdbcDataReader.HasRows == true)
                                        {
                                            values.status = false;
                                            values.message = "product Name Already Exist !!";
                                        }

                                        else
                                        {
                                            msSQL = " select productgroup_name,productgroup_gid from pmr_mst_tproductgroup where productgroup_name = '" +(string.IsNullOrEmpty (product_group) ? product_group : product_group.Replace("'","\\'"))+ "'";
                                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                            if (objOdbcDataReader.HasRows == true)
                                            {
                                               productgroup_gid = objOdbcDataReader["productgroup_gid"].ToString();
                                            }
                                            else
                                            {
                                                msGetGid1 = objcmnfunctions.GetMasterGID("PPGM");
                                                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPGM' order by finyear desc limit 0,1 ";
                                                string lsCode1 = objdbconn.GetExecuteScalar(msSQL);

                                                string lsproductgroup_code = "PGM" + "000" + lsCode1;

                                                msSQL = " insert into pmr_mst_tproductgroup (" +
                                                            " productgroup_gid," +
                                                            " productgroup_code," +
                                                            " productgroup_name," +
                                                            " created_by, " +
                                                            " created_date)" +
                                                            " values(" +
                                                            " '" + msGetGid1 + "'," +
                                                            " '" + lsproductgroup_code + "'," +
                                                            "'" +(String.IsNullOrEmpty( product_group.Trim()) ? product_group.Trim() : product_group.Trim() .Replace("'","\\'"))+ "'," +
                                                            "'" + user_gid + "'," +
                                                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                productgroup_gid = msGetGid1;
                                            }
                                            msSQL = " select productuomclass_gid,productuom_gid from pmr_mst_tproductuom where productuom_name = '" + (String.IsNullOrEmpty(values.productuom_name)? values.productuom_name : values.productuom_name.Replace("'","\\'")) + "'";
                                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                            if (objOdbcDataReader.HasRows == true)
                                            {
                                                productuomclassgid = objOdbcDataReader["productuomclass_gid "].ToString();
                                                productuom_gid = objOdbcDataReader["productuom_gid "].ToString();
                                            }
                                            else
                                            {
                                                msSQL = " Select productuomclass_gid from pmr_mst_tproductuomclass where productuomclass_name = 'Others' limit 1 ";
                                                string productuomclass_gid = objdbconn.GetExecuteScalar(msSQL);

                                                msGetGid2 = objcmnfunctions.GetMasterGID("PPMM");
                                                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPMM' order by finyear desc limit 0,1 ";
                                                string lsCode3 = objdbconn.GetExecuteScalar(msSQL);

                                                string productuom_code = "PU" + "000" + lsCode3;

                                                msSQL = " insert into pmr_mst_tproductuom " +
                                                        " (productuom_gid, " +
                                                        " productuom_code," +
                                                        " productuom_name, " +
                                                        " productuomclass_gid " +
                                                        " ) values ( " +
                                                        " '" + msGetGid2 + "'," +
                                                        " '" + productuom_code + "'," +
                                                        " '" +(String.IsNullOrEmpty( units )? units: units.Replace("'","\\'")) + "'," +
                                                        " '" + productuomclass_gid + "')";

                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                                productuomclassgid = productuomclass_gid;
                                                productuom_gid = msGetGid2;

                                            }
                                          
                                            msGetGid = objcmnfunctions.GetMasterGID("PPTM");
                                            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                                            string lsCode = objdbconn.GetExecuteScalar(msSQL);

                                            string lsproduct_code = "PC" + "00" + lsCode;

                                            msGetGid = objcmnfunctions.GetMasterGID("PPTM");
                                            msSQL = " insert into pmr_mst_tproduct (" +
                                               " product_gid," +
                                               " product_code," +
                                               " product_name," +
                                               " productgroup_gid, " +
                                               " product_desc, " +
                                               " productuom_gid, " +
                                               " mrp_price, " +
                                               " cost_price, " +
                                               " stockable, " +
                                               " status, " +
                                               " created_by, " +
                                               " created_date)" +
                                               " values(" +
                                               " '" + msGetGid + "'," +
                                               "'" + lsproduct_code + "',";
                                            if (product.Trim().Replace("'", "\\'") == null || product.Trim().Replace("'", "\\'") == "")
                                            {
                                                msSQL += "'',";
                                            }
                                            else
                                            {
                                                msSQL += "'" + product.Trim().Replace("'", "\\'") + "',";
                                            }
                                            msSQL += "'" + productgroup_gid + "'," +
                                                     "'" +(String.IsNullOrEmpty( product_description)? product_description : product_description .Replace("'","\\'"))+ "'," +
                                                     "'" + productuom_gid + "',";
                                            if (sales_price == "")
                                            {
                                                msSQL += "'0.00',";
                                            }
                                            else
                                            {
                                                msSQL += "'" + sales_price + "',";
                                            }
                                            if (cost_price == "")
                                            {
                                                msSQL += "'0.00',";
                                            }
                                            else
                                            {
                                                msSQL += "'" + cost_price + "',";
                                            }
                                            msSQL += "'" + "Y" + "'," +
                                            "'" + "1" + "'," +
                                            "'" + user_gid + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult != 0)
                                            {
                                                objResult.status = true;
                                                objResult.message = "Product Imported Successfully";
                                            }
                                            else
                                            {
                                                objResult.status = false;
                                                objResult.message = "Error While Adding Product";
                                            }

                                        }
                                        objOdbcDataReader.Close();
                                      
                                    }

                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        objResult.status = false;
                        objResult.message = ex.Message;
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        return;
                    }
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Invalid file format. Please upload a valid Excel file";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + objResult.message + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while uploading product excel!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
             

        }
        public void DaGetProductReportExport(MdlProduct values)
        {
            try
            {

                msSQL = " SELECT d.producttype_name as 'Product Type',b.productgroup_name as 'Product Group', a.product_code as 'Product Code', CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as 'Product',c.productuomclass_name as 'Unit', a.cost_price as 'Cost Price', " +
        " (case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as 'Avg Lead Time',(case when a.status ='1' then 'Active' else 'Inactive' end) as 'Product Status'" +
        "   from pmr_mst_tproduct a " +
        " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
        " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
        " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
        " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
        " left join adm_mst_tuser f on f.user_gid=a.created_by order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                string lscompany_code = string.Empty;
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Product Report");
                try
                {
                    msSQL = " select company_code from adm_mst_tcompany";

                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    string lspath = ConfigurationManager.AppSettings["exportexcelfile"] + "/product/export" + "/" + lscompany_code + "/" + "CRM/Product/Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                    //values.lspath = ConfigurationManager.AppSettings["file_path"] + "/erp_documents" + "/" + lscompany_code + "/" + "SDC/TestReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                    {
                        if ((!System.IO.Directory.Exists(lspath)))
                            System.IO.Directory.CreateDirectory(lspath);
                    }

                    string lsname = "Product_Report" + DateTime.Now.ToString("(dd-MM-yyyy HH-mm-ss)") + ".xlsx";
                    string lspath1 = ConfigurationManager.AppSettings["exportexcelfile"] + "/product/export" + "/" + lscompany_code + "/" + "CRM/Product/Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsname;

                    workSheet.Cells[1, 1].LoadFromDataTable(dt_datatable, true);
                    FileInfo file = new FileInfo(lspath1);
                    using (var range = workSheet.Cells[1, 1, 1, 8])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                    }
                    excel.SaveAs(file);

                    var getModuleList = new List<productexport_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {

                        getModuleList.Add(new productexport_list
                        {

                            lsname = lsname,
                            lspath1 = lspath1,



                        });
                        values.productexport_list = getModuleList;

                    }
                    dt_datatable.Dispose();
                    values.status = true;
                    values.message = "Success";
                }
                catch (Exception ex)
                {
                    values.status = false;
                    values.message = "Failure";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product report excel!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DadownloadImages(string product_gid, MdlProduct values)
        {
            try
            {

                msSQL = "SELECT product_image, name FROM pmr_mst_tproduct WHERE product_gid = '" + product_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_images>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_images
                        {
                            product_image = dt["product_image"].ToString(),
                            name = dt["name"].ToString()
                        });
                    }
                    values.product_images = getModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while downloading image!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public result DaSendproductmaster(string user_gid, shopifyproductmove_list values)
        {
            result result = new result();
            try
            {

                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    FnLoadproduct(values.shopifyproduct_lists, user_gid);
                }));
                t.Start();
                result.status = true;
                result.message = "Products Moving to CRM!";
                //finally
                //{
                //    if (objOdbcDataReader != null)
                //        objOdbcDataReader.Close();
                //    objdbconn.CloseConn();
                //}
            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Message";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            return result;

        }
        public void FnLoadproduct(shopifyproduct_lists[] objMdlShopifyMessageResponse, string user_gid)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                //objdbconn.OpenConn();
                for (int i = 0; i < objMdlShopifyMessageResponse.Length; i++)
                {

                    msSQL = " select shopify_productid  from pmr_mst_tproduct where shopify_productid = '" + objMdlShopifyMessageResponse[i].shopify_productid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows != true)
                    {
                        if (objMdlShopifyMessageResponse[i].weight != null && objMdlShopifyMessageResponse[i].weight != "")
                        {
                            msSQL = " select productuomclass_gid  from pmr_mst_tproductuomclass where productuomclass_name = '" + objMdlShopifyMessageResponse[i].weight + "' limit 1 ";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows)
                            {
                                lsproductuomclass_gid = objOdbcDataReader["productuomclass_gid"].ToString();

                            }
                            objOdbcDataReader.Close();
                            if (lsproductuomclass_gid != null)
                            {
                                lsproductuomclassgid = lsproductuomclass_gid;
                            }
                        }

                        else
                        {
                            if (lsproductuomclassgid != null && lsproductuomclassgid != "")
                            {
                                msGetGid = objcmnfunctions.GetMasterGID("PUCM");
                                msSQL = " insert into pmr_mst_tproductuomclass (" +
                                " productuomclass_gid," +
                                " productuomclass_name ," +
                                " created_date)" +
                                " values(" +
                                "'" + msGetGid + "',";
                                if (objMdlShopifyMessageResponse[i].weight == null || objMdlShopifyMessageResponse[i].weight == "")
                                {
                                    msSQL += "'',";
                                }
                                else
                                {
                                    msSQL += "'" + objMdlShopifyMessageResponse[i].weight.Replace("'", "\\'") + "',";
                                }
                                msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                lsproductuomclassgid = msGetGid;
                            }
                            else
                            {
                                lsproductuomclassgid = "";
                            }
                        }

                        msSQL = " select productuom_gid  from pmr_mst_tproductuom where productuomclass_gid = '" + lsproductuomclass_gid + "' and productuom_name = '" + objMdlShopifyMessageResponse[i].weight_unit + "' limit 1 ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();

                        }
                        objOdbcDataReader.Close();
                        if (lsproductuom_gid != null)
                        {
                            lsproductuomgid = lsproductuom_gid;
                        }

                        else
                        {
                            if (lsproductuom_gid != null && lsproductuom_gid != "")
                            {
                                msGetGid1 = objcmnfunctions.GetMasterGID("PPMM");
                                msSQL = " insert into pmr_mst_tproductuom (" +
                                " productuom_gid," +
                             " productuomclass_gid," +
                             " productuom_name ," +
                             " created_by, " +
                             " created_date)" +
                             " values(" +
                             "'" + msGetGid1 + "'," +
                             " '" + lsproductuomclass_gid + "'," +
                                 "'" + objMdlShopifyMessageResponse[i].weight_unit + "'," +
                                "'" + user_gid + "',";
                                msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                lsproductuomgid = msGetGid1;
                            }
                            else
                            {

                                lsproductuomgid = "";
                            }

                        }




                        msSQL = " select productgroup_gid from pmr_mst_tproductgroup where productgroup_name = '" + objMdlShopifyMessageResponse[i].product_type + "'  limit 1 ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsproductgroup_gid = objOdbcDataReader["productgroup_gid"].ToString();

                        }
                        objOdbcDataReader.Close();
                        if (lsproductgroup_gid != null)
                        {
                            lsproductgroupgid = lsproductgroup_gid;
                        }
                        else
                        {
                            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPGM' order by finyear desc limit 0,1 ";
                            string lsCode1 = objdbconn.GetExecuteScalar(msSQL);

                            string lsproductgroup_code = "PGC" + "00" + lsCode1;
                            msGetGid5 = objcmnfunctions.GetMasterGID("PPGM");

                            msSQL = " insert into pmr_mst_tproductgroup (" +
                                        " productgroup_gid," +
                                        " productgroup_code," +
                                        " productgroup_name," +
                                        " created_by, " +
                                        " created_date)" +
                                        " values(" +
                                        " '" + msGetGid5 + "'," +
                                        " '" + lsproductgroup_code + "'," +
                                        "'" + objMdlShopifyMessageResponse[i].product_type + "'," +
                                        "'" + user_gid + "'," +
                                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsproductgroupgid = msGetGid5;

                        }





                        msSQL = " select producttype_gid from pmr_mst_tproducttype where producttype_name  = '" + objMdlShopifyMessageResponse[i].product_type + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsproducttype_gid = objOdbcDataReader["producttype_gid"].ToString();

                        }
                        objOdbcDataReader.Close();
                        if (lsproducttype_gid != null)
                        {
                            lsproducttypegid = lsproducttype_gid;
                        }
                        else
                        {
                            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                            string lsCode6 = objdbconn.GetExecuteScalar(msSQL);

                            string lsproduct_typegid = "PG" + "00" + lsCode6;

                            msSQL = " insert into pmr_mst_tproducttype (" +
                            " producttype_gid," +
                         " producttype_name)" +
                         " values(" +
                         "'" + lsproduct_typegid + "',";
                            msSQL += "'" + objMdlShopifyMessageResponse[i].product_type + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsproducttypegid = lsproduct_typegid;

                        }

                        msGetGid = objcmnfunctions.GetMasterGID("PPTM");
                        msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                        string lsCode7 = objdbconn.GetExecuteScalar(msSQL);

                        string lsproduct_code = "PC" + "00" + lsCode7;


                        msGetGid9 = objcmnfunctions.GetMasterGID("PPTM");
                        msSQL = " insert into pmr_mst_tproduct (" +
                               " product_gid," +
                               " product_code," +
                               " product_name," +
                            " productgroup_gid, " +
                            " productuomclass_gid, " +
                            " productuom_gid, " +
                            " stockable, " +
                            " producttype_gid, " +
                            " variant_id, " +
                            " customerproduct_code, " +
                            " inventory_quantity, " +
                            " inventory_item_id, " +
                            " old_inventory_quantity, " +
                            " status, " +
                            " product_image, " +
                            " grams, " +
                            " cost_price, " +
                            " mrp_price, " +
                            " shopify_productid," +
                               " created_by, " +
                               " created_date)" +
                               " values(" +
                               " '" + msGetGid9 + "'," +
                               "'" + lsproduct_code + "',";
                        if (objMdlShopifyMessageResponse[i].product_name == null || objMdlShopifyMessageResponse[i].product_name == "")
                        {
                            msSQL += "'',";
                        }
                        else
                        {

                            msSQL += "'" + objMdlShopifyMessageResponse[i].product_name.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "',";
                        }
                        msSQL += "'" + lsproductgroupgid + "'," +
                                 "'" + lsproductuomclassgid + "'," +
                                 "'" + lsproductuomgid + "'," +
                        "'" + "Y" + "'," +
                        "'" + lsproducttypegid + "'," +
                        "'" + objMdlShopifyMessageResponse[i].variant_id + "'," +
                        "'" + objMdlShopifyMessageResponse[i].sku + "'," +
                        "'" + objMdlShopifyMessageResponse[i].inventory_quantity + "'," +
                       "'" + objMdlShopifyMessageResponse[i].inventory_item_id + "'," +
                        "'" + objMdlShopifyMessageResponse[i].old_inventory_quantity + "',";
                        if (objMdlShopifyMessageResponse[i].Status == "active")
                        {
                            msSQL += "'1',";
                        }
                        else if (objMdlShopifyMessageResponse[i].Status == "draft")
                        {
                            msSQL += "'3',";
                        }
                        else if (objMdlShopifyMessageResponse[i].Status == "archived")
                        {
                            msSQL += "'4',";
                        }
                        if (objMdlShopifyMessageResponse[i].product_image == null)
                        {
                            msSQL += "'0',";
                        }
                        else
                        {
                            msSQL += "'" + objMdlShopifyMessageResponse[i].product_image + "',";
                        }
                        msSQL += "'" + objMdlShopifyMessageResponse[i].grams + "'," +
                       "'" + objMdlShopifyMessageResponse[i].price + "'," +
                       "'" + objMdlShopifyMessageResponse[i].price + "'," +
                        "'" + objMdlShopifyMessageResponse[i].shopify_productid + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                    objOdbcDataReader.Close();

                }



            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Sending product master!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaUpdateShopifyProductPrice(string user_gid, productprice_list values)
        {
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

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                var request = new RestRequest("/admin/api/" + lsstore_month_year + "/variants/" + values.variant_id + ".json", Method.PUT);
                request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", "request_method=PUT");
                var body = "{\"variant\":{\"option1\":" + "\"" + values.attribute + "\"" + ",\"price\":" + "\"" + values.price + "\"" + ",\"inventory_management\":\"shopify\"}}";
                var body_content = JsonConvert.DeserializeObject(body);
                request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    msSQL = " update  crm_smm_tshopifyproduct  set " +
            " price = '" + values.price + "'," +
            " option1 = '" + values.attribute + "'," +
            " updated_by = '" + user_gid + "'," +
            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where shopify_productid='" + values.shopify_productid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update  pmr_mst_tproduct  set " +
                        " mrp_price = '" + values.price + "'," +
                        " option1 = '" + values.attribute + "'," +
                        " cost_price = '" + values.price + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where shopify_productid='" + values.shopify_productid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Price Updated Successfully !";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Product Price";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product Price";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Product Price";
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Update Shopify Product Price!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaUpdateShopifyProductQuantity(string user_gid, productquantity_list values)
        {
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
                msSQL = " SELECT location_id FROM crm_smm_tshopifylocation limit 1 ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {

                    lslocation_id = objOdbcDataReader["location_id"].ToString();


                }


                objOdbcDataReader.Close();


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                var request = new RestRequest("/admin/api/" + lsstore_month_year + "/inventory_levels/set.json", Method.POST);
                request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"location_id\":" + "\"" + lslocation_id + "\"" + ",\"inventory_item_id\":" + "\"" + values.inventory_item_id + "\"" + ",\"available\":" + "\"" + values.inventory_quantity + "\"" + "}";
                var body_content = JsonConvert.DeserializeObject(body);
                request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    msSQL = " update  crm_smm_tshopifyproduct  set " +
            " inventory_quantity = '" + values.inventory_quantity + "'," +
            " old_inventory_quantity = '" + values.inventory_quantity + "'," +
            " updated_by = '" + user_gid + "'," +
            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where shopify_productid='" + values.shopify_productid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update  pmr_mst_tproduct  set " +
                          " inventory_quantity = '" + values.inventory_quantity + "'," +
                          " old_inventory_quantity = '" + values.inventory_quantity + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where shopify_productid='" + values.shopify_productid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update  ims_trn_tstock  set " +
                         " stock_qty = '" + values.inventory_quantity + "'," +
                         " grn_qty = '" + values.inventory_quantity + "'," +
                       " updated_by = '" + user_gid + "'," +
                       " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where shopify_productid='" + values.shopify_productid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        values.status = true;
                        values.message = "Inventory Quantity Updated Successfully !";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Inventory Quantity";
                    }
                }
                else if (response.StatusDescription == "Unprocessable Entity")
                {
                    values.status = false;
                    values.message = "Kindly Set the Product Price First !";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Inventory Quantity";
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Update Shopify Product Quantity !! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }
        public getproduct DaGetShopifyLocation(string user_gid)
        {
            getproduct objresult = new getproduct();
            msSQL = " SELECT access_token,shopify_store_name,store_month_year FROM crm_smm_shopify_service limit 1 ";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {

                lsaccess_token = objOdbcDataReader["access_token"].ToString();
                lsshopify_store_name = objOdbcDataReader["shopify_store_name"].ToString();
                lsstore_month_year = objOdbcDataReader["store_month_year"].ToString();

            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
            var request = new RestRequest("/admin/api/" + lsstore_month_year + "/locations.json?limit=250", Method.GET);
            request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
            IRestResponse response = client.Execute(request);
            string response_content = response.Content;
            locationlist objMdlShopifyMessageResponse = new locationlist();
            objMdlShopifyMessageResponse = JsonConvert.DeserializeObject<locationlist>(response_content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                msSQL = "select location_id from crm_smm_tshopifylocation where location_id='" + objMdlShopifyMessageResponse.locations[0].id + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows != true)
                {
                    msSQL = " insert into crm_smm_tshopifylocation (" +
                         " location_id," +
                            " name)" +
                            " values(" +
                            " '" + objMdlShopifyMessageResponse.locations[0].id + "'," +
                        "'" + objMdlShopifyMessageResponse.locations[0].name + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                objOdbcDataReader.Close();
            }
            return objresult;

        }
        public void DaUpdateShopifyProductImage(string user_gid, updateshopifyimage_list values)
        {
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



                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                var request = new RestRequest("/admin/api/" + lsstore_month_year + "/products/" + values.shopify_productid + "/images.json", Method.POST);
                request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"image\":{\"position\":\"1\",\"attachment\":\"" + values.path + "\"}}";
                var body_content = JsonConvert.DeserializeObject(body);
                request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string errornetsuiteJSON = response.Content;
                shopifyproductImage_list objMdlMailCampaignResponse = new shopifyproductImage_list();
                objMdlMailCampaignResponse = JsonConvert.DeserializeObject<shopifyproductImage_list>(errornetsuiteJSON);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    msSQL = " update  crm_smm_tshopifyproduct  set " +
                    " product_image = '" + objMdlMailCampaignResponse.image.src + "'," +
                    " updated_by = '" + user_gid + "'," +
                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where shopify_productid='" + values.shopify_productid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update  pmr_mst_tproduct  set " +
                        " product_image = '" + objMdlMailCampaignResponse.image.src + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where shopify_productid='" + values.shopify_productid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Image Updated Successfully !";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Product Image";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Product Image";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Product Image";
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Update Shopify Product Image !! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetcustomerInactive(string product_gid, MdlProduct values)
        {
            try
            {

                msSQL = " update pmr_mst_tproduct set" +
                        " status='2'" +
                        " where product_gid = '" + product_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Inactivated Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Customer Inactivated";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while   Updating Customer Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetcustomerActive(string product_gid, MdlProduct values)
        {
            try
            {

                msSQL = " update pmr_mst_tproduct set" +
                        " status='1'" +
                        " where product_gid = '" + product_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Activated Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Customer Activated";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Customer Activated!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetsyncshopify(MdlProduct values)
        {
            try
            {

                msSQL = " SELECT a.option1,a.inventory_quantity,a.old_inventory_quantity,a.variant_id,a.inventory_item_id,a.weight_unit,a.weight,a.price,a.grams,d.producttype_name,b.productgroup_name,b.productgroup_code,a.product_gid, a.product_price, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,  CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
        " c.productuomclass_code,e.productuom_code,c.productuomclass_name,a.product_type,a.vendor_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable,e.productuom_name,d.producttype_name as product_type,(case when a.status ='1' then 'active'  when a.status ='2' then 'draft'  when a.status ='draft' then 'draft' when a.status ='active' then 'active' when a.status ='inactive' then 'inactive' when a.status ='archived' then 'archived'   when a.status ='3' then 'archived' else 'inactive' end)  as Status," +
        " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,(case when a.shopify_productid=s.shopify_productid then 'Assigned' when s.shopify_productid is null then 'Not Assign' end) as status_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time,a.shopify_productid,(case when a.product_image ='0' then 'no' else a.product_image end)  as product_image  from crm_smm_tshopifyproduct a " +
        " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
        " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
        " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
        " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
          " left join pmr_mst_tproduct s on a.shopify_productid = s.shopify_productid " +
        " left join adm_mst_tuser f on f.user_gid=a.created_by WHERE a.product_name NOT IN (SELECT product_name FROM pmr_mst_tproduct) ORDER BY a.id DESC;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list
                        {
                            option1 = dt["option1"].ToString(),
                            inventory_quantity = dt["inventory_quantity"].ToString(),
                            old_inventory_quantity = dt["old_inventory_quantity"].ToString(),
                            variant_id = dt["variant_id"].ToString(),
                            inventory_item_id = dt["inventory_item_id"].ToString(),
                            weight_unit = dt["weight_unit"].ToString(),
                            weight = dt["weight"].ToString(),
                            price = dt["price"].ToString(),
                            status_flag = dt["status_flag"].ToString(),
                            shopify_productid = dt["shopify_productid"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_type = dt["product_type"].ToString(),
                            vendor_name = dt["vendor_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            grams = dt["grams"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            stockable = dt["stockable"].ToString(),

                            productuom_name = dt["productuom_name"].ToString(),

                            Status = dt["Status"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            lead_time = dt["lead_time"].ToString(),


                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

        public void DaGetsync_crm(MdlProduct values)
        {
            try
            {

                msSQL = "SELECT d.producttype_name, b.productgroup_name, b.productgroup_code, a.product_gid, a.product_price, a.cost_price," +
                    " a.product_code, CONCAT_WS('|', a.product_name, a.size, a.width, a.length) AS product_name, CONCAT(f.user_firstname,' '," +
                    "f.user_lastname) AS created_by, DATE_FORMAT(a.created_date,'%d-%m-%Y') AS created_date, c.productuomclass_code, " +
                    "e.productuom_code, c.productuomclass_name, (CASE WHEN a.stockable ='Y' THEN 'Yes' ELSE 'No ' END) AS stockable, " +
                    "e.productuom_name, d.producttype_name AS product_type, CASE WHEN a.status = '1' THEN 'Active' WHEN a.status IS NULL" +
                    " OR a.status = '' OR a.status = '2' THEN 'Inactive' END AS Status, (CASE WHEN a.serial_flag ='Y' THEN 'Yes' ELSE 'No' END) " +
                    "AS serial_flag, (CASE WHEN a.avg_lead_time IS NULL THEN '0 days' ELSE CONCAT(a.avg_lead_time,' ', 'days') END) AS lead_time," +
                    " (CASE WHEN a.shopify_productid IS NOT NULL THEN 'No' ELSE 'Yes' END) AS shopify_productid, a.product_image, " +
                    "(SELECT COUNT(product_gid) FROM pmr_mst_tproduct) AS product_count FROM pmr_mst_tproduct a LEFT JOIN pmr_mst_tproductgroup b" +
                    " ON a.productgroup_gid = b.productgroup_gid LEFT JOIN pmr_mst_tproductuomclass c ON a.productuomclass_gid = c.productuomclass_gid" +
                    " LEFT JOIN pmr_mst_tproducttype d ON a.producttype_gid = d.producttype_gid LEFT JOIN pmr_mst_tproductuom e ON" +
                    " a.productuom_gid = e.productuom_gid LEFT JOIN adm_mst_tuser f ON f.user_gid = a.created_by WHERE a.product_name NOT IN" +
                    " (SELECT product_name FROM crm_smm_tshopifyproduct) ORDER BY a.created_date DESC;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list
                        {
                            shopify_productid = dt["shopify_productid"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            product_count = dt["product_count"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            stockable = dt["stockable"].ToString(),

                            productuom_name = dt["productuom_name"].ToString(),
                            product_type = dt["product_type"].ToString(),
                            Status = dt["Status"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            lead_time = dt["lead_time"].ToString(),


                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }



        }

        public result DaPostsynccrm(string user_gid, shopifyproductmove_list values)
        {
            result result = new result();
            try
            {

                HttpContext ctx = HttpContext.Current;
                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                {
                    HttpContext.Current = ctx;
                    Fnloadsynccrm(user_gid, values.shopifyproduct_lists);
                }));
                t.Start();
                result.status = true;
                result.message = "Products Moving to Shopify!";
                //finally
                //{
                //    if (objOdbcDataReader != null)
                //        objOdbcDataReader.Close();
                //    objdbconn.CloseConn();
                //}
            }
            catch (Exception ex)
            {
                result.message = "Error while Moving Products";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            return result;

        }

        public void Fnloadsynccrm(string user_gid, shopifyproduct_lists[] objMdlShopifyMessageResponse)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                //objdbconn.OpenConn();
                for (int i = 0; i < objMdlShopifyMessageResponse.Length; i++)
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
                    var request = new RestRequest("/admin/api/" + lsstore_month_year + "/products.json", Method.POST);
                    request.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Cookie", "request_method=PUT");
                    var body = "{\"product\":{\"title\":" + "\"" + objMdlShopifyMessageResponse[i].product_name + "\"" + ",\"product_type\":" + "\"" + objMdlShopifyMessageResponse[i].producttype_name + "\"" + ",\"status\":" + "\"" + "active" + "\"" + "}}";
                    var body_content = JsonConvert.DeserializeObject(body);
                    request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    productpost_list objMdlMailCampaignResponse = new productpost_list();
                    objMdlMailCampaignResponse = JsonConvert.DeserializeObject<productpost_list>(errornetsuiteJSON);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        // Parse the original date string to a DateTime object
                        DateTime originalDate = DateTime.ParseExact(objMdlMailCampaignResponse.product.created_at.ToString(), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                        // Convert the DateTime object to the desired format
                        string formattedDate = originalDate.ToString("yyyy-MM-dd");
                        msSQL = " insert into crm_smm_tshopifyproduct (" +
                             " shopify_productid," +
                               " product_name," +
                            " variant_id, " +
                             " product_type, " +
                             " inventory_item_id, " +
                             " option1, " +
                             " product_desc, " +
                            " inventory_quantity, " +
                            " old_inventory_quantity, " +
                            " status, " +
                            " product_image, " +
                            " grams, " +
                            " price, " +
                            " compare_at_price, " +
                            " vendor_name, " +
                               " created_by, " +
                               " created_date)" +
                               " values(" +
                               " '" + objMdlMailCampaignResponse.product.id + "',";
                        if (objMdlMailCampaignResponse.product.title == null || objMdlMailCampaignResponse.product.title == "")
                        {
                            msSQL += "'',";
                        }
                        else
                        {

                            msSQL += "'" + objMdlMailCampaignResponse.product.title.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "',";
                        }
                        msSQL += "'" + objMdlMailCampaignResponse.product.variants[0].id + "'," +
                            "'" + objMdlMailCampaignResponse.product.product_type + "'," +
                            "'" + objMdlMailCampaignResponse.product.variants[0].inventory_item_id + "'," +
                            "'" + objMdlMailCampaignResponse.product.variants[0].option1 + "'," +
                            "'" + objMdlMailCampaignResponse.product.body_html + "'," +
                        "'" + objMdlMailCampaignResponse.product.variants[0].inventory_quantity + "'," +
                        "'" + objMdlMailCampaignResponse.product.variants[0].old_inventory_quantity + "',";
                        if (objMdlMailCampaignResponse.product.status == "active")
                        {
                            msSQL += "'1',";
                        }
                        else if (objMdlMailCampaignResponse.product.status == "draft")
                        {
                            msSQL += "'3',";
                        }
                        else if (objMdlMailCampaignResponse.product.status == "archived")
                        {
                            msSQL += "'4',";
                        }
                        if (objMdlShopifyMessageResponse[i].product_image == null)
                        {
                            msSQL += "'0',";
                        }
                        else
                        {
                            msSQL += "'" + objMdlShopifyMessageResponse[i].product_image + "',";
                        }
                        msSQL += "'" + objMdlMailCampaignResponse.product.variants[0].grams + "'," +
                       "'" + objMdlShopifyMessageResponse[i].cost_price + "'," +
                       "'" + objMdlMailCampaignResponse.product.variants[0].compare_at_price + "'," +
                       "'" + objMdlMailCampaignResponse.product.vendor + "'," +
                        "'" + user_gid + "'," +
                        "'" + formattedDate + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update pmr_mst_tproduct set shopify_productid='" + objMdlMailCampaignResponse.product.id + "' where product_name='" + objMdlShopifyMessageResponse[i].product_name + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " SELECT location_id FROM crm_smm_tshopifylocation limit 1 ";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows)
                            {

                                lslocation_id = objOdbcDataReader["location_id"].ToString();


                            }

                            objOdbcDataReader.Close();

                            var client1 = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                            var request1 = new RestRequest("/admin/api/" + lsstore_month_year + "/variants/" + objMdlMailCampaignResponse.product.variants[0].id + ".json", Method.PUT);
                            request1.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                            request1.AddHeader("Content-Type", "application/json");
                            request1.AddHeader("Cookie", "request_method=PUT");
                            var body1 = "{\"variant\":{\"option1\":" + "\"Default\"" + ",\"price\":" + "\"" + objMdlShopifyMessageResponse[i].cost_price + "\"" + ",\"inventory_management\":\"shopify\"}}";
                            var body_content1 = JsonConvert.DeserializeObject(body1);
                            request1.AddParameter("application/json", body_content1, ParameterType.RequestBody);
                            IRestResponse response1 = client1.Execute(request1);

                            //byte[] urlBytes = Encoding.UTF8.GetBytes(objMdlShopifyMessageResponse[i].product_image);

                            //string base64Url = Convert.ToBase64String(urlBytes)
                            //    ;
                            string encodedUrl = Convert.ToBase64String(Encoding.Default.GetBytes(objMdlShopifyMessageResponse[i].product_image));

                            using (var clients = new WebClient())
                            {
                                byte[] dataBytes = clients.DownloadData(new Uri(objMdlShopifyMessageResponse[i].product_image));
                                string encodedFileAsBase64 = Convert.ToBase64String(dataBytes);


                                var client2 = new RestClient("https://" + lsshopify_store_name + ".myshopify.com");
                                var request2 = new RestRequest("/admin/api/" + lsstore_month_year + "/products/" + objMdlMailCampaignResponse.product.id + "/images.json", Method.POST);
                                request2.AddHeader("X-Shopify-Access-Token", "" + lsaccess_token + "");
                                request2.AddHeader("Content-Type", "application/json");
                                var body2 = "{\"image\":{\"position\":\"1\",\"attachment\":\"" + encodedFileAsBase64 + "\"}}";
                                var body_content2 = JsonConvert.DeserializeObject(body2);
                                request2.AddParameter("application/json", body_content2, ParameterType.RequestBody);
                                IRestResponse response2 = client2.Execute(request2);
                            }
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Adding Shopify Product!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }




        }

        public void DaAddproducttowhatsapp(string user_gid,string branch_gid,string product_gid, product_list values)
        {
            try
            {
                whatsappconfiguration1 getwhatsappcredentials = whatsappcredentials1(branch_gid);

                msSQL = "SELECT currency_code FROM crm_trn_tcurrencyexchange WHERE default_currency = 'Y';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lscurrency_code = objOdbcDataReader["currency_code"].ToString();
                }
                msSQL = "SELECT company_name FROM adm_mst_tcompany";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lscompany_name = objOdbcDataReader["company_name"].ToString();
                }
                msSQL = "select product_name,product_desc,mrp_price,product_image from pmr_mst_tproduct where product_gid='" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsproduct_name = objOdbcDataReader["product_name"].ToString();
                    lsproduct_desc = objOdbcDataReader["product_desc"].ToString();
                    lscost_price = objOdbcDataReader["mrp_price"].ToString();
                    lsproduct_image = objOdbcDataReader["product_image"].ToString();
                }

                decimal costPriceDecimal = decimal.Parse(lscost_price);  // Parse the string to decimal
                int costPriceCents = (int)(costPriceDecimal * 100);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var Client = new RestClient("https://graph.facebook.com");
                var request = new RestRequest("/v20.0/" + getwhatsappcredentials.wacatalouge_id + "/products", Method.POST);
                request.AddParameter("name", lsproduct_name);
                request.AddParameter("description", lsproduct_desc);
                request.AddParameter("availability", "in stock");
                request.AddParameter("price", costPriceCents);
                request.AddParameter("currency", lscurrency_code);
                request.AddParameter("image_url", lsproduct_image);
                request.AddParameter("url", lsproduct_image);
                request.AddParameter("brand", lscompany_name);
                request.AddParameter("access_token", getwhatsappcredentials.waaccess_token);
                request.AddParameter("retailer_id", product_gid);

                IRestResponse response = Client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    dynamic responseData = JsonConvert.DeserializeObject(response.Content);
                    string facebookProductId = responseData.id;
                    msSQL = "update pmr_mst_tproduct set whatsapp_id='" + facebookProductId + "',whatsappproduct_updatedby='" + user_gid + "' where product_gid='" + product_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product moved to WhatsApp Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occurred while moving the product.";
                    }
                }
                else
                {
                    dynamic errorData = JsonConvert.DeserializeObject(response.Content);
                    values.status = false;

                    if (errorData.error.message == "(#10800) Duplicate retailer_id when attempting to create a product.")
                    {
                        values.message = "Product Already added!!";
                    }
                    else if (errorData.error.message == "(#100) Param image_url is not a valid URI.")
                    {
                        values.message = "Invalid Image Url,Kindly Upload the Image";
                    }
                    else if (errorData.error.message == "(#10801) Either \"uploaded_image_id\" or \"image_url\" must be specified.")
                    {
                        values.message = "Product Image is Required!!";
                    }
                    else if (errorData.error.error_user_msg == "Products without \"description\" information can't be uploaded. Please check that this field is included for each product in a separate, labelled column.")
                    {
                        values.message = "Product Description is Required!!";
                    }
                    else
                    {
                        values.message = "Error While Adding Product";

                    }
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Add product to whatsapp!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public void Daupdatewhatsappstockstatus(string branch_gid,product_list values)
        {

            try
            {
                whatsappconfiguration1 getwhatsappcredentials = whatsappcredentials1(branch_gid);

                msSQL = "select whatsappstock_status from pmr_mst_tproduct where whatsapp_id='" + values.whatsapp_id + "'";
                string whatsappstock_status = objdbconn.GetExecuteScalar(msSQL);
                if (whatsappstock_status == "Y")
                {
                    stock_status = "out of stock";
                }
                else
                {
                    stock_status = "in stock";
                }
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var Client = new RestClient("https://graph.facebook.com");
                var request = new RestRequest("/v20.0/" + values.whatsapp_id, Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("availability", stock_status);
                request.AddParameter("access_token", getwhatsappcredentials.waaccess_token);

                IRestResponse response = Client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (stock_status == "out of stock")
                    {
                        msSQL = "update pmr_mst_tproduct set whatsappstock_status='N' where whatsapp_id='" + values.whatsapp_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = "update pmr_mst_tproduct set whatsappstock_status='Y' where whatsapp_id='" + values.whatsapp_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Stock Status Updated Sucessfully!!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occured while updating stock!!";
                    }
                }
            }
            catch (Exception ex)
            {

                values.message = "Error While Updating whats app product Stock Status";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetwhatsappProductSummary(MdlProduct values)
        {

            try
            {
                msSQL = "SELECT (SELECT COUNT(product_gid) FROM pmr_mst_tproduct WHERE status != '2') AS active_Products," +
                     "    (SELECT COUNT(whatsapp_id) FROM pmr_mst_tproduct WHERE whatsapp_id != 'null' and status != '2') AS product_added," +
                     "    (SELECT COUNT(whatsappstock_status) FROM pmr_mst_tproduct WHERE  whatsapp_id != 'null' and whatsappstock_status='Y' and status != '2') AS in_stock," +
                      "    (SELECT COUNT(whatsappstock_status) FROM pmr_mst_tproduct WHERE whatsapp_id != 'null' and whatsappstock_status='N' and status != '2') AS out_of_stock; ";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.active_Products = objOdbcDataReader["active_Products"].ToString();
                    values.product_added = objOdbcDataReader["product_added"].ToString();
                    values.in_stock = objOdbcDataReader["in_stock"].ToString();
                    values.out_of_stock = objOdbcDataReader["out_of_stock"].ToString();

                }

                msSQL = " SELECT d.producttype_name,a.whatsapp_id,a.whatsappstock_status,b.productgroup_name,a.product_desc,b.productgroup_code,a.product_gid, a.mrp_price, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,  CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
                   " c.productuomclass_code,e.productuom_code,c.productuomclass_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable,e.productuom_name,d.producttype_name as product_type,CASE  WHEN a.status = '1' THEN 'Active' WHEN a.status IS NULL OR a.status = '' OR a.status = '2' THEN 'Inactive' END AS Status," +
                   " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time,(case when a.shopify_productid is not null then 'No'  else 'Yes' end)as shopify_productid,a.product_image,(SELECT COUNT(product_gid) FROM pmr_mst_tproduct) AS product_count  from pmr_mst_tproduct a " +
                   " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                   " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                   " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                   " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                   " left join adm_mst_tuser f on f.user_gid=a.created_by where a.status !='2' order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list
                        {
                            shopify_productid = dt["shopify_productid"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            product_count = dt["product_count"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            stockable = dt["stockable"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_type = dt["product_type"].ToString(),
                            Status = dt["Status"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            lead_time = dt["lead_time"].ToString(),
                            whatsapp_id = dt["whatsapp_id"].ToString(),
                            whatsappstock_status = dt["whatsappstock_status"].ToString(),




                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaRemoveproductfromwt(string whatsapp_id,string branch_gid,  result values)
        {
            try
            {
                whatsappconfiguration1 getwhatsappcredentials = whatsappcredentials1(branch_gid);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var Client = new RestClient("https://graph.facebook.com");
                var request = new RestRequest("/v20.0/" + whatsapp_id, Method.DELETE);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("access_token", getwhatsappcredentials.waaccess_token);
                IRestResponse response = Client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    msSQL = "update pmr_mst_tproduct set whatsapp_id='null',whatsappstock_status='Y' where whatsapp_id='"+ whatsapp_id + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Deleted from WhatsApp Sucessfully!!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occured while deleting product!!";
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while remove product from whatsapp!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public whatsappconfiguration1 whatsappcredentials1(string branch_gid)
        {
            whatsappconfiguration1 getwhatsappcredentials = new whatsappconfiguration1();
            try
            {


                msSQL = " select wacatalouge_id,wachannel_id,waaccess_token,meta_phone_id,waphone_number from hrm_mst_tbranch where branch_gid='" + branch_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {

                    getwhatsappcredentials.wacatalouge_id = objOdbcDataReader["wacatalouge_id"].ToString();
                    getwhatsappcredentials.wachannel_id = objOdbcDataReader["wachannel_id"].ToString();
                    getwhatsappcredentials.waaccess_token = objOdbcDataReader["waaccess_token"].ToString();
                    getwhatsappcredentials.meta_phone_id = objOdbcDataReader["meta_phone_id"].ToString();
                    getwhatsappcredentials.waphone_number = objOdbcDataReader["waphone_number"].ToString();

                }
                else
                {

                }
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

            return getwhatsappcredentials;
        }

    }
}
