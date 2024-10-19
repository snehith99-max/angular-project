using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.inventory.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Text;
using OfficeOpenXml.Style;
using static OfficeOpenXml.ExcelErrorValue;
using System.Runtime.Remoting;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;


namespace ems.inventory.DataAccess
{

    public class DaImsTrnOpeningStock
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string msGetGid, lsbranch_gid, lsqtyrequested, msGetStockGID, lsproduct_price, lscustomer_gid, msGetGid1, product_gid, productuom_gid;
        int mnResult, mnResult1;
        string issued_qty;
        double qtyrequested, finalQty;
        double stock, new_stock;
        string base_url, lsclient_id, api_key = string.Empty;
        public void DaGetImsTrnOpeningstockSummary(string branch_gid,string finyear,MdlImsTrnOpeningStock values)
        {
            try
            {

                msSQL = " SELECT g.branch_prefix,a.product_gid,d.stock_gid, a.product_code, a.product_name,  b.productgroup_name, c.productuom_name," +
                " d.opening_qty as opening_stock,g.branch_name,date_format(d.created_date,'%d-%m-%Y') as created_date,d.issued_qty," +
                " e.producttype_name as product_type,d.uom_gid,d.branch_gid,d.display_field,h.location_name " +
                " FROM ims_trn_tstock d " +
                " left join pmr_mst_tproduct a on d.product_gid = a.product_gid" +
                " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
                " left join pmr_mst_tproductuom c on d.uom_gid = c.productuom_gid" +
                " left join pmr_mst_tproducttype e on a.producttype_gid = e.producttype_gid" +
                " left join ims_mst_tstocktype f on d.stocktype_gid = f.stocktype_gid" +
                " left join hrm_mst_tbranch g on d.branch_gid = g.branch_gid " +
                " left join ims_mst_tlocation h on d.location_gid=h.location_gid " +
                " where d.branch_gid='" + branch_gid + "' and  d.remarks='From Opening Stock' ";
                if (finyear!="Null" )
                {
                    msSQL += " and d.financial_year = '" + finyear + "'" ;

                }
              
                msSQL+=" order by d.created_date desc,d.stock_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stock_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stock_list
                        {

                            created_date = dt["created_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            producttype_name = dt["product_type"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            opening_stock = dt["opening_stock"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            issued_qty = dt["issued_qty"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),

                        });
                        values.stock_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Open Stock Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetImsTrnOpeningstockAdd(MdlImsTrnOpeningStock values)
        {
            try
            {
                msSQL = " SELECT a.product_gid, a.product_code, a.product_name, a.product_desc, b.productgroup_name, c.productuom_name, a.created_date," +
                    " a.productuom_gid,a.productgroup_gid FROM pmr_mst_tproduct a" +
                    " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
                    " left join pmr_mst_tproductuom c on a.productuom_gid = c.productuom_gid" +
                    " order by created_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockadd_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockadd_list
                        {

                            product_gid = dt["product_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_desc = dt["product_desc"].ToString(),


                        });
                        values.stockadd_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnChangeLocation(MdlImsTrnOpeningStock values)
        {
            try
            {


                msSQL = " select location_gid,location_name,location_code " +
                " from  ims_mst_tlocation ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetLocation>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetLocation
                        {
                            location_name = dt["location_name"].ToString(),
                            location_gid = dt["location_gid"].ToString(),

                        });
                        values.GetLocation = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Location !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaPostOpeningstock(string user_gid, Postopeningstock values)
        {
            try
            {
                msSQL = " select * from ims_trn_tstock where product_gid= '" + values.product_gid + "' and " +
                    " branch_gid = '" + values.branch_gid + "' and remarks='From Opening Stock'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = " Specified product name has already created";
                }
                else
                {
                    msSQL = " SELECT product_gid,stock_qty,opening_qty FROM ims_trn_tstock " +
                            " where product_gid = '" + values.product_gid + "' and" +
                            " uom_gid = '" + values.uom_gid + "' and" +
                            " stocktype_gid = 'SY0905270001' and" +
                            " branch_gid = '" + values.branch_gid + "' and " +
                            " location_gid='" + values.location_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (objOdbcDataReader.HasRows == false)
                    {
                        objOdbcDataReader.Close();
                    }

                    if (dt_datatable.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            double lsstockQty = Convert.ToDouble(dt["stock_qty"].ToString());
                            double lsopening_qty = Convert.ToDouble(dt["opening_qty"].ToString());
                            double OpeningstockQty = lsopening_qty - Convert.ToDouble(values.stock_qty);
                            qtyrequested = lsstockQty + OpeningstockQty;
                        }

                        if (qtyrequested != 0)
                        {
                            finalQty = qtyrequested;
                        }
                        else
                        {
                            finalQty = int.Parse(values.stock_qty);
                        }
                        msSQL = " UPDATE ims_trn_tstock " +
                                " SET stock_qty = '" + finalQty + "'," +
                                " opening_qty='" + values.stock_qty + "'," +
                                " created_by='" + user_gid + "'" +
                                " WHERE product_gid = '" + values.product_gid + "' and " +
                                " uom_gid= '" + values.uom_gid + "' and " +
                                " branch_gid = '" + values.branch_gid + "' and " +
                                " stocktype_gid = 'SY0905270001' and location_gid='" + values.location_gid + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {

                            values.status = false;
                            values.message = "Error Occured while adding Stock";
                        }
                        else
                        {
                            msSQL = "select mintsoft_flag from adm_mst_tcompany";
                            string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (mintsoft_flag == "Y")
                            {
                                StockToMintSoft OBJMintsoftStock = new StockToMintSoft();

                                msSQL = " select * from smr_trn_tminsoftconfig;";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    objOdbcDataReader.Read();
                                    base_url = objOdbcDataReader["base_url"].ToString();
                                    api_key = objOdbcDataReader["api_key"].ToString();
                                }
                                objOdbcDataReader.Close();
                                msSQL = " select customerproduct_code, mintsoftproduct_id from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    OBJMintsoftStock.SKU = objOdbcDataReader["customerproduct_code"].ToString();
                                    OBJMintsoftStock.ProductId = int.Parse(objOdbcDataReader["mintsoftproduct_id"].ToString());
                                }
                                OBJMintsoftStock.Quantity = Convert.ToInt32(finalQty);
                                OBJMintsoftStock.WarehouseId = 3;
                                //string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                                // Parse the JSON object
                                JObject jsonObject = JObject.Parse(json);

                                // Create a JArray and add the JObject to it
                                JArray jsonArray = new JArray();
                                jsonArray.Add(jsonObject);

                                // Convert the JArray back to JSON string
                                string jsonArrayString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient(base_url);
                                var request = new RestRequest("/api/Product/BulkOnHandStockUpdate", Method.POST);
                                request.AddHeader("ms-apikey", api_key);
                                request.AddParameter("application/json", jsonArrayString, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    List<Class1> objResult = JsonConvert.DeserializeObject<List<Class1>>(response.Content);
                                    if (objResult[0].Success)
                                    {
                                        var result = objResult[0].ID;
                                    }
                                }
                            }
                            values.status = true;
                            values.message = "Opening Stock Updated Successfully";
                        }

                    }
                    else
                    {
                        msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                        if (msGetStockGID == "E")
                        {
                            values.status = true;
                            values.message = "Create sequence code ISKP for temp sales enquiry";
                        }


                        msSQL = " Insert into ims_trn_tstock (" +
                                        " stock_gid," +
                                        " branch_gid, " +
                                        " financial_year, " +
                                        " location_gid, " +
                                        " product_gid," +
                                        " display_field," +
                                        " uom_gid," +
                                        " stock_qty," +
                                        " created_by," +
                                        " created_date," +
                                        " stocktype_gid, " +
                                        " unit_price, " +
                                        " stock_flag," +
                                        " grn_qty," +
                                        " rejected_qty," +
                                        " issued_qty," +
                                        " amend_qty," +
                                        " damaged_qty," +
                                        " adjusted_qty," +
                                        " reference_gid," +
                                        " opening_qty," +
                                        " remarks" +
                                        " )values ( " +
                                        " '" + msGetStockGID + "', " +
                                        " '" + values.branch_gid + "'," +
                                        " '" + values.finyear + "'," +
                                        " '" + values.location_gid + "'," +
                                        " '" + values.product_gid + "'," +
                                        " '" + (string.IsNullOrEmpty(values.display_field) ? "" : values.display_field.Replace("'", "\\\'")) + "'," +
                                        " '" + values.uom_gid + "'," +
                                        " '" + values.stock_qty + "'," +
                                        " '" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                        " 'SY0905270001'," +
                                         " '" + values.unit_price + "'," +
                                        "'Y'," +
                                        "'0.00'," +
                                        "'0.00'," +
                                        "'0.00'," +
                                        "'0.00'," +
                                        "'0.00'," +
                                        "'0.00'," +
                                        "'" + msGetStockGID + "'," +
                                        "'" + values.stock_qty + "'," +
                                        "'From Opening Stock')";


                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {

                            msSQL = "select mintsoft_flag from adm_mst_tcompany";
                            string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (mintsoft_flag == "Y")
                            {

                                StockToMintSoft OBJMintsoftStock = new StockToMintSoft();

                                msSQL = " select * from smr_trn_tminsoftconfig;";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    objOdbcDataReader.Read();
                                    base_url = objOdbcDataReader["base_url"].ToString();
                                    api_key = objOdbcDataReader["api_key"].ToString();
                                }
                                objOdbcDataReader.Close();
                                msSQL = " select customerproduct_code, mintsoftproduct_id from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    OBJMintsoftStock.SKU = objOdbcDataReader["customerproduct_code"].ToString();
                                    OBJMintsoftStock.ProductId = int.Parse(objOdbcDataReader["mintsoftproduct_id"].ToString());
                                }

                                OBJMintsoftStock.Quantity = int.Parse(values.stock_qty);
                                OBJMintsoftStock.WarehouseId = 3;
                                string json = JsonConvert.SerializeObject(OBJMintsoftStock);

                                JObject jsonObject = JObject.Parse(json);

                                // Create a JArray and add the JObject to it
                                JArray jsonArray = new JArray();
                                jsonArray.Add(jsonObject);

                                // Convert the JArray back to JSON string
                                string jsonArrayString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient(base_url);
                                var request = new RestRequest("/api/Product/BulkOnHandStockUpdate", Method.POST);
                                request.AddHeader("ms-apikey", api_key);
                                request.AddParameter("application/json", jsonArrayString, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    List<Class1> objResult = JsonConvert.DeserializeObject<List<Class1>>(response.Content);
                                    if (objResult[0].Success)
                                    {
                                        var result = objResult[0].ID;
                                    }
                                }
                            }

                            values.status = true;
                            values.message = "Opening Stock Inserted Successfully";

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Opening Stock Inserted Failed";
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetEditOpeningStockSummary(string stock_gid, MdlImsTrnOpeningStock values)
        {
            try
            {

                msSQL = " SELECT a.product_gid, b.product_code, a.stock_gid,b.product_name,f.stocktype_name,b.status,a.display_field,g.location_name,g.location_gid,a.financial_year," +
                    " c.productgroup_name,d.productuom_name,a.uom_gid,b.productgroup_gid," +
                    " (a.opening_qty) as opening_stock,a.stock_qty,a.opening_qty, a.unit_price as unit_price," +
                    " e.branch_name,a.branch_gid,(a.opening_qty * a.unit_price) as total_price" +
                    " from ims_trn_tstock a" +
                    " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid" +
                    " left join pmr_mst_tproductuom d on a.uom_gid=d.productuom_gid" +
                    " left join hrm_mst_tbranch e on a.branch_gid=e.branch_gid" +
                    " left join ims_mst_tstocktype f on a.stocktype_gid=f.stocktype_gid" +
                    " left join ims_mst_tlocation g on a.location_gid=g.location_gid" +
                    " where a.stock_gid='" + stock_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEditOpeningStock>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditOpeningStock
                        {


                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            financial_year = dt["financial_year"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            location_gid = dt["location_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["uom_gid"].ToString(),
                            cost_price = dt["unit_price"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            product_desc = dt["display_field"].ToString(),
                            opening_stock = dt["opening_stock"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            product_status = dt["status"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),
                            opening_qty = dt["opening_qty"].ToString(),

                        });
                        values.GetEditOpeningStock = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostOpeningStockUpdate(string user_gid, Stockedit_list1 values)
        {
            try
            {
                msGetStockGID = objcmnfunctions.GetMasterGID("IOSP");
                if (msGetStockGID == "E")
                {
                    values.status = true;
                    values.message = "Create sequence code IOSP for temp sales enquiry";
                }
                msSQL = " SELECT product_gid  FROM pmr_mst_tproduct  WHERE product_name='" + values.product_name + "' ";
                string lsproduct_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT producttype_gid  FROM pmr_mst_tproduct  WHERE product_gid='" + lsproduct_gid + "' ";
                string lsproducttype_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT location_gid  FROM ims_mst_tlocation  WHERE location_name='" + values.location_name + "' ";
                string lslocation_gid = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " INSERT INTO ims_trn_topeningstockedit" +
                                " (stockedit_gid," +
                                " branch_gid," +
                                " financial_year," +
                                " product_gid," +
                                " stockedit_quantity," +
                                " producttype_gid," +
                                " uom_gid," +
                                " display_field," +
                                " reference_gid," +
                                " product_status," +
                                " created_by," +
                                " updated_by," +
                                " updated_date," +
                                " created_date)" +
                                " values (" +
                                " '" + msGetStockGID + "', " +
                                " '" + values.branch_gid + "'," +
                                " '" + values.finyear + "'," +
                                " '" + lsproduct_gid + "'," +
                                " '" + values.stock_qty + "'," +
                                " '" + lsproducttype_gid + "'," +
                                " '" + values.productuom_gid + "'," +
                                " '" + values.product_desc.Replace("'","\\\'") + "'," +
                                " '" + values.stock_gid + "'," +
                                " '" + values.product_status + "'," +
                                " '" + user_gid + "'," +
                                " '" + user_gid + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";


                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                 new_stock = Convert.ToDouble(values.opening_stock)-Convert.ToDouble(values.opening_qty) ;
                 stock = Convert.ToDouble(values.stock_qty) + new_stock;
                msSQL = " UPDATE ims_trn_tstock " +
                        " SET stock_qty = '" + stock + "'," +
                        " display_field='" + values.product_desc.Replace("'", "\\\'") + "'," +
                        " unit_price='" + values.cost_price + "'," +
                        " product_gid='" + lsproduct_gid + "'," +
                        " uom_gid='" + values.productuom_gid + "'," +
                        " branch_gid='" + values.branch_gid + "'," +
                        " location_gid='" + lslocation_gid + "'," +
                        " updated_by= '" + user_gid + "'," +
                        " opening_qty= '" + values.opening_stock + "'," +
                        " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " financial_year='" + values.finyear + "'" +
                        " WHERE stock_gid ='" + values.stock_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = "select mintsoft_flag from adm_mst_tcompany";
                    string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                    if (mintsoft_flag == "Y")
                    {
                        StockToMintSoft OBJMintsoftStock = new StockToMintSoft();

                        msSQL = " select * from smr_trn_tminsoftconfig;";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            objOdbcDataReader.Read();
                            base_url = objOdbcDataReader["base_url"].ToString();
                            api_key = objOdbcDataReader["api_key"].ToString();
                        }
                        objOdbcDataReader.Close();
                        msSQL = " select customerproduct_code, mintsoftproduct_id from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            OBJMintsoftStock.SKU = objOdbcDataReader["customerproduct_code"].ToString();
                            OBJMintsoftStock.ProductId = int.Parse(objOdbcDataReader["mintsoftproduct_id"].ToString());
                        }
                        OBJMintsoftStock.Quantity = int.Parse(stock.ToString("F2"));
                        OBJMintsoftStock.WarehouseId = 3;
                        //string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                        string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                        // Parse the JSON object
                        JObject jsonObject = JObject.Parse(json);

                        // Create a JArray and add the JObject to it
                        JArray jsonArray = new JArray();
                        jsonArray.Add(jsonObject);

                        // Convert the JArray back to JSON string
                        string jsonArrayString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(base_url);
                        var request = new RestRequest("/api/Product/BulkOnHandStockUpdate", Method.POST);
                        request.AddHeader("ms-apikey", api_key);
                        request.AddParameter("application/json", jsonArrayString, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            List<Class1> objResult = JsonConvert.DeserializeObject<List<Class1>>(response.Content);
                            if (objResult[0].Success)
                            {
                                var result = objResult[0].ID;
                            }
                        }
                    }
                    values.status = true;
                    values.message = "Stock Updated Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Stock";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnChangeproductName(string product_gid, MdlImsTrnOpeningStock values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select a.product_name, a.product_code,a.mrp_price as cost_price,a.product_desc, " +
                        " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                         " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                    " where a.product_gid='" + product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetproductsCode>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetproductsCode
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["cost_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),

                        });
                        values.ProductsCode = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
            }

        }
        public void DaGetonchangeProductNamDtl(string productgroup_gid, MdlImsTrnOpeningStock values)
        {
            try
            {
                msSQL = " SELECT a.product_gid, a.product_name, c.productgroup_name " +
                        " FROM pmr_mst_tproduct a " +
                        " LEFT JOIN pmr_mst_tproducttype b ON b.producttype_gid = a.producttype_gid " +
                        " LEFT JOIN pmr_mst_tproductgroup c ON c.productgroup_gid = a.productgroup_gid " +
                        " WHERE a.productgroup_gid = '" + productgroup_gid + "' " +
                        " AND b.producttype_name <> 'Services' " +
                        " GROUP BY a.product_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProduct_name>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProduct_name

                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.GetProduct_name = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
            }

        }

        private static SemaphoreSlim _semaphore6 = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue6 = new ConcurrentQueue<string>();
        private static Task<get> _runningTask6;
        public async Task<get> DaMintsoftProductStockDetailsAsync(string user_gid)
        {
            _queue6.Enqueue(user_gid); // Add the request to the queue
            await ProcessQueue6(); // Process the queue
            return await _runningTask6;
        }

        private async Task ProcessQueue6()
        {
            while (_queue6.TryDequeue(out string user_gid))
            {
                await _semaphore6.WaitAsync(); // Ensure that only one method runs at a time
                try
                {
                    if (_runningTask6 != null && !_runningTask6.IsCompleted)
                    {
                        await _runningTask6; // Wait for the previous task to complete
                    }

                    _runningTask6 = GetMintsoftProductStockDetailsAsync(user_gid); // Start a new task
                    await _runningTask6;
                }
                finally
                {
                    _semaphore6.Release(); // Release the semaphore
                }
            }
        }


        public async Task<get> GetMintsoftProductStockDetailsAsync(string user_gid)
        {


            get objresult = new get();
            try
            {
                msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                // code by snehith to check  mintsoft_flag for  mintsoft stock details
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
                    string apiKey = api_key;
                    string baseUrl = "https://api.mintsoft.co.uk/api/Product/List";
                    int pageNo = 1;
                    bool hasMoreData = true;

                    while (hasMoreData)
                    {
                        string url = $"{baseUrl}?PageNo={pageNo}&ClientId={lsclient_id}";

                        using (var client = new HttpClient())
                        {

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                            HttpResponseMessage response = await client.GetAsync(url);
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();

                                List<MintsoftProduct> products = JsonConvert.DeserializeObject<List<MintsoftProduct>>(responseBody);

                                if (products != null)
                                {
                                    foreach (var item in products)
                                    {

                                        string BaseUrl1 = "https://api.mintsoft.co.uk/api/Product/";
                                        string url1 = $"{BaseUrl1}{item.ID}/Inventory";

                                        using (var client1 = new HttpClient())
                                        {
                                            client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                            client1.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                                            HttpResponseMessage response1 = await client1.GetAsync(url1);
                                            if (response1.IsSuccessStatusCode)
                                            {
                                                string responseBody1 = await response1.Content.ReadAsStringAsync();

                                                List<ProductInventory> inventoryDetails = JsonConvert.DeserializeObject<List<ProductInventory>>(responseBody1);

                                                foreach (var inventory in inventoryDetails)
                                                {

                                                    msSQL = " select product_gid,productuom_gid from pmr_mst_tproduct WHERE mintsoftproduct_id = '" + item.ID + "' or customerproduct_code = '" + item.SKU + "' ";
                                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                                    if (objOdbcDataReader.HasRows == true)
                                                    {
                                                        product_gid = objOdbcDataReader["product_gid"].ToString();
                                                        productuom_gid = objOdbcDataReader["productuom_gid"].ToString();


                                                    }
                                                    objOdbcDataReader.Close();
                                                    string lsproductgroup_gid = "";
                                                    string lsproductuom_gid = "";
                                                    string lsproductuomclass_gid = "";

                                                    msSQL = "select productgroup_gid from pmr_mst_tproductgroup where productgroup_name = 'General'";
                                                    lsproductgroup_gid = objdbconn.GetExecuteScalar(msSQL);

                                                    msSQL = "select productuom_gid,productuomclass_gid from pmr_mst_tproductuom where productuom_name = 'kg'";
                                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                                    if (objOdbcDataReader.HasRows == true)
                                                    {
                                                        lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();
                                                        lsproductuomclass_gid = objOdbcDataReader["productuomclass_gid"].ToString();

                                                    }
                                                    objOdbcDataReader.Close();
                                                    msSQL = " select branch_gid from hrm_mst_tbranch limit 1 ";
                                                    string branch_gid = objdbconn.GetExecuteScalar(msSQL);
                                                    msSQL = " select location_gid from ims_mst_tlocation where branch_gid='" + branch_gid + "' limit 1";
                                                    string location_gid = objdbconn.GetExecuteScalar(msSQL);
                                                    string mysql1 = "SELECT mintsoftproduct_id FROM ims_trn_tstock WHERE mintsoftproduct_id = '" + item.ID + "'";
                                                    objOdbcDataReader = objdbconn.GetDataReader(mysql1);
                                                    if (objOdbcDataReader.HasRows != true)
                                                    {
                                                        if (string.IsNullOrEmpty(product_gid))
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
                                                                    " '" + msGetGid + "'," +
                                                                    "'" + lsproduct_code + "'," +
                                                                    "'" + (item.ID ?? " ") + "'," +
                                                                    "'" + "Finished Goods" + "'," +
                                                                    "'" + lsproductgroup_gid + "'," +
                                                                    "'" + lsproductuomclass_gid + "'," +
                                                                    "'" + lsproductuom_gid + "'," +
                                                                    "'" + (string.IsNullOrEmpty(item.Name) ? "" : item.Name.Replace("'", "\\\'")) + "'," +
                                                                    "'" + (string.IsNullOrEmpty(item.Description) ? "" : item.Description.Replace("'", "\\\'")) + "'," +
                                                                    "'" + item.SKU + "'," +
                                                                    "'" + user_gid + "'," +
                                                                    "'" + (item.Price ?? " ") + "'," +
                                                                    "'" + (item.Price ?? " ") + "'," +
                                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                                            int mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                            if (mnResult2 == 0)
                                                            {
                                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                                             "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                                            }
                                                            else
                                                            {
                                                                product_gid = msGetGid;
                                                                productuom_gid = lsproductuom_gid;
                                                                msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                                                                msSQL = " Insert into ims_trn_tstock (" +
                                                                           " stock_gid," +
                                                                           " branch_gid, " +
                                                                           " location_gid, " +
                                                                           " warehouse_id, " +
                                                                           " product_gid," +
                                                                           " display_field," +
                                                                           " uom_gid," +
                                                                           " stock_qty," +
                                                                           " created_by," +
                                                                           " created_date," +
                                                                           " stocktype_gid, " +
                                                                           " mintsoftproduct_id, " +
                                                                           " mintsoft_inventoryid, " +
                                                                           " stock_flag," +
                                                                           " grn_qty," +
                                                                           " rejected_qty," +
                                                                           " issued_qty," +
                                                                           " amend_qty," +
                                                                           " damaged_qty," +
                                                                           " adjusted_qty," +
                                                                           " reference_gid," +
                                                                           " opening_qty," +
                                                                           " remarks" +
                                                                           " )values ( " +
                                                                           " '" + msGetStockGID + "', " +
                                                                           " '" + branch_gid + "'," +
                                                                           " '" + location_gid + "'," +
                                                                           " '" + inventory.WarehouseId + "'," +
                                                                           " '" + product_gid + "'," +
                                                                           " '" + (string.IsNullOrEmpty(item.Name) ? "" : item.Name.Replace("'", "\\\'")) + "'," +
                                                                           " '" + productuom_gid + "'," +
                                                                           " '" + inventory.StockLevel + "'," +
                                                                           " '" + user_gid + "'," +
                                                                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                                           " 'SY0905270001'," +
                                                                           " '" + inventory.ProductId + "'," +
                                                                           " '" + inventory.ID + "'," +
                                                                           "'Y'," +
                                                                           "'0.00'," +
                                                                           "'0.00'," +
                                                                           "'0.00'," +
                                                                           "'0.00'," +
                                                                           "'0.00'," +
                                                                           "'0.00'," +
                                                                           "'" + msGetStockGID + "'," +
                                                                           "'" + inventory.StockLevel + "'," +
                                                                           "'From Opening Stock')";
                                                                int mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                                if (mnResult1 == 0)
                                                                {
                                                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                                                 "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {

                                                            msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                                                            msSQL = " Insert into ims_trn_tstock (" +
                                                                       " stock_gid," +
                                                                       " branch_gid, " +
                                                                       " location_gid, " +
                                                                       " warehouse_id, " +
                                                                       " product_gid," +
                                                                       " display_field," +
                                                                       " uom_gid," +
                                                                       " stock_qty," +
                                                                       " created_by," +
                                                                       " created_date," +
                                                                       " stocktype_gid, " +
                                                                       " mintsoftproduct_id, " +
                                                                       " mintsoft_inventoryid, " +
                                                                       " stock_flag," +
                                                                       " grn_qty," +
                                                                       " rejected_qty," +
                                                                       " issued_qty," +
                                                                       " amend_qty," +
                                                                       " damaged_qty," +
                                                                       " adjusted_qty," +
                                                                       " reference_gid," +
                                                                       " opening_qty," +
                                                                       " remarks" +
                                                                       " )values ( " +
                                                                       " '" + msGetStockGID + "', " +
                                                                       " '" + branch_gid + "'," +
                                                                       " '" + location_gid + "'," +
                                                                       " '" + inventory.WarehouseId + "'," +
                                                                       " '" + product_gid + "'," +
                                                                       " '" + (string.IsNullOrEmpty(item.Name) ? "" : item.Name.Replace("'", "\\\'")) + "'," +
                                                                       " '" + productuom_gid + "'," +
                                                                       " '" + inventory.StockLevel + "'," +
                                                                       " '" + user_gid + "'," +
                                                                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                                       "'SY0905270001'," +
                                                                       "'" + inventory.ProductId + "'," +
                                                                       "'" + inventory.ID + "'," +
                                                                       "'Y'," +
                                                                       "'0.00'," +
                                                                       "'0.00'," +
                                                                       "'0.00'," +
                                                                       "'0.00'," +
                                                                       "'0.00'," +
                                                                       "'0.00'," +
                                                                       "'" + msGetStockGID + "'," +
                                                                       "'" + inventory.StockLevel + "'," +
                                                                       "'From Opening Stock')";
                                                            int mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                            if (mnResult1 == 0)
                                                            {
                                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                                             "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        BaseUrl1 = "https://api.mintsoft.co.uk/api/Product/";
                                                        url1 = $"{BaseUrl1}{item.ID}/Inventory";

                                                        using (var client2 = new HttpClient())
                                                        {
                                                            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                            client2.DefaultRequestHeaders.Add("ms-apikey", apiKey);

                                                            HttpResponseMessage response2 = await client2.GetAsync(url1);
                                                            if (response2.IsSuccessStatusCode)
                                                            {
                                                                string responseBody2 = await response2.Content.ReadAsStringAsync();

                                                                List<ProductInventory> inventoryDetailss = JsonConvert.DeserializeObject<List<ProductInventory>>(responseBody2);

                                                                foreach (var inventorys in inventoryDetailss)
                                                                {
                                                                    //if (!string.IsNullOrEmpty(product_gid))
                                                                    //{
                                                                    //    msSQL = "UPDATE pmr_mst_tproduct SET " +
                                                                    //            "product_name = '" + item.Name.Replace("'", "\\'").Replace("，", ",") + "'," +
                                                                    //            "product_desc = '" + (string.IsNullOrEmpty(item.Description) ? "" : item.Description.Replace("'", "\\'").Replace("，", ",")) + "'," +
                                                                    //            "customerproduct_code = '" + item.SKU + "'," +
                                                                    //            "created_by = '" + user_gid + "'," +
                                                                    //            "mrp_price = '" + (item.Price ?? " ") + "'," +
                                                                    //            "cost_price = '" + (item.Price ?? " ") + "'," +
                                                                    //            "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                                                    //            " WHERE customerproduct_code = '" + item.SKU + "'" +
                                                                    //            " OR mintsoftproduct_id = '" + (item.ID ?? " ") + "'";

                                                                    //    int mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                                    //    if (mnResult2 == 0)
                                                                    //    {
                                                                    //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                                                    //       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                                                    //    }
                                                                    //}

                                                                    msSQL = "UPDATE ims_trn_tstock SET " +
                                                                             "branch_gid = '" + branch_gid + "', " +
                                                                             "location_gid = '" + location_gid + "', " +
                                                                             "warehouse_id = '" + inventorys.WarehouseId + "', " +
                                                                             "product_gid = '" + product_gid + "', " +
                                                                             "display_field = '" + (string.IsNullOrEmpty(item.Name) ? "" : item.Name.Replace("'", "\\\'")) + "', " +
                                                                             "uom_gid = '" + productuom_gid + "', " +
                                                                             "stock_qty = '" + inventorys.StockLevel + "'," +
                                                                             "opening_qty = '" + inventorys.StockLevel + "'" +
                                                                             "WHERE mintsoftproduct_id = '" + inventorys.ProductId + "' " +
                                                                             "AND mintsoft_inventoryid = '" + inventorys.ID + "'";
                                                                            int mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                                    if (mnResult1 == 0)
                                                                    {
                                                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                                                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                hasMoreData = false;
                                                                objresult.status = false;
                                                                string errorMessage = $"Failed to fetch products. Status code: {response2.StatusCode}, Reason: {response2.ReasonPhrase}";
                                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                                            }
                                                        }


                                                    }
                                                    objOdbcDataReader.Close();

                                                }
                                            }
                                            else
                                            {
                                                hasMoreData = false;
                                                objresult.status = false;
                                                string errorMessage = $"Failed to fetch products. Status code: {response1.StatusCode}, Reason: {response1.ReasonPhrase}";
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                            }
                                        }

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

        public void DaGetBranchDetails(MdlImsTrnOpeningStock values)
        {
            try
            {
                msSQL = " select branch_gid, branch_name from hrm_mst_tbranch order by branch_gid asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<branchdtl_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branchdtl_lists
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.branchdtl_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetFinancialYear(MdlImsTrnOpeningStock values)
        {
            msSQL = " select year(fyear_start) as finyear,finyear_gid from adm_mst_tyearendactivities " +
                    " order by finyear_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var FinancialYear_List = new List<GetFinancialYear_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    FinancialYear_List.Add(new GetFinancialYear_List
                    {
                        finyear_gid = dt["finyear_gid"].ToString(),
                        finyear = dt["finyear"].ToString(),
                    });
                    values.GetFinancialYear_List = FinancialYear_List;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetProductNamDtl(MdlImsTrnOpeningStock values)
        {
            try
            {



                msSQL = " Select a.product_gid, a.product_name from pmr_mst_tproduct a" +
                        " left join pmr_mst_tproducttype b on b.producttype_gid=a.producttype_gid " +
                        "  where status='1' and b.producttype_name!='Services' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductNamDtl>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductNamDtl

                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.GetProductNamDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetProductGroup(MdlImsTrnOpeningStock values)
        {
            try
            {

                msSQL = "  Select a.productgroup_gid, a.productgroup_name from pmr_mst_tproductgroup a " +
                        " left join pmr_mst_tproduct b on b.productgroup_gid = a.productgroup_gid " +
                        " left join pmr_mst_tproducttype c on c.producttype_gid = b.producttype_gid " +
                        " where b.producttype_name != 'Services' order by productgroup_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductGroup1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductGroup1
                        {
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.GetProductGroup1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
    }
}