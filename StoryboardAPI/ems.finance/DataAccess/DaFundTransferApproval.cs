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
    public class DaFundTransferApproval
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        string msEmployeeGID, msGetDlGID2, msGetGid2, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetFundTransferApprovalSummary(MdlFundTransferApproval values)
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
                              " ORDER BY b.transaction_date DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetFundTransferApproval_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetFundTransferApproval_list
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
                    values.GetFundTransferApproval_list = getModuleList;
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
        public void DaFundTransferApprovalStatus(FundTransferApproval_list values, string user_gid)
        {
            try
            {
                if (values.status_flag == "Y")
                {
                    msSQL = "UPDATE acc_trn_tpettycash SET " +
                            "reason = '" + values.reason.Replace("'", "").Replace(",", "") + "'," +
                            "remarks = '" + values.remarks.Replace("'", "").Replace(",", "") + "'," +
                            "approved_by = '" + user_gid + "'," +
                            "approval_flag = '" + values.status_flag + "'," +
                            "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "updated_by = '" + user_gid + "'," +
                            "approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                            "WHERE pettycash_gid = '" + values.pettycashgid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {

                        values.status = true;
                        values.message = "Fund Transfer Approved Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Approving Fund Transfer !!";
                    }
                }
                else
                {
                    msSQL = "UPDATE acc_trn_tpettycash SET " +
                            "reason = '" + values.reason.Replace("'", "").Replace(",", "") + "'," +
                            "approval_flag = '" + values.status_flag + "'," +
                            "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "updated_by = '" + user_gid + "'," +
                            "remarks = '" + values.remarks.Replace("'", "").Replace(",", "") + "'" +
                            "WHERE pettycash_gid = '" + values.pettycashgid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {

                        values.status = true;
                        values.message = "Fund Transfer Rejected Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Rejecting Fund Transfer !!";
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Updating Fund Transfer !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}