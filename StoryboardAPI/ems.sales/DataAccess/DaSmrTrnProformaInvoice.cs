using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnProformaInvoice
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        DataTable dt_datatable;
        DataTable DataTable6 = new DataTable();
        DataTable DataTable5 = new DataTable();
        int mnResult;
        string msSQL, msGetGid, msINGetGID, lscustomerproduct_code, lsproduct_name,path, lsproductuom_name, lsproductuom_gid,lsproductgid;
        string company_logo_path, authorized_sign_path;
        Image company_logo,authorised_signedlogo;
        OdbcDataReader objMySqlDataReader;
        public void DaGetProformaInvoiceSummary(MdlSmrTrnProformaInvoice values)
        {
            try
            {
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag," +
                    " a.mail_status,a.customer_gid,date_format(a.invoice_date, '%d-%m-%Y') as invoice_date,a.invoice_reference,a.additionalcharges_amount," +
                    " a.discount_amount,  a.invoice_flag as overall_status, a.mail_status, a.payment_flag," +
                    " format(a.initialinvoice_amount,2) as initialinvoice_amount," +
                    " case when a.invoicepercent_amount <> 0 then format(a.invoicepercent_amount,2)  else format(a.invoice_amount,2) end as invoice_amount," +
                    " a.customer_contactperson,j.so_referencenumber," +
                    " case when a.currency_code = '"+currency+"' then a.customer_name when a.currency_code is null then a.customer_name  when a.currency_code is not null" +
                    " and a.currency_code <> '"+currency+"' then concat(a.customer_name) end as customer_name,  case when a.customer_contactnumber is null then e.mobile when a.customer_contactnumber" +
                    " is not null then a.customer_contactnumber end as mobile,a.invoice_from  from rbl_trn_tproformainvoice a" +
                    " left join rbl_trn_tproformainvoicedtl b on a.invoice_gid = b.invoice_gid" +
                    " left join rbl_trn_tso2proformainvoice f on f.invoice_gid=a.invoice_gid" +
                    " left join smr_trn_tdeliveryorder d on d.directorder_gid = f.directorder_gid" +
                    " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid" +
                    " left join crm_mst_tcustomercontact e on e.customer_gid=c.customer_gid" +
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code" +
                    " left join smr_trn_tsalesorder j on j.salesorder_gid=f.directorder_gid" +
                    " where 0=0  order by invoice_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<proformainvoicesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new proformainvoicesummary_list
                        {
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_reference = dt["invoice_reference"].ToString(),
                            so_referencenumber = dt["so_referencenumber"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contactperson = dt["customer_contactperson"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            mail_status = dt["mail_status"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                        });
                        values.proformainvoicesummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while loading Proformal Invoice!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaProformaInvoiceSubmit(string employee_gid, proformainvoicelist values)
        {
            try
            {


                msINGetGID = objcmnfunctions.GetMasterGID("SIPT");
                string ls_referenceno = objcmnfunctions.GetMasterGID("PRIN");

                msSQL = "select customer_name from crm_mst_tcustomer where customer_gid = '"+values.customer_gid+"'";
                string lscustomer_name = objdbconn.GetExecuteScalar(msSQL);

                string uiDateStr = values.invoice_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string invoice_date = uiDate.ToString("yyyy-MM-dd");

                string uidate = values.due_date;
                DateTime UIdate = DateTime.ParseExact(uidate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string Due_Date = UIdate.ToString("yyyy-MM-dd");

                msSQL = "insert into rbl_trn_tproformainvoice (" +
                        " invoice_gid," +
                        " branch_gid," +
                        " invoice_refno," +
                        " invoice_reference, " +
                        " user_gid," +
                        " invoice_date," +
                        " payment_date," +
                        " freightcharges_amount," +
                        " additionalcharges_amount," +
                        " discount_amount," +
                        " total_amount," +
                        " payment_term," +
                        " created_date," +
                        " customer_gid," +
                        " customer_name," +
                        " customer_contactperson," +
                        " customer_contactnumber," +
                        " advance_amount," +
                        " invoice_remarks," +
                        " currency_code," +
                        " exchange_rate," +
                        " total_amount_L," +
                        " invoice_amount," +
                        " invoice_amount_L," +
                        " discount_amount_L," +
                        " customer_address," +
                        " termsandconditions," +
                        " customer_email," +
                        " advance_roundoff," +
                        " roundoff," +
                        " mode_of_despatch," +
                        " freight_terms," +
                        " ship_to," +
                        " billing_email," +
                        " sales_type," +
                        " tax_amount4," +
                        " tax_name4," +
                        " total_price," +
                        " freight_charges" +
                        " ) values (" +
                        "'" + msINGetGID + "'," +
                        "'" + values.branch_gid + "'," +
                        "'" + ls_referenceno + "'," +
                        "'" + values.salesorder_gid + "'," +
                        "'" + employee_gid + "'," +
                        "'" + invoice_date + "'," +
                        "'" + Due_Date + "'," +
                        "'" + values.freight_charges + "'," +
                        "'" + values.addon_charge + "'," +
                        "'" + values.additional_discount + "'," +
                        "'" + values.grandtotal.ToString().Replace(",", "") + "'," +
                        "'" + (String.IsNullOrEmpty(values.payment_terms) ? values.payment_terms : values.payment_terms.Replace("'", "\\\'")) + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'" + values.customer_gid + "'," +
                        "'" + lscustomer_name.Replace("'","\\\'") + "'," +
                        "'" + values.customercontact_names.Replace("'", "\\\'") + "'," +
                        "'" + values.customer_mobile + "',";
                        if (values.proforma_advance_amount == "" || values.proforma_advance_amount == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.proforma_advance_amount + "',";
                        }

                        if (values.proforma_remarks == "" || values.proforma_remarks == null)
                        {
                             msSQL += "'" + values.proforma_remarks + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.proforma_remarks.Replace("'", "\\\'") + "',";
                        }
               
                      msSQL += "'" + values.currency_code + "'," +
                        "'" + values.exchange_rate + "'," +
                        "'" + values.grandtotal.ToString().Replace(",", "") + "'," +
                        "'" + values.grandtotal.ToString().Replace(",", "") + "'," +
                        "'" + values.grandtotal.ToString().Replace(",", "") + "'," +
                        "'" + values.additional_discount + "'," +
                        "'" + (String.IsNullOrEmpty(values.customer_address) ? values.customer_address : values.customer_address.Replace("'","\\\'")) + "'," +
                        "'" + (String.IsNullOrEmpty(values.termsandconditions) ? values.termsandconditions : values.termsandconditions.Replace("'", "\\\'")) + "'," +
                        "'" + values.customer_email + "',";
                if (values.proforma_advance_roundoff == "" || values.proforma_advance_roundoff == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.proforma_advance_roundoff + "',";
                }

                msSQL += "'" + values.roundoff + "'," +
                 "'" + (String.IsNullOrEmpty(values.dispatch_mode) ? values.dispatch_mode : values.dispatch_mode.Replace("'", "\\\'")) + "'," +
                 "'" + values.freight_terms.Replace("'","\\\'") + "'," +
                 "'" + (String.IsNullOrEmpty(values.shipping_to) ? values.shipping_to : values.shipping_to.Replace("'", "\\\'")) + "'," +
                 "'" + values.bill_email + "'," +
                 "'" + values.sales_type.Replace("'", "\\\'") + "',";
                if (values.tax_amount4 == "" || values.tax_amount4 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_amount4 + "',";
                }
                if (values.tax_name4 == "" || values.tax_name4 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_name4.Replace("'", "\\\'") + "',";
                }
               msSQL+= "'" + values.totalamount + "'," +
                        "'" + values.freight_charges + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = "SELECT  a.invoicedtl_gid,a.invoice_gid, a.product_gid, a.product_name," +
                  "a.customerproduct_code, a.product_remarks,a.product_code, b.productgroup_gid," +
                  " FORMAT(a.qty_invoice, 2) AS qty_invoice, c.productgroup_name, a.uom_name, b.producttype_gid," +
                  " a.created_by, format(a.product_price,2) as product_price, FORMAT(a.product_price, 2) AS producttotal_price," +
                  " a.discount_percentage,format(a.product_total,2) as product_total, format(a.discount_amount, 2) AS discount_amount," +
                  " a.tax_percentage, format(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field," +
                  " a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2,a.tax_percentage3," +
                  " format(a.tax_amount2, 2) AS tax_amount2, format(a.tax_amount3, 2) AS tax_amount3, a.tax1_gid," +
                  " a.tax2_gid, a.tax3_gid ," +
                  " concat( case when a.tax_name is null then '' else a.tax_name end, ' '," +
                  " case when a.tax_percentage = '0' then '' else concat('',a.tax_percentage,'%') end ," +
                  " case when a.tax_amount = '0' then '' else concat(':',a.tax_amount) end) as tax," +
                  " concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ', " +
                  " case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end, " +
                  " case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2," +
                  " concat(case when a.tax_name3 is null then '' else a.tax_name3 end, ' '," +
                  " case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                  " end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3," +
                  "format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.taxsegment_gid " +
                  " FROM rbl_tmp_tinvoicedtl a  LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                  " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                  " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid  " +
                  " WHERE a.employee_gid = '" + employee_gid + "' and a.invoice_gid='" + values.salesorder_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("SIPC");

                        msSQL = " insert into rbl_trn_tproformainvoicedtl (" +
                                " invoicedtl_gid, " +
                                " invoice_gid, " +
                                " customerproduct_code," +
                                " product_gid, " +
                                " product_code, " +
                                " product_name, " +
                                " productuom_name, " +
                                " uom_gid, " +
                                " product_price, " +
                                " discount_percentage, " +
                                " discount_amount, " +
                                " tax_name, " +
                                " tax_name2, " +
                                " tax_percentage, " +
                                " tax_percentage2, " +
                                " tax_amount, " +
                                " tax_amount2, " +
                                " qty_invoice, " +
                                " product_total, " +
                                " product_price_L, " +
                                " discount_amount_L, " +
                                " tax_amount1_L, " +
                                " tax_amount2_L, " +
                                " product_total_L, " +
                                " created_by, " +
                                " display_field)" +
                                " values ( " +
                                "'" + msGetGid + "', " +
                                "'" + msINGetGID + "'," +
                                "'" + dt["customerproduct_code"].ToString() + "'," +
                                "'" + dt["product_gid"].ToString() + "'," +
                                "'" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                                "'" + dt["product_name"].ToString().Replace("'","\\\'") + "'," +
                                "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                                "'" + dt["uom_gid"].ToString() + "'," +
                                "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                                "'" + dt["discount_percentage"].ToString() + "'," +
                               "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                               "'" + dt["tax_name"].ToString() + "'," +
                                "'" + dt["tax_name2"].ToString() + "'," +
                                "'" + dt["tax_percentage"].ToString() + "'," +
                                "'" + dt["tax_percentage2"].ToString() + "'," +
                                "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                                "'" + dt["qty_invoice"].ToString() + "'," +
                                "'" + dt["product_total"].ToString().Replace(",", "") + "'," +
                                "'" + dt["product_total"].ToString().Replace(",", "") + "'," +
                                "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                                "'" + dt["product_total"].ToString().Replace(",", "") + "'," +
                                "'" + employee_gid + "'," +
                                "'" + dt["display_field"].ToString().Replace("'", "\\\'") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                   

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Invoice Added Successfully!!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occured in Submitting the Invoice!!";

                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Proformal Invoice!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        //Edit
        public void DaGetProformaInvoiceEditdata(string employee_gid,MdlSmrTrnProformaInvoice values, string invoice_gid)
        {
            try
            {
                msSQL = "DELETE FROM rbl_tmp_tinvoicedtl where employee_gid ='" + employee_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0) 
                {
                    msSQL = "select invoicedtl_gid, invoice_gid, product_gid, qty_invoice, product_price, discount_percentage, " +
                        "discount_amount, tax_percentage, tax_amount, excise_percentage, excise_amount, product_total, uom_gid," +
                        " invoice_reference, tax_percentage2, tax_amount2, tax_percentage3, tax_amount3, tax_name, tax_name2," +
                        " tax_name3, display_field, product_remarks, product_price_L, discount_amount_L, tax_amount1_L, tax_amount2_L," +
                        " tax_amount3_L, product_total_L, so_amount, tax1_gid, tax2_gid, tax3_gid, customerproduct_code, created_by," +
                        " created_date, updated_by, updated_date, product_code, product_name, productuom_name, productgroup_gid," +
                        " selling_price, productgroup_name, vendor_price from rbl_trn_tproformainvoicedtl where invoice_gid = '"+invoice_gid+ "' group by product_gid";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("SIPC");


                            msSQL = " insert into rbl_tmp_tinvoicedtl ( " +
                                    " invoicedtl_gid, invoice_gid, product_gid," +
                                    " qty_invoice,product_price, discount_percentage," +
                                    " discount_amount, tax_amount, product_total," +
                                    " uom_gid, uom_name, tax_amount2, tax_amount3," +
                                    " tax_name, tax_name2,tax_name3,display_field," +
                                    " tax1_gid, tax2_gid, tax3_gid, product_name," +
                                    " productgroup_gid," +
                                    " productgroup_name, " +
                                    " employee_gid, " +
                                    " selling_price, " +
                                    " product_code," +
                                    " customerproduct_code, " +
                                    " tax_percentage," +
                                    " tax_percentage2," +
                                    " tax_percentage3 ," +
                                    " vendor_price, " +
                                    " product_remarks " +
                                    " ) values ( " +
                                    "'" + msGetGid + "'," +
                                    "'" + invoice_gid + "'," +
                                    "'" + dt["product_gid"].ToString() + "'," +
                                    "'" + dt["qty_invoice"].ToString() + "'," +
                                    "'" + dt["product_price"].ToString() + "'," +
                                    "'" + dt["discount_percentage"].ToString() + "'," +
                                    "'" + dt["discount_amount"].ToString() + "'," +
                                    "'" + dt["tax_amount"].ToString() + "'," +
                                    "'" + dt["product_total"].ToString() + "'," +
                                    "'" + dt["uom_gid"].ToString() + "'," +
                                    "'" + dt["productuom_name"].ToString().Replace("'", "\\\'") + "'," +
                                    "'" + dt["tax_amount2"].ToString() + "'," +
                                    "'" + dt["tax_amount3"].ToString() + "'," +
                                    "'" + dt["tax_name"].ToString() + "'," +
                                    "'" + dt["tax_name2"].ToString() + "'," +
                                    "'" + dt["tax_name3"].ToString() + "'," +
                                    "'" + dt["display_field"].ToString().Replace("'", "\\\'") + "'," +
                                    "'" + dt["tax1_gid"].ToString() + "'," +
                                    "'" + dt["tax2_gid"].ToString() + "'," +
                                    "'" + dt["tax3_gid"].ToString() + "'," +
                                    "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                                    "'" + dt["productgroup_gid"].ToString() + "'," +
                                    "'" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "'," +
                                    "'" + employee_gid + "',";
                                    if (string.IsNullOrEmpty(dt["selling_price"].ToString()))
                                    {
                                        msSQL += "'0.00',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + dt["selling_price"].ToString() + "',";
                                    }
                           
                                 msSQL+= "'" + dt["product_code"].ToString() + "'," +
                                    "'" + dt["customerproduct_code"].ToString() + "'," +
                                    "'" + dt["tax_percentage"].ToString() + "'," +
                                    "'" + dt["tax_percentage2"].ToString() + "'," +
                                    "'" + dt["tax_percentage3"].ToString() + "',";

                                    if (string.IsNullOrEmpty(dt["vendor_price"].ToString()))
                                    {
                                        msSQL += "'0.00',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + dt["vendor_price"].ToString() + "',";
                                
                                    }
                           
                                    if (dt["product_remarks"].ToString() != null)
                                    {
                                       msSQL += "'" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "')";
                                    }
                                    else
                                    {
                                       msSQL += "'" + dt["product_remarks"].ToString() + "')";
                                    }


                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                    }

                }

                double grand_total = 0.00;
                msSQL = " select c.gst_number,d.mobile as mobile_number,a.customer_gid,g.so_referenceno1,a.invoice_gid,a.invoice_refno,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,a.payment_term,date_format(a.payment_date,'%d-%m-%Y')as payment_date, " +
                    " g.salesorder_gid,format(a.total_price,2) as total_price,g.gst_amount,format(a.total_amount, 2) as total_amount,g.salesorder_gid,a.termsandconditions, " +
                    " concat(j.user_code, ' ', '/', ' ', j.user_firstname, ' ', j.user_lastname) as user_firstname,  " +
                    " format(a.additionalcharges_amount, 2) as additionalcharges_amount,format(a.discount_amount, 2) as discount_amount1 ,format(a.invoice_amount, 2) as invoice_amount, " +
                    " a.customer_name,a.customer_contactperson,a.customer_email,a.customer_address,a.invoice_percent,f.branch_name,g.so_referencenumber,date_format(g.start_date,'%d-%m-%Y') as start_date,date_format(g.end_date,'%d-%m-%Y') as end_date,g.freight_terms,g.payment_terms," +
                    " case when a.customer_contactnumber is null then d.mobile when a.customer_contactnumber is not null then a.customer_contactnumber end as mobile,a.currency_code,a.exchange_rate,  " +
                    " a.invoice_remarks,format(a.roundoff, 2) as roundoff,  " +
                    " format(a.freight_charges, 2) as freight_charges, " +
                    " format(a.buyback_charges, 2) as buyback_charges,  " +
                    " format(a.packing_charges, 2) as packing_charges, " +
                    " format(a.insurance_charges, 2) as insurance_charges, " +
                    " format(a.advance_roundoff, 2) as advanceroundoff,format(a.invoicepercent_amount, 2) as invoicepercent_amount ," +
                    " h.invoicedtl_gid,h.qty_invoice,format(h.product_price, 2) as product_price1, " +
                    " h.customerproduct_code,h.discount_percentage,format(h.discount_amount, 2) as discount_amount, " +
                    " format(h.tax_amount, 2) as tax_amount,format(h.tax_amount2, 2) as tax_amount2 ,format(h.tax_amount3, 2) as tax_amount3,h.tax_name, " +
                    " h.tax_name2,h.tax_name3,format((h.product_price + h.discount_amount), 2) as vendor_price, " +
                    " format(((h.product_price * h.qty_invoice) + h.tax_amount + h.tax_amount2 + h.tax_amount3), 2) as price,h.display_field,  " +
                    " h.product_gid,h.product_price, h.product_code, h.product_name,i.productgroup_gid,m.productgroup_name, " +
                    " h.productuom_name,h.product_total,h.uom_gid ,a.mode_of_despatch,g.currency_gid," +
                    " a.branch_gid,a.ship_to,a.billing_email,a.sales_type,a.freight_terms as freight," +
                    " i.cost_price,a.tax_amount4,a.tax_name4,x.currencyexchange_gid,x.currency_code as code " +
                    " from rbl_trn_tproformainvoice a " +
                    " left join rbl_trn_tso2proformainvoice b on b.invoice_gid = a.invoice_gid " +
                    " left join crm_mst_tcustomer c on c.customer_gid = a.customer_gid " +
                    " left join crm_mst_tcustomercontact d on d.customer_gid = a.customer_gid " +
                    " left join rbl_trn_tinvoicedtl e on e.invoice_gid = a.invoice_gid " +
                    " left join smr_trn_tsalesorder g on g.salesorder_gid =  a.invoice_reference  " +
                    " left join hrm_mst_tbranch f on f.branch_gid = g.branch_gid " +
                    " left join adm_mst_tuser j on j.user_gid = g.salesperson_gid " +
                    " left join rbl_trn_tproformainvoicedtl h  on a.invoice_gid = h.invoice_gid " +
                    " left join pmr_mst_tproduct i on i.product_gid = h.product_gid " +
                    " left join pmr_mst_tproductuom k on k.productuom_gid = h.uom_gid" +
                    " left join rbl_trn_tinvoice l on l.invoice_gid = h.invoice_gid " +
                    " left join pmr_mst_tproductgroup m on m.productgroup_gid = i.productgroup_gid " +
                    " left join crm_trn_tcurrencyexchange x on x.currencyexchange_gid = a.currency_code " +
                    " where a.invoice_gid = '" + invoice_gid + "' group by invoice_gid,product_name";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProformaInvoiceEditlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["product_total"].ToString());
                        getModuleList.Add(new ProformaInvoiceEditlist
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            total_price = dt["total_price"].ToString(),
                            invoicepercent_amount = dt["invoicepercent_amount"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            gst_amount = dt["gst_amount"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            discount_amount1 = dt["discount_amount1"].ToString(),
                            invoice_amount = dt["invoice_amount" ].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contactperson = dt["customer_contactperson"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            invoice_percent = dt["invoice_percent"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            so_referencenumber = dt["so_referencenumber"].ToString(),
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            mobile = dt["mobile_number"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            advanceroundoff = dt["advanceroundoff"].ToString(),
                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
                            product_price1 = dt["product_price1"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            vendor_price = dt["vendor_price"].ToString(),
                            price = dt["price"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            freight = dt["freight"].ToString(),
                            sales_type = dt["sales_type"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            ship_to = dt["ship_to"].ToString(),
                            mode_of_despatch = dt["mode_of_despatch"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            currency_gid = dt["currency_gid"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            tax_amount4 = dt["tax_amount4"].ToString(),
                            tax_name4 = dt["tax_name4"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            code = dt["code"].ToString(),
                        });
                        values.ProformaInvoiceEditlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.grand_total = grand_total;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while edit Proformal Invoice!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        //EditProduct
        public void DaGetOrderToProformaInvoiceProductDetails(string employee_gid, string invoice_gid, MdlSmrTrnProformaInvoice values)
        {
            try
            {


                double grand_total = 0.00;
                double grandtotal = 0.00;

                msSQL = "SELECT  a.invoicedtl_gid,a.invoice_gid, a.product_gid, a.product_name," +
                    "a.customerproduct_code, a.product_remarks,a.product_code, b.productgroup_gid," +
                    " FORMAT(a.qty_invoice, 2) AS qty_ordered, c.productgroup_name, a.uom_name, b.producttype_gid," +
                    " a.created_by, a.product_price, FORMAT(a.product_price, 2) AS producttotal_price," +
                    " a.discount_percentage,a.product_total, FORMAT(a.discount_amount, 2) AS discount_amount," +
                    " a.tax_percentage, FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field," +
                    " a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2,a.tax_percentage3," +
                    " FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3, a.tax1_gid," +
                    " a.tax2_gid, a.tax3_gid ," +
                    " concat( case when a.tax_name is null then '' else a.tax_name end, ' '," +
                    " case when a.tax_percentage = '0' then '' else concat('',a.tax_percentage,'%') end ," +
                    " case when a.tax_amount = '0' then '' else concat(':',a.tax_amount) end) as tax," +
                    " concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ', " +
                    " case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end, " +
                    " case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2," +
                    " concat(case when a.tax_name3 is null then '' else a.tax_name3 end, ' '," +
                    " case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                    " end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3," +
                    "format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.taxsegment_gid " +
                    " FROM rbl_tmp_tinvoicedtl a  LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                    " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid  " +
                    " WHERE a.invoice_gid='" + invoice_gid + "'";
                var GetOrderToInvoiceProduct = new List<GetOrderToProformaInvoiceProduct_list>();
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable1.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable1.Rows)
                    {
                        grand_total += double.Parse(dt["product_total"].ToString());
                        grandtotal += double.Parse(dt["product_total"].ToString());
                        GetOrderToInvoiceProduct.Add(new GetOrderToProformaInvoiceProduct_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            tax1_gid = dt["tax1_gid"].ToString(),
                            tax2_gid = dt["tax2_gid"].ToString(),
                            tax3_gid = dt["tax3_gid"].ToString(),
                            qty_quoted = dt["qty_ordered"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            productprice = dt["product_price"].ToString(),
                            tax_amount1 = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            taxamount = dt["taxamount"].ToString(),
                            display_field = dt["display_field"].ToString(),
                        });
                        values.GetOrderToProformaInvoiceProduct_list = GetOrderToInvoiceProduct;
                        values.grand_total = Math.Round(grand_total, 2);
                        //values.grandtotal = Math.Round(grandtotal, 2);
                    }
                    dt_datatable1.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Order to invoice summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaPostproductDirectinvoice(string employee_gid, Proformainvoiceproductsubmit_list values)
        {
            try
            {
                double discount_precentage = double.TryParse(values.discountprecentage, out double discountprecentage) ? discountprecentage : 0;
                string lstax1, lstax2;
                double lsGrandTotal = (values.productquantity * values.unitprice) - values.discount_amount;



                msSQL = "select productgroup_name from pmr_mst_tproductgroup where productgroup_gid = '" + values.productgroup_gid + "'";
                string lsproductgroupname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT a.productuom_gid, a.product_gid,a.customerproduct_code, a.product_name, b.productuom_name" +
                    " ,a.productgroup_gid FROM pmr_mst_tproduct a " +
                 " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                 " WHERE a.product_gid = '" + values.product_name + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lsproductgid = objMySqlDataReader["product_gid"].ToString();
                    lsproductuom_gid = objMySqlDataReader["productuom_gid"].ToString();
                    lsproduct_name = objMySqlDataReader["product_name"].ToString();
                    lsproductuom_name = objMySqlDataReader["productuom_name"].ToString();
                    lscustomerproduct_code = objMySqlDataReader["customerproduct_code"].ToString();

                    objMySqlDataReader.Close();
                }
                if (values.taxname1 == "" || values.taxname1 == null)
                {
                    lstax1 = "0";
                }
                else
                {
                    msSQL = "select tax_prefix from acp_mst_ttax  where tax_gid='" + values.taxgid1 + "'";
                    lstax1 = objdbconn.GetExecuteScalar(msSQL);
                }

                if (values.taxname2 == "" || values.taxname2 == null)
                {
                    lstax2 = "0";
                }
                else
                {
                    msSQL = "select tax_prefix from acp_mst_ttax where tax_gid='" + values.taxgid2 + "'";
                    lstax2 = objdbconn.GetExecuteScalar(msSQL);
                }
                string invoicedtl_gid = objcmnfunctions.GetMasterGID("SIPC");
                //string invoice_gid = objcmnfunctions.GetMasterGID("SIPT");
                
                if (Convert.ToString(values.productquantity) == "undefined" || values.productquantity == null || Convert.ToString(values.productquantity) == "" || Convert.ToInt32(values.productquantity) < 1)
                {
                    values.status = false;
                    values.message = "Product quantity cannot be zero or empty.";
                    return;
                }
                else if (values.unitprice == null || Convert.ToString(values.unitprice) == "" || Convert.ToString(values.unitprice) == "undefined")
                {
                    values.status = false;
                    values.message = "Price cannot be left empty";
                    return;
                }
                else
                {
                    msSQL = " insert into rbl_tmp_tinvoicedtl ( " +
                        " invoicedtl_gid, invoice_gid, product_gid," +
                        " qty_invoice,product_price, discount_percentage," +
                        " discount_amount, tax_amount, product_total," +
                        " uom_gid, uom_name, tax_amount2, tax_amount3," +
                        " tax_name, tax_name2,tax_name3,display_field," +
                        " tax1_gid, tax2_gid, tax3_gid, product_name," +
                        " productgroup_gid," +
                        " productgroup_name, " +
                        " created_by, " +
                        " employee_gid, " +
                        " selling_price, " +
                        " product_code," +
                        " customerproduct_code, " +
                        " tax_percentage," +
                        " tax_percentage2," +
                        " tax_percentage3 ," +
                        " vendor_price, " +
                        " product_remarks " +
                        " ) values ( " +
                        "'" + invoicedtl_gid + "'," +
                        "'" + values.invoice_gid + "'," +
                        "'" + lsproductgid + "'," +
                        "'" + values.productquantity + "'," +
                        "'" + values.unitprice + "'," +
                        "'" + discount_precentage + "'," +
                        "'" + values.discount_amount + "'," +
                        "'" + values.taxamount1 + "'," +
                        "'" + values.producttotal_amount + "'," +
                        "'" + lsproductuom_gid + "'," +
                        "'" + lsproductuom_name + "'," +
                        "'" + values.taxamount2 + "'," +
                        "'" + values.taxamount3 + "'," +
                        "'" + lstax1 + "'," +
                        "'" + lstax2 + "'," +
                        "'" + values.tax_prefix3 + "',";
                    if (values.product_desc != null)
                    {
                        msSQL += "'" + values.product_desc.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.product_desc + "',";
                    }

                    msSQL +=
                      "'" + values.taxgid1 + "'," +
                      "'" + values.taxgid2 + "'," +
                      "'" + values.taxgid3 + "'," +
                      "'" + lsproduct_name.Replace("'", "\\\'") + "'," +
                      "'" + values.productgroup_gid + "'," +
                      "'" + lsproductgroupname.Replace("'", "\\\'") + "'," +
                      "'" + employee_gid + "'," +
                      "'" + employee_gid + "'," +
                      "'" + lsGrandTotal + "'," +
                      "'" + values.product_code.Replace("'", "\\\'") + "'," +
                      "'" + lscustomerproduct_code + "'," +
                      "'" + values.taxprecentage1 + "'," +
                      "'" + values.taxprecentage2 + "'," +
                      "'" + values.taxprecentage3 + "'," +
                      "'" + values.unitprice + "',";
                    if (values.product_desc != null)
                    {
                        msSQL += "'" + values.product_desc.Replace("'", "\\\'") + "')";
                    }
                    else
                    {
                        msSQL += "'" + values.product_desc + "')";
                    }

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
            catch (Exception ex)
            {
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaUpdateProformainvoice(string employee_gid,ProformaInvoiceEditlist values)
        {
            try
            {

                //msSQL = "delete from rbl_tmp_tinvoicedtl where invoice_gid = '" + values.invoice_gid+"'";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

               
               
                    msSQL = "select invoicedtl_gid, invoice_gid, product_gid, qty_invoice, product_price," +
                        " discount_percentage, discount_amount, tax_percentage, tax_amount, excise_percentage," +
                        " excise_amount, product_total, uom_gid, invoice_reference, tax_percentage2, tax_amount2," +
                        " tax_percentage3, tax_amount3, tax_name, tax_name2, tax_name3, display_field, product_remarks," +
                        " product_price_L, discount_amount_L, tax_amount1_L, tax_amount2_L, tax_amount3_L," +
                        " product_total_L, so_amount, tax1_gid, tax2_gid, tax3_gid, customerproduct_code," +
                        " created_by, created_date, updated_by, updated_date, product_code, product_name," +
                        " uom_name, productgroup_gid, selling_price, productgroup_name," +
                        " vendor_price from rbl_tmp_tinvoicedtl where invoice_gid = '" + values.invoice_gid+"'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    msSQL = "delete from rbl_trn_tproformainvoicedtl where invoice_gid ='" + values.invoice_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if(mnResult == 1) { 
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("SIPC");

                        msSQL = msSQL = " insert into  rbl_trn_tproformainvoicedtl (" +
                                " invoicedtl_gid, " +
                                " invoice_gid, " +
                                " product_gid, " +
                                " product_code, " +
                                " product_name, " +
                                " productuom_name, " +
                                " uom_gid, " +
                                " product_price, " +
                                " discount_percentage, " +
                                " tax_name, " +
                                " tax_name2, " +
                                " tax_name3, " +
                                " tax_percentage, " +
                                " tax_percentage2, " +
                                " tax_percentage3, " +
                                " tax_amount, " +
                                " tax_amount2, " +
                                " tax_amount3, " +
                                " qty_invoice, " +
                                " product_total, " +
                                " product_price_L, " +
                                " discount_amount_L, " +
                                " tax_amount1_L, " +
                                " tax_amount2_L, " +
                                " tax_amount3_L, " +
                                " product_total_L, " +
                                " display_field)" +
                                " values ( " +
                                "'" + msGetGid + "', " +
                                "'" + values.invoice_gid + "'," +
                                "'" + dt["product_gid"].ToString() + "'," +
                                "'" + dt["product_code"].ToString()+ "'," +
                                "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                                "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                                "'" + dt["uom_gid"].ToString() + "'," +
                                "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                                "'" + dt["discount_percentage"].ToString() + "'," +
                                "'" + dt["tax_name"].ToString() + "'," +
                                "'" + dt["tax_name2"].ToString() + "'," +
                                "'" + dt["tax_name3"].ToString() + "'," +
                                "'" + dt["tax_percentage"].ToString() + "'," +
                                "'" + dt["tax_percentage2"].ToString() + "'," +
                                "'" + dt["tax_percentage3"].ToString() + "'," +
                                "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount3"].ToString() + "'," +
                                "'" + dt["qty_invoice"].ToString() + "'," +
                                "'" + dt["product_total"].ToString().Replace(",", "") + "'," +
                                "'" + dt["product_price_L"].ToString().Replace(",", "") + "'," +
                                "'" + dt["discount_amount"].ToString() + "'," +
                                "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount3"].ToString() + "'," +
                                "'" + dt["product_total_L"].ToString().Replace(",", "") + "'," +
                                "'" + (String.IsNullOrEmpty(dt["display_field"].ToString()) ? dt["display_field"].ToString() : dt["display_field"].ToString().Replace("'", "\\\'")) + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    }
                }
                

                msSQL = "Select customer_name from crm_mst_tcustomer where customer_gid = '"+values.customer_gid+"'";
                string lscustomer_name = objdbconn.GetExecuteScalar(msSQL);

                string uiDateStr = values.invoice_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string invoice_date = uiDate.ToString("yyyy-MM-dd");

                string uidate = values.due_date;
                DateTime UIdate = DateTime.ParseExact(uidate,"dd-MM-yyyy",CultureInfo.InvariantCulture);
                string Due_Date = UIdate.ToString("yyyy-MM-dd");


                msSQL = "UPDATE rbl_trn_tproformainvoice SET " +
                          " branch_gid = '" + values.branch_name + "', " +
                          " invoice_reference = '" + values.salesorder_gid + "', " +
                          " invoice_date = '" + invoice_date + "', " +
                          " payment_date = '" + Due_Date + "', " +
                          " freightcharges_amount = '" + values.freight_charges + "', " +
                          " additionalcharges_amount = '" + values.addon_charge + "', " +
                          " discount_amount = '" + values.additional_discount + "', " +
                          " total_amount = '" + values.grandtotal.ToString().Replace(",", "") + "', " +
                          " payment_term = '" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "', " +
                          " created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                          " customer_gid = '" + values.customer_gid + "', " +
                          " customer_name = '" + lscustomer_name.Replace("'", "\\\'") + "', " +
                          " total_price = '" + values.totalamount + "', " +
                          " customer_contactperson = '" + values.customercontact_names.Replace("'", "\\\'") + "', " +
                          " customer_contactnumber = '" + values.customer_mobile + "', " +
                          " advance_amount = '" + (string.IsNullOrEmpty(values.proforma_advance_amount) ? "0.00" : values.proforma_advance_amount) + "', " +
                          " invoice_remarks = '" + (String.IsNullOrEmpty(values.proforma_remarks)?  values.proforma_remarks : values.proforma_remarks.Replace("'","\\\'")) + "', " +
                          " currency_code = '" + values.currency_code + "', " +
                          " exchange_rate = '" + values.exchange_rate + "', " +
                          " total_amount_L = '" + values.grandtotal.ToString().Replace(",", "") + "', " +
                          " invoice_amount = '" + values.grandtotal.ToString().Replace(",", "") + "', " +
                          " invoice_amount_L = '" + values.grandtotal.ToString().Replace(",", "") + "', " +
                          " discount_amount_L = '" + values.additional_discount + "', " +
                          " customer_address = '" + (String.IsNullOrEmpty(values.customer_address) ? values.customer_address : values.customer_address.Replace("'", "\\\'")) + "', " +
                          " termsandconditions = '" + (String.IsNullOrEmpty(values.termsandconditions) ? values.termsandconditions : values.termsandconditions.Replace("'", "\\\'")) + "', " +
                          " customer_email = '" + values.customer_email + "', " +
                          " advance_roundoff = '" + (string.IsNullOrEmpty(values.proforma_advance_roundoff) ? "0.00" : values.proforma_advance_roundoff) + "', " +
                          " roundoff = '" + values.roundoff + "', " +
                          " mode_of_despatch = '" + (String.IsNullOrEmpty(values.dispatch_mode) ? values.dispatch_mode : values.dispatch_mode.Replace("'", "\\\'")) + "', " +
                          " freight_terms = '" + values.delivery_days + "', " +
                          " ship_to = '" + (String.IsNullOrEmpty(values.shipping_to) ? values.shipping_to : values.shipping_to.Replace("'", "\\\'")) + "', " +
                          " billing_email = '" + values.bill_email + "', " +
                          " sales_type = '" + values.sales_type + "', " +
                          " tax_amount4 = '" + values.tax_amount4 + "', " +
                          " tax_name4 = '" + values.tax_name4 + "', " +
                          " freight_charges = '" + values.freight_charges + "' " +
                          " WHERE invoice_gid = '" + values.invoice_gid + "'";


                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if(mnResult == 1)
                {
                    msSQL = "delete from rbl_tmp_tinvoicedtl where employee_gid = '" + employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Invoice Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Proforma Invoice";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Proformal Invoice!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProformaInvoiceViewdata(MdlSmrTrnProformaInvoice values, string invoice_gid)
        {
            try
            {

                msSQL = " select z.tax_name,format(a.tax_amount4 ,2 ) as tax_amount4,c.gst_number,d.mobile as mobile_number,g.so_referenceno1,format(a.advance_amount,2) as advance_amount,a.invoice_gid,a.invoice_refno,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,a.payment_term,date_format(a.payment_date,'%d-%m-%Y')as payment_date," +
                " concat(j.user_code,' ','/',' ',j.user_firstname,' ',j.user_lastname) as user_firstname, " +
                " g.salesorder_gid,format(a.total_price,2) as total_price,g.gst_amount,format(a.total_amount,2)as total_amount,g.salesorder_gid,a.termsandconditions, " +
                " date_format(g.start_date,'%d/%m/%Y') as start_date , date_format(g.end_date,'%d/%m/%Y') as end_date, " +
                " format(a.additionalcharges_amount,2)as additionalcharges_amount,format(a.discount_amount,2)as discount_amount ,format(a.invoice_amount,2)as invoice_amount," +
                " a.customer_name,a.customer_contactperson,a.customer_email,a.customer_address,a.invoice_percent,f.branch_name,g.so_referencenumber,g.payment_terms,a.freight_terms," +
                " case when a.customer_contactnumber is null then d.mobile when a.customer_contactnumber is not null then a.customer_contactnumber end as mobile,a.currency_code,a.exchange_rate, " +
                " format(a.freight_charges,2)as freight_charges," +
                " format(a.buyback_charges,2)as buyback_charges," +
                " format(a.packing_charges,2)as packing_charges," +
                " format(a.insurance_charges,2)as insurance_charges,a.mode_of_despatch,a.sales_type,a.ship_to,a.billing_email,k.salestype_name," +
                " a.invoice_remarks,format(a.advance_roundoff,2) as advance_roundoff," +
                " format(a.roundoff,2) as roundoff,format(a.invoicepercent_amount,2) as invoicepercent_amount,format(e.tax_amount,2) as total_tax_amount " +
                " from rbl_trn_tproformainvoice a " +
                " left join rbl_trn_tso2proformainvoice b on b.invoice_gid=a.invoice_gid " +
                " left join crm_mst_tcustomer c on c.customer_gid=a.customer_gid " +
                " left join crm_mst_tcustomercontact d on d.customer_gid=a.customer_gid " +
                " left join rbl_trn_tinvoicedtl e on e.invoice_gid=a.invoice_gid" +
                " left join smr_trn_tsalesorder g on g.salesorder_gid=a.invoice_reference " +
                " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid " +
                " left join adm_mst_tuser j on j.user_gid  = g.salesperson_gid " +
                "  left join smr_trn_tsalestype k on k.salestype_gid  = a.sales_type " +
                " left join acp_mst_ttax z on a.tax_name4 =  z.tax_gid " +
                " left join crm_trn_tcurrencyexchange o on o.currencyexchange_gid = a.currency_code " +
                " where a.invoice_gid='" + invoice_gid + "' group by a.invoice_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProformaInvoiceEditlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ProformaInvoiceEditlist
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            advance_amount = dt["advance_amount"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            total_price = dt["total_price"].ToString(),
                            invoicepercent_amount = dt["invoicepercent_amount"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            gst_amount = dt["gst_amount"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            total_tax_amount = dt["total_tax_amount"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contactperson = dt["customer_contactperson"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            invoice_percent = dt["invoice_percent"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            so_referencenumber = dt["so_referencenumber"].ToString(),
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            mobile = dt["mobile_number"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            advanceroundoff = dt["advance_roundoff"].ToString(),
                            mode_of_despatch = dt["mode_of_despatch"].ToString(),
                            sales_type = dt["sales_type"].ToString(),
                            ship_to = dt["ship_to"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            salestype_name = dt["salestype_name"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            tax_amount4 = dt["tax_amount4"].ToString(),
                            tax_name4 = dt["tax_name"].ToString(),


                        });
                        values.ProformaInvoiceEditlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while edit Proformal Invoice!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProformaInvoiceProductsEditdata(MdlSmrTrnProformaInvoice values, string invoice_gid)                                                                                                               
        {
            try
            {

                msSQL = " select format(b.cost_price,2) as cost_price,format(a.product_total,2) as totalprice,a.invoicedtl_gid,a.product_remarks,a.invoice_gid,a.qty_invoice," +
               " (z.total_amount-(a.tax_amount+a.tax_amount2+a.tax_amount3)) as product_price,a.customerproduct_code,a.discount_percentage,format(a.discount_amount,2)as discount_amount, " +
               " format(h.tax_amount,2)as tax_amount,format(a.tax_amount2,2)as tax_amount2 ,format(a.tax_amount3,2)as tax_amount3,a.tax_name,a.tax_name2,a.tax_name3," +
               " format((a.product_price_L+a.discount_amount),2) as vendor_price, " +
               " format(((a.product_price_L*a.qty_invoice)+a.tax_amount+a.tax_amount2+a.tax_amount3),2)as price,a.display_field," +
               " a.product_gid, a.product_code, a.product_name,b.productgroup_gid,g.productgroup_name, " +
               " a.productuom_name,a.uom_gid,z.total_amount," +
               " format(h.product_price,2) as product_price1,format(h.discount_percentage,2) as product_discount_percentage ," +
               " format(h.discount_amount,2) as product_discount_amount,format(h.tax_amount,2) as total_tax_amount,format(h.price,2) as total_price,format(z.total_price, 2) as net_amount " +
               " from rbl_trn_tproformainvoicedtl a " +
               " left join rbl_trn_tproformainvoice z on z.invoice_gid=a.invoice_gid " +
               " left join smr_trn_tsalesorder d on d.salesorder_gid = z.invoice_reference " +
               " left join smr_trn_tsalesorderdtl h on h.salesorder_gid = d.salesorder_gid " +
               " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
               " left join pmr_mst_tproductuom c on c.productuom_gid=a.uom_gid" +
               " left join rbl_trn_tinvoice e on e.invoice_gid=a.invoice_gid" +
               " left join pmr_mst_tproductgroup g on g.productgroup_gid=b.productgroup_gid" +
               " where a.invoice_gid='" + invoice_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProformaInvoiceProductlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ProformaInvoiceProductlist
                        {
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            vendor_price = dt["vendor_price"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            margin_amount = dt["discount_amount"].ToString(),
                            margin_percentage = dt["discount_percentage"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            product_price1 = dt["product_price1"].ToString(),
                            total_price = dt["total_price"].ToString(),
                            product_discount_amount = dt["product_discount_amount"].ToString(),
                            product_discount_percentage = dt["product_discount_percentage"].ToString(),
                            net_amount = dt["net_amount"].ToString(),
                            total_tax_amount = dt["total_tax_amount"].ToString(),
                            totalprice = dt["totalprice"].ToString(),
                            cost_price = dt["cost_price"].ToString(),

                        });


                        values.ProformaInvoice_Productlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }


        public Dictionary<string, object> DaGetProformaInvoicePDF(string invoice_gid, string employee_gid, MdlSmrTrnProformaInvoice values)
        {
            var response = new Dictionary<string, object>();
            string full_path = null;

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();

            msSQL = " select a.invoice_gid,date_format(a.invoice_date,'%d-%m-%Y')as invoice_date,a.customer_gid,g.quotation_referenceno1, " +
                   " a.invoice_amount,f.salesorder_gid,f.so_referencenumber,a.invoice_refno, a.freightcharges_amount,format(a.total_price,2) as total_price,f.gst_amount," +
                   " a.total_amount, a.advance_amount, a.customer_name as customer_name, " +
                   " b.customer_code, b.tin_number, b.cst_number,   a.customer_address,f.customer_contact_person as customercontact_name, " +
                   " a.customer_email as email,f.customer_mobile as mobile,a.invoice_percent,a.roundoff,a.advance_roundoff,a.invoicepercent_amount," +
                   " format(if(a.freight_charges is null,'0.00',if(a.freight_charges='','0.00',cast(a.freight_charges as char))),2) as freight_charges, " +
                   " format(if(a.buyback_charges is null,'0.00',if(a.buyback_charges='','0.00',cast(a.buyback_charges as char))),2) as buyback_charges," +
                   " format(if(a.packing_charges is null,'0.00',if(a.packing_charges='','0.00',cast(a.packing_charges as char))),2) as packing_charges, " +
                   " if(a.insurance_charges is null,'0.00',if(a.insurance_charges='','0.00',cast(a.insurance_charges as char))) as insurance_charges," +
                   " format(if(a.additionalcharges_amount is null,'0.00', if(a.additionalcharges_amount='','0.00',cast(a.additionalcharges_amount as char))),2) as additionalcharges_amount," +
                   " format(if(a.discount_amount is null,'0.00', if(a.discount_amount='','0.00',cast(a.discount_amount as char))),2) as discount_amount," +
                   " date_format(f.salesorder_date,'%d-%m-%Y') as customerpo_date, ";

            msSQL += " a.invoice_reference as directorder_gid, a.termsandconditions,a.currency_code as designation,f.so_referenceno1,a.currency_code,format(a.tax_amount4,2) as DataColumn38,a.tax_name4 as DataColumn39,c.tax_name as DataColumn40 " +
                        " from rbl_trn_tproformainvoice a" +
                        " left join crm_mst_tcustomer b on b.customer_gid=a.customer_gid" +
                        " left join smr_trn_tsalesorder f on f.salesorder_gid=a.invoice_reference" +
                        " left join smr_trn_treceivequotation g on g.quotation_gid = f.quotation_gid " +
                        " left join acp_mst_ttax c on c.tax_gid = a.tax_name4 ";
            msSQL += " where a.invoice_gid='" + invoice_gid + "' ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = " select format(a.product_total,2) as product_total,e.invoice_gid,a.qty_invoice ,format(a.product_price,2) as product_price,a.discount_percentage,a.discount_amount as discount_amount," +
                   " a.tax_percentage,a.tax_amount,a.tax_percentage2,a.tax_amount2,a.tax_percentage3,a.tax_amount3," +
                   " a.tax_name,a.tax_name2,a.tax_name3,a.display_field,b.product_code,concat(a.product_name,' - ',a.display_field) as product_name,c.productgroup_name," +
                   " case when a.tax_amount2=0 then concat(a.tax_name, ' : ', CAST(a.tax_amount AS CHAR))" +
                   " else concat(a.tax_name, ' : ', CAST(a.tax_amount AS CHAR), '     ', a.tax_name2, ' : ', CAST(a.tax_amount2 AS CHAR)) end as all_taxes," +
                   " a.productuom_name, b.productgroup_gid" +
                   " from rbl_trn_tproformainvoicedtl a" +
                   " left join rbl_trn_tso2proformainvoice h on h.invoice_gid=a.invoice_gid" +
                   " left join rbl_trn_tproformainvoice e on e.invoice_gid=a.invoice_gid" +
                   " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                   " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid" +
                   " left join pmr_mst_tproductuom d on d.productuom_gid=a.uom_gid" +
                   " where a.invoice_gid='" + invoice_gid + "'  group by a.invoicedtl_gid order by a.invoicedtl_gid asc";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");

            msSQL = " SELECT  tax_name AS thirdtax_amount, SUM(tax_amount) AS total_tax_amount " +
                        " FROM ( " +
                        " SELECT tax_name, tax_amount " +
                        " FROM rbl_trn_tproformainvoicedtl  " +
                        " WHERE invoice_gid  = '" + invoice_gid + "'" +
                        " And Not (tax_name Like '%No%' AND tax_amount = 0) " +
                        " UNION ALL " +
                        " SELECT tax_name2 AS thirdtax_amount, tax_amount2 AS total_tax_amount " +
                        "  FROM rbl_trn_tproformainvoicedtl  " +
                        " WHERE invoice_gid  ='" + invoice_gid + "'" +
                        " And Not (tax_name2 LIKE '%No%' AND tax_amount2 = 0) " +
                        " UNION ALL " +
                        " SELECT tax_name3 AS thirdtax_amount, tax_amount3 AS total_tax_amount " +
                        " FROM rbl_trn_tproformainvoicedtl  " +
                        " WHERE invoice_gid  = '" + invoice_gid + "'" +
                        " AND NOT (tax_name3 LIKE '%No%' AND tax_amount3 = 0) " +
                        ") AS subquery " +
                        " GROUP BY tax_name";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable3");

            msSQL = " select a.branch_name,a.address1,a.city,a.state,a.gst_no, " +
                  " a.postal_code,a.contact_number,a.email,a.tin_number,a.cst_number,a.st_number from hrm_mst_tbranch a" +
                   " left join rbl_trn_tproformainvoice b on b.branch_gid = a.branch_gid " +
                      " where b.invoice_gid = '" + invoice_gid + "'";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable4");

            msSQL = " select a.authorized_sign_path from hrm_mst_tbranch a " +
                      " left join rbl_trn_tproformainvoice b on b.branch_gid = a.branch_gid " +
                      " where b.invoice_gid = '" + invoice_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            DataTable5.Columns.Add("authorized_sign_path", typeof(byte[]));
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow Dt in dt_datatable.Rows)
                {
                    authorized_sign_path = HttpContext.Current.Server.MapPath("../../../" + Dt["authorized_sign_path"].ToString().Replace("../../", ""));
                    if (System.IO.File.Exists(authorized_sign_path) == true)
                    {
                        //Convert  Image Path to Byte
                        authorised_signedlogo = System.Drawing.Image.FromFile(authorized_sign_path);
                        byte[] authorizedsignedlogo_bytes = (byte[])(new ImageConverter()).ConvertTo(authorised_signedlogo, typeof(byte[]));
                        DataRow newRow = DataTable5.NewRow();
                        newRow["authorized_sign_path"] = authorizedsignedlogo_bytes;
                        DataTable5.Rows.Add(newRow);


                    }
                }
            }

            DataTable5.TableName = "DataTable5";
            myDS.Tables.Add(DataTable5);





            msSQL = "  select c.company_logo_path from hrm_mst_tbranch a" +
                    " left join rbl_trn_tproformainvoice b on a.branch_gid=b.branch_gid" +
                    " left join adm_mst_tcompany c on 1=1  where b.invoice_gid='"+invoice_gid+"';";
             dt_datatable = objdbconn.GetDataTable(msSQL);
            DataTable6.Columns.Add("authoriser_sign", typeof(byte[]));
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow Dt in dt_datatable.Rows)
                {
                   company_logo_path = HttpContext.Current.Server.MapPath("../../../" + Dt["company_logo_path"].ToString().Replace("../../", ""));
                   if (System.IO.File.Exists(company_logo_path) == true)
                    {
                        //Convert  Image Path to Byte
                        company_logo = System.Drawing.Image.FromFile(company_logo_path);
                        byte[] companylogo_bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));
                        DataRow newRow = DataTable6.NewRow();
                        newRow["authoriser_sign"] = companylogo_bytes;
                        DataTable6.Rows.Add(newRow);


                    }
                }
            }
            
            DataTable6.TableName = "DataTable6";
            myDS.Tables.Add(DataTable6);

            try
            {

                ReportDocument oRpt = new ReportDocument();
                string base_pathOF_currentFILE = AppDomain.CurrentDomain.BaseDirectory;
                string report_path = Path.Combine(base_pathOF_currentFILE, "ems.sales", "Smr_Trn_SalesProformaInvoice.rpt");

                if (!File.Exists(report_path))
                {
                    values.status = false;
                    values.message = "Your Rpt path not found !!";
                    response = new Dictionary<string, object>
                    {
                        {"status",false },
                        {"message",values.message}
                    };

                }
                oRpt.Load(report_path);
                oRpt.SetDataSource(myDS);


                path = Path.Combine(ConfigurationManager.AppSettings["report_path"]?.ToString());

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string PDFfile_name = "Invoice.pdf";
                full_path = Path.Combine(path, PDFfile_name);

                oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, full_path);
                myConnection.Close();
                response = objFnazurestorage.reportStreamDownload(full_path);
                values.status = true;

            }
            catch (Exception Ex)
            {
                values.status = false;
                values.message = Ex.Message;
                response = new Dictionary<string, object>
                {
                     { "status", false },
                     { "message", Ex.Message }
                };
            }
            finally
            {
                if (full_path != null)
                {
                    try
                    {
                        File.Delete(full_path);
                    }
                    catch (Exception Ex)
                    {
                        values.status = false;
                        values.message = Ex.Message;
                        response = new Dictionary<string, object>
                        {
                             { "status", false },
                             { "message", Ex.Message }
                        };
                    }
                }
            }
            return response;
        }


        public void DeleteProformaInvoice(MdlSmrTrnProformaInvoice values, string invoice_gid, string invoice_reference, string invoice_amount)
        {
            msSQL = " delete from rbl_trn_tproformainvoicedtl where invoice_gid='" + invoice_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                msSQL = " delete from rbl_trn_tproformainvoice where invoice_gid='" + invoice_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " delete from rbl_trn_tso2proformainvoice where invoice_gid='" + invoice_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Proforma Invoice Deleted Successfully";

                    }
                    else
                    {
                        values.status = false;
                        values.message = " Problem occurred while deleting in so2proformainvoice table";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Problem occurred while deleting in proforma invoice table";
                }
            }
            else
            {
                values.status = false;
                values.message = "Problem occurred while deleting in proforma invoicedtl table";
            }
        }

        public void DaGetDeleteInvoiceProductSummary(string invoice_gid, ProformaInvoiceEditlist values)
        {
            try
            {
                msSQL = " delete from rbl_tmp_tinvoicedtl where invoicedtl_gid='" + invoice_gid + "' ";
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
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Invoice Product Delete!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    }
}