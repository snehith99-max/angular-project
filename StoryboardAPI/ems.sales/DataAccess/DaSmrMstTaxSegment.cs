using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using OfficeOpenXml.ConditionalFormatting;

namespace ems.sales.DataAccess
{
    public class DaSmrMstTaxSegment
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid, msGetGid1;
        int mnResult;
        string taxName;
        decimal percentage;

        public void DaGetTaxSegmentSummary(MdlSmrMstTaxSegment values)
        {
            try
            {

                msSQL = " select a.taxsegment_gid,a.taxsegment_name,count(b.customer_name) as taxsegment_count,a.taxsegment_code,a.taxsegment_description,a.active_flag,a.created_by,a.created_date from acp_mst_ttaxsegment a " +
                        " left join  crm_mst_tcustomer  b on b.taxsegment_gid=a.taxsegment_gid where reference_type='Customer' and b.status = 'Active' group by a.taxsegment_gid,a.taxsegment_name ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<TaxSegmentSummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TaxSegmentSummary_list
                        {
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            taxsegment_code = dt["taxsegment_code"].ToString(),
                            taxsegment_description = dt["taxsegment_description"].ToString(),
                            active_flag = dt["active_flag"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            taxsegment_count = dt["taxsegment_count"].ToString(),

                        });
                        values.TaxSegmentSummary_list = getModuleList;
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
        public void DaGetTaxSegmentMapping(MdlSmrMstTaxSegment values)
        {
            try
            {

                msSQL = " select a.taxsegment_gid,a.taxsegment_name,a.taxsegment_code,a.taxsegment_description,a.active_flag,a.created_by,a.created_date from acp_mst_ttaxsegment a " +
                        "  group by a.taxsegment_gid,a.taxsegment_name ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<TaxSegmentSummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TaxSegmentSummary_list
                        {
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            taxsegment_code = dt["taxsegment_code"].ToString(),
                            taxsegment_description = dt["taxsegment_description"].ToString(),
                            active_flag = dt["active_flag"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            taxsegment_count = dt["taxsegment_count"].ToString(),

                        });
                        values.TaxSegmentSummary_list = getModuleList;
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
        public void DaGetTaxSegment2ProductSummary(MdlSmrMstTaxSegment values, string product_gid)
        {
            try
            {

                msSQL = "select  a.taxsegment2product_gid, a.taxsegment_gid,d.taxsegment_name,  a.tax_gid, a.tax_name, a.tax_percentage," +
                    " a.product_gid, a.created_by, a.created_date, a.updated_by, a.updated_date , " +
                    " c.product_name,c.mrp_price, format((c.mrp_price * (a.tax_percentage / 100)),2) AS tax_amount " +
                    " from acp_mst_ttaxsegment2product a left join acp_mst_ttaxsegment b on a.taxsegment_gid=b.taxsegment_gid " +
                    " left join pmr_mst_tproduct c on a.product_gid=c.product_gid " +
                    " left join acp_mst_ttaxsegment d on a.taxsegment_gid = d.taxsegment_gid " +
                   
                    " where a.product_gid= '" + product_gid + "' order by d.taxsegment_name desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<TaxSegmentSummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TaxSegmentSummary_list
                        {
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegment2product_gid = dt["taxsegment2product_gid"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                           
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                          
                        });
                        values.TaxSegmentSummary_list = getModuleList;
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
        public void DaGetTax(MdlSmrTrnSalesorder values)
        {
            try
            {
                msSQL = " select tax_name,tax_gid,percentage, concat(tax_name, ' || ',percentage,'%') as tax from acp_mst_ttax where active_flag='Y'  ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxFourDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxFourDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name4 = dt["tax_name"].ToString(),
                            tax = dt["tax"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTax4Dtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostTaxSegment(string user_gid, TaxSegmentSummary_list values)
        {
            try
            {
                string lsreferencetype = "Customer";
                msSQL = " select taxsegment_name from acp_mst_ttaxsegment where taxsegment_name= '" + values.taxsegment_name.Replace("'", "\\\'") + "'and reference_type='Customer'"; 
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
                          " taxsegment_gid," +
                            " taxsegment_code," +
                            " taxsegment_name," +
                            " taxsegment_description," +
                            " reference_type," +
                            " active_flag," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                             "'" + msGetGid + "'," +
                             "'" + (String.IsNullOrEmpty(values.taxsegment_code) ? values.taxsegment_code : values.taxsegment_code.Replace("'", "\\'")) + "',";
                    if (values.taxsegment_name == null || values.taxsegment_name == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.taxsegment_name.Replace("'", "\\\'") + "'," ;
                    }
                    if (values.taxsegment_description == null || values.taxsegment_description == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" +  values.taxsegment_description.Replace("'", "\\\'") + "',";
                    }

                    msSQL += "'" + (String.IsNullOrEmpty(lsreferencetype) ? lsreferencetype : lsreferencetype.Replace("'", "\\'")) + "'," +
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
        public void DaUpdatedTaxSegmentSummary(string user_gid, TaxSegmentSummary_list values)
        {
            try
            {


                msSQL = " update  acp_mst_ttaxsegment set " +
                      " taxsegment_name  = '" + (String.IsNullOrEmpty(values.taxsegment_name_edit) ? values.taxsegment_name_edit : values.taxsegment_name_edit.Replace("'", "\\'")) + "'," ;
                        if (values.taxsegment_description_edit == null || values.taxsegment_description_edit == "")
                        {
                            msSQL += " taxsegment_description  = '',";
                        }
                        else
                        {
                            msSQL += " taxsegment_description  = '" + values.taxsegment_description_edit.Replace("'", "\\\'") + "',";
                           
                        }
                     msSQL += " active_flag  = '" + values.active_flag_edit + "'," +
                      " updated_by = '" + user_gid + "'," +
                      " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where taxsegment_gid='" + values.taxsegment_gid + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Tax Segment Updated Successfully";
                    return;

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Tax Segment";
                    return;
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating TaxSegment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaDeleteTaxSegmentSummary(string taxsegment_gid, TaxSegmentSummary_list values)
        {
            try
            {

                msSQL = "select * from crm_mst_tcustomer where taxsegment_gid = '"+ taxsegment_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "This tax segment is mapped to the customer!";
                    return;

                }
                msSQL="select * from acp_mst_ttax where taxsegment_gid = '"+taxsegment_gid+"'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "This tax segment is mapped to the Tax!";
                    return;

                }

                else
                {


                    msSQL = "  delete from acp_mst_ttaxsegment where taxsegment_gid='" + taxsegment_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Tax Segment Deleted Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Tax Segment";
                        return;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Tax Segment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostTaxSegment2Product(string user_gid, tax_list values)
        {
            try
            {
                msSQL = " select taxsegment_gid from acp_mst_ttaxsegment2product where taxsegment_gid= '" + values.taxsegment_gid + "' and  tax_gid= '" + values.tax_gid + "' and product_gid= '" + values.product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Tax Segment with this Tax Already Added";
                    return;
                }

                else
                {

                    msSQL = " select tax_name,percentage from acp_mst_ttax where tax_gid= '" + values.tax_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count > 0)
                    {

                        taxName = dt_datatable.Rows[0]["tax_name"].ToString();
                        percentage = Convert.ToDecimal(dt_datatable.Rows[0]["percentage"]);

                    }
                    msSQL = " select mrp_price from pmr_mst_tproduct where product_gid= '" + values.product_gid + "' ";
                    decimal lsmrp_price = Convert.ToDecimal(objdbconn.GetExecuteScalar(msSQL));
                    decimal taxAmount = (lsmrp_price * percentage) / 100;
                    msGetGid1 = objcmnfunctions.GetMasterGID("TS2P");

                    msSQL = " insert into acp_mst_ttaxsegment2product (" +
                            " taxsegment2product_gid," +
                            " taxsegment_gid," +
                            " product_gid," +
                            " tax_gid," +
                            " tax_name," +
                            " tax_percentage," +
                            " tax_amount," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid1 + "'," +
                            "'" + values.taxsegment_gid + "'," +
                            "'" + values.product_gid + "'," +
                            "'" + values.tax_gid + "'," +
                            "'" + taxName + "'," +
                            "'" + percentage + "'," +
                             "'" + taxAmount + "'," +
                            "'" + user_gid + "'," +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Tax Segment to Product Added Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Tax Segment to Product";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Tax Segment to Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaDeleteTaxSegment2Product(string taxsegment2product_gid, TaxSegmentSummary_list values)
        {
            try
            {
                //msSQL = " SELECT * FROM smr_trn_treceivequotationdtl a  " +
                //    " inner join smr_trn_treceivequotation b on a.quotation_gid=b.quotation_gid " +
                //    " where '" + taxsegment_gid + "' in (a.tax1_gid,a.tax2_gid,a.tax3_gid,b.tax_gid) and b.delete_flag='N' ";
                //dt_datatable = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable.Rows.Count == 0)
                //{
                //    msSQL = " SELECT * FROM smr_trn_tsalesorderdtl a " +
                //    " inner join smr_trn_tsalesorder b on a.salesorder_gid=b.salesorder_gid " +
                //    " where '" + taxsegment_gid + "' in (a.tax1_gid,a.tax2_gid,a.tax3_gid,b.tax_gid) ";
                //    dt_datatable = objdbconn.GetDataTable(msSQL);
                //    if (dt_datatable.Rows.Count == 0)
                //    {
                //        msSQL = " SELECT * FROM rbl_trn_tinvoicedtl a " +
                //  " inner join rbl_trn_tinvoice b on a.invoice_gid=b.invoice_gid " +
                //  " where '" + taxsegment_gid + "' in (tax1_gid,tax2_gid,tax3_gid,b.tax_gid) ";
                //        dt_datatable = objdbconn.GetDataTable(msSQL);
                //        if (dt_datatable.Rows.Count == 0)
                //        {

                msSQL = "  delete from acp_mst_ttaxsegment2product where taxsegment2product_gid='" + taxsegment2product_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Tax Segment Deleted Successfully";
                    return;
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Tax Segment";
                    return;
                }
                //        }
                //        else
                //        {
                //            values.status = false;
                //            values.message = " Can't Delete This Tax, This Tax is in Invoice";
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        values.status = false;
                //        values.message = " Can't Delete This Tax, This Tax is in Sales Order Product";
                //        return;
                //    }
                //}

                //else
                //{
                //    values.status = false;
                //    values.message = " Can't Delete This Tax, This Tax is in Quotation Product";
                //    return;
                //}
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Tax Segment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetUnassignedlist(string taxsegment_gid, string customer_gid, MdlSmrMstTaxSegment values)
        {
            try
            {

                msSQL = " select a.customer_gid,a.customer_name,a.taxsegment_gid,b.taxsegment_name from crm_mst_tcustomer a " +
                        " left join acp_mst_ttaxsegment b on a.taxsegment_gid = b.taxsegment_gid " +
                        "  where a.taxsegment_gid is null or a.taxsegment_gid='' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomerUnassignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerUnassignedlist
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            taxsegment_gid = taxsegment_gid,
                            customer_name = dt["customer_name"].ToString(),
                        });
                        values.GetCustomerUnassignedlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Unassigned List Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCustomerAssignedlist(string taxsegment_gid, MdlSmrMstTaxSegment values)
        {
            try
            {
                //msSQL = " select a.customer_gid,h.user_firstname,v.region_name,a.customer_name,DATE_FORMAT(a.created_date,'%d-%m-%Y')  as created_date,a.taxsegment_gid,a.customer_id, " +
                //        " a.salesperson_gid,b.taxsegment_name,d.email,CONCAT(d.customercontact_name, '/', d.mobile, '/',d.email) AS contact_info," +
                //        " a.customer_country,u.country_name, a.customer_pin, a.pricesegment_gid,a.customer_region," +
                //        " a.customer_city, a.customer_state, concat(a.customer_address, '  ', a.customer_address2) as address, " +
                //        " (SELECT CONCAT(DATEDIFF(CURDATE(), MIN(created_date)), ' days') FROM crm_mst_tcustomer WHERE customer_gid = a.customer_gid) as customer_since " +
                //        " from crm_mst_tcustomer a left join crm_mst_tcustomercontact d on a.customer_gid = d.customer_gid" +
                //        " left join acp_mst_ttaxsegment b on a.taxsegment_gid = b.taxsegment_gid left join acp_mst_ttaxsegment2customer c on a.customer_gid = c.customer_gid " +
                //        " left join adm_mst_tuser h on h.user_gid = a.salesperson_gid " +
                //        " left join adm_mst_tcountry u on u.country_gid = a.customer_country " +
                //        " left join crm_mst_tregion v on v.region_gid = a.customer_region " +
                //        " where  a.taxsegment_gid= '" + taxsegment_gid + "' group by a.customer_gid ";

                msSQL = " call crm_mst_spcustomertaxsegmentassign('"+ taxsegment_gid + "')";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomerassignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerassignedlist
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            email = dt["email"].ToString(),
                            customer_country = dt["country_name"].ToString(),
                            customer_pin = dt["customer_pin"].ToString(),
                            customer_city = dt["customer_city"].ToString(),
                            customer_state = dt["customer_state"].ToString(),
                            contact_info = dt["contact_info"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            created_date = dt["customer_since"].ToString(),
                            customer_region = dt["region_name"].ToString(),
                            customer_code  = dt["customer_id"].ToString(),
                            customer_status  = dt["status"].ToString(),
                        });
                        values.GetCustomerassignedlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Assigned List Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostCustomerUnassignedlist(string user_gid, customerassign_list values)
        {
            try
            {
                if (values.campaignunassignemp != null || values.taxsegment_gid != null || values.taxsegment_gid != "")
                {
                    List<string> list2 = new List<string>();
                    msSQL = "SELECT customer_gid FROM crm_mst_tcustomer WHERE taxsegment_gid = '" + values.taxsegment_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        list2.Add(row["customer_gid"].ToString());
                    }
                    foreach (string gid in list2)
                    {
                        if (!values.campaignunassignemp.Any(emp => emp._id == gid))
                        {

                            msSQL = "UPDATE crm_mst_tcustomer SET taxsegment_gid = NULL WHERE customer_gid = '" + gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                    for (int i = 0; i < values.campaignunassignemp.ToArray().Length; i++)
                    {
                        string customerGid = values.campaignunassignemp[i]._id;

                        msSQL = "UPDATE crm_mst_tcustomer SET taxsegment_gid = '" + values.taxsegment_gid + "' WHERE customer_gid = '" + customerGid + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Customer Mapped Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Mapping Customer";
                        }
                    }
                    foreach (string gid in list2)
                    {
                        if (!values.campaignunassignemp.Any(emp => emp._id == gid))
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("TS2C");
                            msSQL = "INSERT INTO acp_mst_ttaxsegment2customer (taxsegment2customer_gid, taxsegment_gid, customer_gid, created_by, created_date) " +
                                    "VALUES ('" + msGetGid + "', '" + values.taxsegment_gid + "', '" + gid + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "No records to process.";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Unassigning!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetVendorUnassignedlist(string taxsegment_gid, string vendor_gid, MdlSmrMstTaxSegment values)
        {
            try
            {

                msSQL = " select a.vendor_gid,a.vendor_companyname,a.taxsegment_gid,b.taxsegment_name from acp_mst_tvendor a " +
                        " left join acp_mst_ttaxsegment b on a.taxsegment_gid = b.taxsegment_gid " +
                        "  where a.taxsegment_gid is null or a.taxsegment_gid='' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetVendorUnassignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendorUnassignedlist
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            taxsegment_gid = taxsegment_gid,
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                        });
                        values.GetVendorUnassignedlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Unassigned List Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetVendorAssignedlist(string taxsegment_gid, string vendor_gid, MdlSmrMstTaxSegment values)
        {
            try
            {
                msSQL = " select a.vendor_gid,a.vendor_companyname,a.taxsegment_gid,b.taxsegment_name from acp_mst_tvendor a " +
                                      " left join acp_mst_ttaxsegment b on a.taxsegment_gid = b.taxsegment_gid " +
                                      " left join acp_mst_ttaxsegment2vendor c on a.vendor_gid = c.vendor_gid where  a.taxsegment_gid= '" + taxsegment_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetVendorUnassignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendorUnassignedlist
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            taxsegment_gid = taxsegment_gid,
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                        });
                        values.GetVendorUnassignedlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Assigned List Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostVendorUnassignedlist(string user_gid, vendorassign_list values)
        {
            try
            {
                if (values.vendorcampaignunassignemp != null || values.taxsegment_gid != null || values.taxsegment_gid != "")
                {
                    List<string> list2 = new List<string>();
                    msSQL = "SELECT vendor_gid FROM acp_mst_tvendor WHERE taxsegment_gid = '" + values.taxsegment_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        list2.Add(row["vendor_gid"].ToString());
                    }
                    foreach (string gid in list2)
                    {
                        if (!values.vendorcampaignunassignemp.Any(emp => emp._id == gid))
                        {

                            msSQL = "UPDATE acp_mst_tvendor SET taxsegment_gid = NULL WHERE vendor_gid = '" + gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }


                    }
                    for (int i = 0; i < values.vendorcampaignunassignemp.ToArray().Length; i++)
                    {
                        string customerGid = values.vendorcampaignunassignemp[i]._id;


                        msSQL = "UPDATE acp_mst_tvendor SET taxsegment_gid = '" + values.taxsegment_gid + "' WHERE vendor_gid = '" + customerGid + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Vendor Mapped Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred While Mapping Vendor";
                        }
                    }
                    foreach (string gid in list2)
                    {
                        if (!values.vendorcampaignunassignemp.Any(emp => emp._id == gid))
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("TS2V");
                            msSQL = "INSERT INTO acp_mst_ttaxsegment2vendor (taxsegment2vendor_gid, taxsegment_gid, vendor_gid, created_by, created_date) " +
                                               "VALUES ('" + msGetGid + "', '" + values.taxsegment_gid + "', '" + gid + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "No records to process.";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Unassigning!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetCustomerCount(MdlSmrMstTaxSegment values)
        {
            try
            {
                msSQL = "SELECT " +
                        " (SELECT COUNT(*) FROM crm_mst_tcustomer) as total_customer, " +
                        " (SELECT COUNT(*) FROM crm_mst_tcustomer a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name LIKE 'within%') as within_customer, " +
                        " (SELECT COUNT(*) FROM crm_mst_tcustomer a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name LIKE 'inter%') as interstate_customer, " +
                        " (SELECT COUNT(*) FROM crm_mst_tcustomer a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name LIKE 'oversea%') as overseas_customer, " +
                        " (select count(*) from crm_mst_tcustomer where taxsegment_gid is not null and taxsegment_gid <> '') as assigncount, " +
                        " (SELECT COUNT(*) FROM crm_mst_tcustomer a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name NOT LIKE 'within%' AND b.taxsegment_name NOT LIKE 'inter%' AND b.taxsegment_name NOT LIKE 'oversea%') as other_customer, " +
                        " (SELECT COUNT(*) FROM crm_mst_tcustomer WHERE taxsegment_gid IS NULL OR taxsegment_gid = '') as unassign_customer ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<TaxSegmentCustomer_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TaxSegmentCustomer_list
                        {
                            total_customer = dt["total_customer"].ToString(),
                            within_customer = dt["within_customer"].ToString(),
                            interstate_customer = dt["interstate_customer"].ToString(),
                            overseas_customer = dt["overseas_customer"].ToString(),
                            assigncount = dt["assigncount"].ToString(),
                            unassign_customer = dt["unassign_customer"].ToString(),
                            other_customer = dt["other_customer"].ToString(),


                        });
                        values.TaxSegmentCustomer_list = getModuleList;
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
        public void DaGetVendorCount(MdlSmrMstTaxSegment values)
        {
            try
            {
                msSQL = "SELECT " +
                        " (SELECT COUNT(*) FROM acp_mst_tvendor) as total_customer, " +
                        " (SELECT COUNT(*) FROM acp_mst_tvendor a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name LIKE 'within%') as within_customer, " +
                        " (SELECT COUNT(*) FROM acp_mst_tvendor a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name LIKE 'inter%') as interstate_customer, " +
                        " (SELECT COUNT(*) FROM acp_mst_tvendor a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name LIKE 'oversea%') as overseas_customer, " +
                        " (SELECT COUNT(*) FROM acp_mst_tvendor a LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid WHERE b.taxsegment_name NOT LIKE 'within%' AND b.taxsegment_name NOT LIKE 'inter%' AND b.taxsegment_name NOT LIKE 'oversea%') as other_customer, " +
                        " (SELECT COUNT(*) FROM acp_mst_tvendor WHERE taxsegment_gid IS NULL OR taxsegment_gid = '') as unassign_customer ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<TaxSegmentVendor_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TaxSegmentVendor_list
                        {
                            total_customer = dt["total_customer"].ToString(),
                            within_customer = dt["within_customer"].ToString(),
                            interstate_customer = dt["interstate_customer"].ToString(),
                            overseas_customer = dt["overseas_customer"].ToString(),
                            unassign_customer = dt["unassign_customer"].ToString(),
                            other_customer = dt["other_customer"].ToString(),


                        });
                        values.TaxSegmentVendor_list = getModuleList;
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
        public void DaGetTotalCustomerSummary(MdlSmrMstTaxSegment values)
        {
            try
            {
                msSQL = "SELECT customer_type,customer_name,customer_gid,customer_address,a.taxsegment_gid,customer_city,c.country_name,customer_state,customer_country, " +
                     " case when b.taxsegment_name is null then 'Unassigned' else b.taxsegment_name end as taxsegment_name" +
                        " FROM crm_mst_tcustomer a " +
                        " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid " +
                        "  left join adm_mst_tcountry c on a.customer_country = c.country_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<TaxSegmentTotalCustomer_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TaxSegmentTotalCustomer_list
                        {

                            customer_name = dt["customer_name"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_city = dt["customer_city"].ToString(),
                            customer_state = dt["customer_state"].ToString(),
                            customer_country = dt["country_name"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),


                        });
                        values.TaxSegmentTotalCustomer_list = getModuleList;
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
        public void DaGetTotalCustomerWithinState(MdlSmrMstTaxSegment values)
        {
            try 
            {
                msSQL = " SELECT customer_type, customer_name,customer_gid, a.taxsegment_gid, customer_address, customer_city, customer_state, " +
                "customer_country, taxsegment_name, c.country_name" +
                 " FROM crm_mst_tcustomer a " +
                 " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid " +
                 " left join adm_mst_tcountry c on a.customer_country = c.country_gid " +
                 " WHERE b.taxsegment_name LIKE 'Within%'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetWithInState = new List<GetWithInState_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow wtin in dt_datatable.Rows)
                    {
                        GetWithInState.Add(new GetWithInState_list
                        {
                            customer_type = wtin["customer_type"].ToString(),
                            customer_gid = wtin["customer_gid"].ToString(),
                            customer_name = wtin["customer_name"].ToString(),
                            customer_address = wtin["customer_address"].ToString(),
                            customer_city = wtin["customer_city"].ToString(),
                            customer_state = wtin["customer_state"].ToString(),
                            customer_country = wtin["country_name"].ToString(),
                            taxsegment_name = wtin["taxsegment_name"].ToString(),
                            taxsegment_gid = wtin["taxsegment_gid"].ToString(),
                        });
                        values.GetWithInState_list = GetWithInState;
                    }
                }
            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }            
        }  
        public void DaGetTotalCustomerInterState(MdlSmrMstTaxSegment values)
        {
            try 
            {
                msSQL = " SELECT customer_type, customer_name,customer_gid, customer_address,a.taxsegment_gid, customer_city, customer_state, " +
                "customer_country, taxsegment_name ,c.country_name" +
                 " FROM crm_mst_tcustomer a " +
                 " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid" +
                 " left join adm_mst_tcountry c on a.customer_country = c.country_gid " +
                 " WHERE b.taxsegment_name LIKE 'Inter%'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetInterState = new List<GetInterState_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow wtin in dt_datatable.Rows)
                    {
                        GetInterState.Add(new GetInterState_list
                        {
                            customer_type = wtin["customer_type"].ToString(),
                            customer_gid = wtin["customer_gid"].ToString(),
                            customer_name = wtin["customer_name"].ToString(),
                            customer_address = wtin["customer_address"].ToString(),
                            customer_city = wtin["customer_city"].ToString(),
                            customer_state = wtin["customer_state"].ToString(),
                            customer_country = wtin["country_name"].ToString(),
                            taxsegment_name = wtin["taxsegment_name"].ToString(),
                            taxsegment_gid = wtin["taxsegment_gid"].ToString(),
                        });
                        values.GetInterState_list = GetInterState;
                    }
                }
            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }            
        } 
        public void DaGetTotalCustomerOverSeaState(MdlSmrMstTaxSegment values)
        {
            try 
            {
                msSQL = " SELECT customer_gid,customer_type, customer_name, customer_address, customer_city, customer_state,a.taxsegment_gid, " +
                " customer_country, taxsegment_name,c.country_name " +
                 " FROM crm_mst_tcustomer a " +
                 " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid " +
                 "  left join adm_mst_tcountry c on a.customer_country = c.country_gid " +
                 " WHERE b.taxsegment_name LIKE 'Overseas%'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetOverseas = new List<GetOverseas_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow wtin in dt_datatable.Rows)
                    {
                        GetOverseas.Add(new GetOverseas_list
                        {
                            customer_type = wtin["customer_type"].ToString(),
                            customer_gid = wtin["customer_gid"].ToString(),
                            customer_name = wtin["customer_name"].ToString(),
                            customer_address = wtin["customer_address"].ToString(),
                            customer_city = wtin["customer_city"].ToString(),
                            customer_state = wtin["customer_state"].ToString(),
                            customer_country = wtin["country_name"].ToString(),
                            taxsegment_name = wtin["taxsegment_name"].ToString(),
                            taxsegment_gid = wtin["taxsegment_gid"].ToString(),
                        });
                        values.GetOverseas_list = GetOverseas;
                    }
                }
            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }            
        }
        public void DaGetCustomerUnassignSummary(MdlSmrMstTaxSegment values)
        {
            try
            {
                //msSQL = "select a.customer_gid,h.user_firstname,a.customer_name," +
                //    "DATE_FORMAT(a.created_date, '%d-%m-%Y') as created_date,a.taxsegment_gid,a.customer_id," +
                //    " a.salesperson_gid,b.taxsegment_name,d.email," +
                //    "CONCAT(d.customercontact_name, '/', d.mobile, '/', d.email) AS contact_info," +
                //    " a.customer_country, a.customer_pin, a.pricesegment_gid,a.customer_region," +
                //    " f.pricesegment_name,a.customer_city, a.customer_state," +
                //    " concat(a.customer_address, '  ', a.customer_address2) as address " +
                //    " from crm_mst_tcustomer a left join crm_mst_tcustomercontact d on a.customer_gid = d.customer_gid" +
                //    " left join acp_mst_ttaxsegment b on a.taxsegment_gid = b.taxsegment_gid" +
                //    " left join acp_mst_ttaxsegment2customer c on a.customer_gid = c.customer_gid  " +
                //    " left join adm_mst_tuser h on h.user_gid = a.salesperson_gid " +
                //    " left join smr_trn_tpricesegment2customer f on a.pricesegment_gid = f.pricesegment_gid" +
                //    " where a.taxsegment_gid IS NULL OR a.taxsegment_gid = '' group by  a.customer_gid,a.taxsegment_gid";
                msSQL = " call crm_mst_spcustomertaxsegment";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetUnassignsummary = new List<GetUnassignsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetUnassignsummary.Add(new GetUnassignsummary_list
                        {

                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_country = dt["customer_country"].ToString(),
                            contact_info = dt["contact_info"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            created_date = dt["customer_since"].ToString(),
                            customer_region = dt["region_name"].ToString(),
                            customer_code = dt["customer_id"].ToString(),
                            pricesegment_name = dt["pricesegment_name"].ToString(),
                            country_name = dt["country_name"].ToString(),


                        });
                        values.GetUnassignsummary_list = GetUnassignsummary;
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
        public void DaGetTaxSegmentDropDown(string taxsegment_gid,MdlSmrMstTaxSegment values)
        {
            try
            {
                msSQL = " select taxsegment_name, taxsegment_code, taxsegment_gid from acp_mst_ttaxsegment" +
                    " where reference_type='Customer' and  taxsegment_gid <> '" + taxsegment_gid + "'";
                        ;

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetTaxSegmentDropDown = new List<GetTaxSegmentDropDown_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetTaxSegmentDropDown.Add(new GetTaxSegmentDropDown_list
                        {
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                        });
                        values.GetTaxSegmentDropDown_list = GetTaxSegmentDropDown;
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
        public void DaPostTaxsegmentMoveOn (string user_gid, PostTaxsegment values)
        {
           
            for(int i = 0; i < values.GetUnassignsummary_list.ToArray().Length; i++) 
            {
                msSQL = " update crm_mst_tcustomer set " +
               "taxsegment_gid='" + values.taxsegment_gid + "' where customer_gid='" + values.GetUnassignsummary_list[i].customer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Tax Segment assign to Customer !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while occured assigning customer to tax segment";
                }
            }           
        }
        public void DaGetOtherSegamentSummary(MdlSmrMstTaxSegment values)
        {
            msSQL = " SELECT customer_type,customer_gid,customer_name,customer_address,a.taxsegment_gid," +
                " customer_city,customer_state,customer_country,c.country_name," +
                " taxsegment_name FROM crm_mst_tcustomer a " +
                " LEFT JOIN acp_mst_ttaxsegment b ON a.taxsegment_gid = b.taxsegment_gid " +
                "  left join adm_mst_tcountry c on a.customer_country = c.country_gid " +
                " WHERE b.taxsegment_name not LIKE 'within%' AND b.taxsegment_name NOT LIKE 'inter%' " +
                "  AND b.taxsegment_name NOT LIKE 'oversea%'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var Getothersegment = new List<GetotherSegment_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    Getothersegment.Add(new GetotherSegment_list
                    {

                        customer_name = dt["customer_name"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        customer_city = dt["customer_city"].ToString(),
                        customer_state = dt["customer_state"].ToString(),
                        customer_country = dt["country_name"].ToString(),
                        taxsegment_name = dt["taxsegment_name"].ToString(),
                        taxsegment_gid = dt["taxsegment_gid"].ToString(),


                    });
                    values.GetotherSegment_list = Getothersegment;
                }
            }
            dt_datatable.Dispose();
        }

    }
}