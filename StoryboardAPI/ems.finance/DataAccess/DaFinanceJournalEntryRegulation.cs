using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;
using System.Threading;
using System.Globalization;

namespace ems.finance.DataAccess
{
    public class DaFinanceJournalEntryRegulation
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, dt_datatable1, dt_customer, dt_tax, dt_accmapping, dt_vendor;
        string msGetGid, lsflag;
        int mnResult;
        string msGetGID, msGetDlGID, msGetDlGID1, msGetDlGID2, msGetSEDlGID;
        string finyear, jounrnal_date, journal_type, reference_gid, lsaccount_gid;
        OdbcDataReader objODBCDataReader;
        string journal_month, journal_year, journal_Day, account_gid, msgetgid = string.Empty;
        List<string> journal_monthandyear = new List<string>();
        string lswthaccount_gid, lsWHTjournal_type, customer_name, journal_gid = string.Empty;
        string lsADaccount_GID, lsADjournal_type = string.Empty;
        string SEjournal_type, LSTransactionCode, LSTransactionType, msGetD3GID, lscustomergid, lsvendor_gid, msGetD2GID,
        journal_date, LSReferenceGID, lsSEAccountname, lsSEaccount_gid = string.Empty;
        string msAccGetGID, lsaccoungroup_gid = string.Empty, LSLedgerType = string.Empty, LSDisplayType = string.Empty, lsreferenceType = string.Empty;
        string acc_subgroup_gid = "", acc_subgroup_name = "", customer_id = "";

        public void DaPostSales1(dateform value)
        {
            var lsdate = DateTime.Parse(value.todate).ToString("yyyy") + "-04-01";
            var to_date = (DateTime.Parse(lsdate).AddYears(1)).ToString("yyyy") + "-03-31";


            List<invoicedtl> Mdl_rblinvoicedtl = new List<invoicedtl>();
            List<Customer> customers = new List<Customer>();
            List<Tax> tax = new List<Tax>();
            List<AccMapping> accmappin = new List<AccMapping>();


            //Delete journal data 

            msSQL1 = " delete from acc_trn_journalentrydtl where journal_gid in (select journal_gid from " +
                    " acc_trn_journalentry where transaction_date>='" + lsdate + "' and transaction_date<='" + to_date + "' and journal_from='Sales' and " +
                    " transaction_gid in(select invoice_gid from rbl_trn_tinvoice where invoice_date>='" + lsdate + "' and invoice_date<='" + to_date + "'))";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
            msSQL1 = "delete from acc_trn_journalentry where transaction_date>='" + lsdate + "' and transaction_date<='" + to_date + "'  and journal_from='Sales'" +
                    " and transaction_gid in(select invoice_gid from rbl_trn_tinvoice where invoice_date>='" + lsdate + "' and invoice_date<='" + to_date + "') ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

            // msSQL = "delete from acc_mst_tchartofaccount where accountgroup_name like 'Sundry Debtor%'";
            //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            //Get customer account_gid
            msSQL1 = "select customer_gid,customer_name,a.taxsegment_gid,b.taxsegment_name,a.customer_code," +
                    " a.account_gid,b.account_name from crm_mst_tcustomer a" +
                    " left join acp_mst_ttaxsegment b on a.taxsegment_gid = b.taxsegment_gid where b.reference_type='Customer'";
            dt_customer = objdbconn.GetDataTable(msSQL1);
            if (dt_customer.Rows.Count <= 0)
            {
                value.message = "Please Assign Customer to TaxSegment";
                value.status = false;
                return;
            }
            else
            {
                customers = cmnfunctions.ConvertDataTable<Customer>(dt_customer);

            }

            //Get Tax account_gid
            msSQL = "select account_gid,tax_gid from acp_mst_ttax where reference_type='Customer'";
            dt_tax = objdbconn.GetDataTable(msSQL);
            if (dt_tax.Rows.Count <= 0)
            {
                value.message = "Please Add Tax Master";
                value.status = false;
                return;
            }
            else
            {
                tax = cmnfunctions.ConvertDataTable<Tax>(dt_tax);
            }

            //Get account mapping
            msSQL = "select account_gid,field_name from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='RBL'";
            dt_accmapping = objdbconn.GetDataTable(msSQL);
            if (dt_accmapping.Rows.Count <= 0)
            {
                value.message = "Please Do Account Mapping";
                value.status = false;
                return;
            }
            else
            {
                accmappin = cmnfunctions.ConvertDataTable<AccMapping>(dt_accmapping);
            }


            msSQL = "select branch_gid,invoice_date,invoice_gid,customer_gid,invoice_refno,invoice_amount," +
                "  sales_type,ifnull(freight_charges,0) as freight_charges ,additionalcharges_amount_L,discount_amount_L,roundoff,ifnull(packing_charges,0) as packing_charges," +
                " ifnull(insurance_charges,0) as insurance_charges,exchange_rate from rbl_trn_tinvoice " +
                " where  invoice_date>='" + lsdate + "' and invoice_date<='" + to_date + "' and " +
                " and invoice_status not in ('Canceled','Invoice Cancelled') order by invoice_date";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count <= 0)
            {
                value.message = "No Invoices are Exist";
                value.status = false;
                return;
            }
            foreach (DataRow dr in dt_datatable.Rows)
            {

                string customer_account_gid = "", salestype_acc_gid = "";
                string tax_account_gid = "";
                double grand_total = 0.00, additionalcharges_amount = 0.00, freight_charges = 0.00, discount_amount = 0.00;
                double roundoff = 0.0, packing_charges = 0.0, insurance_charges = 0.00, lsexchange_rate = 0.00;
                lscustomergid = dr["customer_gid"].ToString();
                jounrnal_date = dr["invoice_date"].ToString();
                grand_total = double.Parse(dr["invoice_amount"].ToString());
                additionalcharges_amount = double.Parse(dr["additionalcharges_amount_L"].ToString());
                freight_charges = double.Parse(dr["freight_charges"].ToString());
                roundoff = double.Parse(dr["roundoff"].ToString());
                packing_charges = double.Parse(dr["packing_charges"].ToString());
                insurance_charges = double.Parse(dr["insurance_charges"].ToString());
                discount_amount = double.Parse(dr["discount_amount_L"].ToString());
                lsexchange_rate = double.Parse(dr["exchange_rate"].ToString());

                journal_function(DateTime.Parse(jounrnal_date.Replace("-", "")));

                journal_year = journal_monthandyear[0];
                journal_month = journal_monthandyear[1];
                journal_Day = journal_monthandyear[2];
                string subgroup_acc_gid = "";
                msSQL = "select account_gid from smr_trn_tsalestype where salestype_gid='" + dr["sales_type"].ToString() + "'";
                salestype_acc_gid = objdbconn.GetExecuteScalar(msSQL);

                lscustomergid = dr["customer_gid"].ToString();
                List<Customer> customer_acc_gid = customers.Where(a => a.customer_gid == lscustomergid).ToList();
                foreach (var dr1 in customer_acc_gid)
                {
                    customer_account_gid = dr1.account_gid;
                    customer_name = dr1.customer_name;
                    customer_id = dr1.customer_code;

                }
                if (customer_account_gid == null || customer_account_gid == "")
                {

                }
                msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
                if (msGetGID == "E")
                {

                }
                msSQL = " Insert into acc_trn_journalentry " +
                    " (journal_gid, " +
                    " journal_refno, " +
                    " transaction_code, " +
                    " transaction_date, " +
                    " invoice_flag," +
                    " remarks, " +
                    " transaction_type," +
                    " reference_type," +
                    " reference_gid," +
                    " transaction_gid, " +
                    " journal_from," +
                    " journal_year, " +
                    " journal_month, " +
                    " journal_day, " +
                    " created_date," +
                    " branch_gid" +
                    " ) values (" +
                    "'" + msGetGID + "'," +
                    "'" + dr["invoice_refno"].ToString() + "'," +
                    "'" + dr["invoice_refno"].ToString() + "'," +
                    "'" + DateTime.Parse(dr["invoice_date"].ToString()).ToString("yyyy-MM-dd") + "'," +
                    "'Y'," +
                    "'" + customer_name + "'," +
                    "'Journal'," +
                    "'" + customer_name + "'," +
                    "'" + dr["customer_gid"].ToString() + "'," +
                    "'" + dr["invoice_gid"].ToString() + "'," +
                    "'Sales'," +
                    "'" + journal_year + "'," +
                    "'" + journal_month + "'," +
                    "'" + journal_Day + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + dr["branch_gid"] + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "'," +
                               "'" + msGetGID + "'," +
                               "'" + customer_account_gid + "'," +
                               "'" + customer_name + "'," +
                               "'dr'," +
                               "'" + grand_total * lsexchange_rate + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                double base_amount = 0.00;
                string lsbase_amount = "0";
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " select sum(product_price * qty_invoice) from rbl_trn_tinvoicedtl " +
                        " where invoice_gid='" + dr["invoice_gid"].ToString() + "'";
                lsbase_amount = objdbconn.GetExecuteScalar(msSQL);
                base_amount = double.Parse(lsbase_amount) * lsexchange_rate;
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "'," +
                               "'" + msGetGID + "'," +
                               "'" + salestype_acc_gid + "'," +
                               "'" + customer_name + "'," +
                               "'cr'," +
                               "'" + base_amount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL1 = " Insert into acc_trn_journalentrydtl " +
                             " (journaldtl_gid, " +
                             " journal_gid, " +
                             " account_gid," +
                             " remarks," +
                             " journal_type," +
                             " transaction_amount)" +
                             " values ";
                var ls = "";
                int i = 0;
                if (additionalcharges_amount != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='RBL' and field_name='Addon Amount' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + customer_name + "'," +
                        "'cr'," +
                        "'" + additionalcharges_amount + "'),";

                }
                if (discount_amount != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='RBL' and field_name='Additional Discount' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + customer_name + "'," +
                        "'dr'," +
                        "'" + discount_amount + "'),";
                }
                if (roundoff != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='RBL' and field_name='Round Off' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + customer_name + "'," +
                        "'cr'," +
                        "'" + roundoff + "'),";
                }
                if (freight_charges != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='RBL' and field_name='Frieght Charges' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + customer_name + "'," +
                        "'cr'," +
                        "'" + freight_charges + "'),";
                }
                if (packing_charges != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='RBL' and field_name='Packing Charges' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + customer_name + "'," +
                        "'cr'," +
                        "'" + packing_charges + "'),";
                }
                if (insurance_charges != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='RBL' and field_name='Insurance' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + customer_name + "'," +
                        "'cr'," +
                        "'" + insurance_charges + "'),";

                }
                if (ls != "")
                {
                    msSQL1 += ls.TrimEnd(',');
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
                }



                foreach (var dr2 in tax)
                {
                    msSQL = " select sum(tax_amount1_L) as tax1 from rbl_trn_tinvoicedtl " +
                        " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                        " and tax1_gid='" + dr2.tax_gid + "'";
                    string tax_amount1 = objdbconn.GetExecuteScalar(msSQL);
                    if (tax_amount1 == null || tax_amount1 == "")
                    {
                        tax_amount1 = "0.00";
                    }
                    msSQL = " select sum(tax_amount2_L) as tax1 from rbl_trn_tinvoicedtl " +
                       " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                       " and tax2_gid='" + dr2.tax_gid + "'";
                    string tax_amount2 = objdbconn.GetExecuteScalar(msSQL);
                    if (tax_amount2 == null || tax_amount2 == "")
                    {
                        tax_amount2 = "0.00";
                    }
                    msSQL = " select sum(tax_amount3_L) as tax1 from rbl_trn_tinvoicedtl " +
                       " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                       " and tax3_gid='" + dr2.tax_gid + "'";
                    string tax_amount3 = objdbconn.GetExecuteScalar(msSQL);
                    if (tax_amount3 == null || tax_amount3 == "")
                    {
                        tax_amount3 = "0.00";
                    }
                    if (tax_amount1 != "0.00" || tax_amount2 != "0.00" || tax_amount3 != "0.00")
                    {
                        double sum_tax = double.Parse(tax_amount1) + double.Parse(tax_amount2) + double.Parse(tax_amount3);
                        string msGetDlGID1 = objcmnfunctions.GetMasterGID_SP("FPCD");
                        msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " remarks," +
                            " journal_type," +
                            " transaction_amount)" +
                            " values (" +
                            "'" + msGetDlGID1 + "', " +
                            "'" + msGetGID + "'," +
                            "'" + dr2.account_gid + "'," +
                            "'" + customer_name + "', " +
                            "'cr'," +
                            "'" + sum_tax + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }


            }

        }

        public void DaPostPurchase1(dateform value)
        {
            //var lsdate = DateTime.Parse(value.todate).ToString("yyyy-MM-dd");

            var lsdate = DateTime.Parse(value.todate).ToString("yyyy") + "-04-01";
            var to_date = (DateTime.Parse(lsdate).AddYears(1)).ToString("yyyy") + "-03-31";


            List<invoicedtl> Mdl_rblinvoicedtl = new List<invoicedtl>();
            List<Vendor> vendors = new List<Vendor>();
            List<Tax> tax = new List<Tax>();
            List<AccMapping> accmappin = new List<AccMapping>();

            //Delete journal data 

            msSQL1 = " delete from acc_trn_journalentrydtl where journal_gid in (select journal_gid from " +
                    " acc_trn_journalentry where   " +
                    " transaction_gid in(select invoice_gid from acp_trn_tinvoice where invoice_date >= '" + lsdate + "' and invoice_date <= '" + to_date + "'))";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
            msSQL1 = "delete from acc_trn_journalentry where " +
                      " transaction_gid in(select invoice_gid from acp_trn_tinvoice where invoice_date>='" + lsdate + "' and invoice_date <='" + to_date + "') ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);



            //Get vendor account_gid
            msSQL1 = "select vendor_companyname ,a.taxsegment_gid,b.taxsegment_name,a.vendor_code,a.vendor_gid, " +
                " a.account_gid,b.account_name from acp_mst_tvendor a " +
                " left join acp_mst_ttaxsegment b on b.taxsegment_gid=a.taxsegment_gid where reference_type='Vendor'";
            dt_vendor = objdbconn.GetDataTable(msSQL1);
            vendors = cmnfunctions.ConvertDataTable<Vendor>(dt_vendor);

            //Get Tax account_gid
            msSQL = "select account_gid,tax_gid,tax_name from acp_mst_ttax where reference_type='Vendor'";
            dt_tax = objdbconn.GetDataTable(msSQL);
            tax = cmnfunctions.ConvertDataTable<Tax>(dt_tax);
            //Get account mapping
            msSQL = "select account_gid,field_name from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='PMR'";
            dt_accmapping = objdbconn.GetDataTable(msSQL);
            accmappin = cmnfunctions.ConvertDataTable<AccMapping>(dt_accmapping);

            msSQL = "select branch_gid,invoice_date,invoice_gid,vendor_gid,invoice_refno,invoice_amount," +
                "  purchase_type,freightcharges ,additionalcharges_amount,discount_amount,exchange_rate ," +
                "  ifnull(round_off,0) as roundoff,packing_charges,ifnull(Tax_amount,0) as overall_tax,tax_gid,tax_name ," +
                " insurance_charges from acp_trn_tinvoice where invoice_date>='" + lsdate + "' and invoice_date <='" + to_date + "'" +
                " and invoice_status<>'IV Canceled'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            foreach (DataRow dr in dt_datatable.Rows)
            {

                string vendor_account_gid = "", purchasetype_acc_gid = "", vendor_account_name = "";
                string tax_account_gid = "", vendor_name = "", vendor_code = "";
                double grand_total = 0.00, additionalcharges_amount = 0.00, freight_charges = 0.00, discount_amount = 0.00;
                double roundoff = 0.0, packing_charges = 0.0, insurance_charges = 0.00, exchange_rate = 0.0, overall_tax_amount = 0.00;
                lsvendor_gid = dr["vendor_gid"].ToString();
                jounrnal_date = dr["invoice_date"].ToString();
                grand_total = double.Parse(dr["invoice_amount"].ToString());
                additionalcharges_amount = double.Parse(dr["additionalcharges_amount"].ToString());
                freight_charges = double.Parse(dr["freightcharges"].ToString());
                roundoff = double.Parse(dr["roundoff"].ToString());
                packing_charges = double.Parse(dr["packing_charges"].ToString());
                insurance_charges = double.Parse(dr["insurance_charges"].ToString());
                overall_tax_amount = double.Parse(dr["overall_tax"].ToString());
                exchange_rate = double.Parse(dr["exchange_rate"].ToString());

                journal_function(DateTime.Parse(jounrnal_date.Replace("-", "")));

                journal_year = journal_monthandyear[0];
                journal_month = journal_monthandyear[1];
                journal_Day = journal_monthandyear[2];

                msSQL = "select account_gid from pmr_trn_tpurchasetype where purchasetype_gid='" + dr["purchase_type"].ToString() + "'";
                purchasetype_acc_gid = objdbconn.GetExecuteScalar(msSQL);

                List<Vendor> vendor_acc_gid = vendors.Where(a => a.vendor_gid == lsvendor_gid).ToList();
                foreach (var dr1 in vendor_acc_gid)
                {
                    vendor_account_gid = dr1.account_gid;
                    vendor_name = dr1.vendor_companyname;
                    vendor_code = dr1.vendor_code;
                    vendor_account_name = dr1.account_name;

                }




                msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
                if (msGetGID == "E")
                {
                    value.message = "Error whiles Genearting Squence Code";
                    value.status = false;
                    break;
                }
                msSQL = " Insert into acc_trn_journalentry " +
                    " (journal_gid, " +
                    " journal_refno, " +
                    " transaction_code, " +
                    " transaction_date, " +
                    " invoice_flag," +
                    " remarks, " +
                    " transaction_type," +
                    " reference_type," +
                    " reference_gid," +
                    " transaction_gid, " +
                    " journal_from," +
                    " journal_year, " +
                    " journal_month, " +
                    " journal_day, " +
                    " branch_gid" +
                    " ) values (" +
                    "'" + msGetGID + "'," +
                    "'" + dr["invoice_refno"].ToString() + "'," +
                    "'" + dr["invoice_refno"].ToString() + "'," +
                    "'" + DateTime.Parse(dr["invoice_date"].ToString()).ToString("yyyy-MM-dd") + "'," +
                    "'Y'," +
                    "'" + vendor_name + "'," +
                    "'Journal'," +
                    "'" + vendor_name + "'," +
                    "'" + dr["vendor_gid"].ToString() + "'," +
                    "'" + dr["invoice_gid"].ToString() + "'," +
                    "'Purchase'," +
                    "'" + journal_year + "'," +
                    "'" + journal_month + "'," +
                    "'" + journal_Day + "'," +
                    "'" + dr["branch_gid"] + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "'," +
                               "'" + msGetGID + "'," +
                               "'" + vendor_account_gid + "'," +
                               "'" + vendor_name + "'," +
                               "'cr'," +
                               "'" + grand_total + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                double base_amount = 0.00;
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " select sum(((product_price * qty_invoice) - (discount_amount))) from acp_trn_tinvoicedtl " +
                        " where invoice_gid='" + dr["invoice_gid"].ToString() + "'";
                string lsbase_amount = objdbconn.GetExecuteScalar(msSQL);
                base_amount = double.Parse(lsbase_amount) * exchange_rate;
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "'," +
                               "'" + msGetGID + "'," +
                               "'" + purchasetype_acc_gid + "'," +
                               "'" + vendor_name + "'," +
                               "'dr'," +
                               "'" + base_amount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL1 = " Insert into acc_trn_journalentrydtl " +
                             " (journaldtl_gid, " +
                             " journal_gid, " +
                             " account_gid," +
                             " remarks," +
                             " journal_type," +
                             " transaction_amount)" +
                             " values ";
                var ls = "";
                int i = 0;
                if (additionalcharges_amount != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='PMR' and field_name='Addon Amount' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + vendor_name + "'," +
                        "'dr'," +
                        "'" + additionalcharges_amount + "'),";

                }
                if (discount_amount != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='PMR' and field_name='Additional Discount' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + vendor_name + "'," +
                        "'cr'," +
                        "'" + discount_amount + "'),";
                }
                if (roundoff != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='PMR' and field_name='Round Off' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + vendor_name + "'," +
                        "'dr'," +
                        "'" + roundoff + "'),";
                }
                if (freight_charges != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='PMR' and field_name='Frieght Charges' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + vendor_name + "'," +
                        "'dr'," +
                        "'" + freight_charges + "'),";
                }
                if (packing_charges != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='PMR' and field_name='Packing Charges' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + vendor_name + "'," +
                        "'cr'," +
                        "'" + packing_charges + "'),";
                }
                if (insurance_charges != 0)
                {
                    i += 1;
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='PMR' and field_name='Insurance' ";
                    string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                    ls += "('" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid2 + "'," +
                        "'" + vendor_name + "'," +
                        "'cr'," +
                        "'" + insurance_charges + "'),";

                }
                if (ls != "" || ls != null)
                {
                    msSQL1 += ls.TrimEnd(',');
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
                }

                string overall_tax_accgid = "";
                var lsoveralltax_gid = dr["tax_gid"].ToString();
                if (overall_tax_amount > 0)
                {
                    List<Tax> overalltax = tax.Where(a => a.tax_gid == lsoveralltax_gid).ToList();
                    foreach (var tx in overalltax)
                    {
                        overall_tax_accgid = tx.account_gid;
                    }

                    msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                                   " (journaldtl_gid, " +
                                   " journal_gid, " +
                                   " account_gid," +
                                   " remarks," +
                                   " journal_type," +
                                   " transaction_amount)" +
                                   " values (" +
                                   "'" + msGetDlGID + "', " +
                                   "'" + msGetGID + "'," +
                                   "'" + overall_tax_accgid + "'," +
                                   "'" + dr["tax_name"].ToString() + "', " +
                                   "'dr'," +
                                   "'" + overall_tax_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
                foreach (var dr2 in tax)
                {
                    string tax_name = dr2.tax_name;

                    msSQL = " select sum(tax_amount1_L) as tax1 from acp_trn_tinvoicedtl " +
                        " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                        " and tax1_gid='" + dr2.tax_gid + "'";
                    string tax_amount1 = objdbconn.GetExecuteScalar(msSQL);
                    if (tax_amount1 == null || tax_amount1 == "")
                    {
                        tax_amount1 = "0.00";
                    }
                    msSQL = " select sum(tax_amount2_L) as tax1 from acp_trn_tinvoicedtl " +
                       " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                       " and tax2_gid='" + dr2.tax_gid + "'";
                    string tax_amount2 = objdbconn.GetExecuteScalar(msSQL);
                    if (tax_amount2 == null || tax_amount2 == "")
                    {
                        tax_amount2 = "0.00";
                    }
                    msSQL = " select sum(tax_amount3_L) as tax1 from acp_trn_tinvoicedtl " +
                       " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                       " and tax3_gid='" + dr2.tax_gid + "'";
                    string tax_amount3 = objdbconn.GetExecuteScalar(msSQL);
                    if (tax_amount3 == null || tax_amount3 == "")
                    {
                        tax_amount3 = "0.00";
                    }
                    if (tax_amount1 != "0.00" || tax_amount2 != "0.00" || tax_amount3 != "0.00")
                    {
                        double sum_tax = double.Parse(tax_amount1) + double.Parse(tax_amount2) + double.Parse(tax_amount3);
                        string msGetDlGID1 = objcmnfunctions.GetMasterGID_SP("FPCD");
                        msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " remarks," +
                            " journal_type," +
                            " transaction_amount)" +
                            " values (" +
                            "'" + msGetDlGID1 + "', " +
                            "'" + msGetGID + "'," +
                            "'" + dr2.account_gid + "'," +
                            "'" + vendor_name + "', " +
                            "'dr'," +
                            "'" + sum_tax + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }


            }
            if (mnResult == 1)
            {
                value.message = "New journals Created";
                value.status = true;
            }
            else
            {
                value.message = "No journals Created";
                value.status = true;
            }


        }

        public void DaPostSalesReceipt1(dateform value)
        {

            //var lsdate = DateTime.Parse(value.todate).ToString("yyyy-MM-dd");
            //Delete journal data 

            var lsdate = DateTime.Parse(value.todate).ToString("yyyy") + "-04-01";
            var to_date = (DateTime.Parse(lsdate).AddYears(1)).ToString("yyyy") + "-03-31";

            msSQL1 = " delete from acc_trn_journalentrydtl where journal_gid in (select journal_gid from " +
                    " acc_trn_journalentry where transaction_date>='" + lsdate + "' and transaction_date <='" + to_date + "' and " +
                    " transaction_gid in(select payment_gid from rbl_trn_tpayment where " +
                    " payment_date>='" + lsdate + "' and payment_date <='" + to_date + "'))";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
            msSQL1 = "delete from acc_trn_journalentry where transaction_date>='" + lsdate + "' and transaction_date<='" + to_date + "'" +
                     " and transaction_gid in(select payment_gid from rbl_trn_tpayment where payment_date>='" + lsdate + "' and payment_date <='" + to_date + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);


            string ls_Chartofacc_gid = "", lspayment_mode = "", lsbank_gid = "";
            double lstotal_amount = 0.00, lspaid_amount = 0.00, lstds_amount = 0.00, lsadjustment_amount = 0.00;
            msSQL = "select payment_gid,payment_mode,a.payment_date ,payment_type,dbank_gid,branch_gid,c.account_gid," +
                   " tds_amount_L,adjust_amount_L,bank_charge,payment_remarks," +
                   " format((a.amount_L + a.bank_charge + a.exchange_gain-a.exchange_loss + a.adjust_amount_L   ),2) " +
                   " as total_amount,exchange_gain,exchange_loss,c.account_gid,b.customer_gid from rbl_trn_tpayment a" +
                   " left join rbl_trn_tinvoice b on a.invoice_gid=b.invoice_gid " +
                   " left join crm_mst_tcustomer c on c.customer_gid=b.customer_gid " +
                   " where approval_status in('Payment Done','Partially Paid') and a.payment_date>='" + lsdate + "' and a.payment_date <='" + to_date + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count <= 0)
            {
                value.message = "No Payments are available to post journal";
                value.status = false;
                return;
            }
            foreach (DataRow dr in dt_datatable.Rows)
            {
                lsaccount_gid = dr["account_gid"].ToString();
                jounrnal_date = dr["payment_date"].ToString();
                journal_function(DateTime.Parse(jounrnal_date.Replace("-", "")));
                lspayment_mode = dr["payment_mode"].ToString();
                lsbank_gid = dr["dbank_gid"].ToString();

                journal_year = journal_monthandyear[0];
                journal_month = journal_monthandyear[1];
                journal_Day = journal_monthandyear[2];
                lstds_amount = double.Parse(dr["tds_amount_L"].ToString());
                lsadjustment_amount = double.Parse(dr["adjust_amount_L"].ToString());
                lstotal_amount = (double.Parse(dr["total_amount"].ToString()) + double.Parse(dr["exchange_gain"].ToString()) - double.Parse(dr["exchange_loss"].ToString())) - lstds_amount;
                lspaid_amount = double.Parse(dr["total_amount"].ToString()) + double.Parse(dr["bank_charge"].ToString()) + double.Parse(dr["adjust_amount_L"].ToString());
                //With Hold Tax
                string lsWHTaccount_GID = "", lsWHTjournal_type = "";
                string lsscreen = "";
                lsWHTjournal_type = "cr";
                journal_type = "cr";
                string msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
                string msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                string msGetDlGID1 = objcmnfunctions.GetMasterGID_SP("FPCD");
                string msGetDlGID2 = objcmnfunctions.GetMasterGID_SP("FPCD");

                lsscreen = "Receipt";
                if(lstds_amount > 0)
                {
                    msSQL = " select a.account_gid from acc_mst_accountmapping a" +
                 " left join acc_mst_tchartofaccount b on b.account_gid=a.account_gid" +
                 " where a.screen_name='" + lsscreen + "' and a.module_name='RBL' and a.field_name='With Hold Tax' ";
                    ls_Chartofacc_gid = objdbconn.GetExecuteScalar(msSQL);
                    if (ls_Chartofacc_gid != "")
                    {
                        msSQL = " Insert into acc_trn_journalentrydtl " +
                                  " (journaldtl_gid, " +
                                  " journal_gid, " +
                                  " account_gid," +
                                  " journal_type," +
                                  " remarks, " +
                                  " transaction_amount) " +
                                  " values (" +
                                  "'" + msGetDlGID1 + "', " +
                                  "'" + msGetGID + "'," +
                                  "'" + ls_Chartofacc_gid + "'," +
                                  "'dr'," +
                                  "'" + dr["payment_remarks"].ToString() + "'," +
                                  "'" + lstds_amount + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }

                //Adjust Amount
                if (lsadjustment_amount > 0)
                {
                    ls_Chartofacc_gid = "";
                    msSQL = " select a.account_gid from acc_mst_accountmapping a" +
                     " left join acc_mst_tchartofaccount b on b.account_gid=a.account_gid" +
                     " where a.screen_name='" + lsscreen + "' and a.module_name='RBL' and a.field_name='Adjustment Amount' ";
                    ls_Chartofacc_gid = objdbconn.GetExecuteScalar(msSQL);
                    if (ls_Chartofacc_gid != "")
                    {
                        msSQL = " Insert into acc_trn_journalentrydtl " +
                                  " (journaldtl_gid, " +
                                  " journal_gid, " +
                                  " account_gid," +
                                  " journal_type," +
                                  " remarks, " +
                                  " transaction_amount) " +
                                  " values (" +
                                  "'" + msGetDlGID2 + "', " +
                                  "'" + msGetGID + "'," +
                                  "'" + ls_Chartofacc_gid + "'," +
                                  "'dr'," +
                                  "'" + dr["payment_remarks"].ToString() + "'," +
                                  "'" + lsadjustment_amount + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                
                //alternate type change event
                string SEjournal_type = "dr";


                if (lspayment_mode == "Cash")
                {
                    LSTransactionCode = "CC001";
                    LSTransactionType = "Cash Book";
                    LSReferenceGID = dr["branch_gid"].ToString();
                }
                else
                {
                    LSTransactionType = "Bank Book";
                    LSReferenceGID = dr["branch_gid"].ToString();
                }
                string msGetSEDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

                if (lsbank_gid == "--Select--")
                {
                    lsSEaccount_gid = "FCOA1404070080";
                    lsSEAccountname = "CASH ON HAND";
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " transaction_gid," +
                        " journal_type," +
                        " remarks, " +
                        " transaction_amount) " +
                        " values (" +
                        "'" + msGetSEDlGID + "', " +
                        "'" + msGetGID + "'," +
                        "'" + lsSEaccount_gid + "'," +
                        "'" + lsaccount_gid + "'," +
                        "'" + SEjournal_type + "'," +
                        "'" + dr["payment_remarks"].ToString().Trim() + "', " +
                        "'" + lstotal_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = "select account_gid,bank_code,bank_name from acc_mst_tbank where bank_gid='" + lsbank_gid + "'";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {
                        lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                        LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                        lsSEAccountname = objODBCDataReader["bank_name"].ToString();
                    }
                    objODBCDataReader.Close();
                    //msSQL = "select account_gid,bank_code,bank_name from acc_mst_tcreditcard where bank_gid='" + lsbank_gid + "'";
                    //objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    //if (objODBCDataReader.HasRows == true)
                    //{
                    //    lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                    //    LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                    //    lsSEAccountname = objODBCDataReader["bank_name"].ToString();
                    //}
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " transaction_gid," +
                            " journal_type," +
                            " remarks, " +
                            " transaction_amount) " +
                            " values (" +
                            "'" + msGetSEDlGID + "', " +
                            "'" + msGetGID + "'," +
                            "'" + lsSEaccount_gid + "'," +
                            "'" + lsaccount_gid + "'," +
                            "'" + SEjournal_type + "'," +
                            "'" + dr["payment_remarks"].ToString() + "', " +
                            "'" + lstotal_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                msSQL = " Insert into acc_trn_journalentrydtl " +
                    " (journaldtl_gid, " +
                    " journal_gid, " +
                    " account_gid," +
                    " transaction_gid," +
                    " journal_type," +
                    " remarks, " +
                    " transaction_amount) " +
                    " values (" +
                    "'" + msGetDlGID + "', " +
                    "'" + msGetGID + "'," +
                    "'" + lsaccount_gid + "'," +
                    "'" + lsSEaccount_gid + "'," +
                    "'" + journal_type + "'," +
                    "'" + dr["payment_remarks"].ToString() + "', " +
                    "'" + lspaid_amount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //---------------------Record for main table insert-----------------------
                msSQL = " Insert into acc_trn_journalentry " +
                  " (journal_gid, " +
                  " transaction_date, " +
                  " remarks, " +
                  " transaction_type," +
                  " branch_gid," +
                  " reference_type, " +
                  " reference_gid, " +
                  " journal_from," +
                  " transaction_code, " +
                  " transaction_gid, " +
                  " journal_year, " +
                  " journal_month, " +
                  " journal_day, " +
                  " journal_refno)" +
                  " values (" +
                  "'" + msGetGID + "', " +
                  "'" + DateTime.Parse(dr["payment_date"].ToString()).ToString("yyyy-MM-dd") + "', " +
                  "'" + dr["payment_remarks"].ToString() + "'," +
                  "'" + LSTransactionType + "'," +
                  "'" + dr["branch_gid"].ToString() + "', " +
                  "'" + lsSEAccountname + "', " +
                  "'" + LSReferenceGID + "', " +
                  "'Receipt'," +
                  "'" + LSTransactionCode + "'," +
                  "'" + dr["payment_gid"].ToString() + "', " +
                  "'" + journal_year + "', " +
                  "'" + journal_month + "', " +
                  "'" + journal_Day + "', " +
                  "'" + dr["payment_gid"].ToString() + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


            }


        }
        public void DaPostPurchasePayment1(dateform value)
        {
            //var lsdate = DateTime.Parse(value.todate).ToString("yyyy-MM-dd");

            var lsdate = DateTime.Parse(value.todate).ToString("yyyy") + "-04-01";
            var to_date = (DateTime.Parse(lsdate).AddYears(1)).ToString("yyyy") + "-03-31";

            string msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
            string msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            string msGetDlGID1 = objcmnfunctions.GetMasterGID_SP("FPCD");
            string msGetDlGID2 = objcmnfunctions.GetMasterGID_SP("FPCD");
            string lspayment_mode = "", lspaymet_date = "";
            string lsbank_gid = "", ls_Chartofacc_gid = "";
            //Delete journal data 
            string payment_type = "Purchase";
            string lsexchangeratevalue = "", lspayment_amount = "", lstds_amount = "", lscal_tds = "", lsbranch_gid = "";
            double lsexchage_rate = 0.0, lspayment_amount_L = 0.0, lstds_amount_L = 0.00, lscal_tds_L = 0.00;
            double additional = 0.0, lsadjustment_amount = 0.00, lspaid_amount = 0.00;


            msSQL1 = " delete from acc_trn_journalentrydtl where journal_gid in (select journal_gid from " +
                    " acc_trn_journalentry where " +
                    " transaction_gid in(select payment_gid from acp_trn_tpayment where payment_date>='" + lsdate + "' and payment_date <='" + to_date + "'))";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
            msSQL1 = "delete from acc_trn_journalentry where " +
                     "  transaction_gid in(select payment_gid from acp_trn_tpayment where payment_date>='" + lsdate + "' and payment_date <= '" + to_date + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

            msSQL = "SELECT payment_gid, payment_date, vendor_gid, payment_remarks, " +
                    " payment_mode, bank_name, branch_name, cheque_no, city_name, dd_no, " +
                    " case when currency_code is null then 'INR' else currency_code end as currency_code," +
                    " case when exchange_rate is null then '1' else exchange_rate end as exchange_rate," +
                    " format(payment_total,2) as payment_total, payment_reference,tds_amount,bank_gid," +
                    " tdscalculated_finalamount,payment_from,purchaseorder_gid as reference_gid,addon_amount,additional_discount" +
                    " FROM acp_trn_tpayment where payment_date>='" + lsdate + "' and payment_date <='" + to_date + "' AND payment_status<> 'PY Canceled'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            foreach (DataRow dr in dt_datatable.Rows)
            {
                lspayment_mode = dr["payment_mode"].ToString();
                lsbank_gid = dr["bank_gid"].ToString();
                lspaymet_date = dr["payment_date"].ToString();
                jounrnal_date = dr["payment_date"].ToString();
                lsexchage_rate = double.Parse(dr["exchange_rate"].ToString());
                lspayment_amount = dr["payment_total"].ToString();
                lstds_amount = dr["tds_amount"].ToString();
                lscal_tds = dr["tdscalculated_finalamount"].ToString();

                lspayment_amount_L = double.Parse(lspayment_amount.ToString()) * lsexchage_rate;
                lstds_amount_L = double.Parse(lstds_amount) * lsexchage_rate;
                //lscal_tds_L = double.Parse(lscal_tds) * lsexchage_rate;
                lspaid_amount = lspayment_amount_L - lstds_amount_L;
                //journal_date = DateTime.Parse(lspaymet_date.Replace("-", "")).ToString("yyyyMMdd");
                //journal_function(DateTime.Parse(lspaymet_date.Replace("-","")));
                journal_function(DateTime.Parse(jounrnal_date.Replace("-", "")));
                journal_year = journal_monthandyear[0];
                journal_month = journal_monthandyear[1];
                journal_Day = journal_monthandyear[2];

                string lsWHTaccount_GID = "", lsWHTjournal_type = "";
                string lsscreen = "";
                lsWHTjournal_type = "dr";
                journal_type = "dr";
                msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msGetDlGID1 = objcmnfunctions.GetMasterGID_SP("FPCD");
                msGetDlGID2 = objcmnfunctions.GetMasterGID_SP("FPCD");

                lsscreen = "Payment";
                msSQL = " select a.account_gid from acc_mst_accountmapping a" +
                  " left join acc_mst_tchartofaccount b on b.account_gid=a.account_gid" +
                  " where a.screen_name='" + lsscreen + "' and a.module_name='PMR' and a.field_name='With Hold Tax' ";
                ls_Chartofacc_gid = objdbconn.GetExecuteScalar(msSQL);
                if (ls_Chartofacc_gid != "")
                {
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                              " (journaldtl_gid, " +
                              " journal_gid, " +
                              " account_gid," +
                              " journal_type," +
                              " remarks, " +
                              " transaction_amount) " +
                              " values (" +
                              "'" + msGetDlGID1 + "', " +
                              "'" + msGetGID + "'," +
                              "'" + ls_Chartofacc_gid + "'," +
                              "'cr'," +
                              "'" + dr["payment_remarks"].ToString() + "'," +
                              "'" + lstds_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                //Adjust Amount
                ls_Chartofacc_gid = "";
                msSQL = " select a.account_gid from acc_mst_accountmapping a" +
                 " left join acc_mst_tchartofaccount b on b.account_gid=a.account_gid" +
                 " where a.screen_name='" + lsscreen + "' and a.module_name='PMR' and a.field_name='Adjustment Amount' ";
                ls_Chartofacc_gid = objdbconn.GetExecuteScalar(msSQL);
                if (ls_Chartofacc_gid != "")
                {
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                              " (journaldtl_gid, " +
                              " journal_gid, " +
                              " account_gid," +
                              " journal_type," +
                              " remarks, " +
                              " transaction_amount) " +
                              " values (" +
                              "'" + msGetDlGID2 + "', " +
                              "'" + msGetGID + "'," +
                              "'" + ls_Chartofacc_gid + "'," +
                              "'cr'," +
                              "'" + dr["payment_remarks"].ToString() + "'," +
                              "'" + lsadjustment_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }


                msSQL = " select account_gid from acp_mst_tvendor where vendor_gid='" + dr["vendor_gid"].ToString() + "'";
                lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                //alternate type change event
                string SEjournal_type = "cr";


                if (lspayment_mode == "Cash")
                {
                    LSTransactionCode = "CC001";
                    LSTransactionType = "Cash Book";
                    LSReferenceGID = dr["branch_name"].ToString();
                }
                else
                {
                    LSTransactionType = "Bank Book";
                    LSReferenceGID = dr["branch_name"].ToString();
                }
                string msGetSEDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

                if (lsbank_gid == "--Select--")
                {
                    lsSEaccount_gid = "FCOA1404070080";
                    lsSEAccountname = "CASH ON HAND";
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " transaction_gid," +
                        " journal_type," +
                        " remarks, " +
                        " transaction_amount) " +
                        " values (" +
                        "'" + msGetSEDlGID + "', " +
                        "'" + msGetGID + "'," +
                        "'" + lsSEaccount_gid + "'," +
                        "'" + lsaccount_gid + "'," +
                        "'" + SEjournal_type + "'," +
                        "'" + dr["payment_remarks"].ToString().Trim() + "', " +
                        "'" + lspaid_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = "select account_gid,bank_code,bank_name from acc_mst_tbank where bank_gid='" + lsbank_gid + "'";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {
                        lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                        LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                        lsSEAccountname = objODBCDataReader["bank_name"].ToString();
                    }
                    objODBCDataReader.Close();
                    msSQL = "select account_gid,bank_code,bank_name from acc_mst_tcreditcard where bank_gid='" + lsbank_gid + "'";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {
                        lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                        LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                        lsSEAccountname = objODBCDataReader["bank_name"].ToString();
                    }
                    objODBCDataReader.Close();
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " transaction_gid," +
                            " journal_type," +
                            " remarks, " +
                            " transaction_amount) " +
                            " values (" +
                            "'" + msGetSEDlGID + "', " +
                            "'" + msGetGID + "'," +
                            "'" + lsSEaccount_gid + "'," +
                            "'" + lsaccount_gid + "'," +
                            "'" + SEjournal_type + "'," +
                            "'" + dr["payment_remarks"].ToString() + "', " +
                            "'" + lspaid_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                msSQL = " Insert into acc_trn_journalentrydtl " +
                    " (journaldtl_gid, " +
                    " journal_gid, " +
                    " account_gid," +
                    " transaction_gid," +
                    " journal_type," +
                    " remarks, " +
                    " transaction_amount) " +
                    " values (" +
                    "'" + msGetDlGID + "', " +
                    "'" + msGetGID + "'," +
                    "'" + lsaccount_gid + "'," +
                    "'" + lsSEaccount_gid + "'," +
                    "'" + journal_type + "'," +
                    "'" + dr["payment_remarks"].ToString() + "', " +
                    "'" + lspayment_amount_L + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //---------------------Record for main table insert-----------------------
                msSQL = " Insert into acc_trn_journalentry " +
                  " (journal_gid, " +
                  " transaction_date, " +
                  " remarks, " +
                  " transaction_type," +
                  " branch_gid," +
                  " reference_type, " +
                  " reference_gid, " +
                  " journal_from," +
                  " transaction_code, " +
                  " transaction_gid, " +
                  " journal_year, " +
                  " journal_month, " +
                  " journal_day, " +
                  " journal_refno)" +
                  " values (" +
                  "'" + msGetGID + "', " +
                  "'" + DateTime.Parse(dr["payment_date"].ToString()).ToString("yyyy-MM-dd") + "', " +
                  "'" + dr["payment_remarks"].ToString() + "'," +
                  "'" + LSTransactionType + "'," +
                  "'" + dr["branch_name"].ToString() + "', " +
                  "'" + lsSEAccountname + "', " +
                  "'" + LSReferenceGID + "', " +
                  "'Payment'," +
                  "'" + LSTransactionCode + "'," +
                  "'" + dr["payment_gid"].ToString() + "', " +
                  "'" + journal_year + "', " +
                  "'" + journal_month + "', " +
                  "'" + journal_Day + "', " +
                  "'" + dr["payment_gid"].ToString() + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


            }

        }

        public void DaPostEmployeeSalary1(dateform value)
        {
            try
            {
                string month, year, journal_refno;
                double grand_total = 0, lsgross = 0, statutory_amount = 0;
                string lsother_add, lsother_ded = "";
                var lsdate = DateTime.Parse(value.todate).ToString("yyyy") + "-04-01";
                var to_date = (DateTime.Parse(lsdate).AddYears(1)).ToString("yyyy") + "-03-31";

                msSQL1 = " delete from acc_trn_journalentrydtl where journal_gid in (select journal_gid from " +
                        " acc_trn_journalentry where " +
                        " transaction_gid in(select salary_gid from pay_trn_tsalary where " +
                        " payrun_date>='" + lsdate + "' and payrun_date <='" + to_date + "'))";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
                msSQL1 = "delete from acc_trn_journalentry where " +
                         "  transaction_gid in(select salary_gid from pay_trn_tsalary where payrun_date>='" + lsdate + "' and payrun_date <='" + to_date + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

                msSQL = "SELECT salary_gid,payrun_date,c.user_code,a.employee_gid,d.account_gid, concat(c.user_firstname,' ',c.user_lastname) as " +
                    " employee_name,earned_gross_salary,a.month,a.year,d.department_name,loan_amount,advance," +
                    " (select sum(earned_salarycomponent_amount) " +
                    " from pay_trn_tsalarydtl x where x.salary_gid=a.salary_gid " +
                    " and x.salarygradetype='Deduction' group by a.salary_gid) as deduction," +
                    " (select (sum(earned_salarycomponent_amount) + sum(earnedemployer_salarycomponentamount))" +
                    " from pay_trn_tsalarydtl x where x.salary_gid=a.salary_gid and x.statutory_flag='Y' " +
                    " group by a.salary_gid) as statutory, " +
                    " (select sum(earned_salarycomponent_amount) + " +
                    " sum(earnedemployer_salarycomponentamount) from pay_trn_tsalarydtl x where x.salary_gid=a.salary_gid " +
                    " and x.salarygradetype='Other' and othercomponent_type='Addition' group by a.salary_gid) as other_add," +
                    " (select sum(earned_salarycomponent_amount) + sum(earnedemployer_salarycomponentamount)" +
                    " from pay_trn_tsalarydtl x where x.salary_gid=a.salary_gid and x.salarygradetype='Other' " +
                    " and othercomponent_type='Deduction' group by a.salary_gid) as other_ded from pay_trn_tsalary a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                    " left join hrm_mst_tdepartment d on d.department_gid = b.department_gid " +
                    " where payrun_date>='" + lsdate + "' and payrun_date <='" + to_date + "'" +
                    "order by payrun_date asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_datatable.Rows)
                    {

                        month = row["month"].ToString();
                        year = row["year"].ToString();
                        jounrnal_date = row["payrun_date"].ToString();
                        int monthNumber = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;
                        journal_refno = "00" + monthNumber;
                        journal_function(DateTime.Parse(jounrnal_date.Replace("-", "")));
                        journal_year = journal_monthandyear[0];
                        journal_month = journal_monthandyear[1];
                        journal_Day = journal_monthandyear[2];
                        lsgross = double.Parse(row["earned_gross_salary"].ToString());
                        lsother_add = row["other_add"].ToString();
                        lsother_ded = row["other_ded"].ToString();
                        string account_gid = row["account_gid"].ToString();
                        string lsstatutory = row["statutory"].ToString();
                        string lsdeduction = row["deduction"].ToString();
                        if (lsother_add == "" || lsother_add == null)
                        {
                            lsother_add = "0";
                        }
                        if (lsother_ded == "" || lsother_ded == null)
                        {
                            lsother_ded = "0";
                        }
                        if(lsstatutory =="" || lsstatutory ==null)
                        {
                            statutory_amount = 0;
                        }
                        else
                        {
                            statutory_amount = double.Parse(lsstatutory);
                        }
                        if(lsdeduction =="" ||  lsdeduction ==null)
                        {
                            lsdeduction = "0";
                        }
                        else
                        {
                            lsdeduction = row["deduction"].ToString();
                        }
                       
                        grand_total = lsgross + statutory_amount - double.Parse(lsdeduction);

                        msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");

                        msSQL = " Insert into acc_trn_journalentry " +
                             " (journal_gid, " +
                             " journal_refno, " +
                             " transaction_code, " +
                             " transaction_date, " +
                             " transaction_type," +
                             " reference_type," +
                             " reference_gid," +
                             " transaction_gid, " +
                             " journal_from," +
                             " journal_year, " +
                             " journal_month, " +
                             " journal_day, " +
                             " remarks, " +
                             " branch_gid)" +
                             " values (" +
                             "'" + msGetGID + "', " +
                             "'" + month + year + "-" + journal_refno + "'," +
                             "'" + row["user_code"].ToString() + "'," +
                             "'" + DateTime.Parse(row["payrun_date"].ToString()).ToString("yyyy-MM-dd") + "', " +
                             "'Journal', " +
                             "'" + row["employee_name"].ToString() + "', " +
                             "'" + row["employee_gid"].ToString() + "', " +
                             "'" + row["salary_gid"].ToString() + "', " +
                             "'Payroll'," + "'" +
                             journal_year + "', " +
                             "'" + journal_month + "', " +
                             "'" + journal_Day + "', " +
                             " 'Salary Payrun for the employee" + " " + row["employee_name"].ToString() + " " + "on this" + " " + month + "-" + year + "', " +
                             "'" + row["department_name"].ToString() + "') ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                            double total = (grand_total - statutory_amount) + double.Parse(lsother_add) - double.Parse(lsother_ded);
                            msSQL = " Insert into acc_trn_journalentrydtl " +
                                   " (journaldtl_gid, " +
                                   " journal_gid, " +
                                   " account_gid," +
                                   " journal_type," +
                                   " transaction_amount)" +
                                   " values (" +
                                   "'" + msGetDlGID + "', " +
                                   "'" + msGetGID + "'," +
                                   "'" + account_gid + "'," +
                                   "'cr'," +
                                   "'" + total + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                           // double total = (grand_total - statutory_amount) + double.Parse(lsother_add) - double.Parse(lsother_ded);
                            msSQL = " Insert into acc_trn_journalentrydtl " +
                                   " (journaldtl_gid, " +
                                   " journal_gid, " +
                                   " account_gid," +
                                   " journal_type," +
                                   " transaction_amount)" +
                                   " values (" +
                                   "'" + msGetDlGID + "', " +
                                   "'" + msGetGID + "'," +
                                   "'FCOA2408255'," +
                                   "'dr'," +
                                   "'" + (grand_total + double.Parse(lsother_add) - double.Parse(lsother_ded)) +  "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        if (mnResult == 1)
                        {
                            string loan_acc_gid = "";
                            double lsloan = double.Parse(row["loan_amount"].ToString()) + double.Parse(row["advance"].ToString());
                            if (lsloan > 0)
                            {

                                msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Loan' and module_name='PAY' and field_name='Loan' ";
                                loan_acc_gid = objdbconn.GetExecuteScalar(msSQL);
                                if (loan_acc_gid != "" && loan_acc_gid != null)
                                {
                                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                                    msSQL = " Insert into acc_trn_journalentrydtl " +
                                              " (journaldtl_gid, " +
                                              " journal_gid, " +
                                              " account_gid," +
                                              " journal_type," +
                                              " transaction_amount)" +
                                              " values (" +
                                              "'" + msGetDlGID + "', " +
                                              "'" + msGetGID + "'," +
                                              "'" + loan_acc_gid + "'," +
                                              "'dr'," +
                                              "'" + lsloan + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                                    msSQL = " Insert into acc_trn_journalentrydtl " +
                                              " (journaldtl_gid, " +
                                              " journal_gid, " +
                                              " account_gid," +
                                              " journal_type," +
                                              " transaction_amount)" +
                                              " values (" +
                                              "'" + msGetDlGID + "', " +
                                              "'" + msGetGID + "'," +
                                              "'" + account_gid + "'," +
                                              "'cr'," +
                                              "'" + lsloan + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }

                            msSQL = " Select a.component_name,earned_salarycomponent_amount as statutory_amount_employee ,earnedemployer_salarycomponentamount " +
                                    " as statutory_amount_employer,b.salary_gid,b.salarycomponent_gid,a.account_gid,a.account_gid_employer,employee_gid,month,year from pay_mst_tsalarycomponent a  " +
                                    " left join pay_trn_tsalarydtl b on a.salarycomponent_gid=b.salarycomponent_gid " +
                                    "left join pay_trn_tsalary c on b.salary_gid=c.salary_gid where b.statutory_flag='Y'";
                            dt_datatable1 = objdbconn.GetDataTable(msSQL);
                            List<salarydtl> salarydtls = new List<salarydtl>();
                            salarydtls = cmnfunctions.ConvertDataTable<salarydtl>(dt_datatable1);

                            List<salarydtl> salarydtls_emp = salarydtls.Where(a=> a.month == month && a.year == year 
                                                        && a.employee_gid == row["employee_gid"].ToString()).ToList();
                            string ls = "";
                            msSQL = " Insert into acc_trn_journalentrydtl " +
                                        " (journaldtl_gid, " +
                                        " journal_gid, " +
                                        " account_gid," +
                                        " journal_type," +
                                        " transaction_amount)" +
                                        " values ";
                            foreach (var dr in salarydtls_emp)
                            {   
                                msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                                
                                     ls +=    "('" + msGetDlGID + "', " +
                                         "'" + msGetGID + "'," +
                                         "'" + dr.account_gid + "'," +
                                         "'cr'," +
                                         "'" + dr.statutory_amount_employee + "'),";

                                msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");

                                     ls += "('" + msGetDlGID + "', " +
                                        "'" + msGetGID + "'," +
                                        "'" + dr.account_gid_employer + "'," +
                                        "'cr'," +
                                        "'" + dr.statutory_amount_employer + "'),";

                            }
                            if(ls != "")
                            {
                                msSQL = msSQL + ls.TrimEnd(',');
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            
                            
                        }
                    }
                }
                if (mnResult == 1)
                {
                    value.message = "Journal Posted Successfully";
                    value.status = true;
                    return;
                }
                else
                {
                    value.message = "No Journals are Posted";
                    value.status = false;
                    return;
                }
            }
            catch
            {
                value.message = "No Journals are Posted";
                value.status = false;
                return;
            }
        }
        public void DaPostDebtorLedger(result value)
        {
            try
            {
                msSQL = "delete from acc_mst_tchartofaccount where accountgroup_name like 'Sundry Debtor%'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                List<Customer> customers = new List<Customer>();
                msSQL1 = "select customer_gid,customer_name,a.taxsegment_gid,b.taxsegment_name,a.customer_code," +
                       " b.account_gid,b.account_name from crm_mst_tcustomer a" +
                       " left join acp_mst_ttaxsegment b on a.taxsegment_gid = b.taxsegment_gid where b.reference_type='Customer'";
                dt_customer = objdbconn.GetDataTable(msSQL1);
                if (dt_customer.Rows.Count <= 0)
                {
                    value.message = "Please Assign Customer to TaxSegment";
                    value.status = false;
                    return;
                }
                else
                {
                    customers = cmnfunctions.ConvertDataTable<Customer>(dt_customer);

                }
                msSQL = "select customer_gid from rbl_trn_tinvoice " +
                    " where invoice_status not in ('Canceled','Invoice Cancelled') group by customer_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count <= 0)
                {
                    value.message = "No Invoices are Exist";
                    value.status = false;
                    return;
                }

                foreach (DataRow dr in dt_datatable.Rows)
                {
                    lscustomergid = dr["customer_gid"].ToString();
                    List<Customer> customer_acc_gid = customers.Where(a => a.customer_gid == lscustomergid).ToList();
                    foreach (var dr1 in customer_acc_gid)
                    {
                        acc_subgroup_gid = dr1.account_gid;
                        acc_subgroup_name = dr1.account_name;
                        customer_name = dr1.customer_name;
                        customer_id = dr1.customer_code;

                    }
                    lsreferenceType = acc_subgroup_name;
                    lsaccoungroup_gid = acc_subgroup_gid;
                    LSLedgerType = "N";
                    LSDisplayType = "Y";
                    string ledger_check = "";
                    if (lsaccoungroup_gid != null || lsaccoungroup_gid != "")
                    {

                        msAccGetGID = objcmnfunctions.GetMasterGID_SP("FCOA");
                        msSQL = " insert into acc_mst_tchartofaccount (" +
                        " account_gid," +
                        " accountgroup_gid," +
                        " accountgroup_name," +
                        " account_code," +
                        " account_name," +
                        " has_child," +
                        " ledger_type," +
                        " display_type," +
                        " Created_Date, " +
                        " Created_By, " +
                        " reference_gid ," +
                        " gl_code) " +
                        " values (" +
                        "'" + msAccGetGID + "'," +
                        "'" + lsaccoungroup_gid + "'," +
                        "'" + lsreferenceType + "'," +
                        "'" + customer_id + "'," +
                        "'" + customer_name + "'," +
                        "'N'," +
                        "'" + LSLedgerType + "'," +
                        "'" + LSDisplayType + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'Finance Regulation'," +
                        "'" + dr["customer_gid"].ToString() + "'," +
                        "'" + customer_name + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update crm_mst_tcustomer set account_gid='" + msAccGetGID + "' where " +
                        " customer_gid='" + dr["customer_gid"].ToString() + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }




                }
                if (mnResult == 1)
                {
                    value.message = "Customer Ledgers Created Successfully";
                    value.status = true;
                    return;
                }
                else
                {
                    value.message = "Error While Creating Customer Ledgers";
                    value.status = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                value.message = "Exception While Creating Customer Ledgers";
                value.status = false;
                return;
            }
        }

        public void DaPostCreditorLedger(result value)
        {
            string vendor_account_gid = "", purchasetype_acc_gid = "", vendor_account_name = "";
            string tax_account_gid = "", vendor_name = "", vendor_code = "";
            List<Vendor> vendors = new List<Vendor>();
            try
            {
                msSQL1 = "select vendor_companyname ,a.taxsegment_gid,b.taxsegment_name,a.vendor_code,a.vendor_gid, " +
               " b.account_gid,b.account_name from acp_mst_tvendor a " +
               " left join acp_mst_ttaxsegment b on b.taxsegment_gid=a.taxsegment_gid where reference_type='Vendor'";
                dt_vendor = objdbconn.GetDataTable(msSQL1);
                vendors = cmnfunctions.ConvertDataTable<Vendor>(dt_vendor);

                msSQL = "delete from acc_mst_tchartofaccount where accountgroup_name like 'Sundry Creditor%'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "select vendor_gid from acp_trn_tinvoice where " +
                "  invoice_status<>'IV Canceled' and vendor_gid is not null and vendor_gid<>'' group by vendor_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                foreach (DataRow dr in dt_datatable.Rows)
                {
                    lsvendor_gid = dr["vendor_gid"].ToString();
                    List<Vendor> vendor_acc_gid = vendors.Where(a => a.vendor_gid == lsvendor_gid).ToList();
                    foreach (var dr1 in vendor_acc_gid)
                    {
                        vendor_account_gid = dr1.account_gid;
                        vendor_name = dr1.vendor_companyname;
                        vendor_code = dr1.vendor_code;
                        vendor_account_name = dr1.account_name;

                    }
                    lsreferenceType = vendor_account_name;
                    lsaccoungroup_gid = vendor_account_gid;
                    LSLedgerType = "N";
                    LSDisplayType = "N";
                    string ledger_check = "";
                    if (lsaccoungroup_gid != null || lsaccoungroup_gid != "")
                    {
                        msAccGetGID = "";
                        msSQL = "select account_gid from acc_mst_tchartofaccount where account_name='" + vendor_name + "' ";
                        ledger_check = objdbconn.GetExecuteScalar(msSQL);
                        if (ledger_check == "" || ledger_check == null)
                        {
                            msAccGetGID = objcmnfunctions.GetMasterGID_SP("FCOA");
                            msSQL = " insert into acc_mst_tchartofaccount (" +
                            " account_gid," +
                            " accountgroup_gid," +
                            " accountgroup_name," +
                            " account_code," +
                            " account_name," +
                            " has_child," +
                            " ledger_type," +
                            " display_type," +
                            " Created_Date, " +
                            " Created_By, " +
                            " reference_gid ," +
                            " gl_code) " +
                            " values (" +
                            "'" + msAccGetGID + "'," +
                            "'" + lsaccoungroup_gid + "'," +
                            "'" + lsreferenceType + "'," +
                            "'" + vendor_code + "'," +
                            "'" + vendor_name + "'," +
                            "'N'," +
                            "'" + LSLedgerType + "'," +
                            "'" + LSDisplayType + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'Finance Regulation'," +
                            "'" + dr["vendor_gid"].ToString() + "'," +
                            "'" + customer_name + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            vendor_account_gid = msAccGetGID;

                        }
                        else
                        {
                            vendor_account_gid = ledger_check;
                        }

                    }
                    if (mnResult == 1)
                    {
                        msSQL = " update acp_mst_tvendor set " +
                            " account_gid = '" + vendor_account_gid + "'" +
                            " where vendor_gid='" + dr["vendor_gid"].ToString() + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                }
                if (mnResult == 1)
                {
                    value.message = "Creditor Created Successfully";
                    value.status = true;
                    return;
                }
                else
                {
                    value.message = "Error while Creating Creditor";
                    value.status = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                value.message = "Exception while Creating Creditor";
                value.status = false;
                return;
            }
        }
        public List<string> journal_function(DateTime invoice_date)
        {
            DateTime finyear = DateTime.MinValue;

            msSQL = "SELECT cast(date_format(fyear_start,'%Y-%m-%d')as char) as fyear_start,finyear_gid FROM" +
                " adm_mst_tyearendactivities ORDER BY finyear_gid DESC LIMIT 1";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                finyear = Convert.ToDateTime(objODBCDataReader["fyear_start"].ToString());
                objODBCDataReader.Close();
            }
            // DateTime invoicedate = Convert.ToDateTime(invoice_date);

            msSQL = " select timestampdiff(Month,'" + finyear.ToString("yyyy-MM-dd") + "','" + invoice_date.ToString("yyyy-MM-dd") + "') as month, " +
                " timestampdiff(day,'" + finyear.ToString("yyyy-MM-dd") + "','" + invoice_date.ToString("yyyy-MM-dd") + "') as day";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                journal_month = objODBCDataReader["month"].ToString();
                journal_Day = objODBCDataReader["day"].ToString();
                journal_year = finyear.Year.ToString();
                objODBCDataReader.Close();
            }

            if (journal_month.Length == 1)
            {
                journal_month = "0" + (Convert.ToInt32(journal_month) + 1).ToString();
            }
            else
            {
                journal_month = (Convert.ToInt32(journal_month) + 1).ToString();
            }

            if (journal_Day.Length == 1)
            {
                journal_Day = "0" + (Convert.ToInt32(journal_Day) + 1).ToString();
            }
            else
            {
                journal_Day = (Convert.ToInt32(journal_Day) + 1).ToString();
            }

            journal_monthandyear.Add(journal_year);
            journal_monthandyear.Add(journal_month);
            journal_monthandyear.Add(journal_Day);
            return journal_monthandyear;
        }


        // Triggers
        public result DaPostSalesReceipt(dateform value)
        {
            result objresult = new result();
            HttpContext ctx = HttpContext.Current;

            System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>

            {

                HttpContext.Current = ctx;

                DaPostSalesReceipt1(value);

            }));

            t.Start();
            objresult.message = "Journal Posting is in Progress";
            objresult.status = true;
            return objresult;
        }
        public result DaPostPurchasePayment(dateform value)
        {
            result objresult = new result();
            HttpContext ctx = HttpContext.Current;

            System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>

            {

                HttpContext.Current = ctx;

                DaPostPurchasePayment1(value);

            }));

            t.Start();
            objresult.message = "Journal Posting is in Progress";
            objresult.status = true;
            return objresult;
        }
        public result DaPostSlaes(dateform value)
        {
            result objresult = new result();
            HttpContext ctx = HttpContext.Current;

            System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>

            {

                HttpContext.Current = ctx;

                DaPostSales1(value);

            }));

            t.Start();
            objresult.message = "Journal Posting is in Progress";
            objresult.status = true;
            return objresult;
        }
        public result DaPostPurchase(dateform value)
        {
            result objresult = new result();
            HttpContext ctx = HttpContext.Current;

            System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>

            {

                HttpContext.Current = ctx;

                DaPostPurchase1(value);

            }));

            t.Start();
            objresult.message = "Journal Posting is in Progress";
            objresult.status = true;
            return objresult;
        }
        public result DaPostEmployeeSalary(dateform value)
        {
            result objresult = new result();
            HttpContext ctx = HttpContext.Current;

            System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>

            {

                HttpContext.Current = ctx;

                DaPostEmployeeSalary1(value);

            }));

            t.Start();
            objresult.message = "Journal Posting is in Progress";
            objresult.status = true;
            return objresult;
        }


    }
}