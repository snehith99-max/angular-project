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
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;


namespace ems.inventory.DataAccess
{

    public class DaImsTrnStockSummary 
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string msGetGid, lsbranch_gid, lsqtyrequested, msGetStockGID, msGetStockGID1, lsproduct_price, lscustomer_gid, lsstockqty;
        int mnResult, mnResult1;
        string issued_qty;
        int qtyrequested, finalQty;
        string base_url, api_key = string.Empty;
        public void DaGetStockSummary(string branch_gid,string finyear,MdlImsTrnStockSummary values)
        {
            try
            {

                msSQL = " select e.branch_prefix,b.customerproduct_code,a.product_gid,a.uom_gid,a.stock_gid,a.created_date,a.reference_gid, " +
                        "  CONCAT(COALESCE(SUM(a.stock_qty + a.amend_qty - a.issued_qty - a.damaged_qty - a.transfer_qty), 0),' ', COALESCE(d.productuom_name, '')) AS stock_balance, " +
                        "  a.branch_gid,b.product_code,b.product_name,a.display_field,c.productgroup_name,d.productuom_name,e.branch_name,f.producttype_name,b.serial_flag, " +
                        " sum(a.transfer_qty) as transfer_qty,g.location_name,g.location_gid,h.bin_number from ims_trn_tstock a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on a.uom_gid = d.productuom_gid " +
                        " left join hrm_mst_tbranch e on a.branch_gid=e.branch_gid " +
                        "  left join pmr_mst_tproducttype f on b.producttype_gid=f.producttype_code " +
                        "  left join ims_mst_tlocation g on a.location_gid=g.location_gid " +
                        "  left join ims_mst_tbin h on a.bin_gid=h.bin_gid " +
                        "  where a.stock_flag='Y' and a.stock_qty+a.amend_qty-a.issued_qty-a.damaged_qty-transfer_qty>0  " +
                        " and a.branch_gid = '" + branch_gid + "' and ";
                         if (finyear != "Null")
                         {
                            msSQL += "a.financial_year = '" + finyear + "'";

                         }
                         msSQL+= " group by a.product_gid,a.display_field,a.uom_gid,a.location_gid,a.branch_gid " +
                                 "  Order by  date(a.created_date) desc,a.created_date asc,a.stock_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocksummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocksummary
                        {
                            sku = dt["customerproduct_code"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            reference_gid = dt["reference_gid"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            transfer_qty = dt["transfer_qty"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            bin_number = dt["bin_number"].ToString(),
                            location_gid = dt["location_gid"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),

                        });
                        values.stocksummary = getModuleList;
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

        public void DaGetProducttype(MdlImsTrnStockSummary values)
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
        public void DaGetProductGroup(MdlImsTrnStockSummary values)
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
        public void DaGetProductUnitclass(MdlImsTrnStockSummary values)
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


        public void DaGetProductNamDtl(MdlImsTrnStockSummary values)
        {
            try
            {



                msSQL = " Select a.product_gid, a.product_name from pmr_mst_tproduct a " +
                        " left join pmr_mst_tproducttype b on b.producttype_gid = a.producttype_gid " +
                        " where status = '1' and b.producttype_name != 'Services'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductNamDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductNamDropdown

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
                values.message = "Exception occured while loading Product name dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // branch

        public void DaGetBranchDtl(MdlImsTrnStockSummary values)
        {
            try
            {



                msSQL = "select branch_gid,branch_name from hrm_mst_tbranch";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchDropdown

                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),

                        });
                        values.GetBranchDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Branch Dropdown !";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetImsTrnOpeningstockAdd(MdlImsTrnStockSummary values)
        {
            try
            {
                msSQL = " SELECT a.product_gid, a.product_code, a.product_name, a.product_desc, b.productgroup_name, c.productuom_name, a.created_date," +
                    " a.productuom_gid,a.productgroup_gid FROM pmr_mst_tproduct a" +
                    " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
                    " left join pmr_mst_tproductuom c on a.productuom_gid = c.productuom_gid" +
                    " order by created_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockaddnew_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockaddnew_list
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
                        values.stockaddnew_list1 = getModuleList;
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

        public void DaGetOnChangeproductName(string product_gid, MdlImsTrnStockSummary values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select a.product_name, a.product_code,a.mrp_price as cost_price,a.product_desc," +
                        " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                         " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                    " where a.product_gid='" + product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetproductsCodestock>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetproductsCodestock
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

        public void DaGetOnChangeLocation(string branch_gid,MdlImsTrnStockSummary values)
        {
            try
            {


                msSQL = " select location_gid,location_name,location_code " +
                " from  ims_mst_tlocation where branch_gid='"+ branch_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetLocationstock>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetLocationstock
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


        public void DaGetFinancialYear( MdlImsTrnStockSummary values)
        {
            try
            {


                msSQL = " select finyear_gid,cast(concat(year(fyear_start), '-', " +
                         " case " +
                         " when fyear_end is not null then year(fyear_end) " +
                         " when month(curdate())= '1' then year(curdate()) " +
                         " when month(curdate())= '2' then year(curdate()) " +
                         " when month(curdate())= '3' then year(curdate()) " +
                         " else year(date_Add(curdate(), interval 1 year)) end )as char) as finyear from adm_mst_tyearendactivities order by finyear_gid desc ";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetFinancialYear>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetFinancialYear
                        {
                            finyear = dt["finyear"].ToString(),
                            finyear_gid = dt["finyear_gid"].ToString(),
                           

                        });
                        values.GetFinancialYear = getModuleList;
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
        public void DaPoststock(string user_gid, Poststock values)
        {

            try
            {

                msSQL = " SELECT product_gid,stock_qty FROM ims_trn_tstock " +
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
                        string lsstockQty = dt["stock_qty"].ToString();
                        int stockQty = int.Parse(lsstockQty);
                        string OpeningstockQty = values.stock_qty;
                        int Openingstock = int.Parse(OpeningstockQty);
                        qtyrequested = stockQty + Openingstock;
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

                            OBJMintsoftStock.Quantity = (finalQty);
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
                                    " remarks" +
                                    " )values ( " +
                                    " '" + msGetStockGID + "', " +
                                    " '" + values.branch_gid + "'," +
                                    " '" + values.finyear + "'," +
                                    " '" + values.location_gid + "'," +
                                    " '" + values.product_gid + "'," +
                                    " '" + values.display_field.Replace("'","\\\'") + "'," +
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
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaGetAmendStockSummary(string product_gid, string uom_gid, string branch_gid, MdlImsTrnStockSummary values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = "  select a.product_gid,a.uom_gid,a.stock_gid,date_format(a.created_date,'%d-%m-%Y') as created_date,sum(a.stock_qty+a.amend_qty-a.issued_qty-a.damaged_qty-a.transfer_qty)" +
                    " as stock_balance, a.branch_gid,b.product_code,b.product_name,a.display_field,a.amend_qty,c.productgroup_name,d.productuom_name,e.branch_name,e.branch_prefix," +
                    " f.producttype_name from ims_trn_tstock a left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid  left join pmr_mst_tproductuom d on" +
                    " a.uom_gid = d.productuom_gid left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid left join pmr_mst_tproducttype f " +
                    " on b.producttype_gid = f.producttype_code where a.stock_flag = 'Y' and a.product_gid = '" + product_gid + "' " +
                    " and a.uom_gid = '" + uom_gid + "' and a.branch_gid = '" + branch_gid + "' group by a.stock_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getamendstock>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getamendstock
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            amend_qty = dt["amend_qty"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                          

                        });
                        values.Getamendstock = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
            }

        }

        public void DaGetAmendStock (string stock_gid, MdlImsTrnStockSummary values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " select a.product_gid,a.uom_gid,a.stock_gid,a.created_date,sum(a.stock_qty+a.amend_qty-a.issued_qty-a.damaged_qty-a.transfer_qty) as stock_balance," +
                        " a.branch_gid,b.product_code,b.product_name,a.display_field,a.damaged_qty,c.productgroup_name,d.productuom_name,e.branch_name,f.producttype_name " +
                        " from ims_trn_tstock a 	left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid  left join pmr_mst_tproductuom d " +
                         " on a.uom_gid = d.productuom_gid left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid left join " +
                         " pmr_mst_tproducttype f on b.producttype_gid = f.producttype_code   where a.stock_gid = '" + stock_gid + "' group by a.stock_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getamendstocklist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getamendstocklist
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                        });
                        values.Getamendstocklist = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
            }

        }


        public void DaPostamendstock(string user_gid, postamendstock values)
        {
            try
            {

                lsstockqty = values.amend_type.ToString() + "" + values.amend_qty.ToString();

                msGetStockGID = objcmnfunctions.GetMasterGID("ISTP");
                if (msGetStockGID == "E")
                {
                    values.status = true;
                    values.message = "Create a Sequence Code ISTP for Stock Details Table";
                }
                msGetStockGID1 = objcmnfunctions.GetMasterGID("IASP");
                if (msGetStockGID1 == "E")
                {
                    values.status = true;
                    values.message = "Create a Sequence Code IASP for Stock Details Table";
                }


                msSQL = " insert into ims_trn_tstockdtl(" +
                                " stockdtl_gid," +
                                " stock_gid," +
                                " branch_gid," +
                                " product_gid," +
                                " uom_gid," +
                                " issued_qty," +
                                " amend_qty," +
                                " damaged_qty," +
                                " adjusted_qty," +
                                " transfer_qty," +
                                " return_qty," +
                                " reference_gid," +
                                " stock_type," +
                                " remarks," +
                                " created_by," +
                                " created_date," +
                                " display_field" +
                                " ) values ( " +
                                "'" + msGetStockGID + "'," +
                                "'" + values.stock_gid + "'," +
                                "'"+ values.branch_gid + "'," +
                                "'" + values.product_gid + "'," +
                                 "'" + values.uom_gid + "'," +
                                "'0.00'," +
                                "'" + lsstockqty + "'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'" + msGetStockGID1 + "'," +
                                "'Amend'," +
                                "'" + (string.IsNullOrEmpty(values.remarks) ? "" : values.remarks.Replace("'", "\\\'")) + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'"+ (string.IsNullOrEmpty(values.display_field) ? "" : values.display_field.Replace("'", "\\\'")) +"')";


                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " Insert into ims_trn_tamendstock (" +
                            " amendstock_gid," +
                            " branch_gid, " +
                            " product_gid," +
                            " stock_quantity," +
                            " stocktype_gid," +
                            " created_by," +
                            " created_date," +
                            " remarks ) " +
                            " values ( " +
                            "'" + msGetStockGID1 + "', " +
                            "'" + values.branch_gid + "'," +
                            "'" + values.product_gid + "', " +
                            "'" + lsstockqty + "'," +
                            "'SY0905270008'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'" + (string.IsNullOrEmpty(values.remarks) ? "" : values.remarks.Replace("'", "\\\'")) + "')";



                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }

                if (mnResult == 1)
                {

                    msSQL = " update ims_trn_tstock set " +
                            " amend_qty=amend_qty + '" + lsstockqty + "'" +
                            " where stock_gid='" + values.stock_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                } 
                    if (mnResult == 1)
                {

                    values.status = true;
                    values.message = "Amend Stock Added Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Inserting Records into Amend Details";
                }



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetDamageStockSummary(string product_gid, string uom_gid, string branch_gid, MdlImsTrnStockSummary values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = "  select a.product_gid,a.uom_gid,a.stock_gid,a.created_date,sum(a.stock_qty+a.amend_qty-a.issued_qty-a.damaged_qty-a.transfer_qty)" +
                    " as stock_balance, a.branch_gid,b.product_code,b.product_name,a.display_field,a.damaged_qty,c.productgroup_name,d.productuom_name,e.branch_name,e.branch_prefix," +
                    " f.producttype_name from ims_trn_tstock a left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid  left join pmr_mst_tproductuom d on" +
                    " a.uom_gid = d.productuom_gid left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid left join pmr_mst_tproducttype f " +
                    " on b.producttype_gid = f.producttype_code where a.stock_flag = 'Y' and a.product_gid = '" + product_gid + "' " +
                    " and a.uom_gid = '" + uom_gid + "' and a.branch_gid = '" + branch_gid + "' group by a.stock_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getdamagestock>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdamagestock
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            damaged_qty = dt["damaged_qty"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),


                        });
                        values.Getdamagestock = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
            }

        }
        public void DaGetProductSplitSummary(string product_gid,MdlImsTrnStockSummary values)
        {
            try
            {

                msSQL = " select b.customerproduct_code,a.product_gid,a.uom_gid,a.stock_gid,a.created_date,a.reference_gid, " +
                        " sum(a.stock_qty+a.amend_qty-a.issued_qty-a.damaged_qty-a.transfer_qty) as stock_balance, " +
                        "  a.branch_gid,b.product_code,b.product_name,a.display_field,c.productgroup_name,d.productuom_name,e.branch_name,f.producttype_name,b.serial_flag, " +
                        " sum(a.transfer_qty) as transfer_qty,g.location_name,g.location_gid,h.bin_number from ims_trn_tstock a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on a.uom_gid = d.productuom_gid " +
                        " left join hrm_mst_tbranch e on a.branch_gid=e.branch_gid " +
                        "  left join pmr_mst_tproducttype f on b.producttype_gid=f.producttype_code " +
                        "  left join ims_mst_tlocation g on a.location_gid=g.location_gid " +
                        "  left join ims_mst_tbin h on a.bin_gid=h.bin_gid " +
                        "  where b.product_gid = '"+product_gid+"' and a.stock_flag='Y' and a.stock_qty+a.amend_qty-a.issued_qty-a.damaged_qty-transfer_qty>0  " +
                        "  group by a.product_gid,a.display_field,a.uom_gid,a.location_gid,a.branch_gid " +
                        "   Order by  date(a.created_date) desc,a.created_date asc,a.stock_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stocksummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stocksummary
                        {
                            sku = dt["customerproduct_code"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            reference_gid = dt["reference_gid"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            transfer_qty = dt["transfer_qty"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            bin_number = dt["bin_number"].ToString(),
                            location_gid = dt["location_gid"].ToString(),

                        });
                        values.stocksummary = getModuleList;
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


        public void DaGetDamagedStock (string stock_gid, MdlImsTrnStockSummary values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " select a.product_gid,a.uom_gid,a.stock_gid,a.created_date,sum(a.stock_qty+a.amend_qty-a.issued_qty-a.damaged_qty-a.transfer_qty) as stock_balance," +
                        " a.branch_gid,b.product_code,b.product_name,a.display_field,a.damaged_qty,c.productgroup_name,d.productuom_name,e.branch_name,f.producttype_name " +
                        " from ims_trn_tstock a 	left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid  left join pmr_mst_tproductuom d " +
                         " on a.uom_gid = d.productuom_gid left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid left join " +
                         " pmr_mst_tproducttype f on b.producttype_gid = f.producttype_code   where a.stock_gid = '" + stock_gid + "' group by a.stock_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getdamagedstocklist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdamagedstocklist
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                        });
                        values.Getdamagedstocklist = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
            }

        }


        public void DaPostDamagedstock(string user_gid, PostDamagedstock values)
        {
            try
            {

               

                msGetStockGID = objcmnfunctions.GetMasterGID("ISTP");
                if (msGetStockGID == "E")
                {
                    values.status = true;
                    values.message = "Create a Sequence Code ISTP for Stock Details Table";
                }
                msGetStockGID1 = objcmnfunctions.GetMasterGID("IDSP");
                if (msGetStockGID1 == "E")
                {
                    values.status = true;
                    values.message = "Create a Sequence Code IDSP for Stock Details Table";
                }


                msSQL = " insert into ims_trn_tstockdtl(" +
                                " stockdtl_gid," +
                                " stock_gid," +
                                " branch_gid," +
                                " product_gid," +
                                " uom_gid," +
                                " issued_qty," +
                                " amend_qty," +
                                " damaged_qty," +
                                " adjusted_qty," +
                                " transfer_qty," +
                                " return_qty," +
                                " reference_gid," +
                                " stock_type," +
                                " remarks," +
                                " created_by," +
                                " created_date," +
                                " display_field" +
                                " ) values ( " +
                                "'" + msGetStockGID + "'," +
                                "'" + values.stock_gid + "'," +
                                "'" + values.branch_gid + "'," +
                                "'" + values.product_gid + "'," +
                                 "'" + values.uom_gid + "'," +
                                "'0.00'," +
                                "'0.00'," +
                                  "'" + values.damaged_qty + "'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'" + msGetStockGID1 + "'," +
                                 "'Damaged'," +
                                   "'" + values.remarks.Replace("'","\\\'") + "'," +
                                  "'" + user_gid + "'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'"+ values.display_field +"')";


                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " Insert into ims_trn_tdamagestock (" +
                            " damagestock_gid," +
                            " branch_gid, " +
                            " product_gid," +
                            " stock_quantity," +
                            " stocktype_gid," +
                            " created_by," +
                            " created_date," +
                            " remarks ) " +
                            " values ( " +
                            " '" + msGetStockGID1 + "', " +
                            " '" + values.branch_gid + "'," +
                            " '" + values.product_gid + "', " +
                            " '" + values.damaged_qty + "'," +
                            " 'SY0905270008'," +
                            " '" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            " '" + values.remarks.Replace("'","\\\'") + "')";



                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }

                if (mnResult == 1)
                {

                    msSQL = " update ims_trn_tstock set " +
                            " damaged_qty=damaged_qty + '" + values.damaged_qty + "'" +
                            " where stock_gid='" + values.stock_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {

                    values.status = true;
                    values.message = "Damaged Stock Added Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Inserting Records into Damaged Details";
                }



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting Damaged Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }



        public void DaGetproductUomName( MdlImsTrnStockSummary values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select productuom_gid,productuom_name from pmr_mst_tproductuom ";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetUomList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetUomList
                        {
                        
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                                              
                         


                        });
                        values.GetUomList = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Uom Name !";
            }

        }

        //public void DaGetProductSplitSummary(string stock_gid, MdlImsTrnStockSummary values)
        //{
        //    try
        //    {
        //        objdbconn.OpenConn();
        //        msSQL = " select sum(a.stock_qty+a.amend_qty-a.issued_qty-a.damaged_qty-a.transfer_qty) as stock_balance," +
        //                " b.product_code,b.product_name,c.productgroup_name,d.productuom_name " +
        //                " from ims_trn_tstock a 	left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
        //                " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid  left join pmr_mst_tproductuom d " +
        //                 " on a.uom_gid = d.productuom_gid left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid left join " +
        //                 " pmr_mst_tproducttype f on b.producttype_gid = f.producttype_code   where a.stock_gid = '" + stock_gid + "' group by a.stock_gid ";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);

        //        var getModuleList = new List<Getproductsplit>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new Getproductsplit
        //                {
        //                    product_name = dt["product_name"].ToString(),
        //                    product_code = dt["product_code"].ToString(),
        //                    productuom_name = dt["productuom_name"].ToString(),
        //                    productgroup_name = dt["productgroup_name"].ToString(),
        //                    stock_balance = dt["stock_balance"].ToString(),
        //                });
        //                values.Getproductsplit = getModuleList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while loading Product Name !";
        //    }

        //}



        public void DaPostSplitstock (string user_gid, PostSplitstock values)
        {
            try
            {



                msGetStockGID = objcmnfunctions.GetMasterGID("ISTP");
                if (msGetStockGID == "E")
                {
                    values.status = true;
                    values.message = "Create a Sequence Code ISTP for Stock Details Table";
                }
                msGetStockGID1 = objcmnfunctions.GetMasterGID("ISKP");
                if (msGetStockGID1 == "E")
                {
                    values.status = true;
                    values.message = "Create a Sequence Code ISKP for Stock Details Table";
                }


                msSQL = " insert into ims_trn_tstockdtl(" +
                                " stockdtl_gid," +
                                " stock_gid," +
                                " branch_gid," +
                                " product_gid," +
                                " uom_gid," +
                                " issued_qty," +
                                " amend_qty," +
                                " damaged_qty," +
                                " adjusted_qty," +
                                " transfer_qty," +
                                " return_qty," +
                                " reference_gid," +
                                " stock_type," +
                                " remarks," +
                                " created_by," +
                                " created_date," +
                                " display_field" +
                                " ) values ( " +
                                "'" + msGetStockGID + "'," +
                                "'" + values.stock_gid + "'," +
                                "'" + values.branch_gid + "'," +
                                "'" + values.product_gid + "'," +
                                 "'" + values.uom_gid + "'," +
                                "'" + values.split_qty + "'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'" + msGetStockGID1 + "'," +
                                 "'Split Product Issued'," +
                                   "'" + values.remarks + "'," +
                                  "'" + user_gid + "'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'')";


                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " insert into ims_trn_tstock (" +
                                    " stock_gid," +
                                    " branch_gid," +
                                    " location_gid," +
                                    " bin_gid," +
                                    " product_gid," +
                                    " uom_gid," +
                                    " stock_qty," +
                                    " grn_qty," +
                                    " rejected_qty," +
                                    " stocktype_gid," +
                                    " reference_gid," +
                                    " stock_flag," +
                                    " remarks," +
                                    " created_by," +
                                    " created_date," +
                                    " issued_qty," +
                                    " amend_qty," +
                                    " damaged_qty" +
                                    " )values(" +
                                    " '" + msGetStockGID1 + "'," +
                                    " '" + values.branch_gid + "'," +
                                    " '" + values.location_gid + "'," +
                                    " '" + values.bin_gid + "'," +
                                    " '" + values.product_gid + "'," +
                                    " '" + values.uom_gid + "'," +
                                    "'" + values.income_qty + "', " +
                                    "'" + values.income_qty + "', " +
                                    " '0'," +
                                    " 'SY0905270002'," +
                                    " '" + values.grn_gid + "'," +
                                    " 'Y'," +
                                    " 'From Split'," +
                                    " '" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    " '0'," +
                                    " '0'," +
                                    " '0'" +
                                    ")";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }

                if (mnResult == 1)
                {

                    msSQL = " update ims_trn_tstock set " +
                            " issued_qty=issued_qty + '" + values.split_qty + "'" +
                            " where stock_gid='" + values.stock_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {

                    values.status = true;
                    values.message = "Split Stock Added Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Inserting Records into Damaged Details";
                }



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting Damaged Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

    }


}