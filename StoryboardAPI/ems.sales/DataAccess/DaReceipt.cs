using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
//using OfficeOpenXml.Style;
using System.Web.UI.WebControls;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.IO;
using System.Net;
using System.Drawing;
using System.Web.DynamicData;
using Image = System.Drawing.Image;
using System.Web.Services.Description;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;


namespace ems.sales.Controllers
{
    public class DaReceipt
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objcmnfinance =  new finance_cmnfunction();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string exchange_type = string.Empty;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsIVt_PY_Flag, lsIVtt_PY_Flag, so_gid,lsinvoice_status, lsPayStatus,lscustomergid, lsentity_code, lsdesignation_code, lsCode, msGetGid, payment_status, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsproduct_code;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string full_path, path, branch_logo_path, authorized_sign_path;
        DataTable DataTable2 = new DataTable();
        double exchange;
        System.Drawing.Image branch_logo, authorized_sign;

        public void DaGetReceiptSummary(MdlReceipt values)
        {
            try
            {

                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select a.payment_gid,b.invoice_refno,b.customer_gid, b.invoice_gid, a.directorder_gid, format((a.amount_L - a.bank_charge+a.exchange_gain-a.exchange_loss+a.adjust_amount_L ),2) as amount, a.payment_mode,a.currency_code, " +
                    " DATE_FORMAT(a.payment_date, '%d-%m-%Y') as payment_date ,a.approval_status,format(a.total_amount,2)as total_amount, " +
                    " case when a.currency_code = '" + currency +
                    "' then b.customer_name " +
                    " when a.currency_code is null then b.customer_name " +
                    " when a.currency_code is not null and a.currency_code <> '" + currency +
                    "' then concat(b.customer_name,' / ',a.currency_code) end as customer_name, " +
                    " concat(b.customer_contactperson,' / ',b.customer_contactnumber,' / ',b.customer_email) as contact, " +
                    " b.customer_gid,a.payment_type " +
                    " from rbl_trn_tpayment a" +
                    " left join rbl_trn_tinvoice b on b.invoice_gid=a.invoice_gid " +
                    " left join crm_mst_tcustomer c on c.customer_gid=b.customer_gid " +
                    " where 1 = 1 and a.payment_return='0' " +
                    " order by a.payment_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<receiptsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new receiptsummary_list
                        {
                            payment_date = dt["payment_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            payment_mode = dt["payment_mode"].ToString(),
                            approval_status = dt["approval_status"].ToString(),
                            payment_gid = dt["payment_gid"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            payment_type = dt["payment_type"].ToString(),


                        });
                        values.receiptsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Receipt!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetAddReceiptSummary(MdlReceipt values)
        {
            try
            {

                

                msSQL = "call smr_trn_addreceiptsummary()";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<receiptaddsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new receiptaddsummary_list
                        {
                            invoice_refno = dt["invoice_references"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact_info"].ToString(),
                            invoice_amount = dt["total_invoice_amount"].ToString(),
                            payment_amount = dt["total_payment_amount"].ToString(),
                            outstanding = dt["total_outstanding"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            invoice_count = dt["invoice_count"].ToString(),

                        });
                        values.receiptaddsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while loading Receipt!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetmodeofpayment(MdlReceipt values)
        {
            try
            {

                msSQL = " Select modeofpayment_gid, payment_type from pay_mst_tmodeofpayment  " +
                   " order by payment_type asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getmodeofpaymentlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getmodeofpaymentlist
                        {
                            modeofpayment_gid = dt["modeofpayment_gid"].ToString(),
                            payment_type = dt["payment_type"].ToString(),
                        });
                        values.Getmodeofpaymentlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding mode of payment!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetMakeReceiptdata(MdlReceipt values, string customer_gid)
        {
            try
            {

                msSQL = " select a.customer_gid,a.invoice_gid,format(a.invoice_amount,2)as invoice_amount,a.currency_code,c.customer_name,c.customer_address,a.customer_contactnumber,a.customer_email,g.exchange_rate,a.invoice_amount_l,a.invoice_status,a.invoice_refno as invoice_id," +
                " c.customer_name,h.branch_name,format(a.payment_amount,2) as payment_amount,e.directorder_gid, a.invoice_reference as serviceorder_gid,a.currency_code,format(d.grandtotal, 2) as total_amount,  " +
                " case when format(d.salesorder_advance,2) is null then '0.0'" +
                " when format(d.salesorder_advance,2) is not null then format(d.salesorder_advance,2) end as advance_adjust,format(d.salesorder_advance, 2) as advance_amount, " +
                " case when format(d.updated_advancewht,2) is null then '0.0'" +
                " when format(d.updated_advancewht,2) is not null then format(d.updated_advancewht,2) end as updated_advancewht," +
                " format((a.invoice_amount-ifnull((a.payment_amount+updated_advance+updated_advancewht),0.00)),2) as os_amount from rbl_trn_tinvoice a" +
                " left join crm_mst_tcustomer c on c.customer_gid=a.customer_gid " +
                " left join hrm_mst_tbranch h on h.branch_gid = a.branch_gid " +
                " left join smr_trn_tsalesorder d on d.salesorder_gid=a.invoice_reference " +
                " left join smr_trn_torderadvance f on d.salesorder_gid=f.order_gid " +
                " left join smr_trn_tdeliveryorder e on e.directorder_gid=a.invoice_reference " +
                " left join crm_trn_tcurrencyexchange g on a.currency_code=g.currency_code" +
                " where a.customer_gid='" + customer_gid +
                " 'and a.invoice_status='Payment Pending' and a.invoice_flag in('Invoice Approved')" +
                " group by a.invoice_gid order by a.invoice_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<makereceipt_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new makereceipt_list
                        {
                            customer_name = dt["customer_name"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            customer_contactnumber = dt["customer_contactnumber"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            invoice_id = dt["invoice_id"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            advance_amount = dt["advance_amount"].ToString(),
                            os_amount = dt["os_amount"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),


                        });

                        values.makereceipt_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Receipt data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }



        public void DaUpdatedMakeReceipt(string user_gid, MdlReceipt values)
        {
            try
            {
                foreach (var data in values.updatereceipt_list)
                {
                    if (data.total_amount != null && data.total_amount != "0.00")
                    {
                        if (double.Parse(data.total_amount) > double.Parse(data.os_amount))
                    {
                        values.status = false;
                        values.message="Payment amount must not be greater than invoice amount -"+"'" +data.invoice_id+"'";
                        return;

                    }
                   

                        string uiDateStr = values.receipt_date;
                        DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string receipt_date1 = uiDate.ToString("yyyy-MM-dd");
                        msSQL = "Select invoice_gid from rbl_trn_Tinvoice where invoice_refno='" + data.invoice_id + "'";
                        string lsinvoice_gid=objdbconn.GetExecuteScalar(msSQL);


                        msSQL = " select bank_gid from acc_mst_tbank where bank_name ='" + values.bank_name + "'";
                        string lsbank_gid = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select branch_gid from hrm_mst_tbranch where branch_name = '" + values.branch_name + "'";
                        string branch_gid = objdbconn.GetExecuteScalar(msSQL);
                        double amount_l = Math.Round(double.Parse(data.receive_amount.ToString()) * double.Parse(data.exchange_rate), 2);
                        double totalamount_l = Math.Round(double.Parse(data.total_amount.ToString()) * double.Parse(data.exchange_rate), 2);
                        double tdsreceivale_amount_l = Math.Round(double.Parse(data.tdsreceivale_amount.ToString()) * double.Parse(data.exchange_rate), 2);
                        double adjust_amount_l = Math.Round(double.Parse(data.adjust_amount.ToString()) * double.Parse(data.exchange_rate), 2);
                        msGetGid = objcmnfunctions.GetMasterGID("BPTP");
                        msSQL = " insert into rbl_trn_tpayment (" +
                            " payment_gid, " +
                            " payment_date," +
                            " invoice_gid," +
                            " amount," +
                            " total_amount," +
                            " tds_amount," +
                            " adjust_amount," +
                            " branch," +
                            " bank_name," +
                            " dbank_gid ," +
                            " cheque_date," +
                            " cheque_number," +
                            " created_by," +
                            " created_date," +
                            " cash_date," +
                            " neft_date," +
                            " neft_transcationid," +
                            " currency_code, " +
                            " exchange_rate," +
                            " bank_charge," +
                            " exchange_loss," +
                            " exchange_gain," +
                            " invoiceamount_basecurrency," +
                            " receivedamount_bank," +
                            " payment_mode," +
                            " payment_type," +
                            " amount_L," +
                            " total_amount_L," +
                            " payment_remarks," +
                            " adjust_amount_L," +
                            " tds_amount_L " + ") values (" +
                            "'" + msGetGid + "'," +
                            "'" + receipt_date1 + "'," +
                            "'" + lsinvoice_gid + "'," +
                            "'" + data.receive_amount.ToString().Replace(",", "") + "'," +
                            "'" + data.total_amount.ToString().Replace(",", "") + "', " +
                            "'" + data.tdsreceivale_amount.ToString().Replace(",", "") + "', " +
                            "'" + data.adjust_amount.ToString().Replace(",", "") + "', " +
                            "'" + branch_gid + "'," +
                            "'" + values.bank_name + "'," +
                            "'" + lsbank_gid + "'," +
                            "'" + values.cheque_date + "'," +
                            "'" + values.cheque_no + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'" + values.payment_date + "'," +
                            "'" + values.neft_date + "'," +
                             "'" + values.trancsaction_no + "'," +
                            "'" + data.currency_code + "'," +
                            "'" + data.exchange_rate + "'," +
                            "'" + data.bank_charges + "'," +
                            "'" + data.exchange_loss + "'," +
                            "'" + data.exchange_gain + "'," +
                            "'" + data.invoice_amount.ToString().Replace(",","") + "'," +
                            "'" + data.received_in_bank.ToString().Replace(",","") + "'," +
                            "'" + values.payment_mode + "'," +
                            "'" + values.payment_mode + "'," +
                            "'" + amount_l + "'," +
                            "'" + totalamount_l + "'," +
                              "'" + data.payment_remarks + "'," +
                            "'" + adjust_amount_l + "'," +
                            "'" + tdsreceivale_amount_l + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            msSQL = "update rbl_trn_tinvoice set payment_amount = payment_amount + '" + data.total_amount.ToString().Replace(",", "") + "' where invoice_gid='" + lsinvoice_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                string lscustomergid = "", lspayment_amount = "", lsinvoice_amount = "";
                                msSQL = "select invoice_amount,payment_amount,invoice_gid,customer_gid from rbl_trn_tinvoice where invoice_gid='" + lsinvoice_gid + "' ";
                                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                if (objMySqlDataReader.HasRows == true)
                                {
                                    lsinvoice_amount = objMySqlDataReader["invoice_amount"].ToString();
                                    lspayment_amount = objMySqlDataReader["payment_amount"].ToString();
                                    lscustomergid = objMySqlDataReader["customer_gid"].ToString();

                                }
                                objMySqlDataReader.Close();

                                msSQL = " select invoice_amount, payment_amount, advance_amount " +
                                    " from rbl_trn_tinvoice  where " +
                                   " invoice_gid = '" + lsinvoice_gid + "'and" +
                                   " trim(invoice_amount) > trim(payment_amount) ";
                                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                if (objMySqlDataReader.HasRows == true)
                                {
                                    lsinvoice_status = "IV Work In Progress";
                                    lsIVt_PY_Flag = "Partially Paid";
                                }
                                else
                                {
                                    lsinvoice_status = "IV Completed";
                                    lsIVt_PY_Flag = "Payment  Done";
                                }
                                objMySqlDataReader.Close();

                                msSQL = " update smr_trn_tdeliveryorder " +
                                    " set invoice_status= '" + lsIVt_PY_Flag + "' where " +
                                    " directorder_gid = '" + values.directorder_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 0)
                                {
                                    values.status = false;
                                }

                                msSQL = " update rbl_trn_tpayment " +
                                    " set approval_status= '" + lsIVt_PY_Flag + "' where " +
                                    " payment_gid = '" + msGetGid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 0)
                                {
                                    values.status = false;
                                }
                                msSQL = "update rbl_trn_tinvoice set invoice_status='" + lsIVt_PY_Flag + "' where invoice_gid='" + lsinvoice_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                             

                                if (values.invoice_from == "Sales")
                                {
                                   
                                    if (values.payment_mode == "Advance Receipt")
                                    {
                                        msSQL = "update smr_trn_tsalesorder set updated_advance=updated_advance-'" + data.total_amount.ToString().Replace(",", "") + "'," +
                                            "updated_advancewht=updated_advancewht-'" + data.tdsreceivale_amount.ToString().Replace(",", "") + "', " +
                                            " received_amount=received_amount+'" + data.total_amount.ToString().Replace(",", "") + "' where salesorder_gid='" + values.salesorder_gid + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                    }
                                    else
                                    {
                                        msSQL = "update smr_trn_tsalesorder set " +
                                           " received_amount=received_amount+'" + data.total_amount.ToString().Replace(",", "") + "' where salesorder_gid='" + values.salesorder_gid + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }

                                }
                                else
                                {
                                    if (values.payment_mode == "Advance Receipt")
                                    {
                                        msSQL = "update smr_trn_tsalesorder set updated_advance=updated_advance-'" + data.total_amount.ToString().Replace(",", "") + "'+'" + data.tdsreceivale_amount.ToString().Replace(",", "") + "'," +
                                            "updated_advancewht=updated_advancewht-'" + data.tdsreceivale_amount.ToString().Replace(",", "") + "', " +
                                        " received_amount=received_amount+'" + data.total_amount.ToString().Replace(",", "") + "' where salesorder_gid='" + values.salesorder_gid + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                                    }
                                    else
                                    {
                                        msSQL = "update smr_trn_tsalesorder set " +
                                       " received_amount=received_amount+'" + data.total_amount.ToString().Replace(",", "") + "' where salesorder_gid='" + values.salesorder_gid + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }
                                }


                              

                                double invoiceAmount = Convert.ToDouble(lsinvoice_amount);

                                double paymentamount = Convert.ToDouble(lspayment_amount);
                                if (invoiceAmount <= paymentamount)
                                {
                                    msSQL = "update rbl_trn_tpayment set approval_status = 'Payment Done' where payment_gid = '" + msGetGid + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                                else
                                {
                                    msSQL = "update rbl_trn_tpayment set approval_status = 'Partially Paid' where payment_gid = '" + msGetGid + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                                

                                msSQL = " select a.invoice_amount,a.payment_amount from rbl_trn_tinvoice a " +
                                        " left join rbl_trn_tpayment b on a.invoice_gid=b.invoice_gid" +
                                        " where b.payment_gid='" + msGetGid + "' and trim(a.invoice_amount) > trim(a.payment_amount) ";
                                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                if (objMySqlDataReader.HasRows == true)
                                {
                                    lsIVtt_PY_Flag = "Partially Paid";
                                }
                                else
                                {
                                    lsIVtt_PY_Flag = "Payment Done";
                                }
                                objMySqlDataReader.Close();

                                msSQL = " update rbl_trn_tinvoice set " +
                                        " invoice_status = '" + lsIVtt_PY_Flag + "'" +
                                        " where invoice_gid = '" + lsinvoice_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "select finance_flag from adm_mst_Tcompany";
                                string lsfinance_flag=objdbconn.GetExecuteScalar(msSQL);    
                                if(lsfinance_flag == "Y")
                                   { 
                                double totalAmount = double.TryParse(data.basecurrencyinvoiceamt, out double totalpriceValue) ? totalpriceValue : 0;
                                double paid_amount = double.TryParse(data.received_in_bank, out double addonChargesValue) ? addonChargesValue : 0;
                                double tds_amount = double.TryParse(data.tdsreceivale_amount, out double freightChargesValue) ? freightChargesValue : 0;
                                double adjust_amount = double.TryParse(data.adjust_amount, out double packingChargesValue) ? packingChargesValue : 0;
                                double bank_charges = double.TryParse(data.bank_charges, out double bank_chargesValue) ? bank_chargesValue : 0;
                                double exchange_loss = double.TryParse(data.exchange_loss, out double roundoffValue) ? roundoffValue : 0;
                                double exchange_gain = double.TryParse(data.exchange_gain, out double discountAmountValue) ? discountAmountValue : 0;
                                //double exchange;

                                if (exchange_loss == 0.00 && exchange_gain == 0.00)
                                {

                                }
                                else if (exchange_loss != 0.00)
                                {
                                    exchange = exchange_loss;
                                    exchange_type = "Exchange Loss";
                                }
                                else if (exchange_gain != 0.00)
                                {
                                    exchange = exchange_gain;
                                    exchange_type = "Exchange Gain";
                                }

                                double total_amount = paid_amount - tds_amount + exchange_gain - exchange_loss;
                                paid_amount = paid_amount + adjust_amount + bank_charges;
                                    double total_amount_l = Math.Round((total_amount * double.Parse(data.exchange_rate)), 2);



                                if (values.payment_mode != "Advance Receipt")
                                {
                                    objcmnfinance.finance_payment("Sales", values.payment_mode, lsbank_gid, values.payment_date, totalamount_l, branch_gid,
                                                                          "Receipt", "RBL", lscustomergid,
                                                                          data.payment_remarks, msGetGid,
                                                                          tdsreceivale_amount_l, adjust_amount,
                                                                          amount_l, msGetGid);

                                    objcmnfinance.jn_exchange(msGetGid, data.payment_remarks, exchange, "Receipt", "RBL", exchange_type);

                                }
                                if (values.payment_mode == "Advance Receipt")
                                {
                                    objcmnfinance.finance_advancepayment("Advance Receipt", values.payment_mode, lsbank_gid,
                                                                           values.payment_date,
                                                                            totalamount_l, branch_gid,
                                                                           "Receipt",
                                                                           "RBL",
                                                                           lscustomergid,
                                                                           data.payment_remarks,
                                                                            msGetGid,
                                                                           tdsreceivale_amount_l,
                                                                           adjust_amount,
                                                                           paid_amount,
                                                                            msGetGid);
                                }
                                if (values.payment_mode == "Cheque" || values.payment_mode == "DD" || values.payment_mode == "NEFT"||values.payment_mode == "Credit Card")
                                {
                                    objcmnfinance.jn_bankcharge(msGetGid,data.payment_remarks, bank_charges, exchange_type, "Receipt", "RBL");
                                }
                                }
                              
                             
                            }
                        }
                    }
                }
                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Payment Done Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while updating Payment";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Receipt!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DadeleteReceiptSummary(string payment_gid, MdlReceipt values)
        {
            try
            {
                string lsinvoice_gid = "", approve = "", lsIVt_PY_Flag ="", lssalesorder_gid ="", lspayment_mode="", lspayment_type="", lsinvoice_status="";
                double paid_amount = 0, advance_amount=0;
                msSQL = "select payment_gid,approval_status,amount,tds_amount,exchange_rate,invoice_gid,directorder_gid,payment_mode,payment_type" +
                        " from rbl_trn_tpayment where payment_gid='" + payment_gid + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    approve = objMySqlDataReader["approval_status"].ToString();
                    paid_amount = double.Parse(objMySqlDataReader["amount"].ToString()) + double.Parse(objMySqlDataReader["tds_amount"].ToString());
                    lsinvoice_gid = objMySqlDataReader["invoice_gid"].ToString();
                    lssalesorder_gid = objMySqlDataReader["directorder_gid"].ToString();
                    lspayment_mode = objMySqlDataReader["payment_mode"].ToString();
                    advance_amount = double.Parse(objMySqlDataReader["amount"].ToString());
                    lspayment_type = objMySqlDataReader["payment_mode"].ToString();

                   
                }
                objMySqlDataReader.Close();
                msSQL = "update rbl_trn_tinvoice set payment_amount=payment_amount-'" + paid_amount + "' where invoice_gid='" + lsinvoice_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL); 

                if (mnResult == 1)
                {
                    msSQL = " select invoice_amount, payment_amount, advance_amount " +
                           " from rbl_trn_tinvoice  where " +
                           " invoice_gid = '" + lsinvoice_gid + "'and" +
                          " invoice_amount > payment_amount ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        if (objMySqlDataReader["payment_amount"].ToString() == "0.00")
                        {
                            lsinvoice_status = "Payment Pending";
                        }
                        else 
                        {
                            lsinvoice_status = "Partially Paid";
                        }
                        lsIVt_PY_Flag = "Partially Paid";
                    }
                    else
                    {

                        lsinvoice_status = "INV Completed";
                        lsIVt_PY_Flag = "Payment Approve Pending";
                        lsinvoice_status = "Payment Approve Pending";

                    }
                    objMySqlDataReader.Close();
                    if(lspayment_mode== "Advance Receipt")
                    {
                        msSQL = " update smr_trn_tsalesorder a " +
                               " set a.salesorder_status= '" + lsIVt_PY_Flag + "' ,  " +
                              " a.received_amount=a.received_amount-'" + paid_amount + "',  " +
                                " a.updated_advance=a.updated_advance-'" + advance_amount + "'  " +
                             " where " +
                           " a.salesorder_gid = '" + lssalesorder_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = " update smr_trn_tsalesorder a " +
                                " set a.salesorder_status= '" + lsIVt_PY_Flag + "' ,  " +
                              " a.received_amount=a.received_amount-'" + paid_amount + "'  " +
                                "where " +
                           " a.salesorder_gid = '" + lssalesorder_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                   
                }
                msSQL = "update rbl_trn_tinvoice set invoice_status='" + lsinvoice_status + "' " +
                       " where invoice_gid='" + lsinvoice_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "  delete from rbl_trn_tpayment where payment_gid='" + payment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                objcmnfinance.invoice_cancel(payment_gid);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Receipt Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Receipt";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Receipt!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetinvoicereceipt(string customer_gid, MdlReceipt values)
        {
            try
            {
                List<invoicereceipt_list> getModuleList = new List<invoicereceipt_list>();

               
                    msSQL = " select a.invoice_gid,a.invoice_refno,date_format(a.invoice_date,'%d-%m-%Y')as invoice_date,c.branch_name," +
                          " a.customer_name,a.customer_contactnumber,a.customer_email,a.invoice_from,a.customer_gid," +
                          " a.customer_address,a.customer_contactperson," +
                          " a.currency_code,a.exchange_rate from rbl_trn_tinvoice a " +
                          " left join hrm_mst_tbranch c on c.branch_gid=a.branch_gid " +
                          " where a.customer_gid='" + customer_gid + "'";
                    var dt_datatable = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new invoicereceipt_list
                            {
                                customer_name = dt["customer_name"].ToString(),
                                customer_address = dt["customer_address"].ToString(),
                                customer_contactnumber = dt["customer_contactnumber"].ToString(),
                                customer_email = dt["customer_email"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                                branch_name = dt["branch_name"].ToString(),
                            });
                        }
                    }
                    dt_datatable.Dispose();
                

                values.invoicereceipt_list = getModuleList;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Receipt details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
        "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetBankDetail(MdlReceipt values)
        {
            try
            {
                msSQL = " select bank_gid,bank_name from acc_mst_tbank  " +
                        " where 1=1 and default_flag='Y' group by bank_gid";



                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBankdtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBankdtldropdown
                        {

                            bank_name = dt["bank_name"].ToString(),
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
        public void DaGetCardDetail(MdlReceipt values)
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
        public void DaGetReceiptApprovalSummary(MdlReceipt values)
        {
            try
            {
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                  string currency = objdbconn.GetExecuteScalar(msSQL);
                
                msSQL = " select a.payment_gid,b.customer_gid, b.customer_name,a.directorder_gid, format(a.amount,2) as amount, a.payment_mode,a.invoice_gid,format((a.amount_L +a.adjust_amount_L  ),2) as payment_total,a.currency_code, " +
                " date_format(a.payment_date,'%d-%m-%Y') as payment_date ,case when a.approval_status='Partially Paid ' then 'Partial Payment Approve Pending' else a.approval_status end as approval_status,a.payment_remarks," +
                " case when a.currency_code = '" + currency + "' then b.customer_name " +
                " when a.currency_code is null then b.customer_name " +
                " when a.currency_code is not null and a.currency_code <> '" + currency + "' then concat(b.customer_name,' / ',h.country) end as customer_name, " +
                " d.customercontact_name,d.mobile, " +
                " b.customer_gid,a.payment_type,a.payment_remarks,a.dbank_gid,b.invoice_reference,b.customer_gid,a.tds_amount_L as tds_amount,a.adjust_amount_L as adjust_amount,a.serial_no,a.total_amount_L as total_amount " +
                " ,a.bank_charge,a.exchange_loss,a.exchange_gain from rbl_trn_tpayment a" +
                " left join rbl_trn_tinvoice b on b.invoice_gid=a.invoice_gid " +
                " left join crm_mst_tcustomer c on c.customer_gid=b.customer_gid " +
                " left join crm_mst_tcustomercontact d on d.customer_gid=b.customer_gid " +
                " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                " where a.approval_status<> 'Canceled' ";

                msSQL += " group by payment_gid order by a.payment_date desc, a.payment_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<receiptapprovallist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new receiptapprovallist
                        {
                            payment_date = dt["payment_date"].ToString(),
                            payment_gid = dt["payment_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customercontact_name = dt["customercontact_name"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            amount = dt["amount"].ToString(),
                            payment_type = dt["payment_type"].ToString(),
                            approval_status = dt["approval_status"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            tds_amount = dt["tds_amount"].ToString(),
                            adjust_amount = dt["adjust_amount"].ToString(),
                            bank_charge = dt["bank_charge"].ToString(),
                            exchange_loss = dt["exchange_loss"].ToString(),
                            exchange_gain = dt["exchange_gain"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            payment_remarks = dt["payment_remarks"].ToString(),
                            dbank_gid = dt["dbank_gid"].ToString(),
                            payment_mode = dt["payment_mode"].ToString()
                        });
                        values.receiptapprovallist = getModuleList;
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
        public void DaGetinvoicereceiptsummary(string customer_gid, MdlReceipt values)
        {
            try
            {

                msSQL = "select a.customer_gid,a.invoice_gid,format(a.invoice_amount,2)as invoice_amount, format((a.invoice_amount * a.exchange_rate),2) as basecurrencyinvoiceamt ,a.currency_code,a.exchange_rate,a.invoice_amount_l,a.invoice_status,a.invoice_refno as invoice_id," +
                        "c.customer_name,format(a.payment_amount,2) as payment_amount,e.directorder_gid, a.invoice_reference as serviceorder_gid,a.currency_code,format(d.grandtotal, 2) as total_amount," +
                        "case when format(d.salesorder_advance,2) is null then '0.0' " +
                        "when format(d.salesorder_advance,2) is not null then format(d.salesorder_advance,2) end as advance_adjust,format(d.salesorder_advance, 2) as advance_amount," +
                        "case when format(d.updated_advancewht,2) is null then '0.0' "+
                        "when format(d.updated_advancewht,2) is not null then format(d.updated_advancewht,2) end as updated_advancewht," +
                        " format((a.invoice_amount-ifnull((a.payment_amount+updated_advance+updated_advancewht),0.00)),2) as os_amount from rbl_trn_tinvoice a " +
                        " left join crm_mst_tcustomer c on c.customer_gid=a.customer_gid" +
                        " left join smr_trn_tsalesorder d on d.salesorder_gid=a.invoice_reference " +
                        " left join smr_trn_torderadvance f on d.salesorder_gid=f.order_gid" +
                        " left join smr_trn_tdeliveryorder e on e.directorder_gid=a.invoice_reference " +
                        " left join crm_trn_tcurrencyexchange g on a.currency_code=g.currency_code  " +
                        " where a.customer_gid = '" + customer_gid + "'and a.invoice_status <>'Payment Done' and a.invoice_flag in('Invoice Approved') and creditnote_status<>'Y'" +
                        " group by a.invoice_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoicereceiptsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new invoicereceiptsummary_list
                        {

                            invoice_id = dt["invoice_id"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            basecurrencyinvoiceamt = dt["basecurrencyinvoiceamt"].ToString(),
                            advance_amount = dt["advance_amount"].ToString(),
                            os_amount = dt["os_amount"].ToString(),
                            payment_remarks = "",
                            receive_amount = "0",
                            received_in_bank = "0",
                            payment_amount = "0",
                            tdsreceivale_amount = "0",
                            adjust_amount = "0",
                            bank_charges = "0",
                            exchange_loss = "0",
                            exchange_gain = "0",


                        });
                        values.invoicereceiptsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                msSQL = "select currency_code,exchange_rate from crm_trn_tcurrencyexchange where default_currency = 'Y'";
                values.defaultcurrency = objdbconn.GetExecuteScalar(msSQL);
                values.defaultexchangerate = objdbconn.GetExecuteScalar(msSQL);
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting add GRN Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                   $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                   ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                   msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                   DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetreceiptdetails(string payment_gid, MdlReceipt values)
        {
            try
            {

                msSQL = "select a.payment_gid,date_format(b.invoice_date , '%d-%m-%Y' )as invoice_date,b.invoice_refno, " +
                        " a.directorder_gid,a.invoice_gid,format(a.amount,2)as amount ,format(a.total_amount,2) as total_amount," +
                        "  format(b.invoice_amount,2)as invoice_amount ,c.qty_invoice,c.product_gid,d.product_name,format(a.tds_amount,2) as tds,a.payment_mode " +
                        "  from rbl_trn_tpayment a" +
                        " left join rbl_trn_tinvoice b on b.invoice_gid=a.invoice_gid" +
                        " left join rbl_trn_tinvoicedtl c on c.invoice_gid=a.invoice_gid" +
                        " left join pmr_mst_tproduct d on d.product_gid=c.product_gid" +
                        " left join smr_trn_tdeliveryorder f on f.directorder_gid=a.directorder_gid" +
                        " left join smr_trn_tsalesorder e on e.salesorder_gid=f.salesorder_gid" +
                        " where a.payment_gid='" + payment_gid + "' group by a.invoice_gid";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new invoice_list
                        {

                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            payment_amount = dt["amount"].ToString(),
                            tds = dt["tds"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            payment_mode = dt["payment_mode"].ToString(),


                        });
                        values.invoice_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Receipt details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public Dictionary<string, object> DaGetReceiptPDF(string payment_gid, string payment_type, MdlReceipt values)
        {

            var response = new Dictionary<string, object>();
            full_path = null;

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();


            msSQL = "select a.payment_date, a.payment_remarks, a.payment_mode, a.cheque_number, " +
                                "a.amount, a.cheque_date,a.serial_no, " +
                                "b.customer_name,c.invoice_refno as invoice_gid,c.currency_code from rbl_trn_tpayment a " +
                                "left join rbl_trn_tinvoice c on c.invoice_gid=a.invoice_gid " +
                                "left join crm_mst_tcustomer b on b.customer_gid=c.customer_gid " +
                                " where a.payment_gid='" + payment_gid + "' ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            if (payment_type == "Services")
            {
                msSQL = " select a.branch_logo_path,a.branch_name,a.address1,a.city,a.state,a.postal_code,a.gst_no," +
                                    " a.st_number,a.contact_number,a.email,c.invoice_reference,a.tin_number,a.cst_number,a.authorized_sign_path from " +
                                    " hrm_mst_tbranch a " +
                                    " left join smr_trn_tsalesorder b on b.branch_gid=a.branch_gid " +
                                    " left join rbl_trn_tinvoice c on c.invoice_reference=b.salesorder_gid " +
                                    " left join rbl_trn_tpayment d on d.invoice_gid=c.invoice_gid " +
                                    " where d.payment_gid='" + payment_gid + "'";
            }
            else if (payment_type == "Agreement")
            {
                msSQL = " select a.branch_logo_path ,a.branch_name,a.address1,a.city,a.state,a.postal_code,a.gst_no," +
                                    " a.st_number,a.contact_number,a.email,a.tin_number,a.cst_number,a.authorized_sign_path from " +
                                    " hrm_mst_tbranch a " +
                                    " left join crm_trn_tagreement b on b.branch_gid=a.branch_gid " +
                                    " left join rbl_trn_tinvoice c on c.invoice_reference=b.agreement_gid " +
                                    " left join rbl_trn_tpayment d on d.invoice_gid=c.invoice_gid " +
                                    " where d.payment_gid='" + payment_gid + "'";
            }
            else
            {
                msSQL = "select a.branch_logo_path ,a.branch_name,a.address1,a.city,a.state,a.postal_code,a.contact_number,a.gst_no," +
                                    " a.email,a.branch_gid, a.authorized_sign_path from hrm_mst_tbranch a " +
                                    " left join hrm_mst_temployee b on a.branch_gid=b.branch_gid " +
                                    " left join rbl_trn_tpayment c on c.created_by=b.user_gid " +
                                    " where c.payment_gid='" + payment_gid + "'";
            }
            dt_datatable = objdbconn.GetDataTable(msSQL);

            dt_datatable.Columns.Add("branch_logo", typeof(byte[]));
            dt_datatable.Columns.Add("authorized_sign", typeof(byte[]));
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow Dt in dt_datatable.Rows)
                {                   
                    branch_logo_path = HttpContext.Current.Server.MapPath("../../../" + Dt["branch_logo_path"].ToString().Replace("../../",""));
                    authorized_sign_path = HttpContext.Current.Server.MapPath("../../../" + Dt["authorized_sign_path"].ToString().Replace("../../", ""));
                    if (File.Exists(branch_logo_path) && File.Exists(authorized_sign_path))
                    {
                        Image branch_logo = Image.FromFile(branch_logo_path);
                        Image authorized_sign = Image.FromFile(authorized_sign_path);

                        Dt["branch_logo"] = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                        Dt["authorized_sign"] = (byte[])(new ImageConverter()).ConvertTo(authorized_sign, typeof(byte[]));
                    }
                    else
                    {
                        Dt["branch_logo"] = DBNull.Value;
                        Dt["authorized_sign"] = DBNull.Value;
                    }                    
                }
            }

            DataTable2 = dt_datatable;
            DataTable2.TableName = "DataTable2";
            myDS.Tables.Add(DataTable2);                     

            try
            {
                
                ReportDocument oRpt = new ReportDocument();
                string base_pathOF_currentFILE = AppDomain.CurrentDomain.BaseDirectory;
                string report_path = Path.Combine(base_pathOF_currentFILE, "ems.sales", "rbl_crp_paymentreceipt.rpt");

                if (!File.Exists(report_path))
                {
                    values.status = false;
                    values.message = "Your Rpt path not found !!";
                    response = new Dictionary<string, object>
                    {
                        {"status",false },
                        {"message",values.message}
                    };

                }
                oRpt.Load(report_path);
                oRpt.SetDataSource(myDS);                
                              

                path = Path.Combine(ConfigurationManager.AppSettings["report_path"]?.ToString());

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                msSQL = "select payment_gid from rbl_trn_Tpayment where payment_gid='" + payment_gid + "'";
                string PDFfile_name =objdbconn.GetExecuteScalar(msSQL);
                full_path = Path.Combine(path, PDFfile_name+".pdf");

                oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, full_path);
                myConnection.Close();
                response = objFnazurestorage.reportStreamDownload(full_path);
                values.status = true;

            }
            catch (Exception Ex)
            {
                values.status = false;
                values.message = Ex.Message;
                response = new Dictionary<string, object>
                {
                     { "status", false },
                     { "message", Ex.Message }
                };
            }
            finally
            {
                if (full_path != null)
                {
                    try
                    {
                        File.Delete(full_path);
                    }
                    catch (Exception Ex)
                    {
                        values.status = false;
                        values.message = Ex.Message;
                        response = new Dictionary<string, object>
                        {
                             { "status", false },
                             { "message", Ex.Message }
                        };
                    }
                }
            }
            return response;
        }

        public void DaPostReceiptApprove(receiptapprove_list values)
        {
            try
            {

                msSQL = " update rbl_trn_tpayment set " +
                        " approval_status = 'Payment Done'" +
                        " where payment_gid = '" + values.payment_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select a.invoice_amount,a.payment_amount from rbl_trn_tinvoice a " +
                        " left join rbl_trn_tpayment b on a.invoice_gid=b.invoice_gid" +
                        " where b.payment_gid='" + values.payment_gid + "' and trim(a.invoice_amount) > trim(a.payment_amount) ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true) 
                {
                    lsIVtt_PY_Flag = "Partially Paid";
                }
                else
                {
                    lsIVtt_PY_Flag = "Payment Done";
                }

                msSQL = " update rbl_trn_tinvoice set " +
                        " invoice_status = '" + lsIVtt_PY_Flag + "'" +
                        " where invoice_gid = '" + values.invoice_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                double totalAmount = double.TryParse(values.total_amount, out double totalpriceValue) ? totalpriceValue : 0;
                double paid_amount = double.TryParse(values.paid_amount, out double addonChargesValue) ? addonChargesValue : 0;
                double tds_amount = double.TryParse(values.tds_amount, out double freightChargesValue) ? freightChargesValue : 0;
                double adjust_amount = double.TryParse(values.adjust_amount, out double packingChargesValue) ? packingChargesValue : 0;
                double bank_charge = double.TryParse(values.bank_charge, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                double exchange_loss = double.TryParse(values.exchange_loss, out double roundoffValue) ? roundoffValue : 0;
                double exchange_gain = double.TryParse(values.exchange_gain, out double discountAmountValue) ? discountAmountValue : 0;
                //double exchange;

                if (exchange_loss == 0.00 && exchange_gain == 0.00)
                {
                    
                }
                else if (exchange_loss != 0.00)
                {
                    exchange = exchange_loss;
                    exchange_type = "Exchange Loss";
                }
                else if (exchange_gain != 0.00)
                {
                    exchange = exchange_gain;
                    exchange_type = "Exchange Gain";
                }

                double total_amount = paid_amount - tds_amount + exchange_gain - exchange_loss;
                paid_amount = paid_amount + adjust_amount + bank_charge;

                if(values.payment_mode != "Advance Receipt")
                {
                    objcmnfinance.finance_payment("Sales", values.payment_mode, values.dbank_gid, values.payment_date, total_amount, values.branch_gid,
                                                          "Receipt","RBL",values.customer_gid,
                                                         values.payment_remarks, values.payment_gid,
                                                          tds_amount,adjust_amount,
                                                          paid_amount, values.invoice_gid);

                    objcmnfinance.jn_exchange(values.payment_gid,values.payment_remarks, exchange, "Receipt", "RBL", exchange_type);

                }
                if (values.payment_mode == "Advance Receipt")
                {
                    objcmnfinance.finance_advancepayment("Advance Receipt", values.payment_mode, values.dbank_gid,
                                                           values.payment_date,
                                                            total_amount, values.branch_gid,
                                                           "Receipt",
                                                           "RBL",
                                                           values.customer_gid,
                                                           values.payment_remarks,
                                                            values.payment_gid,
                                                           tds_amount,
                                                           adjust_amount,
                                                           paid_amount,
                                                            values.invoice_gid);
                }
                if (values.payment_mode == "Cheque" || values.payment_mode == "DD" || values.payment_mode == "NEFT")
                {
                    objcmnfinance.jn_bankcharge(values.payment_gid,values.payment_remarks, bank_charge, exchange_type, "Receipt", "RBL");
                }
                if(mnResult == 1)
                {
                    values.status = true;
                    values.message = "Payment Completed";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting Receipt!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetViewReceipt(string payment_gid, MdlReceipt values)
        {
            try
            {
                msSQL = " select date_format(a.payment_date,'%d-%m-%Y') as payment_date,r.branch_name,e.customer_address," +
                " e.customer_gid,a.currency_code,c.customer_name,c.customer_id,e.customer_contactnumber as mobile, " +
                " e.customer_email as email,a.payment_remarks,a.exchange_rate from rbl_trn_tpayment a " +
                " left join rbl_trn_tinvoice e on a.invoice_gid = e.invoice_gid " +
                " left join crm_mst_tcustomer c on e.customer_gid = c.customer_gid " +
                " left join hrm_mst_tbranch r on r.branch_gid = e.branch_gid " +
                " where a.payment_gid = '" + payment_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var receipt_list = new List<ReceiptView_list>();
                if (dt_datatable.Rows.Count > 0) 
                {
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        receipt_list.Add(new ReceiptView_list
                        {
                            payment_date = row["payment_date"].ToString(),
                            customer_gid = row["customer_gid"].ToString(),
                            currency_code = row["currency_code"].ToString(),
                            customer_name = row["customer_name"].ToString(),
                            customer_id = row["customer_id"].ToString(),
                            mobile = row["mobile"].ToString(),
                            email = row["email"].ToString(),
                            payment_remarks = row["payment_remarks"].ToString(),
                            exchange_rate = row["exchange_rate"].ToString(),
                            branch_name = row["branch_name"].ToString(),
                            customer_address = row["customer_address"].ToString(),
                        });
                        values.ReceiptView_list = receipt_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch(Exception ex) 
            {
                values.message = "Exception occured while Receipt Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetViewReceiptSummary(string payment_gid, MdlReceipt values)
        {
            try
            {
                msSQL = " select format(a.amount,2) as amount,format(a.total_amount,2) as total_pay,format(a.adjust_amount,2) as adjust_amount, " +
                        " format(a.tds_amount,2) as tds_amount,format(ifnull(a.bank_charge,0.00),2) as bank_charge,format(ifnull(a.exchange_loss,0.00),2) as exchange_loss, " +
                        "  format(ifnull(a.exchange_gain,0.00),2) as exchange_gain,b.invoice_refno,b.invoice_gid,format(b.total_amount,2) as total_amount, " +
                        " format((b.invoice_amount-b.payment_amount),2) as outstanding_amount, " +
                        " c.salesorder_gid,format(c.grandtotal,2) as grand_total,a.payment_remarks, " +
                        " format(c.salesorder_advance,2) as advance_received,b.invoice_status,format(b.invoice_amount,2) as invoice_amount, " +
                        " format(a.invoiceamount_basecurrency,2) as invoiceamount_basecurrency ,format(a.receivedamount_bank,2) as receivedamount_bank ," +
                        " format(a.amount,2) as amount," +
                        " format((a.amount*a.exchange_rate),2) as paymentamont_basecur" +
                        " from rbl_trn_tpayment a " +
                        " left join rbl_trn_tinvoice b on a.invoice_gid = b.invoice_gid "+
                        " left join smr_trn_tsalesorder c on b.invoice_reference = c.reference_gid "+
                        " where payment_gid = '" + payment_gid + "' group by a.invoice_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var receipt_list = new List<receiptapprovallist>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        receipt_list.Add(new receiptapprovallist
                        {
                           amount = dt["amount"].ToString(),
                            total_pay = dt["total_pay"].ToString(),
                           total_amount = dt["total_amount"].ToString(),
                            adjust_amount = dt["adjust_amount"].ToString(),
                            tds_amount = dt["tds_amount"].ToString(),
                            bank_charge = dt["bank_charge"].ToString(),
                            exchange_loss = dt["exchange_loss"].ToString(),
                            exchange_gain = dt["exchange_gain"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString(),
                            grand_total = dt["grand_total"].ToString(),
                            payment_remarks = dt["payment_remarks"].ToString(),
                            advance_received = dt["advance_received"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            invoiceamount_basecurrency = dt["invoiceamount_basecurrency"].ToString(),
                            receivedamount_bank = dt["receivedamount_bank"].ToString(),
                            paymentamont_basecur = dt["paymentamont_basecur"].ToString(),
                        });
                        values.receiptapprovallist = receipt_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Receipt Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetViewReceiptBankSummary(string payment_gid, MdlReceipt values)
        {
            try
            {
                msSQL = " select b.bank_name,a.payment_mode,a.cheque_number,a.bank_name,a.branch," +
                    " date_format(a.cheque_date,'%d/%m/%Y') as cheque_date, a.dbank_gid," +
                    " date_format(a.cash_date,'%d/%m/%Y') as cash_date,a.neft_transcationid" +
                    " from rbl_trn_tpayment a " +
                    " left join acc_mst_tbank b on b.bank_gid = a.dbank_gid "+
                    " where a.payment_gid='"+payment_gid+ "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var receipt_list = new List<ReceiptView_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        receipt_list.Add(new ReceiptView_list
                        {
                            payment_mode = row["payment_mode"].ToString(),
                            cheque_number = row["cheque_number"].ToString(),
                            bank_name = row["bank_name"].ToString(),
                            branch = row["branch"].ToString(),
                            cheque_date = row["cheque_date"].ToString(),
                            dbank_gid = row["dbank_gid"].ToString(),
                            cash_date = row["cash_date"].ToString(),
                            neft_transcationid = row["neft_transcationid"].ToString()
                        });
                        values.ReceiptView_list = receipt_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Receipt Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}

