using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;
using System.Globalization;

namespace ems.finance.DataAccess
{
    public class DaAccMstOpeningbalance
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        string msSQL2 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsjournal_type, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, maGetGID, lsvendor_code, msUserGid, msGetDlGID, lsparent_gid, msGetGID2, msGetGID1, lsaccount_name, msGetDlGID2;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetOpeningbalance(string entity_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            msSQL = " select b.account_code,format(a.opening_balance, 2) AS transaction_amount, a.financial_year,c.branch_name, b.account_name, b.accountgroup_name,a.opening_balance_gid " +
                    " ,a.account_gid,a.subgroup_account_gid,a.group_account_gid from acc_trn_topeningbalance a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " left join hrm_mst_tbranch c on c.branch_gid = a.entity_gid " +
                    " where b.ledger_type = 'N' and display_type = 'N' " +
                    " and a.financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "' order by a.group_account_gid,subgroup_account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<Openingbalance_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL1 = "select account_gid, accountgroup_gid, upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + dt["account_gid"] + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL1);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        string subgroup_name = objOdbcDataReader["accountgroup_name"].ToString();
                        string accountgroup_gid = objOdbcDataReader["accountgroup_gid"].ToString();

                        msSQL2 = "select upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                        string MainGroup_name = objdbconn.GetExecuteScalar(msSQL2);
                        string sum_of_maingroup;
                        if (MainGroup_name == "$")
                        {
                            MainGroup_name = subgroup_name;
                            subgroup_name = "-";
                            msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where group_account_gid= '" + accountgroup_gid + "' and financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "'";
                            sum_of_maingroup = objdbconn.GetExecuteScalar(msSQL);
                        }
                        else
                        {
                            msSQL = "select accountgroup_gid from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                            string Maingroup_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where group_account_gid= '" + Maingroup_gid + "' and financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "'";
                            sum_of_maingroup = objdbconn.GetExecuteScalar(msSQL);
                        }
                        getModuleList.Add(new Openingbalance_list
                        {
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            openingfinancial_year = dt["financial_year"].ToString(),
                            credit_amount = dt["transaction_amount"].ToString(),
                            opening_balance_gid = dt["opening_balance_gid"].ToString(),
                            account_code = dt["account_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            subgroup_name = subgroup_name,
                            MainGroup_name = MainGroup_name,
                            sum_of_maingroup = sum_of_maingroup,
                        });
                        values.Openingbalance_list = getModuleList;
                    }
                    objOdbcDataReader.Close();
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccMstOpeningbalance(string entity_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            msSQL = " select b.account_code,format(a.opening_balance, 2) AS transaction_amount, a.financial_year,c.branch_name, b.account_name, b.accountgroup_name,a.opening_balance_gid " +
                    " ,a.account_gid,a.subgroup_account_gid,a.group_account_gid from acc_trn_topeningbalance a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " left join hrm_mst_tbranch c on c.branch_gid = a.entity_gid " +
                    " where b.ledger_type = 'N' and display_type = 'Y' " +
                    " and a.financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "' order by a.group_account_gid,subgroup_account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<Openingbalance_lists>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL1 = "select account_gid, accountgroup_gid, upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + dt["account_gid"] + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL1);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        string subgroup_name = objOdbcDataReader["accountgroup_name"].ToString();
                        string accountgroup_gid = objOdbcDataReader["accountgroup_gid"].ToString();

                        msSQL2 = "select upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                        string MainGroup_name = objdbconn.GetExecuteScalar(msSQL2);
                        string sum_of_maingroup;
                        if (MainGroup_name == "$")
                        {
                            MainGroup_name = subgroup_name;
                            subgroup_name = "-";
                            msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where group_account_gid= '" + accountgroup_gid + "' and financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "'";
                            sum_of_maingroup = objdbconn.GetExecuteScalar(msSQL);
                        }
                        else
                        {
                            msSQL = "select accountgroup_gid from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                            string Maingroup_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where group_account_gid= '" + Maingroup_gid + "' and financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "'";
                            sum_of_maingroup = objdbconn.GetExecuteScalar(msSQL);
                        }
                        getModuleList.Add(new Openingbalance_lists
                        {
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            openingfinancial_year = dt["financial_year"].ToString(),
                            debit_amount = dt["transaction_amount"].ToString(),
                            opening_balance_gid = dt["opening_balance_gid"].ToString(),
                            account_code = dt["account_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            subgroup_name = subgroup_name,
                            MainGroup_name = MainGroup_name,
                            sum_of_maingroup = sum_of_maingroup,
                        });
                        values.Openingbalance_lists = getModuleList;
                    }
                    objOdbcDataReader.Close();
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetParentName(MdlAccMstOpeningbalance values)
        {
            msSQL = " SELECT distinct a.account_gid, CONCAT(UPPER(substring(account_name,1,1)),UPPER(SUBSTRING(account_name,2))) as account_name" +
                    " FROM acc_mst_tchartofaccount a  " +
                    " where (ledger_type='N' and display_type='N') and has_child='Y'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var ParentName_List = new List<GetParentName_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    ParentName_List.Add(new GetParentName_List
                    {
                        parent_gid = dt["account_gid"].ToString(),
                        parent_name = dt["account_name"].ToString(),
                    });
                    values.GetParentName_List = ParentName_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetParentNameAsset(MdlAccMstOpeningbalance values)
        {
            msSQL = " SELECT distinct a.account_gid, CONCAT(UPPER(substring(account_name,1,1)),UPPER(SUBSTRING(account_name,2))) as account_name " +
                    " FROM acc_mst_tchartofaccount a " +
                    " where (ledger_type='N' and display_type='Y') and has_child='Y'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var ParentNameasset_List = new List<GetParentNameasset_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    ParentNameasset_List.Add(new GetParentNameasset_List
                    {
                        parent_gid = dt["account_gid"].ToString(),
                        parent_name = dt["account_name"].ToString(),
                    });
                    values.GetParentNameasset_List = ParentNameasset_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetLiabilityAccountName(string user_gid, MdlAccMstOpeningbalance values)
        {
            msSQL = "SELECT distinct a.account_gid,  CONCAT(UPPER(a.accountgroup_name),' - ',UPPER(substring(a.account_name,1,1)), " +
                    " UPPER(SUBSTRING(a.account_name,2))) as account_name " +
                    " FROM acc_mst_tchartofaccount a " +
                    " left join acc_mst_tcreditcard b on b.account_gid=a.account_gid " +
                    " where a.ledger_type='N' and a.display_type='N' and a.has_child='N' and (b.status_flag <> 'N' OR b.status_flag IS NULL) " +
                    " order by account_name ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var liabilityaact_List = new List<Getliabilityacct_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    liabilityaact_List.Add(new Getliabilityacct_List
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.Getliabilityacct_List = liabilityaact_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAssetAccountName(string user_gid, MdlAccMstOpeningbalance values)
        {
            msSQL = " SELECT distinct a.account_gid,  CONCAT(UPPER(a.accountgroup_name),' - ',UPPER(substring(a.account_name,1,1)), " +
                    " UPPER(SUBSTRING(a.account_name,2))) as account_name,b.default_flag " +
                    " FROM acc_mst_tchartofaccount a " +
                    " left join acc_mst_tbank b on b.account_gid = a.account_gid " +
                    " where  (a.ledger_type='N' and a.display_type='Y') and a.has_child='N' " +
                    " and (b.default_flag <> 'N' OR b.default_flag IS NULL) order by account_name ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var assetacct_List = new List<Getassetacct_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    assetacct_List.Add(new Getassetacct_List
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()
                    });
                    values.Getassetacct_List = assetacct_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostOpeningBalance(string user_gid, openingbalance_list values)
        {
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");

            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("OBLC");

                if (values.balance_type == "Liability")
                {
                    lsjournal_type = "cr";
                }
                else
                {
                    lsjournal_type = "dr";
                }

                msSQL = "select opening_balance_gid from acc_trn_topeningbalance where account_gid= '" + values.liabilityaact_name + "' and financial_year = '" + values.finyear + "' and entity_gid = '" + values.branch_name + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Account Already Exists for this Selected Year!!";
                }
                else
                {

                    msSQL = "select accountgroup_gid from acc_mst_tchartofaccount where account_gid= '" + values.liabilityaact_name + "'";
                    string lsSubgroup_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select accountgroup_gid from acc_mst_tchartofaccount where account_gid= '" + lsSubgroup_gid + "'";
                    string lsgroup_gid = objdbconn.GetExecuteScalar(msSQL);
                    if (lsgroup_gid == "$")
                    {
                        lsgroup_gid = lsSubgroup_gid;
                        lsSubgroup_gid = "-";
                    }
                    msSQL = " Insert into acc_trn_topeningbalance " +
                            " (opening_balance_gid, " +
                            " entity_gid, " +
                            " financial_year, " +
                            " balance_type, " +
                            " account_gid, " +
                            " subgroup_account_gid, " +
                            " group_account_gid, " +
                            " opening_balance, " +
                            " account_ref_no, " +
                            " created_by, " +
                            " created_date) " +
                            " values (" +
                            "'" + msGetGid + "', " +
                            "'" + values.branch_name + "', " +
                            "'" + values.finyear + "', " +
                            "'" + lsjournal_type + "', " +
                            "'" + values.liabilityaact_name + "', " +
                            "'" + lsSubgroup_gid + "', " +
                            "'" + lsgroup_gid + "', " +
                            "'" + values.amount_value.Replace(",", "") + "', " +
                            "'" + values.acctref_no + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Opening Balance Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Opening Balance !!";
                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Opening Balance!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetupdateopeningbalance(string user_gid, openingbalanceedit_list values)
        {
            try
            {
                string remarks;
                if (values.editremarks == null || values.editremarks == "")
                {
                    remarks = "";
                }
                else
                {
                    remarks = values.editremarks.Replace("'", "\\\'");
                }

                msSQL = " update acc_trn_topeningbalance set " +
                        " opening_balance = '" + values.editamount_value.Replace(",", "") + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where opening_balance_gid='" + values.opening_balance_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Liability Opening Balance Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Opening Balance !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Source Details";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetassetupdateopeningbalance(string user_gid, assetopeningbalanceedit_list values)
        {
            try
            {
                string remarks;
                if (values.editassetremarks == null || values.editassetremarks == "")
                {
                    remarks = "";
                }
                else
                {
                    remarks = values.editassetremarks.Replace("'", "\\\'");
                }

                msSQL = " update acc_trn_topeningbalance set " +
                        " opening_balance = '" + values.editassetamount_value.Replace(",", "") + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where opening_balance_gid='" + values.opening_balance_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Asset Opening Balance Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Opening Balance !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Source Details";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetEntityDetails(MdlAccMstOpeningbalance values)
        {
            try
            {
                msSQL = " select entity_gid, entity_name " +
                        " from adm_mst_tentity a " +
                        " order by entity_gid asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<entitydtl_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new entitydtl_lists
                        {
                            entity_gid = dt["entity_gid"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                        });
                        values.entitydtl_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLiabilityGroupNameList(string entity_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            try
            {
                msSQL = "select account_gid, account_code, account_name, gl_code, accountgroup_name, case when (ledger_type='N' and display_type='N') " +
                        " then 'Liability' end as display_type, if(has_child='Y','Y','N') as has_child, if(ledger_type='Y','P/L','BS') as ledger_type, " +
                        " IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id " +
                        " from acc_mst_tchartofaccount where ledger_type='N' and display_type='N' and accountgroup_name='$' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<LiabilityGroupName_lists>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string account_gid = dt["account_gid"].ToString();
                        msSQL1 = "SELECT account_gid, account_code, account_name, gl_code, accountgroup_name " +
                                 "FROM acc_mst_tchartofaccount " +
                                 "WHERE accountgroup_gid = '" + account_gid + "'";
                        DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL1);

                        var getSubGroupList = new List<Liabilitysum_GroupName_lists>();
                        string sum_of_group = "0.00";

                        if (dt_datatable1.Rows.Count != 0)
                        {
                            foreach (DataRow dt1 in dt_datatable1.Rows)
                            {
                                string sub_account_gid = dt1["account_gid"].ToString();
                                msSQL2 = "SELECT FORMAT(SUM(opening_balance), 2) " +
                                         "FROM acc_trn_topeningbalance " +
                                         "WHERE account_gid IN (SELECT account_gid FROM acc_mst_tchartofaccount WHERE accountgroup_gid = '" + sub_account_gid + "') and financial_year='" + finyear + "' and entity_gid='" + entity_gid + "'";
                                string temp_sum_of_group = objdbconn.GetExecuteScalar(msSQL2);

                                if (!string.IsNullOrEmpty(temp_sum_of_group))
                                {
                                    sum_of_group = temp_sum_of_group;
                                }

                                getSubGroupList.Add(new Liabilitysum_GroupName_lists
                                {
                                    account_gid = dt1["account_gid"].ToString(),
                                    account_code = dt1["account_code"].ToString(),
                                    account_name = dt1["account_name"].ToString(),
                                    gl_code = dt1["gl_code"].ToString(),
                                    accountgroup_name = dt1["accountgroup_name"].ToString()
                                });
                            }
                        }

                        getModuleList.Add(new LiabilityGroupName_lists
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            ledger_type = dt["ledger_type"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            sum_of_group = sum_of_group,
                        });
                        values.LiabilityGroupName_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetAssetGroupNameList(string entity_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            try
            {
                msSQL = "SELECT account_gid, account_code, account_name, gl_code, accountgroup_name, " +
                        "CASE WHEN (ledger_type='N' AND display_type='N') THEN 'Liability' END AS display_type, " +
                        "IF(has_child='Y','Y','N') AS has_child, " +
                        "IF(ledger_type='Y','P/L','BS') AS ledger_type, " +
                        "IF(accountgroup_gid = '$', NULL, accountgroup_gid) AS group_id " +
                        "FROM acc_mst_tchartofaccount " +
                        "WHERE ledger_type='N' AND display_type='Y' AND accountgroup_name='$'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<AssetGroupName_lists>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string account_gid = dt["account_gid"].ToString();
                        msSQL1 = "SELECT account_gid, account_code, account_name, gl_code, accountgroup_name " +
                                 "FROM acc_mst_tchartofaccount " +
                                 "WHERE accountgroup_gid = '" + account_gid + "'";
                        DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL1);

                        var getSubGroupList = new List<Assetsum_GroupName_lists>();
                        string sum_of_group = "0.00";

                        if (dt_datatable1.Rows.Count != 0)
                        {
                            foreach (DataRow dt1 in dt_datatable1.Rows)
                            {
                                string sub_account_gid = dt1["account_gid"].ToString();
                                msSQL2 = "SELECT FORMAT(SUM(opening_balance), 2) " +
                                         "FROM acc_trn_topeningbalance " +
                                         "WHERE account_gid IN (SELECT account_gid FROM acc_mst_tchartofaccount WHERE accountgroup_gid = '" + sub_account_gid + "') and financial_year='" + finyear + "' and entity_gid='" + entity_gid + "'";

                                string temp_sum_of_group = objdbconn.GetExecuteScalar(msSQL2);
                                if (!string.IsNullOrEmpty(temp_sum_of_group))
                                {
                                    sum_of_group = temp_sum_of_group;
                                }

                                getSubGroupList.Add(new Assetsum_GroupName_lists
                                {
                                    account_gid = dt1["account_gid"].ToString(),
                                    account_code = dt1["account_code"].ToString(),
                                    account_name = dt1["account_name"].ToString(),
                                    gl_code = dt1["gl_code"].ToString(),
                                    accountgroup_name = dt1["accountgroup_name"].ToString()
                                });
                            }
                        }

                        getModuleList.Add(new AssetGroupName_lists
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            ledger_type = dt["ledger_type"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            sum_of_group = sum_of_group,
                        });
                        values.AssetGroupName_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLiabilitySubGroupNameList(string account_gid, string entity_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            try
            {
                msSQL = "select account_gid, account_code, account_name, gl_code, accountgroup_name from acc_mst_tchartofaccount " +
                        " where accountgroup_gid = '" + account_gid + "' ";

                msSQL1 = "select account_gid, account_name from acc_mst_tchartofaccount where account_gid = '" + account_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL1);

                var getModuleList = new List<LiabilitySubGroupName_lists>();
                string sum_of_subgroup = "0.00";
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where account_gid in ( select account_gid from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"] + "') and financial_year='" + finyear + "' and entity_gid='" + entity_gid + "'";
                        sum_of_subgroup = objdbconn.GetExecuteScalar(msSQL);

                        getModuleList.Add(new LiabilitySubGroupName_lists
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            sum_of_subgroup = sum_of_subgroup,
                        });
                        values.LiabilitySubGroupName_lists = getModuleList;
                    }
                }
                var accountList = new List<LiabilitySubGroupName_lists>();
                if (dt_datatable1.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable1.Rows)
                    {
                        accountList.Add(new LiabilitySubGroupName_lists
                        {
                            account_name = dt["account_name"].ToString()
                        });
                    }
                    values.AccountList = accountList;
                }
                dt_datatable.Dispose();
                dt_datatable1.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetAssetSubGroupNameList(string account_gid, string entity_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            try
            {
                msSQL = "select account_gid, account_code, account_name, gl_code, accountgroup_name from acc_mst_tchartofaccount " +
                        " where accountgroup_gid = '" + account_gid + "' ";

                msSQL1 = "select account_gid, account_name from acc_mst_tchartofaccount where account_gid = '" + account_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL1);

                var getModuleList = new List<AssetSubGroupName_lists>();

                string sum_of_subgroup = "0.00";

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where account_gid in ( select account_gid from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"] + "') and financial_year='" + finyear + "' and entity_gid='" + entity_gid + "'";
                        sum_of_subgroup = objdbconn.GetExecuteScalar(msSQL);

                        getModuleList.Add(new AssetSubGroupName_lists
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            sum_of_subgroup = sum_of_subgroup,
                        });
                        values.AssetSubGroupName_lists = getModuleList;
                    }
                }
                var AssetAccountList = new List<AssetSubGroupName_lists>();
                if (dt_datatable1.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable1.Rows)
                    {
                        AssetAccountList.Add(new AssetSubGroupName_lists
                        {
                            account_name = dt["account_name"].ToString()
                        });
                    }
                    values.AssetAccountList = AssetAccountList;
                }
                dt_datatable.Dispose();
                dt_datatable1.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLiabilityLedgerNameList(string account_gid, string entity_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            try
            {
                msSQL = "select a.account_gid, a.account_code, a.account_name, a.gl_code, a.accountgroup_name ,format(b.opening_balance, 2) AS transaction_amount " +
                        " ,b.financial_year,b.opening_balance_gid from acc_mst_tchartofaccount  a " +
                        " left join acc_trn_topeningbalance b on a.account_gid = b.account_gid " +
                        " where a.accountgroup_gid = '" + account_gid + "'  and b.financial_year='" + finyear + "' and b.entity_gid='" + entity_gid + "'";

                msSQL1 = "select account_gid, account_name from acc_mst_tchartofaccount where account_gid = '" + account_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL1);

                var getModuleList = new List<LiabilityLedgerName_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new LiabilityLedgerName_lists
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            credit_amount = dt["transaction_amount"].ToString(),
                            openingfinancial_year = dt["financial_year"].ToString(),
                            opening_balance_gid = dt["opening_balance_gid"].ToString(),
                        });
                        values.LiabilityLedgerName_lists = getModuleList;
                    }
                }

                var LedgerAccountList = new List<LiabilityLedgerName_lists>();
                if (dt_datatable1.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable1.Rows)
                    {
                        LedgerAccountList.Add(new LiabilityLedgerName_lists
                        {
                            account_name = dt["account_name"].ToString()
                        });
                    }

                    values.LedgerAccountList = LedgerAccountList;
                }
                dt_datatable.Dispose();
                dt_datatable1.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetAssetLedgerNameList(string account_gid, string entity_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            try
            {
                msSQL = "select a.account_gid, a.account_code, a.account_name, a.gl_code, a.accountgroup_name, format(b.opening_balance, 2) AS transaction_amount " +
                        " , b.financial_year,b.opening_balance_gid from acc_mst_tchartofaccount  a " +
                        " left join acc_trn_topeningbalance b on a.account_gid = b.account_gid  " +
                        " where a.accountgroup_gid = '" + account_gid + "' and b.financial_year='" + finyear + "'and b.entity_gid='" + entity_gid + "'";

                msSQL1 = "select account_gid, account_name from acc_mst_tchartofaccount where account_gid = '" + account_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL1);

                var getModuleList = new List<AssetLedgerName_lists>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new AssetLedgerName_lists
                        {
                            account_code = dt["account_code"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            debit_amount = dt["transaction_amount"].ToString(),
                            opening_balance_gid = dt["opening_balance_gid"].ToString(),
                        });
                        values.AssetLedgerName_lists = getModuleList;
                    }
                }

                var AssetLedgerAccountList = new List<AssetLedgerName_lists>();

                if (dt_datatable1.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable1.Rows)
                    {
                        AssetLedgerAccountList.Add(new AssetLedgerName_lists
                        {
                            account_name = dt["account_name"].ToString()
                        });
                    }

                    values.AssetLedgerAccountList = AssetLedgerAccountList;
                }
                dt_datatable.Dispose();
                dt_datatable1.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetBranchDetails(MdlAccMstOpeningbalance values)
        {
            try
            {
                msSQL = " select branch_gid, branch_name from hrm_mst_tbranch order by branch_gid asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<branchdtl_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branchdtl_lists
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.branchdtl_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLiabilitySummary(string branch_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            msSQL = "select a.opening_balance_gid,b.account_code, b.account_gid, b.account_name,b.accountgroup_gid, b.accountgroup_name " +
                    " from acc_trn_topeningbalance a " +
                    " left join acc_mst_tchartofaccount b on a.group_account_gid = b.account_gid " +
                    " where b.ledger_type = 'N' and b.display_type = 'N' " +
                    " and a.financial_year = '" + finyear + "' and entity_gid='" + branch_gid + "' group by b.account_name order by a.group_account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<liabilitySummary_lists>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new liabilitySummary_lists
                    {
                        opening_balance_gid = dt["opening_balance_gid"].ToString(),
                        account_code = dt["account_code"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        accountgroup_gid = dt["accountgroup_gid"].ToString(),
                        accountgroup_name = dt["accountgroup_name"].ToString(),
                    });
                    values.liabilitySummary_lists = getModuleList;
                }
            }

            msSQL = "select a.opening_balance_gid,a.account_gid,b.account_name,a.subgroup_account_gid,a.opening_balance,a.account_ref_no " +
                    " from acc_trn_topeningbalance a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " where a.group_account_gid in (select group_account_gid from acc_trn_topeningbalance where ledger_type = 'N' and display_type = 'N' " +
                    " and financial_year = '" + finyear + "' and entity_gid='" + branch_gid + "') and a.financial_year = '" + finyear + "' and a.entity_gid='" + branch_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList1 = new List<liabilitysubled_Summary_lists>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    string Subgroup_gid = dt["subgroup_account_gid"].ToString();
                    string Subgroup_name = string.Empty;

                    if (Subgroup_gid != "-")
                    {
                        msSQL = "select account_name from acc_mst_tchartofaccount where account_gid = '" + Subgroup_gid + "'";
                        Subgroup_name = objdbconn.GetExecuteScalar(msSQL);
                    }
                    else
                    {
                        Subgroup_name = "=";
                    }
                    getModuleList1.Add(new liabilitysubled_Summary_lists
                    {
                        opening_balance_gid = dt["opening_balance_gid"].ToString(),
                        subgroup_account_gid = dt["subgroup_account_gid"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        opening_balance = dt["opening_balance"].ToString(),
                        Subgroup_name = Subgroup_name,
                    });
                    values.liabilitysubled_Summary_lists = getModuleList1;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAssetSummary(string branch_gid, string finyear, MdlAccMstOpeningbalance values)
        {
            msSQL = "select a.opening_balance_gid,b.account_code, b.account_gid, b.account_name,b.accountgroup_gid, b.accountgroup_name " +
                    " from acc_trn_topeningbalance a " +
                    " left join acc_mst_tchartofaccount b on a.group_account_gid = b.account_gid " +
                    " where b.ledger_type = 'N' and display_type = 'Y' " +
                    " and a.financial_year = '" + finyear + "' and entity_gid= '" + branch_gid + "' group by b.account_name order by a.group_account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<AssetSummary_lists>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new AssetSummary_lists
                    {
                        opening_balance_gid = dt["opening_balance_gid"].ToString(),
                        account_code = dt["account_code"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        accountgroup_gid = dt["accountgroup_gid"].ToString(),
                        accountgroup_name = dt["accountgroup_name"].ToString(),
                    });
                    values.AssetSummary_lists = getModuleList;
                }
            }

            msSQL = " select a.opening_balance_gid,a.account_gid,b.account_name,a.subgroup_account_gid,a.opening_balance,a.account_ref_no " +
                    " from acc_trn_topeningbalance a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " where a.group_account_gid in (select group_account_gid from acc_trn_topeningbalance " +
                    " where ledger_type = 'N' and display_type = 'Y' " +
                    " and financial_year = '" + finyear + "' and entity_gid='" + branch_gid + "') and a.financial_year = '" + finyear + "' and a.entity_gid='" + branch_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList1 = new List<liabilitysubled_Summary_lists>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    string Subgroup_gid = dt["subgroup_account_gid"].ToString();
                    string Subgroup_name = string.Empty;

                    if (Subgroup_gid != "-")
                    {
                        msSQL = "select account_name from acc_mst_tchartofaccount where account_gid = '" + Subgroup_gid + "'";
                        Subgroup_name = objdbconn.GetExecuteScalar(msSQL);
                    }
                    else
                    {
                        Subgroup_name = "=";
                    }
                    getModuleList1.Add(new liabilitysubled_Summary_lists
                    {
                        opening_balance_gid = dt["opening_balance_gid"].ToString(),
                        subgroup_account_gid = dt["subgroup_account_gid"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        opening_balance = dt["opening_balance"].ToString(),
                        Subgroup_name = Subgroup_name,
                    });
                    values.liabilitysubled_Summary_lists = getModuleList1;
                }
            }
            dt_datatable.Dispose();
        }
    }
}
