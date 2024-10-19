using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Globalization;
using ems.sales.Models;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using static System.Drawing.ImageConverter;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using CrystalDecisions.Shared.Json;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using ems.utilities.Functions;
using System.Web;
using System.Data.Odbc;
using System.Data;
using System;
using System.Collections.Generic;


namespace ems.sales.DataAccess
{
    public class DaSmrTrnCreditNote
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        DataTable dt = new DataTable();

        HttpPostedFile httpPostedFile;
        Image branch_logo, DataColumn4;
        string company_logo_path, authorized_sign_path;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable mail_datatable, dt_datatable;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();

        string msSQL = string.Empty;
        string path, full_path = string.Empty;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        DataSet ds_dataset = new DataSet();
        finance_cmnfunction objfincmn = new finance_cmnfunction();
        string lspayment_amount, lsinvoice_amount, mssalesorderGID, creditnote_amount, lsinvoicegid, msstockreturn;
        string lstax2 = "0.00", lstax1 = "0.00", lstax3 = "0.00";
        int mnResult;


        public void DaGetCreditNoteSummary(string employee_gid, MdlSmrTrnCreditNote values)
        {
            try
            {
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " select distinct d.creditnote_gid, d.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag, a.mail_status, a.customer_gid, date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, " +
                       " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as overall_status,d.receipt_gid, " +
                       " a.payment_flag,  format(a.initialinvoice_amount,2) as initialinvoice_amount,date_format(a.credit_date,'%d-%m-%Y') as credit_date,  " +
                       " format(a.invoice_amount,2) as invoice_amount, a.customer_contactperson,format(d.credit_amount,2) as credit_amount, " +
                       " case when a.currency_code = '" + currency + "' then c.customer_name when a.currency_code is null then c.customer_name " +
                       " when a.currency_code is not null and a.currency_code <> '" + currency + "' then concat(c.customer_name,' / ',h.country) end as customer_name, " +
                       " case when a.customer_contactnumber is null then e.mobile when a.customer_contactnumber is not null then a.customer_contactnumber end as mobile " +
                       " from rbl_trn_tcreditnote d " +
                       " left join rbl_trn_tinvoice a on a.invoice_gid=d.invoice_gid " +
                       " left join rbl_trn_tinvoicedtl b on d.invoice_gid = b.invoice_gid " +
                       " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid " +
                       " left join crm_mst_tcustomercontact e on e.customer_gid=c.customer_gid " +
                       " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                       " where d.credit_date is not null order by d.credit_date desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<creditnotesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt_datatable.Rows)
                    {
                        getModuleList.Add(new creditnotesummary_list
                        {
                            credit_date = dr["credit_date"].ToString(),
                            creditnote_gid = dr["creditnote_gid"].ToString(),
                            invoice_date = dr["invoice_date"].ToString(),
                            invoiceref_no = dr["invoice_refno"].ToString(),
                            customer_name = dr["customer_name"].ToString(),
                            credit_amount = dr["credit_amount"].ToString(),
                            invoice_amount = dr["invoice_amount"].ToString(),
                            invoice_gid = dr["invoice_gid"].ToString(),
                            receipt_gid = dr["receipt_gid"].ToString(),
                        });
                        values.creditnotesummary_list = getModuleList;
                    }
                }
               
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting Credit Note summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetCreditNoteAddSelectSummary(MdlSmrTrnCreditNote values)
        {
            try
            {
                msSQL = " select a.invoice_gid,a.invoice_refno,b.customer_name,a.invoice_status,format(a.invoice_amount,2)as invoice_amount,b.customer_gid," +
                      " ifnull(format((a.payment_amount-a.credit_note)*exchange_rate,2),'0') as payment_amount,ifnull(format(((a.invoice_amount*exchange_rate)-(a.payment_amount*exchange_rate)),2),'0') as outstanding,format(a.credit_note,2) as credit_note," +
                      " case when a.customer_contactnumber is null then  concat(c.customercontact_name,'/',c.mobile,'/',c.email)" +
                      " when a.customer_contactnumber is not null then concat(a.customer_contactperson,'/',a.customer_contactnumber,'/',a.customer_email) end as contact" +
                      " from rbl_trn_tinvoice a" +
                      " left join crm_mst_tcustomer b on b.customer_gid=a.customer_gid" +
                      " left join crm_mst_tcustomercontact c on c.customer_gid=a.customer_gid" +
                      " where a.invoice_amount>(a.payment_amount+a.credit_note) and a.invoice_date<='" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                      " and a.invoice_status <>'Invoice Cancelled'" +
                      " order by a.invoice_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<creditnotesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt_datatable.Rows)
                    {
                        getModuleList.Add(new creditnotesummary_list
                        {
                            contact = dr["contact"].ToString(),
                            invoice_status = dr["invoice_status"].ToString(),
                            outstanding = dr["outstanding"].ToString(),
                            invoiceref_no = dr["invoice_refno"].ToString(),
                            customer_name = dr["customer_name"].ToString(),
                            payment_amount = dr["payment_amount"].ToString(),
                            invoice_amount = dr["invoice_amount"].ToString(),
                            credit_note = dr["credit_note"].ToString(),
                            invoice_gid = dr["invoice_gid"].ToString(),
                        });
                        values.creditnotesummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting Credit Note summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetAddSelectSummary(string invoice_gid, MdlSmrTrnCreditNote values)
        {
            try
            {
                msSQL = " select a.invoice_gid,a.invoice_refno,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date," +
                        " a.payment_term,date_format(a.payment_date,'%d-%m-%Y')as payment_date," +
                        " format(a.total_amount,2)as total_amount,a.currency_code,format(a.credit_note,2) as credit_note," +
                        " a.invoice_from,a.invoice_reference,format(sum(distinct e.product_total),2) as price," +
                        " a.Tax_amount,a.tax_percentage,a.tax_name, " +
                        " format(a.additionalcharges_amount,2)as additionalcharges_amount," +
                        " format(a.discount_amount,2)as discount_amount ," +
                        " format(a.invoice_amount,2)as invoice_amount," +
                        " a.customer_name,a.customer_contactperson,a.customer_address,a.customer_email," +
                        " a.customer_contactnumber  as mobile," +
                        " ifnull(format(((a.invoice_amount*exchange_rate)-(a.payment_amount*exchange_rate)),2),'0') as outstanding,ifnull(format((a.payment_amount-a.credit_note)*exchange_rate,2),'0') as payment_amount," +
                        " e.product_remarks,a.invoice_remarks from rbl_trn_tinvoice a " +
                        " left join rbl_trn_tso2invoice b on b.invoice_gid=a.invoice_gid " +
                        " left join rbl_trn_tinvoicedtl e on e.invoice_gid=a.invoice_gid" +
                        " where a.invoice_gid='" + invoice_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addselectCNsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addselectCNsummary_list
                        {
                            invoiceref_no = dr["invoice_refno"].ToString(),
                            invoice_date = dr["invoice_date"].ToString(),
                            payment_term = dr["payment_term"].ToString(),
                            payment_date = dr["payment_date"].ToString(),
                            total_amount = dr["total_amount"].ToString(),
                            currency_code = dr["currency_code"].ToString(),
                            credit_note = dr["credit_note"].ToString(),
                            invoice_from = dr["invoice_from"].ToString(),
                            invoice_reference = dr["invoice_reference"].ToString(),
                            price = dr["price"].ToString(),
                            Tax_amount = dr["Tax_amount"].ToString(),
                            tax_percentage = dr["tax_percentage"].ToString(),
                            tax_name = dr["tax_name"].ToString(),
                            additionalcharges_amount = dr["additionalcharges_amount"].ToString(),
                            discount_amount = dr["discount_amount"].ToString(),
                            invoice_amount = dr["invoice_amount"].ToString(),
                            customer_name = dr["customer_name"].ToString(),
                            customer_contactperson = dr["customer_contactperson"].ToString(),
                            invoice_remarks = dr["invoice_remarks"].ToString(),
                            product_remarks = dr["product_remarks"].ToString(),
                            outstanding = dr["outstanding"].ToString(),
                            mobile = dr["mobile"].ToString(),
                            customer_email = dr["customer_email"].ToString(),
                            customer_address = dr["customer_address"].ToString(),
                            payment_amount = dr["payment_amount"].ToString(),
                        });
                        values.addselectCNsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting Credit Note summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetAddSelectProdSummary(string invoice_gid, MdlSmrTrnCreditNote values)
        {
            try
            {
                msSQL = " select a.invoicedtl_gid,a.invoice_gid,a.qty_invoice," +
                        " format(a.product_price,2)as product_price,a.discount_percentage,format(a.discount_amount,2)as discount_amount, " +
                        " format(a.tax_amount,2)as tax_amount,format(a.tax_amount2,2)as tax_amount2 ,format(a.tax_amount3,2)as tax_amount3,a.tax_name,a.tax_name2,a.tax_name3," +
                        " format(((a.product_price*a.qty_invoice)-a.discount_amount+a.tax_amount+a. +a.tax_amount3),2)as price,a.display_field," +
                        " a.product_gid, a.product_code, a.product_name,b.productgroup_gid,g.productgroup_name, " +
                        " a.productuom_name,a.uom_gid" +
                        " from rbl_trn_tinvoicedtl a " +
                        " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                        " left join pmr_mst_tproductuom c on c.productuom_gid=a.uom_gid" +
                        " left join rbl_trn_tinvoice e on e.invoice_gid=a.invoice_gid" +
                        " left join pmr_mst_tproductgroup g on g.productgroup_gid=b.productgroup_gid" +
                        " where a.invoice_gid='" + invoice_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addselectProdsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addselectProdsummary_list
                        {
                            invoicedtl_gid = dr["invoicedtl_gid"].ToString(),
                            invoice_gid = dr["invoice_gid"].ToString(),
                            qty_invoice = dr["qty_invoice"].ToString(),
                            product_price = dr["product_price"].ToString(),
                            discount_percentage = dr["discount_percentage"].ToString(),
                            discount_amount = dr["discount_amount"].ToString(),
                            tax_amount = dr["tax_amount"].ToString(),
                            tax_amount2 = dr["tax_amount2"].ToString(),
                            tax_amount3 = dr["tax_amount3"].ToString(),
                            tax_name = dr["tax_name"].ToString(),
                            tax_name2 = dr["tax_name2"].ToString(),
                            tax_name3 = dr["tax_name3"].ToString(),
                            price = dr["price"].ToString(),
                            display_field = dr["display_field"].ToString(),
                            product_gid = dr["product_gid"].ToString(),
                            product_code = dr["product_code"].ToString(),
                            product_name = dr["product_name"].ToString(),
                            productgroup_gid = dr["productgroup_gid"].ToString(),
                            productgroup_name = dr["productgroup_name"].ToString(),
                            productuom_name = dr["productuom_name"].ToString(),
                            uom_gid = dr["uom_gid"].ToString(),
                        });
                        values.addselectProdsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Credit Note summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPostCreditNote(string employee_gid, postcreditnote_list values)
        {
            try
            {
                if (Convert.ToDouble(values.outstanding_amount) < Convert.ToDouble(values.creditnote_amount))
                {
                    values.message = "Credit Amount should be Less than or equal to Outstanding Amount";
                    return;
                }
                msSQL = "select invoice_amount,payment_amount from rbl_trn_tinvoice where invoice_gid='" + values.invoice_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                   
                    lsinvoice_amount = objOdbcDataReader["invoice_amount"].ToString();
                    lspayment_amount = objOdbcDataReader["payment_amount"].ToString();

                    objOdbcDataReader.Close();


                }

                string uiDate = values.creditnote_date;
                DateTime uiDatestr = DateTime.ParseExact(uiDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string Credit_Date = uiDatestr.ToString("yyyy-MM-dd");


                if (lsinvoice_amount == values.creditnote_amount)
                {
                    msSQL = " update rbl_trn_tinvoice set credit_note=credit_note+'" + values.creditnote_amount.Replace(",", "") + "', " +
                            " credit_date='" + Credit_Date + "',invoice_status='Credit Noted'   " +
                            " where invoice_gid='" + values.invoice_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = " update rbl_trn_tinvoice set credit_note=credit_note+'" + values.creditnote_amount.Replace(",", "") + "',payment_amount=payment_amount+'" + values.creditnote_amount.Replace(",", "") + "', " +
                            " credit_date='" + Credit_Date + "'   where invoice_gid='" + values.invoice_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                     string mscreditnotegid = objcmnfunctions.GetMasterGID("CRN");
                    msSQL = "insert into rbl_trn_tcreditnote ( " +
                            "  creditnote_gid, " +
                            " invoice_gid, " +
                            " credit_amount, " +
                            " credit_by, " +
                            " remarks, " +
                            " created_by, " +
                            " created_date, " +
                            " credit_date " +
                            " ) values ( " +
                            "'" + mscreditnotegid + "', " +
                            "'" + values.invoice_gid + "', " +
                            "'" + values.creditnote_amount.Replace(",", "") + "', " +
                            "'" + employee_gid + "'," +
                            "'" + (String.IsNullOrEmpty(values.inv_remarks) ? values.inv_remarks : values.inv_remarks.Replace("'","\\\'")) + "', " +
                            "'" + employee_gid + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                            "'" + Credit_Date + "') ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = "update rbl_trn_tinvoice set " +
                                   " creditnote_status='Y'" +
                                   " where invoice_gid ='" + values.invoice_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    msSQL = "select finance_flag from adm_mst_tcompany ";
                    string finance_flag = objdbconn.GetExecuteScalar(msSQL);
                    if (finance_flag == "Y")
                    {
                        string lsinvoiceamount = "",lscustomer_gid="", lsroundoff = "", lsexchangerate = "",lsinvoicerefno="", mysqlinvoiceDate="",sales_type = "", lsbranchgid="", lstax_name = "";
                        double roundoff = 0 , discount_amount =0 , addon_charge = 0, freight_charges = 0, grand_total_l = 0, packing_charges = 0, buyback_charges =0, insurance_charges =0, tax_amount=0;
                        msSQL= "select invoice_amount_L,payment_amount,invoice_refno,roundoff,exchange_rate,branch_gid,customer_gid,buyback_charges,packing_charges,insurance_charges,tax_name,tax_amount, " +
                               " discount_amount_L,additionalcharges_amount_L,frieghtcharges_amount_L,sales_type from rbl_trn_Tinvoice " +
                               " where invoice_gid='"+values.invoice_gid + "'";
                        objMySqlDataReader=objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                           
                            grand_total_l = Math.Round(double.Parse(values.creditnote_amount.Replace(",", "")),2);
                            lsexchangerate = objMySqlDataReader["exchange_rate"].ToString();
                            lsinvoicerefno = objMySqlDataReader["invoice_refno"].ToString();
                            roundoff = Math.Round(double.Parse(objMySqlDataReader["roundoff"].ToString()),2);
                            discount_amount = Math.Round(double.Parse(objMySqlDataReader["discount_amount_L"].ToString()), 2);
                            addon_charge = Math.Round(double.Parse(objMySqlDataReader["additionalcharges_amount_L"].ToString()), 2);
                            freight_charges = Math.Round(double.Parse(objMySqlDataReader["frieghtcharges_amount_L"].ToString()), 2);
                            sales_type = objMySqlDataReader["sales_type"].ToString();
                            lsbranchgid = objMySqlDataReader["branch_gid"].ToString();
                            lscustomer_gid = objMySqlDataReader["customer_gid"].ToString();
                            lstax_name = objMySqlDataReader["tax_name"].ToString();
                            packing_charges = Math.Round(double.Parse(objMySqlDataReader["packing_charges"].ToString()),2);
                            buyback_charges = Math.Round(double.Parse(objMySqlDataReader["buyback_charges"].ToString()),2);
                            insurance_charges = Math.Round(double.Parse(objMySqlDataReader["insurance_charges"].ToString()),2);
                            tax_amount = Math.Round(double.Parse(objMySqlDataReader["tax_amount"].ToString()), 2); 
                        }
                        objMySqlDataReader.Close();
                        double roundoff1 = roundoff * double.Parse(lsexchangerate);
                        string lsproduct_price_l = "", lstax1_gid = "", lstax2_gid = "";
                        msSQL = " select sum(product_price_L) as product_price_L,sum(tax_amount1_L) as tax1,sum(tax_amount2_L) as tax2,tax1_gid,tax2_gid from rbl_trn_tinvoicedtl " +
                             " where invoice_gid='" + values.invoice_gid + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            lsproduct_price_l = objOdbcDataReader["product_price_L"].ToString();
                            lstax1 = objOdbcDataReader["tax1"].ToString();
                            lstax2 = objOdbcDataReader["tax2"].ToString();
                            lstax1_gid = objOdbcDataReader["tax1_gid"].ToString();
                            lstax2_gid = objOdbcDataReader["tax2_gid"].ToString();
                        }
                        objOdbcDataReader.Close();
                       double lsbasic_amount = Math.Round(double.Parse(lsproduct_price_l), 2);

                        string inv_remarks = (String.IsNullOrEmpty(values.inv_remarks) ? values.inv_remarks : values.inv_remarks.Replace("'", "\\\'"));

                        objfincmn.jn_credit_debit_note(Credit_Date, inv_remarks, lsbranchgid, mscreditnotegid, mscreditnotegid
                                                     , lsbasic_amount, addon_charge, discount_amount, grand_total_l, lscustomer_gid, "Invoice", "RBL",
                                                     sales_type, roundoff1, freight_charges, buyback_charges, packing_charges, insurance_charges, tax_amount, lstax_name);

                        if (lstax1 != "0.00" && lstax1 != "" && lstax1 != null && lstax1 != null)
                        {
                            decimal lstaxsum = decimal.Parse(lstax1);
                            string lstaxamount = lstaxsum.ToString("F2");
                            tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                            objfincmn.jn_creditdebit_tax(mscreditnotegid, mscreditnotegid, inv_remarks, tax_amount, lstax1_gid);
                        }
                        if (lstax2 != "0.00" && lstax2 != "" && lstax2 != null && lstax2 != "0")
                        {
                            decimal lstaxsum = decimal.Parse(lstax2);
                            string lstaxamount = lstaxsum.ToString("F2");
                            tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                            objfincmn.jn_creditdebit_tax(mscreditnotegid, mscreditnotegid, inv_remarks, tax_amount, lstax2_gid);
                        }




                    }
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Credit Note Raised Successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while submitting Credit Note summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetViewAddSelectSummary(string invoice_gid, MdlSmrTrnCreditNote values)
        {
            try
            {
                msSQL = "select a.invoice_gid,a.invoice_refno,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,a.payment_term, b.price_total,c.directorder_gid, " +
               "date_format(a.payment_date,'%d-%m-%Y')as payment_date,format(a.total_amount,2)as total_amount,a.termsandconditions,a.Tax_amount,a.tax_name,a.tax_percentage, " +
               "format(a.additionalcharges_amount,2)as additionalcharges_amount,format(a.discount_amount,2)as discount_amount ,format(a.invoice_amount,2)as invoice_amount,format(a.credit_note,2) as credit_note,a.invoice_from,a.invoice_reference, " +
               "a.customer_name,a.customer_contactperson,a.customer_email,a.customer_address,b.product_remarks,a.invoice_remarks,date_format(a.credit_date,'%d-%m-%Y') as credit_date, " +
               "b.product_remarks,a.invoice_total,a.raised_amount,a.extraadditional_amount,a.extradiscount_amount,i.additional_name,h.discount_name,e.salesorder_gid,format((a.invoice_amount-(a.payment_amount)),2) as outstanding,format(a.payment_amount,2) as payment_amount, " +
               "a.extraadditional_code, a.extradiscount_code, format(a.extraadditional_amount,2) as extraadditional_amount, format(a.extradiscount_amount,2) as extradiscount_amount, " +
               "case when a.customer_contactnumber is null then g.mobile when a.customer_contactnumber is not null then a.customer_contactnumber end as mobile,a.currency_code,e.exchange_rate " +
               "from rbl_trn_tinvoice a " +
               "left join (select sum(product_total) as price_total,invoice_gid,product_remarks from rbl_trn_tinvoicedtl group by invoice_gid order by invoice_gid)b  on b.invoice_gid=a.invoice_gid " +
               "left join (select directorder_gid,invoice_gid from rbl_trn_tso2invoice)c on c.invoice_gid=a.invoice_gid " +
               "left join smr_trn_tdeliveryorder d on d.directorder_gid=c.directorder_gid " +
               "left join smr_trn_tsalesorder e on e.salesorder_gid=d.salesorder_gid " +
               "left join pmr_trn_tadditional i on i.additional_gid=a.extraadditional_code " +
               "left join crm_mst_tcustomer f on f.customer_gid=a.customer_gid " +
               "left join crm_mst_tcustomercontact g on g.customer_gid=a.customer_gid " +
               "left join pmr_trn_tdiscount h on h.discount_gid = a.extradiscount_code " +
               "where a.invoice_gid ='" + invoice_gid + "' " +
               "group by a.invoice_gid order by a.invoice_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addselectCNsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addselectCNsummary_list
                        {
                            invoiceref_no = dr["invoice_refno"].ToString(),
                            invoice_date = dr["invoice_date"].ToString(),
                            payment_term = dr["payment_term"].ToString(),
                            payment_date = dr["payment_date"].ToString(),
                            total_amount = dr["total_amount"].ToString(),
                            currency_code = dr["currency_code"].ToString(),
                            credit_note = dr["credit_note"].ToString(),
                            invoice_from = dr["invoice_from"].ToString(),
                            invoice_reference = dr["invoice_reference"].ToString(),
                            Tax_amount = dr["Tax_amount"].ToString(),
                            tax_percentage = dr["tax_percentage"].ToString(),
                            tax_name = dr["tax_name"].ToString(),
                            additionalcharges_amount = dr["additionalcharges_amount"].ToString(),
                            discount_amount = dr["discount_amount"].ToString(),
                            invoice_amount = dr["invoice_amount"].ToString(),
                            customer_name = dr["customer_name"].ToString(),
                            customer_contactperson = dr["customer_contactperson"].ToString(),
                            invoice_remarks = dr["invoice_remarks"].ToString(),
                            product_remarks = dr["product_remarks"].ToString(),
                            outstanding = dr["outstanding"].ToString(),
                            mobile = dr["mobile"].ToString(),
                            customer_email = dr["customer_email"].ToString(),
                            customer_address = dr["customer_address"].ToString(),
                            payment_amount = dr["payment_amount"].ToString(),
                            credit_date = dr["credit_date"].ToString(),
                            price_total = dr["price_total"].ToString(),

                        });
                        values.addselectCNsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting Credit Note summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetViewAddSelectProdSummary(string invoice_gid, MdlSmrTrnCreditNote values)
        {
            try
            {
                msSQL = " select a.invoicedtl_gid,a.invoice_gid,a.qty_invoice," +
                        " format(a.product_price,2)as product_price,a.discount_percentage,format(a.discount_amount,2)as discount_amount, " +
                        " format(a.tax_amount,2)as tax_amount,format(a.tax_amount2,2)as tax_amount2 ,format(a.tax_amount3,2)as tax_amount3,a.tax_name,a.tax_name2,a.tax_name3," +
                        " format(((a.product_price*a.qty_invoice)-a.discount_amount+a.tax_amount+a.tax_amount2+a.tax_amount3),2)as price,a.display_field," +
                        " a.product_gid, a.product_code, a.product_name,b.productgroup_gid,g.productgroup_name, " +
                        " a.productuom_name,a.uom_gid" +
                        " from rbl_trn_tinvoicedtl a " +
                        " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                        " left join pmr_mst_tproductuom c on c.productuom_gid=a.uom_gid" +
                        " left join rbl_trn_tinvoice e on e.invoice_gid=a.invoice_gid" +
                        " left join pmr_mst_tproductgroup g on g.productgroup_gid=b.productgroup_gid" +
                        " where a.invoice_gid='" + invoice_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addselectProdsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addselectProdsummary_list
                        {
                            invoicedtl_gid = dr["invoicedtl_gid"].ToString(),
                            invoice_gid = dr["invoice_gid"].ToString(),
                            qty_invoice = dr["qty_invoice"].ToString(),
                            product_price = dr["product_price"].ToString(),
                            discount_percentage = dr["discount_percentage"].ToString(),
                            discount_amount = dr["discount_amount"].ToString(),
                            tax_amount = dr["tax_amount"].ToString(),
                            tax_amount2 = dr["tax_amount2"].ToString(),
                            tax_amount3 = dr["tax_amount3"].ToString(),
                            tax_name = dr["tax_name"].ToString(),
                            tax_name2 = dr["tax_name2"].ToString(),
                            tax_name3 = dr["tax_name3"].ToString(),
                            price = dr["price"].ToString(),
                            display_field = dr["display_field"].ToString(),
                            product_gid = dr["product_gid"].ToString(),
                            product_code = dr["product_code"].ToString(),
                            product_name = dr["product_name"].ToString(),
                            productgroup_gid = dr["productgroup_gid"].ToString(),
                            productgroup_name = dr["productgroup_name"].ToString(),
                            productuom_name = dr["productuom_name"].ToString(),
                            uom_gid = dr["uom_gid"].ToString(),

                        });
                        values.addselectProdsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Credit Note summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetDeleteCreditNote(string creditnote_gid, string receipt_gid, MdlSmrTrnCreditNote values)
        {
            try
            {
                msSQL = "select credit_amount,invoice_gid from rbl_trn_tcreditnote where creditnote_gid='" + creditnote_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                   
                    creditnote_amount = objOdbcDataReader["credit_amount"].ToString();
                    lsinvoicegid = objOdbcDataReader["invoice_gid"].ToString();

                    objOdbcDataReader.Close();


                }
                msSQL = " update rbl_trn_tinvoice set credit_note=credit_note-'" + creditnote_amount + "',payment_amount=payment_amount-'" + creditnote_amount + "', " +
                        " credit_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'   where invoice_gid='" + lsinvoicegid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = "delete from rbl_trn_tcreditnote where creditnote_gid='" + creditnote_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = "delete from rbl_trn_tpayment where payment_gid='" + receipt_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    objfincmn.invoice_cancel(creditnote_gid);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Credit Note Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occured while deleting creditnote";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Credit Note !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetStockReturnSummary(string invoice_gid, MdlSmrTrnCreditNote values)
        {
            try
            {
                msSQL = "select j.creditnote_gid,j.credit_amount,a.invoice_gid,a.invoice_refno,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,a.payment_term, b.price_total,c.directorder_gid, " +
                          "date_format(a.payment_date,'%d-%m-%Y')as payment_date,format(a.total_amount,2)as total_amount,a.termsandconditions,a.Tax_amount,a.tax_name,a.tax_percentage, " +
                          "format(a.additionalcharges_amount,2)as additionalcharges_amount,format(a.discount_amount,2)as discount_amount ,format(a.invoice_amount,2)as invoice_amount,format(a.credit_note,2) as credit_note,a.invoice_from,a.invoice_reference, " +
                          "a.customer_name,a.customer_contactperson,a.customer_email,a.customer_address,b.product_remarks,a.invoice_remarks,date_format(a.credit_date,'%d-%m-%Y') as credit_date, " +
                          "b.product_remarks,a.invoice_total,a.raised_amount,a.extraadditional_amount,a.extradiscount_amount,i.additional_name,h.discount_name,e.salesorder_gid,format((a.invoice_amount-(a.payment_amount)),2) as outstanding,format(a.payment_amount,2) as payment_amount, " +
                          "a.extraadditional_code, a.extradiscount_code, format(a.extraadditional_amount,2) as extraadditional_amount, format(a.extradiscount_amount,2) as extradiscount_amount, " +
                          "case when a.customer_contactnumber is null then g.mobile when a.customer_contactnumber is not null then a.customer_contactnumber end as mobile,a.currency_code,e.exchange_rate " +
                          "from rbl_trn_tinvoice a " +
                          "left join (select sum(product_total) as price_total,invoice_gid,product_remarks from rbl_trn_tinvoicedtl group by invoice_gid order by invoice_gid)b  on b.invoice_gid=a.invoice_gid " +
                          "left join (select directorder_gid,invoice_gid from rbl_trn_tso2invoice)c on c.invoice_gid=a.invoice_gid " +
                          "left join smr_trn_tdeliveryorder d on d.directorder_gid=c.directorder_gid " +
                          "left join smr_trn_tsalesorder e on e.salesorder_gid=d.salesorder_gid " +
                          "left join pmr_trn_tadditional i on i.additional_gid=a.extraadditional_code " +
                          "left join crm_mst_tcustomer f on f.customer_gid=a.customer_gid " +
                          "left join crm_mst_tcustomercontact g on g.customer_gid=a.customer_gid " +
                          "left join pmr_trn_tdiscount h on h.discount_gid = a.extradiscount_code " +
                          " left join rbl_trn_tcreditnote j on j.invoice_gid=a.invoice_gid " +
                          "where a.invoice_gid ='" + invoice_gid + "' " +
                          "group by a.invoice_gid order by a.invoice_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addselectCNsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addselectCNsummary_list
                        {
                            invoiceref_no = dr["invoice_refno"].ToString(),
                            invoice_date = dr["invoice_date"].ToString(),
                            payment_term = dr["payment_term"].ToString(),
                            payment_date = dr["payment_date"].ToString(),
                            total_amount = dr["total_amount"].ToString(),
                            currency_code = dr["currency_code"].ToString(),
                            credit_note = dr["credit_note"].ToString(),
                            invoice_from = dr["invoice_from"].ToString(),
                            invoice_reference = dr["invoice_reference"].ToString(),
                            Tax_amount = dr["Tax_amount"].ToString(),
                            tax_percentage = dr["tax_percentage"].ToString(),
                            tax_name = dr["tax_name"].ToString(),
                            additionalcharges_amount = dr["additionalcharges_amount"].ToString(),
                            discount_amount = dr["discount_amount"].ToString(),
                            invoice_amount = dr["invoice_amount"].ToString(),
                            customer_name = dr["customer_name"].ToString(),
                            customer_contactperson = dr["customer_contactperson"].ToString(),
                            invoice_remarks = dr["invoice_remarks"].ToString(),
                            product_remarks = dr["product_remarks"].ToString(),
                            outstanding = dr["outstanding"].ToString(),
                            mobile = dr["mobile"].ToString(),
                            customer_email = dr["customer_email"].ToString(),
                            customer_address = dr["customer_address"].ToString(),
                            payment_amount = dr["payment_amount"].ToString(),
                            credit_date = dr["credit_date"].ToString(),
                            price_total = dr["price_total"].ToString(),
                            credit_amount = dr["credit_amount"].ToString(),
                            creditnote_gid = dr["creditnote_gid"].ToString()
                        });
                        values.addselectCNsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting Stock Return Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetStockReturnProdSummary(string invoice_gid, MdlSmrTrnCreditNote values)
        {
            try
            {
                msSQL = " select a.invoicedtl_gid,a.invoice_gid,a.qty_invoice," +
                        " format(a.product_price,2)as product_price,a.discount_percentage,format(a.discount_amount,2)as discount_amount, " +
                        " format(a.tax_amount,2)as tax_amount,format(a.tax_amount2,2)as tax_amount2 ,format(a.tax_amount3,2)as tax_amount3,a.tax_name,a.tax_name2,a.tax_name3," +
                        " format(((a.product_price*a.qty_invoice)-a.discount_amount+a.tax_amount+a.tax_amount2+a.tax_amount3),2)as price,a.display_field," +
                        " a.product_gid, a.product_code, a.product_name,b.productgroup_gid,g.productgroup_name, " +
                        " a.productuom_name,a.uom_gid" +
                        " from rbl_trn_tinvoicedtl a " +
                        " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                        " left join pmr_mst_tproductuom c on c.productuom_gid=a.uom_gid" +
                        " left join rbl_trn_tinvoice e on e.invoice_gid=a.invoice_gid" +
                        " left join pmr_mst_tproductgroup g on g.productgroup_gid=b.productgroup_gid" +
                        " where a.invoice_gid='" + invoice_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addselectProdsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addselectProdsummary_list
                        {
                            invoicedtl_gid = dr["invoicedtl_gid"].ToString(),
                            invoice_gid = dr["invoice_gid"].ToString(),
                            qty_invoice = dr["qty_invoice"].ToString(),
                            product_price = dr["product_price"].ToString(),
                            discount_percentage = dr["discount_percentage"].ToString(),
                            discount_amount = dr["discount_amount"].ToString(),
                            tax_amount = dr["tax_amount"].ToString(),
                            tax_amount2 = dr["tax_amount2"].ToString(),
                            tax_amount3 = dr["tax_amount3"].ToString(),
                            tax_name = dr["tax_name"].ToString(),
                            tax_name2 = dr["tax_name2"].ToString(),
                            tax_name3 = dr["tax_name3"].ToString(),
                            price = dr["price"].ToString(),
                            display_field = dr["display_field"].ToString(),
                            product_gid = dr["product_gid"].ToString(),
                            product_code = dr["product_code"].ToString(),
                            product_name = dr["product_name"].ToString(),
                            productgroup_gid = dr["productgroup_gid"].ToString(),
                            productgroup_name = dr["productgroup_name"].ToString(),
                            productuom_name = dr["productuom_name"].ToString(),
                            uom_gid = dr["uom_gid"].ToString(),

                        });
                        values.addselectProdsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Credit Note summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPostStockReturn(string employee_gid, postcreditnote_list values)
        {
            try
            {
                msstockreturn = objcmnfunctions.GetMasterGID("SRCR");
                if (msstockreturn == "E")
                {
                    values.message = "Cannot able to generate unique id";
                    return;
                }
                msSQL = " insert into rbl_trn_tcreditnotedtl ( " +
                    " creditnotedtl_gid, " +
                    " creditnote_gid, " +
                    " stock_return, " +
                    " product_price, " +
                    " invoicedtl_gid, " +
                    " created_by, " +
                    " created_date " +
                    " ) values ( " +
                     " '" + msstockreturn + "', " +
                     " '" + values.creditnote_gid + "', " +
                     " '" + values.stock_return.Replace(",", "") + "', " +
                     " '" + values.product_price.Replace(",", "") + "', " +
                     " '" + values.invoicedtl_gid + "', " +
                     "'" + employee_gid + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Stock Returned Successfully";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while submitting Credit Note summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public Dictionary<string, object> DaCreditPDF(string creditnote_gid, string invoice_gid, MdlSmrTrnCreditNote values)
        {
            var ls_response = new Dictionary<string, object>();
            try
            {
                OdbcConnection myConnection = new OdbcConnection();
                myConnection.ConnectionString = objdbconn.GetConnectionString();
                OdbcCommand MyCommand = new OdbcCommand();
                MyCommand.Connection = myConnection;
                DataSet myDS = new DataSet();
                OdbcDataAdapter MyDA = new OdbcDataAdapter();

                msSQL = " select a.invoice_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,a.invoice_amount,a.invoice_refno,a.payment_date,a.currency_code,creditnote_irn as creditnoteirn," +
                      "  a.freightcharges_amount, a.additionalcharges_amount, a.Tax_amount,a.tax_name,sum( distinct s.product_total) as price_total,d.credit_amount as DataColumn1," +
                           " a.discount_amount, a.total_amount as total_amount, a.advance_amount, concat('',' ',a.customer_name) as cst_number," +
                           " b.customer_code, b.tin_number,date_format(d.credit_date,'%d-%m-%Y') as creditnote_date,d.creditnote_gid,  " +
                           " a.customer_address,a.customer_contactperson as customercontact_name, a.customer_email as email," +
                           " case when a.customer_contactnumber is null then c.mobile when a.customer_contactnumber is not null then a.customer_contactnumber end as mobile ," +
                           " a.invoice_reference as directorder_gid, a.termsandconditions,a.currency_code as designation,f.so_referenceno1,a.currency_code " +
                           " from rbl_trn_tinvoice a " +
                           " left join rbl_trn_tinvoicedtl s on a.invoice_gid=s.invoice_gid " +
                           " left join crm_mst_tcustomer b on b.customer_gid=a.customer_gid " +
                           " left join crm_mst_tcustomercontact c on c.customer_gid=a.customer_gid " +
                           " left join rbl_trn_tcreditnote d on d.invoice_gid=a.invoice_gid " +
                           " left join smr_trn_tsalesorder f on f.salesorder_gid=a.invoice_reference " +
                           " where a.invoice_gid='" + invoice_gid + "' group by a.invoice_gid order by a.invoice_gid asc ";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable1");

                msSQL = " select distinct a.invoice_gid,a.product_code, case when display_field is null  or   display_field='' then a.product_name else concat(a.product_name,'-',a.display_field) end as product_name,  a.qty_invoice as DataColumn1,format((replace(a.vendor_price,',','')-a.discount_amount),2) as price, a.productuom_name, f.productgroup_gid," +
                    " a.product_total as total_amount,b.invoice_amount,a.display_field,f.productgroup_name,case when c.stock_return is null then 0.00 else c.stock_return end as stock_return,d.credit_amount as credit_note,(a.product_price * c.stock_return) as total," +
                    "   CASE " +
                    " WHEN a.tax_amount2 = 0 THEN CONCAT(a.tax_name COLLATE latin1_general_ci, ':',  ' ', a.tax_amount) " +
                    " WHEN a.tax_amount = 0 THEN CONCAT(a.tax_name2 COLLATE latin1_general_ci, ':',  ' ', a.tax_amount2) " +
                    " ELSE CONCAT(a.tax_name COLLATE latin1_general_ci, ':',  ' ', a.tax_amount, ' ', a.tax_name2 COLLATE latin1_general_ci, ':', ' ', a.tax_amount2)" +
                    " END AS all_taxes " +
                    " from rbl_trn_tinvoicedtl a " +
                    " left join rbl_trn_tinvoice b on a.invoice_gid=b.invoice_gid" +
                    " left join rbl_trn_tcreditnotedtl c on a.invoicedtl_gid=c.invoicedtl_gid" +
                    " left join rbl_trn_tcreditnote d on c.creditnote_gid=d.creditnote_gid" +
                    " left join pmr_mst_tproduct e on e.product_gid=a.product_gid " +
                    " left join pmr_mst_tproductgroup f on e.productgroup_gid=f.productgroup_gid " +
                    " left join pmr_mst_tproductuom g on g.productuom_gid=a.uom_gid " +
                    " where a.invoice_gid= '" + invoice_gid + "' group by a.invoicedtl_gid order by a.invoicedtl_gid asc ";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable2");

                msSQL = " select a.address1 as address, a.branch_name,a.city,a.state,a.postal_code from " +
                                " hrm_mst_tbranch a  " +
                                " left join hrm_mst_temployee b on a.branch_gid=b.branch_gid " +
                                " left join rbl_trn_tinvoice c on b.employee_gid=c.user_gid " +
                                " where c.invoice_gid='" + invoice_gid + "'";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");

                msSQL = " select a.branch_logo,b.company_logo_path as authoriser_sign from " +
                               " hrm_mst_tbranch a  " +
                              " left join adm_mst_tcompany b on 1=1 " +
                               " left join rbl_trn_tinvoice c on c.branch_gid=a.branch_gid " +
                                 " where c.invoice_gid='" + invoice_gid + "'";

                 dt1 = objdbconn.GetDataTable(msSQL);
                DataTable4.Columns.Add("authoriser_sign", typeof(byte[]));
                if (dt1.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt1.Rows)
                    {
                        company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["authoriser_sign"].ToString().Replace("../../", ""));

                        if (System.IO.File.Exists(company_logo_path))
                        {
                            //Convert  Image Path to Byte
                            branch_logo = System.Drawing.Image.FromFile(company_logo_path);
                            byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));

                            DataRow newRow = DataTable4.NewRow();
                            newRow["authoriser_sign"] = branch_logo_bytes;
                            DataTable4.Rows.Add(newRow);
                        }
                    }
                }
                dt.Dispose();
                DataTable4.TableName = "DataTable4";
                myDS.Tables.Add(DataTable4);

                try {
                    ReportDocument oRpt = new ReportDocument();
                    string base_pathOF_currentFILE = AppDomain.CurrentDomain.BaseDirectory;
                    string report_path = Path.Combine(base_pathOF_currentFILE, "ems.sales", "smr_crp_creditnote.rpt");

                    if (!File.Exists(report_path))
                    {
                        values.status = false;
                        values.message = "Your Rpt path not found !!";
                        ls_response = new Dictionary<string, object>
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
                    string PDFfile_name = "CreditNote.pdf";
                    full_path = Path.Combine(path, PDFfile_name);

                    oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, full_path);
                    myConnection.Close();
                    ls_response = objFnazurestorage.reportStreamDownload(full_path);
                    values.status = true;
                }
                catch (Exception ex)
                {
                    values.status = false;
                    values.message = ex.Message;
                    ls_response = new Dictionary<string, object>
                    {
                       { "status", false },
                       { "message", ex.Message }
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
                            values.message = Ex.Message;
                            ls_response = new Dictionary<string, object>
                            {
                                { "status", false },
                                { "message", Ex.Message }
                            };
                        }
                    }
                }
                return ls_response;
            }
            catch (Exception ex)
            {
                values.message = ex.Message;
                ls_response = new Dictionary<string, object>
                {
                   { "status", false },
                   { "message", ex.Message }
                };
                return ls_response;
            }

        }
    }
}