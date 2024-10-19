using System;
using System.Collections.Generic;
using ems.sales.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System.Threading;
using System.Xml.Linq;
using CrystalDecisions.Shared.Json;

namespace ems.sales.DataAccess
{
    public class DaSmrMstTaxSummary
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid ,lsflag, msGetGid1;
        int mnResult;
        private string lsproductuomclass_gid;
        private string lsproductuomclassgid;
        private string lsproductuom_gid;
        finance_cmnfunction objfincmn = new finance_cmnfunction();

        public void DaGetTaxSummary(MdlSmrMstTaxSummary values)
        {
            try
            {

                msSQL = " SELECT COUNT(DISTINCT c.product_gid) AS count, a.tax_prefix, a.tax_gid, f.taxsegment_name, a.percentage,a.taxsegment_gid, " +
                        " a.split_flag, CASE WHEN a.active_flag = 'Y' THEN 'YES' ELSE 'NO' END as active_flag, " +
                        " CONCAT(b.user_firstname, ' ', b.user_lastname) as created_by, DATE_FORMAT(a.created_date, '%d-%m-%Y')  as created_date  " +
                        " FROM acp_mst_ttax a  LEFT JOIN adm_mst_tuser b ON b.user_gid = a.created_by " +
                        " left JOIN acp_mst_ttaxsegment2product c ON c.tax_gid = a.tax_gid " +
                        " left join acp_mst_ttaxsegment f on f.taxsegment_gid = a.taxsegment_gid where a.reference_type='Customer' GROUP BY " +
                        " a.tax_gid, a.tax_name, a.percentage, a.split_flag, a.active_flag, b.user_firstname, b.user_lastname, a.created_date" +
                        "  ORDER BY a.tax_name ASC, a.percentage ASC; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<smrtax_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new smrtax_list
                    {
                        tax_gid = dt["tax_gid"].ToString(),
                        tax_name = dt["taxsegment_name"].ToString(),
                        taxsegment_gid  = dt["taxsegment_gid"].ToString(),
                        percentage = dt["percentage"].ToString(),
                        active_flag = dt["active_flag"].ToString(),
                        tax_prefix = dt["tax_prefix"].ToString(),
                        count = dt["count"].ToString(),

                    });
                    values.smrtax_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetTaxSegmentdropdown(MdlSmrMstTaxSummary values)
        {
            try
            {
                msSQL = "select taxsegment_name ,taxsegment_gid from acp_mst_ttaxsegment where active_flag='Y' and reference_type='Customer'";
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
            catch(Exception ex)
            {
                values.message = "Exception occured while loading Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public string DaGetTaxName(MdlSmrMstTaxSummary values, string tax_gid)
        {
            string tax_name="";
            try
            {

                msSQL = "SELECT tax_name,percentage FROM acp_mst_ttax WHERE tax_gid='" + tax_gid + "' and reference_type='Customer'";
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

        public void DaGetProductCounts(MdlSmrMstTaxSummary values)
        {
            
            try
            {

                msSQL =" SELECT(SELECT COUNT(*) FROM pmr_mst_tproduct) AS total_products, " +
                    "(SELECT COUNT(DISTINCT a.product_gid)  FROM pmr_mst_tproduct a LEFT JOIN acp_mst_ttaxsegment2product b ON a.product_gid = b.product_gid left join acp_mst_ttaxsegment c on c.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_gid IS NOT NULL and c.reference_type = 'Customer') AS assigned_products," +
                    " (SELECT COUNT(DISTINCT a.product_gid) FROM pmr_mst_tproduct a where a.product_gid not in ( SELECT b.product_gid FROM acp_mst_ttaxsegment2product b left JOIN acp_mst_ttaxsegment c ON b.taxsegment_gid = c.taxsegment_gid " +
                     " WHERE c.reference_type = 'Customer') ) AS unassigned_products";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<assignedProductCount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new assignedProductCount_list
                        {

                            countproduct_assigned = dt["assigned_products"].ToString(),
                            countproduct = dt["total_products"].ToString(),
                            countproduct_unassigned = dt["unassigned_products"].ToString(),


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
        }
        public string DaGetAssignedProductCount(MdlSmrMstTaxSummary values, string tax_gid)
        {
            string tax_name = "";
            try
            {

                msSQL = "select a.tax_name, b.product_name, c.taxsegment_gid,c.taxsegment_name from acp_mst_ttaxsegment2product a join pmr_mst_tproduct b on b.product_gid=a.product_gid join acp_mst_ttaxsegment c on c.taxsegment_gid=a.taxsegment_gid  where a.tax_gid='" + tax_gid + "' and c.reference_type='Customer'";
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
        public void DaPostTax(string user_gid, smrtax_list values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("STXM");
                msGetGid1 = objcmnfunctions.GetMasterGID("DG22");
                msSQL = "select sequence_curval from adm_mst_tsequence where sequence_code='DG22'";
                string lscode = objdbconn.GetExecuteScalar(msSQL);
                string glcode = "DG" + lscode;

                msSQL = " select tax_prefix from acp_mst_ttax where tax_prefix='" + values.tax_prefix.Replace("'","\\\'") + "' and reference_type='Customer'";
                string Tax_Prefix = objdbconn.GetExecuteScalar(msSQL);
                if (Tax_Prefix != "")
                {
                    values.status = false;
                    values.message = "Tax Prefix already exist";
                    return;
                }

                else
                {

                msSQL = " insert into acp_mst_ttax(" +
                        " tax_gid," +
                        " taxsegment_gid," +
                        " tax_prefix," +
                        " percentage," +
                        " reference_type," +
                        " active_flag," +
                        " tax_name, " +
                        " gl_code, " +
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
                    msSQL += "'" + (String.IsNullOrEmpty(values.tax_segment) ? values.tax_segment : values.tax_segment.Replace("'", "\\'")) + "',";
                }
                    if (values.tax_prefix == null || values.tax_prefix == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + (String.IsNullOrEmpty(values.tax_prefix) ? values.tax_prefix : values.tax_prefix.Replace("'", "\\'")) + "',";
                    }
                    msSQL += "'" + values.percentage + "'," +
                    
                         "'Customer'," +
                         "'" + values.active_flag + "'," +
                         "'" + (String.IsNullOrEmpty(values.tax_prefix) ? values.tax_prefix : values.tax_prefix.Replace("'", "\\'")) + "'," +
                         "'" + glcode + "'," +
                         "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Tax Added Successfully";
                    return ;
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Tax";
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
        public void DaUpdatedTax(string user_gid, smrtax_list values)
        {
            try
            {
                if (values.activeedit_flag == "YES")
                {
                    lsflag += 'Y';
                }
                else
                {
                    lsflag += 'N';
                }
                msSQL = "select taxsegment_gid from acp_mst_ttaxsegment where taxsegment_name = '" + values.taxsegmentedit.Replace("'","\\\'") + "'";
              string  lstaxsegmentgid = objdbconn.GetExecuteScalar(msSQL);
                
                msSQL = "select * from acp_mst_ttax where tax_prefix = '"+values.edittax_prefix.Replace("'", "\\\'") + "' and taxsegment_gid = '"+ lstaxsegmentgid + "' and percentage = '"+values.editpercentage+ "'and active_flag = '" + lsflag + "' and reference_type='Customer'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows) 
                { 
                    values.status = false;
                    values.message = "Same Tax Details already Exist";
                    objOdbcDataReader.Close();
                    return;
                }
                else
                {
                    msSQL = "select taxsegment_gid from acp_mst_ttaxsegment where taxsegment_name = '" + values.taxsegmentedit.Replace("'", "\\\'") + "'";
                    string lstaxsegment = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " update  acp_mst_ttax set " +
                          " percentage  = '" + values.editpercentage + "'," +
                          " taxsegment_gid  = '" + lstaxsegment + "'," +
                          " tax_prefix  = '" + (String.IsNullOrEmpty(values.edittax_prefix) ? values.edittax_prefix : values.edittax_prefix.Replace("'", "\\'")) + "'," +
                          " tax_name  = '" + (String.IsNullOrEmpty(values.edittax_prefix) ? values.edittax_prefix : values.edittax_prefix.Replace("'", "\\'")) + "'," +
                          " active_flag  = '" + lsflag + "'," +
                          " updated_by = '" + user_gid + "'," +
                          " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where tax_gid='" + values.tax_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = "update acp_mst_ttaxsegment2product set taxsegment_gid='" + lstaxsegment + "'where tax_gid='" + values.tax_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        //msGetGid1 = objcmnfunctions.GetMasterGID("DG22");
                        //msSQL = "select sequence_curval from adm_mst_tsequence where sequence_code='DG22'";
                        //string lscode = objdbconn.GetExecuteScalar(msSQL);
                        //string glcode = "DG" + lscode;
                        //objfincmn.finance_vendor_debitor_edit( values.edittax_prefix, glcode, values.tax_gid);
                        values.status = true;
                        values.message = "Tax Updated Successfully";
                        return;

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Tax";
                        return;
                    }
                }
                objOdbcDataReader.Close();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        public void DadeleteTaxSummary(string tax_gid, smrtax_list values)
        {
            try
            {
                msSQL = "select * from acp_mst_ttaxsegment2product where tax_gid = '" + tax_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
             
                if (objOdbcDataReader.HasRows)
                {

                    values.status = false;
                    values.message = "Tax is mapped to the product";
                    

                }
                else
                {

                    msSQL = "  delete from acp_mst_ttax where tax_gid='" + tax_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Tax Deleted Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Tax";
                        return;
                    }
                }
                objOdbcDataReader.Close();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }


        public string DaGetUnMappedProducts(MdlSmrMstTaxSummary values, string tax_gid,string taxsegment_gid)
        {
            string tax_name = "";
            try
            {
                msSQL = "SELECT a.product_name,a.customerproduct_code, a.product_code, a.cost_price, a.mrp_price, a.product_gid, c.productgroup_gid, " +
        "c.productgroup_name, c.productgroup_code, a.product_price, d.productuom_name " +
        "FROM pmr_mst_tproduct a " +
        "LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid " +
        "LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.productuom_gid " +
        "LEFT JOIN acp_mst_ttaxsegment2product b ON a.product_gid = b.product_gid " +
        "WHERE (b.taxsegment_gid != '" + taxsegment_gid + "'  and b.tax_gid = '" + tax_gid + "') " +
        "or b.product_gid IS NULL";
                //msSQL = " CALL smr_mst_spGetUnmappedProducts('" + taxsegment_gid + "','" + tax_gid + "')";
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

      


        public string GetTaxSegment2ProductSummary(string tax_gid, string taxsegment_gid ,MdlSmrMstTaxSummary values)
        {
            string tax_name = "";
            try
            {
                msSQL = "  SELECT   a.product_name,   a.customerproduct_code,    a.product_code,    a.cost_price,    a.mrp_price,    b.taxsegment_gid," +
                        "    a.product_gid,    c.productgroup_gid,    c.productgroup_name,    c.productgroup_code,    a.product_price,    d.productuom_name," +
                        "   g.productuomclass_name FROM     pmr_mst_tproduct a      LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid  " +
                        "    LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.productuom_gid      LEFT JOIN pmr_mst_tproductuomclass g ON g.productuomclass_gid = a.productuomclass_gid  " +
                        "    LEFT JOIN acp_mst_ttaxsegment2product b ON a.product_gid = b.product_gid AND b.taxsegment_gid = '" + taxsegment_gid + "'  AND b.tax_gid = '" + tax_gid + "' WHERE   b.product_gid IS NULL GROUP BY " +
                        "   a.product_name,   a.customerproduct_code,  a.product_code,  a.cost_price,  a.mrp_price,  b.taxsegment_gid,  a.product_gid,  c.productgroup_gid,   c.productgroup_name," +
                        "  c.productgroup_code, a.product_price, d.productuom_name,  g.productuomclass_name; "; ;

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
                msSQL = "select tax_name from acp_mst_ttax where tax_gid = '" + values.tax_gid + "'";
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
                                                    "'" +values.taxsegment_gid + "', " +
                                                    "'" + values.unmappedproduct_list[i].product_gid + "', " +
                                                    "'" +values.tax_gid + "', " +
                                                    "'" + lstaxname.Replace("'", "\\\'") + "', " +
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



        public string DaGetTaxUnmapping(string tax_gid, string taxsegment_gid, MdlSmrMstTaxSummary values)
        {
            string tax_name = "";
            try
            {
                msSQL = " SELECT a.product_name,a.customerproduct_code, a.product_code, a.cost_price, a.mrp_price,b.taxsegment_gid, a.product_gid," +
                        "  c.productgroup_gid, c.productgroup_name, productgroup_code, a.product_price, d.productuom_name, g.productuomclass_name FROM pmr_mst_tproduct a" +
                        "  LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid  LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.productuom_gid " +
                        "  LEFT JOIN pmr_mst_tproductuomclass g ON g.productuomclass_gid = a.productuomclass_gid  LEFT JOIN acp_mst_ttaxsegment2product b ON a.product_gid = b.product_gid " +
                        " where b.taxsegment_gid ='" + taxsegment_gid + "' and b.tax_gid ='" + tax_gid + "' group by b.product_gid  ";
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

