//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace ems.finance.DataAccess
//{
//    public class DaAccTrnPayment
//    {
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.finance.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
//using System.Web;
//using OfficeOpenXml;
using System.Configuration;
using System.IO;
//using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using static System.Drawing.ImageConverter;

using System.Text;
using System.Web;
using System.Diagnostics.Contracts;


namespace ems.finance.DataAccess
{
    public class DaAccTrnPayment
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, lspaymentflag, lsexpensestatus,
            msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        string lsoutstanding, lsadvance;
        string company_logo_path;
        System.Drawing.Image branch_logo;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();
        DataTable DataTable3 = new DataTable();
        finance_cmnfunction objfincmn = new finance_cmnfunction();

        public void DaGetPaymentRptSummary(MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " select /*+ MAX_EXECUTION_TIME(300000) */ distinct a.payment_gid,h.expense_gid,a.payment_from, case when a.payment_status='PY Approved' then 'Payment Done'" +
                        " when a.payment_status='PY Pending' then 'Payment Pending' when a.payment_status='PY Canceled' then 'Payment Canceled' else a.payment_status end as payment_status," +
                        "CASE WHEN j.poref_no IS NULL OR j.poref_no = '' THEN CASE WHEN j.purchaseorder_gid IS NULL OR j.purchaseorder_gid ='' THEN 'Direct expense' ELSE j.purchaseorder_gid  END ELSE j.poref_no  END AS porefno," +
                        " a.vendor_gid,l.vendorexpenseref_no, format(a.payment_total,2) as payment_total,DATE_FORMAT(a.payment_date,'%d-%m-%Y')  as payment_date,k.costcenter_name,a.payment_reference, " +
                        " c.vendor_code, c.vendor_companyname,a.purchaseorder_gid, case when a.payment_mode='Cash' then a.payment_mode " +
                        " when a.payment_mode='CREDITCARD' then concat(cast(a.payment_mode as char), '/' , cast(d.bank_name as char)) " +
                        " when a.payment_mode='Cheque' then concat(cast(a.payment_mode as char), '/' , a.cheque_no,'/', cast(a.bank_name as char))" +
                        " when a.payment_mode='NEFT' then concat(cast(a.payment_mode as char), '/' , cast(a.bank_name as char))" +
                        " when a.payment_mode='DD' then concat(cast(a.payment_mode as char), '/' , cast(a.bank_name as char)) else a.payment_mode end as payment_mode," +
                        " format(a.tds_amount,2)as tds_amount, format(a.tdscalculated_finalamount,2)as tdscalculated_finalamount from acp_trn_tsundryexpensepayment a" +
                        " left join acp_trn_tsudryexpensespaymentdtl b on a.payment_gid = b.payment_gid left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid left join acc_trn_tsundryexpenses2payment h on h.payment_gid=a.payment_gid " +
                        " left join acc_mst_tcreditcard d on a.bank_gid=d.bank_gid  left join pmr_trn_tpurchaseorder j on a.purchaseorder_gid=j.purchaseorder_gid " +
                        " left join pmr_mst_tcostcenter k on j.costcenter_gid=k.costcenter_gid  left join acc_trn_tsundryexpenses l on c.vendor_gid=l.vendor_gid " +
                        " where a.payment_status <> 'PY Completed'  group by a.payment_gid order by date(a.payment_date) desc,a.payment_date desc, a.payment_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentrpt_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentrpt_list
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            expense_gid = dt["expense_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            //payment_remarks = dt["payment_remarks"].ToString(),
                            payment_total = dt["payment_total"].ToString(),
                            payment_status = dt["payment_status"].ToString(),
                            //user_gid = dt["user_gid"].ToString(),
                            //created_date = dt["created_date"].ToString(),
                            payment_reference = dt["payment_reference"].ToString(),
                            purchaseorder_gid = dt["porefno"].ToString(),
                            //advance_total = dt["advance_total"].ToString(),
                            payment_mode = dt["payment_mode"].ToString(),
                            //bank_name = dt["bank_name"].ToString(),
                            //branch_name = dt["branch_name"].ToString(),
                            //cheque_no = dt["cheque_no"].ToString(),
                            //city_name = dt["city_name"].ToString(),
                            //currency_code = dt["currency_code"].ToString(),
                            //exchange_rate = dt["exchange_rate"].ToString(),
                            tds_amount = dt["tds_amount"].ToString(),
                            tdscalculated_finalamount = dt["tdscalculated_finalamount"].ToString(),
                            //priority = dt["priority"].ToString(),
                            //priority_remarks = dt["priority_remarks"].ToString(),
                            //approved_by = dt["approved_by"].ToString(),
                            //approved_date = dt["approved_date"].ToString(),
                            //reject_reason = dt["reject_reason"].ToString(),
                            //bank_gid = dt["bank_gid"].ToString(),
                            //addon_amount = dt["addon_amount"].ToString(),
                            //additional_discount = dt["additional_discount"].ToString(),
                            //additional_gid = dt["additional_gid"].ToString(),
                            //discount_gid = dt["discount_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            //payment_amount = dt["payment_amount1"].ToString(),
                            //expense_amount = dt["expense_amount1"].ToString(),



                        });
                        values.paymentrptlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetexpensedetails(string expense_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = "select expense_refno,date_format(expense_date,'%d-%m-%y') as expense_date,expense_amount from acc_trn_tsundryexpenses where expense_gid = '" + expense_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getexpense>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getexpense
                        {

                            expense_refno = dt["expense_refno"].ToString(),
                            expense_date = dt["expense_date"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),

                        });
                        values.getexpense = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetPaymentview(string payment_gid, string user_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " SELECT a.payment_gid,c.address1,d.country,date_format(a.payment_date,'%d-%m-%Y') as payment_date, a.vendor_gid, a.payment_remarks, a.payment_status," +
                        " a.payment_mode,a.tds_amount ,a.bank_name, a.branch_name, a.cheque_no, a.city_name, a.dd_no, a.currency_code," +
                        " a.exchange_rate, format(a.payment_total, 2) as payment_total," +
                        " a.payment_reference,b.vendor_companyname,b.contactperson_name,b.contact_telephonenumber," +
                        " b.email_id,concat(c.address1, c.city, c.postal_code, c.state, d.country) as vendoraddress,c.fax" +
                        " FROM acp_trn_tsundryexpensepayment a left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                        " left join adm_mst_taddress c on b.address_gid = c.address_gid " +
                        " left join crm_trn_Tcurrencyexchange d on d.country_gid = c.country_gid where a.payment_gid = '" + payment_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = " select concat(a.user_firstname) as employee_name, " +
                        " b.employee_emailid, b.employee_phoneno, c.department_name " +
                        " from adm_mst_tuser a " +
                        " left join hrm_mst_temployee b on a.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment c on b.department_gid = c.department_gid " +
                        " where a.user_gid = '" + user_gid + "'";
                string name = objdbconn.GetExecuteScalar(msSQL);

                var getModuleList = new List<payment_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new payment_lists
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            payment_remarks = dt["payment_remarks"].ToString(),
                            payment_mode = dt["payment_mode"].ToString(),
                            payment_status = dt["payment_status"].ToString(),
                            tds_amount = dt["tds_amount"].ToString(),
                            payment_total = dt["payment_total"].ToString(),
                            payment_reference = dt["payment_reference"].ToString(),
                            city_name = dt["city_name"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            address1 = dt["address1"].ToString(),
                            country = dt["country"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            fax = dt["fax"].ToString(),
                            vendoraddress = dt["vendoraddress"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            employee_name = name,



                        });
                        values.paymentlists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DagetPaymenamount(string payment_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " Select a.expense2payment_gid,c.tds_amount, a.payment_gid, a.paymentdtl_gid, a.expense_gid, " +
                        " format(a.expense_amount, 2) as expense_amount, format(a.advance_amount, 2) as po_advance, " +
                        " format(a.payment_amount, 2) as payment_amount, b.expense_remarks ,c.payment_reference " +
                        " FROM acc_trn_tsundryexpenses2payment a " +
                        " left join acp_trn_tsudryexpensespaymentdtl b on a.paymentdtl_gid = b.paymentdtl_gid " +
                        " left join acp_trn_tsundryexpensepayment c on a.payment_gid = c.payment_gid " +
                        " where a.payment_gid = '" + payment_gid + "' order by a.payment_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentamount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentamount_list
                        {
                            expense2payment_gid = dt["expense2payment_gid"].ToString(),
                            payment_gid = dt["payment_gid"].ToString(),
                            paymentdtl_gid = dt["paymentdtl_gid"].ToString(),
                            expense_gid = dt["expense_gid"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            po_advance = dt["po_advance"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            expense_remarks = dt["expense_remarks"].ToString(),
                            payment_reference = dt["payment_reference"].ToString(),
                            tds_amount = dt["tds_amount"].ToString(),
                        });

                        values.paymentamount_list = getModuleList;

                    }

                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetPaymentAddproceed(MdlAccTrnPayment values, string vendorname)
        {
            try
            {
                if (vendorname != null)
                {


                    msSQL = " select a.expense_gid,b.vendor_companyname,b.vendor_gid,a.expense_status,format(sum(a.expense_amount),2)as expense_amount," +
                    " format(sum(a.payment_amount),2) as payment_amount,format(sum(a.expense_amount-a.payment_amount),2) as outstanding,a.expense_from, " +
                    " case when b.contact_telephonenumber is null then  concat(b.contactperson_name,'/',b.email_id) " +
                    " when b.contact_telephonenumber is not null then concat(b.contactperson_name,'/',b.contact_telephonenumber,'/',b.email_id) end as contact " +
                    " from acp_mst_tvendor b" +
                    " left join acc_trn_tsundryexpenses a on b.vendor_gid=a.vendor_gid where " +
                    " a.expense_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and " +
                    " ((a.expense_flag = 'Payment Pending' and a.payment_flag ='PY Pending') or " +
                    " (a.expense_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and  " +
                    " a.expense_from <> 'Expenses' and b.vendor_companyname like '%" + vendorname + "%' " +
                    " group by b.vendor_gid order by a.expense_gid desc ";

                }

                else
                {
                    msSQL = " select a.expense_gid,b.vendor_companyname,b.vendor_gid,a.expense_status,format(sum(a.expense_amount),2)as expense_amount," +
                    " format(sum(a.payment_amount),2) as payment_amount,format(sum(a.expense_amount-a.payment_amount),2) as outstanding,a.expense_from, " +
                    " case when b.contact_telephonenumber is null then  concat(b.contactperson_name,'/',b.email_id) " +
                    " when b.contact_telephonenumber is not null then concat(b.contactperson_name,'/',b.contact_telephonenumber,'/',b.email_id) end as contact " +
                    " from acc_trn_tsundryexpenses a" +
                    " left join acp_mst_tvendor b on b.vendor_gid=a.vendor_gid " +
                    " group by b.vendor_gid order by a.expense_gid desc ";

                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentadd>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentadd
                        {
                            expense_gid = dt["expense_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            expense_status = dt["expense_status"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            outstanding = dt["outstanding"].ToString(),
                            expense_from = dt["expense_from"].ToString(),
                            contact = dt["contact"].ToString(),

                        });
                        values.paymentadd = getModuleList;
                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetMakepaymentExpand(string vendor_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " select distinct a.expense_gid,a.expense_refno, date_format(a.expense_date,'%d-%m-%Y') as expense_date, a.expense_from,a.vendor_gid, a.vendor_contact_person, a.vendor_address, a.expense_remarks, " +
                    " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag " +
                    " else a.expense_flag end as 'overall_status', " +
                    " format(a.expense_amount, 2) as expense_amount, a.expense_status, a.user_gid, a.created_date, a.expense_reference,a.agreement_gid, " +
                    " format(ABS(a.payment_amount + a.advance_amount+a.debit_note),2) as payed_amount,format(ABS(a.expense_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding," +
                    " a.payment_date,a.expense_flag, " +
                    " c.vendor_companyname,date_format(a.payment_date,'%d-%m-%Y') as due_date " +
                    " from acc_trn_tsundryexpenses a " +
                    " left join acc_trn_tsundryexpensesdtl b on a.expense_gid = b.expense_gid " +
                    " left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid " +
                    " where a.expense_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and" +
                    " ((a.expense_flag = 'Payment Pending' and a.payment_flag ='PY Pending') or " +
                    " (a.expense_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial'))  and a.vendor_gid='" + vendor_gid + "' " +
                    "group by a.expense_gid  order by a.expense_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentExpand>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentExpand
                        {
                            expense_gid = dt["expense_gid"].ToString(),
                            expense_refno = dt["expense_refno"].ToString(),
                            expense_date = dt["expense_date"].ToString(),
                            expense_from = dt["expense_from"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_contact_person = dt["vendor_contact_person"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            expense_remarks = dt["expense_remarks"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            payed_amount = dt["payed_amount"].ToString(),
                            outstanding = dt["outstanding"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),

                        });
                        values.paymentExpand = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaProductdetails(string expense_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " SELECT a.expense_gid,a.expensedtl_gid, a.product_gid,b.product_code,c.expense_refno,format(a.product_total,2) as product_total , " +
                                " a.qty_expense, b.product_name " +
                                " FROM acc_trn_tsundryexpensesdtl a " +
                                " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                                " left join acc_trn_tsundryexpenses c on a.expense_gid=c.expense_gid" +
                                " where a.expense_gid = '" + expense_gid + "'" +
                                " order by a.expensedtl_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productdetail_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productdetail_list
                        {
                            expense_gid = dt["expense_gid"].ToString(),
                            expensedtl_gid = dt["expensedtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            expense_refno = dt["expense_refno"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            qty_expense = dt["qty_expense"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.productdetail_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetSinglePaymentSummary(string vendor_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " select distinct a.expense_gid, a.expense_status, format(a.expense_amount,2) as expense_amount, a.vendor_gid, a.expense_remarks,a.expense_from," +
                        " case when d.purchaseorder_gid is null then a.expense_reference else d.purchaseorder_gid end as purchaseorder_gid, " +
                        " date_format(a.expense_date,'%d-%m-%Y') as expense_date, format(a.payment_amount + a.advance_amount+a.debit_note,2) as payed_amount, b.vendor_companyname,  " +
                        " format(ABS(a.expense_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding ,date_format(a.payment_date,'%d-%m-%Y') as due_date from acc_trn_tsundryexpenses a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                        " left join acp_trn_tpo2expense d on a.expense_gid = d.expense_gid " +
                        " where a.expense_amount<> ABS((a.payment_amount+a.advance_amount + a.debit_note)) and" +
                        " ((a.expense_flag = 'Payment Pending' and a.payment_flag = 'PY Pending') or" +
                        " (a.expense_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and a.expense_from<> 'Expenses' and b.vendor_gid = '" + vendor_gid + "' " +
                        " order by a.expense_date desc, a.expense_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<singlepayment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new singlepayment_list
                        {
                            expense_gid = dt["expense_gid"].ToString(),
                            expense_status = dt["expense_status"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            expense_remarks = dt["expense_remarks"].ToString(),
                            expense_from = dt["expense_from"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            expense_date = dt["expense_date"].ToString(),
                            payed_amount = dt["payed_amount"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            outstanding = dt["outstanding"].ToString(),


                        });
                        values.singlepaymentlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void Dapaymentcancelsubmit(string payment_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = "select expense_gid from acc_trn_tsundryexpenses2payment where payment_gid='" + payment_gid + "'";
                msSQL = " delete from acc_trn_tsundryexpenses2payment " +
                    " where payment_gid = '" + payment_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                {
                    msSQL = " Update acp_trn_tsundryexpensepayment set " +
                        " payment_status = 'Payment Cancelled'" +
                        " where payment_gid = '" + payment_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Cancel Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While add Branch !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetpaymentCancel(string payment_gid, MdlAccTrnPayment values, string user_gid)
        {
            try
            {
                msSQL = msSQL = " SELECT a.payment_gid,c.address1,d.country, a.payment_date, a.vendor_gid, a.payment_remarks, " +
                    " a.payment_mode, a.bank_name, a.branch_name, a.cheque_no, a.city_name, a.dd_no,  " +
                    " format(a.payment_total, 2) as payment_total, a.payment_reference,b.vendor_companyname,b.contactperson_name,b.contact_telephonenumber, " +
                     " b.email_id,concat(c.address1, c.city, c.postal_code, c.state, d.country) as vendoraddress,c.fax " +
                     "FROM acp_trn_tsundryexpensepayment a" +
                     " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                    " left join adm_mst_taddress c on b.address_gid = c.address_gid " +
                     " left join crm_trn_Tcurrencyexchange d on d.country_gid = c.country_gid where a.payment_gid = '" + payment_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = " select concat (a.user_firstname,' ',c.department_name) as Name from adm_mst_Tuser a " +
                        " left join hrm_mst_Temployee b on a.user_gid=b.user_gid left join   c " +
                        " on b.department_gid=c.department_gid where a.user_gid= '" + user_gid + "' ";
                string Name = objdbconn.GetExecuteScalar(msSQL);

                var getModuleList = new List<paymentcancel>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)

                        getModuleList.Add(new paymentcancel
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            payment_remarks = dt["payment_remarks"].ToString(),
                            payment_mode = dt["payment_mode"].ToString(),
                            payment_total = dt["payment_total"].ToString(),
                            payment_reference = dt["payment_reference"].ToString(),
                            city_name = dt["city_name"].ToString(),
                            address1 = dt["address1"].ToString(),
                            country = dt["country"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contactpersonname = dt["contactperson_name"].ToString(),
                            fax = dt["fax"].ToString(),
                            vendoraddress = dt["vendoraddress"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            name = Name,

                        });
                    values.paymentcancel = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetBankDetail(MdlAccTrnPayment values)
        {
            try
            {
                msSQL = "select bank_gid,bank_name from acc_mst_tbank";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBankdtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBankdtldropdown
                        {
                            bank_name = dt["bank_name"].ToString(),
                            bank_gid = dt["bank_gid"].ToString(),
                        });
                        values.GetBankNameVle = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetmultipleexpense2employeedtl(string user_gid, string vendor_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " select a.vendor_gid, a.email_id,a.contact_telephonenumber,a.vendor_companyname,b.city,concat(a.contactperson_name, ' / ', a.contact_telephonenumber) as vendorcontactdetails, " +
                        " concat(b.address1, ' ', b.address2, '', b.city, '', b.postal_code, '', b.state, '', c.country) as vendoraddress,c.currency_code,c.exchange_rate " +
                        " ,b.fax from acp_mst_tvendor a " +
                        " left join adm_mst_taddress b on a.address_gid = b.address_gid " +
                        " left join crm_trn_Tcurrencyexchange c on b.country_gid = c.country_gid where a.vendor_gid = '" + vendor_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = " select concat (a.user_firstname,' ',c.department_name) as Name from adm_mst_Tuser a " +
                        " left join hrm_mst_Temployee b on a.user_gid=b.user_gid left join hrm_mst_Tdepartment c " +
                        " on b.department_gid=c.department_gid where a.user_gid= '" + user_gid + "' ";
                string Name = objdbconn.GetExecuteScalar(msSQL);

                var getModuleList = new List<Getmultipleexpense2employeedtl>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getmultipleexpense2employeedtl
                        {
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendorcontactdetails = dt["vendorcontactdetails"].ToString(),
                            vendoraddress = dt["vendoraddress"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            fax = dt["fax"].ToString(),
                            name = Name,
                            payment_date = "",
                            payment_remarks = "",
                            paymentnotes = "",
                            vendor_gid = dt["vendor_gid"].ToString(),
                            city = dt["city"].ToString(),

                        });
                        values.Getmultipleexpense2employeedtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetpaymentexpenseSummary(string vendor_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " select distinct a.expense_refno, a.expense_gid, replace(a.expense_status,'IV','Expence') as expense_status,a.advance_amount, format(a.expense_amount,2) as expense_amount, a.vendor_gid, a.expense_remarks,a.expense_from," +
                          " date_format(a.expense_date,'%d-%m-%Y') as expense_date, format(a.payment_amount + a.advance_amount+a.debit_note,2) as payed_amount, b.vendor_companyname,  " +
                        " format(ABS(a.expense_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding ,date_format(a.payment_date,'%d-%m-%Y') as due_date from acc_trn_tsundryexpenses a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid  " +
                        " where a.expense_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and" + " ((a.expense_flag = 'Payment Pending' ) or " +
                        " (a.expense_status = 'IV Completed')) and b.vendor_gid='" + vendor_gid + "'";

                msSQL += " order by a.expense_date desc, a.expense_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMultipleexpenseSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        //if (dt["expense_from"].ToString() == "Services")
                        //{
                        //    msSQL = " select format(a.advance_amount,2) as advance_amount, " +
                        //        " format(a.advance_amount - a.advance_amount_utilized,2)  as outstanding_advance,b.exchange_rate,b.currency_code " +
                        //        " from pbl_trn_tserviceorder a " +
                        //        " left join acc_trn_tsundryexpenses b on a.serviceorder_gid= b.expense_reference " +
                        //        " where serviceorder_gid='" + dt["purchaseorder_gid"].ToString() + "'";
                        //    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        //}
                        //else
                        //{
                        //    msSQL = " select format(purchaseorder_advance,2) as advance_amount, " +
                        //        " format(purchaseorder_advance - purchaseorder_advance_utilized,2) as outstanding_advance, " +
                        //        " case when currency_code is null then 'INR' else currency_code end as currency_code," +
                        //        " case when exchange_rate is null then '1' else exchange_rate end as exchange_rate " +
                        //        " from pmr_trn_tpurchaseorder " + " where purchaseorder_gid = '" + dt["purchaseorder_gid"].ToString() + "'";
                        //    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        //}
                        //if (objOdbcDataReader.HasRows == true)
                        //{
                        //    objdbconn.OpenConn();
                        //    lsadvance = objOdbcDataReader["advance_amount"].ToString();
                        //    lsoutstanding = objOdbcDataReader["outstanding_advance"].ToString();



                        //}
                        //if (lsadvance == null) { lsadvance = "0.0"; }
                        //if (lsoutstanding == null) { lsoutstanding = "0.0"; }
                        getModuleList.Add(new GetMultipleexpenseSummary
                        {

                            expense_gid = dt["expense_gid"].ToString(),
                            expense_refno = dt["expense_refno"].ToString(),
                            expense_date = dt["expense_date"].ToString(),
                            //purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            expense_amount = double.Parse(dt["expense_amount"].ToString()),
                            expense_status = dt["expense_status"].ToString(),
                            outstanding = double.Parse(dt["outstanding"].ToString()),
                            payed_amount = double.Parse(dt["payed_amount"].ToString()),
                            advance = double.Parse(dt["advance_amount"].ToString()),
                            payment_amount = 0.00.ToString(),
                            balancepo_advance = 0.00,
                            grand_total = 0.00,
                            tds_amount = 0.00.ToString(),
                            final_amount = "0.0",
                            totalpo_advance = 0.00,
                            remark = "",
                        });

                        values.GetMultipleexpenseSummary = getModuleList;
                    }


                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetCardDetail(MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " select bank_gid,concat(bank_name,'/',cardholder_name) as bank_name From acc_mst_tcreditcard ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCarddtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCarddtldropdown
                        {
                            bank_gid = dt["bank_gid"].ToString(),
                            bank_name = dt["bank_name"].ToString(),
                        });
                        values.GetCardNameVle = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostmultipleexpense2singlepayment(multipleexpense2singlepayment values, string user_gid)
        {
            try
            {
                string expense_date;
                foreach (var data in values.GetMultipleexpenseSummary)
                {
                    msSQL = "select bank_name from acc_mst_tbank where bank_gid='" + values.bankname + "'";
                    string lsbank = objdbconn.GetExecuteScalar(msSQL);
                    double lspayment_amount;
                    if (data.payment_amount.Contains(","))
                    {
                        lspayment_amount = Convert.ToDouble(data.payment_amount.Replace(",", ""));
                    }
                    else
                    {
                        lspayment_amount = Convert.ToDouble(data.payment_amount);
                    }
                    //double lspayment_amount = data.payment_amount;

                    double lstdsamount;

                    if (data.tds_amount.Contains(","))
                    {
                        lstdsamount = Convert.ToDouble(data.tds_amount.Replace(",", ""));
                    }
                    else
                    {
                        lstdsamount = Convert.ToDouble(data.tds_amount);
                    }
                    double outstanding_amount = data.outstanding;
                    expense_date = data.expense_date;
                    double total = data.advance + lspayment_amount + lstdsamount + data.balancepo_advance;
                    if (data.expense_amount < total)
                    {
                        values.message = "Payment amount cannot be more than expense amount ";
                    }
                    else if (lspayment_amount > outstanding_amount)
                    {
                        values.message = "Payment amount cannot be more than outstanding amount";
                    }
                    else if (lspayment_amount == 0)
                    {
                        values.message = "Payment amount must be numeric";
                    }
                    else
                    {
                        string msGetGID = objcmnfunctions.GetMasterGID("SPYC");
                        string msPYGetGID = objcmnfunctions.GetMasterGID("SPYC");
                        double lsexpense_Amount = data.expense_amount;
                        string lsexpense_remarks = data.remark;
                        double lsAdvance_amount = data.advance;
                        double lspayment_amount_currency = lspayment_amount;
                        double lsexpense_amount_currency = data.expense_amount;
                        double lsadvance_amount_currency = data.advance;
                        msSQL = " insert into acp_trn_tsudryexpensespaymentdtl (" +
                               " paymentdtl_gid, " +
                               " payment_gid, " +
                               " payment_amount, " +
                               " expense_amount, " +
                               " advance_amount, " +
                               " expense_remarks," +
                               " payment_amount_L," +
                               " expense_Amount_L," +
                               " advance_amount_L" + " )values ( " +
                               "'" + msGetGID + "', " +
                               "'" + msPYGetGID + "', " +
                               "'" + lspayment_amount + "', " +
                               "'" + lsexpense_Amount + "', " +
                               "'" + lsAdvance_amount + "', " +
                               "'" + lsexpense_remarks +
                               "'," + "'" + lspayment_amount_currency + "'," +
                               "'" + lsexpense_amount_currency + "'," +
                               "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        string mspGetGID = objcmnfunctions.GetMasterGID("SIPC");

                        msSQL = " insert into acc_trn_tsundryexpenses2payment (" +
                                " expense2payment_gid, " +
                                " payment_gid, " +
                                " paymentdtl_gid, " +
                                " expense_gid, " +
                                " expense_amount, " +
                                " advance_amount, " +
                                " payment_amount," +
                                " payment_amount_L," +
                                " expense_Amount_L," +
                                " advance_amount_L" + " )values ( " +
                                "'" + mspGetGID + "'," +
                                "'" + msPYGetGID + "'," +
                                "'" + msGetGID + "'," +
                                "'" + data.expense_gid + "'," +
                                "'" + lsexpense_Amount + "'," +
                                "'" + lsAdvance_amount + "'," +
                                "'" + lspayment_amount + "'," +
                                "'" + lspayment_amount_currency + "'," +
                                "'" + lsexpense_amount_currency + "'," + "" +
                                "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        double lspay = lstdsamount + lspayment_amount;
                        msSQL = "select payment_amount  from acc_trn_tsundryexpenses  where expense_gid = '" + data.expense_gid + "'";
                        string isamount = objdbconn.GetExecuteScalar(msSQL);
                        if (isamount == "") { isamount = "0"; }
                        if (isamount != null)
                        {
                            double isamount1 = double.Parse(isamount);
                            lspay = lspay + isamount1;
                        }
                        msSQL = " Update acc_trn_tsundryexpenses " +
                            " Set payment_amount = '" + lspay + "'," +
                            " advance_amount = '" + lsAdvance_amount + "'";
                        if (lspayment_amount == outstanding_amount)
                        {
                            msSQL += " ,expense_status = 'IV Completed'";
                        }
                        else
                        {
                            msSQL += " ,expense_status = 'IV Work In Progress'";
                        }

                        msSQL += " where expense_gid = '" + data.expense_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        foreach (var data1 in values.Getmultipleexpense2employeedtl)
                        {
                            {
                                msSQL = " insert into acp_trn_tsundryexpensepayment (" +
                                    " payment_gid, " +
                                    " user_gid, " +
                                    " payment_date," +
                                    " created_date," +
                                    " vendor_gid, " +
                                    " payment_remarks, " +
                                    " payment_reference, " +
                                    " purchaseorder_gid, " +
                                    " payment_mode, " +
                                    " bank_name, " +
                                    " branch_name, " +
                                    " cheque_no, " +
                                    " city_name, " +
                                    " dd_no, " +
                                    " advance_total, " +
                                    " payment_total, " +
                                    " payment_status, " +
                                    " currency_code," +
                                    " exchange_rate," +
                                    " tds_amount," +
                                    " priority, " +
                                    " priority_remarks," +
                                    " bank_gid," +
                                    " payment_from," +
                                    " cheque_date," +
                                    " tdscalculated_finalamount" + " ) values (" +
                                    "'" + msPYGetGID + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                     "'" + data1.vendor_gid + "'," +
                                     "'" + values.payment_remarks + "'," +
                                     "'" + values.payment_note + "'," +
                                     "'" + data.purchaseorder_gid + "'," +
                                     "'" + values.payment_mode + "'," +
                                     "'" + lsbank + "'," +
                                     "'" + values.branch_name + "'," +
                                     "'" + values.cheque_no + "'," +
                                     "'" + data1.city + "'," +
                                     "'" + values.dd_no + "'," +
                                     "'" + data.advance + "'," +
                                     "'" + lspayment_amount + "'," +
                                     "'Payment Done'," +
                                     "'" + data1.currency_code + "'," +
                                     "'" + data1.exchange_rate + "'," +
                                     "'" + lstdsamount + "'," +
                                     "'" + values.priority + "', " +
                                     "'" + values.textbox + "', " +
                                     "'" + values.bankname + "', " +
                                     "'expense completed', " +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                     "'" + total + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }

                        double adjustment_amount = 0.00;
                        objfincmn.finance_payment("Purchase", values.payment_mode, values.bankname, DateTime.Now.ToString("yyyy-MM-dd"), lspayment_amount, values.bankname, "Payment", "PMR", values.vendor_gid, values.payment_remarks, msPYGetGID, lstdsamount, adjustment_amount, total, msPYGetGID);

                        msSQL = " Update acc_trn_tsundryexpenses " +
                                  " Set advance_amount = '" + lsAdvance_amount + "'," +
                                  " payment_amount = '" + lspay + "'" +
                                  " where expense_gid = '" + data.expense_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " select expense_amount, payment_amount, advance_amount from acc_trn_tsundryexpenses  where expense_gid =  '" + data.expense_gid + "' and " +
                            " expense_amount > (payment_amount + advance_amount)";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader.HasRows)
                        {
                            lsexpensestatus = "IV Work In Progress";
                            lspaymentflag = "Payment Done Partial";
                        }
                        else
                        {
                            lsexpensestatus = "IV Completed";
                            lspaymentflag = "Payment Done";
                        }



                        msSQL = " Update acc_trn_tsundryexpenses " +
                                   " Set expense_status = '" + lsexpensestatus + "'," +
                                   " payment_flag = '" + lspaymentflag + "'" +
                                   " where expense_gid = '" + data.expense_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)

                        {

                            values.status = true;
                            values.message = "Payment Done Sucessfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error occured while Payment";
                        }
                    }

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        //public Dictionary<string, object> DaGetPaymentrpt(string payment_gid, MdlAccTrnPayment values, string branch_gid)
        //{

        //    OdbcConnection myConnection = new OdbcConnection();
        //    myConnection.ConnectionString = objdbconn.GetConnectionString();
        //    OdbcCommand MyCommand = new OdbcCommand();
        //    MyCommand.Connection = myConnection;
        //    DataSet myDS = new DataSet();
        //    OdbcDataAdapter MyDA = new OdbcDataAdapter();

        //    msSQL = " select a.payment_gid,date_format(a.payment_date ,'%d-%m-%y') as payment_date,a.vendor_gid,a.payment_remarks,a.payment_total,a.payment_status,a.user_gid, " +
        //            " date_format(a.created_date,'%d-%m-%y') as " +
        //            " created_date,a.payment_reference,a.purchaseorder_gid,a.advance_total,a.payment_mode,a.bank_name,a.branch_name, " +
        //            " concat(a.cheque_no,a.dd_no)as cheque_no,a.city_name,a.currency_code,a.exchange_rate,a.tds_amount,a.tdscalculated_finalamount," +
        //            " a.priority,a.priority_remarks,a.approved_by,a.approved_date,a.reject_reason,a.bank_gid,a.payment_from, " +
        //            " a.addon_amount,a.additional_discount,a.additional_gid,a.discount_gid,b.*,c.*,CASE WHEN f.address2 IS NOT NULL THEN CONCAT(f.address1, ' ', f.address2) ELSE f.address1 END AS address1,f.city,f.state,f.postal_code  " +
        //            " from acp_trn_tsundryexpensepayment a " +
        //            " left join acp_trn_tsudryexpensespaymentdtl c on a.payment_gid=c.payment_gid " +
        //            " left join acp_mst_tvendor b on b.vendor_gid=a.vendor_gid " +
        //            " left join adm_mst_taddress f on b.address_gid=f.address_gid  " +
        //            " where a.payment_gid='" + payment_gid + "' ";

        //    MyCommand.CommandText = msSQL;
        //    MyCommand.CommandType = System.Data.CommandType.Text;
        //    MyDA.SelectCommand = MyCommand;
        //    myDS.EnforceConstraints = false;
        //    MyDA.Fill(myDS, "DataTable1");


        //    //currency number2words = fnconvertnumbertowords(payment_gid, "PO_REPORT");

        //    msSQL = " select a.branch_name, a.address1 as branch_address, a.city, a.state, a.postal_code," +
        //            " a.branch_logo, a.contact_number, a.email, a.tin_number, a.cst_number,b.currency_code,c.currency_symbol," +
        //            " a.authorise_sign from hrm_mst_tbranch a  left join acc_trn_tsundryexpenses x on a.branch_gid=x.branch_gid" +
        //            " left join acc_trn_tsundryexpenses2payment y on x.expense_gid=y.expense_gid" +
        //            " left join acp_trn_tsundryexpensepayment b on y.payment_gid=b.payment_gid" +
        //            " left join crm_trn_tcurrencyexchange c on b.currency_code = c.currency_code " +
        //            " where b.payment_gid='" + payment_gid + "'";


        //    MyCommand.CommandText = msSQL;
        //    MyCommand.CommandType = System.Data.CommandType.Text;
        //    MyDA.SelectCommand = MyCommand;
        //    myDS.EnforceConstraints = false;
        //    MyDA.Fill(myDS, "DataTable2");

        //    msSQL = "select  (branch_logo_path) as company_logo_Path  from hrm_mst_tbranch where branch_gid = '" + branch_gid + "' and  branch_logo_path is not null";

        //    dt1 = objdbconn.GetDataTable(msSQL);
        //    DataTable3.Columns.Add("company_logo_Path", typeof(byte[]));
        //    if (dt1.Rows.Count != 0)
        //    {
        //        foreach (DataRow dr_datarow in dt1.Rows)
        //        {
        //            company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo_Path"].ToString().Replace("../../", ""));

        //            if (System.IO.File.Exists(company_logo_path) == true)
        //            {
        //                //Convert  Image Path to Byte
        //                branch_logo = System.Drawing.Image.FromFile(company_logo_path);
        //                byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
        //                DataRow newRow = DataTable3.NewRow();
        //                newRow["company_logo_Path"] = bytes;
        //                DataTable3.Rows.Add(bytes);
        //            }
        //        }
        //    }
        //    dt1.Dispose();
        //    DataTable3.TableName = "DataTable3";
        //    myDS.Tables.Add(DataTable3);

        //    ReportDocument oRpt = new ReportDocument();
        //    oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_pmr"].ToString(), "pbl_crp_paymentadvicereport.rpt"));
        //    oRpt.SetDataSource(myDS);
        //    string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Payment Receipt" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
        //    oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
        //    myConnection.Close();

        //    var ls_response = objFnazurestorage.reportStreamDownload(path);
        //    File.Delete(path);
        //    return ls_response;

        //}



        public void DaGetSingleaddPaymentSummary(string expense_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = " select distinct a.expense_gid,a.expense_refno,a.advance_amount,replace (a.expense_status,'IV','Expense') as expense_status, format(a.expense_amount,2) as expense_amount, a.vendor_gid, a.expense_remarks,a.expense_from," +
                        " date_format(a.expense_date,'%d-%m-%Y') as expense_date, format(a.payment_amount + a.advance_amount+a.debit_note,2) as payed_amount, b.vendor_companyname,  " +
                        " format(ABS(a.expense_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding ,date_format(a.payment_date,'%d-%m-%Y') as due_date from acc_trn_tsundryexpenses a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid where a.expense_gid in('" + expense_gid + "') order by a.expense_date desc, a.expense_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<singleexpenselist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new singleexpenselist
                        {
                            expense_gid = dt["expense_gid"].ToString(),
                            advance_amount = dt["advance_amount"].ToString(),
                            expense_status = dt["expense_status"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            expense_remarks = dt["expense_remarks"].ToString(),
                            expense_from = dt["expense_from"].ToString(),
                            expense_date = dt["expense_date"].ToString(),
                            payed_amount = dt["payed_amount"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            outstanding = dt["outstanding"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            expense_refno = dt["expense_refno"].ToString(),

                        });
                        values.singleexpenselist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }



        public void DaPostsinglepayment(multipleexpense2singlepayment values, string user_gid)
        {
            try
            {
                string expense_date;
                foreach (var data in values.GetMultipleexpenseSummary)
                {
                    msSQL = "select bank_name from acc_mst_tbank where bank_gid='" + values.bankname + "'";
                    string lsbank = objdbconn.GetExecuteScalar(msSQL);

                    double lspayment_amount;

                    if (data.payment_amount.Contains(","))
                    {
                        lspayment_amount = Convert.ToDouble(data.payment_amount.Replace(",", ""));
                    }
                    else
                    {
                        lspayment_amount = Convert.ToDouble(data.payment_amount);
                    }


                    double lstdsamount;

                    if (string.IsNullOrWhiteSpace(data.tds_amount))
                    {
                        lstdsamount = 0;
                    }
                    else if (data.tds_amount.Contains(","))
                    {
                        lstdsamount = Convert.ToDouble(data.tds_amount.Replace(",", ""));
                    }
                    else
                    {
                        lstdsamount = Convert.ToDouble(data.tds_amount);
                    }



                    //double lspayment_amount = data.payment_amount;
                    double outstanding_amount = data.outstanding;
                    expense_date = data.expense_date;
                    double total = data.advance + lspayment_amount + lstdsamount + data.balancepo_advance;
                    if (data.expense_amount < total)
                    {
                        values.message = "Payment amount cannot be more than expense amount ";
                        return;
                    }
                    else if (lspayment_amount > outstanding_amount)
                    {
                        values.message = "Payment amount cannot be more than outstanding amount";
                        return;
                    }
                    else if (lspayment_amount == 0)
                    {
                        values.message = "Payment amount must be numeric";
                        return;
                    }
                    else
                    {
                        string msGetGID = objcmnfunctions.GetMasterGID("SPYC");
                        string msPYGetGID = objcmnfunctions.GetMasterGID("SPYC");
                        double lsexpense_Amount = data.expense_amount;
                        string lsexpense_remarks = data.remark;
                        double lsAdvance_amount = data.advance;
                        double lspayment_amount_currency = lspayment_amount;
                        double lsexpense_amount_currency = data.expense_amount;
                        double lsadvance_amount_currency = data.advance;
                        msSQL = " insert into acp_trn_tsudryexpensespaymentdtl (" +
                               " paymentdtl_gid, " +
                               " payment_gid, " +
                               " payment_amount, " +
                               " expense_amount, " +
                               " advance_amount, " +
                               " expense_remarks," +
                               " payment_amount_L," +
                               " expense_Amount_L," +
                               " advance_amount_L" + " )values ( " +
                               "'" + msGetGID + "', " +
                               "'" + msPYGetGID + "', " +
                               "'" + lspayment_amount + "', " +
                               "'" + lsexpense_Amount + "', " +
                               "'" + lsAdvance_amount + "', " +
                               "'" + lsexpense_remarks +
                               "'," + "'" + lspayment_amount_currency + "'," +
                               "'" + lsexpense_amount_currency + "'," +
                               "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        string mspGetGID = objcmnfunctions.GetMasterGID("SIPC");

                        msSQL = " insert into acc_trn_tsundryexpenses2payment (" +
                                " expense2payment_gid, " +
                                " payment_gid, " +
                                " paymentdtl_gid, " +
                                " expense_gid, " +
                                " expense_amount, " +
                                " advance_amount, " +
                                " payment_amount," +
                                " payment_amount_L," +
                                " expense_Amount_L," +
                                " advance_amount_L" + " )values ( " +
                                "'" + mspGetGID + "'," +
                                "'" + msPYGetGID + "'," +
                                "'" + msGetGID + "'," +
                                "'" + data.expense_gid + "'," +
                                "'" + lsexpense_Amount + "'," +
                                "'" + lsAdvance_amount + "'," +
                                "'" + lspayment_amount + "'," +
                                "'" + lspayment_amount_currency + "'," +
                                "'" + lsexpense_amount_currency + "'," + "" +
                                "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        double lspay = lstdsamount + lspayment_amount;
                        msSQL = "select payment_amount  from acc_trn_tsundryexpenses  where expense_gid = '" + data.expense_gid + "'";
                        string isamount = objdbconn.GetExecuteScalar(msSQL);
                        if (isamount == "") { isamount = "0"; }
                        if (isamount != null)
                        {
                            double isamount1 = double.Parse(isamount);
                            lspay = lspay + isamount1;
                        }
                        msSQL = " Update acc_trn_tsundryexpenses " +
                            " Set payment_amount = '" + lspay + "'," +
                            " advance_amount = '" + lsAdvance_amount + "'";
                        if (lspayment_amount == outstanding_amount)
                        {
                            msSQL += " ,expense_status = 'IV Completed'";
                        }
                        else
                        {
                            msSQL += " ,expense_status = 'IV Work In Progress'";
                        }

                        msSQL += " where expense_gid = '" + data.expense_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        foreach (var data1 in values.Getmultipleexpense2employeedtl)
                        {
                            {
                                msSQL = " insert into acp_trn_tsundryexpensepayment (" +
                                    " payment_gid, " +
                                    " user_gid, " +
                                    " payment_date," +
                                    " created_date," +
                                    " vendor_gid, " +
                                    " payment_remarks, " +
                                    " payment_reference, " +
                                    " purchaseorder_gid, " +
                                    " payment_mode, " +
                                    " bank_name, " +
                                    " branch_name, " +
                                    " cheque_no, " +
                                    " city_name, " +
                                    " dd_no, " +
                                    " advance_total, " +
                                    " payment_total, " +
                                    " payment_status, " +
                                    " currency_code," +
                                    " exchange_rate," +
                                    " tds_amount," +
                                    " priority, " +
                                    " priority_remarks," +
                                    " bank_gid," +
                                    " payment_from," +
                                    " cheque_date," +
                                    " tdscalculated_finalamount" + " ) values (" +
                                    "'" + msPYGetGID + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                     "'" + data1.vendor_gid + "'," +
                                     "'" + values.payment_remarks + "'," +
                                     "'" + values.payment_note + "'," +
                                     "'" + data.purchaseorder_gid + "'," +
                                     "'" + values.payment_mode + "'," +
                                     "'" + lsbank + "'," +
                                     "'" + values.branch_name + "'," +
                                     "'" + values.cheque_no + "'," +
                                     "'" + data1.city + "'," +
                                     "'" + values.dd_no + "'," +
                                     "'" + data.advance + "'," +
                                     "'" + lspayment_amount + "'," +
                                     "'Payment Done'," +
                                     "'" + data1.currency_code + "'," +
                                     "'" + data1.exchange_rate + "'," +
                                     "'" + lstdsamount + "'," +
                                     "'" + values.priority + "', " +
                                     "'" + values.textbox + "', " +
                                     "'" + values.bankname + "', " +
                                     "'expense completed', " +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                     "'" + total + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                        double adjustment_amount = 0.00;
                        objfincmn.finance_payment("Purchase", values.payment_mode, values.bankname, DateTime.Now.ToString("yyyy-MM-dd"), lspayment_amount, values.bankname, "Payment", "PMR", values.vendor_gid, values.payment_remarks, msPYGetGID, lstdsamount, adjustment_amount, total, msPYGetGID);
                        msSQL = " Update acc_trn_tsundryexpenses " +
                                " Set advance_amount = '" + lsAdvance_amount + "'," +
                                " payment_amount = '" + lspay + "'" +
                                " where expense_gid = '" + data.expense_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " select expense_amount, payment_amount, advance_amount from acc_trn_tsundryexpenses  where expense_gid =  '" + data.expense_gid + "' and " +
                            " expense_amount > (payment_amount + advance_amount)";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader.HasRows)
                        {
                            lsexpensestatus = "IV Work In Progress";
                            lspaymentflag = "Payment Done Partial";
                        }
                        else
                        {
                            lsexpensestatus = "IV Completed";
                            lspaymentflag = "Payment Done";
                        }



                        msSQL = " Update acc_trn_tsundryexpenses " +
                                   " Set expense_status = '" + lsexpensestatus + "'," +
                                   " payment_flag = '" + lspaymentflag + "'" +
                                   " where expense_gid = '" + data.expense_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Payment Done Sucessfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error occured while Payment";
                        }
                    }

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetSundryPaymentViewDetails(string payment_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = "select a.vendor_gid, a.payment_date,b.vendor_companyname,b.contactperson_name,b.email_id, a.currency_code, a.exchange_rate "+
                    " ,c.address1,c.address2 from acp_trn_tsundryexpensepayment a " +
                    " left JOIN acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                    " left join adm_mst_taddress c on b.address_gid = c.address_gid " +
                    " where a.payment_gid = '" + payment_gid +"'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSundryPayment = new List<GetSundryPaymentDetails_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow ds in dt_datatable.Rows)
                    {
                        GetSundryPayment.Add(new GetSundryPaymentDetails_list
                        {
                            vendor_gid = ds["vendor_gid"].ToString(),
                            payment_date = ds["payment_date"].ToString(),
                            vendor_companyname = ds["vendor_companyname"].ToString(),
                            contactperson_name = ds["contactperson_name"].ToString(),
                            email_id = ds["email_id"].ToString(),
                            currency_code = ds["currency_code"].ToString(),
                            exchange_rate = ds["exchange_rate"].ToString(),
                            address1 = ds["address1"].ToString(),
                            address2 = ds["address2"].ToString(),
                        });
                        values.GetSundryPaymentDetails_list = GetSundryPayment;
                    }
                }
            }
            catch(Exception ex)
            {

            }
        } public void DaGetSundryPaymentView(string payment_gid, MdlAccTrnPayment values)
        {
            try
            {
                msSQL = "Select a.expense2payment_gid,c.tds_amount, a.payment_gid, a.paymentdtl_gid, a.expense_gid," +
                    "   format(a.expense_amount, 2) as expense_amount, format(a.advance_amount, 2) as po_advance," +
                    "  format(a.payment_amount, 2) as payment_amount, b.expense_remarks ,c.payment_reference" +
                    "  FROM acc_trn_tsundryexpenses2payment a  " +
                    "left join acp_trn_tsudryexpensespaymentdtl b on a.paymentdtl_gid = b.paymentdtl_gid  " +
                    "   left join acp_trn_tsundryexpensepayment c on a.payment_gid = c.payment_gid  " +
                    "    where a.payment_gid = '" + payment_gid +"' order by a.payment_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSundryPayment = new List<GetSundryPayment_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow ds in dt_datatable.Rows)
                    {
                        GetSundryPayment.Add(new GetSundryPayment_list
                        {
                            expense2payment_gid = ds["expense2payment_gid"].ToString(),
                            tds_amount = ds["tds_amount"].ToString(),
                            payment_gid = ds["payment_gid"].ToString(),
                            paymentdtl_gid = ds["paymentdtl_gid"].ToString(),
                            expense_gid = ds["expense_gid"].ToString(),
                            expense_amount = ds["expense_amount"].ToString(),
                            po_advance = ds["po_advance"].ToString(),
                            payment_amount = ds["payment_amount"].ToString(),
                            expense_remarks = ds["expense_remarks"].ToString(),
                            payment_reference = ds["payment_reference"].ToString(),
                        });
                        values.GetSundryPayment_list = GetSundryPayment;
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}