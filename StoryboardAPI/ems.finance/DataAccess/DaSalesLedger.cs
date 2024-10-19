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
    public class DaSalesLedger
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objfinance = new finance_cmnfunction();
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader, objODBCDatareader1;
        DataTable dt_datatable, dt_datatable1;
        string msGetGid, msGetGid1, msGetGid2, mscusconGetGID, msleadbankGetGID, msleadbankconGetGID, msGetGidfil, lsmodule_name, lssalestype_gid, lssalestype_name, lsasset_status, lsproduct_name, lsassetdtl_gid, lssasset_status;
        int mnResult;
        string lsstart_date = "", lsend_date = "";

        public void DaGetsalesLedger(string user_gid, MdlSalesLedger values)
        {
            try
            {
                //msSQL = " SELECT  DATE_FORMAT(a.transaction_date, '%d-%m-%Y') AS invoice_date, "+
                //               " a.journal_refno,a.transaction_gid AS invoice_gid, c.account_gid,e.customer_id, " +
                //               " e.customer_name,a.journal_gid,e.customer_gid, format(d.transaction_amount,2) as transaction_amount, " +
                //               " COALESCE(tax.tax_amount, '0.00') AS tax_amount, COALESCE(net.net_amount, '0.00') AS net_amount, " +
                //               " COALESCE(net.account_name, c.account_name) AS account_name FROM acc_trn_journalentry a " +
                //               " LEFT JOIN acc_trn_journalentrydtl d ON d.journal_gid = a.journal_gid " +
                //               " LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = d.account_gid " +
                //               " LEFT JOIN crm_mst_tcustomer e ON e.customer_gid = a.reference_gid  " +
                //               " LEFT JOIN ( SELECT b.journal_gid,FORMAT(SUM(b.transaction_amount), 2) AS tax_amount " +
                //               " FROM acc_trn_journalentry a " +
                //               " LEFT JOIN acc_trn_journalentrydtl b ON b.journal_gid = a.journal_gid " +
                //               " LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = b.account_gid " +
                //               " WHERE c.ledger_type = 'N' AND c.display_type = 'N' " +
                //               " GROUP BY a.journal_refno) tax ON tax.journal_gid = a.journal_gid " +
                //               " LEFT JOIN (SELECT b.journal_gid,FORMAT((b.transaction_amount), 2) AS net_amount,c.account_name " +
                //               " FROM acc_trn_journalentry a " +
                //               " LEFT JOIN acc_trn_journalentrydtl b ON b.journal_gid = a.journal_gid " +
                //               " LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = b.account_gid " +
                //               " left join acc_mst_accountmapping d on d.account_gid=c.account_gid " +
                //               " WHERE c.ledger_type = 'Y' AND c.display_type = 'Y' and d.field_name='COGS') net ON net.journal_gid = a.journal_gid " +
                //               " WHERE a.journal_from = 'sales' AND d.journal_type = 'dr' AND a.transaction_type = 'Journal' " +
                //               " group by a.journal_refno order by a.transaction_date desc";
                msSQL = "CALL acp_rpt_tsalesledger()";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSaleLedgerfin_list = new List<GetSaleLedgerfin_list>();
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetSaleLedgerfin_list.Add(new GetSaleLedgerfin_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_gid = dt["invoice_gid"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        customer_id = dt["customer_id"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        journal_gid = dt["journal_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        transaction_amount = dt["transaction_amount"].ToString(),
                        tax_amount = dt["tax_amount"].ToString(),
                        net_amount = dt["net_amount"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                    });
                }
                values.GetSaleLedgerfin_list = GetSaleLedgerfin_list;
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }


        public void DaGetsalesLedgerdatefin(string from_date, string to_date, MdlSalesLedger values)
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
                msSQL = " SELECT  DATE_FORMAT(a.transaction_date, '%d-%m-%Y') AS invoice_date, " +
                                " a.journal_refno,a.transaction_gid AS invoice_gid, c.account_gid,e.customer_id, " +
                                " e.customer_name,a.journal_gid,e.customer_gid, format(d.transaction_amount,2) as transaction_amount, " +
                                " COALESCE(tax.tax_amount, '0.00') AS tax_amount, COALESCE(net.net_amount, '0.00') AS net_amount, " +
                                " COALESCE(net.account_name, c.account_name) AS account_name FROM acc_trn_journalentry a " +
                                " LEFT JOIN acc_trn_journalentrydtl d ON d.journal_gid = a.journal_gid " +
                                " LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = d.account_gid " +
                                " LEFT JOIN crm_mst_tcustomer e ON e.customer_gid = a.reference_gid  " +
                                " LEFT JOIN ( SELECT b.journal_gid,FORMAT(SUM(b.transaction_amount), 2) AS tax_amount " +
                                " FROM acc_trn_journalentry a " +
                                " LEFT JOIN acc_trn_journalentrydtl b ON b.journal_gid = a.journal_gid " +
                                " LEFT JOIN acc_mst_tchartofaccount c ON c.account_gid = b.account_gid " +
                                " WHERE c.ledger_type = 'N' AND c.display_type = 'N' " +
                                " GROUP BY b.journal_gid, c.account_name) tax ON tax.journal_gid = a.journal_gid " +
                                " LEFT JOIN (SELECT b.journal_gid,FORMAT(b.transaction_amount, 2) AS net_amount,c.account_name "+
                                " FROM acc_trn_journalentry a " +
                                " LEFT JOIN acc_trn_journalentrydtl b ON b.journal_gid = a.journal_gid"+
                                " LEFT JOIN  acc_mst_tchartofaccount c ON c.account_gid = b.account_gid"+
                                " LEFT JOIN  acc_mst_accountmapping d ON d.account_gid = c.account_gid"+
                                " WHERE  c.ledger_type = 'Y' AND c.display_type = 'Y' AND d.field_name = 'COGS') net ON net.journal_gid = a.journal_gid " +
                                " WHERE a.journal_from = 'sales' AND d.journal_type = 'dr' AND a.transaction_type = 'Journal' and  a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                                " group by a.journal_refno order by a.transaction_date desc";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSaleLedgerfin_list = new List<GetSaleLedgerfin_list>();
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetSaleLedgerfin_list.Add(new GetSaleLedgerfin_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_gid = dt["invoice_gid"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        customer_id = dt["customer_id"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        journal_gid = dt["journal_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        transaction_amount = dt["transaction_amount"].ToString(),
                        tax_amount = dt["tax_amount"].ToString(),
                        net_amount = dt["net_amount"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                    });
                }
                values.GetSaleLedgerfin_list = GetSaleLedgerfin_list;
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
   
}
}