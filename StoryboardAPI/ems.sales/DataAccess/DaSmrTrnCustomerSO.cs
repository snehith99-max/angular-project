using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnCustomerSO
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msINGetGID, msGetinGid, msSQL1, msSQL2 = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        decimal lsmrp_price;
        double subtotal, reCalTotalAmount, reCaldiscountAmount, reCalTaxAmount, taxAmount, totaltaxamount;
        string lsrefno, lscompany_code, lstaxamount;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lspercentage,
            start_date, msgetlead2campaign_gid, end_date, lspercentage1, lsdesignation_code,
            lstaxname2, lsorder_type, lsproduct_type, lsproductgid1, lstaxname1, lsdiscountpercentage,
            lsdiscountamount, lsprice, lstype1, lsproduct_price, mssalesorderGID, mssalesorderGID1,
            mscusconGetGID, lscustomer_name, msGetCustomergid, lscustomer_gid, msGetGid2, msGetGid3,
            lsCode, msPOGetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;


        string taxsegment_gid, taxsegment_name, tax_name, tax_gid, tax_gid2, tax_gid3, tax_name2, tax_name3,
            tax_name1, tax_percentage, tax_percent2, tax_percent3, tax_amount, tax_amount2, tax_amount3,
            mrp_price, cost_price, tax1, tax2, tax3, lsproductuom_gid, lsproductgid,lsproductgroupgid,
            lsproduct_name, lsproductuom_name, currency_code, exchange_rate,currency_gid;

        double rreCalTotalAmount;

        public void DaGetSmrTrnSalesordersummary(string customer_gid, MdlSmrTrnCustomerSO values)
        {
            try
            {
                msSQL = " select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select distinct a.salesorder_gid, a.billing_email,a.customer_contact_person,a.customer_email,a.so_referenceno1,a.customer_gid,a.so_remarks," +
                        " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') as salesorder_date,c.user_firstname as created_by,a.so_type,a.currency_code," +
                        " a.customer_contact_person, a.salesorder_status,a.currency_code,s.source_name,d.customer_code,i.branch_name, " +
                        " format(a.Grandtotal,2) as Grandtotal, CONCAT(k.user_firstname, ' ', k.user_lastname) AS salesperson_name,  " +
                        " case when a.currency_code = '" + currency + "' then a.customer_name " +
                        " when a.currency_code is null then a.customer_name " +
                        " when a.currency_code is not null and a.currency_code <> '" + currency + "' then (a.customer_name) end as customer_name, " +
                        " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                        " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact,a.invoice_flag," +
                        "a.invoice_gid " +
                        " from cst_trn_tsalesorder a " +
                        " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
                        " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid " +
                        " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                        " left join adm_mst_tuser c on b.user_gid= c.user_gid" +
                        " left join hrm_mst_tbranch i on a.branch_gid= i.branch_gid" +
                        " LEFT JOIN adm_mst_tuser k ON a.salesperson_gid = k.user_gid " +
                        " left join crm_trn_tleadbank l on l.customer_gid=a.customer_gid" +
                        " left join crm_mst_tsource s on s.source_gid=l.source_gid" +
                        " where 1=1 and a.customer_gid = '" + customer_gid + "' and a.salesorder_status not in('Cancelled','SO Amended') order by a.salesorder_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Customersalesorder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Customersalesorder_list
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            so_type = dt["so_type"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            user_firstname = dt["created_by"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            salesperson_name = dt["salesperson_name"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            so_remarks = dt["so_remarks"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                        });
                        values.Customersalesorder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DagetCancelSalesOrder(string salesorder_gid, MdlSmrTrnCustomerSO values)
        {
            try
            {

                msSQL = "SELECT * FROM smr_trn_tsalesorder WHERE salesorder_gid = '" + salesorder_gid + "' and salesorder_status in('Approved', 'Approve Pending')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = "update smr_trn_tsalesorder set salesorder_status='Cancelled' where salesorder_gid='" + salesorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Order Cancelled Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Cancelling Order";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while canceling Sales Order!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetProductGroup(MdlSmrTrnCustomerSO values)
        {
            msSQL = " select productgroup_gid, productgroup_name from pmr_mst_tproductgroup";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<CustomerGetproductgroup>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new CustomerGetproductgroup
                    {
                        productgroup_gid = dt["productgroup_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                    });
                    values.CustomerGetproductgroup = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        private DataTable GetTaxDetailsForProduct(string product_gid, string customer_gid)
        {
            // Query tax segment details based on product_gid
            msSQL = "SELECT f.taxsegment_gid, d.taxsegment_gid, e.taxsegment_name, d.tax_name, d.tax_gid, " +
                "CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, " +
                "d.tax_amount, a.mrp_price, a.cost_price, a.product_gid " +
                "FROM acp_mst_ttaxsegment2product d " +
                "LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                "LEFT JOIN crm_mst_tcustomer f ON f.taxsegment_gid = e.taxsegment_gid " +
                "LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid " +
                "WHERE a.product_gid = '" + product_gid + "' AND f.customer_gid = '" + customer_gid + "'";

            // Execute query to get tax segment details
            return objdbconn.GetDataTable(msSQL);
        }        
        public void DaGetViewsalesorderdetails(string salesorder_gid, string customer_gid, MdlSmrTrnCustomerSO values)
        {
            try
            {

                msSQL = " select d.product_gid,d.product_name,d.salesorderdtl_gid,d.productgroup_name,d.product_code,d.uom_name,d.qty_quoted,d.margin_amount,d.margin_percentage,d.discount_percentage, d.discount_amount," +
                    "FORMAT(d.product_price, 2) AS product_price ,d.tax_name,format(d.tax_amount, 2) as tax_amount,FORMAT(d.price, 2) AS price " +
                    "FROM cst_trn_tsalesorder a LEFT JOIN cst_trn_tsalesorderdtl d ON d.salesorder_gid = a.salesorder_gid" +
                    " WHERE a.salesorder_gid = '" + salesorder_gid + "' order by d.salesorderdtl_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Customerpostsalesorderdetails_list>();
                var getGetTaxSegmentList = new List<CustomerGetTaxSegmentListorder>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Customerpostsalesorderdetails_list
                        {
                            product_code = dt["product_code"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            price = dt["price"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            margin_percentage = dt["margin_percentage"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            margin_amount = dt["margin_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });                      
                        values.Customerpostsalesorderdetails_list = getModuleList;
                        values.CustomerGetTaxSegmentListorder = getGetTaxSegmentList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }        
        public void DaGetProductsearchSummarySales(MdlSmrTrnCustomerSO values)
        {
            try
            {
                string lsSqlType = "product";

                msSQL = " call pmr_mst_spproductsearch('" + lsSqlType + "','','')";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<CustomerGetProductsearchs>();


                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        var product = new CustomerGetProductsearchs
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["mrp_price"].ToString(),
                            quantity = 0,
                            total_amount = 0,
                            discount_percentage = 0,
                            discount_amount = 0,
                        };
                        getModuleList.Add(product);                     
                    }
                    values.CustomerGetProductsearchs = getModuleList;
                }               
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetProductWithTaxSummary(string customer_gid, string product_gid, MdlSmrTrnCustomerSO values)
        {
            string lsSQLTYPE = "customer";

            msSQL = "call pmr_mst_spproductsearch('" + lsSQLTYPE + "','" + product_gid + "', '" + customer_gid + "')";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var allTaxSegmentsList = new List<CustomerGetTaxSegmentListorder>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt1 in dt_datatable.Rows)
                {
                    allTaxSegmentsList.Add(new CustomerGetTaxSegmentListorder
                    {
                        taxsegment_gid = dt1["taxsegment_gid"].ToString(),
                        taxsegment_name = dt1["taxsegment_name"].ToString(),
                        tax_name = dt1["tax_name"].ToString(),
                        tax_percentage = dt1["tax_percentage"].ToString(),
                        tax_gid = dt1["tax_gid"].ToString(),
                        mrp_price = dt1["mrp_price"].ToString(),
                        cost_price = dt1["cost_price"].ToString(),
                        tax_amount = dt1["tax_amount"].ToString(),
                        product_name = dt1["product_name"].ToString(),
                        product_gid = dt1["product_gid"].ToString(),
                        tax_prefix = dt1["tax_prefix"].ToString(),
                        exchange_rate = dt1["exchange_rate"].ToString(),
                        currencyexchange_gid = dt1["currencyexchange_gid"].ToString(),
                    });
                    values.CustomerGetTaxSegmentListorder = allTaxSegmentsList;
                }
            }
        }      
        public void DaProductSalesSummary(string customer_gid, MdlSmrTrnCustomerSO values, string smryproduct_gid)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;
                msSQL = "SELECT a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,a.tax_amount,a.tax_percentage,a.tax_name2,a.tax_amount2,a.tax_percentage2,a.tax_name3,a.tax_amount3,a.tax_percentage3,a.tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
                  " v.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, a.product_code, a.qty_quoted, a.product_remarks, " +
                  " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                  " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                  " FORMAT(a.selling_price, '0.00') AS selling_price,a.product_remarks, " +                 
                  "  format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.tax_rate " +
                  " FROM smr_tmp_tsalesorderdtl a " +
                   " left join smr_trn_tsalesorder s on s.salesorder_gid=a.salesorder_gid " +
                   " left join crm_mst_tcustomer p on p.taxsegment_gid = a.taxsegment_gid " +
                   " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                   " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                   " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                   " WHERE a.employee_gid = '" + customer_gid + "' group by a.tmpsalesorderdtl_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Customersalesorders_list>();
                var getGetTaxSegmentList = new List<CustomerGetTaxSegmentListorder>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new Customersalesorders_list
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            slno = dt["slno"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            discountpercentage = double.Parse(dt["discount_percentage"].ToString()),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_price = dt["product_price"].ToString().Replace(",",""),
                            quantity = dt["qty_quoted"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            producttotalamount = dt["price"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            grand_total = dt["price"].ToString(),
                            grandtotal = dt["price"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),                            
                        });
                    }
                    // Set product summary and tax segment details to values
                    values.Customersalesorders_list = getModuleList;
                }

                dt_datatable.Dispose();
                values.grand_total = grand_total;
                values.grandtotal = grandtotal;
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting product summary!";
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }        
        public void DaPostOnAdds(string employee_gid, Customersalesorders_list values)
        {
            try
            {

                foreach (var data in values.SOProductList)
                {
                    if (data.quantity == null || data.quantity == "0")
                    {

                    }
                    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("VSDT");

                        msSQL = "select product_gid, productgroup_gid from pmr_mst_tproduct where product_name='" + data.product_name + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                          
                            lsproductgroupgid = objOdbcDataReader["productgroup_gid"].ToString();
                            lsproductgid = objOdbcDataReader["product_gid"].ToString();

                            objOdbcDataReader.Close();


                        }

                        msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + data.productuom_name + "'";
                        string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);


                        double discount_precentage = double.TryParse(data.discount_percentage, out double discountprecentage) ? discountprecentage : 0;
                        double discountamount = double.TryParse(data.discountamount, out double discount_amount) ? discount_amount : 0;

                        msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid='" + values.currency_code + "'";
                        string lscurrency = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                         " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                         " WHERE product_gid='" + lsproductgid + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            
                            if (objOdbcDataReader["producttype_name"].ToString() != "Services")
                            {
                                lsorder_type = "Sales";

                            }
                            else
                            {
                                lsorder_type = "Services";
                            }

                            objOdbcDataReader.Close();
                           
                        }                                            

                        msSQL = " insert into smr_tmp_tsalesorderdtl( " +
                           " tmpsalesorderdtl_gid," +
                           " salesorder_gid," +
                           " employee_gid," +
                           " product_gid," +
                           " productgroup_gid," +
                           " productgroup_name," +
                           " product_code," +
                           " product_name," +
                           " product_price," +
                           " qty_quoted," +
                           " uom_gid," +
                           " uom_name," +
                           " price," +
                           " order_type," +
                           " taxsegment_gid, " +                          
                          " tax1_gid, " +
                          " tax2_gid, " +
                          " tax3_gid, " +
                          " tax_name, " +
                          " tax_name2, " +
                          " tax_name3, " +
                          " tax_percentage, " +
                          " tax_percentage2, " +
                          " tax_percentage3, " +
                          " tax_amount, " +
                          " tax_amount2, " +
                          " tax_amount3, " +
                           " discount_amount, " +
                           " discount_percentage" +
                           ")values(" +
                           "'" + msGetGid + "'," +
                           "'" + data.salesorder_gid + "'," +
                           "'" + employee_gid + "'," +
                           "'" + lsproductgid + "'," +
                           "'" + lsproductgroupgid + "'," +
                           "'" + data.productgroup_name + "'," +
                           "'" + data.product_code + "'," +
                           "'" + data.product_name + "'," +
                           "'" + data.unitprice + "'," +
                           "'" + data.quantity + "'," +
                           "'" + lsproductuomgid + "'," +
                           "'" + data.productuom_name + "'," +
                           "'" + data.total_amount + "'," +
                           " '" + lsorder_type + "', " +
                           " '" + values.taxsegment_gid + "', " +                           
                           " '" + values.taxgid1 + "', " +
                           " '" + values.taxgid2 + "', " +
                           " '" + values.taxgid3 + "', " +
                          " '" + values.taxname1 + "', " +
                           " '" + values.taxname2 + "', " +
                           " '" + values.taxname3 + "', " +
                           " '" + values.taxprecentage1 + "', " +                                             
                           " '" + values.taxprecentage2 + "', " +                                                                     
                           " '" + values.taxprecentage3 + "', " +                                                                                             
                           " '" + values.taxamount1 + "', " + 
                           " '" + values.taxamount2 + "', " + 
                           " '" + values.taxamount3 + "', " +
                           "'" + discount_amount + "'," +
                           "'" + discount_precentage + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Product";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }        
        public void DaGetViewsalesorderSummary(string salesorder_gid, MdlSmrTrnCustomerSO values)
        {
            try
            {
                msSQL = " SELECT distinct a.salesorder_gid, h.currency_code,a.billing_email, a.customerbranch_gid, a.exchange_rate, " +
                    " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') AS salesorder_date, a.salesperson_gid, " +
                    " CONCAT(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name, " +
                    " FORMAT(a.Grandtotal, 2) AS Grandtotal, a.termsandconditions, FORMAT(a.addon_charge, 2) AS addon_charge, " +
                    " FORMAT(a.additional_discount_l, 2) AS additional_discount, a.payment_days, FORMAT(a.tax_amount, 2) AS tax_amount, " +
                    " a.delivery_days, a.so_referenceno1, a.payment_terms, a.freight_terms, " +
                    " FORMAT(a.roundoff, 2) AS roundoff, a.so_remarks, a.shipping_to, " +
                    " a.customer_address, a.customer_name,  a.customer_contact_person AS customer_contact_person, " +
                    " CASE WHEN a.start_date = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.start_date, '%d-%m-%Y') END AS start_date,CASE WHEN a.end_date = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.end_date, '%d-%m-%Y') END AS end_date, " +
                    " a.termsandconditions, a.customer_mobile, a.customer_email, e.branch_name, a.order_instruction," +
                    " FORMAT(a.total_amount, 2) AS total_amount, FORMAT(a.freight_charges, 2) AS freight_charges, " +
                    " FORMAT(a.packing_charges, 2) AS packing_charges,format(a.total_price, 2) as total_price,a.tax_name, FORMAT(a.buyback_charges, 2) AS buyback_charges, " +
                    " FORMAT(a.insurance_charges, 2) AS insurance_charges FROM cst_trn_tsalesorder a " +
                    " LEFT JOIN cst_trn_tsalesorderdtl d ON d.salesorder_gid = a.salesorder_gid " +
                    " LEFT JOIN hrm_mst_tbranch e ON e.branch_gid = a.branch_gid " +
                    " LEFT JOIN acp_mst_ttax f ON f.tax_gid = a.tax_gid " +
                    " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.salesperson_gid " +
                    " LEFT JOIN crm_trn_tcurrencyexchange h ON a.currency_gid = h.currencyexchange_gid " +
                    " WHERE a.salesorder_gid = '" + salesorder_gid + "' GROUP BY a.salesorder_gid, d.product_gid ORDER BY a.salesorder_gid ASC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Customerpostsalesorder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Customerpostsalesorder_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            so_remarks = dt["so_remarks"].ToString(),
                            order_instruction = dt["order_instruction"].ToString(),
                        });
                        values.Customerpostsalesorder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }       
        public void GetDeleteDirectSOProductSummary(string tmpsalesorderdtl_gid, Customersalesorders_list values)
        {
            try
            {
                msSQL = "select price from smr_tmp_tsalesorderdtl " +
                    " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                   
                    lsprice = objOdbcDataReader["price"].ToString();
                    objOdbcDataReader.Close();
                    
                }
                msSQL = " delete from smr_tmp_tsalesorderdtl " +
                        " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Deleted Successfully!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting The Product!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting DirectSO product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }        
        public void DaPostSalesOrder(string customer_gid, Customerpostsales_list values)
        {
            try
            {
                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                string lscustomerbranch = "H.Q";
                string lscampaign_gid = "NO CAMPAIGN";

                msSQL = " select * from smr_tmp_tsalesorderdtl " +
                    " where employee_gid='" + customer_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);


                msSQL = " select customer_name from crm_mst_tcustomer where customer_gid='" + values.customer_gid + " '";
                string lscustomername = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select currency_code, exchange_rate from crm_trn_tcurrencyexchange where currencyexchange_gid='" + values.currency_code + " '";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if(objOdbcDataReader.HasRows == true)
                {
                    
                    currency_code = objOdbcDataReader["currency_code"].ToString();
                    exchange_rate = objOdbcDataReader["exchange_rate"].ToString();
                    objOdbcDataReader.Close();
                    
                }

                string lslocaladdon = "0.00";
                string lslocaladditionaldiscount = "0.00";
                string lslocalgrandtotal = " 0.00";
                string lsgst = "0.00";
                string lsamount4 = "0.00";
                string lsroundoff = "0.00";
                //string lsproducttotalamount = "0.00";

                string totalAmount = "0.00";
                string addonCharges = "0.00";
                string freightCharges = "0.00";
                string packingCharges = "0.00";
                string insuranceCharges = "0.00";
                string roundoff = "0.00";
                string additionaldiscountAmount = "0.00";
                string buybackCharges = "0.00";

                string grandTotal = "0.00";

                msSQL = " Select  a.customer_gid,c.customercontact_gid,a.customer_name, i.branch_name,i.branch_gid, c.customercontact_name,c.mobile,c.email,d.region_name, e.customer_type,a.customer_state, " +
                        " a.status as status,concat_ws( '\n', c.address1, c.address2, c.city, c.zip_code, c.country_code) as customer_address from crm_mst_tcustomer a " +
                        " left join crm_mst_tcustomercontact c on a.customer_gid=c.customer_gid " +
                        " left join crm_mst_tregion d on a.customer_region=d.region_gid " +
                        " left join  smr_trn_tsalesorder o on a.customer_gid = o.customer_gid " +
                        " left join  hrm_mst_tbranch i on o.branch_gid= i.branch_gid" +
                        " left join crm_mst_tcustomertype e on a.customer_type=e.customertype_gid  where a.customer_gid='" + customer_gid + "' and c.main_contact = 'Y' group by a.customer_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Customerpostsales_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Customerpostsales_list
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customercontact_gid = dt["customercontact_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customercontact_name = dt["customercontact_name"].ToString(),
                            customer_mobile = dt["mobile"].ToString(),
                            customer_email = dt["email"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            customer_state = dt["customer_state"].ToString(),
                            statuses = dt["status"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString()
                        });

                        if(getModuleList[0].branch_gid != null && getModuleList[0].branch_gid != "")
                        {

                        }
                        else
                        {
                            msSQL = "select distinct branch_gid from hrm_mst_tbranch";
                            getModuleList[0].branch_gid = objdbconn.GetExecuteScalar(msSQL);
                        }


                        lsrefno = objcmnfunctions.GetMasterGID("SOR");

                        mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP");

                        msSQL = " insert  into cst_trn_tsalesorder (" +
                                 " salesorder_gid ," +
                                 " branch_gid," +
                                 " salesorder_date," +
                                 " customer_gid," +
                                 " customer_name," +
                                 " customer_contact_gid," +
                                 " customerbranch_gid," +
                                 " customer_contact_person," +
                                 " customer_address," +
                                 " customer_email, " +
                                 " billing_email ," +
                                 " customer_mobile, " +
                                 " created_by," +
                                 " so_referenceno1 ," +
                                 " order_instruction," +
                                 " Grandtotal, " +
                                 " total_amount," +
                                 " salesorder_status, " +
                                 " addon_charge, " +
                                 " additional_discount, " +
                                 " addon_charge_l, " +
                                 " additional_discount_l, " +
                                 " grandtotal_l, " +
                                 " gst_amount," +
                                 " roundoff, " +
                                 " updated_addon_charge, " +
                                 " updated_additional_discount, " +
                                 " freight_charges," +
                                 " buyback_charges," +
                                 " packing_charges," +
                                 " insurance_charges, " +
                                 " currency_code, " +
                                 " exchange_rate, " +
                                 " currency_gid, " +
                                 "created_date" +
                                 " )values(" +
                                 " '" + mssalesorderGID + "'," +
                                  " '" + getModuleList[0].branch_gid + "'," +
                                 " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                 " '" + getModuleList[0].customer_gid + "'," +
                                 " '" + (String.IsNullOrEmpty(getModuleList[0].customer_name)? getModuleList[0].customer_name : getModuleList[0].customer_name.Replace("'","\\\'")) + "'," +
                                 " '" + lscustomerbranch + "'," +
                                 " '" + getModuleList[0].customercontact_gid + "'," +
                                 " '" + (String.IsNullOrEmpty(values.contact_person) ? values.contact_person : values.contact_person.Replace("'", "\\\'")) + "'," +
                                 " '" + (String.IsNullOrEmpty(getModuleList[0].customer_address) ? getModuleList[0].customer_address : getModuleList[0].customer_address.Replace("'", "\\\'")) + "'," +
                                 " '" + values.customermail + "'," +
                                   " '" + values.email + "'," +
                                 " '" + getModuleList[0].mobile + "'," +
                                 " '" + customer_gid + "'," +
                                 " '" + lsrefno + "'," +
                                 " '" + (String.IsNullOrEmpty(values.so_remarks) ? values.so_remarks : values.so_remarks.Replace("'", "\\\'")) + "'," +
                                 " '" + values.grandtotal.Replace(",", "").Trim() + "'," +
                                 " '" + values.grandtotal.Replace(",", "").Trim() + "'," +
                                 " 'Pending'," +
                                 "'"+ lslocaladdon + "',"+
                                 "'"+ lslocaladditionaldiscount + "',"+
                                 "'"+ lslocaladdon + "'," +
                                 "'"+ lslocaladditionaldiscount + "'," +
                                 "'"+ lslocalgrandtotal + "'," +                        
                                   "'" + lsgst + "'," +                        
                                   "'" + lsroundoff + "'," +                        
                                   "'" + lslocaladdon + "'," +                        
                                   "'" + lslocaladditionaldiscount + "'," +                        
                                   "'" + freightCharges + "'," +                        
                                   "'" + buybackCharges + "'," +                        
                                   "'" + packingCharges + "'," +                        
                                   "'" + insuranceCharges + "'," + 
                                   "'" + currency_code + "'," + 
                                   "'" + values.exchange + "'," + 
                                   "'" + values.currency_code + "'," + 
                                   " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = " Some Error Occurred While Inserting Salesorder Details";
                            return;
                        }                       
                    }
                }


                msSQL = " select " +
                        " tmpsalesorderdtl_gid," +
                        " salesorder_gid," +
                        " productgroup_gid," +
                        " productgroup_name," +
                        " product_gid," +
                        " product_name," +
                         " product_code," +
                        " product_price," +
                        " qty_quoted," +
                        " discount_percentage," +
                        " discount_amount," +
                        " uom_gid," +
                        " uom_name," +
                        " price," +
                        " tax_name," +
                        " tax_name2," +
                        " tax_name3," +
                        " tax1_gid, " +
                        " tax_amount," +
                        " slno," +
                        " tax_percentage," +
                        " tax_percentage2," +
                        " tax_percentage3," +
                        " tax_amount2," +
                        " tax_amount3," +
                        " tax2_gid," +
                        " tax3_gid," +
                        " order_type, " +
                        " taxsegment_gid, " +
                        " taxsegmenttax_gid " +
                        " from smr_tmp_tsalesorderdtl" +
                        " where employee_gid='" + customer_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Customerpostsales_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax2_gid = dt["tax2_gid"].ToString(),
                            tax3_gid = dt["tax3_gid"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            totalamount = dt["price"].ToString(),
                            order_type = dt["order_type"].ToString(),
                            slno = dt["slno"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegmenttax_gid = dt["taxsegmenttax_gid"].ToString(),
                        });

                        int i = 0;

                        mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                        if (mssalesorderGID1 == "E")
                        {
                            values.message = "Create Sequence code for VSDC ";
                            return;
                        }
                        msSQL = " insert into cst_trn_tsalesorderdtl (" +
                             " salesorderdtl_gid ," +
                             " salesorder_gid," +
                             " product_gid ," +
                             " productgroup_gid ," +
                             " productgroup_name ," +
                             " product_name," +
                             " product_code," +
                             " product_price," +
                             " qty_quoted," +
                             " discount_percentage," +
                             " discount_amount," +
                             " tax_amount ," +
                             " uom_gid," +
                             " uom_name," +
                             " price," +
                             " tax_name," +
                             " tax_name2," +
                             " tax_name3," +
                             " tax1_gid," +
                             " slno," +
                             " tax_percentage," +
                             " tax_percentage2," +
                             " tax_percentage3," +
                             " tax_amount2," +
                             " tax_amount3," +
                             " tax2_gid," +
                             " tax3_gid," +
                             " taxsegment_gid," +
                             " taxsegmenttax_gid," +
                             " type " +
                             ")values(" +
                             " '" + mssalesorderGID1 + "'," +
                             " '" + mssalesorderGID + "'," +
                             " '" + dt["product_gid"].ToString() + "'," +
                             " '" + dt["productgroup_gid"].ToString() + "'," +
                             " '" + dt["productgroup_name"].ToString() + "'," +
                             " '" + (String.IsNullOrEmpty(dt["product_name"].ToString()) ? dt["product_name"].ToString() : dt["product_name"].ToString().Replace("'", "\\\'")) + "'," +
                             " '" + (String.IsNullOrEmpty(dt["product_code"].ToString()) ? dt["product_code"].ToString() : dt["product_code"].ToString().Replace("'", "\\\'")) + "'," +
                             " '" + dt["product_price"].ToString() + "'," +
                             " '" + dt["qty_quoted"].ToString() + "'," +
                             " '" + dt["discount_percentage"].ToString() + "'," +
                             " '" + dt["discount_amount"].ToString() + "'," +
                             " '" + dt["tax_amount"].ToString() + "'," +
                             " '" + dt["uom_gid"].ToString() + "'," +
                             " '" + dt["uom_name"].ToString() + "'," +
                             " '" + dt["price"].ToString() + "'," +
                             " '" + dt["tax_name"].ToString() + "'," +
                             " '" + dt["tax_name2"].ToString() + "'," +
                             " '" + dt["tax_name3"].ToString() + "'," +
                             " '" + dt["tax1_gid"].ToString() + "'," +
                             " '" + i + 1 + "'," +
                             " '" + dt["tax_percentage"].ToString() + "'," +
                             " '" + dt["tax_percentage2"].ToString() + "'," +
                             " '" + dt["tax_percentage3"].ToString() + "'," +
                             " '" + dt["tax_amount2"].ToString() + "'," +
                             " '" + dt["tax_amount3"].ToString() + "'," +
                             " '" + dt["tax2_gid"].ToString() + "'," +
                             " '" + dt["tax3_gid"].ToString() + "'," +
                             " '" + dt["taxsegment_gid"].ToString() + "'," +
                             " '" + dt["taxsegmenttax_gid"].ToString() + "'," +
                             " '" + dt["order_type"].ToString() + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Error occurred while Insertion";
                            return;
                        }                     
                    }
                }

                msSQL = "select distinct type from cst_trn_tsalesorderdtl where salesorder_gid='" + mssalesorderGID + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows == true)
                {
                    lsorder_type = "Sales";
                }
                else
                {
                    lsorder_type = "Service";
                }
                msSQL = " update cst_trn_tsalesorder set so_type='" + lsorder_type + "' where salesorder_gid='" + mssalesorderGID + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0 || mnResult == 0)
                {

                    msSQL = " delete from smr_tmp_tsalesorderdtl ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Sales Order  Raised Successfully";
                    return;
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Raising Sales Order";
                    return;
                }              
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void datrucatetmpproductsummary(string employee_gid, string customer_gid)
        {
            msSQL = " delete from smr_tmp_tsalesorderdtl ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult == 1)
            {
            }

        }       
        public void daorderstatuscount(string customer_gid, MdlSmrTrnCustomerSO values)
        {
            msSQL = "SELECT (SELECT COUNT(salesorder_status)  FROM cst_trn_tsalesorder " +
                " WHERE customer_gid = '" + customer_gid + "') AS total_count," +
                " (SELECT COUNT(*) FROM cst_trn_tsalesorder WHERE customer_gid = '" + customer_gid + "'" +
                " AND salesorder_status = 'Completed') AS delivery_count, (SELECT COUNT(*) FROM cst_trn_tsalesorder " +
                " WHERE customer_gid = '" + customer_gid + "' AND salesorder_status = 'Pending') AS pending_count";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<orderstatuscount_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new orderstatuscount_list
                    {
                        deliverycount = dt["delivery_count"].ToString(),
                        pendingcount = dt["pending_count"].ToString(),
                        totalcount = dt["total_count"].ToString()
                    });
                    values.orderstatuscount_list = getModuleList;
                }
            }
            dt_datatable.Dispose();

        }        
    }
}