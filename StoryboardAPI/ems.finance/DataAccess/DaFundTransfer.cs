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
    public class DaFundTransfer
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        string msEmployeeGID, msGetDlGID2, msGetGid2, lsemployee_gid, lsentity_code, lsdepartment_gid,lsbranch_gid, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetFundTransferSummary(MdlFundTransfer values)
        {
            try
            {
                msSQL = "SELECT DISTINCT a.pettycash_gid, FORMAT(a.transaction_amount, 2) AS transaction_amount,b.remarks,b.reason, " +
                              "u.user_firstname, DATE_FORMAT(b.transaction_date, '%d-%m-%Y') as transaction_date, " +
                              "CASE WHEN approval_flag = 'N' THEN 'Pending' " +
                              "WHEN approval_flag = 'Y' THEN 'Approved' " +
                              "WHEN approval_flag = 'R' THEN 'Rejected' " +
                              "END AS approval_flag, " +
                              "(SELECT MAX(e.branch_name) FROM hrm_mst_tbranch e " +
                              "JOIN acc_trn_tpettycashdtl c ON c.branch_gid = e.branch_gid " +
                              "WHERE c.transaction_type = 'C' AND c.pettycash_gid = a.pettycash_gid) AS from_branch, " +
                              "(SELECT MAX(e.branch_gid) FROM hrm_mst_tbranch e " +
                              "JOIN acc_trn_tpettycashdtl c ON e.branch_gid = c.branch_gid " +
                              "WHERE c.transaction_type = 'C' AND c.pettycash_gid = a.pettycash_gid) AS from_branch_gid, " +
                              "(SELECT MAX(e.branch_name) FROM hrm_mst_tbranch e " +
                              "JOIN acc_trn_tpettycashdtl c ON e.branch_gid = c.branch_gid " +
                              "WHERE c.transaction_type = 'D' AND c.pettycash_gid = a.pettycash_gid) AS to_branch, " +
                              "(SELECT MAX(e.branch_gid) FROM hrm_mst_tbranch e " +
                              "JOIN acc_trn_tpettycashdtl c ON e.branch_gid = c.branch_gid " +
                              "WHERE c.transaction_type = 'D' AND c.pettycash_gid = a.pettycash_gid) AS to_branch_gid " +
                              "FROM acc_trn_tpettycashdtl a " +
                              "LEFT JOIN acc_trn_tpettycash b ON a.pettycash_gid = b.pettycash_gid " +
                              "LEFT JOIN hrm_mst_tbranch e ON e.branch_gid = a.branch_gid  LEFT JOIN adm_mst_tuser u on u.user_gid=b.raiser_gid " +
                              " WHERE b.transaction_mode = 'Fund Transfer' " +
                              " ORDER BY a.pettycash_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetFundTransfer_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetFundTransfer_list
                        {
                            pettycash_gid = dt["pettycash_gid"].ToString(),
                            transaction_amount = dt["transaction_amount"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            transaction_date = dt["transaction_date"].ToString(),
                            approval_flag = dt["approval_flag"].ToString(),
                            from_branch = dt["from_branch"].ToString(),
                            from_branch_gid = dt["from_branch_gid"].ToString(),
                            to_branch = dt["to_branch"].ToString(),
                            to_branch_gid = dt["to_branch_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            reason = dt["reason"].ToString(),
                        });
                    }
                    values.GetFundTransfer_list = getModuleList;
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
        public void DaPostFundTransfer(FundTransfer_list values, string user_gid)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("FPCC");
                string date1 = values.created_date;
                string[] components = date1.Split('-');
                string journal_day = components[0];
                string journal_month = components[1];
                string journal_year = components[2];
                string created_Date = journal_year + "-" + journal_month + "-" + journal_day;
                msSQL = "SELECT branch_gid,department_gid  FROM hrm_mst_temployee where  user_gid ='" + user_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    lsbranch_gid = objODBCDatareader["branch_gid"].ToString();
                    lsdepartment_gid = objODBCDatareader["department_gid"].ToString();

                }
                objODBCDatareader.Close();
                msSQL = "INSERT INTO acc_trn_tpettycash (" +
                        "pettycash_gid," +
                        "transaction_date, " +
                        "transaction_mode, " +
                        "transaction_amount, " +
                        "remarks, " +
                        "raiserbranch_gid," +
                        "raiserdepartment_gid," +
                        "raiser_gid," +
                        "created_date," +
                        "created_by," +
                        "approval_flag) " +
                        "VALUES (" +
                        "'" + msGetGid + "', " +
                        "'" + created_Date + "'," +
                        "'Fund Transfer', " +
                        "'" + values.transfer_amount.Replace("'", "").Replace(",", "") + "', " +
                        "'" + values.remarks.Replace("'", "") + "', " +
                        "'" + lsbranch_gid + "'," +
                        "'" + lsdepartment_gid + "'," +
                        "'" + user_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'" + user_gid + "'," +
                        "'N')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("FPCD");
                    if (msGetGid1 == "E")
                    {
                        values.status = false;
                        values.message = "Some error occurred while getting gid!!";
                    }

                    msSQL = "INSERT INTO acc_trn_tpettycashdtl (" +
                             "pettycashdtl_gid," +
                             "branch_gid, " +
                             "pettycash_gid," +
                             "sequence_no," +
                             "accountgroup_gid," +
                             "account_gid," +
                             "transaction_type," +
                             "transaction_amount," +
                             "created_by," +
                             "created_date," +
                             "remarks) " +
                             "VALUES (" +
                             "'" + msGetGid1 + "', " +
                             "'" + values.frombranch + "'," +
                             "'" + msGetGid.Replace("'", "") + "'," +
                             "'1' , " +
                             "'AT1010260077', " +
                             "'EX1202290421', " +
                             "'C', " +
                             "'" + values.transfer_amount.Replace("'", "").Replace(",", "") + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                             "'" + values.remarks.Replace("'", "") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msGetGid2 = objcmnfunctions.GetMasterGID("FPCD");
                        if (msGetGid2 == "E")
                        {
                            values.status = false;
                            values.message = "Some error occurred while getting gid!!";
                        }

                        msSQL = "INSERT INTO acc_trn_tpettycashdtl (" +
                             "pettycashdtl_gid," +
                             "branch_gid, " +
                             "pettycash_gid," +
                             "sequence_no," +
                             "accountgroup_gid," +
                             "account_gid," +
                             "transaction_type," +
                             "transaction_amount," +
                             "created_by," +
                             "created_date," +
                             "remarks) " +
                             "VALUES (" +
                             "'" + msGetGid2 + "', " +
                             "'" + values.tobranch + "'," +
                             "'" + msGetGid.Replace("'", "") + "'," +
                             "'1' , " +
                             "'AT1010260077', " +
                             "'EX1202290421', " +
                             "'D', " +
                             "'" + values.transfer_amount.Replace("'", "").Replace(",", "") + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                             "'" + values.remarks.Replace("'", "") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Fund Transfer Added Successfully !! ";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Fund Transfer !!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Fund Transfer !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Fund Transfer !!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Updating Fund Transfer !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateFundTransfer(FundTransfer_list values, string user_gid)
        {
            try
            {
                string date1 = values.created_date;
                string[] components = date1.Split('-');
                string journal_day = components[0];
                string journal_month = components[1];
                string journal_year = components[2];
                string created_Date = journal_year + "-" + journal_month + "-" + journal_day;

                msSQL = "UPDATE acc_trn_tpettycash SET " +
                        "transaction_amount = '" + values.transfer_amount.Replace("'", "").Replace(",", "") + "'," +
                        "remarks = '" + values.remarks.Replace("'", "").Replace(",", "") + "'," +
                        "updated_by = '" + user_gid + "'," +
                        "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "approval_flag = 'N' " +
                        "WHERE pettycash_gid = '" + values.pettycash_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    msSQL = "UPDATE acc_trn_tpettycashdtl " +
                              "SET transaction_amount ='" + values.transfer_amount.Replace("'", "").Replace(",", "") + "'," +
                              "updated_by = '" + user_gid + "'," +
                              "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                              " remarks = '" + values.remarks.Replace("'", "").Replace(",", "") + "' " +
                              "WHERE pettycash_gid ='" + values.pettycash_gid + "'";
                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult1 == 1)
                    {

                        values.status = true;
                        values.message = "Fund Transfer Updated Successfully !!";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Fund Transfer Detail !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Fund Transfer !!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Updating Fund Transfer !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteFundTransfers(string pettycash_gid, results values)
        {
            try
            {
                msSQL = "DELETE  " +
                          "FROM acc_trn_tpettycashdtl  " +
                          "WHERE pettycash_gid = '" + pettycash_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = "DELETE " +
                        "FROM acc_trn_tpettycash  " +
                        "WHERE pettycash_gid = '" + pettycash_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Fund Transfer Deleted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Fund Transfer !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Fund Transfer !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Fund Transfer!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}