using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;

namespace ems.finance.DataAccess
{
    public class DaPurchaseLedger
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string lsstart_date = "", lsend_date = "";
        public void DaGetPurchaselegder(MdlPurchaseLedger values)
        {

            try
            {
                //msSQL = "  SELECT  DATE_FORMAT(a.transaction_date, '%d-%m-%Y') AS invoice_date,  " +
                //        "  a.journal_refno,a.transaction_gid AS invoice_gid, c.account_gid,e.vendor_code , " +
                //        "  e.vendor_companyname ,a.journal_gid,e.vendor_gid , format(d.transaction_amount, 2) as transaction_amount , " +
                //        "  COALESCE(tax.tax_amount, '0.00') AS tax_amount, COALESCE(net.net_amount, '0.00') AS net_amount, " +
                //        "  COALESCE(net.account_name, c.account_name) AS account_name FROM acc_trn_journalentry a " +
                //        "  LEFT JOIN acc_trn_journalentrydtl d ON d.journal_gid = a.journal_gid " +
                //        "  LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = d.account_gid " +
                //        "  LEFT JOIN acp_mst_tvendor e ON e.vendor_gid = a.reference_gid  " +
                //        "  LEFT JOIN(SELECT b.journal_gid, FORMAT(SUM(b.transaction_amount), 2) AS tax_amount " +
                //        "  FROM acc_trn_journalentry a " +
                //        "  LEFT JOIN acc_trn_journalentrydtl b ON b.journal_gid = a.journal_gid " +
                //        "  LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = b.account_gid " +
                //        "  WHERE c.ledger_type = 'N' AND c.display_type = 'N' and b.journal_type = 'dr' " +
                //        "  GROUP BY b.journal_gid, c.account_name) tax ON tax.journal_gid = a.journal_gid " +
                //        "  LEFT JOIN(SELECT b.journal_gid, FORMAT((b.transaction_amount), 2) AS net_amount, c.account_name " +
                //        "  FROM acc_trn_journalentry a " +
                //        "  LEFT JOIN acc_trn_journalentrydtl b ON b.journal_gid = a.journal_gid " +
                //        "  LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = b.account_gid " +
                //        "  WHERE c.ledger_type = 'Y' AND c.display_type = 'N' and b.journal_type = 'dr' and transaction_amount<>'0.00' GROUP BY b.journal_gid, c.account_name) net ON net.journal_gid = a.journal_gid " +
                //        "  WHERE a.journal_from = 'Purchase' AND d.journal_type = 'cr' AND a.transaction_type = 'Journal'   order by a.transaction_date desc ";

                msSQL = "call acp_rpt_Purchaseledger()";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetPurchaseLedgerfin_list = new List<GetPurchaseLedgerfin_list>();
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetPurchaseLedgerfin_list.Add(new GetPurchaseLedgerfin_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_gid = dt["invoice_gid"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        vendor_code = dt["vendor_code"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        journal_gid = dt["journal_gid"].ToString(),
                        vendor_gid = dt["vendor_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        transaction_amount = dt["transaction_amount"].ToString(),
                        tax_amount = dt["tax_amount"].ToString(),
                        net_amount = dt["net_amount"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                    });
                }
                values.GetPurchaseLedgerfin_list = GetPurchaseLedgerfin_list;
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

        }

        public void DaGetPurchaselegderDatefin(string from_date, string to_date, MdlPurchaseLedger values)
        {

            try
            {

                if ((from_date == null) || (to_date == null))
                {
                    lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                    lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsstart_date = from_date1.ToString("yyyy-MM-dd");
                    DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsend_date = lsDateto.ToString("yyyy-MM-dd");
                }
                msSQL = "  SELECT  DATE_FORMAT(a.transaction_date, '%d-%m-%Y') AS invoice_date,  " +
                        "  a.journal_refno,a.transaction_gid AS invoice_gid, c.account_gid,e.vendor_code , " +
                        "  e.vendor_companyname ,a.journal_gid,e.vendor_gid , format(d.transaction_amount, 2) as transaction_amount , " +
                        "  COALESCE(tax.tax_amount, '0.00') AS tax_amount, COALESCE(net.net_amount, '0.00') AS net_amount, " +
                        "  COALESCE(net.account_name, c.account_name) AS account_name FROM acc_trn_journalentry a " +
                        "  LEFT JOIN acc_trn_journalentrydtl d ON d.journal_gid = a.journal_gid " +
                        "  LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = d.account_gid " +
                        "  LEFT JOIN acp_mst_tvendor e ON e.vendor_gid = a.reference_gid  " +
                        "  LEFT JOIN(SELECT b.journal_gid, FORMAT(SUM(b.transaction_amount), 2) AS tax_amount " +
                        "  FROM acc_trn_journalentry a " +
                        "  LEFT JOIN acc_trn_journalentrydtl b ON b.journal_gid = a.journal_gid " +
                        "  LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = b.account_gid " +
                        "  WHERE c.ledger_type = 'N' AND c.display_type = 'N' and b.journal_type = 'dr' " +
                        "  GROUP BY b.journal_gid) tax ON tax.journal_gid = a.journal_gid " +
                        "  LEFT JOIN(SELECT b.journal_gid, FORMAT(b.transaction_amount, 2) AS net_amount, c.account_name " +
                        "  FROM acc_trn_journalentry a " +
                        "  LEFT JOIN acc_trn_journalentrydtl b ON b.journal_gid = a.journal_gid " +
                        "  LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = b.account_gid " +
                        "  WHERE c.ledger_type = 'Y' AND c.display_type = 'N' and b.journal_type = 'dr' and transaction_amount<>'0.00' GROUP BY b.journal_gid) net ON net.journal_gid = a.journal_gid " +
                        "  WHERE a.journal_from = 'Purchase' AND d.journal_type = 'cr' AND a.transaction_type = 'Journal' and  a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "'" +
                        "  order by a.transaction_date desc ";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetPurchaseLedgerfin_list = new List<GetPurchaseLedgerfin_list>();
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetPurchaseLedgerfin_list.Add(new GetPurchaseLedgerfin_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_gid = dt["invoice_gid"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        vendor_code = dt["vendor_code"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        journal_gid = dt["journal_gid"].ToString(),
                        vendor_gid = dt["vendor_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        transaction_amount = dt["transaction_amount"].ToString(),
                        tax_amount = dt["tax_amount"].ToString(),
                        net_amount = dt["net_amount"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                    });
                }
                values.GetPurchaseLedgerfin_list = GetPurchaseLedgerfin_list;
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

        }
        public void DaGetPurchaselegderView (string invoice_gid, MdlPurchaseLedger values)
        {
            try
            {
                msSQL = "SELECT a.invoice_gid,a.tax_name as tax_name4,k.email_id,a.mode_despatch,a.billing_email,a.shipping_address,a.delivery_term,a.termsandconditions," +
                        " CASE WHEN a.invoice_refno IS NULL OR a.invoice_refno = '' THEN a.invoice_gid ELSE a.invoice_refno END AS invoice_refno, " +
                        " a.invoice_refno,a.invoice_type,concat(m.address1,'\n',m.address2,'\n',m.city,'\n',m.state,'\n',m.postal_code" +
                        " ) as vendor_address,a.vendorinvoiceref_no,k.contact_telephonenumber,a.order_total,a.received_amount,a.received_year,k.contactperson_name," +
                        " j.branch_name,k.vendor_companyname,a.branch_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,i.costcenter_name,i.budget_available, " +
                        " date_format(a.payment_date,'%d-%m-%Y') as payment_date,a.round_off, a.payment_term, a.vendor_gid,format(a.extraadditional_amount, 2) as extraadditional_amount, " +
                        " case when a.currency_code is null then 'INR' else a.currency_code end as currency_code,format(a.extradiscount_amount, 2) as extradiscount_amount, " +
                        " case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,format(e.product_total,2) as price, " +
                        " a.invoice_remarks, a.payment_days,a.reject_reason, a.invoice_status, format(a.invoice_amount,2) as invoice_amount, a.invoice_reference, " +
                        " format(a.freightcharges_amount, 2) as freightcharges_amount, format(a.additionalcharges_amount, 2) as additionalcharges_amount, " +
                        " format(a.discount_amount, 2) as discount_amount, format(a.total_amount, 2) as total_amount,  " +
                        " format(a.freightcharges,2) as freightcharges,format(a.buybackorscrap,2) as buybackorscrap,a.invoice_total,a.raised_amount,a.tax_amount,a.tax_name " +
                        " FROM acp_trn_tinvoice a " +
                        " left join acp_trn_tinvoicedtl e on a.invoice_gid=e.invoice_gid " +
                        " left join acp_trn_tpo2invoice g on a.invoice_gid=g.invoice_gid " +
                        " left join pmr_trn_tpurchaseorder h on g.purchaseorder_gid=h.purchaseorder_gid " +
                        " left join pmr_mst_tcostcenter i on h.costcenter_gid=i.costcenter_gid " +
                        " left join hrm_mst_tbranch j on a.branch_gid=j.branch_gid " +
                        " left join acp_mst_tvendor k on a.vendor_gid=k.vendor_gid " +
                        " left join adm_mst_taddress m on m.address_gid=k.address_gid  " +
                        " where a.invoice_gid = '" + invoice_gid + "' group by a.invoice_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPurchaseLedgerView_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new GetPurchaseLedgerView_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),

                            branch_name = dt["branch_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            vendorinvoiceref_no = dt["vendorinvoiceref_no"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            invoice_type = dt["invoice_type"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            round_off = dt["round_off"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            price = dt["price"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            tax_name4 = dt["tax_name4"].ToString(),
                            delivery_term = dt["delivery_term"].ToString()
                        });
                        values.GetPurchaseLedgerView_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Purchase Ledger View Summary!!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetPurchaselegderProductView(string invoice_gid, MdlPurchaseLedger values)
        {
            try
            {
                double grand_total = 0.00;
                msSQL = " SELECT a.product_gid, concat(a.qty_invoice,' ',a.productuom_name) as qty_invoice, c.invoice_amount,format(a.product_price,2) as product_price, " +
                        " format(a.discount_percentage,2) as discount_percentage,c.round_off,c.buybackorscrap,c.freightcharges, " +
                        " a.taxseg_taxname1,a.taxseg_taxpercent1,a.taxseg_taxamount1,a.taxseg_taxname2,a.taxseg_taxpercent2,a.taxseg_taxamount2,a.taxseg_taxname3,a.taxseg_taxpercent3,taxseg_taxamount3, " +
                        " a.tax_name,a.tax_name2,a.tax_name3,format(a.tax_amount,2),CASE  WHEN a.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(a.tax_amount2, 2) END AS tax_amount2,format(a.tax_amount3,2)as tax_amount3," +
                        " format(a.discount_amount,2) as discount_amount, format(a.tax_percentage,2) as tax_percentage, " +
                        " format(a.tax_amount,2) as tax_amount, a.product_remarks,  format((a.qty_invoice * a.product_price),2) as product_totalprice, " +
                        " format(a.excise_percentage,2) as excise_percentage, format(a.excise_amount,2) as excise_amount, " +
                        " format(a.product_total,2) as product_total, b.po2invoice_gid, a.invoice_gid, a.invoicedtl_gid, b.grn_gid, b.grndtl_gid, " +
                        " b.purchaseorder_gid, b.purchaseorderdtl_gid, b.product_gid, b.qty_invoice,c.additionalcharges_amount, " +
                        " a.product_name,a.display_field, a.product_code,d.additional_name,c.extradiscount_amount,c.extraadditional_amount,e.discount_name, " +
                        " a.productgroup_name,c.termsandconditions, a.productuom_name,f.tax_prefix, " +
                        " concat(f.tax_prefix,', ',a.tax_name) as taxnames,concat(a.tax_amount,', ',a.tax_amount2) as taxamts from acp_trn_tinvoicedtl a " +
                        " left join acp_trn_tpo2invoice b on a.invoicedtl_gid = b.invoicedtl_gid " +
                        " left join acp_trn_tinvoice c on a.invoice_gid = c.invoice_gid   " +
                        " left join acp_mst_ttax f on f.tax_gid = a.tax2_gid     " +
                        " left join pmr_trn_tadditional d on c.extraadditional_code = d.additional_gid  " +
                        " left join pmr_trn_tdiscount e on c.extradiscount_code = e.discount_gid   " +
                        " where a.invoice_gid = '" + invoice_gid + "'" +
                        " order by a.product_name ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPurchaseLedgerProductView_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["product_total"].ToString());
                        getModuleList.Add(new GetPurchaseLedgerProductView_list
                        {

                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field_name = dt["display_field"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            tax_amount1 = dt["tax_amount"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            additional_name = dt["additional_name"].ToString(),
                            extradiscount_amount = dt["extradiscount_amount"].ToString(),
                            extraadditional_amount = dt["extraadditional_amount"].ToString(),
                            discount_name = dt["discount_name"].ToString(),
                            round_off = dt["round_off"].ToString(),
                            addon_amount = dt["additionalcharges_amount"].ToString(),
                            buybackorscrap = dt["buybackorscrap"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            taxseg_taxname1 = dt["taxseg_taxname1"].ToString(),
                            taxseg_taxpercent1 = dt["taxseg_taxpercent1"].ToString(),
                            taxseg_taxamount1 = dt["taxseg_taxamount1"].ToString(),
                            taxseg_taxname2 = dt["taxseg_taxname2"].ToString(),
                            taxseg_taxpercent2 = dt["taxseg_taxpercent2"].ToString(),
                            taxseg_taxamount2 = dt["taxseg_taxamount2"].ToString(),
                            taxseg_taxname3 = dt["taxseg_taxname3"].ToString(),
                            taxseg_taxpercent3 = dt["taxseg_taxpercent3"].ToString(),
                            taxseg_taxamount3 = dt["taxseg_taxamount3"].ToString(),
                            product_description = dt["display_field"].ToString()

                        });
                        values.GetPurchaseLedgerProductView_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.grand_total = grand_total;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Purchase Ledger Product Summary!!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}