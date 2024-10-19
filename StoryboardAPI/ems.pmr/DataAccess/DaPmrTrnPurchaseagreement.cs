using ems.utilities.Functions;
using ems.utilities.Models;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using ems.pmr.Models;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using static System.Drawing.ImageConverter;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using CrystalDecisions.Shared.Json;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;
using Microsoft.SqlServer.Server;

namespace ems.pmr.DataAccess
{
    public class DaPmrTrnPurchaseagreement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        OdbcDataReader odbcdr;
        finance_cmnfunction objfincmn = new finance_cmnfunction();
        string lspop_server, lspop_mail, lspop_password, lscompany, lscompany_code;
        string msINGetGID, msGet_att_Gid, msenquiryloggid;
        string lspath, lspath1, lspath2, lsqty_ordered, mail_path, mail_filepath, pdf_name = "";
        HttpPostedFile httpPostedFile;
        string msSQL, mssql = string.Empty;
        OdbcDataReader objOdbcDataReader, objodbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable mail_datatable, dt_datatable;
        DataSet ds_dataset;
        string msEmployeeGID, msINGetREGID, mspGetGID, msGetagreementGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, lstaxpercentage, msPOGetGID, msPO1GetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lspop_port;
        MailMessage message = new MailMessage();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string company_logo_path, authorized_sign_path;
        Image branch_logo, company_logo, DataColumn14;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();
        string FileExtensionname;
        int lsproduct_price, lsInvoiceqty_billed, lsqty_invoice;
        string lsuom_gid, lsproduct_gid, lsproductgroup_name, lsproductgroup_code, lsDiscount_Percentage
            , lsTax_Amount3, lsTax_Percentage, lsTax_Percentage2, lsTax_Percentage3, lsTax_name, lsTax_name2, lsTax_name3
           , lsTax1_gid, lsTax2_gid, lsTax3_gid, lscurrency_taxamount2,
           lscurrency_taxamount3;
        string tax_percentage2, tax_percentage1, tax_gid2, tax_gid1, lsproductprice, lsnet_amount, expected_date, agreement_date, lsflag;
        string lsproductgid, lsproductuom_gid, lsproduct_name, lsproductuom_name, lsproducttype_name, lsorder_type, qty, cost_price, lsdiscountpercentage, lsdiscountamount, currencyexchange_gid, tax_amount1, tax_amount2, tax_name1, tax_name2,
            tax1, tax2, tax3, tax_amount, lspurchaseorder_from, mrp_price, taxsegment_name, tax_percentage, tax_gid, tax_name, taxsegment_gid, vendor_code, vendor_companyname, contactperson_name, contact_telephonenumber, vendor_address;
        private List<GetTaxSegmentList1> allTaxSegmentsList;
        string lsgrn_gid, lsmode_despatch, lsgrndtl_gid, lspurchaseorder_gid, lspurchaseorderdtl_gid
          , lsproduct_code, lsproductuom_code,
          lstPO_IV_flag;
        string parsed_Date; string invoice_Date;
        double productprice, totalqty, totalamount, taxpercentage1, taxamount1, taxpercentage2, taxamount2, producttotal;
        decimal lsTax_Amount, lsTax_Amount2, lscurrency_taxamount1, lsexchange_rate, lscurrency_unitprice, lscurrency_discountamount, lsDiscount_Amount;
        decimal delsqty_invoice, lsGRN_Sum;
        string lsinvoice_status, lstGRN_IV_flag, lsbranch, lsinvoiceref_flag, lsinv_ref_no, lsvendoraddress, lsvendor_gid, lsvendor_contact, lspayment_terms;


        public void DaGetPurchaseagreementOrderSummary(MdlPmrTrnPurchaseagreement values)
        {
            try
            {
                msSQL = " SELECT a.renewal_gid, a.purchaseorder_gid AS order_agreement_gid, DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date, " +
                        " c.vendor_companyname AS customer_name,CONCAT(c.contactperson_name, ' / ', c.contact_telephonenumber, ' / ', c.email_id) AS contact," +
                        " b.total_amount AS Grandtotal, a.renewal_type,DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date  FROM pmr_trn_trenewal a " +
                        " LEFT JOIN pmr_trn_tpurchaseorder b ON b.purchaseorder_gid = a.purchaseorder_gid LEFT JOIN acp_mst_tvendor c ON a.vendor_gid = c.vendor_gid " +
                        " WHERE a.renewal_status NOT IN ('Closed', 'Dropped') AND a.renewal_type = 'Purchase' AND a.renewal_flag = 'Y' AND b.purchaseorder_status = 'PO Approved' UNION " +
                        "  SELECT a.renewal_gid,a.agreement_gid AS order_agreement_gid, DATE_FORMAT(b.renewal_date, '%d-%m-%Y') AS renewal_date,c.vendor_companyname AS customer_name, " +
                        " CONCAT(c.contactperson_name, ' / ', c.contact_telephonenumber, ' / ', c.email_id) AS contact, b.Grandtotal AS Grandtotal,a.renewal_type,DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date  FROM pmr_trn_trenewal a " +
                        " LEFT JOIN pbl_trn_tagreement b ON b.agreement_gid = a.agreement_gid LEFT JOIN acp_mst_tvendor c ON a.vendor_gid = c.vendor_gid " +
                        " WHERE a.renewal_status NOT IN ('Closed', 'Dropped') AND a.renewal_type = 'Agreement' order by renewal_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpurchaseagreementorder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpurchaseagreementorder_list
                        {

                            renewal_date = dt["renewal_date"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            order_agreement_gid = dt["order_agreement_gid"].ToString(),
                            renewal_type = dt["renewal_type"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            renewal_gid = dt["renewal_gid"].ToString(),
                            agreement_gid = dt["order_agreement_gid"].ToString(),
                        });
                        values.Getpurchaseagreementorder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }

        public void DaPostProductAdd(string user_gid, PostProduct_list values)
        {
            msGetGid = objcmnfunctions.GetMasterGID("PODC");


            msSQL = " SELECT a.productuom_gid, a.product_gid, a.product_name, b.productuom_name,c.producttype_name,a.producttype_gid FROM pmr_mst_tproduct a " +
                 " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                 "  LEFT JOIN pmr_mst_tproducttype c ON c.producttype_gid = a.producttype_gid " +
                 " WHERE product_gid = '" + values.product_name + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows == true)
            {

                lsproductgid = objOdbcDataReader["product_gid"].ToString();
                lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();
                lsproduct_name = objOdbcDataReader["product_name"].ToString();
                lsproductuom_name = objOdbcDataReader["productuom_name"].ToString();
                lsproducttype_name = objOdbcDataReader["producttype_name"].ToString();
            }
            if (lsproducttype_name == "Services")
            {
                msSQL = "INSERT INTO pmr_tmp_tpurchaseorder ( " +
              "tmppurchaseorderdtl_gid, " +
              "tmppurchaseorder_gid, " +
              "qty_poadjusted, " +
              "product_gid, " +
              "product_code, " +
              "product_name, " +
              "display_field, " +
              "productuom_name, " +
              "uom_gid, " +
              "qty_ordered, " +
              "product_price, " +
              "discount_percentage, " +
              "discount_amount, " +
              "taxsegment_gid, " +
              "tax1_gid, " +
              "tax2_gid, " +
              "tax3_gid, " +
              "tax_name, " +
              "tax_name2, " +
              "tax_name3, " +
              "tax_percentage, " +
              "tax_percentage2, " +
              "tax_percentage3, " +
              "tax_amount, " +
              "tax_amount2, " +
              "tax_amount3, " +
              "created_by, " +
              "producttotal_price" +
              ") VALUES ( " +
              "'" + msGetGid + "', " +
              "'', " +
              "'0.00', " +
              "'" + lsproductgid + "'," +
              "'" + values.product_code + "',";
              if (!string.IsNullOrEmpty(values.product_remarks) && values.product_remarks.Contains("'"))
                {
                    msSQL += "'" + values.product_remarks.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.product_remarks + "', ";
                }
                if (!string.IsNullOrEmpty(values.product_remarks) && values.product_remarks.Contains("'"))
                {
                    msSQL += "'" + values.product_remarks.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.product_remarks + "', ";
                }
                
              msSQL += "'" + lsproductuom_name + "'," +
              "'" + lsproductuom_gid + "'," +
              "'" + values.productquantity + "'," +
              "'" + values.unitprice + "',";
                if (string.IsNullOrEmpty(values.discountprecentage))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.discountprecentage + "', ";
                }
                msSQL += "'" + values.discount_amount + "', " +
                          " '" + values.taxsegment_gid + "', " +
                              " '" + values.taxgid1 + "', " +
                              " '" + values.taxgid2 + "', " +
                              " '" + values.taxgid3 + "', " +
                              " '" + values.taxname1 + "', " +
                              " '" + values.taxname2 + "', " +
                              " '" + values.taxname3 + "', ";

                if (string.IsNullOrEmpty(values.taxprecentage1))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxprecentage1 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxprecentage2))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxprecentage2 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxprecentage3))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxprecentage3 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxamount1))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxamount1 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxamount2))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxamount2 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxamount3))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxamount3 + "', ";
                }

                msSQL += "'" + user_gid + "', " +
                         "'" + values.producttotal_amount + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            }
            else
            {

                msSQL = "INSERT INTO pmr_tmp_tpurchaseorder ( " +
              "tmppurchaseorderdtl_gid, " +
              "tmppurchaseorder_gid, " +
              "qty_poadjusted, " +
              "product_gid, " +
              "product_code, " +
              "product_name, " +
              "display_field, " +
              "productuom_name, " +
              "uom_gid, " +
              "qty_ordered, " +
              "product_price, " +
              "discount_percentage, " +
              "discount_amount, " +
              "taxsegment_gid, " +
              "tax1_gid, " +
              "tax2_gid, " +
              "tax3_gid, " +
              "tax_name, " +
              "tax_name2, " +
              "tax_name3, " +
              "tax_percentage, " +
              "tax_percentage2, " +
              "tax_percentage3, " +
              "tax_amount, " +
              "tax_amount2, " +
              "tax_amount3, " +
              "created_by, " +
              "producttotal_price" +
              ") VALUES ( " +
              "'" + msGetGid + "', " +
              "'', " +
              "'0.00', " +
              "'" + lsproductgid + "'," +
              "'" + values.product_code + "'," +
              "'" + lsproduct_name + "',";
                //"'" + values.product_remarks.Trim().Replace("'", "") + "',"+
                if (!string.IsNullOrEmpty(values.product_remarks) && values.product_remarks.Contains("'"))
                {
                    msSQL += "'" + values.product_remarks.Replace("'", "") + "',";
                }
                else
                {
                    msSQL += "'" + values.product_remarks + "', ";
                }

                msSQL += "'" + lsproductuom_name + "'," +
                       "'" + lsproductuom_gid + "'," +
                       "'" + values.productquantity + "'," +
                       "'" + values.unitprice + "',";
                if (string.IsNullOrEmpty(values.discountprecentage))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.discountprecentage + "', ";
                }
                msSQL += "'" + values.discount_amount + "', " +
                          " '" + values.taxsegment_gid + "', " +
                              " '" + values.taxgid1 + "', " +
                              " '" + values.taxgid2 + "', " +
                              " '" + values.taxgid3 + "', " +
                              " '" + values.taxname1 + "', " +
                              " '" + values.taxname2 + "', " +
                              " '" + values.taxname3 + "', ";

                if (string.IsNullOrEmpty(values.taxprecentage1))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxprecentage1 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxprecentage2))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxprecentage2 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxprecentage3))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxprecentage3 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxamount1))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxamount1 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxamount2))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxamount2 + "', ";
                }

                if (string.IsNullOrEmpty(values.taxamount3))
                {
                    msSQL += "'0.00', ";
                }
                else
                {
                    msSQL += "'" + values.taxamount3 + "', ";
                }

                msSQL += "'" + user_gid + "', " +
                         "'" + values.producttotal_amount + "')";

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
        private DataTable GetTaxDetailsForProduct(string product_gid, string vendor_gid)
        {
            // Query tax segment details based on product_gid
            msSQL = " SELECT f.taxsegment_gid, d.taxsegment_gid, e.taxsegment_name, y.tax_prefix as tax_name, d.tax_gid, " +
                " CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, " +
                " d.tax_amount, a.mrp_price, a.cost_price, a.product_gid " +
                " FROM acp_mst_ttaxsegment2product d " +
                " LEFT JOIN acp_mst_ttax y ON y.tax_gid = d.tax_gid " +
                " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                " LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid " +
                " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid " +
                "WHERE a.product_gid = '" + product_gid + "' AND f.vendor_gid = '" + vendor_gid + "'";

            // Execute query to get tax segment details
            return objdbconn.GetDataTable(msSQL);
        }
        public void DaProductSummary(string user_gid, string vendor_gid, MdlPmrTrnPurchaseagreement values, string smryproduct_gid)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;
                msSQL = "SELECT a.tmppurchaseorderdtl_gid, a.tmppurchaseorder_gid, a.product_gid, a.product_name, a.product_code, b.productgroup_gid, " +
                    "FORMAT(a.qty_ordered, 2) AS qty_ordered, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                    "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price, " +
                    "a.discount_percentage, FORMAT(a.discount_amount, 2) AS discount_amount, " +
                    "a.tax_percentage, FORMAT(a.tax_amount, 2) AS tax_amount, FORMAT(a.product_total, 2) AS product_total, a.needby_date, " +
                    "a.type, a.uom_gid, a.display_field, " +
                    "a.purchaserequisitiondtl_gid, a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2, " +
                    "a.tax_percentage3,CASE  WHEN a.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(a.tax_amount2, 2) END AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3, " +
                    "a.tax1_gid, a.tax2_gid, a.tax3_gid, a.taxsegment_gid , " +
                    "  concat(a.tax_name,'\n',', ',a.tax_name2 ) as taxname_detail, concat(a.tax_amount,'\n',', ',a.tax_amount2 ) as taxamt_detail " +
                    "FROM pmr_tmp_tpurchaseorder a " +
                    "LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                    "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
                    "LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid " +
                    "WHERE a.created_by = '" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productsummarylist>();
                var getGetTaxSegmentList = new List<GetTaxSegmentList1>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["producttotal_price"].ToString());
                        grandtotal += double.Parse(dt["producttotal_price"].ToString());
                        getModuleList.Add(new productsummarylist
                        {
                            tmppurchaseorderdtl_gid = dt["tmppurchaseorderdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            qty = dt["qty_ordered"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            //taxamount1 = dt["taxamt_detail"].ToString(),
                            //tax_name1 = dt["taxname_detail"].ToString(),
                            taxamount1 = dt["tax_amount"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            taxamount2 = dt["tax_amount2"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            product_total = dt["producttotal_price"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                        });

                        if (!string.IsNullOrEmpty(vendor_gid) && vendor_gid != "undefined" && vendor_gid != "0")
                        {
                            // Query tax segment details based on product_gid
                            string product_gid = dt["product_gid"].ToString();
                            DataTable taxSegmentDataTable = GetTaxDetailsForProduct(product_gid, vendor_gid);

                            // Add tax segment details to the list
                            foreach (DataRow taxSegmentRow in taxSegmentDataTable.Rows)
                            {
                                getGetTaxSegmentList.Add(new GetTaxSegmentList1
                                {
                                    taxsegment_gid = taxSegmentRow["taxsegment_gid"].ToString(),
                                    product_gid = taxSegmentRow["product_gid"].ToString(),
                                    taxsegment_name = taxSegmentRow["taxsegment_name"].ToString(),
                                    tax_name = taxSegmentRow["tax_name"].ToString(),
                                    tax_percentage = taxSegmentRow["tax_percentage"].ToString(),
                                    tax_gid = taxSegmentRow["tax_gid"].ToString(),
                                    mrp_price = taxSegmentRow["mrp_price"].ToString(),
                                    cost_price = taxSegmentRow["cost_price"].ToString(),
                                    tax_amount = taxSegmentRow["tax_amount"].ToString(),
                                });
                            }
                        }
                    }

                    // Set product summary and tax segment details to values
                    values.productsummarylist = getModuleList;
                    values.GetTaxSegmentList1 = getGetTaxSegmentList;
                }

                dt_datatable.Dispose();
                values.grand_total = grand_total;
                values.grandtotal = grandtotal;
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
        public void DaProductSubmit(string user_gid, GetViewagreementOrder values)
        {
            try
            {

                int lsfreight = 0;
                int lsinsurance = 0;
                string uiDateStr = values.agreement_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string agreement_date = uiDate.ToString("yyyy-MM-dd");

                string uiDateStr1 = values.renewal_date;
                DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string renewal_date = uiDate1.ToString("yyyy-MM-dd");
                string lstax_gid;

                msGetGID = objcmnfunctions.GetMasterGID("BRLP");
                msGetagreementGID = objcmnfunctions.GetMasterGID("VSOP");

                msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "' ";
                string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select poapproval_flag from adm_mst_tcompany";
                string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                if (string.IsNullOrEmpty(values.tax_name4))
                {
                    lstaxpercentage = "0";

                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                    lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                }

                msSQL = " insert into pbl_trn_tagreement (" +
                         " agreement_gid, " +
                         " agreement_referencenumber, " +
                         " agreement_date," +
                         " branch_gid, " +
                         " customer_gid, " +
                         " customer_address, " +
                         " shipping_address, " +
                         " created_by, " +
                         " mode_despatch, " +
                         " agreement_remarks, " +
                         " agreement_referenceno1, " +
                         " payment_days, " +
                         " delivery_days, " +
                         " Grandtotal, " +
                         " grandtotal_l, " +
                         " termsandconditions, " +
                         " agreement_status, " +
                         " agreement_type, " +
                         " addon_charge, " +
                         " addon_charge_l, " +
                         " additional_discount, " +
                         " additional_discount_l, " +
                         " currency_code, " +
                         " currency_gid, " +
                         " exchange_rate, " +
                         " renewal_date, " +
                         " renewal_gid, " +
                         " renewal_description, " +
                         " netamount, " +
                         " addon_amount," +
                         " freightcharges," +
                         " discount_amount," +
                         " tax_gid," +
                         " tax_percentage," +
                         " tax_amount," +
                         " roundoff, " +
                         " created_date," +
                         " taxsegment_gid " +
                         " ) values (" +
                         "'" + msGetagreementGID + "'," +
                         "'" + msGetagreementGID + "'," +
                         "'" + agreement_date + "', " +
                         "'" + values.branch_name + "'," +
                         "'" + values.vendor_companyname + "',";
                if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                {
                    msSQL += "'" + values.address1.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.address1 + "', ";
                }

                //"'" + values.shipping_address.Trim().Replace("'", "") + "'," +

                if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                {
                    msSQL += "'" + values.shipping_address.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.shipping_address + "', ";
                }
                msSQL += "'" + user_gid + "',";
                
                if (!string.IsNullOrEmpty(values.mode_despatch) && values.mode_despatch.Contains("'"))
                {
                    msSQL += "'" + values.mode_despatch.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.mode_despatch + "', ";
                }

                if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                {
                    msSQL += "'" + values.po_covernote.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.po_covernote + "', ";
                }
                msSQL += "'" + msGetagreementGID + "',";
                
                if (!string.IsNullOrEmpty(values.payment_terms) && values.payment_terms.Contains("'"))
                {
                    msSQL += "'" + values.payment_terms.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.payment_terms + "', ";
                }

                if (!string.IsNullOrEmpty(values.delivery_terms) && values.delivery_terms.Contains("'"))
                {
                    msSQL += "'" + values.delivery_terms.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.delivery_terms + "', ";
                }
                msSQL += "'" + values.grandtotal + "'," +
                         "'" + values.grandtotal + "',";
                
                if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                {
                    msSQL += "'" + values.template_content.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.template_content + "', ";
                }


                msSQL += "'PO Approved'," +
                         "'Agreement'," +
                         "'" + values.addoncharge + "'," +
                         "'" + values.addoncharge + "',";
                if (string.IsNullOrEmpty(values.additional_discount))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.additional_discount + "',";
                }
                if (string.IsNullOrEmpty(values.additional_discount))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.additional_discount + "',";
                }
                msSQL += "'" + lscurrency_code + "'," +
                         "'" + values.currency_code + "'," +
                         "'" + values.exchange_rate + "'," +
                         "'" + renewal_date + "', " +
                         "'" + msGetGID + "',";
                if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                {
                    msSQL += "'" + values.po_covernote.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "'" + values.po_covernote + "', ";
                }
                msSQL += "'" + values.totalamount + "'," +
                         "'" + values.addoncharge + "'," +
                         "'" + values.freightcharges + "',";
                if (string.IsNullOrEmpty(values.additional_discount))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.additional_discount + "',";
                }
                msSQL += "'" + values.tax_name4 + "'," +
                "'" + lstaxpercentage + "',";

                if (string.IsNullOrEmpty(values.tax_amount4))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_amount4 + "',";
                }
                if (values.roundoff == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.roundoff + "',";
                }
                msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
                msSQL += "'" + values.taxsegment_gid + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = " Some Error Occurred While Inserting Agreement order Details";
                    return;
                }
                else
                {
                    msSQL = " Insert into pmr_trn_trenewal ( " +
                            " renewal_gid, " +
                            " vendor_gid," +
                            " renewal_date, " +
                            " agreement_gid, " +
                            " created_by, " +
                            " renewal_type, " +
                            " created_date) " +
                           " Values ( " +
                             "'" + msGetGID + "'," +
                             "'" + values.vendor_companyname + "'," +
                             "'" + renewal_date + "'," +
                             "'" + msGetagreementGID + "'," +
                             "'" + user_gid + "'," +
                             "'Agreement'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                    double grandtotal = 0.00;
                    msSQL = "SELECT a.tmppurchaseorderdtl_gid, a.tmppurchaseorder_gid, a.product_gid, a.product_name, a.product_code, b.productgroup_gid, " +
                        "FORMAT(a.qty_ordered, 2) AS qty_ordered, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                        "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price, " +
                        "a.discount_percentage, FORMAT(a.discount_amount, 2) AS discount_amount, " +
                        "a.tax_percentage, FORMAT(a.tax_amount, 2) AS tax_amount, FORMAT(a.product_total, 2) AS product_total, a.needby_date, " +
                        "a.type, a.uom_gid, a.display_field, " +
                        "a.purchaserequisitiondtl_gid, a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2, " +
                        "a.tax_percentage3, FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3, " +
                        "a.tax1_gid, a.tax2_gid, a.tax3_gid, a.taxsegment_gid " +
                        "FROM pmr_tmp_tpurchaseorder a " +
                        "LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                        "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
                        "LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid " +
                        "WHERE a.created_by = '" + user_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msPOGetGID = objcmnfunctions.GetMasterGID("VSDC");

                            msSQL = " insert into pbl_trn_tagreementdtl (" +
                                        " agreementdtl_gid, " +
                                        " agreement_gid, " +
                                        " product_gid, " +
                                        " product_code, " +
                                        " product_name, " +
                                        " productuom_name, " +
                                        " uom_gid, " +
                                        " producttype_gid, " +
                                        " product_price, " +
                                        " producttotal_amount, " +
                                        " discount_percentage, " +
                                        " discount_amount, " +
                                        " tax_name, " +
                                        " tax_name2, " +
                                        " tax_name3, " +
                                        " tax1_gid, " +
                                        " tax2_gid, " +
                                        " tax3_gid, " +
                                        " qty_ordered, " +
                                        " product_price_L, " +
                                        " display_field_name," +
                                        " tax_amount1_L," +
                                        " tax_amount2_L," +
                                        " tax_amount3_L," +
                                         " tax_amount," +
                                        " tax_amount2," +
                                        " tax_amount3," +
                                        " tax_percentage," +
                                        " tax_percentage2," +
                                        " tax_percentage3," +
                                        " created_by," +
                                        " taxsegment_gid" +
                                        " )values ( " +
                                        "'" + msPOGetGID + "', " +
                                        "'" + msGetagreementGID + "'," +
                                        "'" + dt["product_gid"].ToString() + "', " +
                                        "'" + dt["product_code"].ToString() + "', " +
                                        "'" + dt["product_name"].ToString() + "', " +
                                        "'" + dt["productuom_name"].ToString() + "', " +
                                        "'" + dt["uom_gid"].ToString() + "', " +
                                        "'" + dt["type"].ToString() + "', " +
                                        "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                        "'" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                        "'" + dt["discount_percentage"].ToString().Replace(",", "") + "', ";
                            if (string.IsNullOrEmpty(dt["discount_amount"].ToString()))
                            {
                                msSQL += "'0.00',";

                            }
                            else
                            {
                                msSQL += "'" + dt["discount_amount"].ToString().Replace(",", "") + "',";
                            }
                            msSQL += "'" + dt["tax_name"].ToString() + "', " +
                            "'" + dt["tax_name2"].ToString() + "', " +
                            "'" + dt["tax_name3"].ToString() + "', " +
                            "'" + dt["tax1_gid"].ToString() + "', " +
                            "'" + dt["tax2_gid"].ToString() + "', " +
                            "'" + dt["tax3_gid"].ToString() + "', " +
                            "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                            "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                            "'" + dt["display_field"].ToString() + "'," +
                            "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                              "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_percentage"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_percentage2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_percentage3"].ToString().Replace(",", "") + "'," +
                            "'" + user_gid + "'," +
                            "'" + dt["taxsegment_gid"].ToString() + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                    }

                    if (mnResult != 0)

                    {
                        values.status = true;
                        values.message = "Agreement Order Raised Successfully!";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding product in Agreement Order!";


                    }

                    msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    dt_datatable.Dispose();
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding product in Purchase Order!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetViewagreementSummary(string renewal_gid, MdlPmrTrnPurchaseagreement values)
        {
            try
            {
                msSQL = "SELECT purchaseorder_gid FROM pmr_trn_trenewal WHERE renewal_gid = '" + renewal_gid + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                string lspurchaseorder_gid = null;

                if (objodbcDataReader.HasRows)
                {
                    lspurchaseorder_gid = objodbcDataReader["purchaseorder_gid"].ToString();
                }


                if (!string.IsNullOrEmpty(lspurchaseorder_gid))
                {

                    msSQL = " select i.producttotal_amount,a.purchaseorder_remarks as renewal_description,a.freight_terms as  delivery_terms,a.payment_terms,r.renewal_date, a.shipping_address,a.mode_despatch, DATE_FORMAT(a.purchaseorder_date, '%d-%m-%Y') AS Created_date,concat (a.purchaseorder_gid) as purchaseorder_gid,m.user_firstname as requested_by,\r\n " +
                            " b.gst_no,  b.email_id,b.contact_telephonenumber,b.contactperson_name,a.tax_amount as overalltax ,\r\n " +
                            " format(i.product_price_L,2) as product_price_L,  case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,\r\n " +
                            " w.address2,i.tax_name,i.tax_name2,i.tax_name3,  a.branch_gid,  date_format(r.renewal_date, '%d-%m-%y') as renewal_date ,\r\n " +
                            " a.created_by, format(a.total_amount, 2) as total_amount ,  concat(f.address1,f.postal_code) as branch_add1,   \r\n " +
                            " concat( w.address1 ,' ', w.address2, ' ', w.city, '  ', w.postal_code) AS bill_to, a.termsandconditions,\r\n" +
                            " b.vendor_companyname, g.user_firstname as approved_by, i.qty_ordered as qyt_unit ,concat(c.user_firstname, ' - ', e.department_name) as user_firstname,  d.employee_emailid, d.employee_mobileno,\r\n " +
                            " f.branch_name, format(a.discount_amount, 2) as discount_amount, format(a.tax_percentage, 2) as tax_percentage, \r\n " +
                            " format(a.addon_amount, 2) as addon_amount,format(a.roundoff, 2) as roundoff,   a.payment_days,a.tax_gid,a.delivery_days,\r\n " +
                            " format(a.freightcharges,2) as freightcharges,format(a.discount_amount,2) as additional_discount, i.product_gid,\r\n " +
                            " i.product_price, i.qty_ordered, format(i.discount_percentage, 2) as discount_percentage , \r\n " +
                            " format(i.discount_amount, 2) discount_amount1 ,  format(i.tax_percentage, 2) as tax_percentage, \r\n " +
                            " format(i.tax_percentage2, 2) as tax_percentage2,  format(i.tax_percentage3, 2) as tax_percentage3, format(i.tax_amount, 2) as tax_amount, \r\n " +
                            " CASE  WHEN i.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(i.tax_amount2, 2) END AS tax_amount2,  \r\n " +
                            " format(i.tax_amount3, 2) as tax_amount3,  i.product_remarks, format((qty_ordered * i.product_price), 2) as product_totalprice, \r\n " +
                            " format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as product_total,\r\n " +
                            " i.product_code, (i.product_name) as product_name,format(a.netamount, 2) as netamount, k.productgroup_name,i.productuom_name,i.display_field_name, a.currency_code,\r\n " +
                            " i.tax1_gid,y.tax_name as overalltaxname  , concat(i.tax_name,' ,',i.tax_name2) as taxesname, concat(i.tax_amount,' , ',i.tax_amount2) as taxesamt from pmr_trn_tpurchaseorder a \r\n " +
                            " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid  left join adm_mst_taddress w on b.address_gid = w.address_gid  \r\n " +
                            " left join adm_mst_tuser c on c.user_gid = a.created_by left join hrm_mst_temployee d on d.user_gid = c.user_gid \r\n " +
                            " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid  left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid \r\n " +
                            " left join adm_mst_tuser g on g.user_gid = a.approved_by left join pmr_trn_tpurchaseorderdtl i ON i.purchaseorder_gid = a.purchaseorder_gid  \r\n " +
                            " left join acp_mst_ttax y on y.tax_gid = a.tax_gid left join pmr_mst_tproduct j on i.product_gid = j.product_gid \r\n " +
                            " left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid \r\n " +
                            " left join adm_mst_tuser m on m.user_gid = a.created_by left join crm_trn_tcurrencyexchange z on a.currency_code = z.currencyexchange_gid\r\n " +
                            " LEFT JOIN  pmr_trn_trenewal r ON a.purchaseorder_gid = r.purchaseorder_gid  " +
                            " where r.renewal_gid = '" + renewal_gid + "'";
                }
                else
                {
                    msSQL = "  select i.producttotal_amount,a.renewal_description,r.renewal_date,a.delivery_days as delivery_terms,a.payment_days as payment_terms,a.mode_despatch, a.shipping_address, DATE_FORMAT(a.agreement_date, '%d-%m-%Y') AS Created_date,concat (a.agreement_gid) as purchaseorder_gid,m.user_firstname as requested_by,\r\n " +
                            " b.gst_no,  b.email_id,b.contact_telephonenumber,b.contactperson_name,a.tax_amount as overalltax ,\r\n  " +
                            "  format(i.product_price_L,2) as product_price_L,  case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,\r\n " +
                            "  w.address2,i.tax_name,i.tax_name2,i.tax_name3,  a.branch_gid,  date_format(r.renewal_date, '%d-%m-%y') as renewal_date ,\r\n " +
                            "  a.created_by, format(a.Grandtotal, 2) as total_amount ,  concat(f.address1,f.postal_code) as branch_add1,   \r\n " +
                            "  concat( w.address1 ,' ', w.address2, ' ', w.city, '  ', w.postal_code) AS bill_to, a.termsandconditions,\r\n " +
                            "  b.vendor_companyname, g.user_firstname as approved_by, i.qty_ordered as qyt_unit ,concat(c.user_firstname, ' - ', e.department_name) as user_firstname,  d.employee_emailid, d.employee_mobileno,\r\n " +
                            "  f.branch_name, format(a.discount_amount, 2) as discount_amount, format(a.tax_percentage, 2) as tax_percentage, \r\n" +
                            "  format(a.addon_amount, 2) as addon_amount,format(a.roundoff, 2) as roundoff,   a.payment_days,a.tax_gid,a.delivery_days,\r\n " +
                            "  format(a.freightcharges,2) as freightcharges,format(a.discount_amount,2) as additional_discount, i.product_gid,\r\n " +
                            "   i.product_price, i.qty_ordered, format(i.discount_percentage, 2) as discount_percentage , \r\n" +
                            "   format(i.discount_amount, 2) discount_amount1 ,  format(i.tax_percentage, 2) as tax_percentage, \r\n" +
                            "   format(i.tax_percentage2, 2) as tax_percentage2,  format(i.tax_percentage3, 2) as tax_percentage3, format(i.tax_amount, 2) as tax_amount, \r\n" +
                            "  CASE  WHEN i.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(i.tax_amount2, 2) END AS tax_amount2,  \r\n " +
                            "  format(i.tax_amount3, 2) as tax_amount3,  i.product_remarks, format((qty_ordered * i.product_price), 2) as product_totalprice, \r\n " +
                            "  format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as product_total,\r\n " +
                            "  i.product_code, (i.product_name) as product_name,format(a.netamount, 2) as netamount, k.productgroup_name,i.productuom_name,i.display_field_name, a.currency_code,\r\n " +
                            "  i.tax1_gid,y.tax_name as overalltaxname  , concat(i.tax_name,' ,',i.tax_name2) as taxesname, concat(i.tax_amount,' , ',i.tax_amount2) as taxesamt from pbl_trn_tagreement a \r\n " +
                            "   left join acp_mst_tvendor b on a.customer_gid = b.vendor_gid  left join adm_mst_taddress w on b.address_gid = w.address_gid  \r\n" +
                            "   left join adm_mst_tuser c on c.user_gid = a.created_by left join hrm_mst_temployee d on d.user_gid = c.user_gid \r\n" +
                            "  left join hrm_mst_tdepartment e on e.department_gid = d.department_gid  left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid \r\n " +
                            "  left join adm_mst_tuser g on g.user_gid = a.approved_by left join pbl_trn_tagreementdtl i ON i.agreement_gid = a.agreement_gid  \r\n " +
                            "  left join acp_mst_ttax y on y.tax_gid = a.tax_gid left join pmr_mst_tproduct j on i.product_gid = j.product_gid \r\n " +
                            "  left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid \r\n " +
                            "   left join adm_mst_tuser m on m.user_gid = a.created_by left join crm_trn_tcurrencyexchange z on a.currency_code = z.currencyexchange_gid\r\n" +
                            " LEFT JOIN  pmr_trn_trenewal r ON a.agreement_gid = r.agreement_gid  " +
                            " where r.renewal_gid = '" + renewal_gid + "'";

                }
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetViewPurchaseagreement>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetViewPurchaseagreement
                        {
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            Created_date = dt["Created_date"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            tax_number = dt["gst_no"].ToString(),
                            address2 = dt["address2"].ToString(),
                            requested_by = dt["requested_by"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            display_field_name = dt["display_field_name"].ToString(),
                            qyt_unit = dt["qyt_unit"].ToString(),
                            product_price_L = dt["product_price_L"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount1 = dt["discount_amount1"].ToString(),
                            //tax_name = dt["taxesname"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            netamount = dt["netamount"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            overalltaxname = dt["overalltaxname"].ToString(),
                            overalltax = dt["overalltax"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            bill_to = dt["bill_to"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            producttotal_amount = dt["producttotal_amount"].ToString(),
                            renewal_description = dt["renewal_description"].ToString(),
                            delivery_terms = dt["delivery_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString()



                        });

                        values.GetViewPurchaseagreement = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetpurchaseagreementdelete(string renewal_gid, MdlPmrTrnPurchaseagreement values)
        {
            try
            {

                msSQL = " delete from pmr_trn_trenewal " +
                    " where renewal_gid='" + renewal_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Agreement Deleted successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Agreement";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding renewal!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetPurchaseInvoicesummary(string renewal_gid, MdlPmrTrnPurchaseagreement values)
        {
            try
            {
                msSQL = "SELECT purchaseorder_gid FROM pmr_trn_trenewal WHERE renewal_gid = '" + renewal_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                string lspurchaseorder_gid = null;
                if (objOdbcDataReader.HasRows)
                {
                    lspurchaseorder_gid = objOdbcDataReader["purchaseorder_gid"].ToString();
                }
                if (!string.IsNullOrEmpty(lspurchaseorder_gid))
                {
                    msSQL = " SELECT x.tax_prefix,a.total_amount_L, CONCAT(a.purchaseorder_gid) AS purchaseorder_gid,m.user_firstname AS requested_by,a.mode_despatch,a.po_type," +
                        " a.requested_details,a.po_covernote,b.gst_no,b.email_id,b.contact_telephonenumber,b.contactperson_name,a.tax_amount AS overalltax, " +
                        " a.purchaserequisition_gid,a.purchaseorder_remarks,CASE WHEN a.exchange_rate IS NULL THEN '1' ELSE a.exchange_rate END AS exchange_rate," +
                        " w.address2,a.purchaserequisition_gid,a.quotation_gid,a.branch_gid,a.ship_via,a.payment_terms,a.delivery_location,a.freight_terms," +
                        " DATE_FORMAT(a.purchaseorder_date, '%d-%m-%y') AS purchaseorder_date, a.vendor_address,a.vendor_contact_person,a.created_by," +
                        " a.priority,a.priority_remarks,CASE WHEN a.priority = 'Y' THEN 'High' ELSE 'Low' END AS priority_n,a.currency_code," +
                        " CASE WHEN a.invoice_flag <> 'IV Pending' THEN a.invoice_flag  WHEN a.grn_flag <> 'GRN Pending' THEN a.grn_flag" +
                        " ELSE a.purchaseorder_flag END AS 'overall_status',a.approver_remarks,a.purchaseorder_reference,a.total_amount, " +
                        " CONCAT(f.address1, f.postal_code) AS branch_add1,CONCAT_WS(' ', w.address1,'\n', w.address2,'\n', w.city,'\n',w.postal_code) AS bill_to, CONCAT_WS('\n', f.address1, f.city,f.state, f.postal_code) AS shipping_address,a.currency_code AS currencycode," +
                        " a.vendor_emailid,a.vendor_faxnumber, a.vendor_contactnumber,a.termsandconditions,a.payment_term,b.vendor_companyname," +
                        " g.user_firstname AS approved_by,CONCAT(c.user_firstname, ' - ', e.department_name) AS user_firstname,d.employee_emailid," +
                        " d.employee_mobileno,f.branch_name,a.discount_amount,a.tax_percentage," +
                        " a.addon_amount,a.roundoff,CONCAT_WS(' - ', h.costcenter_name, h.costcenter_gid) AS costcenter_name, " +
                        " h.budget_available,h.costcenter_gid,a.payment_days,a.tax_gid,a.delivery_days,a.freightcharges,a.buybackorscrap,a.manualporef_no," +
                        " a.packing_charges,a.insurance_charges,a.discount_amount as additional_discount ," +
                        " CASE WHEN (SELECT SUM(g1.producttotal_amount) FROM pmr_trn_tgrndtl a1 LEFT JOIN pmr_trn_tgrn b1 ON a1.grn_gid = b1.grn_gid " +
                        " LEFT JOIN pmr_trn_tpurchaseorderdtl g1 ON a1.purchaseorderdtl_gid = g1.purchaseorderdtl_gid WHERE b1.grn_gid IN('PGNP2407229')) IS NULL " +
                        " THEN a.netamount ELSE(SELECT SUM(g1.producttotal_amount) FROM pmr_trn_tgrndtl a1 LEFT JOIN pmr_trn_tgrn b1 ON a1.grn_gid = b1.grn_gid  LEFT JOIN pmr_trn_tpurchaseorderdtl g1 ON a1.purchaseorderdtl_gid = g1.purchaseorderdtl_gid WHERE b1.grn_gid IN ('PGNP2407229')) END AS netamount " +
                        " FROM pmr_trn_tpurchaseorder a LEFT JOIN acp_mst_tvendor b ON a.vendor_gid = b.vendor_gid    " +
                        " LEFT JOIN adm_mst_taddress w ON b.address_gid = w.address_gid LEFT JOIN adm_mst_tuser c ON c.user_gid = a.created_by   " +
                        " LEFT JOIN hrm_mst_temployee d ON d.user_gid = c.user_gid  LEFT JOIN hrm_mst_tdepartment e ON e.department_gid = d.department_gid " +
                        " LEFT JOIN hrm_mst_tbranch f ON a.branch_gid = f.branch_gid LEFT JOIN adm_mst_tuser g ON g.user_gid = a.approved_by    LEFT JOIN acp_mst_ttax x ON x.tax_gid = a.tax_gid " +
                        " LEFT JOIN pmr_mst_tcostcenter h ON h.costcenter_gid = a.costcenter_gid LEFT JOIN adm_mst_tuser m ON m.user_gid = a.requested_by LEFT JOIN crm_trn_tcurrencyexchange z ON z.currencyexchange_gid = a.currency_code" +
                        " LEFT JOIN pmr_trn_trenewal r ON a.purchaseorder_gid = r.purchaseorder_gid" +
                        " where r.renewal_gid = '" + renewal_gid + "'";

                }
                else
                {
                    msSQL = " SELECT x.tax_prefix,a.grandtotal_l as total_amount_L , CONCAT(a.agreement_gid) AS purchaseorder_gid,a.mode_despatch,a.renewal_description AS po_covernote, " +
                       " b.gst_no,b.email_id,b.contact_telephonenumber,b.contactperson_name,a.tax_amount AS overalltax,a.renewal_description as purchaseorder_remarks, " +
                       " CASE WHEN a.exchange_rate IS NULL THEN '1' ELSE a.exchange_rate END AS exchange_rate, w.address2,a.branch_gid,a.payment_days as payment_terms ," +
                       " a.delivery_days freight_terms, DATE_FORMAT(a.agreement_date, '%d-%m-%y') AS purchaseorder_date, a.customer_address as vendor_address," +
                       " b.contactperson_name as vendor_contact_person,a.created_by,a.currency_code, a.agreement_referencenumber as purchaseorder_reference,a.grandtotal as total_amount ," +
                       " CONCAT(f.address1, f.postal_code) AS branch_add1,CONCAT_WS(' ', w.address1,'', w.address2,'', w.city,'',w.postal_code) AS bill_to, CONCAT_WS('', f.address1, f.city,f.state, f.postal_code) AS shipping_address,a.currency_code AS currencycode ," +
                       " b.email_id as vendor_emailid,b.contact_telephonenumber as vendor_contactnumber,a.termsandconditions,b.vendor_companyname, g.user_firstname AS approved_by,CONCAT(c.user_firstname, ' - ', e.department_name) AS user_firstname,d.employee_emailid, d.employee_mobileno,f.branch_name,a.discount_amount,a.tax_percentage, a.addon_amount,a.roundoff,a.payment_days,a.tax_gid,a.delivery_days ," +
                       " a.freightcharges, a.packing_charges,a.insurance_charges,a.discount_amount as additional_discount,(SELECT SUM(ad.producttotal_amount)  FROM pbl_trn_tagreementdtl ad LEFT JOIN pmr_trn_trenewal rd ON ad.agreement_gid = rd.agreement_gid  WHERE rd.agreement_gid = a.agreement_gid) AS netamount  FROM pbl_trn_tagreement a " +
                       " LEFT JOIN acp_mst_tvendor b ON a.customer_gid = b.vendor_gid LEFT JOIN adm_mst_taddress w ON b.address_gid = w.address_gid" +
                       " LEFT JOIN adm_mst_tuser c ON c.user_gid = a.created_by   LEFT JOIN hrm_mst_temployee d ON d.user_gid = c.user_gid " +
                       " LEFT JOIN hrm_mst_tdepartment e ON e.department_gid = d.department_gid  LEFT JOIN hrm_mst_tbranch f ON a.branch_gid = f.branch_gid " +
                       " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.approved_by LEFT JOIN acp_mst_ttax x ON x.tax_gid = a.tax_gid " +
                       " LEFT JOIN crm_trn_tcurrencyexchange z ON z.currencyexchange_gid = a.currency_code LEFT JOIN pmr_trn_trenewal r ON a.agreement_gid = r.agreement_gid " +
                       " where r.renewal_gid = '" + renewal_gid + "'";
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetinviocePO1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetinviocePO1
                        {
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            address2 = dt["address2"].ToString(),
                            delivery_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            currency_code = dt["currencycode"].ToString(),
                            po_covernote = dt["po_covernote"].ToString(),
                            netamount = dt["netamount"].ToString(),
                            total_amount_L = dt["total_amount_L"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            bill_to = dt["bill_to"].ToString(),
                            overalltax = dt["overalltax"].ToString(),
                            tax_prefix = dt["tax_prefix"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            renewal_gid = renewal_gid,
                        });

                        values.GetinviocePO1 = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              mssql + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetPurchaseInvoiceproduct(string renewal_gid, MdlPmrTrnPurchaseagreement values)
        {
            try
            {
                double grand_total = 0.00;
                msSQL = "SELECT purchaseorder_gid FROM pmr_trn_trenewal WHERE renewal_gid = '" + renewal_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                string lspurchaseorder_gid = null;
                if (objOdbcDataReader.HasRows)
                {
                    lspurchaseorder_gid = objOdbcDataReader["purchaseorder_gid"].ToString();
                }
                if (!string.IsNullOrEmpty(lspurchaseorder_gid))
                {
                    mssql = " select  concat(i.qty_ordered,' ',i.productuom_name) as qyt_unit ,n.qty_delivered, format(a.tax_percentage, 2) as tax_percentage, format(i.product_price_L,2) as product_price_L, format(a.addon_amount, 2) as addon_amount," +
                            " format(a.roundoff, 2) as roundoff,  concat_ws('-', h.costcenter_name, h.costcenter_gid) as costcenter_name,format(h.budget_available, 2) as budget_available," +
                            " h.costcenter_gid,  a.payment_days,a.tax_gid,a.delivery_days,format(a.freightcharges,2) as freightcharges,a.buybackorscrap,a.manualporef_no,  " +
                            " format(a.packing_charges, 2) as packing_charges, format(a.insurance_charges, 2) as insurance_charges ,format(a.discount_amount,2) as additional_discount,  " +
                            " i.purchaseorderdtl_gid, i.product_gid,  i.product_price, i.qty_ordered,i.needby_date, format(i.discount_percentage, 2) as discount_percentage ,  " +
                            " format(i.discount_amount, 2) discount_amount1 ,  format(i.tax_percentage, 2) as tax_percentage,  format(i.tax_percentage2, 2) as tax_percentage2,  " +
                            " format(i.tax_percentage3, 2) as tax_percentage3,  format(i.tax_amount, 2) as tax_amount,  format(i.tax_amount2, 2) as tax_amount2, " +
                            " format(i.tax_amount3, 2) as tax_amount3,i.taxseg_taxname1,i.taxseg_taxpercent1,format(i.taxseg_taxamount1,2) AS taxseg_taxamount1," +
                            " i.taxseg_taxname2,i.taxseg_taxpercent2,format(i.taxseg_taxamount2,2) AS taxseg_taxamount2,  i.qty_Received, i.qty_grnadjusted, " +
                            " i.taxseg_taxname3,i.taxseg_taxpercent3,format(i.taxseg_taxamount3,2) as taxseg_taxamount3,  i.product_remarks, " +
                            " format((qty_ordered * i.product_price), 2) as product_totalprice,  format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as product_total," +
                            " i.product_code, (i.product_name) as product_name,format(a.netamount, 2) as netamount, k.productgroup_name, i.productuom_name," +
                            " i.purchaseorder_gid,i.display_field_name, a.currency_code,a.poref_no,i.tax1_gid,y.tax_name as overalltaxname, " +
                            " FORMAT(i.tax_amount + i.tax_amount2, 2) AS overall_tax ,i.tax_name, i.tax_percentage, i.tax_name2, i.tax_percentage2 , i.tax_amount,i.tax_amount2 from pmr_trn_tpurchaseorder a  " +
                            " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid  " +
                            " left join adm_mst_taddress w on b.address_gid = w.address_gid  " +
                            " left join adm_mst_tuser c on c.user_gid = a.created_by  " +
                            " left join hrm_mst_temployee d on d.user_gid = c.user_gid  " +
                            " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid  " +
                            " left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid  " +
                            " left join adm_mst_tuser g on g.user_gid = a.approved_by  " +
                            " left join pmr_mst_tcostcenter h on h.costcenter_gid = a.costcenter_gid  " +
                            " left join pmr_trn_tpurchaseorderdtl i ON a.purchaseorder_gid = i.purchaseorder_gid  " +
                            " left join acp_mst_ttax y on y.tax_gid = a.tax_gid  " +
                            " left join pmr_mst_tproduct j on i.product_gid = j.product_gid  " +
                            " left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid  " +
                            " left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid  " +
                            " left join adm_mst_tuser m on m.user_gid = a.requested_by " +
                            "  left join pmr_trn_tgrndtl n on n.purchaseorderdtl_gid = i.purchaseorderdtl_gid " +
                            " left join pmr_trn_tgrn o on o.grn_gid = n.grn_gid " +
                            " LEFT JOIN pmr_trn_trenewal r ON a.purchaseorder_gid = r.purchaseorder_gid " +
                            " where r.renewal_gid = '" + renewal_gid + "'";
                }
                else
                {

                    mssql = " select  concat(i.qty_ordered,' ',i.productuom_name) as qyt_unit , format(a.tax_percentage, 2) as tax_percentage, " +
                    " format(i.product_price_L,2) as product_price_L, format(a.addon_amount, 2) as addon_amount, format(a.roundoff, 2) as roundoff," +
                    " a.payment_days,a.tax_gid,a.delivery_days,format(a.freightcharges,2) as freightcharges,   format(a.packing_charges, 2) as packing_charges, format(a.insurance_charges, 2) as insurance_charges ,format(a.discount_amount,2) as additional_discount,   i.agreementdtl_gid, i.product_gid,  i.product_price, i.qty_ordered,  " +
                    " format(i.discount_percentage, 2) as discount_percentage ,   format(i.discount_amount, 2) discount_amount1 ,  format(i.tax_percentage, 2) as tax_percentage,  format(i.tax_percentage2, 2) as tax_percentage2,  " +
                    " format(i.tax_percentage3, 2) as tax_percentage3,  format(i.tax_amount, 2) as tax_amount,  format(i.tax_amount2, 2) as tax_amount2,  format(i.tax_amount3, 2) as tax_amount3, i.product_remarks,  format((qty_ordered * i.product_price), 2) as product_totalprice,  " +
                    " format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as product_total, i.product_code, (i.product_name) as product_name,format(a.netamount, 2) as netamount, k.productgroup_name, i.productuom_name, i.agreement_gid as purchaseorder_gid ,i.display_field_name, a.currency_code,i.tax1_gid,y.tax_name as overalltaxname, " +
                    " FORMAT(i.tax_amount + i.tax_amount2, 2) AS overall_tax ,i.tax_name, i.tax_percentage, i.tax_name2, i.tax_percentage2 , i.tax_amount,i.tax_amount2 from pbl_trn_tagreement a   left join acp_mst_tvendor b on a.customer_gid = b.vendor_gid  " +
                    " left join adm_mst_taddress w on b.address_gid = w.address_gid   left join adm_mst_tuser c on c.user_gid = a.created_by " +
                    " left join hrm_mst_temployee d on d.user_gid = c.user_gid   left join hrm_mst_tdepartment e on e.department_gid = d.department_gid" +
                    " left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid   left join adm_mst_tuser g on g.user_gid = a.approved_by " +
                    " left join pbl_trn_tagreementdtl i ON a.agreement_gid = i.agreement_gid left join acp_mst_ttax y on y.tax_gid = a.tax_gid " +
                    " left join pmr_mst_tproduct j on i.product_gid = j.product_gid   left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid " +
                    " left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid  LEFT JOIN pmr_trn_trenewal r ON a.agreement_gid = r.agreement_gid " +
                    " where r.renewal_gid = '" + renewal_gid + "'";

                }

                dt_datatable = objdbconn.GetDataTable(mssql);
                var getModuleList = new List<GetinvioceProduct1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        grand_total += double.Parse(dt["product_price_L"].ToString()) * double.Parse(dt["qty_ordered"].ToString()) - double.Parse(dt["discount_percentage"].ToString());

                        getModuleList.Add(new GetinvioceProduct1
                        {

                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            display_field_name = dt["display_field_name"].ToString(),
                            qyt_unit = dt["qyt_unit"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount1 = dt["discount_amount1"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            overalltaxname = dt["overalltaxname"].ToString(),
                            overall_tax = dt["overall_tax"].ToString(),
                            product_price_L = dt["product_price_L"].ToString(),
                        });

                        values.GetinvioceProduct1 = getModuleList;
                        values.grand_total = grand_total;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              mssql + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostOverAllSubmit(string user_gid, string employee_gid, OverallSubmit_list1 values)
        {

            msINGetGID = objcmnfunctions.GetMasterGID("SIVP");

            msSQL = "SELECT purchaseorder_gid FROM pmr_trn_trenewal WHERE renewal_gid = '" + values.renewal_gid + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            string lspurchaseorder_gid = null;
            if (objOdbcDataReader.HasRows)
            {
                lspurchaseorder_gid = objOdbcDataReader["purchaseorder_gid"].ToString();
            }
            if (!string.IsNullOrEmpty(lspurchaseorder_gid))
            {

                mssql = " select  i.tax1_gid as producttax, i.tax2_gid as producttax2, concat(i.qty_ordered,' ',i.productuom_name) as qyt_unit ,n.qty_delivered, format(a.tax_percentage, 2) as tax_percentage, format(i.product_price_L,2) as product_price_L, format(a.addon_amount, 2) as addon_amount," +
                        " format(a.roundoff, 2) as roundoff,  concat_ws('-', h.costcenter_name, h.costcenter_gid) as costcenter_name,format(h.budget_available, 2) as budget_available," +
                        " h.costcenter_gid,  a.payment_days,a.tax_gid,a.delivery_days,format(a.freightcharges,2) as freightcharges,a.buybackorscrap,a.manualporef_no,  " +
                        " format(a.packing_charges, 2) as packing_charges, format(a.insurance_charges, 2) as insurance_charges ,format(a.discount_amount,2) as additional_discount,  " +
                        " i.purchaseorderdtl_gid, i.product_gid,  i.product_price, i.qty_ordered,i.needby_date, format(i.discount_percentage, 2) as discount_percentage ,  " +
                        " format(i.discount_amount, 2) discount_amount1 ,  format(i.tax_percentage, 2) as tax_percentage,  format(i.tax_percentage2, 2) as tax_percentage2,  " +
                        " format(i.tax_percentage3, 2) as tax_percentage3,  format(i.tax_amount, 2) as tax_amount,  format(i.tax_amount2, 2) as tax_amount2, " +
                        " format(i.tax_amount3, 2) as tax_amount3,i.taxseg_taxname1,i.taxseg_taxpercent1,format(i.taxseg_taxamount1,2) AS taxseg_taxamount1," +
                        " i.taxseg_taxname2,i.taxseg_taxpercent2,format(i.taxseg_taxamount2,2) AS taxseg_taxamount2,  i.qty_Received, i.qty_grnadjusted, " +
                        " i.taxseg_taxname3,i.taxseg_taxpercent3,format(i.taxseg_taxamount3,2) as taxseg_taxamount3,  i.product_remarks, " +
                        " format((qty_ordered * i.product_price), 2) as product_totalprice,  format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as producttotal_amount," +
                        " i.product_code, (i.product_name) as product_name,format(a.netamount, 2) as netamount, k.productgroup_name, i.productuom_name," +
                        " i.purchaseorder_gid,i.display_field_name, a.currency_code,a.poref_no,i.tax1_gid,y.tax_name as overalltaxname, " +
                        " FORMAT(i.tax_amount + i.tax_amount2, 2) AS overall_tax ,CONCAT(i.tax_name, ' ', i.tax_percentage, ' , ', i.tax_name2, ' ', i.tax_percentage2) AS taxesname ," +
                        " concat(i.tax_amount,', ',i.tax_amount2) as taxesamt," +
                        " i.discount_amount,l.productuom_code, a.exchange_rate,i.uom_gid,i.taxsegment_gid,i.tax_name,i.tax_name2,a.taxsegmenttax_gid,a.mode_despatch,p.grndtl_gid,o.grn_gid,a.purchaseorder_from from pmr_trn_tpurchaseorder a  " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid  " +
                        " left join adm_mst_taddress w on b.address_gid = w.address_gid  " +
                        " left join adm_mst_tuser c on c.user_gid = a.created_by  " +
                        " left join hrm_mst_temployee d on d.user_gid = c.user_gid  " +
                        " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid  " +
                        " left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid  " +
                        " left join adm_mst_tuser g on g.user_gid = a.approved_by  " +
                        " left join pmr_mst_tcostcenter h on h.costcenter_gid = a.costcenter_gid  " +
                        " left join pmr_trn_tpurchaseorderdtl i ON a.purchaseorder_gid = i.purchaseorder_gid  " +
                        " left join acp_mst_ttax y on y.tax_gid = a.tax_gid  " +
                        " left join pmr_mst_tproduct j on i.product_gid = j.product_gid  " +
                        " left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid  " +
                        " left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid  " +
                        " left join adm_mst_tuser m on m.user_gid = a.requested_by " +
                        "  left join pmr_trn_tgrndtl n on n.purchaseorderdtl_gid = i.purchaseorderdtl_gid " +
                        " left join pmr_trn_tgrn o on o.grn_gid = n.grn_gid " +
                        "  left join pmr_trn_tgrndtl p on o.grn_gid=p.grn_gid" +
                        " LEFT JOIN pmr_trn_trenewal r ON a.purchaseorder_gid = r.purchaseorder_gid " +
                        " where r.renewal_gid = '" + values.renewal_gid + "'";
                dt_datatable = objdbconn.GetDataTable(mssql);

                foreach (DataRow Pr in dt_datatable.Rows)
                {
                    string taxsegment_gid = Pr["taxsegment_gid"].ToString();
                    string taxsegmenttax_gid = Pr["taxsegmenttax_gid"].ToString();
                    lsproduct_gid = Pr["product_gid"].ToString();
                    lsuom_gid = Pr["uom_gid"].ToString();
                    lsmode_despatch = Pr["mode_despatch"].ToString();
                    lsproductgroup_name = Pr["productgroup_name"].ToString();
                    lsqty_ordered = Pr["qty_ordered"].ToString();

                    decimal product_price, lsproduct_price;
                    if (decimal.TryParse(Pr["product_price"]?.ToString(), out product_price))
                    {
                        lsproduct_price = product_price;
                    }
                    else
                    {
                        lsproduct_price = 0;
                    }
                    decimal lsproduct_total, product_total;
                    if (decimal.TryParse(Pr["producttotal_amount"]?.ToString()?.Replace(",", ""), out product_total))
                    {
                        lsproduct_total = product_total;
                    }
                    else
                    {
                        lsproduct_total = 0;
                    }

                    decimal lsInvoiceqty_billed, Invoiceqty_billed;
                    if (decimal.TryParse(Pr["qty_delivered"]?.ToString(), out Invoiceqty_billed))
                    {
                        lsInvoiceqty_billed = Invoiceqty_billed;
                    }
                    else
                    {
                        lsInvoiceqty_billed = 0;
                    }
                    decimal lsproducttotal_amount, producttotal_amount;
                    if (decimal.TryParse(Pr["producttotal_amount"]?.ToString(), out producttotal_amount))
                    {
                        lsproducttotal_amount = producttotal_amount;
                    }
                    else
                    {
                        lsproducttotal_amount = 0;
                    }
                    lsDiscount_Percentage = Pr["discount_percentage"].ToString();
                    lsTax_name = Pr["tax_name"].ToString();
                    lsTax_name2 = Pr["tax_name2"].ToString();
                    lsproduct_code = Pr["product_code"].ToString();
                    lsproduct_name = Pr["product_name"].ToString();
                    lsTax1_gid = Pr["producttax"].ToString();
                    lsTax2_gid = Pr["producttax2"].ToString();
                    lsproductuom_name = Pr["productuom_name"].ToString();

                    lsproductuom_code = Pr["productuom_code"].ToString();
                    lsgrndtl_gid = Pr["grndtl_gid"].ToString();
                    lsgrn_gid = Pr["grn_gid"].ToString();
                    lspurchaseorderdtl_gid = Pr["purchaseorderdtl_gid"].ToString();
                    lspurchaseorder_gid = Pr["purchaseorder_gid"].ToString();
                    lspurchaseorder_from = Pr["purchaseorder_from"].ToString();
                    lsTax_Percentage = Pr["tax_percentage"].ToString();
                    if (decimal.TryParse(Pr["exchange_rate"].ToString(), out decimal exchangeRate))
                    {
                        lsexchange_rate = exchangeRate;
                    }
                    else
                    {
                        lsexchange_rate = 0;
                    }
                    if (decimal.TryParse(Pr["tax_amount"].ToString(), out decimal taxAmount))
                    {
                        lsTax_Amount = taxAmount;
                    }

                    else
                    {

                        lsTax_Amount = 0;
                    }
                    if (decimal.TryParse(Pr["tax_amount2"].ToString(), out decimal taxAmount2))
                    {
                        lsTax_Amount2 = taxAmount2;
                    }

                    else
                    {

                        lsTax_Amount2 = 0;
                    }
                    if (decimal.TryParse(Pr["discount_amount"].ToString(), out decimal discountamount))
                    {
                        lsDiscount_Amount = discountamount;
                    }
                    else
                    {
                        lsDiscount_Amount = 0;
                    }

                    lscurrency_unitprice = lsproduct_price * lsexchange_rate;
                    lscurrency_discountamount = lsDiscount_Amount * lsexchange_rate;
                    lscurrency_taxamount1 = lsTax_Amount * lsexchange_rate;


                    msGetGID = objcmnfunctions.GetMasterGID("SIVC");

                    mssql = " insert into acp_trn_tinvoicedtl (" +
                               " invoicedtl_gid, " +
                               " invoice_gid, " +
                               " vendor_refnodate, " +
                               " product_gid, " +
                               " uom_gid, " +
                               " product_price, " +
                               " product_total, " +
                               " discount_percentage, " +
                               " discount_amount, " +
                               " tax_name, " +
                               " tax_name2, " +
                               " tax_name3, " +
                               " tax_percentage, " +
                               " tax_percentage2, " +
                               " tax_percentage3, " +
                               " tax_amount, " +
                               " tax_amount2, " +
                               " tax_amount3, " +
                               " tax1_gid, " +
                               " tax2_gid, " +
                               " tax3_gid, " +
                               " qty_invoice, " +
                               " product_remarks, " +
                               " display_field," +
                               " product_price_L," +
                               " discount_amount_L," +
                               " tax_amount1_L," +
                               " tax_amount2_L," +
                               " tax_amount3_L," +
                               " taxsegment_gid," +
                               " taxsegmenttax_gid," +
                               " productgroup_code," +
                               " productgroup_name," +
                               " product_code," +
                               " product_name," +
                               " productuom_code," +
                               " productuom_name" +
                               " )values ( " +
                               "'" + msGetGID + "', " +
                               "'" + msINGetGID + "'," +
                               "' " + values.Vendor_ref_no + "'," +
                               "'" + lsproduct_gid + "', " +
                               "'" + lsuom_gid + "', " +
                               "'" + lsproduct_price + "', " +
                               "'" + lsproduct_total + "', " +
                               "'" + lsDiscount_Percentage + "', " +
                               "'" + lsDiscount_Amount + "', " +
                               "'" + lsTax_name + "', " +
                               "'" + lsTax_name2 + "', " +
                               "'" + lsTax_name3 + "', ";
                    if (lsTax_Percentage != "")
                    {
                        mssql += "'0.00', ";
                    }
                    else
                    {
                        mssql += "'" + lsTax_Percentage + "', ";
                    }
                    if (lsTax_Percentage2 != "")
                    {
                        mssql += "'0.00', ";

                    }
                    else
                    {
                        mssql += "'" + lsTax_Percentage2 + "', ";
                    }
                    if (lsTax_Percentage3 != "")
                    {
                        mssql += "'0.00', ";

                    }
                    else
                    {
                        mssql += "'" + lsTax_Percentage3 + "', ";
                    }
                    if (lsTax_Amount != 0)
                    {
                        mssql += "'" + lsTax_Amount + "', ";
                    }
                    else
                    {
                        mssql += "'0.00', ";
                    }
                    if (lsTax_Amount2 != 0)
                    {
                        mssql += "'" + lsTax_Amount2 + "', ";

                    }
                    else
                    {
                        mssql += "'0.00', ";
                    }
                    if (lsTax_Amount3 != "")
                    {
                        mssql += "'0.00', ";

                    }
                    else
                    {
                        mssql += "'" + lsTax_Amount3 + "', ";
                    }
                    mssql += "'" + lsTax1_gid + "'," +
                               "'" + lsTax2_gid + "'," +
                               "'" + lsTax3_gid + "'," +
                               "'" + lsqty_ordered + "', " +
                               "'" + values.product_remarks + "', " +
                               "'" + lsproduct_name + "'," +
                               "'" + lsproduct_total + "'," +
                               "'" + lscurrency_discountamount + "'," +
                               "'" + lscurrency_taxamount1 + "',";
                    if (lscurrency_taxamount2 != "")
                    {
                        mssql += "'0.00', ";
                    }
                    else
                    {
                        mssql += "'" + lscurrency_taxamount2 + "',";
                    }
                    if (lscurrency_taxamount3 != "")
                    {
                        mssql += "'0.00', ";
                    }
                    else
                    {
                        mssql += "'" + lscurrency_taxamount3 + "',";
                    }

                    mssql += "'" + taxsegment_gid + "', " +
                               "'" + taxsegmenttax_gid + "', " +
                        "'" + lsproductgroup_code + "', " +
                               "'" + lsproductgroup_name + "', " +
                                "'" + lsproduct_code + "', " +
                               "'" + lsproduct_name + "', " +
                                "'" + lsproductuom_code + "', " +
                               "'" + lsproductuom_name + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);


                    msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP", "", user_gid);
                    msPOGetGID = objcmnfunctions.GetMasterGID("PODC");

                    msSQL = " insert into pmr_trn_tpurchaseorderdtl (" +
                                " purchaseorderdtl_gid, " +
                                " purchaseorder_gid, " +
                                " product_gid, " +
                                " product_code, " +
                                " product_name, " +
                                " productuom_name, " +
                                " uom_gid, " +
                                " product_price, " +
                                " producttotal_amount, " +
                                " discount_percentage, " +
                                " discount_amount, " +
                                " tax_name, " +
                                " tax_name2, " +
                                " tax_name3, " +
                                " tax1_gid, " +
                                " tax2_gid, " +
                                " tax3_gid, " +
                                " qty_ordered, " +
                                " product_price_L, " +
                                " display_field_name," +
                                " tax_amount1_L," +
                                " tax_amount2_L," +
                                " tax_amount3_L," +
                                 " tax_amount," +
                                " tax_amount2," +
                                " tax_amount3," +
                                " tax_percentage," +
                                " tax_percentage2," +
                                " tax_percentage3," +
                                " taxsegment_gid" +
                                " )values ( " +
                            "'" + msPOGetGID + "', " +
                            "'" + msPO1GetGID + "'," +
                            "'" + lsproduct_gid + "', " +
                            "'" + lsproduct_code + "', " +
                            "'" + lsproduct_name + "', " +
                            "'" + lsproductuom_name + "', " +
                            "'" + lsuom_gid + "', " +
                            "'" + lsproduct_price + "', " +
                            "'" + lsproduct_total + "', " +
                            "'" + lsDiscount_Percentage + "', " +
                            "'" + lsDiscount_Amount + "', " +
                            "'" + lsTax_name + "', " +
                            "'" + lsTax_name2 + "', " +
                            "'" + lsTax_name3 + "', " +
                            "'" + lsTax1_gid + "'," +
                            "'" + lsTax2_gid + "'," +
                            "'" + lsTax3_gid + "'," +
                            "'" + lsqty_ordered + "', " +
                            "'" + lsproduct_price + "', " +
                            "'" + values.product_remarks + "', ";
                    if (lsTax_Amount != 0)
                    {
                        msSQL += "'" + lsTax_Amount + "', ";
                    }
                    else
                    {
                        msSQL += "'0.00', ";
                    }
                    if (lsTax_Amount2 != 0)
                    {
                        msSQL += "'" + lsTax_Amount2 + "', ";

                    }
                    else
                    {
                        msSQL += "'0.00', ";
                    }
                    if (lsTax_Amount3 != "")
                    {
                        msSQL += "'0.00', ";

                    }
                    else
                    {
                        msSQL += "'" + lsTax_Amount3 + "', ";
                    }

                    if (lsTax_Amount != 0)
                    {
                        msSQL += "'" + lsTax_Amount + "', ";
                    }
                    else
                    {
                        msSQL += "'0.00', ";
                    }
                    if (lsTax_Amount2 != 0)
                    {
                        msSQL += "'" + lsTax_Amount2 + "', ";

                    }
                    else
                    {
                        msSQL += "'0.00', ";
                    }
                    if (lsTax_Amount3 != "")
                    {
                        msSQL += "'0.00', ";

                    }
                    else
                    {
                        msSQL += "'" + lsTax_Amount3 + "', ";
                    }
                    if (lsTax_Percentage != "")
                    {
                        msSQL += "'0.00', ";
                    }
                    else
                    {
                        msSQL += "'" + lsTax_Percentage + "', ";
                    }
                    if (lsTax_Percentage2 != "")
                    {
                        msSQL += "'0.00', ";

                    }
                    else
                    {
                        msSQL += "'" + lsTax_Percentage2 + "', ";
                    }
                    if (lsTax_Percentage3 != "")
                    {
                        msSQL += "'0.00', ";

                    }
                    else
                    {
                        msSQL += "'" + lsTax_Percentage3 + "', ";
                    }
                    msSQL += "'" + taxsegment_gid + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    mspGetGID = objcmnfunctions.GetMasterGID("SPIP");
                    mssql = " insert into acp_trn_tpo2invoice (" +
                                " po2invoice_gid, " +
                                " invoice_gid, " +
                                " invoicedtl_gid, " +
                                " grn_gid, " +
                                " grndtl_gid, " +
                                " purchaseorder_gid, " +
                                " purchaseorderdtl_gid, " +
                                " product_gid, " +
                                " qty_invoice, " +
                                " display_field_name)" +
                                " values ( " +
                                "'" + mspGetGID + "'," +
                                "'" + msINGetGID + "'," +
                                "'" + msGetGID + "'," +
                                "'" + lsgrn_gid + "'," +
                                "'" + lsgrndtl_gid + "', " +
                                "'" + lspurchaseorder_gid + "'," +
                                "'" + lspurchaseorderdtl_gid + "'," +
                                "'" + lsproduct_gid + "'," +
                                "'" + lsInvoiceqty_billed + "'," +
                                "'" + lsproduct_name + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);


                    if (lspurchaseorder_from == "Purchase")
                    {
                        decimal lsPO_Sum = lsInvoiceqty_billed;

                        decimal lspo_sumdec = lsInvoiceqty_billed;

                        mssql = " select qty_invoice from pmr_trn_tpurchaseorderdtl  where " +
                                " purchaseorderdtl_gid = '" + lspurchaseorderdtl_gid + "'";
                        odbcdr = objdbconn.GetDataReader(mssql);
                        if (odbcdr.HasRows)
                        {
                            lsqty_invoice = Convert.ToInt32(odbcdr["qty_invoice"].ToString());
                            lsPO_Sum = lsPO_Sum += lsqty_invoice;
                        }

                        mssql = " Update pmr_trn_tpurchaseorderdtl " +
                                                    " Set qty_invoice = '" + lsPO_Sum + "'" +
                                                    " where purchaseorderdtl_gid = '" + lspurchaseorderdtl_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(mssql);


                        mssql = " select qty_invoice, qty_ordered " +
                                    " from pmr_trn_tpurchaseorderdtl  where " +
                                    " purchaseorder_gid = '" + lspurchaseorder_gid + "'and" +
                                    " qty_invoice < qty_ordered ";
                        odbcdr = objdbconn.GetDataReader(mssql);
                        if (odbcdr.HasRows)
                        {
                            lstPO_IV_flag = "Invoice Raised";
                        }
                        else
                        {
                            lstPO_IV_flag = "Invoice Raised";
                        }
                        mssql = " Update pmr_trn_tpurchaseorder " +
                                   " Set invoice_flag = '" + lstPO_IV_flag + "'" +
                                   " where purchaseorder_gid = '" + lspurchaseorder_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(mssql);

                        lsGRN_Sum = lsInvoiceqty_billed;


                        mssql = " select qty_invoice from pmr_trn_tgrndtl  where " +
                                   " grndtl_gid = '" + lsgrndtl_gid + "'";
                        odbcdr = objdbconn.GetDataReader(mssql);
                        if (odbcdr.HasRows)
                        {
                            if (decimal.TryParse(odbcdr["qty_invoice"].ToString(), out decimal qtyinvoice))
                            {
                                delsqty_invoice = qtyinvoice;
                            }
                            else
                            {
                                delsqty_invoice = 0;
                            }
                            lsGRN_Sum = lsGRN_Sum += delsqty_invoice;
                        }

                        mssql = " Update pmr_trn_tgrndtl " +
                                   " Set qty_invoice = '" + lsGRN_Sum + "'" +
                                   " where grndtl_gid = '" + lsgrndtl_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(mssql);


                        mssql = " select qty_delivered, qty_rejected, qty_invoice " +
                                    " from pmr_trn_tgrndtl  where " +
                                    " grn_gid = '" + lsgrn_gid + "'and" +
                                    " qty_invoice < (qty_delivered - qty_rejected) ";
                        odbcdr = objdbconn.GetDataReader(mssql);
                        if (odbcdr.HasRows)
                        {
                            lsinvoice_status = "IV Work In Progress";
                            lstGRN_IV_flag = "Invoice Raised Partial";
                        }
                        else
                        {
                            lsinvoice_status = "IV Completed";
                            lstGRN_IV_flag = "Invoice Raised";
                        }

                        mssql = " Update pmr_trn_tgrn " +
                                    " Set invoice_status = 'IV Completed'," +
                                    " invoice_flag = 'Invoice Raised'" +
                                    " where grn_gid = '" + lsgrn_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
                    }
                }
                mssql = "select vendor_address,branch_gid,vendor_gid,vendor_contact_person from pmr_trn_tpurchaseorder where purchaseorder_gid='" + lspurchaseorder_gid + "'";
                odbcdr = objdbconn.GetDataReader(mssql);
                if (odbcdr.HasRows)
                {
                    lsvendoraddress = odbcdr["vendor_address"].ToString();
                    lsbranch = odbcdr["branch_gid"].ToString();
                    lsvendor_gid = odbcdr["vendor_gid"].ToString();
                    lsvendor_contact = odbcdr["vendor_contact_person"].ToString();
                }
                mssql = " select invoiceref_flag from pbl_mst_tconfiguration ";
                odbcdr = objdbconn.GetDataReader(mssql);
                if (odbcdr.HasRows)
                {
                    lsinvoiceref_flag = odbcdr["invoiceref_flag"].ToString();
                    if (lsinvoiceref_flag == "Y")
                    {
                        lsinv_ref_no = values.inv_ref_no;
                    }
                    else
                    {
                        lsinv_ref_no = objcmnfunctions.GetMasterGID("PINV");
                    }
                }

                string ls_referenceno;

                if (values.inv_ref_no == "")
                {
                    ls_referenceno = objcmnfunctions.getSequencecustomizerGID("PINV", "PBL", values.branch_name);
                }
                else
                {

                    ls_referenceno = values.inv_ref_no;
                }
                string vendor_gid = lsvendor_gid;
                mssql = "select vendor_code from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                string lsvenorcode1 = objdbconn.GetExecuteScalar(mssql);
                mssql = "select vendor_companyname from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                string lsvendor_companyname1 = objdbconn.GetExecuteScalar(mssql);
                mssql = "SELECT account_gid from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                odbcdr = objdbconn.GetDataReader(mssql);
                if (odbcdr.HasRows)
                {
                    while (odbcdr.Read())
                    {
                        string lsaccount_gid = odbcdr["account_gid"]?.ToString(); // Safely get the value

                        // Check if lsaccount_gid is null or empty
                        if (string.IsNullOrEmpty(lsaccount_gid))
                        {
                            objfincmn.finance_vendor_debitor("Purchase", lsvenorcode1, lsvendor_companyname1, vendor_gid, user_gid);
                            string trace_comment = "Added a vendor on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_vendor");
                        }
                    }
                }
                string payment_date = values.payment_date;
                DateTime parsedDate;
                if (DateTime.TryParse(payment_date, out parsedDate))
                {
                    parsed_Date = parsedDate.ToString("yyyy-MM-dd");
                }
                string invoice_date = values.invoice_date;
                if (invoice_date == "")
                {
                    DateTime uiDate = DateTime.ParseExact(invoice_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    invoice_Date = uiDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    invoice_Date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                mssql = "select branch_gid from hrm_mst_tbranch where branch_name ='" + values.branch_name + "'";
                string lsbranchname = objdbconn.GetExecuteScalar(mssql);
                msINGetREGID = objcmnfunctions.GetMasterGID("BRLP");
                mssql = " insert into acp_trn_tinvoice (" +
                        " invoice_gid, " +
                        " invoice_refno, " +
                        " renewal_gid, " +
                        " vendorinvoiceref_no, " +
                        " invoice_reference, " +
                        " shipping_address, " +
                        " mode_despatch, " +
                        " billing_email, " +
                        " vendor_contact_person, " +
                        " vendor_address, " +
                        " user_gid, " +
                        " invoice_date," +
                        " payment_date," +
                        " systemgenerated_amount, " +
                        " additionalcharges_amount, " +
                        " discount_amount, " +
                        " total_amount, " +
                        " invoice_amount, " +
                        " created_date," +
                        " vendor_gid, " +
                        " invoice_status, " +
                        " invoice_flag, " +
                        " invoice_remarks, " +
                        " invoice_from, " +
                        " additionalcharges_amount_L, " +
                        " discount_amount_L, " +
                        " total_amount_L, " +
                        " currency_code," +
                        " exchange_rate," +
                        " freightcharges," +
                        " extraadditional_code," +
                        " extradiscount_code," +
                        " extraadditional_amount," +
                        " extradiscount_amount," +
                        " extraadditional_amount_L," +
                        " extradiscount_amount_L," +
                        " priority, " +
                        " priority_remarks," +
                        " buybackorscrap," +
                        " tax_name," +
                        " tax_percentage," +
                        " Tax_amount," +
                        " branch_gid," +
                        " round_off," +
                        " tax_gid," +
                        " taxsegment_gid," +
                        " taxsegmenttax_gid," +
                        " packing_charges," +
                        " payment_term," +
                        " termsandconditions," +
                        " delivery_term," +
                        " purchaseorder_gid," +
                        " purchase_type," +
                        " insurance_charges " +
                        " ) values (" +
                        "'" + msINGetGID + "'," +
                        "'" + values.inv_ref_no + "'," +
                         "'" + msINGetGID + "'," +
                        "'" + values.inv_ref_no + "'," +
                        "'" + values.inv_ref_no + "'," +
                        "'" + values.shipping_address + "'," +
                        "'" + lsmode_despatch + "'," +
                        "'" + values.billing_email + "'," +
                        "'" + lsvendor_contact + "'," +
                        "'" + lsvendoraddress + "'," +
                        "'" + user_gid + "'," +
                        "'" + invoice_Date + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
                if (string.IsNullOrEmpty(values.total_amount))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.total_amount.Replace(",", "") + "',";
                }
                if (string.IsNullOrEmpty(values.addoncharge))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.addoncharge.Replace(",", "") + "',";
                }

                if (string.IsNullOrEmpty(values.additional_discount))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.additional_discount.Replace(",", "") + "',";
                }
                if (string.IsNullOrEmpty(values.totalamount))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.totalamount.Replace(",", "") + "',";
                }
                if (string.IsNullOrEmpty(values.total_amount))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.total_amount + "',";
                }
                mssql += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                "'" + lsvendor_gid + "'," +
                "'" + "IV Completed" + "'," +
                "'" + "Payment Pending" + "'," +
                "'" + values.invoice_remarks + "', " +
                "'" + lspurchaseorder_from + "'," +
                "'0.00',";

                if (string.IsNullOrEmpty(values.additional_discount))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.additional_discount.Replace(",", "") + "',";
                }
                if (string.IsNullOrEmpty(values.total_amount))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.total_amount + "',";
                }

                mssql += "'" + values.currency_code + "'," +
                  "'" + values.exchange_rate + "',";

                if (string.IsNullOrEmpty(values.freightcharges))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.freightcharges.Replace(",", "") + "',";
                }
                mssql += "'0.00'," +
                        "'0.00'," +
                        "'0.00'," +
                        "'0.00'," +
                        "'0.00'," +
                        "'0.00'," +
                        "'" + values.priority_n + "', " +
                        "'', ";
                if (string.IsNullOrEmpty(values.buybackorscrap))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.buybackorscrap + "',";
                }
                mssql += "'" + values.tax_name + "'," +
                        "'" + lsTax_Percentage + "'," +
                        "'" + values.tax_amount4 + "'," +
                        "'" + lsbranchname + "',";
                if (string.IsNullOrEmpty(values.roundoff))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.roundoff + "',";
                }

                mssql += "'" + lsTax1_gid + "'," +
                      "'" + values.taxsegment_gid + "'," +
                       "'" + values.taxsegmenttax_gid + "',";
                if (string.IsNullOrEmpty(values.packing_charges))
                {
                    mssql += "'0.00',";
                }
                else
                {
                    mssql += "'" + values.packing_charges + "',";
                }
                mssql += "'" + values.payment_terms + "'," +
                         "'" + values.template_content + "'," +
                        "'" + values.delivery_term + "'," +
                        "'" + values.purchaseorder_gid + "'," +
                        "'" + values.purchase_type + "',";

                if (string.IsNullOrEmpty(values.insurance_charges))
                {
                    mssql += "'0.00')";
                }
                else
                {
                    mssql += "'" + values.insurance_charges + "')";
                }

                mnResult = objdbconn.ExecuteNonQuerySQL(mssql);

                msSQL = " insert into pmr_trn_tpurchaseorder (" +
                            " purchaseorder_gid, " +
                            " purchaseorder_reference, " +
                            " purchaseorder_date," +
                            " expected_date," +
                            " branch_gid, " +
                            " created_by, " +
                            " created_date," +
                            " vendor_gid, " +
                            " vendor_address, " +
                            " shipping_address, " +
                            " freight_terms, " +
                            " payment_terms, " +
                            " mode_despatch, " +
                            " currency_code," +
                            " exchange_rate," +
                            " po_covernote," +
                            " purchaseorder_remarks," +
                            " total_amount, " +
                            " termsandconditions, " +
                            " purchaseorder_status, " +
                            " purchaseorder_flag, " +
                            " poref_no, " +
                            " netamount, " +
                            " addon_amount," +
                            " freightcharges," +
                            " discount_amount," +
                            " tax_gid," +
                            " tax_percentage," +
                            " tax_amount," +
                            " roundoff, " +
                            " renewal_flag, " +
                            " taxsegment_gid " +
                            " ) values (" +
                            "'" + msPO1GetGID + "'," +
                            "'" + msPO1GetGID + "'," +
                              "'" + invoice_Date + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'" + lsbranchname + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                           "'" + lsvendor_gid + "'," +
                       "'" + lsvendoraddress + "'," +
                        "'" + values.shipping_address + "'," +
                         "'" + values.delivery_term + "'," +
                      "'" + values.payment_terms + "'," +
                       "'" + lsmode_despatch + "',";
                msSQL += "'" + values.currency_code + "'," +
                "'" + values.exchange_rate + "'," +
                 "'" + values.invoice_remarks + "', " +
                 "'" + values.invoice_remarks + "', ";
                if (string.IsNullOrEmpty(values.total_amount))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.total_amount.Replace(",", "") + "',";
                }
                //"'" + values.template_content.Trim().Replace("'", "") + "',";
                msSQL += "'" + values.template_content + "'," +
                 "'PO Approved'," +
                "'" + "PO Approved" + "'," +

                "'" + values.inv_ref_no + "',";

                if (string.IsNullOrEmpty(values.total_amount))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.total_amount.Replace(",", "") + "',";
                }
                if (string.IsNullOrEmpty(values.addoncharge))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.addoncharge.Replace(",", "") + "',";
                }
                if (string.IsNullOrEmpty(values.freightcharges))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.freightcharges.Replace(",", "") + "',";
                }
                if (string.IsNullOrEmpty(values.additional_discount))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.additional_discount.Replace(",", "") + "',";
                }
                msSQL += "'" + values.tax_name + "'," +
                       "'" + lsTax_Percentage + "'," +
                       "'" + values.tax_amount4 + "',";
                if (string.IsNullOrEmpty(values.roundoff))
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.roundoff + "',";
                }
                msSQL += "'Y',";
                msSQL += "'" + values.taxsegment_gid + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                string uiDateStr1 = values.renewal_date;
                DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string renewal_date = uiDate1.ToString("yyyy-MM-dd");
                msSQL = " Insert into pmr_trn_trenewal ( " +
                            " renewal_gid, " +
                            " renewal_flag, " +
                            " vendor_gid," +
                            " renewal_date, " +
                            " frequency_term, " +
                            " purchaseorder_gid, " +
                            " created_by, " +
                            " renewal_type, " +
                            " created_date) " +
                           " Values ( " +
                             "'" + msINGetREGID + "'," +
                              "'Y'," +
                             "'" + lsvendor_gid + "'," +
                             "'" + renewal_date + "'," +
                             "'" + values.frequency_terms + "'," +
                             "'" + msPO1GetGID + "'," +
                             "'" + user_gid + "'," +
                             "'Purchase'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " update pmr_trn_trenewal set " +
                                   " renewal_status = 'closed' " +
                                   " where purchaseorder_gid = '" + lspurchaseorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult == 1)
                {
                    mssql = "select finance_flag from adm_mst_tcompany ";
                    string finance_flag = objdbconn.GetExecuteScalar(mssql);
                    if (finance_flag == "Y")
                    {
                        double product;
                        double discount;
                        mssql = "SELECT SUM(COALESCE(qty_invoice, 2) * COALESCE(product_price, 2)) AS product, ROUND(SUM(discount_amount), 2) AS discount FROM acp_trn_tinvoicedtl WHERE invoice_gid = '" + msINGetGID + "'";
                        odbcdr = objdbconn.GetDataReader(mssql);

                        if (odbcdr.HasRows)
                        {
                            odbcdr.Read();
                            product = odbcdr["product"] != DBNull.Value ? double.Parse(odbcdr["product"].ToString()) : 0.00;
                            discount = odbcdr["discount"] != DBNull.Value ? double.Parse(odbcdr["discount"].ToString()) : 0.00;
                        }
                        else
                        {
                            product = 0.00;
                            discount = 0.00;
                        }
                        odbcdr.Close();
                        double lsbasic_amount = product - discount;

                        double addonCharges = double.TryParse(values.addoncharge, out double addonChargesValue) ? addonChargesValue : 0;
                        double freightCharges = double.TryParse(values.freightcharges, out double freightChargesValue) ? freightChargesValue : 0;
                        double forwardingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                        double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                        double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                        double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                        double buybackCharges = double.TryParse(values.buybackorscrap, out double buybackChargesValue) ? buybackChargesValue : 0;
                        double overalltax_amount = double.TryParse(values.tax_amount4, out double overalltaxamount) ? overalltaxamount : 0;
                        double grandtotal = double.TryParse(values.total_amount, out double grand_total) ? grand_total : 0;
                        double ExchangeRate = double.TryParse(values.exchange_rate, out double exchange) ? exchange : 0;

                        double fin_basic_amount = lsbasic_amount * ExchangeRate;
                        double fin_addonCharges = addonCharges * ExchangeRate;
                        double fin_freightcharges = freightCharges * ExchangeRate;
                        double fin_forwardingCharges = forwardingCharges * ExchangeRate;
                        double fin_insuranceCharges = insuranceCharges * ExchangeRate;
                        double fin_roundoff = roundoff * ExchangeRate;
                        double fin_buybackCharges = buybackCharges * ExchangeRate;
                        double fin_overalltax_amount = overalltax_amount * ExchangeRate;
                        double fin_additionaldiscountAmount = additionaldiscountAmount * ExchangeRate;
                        double fin_grandtotal = grandtotal * ExchangeRate;


                        objfincmn.jn_purchase_invoice(invoice_Date, values.invoice_remarks, values.branch_name, ls_referenceno, msINGetGID
                         , fin_basic_amount, fin_addonCharges, fin_additionaldiscountAmount, fin_grandtotal, vendor_gid, "Invoice", "PMR",
                         values.purchase_type, fin_roundoff, fin_freightcharges, fin_buybackCharges, values.tax_name, fin_overalltax_amount, fin_forwardingCharges, fin_insuranceCharges);



                    }
                    {
                        OdbcDataReader objODBCDataReader, objODBCDataReader1, objODBCDataReader2, objODBCDataReader3;
                        string lstax_gid, lstaxsum, lstaxamount;

                        objdbconn.OpenConn();
                        string msSQL = "SELECT tax_gid, tax_name, percentage FROM acp_mst_ttax";
                        objODBCDataReader = objdbconn.GetDataReader(msSQL);

                        if (objODBCDataReader.HasRows)
                        {
                            while (objODBCDataReader.Read())
                            {
                                string lstax1 = "0.00";
                                string lstax2 = "0.00";
                                string lstax3 = "0.00";

                                // Tax 1 Calculation
                                msSQL = "SELECT SUM(tax_amount) AS tax1 FROM acp_trn_tinvoicedtl " +
                                        "WHERE invoice_gid = '" + msINGetGID + "' AND tax1_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                objODBCDataReader1 = objdbconn.GetDataReader(msSQL);

                                if (objODBCDataReader1.HasRows && objODBCDataReader1.Read())
                                {
                                    lstax1 = objODBCDataReader1["tax1"] != DBNull.Value ? objODBCDataReader1["tax1"].ToString() : "0.00";
                                }
                                objODBCDataReader1.Close();

                                // Tax 2 Calculation
                                msSQL = "SELECT SUM(tax_amount2) AS tax2 FROM acp_trn_tinvoicedtl " +
                                        "WHERE invoice_gid = '" + msINGetGID + "' AND tax2_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                objODBCDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objODBCDataReader2.HasRows && objODBCDataReader2.Read())
                                {
                                    lstax2 = objODBCDataReader2["tax2"] != DBNull.Value ? objODBCDataReader2["tax2"].ToString() : "0.00";
                                }
                                objODBCDataReader2.Close();

                            }
                        }

                        objdbconn.CloseConn();
                    }
                    values.status = true;
                    values.message = "Invoice Raised Succesfully !";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while Invoice ";
                }
            }

            else
            {

                mssql = " select  i.tax1_gid as producttax,i.tax2_gid as producttax2,concat(i.qty_ordered,' ',i.productuom_name) as qyt_unit,format(a.tax_percentage, 2) as tax_percentage, " +
                      " format(i.product_price_L,2) as product_price_L,format(a.addon_amount, 2) as addon_amount,format(a.roundoff, 2) as roundoff,\r\n" +
                      " a.payment_days,a.tax_gid,a.delivery_days,format(a.freightcharges,2) as freightcharges,format(a.packing_charges, 2) as packing_charges, \r\n" +
                      " format(a.insurance_charges, 2) as insurance_charges ,format(a.discount_amount,2) as additional_discount,i.agreementdtl_gid  as purchaseorderdtl_gid,\r\n " +
                      " i.product_gid,  i.product_price, i.qty_ordered,format(i.discount_percentage, 2) as discount_percentage ,   format(i.discount_amount, 2) discount_amount1,\r\n" +
                      " format(i.tax_percentage, 2) as tax_percentage,  format(i.tax_percentage2, 2) as tax_percentage2,   format(i.tax_percentage3, 2) as tax_percentage3,  format(i.tax_amount, 2) as tax_amount,  format(i.tax_amount2, 2) as tax_amount2,\r\n" +
                      " i.product_remarks,  format((qty_ordered * i.product_price), 2) as product_totalprice,i.product_code, (i.product_name) as product_name,\r\n" +
                      " format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as producttotal_amount,\r\n" +
                      " format(a.grandtotal, 2) as netamount,k.productgroup_name, i.productuom_name,i.agreement_gid as  purchaseorder_gid,a.currency_code,i.tax1_gid,\r\n" +
                      " FORMAT(i.tax_amount + i.tax_amount2, 2) AS overall_tax,CONCAT(i.tax_name, ' ', i.tax_percentage, ' , ', i.tax_name2, ' ', i.tax_percentage2) AS taxesname,\r\n" +
                      " concat(i.tax_amount,', ',i.tax_amount2) as taxesamt,i.discount_amount,l.productuom_code, a.exchange_rate,i.uom_gid,i.taxsegment_gid,i.tax_name,i.tax_name2,\r\n" +
                      " i.taxsegmenttax_gid,a.mode_despatch from pbl_trn_tagreement a  left join acp_mst_tvendor b on a.customer_gid = b.vendor_gid  \r\n" +
                      "  left join adm_mst_taddress w on b.address_gid = w.address_gid  left join adm_mst_tuser c on c.user_gid = a.created_by  \r\n" +
                      "  left join hrm_mst_temployee d on d.user_gid = c.user_gid left join hrm_mst_tdepartment e on e.department_gid = d.department_gid \r\n" +
                      "  left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid   left join adm_mst_tuser g on g.user_gid = a.approved_by  \r\n" +
                      "  left join pbl_trn_tagreementdtl i ON a.agreement_gid = i.agreement_gid left join acp_mst_ttax y on y.tax_gid = a.tax_gid \r\n " +
                      "  left join pmr_mst_tproduct j on i.product_gid = j.product_gid  left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid \r\n" +
                      " left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid  LEFT JOIN pmr_trn_trenewal r ON a.agreement_gid = r.agreement_gid \r\n" +
                      " where r.renewal_gid = '" + values.renewal_gid + "'";
                dt_datatable = objdbconn.GetDataTable(mssql);

                foreach (DataRow Pr in dt_datatable.Rows)
                {
                    string taxsegment_gid = Pr["taxsegment_gid"].ToString();
                    string taxsegmenttax_gid = Pr["taxsegmenttax_gid"].ToString();
                    lsproduct_gid = Pr["product_gid"].ToString();
                    lsuom_gid = Pr["uom_gid"].ToString();
                    lsmode_despatch = Pr["mode_despatch"].ToString();
                    lsproductgroup_name = Pr["productgroup_name"].ToString();
                    lsqty_ordered = Pr["qty_ordered"].ToString();

                    decimal product_price, lsproduct_price;
                    if (decimal.TryParse(Pr["product_price"]?.ToString(), out product_price))
                    {
                        lsproduct_price = product_price;
                    }
                    else
                    {
                        lsproduct_price = 0;
                    }
                    decimal lsproduct_total, product_total;
                    if (decimal.TryParse(Pr["producttotal_amount"]?.ToString()?.Replace(",", ""), out product_total))
                    {
                        lsproduct_total = product_total;
                    }
                    else
                    {
                        lsproduct_total = 0;
                    }


                    decimal lsproducttotal_amount, producttotal_amount;
                    if (decimal.TryParse(Pr["producttotal_amount"]?.ToString(), out producttotal_amount))
                    {
                        lsproducttotal_amount = producttotal_amount;
                    }
                    else
                    {
                        lsproducttotal_amount = 0;
                    }
                    lsDiscount_Percentage = Pr["discount_percentage"].ToString();
                    lsTax_name = Pr["tax_name"].ToString();
                    lsTax_name2 = Pr["tax_name2"].ToString();
                    lsproduct_code = Pr["product_code"].ToString();
                    lsproduct_name = Pr["product_name"].ToString();
                    lsTax1_gid = Pr["producttax"].ToString();
                    lsTax2_gid = Pr["producttax2"].ToString();
                    lsproductuom_name = Pr["productuom_name"].ToString();

                    lsproductuom_code = Pr["productuom_code"].ToString();
                    lspurchaseorderdtl_gid = Pr["purchaseorderdtl_gid"].ToString();
                    lspurchaseorder_gid = Pr["purchaseorder_gid"].ToString();
                    lsTax_Percentage = Pr["tax_percentage"].ToString();
                    if (decimal.TryParse(Pr["exchange_rate"].ToString(), out decimal exchangeRate))
                    {
                        lsexchange_rate = exchangeRate;
                    }
                    else
                    {
                        lsexchange_rate = 0;
                    }
                    if (decimal.TryParse(Pr["tax_amount"].ToString(), out decimal taxAmount))
                    {
                        lsTax_Amount = taxAmount;
                    }

                    else
                    {

                        lsTax_Amount = 0;
                    }
                    if (decimal.TryParse(Pr["tax_amount2"].ToString(), out decimal taxAmount2))
                    {
                        lsTax_Amount2 = taxAmount2;
                    }

                    else
                    {

                        lsTax_Amount2 = 0;
                    }
                    if (decimal.TryParse(Pr["discount_amount"].ToString(), out decimal discountamount))
                    {
                        lsDiscount_Amount = discountamount;
                    }
                    else
                    {
                        lsDiscount_Amount = 0;
                    }

                    lscurrency_unitprice = lsproduct_price * lsexchange_rate;
                    lscurrency_discountamount = lsDiscount_Amount * lsexchange_rate;
                    lscurrency_taxamount1 = lsTax_Amount * lsexchange_rate;


                    msGetGID = objcmnfunctions.GetMasterGID("SIVC");

                    mssql = " insert into acp_trn_tinvoicedtl (" +
                               " invoicedtl_gid, " +
                               " invoice_gid, " +
                               " vendor_refnodate, " +
                               " product_gid, " +
                               " uom_gid, " +
                               " product_price, " +
                               " product_total, " +
                               " discount_percentage, " +
                               " discount_amount, " +
                               " tax_name, " +
                               " tax_name2, " +
                               " tax_name3, " +
                               " tax_percentage, " +
                               " tax_percentage2, " +
                               " tax_percentage3, " +
                               " tax_amount, " +
                               " tax_amount2, " +
                               " tax_amount3, " +
                               " tax1_gid, " +
                               " tax2_gid, " +
                               " tax3_gid, " +
                               " qty_invoice, " +
                               " product_remarks, " +
                               " display_field," +
                               " product_price_L," +
                               " discount_amount_L," +
                               " tax_amount1_L," +
                               " tax_amount2_L," +
                               " tax_amount3_L," +
                               " taxsegment_gid," +
                               " taxsegmenttax_gid," +
                               " productgroup_code," +
                               " productgroup_name," +
                               " product_code," +
                               " product_name," +
                               " productuom_code," +
                               " productuom_name" +
                               " )values ( " +
                               "'" + msGetGID + "', " +
                               "'" + msINGetGID + "'," +
                               "' " + values.Vendor_ref_no + "'," +
                               "'" + lsproduct_gid + "', " +
                               "'" + lsuom_gid + "', " +
                               "'" + lsproduct_price + "', " +
                               "'" + lsproduct_total + "', " +
                               "'" + lsDiscount_Percentage + "', " +
                               "'" + lsDiscount_Amount + "', " +
                               "'" + lsTax_name + "', " +
                               "'" + lsTax_name2 + "', " +
                               "'" + lsTax_name3 + "', ";
                    if (lsTax_Percentage != "")
                    {
                        mssql += "'0.00', ";
                    }
                    else
                    {
                        mssql += "'" + lsTax_Percentage + "', ";
                    }
                    if (lsTax_Percentage2 != "")
                    {
                        mssql += "'0.00', ";

                    }
                    else
                    {
                        mssql += "'" + lsTax_Percentage2 + "', ";
                    }
                    if (lsTax_Percentage3 != "")
                    {
                        mssql += "'0.00', ";

                    }
                    else
                    {
                        mssql += "'" + lsTax_Percentage3 + "', ";
                    }
                    if (lsTax_Amount != 0)
                    {
                        mssql += "'" + lsTax_Amount + "', ";
                    }
                    else
                    {
                        mssql += "'0.00', ";
                    }
                    if (lsTax_Amount2 != 0)
                    {
                        mssql += "'" + lsTax_Amount2 + "', ";

                    }
                    else
                    {
                        mssql += "'0.00', ";
                    }
                    if (lsTax_Amount3 != "")
                    {
                        mssql += "'0.00', ";

                    }
                    else
                    {
                        mssql += "'" + lsTax_Amount3 + "', ";
                    }
                    mssql += "'" + lsTax1_gid + "'," +
                               "'" + lsTax2_gid + "'," +
                               "'" + lsTax3_gid + "'," +
                               "'" + lsqty_ordered + "', " +
                               "'" + values.product_remarks + "', " +
                               "'" + lsproduct_name + "'," +
                               "'" + lsproduct_total + "'," +
                               "'" + lscurrency_discountamount + "'," +
                               "'" + lscurrency_taxamount1 + "',";
                    if (lscurrency_taxamount2 != "")
                    {
                        mssql += "'0.00', ";
                    }
                    else
                    {
                        mssql += "'" + lscurrency_taxamount2 + "',";
                    }
                    if (lscurrency_taxamount3 != "")
                    {
                        mssql += "'0.00', ";
                    }
                    else
                    {
                        mssql += "'" + lscurrency_taxamount3 + "',";
                    }

                    mssql += "'" + taxsegment_gid + "', " +
                               "'" + taxsegmenttax_gid + "', " +
                        "'" + lsproductgroup_code + "', " +
                               "'" + lsproductgroup_name + "', " +
                                "'" + lsproduct_code + "', " +
                               "'" + lsproduct_name + "', " +
                                "'" + lsproductuom_code + "', " +
                               "'" + lsproductuom_name + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);

                    mspGetGID = objcmnfunctions.GetMasterGID("SPIP");
                    mssql = " insert into acp_trn_tpo2invoice (" +
                                " po2invoice_gid, " +
                                " invoice_gid, " +
                                " invoicedtl_gid, " +
                                " grn_gid, " +
                                " grndtl_gid, " +
                                " purchaseorder_gid, " +
                                " purchaseorderdtl_gid, " +
                                " product_gid, " +
                                " qty_invoice, " +
                                " display_field_name)" +
                                " values ( " +
                                "'" + mspGetGID + "'," +
                                "'" + msINGetGID + "'," +
                                "'" + msGetGID + "'," +
                                "'" + lsgrn_gid + "'," +
                                "'" + lsgrndtl_gid + "', " +
                                "'" + lspurchaseorder_gid + "'," +
                                "'" + lspurchaseorderdtl_gid + "'," +
                                "'" + lsproduct_gid + "'," +
                                "'" + lsqty_ordered + "'," +
                                "'" + lsproduct_name + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);


                }
                mssql = "select customer_address as vendor_address,branch_gid,customer_gid as vendor_gid from pbl_trn_tagreement where agreement_gid='" + lspurchaseorder_gid + "'";
                odbcdr = objdbconn.GetDataReader(mssql);
                if (odbcdr.HasRows)
                {
                    lsvendoraddress = odbcdr["vendor_address"].ToString();
                    lsbranch = odbcdr["branch_gid"].ToString();
                    lsvendor_gid = odbcdr["vendor_gid"].ToString();
                }
                mssql = " select invoiceref_flag from pbl_mst_tconfiguration ";
                odbcdr = objdbconn.GetDataReader(mssql);
                if (odbcdr.HasRows)
                {
                    lsinvoiceref_flag = odbcdr["invoiceref_flag"].ToString();
                    if (lsinvoiceref_flag == "Y")
                    {
                        lsinv_ref_no = values.inv_ref_no;
                    }
                    else
                    {
                        lsinv_ref_no = objcmnfunctions.GetMasterGID("PINV");
                    }
                }

                string ls_referenceno;

                if (values.inv_ref_no == "")
                {
                    ls_referenceno = objcmnfunctions.getSequencecustomizerGID("PINV", "PBL", values.branch_name);
                }
                else
                {

                    ls_referenceno = values.inv_ref_no;
                }
                string vendor_gid = lsvendor_gid;
                mssql = "select vendor_code from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                string lsvenorcode1 = objdbconn.GetExecuteScalar(mssql);
                mssql = "select vendor_companyname from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                string lsvendor_companyname1 = objdbconn.GetExecuteScalar(mssql);
                mssql = "SELECT account_gid from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                odbcdr = objdbconn.GetDataReader(mssql);
                if (odbcdr.HasRows)
                {
                    while (odbcdr.Read())
                    {
                        string lsaccount_gid = odbcdr["account_gid"]?.ToString(); // Safely get the value

                        // Check if lsaccount_gid is null or empty
                        if (string.IsNullOrEmpty(lsaccount_gid))
                        {
                            objfincmn.finance_vendor_debitor("Purchase", lsvenorcode1, lsvendor_companyname1, vendor_gid, user_gid);
                            string trace_comment = "Added a vendor on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_vendor");
                        }
                    }
                }
                string payment_date = values.payment_date;
                DateTime parsedDate;
                if (DateTime.TryParse(payment_date, out parsedDate))
                {
                    parsed_Date = parsedDate.ToString("yyyy-MM-dd");
                }
                string invoice_date = values.invoice_date;
                if (invoice_date == "")
                {
                    DateTime uiDate = DateTime.ParseExact(invoice_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    invoice_Date = uiDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    invoice_Date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                mssql = "select branch_gid from hrm_mst_tbranch where branch_name ='" + values.branch_name + "'";
                string lsbranchname = objdbconn.GetExecuteScalar(mssql);

                mssql = " insert into acp_trn_tinvoice (" +
                        " invoice_gid, " +
                        " invoice_refno, " +
                        " vendorinvoiceref_no, " +
                        " invoice_reference, " +
                        " shipping_address, " +
                        " mode_despatch, " +
                        " billing_email, " +
                        " vendor_address, " +
                        " user_gid, " +
                        " invoice_date," +
                        " payment_date," +
                        " systemgenerated_amount, " +
                        " additionalcharges_amount, " +
                        " discount_amount, " +
                        " total_amount, " +
                        " invoice_amount, " +
                        " created_date," +
                        " vendor_gid, " +
                        " invoice_status, " +
                        " invoice_flag, " +
                        " invoice_remarks, " +
                        " invoice_from, " +
                        " additionalcharges_amount_L, " +
                        " discount_amount_L, " +
                        " total_amount_L, " +
                        " currency_code," +
                        " exchange_rate," +
                        " freightcharges," +
                        " extraadditional_code," +
                        " extradiscount_code," +
                        " extraadditional_amount," +
                        " extradiscount_amount," +
                        " extraadditional_amount_L," +
                        " extradiscount_amount_L," +
                        " priority, " +
                        " priority_remarks," +
                        " buybackorscrap," +
                        " tax_name," +
                        " tax_percentage," +
                        " Tax_amount," +
                        " branch_gid," +
                        " round_off," +
                        " tax_gid," +
                        " taxsegment_gid," +
                        " taxsegmenttax_gid," +
                        " packing_charges," +
                        " payment_term," +
                        " termsandconditions," +
                        " delivery_term," +
                        " purchaseorder_gid," +
                        " renewal_gid," +
                        " purchase_type," +
                        " insurance_charges " +
                        " ) values (" +
                        "'" + msINGetGID + "'," +
                        "'" + lsinv_ref_no + "'," +
                        "'" + lsinv_ref_no + "'," +
                        "'" + lsinv_ref_no + "',";
                        if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                        {
                    mssql += "'" + values.shipping_address.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                    mssql += "'" + values.shipping_address + "', ";
                        }
                        if (!string.IsNullOrEmpty(lsmode_despatch) && lsmode_despatch.Contains("'"))
                        {
                    mssql += "'" + lsmode_despatch.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                    mssql += "'" + lsmode_despatch + "', ";
                        }

                mssql += "'" + values.billing_email + "',";
                        if (!string.IsNullOrEmpty(lsvendoraddress) && lsvendoraddress.Contains("'"))
                        {
                    mssql += "'" + lsvendoraddress.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                    mssql += "'" + lsvendoraddress + "', ";
                        }
                mssql += "'" + user_gid + "'," +
                        "'" + invoice_Date + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
                        if (string.IsNullOrEmpty(values.total_amount))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.total_amount.Replace(",", "") + "',";
                        }
                        if (string.IsNullOrEmpty(values.addoncharge))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.addoncharge.Replace(",", "") + "',";
                        }
                        if (string.IsNullOrEmpty(values.additional_discount))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.additional_discount.Replace(",", "") + "',";
                        }
                        if (string.IsNullOrEmpty(values.totalamount))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.totalamount.Replace(",", "") + "',";
                        }
                        if (string.IsNullOrEmpty(values.total_amount))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.total_amount + "',";
                        }
                mssql += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'" + lsvendor_gid + "'," +
                        "'" + "Invoice Approved" + "'," +
                        "'" + "Payment Pending" + "',";
                        if (!string.IsNullOrEmpty(values.invoice_remarks) && values.invoice_remarks.Contains("'"))
                        {
                            mssql += "'" + values.invoice_remarks.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            mssql += "'" + values.invoice_remarks + "', ";
                        }
                        mssql += "'" + lspurchaseorder_from + "'," +
                                 "'" + "0.00" + "',";
                        if (string.IsNullOrEmpty(values.additional_discount))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.additional_discount.Replace(",", "") + "',";
                        }
                        if (string.IsNullOrEmpty(values.total_amount))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.total_amount + "',";
                        }
                mssql += "'" + values.currency_code + "'," +
                                 "'" + values.exchange_rate + "',";
                        if (string.IsNullOrEmpty(values.freightcharges))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.freightcharges.Replace(",", "") + "',";
                        }
                mssql += "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'" + values.priority_n + "', " +
                                "'" + values.priority_n + "', ";
                        if (string.IsNullOrEmpty(values.buybackorscrap))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.buybackorscrap + "',";
                        }
                mssql += "'" + values.tax_name + "'," +
                                "'" + lsTax_Percentage + "'," +
                                "'" + values.tax_amount4 + "'," +
                                "'" + lsbranchname + "',";
                        if (string.IsNullOrEmpty(values.roundoff))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.roundoff + "',";
                        }
                mssql += "'" + lsTax1_gid + "'," +
                                 "'" + values.taxsegment_gid + "'," +
                                 "'" + values.taxsegmenttax_gid + "',";
                        if (string.IsNullOrEmpty(values.packing_charges))
                        {
                    mssql += "'0.00',";
                        }
                        else
                        {
                    mssql += "'" + values.packing_charges + "',";
                        }
                        if (!string.IsNullOrEmpty(values.payment_terms) && values.payment_terms.Contains("'"))
                        {
                    mssql += "'" + values.payment_terms.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                    mssql += "'" + values.payment_terms + "', ";
                        }
                        if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                        {
                    mssql += "'" + values.template_content.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                    mssql += "'" + values.template_content + "', ";
                        }
                        if (!string.IsNullOrEmpty(values.delivery_term) && values.delivery_term.Contains("'"))
                        {
                    mssql += "'" + values.delivery_term.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                    mssql += "'" + values.delivery_term + "', ";
                        }
                mssql += "'" + values.purchaseorder_gid + "'," +
                                "'" + values.renewal_gid + "'," +
                                "'" + values.purchase_type + "',";
                        if (string.IsNullOrEmpty(values.insurance_charges))
                        {
                    mssql += "'0.00')";
                        }
                        else
                        {
                    mssql += "'" + values.insurance_charges + "')";
                        }

                mnResult = objdbconn.ExecuteNonQuerySQL(mssql);

                msSQL = "SELECT agreement_gid FROM pbl_trn_tagreement WHERE renewal_gid = '" + values.renewal_gid + "'";
                string lsagreement_gid = objdbconn.GetExecuteScalar(msSQL);

                string uiDateStr1 = values.renewal_date;
                DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string renewal_date = uiDate1.ToString("yyyy-MM-dd");
                msSQL = " update pmr_trn_trenewal set " +
                        "renewal_date = '" + renewal_date + "' " +
                        " where agreement_gid = '" + lsagreement_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    mssql = "select finance_flag from adm_mst_tcompany ";
                    string finance_flag = objdbconn.GetExecuteScalar(mssql);
                    if (finance_flag == "Y")
                    {
                        double product;
                        double discount;
                        mssql = "SELECT SUM(COALESCE(qty_invoice, 2) * COALESCE(product_price, 2)) AS product, ROUND(SUM(discount_amount), 2) AS discount FROM acp_trn_tinvoicedtl WHERE invoice_gid = '" + msINGetGID + "'";
                        odbcdr = objdbconn.GetDataReader(mssql);

                        if (odbcdr.HasRows)
                        {
                            odbcdr.Read();
                            product = odbcdr["product"] != DBNull.Value ? double.Parse(odbcdr["product"].ToString()) : 0.00;
                            discount = odbcdr["discount"] != DBNull.Value ? double.Parse(odbcdr["discount"].ToString()) : 0.00;
                        }
                        else
                        {
                            product = 0.00;
                            discount = 0.00;
                        }
                        odbcdr.Close();
                        double lsbasic_amount = product - discount;

                        double addonCharges = double.TryParse(values.addoncharge, out double addonChargesValue) ? addonChargesValue : 0;
                        double freightCharges = double.TryParse(values.freightcharges, out double freightChargesValue) ? freightChargesValue : 0;
                        double forwardingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                        double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                        double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                        double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                        double buybackCharges = double.TryParse(values.buybackorscrap, out double buybackChargesValue) ? buybackChargesValue : 0;
                        double overalltax_amount = double.TryParse(values.tax_amount4, out double overalltaxamount) ? overalltaxamount : 0;
                        double grandtotal = double.TryParse(values.total_amount, out double grand_total) ? grand_total : 0;
                        double ExchangeRate = double.TryParse(values.exchange_rate, out double exchange) ? exchange : 0;

                        double fin_basic_amount = lsbasic_amount * ExchangeRate;
                        double fin_addonCharges = addonCharges * ExchangeRate;
                        double fin_freightcharges = freightCharges * ExchangeRate;
                        double fin_forwardingCharges = forwardingCharges * ExchangeRate;
                        double fin_insuranceCharges = insuranceCharges * ExchangeRate;
                        double fin_roundoff = roundoff * ExchangeRate;
                        double fin_buybackCharges = buybackCharges * ExchangeRate;
                        double fin_overalltax_amount = overalltax_amount * ExchangeRate;
                        double fin_additionaldiscountAmount = additionaldiscountAmount * ExchangeRate;
                        double fin_grandtotal = grandtotal * ExchangeRate;


                        objfincmn.jn_purchase_invoice(invoice_Date, values.invoice_remarks, values.branch_name, ls_referenceno, msINGetGID
                         , fin_basic_amount, fin_addonCharges, fin_additionaldiscountAmount, fin_grandtotal, vendor_gid, "Invoice", "PMR",
                         values.purchase_type, fin_roundoff, fin_freightcharges, fin_buybackCharges, values.tax_name, fin_overalltax_amount, fin_forwardingCharges, fin_insuranceCharges);
                        {
                            OdbcDataReader objODBCDataReader, objODBCDataReader1, objODBCDataReader2, objODBCDataReader3;
                            string lstax_gid, lstaxsum, lstaxamount;

                            objdbconn.OpenConn();
                            string msSQL = "SELECT tax_gid, tax_name, percentage FROM acp_mst_ttax";
                            objODBCDataReader = objdbconn.GetDataReader(msSQL);

                            if (objODBCDataReader.HasRows)
                            {
                                while (objODBCDataReader.Read())
                                {
                                    string lstax1 = "0.00";
                                    string lstax2 = "0.00";
                                    string lstax3 = "0.00";

                                    // Tax 1 Calculation
                                    msSQL = "SELECT SUM(tax_amount) AS tax1 FROM acp_trn_tinvoicedtl " +
                                            "WHERE invoice_gid = '" + msINGetGID + "' AND tax1_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                    objODBCDataReader1 = objdbconn.GetDataReader(msSQL);

                                    if (objODBCDataReader1.HasRows && objODBCDataReader1.Read())
                                    {
                                        lstax1 = objODBCDataReader1["tax1"] != DBNull.Value ? objODBCDataReader1["tax1"].ToString() : "0.00";
                                    }
                                    objODBCDataReader1.Close();

                                    // Tax 2 Calculation
                                    msSQL = "SELECT SUM(tax_amount2) AS tax2 FROM acp_trn_tinvoicedtl " +
                                            "WHERE invoice_gid = '" + msINGetGID + "' AND tax2_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                    objODBCDataReader2 = objdbconn.GetDataReader(msSQL);

                                    if (objODBCDataReader2.HasRows && objODBCDataReader2.Read())
                                    {
                                        lstax2 = objODBCDataReader2["tax2"] != DBNull.Value ? objODBCDataReader2["tax2"].ToString() : "0.00";
                                    }
                                    objODBCDataReader2.Close();

                                }
                            }

                            objdbconn.CloseConn();
                        }


                    }
                    //{
                    //    OdbcDataReader objODBCDataReader, objODBCDataReader1, objODBCDataReader2, objODBCDataReader3;
                    //    string lstax_gid, lstaxsum, lstaxamount;

                    //    objdbconn.OpenConn();
                    //    string msSQL = "SELECT tax_gid, tax_name, percentage FROM acp_mst_ttax";
                    //    objODBCDataReader = objdbconn.GetDataReader(msSQL);

                    //    if (objODBCDataReader.HasRows)
                    //    {
                    //        while (objODBCDataReader.Read())
                    //        {
                    //            string lstax1 = "0.00";
                    //            string lstax2 = "0.00";
                    //            string lstax3 = "0.00";

                    //            Tax 1 Calculation
                    //           msSQL = "SELECT SUM(tax_amount) AS tax1 FROM acp_trn_tinvoicedtl " +
                    //                   "WHERE invoice_gid = '" + msINGetGID + "' AND tax1_gid = '" + objODBCDataReader["tax_gid"] + "'";
                    //            objODBCDataReader1 = objdbconn.GetDataReader(msSQL);

                    //            if (objODBCDataReader1.HasRows && objODBCDataReader1.Read())
                    //            {
                    //                lstax1 = objODBCDataReader1["tax1"] != DBNull.Value ? objODBCDataReader1["tax1"].ToString() : "0.00";
                    //            }
                    //            objODBCDataReader1.Close();

                    //            Tax 2 Calculation
                    //           msSQL = "SELECT SUM(tax_amount2) AS tax2 FROM acp_trn_tinvoicedtl " +
                    //                   "WHERE invoice_gid = '" + msINGetGID + "' AND tax2_gid = '" + objODBCDataReader["tax_gid"] + "'";
                    //            objODBCDataReader2 = objdbconn.GetDataReader(msSQL);

                    //            if (objODBCDataReader2.HasRows && objODBCDataReader2.Read())
                    //            {
                    //                lstax2 = objODBCDataReader2["tax2"] != DBNull.Value ? objODBCDataReader2["tax2"].ToString() : "0.00";
                    //            }
                    //            objODBCDataReader2.Close();

                    //        }
                    //    }

                    //    objdbconn.CloseConn();
                    //}
                    values.status = true;
                    values.message = "Invoice Raised Succesfully !";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while Invoice ";
                }
            }

        }


        public void DaGetagreementtoinvoicetag(MdlPmrTrnPurchaseagreement values, string renewal_gid)
        {
            try
            {
                msSQL = " select vendor_gid from pmr_trn_trenewal where renewal_gid ='" + renewal_gid + "'";
                string vendor_gid = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " select a.invoice_gid,a.invoice_type,a.invoice_refno,date_format(a.invoice_date, '%d-%m-%y') as invoice_date, a.vendor_gid,concat(' ',CAST(format(a.invoice_amount,2) as CHAR )) as invoice_amount," +
                   " concat(e.user_firstname,'-',e.user_lastname) as user_name,concat(c.contactperson_name,' / ',c.contact_telephonenumber,' / ',c.email_id) as customercontact_name, " +
                   " format(total_amount,2) as total_amount, concat(' ',CAST(format(a.invoice_amount,2) as CHAR)) as invoice_amount, a.invoice_status, " +
                   " c.vendor_companyname as company_name,c.vendor_gid from acp_trn_tinvoice a " +
                   "left join acp_mst_tvendor c on c.vendor_gid=a.vendor_gid  left join adm_mst_tuser e on a.user_gid = e.user_gid    " +
                   " where  a.invoice_type<>'Opening Invoice' and a.vendor_gid='" + vendor_gid + "' and a.renewal_gid is null ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoicetagsummary_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new invoicetagsummary_list1
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            contact = dt["customercontact_name"].ToString(),
                            customer_name = dt["company_name"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            renewal_gid = renewal_gid,


                        });
                        values.invoicetagsummary_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostMappedinvoicetag(string user_gid, mapinvoice_lists1 values)
        {
            try
            {
                for (int i = 0; i < values.invoicetagsummary_list1.ToArray().Length; i++)
                {
                    msSQL = " select agreement_gid from pmr_trn_trenewal where renewal_gid ='" + values.renewal_gid + "'";
                    string lsagreement_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " update acp_trn_tinvoice Set " +
                            " invoice_from = 'Agreement' ," +
                            " renewal_gid = '" + values.renewal_gid + "'," +
                            " invoice_reference = '" + lsagreement_gid + "'" +
                            " where invoice_gid = '" + values.invoicetagsummary_list1[i].invoice_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Invoice Tag Succesfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while occured Invoice Tag ";
                    }
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occurred while Mapping product !! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //reports

        public void DaGetRenewalChart(MdlPmrTrnPurchaseagreement values)
        {
            try
            {
                msSQL = "SELECT COUNT(CASE WHEN renewal_status = 'Open' THEN 1 END) AS open_count, " +
                    "COUNT(CASE WHEN renewal_status = 'Closed' THEN 1 END) AS closed_count," +
                    "COUNT(CASE WHEN renewal_status = 'Dropped' THEN 1 END) AS dropped_count " +
                    "FROM pmr_trn_trenewal";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRenewalSummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalSummary_lists
                        {
                            open_count = dt["open_count"].ToString(),
                            closed_count = dt["closed_count"].ToString(),
                            dropped_count = dt["dropped_count"].ToString(),
                        });
                        values.GetRenewalSummary_lists = getModuleList;
                    }

                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Renewal Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetMonthyRenewal(MdlPmrTrnPurchaseagreement values)
        {
            try
            {
                msSQL = "SELECT MONTHNAME(renewal_date) AS renewal_month_name,YEAR(renewal_date) AS renewal_year,COUNT(*) " +
                    "AS renewal_count FROM pmr_trn_trenewal WHERE renewal_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) GROUP BY " +
                    "YEAR(renewal_date), MONTH(renewal_date) ORDER BY YEAR(renewal_date), MONTH(renewal_date)";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMonthlyRenewal_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetMonthlyRenewal_lists
                        {
                            renewal_month_name = dt["renewal_month_name"].ToString(),
                            renewal_year = dt["renewal_year"].ToString(),
                            renewal_count = dt["renewal_count"].ToString(),
                        });
                        values.GetMonthlyRenewal_lists = getModuleList;
                    }

                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Renewal Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetRenewalReportForLastSixMonths(MdlPmrTrnPurchaseagreement values)
        {
            try
            {

                msSQL = " select a.renewal_gid, YEAR(a.renewal_date) AS year, a.renewal_date, SUBSTRING(DATE_FORMAT(a.renewal_date, '%M'),1, 3) AS months , " +
                    " COUNT(a.renewal_gid) AS renewalcount, a.renewal_status " +
                    " from pmr_trn_trenewal a  WHERE  a.renewal_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH)  AND a.renewal_date <= DATE(NOW()) " +
                    " GROUP BY DATE_FORMAT(a.renewal_date, '%M') ORDER BY a.renewal_date DESC ";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetRenewalForLastSixMonths_List = new List<GetRenewalForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetRenewalForLastSixMonths_List.Add(new GetRenewalForLastSixMonths_List
                        {
                            renewal_gid = (dt["renewal_gid"].ToString()),
                            renewal_date = (dt["renewal_date"].ToString()),
                            months = (dt["months"].ToString()),
                            year = (dt["year"].ToString()),
                            renewalcount = (dt["renewalcount"].ToString()),
                            renewal_status = (dt["renewal_status"].ToString()),
                        });
                        values.GetRenewalForLastSixMonths_List = GetRenewalForLastSixMonths_List;
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

        public void DaGetRenewalReportForLastSixMonthsSearch(MdlPmrTrnPurchaseagreement values)
        {
            try
            {

                msSQL = " select a.renewal_gid, YEAR(a.renewal_date) AS year, a.renewal_date, SUBSTRING(DATE_FORMAT(a.renewal_date, '%M'),1, 3) AS months , " +
                    "  COUNT(a.renewal_gid) AS renewalcount, a.renewal_status from pmr_trn_trenewal a " +
                    "  WHERE  a.renewal_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH)  AND a.renewal_date <= DATE(NOW()) " +
                    "  GROUP BY DATE_FORMAT(a.renewal_date, '%M') ORDER BY a.renewal_date DESC  ";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetRenewalForLastSixMonths_List = new List<GetRenewalForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetRenewalForLastSixMonths_List.Add(new GetRenewalForLastSixMonths_List
                        {
                            renewal_gid = (dt["renewal_gid"].ToString()),
                            renewal_date = (dt["renewal_date"].ToString()),
                            months = (dt["months"].ToString()),
                            year = (dt["year"].ToString()),
                            renewalcount = (dt["renewalcount"].ToString()),
                            renewal_status = (dt["renewal_status"].ToString()),
                        });
                        values.GetRenewalForLastSixMonths_List = GetRenewalForLastSixMonths_List;
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

        public void DaGetrenewalDetailSummary(string branch_gid, string month, string year, MdlPmrTrnPurchaseagreement values)
        {
            try
            {

                string msSQL = "  SELECT  a.renewal_gid,  b.purchaseorder_gid AS order_agreement_gid,a.vendor_gid, a.renewal_type AS renewal, DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,c.vendor_companyname as customer_name,CONCAT(c.contactperson_name, ' / ', c.contact_telephonenumber, ' / ', c.email_id) AS contact_details," +
                             "  FORMAT(b.total_amount, 2) AS Grandtotal, DATE_FORMAT(b.purchaseorder_date, '%d-%m-%Y') AS order_agreement_date," +
                             " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration,a.renewal_status  FROM pmr_trn_trenewal a  " +
                             " LEFT JOIN acp_mst_tvendor c ON a.vendor_gid = c.vendor_gid LEFT JOIN pmr_trn_tpurchaseorder b ON b.purchaseorder_gid = a.purchaseorder_gid " +
                             " where substring(date_format(a.renewal_date,'%M'),1,3)='" + month + "' and year(a.renewal_date)='" + year + "' and  a.renewal_flag='Y' UNION " +
                             " SELECT a.renewal_gid,b.agreement_gid AS order_agreement_gid,a.vendor_gid, a.renewal_type AS renewal, DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,c.vendor_companyname as customer_name," +
                             "  CONCAT(c.contactperson_name, ' / ', c.contact_telephonenumber, ' / ', c.email_id) AS contact_details,FORMAT(b.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(b.agreement_date, '%d-%m-%Y') AS order_agreement_date,\r\n" +
                             "  CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration,a.renewal_status FROM pmr_trn_trenewal a " +
                             "  LEFT JOIN acp_mst_tvendor c ON a.vendor_gid = c.vendor_gid LEFT JOIN pbl_trn_tagreement b ON b.agreement_gid = a.agreement_gid " +
                             " where substring(date_format(a.renewal_date,'%M'),1,3)='" + month + "' and year(a.renewal_date)='" + year + "' AND a.renewal_type = 'Agreement' ORDER BY renewal_gid DESC";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetRenewalDetailSummarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalDetailSummarylist
                        {
                            renewal_gid = dt["renewal_gid"].ToString(),
                            renewal_status = dt["renewal_status"].ToString(),
                            duration = dt["duration"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            renewal = dt["renewal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                        });
                    }
                    values.GetRenewalDetailSummarylist = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetEditAgreementSummary(string user_gid, string renewal_gid, MdlPmrTrnPurchaseagreement values)
        {
            try
            {
                msSQL = " delete from pmr_tmp_tpurchaseorder where " +
                        " created_by = '" + user_gid + "' ";


                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (renewal_gid.Contains("PPOP"))
                {
                    msSQL = "  select i.purchaseorderdtl_gid as order_agreementdtl_gid,i.purchaseorder_gid as order_agreement_gid,i.producttotal_amount," +
                        " format(i.product_price_L,2) as product_price_L, i.tax_name,i.tax_name2,i.tax_name3,i.qty_ordered as qyt_unit,i.product_gid,i.product_price," +
                        " i.qty_ordered, format(i.discount_percentage, 2) as discount_percentage, format(i.discount_amount, 2) discount_amount1," +
                        "  format(i.tax_percentage, 2) as tax_percentage, format(i.tax_percentage2, 2) as tax_percentage2," +
                        "  format(i.tax_percentage3, 2) as tax_percentage3, format(i.tax_amount, 2) as tax_amount," +
                        " CASE  WHEN i.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(i.tax_amount2, 2) END AS tax_amount2," +
                        " format(i.tax_amount3, 2) as tax_amount3,  i.product_remarks, format((qty_ordered * i.product_price), 2) as product_totalprice," +
                        " format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as product_total," +
                        " i.product_code, (i.product_name) as product_name,i.productuom_name,i.display_field_name," +
                        " i.tax1_gid,i.tax2_gid, concat(i.tax_name,' ,',i.tax_name2) as taxesname, concat(i.tax_amount,' , ',i.tax_amount2) as taxesamt" +
                        " from pmr_trn_tpurchaseorderdtl  i" +
                        " left join pmr_trn_tpurchaseorder b on b.purchaseorder_gid = i.purchaseorder_gid" +
                        "  left join pmr_trn_trenewal c on c.purchaseorder_gid = i.purchaseorder_gid where i.purchaseorder_gid = '" + renewal_gid + "'";
                }
                else
                {

                    msSQL = "  select i.agreementdtl_gid as order_agreementdtl_gid,i.agreement_gid as order_agreement_gid,i.producttotal_amount, format(i.product_price_L,2) as product_price_L," +
                        " i.tax_name,i.tax_name2,i.tax_name3,i.qty_ordered as qyt_unit,i.product_gid,i.product_price," +
                        " i.qty_ordered, format(i.discount_percentage, 2) as discount_percentage," +
                        " format(i.discount_amount, 2) discount_amount1,  format(i.tax_percentage, 2) as tax_percentage," +
                        " format(i.tax_percentage2, 2) as tax_percentage2,  format(i.tax_percentage3, 2) as tax_percentage3," +
                        " format(i.tax_amount, 2) as tax_amount, CASE  WHEN i.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(i.tax_amount2, 2) END AS tax_amount2," +
                        " format(i.tax_amount3, 2) as tax_amount3,  i.product_remarks, format((qty_ordered * i.product_price), 2) as product_totalprice," +
                        " format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as product_total," +
                        " i.product_code, (i.product_name) as product_name,i.productuom_name,i.display_field_name, i.tax1_gid,i.tax2_gid, concat(i.tax_name,' ,',i.tax_name2) as taxesname," +
                        " concat(i.tax_amount,' , ',i.tax_amount2) as taxesamt from pbl_trn_tagreementdtl  i" +
                        " left join pbl_trn_tagreement b on b.agreement_gid = i.agreement_gid" +
                        " left join pmr_trn_trenewal c on i.agreement_gid = c.agreement_gid where i.agreement_gid ='" + renewal_gid + "'";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow da in dt_datatable.Rows)
                    {
                        msPOGetGID = objcmnfunctions.GetMasterGID("VSDC"); ;

                        msSQL = " insert into pmr_tmp_tpurchaseorder ( " +
                                " tmppurchaseorderdtl_gid," +
                                " tmppurchaseorder_gid," +
                                " qty_poadjusted," +
                                " product_gid," +
                                " product_code," +
                                " product_name," +
                                " productuom_name," +
                                " qty_ordered," +
                                " display_field," +
                                " product_price," +
                                " discount_percentage," +
                                " discount_amount," +
                                " tax_name," +
                                " tax_name2," +
                                " tax_percentage," +
                                " tax_percentage2," +
                                " tax1_gid," +
                                " tax2_gid," +
                                " tax_amount, " +
                                " tax_amount2, " +
                                " created_by," +
                                " producttotal_price " +
                                " ) values ( " +
                                "'" + msPOGetGID + "'," +
                                "'" + da["order_agreement_gid"].ToString() + "'," +
                                "'" + da["qty_ordered"].ToString() + "'," +
                                "'" + da["product_gid"].ToString() + "'," +
                                "'" + da["product_code"].ToString() + "'," +
                                "'" + da["product_name"].ToString() + "'," +
                                "'" + da["productuom_name"].ToString() + "'," +
                                "'" + da["qty_ordered"].ToString() + "'," +
                                "'" + da["display_field_name"].ToString() + "'," +
                                "'" + da["product_price"].ToString() + "',";
                        if (string.IsNullOrEmpty(da["discount_percentage"].ToString()))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + da["discount_percentage"].ToString() + "',";
                        }

                        if (string.IsNullOrEmpty(da["discount_amount1"].ToString()))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + da["discount_amount1"].ToString().Replace(",", "") + "',";
                        }
                        msSQL += "'" + da["tax_name"].ToString() + "'," +
                        "'" + da["tax_name2"].ToString() + "'," +
                        "'" + da["tax_percentage"].ToString() + "'," +
                        "'" + da["tax_percentage2"].ToString() + "'," +
                        "'" + da["tax1_gid"].ToString() + "'," +
                        "'" + da["tax2_gid"].ToString() + "',";


                        if (string.IsNullOrEmpty(da["tax_amount"].ToString()))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + da["tax_amount"].ToString().Replace(",", "") + "',";
                        }
                        if (string.IsNullOrEmpty(da["tax_amount2"].ToString()))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + da["tax_amount2"].ToString().Replace(",", "") + "',";
                        }
                        msSQL += "'" + user_gid + "',";
                        if (string.IsNullOrEmpty(da["producttotal_amount"].ToString()))
                        {
                            msSQL += "'0.00')";
                        }
                        else
                        {
                            msSQL += "'" + da["producttotal_amount"].ToString() + "')";
                        }

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                if (mnResult == 1)
                {
                    values.status = true;
                }



                if (renewal_gid.Contains("PPOP"))
                {
                    msSQL = "  select a.vendor_address as customer_address,r.renewal_gid,i.producttotal_amount,a.vendor_gid as customer_gid,r.renewal_description,r.renewal_date," +
                        " a.freight_terms as delivery_terms,a.payment_terms as payment_terms,a.mode_despatch, a.shipping_address," +
                        " DATE_FORMAT(a.purchaseorder_date, '%d-%m-%Y') AS Created_date,concat (a.purchaseorder_gid) as purchaseorder_gid," +
                        " m.user_firstname as requested_by,  b.gst_no,  b.email_id,b.contact_telephonenumber,b.contactperson_name,a.tax_amount as overalltax ," +
                        " format(i.product_price_L,2) as product_price_L,  case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate," +
                        " w.address2,i.tax_name,i.tax_name2,i.tax_name3,  a.branch_gid,  date_format(r.renewal_date, '%d-%m-%Y') as renewal_date1 ," +
                        " a.created_by, format(a.total_amount, 2) as total_amount ,  concat(f.address1,f.postal_code) as branch_add1," +
                        " concat( w.address1 ,' ', w.address2, ' ', w.city, '  ', w.postal_code) AS bill_to, a.termsandconditions," +
                        " b.vendor_companyname, g.user_firstname as approved_by, i.qty_ordered as qyt_unit ," +
                        " concat(c.user_firstname, ' - ', e.department_name) as user_firstname,  d.employee_emailid, d.employee_mobileno," +
                        " f.branch_name, format(a.discount_amount, 2) as discount_amount, format(a.tax_percentage, 2) as tax_percentage, " +
                        " format(a.addon_amount, 2) as addon_amount,format(a.roundoff, 2) as roundoff,   a.payment_days,y.tax_gid,a.delivery_days," +
                        " format(a.freightcharges,2) as freightcharges,format(a.discount_amount,2) as additional_discount, i.product_gid," +
                        " i.product_price, i.qty_ordered, format(i.discount_percentage, 2) as discount_percentage ," +
                        " format(i.discount_amount, 2) discount_amount1 ,  format(i.tax_percentage, 2) as tax_percentage," +
                        " format(i.tax_percentage2, 2) as tax_percentage2,  format(i.tax_percentage3, 2) as tax_percentage3," +
                        " format(i.tax_amount, 2) as tax_amount," +
                        " CASE  WHEN i.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(i.tax_amount2, 2) END AS tax_amount2," +
                        " format(i.tax_amount3, 2) as tax_amount3,  i.product_remarks, format((qty_ordered * i.product_price), 2) as product_totalprice," +
                        " format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as product_total," +
                        " i.product_code, (i.product_name) as product_name,format(a.netamount, 2) as netamount, k.productgroup_name,i.productuom_name," +
                        " i.display_field_name,a.purchaseorder_remarks as agreement_remarks, a.currency_code," +
                        " i.tax1_gid,y.tax_name as overalltaxname,v.currencyexchange_gid as currency_gid, concat(i.tax_name,' ,',i.tax_name2) as taxesname, concat(i.tax_amount,' , ',i.tax_amount2) as taxesamt" +
                        " from pmr_trn_tpurchaseorder a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid  left join adm_mst_taddress w on b.address_gid = w.address_gid " +
                        " left join adm_mst_tuser c on c.user_gid = a.created_by left join hrm_mst_temployee d on d.user_gid = c.user_gid" +
                        " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid  left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid" +
                        " left join adm_mst_tuser g on g.user_gid = a.approved_by left join pmr_trn_tpurchaseorderdtl i ON i.purchaseorder_gid = a.purchaseorder_gid" +
                        " left join acp_mst_ttax y on y.tax_gid = a.tax_gid left join pmr_mst_tproduct j on i.product_gid = j.product_gid" +
                        " left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid" +
                        " left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid" +
                        " left join adm_mst_tuser m on m.user_gid = a.created_by" +
                        " left join crm_trn_tcurrencyexchange v on v.currency_code = a.currency_code" +
                        " left join crm_trn_tcurrencyexchange z on a.currency_code = z.currencyexchange_gid" +
                        " LEFT JOIN  pmr_trn_trenewal r ON a.purchaseorder_gid = r.purchaseorder_gid " +
                        " where r.purchaseorder_gid = '" + renewal_gid + "'";
                }
                else
                {
                    msSQL = "  select a.customer_address,r.renewal_gid,i.producttotal_amount,a.customer_gid,a.renewal_description,r.renewal_date,a.delivery_days as delivery_terms,a.payment_days as payment_terms,a.mode_despatch, a.shipping_address, DATE_FORMAT(a.agreement_date, '%d-%m-%Y') AS Created_date,concat (a.agreement_gid) as purchaseorder_gid,m.user_firstname as requested_by,\r\n " +
                            " b.gst_no,  b.email_id,b.contact_telephonenumber,b.contactperson_name,a.tax_amount as overalltax ,\r\n  " +
                            "  format(i.product_price_L,2) as product_price_L,  case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,\r\n " +
                            "  w.address2,i.tax_name,i.tax_name2,i.tax_name3,  a.branch_gid,  date_format(r.renewal_date, '%d-%m-%Y') as renewal_date1 ,\r\n " +
                            "  a.created_by, format(a.Grandtotal, 2) as total_amount ,  concat(f.address1,f.postal_code) as branch_add1,   \r\n " +
                            "  concat( w.address1 ,' ', w.address2, ' ', w.city, '  ', w.postal_code) AS bill_to, a.termsandconditions,\r\n " +
                            "  b.vendor_companyname, g.user_firstname as approved_by, i.qty_ordered as qyt_unit ,concat(c.user_firstname, ' - ', e.department_name) as user_firstname,  d.employee_emailid, d.employee_mobileno,\r\n " +
                            "  f.branch_name, format(a.discount_amount, 2) as discount_amount, format(a.tax_percentage, 2) as tax_percentage, \r\n" +
                            "  a.addon_amount as addon_amount,a.roundoff as roundoff,   a.payment_days,y.tax_gid,a.delivery_days,\r\n " +
                            "  a.freightcharges as freightcharges,a.discount_amount as additional_discount, i.product_gid,\r\n " +
                            "   i.product_price, i.qty_ordered, format(i.discount_percentage, 2) as discount_percentage , \r\n" +
                            "   format(i.discount_amount, 2) discount_amount1 ,  format(i.tax_percentage, 2) as tax_percentage, \r\n" +
                            "   format(i.tax_percentage2, 2) as tax_percentage2,  format(i.tax_percentage3, 2) as tax_percentage3, format(i.tax_amount, 2) as tax_amount, \r\n" +
                            "  CASE  WHEN i.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(i.tax_amount2, 2) END AS tax_amount2,  \r\n " +
                            "  format(i.tax_amount3, 2) as tax_amount3,  i.product_remarks, format((qty_ordered * i.product_price), 2) as product_totalprice, \r\n " +
                            "  format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as product_total,\r\n " +
                            "  i.product_code, (i.product_name) as product_name,format(a.netamount, 2) as netamount, k.productgroup_name,i.productuom_name,i.display_field_name,a.agreement_remarks, a.currency_code,a.currency_gid,\r\n " +
                            "  i.tax1_gid,y.tax_name as overalltaxname  , concat(i.tax_name,' ,',i.tax_name2) as taxesname, concat(i.tax_amount,' , ',i.tax_amount2) as taxesamt from pbl_trn_tagreement a \r\n " +
                            "   left join acp_mst_tvendor b on a.customer_gid = b.vendor_gid  left join adm_mst_taddress w on b.address_gid = w.address_gid  \r\n" +
                            "   left join adm_mst_tuser c on c.user_gid = a.created_by left join hrm_mst_temployee d on d.user_gid = c.user_gid \r\n" +
                            "  left join hrm_mst_tdepartment e on e.department_gid = d.department_gid  left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid \r\n " +
                            "  left join adm_mst_tuser g on g.user_gid = a.approved_by left join pbl_trn_tagreementdtl i ON i.agreement_gid = a.agreement_gid  \r\n " +
                            "  left join acp_mst_ttax y on y.tax_gid = a.tax_gid left join pmr_mst_tproduct j on i.product_gid = j.product_gid \r\n " +
                            "  left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid \r\n " +
                            "   left join adm_mst_tuser m on m.user_gid = a.created_by left join crm_trn_tcurrencyexchange z on a.currency_code = z.currencyexchange_gid\r\n" +
                            " LEFT JOIN  pmr_trn_trenewal r ON a.agreement_gid = r.agreement_gid  " +
                            " where r.agreement_gid = '" + renewal_gid + "'";
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetViewPurchaseagreement>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetViewPurchaseagreement
                        {
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            Created_date = dt["Created_date"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            tax_number = dt["gst_no"].ToString(),
                            address2 = dt["address2"].ToString(),
                            requested_by = dt["requested_by"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            display_field_name = dt["display_field_name"].ToString(),
                            qyt_unit = dt["qyt_unit"].ToString(),
                            product_price_L = dt["product_price_L"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount1 = dt["discount_amount1"].ToString(),
                            //tax_name = dt["taxesname"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            netamount = dt["netamount"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            overalltaxname = dt["overalltaxname"].ToString(),
                            overalltax = dt["overalltax"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            grandtotal = dt["total_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            bill_to = dt["bill_to"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            renewal_date = dt["renewal_date1"].ToString(),
                            producttotal_amount = dt["producttotal_amount"].ToString(),
                            renewal_description = dt["renewal_description"].ToString(),
                            delivery_terms = dt["delivery_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            currency_gid = dt["currency_gid"].ToString(),
                            agreement_remarks = dt["agreement_remarks"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            renewal_gid = dt["renewal_gid"].ToString(),



                        });

                        values.GetViewPurchaseagreement = getModuleList;

                    }
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostUpdateAgreement(string user_gid, GetViewPurchaseagreement values)
        {
            msSQL = " select * from pmr_tmp_tpurchaseorder " +
                     " where created_by='" + user_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count != 0)
            {
                int lsfreight = 0;
                int lsinsurance = 0;
                string uiDateStr = values.agreement_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string agreement_date = uiDate.ToString("yyyy-MM-dd");

                string uiDateStr1 = values.renewal_date;
                DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string renewal_date = uiDate1.ToString("yyyy-MM-dd");
                string lstax_gid;

                msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "' ";
                string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = "select poapproval_flag from adm_mst_tcompany";
                //string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                if (string.IsNullOrEmpty(values.tax_name4))
                {
                    lstaxpercentage = "0";

                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                    lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.po_no.Contains("PPOP"))
                {
                    msSQL = "UPDATE pmr_trn_tpurchaseorder SET " +
                        "branch_gid = '" + values.branch_name + "', " +
                        "purchaseorder_date = '" + agreement_date + "', " +
                        "vendor_gid = '" + values.vendor_companyname + "', " +
                        "vendor_address = '" + values.address1 + "', " +
                        "shipping_address = '" + values.shipping_address + "', " +
                        "created_by = '" + user_gid + "', " +
                        "mode_despatch = '" + values.mode_despatch + "', " +
                        "purchaseorder_remarks = '" + values.po_covernote + "', " +
                        "purchaseorder_reference = '" + values.po_no + "', " +
                        "payment_days = '" + values.payment_days + "', " +
                        "delivery_days = '" + values.delivery_days + "', " +
                        "total_amount = '" + values.grandtotal.Replace(",", "").Trim() + "', " +
                        "total_amount_l = '" + values.grandtotal.Replace(",", "").Trim() + "', " +
                        "termsandconditions = '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "") + "', " +
                        "purchaseorder_status = 'PO Approved', " +
                        "purchase_type = 'Purchase', " +
                        "addon_amount = '" + (string.IsNullOrEmpty(values.addoncharge) ? "0.00" : values.addoncharge.Replace(",", "").Trim()) + "', " +
                        "addon_amount_l = '" + (string.IsNullOrEmpty(values.addoncharge) ? "0.00" : values.addoncharge.Replace(",", "").Trim()) + "', " +
                        "additional_discount_L = '" + (string.IsNullOrEmpty(values.additional_discount) ? "0.00" : values.additional_discount.Replace(",", "").Trim()) + "', " +
                        "additional_discount_l = '" + (string.IsNullOrEmpty(values.additional_discount) ? "0.00" : values.additional_discount.Replace(",", "").Trim()) + "', " +
                        "currency_code = '" + lscurrency_code + "', " +
                        "exchange_rate = '" + values.exchange_rate + "', " +
                        "purchaseorder_date = '" + renewal_date + "', " +
                        "purchaseorder_remarks = '" + values.po_covernote + "', " +
                        "netamount = '" + values.total_amount + "', " +
                        "addon_amount = '" + values.addoncharge + "', " +
                        "freightcharges = '" + (string.IsNullOrEmpty(values.freightcharges) ? "0.00" : values.freightcharges) + "', " +
                        "discount_amount = '" + (string.IsNullOrEmpty(values.additional_discount) ? "0.00" : values.additional_discount) + "', " +
                        "tax_gid = '" + values.tax_name4 + "', " +
                        "tax_percentage = '" + lstaxpercentage + "', " +
                        "tax_amount = '" + (string.IsNullOrEmpty(values.tax_amount4) ? "0.00" : values.tax_amount4.Replace(",", "").Trim()) + "', " +
                        "roundoff = '" + (string.IsNullOrEmpty(values.roundoff) ? "0.00" : values.roundoff) + "', " +
                        "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                        "taxsegment_gid = '" + values.taxsegment_gid + "' " +
                        "WHERE purchaseorder_gid = '" + values.po_no + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = " Some Error Occurred While Inserting Agreement order Details";
                        return;
                    }
                    else
                    {

                        msSQL = "UPDATE pmr_trn_trenewal SET " +
                           "vendor_gid = '" + values.vendor_companyname + "', " +
                           "renewal_date = '" + renewal_date + "', " +
                           "purchaseorder_gid = '" + values.po_no + "', " +
                           "created_by = '" + user_gid + "', " +
                           "renewal_type = 'Purchase', " +
                           "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                           "WHERE renewal_gid = '" + values.renewal_gid + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }
                else
                {
                    msSQL = "UPDATE pbl_trn_tagreement SET " +
                                        "branch_gid = '" + values.branch_name + "', " +
                                        "agreement_date = '" + agreement_date + "', " +
                                        "customer_gid = '" + values.vendor_companyname + "', ";
                                        if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                                        {
                                            msSQL += "customer_address = '" + values.address1.Replace("'", "\\\'") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "customer_address = '" + values.address1 + "', ";
                                        }
                                        if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                                        {
                                            msSQL += "shipping_address = '" + values.shipping_address.Replace("'", "\\\'") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "shipping_address = '" + values.shipping_address + "', ";
                                        }

                               msSQL += "created_by = '" + user_gid + "', ";
                                        if (!string.IsNullOrEmpty(values.mode_despatch) && values.mode_despatch.Contains("'"))
                                        {
                                            msSQL += "mode_despatch = '" + values.mode_despatch.Replace("'", "\\\'") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "mode_despatch = '" + values.mode_despatch + "', ";
                                        }
                                        if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                                        {
                                            msSQL += "agreement_remarks = '" + values.po_covernote.Replace("'", "\\\'") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "agreement_remarks = '" + values.po_covernote + "', ";
                                        }
                                msSQL += "agreement_referenceno1 = '" + values.po_no + "', " +
                                         "payment_days = '" + values.payment_days + "', " +
                                         "delivery_days = '" + values.delivery_days + "', " +
                                         "Grandtotal = '" + values.grandtotal.Replace(",", "").Trim() + "', " +
                                         "grandtotal_l = '" + values.grandtotal.Replace(",", "").Trim() + "', ";
                                        if (!string.IsNullOrEmpty(values.termsandconditions) && values.termsandconditions.Contains("'"))
                                        {
                                            msSQL += "termsandconditions = '" + values.termsandconditions.Replace("'", "\\\'") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "termsandconditions = '" + values.termsandconditions + "', ";
                                        }
                                msSQL += "agreement_status = 'PO Approved', " +
                                         "agreement_type = 'Agreement', " +
                                         "addon_charge = '" + (string.IsNullOrEmpty(values.addoncharge) ? "0.00" : values.addoncharge.Replace(",", "").Trim()) + "', " +
                                         "addon_charge_l = '" + (string.IsNullOrEmpty(values.addoncharge) ? "0.00" : values.addoncharge.Replace(",", "").Trim()) + "', " +
                                         "additional_discount = '" + (string.IsNullOrEmpty(values.additional_discount) ? "0.00" : values.additional_discount.Replace(",", "").Trim()) + "', " +
                                         "additional_discount_l = '" + (string.IsNullOrEmpty(values.additional_discount) ? "0.00" : values.additional_discount.Replace(",", "").Trim()) + "', " +
                                         "currency_code = '" + lscurrency_code + "', " +
                                         "currency_gid = '" + values.currency_code + "', " +
                                         "exchange_rate = '" + values.exchange_rate + "', " +
                                         "renewal_date = '" + renewal_date + "', " +
                                         "renewal_gid = '" + values.renewal_gid + "', ";
                                        if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                                        {
                                            msSQL += "renewal_description = '" + values.po_covernote.Replace("'", "\\\'") + "', ";
                                        }
                                        else
                                        {
                                            msSQL += "renewal_description = '" + values.po_covernote + "', ";
                                        }
                               msSQL += "netamount = '" + values.total_amount + "', " +
                                        "addon_amount = '" + values.addoncharge + "', " +
                                        "freightcharges = '" + (string.IsNullOrEmpty(values.freightcharges) ? "0.00" : values.freightcharges) + "', " +
                                        "discount_amount = '" + (string.IsNullOrEmpty(values.additional_discount) ? "0.00" : values.additional_discount) + "', " +
                                        "tax_gid = '" + values.tax_name4 + "', " +
                                        "tax_percentage = '" + lstaxpercentage + "', " +
                                        "tax_amount = '" + (string.IsNullOrEmpty(values.tax_amount4) ? "0.00" : values.tax_amount4.Replace(",", "").Trim()) + "', " +
                                        "roundoff = '" + (string.IsNullOrEmpty(values.roundoff) ? "0.00" : values.roundoff) + "', " +
                                        "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                        "taxsegment_gid = '" + values.taxsegment_gid + "' " +
                                        "WHERE agreement_gid = '" + values.po_no + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);




                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = " Some Error Occurred While Inserting Agreement order Details";
                        return;
                    }
                    else
                    {

                        msSQL = "UPDATE pmr_trn_trenewal SET " +
                           "vendor_gid = '" + values.vendor_companyname + "', " +
                           "renewal_date = '" + renewal_date + "', " +
                           "agreement_gid = '" + values.po_no + "', " +
                           "created_by = '" + user_gid + "', " +
                           "renewal_type = 'Agreement', " +
                           "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                           "WHERE renewal_gid = '" + values.renewal_gid + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }

                if (mnResult == 1)
                {

                    msSQL = "SELECT a.tmppurchaseorderdtl_gid, a.tmppurchaseorder_gid, a.product_gid, a.product_name, a.product_code, b.productgroup_gid, " +
                        "FORMAT(a.qty_ordered, 2) AS qty_ordered, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                        "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price, " +
                        "a.discount_percentage, FORMAT(a.discount_amount, 2) AS discount_amount, " +
                        "a.tax_percentage, FORMAT(a.tax_amount, 2) AS tax_amount, FORMAT(a.product_total, 2) AS product_total, a.needby_date, " +
                        "a.type, a.uom_gid, a.display_field, " +
                        "a.purchaserequisitiondtl_gid, a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2, " +
                        "a.tax_percentage3, FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3, " +
                        "a.tax1_gid, a.tax2_gid, a.tax3_gid, a.taxsegment_gid " +
                        "FROM pmr_tmp_tpurchaseorder a " +
                        "LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                        "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
                        "LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid " +
                        "WHERE a.created_by = '" + user_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetViewPurchaseagreement>();

                    msSQL = "  delete from pbl_trn_tagreementdtl where agreement_gid='" + values.po_no + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {

                            msPOGetGID = objcmnfunctions.GetMasterGID("VSDC");
                            msGetagreementGID = objcmnfunctions.GetMasterGID("VSOP");

                            msSQL = " insert into pbl_trn_tagreementdtl (" +
                                        " agreementdtl_gid, " +
                                        " agreement_gid, " +
                                        " product_gid, " +
                                        " product_code, " +
                                        " product_name, " +
                                        " productuom_name, " +
                                        " uom_gid, " +
                                        " producttype_gid, " +
                                        " product_price, " +
                                        " producttotal_amount, " +
                                        " discount_percentage, " +
                                        " discount_amount, " +
                                        " tax_name, " +
                                        " tax_name2, " +
                                        " tax_name3, " +
                                        " tax1_gid, " +
                                        " tax2_gid, " +
                                        " tax3_gid, " +
                                        " qty_ordered, " +
                                        " product_price_L, " +
                                        " display_field_name," +
                                        " tax_amount1_L," +
                                        " tax_amount2_L," +
                                        " tax_amount3_L," +
                                         " tax_amount," +
                                        " tax_amount2," +
                                        " tax_amount3," +
                                        " tax_percentage," +
                                        " tax_percentage2," +
                                        " tax_percentage3," +
                                        " created_by," +
                                        " taxsegment_gid" +
                                        " )values ( " +
                                        "'" + msPOGetGID + "', " +
                                        "'" + values.po_no + "'," +
                                        "'" + dt["product_gid"].ToString() + "', " +
                                        "'" + dt["product_code"].ToString() + "', " +
                                        "'" + dt["product_name"].ToString() + "', " +
                                        "'" + dt["productuom_name"].ToString() + "', " +
                                        "'" + dt["uom_gid"].ToString() + "', " +
                                        "'" + dt["type"].ToString() + "', " +
                                        "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                        "'" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                        "'" + dt["discount_percentage"].ToString().Replace(",", "") + "', ";
                            if (string.IsNullOrEmpty(dt["discount_amount"].ToString()))
                            {
                                msSQL += "'0.00',";

                            }
                            else
                            {
                                msSQL += "'" + dt["discount_amount"].ToString().Replace(",", "") + "',";
                            }
                            msSQL += "'" + dt["tax_name"].ToString() + "', " +
                            "'" + dt["tax_name2"].ToString() + "', " +
                            "'" + dt["tax_name3"].ToString() + "', " +
                            "'" + dt["tax1_gid"].ToString() + "', " +
                            "'" + dt["tax2_gid"].ToString() + "', " +
                            "'" + dt["tax3_gid"].ToString() + "', " +
                            "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                            "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                            "'" + dt["display_field"].ToString() + "'," +
                            "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                              "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_percentage"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_percentage2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_percentage3"].ToString().Replace(",", "") + "'," +
                            "'" + user_gid + "'," +
                            "'" + dt["taxsegment_gid"].ToString() + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                    }

                }

                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Agreement Order Updated Successfully!";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating product in Agreement Order!";


                }

                msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                dt_datatable.Dispose();


            }
        }
    }
}