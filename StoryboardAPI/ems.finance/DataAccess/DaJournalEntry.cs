using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;

namespace ems.finance.DataAccess
{
    public class DaJournalEntry
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        string msEmployeeGID, msGetDlGID2, msGetGid2, lsemployee_gid, lsentity_code, final_path, lsbranch_name, lssjournal_type, lsamount, lsvoucher_refno, lsjournal_refno, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        int mnCtr;
        double lscredit, lsdebit;


        public void DaGetJournalEntrySummary(MdlJournalEntry values)
        {
            msSQL = " select distinct a.journal_gid,a.transaction_type,a.journal_refno,DATE_FORMAT(a.transaction_date, '%d-%m-%Y') as transaction_date,a.remarks " +
                    " from acc_trn_journalentry a  where invoice_flag='Y' and journal_gid in (select journal_gid from acc_trn_journalentrydtl) " +
                    " order by a.transaction_date desc,a.journal_gid desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetJournalEntry_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetJournalEntry_list
                    {
                        journal_gid = dt["journal_gid"].ToString(),
                        transaction_type = dt["transaction_type"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                        transaction_date = dt["transaction_date"].ToString(),
                        remarks = dt["remarks"].ToString(),

                    });
                    values.GetJournalEntry_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetJournalEntryTransaction(string journal_gid, MdlJournalEntry values)
        {
            try
            {
                msSQL = " SELECT e.account_name as voucher_type,b.remarks, b.journaldtl_gid,(case  " +
                         " when b.journal_type='dr' then '0.00' when b.journal_type='cr' then  " +
                         " format(b.transaction_amount,2) end) " +
                         " as credit_amount,(case when b.journal_type='cr' then '0.00' when b.journal_type='dr' " +
                         " then format(b.transaction_amount,2) end) as debit_amount,b.journal_gid,b.account_gid " +
                         " FROM acc_trn_journalentrydtl b " +
                         " left join acc_mst_tchartofaccount e on e.account_gid=b.account_gid " +
                         " where b.journal_gid ='" + journal_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetJournalTransaction_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetJournalTransaction_list
                        {

                            voucher_type = dt["voucher_type"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            journaldtl_gid = dt["journaldtl_gid"].ToString(),
                            credit_amount = dt["credit_amount"].ToString(),
                            debit_amount = dt["debit_amount"].ToString(),
                            journal_gid = dt["journal_gid"].ToString(),
                            account_gid = dt["account_gid"].ToString(),

                        });
                        values.GetJournalTransaction_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + msSQL + "*******Apiref********", "ErrorLog/Finance " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetJournalEntrySummarys(MdlJournalEntry values)
        {
            try
            {
                var getJournalEntryLists = new List<GetJournalEntry_lists>();

                string msSQL = "CALL GetJournalEntrySummary()";

                DataTable dtJournalEntries = objdbconn.GetDataTable(msSQL);

                if (dtJournalEntries.Rows.Count > 0)
                {
                    var journalEntryDict = new Dictionary<string, GetJournalEntry_lists>();

                    string msSQL1 = @"
                                SELECT 
                                account_gid, 
                                accountgroup_gid, 
                                accountgroup_name 
                                FROM acc_mst_tchartofaccount";
                    DataTable dtAccountGroups = objdbconn.GetDataTable(msSQL1);


                    var subgroupNameDict = new Dictionary<string, string>();
                    var mainGroupNameDict = new Dictionary<string, string>();


                    foreach (DataRow row in dtAccountGroups.Rows)
                    {
                        string account_gid = row["account_gid"].ToString();
                        string accountgroup_gid = row["accountgroup_gid"].ToString();
                        string accountgroup_name = row["accountgroup_name"].ToString();


                        if (!subgroupNameDict.ContainsKey(account_gid))
                        {
                            subgroupNameDict[account_gid] = accountgroup_name;
                        }

                        if (!mainGroupNameDict.ContainsKey(accountgroup_gid))
                        {
                            var mainGroupRow = dtAccountGroups.AsEnumerable()
                                .FirstOrDefault(r => r["account_gid"].ToString() == accountgroup_gid);

                            string mainGroupName = mainGroupRow != null ? mainGroupRow["accountgroup_name"].ToString() : null;

                            if (mainGroupName == "$")
                            {
                                mainGroupName = accountgroup_name;
                            }

                            mainGroupNameDict[accountgroup_gid] = mainGroupName;
                        }
                    }

                    foreach (DataRow row in dtJournalEntries.Rows)
                    {
                        string journalGid = row["journal_gid"].ToString();

                        string account_gid = row["account_gid"].ToString();
                        string accountgroup_gid = row["accountgroup_gid"].ToString();

                        string subgroup_name = subgroupNameDict.ContainsKey(account_gid) ? subgroupNameDict[account_gid] : "-";
                        string mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";


                        if (mainGroup_name == "$")
                        {
                            mainGroup_name = subgroup_name;
                            subgroup_name = "-";
                        }

                        if (!journalEntryDict.ContainsKey(journalGid))
                        {
                            journalEntryDict[journalGid] = new GetJournalEntry_lists
                            {
                                journal_gid = journalGid,
                                transaction_type = row["transaction_type"].ToString(),
                                journal_refno = row["journal_refno"].ToString(),
                                transaction_date = row["transaction_date"].ToString(),
                                remarks = row["entry_remarks"].ToString(),
                                document_path = row["document_path"].ToString(),
                                subgroup_name = subgroup_name,
                                MainGroup_name = mainGroup_name,
                                GetJournalTransactions_list = new List<GetJournalTransactions_list>() // Initialize the list
                            };
                        }

                        journalEntryDict[journalGid].GetJournalTransactions_list.Add(new GetJournalTransactions_list
                        {
                            voucher_type = row["voucher_type"].ToString(),
                            remarks = row["detail_remarks"].ToString(),
                            journaldtl_gid = row["journaldtl_gid"].ToString(),
                            credit_amount = row["credit_amount"].ToString(),
                            debit_amount = row["debit_amount"].ToString(),
                            total_credit_amount = row["total_credit_amount"].ToString(),
                            total_debit_amount = row["total_debit_amount"].ToString(),
                            journal_gid = row["detail_journal_gid"].ToString(),
                            account_gid = row["account_gid"].ToString()
                        });
                    }

                    // Convert the list to an array before assigning
                    foreach (var entry in journalEntryDict.Values)
                    {
                        entry.GetJournalTransactions_list = entry.GetJournalTransactions_list.ToList();
                    }

                    getJournalEntryLists = journalEntryDict.Values.ToList();
                    values.GetJournalEntry_lists = getJournalEntryLists;
                }
            }
            catch (Exception ex)
            {
                // Log error details
                string errorMessage = $"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name} - {ex.Message} - SQL: {msSQL} - ApiRef";
                objcmnfunctions.LogForAudit(errorMessage, "ErrorLog/FinanceLog" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }        
        public void DaGetAccountGroupDetails(MdlJournalEntry values)
        {
            msSQL = " select account_gid,account_name  from " +
                    " acc_mst_tchartofaccount where accountgroup_gid='$' and accountgroup_name='$' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetAccountGroupDetails>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAccountGroupDetails
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                    });
                    values.GetAccountGroupDetails = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccountNameDetails(MdlJournalEntry values)
        {
            msSQL = " select account_gid,account_name  from  " +
                    " acc_mst_tchartofaccount where has_child='N' order by account_name";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetAccountNameDetails>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAccountNameDetails
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                    });
                    values.GetAccountNameDetails = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccountNameBaseonGroup(string accountGroup, MdlJournalEntry values)
        {
            msSQL = "select account_gid,account_name  from  " +
                     " acc_mst_tchartofaccount where accountgroup_gid ='" + accountGroup + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetAccountNameDetails>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAccountNameDetails
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                    });
                    values.GetAccountNameDetails = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostJournalEntry(postjournal_list values, string user_gid)
        {
            try
            {
                string date1 = values.created_date;
                string[] components = date1.Split('-');
                string journal_day = components[0];
                string journal_month = components[1];
                string journal_year = components[2];
                string created_Date = journal_year + "-" + journal_month + "-" + journal_day;


                msSQL = "select branch_name from  hrm_mst_tbranch where branch_gid='" + values.branch_name + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    lsbranch_name = objODBCDatareader["branch_name"].ToString();
                }
                objODBCDatareader.Close();
                lsjournal_refno = objcmnfunctions.GetMasterGID("JRNR");
                lsvoucher_refno = objcmnfunctions.GetMasterGID("FPCC");

                msSQL = "Insert into acc_trn_journalentry (journal_gid, journal_refno, transaction_code, transaction_date, remarks, transaction_type, reference_type, reference_gid, transaction_gid, journal_year, journal_month, journal_day, created_by, created_date, branch_gid) " +
                        "values ('" + lsvoucher_refno + "', '" + lsjournal_refno + "', 'JL001', '" + created_Date + "', '" + values.remarks.Replace("'", "").Replace(",", "") + "', 'Journal', '" + lsbranch_name + "', '" + values.branch_name + "', '" + lsvoucher_refno.Replace("'", "") + "', '" + journal_year + "', '" + journal_month + "', '" + journal_day + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + values.branch_name + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error While Adding Journal Entry !!";
                }
                else
                {

                    for (int i = 0; i < values.details.ToArray().Length; i++)
                    {

                        string msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                        if (msGetDlGID == "E")
                        {
                            values.status = false;
                            values.message = "Some error occurred while getting gid!!";
                        }

                        else
                        {
                            if (values.details[i].creditAmount != "0.00")
                            {
                                lsamount = values.details[i].creditAmount;
                                lssjournal_type = "cr";
                            }
                            else
                            {
                                lsamount = values.details[i].debitAmount;
                                lssjournal_type = "dr";
                            }
                            msSQL = "Insert into acc_trn_journalentrydtl (journaldtl_gid, journal_gid, account_gid, remarks, journal_type, created_by, created_date, transaction_amount) " +
                                    "values ('" + msGetDlGID + "', '" + lsvoucher_refno + "', '" + values.details[i].accountName + "', '" + values.details[i].particulars.Replace("'", "").Replace(",", "") + "', '" + lssjournal_type + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + lsamount + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Journal Entry Added Successfully !! ";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Adding Journal Entry !! ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Adding Jounral Entry !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Adding Journal Entry!! " + " *******" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }        
        public void DaPostJournalEntryWithDoc(HttpRequest httpRequest, results values, string user_gid)
        {
            try
            {
                HttpFileCollection httpFileCollection;
                HttpPostedFile httpPostedFile;
                string branch = httpRequest.Form[0];
                string created = httpRequest.Form[1];
                string remarks = httpRequest.Form[2];
                string details = httpRequest.Form[3];
                List<details> details_list = JsonConvert.DeserializeObject<List<details>>(details);

                httpFileCollection = httpRequest.Files;
                string lsfilepath = string.Empty;
                string document_gid = string.Empty;
                string lspath, lspath1, lspath2;
                string lscompany_code = string.Empty;
                string base64String = string.Empty;
                string httpsUrl = string.Empty;

                msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Journal/Document/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int j = 0; j < httpFileCollection.Count; j++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[j];
                        string FileExtension = httpPostedFile.FileName;
                        //string lsfile_gid = msdocument_gid + FileExtension;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Journal/Document/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        //string local_path = "E:/Angular15/AngularUI/src";
                        ms.Close();
                        lspath = "/assets/media/images/erpdocument" + "/" + lscompany_code + "/" + "Journal/Document/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        string final_path = lspath + msdocument_gid + FileExtension;

                        string date1 = created;
                        string[] components = date1.Split('-');
                        string journal_day = components[0];
                        string journal_month = components[1];
                        string journal_year = components[2];
                        string created_Date = journal_year + "-" + journal_month + "-" + journal_day;

                        msSQL = "select branch_name from  hrm_mst_tbranch where branch_gid='" + branch + "'";
                        objODBCDatareader = objdbconn.GetDataReader(msSQL);
                        if (objODBCDatareader.HasRows)
                        {
                            lsbranch_name = objODBCDatareader["branch_name"].ToString();

                        }
                        objODBCDatareader.Close();
                        lsjournal_refno = objcmnfunctions.GetMasterGID("JRNR");
                        lsvoucher_refno = objcmnfunctions.GetMasterGID("FPCC");

                        msSQL = "Insert into acc_trn_journalentry (journal_gid, journal_refno, transaction_code, transaction_date, remarks,document_path,  transaction_type, reference_type, reference_gid, transaction_gid, journal_year, journal_month, journal_day, created_by, created_date, branch_gid) " +
                                "values ('" + lsvoucher_refno + "', '" + lsjournal_refno + "', 'JL001', '" + created_Date + "', '" + remarks.Replace("'", "").Replace(",", "") + "',  '" + final_path + "','Journal', '" + lsbranch_name + "', '" + branch + "', '" + lsvoucher_refno.Replace("'", "") + "', '" + journal_year + "', '" + journal_month + "', '" + journal_day + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + branch + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Error While Adding Journal Entry !!";
                        }
                        else
                        {
                            for (int i = 0; i < details_list.ToArray().Length; i++)
                            {
                                string msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                                if (msGetDlGID == "E")
                                {
                                    values.status = false;
                                    values.message = "Some error occurred while getting gid!!";
                                }
                                else
                                {
                                    if (details_list[i].creditAmount != "0.00")
                                    {
                                        lsamount = details_list[i].creditAmount;
                                        lssjournal_type = "cr";
                                    }
                                    else
                                    {
                                        lsamount = details_list[i].debitAmount;
                                        lssjournal_type = "dr";
                                    }
                                    msSQL = "Insert into acc_trn_journalentrydtl (journaldtl_gid, journal_gid, account_gid, remarks, journal_type, created_by, created_date, transaction_amount) " +
                                            "values ('" + msGetDlGID + "', '" + lsvoucher_refno + "', '" + details_list[i].accountName + "', '" + details_list[i].particulars.Replace("'", "").Replace(",", "") + "', '" + lssjournal_type + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + lsamount + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        values.status = true;
                                        values.message = "Journal Entry Added Successfully !! ";
                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error While Adding Journal Entry !! ";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Adding Jounral Entry !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteJounralEntry(string journal_gid, results values)
        {
            try
            {
                msSQL = "DELETE FROM acc_trn_journalentrydtl WHERE journal_gid = '" + journal_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    msSQL = "DELETE FROM acc_trn_journalentry WHERE journal_gid = '" + journal_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Journal Entry Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Journal Entry !!";

                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Journal Entry !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Journal Entry !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetJournalEntrySummaryEdit(string journal_gid, MdlJournalEntry values)
        {

            msSQL = "SELECT DISTINCT a.journal_gid, a.transaction_type, a.journal_refno, " +
                    "DATE_FORMAT(a.transaction_date, '%d-%m-%Y') AS transaction_date, a.remarks,a.document_path,a.branch_gid " +
                    "FROM acc_trn_journalentry a " +
                    "WHERE journal_gid= '" + journal_gid + "' " +
                    "ORDER BY a.transaction_date DESC, a.journal_gid DESC";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getJournalEntryeditLists = new List<GetJournalEntryedit_lists>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow entryRow in dt_datatable.Rows)
                {
                    getJournalEntryeditLists.Add(new GetJournalEntryedit_lists
                    {
                        journal_gid = entryRow["journal_gid"].ToString(),
                        transaction_type = entryRow["transaction_type"].ToString(),
                        journal_refno = entryRow["journal_refno"].ToString(),
                        transaction_date = entryRow["transaction_date"].ToString(),
                        remarks = entryRow["remarks"].ToString(),
                        document_path = entryRow["document_path"].ToString(),
                        branch_gid = entryRow["branch_gid"].ToString(),
                    });
                    values.GetJournalEntryedit_lists = getJournalEntryeditLists;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetJournalEntrySummaryEditDtl(string journal_gid, MdlJournalEntry values)
        {
            msSQL = "SELECT e.accountgroup_gid as AccountGroup," +
                    " b.account_gid as AccountName," +
                    " b.remarks as particulars," +
                    " CASE WHEN b.journal_type='cr' THEN 0.00 ELSE b.transaction_amount END AS DebitAmount," +
                    " CASE WHEN b.journal_type='dr' THEN 0.00 ELSE b.transaction_amount END AS CreditAmount," +
                    " b.journaldtl_gid  " +
                    " FROM acc_trn_journalentrydtl b " +
                    " LEFT JOIN acc_mst_tchartofaccount e ON e.account_gid = b.account_gid " +
                    " WHERE b.journal_gid = '" + journal_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getJournalEntryeditLists = new List<GetJournalEntryeditdtl_lists>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getJournalEntryeditLists.Add(new GetJournalEntryeditdtl_lists
                    {
                        accountGroup = dt["AccountGroup"].ToString(),
                        accountName = dt["AccountName"].ToString(),
                        debitAmount = dt["DebitAmount"].ToString(),
                        creditAmount = dt["CreditAmount"].ToString(),
                        particulars = dt["particulars"].ToString(),
                        journaldtl_gid = dt["journaldtl_gid"].ToString(),
                    });
                    values.GetJournalEntryeditdtl_lists = getJournalEntryeditLists;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateJournalEntry(updatejournal_list values, string user_gid)
        {
            try
            {
                string date1 = values.created_date;
                string[] components = date1.Split('-');
                string journal_day = components[0];
                string journal_month = components[1];
                string journal_year = components[2];
                string created_Date = journal_year + "-" + journal_month + "-" + journal_day;
                msSQL = "select branch_name from  hrm_mst_tbranch where branch_gid='" + values.branch_name + "'";

                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    lsbranch_name = objODBCDatareader["branch_name"].ToString();
                }
                objODBCDatareader.Close();

                msSQL = " UPDATE acc_trn_journalentry SET  transaction_date = '" + created_Date + "'," +
                        " remarks = '" + values.remarks.Replace("'", "").Replace(",", "") + "',reference_type = '" + lsbranch_name + "', reference_gid = '" + values.branch_name + "',  journal_year = '" + journal_year + "', journal_month = '" + journal_month + "', journal_day = '" + journal_day + "', branch_gid = '" + values.branch_name + "' WHERE journal_gid = '" + values.journal_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error While Updating Journal Entry !!";
                }
                else
                {
                    for (int i = 0; i < values.detailsedit.ToArray().Length; i++)
                    {
                        if (values.detailsedit[i].creditAmount != "0.00" && values.detailsedit[i].creditAmount != "0")
                        {
                            lsamount = values.detailsedit[i].creditAmount;
                            lssjournal_type = "cr";
                        }
                        else if (values.detailsedit[i].debitAmount != "0.00" && values.detailsedit[i].debitAmount != "0")
                        {
                            lsamount = values.detailsedit[i].debitAmount;
                            lssjournal_type = "dr";
                        }
                        else
                        {
                            lsamount = "0.00";
                            lssjournal_type = "dr";
                        }
                        if (values.detailsedit[i].journaldtl_gid != "" && values.detailsedit[i].journaldtl_gid != null)
                        {
                            msSQL = " UPDATE acc_trn_journalentrydtl SET account_gid = '" + values.detailsedit[i].accountName + "', remarks = '" + values.detailsedit[i].particulars.Replace("'", "").Replace(",", "") + "', journal_type = '" + lssjournal_type + "'," +
                                    " transaction_amount = '" + lsamount + "' WHERE  journaldtl_gid = '" + values.detailsedit[i].journaldtl_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Journal Entry Updated Successfully !! ";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Journal Entry !! ";
                            }
                        }
                        else
                        {
                            string msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                            if (msGetDlGID == "E")
                            {
                                values.status = false;
                                values.message = "Some error occurred while getting gid!!";
                            }
                            msSQL = "Insert into acc_trn_journalentrydtl (journaldtl_gid, journal_gid, account_gid, remarks, journal_type, created_by, created_date, transaction_amount) " +
                                    "values ('" + msGetDlGID + "', '" + values.journal_gid + "', '" + values.detailsedit[i].accountName + "', '" + values.detailsedit[i].particulars.Replace("'", "").Replace(",", "") + "', '" + lssjournal_type + "', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + lsamount + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Journal Entry Updated Successfully !! ";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Journal Entry !! ";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Adding Jounral Entry !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Adding Journal Entry!! " + " *******" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaUpdateJournalEntryWithDoc(HttpRequest httpRequest, results values, string user_gid)
        {
            try
            {
                HttpFileCollection httpFileCollection;
                HttpPostedFile httpPostedFile;
                string branch = httpRequest.Form[0];
                string created = httpRequest.Form[1];
                string remarks = httpRequest.Form[2];
                string details = httpRequest.Form[3];
                string journal_gid = httpRequest.Form[4];
                List<detailsedit> details_list = JsonConvert.DeserializeObject<List<detailsedit>>(details);
                httpFileCollection = httpRequest.Files;
                string lsfilepath = string.Empty;
                string document_gid = string.Empty;
                string lspath, lspath1, lspath2;
                string lscompany_code = string.Empty;
                string base64String = string.Empty;
                string httpsUrl = string.Empty;

                msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Journal/Document/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int j = 0; j < httpFileCollection.Count; j++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[j];
                        string FileExtension = httpPostedFile.FileName;
                        //string lsfile_gid = msdocument_gid + FileExtension;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Journal/Document/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        //string local_path = "E:/Angular15/AngularUI/src";
                        ms.Close();
                        lspath = "/assets/media/images/erpdocument" + "/" + lscompany_code + "/" + "Journal/Document/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        string final_path = lspath + msdocument_gid + FileExtension;

                        string date1 = created;
                        string[] components = date1.Split('-');
                        string journal_day = components[0];
                        string journal_month = components[1];
                        string journal_year = components[2];
                        string created_Date = journal_year + "-" + journal_month + "-" + journal_day;

                        msSQL = "select branch_name from  hrm_mst_tbranch where branch_gid='" + branch + "'";
                        objODBCDatareader = objdbconn.GetDataReader(msSQL);
                        if (objODBCDatareader.HasRows)
                        {
                            lsbranch_name = objODBCDatareader["branch_name"].ToString();
                        }
                        objODBCDatareader.Close();


                        msSQL = " UPDATE acc_trn_journalentry SET  transaction_date = '" + created_Date + "'," +
                                " remarks = '" + remarks.Replace("'", "").Replace(",", "") + "',reference_type = '" + lsbranch_name + "', document_path = '" + final_path + "',reference_gid = '" + branch + "',  journal_year = '" + journal_year + "', journal_month = '" + journal_month + "', journal_day = '" + journal_day + "', branch_gid = '" + branch + "' WHERE journal_gid = '" + journal_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Error While Updating Journal Entry !!";
                        }
                        else
                        {
                            for (int i = 0; i < details_list.ToArray().Length; i++)
                            {
                                if (details_list[i].creditAmount != "0.00" && details_list[i].creditAmount != "0")
                                {
                                    lsamount = details_list[i].creditAmount;
                                    lssjournal_type = "cr";
                                }
                                else if (details_list[i].debitAmount != "0.00" && details_list[i].debitAmount != "0")
                                {
                                    lsamount = details_list[i].debitAmount;
                                    lssjournal_type = "dr";
                                }
                                else
                                {
                                    lsamount = "0.00";
                                    lssjournal_type = "dr";
                                }
                                if (details_list[i].journaldtl_gid != "" && details_list[i].journaldtl_gid != null)
                                {
                                    msSQL = " UPDATE acc_trn_journalentrydtl SET account_gid = '" + details_list[i].accountName + "', remarks = '" + details_list[i].particulars.Replace("'", "").Replace(",", "") + "', journal_type = '" + lssjournal_type + "'," +
                                            " transaction_amount = '" + lsamount + "' WHERE  journaldtl_gid = '" + details_list[i].journaldtl_gid + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        values.status = true;
                                        values.message = "Journal Entry Updated Successfully !! ";
                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error While Updating Journal Entry !! ";
                                    }
                                }
                                else
                                {
                                    string msGetDlGID1 = objcmnfunctions.GetMasterGID("FPCD");
                                    if (msGetDlGID1 == "E")
                                    {
                                        values.status = false;
                                        values.message = "Some error occurred while getting gid!!";
                                    }
                                    msSQL = "Insert into acc_trn_journalentrydtl (journaldtl_gid, journal_gid, account_gid, remarks, journal_type, transaction_amount) " +
                                            "values ('" + msGetDlGID1 + "', '" + journal_gid + "', '" + details_list[i].accountName + "', '" + details_list[i].particulars.Replace("'", "").Replace(",", "") + "', '" + lssjournal_type + "', '" + lsamount + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        values.status = true;
                                        values.message = "Journal Entry Updated Successfully !! ";
                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error While Updating Journal Entry !! ";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Updating Jounral Entry !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaDeleteJounralDtlEntry(string journaldtl_gid, results values)
        {
            try
            {
                msSQL = "DELETE FROM acc_trn_journalentrydtl WHERE journaldtl_gid = '" + journaldtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Journal Entry Account Details Deleted Successfully";
                }

                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Journal Entry Account Details !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Journal Entry !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}