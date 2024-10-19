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
    // Code By snehith
    public class DaChartofAccount
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objODBCDatareader;
        OdbcDataReader objODBCDatareader1;
        DataTable dt_datatable;
        string msEmployeeGID, msGetDlGID2, msGetGid2, lsemployee_gid, lsledger_type, lsaccountgid, lshaschild, lsaccount_gid, lshas_child, lsaccount_name, lsdispaly_type, msAccGetGID, lsentity_code, hasChild, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsaccount_code;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaChartofAccountSummary(MdlChartofAccount values)
        {
            try
            {
                msSQL = " select account_gid, account_code, account_name, gl_code,accountgroup_name,  " +
                        " case when (ledger_type='Y' and display_type='N') then 'Expense' end as display_type, " +
                        " if(has_child='Y','Yes','No') as has_child,if(ledger_type='Y','P/L','BS') as ledger_type, " +
                        " IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id   from acc_mst_tchartofaccount where  ledger_type='Y' and display_type='N'  and accountgroup_name='$'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getchartofaccount_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getchartofaccount_list
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            gl_code = dt["gl_code"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            display_type = dt["display_type"].ToString(),
                            has_child = dt["has_child"].ToString(),
                            ledger_type = dt["ledger_type"].ToString(),
                            group_id = dt["group_id"].ToString(),
                        });
                    }
                    values.Getchartofaccount_list = getModuleList;
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                             " * **********" + ex.Message.ToString() + "***********" + msSQL +
                                             "*******Apiref********", "ErrorLog/Finance " + "Log" +
                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaChartofSubAccountSummary(MdlChartofAccount values, string account_gid)
        {
            try
            {
                msSQL = " select account_gid, account_code, account_name, gl_code, accountgroup_name,accountgroup_gid, display_type, has_child, ledger_type  from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getchartofsubaccount_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getchartofsubaccount_list
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            gl_code = dt["gl_code"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            accountgroup_gid = dt["accountgroup_gid"].ToString(),
                            display_type = dt["display_type"].ToString(),
                            has_child = dt["has_child"].ToString(),
                            ledger_type = dt["ledger_type"].ToString(),
                        });
                        values.Getchartofsubaccount_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaChartofAccountAssetSummary(MdlChartofAccount values)
        {
            try
            {
                msSQL = " select account_gid, account_code, account_name, gl_code,accountgroup_name,  " +
                        " case when (ledger_type='N' and display_type='N') then 'Liability' end as display_type, " +
                        " if(has_child='Y','Yes','No') as has_child,if(ledger_type='Y','P/L','BS') as ledger_type," +
                        " IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id   from acc_mst_tchartofaccount where  ledger_type='N' and display_type='Y'  and accountgroup_name='$'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getchartofaccountasset_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getchartofaccountasset_list
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            gl_code = dt["gl_code"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            display_type = dt["display_type"].ToString(),
                            has_child = dt["has_child"].ToString(),
                            ledger_type = dt["ledger_type"].ToString(),
                            group_id = dt["group_id"].ToString(),
                        });
                    }
                    values.Getchartofaccountasset_list = getModuleList;
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                             " * **********" + ex.Message.ToString() + "***********" + msSQL +
                                             "*******Apiref********", "ErrorLog/Finance " + "Log" +
                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaChartofAccountLiabilitySummary(MdlChartofAccount values)
        {
            try
            {
                msSQL = " select account_gid, account_code, account_name, gl_code,accountgroup_name,  " +
                        " case when (ledger_type='N' and display_type='N') then 'Liability' end as display_type, " +
                        " if(has_child='Y','Yes','No') as has_child,if(ledger_type='Y','P/L','BS') as ledger_type," +
                        " IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id   from acc_mst_tchartofaccount where ledger_type='N' and display_type='N' and accountgroup_name='$'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getchartofaccountliability_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getchartofaccountliability_list
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            gl_code = dt["gl_code"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            display_type = dt["display_type"].ToString(),
                            has_child = dt["has_child"].ToString(),
                            ledger_type = dt["ledger_type"].ToString(),
                            group_id = dt["group_id"].ToString(),
                        });
                    }
                    values.Getchartofaccountliability_list = getModuleList;
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                             " * **********" + ex.Message.ToString() + "***********" + msSQL +
                                             "*******Apiref********", "ErrorLog/Finance " + "Log" +
                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaChartofAccountIncomeSummary(MdlChartofAccount values)
        {
            try
            {
                msSQL = " select account_gid, account_code, account_name, gl_code,accountgroup_name,  " +
                        " case when (ledger_type='Y' and display_type='Y') then 'Income' end as display_type,  " +
                        " if(has_child='Y','Yes','No') as has_child,if(ledger_type='Y','P/L','BS') as ledger_type," +
                        " IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id  from acc_mst_tchartofaccount where  ledger_type='Y' and display_type='Y'   and accountgroup_name='$'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getchartofaccountincome_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getchartofaccountincome_list
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            gl_code = dt["gl_code"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            display_type = dt["display_type"].ToString(),
                            has_child = dt["has_child"].ToString(),
                            ledger_type = dt["ledger_type"].ToString(),
                            group_id = dt["group_id"].ToString(),
                        });
                    }
                    values.Getchartofaccountincome_list = getModuleList;
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                             " * **********" + ex.Message.ToString() + "***********" + msSQL +
                                             "*******Apiref********", "ErrorLog/Finance " + "Log" +
                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteChartofAccount(string account_gid,string account_type, chartstatus_lists values)
        {
            try
            {
                if (account_type == "Main_Group")
                {
                    msSQL = " select case when exists (select 1 from acc_mst_tbank where account_gid in " +
                            " (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in" +
                            " (select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as bank";
                    string bank = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_mst_tcreditcard where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as creditcard";
                    string creditcard = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_trn_topeningbalance where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as opening_balance";
                    string openingbalance = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_trn_journalentrydtl where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as journalentry";
                    string journalentry = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from smr_trn_tsalestype where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as salestype";
                    string salestype = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from pmr_trn_tpurchasetype where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as purchasetype";
                    string purchasetype = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from pmr_mst_tproducttype where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as producttype";
                    string producttype = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_mst_accountmapping where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as account_mapping";
                    string account_mapping = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acp_mst_ttaxsegment where accountgroup_gid = '" + account_gid+ "') then 1 else 0 end as taxsegment";
                    string taxsegment = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acp_mst_ttax where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as tax";
                    string tax = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from hrm_mst_tdepartment where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as department";
                    string department = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from pay_mst_tsalarycomponent where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as salarycomponent";
                    string salarycomponent = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_tmp_tsundryexpensesdtl where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid in(select account_gid from acc_mst_tchartofaccount where accountgroup_gid='" + account_gid + "'))) then 1 else 0 end as sundryexpense";
                    string sundryexpense = objdbconn.GetExecuteScalar(msSQL);

                    if (bank == "1" || creditcard == "1" || openingbalance == "1" ||
                        journalentry == "1" || salestype == "1" || purchasetype == "1" ||
                        producttype == "1" || account_mapping == "1" || taxsegment == "1" ||
                        tax == "1" || department == "1" || salarycomponent == "1" || sundryexpense == "1")
                    {
                        values.status = false;
                        values.message = "Can't Delete Account Group Ledgers are Assigned!!";
                    }
                    else
                    {
                        msSQL = "delete from acc_mst_tchartofaccount where account_gid = '" + account_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Account Group Deleted Successfully!!";
                        }
                    }

                }
                else if (account_type == "Sub_Group")
                {
                    msSQL = "select case when exists (select 1 from acc_mst_tbank where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as bank";
                    string bank = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_mst_tcreditcard where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as creditcard";
                    string creditcard = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_trn_topeningbalance where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as opening_balance";
                    string openingbalance = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_trn_journalentrydtl where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as journalentry";
                    string journalentry = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from smr_trn_tsalestype where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as salestype";
                    string salestype = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from pmr_trn_tpurchasetype where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as purchasetype";
                    string purchasetype = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from pmr_mst_tproducttype where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as producttype";
                    string producttype = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_mst_accountmapping where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as account_mapping";
                    string account_mapping = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acp_mst_ttaxsegment where account_gid = '" + account_gid + "') then 1 else 0 end as taxsegment";
                    string taxsegment = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acp_mst_ttax where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as tax";
                    string tax = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from hrm_mst_tdepartment where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as department";
                    string department = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from pay_mst_tsalarycomponent where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as salarycomponent";
                    string salarycomponent = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select case when exists (select 1 from acc_tmp_tsundryexpensesdtl where account_gid in (select account_gid from acc_mst_tchartofaccount where accountgroup_gid= '" + account_gid + "')) then 1 else 0 end as sundryexpense";
                    string sundryexpense = objdbconn.GetExecuteScalar(msSQL);

                    if (bank == "1" || creditcard == "1" || openingbalance == "1" ||
                        journalentry == "1" || salestype == "1" || purchasetype == "1" ||
                        producttype == "1" || account_mapping == "1" || taxsegment == "1" ||
                        tax == "1" || department == "1" || salarycomponent == "1" || sundryexpense == "1")
                    {
                        values.status = false;
                        values.message = "Can't Delete Account SubGroup Ledgers are Assigned!!";
                    }
                    else
                    {
                        msSQL = "delete from acc_mst_tchartofaccount where account_gid = '" + account_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Account SubGroup Deleted Successfully!!";
                        }
                    }
                }
                else
                {
                    msSQL = "select 1 from acc_mst_tbank where account_gid = '" + account_gid + "'";
                    string bank = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from acc_mst_tcreditcard where account_gid ='" + account_gid + "'";
                    string creditcard = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from acc_trn_topeningbalance where account_gid ='" + account_gid + "'";
                    string openingbalance = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from acc_trn_journalentrydtl where account_gid ='" + account_gid + "'";
                    string journalentry = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from smr_trn_tsalestype where account_gid ='" + account_gid + "'";
                    string salestype = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from pmr_trn_tpurchasetype where account_gid ='" + account_gid + "'";
                    string purchasetype = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from pmr_mst_tproducttype where account_gid ='" + account_gid + "'";
                    string producttype = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from acc_mst_accountmapping where account_gid ='" + account_gid + "'";
                    string account_mapping = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from acp_mst_ttax where account_gid ='" + account_gid + "'";
                    string tax = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from hrm_mst_tdepartment where account_gid ='" + account_gid + "'";
                    string department = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from pay_mst_tsalarycomponent where account_gid ='" + account_gid + "'";
                    string salarycomponent = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select 1 from acc_tmp_tsundryexpensesdtl where account_gid ='" + account_gid + "'";
                    string sundryexpense = objdbconn.GetExecuteScalar(msSQL);

                    if (bank == "1" || creditcard == "1" || openingbalance == "1" ||
                        journalentry == "1" || salestype == "1" || purchasetype == "1" ||
                        producttype == "1" || account_mapping == "1" || tax == "1" || department == "1" 
                        || salarycomponent == "1" || sundryexpense == "1")
                    {
                        values.status = false;
                        values.message = "Can't Delete Assigned Ledger!!";
                    }
                    else
                    {
                        msSQL = "delete from acc_mst_tchartofaccount where account_gid = '" + account_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Account Ledger Deleted Successfully!!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Exception occured while Deleting  Chart of Account!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostAccountGroup(string user_gid, chartaccount_list values)
        {
            try
            {
                if (values.accountType == "PL")
                {
                    lsledger_type = "Y";
                }
                else
                {
                    lsledger_type = "N";
                }

                if (values.displayType == "expense" || values.displayType == "liability")
                {
                    lsdispaly_type = "N"; // Override result to "Y" if displayTypes is "expense"
                }
                else if (values.displayType == "income" || values.displayType == "asset")
                {
                    lsdispaly_type = "Y"; // Otherwise, keep result as "N"
                }
                msAccGetGID = objcmnfunctions.GetMasterGID("FCOA");

                if (msAccGetGID == "E")
                {
                    values.status = false;
                    values.message = "Some error occurred while getting gid!!";
                }

                msSQL = "SELECT account_gid  FROM acc_mst_tchartofaccount WHERE account_name  = '" + values.accountgroup + "'";
                msSQL1 = "Select account_gid from acc_mst_tchartofaccount where account_code = '" + values.accountcode + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                objODBCDatareader1 = objdbconn.GetDataReader(msSQL1);
                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Account Group Name Already Exist!!";
                }
                else if (objODBCDatareader1.HasRows == true)
                {
                    values.status = false;
                    values.message = "Account Code Already Exist!!";
                }
                else
                {
                    msSQL = " INSERT INTO acc_mst_tchartofaccount (" +
                            " account_gid, " +
                            " accountgroup_gid, " +
                            " account_code, " +
                            " gl_code, " +
                            " account_name, " +
                            " accountgroup_name, " +
                            " display_type, " +
                            " ledger_type, " +
                            " Created_Date, " +
                            " Created_By) " +
                            " VALUES (" +
                            "'" + msAccGetGID + "', " +
                            " '$', " +
                            "'" + values.accountcode.Replace("'", " ").Replace("'", " ") + "'," +
                            "'" + values.accountcode.Replace("'", " ").Replace("'", " ") + "'," +
                            "'" + values.accountgroup.Replace("'", " ").Replace("'", " ") + "'," +
                            "'$', " +
                            "'" + lsdispaly_type + "', " +
                            "'" + lsledger_type + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Account Group Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Account Group!!";
                    }
                }
                objODBCDatareader.Close();
                objODBCDatareader1.Close();
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Exception occured while Adding  Account Group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateAccountGroup(string user_gid, chartgroupupdate_list values)
        {
            try
            {
                msSQL = " SELECT account_gid,account_name  FROM acc_mst_tchartofaccount WHERE account_name  = '" + values.account_groupname + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows)
                {
                    lsaccount_gid = objODBCDatareader["account_gid"].ToString();
                    lsaccount_name = objODBCDatareader["account_name"].ToString();
                }
                if (lsaccount_gid == values.account_groupgid)
                {
                    msSQL = " update acc_mst_tchartofaccount set " +
                            " account_name = '" + values.account_groupname + "'," +
                            " account_code = '" + values.account_groupcode + "'," +
                            " Updated_By = '" + user_gid + "'," +
                            " Updated_Date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where account_gid='" + values.account_groupgid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Account Group Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Account Group!!";
                    }
                }
                else
                {
                    if (string.Equals(lsaccount_name, values.account_groupname, StringComparison.OrdinalIgnoreCase))
                    {
                        values.status = false;
                        values.message = "Account Group with the same name already exists !!";
                    }
                    else
                    {
                        msSQL = " update acc_mst_tchartofaccount set " +
                                " account_name = '" + values.account_groupname + "'," +
                                " account_code = '" + values.account_groupcode + "'," +
                                " Updated_By = '" + user_gid + "'," +
                                " Updated_Date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where account_gid='" + values.account_groupgid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Account Group Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Account Group !!";
                        }
                    }
                }
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Account Group Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostAccountSubGroup(string user_gid, chartsubaccount_list values)
        {
            try
            {
                if (values.accountType == "PL")
                {
                    lsledger_type = "Y";
                }
                else
                {
                    lsledger_type = "N";
                }

                if (values.displayType == "expense" || values.displayType == "liability")
                {
                    lsdispaly_type = "N"; // Override result to "Y" if displayTypes is "expense"
                }
                else if (values.displayType == "income" || values.displayType == "asset")
                {
                    lsdispaly_type = "Y"; // Otherwise, keep result as "N"
                }

                msSQL = " select accountgroup_gid from acc_mst_tchartofaccount where account_gid = '" + values.account_gid + "'";
                string subacc_gid = objdbconn.GetExecuteScalar(msSQL);


                if (subacc_gid != "$" && values.ledger_flag == "N")
                {
                    lshas_child = "N";
                }
                else
                {
                    lshas_child = "Y";
                }

                msAccGetGID = objcmnfunctions.GetMasterGID("FCOA");

                if (msAccGetGID == "E")
                {
                    values.status = false;
                    values.message = "Some error occurred while getting gid!!";
                }

                msSQL = "SELECT account_gid  FROM acc_mst_tchartofaccount WHERE account_name  = '" + values.accountsubgroup + "'";
                msSQL1 = "select account_gid from acc_mst_tchartofaccount where account_code = '" + values.accountsubcode + "'";
                objODBCDatareader1 = objdbconn.GetDataReader(msSQL1);
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Account Name Already Exist!!";
                }
                else if (objODBCDatareader1.HasRows == true)
                {
                    values.status = false;
                    values.message = "Account Code Already Exist!!";
                }
                else
                {
                    msSQL = " INSERT INTO acc_mst_tchartofaccount (" +
                            " account_gid, " +
                            " accountgroup_gid, " +
                            " account_code, " +
                            " gl_code, " +
                            " has_child," +
                            " account_name, " +
                            " accountgroup_name, " +
                            " display_type, " +
                            " ledger_type, " +
                            " Created_Date, " +
                            " Created_By) " +
                            " VALUES (" +
                            "'" + msAccGetGID + "', " +
                            "'" + values.account_gid.Replace("'", " ").Replace("'", " ") + "'," +
                            "'" + values.accountsubcode.Replace("'", " ").Replace("'", " ") + "'," +
                            "'" + values.accountsubcode.Replace("'", " ").Replace("'", " ") + "'," +
                            "'" + lshas_child + "'," +
                            "'" + values.accountsubgroup.Replace("'", " ").Replace("'", " ") + "'," +
                            "'" + values.accountgroups.Replace("'", " ").Replace("'", " ") + "'," +
                            "'" + lsdispaly_type + "', " +
                            "'" + lsledger_type + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Account SubGroup Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Account SubGroup!!";
                    }
                }



                objODBCDatareader.Close();
                objODBCDatareader1.Close();
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Exception occured while Deleting  Chart of Account!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateAccountSubGroup(string user_gid, chartsubaccount_list values)
        {
            try
            {
                if (values.accountType == "PL")
                {
                    lsledger_type = "Y";
                }
                else
                {
                    lsledger_type = "N";
                }

                if (values.displayType == "expense" || values.displayType == "liability")
                {
                    lsdispaly_type = "N"; // Override result to "Y" if displayTypes is "expense"
                }
                else if (values.displayType == "income" || values.displayType == "asset")
                {
                    lsdispaly_type = "Y"; // Otherwise, keep result as "N"
                }

                if (values.ledger_flag == "N")
                {
                    lshas_child = "Y";
                }
                else
                {
                    lshas_child = "N";
                }
                msSQL1 = "select account_gid from acc_mst_tchartofaccount where account_code = '" + values.accountsubcode + "'";
                objODBCDatareader1 = objdbconn.GetDataReader(msSQL1);
                if (objODBCDatareader1.HasRows)
                {
                    lsaccount_code = objODBCDatareader1["account_gid"].ToString();
                }
                if (lsaccount_code != values.accountcodes)
                {
                    values.status = false;
                    values.message = "Account Code Already Exist!!";
                }
                else
                {

                    msSQL = " SELECT has_child  FROM acc_mst_tchartofaccount WHERE account_gid  = '" + values.accountcodes + "'";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows)
                    {
                        lshaschild = objODBCDatareader["has_child"].ToString();

                    }
                    objODBCDatareader.Close();

                    msSQL = " SELECT account_gid  FROM acc_mst_tchartofaccount WHERE accountgroup_gid  = '" + values.accountcodes + "'";

                    objODBCDatareader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDatareader.HasRows)
                    {
                        lsaccountgid = objODBCDatareader["account_gid"].ToString();
                    }
                    objODBCDatareader.Close();

                    if (lsaccountgid == "" || lsaccountgid == null)
                    {
                        msSQL = " SELECT account_gid,account_name  FROM acc_mst_tchartofaccount WHERE account_name  = '" + values.accountsubgroup + "'";
                        objODBCDatareader = objdbconn.GetDataReader(msSQL);

                        if (objODBCDatareader.HasRows)
                        {
                            lsaccount_gid = objODBCDatareader["account_gid"].ToString();
                            lsaccount_name = objODBCDatareader["account_name"].ToString();
                        }
                        if (lsaccount_gid == values.accountcodes)
                        {
                            msSQL = " update acc_mst_tchartofaccount set " +
                                    " account_name = '" + values.accountsubgroup + "'," +
                                    " account_code = '" + values.accountsubcode + "'," +
                                    " has_child = '" + lshas_child + "'," +
                                    " Updated_By = '" + user_gid + "'," +
                                    " Updated_Date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where account_gid='" + values.accountcodes + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Account Group Updated Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Account Group!!";
                            }
                        }
                        else
                        {
                            if (string.Equals(lsaccount_name, values.accountsubgroup, StringComparison.OrdinalIgnoreCase))
                            {
                                values.status = false;
                                values.message = "Account Group with the same name already exists !!";
                            }
                            else
                            {
                                msSQL = " update acc_mst_tchartofaccount set " +
                                        " account_name = '" + values.accountsubgroup + "'," +
                                        " account_code = '" + values.accountsubcode + "'," +
                                        " has_child = '" + lshas_child + "'," +
                                        " Updated_By = '" + user_gid + "'," +
                                        " Updated_Date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where account_gid='" + values.accountcodes + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "Account Group Updated Successfully !!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Account Group !!";
                                }
                            }
                        }
                        objODBCDatareader.Close();
                    }

                    else if (lshaschild != values.ledger_flag)
                    {
                        msSQL = " SELECT account_gid,account_name  FROM acc_mst_tchartofaccount WHERE account_name  = '" + values.accountsubgroup + "'";
                        objODBCDatareader = objdbconn.GetDataReader(msSQL);

                        if (objODBCDatareader.HasRows)
                        {
                            lsaccount_gid = objODBCDatareader["account_gid"].ToString();
                            lsaccount_name = objODBCDatareader["account_name"].ToString();
                        }

                        if (lsaccount_gid == values.accountcodes)
                        {
                            msSQL = " update acc_mst_tchartofaccount set " +
                                    " account_name = '" + values.accountsubgroup + "'," +
                                    " account_code = '" + values.accountsubcode + "'," +
                                    " has_child = '" + lshas_child + "'," +
                                    " Updated_By = '" + user_gid + "'," +
                                    " Updated_Date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where account_gid='" + values.accountcodes + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Account Group Updated Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Account Group!!";
                            }
                        }
                        else
                        {
                            if (string.Equals(lsaccount_name, values.accountsubgroup, StringComparison.OrdinalIgnoreCase))
                            {
                                values.status = false;
                                values.message = "Account Group with the same name already exists !!";
                            }
                            else
                            {
                                msSQL = " update acc_mst_tchartofaccount set " +
                                        " account_name = '" + values.accountsubgroup + "'," +
                                        " account_code = '" + values.accountsubcode + "'," +
                                        " has_child = '" + lshas_child + "'," +
                                        " Updated_By = '" + user_gid + "'," +
                                        " Updated_Date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where account_gid='" + values.accountcodes + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "Account Group Updated Successfully !!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Account Group !!";
                                }
                            }
                        }
                        objODBCDatareader.Close();
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Account Group has SubAccount Can't Change as Ledger!!";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Account Group Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaChartofAccountCountList(MdlChartofAccount values)
        {
            try
            {
                string msSQL = @"
                SELECT
                COALESCE(SUM(CASE WHEN ledger_type = 'Y' AND display_type = 'N' then 1 else 0 end), 0) AS expense_totalcount,
                COALESCE(SUM(CASE WHEN ledger_type = 'Y' AND display_type = 'Y' then 1 else 0 end), 0) AS income_totalcount,
                COALESCE(SUM(CASE WHEN ledger_type = 'N' AND display_type = 'N' then 1 else 0 end), 0) AS liability_totalcount,
                COALESCE(SUM(CASE WHEN ledger_type = 'N' AND display_type = 'Y' then 1 else 0 end), 0) AS asset_totalcount
                FROM
                acc_mst_tchartofaccount
                WHERE
                ledger_type IN ('Y', 'N')
                AND display_type IN ('Y', 'N')";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getchartofaccountcount_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getchartofaccountcount_list
                        {
                            expense_totalcount = dt["expense_totalcount"].ToString(),
                            income_totalcount = dt["income_totalcount"].ToString(),
                            liability_totalcount = dt["liability_totalcount"].ToString(),
                            asset_totalcount = dt["asset_totalcount"].ToString(),
                        });
                    }
                    values.Getchartofaccountcount_list = getModuleList;
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaChartofAccountFolderList(MdlFinanceFolders values)
        {
            try
            {
                var getexpensefolders = new List<Folders>();

                msSQL = " select account_gid, account_code, account_name, concat(account_code,' - ',upper(account_name)) as account, gl_code,accountgroup_name, case when (ledger_type='Y' " +
                        " and display_type='N') then 'Expense' end as display_type, if(has_child='Y','Y','N') as has_child, if(ledger_type='Y','P/L','BS') as ledger_type, " +
                        " IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id " +
                        " from acc_mst_tchartofaccount where ledger_type='Y' and display_type='N' and accountgroup_name='$' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable != null)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getexpensefolders.Add(new Folders
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            account = dt["account"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            group_id = dt["group_id"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            has_child = dt["has_child"].ToString(),
                        });
                    }
                    values.parentfolders = getexpensefolders;
                }

                var getincomefolders = new List<Folders>();

                msSQL = " select account_gid, account_code, account_name, concat(account_code,' - ',upper(account_name)) as account, gl_code, accountgroup_name, case when (ledger_type='Y' and display_type='Y') " +
                        " then 'Income' end as display_type,if(has_child='Y','Y','N') as has_child, if(ledger_type='Y','P/L','BS') as ledger_type, " +
                        " IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id " +
                        " from acc_mst_tchartofaccount where ledger_type='Y' and display_type='Y' and accountgroup_name='$' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable != null)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getincomefolders.Add(new Folders
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            account = dt["account"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            group_id = dt["group_id"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            has_child = dt["has_child"].ToString(),
                        });
                    }
                    values.incomefolder = getincomefolders;
                }

                var getassetfolders = new List<Folders>();

                msSQL = " select account_gid, account_code, account_name, concat(account_code,' - ',upper(account_name)) as account, gl_code, accountgroup_name, case when (ledger_type='N' and display_type='N') " +
                        " then 'Asset' end as display_type, if(has_child='Y','Y','N') as has_child, if(ledger_type='Y','P/L','BS') as ledger_type, " +
                        " IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id from acc_mst_tchartofaccount " +
                        " where ledger_type='N' and display_type='Y' and accountgroup_name='$' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable != null)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getassetfolders.Add(new Folders
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            account = dt["account"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            group_id = dt["group_id"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            has_child = dt["has_child"].ToString(),
                        });
                    }
                    values.assetfolder = getassetfolders;
                }

                var getliabilityfolders = new List<Folders>();

                msSQL = " select account_gid, account_code, account_name, concat(account_code,' - ',upper(account_name)) as account, gl_code, accountgroup_name, case when (ledger_type='N' and display_type='N') " +
                        " then 'Liability' end as display_type, if(has_child='Y','Y','N') as has_child, if(ledger_type='Y','P/L','BS') as ledger_type, " +
                        " IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id " +
                        " from acc_mst_tchartofaccount where ledger_type='N' and display_type='N' and accountgroup_name='$' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable != null)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getliabilityfolders.Add(new Folders
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            account = dt["account"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            group_id = dt["group_id"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            has_child = dt["has_child"].ToString(),
                        });
                    }
                    values.liabilityfolder = getliabilityfolders;
                }

                var getsubfolders = new List<Folders>();

                msSQL = "select account_code, account_name, account_gid, concat(account_code,' - ',upper(account_name)) as account, accountgroup_gid, accountgroup_name, has_child from acc_mst_tchartofaccount where accountgroup_gid <> '$'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable != null)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getsubfolders.Add(new Folders
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            account = dt["account"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            accountgroup_gid = dt["accountgroup_gid"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            has_child = dt["has_child"].ToString(),
                        });
                    }
                    values.subfolders = getsubfolders;
                }
                values.status = true;
                values.message = "Success";
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString() + "******APIREF******* websiteCustomer2Whatsapp", "SBSWebsite/Log.txt");
            }
        }
        public void DaUpdateLedger(string user_gid, chartsubaccount_list values)
        {
            msSQL = " update acc_mst_tchartofaccount set " +
                    " has_child = '" + values.ledger_flag + "'," +
                    " Updated_By = '" + user_gid + "'," +
                    " Updated_Date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where account_gid='" + values.accountcodes + "' ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Account Group Updated Successfully !!";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Account Group!!";
            }
        }
    }
}