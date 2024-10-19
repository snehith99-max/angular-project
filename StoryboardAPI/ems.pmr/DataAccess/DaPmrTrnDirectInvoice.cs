using ems.pmr.Models;
using ems.utilities.Functions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;


namespace ems.pmr.DataAccess
{
    public class DaPmrTrnDirectInvoice
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        finance_cmnfunction objfincmn = new finance_cmnfunction();
        DataTable dt_datatable;
        DataSet ds_dataset;
        int mnResult;
        string msgetGID, msGetGID, msDIGetGID, txtinvoicefer, msGetGid, invoicedate, paymentdate ,lsproductgid, msGetGIDPO, msPO1GetGID, msPO2GetGID, msPOGetGID3,
            lsproductuom_gid, lsproduct_name, lsproducttype_name, lsproductgroup_name, lsproductuom_name, lstaxpercentage, lstax_name4;
        double lsbasic_amount;

        public void DaGetBranchName(MdlPmrTrnDirectInvoice values)
        {
            try
            {
                msSQL = " select branch_name, branch_gid from hrm_mst_tbranch ";

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
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetVendornamedropDown(MdlPmrTrnDirectInvoice values)
        {
            try
            {
                msSQL = "Select vendor_gid,vendor_companyname from acp_mst_tvendor where blacklist_flag <>'Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetVendornamedropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendornamedropdown
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                        });
                        values.GetVendornamedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetOnChangeVendor(string vendor_gid, MdlPmrTrnDirectInvoice values)
        {
            try
            {

                msSQL = "   select b.address1, b.address2, b.city, b.state, b.postal_code, b.fax,a.contactperson_name, a.vendor_companyname,a.payment_terms," +
                       " a.contact_telephonenumber,a.gst_no, a.taxsegment_gid,c.country_name,a.email_id,a.currencyexchange_gid from acp_mst_tvendor a " +
                       " left join adm_mst_taddress b on b.address_gid = a.address_gid " +
                       " left join adm_mst_tcountry c on c.country_gid = b.country_gid" +
                       " where a.vendor_gid  ='" + vendor_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOnChangeVendor>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnChangeVendor
                        {

                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            postal_code = dt["postal_code"].ToString(),
                            fax = dt["fax"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            gst_no = dt["gst_no"].ToString(),

                        });
                        values.GetOnChangeVendor = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetcurrencyCodedropdown(MdlPmrTrnDirectInvoice values)
        {
            try
            {

                msSQL = " select distinct a.currencyexchange_gid,a.currency_code,a.default_currency,a.exchange_rate from  crm_trn_tcurrencyexchange a " +
   " left join acp_mst_tvendor b on a.currencyexchange_gid = b.currencyexchange_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL, "crm_trn_tcurrencyexchange");

                var getModuleList = new List<Getcurrencycodedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcurrencycodedropdown
                        {
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            default_currency = dt["default_currency"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                        });
                        values.Getcurrencycodedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting currecny!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetPurchaseTypedropDown(MdlPmrTrnDirectInvoice values)
        {
            try
            {
                msSQL = " Select a.account_gid, a.purchasetype_name from pmr_trn_tpurchasetype a where a.account_gid <> 'null' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetPurchaseTypedropDown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPurchaseTypedropDown
                        {
                            account_gid = dt["account_gid"].ToString(),
                            purchasetype_name = dt["purchasetype_name"].ToString(),
                        });
                        values.GetPurchaseTypedropDown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGettaxnamedropdown(MdlPmrTrnDirectInvoice values)
        {
            try
            {
                msSQL = " select tax_gid, tax_name, percentage from acp_mst_ttax ";

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
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public async Task DaGetPblProductsearchSummary(string producttype_gid, string product_name, string vendor_gid, MdlPmrTrnDirectInvoice values)
        {
            try
            {
                // Fetch products synchronously
                DataTable productDt = objdbconn.GetDataTable(GetProductQuery(producttype_gid, product_name));
                var getModuleList = ProcessProductData(productDt);

                // Fetch tax segments synchronously if vendor_gid is provided
                DataTable taxDt = null;
                if (!string.IsNullOrEmpty(vendor_gid) && vendor_gid != "undefined")
                {
                    taxDt = objdbconn.GetDataTable(GetTaxSegmentQuery(vendor_gid));
                }
                var allTaxSegmentsList = ProcessTaxSegmentData(taxDt);

                // Assign lists to values
                values.GetPblProductsearch = getModuleList;
                values.GetPblTaxSegmentList = allTaxSegmentsList;
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        private string GetProductQuery(string producttype_gid, string product_name)
        {
            StringBuilder query = new StringBuilder("SELECT a.product_name, a.product_code, a.product_gid, " +
                " a.cost_price, b.productuom_gid, b.productuom_name, c.productgroup_name, c.productgroup_gid, " +
                " a.productuom_gid, d.producttype_gid FROM pmr_mst_tproduct a " +
                " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                " LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid " +
                " LEFT JOIN pmr_mst_tproducttype d ON d.producttype_gid = a.producttype_gid WHERE 1=1");

            if (!string.IsNullOrEmpty(producttype_gid) && producttype_gid != "undefined" && producttype_gid != "null")
            {
                query.Append(" AND a.producttype_gid = '").Append(producttype_gid).Append("'");
            }

            if (!string.IsNullOrEmpty(product_name) && product_name != "null")
            {
                query.Append(" AND a.product_name LIKE '%").Append(product_name).Append("%'");
            }

            return query.ToString();
        }

        private string GetTaxSegmentQuery(string vendor_gid)
        {
            StringBuilder query = new StringBuilder("SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                " e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                " THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                " a.cost_price, a.product_gid, a.product_name FROM acp_mst_ttaxsegment2product d " +
                " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                " LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid " +
                " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE f.vendor_gid = '").Append(vendor_gid).Append("'");

            return query.ToString();
        }

        private List<GetPblProductsearch> ProcessProductData(DataTable productDt)
        {
            var getModuleList = new List<GetPblProductsearch>();
            foreach (DataRow dtRow in productDt.Rows)
            {
                var product = new GetPblProductsearch
                {

                    product_name = dtRow["product_name"].ToString(),
                    product_gid = dtRow["product_gid"].ToString(),
                    product_code = dtRow["product_code"].ToString(),
                    productuom_name = dtRow["productuom_name"].ToString(),
                    productgroup_name = dtRow["productgroup_name"].ToString(),
                    productuom_gid = dtRow["productuom_gid"].ToString(),
                    producttype_gid = dtRow["producttype_gid"].ToString(),
                    productgroup_gid = dtRow["productgroup_gid"].ToString(),
                    unitprice = dtRow["cost_price"].ToString(),
                    quantity = 0,
                    total_amount = 0,
                    discount_persentage = 0,
                    discount_amount = 0,

                };
                getModuleList.Add(product);
            }
            return getModuleList;
        }

        private List<GetPblTaxSegmentList> ProcessTaxSegmentData(DataTable taxDt)
        {
            var allTaxSegmentsList = new List<GetPblTaxSegmentList>();
            if (taxDt != null)
            {
                foreach (DataRow taxRow in taxDt.Rows)
                {
                    var taxSegment = new GetPblTaxSegmentList
                    {
                        product_name = taxRow["product_name"].ToString(),
                        product_gid = taxRow["product_gid"].ToString(),
                        taxsegment_gid = taxRow["taxsegment_gid"].ToString(),
                        taxsegment_name = taxRow["taxsegment_name"].ToString(),
                        tax_name = taxRow["tax_name"].ToString(),
                        tax_percentage = taxRow["tax_percentage"].ToString(),
                        tax_gid = taxRow["tax_gid"].ToString(),
                        mrp_price = taxRow["mrp_price"].ToString(),
                        cost_price = taxRow["cost_price"].ToString(),
                        tax_amount = taxRow["tax_amount"].ToString(),
                    };
                    allTaxSegmentsList.Add(taxSegment);
                }
            }
            return allTaxSegmentsList;
        }


        public void DaGetExtraAddondropDown(MdlPmrTrnDirectInvoice values)
        {
            try
            {
                msSQL = " select additional_gid, additional_name from pmr_trn_tadditional ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetExtraAddondropDown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetExtraAddondropDown
                        {
                            additional_gid = dt["additional_gid"].ToString(),
                            additional_name = dt["additional_name"].ToString(),
                        });
                        values.GetExtraAddondropDown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetExtraDeductiondropDown(MdlPmrTrnDirectInvoice values)
        {
            try
            {
                msSQL = " select discount_gid, discount_name from pmr_trn_tdiscount ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetExtraDeductiondropDown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetExtraDeductiondropDown
                        {
                            discount_gid = dt["discount_gid"].ToString(),
                            discount_name = dt["discount_name"].ToString(),
                        });
                        values.GetExtraDeductiondropDown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void Dadirectinvoicesubmit(string employee_gid,string user_gid ,directsalesinvoicelist values)
        {
            try
            {
                string ls_referenceno = objcmnfunctions.GetMasterGID("PINV");
                msgetGID = objcmnfunctions.GetMasterGID("SIVP"," ",user_gid);
                msGetGID = objcmnfunctions.GetMasterGID("DINV");
                msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                string lstype1 = "services";



                msSQL = " insert into acp_trn_tinvoice(" +
                         " invoice_gid," +
                         " vendor_gid," +
                         " invoice_refno," +
                         " invoice_reference," +
                         " user_gid," +
                         " invoice_date," +
                         " payment_date," +
                         " additionalcharges_amount," +
                         " discount_amount," +
                         " total_amount," +
                         " invoice_amount," +
                         " payment_term," +
                         " created_date," +
                         " invoice_status, " +
                         " invoice_flag, " +
                         " invoice_from, " +
                         " invoice_remarks, " +
                         " additionalcharges_amount_L," +
                         " discount_amount_L," +
                         " total_amount_L," +
                         " currency_code," +
                         " exchange_rate," +
                         " freightcharges," +
                         " extraadditional_amount," +
                         " extradiscount_amount," +
                         " extraadditional_amount_L," +
                         " extradiscount_amount_L," +
                         " buybackorscrap," +
                         " vendorinvoiceref_no," +
                         " branch_gid," +
                         " vendor_contact_person," +
                         " vendor_address," +
                         " invoice_type," +
                         " round_off" +
                         ") values (" +
                         "'" + msgetGID + "'," +
                         "'" + values.direct_invoice_ven_name + "'," +
                         "'" + values.direct_invoice_refno + "'," +
                         "'" + msGetGID + "'," +
                         "'" + employee_gid + "'," +
                         "'" + values.direct_invoice_date.ToString("yyyy-MM-dd ") + "'," +
                         "'" + values.direct_invoice_due_date.ToString("yyyy-MM-dd ") + "'," +
                         "'" + values.direct_invoice_addon_amount + "'," +
                         "'" + values.direct_invoice_discount_amount + "'," +
                         "'" + values.direct_invoice_grand_total + "'," +
                         "'" + values.direct_invoice_amount + "'," +
                         "'" + values.direct_invoice_payterm + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                         "'IV Approved'," +
                         "'Payment Pending'," +
                         "'" + lstype1 + "'," +
                         "'" + values.direct_invoice_remarks + "'," +
                         "'" + values.direct_invoice_addon_amount + "'," +
                         "'" + values.direct_invoice_discount_amount + "'," +
                         "'" + values.direct_invoice_grand_total + "'," +
                         "'" + values.direct_invoice_currencycode + "'," +
                         "'" + values.direct_invoice_exchange_rate + "'," +
                         "'" + values.direct_invoice_freight_charges + "'," +
                         "'" + values.direct_invoice_extra_addon + "'," +
                         "'" + values.direct_invoice_extra_deduction + "'," +
                         "'" + values.direct_invoice_addon_amount + "'," +
                         "'" + values.direct_invoice_extra_deduction + "'," +
                         "'" + values.direct_invoice_buyback_scrap_charges + "'," +
                         "'" + values.direct_invoice_ven_ref_no + "'," +
                         "'" + values.direct_invoice_branchgid + "'," +
                         "'" + values.direct_invoice_ven_contact_person + "'," +
                         "'" + values.direct_invoice_ven_address + "'," +
                         "'" + values.direct_invoice_type + "'," +
                         "'" + values.direct_invoice_round_off + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    string lstax1, lstax2;
                    double lspercentage1 = 0;
                    double lspercentage2 = 0;
                    double tax1_amount = 0;
                    double tax2_amount = 0;
                    double lsinvoice_amount = values.direct_invoice_amount;

                    if (values.direct_invoice_taxname1 == "")
                    {
                        lspercentage1 = 0;
                    }
                    else
                    {
                        msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.direct_invoice_taxname1 + "'";

                        string lspercentage = objdbconn.GetExecuteScalar(msSQL);

                        tax1_amount = Math.Round(lsinvoice_amount * (Convert.ToDouble(lspercentage) / 100), 2);
                    }

                    if (values.direct_invoice_taxname2 == "")
                    {
                        lspercentage2 = 0;
                    }
                    else
                    {
                        msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.direct_invoice_taxname2 + "'";

                        string lspercentage_2 = objdbconn.GetExecuteScalar(msSQL);

                        tax2_amount = Math.Round(lsinvoice_amount * (Convert.ToDouble(lspercentage_2) / 100), 2);
                    }

                    double Invoice_total = Math.Round((lsinvoice_amount + tax1_amount + tax2_amount), 2);

                    if (values.direct_invoice_taxname1 == "")
                    {
                        lstax1 = "0";
                    }
                    else
                    {
                        msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + values.direct_invoice_taxname1 + "'";

                        lstax1 = objdbconn.GetExecuteScalar(msSQL);
                    }

                    if (values.direct_invoice_taxname2 == "")
                    {
                        lstax2 = "0";
                    }
                    else
                    {
                        msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + values.direct_invoice_taxname2 + "'";
                        lstax2 = objdbconn.GetExecuteScalar(msSQL);
                    }

                    msSQL = " insert into acp_trn_tinvoicedtl(" +
                            " invoicedtl_gid," +
                            " invoice_gid," +
                            " product_price," +
                            " product_total," +
                            " tax_name," +
                            " tax_name2," +
                            " tax_percentage," +
                            " tax_percentage2," +
                            " tax_amount," +
                            " tax_amount2," +
                            " tax1_gid, " +
                            " tax2_gid, " +
                            " display_field," +
                            " product_price_L," +
                            " tax_amount1_L," +
                            " tax_amount2_L" +
                            ") values (" +
                            "'" + msDIGetGID + "'," +
                            "'" + msgetGID + "'," +
                            "'" + lsinvoice_amount + "'," +
                            "'" + Invoice_total + "'," +
                            "'" + lstax1 + "'," +
                            "'" + lstax2 + "'," +
                            "'" + lspercentage1 + "'," +
                            "'" + lspercentage2 + "'," +
                            "'" + tax1_amount + "'," +
                            "'" + tax2_amount + "'," +
                            "'" + values.direct_invoice_taxname1 + "'," +
                            "'" + values.direct_invoice_taxname2 + "'," +
                            "'" + values.direct_invoice_description + "'," +
                            "'" + lsinvoice_amount + "'," +
                            "'" + tax1_amount + "'," +
                            "'" + tax2_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Invoice raised Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While raising Invoice";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetPblProductSummary(string user_gid, string vendor_gid, MdlPmrTrnDirectInvoice values, string smryproduct_gid)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;
                msSQL = "SELECT a.tmpinvoicedtl_gid, a.product_gid,a.qty_invoice, FORMAT(a.qty_ordered, 2) AS qty_ordered, a.product_name, a.product_code," +
                    " b.productgroup_gid, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                    "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price," +
                    " a.discount_percentage, FORMAT(a.discount_amount, 2) AS discount_amount, a.tax_percentage," +
                    " FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field,  a.tax_name," +
                    " a.tax_name2, a.tax_name3, a.tax_percentage2, a.tax_percentage3, " +
                    " FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3," +
                    " a.tax1_gid, a.tax2_gid,a.tax3_gid, a.taxsegment_gid ,g.tax_prefix as tax1,concat(g.tax_prefix,', ', a.tax_name) as " +
                    " taxnames,concat(a.tax_percentage,', ', a.tax_percentage2) as taxpercentage,concat(a.tax_amount,', ', a.tax_amount2) as taxamts FROM acp_tmp_tinvoice a" +
                    " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                    " LEFT JOIN acp_mst_ttax g ON g.tax_gid = a.tax2_gid " +
                    "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                    " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid" +
                    " WHERE a.created_by= '" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Pblproductsummary_list>();
                var getGetTaxSegmentList = new List<PblGetTaxSegmentList>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["producttotal_price"].ToString());
                        grandtotal += double.Parse(dt["producttotal_price"].ToString());
                        getModuleList.Add(new Pblproductsummary_list
                        {
                            tmpinvoicedtl_gid = dt["tmpinvoicedtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            qty = dt["qty_ordered"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            //taxamount1 = dt["taxamts"].ToString(),
                            //tax_name1 = dt["taxnames"].ToString(),
                            taxamount1 = dt["tax_amount"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            taxamount2 = dt["tax_amount2"].ToString(),
                            tax_name2 = dt["tax1"].ToString(),
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
                                getGetTaxSegmentList.Add(new PblGetTaxSegmentList
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
                    values.Pblproductsummary_list = getModuleList;
                    values.PblGetTaxSegmentList = getGetTaxSegmentList;
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
        private DataTable GetTaxDetailsForProduct(string product_gid, string vendor_gid)
        {
            // Query tax segment details based on product_gid 
            msSQL = "SELECT f.taxsegment_gid, d.taxsegment_gid, e.taxsegment_name, d.tax_name, d.tax_gid, " +
                "d.tax_percentage AS tax_percentage, " +
                "d.tax_amount, a.mrp_price, a.cost_price, a.product_gid " +
                "FROM acp_mst_ttaxsegment2product d " +
                "LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                "LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid " +
                "LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid " +
                "WHERE a.product_gid = '" + product_gid + "' AND f.vendor_gid = '" + vendor_gid + "'";

            // Execute query to get tax segment details
            return objdbconn.GetDataTable(msSQL);
        }
        public void DaPblGetOnchangeCurrency(string currencyexchange_gid, MdlPmrTrnDirectInvoice values)
        {
            try
            {

                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
" where currencyexchange_gid='" + currencyexchange_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<PblGetOnchangeCurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new PblGetOnchangeCurrency
                        {

                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.PblGetOnchangeCurrency = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing currecy!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPblPostOnAddproduct(string user_gid, PblsubmitProducts values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("INVT");
                msSQL = " SELECT d.productgroup_name,a.productuom_gid, a.product_gid, a.product_name, b.productuom_name,c.producttype_name,a.producttype_gid FROM pmr_mst_tproduct a " +
                 " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                 "  LEFT JOIN pmr_mst_tproducttype c ON c.producttype_gid = a.producttype_gid " +
                 "  LEFT JOIN pmr_mst_tproductgroup d ON d.productgroup_gid = a.productgroup_gid " +
                 " WHERE product_gid = '" + values.product_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read  ();
                    lsproductgid = objOdbcDataReader["product_gid"].ToString();
                    lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();
                    lsproduct_name = objOdbcDataReader["product_name"].ToString();
                    lsproducttype_name = objOdbcDataReader["producttype_name"].ToString();
                    lsproductgroup_name = objOdbcDataReader["productgroup_name"].ToString();
                    objOdbcDataReader.Close ();
                }
                if (lsproducttype_name == "Services" & lsproductgroup_name== "General")
                {
                    msSQL = " insert into acp_tmp_tinvoice  ( " +
                           " tmpinvoicedtl_gid," +
                           " tmpinvoice_gid," +
                           " qty_invoice," +
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
                            " ) values ( " +
                            "'" + msGetGid + "'," +
                            "''," +
                             "'0.00'," +
                            "'" + lsproductgid + "'," +
               "'" + values.product_code + "'," +
               "'" + values.product_remarks + "'," +
               "'" + values.product_remarks + "'," +
               "'" + lsproductuom_name + "'," +
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
                    msSQL = " insert into acp_tmp_tinvoice  ( " +
                           " tmpinvoicedtl_gid," +
                           " tmpinvoice_gid," +
                           " qty_invoice," +
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
                            " ) values ( " +
                            "'" + msGetGid + "'," +
                            "''," +
                             "'0.00'," +
                            "'" + lsproductgid + "'," +
               "'" + values.product_code + "'," +
               "'" + lsproduct_name + "',";
                if (!string.IsNullOrEmpty(values.product_remarks) && values.product_remarks.Contains("'"))
                    {
                        msSQL += "'" + values.product_remarks.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.product_remarks + "', ";
                    }
                    msSQL +="'" + lsproductuom_name + "'," +
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



        public void DaPblGetAllChargesConfig(string employee_gid, MdlPmrMstPurchaseConfig values)
        {
            try
            {
                msSQL = " select id, charges, flag from pmr_mst_tpurchaseconfig ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salesconfigalllist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salesconfigalllist
                        {
                            id = dt["id"].ToString(),
                            charges = dt["charges"].ToString(),
                            flag = dt["flag"].ToString(),
                        });
                        values.salesconfigalllist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPblDeleteProductSummary(string tmpinvoicedtl_gid, Pdlproductlist values)
        {
            try
            {

                msSQL = "  delete from acp_tmp_tinvoice where tmpinvoicedtl_gid='" + tmpinvoicedtl_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Delete Successfully!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting product !";
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while deleting product in PO!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaPblProductSubmit(string user_gid, PblDirectInvoice values)
        {
            try
            {
                if (string.IsNullOrEmpty(values.invoice_ref_no))
                {
                    int lsfreight = 0;
                    int lsinsurance = 0;

                    string invoice_date = values.invoice_date;
                    if (!string.IsNullOrEmpty(invoice_date))
                    {
                        DateTime uiDate = DateTime.ParseExact(invoice_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        invoicedate = uiDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        invoicedate = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    string due_date = values.due_date;
                    if (!string.IsNullOrEmpty(due_date))
                    {
                        DateTime due_dates = DateTime.ParseExact(due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        due_date = due_dates.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        //due_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        DateTime uiDate = DateTime.ParseExact(values.due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        due_date = uiDate.ToString("yyyy-MM-dd");
                    }
                    if (string.IsNullOrEmpty(values.tax_name4))
                    {
                        lstaxpercentage = "0";

                    }
                    else
                    {
                        msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                        lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                    }

                    string ls_referenceno;

                    if (values.invoice_ref_no == "")
                    {
                        ls_referenceno = objcmnfunctions.getSequencecustomizerGID("PINV", "PBL", values.branch_name);
                    }
                    else
                    {

                        ls_referenceno = values.invoice_ref_no;
                    }

                    string vendor_gid = values.vendor_companyname;


                    msSQL = "select vendor_code from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                    string lsvenorcode1 = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select vendor_companyname from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                    string lsvendor_companyname1 = objdbconn.GetExecuteScalar(msSQL);

                    //msSQL = "select account_gid from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                    //string lsvendor_account = objdbconn.GetExecuteScalar(msSQL);
                    //if (!string.IsNullOrEmpty(lsvendor_account)) { }
                    //else
                    //{
                    //    string msAccGetGID = objcmnfunctions.GetMasterGID("FCOA");

                    //    msSQL = " insert into acc_mst_tchartofaccount( " +
                    //           " account_gid," +
                    //           " accountgroup_gid," +
                    //           " accountgroup_name," +
                    //           " account_code," +
                    //           " account_name," +
                    //           " has_child," +
                    //           " ledger_type," +
                    //           " display_type," +
                    //           " Created_Date, " +
                    //           " Created_By, " +
                    //           " gl_code " +
                    //           " ) values (" +
                    //           "'" + msAccGetGID + "'," +
                    //           "'FCOA000021'," +
                    //           "'Sundry Creditors'," +
                    //           "'" + lsvenorcode1 + "'," +
                    //           "'" + lsvendor_companyname1 + "'," +
                    //           "'N'," +
                    //           "'N'," +
                    //           "'Y'," +
                    //           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    //           "'" + user_gid + "'," +
                    //           "'" + vendor_gid + "')";
                    //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //    if (mnResult == 1)
                    //    {
                    //        msSQL = " update acp_mst_tvendor set " +
                    //                " account_gid = '" + msAccGetGID + "'" +
                    //                " where vendor_gid='" + vendor_gid + "'";
                    //        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //    }
                    //}
                    msSQL = "SELECT account_gid from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows)
                    {
                        while (objOdbcDataReader.Read())
                        {
                            string lsaccount_gid = objOdbcDataReader["account_gid"]?.ToString(); // Safely get the value

                            // Check if lsaccount_gid is null or empty
                            if (string.IsNullOrEmpty(lsaccount_gid))
                            {
                                objfincmn.finance_vendor_debitor("Purchase", lsvenorcode1, lsvendor_companyname1, vendor_gid, user_gid);
                                string trace_comment = "Added a vendor on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_vendor");
                            }
                        }
                    }



                    msGetGID = objcmnfunctions.GetMasterGID("SIVP");
                    msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                    msGetGIDPO = objcmnfunctions.GetMasterGID("PPRP");
                    msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP");
                    msPO2GetGID = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", values.branch_name);
                    // txtinvoicefer = objcmnfunctions.GetMasterGID("DINV");
                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "' ";
                    string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select CASE WHEN tax_prefix IS NULL THEN tax_name ELSE tax_prefix  END AS tax_name FROM    acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                    lstax_name4 = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " insert into acp_trn_tinvoice (" +
                   " invoice_gid, " +
                   " vendor_gid," +
                   " vendor_address, " +
                   " shipping_address, " +
                   " invoice_refno, " +
                   " user_gid, " +
                   " invoice_date," +
                   " payment_date," +
                   " payment_days," +
                   " termsandconditions," +
                   " systemgenerated_amount, " +
                   " additionalcharges_amount, " +
                   " discount_amount, " +
                   " total_amount, " +
                   " invoice_amount, " +
                   " payment_term," +
                   " delivery_term," +
                   " freight_terms," +
                   " mode_despatch, " +
                   " billing_email, " +
                   " created_date," +
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
                   " buybackorscrap," +
                   " vendorinvoiceref_no," +
                   " branch_gid," +
                   " tax_gid," +
                    " tax_name," +
                   " tax_percentage," +
                   " tax_amount," +
                   " purchase_type," +
                   " purchaseorder_gid," +
                   " round_off" +
                   " ) values (" +
                   "'" + msGetGID + "'," +
                   "'" + values.vendor_companyname + "'," +
                     "'" + values.vendor_details + "',";
                    if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                    {
                        msSQL += "'" + values.shipping_address.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.shipping_address + "', ";
                    }
                    msSQL += "'" + ls_referenceno + "'," +
                   "'" + user_gid + "'," +
                   "'" + invoicedate + "'," +
                   "'" + due_date + "'," +
                   "'" + values.payment_days + "',";
                    if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                    {
                        msSQL += "'" + values.template_content.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.template_content + "', ";
                    }
                    msSQL += "'0.00',";
                    if (values.addoncharge == "" || values.addoncharge == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.addoncharge + "',";

                    }
                    if (values.additional_discount == "" || values.additional_discount == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount + "',";

                    }
                    if (values.totalamount == "" || values.totalamount == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.totalamount + "',";

                    }
                    if (values.grandtotal == "" || values.grandtotal == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.grandtotal + "',";

                    }

                    msSQL += "'" + values.payment_terms + "'," +
                    "'" + values.delivery_terms + "'," +
                    "'" + values.freight_terms + "',";
                    if (!string.IsNullOrEmpty(values.dispatch_mode) && values.dispatch_mode.Contains("'"))
                    {
                        msSQL += "'" + values.dispatch_mode.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.dispatch_mode + "', ";
                    }
                    
                    msSQL += "'" + values.billing_mail + "'," +
                   "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                   "'" + "Invoice Approved" + "'," +
                   "'" + "Payment Pending" + "',";
                    if (!string.IsNullOrEmpty(values.invoice_remarks) && values.invoice_remarks.Contains("'"))
                    {
                        msSQL += "'" + values.invoice_remarks.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.invoice_remarks + "', ";
                    }
                    msSQL += "'" + "Direct Invoice" + "'," +
                   "'0.00'," +
                   "'0.00',";
                    if (values.grandtotal == "" && values.grandtotal == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.grandtotal + "',";

                    }
                    msSQL += "'" + lscurrency_code + "',";
                    if (values.exchange_rate == "" && values.exchange_rate == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.exchange_rate + "',";

                    }

                    if (values.freightcharges == "" && values.freightcharges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.freightcharges + "',";

                    }

                    msSQL += "'0.00',";
                    msSQL += "'0.00',";
                    msSQL += "'0.00',";
                    msSQL += "'0.00',";


                    if (values.buybackorscrap == "" || values.buybackorscrap == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.buybackorscrap + "',";

                    }
                    msSQL +=
                    "'" + values.vendor_ref_no + "'," +
                    "'" + values.branch_name + "',";
                    msSQL += "'" + values.tax_name4 + "'," +
                    "'" + lstax_name4 + "'," +
                     "'" + lstaxpercentage + "',";

                    if (string.IsNullOrEmpty(values.tax_amount4))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_amount4 + "',";
                    }
                    msSQL += "'" + values.purchase_type + "'," +
                        "'" + msPO1GetGID + "'," +
                             "'" + values.roundoff + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                  

                    msSQL = " insert into pmr_trn_tpurchaseorder (" +
                            " purchaseorder_gid, " +
                            " purchaserequisition_gid, " +
                            " purchaseorder_reference, " +
                            " purchaseorder_date," +
                            " branch_gid, " +
                            " created_by, " +
                            " created_date," +
                            " vendor_gid, " +
                            " vendor_address, " +
                            " shipping_address, " +
                            " requested_by, " +
                            " freight_terms, " +
                            " payment_terms, " +
                            " requested_details, " +
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
                            " taxsegment_gid " +
                            " ) values (" +
                            "'" + msPO1GetGID + "'," +
                            "'" + msGetGIDPO + "'," +
                            "'" + msPO2GetGID + "'," +
                            "'" + invoicedate + "', " +                          
                            "'" + values.branch_name + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'" + values.vendor_companyname + "',";

                    //"'" + values.address1.Trim().Replace("'", "") + "',";

                    if (!string.IsNullOrEmpty(values.vendor_details) && values.vendor_details.Contains("'"))
                    {
                        msSQL += "'" + values.vendor_details.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.vendor_details + "', ";
                    }

                    //"'" + values.shipping_address.Trim().Replace("'", "") + "'," +

                    if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                    {
                        msSQL += "'" + values.shipping_address.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.shipping_address + "', ";
                    }
                    msSQL += "'" + user_gid + "'," +
                          "'" + values.delivery_terms + "'," +
                          "'" + values.payment_terms + "'," +
                          "'" + values.billing_mail + "',";
                          if (!string.IsNullOrEmpty(values.dispatch_mode) && values.dispatch_mode.Contains("'"))
                    {
                        msSQL += "'" + values.dispatch_mode.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.dispatch_mode + "', ";
                    }
                    
                        msSQL +=  "'" + lscurrency_code + "'," +
                          "'" + values.exchange_rate + "'," +
                          "'" +  "Direct Invoice"  + "'," +
                          "'" + "Direct Invoice" + "'," +
                          "'" + values.grandtotal + "',";
                        if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                    {
                        msSQL += "'" + values.template_content.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.template_content + "', ";
                    }
                   
                        msSQL += "'PO Completed',";
                  
                    
                    msSQL += "'" + "PO Completed" + "'," +
                         "'" + msPO2GetGID + "'," +
                          "'" + values.totalamount + "'," +
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
                    msSQL += "'" + values.taxsegment_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);




                    if (mnResult == 1)
                    {
                        msSQL = "SELECT a.tmpinvoicedtl_gid, a.product_gid,a.qty_invoice, FORMAT(a.qty_ordered, 2) AS qty_ordered," +
                            " a.product_name, a.product_code," +
                         " b.productgroup_gid, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                         "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price," +
                         " a.discount_percentage, a.discount_amount, a.tax_percentage," +
                         " FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field,  a.tax_name," +
                         " a.tax_name2, a.tax_name3, a.tax_percentage2, a.tax_percentage3, " +
                         "FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3," +
                         " a.tax1_gid, a.tax2_gid,a.tax3_gid, a.taxsegment_gid FROM acp_tmp_tinvoice a" +
                         " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                         "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                         " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid" +
                         " WHERE a.created_by= '" + user_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                msSQL = "select tax_prefix from acp_mst_ttax where tax_gid ='" + dt["tax2_gid"].ToString() + "' ";
                                string lstax2 = objdbconn.GetExecuteScalar(msSQL);

                                msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");


                                msSQL = " insert into acp_trn_tinvoicedtl (" +
                                " invoicedtl_gid, " +
                                " invoice_gid, " +
                                " product_gid, " +
                             " product_code, " +
                                 " product_name, " +
                                " productuom_name, " +
                               " uom_gid, " +
                                " productgroup_name, " +
                               " producttype_gid, " +
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
                               " display_field," +
                               " product_price_L," +
                               " tax_amount1_L," +
                               " tax_amount2_L," +
                               " tax_amount3_L," +
                               " qty_invoice, " +
                               " taxsegment_gid" +
                                 " )values ( " +
                                 "'" + msDIGetGID + "', " +
                                 "'" + msGetGID + "'," +
                                 "'" + dt["product_gid"].ToString() + "', " +
                                 "'" + dt["product_code"].ToString() + "', " +
                                 "'" + dt["product_name"].ToString() + "', " +
                                 "'" + dt["productuom_name"].ToString() + "', " +
                                 "'" + dt["uom_gid"].ToString() + "', " +
                                  "'" + dt["productgroup_name"].ToString() + "', " +
                                 "'" + dt["producttype_gid"].ToString() + "', " +
                                 "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["discount_percentage"].ToString() + "', " +
                                 "'" + dt["discount_amount"].ToString() + "', " +
                                 "'" + dt["tax_name"].ToString() + "', " +
                                 "'" + lstax2 + "', " +
                                 "'" + dt["tax_name3"].ToString() + "', " +
                                 "'" + dt["tax_percentage"].ToString() + "', " +
                                 "'" + dt["tax_percentage2"].ToString() + "', " +
                                 "'" + dt["tax_percentage3"].ToString() + "', " +
                                 "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["tax1_gid"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["tax2_gid"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["tax3_gid"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["display_field"].ToString().Replace("'", "\\\'") + "', " +
                                    "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                 "'" + dt["taxsegment_gid"].ToString() + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                msPOGetGID3 = objcmnfunctions.GetMasterGID("PODC");
                                msSQL = " insert into pmr_trn_tpurchaseorderdtl (" +
                                            " purchaseorderdtl_gid, " +
                                            " purchaseorder_gid, " +
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
                                       " taxsegment_gid" +
                                            " )values ( " +
                                            "'" + msPOGetGID3 + "', " +
                                            "'" + msPO1GetGID + "'," +
                                            "'" + dt["product_gid"].ToString() + "', " +
                                            "'" + dt["product_code"].ToString() + "', " +
                                            "'" + dt["product_name"].ToString() + "', " +
                                            "'" + dt["productuom_name"].ToString() + "', " +
                                            "'" + dt["uom_gid"].ToString() + "', " +
                                            "'" + dt["producttype_gid"].ToString() + "', " +
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
                                "'" + lstax2 + "', " +
                                "'" + dt["tax_name3"].ToString() + "', " +
                                "'" + dt["tax1_gid"].ToString() + "', " +
                                "'" + dt["tax2_gid"].ToString() + "', " +
                                "'" + dt["tax3_gid"].ToString() + "', " +
                                "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                "'" + dt["display_field"].ToString().Replace("'", "\\\'") + "'," +
                                "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                                  "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_percentage"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_percentage2"].ToString().Replace(",", "") + "'," +
                                "'" + dt["tax_percentage3"].ToString().Replace(",", "") + "'," +
                                "'" + dt["taxsegment_gid"].ToString() + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                            }

                        }
                        if (mnResult == 1)
                        {
                            msSQL = "select finance_flag from adm_mst_tcompany ";
                            string finance_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (finance_flag == "Y")
                            {
                                double product;
                               double discount;
                                 msSQL = "SELECT SUM(COALESCE(qty_invoice, 2) * COALESCE(product_price, 2)) AS product, ROUND(SUM(discount_amount), 2) AS discount FROM acp_trn_tinvoicedtl WHERE invoice_gid = '" + msGetGID + "'";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                if (objOdbcDataReader.HasRows)
                                {
                                    objOdbcDataReader.Read();  
                                    product = objOdbcDataReader["product"] != DBNull.Value ? double.Parse(objOdbcDataReader["product"].ToString()) : 0.00;
                                    discount = objOdbcDataReader["discount"] != DBNull.Value ? double.Parse(objOdbcDataReader["discount"].ToString()) : 0.00;
                                }
                                else
                                {
                                    product = 0.00;
                                    discount = 0.00;
                                }

                                objOdbcDataReader.Close();  

                            
                                double lsbasic_amount = product - discount;

                                double addonCharges = double.TryParse(values.addoncharge, out double addonChargesValue) ? addonChargesValue : 0;
                                double freightCharges = double.TryParse(values.freightcharges, out double freightChargesValue) ? freightChargesValue : 0;
                                double forwardingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                                double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                                double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                                double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                                double buybackCharges = double.TryParse(values.buybackorscrap, out double buybackChargesValue) ? buybackChargesValue : 0;
                                double overalltax_amount = double.TryParse(values.tax_amount4, out double overalltaxamount) ? overalltaxamount : 0;
                                double grandtotal = double.TryParse(values.grandtotal, out double grand_total) ? grand_total : 0;
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


                                objfincmn.jn_purchase_invoice(invoicedate, values.remarks, values.branch_name, ls_referenceno, msGetGID
                                 , fin_basic_amount, fin_addonCharges, fin_additionaldiscountAmount, fin_grandtotal, vendor_gid, "Invoice", "PMR",
                                 values.purchase_type, fin_roundoff, fin_freightcharges, fin_buybackCharges, lstax_name4, fin_overalltax_amount, fin_forwardingCharges, fin_insuranceCharges);



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
                                                "WHERE invoice_gid = '" + msGetGID + "' AND tax1_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                        objODBCDataReader1 = objdbconn.GetDataReader(msSQL);

                                        if (objODBCDataReader1.HasRows && objODBCDataReader1.Read())
                                        {
                                            lstax1 = objODBCDataReader1["tax1"] != DBNull.Value ? objODBCDataReader1["tax1"].ToString() : "0.00";
                                        }
                                        objODBCDataReader1.Close();

                                        // Tax 2 Calculation
                                        msSQL = "SELECT SUM(tax_amount2) AS tax2 FROM acp_trn_tinvoicedtl " +
                                                "WHERE invoice_gid = '" + msGetGID + "' AND tax2_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                        objODBCDataReader2 = objdbconn.GetDataReader(msSQL);

                                        if (objODBCDataReader2.HasRows && objODBCDataReader2.Read())
                                        {
                                            lstax2 = objODBCDataReader2["tax2"] != DBNull.Value ? objODBCDataReader2["tax2"].ToString() : "0.00";
                                        }
                                        objODBCDataReader2.Close();

                                        // Tax 3 Calculation
                                        msSQL = "SELECT SUM(tax_amount3_L) AS tax3 FROM acp_trn_tinvoicedtl " +
                                                "WHERE invoice_gid = '" + msGetGID + "' AND tax3_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                        objODBCDataReader3 = objdbconn.GetDataReader(msSQL);

                                        if (objODBCDataReader3.HasRows && objODBCDataReader3.Read())
                                        {
                                            lstax3 = objODBCDataReader3["tax3"] != DBNull.Value ? objODBCDataReader3["tax3"].ToString() : "0.00";
                                        }
                                        objODBCDataReader3.Close();

                                        if (lstax1 != "0.00" || lstax2 != "0.00" || lstax3 != "0.00")
                                        {
                                            lstax_gid = objODBCDataReader["tax_gid"].ToString();
                                            lstaxsum = (Convert.ToDecimal(lstax1.Replace(",", "")) +
                                                        Convert.ToDecimal(lstax2.Replace(",", "")) +
                                                        Convert.ToDecimal(lstax3.Replace(",", ""))).ToString();

                                            lstaxamount = (Convert.ToDecimal(lstaxsum) * Convert.ToDecimal(values.exchange_rate)).ToString();

                                            objfincmn.jn_purchase_tax(msGetGID, ls_referenceno, values.remarks, lstaxamount, lstax_gid);
                                        }
                                    }
                                }

                                objdbconn.CloseConn();
                            }


                            msSQL = "  delete from acp_tmp_tinvoice where created_by='" + user_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            values.status = true;
                            values.message = "Invoice Submitted Successfully";
                        }


                        //if (mnResult != 0)
                        //{
                        //    values.status = true;
                        //    values.message = "Direct Invoice Raised Successfully!";
                        //}
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding product in Direct Invoice!";
                        }

                        msSQL = "  delete from acp_tmp_tinvoice where created_by='" + user_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        dt_datatable.Dispose();
                    }
                }



                else
                {
                    msSQL = "select invoice_gid from acp_trn_tinvoice where invoice_refno ='" + values.invoice_ref_no + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        values.message = "Invoice Ref No Alredy Exist!";
                        return;
                    }
                    else
                    {

                        int lsfreight = 0;
                        int lsinsurance = 0;

                        string invoice_date = values.invoice_date;
                        if (!string.IsNullOrEmpty(invoice_date))
                        {
                            DateTime uiDate = DateTime.ParseExact(invoice_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            invoicedate = uiDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            invoicedate = DateTime.Now.ToString("yyyy-MM-dd");
                        }

                        string due_date = values.due_date;
                        if (!string.IsNullOrEmpty(due_date))
                        {
                            DateTime due_dates = DateTime.ParseExact(due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            due_date = due_dates.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            //due_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            DateTime uiDate = DateTime.ParseExact(values.due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            due_date = uiDate.ToString("yyyy-MM-dd");
                        }
                        if (string.IsNullOrEmpty(values.tax_name4))
                        {
                            lstaxpercentage = "0";

                        }
                        else
                        {
                            msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                            lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                        }


                        msGetGID = objcmnfunctions.GetMasterGID("SIVP");
                        msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                        msGetGIDPO = objcmnfunctions.GetMasterGID("PPRP");
                        msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP");
                        // txtinvoicefer = objcmnfunctions.GetMasterGID("DINV");
                        msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "' ";
                        string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select CASE WHEN tax_prefix IS NULL THEN tax_name ELSE tax_prefix  END AS tax_name FROM    acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                        lstax_name4 = objdbconn.GetExecuteScalar(msSQL);
                        //string ls_referenceno = "";
                        string ls_referenceno;

                        if ( values.invoice_ref_no == "") {
                            ls_referenceno = objcmnfunctions.getSequencecustomizerGID("PINV", "PBL", values.branch_name);
                        } else {

                            ls_referenceno = values.invoice_ref_no;
                        }

                        string vendor_gid = values.vendor_companyname;
                        msSQL = "select vendor_code from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                        string lsvenorcode1 = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select vendor_companyname from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                        string lsvendor_companyname1 = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "SELECT account_gid from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader.HasRows)
                        {
                            while (objOdbcDataReader.Read())
                            {
                                string lsaccount_gid = objOdbcDataReader["account_gid"]?.ToString(); // Safely get the value

                                // Check if lsaccount_gid is null or empty
                                if (string.IsNullOrEmpty(lsaccount_gid))
                                {
                                    objfincmn.finance_vendor_debitor("Purchase", lsvenorcode1, lsvendor_companyname1, vendor_gid, user_gid);
                                    string trace_comment = "Added a vendor on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_vendor");
                                }
                            }
                        }


                        //string  msAccGetGID = objcmnfunctions.GetMasterGID("FCOA");

                        //msSQL = " insert into acc_mst_tchartofaccount( " +
                        //       " account_gid," +
                        //       " accountgroup_gid," +
                        //       " accountgroup_name," +
                        //       " account_code," +
                        //       " account_name," +
                        //       " has_child," +
                        //       " ledger_type," +
                        //       " display_type," +
                        //       " Created_Date, " +
                        //       " Created_By, " +
                        //       " gl_code " +
                        //       " ) values (" +
                        //       "'" + msAccGetGID + "'," +
                        //       "'FCOA000022'," +
                        //       "'Sundry Debtors'," +
                        //       "'" + lsvenorcode1 + "'," +
                        //       "'" + lsvendor_companyname1 + "'," +
                        //       "'N'," +
                        //       "'N'," +
                        //       "'Y'," +
                        //       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        //       "'" + user_gid + "'," +
                        //       "'" + vendor_gid + "')";
                        //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        //if (mnResult == 1)
                        //{
                        //    msSQL = " update acp_mst_tvendor set " +
                        //            " account_gid = '" + msAccGetGID + "'" +
                        //            " where vendor_gid='" + vendor_gid + "'";
                        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        //}


                        msSQL = " insert into acp_trn_tinvoice (" +
                       " invoice_gid, " +
                       " vendor_gid," +
                       " vendor_address, " +
                       " shipping_address, " +
                       " invoice_refno, " +
                       " user_gid, " +
                       " invoice_date," +
                       " payment_date," +
                       " payment_days," +
                       " termsandconditions," +
                       " systemgenerated_amount, " +
                       " additionalcharges_amount, " +
                       " discount_amount, " +
                       " total_amount, " +
                       " invoice_amount, " +
                       " payment_term," +
                       " delivery_term," +
                       " freight_terms," +
                       " mode_despatch, " +
                       " billing_email, " +
                       " created_date," +
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
                       " buybackorscrap," +
                       " vendorinvoiceref_no," +
                       " branch_gid," +
                       " tax_gid," +
                        " tax_name," +
                       " tax_percentage," +
                       " tax_amount," +
                       " purchase_type," +
                       " purchaseorder_gid," +
                       " round_off" +
                       " ) values (" +
                       "'" + msGetGID + "'," +
                       "'" + values.vendor_companyname + "'," +
                         "'" + values.vendor_details + "',";
                        if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                        {
                            msSQL += "'" + values.shipping_address.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.shipping_address + "', ";
                        }
                        msSQL += "'" + ls_referenceno + "'," +
                        "'" + user_gid + "'," +
                       "'" + invoicedate + "'," +
                       "'" + due_date + "'," +
                       "'" + values.payment_days + "',";
                        if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                        {
                            msSQL += "'" + values.template_content.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.template_content + "', ";
                        }
                        msSQL += "'0.00',";
                        if (values.addoncharge == "" || values.addoncharge == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.addoncharge + "',";

                        }
                        if (values.additional_discount == "" || values.additional_discount == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.additional_discount + "',";

                        }
                        if (values.totalamount == "" || values.totalamount == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.totalamount + "',";

                        }
                        if (values.grandtotal == "" || values.grandtotal == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.grandtotal + "',";

                        }

                        msSQL += "'" + values.payment_terms + "'," +
                        "'" + values.delivery_terms + "'," +
                        "'" + values.freight_terms + "'," +
                        "'" + values.dispatch_mode + "'," +
                        "'" + values.billing_mail + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                       "'" + "Invoice Approved" + "'," +
                       "'" + "Payment Pending" + "',";
                        if (!string.IsNullOrEmpty(values.invoice_remarks) && values.invoice_remarks.Contains("'"))
                        {
                            msSQL += "'" + values.invoice_remarks.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.invoice_remarks + "', ";
                        }
                        msSQL += "'" + "Direct Invoice" + "'," +
                       "'0.00'," +
                       "'0.00',";
                        if (values.grandtotal == "" && values.grandtotal == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.grandtotal + "',";

                        }
                        msSQL += "'" + lscurrency_code + "',";
                        if (values.exchange_rate == "" && values.exchange_rate == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.exchange_rate + "',";

                        }

                        if (values.freightcharges == "" && values.freightcharges == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.freightcharges + "',";

                        }

                        msSQL += "'0.00',";
                        msSQL += "'0.00',";
                        msSQL += "'0.00',";
                        msSQL += "'0.00',";


                        if (values.buybackorscrap == "" || values.buybackorscrap == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.buybackorscrap + "',";

                        }
                        msSQL +=
                        "'" + values.vendor_ref_no + "'," +
                        "'" + values.branch_name + "',";
                        msSQL += "'" + values.tax_name4 + "'," +
                        "'" + lstax_name4 + "'," +
                         "'" + lstaxpercentage + "',";

                        if (string.IsNullOrEmpty(values.tax_amount4))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.tax_amount4 + "',";
                        }
                        msSQL += "'" + values.purchase_type + "'," +
                                 "'" + msPO1GetGID + "'," +
                                 "'" + values.roundoff + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        

                        msSQL = " insert into pmr_trn_tpurchaseorder (" +
                                " purchaseorder_gid, " +
                                " purchaserequisition_gid, " +
                                " purchaseorder_reference, " +
                                " purchaseorder_date," +
                                " branch_gid, " +
                                " created_by, " +
                                " created_date," +
                                " vendor_gid, " +
                                " vendor_address, " +
                                " shipping_address, " +
                                " requested_by, " +
                                " freight_terms, " +
                                " payment_terms, " +
                                " requested_details, " +
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
                                " taxsegment_gid " +
                                " ) values (" +
                                "'" + msPO1GetGID + "'," +
                                "'" + msGetGIDPO + "'," +
                                "'" + msPO1GetGID + "'," +
                                "'" + invoicedate + "', " +
                                "'" + values.branch_name + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'" + values.vendor_companyname + "',";

                        //"'" + values.address1.Trim().Replace("'", "") + "',";

                        if (!string.IsNullOrEmpty(values.vendor_details) && values.vendor_details.Contains("'"))
                        {
                            msSQL += "'" + values.vendor_details.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.vendor_details + "', ";
                        }

                        //"'" + values.shipping_address.Trim().Replace("'", "") + "'," +

                        if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                        {
                            msSQL += "'" + values.shipping_address.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.shipping_address + "', ";
                        }
                        msSQL += "'" + user_gid + "'," +
                              "'" + values.delivery_terms + "'," +
                              "'" + values.payment_terms + "'," +
                              "'" + values.billing_mail + "'," +
                              "'" + values.dispatch_mode + "'," +
                              "'" + lscurrency_code + "'," +
                              "'" + values.exchange_rate + "'," +
                              "'" + "Direct Invoice" + "'," +
                              "'" + "Direct Invoice" + "'," +
                              "'" + values.grandtotal + "',";
                        //"'" + values.template_content.Trim().Replace("'", "") + "',";
                        if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                        {
                            msSQL += "'" + values.template_content.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.template_content + "', ";
                        }

                        msSQL += "'PO Completed',";


                        msSQL += "'" + "PO Completed" + "'," +
                             "'" + values.po_no + "'," +
                              "'" + values.totalamount + "'," +
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
                        msSQL += "'" + values.taxsegment_gid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            msSQL = "SELECT a.tmpinvoicedtl_gid, a.product_gid,a.qty_invoice, FORMAT(a.qty_ordered, 2) AS qty_ordered," +
                                " a.product_name, a.product_code," +
                             " b.productgroup_gid, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                             "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price," +
                             " a.discount_percentage, a.discount_amount, a.tax_percentage," +
                             " FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field,  a.tax_name," +
                             " a.tax_name2, a.tax_name3, a.tax_percentage2, a.tax_percentage3, " +
                             "FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3," +
                             " a.tax1_gid, a.tax2_gid,a.tax3_gid, a.taxsegment_gid FROM acp_tmp_tinvoice a" +
                             " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                             "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                             " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid" +
                             " WHERE a.created_by= '" + user_gid + "'";
                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    msSQL = "select tax_prefix from acp_mst_ttax where tax_gid ='" + dt["tax2_gid"].ToString() + "' ";
                                    string lstax2 = objdbconn.GetExecuteScalar(msSQL);

                                    msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");


                                    msSQL = " insert into acp_trn_tinvoicedtl (" +
                                    " invoicedtl_gid, " +
                                    " invoice_gid, " +
                                    " product_gid, " +
                                 " product_code, " +
                                     " product_name, " +
                                    " productuom_name, " +
                                   " uom_gid, " +
                                    " productgroup_name, " +
                                   " producttype_gid, " +
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
                                   " display_field," +
                                   " product_price_L," +
                                   " tax_amount1_L," +
                                   " tax_amount2_L," +
                                   " tax_amount3_L," +
                                   " qty_invoice, " +
                                   " taxsegment_gid" +
                                     " )values ( " +
                                     "'" + msDIGetGID + "', " +
                                     "'" + msGetGID + "'," +
                                     "'" + dt["product_gid"].ToString() + "', " +
                                     "'" + dt["product_code"].ToString() + "', " +
                                     "'" + dt["product_name"].ToString() + "', " +
                                     "'" + dt["productuom_name"].ToString() + "', " +
                                     "'" + dt["uom_gid"].ToString() + "', " +
                                      "'" + dt["productgroup_name"].ToString() + "', " +
                                     "'" + dt["producttype_gid"].ToString() + "', " +
                                     "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["discount_percentage"].ToString() + "', " +
                                     "'" + dt["discount_amount"].ToString() + "', " +
                                     "'" + dt["tax_name"].ToString() + "', " +
                                     "'" + lstax2 + "', " +
                                     "'" + dt["tax_name3"].ToString() + "', " +
                                     "'" + dt["tax_percentage"].ToString() + "', " +
                                     "'" + dt["tax_percentage2"].ToString() + "', " +
                                     "'" + dt["tax_percentage3"].ToString() + "', " +
                                     "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax1_gid"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax2_gid"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax3_gid"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["display_field"].ToString().Replace("'", "\\\'") + "', " +
                                        "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                    "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["taxsegment_gid"].ToString() + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    msPOGetGID3 = objcmnfunctions.GetMasterGID("PODC");
                                    msSQL = " insert into pmr_trn_tpurchaseorderdtl (" +
                                                " purchaseorderdtl_gid, " +
                                                " purchaseorder_gid, " +
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
                                           " taxsegment_gid" +
                                                " )values ( " +
                                                "'" + msPOGetGID3 + "', " +
                                                "'" + msPO1GetGID + "'," +
                                                "'" + dt["product_gid"].ToString() + "', " +
                                                "'" + dt["product_code"].ToString() + "', " +
                                                "'" + dt["product_name"].ToString() + "', " +
                                                "'" + dt["productuom_name"].ToString() + "', " +
                                                "'" + dt["uom_gid"].ToString() + "', " +
                                                "'" + dt["producttype_gid"].ToString() + "', " +
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
                                    "'" + lstax2 + "', " +
                                    "'" + dt["tax_name3"].ToString() + "', " +
                                    "'" + dt["tax1_gid"].ToString() + "', " +
                                    "'" + dt["tax2_gid"].ToString() + "', " +
                                    "'" + dt["tax3_gid"].ToString() + "', " +
                                    "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                    "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                    "'" + dt["display_field"].ToString().Replace("'", "\\\'") + "'," +
                                    "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                    "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                                    "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                                      "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                                    "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                                    "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                                    "'" + dt["tax_percentage"].ToString().Replace(",", "") + "'," +
                                    "'" + dt["tax_percentage2"].ToString().Replace(",", "") + "'," +
                                    "'" + dt["tax_percentage3"].ToString().Replace(",", "") + "'," +
                                    "'" + dt["taxsegment_gid"].ToString() + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                }

                            }
                            if (mnResult == 1)
                            {
                               

                                msSQL = "select finance_flag from adm_mst_tcompany ";
                                string finance_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (finance_flag == "Y")
                                {
                                    double product;
                                    double discount;
                                    msSQL = "SELECT SUM(COALESCE(qty_invoice, 2) * COALESCE(product_price, 2)) AS product, ROUND(SUM(discount_amount), 2) AS discount FROM acp_trn_tinvoicedtl WHERE invoice_gid = '" + msGetGID + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                    if (objOdbcDataReader.HasRows)
                                    {
                                        objOdbcDataReader.Read();
                                        product = objOdbcDataReader["product"] != DBNull.Value ? double.Parse(objOdbcDataReader["product"].ToString()) : 0.00;
                                        discount = objOdbcDataReader["discount"] != DBNull.Value ? double.Parse(objOdbcDataReader["discount"].ToString()) : 0.00;
                                    }
                                    else
                                    {
                                        product = 0.00;
                                        discount = 0.00;
                                    }

                                    objOdbcDataReader.Close();


                                    double lsbasic_amount = product - discount;

                                    

                                    double addonCharges = double.TryParse(values.addoncharge, out double addonChargesValue) ? addonChargesValue : 0;
                                    double freightCharges = double.TryParse(values.freightcharges, out double freightChargesValue) ? freightChargesValue : 0;
                                    double forwardingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                                    double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                                    double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                                    double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                                    double buybackCharges = double.TryParse(values.buybackorscrap, out double buybackChargesValue) ? buybackChargesValue : 0;
                                    double overalltax_amount = double.TryParse(values.tax_amount4, out double overalltaxamount) ? overalltaxamount : 0;
                                    double grandtotal = double.TryParse(values.grandtotal, out double grand_total) ? grand_total : 0;
                                    double ExchangeRate = double.TryParse(values.exchange_rate, out double exchange) ? exchange : 0;

                                    double fin_basic_amount = lsbasic_amount * ExchangeRate;
                                    double fin_addonCharges = addonCharges * ExchangeRate;
                                    double fin_freightcharges = freightCharges * ExchangeRate;
                                    double fin_forwardingCharges  = forwardingCharges * ExchangeRate;
                                    double fin_insuranceCharges = insuranceCharges * ExchangeRate;
                                    double fin_roundoff  = roundoff * ExchangeRate;
                                    double fin_buybackCharges = buybackCharges * ExchangeRate;
                                    double fin_overalltax_amount = overalltax_amount * ExchangeRate;
                                    double fin_additionaldiscountAmount  = additionaldiscountAmount * ExchangeRate;
                                    double fin_grandtotal = grandtotal * ExchangeRate;


                                    objfincmn.jn_purchase_invoice(invoicedate, values.remarks, values.branch_name, ls_referenceno, msGetGID
                                     , fin_basic_amount, fin_addonCharges, fin_additionaldiscountAmount, fin_grandtotal, vendor_gid, "Invoice", "PMR",
                                     values.purchase_type, fin_roundoff, fin_freightcharges, fin_buybackCharges, lstax_name4, fin_overalltax_amount, fin_forwardingCharges, fin_insuranceCharges);


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
                                            msSQL = "SELECT SUM(tax_amount1_L) AS tax1 FROM acp_trn_tinvoicedtl " +
                                                    "WHERE invoice_gid = '" + msGetGID + "' AND tax1_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                            objODBCDataReader1 = objdbconn.GetDataReader(msSQL);

                                            if (objODBCDataReader1.HasRows && objODBCDataReader1.Read())
                                            {
                                                lstax1 = objODBCDataReader1["tax1"] != DBNull.Value ? objODBCDataReader1["tax1"].ToString() : "0.00";
                                            }
                                            objODBCDataReader1.Close();

                                            // Tax 2 Calculation
                                            msSQL = "SELECT SUM(tax_amount2_L) AS tax2 FROM acp_trn_tinvoicedtl " +
                                                    "WHERE invoice_gid = '" + msGetGID + "' AND tax2_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                            objODBCDataReader2 = objdbconn.GetDataReader(msSQL);

                                            if (objODBCDataReader2.HasRows && objODBCDataReader2.Read())
                                            {
                                                lstax2 = objODBCDataReader2["tax2"] != DBNull.Value ? objODBCDataReader2["tax2"].ToString() : "0.00";
                                            }
                                            objODBCDataReader2.Close();

                                            // Tax 3 Calculation
                                            msSQL = "SELECT SUM(tax_amount3_L) AS tax3 FROM acp_trn_tinvoicedtl " +
                                                    "WHERE invoice_gid = '" + msGetGID + "' AND tax3_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                            objODBCDataReader3 = objdbconn.GetDataReader(msSQL);

                                            if (objODBCDataReader3.HasRows && objODBCDataReader3.Read())
                                            {
                                                lstax3 = objODBCDataReader3["tax3"] != DBNull.Value ? objODBCDataReader3["tax3"].ToString() : "0.00";
                                            }
                                            objODBCDataReader3.Close();

                                            if (lstax1 != "0.00" || lstax2 != "0.00" || lstax3 != "0.00")
                                            {
                                                lstax_gid = objODBCDataReader["tax_gid"].ToString();
                                                lstaxsum = (Convert.ToDecimal(lstax1.Replace(",", "")) +
                                                            Convert.ToDecimal(lstax2.Replace(",", "")) +
                                                            Convert.ToDecimal(lstax3.Replace(",", ""))).ToString();

                                                lstaxamount = (Convert.ToDecimal(lstaxsum) * Convert.ToDecimal(values.exchange_rate)).ToString();

                                                objfincmn.jn_purchase_tax(msGetGID, ls_referenceno, values.remarks, lstaxamount, lstax_gid);
                                            }
                                        }
                                    }

                                    objdbconn.CloseConn();
                                }


                                msSQL = "  delete from acp_tmp_tinvoice where created_by='" + user_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                values.status = true;
                                values.message = "Invoice Submitted Successfully";
                            }

                            //if (mnResult != 0)
                            //{
                            //    values.status = true;
                            //    values.message = "Direct Invoice Raised Successfully!";
                            //}
                            else
                            {
                                values.status = false;
                                values.message = "Error While Adding product in Direct Invoice!";
                            }

                            
                            dt_datatable.Dispose();
                        }
                    }




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


   




        public void DaPblGetTax(MdlPmrTrnDirectInvoice values)
        {
            try
            {

                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<PblGetTax>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new PblGetTax
                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name"].ToString(),
                            tax_name3 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString(),
                        });
                        values.PblGetTax = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPblGetTax4Dtl(MdlPmrTrnDirectInvoice values)
        {
            try
            {
                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<PblGetTaxFourDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new PblGetTaxFourDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name4 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.PblGetTaxFourDropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPblGetproducttype(MdlPmrTrnDirectInvoice values)
        {
            try
            {

                msSQL = " select producttype_gid, producttype_code , producttype_name from pmr_mst_tproducttype";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<PblGetproducttype>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new PblGetproducttype
                        {
                            producttype_gid = dt["producttype_gid"].ToString(),
                            producttype_code = dt["producttype_code"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                        });
                        values.PblGetproducttype = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaAdditional(MdlPmrTrnDirectInvoice values)
        {
            try
            {
                msSQL = "select additional_gid,additional_name from pmr_trn_tadditional";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var Getadditional = new List<Getadditional_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow ad in dt_datatable.Rows)
                    {
                        Getadditional.Add(new Getadditional_list
                        {
                            additional_gid = ad["additional_gid"].ToString(),
                            additional_name = ad["additional_name"].ToString(),
                        });
                        values.Getadditional_list = Getadditional;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Additional type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaDiscount(MdlPmrTrnDirectInvoice values)
        {
            try
            {
                msSQL = "select discount_gid,discount_name from pmr_trn_tdiscount";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var Getadditional = new List<Getdiscount_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow ad in dt_datatable.Rows)
                    {
                        Getadditional.Add(new Getdiscount_list
                        {
                            discount_gid = ad["discount_gid"].ToString(),
                            discount_name = ad["discount_name"].ToString(),
                        });
                        values.Getdiscount_list = Getadditional;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Additional type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //Edit
        public void DaGetPmrTrnInvoiceedit(string user_gid,string invoice_gid, MdlPmrTrnDirectInvoice values)
        {
            try
            {

                msSQL = "  delete from acp_tmp_tinvoice where  created_by = '" + user_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select  invoicedtl_gid,  invoice_gid, product_gid,  product_code,   product_name, productuom_name,  " +
                    " uom_gid,  productgroup_name,  producttype_gid,  product_price,  product_total,  discount_percentage,  discount_amount,  " +
                    " tax_name,  tax_name2,  tax_name3,  tax_percentage,  tax_percentage2,  tax_percentage3,  tax_amount,  tax_amount2,  tax_amount3," +
                    "  tax1_gid,  tax2_gid,  tax3_gid,created_by,  display_field, product_price_L, tax_amount1_L, tax_amount2_L, tax_amount3_L, qty_invoice," +
                    "  taxsegment_gid from acp_trn_tinvoicedtl where invoice_gid = '" + invoice_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow da in dt_datatable.Rows)
                    {

                        msSQL = " insert into acp_tmp_tinvoice  ( " +
                          " tmpinvoicedtl_gid," +
                          " tmpinvoice_gid," +
                          " qty_invoice," +
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
                           " ) values ( " +
                           "'" + da["invoicedtl_gid"].ToString() + "'," +
                           "'" + da["invoice_gid"].ToString() + "'," +
                           "'" + da["qty_invoice"].ToString() + "'," +
                           "'" + da["product_gid"].ToString() + "'," +
                          "'" + da["product_code"].ToString() + "'," +
                          "'" + da["product_name"].ToString() + "'," +
                          "'" + da["display_field"].ToString() + "'," +
                          "'" + da["productuom_name"].ToString() + "'," +
                          "'" + da["uom_gid"].ToString() + "'," +
                          "'" + da["qty_invoice"].ToString() + "'," +
                          "'" + da["product_price"].ToString() + "'," +
                          "'" + da["discount_percentage"].ToString() + "', " +
                          "'" + da["discount_amount"].ToString() + "', " +
                          " '" + da["taxsegment_gid"].ToString() + "', " +
                          " '" + da["tax1_gid"].ToString() + "', " +
                          " '" + da["tax2_gid"].ToString() + "', " +
                          " '" + da["tax3_gid"].ToString() + "', " +
                          " '" + da["tax_name"].ToString() + "', " +
                          " '" + da["tax_name2"].ToString() + "', " +
                          " '" + da["tax_name3"].ToString() + "', " +
                          "'" + da["tax_percentage"].ToString() + "', " +
                          "'" + da["tax_percentage2"].ToString() + "', " +
                          "'" + da["tax_percentage3"].ToString() + "', " +
                          "'" + da["tax_amount"].ToString() + "', " +
                          "'" + da["tax_amount2"].ToString() + "', " +
                          "'" + da["tax_amount3"].ToString() + "', " +
                          "'" + user_gid + "', " +
                          "'" + da["product_total"].ToString() + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                    }

                }
       

                if (mnResult == 1)
                {
                    values.status = true;


                }

                msSQL = " SELECT a.purchase_type,x.currencyexchange_gid,a.invoice_gid,e.tax1_gid,e.tax_name as tax_name4,k.email_id,a.mode_despatch,a.billing_email,a.shipping_address,a.termsandconditions," +
                        " a.invoice_refno,a.invoice_type,concat(m.address1,'\n',m.address2,'\n',m.city,'\n',m.state,'\n',m.postal_code" +
                        " ) as vendor_address,a.vendorinvoiceref_no,k.contact_telephonenumber,a.order_total,a.received_amount,a.received_year,k.contactperson_name," +
                        " j.branch_name,k.vendor_companyname,a.branch_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, " +
                        " date_format(a.payment_date,'%d-%m-%Y') as payment_date,a.round_off, a.payment_term,a.delivery_term, a.vendor_gid,a.extraadditional_amount as extraadditional_amount, " +
                        " case when a.currency_code is null then 'INR' else a.currency_code end as currency_code,a.extradiscount_amount as extradiscount_amount, " +
                        " case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,e.product_total as price, " +
                        " a.invoice_remarks, a.payment_days,a.reject_reason, a.invoice_status, a.invoice_amount as invoice_amount, a.invoice_reference, " +
                        " a.freightcharges_amount as freightcharges_amount, a.additionalcharges_amount as additionalcharges_amount, " +
                        " a.discount_amount as discount_amount, a.total_amount as total_amount,  " +
                        " a.freightcharges as freightcharges,a.buybackorscrap as buybackorscrap,a.invoice_total,a.raised_amount,a.tax_amount,a.tax_name " +
                        " FROM acp_trn_tinvoice a " +
                        " left join acp_trn_tinvoicedtl e on a.invoice_gid=e.invoice_gid " +
                        " left join acp_trn_tpo2invoice g on a.invoice_gid=g.invoice_gid " +
                        " left join hrm_mst_tbranch j on a.branch_gid=j.branch_gid " +
                        " left join acp_mst_tvendor k on a.vendor_gid=k.vendor_gid " +
                        " left join crm_trn_tcurrencyexchange x on a.currency_code = x.currency_code " +
                        " left join adm_mst_taddress m on m.address_gid=k.address_gid  " +
                        " where a.invoice_gid = '" + invoice_gid + "' group by a.invoice_gid ";


                //dt_datatable = objdbconn.GetDataTable(msSQL);
                //var getModuleList = new List<invoice_listsedit>();
                //if (dt_datatable.Rows.Count != 0)
                //{
                //    foreach (DataRow dt in dt_datatable.Rows)
                //    {
                ds_dataset = objdbconn.GetDataSet(msSQL, "acp_trn_tinvoice");
                var getModuleList = new List<invoice_listsedit>();
                if (ds_dataset.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dt in ds_dataset.Tables[0].Rows)
                    {

                        getModuleList.Add(new invoice_listsedit
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            vendorinvoiceref_no = dt["vendorinvoiceref_no"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            invoice_type = dt["invoice_type"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            round_off = dt["round_off"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            price = dt["price"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                            delivery_term = dt["delivery_term"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            tax_name4 = dt["tax_name4"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            tax1_gid = dt["tax1_gid"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            purchase_type = dt["purchase_type"].ToString(),
                        });
                        values.invoice_listsedit = getModuleList;

                    }
                }
            

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        // update 
        public void DaPblProductUpdate(string user_gid, PblDirectInvoice values)
        {
            try
            {
                int lsfreight = 0;
                int lsinsurance = 0;

                string invoice_date = values.invoice_date;
                if (!string.IsNullOrEmpty(invoice_date))
                {
                    DateTime uiDate = DateTime.ParseExact(invoice_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    invoicedate = uiDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    invoicedate = DateTime.Now.ToString("yyyy-MM-dd");
                }

                string due_date = values.due_date;
                if (!string.IsNullOrEmpty(due_date))
                {
                    DateTime due_dates = DateTime.ParseExact(due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    due_date = due_dates.ToString("yyyy-MM-dd");
                }
                else
                {
                    //due_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime uiDate = DateTime.ParseExact(values.due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    due_date = uiDate.ToString("yyyy-MM-dd");
                }
                if (string.IsNullOrEmpty(values.tax_name4))
                {
                    lstaxpercentage = "0";

                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                    lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                }
                msSQL = "select CASE WHEN tax_prefix IS NULL THEN tax_name ELSE tax_prefix  END AS tax_name from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                lstax_name4 = objdbconn.GetExecuteScalar(msSQL);

                //msGetGID = objcmnfunctions.GetMasterGID("SIVP");
                msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                string vendor_gid = values.vendor_companyname;


                msSQL = "UPDATE acp_trn_tinvoice SET " +
                            "vendor_gid = '" + values.vendor_companyname + "', " +
                            "vendor_address = '" + values.vendor_details + "', " +
                            "shipping_address = '" + (string.IsNullOrEmpty(values.shipping_address) ? values.shipping_address : values.shipping_address.Replace("'", "\\\'")) + "', " +
                            "invoice_refno = '" + values.invoice_ref_no + "', " +
                            "user_gid = '" + user_gid + "', " +
                            "invoice_date = '" + invoicedate + "', " +
                            "payment_date = '" + due_date + "', " +
                            "payment_days = '" + values.payment_days + "', " +
                            "termsandconditions = '" + (string.IsNullOrEmpty(values.template_content) ? values.template_content : values.template_content.Replace("'", "\\\'")) + "', " +
                            "systemgenerated_amount = '0.00', " +
                            "additionalcharges_amount = '" + (string.IsNullOrEmpty(values.addoncharge) ? "0.00" : values.addoncharge) + "', " +
                            "discount_amount = '" + (string.IsNullOrEmpty(values.additional_discount) ? "0.00" : values.additional_discount) + "', " +
                            "total_amount = '" + (string.IsNullOrEmpty(values.totalamount) ? "0.00" : values.totalamount) + "', " +
                            "invoice_amount = '" + (string.IsNullOrEmpty(values.grandtotal) ? "0.00" : values.grandtotal) + "', " +
                            "payment_term = '" + values.payment_terms + "', " +
                            "delivery_term = '" + values.delivery_terms + "', " +
                            "freight_terms = '" + values.freight_terms + "', " +
                            "mode_despatch = '" + values.dispatch_mode + "', " +
                            "billing_email = '" + values.billing_mail + "', " +
                            "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                            "invoice_status = 'Invoice Approved', " +
                            "invoice_flag = 'Payment Pending', " +
                            "invoice_remarks = '" + (string.IsNullOrEmpty(values.invoice_remarks) ? values.invoice_remarks : values.invoice_remarks.Replace("'", "\\\'")) + "', " +
                            "invoice_from = 'Direct Invoice', " +
                            "additionalcharges_amount_L = '0.00', " +
                            "discount_amount_L = '0.00', " +
                            "total_amount_L = '" + (string.IsNullOrEmpty(values.grandtotal) ? "0.00" : values.grandtotal) + "', " +
                            "currency_code = '" + values.currency_code + "', " +
                            "exchange_rate = '" + (string.IsNullOrEmpty(values.exchange_rate) ? "0.00" : values.exchange_rate) + "', " +
                            "freightcharges = '" + (string.IsNullOrEmpty(values.freightcharges) ? "0.00" : values.freightcharges) + "', " +
                            "extraadditional_code = '0.00', " +
                            "extradiscount_code = '0.00', " +
                            "extraadditional_amount = '0.00', " +
                            "extradiscount_amount = '0.00', " +
                            "buybackorscrap = '" + (string.IsNullOrEmpty(values.buybackorscrap) ? "0.00" : values.buybackorscrap) + "', " +
                            "vendorinvoiceref_no = '" + values.vendor_ref_no + "', " +
                            "branch_gid = '" + values.branch_name + "', " +
                            "tax_gid = '" + values.tax_name4 + "', " +
                            "tax_name = '" + lstax_name4 + "', " +
                            "tax_percentage = '" + lstaxpercentage + "', " +
                            "tax_amount = '" + (string.IsNullOrEmpty(values.tax_amount4) ? "0.00" : values.tax_amount4) + "', " +
                            "purchase_type = '" + values.purchasetype_name + "', " +
                            "round_off = '" + values.roundoff + "' " +
                            "WHERE invoice_gid = '" + values.invoice_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                if (mnResult == 1)
                {
                    msSQL = "SELECT a.tmpinvoicedtl_gid, a.product_gid,a.qty_invoice, FORMAT(a.qty_ordered, 2) AS qty_ordered," +
                        " a.product_name, a.product_code," +
                     " b.productgroup_gid, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                     "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price," +
                     " a.discount_percentage, FORMAT(a.discount_amount, 2) AS discount_amount, a.tax_percentage," +
                     " FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field,  a.tax_name," +
                     " a.tax_name2, a.tax_name3, a.tax_percentage2, a.tax_percentage3, " +
                     "FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3," +
                     " a.tax1_gid, a.tax2_gid,a.tax3_gid, a.taxsegment_gid FROM acp_tmp_tinvoice a" +
                     " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                     "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                     " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid" +
                     " WHERE a.created_by= '" + user_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msSQL = "select invoicedtl_gid from acp_trn_tinvoicedtl where invoicedtl_gid = '"+ dt["tmpinvoicedtl_gid"].ToString() + "'";
                             string lsinvoicedtlgid = objdbconn.GetExecuteScalar(msSQL);

                            if(lsinvoicedtlgid == null || lsinvoicedtlgid == "") {
                                msSQL = " insert into acp_trn_tinvoicedtl (" +
                            " invoicedtl_gid, " +
                            " invoice_gid, " +
                            " product_gid, " +
                         " product_code, " +
                             " product_name, " +
                            " productuom_name, " +
                           " uom_gid, " +
                            " productgroup_name, " +
                           " producttype_gid, " +
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
                           " display_field," +
                           " product_price_L," +
                           " tax_amount1_L," +
                           " tax_amount2_L," +
                           " tax_amount3_L," +
                           " qty_invoice, " +
                           " taxsegment_gid," +
                           " created_by" +
                             " )values ( " +
                             "'" + dt["tmpinvoicedtl_gid"].ToString() + "', " +
                             "'" + values.invoice_ref_no + "'," +
                             "'" + dt["product_gid"].ToString() + "', " +
                             "'" + dt["product_code"].ToString() + "', " +
                             "'" + dt["product_name"].ToString() + "', " +
                             "'" + dt["productuom_name"].ToString() + "', " +
                             "'" + dt["uom_gid"].ToString() + "', " +
                              "'" + dt["productgroup_name"].ToString() + "', " +
                             "'" + dt["producttype_gid"].ToString() + "', " +
                             "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                             "'" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                             "'" + dt["discount_percentage"].ToString() + "', " +
                             "'" + dt["discount_amount"].ToString().Replace(",", "") + "', " +
                             "'" + dt["tax_name"].ToString() + "', " +
                             "'" + dt["tax_name2"].ToString() + "', " +
                             "'" + dt["tax_name3"].ToString() + "', " +
                             "'" + dt["tax_percentage"].ToString() + "', " +
                             "'" + dt["tax_percentage2"].ToString() + "', " +
                             "'" + dt["tax_percentage3"].ToString() + "', " +
                             "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                             "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                             "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                             "'" + dt["tax1_gid"].ToString().Replace(",", "") + "', " +
                             "'" + dt["tax2_gid"].ToString().Replace(",", "") + "', " +
                             "'" + dt["tax3_gid"].ToString().Replace(",", "") + "', " +
                             "'" + dt["display_field"].ToString().Replace(",", "") + "', " +
                                "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                            "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                             "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                             "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                             "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                             "'" + dt["taxsegment_gid"].ToString() + "'," +
                             "'" + user_gid + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            else {
                                msSQL = "UPDATE acp_trn_tinvoicedtl SET " +
                                        "product_code = '" + dt["product_code"].ToString() + "', " +
                                        "product_name = '" + dt["product_name"].ToString() + "', " +
                                        "productuom_name = '" + dt["productuom_name"].ToString() + "', " +
                                        "uom_gid = '" + dt["uom_gid"].ToString() + "', " +
                                        "productgroup_name = '" + dt["productgroup_name"].ToString() + "', " +
                                        "producttype_gid = '" + dt["producttype_gid"].ToString() + "', " +
                                        "product_price = '" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                        "product_total = '" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                        "discount_percentage = '" + dt["discount_percentage"].ToString() + "', " +
                                        "discount_amount = '" + dt["discount_amount"].ToString().Replace(",", "") + "', " +
                                        "tax_name = '" + dt["tax_name"].ToString() + "', " +
                                        "tax_name2 = '" + dt["tax_name2"].ToString() + "', " +
                                        "tax_name3 = '" + dt["tax_name3"].ToString() + "', " +
                                        "tax_percentage = '" + dt["tax_percentage"].ToString() + "', " +
                                        "tax_percentage2 = '" + dt["tax_percentage2"].ToString() + "', " +
                                        "tax_percentage3 = '" + dt["tax_percentage3"].ToString() + "', " +
                                        "tax_amount = '" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                        "tax_amount2 = '" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                        "tax_amount3 = '" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                        "tax1_gid = '" + dt["tax1_gid"].ToString().Replace(",", "") + "', " +
                                        "tax2_gid = '" + dt["tax2_gid"].ToString().Replace(",", "") + "', " +
                                        "tax3_gid = '" + dt["tax3_gid"].ToString().Replace(",", "") + "', " +
                                        "display_field = '" + dt["display_field"].ToString().Replace(",", "") + "', " +
                                        "product_price_L = '" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                        "tax_amount1_L = '" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                        "tax_amount2_L = '" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                        "tax_amount3_L = '" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                        "qty_invoice = '" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                        "taxsegment_gid = '" + dt["taxsegment_gid"].ToString() + "' " +
                                    "WHERE invoicedtl_gid = '" + dt["tmpinvoicedtl_gid"].ToString() + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            }
                            // msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");




                        }

                    }

                    if (mnResult != 0)

                    {
                        msSQL = "select finance_flag from adm_mst_tcompany ";
                        string finance_flag = objdbconn.GetExecuteScalar(msSQL);
                        if (finance_flag == "Y")
                        {
                            double product;
                            double discount;
                            msSQL = "SELECT SUM(COALESCE(qty_invoice, 2) * COALESCE(product_price, 2)) AS product, ROUND(SUM(discount_amount), 2) AS discount FROM acp_trn_tinvoicedtl WHERE invoice_gid = '" + values.invoice_gid + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                            if (objOdbcDataReader.HasRows)
                            {
                                objOdbcDataReader.Read();
                                product = objOdbcDataReader["product"] != DBNull.Value ? double.Parse(objOdbcDataReader["product"].ToString()) : 0.00;
                                discount = objOdbcDataReader["discount"] != DBNull.Value ? double.Parse(objOdbcDataReader["discount"].ToString()) : 0.00;
                            }
                            else
                            {
                                product = 0.00;
                                discount = 0.00;
                            }

                            objOdbcDataReader.Close();


                            //double lsbasic_amount = product - discount;

                            //double addonCharges = double.TryParse(values.addoncharge, out double addonChargesValue) ? addonChargesValue : 0;
                            //double freightCharges = double.TryParse(values.freightcharges, out double freightChargesValue) ? freightChargesValue : 0;
                            //double forwardingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                            //double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                            //double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                            //double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                            //double buybackCharges = double.TryParse(values.buybackorscrap, out double buybackChargesValue) ? buybackChargesValue : 0;
                            //double overalltax_amount = double.TryParse(values.tax_amount4, out double overalltaxamount) ? overalltaxamount : 0;
                            //double grandtotal = double.TryParse(values.grandtotal, out double grand_total) ? grand_total : 0;

                            //objfincmn.jn_purchase_invoiceupdate(invoicedate, values.remarks, values.branch_name, values.invoice_ref_no,  values.invoice_gid 
                            // , lsbasic_amount, addonCharges, additionaldiscountAmount, grandtotal, vendor_gid, "Invoice", "PMR",
                            // values.purchase_type, roundoff, freightCharges, buybackCharges, lstax_name4, overalltax_amount, forwardingCharges, insuranceCharges);



                            double lsbasic_amount = product - discount;

                            double addonCharges = double.TryParse(values.addoncharge, out double addonChargesValue) ? addonChargesValue : 0;
                            double freightCharges = double.TryParse(values.freightcharges, out double freightChargesValue) ? freightChargesValue : 0;
                            double forwardingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                            double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                            double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                            double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                            double buybackCharges = double.TryParse(values.buybackorscrap, out double buybackChargesValue) ? buybackChargesValue : 0;
                            double overalltax_amount = double.TryParse(values.tax_amount4, out double overalltaxamount) ? overalltaxamount : 0;
                            double grandtotal = double.TryParse(values.grandtotal, out double grand_total) ? grand_total : 0;
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


                            objfincmn.jn_purchase_invoiceupdate(invoicedate, values.remarks, values.branch_name, values.invoice_ref_no, values.invoice_gid
                             , fin_basic_amount, fin_addonCharges, fin_additionaldiscountAmount, fin_grandtotal, vendor_gid, "Invoice", "PMR",
                             values.purchase_type, fin_roundoff, fin_freightcharges, fin_buybackCharges, lstax_name4, fin_overalltax_amount, fin_forwardingCharges, fin_insuranceCharges);





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
                                            "WHERE invoice_gid = '" + values.invoice_gid + "' AND tax1_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                    objODBCDataReader1 = objdbconn.GetDataReader(msSQL);

                                    if (objODBCDataReader1.HasRows && objODBCDataReader1.Read())
                                    {
                                        lstax1 = objODBCDataReader1["tax1"] != DBNull.Value ? objODBCDataReader1["tax1"].ToString() : "0.00";
                                    }
                                    objODBCDataReader1.Close();

                                    // Tax 2 Calculation
                                    msSQL = "SELECT SUM(tax_amount2) AS tax2 FROM acp_trn_tinvoicedtl " +
                                            "WHERE invoice_gid = '" + values.invoice_gid + "' AND tax2_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                    objODBCDataReader2 = objdbconn.GetDataReader(msSQL);

                                    if (objODBCDataReader2.HasRows && objODBCDataReader2.Read())
                                    {
                                        lstax2 = objODBCDataReader2["tax2"] != DBNull.Value ? objODBCDataReader2["tax2"].ToString() : "0.00";
                                    }
                                    objODBCDataReader2.Close();

                                    // Tax 3 Calculation
                                    msSQL = "SELECT SUM(tax_amount3_L) AS tax3 FROM acp_trn_tinvoicedtl " +
                                            "WHERE invoice_gid = '" + values.invoice_gid + "' AND tax3_gid = '" + objODBCDataReader["tax_gid"] + "'";
                                    objODBCDataReader3 = objdbconn.GetDataReader(msSQL);

                                    if (objODBCDataReader3.HasRows && objODBCDataReader3.Read())
                                    {
                                        lstax3 = objODBCDataReader3["tax3"] != DBNull.Value ? objODBCDataReader3["tax3"].ToString() : "0.00";
                                    }
                                    objODBCDataReader3.Close();

                                    if (lstax1 != "0.00" || lstax2 != "0.00" || lstax3 != "0.00")
                                    {
                                        lstax_gid = objODBCDataReader["tax_gid"].ToString();
                                        lstaxsum = (Convert.ToDecimal(lstax1.Replace(",", "")) +
                                                    Convert.ToDecimal(lstax2.Replace(",", "")) +
                                                    Convert.ToDecimal(lstax3.Replace(",", ""))).ToString();

                                        lstaxamount = (Convert.ToDecimal(lstaxsum) * Convert.ToDecimal(values.exchange_rate)).ToString();

                                        objfincmn.jn_purchase_taxupdate( values.invoice_gid , values.invoice_ref_no, values.remarks, lstaxamount, lstax_gid);
                                    }
                                }
                            }

                            objdbconn.CloseConn();
                        }

                        values.status = true;
                        values.message = "Direct Invoice Updated Successfully!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating product in Direct Invoice!";
                    }

                    msSQL = "  delete from acp_tmp_tinvoice where created_by='" + user_gid + "'  ";
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

        public void DaGetInvoiceDraftsSummary(MdlPmrTrnDirectInvoice values)
        {
            try
            {

                msSQL =  " select a.vendor_gid,b.vendor_companyname,a.invoicedraft_gid,a.invoice_amount,date_format (a.invoice_date,'%d-%m-%Y') as invoice_date, " +
                         " Case  when a.invoice_reference is null or a.invoice_reference='' then a.invoicedraft_gid else a.invoice_reference end as invoice_reference " +
                         " from acp_trn_tinvoicedraft a left join  acp_mst_tvendor b on a.vendor_gid = b.vendor_gid group by a.invoicedraft_gid  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDraftinvoice>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDraftinvoice
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            invoicedraft_gid = dt["invoicedraft_gid"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_reference = dt["invoice_reference"].ToString(),
                        });
                        values.GetDraftinvoice = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }



        public void DaPblDraftOverallSubmit(string user_gid, PblDirectInvoice values)
        {
            try
            {
                if (string.IsNullOrEmpty(values.invoice_ref_no))
                {
                    int lsfreight = 0;
                    int lsinsurance = 0;

                    string invoice_date = values.invoice_date;
                    if (!string.IsNullOrEmpty(invoice_date))
                    {
                        DateTime uiDate = DateTime.ParseExact(invoice_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        invoicedate = uiDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        invoicedate = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    string due_date = values.due_date;
                    if (!string.IsNullOrEmpty(due_date))
                    {
                        DateTime due_dates = DateTime.ParseExact(due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        due_date = due_dates.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        //due_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        DateTime uiDate = DateTime.ParseExact(values.due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        due_date = uiDate.ToString("yyyy-MM-dd");
                    }
                    if (string.IsNullOrEmpty(values.tax_name4))
                    {
                        lstaxpercentage = "0";

                    }
                    else
                    {
                        msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                        lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                    }

                    string ls_referenceno;

                    if (values.invoice_ref_no == "")
                    {
                        ls_referenceno = objcmnfunctions.getSequencecustomizerGID("PINV", "PBL", values.branch_name);
                    }
                    else
                    {

                        ls_referenceno = values.invoice_ref_no;
                    }

                    string vendor_gid = values.vendor_companyname;


                    msSQL = "select vendor_code from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                    string lsvenorcode1 = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select vendor_companyname from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                    string lsvendor_companyname1 = objdbconn.GetExecuteScalar(msSQL);

                    
                    msSQL = "SELECT account_gid from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);



                    msGetGID = objcmnfunctions.GetMasterGID("SIVP");
                    msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                    msGetGIDPO = objcmnfunctions.GetMasterGID("PPRP");
                    msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP", "", user_gid);
                    // txtinvoicefer = objcmnfunctions.GetMasterGID("DINV");
                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "' ";
                    string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select CASE WHEN tax_prefix IS NULL THEN tax_name ELSE tax_prefix  END AS tax_name FROM    acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                    lstax_name4 = objdbconn.GetExecuteScalar(msSQL);
                    if (!string.IsNullOrEmpty(values.invoice_gid))
                    {
                        msSQL = "UPDATE acp_trn_tinvoicedraft SET " +
                                "vendor_gid = '" + values.vendor_companyname + "', " +
                                "vendor_address = '" + values.vendor_details + "', ";

                        // Handle the shipping address replacement for quotes
                        if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                        {
                            msSQL += "shipping_address = '" + values.shipping_address.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "shipping_address = '" + values.shipping_address + "', ";
                        }

                        msSQL += "invoice_refno = '" + ls_referenceno + "', " +
                        "user_gid = '" + user_gid + "', " +
                        "invoice_date = '" + invoicedate + "', " +
                        "payment_date = '" + due_date + "', " +
                        "payment_days = '" + values.payment_days + "', ";

                        // Handle template content replacement for quotes
                        if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                        {
                            msSQL += "termsandconditions = '" + values.template_content.Replace("'", "") + "', ";
                        }
                        else
                        {
                            msSQL += "termsandconditions = '" + values.template_content + "', ";
                        }

                        msSQL += "systemgenerated_amount = '0.00', ";

                        // Handle additional charges
                        if (string.IsNullOrEmpty(values.addoncharge))
                        {
                            msSQL += "additionalcharges_amount = '0.00', ";
                        }
                        else
                        {
                            msSQL += "additionalcharges_amount = '" + values.addoncharge + "', ";
                        }

                        // Handle discounts
                        if (string.IsNullOrEmpty(values.additional_discount))
                        {
                            msSQL += "discount_amount = '0.00', ";
                        }
                        else
                        {
                            msSQL += "discount_amount = '" + values.additional_discount + "', ";
                        }

                        // Handle total amount
                        if (string.IsNullOrEmpty(values.totalamount))
                        {
                            msSQL += "total_amount = '0.00', ";
                        }
                        else
                        {
                            msSQL += "total_amount = '" + values.totalamount + "', ";
                        }

                        // Handle grand total
                        if (string.IsNullOrEmpty(values.grandtotal))
                        {
                            msSQL += "invoice_amount = '0.00', ";
                        }
                        else
                        {
                            msSQL += "invoice_amount = '" + values.grandtotal + "', ";
                        }

                        msSQL += "payment_term = '" + values.payment_terms + "', " +
                                "delivery_term = '" + values.delivery_terms + "', " +
                                "freight_terms = '" + values.freight_terms + "', " +
                                "mode_despatch = '" + values.dispatch_mode + "', " +
                                "billing_email = '" + values.billing_mail + "', " +
                                "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                "invoice_status = 'Invoice Approved', " +
                                "invoice_flag = 'Payment Pending', ";

                        // Handle invoice remarks replacement for quotes
                        if (!string.IsNullOrEmpty(values.invoice_remarks) && values.invoice_remarks.Contains("'"))
                        {
                            msSQL += "invoice_remarks = '" + values.invoice_remarks.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "invoice_remarks = '" + values.invoice_remarks + "', ";
                        }

                        msSQL += "invoice_from = 'Direct Invoice', " +
                                "additionalcharges_amount_L = '0.00', " +
                                "discount_amount_L = '0.00', ";

                        // Handle grand total again
                        if (string.IsNullOrEmpty(values.grandtotal))
                        {
                            msSQL += "total_amount_L = '0.00', ";
                        }
                        else
                        {
                            msSQL += "total_amount_L = '" + values.grandtotal + "', ";
                        }

                        msSQL += "currency_code = '" + lscurrency_code + "', ";

                        // Handle exchange rate
                        if (string.IsNullOrEmpty(values.exchange_rate))
                        {
                            msSQL += "exchange_rate = '0.00', ";
                        }
                        else
                        {
                            msSQL += "exchange_rate = '" + values.exchange_rate + "', ";
                        }

                        // Handle freight charges
                        if (string.IsNullOrEmpty(values.freightcharges))
                        {
                            msSQL += "freightcharges = '0.00', ";
                        }
                        else
                        {
                            msSQL += "freightcharges = '" + values.freightcharges + "', ";
                        }

                        msSQL += "extraadditional_code = '0.00', " +
                                "extradiscount_code = '0.00', " +
                                "extraadditional_amount = '0.00', " +
                                "extradiscount_amount = '0.00', ";

                        // Handle buyback or scrap
                        if (string.IsNullOrEmpty(values.buybackorscrap))
                        {
                            msSQL += "buybackorscrap = '0.00', ";
                        }
                        else
                        {
                            msSQL += "buybackorscrap = '" + values.buybackorscrap + "', ";
                        }

                        msSQL += "vendorinvoiceref_no = '" + values.vendor_ref_no + "', " +
                        "branch_gid = '" + values.branch_name + "', " +
                        "tax_gid = '" + values.tax_name4 + "', " +
                        "tax_name = '" + lstax_name4 + "', " +
                        "tax_percentage = '" + lstaxpercentage + "', ";

                        // Handle tax amount
                        if (string.IsNullOrEmpty(values.tax_amount4))
                        {
                            msSQL += "tax_amount = '0.00', ";
                        }
                        else
                        {
                            msSQL += "tax_amount = '" + values.tax_amount4 + "', ";
                        }

                        msSQL += "purchase_type = '" + values.purchase_type + "', " +
                                 "round_off = '" + values.roundoff + "' ";

                        // Add the WHERE clause to update based on invoicedraft_gid
                        msSQL += "WHERE invoicedraft_gid = '" + values.invoice_gid + "'";

                      


                    }
                    else
                    {
                        msSQL = " insert into acp_trn_tinvoicedraft (" +
                   " invoicedraft_gid, " +
                   " vendor_gid," +
                   " vendor_address, " +
                   " shipping_address, " +
                   " invoice_refno, " +
                   " user_gid, " +
                   " invoice_date," +
                   " payment_date," +
                   " payment_days," +
                   " termsandconditions," +
                   " systemgenerated_amount, " +
                   " additionalcharges_amount, " +
                   " discount_amount, " +
                   " total_amount, " +
                   " invoice_amount, " +
                   " payment_term," +
                   " delivery_term," +
                   " freight_terms," +
                   " mode_despatch, " +
                   " billing_email, " +
                   " created_date," +
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
                   " buybackorscrap," +
                   " vendorinvoiceref_no," +
                   " branch_gid," +
                   " tax_gid," +
                    " tax_name," +
                   " tax_percentage," +
                   " tax_amount," +
                   " purchase_type," +
                   " round_off" +
                   " ) values (" +
                   "'" + msGetGID + "'," +
                   "'" + values.vendor_companyname + "'," +
                     "'" + values.vendor_details + "',";
                        if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                        {
                            msSQL += "'" + values.shipping_address.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.shipping_address + "', ";
                        }
                        msSQL += "'" + ls_referenceno + "'," +
                       "'" + user_gid + "'," +
                       "'" + invoicedate + "'," +
                       "'" + due_date + "'," +
                       "'" + values.payment_days + "',";
                        if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                        {
                            msSQL += "'" + values.template_content.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.template_content + "', ";
                        }
                        msSQL += "'0.00',";
                        if (values.addoncharge == "" || values.addoncharge == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.addoncharge + "',";

                        }
                        if (values.additional_discount == "" || values.additional_discount == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.additional_discount + "',";

                        }
                        if (values.totalamount == "" || values.totalamount == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.totalamount + "',";

                        }
                        if (values.grandtotal == "" || values.grandtotal == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.grandtotal + "',";

                        }

                        msSQL += "'" + values.payment_terms + "'," +
                        "'" + values.delivery_terms + "'," +
                        "'" + values.freight_terms + "'," +
                        "'" + values.dispatch_mode + "'," +
                        "'" + values.billing_mail + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                       "'" + "Invoice Approved" + "'," +
                       "'" + "Payment Pending" + "',";
                        if (!string.IsNullOrEmpty(values.invoice_remarks) && values.invoice_remarks.Contains("'"))
                        {
                            msSQL += "'" + values.invoice_remarks.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.invoice_remarks + "', ";
                        }
                        msSQL += "'" + "Direct Invoice" + "'," +
                       "'0.00'," +
                       "'0.00',";
                        if (values.grandtotal == "" && values.grandtotal == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.grandtotal + "',";

                        }
                        msSQL += "'" + lscurrency_code + "',";
                        if (values.exchange_rate == "" && values.exchange_rate == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.exchange_rate + "',";

                        }

                        if (values.freightcharges == "" && values.freightcharges == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.freightcharges + "',";

                        }

                        msSQL += "'0.00',";
                        msSQL += "'0.00',";
                        msSQL += "'0.00',";
                        msSQL += "'0.00',";


                        if (values.buybackorscrap == "" || values.buybackorscrap == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.buybackorscrap + "',";

                        }
                        msSQL +=
                        "'" + values.vendor_ref_no + "'," +
                        "'" + values.branch_name + "',";
                        msSQL += "'" + values.tax_name4 + "'," +
                        "'" + lstax_name4 + "'," +
                         "'" + lstaxpercentage + "',";

                        if (string.IsNullOrEmpty(values.tax_amount4))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + values.tax_amount4 + "',";
                        }
                        msSQL += "'" + values.purchase_type + "'," +
                                 "'" + values.roundoff + "')";
                    }
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);              




                    if (mnResult == 1)
                    {
                        msSQL = " delete from acp_trn_tinvoicedraftdtl where invoicedraft_gid = '" + values.invoice_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "SELECT a.tmpinvoicedtl_gid, a.product_gid,a.qty_invoice, FORMAT(a.qty_ordered, 2) AS qty_ordered," +
                            " a.product_name, a.product_code," +
                         " b.productgroup_gid, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                         "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price," +
                         " a.discount_percentage, a.discount_amount, a.tax_percentage," +
                         " FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field,  a.tax_name," +
                         " a.tax_name2, a.tax_name3, a.tax_percentage2, a.tax_percentage3, " +
                         "FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3," +
                         " a.tax1_gid, a.tax2_gid,a.tax3_gid, a.taxsegment_gid FROM acp_tmp_tinvoice a" +
                         " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                         "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                         " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid" +
                         " WHERE a.created_by= '" + user_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                msSQL = "select tax_prefix from acp_mst_ttax where tax_gid ='" + dt["tax2_gid"].ToString() + "' ";
                                string lstax2 = objdbconn.GetExecuteScalar(msSQL);

                                if (!string.IsNullOrEmpty(values.invoice_gid))
                                {
                                    msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                                    msSQL = " insert into acp_trn_tinvoicedraftdtl (" +
                                    " invoicedtldraft_gid, " +
                                    " invoicedraft_gid, " +
                                    " product_gid, " +
                                 " product_code, " +
                                     " product_name, " +
                                    " productuom_name, " +
                                   " uom_gid, " +
                                    " productgroup_name, " +
                                   " producttype_gid, " +
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
                                   " display_field," +
                                   " product_price_L," +
                                   " tax_amount1_L," +
                                   " tax_amount2_L," +
                                   " tax_amount3_L," +
                                   " qty_invoice, " +
                                   " taxsegment_gid" +
                                     " )values ( " +
                                     "'" + msDIGetGID + "', " +
                                     "'" + values.invoice_gid + "'," +
                                     "'" + dt["product_gid"].ToString() + "', " +
                                     "'" + dt["product_code"].ToString() + "', " +
                                     "'" + dt["product_name"].ToString() + "', " +
                                     "'" + dt["productuom_name"].ToString() + "', " +
                                     "'" + dt["uom_gid"].ToString() + "', " +
                                      "'" + dt["productgroup_name"].ToString() + "', " +
                                     "'" + dt["producttype_gid"].ToString() + "', " +
                                     "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["discount_percentage"].ToString() + "', " +
                                     "'" + dt["discount_amount"].ToString() + "', " +
                                     "'" + dt["tax_name"].ToString() + "', " +
                                     "'" + lstax2 + "', " +
                                     "'" + dt["tax_name3"].ToString() + "', " +
                                     "'" + dt["tax_percentage"].ToString() + "', " +
                                     "'" + dt["tax_percentage2"].ToString() + "', " +
                                     "'" + dt["tax_percentage3"].ToString() + "', " +
                                     "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax1_gid"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax2_gid"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax3_gid"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["display_field"].ToString().Replace(",", "") + "', " +
                                        "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                    "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["taxsegment_gid"].ToString() + "')";
                                }
                                else
                                {

                                    msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                                    msSQL = " insert into acp_trn_tinvoicedraftdtl (" +
                                    " invoicedtldraft_gid, " +
                                    " invoicedraft_gid, " +
                                    " product_gid, " +
                                 " product_code, " +
                                     " product_name, " +
                                    " productuom_name, " +
                                   " uom_gid, " +
                                    " productgroup_name, " +
                                   " producttype_gid, " +
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
                                   " display_field," +
                                   " product_price_L," +
                                   " tax_amount1_L," +
                                   " tax_amount2_L," +
                                   " tax_amount3_L," +
                                   " qty_invoice, " +
                                   " taxsegment_gid" +
                                     " )values ( " +
                                     "'" + msDIGetGID + "', " +
                                     "'" + msGetGID + "'," +
                                     "'" + dt["product_gid"].ToString() + "', " +
                                     "'" + dt["product_code"].ToString() + "', " +
                                     "'" + dt["product_name"].ToString() + "', " +
                                     "'" + dt["productuom_name"].ToString() + "', " +
                                     "'" + dt["uom_gid"].ToString() + "', " +
                                      "'" + dt["productgroup_name"].ToString() + "', " +
                                     "'" + dt["producttype_gid"].ToString() + "', " +
                                     "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["discount_percentage"].ToString() + "', " +
                                     "'" + dt["discount_amount"].ToString() + "', " +
                                     "'" + dt["tax_name"].ToString() + "', " +
                                     "'" + lstax2 + "', " +
                                     "'" + dt["tax_name3"].ToString() + "', " +
                                     "'" + dt["tax_percentage"].ToString() + "', " +
                                     "'" + dt["tax_percentage2"].ToString() + "', " +
                                     "'" + dt["tax_percentage3"].ToString() + "', " +
                                     "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax1_gid"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax2_gid"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax3_gid"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["display_field"].ToString().Replace(",", "") + "', " +
                                        "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                    "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                     "'" + dt["taxsegment_gid"].ToString() + "')";

                                }
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                               

                            }

                        }
                        if (mnResult == 1)
                        {
                            msSQL = "  delete from acp_trn_tinvoicedraft where invoicedraft_gid='" + values.invoice_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            msSQL = "  delete from acp_trn_tinvoicedraftdtl where invoicedraft_gid='" + values.invoice_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            msSQL = "  delete from acp_tmp_tinvoice where created_by='" + user_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            values.status = true;
                            values.message = "Invoice Submitted Successfully";
                        }


                       
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding product in Direct Invoice!";
                        }

                        msSQL = "  delete from acp_tmp_tinvoice where created_by='" + user_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        dt_datatable.Dispose();
                    }
                }



                else
                {
                    msSQL = "select invoice_gid from acp_trn_tinvoice where invoice_refno ='" + values.invoice_ref_no + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        values.message = "Invoice Ref No Alredy Exist!";
                        return;
                    }
                    else
                    {

                        int lsfreight = 0;
                        int lsinsurance = 0;

                        string invoice_date = values.invoice_date;
                        if (!string.IsNullOrEmpty(invoice_date))
                        {
                            DateTime uiDate = DateTime.ParseExact(invoice_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            invoicedate = uiDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            invoicedate = DateTime.Now.ToString("yyyy-MM-dd");
                        }

                        string due_date = values.due_date;
                        if (!string.IsNullOrEmpty(due_date))
                        {
                            DateTime due_dates = DateTime.ParseExact(due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            due_date = due_dates.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            //due_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            DateTime uiDate = DateTime.ParseExact(values.due_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            due_date = uiDate.ToString("yyyy-MM-dd");
                        }
                        if (string.IsNullOrEmpty(values.tax_name4))
                        {
                            lstaxpercentage = "0";

                        }
                        else
                        {
                            msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                            lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                        }


                        msGetGID = objcmnfunctions.GetMasterGID("SIVP");
                        msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                        msGetGIDPO = objcmnfunctions.GetMasterGID("PPRP");
                        msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP", "", user_gid);
                        // txtinvoicefer = objcmnfunctions.GetMasterGID("DINV");
                        msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "' ";
                        string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select CASE WHEN tax_prefix IS NULL THEN tax_name ELSE tax_prefix  END AS tax_name FROM    acp_mst_ttax  where tax_gid='" + values.tax_name4 + "'";
                        lstax_name4 = objdbconn.GetExecuteScalar(msSQL);
                        //string ls_referenceno = "";
                        string ls_referenceno;

                        if (values.invoice_ref_no == "")
                        {
                            ls_referenceno = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", values.branch_name);
                        }
                        else
                        {

                            ls_referenceno = values.invoice_ref_no;
                        }

                        string vendor_gid = values.vendor_companyname;
                        msSQL = "select vendor_code from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                        string lsvenorcode1 = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select vendor_companyname from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "' ";
                        string lsvendor_companyname1 = objdbconn.GetExecuteScalar(msSQL);

                        if (!string.IsNullOrEmpty(values.invoice_gid))
                        {
                            msSQL = "UPDATE acp_trn_tinvoicedraft SET " +
                                    "vendor_gid = '" + values.vendor_companyname + "', " +
                                    "vendor_address = '" + values.vendor_details + "', ";

                            // Handle the shipping address replacement for quotes
                            if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                            {
                                msSQL += "shipping_address = '" + values.shipping_address.Replace("'", "\\\'") + "', ";
                            }
                            else
                            {
                                msSQL += "shipping_address = '" + values.shipping_address + "', ";
                            }

                            msSQL += "invoice_refno = '" + ls_referenceno + "', " +
                                    "user_gid = '" + user_gid + "', " +
                                    "invoice_date = '" + invoicedate + "', " +
                                    "payment_date = '" + due_date + "', " +
                                    "payment_days = '" + values.payment_days + "', ";

                            // Handle template content replacement for quotes
                            if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                            {
                                msSQL += "termsandconditions = '" + values.template_content.Replace("'", "") + "', ";
                            }
                            else
                            {
                                msSQL += "termsandconditions = '" + values.template_content + "', ";
                            }

                            msSQL += "systemgenerated_amount = '0.00', ";

                            // Handle additional charges
                            if (string.IsNullOrEmpty(values.addoncharge))
                            {
                                msSQL += "additionalcharges_amount = '0.00', ";
                            }
                            else
                            {
                                msSQL += "additionalcharges_amount = '" + values.addoncharge + "', ";
                            }

                            // Handle discounts
                            if (string.IsNullOrEmpty(values.additional_discount))
                            {
                                msSQL += "discount_amount = '0.00', ";
                            }
                            else
                            {
                                msSQL += "discount_amount = '" + values.additional_discount + "', ";
                            }

                            // Handle total amount
                            if (string.IsNullOrEmpty(values.totalamount))
                            {
                                msSQL += "total_amount = '0.00', ";
                            }
                            else
                            {
                                msSQL += "total_amount = '" + values.totalamount + "', ";
                            }

                            // Handle grand total
                            if (string.IsNullOrEmpty(values.grandtotal))
                            {
                                msSQL += "invoice_amount = '0.00', ";
                            }
                            else
                            {
                                msSQL += "invoice_amount = '" + values.grandtotal + "', ";
                            }

                            msSQL += "payment_term = '" + values.payment_terms + "', " +
                                    "delivery_term = '" + values.delivery_terms + "', " +
                                    "freight_terms = '" + values.freight_terms + "', " +
                                    "mode_despatch = '" + values.dispatch_mode + "', " +
                                    "billing_email = '" + values.billing_mail + "', " +
                                    "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                    "invoice_status = 'Invoice Approved', " +
                                    "invoice_flag = 'Payment Pending', ";

                            // Handle invoice remarks replacement for quotes
                            if (!string.IsNullOrEmpty(values.invoice_remarks) && values.invoice_remarks.Contains("'"))
                            {
                                msSQL += "invoice_remarks = '" + values.invoice_remarks.Replace("'", "\\\'") + "', ";
                            }
                            else
                            {
                                msSQL += "invoice_remarks = '" + values.invoice_remarks + "', ";
                            }

                            msSQL += "invoice_from = 'Direct Invoice', " +
                                    "additionalcharges_amount_L = '0.00', " +
                                    "discount_amount_L = '0.00', ";

                            // Handle grand total again
                            if (string.IsNullOrEmpty(values.grandtotal))
                            {
                                msSQL += "total_amount_L = '0.00', ";
                            }
                            else
                            {
                                msSQL += "total_amount_L = '" + values.grandtotal + "', ";
                            }

                            msSQL += "currency_code = '" + lscurrency_code + "', ";

                            // Handle exchange rate
                            if (string.IsNullOrEmpty(values.exchange_rate))
                            {
                                msSQL += "exchange_rate = '0.00', ";
                            }
                            else
                            {
                                msSQL += "exchange_rate = '" + values.exchange_rate + "', ";
                            }

                            // Handle freight charges
                            if (string.IsNullOrEmpty(values.freightcharges))
                            {
                                msSQL += "freightcharges = '0.00', ";
                            }
                            else
                            {
                                msSQL += "freightcharges = '" + values.freightcharges + "', ";
                            }

                            msSQL += "extraadditional_code = '0.00', " +
                                    "extradiscount_code = '0.00', " +
                                    "extraadditional_amount = '0.00', " +
                                    "extradiscount_amount = '0.00', ";

                            // Handle buyback or scrap
                            if (string.IsNullOrEmpty(values.buybackorscrap))
                            {
                                msSQL += "buybackorscrap = '0.00', ";
                            }
                            else
                            {
                                msSQL += "buybackorscrap = '" + values.buybackorscrap + "', ";
                            }

                            msSQL += "vendorinvoiceref_no = '" + values.vendor_ref_no + "', " +
                                        "branch_gid = '" + values.branch_name + "', " +
                                        "tax_gid = '" + values.tax_name4 + "', " +
                                        "tax_name = '" + lstax_name4 + "', " +
                                        "tax_percentage = '" + lstaxpercentage + "', ";

                            // Handle tax amount
                            if (string.IsNullOrEmpty(values.tax_amount4))
                            {
                                msSQL += "tax_amount = '0.00', ";
                            }
                            else
                            {
                                msSQL += "tax_amount = '" + values.tax_amount4 + "', ";
                            }

                            msSQL += "purchase_type = '" + values.purchase_type + "', " +
                                     "round_off = '" + values.roundoff + "' ";

                            // Add the WHERE clause to update based on invoicedraft_gid
                            msSQL += "WHERE invoicedraft_gid = '" + values.invoice_gid + "'";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        }
                        else
                        {

                            msSQL = " insert into acp_trn_tinvoicedraft (" +
                       " invoicedraft_gid, " +
                       " vendor_gid," +
                       " vendor_address, " +
                       " shipping_address, " +
                       " invoice_refno, " +
                       " user_gid, " +
                       " invoice_date," +
                       " payment_date," +
                       " payment_days," +
                       " termsandconditions," +
                       " systemgenerated_amount, " +
                       " additionalcharges_amount, " +
                       " discount_amount, " +
                       " total_amount, " +
                       " invoice_amount, " +
                       " payment_term," +
                       " delivery_term," +
                       " freight_terms," +
                       " mode_despatch, " +
                       " billing_email, " +
                       " created_date," +
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
                       " buybackorscrap," +
                       " vendorinvoiceref_no," +
                       " branch_gid," +
                       " tax_gid," +
                        " tax_name," +
                       " tax_percentage," +
                       " tax_amount," +
                       " purchase_type," +
                       " round_off" +
                       " ) values (" +
                       "'" + msGetGID + "'," +
                       "'" + values.vendor_companyname + "'," +
                         "'" + values.vendor_details + "',";
                            if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                            {
                                msSQL += "'" + values.shipping_address.Replace("'", "\\\'") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.shipping_address + "', ";
                            }
                            msSQL += "'" + ls_referenceno + "'," +
                            "'" + user_gid + "'," +
                           "'" + invoicedate + "'," +
                           "'" + due_date + "'," +
                           "'" + values.payment_days + "',";
                            if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                            {
                                msSQL += "'" + values.template_content.Replace("'", "") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.template_content + "', ";
                            }
                            msSQL += "'0.00',";
                            if (values.addoncharge == "" || values.addoncharge == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.addoncharge + "',";

                            }
                            if (values.additional_discount == "" || values.additional_discount == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.additional_discount + "',";

                            }
                            if (values.totalamount == "" || values.totalamount == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.totalamount + "',";

                            }
                            if (values.grandtotal == "" || values.grandtotal == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.grandtotal + "',";

                            }

                            msSQL += "'" + values.payment_terms + "'," +
                            "'" + values.delivery_terms + "'," +
                            "'" + values.freight_terms + "'," +
                            "'" + values.dispatch_mode + "'," +
                            "'" + values.billing_mail + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                           "'" + "Invoice Approved" + "'," +
                           "'" + "Payment Pending" + "',";
                            if (!string.IsNullOrEmpty(values.invoice_remarks) && values.invoice_remarks.Contains("'"))
                            {
                                msSQL += "'" + values.invoice_remarks.Replace("'", "\\\'") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.invoice_remarks + "', ";
                            }
                            msSQL += "'" + "Direct Invoice" + "'," +
                           "'0.00'," +
                           "'0.00',";
                            if (values.grandtotal == "" && values.grandtotal == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.grandtotal + "',";

                            }
                            msSQL += "'" + lscurrency_code + "',";
                            if (values.exchange_rate == "" && values.exchange_rate == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.exchange_rate + "',";

                            }

                            if (values.freightcharges == "" && values.freightcharges == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.freightcharges + "',";

                            }

                            msSQL += "'0.00',";
                            msSQL += "'0.00',";
                            msSQL += "'0.00',";
                            msSQL += "'0.00',";


                            if (values.buybackorscrap == "" || values.buybackorscrap == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.buybackorscrap + "',";

                            }
                            msSQL +=
                            "'" + values.vendor_ref_no + "'," +
                            "'" + values.branch_name + "',";
                            msSQL += "'" + values.tax_name4 + "'," +
                            "'" + lstax_name4 + "'," +
                             "'" + lstaxpercentage + "',";

                            if (string.IsNullOrEmpty(values.tax_amount4))
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.tax_amount4 + "',";
                            }
                            msSQL += "'" + values.purchase_type + "'," +
                                     "'" + values.roundoff + "')";
                        }
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                       

                        if (mnResult == 1)
                        {
                            msSQL = " delete from acp_trn_tinvoicedraftdtl where invoicedraft_gid = '" + values.invoice_gid + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "SELECT a.tmpinvoicedtl_gid, a.product_gid,a.qty_invoice, FORMAT(a.qty_ordered, 2) AS qty_ordered," +
                                " a.product_name, a.product_code," +
                             " b.productgroup_gid, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                             "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price," +
                             " a.discount_percentage, a.discount_amount, a.tax_percentage," +
                             " FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field,  a.tax_name," +
                             " a.tax_name2, a.tax_name3, a.tax_percentage2, a.tax_percentage3, " +
                             "FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3," +
                             " a.tax1_gid, a.tax2_gid,a.tax3_gid, a.taxsegment_gid FROM acp_tmp_tinvoice a" +
                             " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                             "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                             " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid" +
                             " WHERE a.created_by= '" + user_gid + "'";
                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    msSQL = "select tax_prefix from acp_mst_ttax where tax_gid ='" + dt["tax2_gid"].ToString() + "' ";
                                    string lstax2 = objdbconn.GetExecuteScalar(msSQL);

                                    if (!string.IsNullOrEmpty(values.invoice_gid))
                                    {
                                        msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                                        msSQL = " insert into acp_trn_tinvoicedraftdtl (" +
                                        " invoicedtldraft_gid, " +
                                        " invoicedraft_gid, " +
                                        " product_gid, " +
                                     " product_code, " +
                                         " product_name, " +
                                        " productuom_name, " +
                                       " uom_gid, " +
                                        " productgroup_name, " +
                                       " producttype_gid, " +
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
                                       " display_field," +
                                       " product_price_L," +
                                       " tax_amount1_L," +
                                       " tax_amount2_L," +
                                       " tax_amount3_L," +
                                       " qty_invoice, " +
                                       " taxsegment_gid" +
                                         " )values ( " +
                                         "'" + msDIGetGID + "', " +
                                         "'" + values.invoice_gid + "'," +
                                         "'" + dt["product_gid"].ToString() + "', " +
                                         "'" + dt["product_code"].ToString() + "', " +
                                         "'" + dt["product_name"].ToString() + "', " +
                                         "'" + dt["productuom_name"].ToString() + "', " +
                                         "'" + dt["uom_gid"].ToString() + "', " +
                                          "'" + dt["productgroup_name"].ToString() + "', " +
                                         "'" + dt["producttype_gid"].ToString() + "', " +
                                         "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["discount_percentage"].ToString() + "', " +
                                         "'" + dt["discount_amount"].ToString() + "', " +
                                         "'" + dt["tax_name"].ToString() + "', " +
                                         "'" + lstax2 + "', " +
                                         "'" + dt["tax_name3"].ToString() + "', " +
                                         "'" + dt["tax_percentage"].ToString() + "', " +
                                         "'" + dt["tax_percentage2"].ToString() + "', " +
                                         "'" + dt["tax_percentage3"].ToString() + "', " +
                                         "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax1_gid"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax2_gid"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax3_gid"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["display_field"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                        "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["taxsegment_gid"].ToString() + "')";

                                    }
                                    else
                                    {
                                        msDIGetGID = objcmnfunctions.GetMasterGID("SIVC");
                                        msSQL = " insert into acp_trn_tinvoicedraftdtl (" +
                                        " invoicedtldraft_gid, " +
                                        " invoicedraft_gid, " +
                                        " product_gid, " +
                                     " product_code, " +
                                         " product_name, " +
                                        " productuom_name, " +
                                       " uom_gid, " +
                                        " productgroup_name, " +
                                       " producttype_gid, " +
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
                                       " display_field," +
                                       " product_price_L," +
                                       " tax_amount1_L," +
                                       " tax_amount2_L," +
                                       " tax_amount3_L," +
                                       " qty_invoice, " +
                                       " taxsegment_gid" +
                                         " )values ( " +
                                         "'" + msDIGetGID + "', " +
                                         "'" + msGetGID + "'," +
                                         "'" + dt["product_gid"].ToString() + "', " +
                                         "'" + dt["product_code"].ToString() + "', " +
                                         "'" + dt["product_name"].ToString() + "', " +
                                         "'" + dt["productuom_name"].ToString() + "', " +
                                         "'" + dt["uom_gid"].ToString() + "', " +
                                          "'" + dt["productgroup_name"].ToString() + "', " +
                                         "'" + dt["producttype_gid"].ToString() + "', " +
                                         "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["discount_percentage"].ToString() + "', " +
                                         "'" + dt["discount_amount"].ToString() + "', " +
                                         "'" + dt["tax_name"].ToString() + "', " +
                                         "'" + lstax2 + "', " +
                                         "'" + dt["tax_name3"].ToString() + "', " +
                                         "'" + dt["tax_percentage"].ToString() + "', " +
                                         "'" + dt["tax_percentage2"].ToString() + "', " +
                                         "'" + dt["tax_percentage3"].ToString() + "', " +
                                         "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax1_gid"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax2_gid"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax3_gid"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["display_field"].ToString().Replace(",", "") + "', " +
                                            "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                        "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                         "'" + dt["taxsegment_gid"].ToString() + "')";
                                    }
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    


                                }

                            }
                            if (mnResult == 1)
                            {


                                msSQL = "  delete from acp_trn_tinvoicedraft where invoicedraft_gid='" + values.invoice_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL = "  delete from acp_trn_tinvoicedraftdtl where invoicedraft_gid='" + values.invoice_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                msSQL = "  delete from acp_tmp_tinvoice where created_by='" + user_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                values.status = true;
                                values.message = "Invoice Submitted Successfully";
                            }

                            
                            else
                            {
                                values.status = false;
                                values.message = "Error While Adding product in Direct Invoice!";
                            }


                            dt_datatable.Dispose();
                        }
                    }




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

        public void DaGetPmrTrnInvoiceDraft(string user_gid, string invoice_gid, MdlPmrTrnDirectInvoice values)
        {
            try
            {

                msSQL = "  delete from acp_tmp_tinvoice where  created_by = '" + user_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select  invoicedtldraft_gid,  invoicedraft_gid, product_gid,  product_code,   product_name, productuom_name,  " +
                    " uom_gid,  productgroup_name,  producttype_gid,  product_price,  product_total,  discount_percentage,  discount_amount,  " +
                    " tax_name,  tax_name2,  tax_name3,  tax_percentage,  tax_percentage2,  tax_percentage3,  tax_amount,  tax_amount2,  tax_amount3," +
                    "  tax1_gid,  tax2_gid,  tax3_gid,created_by,  display_field, product_price_L, tax_amount1_L, tax_amount2_L, tax_amount3_L, qty_invoice," +
                    "  taxsegment_gid from acp_trn_tinvoicedraftdtl where invoicedraft_gid = '" + invoice_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow da in dt_datatable.Rows)
                    {

                        msSQL = " insert into acp_tmp_tinvoice  ( " +
                          " tmpinvoicedtl_gid," +
                          " tmpinvoice_gid," +
                          " qty_invoice," +
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
                           " ) values ( " +
                           "'" + da["invoicedtldraft_gid"].ToString() + "'," +
                           "'" + da["invoicedraft_gid"].ToString() + "'," +
                           "'" + da["qty_invoice"].ToString() + "'," +
                           "'" + da["product_gid"].ToString() + "'," +
                          "'" + da["product_code"].ToString() + "'," +
                          "'" + da["product_name"].ToString() + "'," +
                          "'" + da["display_field"].ToString() + "'," +
                          "'" + da["productuom_name"].ToString() + "'," +
                          "'" + da["uom_gid"].ToString() + "'," +
                          "'" + da["qty_invoice"].ToString() + "'," +
                          "'" + da["product_price"].ToString() + "'," +
                          "'" + da["discount_percentage"].ToString() + "', " +
                          "'" + da["discount_amount"].ToString() + "', " +
                          " '" + da["taxsegment_gid"].ToString() + "', " +
                          " '" + da["tax1_gid"].ToString() + "', " +
                          " '" + da["tax2_gid"].ToString() + "', " +
                          " '" + da["tax3_gid"].ToString() + "', " +
                          " '" + da["tax_name"].ToString() + "', " +
                          " '" + da["tax_name2"].ToString() + "', " +
                          " '" + da["tax_name3"].ToString() + "', " +
                          "'" + da["tax_percentage"].ToString() + "', " +
                          "'" + da["tax_percentage2"].ToString() + "', " +
                          "'" + da["tax_percentage3"].ToString() + "', " +
                          "'" + da["tax_amount"].ToString() + "', " +
                          "'" + da["tax_amount2"].ToString() + "', " +
                          "'" + da["tax_amount3"].ToString() + "', " +
                          "'" + user_gid + "', " +
                          "'" + da["product_total"].ToString() + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                    }

                }


                if (mnResult == 1)
                {
                    values.status = true;


                }

                msSQL = " SELECT a.purchase_type,x.currencyexchange_gid,a.invoicedraft_gid,e.tax1_gid,e.tax_name as tax_name4,k.email_id,a.mode_despatch,a.billing_email,a.shipping_address,a.termsandconditions,a.invoice_refno,a.invoice_type,concat(m.address1,'\n',m.address2,'\n',m.city,'\n',m.state,'\n',m.postal_code" +
                        " ) as vendor_address,a.vendorinvoiceref_no,k.contact_telephonenumber,a.order_total,a.received_amount,a.received_year,k.contactperson_name," +
                        " j.branch_name,k.vendor_companyname,a.branch_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, " +
                        " date_format(a.payment_date,'%d-%m-%Y') as payment_date,a.round_off, a.payment_term,a.delivery_term, a.vendor_gid,a.extraadditional_amount as extraadditional_amount, " +
                        " case when a.currency_code is null then 'INR' else a.currency_code end as currency_code,a.extradiscount_amount as extradiscount_amount, " +
                        " case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,e.product_total as price, " +
                        " a.invoice_remarks, a.payment_days,a.reject_reason, a.invoice_status, a.invoice_amount as invoice_amount, a.invoice_reference, " +
                        " a.freightcharges_amount as freightcharges_amount, a.additionalcharges_amount as additionalcharges_amount, " +
                        " a.discount_amount as discount_amount, a.total_amount as total_amount,  " +
                        " a.freightcharges as freightcharges,a.buybackorscrap as buybackorscrap,a.invoice_total,a.raised_amount,a.tax_amount,a.tax_name " +
                        " FROM acp_trn_tinvoicedraft a " +
                        " left join acp_trn_tinvoicedraftdtl e on a.invoicedraft_gid=e.invoicedraft_gid " +               
                        " left join hrm_mst_tbranch j on a.branch_gid=j.branch_gid " +
                        " left join acp_mst_tvendor k on a.vendor_gid=k.vendor_gid " +
                        " left join crm_trn_tcurrencyexchange x on a.currency_code = x.currency_code " +
                        " left join adm_mst_taddress m on m.address_gid=k.address_gid  " +
                        " where a.invoicedraft_gid = '" + invoice_gid + "' group by a.invoicedraft_gid ";


                //dt_datatable = objdbconn.GetDataTable(msSQL);
                //var getModuleList = new List<invoice_listsedit>();
                //if (dt_datatable.Rows.Count != 0)
                //{
                //    foreach (DataRow dt in dt_datatable.Rows)
                //    {
                ds_dataset = objdbconn.GetDataSet(msSQL, "acp_trn_tinvoice");
                var getModuleList = new List<invoice_listsedit>();
                if (ds_dataset.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dt in ds_dataset.Tables[0].Rows)
                    {

                        getModuleList.Add(new invoice_listsedit
                        {
                            invoice_gid = dt["invoicedraft_gid"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            vendorinvoiceref_no = dt["vendorinvoiceref_no"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            invoice_type = dt["invoice_type"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            round_off = dt["round_off"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            price = dt["price"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                            delivery_term = dt["delivery_term"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            tax_name4 = dt["tax_name4"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            tax1_gid = dt["tax1_gid"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            purchase_type = dt["purchase_type"].ToString(),
                        });
                        values.invoice_listsedit = getModuleList;

                    }
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }






    }
}