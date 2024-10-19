using ems.sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
//using static OfficeOpenXml.ExcelErrorValue;
using File = System.IO.File;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace ems.sales.DataAccess
{
    public class DaSmrRptInvoiceReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();

        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objMySqlDataReader, objOdbcDataReader, objODBCDataReader1, objODBCDataReader3, objODBCDataReader2;
        DataTable dt_datatable;
        string company_logo_path, authorized_sign_path,qr_path, lscustomer_gid, stock_gid, product_gid;
        Image branch_logo, DataColumn4,qrpath;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();
        string msEmployeeGID, msINGetGID, msSTCKGetGID, msstockdtlGid, mssalesorderGID, lscustomergid, lsemployee_gid, lsinvoice_gid, lstax_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid2, msGetGid, msGetGid3, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string path, branch_logo_path;
        DataTable DataTable1 = new DataTable();
        string msenquiryloggid, lsproductgid, lsproductuom_gid, lsproduct_name, lsproductuom_name,
            lscustomerproduct_code, mssalesorderGID1, msSalesGid;
        string lsCustomername, lscustomer_id, lscustomer_code, msAccGetGID, lsbranch, lssendername, lsdc,
            lssenderdesignation, lssender_contactnumber, lstax2 = "0.00", lstax1 = "0.00", lstax3 = "0.00";
        double lsbasic_amount, lsunitprice, lsproduct_price, lstotal, lsproduct_discount;
        finance_cmnfunction objfincmn = new finance_cmnfunction();
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private static Task<result> _runningTask;

        public async Task<result> DaGetShopifyInvoice(string employee_gid)
        {
            _queue.Enqueue(employee_gid); // Add the request to the queue
            await ProcessQueue(); // Process the queue
            return await _runningTask;
        }
        private async Task ProcessQueue()
        {
            while (_queue.TryDequeue(out string employee_gid))
            {
                await _semaphore.WaitAsync(); // Ensure that only one method runs at a time
                try
                {
                    if (_runningTask != null && !_runningTask.IsCompleted)
                    {
                        await _runningTask; // Wait for the previous task to complete
                    }

                    _runningTask = ExecuteGetShopifyInvoice(employee_gid); // Start a new task
                    await _runningTask;
                }
                finally
                {
                    _semaphore.Release(); // Release the semaphore
                }
            }
        }
        ///taxsegment
        public async Task<result> ExecuteGetShopifyInvoice(string employee_gid)
        {

            result objresult = new result();
            List<SalesOrderDetails> salesOrders = new List<SalesOrderDetails>();
            List<SalesOrderDetailsdtl> orderDetail = new List<SalesOrderDetailsdtl>();


            try
            {
                msSQL = "select shipsupply from adm_mst_Tcompany";
                string lsshopifyflag = objdbconn.GetExecuteScalar(msSQL);
                if (lsshopifyflag == "Y")
                {
                    msSQL = "call smr_trn_tshopifyordertoinvoice";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    foreach (DataRow dr in dt_datatable.Rows)
                    {

                        SalesOrderDetails order = new SalesOrderDetails
                        {
                            salesorder_gid = dr["salesorder_gid"].ToString(),
                            branch_gid = dr["branch_gid"].ToString(),
                            account_gid = dr["account_gid"].ToString(),
                            salesorder_date = Convert.ToDateTime(dr["salesorder_date"].ToString()),
                            customer_gid = dr["customer_gid"].ToString(),
                            customer_contact_person = dr["customer_contact_person"].ToString(),
                            customer_address = dr["customer_address"].ToString(),
                            customer_email = dr["customer_email"].ToString(),
                            so_referencenumber = dr["so_referencenumber"].ToString(),
                            so_remarks = dr["so_remarks"].ToString(),
                            total_amount = dr["total_amount"].ToString(),
                            payment_days = dr["payment_days"].ToString(),
                            delivery_days = dr["delivery_days"].ToString(),
                            salesorder_status = dr["salesorder_status"].ToString(),
                            created_by = dr["created_by"].ToString(),
                            created_date = dr["created_date"].ToString(),
                            updated_by = dr["updated_by"].ToString(),
                            updated_date = dr["updated_date"].ToString(),
                            termsandconditions = dr["termsandconditions"].ToString(),
                            so_referenceno1 = dr["so_referenceno1"].ToString(),
                            Grandtotal = double.Parse(dr["Grandtotal"].ToString()),
                            customer_name = dr["customer_name"].ToString(),
                            so_type = dr["so_type"].ToString(),
                            invoice_flag = dr["invoice_flag"].ToString(),
                            salesorder_remarks = dr["salesorder_remarks"].ToString(),
                            salesorder_flag = dr["salesorder_flag"].ToString(),
                            addon_charge = dr["addon_charge"].ToString(),
                            additional_discount = dr["additional_discount"].ToString(),
                            invoice_amount = dr["invoice_amount"].ToString(),
                            cst_number = dr["cst_number"].ToString(),
                            addon_charge_l = dr["addon_charge_l"].ToString(),
                            additional_discount_l = dr["additional_discount_l"].ToString(),
                            grandtotal_l = dr["grandtotal_l"].ToString(),
                            currency_code = dr["currency_code"].ToString(),
                            exchange_rate = dr["exchange_rate"].ToString(),
                            currency_gid = dr["currency_gid"].ToString(),

                            shipping_to = dr["shipping_to"].ToString(),
                            fulfillment_status = dr["fulfillment_status"].ToString(),
                            payment_terms = dr["payment_terms"].ToString(),
                            customerbranch_gid = dr["customerbranch_gid"].ToString(),

                            order_note = dr["order_note"].ToString(),
                            gst_amount = dr["gst_amount"].ToString(),
                            tax_name = dr["tax_name"].ToString(),
                            tax_gid = dr["tax_gid"].ToString(),
                            total_price = dr["total_price"].ToString(),
                            costcenter_gid = dr["costcenter_gid"].ToString(),
                            quotation_gid = dr["quotation_gid"].ToString(),
                            roundoff = dr["roundoff"].ToString(),
                            progressive_flag = dr["progressive_flag"].ToString(),
                            advanceamount_bank = dr["advanceamount_bank"].ToString(),
                            advancewhtbase_currency = dr["advancewhtbase_currency"].ToString(),
                            billing_to = dr["billing_to"].ToString(),
                            freight_charges = dr["freight_charges"].ToString(),
                            buyback_charges = dr["buyback_charges"].ToString(),
                            packing_charges = dr["packing_charges"].ToString(),
                            insurance_charges = dr["insurance_charges"].ToString(),
                            mawb_no = dr["mawb_no"].ToString(),
                            hawb_no = dr["hawb_no"].ToString(),
                            invoice_no = dr["invoice_no"].ToString(),
                            direct_shipment = dr["direct_shipment"].ToString(),
                            branch_name = dr["branch_name"].ToString(),
                            businessunit_gid = dr["businessunit_gid"].ToString(),
                            description = dr["description"].ToString(),
                            others = dr["others"].ToString(),
                            flight_date = dr["flight_date"].ToString(),
                            businessunit_name = dr["businessunit_name"].ToString(),
                            consignee_to = dr["consignee_to"].ToString(),
                            customerref_no = dr["customerref_no"].ToString(),
                            supplier_name = dr["supplier_name"].ToString(),
                            supplier_address = dr["supplier_address"].ToString(),
                            buyer_gid = dr["buyer_gid"].ToString(),
                            supplier_gid = dr["supplier_gid"].ToString(),
                            agent_gid = dr["agent_gid"].ToString(),
                            shipper_address = dr["shipper_address"].ToString(),
                            consignee_address = dr["consignee_address"].ToString(),
                            shopify_orderid = dr["shopify_orderid"].ToString(),
                            tax_name4 = dr["tax_name4"].ToString(),
                            shopifyorder_number = dr["shopifyorder_number"].ToString(),
                            shopifycustomer_id = dr["shopifycustomer_id"].ToString(),
                            customer_contactperson = dr["customer_contactperson"].ToString(),
                            mobile = dr["mobile"].ToString(),
                            tax_amount = dr["tax_amount"].ToString(),
                            customer_instruction = dr["customer_instruction"].ToString(),
                            billing_email = dr["billing_email"].ToString(),
                            mintsoftid = dr["mintsoftid"].ToString(),
                            order_instruction = dr["order_instruction"].ToString(),
                            message_id = dr["message_id"].ToString(),
                            source_flag = dr["source_flag"].ToString(),
                            shopify_ids = dr["shopify_customerid"].ToString(),

                        };


                        salesOrders.Add(order);
                    }



                    foreach (var order in salesOrders)
                    {
                        msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                        string lscurrency = objdbconn.GetExecuteScalar(msSQL);
                        msSQL1 = "select * from rbl_trn_tinvoice where shopify_orderid='" + order.shopify_orderid + "' ";
                        //dt_datatable = objdbconn.GetDataTable(msSQL1);
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL1);
                        if (objMySqlDataReader.HasRows)
                        {
                            objMySqlDataReader.Close();
                        }
                        
                        else
                        {
                            msSQL = " select customer_gid from crm_mst_tcustomer where shopify_id='" + order.shopify_ids + "' ";

                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader.HasRows)
                            {
                                lscustomer_gid = objMySqlDataReader["customer_gid"].ToString();


                            }
                            objMySqlDataReader.Close();
                            if (lscustomer_gid != null && lscustomer_gid != "")
                            {
                                lscustomergid = lscustomer_gid;
                            }
                            else
                            {
                                lscustomergid = null;
                            }
                            msSQL = "Select finance_flag from adm_mst_Tcompany";
                            string lsfinanceflag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsfinanceflag == "Y")
                            {
                                msSQL = "select customer_name, customer_code, customer_id from crm_mst_tcustomer where customer_gid='" + lscustomergid + "'";
                                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                if (objMySqlDataReader.HasRows == true)
                                {
                                   
                                    lsCustomername = objMySqlDataReader["customer_name"].ToString();
                                    lscustomer_code = objMySqlDataReader["customer_code"].ToString();
                                    lscustomer_id = objMySqlDataReader["customer_id"].ToString();

                                    objMySqlDataReader.Close();
                                }

                                msSQL = "SELECT account_gid FROM crm_mst_tcustomer WHERE customer_gid='" + lscustomergid + "'";
                                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader.HasRows)
                                {
                                    while (objMySqlDataReader.Read())
                                    {
                                        string lsaccount_gid = objMySqlDataReader["account_gid"]?.ToString(); // Safely get the value

                                        // Check if lsaccount_gid is null or empty
                                        if (string.IsNullOrEmpty(lsaccount_gid))
                                        {
                                            objfincmn.finance_vendor_debitor("Sales", lscustomer_id, lsCustomername, lscustomergid, employee_gid);
                                            string trace_comment = "Added a customer on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            objcmnfunctions.Tracelog(msGetGid, employee_gid, trace_comment, "added_customer");
                                        }
                                    }
                                }

                                objMySqlDataReader.Close();
                            }

                            msSQL = " select * from hrm_mst_temployee where employee_gid='" + employee_gid + "' ";
                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader.HasRows)
                            {
                                objMySqlDataReader.Read();
                                lssendername = objMySqlDataReader["employee_gid"].ToString();
                                lssenderdesignation = objMySqlDataReader["designation_gid"].ToString();
                                lssender_contactnumber = objMySqlDataReader["employee_mobileno"].ToString();
                                lsbranch = objMySqlDataReader["branch_gid"].ToString();
                                objMySqlDataReader.Close();
                            }

                            msINGetGID = objcmnfunctions.GetMasterGID_SP("SIVT");

                            msSTCKGetGID = objcmnfunctions.GetMasterGID_SP("ISKP");

                            lsdc = "DC" + objcmnfunctions.GetRandomString(5);

                            mssalesorderGID = objcmnfunctions.GetMasterGID_SP("VDOP");

                            string ls_referenceno = "SI" + order.shopifyorder_number;

                            msSQL = "select invoice_gid from rbl_trn_Tinvoice where invoice_refno= '" + ls_referenceno + "' ";
                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader.HasRows == true)
                            {
                                lsinvoice_gid = objMySqlDataReader["invoice_gid"].ToString();

                                objMySqlDataReader.Close();
                            }
                            else
                            {
                                lsinvoice_gid = null;
                            }
                            if (lsinvoice_gid == null || lsinvoice_gid == "")
                            {
                                double addonCharges = double.TryParse(order.addon_charge, out double addonChargesValue) ? addonChargesValue : 0;
                                double freightCharges = double.TryParse(order.freight_charges, out double freightChargesValue) ? freightChargesValue : 0;
                                double packingCharges = double.TryParse(order.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                                double insuranceCharges = double.TryParse(order.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                                double roundoff = double.TryParse(order.roundoff, out double roundoffValue) ? roundoffValue : 0;
                                double additionaldiscountAmount = double.TryParse(order.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                                double buybackCharges = double.TryParse(order.buyback_charges, out double buybackChargesValue) ? buybackChargesValue : 0;
                                double addonCharges_l = Math.Round(addonCharges * double.Parse(order.exchange_rate), 2);
                                double freightCharges_l = Math.Round(freightCharges * double.Parse(order.exchange_rate), 2);
                                double packingCharges_l = Math.Round(packingCharges * double.Parse(order.exchange_rate), 2);
                                double insuranceCharges_l = Math.Round(insuranceCharges * double.Parse(order.exchange_rate), 2);
                                double roundoff_l = Math.Round(roundoff * Convert.ToDouble(order.exchange_rate),2);
                                double additionaldiscountAmount_l = Math.Round(additionaldiscountAmount * Convert.ToDouble(order.exchange_rate),2);
                                double buybackCharges_l = Math.Round(buybackCharges * Convert.ToDouble(order.exchange_rate),2);


                                msSQL = " insert into rbl_trn_tinvoice(" +
                             " invoice_gid," +
                             " invoice_date," +
                             " shopify_orderid," +
                             " payment_term, " +
                             " payment_date," +
                             " invoice_from," +
                             " customer_gid," +
                             " customer_name," +
                             " customer_contactperson," +
                             " customer_contactnumber," +
                             " customer_address," +
                             " customer_email," +
                             " total_amount," +
                             " invoice_amount," +
                             " invoice_refno," +
                             " invoice_status," +
                             " invoice_flag," +
                             " user_gid," +
                             " discount_amount," +
                             " additionalcharges_amount," +
                             " total_amount_L," +
                             " invoice_amount_L," +
                             " discount_amount_L," +
                             " additionalcharges_amount_L," +
                             " invoice_remarks," +
                             " currency_code," +
                             " exchange_rate," +
                             " branch_gid, " +
                             " roundoff," +
                              " tax_gid," +
                             " tax_name," +
                             " tax_amount," +
                             " created_date," +
                             " freight_charges," +
                             " packing_charges," +
                             " delivery_date," +
                             " payment_days," +
                             " invoice_reference, " +
                             " bill_email, " +
                             " po_number, " +
                             " shipping_to " +
                             " ) values (" +
                             " '" + msINGetGID + "'," +
                             "'" + order.salesorder_date.ToString("yyyy-MM-dd") + "'," +
                             "'" + order.shopify_orderid + "'," +
                             "'" + order.payment_days + "'," +
                             "'" + order.salesorder_date.ToString("yyyy-MM-dd") + "'," +
                             "'Shopify Invoice'," +
                             " '" + order.customer_gid + "'," +
                             " '" + order.customer_name.Replace("'", "\\\'") + "'," +
                             " '" + order.customer_contactperson + "'," +
                             " '" + order.customer_mobile + "'," +
                             " '" + order.billing_to.Replace("'", "\\\'") + "'," +
                             " '" + order.customer_email + "'," +
                             " '" + order.total_price.Replace(",", "").Trim() + "'," +
                             " '" + order.Grandtotal + "'," +
                             " '" + ls_referenceno + "'," +
                             " 'fulfilled'," +
                             " 'Invoice Raised'," +
                             " '" + employee_gid + "'," +
                             " '" + order.addon_charge + "'," +
                             " '" + order.addon_charge + "'," +
                             "'" + order.total_price.Replace(",", "").Trim() + "'," +
                             "'" + order.Grandtotal + "'," +
                             "'" + order.additional_discount + "'," +
                             "'" + order.addon_charge + "'," +
                             "'" + order.shopifyorder_number + "'," +
                             "'" + order.currency_code + "'," +
                             "'" + order.exchange_rate + "'," +
                             "'" + order.branch_name + "', " +
                             "'" + order.roundoff + "'," +
                             "'" + order.tax_name + "'," +
                             "'" + order.tax_name + "', " +
                             "'" + order.tax_amount + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                             "'" + order.freight_charges + "'," +
                             "'" + order.packing_charges + "'," +
                             "'" + order.delivery_days + "'," +
                             "'" + order.payment_days + "'," +
                             "'" + order.salesorder_gid + "'," +
                             "'" + order.customer_email + "'," +
                              "'" + order.shopifyorder_number + "'," +
                             "'" + order.shipping_to.Replace("'", "\\\'") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                                if (mnResult == 1)
                                {
                                    msSQL = "select finance_flag from adm_mst_tcompany ";
                                    string finance_flag = objdbconn.GetExecuteScalar(msSQL);
                                    if (finance_flag == "Y")
                                    {

                                        string lsproduct_price_l = "", lstax1_gid = "", lstax2_gid = "";
                                        msSQL = " select sum(product_price_L) as product_price_L,sum(tax_amount1_L) as tax1,sum(tax_amount2_L) as tax2,tax1_gid,tax2_gid from rbl_trn_tinvoicedtl " +
                                             " where invoice_gid='" + msINGetGID + "' ";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objOdbcDataReader.HasRows == true)
                                        {
                                            lsproduct_price_l = objOdbcDataReader["product_price_L"].ToString();
                                            lstax1 = objOdbcDataReader["tax1"].ToString();
                                            lstax2 = objOdbcDataReader["tax2"].ToString();
                                            lstax1_gid = objOdbcDataReader["tax1_gid"].ToString();
                                            lstax2_gid = objOdbcDataReader["tax2_gid"].ToString();
                                        }
                                        objOdbcDataReader.Close();
                                        lsbasic_amount = Math.Round(double.Parse(lsproduct_price_l), 2);
                                        objfincmn.jn_invoice(order.salesorder_date.ToString("yyyy-MM-dd"), "Shopify Invoice", order.branch_name, ls_referenceno, msINGetGID
                                         , lsbasic_amount, addonCharges, additionaldiscountAmount, order.Grandtotal, order.customer_gid, "Invoice", "RBL",
                                         "Services", roundoff_l, freightCharges_l, buybackCharges_l, packingCharges_l, insuranceCharges_l, order.tax_amount, order.tax_name);






                                        if (lstax1 != "0.00" && lstax1 != "")
                                        {
                                            decimal lstaxsum = decimal.Parse(lstax1);
                                            string lstaxamount = lstaxsum.ToString("F2");
                                            double tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                                            objfincmn.jn_sales_tax(msINGetGID, ls_referenceno, "Shopify Invoice", tax_amount, lstax1_gid);
                                        }
                                        if (lstax2 != "0.00" && lstax2 != "" && lstax2 != "0" && lstax2 != null)
                                        {
                                            decimal lstaxsum = decimal.Parse(lstax2);
                                            string lstaxamount = lstaxsum.ToString("F2");
                                            double tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                                            objfincmn.jn_sales_tax(msINGetGID, ls_referenceno, "Shopify Invoice", tax_amount, lstax2_gid);
                                        }




                                    }
                                }

                                msSQL = " insert into smr_trn_tdeliveryorder (" +
                                       " directorder_gid, " +
                                        " directorder_date," +
                                        " directorder_refno, " +
                                        " salesorder_gid, " +
                                        " invoice_gid, " +
                                        " customer_gid, " +
                                        " customer_name , " +
                                        " customerbranch_gid, " +
                                        " customer_branchname, " +
                                        " customer_contactperson, " +
                                        " customer_contactnumber, " +
                                        " customer_address, " +
                                        " directorder_status, " +
                                        " terms_condition, " +
                                        " created_date, " +
                                        " created_name, " +
                                        " sender_name," +
                                        " delivered_by," +
                                        " dc_no," +
                                        " mode_of_despatch, " +
                                        " tracker_id, " +
                                        " sender_designation," +
                                        " sender_contactnumber, " +
                                        " grandtotal_amount, " +
                                        " delivered_date," +
                                        " shipping_to, " +
                                        " no_of_boxs, " +
                                        " dc_note, " +
                                        " customer_emailid " +
                                        ") values (" +
                                        "'" + mssalesorderGID + "'," +
                                        "'" + order.salesorder_date.ToString("yyyy-MM-dd") + "'," +
                                        "'" + mssalesorderGID + "'," +
                                        "'" + order.salesorder_gid + "'," +
                                        "'" + msINGetGID + "'," +
                                        "'" + order.customer_gid + "'," +
                                        "'" + order.customer_name.Replace("'", "\\\'") + "'," +
                                        "'" + order.branch_name + "'," +
                                        "'" + order.customer_name.Replace("'", "\\\'") + "'," +
                                        "'" + order.customer_contactperson + "'," +
                                        "'" + order.customer_mobile + "'," +
                                        " '" + order.customer_address.Replace("'", "\\\'") + "'," +
                                        "'Despatch Done',";
                                if (order.termsandconditions != null)
                                {
                                    msSQL += "'" + order.termsandconditions.Replace("'", "\\\'") + "', ";

                                }
                                else
                                {
                                    msSQL += "'" + order.termsandconditions + "', ";
                                }
                                msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                        "'" + employee_gid + "'," +
                                        "'" + lssendername + "'," +
                                        "'" + employee_gid + "'," +
                                        "'" + lsdc + "'," +
                                        "''," +
                                        "''," +
                                        "'" + lssenderdesignation + "'," +
                                        "'" + lssender_contactnumber + "',";
                                if (order.Grandtotal == null || order.Grandtotal == 0)
                                {
                                    msSQL += "'0.00',";
                                }
                                else
                                {
                                    msSQL += "'" + order.Grandtotal + "',";
                                }
                                msSQL += "'" + order.salesorder_date.ToString("yyyy-MM-dd") + "'," +
                                "'" + order.customer_address.Replace("'", "\\\'") + "'," +
                                "''," +
                                "'" + order.so_remarks.Replace("'", "\\\'") + "'," +
                                "'" + order.customer_email + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    string msSQLDetails = " SELECT *  FROM  smr_trn_tsalesorderdtl WHERE salesorder_gid = '" + order.salesorder_gid + "'";
                                    DataTable dtDetails = objdbconn.GetDataTable(msSQLDetails);
                                    foreach (DataRow drDetail in dtDetails.Rows)
                                    {
                                        double lsexchangerate = Math.Round(double.Parse(order.exchange_rate),2);
                                        double lsvendorprice= Math.Round(double.Parse(drDetail["product_price"].ToString().Replace(",", "")),2);
                                        lsproduct_discount = Math.Round(double.Parse(drDetail["discount_amount"].ToString().Replace(",", "")), 2);

                                        double lsproduct_price = Math.Round((double.Parse(drDetail["qty_quoted"].ToString().Replace(",", "")) * lsvendorprice) - lsproduct_discount, 2);
                                        double lstaxamount_l = Math.Round((double.Parse(drDetail["tax_amount"].ToString().Replace(",", "")) * lsexchangerate), 2);
                                        double lsdiscountamount_l = Math.Round((lsproduct_discount * lsexchangerate),2);
                                        double lsproducttotal_l = Math.Round((double.Parse(drDetail["price"].ToString().Replace(",", "")) * lsexchangerate), 2);

                                        //insert query here  for invoice dtl table 
                                        msGetGid = objcmnfunctions.GetMasterGID_SP("SIVC");
                                        msSQL = " insert into rbl_trn_tinvoicedtl (" +
                                                " invoicedtl_gid, " +
                                                " invoice_gid, " +
                                                " product_gid, " +
                                                " product_code, " +
                                                " productgroup_gid, " +
                                                " productgroup_name, " +
                                                " product_name, " +
                                                " uom_gid, " +
                                                " productuom_name, " +
                                                " product_price, " +
                                                " discount_percentage, " +
                                                " discount_amount, " +
                                                " tax_name, " +
                                                " tax1_gid," +
                                                " tax_percentage, " +
                                                " tax_amount, " +
                                                " qty_invoice, " +
                                                " product_remarks, " +
                                                " product_total, " +
                                                " product_price_L, " +
                                                " discount_amount_L, " +
                                                " tax_amount1_L, " +
                                                " product_total_L, " +
                                                " display_field, " +
                                                " customerproduct_code," +
                                                " vendor_price," +
                                                " created_by," +
                                                " created_date " +
                                                " ) values ( " +
                                                "'" + msGetGid + "'," +
                                                "'" + msINGetGID + "'," +
                                                "'" + drDetail["product_gid"].ToString() + "'," +
                                                "'" + drDetail["product_code"].ToString() + "'," +
                                                "'" + drDetail["productgroup_gid"].ToString() + "'," +
                                                "'" + drDetail["productgroup_name"].ToString() + "'," +
                                                "'" + drDetail["product_name"].ToString() + "'," +
                                                "'" + drDetail["uom_gid"].ToString() + "'," +
                                                "'" + drDetail["uom_name"].ToString() + "'," +
                                                "'" + lsproduct_price + "'," +
                                                "'" + drDetail["discount_percentage"].ToString() + "'," +
                                                "'" + drDetail["discount_amount"].ToString().Replace(",", "") + "'," +
                                                "'" + drDetail["tax_name"].ToString() + "'," +
                                                "'" + drDetail["tax1_gid"].ToString() + "'," +
                                                "'" + drDetail["tax_percentage"].ToString() + "'," +
                                                "'" + drDetail["tax_amount"].ToString().Replace(",", "") + "'," +
                                                "'" + drDetail["qty_quoted"].ToString().Replace(",", "") + "'," +
                                                "'" + drDetail["display_field"].ToString() + "'," +
                                                "'" + drDetail["price"].ToString().Replace(",", "") + "'," +
                                                "'" + drDetail["product_price"].ToString().Replace(",", "") + "'," +
                                                "'" + lsdiscountamount_l + "'," +
                                                "'" + lstaxamount_l + "'," +
                                                "'" + lsproducttotal_l + "'," +
                                                "'" + drDetail["product_name"].ToString() + "'," +
                                                "'" + drDetail["product_code"].ToString() + "'," +
                                                "'" + lsvendorprice + "'," +
                                                "'" + employee_gid + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {

                                            msGetGid = objcmnfunctions.GetMasterGID_SP("VDDC");
                                            msSQL = "INSERT INTO smr_trn_tdeliveryorderdtl (" +
                                                    "directorderdtl_gid, " +
                                                    "directorder_gid, " +
                                                    "productgroup_gid, " +
                                                    "productgroup_name, " +
                                                    "product_gid, " +
                                                    "product_name, " +
                                                    "product_code, " +
                                                    "product_uom_gid, " +
                                                    "productuom_name, " +
                                                    "product_qty, " + // Keep product_qty unchanged                                    
                                                    "product_price, " +
                                                    "product_total, " +
                                                    "salesorderdtl_gid, " +
                                                    "product_qtydelivered" +
                                                    ") VALUES ( " +
                                                    "'" + msGetGid + "', " +
                                                    "'" + mssalesorderGID + "', " +
                                                    "'" + drDetail["productgroup_gid"].ToString() + "', " +
                                                    "'" + drDetail["productgroup_name"].ToString() + "', " +
                                                    "'" + drDetail["product_gid"].ToString() + "', " +
                                                    "'" + drDetail["product_name"].ToString() + "', " +
                                                    "'" + drDetail["product_code"].ToString() + "', " +
                                                    "'" + drDetail["uom_gid"].ToString() + "', " +
                                                    "'" + drDetail["uom_name"].ToString() + "', " +
                                                    "'" + drDetail["qty_quoted"].ToString().Replace(",", "") + "', ";

                                            if (drDetail["price"] == null || DBNull.Value.Equals(drDetail["price"]))
                                            {
                                                msSQL += "null,";
                                            }
                                            else
                                            {
                                                msSQL += "'" + drDetail["price"] + "',";
                                            }

                                            if (drDetail["price"] == null || DBNull.Value.Equals(drDetail["price"]))
                                            {
                                                msSQL += "null,";
                                            }
                                            else
                                            {
                                                msSQL += "'" + drDetail["price"] + "',";
                                            }

                                            msSQL += "'" + order.salesorder_gid + "'," +
                                                     "'" + drDetail["qty_quoted"].ToString().Replace(",", "") + "')"; // Use the calculated delivery quantity
                                            // Execute the insert query
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }

                                        msSQL = " select product_gid, stock_gid from ims_trn_tstock where product_gid='" + drDetail["product_gid"] + "'";
                                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objMySqlDataReader.HasRows == true)
                                        {
                                            product_gid = objMySqlDataReader["product_gid"].ToString();
                                            stock_gid = objMySqlDataReader["product_gid"].ToString();

                                            if (product_gid != "")
                                            {
                                                msSQL = " update ims_trn_tstock set stock_qty= stock_qty - '" + drDetail["qty_quoted"] + "'";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                                msSQL = " select * from ims_tmp_tstock where created_by='" + employee_gid + "'";
                                                dt_datatable = objdbconn.GetDataTable(msSQL);

                                                if (dt_datatable.Rows.Count != 0)
                                                {

                                                    foreach (DataRow dt in dt_datatable.Rows)
                                                    {


                                                        msstockdtlGid = objcmnfunctions.GetMasterGID_SP("ISTP");

                                                        msSQL = "insert into ims_trn_tstockdtl(" +
                                                                   "stockdtl_gid," +
                                                                   "stock_gid," +
                                                                   "branch_gid," +
                                                                   "product_gid," +
                                                                   "uom_gid," +
                                                                   "issued_qty," +
                                                                   "amend_qty," +
                                                                   "damaged_qty," +
                                                                   "adjusted_qty," +
                                                                   "transfer_qty," +
                                                                   "return_qty," +
                                                                   "reference_gid," +
                                                                   "stock_type," +
                                                                   "remarks," +
                                                                   "created_by," +
                                                                   "created_date," +
                                                                   "display_field" +
                                                                   ") values ( " +
                                                                    "'" + msstockdtlGid + "'," +
                                                                   "'" + dt["stock_gid"].ToString() + "'," +
                                                                   "'" + lsbranch + "'," +
                                                                   "'" + dt["product_gid"].ToString() + "'," +
                                                                   "'" + dt["productuom_gid"].ToString() + "',";
                                                        if (dt["stock_quantity"].ToString() == null || dt["stock_quantity"].ToString() == "")
                                                        {
                                                            msSQL += "'0.00',";
                                                        }
                                                        else
                                                        {
                                                            msSQL += "'" + dt["stock_quantity"].ToString() + "',";

                                                        }
                                                        msSQL += "'0.00'," +
                                                                   "'0.00'," +
                                                                   "'0.00'," +
                                                                   "'0.00'," +
                                                                   "'0.00'," +
                                                                   "'" + mssalesorderGID + "'," +
                                                                   "'Delivery'," +
                                                                   "''," +
                                                                   "'" + employee_gid + "'," +
                                                                   "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                                   "'')";

                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                        msSQL = " update ims_trn_tstock set " +
                                                                " stock_qty = stock_qty - '" + dt["stock_quantity"].ToString() + "' " +
                                                                " where stock_gid='" + dt["stock_gid"].ToString() + "'";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                        msSQL = " select distinct  " +
                                                               " sum(qty_quoted) as qty_quoted,sum(product_delivered) as product_delivered " +
                                                               " from smr_trn_tsalesorderdtl where salesorder_gid='" + drDetail["salesorder_gid"] + "' group by salesorder_gid " +
                                                               " having(qty_quoted <> product_delivered) ";
                                                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                                                        if (objMySqlDataReader.HasRows == true)
                                                        {
                                                            msSQL = " update smr_trn_tsalesorder " +
                                                                    " set salesorder_status= 'Partially Delivered' where " +
                                                                    " salesorder_gid = '" + drDetail["salesorder_gid"] + "'";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        }
                                                        else
                                                        {
                                                            msSQL = " update smr_trn_tsalesorder " +
                                                                    " set salesorder_status= 'Dispatched' where " +
                                                                    " salesorder_gid = '" + drDetail["salesorder_gid"] + "'";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                            msSQL = " update smr_trn_tdeliveryorder " +
                                                                    " set delivery_status ='Delivered' where " +
                                                                    " salesorder_gid='" + drDetail["salesorder_gid"] + "'";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                       
                                        else
                                        {
                                            msstockdtlGid = objcmnfunctions.GetMasterGID_SP("ISTP");

                                            msSQL = "insert into ims_trn_tstockdtl(" +
                                                    "stockdtl_gid," +
                                                    "stock_gid," +
                                                    "branch_gid," +
                                                    "product_gid," +
                                                    "uom_gid," +
                                                    "issued_qty," +
                                                    "amend_qty," +
                                                    "damaged_qty," +
                                                    "adjusted_qty," +
                                                    "transfer_qty," +
                                                    "return_qty," +
                                                    "reference_gid," +
                                                    "stock_type," +
                                                    "remarks," +
                                                    "created_by," +
                                                    "created_date," +
                                                    "display_field" +
                                                    ") values ( " +
                                                    "'" + msstockdtlGid + "'," +
                                                    "'" + msSTCKGetGID + "'," +
                                                    "'" + lsbranch + "'," +
                                                    "'" + drDetail["product_gid"].ToString() + "'," +
                                                    "'" + drDetail["uom_gid"].ToString() + "',";
                                            if (drDetail["qty_quoted"].ToString() == null || drDetail["qty_quoted"].ToString() == "")
                                            {
                                                msSQL += "'0.00',";
                                            }
                                            else
                                            {
                                                msSQL += "'" + drDetail["qty_quoted"].ToString() + "',";
                                            }
                                            msSQL += "'0.00'," +
                                                       "'0.00'," +
                                                       "'0.00'," +
                                                       "'0.00'," +
                                                       "'0.00'," +
                                                       "'" + mssalesorderGID + "'," +
                                                       "'Delivery'," +
                                                       "''," +
                                                       "'" + employee_gid + "'," +
                                                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                       "'')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                            msSQL = " update ims_trn_tstock set " +
                                                    " stock_qty = stock_qty - '" + drDetail["qty_quoted"].ToString() + "' " +
                                                    " where stock_gid='" + msSTCKGetGID + "'";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                            msSQL = " select distinct  " +
                                                   " sum(qty_quoted) as qty_quoted,sum(product_delivered) as product_delivered " +
                                                   " from smr_trn_tsalesorderdtl where salesorder_gid='" + drDetail["salesorder_gid"] + "' group by salesorder_gid " +
                                                   " having(qty_quoted <> product_delivered) ";
                                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                            if (objMySqlDataReader.HasRows == true)
                                            {
                                                msSQL = " update smr_trn_tsalesorder " +
                                                        " set salesorder_status= 'Delivery Done Partial' where " +
                                                        " salesorder_gid = '" + drDetail["salesorder_gid"] + "'";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                            }
                                            else
                                            {
                                                msSQL = " update smr_trn_tsalesorder " +
                                                        " set salesorder_status= 'Delivery Completed' where " +
                                                        " salesorder_gid = '" + drDetail["salesorder_gid"] + "'";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                msSQL = " update smr_trn_tdeliveryorder " +
                                                        " set delivery_status ='Delivery Completed' where " +
                                                        " salesorder_gid='" + drDetail["salesorder_gid"] + "'";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }

                                        }
                                        objMySqlDataReader.Close();
                                    }
                                }
                            }
                        }
                    }
                }
                return objresult;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                                ex.Message.ToString() + "***********" + ex.Message.ToString() + "*****Query****" +
                                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }
        public void DaGetProductsearchSummary(string producttype_gid, string product_name, string customer_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {
                StringBuilder sqlQuery = new StringBuilder("SELECT a.product_name, a.product_code, a.product_gid, " +
                    " a.cost_price, b.productuom_gid, b.productuom_name, c.productgroup_name, c.productgroup_gid, " +
                    " a.productuom_gid, d.producttype_gid FROM pmr_mst_tproduct a " +
                    " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid " +
                    " LEFT JOIN pmr_mst_tproducttype d ON d.producttype_gid = a.producttype_gid WHERE 1=1");

                if (!string.IsNullOrEmpty(producttype_gid) && producttype_gid != "undefined" && producttype_gid != "null")
                {
                    sqlQuery.Append(" AND a.producttype_gid = '").Append(producttype_gid).Append("'");
                }

                if (!string.IsNullOrEmpty(product_name) && product_name != "null")
                {
                    sqlQuery.Append(" AND a.product_name LIKE '%").Append(product_name).Append("%'");
                }

                dt_datatable = objdbconn.GetDataTable(sqlQuery.ToString());
                var getModuleList = new List<GetProductsearchlist>();
                var allTaxSegmentsList = new List<GetTaxSegmentListDetails>(); // Create list to store all tax segments

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        var product = new GetProductsearchlist
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["cost_price"].ToString(),
                            quantity = 0,
                            total_amount = 0,
                            discount_persentage = 0,
                            discount_amount = 0,
                        };
                        getModuleList.Add(product);

                        if (!string.IsNullOrEmpty(customer_gid) && customer_gid != "undefined")
                        {
                            string productGid = product.product_gid;
                            string productName = product.product_name;

                            StringBuilder taxSegmentQuery = new StringBuilder("SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                                " e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                                " THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                                " a.cost_price FROM acp_mst_ttaxsegment2product d " +
                                " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                                " LEFT JOIN crm_mst_tcustomer f ON f.taxsegment_gid = e.taxsegment_gid " +
                                " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE a.product_gid = '");
                            taxSegmentQuery.Append(productGid).Append("' AND f.customer_gid = '").Append(customer_gid).Append("'");

                            dt_datatable = objdbconn.GetDataTable(taxSegmentQuery.ToString());

                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt1 in dt_datatable.Rows)
                                {
                                    allTaxSegmentsList.Add(new GetTaxSegmentListDetails
                                    {
                                        product_name = productName,
                                        product_gid = productGid,
                                        taxsegment_gid = dt1["taxsegment_gid"].ToString(),
                                        taxsegment_name = dt1["taxsegment_name"].ToString(),
                                        tax_name = dt1["tax_name"].ToString(),
                                        tax_percentage = dt1["tax_percentage"].ToString(),
                                        tax_gid = dt1["tax_gid"].ToString(),
                                        mrp_price = dt1["mrp_price"].ToString(),
                                        cost_price = dt1["cost_price"].ToString(),
                                        tax_amount = dt1["tax_amount"].ToString(),
                                    });
                                }
                            }
                        }
                    }
                    values.GetProductsearchlist = getModuleList; // Assign GetProductsearchlist to values.GetProductsearchlist
                }
                values.GetTaxSegmentListDetails = allTaxSegmentsList; // Assign allTaxSegmentsList to values.GetTaxSegmentList
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }







        public void DaGetProductNamedropDown(MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = " select product_name, product_gid, product_code from pmr_mst_tproduct ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getproductnamedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproductnamedropdown
                        {
                            productgid = dt["product_gid"].ToString(),
                            productname = dt["product_name"].ToString(),
                            productcode = dt["product_code"].ToString(),

                        });
                        values.Getproductnamedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetcurrencyCodedropdown(MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = " select currency_code, currencyexchange_gid " +
                    " from crm_trn_tcurrencyexchange ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getcurrencycodedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcurrencycodedropdown
                        {
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                        });
                        values.Getcurrencycodedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Currency code!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetCustomerNamedropDown(MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = " select customer_name, customer_gid from crm_mst_tcustomer ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomernamedropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomernamedropdown
                        {
                            customergid = dt["customer_gid"].ToString(),
                            customername = dt["customer_name"].ToString(),
                        });
                        values.GetCustomernamedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customer name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetBranchName(MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = " select branch_name, branch_gid from hrm_mst_tbranch";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetBranchnamedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchnamedropdown
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.GetBranchnamedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Branch name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGettaxnamedropdown(MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = " select tax_gid,tax_name,percentage from acp_mst_ttax where reference_type='Customer' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettaxnamedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Gettaxnamedropdown
                        {
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_percentage = dt["percentage"].ToString(),
                        });
                        values.Gettaxnamedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetTermsandConditions(MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = " select template_gid, template_name, template_content from adm_mst_ttemplate ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTermsandconditionsDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTermsandconditionsDropdown
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_content = dt["template_content"].ToString()
                        });
                        values.terms_lists = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading terms and conditions!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DainvoiceProductSummary(string employee_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {

                double grand_total = 0.00;

                msSQL = "select invoicedtl_gid, invoice_gid, product_gid, qty_invoice, format(product_price,2) as product_price," +
                        " concat(discount_percentage,'%','  -  ' ,format(discount_amount,2)) as discount, " +
                        " concat(hsn_code, ' / ',hsn_description) as hsn, format(product_total,2) as product_total, uom_gid," +
                        " uom_name, tax_name, format(tax_amount, 2) as tax_amount," +
                        " display_field, tax1_gid, tax2_gid, product_name, productgroup_gid, " +
                        " productgroup_name,  employee_gid,  selling_price,  product_code," +
                        " customerproduct_code,  tax_percentage, tax_percentage2, sl_no, " +
                        " format(vendor_price,2) as vendor_price from rbl_tmp_tinvoicedtl where employee_gid='" + employee_gid + "' order by invoicedtl_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoiceproductsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["product_total"].ToString());
                        getModuleList.Add(new invoiceproductsummary_list
                        {
                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            discount = dt["discount"].ToString(),
                            selling_price = dt["selling_price"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            hsn = dt["hsn"].ToString(),
                        });
                        values.invoiceproductsummarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.grand_total = Math.Round(grand_total, 2);
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Invoice Product Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnChangeCurrency(string currencyexchange_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
                    " where currencyexchange_gid='" + currencyexchange_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOnChangeCurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnChangeCurrency
                        {
                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.GetOnChangeCurrency = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Change Currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnChangeTerms(string template_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {

                if (template_gid != null)
                {
                    msSQL = " select template_gid, template_name, template_content from adm_mst_ttemplate where template_gid='" + template_gid + "' ";

                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetTermsandconditionsDropdown>();

                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetTermsandconditionsDropdown
                            {
                                template_gid = dt["template_gid"].ToString(),
                                template_name = dt["template_name"].ToString(),
                                template_content = dt["template_content"].ToString(),
                            });
                            values.terms_lists = getModuleList;
                        }
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading change terms!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnChangeCustomer(MdlSmrRptInvoiceReport values, string customer_gid)
        {
            try
            {
                msSQL = " SELECT B.customerbranch_name, B.customercontact_name, B.mobile, B.email, concat(B.address1,', ', B.address2,', ', B.city,' - ', B.zip_code,', ', B.state) AS customer_address, A.taxsegment_gid " +
                        " FROM crm_mst_tcustomer AS A LEFT JOIN crm_mst_tcustomercontact AS B ON A.customer_gid = B.customer_gid" +
                         " WHERE A.customer_gid = '" + customer_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getcustomeronchangedetails>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcustomeronchangedetails
                        {
                            customerbranchname = dt["customerbranch_name"].ToString(),
                            customercontactname = dt["customercontact_name"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            email = dt["email"].ToString(),
                            address = dt["customer_address"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                        });
                        values.Getcustomeronchangedetails = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Change Customer!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnChangeProduct(MdlSmrRptInvoiceReport values, string product_gid)
        {
            try
            {


                msSQL = " SELECT b.productgroup_name, a.product_code,a.cost_price, a.hsn_number, a.hsn_desc, c.productuom_name, a.product_price " +
                        " FROM pmr_mst_tproduct AS a LEFT JOIN pmr_mst_tproductgroup AS b ON a.productgroup_gid = b.productgroup_gid LEFT JOIN pmr_mst_tproductuom AS c ON a.productuom_gid = c.productuom_gid " +
                        " WHERE a.product_gid = '" + product_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getproductonchangedetails>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproductonchangedetails
                        {
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            hsn_code = dt["hsn_number"].ToString(),
                            hsn_description = dt["hsn_desc"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            unitprice = dt["cost_price"].ToString(),
                            product_price = dt["product_price"].ToString(),
                        });
                        values.Getproductonchangedetails = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Change Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaInvoicePostProduct(string employee_gid, invoiceproductlist values)
        {
            try
            {

                double lstotalamount = Math.Round((values.unitprice), 2) * (values.quantity) * (values.exchangerate);
                //double lstotalamount = Math.Round((values.unitprice), 2) * (values.quantity);
                double discountamount = Math.Round(lstotalamount * ((values.discountpercentage) / 100), 2);
                double lsGrandtotal = Math.Round(((values.unitprice) * (values.quantity)) - discountamount, 2);
                msGetGid = objcmnfunctions.GetMasterGID("SIVC");
                string lsrefno = "";
                //double lspercentage1 = 0;
                //double lspercentage2 = 0;
                string lspercentage1, lspercentage2;
                double tax1_amount = 0;
                double tax2_amount = 0;

                string lstax1, lstax2;
                msSQL = "select product_name from pmr_mst_tproduct where product_gid='" + values.productgid + "'";
                string lsproductName = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_gid + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_gid + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                if (values.taxname1 == "" || values.taxname1 == null)
                {
                    lspercentage1 = "0";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.taxname1 + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                    tax1_amount = Math.Round(lsGrandtotal * (Convert.ToDouble(lspercentage1) / 100), 2);
                }
                if (values.taxname2 == "" || values.taxname2 == null)
                {
                    lspercentage2 = "0";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.taxname2 + "'";
                    lspercentage2 = objdbconn.GetExecuteScalar(msSQL);
                    tax2_amount = Math.Round(lsGrandtotal * (Convert.ToDouble(lspercentage2) / 100), 2);
                }
                double Grandtotalamount = Math.Round((lsGrandtotal + tax1_amount + tax2_amount), 2);

                if (values.taxname1 == "" || values.taxname1 == null)
                {
                    lstax1 = "0";
                }
                else
                {
                    msSQL = "select tax_prefix from acp_mst_ttax  where tax_gid='" + values.taxname1 + "'";
                    lstax1 = objdbconn.GetExecuteScalar(msSQL);
                }

                if (values.taxname2 == "" || values.taxname2 == null)
                {
                    lstax2 = "0";
                }
                else
                {
                    msSQL = "select tax_prefix from acp_mst_ttax where tax_gid='" + values.taxname2 + "'";
                    lstax2 = objdbconn.GetExecuteScalar(msSQL);
                }
                {
                    double lsproductprice=Math.Round((values.unitprice-double.Parse(values.discountamount)),2);
                    msSQL = " insert into rbl_tmp_tinvoicedtl ( " +
                            " invoicedtl_gid," +
                            " invoice_gid," +
                            " product_gid," +
                            " qty_invoice," +
                            " product_price," +
                            " discount_percentage," +
                            " discount_amount," +
                            " tax_amount," +
                            " product_total," +
                            " uom_gid," +
                            " uom_name," +
                            " tax_amount2," +
                            " tax_name," +
                            " tax_name2," +
                            " display_field," +
                            " tax1_gid," +
                            " tax2_gid," +
                            " product_name," +
                            " productgroup_gid," +
                            " productgroup_name, " +
                            " employee_gid, " +
                            " product_code," +
                            " tax_percentage," +
                            " tax_percentage2," +
                            " vendor_price " +
                            " ) values ( " +
                            "'" + msGetGid + "'," +
                            "'" + lsrefno + "'," +
                            "'" + values.productgid + "'," +
                            "'" + values.quantity + "'," +
                            "'" + values.unitprice + "'," +
                            "'" + values.discountpercentage + "'," +
                            "'" + discountamount + "'," +
                            "'" + tax1_amount + "'," +
                            "'" + Grandtotalamount + "'," +
                            "'" + lsproductuomgid + "'," +
                            "'" + values.productuom_gid + "'," +
                            "'" + tax2_amount + "'," +
                            "'" + lstax1 + "'," +
                            "'" + lstax2 + "'," +
                            "'" + values.display_field + "'," +
                            "'" + values.taxname1 + "'," +
                            "'" + values.tax_gid2 + "'," +
                            "'" + lsproductName + "'," +
                            "'" + lsproductgroupgid + "'," +
                            "'" + values.productgroup_gid + "'," +
                            "'" + employee_gid + "'," +
                            "'" + values.productcode + "'," +
                            "'" + lspercentage1 + "'," +
                            "'" + lspercentage2 + "'," +
                            "'" + values.unitprice + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Invoice Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }



        public void DaGetEditInvoiceProductSummary(string employee_gid, MdlSmrRptInvoiceReport values, string invoice_gid)
        {
            try
            {

                double grand_total = 0.00;
                double grandtotal = 0.00;
                msSQL = "select invoicedtl_gid, invoice_gid,tax_name2,format(tax_amount2, 2) as tax_amount2, product_gid,product_remarks, qty_invoice, format(product_price, 2) as product_price, " +
                         " concat(discount_percentage, '%') as discount_percentage , format(discount_amount, 2) as discount, " +
                         " concat(hsn_code, ' / ', hsn_description) as hsn, format(product_total, 2) as product_total, uom_gid, " +
                        " uom_name, tax_name, format(tax_amount, 2) as tax_amount," +
                        " display_field, tax1_gid, tax2_gid, product_name, productgroup_gid, " +
                       "  productgroup_name,  employee_gid,  selling_price,  product_code," +
                        " customerproduct_code,  tax_percentage, tax_percentage2, sl_no, " +
                        " format(vendor_price, 2) as vendor_price from rbl_trn_tinvoicedtl where invoice_gid = '" + invoice_gid + "' union " +
                       "  select invoicedtl_gid, invoice_gid,tax_name2,format(tax_amount2, 2) as tax_amount2, product_gid,product_remarks, qty_invoice, format(product_price, 2) as product_price, " +
                        " concat(discount_percentage, '%') as discount_percentage ,  format(discount_amount, 2) as discount,  " +
                      "   concat(hsn_code, ' / ', hsn_description) as hsn, format(product_total, 2) as product_total, uom_gid, " +
                      "    uom_name, tax_name, format(tax_amount, 2) as tax_amount, " +
                       "   display_field, tax1_gid, tax2_gid, product_name, productgroup_gid, " +
                      "    productgroup_name,  employee_gid,  selling_price,  product_code, " +
                      "    customerproduct_code,  tax_percentage, tax_percentage2, sl_no, " +
                       "   format(vendor_price, 2) as vendor_price from rbl_tmp_tinvoicedtl where employee_gid = '" + employee_gid + "' order by invoicedtl_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<editinvoiceproductsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["product_total"].ToString());
                        grandtotal += double.Parse(dt["product_total"].ToString());
                        string productgroup = dt["productgroup_gid"].ToString();

                        msSQL = "select productgroup_name from pmr_mst_tproductgroup where productgroup_gid = '" + productgroup + "'";
                        string productgroup_gid = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new editinvoiceproductsummary_list
                        {
                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = productgroup_gid,
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            vendor_price = dt["vendor_price"].ToString(),
                            discount = dt["discount"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            selling_price = dt["selling_price"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            tax = dt["tax_name"].ToString(),
                            taxname2 = dt["tax_name2"].ToString(),
                            taxamount2 = dt["tax_amount2"].ToString(),
                            hsn = dt["hsn"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                        });
                        values.editinvoiceproductsummarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.grand_total = Math.Round(grand_total, 2);
                values.grandtotal = Math.Round(grandtotal, 2);
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Edit Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
       "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetEditInvoice(string invoice_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {
                msSQL = " select  a.invoice_gid,a.customer_name,b.customercontact_name,b.mobile as customer_contactnumber,a.shipping_to,c.currencyexchange_gid,a.bill_email," +
                        " b.email as customer_email, z.branch_name,a.customer_address,a.customer_gid,a.mode_of_despatch,a.invoice_type,a.tax_gid,a.tax_amount, " +
                        " a.branch_gid,a.currency_code, a.exchange_rate,a.invoice_remarks,a.invoice_refno, DATE_FORMAT(a.invoice_date, '%d-%m-%Y') as invoice_date, " +
                        " DATE_FORMAT(a.delivery_date, '%d-%m-%Y') as delivery_date,a.delivery_days,a.payment_days,a.payment_term,DATE_FORMAT(a.payment_date, '%d-%m-%Y') as payment_date,a.invoice_amount, a.additionalcharges_amount," +
                        " a.discount_amount,a.freight_charges, a.buyback_charges, a.packing_charges, a.insurance_charges,a.roundoff, a.total_amount," +
                        " a.termsandconditions,a.sales_type,y.salestype_name from rbl_trn_tinvoice a " +
                        " LEFT JOIN crm_mst_tcustomercontact b ON a.customer_gid=b.customer_gid " +
                         " LEFT JOIN hrm_mst_tbranch z ON z.branch_gid=a.branch_gid " +
                        " left join crm_trn_tcurrencyexchange c on c.currency_code=a.currency_code " +
                        " left join smr_trn_tsalestype y on y.salestype_gid = a.sales_type " +
                        " where invoice_gid= '" + invoice_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<InvoiceEdit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new InvoiceEdit_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customercontact_name = dt["customercontact_name"].ToString(),
                            customer_contactnumber = dt["customer_contactnumber"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            customerbranch_name = dt["branch_name"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            mode_of_despatch = dt["mode_of_despatch"].ToString(),
                            invoice_type = dt["invoice_type"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            delivery_date = dt["delivery_date"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            shipping_to = dt["shipping_to"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            bill_email = dt["bill_email"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            salestype_name = dt["salestype_name"].ToString(),
                            sales_type = dt["sales_type"].ToString(),

                        });
                        values.InvoiceEdit_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Edit invoice!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaUpdatedInvoice(string employee_gid, invoicelist values)
        {
            try
            {
                msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                string lstaxname = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select tax_percentage from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                string lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);



                double addonCharges = double.TryParse(values.addon_charge, out double addonChargesValue) ? addonChargesValue : 0;
                double freightCharges = double.TryParse(values.freight_charges, out double freightChargesValue) ? freightChargesValue : 0;
                double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                double addonCharges_l = Math.Round(addonCharges * double.Parse(values.exchange_rate), 2);
                double freightCharges_l = Math.Round(freightCharges * double.Parse(values.exchange_rate), 2); ;
                double insuranceCharges_l = Math.Round(insuranceCharges * double.Parse(values.exchange_rate), 2);
                double additionaldiscountAmount_l = Math.Round(additionaldiscountAmount * double.Parse(values.exchange_rate), 2);


                string uiDateStr = values.invoice_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlinvoiceDate = uiDate.ToString("yyyy-MM-dd");
                string uiDateStr2 = values.due_date;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlpaymentDate = uiDate2.ToString("yyyy-MM-dd");
                double lsexchange = double.Parse(values.exchange_rate);
                double lstotalamount_l = Math.Round((double.Parse(values.totalamount) * lsexchange), 2);
                double lsgrandtotal_l = Math.Round((double.Parse(values.grandtotal) * lsexchange), 2);
                double lsaddoncharges_l = Math.Round((double.Parse(values.addon_charge) * lsexchange), 2);
                double lsadditionaldiscountAmount_l = Math.Round((additionaldiscountAmount * lsexchange), 2);





                msSQL = "select customer_name from crm_mst_tcustomer where customer_gid='" + values.customer_gid + "'";
                string lsCustomername = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid='" + values.currency_code + "'";
                string lscurrencycode = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update rbl_trn_tinvoice set " +
                        " invoice_date='" + mysqlinvoiceDate + "'," +
                        " payment_term='" + values.payment_days + "'," +
                        " payment_date='" + mysqlpaymentDate + "'," +
                        " customer_gid='" + values.customer_gid + "'," +
                        " customer_name='" + lsCustomername.Replace("'", "\\\'") + "'," +
                        " customer_contactnumber='" + values.customercontactnumber + "'," +
                        " customer_address='" + values.customer_address.Replace("'", "\\\'") + "'," +
                        " customer_email='" + values.customeremailaddress + "'," +
                        " total_amount='" + values.totalamount + "'," +
                        " invoice_amount='" + values.grandtotal + "'," +
                         " bill_email='" + values.bill_email + "'," +
                         " mode_of_despatch='" + (String.IsNullOrEmpty(values.dispatch_mode) ? values.dispatch_mode : values.dispatch_mode.Replace("'", "\\\'")) + "'," +
                        " user_gid='" + employee_gid + "'," +
                        " discount_amount='" + additionaldiscountAmount + "'," +
                        " additionalcharges_amount='" + addonCharges + "'," +
                        " total_amount_L='" + lstotalamount_l + "'," +
                        " invoice_amount_L='" + lsgrandtotal_l + "'," +
                        " discount_amount_L='" + lsadditionaldiscountAmount_l + "'," +
                        " additionalcharges_amount_L='" + lsaddoncharges_l + "'," +
                        " termsandconditions='" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "\\\'") + "'," +
                        " currency_code='" + lscurrencycode + "'," +
                        " exchange_rate='" + values.exchange_rate + "'," +
                        " tax_gid='" + values.tax_name4 + "'," +
                         " tax_name='" + lstaxname + "'," +
                         " tax_percentage='" + lstaxpercentage + "'," +
                         " tax_amount='" + values.tax_amount4 + "'," +
                        " branch_gid='" + values.branch_name + "'," +
                        " roundoff='" + roundoff + "'," +
                        " created_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " freight_charges='" + freightCharges + "'," +
                        " sales_type='" + values.sales_type + "'," +
                        " insurance_charges='" + insuranceCharges + "'" +
                        " where invoice_gid='" + values.invoice_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {


                    msSQL = "SELECT  a.invoicedtl_gid, a.product_gid, a.customerproduct_code,a.product_name, a.product_remarks,a.product_code, b.productgroup_gid," +
                    " format(a.qty_invoice, 2) AS qty_ordered_1, a.qty_invoice as qty_ordered, c.productgroup_name, a.uom_name, b.producttype_gid," +
                    " a.created_by, a.product_price, format(a.product_price, 2) AS producttotal_price," +
                    " a.discount_percentage,a.product_total,a.discount_amount, format(a.discount_amount, 2) AS discount_amount_1," +
                    " a.tax_percentage, format(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field," +
                    " a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2,a.tax_percentage3," +
                    " format(a.tax_amount2, 2) AS tax_amount2, format(a.tax_amount3, 2) AS tax_amount3, a.tax1_gid," +
                    " a.tax2_gid, a.tax3_gid ," +
                    " concat( case when a.tax_name is null then '' else a.tax_name end, ' '," +
                    " case when a.tax_percentage = '0' then '' else concat('',a.tax_percentage,'%') end ," +
                    " case when a.tax_amount = '0' then '' else concat(':',a.tax_amount) end) as tax," +
                    " concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ', " +
                    " case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end, " +
                    " case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2," +
                    " concat(case when a.tax_name3 is null then '' else a.tax_name3 end, ' '," +
                    " case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                    " end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3," +
                    " format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.taxsegment_gid ,a.vendor_price" +
                    " FROM rbl_tmp_tinvoicedtl a  " +
                    "LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                    " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid  " +
                    " WHERE a.employee_gid = '" + employee_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        double lsvendorprice = Math.Round(double.Parse(dt["vendor_price"].ToString().Replace(",", "")), 2);
                        double lsproduct_discount = Math.Round(double.Parse(dt["discount_amount"].ToString().Replace(",", "")), 2);
                        double lsproduct_price = Math.Round((double.Parse(dt["qty_ordered"].ToString()) * lsvendorprice) - lsproduct_discount, 2);
                        double lsexchangerate = double.Parse(values.exchange_rate);
                        double lsproductprice_l = Math.Round((lsproduct_price * lsexchangerate), 2);
                        double lstaxamount_l = Math.Round((double.Parse(dt["tax_amount"].ToString().Replace(",", "")) * lsexchangerate), 2);
                        double lstaxamount2_l = Math.Round((double.Parse(dt["tax_amount2"].ToString().Replace(",", "")) * lsexchangerate), 2);
                        double lstaxamount3_l = Math.Round((double.Parse(dt["tax_amount3"].ToString().Replace(",", "")) * lsexchangerate), 2);
                        double lsdiscountamount_l = Math.Round((double.Parse(dt["discount_amount"].ToString().Replace(",", "")) * lsexchangerate), 2);
                        double lsproducttotal_l = Math.Round((double.Parse(dt["product_total"].ToString().Replace(",", "")) * lsexchangerate), 2);
                        msGetGid = objcmnfunctions.GetMasterGID("SIVC");

                        string display_field = dt["product_remarks"].ToString();

                        msSQL = " insert into rbl_trn_tinvoicedtl (" +
                                 " invoicedtl_gid, " +
                                 " invoice_gid, " +
                                 " product_gid, " +
                                 " product_code, " +
                                 " productgroup_gid, " +
                                 " productgroup_name, " +
                                 " product_name, " +
                                 " uom_gid, " +
                                 " productuom_name, " +
                                 " product_price, " +
                                 " vendor_price, " +
                                 " discount_percentage, " +
                                 " discount_amount, " +
                                 " tax_name, " +
                                 " tax_name2, " +
                                 " tax_name3, " +
                                 " tax1_gid," +
                                 " tax2_gid," +
                                 " tax3_gid," +
                                 " tax_percentage, " +
                                 " tax_percentage2, " +
                                 " tax_percentage3, " +
                                 " tax_amount, " +
                                 " tax_amount2, " +
                                 " tax_amount3, " +
                                 " qty_invoice, " +
                                 " product_remarks, " +
                                " product_total, " +
                                " product_price_L, " +
                                " discount_amount_L, " +
                                " tax_amount1_L, " +
                                " tax_amount2_L, " +
                                " tax_amount3_L, " +
                                " product_total_L, " +
                                " display_field, " +
                                " customerproduct_code," +
                                " taxsegment_gid," +
                                " employee_gid," +
                                " created_date " +
                                " ) values ( " +
                                "'" + msGetGid + "'," +
                                "'" + values.invoice_gid + "'," +
                                "'" + dt["product_gid"].ToString() + "'," +
                                "'" + dt["product_code"].ToString() + "'," +
                                "'" + dt["productgroup_gid"].ToString() + "'," +
                                "'" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "'," +
                                "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                                "'" + dt["uom_gid"].ToString() + "'," +
                                "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                                "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                                "'" + lsvendorprice + "'," +
                                "'" + dt["discount_percentage"].ToString() + "'," +
                                "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_name"].ToString() + "'," +
                                "'" + dt["tax_name2"].ToString() + "'," +
                                "'" + dt["tax_name3"].ToString() + "'," +
                                "'" + dt["tax1_gid"].ToString() + "'," +
                                "'" + dt["tax2_gid"].ToString() + "'," +
                                "'" + dt["tax3_gid"].ToString() + "'," +
                                "'" + dt["tax_percentage"].ToString() + "'," +
                                "'" + dt["tax_percentage2"].ToString() + "'," +
                                "'" + dt["tax_percentage3"].ToString() + "'," +
                                "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                                "'" + dt["qty_ordered"].ToString().Replace(",", "") + "',";
                                 if (display_field != null)
                        {
                            msSQL += "'" + display_field.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + display_field + "',";
                        }
                        msSQL += "'" + dt["product_total"].ToString().Replace(",", "") + "'," +
                                  "'" + lsproductprice_l + "'," +
                                  "'" + lsdiscountamount_l + "'," +
                                  "'" + lstaxamount_l + "'," +
                                  "'" + lstaxamount2_l + "'," +
                                  "'" + lstaxamount3_l + "'," +
                                  "'" + lsproducttotal_l + "',";
                                if (display_field != null)
                        {
                            msSQL += "'" + display_field.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + display_field + "',";
                        }
                        msSQL += "'" + dt["customerproduct_code"].ToString() + "'," +
                                "'" + dt["taxsegment_gid"].ToString() + "'," +
                                "'" + employee_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult == 1)
                    {
                        msSQL = " delete from rbl_tmp_tinvoicedtl where employee_gid='" + employee_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = "select finance_flag from adm_mst_tcompany ";
                        string finance_flag = objdbconn.GetExecuteScalar(msSQL);
                        if (finance_flag == "Y")
                        {
                            string lsremarks = "", ls_referenceno = "", lssales_type = "", lsproduct_price_l="", lstax1_gid="", lstax2_gid="";
                            double lsbuyback_charges = 0, lspacking_charges = 0;
                            msSQL = " Select invoice_remarks ,invoice_gid, invoice_refno ,sales_type,buyback_charges,packing_charges from rbl_trn_Tinvoice where invoice_gid='" + values.invoice_gid + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                            if (objOdbcDataReader.HasRows == true)
                            {
                                lsremarks = objOdbcDataReader["invoice_remarks"].ToString();
                                ls_referenceno = objOdbcDataReader["invoice_refno"].ToString();
                                lssales_type = objOdbcDataReader["sales_type"].ToString();
                                lsinvoice_gid = objOdbcDataReader["invoice_gid"].ToString();
                                lsbuyback_charges = double.Parse(objOdbcDataReader["buyback_charges"].ToString());
                                lspacking_charges = double.Parse(objOdbcDataReader["packing_charges"].ToString());

                            }
                            objOdbcDataReader.Close();


                            double roundoff1 = roundoff * Convert.ToDouble(values.exchange_rate);
                            double lsbuyback_charges_l = lsbuyback_charges * Convert.ToDouble(values.exchange_rate);
                            double lspacking_charges_l = lspacking_charges * Convert.ToDouble(values.exchange_rate);
                            msSQL = " select sum(product_price) as product_price_L,sum(tax_amount) as tax1,sum(tax_amount2) as tax2,tax1_gid,tax2_gid from rbl_trn_tinvoicedtl " +
                                     " where invoice_gid='" + lsinvoice_gid + "' ";
                            objOdbcDataReader=objdbconn.GetDataReader (msSQL);
                            if(objOdbcDataReader.HasRows == true)
                            {
                                lsproduct_price_l = objOdbcDataReader["product_price_L"].ToString();
                                lstax1 = objOdbcDataReader["tax1"].ToString();
                                lstax2 = objOdbcDataReader["tax2"].ToString();
                                lstax1_gid = objOdbcDataReader["tax1_gid"].ToString();
                                lstax2_gid = objOdbcDataReader["tax2_gid"].ToString();
                            }
                            objOdbcDataReader.Close ();
                            lsbasic_amount = Math.Round(double.Parse(lsproduct_price_l)* Convert.ToDouble(values.exchange_rate), 2);



                            objfincmn.jn_updatedinvoice(mysqlinvoiceDate, lsremarks, values.branch_name, ls_referenceno, lsinvoice_gid
                             , lsbasic_amount, addonCharges_l, additionaldiscountAmount_l, lsgrandtotal_l, values.customer_gid, "Invoice", "RBL",
                             lssales_type, roundoff1, freightCharges_l, lsbuyback_charges_l, lspacking_charges_l, insuranceCharges_l, values.tax_amount4, values.tax_name4);
                          
                                   


                                    if (lstax1 != "0.00" && lstax1 != "" && lstax1 != null)
                                    {
                                      decimal lstaxsum = decimal.Parse(lstax1);
                                        string lstaxamount = lstaxsum.ToString("F2");
                                        double tax_amount = Math.Round(double.Parse(lstaxamount)* Convert.ToDouble(values.exchange_rate), 2);

                                        objfincmn.jn_sales_tax(lsinvoice_gid, ls_referenceno, lsremarks, tax_amount, lstax1_gid);
                                    }
                                    if (lstax2 != "0.00" && lstax2 != "" && lstax2 != null && lstax2 != "0")
                                    {
                                               decimal lstaxsum = decimal.Parse(lstax2);
                                              string lstaxamount = lstaxsum.ToString("F2");
                                                double tax_amount = Math.Round(double.Parse(lstaxamount) * Convert.ToDouble(values.exchange_rate), 2);

                                        objfincmn.jn_sales_tax(lsinvoice_gid, ls_referenceno, lsremarks, tax_amount, lstax2_gid);
                                    }
                               
                        }

                    }
                   

                    values.status = true;
                    values.message = "Invoice Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating The Invoice";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Invoice Update!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetDirectInvoiceEditProductSummary(string invoicedtl_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = "select invoicedtl_gid,invoice_gid,product_gid,qty_invoice,product_price,discount_percentage," +
                    "discount_amount,tax_amount,product_total,uom_gid,uom_name,tax_amount2,tax_name,tax_name2," +
                    "display_field,tax1_gid,tax2_gid,product_name,productgroup_gid,productgroup_name,employee_gid," +
                    "product_code,tax_percentage,tax_percentage2,vendor_price from rbl_tmp_tinvoicedtl  where invoicedtl_gid='" + invoicedtl_gid + "';";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoiceproductsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new invoiceproductsummary_list
                        {

                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            tax1_gid = dt["tax1_gid"].ToString(),

                            product_total = dt["product_total"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),

                        });
                        values.invoiceproductsummarylist = getModuleList;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  DirectQuotationEditProductSummary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaUpdateDirectInvoiceEditProductSummary(string employee_gid, invoiceproductlist values)
        {
            try
            {

                double lstotalamount = Math.Round((values.unitprice), 2) * (values.quantity);
                double discountamount = Math.Round(lstotalamount * ((values.discountpercentage) / 100), 2);
                double lsGrandtotal = Math.Round(((values.unitprice) * (values.quantity)) - discountamount, 2);
                msGetGid = objcmnfunctions.GetMasterGID("SIVC");
                string lsrefno = "";
                //double lspercentage1 = 0;
                //double lspercentage2 = 0;
                string lspercentage1, lspercentage2;
                double tax1_amount = 0;
                double tax2_amount = 0;

                string lstax1, lstax2;
                msSQL = "select product_name from pmr_mst_tproduct where product_gid='" + values.productgid + "'";
                string lsproductName = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_gid + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_gid + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                if (values.taxname1 == "" || values.taxname1 == null)
                {
                    lspercentage1 = "0";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.taxname1 + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                    tax1_amount = Math.Round(lsGrandtotal * (Convert.ToDouble(lspercentage1) / 100), 2);
                }
                if (values.taxname2 == "" || values.taxname2 == null)
                {
                    lspercentage2 = "0";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.taxname2 + "'";
                    lspercentage2 = objdbconn.GetExecuteScalar(msSQL);
                    tax2_amount = Math.Round(lsGrandtotal * (Convert.ToDouble(lspercentage2) / 100), 2);
                }
                double Grandtotalamount = Math.Round((lsGrandtotal + tax1_amount + tax2_amount), 2);

                if (values.taxname1 == "" || values.taxname1 == null)
                {
                    lstax1 = "0";
                }
                else
                {
                    msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + values.taxname1 + "'";
                    lstax1 = objdbconn.GetExecuteScalar(msSQL);
                }

                if (values.taxname2 == "" || values.taxname2 == null)
                {
                    lstax2 = "0";
                }
                else
                {
                    msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.taxname2 + "'";
                    lstax2 = objdbconn.GetExecuteScalar(msSQL);
                }
                {
                    msSQL = "UPDATE rbl_tmp_tinvoicedtl SET qty_invoice = '" + values.quantity + "', " +
                        "product_price = '" + values.unitprice + "', " +
                        "discount_percentage = '" + values.discountpercentage + "'," +
                        " discount_amount = '" + discountamount + "'," +
                        " tax_amount = '" + tax1_amount + "', " +
                        "product_total = '" + Grandtotalamount + "', " +
                        "uom_gid = '" + lsproductuomgid + "'," +
                        " uom_name = '" + values.productuom_gid + "', " +
                        "tax_amount2 = '" + tax2_amount + "'," +
                        " tax_name = '" + lstax1 + "'," +
                        " tax_name2 = '" + lstax2 + "', " +
                        "display_field = '" + values.productdescription + "'," +
                        " tax1_gid = '" + values.taxname1 + "'," +
                        " tax2_gid = '" + values.tax_gid2 + "'," +
                        " product_name = '" + lsproductName + "', " +
                        "productgroup_gid = '" + lsproductgroupgid + "'," +
                        " productgroup_name = '" + values.productgroup_gid + "', " +
                        "employee_gid = '" + employee_gid + "', " +
                        "product_code = '" + values.productcode + "', " +
                        "tax_percentage = '" + lspercentage1 + "'," +
                        " tax_percentage2 = '" + lspercentage2 + "', " +
                        "vendor_price = '" + lsGrandtotal + "' WHERE invoicedtl_gid = '" + values.invoicedtl_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Invoice Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaSalesinvoiceSummary(MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = " select distinct b.salesorder_gid as directorder_gid,c.leadbank_gid,d.lead2campaign_gid,b.salesorder_date as directorder_date,b.salesorder_gid as directorder_refno,b.customer_name, " +
                        " b.customer_contact_person, cast(concat(b.so_referenceno1," +
                        " if(b.so_referencenumber<>'',concat(' ',' | ',' ',b.so_referencenumber),'') ) as char) as so_referenceno1,b.progressive_flag, " +
                        " concat(b.customer_contact_person,' / ',b.customer_email,' / ',b.customer_mobile) as mobile, " +
                        " format(b.Grandtotal,2) as grandtotal,format(b.invoice_amount,2) as invoice_amout,format(b.Grandtotal-b.invoice_amount,2) as outstanding_amount, " +
                        " b.currency_code,invoice_flag as status,b.customer_gid, b.so_type as order_type " +
                        " from smr_trn_tsalesorder b " +
                        " left join smr_trn_tdeliveryorder a on b.salesorder_gid=a.salesorder_gid " +
                        " left join crm_trn_tleadbank c on c.customer_gid = b.customer_gid " +
                        " left join crm_trn_tlead2campaign d on d.leadbank_gid = c.leadbank_gid " +
                        " where 1=1 and case when so_type='Sales' then  b.salesorder_status not in  ('SO Amended','Rejected','Cancelled','Approve Pending')" +
                        " else b.salesorder_status in('Delivery Done Partial','Delivery Completed','Approved') end" +
                        " order by directorder_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<salesinvoicesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salesinvoicesummary_list
                        {
                            directorder_gid = dt["directorder_gid"].ToString(),
                            directorder_date = Convert.ToDateTime(dt["directorder_date"].ToString()),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            order_type = dt["order_type"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            grandtotal = dt["grandtotal"].ToString(),
                            invoice_amout = dt["invoice_amout"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString(),
                            status = dt["status"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                        });
                        values.salesinvoicesummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while loading Sales Invoice!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetproductsummarydata(MdlSmrRptInvoiceReport values, string directorder_gid)
        {
            try
            {
                msSQL = " select a.product_gid,a.salesorderdtl_gid as serviceorderdtl_gid,d.productuom_name,b.customerproduct_code,  a.tax1_gid,format(a.qty_quoted, 2) as qty_quoted,b.product_code, d.productuom_name," +
                      " a.uom_gid, format(a.tax_amount, 2) as tax_amount1, format(a.price, 2) as total_amount, a.product_name,a.tax_name," +
                      " format(a.discount_amount, 2) as discount_amount,format(a.product_price, 2) as product_price, a.discount_percentage" +
                      " from smr_trn_tsalesorderdtl a " +
                      " left join pmr_mst_tproduct b on a.product_gid = b.product_gid" +
                      " left join smr_trn_tsalesorder c on a.salesorder_gid = c.salesorder_gid " +
                      " left join pmr_mst_tproductuom d on b.productuom_gid = d.productuom_gid " +
                      " where a.salesorder_gid = '" + directorder_gid + "' group by a.product_gid order by a.salesorderdtl_gid asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salesinvoiceproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salesinvoiceproduct_list
                        {

                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            unit = dt["productuom_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            tax = dt["tax_name"].ToString(),
                            tax1_gid = dt["tax1_gid"].ToString(),
                            tax_amount1 = dt["tax_amount1"].ToString(),
                        });
                        values.salesinvoiceproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DasalesinvoiceOnsubmit(string employee_gid, salesinvoice_list values)
        {

            try
            {
                string uiDateStr = values.invoice_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlinvoiceDate = uiDate.ToString("yyyy-MM-dd");

                string uiDateStr2 = values.invoiceaccounting_duedate;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlpaymentDate = uiDate2.ToString("yyyy-MM-dd");

                msSQL = "select branch_gid from hrm_mst_tbranch where branch_name='" + values.invoiceaccounting_branch + "'";
                string lsbranchgid = objdbconn.GetExecuteScalar(msSQL);


                string lstype1 = "services";
                string ls_referenceno = "";
                ls_referenceno = objcmnfunctions.getSequencecustomizerGID("INV", "RBL", values.branch_name);
                msINGetGID = objcmnfunctions.GetMasterGID("SIVT");

                msSQL = " insert into rbl_trn_tinvoice(" +
                     " invoice_gid," +
                     " invoice_date," +
                     " payment_term, " +
                     " payment_date," +
                     " invoice_type," +
                     " invoice_reference," +
                     " customer_gid," +
                     " customer_name," +
                     " customer_contactperson," +
                     " customer_contactnumber," +
                     " customer_address," +
                     " customer_email," +
                     " branch_gid," +
                     " total_amount," +
                     " invoice_amount," +
                     " invoice_refno," +
                     " invoice_status," +
                     " invoice_flag," +
                     " user_gid," +
                     " total_amount_L," +
                     " invoice_amount_L," +
                     " invoice_remarks," +
                     " termsandconditions," +
                     " currency_code," +
                     " exchange_rate," +
                     " created_date," +
                     " freight_charges," +
                     " packing_charges," +
                     " insurance_charges " +
                     " ) values (" +
                     " '" + msINGetGID + "'," +
                     "'" + mysqlinvoiceDate + "'," +
                     "'" + values.invoiceaccounting_payterm + "'," +
                     "'" + mysqlpaymentDate + "'," +
                     "'" + lstype1 + "'," +
                     "'" + values.invoiceaccounting_salesorder_gid + "'," +
                     "'" + values.customer_gid + "'," +
                     "'" + values.invoiceaccounting_customername + "'," +
                     "'" + values.invoiceaccounting_contactperson + "'," +
                     "'" + values.invoiceaccounting_contactnumber + "'," +
                     "'" + values.invoiceaccounting_customeraddress.Replace("'", "\\\'") + "'," +
                     "'" + values.invoiceaccounting_email + "'," +
                     "'" + lsbranchgid + "'," +
                     "'" + values.invoiceaccounting_ordertotal_withtax.ToString().Replace(",", "") + "'," +
                     "'" + values.invoiceaccounting_grandtotal.ToString().Replace(",", "") + "'," +
                     "'" + ls_referenceno + "'," +
                     "'Payment Pending'," +
                     "'Invoice Approved'," +
                     "'" + employee_gid + "'," +
                     "'" + values.invoiceaccounting_ordertotal_withtax.ToString().Replace(",", "") + "'," +
                     "'" + values.invoiceaccounting_grandtotal.ToString().Replace(",", "") + "'," +
                     "'" + values.invoiceaccounting_remarks + "'," +
                     "'" + values.invoiceaccounting_termsandconditions.Replace("'", "\\\'") + "', " +
                     "'" + values.invoiceaccounting_currency + "', " +
                     "'" + values.invoiceaccounting_exchangerate + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                     "'" + values.invoiceaccounting_freightcharges + "'," +
                     "'" + values.invoiceaccounting_packingcharges + "'," +
                     "'" + values.invoiceaccounting_insurancecharges + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " select a.product_gid,a.product_code,e.productgroup_name,d.productuom_name,format(a.product_price, 2) as product_price," +
                            " a.salesorderdtl_gid as serviceorderdtl_gid,a.tax1_gid,a.tax_name,format(a.qty_quoted, 2) as qty_quoted,a.uom_gid ," +
                            " format(a.tax_amount, 2) as tax_amount, format(a.price, 2) as total_amount, e.productgroup_gid, a.product_name,a.tax_name, a.discount_amount, " +
                            " a.discount_percentage from smr_trn_tsalesorderdtl a  " +
                            " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                            " left join pmr_mst_tproductuom d on b.productuom_gid = d.productuom_gid " +
                            " left join pmr_mst_tproductgroup e on b.productgroup_gid = e.productgroup_gid " +
                            " left join smr_trn_tsalesorder c on a.salesorder_gid = c.salesorder_gid " +
                            " where a.salesorder_gid = '" + values.invoiceaccounting_salesorder_gid + "' order by a.salesorderdtl_gid asc";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("SIVC");
                        msSQL = " insert into rbl_trn_tinvoicedtl ( " +
                                                    " invoicedtl_gid," +
                                                    " invoice_gid," +
                                                    " product_gid," +
                                                    " product_price," +
                                                    " discount_percentage," +
                                                    " discount_amount," +
                                                    " uom_gid," +
                                                    " tax_amount," +
                                                    " tax_name," +
                                                    " tax1_gid," +
                                                    " product_name," +
                                                    " employee_gid, " +
                                                    " product_code," +
                                                    " uom_name," +
                                                    " productgroup_gid, " +
                                                    " productgroup_name," +
                                                    " selling_price," +
                                                    " product_price_L " +
                                                    " ) values ( " +
                                                    "'" + msGetGid + "'," +
                                                    "'" + msINGetGID + "'," +
                                                    "'" + dt["product_gid"].ToString() + "'," +
                                                    "'" + dt["product_price"].ToString() + "'," +
                                                    "'" + dt["discount_percentage"].ToString() + "',";
                        if (dt["discount_amount"].ToString() != "")
                        {
                            msSQL += "'" + dt["discount_amount"].ToString().Replace(",", "") + "',";
                        }
                        else
                        {
                            msSQL += "'0.00', ";
                        }
                        msSQL += "'" + dt["uom_gid"].ToString() + "'," +
                                                     "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                                     "'" + dt["tax_name"].ToString() + "'," +
                                                     "'" + dt["tax1_gid"].ToString() + "'," +
                                                     "'" + dt["product_name"].ToString() + "'," +
                                                     "'" + employee_gid + "'," +
                                                     "'" + dt["product_code"].ToString() + "'," +
                                                     "'" + dt["productuom_name"].ToString() + "'," +
                                                     "'" + dt["productgroup_gid"].ToString() + "'," +
                                                     "'" + dt["productgroup_name"].ToString() + "'," +
                                                     "'" + dt["product_price"].ToString() + "'," +
                                                     "'" + dt["product_price"].ToString().Replace(",", "") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Invoice Added Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Invoice";
                        return;
                    }
                }
                else
                {
                    values.status = false;
                    values.message = " Error While Raising Invoice";
                    return;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Invoice Submit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
   "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        //Rpt//
        public Dictionary<string, object> DaGetInvoicePDF(string invoice_gid, MdlSmrRptInvoiceReport values)
        {

            string full_path = null;
            msSQL = " select company_code from adm_mst_Tcompany";
            string lscompanycode = objdbconn.GetExecuteScalar(msSQL);

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();
            currency number2words = fnconvertnumbertowords(invoice_gid, "INVOICE_REPORT");

            msSQL = "select  '" + number2words.number2words + "' AS amount_words ,DATE_FORMAT(a.invoice_date,'%d/%m/%Y') as invoice_date, DATE_FORMAT(a.payment_date, '%d/%m/%Y') AS payment_date, " +
                                       " CONCAT('" + number2words.symbol + " ',format(a.invoice_amount, 2)COLLATE utf8mb4_general_ci) as invoice_amount, a.irn ,a.complaint_gid," +
                                       " a.invoice_refno,freight_charges as  freightcharges_amount,  " +
                                       " a.discount_amount,a.total_amount, a.advance_amount, a.customer_name, c.gst_number,h.gst_no as invoice_gid," +
                                       " b.customer_id, a.tin_number, a.cst_number,shipping_to as DataColumn1 , a.roundoff as DataColumn27," +
                                       " a.customer_address,a.customer_contactperson as customercontact_name, a.customer_email as email, " +
                                       " a.customer_contactnumber, a.invoice_amount as DataColumn3, " +
                                       " a.po_number as invoice_reference, a.termsandconditions,a.currency_code " +
                                       " from rbl_trn_tinvoice a " +
                                       " left join crm_mst_tcustomer b on b.customer_gid=a.customer_gid " +
                                       " left join crm_mst_tcustomercontact c on c.customer_gid=b.customer_gid " +
                                       " left join hrm_mst_tbranch h on h.branch_gid=a.branch_gid " +
                                       " where a.invoice_gid='" + invoice_gid + "' ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = "SELECT a.invoicedtl_gid, a.invoice_gid,  " +
                  " a.product_gid, a.productuom_name, " +
                  "case  when a.display_field is null or  a.display_field='' and a.product_remarks is null or a.product_remarks='' and b.hsn_number is null  then concat(a.product_code,'-',a.product_name) " +
                  " when a.display_field is null or  a.display_field='' and a.product_remarks is null or a.product_remarks=''  then concat(b.hsn_number,'-',a.product_name) " +
                  " when a.display_field is null or a.display_field='' then concat(b.hsn_number,'-',a.product_name,'-',a.product_remarks)  " +
                  " when a.display_field is null or a.display_field='' then concat(b.hsn_number,'-',a.product_name,'-',a.product_remarks) " +
                  "  else concat(b.hsn_number,'-',a.product_name,'-',a.display_field) end as DataColumn39, " +
                  " a.product_name,case when a.display_field is null or a.display_field='' then a.product_name else concat(a.product_name,'-', a.display_field ) End AS display_field_1, " +
                   " a.qty_invoice, a.vendor_price as  product_price, a.tax_amount,a.discount_percentage,a.discount_amount ," +
                   "  case when b.hsn_number is null then a.product_code else b.hsn_number end as product_code," +
                    " FORMAT(((a.qty_invoice * a.vendor_price) - a.discount_amount), 2) AS product_total, " +
                  " CASE " +
                  " WHEN a.tax_amount2 = 0 THEN CONCAT(a.tax_name COLLATE latin1_general_ci, ':',  ' ', a.tax_amount) " +
                  " WHEN a.tax_amount = 0 THEN CONCAT(a.tax_name2 COLLATE latin1_general_ci, ':',  ' ', a.tax_amount2) " +
                 " ELSE CONCAT(a.tax_name COLLATE latin1_general_ci, ':',  ' ', a.tax_amount, ' ', a.tax_name2 COLLATE latin1_general_ci, ':', ' ', a.tax_amount2) " +
                 "  END AS all_taxes, " +
                " FORMAT((((a.qty_invoice * a.vendor_price) - a.discount_amount)+a.tax_amount+a.tax_amount2), 2) AS DataColumn17,a.tax_percentage as DataColumn18 " +
                 " FROM rbl_trn_tinvoicedtl a " +
                 " LEFT JOIN rbl_trn_tinvoice e ON e.invoice_gid = a.invoice_gid " +
                 " LEFT JOIN pmr_mst_tproduct b ON b.product_gid = a.product_gid " +
                 " LEFT JOIN pmr_mst_tproductgroup c ON c.productgroup_gid = b.productgroup_gid " +
                 " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid " +
                 " LEFT JOIN smr_trn_tsalesorderdtl m ON m.salesorderdtl_gid = a.salesorderdtl_gid " +
                 " WHERE a.invoice_gid = '" + invoice_gid + "' ";


            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");
            if (lscompanycode == "BOBA")
            {
                msSQL = " select a.invoice_gid,  a.tax_name as DataColumn36," +
                         " Format(a.tax_amount,2)as DataColumn35, a.tax_name2 as DataColumn34, " +
                         " (a.tax_amount2)as DataColumn33,case when (a.tax_name3='--No Tax--' or a.tax_name3='NoTax') then ' ' when (a.tax_name3<>'--No Tax--' or a.tax_name3<>'NoTax') then a.tax_name3 end as thirdtax_name3," +
                         " case when format(sum(a.tax_amount3),2)=0.00 then ' ' when format(sum(a.tax_amount3),2)<>0.00 then format(sum(a.tax_amount3),2) end as sum_tax3," +
                         " format(sum((a.tax_amount + a.tax_amount2 + a.tax_amount3)),2) as total_tax_amount,(a.tax_amount3)as thirdtax_amount,sum((((a.qty_invoice*a.vendor_price)-a.discount_amount)))as total_productprice ," +
                         " CONCAT('" + number2words.symbol + " ', (CAST(SUM(CASE WHEN a.tax_name in ( 'ZERO VAT','VAT 0%') THEN  (a.qty_invoice * a.vendor_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn14," +
                         " CONCAT('" + number2words.symbol + " ', (CAST(SUM(CASE WHEN a.tax_name in ( 'ZERO VAT','VAT 0%') THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn15," +
                         " CONCAT('" + number2words.symbol + " ', (CAST(SUM(CASE WHEN a.tax_name In ('VAT 20%') AND a.tax_name NOT LIKE '%ZERO VAT%' THEN  (a.qty_invoice * a.vendor_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn16," +
                         " CONCAT('" + number2words.symbol + " ', (CAST(SUM(CASE WHEN a.tax_name LIKE '%VAT%' AND a.tax_name NOT LIKE '%ZERO VAT%' THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn17,sum(a.discount_amount) as DataColumn26" +
                         " from rbl_trn_tinvoicedtl a " +
                         " left join rbl_trn_tinvoice e on e.invoice_gid=a.invoice_gid " +
                         " left join pmr_mst_tproduct b on b.product_gid=a.product_gid " +
                         " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                         " left join pmr_mst_tproductuom d on d.productuom_gid=a.uom_gid " +
                         " where a.invoice_gid = '" + invoice_gid + "'";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");
            }
            else
            {
                msSQL = " select a.invoice_gid,  a.tax_name  as DataColumn36,a.tax_name2  as DataColumn34," +
                       " (a.tax_amount)as firsttax_amount,case when (a.tax_name2='--No Tax--' or a.tax_name2='NoTax') then ' ' when (a.tax_name2<>'--No Tax--' or a.tax_name2<>'NoTax') then a.tax_name2 end as secondtax_name2," +
                       " (a.tax_amount2)as secondtax_amount,case when (a.tax_name3='--No Tax--' or a.tax_name3='NoTax') then ' ' when (a.tax_name3<>'--No Tax--' or a.tax_name3<>'NoTax') then a.tax_name3 end as thirdtax_name3," +
                       " case when format(sum(a.tax_amount),2)=0.00 then 0 when format(sum(a.tax_amount),2)<>0.00 then format(sum(a.tax_amount),2) end as DataColumn35 ," +
                       " case when format(sum(a.tax_amount2),2)=0.00 then 0 when format(sum(a.tax_amount2),2)<>0.00 then format(sum(a.tax_amount2),2) end as DataColumn33," +
                       " case when format(sum(a.tax_amount3),2)=0.00 then 0 when format(sum(a.tax_amount3),2)<>0.00 then format(sum(a.tax_amount3),2) end as sum_tax3," +
                       " format(sum((a.tax_amount + a.tax_amount2 + a.tax_amount3)),2) as total_tax_amount,(a.tax_amount3)as thirdtax_amount,sum(a.product_price)as total_productprice,sum(a.discount_amount) as DataColumn26" +
                       " from rbl_trn_tinvoicedtl a " +
                       " left join rbl_trn_tinvoice e on e.invoice_gid=a.invoice_gid " +
                       " left join pmr_mst_tproduct b on b.product_gid=a.product_gid " +
                       " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                       " left join pmr_mst_tproductuom d on d.productuom_gid=a.uom_gid " +
                       " where a.invoice_gid = '" + invoice_gid + "' group by a.invoice_gid";
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");
            }


            msSQL = "SELECT a.address1, a.branch_name,a.branch_logo_path as DataColumn3 ,b.qr_path as DataColumn5, a.authorized_sign_path as DataColumn4,a.city, a.gst_no, a.state, a.postal_code, a.contact_number, a.email, a.email_id, a.branch_gid, a.branch_logo, a.tin_number, a.cst_number,b.termsandconditions " +
                     "FROM rbl_trn_Tinvoicedtl c " +
                     "LEFT JOIN rbl_trn_tinvoice b ON c.invoice_gid = b.invoice_gid " +
                     "LEFT JOIN hrm_mst_tbranch a ON b.branch_gid = a.branch_gid " +
                     "WHERE b.invoice_gid='" + invoice_gid + "' group by b.invoice_gid";

            dt1 = objdbconn.GetDataTable(msSQL);
            DataTable4.Columns.Add("DataColumn3", typeof(byte[]));
            DataTable4.Columns.Add("DataColumn4", typeof(byte[]));
            DataTable4.Columns.Add("DataColumn5", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["DataColumn3"].ToString().Replace("../../", ""));
                    authorized_sign_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["DataColumn4"].ToString().Replace("../../", ""));
                    qr_path = dr_datarow["DataColumn5"].ToString();

                    if (System.IO.File.Exists(company_logo_path) && System.IO.File.Exists(authorized_sign_path)&&System.IO.File.Exists(qr_path)==false)
                    {
                        //Convert  Image Path to Byte
                        branch_logo = System.Drawing.Image.FromFile(company_logo_path);
                        DataColumn4 = System.Drawing.Image.FromFile(authorized_sign_path);
                        byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                        byte[] DataColumn4_bytes = (byte[])(new ImageConverter()).ConvertTo(DataColumn4, typeof(byte[]));

                        DataRow newRow = DataTable4.NewRow();
                        newRow["DataColumn3"] = branch_logo_bytes;
                        newRow["DataColumn4"] = DataColumn4_bytes;
                        DataTable4.Rows.Add(newRow);
                    }
                    if (System.IO.File.Exists(qr_path))
                    {
                        branch_logo = System.Drawing.Image.FromFile(company_logo_path);

                        DataColumn4 = System.Drawing.Image.FromFile(authorized_sign_path);
                        qrpath = System.Drawing.Image.FromFile(qr_path);


                        byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                        byte[] DataColumn4_bytes = (byte[])(new ImageConverter()).ConvertTo(DataColumn4, typeof(byte[]));

                        byte[] qrpath_bytes = (byte[])(new ImageConverter()).ConvertTo(qrpath, typeof(byte[]));
                        DataRow newRow = DataTable4.NewRow();
                        newRow["DataColumn3"] = branch_logo_bytes;
                        newRow["DataColumn4"] = DataColumn4_bytes;
                        newRow["DataColumn5"] = qrpath_bytes;
                        DataTable4.Rows.Add(newRow);



                    }
                }
            }




            dt1.Dispose();
            DataTable4.TableName = "DataTable4";
            myDS.Tables.Add(DataTable4);
            msSQL = "select service_rpt from rbl_mst_tconfiguration";

            string lspdf = objdbconn.GetExecuteScalar(msSQL);
            msSQL = "select invoice_refno from rbl_trn_Tinvoice where invoice_gid='" + invoice_gid + "'";
            string lspdfname = objdbconn.GetExecuteScalar(msSQL);
            string lspdf_name = lspdfname.Replace("/", "");


            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_sales"].ToString(), lspdf));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), lspdf_name + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();
            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;


        }
        public currency fnconvertnumbertowords(string gid, string type)
        {
            currency obj = new currency();
            string number = string.Empty;
            string words = string.Empty;
            string lscurrency_code = string.Empty;

            if (type == "PO_REPORT")
            {
                msSQL = "select total_amount from pmr_trn_tpurchaseorder where purchaseorder_gid='" + gid + "'";
                number = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select currency_code from pmr_trn_tpurchaseorder where purchaseorder_gid='" + gid + "'";
                lscurrency_code = objdbconn.GetExecuteScalar(msSQL);
            }
            else if (type == "SQ_REPORT")
            {
                msSQL = "select Grandtotal from smr_trn_treceivequotation where quotation_gid='" + gid + "'";
                number = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select currency_code from smr_trn_treceivequotation where quotation_gid='" + gid + "'";
                lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

            }
            else if (type == "INVOICE_REPORT")
            {
                msSQL = "select invoice_amount from rbl_trn_tinvoice where invoice_gid='" + gid + "'";
                number = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select currency_code from rbl_trn_tinvoice where invoice_gid='" + gid + "'";
                lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

            }
            string[] strarr = number.Split('.');
            string int_part = objcmnfunctions.NumberToWords(Int32.Parse(strarr[0]));


            string dec_part = "";
            if (strarr.Length > 1 && !string.IsNullOrEmpty(strarr[1]))
            {
                dec_part = objcmnfunctions.NumberToWords(Int32.Parse(strarr[1]));
            }

            if (!string.IsNullOrEmpty(dec_part))
            {
                if (lscurrency_code == "INR")
                {
                    words = int_part + " RUPEES AND " + dec_part + " PAISA ONLY";
                    words = words.ToUpper();
                    obj.symbol = "₹";

                }
                else if (lscurrency_code == "GBP")
                {
                    words = int_part + " POUNDS AND " + dec_part + " PENCE ONLY";
                    words = words.ToUpper();
                    obj.symbol = "£";
                }
                else if (lscurrency_code == "EUR")
                {
                    words = int_part + " EUROS AND " + dec_part + " CENTS ONLY";
                    words = words.ToUpper();
                    obj.symbol = "€";
                }

            }
            else
            {
                if (lscurrency_code == "INR")
                {
                    words = int_part + " RUPEES ONLY";
                    words = words.ToUpper();
                    obj.symbol = "₹";
                }

                else if (lscurrency_code == "GBP")
                {
                    words = int_part + " POUNDS ONLY";
                    words = words.ToUpper();
                    obj.symbol = "£";

                }
                else if (lscurrency_code == "EUR")
                {
                    words = int_part + " EUROS ONLY";
                    words = words.ToUpper();
                    obj.symbol = "€";

                }
            }

            obj.number2words = words;
            return obj;
        }

        public class currency
        {
            public string number2words { get; set; }
            public string symbol { get; set; }
        }


        public void DaGetSendMail_MailId(MdlSmrRptInvoiceReport values, string invoice_gid)
        {

            try
            {


                msSQL = " select c.invoice_gid,c.invoice_refno,DATE_FORMAT(c.payment_date,'%d-%m-%Y') as payment_date,c.invoice_amount,eportal_emailid,group_concat(a.email SEPARATOR ',') as contact_emails,group_concat(a.billing_email SEPARATOR ',' ) as billing_email,b.customer_name,b.customer_gid from crm_mst_tcustomercontact a " +
                    " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                    " left join rbl_trn_tinvoice c on b.customer_gid = c.customer_gid where c.invoice_gid = '" + invoice_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetSendMail_MailId>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetSendMail_MailId

                        {
                            to_customer_email = dt["contact_emails"].ToString(),
                            cc_contact_emails = dt["billing_email"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_reference = dt["invoice_refno"].ToString(),
                            due_date = dt["payment_date"].ToString(),
                            total_amount = dt["invoice_amount"].ToString(),

                        });
                        values.GetSendMail_MailId = getModuleList;
                    }
                }
                dt_datatable.Dispose();

                msSQL = "select a.company_name , b.symbol from adm_mst_tcompany a left join crm_trn_tcurrencyexchange b on b.country_gid = a.country_gid";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.subcompanyname = objMySqlDataReader["company_name"].ToString();
                    values.subsymbol = objMySqlDataReader["symbol"].ToString();

                    objMySqlDataReader.Close();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Mail Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetfrommailid(MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = "select accounts_email from adm_mst_tcompany";
                values.frommailid = objdbconn.GetExecuteScalar(msSQL);

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Mail Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public gmailconfiguration gmailcrendentials()
        {
            gmailconfiguration getgmailcredentials = new gmailconfiguration();
            try
            {
                msSQL = "select * from  smr_smm_gmail_service ;";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objMySqlDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objMySqlDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objMySqlDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objMySqlDataReader["gmail_address"].ToString();
                    getgmailcredentials.default_template = objMySqlDataReader["default_template"].ToString();
                }
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            return getgmailcredentials;
        }

        public graphtoken generateGraphAccesstoken()
        {
            graphtoken objtoken = new graphtoken();
            mdlgraph_list objMdlGraph = new mdlgraph_list();

            try
            {
                msSQL = "select client_id,client_secret,tenant_id from crm_smm_outlook_service";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader != null && objMySqlDataReader.HasRows)
                {
                    objMdlGraph.tenantID = objMySqlDataReader["tenant_id"].ToString();
                    objMdlGraph.clientID = objMySqlDataReader["client_id"].ToString();
                    objMdlGraph.client_secret = objMySqlDataReader["client_secret"].ToString();
                }
                objMySqlDataReader.Close();

                if (!string.IsNullOrEmpty(objMdlGraph.tenantID) && !string.IsNullOrEmpty(objMdlGraph.clientID) && !string.IsNullOrEmpty(objMdlGraph.client_secret))
                {
                    msSQL = "select token,expiry_time from crm_smm_tgraphtoken";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader != null && objMySqlDataReader.HasRows)
                    {
                        DateTime expiry = DateTime.Parse(objMySqlDataReader["expiry_time"].ToString());
                        if (DateTime.Compare(expiry, DateTime.Now) < 0)
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["GraphLoginURL"].ToString());
                            var request = new RestRequest("/" + objMdlGraph.tenantID + "/oauth2/v2.0/token", Method.POST);
                            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                            request.AddParameter("client_id", objMdlGraph.clientID);
                            request.AddParameter("scope", ConfigurationManager.AppSettings["GraphLoginScope"].ToString());
                            request.AddParameter("client_secret", objMdlGraph.client_secret);
                            request.AddParameter("grant_type", "client_credentials");
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                graphLoginSuccessResponse objgraphLoginSuccessResponse = new graphLoginSuccessResponse();
                                objgraphLoginSuccessResponse = JsonConvert.DeserializeObject<graphLoginSuccessResponse>(response.Content);
                                objtoken.access_token = objgraphLoginSuccessResponse.access_token;
                                objtoken.status = true;
                                msSQL = "update crm_smm_tgraphtoken set token = '" + objtoken.access_token +
                                        "',expiry_time = '" + DateTime.Now.AddSeconds(3595).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 0)
                                {
                                    objcmnfunctions.LogForAudit("Error occurred while insert: " + msSQL);
                                }
                            }
                        }
                        else
                        {
                            objtoken.access_token = objMySqlDataReader["token"].ToString();
                            objtoken.status = true;
                        }
                    }
                    else
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["GraphLoginURL"].ToString());
                        var request = new RestRequest("/" + objMdlGraph.tenantID + "/oauth2/v2.0/token", Method.POST);
                        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        request.AddParameter("client_id", objMdlGraph.clientID);
                        request.AddParameter("scope", ConfigurationManager.AppSettings["GraphLoginScope"].ToString());
                        request.AddParameter("client_secret", objMdlGraph.client_secret);
                        request.AddParameter("grant_type", "client_credentials");
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            graphLoginSuccessResponse objgraphLoginSuccessResponse = new graphLoginSuccessResponse();
                            objgraphLoginSuccessResponse = JsonConvert.DeserializeObject<graphLoginSuccessResponse>(response.Content);
                            objtoken.access_token = objgraphLoginSuccessResponse.access_token;
                            objtoken.status = true;
                            msSQL = "insert into crm_smm_tgraphtoken(token,expiry_time)values(" +
                                    "'" + objtoken.access_token + "'," +
                                    "'" + DateTime.Now.AddSeconds(3595).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("Error occurred while insert: " + msSQL);
                            }
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("Error while generating access token: " + response.Content.ToString());
                        }
                    }
                    objMySqlDataReader.Close();
                }
                else
                {
                    objtoken.message = "Kindly add the app details for sending mails!";
                }

            }
            catch (Exception ex)
            {
                objtoken.message = ex.Message;
                objcmnfunctions.LogForAudit("Exception while generating access token: " + ex.ToString());
            }
            return objtoken;
        }


        public void DaPostMail(HttpRequest httpRequest, string user_gid, result objResult)
        {
            {
                string lscompany_code = "";
                msSQL = " select company_code from adm_mst_tcompany";
                string lscompany_codecheck = objdbconn.GetExecuteScalar(msSQL);

                if (lscompany_codecheck == "BOBA" || lscompany_codecheck == "MEDIA")
                {



                    string final_path = string.Empty;
                    string finalEmailBody = string.Empty;
                    string msdocument_gid = string.Empty;

                    msSQL = " select company_code from adm_mst_tcompany";
                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                    gmailconfiguration getgmailcredentials = gmailcrendentials();

                    var options = new RestClient("https://accounts.google.com");
                    var request = new RestRequest("/o/oauth2/token", Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Cookie", "__Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; __Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; NID=511=Kbcaybba2NN2meR6QaA1TfflTMD5X6JNa-vmRpwiHUMRZn4ru7MVloJIso4PHGVH40ORQYUKH1LJvUttyG79Vi5eCXIX4UgloYTQFHN0qYHR67sn7fC32LHA7Cfdbgz4G6g5FhOnXus0SVeSyNx4QPRGFQPakVRKwNlBIxK8FGc; __Host-GAPS=1:7IaKbeoNAD6DkBpPrg7Pl6ppMRE7yQ:a2G_qIglM2A-euWd");
                    var body = @"{" + "\n" +
                    @"    ""client_id"": " + "\"" + getgmailcredentials.client_id + "\"" + "," + "\n" +

                    @"    ""client_secret"":  " + "\"" + getgmailcredentials.client_secret + "\"" + "," + "\n" +
                    @"    ""grant_type"": ""refresh_token"",
                          " + "\n" +
                    @"    ""refresh_token"": " + "\"" + getgmailcredentials.refresh_token + "" + "\"" + "\n" + @"}";

                    request.AddParameter("application/json", body, RestSharp.ParameterType.RequestBody);
                    IRestResponse response = options.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    refreshtokenlist objMdlGmailCampaignResponse = new refreshtokenlist();
                    objMdlGmailCampaignResponse = JsonConvert.DeserializeObject<refreshtokenlist>(errornetsuiteJSON);


                    // attachment get function

                    HttpFileCollection httpFileCollection;
                    string basecode = httpRequest.Form["gmailfiles"];
                    //List<sendmail_list> hrDocuments = JsonConvert.DeserializeObject<List<sendmail_list>>(basecode);



                    //split function

                    string employee_emailid = httpRequest.Form["employee_emailid"];
                    string sub = httpRequest.Form["sub"];
                    string to = httpRequest.Form["to"];
                    string[] to_mails = to.Split(',');
                    string bodies = httpRequest.Form["body"];
                    string cc = httpRequest.Form["cc"];
                    string[] cc_mails = cc.Split(',');
                    string bcc = httpRequest.Form["bcc"];
                    string[] bcc_mails = bcc.Split(',');
                    string invoice_gid = httpRequest.Form["invoice_gid"];
                    //string bcc = httpRequest.Form["bcc"];
                    //string cc = httpRequest.Form["cc"];


                    List<DbAttachmentPath> dbattachmentpath = new List<DbAttachmentPath>();
                    List<MailAttachmentbase64> mailAttachmentbase64 = new List<MailAttachmentbase64>();
                    string lsfilepath = string.Empty;
                    string document_gid = string.Empty;
                    string lspath, lspath1;
                    string FileExtension = string.Empty;
                    string file_name = string.Empty;
                    string httpsUrl = string.Empty;

                    HttpPostedFile httpPostedFile;

                    if (httpRequest.Files.Count > 0)
                    {
                        string lsfirstdocument_filepath = string.Empty;
                        httpFileCollection = httpRequest.Files;
                        for (int i = 0; i < httpFileCollection.Count; i++)
                        {
                            MemoryStream ms = new MemoryStream();
                            //var getdocumentdtl = hrDocuments.Where(a => a.AutoID_Key == httpFileCollection.AllKeys[i]).FirstOrDefault();
                            msdocument_gid = objcmnfunctions.GetMasterGID("GILC");
                            httpPostedFile = httpFileCollection[i];
                            file_name = httpPostedFile.FileName;
                            string type = httpPostedFile.ContentType;
                            string lsfile_gid = msdocument_gid;
                            string lscompany_document_flag = string.Empty;
                            FileExtension = Path.GetExtension(file_name).ToLower();
                            lsfile_gid = lsfile_gid + FileExtension;
                            Stream ls_readStream;
                            ls_readStream = httpPostedFile.InputStream;
                            ls_readStream.CopyTo(ms);
                            string base64String = string.Empty;
                            bool status1;


                            status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Mail/Post/Purchase/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                            final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                   '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                   '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];

                            byte[] fileBytes = ms.ToArray();
                            base64String = Convert.ToBase64String(fileBytes);

                            ms.Close();

                            dbattachmentpath.Add(new DbAttachmentPath
                            { path = httpsUrl }
                            );
                            mailAttachmentbase64.Add(new MailAttachmentbase64
                            {
                                name = httpPostedFile.FileName,
                                type = type,
                                data = base64String

                            });
                        }
                        StringBuilder emailBody = new StringBuilder();
                        string boundary = "--boundary_example";
                        foreach (var attachment in mailAttachmentbase64)
                        {
                            emailBody.AppendLine("Content-Type: " + attachment.type + "; charset=UTF-8");
                            emailBody.AppendLine("Content-Transfer-Encoding: base64");
                            emailBody.AppendLine("Content-Disposition: attachment; filename=\"" + attachment.name + "\"");
                            emailBody.AppendLine();
                            emailBody.AppendLine(attachment.data);
                            emailBody.AppendLine(boundary);
                        }
                        var subjectEncoded = "=?UTF-8?B?" + Convert.ToBase64String(Encoding.UTF8.GetBytes(sub)) + "?=";
                        finalEmailBody = emailBody.ToString();
                        string cc_emailString = String.Join(", ", cc_mails);
                        string to_emailString = String.Join(", ", to_mails);
                        string bcc_emailString = String.Join(", ", bcc_mails);
                        var options1 = new RestClient("https://www.googleapis.com");
                        var request1 = new RestRequest("/upload/gmail/v1/users/me/messages/send?uploadType=media", Method.POST);
                        request1.AddHeader("Authorization", "Bearer  " + objMdlGmailCampaignResponse.access_token + "");
                        request1.AddHeader("Content-Type", "message/rfc822");
                        request1.AddHeader("Cookie", "COMPASS=gmail-api-uploads-blobstore=CgAQ18HPrwYagAEACWuJVwxF5peESk5gbz5AE37T8tg_Yoh-YDKTZilInpK22DHDg7FuuU3LXoN11GAOnyqdLslKu6I5ePsGXCEsd3xSS2yUEWvqsZJtNX4R-ajkWB37okK3XRlg3MQM0P22BdEB5efrYEFEwlQWnrxQUPmucfMFffcwQAMnVi0yfzAB; COMPASS=gmail-api-uploads-blobstore=CgAQnrbWrwYagAEACWuJVwxF5peESk5gbz5AE37T8tg_Yoh-YDKTZilInpK22DHDg7FuuU3LXoN11GAOnyqdLslKu6I5ePsGXCEsd3xSS2yUEWvqsZJtNX4R-ajkWB37okK3XRlg3MQM0P22BdEB5efrYEFEwlQWnrxQUPmucfMFffcwQAMnVi0yfzAB; COMPASS=gmail-api-uploads-blobstore=CgAQ3M3WrwYagAEACWuJV70L2ArobrIhJ3QHHVMUMuhVzIt7hY_BOzuIcJ9f8aTM0lWNTsBGq8iRVZqbbVDXK1zOu9pSPMnm5hcrkX1dIku9gne04K4azeD3LO9TlrMLOaKbRMBzaLZZEsjzHG9ogDw5OoF3IB-_eL6aX22cxCxfAiuXIJU9MPFqsDAB");
                        var body1 = @"From:" + getgmailcredentials.gmail_address + "" + "\n" +
                        @"To:" + to_emailString + "" + "\n" +
                         @"Cc:" + cc_emailString + "" + "\n" +
                         @"Bcc:" + bcc_emailString + "" + "\n" +
                        @"Subject: " + subjectEncoded + "" + "\n" +
                        @"MIME-Version: 1.0" + "\n" +
                        @"Content-Type: multipart/mixed; boundary=""boundary_example""" + "\n" +
                        @"" + "\n" +
                        @"--boundary_example" + "\n" +
                        @"Content-Type: text/html; charset=""UTF-8""" + "\n" +
                        @"MIME-Version: 1.0" + "\n" +
                        @"" + "\n" +
                        @"<html>" + "\n" +
                        @"  <body>" + "\n" +
                        @"    <p>" + bodies + "</p>" + "\n" +
                        @"    <p>" + getgmailcredentials.default_template + "</p>" + "\n" +
                        @"  </body>" + "\n" +
                        @"</html>" + "\n" +
                        @"" + "\n" +
                        @"--boundary_example" + "\n" +
                        finalEmailBody +
                        "\n";
                        request1.AddParameter("message/rfc822", body1, RestSharp.ParameterType.RequestBody);
                        IRestResponse response1 = options1.Execute(request1);
                        string errornetsuiteJSON1 = response1.Content;
                        responselist objMdlGmailCampaignResponse1 = new responselist();
                        objMdlGmailCampaignResponse1 = JsonConvert.DeserializeObject<responselist>(errornetsuiteJSON);
                        if (response1.StatusCode == HttpStatusCode.OK)
                        {
                            msSQL = "INSERT INTO crm_trn_gmail (" +
                                  "gmail_gid, " +
                                "from_mailaddress, " +
                                "to_mailaddress, " +
                                "mail_subject, " +
                                "mail_body, " +
                                "transmission_id, " +
                                "leadbank_gid, " +
                                 " created_by, " +
                                "created_date) " +
                                "VALUES (" +
                                 "'" + msdocument_gid + "', " +
                                "'" + getgmailcredentials.gmail_address + "', " +
                                "'" + to + "', " +
                                "'" + sub.Replace("'", "\\\'") + "', " +
                                "'" + bodies.Replace("'", "\\\'") + "', " +
                                "'" + objMdlGmailCampaignResponse1.id + "', " +
                                "'" + invoice_gid + "', " +
                                  "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            for (int i = 0; i < dbattachmentpath.Count; i++)
                            {
                                msGetGid1 = objcmnfunctions.GetMasterGID("UPLF");
                                msSQL = "INSERT INTO crm_trn_tfiles (" +
                                 "file_gid, " +
                                "mailmanagement_gid, " +
                                "document_name, " +
                              "document_path, " +
                                "created_date) " +
                                "VALUES (" +
                                 "'" + msGetGid1 + "', " +
                                "'" + msdocument_gid + "', " +
                                "'" + mailAttachmentbase64[i].name + "', " +
                                 "'" + dbattachmentpath[i].path + "', " +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            for (int i = 0; i < dbattachmentpath.Count; i++)
                            {
                                string encodedBody = EncodeToBase64(bodies);
                                string encodedsub = EncodeToBase64(sub);


                                msSQL = "INSERT INTO smr_trn_gmailattachment (" +
                                      "gmail_gid, " +
                                    "from_mailaddress, " +
                                    "to_mailaddress, " +
                                    "mail_subject, " +
                                    "mail_body, " +
                                    "transmission_id, " +
                                     "document_name, " +
                                  "document_path, " +
                                  "file_gid, " +
                                     " created_by, " +
                                    "created_date) " +
                                    "VALUES (" +
                                     "'" + msGetGid1 + "', " +
                                    "'" + employee_emailid + "', " +
                                    "'" + to + "', " +
                                    "'" + encodedsub + "', " +
                                    "'" + encodedBody + "', " +
                                    "'" + objMdlGmailCampaignResponse1.id + "', " +
                                     "'" + mailAttachmentbase64[i].name + "', " +
                                     "'" + dbattachmentpath[i].path + "', " +
                                      "'" + invoice_gid + "', " +
                                      "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }
                }
                else
                {
                    // attachment get function


                    graphtoken objtoken = new graphtoken();



                    HttpFileCollection httpFileCollection;
                    string basecode = httpRequest.Form["gmailfiles"];
                    //List<sendmail_list> hrDocuments = JsonConvert.DeserializeObject<List<sendmail_list>>(basecode);



                    //split function


                    //string bcc = httpRequest.Form["bcc"];
                    //string cc = httpRequest.Form["cc"];


                    List<DbAttachmentPath> dbattachmentpath = new List<DbAttachmentPath>();
                    List<MailAttachmentbase64> mailAttachmentbase64 = new List<MailAttachmentbase64>();
                    string lsfilepath = string.Empty;
                    string document_gid = string.Empty;
                    string lspath, lspath1;
                    string FileExtension = string.Empty;
                    string file_name = string.Empty;
                    string httpsUrl = string.Empty;

                    HttpPostedFile httpPostedFile;

                    if (httpRequest.Files.Count > 0)
                    {
                        string lsfirstdocument_filepath = string.Empty;
                        httpFileCollection = httpRequest.Files;
                        for (int i = 0; i < httpFileCollection.Count; i++)
                        {
                            MemoryStream ms = new MemoryStream();
                            //var getdocumentdtl = hrDocuments.Where(a => a.AutoID_Key == httpFileCollection.AllKeys[i]).FirstOrDefault();

                            string msGet_att_Gid = objcmnfunctions.GetMasterGID("BEAC");
                            httpPostedFile = httpFileCollection[i];
                            file_name = httpPostedFile.FileName;
                            string type = httpPostedFile.ContentType;
                            string lsfile_gid = msGet_att_Gid;
                            string lscompany_document_flag = string.Empty;
                            FileExtension = Path.GetExtension(file_name).ToLower();
                            lsfile_gid = lsfile_gid + FileExtension;
                            Stream ls_readStream;
                            ls_readStream = httpPostedFile.InputStream;
                            ls_readStream.CopyTo(ms);
                            string base64String = string.Empty;
                            bool status1;
                            string final_path;


                            status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Mail/Post/Purchase/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msGet_att_Gid + FileExtension, FileExtension, ms);
                            final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msGet_att_Gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                   '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                   '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];

                            byte[] fileBytes = ms.ToArray();
                            base64String = Convert.ToBase64String(fileBytes);



                            dbattachmentpath.Add(new DbAttachmentPath
                            { path = httpsUrl }
                            );
                            mailAttachmentbase64.Add(new MailAttachmentbase64
                            {
                                name = httpPostedFile.FileName,
                                type = type,
                                data = base64String

                            });
                            ms.Close();
                            objtoken = generateGraphAccesstoken();
                        }

                        if (objtoken.status)
                        {
                            string employee_emailid = httpRequest.Form["employee_emailid"];
                            string sub = httpRequest.Form["sub"];

                            string bodies = httpRequest.Form["body"];


                            string invoice_gid = httpRequest.Form["invoice_gid"];
                            string cc_mail = httpRequest.Form["cc"];
                            string to_mail = httpRequest.Form["to"];
                            string bcc = httpRequest.Form["bcc"];

                            MdlGraphMailContent objMdlGraphMailContent = new MdlGraphMailContent();
                            objMdlGraphMailContent.message = new Message1();
                            objMdlGraphMailContent.saveToSentItems = true;
                            objMdlGraphMailContent.message.body = new Body2();
                            objMdlGraphMailContent.message.body.contentType = "HTML";
                            objMdlGraphMailContent.message.body.content = bodies;
                            objMdlGraphMailContent.message.subject = sub;
                            string[] to_mails = to_mail.Split(',');
                            string[] cc_mails = null;
                            string[] bcc_mails = null;

                            if (cc_mail != null && cc_mail != "")
                            {
                                cc_mails = cc_mail.Split(',');
                            }
                            if (bcc != null && bcc != "")
                            {
                                bcc_mails = bcc.Split(',');
                            }

                            //string[] files = lspath2.Split(',');

                            if (to_mails != null)
                            {
                                objMdlGraphMailContent.message.toRecipients = new Torecipient[to_mails.Length];
                                for (int i = 0; i < objMdlGraphMailContent.message.toRecipients.Length; i++)
                                {
                                    objMdlGraphMailContent.message.toRecipients[i] = new Torecipient();
                                    objMdlGraphMailContent.message.toRecipients[i].emailAddress = new Emailaddress();
                                    objMdlGraphMailContent.message.toRecipients[i].emailAddress.address = to_mails[i];
                                }
                            }

                            if (cc_mails != null && cc_mails.Length != 0)
                            {
                                objMdlGraphMailContent.message.ccRecipients = new Torecipient[cc_mails.Length];
                                for (int i = 0; i < objMdlGraphMailContent.message.ccRecipients.Length; i++)
                                {
                                    objMdlGraphMailContent.message.ccRecipients[i] = new Torecipient();
                                    objMdlGraphMailContent.message.ccRecipients[i].emailAddress = new Emailaddress();
                                    objMdlGraphMailContent.message.ccRecipients[i].emailAddress.address = cc_mails[i];
                                }
                            }
                            if (bcc_mails != null && bcc_mails.Length != 0)
                            {
                                objMdlGraphMailContent.message.bccRecipients = new Torecipient[bcc_mails.Length];
                                for (int i = 0; i < objMdlGraphMailContent.message.bccRecipients.Length; i++)
                                {
                                    objMdlGraphMailContent.message.bccRecipients[i] = new Torecipient();
                                    objMdlGraphMailContent.message.bccRecipients[i].emailAddress = new Emailaddress();
                                    objMdlGraphMailContent.message.bccRecipients[i].emailAddress.address = bcc_mails[i];
                                }
                            }

                            if (httpFileCollection.Count != null && httpFileCollection.Count != 0)
                            {
                                objMdlGraphMailContent.message.attachments = new attachments[mailAttachmentbase64.Count];
                                for (int i = 0; i < mailAttachmentbase64.Count; i++)
                                {
                                    objMdlGraphMailContent.message.attachments[i] = new attachments();
                                    objMdlGraphMailContent.message.attachments[i].name = mailAttachmentbase64[i].name;
                                    objMdlGraphMailContent.message.attachments[i].contentBytes = mailAttachmentbase64[i].data;
                                    objMdlGraphMailContent.message.attachments[i].OdataType = "#microsoft.graph.fileAttachment";
                                }
                            }

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["GraphSendURL"].ToString());
                            var request = new RestRequest("/v1.0/users/" + employee_emailid + "/sendMail", Method.POST);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("Authorization", objtoken.access_token);
                            string request_body = JsonConvert.SerializeObject(objMdlGraphMailContent);
                            request.AddParameter("application/json", request_body, RestSharp.ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.Accepted)
                            {
                                string encodedBody = EncodeToBase64(bodies);
                                string encodedsub = EncodeToBase64(sub);
                                for (int i = 0; i < dbattachmentpath.Count; i++)
                                {
                                    msGetGid1 = objcmnfunctions.GetMasterGID("UPLF");

                                    msSQL = "INSERT INTO smr_trn_gmailattachment (" +
                                          "gmail_gid, " +
                                        "from_mailaddress, " +
                                        "to_mailaddress, " +
                                        "mail_subject, " +
                                        "mail_body, " +
                                         "document_name, " +
                                      "document_path, " +
                                      "file_gid, " +
                                         " created_by, " +
                                        "created_date) " +
                                        "VALUES (" +
                                         "'" + msGetGid1 + "', " +
                                        "'" + employee_emailid + "', " +
                                        "'" + to_mail + "', " +
                                        "'" + encodedsub + "', " +
                                        "'" + encodedBody + "', " +
                                         "'" + mailAttachmentbase64[i].name + "', " +
                                         "'" + dbattachmentpath[i].path + "', " +
                                          "'" + invoice_gid + "', " +
                                          "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }

                            }
                            else
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + response.Content.ToString() + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                            }
                        }

                    }
                }
                if (mnResult == 1)
                {
                    objResult.status = true;
                    objResult.message = "Mail Sent Successfully !!";
                }
                else
                {
                    objResult.status = false;
                    objResult.message = " Mail Not Sent !! ";
                }
            }
        }
        private static string EncodeToBase64(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        public void Daviewinvoice(string invoice_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = " select a.invoice_refno,date_format(a.invoice_date, '%d-%m-%Y') as invoice_date,a.customer_name,a.customer_address,a.shipping_to," +
                        " a.customer_contactnumber,a.customer_email,a.branch_gid,a.currency_code,a.exchange_rate," +
                        " a.payment_term as payment_days,a.delivery_days,a.mode_of_despatch,b.branch_name,c.gst_number," +
                        " format(a.total_amount,2) as total_amount, format(a.additionalcharges_amount,2)as additionalcharges_amount,a.termsandconditions," +
                        " format(a.discount_amount_L,2)as discount_amount_L,format(a.freight_charges,2)as freight_charges," +
                        " a.tax_name,format(a.tax_amount,2) as tax_amount,a.bill_email,format(a.roundoff,2) as roundoff," +
                        " format(a.invoice_amount,2) as invoice_amount,date_format(a.payment_date, '%d-%m-%Y') as payment_date,a.sales_type,y.salestype_name  from rbl_trn_tinvoice a " +
                        " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid " +
                        " left join crm_mst_tcustomer c on c.customer_gid = a.customer_gid " +
                        " left join smr_trn_tsalestype y on y.salestype_gid = a.sales_type " +
                        " where a.invoice_gid = '" + invoice_gid + "'  group by invoice_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Salesviewinvoice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Salesviewinvoice_list
                        {
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            shipping_to = dt["shipping_to"].ToString(),
                            customer_contactnumber = dt["customer_contactnumber"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            mode_of_despatch = dt["mode_of_despatch"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            discount_amount_L = dt["discount_amount_L"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            bill_email = dt["bill_email"].ToString(),
                            salestype_name = dt["salestype_name"].ToString(),

                        });
                        values.Salesviewinvoice_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Invoice View!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void Daviewinvoiceproduct(string invoice_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {
                msSQL = "  SELECT  a.invoicedtl_gid, a.product_gid, a.product_name, a.product_remarks,a.product_code, b.productgroup_gid, " +
                         "  FORMAT(a.qty_invoice, 2) AS qty_ordered, c.productgroup_name, a.uom_name, b.producttype_gid, " +
                         "  a.created_by, a.product_price, FORMAT(a.product_price, 2) AS producttotal_price, " +
                         "  a.discount_percentage,format(a.product_total,2) as product_total, FORMAT(a.discount_amount, 2) AS discount_amount,format(a.vendor_price,2) as vendor_price, " +
                         "  a.tax_percentage, FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field,  " +
                         "  a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2,a.tax_percentage3, " +
                         "  FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3, a.tax1_gid, " +
                         "  a.tax2_gid, a.tax3_gid , " +
                         "  concat( case when a.tax_name is null then '' else a.tax_name end, ' ', " +
                         "  case when a.tax_percentage = '0' then '' else concat('', a.tax_percentage, '%') end , " +
                         "  case when a.tax_amount = '0' then '' else concat(':', a.tax_amount) end) as tax, " +
                         "  concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ',  " +
                         "  case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end,  " +
                         "  case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2, " +
                         "  concat(case when a.tax_name3 is null then '' else a.tax_name3 end, ' ', " +
                         "  case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                         "  end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3, " +
                         "  format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.taxsegment_gid, " +
                         "  e.tax_prefix as tax_prefix1,f.tax_prefix  as tax_prefix2 " +
                         "  FROM rbl_trn_tinvoicedtl a" +
                         "  LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                         "  left join acp_mst_ttax e on e.tax_gid = a.tax1_gid " +
                         "  left join acp_mst_ttax f on f.tax_gid = a.tax2_gid" +
                         "  LEFT JOIN pmr_mst_tproductgroup c ON c.productgroup_gid = a.productgroup_gid " +
                         "  LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid  where a.invoice_gid ='" + invoice_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<directinvoiceproductsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {


                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        double lsprice = Math.Round((double.Parse(dt["vendor_price"].ToString()) * double.Parse(dt["qty_ordered"].ToString()) - double.Parse(dt["discount_amount"].ToString())),2);

                        getModuleList.Add(new directinvoiceproductsummary_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            qty = dt["qty_ordered"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            taxname1 = dt["tax_name"].ToString(),
                            taxname2 = dt["tax_name2"].ToString(),
                            taxname3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tax = dt["tax"].ToString(),
                            tax2 = dt["tax2"].ToString(),
                            tax3 = dt["tax3"].ToString(),
                            taxamount = dt["taxamount"].ToString(),
                            vendor_price =double.Parse( dt["vendor_price"].ToString()),
                            product_remarks = dt["product_remarks"].ToString(),
                            tax_prefix1 = dt["tax_prefix1"].ToString(),
                            tax_prefix2 = dt["tax_prefix2"].ToString(),
                            price=lsprice

                        });
                        values.directinvoiceproductsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Invoice View!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetSalesType(MdlSmrRptInvoiceReport values)
        {
            try
            {



                msSQL = " Select a.salestype_gid,a.salestype_code,a.salestype_name,b.account_name,b.account_gid " +
                    " from smr_trn_tsalestype a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " order by salestype_name asc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getsalestype>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getsalestype

                        {
                            salestype_gid = dt["salestype_gid"].ToString(),
                            salestype_code = dt["salestype_code"].ToString(),
                            salestype_name = dt["salestype_name"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString()

                        });
                        values.Getsalestype = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Type dropdown  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }



        //////-------------------------------------------new design asset-sm-------------------------------------------------//////
        public void DaSaleinvoiceSummary(MdlSmrRptInvoiceReport values)
        {
            try
            {
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string lscurrency = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = " select distinct a.invoice_gid,s.salesorder_status, case when a.invoice_reference like '%AREF%' then j.agreement_referencenumber else " +
                //" cast(concat(s.so_referenceno1,if(s.so_referencenumber<>'',concat(' ',' | ',' ',s.so_referencenumber),'') ) as char)  end as so_referencenumber," +
                //" a.invoice_refno," +
                //"concat(a.invoice_status,',',a.invoice_flag,',',s.salesorder_status,',',case when a.irn is not null then 'IRN Generated' else 'IRN Not Yet Generated' end,' '," +
                //"case when a.irncancel_date is not null then 'IRN Cancelled' else ' ' end,' ',case when a.creditnote_status = 'Y' then 'Credit Noted' else '' end) as status," +
                //" a.mail_status,a.customer_gid,a.invoice_date,a.invoice_reference,a.additionalcharges_amount,a.discount_amount, " +
                //" CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', a.mail_status," +
                //" a.payment_flag,  format(a.initialinvoice_amount,2) as initialinvoice_amount,s.salesorder_gid,a.invoice_status,a.invoice_flag, " +
                //" case when a.irn is not null then 'IRN Generated' else 'IRN Pending' end as 'irn_status',case when a.irncancel_date is not null then 'IRN Cancelled' else '' end as 'irncancel_status'," +
                //" format(a.invoice_amount,2) as invoice_amount,c.customer_code," +
                //" case when a.customer_contactnumber is null then concat(a.customer_contactperson,' / ',a.customer_contactnumber) " +
                //" else  concat(a.customer_contactperson, if(a.customer_email='',' ',concat(' / ',a.customer_email))) end as customer_contactperson, " +
                //" case when a.currency_code = '" + lscurrency + "' then concat(c.customer_code ,' / ',a.customer_name)  when a.currency_code is null then concat(c.customer_code,' / ', a.customer_name) " +
                //" when a.currency_code is not null and a.currency_code <> '" + lscurrency + "' then concat(c.customer_code ,' / ',a.customer_name,' / ',h.country) end as customer_name,a.currency_code, " +
                //" a.customer_contactnumber  as mobile,a.invoice_from,i.directorder_gid,a.progressive_invoice " +
                //" from rbl_trn_tinvoice a " +
                //" left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                //" left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid " +
                //" left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                //" left join smr_trn_tsalesorder s on a.invoice_reference = s.salesorder_gid " +
                //" left join crm_trn_tagreement j on j.agreement_gid = a.invoice_reference " +
                //" left join smr_trn_tdeliveryorder i on s.salesorder_gid=i.salesorder_gid " +
                //" where a.invoice_type<>'Opening Invoice' order by date(a.invoice_date) desc,a.invoice_date asc, a.invoice_gid desc ";

                msSQL = "call smr_trn_invoicesummary('" + lscurrency + "')";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salesinvoicesummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    var smrinvoicesummary_listdict = new Dictionary<string, salesinvoicesummary_list>();
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string invoice_gid = dt["invoice_gid"].ToString();


                        if (!smrinvoicesummary_listdict.ContainsKey(invoice_gid))
                        {
                            smrinvoicesummary_listdict[invoice_gid] = new salesinvoicesummary_list
                            {
                                invoice_gid = invoice_gid,
                                invoice_date = Convert.ToDateTime(dt["invoice_date"].ToString()),
                                invoice_refno = dt["invoice_refno"].ToString(),
                                customer_name = dt["customer_name"].ToString(),
                                customer_contactperson = dt["customer_contactperson"].ToString(),
                                invoice_reference = dt["invoice_reference"].ToString(),
                                invoice_from = dt["invoice_from"].ToString(),
                                invoice_amount = dt["invoice_amount"].ToString(),
                                salesorder_status = dt["salesorder_status"].ToString(),
                                so_referencenumber = dt["so_referencenumber"].ToString(),
                                status = dt["status"].ToString(),
                                mail_status = dt["mail_status"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                                additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                                discount_amount = dt["discount_amount"].ToString(),
                                payment_flag = dt["payment_flag"].ToString(),
                                initialinvoice_amount = dt["initialinvoice_amount"].ToString(),
                                salesorder_gid = dt["salesorder_gid"].ToString(),
                                invoice_status = dt["invoice_status"].ToString(),
                                currency_code = dt["currency_code"].ToString(),
                                mobile = dt["mobile"].ToString(),
                                progressive_invoice = dt["progressive_invoice"].ToString(),
                                directorder_gid = dt["directorder_gid"].ToString(),
                                customer_code = dt["customer_code"].ToString(),
                                einvoice_flag = dt["einvoice_flag"].ToString(),
                                irn= dt["irn"].ToString()

                            };
                        };
                    }
                    getModuleList = smrinvoicesummary_listdict.Values.ToList();
                    values.salesinvoicesummary_list = getModuleList;
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Invoice Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaProductSummary(string employee_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;
                msSQL = "SELECT  a.invoicedtl_gid,a.invoice_gid, a.product_gid, a.product_name, a.product_remarks,a.product_code, b.productgroup_gid," +
                    " FORMAT(a.qty_invoice, 2) AS qty_ordered, c.productgroup_name, a.uom_name, b.producttype_gid," +
                    " a.created_by, a.product_price, FORMAT(a.product_price, 2) AS producttotal_price," +
                    " a.discount_percentage,a.product_total, FORMAT(a.discount_amount, 2) AS discount_amount," +
                    " a.tax_percentage, FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field," +
                    " a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2,a.tax_percentage3," +
                    " FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3, a.tax1_gid," +
                    " a.tax2_gid, a.tax3_gid ," +
                    " concat( case when a.tax_name is null then '' else a.tax_name end, ' '," +
                    " case when a.tax_percentage = '0' then '' else concat('',a.tax_percentage,'%') end ," +
                    " case when a.tax_amount = '0' then '' else concat(':',a.tax_amount) end) as tax," +
                    " concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ', " +
                    " case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end, " +
                    " case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2," +
                    " concat(case when a.tax_name3 is null then '' else a.tax_name3 end, ' '," +
                    " case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                    " end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3," +
                    "format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.taxsegment_gid " +
                    " FROM rbl_tmp_tinvoicedtl a  LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                    " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid  " +
                    " WHERE a.employee_gid = '" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<directinvoiceproductsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["product_total"].ToString());
                        grandtotal += double.Parse(dt["product_total"].ToString());
                        getModuleList.Add(new directinvoiceproductsummary_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            qty = dt["qty_ordered"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            taxname1 = dt["tax_name"].ToString(),
                            taxname2 = dt["tax_name2"].ToString(),
                            taxname3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tax = dt["tax"].ToString(),
                            tax2 = dt["tax2"].ToString(),
                            tax3 = dt["tax3"].ToString(),
                            taxamount = dt["taxamount"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                        });
                    }
                    values.directinvoiceproductsummary_list = getModuleList;
                }

                dt_datatable.Dispose();
                values.grand_total = Math.Round(grand_total, 2);
                values.grandtotal = Math.Round(grandtotal, 2);
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting product summary!";
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostproductDirectinvoice(string employee_gid, directinvoiceproductsubmit_list values)
        {
            try
            {
                double discount_precentage = double.TryParse(values.discountprecentage, out double discountprecentage) ? discountprecentage : 0;
                string lstax1, lstax2;
                double lsGrandTotal = (values.productquantity * values.unitprice) - values.discount_amount;



                msSQL = "select productgroup_name from pmr_mst_tproductgroup where productgroup_gid = '" + values.productgroup_gid + "'";
                string lsproductgroupname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT a.productuom_gid, a.product_gid,a.customerproduct_code, a.product_name, b.productuom_name" +
                    " ,a.productgroup_gid FROM pmr_mst_tproduct a " +
                 " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                 " WHERE a.product_gid = '" + values.product_name.Replace("'", "\\\'") + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lsproductgid = objMySqlDataReader["product_gid"].ToString();
                    lsproductuom_gid = objMySqlDataReader["productuom_gid"].ToString();
                    lsproduct_name = objMySqlDataReader["product_name"].ToString();
                    lsproductuom_name = objMySqlDataReader["productuom_name"].ToString();
                    lscustomerproduct_code = objMySqlDataReader["customerproduct_code"].ToString();

                    objMySqlDataReader.Close();
                }
                if (values.taxname1 == "" || values.taxname1 == null)
                {
                    lstax1 = "0";
                }
                else
                {
                    msSQL = "select tax_prefix from acp_mst_ttax  where tax_gid='" + values.taxgid1 + "'";
                    lstax1 = objdbconn.GetExecuteScalar(msSQL);
                }

                if (values.taxname2 == "" || values.taxname2 == null)
                {
                    lstax2 = "0";
                }
                else
                {
                    msSQL = "select tax_prefix from acp_mst_ttax where tax_gid='" + values.taxgid2 + "'";
                    lstax2 = objdbconn.GetExecuteScalar(msSQL);
                }
                string invoicedtl_gid = objcmnfunctions.GetMasterGID("SIVC");
                string invoice_gid = objcmnfunctions.GetMasterGID("SIVC");
                msGetGid = objcmnfunctions.GetMasterGID("SIVC");
                
                 if (values.unitprice == null || Convert.ToString(values.unitprice) == "" || Convert.ToString(values.unitprice) == "undefined")
                {
                    values.status = false;
                    values.message = "Price cannot be left empty";
                    return;
                }
                else
                {
                    msSQL = " insert into rbl_tmp_tinvoicedtl ( " +
                        " invoicedtl_gid, invoice_gid, product_gid," +
                        " qty_invoice,product_price, discount_percentage," +
                        " discount_amount, tax_amount, product_total," +
                        " uom_gid, uom_name, tax_amount2, tax_amount3," +
                        " tax_name, tax_name2,tax_name3,display_field," +
                        " tax1_gid, tax2_gid, tax3_gid, product_name," +
                        " productgroup_gid," +
                        " productgroup_name, " +
                        " employee_gid, " +
                        " selling_price, " +
                        " product_code," +
                        " customerproduct_code, " +
                        " tax_percentage," +
                        " tax_percentage2," +
                        " tax_percentage3 ," +
                        " vendor_price, " +
                        " product_remarks " +
                        " ) values ( " +
                        "'" + invoicedtl_gid + "'," +
                        "'" + invoice_gid + "'," +
                        "'" + lsproductgid + "'," +
                        "'" + values.productquantity + "'," +
                        "'" + values.unitprice + "'," +
                        "'" + discount_precentage + "'," +
                        "'" + values.discount_amount + "'," +
                        "'" + values.taxamount1 + "'," +
                        "'" + values.producttotal_amount + "'," +
                        "'" + lsproductuom_gid + "'," +
                        "'" + lsproductuom_name.Replace("'", "\\\'") + "'," +
                        "'" + values.taxamount2 + "'," +
                        "'" + values.taxamount3 + "'," +
                        "'" + lstax1 + "'," +
                        "'" + lstax2 + "'," +
                        "'" + values.tax_prefix3 + "',";
                    if(values.product_desc != null)
                    {
                        msSQL += "'" + values.product_desc.Replace("'","\\\'") + "',";
                    }
                    else
                    {
                      msSQL +=  "'" + values.product_desc + "',";
                    }

                    msSQL +=
                      "'" + values.taxgid1 + "'," +
                      "'" + values.taxgid2 + "'," +
                      "'" + values.taxgid3 + "'," +
                      "'" + lsproduct_name.Replace("'", "\\\'") + "'," +
                      "'" + values.productgroup_gid + "'," +
                      "'" + lsproductgroupname.Replace("'", "\\\'") + "'," +
                      "'" + employee_gid + "'," +
                      "'" + lsGrandTotal + "'," +
                      "'" + values.product_code + "'," +
                      "'" + lscustomerproduct_code + "'," +
                      "'" + values.taxprecentage1 + "'," +
                      "'" + values.taxprecentage2 + "'," +
                      "'" + values.taxprecentage3 + "'," +
                      "'" + values.unitprice + "',";
                       if (values.product_desc != null)
                    {
                        msSQL += "'" + values.product_desc.Replace("'", "\\\'") + "')";
                    }
                    else
                    {
                        msSQL += "'" + values.product_desc + "')";
                    }

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Product";
                    }
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void Dachangeinvoicerefno(invoicerefno_list values)
        {
            msSQL = "select * from rbl_trn_tinvoice where invoice_refno ='" + values.new_invoicerefno + "' ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows)
            {
                values.status = false;
                values.message = "Invoice Reference No already Exist";
                objMySqlDataReader.Close();
                return;
            }
            else
            {
                msSQL = "update rbl_trn_tinvoice set " +
                    " invoice_refno='" + values.new_invoicerefno + "' where invoice_gid = '" + values.invoice_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = "Select finance_flag from adm_mst_Tcompany";
                    string finance_flag=objdbconn.GetExecuteScalar(msSQL);
                    if (finance_flag == "Y") { 
                    msSQL = "update acc_trn_journalentry set " +
                           " journal_refno='" + values.new_invoicerefno + "',"+
                           " transaction_code='" + values.new_invoicerefno + "'"+
                          " where journal_refno = '" + values.Old_invoicerefno + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1) { 
                    values.status = true;
                    values.message = "Invoice Reference No updated Successfully";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Invoice Reference No.";
                }
            }
        }
        public void Dacancelinvoice(CancelinvoiceList values)
        {
            try
            {
                double invoice_amount = double.Parse(values.invoice_amount);

                msSQL = "update smr_trn_tsalesorder set invoice_amount " +
                    " = invoice_amount - '" + values.invoice_amount.Replace(",", "").Trim() + "' where salesorder_gid = '" + values.salesorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    msSQL = "update smr_trn_tsalesorder set " +
                        " invoice_flag = 'Invoice Pending', " +
                        " salesorder_status = 'Invoice Cancelled' " +
                        " where salesorder_gid = '" + values.salesorder_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msSQL = "select * from smr_trn_tdeliveryorder where salesorder_gid='" + values.salesorder_gid + "'";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows)
                        {
                            msSQL = "update smr_trn_tdeliveryorder set " +
                                " invoice_status = 'Invoice Pending' where salesorder_gid = '" + values.salesorder_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                        }
                        msSQL = "update rbl_trn_tinvoice set " +
                                " invoice_flag = 'Invoice Cancelled', " +
                                " invoice_status = 'Invoice Cancelled' where invoice_gid = '" + values.invoice_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1)
                    {
                        objfincmn.invoice_cancel(values.invoice_gid);
                        values.status = true;
                        values.message = "Invoice Cancelled Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Cancelling Invoice";
                    }


                }
            }
            catch (Exception e)
            {

            }
        }
        public void DaGetDeleteInvoiceProductSummary(string invoice_gid, invoiceproductlist values)
        {
            try
            {
                msSQL = " delete from rbl_tmp_tinvoicedtl where invoicedtl_gid='" + invoice_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Invoice Product Delete!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetDeletemainInvoiceProductSummary(string invoicedtl_gid, MdlSmrRptInvoiceReport values)
        {
            try
            {
                msSQL = " delete from rbl_trn_tinvoicedtl where invoicedtl_gid='" + invoicedtl_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Invoice Product Delete!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaSalesInvoiceSubmit(string employee_gid, string user_gid, salesinvoicelist values)
        {
            try
            {
                double addonCharges = double.TryParse(values.addon_charge, out double addonChargesValue) ? addonChargesValue : 0;
                double freightCharges = double.TryParse(values.freight_charges, out double freightChargesValue) ? freightChargesValue : 0;
                double forwardingCharges = double.TryParse(values.forwardingCharges, out double packingChargesValue) ? packingChargesValue : 0;
                double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                double buybackCharges = double.TryParse(values.buybackcharges, out double buybackChargesValue) ? buybackChargesValue : 0;
                double addonCharges_l = Math.Round(addonCharges*double.Parse(values.exchange_rate),2);
                double freightCharges_l = Math.Round(freightCharges * double.Parse(values.exchange_rate), 2); ;
                double forwardingCharges_l = Math.Round(forwardingCharges * double.Parse(values.exchange_rate), 2); 
                double insuranceCharges_l = Math.Round(insuranceCharges * double.Parse(values.exchange_rate), 2); 
                double additionaldiscountAmount_l = Math.Round(additionaldiscountAmount * double.Parse(values.exchange_rate), 2); 
                double buybackCharges_l = Math.Round(buybackCharges * double.Parse(values.exchange_rate), 2);


                string uiDateStr = values.invoice_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlinvoiceDate = uiDate.ToString("yyyy-MM-dd");

                string uiDateStr2 = values.due_date;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlpaymentDate = uiDate2.ToString("yyyy-MM-dd");




                msINGetGID = objcmnfunctions.GetMasterGID("SIVT");
                msSalesGid = objcmnfunctions.GetMasterGID("VSOP");
                msSQL = "select company_code from adm_mst_Tcompany";
                string lscompanycode = objdbconn.GetExecuteScalar(msSQL);
                string ls_referenceno = "";
                ls_referenceno = objcmnfunctions.getSequencecustomizerGID("INV", "RBL", values.branch_name);



                msSQL = "select customer_name, customer_code, customer_id from crm_mst_tcustomer where customer_gid='" + values.customer_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    
                    lsCustomername = objMySqlDataReader["customer_name"].ToString();
                    lscustomer_code = objMySqlDataReader["customer_code"].ToString();
                    lscustomer_id = objMySqlDataReader["customer_id"].ToString();

                    objMySqlDataReader.Close();
                }

                msSQL = " select customercontact_name, email, mobile, concat(address1,' ', address2) as address from crm_mst_tcustomercontact" +
                    " where customer_gid='" + values.customer_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    
                    values.customercontactperson = objMySqlDataReader["customercontact_name"].ToString();
                    values.customercontactnumber = objMySqlDataReader["mobile"].ToString();
                    //values.customeraddress = objMySqlDataReader["address"].ToString();
                    values.customeremailaddress = objMySqlDataReader["email"].ToString();

                    objMySqlDataReader.Close();
                }

                msSQL = "select currency_code from crm_trn_tcurrencyexchange  where currencyexchange_gid='" + values.currency_code + "'";
                string lscurrencycode = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                string lstaxname = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                string lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);



                msSQL = "SELECT  a.invoicedtl_gid, a.product_gid, a.customerproduct_code,a.product_name, a.product_remarks,a.product_code, b.productgroup_gid," +
                " format(a.qty_invoice, 2) AS qty_ordered_1, a.qty_invoice as qty_ordered, c.productgroup_name, a.uom_name, b.producttype_gid," +
                " a.created_by, a.product_price, format(a.product_price, 2) AS producttotal_price," +
                " a.discount_percentage,a.product_total,a.discount_amount, format(a.discount_amount, 2) AS discount_amount_1," +
                " a.tax_percentage, format(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field," +
                " a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2,a.tax_percentage3," +
                " format(a.tax_amount2, 2) AS tax_amount2, format(a.tax_amount3, 2) AS tax_amount3, a.tax1_gid," +
                " a.tax2_gid, a.tax3_gid ," +
                " concat( case when a.tax_name is null then '' else a.tax_name end, ' '," +
                " case when a.tax_percentage = '0' then '' else concat('',a.tax_percentage,'%') end ," +
                " case when a.tax_amount = '0' then '' else concat(':',a.tax_amount) end) as tax," +
                " concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ', " +
                " case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end, " +
                " case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2," +
                " concat(case when a.tax_name3 is null then '' else a.tax_name3 end, ' '," +
                " case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                " end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3," +
                " format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.taxsegment_gid, vendor_price" +
                " FROM rbl_tmp_tinvoicedtl a  " +
                "LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid  " +
                " WHERE a.employee_gid = '" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    double lsvendorprice = Math.Round(double.Parse(dt["vendor_price"].ToString().Replace(",", "")), 2);
                    double lsproduct_discount = Math.Round(double.Parse(dt["discount_amount"].ToString().Replace(",", "")), 2);

                    double lsproduct_price = Math.Round((double.Parse(dt["qty_ordered"].ToString()) * lsvendorprice) - lsproduct_discount, 2);
                    double lsexchangerate = double.Parse(values.exchange_rate);
                    double lsproductprice_l = Math.Round((lsproduct_price * lsexchangerate), 2);
                    double lstaxamount_l = Math.Round((double.Parse(dt["tax_amount"].ToString().Replace(",", "")) * lsexchangerate), 2);
                    double lstaxamount2_l = Math.Round((double.Parse(dt["tax_amount2"].ToString().Replace(",", "")) * lsexchangerate), 2);
                    double lstaxamount3_l = Math.Round((double.Parse(dt["tax_amount3"].ToString().Replace(",", "")) * lsexchangerate), 2);
                    double lsdiscountamount_l = Math.Round((double.Parse(dt["discount_amount"].ToString().Replace(",", "")) * lsexchangerate), 2);
                    double lsproducttotal_l = Math.Round((double.Parse(dt["product_total"].ToString().Replace(",", "")) * lsexchangerate), 2);

                    string display_field = dt["product_remarks"].ToString();

                    msGetGid = objcmnfunctions.GetMasterGID_SP("SIVC");

                    msSQL = " insert into rbl_trn_tinvoicedtl (" +
                        " invoicedtl_gid, " +
                        " invoice_gid, " +
                        " product_gid, " +
                        " product_code, " +
                       " productgroup_gid, " +
                        " productgroup_name, " +
                        " product_name, " +
                        " uom_gid, " +
                        " productuom_name, " +
                        " product_price, " +
                        " discount_percentage, " +
                        " discount_amount, " +
                        " tax_name, " +
                        " tax_name2, " +
                        " tax_name3, " +
                        " tax1_gid," +
                        " tax2_gid," +
                        " tax3_gid," +
                        " tax_percentage, " +
                        " tax_percentage2, " +
                        " tax_percentage3, " +
                        " tax_amount, " +
                        " tax_amount2, " +
                        " tax_amount3, " +
                        " qty_invoice, " +
                        " product_remarks, " +
                        " product_total, " +
                        " product_price_L, " +
                        " discount_amount_L, " +
                        " tax_amount1_L, " +
                        " tax_amount2_L, " +
                        " tax_amount3_L, " +
                        " product_total_L, " +
                        " display_field, " +
                        " customerproduct_code," +
                        " taxsegment_gid," +
                        " vendor_price," +
                        " created_by," +
                        " created_date " +
                            " ) values ( " +
                            "'" + msGetGid + "'," +
                            "'" + msINGetGID + "'," +
                            "'" + dt["product_gid"].ToString() + "'," +
                            "'" + dt["product_code"].ToString() + "'," +
                               "'" + dt["productgroup_gid"].ToString() + "'," +
                           "'" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "'," +
                            "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                            "'" + dt["uom_gid"].ToString() + "'," +
                            "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                            "'" + lsproduct_price + "'," +
                            "'" + dt["discount_percentage"].ToString() + "'," +
                            "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_name"].ToString() + "'," +
                            "'" + dt["tax_name2"].ToString() + "'," +
                            "'" + dt["tax_name3"].ToString() + "'," +
                            "'" + dt["tax1_gid"].ToString() + "'," +
                            "'" + dt["tax2_gid"].ToString() + "'," +
                            "'" + dt["tax3_gid"].ToString() + "'," +
                            "'" + dt["tax_percentage"].ToString() + "'," +
                            "'" + dt["tax_percentage2"].ToString() + "'," +
                            "'" + dt["tax_percentage3"].ToString() + "'," +
                            "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                            "'" + dt["qty_ordered"].ToString().Replace(",", "") + "',";
                    if(display_field != null)
                    {
                        msSQL += "'" + display_field.Replace("'","\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + display_field + "',";
                    }

                    msSQL += "'" + dt["product_total"].ToString().Replace(",", "") + "'," +
                        "'" + lsproductprice_l + "'," +
                        "'" + lsdiscountamount_l + "'," +
                        "'" + lstaxamount_l + "'," +
                        "'" + lstaxamount2_l + "'," +
                        "'" + lstaxamount3_l + "'," +
                        "'" + lsproducttotal_l + "',";
                            if (display_field != null)
                    {
                        msSQL += "'" + display_field.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + display_field + "',";
                    }

                   msSQL += "'" + dt["customerproduct_code"].ToString() + "'," +
                            "'" + dt["taxsegment_gid"].ToString() + "'," +
                            "'" + lsvendorprice + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')"
                            ;
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        mssalesorderGID1 = objcmnfunctions.GetMasterGID_SP("VSDC");
                        msSQL = " insert into smr_trn_tsalesorderdtl (" +
                             " salesorderdtl_gid ," +
                             " salesorder_gid," +
                             " product_gid ," +
                             " productgroup_gid," +
                             " productgroup_name," +
                             " customerproduct_code," +
                             " product_name," +
                             " product_code," +
                             " display_field," +
                             " product_price," +
                             " qty_quoted," +
                             " tax_amount ," +
                             " uom_gid," +
                             " uom_name," +
                             " price," +
                             " tax_name," +
                             " tax_name2," +
                             " tax_name3," +
                             " tax1_gid," +
                             " tax2_gid," +
                             " tax3_gid," +
                             " tax_amount2," +
                             " tax_amount3," +
                             " tax_percentage," +
                             " tax_percentage2," +
                             " tax_percentage3," +
                             " type ," +
                             " tax_amount_l," +
                             " tax_amount2_l," +
                             " tax_amount3_l," +
                             " discount_percentage," +
                             " discount_amount," +
                             " product_price_l," +
                             " price_l ," +
                             " taxsegment_gid " +
                             ") values (" +
                             "'" + mssalesorderGID1 + "'," +
                             "'" + msSalesGid + "'," +
                              "'" + dt["product_gid"].ToString() + "'," +
                              "'" + dt["productgroup_gid"].ToString() + "'," +
                              "'" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "'," +
                              "'" + dt["customerproduct_code"].ToString() + "'," +
                              "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                              "'" + dt["product_code"].ToString() + "',";
                             if (display_field != null)
                        {
                            msSQL += "'" + display_field.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + display_field + "',";
                        }
                       msSQL += "'" + lsvendorprice + "'," +
                              "'" + dt["qty_ordered"].ToString().Replace(",", "") + "'," +
                              "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                              "'" + dt["uom_gid"].ToString() + "'," +
                              "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                              "'" + dt["product_total"].ToString() + "'," +
                              "'" + dt["tax_name"].ToString() + "'," +
                              "'" + dt["tax_name2"].ToString() + "'," +
                              "'" + dt["tax_name3"].ToString() + "'," +
                              "'" + dt["tax1_gid"].ToString() + "'," +
                              "'" + dt["tax2_gid"].ToString() + "'," +
                              "'" + dt["tax3_gid"].ToString() + "'," +
                              "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                              "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                              "'" + dt["tax_percentage"].ToString() + "'," +
                              "'" + dt["tax_percentage2"].ToString() + "'," +
                              "'" + dt["tax_percentage3"].ToString() + "'," +
                              "'Sales'," +
                              "'" + lstaxamount_l + "'," +
                              "'" + lstaxamount2_l + "'," +
                              "'" + lstaxamount3_l + "'," +
                              "'" + dt["discount_percentage"].ToString() + "'," +
                              "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                              "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                              "'" + lsproducttotal_l + "'," +
                              "'" + dt["taxsegment_gid"].ToString() + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                    }
                }
                msSQL = "select company_code from adm_mst_Tcompany";
                string lscompany_code=objdbconn.GetExecuteScalar(msSQL);
                string so_reference = "";
                if (lscompany_code == "BOBA") { 
                 so_reference = "SO/" + ls_referenceno;
                }
                else
                {
                    so_reference = objcmnfunctions.GetMasterGID("SO");
                }
                string lstype = string.Empty;
                msSQL = "select distinct type from smr_trn_tsalesorderdtl where salesorder_gid='" + msSalesGid + "' ";
                string lstype_1 = objdbconn.GetExecuteScalar(msSQL);
                if (lstype == "")
                {
                    lstype = lstype_1;
                }
                else
                {
                    lstype = "Both";
                }
                double lsexchange = double.Parse(values.exchange_rate);
                double lstotalamount_l = Math.Round((double.Parse(values.totalamount) * lsexchange), 2);
                double lsgrandtotal_l = Math.Round((double.Parse(values.grandtotal) * lsexchange), 2);
                double lsaddoncharges_l = Math.Round((double.Parse(values.addon_charge) * lsexchange), 2);
                double lsadditionaldiscountAmount_l = Math.Round((additionaldiscountAmount * lsexchange), 2);

                msSQL = " insert into rbl_trn_tinvoice(" +
                       " invoice_gid," +
                       " invoice_date," +
                       " payment_term, " +
                       " payment_date," +
                       " invoice_from," +
                       " customer_gid," +
                       " customer_name," +
                       " customer_contactperson," +
                       " customer_contactnumber," +
                       " customer_address," +
                       " customer_email," +
                       " mode_of_despatch," +
                       " total_amount," +
                       " invoice_amount," +
                       " invoice_refno," +
                       " invoice_status," +
                       " invoice_flag," +
                       " user_gid," +
                       " discount_amount," +
                       " additionalcharges_amount," +
                       " total_amount_L," +
                       " invoice_amount_L," +
                       " discount_amount_L," +
                       " additionalcharges_amount_L," +
                       " invoice_remarks," +
                       " termsandconditions," +
                       " currency_code," +
                       " exchange_rate," +
                       " branch_gid, " +
                       " roundoff," +
                        " tax_gid," +
                       " tax_name," +
                       " tax_percentage, " +
                       " tax_amount," +
                       " taxsegment_gid," +
                       " created_date," +
                       " freight_charges," +
                       " packing_charges," +
                       " delivery_date," +
                       " payment_days," +
                       " delivery_days," +
                       " insurance_charges, " +
                       " invoice_reference, " +
                       " bill_email, " +
                       " po_number, " +
                       " sales_type ," +
                       " shipping_to " +
                       " ) values (" +
                       " '" + msINGetGID + "'," +
                       "'" + mysqlinvoiceDate + "'," +
                       "'" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "'," +
                       "'" + mysqlpaymentDate + "'," +
                       "'Direct Invoice'," +
                       " '" + values.customer_gid + "'," +
                       " '" + lsCustomername.Replace("'", "\\\'") + "'," +
                       " '" + values.customercontactperson.Replace("'", "\\\'") + "'," +
                       " '" + values.customercontactnumber + "'," +
                       " '" + values.customeraddress.Replace("'", "\\\'") + "'," +
                       " '" + values.customeremailaddress + "'," +
                       " '" + (String.IsNullOrEmpty(values.dispatch_mode) ? values.dispatch_mode : values.dispatch_mode.Replace("'", "\\\'")) + "'," +
                       " '" + values.totalamount.Replace(",", "").Trim() + "'," +
                       " '" + values.grandtotal.Replace(",", "").Trim() + "'," +
                       " '" + ls_referenceno + "'," +
                       " 'Payment Pending'," +
                       " 'Invoice Approved'," +
                       " '" + employee_gid + "'," +
                       " '" + additionaldiscountAmount + "'," +
                       " '" + addonCharges + "'," +
                       "'" + lstotalamount_l + "'," +
                       "'" + lsgrandtotal_l + "'," +
                       "'" + lsadditionaldiscountAmount_l + "'," +
                       "'" + lsaddoncharges_l + "',";
                if (values.remarks != null)
                {
                    msSQL += "'" + values.remarks.Replace("'", "\\\'") + "',";
                }
                else
                {
                    msSQL += "'" + values.remarks + "',";
                }
              msSQL +=  "'" + (String.IsNullOrEmpty(values.termsandconditions) ? values.termsandconditions : values.termsandconditions.Replace("'", "\\\'")) + "', " +
                       "'" + lscurrencycode + "'," +
                       "'" + values.exchange_rate + "'," +
                       "'" + values.branch_name + "', " +
                       "'" + roundoff + "'," +
                       "'" + values.tax_name4 + "'," +
                       "'" + lstaxname + "', " +
                       "'" + lstaxpercentage + "'," +
                       "'" + values.tax_amount4 + "'," +
                       "'" + values.taxsegment_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                       "'" + freightCharges + "'," +
                       "'" + forwardingCharges + "'," +
                       "'" + (String.IsNullOrEmpty(values.delivery_days) ? values.delivery_days : values.delivery_days.Replace("'", "\\\'")) + "'," +
                       "'" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "'," +
                       "'" + (String.IsNullOrEmpty(values.delivery_days) ? values.delivery_days : values.delivery_days.Replace("'", "\\\'")) + "'," +
                       "'" + insuranceCharges + "'," +
                       "'" + msSalesGid + "'," +
                       "'" + values.bill_email + "'," +
                        "'" + values.order_refno + "'," +
                       "'" + values.sales_type + "'," +
                       "'" + (String.IsNullOrEmpty(values.shipping_to) ? values.shipping_to : values.shipping_to.Replace("'", "\\\'")) + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "SELECT account_gid FROM crm_mst_tcustomer WHERE customer_gid='" + values.customer_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows)
                {
                    while (objMySqlDataReader.Read())
                    {
                        string lsaccount_gid = objMySqlDataReader["account_gid"]?.ToString(); // Safely get the value

                        // Check if lsaccount_gid is null or empty
                        if (string.IsNullOrEmpty(lsaccount_gid))
                        {
                            objfincmn.finance_vendor_debitor("Sales", lscustomer_id, lsCustomername, values.customer_gid, user_gid);
                            string trace_comment = "Added a customer on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_customer");
                        }
                    }
                }
                objMySqlDataReader.Close();
                if (mnResult == 1)
                {
                    msSQL = " insert into smr_trn_tsalesorder ( " +
                       " salesorder_gid," +
                       " branch_gid," +
                       " salesorder_date," +
                       " customer_gid," +
                       " customer_contact_person," +
                       " customer_address," +
                       " total_amount," +
                       " payment_days, " +
                       " payment_terms, " +
                       " salesorder_status," +
                       " created_by," +
                       " created_date, " +
                       " termsandconditions, " +
                       " so_referenceno1," +
                       " Grandtotal, " +
                       " customer_name," +
                       " so_type," +
                       " invoice_flag," +
                       " addon_charge," +
                       " additional_discount, " +
                       " invoice_amount, " +
                       " addon_charge_l," +
                       " additional_discount_l, " +
                       " currency_code, " +
                       " exchange_rate," +
                       " customer_email," +
                       " customer_mobile," +
                       " total_price, " +
                       " roundoff, " +
                       " freight_charges," +
                       " shipping_to, " +
                       "tax_name," +
                       "tax_amount," +
                       " insurance_charges, " +
                        " po_number, " +
                        " sales_type ," +
                       " delivery_days " +
                       " ) values (" +
                       "'" + msSalesGid + "'," +
                       "'" + values.branch_name + "'," +
                       "'" + mysqlinvoiceDate + "'," +
                       "'" + values.customer_gid + "'," +
                       "'" + values.customercontactperson.Replace("'", "\\\'") + "'," +
                       "'" + values.customeraddress.Replace("'", "\\\'") + "'," +
                       "'" + values.grandtotal + "'," +
                       "'" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "'," +
                       "'" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "'," +
                       "'Invoice Raised'," +
                       "'" + employee_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "") + "', " +
                       "'" + so_reference + "'," +
                       "'" + values.grandtotal + "'," +
                       "'" + lsCustomername.Replace("'", "\\\'") + "'," +
                       "'" + lstype + "'," +
                       "'Invoice Approval Pending'," +
                       "'" + addonCharges + "'," +
                       "'" + additionaldiscountAmount + "'," +
                       "'" + values.grandtotal + "'," +
                       "'" + lsaddoncharges_l + "'," +
                       "'" + lsadditionaldiscountAmount_l + "'," +
                       "'" + lscurrencycode + "'," +
                       "'" + values.exchange_rate + "'," +
                       "'" + values.customeremailaddress + "'," +
                       "'" + values.customercontactnumber + "'," +
                       "'" + values.grandtotal + "'," +
                       "'" + roundoff + "'," +
                       "'" + freightCharges + "'," +
                       "'" + (String.IsNullOrEmpty(values.shipping_to) ? values.shipping_to : values.shipping_to.Replace("'", "\\\'")) + "'," +
                       "'" + lstaxname + "'," +
                       "'" + values.tax_amount4 + "'," +
                       "'" + insuranceCharges + "'," +
                        "'" + values.order_refno + "'," +
                          "'" + values.sales_type + "'," +
                       "'" + values.delivery_days + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                    msSQL = " update acp_trn_torder set so_type='" + lstype + "' where salesorder_gid='" + msSalesGid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "select finance_flag from adm_mst_tcompany ";
                    string finance_flag = objdbconn.GetExecuteScalar(msSQL);
                    if (finance_flag == "Y")
                    {

                        double roundoff_l = roundoff * Convert.ToDouble(values.exchange_rate);
                        string lsproduct_price_l = "", lstax1_gid = "", lstax2_gid = "";
                        msSQL = " select sum(product_price_L) as product_price_L,sum(tax_amount1_L) as tax1,sum(tax_amount2_L) as tax2,tax1_gid,tax2_gid from rbl_trn_tinvoicedtl " +
                             " where invoice_gid='" + msINGetGID + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            lsproduct_price_l = objOdbcDataReader["product_price_L"].ToString();
                            lstax1 = objOdbcDataReader["tax1"].ToString();
                            lstax2 = objOdbcDataReader["tax2"].ToString();
                            lstax1_gid = objOdbcDataReader["tax1_gid"].ToString();
                            lstax2_gid = objOdbcDataReader["tax2_gid"].ToString();
                        }
                        objOdbcDataReader.Close();
                        lsbasic_amount = Math.Round(double.Parse(lsproduct_price_l), 2);
                        objfincmn.jn_invoice(mysqlinvoiceDate, values.remarks, values.branch_name, ls_referenceno, msINGetGID
                         , lsbasic_amount, addonCharges_l, additionaldiscountAmount_l, lsgrandtotal_l, values.customer_gid, "Invoice", "RBL",
                         values.sales_type, roundoff_l, freightCharges_l, buybackCharges_l, forwardingCharges_l, insuranceCharges_l, values.tax_amount4, values.tax_name4);
                       

                      



                        if (lstax1 != "0.00" && lstax1 != "" && lstax1 != null &&lstax1!=null)
                                {
                            decimal lstaxsum = decimal.Parse(lstax1);
                                    string lstaxamount = lstaxsum.ToString("F2");
                                    double tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                                    objfincmn.jn_sales_tax(msINGetGID, ls_referenceno, values.remarks, tax_amount, lstax1_gid);
                                }
                                if (lstax2 != "0.00" && lstax2 != "" && lstax2 != null && lstax2!="0")
                                {
                            decimal lstaxsum = decimal.Parse(lstax2);
                                    string lstaxamount = lstaxsum.ToString("F2");
                                    double tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                                    objfincmn.jn_sales_tax(msINGetGID, ls_referenceno, values.remarks, tax_amount, lstax2_gid);
                                }
                            


                        
                    }

                    msSQL = " delete from rbl_tmp_tinvoicedtl where employee_gid='" + employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    values.status = true;
                    values.message = "Invoice Submitted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while submitting the Invoice";
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while submitting Invoice!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetInvoiceForLastSixMonths(MdlSmrRptInvoiceReport values)
        {
            try
            {
                msSQL = " SELECT format( ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2),2) AS invoiceamount," +
                    " ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2) AS invoiceamount1, " +
                        " YEAR(a.invoice_date) AS year, a.invoice_date, SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'),1, 3) AS months,a.invoice_gid," +
                        " COUNT(a.invoice_gid) AS invoicecount,taxtotal.taxtotal, format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate) - COALESCE(taxtotal.taxtotal, 0),2),2) AS netamount " +
                        " FROM rbl_trn_tinvoice a LEFT JOIN smr_trn_tsalesorder i ON i.salesorder_gid = a.invoice_reference LEFT JOIN (SELECT " +
                        " YEAR(a.invoice_date) AS year, DATE_FORMAT(a.invoice_date, '%M') AS month, format(round(SUM(b.tax_amount1_L + b.tax_amount2_L + b.tax_amount3_L),2),2) AS taxtotal " +
                        " FROM rbl_trn_tinvoicedtl b LEFT JOIN rbl_trn_tinvoice a ON b.invoice_gid = a.invoice_gid" +
                        " WHERE  a.invoice_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH) " +
                        " AND a.invoice_date <= DATE(NOW())" +
                        " AND a.invoice_status NOT IN ('Invoice Cancelled' , 'Rejected')" +
                        " AND a.invoice_flag NOT IN ('Invoice Cancelled' , 'Rejected')" +
                        " GROUP BY DATE_FORMAT(a.invoice_date, '%M')) taxtotal ON taxtotal.year = YEAR(a.invoice_date)" +
                        " AND taxtotal.month = DATE_FORMAT(a.invoice_date, '%M')" +
                       " WHERE a.invoice_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH) AND a.invoice_date <= DATE(NOW())" +
                        " AND a.invoice_status NOT IN ('Invoice Cancelled' , 'Rejected')" +
                        " AND a.invoice_flag NOT IN ('Invoice Cancelled' , 'Rejected') GROUP BY DATE_FORMAT(a.invoice_date, '%M') ORDER BY a.invoice_date DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetInvoiceForLastSixMonths_List = new List<GetInvoiceForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetInvoiceForLastSixMonths_List.Add(new GetInvoiceForLastSixMonths_List
                        {
                            invoice_gid = (dt["invoice_gid"].ToString()),
                            invoice_date = (dt["invoice_date"].ToString()),
                            months = (dt["months"].ToString()),
                            year = (dt["year"].ToString()),
                            Tax_amount = (dt["taxtotal"].ToString()),
                            net_amount = (dt["netamount"].ToString()),
                            invoiceamount = (dt["invoiceamount"].ToString()),
                            invoiceamount1 = (dt["invoiceamount1"].ToString()),
                            invoicecount = (dt["invoicecount"].ToString()),
                        });
                        values.GetInvoiceForLastSixMonths_List = GetInvoiceForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Report For Last Six Months !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetReceiptForLastSixMonths(MdlSmrRptInvoiceReport values)
        {
            try
            {
                msSQL = " SELECT  DATE_FORMAT(a.payment_date, '%M %Y') AS month_year,COUNT(a.payment_gid) AS invoicecount,sum(a.total_amount) AS paymentamount,DATE_FORMAT(a.payment_date, '%M') AS month,DATE_FORMAT(a.payment_date, '%Y') AS year " +
                        " FROM rbl_trn_tpayment a WHERE a.payment_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) " +
                        " GROUP BY DATE_FORMAT(a.payment_date, '%M %Y')" +
                        " ORDER BY a.payment_date DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetReceiptForLastSixMonths_List = new List<GetReceiptForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetReceiptForLastSixMonths_List.Add(new GetReceiptForLastSixMonths_List
                        {


                            month_year = (dt["month_year"].ToString()),
                            year = (dt["year"].ToString()),
                            month = (dt["month"].ToString()),
                            paymentamount = (dt["paymentamount"].ToString()),
                            invoicecount = (dt["invoicecount"].ToString()),
                        });
                        values.GetReceiptForLastSixMonths_List = GetReceiptForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Report For Last Six Months !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetInvoiceReportForLastSixMonthsSearch(MdlSmrRptInvoiceReport values, string from_date, string to_date)
        {
            try
            {
                if (from_date == null && to_date == null)
                {
                    msSQL = " SELECT  format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2),2) AS invoiceamount, " +
                        " ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2) AS invoiceamount1, " +
                        " YEAR(a.invoice_date) AS year, a.invoice_date, SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'),1, 3) AS months,a.invoice_gid," +
                        " COUNT(a.invoice_gid) AS invoicecount,taxtotal.taxtotal, format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate) - COALESCE(taxtotal.taxtotal, 0),2),2) AS netamount " +
                        " FROM rbl_trn_tinvoice a LEFT JOIN smr_trn_tsalesorder i ON i.salesorder_gid = a.invoice_reference LEFT JOIN (SELECT " +
                        " YEAR(a.invoice_date) AS year, DATE_FORMAT(a.invoice_date, '%M') AS month, format(round(SUM(b.tax_amount1_L + b.tax_amount2_L + b.tax_amount3_L),2),2) AS taxtotal " +
                        " FROM rbl_trn_tinvoicedtl b LEFT JOIN rbl_trn_tinvoice a ON b.invoice_gid = a.invoice_gid" +
                        " WHERE  a.invoice_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH) " +
                        " AND a.invoice_date <= DATE(NOW())" +
                        " AND a.invoice_status NOT IN ('Invoice Cancelled' , 'Rejected')" +
                        " AND a.invoice_flag NOT IN ('Invoice Cancelled' , 'Rejected')" +
                        " GROUP BY DATE_FORMAT(a.invoice_date, '%M')) taxtotal ON taxtotal.year = YEAR(a.invoice_date)" +
                        " AND taxtotal.month = DATE_FORMAT(a.invoice_date, '%M')" +
                       " WHERE a.invoice_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH) AND a.invoice_date <= DATE(NOW())" +
                        " AND a.invoice_status NOT IN ('Invoice Cancelled' , 'Rejected')" +
                        " AND a.invoice_flag NOT IN ('Invoice Cancelled' , 'Rejected') GROUP BY DATE_FORMAT(a.invoice_date, '%M') ORDER BY a.invoice_date DESC";
                }
                else
                {
                    msSQL = " SELECT  format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2),2) AS invoiceamount, " +
                        " ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2) AS invoiceamount1, " +
                        " YEAR(a.invoice_date) AS year, a.invoice_date, SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'),1, 3) AS months,a.invoice_gid," +
                        " COUNT(a.invoice_gid) AS invoicecount,taxtotal.taxtotal, format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate) - COALESCE(taxtotal.taxtotal, 0),2),2) AS netamount " +
                        " FROM rbl_trn_tinvoice a LEFT JOIN smr_trn_tsalesorder i ON i.salesorder_gid = a.invoice_reference LEFT JOIN (SELECT " +
                        " YEAR(a.invoice_date) AS year, DATE_FORMAT(a.invoice_date, '%M') AS month, format(round(SUM(b.tax_amount1_L + b.tax_amount2_L + b.tax_amount3_L),2),2) AS taxtotal " +
                        " FROM rbl_trn_tinvoicedtl b LEFT JOIN rbl_trn_tinvoice a ON b.invoice_gid = a.invoice_gid" +
                        " WHERE  a.invoice_date between '" + from_date + "' and '" + to_date + "' " +
                        " AND a.invoice_status NOT IN ('Invoice Cancelled' , 'Rejected')" +
                        " AND a.invoice_flag NOT IN ('Invoice Cancelled' , 'Rejected')" +
                        " GROUP BY DATE_FORMAT(a.invoice_date, '%M')) taxtotal ON taxtotal.year = YEAR(a.invoice_date)" +
                        " AND taxtotal.month = DATE_FORMAT(a.invoice_date, '%M')" +
                       " WHERE a.invoice_date between '" + from_date + "' and '" + to_date + "' " +
                        " AND a.invoice_status NOT IN ('Invoice Cancelled' , 'Rejected')" +
                        " AND a.invoice_flag NOT IN ('Invoice Cancelled' , 'Rejected') GROUP BY DATE_FORMAT(a.invoice_date, '%M') ORDER BY a.invoice_date DESC";

                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetInvoiceForLastSixMonths_List = new List<GetInvoiceForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetInvoiceForLastSixMonths_List.Add(new GetInvoiceForLastSixMonths_List
                        {
                            invoice_gid = (dt["invoice_gid"].ToString()),
                            invoice_date = (dt["invoice_date"].ToString()),
                            months = (dt["months"].ToString()),
                            year = (dt["year"].ToString()),
                            invoiceamount = (dt["invoiceamount"].ToString()),
                            invoiceamount1 = (dt["invoiceamount1"].ToString()),
                            Tax_amount = (dt["taxtotal"].ToString()),
                            net_amount = (dt["netamount"].ToString()),
                            invoicecount = (dt["invoicecount"].ToString()),
                        });
                        values.GetInvoiceForLastSixMonths_List = GetInvoiceForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetInvoiceDetailSummary(string branch_gid, string month, string year, MdlSmrRptInvoiceReport values)
        {
            try
            {

                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " select distinct a.invoice_gid,a.user_gid, a.invoice_refno,a.invoice_status as status,DATE_FORMAT(a.invoice_date, '%d-%m-%Y') as invoice_date ,  " +
                    " concat(d.user_firstname,' ',d.user_lastname) as name,a.invoice_reference,a.branch_gid,f.branch_name, " +
                    " format(round(((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount)*a.exchange_rate),2),2) as invoiceamount,concat_ws( '/',y.customercontact_name, y.mobile, y.email) as customer_contactperson, " +
                    " case when a.currency_code = '" + currency + "' then concat(z.customer_id,' / ',z.customer_name) when a.currency_code is null then concat(z.customer_id,' / ',z.customer_name) " +
                    "  when a.currency_code is not null and a.currency_code <> '" + currency + "' then concat(z.customer_id,' / ',z.customer_name) end as customer_name, " +
                    "  a.customer_contactnumber  as mobile,a.invoice_from,a.currency_code  " +
                    " from rbl_trn_tinvoice a  " +
                    " left join smr_trn_tsalesorder i on i.salesorder_gid=a.invoice_reference  " +
                    " left join hrm_mst_temployee b on b.employee_gid= a.user_gid  " +
                    " left join adm_mst_tuser d on d.user_gid=b.user_gid " +
                    " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid " +
                    " left join crm_mst_tcustomer z on z.customer_gid = a.customer_gid " +
                     " left join crm_mst_tcustomercontact y on y.customer_gid = z.customer_gid " +
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                    " where substring(date_format(a.invoice_date,'%M'),1,3)='" + month + "' and year(a.invoice_date)='" + year + "'  " +
                    " and a.invoice_status not in('Invoice Cancelled','Rejected')  " +
                    " and a.invoice_flag not in('Invoice Cancelled','Rejected') ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetInvoiceDetailSummary = new List<GetInvoiceDetailSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetInvoiceDetailSummary.Add(new GetInvoiceDetailSummary
                        {
                            invoice_refno = (dt["invoice_refno"].ToString()),
                            invoice_date = (dt["invoice_date"].ToString()),
                            customer_name = (dt["customer_name"].ToString()),
                            contact_details = (dt["customer_contactperson"].ToString()),
                            salesinvoice_status = (dt["status"].ToString()),
                            branch_name = (dt["branch_name"].ToString()),
                            created_by = (dt["name"].ToString()),
                            invoiceamount = (dt["invoiceamount"].ToString()),
                            invoice_gid = (dt["invoice_gid"].ToString()),
                        });
                        values.GetInvoiceDetailSummary = GetInvoiceDetailSummary;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Detail Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetInvoivceIndividualreport(MdlSmrRptOrderReport values, string salesorder_gid)
        {
            try
            {
                msSQL = "select a.salesorder_gid,b.invoice_refno,a.salesorder_date,a.so_referenceno1," +
                    "a.customer_name,concat(a.customer_contact_person,'/',a.customer_address,'/',a.customer_mobile,'/',a.customer_email)as customer_details," +
                    "b.invoice_date,format((a.grandtotal_l),2) as grand_total, format((b.advance_amount),2) as advance_amount,a.salesorder_status,a.so_type," +
                    " format((b.total_amount_L),2) as invoice_amount, format((((a.grandtotal_l)-(b.total_amount_L))),2) as pending_invoice_amount,c.branch_name " +
                    " from smr_trn_tsalesorder a left join rbl_trn_tinvoice b on  a.salesorder_gid=b.invoice_reference left join hrm_mst_tbranch c on  a.branch_gid=c.branch_gid where salesorder_gid='" + salesorder_gid + "' order by a.salesorder_date desc ;";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var individualreport_list = new List<individualreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        individualreport_list.Add(new individualreport_list
                        {
                            invoice_refno = (dt["invoice_refno"].ToString()),
                            salesorder_date = (dt["salesorder_date"].ToString()),
                            so_referenceno1 = (dt["so_referenceno1"].ToString()),
                            customer_name = (dt["customer_name"].ToString()),
                            customer_details = (dt["customer_details"].ToString()),
                            branch_name = (dt["branch_name"].ToString()),
                            salesorder_status = (dt["salesorder_status"].ToString()),
                            so_type = (dt["so_type"].ToString()),
                            invoice_date = (dt["invoice_date"].ToString()),
                            grand_total = (dt["grand_total"].ToString()),
                            advance_amount = (dt["advance_amount"].ToString()),
                            invoice_amount = (dt["invoice_amount"].ToString()),
                            pending_invoice_amount = (dt["pending_invoice_amount"].ToString()),
                        });
                        values.individualreport_list = individualreport_list;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Detail Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetReceiptDetailSummary(string branch_gid, string month, string year, MdlSmrRptInvoiceReport values)
        {
            try
            {
                msSQL = " select a.invoice_gid, DATE_FORMAT(a.payment_date, '%d-%m-%Y') as payment_date,a.total_amount,a.approval_status,c.customer_name ," +
                    " concat(b.customer_contactperson, ' / ', b.customer_contactnumber, ' / ', b.customer_email) as contact from rbl_trn_tpayment a " +
                    " left join rbl_trn_tinvoice b on b.invoice_gid = a.invoice_gid " +
                    " left join crm_mst_tcustomer c on c.customer_gid = b.customer_gid " +
                    " where date_format(a.payment_date,'%M') ='" + month + "' and year(a.payment_date)='" + year + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetReceiptDetailSummary = new List<GetReceiptDetailSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetReceiptDetailSummary.Add(new GetReceiptDetailSummary
                        {
                            invoice_gid = (dt["invoice_gid"].ToString()),
                            payment_date = (dt["payment_date"].ToString()),
                            customer_name = (dt["customer_name"].ToString()),
                            approval_status = (dt["approval_status"].ToString()),
                            total_amount = (dt["total_amount"].ToString()),
                            contact = (dt["contact"].ToString()),
                        });
                        values.GetReceiptDetailSummary = GetReceiptDetailSummary;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Detail Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetReceiptReportForLastSixMonthsSearch(MdlSmrRptInvoiceReport values, string from_date, string to_date)
        {
            try
            {
                if (from_date == null && to_date == null)
                {
                    msSQL = " SELECT  DATE_FORMAT(a.payment_date, '%M %Y') AS month_year,COUNT(a.payment_gid) AS invoicecount,sum(a.total_amount) AS paymentamount,DATE_FORMAT(a.payment_date, '%M') AS month,DATE_FORMAT(a.payment_date, '%Y') AS year " +
                            " FROM rbl_trn_tpayment a WHERE a.payment_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) " +
                            " GROUP BY DATE_FORMAT(a.payment_date, '%M %Y')" +
                            " ORDER BY a.payment_date DESC ";
                }
                else
                {
                    msSQL = " SELECT  DATE_FORMAT(a.payment_date, '%M %Y') AS month_year,COUNT(a.payment_gid) AS invoicecount,sum(a.total_amount) AS paymentamount,DATE_FORMAT(a.payment_date, '%M') AS month,DATE_FORMAT(a.payment_date, '%Y') AS year " +
                       " FROM rbl_trn_tpayment a " +
                       "  where a.payment_date between '" + from_date + "' and '" + to_date + "'" +
                       " GROUP BY DATE_FORMAT(a.payment_date, '%M %Y')" +
                       " ORDER BY a.payment_date DESC ";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetReceiptForLastSixMonths_List = new List<GetReceiptForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetReceiptForLastSixMonths_List.Add(new GetReceiptForLastSixMonths_List
                        {
                            month_year = (dt["month_year"].ToString()),
                            year = (dt["year"].ToString()),
                            month = (dt["month"].ToString()),
                            paymentamount = (dt["paymentamount"].ToString()),
                            invoicecount = (dt["invoicecount"].ToString()),
                        });
                        values.GetReceiptForLastSixMonths_List = GetReceiptForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
