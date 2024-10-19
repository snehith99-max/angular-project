using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.sales.Models;
using RestSharp;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;



namespace ems.sales.DataAccess
{
    public class DaWhatsappproductpricemanagement
    {
        string msSQL = string.Empty;
        DataTable dt_datatable;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        OdbcDataReader objOdbcDataReader;
        int mnresult, mnResult, mnResult1, mnResult2;
        string msGetGid, stock_status, lscurrency_code, lscompany_website, lsproduct_desc, lscost_price, lsproduct_image, lscompany_name, lsproduct_name, lsbranch2product_gid;
        public void DaGetwabranchsummary(Mdlwhatsappproductpricemanagement values)
        {
            try
            {
                msSQL = "select count(product_gid) as total_products from pmr_mst_tproduct";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                   
                    values.total_products = objOdbcDataReader["total_products"].ToString();
                    objOdbcDataReader.Close();
                }

                msSQL = "select a.branch_gid,a.branch_code,a.branch_name,COUNT(DISTINCT b.product_gid) AS assignedproduct,a.manager_number,a.msgsend_manger,a.owner_number,a.msgsend_owner,a.cart_status from hrm_mst_tbranch a " +
                    "left join otl_trn_branch2product b on b.branch_gid=a.branch_gid group by a.branch_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branchsum_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branchsum_list
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_code = dt["branch_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            assignedproduct = dt["assignedproduct"].ToString(),
                            manager_number = dt["manager_number"].ToString(),
                            msgsend_manger = dt["msgsend_manger"].ToString(),
                            owner_number = dt["owner_number"].ToString(),
                            msgsend_owner = dt["msgsend_owner"].ToString(),
                            cart_status = dt["cart_status"].ToString(),
                        });
                    }
                    values.branchsum_list = getModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading branch summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Getting summary details" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/" + "DaGetwabranchsummary " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaGetWaunassignedproductsummary(string branch_gid, Mdlwhatsappproductpricemanagement values)
        {
            try
            {
                msSQL = " SELECT a.product_image,a.product_gid,a.customerproduct_code,a.product_desc,a.mrp_price, a.product_code,product_name " +
                        "from pmr_mst_tproduct a where a.product_gid not in  (select product_gid from otl_trn_branch2product where branch_gid = '" + branch_gid + "')order by a.product_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<unassignedproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new unassignedproduct_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),
                        });
                    }
                    values.unassignedproduct_list = getModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting unassignedproducts!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Getting summary details" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/" + "DaGetWaunassignedproductsummary " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void Dapostwaassignedlist(string user_gid, waassign_list values)
        {
            try
            {

                for (int i = 0; i < values.waprodassign.ToArray().Length; i++)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("BPRO");
                    msSQL = " insert into otl_trn_branch2product ( " +
                            " branch2product_gid, " +
                            " branch_gid, " +
                            " product_gid, " +
                            " whatsapp_price, " +
                            " cost_price, " +
                            " created_by," +
                            " created_date ) " +
                            " values (  " +
                            " '" + msGetGid + "', " +
                            " '" + values.branch_gid + "', " +
                            " '" + values.waprodassign[i].product_gid + "', " +
                            " '" + values.waprodassign[i].mrp_price + "', " +
                            " '" + values.waprodassign[i].mrp_price + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product assigned successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Product Assign";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Assign Product to summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While inserting details" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Insert/" + "Dapostwaassignedlist " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetwaassignedproductSummary(string branch_gid, Mdlwhatsappproductpricemanagement values)
        {
            try
            {


                msSQL = " select a.branch_gid, a.product_gid, a.branch2product_gid, a.whatsapp_price,b.product_image," +
                    " b.customerproduct_code, b.product_code, b.product_name, b.product_desc from otl_trn_branch2product a" +
                    " left join pmr_mst_tproduct b on b.product_gid = a.product_gid where a.branch_gid='" + branch_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetAmendProduct = new List<assignedproduct_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        GetAmendProduct.Add(new assignedproduct_list
                        {
                            branch_gid = row["branch_gid"].ToString(),
                            product_gid = row["product_gid"].ToString(),
                            branch2product_gid = row["branch2product_gid"].ToString(),
                            whatsapp_price = row["whatsapp_price"].ToString(),
                            product_image = row["product_image"].ToString(),
                            sku = row["customerproduct_code"].ToString(),
                            product_name = row["product_name"].ToString(),
                            product_code = row["product_code"].ToString(),
                            product_desc = row["product_desc"].ToString(),
                        });
                        values.assignedproduct_list = GetAmendProduct;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Getting summary details- (Daoverallsummary)  " + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********",
                 "/WhatsAppOrders/ErrorLog/Summary/" + "DaGetwaassignedproductSummary " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void Daupdatewaproductprice(string user_gid, waassign_list values)
        {
            try
            {
                whatsappconfiguration2 getwhatsappcredentials = whatsappcredentials2(values.branch_gid);
                paymentgatewayconfiguration getpaymentgatewaycredentials = paymentgatewaycredentials();

                foreach (var data in values.waprodassign)
                {

                    msSQL = "select whatsapp_id from otl_trn_branch2product where branch2product_gid='" + data.branch2product_gid + "' and branch_gid='" + values.branch_gid + "' ";
                    string whatsapp_id = objdbconn.GetExecuteScalar(msSQL);
                    if (whatsapp_id != "null" && whatsapp_id != "")
                    {
                        int costPriceCents = (int)(decimal.Parse(data.whatsapp_price) * 100);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var Client = new RestClient("https://graph.facebook.com");
                        var request = new RestRequest("/v20.0/" + whatsapp_id, Method.POST);
                        request.AddParameter("price", costPriceCents);
                        request.AddParameter("access_token", getwhatsappcredentials.waaccess_token);
                        IRestResponse response = Client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            if (getpaymentgatewaycredentials.payment_gateway == "STRIPE")
                            {
                                string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(getpaymentgatewaycredentials.stripe_key + ":"));
                                string token = "Basic " + encodedCredentials;
                                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency = 'Y'";
                                string currency = objdbconn.GetExecuteScalar(msSQL);
                                double amount = Convert.ToDouble(data.whatsapp_price) * 100;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var priceclient = new RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                                var pricerequest = new RestRequest("/v1/prices", Method.POST);
                                pricerequest.AddHeader("authorization", token);
                                pricerequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                pricerequest.AddParameter("currency", currency);
                                pricerequest.AddParameter("unit_amount", amount);
                                pricerequest.AddParameter("product", data.branch2product_gid);
                                IRestResponse priceresponse = priceclient.Execute(pricerequest);
                                if (priceresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    stripePriceResponses objstripePriceResponse = JsonConvert.DeserializeObject<stripePriceResponses>(priceresponse.Content);
                                    msSQL = "update otl_trn_branch2product set stripe_price_gid = '" + objstripePriceResponse.id + "' ,whatsapp_price='" + data.whatsapp_price + "'where branch2product_gid = '" + data.branch2product_gid + "' and  branch_gid='" + values.branch_gid + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {

                                        values.status = true;
                                        values.message = "WhatsApp Product Price Updated Successfully!! ";

                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error while updating WhatsApp Product Price!!";
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred " + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Daupdatewaproductprice " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                    }
                                }
                                else
                                {
                                    msSQL = "select whatsapp_price from otl_trn_branch2product where branch2product_gid='" + data.branch2product_gid + "' and branch_gid='" + values.branch_gid + "' ";
                                    string whatsapp_price = objdbconn.GetExecuteScalar(msSQL);
                                    int whatsapp_price1 = (int)(decimal.Parse(whatsapp_price) * 100);
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    var Clients = new RestClient("https://graph.facebook.com");
                                    var requests = new RestRequest("/v20.0/" + whatsapp_id, Method.POST);
                                    requests.AddParameter("price", whatsapp_price1);
                                    requests.AddParameter("access_token", getwhatsappcredentials.waaccess_token);
                                    IRestResponse responses = Clients.Execute(requests);
                                    if (responses.StatusCode == HttpStatusCode.OK)
                                    { 

                                    }
                                     values.status = false;
                                    values.message = "Error while updating WhatsApp Product Price at stripe!!";
                                    objcmnfunctions.LogForAudit("Error Occured while updating price at whatsapp catalouge " + " Error: " + priceresponse.Content, "/WhatsAppOrders/ErrorLog/Update/" + "Daupdatewaproductprice " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                }
                            }
                            else
                            {
                                msSQL = "update otl_trn_branch2product set whatsapp_price='" + data.whatsapp_price + "' where branch2product_gid = '" + data.branch2product_gid + "' and  branch_gid='" + values.branch_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "WhatsApp Product Price Updated Successfully!! ";

                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error while updating WhatsApp Product Price!!";

                                }

                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error while updating WhatsApp Product Price!!";
                            objcmnfunctions.LogForAudit("Error Occured while updating price at whatsapp catalouge " + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/Update/" + "Daupdatewaproductprice " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }

                    }
                    else
                    {
                        msSQL = " update otl_trn_branch2product set whatsapp_price='" + data.whatsapp_price + "' where branch_gid='" + values.branch_gid + "' and branch2product_gid='" + data.branch2product_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "WhatsApp Product Price Updated Successfully!! ";
                        }
                        else
                        {
                            values.status = true;
                            values.message = "Error while updating WhatsApp Product Price!!";
                            objcmnfunctions.LogForAudit("Error Occured " + " Error: " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Daupdatewaproductprice " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while update waproduct price!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Daupdatewaproductprice " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public whatsappconfiguration2 whatsappcredentials2(string branch_gid)
        {
            whatsappconfiguration2 getwhatsappcredentials = new whatsappconfiguration2();
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
                    objOdbcDataReader.Close();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Configuration/" + "DaWhatsappproductpricemanagement.cs " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

            return getwhatsappcredentials;
        }
        public void DaGetbranchwhatsappProductSummary(string branch_gid, Mdlstockdetails values)
        {

            try
            {
                msSQL = "SELECT (SELECT COUNT(a.product_gid) FROM otl_trn_branch2product a left join pmr_mst_tproduct b on b.product_gid=a.product_gid WHERE b.status != '2' and a.branch_gid='" + branch_gid + "') AS active_Products ," +
                     "    (SELECT COUNT(a.whatsapp_id) FROM otl_trn_branch2product a  left join pmr_mst_tproduct b on b.product_gid=a.product_gid  WHERE a.whatsapp_id != 'null' and b.status != '2' and a.branch_gid='" + branch_gid + "' ) AS product_added," +
                     "    (SELECT COUNT(a.whatsappstock_status) FROM otl_trn_branch2product a  left join pmr_mst_tproduct b on b.product_gid=a.product_gid  WHERE  a.whatsapp_id != 'null' and a.whatsappstock_status='Y' and b.status != '2' and a.branch_gid='" + branch_gid + "') AS in_stock," +
                     "    (SELECT COUNT(a.whatsappstock_status) FROM otl_trn_branch2product a left join pmr_mst_tproduct b on b.product_gid=a.product_gid WHERE a.whatsapp_id != 'null' and a.whatsappstock_status='N' and b.status != '2' and a.branch_gid='" + branch_gid + "') AS out_of_stock; ";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                   
                    values.active_Products = objOdbcDataReader["active_Products"].ToString();
                    values.product_added = objOdbcDataReader["product_added"].ToString();
                    values.in_stock = objOdbcDataReader["in_stock"].ToString();
                    values.out_of_stock = objOdbcDataReader["out_of_stock"].ToString();
                    objOdbcDataReader.Close();
                }

                msSQL = "select a.product_gid,a.branch2product_gid,a.branch_gid,a.whatsapp_price,(case when b.shopify_productid is not null then 'No'  else 'Yes' end)as shopify_productid,b.product_image,b.product_name,b.product_code,c.productgroup_name," +
                    "b.mrp_price,b.product_desc,CASE  WHEN b.status = '1' THEN 'Active' WHEN b.status IS NULL OR b.status = '' OR b.status = '2' THEN 'Inactive' END AS Status,a.whatsapp_id,a.whatsappstock_status from otl_trn_branch2product a" +
                    " left join pmr_mst_tproduct b on b.product_gid=a.product_gid   " +
                    " left join pmr_mst_tproductgroup c on c.productgroup_gid =b.productgroup_gid where  b.status !='2' and a.branch_gid='" + branch_gid + "' order by a.created_date desc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branchproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branchproduct_list
                        {
                            shopify_productid = dt["shopify_productid"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            Status = dt["Status"].ToString(),
                            whatsapp_id = dt["whatsapp_id"].ToString(),
                            whatsappstock_status = dt["whatsappstock_status"].ToString(),
                            whatsapp_price = dt["whatsapp_price"].ToString(),
                            branch2product_gid = dt["branch2product_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                    }
                    values.branchproduct_list = getModuleList;

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/ " + "DaGetbranchwhatsappProductSummary " + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaBranchAddproducttowhatsapp(string user_gid, string product_gid, string branch_gid, branchproduct_list values)
        {
            try
            {
                whatsappconfiguration2 getwhatsappcredentials = whatsappcredentials2(branch_gid);
                paymentgatewayconfiguration getpaymentgatewaycredentials = paymentgatewaycredentials();

                msSQL = "SELECT currency_code FROM crm_trn_tcurrencyexchange WHERE default_currency = 'Y';";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                   
                    lscurrency_code = objOdbcDataReader["currency_code"].ToString();
                    objOdbcDataReader.Close();
                }
                msSQL = "SELECT company_name FROM adm_mst_tcompany";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                   
                    lscompany_name = objOdbcDataReader["company_name"].ToString();
                    objOdbcDataReader.Close();
                }
                msSQL = "select a.product_name,a.product_desc,b.whatsapp_price,a.product_image,b.branch2product_gid from pmr_mst_tproduct a left join otl_trn_branch2product b on b.product_gid=a.product_gid where a.product_gid='" + product_gid + "' and b.branch_gid='" + branch_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                   
                    lsproduct_name = objOdbcDataReader["product_name"].ToString();
                    lsproduct_desc = objOdbcDataReader["product_desc"].ToString();
                    lscost_price = objOdbcDataReader["whatsapp_price"].ToString();
                    lsproduct_image = objOdbcDataReader["product_image"].ToString();
                    lsbranch2product_gid = objOdbcDataReader["branch2product_gid"].ToString();
                    objOdbcDataReader.Close();
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
                request.AddParameter("retailer_id", lsbranch2product_gid);

                IRestResponse response = Client.Execute(request);
                dynamic responseData = JsonConvert.DeserializeObject(response.Content);
                string facebookProductId = responseData.id;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (getpaymentgatewaycredentials.payment_gateway == "STRIPE")
                    {
                        
                        msSQL = "select stripe_price_gid from otl_trn_branch2product where branch2product_gid='" + lsbranch2product_gid + "'";
                        string stripe_price_gid = objdbconn.GetExecuteScalar(msSQL);
                        if (stripe_price_gid == "null" || stripe_price_gid == "")
                        {
                            string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(getpaymentgatewaycredentials.stripe_key + ":"));
                            string token = "Basic " + encodedCredentials;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var productclient = new RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                            var productrequest = new RestRequest("/v1/products", Method.POST);
                            productrequest.AddHeader("authorization", token);
                            productrequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                            productrequest.AddParameter("name", lsproduct_name);
                            productrequest.AddParameter("description", lsproduct_desc);
                            productrequest.AddParameter("id", lsbranch2product_gid);
                            IRestResponse productresponse = productclient.Execute(productrequest);
                            if (productresponse.StatusCode == HttpStatusCode.OK)
                            {
                                double amount = Convert.ToDouble(lscost_price) * 100;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var priceclient = new RestClient(ConfigurationManager.AppSettings["stripeBaseURL"].ToString());
                                var pricerequest = new RestRequest("/v1/prices", Method.POST);
                                pricerequest.AddHeader("authorization", token);
                                pricerequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                pricerequest.AddParameter("currency", lscurrency_code);
                                pricerequest.AddParameter("unit_amount", amount);
                                pricerequest.AddParameter("product", lsbranch2product_gid);
                                IRestResponse priceresponse = priceclient.Execute(pricerequest);
                                if (priceresponse.StatusCode == HttpStatusCode.OK)
                                {
                                    stripePriceResponses objstripePriceResponse = JsonConvert.DeserializeObject<stripePriceResponses>(priceresponse.Content);
                                    msSQL = "update otl_trn_branch2product set stripe_price_gid = '" + objstripePriceResponse.id + "',whatsapp_id='" + facebookProductId + "',whatsappproduct_updatedby='" + user_gid + "' where branch2product_gid = '" + lsbranch2product_gid + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Product moved to WhatsApp Successfully";

                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error occurred while adding the product.";
                                        objcmnfunctions.LogForAudit("Error Occured" + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/ProductPush/" + "DaBranchAddproducttowhatsapp " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                    }
                                }
                                else
                                {
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    var Clients = new RestClient("https://graph.facebook.com");
                                    var requests = new RestRequest("/v20.0/" + facebookProductId, Method.DELETE);
                                    requests.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                    requests.AddParameter("access_token", getwhatsappcredentials.waaccess_token);
                                    IRestResponse responses = Clients.Execute(requests);
                                    if (responses.StatusCode == HttpStatusCode.OK)
                                    {

                                    }
                                    values.status = false;
                                    values.message = "Error occurred while adding the product at stripe.";
                                    objcmnfunctions.LogForAudit("Error Occured while updating product price at stripe: " + " Error: " + priceresponse.Content, "/WhatsAppOrders/ErrorLog/ProductPush/" + "DaBranchAddproducttowhatsapp " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                }

                            }

                            else
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var Clients = new RestClient("https://graph.facebook.com");
                                var requests = new RestRequest("/v20.0/" + facebookProductId, Method.DELETE);
                                requests.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                requests.AddParameter("access_token", getwhatsappcredentials.waaccess_token);
                                IRestResponse responses = Clients.Execute(requests);
                                if (responses.StatusCode == HttpStatusCode.OK)
                                {

                                }
                                values.status = false;
                                values.message = "Error occurred while adding the product in stripe.";
                                objcmnfunctions.LogForAudit("Error Occured while adding product at stripe: " + " Error: " + productresponse.Content, "/WhatsAppOrders/ErrorLog/ProductPush/" + "DaBranchAddproducttowhatsapp " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                            }
                        }
                        else
                        {
                            msSQL = "update otl_trn_branch2product set whatsapp_id='" + facebookProductId + "',whatsappproduct_updatedby='" + user_gid + "' where branch2product_gid='" + lsbranch2product_gid + "' and branch_gid ='" + branch_gid + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = "Product Added to WhatsApp Successfully";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error occurred while updating the product.";
                                objcmnfunctions.LogForAudit("Error Occured" + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/ProductPush/" + "DaBranchAddproducttowhatsapp " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                            }
                        }
                    }
                    else
                    {
                        msSQL = "update otl_trn_branch2product set whatsapp_id='" + facebookProductId + "',whatsappproduct_updatedby='" + user_gid + "' where branch2product_gid='" + lsbranch2product_gid + "' and branch_gid ='" + branch_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Added to WhatsApp Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error occurred while adding the product to WhatsApp.";
                            objcmnfunctions.LogForAudit("Error Occured" + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/ProductPush/" + "DaBranchAddproducttowhatsapp " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }

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
                        values.message = "Error While Adding Product at WhatsApp";
                        objcmnfunctions.LogForAudit("Error Occured while add product to whatsapp catalouge " + " Error: " + response.Content, "/WhatsAppOrders/ErrorLog/ProductPush/" + "DaBranchAddproducttowhatsapp " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Add product to whatsapp!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/ProductPush/" + "DaBranchAddproducttowhatsapp " + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

        }
        public void DaBranchupdatewhatsappstockstatus(branchproduct_list values)
        {

            try
            {
                whatsappconfiguration2 getwhatsappcredentials = whatsappcredentials2(values.branch_gid);

                msSQL = "select whatsappstock_status from otl_trn_branch2product where whatsapp_id='" + values.whatsapp_id + "'  and branch_gid ='" + values.branch_gid + "' ";
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
                        msSQL = "update otl_trn_branch2product set whatsappstock_status='N' where whatsapp_id='" + values.whatsapp_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = "update otl_trn_branch2product set whatsappstock_status='Y' where whatsapp_id='" + values.whatsapp_id + "'";
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
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Updating " + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaBranchupdatewhatsappstockstatus " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
            }
            catch (Exception ex)
            {

                values.message = "Error While Updating whats app product Stock Status";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaBranchupdatewhatsappstockstatus " + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaBranchRemoveproductfromwt(string whatsapp_id, string branch_gid, branchproduct_list values)
        {
            try
            {
                whatsappconfiguration2 getwhatsappcredentials = whatsappcredentials2(branch_gid);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var Client = new RestClient("https://graph.facebook.com");
                var request = new RestRequest("/v20.0/" + whatsapp_id, Method.DELETE);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("access_token", getwhatsappcredentials.waaccess_token);
                IRestResponse response = Client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    msSQL = "update otl_trn_branch2product set whatsapp_id='null',whatsappstock_status='Y' where whatsapp_id='" + whatsapp_id + "' ";
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
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred" + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Delete/" + "DaBranchRemoveproductfromwt " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while remove product from whatsapp!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetwabproduct(string branch_gid, Mdlwhatsappproductpricemanagement values)
        {

            try
            {
                msSQL = "select b.product_name," +
                   "a.whatsapp_id,a.whatsappstock_status from otl_trn_branch2product a" +
                   " left join pmr_mst_tproduct b on b.product_gid=a.product_gid   " +
                   "  where  b.status !='2' and a.branch_gid='" + branch_gid + "' and a.whatsapp_id!='null' and a.whatsappstock_status='N' order by a.whatsapp_id desc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branchofsproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branchofsproduct_list
                        {
                            product_name = dt["product_name"].ToString(),
                            whatsapp_id = dt["whatsapp_id"].ToString(),
                            whatsappstock_status = dt["whatsappstock_status"].ToString(),
                        });
                    }
                    values.branchofsproduct_list = getModuleList;

                }
                dt_datatable.Dispose();

                msSQL = "select b.product_name," +
                 "a.whatsapp_id,a.whatsappstock_status from otl_trn_branch2product a" +
                 " left join pmr_mst_tproduct b on b.product_gid=a.product_gid   " +
                 "  where  b.status !='2' and a.branch_gid='" + branch_gid + "' and a.whatsapp_id!='null' and a.whatsappstock_status='Y' order by a.whatsapp_id desc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList1 = new List<branchinsproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new branchinsproduct_list
                        {
                            product_name = dt["product_name"].ToString(),
                            whatsapp_id = dt["whatsapp_id"].ToString(),
                            whatsappstock_status = dt["whatsappstock_status"].ToString(),
                        });
                    }
                    values.branchinsproduct_list = getModuleList1;

                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/" + "DaGetwabproduct " + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public paymentgatewayconfiguration paymentgatewaycredentials()
        {
            paymentgatewayconfiguration getpaymentgatewaycredentials = new paymentgatewayconfiguration();
            try
            {


                msSQL = " SELECT CASE WHEN payment_gateway = 'RAZORPAY' THEN key1 ELSE NULL END AS key1,key2,CASE WHEN payment_gateway = 'STRIPE' THEN key1 ELSE NULL END AS key3,payment_gateway FROM adm_Mst_tcompany LIMIT 1; ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                   
                    getpaymentgatewaycredentials.razorpay_accname = objOdbcDataReader["key1"].ToString();
                    getpaymentgatewaycredentials.razorpay_accpwd = objOdbcDataReader["key2"].ToString();
                    getpaymentgatewaycredentials.stripe_key = objOdbcDataReader["key3"].ToString();
                    getpaymentgatewaycredentials.payment_gateway = objOdbcDataReader["payment_gateway"].ToString();
                    objOdbcDataReader.Close();
                }

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Configuration/" + "DaWhatsappproductpricemanagement.cs " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }

            return getpaymentgatewaycredentials;
        }
        public void Dawtbranchupdatestockstatus(branchproduct_list values, string branch_gid)
        {

            try
            {
                whatsappconfiguration2 getwhatsappcredentials = whatsappcredentials2(branch_gid);

                msSQL = "select whatsappstock_status from otl_trn_branch2product where whatsapp_id='" + values.whatsapp_id + "'  and branch_gid ='" + branch_gid + "' ";
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
                        msSQL = "update otl_trn_branch2product set whatsappstock_status='N' where whatsapp_id='" + values.whatsapp_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = "update otl_trn_branch2product set whatsappstock_status='Y' where whatsapp_id='" + values.whatsapp_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Stock Status Updated Sucessfully!!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occured while updating stock!!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Updating " + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaBranchupdatewhatsappstockstatus " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
            }
            catch (Exception ex)
            {

                values.message = "Error While Updating whats app product Stock Status";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "DaBranchupdatewhatsappstockstatus " + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void Daupdatemobileconfig(mobileconfig_list values)
        {
            try
            {
                msSQL = "update hrm_Mst_tbranch set manager_number ='" + values.manager_number + "',msgsend_manger ='" + values.msgsend_manger + "',owner_number='" + values.owner_number + "'," +
                    "msgsend_owner='" + values.msgsend_owner + "' where branch_gid='" + values.branch_gid + "'";
                mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnresult == 1)
                {
                    values.status = true;
                    values.message = "Updated Sucessfully!!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while updating !!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Updating " + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Daupdatemobileconfig " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while update mobile config!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While inserting details" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Update/" + "Daupdatemobileconfig " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
