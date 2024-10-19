using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using System.Data.OleDb;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Runtime.Remoting;
using System.Web.Http.Results;
using CrystalDecisions.Shared.Json;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;


namespace ems.sales.DataAccess
{
    public class DaSmrMstProduct
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        private OdbcDataReader objOdbcDataReader;

        DataTable dt_datatable;
        string exclproducttypecode, lsproduct_code, mcGetGID, final_path, msGetGID, maGetGID, mrGetGID, msGetGid, msGETLOGGID;
        int mnResult;

        string base_url, api_key, lsclient_id = string.Empty;


        public void DaGetSalesProductSummary(MdlSmrMstProduct values)
        {
            try
            {

                msSQL = " SELECT d.producttype_name,b.productgroup_name, a.product_desc,b.productgroup_code,a.product_gid, a.product_price, a.cost_price, format(a.mrp_price,2) as mrp_price , a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,  CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
                    " c.productuomclass_code,e.productuom_code,c.productuomclass_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable,e.productuom_name,d.producttype_name as product_type,a.product_image, " +
                    " CASE  WHEN a.status = '1' THEN 'Active' WHEN a.status IS NULL OR a.status = '' OR a.status = '2' THEN 'InActive' END AS status," +
                    " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time, a.customerproduct_code  from pmr_mst_tproduct a " +
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
                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            productuomclass_code = dt["productuomclass_code"].ToString(),
                            productuom_code = dt["productuom_code"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            stockable = dt["stockable"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_type = dt["product_type"].ToString(),
                            Status = dt["status"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            lead_time = dt["lead_time"].ToString(),
                            product_image = dt["product_image"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),


                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // Add Event

        public void DaPostSalesProduct(string user_gid, product_list values)
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

                msSQL = " select * from pmr_mst_tproduct where product_name= '" + values.product_name.Replace("'","\\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    values.message = "Product Name Already Exist";
                }

                else
                {

                    msGetGid = objcmnfunctions.GetMasterGID("PPTM");
                    //string msGetGid2 = objcmnfunctions.GetMasterGID("VPDC");

                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lsproduct_code = "PC" + "00" + lsCode;

                    msSQL = "select producttype_name from pmr_mst_tproducttype where producttype_gid = '" + values.producttype_name.Replace("'", "\\\'") + "'";
                    string lsproducttype = objdbconn.GetExecuteScalar(msSQL);


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
                            " customerproduct_code, " +
                            " hsn_number, " +
                            " avg_lead_time, " +
                            " stockable, " +
                            " status, " +
                            " producttype_name, " +
                           " producttype_gid, " +
                              " tax_gid, " +
                            " purchasewarrenty_flag, " +
                            " expirytracking_flag, " +
                            " batch_flag," +
                            " serial_flag," +
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
                    if (values.product_desc == null || values.product_desc == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.product_desc.Replace("'", "\\\'") + "',";
                    }
                    msSQL += "'" + values.productgroup_name + "'," +
                     "'" + values.productuomclass_name + "'," +
                     "'" + values.productuom_name + "'," +
                    "'" + values.mrp_price + "',";
                    if (string.IsNullOrEmpty(values.cost_price))
                    {
                        msSQL += "0.00,";
                    }
                    else
                    {
                        msSQL += "'" + values.cost_price + "',";
                    }

                    msSQL += "'" + (String.IsNullOrEmpty(values.sku) ? values.sku : values.sku.Replace("'", "\\\'")) + "'," +
                       "'" + (String.IsNullOrEmpty(values.sku) ? values.sku : values.sku.Replace("'","\\\'")) + "'," +
                      "'" + values.avg_lead_time + "'," +
                      "'" + "Y" + "'," +
                      "'" + "1" + "'," +
                      "'" + (String.IsNullOrEmpty(lsproducttype) ? lsproducttype : lsproducttype.Replace("'","\\\'")) + "'," +
                       "'" + values.producttype_name + "'," +
                       "'" + values.tax + "'," +
                     "'" + values.purchasewarrenty_flag + "'," +
                     "'" + values.expirytracking_flag + "'," +
                     "'" + values.batch_flag + "'," +
                     "'" + values.serial_flag + "'," +
                     "'" + user_gid + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

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
                            request.AddHeader("ms-apikey", api_key);
                            var jsonPayload = new
                            {
                                SKU = values.sku.Replace("'", "\\\'"),
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
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                                     "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        values.status = false;
                        values.message = "Please Enter All Mandatory Fields";
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Product  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
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

        //product type dropdown
        public void DaGetProducttype(MdlSmrMstProduct values)
        {
            try
            {

                msSQL = " Select producttype_name,producttype_gid  " +
                    " from pmr_mst_tproducttype ";
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
                values.message = "Exception occured while Getting Product Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void Dagettaxdropdown(MdlSmrMstProduct values)
        {
            try
            {

                msSQL = " Select tax_prefix,tax_gid  " +
                    " from acp_mst_ttax where active_flag='Y' and reference_type='Customer' ";
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

        //product group dropdown
        public void DaGetProductGroup(MdlSmrMstProduct values)
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
                values.message = "Exception occured while Getting Prodcut Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        //product unit class
        public void DaGetProductUnitclass(MdlSmrMstProduct values)
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
                values.message = "Exception occured while Getting Product UOM Class Name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        //product unit
        public void DaGetProductUnit( MdlSmrMstProduct values)
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

        //on change
        public void DaGetOnChangeProductUnitClass(string productuomclass_gid, MdlSmrMstProduct values)
        {
            try
            {

                msSQL = " select productuom_name,productuom_gid from pmr_mst_tproductuom a left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid  " +
                        " where a.productuomclass_gid='" + productuomclass_gid + "' order by a.sequence_level ";
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
                values.message = "Exception occured while Getting Product Unit Class Name  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaSmrMstProductUpdate(string user_gid, product_list values)
        {
            try
            {
                string lsmrpprice = "";
                string priceSegmentGid = "";

                msSQL = " select * from smr_trn_tminsoftconfig;";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    base_url = objOdbcDataReader["base_url"].ToString();
                    api_key = objOdbcDataReader["api_key"].ToString();
                }
                objOdbcDataReader.Close();

                msSQL = " select product_name from pmr_mst_tproduct  " +
      " where LOWER(product_name) = '" + values.product_name.Replace("'", "\\\'").ToLower() + "'" +
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

                    msSQL = " SELECT productgroup_gid FROM pmr_mst_tproductgroup WHERE productgroup_name='" + values.productgroupname.Replace("'", "\\\'") + "' ";
                    string lsproductgroup_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " SELECT mrp_price FROM pmr_mst_tproduct WHERE product_gid='" + values.product_gid + "' ";
                    lsmrpprice = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " SELECT tax_gid FROM acp_mst_ttax WHERE tax_prefix='" + values.tax.Replace("'", "\\\'") + "' ";
                    string lstax = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " SELECT producttype_gid FROM pmr_mst_tproducttype WHERE producttype_name='" + values.producttypename.Replace("'", "\\\'") + "' ";
                    string lsproducttype_gid = objdbconn.GetExecuteScalar(msSQL);

                    //msSQL = " SELECT productuomclass_gid FROM pmr_mst_tproductuomclass WHERE productuomclass_gid='" + values.productuomclassname + "' ";
                    //string lsproductuomclass_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " SELECT productuom_gid FROM pmr_mst_tproductuom WHERE productuom_name='" + values.productuomname.Replace("'", "\\\'") + "' ";
                    string lsproductuom_gid = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " update  pmr_mst_tproduct  set " +
                  " product_name = '" + (String.IsNullOrEmpty(values.product_name) ? values.product_name : values.product_name.Replace("'","\\\'")) + "'," +
                  " product_code = '" + values.product_code + "'," +
                  " product_desc = '" + (String.IsNullOrEmpty(values.product_desc) ? values.product_desc : values.product_desc.Replace("'","\\\'")) + "'," +
                  " currency_code = '" + values.currency_code + "'," +
                  " productgroup_gid = '" + lsproductgroup_gid + "'," +
                  " productuom_gid = '" + lsproductuom_gid + "'," +
                   " producttype_gid = '" + lsproducttype_gid + "'," +
                    " producttype_name = '" + values.producttype_name + "'," +
                  " mrp_price = '" + values.mrp_price.Replace(",","") + "'," +
                  " hsn_number = '" + (String.IsNullOrEmpty(values.sku) ? values.sku : values.sku.Replace("'", "\\\'")) + "'," +
                  " cost_price = '" + values.cost_price.Replace(",", "") + "'," +
                  " avg_lead_time = '" + values.avg_lead_time + "'," +
                  " purchasewarrenty_flag = '" + values.purchasewarrenty_flag + "'," +
                  " expirytracking_flag = '" + values.expirytracking_flag + "'," +
                  " batch_flag = '" + values.batch_flag + "'," +
                  " serial_flag = '" + values.serial_flag + "'," +
                  " customerproduct_code = '" + (String.IsNullOrEmpty(values.sku) ? values.sku : values.sku.Replace("'", "\\\'")) + "'," +
                  "tax_gid = '" + lstax + "'," +
                  " updated_by = '" + user_gid + "'," +
                  " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where product_gid='" + values.product_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (lsmrpprice != values.mrp_price)
                    {
                        msSQL = "select distinct pricesegment_gid from smr_trn_tpricesegment2product" +
                            " where product_gid = '" + values.product_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        values.pricesegments = new List<string>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                priceSegmentGid = dt["pricesegment_gid"].ToString();
                                values.pricesegments.Add(priceSegmentGid);

                            }

                        }
                        for (int i = 0; i < values.pricesegments.Count; i++)
                        {
                            msSQL = " select discount_percentage from smr_trn_tpricesegment  " +
                                " where pricesegment_gid ='" + values.pricesegments[i] + "'";
                            string lsdiscountpercentage = objdbconn.GetExecuteScalar(msSQL);
                            double lsdiscountamount = 0;
                            double lsproductprice = 0;
                            double lsdoublemrpprice = 0;
                            lsdoublemrpprice = double.Parse(values.mrp_price);
                            lsdiscountamount = (double.Parse(lsdiscountpercentage) * lsdoublemrpprice) / 100;
                            lsproductprice = Math.Round(lsdoublemrpprice - lsdiscountamount, 2);


                            msSQL = "update smr_trn_tpricesegment2product set product_price = '" + lsproductprice + "'," +
                                " discount_amount  = '" + lsdiscountamount + "'" +
                                " where pricesegment_gid = '" + values.pricesegments[i] + "'" +
                                " and product_gid = '" + values.product_gid + "'";
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
                            msSQL = " select mintsoftproduct_id from pmr_mst_tproduct  where product_gid='" + values.product_gid + "'  ";
                            string mintsoftproduct_id = objdbconn.GetExecuteScalar(msSQL);
                            var client = new RestClient(base_url);
                            var request = new RestRequest("/api/Product/",Method.POST);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("ms-apikey", api_key);
                            var jsonPayload = new
                            {
                                ID = mintsoftproduct_id,
                                SKU = values.sku.Replace("'", "\\\'"),
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
                values.message = "Exception occured while Updating Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }
        //Edit
        public void DaGetEditProductSummary(string product_gid, MdlSmrMstProduct values)
        {
            try
            {

                msSQL = "  select  a.batch_flag,a.serial_flag, a.purchasewarrenty_flag, y.tax_prefix ,a.expirytracking_flag,a.product_desc,a.avg_lead_time,a.customerproduct_code," +
              "  format(a.mrp_price,2) as mrp_price,format(a.cost_price,2) as cost_price,a.product_gid,a.product_name,a.product_code,b.productgroup_gid,b.productgroup_name,c.productuomclass_gid,c.productuomclass_name," +
              "  d.producttype_gid,d.producttype_name,e.productuom_gid,e.productuom_name from pmr_mst_tproduct a " +
              " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
              " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid" +
              " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
              " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid" +
               " left join acp_mst_ttax y on a.tax_gid = y.tax_gid" +
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
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            tax_name = dt["tax_prefix"].ToString(),
                            avg_lead_time = dt["avg_lead_time"].ToString(),



                        });
                        values.GetEditProductSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Prodcut Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        //View

        public void DaGetViewProductSummary(string product_gid, MdlSmrMstProduct values)
        {
            try
            {

                msSQL = "  select CASE WHEN a.batch_flag = 'N' THEN 'NO' ELSE 'YES' END AS batch_flag,CASE WHEN a.serial_flag = 'N' THEN 'NO' ELSE 'YES' END AS serial_flag," +
                    " CASE WHEN a.purchasewarrenty_flag = 'N' THEN 'NO' ELSE 'YES' END AS purchasewarrenty_flag,CASE WHEN a.expirytracking_flag = 'N' THEN 'NO' ELSE 'YES' END AS expirytracking_flag," +
                    "  a.product_desc,a.avg_lead_time,format(a.mrp_price,2) as mrp_price,z.tax_prefix , e.producttype_name,format(a.cost_price,2) as cost_price,b.currency_code,a.product_gid,c.productgroup_name,a.product_name,f.productuomclass_name,a.product_code,d.productuom_name, " +
                    "  a. customerproduct_code from pmr_mst_tproduct a " +
                    "  left join crm_trn_tcurrencyexchange b on b.currency_code=a.currency_code" +
                    "  left  join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid" +
                    "  left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid" +
                    "  left  join pmr_mst_tproducttype e on a.producttype_gid=e.producttype_gid" +
                     "  left  join acp_mst_ttax z on a.tax_gid=z.tax_gid" +
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
                            tax = dt["tax_prefix"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),

                        });
                        values.GetViewProductSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Prodcut  View !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetDeleteSalesProductdetails(string product_gid, product_list values)
        {
            try
            {
                bool pmrrrequisition = false, materialrequisition = false,
                    salesorderdtl = false, invoicedtl = false, cashsalesdtl = false, salesenquirydtl = false,
                    receivequote = false, serviceorderdtl = false;
                msSQL = " select product_gid from  pmr_trn_tpurchaserequisitiondtl " +
                        " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
              
                if (objOdbcDataReader.HasRows)
                {
                    pmrrrequisition = true;
                }
                msSQL = " select product_gid from ims_trn_tmaterialrequisitiondtl " +
                      " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows)
                {
                    materialrequisition = true;

                }
                msSQL = " select product_gid from smr_trn_tsalesorderdtl " +
                    " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows)
                {
                    salesorderdtl = true;

                }
                msSQL = " select product_gid from rbl_trn_tinvoicedtl " +
                  " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows)
                {
                    invoicedtl = true;

                }
                msSQL = " select product_gid from smr_trn_tcashsalesdtl " +
                         " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows)
                {
                    cashsalesdtl = true;

                }
                msSQL = " select product_gid from smr_trn_tsalesenquirydtl " +
                        " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows)
                {
                    salesenquirydtl = true;

                }
                msSQL = " select product_gid from smr_trn_treceivequotationdtl " +
                        " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                ;
                if (objOdbcDataReader.HasRows)
                {
                    receivequote = true;

                }
                msSQL = " select product_gid from pbl_trn_tserviceorderdtl " +
                        " where product_gid = '" + product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
             
                if (objOdbcDataReader.HasRows)
                {
                    serviceorderdtl = true;

                }
                if (!(pmrrrequisition || materialrequisition || invoicedtl || cashsalesdtl || salesenquirydtl || receivequote || serviceorderdtl || salesorderdtl))
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
                values.message = "Exception occured whileDeleting Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }        //public void DaProductImportExcel(HttpRequest httpRequest, string user_gid, result productresult, product_list values)
        //{
        //    try {

        //        string lscompany_code;
        //    try
        //    {
        //        HttpFileCollection httpFileCollection;
        //        string lspath, lsfilePath;

        //        msSQL = " select company_code from adm_mst_tcompany";
        //        lscompany_code = objdbconn.GetExecuteScalar(msSQL);

        //        // Create Directory
        //        lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"];

        //        if (!Directory.Exists(lsfilePath))
        //            Directory.CreateDirectory(lsfilePath);

        //        httpFileCollection = httpRequest.Files;
        //        for (int i = 0; i < httpFileCollection.Count; i++)
        //        {
        //            httpPostedFile = httpFileCollection[i];
        //        }
        //        string FileExtension = httpPostedFile.FileName;

        //        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
        //        string lsfile_gid = msdocument_gid;
        //        FileExtension = Path.GetExtension(FileExtension).ToLower();
        //        lsfile_gid = lsfile_gid + FileExtension;
        //        FileInfo fileinfo = new FileInfo(lsfilePath);
        //        Stream ls_readStream;
        //        ls_readStream = httpPostedFile.InputStream;
        //        MemoryStream ms = new MemoryStream();
        //        ls_readStream.CopyTo(ms);

        //        //path creation        
        //        lspath = lsfilePath + "/";
        //        FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
        //        ms.WriteTo(file);
        //        try
        //        {
        //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;                    
        //            string status;
        //            status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
        //            file.Close();
        //            ms.Close();                    
        //        }
        //        catch (Exception ex)
        //        {
        //            productresult.status = false;
        //            productresult.message = ex.ToString();
        //            return;
        //        }

        //        //Excel To DataTable
        //        try
        //        {
        //            DataTable dataTable = new DataTable();
        //            int totalSheet = 1;
        //            string connectionString = string.Empty;
        //            string fileExtension = Path.GetExtension(lspath);

        //            //lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";
        //            //string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");
        //            //excelRange = "A1:" + endRange + rowCount.ToString();
        //            //dt = objcmnfunctions.ExcelToDataTable(correctedPath, excelRange);
        //            //dt = dt.Rows.Cast<DataRow>().Where(r => string.Join("", r.ItemArray).Trim() != string.Empty).CopyToDataTable();
        //            lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

        //            string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");

        //            try
        //            {
        //                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
        //            }
        //            catch (Exception ex)
        //            {

        //            }

        //            using (OleDbConnection connection = new OleDbConnection(connectionString))
        //            {
        //                connection.Open();
        //                DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        //                if (schemaTable != null)
        //                {
        //                    var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
        //                                         where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
        //                                         select dataRow).CopyToDataTable();

        //                    schemaTable = tempDataTable;
        //                    totalSheet = schemaTable.Rows.Count;
        //                    using (OleDbCommand command = new OleDbCommand())
        //                    {                                
        //                        command.Connection = connection;
        //                        command.CommandText = "select * from [Sheet1$]";

        //                        using (OleDbDataReader reader = command.ExecuteReader())
        //                        {
        //                            dataTable.Load(reader);
        //                        }                             

        //                        foreach (DataRow dt_product in dataTable.Rows)
        //                        {
        //                            string exproducttype = dt_product["PRODUCT TYPE"].ToString();
        //                            string exproductgroup = dt_product["PRODUCT GROUP"].ToString();
        //                            string exproductcode = dt_product["PRODUCT CODE"].ToString();
        //                            string exproducthsncode = dt_product["HSN CODE"].ToString();
        //                            string exproduct = dt_product["PRODUCT"].ToString();
        //                            string exproductdescription = dt_product["PRODUCT DESCRIPTION"].ToString();
        //                            string exproductunit = dt_product["UNITS"].ToString();
        //                            string exproductserialno = dt_product["SERIAL NUMBER TRACKER"].ToString();
        //                            string exproductwarrenty = dt_product["WARRANTY TRACKER"].ToString();
        //                            string exproductExpirydate = dt_product["EXPIRY DATE TRACKER"].ToString();
        //                            string exproductcostprice = dt_product["COST PRICE"].ToString();
        //                            string lsstatus = "Y", lsserialflag = "1", lsserialtracking_flag = "", lsstockflag = "";




        //                            msSQL = "Select producttype_gid from pmr_mst_tproducttype " +
        //                                    "where producttype_name = '" + exproducttype + "'";
        //                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //                            if (objOdbcDataReader.HasRows == true)
        //                            {
        //                                objOdbcDataReader.Read();
        //                                exclproducttypecode = objOdbcDataReader["producttype_gid"].ToString();
        //                                objOdbcDataReader.Close();
        //                            }
        //                            msSQL = " Select productgroup_gid from pmr_mst_tproductgroup " +
        //                            " where productgroup_name = '" + exproductgroup + "'";
        //                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //                            if (objOdbcDataReader.HasRows == true)
        //                            {
        //                                objOdbcDataReader.Read();
        //                                mcGetGID = objOdbcDataReader["productgroup_gid"].ToString();
        //                                objOdbcDataReader.Close();
        //                            }
        //                            else
        //                            {
        //                                mcGetGID = objcmnfunctions.GetMasterGID("PPGM");
        //                            }                                   
        //                            msSQL = "insert into pmr_mst_tproductgroup (" +
        //                                            "productgroup_gid, " +
        //                                            "productgroup_code, " +
        //                                            "productgroup_name) " +
        //                                            "values (" +
        //                                            "'" + mcGetGID + "', " +
        //                                            "'" + exproductgroup + "', " +
        //                                            "'" + exproductgroup + "')";
        //                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                            if (mnResult != 0)
        //                            {
        //                                msSQL = " Select productuomclass_gid from pmr_mst_tproductuomclass " +
        //                                        " where productuomclass_name = '" + exproductunit + "'";
        //                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //                                if (objOdbcDataReader.HasRows == true)
        //                                {
        //                                    objOdbcDataReader.Read();
        //                                    maGetGID = objOdbcDataReader["productuomclass_gid"].ToString();
        //                                    objOdbcDataReader.Close();
        //                                }
        //                                else
        //                                {
        //                                    maGetGID = objcmnfunctions.GetMasterGID("PUCM");
        //                                }
        //                                msSQL = "insert into pmr_mst_tproductuomclass (" +
        //                                        "productuomclass_gid, " +
        //                                        "productuomclass_code, " +
        //                                        "productuomclass_name) " +
        //                                        "values(" +
        //                                        "'" + maGetGID + "', " +
        //                                        "'" + exproductunit + "', " +
        //                                        "'" + exproductunit + "')";
        //                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                                if (mnResult != 0)
        //                                {
        //                                    msSQL = " Select productuom_gid from pmr_mst_tproductuom " +
        //                                            " where productuom_name = '" + exproductunit + "'";
        //                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //                                    if (objOdbcDataReader.HasRows == true)
        //                                    {
        //                                        objOdbcDataReader.Read();
        //                                        msGetGID = objOdbcDataReader["productuom_gid"].ToString();
        //                                        objOdbcDataReader.Close();
        //                                    }
        //                                    else
        //                                    {
        //                                        msGetGID = objcmnfunctions.GetMasterGID("PPMM");
        //                                    }
        //                                    msSQL = "insert into pmr_mst_tproductuom (" +
        //                                            "productuom_gid, " +
        //                                            "productuom_code, " +
        //                                            "productuom_name, " +
        //                                            "productuomclass_gid) " +
        //                                            "values(" +
        //                                            "'" + msGetGID + "', " +
        //                                            "'" + exproductunit + "', " +
        //                                            "'" + exproductunit + "', " +
        //                                            "'" + maGetGID + "')";
        //                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                                    if (mnResult != 0)
        //                                    {
        //                                        msSQL = " Select product_gid from pmr_mst_tproduct " +
        //                                                " where product_code =  '" + exproductcode + "' and product_name = '" + exproduct + "'";
        //                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //                                        if (objOdbcDataReader.HasRows == true)
        //                                        {
        //                                            objOdbcDataReader.Close();
        //                                        }
        //                                        else
        //                                        {
        //                                            mrGetGID = objcmnfunctions.GetMasterGID("PPTM");
        //                                        }

        //                                        msSQL = "insert into pmr_mst_tproduct (" +
        //                                              "product_gid, " +
        //                                              "productgroup_gid, " +
        //                                                        "productuom_gid, " +
        //                                                       "productuomclass_gid, " +
        //                                                        "product_code, " +
        //                                                        "product_name, " +
        //                                                        "producttype_gid, " +
        //                                                        "status, " +
        //                                                                "serial_flag, " +
        //                                                                 "serialtracking_flag, " +
        //                                                                 "warrentytracking_flag, " +
        //                                                                 "expirytracking_flag, " +
        //                                                                " created_by, " +
        //                                                                 " created_date) " +
        //                                                                 "values (" +
        //                                                               "'" + mrGetGID + "', " +
        //                                                                 "'" + mcGetGID + "', " +
        //                                                                 "'" + msGetGID + "'," +
        //                                                                 "'" + maGetGID + "'," +
        //                                                                "'" + exproductcode + "', " +
        //                                                                 "'" + exproduct + "', " +
        //                                                                 "'" + exclproducttypecode + "', " +
        //                                                                "'" + lsstatus + "', " +
        //                                                                 "'" + lsserialflag + "', " +
        //                                                                 "'" + lsserialtracking_flag + "', " +
        //                                                                "'" + exproductwarrenty + "', " +
        //                                                                 "'" + exproductwarrenty + "', " +
        //                                                                 "'" + user_gid + "'," +
        //                                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
        //                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                                            }
        //                                        }
        //                                }

        //                        }
        //                    }
        //                }
        //            }
        //        }               
        //        catch (Exception ex)
        //        {
        //            productresult.status = false;
        //            productresult.message = ex.ToString();
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        productresult.status = false;
        //        productresult.message = ex.ToString();
        //    }
        //    if (mnResult != 0)
        //    {
        //        values.status = true;
        //        values.message = "Product Added Successfully";
        //    }
        //    else
        //    {
        //        values.status = false;
        //        values.message = "Please Enter All Mandatory Fields";
        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Importin Product !";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
        //       $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
        //       msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
        //    }

        //}




        //Product Import Excel

        public void DaProductImportExcel(HttpRequest httpRequest, string user_gid, result productresult, product_list values)
        {
            try
            {

                string lscompany_code;
                try
                {
                    HttpFileCollection httpFileCollection;
                    string lspath, lsfilePath;

                    msSQL = " select company_code from adm_mst_tcompany";
                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                    // Create Directory
                    lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"] +"erp_documents/" + lscompany_code + "/Sales_module/Import_Excel/Product_Details/"
                        + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

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
                    try
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        string status;
                        status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                        file.Close();
                        ms.Close();
                    }
                    catch (Exception ex)
                    {
                        productresult.status = false;
                        productresult.message = ex.ToString();
                        return;
                    }

                    //Excel To DataTable
                    try
                    {
                        DataTable dataTable = new DataTable();
                        int totalSheet = 1;
                        string connectionString = string.Empty;
                        string fileExtension = Path.GetExtension(lspath);

                        //lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";
                        //string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");
                        //excelRange = "A1:" + endRange + rowCount.ToString();
                        //dt = objcmnfunctions.ExcelToDataTable(correctedPath, excelRange);
                        //dt = dt.Rows.Cast<DataRow>().Where(r => string.Join("", r.ItemArray).Trim() != string.Empty).CopyToDataTable();
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

                                    foreach (DataRow dt_product in dataTable.Rows)
                                    {
                                        string exproducttype = dt_product["PRODUCT TYPE"].ToString();
                                        string exproductgroup = dt_product["PRODUCT GROUP"].ToString();
                                        //string exproductcode = dt_product["PRODUCT CODE"].ToString();
                                        string exproducthsncode = dt_product["HSN CODE"].ToString();
                                        string exproduct = dt_product["PRODUCT"].ToString();
                                        string exproductdescription = dt_product["PRODUCT DESCRIPTION"].ToString();
                                        string exproductunit = dt_product["UNITS"].ToString();
                                        string exproductunitclass = dt_product["UNIT CLASS"].ToString();
                                        string exproductserialno = dt_product["SERIAL NUMBER TRACKER"].ToString();
                                        string exproductwarrenty = dt_product["WARRANTY TRACKER"].ToString();
                                        string exproductExpirydate = dt_product["EXPIRY DATE TRACKER"].ToString();
                                        string exproductcostprice = dt_product["COST PRICE"].ToString();
                                        string exproductmrp = dt_product["MRP"].ToString();
                                        string lsstatus = "1", lsserialflag = "", lsserialtracking_flag = "";
                                        if (exproducttype != null && exproducttype != "" && exproductgroup != null && exproductgroup != "" &&
                                                    exproduct != null && exproduct != ""  && exproductmrp != null && exproductmrp != "")
                                        {
                                            msSQL = "Select producttype_gid from pmr_mst_tproducttype " +
                                                    "where producttype_name = '" + exproducttype + "'";
                                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                            if (objOdbcDataReader.HasRows == true)
                                            {
                                               
                                                exclproducttypecode = objOdbcDataReader["producttype_gid"].ToString();
                                                
                                            }
                                            msSQL = " Select productgroup_gid from pmr_mst_tproductgroup " +
                                            " where productgroup_name = '" + exproductgroup + "'";
                                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                            if (objOdbcDataReader.HasRows == true)
                                            {
                                                
                                                mcGetGID = objOdbcDataReader["productgroup_gid"].ToString();
                                               
                                            }
                                            else
                                            {
                                                mcGetGID = objcmnfunctions.GetMasterGID("PPGM");
                                            }
                                            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPGM' order by finyear desc limit 0,1 ";
                                            string lsCode1 = objdbconn.GetExecuteScalar(msSQL);

                                            string lsproductgroup_code = "PGC" + "00" + lsCode1;
                                            msSQL = "insert into pmr_mst_tproductgroup (" +
                                                            "productgroup_gid, " +
                                                            "productgroup_code, " +
                                                            "productgroup_name) " +
                                                            "values (" +
                                                            "'" + mcGetGID + "', " +
                                                            "'" + lsproductgroup_code + "', " +
                                                            "'" + exproductgroup + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                            {
                                                msSQL = " Select productuomclass_gid from pmr_mst_tproductuomclass " +
                                                        " where productuomclass_name = '" + exproductunit + "'";
                                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                                if (objOdbcDataReader.HasRows == true)
                                                {
                                                    
                                                    maGetGID = objOdbcDataReader["productuomclass_gid"].ToString();
                                                    
                                                }
                                                else
                                                {
                                                    maGetGID = objcmnfunctions.GetMasterGID("PUCM");
                                                }
                                                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PUCM' order by finyear desc limit 0,1 ";
                                                string lsCode = objdbconn.GetExecuteScalar(msSQL);

                                                string productuomclass_code = "PUC" + "000" + lsCode;
                                                msSQL = "insert into pmr_mst_tproductuomclass (" +
                                                        "productuomclass_gid, " +
                                                        "productuomclass_code, " +
                                                        "productuomclass_name) " +
                                                        "values(" +
                                                        "'" + maGetGID + "', " +
                                                        "'" + productuomclass_code + "', " +
                                                        "'" + exproductunitclass + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                {
                                                    msSQL = " Select productuom_gid from pmr_mst_tproductuom " +
                                                            " where productuom_name = '" + exproductunit + "'";
                                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                                    if (objOdbcDataReader.HasRows == true)
                                                    {
                                                       
                                                        msGetGID = objOdbcDataReader["productuom_gid"].ToString();
                                                        
                                                    }
                                                    else
                                                    {
                                                        msGetGID = objcmnfunctions.GetMasterGID("PPMM");
                                                    }
                                                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPMM' order by finyear desc limit 0,1 ";
                                                    string lsCode3 = objdbconn.GetExecuteScalar(msSQL);

                                                    string productuom_code = "PU" + "000" + lsCode3;
                                                    msSQL = "insert into pmr_mst_tproductuom (" +
                                                            "productuom_gid, " +
                                                            "productuom_code, " +
                                                            "productuom_name, " +
                                                            "productuomclass_gid) " +
                                                            "values(" +
                                                            "'" + msGetGID + "', " +
                                                            "'" + productuom_code + "', " +
                                                            "'" + exproductunit + "', " +
                                                            "'" + maGetGID + "')";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                    {
                                                        msSQL = " Select product_gid from pmr_mst_tproduct " +
                                                                " where product_code =  '" + lsproduct_code + "' and product_name = '" + exproduct + "'";
                                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                                        if (objOdbcDataReader.HasRows == true)
                                                        {
                                                            objOdbcDataReader.Close();
                                                        }
                                                        else
                                                        {
                                                            mrGetGID = objcmnfunctions.GetMasterGID("PPTM");


                                                        }
                                                        msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                                                        string lsCode4 = objdbconn.GetExecuteScalar(msSQL);
                                                        lsproduct_code = "PC" + "00" + lsCode4;

                                                        msSQL = "insert into pmr_mst_tproduct (" +
                                                              "product_gid, " +
                                                              "productgroup_gid, " +
                                                                        "productuom_gid, " +
                                                                       "productuomclass_gid, " +
                                                                        "product_code, " +
                                                                        "product_name, " +
                                                                        "producttype_gid, " +
                                                                        "status, " +
                                                                        "cost_price, " +
                                                                        "mrp_price, " +
                                                                                "serial_flag, " +
                                                                                 "serialtracking_flag, " +
                                                                                 "warrentytracking_flag, " +
                                                                                 "expirytracking_flag, " +
                                                                                " created_by, " +
                                                                                 " created_date) " +
                                                                                 "values (" +
                                                                               "'" + mrGetGID + "', " +
                                                                                 "'" + mcGetGID + "', " +
                                                                                 "'" + msGetGID + "'," +
                                                                                 "'" + maGetGID + "'," +
                                                                                "'" + lsproduct_code + "', " +
                                                                                 "'" + exproduct + "', " +
                                                                                 "'" + exclproducttypecode + "', " +
                                                                                "'" + lsstatus + "', " +
                                                                                "'" + exproductcostprice + "', " +
                                                                                "'" + exproductmrp + "', " +
                                                                                 "'" + lsserialflag + "', " +
                                                                                 "'" + lsserialtracking_flag + "', " +
                                                                                "'" + exproductwarrenty + "', " +
                                                                                 "'" + exproductwarrenty + "', " +
                                                                                 "'" + user_gid + "'," +
                                                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    }
                                                    if (mnResult == 1)
                                                    {
                                                        values.status = true;
                                                        values.message = " Excel Successfully Imported";
                                                        return;
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            msGETLOGGID = objcmnfunctions.GetMasterGID("MPEL");
                                            msSQL = " insert into pmr_tmp_tproducttemplog (" +
                                                            " producttemplog_gid, " +
                                                            " uploadexcellog_gid, " +
                                                            " productgroup," +
                                                            " producttype," +
                                                            " product_name, " +
                                                            " productuom_name, " +
                                                            " productuomclass_name, " +
                                                            " cost_price, " +
                                                            " mrp" +
                                                            " ) values ( " +
                                                            "'" + msGETLOGGID + "'," +
                                                            "'" + msdocument_gid + "'," +
                                                            "'" + exproductgroup.Trim() + "'," +
                                                            "'" + exproducttype.Trim() + "'," +
                                                            "'" + exproduct.Trim() + "'," +
                                                            "'" + exproductunit.Trim() + "'," +
                                                            "'" + exproductunitclass.Trim() + "'," +
                                                            "'" + exproductcostprice.Trim() + "'," +
                                                            "'" + exproductmrp.Trim() + "')";

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
                        productresult.status = false;
                        productresult.message = ex.ToString();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    productresult.status = false;
                    productresult.message = ex.ToString();
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Please Enter All Mandatory Fields";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Importing Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        // Product Active - Inactive
        public void DaGetcustomerInactive(string product_gid, MdlSmrTrnCustomerSummary values)
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

        public void DaGetcustomerActive(string product_gid, MdlSmrTrnCustomerSummary values)
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
        public void DaGetProductReportExport(MdlSmrMstProduct values)
        {
            try
            {
                msSQL = " SELECT b.productgroup_name as ProductGroupName, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as ProductName,a.product_code as ProductCode, " +
                    " e.productuom_name as Unit,d.producttype_name as ProductType   from pmr_mst_tproduct a " +
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

                    string lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents/product/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                    if (!System.IO.Directory.Exists(lspath))
                        System.IO.Directory.CreateDirectory(lspath);

                    string lsname2 = "Product_Report" + DateTime.Now.ToString("(dd-MM-yyyy HH-mm-ss)") + ".xlsx";
                    string lspath1 = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents/product/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsname2;

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
            catch (Exception ex)
            {
                values.message = "Exception occurred while Exporting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }



        //Product Upload Image
        public void DaProductImage(HttpRequest httpRequest, result objResult, string user_gid)
        {

            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;



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


                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Sales/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                        ms.Close();

                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "Sales/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
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


        //Filter Product

        public void DaGetFilterProduct(string producttype_gid, MdlSmrMstProduct values)
        {
            try
            {


                msSQL = " Select a.product_gid,a.product_name,a.product_code,date_format(a.created_date,'%d-%m-%Y')  as created_date,e.user_firstname,a.created_by,a.product_name, a.cost_price,b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid, " +
                        " d.producttype_gid,d.producttype_name,a.product_image," +
                        " CASE  WHEN a.status = '1' THEN 'Active' WHEN a.status IS NULL OR a.status = '' OR a.status = '2' THEN 'InActive' END AS status from pmr_mst_tproduct a  " +
                        " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join adm_mst_tuser e on a.created_by = e.user_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                        " left join pmr_mst_tproducttype d on d.producttype_gid = a.producttype_gid  " +
                        " where d.producttype_gid='" + producttype_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            Status = dt["status"].ToString(),
                            created_by = dt["user_firstname"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            product_image = dt["product_image"].ToString(),

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

        // IMPORT EXCEEL LOG
        public void DaGetImportLog(MdlSmrMstProduct values)
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

        // LOG DELETE
        public void DaDeleteLog (string log_id, product_list values)
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

        //Minsoft Api code by snehith
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private static Task<get> _runningTask;
        public async Task<get> DaMintsoftProductDetailsAsync(string user_gid)
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

                    _runningTask = GetMintsoftProductDetailsAsync(user_gid); // Start a new task
                    await _runningTask;
                }
                finally
                {
                    _semaphore.Release(); // Release the semaphore
                }
            }
        }


        public async Task<get> GetMintsoftProductDetailsAsync(string user_gid)
        {


            get objresult = new get();
            try
            {
                string lsproductgroup_gid = "";
                string lsproductuom_gid = "";
                string lsproductuomclass_gid = "";

                msSQL = "select productgroup_gid from pmr_mst_tproductgroup where productgroup_name = 'General'";
                lsproductgroup_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid,productuomclass_gid from pmr_mst_tproductuom where productuom_name = 'kg'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if(objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();
                    lsproductuomclass_gid = objOdbcDataReader["productuomclass_gid"].ToString();

                }
                objOdbcDataReader.Close();
                msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                // code by snehith to push  product in mintsoft
                if (mintsoft_flag == "Y")
                {
                    string lsMintSoftAccessToken = "", lsbaseurl = "", lsclient_id="";
                    msSQL = "select base_url,api_key,client_id from smr_trn_tminsoftconfig";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        objOdbcDataReader.Read();
                        lsbaseurl = objOdbcDataReader["base_url"].ToString();
                        lsMintSoftAccessToken = objOdbcDataReader["api_key"].ToString();
                        lsclient_id = objOdbcDataReader["client_id"].ToString();
                    }
                    objOdbcDataReader.Close();
                    string baseUrl = lsbaseurl + "/api/Product/List";
                    int pageNo = 1;
                    bool hasMoreData = true;

                    while (hasMoreData)
                    {
                        string url = $"{baseUrl}?PageNo={pageNo}&ClientId={lsclient_id}";
                        using (var client = new HttpClient())
                        {

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("ms-apikey", lsMintSoftAccessToken);



                            HttpResponseMessage response = await client.GetAsync(url);
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();

                                List<MintsoftProductdetails> products = JsonConvert.DeserializeObject<List<MintsoftProductdetails>>(responseBody);

                                

                                if (products != null)
                                {
                                    foreach (var item in products)
                                    {


                                        // Check if the product already exists in the database
                                        string mysql1 = "SELECT mintsoftproduct_id FROM pmr_mst_tproduct WHERE customerproduct_code = '" + item.SKU.Replace("'", "\\\'") + "'";
                                        objOdbcDataReader = objdbconn.GetDataReader(mysql1);
                                        if (objOdbcDataReader.HasRows != true)
                                        {
                                   
                                                msGetGid = objcmnfunctions.GetMasterGID("PPTM");
                                                //string msGetGid2 = objcmnfunctions.GetMasterGID("VPDC");
                                                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PPTM' order by finyear desc limit 0,1 ";
                                                string lsCode = objdbconn.GetExecuteScalar(msSQL);
                                                string lsproduct_code = "PC" + "00" + lsCode;
                                                msSQL = " insert into pmr_mst_tproduct (" +
                                                        " product_gid," +
                                                        " product_code," +
                                                        " mintsoftproduct_id, " +
                                                        " producttype_name, " +
                                                        " productgroup_gid, " +
                                                        " productuomclass_gid, " +
                                                        " productuom_gid, " +
                                                        " product_name," +
                                                        " product_desc, " +
                                                        " customerproduct_code, " +
                                                        " created_by, " +
                                                        " mrp_price, " +
                                                        " cost_price, " +
                                                        " created_date)" +
                                                        " values(" +
                                                        "'" + msGetGid + "'," +
                                                        "'" + lsproduct_code + "'," +
                                                        "'" + (item.ID ?? " ") + "'," +
                                                        "'" + "Finished Goods" + "'," +
                                                        "'" + lsproductgroup_gid + "'," +
                                                        "'" + lsproductuomclass_gid + "'," +
                                                        "'" + lsproductuom_gid + "'," +
                                                        "'" + (string.IsNullOrEmpty(item.Name) ? "" : item.Name.Replace("'", "\\'").Replace("，", ",")) + "'," +
                                                        "'" + (string.IsNullOrEmpty(item.Description) ? "" : item.Description.Replace("'", "\\'").Replace("，", ",")) + "'," +
                                                        "'" + (string.IsNullOrEmpty(item.SKU) ? "" : item.SKU.Replace("'", "\\'").Replace("，", ",")) + "'," +
                                                        "'" + user_gid + "'," +
                                                        "'" + (item.Price ?? " ") + "'," +
                                                        "'" + (item.Price ?? " ") + "'," +
                                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }
                                        else
                                        {
                                            string imageUrl = string.IsNullOrEmpty(item.ImageURL) ? "../../images/no_preview.jpg" : item.ImageURL.Replace("'", "\\'");
                                            string updateSQL = " UPDATE pmr_mst_tproduct SET " +
                                                               " mintsoftproduct_id = '" + item.ID + "'," +
                                                               " updated_by = '" + user_gid + "', " +
                                                               " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                               " WHERE customerproduct_code = '" + item.SKU.Replace("'", "\\\'") + "'";
                                            int mnResult = objdbconn.ExecuteNonQuerySQL(updateSQL);


                                        }
                                        objOdbcDataReader.Close();







                                    }
                                    pageNo++;
                                }
                                else
                                {
                                    objresult.status = true;
                                    hasMoreData = false;
                                }
                            }
                            else
                            {
                                hasMoreData = false;
                                objresult.status = false;
                                string errorMessage = $"Failed to fetch products. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
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
            return objresult;
        }
    }
}