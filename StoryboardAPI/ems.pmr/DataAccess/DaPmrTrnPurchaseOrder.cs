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
using RestSharp;

namespace ems.pmr.DataAccess
{
    public class DaPmrTrnPurchaseOrder
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string lspop_server, lspop_mail, lspop_password, lscompany, lscompany_code;
        string msINGetGID, msGet_att_Gid, msenquiryloggid;
        string lspath, lspath1, lspath2, mail_path, mail_filepath, pdf_name = "";
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable mail_datatable, dt_datatable;
        DataSet ds_dataset;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, lstaxpercentage, msPOGetGID, msPO1GetGID, msPO2GetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lspop_port;
        MailMessage message = new MailMessage();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string company_logo_path, authorized_sign_path;
        Image branch_logo, company_logo, DataColumn14;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();
        string FileExtensionname;
        string tax_percentage2, tax_percentage1, tax_gid2, tax_gid1, lsproductprice, lsnet_amount, expected_date, po_date, lsflag;
        string lsproductgid, lsproductuom_gid, lsproduct_name, lsproductuom_name, lsproducttype_name, lsproductgroup_name, lsorder_type, qty,cost_price, lsdiscountpercentage, lsdiscountamount, currencyexchange_gid, tax_amount1, tax_amount2, tax_name1, tax_name2,
            tax1, tax2, tax3, tax_amount, mrp_price, taxsegment_name, tax_percentage, tax_gid, tax_name, taxsegment_gid, vendor_code, vendor_companyname, contactperson_name, contact_telephonenumber, vendor_address;
        private List<GetTaxSegmentList> allTaxSegmentsList;
        double productprice, totalqty, totalamount, taxpercentage1, taxamount1, taxpercentage2, taxamount2, producttotal;
        public void DaGetPurchaseOrderSummary(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                msSQL = " select /*+ MAX_EXECUTION_TIME(600000) */ distinct h.costcenter_name,a.purchaseorder_gid, " +
                        " b.vendor_code,concat(b.contactperson_name,' / ',b.email_id,' / ',b.contact_telephonenumber) as Contact,a.file_name," +
                        " concat(b.vendor_code,'/',b.vendor_companyname) as Vendor," +
                        " CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorder_gid ELSE a.poref_no END AS porefno, "+
                        " a.poref_no, a.purchaseorder_remarks,a.purchaseorder_status, format(a.total_amount,2) as total_amount,Date_add(a.purchaseorder_date,Interval delivery_days day) as ExpectedPODeliveryDate ," +
                        " format(a.total_amount,2) as paymentamount,a.file_path, " +
                        " DATE_FORMAT(a.purchaseorder_date , '%d-%m-%Y') as purchaseorder_date ,a.vendor_status," +
                        " CASE when a.invoice_flag <> 'IV Pending' then a.invoice_flag" +
                        " when a.grn_flag <> 'GRN Pending' then a.grn_flag" +
                        " else a.purchaseorder_flag end as 'overall_status'," +
                        " a.purchaseorder_flag, a.grn_flag, a.invoice_flag," +
                        " b.vendor_companyname, c.branch_name, " +
                        " case when group_concat(distinct purchaserequisition_referencenumber)=',' then '' " +
                        " when group_concat(distinct purchaserequisition_referencenumber) <> ',' then group_concat(distinct purchaserequisition_referencenumber) end  as refrence_no, " +
                        " bscc_flag,po_type,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,concat(d.user_firstname,' ',d.user_lastname) as created_by from pmr_trn_tpurchaseorder a" +
                        " left join  acp_mst_tvendor b on b.vendor_gid = a.vendor_gid" +
                        " left join hrm_mst_tbranch c on c.branch_gid = a.branch_gid" +
                        " left join adm_mst_tuser d on d.user_gid = a.created_by" +
                        " left join hrm_mst_temployee e on e.user_gid = d.user_gid" +
                        " left join adm_mst_tmodule2employee  f on f.employee_gid = e.employee_gid" +
                        " left join pmr_mst_tcostcenter h on h.costcenter_gid=a.costcenter_gid" +
                        " left join pmr_Trn_tpr2po i on i.purchaseorder_gid=a.purchaseorder_gid" +
                        " left join pmr_Trn_tpurchaserequisition j on j.purchaserequisition_gid=i.purchaserequisition_gid " +
                        " left join crm_trn_tcurrencyexchange k on k.currencyexchange_gid =a.currency_code" +
                        " where 1=1 and a.workorderpo_flag='N'  and a.purchaseorder_status <>'PO Amended' and a.purchaseorder_flag <> 'PO Canceled' group by a.purchaseorder_gid " +
                        " order by  date(a.purchaseorder_date) desc,a.purchaseorder_date asc, a.purchaseorder_gid desc, b.vendor_companyname desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                msSQL = "select flag from pmr_mst_tpurchaseconfig where id = '21'";
                lsflag = objdbconn.GetExecuteScalar(msSQL);
                var getModuleList = new List<GetPurchaseOrder_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPurchaseOrder_lists
                        {
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            ExpectedPODeliveryDate = dt["ExpectedPODeliveryDate"].ToString(),
                            porefno = dt["porefno"].ToString(),
                            poref_no = dt["poref_no"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            remarks = dt["purchaseorder_remarks"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            purchaseorder_status = dt["purchaseorder_status"].ToString(),
                            vendor_status = dt["vendor_status"].ToString(),
                            paymentamount = dt["paymentamount"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            Vendor = dt["Vendor"].ToString(),
                            Contact = dt["Contact"].ToString(),
                            file_path = dt["file_path"].ToString(),
                            flag = lsflag,
                            file_name = dt["file_name"].ToString(),
                        });
                        values.GetPurchaseOrder_lists = getModuleList;
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
        public void DaGetBranch(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " Select branch_name,branch_gid " +
                " from hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranch>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranch
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.GetBranch = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetVendor(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = "Select vendor_gid,vendor_companyname from acp_mst_tvendor where blacklist_flag <>'Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetVendor>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendor
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                        });
                        values.GetVendor = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetVendorContract(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = "Select vendor_gid,vendor_companyname from pmr_trn_tratecontract ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetVendorContract>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendorContract
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                        });
                        values.GetVendorContract = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetproducttype(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " select producttype_gid, producttype_code , producttype_name from pmr_mst_tproducttype";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getproducttype>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproducttype
                        {
                            producttype_gid = dt["producttype_gid"].ToString(),
                            producttype_code = dt["producttype_code"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                        });
                        values.Getproducttype = getModuleList;
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
        public void DaGetDispatchToBranch(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " Select branch_name,branch_gid " +
        " from hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDispatchToBranch>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDispatchToBranch
                        {
                            dispatch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.GetDispatchToBranch = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting despatch to branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetCurrency(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " select distinct a.currencyexchange_gid,a.currency_code,a.default_currency,a.exchange_rate from  crm_trn_tcurrencyexchange a " +
   " left join acp_mst_tvendor b on a.currencyexchange_gid = b.currencyexchange_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL, "crm_trn_tcurrencyexchange");

                var getModuleList = new List<GetCurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCurrency
                        {
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            default_currency = dt["default_currency"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                        });
                        values.GetCurrency = getModuleList;
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
        public void DaGetTax(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTax>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTax
                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name"].ToString(),
                            tax_name3 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString(),
                        });
                        values.GetTax = getModuleList;
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
        public void DaGetTax4Dtl(MdlPmrTrnPurchaseOrder values)
        {
            try
            {



                msSQL = " SELECT   CASE WHEN tax_name IS NULL OR tax_name = '' THEN tax_prefix ELSE tax_name  END AS tax_name, tax_gid, percentage FROM  acp_mst_ttax WHERE   active_flag = 'Y' and reference_type='Vendor'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxFourDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxFourDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name4 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTax4Dtl = getModuleList;
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
        public void DaGetProductCode(MdlPmrTrnPurchaseOrder values)
        {
            try
            {


                msSQL = " Select product_code,product_gid,product_name" +
                        " from pmr_mst_tproduct ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductCode>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductCode
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                        });
                        values.GetProductCode = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Product code!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetViewPurchaseOrderSummary(string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " select r.renewal_flag,date_format(r.renewal_date, '%d-%m-%y') as renewal_date, i.producttotal_amount, a.shipping_address, concat(a.purchaseorder_gid) as purchaseorder_gid, m.user_firstname as requested_by,  a.mode_despatch, " +
                    " a.requested_details, a.po_covernote, b.gst_no, b.email_id, b.contact_telephonenumber, b.contactperson_name,  a.tax_amount as overalltax , " +
                    " format(i.product_price_L, 2) as product_price_L, a.purchaserequisition_gid, a.purchaseorder_remarks,  " +
                    " case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate, w.address2, i.tax_name, i.tax_name2, i.tax_name3,  a.purchaserequisition_gid, " +
                    " a.quotation_gid, a.branch_gid, a.ship_via, a.payment_terms, a.delivery_location, a.freight_terms,  date_format(a.purchaseorder_date, '%d-%m-%y') as purchaseorder_date, " +
                    " date_format(a.expected_date, '%d-%m-%y') as expected_date,  a.vendor_address, a.vendor_contact_person, a.created_by, a.priority, a.priority_remarks,  " +
                    " case when a.priority = 'Y' then 'High' else 'Low' end as priority_n,  CASE when a.invoice_flag<> 'IV Pending' then a.invoice_flag " +
                    " when a.grn_flag<> 'GRN Pending' then a.grn_flag  else a.purchaseorder_flag end as 'overall_status', a.approver_remarks, a.purchaseorder_reference,  " +
                    " format(a.total_amount, 2) as total_amount, concat(f.address1, f.postal_code) as branch_add1,  concat(w.address1, ' ', w.address2, ' ', w.city, ' ', w.postal_code) AS bill_to, " +
                    " a.vendor_emailid,  a.vendor_faxnumber, a.vendor_contactnumber, a.termsandconditions, a.payment_term, b.vendor_companyname,  g.user_firstname as approved_by, i.qty_ordered as qyt_unit, " +
                    " concat(c.user_firstname, ' - ', e.department_name) as user_firstname,  d.employee_emailid, d.employee_mobileno, f.branch_name, format(a.discount_amount, 2) as discount_amount,  " +
                    " format(a.tax_percentage, 2) as tax_percentage, format(a.addon_amount, 2) as addon_amount,  format(a.roundoff, 2) as roundoff, concat_ws('-', h.costcenter_name, h.costcenter_gid) as costcenter_name," +
                    " format(h.budget_available, 2) as budget_available, h.costcenter_gid, a.payment_days, a.tax_gid,  a.delivery_days, format(a.freightcharges, 2) as freightcharges, a.buybackorscrap, a.manualporef_no," +
                    " format(a.packing_charges, 2) as packing_charges, format(a.insurance_charges, 2) as insurance_charges,  format(a.discount_amount, 2) as additional_discount, i.purchaseorderdtl_gid, i.product_gid, i.product_price," +
                    " i.qty_ordered, i.needby_date, format(i.discount_percentage, 2) as discount_percentage, format(i.discount_amount, 2) discount_amount1,  format(i.tax_percentage, 2) as tax_percentage," +
                    " format(i.tax_percentage2, 2) as tax_percentage2, format(i.tax_percentage3, 2) as tax_percentage3,  format(i.tax_amount, 2) as tax_amount, CASE WHEN i.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(i.tax_amount2, 2) END AS tax_amount2," +
                    " format(i.tax_amount3, 2) as tax_amount3, i.taxseg_taxname1, i.taxseg_taxpercent1, format(i.taxseg_taxamount1, 2) AS taxseg_taxamount1, i.taxseg_taxname2, i.taxseg_taxpercent2, format(i.taxseg_taxamount2, 2) AS taxseg_taxamount2," +
                    " i.qty_Received, i.qty_grnadjusted,  i.taxseg_taxname3, i.taxseg_taxpercent3, format(i.taxseg_taxamount3, 2) as taxseg_taxamount3, i.product_remarks,  format((qty_ordered * i.product_price), 2) as product_totalprice," +
                    " format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2) as product_total,  i.product_code, (i.product_name) as product_name, format(a.netamount, 2) as netamount," +
                    " k.productgroup_name,  i.productuom_name, i.purchaseorder_gid, i.display_field_name, a.currency_code, a.poref_no, i.tax1_gid, y.tax_name as overalltaxname,  " +
                    " (SELECT FORMAT(SUM(tax_amount), 2) FROM pmr_trn_tpurchaseorderdtl where purchaseorder_gid = 'PPOP24093050') AS overall_tax, " +
                    " concat(i.tax_name, ' ,', i.tax_name2) as taxesname, concat(i.tax_amount, ', ', i.tax_amount2) as taxesamt, r.frequency_term, r.renewal_type,  a.file_path,a.file_name " +
                    " from pmr_trn_tpurchaseorder a  left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid  left join adm_mst_taddress w on b.address_gid = w.address_gid  " +
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
                    " left join adm_mst_tuser m on m.user_gid = a.requested_by  " +
                    " left join crm_trn_tcurrencyexchange z on a.currency_code = z.currencyexchange_gid  " +
                    " left join pmr_trn_trenewal r ON a.purchaseorder_gid = r.purchaseorder_gid    " +
                    " where a.purchaseorder_gid = '" + purchaseorder_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetViewPurchaseOrder>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetViewPurchaseOrder
                        {
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            tax_number = dt["gst_no"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            address2 = dt["address2"].ToString(),
                            requested_by = dt["requested_by"].ToString(),
                            requested_details = dt["requested_details"].ToString(),
                            delivery_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            po_covernote = dt["po_covernote"].ToString(),
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
                            overall_tax = dt["overall_tax"].ToString(),
                            overalltax = dt["overalltax"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            bill_to = dt["bill_to"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            po_no = dt["poref_no"].ToString(),
                            producttotal_amount = dt["producttotal_amount"].ToString(),
                            frequency_term = dt["frequency_term"].ToString(),
                            renewal_type = dt["renewal_type"].ToString(),
                            file_path = dt["file_path"].ToString(),
                            file_name = dt["file_name"].ToString(),
                            renewal_flag = dt["renewal_flag"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),





                        });

                        values.GetViewPurchaseOrder = getModuleList;

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

        public void DaGetOnchangeCurrency(string currencyexchange_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
                        " where currencyexchange_gid='" + currencyexchange_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOnchangeCurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnchangeCurrency
                        {

                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.GetOnchangeCurrency = getModuleList;
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



        public void DaGetOnChangeBranch(string branch_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " select branch_name, address1,city,state, postal_code from hrm_mst_tbranch  " +
                        " where branch_gid  ='" + branch_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranch>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranch
                        {
                            branch_name = dt["branch_name"].ToString(),
                            address1 = dt["address1"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            postal_code = dt["postal_code"].ToString(),


                        });
                        values.GetBranch = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while gonchanging branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnChangeVendor(string vendor_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = "   select b.address1, b.address2, b.city, b.state, b.postal_code, b.fax,a.contactperson_name, a.vendor_companyname,a.payment_terms," +
                        " a.contact_telephonenumber,a.gst_no, a.taxsegment_gid,c.country_name,a.email_id,a.currencyexchange_gid from acp_mst_tvendor a " +
                        " left join adm_mst_taddress b on b.address_gid = a.address_gid " +
                        " left join adm_mst_tcountry c on c.country_gid = b.country_gid" +
                        " where a.vendor_gid  ='" + vendor_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetVendor>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendor
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
                        values.GetVendor = getModuleList;
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

        public void DaGetuser(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " SELECT concat(a.user_firstname,' ',a.user_lastname) as employee_name,a.user_gid ,b.employee_gid,c.department_name FROM adm_mst_tuser a " +
                " left join hrm_mst_temployee b on b.user_gid=a.user_gid " +
                " left join hrm_mst_tdepartment c on c.department_gid=b.department_gid " +
                " order by a.user_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getuser>();
                if (dt_datatable.Rows.Count != 0)

                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getuser
                        {
                            user_gid = dt["user_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),

                        });
                        values.Getuser = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured whilegetting user details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetOnChangeProductCode(string product_code, MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                if (product_code != null)
                {
                    msSQL = " Select a.product_name, a.product_code, b.productuom_gid,b.productuom_name,c.productgroup_name  from pmr_mst_tproduct a  " +
                     " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                " where a.product_code='" + product_code + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetProductCode>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetProductCode
                            {
                                product_name = dt["product_name"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),

                            });
                            values.GetProductCode = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  on changing product code!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetOnChangeProductName(string product_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {


                if (product_gid != null)
                {
                    msSQL = " Select a.product_name, a.product_code, a.cost_price,b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                         " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                    " where a.product_gid='" + product_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetProductCode>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetProductCode
                            {
                                product_name = dt["product_name"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productuom_gid = dt["productuom_gid"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),
                                unitprice = dt["cost_price"].ToString()

                            });
                            values.GetProductCode = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }



        public void DaGetProduct(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = " Select product_gid, product_name from pmr_mst_tproduct  " +
                  " order by product_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProduct>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProduct
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                        });
                        values.GetProduct = getModuleList;
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

        public void DaGetOnAdd(string user_gid, productlist values)
        {
            try
            {

                msGetGid = objcmnfunctions.GetMasterGID("PODC");
                string lsrefno = "";
                string lspercentage1, lspercentage2;
                string lstax1, lstax2;
                msSQL = "select product_name from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                string lsproductName = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select branch_gid from hrm_mst_tbranch where branch_name='" + values.branch_name + "'";
                string lsbranchgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                if (string.IsNullOrEmpty(values.tax_name1))
                {
                    lspercentage1 = "0";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name1 + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }

                if (string.IsNullOrEmpty(values.tax_name1))
                {
                    lstax1 = "0";
                }
                else
                {
                    msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + values.tax_name1 + "'";
                    lstax1 = objdbconn.GetExecuteScalar(msSQL);
                }


                {
                    msSQL = " insert into pmr_tmp_tpurchaseorder ( " +
                            " tmppurchaseorderdtl_gid," +
                            " tmppurchaseorder_gid," +
                            " qty_poadjusted," +
                            " product_gid," +
                            " product_code," +
                            " product_name," +
                            " productuom_name," +
                            " uom_gid," +
                            " qty_ordered," +
                            " product_price," +
                            " discount_percentage," +
                            " discount_amount," +
                            " tax_name," +
                            " tax_percentage," +
                            " tax1_gid," +
                            " tax_amount, " +
                             " created_by," +
                            " producttotal_price " +
                            " ) values ( " +
                            "'" + msGetGid + "'," +
                            "''," +
                             "'0.00'," +
                            "'" + values.product_gid + "'," +
                            "'" + values.productcode + "'," +
                            "'" + lsproductName + "'," +
                            "'" + values.productuom_name + "'," +
                            "'" + lsproductuomgid + "'," +
                            "'" + values.quantity + "'," +
                            "'" + values.unitprice + "',";
                    if (string.IsNullOrEmpty(values.discountpercentage))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.discountpercentage + "',";
                    }
                    msSQL += "'" + values.discountamount + "'," +
                            "'" + lstax1 + "'," +
                            "'" + lspercentage1 + "'," +
                            "'" + values.tax_name1 + "',";


                    if (string.IsNullOrEmpty(values.taxamount1))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.taxamount1 + "',";
                    }
                    msSQL += "'" + user_gid + "'," +
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
                values.message = "Exception occured while getting product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
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
        public void DaProductSummary(string user_gid, string vendor_gid, MdlPmrTrnPurchaseOrder values, string smryproduct_gid)
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
                var getModuleList = new List<productsummary_list>();
                var getGetTaxSegmentList = new List<GetTaxSegmentList>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["producttotal_price"].ToString());
                        grandtotal += double.Parse(dt["producttotal_price"].ToString());
                        getModuleList.Add(new productsummary_list
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
                                getGetTaxSegmentList.Add(new GetTaxSegmentList
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
                    values.productsummary_list = getModuleList;
                    values.GetTaxSegmentList = getGetTaxSegmentList;
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

        //Submit
        public void DaProductSubmit(string user_gid, GetViewPurchaseOrder values)
        {
            try
            {
                if (string.IsNullOrEmpty(values.po_no))
                {
                       int lsfreight = 0;
                        int lsinsurance = 0;
                        string uiDateStr = values.po_date;
                        DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string po_date = uiDate.ToString("yyyy-MM-dd");

                        string uiDateStr2 = values.expected_date;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string expected_date = uiDate2.ToString("yyyy-MM-dd");

                        string lstax_gid;

                        msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                    msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP");
                    msPO2GetGID = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", values.branch_name);
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

                        msSQL = " insert into pmr_trn_tpurchaseorder (" +
                             " purchaseorder_gid, " +
                             " purchaserequisition_gid, " +
                             " purchaseorder_reference, " +
                             " purchaseorder_date," +
                             " expected_date," +
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
                             " renewal_flag, " +
                             " taxsegment_gid " +
                             " ) values (" +
                             "'" + msPO1GetGID + "'," +
                             "'" + msGetGID + "'," +
                             "'" + msPO2GetGID + "'," +
                             "'" + po_date + "', " +
                             "'" + expected_date + "', " +
                             "'" + values.branch_name + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'" + values.vendor_companyname + "',";

                        //"'" + values.address1.Trim().Replace("'", "") + "',";

                        if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                        {
                            msSQL += "'" + values.address1.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.address1 + "', ";
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
                    msSQL += "'" + values.employee_name + "',";

                    if (!string.IsNullOrEmpty(values.delivery_terms) && values.delivery_terms.Contains("'"))
                    {
                        msSQL += "'" + values.delivery_terms.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.delivery_terms + "', ";
                    }
                    if (!string.IsNullOrEmpty(values.payment_terms) && values.payment_terms.Contains("'"))
                    {
                        msSQL += "'" + values.payment_terms.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.payment_terms + "', ";
                    }

                    msSQL += "'" + values.Requestor_details + "',";

                    if (!string.IsNullOrEmpty(values.dispatch_mode) && values.dispatch_mode.Contains("'"))
                    {
                        msSQL += "'" + values.dispatch_mode.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.dispatch_mode + "', ";
                    }
                    msSQL += "'" + lscurrency_code + "'," +
                              "'" + values.exchange_rate + "',";
                    if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                    {
                        msSQL += "'" + values.po_covernote.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.po_covernote + "', ";
                    }
                    if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                    {
                        msSQL += "'" + values.po_covernote.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.po_covernote + "', ";
                    }
                    msSQL += "'" + values.grandtotal + "',";
                        
                        if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                        {
                            msSQL += "'" + values.template_content.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.template_content + "', ";
                        }
                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "'PO Approved',";
                        }
                        else
                        {
                            msSQL += "'Operation Pending PO',";
                        }
                    msSQL += "'" + "PO Approved" + "',";
                   
                    if (values.po_no == "")
                    {
                        //po_no = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", values.branch_name);
                        msSQL += "'"+ msPO2GetGID + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.po_no + "',";
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
                    msSQL += "'" + values.renewal_mode + "',";
                    msSQL += "'" + values.taxsegment_gid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (values.renewal_mode == "Y")
                    {
                        string uiDateStr3 = values.renewal_date;
                        DateTime uiDate4 = DateTime.ParseExact(uiDateStr3, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        string renewal_date = uiDate4.ToString("yyyy-MM-dd");

                        msINGetGID = objcmnfunctions.GetMasterGID("BRLP");

                        msSQL = " Insert into pmr_trn_trenewal ( " +
                                " renewal_gid, " +
                                " renewal_flag, " +
                                " frequency_term, " +
                                " vendor_gid," +
                                " renewal_date, " +
                                " purchaseorder_gid, " +
                                " created_by, " +
                                " renewal_type, " +
                                " created_date) " +
                               " Values ( " +
                                 "'" + msINGetGID + "'," +
                                 "'" + values.renewal_mode + "'," +
                                 "'" + values.frequency_terms + "'," +
                                 "'" + values.vendor_companyname + "'," +
                                 "'" + renewal_date + "'," +
                                 "'" + msPO1GetGID + "'," +
                                 "'" + user_gid + "'," +
                                 "'Purchase'," +
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
                                    msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
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
                                                "'" + msPOGetGID + "', " +
                                                "'" + msPO1GetGID + "'," +
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

                            if (mnResult != 0)

                            {
                                values.status = true;
                                values.message = "Purchase Order Raised Successfully!";
                            if (!string.IsNullOrEmpty(values.purchaseorder_gid))
                            {
                                msSQL = "  delete from pmr_trn_tpurchaseorderdraft where purchaseorderdraft_gid='" +values.purchaseorder_gid + "'  ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL = "  delete from pmr_trn_tpurchaseorderdraftdtl where purchaseorderdraft_gid='" + values.purchaseorder_gid + "'  ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            else
                            {

                            }


                        }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Adding product in Purchase Order!";


                            }

                            msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            dt_datatable.Dispose();
                        }
                    }

                
                else {

                    msSQL = "select poref_no from pmr_trn_tpurchaseorder where poref_no ='" + values.po_no + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        values.message = "PO Ref No Alredy Exist!";
                        return;
                    }
                    else
                    {
                        int lsfreight = 0;
                        int lsinsurance = 0;
                        string uiDateStr = values.po_date;
                        DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string po_date = uiDate.ToString("yyyy-MM-dd");

                        string uiDateStr2 = values.expected_date;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string expected_date = uiDate2.ToString("yyyy-MM-dd");

                        string lstax_gid;

                        msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                        msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP");
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

                        msSQL = " insert into pmr_trn_tpurchaseorder (" +
                             " purchaseorder_gid, " +
                             " purchaserequisition_gid, " +
                             " purchaseorder_reference, " +
                             " purchaseorder_date," +
                             " expected_date," +
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
                              " renewal_flag, " +
                             " taxsegment_gid " +
                             " ) values (" +
                             "'" + msPO1GetGID + "'," +
                             "'" + msGetGID + "'," +
                             "'" + msPO1GetGID + "'," +
                             "'" + po_date + "', " +
                             "'" + expected_date + "', " +
                             "'" + values.branch_name + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'" + values.vendor_companyname + "',";

                        //"'" + values.address1.Trim().Replace("'", "") + "',";

                        if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                        {
                            msSQL += "'" + values.address1.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.address1 + "', ";
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
                        msSQL += "'" + values.employee_name + "'," +
                              "'" + values.delivery_terms + "'," +
                              "'" + values.payment_terms + "'," +
                              "'" + values.Requestor_details + "'," +
                              "'" + values.dispatch_mode + "'," +
                              "'" + lscurrency_code + "'," +
                              "'" + values.exchange_rate + "'," +
                              "'" + values.po_covernote + "'," +
                              "'" + values.po_covernote + "'," +
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
                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "'PO Approved',";
                        }
                        else
                        {
                            msSQL += "'Operation Pending PO',";
                        }
                        msSQL += "'" + "PO Approved" + "'," +
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
                        msSQL += "'" + values.renewal_mode + "',";
                        msSQL += "'" + values.taxsegment_gid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (values.renewal_mode == "Y")
                        {
                            string uiDateStr3 = values.renewal_date;
                            DateTime uiDate4 = DateTime.ParseExact(uiDateStr3, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string renewal_date = uiDate4.ToString("yyyy-MM-dd");

                            msINGetGID = objcmnfunctions.GetMasterGID("BRLP");

                            msSQL = " Insert into pmr_trn_trenewal ( " +
                                    " renewal_gid, " +
                                    " renewal_flag, " +
                                    " frequency_term, " +
                                    " vendor_gid," +
                                    " renewal_date, " +
                                    " purchaseorder_gid, " +
                                    " created_by, " +
                                    " renewal_type, " +
                                    " created_date) " +
                                   " Values ( " +
                                     "'" + msINGetGID + "'," +
                                     "'" + values.renewal_mode + "'," +
                                     "'" + values.frequency_terms + "'," +
                                     "'" + values.vendor_companyname + "'," +
                                     "'" + renewal_date + "'," +
                                     "'" + msPO1GetGID + "'," +
                                     "'" + user_gid + "'," +
                                     "'Purchase'," +
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
                                    msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
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
                                                "'" + msPOGetGID + "', " +
                                                "'" + msPO1GetGID + "'," +
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

                            if (mnResult != 0)

                            {
                                values.status = true;
                                values.message = "Purchase Order Raised Successfully!";

                                if (!string.IsNullOrEmpty(values.purchaseorder_gid))
                                {
                                    msSQL = "  delete from pmr_trn_tpurchaseorderdraft where purchaseorderdraft_gid='" + values.purchaseorder_gid + "'  ";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    msSQL = "  delete from pmr_trn_tpurchaseorderdraftdtl where purchaseorderdraft_gid='" + values.purchaseorder_gid + "'  ";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                }
                                else
                                {

                                }

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Adding product in Purchase Order!";


                            }

                            msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
        public void DaDeleteProductSummary(string tmppurchaseorderdtl_gid, productlist values)
        {
            try
            {

                msSQL = "  delete from pmr_tmp_tpurchaseorder where tmppurchaseorderdtl_gid='" + tmppurchaseorderdtl_gid + "'  ";
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
        //mailfunction

        public void DaMaillId(string employee_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = "select employee_emailid from hrm_mst_temployee where employee_gid = '" + employee_gid + "' union select pop_username from adm_mst_tcompany";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMailId_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetMailId_list
                        {
                            employee_emailid = dt["employee_emailid"].ToString(),

                        });
                        values.GetMailId_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Mail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetTemplatelist(MdlPmrTrnPurchaseOrder values)
        {
            try
            {


                msSQL = " select a.template_gid, c.template_name, c.template_content from adm_trn_ttemplate2module a " +
                 " left join adm_mst_tmodule b on a.module_gid = b.module_gid " +
                 " left join adm_mst_ttemplate c on a.template_gid = c.template_gid " +
                 " left join adm_mst_ttemplatetype d on c.templatetype_gid = d.templatetype_gid " +
                 " where a.module_gid = 'MKT' and c.templatetype_gid='2' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<templatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new templatelist
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_content = dt["template_content"].ToString(),
                        });
                        values.templatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Template!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetTemplatet(string template_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {


                msSQL = " select a.template_gid, c.template_name, c.template_content from adm_trn_ttemplate2module a " +
                 " left join adm_mst_tmodule b on a.module_gid = b.module_gid " +
                 " left join adm_mst_ttemplate c on a.template_gid = c.template_gid " +
                 " left join adm_mst_ttemplatetype d on c.templatetype_gid = d.templatetype_gid " +
                 " where a.module_gid = 'MKT' and c.template_gid='" + template_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<templatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new templatelist
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_content = dt["template_content"].ToString(),
                        });
                        values.templatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Template!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostMail(HttpRequest httpRequest, string user_gid, result objResult)
        {
            {

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // attachment get function

                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gif = string.Empty;
                MemoryStream ms_stream = new MemoryStream();

                //split function

                string employee_emailid = httpRequest.Form["employee_emailid"];
                string sub = httpRequest.Form["sub"];
                string to = httpRequest.Form["to"];
                string body = httpRequest.Form["body"];
                string bcc = httpRequest.Form["bcc"];
                string cc = httpRequest.Form["cc"];

                HttpPostedFile httpPostedFile;

                // save path

                string lsPath = string.Empty;
                lsPath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Mail/Post/Purchase/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                {
                    if ((!System.IO.Directory.Exists(lsPath)))
                        System.IO.Directory.CreateDirectory(lsPath);
                }
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        string file_path = string.Empty;
                        httpFileCollection = httpRequest.Files;
                        for (int i = 0; i < httpFileCollection.Count; i++)
                        {
                            string document_gid = objcmnfunctions.GetMasterGID("UPLF");
                            httpPostedFile = httpFileCollection[i];
                            string FileExtension = httpPostedFile.FileName;
                            pdf_name = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);
                            string lsfilepath_gid = document_gid;
                            FileExtension = Path.GetExtension(FileExtension).ToLower();
                            string lsfilepaths_gid = lsfilepath_gid + FileExtension;
                            Stream ls_stream;
                            ls_stream = httpPostedFile.InputStream;
                            ls_stream.CopyTo(ms_stream);

                            //lspath = "/erp_documents" + "/" + lscompany_code + "/" + "mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            string return_path, attachement_path;
                            // take last 4 digit

                            string last_4_digit = pdf_name + lsfilepath_gid;
                            string get_last_4_digit = objcmnfunctions.ExtractLast4Digits(last_4_digit);

                            lspath1 = "erp_documents" + "/" + lscompany_code + "/" + "Mail/Post/Purchase/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsfilepath_gid + FileExtension;
                            lspath2 = "erp_documents" + "/" + lscompany_code + "/" + "Mail/Post/Purchase/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + pdf_name + "-" + get_last_4_digit + FileExtension;
                            mail_path = lspath1;
                            attachement_path = lspath2;

                            // upload file
                            return_path = objcmnfunctions.uploadFile(lsPath + pdf_name + "-" + get_last_4_digit, FileExtension);
                            ms_stream.Close();

                            // Get file attachment from path

                            //mail_filepath =  System.IO.Path.GetFileName(document_gid);
                            msGet_att_Gid = objcmnfunctions.GetMasterGID("BEAC");
                            msenquiryloggid = objcmnfunctions.GetMasterGID("BELP");
                            msSQL = " insert into acc_trn_temailattachments (" +
                                     " emailattachment_gid, " +
                                     " email_gid, " +
                                     " attachment_systemname, " +
                                     " attachment_path, " +
                                     " inbuild_attachment, " +
                                     " attachment_type " +
                                     " ) values ( " +
                                     "'" + msGet_att_Gid + "'," +
                                     "'" + msenquiryloggid + "'," +
                                     "'" + pdf_name + "'," +
                                     "'" + attachement_path + "', " +
                                     "'" + mail_path + "', " +
                                     "'" + FileExtension + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }

                }
                catch (Exception errormessege)
                {

                }

                msSQL = " select attachment_path as document_path from acc_trn_temailattachments where email_gid='" + msenquiryloggid + "'";
                mail_datatable = objdbconn.GetDataTable(msSQL);

                string result_values = objcmnfunctions.send_mailSMTP(employee_emailid, to, sub, body, cc, bcc, mail_datatable);

                //  message of mail

                // mail send 
                if (result_values == "Send")
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
        //download withoutprice PDF
        public Dictionary<string, object> DaGetPurchaserwithoutpricepdf(string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();

            msSQL = "select a.purchaseorder_gid,k.currency_symbol,a.addon_amount,a.purchaseorder_remarks, a.purchaserequisition_gid,a.currency_code,CONCAT(cast(a.payment_days as char(20)),' Days') as payment_day ,a.tax_amount,a.ship_via,a.freight_terms,a.delivery_location,a.payment_terms, " +
               " date_format(a.purchaseorder_date,'%d/%m/%Y') as purchaseorder_date,n.tax_name, h.gst_no as companygst_no,date_format(Date_add(purchaseorder_date,Interval a.delivery_days day),'%d-%m-%y') as ExpDate," +
               " a.vendor_address, a.vendor_contact_person, a.created_by as user_gid, " +
               " a.purchaseorder_reference,format(a.total_amount,2) as total_amount , " +
               " a.vendor_faxnumber as fax, a.vendor_contactnumber as contact_telephonenumber,h.contact_number as DataColumn30,h.email as DataColumn31," +
               " a.termsandconditions,a.purchaseorder_reference,b.pan_number,h.address1 as DataColumn26,h.city as DataColumn27,h.state as DataColumn28,h.postal_code as DataColumn29, " +
               " CASE when a.quote_referenceno = '--Select--' then '' " +
               " else a.quote_referenceno end as 'quotation_ref', " +
               " b.vendor_companyname,a.vendor_emailid as email_id," +
               " a.freightcharges as freight_charges,format(a.buybackorscrap,2) as buyback, " +
               " format(a.packing_charges,2)as packing_charges ,format(a.insurance_charges,2)as insurance_charges," +
               "(a.total_amount- a.discount_amount + a.tax_amount) as total_amount1, " +
               " concat(c.user_firstname,' - ',e.department_name) as user_firstname,p.salesperson_gid,q.slno,(r.user_firstname)as salesperson_name,a.roundoff, " +
               " d.employee_emailid as user_email, d.employee_phoneno as user_phone , f.city , f.state , f.postal_code , g.country_name, " +
               " h.branch_name, h.branch_header, b.ifsc_code as ecc_no, b.rtgs_code as tngst_no, b.cst_number as cst, " +
               " b.tin_number as tin_no, (h.branch_location) as branch_footer,a.discount_percentage,a.discount_amount as discount_amount1,a.shipping_address, " +
               " i.costcenter_code as cost_center,i.costcenter_name,j.quotation_referenceno,date_format(l.purchaserequisition_date,'%d-%m-%Y')as purchaserequisition_date,b.gst_number from pmr_trn_tpurchaseorder a " +
               " left join pmr_mst_tcostcenter i on i.costcenter_gid = a.costcenter_gid " +
               " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
               " left join adm_mst_tuser c on c.user_gid = a.created_by " +
               " left join hrm_mst_temployee d on d.user_gid = c.user_gid " +
               " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
               " left join adm_mst_taddress f on  f.address_gid = b.address_gid " +
               " left join adm_mst_tcountry g on g.country_gid = f.country_gid " +
               " left join hrm_mst_tbranch h on a.branch_gid = h.branch_gid " +
               " left join crm_trn_tcurrencyexchange k on g.country_gid=k.country_gid " +
               " left join pmr_trn_tquotationvendordetails j on a.quotation_gid=j.quotation_gid" +
               " left join pmr_trn_tpurchaserequisition l on a.purchaserequisition_gid=l.purchaserequisition_gid" +
               " left join acp_mst_ttax n on a.tax_gid = n.tax_gid " +
               " left join pmr_trn_tsalesorder2purchaseorder p on a.purchaseorder_gid=p.purchaseorder_gid " +
               " left join smr_trn_tsalesorderdtl q on p.salesorder_gid=q.salesorder_gid " +
               " left join adm_mst_tuser r on p.salesperson_gid=r.user_gid " +
               " WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
               " group by a.purchaseorder_gid";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");



            currency number2words = fnconvertnumbertowords(purchaseorder_gid, "PO_REPORT");

            msSQL = " SELECT a.product_gid, b.hsn_number, a.purchaseorderdtl_gid,  a.qty_ordered, a.discount_percentage,  CONCAT('" + number2words.symbol + " ', FORMAT(a.discount_amount, 2)COLLATE utf8mb4_general_ci) AS discount_amount," +
                    " a.tax_name, a.tax_amount, a.tax_name2,a.tax_amount2, a.tax_name3,a.producttotal_amount, a.tax_amount3, a.tax_percentage, a.tax_percentage2, a.tax_percentage3,  e.addon_amount, a.product_remarks,  a.needby_date, " +
                    "  format(a. product_price,2) as product_price,concat(a.tax_name,'\n',a.tax_name2) as DataColumn41,concat(a.tax_amount,'\n',a.tax_amount2) as DataColumn39 , " +
                    " ROUND(((a.qty_ordered * a.product_price )+( a.tax_amount2 + a.tax_amount) - a.discount_amount), 2) AS product_total,  " +
                    " ROUND(SUM(((a.qty_ordered * a.product_price) + (a.tax_amount2 + a.tax_amount) - a.discount_amount)), 2) AS DataColumn44 , " +
                    " CASE  WHEN a.tax_amount2 = 0 AND tax_amount3 = 0 THEN CONCAT(a.tax_name, '" + number2words.symbol + " ', a.tax_amount COLLATE utf8mb4_general_ci)\n " +
                    " WHEN a.tax_amount = 0 AND tax_amount2 = 0 THEN  " +
                    " CONCAT(a.tax_name3, '" + number2words.symbol + " ', a.tax_amount COLLATE utf8mb4_general_ci) ELSE   CONCAT(a.tax_name, '" + number2words.symbol + " ', " +
                    " a.tax_amount COLLATE utf8mb4_general_ci, '\n', a.tax_name2, '" + number2words.symbol + " ', a.tax_amount2   " +
                    " COLLATE utf8mb4_general_ci) END AS DataColumn36, e.payment_terms AS DataColumn37,CASE WHEN b.customerproduct_code IS NOT NULL AND b.customerproduct_code != '' THEN b.customerproduct_code ELSE b.product_code END AS product_code,a.product_name,a.productuom_name " +
                    " FROM pmr_trn_tpurchaseorderdtl a " +
                    " LEFT JOIN pmr_trn_tpurchaseorder e ON e.purchaseorder_gid = a.purchaseorder_gid " +
                    " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
                    " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid " +
                    " WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
                    " GROUP BY a.purchaseorderdtl_gid";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");



            //currency number2words = fnconvertnumbertowords(purchaseorder_gid, "PO_REPORT");

            msSQL = "SELECT e.purchaseorder_date, e.delivery_days,sum(a.producttotal_amount) as DataColumn40,  " +
                        " CONCAT('" + number2words.symbol + " ',format(e.total_amount, 2)COLLATE utf8mb4_general_ci) as total_amount, " +
                        " FORMAT(e.discount_amount,2) AS dis_amount,CONCAT('" + number2words.symbol + " ',Format(e.netamount,2)COLLATE utf8mb4_general_ci) as DataColumn11, " +
                        " ('" + number2words.number2words + "') AS DataColumn10, " +
                        "purchaseorder_reference, e.currency_code,f.branch_name as DataColumn12,f.address1 as DataColumn13 ,f.city as DataColumn14 ," +
                        "f.gst_no as DataColumn15, f.state as DataColumn16, f.postal_code, f.contact_number as DataColumn17 , f.email as DataColumn18, f.email_id, " +
                         " format(sum((a.tax_amount + a.tax_amount2 + a.tax_amount3)),2) as DataColumn19, " +
                         " format(sum((a.qty_ordered*a.product_price) - a.discount_amount),2) as DataColumn20, " +
                          " CONCAT('" + number2words.symbol + " ', (CAST(SUM(CASE WHEN a.tax_name LIKE '%ZERO VAT%' THEN  (a.qty_ordered * a.product_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn21," +
                         " CONCAT('" + number2words.symbol + " ', (CAST(SUM(CASE WHEN a.tax_name LIKE '%ZERO VAT%' THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn22," +
                         " CONCAT('" + number2words.symbol + " ', (CAST(SUM(CASE WHEN a.tax_name LIKE '%VAT%' AND a.tax_name NOT LIKE '%ZERO VAT%' THEN  (a.qty_ordered * a.product_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn23," +
                         " CONCAT('" + number2words.symbol + " ', (CAST(SUM(CASE WHEN a.tax_name LIKE '%VAT%' AND a.tax_name NOT LIKE '%ZERO VAT%' THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn24 " +
                        " FROM pmr_trn_tpurchaseorderdtl a " +
                        "LEFT JOIN pmr_trn_tpurchaseorder e ON e.purchaseorder_gid = a.purchaseorder_gid " +
                        " left join hrm_mst_tbranch f on f.branch_gid = e.branch_gid " +
                        "LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                        "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
                        "LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid " +
                        "WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
                        "GROUP BY a.purchaseorder_gid";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable3");

            msSQL = "SELECT a.address1, a.branch_name,a.branch_logo_path as branch_logo, a.authorized_sign_path as DataColumn14,a.city, a.gst_no, a.state, a.postal_code, a.contact_number, a.email, a.email_id, a.branch_gid, a.branch_logo, a.tin_number, a.cst_number,b.termsandconditions as DataColumn14 " +
               "FROM hrm_mst_tbranch a " +
               "LEFT JOIN pmr_trn_tpurchaseorder b ON b.branch_gid = a.branch_gid " +
               "WHERE b.purchaseorder_gid='" + purchaseorder_gid + "'";

            dt1 = objdbconn.GetDataTable(msSQL);
            DataTable4.Columns.Add("branch_logo", typeof(byte[]));
            DataTable4.Columns.Add("DataColumn14", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["branch_logo"].ToString().Replace("../../", ""));
                    authorized_sign_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["DataColumn14"].ToString().Replace("../../", ""));

                    if (System.IO.File.Exists(company_logo_path) && System.IO.File.Exists(authorized_sign_path))
                    {
                        //Convert  Image Path to Byte
                        branch_logo = System.Drawing.Image.FromFile(company_logo_path);
                        DataColumn14 = System.Drawing.Image.FromFile(authorized_sign_path);
                        byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                        byte[] DataColumn14_bytes = (byte[])(new ImageConverter()).ConvertTo(DataColumn14, typeof(byte[]));

                        DataRow newRow = DataTable4.NewRow();
                        newRow["branch_logo"] = branch_logo_bytes;
                        newRow["DataColumn14"] = DataColumn14_bytes;
                        DataTable4.Rows.Add(newRow);
                    }
                }
            }




            dt1.Dispose();
            DataTable4.TableName = "DataTable4";
            myDS.Tables.Add(DataTable4);

            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_pmr"].ToString(), "PmrTrnwithoutprice.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Purchase Order_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();

            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;


        }
            //download Report files
            public Dictionary<string, object> DaGetPurchaseOrderRpt(string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {
            msSQL = "select company_code from adm_mst_tcompany";
            string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            if (lscompany_code == "BOBA" || lscompany_code == "boba")
            {
                OdbcConnection myConnection = new OdbcConnection();
                myConnection.ConnectionString = objdbconn.GetConnectionString();
                OdbcCommand MyCommand = new OdbcCommand();
                MyCommand.Connection = myConnection;
                DataSet myDS = new DataSet();
                OdbcDataAdapter MyDA = new OdbcDataAdapter();

                msSQL = "select a.purchaseorder_gid,k.currency_symbol,a.purchaseorder_remarks, a.purchaserequisition_gid,a.currency_code,CONCAT(cast(a.payment_days as char(20)),' Days') as payment_day ,a.tax_amount,a.ship_via,a.freight_terms,a.delivery_location,a.payment_terms, " +
                    " CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorder_gid ELSE a.poref_no END AS purchaseorder_reference," +
                   " date_format(a.purchaseorder_date,'%d/%m/%Y') as purchaseorder_date,n.tax_name, h.gst_no as companygst_no,date_format(Date_add(purchaseorder_date,Interval a.delivery_days day),'%d-%m-%y') as ExpDate," +
                   " a.vendor_address, a.vendor_contact_person, a.created_by as user_gid, " +
                   " format(a.total_amount,2) as total_amount , " +
                   " a.vendor_faxnumber as fax, a.vendor_contactnumber as contact_telephonenumber," +
                   " a.termsandconditions,b.pan_number, " +
                   " CASE when a.quote_referenceno = '--Select--' then '' " +
                   " else a.quote_referenceno end as 'quotation_ref', " +
                   " b.vendor_companyname,a.vendor_emailid as email_id," +
                   " format(a.freightcharges+a.freighttax_amount,2) as freight_charges,format(a.buybackorscrap,2) as buyback, " +
                   " format(a.packing_charges,2)as packing_charges ,format(a.insurance_charges,2)as insurance_charges," +
                   "(a.total_amount- a.discount_amount + a.tax_amount) as total_amount1, " +
                   " concat(c.user_firstname,' - ',e.department_name) as user_firstname,p.salesperson_gid,q.slno,(r.user_firstname)as salesperson_name,a.roundoff, " +
                   " d.employee_emailid as user_email, d.employee_phoneno as user_phone , f.city , f.state , f.postal_code , g.country_name, " +
                   " h.branch_name, h.branch_header, b.ifsc_code as ecc_no, b.rtgs_code as tngst_no, b.cst_number as cst, " +
                   " b.tin_number as tin_no, (h.branch_location) as branch_footer,a.discount_percentage,a.discount_amount as discount_amount1,a.shipping_address, " +
                   " i.costcenter_code as cost_center,i.costcenter_name,j.quotation_referenceno,date_format(l.purchaserequisition_date,'%d-%m-%Y')as purchaserequisition_date,b.gst_number from pmr_trn_tpurchaseorder a " +
                   " left join pmr_mst_tcostcenter i on i.costcenter_gid = a.costcenter_gid " +
                   " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                   " left join adm_mst_tuser c on c.user_gid = a.created_by " +
                   " left join hrm_mst_temployee d on d.user_gid = c.user_gid " +
                   " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
                   " left join adm_mst_taddress f on  f.address_gid = b.address_gid " +
                   " left join adm_mst_tcountry g on g.country_gid = f.country_gid " +
                   " left join hrm_mst_tbranch h on a.branch_gid = h.branch_gid " +
                   " left join crm_trn_tcurrencyexchange k on g.country_gid=k.country_gid " +
                   " left join pmr_trn_tquotationvendordetails j on a.quotation_gid=j.quotation_gid" +
                   " left join pmr_trn_tpurchaserequisition l on a.purchaserequisition_gid=l.purchaserequisition_gid" +
                   " left join acp_mst_ttax n on a.tax_gid = n.tax_gid " +
                   " left join pmr_trn_tsalesorder2purchaseorder p on a.purchaseorder_gid=p.purchaseorder_gid " +
                   " left join smr_trn_tsalesorderdtl q on p.salesorder_gid=q.salesorder_gid " +
                   " left join adm_mst_tuser r on p.salesperson_gid=r.user_gid " +
                   " WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
                   " group by a.purchaseorder_gid";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable1");



                currency number2words = fnconvertnumbertowords(purchaseorder_gid, "PO_REPORT");

                msSQL = " SELECT a.product_gid, b.hsn_number, a.purchaseorderdtl_gid,  a.qty_ordered, a.discount_percentage,  CONCAT('" + number2words.symbol + " ', FORMAT(a.discount_amount, 2)COLLATE utf8mb4_general_ci) AS discount_amount," +
                        " a.tax_name, a.tax_amount, a.tax_name2,a.tax_amount2, a.tax_name3, a.tax_amount3, a.tax_percentage, a.tax_percentage2, a.tax_percentage3,  e.addon_amount, a.product_remarks,  a.needby_date, " +
                        "  format(a. product_price,2) as product_price, " +
                        " format(((a.qty_ordered*a.product_price)- a.discount_amount),2) as product_total,  " +
                        " CASE  WHEN a.tax_amount2 = 0 AND tax_amount3 = 0 THEN CONCAT(a.tax_name, '" + number2words.symbol + " ', a.tax_amount COLLATE utf8mb4_general_ci)\n " +
                        " WHEN a.tax_amount = 0 AND tax_amount2 = 0 THEN  " +
                        " CONCAT(a.tax_name3, '" + number2words.symbol + " ', a.tax_amount COLLATE utf8mb4_general_ci) ELSE   CONCAT(a.tax_name, '" + number2words.symbol + " ', " +
                        " a.tax_amount COLLATE utf8mb4_general_ci, '\n', a.tax_name2, '" + number2words.symbol + " ', a.tax_amount2   " +
                        " COLLATE utf8mb4_general_ci) END AS DataColumn36, e.payment_terms AS DataColumn37,b.customerproduct_code as product_code,a.product_name,a.productuom_name " +
                        " FROM pmr_trn_tpurchaseorderdtl a " +
                        " LEFT JOIN pmr_trn_tpurchaseorder e ON e.purchaseorder_gid = a.purchaseorder_gid " +
                        " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                        " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
                        " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid " +
                        " WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
                        " GROUP BY a.purchaseorderdtl_gid";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable2");



                //currency number2words = fnconvertnumbertowords(purchaseorder_gid, "PO_REPORT");

                msSQL = "SELECT e.purchaseorder_date, e.delivery_days,f.address1 as DataColumn40,f.city as DataColumn39,f.state  as DataColumn38,f.postal_code as DataColumn37, " +
                            " CONCAT('" + number2words.symbol + " ',format(e.total_amount, 2)COLLATE utf8mb4_general_ci) as total_amount, " +
                            " FORMAT(e.discount_amount,2) AS dis_amount,CONCAT('" + number2words.symbol + " ',Format(e.netamount,2)COLLATE utf8mb4_general_ci) as DataColumn11, " +
                            " ('" + number2words.number2words + "') AS DataColumn10, " +
                            "purchaseorder_reference, e.currency_code,f.branch_name as DataColumn12,f.address1 as DataColumn13 ,f.city as DataColumn14 ," +
                            "f.gst_no as DataColumn15, f.state as DataColumn16, f.contact_number as DataColumn17 , f.email as DataColumn18, f.email_id, " +
                             " format(sum((a.tax_amount + a.tax_amount2 + a.tax_amount3)),2) as DataColumn19, " +
                             " format(sum((a.qty_ordered*a.product_price) - a.discount_amount),2) as DataColumn20, " +
                             " CONCAT('" + number2words.symbol + " ', CAST(SUM(CASE WHEN a.tax_name IN ('ZERO VAT', 'VAT 0%') THEN (a.qty_ordered * a.product_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2))) AS DataColumn21, "+
                             " CONCAT(' " + number2words.symbol + "', CAST(SUM(CASE WHEN a.tax_name IN('ZERO VAT', 'VAT 0%') THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2))) AS DataColumn22,"+
                             " CONCAT('" + number2words.symbol + " ', CAST(SUM(CASE WHEN a.tax_name IN('VAT 20%') AND a.tax_name NOT LIKE '%ZERO VAT%' THEN(a.qty_ordered * a.product_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2))) AS DataColumn23,"+
                             " CONCAT('" + number2words.symbol + "', CAST(SUM(CASE WHEN a.tax_name IN('VAT 20%') AND a.tax_name NOT LIKE '%ZERO VAT%' THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2))) AS DataColumn24 "+
                             " FROM pmr_trn_tpurchaseorderdtl a " +
                            "LEFT JOIN pmr_trn_tpurchaseorder e ON e.purchaseorder_gid = a.purchaseorder_gid " +
                            " left join hrm_mst_tbranch f on f.branch_gid = e.branch_gid " +
                            "LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                            "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
                            "LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid " +
                            "WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
                            "GROUP BY a.purchaseorder_gid";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");

                msSQL = "SELECT a.address1, a.branch_name,a.branch_logo_path as branch_logo, a.authorized_sign_path as DataColumn14,a.city, a.gst_no, a.state, a.postal_code, a.contact_number, a.email, a.email_id, a.branch_gid, a.branch_logo, a.tin_number, a.cst_number,b.termsandconditions as DataColumn14 " +
             "FROM hrm_mst_tbranch a " +
             "LEFT JOIN pmr_trn_tpurchaseorder b ON b.branch_gid = a.branch_gid " +
             "WHERE b.purchaseorder_gid='" + purchaseorder_gid + "'";

                dt1 = objdbconn.GetDataTable(msSQL);
                DataTable4.Columns.Add("branch_logo", typeof(byte[]));
                DataTable4.Columns.Add("DataColumn14", typeof(byte[]));
                if (dt1.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt1.Rows)
                    {
                        company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["branch_logo"].ToString().Replace("../../", ""));
                        authorized_sign_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["DataColumn14"].ToString().Replace("../../", ""));

                        if (System.IO.File.Exists(company_logo_path) && System.IO.File.Exists(authorized_sign_path))
                        {
                            //Convert  Image Path to Byte
                            branch_logo = System.Drawing.Image.FromFile(company_logo_path);
                            DataColumn14 = System.Drawing.Image.FromFile(authorized_sign_path);
                            byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                            byte[] DataColumn14_bytes = (byte[])(new ImageConverter()).ConvertTo(DataColumn14, typeof(byte[]));

                            DataRow newRow = DataTable4.NewRow();
                            newRow["branch_logo"] = branch_logo_bytes;
                            newRow["DataColumn14"] = DataColumn14_bytes;
                            DataTable4.Rows.Add(newRow);
                        }
                    }
                }




                dt1.Dispose();
                DataTable4.TableName = "DataTable4";
                myDS.Tables.Add(DataTable4);

                ReportDocument oRpt = new ReportDocument();
                oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_pmr"].ToString(), "PmrCrpPurchaseOrder.rpt"));
                oRpt.SetDataSource(myDS);
                string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Purchase Order_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
                oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
                myConnection.Close();

                var ls_response = objFnazurestorage.reportStreamDownload(path);
                File.Delete(path);
                return ls_response;
            }
            

            else if (lscompany_code == "MEDIA" || lscompany_code == "media")
            {

                OdbcConnection myConnection = new OdbcConnection();
                myConnection.ConnectionString = objdbconn.GetConnectionString();
                OdbcCommand MyCommand = new OdbcCommand();
                MyCommand.Connection = myConnection;
                DataSet myDS = new DataSet();
                OdbcDataAdapter MyDA = new OdbcDataAdapter();

                msSQL = "select a.purchaseorder_gid,k.currency_symbol,a.addon_amount,a.purchaseorder_remarks, a.purchaserequisition_gid,a.currency_code,CONCAT(cast(a.payment_days as char(20)),' Days') as payment_day ,a.tax_amount,a.ship_via,a.freight_terms,a.delivery_location,a.payment_terms, " +
                    " CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorder_gid ELSE a.poref_no END AS purchaseorder_reference," +
                   " date_format(a.purchaseorder_date,'%d/%m/%Y') as purchaseorder_date,n.tax_name, h.gst_no as companygst_no,date_format(Date_add(purchaseorder_date,Interval a.delivery_days day),'%d-%m-%y') as ExpDate," +
                   " a.vendor_address, a.vendor_contact_person, a.created_by as user_gid, " +
                   " format(a.total_amount,2) as total_amount , " +
                   " a.vendor_faxnumber as fax, b.contact_telephonenumber as contact_telephonenumber," +
                   " a.termsandconditions,b.pan_number, " +
                   " CASE when a.quote_referenceno = '--Select--' then '' " +
                   " else a.quote_referenceno end as 'quotation_ref', " +
                   " b.vendor_companyname,a.vendor_emailid as email_id," +
                   " a.freightcharges as freight_charges,format(a.buybackorscrap,2) as buyback, " +
                   " format(a.packing_charges,2)as packing_charges ,format(a.insurance_charges,2)as insurance_charges," +
                   "(a.total_amount- a.discount_amount + a.tax_amount) as total_amount1, " +
                   " concat(c.user_firstname,' - ',e.department_name) as user_firstname,p.salesperson_gid,q.slno,(r.user_firstname)as salesperson_name,a.roundoff, " +
                   " d.employee_emailid as user_email, d.employee_phoneno as user_phone , f.city , f.state , f.postal_code , g.country_name, " +
                   " h.branch_name, h.branch_header, b.ifsc_code as ecc_no, b.rtgs_code as tngst_no, b.cst_number as cst ,b.email_id DataColumn34,a.poref_no DataColumn26, " +
                   " b.tin_number as tin_no, (h.branch_location) as branch_footer,a.discount_percentage,a.discount_amount as discount_amount1,a.shipping_address, " +
                   " i.costcenter_code as cost_center,i.costcenter_name,j.quotation_referenceno,date_format(l.purchaserequisition_date,'%d-%m-%Y')as purchaserequisition_date,b.tax_number as DataColumn35 from pmr_trn_tpurchaseorder a " +
                   " left join pmr_mst_tcostcenter i on i.costcenter_gid = a.costcenter_gid " +
                   " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                   " left join adm_mst_tuser c on c.user_gid = a.created_by " +
                   " left join hrm_mst_temployee d on d.user_gid = c.user_gid " +
                   " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
                   " left join adm_mst_taddress f on  f.address_gid = b.address_gid " +
                   " left join adm_mst_tcountry g on g.country_gid = f.country_gid " +
                   " left join hrm_mst_tbranch h on a.branch_gid = h.branch_gid " +
                   " left join crm_trn_tcurrencyexchange k on g.country_gid=k.country_gid " +
                   " left join pmr_trn_tquotationvendordetails j on a.quotation_gid=j.quotation_gid" +
                   " left join pmr_trn_tpurchaserequisition l on a.purchaserequisition_gid=l.purchaserequisition_gid" +
                   " left join acp_mst_ttax n on a.tax_gid = n.tax_gid " +
                   " left join pmr_trn_tsalesorder2purchaseorder p on a.purchaseorder_gid=p.purchaseorder_gid " +
                   " left join smr_trn_tsalesorderdtl q on p.salesorder_gid=q.salesorder_gid " +
                   " left join adm_mst_tuser r on p.salesperson_gid=r.user_gid " +
                   " WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
                   " group by a.purchaseorder_gid";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable1");



                currency number2words = fnconvertnumbertowords(purchaseorder_gid, "PO_REPORT");

             

          msSQL = " SELECT a.product_gid, b.hsn_number, a.purchaseorderdtl_gid,a.display_field_name ,a.qty_ordered, a.discount_percentage,  CONCAT('', FORMAT(a.discount_amount, 2)COLLATE utf8mb4_general_ci) AS discount_amount," +
        " a.tax_name, a.tax_amount, a.tax_name2,a.tax_amount2, a.tax_name3,a.producttotal_amount, a.tax_amount3, a.tax_percentage, a.tax_percentage2, a.tax_percentage3,  e.addon_amount, a.product_remarks,  a.needby_date, " +
        "  FORMAT(a.product_price, 2) AS product_price, CONCAT(a.tax_name, '\n', a.tax_name2) AS DataColumn41, CASE WHEN a.tax_amount2 != 0 THEN CONCAT(a.tax_amount, '\\n', a.tax_amount2) ELSE a.tax_amount END AS DataColumn39, " +
        " ROUND(((a.qty_ordered * a.product_price) + (a.tax_amount2 + a.tax_amount) - a.discount_amount), 2) AS product_total, " +
        " ROUND(SUM(((a.qty_ordered * a.product_price) + (a.tax_amount2 + a.tax_amount) - a.discount_amount)), 2) AS DataColumn44, " +
        " CASE WHEN a.tax_amount2 = 0 AND a.tax_amount3 = 0 THEN CONCAT(a.tax_name, '', a.tax_amount COLLATE utf8mb4_general_ci) " +
        " WHEN a.tax_amount = 0 AND a.tax_amount2 = 0 THEN CONCAT(a.tax_name3, ' ', a.tax_amount3 COLLATE utf8mb4_general_ci) " +
        " ELSE CONCAT(a.tax_name, ' ', a.tax_amount COLLATE utf8mb4_general_ci, '\n', a.tax_name2, '', a.tax_amount2 COLLATE utf8mb4_general_ci) END AS DataColumn36, " +
        " e.payment_terms AS DataColumn37, b.customerproduct_code AS product_code, a.productuom_name, " +
        " CASE WHEN a.display_field_name IS NULL OR a.display_field_name = '' THEN a.product_name ELSE CONCAT(a.product_name, '\\n', a.display_field_name) END AS product_name " +
        " FROM pmr_trn_tpurchaseorderdtl a " +
        " LEFT JOIN pmr_trn_tpurchaseorder e ON e.purchaseorder_gid = a.purchaseorder_gid " +
        " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
        " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
        " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid " +
        " WHERE a.purchaseorder_gid = '" + purchaseorder_gid + "' " +
        " GROUP BY a.purchaseorderdtl_gid";


                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable2");



                //currency number2words = fnconvertnumbertowords(purchaseorder_gid, "PO_REPORT");

                msSQL = "SELECT e.purchaseorder_date, e.delivery_days,sum(a.producttotal_amount) as DataColumn40,f.gst_no as DataColumn39, " +
                            " CONCAT('',format(e.total_amount, 2)COLLATE utf8mb4_general_ci) as total_amount, " +
                            " FORMAT(e.discount_amount,2) AS dis_amount,CONCAT('',Format(e.netamount,2)COLLATE utf8mb4_general_ci) as DataColumn11, " +
                           " CONCAT(UPPER(SUBSTRING('" + number2words.number2words + "', 1, 1)), LOWER(SUBSTRING('" + number2words.number2words + "', 2))) AS DataColumn10, " +
                            "purchaseorder_reference, e.currency_code,f.branch_name as DataColumn12,f.address1 as DataColumn13 ,f.city as DataColumn14 ," +
                            "f.gst_no as DataColumn15, f.state as DataColumn16, f.postal_code as DataColumn33, f.contact_number as DataColumn17 , f.email as DataColumn18, f.email_id, " +
                             " format(sum((a.tax_amount + a.tax_amount2 + a.tax_amount3)),2) as DataColumn19, " +
                             " format(sum((a.qty_ordered*a.product_price) - a.discount_amount),2) as DataColumn20, " +
                              " CONCAT('', (CAST(SUM(CASE WHEN a.tax_name LIKE '%ZERO VAT%' THEN  (a.qty_ordered * a.product_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn21," +
                             " CONCAT('', (CAST(SUM(CASE WHEN a.tax_name LIKE '%ZERO VAT%' THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn22," +
                             " CONCAT('', (CAST(SUM(CASE WHEN a.tax_name LIKE '%VAT%' AND a.tax_name NOT LIKE '%ZERO VAT%' THEN  (a.qty_ordered * a.product_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn23," +
                             " CONCAT('', (CAST(SUM(CASE WHEN a.tax_name LIKE '%VAT%' AND a.tax_name NOT LIKE '%ZERO VAT%' THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2)))) AS DataColumn24 " +
                            " FROM pmr_trn_tpurchaseorderdtl a " +
                            "LEFT JOIN pmr_trn_tpurchaseorder e ON e.purchaseorder_gid = a.purchaseorder_gid " +
                            " left join hrm_mst_tbranch f on f.branch_gid = e.branch_gid " +
                            "LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                            "LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
                            "LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid " +
                            "WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
                            "GROUP BY a.purchaseorder_gid";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");

                msSQL = "SELECT a.address1, a.branch_name,a.branch_logo_path, a.authorized_sign_path ,a.city, a.gst_no, a.state, a.postal_code, a.contact_number, a.email, a.email_id, a.branch_gid, a.branch_logo, a.tin_number, a.cst_number,b.termsandconditions as DataColumn14 " +
             "FROM hrm_mst_tbranch a " +
             "LEFT JOIN pmr_trn_tpurchaseorder b ON b.branch_gid = a.branch_gid " +
             "WHERE b.purchaseorder_gid='" + purchaseorder_gid + "'";

                dt1 = objdbconn.GetDataTable(msSQL);
                DataTable4.Columns.Add("branch_logo", typeof(byte[]));
                DataTable4.Columns.Add("DataColumn14", typeof(byte[]));
                if (dt1.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt1.Rows)
                    {
                        company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["branch_logo_path"].ToString().Replace("../../", ""));
                        authorized_sign_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["authorized_sign_path"].ToString().Replace("../../", ""));

                        if (System.IO.File.Exists(company_logo_path) && System.IO.File.Exists(authorized_sign_path))
                        {
                            //Convert  Image Path to Byte
                            branch_logo = System.Drawing.Image.FromFile(company_logo_path);
                            DataColumn14 = System.Drawing.Image.FromFile(authorized_sign_path);
                            byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                            byte[] DataColumn14_bytes = (byte[])(new ImageConverter()).ConvertTo(DataColumn14, typeof(byte[]));

                            DataRow newRow = DataTable4.NewRow();
                            newRow["branch_logo"] = branch_logo_bytes;
                            newRow["DataColumn14"] = DataColumn14_bytes;
                            DataTable4.Rows.Add(newRow);
                        }
                    }
                }




                dt1.Dispose();
                DataTable4.TableName = "DataTable4";
                myDS.Tables.Add(DataTable4);

                ReportDocument oRpt = new ReportDocument();
                oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_pmr"].ToString(), "PoMedialink.rpt"));
                oRpt.SetDataSource(myDS);
                string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Purchase Order_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
                oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
                myConnection.Close();

                var ls_response = objFnazurestorage.reportStreamDownload(path);
                File.Delete(path);
                return ls_response;
            }


            else
            {
                OdbcConnection myConnection = new OdbcConnection();
                myConnection.ConnectionString = objdbconn.GetConnectionString();
                OdbcCommand MyCommand = new OdbcCommand();
                MyCommand.Connection = myConnection;
                DataSet myDS = new DataSet();
                OdbcDataAdapter MyDA = new OdbcDataAdapter();
                currency number2words = fnconvertnumbertowords(purchaseorder_gid, "PO_REPORT");

                msSQL = "select date_format(a.purchaseorder_date,'%d/%m/%Y') as DataColumn4,a.purchaseorder_reference,a.branch_gid,b.vendor_companyname,a.vendor_gid,a.vendor_address,a.vendor_gid, " +
                   " CONCAT('" + number2words.symbol + " ',format(a.total_amount,2)) AS DataColumn5,a.mode_despatch,a. currency_code,a.freightcharges as freight_charges,a. shipping_address,a.netamount,a.vendor_emailid,a.tax_percentage, " +
                  "  CONCAT(UPPER(SUBSTRING('" + number2words.number2words + "', 1, 1)), LOWER(SUBSTRING('" + number2words.number2words + "', 2))) AS WORDS, " +
                   " a.discount_amount,a.discount_percentage,a.termsandconditions,a.delivery_days,a.payment_term,a.expected_date,a.netamount, " +
                   " CASE WHEN a.poref_no IS NOT NULL AND a.poref_no != '' THEN a.poref_no ELSE a.purchaseorder_gid END AS purchaseorder_gid, " +
                   " c.branch_name,c.address1,c.city,c.state,c.postal_code,c.contact_number,c.email,c.email_id,c.gst_no, " +
                   " a.discount_amount,a.roundoff,a.tax_amount,a.addon_amount from pmr_trn_tpurchaseorder a " +
                   " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                   " left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                   " WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
                   " group by a.purchaseorder_gid";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable1");



                msSQL="select a.purchaseorderdtl_gid, format(a.producttotal_amount,2) AS DataColumn18 ,a.product_gid,a.uom_gid," +
                    " format(a.product_price,2) as product_price,a.qty_ordered,a.discount_percentage, a.discount_amount, " +
                    " CASE WHEN(a.tax_name = '--No Tax--' OR a.tax_name = 'NoTax') THEN 'No Tax' WHEN(a.tax_name2 = '--No Tax--' OR a.tax_name2 = 'NoTax') THEN CONCAT(a.tax_name, ': ', CAST(a.tax_amount AS CHAR) )  ELSE CONCAT(a.tax_name, ': ', CAST(a.tax_amount AS CHAR)," +
                    " IF(a.tax_amount2<> 0, CONCAT('\n', a.tax_name2, ' ', CAST(a.tax_amount2 AS CHAR)), '')  ) END AS all_taxes," +
                    " a.tax_percentage,a.tax_amount,a.tax_amount2,a.tax_percentage2,a.tax_name,a.tax_name2,a.tax1_gid," +
                    " a.tax2_gid,a.product_code, CASE WHEN a.display_field_name IS NULL OR a.display_field_name = '' THEN a.product_name ELSE CONCAT(a.product_name, '\n', a.display_field_name) END AS product_name  ,a.productuom_name,a.taxsegment_gid,b.product_desc from pmr_trn_tpurchaseorderdtl a" +
                    " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                    " WHERE a.purchaseorder_gid = '"+ purchaseorder_gid +"'  GROUP BY a.purchaseorderdtl_gid";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable2");


                msSQL = " select purchaseorder_gid,purchaseorderdtl_gid,product_gid,uom_gid,product_price,qty_ordered, discount_percentage,discount_amount, " +
                          " tax_percentage,tax_amount,tax_percentage2,tax1_gid,tax2_gid, " +
                          " format(sum((a.tax_amount + a.tax_amount2)),2) as tax_total, " +
                          " format(sum((a.qty_ordered*a.product_price)+(a.tax_amount + a.tax_amount2) - a.discount_amount),2) as product_total, " +
                          " format(sum((a.qty_ordered*a.product_price) - a.discount_amount),2) as DataColumn25, " +
                          " CONCAT('" + number2words.symbol + " ', FORMAT(SUM(COALESCE(a.tax_amount, 0)), 2)) AS taxes1," +
                          " CONCAT('" + number2words.symbol + " ',FORMAT(SUM(COALESCE(a.tax_amount2, 0)), 2)) AS taxes2, " +
                          " CONCAT(COALESCE(NULLIF(a.tax_name, ''), 'No Tax'), ' ', a.tax_percentage, '%') AS tax_name1, " +
                          " CONCAT(COALESCE(NULLIF(a.tax_name2, ''), 'No Tax'), ' ', a.tax_percentage2, '%') AS tax_name2 from pmr_trn_tpurchaseorderdtl a " +
                            "WHERE a.purchaseorder_gid='" + purchaseorder_gid + "' " +
                            "GROUP BY a.purchaseorder_gid";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");

                msSQL = " select a.branch_logo_path as company_logo from hrm_mst_tbranch a " +
                  "LEFT JOIN pmr_trn_tpurchaseorder b ON b.branch_gid = a.branch_gid " +
                   "WHERE b.purchaseorder_gid='" + purchaseorder_gid + "'";
                dt1 = objdbconn.GetDataTable(msSQL);
                DataTable4.Columns.Add("company_logo", typeof(byte[]));

                if (dt1.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt1.Rows)
                    {
                        company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo"].ToString().Replace("../../", ""));

                        if (System.IO.File.Exists(company_logo_path))
                        {
                            //Convert  Image Path to Byte
                            company_logo = System.Drawing.Image.FromFile(company_logo_path);
                            byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));
                            DataRow newRow = DataTable4.NewRow();
                            newRow["company_logo"] = branch_logo_bytes;

                            DataTable4.Rows.Add(newRow);
                        }
                    }
                }
                dt1.Dispose();
                DataTable4.TableName = "DataTable4";
                myDS.Tables.Add(DataTable4);

                ReportDocument oRpt = new ReportDocument();
                oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_pmr"].ToString(), "pmr_trn_tPurchaseOrder.rpt"));
                oRpt.SetDataSource(myDS);
                string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Purchase Order_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
                oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
                myConnection.Close();

                var ls_response = objFnazurestorage.reportStreamDownload(path);
                File.Delete(path);
                return ls_response;
            }
        }


        //based on currency

        public currency fnconvertnumbertowords(string gid, string type)
        {
            currency obj = new currency();
            string number = string.Empty;
            string words = string.Empty;
            string lscurrency_code = string.Empty;

            if (type == "PO_REPORT")
            {
                msSQL = "select REPLACE(FORMAT(total_amount,2), ',', '') AS formatted_amount from pmr_trn_tpurchaseorder where purchaseorder_gid='" + gid + "'";
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
                    // obj.symbol = "";

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
                else if (lscurrency_code == "USD")
                {
                    words = int_part + " DOLLARS AND " + dec_part + " CENTS ONLY";
                    words = words.ToUpper();
                    obj.symbol = "$";
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
                else if (lscurrency_code == "USD")
                {
                    words = int_part + " DOLLARS ONLY";
                    words = words.ToUpper();
                    obj.symbol = "$";

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

        public async Task DaGetProductsearchSummary(string producttype_gid, string product_name, string vendor_gid, MdlPmrTrnPurchaseOrder values)
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
                values.GetProductsearch = getModuleList;
                values.GetTaxSegmentList = allTaxSegmentsList;
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

        private List<GetProductsearch> ProcessProductData(DataTable productDt)
        {
            var getModuleList = new List<GetProductsearch>();
            foreach (DataRow dtRow in productDt.Rows)
            {
                var product = new GetProductsearch
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

        private List<GetTaxSegmentList> ProcessTaxSegmentData(DataTable taxDt)
        {
            var allTaxSegmentsList = new List<GetTaxSegmentList>();
            if (taxDt != null)
            {
                foreach (DataRow taxRow in taxDt.Rows)
                {
                    var taxSegment = new GetTaxSegmentList
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

        public void DaPostOnAddproduct(string user_gid, submitProducts values)
        {
            try
            {
                string taxsegment_gid = values.taxsegment_gid;

                foreach (var data in values.POProductList)
                {
                    if (data.quantity == null || data.quantity == "0")
                    {

                    }
                    else
                    {
                        string tax_gid1 = "";
                        string tax_gid2 = "";
                        string tax_gid3 = "";

                        string tax_name1 = "";
                        string tax_name2 = "";
                        string tax_name3 = "";

                        string tax_percent1 = "";
                        string tax_percent2 = "";
                        string tax_percent3 = "";

                        string tax_amount1 = "";
                        string tax_amount2 = "";
                        string tax_amount3 = "";

                        if (data.taxSegments != null && data.taxSegments.Count > 0)
                        {
                            tax_gid1 = data.taxSegments[0]?.tax_gid ?? "--no tax--";
                            tax_name1 = data.taxSegments[0]?.tax_name ?? "--no tax--";
                            tax_percent1 = data.taxSegments[0]?.tax_percentage ?? "--no tax--";
                            tax_amount1 = data.taxSegments[0]?.taxAmount ?? "--no tax--";

                            if (data.taxSegments.Count > 1)
                            {
                                tax_gid2 = data.taxSegments[1]?.tax_gid ?? "--no tax--";
                                tax_name2 = data.taxSegments[1]?.tax_name ?? "--no tax--";
                                tax_percent2 = data.taxSegments[1]?.tax_percentage ?? "--no tax--";
                                tax_amount2 = data.taxSegments[1]?.taxAmount ?? "--no tax--";
                            }

                            if (data.taxSegments.Count > 2)
                            {
                                tax_gid3 = data.taxSegments[2]?.tax_gid ?? "--no tax--";
                                tax_name3 = data.taxSegments[2]?.tax_name ?? "--no tax--";
                                tax_percent3 = data.taxSegments[2]?.tax_percentage ?? "--no tax--";
                                tax_amount3 = data.taxSegments[2]?.taxAmount ?? "--no tax--";
                            }
                        }




                        msGetGid = objcmnfunctions.GetMasterGID("PODC");


                        string lspercentage1, lspercentage2, lstax_name1, lstax_name2;

                        if (string.IsNullOrEmpty(values.tax1))
                        {
                            lspercentage1 = "0";
                        }
                        else
                        {
                            msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + data.tax1 + "'";
                            lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                        }
                        if (string.IsNullOrEmpty(values.tax2))
                        {
                            lspercentage2 = "0";
                        }
                        else
                        {
                            msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + data.tax2 + "'";
                            lspercentage2 = objdbconn.GetExecuteScalar(msSQL);
                        }
                        msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + data.tax1 + "'";
                        lstax_name1 = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + data.tax2 + "'";
                        lstax_name2 = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " insert into pmr_tmp_tpurchaseorder ( " +
                           " tmppurchaseorderdtl_gid," +
                           " tmppurchaseorder_gid," +
                           " qty_poadjusted," +
                           " product_gid," +
                           " product_code," +
                           " product_name," +
                           " productuom_name," +
                           " uom_gid," +
                           " qty_ordered," +
                           " product_price," +
                           " discount_percentage," +
                           " discount_amount," +
                           " tax_name," +
                           " tax_name2," +
                           " tax_name3," +
                           " tax_percentage," +
                           " tax_percentage2," +
                           " tax_percentage3," +
                           " tax1_gid," +
                           " tax2_gid," +
                           " tax_amount, " +
                           " tax_amount2, " +
                           " tax_amount3," +
                            " taxsegment_gid, " +
                        " taxsegmenttax_gid, " +

                         " taxseg_taxgid1, " +
                        " taxseg_taxgid2, " +
                         " taxseg_taxgid3, " +
                        " taxseg_taxname1, " +
                         " taxseg_taxname2, " +
                          " taxseg_taxname3, " +
                               " taxseg_taxpercent1, " +
                        " taxseg_taxpercent2, " +
                         " taxseg_taxpercent3, " +
                        " taxseg_taxamount1, " +
                         " taxseg_taxamount2, " +
                          " taxseg_taxamount3, " +
                          " taxseg_taxtotal, " +

                        " display_field," +
                           " created_by," +
                           " producttotal_price " +
                           " ) values ( " +
                           "'" + msGetGid + "'," +
                           "''," +
                            "'0.00'," +
                           "'" + data.product_gid + "'," +
                           "'" + data.product_code + "'," +
                           "'" + data.product_name + "'," +
                           "'" + data.productuom_name + "'," +
                           "'" + data.productuom_gid + "'," +
                           "'" + data.quantity + "'," +
                           "'" + data.unitprice + "'," +
                           "'" + data.discount_persentage + "'," +
                           "'" + data.discount_amount + "'," +
                           "'" + lstax_name1 + "'," +
                           "'" + lstax_name2 + "'," +
                           "'" + data.tax3 + "'," +
                           "'" + lspercentage1 + "'," +
                           "'" + lspercentage2 + "'," +
                           "'0.00'," +
                           "'" + data.tax1 + "'," +
                           "'" + data.tax2 + "'," +
                           "'" + data.taxamount1 + "'," +
                           "'" + data.taxamount2 + "'," +
                           "'0.00'," +
                            "'" + data.taxsegment_gid + "'," +
                        "'" + data.tax_gids_string + "'," +

                          "'" + tax_gid1 + "'," +
                        "'" + tax_gid2 + "'," +
                          "'" + tax_gid3 + "'," +
                        "'" + tax_name1 + "'," +
                          "'" + tax_name2 + "'," +
                        "'" + tax_name3 + "'," +
                          "'" + tax_percent1 + "'," +
                        "'" + tax_percent2 + "'," +
                          "'" + tax_percent3 + "'," +
                        "'" + tax_amount1 + "'," +
                          "'" + tax_amount2 + "'," +
                        "'" + tax_amount3 + "'," +
                        "'" + data.totalTaxAmount + "'," +

                         "'" + data.display_field + "'," +
                           "'" + user_gid + "'," +
                           "'" + data.total_amount + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


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


        public void DaGetProductTaxSegment(string product_gid, string vendor_gid, MdlPmrTrnPurchaseOrder values)

        {
            try
            {
                msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                                " CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage,d.tax_amount, " +
                                " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                                " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                                " left join acp_mst_tvendor f on f.taxsegment_gid = e.taxsegment_gid " +
                                " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                                " where a.product_gid='" + product_gid + "'   and f.vendor_gid='" + vendor_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getGetTaxSegmentList = new List<GetTaxSegmentList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getGetTaxSegmentList.Add(new GetTaxSegmentList
                        {
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),

                        });
                        values.GetTaxSegmentList = getGetTaxSegmentList;
                    }
                }

                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Getting Tax Segment details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }


        }


        public void DaPostProductAdd(string user_gid, PostPOProduct_list values)
        {
            msGetGid = objcmnfunctions.GetMasterGID("PODC");


            msSQL = " SELECT d.productgroup_name,a.productuom_gid, a.product_gid, a.product_name, b.productuom_name,c.producttype_name,a.producttype_gid FROM pmr_mst_tproduct a " +
                 " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                 "  LEFT JOIN pmr_mst_tproducttype c ON c.producttype_gid = a.producttype_gid " +
                 "  LEFT JOIN pmr_mst_tproductgroup d ON d.productgroup_gid = a.productgroup_gid  " +
                 " WHERE product_gid = '" + values.product_name + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows == true)
            {
                objOdbcDataReader.Read();
                lsproductgid = objOdbcDataReader["product_gid"].ToString();
                lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();
                lsproduct_name = objOdbcDataReader["product_name"].ToString();
                lsproductuom_name = objOdbcDataReader["productuom_name"].ToString();
                lsproducttype_name = objOdbcDataReader["producttype_name"].ToString();
                lsproductgroup_name = objOdbcDataReader["productgroup_name"].ToString();
                objOdbcDataReader.Close();
            }
            if (lsproducttype_name == "Services" & lsproductgroup_name== "General")
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
            else {

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
        public void DaGetProductWithTaxSummary(string product_gid, string vendor_gid, MdlPmrTrnPurchaseOrder values)
        {
            string lsSQLTYPE = "vendor";

            msSQL = "call pmr_mst_spproductsearch('" + lsSQLTYPE + "','" + product_gid + "', '" + vendor_gid + "')";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var allTaxSegmentsList = new List<GetTaxSegmentListpurchaseorder>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt1 in dt_datatable.Rows)
                {
                    allTaxSegmentsList.Add(new GetTaxSegmentListpurchaseorder
                    {
                        taxsegment_gid = dt1["taxsegment_gid"].ToString(),
                        taxsegment_name = dt1["taxsegment_name"].ToString(),
                        tax_name = dt1["tax_name"].ToString(),
                        tax_percentage = dt1["tax_percentage"].ToString(),
                        tax_gid = dt1["tax_gid"].ToString(),
                        mrp_price = dt1["mrp_price"].ToString(),
                        cost_price = dt1["cost_price"].ToString(),
                        tax_amount = dt1["tax_amount"].ToString(),
                        product_name = dt1["product_name"].ToString(),
                        product_gid = dt1["product_gid"].ToString(),
                        tax_prefix = dt1["tax_prefix"].ToString(),
                    });
                    values.GetTaxSegmentListpurchaseorder = allTaxSegmentsList;
                }
            }
        }

        public void DaGetProductsearchSummaryPurchase(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                string lsSqlType = "product";

                msSQL = " call pmr_mst_spproductsearch('" + lsSqlType + "','','')";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetpurchaseorderProductsearchs>();


                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        var product = new GetpurchaseorderProductsearchs
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
                            //producttype_name = dt["producttype_name"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            quantity = 0,
                            total_amount = 0,
                            discount_percentage = 0,
                            discount_amount = 0,
                        };
                        getModuleList.Add(product);

                    }
                    values.GetpurchaseorderProductsearchs = getModuleList;
                }

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


        public void DaGetEditPurchaseOrderSummary(string user_gid, string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                msSQL = " delete from pmr_tmp_tpurchaseorder where " +
                        " created_by = '" + user_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select purchaseorderdtl_gid,purchaseorder_gid,qty_ordered,product_gid,product_code,product_name,productuom_name,product_price,display_field_name," +
                        " discount_percentage,discount_amount,tax_name,tax_name2,tax_name3,tax_percentage,tax_percentage2,tax_amount2,tax_amount,tax_amount1_L,tax_amount2_L, " +
                        " tax1_gid,tax2_gid,producttotal_amount from pmr_trn_tpurchaseorderdtl " +
                        " WHERE purchaseorder_gid = '" + purchaseorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow da in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("PODC");


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
                                "'" + msGetGid + "'," +
                                "'" + da["purchaseorder_gid"].ToString() + "'," +
                                "'0.00'," +
                                "'" + da["product_gid"].ToString() + "'," +
                                "'" + da["product_code"].ToString() + "'," +
                                "'" + da["product_name"].ToString() + "'," +
                                "'" + da["productuom_name"].ToString() + "'," +
                                "'" + da["qty_ordered"].ToString() + "'," +
                                "'" + da["display_field_name"].ToString().Replace("'", "\\\'") + "', "+
                                "'" + da["product_price"].ToString() + "',";
                        if (string.IsNullOrEmpty(da["discount_percentage"].ToString()))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + da["discount_percentage"].ToString() + "',";
                        }
                        msSQL += "'" + da["discount_amount"].ToString() + "'," +
                        "'" + da["tax_name"].ToString() + "'," +
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
                            msSQL += "'" + da["tax_amount"].ToString() + "',";
                        }
                        if (string.IsNullOrEmpty(da["tax_amount2"].ToString()))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + da["tax_amount2"].ToString() + "',";
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


                msSQL = "select x.renewal_flag,x.frequency_term,date_format(x.renewal_date,'%d-%m-%Y') as renewal_date,a.file_path,a.file_name,b.vendor_companyname,b.address_gid,a.shipping_address,c.branch_name,d.tax_name, a.purchaseorder_gid,a.branch_gid,a.vendor_gid," +
                        " a.vendor_address,a.mode_despatch,a.poref_no,a.termsandconditions,a.discount_amount,a.tax_percentage,a.tax_amount,a.addon_amount,g.currencyexchange_gid, " +
                        " a.currency_code,a.exchange_rate,a.freightcharges,a.tax_gid,a.roundoff,a.freight_terms,a.netamount,a.requested_by,e.user_firstname,b.email_id," +
                        " a.requested_details,a.po_covernote ,a.total_amount,date_format(a.purchaseorder_date,'%d-%m-%Y') as po_date,date_format(a.expected_date,'%d-%m-%Y') as expected_date,a.payment_terms,f.address2,b.gst_no,b.contact_telephonenumber " +
                        " from pmr_trn_tpurchaseorder a left join acp_mst_tvendor b on a.vendor_gid=b.vendor_gid " +
                        " left join hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                        " left join acp_mst_ttax d on a.tax_gid= d.tax_gid " +
                        " left join adm_mst_taddress f on b.address_gid = f.address_gid" +
                        " left join adm_mst_tuser e on a.requested_by = e.user_gid " +
                        " left join pmr_trn_trenewal x on a.purchaseorder_gid = x.purchaseorder_gid " +
                        " left join crm_trn_tcurrencyexchange g on a.currency_code = g.currency_code  " +
                        " where a.purchaseorder_gid = '" + purchaseorder_gid + "'";
                ds_dataset = objdbconn.GetDataSet(msSQL, "pmr_trn_tpurchaseorder");
                var getModuleList = new List<GetEditPurchaseOrderSummary>();
                if (ds_dataset.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dt in ds_dataset.Tables[0].Rows)
                    {
                        getModuleList.Add(new GetEditPurchaseOrderSummary
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            address_gid = dt["address_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            netamount = dt["netamount"].ToString().Replace(".00",""),
                            requested_by = dt["requested_by"].ToString(),
                            requested_details = dt["requested_details"].ToString(),
                            po_covernote = dt["po_covernote"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            po_date = dt["po_date"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            address2 = dt["address2"].ToString(),
                            gst_no = dt["gst_no"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            poref_no = dt["poref_no"].ToString(),
                            file_name = dt["file_name"].ToString(),
                            file_path = dt["file_path"].ToString(),
                            renewal_flag = dt["renewal_flag"].ToString(),
                            frequency_term = dt["frequency_term"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                        });
                        values.GetEditPurchaseOrderSummary = getModuleList;
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


        public void DaPoSubmit(string user_gid, GetViewPurchaseOrder values)
        {
            try
            {
               

              

                int lsfreight = 0;
                int lsinsurance = 0;
                string uiDateStr = values.po_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string po_date = uiDate.ToString("yyyy-MM-dd");

                string uiDateStr2 = values.expected_date;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string expected_date = uiDate2.ToString("yyyy-MM-dd");
              
                string lstax_gid;
                msSQL = "select poapproval_flag from adm_mst_tcompany";
                string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                //msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP", "", user_gid);
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "' ";
                string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                if (string.IsNullOrEmpty(values.tax_name4))
                {
                    lstaxpercentage = "0";

                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_prefix='" + values.tax_name4 + "'";
                    lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                }
            

                msSQL = "UPDATE pmr_trn_tpurchaseorder SET " +
                         "purchaserequisition_gid = '" + msGetGID + "', " +
                         "purchaseorder_reference = '" + values.purchaseorder_gid + "', " +
                         "purchaseorder_date = '" + po_date + "', " +
                         "expected_date = '" + expected_date + "', " +
                         "branch_gid = '" + values.branch_name + "', " +
                         "created_by = '" + user_gid + "', " +
                         "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                         "vendor_gid = '" + values.vendor_companyname + "', ";

                        if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                        {
                            msSQL += "vendor_address = '" + values.address1.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "vendor_address = '" + values.address1 + "', ";
                        }

                        if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                        {
                            msSQL += "shipping_address = '" + values.shipping_address.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "shipping_address = '" + values.shipping_address + "', ";
                        }
                        if (!string.IsNullOrEmpty(values.employee_name) && values.employee_name.Contains("'"))
                        {
                            msSQL += "requested_by = '" + values.employee_name.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "requested_by = '" + values.employee_name + "', ";
                        }
                        if (!string.IsNullOrEmpty(values.delivery_terms) && values.delivery_terms.Contains("'"))
                        {
                            msSQL += "freight_terms = '" + values.delivery_terms.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "freight_terms = '" + values.delivery_terms + "', ";
                        }
                        if (!string.IsNullOrEmpty(values.payment_terms) && values.payment_terms.Contains("'"))
                        {
                            msSQL += "payment_terms = '" + values.payment_terms.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "payment_terms = '" + values.payment_terms + "', ";
                        }

                msSQL += "requested_details = '" + values.Requestor_details + "', ";
                        if (!string.IsNullOrEmpty(values.dispatch_mode) && values.dispatch_mode.Contains("'"))
                        {
                            msSQL += "mode_despatch = '" + values.dispatch_mode.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "mode_despatch = '" + values.dispatch_mode + "', ";
                        }
                        msSQL += "currency_code = '" + lscurrency_code + "', " +
                         "exchange_rate = '" + values.exchange_rate + "', ";
                        if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                        {
                            msSQL += "po_covernote = '" + values.po_covernote.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "po_covernote = '" + values.po_covernote + "', ";
                        }
                        if (!string.IsNullOrEmpty(values.po_covernote) && values.po_covernote.Contains("'"))
                        {
                            msSQL += "purchaseorder_remarks = '" + values.po_covernote.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "purchaseorder_remarks = '" + values.po_covernote + "', ";
                        }
                        msSQL +="total_amount = '" + values.grandtotal + "', ";

                        if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                        {
                            msSQL += "termsandconditions = '" + values.template_content.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "termsandconditions = '" + values.template_content + "', ";
                        }

                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "purchaseorder_status = 'PO Approved', ";
                        }
                        else
                        {
                            msSQL += "purchaseorder_status = 'Operation Pending PO', ";
                        }

                        msSQL += "purchaseorder_flag = 'PO Approved', " +
                             "poref_no = '" + values.po_no + "', " +
                             "netamount = '" + values.totalamount + "', " +
                             "addon_amount = '" + values.addoncharge + "', " +
                             "freightcharges = '" + values.freightcharges + "', ";

                        if (string.IsNullOrEmpty(values.additional_discount))
                        {
                            msSQL += "discount_amount = '0.00', ";
                        }
                        else
                        {
                            msSQL += "discount_amount = '" + values.additional_discount + "', ";
                        }

                        msSQL += "tax_gid = '" + values.tax_name4 + "', ";

                        if (string.IsNullOrEmpty(lstaxpercentage))
                        {
                            msSQL += "tax_percentage = '0.00', ";
                        }
                        else
                        {
                            msSQL += "tax_percentage = '" + lstaxpercentage + "', ";
                        }

                        if (string.IsNullOrEmpty(values.tax_amount4))
                        {
                            msSQL += "tax_amount = '0.00', ";
                        }
                        else
                        {
                            msSQL += "tax_amount = '" + values.tax_amount4 + "', ";
                        }

                        if (values.roundoff == null)
                        {
                            msSQL += "roundoff = '0.00', ";
                        }
                        else
                        {
                            msSQL += "roundoff = '" + values.roundoff + "', ";
                        }

                msSQL += "taxsegment_gid = '" + values.taxsegment_gid + "' " +
                     "WHERE purchaseorder_gid = '" + values.purchaseorder_gid + "';";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (values.renewal_mode == "Y")
                {

                    msSQL = "select renewal_gid from pmr_trn_trenewal where purchaseorder_gid ='" + values.purchaseorder_gid + "' ";
                    string lsrenewal = objdbconn.GetExecuteScalar(msSQL);
                    if (!string.IsNullOrEmpty(lsrenewal))
                    {
                        string uiDateStr3 = values.renewal_date;
                        DateTime uiDate4 = DateTime.ParseExact(uiDateStr3, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        string renewal_date = uiDate4.ToString("yyyy-MM-dd");

                        msSQL = "UPDATE pmr_trn_trenewal SET " +        
                                "renewal_flag = '" + values.renewal_mode + "', " +
                                "frequency_term = '" + values.frequency_terms + "', " +
                                "vendor_gid = '" + values.vendor_companyname + "', " +
                                "renewal_date = '" + renewal_date + "', " +
                                "created_by = '" + user_gid + "', " +
                                "renewal_type = 'Purchase', " +
                                "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                                "WHERE purchaseorder_gid = '" + values.purchaseorder_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }
                    else
                    {
                        string uiDateStr3 = values.renewal_date;
                        DateTime uiDate4 = DateTime.ParseExact(uiDateStr3, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        string renewal_date = uiDate4.ToString("yyyy-MM-dd");

                        msINGetGID = objcmnfunctions.GetMasterGID("BRLP");

                        msSQL = " Insert into pmr_trn_trenewal ( " +
                                " renewal_gid, " +
                                " renewal_flag, " +
                                " frequency_term, " +
                                " vendor_gid," +
                                " renewal_date, " +
                                " purchaseorder_gid, " +
                                " created_by, " +
                                " renewal_type, " +
                                " created_date) " +
                               " Values ( " +
                                 "'" + msINGetGID + "'," +
                                 "'" + values.renewal_mode + "'," +
                                 "'" + values.frequency_terms + "'," +
                                 "'" + values.vendor_companyname + "'," +
                                 "'" + renewal_date + "'," +
                                 "'" + values.purchaseorder_gid + "'," +
                                 "'" + user_gid + "'," +
                                 "'Purchase'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }




                if (mnResult == 1)
                {
                    msSQL = " delete from pmr_trn_tpurchaseorderdtl where " +
                       " purchaseorder_gid = '" + values.purchaseorder_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    double grandtotal = 0.00;
                    msSQL = "SELECT a.tmppurchaseorderdtl_gid, a.tmppurchaseorder_gid, a.product_gid, a.product_name, a.product_code, b.productgroup_gid, " +
                        "FORMAT(a.qty_ordered, 2) AS qty_ordered, c.productgroup_name, a.productuom_name, b.producttype_gid, " +
                        "a.created_by, a.product_price, FORMAT(a.producttotal_price, 2) AS producttotal_price, " +
                        "a.discount_percentage, a.discount_amount, " +
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
                            msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
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
                                        "'" + msPOGetGID + "', " +
                                        "'" + values.purchaseorder_gid + "'," +
                                        "'" + dt["product_gid"].ToString() + "', " +
                                        "'" + dt["product_code"].ToString() + "', " +
                                        "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," + 
                                        "'" + dt["productuom_name"].ToString().Replace("'", "\\\'") + "'," + 
                                        "'" + dt["uom_gid"].ToString() + "', " +
                                        "'" + dt["type"].ToString().Replace("'", "\\\'") + "'," + 
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

                    if (mnResult != 0)

                    {
                        values.status = true;
                        values.message = "Purchase Order Raised Successfully!";



                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding product in Purchase Order!";


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

        //Contract PO

        public void DaGetContractPO(string vendor_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                msSQL = " select a.vendor_companyname,a.vendor_gid,b.product_name,b.product_price,c.product_code,d.producttype_name,e.productuom_name,e.productuom_gid, " +
                        " d.producttype_gid,b.product_gid,b.productgroup_gid from pmr_trn_tratecontract a " +
                        " left join pmr_trn_tratecontractdtl b on a.vendor_gid = b.vendor_gid " +
                        " left join pmr_mst_tproduct c on b.product_gid = c.product_gid " +
                        " left join pmr_mst_tproducttype d on c.producttype_gid = d.producttype_gid " +
                        " LEFT JOIN pmr_mst_tproductuom e ON e.productuom_gid = c.productuom_gid " +
                        " LEFT JOIN pmr_mst_tproductuomclass g ON g.productuomclass_gid = c.productuomclass_gid " +
                        " where a.vendor_gid = '" + vendor_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetContractPO>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetContractPO
                        {
                            product_name = dt["product_name"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                        });
                        values.GetContractPO = getModuleList;
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
        //Contract PO Submit
        public void DaPostcontractpo(string user_gid, contractposubmit_list values)
        {
            try
            {
                DateTime uiDate = DateTime.ParseExact(values.expected_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                expected_date = uiDate.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime uiDate1 = DateTime.ParseExact(values.po_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                po_date = uiDate1.ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 0; i < values.contractpo_list.Count; i++)
                {
                    msSQL = " select a.vendor_code,a.vendor_companyname,a.contactperson_name,a.contact_telephonenumber,a.currencyexchange_gid," +
                            " concat(b.address1,',',b.address2,',',b.city,',',b.state,',',b.postal_code) as vendor_address " +
                            " from acp_mst_tvendor a " +
                            " left join adm_mst_taddress  b on b.address_gid=a.address_gid " +
                            " where a.vendor_gid='"+values.vendor_gid+"'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            vendor_code = dt["vendor_code"].ToString();
                            vendor_companyname = dt["vendor_companyname"].ToString();
                            contactperson_name = dt["contactperson_name"].ToString();
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString();
                            vendor_address = dt["vendor_address"].ToString();
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString();

                        }
                    }
                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + currencyexchange_gid + "' ";
                    string lscurrency_code=objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " SELECT f.taxsegment_gid, d.taxsegment_gid,a.product_gid, a.product_name,concat(b.tax_prefix ) as tax_prefix," +
                            " e.taxsegment_name, d.tax_name, d.tax_gid,d.tax_percentage," +
                            " d.tax_amount, a.mrp_price," +
                            " a.cost_price FROM acp_mst_ttaxsegment2product d" +
                            " left join acp_mst_ttax b on b.tax_gid=d.tax_gid " +
                            " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid" +
                            " LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid" +
                            " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE a.product_gid='" + values.contractpo_list[i].product_gid + "'" +
                            " and f.vendor_gid='" + values.vendor_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt1 in dt_datatable.Rows)
                        {
                            taxsegment_gid = dt1["taxsegment_gid"].ToString();
                            taxsegment_name = dt1["taxsegment_name"].ToString();
                            mrp_price = dt1["mrp_price"].ToString();
                            cost_price = dt1["cost_price"].ToString();
                            if (tax_name1 == null)
                            {
                                tax_name1 = dt1["tax_name"].ToString();
                            }
                            else if (tax_name2 == null)
                            {
                                tax_name2 = dt1["tax_name"].ToString();
                            }
                            if (tax_percentage1 == null)
                            {
                                tax_percentage1 = dt1["tax_percentage"].ToString();
                            }
                            else if (tax_percentage2 == null)
                            {
                                tax_percentage2 = dt1["tax_percentage"].ToString();
                            }
                            if (tax_gid1 == null)
                            {
                                tax_gid1 = dt1["tax_gid"].ToString();
                            }
                            else if (tax_gid2 == null)
                            {
                                tax_gid2 = dt1["tax_gid"].ToString();
                            }
                            if(tax_amount1== null) 
                            { 
                               tax_amount1 = dt1["tax_amount"].ToString();
                            }
                            else if (tax_amount2 == null)
                            {
                             tax_amount2 = dt1["tax_amount"].ToString();
                            }

                        msSQL = " select product_price from pmr_trn_tratecontract a " +
                                " left join pmr_trn_tratecontractdtl b on b.ratecontract_gid = a.ratecontract_gid" +
                                " where a.vendor_gid = '" + values.vendor_gid + "' and b.product_gid = '" + values.contractpo_list[i].product_gid + "'";
                             lsproductprice = objdbconn.GetExecuteScalar(msSQL);
                             productprice = Convert.ToDouble(lsproductprice);
                             qty = values.contractpo_list[i].qty_ordered;
                             totalqty = Convert.ToDouble(qty);
                             totalamount = (productprice * totalqty);
                             taxpercentage1 = Convert.ToDouble(tax_percentage1);
                             taxamount1 = (totalamount * taxpercentage1) / 100;
                             taxpercentage2 = Convert.ToDouble(tax_percentage2);
                             taxamount2 = (totalamount * taxpercentage2) / 100;
                             producttotal = (totalamount + taxamount1 + taxamount2);
                        }
                    }
                    else
                    {
                        msSQL = " select product_price from pmr_trn_tratecontract a " +
                                " left join pmr_trn_tratecontractdtl b on b.ratecontract_gid = a.ratecontract_gid" +
                                " where a.vendor_gid = '" + values.vendor_gid + "' and b.product_gid = '" + values.contractpo_list[i].product_gid + "'";
                        lsproductprice = objdbconn.GetExecuteScalar(msSQL);
                        productprice = Convert.ToDouble(lsproductprice);
                        qty = values.contractpo_list[i].qty_ordered;
                        totalqty = Convert.ToDouble(qty);
                        totalamount = (productprice * totalqty);
                        producttotal = (totalamount + taxamount1 + taxamount2);
                        
                    }
                    msSQL = " select poapproval_flag from adm_mst_tcompany ";
                    string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);
                    if (msPO1GetGID == null)
                    {
                        msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                        msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP", "", user_gid);
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
                                 " requested_by, " +
                                 " currency_code," +
                                 " purchaseorder_status, " +
                                 " purchaseorder_flag, " +
                                 " poref_no, " +
                                 " tax_gid," +
                                 " tax_percentage," +
                                 " tax_amount," +
                                 " taxsegment_gid, " +
                                 " expected_date, " +
                                 " purchaseorder_remarks, " +
                                 " po_type, " +
                                 " termsandconditions, " +
                                 " shipping_address " +
                                 " ) values (" +
                                 "'" + msPO1GetGID + "'," +
                                 "'" + msGetGID + "'," +
                                 "'" + msPO1GetGID + "'," +
                                 "'" + po_date + "', " +
                                 "'" + values.branch_gid + "'," +
                                 "'" + user_gid + "'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                 "'" + values.vendor_gid + "'," +
                                 "'" + vendor_address + "'," +
                                 "'" + user_gid + "'," +
                                 "'" + lscurrency_code + "',";
                                 if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                                {
                                    msSQL += "'PO Approved',";
                                }
                                else
                                {
                                    msSQL += "'Operation Pending PO',";
                                }
                                msSQL += "'PO Approved'," +
                                         "'Purchase'," + 
                                         "'" + tax_gid + "',";
                                if (string.IsNullOrEmpty(tax_percentage))
                                {
                                    msSQL += "'0.00',";
                                }
                                else
                                {
                                    msSQL += "'" + tax_percentage + "',";
                                }
                                if (string.IsNullOrEmpty(tax_amount))
                                {
                                    msSQL += "'0.00',";
                                }
                                else
                                {
                                    msSQL += "'" + tax_amount + "',";
                                }
                                msSQL += "'" + taxsegment_gid + "'," +
                                        "'" + expected_date + "'," +
                                        "'" + values.po_covernote + "'," +
                                        "'Contract PO'," +
                                        "'" + values.template_content + "'," +
                                        "'" + values.shipping_address + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
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
                                " tax1_gid, " +
                                " tax2_gid, " +
                                " qty_ordered, " +
                                " product_price_L, " +
                                " display_field_name," +
                                " tax_amount1_L," +
                                " tax_amount2_L," +
                                " tax_amount," +
                                " tax_amount2," +
                                " tax_percentage," +
                                " tax_percentage2," +
                                " taxsegment_gid" +
                                " )values ( " +
                                "'" + msPOGetGID + "', " +
                                "'" + msPO1GetGID + "'," +
                                "'" + values.contractpo_list[i].product_gid + "', " +
                                "'" + values.contractpo_list[i].product_code + "', " +
                                "'" + values.contractpo_list[i].product_name + "', " +
                                "'" + values.contractpo_list[i].productuom_name + "', " +
                                "'" + values.contractpo_list[i].productuom_gid + "', " +
                                "'" + values.contractpo_list[i].producttype_gid + "', " +
                                "'" + values.contractpo_list[i].product_price + "', " +
                                "'" + producttotal + "', ";
                    if (string.IsNullOrEmpty(values.contractpo_list[i].discount_percentage))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.contractpo_list[i].discount_percentage + "',";
                    }


                    if (string.IsNullOrEmpty(values.contractpo_list[i].discount_amount))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.contractpo_list[i].discount_amount + "',";
                    }
                    msSQL += "'" + tax_name1 + "', " +
                    "'" + tax_name2 + "', " +
                    "'" + tax_gid1 + "', " +
                    "'" + tax_gid2 + "', " +
                    "'" + values.contractpo_list[i].qty_ordered + "', " +
                    "'" + values.contractpo_list[i].product_price + "', " +
                    "'" + values.contractpo_list[i].display_field_name + "'," +
                    "'" + taxamount1 + "'," +
                    "'" + taxamount2 + "'," +
                    "'" + taxamount1 + "'," +
                    "'" + taxamount2 + "',"+
                    "'" + taxpercentage1 + "',"+
                    "'" + taxpercentage2 + "',"+
                    "'" + taxsegment_gid + "')";
                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult1 != 0)
                    {
                        tax_name1 = null;
                        tax_name2= null;
                        tax_percentage1 = null;
                        tax_percentage2 = null;
                        tax_gid1 = null;
                        tax_gid2 = null;
                        tax_amount1 = null;
                        tax_amount2 = null;
                    }
                }
                if (mnResult1 != 0)
                {
                    msSQL = "select sum(producttotal_amount) as net_amount from pmr_trn_tpurchaseorderdtl where purchaseorder_gid ='"+ msPO1GetGID + "'";
                    lsnet_amount=objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " update pmr_trn_tpurchaseorder set " +
                           " total_amount = '" + lsnet_amount + "'," +
                           " netamount = '" + lsnet_amount + "'" +
                           " where purchaseorder_gid='" + msPO1GetGID + "'";
                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult2 != 0)
                    {
                        values.status = true;
                        values.message = "Purchase Order Raised Successfully!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding product in Purchase Order!";
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

        public void DaOperationalApprovalPO(string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                msSQL = " update pmr_trn_tpurchaseorder set purchaseorder_status='Operational Approved PO, Finance Pending PO'," +
                    " purchaseorder_flag='Operational PO Approved,Finance PO Pending' " +
                    "where purchaseorder_gid='" + purchaseorder_gid+ "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = " Purchase order approved successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while approve purchase order.";
                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while adding product in Purchase Order!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        } 

        public void DaOperationalRejectPO(string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                msSQL = " update pmr_trn_tpurchaseorder set purchaseorder_status='Operational Rejected PO , Finance Pending PO'," +                    
                    " purchaseorder_flag='Operational PO Rejected,Finance PO Pending' " +
                    "where purchaseorder_gid='" + purchaseorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "PO Rejected.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while Reject PO.";
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
        public void DaFinanceApprovalPO(string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                msSQL = " update pmr_trn_tpurchaseorder set purchaseorder_status='Operational Approved PO, Finance Approved PO'," +
                    " purchaseorder_flag='Operational Approved PO, Finance Approved PO' " +
                    " where purchaseorder_gid='" + purchaseorder_gid+ "'";                    
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = " Purchase order approved successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while approve purchase order.";
                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while adding product in Purchase Order!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        } 
        public void DaFinanceRejectPO(string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                msSQL = " update pmr_trn_tpurchaseorder set purchaseorder_status='Operational Approved PO, Finance Rejected PO'," +
                    " purchaseorder_flag='Operational Approved PO,Finance Rejected PO' " +
                    "where purchaseorder_gid='" + purchaseorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = " PO Rejected.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while Reject PO.";
                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while adding product in Purchase Order!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaFinanceApprovalSummary(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                msSQL = " SELECT /*+ MAX_EXECUTION_TIME(600000) */ DISTINCT h.costcenter_name,a.purchaseorder_gid, " +
                        " b.vendor_code,CONCAT(b.contactperson_name, ' / ', b.email_id, ' / ', b.contact_telephonenumber) AS Contact, " +
                        " CONCAT(b.vendor_code, '/', b.vendor_companyname) AS Vendor, " +
                        " IF(a.poref_no IS NULL, a.purchaseorder_gid, " +
                        " IF(a.poref_no = '', a.purchaseorder_gid, " +
                        " IF(a.poref_no = a.purchaseorder_gid, a.purchaseorder_gid, CONCAT(a.purchaseorder_gid, '/', a.poref_no)))) AS porefno, " +
                        " a.poref_no,a.purchaseorder_remarks,a.purchaseorder_status, " +
                        " FORMAT(a.total_amount, 2) AS total_amount, DATE_ADD(a.purchaseorder_date, INTERVAL delivery_days DAY) AS ExpectedPODeliveryDate, " +
                        " FORMAT(a.total_amount, 2) AS paymentamount, DATE_FORMAT(a.purchaseorder_date, '%d-%m-%Y') AS purchaseorder_date, a.vendor_status, " +
                        " CASE WHEN a.invoice_flag<> 'IV Pending' THEN a.invoice_flag " +
                        " WHEN a.grn_flag<> 'GRN Pending' THEN a.grn_flag ELSE a.purchaseorder_flag " +
                        " END AS 'overall_status',a.purchaseorder_flag,a.grn_flag,a.invoice_flag,b.vendor_companyname,c.branch_name, " +
                        " CASE WHEN GROUP_CONCAT(DISTINCT purchaserequisition_referencenumber) = ',' THEN '' " +
                        " WHEN GROUP_CONCAT(DISTINCT purchaserequisition_referencenumber) <> ',' THEN GROUP_CONCAT(DISTINCT purchaserequisition_referencenumber) " +
                        " END AS refrence_no,bscc_flag,po_type,DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date, CONCAT(d.user_firstname, ' ', d.user_lastname) AS created_by " +
                        " FROM pmr_trn_tpurchaseorder a " +
                        " LEFT JOIN acp_mst_tvendor b ON b.vendor_gid = a.vendor_gid " +
                        " LEFT JOIN hrm_mst_tbranch c ON c.branch_gid = a.branch_gid " +
                        " LEFT JOIN adm_mst_tuser d ON d.user_gid = a.created_by " +
                        " LEFT JOIN hrm_mst_temployee e ON e.user_gid = d.user_gid " +
                        " LEFT JOIN adm_mst_tmodule2employee f ON f.employee_gid = e.employee_gid " +
                        " LEFT JOIN pmr_mst_tcostcenter h ON h.costcenter_gid = a.costcenter_gid " +
                        " LEFT JOIN pmr_Trn_tpr2po i ON i.purchaseorder_gid = a.purchaseorder_gid " +
                        " LEFT JOIN pmr_Trn_tpurchaserequisition j ON j.purchaserequisition_gid = i.purchaserequisition_gid " +
                        " LEFT JOIN crm_trn_tcurrencyexchange k ON k.currencyexchange_gid = a.currency_code " +
                        " WHERE a.purchaseorder_status = 'Operational Approved PO, Finance Pending PO' " +
                        " GROUP BY a.purchaseorder_gid " +
                        " ORDER BY DATE(a.purchaseorder_date) DESC,a.purchaseorder_date ASC, a.purchaseorder_gid DESC, b.vendor_companyname DESC";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPurchaseOrder_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPurchaseOrder_lists
                        {

                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            ExpectedPODeliveryDate = dt["ExpectedPODeliveryDate"].ToString(),
                            porefno = dt["porefno"].ToString(),
                            poref_no = dt["poref_no"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            remarks = dt["purchaseorder_remarks"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            purchaseorder_status = dt["purchaseorder_status"].ToString(),
                            vendor_status = dt["vendor_status"].ToString(),
                            paymentamount = dt["paymentamount"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            Vendor = dt["Vendor"].ToString(),
                            Contact = dt["Contact"].ToString(),

                        });
                        values.GetPurchaseOrder_lists = getModuleList;
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

        public void DaGetOpertionalApprovalSummary(MdlPmrTrnPurchaseOrder values)
        {
            try
            {


                msSQL =" SELECT /*+ MAX_EXECUTION_TIME(600000) */ DISTINCT h.costcenter_name,a.purchaseorder_gid, " +
                        " b.vendor_code,CONCAT(b.contactperson_name, ' / ', b.email_id, ' / ', b.contact_telephonenumber) AS Contact, " +
                        " CONCAT(b.vendor_code, '/', b.vendor_companyname) AS Vendor, " +
                        " IF(a.poref_no IS NULL, a.purchaseorder_gid, IF(a.poref_no = '', a.purchaseorder_gid, IF(a.poref_no = a.purchaseorder_gid, a.purchaseorder_gid, " +
                        " CONCAT(a.purchaseorder_gid, '/', a.poref_no)))) AS porefno, a.poref_no,a.purchaseorder_remarks,a.purchaseorder_status, " +
                        " FORMAT(a.total_amount, 2) AS total_amount, DATE_ADD(a.purchaseorder_date, INTERVAL delivery_days DAY) AS ExpectedPODeliveryDate, " +
                        " FORMAT(a.total_amount, 2) AS paymentamount, DATE_FORMAT(a.purchaseorder_date, '%d-%m-%Y') AS purchaseorder_date, a.vendor_status, " +
                        " CASE WHEN a.invoice_flag<> 'IV Pending' THEN a.invoice_flag " +
                        " WHEN a.grn_flag<> 'GRN Pending' THEN a.grn_flag " +
                        " ELSE a.purchaseorder_flag END AS 'overall_status',a.purchaseorder_flag,a.grn_flag,a.invoice_flag, " +
                        " b.vendor_companyname,c.branch_name, " +
                        " CASE WHEN GROUP_CONCAT(DISTINCT purchaserequisition_referencenumber) = ',' THEN '' " +
                        " WHEN GROUP_CONCAT(DISTINCT purchaserequisition_referencenumber) <> ',' THEN GROUP_CONCAT(DISTINCT purchaserequisition_referencenumber) " +
                        " END AS refrence_no,bscc_flag,po_type,DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date, CONCAT(d.user_firstname, ' ', d.user_lastname) AS created_by " +
                        " FROM pmr_trn_tpurchaseorder a " +
                        " LEFT JOIN acp_mst_tvendor b ON b.vendor_gid = a.vendor_gid " +
                        " LEFT JOIN hrm_mst_tbranch c ON c.branch_gid = a.branch_gid " +
                        " LEFT JOIN adm_mst_tuser d ON d.user_gid = a.created_by " +
                        " LEFT JOIN hrm_mst_temployee e ON e.user_gid = d.user_gid " +
                        " LEFT JOIN adm_mst_tmodule2employee f ON f.employee_gid = e.employee_gid " +
                        " LEFT JOIN pmr_mst_tcostcenter h ON h.costcenter_gid = a.costcenter_gid " +
                        " LEFT JOIN pmr_Trn_tpr2po i ON i.purchaseorder_gid = a.purchaseorder_gid " +
                        " LEFT JOIN pmr_Trn_tpurchaserequisition j ON j.purchaserequisition_gid = i.purchaserequisition_gid " +
                        " LEFT JOIN crm_trn_tcurrencyexchange k ON k.currencyexchange_gid = a.currency_code " +
                        " WHERE a.purchaseorder_status IN('Operation Pending PO', 'Cancel PO', 'Reject PO') " +
                        " GROUP BY a.purchaseorder_gid " +
                        " ORDER BY DATE(a.purchaseorder_date) DESC,a.purchaseorder_date ASC, a.purchaseorder_gid DESC, b.vendor_companyname DESC ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPurchaseOrder_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPurchaseOrder_lists
                        {

                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            ExpectedPODeliveryDate = dt["ExpectedPODeliveryDate"].ToString(),
                            porefno = dt["porefno"].ToString(),
                            poref_no = dt["poref_no"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            remarks = dt["purchaseorder_remarks"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            purchaseorder_status = dt["purchaseorder_status"].ToString(),
                            vendor_status = dt["vendor_status"].ToString(),
                            paymentamount = dt["paymentamount"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            Vendor = dt["Vendor"].ToString(),
                            Contact = dt["Contact"].ToString(),

                        });
                        values.GetPurchaseOrder_lists = getModuleList;
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

        public void DaGetproductdeletetemp(string user_gid) 
        {
            try
            {

                msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0) { }

               
            }

            catch (Exception ex)
            {
                
            }


        }

        public void DaGetVendoremail(string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = "select b.email_id from pmr_trn_tpurchaseorder a left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid  where purchaseorder_gid  = '" + purchaseorder_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMailId_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetMailId_list
                        {
                            vendor_emailid = dt["email_id"].ToString(),

                        });
                        values.GetMailId_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Mail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        //Minsoft Api code by snehith for create asn
        public result DaCreateASN(string purchaseorder_gid)
        {
            result objresult = new result();
            ASNList objMdlMintsoftJSON = new ASNList();
            try
            {
                msSQL = "select poref_no,expected_date from pmr_trn_tpurchaseorder " +
                        "where purchaseorder_gid = '" + purchaseorder_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    string expecteddate = objOdbcDataReader["expected_date"].ToString() + DateTimeOffset.Now.ToString("THH:mm:ss.fffffffK");
                    objMdlMintsoftJSON.WarehouseId = 3;
                    objMdlMintsoftJSON.POReference = objOdbcDataReader["poref_no"].ToString();
                    objMdlMintsoftJSON.EstimatedDelivery = expecteddate;
                    objMdlMintsoftJSON.GoodsInType = "TwentyFtContainer";
                    objOdbcDataReader.Close();
                }

                msSQL = "select b.customerproduct_code,a.qty_ordered,b.mintsoftproduct_id" +
                        "from pmr_trn_tpurchaseorderdtl a " +
                        "left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        "where salesorder_gid = '" + purchaseorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    int i = 0;
                    objMdlMintsoftJSON.Items = new AsnItem[dt_datatable.Rows.Count];

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        objMdlMintsoftJSON.Items[i] = new AsnItem();
                        objMdlMintsoftJSON.Items[i].ProductId = dt["mintsoftproduct_id"].ToString();
                        objMdlMintsoftJSON.Items[i].SKU = dt["customerproduct_code"].ToString();
                        objMdlMintsoftJSON.Items[i].Quantity = Convert.ToInt32(dt["qty_ordered"].ToString());
                        i++;
                    }
                    dt_datatable.Dispose();
                }
                string json = JsonConvert.SerializeObject(objMdlMintsoftJSON);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["MintSoftBaseURL"].ToString());
                var request = new RestRequest("/api/ASN", Method.PUT);
                request.AddHeader("ms-apikey", ConfigurationManager.AppSettings["MintSoftAccessToken"].ToString());
                request.AddParameter("application/json", json);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseData = JsonConvert.DeserializeObject<MintsoftASNResponse>(response.Content);
                    msSQL = "update pmr_trn_tpurchaseorder set mintsoftasn_id = '" + responseData.ID + "' where purchaseorder_gid ='" + purchaseorder_gid + "'";
                    int mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        objresult.status = true;
                        objresult.message = "ASN Added Successfully";
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                       "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        objresult.status = false;
                        objresult.message = "Error While Creating ASN";
                    }
                }
                else
                {
                    string errorMessage = $"Failed to fetch products. Status code: {response.StatusCode}, Reason: {response.ErrorMessage}";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + errorMessage, "ErrorLog/Purchase/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    objresult.status = false;
                    objresult.message = "Error While Creating ASN";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Exception occured while posting to Mintsoft!";
            }
            return objresult;
        }
        public class MintsoftASNResponse
        {
            public string ID { get; set; }
            public string Success { get; set; }
            public string Message { get; set; }
            public string WarningMessage { get; set; }
            public string AllocatedFromReplen { get; set; }
        }

        public void daGetpurchaseordersixmonthschart(MdlPmrTrnPurchaseOrder values)
        {


            msSQL = "  select DATE_FORMAT(purchaseorder_date, '%b-%Y')  as purchaseorder_date," +
                " substring(date_format(a.purchaseorder_date,'%M'),1,3)as month," +
                " a.purchaseorder_gid,year(a.purchaseorder_date) as year," +
                " format(round(sum(a.total_amount),2),2)as amount," +
                " round(sum(a.total_amount),2) as amount1," +
                " count(a.purchaseorder_gid)as ordercount ," +
                " date_format(purchaseorder_date,'%M/%Y') as month_wise" +
                " from pmr_trn_tpurchaseorder a where a.purchaseorder_date > date_add(now(), interval-6 month)" +
                " and a.purchaseorder_date<=date(now())and a.purchaseorder_status not in('PO Amended','PO Canceled','Rejected')" +
                " group by date_format(a.purchaseorder_date,'%M') order by a.purchaseorder_date desc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var purchaseorderlastsixmonths_list = new List<purchaseorderlastsixmonths_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    purchaseorderlastsixmonths_list.Add(new purchaseorderlastsixmonths_list
                    {
                        purchaseorder_date = (dt["purchaseorder_date"].ToString()),
                        months = (dt["month"].ToString()),
                        orderamount = (dt["amount1"].ToString()),

                    });
                    values.purchaseorderlastsixmonths_list = purchaseorderlastsixmonths_list;
                }

            }

            msSQL = "select COUNT(CASE WHEN a.purchaseorder_status = 'PO Completed' THEN 1 END) AS invoice_count," +
                    " COUNT(a.purchaseorder_gid) AS approved_count FROM pmr_trn_tpurchaseorder a" +
                    " WHERE  a.purchaseorder_date > DATE_ADD(NOW(), INTERVAL -6 MONTH)  AND a.purchaseorder_date <= DATE(NOW())";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                values.ordertoinvoicecount = objOdbcDataReader["invoice_count"].ToString();
                values.ordercount = objOdbcDataReader["approved_count"].ToString();
            }

        }
        public void DaPostPurchaseOrderfileupload(HttpRequest httpRequest, result objResult, string user_gid)
        {
            try
            {

                //var POProductList = httpRequest.Form["POProductList"].ToString();
                var branchName = httpRequest.Form["branch_name"].ToString();
                var branchGid = httpRequest.Form["branch_gid"].ToString();
                var poNo = httpRequest.Form["po_no"].ToString();
                var poDate = httpRequest.Form["po_date"].ToString();
                var expectedDate = httpRequest.Form["expected_date"].ToString();
                var vendorGid = httpRequest.Form["vendor_gid"].ToString();
                var vendorCompanyName = httpRequest.Form["vendor_companyname"].ToString();
                var vendorDetails = httpRequest.Form["vendor_details"].ToString();
                var address1 = httpRequest.Form["address1"].ToString();
                var employeeName = httpRequest.Form["employee_name"].ToString();
                var deliveryTerms = httpRequest.Form["delivery_terms"].ToString();
                var paymentTerms = httpRequest.Form["payment_terms"].ToString();
                var requestorDetails = httpRequest.Form["Requestor_details"].ToString();
                var dispatchMode = httpRequest.Form["dispatch_mode"].ToString();
                var currencyGid = httpRequest.Form["currency_gid"].ToString();
                var currencyCode = httpRequest.Form["currency_code"].ToString();
                var exchangeRate = httpRequest.Form["exchange_rate"].ToString();
                var poCoverNote = httpRequest.Form["po_covernote"].ToString();
                var templateName = httpRequest.Form["template_name"].ToString();
                var templateContent = httpRequest.Form["template_content"].ToString();
                var templateGid = httpRequest.Form["template_gid"].ToString();
                var totalAmount = httpRequest.Form["totalamount"].ToString();
                var addonCharge = httpRequest.Form["addoncharge"].ToString();
                var additionalDiscount = httpRequest.Form["additional_discount"].ToString();
                var freightCharges = httpRequest.Form["freightcharges"].ToString();
                var roundOff = httpRequest.Form["roundoff"].ToString();
                var grandTotal = httpRequest.Form["grandtotal"].ToString();
                var taxGid = httpRequest.Form["tax_gid"].ToString();
                var taxName1 = httpRequest.Form["tax_name1"].ToString();
                var taxName2 = httpRequest.Form["tax_name2"].ToString();
                var taxName3 = httpRequest.Form["tax_name3"].ToString();
                var taxAmount1 = httpRequest.Form["taxamount1"].ToString();
                var taxAmount2 = httpRequest.Form["taxamount2"].ToString();
                var taxAmount3 = httpRequest.Form["taxamount3"].ToString();
                var taxAmount4 = httpRequest.Form["tax_amount4"].ToString();
                var taxName4 = httpRequest.Form["tax_name4"].ToString();
                var taxSegmentGid = httpRequest.Form["taxsegment_gid"].ToString();
                var shippingAddress = httpRequest.Form["shipping_address"].ToString();
                var renewal_mode = httpRequest.Form["renewal_flag"].ToString();
                var frequency_terms = httpRequest.Form["frequency_terms"].ToString();
                string renewal_date = httpRequest.Form["renewal_date"].ToString();
                var purchaseorder_gid = httpRequest.Form["purchaseorder_gid"].ToString();


                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                MemoryStream ms_stream = new MemoryStream();
                string document_gid = string.Empty;
                string lscompany_code = string.Empty;
                HttpPostedFile httpPostedFile;
                string lspath;
                string final_path = "";
                string vessel_name = "";



                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }

                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        FileExtensionname = httpPostedFile.FileName;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        ms.Close();
                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        final_path = lspath + msdocument_gid + FileExtension;

                    }
                }

                if (string.IsNullOrEmpty(poNo))
                {
                    int lsfreight = 0;
                    int lsinsurance = 0;
                    string uiDateStr = poDate;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string po_date = uiDate.ToString("yyyy-MM-dd");

                    string uiDateStr2 = expectedDate;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string expected_date = uiDate2.ToString("yyyy-MM-dd");

                    string lstax_gid;

                    msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                    msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP");
                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + currencyCode + "' ";
                    string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select poapproval_flag from adm_mst_tcompany";
                    string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                    if (string.IsNullOrEmpty(taxName4))
                    {
                        lstaxpercentage = "0";

                    }
                    else
                    {
                        msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + taxName4 + "'";
                        lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                    }

                    msSQL = " insert into pmr_trn_tpurchaseorder (" +
                         " purchaseorder_gid, " +
                         " purchaserequisition_gid, " +
                         " purchaseorder_reference, " +
                         " purchaseorder_date," +
                         " expected_date," +
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
                         " file_name, " +
                         " file_path, " +
                         " renewal_flag ," +
                         " taxsegment_gid " +
                         " ) values (" +
                         "'" + msPO1GetGID + "'," +
                         "'" + msGetGID + "'," +
                         "'" + msPO1GetGID + "'," +
                         "'" + po_date + "', " +
                         "'" + expected_date + "', " +
                         "'" + branchName + "'," +
                         "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                         "'" + vendorCompanyName + "',";

                    //"'" + values.address1.Trim().Replace("'", "") + "',";

                    if (!string.IsNullOrEmpty(address1) && address1.Contains("'"))
                    {
                        msSQL += "'" + address1.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + address1 + "', ";
                    }

                    //"'" + values.shipping_address.Trim().Replace("'", "") + "'," +

                    if (!string.IsNullOrEmpty(shippingAddress) && shippingAddress.Contains("'"))
                    {
                        msSQL += "'" + shippingAddress.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + shippingAddress + "', ";
                    }
                    msSQL += "'" + employeeName + "'," +
                          "'" + deliveryTerms + "'," +
                          "'" + paymentTerms + "'," +
                          "'" + requestorDetails + "',";
                    if (!string.IsNullOrEmpty(dispatchMode) && dispatchMode.Contains("'"))
                    {
                        msSQL += "'" + dispatchMode.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + dispatchMode + "', ";
                    }
                    msSQL += "'" + lscurrency_code + "'," +
                          "'" + exchangeRate + "',";
                    if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                    {
                        msSQL += "'" + poCoverNote.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + poCoverNote + "', ";
                    }
                    if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                    {
                        msSQL += "'" + poCoverNote.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + poCoverNote + "', ";
                    }
                    msSQL +="'" + grandTotal + "',";
                    if (!string.IsNullOrEmpty(templateContent) && templateContent.Contains("'"))
                    {
                        msSQL += "'" + templateContent.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + templateContent + "', ";
                    }
                    if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                    {
                        msSQL += "'PO Approved',";
                    }
                    else
                    {
                        msSQL += "'Operation Pending PO',";
                    }
                    msSQL += "'" + "PO Approved" + "',";
                            //"'" + poNo + "'," +
                    if (poNo == "")
                    {
                        poNo = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", branchName);
                        msSQL += "'" + poNo + "',";
                    }
                    else
                    {
                        msSQL += "'" + poNo + "',";
                    }
                    msSQL += "'" + totalAmount + "'," +
                         "'" + addonCharge + "'," +
                         "'" + freightCharges + "',";
                    if (string.IsNullOrEmpty(additionalDiscount))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + additionalDiscount + "',";
                    }
                    msSQL += "'" + taxName4 + "'," +
                    "'" + lstaxpercentage + "',";

                    if (string.IsNullOrEmpty(taxAmount4))
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + taxAmount4 + "',";
                    }
                    if (roundOff == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + roundOff + "',";
                    }

                    msSQL += "'" + FileExtensionname + "'," +
                             "'" + final_path + "'," +
                               "'" + renewal_mode + "'," +
                             "'" + taxSegmentGid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (renewal_mode == "Y")
                    {
                        string uiDateStr5 = renewal_date;
                        DateTime uiDate5 = DateTime.ParseExact(uiDateStr5, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        string renewaldate = uiDate5.ToString("yyyy-MM-dd");
                        msINGetGID = objcmnfunctions.GetMasterGID("BRLP");

                        msSQL = " Insert into pmr_trn_trenewal ( " +
                                " renewal_gid, " +
                                " renewal_flag, " +
                                " frequency_term, " +
                                " vendor_gid," +
                                " renewal_date, " +
                                " purchaseorder_gid, " +
                                " created_by, " +
                                " renewal_type, " +
                                " created_date) " +
                               " Values ( " +
                                 "'" + msINGetGID + "'," +
                                 "'" + renewal_mode + "'," +
                                 "'" + frequency_terms + "'," +
                                 "'" + vendorCompanyName + "'," +
                                 "'" + renewaldate + "'," +
                                 "'" + msPO1GetGID + "'," +
                                 "'" + user_gid + "'," +
                                 "'Purchase'," +
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
                                msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
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
                                            "'" + msPOGetGID + "', " +
                                            "'" + msPO1GetGID + "'," +
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

                        if (mnResult != 0)

                        {
                            objResult.status = true;
                            objResult.message = "Purchase Order Raised Successfully!";
                            if (!string.IsNullOrEmpty(purchaseorder_gid))
                            {
                                msSQL = "  delete from pmr_trn_tpurchaseorderdraft where purchaseorderdraft_gid='" + purchaseorder_gid + "'  ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL = "  delete from pmr_trn_tpurchaseorderdraftdtl where purchaseorderdraft_gid='" + purchaseorder_gid + "'  ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            }
                            else
                            {
                                
                            }



                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Error While Adding product in Purchase Order!";


                        }

                        msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        dt_datatable.Dispose();
                    }
                }


                else
                {

                    msSQL = "select poref_no from pmr_trn_tpurchaseorder where poref_no ='" + poNo + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        objResult.message = "PO Ref No Alredy Exist!";
                        return;
                    }
                    else
                    {
                        int lsfreight = 0;
                        int lsinsurance = 0;
                        string uiDateStr = poDate;
                        DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string po_date = uiDate.ToString("yyyy-MM-dd");

                        string uiDateStr2 = expectedDate;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string expected_date = uiDate2.ToString("yyyy-MM-dd");

                        string lstax_gid;

                        msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                        msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP");
                        msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + currencyCode + "' ";
                        string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select poapproval_flag from adm_mst_tcompany";
                        string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                        if (string.IsNullOrEmpty(taxName4))
                        {
                            lstaxpercentage = "0";

                        }
                        else
                        {
                            msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + taxName4 + "'";
                            lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                        }

                        msSQL = " insert into pmr_trn_tpurchaseorder (" +
                             " purchaseorder_gid, " +
                             " purchaserequisition_gid, " +
                             " purchaseorder_reference, " +
                             " purchaseorder_date," +
                             " expected_date," +
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
                             " file_path, " +
                             " file_name, " +
                             " taxsegment_gid " +
                             " ) values (" +
                             "'" + msPO1GetGID + "'," +
                             "'" + msGetGID + "'," +
                             "'" + msPO1GetGID + "'," +
                             "'" + po_date + "', " +
                             "'" + expected_date + "', " +
                             "'" + branchName + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'" + vendorCompanyName + "',";

                        //"'" + values.address1.Trim().Replace("'", "") + "',";

                        if (!string.IsNullOrEmpty(address1) && address1.Contains("'"))
                        {
                            msSQL += "'" + address1.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + address1 + "', ";
                        }

                        //"'" + values.shipping_address.Trim().Replace("'", "") + "'," +

                        if (!string.IsNullOrEmpty(shippingAddress) && shippingAddress.Contains("'"))
                        {
                            msSQL += "'" + shippingAddress.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + shippingAddress + "', ";
                        }
                        msSQL += "'" + employeeName + "'," +
                              "'" + deliveryTerms + "'," +
                              "'" + paymentTerms + "'," +
                              "'" + requestorDetails + "',";
                        if (!string.IsNullOrEmpty(dispatchMode) && dispatchMode.Contains("'"))
                        {
                            msSQL += "'" + dispatchMode.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + dispatchMode + "', ";
                        }
                        msSQL += "'" + lscurrency_code + "'," +
                              "'" + exchangeRate + "',"; 
                        if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                        {
                            msSQL += "'" + poCoverNote.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + poCoverNote + "', ";
                        }
                        if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                        {
                            msSQL += "'" + poCoverNote.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + poCoverNote + "', ";
                        }
                         msSQL +="'" + grandTotal + "',";
                        
                        if (!string.IsNullOrEmpty(templateContent) && templateContent.Contains("'"))
                        {
                            msSQL += "'" + templateContent.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + templateContent + "', ";
                        }
                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "'PO Approved',";
                        }
                        else
                        {
                            msSQL += "'Operation Pending PO',";
                        }
                        msSQL += "'" + "PO Approved" + "'," +
                             "'" + poNo + "'," +
                              "'" + totalAmount + "'," +
                             "'" + addonCharge + "'," +
                             "'" + freightCharges + "',";
                        if (string.IsNullOrEmpty(additionalDiscount))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + additionalDiscount + "',";
                        }
                        msSQL += "'" + taxName4 + "',";
                        if (string.IsNullOrEmpty(lstaxpercentage))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + lstaxpercentage + "',";
                        }

                        if (string.IsNullOrEmpty(taxAmount4))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + taxAmount4 + "',";
                        }
                        if (roundOff == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + roundOff + "',";
                        }
                        msSQL += "'" + final_path + "'," +
                                 "'" + FileExtensionname + "'," +
                                 "'" + taxSegmentGid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (renewal_mode == "Y")
                        {
                            string uiDateStr5 = renewal_date;
                            DateTime uiDate5 = DateTime.ParseExact(uiDateStr5, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string renewaldate = uiDate5.ToString("yyyy-MM-dd");
                            msINGetGID = objcmnfunctions.GetMasterGID("BRLP");

                            msSQL = " Insert into pmr_trn_trenewal ( " +
                                    " renewal_gid, " +
                                    " renewal_flag, " +
                                    " frequency_term, " +
                                    " vendor_gid," +
                                    " renewal_date, " +
                                    " purchaseorder_gid, " +
                                    " created_by, " +
                                    " renewal_type, " +
                                    " created_date) " +
                                   " Values ( " +
                                     "'" + msINGetGID + "'," +
                                     "'" + renewal_mode + "'," +
                                     "'" + frequency_terms + "'," +
                                     "'" + vendorCompanyName + "'," +
                                     "'" + renewaldate + "'," +
                                     "'" + msPO1GetGID + "'," +
                                     "'" + user_gid + "'," +
                                     "'Purchase'," +
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
                                    msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
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
                                                "'" + msPOGetGID + "', " +
                                                "'" + msPO1GetGID + "'," +
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

                            if (mnResult != 0)

                            {
                                objResult.status = true;
                                objResult.message = "Purchase Order Raised Successfully!";



                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Adding product in Purchase Order!";


                            }

                            msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            dt_datatable.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Submitting Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }



        public void DaPostPurchaseOrderfileuploaddraft(HttpRequest httpRequest, result objResult, string user_gid)
        {
            try
            {
                //var POProductList = httpRequest.Form["POProductList"].ToString();
                var branchName = httpRequest.Form["branch_name"].ToString();
                var branchGid = httpRequest.Form["branch_gid"].ToString();
                var poNo = httpRequest.Form["po_no"].ToString();
                var poDate = httpRequest.Form["po_date"].ToString();
                var expectedDate = httpRequest.Form["expected_date"].ToString();
                var vendorGid = httpRequest.Form["vendor_gid"].ToString();
                var vendorCompanyName = httpRequest.Form["vendor_companyname"].ToString();
                var vendorDetails = httpRequest.Form["vendor_details"].ToString().Replace("'", "\\\'");
                var address1 = httpRequest.Form["address1"].ToString().Replace("'", "\\\'");
                var employeeName = httpRequest.Form["employee_name"].ToString();
                var deliveryTerms = httpRequest.Form["delivery_terms"].ToString().Replace("'", "\\\'");
                var paymentTerms = httpRequest.Form["payment_terms"].ToString().Replace("'", "\\\'");
                var requestorDetails = httpRequest.Form["Requestor_details"].ToString().Replace("'", "\\\'");
                var dispatchMode = httpRequest.Form["dispatch_mode"].ToString();
                var currencyGid = httpRequest.Form["currency_gid"].ToString();
                var currencyCode = httpRequest.Form["currency_code"].ToString();
                var exchangeRate = httpRequest.Form["exchange_rate"].ToString();
                var poCoverNote = httpRequest.Form["po_covernote"].ToString().Replace("'", "\\\'");
                var templateName = httpRequest.Form["template_name"].ToString().Replace("'", "\\\'");
                var templateContent = httpRequest.Form["template_content"].ToString().Replace("'", "\\\'");
                var templateGid = httpRequest.Form["template_gid"].ToString();
                var totalAmount = httpRequest.Form["totalamount"].ToString();
                var addonCharge = httpRequest.Form["addoncharge"].ToString();
                var additionalDiscount = httpRequest.Form["additional_discount"].ToString();
                var freightCharges = httpRequest.Form["freightcharges"].ToString();
                var roundOff = httpRequest.Form["roundoff"].ToString();
                var grandTotal = httpRequest.Form["grandtotal"].ToString();
                var taxGid = httpRequest.Form["tax_gid"].ToString();
                var taxName1 = httpRequest.Form["tax_name1"].ToString();
                var taxName2 = httpRequest.Form["tax_name2"].ToString();
                var taxName3 = httpRequest.Form["tax_name3"].ToString();
                var taxAmount1 = httpRequest.Form["taxamount1"].ToString();
                var taxAmount2 = httpRequest.Form["taxamount2"].ToString();
                var taxAmount3 = httpRequest.Form["taxamount3"].ToString();
                var taxAmount4 = httpRequest.Form["tax_amount4"].ToString();
                var taxName4 = httpRequest.Form["tax_name4"].ToString();
                var taxSegmentGid = httpRequest.Form["taxsegment_gid"].ToString();
                var shippingAddress = httpRequest.Form["shipping_address"].ToString().Replace("'", "\\\'");
                var renewal_mode = httpRequest.Form["renewal_flag"].ToString();
                var frequency_terms = httpRequest.Form["frequency_terms"].ToString();
                var renewal_date = httpRequest.Form["renewal_date"].ToString();
                var purchaseorder_gid = httpRequest.Form["purchaseorder_gid"].ToString();

                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                MemoryStream ms_stream = new MemoryStream();
                string document_gid = string.Empty;
                string lscompany_code = string.Empty;
                HttpPostedFile httpPostedFile;
                string lspath;
                string final_path = "";
                string vessel_name = "";



                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }

                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        FileExtensionname = httpPostedFile.FileName;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        ms.Close();
                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        final_path = lspath + msdocument_gid + FileExtension;

                    }
                }

                if (string.IsNullOrEmpty(poNo))
                {
                    int lsfreight = 0;
                    int lsinsurance = 0;
                    string uiDateStr = poDate;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string po_date = uiDate.ToString("yyyy-MM-dd");
                    string uiDateStr2 = expectedDate;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string expected_date = uiDate2.ToString("yyyy-MM-dd");
                    string lstax_gid;

                    msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                    msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP", "", user_gid);
                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + currencyCode + "' ";
                    string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select poapproval_flag from adm_mst_tcompany";
                    string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                    if (string.IsNullOrEmpty(taxName4))
                    {
                        lstaxpercentage = "0";

                    }
                    else
                    {
                        msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + taxName4 + "'";
                        lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                    }
                    //-----------------------------------------------//
                    //msSQL = "select purchaseorderdraft_gid from pmr_trn_tpurchaseordefdraft  where purchaseorderdraft_gid='" + purchaseorder_gid + "'";

                    //string lstaxpono = objdbconn.GetExecuteScalar(msSQL);

                    if (!string.IsNullOrEmpty(purchaseorder_gid))

                    {

                        msSQL = "UPDATE pmr_trn_tpurchaseorderdraft SET " +

                                 "purchaserequisition_gid = '" + msGetGID + "', " +

                                 "purchaseorder_reference = '" + msPO1GetGID + "', " +

                                 "purchaseorder_date = '" + po_date + "', " +

                                 "expected_date = '" + expected_date + "', " +

                                 "branch_gid = '" + branchName + "', " +

                                 "created_by = '" + user_gid + "', " +

                                 "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +

                                 "vendor_gid = '" + vendorCompanyName + "', ";

                        // Handle address1 (replace ' if present)

                        if (!string.IsNullOrEmpty(address1) && address1.Contains("'"))

                        {

                            msSQL += "vendor_address = '" + address1.Replace("'", "") + "', ";

                        }

                        else

                        {

                            msSQL += "vendor_address = '" + address1 + "', ";

                        }

                        // Handle shipping address (replace ' if present)

                        if (!string.IsNullOrEmpty(shippingAddress) && shippingAddress.Contains("'"))

                        {

                            msSQL += "shipping_address = '" + shippingAddress.Replace("'", "\\\'") + "', ";

                        }

                        else

                        {

                            msSQL += "shipping_address = '" + shippingAddress + "', ";

                        }

                        msSQL +=

                            "requested_by = '" + employeeName + "', " +

                            "freight_terms = '" + deliveryTerms + "', " +

                            "payment_terms = '" + paymentTerms + "', " +

                            "requested_details = '" + requestorDetails + "', " +

                            "mode_despatch = '" + dispatchMode + "', " +

                            "currency_code = '" + currencyCode + "', " +

                            "exchange_rate = '" + exchangeRate + "', " +

                            "po_covernote = '" + poCoverNote.Replace("'", "\\\'") + "', " +

                            "purchaseorder_remarks = '" + poCoverNote.Replace("'", "\\\'") + "', " +

                            "total_amount = '" + grandTotal + "', ";

                        // Handle template_content

                        if (!string.IsNullOrEmpty(templateContent) && templateContent.Contains("'"))

                        {

                            msSQL += "termsandconditions = '" + templateContent.Replace("'", "\\\'") + "', ";

                        }

                        else

                        {

                            msSQL += "termsandconditions = '" + templateContent + "', ";

                        }

                        // Approval flag logic

                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)

                        {

                            msSQL += "purchaseorder_status = 'PO Approved', ";

                        }

                        else

                        {

                            msSQL += "purchaseorder_status = 'Operation Pending PO', ";

                        }

                        msSQL += "purchaseorder_flag = 'PO Approved', ";


                        msSQL += "poref_no = '" + poNo + "', ";


                        msSQL += "netamount = '" + totalAmount + "', " +

                            "addon_amount = '" + addonCharge + "', " +

                            "freightcharges = '" + freightCharges + "', ";

                        // Handle additional_discount

                        if (string.IsNullOrEmpty(additionalDiscount))

                        {

                            msSQL += "discount_amount = '0.00', ";

                        }

                        else

                        {

                            msSQL += "discount_amount = '" + additionalDiscount + "', ";

                        }

                        msSQL +=

                            "tax_gid = '" + taxName4 + "', " +

                            "tax_percentage = '" + lstaxpercentage + "', ";

                        // Handle tax_amount4

                        if (string.IsNullOrEmpty(taxAmount4))

                        {

                            msSQL += "tax_amount = '0.00', ";

                        }

                        else

                        {

                            msSQL += "tax_amount = '" + taxAmount4 + "', ";

                        }

                        // Handle roundoff

                        if (roundOff == null)

                        {

                            msSQL += "roundoff = '0.00', ";

                        }

                        else

                        {

                            msSQL += "roundoff = '" + roundOff + "', ";

                        }

                        msSQL += "renewal_flag = '" + renewal_mode + "', " +

                                 "taxsegment_gid = '" + taxSegmentGid + "' " + // No comma here as it's the last column

                                 "WHERE purchaseorderdraft_gid = '" + purchaseorder_gid + "'"; // Condition to update

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }

                    else

                    {


                        msSQL = " insert into pmr_trn_tpurchaseorderdraft (" +
                         " purchaseorderdraft_gid, " +
                         " purchaserequisition_gid, " +
                         " purchaseorder_reference, " +
                         " purchaseorder_date," +
                         " expected_date," +
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
                         " file_name, " +
                         " file_path, " +
                         " renewal_flag ," +
                         " taxsegment_gid " +
                         " ) values (" +
                         "'" + msPO1GetGID + "'," +
                         "'" + msGetGID + "'," +
                         "'" + msPO1GetGID + "'," +
                         "'" + po_date + "', " +
                         "'" + expected_date + "', " +
                         "'" + branchName + "'," +
                         "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                         "'" + vendorCompanyName + "',";

                        //"'" + values.address1.Trim().Replace("'", "") + "',";

                        if (!string.IsNullOrEmpty(address1) && address1.Contains("'"))
                        {
                            msSQL += "'" + address1.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + address1 + "', ";
                        }

                        //"'" + values.shipping_address.Trim().Replace("'", "") + "'," +

                        if (!string.IsNullOrEmpty(shippingAddress) && shippingAddress.Contains("'"))
                        {
                            msSQL += "'" + shippingAddress.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + shippingAddress + "', ";
                        }
                        msSQL += "'" + employeeName + "'," +
                              "'" + deliveryTerms + "'," +
                              "'" + paymentTerms + "'," +
                              "'" + requestorDetails + "'," +
                              "'" + dispatchMode + "'," +
                              "'" + lscurrency_code + "'," +
                              "'" + exchangeRate + "'," +
                              "'" + poCoverNote + "'," +
                              "'" + poCoverNote + "'," +
                              "'" + grandTotal + "',";
                        //"'" + values.template_content.Trim().Replace("'", "") + "',";
                        if (!string.IsNullOrEmpty(templateContent) && templateContent.Contains("'"))
                        {
                            msSQL += "'" + templateContent.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + templateContent + "', ";
                        }
                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "'PO Approved',";
                        }
                        else
                        {
                            msSQL += "'Operation Pending PO',";
                        }
                        msSQL += "'" + "PO Approved" + "'," +
                             "'" + poNo + "'," +
                              "'" + totalAmount + "'," +
                             "'" + addonCharge + "'," +
                             "'" + freightCharges + "',";
                        if (string.IsNullOrEmpty(additionalDiscount))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + additionalDiscount + "',";
                        }
                        msSQL += "'" + taxName4 + "'," +
                        "'" + lstaxpercentage + "',";

                        if (string.IsNullOrEmpty(taxAmount4))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + taxAmount4 + "',";
                        }
                        if (roundOff == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + roundOff + "',";
                        }

                        msSQL += "'" + FileExtensionname + "'," +
                                 "'" + final_path + "'," +
                                   "'" + renewal_mode + "'," +
                                 "'" + taxSegmentGid + "')";
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
                                msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
                                msSQL = " insert into pmr_trn_tpurchaseorderdraftdtl (" +
                                            " purchaseorderdraftdtl_gid, " +
                                            " purchaseorderdraft_gid, " +
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
                                            "'" + msPOGetGID + "', " +
                                            "'" + msPO1GetGID + "'," +
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

                        if (mnResult != 0)

                        {
                            objResult.status = true;
                            objResult.message = "Purchase Order Draft Successfully!";



                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Error While Adding product in Purchase Order!";


                        }

                        msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        dt_datatable.Dispose();
                    }
                }


                else
                {

                    msSQL = "select poref_no from pmr_trn_tpurchaseorder where poref_no ='" + poNo + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        objResult.message = "PO Ref No Alredy Exist!";
                        return;
                    }
                    else
                    {
                        int lsfreight = 0;
                        int lsinsurance = 0;
                        string uiDateStr = poDate;
                        DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string po_date = uiDate.ToString("yyyy-MM-dd");

                        string uiDateStr2 = expectedDate;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string expected_date = uiDate2.ToString("yyyy-MM-dd");

                        string lstax_gid;

                        msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                        msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP", "", user_gid);
                        msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + currencyCode + "' ";
                        string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select poapproval_flag from adm_mst_tcompany";
                        string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                        if (string.IsNullOrEmpty(taxName4))
                        {
                            lstaxpercentage = "0";

                        }
                        else
                        {
                            msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + taxName4 + "'";
                            lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                        }

                        msSQL = " insert into pmr_trn_tpurchaseorderdraft (" +
                             " purchaseorderdraft_gid, " +
                             " purchaserequisition_gid, " +
                             " purchaseorder_reference, " +
                             " purchaseorder_date," +
                             " expected_date," +
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
                             " file_path, " +
                             " file_name, " +
                             " taxsegment_gid " +
                             " ) values (" +
                             "'" + msPO1GetGID + "'," +
                             "'" + msGetGID + "'," +
                             "'" + msPO1GetGID + "'," +
                             "'" + po_date + "', " +
                             "'" + expected_date + "', " +
                             "'" + branchName + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'" + vendorCompanyName + "',";

                        //"'" + values.address1.Trim().Replace("'", "") + "',";

                        if (!string.IsNullOrEmpty(address1) && address1.Contains("'"))
                        {
                            msSQL += "'" + address1.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + address1 + "', ";
                        }

                        //"'" + values.shipping_address.Trim().Replace("'", "") + "'," +

                        if (!string.IsNullOrEmpty(shippingAddress) && shippingAddress.Contains("'"))
                        {
                            msSQL += "'" + shippingAddress.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + shippingAddress + "', ";
                        }
                        msSQL += "'" + employeeName + "'," +
                              "'" + deliveryTerms + "'," +
                              "'" + paymentTerms + "'," +
                              "'" + requestorDetails + "'," +
                              "'" + dispatchMode + "'," +
                              "'" + lscurrency_code + "'," +
                              "'" + exchangeRate + "'," +
                              "'" + poCoverNote + "'," +
                              "'" + poCoverNote + "'," +
                              "'" + grandTotal + "',";
                        //"'" + values.template_content.Trim().Replace("'", "") + "',";
                        if (!string.IsNullOrEmpty(templateContent) && templateContent.Contains("'"))
                        {
                            msSQL += "'" + templateContent.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + templateContent + "', ";
                        }
                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "'PO Approved',";
                        }
                        else
                        {
                            msSQL += "'Operation Pending PO',";
                        }
                        msSQL += "'" + "PO Approved" + "'," +
                             "'" + poNo + "'," +
                              "'" + totalAmount + "'," +
                             "'" + addonCharge + "'," +
                             "'" + freightCharges + "',";
                        if (string.IsNullOrEmpty(additionalDiscount))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + additionalDiscount + "',";
                        }
                        msSQL += "'" + taxName4 + "',";
                        if (string.IsNullOrEmpty(lstaxpercentage))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + lstaxpercentage + "',";
                        }

                        if (string.IsNullOrEmpty(taxAmount4))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + taxAmount4 + "',";
                        }
                        if (roundOff == null)
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + roundOff + "',";
                        }
                        msSQL += "'" + final_path + "'," +
                                 "'" + FileExtensionname + "'," +
                                 "'" + taxSegmentGid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                                    msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
                                    msSQL = " insert into pmr_trn_tpurchaseorderdraftdtl (" +
                                                " purchaseorderdraftdtl_gid, " +
                                                " purchaseorderdraft_gid, " +
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
                                                "'" + msPOGetGID + "', " +
                                                "'" + msPO1GetGID + "'," +
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

                            if (mnResult != 0)

                            {
                                objResult.status = true;
                                objResult.message = "Purchase Order Draft Successfully!";



                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Adding in Purchase Order!";


                            }

                            msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            dt_datatable.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Submitting Purchase Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }


        public void DaProductSubmitdraft (string user_gid, GetViewPurchaseOrder values)
        {
            try
            {
                if (string.IsNullOrEmpty(values.po_no))
                {
                    int lsfreight = 0;
                    int lsinsurance = 0;
                    string uiDateStr = values.po_date;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string po_date = uiDate.ToString("yyyy-MM-dd");

                    string uiDateStr2 = values.expected_date;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string expected_date = uiDate2.ToString("yyyy-MM-dd");

                    string lstax_gid;

                    msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                    msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP", "", user_gid);
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

                    msSQL = "select purchaseorderdraft_gid from pmr_trn_tpurchaseorderdraft  where purchaseorderdraft_gid='" + values.purchaseorder_gid + "'";

                    if (!string.IsNullOrEmpty(values.purchaseorder_gid))
                    {
                        msSQL = "UPDATE pmr_trn_tpurchaseorderdraft SET " +
                                "purchaserequisition_gid = '" + msGetGID + "', " +
                                "purchaseorder_reference = '" + msPO1GetGID + "', " +
                                "purchaseorder_date = '" + po_date + "', " +
                                "expected_date = '" + expected_date + "', " +
                                "branch_gid = '" + values.branch_name + "', " +
                                "created_by = '" + user_gid + "', " +
                                "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                "vendor_gid = '" + values.vendor_companyname + "', ";
                                // Handle single quotes for `address1`
                                if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                                {
                                    msSQL += "vendor_address = '" + values.address1.Replace("'", "") + "', ";
                                }
                                else
                                {
                                    msSQL += "vendor_address = '" + values.address1 + "', ";
                                }

                                // Handle single quotes for `shipping_address`
                                if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                                {
                                    msSQL += "shipping_address = '" + values.shipping_address.Replace("'", "\\\'") + "', ";
                                }
                                else
                                {
                                    msSQL += "shipping_address = '" + values.shipping_address + "', ";
                                }

                                msSQL += "requested_by = '" + values.employee_name + "', " +
                                         "freight_terms = '" + values.delivery_terms + "', " +
                                         "payment_terms = '" + values.payment_terms + "', " +
                                         "requested_details = '" + values.Requestor_details + "', " +
                                         "mode_despatch = '" + values.dispatch_mode + "', " +
                                         "currency_code = '" + lscurrency_code + "', " +
                                         "exchange_rate = '" + values.exchange_rate + "', " +
                                         "po_covernote = '" + values.po_covernote.Replace("'", "\\\'") + "', " +
                                         "purchaseorder_remarks = '" + values.po_covernote.Replace("'", "\\\'") + "', " +
                                         "total_amount = '" + values.grandtotal + "', ";

                                // Handle single quotes for `template_content`
                                if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                                {
                                    msSQL += "termsandconditions = '" + values.template_content.Replace("'", "\\\'") + "', ";
                                }
                                else
                                {
                                    msSQL += "termsandconditions = '" + values.template_content + "', ";
                                }

                                // Set the `purchaseorder_status` based on `lsapproval_flage`
                                if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                                {
                                    msSQL += "purchaseorder_status = 'PO Approved', ";
                                }
                                else
                                {
                                    msSQL += "purchaseorder_status = 'Operation Pending PO', ";
                                }

                                msSQL += "purchaseorder_flag = 'PO Approved', ";

                                // Handle the `po_no`
                                string po_no = "";
                                if (values.po_no == "")
                                {
                                    po_no = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", values.branch_name);
                                    msSQL += "poref_no = '" + po_no + "', ";
                                }
                                else
                                {
                                    msSQL += "poref_no = '" + values.po_no + "', ";
                                }

                                msSQL += "netamount = '" + values.totalamount + "', " +
                                         "addon_amount = '" + values.addoncharge + "', " +
                                         "freightcharges = '" + values.freightcharges + "', ";

                                // Handle `additional_discount`
                                if (string.IsNullOrEmpty(values.additional_discount))
                                {
                                    msSQL += "discount_amount = '0.00', ";
                                }
                                else
                                {
                                    msSQL += "discount_amount = '" + values.additional_discount + "', ";
                                }

                                msSQL += "tax_gid = '" + values.tax_name4 + "', " +
                                         "tax_percentage = '" + lstaxpercentage + "', ";

                                // Handle `tax_amount4`
                                if (string.IsNullOrEmpty(values.tax_amount4))
                                {
                                    msSQL += "tax_amount = '0.00', ";
                                }
                                else
                                {
                                    msSQL += "tax_amount = '" + values.tax_amount4 + "', ";
                                }

                                // Handle `roundoff`
                                if (values.roundoff == null)
                                {
                                    msSQL += "roundoff = '0.00', ";
                                }
                                else
                                {
                                    msSQL += "roundoff = '" + values.roundoff + "', ";
                                }

                                msSQL += "renewal_flag = '" + values.renewal_mode + "', " +
                                         "taxsegment_gid = '" + values.taxsegment_gid + "' " +
                                         "WHERE purchaseorderdraft_gid = '" + msPO1GetGID + "'";  // Add your condition here

                    }
                    else
                    {

                        msSQL = " insert into pmr_trn_tpurchaseorderdraft (" +
                             " purchaseorderdraft_gid, " +
                             " purchaserequisition_gid, " +
                             " purchaseorder_reference, " +
                             " purchaseorder_date," +
                             " expected_date," +
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
                             " renewal_flag, " +
                             " taxsegment_gid " +
                             " ) values (" +
                             "'" + msPO1GetGID + "'," +
                             "'" + msGetGID + "'," +
                             "'" + msPO1GetGID + "'," +
                             "'" + po_date + "', " +
                             "'" + expected_date + "', " +
                             "'" + values.branch_name + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'" + values.vendor_companyname + "',";

                        //"'" + values.address1.Trim().Replace("'", "") + "',";

                        if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                        {
                            msSQL += "'" + values.address1.Replace("'", "") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.address1 + "', ";
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
                        msSQL += "'" + values.employee_name + "'," +
                              "'" + values.delivery_terms + "'," +
                              "'" + values.payment_terms + "'," +
                              "'" + values.Requestor_details + "'," +
                              "'" + values.dispatch_mode + "'," +
                              "'" + lscurrency_code + "'," +
                              "'" + values.exchange_rate + "'," +
                              "'" + values.po_covernote.Replace("'", "\\\'") + "'," +
                              "'" + values.po_covernote.Replace("'", "\\\'") + "'," +
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
                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "'PO Approved',";
                        }
                        else
                        {
                            msSQL += "'Operation Pending PO',";
                        }
                        msSQL += "'" + "PO Approved" + "',";
                        string po_no = "";
                        if (values.po_no == "")
                        {
                            po_no = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", values.branch_name);
                            msSQL += "'" + po_no + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.po_no + "',";
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
                        msSQL += "'" + values.renewal_mode + "',";
                        msSQL += "'" + values.taxsegment_gid + "')";
                    }
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    

                   

                    if (mnResult == 1)
                    {
                        msSQL = "delete from pmr_trn_tpurchaseorderdraftdtl where purchaseorderdraft_gid = '" + values.purchaseorder_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

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
                                if (!string.IsNullOrEmpty(values.purchaseorder_gid))
                                {
                                    msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
                                    msSQL = " insert into pmr_trn_tpurchaseorderdraftdtl (" +
                                                " purchaseorderdraftdtl_gid, " +
                                                " purchaseorderdraft_gid, " +
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
                                                "'" + msPOGetGID + "', " +
                                                "'" + values.purchaseorder_gid + "'," +
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
                                }
                                else
                                {
                                    msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
                                    msSQL = " insert into pmr_trn_tpurchaseorderdraftdtl (" +
                                                " purchaseorderdraftdtl_gid, " +
                                                " purchaseorderdraft_gid, " +
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
                                                "'" + msPOGetGID + "', " +
                                                "'" + msPO1GetGID + "'," +
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
                                }
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                        }

                        if (mnResult != 0)

                        {
                            values.status = true;
                            values.message = "Purchase Order Draft Successfully!";



                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding product in Purchase Order!";


                        }

                        msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        dt_datatable.Dispose();
                    }
                }


                else
                {

                    msSQL = "select poref_no from pmr_trn_tpurchaseorder where poref_no ='" + values.po_no + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        values.message = "PO Ref No Alredy Exist!";
                        return;
                    }
                    else
                    {
                        int lsfreight = 0;
                        int lsinsurance = 0;
                        string uiDateStr = values.po_date;
                        DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string po_date = uiDate.ToString("yyyy-MM-dd");

                        string uiDateStr2 = values.expected_date;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string expected_date = uiDate2.ToString("yyyy-MM-dd");

                        string lstax_gid;

                        msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                        msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP", "", user_gid);
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
                        //msSQL = "select purchaseorderdraft_gid from pmr_trn_tpurchaseordefdraft  where purchaseorderdraft_gid='" + values.purchaseorder_gid + "'";

                        if (!string.IsNullOrEmpty(values.purchaseorder_gid))
                        {
                            msSQL = "UPDATE pmr_trn_tpurchaseorderdraft SET " +
                                    "purchaserequisition_gid = '" + msGetGID + "', " +
                                    "purchaseorder_reference = '" + msPO1GetGID + "', " +
                                    "purchaseorder_date = '" + po_date + "', " +
                                    "expected_date = '" + expected_date + "', " +
                                    "branch_gid = '" + values.branch_name + "', " +
                                    "created_by = '" + user_gid + "', " +
                                    "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                    "vendor_gid = '" + values.vendor_companyname + "', ";
                            // Handle single quotes for `address1`
                            if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                            {
                                msSQL += "vendor_address = '" + values.address1.Replace("'", "") + "', ";
                            }
                            else
                            {
                                msSQL += "vendor_address = '" + values.address1 + "', ";
                            }

                            // Handle single quotes for `shipping_address`
                            if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                            {
                                msSQL += "shipping_address = '" + values.shipping_address.Replace("'", "\\\'") + "', ";
                            }
                            else
                            {
                                msSQL += "shipping_address = '" + values.shipping_address + "', ";
                            }

                            msSQL += "requested_by = '" + values.employee_name + "', " +
                                     "freight_terms = '" + values.delivery_terms + "', " +
                                     "payment_terms = '" + values.payment_terms + "', " +
                                     "requested_details = '" + values.Requestor_details + "', " +
                                     "mode_despatch = '" + values.dispatch_mode + "', " +
                                     "currency_code = '" + lscurrency_code + "', " +
                                     "exchange_rate = '" + values.exchange_rate + "', " +
                                     "po_covernote = '" + values.po_covernote.Replace("'", "\\\'") + "', " +
                                     "purchaseorder_remarks = '" + values.po_covernote.Replace("'", "\\\'") + "', " +
                                     "total_amount = '" + values.grandtotal + "', ";

                            // Handle single quotes for `template_content`
                            if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                            {
                                msSQL += "termsandconditions = '" + values.template_content.Replace("'", "\\\'") + "', ";
                            }
                            else
                            {
                                msSQL += "termsandconditions = '" + values.template_content + "', ";
                            }

                            // Set the `purchaseorder_status` based on `lsapproval_flage`
                            if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                            {
                                msSQL += "purchaseorder_status = 'PO Approved', ";
                            }
                            else
                            {
                                msSQL += "purchaseorder_status = 'Operation Pending PO', ";
                            }

                            msSQL += "purchaseorder_flag = 'PO Approved', ";

                            // Handle the `po_no`
                            string po_no = "";
                            if (values.po_no == "")
                            {
                                po_no = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", values.branch_name);
                                msSQL += "poref_no = '" + po_no + "', ";
                            }
                            else
                            {
                                msSQL += "poref_no = '" + values.po_no + "', ";
                            }

                            msSQL += "netamount = '" + values.totalamount + "', " +
                                     "addon_amount = '" + values.addoncharge + "', " +
                                     "freightcharges = '" + values.freightcharges + "', ";

                            // Handle `additional_discount`
                            if (string.IsNullOrEmpty(values.additional_discount))
                            {
                                msSQL += "discount_amount = '0.00', ";
                            }
                            else
                            {
                                msSQL += "discount_amount = '" + values.additional_discount + "', ";
                            }

                            msSQL += "tax_gid = '" + values.tax_name4 + "', " +
                                     "tax_percentage = '" + lstaxpercentage + "', ";

                            // Handle `tax_amount4`
                            if (string.IsNullOrEmpty(values.tax_amount4))
                            {
                                msSQL += "tax_amount = '0.00', ";
                            }
                            else
                            {
                                msSQL += "tax_amount = '" + values.tax_amount4 + "', ";
                            }

                            // Handle `roundoff`
                            if (values.roundoff == null)
                            {
                                msSQL += "roundoff = '0.00', ";
                            }
                            else
                            {
                                msSQL += "roundoff = '" + values.roundoff + "', ";
                            }

                            msSQL += "renewal_flag = '" + values.renewal_mode + "', " +
                                     "taxsegment_gid = '" + values.taxsegment_gid + "' " +
                                     "WHERE purchaseorderdraft_gid = '" + msPO1GetGID + "'";  // Add your condition here

                        }
                        else
                        {

                            msSQL = " insert into pmr_trn_tpurchaseorderdraft (" +
                             " purchaseorderdraft_gid, " +
                             " purchaserequisition_gid, " +
                             " purchaseorder_reference, " +
                             " purchaseorder_date," +
                             " expected_date," +
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
                               " renewal_flag, " +
                             " taxsegment_gid " +
                             " ) values (" +
                             "'" + msPO1GetGID + "'," +
                             "'" + msGetGID + "'," +
                             "'" + msPO1GetGID + "'," +
                             "'" + po_date + "', " +
                             "'" + expected_date + "', " +
                             "'" + values.branch_name + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                             "'" + values.vendor_companyname + "',";

                            //"'" + values.address1.Trim().Replace("'", "") + "',";

                            if (!string.IsNullOrEmpty(values.address1) && values.address1.Contains("'"))
                            {
                                msSQL += "'" + values.address1.Replace("'", "") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.address1 + "', ";
                            }

                            //"'" + values.shipping_address.Trim().Replace("'", "") + "'," +

                            if (!string.IsNullOrEmpty(values.shipping_address) && values.shipping_address.Contains("'"))
                            {
                                msSQL += "'" + values.shipping_address.Replace("'", "") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.shipping_address + "', ";
                            }
                            msSQL += "'" + values.employee_name + "'," +
                                  "'" + values.delivery_terms + "'," +
                                  "'" + values.payment_terms + "'," +
                                  "'" + values.Requestor_details + "'," +
                                  "'" + values.dispatch_mode + "'," +
                                  "'" + lscurrency_code + "'," +
                                  "'" + values.exchange_rate + "'," +
                                  "'" + values.po_covernote + "'," +
                                  "'" + values.po_covernote + "'," +
                                  "'" + values.grandtotal + "',";
                            //"'" + values.template_content.Trim().Replace("'", "") + "',";
                            if (!string.IsNullOrEmpty(values.template_content) && values.template_content.Contains("'"))
                            {
                                msSQL += "'" + values.template_content.Replace("'", "") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.template_content + "', ";
                            }
                            if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                            {
                                msSQL += "'PO Approved',";
                            }
                            else
                            {
                                msSQL += "'Operation Pending PO',";
                            }
                            msSQL += "'" + "PO Approved" + "'," +
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
                            msSQL += "'" + values.renewal_mode + "',";
                            msSQL += "'" + values.taxsegment_gid + "')";
                        }
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = " delete from pmr_trn_tpurchaseorderdraftdtl where purchaseorderdraft_gid = '" + values.purchaseorder_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                                    if (!string.IsNullOrEmpty(values.purchaseorder_gid))
                                    {
                                        msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
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
                                                "'" + msPOGetGID + "', " +
                                                "'" + values.purchaseorder_gid + "'," +
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
                                    }
                                    else
                                    {
                                        msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
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
                                                "'" + msPOGetGID + "', " +
                                                "'" + msPO1GetGID + "'," +
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
                                    }

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }

                            }

                            if (mnResult != 0)

                            {
                                values.status = true;
                                values.message = "Purchase Order Draft Successfully!";



                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Adding product in Purchase Order!";


                            }

                            msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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


        public void DaGetPoDraftsSummary(MdlPmrTrnPurchaseOrder values)
        {
            try
            {

                msSQL = "select a.vendor_gid,b.vendor_companyname,a.purchaseorderdraft_gid,a.total_amount,date_format (a.purchaseorder_date,'%d-%m-%Y') as purchaseorder_date ,CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorderdraft_gid ELSE a.poref_no END AS porefno from pmr_trn_tpurchaseorderdraft a left join  acp_mst_tvendor b on a.vendor_gid = b.vendor_gid group by a.purchaseorderdraft_gid  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDraft>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDraft
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            purchaseorderdraft_gid = dt["purchaseorderdraft_gid"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            poref_no = dt["porefno"].ToString(),
                        });
                        values.GetDraft = getModuleList;
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

        public void DaGetDraftPurchaseOrderSummary(string user_gid, string purchaseorderdraft_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                msSQL = " delete from pmr_tmp_tpurchaseorder where " +
                        " created_by = '" + user_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select purchaseorderdraftdtl_gid,purchaseorderdraft_gid,qty_ordered,product_gid,product_code,product_name,productuom_name,product_price,display_field_name," +
                        " discount_percentage,discount_amount,tax_name,tax_name2,tax_name3,tax_percentage,tax_percentage2,tax_amount2,tax_amount,tax_amount1_L,tax_amount2_L, " +
                        " tax1_gid,tax2_gid,producttotal_amount from pmr_trn_tpurchaseorderdraftdtl " +
                        " WHERE purchaseorderdraft_gid = '" + purchaseorderdraft_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow da in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("PODC");


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
                                "'" + msGetGid + "'," +
                                "'" + da["purchaseorderdraft_gid"].ToString() + "'," +
                                "'0.00'," +
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
                        msSQL += "'" + da["discount_amount"].ToString() + "'," +
                        "'" + da["tax_name"].ToString() + "'," +
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
                            msSQL += "'" + da["tax_amount"].ToString() + "',";
                        }
                        if (string.IsNullOrEmpty(da["tax_amount2"].ToString()))
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + da["tax_amount2"].ToString() + "',";
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


                msSQL = "select b.vendor_companyname,b.address_gid,a.shipping_address,c.branch_name,d.tax_name, a.purchaseorderdraft_gid as purchaseorder_gid,a.branch_gid,a.vendor_gid," +
                        " a.vendor_address,a.mode_despatch,a.poref_no,a.termsandconditions,a.discount_amount,a.tax_percentage,a.tax_amount,a.addon_amount,g.currencyexchange_gid, " +
                        " a.currency_code,a.exchange_rate,a.freightcharges,a.tax_gid,a.roundoff,a.freight_terms,a.netamount,a.requested_by,e.user_firstname,b.email_id," +
                        " a.requested_details,a.po_covernote ,a.total_amount,date_format(a.purchaseorder_date,'%d-%m-%Y') as po_date,date_format(a.expected_date,'%d-%m-%Y') as expected_date,a.payment_terms,f.address2,b.gst_no,b.contact_telephonenumber " +
                        " from pmr_trn_tpurchaseorderdraft a left join acp_mst_tvendor b on a.vendor_gid=b.vendor_gid " +
                        " left join hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                        " left join acp_mst_ttax d on a.tax_gid= d.tax_gid " +
                        " left join adm_mst_taddress f on b.address_gid = f.address_gid" +
                        " left join adm_mst_tuser e on a.requested_by = e.user_gid " +
                        " left join crm_trn_tcurrencyexchange g on a.currency_code = g.currency_code  " +
                        " where a.purchaseorderdraft_gid = '" + purchaseorderdraft_gid + "'";
                ds_dataset = objdbconn.GetDataSet(msSQL, "pmr_trn_tpurchaseorderdraft");
                var getModuleList = new List<GetEditPurchaseOrderSummary>();
                if (ds_dataset.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dt in ds_dataset.Tables[0].Rows)
                    {
                        getModuleList.Add(new GetEditPurchaseOrderSummary
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            address_gid = dt["address_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            netamount = dt["netamount"].ToString().Replace(".00", ""),
                            requested_by = dt["requested_by"].ToString(),
                            requested_details = dt["requested_details"].ToString(),
                            po_covernote = dt["po_covernote"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            po_date = dt["po_date"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            address2 = dt["address2"].ToString(),
                            gst_no = dt["gst_no"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            poref_no = dt["poref_no"].ToString(),
                        });
                        values.GetEditPurchaseOrderSummary = getModuleList;
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

        public void DaUpdatePurchaseOrderfileupload(HttpRequest httpRequest, result objResult, string user_gid)
        {
            try
            {

                //var POProductList = httpRequest.Form["POProductList"].ToString();
                var branchName = httpRequest.Form["branch_name"].ToString();
                var branchGid = httpRequest.Form["branch_gid"].ToString();
                var poNo = httpRequest.Form["po_no"].ToString();
                var poDate = httpRequest.Form["po_date"].ToString();
                var expectedDate = httpRequest.Form["expected_date"].ToString();
                var vendorGid = httpRequest.Form["vendor_gid"].ToString();
                var vendorCompanyName = httpRequest.Form["vendor_companyname"].ToString();
                var vendorDetails = httpRequest.Form["vendor_details"].ToString();
                var address1 = httpRequest.Form["address1"].ToString();
                var employeeName = httpRequest.Form["employee_name"].ToString();
                var deliveryTerms = httpRequest.Form["delivery_terms"].ToString();
                var paymentTerms = httpRequest.Form["payment_terms"].ToString();
                var requestorDetails = httpRequest.Form["Requestor_details"].ToString();
                var dispatchMode = httpRequest.Form["dispatch_mode"].ToString();
                var currencyGid = httpRequest.Form["currency_gid"].ToString();
                var currencyCode = httpRequest.Form["currency_code"].ToString();
                var exchangeRate = httpRequest.Form["exchange_rate"].ToString();
                var poCoverNote = httpRequest.Form["po_covernote"].ToString();
                var templateName = httpRequest.Form["template_name"].ToString();
                var templateContent = httpRequest.Form["template_content"].ToString();
                var templateGid = httpRequest.Form["template_gid"].ToString();
                var totalAmount = httpRequest.Form["totalamount"].ToString();
                var addonCharge = httpRequest.Form["addoncharge"].ToString();
                var additionalDiscount = httpRequest.Form["additional_discount"].ToString();
                var freightCharges = httpRequest.Form["freightcharges"].ToString();
                var roundOff = httpRequest.Form["roundoff"].ToString();
                var grandTotal = httpRequest.Form["grandtotal"].ToString();
                var taxGid = httpRequest.Form["tax_gid"].ToString();
                var taxName1 = httpRequest.Form["tax_name1"].ToString();
                var taxName2 = httpRequest.Form["tax_name2"].ToString();
                var taxName3 = httpRequest.Form["tax_name3"].ToString();
                var taxAmount1 = httpRequest.Form["taxamount1"].ToString();
                var taxAmount2 = httpRequest.Form["taxamount2"].ToString();
                var taxAmount3 = httpRequest.Form["taxamount3"].ToString();
                var taxAmount4 = httpRequest.Form["tax_amount4"].ToString();
                var taxName4 = httpRequest.Form["tax_name4"].ToString();
                var taxSegmentGid = httpRequest.Form["taxsegment_gid"].ToString();
                var shippingAddress = httpRequest.Form["shipping_address"].ToString();
                var renewal_mode = httpRequest.Form["renewal_flag"].ToString();
                var frequency_terms = httpRequest.Form["frequency_terms"].ToString();
                var renewal_date = httpRequest.Form["renewal_date"].ToString();
                var purchaseorder_gid = httpRequest.Form["purchaseorder_gid"].ToString();


                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                MemoryStream ms_stream = new MemoryStream();
                string document_gid = string.Empty;
                string lscompany_code = string.Empty;
                HttpPostedFile httpPostedFile;
                string lspath;
                string final_path = "";
                string vessel_name = "";



                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }

                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        FileExtensionname = httpPostedFile.FileName;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        ms.Close();
                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "Purchase/Purchaseorderfiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        final_path = lspath + msdocument_gid + FileExtension;

                    }
                }

                if (string.IsNullOrEmpty(poNo))
                {
                    int lsfreight = 0;
                    int lsinsurance = 0;
                    string uiDateStr = poDate;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string po_date = uiDate.ToString("yyyy-MM-dd");

                    string uiDateStr2 = expectedDate;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string expected_date = uiDate2.ToString("yyyy-MM-dd");

                    string lstax_gid;

                    msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                    msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP");
                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + currencyCode + "' ";
                    string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select poapproval_flag from adm_mst_tcompany";
                    string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                    if (string.IsNullOrEmpty(taxName4))
                    {
                        lstaxpercentage = "0";

                    }
                    else
                    {
                        msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + taxName4 + "'";
                        lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                    }

                    msSQL = "UPDATE pmr_trn_tpurchaseorder SET " +
                            "purchaserequisition_gid = '" + msGetGID + "', " +
                            "purchaseorder_reference = '" + msPO1GetGID + "', " +
                            "purchaseorder_date = '" + po_date + "', " +
                            "expected_date = '" + expected_date + "', " +
                            "branch_gid = '" + branchName + "', " +
                            "created_by = '" + user_gid + "', " +
                            "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', ";
                    if (!string.IsNullOrEmpty(vendorCompanyName) && vendorCompanyName.Contains("'"))
                    {
                        msSQL += "vendor_gid = '" + vendorCompanyName.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "vendor_gid = '" + vendorCompanyName + "', ";
                    }

                    if (!string.IsNullOrEmpty(address1) && address1.Contains("'"))
                    {
                        msSQL += "vendor_address = '" + address1.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "vendor_address = '" + address1 + "', ";
                    }

                    if (!string.IsNullOrEmpty(shippingAddress) && shippingAddress.Contains("'"))
                    {
                        msSQL += "shipping_address = '" + shippingAddress.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "shipping_address = '" + shippingAddress + "', ";
                    }
                    if (!string.IsNullOrEmpty(employeeName) && employeeName.Contains("'"))
                    {
                        msSQL += "requested_by = '" + employeeName.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "requested_by = '" + employeeName + "', ";
                    }
                    if (!string.IsNullOrEmpty(deliveryTerms) && deliveryTerms.Contains("'"))
                    {
                        msSQL += "freight_terms = '" + deliveryTerms.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "freight_terms = '" + deliveryTerms + "', ";
                    }
                    if (!string.IsNullOrEmpty(paymentTerms) && paymentTerms.Contains("'"))
                    {
                        msSQL += "payment_terms = '" + paymentTerms.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "payment_terms = '" + paymentTerms + "', ";
                    }

                    msSQL += "requested_details = '" + requestorDetails + "', ";
                    if (!string.IsNullOrEmpty(dispatchMode) && dispatchMode.Contains("'"))
                    {
                        msSQL += "mode_despatch = '" + dispatchMode.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "mode_despatch = '" + dispatchMode + "', ";
                    }
                    msSQL += "currency_code = '" + lscurrency_code + "', " +
                          "exchange_rate = '" + exchangeRate + "', ";
                    if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                    {
                        msSQL += "po_covernote = '" + poCoverNote.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "po_covernote = '" + poCoverNote + "', ";
                    }
                    if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                    {
                        msSQL += "purchaseorder_remarks = '" + poCoverNote.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "purchaseorder_remarks = '" + poCoverNote + "', ";
                    }
                    msSQL += "total_amount = '" + grandTotal + "', ";

                    if (!string.IsNullOrEmpty(templateContent) && templateContent.Contains("'"))
                    {
                        msSQL += "termsandconditions = '" + templateContent.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "termsandconditions = '" + templateContent + "', ";
                    }

                    if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                    {
                        msSQL += "purchaseorder_status = 'PO Approved', ";
                    }
                    else
                    {
                        msSQL += "purchaseorder_status = 'Operation Pending PO', ";
                    }

                    msSQL += "purchaseorder_flag = 'PO Approved', ";

                    if (poNo == "")
                    {
                        poNo = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", branchName);
                        msSQL += "poref_no = '" + poNo + "', ";
                    }
                    else
                    {
                        msSQL += "poref_no = '" + poNo + "', ";
                    }

                    msSQL += "netamount = '" + totalAmount + "', " +
                          "addon_amount = '" + addonCharge + "', " +
                          "freightcharges = '" + freightCharges + "', ";

                    if (string.IsNullOrEmpty(additionalDiscount))
                    {
                        msSQL += "discount_amount = '0.00', ";
                    }
                    else
                    {
                        msSQL += "discount_amount = '" + additionalDiscount + "', ";
                    }

                    msSQL += "tax_gid = '" + taxName4 + "', " +
                          "tax_percentage = '" + lstaxpercentage + "', ";

                    if (string.IsNullOrEmpty(taxAmount4))
                    {
                        msSQL += "tax_amount = '0.00', ";
                    }
                    else
                    {
                        msSQL += "tax_amount = '" + taxAmount4 + "', ";
                    }

                    if (roundOff == null)
                    {
                        msSQL += "roundoff = '0.00', ";
                    }
                    else
                    {
                        msSQL += "roundoff = '" + roundOff + "', ";
                    }

                    msSQL += "file_name = '" + FileExtensionname + "', " +
                              "file_path = '" + final_path + "', " +
                              "renewal_flag = '" + renewal_mode + "', " +
                              "taxsegment_gid = '" + taxSegmentGid + "' " +
                          "WHERE purchaseorder_gid = '" + purchaseorder_gid + "';"; 

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (renewal_mode == "Y")
                    {
                        

                        msSQL = "UPDATE pmr_trn_trenewal SET " +
                        "renewal_flag = '" + renewal_mode + "', " +
                        "frequency_term = '" + frequency_terms + "', " +
                        "vendor_gid = '" + vendorCompanyName + "', " +
                        "renewal_date = '" + renewal_date + "', " +
                        "created_by = '" + user_gid + "', " +
                        "renewal_type = 'Purchase', " +
                        "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                        "WHERE purchaseorder_gid = '" + purchaseorder_gid + "'"; 

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1)
                    {
                        msSQL = " delete from pmr_trn_tpurchaseorderdtl WHERE purchaseorder_gid = '" + purchaseorder_gid + "'";
                         mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                                msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
                                msSQL = "UPDATE pmr_trn_tpurchaseorderdtl SET " +
                                        "purchaseorderdtl_gid = '" + msPOGetGID + "', " +
                                        "product_gid = '" + dt["product_gid"].ToString() + "', " +
                                        "product_code = '" + dt["product_code"].ToString() + "', " +
                                        "product_name = '" + (String.IsNullOrEmpty(dt["product_name"].ToString()) ? dt["product_name"].ToString() : dt["product_name"].ToString().Replace("'", "\\\'")) + "', " +
                                        "productuom_name = '" + (String.IsNullOrEmpty(dt["productuom_name"].ToString()) ? dt["productuom_name"].ToString() : dt["productuom_name"].ToString().Replace("'", "\\\'")) + "', " +
                                        "uom_gid = '" + dt["uom_gid"].ToString() + "', " +
                                        "producttype_gid = '" + dt["type"].ToString() + "', " +
                                        "product_price = '" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                        "producttotal_amount = '" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                        "discount_percentage = '" + dt["discount_percentage"].ToString().Replace(",", "") + "', ";

                                if (string.IsNullOrEmpty(dt["discount_amount"].ToString()))
                                {
                                    msSQL += "discount_amount = '0.00', ";
                                }
                                else
                                {
                                    msSQL += "discount_amount = '" + dt["discount_amount"].ToString().Replace(",", "") + "', ";
                                }

                                msSQL += "tax_name = '" + dt["tax_name"].ToString() + "', " +
                                        "tax_name2 = '" + dt["tax_name2"].ToString() + "', " +
                                        "tax_name3 = '" + dt["tax_name3"].ToString() + "', " +
                                        "tax1_gid = '" + dt["tax1_gid"].ToString() + "', " +
                                        "tax2_gid = '" + dt["tax2_gid"].ToString() + "', " +
                                        "tax3_gid = '" + dt["tax3_gid"].ToString() + "', " +
                                        "qty_ordered = '" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                        "product_price_L = '" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                         "display_field_name = '" + (String.IsNullOrEmpty(dt["display_field"].ToString()) ? dt["display_field"].ToString() : dt["display_field"].ToString().Replace("'", "\\\'")) + "', " +
                                        "tax_amount1_L = '" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                        "tax_amount2_L = '" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                        "tax_amount3_L = '" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                        "tax_amount = '" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                        "tax_amount2 = '" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                        "tax_amount3 = '" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                        "tax_percentage = '" + dt["tax_percentage"].ToString().Replace(",", "") + "', " +
                                        "tax_percentage2 = '" + dt["tax_percentage2"].ToString().Replace(",", "") + "', " +
                                        "tax_percentage3 = '" + dt["tax_percentage3"].ToString().Replace(",", "") + "', " +
                                        "taxsegment_gid = '" + dt["taxsegment_gid"].ToString() + "' " +
                                        "WHERE purchaseorder_gid = '" + purchaseorder_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                        }

                        if (mnResult != 0)

                        {
                            objResult.status = true;
                            objResult.message = "Purchase Order Raised Successfully!";
                        
                           

                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Error While Adding product in Purchase Order!";


                        }

                        msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        dt_datatable.Dispose();
                    }
                }


                else
                {

                        int lsfreight = 0;
                        int lsinsurance = 0;
                        string uiDateStr = poDate;
                        DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string po_date = uiDate.ToString("yyyy-MM-dd");

                        string uiDateStr2 = expectedDate;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        string expected_date = uiDate2.ToString("yyyy-MM-dd");

                        string lstax_gid;

                        msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                        msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP");
                        msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + currencyCode + "' ";
                        string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select poapproval_flag from adm_mst_tcompany";
                        string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                        if (string.IsNullOrEmpty(taxName4))
                        {
                            lstaxpercentage = "0";

                        }
                        else
                        {
                            msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + taxName4 + "'";
                            lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);
                        }

                        msSQL = "UPDATE pmr_trn_tpurchaseorder SET " +
                          "purchaserequisition_gid = '" + msGetGID + "', " +
                          "purchaseorder_reference = '" + msPO1GetGID + "', " +
                          "purchaseorder_date = '" + po_date + "', " +
                          "expected_date = '" + expected_date + "', " +
                          "branch_gid = '" + branchName + "', " +
                          "created_by = '" + user_gid + "', " +
                          "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                          "vendor_gid = '" + vendorCompanyName + "', ";

                        if (!string.IsNullOrEmpty(address1) && address1.Contains("'"))
                        {
                            msSQL += "vendor_address = '" + address1.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "vendor_address = '" + address1 + "', ";
                        }

                        if (!string.IsNullOrEmpty(shippingAddress) && shippingAddress.Contains("'"))
                        {
                            msSQL += "shipping_address = '" + shippingAddress.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "shipping_address = '" + shippingAddress + "', ";
                        }
                    if (!string.IsNullOrEmpty(employeeName) && employeeName.Contains("'"))
                    {
                        msSQL += "requested_by = '" + employeeName.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "requested_by = '" + employeeName + "', ";
                    }
                    if (!string.IsNullOrEmpty(deliveryTerms) && deliveryTerms.Contains("'"))
                    {
                        msSQL += "freight_terms = '" + deliveryTerms.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "freight_terms = '" + deliveryTerms + "', ";
                    }
                    if (!string.IsNullOrEmpty(paymentTerms) && paymentTerms.Contains("'"))
                    {
                        msSQL += "payment_terms = '" + paymentTerms.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "payment_terms = '" + paymentTerms + "', ";
                    }

                    msSQL +="requested_details = '" + requestorDetails + "', ";
                        if (!string.IsNullOrEmpty(dispatchMode) && dispatchMode.Contains("'"))
                        {
                            msSQL += "mode_despatch = '" + dispatchMode.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "mode_despatch = '" + dispatchMode + "', ";
                        }
                    msSQL += "currency_code = '" + lscurrency_code + "', " +
                          "exchange_rate = '" + exchangeRate + "', ";
                    if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                    {
                        msSQL += "po_covernote = '" + poCoverNote.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "po_covernote = '" + poCoverNote + "', ";
                    }
                    if (!string.IsNullOrEmpty(poCoverNote) && poCoverNote.Contains("'"))
                    {
                        msSQL += "purchaseorder_remarks = '" + poCoverNote.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "purchaseorder_remarks = '" + poCoverNote + "', ";
                    }
                    msSQL += "total_amount = '" + grandTotal + "', ";

                        if (!string.IsNullOrEmpty(templateContent) && templateContent.Contains("'"))
                        {
                            msSQL += "termsandconditions = '" + templateContent.Replace("'", "\\\'") + "', ";
                        }
                        else
                        {
                            msSQL += "termsandconditions = '" + templateContent + "', ";
                        }

                        if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                        {
                            msSQL += "purchaseorder_status = 'PO Approved', ";
                        }
                        else
                        {
                            msSQL += "purchaseorder_status = 'Operation Pending PO', ";
                        }

                        msSQL += "purchaseorder_flag = 'PO Approved', ";

                        if (poNo == "")
                        {
                            poNo = objcmnfunctions.getSequencecustomizerGID("PPOP", "PMR", branchName);
                            msSQL += "poref_no = '" + poNo + "', ";
                        }
                        else
                        {
                            msSQL += "poref_no = '" + poNo + "', ";
                        }

                        msSQL += "netamount = '" + totalAmount + "', " +
                              "addon_amount = '" + addonCharge + "', " +
                              "freightcharges = '" + freightCharges + "', ";

                        if (string.IsNullOrEmpty(additionalDiscount))
                        {
                            msSQL += "discount_amount = '0.00', ";
                        }
                        else
                        {
                            msSQL += "discount_amount = '" + additionalDiscount + "', ";
                        }

                        msSQL += "tax_gid = '" + taxName4 + "', " +
                              "tax_percentage = '" + lstaxpercentage + "', ";

                        if (string.IsNullOrEmpty(taxAmount4))
                        {
                            msSQL += "tax_amount = '0.00', ";
                        }
                        else
                        {
                            msSQL += "tax_amount = '" + taxAmount4 + "', ";
                        }

                        if (roundOff == null)
                        {
                            msSQL += "roundoff = '0.00', ";
                        }
                        else
                        {
                            msSQL += "roundoff = '" + roundOff + "', ";
                        }

                        msSQL += "file_name = '" + FileExtensionname + "', " +
                                  "file_path = '" + final_path + "', " +
                                  "renewal_flag = '" + renewal_mode + "', " +
                                  "taxsegment_gid = '" + taxSegmentGid + "' " +
                              "WHERE purchaseorder_gid = '" + purchaseorder_gid + "';";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (renewal_mode == "Y")
                        {


                            msSQL = "UPDATE pmr_trn_trenewal SET " +
                            "renewal_flag = '" + renewal_mode + "', " +
                            "frequency_term = '" + frequency_terms + "', " +
                            "vendor_gid = '" + vendorCompanyName + "', " +
                            "renewal_date = '" + renewal_date + "', " +
                            "created_by = '" + user_gid + "', " +
                            "renewal_type = 'Purchase', " +
                            "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                            "WHERE purchaseorder_gid = '" + purchaseorder_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        if (mnResult == 1)
                        {
                                msSQL = " delete from pmr_trn_tpurchaseorderdtl WHERE purchaseorder_gid = '" + purchaseorder_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                                    msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
                                        msSQL = "UPDATE pmr_trn_tpurchaseorderdtl SET " +
                                            "purchaseorderdtl_gid = '" + msPOGetGID + "', " +
                                            "product_gid = '" + dt["product_gid"].ToString() + "', " +
                                            "product_code = '" + dt["product_code"].ToString() + "', " +
                                             "product_name = '" + (String.IsNullOrEmpty(dt["product_name"].ToString()) ? dt["product_name"].ToString() : dt["product_name"].ToString().Replace("'", "\\\'")) + "', " +
                                             "productuom_name = '" + (String.IsNullOrEmpty(dt["productuom_name"].ToString()) ? dt["productuom_name"].ToString() : dt["productuom_name"].ToString().Replace("'", "\\\'")) + "', " +
                                            "uom_gid = '" + dt["uom_gid"].ToString() + "', " +
                                            "producttype_gid = '" + dt["type"].ToString() + "', " +
                                            "product_price = '" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                            "producttotal_amount = '" + dt["producttotal_price"].ToString().Replace(",", "") + "', " +
                                            "discount_percentage = '" + dt["discount_percentage"].ToString().Replace(",", "") + "', ";

                                        if (string.IsNullOrEmpty(dt["discount_amount"].ToString()))
                                        {
                                            msSQL += "discount_amount = '0.00', ";
                                        }
                                        else
                                        {
                                            msSQL += "discount_amount = '" + dt["discount_amount"].ToString().Replace(",", "") + "', ";
                                        }

                                        msSQL += "tax_name = '" + dt["tax_name"].ToString() + "', " +
                                                "tax_name2 = '" + dt["tax_name2"].ToString() + "', " +
                                                "tax_name3 = '" + dt["tax_name3"].ToString() + "', " +
                                                "tax1_gid = '" + dt["tax1_gid"].ToString() + "', " +
                                                "tax2_gid = '" + dt["tax2_gid"].ToString() + "', " +
                                                "tax3_gid = '" + dt["tax3_gid"].ToString() + "', " +
                                                "qty_ordered = '" + dt["qty_ordered"].ToString().Replace(",", "") + "', " +
                                                "product_price_L = '" + dt["product_price"].ToString().Replace(",", "") + "', " +
                                                 "display_field_name = '" + (String.IsNullOrEmpty(dt["display_field"].ToString()) ? dt["display_field"].ToString() : dt["display_field"].ToString().Replace("'", "\\\'")) + "', " +
                                                "tax_amount1_L = '" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                                "tax_amount2_L = '" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                                "tax_amount3_L = '" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                                "tax_amount = '" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                                "tax_amount2 = '" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                                                "tax_amount3 = '" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                                                "tax_percentage = '" + dt["tax_percentage"].ToString().Replace(",", "") + "', " +
                                                "tax_percentage2 = '" + dt["tax_percentage2"].ToString().Replace(",", "") + "', " +
                                                "tax_percentage3 = '" + dt["tax_percentage3"].ToString().Replace(",", "") + "', " +
                                                "taxsegment_gid = '" + dt["taxsegment_gid"].ToString() + "' " +
                                                "WHERE purchaseorder_gid = '" + purchaseorder_gid + "'";

                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }

                            }

                            if (mnResult != 0)

                            {
                                objResult.status = true;
                                objResult.message = "Purchase Order Raised Successfully!";



                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Adding product in Purchase Order!";


                            }

                            msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            dt_datatable.Dispose();
                        }
                    }
                
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Submitting Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }






    }
}


