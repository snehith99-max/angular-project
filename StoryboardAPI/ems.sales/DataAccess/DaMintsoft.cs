using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Data.Odbc;
using Newtonsoft.Json;
using System.Data;
using RestSharp;
using System.Net;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Policy;
using System.Globalization;

namespace ems.sales.DataAccess
{
    public class DaMintsoft
    {
        dbconn objdbconn = new dbconn();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1, objOdbcDataReader2, objOdbcDataReader3 , objOdbcDataReader22, objOdbcDataReader33;
        DataTable dt_datatable;
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string mssalesorderGID, mssalesorderGID1, msGetGid1, msGetGid;
        private static SemaphoreSlim _semaphore2 = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue2 = new ConcurrentQueue<string>();
        private static Task<get> _runningTask2;
        int mnResult, mnResult1;
        string base_url, lsclient_id, api_key = string.Empty, msstockdtlGid;
        public result DaCreateOrder(string salesorder_gid, int CourierServiceId)
        {
            result objresult = new result();
            MdlMintsoftJSON objMdlMintsoftJSON = new MdlMintsoftJSON();
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

                msSQL = "select name from crm_smm_tmintsoftcourierservice where courierservice_id='"+ CourierServiceId + "'";
                string CourierService = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select a.customer_name,a.customer_address,a.customer_address2,b.zip_code,a.customer_city,a.customer_state,a.customer_pin,b.email,b.mobile,b.phone_no,b.customercontact_name," +
                        "c.country_name,d.so_referenceno1,d.start_date " +
                        "from crm_mst_tcustomer a " +
                        "left join crm_mst_tcustomercontact b on a.customer_gid = b.customer_gid " +
                        "left join adm_mst_tcountry c on c.country_gid = a.customer_country " +
                        "inner join smr_trn_tsalesorder d on a.customer_gid = d.customer_gid " +
                        "where d.salesorder_gid = '" + salesorder_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
              
                if (objOdbcDataReader.HasRows)
                {
                    if (!string.IsNullOrEmpty(objOdbcDataReader["customer_name"].ToString()))
                    {
                        objMdlMintsoftJSON.CompanyName = objOdbcDataReader["customer_name"].ToString();
                        if (string.IsNullOrEmpty(objOdbcDataReader["customercontact_name"].ToString()))
                        {
                            objMdlMintsoftJSON.FirstName = objOdbcDataReader["customer_name"].ToString();
                        }
                        else
                        {
                            objMdlMintsoftJSON.FirstName = objOdbcDataReader["customercontact_name"].ToString();

                        }
                    }
                    else
                    {
                        objMdlMintsoftJSON.FirstName = objOdbcDataReader["customercontact_name"].ToString();

                    }
                    //objMdlMintsoftJSON.Address1 = objOdbcDataReader["customer_address"].ToString();
                    //objMdlMintsoftJSON.Address2 = objOdbcDataReader["customer_address2"].ToString();
                    //objMdlMintsoftJSON.PostCode = objOdbcDataReader["zip_code"].ToString();
                    //objMdlMintsoftJSON.Town = objOdbcDataReader["customer_city"].ToString();
                    objMdlMintsoftJSON.Email = objOdbcDataReader["email"].ToString();
                    objMdlMintsoftJSON.Mobile = objOdbcDataReader["mobile"].ToString();
                    objMdlMintsoftJSON.Phone = objOdbcDataReader["phone_no"].ToString();
                    objMdlMintsoftJSON.OrderNumber = objOdbcDataReader["so_referenceno1"].ToString();
                    objMdlMintsoftJSON.ExternalOrderReference = objOdbcDataReader["so_referenceno1"].ToString();
                    objMdlMintsoftJSON.WarehouseId = 3;
                    objMdlMintsoftJSON.Warehouse = "Main Warehouse";
                    objMdlMintsoftJSON.CourierServiceId = CourierServiceId;
                    objMdlMintsoftJSON.CourierService = CourierService;
                    objMdlMintsoftJSON.ClientId = int.Parse(lsclient_id);
                }
                msSQL = "select customerbranch_gid from smr_trn_tsalesorder where salesorder_gid = '" + salesorder_gid + "' ";
                objOdbcDataReader22 = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader22.HasRows) 
                {
                    msSQL = "select address1 , address2 , city , zip_code from crm_mst_tcustomercontact where customercontact_gid = '" + objOdbcDataReader22["customerbranch_gid"].ToString() + "'";
                    objOdbcDataReader33 = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader33.HasRows)
                    {
                        objMdlMintsoftJSON.Address1 = objOdbcDataReader33["address1"].ToString();
                        objMdlMintsoftJSON.Address2 = objOdbcDataReader33["address2"].ToString();
                        objMdlMintsoftJSON.PostCode = objOdbcDataReader33["zip_code"].ToString();
                        objMdlMintsoftJSON.Town = objOdbcDataReader33["city"].ToString();
                    }
                    objOdbcDataReader33.Close();

                }
                objOdbcDataReader22.Close();
                double orderTotal = 0;

                msSQL = "select b.customerproduct_code,a.discount_amount,b.cost_price,a.qty_quoted " +
                        "from smr_trn_tsalesorderdtl a " +
                        "left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        "where salesorder_gid = '" + salesorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    int i = 0;
                    objMdlMintsoftJSON.OrderItems = new Orderitem[dt_datatable.Rows.Count];

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        objMdlMintsoftJSON.OrderItems[i] = new Orderitem();
                        objMdlMintsoftJSON.OrderItems[i].SKU = dt["customerproduct_code"].ToString();
                        objMdlMintsoftJSON.OrderItems[i].Discount = double.Parse(dt["discount_amount"].ToString());
                        objMdlMintsoftJSON.OrderItems[i].UnitPrice = double.Parse(dt["cost_price"].ToString());
                        objMdlMintsoftJSON.OrderItems[i].Quantity = int.Parse(dt["qty_quoted"].ToString());
                        orderTotal += objMdlMintsoftJSON.OrderItems[i].UnitPrice * objMdlMintsoftJSON.OrderItems[i].Quantity;
                        i++;
                    }
                    dt_datatable.Dispose();
                }
                objMdlMintsoftJSON.OrderValue = orderTotal;
                string json = JsonConvert.SerializeObject(objMdlMintsoftJSON);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(base_url);
                var request = new RestRequest("/api/Order", Method.PUT);
                request.AddHeader("ms-apikey", api_key);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    List<Class1> objMdlMintSoftResponse = JsonConvert.DeserializeObject<List<Class1>>(response.Content);

                    if (objMdlMintSoftResponse[0].Success)
                    {
                        msSQL = "update smr_trn_tsalesorder set mintsoftid = '" + objMdlMintSoftResponse[0].OrderId + "' where salesorder_gid ='" + salesorder_gid + "'";
                        int mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        objresult.message = objMdlMintSoftResponse[0].Message;
                        objresult.status = true;
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = objMdlMintSoftResponse[0].Message;
                    }
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Exception occured while posting to Mintsoft!";
            }
            return objresult;
        }
        public async Task<get> DaMintsoftCourierDetails(string user_gid)
        {
            _queue2.Enqueue(user_gid); // Add the request to the queue
            await ProcessQueueCourier(); // Process the queue
            return await _runningTask2;
        }
        private async Task ProcessQueueCourier()
        {
            while (_queue2.TryDequeue(out string user_gid))
            {
                await _semaphore2.WaitAsync(); // Ensure that only one method runs at a time
                try
                {
                    if (_runningTask2 != null && !_runningTask2.IsCompleted)
                    {
                        await _runningTask2; // Wait for the previous task to complete
                    }

                    _runningTask2 = GetMintsoftCourierDetails(user_gid); // Start a new task
                    await _runningTask2;
                }
                finally
                {
                    _semaphore2.Release(); // Release the semaphore
                }
            }
        }
        public async Task<get> GetMintsoftCourierDetails(string user_gid)
        {


            get objresult = new get();
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

                msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                // code by snehith to check  mintsoft_flag for  mintsoft
                if (mintsoft_flag == "Y")
                {
                    // Set API endpoint and API key
                    string apiUrl = base_url + "/api/Courier/Services";
                    string apiKey = api_key;

                    // Get JSON data from API
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                        HttpResponseMessage response = await client.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            //  var warehousedetails = JsonConvert.DeserializeObject<List<CourierSerivce>>(responseBody);

                            List<CourierSerivce> CourierSerivcedetails = JsonConvert.DeserializeObject<List<CourierSerivce>>(responseBody);

                            foreach (var item in CourierSerivcedetails)
                            {
                                string mysql2 = "SELECT courierservice_id FROM crm_smm_tmintsoftcourierservice WHERE courierservice_id = '" + item.ID + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(mysql2);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    msSQL = "INSERT INTO crm_smm_tmintsoftcourierservice (" +
                                          "courierservice_id, " +
                                           "courierservicetype_id, " +
                                           "name, " +
                                           "trackingurl, " +
                                           "activeb, " +
                                           "created_date, " +
                                           "created_by" +
                                           ") VALUES (" +
                                            "'" + item.ID + "'," +
                                           "'" + item.CourierServiceTypeId + "'," +
                                           "'" + (item.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'," +
                                           "'" + item.TrackingURL + "'," +
                                           "'" + (item.ActiveB ? "true" : "false") + "'," +
                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                           "'" + user_gid + "'" +
                                           ")";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }

                                }
                                else
                                {
                                    msSQL = "UPDATE crm_smm_tmintsoftcourierservice SET " +
                                             "courierservicetype_id = '" + item.CourierServiceTypeId + "', " +
                                             "name = '" + (item.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', " +
                                             "trackingurl = '" + item.TrackingURL + "', " +
                                             "activeb = '" + (item.ActiveB ? "true" : "false") + "', " +
                                             "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                             "created_by = '" + user_gid + "' " +
                                             "WHERE courierservice_id = '" + item.ID + "'";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }
                                }
                                objOdbcDataReader.Close();
                            }

                        }
                        else
                        {
                            objresult.status = false;
                            string errorMessage = $"Failed to fetch warehouse details. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    string BaseUrl1 = base_url+"/api/Courier/ServiceTypes";
                    string url1 = $"{BaseUrl1}";

                    using (var client1 = new HttpClient())
                    {
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client1.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                        HttpResponseMessage response1 = await client1.GetAsync(url1);
                        if (response1.IsSuccessStatusCode)
                        {
                            string responseBody1 = await response1.Content.ReadAsStringAsync();
                            //var locationdetails = JsonConvert.DeserializeObject<List<CourierSerivceType>>(responseBody1);
                            List<CourierSerivceType> CourierSerivceTypedetails = JsonConvert.DeserializeObject<List<CourierSerivceType>>(responseBody1);
                            foreach (var item1 in CourierSerivceTypedetails)
                            {
                                string mysql2 = "SELECT courierservicetype_id FROM crm_smm_tmintsoftcourierservicetype WHERE courierservicetype_id = '" + item1.ID + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(mysql2);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    msSQL = "INSERT INTO crm_smm_tmintsoftcourierservicetype (" +
                                                               "courierservicetype_id, " +
                                                               "name, " +
                                                               "active, " +
                                                               "created_by, " +
                                                               "created_date" +
                                                               ") VALUES (" +
                                                               "'" + item1.ID + "'," +
                                                               "'" + (item1.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'," +
                                                               "'" + item1.Active + "'," +
                                                               "'" + user_gid + "'," +
                                                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                               ")";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }

                                }
                                else
                                {
                                    msSQL = "UPDATE crm_smm_tmintsoftcourierservicetype SET " +
                                             "name = '" + (item1.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', " +
                                             "active = '" + item1.Active + "', " +
                                             "created_by = '" + user_gid + "', " +
                                             "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                             "WHERE courierservicetype_id = '" + item1.ID + "'";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }
                                }
                                objOdbcDataReader.Close();
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            string errorMessage = $"Failed to fetch warehouse details. Status code: {response1.StatusCode}, Reason: {response1.ReasonPhrase}";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }



                    //code by snehith for order channel
                    string BaseUrl2 = base_url+"/api/Order/Channels";
                    string url2 = $"{BaseUrl2}";

                    using (var client2 = new HttpClient())
                    {
                        client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client2.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                        HttpResponseMessage response2 = await client2.GetAsync(url2);
                        if (response2.IsSuccessStatusCode)
                        {
                            string responseBody2 = await response2.Content.ReadAsStringAsync();
                            //var locationdetails = JsonConvert.DeserializeObject<List<CourierSerivceType>>(responseBody1);
                            List<Orderchannel> Orderchanneldetails = JsonConvert.DeserializeObject<List<Orderchannel>>(responseBody2);
                            foreach (var item2 in Orderchanneldetails)
                            {
                                string mysql2 = "SELECT channel_id FROM crm_smm_tmintsoftorderchannel WHERE channel_id = '" + item2.ID + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(mysql2);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    msSQL = "INSERT INTO crm_smm_tmintsoftorderchannel (" +
                                                               "channel_id, " +
                                                               "name, " +
                                                               "description, " +
                                                               "active, " +
                                                               "logo, " +
                                                               "client_id, " +
                                                               "created_by, " +
                                                               "created_date" +
                                                               ") VALUES (" +
                                                               "'" + item2.ID + "'," +
                                                               "'" + (item2.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'," +
                                                               "'" + (item2.Description ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'," +
                                                               "'" + item2.Active + "'," +
                                                               "'" + item2.Logo + "'," +
                                                               "'" + item2.ClientId + "'," +
                                                               "'" + user_gid + "'," +
                                                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                               ")";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }

                                }
                                else
                                {
                                    msSQL = "UPDATE crm_smm_tmintsoftorderchannel SET " +
                                              "name = '" + (item2.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', " +
                                              "description = '" + (item2.Description ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', " +
                                            "active = '" + item2.Active + "', " +
                                            "logo = '" + item2.Logo + "', " +
                                            "client_id = '" + item2.ClientId + "', " +
                                            "created_by = '" + user_gid + "', " +
                                            "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                            "WHERE channel_id = '" + item2.ID + "'";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }
                                }
                                objOdbcDataReader.Close();
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            string errorMessage = $"Failed to fetch warehouse details. Status code: {response2.StatusCode}, Reason: {response2.ReasonPhrase}";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }

                    //code by snehith for order statuses
                    string BaseUrl3 = base_url+"/api/Order/Statuses";
                    string url3 = $"{BaseUrl3}";

                    using (var client3 = new HttpClient())
                    {
                        client3.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client3.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                        HttpResponseMessage response3 = await client3.GetAsync(url3);
                        if (response3.IsSuccessStatusCode)
                        {
                            string responseBody3 = await response3.Content.ReadAsStringAsync();
                            //var locationdetails = JsonConvert.DeserializeObject<List<CourierSerivceType>>(responseBody1);
                            List<OrderStatuses> Orderstatusdetails = JsonConvert.DeserializeObject<List<OrderStatuses>>(responseBody3);
                            foreach (var item3 in Orderstatusdetails)
                            {
                                string mysql2 = "SELECT orderstatus_id FROM crm_smm_tmintsoftorderstatuses WHERE orderstatus_id = '" + item3.ID + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(mysql2);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    msSQL = "INSERT INTO crm_smm_tmintsoftorderstatuses (" +
                                                               "orderstatus_id, " +
                                                               "name, " +
                                                               "externalname, " +
                                                               "created_by, " +
                                                               "created_date" +
                                                               ") VALUES (" +
                                                               "'" + item3.ID + "'," +
                                                               "'" + (item3.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'," +
                                                               "'" + (item3.ExternalName ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'," +
                                                               "'" + user_gid + "'," +
                                                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                               ")";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }

                                }
                                else
                                {
                                    msSQL = "UPDATE crm_smm_tmintsoftorderstatuses SET " +
                                            "name = '" + (item3.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', " +
                                            "externalname = '" + (item3.ExternalName ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', " +
                                            "created_by = '" + user_gid + "', " +
                                            "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                            "WHERE orderstatus_id = '" + item3.ID + "'";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }
                                }
                                objOdbcDataReader.Close();
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            string errorMessage = $"Failed to fetch warehouse details. Status code: {response3.StatusCode}, Reason: {response3.ReasonPhrase}";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                }
                else
                {
                    objresult.status = true;
                }
            }
            catch (Exception ex)
            {
                objresult.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"Error: {ex.Message}", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


            return objresult;
        }

        private static SemaphoreSlim _semaphore4 = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue4 = new ConcurrentQueue<string>();
        private static Task<get> _runningTasks;
        public async Task<get> DaMintsoftAsnstatusgoodssupplier(string user_gid)
        {
            _queue4.Enqueue(user_gid); // Add the request to the queue
            await ProcessQueueASNstatus(); // Process the queue
            return await _runningTasks;
        }
        private async Task ProcessQueueASNstatus()
        {
            while (_queue4.TryDequeue(out string user_gid))
            {
                await _semaphore4.WaitAsync(); // Ensure that only one method runs at a time
                try
                {
                    if (_runningTasks != null && !_runningTasks.IsCompleted)
                    {
                        await _runningTasks; // Wait for the previous task to complete
                    }

                    _runningTasks = GetMintsoftAsnstatusgoodssupplier(user_gid); // Start a new task
                    await _runningTasks;
                }
                finally
                {
                    _semaphore4.Release(); // Release the semaphore
                }
            }
        }

        public async Task<get> GetMintsoftAsnstatusgoodssupplier(string user_gid)
        {


            get objresult = new get();
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
                msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                // code by snehith to check  mintsoft_flag for  mintsoft
                if (mintsoft_flag == "Y")
                {
                    // Set API endpoint and API key
                    string apiUrl = base_url+"/api/ASN/Statuses";
                    string apiKey = api_key;

                    //// Get JSON data from API
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                        HttpResponseMessage response = await client.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            //  var warehousedetails = JsonConvert.DeserializeObject<List<ASNStatuses>>(responseBody);

                            List<ASNStatuses> ASNStatusesdetails = JsonConvert.DeserializeObject<List<ASNStatuses>>(responseBody);

                            foreach (var item in ASNStatusesdetails)
                            {
                                string mysql2 = "SELECT asnstatus_id FROM crm_smm_tmintsoftasnstatuses  WHERE asnstatus_id = '" + item.ID + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(mysql2);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    msSQL = "INSERT INTO crm_smm_tmintsoftasnstatuses  (" +
                                          "asnstatus_id, " +
                                           "asnstatus_name, " +
                                           "colour, " +
                                           "text_colour, " +
                                           "created_date, " +
                                           "created_by" +
                                           ") VALUES (" +
                                            "'" + item.ID + "'," +
                                           "'" + item.Name + "'," +
                                           "'" + (item.Colour ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'," +
                                           "'" + item.TextColour + "'," +
                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                           "'" + user_gid + "'" +
                                           ")";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }

                                }
                                else
                                {
                                    msSQL = "UPDATE crm_smm_tmintsoftasnstatuses SET " +
                                             "asnstatus_name = '" + item.Name + "', " +
                                             "colour = '" + (item.Colour ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "', " +
                                             "text_colour = '" + item.TextColour + "', " +
                                             "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                             "created_by = '" + user_gid + "' " +
                                             "WHERE asnstatus_id = '" + item.ID + "'";

                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                     "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                    }

                                }
                                objOdbcDataReader.Close();


                            }
                        }
                        else
                        {
                            objresult.status = false;
                            string errorMessage = $"Failed to fetch warehouse details. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    string BaseUrl1 = base_url+"/api/ASN/GoodsInTypes";
                    string url1 = $"{BaseUrl1}";

                    using (var client1 = new HttpClient())
                    {
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client1.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                        HttpResponseMessage response1 = await client1.GetAsync(url1);
                        if (response1.IsSuccessStatusCode)
                        {
                            string responseBody1 = await response1.Content.ReadAsStringAsync();
                            //var locationdetails = JsonConvert.DeserializeObject<List<CourierSerivceType>>(responseBody1);
                            List<ASNGoodsintype> ASNGoodsintypedetails = JsonConvert.DeserializeObject<List<ASNGoodsintype>>(responseBody1);
                            foreach (var item1 in ASNGoodsintypedetails)
                            {
                                msSQL = "SELECT goodsintypes_id FROM crm_smm_tmintsoftasngoodsintypes WHERE goodsintypes_id = '" + item1.ID + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    msSQL = "INSERT INTO crm_smm_tmintsoftasngoodsintypes (" +
                                                               "goodsintypes_id, " +
                                                               "goodsintypes_name" +
                                                               ") VALUES (" +
                                                               "'" + item1.ID + "'," +
                                                               "'" + (item1.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("，", ",").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "'" +
                                                               ")";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {

                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                    }

                                }
                                else
                                {
                                    msSQL = "UPDATE crm_smm_tmintsoftasngoodsintypes SET " +
                                 "goodsintypes_name = '" + (item1.Name ?? " ").Replace("'", "\\'").Replace("，", ",").Replace("'", ",").Replace("\'", ",") + "' " +
                                 "WHERE goodsintypes_id = '" + item1.ID + "'";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                        $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "**********" +
                                        "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                    }

                                }
                                objOdbcDataReader.Close();
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            string errorMessage = $"Failed to fetch warehouse details. Status code: {response1.StatusCode}, Reason: {response1.ReasonPhrase}";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    string BaseUrl4 = base_url + "/api/RefData/Countries";
                    string url4 = $"{BaseUrl4}";

                    using (var client4 = new HttpClient())
                    {
                        client4.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client4.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                        HttpResponseMessage response4 = await client4.GetAsync(url4);
                        if (response4.IsSuccessStatusCode)
                        {
                            string responseBody4 = await response4.Content.ReadAsStringAsync();
                            List<MintsoftCountries> MintsoftCountriesList = JsonConvert.DeserializeObject<List<MintsoftCountries>>(responseBody4);

                            foreach (var item4 in MintsoftCountriesList)
                            {
                                string newCountryGid = string.Empty;
                                string newCountryCode = string.Empty;
                                string formattedDate = FormatDate(item4.LastUpdated);
                                string mysqlCheck = "SELECT country_gid FROM adm_mst_tcountry WHERE mintsoft_countryid = '" + item4.ID + "' or  country_name = '" + item4.Name.Replace("'", "\\'") + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(mysqlCheck);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    string getLastRecordSQL = "SELECT country_gid, country_code ,display_order " +
                                                      "FROM adm_mst_tcountry " +
                                                      "ORDER BY country_gid DESC " +
                                                      "LIMIT 1";
                                    objOdbcDataReader = objdbconn.GetDataReader(getLastRecordSQL);
                                    string lastCountryGid = string.Empty;
                                    string lastCountryCode = string.Empty;
                                    int lastDisplayOrder = 0;
                                    int newDisplayOrder = 0;
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        lastCountryGid = objOdbcDataReader["country_gid"].ToString();
                                        lastCountryCode = objOdbcDataReader["country_code"].ToString();
                                        lastDisplayOrder = Convert.ToInt32(objOdbcDataReader["display_order"]);

                                    }
                                    objOdbcDataReader.Close();
                                    if (!string.IsNullOrEmpty(lastCountryGid) && lastCountryGid.Length > 2)
                                    {
                                        // Extract the numeric part of country_gid
                                        string numericPartGid = lastCountryGid.Substring(2);
                                        int newGidNumber = int.Parse(numericPartGid) + 1;
                                        newCountryGid = "CN" + newGidNumber.ToString("D8"); // 8-digit numeric part for country_gid

                                        // Extract the numeric part of country_code
                                        string numericPartCode = lastCountryCode.Substring(2);
                                        int newCodeNumber = int.Parse(numericPartCode) + 1;
                                        newCountryCode = "CN" + newCodeNumber.ToString("D2"); // 2-digit numeric part for country_code

                                        // Increment display_order
                                        newDisplayOrder = lastDisplayOrder + 1; // Increment by 1, assuming display_order is an integer

                                    }

                                    string msSQL = "INSERT INTO adm_mst_tcountry (" +
                                                 "country_gid, " +
                                                 "country_code, " +
                                                 "country_name, " +
                                                 "display_order, " +
                                                 "mintsoft_countryid, " +
                                                 "mintsoft_code, " +
                                                 "mintsoft_code3, " +
                                                 "created_by, " +
                                                 "created_date" +
                                                 ") VALUES (" +
                                                 "'" + newCountryGid + "'," +
                                                 "'" + (newCountryCode ?? "").Replace("'", "\\'") + "'," +
                                                 "'" + (item4.Name ?? "").Replace("'", "\\'") + "'," +
                                                 "'" + newDisplayOrder + "'," +
                                                 "'" + item4.ID + "'," +
                                                 "'" + (item4.Code ?? "").Replace("'", "\\'") + "'," +
                                                 "'" + (item4.Code3 ?? "").Replace("'", "\\'") + "'," +
                                                 "'" + user_gid + "'," +
                                                 "'" + formattedDate + "'" +
                                                 ")";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult == 0)
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                                                    $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " ***********" +
                                                                    "*****Query****" + msSQL + "*******Apiref********",
                                                                    "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                    }
                                }
                                else
                                {
                                    string msSQL = "UPDATE adm_mst_tcountry SET " +
                                           "country_name = '" + (item4.Name ?? "").Replace("'", "\\'") + "', " +
                                           "mintsoft_countryid = '" + item4.ID + "', " +
                                           "mintsoft_code = '" + (item4.Code ?? "").Replace("'", "\\'") + "', " +
                                           "mintsoft_code3 = '" + (item4.Code3 ?? "").Replace("'", "\\'") + "', " +
                                           "updated_by = '" + user_gid + "', " +
                                           "updated_date = '" + formattedDate + "' " +
                                           "WHERE mintsoft_countryid = '" + item4.ID + "' " +
                                           "OR country_name = '" + (item4.Name ?? "").Replace("'", "\\'") + "'";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                                                    $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " ***********" +
                                                                    "*****Query****" + msSQL + "*******Apiref********",
                                                                    "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                    }
                                }
                                objOdbcDataReader.Close();
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            string errorMessage = $"Failed to fetch client details. Status code: {response4.StatusCode}, Reason: {response4.ReasonPhrase}";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                                        $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + errorMessage,
                                                        "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }

                    string BaseUrl5 = base_url + "/api/RefData/Currencies";
                    string url5 = $"{BaseUrl5}";

                    using (var client5 = new HttpClient())
                    {
                        client5.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client5.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                        HttpResponseMessage response5 = await client5.GetAsync(url5);
                        if (response5.IsSuccessStatusCode)
                        {
                            string responseBody5 = await response5.Content.ReadAsStringAsync();
                            List<MintsoftCurrencies> MintsoftCurrencyList = JsonConvert.DeserializeObject<List<MintsoftCurrencies>>(responseBody5);

                            foreach (var item5 in MintsoftCurrencyList)
                            {
                                string formattedDate = FormatDate(item5.LastUpdated);
                                string mysqlCheck = "SELECT currencyexchange_gid FROM crm_trn_tcurrencyexchange WHERE mintsoft_currencyid = '" + item5.ID + "' or  currency_code = '" + item5.Code + "' ";
                                objOdbcDataReader = objdbconn.GetDataReader(mysqlCheck);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    msGetGid = objcmnfunctions.GetMasterGID("CUR");
                                    string msSQL = "INSERT INTO crm_trn_tcurrencyexchange (" +
                                        "currencyexchange_gid,currency_code, symbol,default_currency,mintsoft_currencyid,mintsoft_currencyname,created_by, created_date" +
                                        ") VALUES (" +
                                        $"'{msGetGid}', " +
                                        $"'{item5.Code}', " +
                                        $"'{item5.Symbol}', " +
                                        $"'N', " +
                                        $"'{item5.ID}', " +
                                        $"'{(item5.Name ?? "").Replace("'", "\\'")}', " +
                                        $"'{user_gid}', " +
                                        $"'{formattedDate}'" +
                                        ")";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                                                    $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " ***********" +
                                                                    "*****Query****" + msSQL + "*******Apiref********",
                                                                    "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                    }
                                }
                                else
                                {
                                    string msSQL = "UPDATE crm_trn_tcurrencyexchange SET " +
                                          "currency_code = '" + item5.Code + "', " +
                                          "symbol = '" + (item5.Symbol?.Replace("'", "\\'")) + "', " +
                                          "mintsoft_currencyid = '" + item5.ID + "', " +
                                          "mintsoft_currencyname = '" + (item5.Name?.Replace("'", "\\'")) + "', " +
                                          "created_by = '" + user_gid + "', " +
                                          "created_date = '" + formattedDate + "' " +
                                          "WHERE mintsoft_currencyid = '" + item5.ID + "' OR currency_code = '" + item5.Code + "'";
                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                                                    $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " ***********" +
                                                                    "*****Query****" + msSQL + "*******Apiref********",
                                                                    "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                    }
                                }
                                objOdbcDataReader.Close();
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            string errorMessage = $"Failed to fetch client details. Status code: {response5.StatusCode}, Reason: {response5.ReasonPhrase}";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                                        $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + errorMessage,
                                                        "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }

                    //code by snehith for order channel
                    string BaseUrl2 = base_url+"/api/Product/Suppliers";
                    string url2 = $"{BaseUrl2}?ClientId={lsclient_id}";

                    using (var client2 = new HttpClient())
                    {
                        client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client2.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                        HttpResponseMessage response2 = await client2.GetAsync(url2);
                        if (response2.IsSuccessStatusCode)
                        {
                            string responseBody2 = await response2.Content.ReadAsStringAsync();
                            //var locationdetails = JsonConvert.DeserializeObject<List<CourierSerivceType>>(responseBody1);
                            List<ASNSupplier> ASNSupplierdetails = JsonConvert.DeserializeObject<List<ASNSupplier>>(responseBody2);
                            foreach (var item2 in ASNSupplierdetails)
                            {

                                msSQL = " select country_gid from adm_mst_tcountry where mintsoft_countryid =  '" + item2.CountryId + "' ";
                                string country_gid = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = " select currencyexchange_gid from crm_trn_tcurrencyexchange where mintsoft_currencyid =  '" + item2.CurrencyId + "' ";
                                string currencyexchange_gid = objdbconn.GetExecuteScalar(msSQL);
                                string mysql2 = "SELECT supplier_id FROM acp_mst_tvendor WHERE supplier_id = '" + item2.ID + "' or  vendor_companyname = '" + (item2.Name ?? " ").Replace("'", "\\'") + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(mysql2);
                                if (objOdbcDataReader.HasRows != true)
                                {
                                    string formattedDate = FormatDate(item2.LastUpdated);
                                    msGetGid1 = objcmnfunctions.GetMasterGID("PVRR");
                                    msGetGid = objcmnfunctions.GetMasterGID("SADM");

                                    msSQL = "INSERT INTO acp_mst_tvendor (" +
                                            "vendor_gid, " +
                                             "vendorregister_gid, " +
                                            "supplier_id, " +
                                            "vendor_code, " +
                                            "vendor_companyname, " +
                                            "contactperson_name, " +
                                            "contact_telephonenumber, " +
                                            "tin_number, " +
                                            "email_id, " +
                                            "active_flag, " +
                                            "address_gid," +
                                            "mintsoft_countryid, " +
                                            "mintsoft_currencyid," +
                                            "currencyexchange_gid," +
                                            "created_by, " +
                                            "created_date" +
                                            ") VALUES (" +
                                            "'" + msGetGid1 + "'," +
                                             "'" + msGetGid1 + "'," +
                                             "'" + item2.ID + "'," +
                                            "'" + (item2.Code ?? " ").Replace("'", "\\'") + "'," +
                                            "'" + (item2.Name ?? " ").Replace("'", "\\'") + "'," +
                                            "'" + (item2.ContactName ?? " ").Replace("'", "\\'") + "'," +
                                            "'" + (item2.ContactNumber ?? " ").Replace("'", "\\'") + "'," +
                                            "''," +
                                            "'" + (item2.ContactEmail ?? " ").Replace("'", "\\'") + "'," +
                                            "'" + (item2.Active.ToLower() == "true" ? "Y" : "N") + "'," +
                                            "'" + msGetGid + "'," +
                                            "'" + item2.CountryId + "'," +
                                             "'" + item2.CurrencyId + "'," +
                                             "'" + currencyexchange_gid + "'," +
                                            "'" + user_gid + "'," +
                                            "'" + formattedDate + "'" +
                                            ")";

                                    int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        msSQL = "INSERT INTO adm_mst_taddress (" +
                                                        "address_gid, " +
                                                        "country_gid, " +
                                                        "address1, " +
                                                        "address2, " +
                                                        "city, " +
                                                        "state, " +
                                                        "postal_code, " +
                                                        "vendor_gid, " +
                                                        "created_by, " +
                                                        "created_date" +
                                                        ") VALUES (" +
                                                        "'" + msGetGid + "'," +
                                                        "'" + country_gid + "'," +
                                                        "'" + (item2.AddressLine1 ?? " ").Replace("'", "\\'") + "'," +
                                                        "'" + (item2.AddressLine2 ?? " ").Replace("'", "\\'") + "'," +
                                                        "'" + (item2.Town ?? " ").Replace("'", "\\'") + "'," +
                                                        "'" + (item2.County ?? " ").Replace("'", "\\'") + "'," +
                                                        "'" + (item2.Postcode ?? " ").Replace("'", "\\'") + "'," +
                                                        "'" + msGetGid1 + "'," +
                                                        "'" + user_gid + "'," +
                                                        "'" + formattedDate + "'" +
                                                        ")";

                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 0)
                                        {

                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                           "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                        }
                                    }
                                    else
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                    "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");


                                    }

                                }
                                else
                                {
                                    string formattedDate = FormatDate(item2.LastUpdated);
                                    msSQL = "UPDATE acp_mst_tvendor SET " +
                                             "vendor_code = '" + (item2.Code ?? " ").Replace("'", "\\'") + "'," +
                                             "contactperson_name = '" + (item2.ContactName ?? " ").Replace("'", "\\'") + "'," +
                                             "contact_telephonenumber = '" + (item2.ContactNumber ?? " ").Replace("'", "\\'") + "'," +
                                             "email_id = '" + (item2.ContactEmail ?? " ").Replace("'", "\\'") + "'," +
                                             "active_flag = '" + (item2.Active.ToLower() == "true" ? "Y" : "N") + "'," +
                                             "address_gid = '" + msGetGid + "'," +
                                             "mintsoft_countryid = '" + item2.CountryId + "'," +
                                             "mintsoft_currencyid = '" + item2.CurrencyId + "'," +
                                             "currencyexchange_gid = '" + currencyexchange_gid + "'," +
                                             "created_by = '" + user_gid + "'," +
                                             "created_date = '" + formattedDate + "'" +
                                             " WHERE supplier_id = '" + item2.ID + "' AND vendor_companyname = '" + (item2.Name ?? " ").Replace("'", "\\'") + "'";
                                    int mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult1 == 1)
                                    {
                                        msSQL = " select vendor_gid from acp_mst_tvendor where supplier_id = '" + item2.ID + "' AND vendor_companyname = '" + (item2.Name ?? " ").Replace("'", "\\'") + "'";
                                        string vendor_gid = objdbconn.GetExecuteScalar(msSQL);

                                        msSQL = "UPDATE adm_mst_taddress SET " +
                                    "country_gid = '" + country_gid + "'," +
                                    "address1 = '" + (item2.AddressLine1 ?? " ").Replace("'", "\\'") + "'," +
                                    "address2 = '" + (item2.AddressLine2 ?? " ").Replace("'", "\\'") + "'," +
                                    "city = '" + (item2.Town ?? " ").Replace("'", "\\'") + "'," +
                                    "state = '" + (item2.County ?? " ").Replace("'", "\\'") + "'," +
                                    "postal_code = '" + (item2.Postcode ?? " ").Replace("'", "\\'") + "'," +
                                    "created_by = '" + user_gid + "'," +
                                    "created_date = '" + formattedDate + "'" +
                                    " WHERE vendor_gid = '" + vendor_gid + "'";
                                        int mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult2 == 0)
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " **********" +
                                                "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        }
                                    }
                                    else
                                    {
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                     "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                    }

                                }
                                objOdbcDataReader.Close();
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            string errorMessage = $"Failed to fetch warehouse details. Status code: {response2.StatusCode}, Reason: {response2.ReasonPhrase}";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }


                    //code by snehith for  Client
                    if (ConfigurationManager.AppSettings["MintSoftLive_flag"].ToString() == "Y")
                    {
                        string BaseUrl3 = base_url + "/api/Client";
                        string url3 = $"{BaseUrl3}";

                        using (var client3 = new HttpClient())
                        {
                            client3.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client3.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                            HttpResponseMessage response3 = await client3.GetAsync(url3);
                            if (response3.IsSuccessStatusCode)
                            {
                                string responseBody3 = await response3.Content.ReadAsStringAsync();
                                List<ClientDetails> clientDetailsList = JsonConvert.DeserializeObject<List<ClientDetails>>(responseBody3);
                                foreach (var item3 in clientDetailsList)
                                {
                                    string formattedDate = FormatDate(item3.LastUpdated);
                                    string mysql2 = "SELECT client_id FROM crm_smm_tmintsoftclientdetails WHERE client_id = '" + item3.ID + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(mysql2);
                                    if (objOdbcDataReader.HasRows != true)
                                    {
                                        // Insert new record
                                        msSQL = "INSERT INTO crm_smm_tmintsoftclientdetails (" +
                                                "client_id, shortname, client_name, client_code, brandname, contactname, contactnumber, addressline1, " +
                                                "addressline2, addressline3, town, county, postcode, ppinumber, vatnumber, eorinumber, " +
                                                "vatexempt, nireorinumber, iossnumber, countryid, contactemail, packaginginstructions, currencyid, " +
                                                "active, onstop, accountingintegrationtype, customerregistrationnumber, created_by, created_date" +
                                                ") VALUES (" +
                                                "'" + item3.ID + "', " +
                                                "'" + (item3.ShortName ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.Name ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.Code ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.BrandName ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.ContactName ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.ContactNumber ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.AddressLine1 ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.AddressLine2 ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.AddressLine3 ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.Town ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.County ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.Postcode ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.PPINumber ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.VATNumber ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.EORINumber ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + item3.VatExempt + "', " +
                                                "'" + (item3.NIREORINumber ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.IOSSNumber ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.CountryId.ToString() ?? "0") + "', " +
                                                "'" + (item3.ContactEmail ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.PackagingInstructions ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.CurrencyId.ToString() ?? "0") + "', " +
                                                "'" + item3.Active + "', " +
                                                "'" + item3.OnStop + "', " +
                                                "'" + (item3.AccountingIntegrationType ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + (item3.CustomerRegistrationNumber ?? " ").Replace("'", "\\'") + "', " +
                                                "'" + user_gid + "', " +
                                                "'" + formattedDate + "'" +
                                                ")";
                                        int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 0)
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                                                        $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " ***********" +
                                                                        "*****Query****" + msSQL + "*******Apiref********",
                                                                        "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        }
                                    }
                                    else
                                    {
                                        // Update existing record
                                        msSQL = "UPDATE crm_smm_tmintsoftclientdetails SET " +
                                                "shortname = '" + (item3.ShortName ?? " ").Replace("'", "\\'") + "', " +
                                                "client_name = '" + (item3.Name ?? " ").Replace("'", "\\'") + "', " +
                                                "client_code = '" + (item3.Code ?? " ").Replace("'", "\\'") + "', " +
                                                "brandname = '" + (item3.BrandName ?? " ").Replace("'", "\\'") + "', " +
                                                "contactname = '" + (item3.ContactName ?? " ").Replace("'", "\\'") + "', " +
                                                "contactnumber = '" + (item3.ContactNumber ?? " ").Replace("'", "\\'") + "', " +
                                                "addressline1 = '" + (item3.AddressLine1 ?? " ").Replace("'", "\\'") + "', " +
                                                "addressline2 = '" + (item3.AddressLine2 ?? " ").Replace("'", "\\'") + "', " +
                                                "addressline3 = '" + (item3.AddressLine3 ?? " ").Replace("'", "\\'") + "', " +
                                                "town = '" + (item3.Town ?? " ").Replace("'", "\\'") + "', " +
                                                "county = '" + (item3.County ?? " ").Replace("'", "\\'") + "', " +
                                                "postcode = '" + (item3.Postcode ?? " ").Replace("'", "\\'") + "', " +
                                                "ppinumber = '" + (item3.PPINumber ?? " ").Replace("'", "\\'") + "', " +
                                                "vatnumber = '" + (item3.VATNumber ?? " ").Replace("'", "\\'") + "', " +
                                                "eorinumber = '" + (item3.EORINumber ?? " ").Replace("'", "\\'") + "', " +
                                                "vatexempt = '" + item3.VatExempt + "', " +
                                                "nireorinumber = '" + (item3.NIREORINumber ?? " ").Replace("'", "\\'") + "', " +
                                                "iossnumber = '" + (item3.IOSSNumber ?? " ").Replace("'", "\\'") + "', " +
                                                "countryid = '" + (item3.CountryId.ToString() ?? "0") + "', " +
                                                "contactemail = '" + (item3.ContactEmail ?? " ").Replace("'", "\\'") + "', " +
                                                "packaginginstructions = '" + (item3.PackagingInstructions ?? " ").Replace("'", "\\'") + "', " +
                                                "currencyid = '" + (item3.CurrencyId.ToString() ?? "0") + "', " +
                                                "active = '" + item3.Active + "', " +
                                                "onstop = '" + item3.OnStop + "', " +
                                                "accountingintegrationtype = '" + (item3.AccountingIntegrationType ?? " ").Replace("'", "\\'") + "', " +
                                                "customerregistrationnumber = '" + (item3.CustomerRegistrationNumber ?? " ").Replace("'", "\\'") + "', " +
                                                "created_by = '" + user_gid + "', " +
                                                "created_date = '" + formattedDate + "' " +
                                                "WHERE client_id = '" + item3.ID + "'";
                                        int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 0)
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                                                        $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " ***********" +
                                                                        "*****Query****" + msSQL + "*******Apiref********",
                                                                        "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        }
                                    }
                                    objOdbcDataReader.Close();
                                }
                            }
                            else
                            {
                                objresult.status = false;
                                string errorMessage = $"Failed to fetch client details. Status code: {response3.StatusCode}, Reason: {response3.ReasonPhrase}";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                                                            $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + errorMessage,
                                                            "ErrorLog/Mintsoft/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }
                    }
                }
                else
                {
                    objresult.status = true;
                }
            }
            catch (Exception ex)
            {
                objresult.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"Error: {ex.Message}", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return objresult;
        }
        public async Task<get> DaMintsoftgetsalesorders(string employee_gid)
        {
            _queue4.Enqueue(employee_gid); // Add the request to the queue
            await ProcessQueuegetsalesorder(); // Process the queue
            return await _runningTasks;
        }
        private async Task ProcessQueuegetsalesorder()
        {
            while (_queue4.TryDequeue(out string employee_gid))
            {
                await _semaphore4.WaitAsync(); // Ensure that only one method runs at a time
                try
                {
                    if (_runningTasks != null && !_runningTasks.IsCompleted)
                    {
                        await _runningTasks; // Wait for the previous task to complete
                    }

                    _runningTasks = Getsalesorderfrommintsoft(employee_gid); // Start a new task
                    await _runningTasks;
                }
                finally
                {
                    _semaphore4.Release(); // Release the semaphore
                }
            }
        }
        public async Task<get> Getsalesorderfrommintsoft(string employee_gid)
        {
           

            get objresult = new get();
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

                msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                // code by snehith to check  mintsoft_flag for  mintsoft
                if (mintsoft_flag == "Y")
                {
                    // Set API endpoint and API key
                    string apiUrl = base_url + "/api/Order/List";
                    string apiKey = api_key;
                    bool hasMoreData = true;
                    while (hasMoreData)
                    {
                        string BaseURL = $"{apiUrl}?ClientId={lsclient_id}&WarehouseId=3";
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                            HttpResponseMessage response = await client.GetAsync(BaseURL);
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                List<getordersfrommintsoft> Orders = JsonConvert.DeserializeObject<List<getordersfrommintsoft>>(responseBody);


                                if (Orders != null)
                                {
                                    foreach (var ord in Orders)
                                    {
                                        string lssendername = "", lssenderdesignation = "", lssender_contactnumber = "", lsbranchs = "";
                                        msSQL = "select * from smr_trn_tsalesorder where so_referenceno1 = '" + ord.ExternalOrderReference + "'";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objOdbcDataReader.HasRows)
                                        {
                                            if (ord.TrackingNumber != null)
                                            {
                                                msSQL = "select * from smr_trn_tdeliveryorder where salesorder_gid = '" + objOdbcDataReader["salesorder_gid"].ToString() + "'";
                                                objOdbcDataReader3 = objdbconn.GetDataReader(msSQL);
                                                if (objOdbcDataReader3.HasRows != true)
                                                {

                                               
                                                mssalesorderGID = objcmnfunctions.GetMasterGID("VDOP");
                                               string  lsdc = "DC" + objcmnfunctions.GetRandomString(5);

                                                string customer_address = objOdbcDataReader["customer_address"].ToString();
                                                string termsandconditions = objOdbcDataReader["termsandconditions"].ToString();
                                                string shipping_to = objOdbcDataReader["shipping_to"].ToString();
                                                

                                                DateTime salesorder_date = Convert.ToDateTime(objOdbcDataReader["salesorder_date"].ToString());

                                               

                                              


                                                msSQL = " select * from hrm_mst_temployee where employee_gid='" + employee_gid + "' ";
                                                objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);
                                                if (objOdbcDataReader1.HasRows)
                                                {
                                                    objOdbcDataReader1.Read();
                                                      lssendername = objOdbcDataReader1["employee_gid"].ToString();
                                                    lssenderdesignation = objOdbcDataReader1["designation_gid"].ToString();
                                                    lssender_contactnumber = objOdbcDataReader1["employee_mobileno"].ToString();
                                                   lsbranchs = objOdbcDataReader1["branch_gid"].ToString();
                                                    objOdbcDataReader1.Close();
                                                }

                                                msSQL = " insert into smr_trn_tdeliveryorder (" +
                                          " directorder_gid, " +
                                           " directorder_date," +
                                           " directorder_refno, " +
                                           " salesorder_gid, " +
                                           " customer_gid, " +
                                           " customer_name , " +
                                           " customerbranch_gid, " +
                                           " customer_branchname, " +
                                           " customer_contactperson, " +
                                           " customer_contactnumber, " +
                                           " customer_address, " +
                                           " directorder_status, " +
                                           " terms_condition, " +
                                           " created_date, " +
                                           " created_name, " +
                                           " sender_name," +
                                           " delivered_by," +
                                           " dc_no," +
                                           " mode_of_despatch, " +
                                           " tracker_id, " +
                                             " tracker_url, " +
                                           " sender_designation," +
                                           " sender_contactnumber, " +
                                           " grandtotal_amount, " +
                                           " delivered_date," +
                                           " shipping_to, " +
                                           " no_of_boxs, " +
                                           " dc_note, " +
                                           " customer_emailid " +
                                           ") values (" +
                                           "'" + mssalesorderGID + "',";
                                                if(ord.DespatchDate  != null )
                                                {
                                                    DateTime despatch_date = Convert.ToDateTime(ord.DespatchDate);

                                                    msSQL += "'" + despatch_date.ToString("yyyy-MM-dd") + "',";
                                                }
                                                else
                                                {
                                                    msSQL += "'" + salesorder_date.ToString("yyyy-MM-dd") + "', ";
                                                }
                                                msSQL +=
                                                  "'" + mssalesorderGID + "'," +
                                                  "'" + objOdbcDataReader["salesorder_gid"].ToString() + "'," +
                                                  "'" + objOdbcDataReader["customer_gid"].ToString() + "'," +
                                                    "'" + (String.IsNullOrEmpty(objOdbcDataReader["customer_name"].ToString()) ? objOdbcDataReader["customer_name"].ToString() : objOdbcDataReader["customer_name"].ToString().Replace("'", "\\\'")) + "'," +
                                                  "'" + objOdbcDataReader["branch_gid"].ToString() + "'," +
                                                  "'" + (String.IsNullOrEmpty(objOdbcDataReader["customer_name"].ToString()) ? objOdbcDataReader["customer_name"].ToString() : objOdbcDataReader["customer_name"].ToString().Replace("'", "\\\'")) + "'," +
                                                  "'" + objOdbcDataReader["customer_contact_person"].ToString() + "'," +
                                                  "'" + objOdbcDataReader["mobile"].ToString() + "',";
                                                if(customer_address != null )
                                                {
                                                    msSQL += " '" + customer_address.Replace("'", "\\\'") + "',";
                                                }
                                                else
                                                {
                                                    msSQL += "'',";
                                                }

                                                msSQL += "'Despatch Done',";
                                                if (termsandconditions !=null)
                                                {
                                                    msSQL += "'" + termsandconditions.Replace("'", "\\\'") + "', ";
                                                }
                                                else
                                                {
                                                    msSQL += "'', ";
                                                }
                                          
                                           msSQL+=  "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                           "'" + employee_gid + "'," +
                                           "'" + lssendername + "'," +
                                           "'" + employee_gid + "'," +
                                           "'" + lsdc + "'," +
                                           "'"+ord.CourierServiceName + "'," +
                                           "'"+ ord.TrackingNumber + "'," +
                                           "'" + ord.TrackingURL + "'," +
                                           "'" + lssenderdesignation + "'," +
                                           "'" + lssender_contactnumber + "',";
                                                if (objOdbcDataReader["Grandtotal"].ToString() == null || objOdbcDataReader["Grandtotal"].ToString() == "")
                                                {
                                                    msSQL += "'0.00',";
                                                }
                                                else
                                                {
                                                    msSQL += "'" + objOdbcDataReader["Grandtotal"].ToString() + "',";
                                                }
                                                if (ord.DespatchDate != null)
                                                {
                                                    DateTime despatch_date = Convert.ToDateTime(ord.DespatchDate);
                                                    msSQL += "'" + despatch_date.ToString("yyyy-MM-dd") + "',";
                                                }
                                                else
                                                {
                                                    msSQL += "'" + salesorder_date.ToString("yyyy-MM-dd") + "', ";
                                                }
                                                if (shipping_to != null)
                                                {
                                                    msSQL += "'" + shipping_to.Replace("'", "\\\'") + "',";
                                                }
                                                else
                                                {
                                                    msSQL += "'',";
                                                }

                                                msSQL += "'" + ord.NumberOfParcels + "',";
                                                if (ord.DeliveryNotes != null)
                                                {
                                                    msSQL += "'" + ord.DeliveryNotes.Replace("'", "\\\'") + "',";
                                                }
                                                else
                                                {
                                                    msSQL += "'',";
                                                }
                                               
                                               msSQL += "'" + objOdbcDataReader["customer_email"].ToString() + "')";

                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                    if (mnResult != 0)
                                                    {
                                                        string msSQLDetails = " SELECT *  FROM  smr_trn_tsalesorderdtl WHERE salesorder_gid = '" + objOdbcDataReader["salesorder_gid"].ToString() + "'";
                                                        DataTable dtDetails = objdbconn.GetDataTable(msSQLDetails);
                                                        foreach (DataRow drDetail in dtDetails.Rows)
                                                        {
                                                            msGetGid = objcmnfunctions.GetMasterGID("VDDC");
                                                            msSQL = "INSERT INTO smr_trn_tdeliveryorderdtl (" +
                                                                    "directorderdtl_gid, " +
                                                                    "directorder_gid, " +
                                                                    "productgroup_gid, " +
                                                                    "productgroup_name, " +
                                                                    "product_gid, " +
                                                                    "product_name, " +
                                                                    "product_code, " +
                                                                    "product_uom_gid, " +
                                                                    "productuom_name, " +
                                                                    "product_qty, " + // Keep product_qty unchanged                                    
                                                                    "product_price, " +
                                                                    "product_total, " +
                                                                    "salesorderdtl_gid, " +
                                                                    "product_qtydelivered" +
                                                                    ") VALUES ( " +
                                                                    "'" + msGetGid + "', " +
                                                                    "'" + mssalesorderGID + "', " +
                                                                    "'" + drDetail["productgroup_gid"].ToString() + "', " +
                                                                    "'" + drDetail["productgroup_name"].ToString() + "', " +
                                                                    "'" + drDetail["product_gid"].ToString() + "', " +
                                                                    "'" + drDetail["product_name"].ToString() + "', " +
                                                                    "'" + drDetail["product_code"].ToString() + "', " +
                                                                    "'" + drDetail["uom_gid"].ToString() + "', " +
                                                                    "'" + drDetail["uom_name"].ToString() + "', " +
                                                                    "'" + drDetail["qty_quoted"].ToString().Replace(",", "") + "', ";

                                                            if (drDetail["price"] == null || DBNull.Value.Equals(drDetail["price"]))
                                                            {
                                                                msSQL += "null,";
                                                            }
                                                            else
                                                            {
                                                                msSQL += "'" + drDetail["price"] + "',";
                                                            }

                                                            if (drDetail["price"] == null || DBNull.Value.Equals(drDetail["price"]))
                                                            {
                                                                msSQL += "null,";
                                                            }
                                                            else
                                                            {
                                                                msSQL += "'" + drDetail["price"] + "',";
                                                            }

                                                            msSQL += "'" + objOdbcDataReader["salesorder_gid"].ToString() + "'," +
                                                                     "'" + drDetail["qty_quoted"].ToString().Replace(",", "") + "')"; // Use the calculated delivery quantity
                                                                                                                                      // Execute the insert query
                                                            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                            if (mnResult1 != 0)
                                                            {
                                                                msSQL = "select stock_gid from ims_trn_tstock where product_gid='" + drDetail["product_gid"].ToString() + "'";
                                                                string stock_gid = objdbconn.GetExecuteScalar(msSQL);
                                                                msstockdtlGid = objcmnfunctions.GetMasterGID("ISTP");
                                                                msSQL = "insert into ims_trn_tstockdtl(" +
                                                                           "stockdtl_gid," +
                                                                           "stock_gid," +
                                                                           "branch_gid," +
                                                                           "product_gid," +
                                                                           "uom_gid," +
                                                                           "issued_qty," +
                                                                           "amend_qty," +
                                                                           "damaged_qty," +
                                                                           "adjusted_qty," +
                                                                           "transfer_qty," +
                                                                           "return_qty," +
                                                                           "reference_gid," +
                                                                           "stock_type," +
                                                                           "remarks," +
                                                                           "created_by," +
                                                                           "created_date," +
                                                                           "display_field" +
                                                                           ") values ( " +
                                                                           "'" + msstockdtlGid + "'," +
                                                                           "'" + stock_gid + "'," +
                                                                           "'" + objOdbcDataReader["branch_gid"].ToString() + "'," +
                                                                           "'" + drDetail["product_gid"].ToString() + "'," +
                                                                           "'" + drDetail["uom_gid"].ToString() + "',";
                                                                if (drDetail["qty_quoted"].ToString() == null || drDetail["qty_quoted"].ToString() == "")
                                                                {
                                                                    msSQL += "'0.00',";
                                                                }
                                                                else
                                                                {
                                                                    msSQL += "'" + drDetail["qty_quoted"].ToString() + "',";

                                                                }
                                                                msSQL += "'0.00'," +
                                                                           "'0.00'," +
                                                                           "'0.00'," +
                                                                           "'0.00'," +
                                                                           "'0.00'," +
                                                                           "'" + mssalesorderGID + "'," +
                                                                           "'Delivery'," +
                                                                           "''," +
                                                                           "'" + employee_gid + "'," +
                                                                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                                           "'')";

                                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                                msSQL = " update ims_trn_tstock set " +
                                                                        " stock_qty = stock_qty - '" + drDetail["qty_quoted"].ToString() + "' " +
                                                                        " where stock_gid='" + stock_gid + "'";
                                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                                msSQL = " select distinct  " +
                                                                         " sum(qty_quoted) as qty_quoted,sum(product_delivered) as product_delivered " +
                                                                         " from smr_trn_tsalesorderdtl where salesorder_gid='" + objOdbcDataReader["salesorder_gid"].ToString() + "' group by salesorder_gid " +
                                                                         " having(qty_quoted <> product_delivered) ";
                                                                objOdbcDataReader2 = objdbconn.GetDataReader(msSQL);

                                                                if (objOdbcDataReader2.HasRows == true)
                                                                {
                                                                    msSQL = " update smr_trn_tsalesorder " +
                                                                            " set salesorder_status= 'Partially Delivered' where " +
                                                                            " salesorder_gid = '" + objOdbcDataReader["salesorder_gid"].ToString() + "'";
                                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                                }
                                                                else
                                                                {
                                                                    msSQL = " update smr_trn_tsalesorder " +
                                                                            " set salesorder_status= 'Dispatched' where " +
                                                                            " salesorder_gid = '" + objOdbcDataReader["salesorder_gid"].ToString() + "'";
                                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                                    msSQL = " update smr_trn_tdeliveryorder " +
                                                                            " set delivery_status ='Delivered' where " +
                                                                            " salesorder_gid='" + objOdbcDataReader["salesorder_gid"].ToString() + "'";
                                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                                }

                                                            }

                                                        }
                                                    }
                                                    
                                                }
                                               
                                                else
                                                {
                                                    msSQL = "update smr_trn_tdeliveryorder set tracker_url = '" + ord.TrackingNumber + "'," +
                                                        " tracker_url = '" + ord.TrackingURL + "'," +
                                                        " no_of_boxs = '"+ord.NumberOfParcels + "' where salesorder_gid = '" + objOdbcDataReader["salesorder_gid"].ToString() + "'";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                }
                                                objOdbcDataReader3.Close();
                                            }
                                        objOdbcDataReader.Close();
                                        }
                                        if (objOdbcDataReader != null)
                                        {
                                            objOdbcDataReader.Close();
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
                objresult.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"Error: {ex.Message}", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return objresult;

        }


                private string FormatDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return "NULL"; // or return an empty string if you prefer
            }

            DateTime parsedDate;
            // Try parsing the date with multiple formats
            bool success = DateTime.TryParseExact(
                dateString.Substring(0, Math.Min(dateString.Length, 23)), // Trimming to milliseconds if too long
                new[]
                {
       "yyyy-MM-ddTHH:mm:ss.fff",
       "yyyy-MM-ddTHH:mm:ss.ff",
       "yyyy-MM-ddTHH:mm:ss.f",
       "yyyy-MM-ddTHH:mm:ss"
                },
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out parsedDate
            );

            // Return the formatted date or NULL if parsing failed
            return success ? parsedDate.ToString("yyyy-MM-dd HH:mm:ss") : "NULL";
        }

    }

}
