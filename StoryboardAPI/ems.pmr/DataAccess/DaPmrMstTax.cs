using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.pmr.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
//using OfficeOpenXml;
using System.Configuration;
using System.IO;
//using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using static ems.pmr.Models.pmrtax_list;


namespace ems.pmr.DataAccess
{
    public class DaPmrMstTax
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetTaxSummary(MdlPmrMstTax values)
        {
            try
            {

                msSQL = " SELECT COUNT(DISTINCT c.product_gid) AS count, a.tax_gid,a.tax_prefix, a.tax_name, a.percentage,g.taxsegment_name,a.taxsegment_gid, " +
                        " a.split_flag, CASE WHEN a.active_flag = 'Y' THEN 'YES' ELSE 'NO' END as active_flag, " +
                        " CONCAT(b.user_firstname, ' ', b.user_lastname) as created_by, DATE_FORMAT(a.created_date, '%d-%m-%Y')  as created_date  " +
                        " FROM acp_mst_ttax a left JOIN adm_mst_tuser b ON b.user_gid = a.created_by " +
                        " left join acp_mst_ttaxsegment g on g.taxsegment_gid=a.taxsegment_gid " +
                        " left JOIN acp_mst_ttaxsegment2product c ON c.tax_gid = a.tax_gid where a.reference_type='Vendor' GROUP BY " +
                        " a.tax_gid, a.tax_name, a.percentage, a.split_flag, a.active_flag, b.user_firstname, b.user_lastname, a.created_date" +
                        "  ORDER BY  a.percentage ASC ,a.tax_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<pmrtax_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new pmrtax_list
                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_prefix = dt["tax_prefix"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            percentage = dt["percentage"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            count = dt["count"].ToString(),
                        });
                        values.pmrtax_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting Tax summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetTaxSegmentdropdown(MdlPmrMstTax values)
        {
            try
            {
                msSQL = "select taxsegment_name ,taxsegment_gid from acp_mst_ttaxsegment where active_flag='Y' and reference_type='Vendor'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<TaxSegmentdtl_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TaxSegmentdtl_list
                        {

                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),


                        });
                        values.TaxSegmentdtl_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public string DaGetTaxName(MdlPmrMstTax values, string tax_gid)
        {
            string tax_name = "";
            try
            {

                msSQL = "SELECT tax_name,percentage FROM acp_mst_ttax WHERE tax_gid='" + tax_gid + "' and reference_type='Vendor'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<smrtax_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new smrtax_list
                        {

                            tax_name = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString(),


                        });
                        values.smrtax_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return tax_name;
        }
        public string DaGetAssignedProductCount(MdlPmrMstTax values, string tax_gid)
        {
            string tax_name = "";
            try
            {

                msSQL = "select a.tax_name, b.product_name, c.taxsegment_gid,c.taxsegment_name from acp_mst_ttaxsegment2product a"+
                         "  join pmr_mst_tproduct b on b.product_gid=a.product_gid"+
                          " join acp_mst_ttaxsegment c on c.taxsegment_gid=a.taxsegment_gid  where a.tax_gid='" + tax_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<assignedProductCount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new assignedProductCount_list
                        {

                            tax_name = dt["tax_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),


                        });
                        values.assignedProductCount_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return tax_name;
        }


        public void DaPostTax(string user_gid, pmrtax_list values)
        {
            try
            {
                msSQL = " select tax_prefix from acp_mst_ttax where tax_prefix='" + values.tax_prefix + "'and reference_type='Vendor'";
                string Tax_Prefix = objdbconn.GetExecuteScalar(msSQL);
                if(Tax_Prefix != "" ) 
                {
                    values.status = false;
                    values.message = "Tax Prefix already exist";
                    return;
                }

                msSQL = " select tax_name from acp_mst_ttax where tax_name='" + values.tax_name + "' and reference_type='Vendor'";
                string Tax_Name = objdbconn.GetExecuteScalar(msSQL);
                if (Tax_Name == "")
                {
                    msGetGid = objcmnfunctions.GetMasterGID("STXM");
              
                    msSQL = " insert into acp_mst_ttax(" +
                            " tax_gid," +
                           " taxsegment_gid," +
                            " tax_prefix," +
                            " tax_name," +
                            " reference_type," +
                            " percentage," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid + "',";
                    if (values.tax_segment == null || values.tax_segment == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_segment + "',";
                    }
                    if (values.tax_prefix == null || values.tax_prefix == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_prefix.Replace("'", "\\\'")+ "',";
                    }
                    if (values.tax_prefix == null || values.tax_prefix == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_prefix.Replace("'", "\\\'") + "',";
                    }
                    msSQL += "'Vendor'," +
                         "'" + values.percentage + "'," +
                         "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Tax Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Tax";
                }
                }
                else
                {
                    values.status = false;
                    values.message = "Tax Name already exist";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Adding Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaUpdatedTax(string user_gid, pmrtax_list values)
        {
            try
            {
                msSQL = "select taxsegment_gid from acp_mst_ttaxsegment where taxsegment_name = '" + values.taxsegmentedit + "' and reference_type='Vendor' ";
                string lstaxsegment = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update  acp_mst_ttax set " +
                       " tax_prefix = '" + values.taxedit_prefix + "'," +
                       " tax_name  = '" + values.taxedit_name + "'," +
                       " percentage  = '" + values.editpercentage + "'," +
                       " taxsegment_gid  = '" + lstaxsegment + "'," +
                       " updated_by = '" + user_gid + "'," +
                       " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where tax_gid='" + values.tax_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1) { 
                msSQL = "update acp_mst_ttaxsegment2product set taxsegment_gid='" + lstaxsegment + "'where tax_gid='" + values.tax_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                    {

                        values.status = true;
                        values.message = "Tax Updated Successfully";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Tax";
                    }

                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DadeleteTaxSummary(string tax_gid, pmrtax_list values)
        {
            try
            {
                msSQL = "select * from acp_mst_ttaxsegment2product where tax_gid = '" + tax_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    values.status = false;
                    values.message = "Tax is mapped to the product";
                    objOdbcDataReader.Close();
                }
                else
                {

                    msSQL = "  delete from acp_mst_ttax where tax_gid='" + tax_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Tax Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Tax";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public string GetTaxSegment2ProductSummary(string tax_gid, string taxsegment_gid, MdlPmrMstTax values)
        {
            string tax_name = "";
            try
            {
                msSQL = "  SELECT   a.product_name,   a.customerproduct_code,    a.product_code, a.product_desc  , a.cost_price,    a.mrp_price,    b.taxsegment_gid," +
                    "    a.product_gid,    c.productgroup_gid,    c.productgroup_name,    c.productgroup_code,    a.product_price,    d.productuom_name," +
                    "   g.productuomclass_name FROM     pmr_mst_tproduct a      LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid  " +
                    "    LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.productuom_gid      LEFT JOIN pmr_mst_tproductuomclass g ON g.productuomclass_gid = a.productuomclass_gid  " +
                    "    LEFT JOIN acp_mst_ttaxsegment2product b ON a.product_gid = b.product_gid AND b.taxsegment_gid = '" + taxsegment_gid + "'  AND b.tax_gid = '" + tax_gid + "' WHERE   b.product_gid IS NULL GROUP BY " +
                    "   a.product_name,   a.customerproduct_code,  a.product_code,  a.cost_price,  a.mrp_price,  b.taxsegment_gid,  a.product_gid,  c.productgroup_gid,   c.productgroup_name," +
                    "  c.productgroup_code, a.product_price, d.productuom_name,  g.productuomclass_name; ";
                //"   where z.tax_gid != '" + tax_gid + "' and z.taxsegment_gid != '" + taxsegment_gid + "' OR z.tax_gid IS NULL OR z.tax_gid = '' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<unmappedproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new unmappedproduct_list
                        {

                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),

                        });
                        values.unmappedproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return tax_name;
        }


        public void DaPostMappedProducts(string user_gid, mapproduct_lists values)
        {
            try
            {
                msSQL = "select tax_prefix from acp_mst_ttax where tax_gid = '" + values.tax_gid + "'";
                string lstaxname = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select percentage from acp_mst_ttax where tax_gid = '" + values.tax_gid + "'";
                string lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);

                for (int i = 0; i < values.unmappedproduct_list.ToArray().Length; i++)

                {

                    decimal percentage = Convert.ToDecimal(lstaxpercentage);

                    msSQL = "SELECT mrp_price FROM pmr_mst_tproduct WHERE product_gid = '" + values.unmappedproduct_list[i].product_gid + "'";
                    decimal lsmrp_price = Convert.ToDecimal(objdbconn.GetExecuteScalar(msSQL));
                    decimal taxAmount = (lsmrp_price * percentage) / 100;
                    string msGetGid1 = objcmnfunctions.GetMasterGID("TS2P");
                    msSQL = "INSERT INTO acp_mst_ttaxsegment2product (" +
                                                    "taxsegment2product_gid, " +
                                                    "taxsegment_gid, " +
                                                    "product_gid, " +
                                                    "tax_gid, " +
                                                    "tax_name, " +
                                                    "tax_percentage, " +
                                                    "tax_amount, " +
                                                    "created_by, " +
                                                    "created_date) " +
                                                    "VALUES (" +
                                                    "'" + msGetGid1 + "', " +
                                                    "'" + values.taxsegment_gid + "', " +
                                                    "'" + values.unmappedproduct_list[i].product_gid + "', " +
                                                    "'" + values.tax_gid + "', " +
                                                    "'" + lstaxname + "', " +
                                                    "'" + percentage + "', " +
                                                    "'" + taxAmount + "', " +
                                                    "'" + user_gid + "', " +
                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Tax  assign to Product !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while occured assigning Product to tax ";
                    }
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occurred while Mapping product !! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }



        public string DaGetTaxUnmapping(string tax_gid, string taxsegment_gid, MdlPmrMstTax values)
        {
            string tax_name = "";
            try
            {
                msSQL = " SELECT a.product_name,a.customerproduct_code, a.product_code,a.product_desc ,a.cost_price, a.mrp_price,b.taxsegment_gid, a.product_gid," +
                        "  c.productgroup_gid, c.productgroup_name, productgroup_code, a.product_price, d.productuom_name, g.productuomclass_name FROM pmr_mst_tproduct a" +
                        "  LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid  LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.productuom_gid " +
                        "  LEFT JOIN pmr_mst_tproductuomclass g ON g.productuomclass_gid = a.productuomclass_gid  LEFT JOIN acp_mst_ttaxsegment2product b ON a.product_gid = b.product_gid " +
                        " where b.taxsegment_gid ='" + taxsegment_gid + "' and b.tax_gid ='" + tax_gid + "' group by b.product_gid ";             

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<unmappedproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new unmappedproduct_list
                        {

                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_code = dt["productgroup_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            sku = dt["customerproduct_code"].ToString(),

                        });
                        values.unmappedproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return tax_name;
        }


        public void DaPostUnMappedProducts(string user_gid, mapproduct_lists values)
        {
            try
            {


                for (int i = 0; i < values.unmappedproduct_list.ToArray().Length; i++)

                {


                    msSQL = " delete from acp_mst_ttaxsegment2product where tax_gid='" + values.tax_gid + "' and taxsegment_gid='" + values.taxsegment_gid + "'  and product_gid='" + values.unmappedproduct_list[i].product_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Unassign Tax to Product Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while occured assigning Product to tax ";
                    }
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occurred while Mapping product !! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }




    }
}