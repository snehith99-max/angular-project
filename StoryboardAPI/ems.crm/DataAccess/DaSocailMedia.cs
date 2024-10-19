﻿using ems.crm.Models;
using ems.system.Models;
using ems.utilities.Functions;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Http.Results;
using static ems.crm.Models.leadbank_list;
using static OfficeOpenXml.ExcelErrorValue;



namespace ems.crm.DataAccess
{
    public class DaSocailMedia
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;

        string msSQL = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        public shopifystorename DaGetShopifyStoreName()
        {

            shopifystorename objMdlShopifyMessageResponse = new shopifystorename();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //var client = new RestClient("https://e32524.myshopify.com");

                var client = new RestClient("https://" + ConfigurationManager.AppSettings["shopify_store"] + ".myshopify.com");
                var request = new RestRequest("/admin/api/" + ConfigurationManager.AppSettings["store_month"] + "/shop.json", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Shopify-Access-Token", "" + ConfigurationManager.AppSettings["access_token"] + "");
                IRestResponse responseAddress = client.Execute(request);
                string address_erpid = responseAddress.Content;
                string errornetsuiteJSON = responseAddress.Content;
                objMdlShopifyMessageResponse = JsonConvert.DeserializeObject<shopifystorename>(errornetsuiteJSON);


            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error Occured While Getting Shopify Store Name!!!" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            return objMdlShopifyMessageResponse;


        }
        public shopifyproductcount DaGetShopifyProductCount()
        {
            shopifyproductcount objMdlShopifyMessageResponse = new shopifyproductcount();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //var client = new RestClient("https://e32524.myshopify.com");

                var client = new RestClient("https://" + ConfigurationManager.AppSettings["shopify_store"] + ".myshopify.com");
                var request = new RestRequest("/admin/api/" + ConfigurationManager.AppSettings["store_month"] + "/products/count.json", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Shopify-Access-Token", "" + ConfigurationManager.AppSettings["access_token"] + "");
                IRestResponse responseAddress = client.Execute(request);
                string address_erpid = responseAddress.Content;
                string errornetsuiteJSON = responseAddress.Content;
                objMdlShopifyMessageResponse = JsonConvert.DeserializeObject<shopifyproductcount>(errornetsuiteJSON);
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error Occured While Shopify Product Count" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return objMdlShopifyMessageResponse;


        }
        public shopifycustomercount DaGetShopifyCustomerCount()
        {
            shopifycustomercount objMdlShopifyMessageResponse = new shopifycustomercount();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //var client = new RestClient("https://e32524.myshopify.com");

                var client = new RestClient("https://" + ConfigurationManager.AppSettings["shopify_store"] + ".myshopify.com");
                var request = new RestRequest("/admin/api/" + ConfigurationManager.AppSettings["store_month"] + "/customers/count.json", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Shopify-Access-Token", "" + ConfigurationManager.AppSettings["access_token"] + "");
                IRestResponse responseAddress = client.Execute(request);
                string address_erpid = responseAddress.Content;
                string errornetsuiteJSON = responseAddress.Content;
                objMdlShopifyMessageResponse = JsonConvert.DeserializeObject<shopifycustomercount>(errornetsuiteJSON);
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error Occured While Shopify Customer Count" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

            return objMdlShopifyMessageResponse;

        }

        public shopifyordercount DaGetShopifyOrderCount()
        {
            shopifyordercount objMdlShopifyMessageResponse = new shopifyordercount();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //var client = new RestClient("https://e32524.myshopify.com");

                var client = new RestClient("https://" + ConfigurationManager.AppSettings["shopify_store"] + ".myshopify.com");
                var request = new RestRequest("/admin/api/" + ConfigurationManager.AppSettings["store_month"] + "/orders/count.json", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Shopify-Access-Token", "" + ConfigurationManager.AppSettings["access_token"] + "");
                IRestResponse responseAddress = client.Execute(request);
                string address_erpid = responseAddress.Content;
                string errornetsuiteJSON = responseAddress.Content;
                objMdlShopifyMessageResponse = JsonConvert.DeserializeObject<shopifyordercount>(errornetsuiteJSON);
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error Occured While Shopify Order Count" + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

            return objMdlShopifyMessageResponse;
        }

        public void DaGetContactCount(MdlSocialMedia values)
        {
            try
            {
                 
                msSQL = " select count(whatsapp_gid) as contact_count from crm_smm_whatsapp";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<contactcount_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new contactcount_list1
                        {
                            contact_count1 = dt["contact_count"].ToString(),


                        });
                        values.contactcount_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Whatsapp Contact Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetMessageCount(MdlSocialMedia values)
        {

            try
            {
                 
                msSQL = " select count(direction) as message_count from crm_trn_twhatsappmessages";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<messagecount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new messagecount_list
                        {
                            message_count = dt["message_count"].ToString(),
                        });
                        values.messagecount_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Message Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
          
        }
        public void DaGetMessageOutgoingCount(MdlSocialMedia values)
        {

            try
            {
                 
                msSQL = " SELECT COUNT(direction) AS sent_count FROM crm_trn_twhatsappmessages WHERE direction = 'outgoing'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<messageoutgoing_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new messageoutgoing_list
                        {
                            sent_count = dt["sent_count"].ToString(),


                        });
                        values.messageoutgoing_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Outgoing Message!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
       
        }

        public void DaGetMessageIncomingCount(MdlSocialMedia values)
        {

            try
            {
                 
                msSQL = " SELECT COUNT(direction) AS incoming_count FROM crm_trn_twhatsappmessages WHERE direction = 'incoming'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<messageincoming_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new messageincoming_list
                        {
                            incoming_count = dt["incoming_count"].ToString(),


                        });
                        values.messageincoming_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Incoming Message Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }        
        }
        public void DaGetEmailStatusCount(MdlSocialMedia values)
        {

            try
            {
                 
                msSQL = " Select count(status_delivery) as deliverytotal_count,count(status_open) as opentotal_count,count(status_click) as clicktotal_count from crm_smm_mailmanagement";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<emailstatus_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new emailstatus_list
                        {
                            deliverytotal_count = dt["deliverytotal_count"].ToString(),
                            opentotal_count = dt["opentotal_count"].ToString(),
                            clicktotal_count = dt["clicktotal_count"].ToString(),

                        });
                        values.emailstatus_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Email Status Count!";
            }
        }

        public void DaGetSentCount(MdlSocialMedia values)
        {

            try
            {
                 
                msSQL = " select count(mailmanagement_gid) as mail_sent from crm_smm_mailmanagement;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<sentmailcount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new sentmailcount_list
                        {
                            mail_sent = dt["mail_sent"].ToString(),


                        });
                        values.sentmailcount_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Sent Mail Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetShopifyProductCounts(MdlSocialMedia values)
        {
            try
            {
                 

                msSQL = "SELECT COUNT(shopify_productid) AS shopifyproduct_count FROM crm_smm_tshopifyproduct";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<shopifyproducts_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new shopifyproducts_list
                        {
                            shopifyproduct_count = dt["shopifyproduct_count"].ToString()
                        });
                        values.shopifyproducts_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Shopify Product Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
    }
}