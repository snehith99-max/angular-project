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
    public class DaAccOpeningBalanceReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        DataTable dt_datatable1;

        string parent_gid = string.Empty;
        string parent_gid1 = string.Empty;
        public void DaOpeningBalanceReportFolderList(string entity_name, string finyear, MdlAccOpeningBalanceReport values)
        {
            try
            {
                var getmoduleliabilityfolders = new List<openingbalanceFolders>();

                msSQL = $@"
                       SELECT 
                        GROUP_CONCAT(CONCAT(""'"", x.accountgroup_gid, ""'"") SEPARATOR ',') AS accountgroup_gids
                       FROM (
                           SELECT 
                               c.accountgroup_gid, 
                               c.accountgroup_name
                           FROM 
                               acc_trn_journalentrydtl a
                           LEFT JOIN 
                               acc_trn_journalentry b ON a.journal_gid = b.journal_gid
                           LEFT JOIN 
                               acc_mst_tchartofaccount c ON a.account_gid = c.account_gid
                           WHERE 
                               a.transaction_amount <> 0.00 
                               AND c.ledger_type = 'N' 
                               AND c.display_type = 'N'
                               AND b.branch_gid = '{entity_name}'
                               AND b.openingfinancial_year = '{finyear}' 

                           GROUP BY 
                               c.accountgroup_gid
                       ) x;";
                parent_gid = objdbconn.GetExecuteScalar(msSQL);

                if (!string.IsNullOrEmpty(parent_gid))
                {
                    msSQL = $@" SELECT 
                          x.accountgroup_gid as account_gid, 
                          x.accountgroup_name as account_name, 
                          FORMAT(COALESCE(y.credit_amount, 0), '#,##0.00') AS credit_amount, 
                          FORMAT(COALESCE(z.debit_amount, 0), '#,##0.00') AS debit_amount
                        FROM(
                          SELECT
                            c.accountgroup_gid,
                            c.accountgroup_name
                          FROM
                            acc_trn_journalentrydtl a
                          LEFT JOIN
                            acc_trn_journalentry b ON a.journal_gid = b.journal_gid
                          LEFT JOIN
                            acc_mst_tchartofaccount c ON a.account_gid = c.account_gid
                          WHERE
                            (transaction_amount<> 0.00)
                            AND(c.ledger_type = 'N' AND display_type = 'N')
                            AND b.branch_gid = '{entity_name}'
                            AND  b.openingfinancial_year = '{finyear}' 
                          GROUP BY
                            c.accountgroup_gid
                                        ) x
                                        LEFT JOIN(
                                        SELECT
                            c.accountgroup_gid,
                            SUM(CASE
                              WHEN a.journal_type = 'cr' THEN a.transaction_amount
                              ELSE 0.00
                            END) AS credit_amount
                          FROM
                            acc_trn_journalentrydtl a
                          LEFT JOIN
                            acc_trn_journalentry b ON a.journal_gid = b.journal_gid
                          LEFT JOIN
                            acc_mst_tchartofaccount c ON a.account_gid = c.account_gid
                          WHERE
                            a.transaction_amount<> 0.00
                            AND c.ledger_type = 'N'
                            AND c.display_type = 'N'
                            AND c.accountgroup_gid IN ({parent_gid})
                            AND b.branch_gid = '{entity_name}'
                            AND  b.openingfinancial_year = '{finyear}' 
                          GROUP BY
                            c.accountgroup_gid
                        ) y ON x.accountgroup_gid = y.accountgroup_gid
                        LEFT JOIN(
                          SELECT
                            c.accountgroup_gid,
                            SUM(CASE
                              WHEN a.journal_type = 'dr' THEN a.transaction_amount
                              ELSE 0.00
                            END) AS debit_amount
                          FROM
                            acc_trn_journalentrydtl a
                          LEFT JOIN
                            acc_trn_journalentry b ON a.journal_gid = b.journal_gid
                          LEFT JOIN
                            acc_mst_tchartofaccount c ON a.account_gid = c.account_gid
                          WHERE
                            a.transaction_amount<> 0.00
                            AND c.ledger_type = 'N'
                            AND c.display_type = 'N'
                            AND c.accountgroup_gid IN  ({parent_gid})
                            AND b.branch_gid = '{entity_name}'
                            AND  b.openingfinancial_year = '{finyear}' 
                          GROUP BY
                            c.accountgroup_gid
                        ) z ON x.accountgroup_gid = z.accountgroup_gid";

                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable != null)
                    {
                        foreach (DataRow dr in dt_datatable.Rows)
                        {
                            getmoduleliabilityfolders.Add(new openingbalanceFolders
                            {
                                account_gid = dr["account_gid"].ToString(),
                                account_name = dr["account_name"].ToString(),
                                sum_credit = dr["credit_amount"].ToString(),
                                sum_debit = dr["debit_amount"].ToString(),
                            });
                        }
                    }
                    values.openingbalanceFolders = getmoduleliabilityfolders;
                    dt_datatable.Dispose();
                    //second_list
                    var getmoduleliabilitysubfolders = new List<openingbalanceFolders>();
                    foreach (var parentgid in values.openingbalanceFolders)
                    {
                        msSQL1 = $@"
                            SELECT 
                            account_gid, 
                            account_name,
                            accountgroup_gid, 
                            FORMAT(credit_amount, '#,##0.00') AS credit_amount, 
                            FORMAT(debit_amount, '#,##0.00') AS debit_amount 
                        FROM (
                            SELECT 
                                c.account_gid,  
                                c.account_name, 
                                c.accountgroup_gid, 
                                CAST(SUM(CASE 
                                            WHEN a.journal_type='cr'  THEN a.transaction_amount 
                                            ELSE 0.00 
                                        END) AS decimal) AS credit_amount, 
                                CAST(SUM(CASE 
                                            WHEN a.journal_type='dr'  THEN a.transaction_amount 
                                            ELSE 0.00 
                                        END) AS decimal) AS debit_amount 
                            FROM acc_trn_journalentrydtl a  
                            LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid 
                            LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid 
                            WHERE (transaction_amount <> 0.00) 
                                  AND (c.ledger_type = 'N' AND display_type = 'N') 
                                  AND c.accountgroup_gid != '$' 
                                  AND c.accountgroup_gid = '{parentgid.account_gid}'  
                                  AND b.branch_gid = '{entity_name}' 
                                  AND  b.openingfinancial_year = '{finyear}'   
                            GROUP BY c.account_gid 
                        ) x 
                        ORDER BY  account_gid ASC;
                        ";

                        dt_datatable1 = objdbconn.GetDataTable(msSQL1);
                        if (dt_datatable1 != null)
                        {
                            foreach (DataRow dr in dt_datatable1.Rows)
                            {
                                getmoduleliabilitysubfolders.Add(new openingbalanceFolders
                                {
                                    account_gid = dr["account_gid"].ToString(),
                                    account_name = dr["account_name"].ToString(),
                                    sum_credit = dr["credit_amount"].ToString(),
                                    sum_debit = dr["debit_amount"].ToString(),
                                    accountgroup_gid = dr["accountgroup_gid"].ToString()
                                });
                            }
                        }

                        values.openingbalanceSubFolders = getmoduleliabilitysubfolders;
                    }
                    dt_datatable1.Dispose();
                }
                //Third_sub
                var getmoduleAssetfolders = new List<openingbalanceFolders>();
                msSQL = $@"
                       SELECT 
                        GROUP_CONCAT(CONCAT(""'"", x.accountgroup_gid, ""'"") SEPARATOR ',') AS accountgroup_gids
                       FROM (
                           SELECT 
                               c.accountgroup_gid, 
                               c.accountgroup_name
                           FROM 
                               acc_trn_journalentrydtl a
                           LEFT JOIN 
                               acc_trn_journalentry b ON a.journal_gid = b.journal_gid
                           LEFT JOIN 
                               acc_mst_tchartofaccount c ON a.account_gid = c.account_gid
                           WHERE 
                               a.transaction_amount <> 0.00 
                               AND c.ledger_type = 'N' 
                               AND c.display_type = 'Y'
                               AND b.branch_gid = '{entity_name}'
                               AND  b.openingfinancial_year = '{finyear}'  

                           GROUP BY 
                               c.accountgroup_gid
                       ) x;";
                parent_gid1 = objdbconn.GetExecuteScalar(msSQL);

                if (!string.IsNullOrEmpty(parent_gid1))
                {
                    msSQL =
                          $@" SELECT 
                          x.accountgroup_gid as account_gid, 
                          x.accountgroup_name as account_name, 
                          FORMAT(COALESCE(y.credit_amount, 0), '#,##0.00') AS credit_amount, 
                          FORMAT(COALESCE(z.debit_amount, 0), '#,##0.00') AS debit_amount
                        FROM(
                          SELECT
                            c.accountgroup_gid,
                            c.accountgroup_name
                          FROM
                            acc_trn_journalentrydtl a
                          LEFT JOIN
                            acc_trn_journalentry b ON a.journal_gid = b.journal_gid
                          LEFT JOIN
                            acc_mst_tchartofaccount c ON a.account_gid = c.account_gid
                          WHERE
                            (transaction_amount<> 0.00)
                            AND(c.ledger_type = 'N' AND display_type = 'Y')
                            AND b.branch_gid = '{entity_name}'
                            AND b.openingfinancial_year = '{finyear}'  
                          GROUP BY
                            c.accountgroup_gid
                                        ) x
                                        LEFT JOIN(
                                        SELECT
                            c.accountgroup_gid,
                            SUM(CASE
                              WHEN a.journal_type = 'cr' THEN a.transaction_amount
                              ELSE 0.00
                            END) AS credit_amount
                          FROM
                            acc_trn_journalentrydtl a
                          LEFT JOIN
                            acc_trn_journalentry b ON a.journal_gid = b.journal_gid
                          LEFT JOIN
                            acc_mst_tchartofaccount c ON a.account_gid = c.account_gid
                          WHERE
                            a.transaction_amount<> 0.00
                            AND c.ledger_type = 'N'
                            AND c.display_type = 'Y'
                            AND c.accountgroup_gid IN ({parent_gid1})
                            AND b.branch_gid = '{entity_name}'
                            AND b.openingfinancial_year = '{finyear}'  
                          GROUP BY
                            c.accountgroup_gid
                        ) y ON x.accountgroup_gid = y.accountgroup_gid
                        LEFT JOIN(
                          SELECT
                            c.accountgroup_gid,
                            SUM(CASE
                              WHEN a.journal_type = 'dr' THEN a.transaction_amount
                              ELSE 0.00
                            END) AS debit_amount
                          FROM
                            acc_trn_journalentrydtl a
                          LEFT JOIN
                            acc_trn_journalentry b ON a.journal_gid = b.journal_gid
                          LEFT JOIN
                            acc_mst_tchartofaccount c ON a.account_gid = c.account_gid
                          WHERE
                            a.transaction_amount<> 0.00
                            AND c.ledger_type = 'N'
                            AND c.display_type = 'Y'
                            AND c.accountgroup_gid IN  ({parent_gid1})
                            AND b.branch_gid = '{entity_name}'
                            AND b.openingfinancial_year = '{finyear}'  
                          GROUP BY
                            c.accountgroup_gid
                        ) z ON x.accountgroup_gid = z.accountgroup_gid";
                    ;
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable != null)
                    {
                        foreach (DataRow dr in dt_datatable.Rows)
                        {
                            getmoduleAssetfolders.Add(new openingbalanceFolders
                            {
                                account_gid = dr["account_gid"].ToString(),
                                account_name = dr["account_name"].ToString(),
                                sum_credit = dr["credit_amount"].ToString(),
                                sum_debit = dr["debit_amount"].ToString(),
                            });
                        }
                    }
                    values.openingbalanceAssetFolders = getmoduleAssetfolders;
                    dt_datatable.Dispose();
                    //Fourth_List
                    var getmoduleAssetsubfolders = new List<openingbalanceFolders>();

                    foreach (var parentgid in values.openingbalanceAssetFolders)
                    {
                        msSQL1 = $@"SELECT 
                            account_gid, 
                            account_name,
                            accountgroup_gid, 
                            FORMAT(credit_amount, '#,##0.00') AS credit_amount, 
                            FORMAT(debit_amount, '#,##0.00') AS debit_amount 
                        FROM (
                            SELECT 
                                c.account_gid, 
                                c.account_name, 
                                c.accountgroup_gid, 
                                CAST(SUM(CASE 
                                            WHEN a.journal_type='cr' THEN a.transaction_amount 
                                            ELSE 0.00 
                                        END) AS decimal) AS credit_amount, 
                                CAST(SUM(CASE 
                                            WHEN a.journal_type='dr' THEN a.transaction_amount 
                                            ELSE 0.00 
                                        END) AS decimal) AS debit_amount 
                            FROM acc_trn_journalentrydtl a  
                            LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid 
                            LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid 
                            WHERE (transaction_amount <> 0.00) 
                                  AND (c.ledger_type = 'N' AND display_type = 'Y') 
                                  AND c.accountgroup_gid != '$' 
                                  AND c.accountgroup_gid = '{parentgid.account_gid}'  
                                  AND b.branch_gid = '{entity_name}' 
                                  AND b.openingfinancial_year = '{finyear}'  
                            GROUP BY c.account_gid 
                        ) x 
                        ORDER BY account_gid ASC;
                        ";

                        dt_datatable1 = objdbconn.GetDataTable(msSQL1);
                        if (dt_datatable1 != null)
                        {
                            foreach (DataRow dr in dt_datatable1.Rows)
                            {
                                getmoduleAssetsubfolders.Add(new openingbalanceFolders
                                {
                                    account_gid = dr["account_gid"].ToString(),
                                    account_name = dr["account_name"].ToString(),
                                    sum_credit = dr["credit_amount"].ToString(),
                                    sum_debit = dr["debit_amount"].ToString(),
                                    accountgroup_gid = dr["accountgroup_gid"].ToString()
                                });
                            }
                        }
                        values.openingbalanceAssetSubFolders = getmoduleAssetsubfolders;
                    }
                    dt_datatable1.Dispose();
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString() + "******APIREF******* websiteCustomer2Whatsapp", "SBSWebsite/Log.txt");
            }
        }
    }
}
