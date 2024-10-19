using ems.finance.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Web;
using System.Linq;
using OfficeOpenXml.Style;

namespace ems.finance.DataAccess
{
    public class DaTaxManagements
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        string msGetGid, msGetGidfil, lstaxable_amount, lstotaltax, lblprecarry, lstax_payable, lbltotaltax,lstaxpayable, lscarryforward, lstotaloutputcredit_tax, lstotalinputdebit_tax, lsltax_payable, lslesscarry_forward, lsmonth,lsyear,invoice_date, lblcarryforward, ls_lblcarryforward;
        int mnResult;
        decimal lstotal, dtotinput_tax, dtotoutput_tax, dtotcredit_tax, dtotdebit_tax;
        double dtotax_payable, dtointerestcharge_payable, dtopenality_payable, dlblprecarry, lbldtotaltax, lstotaloutputtax;           
        public void DaGetTaxManagementSummary(string user_gid, MdlTaxManagements values)
        {
            msSQL = " SELECT CONCAT(MONTHNAME(STR_TO_DATE(month, '%m')) COLLATE utf8mb4_general_ci, ' ', " +
                    " CONVERT(year, CHAR(10)) COLLATE utf8mb4_general_ci) AS month," +
                    " taxfiling_gid AS fillingref_no, FORMAT(tax_input, 2) AS tax_input, FORMAT(tax_output, 2) AS tax_output, " +
                    " FORMAT(CASE WHEN overalltax_amount = 0.00 THEN tax_carryforward " +
                    " ELSE overalltax_amount END, 2) AS overalltax_amount, DATE_FORMAT(created_date, '%d-%m-%Y') AS taxfilling_date " +
                    " FROM acc_trn_ttaxfiling " +
                    " ORDER BY created_date DESC, taxfiling_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var TaxManagementSummary_List = new List<GetTaxManagementSummary_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    TaxManagementSummary_List.Add(new GetTaxManagementSummary_List
                    {
                        fillingref_no = dt["fillingref_no"].ToString(),
                        tax_input = dt["tax_input"].ToString(),
                        tax_output = dt["tax_output"].ToString(),
                        overalltax_amount = dt["overalltax_amount"].ToString(),
                        taxfilling_date = dt["taxfilling_date"].ToString(),
                        month = dt["month"].ToString()
                    });
                    values.GetTaxManagementSummary_List = TaxManagementSummary_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetOutputTaxView(string user_gid, MdlTaxManagements values, string taxfiling_gid)
        {
            msSQL = " select a.invoice_gid,date_format(a.invoice_date, '%d-%m-%Y') as invoice_date," +
                    " a.invoice_refno,upper(h.customer_name) as customer_name, " +
                    " format(h.invoice_amount, 2) as invoice_amount,format(h.taxable_amount, 2) as taxable_amount," +
                    " a.irn, a.customer_name as 'Customer',b.gst_number, " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'SGST0' " +
                    " from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '0'" +
                    " and x.tax_name like 'SGST%'),2) as 'SGST 0%', " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'SGST 2.5'" +
                    " from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '2.5'" +
                    " and x.tax_name like 'SGST%'),2) as 'SGST 2.5%', " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'SGST 6'" +
                    " from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '6'" +
                    " and x.tax_name like 'SGST%'),2) as 'SGST 6%'," +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'SGST 9'" +
                    " from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '9' " +
                    " and x.tax_name like 'SGST%'),2) as 'SGST 9%'," +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'SGST 14'" +
                    " from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '14'" +
                    " and x.tax_name like 'SGST%'),2) as 'SGST 14%', " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'CGST 0'" +
                    " from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '0'" +
                    " and x.tax_name like 'CGST%'),2) as 'CGST 0%', " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'CGST 2.5'" +
                    " from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '2.5'" +
                    " and x.tax_name like 'CGST%'),2) as 'CGST 2.5%', " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'CGST 6'" +
                    " from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '6'" +
                    " and x.tax_name like 'CGST%'),2) as 'CGST 6%', " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'CGST 9'" +
                    " from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '9'" +
                    " and x.tax_name like 'CGST%'),2) as 'CGST 9%', " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'CGST 14'" +
                    " from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '14'" +
                    " and x.tax_name like 'CGST%'),2) as 'CGST 14%', " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'IGST 0' from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '0' and x.tax_name like 'IGST%'),2) as 'IGST 0%'," +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'IGST 5' from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '5' and x.tax_name like 'IGST%'),2) as 'IGST 5%'," +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'IGST 12' from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '12' and x.tax_name like 'IGST%'),2) as 'IGST 12%', " +
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'IGST 18' from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '18' and x.tax_name like 'IGST%'),2) as 'IGST 18%'," +      
                    " format((select ifnull(sum(x.tax_amount), 0.00) as 'IGST 28' from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage = '28' and x.tax_name like 'IGST%'),2) as 'IGST 28%'," +    
                    " format(a.invoice_amount, 2) as 'invoiceamount'," +
                    " format((select ifnull(sum(x.tax_amount), 0.00) from rbl_trn_vinvoicetax x" +
                    " where x.invoice_gid = a.invoice_gid ),2) as 'taxableamount', " +                                            
                    " format((select sum(x.qty_invoice * (x.product_price)) + (ifnull((y.freight_charges + y.packing_charges" +
                    " + y.insurance_charges + y.additionalcharges_amount + roundoff), 0.00)) - ifnull(y.buyback_charges, 0.00) - ifnull(y.discount_amount, 0.00)" +
                    " from rbl_trn_tinvoicedtl x" +
                    " inner join rbl_trn_tinvoice y on x.invoice_gid = y.invoice_gid" +
                    " where a.invoice_gid = x.invoice_gid" +
                    " group by y.invoice_gid),2) as 'nontaxable_amount'" +
                    " from rbl_trn_tinvoice a " +
                    " inner join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                    " left join acc_trn_ttaxfilingdtl h on a.invoice_gid = h.invoice_gid where " +
                    " h.taxfiling_gid = '" + taxfiling_gid + "' group by a.invoice_gid order by a.invoice_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var OutputTaxView_List = new List<GetOutputTaxView_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    OutputTaxView_List.Add(new GetOutputTaxView_List
                    {
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        taxable_amount = dt["taxable_amount"].ToString(),
                        irn = dt["irn"].ToString(),
                        customer = dt["customer"].ToString(),
                        gst_number = dt["gst_number"].ToString(),
                        SGST_0 = dt["SGST 0%"].ToString(),
                        SGST_25 = dt["SGST 2.5%"].ToString(),
                        SGST_6 = dt["SGST 6%"].ToString(),
                        SGST_9 = dt["SGST 9%"].ToString(),
                        SGST_14 = dt["SGST 14%"].ToString(),
                        CGST_0 = dt["CGST 0%"].ToString(),
                        CGST_25 = dt["CGST 2.5%"].ToString(),
                        CGST_6 = dt["CGST 6%"].ToString(),
                        CGST_9 = dt["CGST 9%"].ToString(),
                        CGST_14 = dt["CGST 14%"].ToString(),
                        IGST_0 = dt["IGST 0%"].ToString(),
                        IGST_5 = dt["IGST 5%"].ToString(),
                        IGST_12 = dt["IGST 12%"].ToString(),
                        IGST_18 = dt["IGST 18%"].ToString(),
                        IGST_28 = dt["IGST 28%"].ToString(),
                        invoiceamount = dt["invoiceamount"].ToString(),
                        taxableamount = dt["taxableamount"].ToString(),
                        nontaxable_amount = dt["nontaxable_amount"].ToString(),
                    });
                    values.GetOutputTaxView_List = OutputTaxView_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetInputTaxView(string user_gid, MdlTaxManagements values, string taxfiling_gid)
        {
              msSQL = " select a.invoice_gid,date_format(a.invoice_date, '%d-%m-%Y') as invoice_date, " +
                      " a.invoice_refno, upper(h.vendor_name) as vendor_name, " +
                      " format(h.invoice_amount, 2) as invoice_amount,format(h.taxable_amount, 2) as taxable_amount, " +
                      " b.vendor_companyname as 'vendor',b.gst_number, " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST0' " +
                      " from pbl_trn_vinvoicetax x " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' " +
                      " and x.tax_name like 'SGST%'),2) as 'SGST 0%',  " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 2.5'  " +
                      " from pbl_trn_vinvoicetax x  " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                      " and x.tax_name like 'SGST%'),2) as 'SGST 2.5%', " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 6' " +
                      " from pbl_trn_vinvoicetax x  " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                      " and x.tax_name like 'SGST%'),2) as 'SGST 6%',  " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 9' " +
                      " from pbl_trn_vinvoicetax x  " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9' " +
                      " and x.tax_name like 'SGST%'),2) as 'SGST 9%',  " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 14' " +
                      " from pbl_trn_vinvoicetax x  " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                      " and x.tax_name like 'SGST%'),2) as 'SGST 14%'," +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 0' " +
                      " from pbl_trn_vinvoicetax x  " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0'  " +
                      " and x.tax_name like 'CGST%'),2) as 'CGST 0%', " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 2.5' " +
                      " from pbl_trn_vinvoicetax x  " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                      " and x.tax_name like 'CGST%'),2) as 'CGST 2.5%', " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 6' " +
                      " from pbl_trn_vinvoicetax x  " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                      " and x.tax_name like 'CGST%'),2) as 'CGST 6%',  " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 9' " +
                      " from pbl_trn_vinvoicetax x  " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9'  " +
                      " and x.tax_name like 'CGST%'),2) as 'CGST 9%',  " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 14' " +
                      " from pbl_trn_vinvoicetax x" +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                      " and x.tax_name like 'CGST%'),2) as 'CGST 14%'," +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 0' from pbl_trn_vinvoicetax x" +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' and x.tax_name like 'IGST%'),2) as 'IGST 0%', " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 5' from pbl_trn_vinvoicetax x" +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='5' and x.tax_name like 'IGST%'),2) as 'IGST 5%', " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 12' from pbl_trn_vinvoicetax x  " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='12' and x.tax_name like 'IGST%'),2) as 'IGST 12%', " +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 18' from pbl_trn_vinvoicetax x " +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='18' and x.tax_name like 'IGST%'),2) as 'IGST 18%'," +
                      " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 28' from pbl_trn_vinvoicetax x" +
                      " where x.invoice_gid=a.invoice_gid and x.tax_percentage='28' and x.tax_name like 'IGST%'),2) as 'IGST 28%'," +
                      " format(a.invoice_amount,2) as 'invoiceamount', " +
                      " format((select ifnull(sum(x.tax_amount),0.00) from pbl_trn_vinvoicetax x " +
                      "  where x.invoice_gid=a.invoice_gid ),2) as 'taxableamount', " +
                      " format((select ifnull(sum((qty_invoice * product_price)-x.discount_amount),0.00) +  " +
                      " ifnull((additionalcharges_amount+freightcharges+round_off),0.00)- ifnull((y.discount_amount+buybackorscrap),0.00) " +
                      " from acp_trn_tinvoicedtl x " +
                      " inner join acp_trn_tinvoice y on x.invoice_gid=y.invoice_gid " +
                      " where y.invoice_gid=a.invoice_gid group by y.invoice_gid),2) as 'nontaxable_amount' " +
                      " from acp_trn_tinvoice a " +
                      " left join acp_mst_tvendor b on a.vendor_gid=b.vendor_gid " +
                      " left join acc_trn_ttaxfilingdtl h on h.invoice_gid = a.invoice_gid " +
                      " where h.taxfiling_gid = '" + taxfiling_gid + "' order by invoice_date, invoice_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var InputTaxView_List = new List<GetInputTaxView_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    InputTaxView_List.Add(new GetInputTaxView_List
                    {
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        vendor_name = dt["vendor_name"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        taxable_amount = dt["taxable_amount"].ToString(),
                        vendor = dt["vendor"].ToString(),
                        gst_number = dt["gst_number"].ToString(),
                        SGST_0 = dt["SGST 0%"].ToString(),
                        SGST_25 = dt["SGST 2.5%"].ToString(),
                        SGST_6 = dt["SGST 6%"].ToString(),
                        SGST_9 = dt["SGST 9%"].ToString(),
                        SGST_14 = dt["SGST 14%"].ToString(),
                        CGST_0 = dt["CGST 0%"].ToString(),
                        CGST_25 = dt["CGST 2.5%"].ToString(),
                        CGST_6 = dt["CGST 6%"].ToString(),
                        CGST_9 = dt["CGST 9%"].ToString(),
                        CGST_14 = dt["CGST 14%"].ToString(),
                        IGST_0 = dt["IGST 0%"].ToString(),
                        IGST_5 = dt["IGST 5%"].ToString(),
                        IGST_12 = dt["IGST 12%"].ToString(),
                        IGST_18 = dt["IGST 18%"].ToString(),
                        IGST_28 = dt["IGST 28%"].ToString(),
                        invoiceamount = dt["invoiceamount"].ToString(),
                        taxableamount = dt["taxableamount"].ToString(),
                        nontaxable_amount = dt["nontaxable_amount"].ToString(),
                    });
                    values.GetInputTaxView_List = InputTaxView_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCreditNoteTaxView(string user_gid, MdlTaxManagements values, string taxfiling_gid)
        {
            msSQL = " select a.invoice_gid,date_format(a.credit_date, '%d-%m-%Y') as credit_date, " +
                    " a.creditnote_gid, upper(b.customer_name) as customer_name, " +
                    " format(b.invoice_amount, 2) as invoice_amount, format(b.taxable_amount, 2) as taxable_amount from rbl_trn_tcreditnote a " +
                    " left join acc_trn_ttaxfilingdtl b on a.invoice_gid = b.invoice_gid " +
                    " where b.taxfiling_gid = '" + taxfiling_gid + "' group by a.invoice_gid order by a.invoice_gid desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var CreditNoteTaxView_List = new List<GetCreditNoteTaxView_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CreditNoteTaxView_List.Add(new GetCreditNoteTaxView_List
                    {                        
                        invoice_gid = dt["invoice_gid"].ToString(),
                        credit_date = dt["credit_date"].ToString(),
                        creditnote_gid = dt["creditnote_gid"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        taxable_amount = dt["taxable_amount"].ToString()
                    });
                    values.GetCreditNoteTaxView_List = CreditNoteTaxView_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetTotalTaxView(string user_gid, MdlTaxManagements values, string taxfiling_gid)
        {
            msSQL = " select Format(tax_input,2) as tax_input,Format(tax_output,2) as tax_output," +
                    " Format(interest_payable,2) as interest_payable,Format(penalty_payable,2) as penalty_payable," +
                    " Format(tax_carryforward,2) as tax_carryforward,Format(pre_carry_forward,2) as pre_carry_forward," +
                    " Format(overalltax_amount,2) as overalltax_amount from acc_trn_ttaxfiling " +
                    " where taxfiling_gid='" + taxfiling_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var TotalTaxView_List = new List<GetTotalTaxView_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    TotalTaxView_List.Add(new GetTotalTaxView_List
                    {
                        lessinput_tax  = dt["tax_input"].ToString(),
                        totaloutputtaxwith_credit= dt["tax_output"].ToString(),
                        interestcharges_payable= dt["interest_payable"].ToString(),
                        penalty_payable = dt["penalty_payable"].ToString(),
                        previouscarry_forward = dt["pre_carry_forward"].ToString(),
                        totalcarry_forward = dt["tax_carryforward"].ToString(),
                        totaltax_payable  = dt["overalltax_amount"].ToString(),
                    });
                    values.GetTotalTaxView_List = TotalTaxView_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetTotalCreditTaxView(string user_gid, MdlTaxManagements values, string taxfiling_gid)
        {
            msSQL = " select Format(sum(taxable_amount),2) as taxable_amount," +
                    " Format(tax_output,2) as tax_output," +
                    " Format(tax_input,2) as tax_input " +
                    " from acc_trn_ttaxfiling a " +
                    " left join acc_trn_ttaxfilingdtl b on b.taxfiling_gid = a.taxfiling_gid " +
                    " where b.taxfiling_gid='" + taxfiling_gid + "'" +
                    " and invoice_type='credit' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var TotalCreditTaxView_List = new List<GetTotalCreditTaxView_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                string tax_output = dt_datatable.Rows[0]["tax_output"].ToString();
                string tax_input = dt_datatable.Rows[0]["tax_input"].ToString();
                string taxable_amount = dt_datatable.Rows[0]["taxable_amount"].ToString();

                lstotaloutputtax = Convert.ToDouble(tax_output) + Convert.ToDouble(taxable_amount);

                double outputTax = Convert.ToDouble(tax_output);
                double inputTax = Convert.ToDouble(tax_input);
                double total = outputTax - inputTax;

                if (total > 0.00)
                {
                    lstaxpayable = FormatNumber(outputTax - inputTax);
                }
                else
                {
                    lscarryforward = FormatNumber(outputTax - inputTax);
                }
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    if (dt["taxable_amount"].ToString() == "")
                    {
                        lstaxable_amount = "0.00";
                    }
                    else
                    {
                        lstaxable_amount = dt["taxable_amount"].ToString();
                    }
                    TotalCreditTaxView_List.Add(new GetTotalCreditTaxView_List
                    {
                        totalcredit_tax = lstaxable_amount,
                        tax_payable = lstaxpayable,
                        carry_forward = lscarryforward,
                        totaloutput_tax = lstotaloutputtax
                    });
                    values.GetTotalCreditTaxView_List = TotalCreditTaxView_List;
                }
            }
            dt_datatable.Dispose();
        }
        private string FormatNumber(object value)
        {
            if (value != DBNull.Value)
            {
                decimal balance;
                if (decimal.TryParse(value.ToString(), out balance))
                {
                    return balance.ToString("N2");
                }
            }
            return string.Empty;
        }
        public void DaInputTaxSubmit(string user_gid, InputTaxSummary values)
        {
            try
            {              

                msSQL = " delete from acc_tmp_ttaxinput";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                for (int i = 0; i < values.InputTaxSummary_List.ToArray().Length; i++)
                {
                    // Parse the original date string using custom date format
                    DateTime invoices_date = DateTime.ParseExact(values.InputTaxSummary_List[i].invoice_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                   
                    // Convert the date to the desired format
                    string invoicedate = invoices_date.ToString("yyyy-MM-dd"); 

                    msGetGid = objcmnfunctions.GetMasterGID("IVIT");

                    msSQL = " Insert into acc_tmp_ttaxinput ( " +
                             " tmptaxinput_gid, " +
                             " invoice_gid, " +
                             " invoice_date, " +
                             " vendor_name, " +
                             " gst_no, " +
                             " invoice_amount, " +
                             " taxable_amount, " +
                             " created_by, " +
                             " created_date " +
                             " ) Values ( " +
                             " '" + msGetGid + "'," +
                             " '" + values.InputTaxSummary_List[i].invoice_gid + "', " +
                             " '" + invoicedate + "', " +
                             " '" + values.InputTaxSummary_List[i].vendor_name + "', " +
                             " '" + values.InputTaxSummary_List[i].gst_number + "', " +
                             " '" + Convert.ToDouble(values.InputTaxSummary_List[i].invoice_amount) + "', " +
                             " '" + Convert.ToDouble(values.InputTaxSummary_List[i].Taxable_Amount) + "', " +
                             " '" + user_gid + "', " +
                             " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occurred while updating Input Tax";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + values.message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "Finance/ErrorLog/Tax Management/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Records Inserted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while updating Input Tax";
                }
            }
            
            catch (Exception ex)
            {
                values.message = "Error Occurred while updating Input Tax";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetCompileInputTax(string user_gid, MdlTaxManagements values)
        {
            msSQL = " select tmptaxinput_gid,invoice_gid," +
                    " date_format(invoice_date, '%d-%m-%Y') as invoice_date," +
                    " format(invoice_amount, 2) as invoice_amount, format(taxable_amount, 2) as taxable_amount," +
                    " vendor_name,gst_no from acc_tmp_ttaxinput ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var CompileInputTax_List = new List<GetCompileInputTax_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CompileInputTax_List.Add(new GetCompileInputTax_List
                    {
                        tmptaxinput_gid = dt["tmptaxinput_gid"].ToString(),
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        taxable_amount = dt["taxable_amount"].ToString(),
                        vendor_name = dt["vendor_name"].ToString(),
                        gst_no = dt["gst_no"].ToString()
                    });
                    values.GetCompileInputTax_List = CompileInputTax_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCompileOutputTax(string user_gid, MdlTaxManagements values)
        {
            msSQL = " select tmptaxoutput_gid, invoice_gid," +
                    " date_format(invoice_date, '%d-%m-%Y') as invoice_date," +
                    " format(invoice_amount, 2) as invoice_amount, format(taxable_amount, 2) as taxable_amount," +
                    " customer_name, gst_no from acc_tmp_ttaxoutput ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var CompileOutputTax_List = new List<GetCompileOutputTax_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CompileOutputTax_List.Add(new GetCompileOutputTax_List
                    {
                        tmptaxoutput_gid = dt["tmptaxoutput_gid"].ToString(),
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        taxable_amount = dt["taxable_amount"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        gst_no = dt["gst_no"].ToString()
                    });
                    values.GetCompileOutputTax_List = CompileOutputTax_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCompileCreditTax(string user_gid, MdlTaxManagements values)
        {
            msSQL = " select tmpcredittaxoutput_gid,invoice_gid," +
                    " date_format(invoice_date, '%d-%m-%Y') as invoice_date, format(invoice_amount, 2) as invoice_amount," +
                    " format(taxable_amount, 2) as taxable_amount, customer_name, gst_no " +
                    " from acc_tmp_ttaxcreditnotetax ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var CompileCreditTax_List = new List<GetCompileCreditTax_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CompileCreditTax_List.Add(new GetCompileCreditTax_List
                    {
                        tmpcredittaxoutput_gid = dt["tmpcredittaxoutput_gid"].ToString(),
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        taxable_amount = dt["taxable_amount"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        gst_no = dt["gst_no"].ToString()
                    });
                    values.GetCompileCreditTax_List = CompileCreditTax_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCompileDebitTax(string user_gid, MdlTaxManagements values)
        {
            msSQL = " select tmpdebittaxoutput_gid,invoice_gid," +
                    " date_format(debit_date, '%d-%m-%Y') as debit_date, format(invoice_amount, 2) as invoice_amount," +
                    " format(taxable_amount, 2) as taxable_amount, vendor_name, gst_no " +
                    " from acc_tmp_ttaxdebitnote ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var CompileDebitTax_List = new List<GetCompileDebitTax_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CompileDebitTax_List.Add(new GetCompileDebitTax_List
                    {
                        tmpdebittaxoutput_gid = dt["tmpdebittaxoutput_gid"].ToString(),
                        invoice_gid = dt["invoice_gid"].ToString(),
                        debit_date = dt["debit_date"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        taxable_amount = dt["taxable_amount"].ToString(),
                        vendor_name = dt["vendor_name"].ToString(),
                        gst_no = dt["gst_no"].ToString()
                    });
                    values.GetCompileDebitTax_List = CompileDebitTax_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaOutputTaxSubmit(string user_gid, OutputTaxSummary values)
        {
            try
            {
                msSQL = " delete from acc_tmp_ttaxoutput";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                for (int i = 0; i < values.OutputTaxSummary_List.ToArray().Length; i++)
                {
                    // Parse the original date string using custom date format
                    DateTime outinvoice_date = DateTime.ParseExact(values.OutputTaxSummary_List[i].invoice_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    // Convert the date to the desired format
                    string invoiceout_date = outinvoice_date.ToString("yyyy-MM-dd");

                    msGetGid = objcmnfunctions.GetMasterGID("IVOT");

                    msSQL = " Insert into acc_tmp_ttaxoutput ( " +
                            " tmptaxoutput_gid, " +
                            " invoice_gid, " +
                            " invoice_date, " +
                            " customer_name , " +
                            " gst_no, " +
                            " invoice_amount, " +
                            " taxable_amount, " +
                            " created_by, " +
                            " created_date " +
                            " ) Values ( " +
                            " '" + msGetGid + "'," +
                            " '" + values.OutputTaxSummary_List[i].invoice_gid + "', " +
                            " '" + invoiceout_date + "', " +
                            " '" + values.OutputTaxSummary_List[i].customer_name + "', " +
                            " '" + values.OutputTaxSummary_List[i].gst_number + "', " +
                            " '" + Convert.ToDouble(values.OutputTaxSummary_List[i].invoice_amount) + "', " +
                            " '" + Convert.ToDouble(values.OutputTaxSummary_List[i].Taxable_Amount) + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occurred while updating Output Tax";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + values.message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "Finance/ErrorLog/Tax Management/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }  
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Records Inserted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while updating Output Tax";
                }
            }

            catch (Exception ex)
            {
                values.message = "Error Occurred while updating Output Tax";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "Finance/ErrorLog/Tax Management/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaCreditNoteTaxSubmit(string user_gid, CreditNoteTaxSummary values)
        {
            try
            {
                msSQL = " delete from acc_tmp_ttaxcreditnotetax";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                for (int i = 0; i < values.CreditNoteTaxSummary_List.ToArray().Length; i++)
                {

                    // Parse the original date string using custom date format
                    DateTime creditinvoice_date = DateTime.ParseExact(values.CreditNoteTaxSummary_List[i].credit_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    // Convert the date to the desired format
                    string invoicecredit_date = creditinvoice_date.ToString("yyyy-MM-dd");

                    msGetGid = objcmnfunctions.GetMasterGID("IVOT");

                    msSQL = " Insert into acc_tmp_ttaxcreditnotetax ( " +
                            " tmpcredittaxoutput_gid, " +
                            " invoice_gid, " +
                            " invoice_date, " +
                            " customer_name , " +
                            " gst_no, " +
                            " invoice_amount, " +
                            " taxable_amount, " +
                            " created_by, " +
                            " created_date " +
                            " ) Values ( " +
                            " '" + msGetGid + "'," +
                            " '" + values.CreditNoteTaxSummary_List[i].invoice_gid + "', " +
                            " '" + invoicecredit_date + "', " +
                            " '" + values.CreditNoteTaxSummary_List[i].customer_name + "', " +
                            " '" + values.CreditNoteTaxSummary_List[i].gst_number + "', " +
                            " '" + Convert.ToDouble(values.CreditNoteTaxSummary_List[i].invoice_amount) + "', " +
                            " '" + Convert.ToDouble(values.CreditNoteTaxSummary_List[i].Taxable_Amount) + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occurred while updating Credit Note Tax";
                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Records Inserted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while updating Credit Note Tax";
                }
            }

            catch (Exception ex)
            {
                values.message = "Error Occurred while updating Credit Note Tax";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaDebitNoteTaxSummary(string user_gid, DebitNoteTaxSummary values)
        {
            try
            {
                msSQL = " delete from acc_tmp_ttaxdebitnote";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                for (int i = 0; i < values.DebitNoteTaxSummary_List.ToArray().Length; i++)
                {
                    // Parse the original date string using custom date format
                    DateTime debitinvoice_date = DateTime.ParseExact(values.DebitNoteTaxSummary_List[i].debit_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    // Convert the date to the desired format
                    string invoicedebit_date = debitinvoice_date.ToString("yyyy-MM-dd");

                    msGetGid = objcmnfunctions.GetMasterGID("IVOT");

                    msSQL = " Insert into acc_tmp_ttaxdebitnote ( " +
                            " tmpdebittaxoutput_gid, " +
                            " invoice_gid, " +
                            " debit_date , " +
                            " vendor_name , " +
                            " gst_no, " +
                            " invoice_amount, " +
                            " taxable_amount, " +
                            " created_by, " +
                            " created_date " +
                            " ) Values ( " +
                            " '" + msGetGid + "'," +
                            " '" + values.DebitNoteTaxSummary_List[i].invoice_gid + "', " +
                            " '" + invoicedebit_date + "', " +
                            " '" + values.DebitNoteTaxSummary_List[i].vendor_name + "', " +
                            " '" + values.DebitNoteTaxSummary_List[i].gst_number + "', " +
                            " '" + Convert.ToDouble(values.DebitNoteTaxSummary_List[i].invoice_amount) + "', " +
                            " '" + Convert.ToDouble(values.DebitNoteTaxSummary_List[i].Taxable_Amount) + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occurred while updating Debit Note Tax";
                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Records Inserted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while updating Debit Note Tax";
                }
            }

            catch (Exception ex)
            {
                values.message = "Error Occurred while updating Debit Note Tax";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }        
        public void DaGetTaxCalculation(string user_gid, MdlTaxManagements values)
        {
            decimal lsinterest_payable = 0;
            decimal lspenalty_payable = 0;
            decimal dlstax_payable = 0;
            decimal lbltotaltax = 0;
            decimal dlstotaloutputcredit_tax = 0;
            decimal dlstotalinputdebit_tax = 0;
            decimal dlslesscarry_forward = 0;

            msSQL = " select (select Format(sum(taxable_amount),2) from acc_tmp_ttaxinput) as input_tax, " +
                    " (select Format(sum(taxable_amount),2) from acc_tmp_ttaxoutput) as output_tax," +
                    " (select Format(sum(taxable_amount),2) from acc_tmp_ttaxcreditnotetax) as credit_tax, " +
                    " (select Format(sum(taxable_amount),2) from acc_tmp_ttaxdebitnote) as debit_tax ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var TaxCalculation_List = new List<GetTaxCalculation_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                string totinput_tax = dt_datatable.Rows[0]["input_tax"].ToString();
                string totoutput_tax = dt_datatable.Rows[0]["output_tax"].ToString();
                string totcredit_tax = dt_datatable.Rows[0]["credit_tax"].ToString();
                string totdebit_tax = dt_datatable.Rows[0]["debit_tax"].ToString();
                
                if (totinput_tax != "")
                {
                     dtotinput_tax = decimal.Parse(totinput_tax);
                }
                if (totoutput_tax != "")
                {
                    dtotoutput_tax = decimal.Parse(totoutput_tax);
                }
                if (totcredit_tax != "")
                {
                    dtotcredit_tax = decimal.Parse(totcredit_tax);
                }
                if (totdebit_tax != "")
                {
                    dtotdebit_tax = decimal.Parse(totdebit_tax);
                }


                if (totinput_tax != "" || totoutput_tax != "")
                {
                    decimal lstotal = dtotoutput_tax - dtotcredit_tax- dtotinput_tax;

                    if (lstotal > 0)
                    {
                        dlstotaloutputcredit_tax = dtotoutput_tax - dtotcredit_tax;
                        dlstotalinputdebit_tax = dtotinput_tax + dtotdebit_tax;
                        dlstax_payable = (dtotcredit_tax - dtotinput_tax) + (dtotdebit_tax);                       
                    }
                    else
                    {
                        dlstotaloutputcredit_tax = dtotoutput_tax - dtotcredit_tax;
                        dlslesscarry_forward = (dtotcredit_tax - dtotinput_tax) + (dtotdebit_tax);                       
                    }
                    lbltotaltax = dlstax_payable + lsinterest_payable + lspenalty_payable;
                }
                else { }

                string totaloutputcredit_tax = FormatNumber(dlstotaloutputcredit_tax);
                string totalinputdebit_tax = FormatNumber(dlstotalinputdebit_tax);
                string lstax_payable = FormatNumber(dlstax_payable);
                string lesscarry_forward = FormatNumber(dlslesscarry_forward);
                string ttotaltax = FormatNumber(lbltotaltax);

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    TaxCalculation_List.Add(new GetTaxCalculation_List
                    {
                        input_tax = totinput_tax,
                        output_tax = totoutput_tax,
                        credit_tax = totcredit_tax,
                        debit_tax = totdebit_tax,
                        totaloutputcredit_tax = totaloutputcredit_tax,
                        totalinputdebit_tax = totalinputdebit_tax,
                        tax_payable = lstax_payable,
                        lesscarry_forward = lesscarry_forward,
                        totaltax = ttotaltax

                    });
                    values.GetTaxCalculation_List = TaxCalculation_List;
                }
            }
            dt_datatable.Dispose();
        }        
        // Tax Overall Submit
        public void DaPostTaxOverallSubmit(string user_gid, TaxOverallSubmitDtls values)
        {
            try
            {
                msGetGidfil = objcmnfunctions.GetMasterGID("FTXF");

                if (values.CompileInputTax_List != null) 
                { 
                    foreach (var data1 in values.CompileInputTax_List)
                    {
                        // Parse the original date string using custom date format
                        DateTime compileininvoice_date = DateTime.ParseExact(data1.invoice_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        // Convert the date to the desired format
                        string compileinputinvoice_date = compileininvoice_date.ToString("yyyy-MM-dd");

                        msGetGid = objcmnfunctions.GetMasterGID("FTDT");

                        msSQL = " Insert into acc_trn_ttaxfilingdtl ( " +
                                " taxfilingdtl_gid, " +
                                " taxfiling_gid, " +
                                " taxinput_gid , " +
                                " invoice_gid , " +
                                " invoice_date, " +
                                " invoice_type, " +
                                " vendor_name, " +
                                " gst_no, " +
                                " invoice_amount, " +
                                " taxable_amount, " +
                                " created_by, " +
                                " created_date " +
                                " ) Values ( " +
                                " '" + msGetGid + "'," +
                                " '" + msGetGidfil + "'," +
                                " '" + data1.tmptaxinput_gid + "', " +
                                 " '" + data1.invoice_gid + "', " +
                                " '" + compileinputinvoice_date + "', " +
                                " 'input', " +
                                " '" + data1.vendor_name + "', " +
                                " '" + data1.gst_no + "', " +
                                " '" + Convert.ToDouble(data1.invoice_amount) + "', " +
                                " '" + Convert.ToDouble(data1.taxable_amount) + "', " +
                                " '" + user_gid + "', " +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " update acp_trn_tinvoice set taxfiling_flag='Y'" +
                                    " where invoice_gid='" + data1.invoice_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred while updating Input Tax";
                        }

                    }
                }
                if (values.CompileOutputTax_List != null)
                {
                    foreach (var data2 in values.CompileOutputTax_List)
                    {
                        // Parse the original date string using custom date format
                        DateTime compileoutinvoice_date = DateTime.ParseExact(data2.invoice_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        // Convert the date to the desired format
                        string compileoutputinvoice_date = compileoutinvoice_date.ToString("yyyy-MM-dd");

                        msGetGid = objcmnfunctions.GetMasterGID("FTDT");

                        msSQL = " Insert into acc_trn_ttaxfilingdtl ( " +
                                " taxfilingdtl_gid, " +
                                " taxfiling_gid, " +
                                " taxoutput_gid , " +
                                " invoice_gid , " +
                                " invoice_date, " +
                                " invoice_type, " +
                                " customer_name, " +
                                " gst_no, " +
                                " invoice_amount, " +
                                " taxable_amount, " +
                                " created_by, " +
                                " created_date " +
                                " ) Values ( " +
                                " '" + msGetGid + "'," +
                                " '" + msGetGidfil + "'," +
                                " '" + data2.tmptaxoutput_gid + "', " +
                                 " '" + data2.invoice_gid + "', " +
                                " '" + compileoutputinvoice_date + "', " +
                                " 'output', " +
                                " '" + data2.customer_name + "', " +
                                " '" + data2.gst_no + "', " +
                                " '" + Convert.ToDouble(data2.invoice_amount) + "', " +
                                " '" + Convert.ToDouble(data2.taxable_amount) + "', " +
                                " '" + user_gid + "', " +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " update rbl_trn_tinvoice set taxfiling_flag='Y'" +
                                    " where invoice_gid='" + data2.invoice_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occurred while updating Output Tax";
                        }

                    }
                }
                if (values.CompileCreditTax_List != null)
                { 
                foreach (var data3 in values.CompileCreditTax_List)
                {
                        // Parse the original date string using custom date format
                        DateTime compilecrinvoice_date = DateTime.ParseExact(data3.invoice_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        // Convert the date to the desired format
                        string compilecreditinvoice_date = compilecrinvoice_date.ToString("yyyy-MM-dd");

                        msGetGid = objcmnfunctions.GetMasterGID("FTDT");

                    msSQL = " Insert into acc_trn_ttaxfilingdtl ( " +
                            " taxfilingdtl_gid, " +
                            " taxfiling_gid, " +
                            " taxoutput_gid , " +
                            " invoice_gid , " +
                            " invoice_date, " +
                            " invoice_type, " +
                            " customer_name, " +
                            " gst_no, " +
                            " invoice_amount, " +
                            " taxable_amount, " +
                            " created_by, " +
                            " created_date " +
                            " ) Values ( " +
                            " '" + msGetGid + "'," +
                            " '" + msGetGidfil + "'," +
                            " '" + data3.tmpcredittaxoutput_gid + "', " +
                            " '" + data3.invoice_gid + "', " +
                            " '" + compilecreditinvoice_date + "', " +
                            " 'credit', " +
                            " '" + data3.customer_name + "', " +
                            " '" + data3.gst_no + "', " +
                            " '" + Convert.ToDouble(data3.invoice_amount) + "', " +
                            " '" + Convert.ToDouble(data3.taxable_amount) + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                    {
                        msSQL = " update rbl_trn_tinvoice set taxfiling_flag='Y'" +
                               " where invoice_gid='" + data3.invoice_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred while updating Credit Tax";
                    }

                }
                }
                msSQL = " select taxfiling_gid,month(invoice_date) as month,year(invoice_date) as year, " +
                        " invoice_date from  acc_trn_ttaxfilingdtl " +
                        " where taxfiling_gid = '" + msGetGidfil + "' group by taxfiling_gid ";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    lsmonth = objODBCDatareader["month"].ToString();
                    lsyear = objODBCDatareader["year"].ToString();
                    invoice_date = objODBCDatareader["invoice_date"].ToString();
                }
                objODBCDatareader.Close();

                msSQL = "select tax_carryforward from acc_trn_ttaxfiling where month='" + lsmonth + "'" +
                        "and year='" + lsyear + "'";
                lblprecarry = objdbconn.GetExecuteScalar(msSQL);
                if( lblprecarry == "")
                {
                    lblprecarry = "0.00";
                }            
       
                 
                if (values.TaxCalculation_List[0].tax_payable != "0")
                {
                    dtotax_payable = double.Parse(values.TaxCalculation_List[0].tax_payable);
                    dtointerestcharge_payable = double.Parse(values.interestcharge_payable);
                    dtopenality_payable = double.Parse(values.penality_payable);
                    dlblprecarry = double.Parse(lblprecarry);

                    lbldtotaltax = dtotax_payable + dtointerestcharge_payable + dtopenality_payable + dlblprecarry;                    
                }
                else {
                    ls_lblcarryforward = lblcarryforward + lblprecarry;
                }
                if (ls_lblcarryforward == "")
                {
                    ls_lblcarryforward = "0.00";
                }
                else
                {
                    ls_lblcarryforward = lblcarryforward + lblprecarry;

                }

                 msSQL = " Insert into acc_trn_ttaxfiling ( " +
                         " taxfiling_gid, " +
                         " tax_input, " +
                         " tax_output , " +
                         " tax_payable , " +
                         " tax_carryforward, " +
                         " interest_payable, " +
                         " penalty_payable, " +
                         " pre_carry_forward, " +
                         " overalltax_amount, " +
                         " month, " +
                         " year, " +
                         " created_by, " +
                         " created_date " +
                         " ) Values ( " +
                         " '" + msGetGidfil + "'," +
                         " '" + Convert.ToDouble(values.TaxCalculation_List[0].input_tax) + "'," +
                         " '" + Convert.ToDouble(values.TaxCalculation_List[0].output_tax) + "', " +
                         " '" + Convert.ToDouble(values.TaxCalculation_List[0].tax_payable) + "', " +
                         " '" + ls_lblcarryforward + "', " +
                         " '" + Convert.ToDouble(values.interestcharge_payable) + "', " +
                         " '" + Convert.ToDouble(values.penality_payable) + "', " +
                         " '" + dlblprecarry + "', " +
                         " '" + lbldtotaltax + "', " +
                         " '" + lsmonth + "', " +
                         " '" + lsyear + "', " +
                         " '" + user_gid + "', " +
                         " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Tax Filing Submitted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while Adding Tax Filing";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Tax Filing!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaTempTableDataDeletions(MdlTaxManagements values)
        {
            try
            {
                msSQL = " delete from acc_tmp_ttaxinput";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " delete from acc_tmp_ttaxoutput";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " delete from acc_tmp_ttaxcreditnotetax";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " delete from acc_tmp_ttaxdebitnote";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Deleting Temp Table Datas!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}