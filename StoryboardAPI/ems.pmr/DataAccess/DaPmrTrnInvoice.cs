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
using System.Text;
using System.Reflection.Emit;


namespace ems.pmr.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class DaPmrTrnInvoice
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        finance_cmnfunction objfincmn = new finance_cmnfunction();
        DataTable dt_datatable;
        string msGetGid;
        int mnResult, mnResult1;
        public void DaGetPmrTrnInvoiceAddSelectSummary(MdlPmrTrnInvoice values)
        {
            try {
                //msSQL = " select * from pbl_trn_tinvoiceaddsum_new_V";

                msSQL = " SELECT x.vendor_gid AS vendor_gid,x.purchaseorder_gid AS purchaseorder_gid,x.porefno, " +
                        "  x.created_by AS created_by, x.purchaseorder_status AS purchaseorder_status, x.branch_name AS branch_name, " +
                        "  x.vendor_contact_person AS vendor_contact_person, FORMAT(x.total_amount, 2) AS total_amount,  " +
                        "  DATE_FORMAT(x.purchaseorder_date, '%d-%m-%y') AS purchaseorder_date, x.purchaseorder_from AS purchaseorder_from,  " +
                        "  x.vendor_companyname AS vendor_companyname,x.grn_gid as grn_gid FROM(SELECT a.vendor_gid AS vendor_gid, a.vendor_address as vendor_address,  " +
                        "  CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorder_gid ELSE a.poref_no END AS porefno,a.purchaseorder_gid AS purchaseorder_gid , a.created_by AS created_by,  " +
                        "  a.purchaseorder_status AS purchaseorder_status, c.branch_name AS branch_name,  " +
                        "  CONCAT(b.contactperson_name,'/',b.email_id,'/',b.contact_telephonenumber) As vendor_contact_person, a.total_amount AS total_amount,  " +
                        "  a.purchaseorder_date AS purchaseorder_date, a.purchaseorder_from AS purchaseorder_from,  " +
                        "  b.vendor_companyname AS vendor_companyname,d.grn_gid as grn_gid,CONCAT(b.vendor_code, '/', b.vendor_companyname) AS vendor FROM  " +
                        "  pmr_trn_tpurchaseorder a LEFT JOIN acp_mst_tvendor b ON a.vendor_gid = b.vendor_gid  " +
                        "  LEFT JOIN hrm_mst_tbranch c ON a.branch_gid = c.branch_gid  " +
                        "  left join pmr_trn_tgrn d On a.purchaseorder_gid = d.purchaseorder_gid WHERE  " +
                        "  ((d.grn_flag='GRN Approved') and (d.invoice_flag = 'IV Pending')  " +
                        "  OR(d.invoice_flag = 'Invoice Raised Partial')  " +
                        "  ) GROUP BY  a.purchaseorder_gid) x ORDER BY x.purchaseorder_date DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<invoice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new invoice_list
                    {
                        vendor_gid = dt["vendor_gid"].ToString(),
                        grn_gid = dt["grn_gid"].ToString(),
                        purchaseorder_date = dt["purchaseorder_date"].ToString(),
                        purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        vendor_contact_person = dt["vendor_contact_person"].ToString(),
                        total_amount = dt["total_amount"].ToString(),
                        purchaseorder_from = dt["purchaseorder_from"].ToString(),
                        purchaseorder_status = dt["purchaseorder_status"].ToString(),
                        porefno = dt["porefno"].ToString()
                    });
                    values.invoice_list = getModuleList;
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

        public void DaGetPmrTrnInvoiceServiceSummary(MdlPmrTrnInvoice values)
        {
            try
            {
                //msSQL = " select * from pbl_trn_tinvoiceaddsum_new_V";

                msSQL = " select concat(k.contactperson_name,' / ',k.email_id,' / ',k.contact_telephonenumber) as Contact,k.vendor_companyname," +
                        " j.branch_name,a.purchaseorder_gid,a.branch_gid,date_format(a.purchaseorder_date,'%d-%m-%Y') as purchaseorder_date,a.vendor_gid,a.vendor_contact_person," +
                        " a.vendor_address,a.purchaseorder_reference,a.total_amount,a.purchaseorder_status,a.purchaseorder_flag," +
                        " a.invoice_flag,a.payment_flag,CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorder_gid ELSE " +
                        " a.poref_no END AS porefno from pmr_trn_tpurchaseorder a LEFT JOIN pmr_trn_tpurchaseorderdtl s ON " +
                        " a.purchaseorder_gid = s.purchaseorder_gid  LEFT JOIN pmr_mst_tproduct g ON s.product_gid = g.product_gid " +
                        " LEFT JOIN pmr_mst_tproducttype i ON i.producttype_gid = g.producttype_gid " +
                        " LEFT JOIN acp_trn_tinvoice z ON z.purchaseorder_gid = a.purchaseorder_gid " +
                        " LEFT JOIN hrm_mst_tbranch j ON j.branch_gid = a.branch_gid " +
                        " LEFT JOIN acp_mst_tvendor k ON k.vendor_gid = a.vendor_gid " +
                        " LEFT JOIN pmr_mst_tproductgroup f ON g.productgroup_gid = f.productgroup_gid " +
                        " WHERE NOT (f.productgroup_name = 'General' AND i.producttype_name = 'Services') " + 
                        " AND i.producttype_name = 'Services' and (a.invoice_flag='IV Pending' or a.invoice_flag='Invoice Raised Partial')" +
                        " and a.purchaseorder_status <>'PO Amended' and a.purchaseorder_flag <> 'PO Canceled'  AND (COALESCE(z.invoice_amount, 0) <> a.total_amount)  and a.purchaseorder_flag <> 'PO Completed' GROUP BY " +
                        " a.purchaseorder_gid ORDER BY a.purchaseorder_date DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new invoice_list
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                             purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_contact_person = dt["Contact"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            //purchaseorder_from = dt["purchaseorder_from"].ToString(),
                            purchaseorder_status = dt["purchaseorder_status"].ToString(),
                            porefno = dt["porefno"].ToString()
                        });
                        values.Serviceinvoice_list = getModuleList;
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

        public void DaGetInvoiceSummary(MdlPmrTrnInvoice values, string user_gid)
        {
            try {

                //msSQL = " select /*+ MAX_EXECUTION_TIME(300000) */ distinct a.invoice_gid,a.agreement_gid, a.invoice_refno, case when a.invoice_status='IV Approved' then 'IV Completed' else a.invoice_status end as invoice_status, a.invoice_flag, " +
                //    " concat(c.contactperson_name,'/',c.email_id,'/',c.contact_telephonenumber) as contact,CASE when a.payment_flag <> 'PY Pending' then a.payment_flag " +
                //    " else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount, " +
                //    " concat(c.vendor_code, ' / ',c.vendor_companyname) as Vendor, "+
                //    " a.vendor_gid, format(a.invoice_amount,2) as invoice_amount,a.vendorinvoiceref_no, a.vendor_gid, i.costcenter_gid, " +
                //    " CASE WHEN i.costcenter_gid is NOT NULL THEN " +
                //    " (select costcenter_name from pmr_mst_tcostcenter x where h.costcenter_gid=x.costcenter_gid) " +
                //    " ELSE (select costcenter_name from pmr_mst_tcostcenter y where j.costcenter_gid=y.costcenter_gid) END as costcenter_name, " +
                //    "  DATE_FORMAT(a.invoice_date, '%d-%m-%Y') AS invoice_date," +
                //    " DATE_FORMAT(a.payment_date, '%d-%m-%Y') AS payment_date," +
                //    " a.payment_flag,a.invoice_type, " +
                //    " c.vendor_code, c.vendor_companyname,a.invoice_from,a.invoice_reference " +
                //    " from acp_trn_tinvoice a " +
                //    " left join acp_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                //    " left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid " +
                //    " left join adm_mst_tuser d on d.user_gid = a.user_gid " +
                //    " left join hrm_mst_temployee e on e.user_gid = d.user_gid " +
                //    " left join adm_mst_tmodule2employee  f on f.employee_gid = e.employee_gid " +
                //    " left join acp_trn_tpo2invoice g on g.invoice_gid=a.invoice_gid " +
                //    " left join pmr_trn_tpurchaseorder h on g.purchaseorder_gid=h.purchaseorder_gid " +
                //    " left join pmr_mst_tcostcenter i on h.costcenter_gid=i.costcenter_gid " +
                //    " left join pbl_trn_tserviceorder j on j.serviceorder_gid=a.invoice_reference " +
                //    " where a.invoice_type<>'Opening Invoice'  " +
                //    " order by date(a.invoice_date) desc,a.invoice_date asc, a.invoice_gid desc ";
                msSQL = "call pmr_trn_invoicesummary";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<invoice_lista>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new invoice_lista
                    {
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        payment_date = dt["payment_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        vendorinvoiceref_no = dt["vendorinvoiceref_no"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        costcenter_name = dt["costcenter_name"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        Vendor = dt["Vendor"].ToString(),
                        invoice_type = dt["invoice_type"].ToString(),
                        invoice_status = dt["invoice_status"].ToString(),
                        overall_status = dt["overall_status"].ToString(),
                        contact = dt["contact"].ToString(),
                        vendor_code = dt["vendor_code"].ToString(),

                    });
                    values.invoice_lista = getModuleList;
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

        //purchasetype dropdowm
        public void DaGetPmrPurchaseDtl(MdlPmrTrnInvoice values)
        {
            try
            {

                msSQL = " select  a.account_gid, a.purchasetype_name " +
                        " from pmr_trn_tpurchasetype a ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPurchaseTypeDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPurchaseTypeDropdown
                        {
                            account_gid = dt["account_gid"].ToString(),
                            purchasetype_name = dt["purchasetype_name"].ToString(),
                        });
                        values.GetPmrPurchaseDtl = getModuleList;
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
        public void DaGetEditInvoiceSummary(string vendor_gid, MdlPmrTrnInvoice values)
        {
            try
            {
                msSQL = " SELECT a.serviceorder_gid,date_format(a.serviceorder_date, '%d/%m/%Y') as serviceorder_date," +
                    " b.vendor_companyname,b.vendor_gid,format(a.grand_total, 2) as grand_total,a.currency_code,sum(e.invoice_amount)" +
                    " as invoice_amount,f.tax_name,a.tax_amount, b.contactperson_name,b.email_id,b.vendor_code,c.exchange_rate,s.outstanding_amount," +
                    " format(a.addon_amount, 2) as addon_amount ,format(a.discount_amount, 2) as discount_amount,d.branch_name , " +
                    " g.product_gid, g.serviceorderdtl_gid,g.quantity,g.unit_price,format(g.amount, 2) as amount ,e.invoice_gid, e.invoice_date, e.invoice_remarks, " +
                    " format(g.tax_amount1, 2) as tax_amount1,g.tax1_gid,g.tax2_gid,g.tax3_gid, " + "format(g.tax_amount2, 2) as tax_amount2," +
                    " format(g.tax_amount3, 2) as tax_amount3, format(g.total_amount - g.tax_amount1 - tax_amount2 - tax_amount3, 2) as total_amount," +
                    " g.description, h.product_name, h.product_code, i.productgroup_name, i.productgroup_code,j.productuom_name,j.productuom_code,g.tax_name1, " +
                    " g.tax_name2,g.tax_name3,format(g.discount_amount, 2) as discount_amount, format((g.amount * quantity -if (g.discount_amount is null,'0.00', " +
                    " g.discount_amount)),2) as amount1,g.discount_percentage , e.invoice_refno,e.payment_date ,e.payment_term, m.purchasetype_name " +
                    " from pbl_trn_tserviceorder a  " +
                    " left join acp_mst_tvendor b on a.customer_gid = b.vendor_gid " +
                    " left join crm_trn_tcurrencyexchange c on a.currency_code = c.currency_code " +
                    " left join hrm_mst_tbranch d on a.branch_gid = d.branch_gid" +
                    " left join acp_trn_tinvoice e on a.serviceorder_gid = e.invoice_reference" +
                    " left join acp_mst_ttax f on a.tax_gid = f.tax_gid" +
                    " left join pbl_trn_tserviceorderdtl g on a.serviceorder_gid = g.serviceorder_gid " +
                    " left join pmr_mst_tproduct h on g.product_gid = h.product_gid " +
                    " left join pmr_mst_tproductgroup i on h.productgroup_gid = i.productgroup_gid " +
                    " left join pmr_mst_tproductuom j on h.productuom_gid = j.productuom_gid" +
                    " left join pmr_trn_tpurchaseorder l on b. vendor_gid=a.serviceorder_gid " +
                    " left join  pmr_trn_tpurchasetype m on m. purchasetype_gid=a.serviceorder_gid " +
                    " left join pbl_trn_tserviceorder k on g.serviceorder_gid = k.serviceorder_gid " +
                    " left join pbl_trn_tinvoiceaddsum_V s on b.vendor_companyname = s.vendor_companyname " +
                    " where a.customer_gid = '" + vendor_gid + "'";
                string msgetGID = objcmnfunctions.GetMasterGID("SIVP");
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEditInvList>();
                var allTaxSegmentsList = new List<GetIvTaxSegmentList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditInvList
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            serviceorder_gid = dt["serviceorder_gid"].ToString(),
                            serviceorder_date = dt["serviceorder_date"].ToString(),
                            invoice_refno = msgetGID,
                            payment_date = dt["payment_date"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            purchasetype_name = dt["purchasetype_name"].ToString(),
                            //addon_amount = dt["addon_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            description = dt["description"].ToString(),
                            quantity = dt["quantity"].ToString(),
                            unit_price = dt["unit_price"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount1 = dt["tax_amount1"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_name1 = dt["tax_name1"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString()
                        });
                       
                        string taxsegment_gid = dt["taxsegment_gid"].ToString();
                        string productGid = dt["product_gid"].ToString();
                        string productName = dt["product_name"].ToString();
                        string lsvendor_gid = dt["vendor_gid"].ToString();

                        if (!string.IsNullOrEmpty(taxsegment_gid) && taxsegment_gid != "undefined")
                        {
                            StringBuilder taxSegmentQuery = new StringBuilder("SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                               "e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                               "THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                               "a.cost_price FROM acp_mst_ttaxsegment2product d " +
                               "LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                               "LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid " +
                               "LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE a.product_gid = '");
                            taxSegmentQuery.Append(productGid).Append("' AND f.vendor_gid = '").Append(lsvendor_gid).Append("'");

                            dt_datatable = objdbconn.GetDataTable(taxSegmentQuery.ToString());

                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt1 in dt_datatable.Rows)
                                {
                                    allTaxSegmentsList.Add(new GetIvTaxSegmentList
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
                }
                values.invoiceaddcomfirm_list = getModuleList;            
                values.GetIvTaxSegmentList = allTaxSegmentsList;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGettaxnamedropdown(MdlPmrTrnInvoice values)
        {
            try
            {
                msSQL = " select tax_gid,tax_name,percentage from acp_mst_ttax ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<taxnamedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new taxnamedropdown
                        {
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_percentage = dt["percentage"].ToString()

                        });
                        values.taxnamedropdown = getModuleList;
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


        public void DaGetPmrTrnInvoiceview(string invoice_gid, MdlPmrTrnInvoice values)
        {
            try
            {
                msSQL = " SELECT a.invoice_gid,a.tax_name as tax_name4,k.email_id,a.mode_despatch,a.billing_email,a.shipping_address,a.delivery_term,a.termsandconditions," +
                         " CASE WHEN a.invoice_refno IS NULL OR a.invoice_refno = '' THEN a.invoice_gid ELSE a.invoice_refno END AS invoice_refno, " +
                        " a.invoice_refno,a.invoice_type,concat(m.address1,'\n',m.address2,'\n',m.city,'\n',m.state,'\n',m.postal_code" +
                        " ) as vendor_address,a.vendorinvoiceref_no,k.contact_telephonenumber,a.order_total,a.received_amount,a.received_year,k.contactperson_name," +
                        " j.branch_name,k.vendor_companyname,a.branch_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,i.costcenter_name,i.budget_available, " +
                        " date_format(a.payment_date,'%d-%m-%Y') as payment_date,a.round_off, a.payment_term, a.vendor_gid,format(a.extraadditional_amount, 2) as extraadditional_amount, " +
                        " case when a.currency_code is null then 'INR' else a.currency_code end as currency_code,format(a.extradiscount_amount, 2) as extradiscount_amount, " +
                        " case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,format(e.product_total,2) as price, " +
                        " a.invoice_remarks, a.payment_days,a.reject_reason, a.invoice_status, format(a.invoice_amount,2) as invoice_amount, a.invoice_reference, " +
                        " format(a.freightcharges_amount, 2) as freightcharges_amount, format(a.additionalcharges_amount, 2) as additionalcharges_amount, " +
                        " format(a.discount_amount, 2) as discount_amount, format(a.total_amount, 2) as total_amount,  " +
                        " format(a.freightcharges,2) as freightcharges,format(a.buybackorscrap,2) as buybackorscrap,a.invoice_total,a.raised_amount,a.tax_amount,a.tax_name " +
                        " FROM acp_trn_tinvoice a " +
                        " left join acp_trn_tinvoicedtl e on a.invoice_gid=e.invoice_gid " +
                        " left join acp_trn_tpo2invoice g on a.invoice_gid=g.invoice_gid " +
                        " left join pmr_trn_tpurchaseorder h on g.purchaseorder_gid=h.purchaseorder_gid " +
                        " left join pmr_mst_tcostcenter i on h.costcenter_gid=i.costcenter_gid " +
                        " left join hrm_mst_tbranch j on a.branch_gid=j.branch_gid " +
                        " left join acp_mst_tvendor k on a.vendor_gid=k.vendor_gid " +
                        " left join adm_mst_taddress m on m.address_gid=k.address_gid  " +
                        " where a.invoice_gid = '" + invoice_gid + "' group by a.invoice_gid ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoice_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                       
                        getModuleList.Add(new invoice_lists
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),

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
                            mode_despatch = dt["mode_despatch"].ToString(),
                            tax_name4 = dt["tax_name4"].ToString(),
                            delivery_term = dt["delivery_term"].ToString()
                        });
                        values.invoice_lists = getModuleList;
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
        public void DaGetPmrTrnInvoiceviewproduct(string invoice_gid, MdlPmrTrnInvoice values)
        {
            try
            {
                double grand_total = 0.00;
                msSQL = " SELECT a.product_gid, concat(a.qty_invoice,' ',a.productuom_name) as qty_invoice, c.invoice_amount,format(a.product_price,2) as product_price, " +
                        " format(a.discount_percentage,2) as discount_percentage,c.round_off,c.buybackorscrap,c.freightcharges, " +
                        " a.taxseg_taxname1,a.taxseg_taxpercent1,a.taxseg_taxamount1,a.taxseg_taxname2,a.taxseg_taxpercent2,a.taxseg_taxamount2,a.taxseg_taxname3,a.taxseg_taxpercent3,taxseg_taxamount3, " +
                        " a.tax_name,a.tax_name2,a.tax_name3,CASE  WHEN a.tax_amount2 = 0.00 THEN NULL ELSE FORMAT(a.tax_amount2, 2) END AS tax_amount2,format(a.tax_amount3,2)as tax_amount3," +
                        " format(a.discount_amount,2) as discount_amount, format(a.tax_percentage,2) as tax_percentage, " +
                        " format(a.tax_amount,2) as tax_amount, a.product_remarks,  format((a.qty_invoice * a.product_price),2) as product_totalprice, " +
                        " format(a.excise_percentage,2) as excise_percentage, format(a.excise_amount,2) as excise_amount, " +
                        " format(a.product_total,2) as product_total, b.po2invoice_gid, a.invoice_gid, a.invoicedtl_gid, b.grn_gid, b.grndtl_gid, " +
                        " b.purchaseorder_gid, b.purchaseorderdtl_gid, b.product_gid, b.qty_invoice,c.additionalcharges_amount, " +
                        " a.product_name,a.display_field, a.product_code,d.additional_name,c.extradiscount_amount,c.extraadditional_amount,e.discount_name, " +
                        " a.productgroup_name,c.termsandconditions, a.productuom_name,f.tax_prefix, " +
                        " concat(f.tax_prefix,', ',a.tax_name) as taxnames,concat(a.tax_amount,', ',a.tax_amount2) as taxamts from acp_trn_tinvoicedtl a " +
                        " left join acp_trn_tpo2invoice b on a.invoicedtl_gid = b.invoicedtl_gid " +
                        " left join acp_trn_tinvoice c on a.invoice_gid = c.invoice_gid   " +
                        " left join acp_mst_ttax f on f.tax_gid = a.tax2_gid     " +
                        " left join pmr_trn_tadditional d on c.extraadditional_code = d.additional_gid  " +
                        " left join pmr_trn_tdiscount e on c.extradiscount_code = e.discount_gid   " +
                        " where a.invoice_gid = '" + invoice_gid + "'" +
                        " order by a.product_name ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoiceProduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["product_total"].ToString());
                        getModuleList.Add(new invoiceProduct_list
                        {
                                                      
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field_name = dt["display_field"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            tax_amount1 = dt["tax_amount"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            additional_name = dt["additional_name"].ToString(),
                            extradiscount_amount = dt["extradiscount_amount"].ToString(),
                            extraadditional_amount = dt["extraadditional_amount"].ToString(),
                            discount_name = dt["discount_name"].ToString(),
                            round_off = dt["round_off"].ToString(),
                            addon_amount = dt["additionalcharges_amount"].ToString(),
                            buybackorscrap = dt["buybackorscrap"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            taxseg_taxname1 = dt["taxseg_taxname1"].ToString(),
                            taxseg_taxpercent1 = dt["taxseg_taxpercent1"].ToString(),
                            taxseg_taxamount1 = dt["taxseg_taxamount1"].ToString(),
                            taxseg_taxname2 = dt["taxseg_taxname2"].ToString(),
                            taxseg_taxpercent2 = dt["taxseg_taxpercent2"].ToString(),
                            taxseg_taxamount2 = dt["taxseg_taxamount2"].ToString(),
                            taxseg_taxname3 = dt["taxseg_taxname3"].ToString(),
                            taxseg_taxpercent3 = dt["taxseg_taxpercent3"].ToString(),
                            taxseg_taxamount3 = dt["taxseg_taxamount3"].ToString(),
                            product_description = dt["display_field"].ToString()
                            
                        });
                        values.invoiceProduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.grand_total = grand_total;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void Dapblinvoicesubmit(string employee_gid, pblinvoice_list values)
        {
            try
            {



                string ls_referenceno = objcmnfunctions.GetMasterGID("SIVP");
                string lstype1 = "Services";

                msSQL = " select branch_gid from hrm_mst_tbranch where branch_name='" + values.branch_name + "'";
                string lsbranch_gid = objdbconn.GetExecuteScalar(msSQL);

                if (values.addon_amount == "" || values.addon_amount == null) { values.addon_amount = "0"; }
                if (values.discount_amount == "" || values.discount_amount == null) { values.discount_amount = "0"; }


                msSQL = " insert into acp_trn_tinvoice(" +
                    " invoice_gid," +
                    " invoice_date," +
                    " invoice_reference," +
                    " payment_date," +
                    " payment_term," +
                    " vendor_gid," +
                    " total_amount," +
                    " invoice_amount," +
                    " invoice_refno," +
                    " invoice_status, " +
                    " invoice_flag, " +
                    " user_gid," +
                    " created_date," +
                    " discount_amount," +
                    " additionalcharges_amount," +
                    " total_amount_L," +
                    " discount_amount_L," +
                    " additionalcharges_amount_L," +
                    " invoice_from," +
                    " invoice_type," +
                    " currency_code," +
                    " exchange_rate," +
                    " branch_gid," +
                    " invoice_remarks " +
                    ") values (" +
                     "'" + values.invoice_gid + "'," +
                     "'" + values.invoice_date.ToString("yyyy-MM-dd ") + "'," +
                     "'" + values.serviceorder_gid + "'," +
                     "'" + values.payment_date.ToString("yyyy-MM-dd ") + "'," +
                     "'" + values.payment_term + "'," +
                     "'" + values.vendor_gid + "'," +
                     "'" + values.grand_total + "'," +
                     "'" + values.invoice_amount + "'," +
                     "'" + values.invoice_gid + "'," +
                     "'IV Approved'," +
                     "'Payment Pending'," +
                     "'" + employee_gid + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                     "'" + values.discount_amount + "'," +
                     "'" + values.addon_amount + "'," +
                     "'" + values.grand_total + "'," +
                     "'" + values.discount_amount + "'," +
                     "'" + values.addon_amount + "'," +
                     "'" + values.invoice_from + "'," +
                     "'" + lstype1 + "'," +
                     "'" + values.currency_code + "'," +
                     "'" + values.exchange_rate + "'," +
                     "'" + lsbranch_gid + "'," +
                     "'" + values.invoice_remarks + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " SELECT a.serviceorder_gid,date_format(a.serviceorder_date, '%d/%m/%Y') as serviceorder_date," +
                    " b.vendor_companyname,b.vendor_gid,format(a.grand_total, 2) as grand_total,a.currency_code,sum(e.invoice_amount)" +
                    " as invoice_amount,f.tax_name,a.tax_amount, b.contactperson_name,b.email_id,b.vendor_code,c.exchange_rate," +
                    " format(a.addon_amount, 2) as addon_amount ,format(a.discount_amount, 2) as discount_amount,d.branch_name , " +
                    " g.product_gid, g.serviceorderdtl_gid,g.quantity,g.unit_price,format(g.amount, 2) as amount ,e.invoice_gid, e.invoice_date, e.invoice_remarks, " +
                    " format(g.tax_amount1, 2) as tax_amount1,g.tax1_gid,g.tax2_gid,g.tax3_gid, " + "format(g.tax_amount2, 2) as tax_amount2," +
                    " format(g.tax_amount3, 2) as tax_amount3, format(g.total_amount - g.tax_amount1 - tax_amount2 - tax_amount3, 2) as total_amount," +
                    " g.description, h.product_name, h.product_code, i.productgroup_name, i.productgroup_code,j.productuom_name,j.productuom_code,g.tax_name1, " +
                    " g.tax_name2,g.tax_name3,format(g.discount_amount, 2) as discount_amount, format((g.amount * quantity -if (g.discount_amount is null,'0.00', " +
                    " g.discount_amount)),2) as amount1,g.discount_percentage , e.invoice_refno,e.payment_date ,e.payment_term, m.purchasetype_name " +
                    " from pbl_trn_tserviceorder a  " +
                    " left join acp_mst_tvendor b on a.customer_gid = b.vendor_gid " +
                    " left join crm_trn_tcurrencyexchange c on a.currency_code = c.currency_code " +
                    " left join hrm_mst_tbranch d on a.branch_gid = d.branch_gid" +
                    " left join acp_trn_tinvoice e on a.serviceorder_gid = e.invoice_reference" +
                    " left join acp_mst_ttax f on a.tax_gid = f.tax_gid" +
                    " left join pbl_trn_tserviceorderdtl g on a.serviceorder_gid = g.serviceorder_gid " +
                    " left join pmr_mst_tproduct h on g.product_gid = h.product_gid " +
                    " left join pmr_mst_tproductgroup i on h.productgroup_gid = i.productgroup_gid " +
                    " left join pmr_mst_tproductuom j on h.productuom_gid = j.productuom_gid" +
                    " left join pmr_trn_tpurchaseorder l on b. vendor_gid=a.serviceorder_gid " +
                    " left join  pmr_trn_tpurchasetype m on m. purchasetype_gid=a.serviceorder_gid " +
                    " left join pbl_trn_tserviceorder k on g.serviceorder_gid = k.serviceorder_gid " +
                    " where a.customer_gid = '" + values.vendor_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("SIVC");
                        msSQL = " insert into acp_trn_tinvoicedtl (" +
                            " invoicedtl_gid, " +
                            " invoice_gid, " +
                            " product_gid, " +
                            " product_price, " +
                            " product_total, " +
                            " discount_percentage, " +
                            " discount_amount, " +
                            " tax_name, " +
                            " tax_name2, " +
                            " tax_name3, " +
                            " tax_amount, " +
                            " tax_amount2, " +
                            " tax_amount3, " +
                            " tax1_gid, " +
                            " tax2_gid, " +
                            " tax3_gid, " +
                            " product_remarks, " +
                            " product_price_L," +
                            " discount_amount_L," +
                            " tax_amount1_L," +
                            " tax_amount2_L," +
                            " tax_amount3_L," +
                            " qty_invoice," +
                            " productgroup_code," +
                            " productgroup_name," +
                            " product_code," +
                            " product_name," +
                            " productuom_code," +
                            " productuom_name" +
                            " ) values ( " +
                            "'" + msGetGid + "'," +
                            "'" + values.invoice_gid + "'," +
                            "'" + dt["product_gid"].ToString() + "'," +
                            "'" + dt["unit_price"].ToString().Replace(",", "") + "'," +
                            "'" + dt["total_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["discount_percentage"].ToString() + "'," +
                            "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_name1"].ToString() + "'," +
                            "'" + dt["tax_name2"].ToString() + "'," +
                            "'" + dt["tax_name3"].ToString() + "'," +
                            "'" + dt["tax_amount1"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax1_gid"].ToString() + "'," +
                            "'" + dt["tax2_gid"].ToString() + "'," +
                            "'" + dt["tax3_gid"].ToString() + "'," +
                            "'" + dt["description"].ToString() + "'," +
                            "'" + dt["unit_price"].ToString().Replace(",", "") + "'," +
                            "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount1"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                            "'" + dt["quantity"].ToString().Replace(",", "") + "'," +
                            "'" + dt["productgroup_code"].ToString() + "'," +
                            "'" + dt["productgroup_name"].ToString() + "'," +
                            "'" + dt["product_code"].ToString() + "'," +
                            "'" + dt["product_name"].ToString() + "'," +
                            "'" + dt["productuom_code"].ToString() + "'," +
                            "'" + dt["productuom_name"].ToString() + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Invoice Added Successfully";
                    }
                    else
                    {

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

        public void DaGetPurchaseOrderSummary(MdlPmrTrnPurchaseOrder values)
        {
            try
            {


                msSQL = " select /*+ MAX_EXECUTION_TIME(600000) */ distinct h.costcenter_name,a.purchaseorder_gid, " +
                         " b.vendor_code,concat(b.contactperson_name,' / ',b.email_id,' / ',b.contact_telephonenumber) as Contact,l.req_status as req_status," +
                         " concat(b.vendor_code,'/',b.vendor_companyname) as Vendor," +
                        " if(a.poref_no is null,a.purchaseorder_gid, " +
                        " if(a.poref_no='',a.purchaseorder_gid, " +
                        " if(a.poref_no=a.purchaseorder_gid,a.purchaseorder_gid,concat(a.purchaseorder_gid,'/',a.poref_no)))) " +
                       " as porefno,a.poref_no, " +
                      " a.purchaseorder_remarks,a.purchaseorder_status, format(a.total_amount,2) as total_amount,Date_add(a.purchaseorder_date,Interval delivery_days day) as ExpectedPODeliveryDate ," +
                        " format(a.total_amount,2) as paymentamount, " +
                        " DATE_FORMAT(a.purchaseorder_date , '%d-%m-%Y') as purchaseorder_date ,a.vendor_status," +
                      " CASE when a.invoice_flag <> 'IV Pending' then a.invoice_flag" +
                      " when a.grn_flag <> 'GRN Pending' then a.grn_flag" +
                        " else a.purchaseorder_flag end as 'overall_status'," +
                       " a.purchaseorder_flag, a.grn_flag, a.invoice_flag," +
                " b.vendor_companyname, c.branch_name, " +
                " case when group_concat(distinct purchaserequisition_referencenumber)=',' then '' " +
                " when group_concat(distinct purchaserequisition_referencenumber) <> ',' then group_concat(distinct purchaserequisition_referencenumber) end  as refrence_no, " +
                " bscc_flag,po_type,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,concat(d.user_firstname,' ',d.user_lastname) as created_by from pmr_trn_tpurchaseorder a " +
                " LEFT JOIN pmr_trn_tinvoiceapproval l ON l.purchaseorder_gid = a.purchaseorder_gid" +
                " left join  acp_mst_tvendor b on b.vendor_gid = a.vendor_gid" +
                " left join hrm_mst_tbranch c on c.branch_gid = a.branch_gid" +
                " left join adm_mst_tuser d on d.user_gid = a.created_by" +
                " left join hrm_mst_temployee e on e.user_gid = d.user_gid" +
                " left join adm_mst_tmodule2employee  f on f.employee_gid = e.employee_gid" +
                " left join pmr_mst_tcostcenter h on h.costcenter_gid=a.costcenter_gid" +
                " left join pmr_Trn_tpr2po i on i.purchaseorder_gid=a.purchaseorder_gid" +
                " left join pmr_Trn_tpurchaserequisition j on j.purchaserequisition_gid=i.purchaserequisition_gid " +
                " left join crm_trn_tcurrencyexchange k on k.currencyexchange_gid =a.currency_code" +
                " where 1=1 and a.workorderpo_flag='N'  and a.purchaseorder_status <>'PO Amended' and a.purchaseorder_flag <> 'PO Canceled' and l.req_status = 'Pending' group by a.purchaseorder_gid order by  date(a.purchaseorder_date) desc,a.purchaseorder_date asc, a.purchaseorder_gid desc, b.vendor_companyname desc ";


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

        public void DaGetInvoiceDelete(string invoice_gid, taxnamedropdown values)
        {
            try
            {
                msSQL = "update  acp_trn_tinvoice set " +
                    "invoice_status  = 'IV Canceled'," +
                    "invoice_amount  = '0.00'," +
                    "invoice_flag  = 'Invoice Canceled'" +
                   " WHERE invoice_gid = '" + invoice_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    objfincmn.invoice_cancel(invoice_gid);
                    values.status = true;
                    values.message = "Invoice Cancel Successfully";

                }

                else
                {
                    values.status = false;
                    values.message = "Error While Cancel Invoice !!";

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Payment Invoice !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //invoice report

        public void DaGetpmrInvoiceForLastSixMonths(MdlPmrTrnInvoice values)
        {
            try
            {
                msSQL = " SELECT format( ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2),2) AS invoiceamount," +
                    " ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2) AS invoiceamount1, " +
                        " YEAR(a.invoice_date) AS year, a.invoice_date, SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'),1, 3) AS months,a.invoice_gid," +
                        " COUNT(a.invoice_gid) AS invoicecount,taxtotal.taxtotal, format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate) - COALESCE(taxtotal.taxtotal, 0),2),2) AS netamount " +
                        " FROM acp_trn_tinvoice a" +
                        " LEFT JOIN pmr_trn_tpurchaseorder i ON i.purchaseorder_gid = a.invoice_reference LEFT JOIN (SELECT " +
                        " YEAR(a.invoice_date) AS year, DATE_FORMAT(a.invoice_date, '%M') AS month, format(round(SUM(b.tax_amount1_L + b.tax_amount2_L + b.tax_amount3_L),2),2) AS taxtotal " +
                        " FROM acp_trn_tinvoicedtl b LEFT JOIN acp_trn_tinvoice a ON b.invoice_gid = a.invoice_gid" +
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

                var GetpmrInvoiceForLastSixMonths_List = new List<GetpmrInvoiceForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetpmrInvoiceForLastSixMonths_List.Add(new GetpmrInvoiceForLastSixMonths_List
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
                        values.GetpmrInvoiceForLastSixMonths_List = GetpmrInvoiceForLastSixMonths_List;
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

        public void DaGetpmrInvoiceReportForLastSixMonthsSearch(MdlPmrTrnInvoice values, string from_date, string to_date)
        {
            try
            {
                if (from_date == null && to_date == null)
                {
                    msSQL = " SELECT  format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2),2) AS invoiceamount, " +
                            " ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2) AS invoiceamount1, " +
                            " YEAR(a.invoice_date) AS year, a.invoice_date, SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'),1, 3) AS months,a.invoice_gid," +
                            " COUNT(a.invoice_gid) AS invoicecount,taxtotal.taxtotal, format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate) - COALESCE(taxtotal.taxtotal, 0),2),2) AS netamount " +
                            " FROM acp_trn_tinvoice a LEFT JOIN pmr_trn_tpurchaseorder i ON i.purchaseorder_gid = a.invoice_reference LEFT JOIN (SELECT " +
                            " YEAR(a.invoice_date) AS year, DATE_FORMAT(a.invoice_date, '%M') AS month, format(round(SUM(b.tax_amount1_L + b.tax_amount2_L + b.tax_amount3_L),2),2) AS taxtotal " +
                            " FROM acp_trn_tinvoicedtl b " +
                            " LEFT JOIN acp_trn_tinvoice a ON b.invoice_gid = a.invoice_gid" +
                            " WHERE  a.invoice_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH) " +
                            " AND a.invoice_date <= DATE(NOW())" +
                            " AND a.invoice_status NOT IN ('Invoice Cancelled' , 'Rejected')" +
                            " AND a.invoice_flag NOT IN ('Invoice Cancelled' , 'Rejected')" +
                            " GROUP BY DATE_FORMAT(a.invoice_date, '%M')) taxtotal ON taxtotal.year = YEAR(a.invoice_date)" +
                            " AND taxtotal.month = DATE_FORMAT(a.invoice_date, '%M')" +
                            " WHERE a.invoice_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH) AND a.invoice_date <= DATE(NOW())" +
                            " AND a.invoice_status NOT IN ('Invoice Cancelled' , 'Rejected')" +
                            " AND a.invoice_flag NOT IN ('Invoice Cancelled' , 'Rejected')" +
                            " GROUP BY DATE_FORMAT(a.invoice_date, '%M') ORDER BY a.invoice_date DESC";
                }
                else
                {
                    msSQL = " SELECT  format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2),2) AS invoiceamount, " +
                        " ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate), 2) AS invoiceamount1, " +
                        " YEAR(a.invoice_date) AS year, a.invoice_date, SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'),1, 3) AS months,a.invoice_gid," +
                        " COUNT(a.invoice_gid) AS invoicecount,taxtotal.taxtotal, format(ROUND(SUM((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount) * a.exchange_rate) - COALESCE(taxtotal.taxtotal, 0),2),2) AS netamount " +
                        " FROM acp_trn_tinvoice a" +
                        " LEFT JOIN pmr_trn_tpurchaseorder i ON i.purchaseorder_gid = a.invoice_reference LEFT JOIN (SELECT " +
                        " YEAR(a.invoice_date) AS year, DATE_FORMAT(a.invoice_date, '%M') AS month, format(round(SUM(b.tax_amount1_L + b.tax_amount2_L + b.tax_amount3_L),2),2) AS taxtotal " +
                        " FROM acp_trn_tinvoicedtl b " +
                        "LEFT JOIN acp_trn_tinvoice a ON b.invoice_gid = a.invoice_gid" +
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

                var GetpmrInvoiceForLastSixMonths_List = new List<GetpmrInvoiceForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetpmrInvoiceForLastSixMonths_List.Add(new GetpmrInvoiceForLastSixMonths_List
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
                        values.GetpmrInvoiceForLastSixMonths_List = GetpmrInvoiceForLastSixMonths_List;
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

        public void DaGetpmrInvoiceDetailSummary(string branch_gid, string month, string year, MdlPmrTrnInvoice values)
        {
            try
            {

                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " select distinct a.invoice_gid,a.user_gid, a.invoice_refno,a.invoice_status as status,DATE_FORMAT(a.invoice_date, '%d-%m-%Y') as invoice_date ,  " +
                    " concat(d.user_firstname,' ',d.user_lastname) as name,a.invoice_reference,a.branch_gid,f.branch_name, " +
                    " format(round(((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount)*a.exchange_rate),2),2) as invoiceamount," +
                    " c.contact_telephonenumber, CONCAT(c.vendor_code, '/', c.vendor_companyname) AS vendor,  " +
                    " a.invoice_from,a.currency_code  " +
                    " from acp_trn_tinvoice a  " +
                    " left join pmr_trn_tpurchaseorder i on i.purchaseorder_gid = a.invoice_reference  " +
                    " left join hrm_mst_temployee b on b.employee_gid= a.user_gid  " +
                    " left join adm_mst_tuser d on d.user_gid=b.user_gid " +
                    " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid " +
                    "  left join acp_mst_tvendor c on c.vendor_gid = a.vendor_gid   " +
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                    " where substring(date_format(a.invoice_date,'%M'),1,3)='" + month + "' and year(a.invoice_date)='" + year + "'  " +
                    " and a.invoice_status not in('Invoice Cancelled','Rejected')  " +
                    " and a.invoice_flag not in('Invoice Cancelled','Rejected') ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetpmrInvoiceDetailSummary = new List<GetpmrInvoiceDetailSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetpmrInvoiceDetailSummary.Add(new GetpmrInvoiceDetailSummary
                        {
                            invoice_refno = (dt["invoice_refno"].ToString()),
                            invoice_date = (dt["invoice_date"].ToString()),
                            customer_name = (dt["vendor"].ToString()),
                            contact_details = (dt["contact_telephonenumber"].ToString()),
                            salesinvoice_status = (dt["status"].ToString()),
                            branch_name = (dt["branch_name"].ToString()),
                            created_by = (dt["name"].ToString()),
                            invoiceamount = (dt["invoiceamount"].ToString()),
                            invoice_gid = (dt["invoice_gid"].ToString()),
                        });
                        values.GetpmrInvoiceDetailSummary = GetpmrInvoiceDetailSummary;
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

        //individualreport details
        public void DaGetIndividualreport(MdlPmrTrnInvoice values, string purchaseorder_gid)
        {
            try
            {
                msSQL = "select a.purchaseorder_gid,b.invoice_refno,a.purchaseorder_date,a.purchaseorder_gid," +
                    "a.customer_name,concat(a.customer_contact_person,'/',a.customer_address,'/',a.customer_mobile,'/',a.customer_email)as customer_details," +
                    "b.invoice_date,format((a.grandtotal_l),2) as grand_total, format((b.advance_amount),2) as advance_amount,a.purchaseorder_status,a.purchase_type," +
                    " format((b.total_amount_L),2) as invoice_amount, format((((a.grandtotal_l)-(b.total_amount_L))),2) as pending_invoice_amount,c.branch_name " +
                    " from pmr_trn_tpurchaseorder a " +
                    "left join acp_trn_tinvoice b on  a.purchaseorder_gid=b.invoice_reference " +
                    "left join hrm_mst_tbranch c on  a.branch_gid=c.branch_gid" +
                    " where purchaseorder_gid='" + purchaseorder_gid + "' order by a.purchaseorder_date desc ;";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var pmrindividualreport_list = new List<pmrindividualreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        pmrindividualreport_list.Add(new pmrindividualreport_list
                        {
                            invoice_refno = (dt["invoice_refno"].ToString()),
                            salesorder_date = (dt["purchaseorder_date"].ToString()),
                            so_referenceno1 = (dt["purchaseorder_gid"].ToString()),
                            customer_name = (dt["customer_name"].ToString()),
                            customer_details = (dt["customer_details"].ToString()),
                            branch_name = (dt["branch_name"].ToString()),
                            salesorder_status = (dt["purchaseorder_status"].ToString()),
                            so_type = (dt["purchase_type"].ToString()),
                            invoice_date = (dt["invoice_date"].ToString()),
                            grand_total = (dt["grand_total"].ToString()),
                            advance_amount = (dt["advance_amount"].ToString()),
                            invoice_amount = (dt["invoice_amount"].ToString()),
                            pending_invoice_amount = (dt["pending_invoice_amount"].ToString()),
                        });
                        values.pmrindividualreport_list = pmrindividualreport_list;
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


    }
}


