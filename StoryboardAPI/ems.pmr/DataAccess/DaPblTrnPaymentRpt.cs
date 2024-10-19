using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.pmr.Models;
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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using static System.Drawing.ImageConverter;

using System.Text;
using System.Web;


namespace ems.pmr.DataAccess
{
    public class DaPblTrnPaymentRpt
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, lspaymentflag, lsinvoicestatus, exchange_type,
            msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        double exchange1;
        string lsoutstanding, lsadvance;
        string company_logo_path;
     System.Drawing.Image branch_logo;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();
        DataTable DataTable3 = new DataTable();
        finance_cmnfunction objfincmn = new finance_cmnfunction();

        public void DaGetPaymentRptSummary(MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select /*+ MAX_EXECUTION_TIME(300000) */ distinct a.payment_gid,h.invoice_gid,a.payment_from, case when a.payment_status='PY Approved' then 'Payment Done'" +                    
                        " when a.payment_status='PY Pending' then 'Payment Pending' when a.payment_status='PY Canceled' then 'Payment Canceled' else a.payment_status end as payment_status," +
                        "CASE WHEN j.poref_no IS NULL OR j.poref_no = '' THEN CASE WHEN j.purchaseorder_gid IS NULL OR j.purchaseorder_gid ='' THEN 'Direct Invoice' ELSE j.purchaseorder_gid  END ELSE j.poref_no  END AS porefno," +
                        " a.vendor_gid,l.vendorinvoiceref_no, format(a.payment_total,2) as payment_total,DATE_FORMAT(a.payment_date,'%d-%m-%Y')  as payment_date,k.costcenter_name,a.payment_reference, " +
                        " c.vendor_code, c.vendor_companyname,a.purchaseorder_gid, case when a.payment_mode='Cash' then a.payment_mode " +
                        " when a.payment_mode='CREDITCARD' then concat(cast(a.payment_mode as char), '/' , cast(d.bank_name as char)) " +
                        " when a.payment_mode='Cheque' then concat(cast(a.payment_mode as char), '/' , a.cheque_no,'/', cast(a.bank_name as char))" +
                        " when a.payment_mode='NEFT' then concat(cast(a.payment_mode as char), '/' , cast(a.bank_name as char))" +
                        " when a.payment_mode='DD' then concat(cast(a.payment_mode as char), '/' , cast(a.bank_name as char)) else a.payment_mode end as payment_mode," +
                        " format(a.tds_amount,2)as tds_amount, format(a.tdscalculated_finalamount,2)as tdscalculated_finalamount from acp_trn_tpayment a" +
                        " left join acp_trn_tpaymentdtl b on a.payment_gid = b.payment_gid left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid left join acp_trn_tinvoice2payment h on h.payment_gid=a.payment_gid " +
                        " left join acc_mst_tcreditcard d on a.bank_gid=d.bank_gid  left join pmr_trn_tpurchaseorder j on a.purchaseorder_gid=j.purchaseorder_gid " +
                        " left join pmr_mst_tcostcenter k on j.costcenter_gid=k.costcenter_gid  left join acp_trn_tinvoice l on c.vendor_gid=l.vendor_gid " +
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
                            invoice_gid = dt["invoice_gid"].ToString(),
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
                            //invoice_amount = dt["invoice_amount1"].ToString(),
                            
                            

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

        public void DaGetInvoicedetails(string invoice_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = "select invoice_refno,date_format(invoice_date,'%d-%m-%y') as invoice_date,invoice_amount from acp_trn_tinvoice where invoice_gid = '" + invoice_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetInvoice>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetInvoice
                        {

                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),

                        });
                        values.getinvoice = getModuleList;
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

        public void DaGetPaymentview(string payment_gid, string user_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " SELECT a.payment_gid,c.address1,d.country,date_format(a.payment_date,'%d-%m-%Y') as payment_date, a.vendor_gid, a.payment_remarks, a.payment_status," +
                        " a.payment_mode,a.tds_amount ,a.bank_name, a.branch_name, a.cheque_no, a.city_name, a.dd_no, a.currency_code," +
                        " a.exchange_rate, format(a.payment_total, 2) as payment_total," +
                        " a.payment_reference,b.vendor_companyname,b.contactperson_name,b.contact_telephonenumber," +
                        " b.email_id,concat(c.address1, c.city, c.postal_code, c.state, d.country) as vendoraddress,c.fax" +
                        " FROM acp_trn_tpayment a left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
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


        public void DagetPaymenamount(string payment_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " Select a.invoice2payment_gid,c.tds_amount, a.payment_gid, a.paymentdtl_gid, a.invoice_gid, " +
                        " format(a.invoice_amount, 2) as invoice_amount, format(a.advance_amount, 2) as po_advance, " +
                        " format(a.payment_amount, 2) as payment_amount, b.invoice_remarks ,c.payment_reference ,b.exchangegain,b.exchangeloss,b.bankcharges " +
                        " FROM acp_trn_tinvoice2payment a " +
                        " left join acp_trn_tpaymentdtl b on a.paymentdtl_gid = b.paymentdtl_gid " +
                        " left join acp_trn_tpayment c on a.payment_gid = c.payment_gid " +
                        " where a.payment_gid = '" + payment_gid + "' order by a.payment_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentamount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentamount_list
                        {
                            invoice2payment_gid = dt["invoice2payment_gid"].ToString(),
                            payment_gid = dt["payment_gid"].ToString(),
                            paymentdtl_gid = dt["paymentdtl_gid"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            po_advance = dt["po_advance"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            payment_reference = dt["payment_reference"].ToString(),
                            tds_amount = dt["tds_amount"].ToString(),
                            exchangegain = dt["exchangegain"].ToString(),
                            exchangeloss = dt["exchangeloss"].ToString(),
                            bankcharges = dt["bankcharges"].ToString(),
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

        public void DaGetPaymentAddproceed(MdlPblTrnPaymentRpt values, string vendorname)
        {
            try
            {
                if (vendorname != null)
                {


                    msSQL = " select a.invoice_gid,b.vendor_companyname,b.vendor_gid,a.invoice_status,format(sum(a.invoice_amount),2)as invoice_amount," +
                    " format(sum(a.payment_amount),2) as payment_amount,format(sum(a.invoice_amount-a.payment_amount),2) as outstanding,a.invoice_from, " +
                    " case when b.contact_telephonenumber is null then  concat(b.contactperson_name,'/',b.email_id) " +
                    " when b.contact_telephonenumber is not null then concat(b.contactperson_name,'/',b.contact_telephonenumber,'/',b.email_id) end as contact " +
                    " from acp_mst_tvendor b" +
                    " left join acp_trn_tinvoice a on b.vendor_gid=a.vendor_gid where " +
                    " a.invoice_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and " +
                    " ((a.invoice_flag = 'Payment Pending' and a.payment_flag ='PY Pending') or " +
                    " (a.invoice_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and  " +
                    " a.invoice_from <> 'Expenses' and b.vendor_companyname like '%" + vendorname + "%' " +
                    " group by b.vendor_gid order by a.invoice_gid desc ";

                }

                else
                {
                    msSQL = " select a.invoice_gid,b.vendor_companyname,b.vendor_gid,a.invoice_status,format(sum(a.invoice_amount),2)as invoice_amount," +
                    " format(sum(a.payment_amount),2) as payment_amount,format(sum(a.invoice_amount-a.payment_amount),2) as outstanding,a.invoice_from, " +
                    " case when b.contact_telephonenumber is null then  concat(b.contactperson_name,'/',b.email_id) " +
                    " when b.contact_telephonenumber is not null then concat(b.contactperson_name,'/',b.contact_telephonenumber,'/',b.email_id) end as contact " +
                    " from acp_mst_tvendor b" +
                    " left join acp_trn_tinvoice a on b.vendor_gid=a.vendor_gid where " +
                    " a.invoice_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and " +
                    " ((a.invoice_flag = 'Payment Pending' and a.payment_flag ='PY Pending') or " +
                    " (a.invoice_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and a.invoice_from <> 'Expenses' " +
                    " group by b.vendor_gid order by a.invoice_gid desc ";

                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentadd>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentadd
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            outstanding = dt["outstanding"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
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

        public void DaGetMakepaymentExpand(string vendor_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select distinct a.invoice_gid,a.invoice_refno, date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, a.invoice_from,a.vendor_gid, a.vendor_contact_person, a.vendor_address, a.invoice_remarks, " +
                    " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag " +
                    " else a.invoice_flag end as 'overall_status', " +
                    " format(a.invoice_amount, 2) as invoice_amount, a.invoice_status, a.user_gid, a.created_date, a.invoice_reference,a.agreement_gid, " +
                    " format(ABS(a.payment_amount + a.advance_amount+a.debit_note),2) as payed_amount,format(ABS(a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding," +
                    " a.payment_date,a.invoice_flag,case when d.purchaseorder_gid is null then a.invoice_reference else d.purchaseorder_gid end as purchaseorder_gid , " +
                    " c.vendor_companyname,date_format(a.payment_date,'%d-%m-%Y') as due_date,b.producttype_gid " +
                    " from acp_trn_tinvoice a " +
                    " left join acp_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                    " left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid " +
                    " left join acp_trn_tpo2invoice d on a.invoice_gid = d.invoice_gid  " +
                    " where a.invoice_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and" +
                    " ((a.invoice_flag = 'Payment Pending' and a.payment_flag ='PY Pending') or " +
                    " (a.invoice_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and a.invoice_from <> 'Expenses' and a.vendor_gid='" + vendor_gid + "' " +
                    "group by a.invoice_gid  order by a.invoice_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentExpand>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentExpand
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_contact_person = dt["vendor_contact_person"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            payed_amount = dt["payed_amount"].ToString(),
                            outstanding = dt["outstanding"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),

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

        public void DaProductdetails(string invoice_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " SELECT a.invoice_gid,a.invoicedtl_gid, a.product_gid,b.product_code,c.invoice_refno,format(a.product_total,2) as product_total , " +
                                " a.qty_invoice, b.product_name " +
                                " FROM acp_trn_tinvoicedtl a " +
                                " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                                " left join acp_trn_tinvoice c on a.invoice_gid=c.invoice_gid" +
                                " where a.invoice_gid = '" + invoice_gid + "'" +
                                " order by a.invoicedtl_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productdetail_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productdetail_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
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

        public void DaGetSinglePaymentSummary(string vendor_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select distinct a.invoice_gid, a.invoice_status, format(a.invoice_amount,2) as invoice_amount, a.vendor_gid, a.invoice_remarks,a.invoice_from," +
                        " case when d.purchaseorder_gid is null then a.invoice_reference else d.purchaseorder_gid end as purchaseorder_gid, " +
                        " date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, format(a.payment_amount + a.advance_amount+a.debit_note,2) as payed_amount, b.vendor_companyname,  " +
                        " format(ABS(a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding ,date_format(a.payment_date,'%d-%m-%Y') as due_date from acp_trn_tinvoice a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                        " left join acp_trn_tpo2invoice d on a.invoice_gid = d.invoice_gid " +
                        " where a.invoice_amount<> ABS((a.payment_amount+a.advance_amount + a.debit_note)) and" +
                        " ((a.invoice_flag = 'Payment Pending' and a.payment_flag = 'PY Pending') or" +
                        " (a.invoice_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and a.invoice_from<> 'Expenses' and b.vendor_gid = '" + vendor_gid + "' " +
                        " order by a.invoice_date desc, a.invoice_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<singlepayment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new singlepayment_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
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
        public void Dapaymentcancelsubmit(string payment_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = "select invoice_gid from acp_trn_Tinvoice2payment where payment_gid='" + payment_gid + "'";
                msSQL = " delete from acp_trn_tinvoice2payment " +
                    " where payment_gid = '" + payment_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                {
                    msSQL = " Update acp_trn_tpayment set " +
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
        public void DaGetpaymentCancel(string payment_gid, MdlPblTrnPaymentRpt values,string user_gid)
        {
            try
            {
                msSQL = msSQL = " SELECT a.payment_gid,c.address1,d.country, a.payment_date, a.vendor_gid, a.payment_remarks, " +
                    " a.payment_mode, a.bank_name, a.branch_name, a.cheque_no, a.city_name, a.dd_no,  " +
                    " format(a.payment_total, 2) as payment_total, a.payment_reference,b.vendor_companyname,b.contactperson_name,b.contact_telephonenumber, " +
                     " b.email_id,concat(c.address1, c.city, c.postal_code, c.state, d.country) as vendoraddress,c.fax " +
                     "FROM acp_trn_tpayment a" +
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
        public void DaGetBankDetail(MdlPblTrnPaymentRpt values)
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
        public void DaGetmultipleinvoice2employeedtl(string user_gid, string vendor_gid, MdlPblTrnPaymentRpt values)
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
                msSQL = " select currency_code  from crm_trn_Tcurrencyexchange where default_currency ='Y' ";
                string currency = objdbconn.GetExecuteScalar(msSQL);

                var getModuleList = new List<Getmultipleinvoice2employeedtl>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        
                        getModuleList.Add(new Getmultipleinvoice2employeedtl
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
                            currency= currency,
                            payment_date = "",
                            payment_remarks = "",
                            paymentnotes = "",
                            vendor_gid = dt["vendor_gid"].ToString(),
                            city = dt["city"].ToString(),

                        });
                        values.Getmultipleinvoice2employeedtl = getModuleList;
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
        public void DaGetpaymentInvoiceSummary(string vendor_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select distinct a.invoice_refno, a.invoice_gid, a.invoice_status,a.advance_amount, format(a.invoice_amount,2) as invoice_amount, a.vendor_gid, a.invoice_remarks,a.invoice_from," +
                        " case when d.purchaseorder_gid is null then 'Direct Invoice' else d.purchaseorder_gid end as purchaseorder_gid, " +
                        " date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, format(a.payment_amount + a.advance_amount+a.debit_note,2) as payed_amount, b.vendor_companyname,  " +
                        " format(ABS(a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding ,date_format(a.payment_date,'%d-%m-%Y') as due_date from acp_trn_tinvoice a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " + " left join acp_trn_tpo2invoice d on a.invoice_gid = d.invoice_gid " +
                        " where a.invoice_amount > ABS((a.payment_amount+a.advance_amount+a.debit_note)) and" + " ((a.invoice_flag = 'Payment Pending' ) or " +
                        " (a.invoice_status = 'IV Completed')) and b.vendor_gid='" + vendor_gid + "' and a.invoice_from <> 'Expenses'";

                msSQL += " order by a.invoice_date desc, a.invoice_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMultipleInvoiceSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        if (dt["invoice_from"].ToString() == "Services")
                        {
                            msSQL = " select format(a.advance_amount,2) as advance_amount, " +
                                " format(a.advance_amount - a.advance_amount_utilized,2)  as outstanding_advance,b.exchange_rate,b.currency_code " +
                                " from pbl_trn_tserviceorder a " +
                                " left join acp_trn_tinvoice b on a.serviceorder_gid= b.invoice_reference " +
                                " where serviceorder_gid='" + dt["purchaseorder_gid"].ToString() + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        }
                        else
                        {
                            msSQL = " select format(purchaseorder_advance,2) as advance_amount, " +
                                " format(purchaseorder_advance - purchaseorder_advance_utilized,2) as outstanding_advance, " +
                                " case when currency_code is null then 'INR' else currency_code end as currency_code," +
                                " case when exchange_rate is null then '1' else exchange_rate end as exchange_rate " +
                                " from pmr_trn_tpurchaseorder " + " where purchaseorder_gid = '" + dt["purchaseorder_gid"].ToString() + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        }
                        if (objOdbcDataReader.HasRows == true)
                        {
                            objOdbcDataReader.Read();   
                            objdbconn.OpenConn();
                            lsadvance = objOdbcDataReader["advance_amount"].ToString();
                            lsoutstanding = objOdbcDataReader["outstanding_advance"].ToString();
                            objOdbcDataReader.Close();


                        }
                        if (lsadvance == null) { lsadvance = "0.0"; }
                        if (lsoutstanding == null) { lsoutstanding = "0.0"; }
                        getModuleList.Add(new GetMultipleInvoiceSummary
                        {

                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            invoice_amount = double.Parse(dt["invoice_amount"].ToString()),
                            invoice_status = dt["invoice_status"].ToString(),
                            outstanding = double.Parse(dt["outstanding"].ToString()),
                            payed_amount = double.Parse(dt["payed_amount"].ToString()),
                            advance = double.Parse(dt["advance_amount"].ToString()),
                            payment_amount = 0.00.ToString(),
                            balancepo_advance = 0.00,
                            grand_total = 0.00,
                            tds_amount = 0.00.ToString(),
                            exchangeloss = 0.00.ToString(),
                            exchangegain = 0.00.ToString(),
                            bankcharges = 0.00.ToString(),
                            final_amount = "0.0",
                            totalpo_advance = 0.00,
                            remark = "",
                        });

                        values.GetMultipleInvoiceSummary = getModuleList;
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
        public void DaGetCardDetail(MdlPblTrnPaymentRpt values)
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
        public void DaPostmultipleinvoice2singlepayment(multipleinvoice2singlepayment values,string user_gid)
        {
            try
            {
                string invoice_date;
                foreach (var data in values.GetMultipleInvoiceSummary)
                {
                    msSQL = "select bank_name from acc_mst_tbank where bank_gid='"+values.bankname+"'";
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

                    double lsbankcharges ;

                    if (data.tds_amount.Contains(","))
                    {
                        lsbankcharges = Convert.ToDouble(data.bankcharges.Replace(",", ""));
                    }
                    else
                    {
                        lsbankcharges = Convert.ToDouble(data.bankcharges);
                    }
                    double lsexchangeloss;

                    if (data.tds_amount.Contains(","))
                    {
                        lsexchangeloss = Convert.ToDouble(data.exchangeloss.Replace(",", ""));
                    }
                    else
                    {
                        lsexchangeloss = Convert.ToDouble(data.exchangeloss);
                    }

                    double lsexchangegain;

                    if (data.tds_amount.Contains(","))
                    {
                        lsexchangegain = Convert.ToDouble(data.exchangegain.Replace(",", ""));
                    }
                    else
                    {
                        lsexchangegain = Convert.ToDouble(data.exchangegain);
                    }

                    double outstanding_amount = data.outstanding;
                    invoice_date = data.invoice_date;
                    double total =  data.advance + lspayment_amount + lstdsamount + data.balancepo_advance + lsbankcharges + lsexchangeloss + lsexchangegain;
                    //if (data.invoice_amount < total)
                    //{
                    //    values.message = "Payment amount cannot be more than invoice amount ";
                    //}
                    //else if (lspayment_amount > outstanding_amount)
                    //{
                    //    values.message = "Payment amount cannot be more than outstanding amount";
                    //}
                    //else if (lspayment_amount == 0)
                    if (lspayment_amount == 0)
                    {
                        values.message = "Payment amount must be numeric";
                    }
                    else
                    {
                        string msGetGID = objcmnfunctions.GetMasterGID("SPYC");
                        string msPYGetGID = objcmnfunctions.GetMasterGID("SPYC");
                        double lsInvoice_Amount = data.invoice_amount;
                        string lsInvoice_remarks = data.remark;
                        double lsAdvance_amount = data.advance;
                        double lspayment_amount_currency = lspayment_amount;
                        double lsinvoice_amount_currency = data.invoice_amount;
                        double lsadvance_amount_currency = data.advance;
                        msSQL = " insert into acp_trn_tpaymentdtl (" +
                               " paymentdtl_gid, " +
                               " payment_gid, " +
                               " payment_amount, " +
                               " invoice_amount, " +
                               " advance_amount, " +
                               " bankcharges, " +
                               " exchangeloss, " +
                               " exchangegain, " +
                               " invoice_remarks," +
                               " payment_amount_L," +
                               " invoice_Amount_L," +
                               " advance_amount_L" + " )values ( " +
                               "'" + msGetGID + "', " +
                               "'" + msPYGetGID + "', " +
                               "'" + lspayment_amount + "', " +
                               "'" + lsInvoice_Amount + "', " +
                               "'" + lsAdvance_amount + "', " +
                               "'" + lsbankcharges + "', " +
                               "'" + lsexchangeloss + "', " +
                               "'" + lsexchangegain + "', " +
                               "'" + lsInvoice_remarks + "'," +
                               "'" + lspayment_amount_currency + "'," +
                               "'" + lsinvoice_amount_currency + "'," +
                               "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        string mspGetGID = objcmnfunctions.GetMasterGID("SIPC");

                        msSQL = " insert into acp_trn_tinvoice2payment (" +
                                " invoice2payment_gid, " +
                                " payment_gid, " +
                                " paymentdtl_gid, " +
                                " invoice_gid, " +
                                " invoice_amount, " +
                                " advance_amount, " +
                                " payment_amount," +
                                " payment_amount_L," +
                                " invoice_Amount_L," +
                                " bankcharges, " +
                               " exchangeloss, " +
                               " exchangegain, " +
                                " advance_amount_L" + " )values ( " +
                                "'" + mspGetGID + "'," +
                                "'" + msPYGetGID + "'," +
                                "'" + msGetGID + "'," +
                                "'" + data.invoice_gid + "'," +
                                "'" + lsInvoice_Amount + "'," +
                                "'" + lsAdvance_amount + "'," +
                                "'" + lspayment_amount + "'," +
                                "'" + lspayment_amount_currency + "'," +
                                "'" + lsinvoice_amount_currency + "'," + 
                                  "'" + lsbankcharges + "', " +
                               "'" + lsexchangeloss + "', " +
                               "'" + lsexchangegain + "', " +
                                "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        double lspay = lstdsamount + lspayment_amount;
                        msSQL = "select payment_amount  from acp_trn_tinvoice  where invoice_gid = '" + data.invoice_gid + "'";
                        string isamount = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select exchange_rate  from acp_trn_tinvoice  where invoice_gid = '" + data.invoice_gid + "'";
                        string iexchange = objdbconn.GetExecuteScalar(msSQL);
                        double exchange = double.Parse(iexchange);
                        if (isamount == "") { isamount = "0"; }
                        if (isamount != null)
                        {
                            double isamount1 = double.Parse(isamount);                            
                            double overall = isamount1 * exchange;
                            lspay = lspay + overall;
                            
                        }
                        lspay = lspay / exchange;
                        msSQL = " Update acp_trn_tinvoice " +
                            " Set payment_amount = '" + lspay + "'," +
                            " advance_amount = '" + lsAdvance_amount + "'";
                        if (lspayment_amount == outstanding_amount)
                        {
                            msSQL += " ,invoice_status = 'IV Completed'";
                        }
                        else
                        {
                            msSQL += " ,invoice_status = 'IV Work In Progress'";
                        }

                        msSQL += " where invoice_gid = '" + data.invoice_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        foreach (var data1 in values.Getmultipleinvoice2employeedtl)
                        {
                            {
                                msSQL = " insert into acp_trn_tpayment (" +
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
                                     "'invoice completed', " +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                     "'" + total + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }

                        double adjustment_amount = 0.00;
                     
                        if (lsexchangegain == 0.00 && lsexchangeloss == 0.00)
                        {
                            exchange1 = 0.00;
                            exchange_type = "";
                        }
                        else if (lsexchangeloss != 0.00)
                        {
                            exchange1 = lsexchangeloss;
                           exchange_type = "Exchange Loss";
                        }
                        else if (lsexchangegain != 0.00)
                        {
                            exchange1 = lsexchangegain;
                           exchange_type = "Exchange Gain";
                        }

                        objfincmn.finance_payment("Purchase", values.payment_mode, values.bankname, DateTime.Now.ToString("yyyy-MM-dd"), lspayment_amount, values.bankname, "Payment", "PMR", values.vendor_gid, values.payment_remarks, msPYGetGID, lstdsamount, adjustment_amount, total, msPYGetGID);
                        objfincmn.jn_exchange_purchase(msPYGetGID, values.payment_remarks, exchange1, "Payment", "PMR", exchange_type);
                          objfincmn.jn_bankcharge(msPYGetGID, values.payment_remarks, lsbankcharges, values.bankname, "Payment", "PMR");
                        msSQL = " Update acp_trn_tinvoice " +
                                  " Set advance_amount = '" + lsAdvance_amount + "'," +
                                  " payment_amount = '" + lspay + "'" +
                                  " where invoice_gid = '" + data.invoice_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " select invoice_amount, payment_amount, advance_amount from acp_trn_tinvoice  where invoice_gid =  '" + data.invoice_gid + "' and " +
                            " invoice_amount > (payment_amount + advance_amount)";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader.HasRows)
                        {
                            objOdbcDataReader.Read();   
                            lsinvoicestatus = "IV Work In Progress";
                            lspaymentflag = "Payment Done Partial";
                            objOdbcDataReader.Close();
                        }
                        else
                        {
                            objOdbcDataReader.Read();
                            lsinvoicestatus = "IV Completed";
                            lspaymentflag = "Payment Done";
                            objOdbcDataReader.Close();
                        }
                   


                        msSQL = " Update acp_trn_tinvoice " +
                                   " Set invoice_status = '" + lsinvoicestatus + "'," +
                                   " payment_flag = '" + lspaymentflag + "'" +
                                   " where invoice_gid = '" + data.invoice_gid + "'";
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

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public Dictionary<string, object> DaGetPaymentrpt (string payment_gid, MdlPblTrnPaymentRpt values, string branch_gid)
        {

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();

            msSQL = " select a.payment_gid,date_format(a.payment_date ,'%d-%m-%y') as payment_date,a.vendor_gid,a.payment_remarks,a.payment_total,a.payment_status,a.user_gid, " +
                    " date_format(a.created_date,'%d-%m-%y') as " +
                    " created_date,a.payment_reference,a.purchaseorder_gid,a.advance_total,a.payment_mode,a.bank_name,a.branch_name, " +
                    " concat(a.cheque_no,a.dd_no)as cheque_no,a.city_name,a.currency_code,a.exchange_rate,a.tds_amount,a.tdscalculated_finalamount," +
                    " a.priority,a.priority_remarks,a.approved_by,a.approved_date,a.reject_reason,a.bank_gid,a.payment_from, " +
                    " a.addon_amount,a.additional_discount,a.additional_gid,a.discount_gid,b.*,c.*,CASE WHEN f.address2 IS NOT NULL THEN CONCAT(f.address1, ' ', f.address2) ELSE f.address1 END AS address1,f.city,f.state,f.postal_code  " +
                    " from acp_trn_tpayment a " +
                    " left join acp_trn_tpaymentdtl c on a.payment_gid=c.payment_gid " +
                    " left join acp_mst_tvendor b on b.vendor_gid=a.vendor_gid "+
                    " left join adm_mst_taddress f on b.address_gid=f.address_gid  " +
                    " where a.payment_gid='"  + payment_gid + "' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");


            //currency number2words = fnconvertnumbertowords(payment_gid, "PO_REPORT");

            msSQL = " select a.branch_name, a.address1 as branch_address, a.city, a.state, a.postal_code," +
                    " a.branch_logo, a.contact_number, a.email, a.tin_number, a.cst_number,b.currency_code,c.currency_symbol," +
                    " a.authorise_sign from hrm_mst_tbranch a  left join acp_trn_tinvoice x on a.branch_gid=x.branch_gid" +
                    " left join acp_trn_tinvoice2payment y on x.invoice_gid=y.invoice_gid" +
                    " left join acp_trn_tpayment b on y.payment_gid=b.payment_gid" +
                    " left join crm_trn_tcurrencyexchange c on b.currency_code = c.currency_code " +
                    " where b.payment_gid='" + payment_gid + "'";


            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");

            msSQL = "select  (branch_logo_path) as company_logo_Path  from hrm_mst_tbranch where branch_gid = '" + branch_gid + "' and  branch_logo_path is not null";

             dt1 = objdbconn.GetDataTable(msSQL);
            DataTable3.Columns.Add("company_logo_Path", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo_Path"].ToString().Replace("../../", ""));

                    if (System.IO.File.Exists(company_logo_path) == true)
                    {
                        //Convert  Image Path to Byte
                        branch_logo = System.Drawing.Image.FromFile(company_logo_path);
                        byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                        DataRow newRow = DataTable3.NewRow();
                        newRow["company_logo_Path"] = bytes;
                        DataTable3.Rows.Add(bytes);
                    }
                }
            }
            dt1.Dispose();
            DataTable3.TableName = "DataTable3";
            myDS.Tables.Add(DataTable3);

            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_pmr"].ToString(), "pbl_crp_paymentadvicereport.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Payment Receipt" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();

            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;

        }



        public void DaGetSingleaddPaymentSummary(string invoice_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select distinct a.invoice_gid,a.invoice_refno,a.advance_amount, replace( a.invoice_status,'IV','Invoice') as invoice_status, a.invoice_amount as invoice_amount, a.vendor_gid, a.invoice_remarks,a.invoice_from," +
                        " date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, a.payment_amount + a.advance_amount+a.debit_note as payed_amount, b.vendor_companyname,  " +
                        " (a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)) as outstanding ,date_format(a.payment_date,'%d-%m-%Y') as due_date from acp_trn_tinvoice a "+
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid where a.invoice_gid in('" + invoice_gid + "') order by a.invoice_date desc, a.invoice_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<singleinvoicelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new singleinvoicelist
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            advance_amount = dt["advance_amount"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            payed_amount = dt["payed_amount"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            outstanding = dt["outstanding"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            tds_amount = 0.00.ToString(),
                            exchangeloss = 0.00.ToString(),
                            exchangegain = 0.00.ToString(),
                            bankcharges = 0.00.ToString(),

                        });
                        values.singleinvoicelist = getModuleList;
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



        public void DaPostsinglepayment(multipleinvoice2singlepayment values, string user_gid)
        {
            try
            {
                string invoice_date;
                foreach (var data in values.GetMultipleInvoiceSummary)
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

                    double lsbankcharges;

                    if (data.tds_amount.Contains(","))
                    {
                        lsbankcharges = Convert.ToDouble(data.bankcharges.Replace(",", ""));
                    }
                    else
                    {
                        lsbankcharges = Convert.ToDouble(data.bankcharges);
                    }
                    double lsexchangeloss;

                    if (data.tds_amount.Contains(","))
                    {
                        lsexchangeloss = Convert.ToDouble(data.exchangeloss.Replace(",", ""));
                    }
                    else
                    {
                        lsexchangeloss = Convert.ToDouble(data.exchangeloss);
                    }

                    double lsexchangegain;

                    if (data.tds_amount.Contains(","))
                    {
                        lsexchangegain = Convert.ToDouble(data.exchangegain.Replace(",", ""));
                    }
                    else
                    {
                        lsexchangegain = Convert.ToDouble(data.exchangegain);
                    }

                    double outstanding_amount = data.outstanding;
                    invoice_date = data.invoice_date;
                    double total = data.advance + lspayment_amount + lstdsamount + data.balancepo_advance + lsbankcharges + lsexchangeloss + lsexchangegain;
                    //if (data.invoice_amount < total)
                    //{
                    //    values.message = "Payment amount cannot be more than invoice amount ";
                    //}
                    //else if (lspayment_amount > outstanding_amount)
                    //{
                    //    values.message = "Payment amount cannot be more than outstanding amount";
                    //}
                    //else if (lspayment_amount == 0)
                    if (lspayment_amount == 0)
                    {
                        values.message = "Payment amount must be numeric";
                    }
                    else
                    {
                        string msGetGID = objcmnfunctions.GetMasterGID("SPYC");
                        string msPYGetGID = objcmnfunctions.GetMasterGID("SPYC");
                        double lsInvoice_Amount = data.invoice_amount;
                        string lsInvoice_remarks = data.remark;
                        double lsAdvance_amount = data.advance;
                        double lspayment_amount_currency = lspayment_amount;
                        double lsinvoice_amount_currency = data.invoice_amount;
                        double lsadvance_amount_currency = data.advance;
                        msSQL = " insert into acp_trn_tpaymentdtl (" +
                               " paymentdtl_gid, " +
                               " payment_gid, " +
                               " payment_amount, " +
                               " invoice_amount, " +
                               " advance_amount, " +
                               " bankcharges, " +
                               " exchangeloss, " +
                               " exchangegain, " +
                               " invoice_remarks," +
                               " payment_amount_L," +
                               " invoice_Amount_L," +
                               " advance_amount_L" + " )values ( " +
                               "'" + msGetGID + "', " +
                               "'" + msPYGetGID + "', " +
                               "'" + lspayment_amount + "', " +
                               "'" + lsInvoice_Amount + "', " +
                               "'" + lsAdvance_amount + "', " +
                               "'" + lsbankcharges + "', " +
                               "'" + lsexchangeloss + "', " +
                               "'" + lsexchangegain + "', " +
                               "'" + lsInvoice_remarks + "'," +
                               "'" + lspayment_amount_currency + "'," +
                               "'" + lsinvoice_amount_currency + "'," +
                               "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        string mspGetGID = objcmnfunctions.GetMasterGID("SIPC");

                        msSQL = " insert into acp_trn_tinvoice2payment (" +
                                " invoice2payment_gid, " +
                                " payment_gid, " +
                                " paymentdtl_gid, " +
                                " invoice_gid, " +
                                " invoice_amount, " +
                                " advance_amount, " +
                                " payment_amount," +
                                " payment_amount_L," +
                                " invoice_Amount_L," +
                                " bankcharges, " +
                               " exchangeloss, " +
                               " exchangegain, " +
                                " advance_amount_L" + " )values ( " +
                                "'" + mspGetGID + "'," +
                                "'" + msPYGetGID + "'," +
                                "'" + msGetGID + "'," +
                                "'" + data.invoice_gid + "'," +
                                "'" + lsInvoice_Amount + "'," +
                                "'" + lsAdvance_amount + "'," +
                                "'" + lspayment_amount + "'," +
                                "'" + lspayment_amount_currency + "'," +
                                "'" + lsinvoice_amount_currency + "'," +
                                  "'" + lsbankcharges + "', " +
                               "'" + lsexchangeloss + "', " +
                               "'" + lsexchangegain + "', " +
                                "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        double lspay = lstdsamount + lspayment_amount;
                        msSQL = "select payment_amount  from acp_trn_tinvoice  where invoice_gid = '" + data.invoice_gid + "'";
                        string isamount = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select exchange_rate  from acp_trn_tinvoice  where invoice_gid = '" + data.invoice_gid + "'";
                        string iexchange = objdbconn.GetExecuteScalar(msSQL);
                        double exchange = double.Parse(iexchange);
                        if (isamount == "") { isamount = "0"; }
                        if (isamount != null)
                        {
                            double isamount1 = double.Parse(isamount);
                            double overall = isamount1 * exchange;
                            lspay = lspay + overall;

                        }
                        lspay = lspay / exchange;
                        msSQL = " Update acp_trn_tinvoice " +
                            " Set payment_amount = '" + lspay + "'," +
                            " advance_amount = '" + lsAdvance_amount + "'";
                        if (lspayment_amount == outstanding_amount)
                        {
                            msSQL += " ,invoice_status = 'IV Completed'";
                        }
                        else
                        {
                            msSQL += " ,invoice_status = 'IV Work In Progress'";
                        }

                        msSQL += " where invoice_gid = '" + data.invoice_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        foreach (var data1 in values.Getmultipleinvoice2employeedtl)
                        {
                            {
                                msSQL = " insert into acp_trn_tpayment (" +
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
                                     "'invoice completed', " +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                     "'" + total + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }

                        double adjustment_amount = 0.00;

                        if (lsexchangegain == 0.00 && lsexchangeloss == 0.00)
                        {
                            exchange1 = 0.00;
                            exchange_type = "";
                        }
                        else if (lsexchangeloss != 0.00)
                        {
                            exchange1 = lsexchangeloss;
                            exchange_type = "Exchange Loss";
                        }
                        else if (lsexchangegain != 0.00)
                        {
                            exchange1 = lsexchangegain;
                            exchange_type = "Exchange Gain";
                        }

                        objfincmn.finance_payment("Purchase", values.payment_mode, values.bankname, DateTime.Now.ToString("yyyy-MM-dd"), lspayment_amount, values.bankname, "Payment", "PMR", values.vendor_gid, values.payment_remarks, msPYGetGID, lstdsamount, adjustment_amount, total, msPYGetGID);
                        objfincmn.jn_exchange_purchase(msPYGetGID, values.payment_remarks, exchange1, "Payment", "PMR", exchange_type);
                        objfincmn.jn_bankcharge(msPYGetGID, values.payment_remarks, lsbankcharges, values.bankname, "Payment", "PMR");
                        msSQL = " Update acp_trn_tinvoice " +
                                  " Set advance_amount = '" + lsAdvance_amount + "'," +
                                  " payment_amount = '" + lspay + "'" +
                                  " where invoice_gid = '" + data.invoice_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " select invoice_amount, payment_amount, advance_amount from acp_trn_tinvoice  where invoice_gid =  '" + data.invoice_gid + "' and " +
                            " invoice_amount > (payment_amount + advance_amount)";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader.HasRows)
                        {
                            objOdbcDataReader.Read();
                            lsinvoicestatus = "IV Work In Progress";
                            lspaymentflag = "Payment Done Partial";
                            objOdbcDataReader.Close();
                        }
                        else
                        {
                            lsinvoicestatus = "IV Completed";
                            lspaymentflag = "Payment Done";
                        }



                        msSQL = " Update acp_trn_tinvoice " +
                                   " Set invoice_status = '" + lsinvoicestatus + "'," +
                                   " payment_flag = '" + lspaymentflag + "'" +
                                   " where invoice_gid = '" + data.invoice_gid + "'";
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

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }




    }

}