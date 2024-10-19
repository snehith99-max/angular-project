using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ems.pmr.Models;
using ems.utilities.Functions;
using StoryboardAPI.Authorization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Http.Routing.Constraints;

namespace ems.pmr.DataAccess
{
    public class DaPblDebitNote
    {
        string msSQL, lsbranch, msGetGID, msPYGetGID, mspGetGID, mssalesorderGID, msstockreturn = string.Empty;
        int mnResult;
        DataTable dt_datatable, objTbl;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objfncmnfunction = new finance_cmnfunction();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        DataSet ds_dataset = new DataSet();
        DataTable dt_table = new DataTable();
        OdbcDataReader objOdbcDataReader;
        string  lsfrom, lsordergid, invoice_gid = string.Empty;
        double invoice_amount, debit_amount;
        string path, full_path = string.Empty ;
        string stock_gid, stock_balance;
        string lstax2 = "0.00", lstax1 = "0.00", lstax3 = "0.00";
        public void DaGetDebitNoteSummary(MdlPblDebitNote values)
        {
            msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
            string currency = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select distinct d.debitnote_gid,a.invoice_from, d.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag,a.mail_status,a.vendor_gid,date_format(a.invoice_date, '%d-%m-%Y') as invoice_date,i.vendor_refnodate," +
            " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status'," +
            " a.payment_flag,  format(a.initialinvoice_amount,2) as initialinvoice_amount,date_format(a.debit_date, '%d-%m-%Y')as debit_date,d.payment_gid," +
            " format(a.invoice_amount,2) as invoice_amount,format(d.debit_amount,2) as debit_amount,c.contactperson_name,format(d.debit_amount,2) as debit_amount," +
            " case when a.currency_code = '" + currency + "' then c.vendor_companyname when a.currency_code is null then c.vendor_companyname" +
            " when a.currency_code is not null and a.currency_code <> '" + currency + "' then concat(c.vendor_companyname,' / ',h.country) end as vendor_companyname," +
            " c.contact_telephonenumber as mobile" +
            " from pmr_trn_tdebitnote d" +
            " left join acp_trn_tinvoice a on a.invoice_gid = d.invoice_gid" +
            " left join rbl_trn_tinvoicedtl b on d.invoice_gid = b.invoice_gid" +
            " left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid" +
            " left join acp_trn_tinvoicedtl i on a.invoice_gid =i.invoice_gid " +
            " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code" +
            " where a.debit_date is not null";
            msSQL += " order by date(a.invoice_date) desc,a.invoice_date asc, d.invoice_gid desc ";
            ds_dataset = objdbconn.GetDataSet(msSQL, "pmr_trn_tdebitnote");
            var Getdebitnote = new List<Getdebitnote_list>();
            if (ds_dataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                {
                    Getdebitnote.Add(new Getdebitnote_list
                    {
                        invoice_gid = ds["invoice_gid"].ToString(),
                        debitnote_gid = ds["debitnote_gid"].ToString(),
                        vendor_gid = ds["vendor_gid"].ToString(),
                        invoice_date = ds["invoice_date"].ToString(),
                        invoice_refno = ds["invoice_refno"].ToString(),
                        vendor_refnodate = ds["vendor_refnodate"].ToString(),
                        vendor_companyname = ds["vendor_companyname"].ToString(),
                        contactperson_name = ds["contactperson_name"].ToString(),
                        mobile = ds["mobile"].ToString(),
                        invoice_amount = ds["invoice_amount"].ToString(),
                        debit_amount = ds["debit_amount"].ToString(),
                        debit_date = ds["debit_date"].ToString(),
                        invoice_from = ds["invoice_from"].ToString(),
                        payment_gid = ds["payment_gid"].ToString(),
                    });
                    values.Getdebitnote_list = Getdebitnote;                                                                        
                }                 
            }
            if (values.Getdebitnote_list != null)
            {
                var GetDebitdtl = new List<GetDebitdtl_list>();
                for (int i = 0; i < values.Getdebitnote_list.ToArray().Length; i++)
                {
                    msSQL = " select * from pbl_trn_tdebitnotedtl where debitnote_gid='" + values.Getdebitnote_list[i].debitnote_gid + "'";
                    dt_table  = objdbconn.GetDataTable(msSQL);                    
                    if (dt_table.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dt_table.Rows)
                        {
                            GetDebitdtl.Add(new GetDebitdtl_list
                            {
                                debitnotedtl_gid = dt["debitnotedtl_gid"].ToString(),
                                debitnote_gid = dt["debitnote_gid"].ToString(),
                            });                            
                        }
                    }
                }
                values.GetDebitdtl_list = GetDebitdtl;
            }
        }
        public void DaGetRaiseDebitNote(MdlPblDebitNote values)
        {
            msSQL = " select a.invoice_gid,a.invoice_refno,b.vendor_companyname,a.invoice_status,format(a.invoice_amount,2)as invoice_amount,b.vendor_gid," +
                " format((a.payment_amount+a.advance_amount),2) as payment_amount,format((a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding,a.debit_note," +
                " case when b.contact_telephonenumber is null then  concat(b.contactperson_name,'/ ',b.contact_telephonenumber,'/ ',b.email_id)" +
                " when b.contact_telephonenumber is not null then concat(b.contactperson_name,'/ ',b.contact_telephonenumber,'/ ',b.email_id) end as contact," +
                " e.costcenter_name,f.vendor_refnodate " +
                " from acp_trn_tinvoice a" +
                " left join acp_mst_tvendor b on b.vendor_gid=a.vendor_gid" +
                " left join acp_trn_tpo2invoice c on a.invoice_gid=c.invoice_gid " +
                " left join pmr_trn_tpurchaseorder d on c.purchaseorder_gid=d.purchaseorder_gid " +
                " left join pmr_mst_tcostcenter e on d.costcenter_gid=e.costcenter_gid " +
                " left join acp_trn_tinvoicedtl f on a.invoice_gid=f.invoice_gid " +
                " where a.invoice_amount>(a.payment_amount+a.debit_note) and a.invoice_date<='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
            msSQL += "group by a.invoice_gid order by a.invoice_gid desc";
            ds_dataset = objdbconn.GetDataSet(msSQL, "acp_trn_tinvoice");
            var Getraisedebitnote = new List<Getraisedebitnote_list>();
            if (ds_dataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                {
                    Getraisedebitnote.Add(new Getraisedebitnote_list
                    {
                        invoice_gid = ds["invoice_gid"].ToString(),
                        invoice_refno = ds["invoice_refno"].ToString(),
                        invoice_status = ds["invoice_status"].ToString(),
                        vendor_companyname = ds["vendor_companyname"].ToString(),
                        invoice_amount = ds["invoice_amount"].ToString(),
                        vendor_gid = ds["vendor_gid"].ToString(),
                        payment_amount = ds["payment_amount"].ToString(),
                        outstanding = ds["outstanding"].ToString(),
                        contact = ds["contact"].ToString(),
                        debit_note = ds["debit_note"].ToString(),
                        costcenter_name = ds["costcenter_name"].ToString(),
                        vendor_refnodate = ds["vendor_refnodate"].ToString(),
                    });
                    values.Getraisedebitnote_list = Getraisedebitnote;
                }

            }
        }
        public void DaGetRaiseDebitNoteAdd(string invoice_gid, MdlPblDebitNote values)
        {
            msSQL = "select a.invoice_gid,a.invoice_refno,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date," +
                "a.payment_term,date_format(a.payment_date,'%d-%m-%Y')as payment_date,e.price, " +
                  " format(a.total_amount,2)as total_amount,format(a.debit_note,2) as debit_note,a.invoice_from," +
                  "b.purchaseorder_gid as invoice_reference,a.tax_amount,a.tax_percentage,a.tax_name, " +
                  " format(a.additionalcharges_amount,2)as additionalcharges_amount,format(a.discount_amount,2)as discount_amount ," +
                  "format(a.invoice_amount,2)as invoice_amount,a.currency_code, " +
                  " c.vendor_companyname,c.contactperson_name,concat(f.address1,',',address2) as vendor_address," +
                  "c.email_id,c.contact_telephonenumber  as mobile, " +
                  " format((a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding,format(a.freightcharges,2) as freightcharges," +
                  "format(a.buybackorscrap,2) as buybackorscrap, " +
                  " format((a.payment_amount+a.advance_amount),2) as payment_amount,h.costcenter_name,e.vendor_refnodate, " +
                  " e.product_remarks,a.invoice_remarks,a.branch_gid,d.branch_name,a.vendor_gid,a.exchange_rate from acp_trn_tinvoice a " +
                  " left join (select purchaseorder_gid,invoice_gid from acp_trn_tpo2invoice ) b on b.invoice_gid=a.invoice_gid " +
                  " left join acp_mst_tvendor c on a.vendor_gid=c.vendor_gid " +
                  " left join adm_mst_taddress f on c.address_gid=f.address_gid " +
                  " left join pmr_trn_tpurchaseorder g on b.purchaseorder_gid=g.purchaseorder_gid " +
                  " left join pmr_mst_tcostcenter h on g.costcenter_gid=h.costcenter_gid " +
                  " left join hrm_mst_tbranch d on a.branch_gid=d.branch_gid " +
                  " left join (select invoice_gid,format(sum( product_total),2) as price,product_remarks,vendor_refnodate from acp_trn_tinvoicedtl group by invoice_gid order by invoice_gid)  e on e.invoice_gid=a.invoice_gid " +
                  " where a.invoice_gid='" + invoice_gid + "' group by a.invoice_gid ";
            ds_dataset = objdbconn.GetDataSet(msSQL, "acp_trn_tinvoice");
            var GetRaiseDebitNoteAdd = new List<GetRaiseDebitNoteAdd_list>();
            if (ds_dataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                {
                    GetRaiseDebitNoteAdd.Add(new GetRaiseDebitNoteAdd_list
                    {
                        invoice_gid = ds["invoice_gid"].ToString(),
                        invoice_refno = ds["invoice_refno"].ToString(),
                        invoice_date = ds["invoice_date"].ToString(),
                        payment_term = ds["payment_term"].ToString(),
                        payment_date = ds["payment_date"].ToString(),
                        price = ds["price"].ToString(),
                        total_amount = ds["total_amount"].ToString(),
                        debit_note = ds["debit_note"].ToString(),
                        invoice_from = ds["invoice_from"].ToString(),
                        invoice_reference = ds["invoice_reference"].ToString(),
                        tax_amount = ds["tax_amount"].ToString(),
                        tax_percentage = ds["tax_percentage"].ToString(),
                        tax_name = ds["tax_name"].ToString(),
                        additionalcharges_amount = ds["additionalcharges_amount"].ToString(),
                        discount_amount = ds["discount_amount"].ToString(),
                        invoice_amount = ds["invoice_amount"].ToString(),
                        currency_code = ds["currency_code"].ToString(),
                        vendor_companyname = ds["vendor_companyname"].ToString(),
                        contactperson_name = ds["contactperson_name"].ToString(),
                        vendor_address = ds["vendor_address"].ToString(),
                        email_id = ds["email_id"].ToString(),
                        mobile = ds["mobile"].ToString(),
                        outstanding = ds["outstanding"].ToString(),
                        freightcharges = ds["freightcharges"].ToString(),
                        buybackorscrap = ds["buybackorscrap"].ToString(),
                        payment_amount = ds["payment_amount"].ToString(),
                        costcenter_name = ds["costcenter_name"].ToString(),
                        vendor_refnodate = ds["vendor_refnodate"].ToString(),
                        product_remarks = ds["product_remarks"].ToString(),
                        invoice_remarks = ds["invoice_remarks"].ToString(),
                        branch_gid = ds["branch_gid"].ToString(),
                        branch_name = ds["branch_name"].ToString(),
                        vendor_gid = ds["vendor_gid"].ToString(),
                        exchange_rate = ds["exchange_rate"].ToString(),
                        creditinvoice_amount = 0.00,
                    });
                    values.GetRaiseDebitNoteAdd_list = GetRaiseDebitNoteAdd;
                }
            }
        }
        public void DaGetRaiseDebitProductSummary(string invoice_gid, MdlPblDebitNote values)
        {

            msSQL = " select h.productuom_name,a.invoicedtl_gid,a.invoice_gid,a.qty_invoice," +
              " format(a.product_price,2)as product_price,a.discount_percentage,format(a.discount_amount,2)as discount_amount, " +
              " format(a.tax_amount,2)as tax_amount,format(a.tax_amount2,2)as tax_amount2 ,format(a.tax_amount3,2)as tax_amount3," +
              "a.tax_name,a.tax_name2,a.tax_name3," +
              " format(((a.product_price*a.qty_invoice)-a.discount_amount+a.tax_amount+a.tax_amount2+a.tax_amount3),2)as price," +
              "a.display_field," +
              " a.product_gid, a.product_code, a.product_name,g.productgroup_gid,a.productgroup_name, " +
              " a.uom_gid" +
              " from acp_trn_tinvoicedtl a " +
              " left join acp_trn_tinvoice e on e.invoice_gid=a.invoice_gid" +
              " left join pmr_mst_tproduct b on b.product_gid=a.product_gid " +
              " left join pmr_mst_tproductgroup g on g.productgroup_gid=b.productgroup_gid " +
              " left join pmr_mst_tproductuom h on h.productuom_gid = a.uom_gid " +
              " where a.invoice_gid='" + invoice_gid + "' ";
            ds_dataset = objdbconn.GetDataSet(msSQL, "acp_trn_tinvoicedtl");
            var GetDebitProduct = new List<GetDebitProduct_list>();
            if (ds_dataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                {
                    GetDebitProduct.Add(new GetDebitProduct_list
                    {
                        invoicedtl_gid = ds["invoicedtl_gid"].ToString(),
                        invoice_gid = ds["invoice_gid"].ToString(),
                        qty_invoice = ds["qty_invoice"].ToString(),
                        product_price = ds["product_price"].ToString(),
                        discount_percentage = ds["discount_percentage"].ToString(),
                        discount_amount = ds["discount_amount"].ToString(),
                        tax_amount = ds["tax_amount"].ToString(),
                        tax_amount2 = ds["tax_amount2"].ToString(),
                        tax_amount3 = ds["tax_amount3"].ToString(),
                        tax_name = ds["tax_name"].ToString(),
                        tax_name2 = ds["tax_name2"].ToString(),
                        tax_name3 = ds["tax_name3"].ToString(),
                        price = ds["price"].ToString(),
                        display_field = ds["display_field"].ToString(),
                        product_gid = ds["product_gid"].ToString(),
                        product_code = ds["product_code"].ToString(),
                        productgroup_gid = ds["productgroup_gid"].ToString(),
                        product_name = ds["product_name"].ToString(),
                        productgroup_name = ds["productgroup_name"].ToString(),
                        productuom_name = ds["productuom_name"].ToString(),
                        uom_gid = ds["uom_gid"].ToString(),
                    });
                    values.GetDebitProduct_list = GetDebitProduct;
                }
            }
        }
        public void DaPostDebitNote(string employee_gid, Postdebit_list values)
        {
            DateTime credit_date = DateTime.ParseExact(values.debitnote_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string debitnote_date = credit_date.ToString("yyyy-MM-dd");

            string credit_amount = values.creditinvoice_amount.ToString().Replace(",", "");
            double credit_amount1 = Convert.ToDouble(credit_amount);

            msSQL = " select invoice_amount,invoice_from,invoice_reference from acp_trn_tinvoice where invoice_gid='" + values.invoice_gid + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows == true)
            {
                objOdbcDataReader.Read();
                invoice_amount = double.Parse(objOdbcDataReader["invoice_amount"].ToString());
                lsordergid = objOdbcDataReader["invoice_reference"].ToString();
                lsfrom = objOdbcDataReader["invoice_from"].ToString();
                objOdbcDataReader.Close();
            }
            msGetGID = objcmnfunctions.GetMasterGID("SPYC");
            msPYGetGID = objcmnfunctions.GetMasterGID("SPYC");

            msSQL = " insert into acp_trn_tpaymentdtl (" +
                  " paymentdtl_gid, " +
                  " payment_gid, " +
                  " payment_amount, " +
                  " invoice_amount, " +
                  " advance_amount, " +
                  " invoice_remarks," +
                  " payment_amount_L," +
                  " invoice_Amount_L," +
                  " advance_amount_L" +
                  " )values ( " +
                  "'" + msGetGID + "'," +
                  "'" + msPYGetGID + "'," +
                  "'" + values.creditinvoice_amount.ToString().Replace(",", "") + "'," +
                  "'" + invoice_amount + "', " +
                  "'" + 0.0 + "'," +
                  "'" + values.reasondebit + "'," +
                  "'" + credit_amount1 * values.exchange_rate + "'," +
                  "'" + invoice_amount * values.exchange_rate + "'," +
                  "'" + 0.0 + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                mspGetGID = objcmnfunctions.GetMasterGID("SIPC");

                msSQL = " insert into acp_trn_tinvoice2payment (" +
                    " invoice2payment_gid, " +
                    " payment_gid, " +
                    " paymentdtl_gid, " +
                    " invoice_gid, " +
                    " invoice_amount, " +
                    " advance_amount, " +
                    " payment_amount," +
                    " payment_amount_L," +
                    " invoice_Amount_L," +
                    " advance_amount_L" +
                    " )values ( " +
                    "'" + mspGetGID + "'," +
                    "'" + msPYGetGID + "'," +
                    "'" + msGetGID + "'," +
                    "'" + values.invoice_gid + "'," +
                    "'" + invoice_amount.ToString().Replace(",", "") + "'," +
                    "'" + 0.0 + "'," +
                    "'" + values.creditinvoice_amount.ToString().Replace(",", "") + "'," +
                    "'" + values.creditinvoice_amount.ToString().Replace(",", "") + "'," +
                    "'" + invoice_amount.ToString().Replace(",", "") + "'," +
                    "'" + 0.0 + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            mssalesorderGID = objcmnfunctions.GetMasterGID("VCRN");

            msSQL = "insert into pmr_trn_tdebitnote ( " +
                    "  debitnote_gid, " +
                    " invoice_gid, " +
                    " debit_amount, " +
                    " debit_by, " +
                    " debit_date, " +
                    " payment_gid, " +
                    " created_date, " +
                    " created_by, " +
                    " remarks " +
                    " ) values ( " +
                    " '" + mssalesorderGID + "', " +
                    " '" + values.invoice_gid + "', " +
                    " '" + values.creditinvoice_amount.ToString().Replace(",", "") + "', " +
                    "'" + employee_gid + "'," +
                    " '" + debitnote_date + "'," +
                    " '" + msPYGetGID + "', " +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                    " '" + employee_gid + "'," +
                    " '" + values.reasondebit + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                msSQL = " update acp_trn_tinvoice set debit_note=debit_note+'" + values.creditinvoice_amount.ToString().Replace(",", "") + "', " +
                " debit_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where invoice_gid='" + values.invoice_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //objfncmnfunction.finance_payment("Purchase", values.payment_mode, "", debitnote_date,
                //credit_amount1 * values.exchange_rate, values.branch_gid, "Debit Note", "PMR", values.vendor_gid, values.reasondebit,
                //values.invoice_refno, 0.00, 0.00, credit_amount1 * values.exchange_rate, msPYGetGID);

                msSQL = "select finance_flag from adm_mst_tcompany ";
                string finance_flag = objdbconn.GetExecuteScalar(msSQL);
                if (finance_flag == "Y")
                {
                    string lsinvoiceamount = "", lscustomer_gid = "", lsroundoff = "", lsexchangerate = "", lsinvoicerefno = "", mysqlinvoiceDate = "", sales_type = "", lsbranchgid = "", lstax_name = "";
                    double roundoff = 0, discount_amount = 0, addon_charge = 0, freight_charges = 0, grand_total_l = 0, packing_charges = 0, buyback_charges = 0, insurance_charges = 0, tax_amount = 0;
                    //msSQL = "select invoice_amount_L,payment_amount,invoice_refno,roundoff,exchange_rate,branch_gid,customer_gid,buyback_charges,packing_charges,insurance_charges,tax_name,tax_amount, " +
                    //       " discount_amount_L,additionalcharges_amount_L,frieghtcharges_amount_L,sales_type from rbl_trn_Tinvoice " +
                    msSQL= "  select invoice_gid,vendor_gid,branch_gid, invoice_amount,payment_amount,exchange_rate,invoice_refno,freightcharges_amount,additionalcharges_amount,discount_amount," +
                           "     total_amount,freightcharges,buybackorscrap,round_off,packing_charges,insurance_charges,tax_name,invref_no,purchase_type,tax_amount from acp_trn_tinvoice " +
                           " where invoice_gid='" + values.invoice_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        objOdbcDataReader.Read();
                        grand_total_l = Math.Round(double.Parse(objOdbcDataReader["invoice_amount"].ToString()), 2);
                        lsexchangerate = objOdbcDataReader["exchange_rate"].ToString();
                        lsinvoicerefno = objOdbcDataReader["invoice_refno"].ToString();
                        roundoff = Math.Round(double.Parse(objOdbcDataReader["round_off"].ToString()), 2);
                        discount_amount = Math.Round(double.Parse(objOdbcDataReader["discount_amount"].ToString()), 2);
                        addon_charge = Math.Round(double.Parse(objOdbcDataReader["additionalcharges_amount"].ToString()), 2);
                        freight_charges = Math.Round(double.Parse(objOdbcDataReader["freightcharges_amount"].ToString()), 2);
                        sales_type = objOdbcDataReader["purchase_type"].ToString();
                        lsbranchgid = objOdbcDataReader["branch_gid"].ToString();
                        lscustomer_gid = objOdbcDataReader["vendor_gid"].ToString();
                        lstax_name = objOdbcDataReader["tax_name"].ToString();
                        packing_charges = Math.Round(double.Parse(objOdbcDataReader["packing_charges"].ToString()), 2);
                        buyback_charges = Math.Round(double.Parse(objOdbcDataReader["buybackorscrap"].ToString()), 2);
                        insurance_charges = Math.Round(double.Parse(objOdbcDataReader["insurance_charges"].ToString()), 2);
                        tax_amount = Math.Round(double.Parse(objOdbcDataReader["tax_amount"].ToString()), 2);
                    }
                    objOdbcDataReader.Close();
                    double roundoff1 = roundoff * double.Parse(lsexchangerate);
                    string lsproduct_price_l = "", lstax1_gid = "", lstax2_gid = "";
                    msSQL = "  select sum(product_price) as product_price_L,sum(tax_amount) as tax1,sum(tax_amount2) as tax2,tax1_gid,tax2_gid  from acp_trn_tinvoicedtl" +
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
                    objfncmnfunction.jn_debit_note(debitnote_date, values.reasondebit, lsbranchgid, values.invoice_gid, values.invoice_gid
                                                 ,values.creditinvoice_amount,values.creditinvoice_amount, lscustomer_gid, "Invoice", "PBL",  sales_type);

          

                    if (lstax1 != "0.00" && lstax1 != "" && lstax1 != null && lstax1 != null)
                    {
                        decimal lstaxsum = decimal.Parse(lstax1);
                        string lstaxamount = lstaxsum.ToString("F2");
                        tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                        objfncmnfunction.jn_debit_tax(mssalesorderGID, mssalesorderGID, values.reasondebit, tax_amount, lstax1_gid);
                    }
                    if (lstax2 != "0.00" && lstax2 != "" && lstax2 != null && lstax2 != "0")
                    {
                        decimal lstaxsum = decimal.Parse(lstax2);
                        string lstaxamount = lstaxsum.ToString("F2");
                        tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                        objfncmnfunction.jn_debit_tax(mssalesorderGID, mssalesorderGID, values.reasondebit, tax_amount, lstax2_gid);
                    }



                }


                values.status = true;
                values.message = "Debit note Successfully raised.";
            }
            else
            {
                values.status = false;
                values.message = "Error Occurred while raising the debit note.";
            }
        }
        public void DaGetStockReturnSummary(string invoice_gid, MdlPblDebitNote values)
        {
            msSQL = " select a.invoice_gid,a.invoice_refno,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date," +
                "a.payment_term,date_format(a.payment_date,'%d-%m-%Y')as payment_date," +
                 " format(a.total_amount,2)as total_amount,format(g.debit_amount,2) as debit_note,a.invoice_from," +
                 " date_format(a.debit_date,'%d-%m-%Y')as debit_date,a.invoice_reference," +
                 " format(a.additionalcharges_amount,2)as additionalcharges_amount,format(a.discount_amount,2)as discount_amount ," +
                 " format(a.invoice_amount,2)as invoice_amount,format(sum(e.product_total),2) as price,a.tax_amount,a.tax_percentage," +
                 " a.tax_name, a.freightcharges, a.buybackorscrap," +
                 " c.vendor_companyname,c.contactperson_name,concat(f.address1,',',address2) as vendor_address,a.currency_code," +
                 " c.email_id,c.contact_telephonenumber  as mobile,format((a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding," +
                 "format((a.payment_amount+a.advance_amount+a.debit_note),2) as payment_amount," +
                 " e.product_remarks,a.invoice_remarks,g.remarks from acp_trn_tinvoice a" +
                 " left join acp_mst_tvendor c on a.vendor_gid=c.vendor_gid" +
                 " left join pmr_trn_tdebitnote g on a.invoice_gid=g.invoice_gid" +
                 " left join adm_mst_taddress f on c.address_gid=f.address_gid" +
                 " left join acp_trn_tinvoicedtl e on e.invoice_gid=a.invoice_gid" +
                 " where a.invoice_gid='" + invoice_gid + "' group by a.invoice_gid ";
            ds_dataset = objdbconn.GetDataSet(msSQL, "acp_trn_tinvoice");
            var GetStockReturn = new List<GetStockReturnDebit_list>();
            if (ds_dataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                {
                    GetStockReturn.Add(new GetStockReturnDebit_list
                    {
                        invoice_refno = ds["invoice_refno"].ToString(),
                        invoice_date = ds["invoice_date"].ToString(),
                        invoice_reference = ds["invoice_reference"].ToString(),
                        vendor_companyname = ds["vendor_companyname"].ToString(),
                        contactperson_name = ds["contactperson_name"].ToString(),
                        mobile = ds["mobile"].ToString(),
                        email_id = ds["email_id"].ToString(),
                        vendor_address = ds["vendor_address"].ToString(),
                        remarks = ds["remarks"].ToString(),
                        price = ds["price"].ToString(),
                        currency_code = ds["currency_code"].ToString(),
                        total_amount = ds["total_amount"].ToString(),
                        additionalcharges_amount = ds["additionalcharges_amount"].ToString(),
                        discount_amount = ds["discount_amount"].ToString(),
                        freightcharges = ds["freightcharges"].ToString(),
                        invoice_amount = ds["invoice_amount"].ToString(),
                        payment_amount = ds["payment_amount"].ToString(),
                        outstanding = ds["outstanding"].ToString(),
                        debit_note = ds["debit_note"].ToString(),
                        tax_name = ds["tax_name"].ToString(),
                    });
                    values.GetStockReturnDebit_list = GetStockReturn;
                }
            }
        }
        public void DaGetStockRetrunProductSummary(string invoice_gid, MdlPblDebitNote values)
        {
            msSQL = " select a.invoicedtl_gid,a.invoice_gid,a.qty_invoice," +
              " format(a.product_price,2)as product_price,a.discount_percentage," +
              " format(a.discount_amount,2)as discount_amount, " +
              " format(a.tax_amount,2)as tax_amount,format(a.tax_amount2,2)as tax_amount2 ," +
              " format(a.tax_amount3,2)as tax_amount3,a.tax_name,a.tax_name2,a.tax_name3," +
              " format(a.product_total,2) as product_total,a.display_field," +
              " a.product_gid, a.product_code, a.product_name,a.productgroup_name,a.productuom_name, " +
              " a.productuom_name,a.uom_gid" +
              " from acp_trn_tinvoicedtl a " +
              " where a.invoice_gid='" + invoice_gid + "' ";
            ds_dataset = objdbconn.GetDataSet(msSQL, "acp_trn_tinvoicedtl");
            var GetStockReturnproduct = new List<GetStockReturnproduct_list>();
            if (ds_dataset.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow ds in ds_dataset.Tables[0].Rows)
                {
                    GetStockReturnproduct.Add(new GetStockReturnproduct_list
                    {
                        invoicedtl_gid = ds["invoicedtl_gid"].ToString(),
                        invoice_gid = ds["invoice_gid"].ToString(),
                        qty_invoice = ds["qty_invoice"].ToString(),
                        discount_percentage = ds["discount_percentage"].ToString(),
                        product_price = ds["product_price"].ToString(),
                        discount_amount = ds["discount_amount"].ToString(),
                        tax_amount = ds["tax_amount"].ToString(),
                        tax_amount2 = ds["tax_amount2"].ToString(),
                        tax_amount3 = ds["tax_amount3"].ToString(),
                        tax_name = ds["tax_name"].ToString(),
                        tax_name2 = ds["tax_name2"].ToString(),
                        tax_name3 = ds["tax_name3"].ToString(),
                        product_total = ds["product_total"].ToString(),
                        display_field = ds["display_field"].ToString(),
                        product_gid = ds["product_gid"].ToString(),
                        product_code = ds["product_code"].ToString(),
                        product_name = ds["product_name"].ToString(),
                        productgroup_name = ds["productgroup_name"].ToString(),
                        productuom_name = ds["productuom_name"].ToString(),
                        uom_gid = ds["uom_gid"].ToString(),
                        stockreturn_qty = 0.00,
                    });
                    values.GetStockReturnproduct_list = GetStockReturnproduct;
                }
            }
        }
        public void DaPostStockReturnDebit(string employee_gid, PostReturnStockDebit_list values)
        {

            for (int i = 0; i < values.GetStockReturnproduct_list.Length; i++)
            {
                msstockreturn = objcmnfunctions.GetMasterGID("SRCR");

                msSQL = " insert into pbl_trn_tdebitnotedtl ( " +
                    " debitnotedtl_gid, " +
                    " debitnote_gid, " +
                    " stock_return, " +
                    " product_price, " +
                    " invoicedtl_gid, " +
                    " created_by, " +
                    " created_date " +
                    " ) values ( " +
                     " '" + msstockreturn + "', " +
                     " '" + values.debitnote_gid + "', " +
                     " '" + values.GetStockReturnproduct_list[i].stockreturn_qty + "', " +
                     " '" + values.GetStockReturnproduct_list[i].product_price + "', " +
                     " '" + values.GetStockReturnproduct_list[i].invoicedtl_gid + "', " +
                     "'" + employee_gid + "', " +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "') ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select  branch_gid from acp_trn_tinvoice  where invoice_gid ='" + values.GetStockReturnproduct_list[i].invoice_gid + "' ";
                lsbranch = objdbconn.GetExecuteScalar(msSQL);

                if (mnResult == 1)
                {
                    msSQL = "SELECT (stock_qty + amend_qty - issued_qty - damaged_qty - transfer_qty) AS stock_qty, " +
                                "damaged_qty, stock_gid " +
                                "FROM ims_trn_tstock " +
                                "WHERE (stock_qty > amend_qty) AND product_gid = '" + values.GetStockReturnproduct_list[i].product_gid + "' " +
                                "AND branch_gid = '" + lsbranch + "' AND stock_flag = 'Y'";


                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    List<int> quantities = new List<int>();
                    int qtyrest = Convert.ToInt32(values.GetStockReturnproduct_list[i].stockreturn_qty);

                    while (objOdbcDataReader.Read())
                    {
                        int stockQty = Convert.ToInt32(objOdbcDataReader["stock_qty"]);
                        int quantityForThisRow = Math.Min(qtyrest, stockQty);

                        quantities.Add(quantityForThisRow);
                        qtyrest -= quantityForThisRow;

                        if (qtyrest <= 0)
                        {
                            break; // Exit the loop if the requested quantity has been fully accounted for
                        }
                    }

                    objOdbcDataReader.Close();

                    // Step 2: Update the Rows with Calculated Quantities
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL); // Reset the data reader to the beginning
                    int index = 0;

                    while (objOdbcDataReader.Read() && index < quantities.Count)
                    {
                        int issueQty = Convert.ToInt32(objOdbcDataReader["damaged_qty"]);
                        string stockgid = objOdbcDataReader["stock_gid"].ToString();
                        int quantityForThisRow = quantities[index];
                        index++;

                                   msSQL = "UPDATE ims_trn_tstock " +
                                    "SET damaged_qty = " + (issueQty + quantityForThisRow) + " " +
                                    "WHERE product_gid = '" + values.GetStockReturnproduct_list[i].product_gid + "' " +
                                    "AND branch_gid = '" + lsbranch + "' " +
                                    "AND stock_gid = '" + stockgid + "'";
                       

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }

                    objOdbcDataReader.Close();



                }


                msSQL = " select  debit_note from acp_trn_tinvoice  where invoice_gid ='" + values.GetStockReturnproduct_list[i].invoice_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    double lsdebit_note = double.Parse(objOdbcDataReader["debit_note"].ToString());



                    msSQL = " UPDATE acp_trn_tinvoice SET " +
                            " debit_note = '" + lsdebit_note + values.GetStockReturnproduct_list[i].stockreturn_qty + "', " +
                            " debit_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                            " WHERE invoice_gid = '" + values.GetStockReturnproduct_list[i].invoice_gid + "'; ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                }



                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Stock Return Successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while Stock Return.";
                }
            }
            
        }
        public void DaDeleteDebitNote(string debitnote_gid, string payment_gid, MdlPblDebitNote values)
        {
            msSQL = "select debit_amount,invoice_gid from pmr_trn_tdebitnote where debitnote_gid='" + debitnote_gid + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows == true)
            {
                objOdbcDataReader.Read();
                debit_amount = Convert.ToUInt32(objOdbcDataReader["debit_amount"].ToString());
                invoice_gid = objOdbcDataReader["invoice_gid"].ToString();
                objOdbcDataReader.Close ();
            }
            msSQL = " update acp_trn_tinvoice set debit_note=debit_note-'" + debit_amount + "', " +
           " debit_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'   where invoice_gid='" + invoice_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                msSQL = "delete from pmr_trn_tdebitnote where debitnote_gid='" + debitnote_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "delete from acp_trn_tpayment where payment_gid='" + payment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            if (mnResult == 1)
            {
                objfncmnfunction.invoice_cancel(payment_gid);
                values.status = true;
                values.message = "Debitnote deleted successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error occured while deleting debitnote";
            }
        }
        public Dictionary<string, object> DaDebitPDF(string debitnote_gid, string invoice_gid, MdlPblDebitNote values) 
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

                msSQL = "select a.invoice_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoicedate,a.invoice_refno,a.invoice_reference,e.debitnote_gid,date_format(e.debit_date,'%d-%m-%Y') as debitdate,e.debit_amount," +
                      " b.vendor_companyname, b.contactperson_name, b.contact_telephonenumber, b.email_id," +
                      " c.address1, c.address2, c.city, c.state, c.postal_code, d.country_name, a.currency_code" +
                      " from pmr_trn_tdebitnote e" +
                      " left join acp_trn_tinvoice a on e.invoice_gid = a.invoice_gid" +
                      " left join acp_mst_tvendor b on a.vendor_gid=b.vendor_gid" +
                      " left join adm_mst_taddress  c on b.address_gid=c.address_gid" +
                      " left join adm_mst_tcountry d on c.country_gid=d.country_gid " +
                      " where e.debitnote_gid= '" + debitnote_gid +  "'";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable1");

                msSQL = " select a.debitnote_gid, b.debitnotedtl_gid,b.stock_return,format(b.product_price,2) as product_price ,c.product_name,c.product_code , (b.stock_return * b.product_price) as total " +
                        " from pmr_trn_tdebitnote a " +
                        " left join  pbl_trn_tdebitnotedtl b on b.debitnote_gid = a.debitnote_gid " +
                        " left join acp_trn_tinvoicedtl c on c.invoicedtl_gid = b.invoicedtl_gid " +
                        " where a.debitnote_gid='" + debitnote_gid + "'";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable2");

                msSQL = " select distinct d.branch_gid,d.address1 as address, d.branch_name,d.city,d.state,d.postal_code,d.branch_logo from acp_trn_tinvoice a " +
                        " inner join hrm_mst_temployee c on c.user_gid=a.user_gid " +
                        " inner join hrm_mst_tbranch d on d.branch_gid=c.branch_gid " +
                        " where a.invoice_gid ='" + invoice_gid + "'";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");

                try
                {
                    ReportDocument oRpt = new ReportDocument();
                    string base_pathOF_currentFILE = AppDomain.CurrentDomain.BaseDirectory;
                    string report_path = Path.Combine(base_pathOF_currentFILE, "ems.pmr", "Reports", "pbl_crp_debitnote.rpt");

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
                    string PDFfile_name = "Debitnote.pdf";
                    full_path = Path.Combine(path, PDFfile_name);

                    oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, full_path);
                    myConnection.Close();
                    ls_response = objFnazurestorage.reportStreamDownload(full_path);
                    values.status = true;
                }
                catch(Exception ex)
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