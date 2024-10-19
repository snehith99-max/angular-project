using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.sales.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static ems.sales.Models.MdlSmrMstPricesegmentSummary;
using System.Security.Cryptography;
using static OfficeOpenXml.ExcelErrorValue;


namespace ems.sales.DataAccess
{
    public class DaSmrMstPricesegmentSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, msSTOCKGetGID, msSTOCKHISTORYGetGID, lsold_price, lsentity_code, msGetGID1, lsdesignation_code, msGetGID, lsCode, msGetGid, msGetGid1, msGetGid2, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        //summary
        public void DaGetSmrMstPricesegmentSummary(MdlSmrMstPricesegmentSummary values)
        {
            try
            {
                
                msSQL = " select a.pricesegment_prefix,a.pricesegment_gid,a.pricesegment_name,a.pricesegment_code,a.discount_percentage , " +
                        "  (select count(y.product_gid) from smr_trn_tpricesegment2product y where y.pricesegment_gid = a.pricesegment_gid)  as productcount," +
                         "  (select count(x.customer_gid) from smr_trn_tpricesegment2customer x where x.pricesegment_gid=a.pricesegment_gid) as customer_count"+
                          " from smr_trn_tpricesegment  a" +
                         " group by a.pricesegment_gid  order by a.pricesegment_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpricesegment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpricesegment_list
                        {
                            pricesegment_prefix = dt["pricesegment_prefix"].ToString(),
                            pricesegment_gid = dt["pricesegment_gid"].ToString(),
                            pricesegment_code = dt["pricesegment_code"].ToString(),
                            pricesegment_name = dt["pricesegment_name"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            productcount = dt["productcount"].ToString(),
                            customercount = dt["customer_count"].ToString()

                        });
                        values.pricesegment_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Loading Pricesegment Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaPostPostPriceSegment(string user_gid, Getpricesegment_list values)
        {
            try
            {
               

                msGetGid = objcmnfunctions.GetMasterGID("SPRS");
                msSQL = " select * from smr_trn_tpricesegment where pricesegment_name= '" + values.pricesegment_name.Replace("'", "\\\'") + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Price Segment Name Already Exist";
                    return;
                }
                msSQL = " select pricesegment_prefix from smr_trn_tpricesegment where pricesegment_prefix='" + values.pricesegment_prefix.Replace("'", "\\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows ==true)
                {
                    values.status = false;
                    values.message = "Price Segment Prefix already exist";
                        objOdbcDataReader.Close();
                                    
                    return;
                }
                
                else
                {
                    msSQL = " insert into smr_trn_tpricesegment ( " +
                   " pricesegment_prefix, " +
                  " pricesegment_gid," +
                  " pricesegment_code, " +
                  " pricesegment_name, " +
                  " discount_percentage, " +
                  " created_by, " +
                  " created_date" +
                  " ) values( " +
                  " '" + (String.IsNullOrEmpty(values.pricesegment_prefix) ? values.pricesegment_prefix : values.pricesegment_prefix.Replace("'", "\\'")) + "', " +
                  " '" + msGetGid + "'," +
                  " '" + (String.IsNullOrEmpty(values.pricesegment_code) ? values.pricesegment_code : values.pricesegment_code.Replace("'", "\\'")) + "'," +
                  " '" + (String.IsNullOrEmpty(values.pricesegment_name) ? values.pricesegment_name : values.pricesegment_name.Replace("'", "\\'")) + "'," +
                  " '" + values.discount_percentage + "'," +
                  "'" + user_gid + "'," +
                  "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Price Segment Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Price Segment";

                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting Price Segment ummary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaUpdatePriceSegment(string user_gid, Getpricesegment_list values)
        {
            try
            {
                string productGid = "";
                string lslocaldiscountpercentage = "";
                msSQL = "select discount_percentage from smr_trn_tpricesegment where " +
                    "pricesegment_gid = '" + values.pricesegment_gid + "'";
                lslocaldiscountpercentage = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " update  smr_trn_tpricesegment   set " +
                        "pricesegment_prefix  = '" + (String.IsNullOrEmpty(values.editpricesegment_prefix) ? values.editpricesegment_prefix : values.editpricesegment_prefix.Replace("'", "\\'")) + "'," +
                  " pricesegment_code  = '" + (String.IsNullOrEmpty(values.pricesegmentedit_code) ? values.pricesegmentedit_code : values.pricesegmentedit_code.Replace("'", "\\'")) + "'," +
                  " discount_percentage  = '" + values.editdiscount_percentage + "'," +
                  " pricesegment_name = '" + (String.IsNullOrEmpty(values.pricesegmentedit_name) ? values.pricesegmentedit_name : values.pricesegmentedit_name.Replace("'", "\\'")) + "'," +
                  " updated_by = '" + user_gid + "'," +
                  " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where pricesegment_gid ='" + values.pricesegment_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if(lslocaldiscountpercentage != values.discount_percentage)
                {
                    msSQL = "select distinct product_gid from smr_trn_tpricesegment2product" +
                           " where pricesegment_gid = '" + values.pricesegment_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    values.products = new List<string>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            productGid = dt["product_gid"].ToString();
                            values.products.Add(productGid);

                        }


                    }
                    for (int i = 0; i < values.products.Count; i++)
                    {
                        msSQL = " select mrp_price from pmr_mst_tproduct  " +
                            " where product_gid ='" + values.products[i] + "'";
                        string lsmrpprice = objdbconn.GetExecuteScalar(msSQL);
                        double lsdiscountamount = 0;
                        double lsproductprice = 0;
                        double lsdoublemrpprice = 0;
                        string lsdiscountpercentage = values.editdiscount_percentage;
                        lsdoublemrpprice = double.Parse(lsmrpprice);
                        lsdiscountamount = (double.Parse(lsdiscountpercentage) * lsdoublemrpprice) / 100;
                        lsproductprice = Math.Round(lsdoublemrpprice - lsdiscountamount, 2);


                        msSQL = "update smr_trn_tpricesegment2product set product_price = '" + lsproductprice + "'," +
                            " discount_amount  = '" + lsdiscountamount + "'," +
                            " discount_percentage = '"+values.editdiscount_percentage+"'" +
                            " where pricesegment_gid = '" + values.pricesegment_gid + "'" +
                            " and product_gid = '" + values.products[i] + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }


                    if (mnResult != 0)
                    {

                        values.status = true;
                        values.message = "Price Segment Updated Successfully";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Price Segment";
                    }
                
                //}
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating  Pricesegment Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DadeletePriceSegmentSummary(string pricesegment_gid, Getpricesegment_list values)
        {
            try
            {
                msSQL = "select pricesegment_gid from smr_trn_tpricesegment2product where pricesegment_gid ='" + pricesegment_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "If the price segment is already assigned to a products, it cannot be deleted";
                    return;

                }
                objOdbcDataReader.Close();
                msSQL = "select pricesegment_gid from smr_trn_tpricesegment2customer where pricesegment_gid ='" + pricesegment_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "If the price segment is already assigned to a Customers, it cannot be deleted ";
                    return;

                }
                objOdbcDataReader.Close();


                msSQL = "  delete from smr_trn_tpricesegment  where pricesegment_gid='" + pricesegment_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Price Segment Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Price Segment";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deletting Price Segment  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaGetPricesegmentgrid(string pricesegment_gid, MdlSmrMstPricesegmentSummary values)
        {
            try
            {
               
                msSQL = " select a.customer_gid,a.pricesegment_gid,a.customer_name,b.customer_id, " +
                             " concat(d.customercontact_name,'/',d.mobile,'/',d.email) as contact_details, " +
                             " case when c.region_name is null then concat(b.customer_city,'',b.customer_state) " +
                             " else Concat(c.region_name,'',b.customer_city,'',b.customer_state) end as region_name " +
                             " from smr_trn_tpricesegment2customer a " +
                             " left join crm_mst_tcustomer b on a.customer_gid=b.customer_gid " +
                             " left join crm_mst_tregion c on b.customer_region =c.region_gid " +
                             " left join crm_mst_tcustomercontact d on b.customer_gid=d.customer_gid where pricesegment_gid='" + pricesegment_gid + "' " +
                             " and b.status='Active' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpricesegmentgrid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpricesegmentgrid_list
                        {
                            pricesegment_gid = dt["pricesegment_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_id = dt["customer_id"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),

                        });
                        values.pricesegmentgrid_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Pricesegmentgrid !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        //product group drodown
        public void DaGetSmrGroupDtl(MdlSmrMstPricesegmentSummary values)
        {
            try
            {
                
                msSQL = " select distinct productgroup_gid,concat(productgroup_code,'|',productgroup_name) as productgroup_name " +
                       " from pmr_mst_tproductgroup where delete_flag='N' order by productgroup_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductGroupDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductGroupDropdown
                        {
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.GetSmrGroupDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Group  Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        //product Name drodown
        public void DaGetSmrProductDtl(MdlSmrMstPricesegmentSummary values)
        {
            try
            {
               
                msSQL = " select distinct a.product_gid,concat(product_code,'|',product_name) as product_name from pmr_mst_tproduct a where delete_flag='N' " +
                    " order by product_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductNameDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductNameDropdown
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                        });
                        values.GetSmrProductDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        //product Unit drodown
        public void DaGetSmrUnitDtl(MdlSmrMstPricesegmentSummary values)
        {
            try
            {
               
                msSQL = " select distinct productuom_gid,productuom_name from pmr_mst_tproductuom where delete_flag='N' " +
                    " order by productuom_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductUnitDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductUnitDropdown
                        {
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                        });
                        values.GetSmrUnitDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Uom Name  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        //Product Assign
        public void DaGetSmrMstProductAssignSummary(MdlSmrMstPricesegmentSummary values,string pricesegment_gid)
        {
            try
            {

                //msSQL = "select a.product_name,d.mrp,d.discount_amount,d.product_price,a.product_code,d.pricesegment_gid, d.pricesegment_name,a.customerproduct_code, " +
                //        "  c.productgroup_gid,a.product_gid,c.productgroup_name,b.productuom_name,  b.productuom_gid, a.created_date from pmr_mst_tproduct a   " +
                //        "  left join pmr_mst_tproductuom b on a.productuom_gid=b.productuom_gid " +
                //        " left join pmr_mst_tproductgroup c on  a.productgroup_gid=c.productgroup_gid " +
                //        " left join smr_trn_tpricesegment2product d on a.product_gid=d.product_gid " +
                //        " where d.pricesegment_gid='"+ pricesegment_gid + "' group by a.product_gid order by a.created_date desc";

                //msSQL = "SELECT a.product_name,a.mrp_price,a.product_code,a.product_gid," +
                //   "  d.productgroup_name, a.customerproduct_code , a.product_desc, " +
                //   " c.productuom_name " +
                //   " FROM pmr_mst_tproduct a " +
                //   " LEFT JOIN pmr_mst_tproductuom c ON a.productuom_gid = c.productuom_gid " +
                //   " LEFT JOIN pmr_mst_tproductgroup d ON a.productgroup_gid = d.productgroup_gid " +

                //   " left join smr_trn_tpricesegment2product h on h.product_gid = a.product_gid " +
                //   " where a.product_gid NOT in (select product_gid " +
                //   " from smr_trn_tpricesegment2product where pricesegment_gid = '" + pricesegment_gid + "')";
                string sqltype = "unassignsummary";

                msSQL = " call smr_trn_sppriceproductsummary('"+ sqltype + "','"+pricesegment_gid+"')";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProduct>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProduct
                        {
                            
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            mrp = dt["mrp_price"].ToString()

                        });
                        values.productgroup_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading  ProductAssignSummary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetSmrMstProductunAssignSummary(MdlSmrMstPricesegmentSummary values, string pricesegment_gid)
        {
            try
            {


                //msSQL = "SELECT a.product_name,a.mrp_price,a.product_code,h.pricesegment2product_gid , a.product_gid," +
                //   "  d.productgroup_name, a.customerproduct_code , a.product_desc, " +
                //   " c.productuom_name " +
                //   " FROM pmr_mst_tproduct a " +
                //   " LEFT JOIN pmr_mst_tproductuom c ON a.productuom_gid = c.productuom_gid " +
                //   " LEFT JOIN pmr_mst_tproductgroup d ON a.productgroup_gid = d.productgroup_gid " +

                //   " left join smr_trn_tpricesegment2product h on h.product_gid = a.product_gid " +
                //   " where a.product_gid in (select product_gid " +
                //   " from smr_trn_tpricesegment2product where pricesegment_gid = '" + pricesegment_gid + "')";
                string sqltype = "assignsummary";
                msSQL = "call smr_trn_sppriceproductsummary('" + sqltype + "','" + pricesegment_gid + "')";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<unassignproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new unassignproduct_list
                        {

                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            pricesegment2product_gid = dt["pricesegment2product_gid"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            mrp = dt["mrp_price"].ToString()

                        });
                        values.unassignproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading  ProductAssignSummary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // on change
        public void DaGetOnChangeProductName(string product_gid, MdlSmrMstPricesegmentSummary values)
        {
            try
            {
               
                if (product_gid != null)
                {
                    msSQL = " Select distinct a.product_gid,a.product_name,a.product_code,a.productgroup_gid,d.productgroup_name,c.productuom_gid, c.productuom_name " +
                            " from pmr_mst_tproduct a " +
                            " left join pmr_mst_tproductuom c on a.productuom_gid= c.productuom_gid " +
                            " left join pmr_mst_tproductgroup d on a.productgroup_gid=d.productgroup_gid " +
                            " where a.product_gid = '" + product_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetProductName>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetProductName
                            {
                                product_gid = dt["product_gid"].ToString(),
                                product_name = dt["product_name"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productuom_gid = dt["productuom_gid"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),

                            });
                            values.OnChangeProductName = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception Occured while Onchange Product Name  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        // Product
        public void DaPostProduct(string user_gid, Getproduct_list values)
        {
            try
            {
                foreach (var data in values.productgroup_list)
                {
                    string lsproductname, lsproductcode, lsuom_gid, lsproductgroupcode, lscustomerproduct_code, lsproductuom_name, lsproductgroup_name, lsproduct_price;
                    msSQL = " Select distinct a.product_gid,a.product_name,a.product_code,a.mrp_price,a.productgroup_gid,a.customerproduct_code,d.productgroup_code,d.productgroup_name,c.productuom_gid, c.productuom_name " +
                              " from pmr_mst_tproduct a " +
                              " left join pmr_mst_tproductuom c on a.productuom_gid= c.productuom_gid " +
                              " left join pmr_mst_tproductgroup d on a.productgroup_gid=d.productgroup_gid " +
                              " where a.product_gid = '" + data.product_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    
                    if (objOdbcDataReader.HasRows == true)
                    {
                       
                        lsproductname = objOdbcDataReader["product_name"].ToString();
                        lsproductcode = objOdbcDataReader["product_code"].ToString();
                        lsuom_gid = objOdbcDataReader["productuom_gid"].ToString();
                        lsproductgroup_name = objOdbcDataReader["productgroup_name"].ToString();
                        lsproductuom_name = objOdbcDataReader["productgroup_name"].ToString();
                        lscustomerproduct_code = objOdbcDataReader["customerproduct_code"].ToString();
                        lsproductgroupcode = objOdbcDataReader["productgroup_code"].ToString();
                        lsproduct_price = objOdbcDataReader["mrp_price"].ToString();
                        msSQL = " select pricesegment_name from smr_trn_tpricesegment  " +
                            " where pricesegment_gid ='" + values.pricesegment_gid + "'";
                        string lspricesegmentname = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select discount_percentage from smr_trn_tpricesegment  " +
                                " where pricesegment_gid ='" + values.pricesegment_gid + "'";
                        string lsdiscountpercentage = objdbconn.GetExecuteScalar(msSQL);
                        double lsdiscountamount = 0;
                        double lsunitprice = 0;
                        double lsdoubleproductprice = 0;
                        lsdoubleproductprice = double.Parse(lsproduct_price);
                        lsdiscountamount = (double.Parse(lsdiscountpercentage) * lsdoubleproductprice) / 100;
                        lsunitprice = lsdoubleproductprice - lsdiscountamount;


                        msGetGid2 = objcmnfunctions.GetMasterGID("SRCT");

                        msSQL = " insert into smr_trn_tpricesegment2product ( " +
                           " pricesegment2product_gid," +
                            " product_code, " +
                            " product_name," +
                            " product_gid," +
                            " pricesegment_gid," +
                            " pricesegment_name," +
                            " productuom_gid," +
                            " productuom_name," +
                            " productgroup_code," +
                            " productgroup , " +
                            " product_price, " +
                            " mrp, " +
                            " discount_percentage, " +
                            " discount_amount, " +
                            " customerproduct_code, " +
                            " created_by," +
                            " created_date " +
                           " ) values( " +
                           " '" + msGetGid2 + "', " +
                           " '" + lsproductcode + "'," +
                           " '" + lsproductname.Replace("'","").Trim() + "'," +
                           " '" + data.product_gid + "'," +
                           " '" + values.pricesegment_gid + "'," +
                           " '" + lspricesegmentname + "'," +
                           " '" + lsuom_gid + "'," +
                           " '" + lsproductuom_name + "'," +
                           " '" + lsproductgroupcode + "'," +
                           " '" + lsproductgroup_name + "'," +
                           " '" + lsunitprice + "'," +
                           " '" + lsproduct_price + "'," +
                           " '" + lsdiscountpercentage + "'," +
                           " '" + lsdiscountamount + "'," +
                           " '" + lscustomerproduct_code + "'," +
                           "'" + user_gid + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    objOdbcDataReader.Close();


                    if (mnResult != 0)
                    {

                        values.status = true;
                        values.message = "Products Assigned Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Assigning Products";
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Assigning Products !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        //Product Head
        public void DaGetSmrMstProductHead(MdlSmrMstPricesegmentSummary values,string pricesegment_gid)
        {
            try
            {
                
                msSQL = " select pricesegment_gid, pricesegment_name,concat(pricesegment_code,'  /  ',pricesegment_name) as price_details," +
                        " discount_percentage from smr_trn_tpricesegment"+
                        " where pricesegment_gid='" + pricesegment_gid + "' group by pricesegment_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpricesegment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpricesegment_list
                        {
                            pricesegment_gid = dt["pricesegment_gid"].ToString(),
                            pricesegment_name = dt["pricesegment_name"].ToString(),
                            price_details = dt["price_details"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),

                        });
                        values.pricesegment_list = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Pricesegment Name  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        public void DaGetOnChangeProduct(MdlSmrMstPricesegmentSummary values, string product_gid)
        {
            try
            {


                msSQL = " SELECT b.productgroup_name, a.product_code,a.cost_price, a.hsn_number, a.hsn_desc, c.productuom_name, a.product_price " +
                        " FROM pmr_mst_tproduct AS a LEFT JOIN pmr_mst_tproductgroup AS b ON a.productgroup_gid = b.productgroup_gid LEFT JOIN pmr_mst_tproductuom AS c ON a.productuom_gid = c.productuom_gid " +
                        " WHERE a.product_gid = '" + product_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getproductonchange>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproductonchange
                        {
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            hsn_code = dt["hsn_number"].ToString(),
                            hsn_description = dt["hsn_desc"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            unitprice = dt["cost_price"].ToString(),
                            product_price = dt["product_price"].ToString(),
                        });
                        values.Getproductonchange = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Change Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // Update

        public void DaGetUpdateProduct(string employee_gid, Getproduct_list values)

        {
            try
            {
                msSQL = " select discount_percentage from smr_trn_tpricesegment  " +
                       " where pricesegment_gid ='" + values.pricesegment_gid + "'";
                string lsdiscountpercentage = objdbconn.GetExecuteScalar(msSQL);
                double lsdiscountamount = 0;
                double lsunitprice = 0;
                lsdiscountamount = double.Parse(lsdiscountpercentage) * values.product_price;
                lsunitprice = values.product_price - lsdiscountamount;
                string lsold_price = "0.00";


                msSQL = " INSERT INTO smr_trn_tstockpricehistory( " +
                        " product_gid, " +
                        " customerproduct_code," +
                        " old_price, " +
                        " stock_gid, " +
                        " updated_price, " +
                        " updated_by ," +
                        " updated_date " +
                        " ) VALUES ( " +
                        " '" + values.product_gid + "', " +
                        " '" + values.customerproduct_code + "'," +
                        " ' " + values.originalProductPrice.Replace(",", "").Trim() + "', " +
                        " '" + values.stock_gid + "', " +
                        " '" + values.product_price + "', " +
                        " '" + employee_gid + "', " +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ) ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msSQL = " update smr_trn_tpricesegment2product set " +
                        " product_price ='" + values.product_price + "', " +
                        " customerproduct_code='" + values.customerproduct_code + "', " +
                       " updated_by = '" + employee_gid + "'," +
                       " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                      " where product_gid='" + values.product_gid + "' and productuom_gid='" + values.productuom_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update ims_trn_tstock set unit_price ='" + values.product_price + "' " +
                              " where stock_gid= '" + values.stock_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
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
            catch (Exception ex)
            {
                values.message = "Exception occured while Update Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Un Assign
        public void DaGetUnAssignProduct(string product_gid, GetProduct values)
        {
            try
            {
               
                msSQL = " update  smr_trn_tpricesegment2product set unassigned_flag='Y' where product_gid='" + product_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Un Assigned Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Un Assigning Product ";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Un Assign Product  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        //Assign - Unassign
        public void DaGetUnassignedlists(string pricesegment_gid, MdlSmrMstPricesegmentSummary values)
        {
            try
            {
                
                msSQL = " select a.customer_gid,a.customer_name from crm_mst_tcustomer a where a.customer_gid not in" +
                    " (select distinct b.customer_gid from smr_trn_tpricesegment2customer b" +
                    " where 1=1 )  and a.status='Active' and a.customer_name!='' order by a.customer_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetUnassignedlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetUnassignedlists
                        {


                            employee_gid = dt["customer_gid"].ToString(),
                            employee_name = dt["customer_name"].ToString(),




                        });
                        values.GetUnassignedlists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Employee Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetAssignedlists(string pricesegment_gid, MdlSmrMstPricesegmentSummary values)
        {
            try
            {
                

                msSQL = " select distinct customer_gid,customer_name from smr_trn_tpricesegment2customer a " +
                    " where pricesegment_gid = '" + pricesegment_gid + "' order by customer_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAssignedlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAssignedlists
                        {
                            employee_gid = dt["customer_gid"].ToString(),
                            employee_name = dt["customer_name"].ToString(),
                        });
                        values.GetAssignedlists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Assigned Customer  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetUnassigned(MdlSmrMstPricesegmentSummary values)
        {
            try
            {
               
                msSQL = " select a.customer_gid,a.customer_name from crm_mst_tcustomer a where a.customer_gid not in" +
                   " (select distinct b.customer_gid from smr_trn_tpricesegment2customer b" +
                   " where 1=1 )  and a.status='Active' order by a.customer_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetUnassigned>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetUnassigned
                        {
                            employee_gid = dt["customer_gid"].ToString(),
                            employee_name = dt["customer_name"].ToString(),
                        });
                        values.GetUnassigned = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while UnAssigned Customer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaPostAssignedlist(string user_gid, campaignassign_list values)
        {
            try
            {
               
                if (values.campaignunassign != null || values.pricesegment_gid != null || values.pricesegment_gid != "")
                {
                    msSQL = " select pricesegment_name from smr_trn_tpricesegment " +
                             " where pricesegment_gid='" + values.pricesegment_gid + "'";
                    string lspricesegmentname= objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "DELETE FROM smr_trn_tpricesegment2customer WHERE pricesegment_gid = '" + values.pricesegment_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    for (int i = 0; i < values.campaignunassign.ToArray().Length; i++)
                    {



                        msGetGID = objcmnfunctions.GetMasterGID("VPDC");
                        //msGetGID1 = objcmnfunctions.GetMasterGID("SPRS");
                        msSQL = " insert into smr_trn_tpricesegment2customer(" +
                                    " pricesegment2customer_gid, " +
                                    " pricesegment_gid, " +
                                    " pricesegment_name," +
                                    " customer_gid, " +
                                    " customer_name, " +
                                    " created_by, " +
                                    " created_date" +
                                    " )values( " +
                                    "'" + msGetGID + "', " +
                                    "'" + values.pricesegment_gid + "'," +
                                    "'" + lspricesegmentname + "'," +
                                    "'" + values.campaignunassign[i]._id + "', " +
                                    "'" + values.campaignunassign[i]._name + "', " +
                                    "'" + user_gid + "', " +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = false;
                            values.message = "Customer assigned to PriceSegment successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Customer Assign";
                        }
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Customer Assign";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Customer Assigne !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaPostUnassignedlist(string user_gid, campaignassign_list values)
        {
            try
            {
               
                for (int i = 0; i < values.campaignassign.ToArray().Length; i++)
                {

                    msSQL = " delete from smr_trn_tpricesegment2customer " +
                            " where pricesegment_gid = '" + values.pricesegment_gid + "' and customer_gid = '" + values.campaignassign[i]._id + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = false;
                        values.message = "Customer Unassigned to PriceSegment Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Customer Unassign";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Customer Unassign !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }
        public void daassignedpricesegmentproducts(string pricesegment_gid , MdlSmrMstPricesegmentSummary values)
        {
            msSQL = "select product_name , product_code from smr_trn_tpricesegment2product where pricesegment_gid = '" + pricesegment_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<pricesegmentproduct_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new pricesegmentproduct_list
                    {
                        product_name = dt["product_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                    });
                    values.pricesegmentproduct_list = getModuleList;
                }
            }
            dt_datatable.Dispose();


        }
        public void daassignedpricesegmentscustomers(string pricesegment_gid , MdlSmrMstPricesegmentSummary values)
        {
            msSQL = "select a.pricesegment_gid ,b.customer_name,b.customer_id from smr_trn_tpricesegment2customer a " +
                "left join crm_mst_tcustomer  b on b.customer_gid = a.customer_gid " +
                "where a.pricesegment_gid = '"+pricesegment_gid+"'";
            dt_datatable = objdbconn.GetDataTable (msSQL);
            var getmodulelist = new List<pricesegmentcustomer_list>();
            if(dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new pricesegmentcustomer_list
                    {
                        customer_name = dt["customer_name"].ToString(),
                        customer_id = dt["customer_id"].ToString(),
                    });
                    values.pricesegmentcustomer_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

        // Assign Summary
        public void Daassigncustomer(string pricesegment_gid, MdlSmrMstPricesegmentSummary values)
        {

            try
            {

                //msSQL = "SELECT DISTINCT UCASE(a.customer_id) AS customer_id,a.customer_gid,z.pricesegment_name,a.customer_name," +
                //    " CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details, a.salesperson_gid," +
                //    " h.user_firstname as salespersonname,"+
                //    " d.region_name,a.customer_country, (SELECT CONCAT(DATEDIFF(CURDATE(), MIN(created_date)), ' days') FROM crm_mst_tcustomer WHERE customer_gid = a.customer_gid) as customer_since " +
                //    " FROM crm_mst_tcustomer a " +
                //    " LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                //    " LEFT JOIN crm_mst_tregion d ON a.customer_region = d.region_gid " +
                //    " LEFT JOIN smr_trn_tpricesegment z ON a.pricesegment_gid = z.pricesegment_gid " +
                //    " left join adm_mst_tuser h on h.user_gid = a.salesperson_gid " +
                //    " where  a.status = 'Active' and a.pricesegment_gid  is  null";

                msSQL = "call smr_trn_sppriceunassignsummary";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<pricesegmentassign>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new pricesegmentassign
                        {
                            customer_id = dt["customer_id"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            customer_country = dt["country_name"].ToString(),
                            customer_since = dt["customer_since"].ToString(),
                            salesperson_gid = dt["salespersonname"].ToString(),

                        });
                        values.pricesegmentassign = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void Daassigncustomerheadercount(string pricesegment_gid, MdlSmrMstPricesegmentSummary values)
        {
            msSQL = "select pricesegment_name ,discount_percentage from smr_trn_tpricesegment where pricesegment_gid ='" + pricesegment_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<pricesegmentheader_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new pricesegmentheader_list
                    {
                        pricesegment_count = dt["discount_percentage"].ToString(),
                        pricesegment_name = dt["pricesegment_name"].ToString(),
                        
                    });
                    values.pricesegmentheader_list = getModuleList;
                }
            }
            dt_datatable.Dispose();

        }


        // unAssign Summary
        public void Daunassigncustomer(string pricesegment_gid, MdlSmrMstPricesegmentSummary values)
        {

            try
            {

                //msSQL = "SELECT DISTINCT UCASE(a.customer_id) AS customer_id,a.customer_gid,z.pricesegment_name,a.customer_name," +
                //    " CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details, a.salesperson_gid," +
                //    " h.user_firstname as salespersonname," +
                //    " d.region_name,a.customer_country,(SELECT CONCAT(DATEDIFF(CURDATE(), MIN(created_date)), ' days') FROM crm_mst_tcustomer WHERE customer_gid = a.customer_gid) as customer_since " +
                //    " FROM crm_mst_tcustomer a " +
                //    " LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                //    " LEFT JOIN crm_mst_tregion d ON a.customer_region = d.region_gid " +
                //    " LEFT JOIN smr_trn_tpricesegment z ON a.pricesegment_gid = z.pricesegment_gid " +
                //    " left join adm_mst_tuser h on h.user_gid = a.salesperson_gid " +
                //    " where  a.status = 'Active' and a.pricesegment_gid is not null";
                msSQL = "call smr_trn_sppriceassignsummary('"+pricesegment_gid+"')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<pricesegmentunassign>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new pricesegmentunassign
                        {
                            customer_id = dt["customer_id"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            pricesegment_name = dt["pricesegment_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            customer_country = dt["country_name"].ToString(),
                            customer_since = dt["customer_since"].ToString(),
                           
                            salespersonname = dt["salespersonname"].ToString(),

                        });
                        values.pricesegmentunassign = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPricesegmentAssignSubmitCustomer(customerpricesegmentsubmitlist values, string employee_gid)
        {
            try
            {

                string lscustomer_gid;


                msSQL = " insert into smr_trn_tpricesegment2customer(" +
                                " pricesegment2customer_gid, " +
                                " pricesegment_gid, " +
                                " pricesegment_name," +
                                " customer_gid, " +
                                " customer_name, " +
                                " created_by, " +
                                " created_date" +
                                " )values ";
                        int i = 0;

                string msSQL1 = " update crm_mst_tcustomer set pricesegment_gid='" + values.pricesegment_gid + "'  where  customer_gid In(";
                        foreach (var data in values.pricesegmentassign)
                         {
                    if (i != 0)
                    {
                        msSQL += ",";
                        msSQL1 += ",";
                    }
                    i++;

                    msGetGID = objcmnfunctions.GetMasterGID("VPDC");
                    
                                 msSQL += "('" + msGetGID + "', " +
                                    "'" + values.pricesegment_gid + "'," +
                                    "'" + data.pricesegment_name + "'," +
                                    "'" + data.customer_gid + "', " +
                                    "'" + data.customer_name.Replace("'","").Trim() + "', " +
                                    "'" + employee_gid + "', " +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";


                    msSQL1 +="'" + data.customer_gid + "'";

                    if (i >= 50)
                    {
                        msSQL1 += ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        int mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL1);
                        i = 0;
                        msSQL = " insert into smr_trn_tpricesegment2customer(" +
                                " pricesegment2customer_gid, " +
                                " pricesegment_gid, " +
                                " pricesegment_name," +
                                " customer_gid, " +
                                " customer_name, " +
                                " created_by, " +
                                " created_date" +
                                " )values ";

                         msSQL1 = " update crm_mst_tcustomer set pricesegment_gid='" + values.pricesegment_gid + "'  where  customer_gid In(";


                    }

                }
                if (i >= 1) { 
                msSQL1 += ")";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                 mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL1);

                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Customer Assigned Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Assigned Customer";
                }


            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPriceSegmetUnAssignSubmit(customerpricesegmentunsubmitlist values, string employee_gid)
        {
            try
            {
                string lspricesegment_gid = values.pricesegment_gid;

                
                 int i= 0;
                    msSQL = " delete from smr_trn_tpricesegment2customer where (pricesegment_gid , customer_gid) in (";

                     string msSQL1 = " update crm_mst_tcustomer set pricesegment_gid=null  where customer_gid in (";
                   
                   
                foreach (var data in values.pricesegmentunassign)
                {
                    if (i != 0)
                    {
                        msSQL += ",";
                        msSQL1 += ",";
                    }
                    i++;
                    msSQL += "('" + lspricesegment_gid + "','" + data.customer_gid + "')";
                    msSQL1 += "'" + data.customer_gid + "'";
                    if (i >= 50)
                    {
                        msSQL += ")";
                        msSQL1 += ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        int mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL1);
                        i = 0;

                        msSQL = " delete from smr_trn_tpricesegment2customer where (pricesegment_gid , customer_gid) in (";

                        msSQL1 = " update crm_mst_tcustomer set pricesegment_gid=null  where customer_gid in (";

                    }



                }
                if (i >= 1)
                {
                    msSQL1 += ")";
                    msSQL += ")";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL1);

                }


                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Customer  unassign Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Customer Unassign";
                    }
                }

            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


    }

}
