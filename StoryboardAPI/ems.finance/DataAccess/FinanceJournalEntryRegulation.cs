using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;
using OfficeOpenXml.ConditionalFormatting;

namespace ems.finance.DataAccess
{
    public class FinanceJournalEntryRegulation
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, dt_datatable1,dt_customer,dt_tax,dt_accmapping, dt_vendor;
        string msGetGid, lsflag;
        int mnResult;
        string msGetGID, msGetDlGID, msGetDlGID1, msGetDlGID2, msGetSEDlGID;
        string  finyear, jounrnal_date, journal_type, reference_gid, lsaccount_gid;
        OdbcDataReader objODBCDataReader;
        string journal_month, journal_year, journal_Day, account_gid, msgetgid = string.Empty;
        List<string> journal_monthandyear = new List<string>();
        string lswthaccount_gid, lsWHTjournal_type, customer_name, journal_gid = string.Empty;
        string lsADaccount_GID, lsADjournal_type = string.Empty;
        string SEjournal_type, LSTransactionCode, LSTransactionType, msGetD3GID, lscustomergid,lsvendor_gid, msGetD2GID,
        journal_date, LSReferenceGID, lsSEAccountname, lsSEaccount_gid = string.Empty;
        string msAccGetGID, lsaccoungroup_gid = string.Empty, LSLedgerType = string.Empty, LSDisplayType = string.Empty, lsreferenceType = string.Empty;
        string acc_subgroup_gid = "", acc_subgroup_name ="",customer_id="";
        public void DaPostSales(string date)
        {
            var lsdate = DateTime.Parse(date).ToString("yyyy-MM-dd");
            List<invoicedtl> Mdl_rblinvoicedtl =new List<invoicedtl>();
            List<Customer> customers = new List<Customer>();
            List<Tax> tax  =new List<Tax>();
            List<AccMapping> accmappin = new List<AccMapping>();
            msSQL1 = "update crm_mst_tcustomer set account_gid=null where customer_gid not in (select customer_gid from " +
                    " rbl_trn_tinvoice";
            mnResult =objdbconn.ExecuteNonQuerySQL(msSQL1);

            //Delete journal data 

            msSQL1 = " delete from acc_trn_journalentrydtl where journal_gid in (select journal_gid from " +
                    " acc_trn_journalentry where transaction_date>='"+ lsdate +"' and journal_from='Sales' and " +
                    " transaction_gid in(select invoice_gid from rbl_trn_tinvoice where invoice_date>='"+ lsdate +"'))";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
            msSQL1 = "delete from acc_trn_journalentry where transaction_date>='"+ lsdate +"' and journal_from='Sales'" +
                    " and transaction_gid in(select invoice_gid from rbl_trn_tinvoice where invoice_date>='"+ lsdate +"') ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);


            //Get customer account_gid
            msSQL1 = "select customer_gid,customer_name,a.taxsegment_gid,b.taxsegment_name,a.customer_code," +
                    " b.account_gid,b.account_name from crm_mst_tcustomer a" +
                    " left join acp_mst_ttaxsegment b on a.taxsegment_gid = b.taxsegment_gid";
            dt_customer = objdbconn.GetDataTable(msSQL1);
            customers = cmnfunctions.ConvertDataTable<Customer>(dt_datatable) ;

            //Get Tax account_gid
            msSQL = "select account_gid,tax_gid from acp_mst_ttax";
            dt_tax = objdbconn.GetDataTable(msSQL);
            tax =cmnfunctions.ConvertDataTable<Tax>(dt_tax);
            //Get account mapping
            msSQL = "select account_gid,field_name from acc_mst_accountmapping where screen_name='Invoice'" +
                    " and module_name='RBL'";
            dt_accmapping = objdbconn.GetDataTable( msSQL);
            accmappin=cmnfunctions.ConvertDataTable<AccMapping>(dt_accmapping);

            msSQL = "select branch_gid,invoice_date,invoice_gid,customer_gid,invoice_refno,invoice_amount," +
                "  sales_type,freight_charges ,additionalcharges_amount,discount_amount ,roundoff,packing_charges," +
                " insurance_charges from rbl_trn_tinvoice " +
                " where invoice_date>='"+ lsdate +" and invoice_status not in ('Canceled','Invoice Cancelled')'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            
            foreach (DataRow dr in dt_datatable.Rows)
            {
                
                string customer_account_gid = "",salestype_acc_gid="";
                string tax_account_gid = "";
                double grand_total = 0.00, additionalcharges_amount = 0.00, freight_charges = 0.00, discount_amount = 0.00;
                double roundoff=0.0, packing_charges=0.0, insurance_charges=0.00;
                lscustomergid = dr["customer_gid"].ToString();
                jounrnal_date = dr["invoice_date"].ToString();
                grand_total = double.Parse(dr["invoice_amount"].ToString());
                additionalcharges_amount = double.Parse(dr["additionalcharges_amount"].ToString());
                freight_charges = double.Parse(dr["freight_charges"].ToString()) ;
                roundoff = double.Parse(dr["roundoff"].ToString() );
                packing_charges = double.Parse(dr["packing_charges"].ToString());
                insurance_charges = double.Parse(dr["insurance_charges"].ToString());
                journal_function(jounrnal_date);
                journal_year = journal_monthandyear[0];
                journal_month = journal_monthandyear[1];
                journal_Day = journal_monthandyear[2];
                string subgroup_acc_gid = "";
                msSQL = "select account_gid from smr_trn_tsalestype where salestype_gid='" + dr["sales_type"].ToString() + "'";
                salestype_acc_gid = objdbconn.GetExecuteScalar(msSQL);

                List<Customer> customer_acc_gid=customers.Where(a=> a.customer_gid == lscustomergid).ToList();
                foreach(var dr1 in customer_acc_gid)
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

                if(lsaccoungroup_gid != null || lsaccoungroup_gid !="")
                {
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
                    " gl_code) " +
                    " values (" +
                    "'" + msGetDlGID + "'," +
                    "'" + lsaccoungroup_gid + "'," +
                    "'" + lsreferenceType + "'," +
                    "'" + customer_id + "'," +
                    "'" + customer_name + "'," +
                    "'N'," +
                    "'" + LSLedgerType + "'," +
                    "'" + LSDisplayType + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    "'Finance Regulation'," +
                    "'" + customer_name + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
                customer_account_gid = msGetDlGID;

                msSQL ="update crm_mst_tcustomer set account_gid='" + msGetDlGID  +"' where " +
                    " customer_gid='"+ dr["customer_gid"].ToString() +"'";
                mnResult=objdbconn.ExecuteNonQuerySQL (msSQL);

                msGetGID = objcmnfunctions.GetMasterGID("FPCC");
                if(msGetGID =="E")
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
                    " branch_gid" +
                    " ) values (" +
                    "'" + msGetGID + "'," +
                    "'" + dr["invoice_refno"].ToString() + "'," +
                    "'" + dr["invoice_refno"].ToString() + "'," +
                    "'" + dr["invoice_date"].ToString() + "'," +
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
                    "'" + dr["branch_gid"] + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                               "'" + grand_total + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                
                msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                msSQL = " select sum(product_price) from rbl_trn_tinvoicedtl " +
                        " where invoice_gid='"+ dr["invoice_gid"].ToString() +"'";
                string lsbase_amount = objdbconn.GetExecuteScalar(msSQL);

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
                               "'"+ double.Parse(lsbase_amount) +"')";
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
                    msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                    msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                    msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                    msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                    msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                    msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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

                msSQL1 += ls.TrimEnd(',');
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

                foreach(var dr2 in tax)
                {
                    msSQL = " select sum(tax_amount) as tax1 from rbl_trn_tinvoicedtl " +
                        " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                        " and tax1_gid='" + dr2.tax_gid + "'";
                    string tax_amount1 = objdbconn.GetExecuteScalar(msSQL);
                    if (tax_amount1 == null || tax_amount1=="")
                    {
                        tax_amount1 = "0.00";
                    }
                    msSQL = " select sum(tax_amount2) as tax1 from rbl_trn_tinvoicedtl " +
                       " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                       " and tax1_gid='" + dr2.tax_gid + "'";
                    string tax_amount2 = objdbconn.GetExecuteScalar(msSQL);
                    if (tax_amount2 == null || tax_amount2 == "")
                    {
                        tax_amount2 = "0.00";
                    }
                    msSQL = " select sum(tax_amount3) as tax1 from rbl_trn_tinvoicedtl " +
                       " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                       " and tax1_gid='" + dr2.tax_gid + "'";
                    string tax_amount3 = objdbconn.GetExecuteScalar(msSQL);
                    if (tax_amount3 == null || tax_amount3 == "")
                    {
                        tax_amount3 = "0.00";
                    }
                    if(tax_amount1 !="0.00" || tax_amount2 != "0.00" || tax_amount3 !="0.00")
                    {
                        double sum_tax = double.Parse(tax_amount1) + double.Parse(tax_amount2) + double.Parse(tax_amount3);
                       string msGetDlGID1 = objcmnfunctions.GetMasterGID("FPCD");
                        msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " remarks," +
                            " journal_type," +
                            " transaction_amount)" +
                            " values (" +
                            "'" + msGetDlGID1 + "', " +
                            "'" + msgetgid + "'," +
                            "'" + dr2.tax_account_gid + "'," +
                            "'" + customer_name + "', " +
                            "'cr'," +
                            "'" + sum_tax + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                

            }

        }

        public void DaPostPurchase(string date)
        {
                var lsdate = DateTime.Parse(date).ToString("yyyy-MM-dd");
                List<invoicedtl> Mdl_rblinvoicedtl = new List<invoicedtl>();
                List<Vendor> vendors = new List<Vendor>();
                List<Tax> tax = new List<Tax>();
                List<AccMapping> accmappin = new List<AccMapping>();
                msSQL1 = "update acp_mst_tvendor set account_gid=null where vendor_gid not in (select vendor_gid from " +
                        " acp_trn_tinvoice";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

                //Delete journal data 

                msSQL1 = " delete from acc_trn_journalentrydtl where journal_gid in (select journal_gid from " +
                        " acc_trn_journalentry where transaction_date>='"+ lsdate +"' and journal_from='Purchase' " +
                        " and transaction_gid in(select invoice_gid from acp_trn_tinvoice where invoice_date >= '"+ lsdate +"'))";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
                msSQL1 = "delete from acc_trn_journalentry where transaction_date>='"+ lsdate +"' and journal_from='Purchase'" +
                          " and transaction_gid in(select invoice_gid from acp_trn_tinvoice where invoice_date>='"+ lsdate +"')) ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

                //Get invoicedtl data

                msSQL1 = "select invoice_gid,invoice_date from acp_trn_tinvoicedtl " +
                            " where invoice_gid in(select invoice_gid from acp_trn_tinvoice where " +
                            " invoice_date>='"+ lsdate +"' and invoice_status<>'IV Canceled')";
                dt_datatable1 = objdbconn.GetDataTable(msSQL1);
                Mdl_rblinvoicedtl = cmnfunctions.ConvertDataTable<invoicedtl>(dt_datatable1);

                //Get vendor account_gid
                msSQL1 = "select vendor_name ,a.taxsegment_gid,b.taxsegment_name,a.vendor_code, " +
                    " b.account_gid,b.account_name from acp_mst_tvendor a " +
                    " left join acp_mst_ttaxsegment b on b.taxsegment_gid=a.taxsegment_gid";
                dt_vendor = objdbconn.GetDataTable(msSQL1);
                vendors = cmnfunctions.ConvertDataTable<Vendor>(dt_vendor);

                //Get Tax account_gid
                msSQL = "select account_gid,tax_gid from acp_mst_ttax";
                dt_tax = objdbconn.GetDataTable(msSQL);
                tax = cmnfunctions.ConvertDataTable<Tax>(dt_tax);
                //Get account mapping
                msSQL = "select account_gid,field_name from acc_mst_accountmapping where screen_name='Invoice'" +
                        " and module_name='RBL'";
                dt_accmapping = objdbconn.GetDataTable(msSQL);
                accmappin = cmnfunctions.ConvertDataTable<AccMapping>(dt_accmapping);

                msSQL = "select branch_gid,invoice_date,invoice_gid,customer_gid,invoice_refno,invoice_amount," +
                    "  sales_type,freight_charges ,additionalcharges_amount,discount_amount ,roundoff,packing_charges," +
                    " insurance_charges from acp_trn_tinvoice where invoice_date>='"+ lsdate +"' and invoice_status<>'IV Canceled'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                foreach (DataRow dr in dt_datatable.Rows)
                {

                    string vendor_account_gid = "", purchasetype_acc_gid = "", vendor_account_name="";
                    string tax_account_gid = "", vendor_name="",vendor_code="";
                    double grand_total = 0.00, additionalcharges_amount = 0.00, freight_charges = 0.00, discount_amount = 0.00;
                    double roundoff = 0.0, packing_charges = 0.0, insurance_charges = 0.00;
                    lsvendor_gid = dr["vendor_gid"].ToString();
                    jounrnal_date = dr["invoice_date"].ToString();
                    grand_total = double.Parse(dr["invoice_amount"].ToString());
                    additionalcharges_amount = double.Parse(dr["additionalcharges_amount"].ToString());
                    freight_charges = double.Parse(dr["freight_charges"].ToString());
                    roundoff = double.Parse(dr["roundoff"].ToString());
                    packing_charges = double.Parse(dr["packing_charges"].ToString());
                    insurance_charges = double.Parse(dr["insurance_charges"].ToString());
                    journal_function(jounrnal_date);
                    journal_year = journal_monthandyear[0];
                    journal_month = journal_monthandyear[1];
                    journal_Day = journal_monthandyear[2];

                    msSQL = "select account_gid from pmr_trn_tpurchasetype where purchasetype_gid='" + dr["purchase_type"].ToString() + "'";
                    purchasetype_acc_gid = objdbconn.GetExecuteScalar(msSQL);

                    List<Vendor> vendor_acc_gid = vendors.Where(a => a.vendor_gid == lsvendor_gid).ToList();
                    foreach (var dr1 in vendor_acc_gid)
                    {
                        vendor_account_gid = dr1.account_gid;
                        vendor_name = dr1.vendor_name;
                        vendor_code = dr1.vendor_code;
                        vendor_account_name = dr1.account_name;
                        
                    }
                        lsreferenceType = vendor_account_name;
                        lsaccoungroup_gid = vendor_account_gid;
                        LSLedgerType = "N";
                        LSDisplayType = "N";

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
                            " gl_code) " +
                            " values (" +
                            "'" + msGetDlGID + "'," +
                            "'" + lsaccoungroup_gid + "'," +
                            "'" + lsreferenceType + "'," +
                            "'" + vendor_code + "'," +
                            "'" + vendor_name + "'," +
                            "'N'," +
                            "'" + LSLedgerType + "'," +
                            "'" + LSDisplayType + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'Finance Regulation'," +
                            "'" + vendor_name + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if(mnResult==1)
                {
                    msSQL = " update acp_mst_tvendor set " +
                        " account_gid = '" + msGetDlGID + "'" +
                        " where vendor_gid='" + dr["vendor_gid"].ToString() + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                msGetGID = objcmnfunctions.GetMasterGID("FPCC");
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
                        " branch_gid" +
                        " ) values (" +
                        "'" + msGetGID + "'," +
                        "'" + dr["invoice_refno"].ToString() + "'," +
                        "'" + dr["invoice_refno"].ToString() + "'," +
                        "'" + dr["invoice_date"].ToString() + "'," +
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

                    msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                                   "'" + account_gid + "'," +
                                   "'" + vendor_name + "'," +
                                   "'cr'," +
                                   "'" + grand_total + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                    msSQL = " select sum(product_price) from acp_trn_tinvoicedtl " +
                            " where invoice_gid='" + dr["invoice_gid"].ToString() + "'";
                    string lsbase_amount = objdbconn.GetExecuteScalar(msSQL);

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
                                   "'" + double.Parse(lsbase_amount) + "')";
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
                        msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                        msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                        msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
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
                        msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                        msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                        " and module_name='RBL' and field_name='Frieght Charges' ";
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
                        msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                        msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                        " and module_name='RBL' and field_name='Packing Charges' ";
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
                        msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                        msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Invoice'" +
                        " and module_name='RBL' and field_name='Insurance' ";
                        string account_gid2 = objdbconn.GetExecuteScalar(msSQL);

                        ls += "('" + msGetDlGID + "'," +
                            "'" + msGetGID + "'," +
                            "'" + account_gid2 + "'," +
                            "'" + vendor_name + "'," +
                            "'cr'," +
                            "'" + insurance_charges + "'),";

                    }
                    if(ls !="" || ls !=null)
                    {
                        msSQL1 += ls.TrimEnd(',');
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
                    }

                    foreach (var dr2 in tax)
                    {
                        msSQL = " select sum(tax_amount) as tax1 from acp_trn_tinvoicedtl " +
                            " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                            " and tax1_gid='" + dr2.tax_gid + "'";
                        string tax_amount1 = objdbconn.GetExecuteScalar(msSQL);
                        if (tax_amount1 == null || tax_amount1 == "")
                        {
                            tax_amount1 = "0.00";
                        }
                        msSQL = " select sum(tax_amount2) as tax1 from acp_trn_tinvoicedtl " +
                           " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                           " and tax1_gid='" + dr2.tax_gid + "'";
                        string tax_amount2 = objdbconn.GetExecuteScalar(msSQL);
                        if (tax_amount2 == null || tax_amount2 == "")
                        {
                            tax_amount2 = "0.00";
                        }
                        msSQL = " select sum(tax_amount3) as tax1 from acp_trn_tinvoicedtl " +
                           " where invoice_gid='" + dr["invoice_gid"].ToString() + "' " +
                           " and tax1_gid='" + dr2.tax_gid + "'";
                        string tax_amount3 = objdbconn.GetExecuteScalar(msSQL);
                        if (tax_amount3 == null || tax_amount3 == "")
                        {
                            tax_amount3 = "0.00";
                        }
                        if (tax_amount1 != "0.00" || tax_amount2 != "0.00" || tax_amount3 != "0.00")
                        {
                            double sum_tax = double.Parse(tax_amount1) + double.Parse(tax_amount2) + double.Parse(tax_amount3);
                            string msGetDlGID1 = objcmnfunctions.GetMasterGID("FPCD");
                            msSQL = " Insert into acc_trn_journalentrydtl " +
                                " (journaldtl_gid, " +
                                " journal_gid, " +
                                " account_gid," +
                                " remarks," +
                                " journal_type," +
                                " transaction_amount)" +
                                " values (" +
                                "'" + msGetDlGID1 + "', " +
                                "'" + msgetgid + "'," +
                                "'" + dr2.tax_account_gid + "'," +
                                "'" + vendor_name + "', " +
                                "'dr'," +
                                "'" + sum_tax + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }


                }
            

        }

        public void DaPostSalesReceipt(string date)
        {
           string msGetGID = objcmnfunctions.GetMasterGID("FPCC");
           string  msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
           string msGetDlGID1 = objcmnfunctions.GetMasterGID("FPCD");
           string msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");
            var lsdate = DateTime.Parse(date).ToString("yyyy-MM-dd");
            //Delete journal data 

            msSQL1 = " delete from acc_trn_journalentrydtl where journal_gid from (select journal_gid from " +
                    " acc_trn_journalentry where transaction_date>='"+ lsdate +"' and " +
                    " transaction_gid in(select payment_gid from rbl_trn_tpayment where payment_date>='"+ lsdate +"'))";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
            msSQL1 = "delete from acc_trn_journalentry where transaction_date>='"+ lsdate +"' and journal_from='sales'" +
                     " and transaction_gid in(select payment_gid from rbl_trn_tpayment where payment_date>='"+ lsdate +"')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);


            string ls_Chartofacc_gid = "", lspayment_mode="",lsbank_gid="";
            double lstotal_amount = 0.00, lspaid_amount = 0.00,lstds_amount=0.00,lsadjustment_amount=0.00;
            msSQL = "select payment_gid,payment_mode,a.payment_date ,payment_type,dbank_gid,branch_gid," +
                   " tds_amount_L,adjust_amount_L,bank_charge,payment_remarks," +
                   " format((a.amount_L - a.bank_charge+a.exchange_gain-a.exchange_loss+a.adjust_amount_L  ),2) " +
                   " as total_amount,exchange_gain,exchange_loss,c.account_gid,b.customer_gid from rbl_trn_tpayment a" +
                   " left join rbl_trn_tinvoice b on a.invoice_gid=b.invoice_gid " +
                   " left join crm_mst_tcustomer c on c.customer_gid=b.customer_gid " +
                   " where approval_status='Payment Done' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            foreach(DataRow dr in dt_datatable.Rows)
            {
                jounrnal_date = DateTime.Parse(dr["payment_date"].ToString()).ToString();
                lspayment_mode = dr["payment_mode"].ToString();
                lsbank_gid = dr["dbank_gid"].ToString();
                journal_function(journal_date);
                journal_year = journal_monthandyear[0];
                journal_month = journal_monthandyear[1];
                journal_Day = journal_monthandyear[2];
                lstds_amount = double.Parse(dr["tds_amount_L"].ToString());
                lsadjustment_amount = double.Parse(dr["adjust_amount_L "].ToString());
                lstotal_amount = double.Parse(dr["total_amount"].ToString()) + double.Parse(dr["exchange_gain"].ToString()) - double.Parse(dr["exchange_loss"].ToString());
                lspaid_amount = double.Parse(dr["total_amount"].ToString()) + double.Parse(dr["bank_charge"].ToString()) + double.Parse(dr["adjust_amount_L "].ToString());
                //With Hold Tax
                string lsWHTaccount_GID = "", lsWHTjournal_type="";
                string lsscreen = "";
                lsWHTjournal_type = "cr";
                if (dr["payment_mode"].ToString() == "Receipt")
                    lsscreen = "Receipt";
                else if (dr["payment_mode"].ToString()=="Advance Receipt")
                {
                    lsscreen = "Advance";
                }
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
                              "'cr'," +
                              "'" + dr["payment_remarks"].ToString() + "'," +
                              "'" + lstds_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                //Adjust Amount
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
                              "'" + msGetDlGID1 + "', " +
                              "'" + msGetGID + "'," +
                              "'" + ls_Chartofacc_gid + "'," +
                              "'cr'," +
                              "'" + dr["payment_remarks"].ToString() + "'," +
                              "'" + lsadjustment_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                string msGetSEDlGID = objcmnfunctions.GetMasterGID("FPCD");

                if(lsbank_gid== "--Select--")
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
                    if(objODBCDataReader.HasRows==true)
                    {
                        lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                        LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                        lsSEAccountname = objODBCDataReader["bank_name"].ToString();
                    }
                    msSQL = "select account_gid,bank_code,bank_name from acc_mst_tcreditcard where bank_gid='" + lsbank_gid + "'";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {
                        lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                        LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                        lsSEAccountname = objODBCDataReader["bank_name"].ToString();
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
                            "'" + msGetSEDlGID + "', " +
                            "'" + msGetGID + "'," +
                            "'" + lsSEaccount_gid + "'," +
                            "'" + lsaccount_gid + "'," +
                            "'" + SEjournal_type + "'," +
                            "'" + dr["payment_remarks"].ToString() +"', " +
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
                  "'" + DateTime.Parse(journal_date).ToString("yyyy-MM-dd") + "', " +
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
        public void DaPostPurchasePayment (string date)
        {
            var lsdate = DateTime.Parse(date).ToString("yyyy-MM-dd");
            string msGetGID = objcmnfunctions.GetMasterGID("FPCC");
            string msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
            string msGetDlGID1 = objcmnfunctions.GetMasterGID("FPCD");
            string msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");
            string lspayment_mode = "",lspaymet_date="";
            string lsbank_gid = "";
            //Delete journal data 
            string payment_type = "Purchase";
            string lsexchangeratevalue = "", lspayment_amount = "",lstds_amount="",lscal_tds="",lsbranch_gid="";
            double lsexchage_rate = 0.0 , lspayment_amount_L=0.0, lstds_amount_L=0.00, lscal_tds_L=0.00;
            double additional = 0.0, lsadjustment_amount = 0.00;
            msSQL1 = " delete from acc_trn_journalentrydtl where journal_gid from (select journal_gid from " +
                    " acc_trn_journalentry where transaction_date>='"+ lsdate +"' and " +
                    " transaction_gid in(select payment_gid from acp_trn_tpayment where payment_date>='"+ lsdate +"'))";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
            msSQL1 = "delete from acc_trn_journalentry where transaction_date>='"+ lsdate +"' and journal_from='sales'" +
                     " and transaction_gid in(select payment_gid from acp_trn_tpayment where payment_date>='"+ lsdate +"')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

            msSQL = "SELECT payment_gid, date_format(payment_date,'%d-%m-%y')as payment_date, vendor_gid, payment_remarks, " +
                    " payment_mode, bank_name, branch_name, cheque_no, city_name, dd_no, " +
                    " case when currency_code is null then 'INR' else currency_code end as currency_code," +
                    " case when exchange_rate is null then '1' else exchange_rate end as exchange_rate," +
                    " format(payment_total,2) as payment_total, payment_reference,tds_amount,bank_gid," +
                    " tdscalculated_finalamount,payment_from,purchaseorder_gid as reference_gid,addon_amount,additional_discount" +
                    " FROM acp_trn_tpayment where1=1";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            foreach(DataRow dr in dt_datatable.Rows)
            {
                lspayment_mode = dr["payment_mode"].ToString();
                lsbank_gid = dr["bank_gid"].ToString();
                lspaymet_date = dr["payment_date"].ToString();
                lsexchage_rate = double.Parse(dr["exchange_rate"].ToString());
                lspayment_amount = dr["payment_total"].ToString();
                lstds_amount = dr["tds_amount"].ToString();
                lscal_tds = dr["tdscalculated_finalamount"].ToString();

                lspayment_amount_L = double.Parse(lspayment_amount.ToString()) * lsexchage_rate;
                lstds_amount_L = double.Parse(lstds_amount) * lsexchage_rate;
                lscal_tds_L = double.Parse(lscal_tds) * lsexchage_rate;
                journal_date = DateTime.Parse(lsdate.Replace("-", "")).ToString("yyyyMMdd");
            }

        }
        public List<string> journal_function(string invoice_date)
        {
            DateTime finyear = DateTime.MinValue;

            msSQL = "SELECT cast(date_format(fyear_start,'%Y-%m-%d')as char) as fyear_start,finyear_gid FROM" +
                " adm_mst_tyearendactivities ORDER BY finyear_gid DESC LIMIT 1";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                finyear = Convert.ToDateTime(objODBCDataReader["fyear_start"].ToString());
            }
            DateTime invoicedate = Convert.ToDateTime(invoice_date);

            msSQL = " select timestampdiff(Month,'" + finyear.ToString("yyyy-MM-dd") + "','" + invoicedate.ToString("yyyy-MM-dd") + "') as month, " +
                " timestampdiff(day,'" + finyear.ToString("yyyy-MM-dd") + "','" + invoicedate.ToString("yyyy-MM-dd") + "') as day";
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
    }
}