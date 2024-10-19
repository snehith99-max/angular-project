using ems.pmr.Models;
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
using System.Data.OleDb;

using System.Text.RegularExpressions;
using OfficeOpenXml.Style;
using System.Drawing;
using Newtonsoft.Json;
using System.Net;
using RestSharp;

namespace ems.pmr.DataAccess
{
    public class DaPmrMstProduct
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string producttype_gid;

        string base_url, lsclient_id, api_key = string.Empty;

        string msEmployeeGID, lsemployee_gid, lsuser_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, mstax, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, status, mcGetGID, productgroup_code, msGetGID, maGetGID, productuomclass_code, mrGetGID;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, importcount;
        public void DaGetProductSummary(MdlPmrMstProduct values)
        {
            try
            {

                msSQL = " SELECT d.producttype_name,b.productgroup_name,b.productgroup_code,a.product_gid,a.customerproduct_code,a.product_desc,a.product_price, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,  CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
                    " c.productuomclass_code,e.productuom_code,c.productuomclass_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable,e.productuom_name,d.producttype_name as product_type,(case when a.status ='1' then 'Active' else 'Inactive' end) as Status," +
                    " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time  from pmr_mst_tproduct a " +
                    " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                    " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                    " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                    " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                    " left join adm_mst_tuser f on f.user_gid=a.created_by order by a.product_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            product_desc = dt["product_desc"].ToString(),

                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            stockable = dt["stockable"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),

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
                values.message = "Exception occured while Getting Product Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostProduct(string user_gid, product_list values)
        {

            try
            {

              

                msSQL = " select * from pmr_mst_tproduct where product_name= '" + values.product_name + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    //values.status = true;
                    values.message = "Product Name Already Exist";
                }
                else
                {

                    msGetGid = objcmnfunctions.GetMasterGID("PPTM");

                  

                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lsproduct_code = "PC" + "00" + lsCode;


                    msSQL = " insert into pmr_mst_tproduct (" +
                            " product_gid," +
                            " product_code," +
                            " product_name," +
                            " product_desc, " +
                            " productgroup_gid, " +
                            " productuomclass_gid, " +
                            " productuom_gid, " +
                            " mrp_price, " +
                            " cost_price, " +
                            " avg_lead_time, " +
                            " stockable, " +
                            " status, " +
                            " producttype_gid, " +
                            " purchasewarrenty_flag, " +
                            " expirytracking_flag, " +
                            " batch_flag," +
                            " serial_flag," +
                            " tax_gid, " +
                            " tax_gid1, " +
                            " customerproduct_code, " +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid + "'," +
                            "'" + lsproduct_code + "',";
                    if (values.product_name == null || values.product_name == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.product_name.Replace("'", "\\\'") + "',";
                    }
                    //"'" + values.product_desc.Replace("'", "\\'") + "'," +
                    if (!string.IsNullOrEmpty(values.product_desc) && values.product_desc.Contains("'"))
                    {
                        msSQL += "'" + values.product_desc.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.product_desc.Replace("'", "\\\'") + "', ";
                    }
                    msSQL += "'" + values.productgroup_name + "'," +
                             "'" + values.productuomclass_name + "'," +
                             "'" + values.productuom_name + "',";
                                   if (string.IsNullOrEmpty(values.mrp_price))
                                {
                                    msSQL += "'0.00',";
                                }
                                else
                                {
                                    msSQL += "'" + values.mrp_price + "',";
                                }
                    msSQL += "'" + values.cost_price + "'," +
                             "'" + values.avg_lead_time + "'," +
                             "'" + "Y" + "'," +
                             "'" + "1" + "'," +
                             "'" + values.producttype_name + "'," +
                             "'" + values.purchasewarrenty_flag + "'," +
                             "'" + values.expirytracking_flag + "'," +
                             "'" + values.batch_flag + "'," +
                             "'" + values.serial_flag + "',";

                    if (values.tax.Count > 0 && !string.IsNullOrEmpty(values.tax[0]))
                    {
                        msSQL += "'" + values.tax[0] + "',";
                    }
                    else
                    {
                        msSQL += "'0.00',";
                    }

                    if (values.tax.Count > 1 && !string.IsNullOrEmpty(values.tax[1]))
                    {
                        msSQL += "'" + values.tax[1] + "',";
                    }
                    else
                    {
                        msSQL += "'0.00',";
                    }
                    msSQL +=   "'" + values.sku + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    for (int i = 0; i < values.tax.Count; i++)
                    {
                        string taxId = values.tax[i].ToString();

                        // Retrieve the necessary tax information
                        msSQL = "SELECT taxsegment_gid, tax_prefix, percentage FROM acp_mst_ttax WHERE tax_gid='" + taxId + "'";
                        var dataTable = objdbconn.GetDataTable(msSQL);
                        if (dataTable.Rows.Count > 0)
                        {
                            var row = dataTable.Rows[0];
                            string taxSegmentGid = row["taxsegment_gid"].ToString();
                            string taxName = row["tax_prefix"].ToString();
                            string taxPercentage = row["percentage"].ToString();

                                          
                            
                            mstax = objcmnfunctions.GetMasterGID("TS2P");


                            msSQL = " insert into acp_mst_ttaxsegment2product (" +
                                   " taxsegment2product_gid," +
                                   " taxsegment_gid," +
                                   " tax_gid," +
                                   " tax_name," +
                                   " tax_percentage, " +
                                   " product_gid, " +                                               
                                   " created_by, " +
                                   " created_date)" +
                                   " values(" +
                           " '" + mstax + "'," +                           
                           " '" + taxSegmentGid + "'," +                           
                           "'" + taxId + "'," +
                           "'" + taxName + "'," +
                           "'" + taxPercentage + "'," +
                           "'" + msGetGid + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);




                        }
                    }

                    if (mnResult != 0)
                    {
                        msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                        string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                        // code by snehith to push  product in mintsoft
                        if (mintsoft_flag == "Y")
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

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(base_url);
                            var request = new RestRequest("/api/Product/",Method.PUT);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("ms-apikey", api_key);
                            var jsonPayload = new
                            {
                                SKU = values.sku,
                                Name = values.product_name.Replace("'", "\\'"),
                                Description = values.product_desc.Replace("'", "\\\'"),
                                Price = values.cost_price,
                                ClientId = lsclient_id
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
                        values.message = "Error While Adding Product ";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
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
        public void DaPmrMstProductUpdate(string user_gid, product_list values)
        {
            try
            {

                

                msSQL = " select product_name from pmr_mst_tproduct  " +
                      " where LOWER(product_name) = '" + values.product_name.Replace("'", "''").ToLower() + "'" +
                      " and product_gid !='" + values.product_gid + "'";
                string lsProductName = objdbconn.GetExecuteScalar(msSQL);
                if (lsProductName != "" && lsProductName != null)
                {
                    values.status = false;
                    values.message = "Product Name Already Exist";
                    return;
                }
                else
                {


                    msSQL = " SELECT productgroup_gid FROM pmr_mst_tproductgroup WHERE productgroup_name='" + values.productgroup_name + "' ";
                    string lsproductgroup_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " SELECT producttype_gid FROM pmr_mst_tproducttype WHERE producttype_name='" + values.producttype_name + "' ";
                    string lsproducttype_gid = objdbconn.GetExecuteScalar(msSQL);



                    msSQL = " SELECT productuom_gid FROM pmr_mst_tproductuom WHERE productuom_name='" + values.productuom_name + "' ";
                    string lsproductuom_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " SELECT tax_gid FROM acp_mst_ttax WHERE tax_prefix='" + values.tax + "' ";
                    string lstax = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " SELECT tax_gid FROM acp_mst_ttax WHERE tax_prefix='" + values.tax1 + "' ";
                    string lstax1 = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " update  pmr_mst_tproduct  set " +
                            " product_name = '" + values.product_name + "'," +
                            " product_code = '" + values.product_code + "'," +
                            " product_desc = '" + values.product_desc.Replace("'", "\\'") + "'," +
                            " currency_code = '" + values.currency_code + "'," +
                            " productgroup_gid = '" + lsproductgroup_gid + "'," +
                            " producttype_gid = '" + lsproducttype_gid + "'," +

                            " productuom_gid = '" + lsproductuom_gid + "'," +
                            " mrp_price = '" + values.mrp_price + "'," +
                            " cost_price = '" + values.cost_price + "'," +
                            " avg_lead_time = '" + values.avg_lead_time + "'," +
                            " purchasewarrenty_flag = '" + values.purchasewarrenty_flag + "'," +
                            " expirytracking_flag = '" + values.expirytracking_flag + "'," +
                            " batch_flag = '" + values.batch_flag + "'," +
                            " serial_flag = '" + values.serial_flag + "'," +
                              " customerproduct_code = '" + values.sku + "',";
                               //"tax_gid = '" + lstax + "'," +
                               //"tax_gid1 = '" + lstax1 + "'," +
                                if (values.tax.Count > 0 && !string.IsNullOrEmpty(values.tax[0]))
                    {
                        msSQL += "tax_gid = '" + values.tax[0] + "',";
                    }
                    else
                    {
                        msSQL += " tax_gid = '0.00',";
                    }

                    if (values.tax.Count > 1 && !string.IsNullOrEmpty(values.tax[1]))
                    {
                        msSQL += "tax_gid1 = '" + values.tax[1] + "',";
                    }
                    else
                    {
                        msSQL += " tax_gid1 = '0.00',";
                    }
                    msSQL += " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where product_gid='" + values.product_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    for (int i = 0; i < values.tax.Count; i++)
                    {
                        string taxId = values.tax[i].ToString();

                        // Retrieve the necessary tax information
                        msSQL = "SELECT taxsegment_gid, tax_prefix, percentage FROM acp_mst_ttax WHERE tax_gid='" + taxId + "'";
                        var dataTable = objdbconn.GetDataTable(msSQL);
                        if (dataTable.Rows.Count > 0)
                        {
                            var row = dataTable.Rows[0];
                            string taxSegmentGid = row["taxsegment_gid"].ToString();
                            string taxName = row["tax_prefix"].ToString();
                            string taxPercentage = row["percentage"].ToString();                                          


                            msSQL = "UPDATE acp_mst_ttaxsegment2product SET " +
                                    " taxsegment_gid = '" + taxSegmentGid + "'," +
                                    " tax_gid = '" + taxId + "'," +
                                    " tax_name = '" + taxName + "'," +
                                    " tax_percentage = '" + taxPercentage + "'," +
                                    " created_by = '" + user_gid + "'," +
                                    " created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                    " WHERE product_gid = '" + values.product_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);




                        }
                    }



                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                        string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                        // code by snehith to push  product in mintsoft
                        if (mintsoft_flag == "Y")
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
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }
        public void DaGetProducttype(MdlPmrMstProduct values)
        {
            try
            {

                msSQL = " Select producttype_name,producttype_gid  " +
                    " from pmr_mst_tproducttype";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProducttype>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProducttype
                        {
                            producttype_name = dt["producttype_name"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                        });
                        values.GetProducttype = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Product type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetProductGroup(MdlPmrMstProduct values)
        {
            try
            {

                msSQL = " Select productgroup_gid, productgroup_name from pmr_mst_tproductgroup  " +
                    " order by productgroup_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductGroup>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductGroup
                        {
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.GetProductGroup = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetProductUnitclass(MdlPmrMstProduct values)
        {
            try
            {

                msSQL = " Select productuomclass_gid, productuomclass_code, productuomclass_name  " +
                " from pmr_mst_tproductuomclass order by productuomclass_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductUnitclass>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductUnitclass
                        {
                            productuomclass_gid = dt["productuomclass_gid"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                        });
                        values.GetProductUnitclass = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product unit class!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        //public void DaGetProductUnit(string productuomclass_gid, MdlPmrMstProduct values)
        //{
        //    try
        //    {

        //        msSQL = " select productuom_name,productuom_gid from pmr_mst_tproductuom a left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid  " +
        //                " where a.productuomclass_gid='" + productuomclass_gid + "' order by a.sequence_level ";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getModuleList = new List<GetProductUnit>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new GetProductUnit
        //                {
        //                    productuom_name = dt["productuom_name"].ToString(),
        //                    productuom_gid = dt["productuom_gid"].ToString(),

        //                });
        //                values.GetProductUnit = getModuleList;
        //            }
        //        }
        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Getting Product Unit!";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
        //            $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
        //        ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
        //        msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
        //        DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
        //    }

        //}

        public void DaGetOnChangeProductUnitClass(string productuomclass_gid, MdlPmrMstProduct values)
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
                values.message = "Exception occured while changing Product unit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetDeleteProductdetails(string product_gid, product_list values)
        {
            try
            {
                bool purchaserequisition_flag = false, materialrequisition_flag = false, salesorder_flag = false, invoice_flag = false, receivequotation_flag = false, salesenquiry_flag = false;
                msSQL = " select product_gid from pmr_trn_tpurchaserequisitiondtl " +
                        " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)
                {
                    purchaserequisition_flag = true;
                }
                msSQL = " select product_gid from ims_trn_tmaterialrequisitiondtl " +
                        " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)
                {
                    materialrequisition_flag = true;
                }
                msSQL = " select product_gid from pmr_trn_tpurchaserequisitiondtl " +
                       " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)
                {
                    salesorder_flag = true;
                }
                msSQL = " select product_gid from smr_trn_tinvoicedtl " +
                        " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)
                {
                    invoice_flag = true;
                }

                msSQL = " select product_gid from smr_trn_tsalesenquirydtl " +
                        " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)
                {
                    salesenquiry_flag = true;
                }
                msSQL = " select product_gid from smr_trn_treceivequotationdtl " +
                        " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
                if (objOdbcDataReader.HasRows)
                {
                    receivequotation_flag = true;
                }
                if (!(purchaserequisition_flag || materialrequisition_flag || salesorder_flag || invoice_flag || receivequotation_flag || salesenquiry_flag))
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
                    values.message = " Please delete the corresponding Material Request assigned for this category ";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Product Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetViewProductSummary(string product_gid, MdlPmrMstProduct values)
        {
            try
            {


                msSQL = "  select CASE WHEN a.batch_flag = 'N' THEN 'NO' ELSE 'YES' END AS batch_flag,CASE WHEN a.serial_flag = 'N' THEN 'NO' ELSE 'YES' END AS serial_flag,y.tax_prefix as tax_prefix1," +
                    " CASE WHEN a.purchasewarrenty_flag = 'N' THEN 'NO' ELSE 'YES' END AS purchasewarrenty_flag,CASE WHEN a.expirytracking_flag = 'N' THEN 'NO' ELSE 'YES' END AS expirytracking_flag," +
                    "  a.product_desc,a.avg_lead_time,a.mrp_price,z.tax_prefix , e.producttype_name,a.cost_price,b.currency_code,a.product_gid,c.productgroup_name,a.product_name,f.productuomclass_name,a.product_code,d.productuom_name, " +
                    "  a.customerproduct_code from pmr_mst_tproduct a " +
                    "  left join crm_trn_tcurrencyexchange b on b.currency_code=a.currency_code" +
                    "  left  join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid" +
                    "  left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid" +
                    "  left  join pmr_mst_tproducttype e on a.producttype_gid=e.producttype_gid" +
                     "  left  join acp_mst_ttax z on a.tax_gid=z.tax_gid" +
                     "  left  join acp_mst_ttax y on a.tax_gid1=y.tax_gid " +
                    "  left  join pmr_mst_tproductuomclass f on a.productuomclass_gid=f.productuomclass_gid" +
                    "  where a.product_gid='" + product_gid + "' group by a.product_gid";

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
                            //batch_flag = dt["batch_flag"].ToString(),
                            //serial_flag = dt["serial_flag"].ToString(),
                            //serialtracking_flag = dt["serialtracking_flag"].ToString(),
                            //expirytracking_flag = dt["expirytracking_flag"].ToString(),
                            //purchasewarrenty_flag = dt["purchasewarrenty_flag"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            avg_lead_time = dt["avg_lead_time"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            tax = dt["tax_prefix"].ToString(),
                            tax1 = dt["tax_prefix1"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),

                        });
                        values.GetViewProductSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while viewing Product Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetEditProductSummary(string product_gid, MdlPmrMstProduct values)
        {
            try
            {

                msSQL = "  select  a.batch_flag,a.serial_flag, a.purchasewarrenty_flag, y.tax_prefix ,z.tax_prefix as tax_prefix1,a.expirytracking_flag,a.product_desc,a.avg_lead_time,a.customerproduct_code,a.tax_gid,a.tax_gid1," +
                        "  a.mrp_price,a.cost_price,a.product_gid,a.product_name,a.product_code,b.productgroup_gid,b.productgroup_name,c.productuomclass_gid,c.productuomclass_name," +
                        "  d.producttype_gid,d.producttype_name,e.productuom_gid,e.productuom_name from pmr_mst_tproduct a " +
                        " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
                        " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid" +
                        " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                        " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid" +
                        " left join acp_mst_ttax y on a.tax_gid = y.tax_gid" +
                        " left join acp_mst_ttax z on a.tax_gid1 = z.tax_gid" +
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
                            sku = dt["customerproduct_code"].ToString(),
                            tax = dt["tax_prefix"].ToString(),
                            tax1 = dt["tax_prefix1"].ToString(),
                            tax_gid1 = dt["tax_gid1"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),



                        });
                        values.GetEditProductSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Product Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaproductImportExcel(HttpRequest httpRequest, string user_gid, result objResult, product_list values)
        {
            try
            {


                string lscompany_code;
                string excelRange, endRange;
                int rowCount, columnCount;


                
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
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    string status;
                    status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                    file.Close();
                    ms.Close();
                //bool status1;


                //status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Lead Import/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                //ms.Close();

                // Connect to the storage account
                //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"].ToString());
                //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                //CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["blob_containername"].ToString());

                // Get a reference to the blob
                //CloudBlockBlob blockBlob = container.GetBlockBlobReference(lscompany_code + "/" + "Lead Import/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension);
                ////string path_url = lscompany_code + "/" + "Lead Import/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension;


                // Download the blob's contents and read Excel file
                //using (MemoryStream memoryStream = new MemoryStream())
                //{
                //    // await blockBlob.DownloadToStreamAsync(memoryStream);

                //    blockBlob.DownloadToStream(memoryStream);
                //    memoryStream.Seek(0, SeekOrigin.Begin);
                //    memoryStream.Position = 0;
                //    // Load Excel package from the memory stream
                //    //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                //    using (ExcelPackage package = new ExcelPackage(memoryStream))
                //    {
                //        ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"]; // worksheet name
                //                                                                          // Remove the first row
                //        worksheet.DeleteRow(1);

                //        // Convert Excel data to array list format
                //        List<List<string>> excelData = new List<List<string>>();

                //        for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                //        {
                //            List<string> rowData = new List<string>();
                //            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                //            {
                //                var cellValue = worksheet.Cells[row, col].Value?.ToString();
                //                rowData.Add(cellValue);
                //            }

                //            string producttype_name = rowData[0];
                //            string productgroup_name = rowData[1];
                //            string product_code = rowData[2];
                //            string hsn_code = rowData[3];
                //            string product_name = rowData[4];
                //            string product_des = rowData[5];
                //            string productuomclass_name = rowData[6];
                //            string serialtracking_flag = rowData[7];
                //            string warrentytracking_flag = rowData[8];
                //            string expirytracking_flag = rowData[9];
                //            string cost_price = rowData[10];
                //string country_name = rowData[11];
                //string source_name = rowData[12];
                //string customer_type = rowData[13];
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

                                importcount = 0;

                                char[] charsToReplace = { '*', ' ', '/', '@', '$', '#', '!', '^', '%', '(', ')', '\'' };

                                // Get the header names from the CSV file
                                List<string> headers = dataTable.Columns.Cast<DataColumn>().Select(column =>
                                    string.Join("", column.ColumnName.Split(charsToReplace, StringSplitOptions.RemoveEmptyEntries))
                                        .ToLower()).ToList();
                                if (dataTable.Rows.Count == 0)
                                {
                                    values.message = "No data found ";
                                    values.status = false;
                                    return;
                                }

                                foreach (DataRow dt_product in dataTable.Rows)
                                {

                                    string producttype_name = dt_product["ProductType"].ToString();
                                    string productgroup_name = dt_product["ProductGroup"].ToString();
                                    string product_code = dt_product["ProductCode"].ToString();
                                    string hsn_code = dt_product["SKU"].ToString();
                                    string product_name = dt_product["Product"].ToString();
                                    string product_des = dt_product["Description"].ToString();
                                    string productuomclass_name = dt_product["Units"].ToString();
                                    string cost_price = dt_product["CostPrice"].ToString();
                                    string tax_name = dt_product["TaxName"].ToString();
                                    string tax_name2 = dt_product["TaxName2"].ToString();


                                    if (productgroup_name != null && productgroup_name != "" && product_name != null && product_name != "" &&
                                        cost_price != null && cost_price != "" && hsn_code != null && hsn_code != "" &&
                                        productuomclass_name != null && productuomclass_name != "" && tax_name != null && tax_name != "" && tax_name2 != null && tax_name2 != ""
                                        && producttype_name != null && producttype_name != "")
                                    {


                                        msSQL = " Select producttype_gid from pmr_mst_tproducttype " +
                                                " where producttype_name = '" + producttype_name + "'";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objOdbcDataReader.HasRows == true)
                                        {
                                            objOdbcDataReader.Read();
                                            producttype_gid = objOdbcDataReader["producttype_gid"].ToString();
                                            objOdbcDataReader.Close();
                                        }
                                        msSQL = " Select productgroup_gid , productgroup_code from pmr_mst_tproductgroup " +
                                        " where productgroup_name = '" + productgroup_name + "'";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                        if (objOdbcDataReader.HasRows == true)
                                        {
                                            objOdbcDataReader.Read();
                                            mcGetGID = objOdbcDataReader["productgroup_gid"].ToString();
                                            productgroup_code = objOdbcDataReader["productgroup_code"].ToString();
                                            objOdbcDataReader.Close();
                                        }
                                        else
                                        {
                                            mcGetGID = objcmnfunctions.GetMasterGID("PPGM");
                                            productgroup_code = objcmnfunctions.GetMasterGID("PPGM");

                                            msSQL = "insert into pmr_mst_tproductgroup (" +
                                                        "productgroup_gid, " +
                                                        "productgroup_code, " +
                                                        "productgroup_name) " +
                                                        "values (" +
                                                        "'" + mcGetGID + "', " +
                                                        "'" + productgroup_code + "', " +
                                                        "'" + productgroup_name + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        }
                                        msSQL = " Select productuomclass_gid from pmr_mst_tproductuomclass " +
                                                " where productuomclass_name = '" + productuomclass_name + "'";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objOdbcDataReader.HasRows == true)
                                        {
                                            objOdbcDataReader.Read();
                                            maGetGID = objOdbcDataReader["productuomclass_gid"].ToString();
                                            productuomclass_code = objOdbcDataReader["productuomclass_gid"].ToString();

                                            objOdbcDataReader.Close();
                                        }
                                        else
                                        {
                                            maGetGID = objcmnfunctions.GetMasterGID("PUCM");
                                            productuomclass_code = objcmnfunctions.GetMasterGID("PUCM");

                                            msSQL = "insert into pmr_mst_tproductuomclass (" +
                                                    "productuomclass_gid, " +
                                                    "productuomclass_code, " +
                                                    "productuomclass_name) " +
                                                    "values(" +
                                                    "'" + maGetGID + "', " +
                                                    "'" + productuomclass_code + "', " +
                                                    "'" + productuomclass_name + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }

                                        msSQL = " Select productuom_gid from pmr_mst_tproductuom " +
                                                " where productuom_name = '" + productuomclass_name + "'";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objOdbcDataReader.HasRows == true)
                                        {
                                            objOdbcDataReader.Read();
                                            msGetGID = objOdbcDataReader["productuom_gid"].ToString();
                                            objOdbcDataReader.Close();
                                        }
                                        else
                                        {
                                            msGetGID = objcmnfunctions.GetMasterGID("PPMM");

                                            msSQL = "insert into pmr_mst_tproductuom (" +
                                                    "productuom_gid, " +
                                                    "productuom_code, " +
                                                    "productuom_name, " +
                                                    "productuomclass_gid) " +
                                                    "values(" +
                                                    "'" + msGetGID + "', " +
                                                    "'" + productuomclass_name + "', " +
                                                    "'" + productuomclass_name + "', " +
                                                    "'" + maGetGID + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }

                                        //insertion in table

                                        //msSQL = " Select product_gid from pmr_mst_tproduct " +
                                        //               " where product_code =  '" + exproductcode + "' and product_name = '" + exproduct + "'";
                                        //objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        //if (objOdbcDataReader.HasRows == true)
                                        //{
                                        //    objOdbcDataReader.Close();
                                        //}
                                        //else
                                        //{
                                        //    mrGetGID = objcmnfunctions.GetMasterGID("PPTM");
                                        //}


                                        msSQL = "select tax_gid from acp_mst_ttax where tax_name = '" + tax_name + "'";
                                        string tax1_gid = objdbconn.GetExecuteScalar(msSQL);
                                        msSQL = "select tax_gid from acp_mst_ttax where tax_name = '" + tax_name2 + "'";
                                        string tax2_gid = objdbconn.GetExecuteScalar(msSQL);
                                        mrGetGID = objcmnfunctions.GetMasterGID("PPTM");

                                        msSQL = "insert into pmr_mst_tproduct (" +
                                               "product_gid, " +
                                               "productgroup_gid, " +
                                               "productuom_gid, " +
                                               "productuomclass_gid, " +
                                               "producttype_gid, " +
                                               "product_code, " +
                                               "customerproduct_code, " +
                                               "product_name, " +
                                               "producttype_name, " +
                                               "product_desc, " +
                                               "status, " +
                                               "cost_price, " +
                                               "serial_flag, " +
                                               "tax_gid, " +
                                               "tax_gid1, " +
                                               " created_by, " +
                                               " created_date) " +
                                               "values (" +
                                               "'" + mrGetGID + "', " +
                                               "'" + mcGetGID + "', " +
                                               "'" + msGetGID + "'," +
                                               "'" + maGetGID + "'," +
                                               "'" + producttype_gid + "', " +
                                               "'" + product_code + "', " +
                                               "'" + hsn_code + "', " +
                                               "'" + product_name + "', " +
                                               "'" + producttype_name + "', " +
                                               "'" + product_des + "', " +
                                               "'1', " +
                                               "'" + cost_price + "', " +
                                               "' Y ', " +
                                               "'" + tax1_gid + "', " +
                                               "'" + tax2_gid + "', " +
                                               "'" + user_gid + "'," +
                                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        if (mnResult != 0)
                                        {
                                            values.status = true;
                                            values.message = "Product Imported Successfully";
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        string msGETLOGGID = objcmnfunctions.GetMasterGID("MPEL");
                                        msSQL = " insert into pmr_tmp_tproducttemplog (" +
                                                        " producttemplog_gid, " +
                                                        " uploadexcellog_gid, " +
                                                        " productgroup," +
                                                        " producttype," +
                                                        " product_name, " +
                                                        " productuom_name, " +
                                                        " cost_price " +
                                                        " ) values ( " +
                                                        "'" + msGETLOGGID + "'," +
                                                        "'" + msdocument_gid + "'," +
                                                        "'" + productgroup_name.Trim() + "'," +
                                                        "'" + producttype_name.Trim() + "'," +
                                                        "'" + product_name.Trim() + "'," +
                                                        "'" + productuomclass_name.Trim() + "'," +
                                                        "'" + cost_price.Trim() + "')";

                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        if (mnResult == 1)
                                        {
                                            values.status = false;
                                            values.message = "Excel Import Un Successfull! Kindly Check the Log For Details";
                                        }
                                    }


                                }
                                
                                
                            }

                        }
                    }
                }

                catch (Exception ex)
                {
                    values.status = false;
                    values.message = ex.Message.ToString();
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Imported Successfully";
                }

                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Product template";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetImportLog(MdlPmrMstProduct values)
        {
            try
            {

                msSQL = " SELECT producttemplog_gid, uploadexcellog_gid, " +
                        " CASE WHEN productuomclass_name IS NULL OR productuomclass_name = '' THEN '---/---' ELSE productuomclass_name END AS productuomclass_name," +
                        " CASE WHEN producttype IS NULL OR producttype = '' THEN '---/---' ELSE producttype END AS producttype, " +
                        " CASE WHEN productuom_name IS NULL OR productuom_name = '' THEN '---/---' ELSE productuom_name END AS productuom_name, " +
                        " CASE WHEN productgroup IS NULL OR productgroup = '' THEN '---/---' ELSE productgroup END AS productgroup, " +
                        " CASE WHEN product_name IS NULL OR product_name = '' THEN '---/---' ELSE product_name END AS product_name, " +
                        " CASE WHEN cost_price IS NULL OR cost_price = '' THEN '---/---' ELSE cost_price END AS cost_price, " +
                        " CASE WHEN mrp IS NULL OR mrp = '' THEN '---/---' ELSE mrp END AS mrp " +
                        " FROM pmr_tmp_tproducttemplog ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list
                        {
                            log_id = dt["producttemplog_gid"].ToString(),
                            document_id = dt["uploadexcellog_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            producttype_name = dt["producttype"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp"].ToString(),
                        });
                        values.product_list = getModuleList;
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaDeleteLog(string log_id, product_list values)
        {
            msSQL = " delete from pmr_tmp_tproducttemplog where producttemplog_gid = '" + log_id + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Log Deleted Successfully !!";
            }
            else
            {
                values.status = false;
                values.message = "Error While Deleting Record";
            }
        }
        public void DaGetProductReportExport(MdlPmrMstProduct values)
        {
            try
            {
                msSQL = " SELECT b.productgroup_name as ProductGroupName,a.product_code as ProductCode," +
                    " a.customerproduct_code as SKU, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as ProductName," +
                    " e.productuom_name as Unit,a.cost_price as CostPrice,d.producttype_name as ProductType,g.tax_name as TaxName ,h.tax_name as TaxName2 " +
                    " from pmr_mst_tproduct a " +
                    " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                    " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                    " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                    " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                    " left join acp_mst_ttax g on g.tax_gid = a.tax_gid  left join acp_mst_ttax h on h.tax_gid = a.tax_gid1 " +
                    " left join adm_mst_tuser f on f.user_gid=a.created_by order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                string lscompany_code = string.Empty;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Product Report");
                    try
                    {
                        msSQL = " select company_code from adm_mst_tcompany";
                        lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                        string lspath = ConfigurationManager.AppSettings["exportexcelfile"] + "/prodcut/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                        if (!System.IO.Directory.Exists(lspath))
                            System.IO.Directory.CreateDirectory(lspath);

                        string lsname2 = "Product_Report" + DateTime.Now.ToString("(dd-MM-yyyy HH-mm-ss)") + ".xlsx";
                        string lspath1 = ConfigurationManager.AppSettings["exportexcelfile"] + "/prodcut/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsname2;

                        workSheet.Cells[1, 1].LoadFromDataTable(dt_datatable, true);
                        FileInfo file = new FileInfo(lspath1);
                        using (var range = workSheet.Cells[1, 1, 1, 9])
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
                                lsname2 = lsname2,
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
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Exporting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPostTaxSegment2Product(string user_gid, TaxSegmentSummary_list values)
        {
            try
            {
                msSQL = " select taxsegment_name from acp_mst_ttaxsegment where taxsegment_name= '" + values.taxsegment_name + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Tax Segment Name Already Exist";
                    return;
                }

                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("TXSG");
                    msSQL = " insert into acp_mst_ttaxsegment(" +
                            " taxsegment_gid, " +
                            " taxsegment_code," +
                            " taxsegment_name," +
                            " taxsegment_description," +
                            " active_flag," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                             " '" + msGetGid + "'," +
                             " '" + values.taxsegment_code + "',";
                    if (values.taxsegment_name == null || values.taxsegment_name == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.taxsegment_name.Replace("'", "\\'") + "',";
                    }
                    msSQL += "'" + values.taxsegment_description + "'," +
                        "'" + values.active_flag + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Tax Segment Added Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Tax Segment";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetProductUnit(MdlPmrMstProduct values)
        {
            try
            {

                msSQL = " select productuom_name,productuom_gid from pmr_mst_tproductuom a left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid  " +
                        " order by a.sequence_level ";
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
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product UOM Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void Dagettaxdropdown(MdlPmrMstProduct values)
        {
            try
            {

                msSQL = " Select tax_prefix,tax_gid  " +
                    " from acp_mst_ttax where active_flag='Y' and reference_type='Vendor' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<taxdtl_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new taxdtl_list
                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_prefix"].ToString(),
                        });
                        values.taxdtl_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetcustomerActive(string product_gid, MdlPmrMstProduct values)
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

        public void DaGetcustomerInactive(string product_gid, MdlPmrMstProduct values)
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
    }
}
        














