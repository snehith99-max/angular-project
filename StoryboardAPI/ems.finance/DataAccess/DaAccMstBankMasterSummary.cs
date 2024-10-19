using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

namespace ems.finance.DataAccess
{
    public class DaAccMstBankMasterSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable, dt_datatable1;
        string msEmployeeGID, msGetDlGID2, msGetGidOpBe, msGetGid2, lsbankgid, lsbank_gid, lsemployee_gid, lsbranch_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsdub_bankgid, lsdub_bankname;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetBankMasterSummary(string default_flag, MdlAccMstBankMasterSummary values)
        {
            msSQL = " SELECT distinct a.bank_gid, a.bank_code, b.branch_prefix as branch_name, a.bank_name, " +
                    " a.gl_code, a.account_no, a.account_type, a.ifsc_code, a.neft_code, a.swift_code, format(a.openning_balance,2) as openning_balance, a.default_flag as status_flag " +
                    " FROM acc_mst_tbank a " +
                    " left join hrm_mst_tbranch b on b.branch_gid = a.branch_gid " +
                    " where a.default_flag='" + default_flag + "'" +
                    " group by bank_gid asc order by a.bank_gid desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetBankMaster_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetBankMaster_list
                    {
                        bank_gid = dt["bank_gid"].ToString(),
                        bank_code = dt["bank_code"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        bank_name = dt["bank_name"].ToString(),
                        account_type = dt["account_type"].ToString(),
                        account_no = dt["account_no"].ToString(),
                        ifsc_code = dt["ifsc_code"].ToString(),
                        neft_code = dt["neft_code"].ToString(),
                        swift_code = dt["swift_code"].ToString(),
                        openning_balance = dt["openning_balance"].ToString(),
                        status_flag = dt["status_flag"].ToString(),
                    });
                    values.GetBankMaster_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetBankMasterSummaryInactive(string default_flag, MdlAccMstBankMasterSummary values)
        {
            msSQL = " SELECT distinct a.bank_gid, a.bank_code, b.branch_prefix as branch_name, a.bank_name, a.gl_code, a.account_no, a.account_type, a.ifsc_code, a.neft_code, " +
                    " a.swift_code, format(a.openning_balance,2) as openning_balance, a.default_flag as status_flag, date_format(a.updated_date, '%d-%b-%Y') as updated_date " +
                    " FROM acc_mst_tbank a " +
                    " left join hrm_mst_tbranch b on b.branch_gid = a.branch_gid " +
                    " where a.default_flag='" + default_flag + "'" +
                    " group by bank_gid asc order by a.bank_gid desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetBankMaster_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetBankMaster_list
                    {
                        bank_gid = dt["bank_gid"].ToString(),
                        bank_code = dt["bank_code"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        bank_name = dt["bank_name"].ToString(),
                        account_type = dt["account_type"].ToString(),
                        account_no = dt["account_no"].ToString(),
                        ifsc_code = dt["ifsc_code"].ToString(),
                        neft_code = dt["neft_code"].ToString(),
                        swift_code = dt["swift_code"].ToString(),
                        openning_balance = dt["openning_balance"].ToString(),
                        status_flag = dt["status_flag"].ToString(),
                        updated_date = dt["updated_date"].ToString(),
                    });
                    values.GetBankMaster_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccountType(MdlAccMstBankMasterSummary values)
        {
            msSQL = " select account_gid,account_type from acc_mst_taccounttypes ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetAccountType>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAccountType
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_type = dt["account_type"].ToString(),
                    });
                    values.GetAccountType = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccountGroup(MdlAccMstBankMasterSummary values)
        {
            msSQL = " select distinct account_gid, CONCAT(UCASE(substring(account_name,1,1)), LCASE(SUBSTRING(account_name,2))) as account_name " +
                    " from acc_mst_tchartofaccount where has_child='Y' and ledger_type='N' and display_type='Y' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetAccountGroup>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAccountGroup
                    {
                        accountgroup_gid = dt["account_gid"].ToString(),
                        accountgroup_name = dt["account_name"].ToString(),
                    });
                    values.GetAccountGroup = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetBranchName(MdlAccMstBankMasterSummary values)
        {
            msSQL = "select branch_gid, branch_name from hrm_mst_tbranch ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetBranchName>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetBranchName
                    {
                        branch_gid = dt["branch_gid"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                    });
                    values.GetBranchName = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostBankMaster(string user_gid, GetBankMaster_list values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("FATG");
                msGetGid2 = objcmnfunctions.GetMasterGID("FPCC");
                msGetGid1 = objcmnfunctions.GetMasterGID("FPCC");
                msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");

                msSQL = "select branch_name from  hrm_mst_tbranch where branch_gid ='" + values.branch_name + "'";
                string lsbranch_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select accountgroup_name from  acc_mst_tchartofaccount where accountgroup_gid='" + values.accountgroup_name + "'";
                string lsaccountgroup_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT account_no FROM acc_mst_tbank WHERE account_no='" + values.account_no + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = " SELECT bank_name FROM acc_mst_tbank WHERE bank_name='" + values.bank_name + "' ";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Bank name already exist";
                    return;
                }

                if (dt_datatable.Rows.Count == 0)
                {
                    msSQL = " insert into acc_mst_tbank  (" +
                            " bank_gid, " +
                            " bank_name, " +
                            " account_no, " +
                            " account_type, " +
                            " ifsc_code, " +
                            " swift_code, " +
                            " branch_gid, " +
                            " account_gid, " +
                            " default_flag, " +
                            " created_date, " +
                            " created_by, " +
                            " openning_balance)" +
                            " values (" +
                            "'" + msGetGid + "', " +
                            "'" + values.bank_name + "'," +
                            "'" + values.account_no + "'," +
                            "'" + values.account_type + "'," +
                            "'" + values.ifsc_code + "'," +
                            "'" + values.swift_code + "'," +
                            "'" + values.branch_name + "'," +
                            "'" + msGetGid2 + "'," +
                            "'Y'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "'," +
                            "'" + values.openning_balance.Replace(",", "") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msSQL = "select accountgroup_name " +
                             " from  acc_mst_tchartofaccount where  accountgroup_gid = '" + values.accountgroup_name + "'";
                        string accountgroup_name = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = " insert into acc_mst_tchartofaccount   " +
                                " (account_gid, " +
                                " account_name, " +
                                " accountgroup_gid, " +
                                " accountgroup_name," +
                                " ledger_type," +
                                " display_type," +
                                " has_child," +
                                " gl_code," +
                                " Created_By, " +
                                " Created_Date) " +
                                " values (" +
                                "'" + msGetGid2 + "', " +
                                "'" + values.bank_name + "'," +
                                "'" + values.accountgroup_name + "'," +
                                "'" + accountgroup_name + "'," +
                                "'" + 'N' + "'," +
                                "'" + 'Y' + "'," +
                                "'" + 'N' + "'," +
                                "'" + values.bank_name + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        string date1 = values.created_date;
                        string[] components = date1.Split('-');
                        string year = components[0];
                        string month = components[1];
                        string day = components[2];
                        DateTime originalDate = DateTime.ParseExact(values.created_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        string created_Date = originalDate.ToString("yyyy-MM-dd");
                        string lsjournal_refno = objcmnfunctions.GetMasterGID("JRNR");

                        msSQL = " insert into acc_trn_journalentry " +
                                  " (journal_gid, " +
                                  " journal_refno, " +
                                  " transaction_date, " +
                                  " branch_gid," +
                                  " reference_type," +
                                  " reference_gid," +
                                  " transaction_gid, " +
                                  " remarks," +
                                  " journal_year, " +
                                  " journal_month, " +
                                  " journal_day, " +
                                  " created_by, " +
                                  " created_date, " +
                                  " transaction_type)" +
                                    " values (" +
                                    "'" + msGetGid1 + "', " +
                                    "'" + lsjournal_refno + "'," +
                                    "'" + created_Date + "'," +
                                    "'" + values.branch_name + "'," +
                                    "'" + values.bank_name + "'," +
                                    "'" + msGetGid + "'," +
                                    "'" + msGetGid1 + "'," +
                                    "'" + values.remarks.Replace("'", "").Replace(",", "") + "'," +
                                    "'" + day + "'," +
                                    "'" + month + "'," +
                                    "'" + year + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    "'" + "Bank Opening Balance" + "')";
                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult2 != 0)
                        {
                            msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");

                            msSQL = " insert into acc_trn_journalentrydtl " +
                                    " (journaldtl_gid, " +
                                    " journal_gid, " +
                                    " transaction_amount," +
                                    " journal_type, " +
                                    " remarks, " +
                                    " transaction_gid," +
                                    " created_by," +
                                    " created_date," +
                                    " account_gid) " +
                                    " values (" +
                                    "'" + msGetDlGID2 + "', " +
                                    "'" + msGetGid1 + "'," +
                                    "'" + values.openning_balance.Replace(",", "") + "'," +
                                    "'" + "cr" + "'," +
                                    "'" + values.remarks.Replace("'", "").Replace(",", "") + "'," +
                                    "'" + msGetGid + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    "'" + msGetGid2 + "')";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult3 != 0)
                            {
                                msGetGidOpBe = objcmnfunctions.GetMasterGID("OBLC");

                                msSQL = " insert into acc_trn_topeningbalance ( " +
                                        " opening_balance_gid, " +
                                        " entity_gid, " +
                                        " financial_year, " +
                                        " balance_type, " +
                                        " account_gid, " +
                                        " subgroup_account_gid, " +
                                        " group_account_gid, " +
                                        " opening_balance, " +
                                        " created_by, " +
                                        " created_date) " +
                                        " value ( " +
                                        "'" + msGetGidOpBe + "', " +
                                        "'" + values.branch_name + "'," +
                                        "'" + day + "'," +
                                        "'" + "dr" + "'," +
                                        "'" + msGetGid2 + "', " +
                                        "'" + values.accountgroup_name + "'," +
                                        "(select accountgroup_gid from acc_mst_tchartofaccount where account_gid = '" + values.accountgroup_name + "') ," +
                                        "'" + values.openning_balance.Replace(",", "") + "', "+
                                         "'" + user_gid + "'," +
                                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult4 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            if(mnResult4 != 0)
                            {
                                values.status = true;
                                values.message = "Bank Master Added Successfully";
                            }
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Bank Master";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Account No Already Exist !!";
                }
            }
            catch (Exception ex)
            {

                values.status = false;
                values.message = "Error While Adding Bank Master !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Adding Bank Master!! " + " *******" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetBankMasterDetail(string bank_gid, MdlAccMstBankMasterSummary values)
        {
            msSQL = " SELECT distinct d.branch_name,DATE_FORMAT(g.transaction_date, '%d-%m-%Y') AS created_date,e.accountgroup_name,e.accountgroup_gid,a.bank_gid," +
                    " a.bank_code,a.bank_name,a.account_no,a.account_type,a.ifsc_code,a.account_gid," +
                    " a.neft_code,a.swift_code," +
                    " format(a.openning_balance,2) as openning_balance,g.journal_refno,g.remarks,a.branch_gid " +
                    " FROM  acc_mst_tbank a " +
                    " left join hrm_mst_tbranch d on d.branch_gid=a.branch_gid " +
                    " left join acc_trn_journalentry g on g.reference_gid=a.bank_gid  " +
                    " left join acc_mst_tchartofaccount e on a.account_gid=e.account_gid " +
                    " where a.bank_gid = '" + bank_gid + "'  group by a.account_gid";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetEditBankMaster_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEditBankMaster_list
                    {
                        bank_gid = dt["bank_gid"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        accountgroup_gid = dt["accountgroup_gid"].ToString(),
                        bank_code = dt["bank_code"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        bank_name = dt["bank_name"].ToString(),
                        account_type = dt["account_type"].ToString(),
                        account_no = dt["account_no"].ToString(),
                        ifsc_code = dt["ifsc_code"].ToString(),
                        neft_code = dt["neft_code"].ToString(),
                        swift_code = dt["swift_code"].ToString(),
                        openning_balance = dt["openning_balance"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        accountgroup_name = dt["accountgroup_name"].ToString(),
                    });
                    values.GetEditBankMaster_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostBankMasterUpdate(string user_gid, GetEditBankMaster_list values)
        {
            try
            {
                msSQL = "select branch_name from  hrm_mst_tbranch where branch_gid ='" + values.branch_gid + "'";
                string lsbranch_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select account_gid from  acc_mst_tbank where bank_gid='" + values.bank_gid + "'";
                string account_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT bank_name,bank_gid FROM acc_mst_tbank WHERE bank_name='" + values.bank_name + "' ";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows)
                {
                    lsdub_bankgid = objODBCDatareader["bank_gid"].ToString();
                }
                objODBCDatareader.Close();
                if (lsdub_bankgid != values.bank_gid)
                {
                    values.status = false;
                    values.message = "Bank name already exist";
                    return;
                }

                msSQL = "select accountgroup_name from  acc_mst_tchartofaccount where accountgroup_gid='" + values.accountgroup_gid + "'";
                string lsaccountgroup_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select bank_gid from  acc_mst_tbank where account_no='" + values.account_no + "' limit 1 ";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    lsbank_gid = objODBCDatareader["bank_gid"].ToString();
                }
                objODBCDatareader.Close();

                msSQL = "select branch_gid from  hrm_mst_tbranch where branch_name='" + values.branch_name + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows)
                {
                    lsbranch_gid = objODBCDatareader["branch_gid"].ToString();

                }
                objODBCDatareader.Close();

                msSQL = "select bank_gid from  acc_mst_tbank where account_no='" + values.account_no + "' and bank_gid='" + values.bank_gid + "' limit 1 ";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows)
                {
                    lsbankgid = objODBCDatareader["bank_gid"].ToString();
                }
                objODBCDatareader.Close();

                if (lsbank_gid == values.bank_gid)
                {
                    // Parse the original date string using custom date format
                    DateTime originalDate = DateTime.ParseExact(values.created_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    string created_Date = originalDate.ToString("yyyy-MM-dd");
                    msSQL = " UPDATE acc_mst_tbank SET " +
                            " bank_name ='" + values.bank_name + "'," +
                            " account_no ='" + values.account_no + "'," +
                            " account_type ='" + values.account_type + "'," +
                            " ifsc_code ='" + values.ifsc_code + "'," +
                            " swift_code ='" + values.swift_code + "'," +
                            " branch_gid ='" + lsbranch_gid + "'," +
                            " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            " updated_by ='" + user_gid + "'," +
                            " openning_balance ='" + values.openning_balance.Replace(",","") + "' " +
                            " WHERE " +
                            " bank_gid='" + values.bank_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msSQL = " select account_gid " +
                                " from  acc_mst_tchartofaccount where  account_name = '" + values.accountgroup_name + "'";
                        string accountgid = objdbconn.GetExecuteScalar(msSQL);
                        
                        msSQL = " update acc_mst_tchartofaccount set" +
                                " account_name='" + values.bank_name + "'," +
                                " Updated_Date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                " accountgroup_gid='" + accountgid + "'," +
                                " accountgroup_name='" + values.accountgroup_name + "'," +
                                " Updated_By='" + user_gid + "'," +
                                " gl_code='" + values.bank_name + "'" +
                                " where account_gid='" + values.account_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = "select transaction_amount, a.journaldtl_gid, b.journal_gid " +
                                    "from acc_trn_journalentrydtl a " +
                                    "left join acc_trn_journalentry b on a.journal_gid = b.journal_gid " +
                                    "where b.reference_gid = '" + values.bank_gid + "' " +
                                    "and b.transaction_type = 'Bank Opening Balance' " +
                                    "and b.branch_gid = '" + values.branch_gid + "'";
                            objODBCDatareader = objdbconn.GetDataReader(msSQL);

                            if (objODBCDatareader.HasRows)
                            {
                                msSQL = "update acc_trn_journalentrydtl set  remarks = '" + values.remarks.Replace("'", "").Replace(",", "") + "', updated_by = '" + user_gid + "', updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',  transaction_amount = '" + values.openning_balance.Replace(",", "") + "' " +
                                        "where journaldtl_gid = '" + objODBCDatareader["journaldtl_gid"].ToString() + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "update acc_trn_journalentry set transaction_date = '" +
                                        created_Date + "', " +
                                        "branch_gid = '" + values.branch_gid + "', " +
                                        "remarks = '" + values.remarks.Replace("'", "").Replace(",", "") + "', " +
                                        "updated_by = '" + user_gid + "', " +
                                        "updatede_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                        "transaction_gid = '" + objODBCDatareader["journal_gid"].ToString() + "' " +
                                        "where journal_gid = '" + objODBCDatareader["journal_gid"].ToString() + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL= "update acc_trn_topeningbalance set " +
                                       "opening_balance ='" + values.openning_balance.Replace(",", "") + "', " +
                                       "updated_by = '" + user_gid + "', " +
                                       "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                       " where account_gid='" + values.account_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "Bank Master Updated Successfully !! ";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Bank Master ";
                                }
                            }
                            else
                            {
                                // Insert new record into acc_trn_journalentry
                                string msGetGID1 = objcmnfunctions.GetMasterGID("FPCC");
                                if (msGetGID1 == "E")
                                {
                                    values.status = false;
                                    values.message = "Some error occurred while getting gid!!";
                                }
                                string date1 = values.created_date;
                                string[] components = date1.Split('-');
                                string year = components[0];
                                string month = components[1];
                                string day = components[2];

                                msSQL = "insert into acc_trn_journalentry " +
                                        "(journal_gid, transaction_date, branch_gid, " +
                                        "reference_type, reference_gid, transaction_gid, remarks, journal_year, " +
                                        "journal_month, journal_day, created_by, created_date, transaction_type) " +
                                        "values " +
                                        "('" + msGetGID1 + "','" +
                                        created_Date + "', '" +
                                        values.branch_gid + "', '" + values.bank_name + "', '" +
                                        values.bank_gid + "', '" + msGetGID1 + "', '" + values.remarks.Replace("'", "").Replace(",", "") + "', '" +
                                        year + "', '" + month + "', '" + day + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'Bank Opening Balance')";
                                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    string msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");
                                    if (msGetDlGID2 == "E")
                                    {
                                        values.status = false;
                                        values.message = "Some error occurred while getting gid!!";
                                    }

                                    // Insert new record into acc_trn_journalentrydtl
                                    msSQL = "insert into acc_trn_journalentrydtl " +
                                            "(journaldtl_gid, journal_gid, transaction_amount, journal_type, remarks, " +
                                            "transaction_gid, created_by, created_date, account_gid) " +
                                            "values " +
                                            "('" + msGetDlGID2 + "', '" + msGetGID1 + "', '" + values.openning_balance + "', " +
                                            "'cr', '" + values.remarks.Replace("'", "").Replace(",", "") + "', '" + values.bank_gid + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + account_gid + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        values.status = true;
                                        values.message = "Bank Master Updated Successfully !! ";
                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error While Updating Bank Master ";
                                    }
                                }

                            }
                            objODBCDatareader.Close();
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Bank Master ";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Bank Master ";
                    }
                }
                else if (lsbank_gid == lsbankgid)
                {
                    // Parse the original date string using custom date format
                    DateTime originalDate = DateTime.ParseExact(values.created_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    string created_Date = originalDate.ToString("yyyy-MM-dd");

                    msSQL = " UPDATE acc_mst_tbank SET " +
                        " bank_name ='" + values.bank_name + "'," +
                        " account_no ='" + values.account_no + "'," +
                        " account_type ='" + values.account_type + "'," +
                        " ifsc_code ='" + values.ifsc_code + "'," +
                        " swift_code ='" + values.swift_code + "'," +
                        " branch_gid ='" + lsbranch_gid + "'," +
                        " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " updated_by ='" + user_gid + "'," +
                        " openning_balance ='" + values.openning_balance.Replace(",", "") + "' " +
                        " WHERE " +
                        " bank_gid='" + values.bank_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msSQL = "select account_gid " +
                                "from  acc_mst_tchartofaccount where  account_name = '" + values.accountgroup_name + "'";
                        string accountgid = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " update acc_mst_tchartofaccount set" +
                                " account_name='" + values.bank_name + "'," +
                                " Updated_Date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                " accountgroup_gid='" + accountgid + "'," +
                                " accountgroup_name='" + values.accountgroup_name + "'," +
                                " Updated_By='" + user_gid + "'," +
                                " gl_code='" + values.bank_name + "'" +
                                " where account_gid='" + values.account_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = "select transaction_amount, a.journaldtl_gid, b.journal_gid " +
                                          "from acc_trn_journalentrydtl a " +
                                          "left join acc_trn_journalentry b on a.journal_gid = b.journal_gid " +
                                          "where b.reference_gid = '" + values.bank_gid + "' " +
                                          "and b.transaction_type = 'Bank Opening Balance' " +
                                          "and b.branch_gid = '" + values.branch_gid + "'";
                            objODBCDatareader = objdbconn.GetDataReader(msSQL);
                            if (objODBCDatareader.HasRows)
                            {
                                msSQL = "update acc_trn_journalentrydtl set  remarks = '" + values.remarks + "', updated_by = '" + user_gid + "', updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', transaction_amount = '" + values.openning_balance + "' " +
                                        "where journaldtl_gid = '" + objODBCDatareader["journaldtl_gid"].ToString() + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "update acc_trn_journalentry set transaction_date = '" +
                                        created_Date + "', " +
                                         "branch_gid = '" + values.branch_gid + "', " +
                                          "remarks = '" + values.remarks + "', " +
                                          "updated_by = '" + user_gid + "', " +
                                          "updatede_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                        "transaction_gid = '" + objODBCDatareader["journal_gid"].ToString() + "' " +
                                        "where journal_gid = '" + objODBCDatareader["journal_gid"].ToString() + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    values.status = true;
                                    values.message = "Bank Master Updated Successfully !! ";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Bank Master ";
                                }
                            }
                            else
                            {
                                // Insert new record into acc_trn_journalentry
                                string msGetGID1 = objcmnfunctions.GetMasterGID("FPCC");
                                if (msGetGID1 == "E")
                                {
                                    values.status = false;
                                    values.message = "Some error occurred while getting gid!!";
                                }
                                string date1 = values.created_date;
                                string[] components = date1.Split('-');
                                string year = components[0];
                                string month = components[1];
                                string day = components[2];

                                msSQL = "insert into acc_trn_journalentry " +
                                        "(journal_gid, transaction_date, branch_gid, " +
                                        "reference_type, reference_gid, transaction_gid, remarks, journal_year, " +
                                        "journal_month, journal_day, created_by, created_date, transaction_type) " +
                                        "values " +
                                        "('" + msGetGID1 + "','" +
                                        created_Date + "', '" +
                                       values.branch_gid + "', '" + values.bank_name + "', '" +
                                       values.bank_gid + "', '" + msGetGID1 + "', '" + values.remarks + "', '" +
                                        year + "', '" + month + "', '" + day + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'Bank Opening Balance')";
                                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    string msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");
                                    if (msGetDlGID2 == "E")
                                    {
                                        values.status = false;
                                        values.message = "Some error occurred while getting gid!!";
                                    }

                                    // Insert new record into acc_trn_journalentrydtl
                                    msSQL = "insert into acc_trn_journalentrydtl " +
                                            "(journaldtl_gid, journal_gid, transaction_amount, journal_type, remarks, " +
                                            "transaction_gid, created_by, created_date, account_gid) " +
                                            "values " +
                                            "('" + msGetDlGID2 + "', '" + msGetGID1 + "', '" + values.openning_balance + "', " +
                                            "'cr', '" + values.remarks + "', '" + values.bank_gid + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + account_gid + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult == 1)
                                    {
                                        values.status = true;
                                        values.message = "Bank Master Updated Successfully !! ";
                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error While Updating Bank Master ";
                                    }
                                }
                            }
                            objODBCDatareader.Close();
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Bank Master ";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Bank Master ";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Bank with the same Account No already exists !! ";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Updating Bank Master !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Updating Bank Master!! " + " *******" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostBankMasterStatus(string status_flag, string bank_gid, result values)
        {
            try
            {
                msSQL = " UPDATE acc_mst_tbank SET " +
                        " default_flag ='" + status_flag + "', " +
                        " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " WHERE " +
                        " bank_gid='" + bank_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1 && status_flag == "Y")
                {
                    values.status = true;
                    values.message = "Bank Activated Successfully !!";
                }
                else if (mnResult == 1 && status_flag == "N")
                {
                    values.status = true;
                    values.message = "Bank Deactivated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Activating Bank Master Status";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Updating Bank Master !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Updating Bank Master!! " + " *******" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}