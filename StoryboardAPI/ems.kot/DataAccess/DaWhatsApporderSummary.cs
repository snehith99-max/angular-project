using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.kot.Models;
using System.Configuration;
using System.Net;
using System.Reflection.Emit;
using RestSharp;
using Newtonsoft.Json;
using System.Web.Http.Results;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using System.IO;

namespace ems.kot.DataAccess
{
    public class DaWhatsApporderSummary
    {
        string msSQL = string.Empty;
        DataTable dt_datatable;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        OdbcDataReader objOdbcDataReader;
        HttpPostedFile httpPostedFile;
        string lscustomertype_gid, lscompany_code, final_path, CompanyName, MobileNumber, msdocument_gid, lssection_params, lstemplate_body;
        int mnResult, mnResult3, successcount, failedcount;
        public void Daoverallsummary(string branch_gid, mdloveralsummary values)
        {
            try
            {
                var getModuleList = new List<Folders1>();

                msSQL = "select a.kot_gid,a.order_id,a.eta_time,a.customer_phone,a.order_instructions,a.kitchen_status,a.payment_method, date_format(a.created_date, '%H:%i:%s') AS created_date, a.payment_status,a.message_id,sum(b.product_quantity) as total_quantity ,count(b.kot_product_gid) as total_product " +
                        "from otl_trn_tkot a " +
                        "left join otl_trn_tkotdtl b on b.kot_gid = a.kot_gid " +
                        "where a.branch_gid = '" + branch_gid + "' and a.order_status = 'CONFIRMED' " +
                        "and a.kitchen_status NOT IN('R', 'D')  group by kot_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Folders1
                        {
                            kot_gid = dt["kot_gid"].ToString(),
                            order_id = dt["order_id"].ToString(),
                            customer_phone = dt["customer_phone"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            payment_status = dt["payment_status"].ToString(),
                            total_quantity = dt["total_quantity"].ToString(),
                            total_product = dt["total_product"].ToString(),
                            orderstatus = dt["kitchen_status"].ToString(),
                            eta_time = dt["eta_time"].ToString(),
                            order_instructions = dt["order_instructions"].ToString(),
                            payment_method = dt["payment_method"].ToString(),
                        });
                    }
                    values.mainsummary = getModuleList;

                }


                msSQL = "select a.kot_gid,a.kotdtl_gid,a.product_quantity ,a.kot_product_gid, " +
                        "case when b.product_gid is not null then b.product_name else (select product_name from pmr_mst_tproduct a " +
                        "left join otl_trn_branch2product b on a.product_gid = b.product_gid " +
                        "where b.branch2product_gid = a.kot_product_gid) end as product_name " +
                        "from otl_trn_tkotdtl a " +
                        "left join pmr_mst_tproduct b on b.product_gid = a.kot_product_gid " +
                        "left join otl_trn_tkot e on e.kot_gid = a.kot_gid " +
                        "left join otl_trn_branch2product c on a.kot_product_gid = c.branch2product_gid " +
                        "left join otl_trn_branch2product d on d.product_gid = b.product_gid " +
                        "where e.branch_gid = '" + branch_gid + "'  and e.order_status = 'CONFIRMED' and e.kitchen_status NOT IN('R', 'D')";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList1 = new List<Folders1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new Folders1
                        {
                            kot_gid = dt["kot_gid"].ToString(),
                            kotdtl_gid = dt["kotdtl_gid"].ToString(),
                            product_quantity = dt["product_quantity"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                    }
                    values.subsummary = getModuleList1;

                }


                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting summary Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Getting summary details- (Daoverallsummary)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/" + "Daoverallsummary " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaUpdatedETD(string branch_gid, etdupdate_List values)
        {
            try
            {
                string[] timeParts = values.order_eta.Split('.', ':');
                int hours = 0;
                int minutes = 0;
                if (timeParts.Length == 1)
                {
                    minutes = Convert.ToInt32(timeParts[0]);
                }
                else if (timeParts.Length == 2)
                {
                    hours = Convert.ToInt32(timeParts[0]);
                    minutes = Convert.ToInt32(timeParts[1]);
                }
                else
                {
                    values.status = false;
                    values.message = "Invalid time format.Expected format: hh.mm or hh:mm. !!";
                }

                msSQL = "select order_id from otl_trn_tkot where kot_gid = '" + values.kot_gid + "'";
                string order_id = objdbconn.GetExecuteScalar(msSQL);
                int totalMinutes = hours * 60 + minutes;
                TimeSpan formattedTime = TimeSpan.FromMinutes(totalMinutes);
                TimeSpan current_time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
                TimeSpan total_time = current_time + formattedTime;
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials(branch_gid);
                var body = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.customer_phone + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"*PREPARING* *YOUR* *FOOD*\\n\\nYour order " + order_id + " will be ready in the next " + values.order_eta + " " + values.order_duration + "\"}}}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                 Results objsendmessage = JsonConvert.DeserializeObject<Results>(response.Content);
                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    msSQL = "Update otl_trn_tkot set order_eta='" + formattedTime + "',eta_time='" + total_time + "',kitchen_status='A' where kot_gid='" + values.kot_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        string message = "<b>PREPARING<b> <b>YOUR<b> <b>FOOD <br><br>Your order <b>" + order_id + "<b> will be ready in the next " + values.order_eta + " " + values.order_duration + "";
                        msSQL = "insert into crm_trn_twhatsappmessages(" +
                                "message_id," +
                                "contact_id," +
                                "direction," +
                                "type," +
                                "template_body," +
                                "status," +
                                "created_date)" +
                                "values(" +
                                "'" + objsendmessage.id + "'," +
                                "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                "'" + objsendmessage.direction + "'," +
                                "'Template'," +
                                "'" + message.Replace("'", "\'") + "'," +
                                "'" + objsendmessage.status + "'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        values.status = true;
                        values.message = "Cooking Started!!";
                    }

                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Updating!!";
                    objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + body + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Update/" + "DaUpdatedETD " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating  ETD !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Updating ETD" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaUpdatedETD " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaOrderstatusupdate(string kitchen_status, string kot_gid, string branch_gid, string customer_phone, result values)
        {
            try
            {
                if (kitchen_status == "R")
                {
                    msSQL = "select order_id from otl_trn_tkot where kot_gid = '" + kot_gid + "'";
                    string order_id = objdbconn.GetExecuteScalar(msSQL);
                    whatsappconfiguration getwhatsappcredentials = whatsappcredentials(branch_gid);
                    var body = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + customer_phone + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"*ORDER* *READY*\\n\\nYour order " + order_id + " is ready!! Enjoy your meal\"}}}";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Results objsendmessage = JsonConvert.DeserializeObject<Results>(response.Content);
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        msSQL = "update otl_trn_tkot set kitchen_status = '" + kitchen_status + "' where kot_gid = '" + kot_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            string message = "<b>ORDER<b> <b>READY<b> <br><br>Your order <b>" + order_id + "<b> is ready!! Enjoy your meal ";
                            msSQL = "insert into crm_trn_twhatsappmessages(" +
                                    "message_id," +
                                    "contact_id," +
                                    "direction," +
                                    "type," +
                                    "template_body," +
                                    "status," +
                                    "created_date)" +
                                    "values(" +
                                    "'" + objsendmessage.id + "'," +
                                    "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                    "'" + objsendmessage.direction + "'," +
                                    "'Template'," +
                                    "'" + message.Replace("'", "\'") + "'," +
                                    "'" + objsendmessage.status + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            values.status = true;
                            values.message = "The order is ready to be delivered.!!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occured While Updating!!";
                            objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + body + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Update/" + "DaOrderstatusupdate  " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }

                    }

                }
                else
                {
                    msSQL = "update otl_trn_tkot set kitchen_status = '" + kitchen_status + "' where kot_gid = '" + kot_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "The order has been delivered successfully!!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured While Updating!!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Updating " + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaOrderstatusupdate " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Updating  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "WhatsAppOrders/ErrorLog/Update/" + "DaOrderstatusupdate " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void Daadminreadyordersummary(string branch_gid, mdloveralsummary values)
        {
            try
            {
                var getModuleList = new List<Folders1>();

                msSQL = "select a.kot_gid,a.order_id,a.eta_time,a.customer_phone,Format(a.kot_tot_price,2) as kot_tot_price,a.payment_method,a.order_instructions,date_format(a.created_date, '%H:%i:%s') AS created_date ,a.payment_status,a.message_id,sum(b.product_quantity) as total_quantity ,c.symbol," +
                 "count(b.kot_product_gid) as total_product   from otl_trn_tkot a left join otl_trn_tkotdtl b on b.kot_gid=a.kot_gid left join crm_trn_tcurrencyexchange c on c.currency_code=b.currency " +
                 " where a.branch_gid='" + branch_gid + "' and a.kitchen_status='R' and a.kitchen_status!='D' group by kot_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Folders1
                        {
                            kot_gid = dt["kot_gid"].ToString(),
                            order_id = dt["order_id"].ToString(),
                            customer_phone = dt["customer_phone"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            payment_status = dt["payment_status"].ToString(),
                            total_quantity = dt["total_quantity"].ToString(),
                            total_product = dt["total_product"].ToString(),
                            eta_time = dt["eta_time"].ToString(),
                            kot_tot_price = dt["kot_tot_price"].ToString(),
                            order_instructions = dt["order_instructions"].ToString(),
                            symbol = dt["symbol"].ToString(),
                            payment_method = dt["payment_method"].ToString(),

                        });
                    }
                    values.mainsummary = getModuleList;
                }

                msSQL = "select a.kot_gid,a.kotdtl_gid,a.product_quantity ,a.kot_product_gid, " +
                        "case when b.product_gid is not null then b.product_name else (select product_name from pmr_mst_tproduct a " +
                        "left join otl_trn_branch2product b on a.product_gid = b.product_gid " +
                        "where b.branch2product_gid = a.kot_product_gid) end as product_name " +
                        "from otl_trn_tkotdtl a " +
                        "left join pmr_mst_tproduct b on b.product_gid = a.kot_product_gid " +
                        "left join otl_trn_tkot e on e.kot_gid = a.kot_gid " +
                        "left join otl_trn_branch2product c on a.kot_product_gid = c.branch2product_gid " +
                        "left join otl_trn_branch2product d on d.product_gid = b.product_gid " +
                        "where e.branch_gid = '" + branch_gid + "'  and e.order_status = 'CONFIRMED' and e.kitchen_status='R' and e.kitchen_status !='D'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList1 = new List<Folders1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new Folders1
                        {
                            kot_gid = dt["kot_gid"].ToString(),
                            kotdtl_gid = dt["kotdtl_gid"].ToString(),
                            product_quantity = dt["product_quantity"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                    }
                    values.subsummary = getModuleList1;

                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting summary Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Getting summary details" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/" + "Daadminreadyordersummary " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void Daadmindeliveredordersummary(string branch_gid, MdlWhatsApporderSummary values)
        {
            try
            {

                msSQL = "select a.kot_gid,a.order_id,a.customer_phone,Format(a.kot_tot_price,2) as kot_tot_price,date_format(a.created_date, '%H:%i:%s') AS created_date,sum(b.product_quantity) as total_quantity ," +
                        "count(b.kot_product_gid) as total_product " +
                        "from otl_trn_tkot a " +
                        "left join otl_trn_tkotdtl b on b.kot_gid=a.kot_gid " +
                        "where a.branch_gid='" + branch_gid + "' and a.kitchen_status = 'D' group by kot_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<delivered_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new delivered_List
                        {
                            kot_gid = dt["kot_gid"].ToString(),
                            order_id = dt["order_id"].ToString(),
                            customer_phone = dt["customer_phone"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            total_quantity = dt["total_quantity"].ToString(),
                            total_product = dt["total_product"].ToString(),
                            kot_tot_price = dt["kot_tot_price"].ToString(),


                        });
                        values.delivered_List = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting summary Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Getting summary details " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/" + "Daadmindeliveredordersummary " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void Dashopenable(string status, string branch_gid, result values)
        {
            try
            {
                if (status == "Y")
                {
                    bool ls_cart = true;
                    whatsappconfiguration getwhatsappcredentials = whatsappcredentials(branch_gid);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var Client = new RestClient("https://graph.facebook.com");
                    var requests = new RestRequest("/" + getwhatsappcredentials.meta_phone_id + "/whatsapp_commerce_settings?", Method.POST);
                    requests.AddParameter("is_cart_enabled", ls_cart);
                    requests.AddParameter("access_token", "" + getwhatsappcredentials.waaccess_token + "");
                    IRestResponse responses = Client.Execute(requests);
                    if (responses.StatusCode == HttpStatusCode.OK)
                    {
                        msSQL = "select msgsend_manger from hrm_mst_tbranch where branch_gid = '" + branch_gid + "'";
                        string manager_flag = objdbconn.GetExecuteScalar(msSQL);
                        if (manager_flag == "Y")
                        {
                            var body = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + getwhatsappcredentials.manager_number + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"The shop is now open to receive orders.\"}}}";
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                            var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                            request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                            request.AddParameter("application/json", body, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.Accepted)
                            {
                                msSQL = "update hrm_mst_tbranch set cart_status = '" + status + "' where branch_gid = '" + branch_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "The shop is now open to receive orders.!!";

                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating!!";
                                    objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + body + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating!!";
                                objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + " Error: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                            }
                        }
                        else
                        {

                            msSQL = "update hrm_mst_tbranch set cart_status = '" + status + "' where branch_gid = '" + branch_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "The shop is now open to receive orders.!!";

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating!!";
                                objcmnfunctions.LogForAudit("Error Occured while Upadting Shop Status ", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                            }

                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating!!";
                        objcmnfunctions.LogForAudit("Error Occured while Upadting Shop Status at facebook ", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
                else
                {
                    bool ls_cart = false;
                    whatsappconfiguration getwhatsappcredentials = whatsappcredentials(branch_gid);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var Client = new RestClient("https://graph.facebook.com");
                    var requests = new RestRequest("/" + getwhatsappcredentials.meta_phone_id + "/whatsapp_commerce_settings?", Method.POST);
                    requests.AddParameter("is_cart_enabled", ls_cart);
                    requests.AddParameter("access_token", "" + getwhatsappcredentials.waaccess_token + "");
                    IRestResponse responses = Client.Execute(requests);
                    if (responses.StatusCode == HttpStatusCode.OK)
                    {
                        msSQL = "select msgsend_manger from hrm_mst_tbranch where branch_gid = '" + branch_gid + "'";
                        string manager_flag = objdbconn.GetExecuteScalar(msSQL);
                        if (manager_flag == "Y")
                        {
                            var body = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + getwhatsappcredentials.manager_number + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"The WhatsApp order system is closed now.\"}}}";
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                            var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                            request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                            request.AddParameter("application/json", body, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.Accepted)
                            {
                                msSQL = "update hrm_mst_tbranch set cart_status = '" + status + "' where branch_gid = '" + branch_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "The WhatsApp order system is closed now.!!";

                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating!!";
                                    objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + body + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating!!";
                                objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + " Error: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                            }
                        }
                        else
                        {
                            msSQL = "update hrm_mst_tbranch set cart_status = '" + status + "' where branch_gid = '" + branch_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "The WhatsApp order system is closed now.!!";

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating!!";
                                objcmnfunctions.LogForAudit("Error Occured while Upadting Shop Status ", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                            }

                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating!!";
                        objcmnfunctions.LogForAudit("Error Occured while Upadting Shop Status at facebook ", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Updating - (Dashopenable)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "WhatsAppOrders/ErrorLog/Update/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void Dagetshopdetails(string branch_gid, shop_details values)
        {
            try
            {
                msSQL = "select cart_status from hrm_mst_tbranch where branch_gid='" + branch_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.cart_status = objOdbcDataReader["cart_status"].ToString();

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting  Cart details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/" + "Dagetshopdetails " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public whatsappconfiguration whatsappcredentials(string branch_gid)
        {
            whatsappconfiguration getwhatsappcredentials = new whatsappconfiguration();
            try
            {


                msSQL = " select wacatalouge_id,wachannel_id,waaccess_token,meta_phone_id,waphone_number,waworkspace_id,bird_token,manager_number from hrm_mst_tbranch where branch_gid='" + branch_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {

                    getwhatsappcredentials.wacatalouge_id = objOdbcDataReader["wacatalouge_id"].ToString();
                    getwhatsappcredentials.wachannel_id = objOdbcDataReader["wachannel_id"].ToString();
                    getwhatsappcredentials.waaccess_token = objOdbcDataReader["waaccess_token"].ToString();
                    getwhatsappcredentials.meta_phone_id = objOdbcDataReader["meta_phone_id"].ToString();
                    getwhatsappcredentials.waphone_number = objOdbcDataReader["waphone_number"].ToString();
                    getwhatsappcredentials.waworkspace_id = objOdbcDataReader["waworkspace_id"].ToString();
                    getwhatsappcredentials.bird_token = objOdbcDataReader["bird_token"].ToString();
                    getwhatsappcredentials.manager_number = objOdbcDataReader["manager_number"].ToString();

                }

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Configuration/" + "DaWhatsApporderSummary.cs " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

            return getwhatsappcredentials;
        }
        public etdupdate_List daUpdateAndPayOrder(etdupdate_List values)
        {
            try
            {
                msSQL = "update otl_trn_tkot set kitchen_status='D',payment_status = 'PAID' where kot_gid='" + values.kot_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                }
                else
                {
                    values.message = "Error occured while updating status";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating status";
            }
            return values;
        }


        public void Dawhatsappdashboard(string branch_gid, mdlwhatsapporderdashboard values)
        {
            try
            {
                msSQL = "select count(order_id) as total_ordercount,format( sum(kot_tot_price),2) as kot_totalprice, DATE_FORMAT(min(created_date), '%a, %d %b %Y') as min, DATE_FORMAT(max(created_date), '%a, %d %b %Y') as max from otl_trn_tkot where order_status = 'CONFIRMED' and payment_status = 'PAID' and branch_gid= '" + branch_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.total_ordercount = objOdbcDataReader["total_ordercount"].ToString();
                    values.kot_totalprice = objOdbcDataReader["kot_totalprice"].ToString();
                    values.min_date = objOdbcDataReader["min"].ToString();
                    values.max_date = objOdbcDataReader["max"].ToString();

                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/" + "Dagetshopdetails " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }

        public void Dawhatsappproductdtl(string branch_gid, mdlwhatsapporderprdtdtl values) {
            try
            {

                msSQL = "SELECT (SELECT COUNT(a.product_gid) FROM otl_trn_branch2product a left join pmr_mst_tproduct b on b.product_gid=a.product_gid WHERE b.status != '2' and a.branch_gid='" + branch_gid + "') AS active_Products ," +
         "    (SELECT COUNT(a.whatsapp_id) FROM otl_trn_branch2product a  left join pmr_mst_tproduct b on b.product_gid=a.product_gid  WHERE a.whatsapp_id != 'null' and b.status != '2' and a.branch_gid='" + branch_gid + "' ) AS product_added," +
         "    (SELECT COUNT(a.whatsappstock_status) FROM otl_trn_branch2product a  left join pmr_mst_tproduct b on b.product_gid=a.product_gid  WHERE  a.whatsapp_id != 'null' and a.whatsappstock_status='Y' and b.status != '2' and a.branch_gid='" + branch_gid + "') AS in_stock," +
         "    (SELECT COUNT(a.whatsappstock_status) FROM otl_trn_branch2product a left join pmr_mst_tproduct b on b.product_gid=a.product_gid WHERE a.whatsapp_id != 'null' and a.whatsappstock_status='N' and b.status != '2' and a.branch_gid='" + branch_gid + "') AS out_of_stock," +
         "    branch_name,manager_number,msgsend_manger,owner_number,msgsend_owner FROM hrm_mst_tbranch WHERE branch_gid = '" + branch_gid + "'";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {

                    values.active_Products = objOdbcDataReader["active_Products"].ToString();
                    values.product_added = objOdbcDataReader["product_added"].ToString();
                    values.in_stock = objOdbcDataReader["in_stock"].ToString();
                    values.out_of_stock = objOdbcDataReader["out_of_stock"].ToString();
                    values.branch_name = objOdbcDataReader["branch_name"].ToString();
                    values.manager_number = objOdbcDataReader["manager_number"].ToString();
                    values.msgsend_manger = objOdbcDataReader["msgsend_manger"].ToString();
                    values.owner_number = objOdbcDataReader["owner_number"].ToString();
                    values.msgsend_owner = objOdbcDataReader["msgsend_owner"].ToString();
                    values.branch_gid = branch_gid;
                    objOdbcDataReader.Close();
                }
            }
            catch (Exception ex) {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/" + "Dagetshopdetails " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

       

        public void DaUpdateorderrejectreason(mdlorderreject values)
        {
            try
            {
                CheckServicewindow objsendmessage1 = new CheckServicewindow();
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials(values.branch_gid);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/contacts/" + values.contact_id, Method.GET);
                request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                IRestResponse response1 = client.Execute(request);
                string waresponse1 = response1.Content;
                objsendmessage1 = JsonConvert.DeserializeObject<CheckServicewindow>(waresponse1);
                if (objsendmessage1.serviceWindowExpireAt != null)
                {
                    var body = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.customer_phone + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"*ORDER* *REJECTED*\\n\\nDear Customer, Your Order No. *" + values.order_id + "* has been rejected because of below reason.\\n\\n"+values.reject_reason.Replace("\n", "\\n") + "\"}}}";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var clients = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var requests = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                    requests.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                    requests.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = clients.Execute(requests);
                    Results objsendmessage = JsonConvert.DeserializeObject<Results>(response.Content);
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        msSQL = "update otl_trn_tkot set reject_reason='" + values.reject_reason + "',order_status='REJECTED' where kot_gid='" + values.kot_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            deleteMessageInsert(objsendmessage, values.order_id, values.reject_reason);
                            values.status = true;
                            values.message = "Order Rejected";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occured While Sending Message";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Sending Reject Message" + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaUpdateorderrejectreason " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured While sending Message";
                        objcmnfunctions.LogForAudit("Error Occured while sending Message: " + body + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Update/" + "DaUpdateorderrejectreason " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
                else
                {
                    msSQL = "select project_id,version_id from otl_trn_twhatsapptemplate where template_message_purpose='ORDER REJECT'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.customer_phone + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + objOdbcDataReader["project_id"].ToString() + "\",\"version\":\"" + objOdbcDataReader["version_id"].ToString() + "\",\"locale\":\"en\",\"parameters\":[{\"type\":\"string\",\"key\":\"order_id\",\"value\":\"" + values.order_id + "\"},{\"type\":\"string\",\"key\":\"reject_reason\",\"value\":\"" + values.reject_reason.Replace("\n", "\\\\n") + "\"}]}}";
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                        var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                        request1.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                        request1.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                        IRestResponse response2 = client1.Execute(request1);
                        Results objsendmessage = JsonConvert.DeserializeObject<Results>(response2.Content);
                        if (response2.StatusCode == HttpStatusCode.Accepted)
                        {
                            msSQL = "update otl_trn_tkot set reject_reason='"+values.reject_reason + "',order_status='REJECTED' where kot_gid='" + values.kot_gid+"' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                deleteMessageInsert(objsendmessage, values.order_id, values.reject_reason, objOdbcDataReader["project_id"].ToString(), objOdbcDataReader["version_id"].ToString());
                                values.status = true;
                                values.message = "Order Rejected";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occured While sending Message";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Sending Reject Message"+ " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaUpdateorderrejectreason " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occured While sending Message";
                            objcmnfunctions.LogForAudit("Error Occured while sending Message: " + contactjson + " Error: " + response2.Content, "/WhatsAppOrders/ErrorLog/Update/" + "DaUpdateorderrejectreason " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Order Reject Template is Required";

                    }


                }


            }   
            catch (Exception ex)
            {
                values.message = "Exception occured while Sending Reject Message!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Sending Reject Message" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaUpdateorderrejectreason " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void Dawosshopenable(mdlwosshopenable values)
        {
            try
            {
                if (values.shopstatus == "Y")
                {
                    bool ls_cart = true;
                    whatsappconfiguration getwhatsappcredentials = whatsappcredentials(values.branch_gid);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var Client = new RestClient("https://graph.facebook.com");
                    var requests = new RestRequest("/" + getwhatsappcredentials.meta_phone_id + "/whatsapp_commerce_settings?", Method.POST);
                    requests.AddParameter("is_cart_enabled", ls_cart);
                    requests.AddParameter("access_token", "" + getwhatsappcredentials.waaccess_token + "");
                    IRestResponse responses = Client.Execute(requests);
                    if (responses.StatusCode == HttpStatusCode.OK)
                    {
                        msSQL = "select msgsend_manger from hrm_mst_tbranch where branch_gid = '" + values.branch_gid + "'";
                        string manager_flag = objdbconn.GetExecuteScalar(msSQL);
                        if (manager_flag == "Y")
                        {
                            var body = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + getwhatsappcredentials.manager_number + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"The shop is now open to receive orders.\"}}}";
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                            var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                            request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                            request.AddParameter("application/json", body, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.Accepted)
                            {
                                msSQL = "update hrm_mst_tbranch set cart_status = '" + values.shopstatus + "' where branch_gid = '" + values.branch_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "The shop is now open to receive orders.!!";

                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating!!";
                                    objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + body + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating!!";
                                objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + " Error: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                            }
                        }
                        else
                        {
                            msSQL = "update hrm_mst_tbranch set cart_status = '" + values.shopstatus + "' where branch_gid = '" + values.branch_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "The shop is now open to receive orders.!!";

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating!!";
                                objcmnfunctions.LogForAudit("Error Occured while Upadting Shop Status ", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                            }

                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating!!";
                        objcmnfunctions.LogForAudit("Error Occured while Upadting Shop Status at facebook ", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
                else
                {
                    bool ls_cart = false;
                    whatsappconfiguration getwhatsappcredentials = whatsappcredentials(values.branch_gid);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var Client = new RestClient("https://graph.facebook.com");
                    var requests = new RestRequest("/" + getwhatsappcredentials.meta_phone_id + "/whatsapp_commerce_settings?", Method.POST);
                    requests.AddParameter("is_cart_enabled", ls_cart);
                    requests.AddParameter("access_token", "" + getwhatsappcredentials.waaccess_token + "");
                    IRestResponse responses = Client.Execute(requests);
                    if (responses.StatusCode == HttpStatusCode.OK)
                    {
                        msSQL = "select msgsend_manger from hrm_mst_tbranch where branch_gid = '" + values.branch_gid + "'";
                        string manager_flag = objdbconn.GetExecuteScalar(msSQL);
                        if (manager_flag == "Y")
                        {
                            var body = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + getwhatsappcredentials.manager_number + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"The WhatsApp order system is closed now.\"}}}";
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                            var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                            request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                            request.AddParameter("application/json", body, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.Accepted)
                            {
                                msSQL = "update hrm_mst_tbranch set cart_status = '" + values.shopstatus + "' where branch_gid = '" + values.branch_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "The WhatsApp order system is closed now.!!";

                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating!!";
                                    objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + body + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating!!";
                                objcmnfunctions.LogForAudit("Error Occured while sending confirmationmessage: " + " Error: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                            }
                        }
                        else
                        {
                            msSQL = "update hrm_mst_tbranch set cart_status = '" + values.shopstatus + "' where branch_gid = '" + values.branch_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "The WhatsApp order system is closed now.!!";

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating!!";
                                objcmnfunctions.LogForAudit("Error Occured while Upadting Shop Status ", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                            }

                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating!!";
                        objcmnfunctions.LogForAudit("Error Occured while Upadting Shop Status at facebook ", "/WhatsAppOrders/ErrorLog/Update/" + "Dashopenable " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Updating - (Dashopenable)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "WhatsAppOrders/ErrorLog/Update/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void Daorderupdates(mdlorderupdates values)
        {
            try
            {
                CheckServicewindow objsendmessage1 = new CheckServicewindow();
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials(values.branch_gid);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/contacts/" + values.contact_id, Method.GET);
                request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                IRestResponse response1 = client.Execute(request);
                string waresponse1 = response1.Content;
                objsendmessage1 = JsonConvert.DeserializeObject<CheckServicewindow>(waresponse1);
                if (objsendmessage1.serviceWindowExpireAt != null)
                {
                    var body = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.customer_phone + "\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"*ORDER* *UPDATE*\\n\\nDear Customer, Please find the below update on your order no. *" + values.order_id + "*\\n\\n" + values.order_update.Replace("\n", "\\n") + "\"}}}";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var clients = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var requests = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                    requests.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                    requests.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = clients.Execute(requests);
                    Results objsendmessage = JsonConvert.DeserializeObject<Results>(response.Content);
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        updateMessageInsert(objsendmessage, values.order_id, values.order_update);
                        values.status = true;
                        values.message = "Order Updated";                        
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured While Updating Order";
                        objcmnfunctions.LogForAudit("Error Occured while sending order updating msg Message: " + body + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Update/" + "Daorderupdates " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
                else
                {
                    msSQL = "select project_id,version_id from otl_trn_twhatsapptemplate where template_message_purpose='ORDER UPDATE'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.customer_phone + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + objOdbcDataReader["project_id"].ToString() + "\",\"version\":\"" + objOdbcDataReader["version_id"].ToString() + "\",\"locale\":\"en\",\"parameters\":[{\"type\":\"string\",\"key\":\"order_id\",\"value\":\"" + values.order_id + "\"},{\"type\":\"string\",\"key\":\"order_update\",\"value\":\"" + values.order_update.Replace("\n", "\\\\n") + "\"}]}}";
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                        var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                        request1.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                        request1.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                        IRestResponse response2 = client1.Execute(request1);
                        Results objsendmessage = JsonConvert.DeserializeObject<Results>(response2.Content);
                        if (response2.StatusCode == HttpStatusCode.Accepted)
                        {
                            updateMessageInsert(objsendmessage, values.order_id, values.order_update, objOdbcDataReader["project_id"].ToString(), objOdbcDataReader["version_id"].ToString());
                            values.status = true;
                            values.message = "Order Updated";                            
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occured While Updating Order";
                            objcmnfunctions.LogForAudit("Error Occured while sending order updating msg Message: " + contactjson + " Error: " + response2.Content, "/WhatsAppOrders/ErrorLog/Update/" + "Daorderupdates " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Order Update Template is Required";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Sending Reject Message!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Sending Reject Message" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaUpdateorderrejectreason " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaGetwoscontctsummary(MdlWhatsApporderSummary values)
        {
            try
            {
                msSQL = "select count(whatsapp_gid) as contact_count from crm_smm_whatsapp ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.contact_count = objOdbcDataReader["contact_count"].ToString();
                }
                msSQL = "SELECT b.customer_type,b.lead_status,SUBSTRING(displayName, 1, 1) AS first_letter,displayName,Wvalue,id,customer_from," +
                             " (SELECT COUNT(read_flag) FROM crm_trn_twhatsappmessages WHERE contact_id = id  AND direction = 'incoming' AND read_flag = 'N') AS count," +
                             " (SELECT CASE WHEN DATE(created_date) = CURDATE() THEN TIME_FORMAT(created_date, '%h:%i %p') " +
                             " WHEN DATE(created_date) = CURDATE() - INTERVAL 1 DAY THEN 'Yesterday'  ELSE DATE_FORMAT(created_date, '%d/%m/%y') END AS formatted_date" +
                             " FROM crm_trn_twhatsappmessages WHERE contact_id = id ORDER BY created_date DESC LIMIT 1) AS last_seen, " +
                             " (SELECT CASE WHEN LENGTH(message_text) > 10 THEN CONCAT(SUBSTRING(message_text, 1, 15), '...') ELSE message_text END FROM crm_trn_twhatsappmessages " +
                             " WHERE contact_id = id ORDER BY created_date DESC LIMIT 1) AS last_message " +
                             " FROM crm_smm_whatsapp " +
                             " LEFT JOIN crm_trn_tleadbank b ON b.wh_id =id  where Wvalue is not null " +
                             " ORDER BY CASE WHEN last_seen REGEXP '^[0-9]{2}:[0-9]{2} [APMapm]{2}$' THEN 1 WHEN last_seen = 'Yesterday' THEN 2 ELSE 3 END, last_seen DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<woscontactsummary_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new woscontactsummary_List
                        {
                            whatsapp_gid = dt["id"].ToString(),
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),
                            first_letter = dt["first_letter"].ToString(),
                            read_flag = dt["count"].ToString(),
                            last_seen = dt["last_seen"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            customer_from = dt["customer_from"].ToString(),
                            last_message = dt["last_message"].ToString(),

                        });
                    }
                    values.woscontactsummary_List = getModuleList;

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contacts";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public void DaGetWhatsappchatSummary(Mdlwhatsappchat_list values, string whatsapp_gid)
        {
            try
            {
                msSQL = "update crm_trn_twhatsappmessages set read_flag = 'Y' where contact_id ='" + whatsapp_gid + "' and direction='incoming'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL = " select a.displayName,SUBSTRING(a.displayName, 1, 1) AS first_letter,a.wvalue AS identifierValue,a.firstName,a.lastName,a.id,b.leadbank_gid,  " +
                        " b.customertype_gid from crm_smm_whatsapp a left join crm_trn_tleadbank b on b.wh_id= a.whatsapp_gid where a.id='" + whatsapp_gid + "' ";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.displayName = objOdbcDataReader["displayName"].ToString();
                    values.first_letter = objOdbcDataReader["first_letter"].ToString();
                    values.identifierValue = objOdbcDataReader["identifierValue"].ToString();
                    values.firstName = objOdbcDataReader["firstName"].ToString();
                    values.lastName = objOdbcDataReader["lastName"].ToString();
                    values.contact_id = objOdbcDataReader["id"].ToString();
                    values.leadbank_gid = objOdbcDataReader["leadbank_gid"].ToString();
                    values.customertype_gid = objOdbcDataReader["customertype_gid"].ToString();

                }

                msSQL = "select a.message_text,a.type,a.status,CONCAT(DATE_FORMAT(a.created_date, '%e %b %y, '), DATE_FORMAT(a.created_date, '%h:%i %p')) AS time,d.wvalue AS identifierValue," +
                        " a.contact_id,a.direction,a.created_date,a.message_id,b.document_name,b.document_path,a.project_id,a.template_body,a.template_footer,a.template_image," +
                        " c.media_url from crm_trn_twhatsappmessages a left join crm_trn_tfiles b on b.message_gid=a.message_id left join crm_smm_whatsapptemplate c on c.project_id=a.project_id LEFT JOIN crm_smm_whatsapp d ON d.id = a.contact_id " +
                        " WHERE  contact_id = '" + whatsapp_gid + "'ORDER BY a.created_date DESC,time DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<wosmsgchatsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new wosmsgchatsummary_list
                        {

                            message_text = dt["message_text"].ToString(),
                            type = dt["type"].ToString(),
                            status = dt["status"].ToString(),
                            time = dt["time"].ToString(),
                            contact_id = dt["contact_id"].ToString(),
                            direction = dt["direction"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            message_id = dt["message_id"].ToString(),
                            document_name = dt["document_name"].ToString(),
                            document_path = dt["document_path"].ToString(),
                            project_id = dt["project_id"].ToString(),
                            template_body = dt["template_body"].ToString(),
                            footer = dt["template_footer"].ToString(),
                            template_image = dt["template_image"].ToString(),
                            media_url = dt["media_url"].ToString(),
                            identifierValue = dt["identifierValue"].ToString(),


                        });
                    }
                    values.wosmsgchatsummary_list = getModuleList;

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Messages";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public void Daupdatewoscontact(mdlwoscontactupdate values, string user_gid)
        {
            try
            {
                msSQL = "update  crm_smm_whatsapp set " +
                        " displayName = '" + values.displayName_edit + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                        "' where wvalue ='" + values.phone_edit + "'  ";

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
        public result Dawosmsgsend(individualmessagesend values, string user_gid,string branch_gid)
        {
            result objresult = new result();
            try
            {

                int i = 0;
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials(branch_gid);

                Results objsendmessage = new Results();
                msSQL = "select section_params,template_body from otl_trn_twhatsapptemplate where project_id='" + values.project_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {

                    lssection_params = objOdbcDataReader["section_params"].ToString();
                    lstemplate_body = objOdbcDataReader["template_body"].ToString();
                }
                if (!string.IsNullOrEmpty(values.project_id) && !string.IsNullOrEmpty(lssection_params) && !string.IsNullOrEmpty(lstemplate_body))
                {
                    string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + values.project_id + "\",\"version\":\"" + values.version + "\",\"locale\":\"en\",\"parameters\":[" + lssection_params + "]}}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string waresponse = response.Content;
                    objsendmessage = JsonConvert.DeserializeObject<Results>(waresponse);

                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {

                        msSQL = "insert into crm_trn_twhatsappmessages(" +
                                "message_id," +
                                "contact_id," +
                                 "identifiervalue," +
                                "direction," +
                                "type," +
                                "project_id," +
                                "version_id," +
                                "template_body," +
                                "status," +
                                "created_date)" +
                                "values(" +
                                "'" + objsendmessage.id + "'," +
                                "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                "'" + objsendmessage.receiver.contacts[0].identifierValue + "'," +
                                "'" + objsendmessage.direction + "'," +
                                 "'Template',";
                        msSQL += "'" + objsendmessage.template.projectId + "'," +
                                "'" + objsendmessage.template.version + "'," +
                               "'" + lstemplate_body + "'," +
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
                            objresult.status = false;
                            objresult.message = "Failed to Send!!!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + " Insert failed: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Insert/" + "Dawosmsgsend " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Failed to Send!!!";
                        objcmnfunctions.LogForAudit("Error Occured while sending message: " + contactjson + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Api/" + "DaUpdatedETD " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
                else if (values.project_id != null)
                {
                    string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + values.project_id + "\",\"version\":\"" + values.version + "\",\"locale\":\"en\"}}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string waresponse = response.Content;
                    objsendmessage = JsonConvert.DeserializeObject<Results>(waresponse);

                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        msSQL = " select media_url,template_body,footer from crm_smm_whatsapptemplate where project_id ='" + values.project_id + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            values.media_url = objOdbcDataReader["media_url"].ToString();
                            values.template_body = objOdbcDataReader["template_body"].ToString();
                            values.footer = objOdbcDataReader["footer"].ToString();

                        }

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
                                "'" + values.media_url + "'," +
                                "'" + values.template_body + "'," +
                                 "'" + values.footer + "',";
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
                            objresult.status = false;
                            objresult.message = "Failed to Send!!!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + " Insert failed: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Insert/" + "Dawosmsgsend " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Failed to Send!!!";
                        objcmnfunctions.LogForAudit("Error Occured while sending message: " + contactjson + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Api/" + "DaUpdatedETD " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }

                else
                {
                    Servicewindows objsendmessage1 = new Servicewindows();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/contacts/" + values.contact_id, Method.GET);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                    IRestResponse response1 = client.Execute(request);
                    string waresponse1 = response1.Content;
                    objsendmessage1 = JsonConvert.DeserializeObject<Servicewindows>(waresponse1);
                    if (objsendmessage1.serviceWindowExpireAt != null)
                    {
                        string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"" + values.sendtext + "\"}}}";

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                        var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                        request1.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                        request1.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                        IRestResponse response2 = client1.Execute(request1);
                        string waresponse = response2.Content;
                        objsendmessage = JsonConvert.DeserializeObject<Results>(waresponse);

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
                                objresult.status = false;
                                objresult.message = "Failed to Send!";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + " Insert failed: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Insert/" + "Dawosmsgsend " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                            }
                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Failed to Send!!!";
                            objcmnfunctions.LogForAudit("Error Occured while sending message: " + contactjson + " Error: " + response2.Content, "/WhatsAppOrders/ErrorLog/Api/" + "DaUpdatedETD " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

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
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

            return objresult;
        }
        public result dawosdocumentssend(HttpRequest httpRequest,string branch_gid)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials(branch_gid);
            result objresult = new result();
            try
            {


                Servicewindows objsendmessage1 = new Servicewindows();
                string contact_id = httpRequest.Form[1];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/contacts/" + contact_id, Method.GET);
                request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                IRestResponse response1 = client.Execute(request);
                string waresponse1 = response1.Content;
                objsendmessage1 = JsonConvert.DeserializeObject<Servicewindows>(waresponse1);
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
                                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "WOS/Whatsapp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                                ms.Close();

                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "WOS/Whatsapp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";


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
                                    Results objsendmessage = new Results();
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                    var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                                    request1.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                                    request1.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                    IRestResponse response = client1.Execute(request1);
                                    string waresponse = response.Content;
                                    objsendmessage = JsonConvert.DeserializeObject<Results>(waresponse);

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
                                                objresult.status = false;
                                                objresult.message = "Error occured while sending file !!";
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Update failed: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "dawosdocumentssend" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                            }
                                            objresult.status = true;
                                            objresult.message = "Delivered!";
                                        }
                                        else
                                        {
                                            objresult.status = false;
                                            objresult.message = "Error occured while sending file!!";
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + " Insert failed: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Insert/" + "dawosdocumentssend" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                        }
                                    }
                                    else
                                    {
                                        objresult.status = false;
                                        objresult.message = "Error occured while sending file !!";
                                        objcmnfunctions.LogForAudit("Error Occured while sending message: " + contactjson + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Api/" + "dawosdocumentssend " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                    }
                                }
                                else
                                {
                                    objresult.status = false;
                                    objresult.message = "Error occured while uploading document!";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "WOS Insert failed: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Insert/" + " dawosdocumentssend" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

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
        public Mdlwosfile_list dawosgetfilesummary(string contact_id)
        {
            Mdlwosfile_list obj = new Mdlwosfile_list();
            var getimagesList = new List<wosimages_list>();
            var getfilesList = new List<wosfiles_list>();
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
                        getimagesList.Add(new wosimages_list
                        {
                            created_date = dt["created_date"].ToString(),
                            file_gid = dt["file_gid"].ToString(),
                            file_name = dt["document_name"].ToString(),
                            file_path = dt["document_path"].ToString(),
                        });
                        obj.wosimages_list = getimagesList;
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
                        getfilesList.Add(new wosfiles_list
                        {
                            created_date = dt["created_date"].ToString(),
                            file_gid = dt["file_gid"].ToString(),
                            file_name = dt["document_name"].ToString(),
                            file_path = dt["document_path"].ToString(),
                        });
                        obj.wosfiles_list = getfilesList;
                        obj.status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.message = "Exception occured !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return obj;
        }
        public void DawosContactImport(HttpRequest httpRequest, string user_gid,string branch_gid, result objResult, contact_infolist values)
        {
            string lscompany_code;

            try
            {
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials(branch_gid);
                wosRootobject objwosRootobject = new wosRootobject();
                HttpFileCollection httpFileCollection;
                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                lsfilePath = ConfigurationManager.AppSettings["contactimportexcel"] + lscompany_code + "/" + " Import_Excel/KOT_Module/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;

                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                string lsfile_gid = msdocument_gid;
                FileExtension = Path.GetExtension(FileExtension).ToLower();
                lsfile_gid = lsfile_gid + FileExtension;
                FileInfo fileinfo = new FileInfo(lsfilePath);
                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

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

                                successcount = 0;
                                failedcount = 0;
                                for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                                {
                                    DataRow row = dataTable.Rows[i];
                                    if (row.ItemArray.All(field => string.IsNullOrWhiteSpace(field?.ToString())))
                                    {
                                        dataTable.Rows.Remove(row);
                                    }
                                }
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    CompanyName = row["Name"].ToString();
                                    MobileNumber = row["MobileNumber(with Country Code)"].ToString();

                                    msSQL = "select wvalue from crm_smm_whatsapp where wvalue ='"
                                        + MobileNumber + "'";
                                    string existingmobilenumber = objdbconn.GetExecuteScalar(msSQL);

                                    if (string.IsNullOrEmpty(existingmobilenumber))
                                    {
                                        string contactjson = "{\"displayName\":\"" + CompanyName + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + "+" + MobileNumber + "\"}]}";
                                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                        var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                        var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/contacts", Method.POST);
                                        request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                                        request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                        IRestResponse response = client.Execute(request);
                                        var responseoutput = response.Content;
                                        objwosRootobject = JsonConvert.DeserializeObject<wosRootobject>(responseoutput);
                                        if (response.StatusCode == HttpStatusCode.Created)
                                        {
                                            msSQL = "insert into crm_smm_whatsapp(" +
                                            "id," +
                                            "wvalue," +
                                            "displayName," +
                                            "created_date," +
                                            "created_by" +
                                            ")" +
                                            "values(" +
                                            "'" + objwosRootobject.id + "',";
                                            if (MobileNumber.StartsWith("+"))
                                            {
                                                msSQL += "'" + MobileNumber + "',";

                                            }
                                            else
                                            {
                                                msSQL += "'" + "+" + MobileNumber + "',";
                                            }
                                            msSQL += "'" + CompanyName + "'," +
                                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                     "'" + user_gid + "')";

                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult == 1)
                                            {
                                                successcount++;
                                            }
                                            else
                                            {
                                                failedcount++;
                                            }

                                        }
                                        else
                                        {
                                            failedcount++;
                                        }
                                    }
                                    else
                                    {
                                        failedcount++;
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = ex.ToString();
                    return;
                }
                if (mnResult == 1)
                {
                    if (failedcount > 0 && successcount > 0)
                    {
                        objResult.status = true;
                        objResult.message = successcount + " New Contacts imported Successfully. " + failedcount + " Contacts got failed";
                    }
                    else
                    {
                        objResult.status = true;
                        objResult.message = successcount + " New Contacts imported Successfully";
                    }
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Posting " + failedcount + " Contacts got failed";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetwosmsgtemplatesummary(MdlWhatsApporderSummary values)
        {
            try
            {
                msSQL = "select project_id,version_id,template_message_purpose from otl_trn_twhatsapptemplate where template_message_purpose not in('ORDER REJECT','ORDER UPDATE') ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<wosmsgtemplate_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new wosmsgtemplate_list
                        {
                            id = dt["project_id"].ToString(),
                            p_name = dt["template_message_purpose"].ToString(),
                            version = dt["version_id"].ToString(),
                        });
                        values.wosmsgtemplate_list = getModuleList;
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
        public void DaGettotalcontactlist(MdlWhatsApporderSummary values)
        {
            try
            {

                msSQL = "select count(whatsapp_gid) as contact_count from crm_smm_whatsapp ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.contact_count = objOdbcDataReader["contact_count"].ToString();
                }
                msSQL = "select id,whatsapp_gid,wvalue,displayName from crm_smm_whatsapp ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<wostotalcontact_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new wostotalcontact_list
                        {
                            whatsapp_gid = dt["id"].ToString(),
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),

                        });
                    }
                    values.wostotalcontact_list = getModuleList;

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contacts";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/ " + " DaGettotalcontactlist" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaPostbulktemplatesend(string branch_gid, mdlwosbulkremplates values)
        {
            try
            {
                msSQL = "select version_id,project_id,section_params,template_body from otl_trn_twhatsapptemplate where template_message_purpose = 'PRODUCT LIST'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    string version_id = objOdbcDataReader["version_id"].ToString();
                    string project_id = objOdbcDataReader["project_id"].ToString();
                    string section_params = objOdbcDataReader["section_params"].ToString();
                    string template_body = objOdbcDataReader["template_body"].ToString();

                    if (!string.IsNullOrEmpty(version_id) && !string.IsNullOrEmpty(project_id) && !string.IsNullOrEmpty(section_params) && !string.IsNullOrEmpty(template_body))
                    {
                        HttpContext ctx = HttpContext.Current;
                        System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>
                        {
                            HttpContext.Current = ctx;
                            Dawossendbulktemplates(version_id, project_id, section_params, template_body, branch_gid, values.wosbulktemplatesend);
                        }));
                        t.Start();
                        values.status = true;
                        values.message = "Messages sent successfully!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Template Id's Required";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Template Id's Required";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while send bulk template msg";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While inserting details" + ex.Message.ToString() + " *********Apiref********", "/WhatsAppOrders/ErrorLog/Api/" + " DaPostbulktemplatesend " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }

        public void Dawossendbulktemplates(string version_id, string project_id, string section_params,string template_body, string branch_gid, List<wosbulktemplate_send> wosbulktemplatesend)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials(branch_gid);

            foreach (var item in wosbulktemplatesend)
            {
                Results objsendmessage = new Results();
                try
                {
                    string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + item.value + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + project_id + "\",\"version\":\"" + version_id + "\",\"locale\":\"en\",\"parameters\":[" + section_params + "]}}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/channels/" + getwhatsappcredentials.wachannel_id + "/messages", Method.POST);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string waresponse = response.Content;
                    objsendmessage = JsonConvert.DeserializeObject<Results>(waresponse);

                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {

                        msSQL = "insert into crm_trn_twhatsappmessages(" +
                                "message_id," +
                                "contact_id," +
                                 "identifiervalue," +
                                "direction," +
                                "type," +
                                "project_id," +
                                "version_id," +
                                "template_body," +
                                "status," +
                                "created_date)" +
                                "values(" +
                                "'" + objsendmessage.id + "'," +
                                "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                "'" + objsendmessage.receiver.contacts[0].identifierValue + "'," +
                                "'" + objsendmessage.direction + "'," +
                                "'Template'," +
                                "'" + objsendmessage.template.projectId + "'," +
                                "'" + objsendmessage.template.version + "'," +
                                "'" + template_body.Replace("'","\'") + "'," +
                                "'" + objsendmessage.status + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "update crm_trn_twhatsappmessages set sendcampaign_flag ='Y'  where message_id  ='" + objsendmessage.id + "' and project_id = '" + project_id + "'";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult3 == 0)
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "****************Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + " Dawossendbulktemplates" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                            }
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "****************Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Insert/" + " Dawossendbulktemplates" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("Error Occured while send bulk template message: " + contactjson + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Api/" + " Dawossendbulktemplates " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                catch (Exception ex)
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured while sending message to " + item.value + " project ID : " + project_id + " *******" + ex.Message.ToString() + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Insert/" + " Dawossendbulktemplates" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }

        }

        public void updateMessageInsert(Results objsendmessage, string order_id, string order_update,string project_id="",string version_id="")
        {
            string message = "<b>ORDER<b> <b>UPDATE<b><br><br>Dear Customer, Please find the below update on your order no.<b>" + order_id + "<b><br><br>" + order_update.Replace("\n", "<br>");
            msSQL = "insert into crm_trn_twhatsappmessages(" +
                    "message_id," +
                    "contact_id," +
                    "direction," +
                    "type," +
                    "project_id," +
                    "version_id," +
                    "template_body," +
                    "status," +
                    "created_date)" +
                    "values(" +
                    "'" + objsendmessage.id + "'," +
                    "'" + objsendmessage.receiver.contacts[0].id + "'," +
                    "'" + objsendmessage.direction + "'," +
                    "'Template'," +
                    "'" + project_id + "'," +
                    "'" + version_id + "'," +
                    "'" + message.Replace("'","\'") + "'," +
                    "'" + objsendmessage.status + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
        }

        public void deleteMessageInsert(Results objsendmessage, string order_id, string order_reject, string project_id = "", string version_id = "")
        {
            string message = "<b>ORDER<b> <b>REJECTED<b><br><br>Dear Customer, Your order no.<b>" + order_id + "<b> has been rejected because of below reason.<br><br>" + order_reject.Replace("\n", "<br>");
            msSQL = "insert into crm_trn_twhatsappmessages(" +
                    "message_id," +
                    "contact_id," +
                    "direction," +
                    "type," +
                    "project_id," +
                    "version_id," +
                    "template_body," +
                    "status," +
                    "created_date)" +
                    "values(" +
                    "'" + objsendmessage.id + "'," +
                    "'" + objsendmessage.receiver.contacts[0].id + "'," +
                    "'" + objsendmessage.direction + "'," +
                    "'Template'," +
                    "'" + project_id +"'," +
                    "'" + version_id+ "'," +
                    "'" + message.Replace("'", "\'") + "'," +
                    "'" + objsendmessage.status + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
        }

        public result dawoscontactcreate(mdlwoscontactcreate values, string user_gid, string branch_gid)
        {
            result objresult = new result();
            try
            {
                msSQL = "select wvalue from crm_smm_whatsapp where wvalue='" + values.phone.e164Number + "' ";
                string existingmobilenumber = objdbconn.GetExecuteScalar(msSQL);

                if (string.IsNullOrEmpty(existingmobilenumber))
                {
                    int i = 0;
                    whatsappconfiguration getwhatsappcredentials = whatsappcredentials(branch_gid);
                    wosRootobject objwosRootobject = new wosRootobject();
                    string contactjson = "{\"displayName\":\"" + values.CompanyName + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + values.phone.e164Number + "\"}],\"firstName\":\"" + values.firstName + "\",\"gender\":\"" + values.gender + "\",\"lastName\":\"" + values.lastName + "\"}";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.waworkspace_id + "/contacts", Method.POST);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.bird_token + "");
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var responseoutput = response.Content;
                    objwosRootobject = JsonConvert.DeserializeObject<wosRootobject>(responseoutput);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {

                        msSQL = "insert into crm_smm_whatsapp(id,wvalue,displayName,created_date,created_by)values(" +
                                "'" + objwosRootobject.id + "'," +
                                "'" + values.phone.e164Number + "'," +
                                "'" + values.CompanyName + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                "'" + user_gid + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            objresult.status = true;
                            objresult.message = "Contact created successfully!";
                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Error occured while adding contact!";
                        }

                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Error occured while creating contact!!";
                        objcmnfunctions.LogForAudit("Error occured while creating contact: " + contactjson + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Api/" + " dawoscontactcreate " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Contact number already exists !!";

                }
            }
            catch (Exception ex)
            {
                objresult.message = "Error occured while posting contact!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

            return objresult;
        }

    }
}