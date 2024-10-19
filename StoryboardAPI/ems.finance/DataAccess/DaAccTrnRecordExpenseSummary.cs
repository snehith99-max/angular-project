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
    public class DaAccTrnRecordExpenseSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        string msEmployeeGID, msGetDlGID2, msGetGid2, lsemployee_gid, lsledger_type, lsaccountgid, lshaschild, lsaccount_gid, lshas_child, lsaccount_name, lsdispaly_type, msAccGetGID, lsentity_code, hasChild, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaRecordExpenseSummary(MdlAccTrnRecordExpenseSummary values)
        {
            try
            {
                msSQL = " select distinct a.expenserequisition_gid, " +
                        " DATE_format(a.expenserequisition_date,'%d-%m-%Y') as expenserequisition_date, " +
                        " a.expenserequisition_flag, " +
                        " a.vendor, " +
                        " a.vendor_gst, " +
                        " a.contactperson_name, " +
                        " DATE_format(a.due_date,'%d-%m-%Y') as due_date, " +
                        " CASE when a.payment_flag <> 'Payment Pending' then a.payment_flag  " +
                        " when a.invoice_flag <>'Invoice Pending' then a.invoice_flag  " +
                        " else a.expenserequisition_flag end as 'overall_status', " +
                        " b.user_firstname, d.department_name, e.branch_name,format(a.total_amount,2) as total_amount " +
                        " from tae_trn_texpenserequisition a  " +
                        " left join adm_mst_tuser b on a.created_by = b.user_gid  " +
                        " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid  " +
                        " left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid " +
                        " left join adm_mst_tmodule2employee  f on f.employee_gid = c.employee_gid  " +
                        " where 0=0 order by expenserequisition_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<record_expense_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new record_expense_list
                        {
                            expenserequisition_gid = dt["expenserequisition_gid"].ToString(),
                            expenserequisition_date = dt["expenserequisition_date"].ToString(),
                            expenserequisition_flag = dt["expenserequisition_flag"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            vendor_gst = dt["vendor_gst"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            vendor = dt["vendor"].ToString(),

                        });
                    }
                    values.record_expense_list = getModuleList;
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
        public void dagetvendordropdown(MdlAccTrnRecordExpenseSummary values)
        {
            try
            {
                msSQL = "select vendor_gid , vendor_companyname from acp_mst_tvendor";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<vendor_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new vendor_list
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_name = dt["vendor_companyname"].ToString(),                           
                        });
                    }
                    values.vendor_list = getModuleList;
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
        public void DaGetaccountgroupnamedropdown(MdlAccTrnRecordExpenseSummary values)
        {
            try
            {
                msSQL = " select account_gid, UCASE(account_name) as account_name, accountgroup_name from acc_mst_tchartofaccount where accountgroup_gid = '$' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<accountgroupname_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new accountgroupname_list
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                        });
                    }
                    values.accountgroupname_list = getModuleList;
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
        public void daonchangevendordetails(string vendor_gid, MdlAccTrnRecordExpenseSummary values)
        {
            try
            {
                msSQL = " select a.gst_number , b.address1 from acp_mst_tvendor a " +
                        " left join adm_mst_taddress b on a.address_gid = b.address_gid where a.vendor_gid = '" + vendor_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<vendordetails_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new vendordetails_list
                        {

                            vendor_address = dt["address1"].ToString(),
                            vendor_gst = dt["gst_number"].ToString(),

                        });
                    }
                    values.vendordetails_list = getModuleList;
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
        public void daonchangeaccountgroup(string account_gid, MdlAccTrnRecordExpenseSummary values)
        {
            try
            {
                msSQL = " select account_gid, account_name, accountgroup_name from acc_mst_tchartofaccount " +
                        " where accountgroup_gid = '" + account_gid + "' ";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    string accountgroup_gid = objODBCDatareader["account_gid"].ToString();
                    string accountsubgroup_name = objODBCDatareader["account_name"].ToString();

                    msSQL = " select account_gid, account_name from acc_mst_tchartofaccount " +
                            " where accountgroup_gid = '" + accountgroup_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    string ledger_name = "";

                    if (dt_datatable.Rows.Count > 0)
                    {
                        DataRow row = dt_datatable.Rows[0];

                        ledger_name = row["account_name"].ToString();

                        var getModuleList = new List<accountgroup_lists>();

                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new accountgroup_lists
                            {
                                account_gid = row["account_gid"].ToString(),
                                account_name = accountsubgroup_name + " - " + ledger_name
                            });
                        }
                        values.accountgroup_lists = getModuleList;
                        dt_datatable.Dispose();
                    }
                    objODBCDatareader.Close();
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
        public void DaMakePaymentUpdate(record_expense_list values)
        {
            try
            {
                msSQL = " update tae_trn_texpenserequisition set " +
                        " bank_name ='" + values.editbank_name + "'," +
                        " payment_mode ='" + values.payment_mode + "'," +
                        " payment_date ='" + values.payment_date + "'," +
                        " transaction_number ='" + values.transaction_number + "'" +
                        " where expenserequisition_gid ='" + values.expenserequisition_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Make Payments Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Make Payment !!";
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
        public void DaPostExpenseMulAddDtls(string user_gid, expensesubmitlist values)
        {
            try
            {
                foreach (var data in values.expensemuladd_list)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("EXRD");

                    msSQL = " insert into tae_trn_texpenserequisitiondtl( " +
                            " expenserequisitiondtl_gid, " +
                            " accountgroup_gid, " +
                            " account_gid," +
                            " expense_amount," +
                            " remarks," +
                            " claim_date," +
                            " created_by," +
                            " created_date," +
                            " user_gid) " +
                            " values( " +
                            "'" + msGetGid + "', " +
                            "'" + data.accountgroup_name + "'," +
                            "'" + data.account_name + "'," +
                            "'" + data.transaction_amount + "'," +
                            "'" + data.remarks + "'," +
                            "'" + data.claim_date + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Expense Details Added Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Expense Details !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Expense Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostExpenseDetailsAdd(string user_gid, expensedetailadd_list values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("EXRQ");

                msSQL = " Insert into tae_trn_texpenserequisition " +
                        " (expenserequisition_gid, " +
                        " expenserequisition_date, " +
                        " expenserequisition_flag, " +
                        " contactperson_name," +
                        " due_date," +
                        " vendor," +
                        " vendor_gst," +
                        " vendor_address," +
                        " expenserequisition_status, " +
                        " approved_by , " +
                        " approved_date , " +
                        " created_by, " +
                        " created_date)" +
                        " values (" +
                        "'" + msGetGid + "', " +
                        //"'" + values.expenserequisition_date + "', " +
                        " str_to_date('" + values.expenserequisition_date + "', '%d-%m-%Y')," +
                        "'ER Pending Approval', " +
                        "'" + values.contactperson_name + "'," +
                        //"'" + values.due_date + "', " +
                        " str_to_date('" + values.due_date + "', '%d-%m-%Y')," +
                        "'" + values.vendor + "', " +
                        "'" + values.vendor_gst + "', " +
                        "'" + values.vendor_address + "', " +
                        "'Approved', " +
                        "'" + user_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'" + user_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = " update tae_trn_texpenserequisitiondtl set " +
                            " expenserequisition_gid ='" + msGetGid + "'" +
                            " where created_by='" + user_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Expense Details Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Expense Details !!";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Expense Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteExpense(string expenserequisition_gid, MdlAccTrnRecordExpenseSummary values)
        {
            msSQL = " delete from tae_trn_texpenserequisition where expenserequisition_gid='" + expenserequisition_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msSQL = " delete from tae_trn_texpenserequisitiondtl where expenserequisition_gid='" + expenserequisition_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Expense Deleted Successfully";
            }
            else
            {
                    values.status = false;
                    values.message = "Error While Deleting Expense";
            }
        }
        public void DaMakePaymentdetails(string expenserequisition_gid, MdlAccTrnRecordExpenseSummary values)
        {
            try
            {
                msSQL = " select DATE_format(expenserequisition_date,'%d-%m-%Y') as expenserequisition_date," +
                        " contactperson_name," +
                        " DATE_format(due_date,'%d-%m-%Y') as due_date, " +
                        " vendor," +
                        " vendor_gst," +
                        " vendor_address " +
                        " from tae_trn_texpenserequisition where expenserequisition_gid= '" + expenserequisition_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<record_expense_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new record_expense_list
                        {
                            expenserequisition_date = dt["expenserequisition_date"].ToString(),
                            vendor_gst = dt["vendor_gst"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            vendor = dt["vendor"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                        });
                    }
                    values.Make_Payment_Group_list = getModuleList;
                    dt_datatable.Dispose();
                }

                msSQL1 = " select accountgroup_gid,account_gid,expense_amount,remarks," +
                         " DATE_format(claim_date,'%d-%m-%Y') as claim_date " +
                         " from tae_trn_texpenserequisitiondtl where expenserequisition_gid= '" + expenserequisition_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL1);
                var getModuleList1 = new List<expensemuladd_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList1.Add(new expensemuladd_list
                        {
                            accountgroup_name = dt["accountgroup_gid"].ToString(),
                            account_name = dt["account_gid"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            claim_date = dt["claim_date"].ToString(),
                        });
                    }
                    values.Make_Payment_list = getModuleList1;
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
    }
}